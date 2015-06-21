//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$FEP_ElementProperties = "scaleMin scaleMax scaleExponent elevationMin elevationMax sinkMin sinkMax sinkRadius slopeMin slopeMax rotationRange probability ForestItemData";
function FEP_Manager::updateBrushProperties( %this,%item ) {
	%selectedItem = ForestManagerBrushTree.getSelectedItem();
	%selectedObject = ForestManagerBrushTree.getSelectedObject();
	%selCount = ForestManagerBrushTree.getSelectedItemsCount();
	devLog("Item=",%item,"SelectedItem = ",%selectedItem,"SelectedObj=",%selectedObject);
	FEP_Manager.selectedBrushName = %item.internalName;
	%this.selectedObject = %item;

	if (%item.getClassName() $= "SimGroup")
		%selType = "Group";
	else if (%item.getClassName() $= "ForestBrush")
		%selType = "Brush";
	else if (%item.getClassName() $= "ForestBrushElement")
		%selType = "Element";

	foreach(%cont in FEP_ManagerBrushProperties) {
		if (%cont.internalName $= %selType)
			%cont.visible = true;
		else
			%cont.visible = false;
	}

	switch$(%selType) {
	case "Group":
	case "Brush":
	case "Element":
		devLog("Setting element",%item);

		foreach$(%field in $FEP_ElementProperties)
			eval("FEP_ManagerBrushProperties-->"@%field@".setText(%item."@%field@");");
	}
}
//==============================================================================
function FEP_ManagerElementEdit::onValidate( %this ) {
	%element = FEP_Manager.selectedObject;

	if (%element.getClassName() !$= "ForestBrushElement")
		return;

	%value = %this.getValue();
	eval("%element."@%this.internalName@" = %value;");
	FEP_Manager.setDirty();
}
//==============================================================================
// Brush Tree Functions
//==============================================================================

//==============================================================================
function FEP_Manager::newBrushGroup( %this ) {
	%internalName = getUniqueInternalName( "Group", ForestBrushGroup, true );
	//Add new group to the closest SimGroup if none selected, add to root
	%selected = ForestManagerBrushTree.getSelectedObject();

	if (isObject(%selected)) {
		if (%selected.getClassName() $= "SimGroup")
			%addTo = %selected;
		else if (%selected.parentGroup.getClassName() $= "SimGroup")
			%addTo = %selected.parentGroup;
		//Extra check in case an element inside a brush is selected
		else if (%selected.parentGroup.parentGroup.getClassName() $= "SimGroup")
			%addTo = %selected.parentGroup.parentGroup;
	}

	if (!isObject(%addTo))
		%addTo = ForestBrushGroup;

	%internalName = getUniqueInternalName( %addTo.internalName@"_", ForestBrushGroup, true );
	%brush = new SimGroup() {
		internalName = %internalName;
		parentGroup = %addTo;
	};
	MECreateUndoAction::submit( %brush );
	ForestManagerBrushTree.open( ForestBrushGroup );
	ForestManagerBrushTree.buildVisibleTree(true);
	%item = ForestManagerBrushTree.findItemByObjectId( %brush );
	ForestManagerBrushTree.clearSelection();
	ForestManagerBrushTree.addSelection( %item );
	ForestManagerBrushTree.scrollVisible( %item );
	ForestEditorPlugin.dirty = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::newBrush( %this ) {
	%internalName = getUniqueInternalName( "Brush", ForestBrushGroup, true );
	%selected = ForestManagerBrushTree.getSelectedObject();

	if (%selected.getClassName() $= "SimGroup")
		%addTo = %selected;
	else if (%selected.parentGroup.getClassName() $= "SimGroup")
		%addTo = %selected.parentGroup;

	%brush = new ForestBrush() {
		internalName = %internalName;
		parentGroup = %parent;
	};

	if (isObject(%addTo))
		%addTo.add(%brush);

	devLog("Brush:",%brush,"Parent",%brush.parentGroup);
	MECreateUndoAction::submit( %brush );
	ForestManagerBrushTree.open( ForestBrushGroup );
	ForestManagerBrushTree.buildVisibleTree(true);
	%item = ForestManagerBrushTree.findItemByObjectId( %brush );
	ForestManagerBrushTree.clearSelection();
	ForestManagerBrushTree.addSelection( %item );
	ForestManagerBrushTree.scrollVisible( %item );
	ForestEditorPlugin.dirty = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::newBrushElement( %this ) {
	%sel = ForestManagerBrushTree.getSelectedObject();

	if ( !isObject( %sel ) )
		%parentGroup = ForestBrushGroup;
	else {
		if ( %sel.getClassName() $= "ForestBrushElement" )
			%parentGroup = %sel.parentGroup;
		else
			%parentGroup = %sel;
	}

	%internalName = getUniqueInternalName( "Element", ForestBrushGroup, true );
	%element = new ForestBrushElement() {
		internalName = %internalName;
		parentGroup =  %parentGroup;
	};
	MECreateUndoAction::submit( %element );
	ForestManagerBrushTree.clearSelection();
	ForestManagerBrushTree.buildVisibleTree( true );
	%item = ForestManagerBrushTree.findItemByObjectId( %element.getId() );
	ForestManagerBrushTree.scrollVisible( %item );
	ForestManagerBrushTree.addSelection( %item );
	ForestEditorPlugin.dirty = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::deleteSelectedBrush( %this ) {
	%selItemList = ForestManagerBrushTree.getSelectedItemsList();
	%selObjList = ForestManagerBrushTree.getSelectedObjectList();
	devLog("Selected ITEMS:",%selItemList);
	devLog("Selected OBJECTS:",%selObjList);
}
//------------------------------------------------------------------------------
//==============================================================================
// Brush Tree Callbacks
//==============================================================================
//==============================================================================
function ForestManagerBrushTree::onAddGroupSelected( %this,%simGroup ) {
	devLog("ForestManagerBrushTree::onAddGroupSelected( %this,%simGroup )",%this,%simGroup );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onAddMultipleSelectionBegin( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onAddMultipleSelectionBegin( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onAddMultipleSelectionEnd( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onAddMultipleSelectionEnd( %this,%arg1 )",%this,%arg1 );
	FEP_Manager.updateBrushProperties();
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onBeginReparenting( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onBeginReparenting( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onClearSelection( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onClearSelection( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onDeleteObject( %this,%obj ) {
	devLog("ForestManagerBrushTree::onDeleteObject( %this,%obj )",%this,%obj );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onDeleteSelection( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onDeleteSelection( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onDragDropped( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onDragDropped( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onEndReparenting( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onEndReparenting( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onInspect( %this,%item ) {
	devLog("ForestManagerBrushTree::onInspect( %this,%item )",%this,%item );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onKeyDown( %this,%modifier,%keyCode ) {
	devLog("ForestManagerBrushTree::onKeyDown( %this,%modifier,%keyCode )",%this,%modifier,%keyCode );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onMouseDragged( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onMouseDragged( %this,%arg1 )",%this,%arg1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onObjectDeleteCompleted( %this,%arg1 ) {
	devLog("ForestManagerBrushTree::onObjectDeleteCompleted( %this,%arg1 )",%this,%arg1 );
	ForestDataManager.setDirty(ForestBrushGroup);
	ForestDataManager.dirty = true;
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onRemoveSelection( %this,%item ) {
	devLog("ForestManagerBrushTree::onRemoveSelection( %this,%item )",%this,%item );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onReparent( %this,%item,%oldParent,%newParent ) {
	devLog("ForestManagerBrushTree::onReparent( %this,%item,%oldParent,%newParent )",%this,%item,%oldParent,%newParent );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onRightMouseDown( %this,%itemId,%mousePos,%obj ) {
	devLog("ForestManagerBrushTree::onRightMouseDown( %this,%itemId,%mousePos,%obj )",%this,%itemId,%mousePos,%obj );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onRightMouseUp( %this,%itemId,%mousePos,%obj ) {
	devLog("ForestManagerBrushTree::onRightMouseUp( %this,%itemId,%mousePos,%obj )",%this,%itemId,%mousePos,%obj );
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onSelect( %this,%item ) {
	devLog("ForestManagerBrushTree::onSelect( %this,%item )",%this,%item );
	FEP_Manager.updateBrushProperties(%item);
}
//------------------------------------------------------------------------------
//==============================================================================
function ForestManagerBrushTree::onUnselect( %this,%item ) {
	devLog("ForestManagerBrushTree::onUnselect( %this,%item )",%this,%item );
	FEP_Manager.updateBrushProperties();
}
//------------------------------------------------------------------------------
/*

void 	onInspect (int itemOrObjectId)
void 	onKeyDown (int modifier, int keyCode)
void 	onMouseDragged ()
void 	onMouseUp (int hitItemId, int mouseClickCount)
void 	onObjectDeleteCompleted ()
void 	onRemoveSelection (int itemOrObjectId)
void 	onReparent (int itemOrObjectId, int oldParentItemOrObjectId, int newParentItemOrObjectId)
void 	onRightMouseDown (int itemId, Point2I mousePos, SimObject object)
void 	onRightMouseUp (int itemId, Point2I mousePos, SimObject object)
void 	onSelect (int itemOrObjectId)
void 	onUnselect (int itemOrObjectId)

	onAddGroupSelected (SimGroup group)
void 	onAddMultipleSelectionBegin ()
void 	onAddMultipleSelectionEnd ()
void 	onAddSelection (int itemOrObjectId, bool isLastSelection)
void 	onBeginReparenting ()
void 	onClearSelection ()
void 	onDefineIcons ()
bool 	onDeleteObject (SimObject object)
void 	onDeleteSelection ()
void 	onDragDropped ()
void 	onEndReparenting ()
*/