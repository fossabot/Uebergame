//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
/*
%cfgArray.setVal("FIELD",       "DEFAULT" TAB "TEXT" TAB "TextEdit" TAB "" TAB "");
*/
//==============================================================================

function ForestEditorPlugin::initDefaultCfg( %this,%cfgArray )
{  
   %cfgArray.setVal("BrushSize",       "2" TAB "BrushSize" TAB "TextEdit" TAB "" TAB "ForestEditorPlugin");
   %cfgArray.setVal("GlobalScale",     "1" TAB "GlobalScale" TAB "TextEdit" TAB "" TAB "ForestEditorPlugin");
   %cfgArray.setVal("DefaultBrush",    "BaseBrush" TAB "DefaultBrush" TAB "TextEdit" TAB "" TAB "ForestEditorPlugin");
}
