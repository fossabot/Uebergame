//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
/*
%cfgArray.setVal("FIELD",       "DEFAULT" TAB "TEXT" TAB "TextEdit" TAB "" TAB "");
*/
//==============================================================================

function RoadEditorPlugin::initDefaultCfg( %this,%cfgArray )
{  
   %cfgArray.group[1] = "General settings";
   %cfgArray.group[2] = "Color settings";
   %cfgArray.group[3] = "Console settings";
   %cfgArray.setVal("DefaultWidth",       "10" TAB "Default Width" TAB "SliderEdit" TAB "range::0 100;tickAt 1" TAB "RoadEditorGui" TAB "1");
   %cfgArray.setVal("HoverSplineColor",   "255 0 0 255" TAB "HoverSplineColor" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB "2");
   %cfgArray.setVal("SelectedSplineColor","0 255 0 255" TAB "SelectedSplineColor" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB "2");
   %cfgArray.setVal("HoverNodeColor",       "255 255 255 255" TAB "HoverNodeColor" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB "2");
   %cfgArray.setVal("borderMovePixelSize",       "20" TAB "borderMovePixelSize" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB "1");
   %cfgArray.setVal("borderMoveSpeed",       "0.1" TAB "borderMoveSpeed" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB "1");
   %cfgArray.setVal("MaterialName",       "DefaultDecalRoadMaterial" TAB "MaterialName" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB "1");
   %cfgArray.setVal("consoleFrameColor",       "255 0 0 255" TAB "consoleFrameColor" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB "3");
   %cfgArray.setVal("consoleFillColor",       "0 0 0 0" TAB "consoleFillColor" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB "3");
   %cfgArray.setVal("consoleSphereLevel",       "1" TAB "consoleSphereLevel" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB "3");
   %cfgArray.setVal("consoleCircleSegments",       "32" TAB "consoleCircleSegments" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB "3");
   %cfgArray.setVal("consoleLineWidth",       "1" TAB "consoleLineWidth" TAB "TextEdit" TAB "" TAB "RoadEditorGui" TAB "3");
}