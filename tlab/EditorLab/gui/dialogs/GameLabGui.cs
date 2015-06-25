//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Activate the interface for a plugin
//==============================================================================
//==============================================================================
//Set a plugin as active (Selected Editor Plugin)
function Lab::toggleGameDlg(%this,%dlg,%child) {
	%dlgCtrl = %dlg;
	if (%child !$= "")
		%dlgCtrl = %dlg.findObjectByInternalName(%child);
	if (!isObject(%dlgCtrl))
		return;
		
	if (Lab.currentGameDlg $= %dlgCtrl){
		%dlg.editorParent.add(%dlgCtrl);
		popDlg(GameLabGui);
		Lab.currentGameDlg = "";
	} else if (isObject(Lab.currentGameDlg)){
		Lab.currentGameDlg.editorParent.add(Lab.currentGameDlg);
		%dlg.editorParent = %dlgCtrl.parentGroup;
		GameLabGui.add(%dlgCtrl);
		Lab.currentGameDlg = %dlgCtrl;
		pushDlg(GameLabGui);
	}else {		
		%dlg.editorParent = %dlgCtrl.parentGroup;
		GameLabGui.add(%dlgCtrl);
		Lab.currentGameDlg = %dlgCtrl;
		pushDlg(GameLabGui);
	}
	
}
//------------------------------------------------------------------------------

