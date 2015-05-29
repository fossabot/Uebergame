//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//------------------------------------------------------------------------------
function MaterialEditorPlugin::initDefaultCfg( %this,%cfgArray )
{  
   %cfgArray.group[%groupId++] = "Display properties options";
   %cfgArray.setVal("PropShowMap_specular", "10" TAB "PropShowMap_specular" TAB "Checkbox" TAB "" TAB "" TAB %groupId);
   %cfgArray.setVal("PropShowMap_Normal",   "255 0 0 255" TAB "PropShowMap_Normal" TAB "Checkbox" TAB "" TAB "" TAB %groupId);
   %cfgArray.setVal("PropShowMap_Detail","0 255 0 255" TAB "PropShowMap_Detail" TAB "Checkbox" TAB "" TAB "" TAB %groupId);
   %cfgArray.setVal("PropShowGroup_animation", "10" TAB "PropShowGroup_animation" TAB "Checkbox" TAB "" TAB "" TAB %groupId);
   %cfgArray.setVal("PropShowMap_advanced",   "255 0 0 255" TAB "PropShowMap_advanced" TAB "Checkbox" TAB "" TAB "" TAB %groupId);
   %cfgArray.setVal("PropShowMap_rendering","0 255 0 255" TAB "PropShowMap_rendering" TAB "Checkbox" TAB "" TAB "" TAB %groupId);
   %cfgArray.setVal("ThumbnailCountIndex","1" TAB "ThumbnailCountIndex" TAB "TextEdit" TAB "" TAB "$Pref::MaterialSelector::ThumbnailCountIndex" TAB %groupId);
}
