//==============================================================================
// TorqueLab -> Fonts Setup
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function TerrainMatDlgActiveNameEdit::onValidate( %this ) {
	devLog("TerrainMatDlgActiveNameEdit::onValidate",%this.getText());
	TerrainMaterialDlg.setMaterialName(%this.getText());	
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::nameAltCommand( %this, %newName ) {
	devLog("TerrainMaterialDlg::nameAltCommand",%newName);
	
	
}
//------------------------------------------------------------------------------
//==============================================================================
function TMD_MaterialEdit::onValidate( %this ) {	
	devLog("TMD_MaterialEdit onValidate");
	//if (!isObject(%this.activeMat)) return;
	
	//%this.mat.setFieldValue(%this.internalName,%this.getText());
	//EPainter.setMaterialDirty( %this.mat,%this.nameCtrl );
	
	TerrainMaterialDlg.dirtyMat[TerrainMaterialDlg.activeMat] = true;
	TerrainMaterialDlg-->saveMatButton.setActive(true);
	
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainMaterialDlg::setMaterialName( %this, %newName ) {
	%mat = %this.activeMat;

	if( %mat.internalName !$= %newName ) {
		%existingMat = TerrainMaterialSet.findObjectByInternalName( %newName );

		if( isObject( %existingMat ) ) {
			LabMsgOK( "Error",
						 "There already is a terrain material called '" @ %newName @ "'.", "", "" );
		} else {
			%mat.setInternalName( %newName );
			%this-->matLibTree.buildVisibleTree( false );
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::changeBase( %this ) {
	%ctrl = %this-->baseTexCtrl;
	%file = %ctrl.bitmap;
	devLog("changeBase File:",%file);
	if( getSubStr( %file, 0 , 6 ) $= "tlab/" )
		%file = "";

	%file = TerrainMaterialDlg._selectTextureFileDialog( %file );

	if( %file $= "" ) {
		if( %ctrl.bitmap !$= "" )
			%file = %ctrl.bitmap;
		else
			%file = "tlab/materialEditor/assets/unknownImage";
	}

	%file = makeRelativePath( %file, getMainDotCsDir() );
	%ctrl.setBitmap( %file );
	%this-->diffuseMapFile.setText( fileBase(%file) );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::changeDetail( %this ) {
	%ctrl = %this-->detailTexCtrl;
	%file = %ctrl.bitmap;
devLog("changeDetail File:",%file);
	if( getSubStr( %file, 0 , 6 ) $= "tlab/" )
		%file = "";

	%file = TerrainMaterialDlg._selectTextureFileDialog( %file );

	if( %file $= "" ) {
		if( %ctrl.bitmap !$= "" )
			%file = %ctrl.bitmap;
		else
			%file = "tlab/materialEditor/assets/unknownImage";
	}

	%file = makeRelativePath( %file, getMainDotCsDir() );
	%ctrl.setBitmap( %file );
	%this-->detailMapFile.setText( fileBase(%file) );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::changeMacro( %this ) {
	%ctrl = %this-->macroTexCtrl;
	%file = %ctrl.bitmap;
devLog("changeMacro File:",%file);
	if( getSubStr( %file, 0 , 6 ) $= "tlab/" )
		%file = "";

	%file = TerrainMaterialDlg._selectTextureFileDialog( %file );

	if( %file $= "" ) {
		if( %ctrl.bitmap !$= "" )
			%file = %ctrl.bitmap;
		else
			%file = "tlab/materialEditor/assets/unknownImage";
	}

	%file = makeRelativePath( %file, getMainDotCsDir() );
	%ctrl.setBitmap( %file );
	%this-->macroMapFile.setText( fileBase(%file) );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::changeNormal( %this ) {
	%ctrl = %this-->normTexCtrl;
	%file = %ctrl.bitmap;
devLog("changeNormal File:",%file);
	if( getSubStr( %file, 0 , 6 ) $= "tlab/" )
		%file = "";

	%file = TerrainMaterialDlg._selectTextureFileDialog( %file );

	if( %file $= "" ) {
		if( %ctrl.bitmap !$= "" )
			%file = %ctrl.bitmap;
		else
			%file = "tlab/materialEditor/assets/unknownImage";
	}

	%file = makeRelativePath( %file, getMainDotCsDir() );
	%ctrl.setBitmap( %file );
	%this-->normalMapFile.setText( fileBase(%file) );
}
//------------------------------------------------------------------------------

function TerrainMaterialDlg::selectMatId( %this,%matId ) {
	%item = %matLibTree.findItemByObjectId( %matId );
	%matLibTree.selectItem( %item );
}

//==============================================================================
function TerrainMaterialDlg::activateMaterialCtrls( %this, %active ) {
	%parent = %this-->matSettingsParent;
	%count = %parent.getCount();

	for ( %i = 0; %i < %count; %i++ )
		%parent.getObject( %i ).setActive( %active );
}
//------------------------------------------------------------------------------

//==============================================================================
function TerrainMaterialDlg::setActiveMaterial( %this, %mat ) {	

	if (  isObject( %mat ) &&
			%mat.isMemberOfClass( TerrainMaterial ) ) {
		if(  isObject( %mat.matSource ) &&
				%mat.matSource.isMemberOfClass( TerrainMaterial ) ) {
			warnLog("The material loaded is linked to another material:",%mat.matSource.getName());
		}
		%this.canSaveDirty = true;
		%this.activeMat = %mat;				
	
		%this-->matNameCtrl.setText( %mat.internalName );

		if (%mat.diffuseMap $= "") {
			%this-->baseTexCtrl.setBitmap( "tlab/materialEditor/assets/unknownImage" );
		} else {
			%this-->baseTexCtrl.setBitmap( %mat.diffuseMap );
		}

		if (%mat.detailMap $= "") {
			%this-->detailTexCtrl.setBitmap( "tlab/materialEditor/assets/unknownImage" );
		} else {
			%this-->detailTexCtrl.setBitmap( %mat.detailMap );
		}

		if (%mat.macroMap $= "") {
			%this-->macroTexCtrl.setBitmap( "tlab/materialEditor/assets/unknownImage" );
		} else {
			%this-->macroTexCtrl.setBitmap( %mat.macroMap );
		}

		if (%mat.normalMap $= "") {
			%this-->normTexCtrl.setBitmap( "tlab/materialEditor/assets/unknownImage" );
		} else {
			%this-->normTexCtrl.setBitmap( %mat.normalMap );
		}

		%this-->diffuseMapFile.setText( fileBase(%mat.diffuseMap) );
		%this-->detailMapFile.setText(  fileBase(%mat.detailMap) );
		%this-->macroMapFile.setText(  fileBase(%mat.macroMap) );
		%this-->normalMapFile.setText(  fileBase(%mat.normalMap) );
		%this-->detSizeCtrl.setText( %mat.detailSize );
		%this-->baseSizeCtrl.setText( %mat.diffuseSize );
		%this-->detStrengthCtrl.setText( %mat.detailStrength );
		%this-->detDistanceCtrl.setText( %mat.detailDistance );
		%this-->sideProjectionCtrl.setValue( %mat.useSideProjection );
		%this-->parallaxScaleCtrl.setText( %mat.parallaxScale );
		%this-->macroSizeCtrl.setText( %mat.macroSize );
		%this-->macroStrengthCtrl.setText( %mat.macroStrength );
		%this-->macroDistanceCtrl.setText( %mat.macroDistance );
		%this.activateMaterialCtrls( true );
	} else {
		%this.activeMat = 0;
		%this.activateMaterialCtrls( false );
	}
	
	%this.setCloningSource(%this.activeMat);
	%this.updateMaterialMapping(%this.activeMat);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::saveAllDirty( %this ) {
	EPainter.saveDirtyMaterials();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::saveActiveDirty( %this ) {
		%this.saveDirtyMaterial( %this.activeMat );
	// Save all changes.
	ETerrainMaterialPersistMan.saveDirty();

}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::saveDirtyMaterial( %this, %mat ) {
	// Skip over obviously bad cases.
	if (  !isObject( %mat ) ||
			!%mat.isMemberOfClass( TerrainMaterial ) )
		return;

	if (!%this.canSaveDirty){
		warnLog("Save dirty is disabled. Material skipped:",%mat.internalName);
		return;
	}
	/*if(  isObject( %mat.matSource ) )	{
		if (%mat.matSource.isMemberOfClass( TerrainMaterial )){
		LabMsgOk("Can't save linked material","The current material is linked to:" SPC %mat.matSource.getName() SPC ". To avoid linkage issues, it's not possible to save the current material. Please apply the changes you want to the source material!");
		return;
		}
	}*/
	// Read out properties from the dialog.
	%newName = %this-->matNameCtrl.getText();

	if (%this-->baseTexCtrl.bitmap $= "tlab/materialEditor/assets/unknownImage") {
		%newDiffuse = "";
	} else {
		%newDiffuse = %this-->baseTexCtrl.bitmap;
	}

	if (%this-->normTexCtrl.bitmap $= "tlab/materialEditor/assets/unknownImage") {
		%newNormal = "";
	} else {
		%newNormal = %this-->normTexCtrl.bitmap;
	}

	if (%this-->detailTexCtrl.bitmap $= "tlab/materialEditor/assets/unknownImage") {
		%newDetail = "";
	} else {
		%newDetail = %this-->detailTexCtrl.bitmap;
	}

	if (%this-->macroTexCtrl.bitmap $= "tlab/materialEditor/assets/unknownImage") {
		%newMacro = "";
	} else {
		%newMacro = %this-->macroTexCtrl.bitmap;
	}

	%detailSize = %this-->detSizeCtrl.getText();
	%diffuseSize = %this-->baseSizeCtrl.getText();
	%detailStrength = %this-->detStrengthCtrl.getText();
	%detailDistance = %this-->detDistanceCtrl.getText();
	%useSideProjection = %this-->sideProjectionCtrl.getValue();
	%parallaxScale = %this-->parallaxScaleCtrl.getText();
	%macroSize = %this-->macroSizeCtrl.getText();
	%macroStrength = %this-->macroStrengthCtrl.getText();
	%macroDistance = %this-->macroDistanceCtrl.getText();

	// If no properties of this materials have changed,
	// return.

	if (  %mat.internalName $= %newName &&
										%mat.diffuseMap $= %newDiffuse &&
												%mat.normalMap $= %newNormal &&
														%mat.detailMap $= %newDetail &&
																%mat.macroMap $= %newMacro &&
																		%mat.detailSize == %detailSize &&
																		%mat.diffuseSize == %diffuseSize &&
																		%mat.detailStrength == %detailStrength &&
																		%mat.detailDistance == %detailDistance &&
																		%mat.useSideProjection == %useSideProjection &&
																		%mat.macroSize == %macroSize &&
																		%mat.macroStrength == %macroStrength &&
																		%mat.macroDistance == %macroDistance &&
																		%mat.parallaxScale == %parallaxScale ){
		warnLog("No change detected, skipping save for mat:",%mat.internalName);
		return;
		}

	// Make sure the material name is unique.

	if( %mat.internalName !$= %newName ) {
		%existingMat = TerrainMaterialSet.findObjectByInternalName( %newName );

		if( isObject( %existingMat ) ) {
			LabMsgOK( "Error",
						 "There already is a terrain material called '" @ %newName @ "'.", "", "" );
			// Reset the name edit control to the old name.
			%this-->matNameCtrl.setText( %mat.internalName );
		} else
			%mat.setInternalName( %newName );
	}

	%mat.diffuseMap = %newDiffuse;
	%mat.normalMap = %newNormal;
	%mat.detailMap = %newDetail;
	%mat.macroMap = %newMacro;
	%mat.detailSize = %detailSize;
	%mat.diffuseSize = %diffuseSize;
	%mat.detailStrength = %detailStrength;
	%mat.detailDistance = %detailDistance;
	%mat.macroSize = %macroSize;
	%mat.macroStrength = %macroStrength;
	%mat.macroDistance = %macroDistance;
	%mat.useSideProjection = %useSideProjection;
	%mat.parallaxScale = %parallaxScale;
	EPainter.setMaterialDirty(%mat);
}
//------------------------------------------------------------------------------

//==============================================================================
//Call when TerrainMaterialDlg is open and create a copy of all materials
function TerrainMaterialDlg::snapshotMaterials( %this ) {
	if( !isObject( TerrainMaterialDlgSnapshot ) )
		new SimGroup( TerrainMaterialDlgSnapshot );

	%group = TerrainMaterialDlgSnapshot;
	%group.clear();
	%matCount = TerrainMaterialSet.getCount();

	for( %i = 0; %i < %matCount; %i ++ ) {
		%mat = TerrainMaterialSet.getObject( %i );

		if( !isMemberOfClass( %mat.getClassName(), "TerrainMaterial" ) )
			continue;

		%snapshot = new ScriptObject() {
			parentGroup = %group;
			material = %mat;
			internalName = %mat.internalName;
			diffuseMap = %mat.diffuseMap;
			normalMap = %mat.normalMap;
			detailMap = %mat.detailMap;
			macroMap = %mat.macroMap;
			detailSize = %mat.detailSize;
			diffuseSize = %mat.diffuseSize;
			detailStrength = %mat.detailStrength;
			detailDistance = %mat.detailDistance;
			macroSize = %mat.macroSize;
			macroStrength = %mat.macroStrength;
			macroDistance = %mat.macroDistance;
			useSideProjection = %mat.useSideProjection;
			parallaxScale = %mat.parallaxScale;
		};
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Restore the materials to the state of when the TerrainMaterialDlg was open
function TerrainMaterialDlg::restoreMaterials( %this ) {
	if( !isObject( TerrainMaterialDlgSnapshot ) ) {
		error( "TerrainMaterial::restoreMaterials - no snapshot present" );
		return;
	}

	%count = TerrainMaterialDlgSnapshot.getCount();

	for( %i = 0; %i < %count; %i ++ ) {
		%obj = TerrainMaterialDlgSnapshot.getObject( %i );
		%mat = %obj.material;
		%mat.setInternalName( %obj.internalName );
		%mat.diffuseMap = %obj.diffuseMap;
		%mat.normalMap = %obj.normalMap;
		%mat.detailMap = %obj.detailMap;
		%mat.macroMap = %obj.macroMap;
		%mat.detailSize = %obj.detailSize;
		%mat.diffuseSize = %obj.diffuseSize;
		%mat.detailStrength = %obj.detailStrength;
		%mat.detailDistance = %obj.detailDistance;
		%mat.macroSize = %obj.macroSize;
		%mat.macroStrength = %obj.macroStrength;
		%mat.macroDistance = %obj.macroDistance;
		%mat.useSideProjection = %obj.useSideProjection;
		%mat.parallaxScale = %obj.parallaxScale;
	}
}
//------------------------------------------------------------------------------
