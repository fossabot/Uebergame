//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function EToolDlgGroup::openSlider (%this,%button) {
	%cursorpos = Canvas.getCursorPos();
	%realPos = %button.getRealPosition();
	EToolDialogSlider.position.y = 0;
	EToolDialogSlider.position.x = %realPos.x - EToolDialogSlider.extent.x/2;
	%targetCtrl = %button.parentGroup.findObjectByInternalName(%button.targetCtrl);
	%realPos = %button.getRealPosition();
	%slider = EToolDialogSlider-->slider;
	%slider.updateCommand = %button.updateCommand;
	%slider.targetCtrl = %targetCtrl;
	%slider.setValue(%targetCtrl.getValue());
	%slider.range = %button.range;
	show(EToolDialogSliderMouse);
}


function EToolDlgSlider::onMouseDragged (%this, %modifier, %mousePoint,%mouseClickCount) {
	%value = mFloor(%this.getValue());
	%this.targetCtrl.setValue(%value);
	%command = strreplace(%this.updateCommand,"***",%value);
	eval(%command);
}
function EToolDlgMouseArea::onMouseDown (%this, %modifier, %mousePoint,%mouseClickCount) {
	hide(%this);
}