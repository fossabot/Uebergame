//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function LabModelPlugin::initParamsArray( %this,%cfgArray ) {
	$LabModelCfg = newScriptObject("LabModelCfg");
	%cfgArray.group[1] = "General settings";
	%cfgArray.setVal("BackgroundColor",       "0 0 0 100" TAB "BackgroundColor" TAB "TextEdit" TAB "" TAB "LabModelPreviewGui-->previewBackground.color" TAB "1");
	%cfgArray.setVal("HighlightMaterial",   "1" TAB "HighlightMaterial" TAB "TextEdit" TAB "" TAB "" TAB "1");
	%cfgArray.setVal("ShowNodes","1" TAB "ShowNodes" TAB "TextEdit" TAB "" TAB "" TAB "1");
	%cfgArray.setVal("ShowBounds",       "0" TAB "ShowBounds" TAB "TextEdit" TAB "" TAB "" TAB "1");
	%cfgArray.setVal("ShowObjBox",       "1" TAB "ShowObjBox" TAB "TextEdit" TAB "" TAB "" TAB "1");
	%cfgArray.setVal("RenderMounts",       "1" TAB "RenderMounts" TAB "TextEdit" TAB "" TAB "" TAB "1");
	%cfgArray.setVal("RenderCollision",       "0" TAB "RenderCollision" TAB "TextEdit" TAB "" TAB "" TAB "1");
	%cfgArray.setVal("AdvancedWndVisible",       "1" TAB "RenderCollision" TAB "TextEdit" TAB "" TAB "" TAB "1");
	%cfgArray.group[2] = "Grid settings";
	%cfgArray.setVal("ShowGrid",       "1" TAB "ShowGrid" TAB "TextEdit" TAB "" TAB "LabModelPreview" TAB "2");
	%cfgArray.setVal("GridSize",       "0.1" TAB "GridSize" TAB "TextEdit" TAB "" TAB "LabModelPreview" TAB "2");
	%cfgArray.setVal("GridDimension",       "40 40" TAB "GridDimension" TAB "TextEdit" TAB "" TAB "LabModelPreview" TAB "2");
	%cfgArray.group[3] = "Sun settings";
	%cfgArray.setVal("SunDiffuseColor",       "255 255 255 255" TAB "SunDiffuseColor" TAB "TextEdit" TAB "" TAB "LabModelPreview" TAB "3");
	%cfgArray.setVal("SunAmbientColor",       "180 180 180 255" TAB "SunAmbientColor" TAB "TextEdit" TAB "" TAB "LabModelPreview" TAB "3");
	%cfgArray.setVal("SunAngleX",       "45" TAB "SunAngleX" TAB "TextEdit" TAB "" TAB "LabModelPreview" TAB "3");
	%cfgArray.setVal("SunAngleZ",       "135" TAB "SunAngleZ" TAB "TextEdit" TAB "" TAB "LabModelPreview" TAB "3");
}
