//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Update the GuiControl data field depending of the Class
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildLabParams( %params ) {

	%widgetsControl = "wLabParams_"@%params.style;

	if ( %params.aggregateStyle $= "")
		%aggregateClass = "AggregateVar";
	else
		%aggregateClass = "Aggregate"@%params.aggregateStyle;



	%baseCtrl = %params.baseGuiControl;
	%widgetStyle = %params.style;

	if (!isObject(%baseCtrl)) {
		warnLog("Invalid Lab Settings Params COntainer:",%baseCtrl);
		return;
	}


	%gid = 1;
	while(%params.groupTitle[%gid] !$="") {	   
		%stack = %params.groupStack[%gid];
		%stackObj = getWord(%stack,0);
		%stackOption = getWord(%stack,1);
		if (!isObject(%stackObj))
			%stackObj = %baseCtrl.findObjectByInternalName(getWord(%stack,0),true);

		if (!isObject(%stackObj)) {
			warnLog("Invalid stack object for group:",getWord(%stack,0),"Param building skipped");
			%gid++;
			continue;
		}
		if (%stackOption !$= "Append")
			%stackObj.clear();

		//------------------------------------------------
		//Group Display Setup
		%display = %params.groupDisplay[%gid];
		%displayType = getWord(%display,0);
		%displayOptions = getWords(%display,1);



		%displayWidget = %widgetsControl.findObjectByInternalName(%displayType);
		if(isObject(%displayWidget)) {
			%displayCtrl = cloneObject(%displayWidget);
			%displayClass = %displayCtrl.getClassName();
			switch$(%displayClass) {
			case "GuiRolloutCtrl":
				%displayCtrl.caption = %params.groupTitle[%gid];
				%newStackObj = %displayCtrl->stackCtrl;
				%rolloutCtrl.expanded = false;
			default:
				%displayCtrl-->title.text = %params.groupTitle[%gid];
			}
			foreach$(%word in %displayOptions) {
				if (%word $= "AutoCollapse") {
					%displayCtrl.autoCollapseSiblings = true;
				}
				if (%word $= "Collapsed") {
					%displayCtrl.expanded = false;
				}
			}
			/*
			%displayOptions = strreplace(%displayOptions,";;","\t");
			%fieldCount = getFieldCount(%displayOptions);
			for(%i=0;%i<%fieldCount;%i++){
			   %setting = getField(%displayOptions ,%i);

			   if (%setting $= "AutoCollapse")
			      %displayCtrl.autoCollapseSiblings = true;
			  // %fullSetting = strreplace(%setting,"::",".\"") @"\";";
			  // eval(%displayCtrl@%fullSetting);
			}
			*/

			%stackObj.add(%displayCtrl);

			if(isObject(%newStackObj))
				%stackObj = %newStackObj;

			%newStackObj = "";
		} else
			info("No valid group display setting found for group:",%params.groupTitle[%gid],"DisplayType:",%displayType);


		//------------------------------------------------
		//Group Fields Setup
		//Field 0 = Setting (Global if start with $)
		//Field 1 = Title
		//Field 2 = Type
		//Field 3 = Options
		%fid=1;
		while (%params.groupFieldData[%gid,%fid] !$= "") {
			%setting = %params.groupStack[%gid];

			%command = %params.common["Command"];
			%altCommand = %params.common["AltCommand"];

			if ( %params.groupCommand[%gid] !$= "")
				%command =  %params.groupCommand[%gid];
			if ( %params.groupAltCommand[%gid] !$= "")
				%altCommand =  %params.groupAltCommand[%gid];

			%fieldSetting = getField(%params.groupFieldData[%gid,%fid],0);
			%fieldTitle = getField(%params.groupFieldData[%gid,%fid],1);
			%fieldType = getField(%params.groupFieldData[%gid,%fid],2);
			%fieldOptions = getField(%params.groupFieldData[%gid,%fid],3);
			%fieldCommand = %command;
			%fieldAltCommand = %altCommand;

			if (%fieldTitle $= "") %fieldTitle = %fieldSetting;
			%fieldInternalName = %fieldSetting;
			%fieldVariable = "";
			if (getSubStr(%fieldInternalName,0,1) $= "$") {
				%fieldVariable = %fieldInternalName;
				%fieldInternalName = strreplace(%fieldInternalName,"$","");
				%fieldInternalName = strreplace(%fieldInternalName,"::","__");
				eval("%fieldValue = "@%fieldVariable@";");
			}

			%widgetSource = %fieldType;
			if (%widgetSource $= "ColorInt")
				%widgetSource = "Color";

			%fieldWidget = %widgetsControl.findObjectByInternalName(%widgetSource);
			if(!isObject(%fieldWidget)) {
				warnLog("Couldn't find widget for setting type:",%fieldType,"This setting building is skipped WidgetSource:",%widgetsControl.getName());
				%fid++;
				continue;
			}

			%paramsFieldList = trim(%paramsFieldList SPC %fieldSetting);
			%pill = cloneObject(%fieldWidget);
			%fieldCategory = %fieldType;
			if (%pill.paramType !$= "") %fieldCategory = %pill.paramType;

			%pill.class = %aggregateClass;
			//------------------------------------------------
			//Prepare the field special options
			// The options are applied for each Field Category alone
			%fieldOptionList[%fieldSetting] = "";
			%fieldOptions = strreplace(%fieldOptions,";;","\t");
			%fieldCount = getFieldCount(%fieldOptions);
			for(%i=0; %i<%fieldCount; %i++) {
				%option = getField(%fieldOptions ,%i);
				%optWords = strreplace(%option,"::"," ");
				%optField = getWord(%optWords,0);
				%optCmd = getWords(%optWords,1);
				%fieldOption[%fieldSetting,%optField] = %optCmd;
				%fieldOptionCmd[%fieldSetting,%optField] = "."@%optField@" = \""@ %optCmd  @"\";";
				%fieldOptionList[%fieldSetting] = trim(%fieldOptionList[%fieldSetting] SPC %optField);
			}
			%tmpFieldValue = %fieldValue;
			%multiplier = 1;
			%tooltip = %fieldOption[%fieldSetting,"tooltip"];
			if (%tooltip !$= "" && %params.tooltipDelay !$= "")
				%tooltipDelay = %params.tooltipDelay;
			else
				%tooltipDelay = 1000;
			switch$( %fieldCategory) {
			//===========================================
			//SliderEdit Control Type Creation
			case "SliderEdit":

				%mouseArea = %pill-->mouse;
				if (isObject (%mouseArea)) {
					%mouseArea.infoText = %tooltip;
					if (%params.mouseAreaClass !$="")
						%mouseArea.superClass = %params.mouseAreaClass;
				}

				%pill-->field.text = %fieldTitle;
				%pill-->field.internalName = "field";

				//TextEdit ctrl update
				%textEdit = %pill-->textEdit;
				%textEdit.command = %fieldCommand;
				%textEdit.altCommand = %fieldAltCommand;
				%textEdit.internalName = %fieldInternalName;
				if (%fieldValue !$= "")
					%textEdit.text = %fieldValue;


				%precision =  %fieldOption[%fieldSetting,"precision"];
				if (%precision !$="" && %fieldValue !$= "") {
					%fixValue = setFloatPrecision(%fieldValue,%precision);
					%textEdit.text = %fixValue;
					%textEdit.variable = "";
				} else
					%textEdit.variable = %fieldVariable;


				//Slider ctrl update
				%slider = %pill-->slider;
				%range = %fieldOption[%fieldSetting,"range"];
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

				foreach$(%option in %fieldOptionList[%fieldSetting])
					eval(%slider@%fieldOptionCmd[%fieldSetting,%option]);


				%slider.command = %fieldCommand;
				%slider.altCommand = %fieldAltCommand;
				%slider.internalName = %fieldInternalName@"_slider";
				%slider.variable = %fieldVariable;
				if (%fieldValue !$= "")
					%slider.setValue(%fieldValue);

				%slider.tooltip = %tooltip;
				%slider.hovertime = %tooltipDelay;
				%pill-->field.tooltip = %tooltip;
				%pill-->field.hovertime = %tooltipDelay;
				%textEdit.tooltip = %tooltip;
				%textEdit.hovertime = %tooltipDelay;
				//===========================================
			//SliderEdit Control Type Creation
			case "Slider":

				%mouseArea = %pill-->mouse;
				if (isObject (%mouseArea)) {
					%mouseArea.infoText = %tooltip;
					if (%params.mouseAreaClass !$="")
						%mouseArea.superClass = %params.mouseAreaClass;
				}

				%pill-->field.text = %fieldTitle;
				%pill-->field.internalName = "field";				


				//Slider ctrl update
				%slider = %pill-->slider;
				%range = %fieldOption[%fieldSetting,"range"];
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

				foreach$(%option in %fieldOptionList[%fieldSetting])
					eval(%slider@%fieldOptionCmd[%fieldSetting,%option]);


				%slider.command = %fieldCommand;
				%slider.altCommand = %fieldAltCommand;
				%slider.internalName = %fieldInternalName;
				%slider.variable = %fieldVariable;
				if (%fieldValue !$= "")
					%slider.setValue(%fieldValue);

				%slider.tooltip = %tooltip;
				%slider.hovertime = %tooltipDelay;
				%pill-->field.tooltip = %tooltip;
				%pill-->field.hovertime = %tooltipDelay;
			
			//===========================================
			//SliderEdit Control Type Creation
			case "SliderTripleEdit":

				%pill-->field.text = %fieldTitle;
				%pill-->field.internalName = "field";

				//TextEdit ctrl update
				%textEdit = %pill-->textEdit;
				%textEdit.command = %fieldCommand;
				%textEdit.altCommand = %fieldAltCommand;
				%textEdit.internalName = %fieldInternalName;
				if (%fieldValue !$= "")
					%textEdit.text = %fieldValue;


				%precision =  %fieldOption[%fieldSetting,"precision"];
				if (%precision !$="" && %fieldValue !$= "") {
					%fixValue = setFloatPrecision(%fieldValue,%precision);
					%textEdit.text = %fixValue;
					%textEdit.variable = "";
				} else
					%textEdit.variable = %fieldVariable;


				//Slider ctrl update
				%slider = %pill-->slider;
				%range = %fieldOption[%fieldSetting,"range"];
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

				foreach$(%option in %fieldOptionList[%fieldSetting])
					eval(%slider@%fieldOptionCmd[%fieldSetting,%option]);


				%slider.command = %fieldCommand;
				%slider.altCommand = %fieldAltCommand;
				%slider.internalName = %fieldInternalName@"_slider";
				%slider.variable = %fieldVariable;
				if (%fieldValue !$= "")
					%slider.setValue(%fieldValue);

				//Slider1 ctrl update
				%slider1 = %pill-->slider1;
				%range1 = %fieldOption[%fieldSetting,"range1"];
				if (%range1 $="") %range1 = "0 1";

				if (isObject(%pill-->minRange1)) {
					%pill-->minRange1.text = %range1.x;
					%pill-->minRange1.internalName = "";
				}
				if (isObject(%pill-->maxRange1)) {
					%pill-->maxRange1.text = %range1.y;
					%pill-->maxRange1.internalName = "";
				}

				foreach$(%option in %fieldOptionList[%fieldSetting])
					eval(%slider1@%fieldOptionCmd[%fieldSetting,%option]);

				%slider1.range = %range1;
				%slider1.command = %fieldCommand;
				%slider1.altCommand = %fieldAltCommand;
				%slider1.internalName = %fieldInternalName@"_slider1";
				%slider1.variable = %fieldVariable;
				if (%fieldValue !$= "")
					%slider1.setValue(%fieldValue);

				//Slider2 ctrl update
				%slider2 = %pill-->slider2;
				%range2 = %fieldOption[%fieldSetting,"range2"];
				if (%range2 $="") %range2 = "0 1";
				%slider2.range = %range2;
				if (isObject(%pill-->minRange2)) {
					%pill-->minRange2.text = %range2.x;
					%pill-->minRange2.internalName = "";
				}
				if (isObject(%pill-->maxRange2)) {
					%pill-->maxRange2.text = %range2.y;
					%pill-->maxRange2.internalName = "";
				}

				foreach$(%option in %fieldOptionList[%fieldSetting])
					eval(%slider2@%fieldOptionCmd[%fieldSetting,%option]);

				%slider2.range = %range2;
				%slider2.command = %fieldCommand;
				%slider2.altCommand = %fieldAltCommand;
				%slider2.internalName = %fieldInternalName@"_slider2";
				%slider2.variable = %fieldVariable;
				if (%fieldValue !$= "")
					%slider2.setValue(%fieldValue);
			//===========================================
			//SliderEdit Control Type Creation
			case "SliderText":

				%pill-->field.text = %fieldTitle;
				%pill-->field.internalName = "field";

				//TextEdit ctrl update
				%textValue = %pill-->textValue;
				%textValue.internalName = %fieldInternalName;




				//Slider ctrl update
				%slider = %pill-->slider;
				%range = %fieldOption[%fieldSetting,"range"];
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

				foreach$(%option in %fieldOptionList[%fieldSetting])
					eval(%slider@%fieldOptionCmd[%fieldSetting,%option]);

				%slider.skipPrecision = true;
				%slider.command = %fieldCommand;
				%slider.altCommand = %fieldAltCommand;
				%slider.internalName = %fieldInternalName@"_slider";
				%slider.variable = %fieldVariable;
				if (%fieldValue !$= "")
					%slider.setValue(%fieldValue);
			//===========================================
			//TextEdit Control Type Creation
			case "TextEdit":
				%pill-->field.text = %fieldTitle;
				%pill-->field.internalName = "";

				//TextEdit ctrl update
				%textEdit = %pill-->edit;
				%textEdit.command = %fieldCommand;
				%textEdit.altCommand = %fieldAltCommand;
				%textEdit.internalName = %fieldInternalName;
				%textEdit.variable = %fieldVariable;

			//===========================================
			//Checkbox Control Type Creation
			case "Checkbox":
				%pill-->field.text = %fieldTitle;
				%pill-->field.internalName = "";

				//Checkbox ctrl update
				%checkbox = %pill-->checkbox;
				%checkbox.text = "";
				%checkbox.command = %fieldCommand;
				%checkbox.altCommand = %fieldAltCommand;
				%checkbox.internalName = %fieldInternalName;
				%checkbox.variable = %fieldVariable;

			//===========================================
			//Color Control Type Creation
			case "Color":
				%pill-->field.text = %fieldTitle;
				%pill-->field.internalName = "";

				//ColorPicker ctrl update
				%colorPicker = %pill-->colorPicker;
				%colorPicker.command = %fieldCommand;
				%colorPicker.altCommand = %fieldAltCommand;
				%colorPicker.internalName = %fieldInternalName;
				%checkbox.variable = %fieldVariable;

				%noAlpha = %fieldOption[%fieldSetting,"noalpha"];
				%colorPicker.lockedAlpha = %noAlpha;

			//===========================================
			//Color Control Type Creation
			case "ColorInt":
				%pill-->field.text = %fieldTitle;
				%pill-->field.internalName = "";

				//ColorPicker ctrl update
				%colorPicker = %pill-->colorPicker;
				%colorPicker.command = %fieldCommand;
				%colorPicker.altCommand = %fieldAltCommand;
				%colorPicker.internalName = %fieldInternalName;
				%checkbox.variable = %fieldVariable;

				%noAlpha = %fieldOption[%fieldSetting,"noalpha"];
				%colorPicker.lockedAlpha = %noAlpha;
				%colorPicker.isIntColor = true;

			//===========================================
			//Color Control Type Creation
			case "ColorAlpha":
				%pill-->field.text = %fieldTitle;
				%pill-->field.internalName = "NoUpdate";

				//ColorPicker ctrl update
				%colorPicker = %pill-->colorPicker;
				%colorPicker.command = %fieldCommand;
				%colorPicker.altCommand = %fieldAltCommand;
				%colorPicker.internalName = %fieldInternalName;
				%colorPicker.variable = %fieldVariable;
				%colorPicker.alpha = "alphaSlider";

				%alphaSlider = %pill-->slider;
				%alphaSlider.command = "updateParamColorAlpha($Me);";
				%alphaSlider.altCommand = "updateParamColorAlpha($Me);";
				%alphaSlider.internalName = "alphaSlider";
				%alphaSlider.dataType = %setting;
				%alphaSlider.range = "0 1";
				%alphaSlider.noFriends = true;
				%alphaSlider.variable = %fieldVariable;
			//===========================================
			//Dropdown Control Type Creation
			case "Dropdown":
				%pill-->field.text = %fieldTitle;
				%pill-->field.internalName = "";

				eval("%value = "@%fieldVariable@";");

				//Dropdown ctrl updare
				%menu = %pill-->dropdown;
				%menu.command = %fieldCommand;
				%menu.altCommand = %fieldAltCommand;
				%menu.internalName = %fieldInternalName;
				%menu.variable = %fieldVariable;
				%menu.canSaveDynamicFields = true;




				//Update dropdown data
				%menu.clear();
				%selectedId = 0;
				%menuData =  %fieldOption[%fieldSetting,"menuData"];
				if (%menuData!$="") {
					%updType = getWord(%menuData,0);
					%updValue = getWords(%menuData,1);
					%menu.guiGroup = %updValue;
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

							%menu.add(%obj.getName(),%menuId);
							%menuId++;
						}
					}
				}


				if (%fieldOption[%fieldSetting,"guiGroup"] !$="")  %menu.guiGroup = %fieldOption[%fieldSetting,"guiGroup"];
				if (%menu.guiGroup !$= "")   UI.setCtrlGuiGroup(%menu);

				%menu.text = %value;
			//===========================================
			//Dropdown Control Type Creation
			case "DropdownIcon":
				%pill-->field.text = %fieldTitle;
				%pill-->field.internalName = "";

				//Dropdown ctrl updare
				%menu = %pill-->dropdown;
				%menu.command = %fieldCommand;
				%menu.altCommand = %fieldAltCommand;
				%menu.internalName = %fieldInternalName;
				%menu.variable = %fieldVariable;

				if ( %fieldOption[%fieldSetting,"guiGroup"] !$="") {
					%menu.guiGroup =  %fieldOption[%fieldSetting,"guiGroup"];
					UI.setCtrlGuiGroup(%menu);
				}


				//Update dropdown data
				%menu.clear();
				%menuData =  %fieldOption[%fieldSetting,"menuData"];
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
				%icon.command =  %fieldOption[%fieldSetting,"iconCommand"];
				%icon.iconBitmap =   %fieldOption[%fieldSetting,"icon"];



//===============================================================================
			}//End of switch
			%stackObj.add(%pill);


			%fid++;
		}
		%gid++;
	}

	return %paramsFieldList;
}
//------------------------------------------------------------------------------
