//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Initialize the Toolbar
//==============================================================================
//==============================================================================
function Lab::initAllPluginsToolbar() {
	foreach(%gui in LabToolbarGuiSet) {
		
		Lab.initPluginToolbar(%gui);
		
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::initPluginToolbar(%this,%gui) {
	if (%gui.plugin $= "")
		%gui.plugin = strreplace(%gui.getName(),"Toolbar","");
	%pluginObj = %gui.plugin@"Plugin";
	if (!isObject(%pluginObj))
		warnLog("Can't find the plugin obj for gui:",%gui,%gui.getName());
		
	%defaultStack = %gui;
	if (%gui.getClassName() !$= "GuiStackControl")
		%defaultStack = %gui.getObject(0);		
	
	%defaultStack.superClass = "ToolbarPluginStack";
	%gui.defaultStack = %defaultStack;
	
	%end = %gui.defaultStack-->StackEnd;
	if (!isObject(%end)){
		%end =  new GuiContainer() {        
         extent = "84 30";        
         profile = "ToolsDefaultProfile";        
         internalName = "StackEnd";
         superClass = "PluginToolbarEnd";       
      };
      %defaultStack.add(%end);
	}
	
	
	%defaultStack.pushToBack(%end);
	%pluginObj.toolbarGroups = "";
	foreach(%ctrl in %defaultStack)
		if (%ctrl.getClassName() $= "GuiStackControl")
			%pluginObj.toolbarGroups = strAddWord(%pluginObj.toolbarGroups,%ctrl.getId());
		
	%this.scanPluginToolbarGroup(%defaultStack,"",%pluginObj);	
	
	%plugName = %pluginObj.displayName;
	if (%pluginObj.displayName $= "")
		%plugName = "InvalidPlugin" SPC %gui.plugin;
	
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::scanPluginToolbarGroup(%this,%group,%level,%pluginObj) {
	
	foreach(%ctrl in %group) {
		
		if (%ctrl.getCount() > 0){
			
			%this.scanPluginToolbarGroup(%ctrl,%level++);
			continue;
		}
		if (%ctrl.isMemberOfClass("GuiIconButtonCtrl")){			
			%ctrl.superClass = "ToolbarIcon";	
			%ctrl.useMouseEvents = true;	
		}
	}
}

//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
//==============================================================================
function Lab::toggleToolbarGroupVisible(%this,%groupId,%itemId) {	
	%menu = Lab.currentEditor.toolbarMenu;
	%checked = %menu.isItemChecked(%itemId);
	toggleVisible(%groupId);
	%groupId.visible = !%checked;
	devLog("GroupId",%groupId,"Visible=",%checked);
	//if (%groupId.isVisible())
		%menu.checkItem(%itemId,!%checked);
	//else
	//	%menu.checkItem(%itemId,false);
}
//------------------------------------------------------------------------------

//==============================================================================
// Show Popup Options
//==============================================================================
//==============================================================================
function Lab::rebuildAllToolbarPluginMenus(%this) {
	foreach(%gui in LabToolbarGuiSet) {
		%pluginObj = %gui.plugin@"Plugin";
		delObj(%pluginObj.toolbarMenu);		
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::ShowToolbarOptionMenu(%this) {
	devLog("Lab::ShowToolbarOptionMenu(%this) ",%this);
	%plugObj = Lab.currentEditor;
	if (!isObject(%plugObj.toolbarMenu)){
		%menu = new PopupMenu() {			
			superClass = "ContextMenu";
			isPopup = true;		
			
			object = -1;
			profile = $LabCfg_ContextMenuProfile;
		};
		%itemId = 0;
		foreach$(%toolbarGroup in %plugObj.toolbarGroups){
			%line = "Toggle" SPC %toolbarGroup.internalName TAB "" TAB "Lab.toggleToolbarGroupVisible("@%toolbarGroup.getId()@","@%itemId@");";
			%menu.addItem(%itemId,%line);
			%menu.groupItem[%itemId] = %toolbarGroup.getId();
			%menu.itemGroup[%toolbarGroup.getId()] = %itemId;
			if (%toolbarGroup.isVisible())
				%menu.checkItem(%itemId,true);
			else 
				%menu.checkItem(%itemId,false);
			%itemId++;			
		}
		%plugObj.toolbarMenu = %menu;
	}	
	
	%plugObj.toolbarMenu.showPopup(Canvas);	
}
//------------------------------------------------------------------------------
//==============================================================================
function ToolbarArea::onRightMouseDown(%this,%modifier,%point,%clicks) {
	devLog("ToolbarArea::onRightMouseDown(%this,%modifier,%point,%clicks) ",%this,%modifier,%point,%clicks);
	Lab.ShowToolbarOptionMenu();	

}
//------------------------------------------------------------------------------
//==============================================================================
function ToolbarBoxChild::onRightMouseDown(%this,%modifier,%point,%clicks) {
	devLog("ToolbarBoxChild::onRightMouseDown(%this,%modifier,%point,%clicks) ",%this,%modifier,%point,%clicks);
	Lab.ShowToolbarOptionMenu();	

}
//------------------------------------------------------------------------------
//==============================================================================
function ToolbarIcon::onRightClick(%this,%modifier,%point,%clicks) {
	devLog("ToolbarIcon::onRightClick(%this,%modifier,%point,%clicks) ",%this,%modifier,%point,%clicks);
	Lab.ShowToolbarOptionMenu();	

}
//------------------------------------------------------------------------------
