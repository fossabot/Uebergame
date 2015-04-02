//==============================================================================
// GameLab -> Functions to prepare data for Params Building
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Convert an array object into params
function convertArrayToParam( %array,%rebuild ) {	
	if (!isObject(%array)) {
		info("No setting group found for array CFG:",%array);
		return;
	}
   %paramName = "paramObj_"@%array.paramName;
  
	%params = newScriptObject(%paramName);		
	
   %params.style = "Default_230";
   if (%array.style !$= "")
      %params.style = %array.style ;     
	
	%params.baseGuiControl = %array.container.getName();
	
	%params.aggregateStyle = "Box";	
	%params.prefGroup = %array.prefGroup;
   %params.common["command"] = "updateParamCtrl($ThisControl,\"autoUpdateParam\",\""@%paramName@"\",\"\",\"\");";
   %params.common["altCommand"] = "updateParamCtrl($ThisControl,\"autoUpdateParam\",\""@%paramName@"\",\"\",\"\");";	
   if (%array.common["command"] !$= "")   
	   %params.common["command"] = %array.common["command"];
   if (%array.common["altCommand"] !$= "")  
	   %params.common["altCommand"] = %array.common["altCommand"];	   
	   
	%pid = 0;
	if (%array.group[1] $= "")
	   %array.group[1] = "General Settings";

	%gid = 1;
	%suffix = "";
	while(%array.group[%gid] !$= "") {
	   %stackId = getField(%array.group[%gid],1);
	   if (%stackId !$= "")
	      devLog("Using stackId for this group:",%stackId,"Group:",getField(%array.group[%gid],0));
	   		  
      %params.groupTitle[%gid] =  getField(%array.group[%gid],0);
      %params.groupStack[%gid] = "Params_Stack"@%stackId@ %suffix;
      %params.groupDisplay[%gid] = "Rollout";
		%suffix = " Append";
		%gid++;
	}
	%i = 0;
	for( ; %i < %array.count() ; %i++) {
		%field = %array.getKey(%i);
		%data = %array.getValue(%i);
		
		%groupId = getField(%data,5);
		%data = removeField(%data,5);
		%params.defaultValue[%field] = getField(%data,0);
		if (%groupId $= "") %groupId = 1;
		%paramData =  setField( %data, 0, %field); //Replace default value with field     
		if (getField(%data,2)$= ""){
		   //Skip field with no params control		 
		   %params.skipField[%field] = true;		   
		}
		else
		   %params.groupFieldData[%groupId,%pid[%groupId]++] = %paramData;
		%params.syncObjs[%field] = getField(%paramData,4);
		
		%params.fieldList = trim(%params.fieldList SPC %field);
	}
  
  return %params;	
}
//------------------------------------------------------------------------------
