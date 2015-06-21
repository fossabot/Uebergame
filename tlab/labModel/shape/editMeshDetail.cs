//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
// Detail/Mesh Editing
//------------------------------------------------------------------------------

function LabModelDetails::onWake( %this ) {
	// Initialise popup menus
	%this-->bbType.clear();
	%this-->bbType.add( "None", 0 );
	%this-->bbType.add( "Billboard", 1 );
	%this-->bbType.add( "Z Billboard", 2 );
	%this-->addGeomTo.clear();
	%this-->addGeomTo.add( "current detail", 0 );
	%this-->addGeomTo.add( "new detail", 1 );
	%this-->addGeomTo.setSelected( 0, false );
	LabModelDetailTree.onDefineIcons();
}

function LabModelDetailTree::onDefineIcons(%this) {
	// Set the tree view icon indices and texture paths
	%this._imageNone = 0;
	%this._imageHidden = 1;
	%icons = ":" @                                        // no icon
				"tlab/gui/icons/default/visible_i:";               // hidden
	%this.buildIconTable( %icons );
}

// Return true if the item in the details tree view is a detail level (false if
// a mesh)
function LabModelDetailTree::isDetailItem( %this, %id ) {
	return ( %this.getParent( %id ) == 1 );
}

// Get the detail level index from the ID of an item in the details tree view
function LabModelDetailTree::getDetailLevelFromItem( %this, %id ) {
	if ( %this.isDetailItem( %id ) )
		%detSize = %this.getItemValue( %id );
	else
		%detSize = %this.getItemValue( %this.getParent( %id ) );

	return LabModel.shape.getDetailLevelIndex( %detSize );
}

function LabModelDetailTree::addMeshEntry( %this, %name, %noSync ) {
	// Add new detail level if required
	%size = getTrailingNumber( %name );
	%detailID = %this.findItemByValue( %size );

	if ( %detailID <= 0 ) {
		%dl = LabModel.shape.getDetailLevelIndex( %size );
		%detName = LabModel.shape.getDetailLevelName( %dl );
		%detailID = LabModelDetailTree.insertItem( 1, %detName, %size, "" );

		// Sort details by decreasing size
		for ( %sibling = LabModelDetailTree.getPrevSibling( %detailID );
				( %sibling > 0 ) && ( LabModelDetailTree.getItemValue( %sibling ) < %size );
				%sibling = LabModelDetailTree.getPrevSibling( %detailID ) )
			LabModelDetailTree.moveItemUp( %detailID );

		if ( !%noSync )
			LabModelDetails.update_onDetailsChanged();
	}

	return %this.insertItem( %detailID, %name, "", "" );
}

function LabModelDetailTree::removeMeshEntry( %this, %name, %size ) {
	%size = getTrailingNumber( %name );
	%id = LabModelDetailTree.findItemByName( %name );

	if ( LabModel.shape.getDetailLevelIndex( %size ) < 0 ) {
		// Last mesh of a detail level has been removed => remove the detail level
		%this.removeItem( %this.getParent( %id ) );
		LabModelDetails.update_onDetailsChanged();
	} else
		%this.removeItem( %id );
}

function LabModelAdvancedWindow::update_onShapeSelectionChanged( %this ) {
	LabModelPreview.currentDL = 0;
	LabModelPreview.onDetailChanged();
}

function LabModelPropWindow::update_onDetailRenamed( %this, %oldName, %newName ) {
	// --- DETAILS TAB ---
	// Rename detail entry
	%id = LabModelDetailTree.findItemByName( %oldName );

	if ( %id > 0 ) {
		%size = LabModelDetailTree.getItemValue( %id );
		LabModelDetailTree.editItem( %id, %newName, %size );

		// Sync text if item is selected
		if ( LabModelDetailTree.isItemSelected( %id ) &&
				( LabModelDetails-->meshName.getText() !$= %newName ) )
			LabModelDetails-->meshName.setText( stripTrailingNumber( %newName ) );
	}
}

function LabModelPropWindow::update_onDetailSizeChanged( %this, %oldSize, %newSize ) {
	// --- MISC ---
	LabModelPreview.refreshShape();
	%dl = LabModel.shape.getDetailLevelIndex( %newSize );

	if ( LabModelAdvancedWindow-->detailSize.getText() $= %oldSize ) {
		LabModelPreview.currentDL = %dl;
		LabModelAdvancedWindow-->detailSize.setText( %newSize );
		LabModelDetails-->meshSize.setText( %newSize );
	}

	// --- DETAILS TAB ---
	// Update detail entry then resort details by size
	%id = LabModelDetailTree.findItemByValue( %oldSize );
	%detName = LabModel.shape.getDetailLevelName( %dl );
	LabModelDetailTree.editItem( %id, %detName, %newSize );

	for ( %sibling = LabModelDetailTree.getPrevSibling( %id );
			( %sibling > 0 ) && ( LabModelDetailTree.getItemValue( %sibling ) < %newSize );
			%sibling = LabModelDetailTree.getPrevSibling( %id ) )
		LabModelDetailTree.moveItemUp( %id );

	for ( %sibling = LabModelDetailTree.getNextSibling( %id );
			( %sibling > 0 ) && ( LabModelDetailTree.getItemValue( %sibling ) > %newSize );
			%sibling = LabModelDetailTree.getNextSibling( %id ) )
		LabModelDetailTree.moveItemDown( %id );

	// Update size values for meshes of this detail
	for ( %child = LabModelDetailTree.getChild( %id );
			%child > 0;
			%child = LabModelDetailTree.getNextSibling( %child ) ) {
		%meshName = stripTrailingNumber( LabModelDetailTree.getItemText( %child ) );
		LabModelDetailTree.editItem( %child, %meshName SPC %newSize, "" );
	}
}

function LabModelDetails::update_onDetailsChanged( %this ) {
	%detailCount = LabModel.shape.getDetailLevelCount();
	LabModelAdvancedWindow-->detailSlider.range = "0" SPC ( %detailCount-1 );

	if ( %detailCount >= 2 )
		LabModelAdvancedWindow-->detailSlider.ticks = %detailCount - 2;
	else
		LabModelAdvancedWindow-->detailSlider.ticks = 0;

	// Initialise imposter settings
	LabModelAdvancedWindow-->bbUseImposters.setValue( LabModel.shape.getImposterDetailLevel() != -1 );

	// Update detail parameters
	if ( LabModelPreview.currentDL < %detailCount ) {
		%settings = LabModel.shape.getImposterSettings( LabModelPreview.currentDL );
		%isImposter = getWord( %settings, 0 );
		LabModelAdvancedWindow-->imposterInactive.setVisible( !%isImposter );
		LabModelAdvancedWindow-->bbEquatorSteps.setText( getField( %settings, 1 ) );
		LabModelAdvancedWindow-->bbPolarSteps.setText( getField( %settings, 2 ) );
		LabModelAdvancedWindow-->bbDetailLevel.setText( getField( %settings, 3 ) );
		LabModelAdvancedWindow-->bbDimension.setText( getField( %settings, 4 ) );
		LabModelAdvancedWindow-->bbIncludePoles.setValue( getField( %settings, 5 ) );
		LabModelAdvancedWindow-->bbPolarAngle.setText( getField( %settings, 6 ) );
	}
}

function LabModelPropWindow::update_onObjectNodeChanged( %this, %objName ) {
	// --- MISC ---
	LabModelPreview.refreshShape();

	// --- DETAILS TAB ---
	// Update the node popup menu if this object is selected
	if ( LabModelDetails-->meshName.getText() $= %objName ) {
		%nodeName = LabModel.shape.getObjectNode( %objName );

		if ( %nodeName $= "" )
			%nodeName = "<root>";

		%id = LabModelDetails-->objectNode.findText( %nodeName );
		LabModelDetails-->objectNode.setSelected( %id, false );
	}
}

function LabModelPropWindow::update_onObjectRenamed( %this, %oldName, %newName ) {
	// --- DETAILS TAB ---
	// Rename tree entries for this object
	%count = LabModel.shape.getMeshCount( %newName );

	for ( %i = 0; %i < %count; %i++ ) {
		%size = getTrailingNumber( LabModel.shape.getMeshName( %newName, %i ) );
		%id = LabModelDetailTree.findItemByName( %oldName SPC %size );

		if ( %id > 0 ) {
			LabModelDetailTree.editItem( %id, %newName SPC %size, "" );

			// Sync text if item is selected
			if ( LabModelDetailTree.isItemSelected( %id ) &&
					( LabModelDetails-->meshName.getText() !$= %newName ) )
				LabModelDetails-->meshName.setText( %newName );
		}
	}
}

function LabModelPropWindow::update_onMeshAdded( %this, %meshName ) {
	// --- MISC ---
	LabModelPreview.refreshShape();
	LabModelPreview.updateNodeTransforms();

	// --- COLLISION WINDOW ---
	// Add object to target list if it does not already exist
	if ( !LabModel.isCollisionMesh( %meshName ) ) {
		%objName = stripTrailingNumber( %meshName );
		%id = LabModelColWindow-->colTarget.findText( %objName );

		if ( %id == -1 )
			LabModelColWindow-->colTarget.add( %objName );
	}

	// --- DETAILS TAB ---
	%id = LabModelDetailTree.addMeshEntry( %meshName );
	LabModelDetailTree.clearSelection();
	LabModelDetailTree.selectItem( %id );
}

function LabModelPropWindow::update_onMeshSizeChanged( %this, %meshName, %oldSize, %newSize ) {
	// --- MISC ---
	LabModelPreview.refreshShape();
	// --- DETAILS TAB ---
	// Move the mesh to the new location in the tree
	%selected = LabModelDetailTree.getSelectedItem();
	%id = LabModelDetailTree.findItemByName( %meshName SPC %oldSize );
	LabModelDetailTree.removeMeshEntry( %meshName SPC %oldSize );
	%newId = LabModelDetailTree.addMeshEntry( %meshName SPC %newSize );

	// Re-select the new entry if it was selected
	if ( %selected == %id ) {
		LabModelDetailTree.clearSelection();
		LabModelDetailTree.selectItem( %newId );
	}
}

function LabModelPropWindow::update_onMeshRemoved( %this, %meshName ) {
	// --- MISC ---
	LabModelPreview.refreshShape();
	// --- COLLISION WINDOW ---
	// Remove object from target list if it no longer exists
	%objName = stripTrailingNumber( %meshName );

	if ( LabModel.shape.getObjectIndex( %objName ) == -1 ) {
		%id = LabModelColWindow-->colTarget.findText( %objName );

		if ( %id != -1 )
			LabModelColWindow-->colTarget.clearEntry( %id );
	}

	// --- DETAILS TAB ---
	// Determine which item to select next
	%id = LabModelDetailTree.findItemByName( %meshName );

	if ( %id > 0 ) {
		%nextId = LabModelDetailTree.getPrevSibling( %id );

		if ( %nextId <= 0 ) {
			%nextId = LabModelDetailTree.getNextSibling( %id );

			if ( %nextId <= 0 )
				%nextId = 2;
		}

		// Remove the entry from the tree
		%meshSize = getTrailingNumber( %meshName );
		LabModelDetailTree.removeMeshEntry( %meshName, %meshSize );

		// Change selection if needed
		if ( LabModelDetailTree.getSelectedItem() == -1 )
			LabModelDetailTree.selectItem( %nextId );
	}
}

function LabModelDetailTree::onSelect( %this, %id ) {
	%name = %this.getItemText( %id );
	%baseName = stripTrailingNumber( %name );
	%size = getTrailingNumber( %name );
	LabModelDetails-->meshName.setText( %baseName );
	LabModelDetails-->meshSize.setText( %size );
	// Select the appropriate detail level
	%dl = %this.getDetailLevelFromItem( %id );
	LabModelPreview.currentDL = %dl;

	if ( %this.isDetailItem( %id ) ) {
		// Selected a detail => disable mesh controls
		LabModelDetails-->editMeshInactive.setVisible( true );
		LabModelPreview.selectedObject = -1;
		LabModelPreview.selectedObjDetail = 0;
	} else {
		// Selected a mesh => sync mesh controls
		LabModelDetails-->editMeshInactive.setVisible( false );

		switch$ ( LabModel.shape.getMeshType( %name ) ) {
		case "normal":
			LabModelDetails-->bbType.setSelected( 0, false );

		case "billboard":
			LabModelDetails-->bbType.setSelected( 1, false );

		case "billboardzaxis":
			LabModelDetails-->bbType.setSelected( 2, false );
		}

		%node = LabModel.shape.getObjectNode( %baseName );

		if ( %node $= "" )
			%node = "<root>";

		LabModelDetails-->objectNode.setSelected( LabModelDetails-->objectNode.findText( %node ), false );
		LabModelPreview.selectedObject = LabModel.shape.getObjectIndex( %baseName );
		LabModelPreview.selectedObjDetail = %dl;
	}
}

function LabModelDetailTree::onRightMouseUp( %this, %itemId, %mouse ) {
	// Open context menu if this is a Mesh item
	if ( !%this.isDetailItem( %itemId ) ) {
		if( !isObject( "LabModelMeshPopup" ) ) {
			new PopupMenu( LabModelMeshPopup ) {
				superClass = "MenuBuilder";
				isPopup = "1";
				item[ 0 ] = "Hidden" TAB "" TAB "LabModelDetailTree.onHideMeshItem( %this._objName, !%this._itemHidden );";
				item[ 1 ] = "-";
				item[ 2 ] = "Hide all" TAB "" TAB "LabModelDetailTree.onHideMeshItem( \"\", true );";
				item[ 3 ] = "Show all" TAB "" TAB "LabModelDetailTree.onHideMeshItem( \"\", false );";
			};
		}

		LabModelMeshPopup._objName = stripTrailingNumber( %this.getItemText( %itemId ) );
		LabModelMeshPopup._itemHidden = LabModelPreview.getMeshHidden( LabModelMeshPopup._objName );
		LabModelMeshPopup.checkItem( 0, LabModelMeshPopup._itemHidden );
		LabModelMeshPopup.showPopup( Canvas );
	}
}

function LabModelDetailTree::onHideMeshItem( %this, %objName, %hide ) {
	if ( %hide )
		%imageId = %this._imageHidden;
	else
		%imageId = %this._imageNone;

	if ( %objName $= "" ) {
		// Show/hide all
		LabModelPreview.setAllMeshesHidden( %hide );

		for ( %parent = %this.getChild(%this.getFirstRootItem()); %parent > 0; %parent = %this.getNextSibling(%parent) )
			for ( %child = %this.getChild(%parent); %child > 0; %child = %this.getNextSibling(%child) )
				%this.setItemImages( %child, %imageId, %imageId );
	} else {
		// Show/hide all meshes for this object
		LabModelPreview.setMeshHidden( %objName, %hide );
		%count = LabModel.shape.getMeshCount( %objName );

		for ( %i = 0; %i < %count; %i++ ) {
			%meshName = LabModel.shape.getMeshName( %objName, %i );
			%id = LabModelDetailTree.findItemByName( %meshName );

			if ( %id > 0 )
				%this.setItemImages( %id, %imageId, %imageId );
		}
	}
}

function LabModelPreview::onDetailChanged( %this ) {
	// Update slider
	if ( mRound( LabModelAdvancedWindow-->detailSlider.getValue() ) != %this.currentDL )
		LabModelAdvancedWindow-->detailSlider.setValue( %this.currentDL );

	LabModelAdvancedWindow-->detailSize.setText( %this.detailSize );
	LabModelDetails.update_onDetailsChanged();
	%id = LabModelDetailTree.getSelectedItem();

	if ( ( %id <= 0 ) || ( %this.currentDL != LabModelDetailTree.getDetailLevelFromItem( %id ) ) ) {
		%id = LabModelDetailTree.findItemByValue( %this.detailSize );

		if ( %id > 0 ) {
			LabModelDetailTree.clearSelection();
			LabModelDetailTree.selectItem( %id );
		}
	}
}

function LabModelAdvancedWindow::onEditDetailSize( %this ) {
	// Change the size of the current detail level
	%oldSize = LabModel.shape.getDetailLevelSize( LabModelPreview.currentDL );
	%detailSize = %this-->detailSize.getText();
	LabModel.doEditDetailSize( %oldSize, %detailSize );
}

function LabModelDetails::onEditName( %this ) {
	%newName = %this-->meshName.getText();
	// Check if we are renaming a detail or a mesh
	%id = LabModelDetailTree.getSelectedItem();
	%oldName = LabModelDetailTree.getItemText( %id );

	if ( LabModelDetailTree.isDetailItem( %id ) ) {
		// Rename the selected detail level
		%oldSize = getTrailingNumber( %oldName );
		LabModel.doRenameDetail( %oldName, %newName @ %oldSize );
	} else {
		// Rename the selected mesh
		LabModel.doRenameObject( stripTrailingNumber( %oldName ), %newName );
	}
}

function LabModelDetails::onEditSize( %this ) {
	%newSize = %this-->meshSize.getText();
	// Check if we are changing the size for a detail or a mesh
	%id = LabModelDetailTree.getSelectedItem();

	if ( LabModelDetailTree.isDetailItem( %id ) ) {
		// Change the size of the selected detail level
		%oldSize = LabModelDetailTree.getItemValue( %id );
		LabModel.doEditDetailSize( %oldSize, %newSize );
	} else {
		// Change the size of the selected mesh
		%meshName = LabModelDetailTree.getItemText( %id );
		LabModel.doEditMeshSize( %meshName, %newSize );
	}
}

function LabModelDetails::onEditBBType( %this ) {
	// This command is only valid for meshes (not details)
	%id = LabModelDetailTree.getSelectedItem();

	if ( !LabModelDetailTree.isDetailItem( %id ) ) {
		%meshName = LabModelDetailTree.getItemText( %id );
		%bbType = LabModelDetails-->bbType.getText();

		switch$ ( %bbType ) {
		case "None":
			%bbType = "normal";

		case "Billboard":
			%bbType = "billboard";

		case "Z Billboard":
			%bbType = "billboardzaxis";
		}

		LabModel.doEditMeshBillboard( %meshName, %bbType );
	}
}

function LabModelDetails::onSetObjectNode( %this ) {
	// This command is only valid for meshes (not details)
	%id = LabModelDetailTree.getSelectedItem();

	if ( !LabModelDetailTree.isDetailItem( %id ) ) {
		%meshName = LabModelDetailTree.getItemText( %id );
		%objName = stripTrailingNumber( %meshName );
		%node = %this-->objectNode.getText();

		if ( %node $= "<root>" )
			%node = "";

		LabModel.doSetObjectNode( %objName, %node );
	}
}

function LabModelDetails::onAddMeshFromFile( %this, %path ) {
	if ( %path $= "" ) {
		getLoadFilename( "DTS Files|*.dts|COLLADA Files|*.dae|Google Earth Files|*.kmz", %this @ ".onAddMeshFromFile", %this.lastPath );
		return;
	}

	%path = makeRelativePath( %path, getMainDotCSDir() );
	%this.lastPath = %path;

	// Determine the detail level to use for the new geometry
	if ( %this-->addGeomTo.getText() $= "current detail" ) {
		%size = LabModel.shape.getDetailLevelSize( LabModelPreview.currentDL );
	} else {
		// Check if the file has an LODXXX hint at the end of it
		%base = fileBase( %path );
		%pos = strstr( %base, "_LOD" );

		if ( %pos > 0 )
			%size = getSubStr( %base, %pos + 4, strlen( %base ) ) + 0;
		else
			%size = 2;

		// Make sure size is not in use
		while ( LabModel.shape.getDetailLevelIndex( %size ) != -1 )
			%size++;
	}

	LabModel.doAddMeshFromFile( %path, %size );
}

function LabModelDetails::onDeleteMesh( %this ) {
	%id = LabModelDetailTree.getSelectedItem();

	if ( LabModelDetailTree.isDetailItem( %id ) ) {
		%detSize = LabModelDetailTree.getItemValue( %id );
		LabModel.doRemoveShapeData( "Detail", %detSize );
	} else {
		%name = LabModelDetailTree.getItemText( %id );
		LabModel.doRemoveShapeData( "Mesh", %name );
	}
}

function LabModelDetails::onToggleImposter( %this, %useImposter ) {
	%hasImposterDetail = ( LabModel.shape.getImposterDetailLevel() != -1 );

	if ( %useImposter == %hasImposterDetail )
		return;

	if ( %useImposter ) {
		// Determine an unused detail size
		for ( %detailSize = 0; %detailSize < 50; %detailSize++ ) {
			if ( LabModel.shape.getDetailLevelIndex( %detailSize ) == -1 )
				break;
		}

		// Set some initial values for the imposter
		%bbEquatorSteps = 6;
		%bbPolarSteps = 0;
		%bbDetailLevel = 0;
		%bbDimension = 128;
		%bbIncludePoles = 0;
		%bbPolarAngle = 0;
		// Add a new imposter detail level to the shape
		LabModel.doEditImposter( -1, %detailSize, %bbEquatorSteps, %bbPolarSteps,
										 %bbDetailLevel, %bbDimension, %bbIncludePoles, %bbPolarAngle );
	} else {
		// Remove the imposter detail level
		LabModel.doRemoveImposter();
	}
}

function LabModelDetails::onEditImposter( %this ) {
	// Modify the parameters of the current imposter detail level
	%detailSize = LabModel.shape.getDetailLevelSize( LabModelPreview.currentDL );
	%bbDimension = LabModelAdvancedWindow-->bbDimension.getText();
	%bbDetailLevel = LabModelAdvancedWindow-->bbDetailLevel.getText();
	%bbEquatorSteps = LabModelAdvancedWindow-->bbEquatorSteps.getText();
	%bbIncludePoles = LabModelAdvancedWindow-->bbIncludePoles.getValue();
	%bbPolarSteps = LabModelAdvancedWindow-->bbPolarSteps.getText();
	%bbPolarAngle = LabModelAdvancedWindow-->bbPolarAngle.getText();
	LabModel.doEditImposter( LabModelPreview.currentDL, %detailSize,
									 %bbEquatorSteps, %bbPolarSteps, %bbDetailLevel, %bbDimension,
									 %bbIncludePoles, %bbPolarAngle );
}


function LabModel::autoAddDetails( %this, %dest ) {
	// Sets of LOD files are named like:
	//
	// MyShape_LOD200.dae
	// MyShape_LOD64.dae
	// MyShape_LOD2.dae
	//
	// Determine the base name of the input file (MyShape_LOD in the example above)
	// and use that to find any other shapes in the set.
	%base = fileBase( %dest.baseShape );
	%pos = strstr( %base, "_LOD" );

	if ( %pos < 0 ) {
		echo( "Not an LOD shape file" );
		return;
	}

	%base = getSubStr( %base, 0, %pos + 4 );
	echo( "Base is: " @ %base );
	%filePatterns = filePath( %dest.baseShape ) @ "/" @ %base @ "*" @ fileExt( %dest.baseShape );
	echo( "Pattern is: " @ %filePatterns );
	%fullPath = findFirstFileMultiExpr( %filePatterns );

	while ( %fullPath !$= "" ) {
		%fullPath = makeRelativePath( %fullPath, getMainDotCSDir() );

		if ( %fullPath !$= %dest.baseShape ) {
			echo( "Found LOD shape file: " @ %fullPath );
			// Determine the detail size ( number after the base name ), then add the
			// new mesh
			%size = strreplace( fileBase( %fullPath ), %base, "" );
			LabModel.addLODFromFile( %dest, %fullPath, %size, 0 );
		}

		%fullPath = findNextFileMultiExpr( %filePatterns );
	}

	if ( %this.shape == %dest ) {
		LabModelPreview.refreshShape();
		LabModelDetails.update_onDetailsChanged();
	}
}

function LabModel::addLODFromFile( %this, %dest, %filename, %size, %allowUnmatched ) {
	// Get (or create) a TSShapeConstructor object for the source shape. Need to
	// exec the script manually as the resource may not have been loaded yet
	%csPath = filePath( %filename ) @ "/" @ fileBase( %filename ) @ ".cs";

	if ( isFile( %csPath ) )
		exec( %csPath );

	%source = LabModel.findConstructor( %filename );

	if ( %source == -1 )
		%source = LabModel.createConstructor( %filename );

	%source.lodType = "SingleSize";
	%source.singleDetailSize = %size;
	// Create a temporary TSStatic to ensure the resource is loaded
	%temp = new TSStatic() {
		shapeName = %filename;
		collisionType = "None";
	};
	%meshList = "";

	if ( isObject( %temp ) ) {
		// Add a new mesh for each object in the source shape
		%objCount = %source.getObjectCount();

		for ( %i = 0; %i < %objCount; %i++ ) {
			%objName = %source.getObjectName( %i );
			echo( "Checking for object " @ %objName );

			if ( %allowUnmatched || ( %dest.getObjectIndex( %objName ) != -1 ) ) {
				// Add the source object's highest LOD mesh to the destination shape
				echo( "Adding detail size" SPC %size SPC "for object" SPC %objName );
				%srcName = %source.getMeshName( %objName, 0 );
				%destName = %objName SPC %size;
				%dest.addMesh( %destName, %filename, %srcName );
				%meshList = %meshList TAB %destName;
			}
		}

		%temp.delete();
	}

	return trim( %meshList );
}
