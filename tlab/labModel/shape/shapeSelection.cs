//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function LabModel::selectWorldEditorObject( %this ) {
		// Try to start with the shape selected in the world editor
	%count = EWorldEditor.getSelectionSize();

	for (%i = 0; %i < %count; %i++) {
		%obj = EWorldEditor.getSelectedObject(%i);
		%shapeFile = LabModel.getObjectShapeFile(%obj);

		if (%shapeFile !$= "") {
			if (!isObject(LabModel.shape) || (LabModel.shape.baseShape !$= %shapeFile)) {
				// Call the 'onSelect' method directly if the object is not in the
				// MissionGroup tree (such as a Player or Projectile object).
				LabModelShapeTreeView.clearSelection();

				if (!LabModelShapeTreeView.selectItem(%obj))
					LabModelShapeTreeView.onSelect(%obj);

				// 'fitToShape' only works after the GUI has been rendered, so force a repaint first
				Canvas.repaint();
				LabModelPreview.fitToShape();
			}

			break;
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function LabModel::setSelectedObject( %this,%obj ) {
	%path = LabModel.getObjectShapeFile( %obj );	
	LabModel.shape = LabModel.findConstructor( %path );
	LabModel.object = %obj;
}
//------------------------------------------------------------------------------

//==============================================================================
function LabModel::deleteSelection( %this ) {

}
//------------------------------------------------------------------------------
//==============================================================================
// Handle a selection in the MissionGroup shape selector
function LabModelShapeTreeView::onSelect( %this, %obj ) {
	%path = LabModel.getObjectShapeFile( %obj );

	if ( %path !$= "" )
		LabModelSelectWindow.onSelect( %path );

	// Set the object type (for required nodes and sequences display)
	%objClass = %obj.getClassName();
	%hintId = -1;
	%count = ShapeHintGroup.getCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%hint = ShapeHintGroup.getObject( %i );

		if ( %objClass $= %hint.objectType ) {
			%hintId = %hint;
			break;
		} else if ( isMemberOfClass( %objClass, %hint.objectType ) ) {
			%hintId = %hint;
		}
	}	
	
	LabModel.setSelectedObject(%obj);
	LabModelHintMenu.setSelected( %hintId );
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
// Shape Selection
//------------------------------------------------------------------------------

function LabModel::findConstructor( %this, %path ) {
	%count = TSShapeConstructorGroup.getCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%obj = TSShapeConstructorGroup.getObject( %i );

		if ( %obj.baseShape $= %path )
			return %obj;
	}

	return -1;
}

function LabModel::createConstructor( %this, %path ) {
	%name = strcapitalise( fileBase( %path ) ) @ strcapitalise( getSubStr( fileExt( %path ), 1, 3 ) );
	%name = strreplace( %name, "-", "_" );
	%name = strreplace( %name, ".", "_" );
	%name = getUniqueName( %name );
	return new TSShapeConstructor( %name ) {
		baseShape = %path;
	};
}

function LabModel::saveConstructor( %this, %constructor ) {
	%savepath = filePath( %constructor.baseShape ) @ "/" @ fileBase( %constructor.baseShape ) @ ".cs";
	new PersistenceManager( LabModel_perMan );
	LabModel_perMan.setDirty( %constructor, %savepath );
	LabModel_perMan.saveDirtyObject( %constructor );
	LabModel_perMan.delete();
}

// Handle a selection in the shape selector list
function LabModelSelectWindow::onSelect( %this, %path ) {
	// Prompt user to save the old shape if it is dirty
	if ( LabModel.isDirty() ) {
		%cmd = "ColladaImportDlg.showDialog( \"" @ %path @ "\", \"LabModel.selectShape( \\\"" @ %path @ "\\\", ";
		LabMsgYesNoCancel( "Shape Modified", "Would you like to save your changes?", %cmd @ "true );\" );", %cmd @ "false );\" );" );
	} else {
		%cmd = "LabModel.selectShape( \"" @ %path @ "\", false );";
		ColladaImportDlg.showDialog( %path, %cmd );
	}
}

function LabModel::selectShape( %this, %path, %saveOld ) {
	devLog("LabModel::selectShape");
	LabModelPreview.setModel( "" );

	if ( %saveOld ) {
		// Save changes to a TSShapeConstructor script
		%this.saveChanges();
	} else if ( LabModel.isDirty() ) {
		// Purge all unsaved changes
		%oldPath = LabModel.shape.baseShape;
		LabModel.shape.delete();
		LabModel.shape = 0;
		reloadResource( %oldPath );   // Force game objects to reload shape
	}

	// Initialise the shape preview window
	if ( !LabModelPreview.setModel( %path ) ) {
		LabMsgOK( "Error", "Failed to load '" @ %path @ "'. Check the console for error messages." );
		return;
	}

	LabModelPreview.fitToShape();
	LabModelUndoManager.clearAll();
	LabModel.setDirty( false );
	// Get ( or create ) the TSShapeConstructor object for this shape
	LabModel.shape = LabModel.findConstructor( %path );

	if ( LabModel.shape <= 0 ) {

		if ( LabModel.shape <= 0 ) {
			error( "LabModel: Error - could not select " @ %path );
			return;
		}
	}

	// Initialise the editor windows	
	LabModelMountWindow.update_onShapeSelectionChanged();
	
	LabModelColWindow.update_onShapeSelectionChanged();
	LabModelPropWindow.update_onShapeSelectionChanged();
	LabModelPreview.refreshShape();
	// Update object type hints
	LabModelSelectWindow.updateHints();
	// Update editor status bar
	EditorGuiStatusBar.setSelection( %path );
}



function LabModelSelectMenu::onSelect( %this, %id, %text ) {
	%split = strreplace( %text, "/", " " );
	LabModelSelectWindow.navigate( %split );
}

// Update the GUI in response to the shape selection changing
function LabModelPropWindow::update_onShapeSelectionChanged( %this ) {
	// --- NODES TAB ---
	LabModelNodeTreeView.removeItem( 0 );
	%rootId = LabModelNodeTreeView.insertItem( 0, "<root>", 0, "" );
	%count = LabModel.shape.getNodeCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%name = LabModel.shape.getNodeName( %i );

		if ( LabModel.shape.getNodeParentName( %name ) $= "" )
			LabModelNodeTreeView.addNodeTree( %name );
	}

	%this.update_onNodeSelectionChanged( -1 );    // no node selected

	// --- DETAILS TAB ---
	// Add detail levels and meshes to tree
	LabModelDetailTree.clearSelection();
	LabModelDetailTree.removeItem( 0 );
	%root = LabModelDetailTree.insertItem( 0, "<root>", "", "" );
	%objCount = LabModel.shape.getObjectCount();

	for ( %i = 0; %i < %objCount; %i++ ) {
		%objName = LabModel.shape.getObjectName( %i );
		%meshCount = LabModel.shape.getMeshCount( %objName );

		for ( %j = 0; %j < %meshCount; %j++ ) {
			%meshName = LabModel.shape.getMeshName( %objName, %j );
			LabModelDetailTree.addMeshEntry( %meshName, 1 );
		}
	}

	// Initialise object node list
	LabModelDetails-->objectNode.clear();
	LabModelDetails-->objectNode.add( "<root>" );
	%nodeCount = LabModel.shape.getNodeCount();

	for ( %i = 0; %i < %nodeCount; %i++ )
		LabModelDetails-->objectNode.add( LabModel.shape.getNodeName( %i ) );

	// --- MATERIALS TAB ---
	LabModelMaterials.updateMaterialList();
}
