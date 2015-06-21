//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


// Replace the command field in an Editor PopupMenu item (returns the original value)
function LabModelPlugin::replaceMenuCmd(%this, %menuTitle, %id, %newCmd) {
	if (!$Cfg_UseCoreMenubar) return;

	%menu = Lab.findMenu( %menuTitle );
	%cmd = getField( %menu.item[%id], 2 );
	%menu.setItemCommand( %id, %newCmd );
	return %cmd;
}

function LabModelPlugin::onWorldEditorStartup(%this) {
	Parent::onWorldEditorStartup( %this );
	$LabModelSelectedShape = newScriptObject("LabModelSelectedShape");
	$LabModelSelectedDetail = newScriptObject("LabModelSelectedDetail");
	// Add ourselves to the Editor Settings window
	
	// Initialise gui
	LabModelTabBook.selectPage(0);
	LabModelSelectWindow-->tabBook.selectPage(0);
	LabModelSelectWindow.navigate("");
	SetToggleButtonValue( LabModelToolbar-->orbitNodeBtn, 0 );
	SetToggleButtonValue( LabModelToolbar-->ghostMode, 0 );
	// Initialise hints menu
	LabModelHintMenu.clear();
	%count = ShapeHintGroup.getCount();

	for (%i = 0; %i < %count; %i++) {
		%hint = ShapeHintGroup.getObject(%i);
		LabModelHintMenu.add(%hint.objectType, %hint);
	}
}

function LabModelPlugin::open(%this, %filename) {
	if ( !%this.isActivated ) {
		// Activate the Shape Editor
		// Lab.setEditor( %this, true );
		// Get editor settings (note the sun angle is not configured in the settings
		// dialog, so apply the settings here instead of in readSettings)
		LabModelPreviewGui.fitIntoParents();
		LabModelPreview-->previewBackground.fitIntoParents();
		LabModelPreview.fitIntoParents();
		$wasInWireFrameMode = $gfx::wireframe;
		LabModelToolbar-->wireframeMode.setStateOn($gfx::wireframe);

		if ( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" )
			LabModelNodes-->objectTransform.setStateOn(1);
		else
			LabModelNodes-->worldTransform.setStateOn(1);

		// Initialise and show the shape editor
		LabModelShapeTreeView.open(MissionGroup);
		LabModelShapeTreeView.buildVisibleTree(true);
		EditorGui.bringToFront(LabModelPreviewGui);
		EWToolsPaletteArray->WorldEditorMove.performClick();
		%this.map.push();
		// Switch to the LabModel UndoManager
		%this.oldUndoMgr = Editor.getUndoManager();
		Editor.setUndoManager( LabModelUndoManager );
		LabModelPreview.setDisplayType( Lab.cameraDisplayType );
		%this.initStatusBar();
		// Customise menu bar
		%this.oldCamFitCmd = %this.replaceMenuCmd( "Camera", 8, "LabModelPreview.fitToShape();" );
		%this.oldCamFitOrbitCmd = %this.replaceMenuCmd( "Camera", 9, "LabModelPreview.fitToShape();" );
		Parent::onActivated(%this);
	}

	// Select the new shape
	if (isObject(LabModel.shape) && (LabModel.shape.baseShape $= %filename)) {
		// Shape is already selected => re-highlight the selected material if necessary
		LabModelMaterials.updateSelectedMaterial(LabModelMaterials-->highlightMaterial.getValue());
	} else if (%filename !$= "") {
		LabModel.selectShape(%filename, LabModel.isDirty());
		// 'fitToShape' only works after the GUI has been rendered, so force a repaint first
		Canvas.repaint();
		LabModelPreview.fitToShape();
	}
}

function LabModelPlugin::onActivated(%this) {
	%this.open("");
	//Assign the Camera fit to the GuiLabModelPreview
	Lab.fitCameraGui = LabModelPreview;
	LabModel.selectWorldEditorObject();
}

function LabModelPlugin::initStatusBar(%this) {
	EditorGuiStatusBar.setInfo("Shape editor ( Shift Click ) to speed up camera.");
	EditorGuiStatusBar.setSelection( LabModel.shape.baseShape );
}

function LabModelPlugin::onDeactivated(%this) {
	// Notify game objects if shape has been modified
	if ( LabModel.isDirty() )
		LabModel.shape.notifyShapeChanged();

	$gfx::wireframe = $wasInWireFrameMode;
	LabModelMaterials.updateSelectedMaterial(false);

	if( EditorGui-->MatEdPropertiesWindow.visible ) {
		LabModelMaterials.editSelectedMaterialEnd( true );
	}

	%this.map.pop();
	// Restore the original undo manager
	Editor.setUndoManager( %this.oldUndoMgr );
	// Restore menu bar
	%this.replaceMenuCmd( "Camera", 8, %this.oldCamFitCmd );
	%this.replaceMenuCmd( "Camera", 9, %this.oldCamFitOrbitCmd );
	Parent::onDeactivated(%this);
}

function LabModelPlugin::onExitMission( %this ) {
	// unselect the current shape
	LabModelPreview.setModel( "" );

	if (LabModel.shape != -1)
		delObj(LabModel.shape);

	LabModel.shape = 0;
	LabModelUndoManager.clearAll();
	LabModel.setDirty( false );

	LabModelNodeTreeView.removeItem( 0 );
	LabModelPropWindow.update_onNodeSelectionChanged( -1 );
	LabModelDetailTree.removeItem( 0 );
	LabModelMaterialList.clear();
	LabModelMountWindow-->mountList.clear();

}

function LabModelPlugin::openShape( %this, %path, %discardChangesToCurrent ) {
//    Lab.setEditor( LabModelPlugin );
	if( LabModel.isDirty() && !%discardChangesToCurrent ) {
		LabMsgYesNo( "Save Changes?",
						 "Save changes to current shape?",
						 "LabModel.saveChanges(); LabModelPlugin.openShape(\"" @ %path @ "\");",
						 "LabModelPlugin.openShape(\"" @ %path @ "\");" );
		return;
	}

	LabModel.selectShape( %path );
	LabModelPreview.fitToShape();
}
function LabModelPlugin::onPreSave( %this ) {
	LabModelPreview.selectedNode = "-1";
	LabModelPreview.selectedObject = "-1";
	LabModelPreview.selectedObjDetail = "-1";
	LabModelPreview.activeThread = "-1";
}

