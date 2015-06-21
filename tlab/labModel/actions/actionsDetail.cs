//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//------------------------------------------------------------------------------
// Rename detail
function LabModel::doRenameDetail( %this, %oldName, %newName ) {
	%action = %this.createAction( ActionRenameDetail, "Rename detail" );
	%action.oldName = %oldName;
	%action.newName = %newName;
	%this.doAction( %action );
}

function ActionRenameDetail::doit( %this ) {
	if ( LabModel.shape.renameDetailLevel( %this.oldName, %this.newName ) ) {
		LabModelPropWindow.update_onDetailRenamed( %this.oldName, %this.newName );
		return true;
	}

	return false;
}

function ActionRenameDetail::undo( %this ) {
	Parent::undo( %this );

	if ( LabModel.shape.renameDetailLevel( %this.newName, %this.oldName ) )
		LabModelPropWindow.update_onDetailRenamed( %this.newName, %this.oldName );
}

//------------------------------------------------------------------------------
// Edit detail size
function LabModel::doEditDetailSize( %this, %oldSize, %newSize ) {
	%action = %this.createAction( ActionEditDetailSize, "Edit detail size" );
	%action.oldSize = %oldSize;
	%action.newSize = %newSize;
	%this.doAction( %action );
}

function ActionEditDetailSize::doit( %this ) {
	%dl = LabModel.shape.setDetailLevelSize( %this.oldSize, %this.newSize );

	if ( %dl != -1 ) {
		LabModelPropWindow.update_onDetailSizeChanged( %this.oldSize, %this.newSize );
		return true;
	}

	return false;
}

function ActionEditDetailSize::undo( %this ) {
	Parent::undo( %this );
	%dl = LabModel.shape.setDetailLevelSize( %this.newSize, %this.oldSize );

	if ( %dl != -1 )
		LabModelPropWindow.update_onDetailSizeChanged( %this.newSize, %this.oldSize );
}

//------------------------------------------------------------------------------
// Edit mesh size
function LabModel::doEditMeshSize( %this, %meshName, %size ) {
	%action = %this.createAction( ActionEditMeshSize, "Edit mesh size" );
	%action.meshName = stripTrailingNumber( %meshName );
	%action.oldSize = getTrailingNumber( %meshName );
	%action.newSize = %size;
	%this.doAction( %action );
}

function ActionEditMeshSize::doit( %this ) {
	if ( LabModel.shape.setMeshSize( %this.meshName SPC %this.oldSize, %this.newSize ) ) {
		LabModelPropWindow.update_onMeshSizeChanged( %this.meshName, %this.oldSize, %this.newSize );
		return true;
	}

	return false;
}

function ActionEditMeshSize::undo( %this ) {
	Parent::undo( %this );

	if ( LabModel.shape.setMeshSize( %this.meshName SPC %this.newSize, %this.oldSize ) )
		LabModelPropWindow.update_onMeshSizeChanged( %this.meshName, %this.oldSize, %this.oldSize );
}

//------------------------------------------------------------------------------
// Edit billboard type
function LabModel::doEditMeshBillboard( %this, %meshName, %type ) {
	%action = %this.createAction( ActionEditMeshBillboard, "Edit mesh billboard" );
	%action.meshName = %meshName;
	%action.oldType = %this.shape.getMeshType( %meshName );
	%action.newType = %type;
	%this.doAction( %action );
}

function ActionEditMeshBillboard::doit( %this ) {
	if ( LabModel.shape.setMeshType( %this.meshName, %this.newType ) ) {
		switch$ ( LabModel.shape.getMeshType( %this.meshName ) ) {
		case "normal":
			LabModelDetails-->bbType.setSelected( 0, false );

		case "billboard":
			LabModelDetails-->bbType.setSelected( 1, false );

		case "billboardzaxis":
			LabModelDetails-->bbType.setSelected( 2, false );
		}

		return true;
	}

	return false;
}

function ActionEditMeshBillboard::undo( %this ) {
	Parent::undo( %this );

	if ( LabModel.shape.setMeshType( %this.meshName, %this.oldType ) ) {
		%id = LabModelDetailTree.getSelectedItem();

		if ( ( %id > 1 ) && ( LabModelDetailTree.getItemText( %id ) $= %this.meshName ) ) {
			switch$ ( LabModel.shape.getMeshType( %this.meshName ) ) {
			case "normal":
				LabModelDetails-->bbType.setSelected( 0, false );

			case "billboard":
				LabModelDetails-->bbType.setSelected( 1, false );

			case "billboardzaxis":
				LabModelDetails-->bbType.setSelected( 2, false );
			}
		}
	}
}

//------------------------------------------------------------------------------
// Remove Detail

function LabModel::doRemoveDetail( %this, %size ) {
	%action = %this.createAction( ActionRemoveDetail, "Remove detail level" );
	%action.size = %size;
	%this.doAction( %action );
}

function ActionRemoveDetail::doit( %this ) {
	%meshList = LabModel.getDetailMeshList( %this.size );

	if ( LabModel.shape.removeDetailLevel( %this.size ) ) {
		%meshCount = getFieldCount( %meshList );

		for ( %i = 0; %i < %meshCount; %i++ )
			LabModelPropWindow.update_onMeshRemoved( getField( %meshList, %i ) );

		return true;
	}

	return false;
}

function ActionRemoveDetail::undo( %this ) {
	Parent::undo( %this );
}

//------------------------------------------------------------------------------
// Add/edit imposter
function LabModel::doEditImposter( %this, %dl, %detailSize, %bbEquatorSteps, %bbPolarSteps,
											  %bbDetailLevel, %bbDimension, %bbIncludePoles, %bbPolarAngle ) {
	%action = %this.createAction( ActionEditImposter, "Edit imposter" );
	%action.oldDL = %dl;

	if ( %action.oldDL != -1 ) {
		%action.oldSize = LabModel.shape.getDetailLevelSize( %dl );
		%action.oldImposter = LabModel.shape.getImposterSettings( %dl );
	}

	%action.newSize = %detailSize;
	%action.newImposter = "1" TAB %bbEquatorSteps TAB %bbPolarSteps TAB %bbDetailLevel TAB
								 %bbDimension TAB %bbIncludePoles TAB %bbPolarAngle;
	%this.doAction( %action );
}

function ActionEditImposter::doit( %this ) {
	// Unpack new imposter settings
	for ( %i = 0; %i < 7; %i++ )
		%val[%i] = getField( %this.newImposter, %i );

	LabModelWaitGui.show( "Generating imposter bitmaps..." );
	// Need to de-highlight the current material, or the imposter will have the
	// highlight effect baked in!
	LabModelMaterials.updateSelectedMaterial( false );
	%dl = LabModel.shape.addImposter( %this.newSize, %val[1], %val[2], %val[3], %val[4], %val[5], %val[6] );
	LabModelWaitGui.hide();
	// Restore highlight effect
	LabModelMaterials.updateSelectedMaterial( LabModelMaterials-->highlightMaterial.getValue() );

	if ( %dl != -1 ) {
		LabModelPreview.refreshShape();
		LabModelPreview.currentDL = %dl;
		LabModelAdvancedWindow-->detailSize.setText( %this.newSize );
		LabModelDetails-->meshSize.setText( %this.newSize );
		LabModelDetails.update_onDetailsChanged();
		return true;
	}

	return false;
}

function ActionEditImposter::undo( %this ) {
	Parent::undo( %this );

	// If this was a new imposter, just remove it. Otherwise restore the old settings
	if ( %this.oldDL < 0 ) {
		if ( LabModel.shape.removeImposter() ) {
			LabModelPreview.refreshShape();
			LabModelPreview.currentDL = 0;
			LabModelDetails.update_onDetailsChanged();
		}
	} else {
		// Unpack old imposter settings
		for ( %i = 0; %i < 7; %i++ )
			%val[%i] = getField( %this.oldImposter, %i );

		LabModelWaitGui.show( "Generating imposter bitmaps..." );
		// Need to de-highlight the current material, or the imposter will have the
		// highlight effect baked in!
		LabModelMaterials.updateSelectedMaterial( false );
		%dl = LabModel.shape.addImposter( %this.oldSize, %val[1], %val[2], %val[3], %val[4], %val[5], %val[6] );
		LabModelWaitGui.hide();
		// Restore highlight effect
		LabModelMaterials.updateSelectedMaterial( LabModelMaterials-->highlightMaterial.getValue() );

		if ( %dl != -1 ) {
			LabModelPreview.refreshShape();
			LabModelPreview.currentDL = %dl;
			LabModelAdvancedWindow-->detailSize.setText( %this.oldSize );
			LabModelDetails-->meshSize.setText( %this.oldSize );
		}
	}
}

//------------------------------------------------------------------------------
// Remove imposter
function LabModel::doRemoveImposter( %this ) {
	%action = %this.createAction( ActionRemoveImposter, "Remove imposter" );
	%dl = LabModel.shape.getImposterDetailLevel();

	if ( %dl != -1 ) {
		%action.oldSize = LabModel.shape.getDetailLevelSize( %dl );
		%action.oldImposter = LabModel.shape.getImposterSettings( %dl );
		%this.doAction( %action );
	}
}

function ActionRemoveImposter::doit( %this ) {
	if ( LabModel.shape.removeImposter() ) {
		LabModelPreview.refreshShape();
		LabModelPreview.currentDL = 0;
		LabModelDetails.update_onDetailsChanged();
		return true;
	}

	return false;
}

function ActionRemoveImposter::undo( %this ) {
	Parent::undo( %this );

	// Unpack the old imposter settings
	for ( %i = 0; %i < 7; %i++ )
		%val[%i] = getField( %this.oldImposter, %i );

	LabModelWaitGui.show( "Generating imposter bitmaps..." );
	%dl = LabModel.shape.addImposter( %this.oldSize, %val[1], %val[2], %val[3], %val[4], %val[5], %val[6] );
	LabModelWaitGui.hide();

	if ( %dl != -1 ) {
		LabModelPreview.refreshShape();
		LabModelPreview.currentDL = %dl;
		LabModelAdvancedWindow-->detailSize.setText( %this.oldSize );
		LabModelDetails-->meshSize.setText( %this.oldSize );
		LabModelDetails.update_onDetailsChanged();
	}
}