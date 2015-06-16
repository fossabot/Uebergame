//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


// Replace the command field in an Editor PopupMenu item (returns the original value)
function ShapeEditorPlugin::replaceMenuCmd(%this, %menuTitle, %id, %newCmd) {
	if (!$Cfg_UseCoreMenubar) return;

	%menu = Lab.findMenu( %menuTitle );
	%cmd = getField( %menu.item[%id], 2 );
	%menu.setItemCommand( %id, %newCmd );
	return %cmd;
}

function ShapeEditorPlugin::onWorldEditorStartup(%this) {
	Parent::onWorldEditorStartup( %this );
	// Add ourselves to the Editor Settings window
	ShapeEdAnimWindow.resize( -1, 526, 593, 53 );
	// Initialise gui
	ShapeEdSeqNodeTabBook.selectPage(0);
	ShapeEdAdvancedWindow-->tabBook.selectPage(0);
	ShapeEdSelectWindow-->tabBook.selectPage(0);
	ShapeEdSelectWindow.navigate("");
	SetToggleButtonValue( ShapeEditorToolbar-->orbitNodeBtn, 0 );
	SetToggleButtonValue( ShapeEditorToolbar-->ghostMode, 0 );
	// Initialise hints menu
	ShapeEdHintMenu.clear();
	%count = ShapeHintGroup.getCount();

	for (%i = 0; %i < %count; %i++) {
		%hint = ShapeHintGroup.getObject(%i);
		ShapeEdHintMenu.add(%hint.objectType, %hint);
	}
}

function ShapeEditorPlugin::open(%this, %filename) {
	if ( !%this.isActivated ) {
		// Activate the Shape Editor
		// Lab.setEditor( %this, true );
		// Get editor settings (note the sun angle is not configured in the settings
		// dialog, so apply the settings here instead of in readSettings)
		ShapeEdPreviewGui.fitIntoParents();
		ShapeEdPreviewGui-->previewBackground.fitIntoParents();
		ShapeEdShapeView.fitIntoParents();
		$wasInWireFrameMode = $gfx::wireframe;
		ShapeEditorToolbar-->wireframeMode.setStateOn($gfx::wireframe);

		if ( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" )
			ShapeEdNodes-->objectTransform.setStateOn(1);
		else
			ShapeEdNodes-->worldTransform.setStateOn(1);

		// Initialise and show the shape editor
		ShapeEdShapeTreeView.open(MissionGroup);
		ShapeEdShapeTreeView.buildVisibleTree(true);
		EditorGui.bringToFront(ShapeEdPreviewGui);
		EWToolsPaletteArray->WorldEditorMove.performClick();
		%this.map.push();
		// Switch to the ShapeEditor UndoManager
		%this.oldUndoMgr = Editor.getUndoManager();
		Editor.setUndoManager( ShapeEdUndoManager );
		ShapeEdShapeView.setDisplayType( Lab.cameraDisplayType );
		%this.initStatusBar();
		// Customise menu bar
		%this.oldCamFitCmd = %this.replaceMenuCmd( "Camera", 8, "ShapeEdShapeView.fitToShape();" );
		%this.oldCamFitOrbitCmd = %this.replaceMenuCmd( "Camera", 9, "ShapeEdShapeView.fitToShape();" );
		Parent::onActivated(%this);
	}

	// Select the new shape
	if (isObject(ShapeEditor.shape) && (ShapeEditor.shape.baseShape $= %filename)) {
		// Shape is already selected => re-highlight the selected material if necessary
		ShapeEdMaterials.updateSelectedMaterial(ShapeEdMaterials-->highlightMaterial.getValue());
	} else if (%filename !$= "") {
		ShapeEditor.selectShape(%filename, ShapeEditor.isDirty());
		// 'fitToShape' only works after the GUI has been rendered, so force a repaint first
		Canvas.repaint();
		ShapeEdShapeView.fitToShape();
	}
}

function ShapeEditorPlugin::onActivated(%this) {
	%this.open("");
	//Assign the Camera fit to the GuiShapeEdPreview
	Lab.fitCameraGui = ShapeEdShapeView;
	// Try to start with the shape selected in the world editor
	%count = EWorldEditor.getSelectionSize();

	for (%i = 0; %i < %count; %i++) {
		%obj = EWorldEditor.getSelectedObject(%i);
		%shapeFile = ShapeEditor.getObjectShapeFile(%obj);

		if (%shapeFile !$= "") {
			if (!isObject(ShapeEditor.shape) || (ShapeEditor.shape.baseShape !$= %shapeFile)) {
				// Call the 'onSelect' method directly if the object is not in the
				// MissionGroup tree (such as a Player or Projectile object).
				ShapeEdShapeTreeView.clearSelection();

				if (!ShapeEdShapeTreeView.selectItem(%obj))
					ShapeEdShapeTreeView.onSelect(%obj);

				// 'fitToShape' only works after the GUI has been rendered, so force a repaint first
				Canvas.repaint();
				ShapeEdShapeView.fitToShape();
			}

			break;
		}
	}
}

function ShapeEditorPlugin::initStatusBar(%this) {
	EditorGuiStatusBar.setInfo("Shape editor ( Shift Click ) to speed up camera.");
	EditorGuiStatusBar.setSelection( ShapeEditor.shape.baseShape );
}

function ShapeEditorPlugin::onDeactivated(%this) {
	// Notify game objects if shape has been modified
	if ( ShapeEditor.isDirty() )
		ShapeEditor.shape.notifyShapeChanged();

	$gfx::wireframe = $wasInWireFrameMode;
	ShapeEdMaterials.updateSelectedMaterial(false);

	if( EditorGui-->MatEdPropertiesWindow.visible ) {
		ShapeEdMaterials.editSelectedMaterialEnd( true );
	}

	%this.map.pop();
	// Restore the original undo manager
	Editor.setUndoManager( %this.oldUndoMgr );
	// Restore menu bar
	%this.replaceMenuCmd( "Camera", 8, %this.oldCamFitCmd );
	%this.replaceMenuCmd( "Camera", 9, %this.oldCamFitOrbitCmd );
	Parent::onDeactivated(%this);
}

function ShapeEditorPlugin::onExitMission( %this ) {
	// unselect the current shape
	ShapeEdShapeView.setModel( "" );

	if (ShapeEditor.shape != -1)
		delObj(ShapeEditor.shape);

	ShapeEditor.shape = 0;
	ShapeEdUndoManager.clearAll();
	ShapeEditor.setDirty( false );
	ShapeEdSequenceList.clear();
	ShapeEdNodeTreeView.removeItem( 0 );
	ShapeEdPropWindow.update_onNodeSelectionChanged( -1 );
	ShapeEdDetailTree.removeItem( 0 );
	ShapeEdMaterialList.clear();
	ShapeEdMountWindow-->mountList.clear();
	ShapeEdThreadWindow-->seqList.clear();
	ShapeEdThreadList.clear();
}

function ShapeEditorPlugin::openShape( %this, %path, %discardChangesToCurrent ) {
//    Lab.setEditor( ShapeEditorPlugin );
	if( ShapeEditor.isDirty() && !%discardChangesToCurrent ) {
		LabMsgYesNo( "Save Changes?",
						 "Save changes to current shape?",
						 "ShapeEditor.saveChanges(); ShapeEditorPlugin.openShape(\"" @ %path @ "\");",
						 "ShapeEditorPlugin.openShape(\"" @ %path @ "\");" );
		return;
	}

	ShapeEditor.selectShape( %path );
	ShapeEdShapeView.fitToShape();
}
function ShapeEditorPlugin::onPreSave( %this ) {
	ShapeEdShapeView.selectedNode = "-1";
	ShapeEdShapeView.selectedObject = "-1";
	ShapeEdShapeView.selectedObjDetail = "-1";
	ShapeEdShapeView.activeThread = "-1";
}

