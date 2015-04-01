//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function RoadEditorPlugin::initDefaultCfg( %this,%cfgArray )
{  
   %cfgArray.group[1] = "General settings";
   %cfgArray.group[2] = "Color settings";
   %cfgArray.group[3] = "Console settings";
   %cfgArray.setVal("DefaultWidth",       "10" TAB "DefaultWidth" TAB "TextEdit" TAB "" TAB "RiverEditorGui" TAB "1");
   %cfgArray.setVal("DefaultDepth",   "5" TAB "DefaultDepth" TAB "TextEdit" TAB "" TAB "RiverEditorGui" TAB "2");
   %cfgArray.setVal("DefaultNormal","0 0 1" TAB "DefaultNormal" TAB "TextEdit" TAB "" TAB "RiverEditorGui" TAB "2");
   %cfgArray.setVal("HoverSplineColor",       "255 0 0 255" TAB "HoverSplineColor" TAB "ColorInt" TAB "" TAB "RiverEditorGui" TAB "2");
   %cfgArray.setVal("SelectedSplineColor",       "0 255 0 255" TAB "SelectedSplineColor" TAB "ColorInt" TAB "" TAB "RiverEditorGui" TAB "1");
   %cfgArray.setVal("HoverNodeColor",       "255 255 255 255" TAB "HoverNodeColor" TAB "ColorInt" TAB "" TAB "RiverEditorGui" TAB "1");

}
