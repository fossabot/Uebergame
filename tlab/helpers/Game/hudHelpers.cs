//==============================================================================
// Boost! -> GuiControl Functions Helpers
// Copyright NordikLab Studio, 2013
//==============================================================================
//==============================================================================
// Schedule global on-off - Used to limit output of fast logs
//==============================================================================


//==============================================================================
// WORLD / SCREEN POSITIONING HELPERS
//==============================================================================
//==============================================================================
// Get the Screen coords for a 3D position
function worldToScreen(%pos) {
	return $HudCtrl.project(%pos);
}
//------------------------------------------------------------------------------
//==============================================================================
// Get the World coords from a screen pos (X Y Depth)
function screenToWorld(%pos) {
	return $HudCtrl.unproject(%pos);
}
//------------------------------------------------------------------------------








