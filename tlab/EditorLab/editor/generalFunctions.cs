//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
//==============================================================================

$RelightCallback = "";

function EditorLightingComplete() {
	$lightingMission = false;
	RelightStatus.visible = false;

	if ($RelightCallback !$= "") {
		eval($RelightCallback);
	}

	$RelightCallback = "";
}
//------------------------------------------------------------------------------
//==============================================================================
function updateEditorLightingProgress() {
	RelightProgress.setValue(($SceneLighting::lightingProgress));
	if ($lightingMission)
		$lightingProgressThread = schedule(1, 0, "updateEditorLightingProgress");
}
//------------------------------------------------------------------------------
//==============================================================================
function Editor::lightScene(%this, %callback, %forceAlways) {
	if ($lightingMission)
		return;

	$lightingMission = true;
	$RelightCallback = %callback;
	RelightStatus.visible = true;
	RelightProgress.setValue(0);
	Canvas.repaint();
	lightScene("EditorLightingComplete", %forceAlways);
	updateEditorLightingProgress();
}
//------------------------------------------------------------------------------
//==============================================================================
function EditTSCtrl::updateGizmoMode( %this, %mode ) {
	// Called when the gizmo mode is changed from C++

	if ( %mode $= "None" )
		EditorGuiToolbar->NoneModeBtn.performClick();
	else if ( %mode $= "Move" )
		EditorGuiToolbar->MoveModeBtn.performClick();
	else if ( %mode $= "Rotate" )
		EditorGuiToolbar->RotateModeBtn.performClick();
	else if ( %mode $= "Scale" )
		EditorGuiToolbar->ScaleModeBtn.performClick();
}
//------------------------------------------------------------------------------