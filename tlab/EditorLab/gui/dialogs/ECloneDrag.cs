//==============================================================================
// Lab Helpers ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Profile FONTS manipulation functions
//==============================================================================

//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneDrag::toggleVisibility( %this ) {
	if ( %this.visible  ) {
		%this.setVisible(false);
		//SnapToBar-->snappingSettingsBtn.setStateOn(false);
	} else {
		%this.showDialog();
	}

}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneDrag::showDialog( %this ) {
	if (!Lab.CloneDragEnabled) return;
	%this.setVisible(true);
	%this.selectWindow();
	%this.setCollapseGroup(false);
}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneDrag::onWake( %this ) {
	%this-->copyCount.text = "1";
	%this-->copyOffset.text = "0 0 0";

}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneDrag::doCopy( %this ) {
	%copyCount = %this-->copyCount.getValue();
	if (%copyCount > 100)%copyCount = 100;
	if (%copyCount <= 0) return;
	Lab.copySelection(%this.copyOffset,%copyCount);
	%this.setVisible(false);
}
//------------------------------------------------------------------------------

//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneDrag::enableCloneDrag( %this,%enabled ) {
	Lab.CloneDragEnabled = %enabled;
	%button = SceneEditorToolbar-->cloneDragToggle;
	%button.setStateOn(%enabled);
	
}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneDrag::toggleCloneDrag( %this ) {
	Lab.CloneDragEnabled = !Lab.CloneDragEnabled;
	%this.enableCloneDrag(Lab.CloneDragEnabled);
	
	
}
//------------------------------------------------------------------------------