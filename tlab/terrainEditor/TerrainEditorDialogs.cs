//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function TerrainEditorDialogs::toggleDlg( %this, %dlg ) {
	%dlgCtrl = %this.findObjectByInternalName(%dlg);

	if (!isObject(%dlgCtrl))
		return;

	devLog(%dlg,"isAwake?",%dlgCtrl.isVisible());

	if (%dlgCtrl.isVisible()) {
		//Close the dialog
		hide(%dlgCtrl);
		%this.checkState();
	} else {
		show(%this);
		show(%dlgCtrl);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainEditorDialogs::checkState( %this ) {
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
function TerrainEditorDialogs::onSleep( %this ) {
	devLog("SceneEditorDialogs::onSleep");

	foreach(%ctrl in %this)
		hide(%ctrl);
}
//------------------------------------------------------------------------------