//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function Lab::initConfigSystem( %this ) {	
   //Start by building all the ConfigArray Params
   $LabConfigArrayGroup = newSimGroup("LabConfigArrayGroup");
   
   %this.initCommonParams(); 
	%this.initCommonSettings(); //FIXME Old param system
   %this.initAllPluginConfig();
   Lab.buildAllConfigParams();
	 LabCfg.read();	
	Lab.readAllConfigArray();
	%this.initAllConfigArray();	
}
//------------------------------------------------------------------------------



//==============================================================================
//Initialize plugin data
function Lab::initObjectConfigArray(%this,%obj,%name,%type,%dontBuildParams) {	   
	if (%obj.isMethod("initDefaultCfg")) {
	   %array = %this.createConfigArray(%name,%type);
      
      if (!isObject(%array))
         return;
         
		%obj.initDefaultCfg(%array);
		//%this.initConfigArray( %array,!%dontBuildParams);		
	}
}
//------------------------------------------------------------------------------

//==============================================================================
//Initialize plugin data
function Lab::createConfigArray(%this,%name,%type) {
   if (%type $= "")
	   %type = "Default";
   if (%name $= ""){
      warnLog("You need to specify a name for the settings which is unique in this type");
	  return;
   }
		%array = newArrayObject("ar"@%name@"Cfg",LabConfigArrayGroup);
		%array.srcObj =%obj;
		%array.groupLink = %type@"_"@%name;
		%array.groupLinkName = %name SPC "Settings";
		%array.internalName = %name;
		
   return %array;		
}
//------------------------------------------------------------------------------
//==============================================================================

function Lab::initAllConfigArray(%this) {
   foreach(%array in LabConfigArrayGroup)
		%this.initConfigArray(%array);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::initConfigArray( %this,%array,%buildParams ) {

	if (!isObject(%array)) {
		info("No setting group found for array Cfg:",%array.internalName);
		return;
	}

	%pattern = strreplace(%array.groupLink,"_","/");
	LabCfg.beginGroup( %pattern,true );
	%i = 0;
	for( ; %i < %array.count() ; %i++) {
		%field = %array.getKey(%i);
		%data = %array.getValue(%i);
		%default =  getField(%data,0);
		Lab.addDefaultSetting(  %field, %default );
	}

	LabCfg.endGroup();
	if (%buildParams)
	   %this.buildArrayCfgParams(%array);

}
//------------------------------------------------------------------------------
//==============================================================================

function Lab::readAllConfigArray(%this) {
   foreach(%array in LabConfigArrayGroup)
		%this.readConfigArray(%array);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::readConfigArray(%this,%array) {	
	%pattern = strreplace(%array.groupLink,"_","/");	
	LabCfg.beginGroup( %pattern, true );
	%i = 0;
	for( ; %i < %array.count() ; %i++) {
		%field = %array.getKey(%i);
		
		%value = LabCfg.value( %field );	
		$Cfg_[%array.groupLink,%field] = %value;		
	}
	LabCfg.endGroup();
	
	%i = 0;
	for( ; %i < %array.count() ; %i++) {
		%field = %array.getKey(%i);
		%value = $Cfg_[%array.groupLink,%field];			
		%this.updateConfigFieldValue(%array,%field,%value);	
	}
}
//------------------------------------------------------------------------------
//==============================================================================

function Lab::updateConfigFieldValue(%this,%array,%field,%value) {
   %paramObj = %array.paramObj;
   if (!isObject(%paramObj)){
      warnLog("No Parameter object found for this config object:",%array.getName());
      return;
   }			
	
   %this.updateSettingsParams( %field,%value,%paramObj);
   %this.syncConfigParamField(%paramObj,%field,%value);
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::storeArrayCfg(%this,%array) {
	if (!isObject(%array)) {
		info("No setting group found for array Cfg:",%array.internalName);
		return;
	}

	%pattern = strreplace(%array.groupLink,"_","/");
	LabCfg.beginGroup( %pattern, true );
	%i = 0;
	for( ; %i < %array.count() ; %i++) {
		%field = %array.getKey(%i);
		LabCfg.setValue( %field , $Cfg_[%array.groupLink,%field]);
	}
	LabCfg.endGroup();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::exportCfgPrefs(%this) {
	export("$Cfg_*", "tlab/configPrefs.cs", false);
}
//------------------------------------------------------------------------------