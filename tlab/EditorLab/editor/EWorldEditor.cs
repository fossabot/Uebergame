//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function EWorldEditor::syncGui( %this ) {
   if(!EditorGui.isAwake()){     
      return;
   }

	%this.syncToolPalette();

	SceneEditorTree.update();
	Editor.getUndoManager().updateUndoMenu( );
	EditorGuiStatusBar.setSelectionObjectsByCount( %this.getSelectionSize() );

	//SceneTreeWindow-->LockSelection.setStateOn( %this.getSelectionLockCount() > 0 );

	SceneEditorToolbar-->boundingBoxColBtn.setStateOn( EWorldEditor.boundingBoxCollision );

	    if( EWorldEditor.objectsUseBoxCenter ) {
	        SceneEditorToolbar-->centerObject.iconBitmap ="tlab/gui/icons/toolbar_assets/SelObjectCenter";
	    } else {
	         SceneEditorToolbar-->centerObject.iconBitmap ="tlab/gui/icons/toolbar_assets/SelObjectBounds";
	    }

	    if( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" ) {
	        SceneEditorToolbar-->objectTransform.iconBitmap ="tlab/gui/icons/toolbar_assets/TransformObject";
	    } else {
	        SceneEditorToolbar-->objectTransform.iconBitmap ="tlab/gui/icons/toolbar_assets/TransformWorld";
	    }
	
	SceneEditorToolbar-->renderHandleBtn.setStateOn( EWorldEditor.renderObjHandle );
	SceneEditorToolbar-->renderTextBtn.setStateOn( EWorldEditor.renderObjText );

	SnapToBar-->objectSnapBtn.setStateOn( EWorldEditor.getSoftSnap() );
	SceneEditorToolbar-->softSnapSizeTextEdit.setText( EWorldEditor.getSoftSnapSize() );
	ESnapOptions-->SnapSize.setText( EWorldEditor.getSoftSnapSize() );
	ESnapOptions-->GridSize.setText( EWorldEditor.getGridSize() );

	ESnapOptions-->GridSnapButton.setStateOn( %this.getGridSnap() );
	SnapToBar-->objectGridSnapBtn.setStateOn( %this.getGridSnap() );
	ESnapOptions-->NoSnapButton.setStateOn( !%this.stickToGround && !%this.getSoftSnap() && !%this.getGridSnap() );
}

function EWorldEditor::syncToolPalette( %this ) {
	switch$ ( GlobalGizmoProfile.mode ) {
	case "None":
		EWorldEditorNoneModeBtn.performClick();
	case "Move":
		EWorldEditorMoveModeBtn.performClick();
	case "Rotate":
		EWorldEditorRotateModeBtn.performClick();
	case "Scale":
		EWorldEditorScaleModeBtn.performClick();
	}
}

function EWorldEditor::addSimGroup( %this, %groupCurrentSelection ) {

	%activeSelection = %this.getActiveSelection();
	if ( %activeSelection.getObjectIndex( MissionGroup ) != -1 ) {
		LabMsgOK( "Error", "Cannot add MissionGroup to a new SimGroup" );
		return;
	}

	// Find our parent.

	%parent = MissionGroup;
	if( !%groupCurrentSelection && isObject( %activeSelection ) && %activeSelection.getCount() > 0 ) {
		%firstSelectedObject = %activeSelection.getObject( 0 );
		if( %firstSelectedObject.isMemberOfClass( "SimGroup" ) )
			%parent = %firstSelectedObject;
		else if( %firstSelectedObject.getId() != MissionGroup.getId() )
			%parent = %firstSelectedObject.parentGroup;
	}

	// If we are about to do a group-selected as well,
	// starting recording an undo compound.

	if( %groupCurrentSelection )
		Editor.getUndoManager().pushCompound( "Group Selected" );

	// Create the SimGroup.

	%object = new SimGroup() {
		parentGroup = %parent;
	};
	MECreateUndoAction::submit( %object );

	// Put selected objects into the group, if requested.

	if( %groupCurrentSelection && isObject( %activeSelection ) ) {
		%undo = UndoActionReparentObjects::create( SceneEditorTree );

		%numObjects = %activeSelection.getCount();
		for( %i = 0; %i < %numObjects; %i ++ ) {
			%sel = %activeSelection.getObject( %i );
			%undo.add( %sel, %sel.parentGroup, %object );
			%object.add( %sel );
		}

		%undo.addToManager( Editor.getUndoManager() );
	}

	// Stop recording for group-selected.

	if( %groupCurrentSelection )
		Editor.getUndoManager().popCompound();

	// When not grouping selection, make the newly created SimGroup the
	// current selection.

	if( !%groupCurrentSelection ) {
		EWorldEditor.clearSelection();
		EWorldEditor.selectObject( %object );
	}

	// Refresh the Gui.

	%this.syncGui();
}

function EWorldEditor::toggleLockChildren( %this, %simGroup ) {
	foreach( %child in %simGroup ) {
		if( %child.isMemberOfClass( "SimGroup" ) )
			%this.toggleLockChildren( %child );
		else
			%child.setLocked( !%child.locked );
	}

	EWorldEditor.syncGui();
}

function EWorldEditor::toggleHideChildren( %this, %simGroup ) {
	foreach( %child in %simGroup ) {
		if( %child.isMemberOfClass( "SimGroup" ) )
			%this.toggleHideChildren( %child );
		else
			%this.hideObject( %child, !%child.hidden );
	}

	EWorldEditor.syncGui();
}

function EWorldEditor::convertSelectionToPolyhedralObjects( %this, %className ) {
	%group = %this.getNewObjectGroup();
	%undoManager = Editor.getUndoManager();

	%activeSelection = %this.getActiveSelection();
	while( %activeSelection.getCount() != 0 ) {
		%oldObject = %activeSelection.getObject( 0 );
		%newObject = %this.createPolyhedralObject( %className, %oldObject );
		if( isObject( %newObject ) ) {
			%undoManager.pushCompound( "Convert ConvexShape to " @ %className );
			%newObject.parentGroup = %oldObject.parentGroup;
			MECreateUndoAction::submit( %newObject );
			MEDeleteUndoAction::submit( %oldObject );
			%undoManager.popCompound();
		}
	}
}

function EWorldEditor::convertSelectionToConvexShape( %this ) {
	%group = %this.getNewObjectGroup();
	%undoManager = Editor.getUndoManager();

	%activeSelection = %this.getActiveSelection();
	while( %activeSelection.getCount() != 0 ) {
		%oldObject = %activeSelection.getObject( 0 );
		%newObject = %this.createConvexShapeFrom( %oldObject );
		if( isObject( %newObject ) ) {
			%undoManager.pushCompound( "Convert " @ %oldObject.getClassName() @ " to ConvexShape" );
			%newObject.parentGroup = %oldObject.parentGroup;
			MECreateUndoAction::submit( %newObject );
			MEDeleteUndoAction::submit( %oldObject );
			%undoManager.popCompound();
		}
	}
}

function EWorldEditor::getNewObjectGroup( %this ) {
	return SceneCreatorWindow.getNewObjectGroup();
}

function EWorldEditor::deleteMissionObject( %this, %object ) {
	// Unselect in editor tree.

	%id = SceneEditorTree.findItemByObjectId( %object );
	SceneEditorTree.selectItem( %id, false );

	// Delete object.

	MEDeleteUndoAction::submit( %object );
	EWorldEditor.isDirty = true;
	SceneEditorTree.buildVisibleTree( true );
}

function EWorldEditor::selectAllObjectsInSet( %this, %set, %deselect ) {
	if( !isObject( %set ) )
		return;

	foreach( %obj in %set ) {
		if( %deselect )
			%this.unselectObject( %obj );
		else
			%this.selectObject( %obj );
	}
}
//-----------------------------------------------------------------------------

function EWorldEditor::getGridSnap( %this ) {
	return %this.gridSnap;
}

function EWorldEditor::setGridSnap( %this, %value ) {
	%this.gridSnap = %value;
	GlobalGizmoProfile.snapToGrid = %value;
	%this.syncGui();
}

function EWorldEditor::getGridSize( %this ) {
	return %this.gridSize;
}

function EWorldEditor::setGridSize( %this, %value ) {
	GlobalGizmoProfile.gridSize = %value SPC %value SPC %value;
	%this.gridSize = %value;

   if (EditorIsActive())
	   %this.syncGui();
}

//-----------------------------------------------------------------------------

function EWorldEditor::areAllSelectedObjectsOfType( %this, %className ) {
	%activeSelection = %this.getActiveSelection();
	if( !isObject( %activeSelection ) )
		return false;

	%count = %activeSelection.getCount();
	for( %i = 0; %i < %count; %i ++ ) {
		%obj = %activeSelection.getObject( %i );
		if( !%obj.isMemberOfClass( %className ) )
			return false;
	}

	return true;
}

//-----------------------------------------------------------------------------
function EWorldEditorToggleCamera::toggleBitmap(%this) {
	%currentImage = %this.bitmap;

	if ( %currentImage $= "tlab/gui/icons/default/toolbar/player" )
		%image = "tlab/gui/icons/default/toolbar/camera";
	else
		%image = "tlab/gui/icons/default/toolbar/player";

	%this.setBitmap( %image );
}

function EWorldEditorCameraSpeed::updateMenuBar(%this, %editorBarCtrl) {
	// Update Toolbar TextEdit
	if( %editorBarCtrl.getId() == CameraSpeedDropdownCtrlContainer-->slider.getId() ) {
		%value = %editorBarCtrl.getValue();
		EWorldEditorCameraSpeed.setText( %value );
		$Camera::movementSpeed = %value;
	}

	// Update Toolbar Slider
	if( %editorBarCtrl.getId() == EWorldEditorCameraSpeed.getId() ) {
		%value = %editorBarCtrl.getText();
		if ( %value !$= "" ) {
			if ( %value <= 0 ) {  // camera speed must be >= 0
				%value = 1;
				%editorBarCtrl.setText( %value );
			}
			CameraSpeedDropdownCtrlContainer-->slider.setValue( %value );
			$Camera::movementSpeed = %value;
		}
	}

	// Update Editor
	EditorCameraSpeedOptions.checkRadioItem(0, 6, -1);
}

//-----------------------------------------------------------------------------

function EWorldEditorAlignPopup::onSelect(%this, %id, %text) {
	if ( GlobalGizmoProfile.mode $= "Scale" && %text $= "World" ) {
		EWorldEditorAlignPopup.setSelected(1);
		return;
	}

	GlobalGizmoProfile.alignment = %text;
}

//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------

function EWorldEditorNoneModeBtn::onClick(%this) {
	GlobalGizmoProfile.mode = "None";

	EditorGuiStatusBar.setInfo("Selection arrow.");
}

function EWorldEditorMoveModeBtn::onClick(%this) {
	GlobalGizmoProfile.mode = "Move";

	%cmdCtrl = "CTRL";
	if( $platform $= "macos" )
		%cmdCtrl = "CMD";

	EditorGuiStatusBar.setInfo( "Move selection.  SHIFT while dragging duplicates objects.  " @ %cmdCtrl @ " to toggle soft snap.  ALT to toggle grid snap." );
}

function EWorldEditorRotateModeBtn::onClick(%this) {
	GlobalGizmoProfile.mode = "Rotate";

	EditorGuiStatusBar.setInfo("Rotate selection.");
}

function EWorldEditorScaleModeBtn::onClick(%this) {
	GlobalGizmoProfile.mode = "Scale";

	EditorGuiStatusBar.setInfo("Scale selection.");
}
