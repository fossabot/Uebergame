//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//------------------------------------------------------------------------------
function WorldEditor::onEndDrag( %this, %obj ) {
   
	SceneInspector.inspect( %obj );
	SceneInspector.apply();
	if ( EWorldEditor.getSelectedObject(0).startDrag $= "")
	   return;
   
	%endPos = EWorldEditor.getSelectedObject(0).getPosition();
   %startPos = EWorldEditor.getSelectedObject(0).startDrag;
   EWorldEditor.getSelectedObject(0).startDrag = ""; 
   
	%xMove = %endPos.x - %startPos.x;
	%yMove = %endPos.y - %startPos.y;
	%zMove = %endPos.z - %startPos.z;

	ECloneDrag.copyOffset = %xMove SPC %yMove SPC %zMove;
	//ECloneDrag.copySet = %dragSet;
	ECloneDrag.showDialog();	

}
function WorldEditor::onDragCopySet( %this, %set ) {  
   //%set.getObject(0).startDrag = %set.getObject(0).getPosition();
   
}
function WorldEditor::onDragCopy( %this, %obj ) {  
    %obj.startDrag = %obj.getPosition();
}


function WorldEditor::onSelect( %this, %obj ) {

	%continue = Lab.onSelectObject(%obj);
	if (!%continue) return;
//Check to tell that the selection is called from a group
	%obj.byGroup = false;
	SceneEditorTree.addSelection( %obj );
	
	//Store the source Location of Object 0 in case we drag copy
	

	_setShadowVizLight( %obj );

	//SceneInspector.inspect( %obj );

	if ( isObject( %obj ) && %obj.isMethod( "onEditorSelect" ) )
		%obj.onEditorSelect( %this.getSelectionSize() );

	Lab.currentEditor.onObjectSelected( %obj );

	// Inform the camera
	commandToServer('EditorOrbitCameraSelectChange', %this.getSelectionSize(), %this.getSelectionCentroid());

	EditorGuiStatusBar.setSelectionObjectsByCount(%this.getSelectionSize());

	// Update the materialEditorList
	$Lab::materialEditorList = %obj.getId();

	// Used to help the Material Editor( the M.E doesn't utilize its own TS control )
	// so this dirty extension is used to fake it
	if ( MaterialEditorPreviewWindow.isVisible() )
		MaterialEditorGui.prepareActiveObject();

	// Update the Transform Selection window
	ETransformSelection.onSelectionChanged();
	
	

}
function WorldEditor::on3DMouseEnter( %this, %obj ) {

}

function WorldEditor::on3DMouseLeave( %this, %obj ) {

}
function WorldEditor::onMouseDragged( %this, %obj ) {

}



function WorldEditor::on3DRightMouseUp( %this, %obj ) {

}

function WorldEditor::on3DRightMouseDown( %this, %obj ) {

}

function WorldEditor::on3DMouseUp( %this, %obj ) {

}
function WorldEditor::on3DMouseDown( %this, %obj ) {

}

function WorldEditor::onMultiSelect( %this, %set ) {
	// This is called when completing a drag selection ( on3DMouseUp )
	// so we can avoid calling onSelect for every object. We can only
	// do most of this stuff, like inspecting, on one object at a time anyway.

	%count = %set.getCount();
	%i = 0;

	foreach( %obj in %set ) {
		if ( %obj.isMethod( "onEditorSelect" ) )
			%obj.onEditorSelect( %count );

		%i ++;
		SceneEditorTree.addSelection( %obj, %i == %count );
		Lab.currentEditor.onObjectSelected( %obj );
	}

	// Inform the camera
	commandToServer( 'EditorOrbitCameraSelectChange', %count, %this.getSelectionCentroid() );

	EditorGuiStatusBar.setSelectionObjectsByCount( EWorldEditor.getSelectionSize() );

	// Update the Transform Selection window, if it is
	// visible.

	if( ETransformSelection.isVisible() )
		ETransformSelection.onSelectionChanged();
}

function WorldEditor::onUnSelect( %this, %obj ) {
	if ( isObject( %obj ) && %obj.isMethod( "onEditorUnselect" ) )
		%obj.onEditorUnselect();

   %continue = Lab.onUnSelectObject(%obj);
   
   if (!%continue)
      return;

	Lab.currentEditor.onObjectDeselected( %obj );

	SceneInspector.removeInspect( %obj );
	SceneEditorTree.removeSelection(%obj);

	// Inform the camera
	commandToServer('EditorOrbitCameraSelectChange', %this.getSelectionSize(), %this.getSelectionCentroid());

	EditorGuiStatusBar.setSelectionObjectsByCount(%this.getSelectionSize());

	// Update the Transform Selection window
	ETransformSelection.onSelectionChanged();
}

function WorldEditor::onClearSelection( %this ) {
	Lab.currentEditor.onSelectionCleared();

	SceneEditorTree.clearSelection();

	// Inform the camera
	commandToServer('EditorOrbitCameraSelectChange', %this.getSelectionSize(), %this.getSelectionCentroid());

	EditorGuiStatusBar.setSelectionObjectsByCount(%this.getSelectionSize());

	// Update the Transform Selection window
	ETransformSelection.onSelectionChanged();
}

function WorldEditor::onSelectionCentroidChanged( %this ) {
	// Inform the camera
	commandToServer('EditorOrbitCameraSelectChange', %this.getSelectionSize(), %this.getSelectionCentroid());

	// Refresh inspector.
	SceneInspector.refresh();
}

//////////////////////////////////////////////////////////////////////////

function WorldEditor::init(%this) {
	// add objclasses which we do not want to collide with
	%this.ignoreObjClass(Sky);

	// editing modes
	WEditorPlugin.numEditModes = 3;
	WEditorPlugin.editMode[0]    = "move";
	WEditorPlugin.editMode[1]    = "rotate";
	WEditorPlugin.editMode[2]    = "scale";

	// context menu
	new GuiControl(WEContextPopupDlg, EditorGuiGroup) {
		profile = "ToolsGuiModelessDialogProfile";
		horizSizing = "width";
		vertSizing = "height";
		position = "0 0";
		extent = "640 480";
		minExtent = "8 8";
		visible = "1";
		setFirstResponder = "0";
		modal = "1";

		new GuiPopUpMenuCtrl(WEContextPopup) {
			profile = "ToolsScrollProfile";
			position = "0 0";
			extent = "0 0";
			minExtent = "0 0";
			maxPopupHeight = "200";
			command = "canvas.popDialog(WEContextPopupDlg);";
		};
	};
	WEContextPopup.setVisible(false);

	// Make sure we have an active selection set.
	if( !%this.getActiveSelection() )
		%this.setActiveSelection( new WorldEditorSelection( EWorldEditorSelection ) );
}

//------------------------------------------------------------------------------

function WorldEditor::onDblClick(%this, %obj) {
	// Commented out because making someone double click to do this is stupid
	// and has the possibility of moving hte object

	//SceneInspector.inspect(%obj);
	//InspectorNameEdit.setValue(%obj.getName());
}

function WorldEditor::onClick( %this, %obj ) {
	SceneInspector.inspect( %obj );
}



//------------------------------------------------------------------------------

function WorldEditor::export(%this) {
	getSaveFilename("~/editor/*.mac|mac file", %this @ ".doExport", "selection.mac");
}

function WorldEditor::doExport(%this, %file) {
	missionGroup.save("~/editor/" @ %file, true);
}

function WorldEditor::import(%this) {
	getLoadFilename("~/editor/*.mac|mac file", %this @ ".doImport");
}

function WorldEditor::doImport(%this, %file) {
	exec("~/editor/" @ %file);
}

function WorldEditor::onGuiUpdate(%this, %text) {
}

function WorldEditor::getSelectionLockCount(%this) {
	%ret = 0;
	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);
		if(%obj.locked)
			%ret++;
	}
	return %ret;
}

function WorldEditor::getSelectionHiddenCount(%this) {
	%ret = 0;
	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);
		if(%obj.hidden)
			%ret++;
	}
	return %ret;
}

function WorldEditor::dropCameraToSelection(%this) {
	if(%this.getSelectionSize() == 0)
		return;

	%pos = %this.getSelectionCentroid();
	%cam = LocalClientConnection.camera.getTransform();

	// set the pnt
	%cam = setWord(%cam, 0, getWord(%pos, 0));
	%cam = setWord(%cam, 1, getWord(%pos, 1));
	%cam = setWord(%cam, 2, getWord(%pos, 2));

	LocalClientConnection.camera.setTransform(%cam);
}

/// Pastes the selection at the same place (used to move obj from a group to another)
function WorldEditor::moveSelectionInPlace(%this) {
	%saveDropType = %this.dropType;
	%this.dropType = "atCentroid";
	%this.copySelection();
	%this.deleteSelection();
	%this.pasteSelection();
	%this.dropType = %saveDropType;
}

function WorldEditor::addSelectionToAddGroup(%this) {
	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);
		$InstantGroup.add(%obj);
	}
}

// resets the scale and rotation on the selection set
function WorldEditor::resetTransforms(%this) {
	%this.addUndoState();

	for(%i = 0; %i < %this.getSelectionSize(); %i++) {
		%obj = %this.getSelectedObject(%i);
		%transform = %obj.getTransform();

		%transform = setWord(%transform, 3, "0");
		%transform = setWord(%transform, 4, "0");
		%transform = setWord(%transform, 5, "1");
		%transform = setWord(%transform, 6, "0");

		//
		%obj.setTransform(%transform);
		%obj.setScale("1 1 1");
	}
}


function WorldEditorToolbarDlg::init(%this) {
	WorldEditorInspectorCheckBox.setValue( WorldEditorToolFrameSet.isMember( "EditorToolInspectorGui" ) );
	WorldEditorMissionAreaCheckBox.setValue( WorldEditorToolFrameSet.isMember( "EditorToolMissionAreaGui" ) );
	WorldEditorTreeCheckBox.setValue( WorldEditorToolFrameSet.isMember( "EditorToolTreeViewGui" ) );
	WorldEditorCreatorCheckBox.setValue( WorldEditorToolFrameSet.isMember( "EditorToolCreatorGui" ) );
}

function WorldEditor::onAddSelected(%this,%obj) {
	SceneEditorTree.addSelection(%obj);
}

function WorldEditor::onWorldEditorUndo( %this ) {
	SceneInspector.refresh();
}