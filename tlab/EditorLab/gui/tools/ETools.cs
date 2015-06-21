//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function ETools::initTools(%this) {
	foreach(%gui in %this){
		if (%gui.isMethod("initTool"))
			%gui.initTool();
	}
	ETransformBoxGui.initTool();
}
//------------------------------------------------------------------------------

//==============================================================================
function ETools::toggleTool(%this,%tool) {
	%dlg = %this.findObjectByInternalName(%tool,true);
	%this.fitIntoParents();
	devLog("Toogletool:",%tool);
	ETools.visible = true;

	if (%dlg.visible) {
		%dlg.setVisible(false);
		%position = getRealCursorPos();
		%dlg.position = %position;
		%dlg.position.x -= %dlg.extent.x/2;
		//%dlg.position.y -= EditorGuiMenubar.extent.y;
	} else {
		%dlg.setVisible(true);
	}
	
	%hideMe = true;
	foreach(%gui in %this)
		if (%gui.visible)
			%hideMe = false;
	if (%hideMe)
		%this.visible = 0;
	
	if (isObject(%dlg.linkedButton))
		%dlg.linkedButton.setStateOn(%dlg.visible);
}
//------------------------------------------------------------------------------

//==============================================================================
function ETools::showTool(%this,%tool) {
	%dlg = %this.findObjectByInternalName(%tool,true);
	%this.fitIntoParents();
	devLog("Toogletool:",%tool);
	ETools.visible = true;


	%dlg.setVisible(true);
		
	if (isObject(%dlg.linkedButton))
		%dlg.linkedButton.setStateOn(%dlg.visible);
}
//------------------------------------------------------------------------------

//==============================================================================
function ETools::hideTool(%this,%tool) {
	%dlg = %this.findObjectByInternalName(%tool,true);
	
	devLog("Toogletool:",%tool);	


	%dlg.setVisible(false);
	
	%hideMe = true;
	foreach(%gui in %this)
		if (%gui.visible)
			%hideMe = false;
			
	if (%hideMe)
		%this.visible = 0;
		
	if (isObject(%dlg.linkedButton))
		%dlg.linkedButton.setStateOn(%dlg.visible);
}
//------------------------------------------------------------------------------