//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
// Shape Editor
//------------------------------------------------------------------------------

function initializeLabModel() {
	echo(" % - Initializing Shape Editor");
	execLabModel(true);
	Lab.addPluginGui("LabModel",LabModelTools);
	Lab.addPluginEditor("LabModel",LabModelPreviewGui);
	Lab.addPluginToolbar("LabModel",LabModelToolbar);
	Lab.addPluginPalette("LabModel",   LabModelPalette);
	Lab.createPlugin("LabModel","Model Editor");
	LabModelPlugin.editorGui = LabModelPreview;
	// Add windows to editor gui
	%map = new ActionMap();
	%map.bindCmd( keyboard, "escape", "ToolsToolbarArray->SceneEditorPalette.performClick();", "" );
	%map.bindCmd( keyboard, "1", "LabModelNoneModeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "2", "LabModelMoveModeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "3", "LabModelRotateModeBtn.performClick();", "" );
	//%map.bindCmd( keyboard, "4", "LabModelScaleModeBtn.performClick();", "" ); // not needed for the shape editor
	%map.bindCmd( keyboard, "n", "LabModelToolbar->showNodes.performClick();", "" );
	%map.bindCmd( keyboard, "t", "LabModelToolbar->ghostMode.performClick();", "" );
	%map.bindCmd( keyboard, "r", "LabModelToolbar->wireframeMode.performClick();", "" );
	%map.bindCmd( keyboard, "f", "LabModelToolbar->fitToShapeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "g", "LabModelToolbar->showGridBtn.performClick();", "" );
	%map.bindCmd( keyboard, "h", "LabModelSelectWindow->tabBook.selectPage( 2 );", "" ); // Load help tab
	%map.bindCmd( keyboard, "l", "LabModelSelectWindow->tabBook.selectPage( 1 );", "" ); // load Library Tab
	%map.bindCmd( keyboard, "j", "LabModelSelectWindow->tabBook.selectPage( 0 );", "" ); // load scene object Tab
	%map.bindCmd( keyboard, "SPACE", "LabModelAnimWindow.togglePause();", "" );
	%map.bindCmd( keyboard, "i", "LabModelSequences.onEditSeqInOut(\"in\", LabModelSeqSlider.getValue());", "" );
	%map.bindCmd( keyboard, "o", "LabModelSequences.onEditSeqInOut(\"out\", LabModelSeqSlider.getValue());", "" );
	%map.bindCmd( keyboard, "shift -", "LabModelSeqSlider.setValue(LabModelAnimWindow-->seqIn.getText());", "" );
	%map.bindCmd( keyboard, "shift =", "LabModelSeqSlider.setValue(LabModelAnimWindow-->seqOut.getText());", "" );
	%map.bindCmd( keyboard, "=", "LabModelAnimWindow-->stepFwdBtn.performClick();", "" );
	%map.bindCmd( keyboard, "-", "LabModelAnimWindow-->stepBkwdBtn.performClick();", "" );
	LabModelPlugin.map = %map;
	//LabModelPlugin.initSettings();
}
//==============================================================================
// Load the Scene Editor Plugin scripts, load Guis if %loadgui = true
function execLabModel(%loadGui) {
	if (%loadGui) {
		exec("tlab/LabModel/gui/LabModelPreviewGui.gui");
		exec("tlab/LabModel/gui/LabModelAnimWindow.gui");
		exec("tlab/LabModel/gui/LabModelToolbar.gui");
		exec("tlab/LabModel/gui/LabModelPalette.gui");
		exec("tlab/LabModel/gui/LabModelTools.gui");
	}

	exec("tlab/LabModel/initActions.cs");
	exec("tlab/LabModel/LabModelPlugin.cs");
	exec("tlab/LabModel/LabModelParams.cs");
	exec("tlab/LabModel/LabModelTools.cs");
	exec("tlab/LabModel/LabModelEditor.cs");
	execPattern("tlab/LabModel/shape/*.cs");
	execPattern("tlab/LabModel/scripts/*.cs");
	execPattern("tlab/LabModel/actions/*.cs");
	execPattern("tlab/LabModel/infoTabs/*.cs");
}
//------------------------------------------------------------------------------


function destroyLabModel() {
}

function SetToggleButtonValue(%ctrl, %value) {
	if ( %ctrl.getValue() != %value )
		%ctrl.performClick();
}

function LabModelWireframeMode() {
	$gfx::wireframe = !$gfx::wireframe;
	LabModelToolbar-->wireframeMode.setStateOn($gfx::wireframe);
}
