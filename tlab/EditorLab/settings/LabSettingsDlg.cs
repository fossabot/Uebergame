//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


function LabSettingsDlg::onWake( %this ) {
}

//==============================================================================
function LabSettingsDlg::setSelectedSettings( %this,%treeItemObj ) {
	if (!isObject(%treeItemObj)) {
		warnLog("Invalid settings item objects selected:",%treeItemObj);
		return;
	}
	foreach(%gui in LS_SettingsContainer) {
		hide(%gui);
	}
	show(%treeItemObj.itemContainer);


}
//------------------------------------------------------------------------------
function LabSettingsDlg::createSettingContainer( %this,%group,%subgroup ) {
	%containerName = "lsPage_"@%group@"_"@%subgroup;
	if (isObject(%containerName)) {
		warnLog("There's already an object using that name:",%containerName.getName(),"ObjId=",%containerName.getId());
		return;
	}
	%newContainer = cloneWidget(LS_SampleContainer);
	%newContainer.setName(%containerName);
	%newContainer.internalName = %group@"_"@%subgroup;
	LS_SettingsContainer.add(%newContainer);
	return %newContainer;

}

//==============================================================================
function LabSettingsDlg::clearSettingsContainer( %this ) {

	foreach(%gui in LS_SettingsContainer) {
		if (%gui.internalName $= "core") continue;

		delObj(%gui);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Tree Builder functions
//==============================================================================
//==============================================================================
function LabSettingTree::rebuildAll( %this ) {

	LabSettingTree.clear();
	LabSettingsDlg.clearSettingsContainer();
	Lab.buildAllParams(true);

	LabSettingTree.buildVisibleTree();
}
//------------------------------------------------------------------------------

//==============================================================================
// LabSettingTree Callbacks
//==============================================================================

//==============================================================================
function LabSettingTree::onSelect( %this,%itemId ) {

	%text = %this.getItemText(%itemId);
	%value = %this.getItemValue(%itemId);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabSettingTree::onMouseUp( %this,%itemId,%clicks ) {

	%itemObj = $LabSettingItemObj[%itemId];

	%text = %this.getItemText(%itemId);
	%value = %this.getItemValue(%itemId);
	if (isObject(%itemObj)) {
		LabSettingsDlg.setSelectedSettings(%itemObj);
	} else {
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function LabSettingTree::addSettingGroup( %this,%group) {
	%tree = LabSettingTree;
	%groupTitle = $LabSettingsGroupName[%group];
	if (%groupTitle $="") %groupTitle = %group;
	%parentName = %tree.findItemByName( %group );
	%groupId = %tree.findItemByValue( %group );

	if( %groupId == 0 ) {
		%groupId = %tree.insertItem( 0, %groupTitle,%group );
	}
	return %groupId;
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::addParamToTree( %this,%paramObj) {

	if (!isObject(%paramObj.baseGuiControl)) {
		warnLog("Param:",%paramObj.groupLink,"Have invalid container! Tree item creation cancelled");
		return;
	}
	%tree = LabSettingTree;
	%parentId = LabSettingTree.addSettingGroup(%paramObj.group);


	%name = "soSettingItem_"@%paramObj.groupLink;
	%itemId = %tree.findChildItemByName( %parentID,%paramObj.type);
	if( !%itemId ) {
		%itemId = %tree.insertItem( %parentID, %paramObj.type,%paramObj.pattern );
	}

	%itemObj = newScriptObject(%name);
	%itemObj.groupParent = %paramObj.group;
	%itemObj.groupItem = %paramObj.type;
	$LabSettingItemObj[%itemId] = %itemObj;

	%itemObj.itemContainer = %paramObj.baseGuiControl;
}
//------------------------------------------------------------------------------