//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamColor( %pData ) {

	%pData.pill-->field.text = %pData.Title;
				%pData.pill-->field.internalName = "";

				//ColorPicker ctrl update
				%colorPicker = %pData.pill-->colorPicker;
				%colorPicker.command = %pData.Command;
				%colorPicker.altCommand = %pData.AltCommand;
				%colorPicker.internalName = %pData.InternalName;
				%checkbox.variable = %pData.Variable;

				%noAlpha = %pData.Option[%pData.Setting,"noalpha"];
				%colorPicker.lockedAlpha = %noAlpha;
}
//------------------------------------------------------------------------------
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamColorInt( %pData ) {

		%pData.pill-->field.text = %pData.Title;
				%pData.pill-->field.internalName = "";

				//ColorPicker ctrl update
				%colorPicker = %pData.pill-->colorPicker;
				%colorPicker.command = %pData.Command;
				%colorPicker.altCommand = %pData.AltCommand;
				%colorPicker.internalName = %pData.InternalName;
				%checkbox.variable = %pData.Variable;

				%noAlpha = %pData.Option[%pData.Setting,"noalpha"];
				%colorPicker.lockedAlpha = %noAlpha;
				%colorPicker.isIntColor = true;
}
//------------------------------------------------------------------------------
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamColorAlpha( %pData ) {

		%pData.pill-->field.text = %pData.Title;
				%pData.pill-->field.internalName = "NoUpdate";

				//ColorPicker ctrl update
				%colorPicker = %pData.pill-->colorPicker;
				%colorPicker.command = %pData.Command;
				%colorPicker.altCommand = %pData.AltCommand;
				%colorPicker.internalName = %pData.InternalName;
				%colorPicker.variable = %pData.Variable;
				%colorPicker.alpha = "alphaSlider";

				%alphaSlider = %pData.pill-->slider;
				%alphaSlider.command = "updateParamColorAlpha($Me);";
				%alphaSlider.altCommand = "updateParamColorAlpha($Me);";
				%alphaSlider.internalName = "alphaSlider";
				%alphaSlider.dataType = %setting;
				%alphaSlider.range = "0 1";
				%alphaSlider.noFriends = true;
				%alphaSlider.variable = %pData.Variable;
}
//------------------------------------------------------------------------------
	