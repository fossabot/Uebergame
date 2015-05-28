//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Prepare the Settings Params for specific LabSettingsDlg use
function Lab::validateLabSettingsParams( %this,%params ) {
	%gid = 1;
	while (%params.groupData[%gid] !$= "") {
		%title = getField(%params.groupData[%gid],0);
		%stack = getField(%params.groupData[%gid],1);
		%display = getField(%params.groupData[%gid],2);

		if (%stack $= "") %stack = "Params_Stack";

		if (%display $= "") %display = "Rollout";

		%params.groupData[%gid] = %title TAB %stack TAB %display;



		%pid=1;
		while (%params.groupParam[%gid,%pid] !$= "") {
			%paramData = %params.groupParam[%gid,%pid];

			%params.groupFieldData[%gid,%pid] = %paramData;
			%pid++;
		}
		%gid++;
	}
	return %params;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the Settings Params for specific LabSettingsDlg use
function Lab::prepareLabParams( %this,%params ) {
	%gid = 1;

	%baseCtrl = %params.baseGuiControl;
	if (!isObject(%baseCtrl)) {
		return;
	}
	%baseCtrl-->ButtonReset.command = "Lab.resetPatternSettings(\""@%params.pattern@"\",\""@%params.getName()@"\"); ";

	while (%params.groupData[%gid] !$= "") {
      %title = getField(%params.groupData[%gid],0);
      %stack = getField(%params.groupData[%gid],1);
      %display = getField(%params.groupData[%gid],2);
	
      %params.groupTitle[%gid] = %title;
      %params.groupStack[%gid] = %stack;
      %params.groupDisplay[%gid] = %display;

      %pid=1;
      while (%params.groupParam[%gid,%pid] !$= "") {       
         %paramData = %params.groupParam[%gid,%pid];
         %params.groupFieldData[%gid,%pid] = %paramData;
         %pid++;
      }
      %gid++;
   }

   buildLabParams(%params);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::updateSettingsParams( %this,%field,%value,%paramObj,%callback ) {
   LabCfg.beginGroup(%paramObj.pattern,true);
   LabCfg.setValue(%field,%value);
   LabCfg.endGroup();   

   if (%callback !$="")
      eval(%callback);
   
   %syncObjs = %paramObj.syncObjs[%field]; 
   for( ; %i< getFieldCount(%syncObjs);%i++){	      
      %data = getField(%syncObjs ,%i);   
       //devLog("SyncObjs for field:", %field,"Is:", %data);
      if (isObject(%data)){
         %data.setFieldValue(%field,%value);
      }
      else if (strstr(%data,");") !$= "-1"){
         //Seem to be a function so replace ** with the value         
         %function = strreplace(%data,"**",%value);	
         //devLog("SyncObjs is a function:", %function);
         eval(%function);
      }      
      else if (getSubStr(%data,0,1) $= "$"){
         //devLog("updateSettingsParams Global:",%data,"Field",%field,"Value",%value);
         //Seem to be a function so replace ** with the value   
         %data = strreplace(%data,"\"","");	      
         eval(%data@" = %value;");        
      }
      // Check for special object field name EX: EWorldEditor.stickToGround
      else if (strstr(%data,".") !$= "-1"){
         //devLog("updateSettingsParams SpecialObjField:",%data,"Field",%field,"Value",%value);
         //Seem to be a function so replace ** with the value             
         eval(%data@" = %value;");        
      }
   }
  
   if (%paramObj.prefGroup !$="")
	   $Cfg_[%paramObj.prefGroup,%field] = %value;	
}
//==============================================================================

function Lab::syncLabSettingsParams( %this,%paramObj ) {
	LabCfg.beginGroup(%paramObj.pattern);
	%container = %paramObj.baseGuiControl;
	%settingList = Lab.getPatternSettings(%paramObj.pattern);
	foreach$(%field in %settingList) {
      %ctrl = %container.findObjectByInternalName(%field,true);
      %value = LabCfg.value(%field);
    %ctrl.setTypeValue(%value);

      if (%paramObj.syncObjs[%field] !$= ""){
         foreach$(%obj in %paramObj.syncObjs[%field]){
            if (isObject(%obj)){
               %obj.setFieldValue(%field,%value);
             }
         }
      }
   }
	LabCfg.endGroup();
}
	//==============================================================================
	
//==============================================================================
function Lab::syncConfigParamField(%this,%paramObj,%field,%value) {	 
   if (%paramObj.skipField[%field]){     
      return;
   }
   %container = %paramObj.baseGuiControl;
   %ctrl = %container.findObjectByInternalName(%field,true);
   %contName = %container;
   if (%value $= ""){
     // devLog("Syncing param field with empty value",%field,"Group",%paramObj.groupLink);
      return;
   }
   if (isObject(%contName))%contName = %contName.getName();
   if (isObject(%ctrl)){   
      %ctrl.setTypeValue(%value);
   }
   else
      warnLog("Couldn't find a control that hold value for this field:",%field,"in container",%contName);		
	
}
//------------------------------------------------------------------------------
