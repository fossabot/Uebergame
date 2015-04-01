//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function ShapeEditorPlugin::initDefaultCfg( %this,%cfgArray )
{  
   %cfgArray.group[1] = "General settings";   

   %cfgArray.setVal("BackgroundColor",       "0 0 0 100" TAB "BackgroundColor" TAB "TextEdit" TAB "" TAB "ShapeEdPreviewGui-->previewBackground.color" TAB "1");
   %cfgArray.setVal("HighlightMaterial",   "1" TAB "HighlightMaterial" TAB "TextEdit" TAB "" TAB "" TAB "1");
   %cfgArray.setVal("ShowNodes","1" TAB "ShowNodes" TAB "TextEdit" TAB "" TAB "" TAB "1");
   %cfgArray.setVal("ShowBounds",       "0" TAB "ShowBounds" TAB "TextEdit" TAB "" TAB "" TAB "1");
   %cfgArray.setVal("ShowObjBox",       "1" TAB "ShowObjBox" TAB "TextEdit" TAB "" TAB "" TAB "1");
   %cfgArray.setVal("RenderMounts",       "1" TAB "RenderMounts" TAB "TextEdit" TAB "" TAB "" TAB "1");
   %cfgArray.setVal("RenderCollision",       "0" TAB "RenderCollision" TAB "TextEdit" TAB "" TAB "" TAB "1");
   %cfgArray.setVal("AdvancedWndVisible",       "1" TAB "RenderCollision" TAB "TextEdit" TAB "" TAB "" TAB "1");
   
   %cfgArray.group[2] = "Grid settings";
   %cfgArray.setVal("ShowGrid",       "1" TAB "ShowGrid" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB "2");
   %cfgArray.setVal("GridSize",       "0.1" TAB "GridSize" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB "2");
   %cfgArray.setVal("GridDimension",       "40 40" TAB "GridDimension" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB "2");
   
   %cfgArray.group[3] = "Sun settings";
   %cfgArray.setVal("SunDiffuseColor",       "255 255 255 255" TAB "SunDiffuseColor" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB "3");
   %cfgArray.setVal("SunAmbientColor",       "180 180 180 255" TAB "SunAmbientColor" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB "3");
   %cfgArray.setVal("SunAngleX",       "45" TAB "SunAngleX" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB "3");
   %cfgArray.setVal("SunAngleZ",       "135" TAB "SunAngleZ" TAB "TextEdit" TAB "" TAB "ShapeEdShapeView" TAB "3");
}

function ShapeEditorPlugin::initDefaultSettings( %this ) {

    Lab.addDefaultSetting( "BackgroundColor",    "0 0 0 100" );
    Lab.addDefaultSetting( "HighlightMaterial", 1 );
    Lab.addDefaultSetting( "ShowNodes", 1 );
    Lab.addDefaultSetting( "ShowBounds", 0 );
    Lab.addDefaultSetting( "ShowObjBox", 1 );
    Lab.addDefaultSetting( "RenderMounts", 1 );
    Lab.addDefaultSetting( "RenderCollision", 0 );

    // Grid
    Lab.addDefaultSetting( "ShowGrid", 1 );
    Lab.addDefaultSetting( "GridSize", 0.1 );
    Lab.addDefaultSetting( "GridDimension", "40 40" );

    // Sun
    Lab.addDefaultSetting( "SunDiffuseColor",    "255 255 255 255" );
    Lab.addDefaultSetting( "SunAmbientColor",    "180 180 180 255" );
    Lab.addDefaultSetting( "SunAngleX",          "45" );
    Lab.addDefaultSetting( "SunAngleZ",          "135" );

    // Sub-windows
    Lab.addDefaultSetting( "AdvancedWndVisible",   "1" );
}

//==============================================================================
// Automated editor plugin setting interface
function ShapeEditorPlugin::buildParams(%this,%params ) {

//-------------------------------------------------
// Group 1 Configuration
    %gid++;
    %pid = 0;
    %params.groupData[%gid] = "Display options" TAB "Params_Stack" TAB "Rollout";
    %params.groupParam[%gid,%pid++] = "BackgroundColor"  TAB "" TAB "Color";
    %params.groupParam[%gid,%pid++] = "HighlightMaterial"  TAB "" TAB "Checkbox";
    %params.groupParam[%gid,%pid++] = "ShowNodes"  TAB "" TAB "Checkbox";
    %params.groupParam[%gid,%pid++] = "ShowBounds"  TAB "" TAB "Checkbox";
    %params.groupParam[%gid,%pid++] = "ShowObjBox"  TAB "" TAB "Checkbox";
    %params.groupParam[%gid,%pid++] = "RenderMounts"  TAB "" TAB "Checkbox";
    %params.groupParam[%gid,%pid++] = "RenderCollision"  TAB "" TAB "Checkbox";
    %params.groupParam[%gid,%pid++] = "AdvancedWndVisible"  TAB "Advanced window" TAB "Checkbox";

//-------------------------------------------------
// Group 2 Configuration
    %gid++;
    %pid = 0;
    %params.groupData[%gid] = "Grid options" TAB "Params_Stack Append" TAB "Rollout";
    %params.groupParam[%gid,%pid++] = "ShowGrid"  TAB "" TAB "Checkbox";
    %params.groupParam[%gid,%pid++] = "GridSize"  TAB "" TAB "SliderEdit" TAB "range::0 10;;precision::1";
    %params.groupParam[%gid,%pid++] = "GridDimension"  TAB "" TAB "TextEdit";


//-------------------------------------------------
// Group 3 Configuration
    %gid++;
    %pid = 0;
    %params.groupData[%gid] = "Lighting options" TAB "Params_Stack Append" TAB "Rollout";
    %params.groupParam[%gid,%pid++] = "SunDiffuseColor"  TAB "" TAB "Color";
    %params.groupParam[%gid,%pid++] = "SunAmbientColor"  TAB "" TAB "Color";
    %params.groupParam[%gid,%pid++] = "SunAngleX"  TAB "" TAB "SliderEdit" TAB "range::0 360;;precision::0";
    %params.groupParam[%gid,%pid++] = "SunAngleZ"  TAB "" TAB "SliderEdit" TAB "range::0 360;;precision::0";


    return %params;
}
//------------------------------------------------------------------------------
