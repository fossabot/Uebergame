//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
// Shape Editor
//------------------------------------------------------------------------------

function initializeShapeEditor() {
	echo(" % - Initializing Shape Editor");
	exec("./gui/Profiles.cs");
	exec("tlab/shapeEditor/gui/shapeEdPreviewWindow.gui");
	exec("./gui/shapeEdAnimWindow.gui");
	exec("./gui/shapeEditorToolbar.gui");
	exec("./gui/shapeEditorPalette.gui");
	exec("./gui/ShapeEditorTools.gui");
	exec("./scripts/shapeEditor.cs");
	exec("./scripts/shapeEditorHints.cs");
	exec("./scripts/shapeEditorActions.cs");
	exec("tlab/shapeEditor/ShapeEditorPlugin.cs");
	exec("tlab/shapeEditor/ShapeEditorParams.cs");
	Lab.addPluginGui("ShapeEditor",ShapeEditorTools);
	Lab.addPluginEditor("ShapeEditor",ShapeEdPreviewGui);
	Lab.addPluginEditor("ShapeEditor",ShapeEdAnimWindow,true);
	Lab.addPluginToolbar("ShapeEditor",ShapeEditorToolbar);
	Lab.addPluginPalette("ShapeEditor",   ShapeEditorPalette);
	Lab.createPlugin("ShapeEditor","Shape Editor");
	ShapeEditorPlugin.editorGui = ShapeEdShapeView;
	// Add windows to editor gui
	%map = new ActionMap();
	%map.bindCmd( keyboard, "escape", "ToolsToolbarArray->SceneEditorPalette.performClick();", "" );
	%map.bindCmd( keyboard, "1", "ShapeEditorNoneModeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "2", "ShapeEditorMoveModeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "3", "ShapeEditorRotateModeBtn.performClick();", "" );
	//%map.bindCmd( keyboard, "4", "ShapeEditorScaleModeBtn.performClick();", "" ); // not needed for the shape editor
	%map.bindCmd( keyboard, "n", "ShapeEditorToolbar->showNodes.performClick();", "" );
	%map.bindCmd( keyboard, "t", "ShapeEditorToolbar->ghostMode.performClick();", "" );
	%map.bindCmd( keyboard, "r", "ShapeEditorToolbar->wireframeMode.performClick();", "" );
	%map.bindCmd( keyboard, "f", "ShapeEditorToolbar->fitToShapeBtn.performClick();", "" );
	%map.bindCmd( keyboard, "g", "ShapeEditorToolbar->showGridBtn.performClick();", "" );
	%map.bindCmd( keyboard, "h", "ShapeEdSelectWindow->tabBook.selectPage( 2 );", "" ); // Load help tab
	%map.bindCmd( keyboard, "l", "ShapeEdSelectWindow->tabBook.selectPage( 1 );", "" ); // load Library Tab
	%map.bindCmd( keyboard, "j", "ShapeEdSelectWindow->tabBook.selectPage( 0 );", "" ); // load scene object Tab
	%map.bindCmd( keyboard, "SPACE", "ShapeEdAnimWindow.togglePause();", "" );
	%map.bindCmd( keyboard, "i", "ShapeEdSequences.onEditSeqInOut(\"in\", ShapeEdSeqSlider.getValue());", "" );
	%map.bindCmd( keyboard, "o", "ShapeEdSequences.onEditSeqInOut(\"out\", ShapeEdSeqSlider.getValue());", "" );
	%map.bindCmd( keyboard, "shift -", "ShapeEdSeqSlider.setValue(ShapeEdAnimWindow-->seqIn.getText());", "" );
	%map.bindCmd( keyboard, "shift =", "ShapeEdSeqSlider.setValue(ShapeEdAnimWindow-->seqOut.getText());", "" );
	%map.bindCmd( keyboard, "=", "ShapeEdAnimWindow-->stepFwdBtn.performClick();", "" );
	%map.bindCmd( keyboard, "-", "ShapeEdAnimWindow-->stepBkwdBtn.performClick();", "" );
	ShapeEditorPlugin.map = %map;
	//ShapeEditorPlugin.initSettings();
}

function destroyShapeEditor() {
}

function SetToggleButtonValue(%ctrl, %value) {
	if ( %ctrl.getValue() != %value )
		%ctrl.performClick();
}

function shapeEditorWireframeMode() {
	$gfx::wireframe = !$gfx::wireframe;
	ShapeEditorToolbar-->wireframeMode.setStateOn($gfx::wireframe);
}
