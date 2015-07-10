//==============================================================================
// Helpers Lab -> UI - Canvas helpers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
//! getRealCursorPos() > Get the Cursor position inside the engine window
//+ Canvas.getCursorPos() return the cursor position on the user screen, with this function
//+ you will get the cursor position inside Torque3D window.
//+ Usage: %realPos = getRealCursorPos();
function getRealCursorPos() {
	%cursorPos = Canvas.getCursorPos();
	%windowPos = Canvas.getWindowPosition();
	%realPos = getWords(VectorSub(%cursorPos,%windowPos),0,1);
	
	return %realPos;  
}
//------------------------------------------------------------------------------


//==============================================================================
// Push, Pop and Toggle Dialogs
//==============================================================================

//==============================================================================
//Push a dialog to the canvas
function pushDlg(%dlg) {
	Canvas.pushDialog(%dlg);
}
//------------------------------------------------------------------------------
//==============================================================================
//Pop a dialog from the canvas
function popDlg(%dlg) {
	Canvas.popDialog(%dlg);
}
//------------------------------------------------------------------------------
//==============================================================================
// Toggle a Dialog GUI
function toggleDlg(%dlg) {
	if (%dlg.isAwake())
		popDlg(%dlg);
	else
		pushDlg(%dlg);
}
//------------------------------------------------------------------------------

//==============================================================================
// Content GUI helpers (Canvas.setContent() simplified)
//------------------------------------------------------------------------------
$HLab_DefaultGui = "MainMenuGui"; //DefaultGui if invalid GUI called
//==============================================================================
//==============================================================================
// Set canvas content and store previous GUI (Can be specified optionally)
function setGui(%gui,%previousGui) {
	$PreviousGui = Canvas.getContent();
	Canvas.setContent(%gui);
	if (isObject(%previousGui))
		$PreviousGui = %previousGui;
}
//------------------------------------------------------------------------------
//==============================================================================
// Set the Previous canvas content that was loaded before the current content
function setPrevGui(%defaultGui) { 
   if (%defaultGui $= "")
      %defaultGui = $HLab_DefaultGui;  
   %setGui = $PreviousGui;
   if (!isObject($PreviousGui) && isObject(%defaultGui)){
      warnLog("Couldn't find a previous GUI, calling the Default GUI:",%defaultGui);
      %setGui = %defaultGui;
   }
   
   if (!isObject(%setGui)){   
      warnLog("There's no previous GUI Stored! Nothing done...");
      return;      
   }	   
   Canvas.setContent(%setGui);	
}
//------------------------------------------------------------------------------

//==============================================================================
// Toggle between Specified GUI and the PreviousGui setted
function toggleGui(%gui) {
	if (!isObject(%gui))return;
	if (Canvas.getContent().getId() $= %gui.getId())
		setGui($PreviousGui);
	else
		setGui(%gui);
}
//------------------------------------------------------------------------------