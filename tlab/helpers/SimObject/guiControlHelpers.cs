//==============================================================================
// Boost! -> GuiControl Functions Helpers
// Copyright NordikLab Studio, 2013
//==============================================================================
//==============================================================================
// Common GuiControl Helpers
//==============================================================================

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
// Content GUI helpers
//==============================================================================
//==============================================================================
// Short for Canvas.setContent(%gui)
function setGui(%gui,%previousGui) {
	$PreviousGui = Canvas.getContent();
	Canvas.setContent(%gui);
	if (isObject(%previousGui))
		%gui.previousGui = %previousGui;
}
//------------------------------------------------------------------------------
//==============================================================================
// Toggle between GUI
function toggleGui(%gui) {
	if (!isObject(%gui))return;
	if (Canvas.getContent().getId() $= %gui.getId())
		setGui($PreviousGui);
	else
		setGui(%gui);
}
//------------------------------------------------------------------------------
//==============================================================================
// Toggle between GUI
function fitIntoParents(%gui,%margin) {
	%parent = %gui.parentGroup;

	%pos = "0 0";
	%extent = %parent.extent;

	%gui.position = %pos;
	%gui.extent = %extent;
}
//------------------------------------------------------------------------------

//==============================================================================
// Initialize the client-side scripts
function GuiControl::collapseChilds(%this, %state) {
	%this.callOnChildrenNoRecurse("collapse", %state);
}
//------------------------------------------------------------------------------
//==============================================================================
// Initialize the client-side scripts
function GuiControl::expandChilds(%this, %state) {
	foreach(%ctrl in %this) {
		%ctrl.expanded = %state;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// GuiStackControl Helpers
//==============================================================================
//==============================================================================
// Collapse all stacks
function StackCollapse::collapseAll(%this) {
	%this.callOnChildrenNoRecurse("setCollapsed", true);
}
//----------------------------------------------------------------------------

//==============================================================================
// Drag & Drop
//==============================================================================

//==============================================================================
// Collapse all stacks
function GuiControl::getRealPosition(%this) {   
   %parent = %this.parentGroup;
   %pos = %this.position;
   while(isObject(%parent)){
     %pos.x += %parent.position.x;
      %pos.y += %parent.position.y;
	   %parent = %parent.parentGroup;
   }
   return %pos;
}
//----------------------------------------------------------------------------


//==============================================================================
// Collapse all stacks
function GuiControl::forceInsideCtrl(%this,%ctrl) {   
   %maxX = %ctrl.extent.x - %this.extent.x;
   %maxY = %ctrl.extent.y - %this.extent.y;
   
   if (%maxX < 0 || %maxY < 0){
      warnLog("The control doesn't fit into the target because it's bigger! Source:",%this.extent,"Target:",%ctrl.extent);
      return;
   }  
   %this.position.x = mClamp(%this.position.x,0,%maxX);
   %this.position.y = mClamp(%this.position.y,0,%maxY);  
}
//----------------------------------------------------------------------------

//==============================================================================
// SetValue Safe Functions
//==============================================================================
function GuiControl::setValueSafe(%this, %val) {
	%cmd = %this.command;
	%alt = %this.altCommand;
	%this.command = "";
	%this.altCommand = "";
	%this.setValue(%val);
	%this.command = %cmd;
	%this.altCommand = %alt;
}

function GuiControl::shareValueSafe(%this, %dest) {
	%dest.setValueSafe(%this.getValue());
}

function GuiControl::shareValueSafeDelay(%this, %dest, %delayMs) {
	%this.schedule(%delayMs, "shareValueSafe", %dest);
}

//==============================================================================
// Update the GuiControl data field depending of the Class (use by Aggregate system)
//==============================================================================
//==============================================================================
// Initialized the Env Manager Level setup
function GuiControl::getType( %this ) {


	if (!isObject(%this)) {

		return "";
	}

	%class = %this.getClassName();
	if (%class $= "GuiTextEditSliderBitmapCtrl" || %class $= "GuiTextEditSliderCtrl"
		              || %class $= "GuiSliderCtrl")
			%type = "Value";

	if (%class $= "GuiCheckBoxCtrl")
			%type = "Checkbox";

	if (%class $= "GuiColorPickerCtrl")
			%type = "Color";

	if (%class $= "GuiPopUpMenuCtrl" || %class $= "GuiTextEditCtrl")
			%type = "Text";
	return %type;
}
//------------------------------------------------------------------------------
//==============================================================================
// Initialized the Env Manager Level setup
function GuiControl::setTypeValue( %this,%value ) {

	%type = %this.getType();

	switch$(%type) {
	case "Value":
		%this.setValue(%value);

	case "Checkbox":
		%this.setStateOn(%value);
	case "Color":
		%this.BaseColor = %value;
	case "Text":
		%this.setText(%value);
	}

}
//------------------------------------------------------------------------------
//==============================================================================
// Initialized the Env Manager Level setup
function GuiControl::getTypeValue( %this ) {
	%type = %this.getType();

	switch$(%type) {
	case "Value":
		%value = %this.getValue();
	case "Text":
		%value = %this.getText();

	case "Checkbox":
		%value = %this.isStateOn();
	case "Color":
		%value = %this.BaseColor ;

	}
	return %value;
}
//------------------------------------------------------------------------------