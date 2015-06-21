//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//------------------------------------------------------------------------------
// Shape Preview
//------------------------------------------------------------------------------

function LabModelPreviewGui::updatePreviewBackground( %color ) {
	LabModelPreviewGui-->previewBackground.color = %color;
	LabModelToolbar-->previewBackgroundPicker.color = %color;
}

function showLabModelPreview() {
	%visible = LabModelToolbar-->showPreview.getValue();
	LabModelPreviewGui.setVisible( %visible );
}
