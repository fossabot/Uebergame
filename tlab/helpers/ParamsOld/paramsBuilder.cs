//==============================================================================
// Boost! -> Helper functions for common settings GUI needs
// Copyright NordikLab Studio, 2013
//==============================================================================
//==============================================================================
// Update the GuiControl data field depending of the Class
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamsInterface( %params ) {


	%widgetsControl = "wg_ParamsSystem_"@%params.style;
	%w["Header"] = %widgetsControl->Header;
	%w["Rollout"] = %widgetsControl->Rollout;

	%baseCtrl = %params.baseGuiControl;
	%stackBase = %params.baseStack;
	if (isObject(%stackBase))
		%stackBase.clear();

	%command = %params.common["Command"];
	%altCommand = %params.common["AltCommand"];
	%i = 1;

	while(%params.group[%i] !$="") {
		%display = "Fullview";
		%stackData = "";
		//info("Starting generation of:",%params.group[%i],"stack");
		if (isObject(%stackBase))
			%stackData = %stackBase;
		if (%params.groupStack[%i] !$="") {
			%stack = getWord(%params.groupStack[%i],0);
			%action = getWord(%params.groupStack[%i],1);
			%stackData = %baseCtrl.findObjectByInternalName(%stack,true);
		}


		if (!isObject(%stackData)) {
			warnLog("Invalid stack to add parameters, operation cancelled");
			%i++;
			continue;
		}

		if (%action !$= "Append" && %params.groupStack[%i] !$="")
			%stackData.clear();

		if (%params.display !$="")
			%display = %params.display;

		if (%params.groupDisplay[%i] !$="")
			%display = %params.groupDisplay[%i];

		if (%display $="Rollout") {
			//Create group rollout and add it to stack
			%rolloutCtrl = cloneObject(%w["Rollout"]);
			%rolloutCtrl.caption = %params.group[%i];
			%rolloutCtrl.expanded = false;
			if (%params.groupExpanded[%i])
				%rolloutCtrl.expanded = true;
			%stackData.add(%rolloutCtrl);
			%stackData = %rolloutCtrl-->stackCtrl;
		} else if (%display $="Fullview") {
			//Create group rollout and add it to stack
			%titleCtrl = cloneObject(%w["Header"]);
			%titleCtrl-->title.text = %params.group[%i];
			%stackData.add(%titleCtrl);
		}



		//Add all control in setting list
		foreach$(%setting in %params.groupFields[%i]) {
			if (%setting $= ">>") {
				%stackMain.add(cloneObject(%w["Seperator"]));
				%i++;
				continue;
			}

			%fieldsList = trim(%fieldsList SPC %setting);

			%type = getField(%params.setting[%setting],0);
			%field = getField(%params.setting[%setting],1);
			%pillSrc = %w[%type];
			eval("%pillSrc = %widgetsControl->"@%type@";");
			if (!isObject(%pillSrc)) {
				warnLog("Trying to clone invalid param widget for type:",%type);
				continue;
			}
			%pill = cloneObject(%pillSrc);

			if(%type $= "Dropdown2L") %type = "DropDown";
			switch$( %type) {
			//===========================================
			//SliderEdit Control Type Creation
			case "SliderEdit":

				%pill-->field.text = %field;
				//%pill-->field.internalName = "";

				//TextEdit ctrl update
				%textEdit = %pill-->textEdit;
				%textEdit.command = %command;
				%textEdit.altCommand = %altCommand;
				%textEdit.internalName = %setting;
				//Slider ctrl update
				%slider = %pill-->slider;
				%range = %params.settingParam[%setting,"range"];
				if (%range $="") %range = "0 1";

				%slider.range = %range;
				if (isObject(%pill-->minRange)) {
					%pill-->minRange.text = %range.x;
					%pill-->minRange.internalName = "";
				}
				if (isObject(%pill-->maxRange)) {
					%pill-->maxRange.text = %range.y;
					%pill-->maxRange.internalName = "";
				}

				%slider.command = %command;
				%slider.altCommand = %altCommand;
				%slider.internalName = %setting@"_slider";

			//===========================================
			//TextEdit Control Type Creation
			case "TextEdit":
				%pill-->field.text = %field;
				//%pill-->field.internalName = "";

				//TextEdit ctrl update
				%textEdit = %pill-->textEdit;
				%textEdit.command = %command;
				%textEdit.altCommand = %altCommand;
				%textEdit.internalName = %setting;
				%textEdit.variable = "";
			//===========================================
			//Gears Special Control Type Creation
			case "Gears":
				%pill-->Gear.text = "#"@%field+1;
				%pill-->Gear.internalName = "";

				//Radio ctrl update
				%ratio = %pill-->ratio;
				%ratio.command = %command;
				%ratio.altCommand = %altCommand;
				%ratio.internalName = "gearRatio"@%field;

				//MaxSpeed ctrl update
				%maxSpeed = %pill-->maxSpeed;
				%maxSpeed.command = %command;
				%maxSpeed.altCommand = %altCommand;
				%maxSpeed.internalName = "gearMax"@%field;
			//===========================================
			//Checkbox Control Type Creation
			case "Checkbox":
				%pill-->field.text = %field;
				%pill-->field.internalName = "";

				//Checkbox ctrl update
				%checkbox = %pill-->checkbox;
				%checkbox.internalName = %setting;
				%checkbox.text = %params.settingParam[%setting,"Text"];
				%checkbox.command = %command;
				%checkbox.altCommand = %altCommand;
				%checkbox.internalName = %setting;

			//===========================================
			//Color Control Type Creation
			case "Color":
				%pill-->field.text = %field;
				%pill-->field.internalName = "";

				//ColorPicker ctrl update
				%colorPicker = %pill-->colorPicker;
				%colorPicker.command = %command;
				%colorPicker.altCommand = %altCommand;
				%colorPicker.internalName = %setting;

			//===========================================
			//Color Control Type Creation
			case "ColorAlpha":
				%pill-->field.text = %field;
				%pill-->field.internalName = "NoUpdate";

				//ColorPicker ctrl update
				%colorPicker = %pill-->colorPicker;
				%colorPicker.command = %command;
				%colorPicker.altCommand = %altCommand;
				%colorPicker.internalName = %setting;
				%colorPicker.alpha = "alphaSlider";

				%alphaSlider = %pill-->slider;
				%alphaSlider.command = "updateParamColorAlpha($ThisControl);";
				%alphaSlider.altCommand = "updateParamColorAlpha($ThisControl);";
				%alphaSlider.internalName = "alphaSlider";
				%alphaSlider.dataType = %setting;
				%alphaSlider.range = "0 1";
				%alphaSlider.noFriends = true;
			//===========================================
			//Dropdown Control Type Creation
			case "Dropdown":
				%pill-->field.text = %field;
				%pill-->field.internalName = "";

				//Dropdown ctrl updare
				%menu = %pill-->dropdown;
				%menu.command = %command;
				%menu.altCommand = %altCommand;
				%menu.internalName = %setting;

				if (%params.settingParam[%setting,"guiGroup"] !$="") {
					%menu.guiGroup = %params.settingParam[%setting,"guiGroup"];
					UI.setCtrlGuiGroup(%menu);
				}
				if (%params.settingParam[%setting,"internalName"] !$="")
					%menu.internalName = %params.settingParam[%setting,"internalName"];

				//Update dropdown data
				%menu.clear();
				%menuData = %params.settingParam[%setting,"menuData"];
				if (%menuData!$="") {
					%updType = getWord(%menuData,0);
					%updValue = getWords(%menuData,1);
					if (%updType $="group") {
						%menuId = 1;
						foreach(%obj in %updValue) {
							%menu.add(%obj.getName(),%menuId);
							%menuId++;
						}
					} else if (%updType $="list") {
						eval("%datalist = $"@%updValue@";");
						%menuId = 1;
						foreach$(%obj in %datalist) {
							%menu.add(%obj,%menuId);
							%menuId++;
						}
					}
				}
			//===========================================
			//Dropdown Control Type Creation
			case "DropdownIcon":
				%pill-->field.text = %setting;
				%pill-->field.internalName = "";

				//Dropdown ctrl updare
				%menu = %pill-->dropdown;
				%menu.command = %command;
				%menu.altCommand = %altCommand;
				%menu.internalName = %setting;

				if (%params.settingParam[%setting,"guiGroup"] !$="") {
					%menu.guiGroup = %params.settingParam[%setting,"guiGroup"];
					UI.setCtrlGuiGroup(%menu);
				}
				if (%params.settingParam[%setting,"internalName"] !$="")
					%menu.internalName = %params.settingParam[%setting,"internalName"];

				//Update dropdown data
				%menu.clear();
				%menuData = %params.settingParam[%setting,"menuData"];
				if (%menuData!$="") {
					%updType = getWord(%menuData,0);
					%updValue = getWords(%menuData,1);
					if (%updType $="group") {
						%menuId = 1;
						foreach(%obj in %updValue) {
							%menu.add(%obj.getName(),%menuId);
							%menuId++;
						}
					} else if (%updType $="list") {
						eval("%datalist = $"@%updValue@";");
						%menuId = 1;
						foreach$(%obj in %datalist) {
							%menu.add(%obj,%menuId);
							%menuId++;
						}
					}
				}
				//Dropdown ctrl updare
				%icon = %pill-->icon;
				%icon.command = %params.settingParam[%setting,"iconCommand"];
				%icon.iconBitmap =  %params.settingParam[%setting,"icon"];



//===============================================================================
			}//End of switch
			%stackData.add(%pill);
		}
		%i++;
	}
	return %fieldList;
}
//------------------------------------------------------------------------------
