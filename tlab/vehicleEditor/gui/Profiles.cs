//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//-----------------------------------------------------------------------------
// Shape Editor Profiles
//-----------------------------------------------------------------------------

singleton GuiControlProfile(GuiVehicleEdScrollProfile : ToolsScrollProfile) {
    // Don't clear the scroll area (since we need to be able to see the GuiContainer
    // underneath that provides the fill color for the header row)
    opaque = false;
    category = "Editor";
};

singleton GuiControlProfile(GuiVehicleEdTextListProfile : ToolsGuiTextListProfile) {
    // Customise the not-active font used for the header row
    fontColorNA = "75 75 75";
    category = "Editor";
};

singleton GuiControlProfile(GuiVehicleEdRolloutProfile : GuiInspectorRolloutProfile0) {
    bitmap = "tlab/editorclasses/gui/images/rollout";
    category = "Editor";
};

singleton GuiControlProfile( GuiVehicleEdTransitionSliderProfile ) {
    bitmap = "tlab/VehicleEditor/images/transition_slider";
    category = "Core";
};
