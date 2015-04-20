//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function VehicleEditorPlugin::initDefaultCfg( %this,%cfgArray )
{  

   %cfgArray.group[%gid++] = "General settings";   

   %cfgArray.setVal("BackgroundColor",       "0 0 0 100" TAB "BackgroundColor" TAB "TextEdit" TAB "" TAB "VehicleEditorPreview-->previewBackground.color" TAB %gid);
   %cfgArray.setVal("HighlightMaterial",   "1" TAB "HighlightMaterial" TAB "TextEdit" TAB "" TAB "" TAB %gid);
   %cfgArray.setVal("ShowNodes","1" TAB "ShowNodes" TAB "TextEdit" TAB "" TAB "" TAB %gid);
   %cfgArray.setVal("ShowBounds",       "0" TAB "ShowBounds" TAB "TextEdit" TAB "" TAB "" TAB %gid);
   %cfgArray.setVal("ShowObjBox",       "1" TAB "ShowObjBox" TAB "TextEdit" TAB "" TAB "" TAB %gid);
   %cfgArray.setVal("RenderMounts",       "1" TAB "RenderMounts" TAB "TextEdit" TAB "" TAB "" TAB %gid);
   %cfgArray.setVal("RenderCollision",       "0" TAB "RenderCollision" TAB "TextEdit" TAB "" TAB "" TAB %gid);
   %cfgArray.setVal("AdvancedWndVisible",       "1" TAB "RenderCollision" TAB "TextEdit" TAB "" TAB "" TAB %gid);
   
   %cfgArray.group[%gid++] = "Grid settings";
   %cfgArray.setVal("ShowGrid",       "1" TAB "ShowGrid" TAB "TextEdit" TAB "" TAB "VehicleEdShapeView" TAB %gid);
   %cfgArray.setVal("GridSize",       "0.1" TAB "GridSize" TAB "TextEdit" TAB "" TAB "VehicleEdShapeView" TAB %gid);
   %cfgArray.setVal("GridDimension",       "40 40" TAB "GridDimension" TAB "TextEdit" TAB "" TAB "VehicleEdShapeView" TAB %gid);
   
   %cfgArray.group[%gid++] = "Sun settings";
   %cfgArray.setVal("SunDiffuseColor",       "255 255 255 255" TAB "SunDiffuseColor" TAB "TextEdit" TAB "" TAB "VehicleEdShapeView" TAB %gid);
   %cfgArray.setVal("SunAmbientColor",       "180 180 180 255" TAB "SunAmbientColor" TAB "TextEdit" TAB "" TAB "VehicleEdShapeView" TAB %gid);
   %cfgArray.setVal("SunAngleX",       "45" TAB "SunAngleX" TAB "TextEdit" TAB "" TAB "VehicleEdShapeView" TAB %gid);
   %cfgArray.setVal("SunAngleZ",       "135" TAB "SunAngleZ" TAB "TextEdit" TAB "" TAB "VehicleEdShapeView" TAB %gid);
   
   %cfgArray.group[%gid++] = "Data settings";
   %vc = "WheeledVehicle HoverVehicle FlyingVehicle StreetVehicle";
   %cfgArray.setVal("VehicleClasses",      %vc TAB "VehicleClasses" TAB "TextEdit" TAB "" TAB "$VehicleEditor::Classes" TAB %gid);
}

function VehicleEditorPlugin::initDefaultSettings( %this ) {

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
function VehicleEditorPlugin::buildParams(%this,%params ) {

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
