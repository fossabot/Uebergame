//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Manage Plugins Dialogs
//==============================================================================
//==============================================================================
function Lab::initAllPluginsDialogs( %this ) {
	foreach(%gui in LabDialogGuiSet) {	
		foreach(%dlg in %gui){
			%dlg.visible = 0;
		}		
		%gui.initDialogs();
	}	
}
//------------------------------------------------------------------------------
//==============================================================================
// Manage Plugins Dialogs
//==============================================================================

//==============================================================================
function PluginDlg::initDialogs( %this ) {
	foreach(%obj in %this) {
		if (%obj.isMethod("initDialog"))
				%obj.initDialog();
	}

}
//------------------------------------------------------------------------------
//==============================================================================
function PluginDlg::onActivatedDialogs( %this ) {
	
	
	if (%this.isMethod("onActivated"))
				%this.onActivated();
				
	foreach(%obj in %this) {
		%obj.visible = "0";
		if (%obj.isMethod("onActivated"))
				%obj.onActivated();
	}
	%this.visible = "0";
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginDlg::toggleDlg( %this, %dlg ) {
	%this.fitIntoParents();
	%dlgCtrl = %this.findObjectByInternalName(%dlg);

	if (!isObject(%dlgCtrl))
		return;

	if (%dlgCtrl.isVisible()) 		
		%this.hideDlg(%dlg);		
	else 
		%this.showDlg(%dlg);		
	
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginDlg::showDlg( %this, %dlg ) {
	%this.fitIntoParents();
	%dlgCtrl = %this.findObjectByInternalName(%dlg);

	if (!isObject(%dlgCtrl))
		return;

	if (%dlgCtrl.isVisible()) 
		return;
		
	show(%dlgCtrl);
	show(%this);	
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginDlg::hideDlg( %this, %dlg ) {
	%this.fitIntoParents();
	%dlgCtrl = %this.findObjectByInternalName(%dlg);

	if (!isObject(%dlgCtrl))
		return;

	if (!%dlgCtrl.isVisible()) 
		return;
		
	hide(%dlgCtrl);
	%this.checkState();
}
//------------------------------------------------------------------------------
//==============================================================================
function PluginDlg::checkState( %this ) {
	%visibleCount = 0;
	%hiddenCount = 0;

	foreach(%ctrl in %this) {
		if (%ctrl.visible)
			%visibleCount++;
		else
			%hiddenCount++;
	}

	//If all are hidden, hide the dialog container
	if (%visibleCount == 0 && %this.visible)
		hide(%this);
	else if (!%this.visible)
		show(%this);
}
//------------------------------------------------------------------------------

//==============================================================================
function PluginDlg::onSleep( %this ) {
	devLog("PluginDlg::onSleep");

	foreach(%ctrl in %this)
		hide(%ctrl);
}
//------------------------------------------------------------------------------