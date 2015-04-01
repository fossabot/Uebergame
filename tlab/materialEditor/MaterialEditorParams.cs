//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Set default settings for the plugin
function MaterialEditorPlugin::initDefaultSettings( %this ) {

    Lab.addDefaultSetting(  "MaterialsPerPage",         "50" );
}
//------------------------------------------------------------------------------
//==============================================================================
// Automated editor plugin setting interface
function MaterialEditorPlugin::buildParams(%this,%params ) {

//-------------------------------------------------
// Group 1 Configuration
    %gid++;
    %pid = 0;
    %params.groupData[%gid] = "General settings" TAB "Params_Stack" TAB "Rollout";
    %params.groupParam[%gid,%pid++] = "MaterialsPerPage"  TAB "" TAB "SliderEdit" TAB "range::0 100;;precision::0";
    return %params;
}
//------------------------------------------------------------------------------