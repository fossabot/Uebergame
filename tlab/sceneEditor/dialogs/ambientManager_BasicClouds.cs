//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
//SEP_ScatterSkyManager.buildParams();
function SEP_AmbientManager::buildBasicCloudsParams( %this ) {
	%arCfg = createParamsArray("SEP_BasicClouds",SEP_BasicCloudsProperties);
	%arCfg.updateFunc = "SEP_AmbientManager.updateBasicCloudsParam";
	%arCfg.style = "LabCfgB_230";
	%arCfg.useNewSystem = true;

	%arCfg.group[%gid++] = "Cloud Layer #1" TAB "Stack StackA";

	%arCfg.setVal("layerEnabled[0]",       "" TAB "Enabled" TAB "Checkbox" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texture[]",       "" TAB "Texture" TAB "FileSelect" TAB "callback>>SEP_AmbientManager.getBasicCloudTexture(0);" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texScale[0]",       "" TAB "Layer scale" TAB "SliderEdit" TAB "range>>0 5" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texDirection[0]",       "" TAB "Layer direction" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texSpeed[0]",       "" TAB "Layer speed" TAB "SliderEdit" TAB "range>>0 5" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texOffset[0]",       "" TAB "Layer offset" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("height[0]",       "" TAB "Layer heigth" TAB "SliderEdit" TAB "range>>0 10;;tickAt 1" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);

	%arCfg.group[%gid++] = "Cloud Layer #2" TAB "Stack StackA";

	%arCfg.setVal("layerEnabled[1]",       "" TAB "Enabled" TAB "Checkbox" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texture[1]",       "" TAB "Texture" TAB "FileSelect" TAB "callback>>SEP_AmbientManager.getBasicCloudTexture(1);" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texScale[1]",       "" TAB "Layer scale" TAB "SliderEdit" TAB "range>>0 5" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texDirection[1]",       "" TAB "Layer direction" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texSpeed[1]",       "" TAB "Layer speed" TAB "SliderEdit" TAB "range>>0 5" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texOffset[1]",       "" TAB "Layer offset" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("height[1]",       "" TAB "Layer heigth" TAB "SliderEdit" TAB "range>>0 10;;tickAt 1" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	
	%arCfg.group[%gid++] = "Cloud Layer #3" TAB "Stack StackB";

	%arCfg.setVal("layerEnabled[2]",       "" TAB "Enabled" TAB "Checkbox" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texture[2]",       "" TAB "Texture" TAB "FileSelect" TAB "callback>>SEP_AmbientManager.getBasicCloudTexture(2);" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texScale[2]",       "" TAB "Layer scale" TAB "SliderEdit" TAB "range>>0 5" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texDirection[2]",       "" TAB "Layer direction" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texSpeed[2]",       "" TAB "Layer speed" TAB "SliderEdit" TAB "range>>0 5" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("texOffset[2]",       "" TAB "Layer offset" TAB "TextEdit" TAB "" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	%arCfg.setVal("height[2]",       "" TAB "Layer heigth" TAB "SliderEdit" TAB "range>>0 10;;tickAt 1" TAB "SEP_AmbientManager.selectedBasicClouds" TAB %gid);
	buildParamsArray(%arCfg,false);
	%this.BasicCloudsParamArray = %arCfg;
}
//------------------------------------------------------------------------------

//==============================================================================
function SEP_AmbientManager::updateBasicCloudsParam(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3) {
	logd("SEP_AmbientManager::updateBasicCloudsParam(%this,%field,%value,%ctrl,%arg1,%arg2,%arg3)",%this,%field,%value,%ctrl,%arg1,%arg2,%arg3);
	%fieldData = strreplace(%field,"["," ");
	%fieldData = strreplace(%fieldData,"]","");
	%this.updateBasicCloudField(getWord(%fieldData,0),%value, getWord(%fieldData,1));
}
//------------------------------------------------------------------------------


//==============================================================================
// Sync the current profile values into the params objects
function SEP_AmbientManager::getBasicCloudTexture( %this,%layer ) { 
	%this.currentCloudLayer = %layer;
   %currentFile = $LGM_SelectedObject.bitmap;
   getLoadFilename("*.*|*.*", "SEP_AmbientManager.setBasicCloudTexture", %currentFile);
}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function SEP_AmbientManager::setBasicCloudTexture( %this,%file ) { 
   
  %filename = makeRelativePath( %file, getMainDotCsDir() );   
  %layer = %this.currentCloudLayer;
   %this.updateBasicCloudField("texture",%filename,%layer);

}
//------------------------------------------------------------------------------
//==============================================================================
// Sync the current profile values into the params objects
function SEP_AmbientManager::updateBasicCloudField( %this,%field, %value,%layerId ) { 
	devLog("SEP_AmbientManager::updateBasicCloudField( %this,%field, %value,%layerId )",%this,%field, %value,%layerId );
 	%obj = %this.selectedBasicClouds;

	if (!isObject(%obj)) {
		warnLog("Can't update ground cover value because none is selected. Tried wth:",%obj);
		return;
	}

	%currentValue = %obj.getFieldValue(%field,%layerId);

	if (%currentValue $= %value) {		
		return;
	}
	
	
	//eval("%obj."@%checkField@" = %value;");
	%obj.setFieldValue(%field,%value,%layerId);
	EWorldEditor.isDirty = true;
	%this.setBasicCloudsDirty(true);  
	BasicCloudsInspector.refresh();
	BasicCloudsInspector.apply();
	syncParamArray(SEP_AmbientManager.BasicCloudsParamArray);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::initBasicCloudsData( %this ) {
	%basicCloudsList = Lab.getMissionObjectClassList("BasicClouds");
	%this.selectBasicClouds(getWord(%basicCloudsList,0));	
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::selectBasicClouds(%this,%obj) {
	logd("SEP_AmbientManager::selectBasicClouds(%this,%obj)",%this,%obj);

	if (!isObject(%obj)) {
		%this.selectedScatterSky = "";
		%this.selectedScatterSkyName = "";
		return;
	}
	
	%this.selectedBasicClouds = %obj;
	%this.selectedBasicCloudsName = %obj.getName();
	%this.setBasicCloudsDirty();
	BasicCloudsInspector.inspect(	%obj);
	syncParamArray(%this.BasicCloudsParamArray);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::saveBasicCloudsObject(%this) {
	logd("SEP_AmbientManager::saveBasicCloudsObject(%this)",%this);
	%obj = %this.selectedBasicClouds;

	if (!isObject(%obj)) {
		warnLog("Can't save BasicClouds because none is selected. Tried wth:",%obj);
		return;
	}

	if (!SEP_AmbientManager_PM.isDirty(%obj)) {
		warnLog("Object is not dirty, nothing to save");
		return;
	}

	//SEP_AmbientManager_PM.setDirty(%obj);
	SEP_AmbientManager_PM.saveDirtyObject(%obj);
	%this.setBasicCloudsDirty(false);
}
//------------------------------------------------------------------------------
//==============================================================================
function SEP_AmbientManager::setBasicCloudsDirty(%this,%isDirty) {
	logd("SEP_AmbientManager::setBasicCloudsDirty(%this,%isDirty)",%this,%isDirty);
	%obj = %this.selectedBasicClouds;

	if (%isDirty $="")
		%isDirty = SEP_AmbientManager_PM.isDirty(%obj);
	else if ( !SEP_AmbientManager_PM.isDirty(%obj) && %isDirty)
		SEP_AmbientManager_PM.setDirty( %obj );
	else if ( SEP_AmbientManager_PM.isDirty(%obj) && !%isDirty)
		SEP_AmbientManager_PM.removeDirty( %obj );

	%this.isDirty = %isDirty;
	SEP_BasicCloudsSaveButton.active = %isDirty;
}
//------------------------------------------------------------------------------


//==============================================================================
/*
layerEnabled
texture
texScale
texDirection
texSpeed
texOffset
height
 
*/
