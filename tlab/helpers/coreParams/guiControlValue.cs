//==============================================================================
// Helpers Lab -> UI - Canvas helpers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Update the GuiControl data field depending of the Class (use by Aggregate system)
//==============================================================================
//==============================================================================
// Get the type of a GuiControl depending of it class
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

	if (%class $= "GuiPopUpMenuCtrl" || %class $= "GuiTextEditCtrl" || %class $= "GuiTextCtrl")
			%type = "Text";
	return %type;
}
//------------------------------------------------------------------------------

//==============================================================================
/// Set the value for a GuiControl depending of it's type
/// %value : Value to assign to control
/// %updateFriends : Check for aggregated ctrl friends
function GuiControl::setTypeValue( %this,%value,%updateFriends ) {

	%type = %this.getType();

	switch$(%type) {
	case "Value":
		%this.setValue(%value);

	case "Checkbox":
		%this.setStateOn(%value);
	case "Color":
	   if (%this.isIntColor)
	     %this.BaseColor = ColorIntToFloat(%value);
		else
		   %this.BaseColor = %value;
		%this.updateColor();
	case "Text":
		%this.setText(%value);
	default:
		%this.setValue(%value);
	}
	if (%updateFriends)
		%this.updateFriends();

}
//------------------------------------------------------------------------------
//==============================================================================
/// Get the value of a GuiControl depending of it type
/// return : The value assigned to the control
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

//==============================================================================
// Initialize the client-side scripts
function GuiControl::syncObj(%this, %obj,%field,%skipAggregate) {
   %value = %this.getTypeValue();
   if (isObject(%obj) && %field !$= ""){
      %obj.setFieldValue(%field,%value);
      devLog("SyncObj",%obj.getName(),"Field",%field,"Value",%obj.getFieldValue(%field));
   }
   if (!%skipAggregate)
      %this.updateFriends();
      
   
	
}
//------------------------------------------------------------------------------