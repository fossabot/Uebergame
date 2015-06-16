//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$ESnapOptions_Initialized = false;
//==============================================================================
function EToolCamViewDlg::onWake(%this) {
}
//------------------------------------------------------------------------------

//==============================================================================
function EToolCamViewDlg::setView(%this,%dir,%button) {
	%camCtrl = %button.parentGroup;
	Lab.setCameraViewType(%dir);
}
//------------------------------------------------------------------------------
function EToolCamViewDlg::setState( %this,%enable,%forced ) {
	//Don't store state if forced (Plugin with no 3D)
	if (!%forced)
		$Lab_CamViewEnabled = %enable;

	if (!isObject(EToolCamViewSet))
		return;

	foreach(%camCtrl in EToolCamViewSet)
		%camCtrl.visible = %enable;
}
//  EToolCamViewDlg.addCamViewToCtrl(EditorFrameWorld);
//==============================================================================
function EToolCamViewDlg::addCamViewToCtrl(%this,%ctrl) {
	if (!isObject(EToolCamViewSet))
		newSimSet("EToolCamViewSet");

	//Make sure to delete the Camview if already assigned
	delObj(%ctrl-->CamViewFrame);
	%camCtrl = %this-->camViewCtrl.deepClone();
	%ctrl.add(%camCtrl);
	%camCtrl.superClass = "EToolCamCtrl";
	%camCtrl.position = EWToolsPaletteContainer.extent.x SPC EWToolsToolbar.extent.y;
	%camCtrl.internalName = "CamViewFrame";
	%camCtrl-->dragCam.viewCtrl = %camCtrl;
	//%camCtrl.AlignCtrlToParent("right");
	%this.setCurrentView(%camCtrl);
	EToolCamViewSet.add(%camCtrl);
	LabGuiSet.add(%camCtrl);
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolCamViewDlg::updateCurrentView(%this) {
	if (isObject(EToolCamViewSet))
		foreach(%ctrl in EToolCamViewSet)
			EToolCamViewDlg.setCurrentView(%ctrl);
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolCamViewDlg::setCurrentView(%this,%camCtrl) {
	%curCamId = Lab.cameraDisplayType;
	%text = getWord($LabCameraDisplayName[%curCamId],0);
	echo("Text:",%text);

	if (%text $= "Standard")
		%text = "3D";

	%camCtrl-->modeText.text = %text;
}
//------------------------------------------------------------------------------
//==============================================================================
function ECamViewClose::onClick( %this,%a1,%a2,%a3 ) {
	%parent = %this.parentGroup;
	hide(%parent);
	$Lab_CamViewEnabled = false;
}
//------------------------------------------------------------------------------

//==============================================================================
function ECamViewDrag::onMouseDragged( %this,%a1,%a2,%a3 ) {
	%parent = %this.parentGroup;
	%this.viewCtrl.superClass = "";
	%this.viewCtrl.class = "";
	%this.viewCtrl.superClass = "EToolCamCtrl";
	dragAndDropCtrl(%this.viewCtrl);
	hide(%parent);
}
//------------------------------------------------------------------------------
//==============================================================================
function EToolCamCtrl::DragSuccess( %this,%droppedCtrl,%pos) {
	show(%this);
	%realpos = EditorFrameWorld.getRealPosition();
	%pos.y -= %realPos.y + %this.extent.y/2;
	%pos.x -= %realPos.x + %this.extent.x/2;
	%this.position = %pos;
	%this.forceInsideCtrl(EditorFrameWorld);
}
//------------------------------------------------------------------------------
function EToolCamCtrl::DragSuccess2( %this,%droppedCtrl,%pos) {
	show(%this);
	%newPos = %pos;
	%realpos = %droppedCtrl.getRealPosition();
	%newPos.y -= %realPos.y + %this.extent.y/2;
	%newPos.x -= %this.extent.x/2;
	%this.position = %newPos;
	devLog("Dropped at:",%newPos,"Original",%pos);
	// %this.forceInsideCtrl(EditorFrameWorld);
}
//==============================================================================
function EToolCamCtrl::DragFailed( %this,%droppedCtrl,%pos) {
	devLog("Drag Failed Dropped in:",%droppedCtrl,"at position:",%pos);
}
//------------------------------------------------------------------------------