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
   
}
//------------------------------------------------------------------------------