//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Prepare the Settings Params for specific LabSettingsDlg use
function Lab::regenerateSettings( %this ) {
   LabSettingTree.clear();
	LabSettingsDlg.clearSettingsContainer();
	Lab.buildAllParams(true);
	LabSettingTree.buildVisibleTree();	
}

//==============================================================================
// Prepare the Settings Params for specific LabSettingsDlg use
function Lab::buildAllParams( %this,%rebuild ) {

	//%this.buildGeneralParams(%rebuild);
	%this.reloadAllSettings();
	%this.buildAllConfigParams();
}
//==============================================================================
// Prepare the Settings Params for specific LabSettingsDlg use
function Lab::buildAllConfigParams( %this,%setDefault ) {   
   LabSettingTree.clear();
	LabSettingsDlg.clearSettingsContainer();
	foreach(%array in LabConfigArrayGroup){
		%this.buildArrayCfgParams(%array,true);
		if (%setDefault)
         %this.setConfigArrayDefault(%array);
	}
}
//==============================================================================
// Prepare the Settings Params for specific LabSettingsDlg use
function Lab::buildArrayCfgParams( %this,%array,%rebuild ) {
	
	if (!isObject(%array)) {
		info("No setting group found for array CFG:",%array);
		return;
	}
	%groupLink = %array.groupLink;

	%pattern = strreplace(%groupLink,"_","/");
	%groupTypeStr = strreplace(%groupLink,"_"," ");
	%group = getWord(%groupTypeStr,0);
	%type = getWord(%groupTypeStr,1);

	%params = newScriptObject("LabParam_"@%groupLink);
	%params.type = %type;
	%params.group = %group;
	%params.groupLink = %groupLink;
	%params.pattern =  %pattern;
	%params.prefGroup = %groupLink;
	%container = "lsContainer_"@%groupLink;
	%params.configArray = %array;
   %array.paramObj = %params;
	//if (%rebuild)
		delObj(%container);

	if (!isObject(%container)) {
		%newContainer = cloneObject(LS_SampleContainer);
		%newContainer.setName(%container);
		%newContainer.internalName = %groupLink;
		LS_SettingsContainer.add(%newContainer);
		%container = %newContainer;
	}


	%container-->settingsTitle.text = %array.groupLinkName;
	%params.baseGuiControl = %container.getName();
	%params.aggregateStyle = "Box";
	%params.style = "Default_230";
	%params.mouseAreaClass = "LabSettingsParamsMouse";
	%params.tooltipDelay = "2000";

	%params.common["command"] = "updateParamCtrl($ThisControl,\"Lab.updateSettingsParams\",\"LabParam_"@%groupLink@"\",\"\",\"\");";
	%params.common["altCommand"] = "updateParamCtrl($ThisControl,\"Lab.updateSettingsParams\",\"LabParam_"@%groupLink@"\",\"\",\"\");";
	%pid = 0;
	%params.groupData[1] = %array.groupLinkName TAB "Params_Stack" TAB "Rollout";

	%gid = 1;
	%suffix = "";
	while(%array.group[%gid] !$= "") {	
	   //devLog("%array.group[%gid]",%array.group[%gid]);
		%params.groupData[%gid] = %array.group[%gid] TAB "Params_Stack" @ %suffix TAB "Rollout";
		%suffix = " Append";
		%gid++;
	}
	%i = 0;
	for( ; %i < %array.count() ; %i++) {
		%field = %array.getKey(%i);
		%data = %array.getValue(%i);
		//Skip field with no params control
		
		%groupId = getField(%data,5);
		%data = removeField(%data,5);
		%params.defaultValue[%field] = getField(%data,0);
		if (%groupId $= "") %groupId = 1;
		%paramData =  setField( %data, 0, %field); //Replace default value with field     
		if (getField(%data,2)$= ""){		 
		   %params.skipField[%field] = true;		   
		}
		else
		   %params.groupParam[%groupId,%pid[%groupId]++] = %paramData;
		%params.syncObjs[%field] = getField(%paramData,4);
		
	}

	//Validate the ParamsObject
	%params = Lab.validateLabSettingsParams(%params);
	%array.pluginObj.paramObj = %array.paramObj;
	%this.addParamToTree(%params);
	%this.prepareLabParams(%params);
  
	//%this.syncLabSettingsParams(%params);	
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setConfigArrayDefault(%this,%array) {	
	%pattern = strreplace(%array.groupLink,"_","/");
	
	%i = 0;
	for( ; %i < %array.count() ; %i++) {
		%field = %array.getKey(%i);
		%data = %array.getValue(%i);
		%default = getField(%data,0);
		$Cfg_[%array.groupLink,%field] = %value;
		
		%this.updateConfigFieldValue(%array,%field,%default);		
	}
}
//------------------------------------------------------------------------------

