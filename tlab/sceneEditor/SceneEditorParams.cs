//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SceneEditorPlugin::initDefaultCfg( %this,%cfgArray )
{  	
   %cfgArray.group[%groupId++] = "General settings";
   %cfgArray.setVal("DefaultWidth",       "10" TAB "Default Width" TAB "SliderEdit" TAB "range::0 100;tickAt 1" TAB "" TAB %groupId);
   %cfgArray.setVal("DefaultWidth",       "10" TAB "Default Width" TAB "SliderEdit" TAB "range::0 100;tickAt 1" TAB "" TAB %groupId);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SceneEditorPlugin::initParamsArray( %this,%array )
{  
	$SceneEdCfg = newScriptObject("SceneEditorCfg");
   %array.group[%groupId++] = "General settings";
 
   %array.setVal("DefaultWidth",       "10" TAB "Default Width" TAB "SliderEdit"  TAB "range>>0 100;;tickAt>>1" TAB "SceneEditorCfg" TAB %groupId);
   %array.setVal("DropLocation",       "10" TAB "Drop object location" TAB "Dropdown"  TAB "itemList>>$TLab_Object_DropTypes" TAB "SceneEditorCfg" TAB %groupId);
}
//------------------------------------------------------------------------------
/*==============================================================================
arSceneEditorCfg
//------------------------------------------------------------------------------
$Cfg_Plugins_SceneEditor_DefaultWidth = "10";
$Cfg_Plugins_SceneEditor_isEnabled = "1";
$Cfg_Plugins_SceneEditor_pluginOrder = "1";
//-----------------------------------------------------------------------------*/
