//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamsObject( %params ) {

	%widgetsControl = "wParams_"@%params.style;

	if ( %params.aggregateStyle $= "")
		%aggregateClass = "AggregateGlobal";
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
			%pData.Count = getFieldCount(%displayOptions);
			for(%i=0;%i<%pData.Count;%i++){
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
		    %pData = newScriptObject("paramDataHolder"); 
         
			%setting = %params.groupStack[%gid];

			%command = %params.common["Command"];
			%altCommand = %params.common["AltCommand"];
        
			if ( %params.groupCommand[%gid] !$= "")
				%command =  %params.groupCommand[%gid];
			if ( %params.groupAltCommand[%gid] !$= "")
				%altCommand =  %params.groupAltCommand[%gid];

			%pData.Setting = getField(%params.groupFieldData[%gid,%fid],0);
			%pData.Title = getField(%params.groupFieldData[%gid,%fid],1);
			%pData.Type = getField(%params.groupFieldData[%gid,%fid],2);
			%pData.Options = getField(%params.groupFieldData[%gid,%fid],3);
			%pData.Command = %command;
			%pData.AltCommand = %altCommand;

			if (%pData.Title $= "") %pData.Title = %pData.Setting;
			%pData.InternalName = %pData.Setting;
			%pData.Variable = "";
			if (getSubStr(%pData.InternalName,0,1) $= "$") {
				%pData.Variable = %pData.InternalName;
				%pData.InternalName = strreplace(%pData.InternalName,"$","");
				%pData.InternalName = strreplace(%pData.InternalName,"::","__");
				eval("%pData.Value = "@%pData.Variable@";");
			}

			   
			%widgetSource = %pData.Type;
			if (%widgetSource $= "ColorInt")
				%widgetSource = "Color";

			%pData.Widget = %widgetsControl.findObjectByInternalName(%widgetSource);
			if(!isObject(%pData.Widget)) {
				warnLog("Couldn't find widget for setting type:",%pData.Type,"This setting building is skipped WidgetSource:",%widgetsControl.getName());
				%fid++;
				continue;
			}

			%paramsFieldList = trim(%paramsFieldList SPC %pData.Setting);
			%pData.pill = cloneObject(%pData.Widget);
			%pData.Category = %pData.Type;
			if (%pData.pill.paramType !$= "") %pData.Category = %pData.pill.paramType;

			%pData.pill.class = %aggregateClass;
			//------------------------------------------------
			//Prepare the field special options
			// The options are applied for each Field Category alone
			%pData.OptionList[%pData.Setting] = "";
			%pData.Options = strreplace(%pData.Options,";;","\t");
			%pData.Count = getFieldCount(%pData.Options);
			for(%i=0; %i<%pData.Count; %i++) {
				%option = getField(%pData.Options ,%i);
				%optWords = strreplace(%option,"::"," ");
				%optField = getWord(%optWords,0);
				%optCmd = getWords(%optWords,1);
				%pData.Option[%pData.Setting,%optField] = %optCmd;
				%pData.OptionCmd[%pData.Setting,%optField] = "."@%optField@" = \""@ %optCmd  @"\";";
				%pData.OptionList[%pData.Setting] = trim(%pData.OptionList[%pData.Setting] SPC %optField);
			}
			%tmpFieldValue = %pData.Value;
			%multiplier = 1;
			%tooltip = %pData.Option[%pData.Setting,"tooltip"];
			if (%tooltip !$= "" && %params.tooltipDelay !$= "")
				%tooltipDelay = %params.tooltipDelay;
			else
				%tooltipDelay = 1000;
				
         	%pData.mouseAreaClass = %params.mouseAreaClass;
         	
         if (isFunction("buildParam"@%pData.Category))
            eval("buildParam"@%pData.Category@"(%pData);");
         else
            warnLog("Couldn't create the param, there's no function for that control type:",%pData.Category);
            
         /*
			switch$( %pData.Category) {
			//===========================================
			//SliderEdit Control Type Creation
			case "SliderEdit":
         buildParamSliderEdit(%pData);
		
				//===========================================
			//SliderEdit Control Type Creation
			case "Slider":
            buildParamSlider(%pData);
				
			
			//===========================================
			//SliderEdit Control Type Creation
			case "SliderTripleEdit":
             buildParamSliderTripleEdit(%pData);
				
			//===========================================
			//SliderEdit Control Type Creation
			case "SliderText":
             buildParamSliderText(%pData);
				
			//===========================================
			//TextEdit Control Type Creation
			case "TextEdit":
			buildParamTextEdit(%pData);
				
			//===========================================
			//Checkbox Control Type Creation
			case "Checkbox":
				 buildParamCheckbox(%pData);

			//===========================================
			//Color Control Type Creation
			case "Color":
				buildParamColor(%pData);

			//===========================================
			//Color Control Type Creation
			case "ColorInt":
				buildParamColorInt(%pData);
			//===========================================
			//Color Control Type Creation
			case "ColorAlpha":
			buildParamColorAlpha(%pData);
			
			//===========================================
			//Dropdown Control Type Creation
			case "Dropdown":			  
			                 
                buildParamDropdown(%pData);
         
			//===========================================
			//Dropdown Control Type Creation
			case "DropdownIcon":
				buildParamDropdownIcon(%pData);



//===============================================================================
			}//End of switch
			
			*/
			
			%stackObj.add(%pData.pill);


			%fid++;
		}
		%gid++;
	}

	return %paramsFieldList;
}
//------------------------------------------------------------------------------
