//==============================================================================
// TorqueLab -> Core Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Those profiles are defined in the code directly
//==============================================================================


new GuiControlProfile (ToolsDefaultProfile) {
    tab = false;
    canKeyFocus = false;
    hasBitmapArray = false;
    mouseOverSelected = false;

    // fill color
    opaque = "1";
    fillColor = "243 241 241 0";
    fillColorHL ="229 229 236 0";
    fillColorSEL = "99 101 138 156";
    fillColorNA = "255 255 255 0";

    // border color
    border = 0;
    borderColor   = "100 100 100";
    borderColorHL = "50 50 50 50";
    borderColorNA = "75 75 75";

    // font
    fontType = "Gafata Std";
    fontSize = 16;
    fontCharset = ANSI;

    fontColor = "0 0 0";
    fontColorHL = "0 0 0";
    fontColorNA = "0 0 0";
    fontColorSEL= "255 255 255";

    // bitmap information
    bitmapBase = "";
    textOffset = "0 0";

    // used by guiTextControl
    modal = true;
    justify = "left";
    autoSizeWidth = false;
    autoSizeHeight = false;
    returnTab = false;
    numbersOnly = false;
    cursorColor = "0 0 0 255";

    category = "Tools";
   borderThickness = "0";
    // sounds
    //soundButtonDown = "";
    //soundButtonOver = "";
};


new GuiControlProfile (ToolsGuiSolidDefaultProfile : ToolsDefaultProfile) {
    opaque = true;
    border = true;
    category = "Tools";
};

new GuiControlProfile (ToolsGuiTransparentProfile : ToolsDefaultProfile) {
    opaque = false;
    border = false;
    category = "Tools";
};

new GuiControlProfile( ToolsGuiGroupBorderProfile ) {
    border = false;
    opaque = false;
    hasBitmapArray = true;

    bitmap = "tlab/gui/icons/default/group-border";
    category = "Tools";
};


new GuiControlProfile (ToolsGuiToolTipProfile) {
    // fill color
    fillColor = "239 237 222";

    // border color
    borderColor   = "138 134 122";

    // font
    fontType = "Arial";
    fontSize = 14;
    fontColor = "0 0 0";

    category = "Tools";
};

singleton GuiControlProfile( ToolsGuiModelessDialogProfile : ToolsDefaultProfile ) {
    modal = false;
    category = "Tools";
};

singleton GuiControlProfile( ToolsDefaultProfile_NoModal : ToolsDefaultProfile ) {
    modal = false;
    category = "Tools";
};


new GuiControlProfile( ToolsGuiRLProgressBitmapProfile ) {
    border = false;
    hasBitmapArray = true;
    bitmap = "tlab/gui/icons/default/rl-loadingbar";
    category = "Tools";
};





new GuiControlProfile(ToolsGuiEditorTabPage) {
    opaque = true;
    border = false;
    fontColor = "0 0 0";
    fontColorHL = "0 0 0";
    fixedExtent = false;
    justify = "center";
    canKeyFocus = false;
    bitmap = "tlab/gui/icons/default/tab";
    hasBitmapArray = true;
    category = "Tools";
};



new GuiControlProfile( ToolsGuiRadioProfile ) {
    fontSize = 14;
    fillColor = "232 232 232";
    fontColor = "20 20 20";
    fontColorHL = "80 80 80";
    fixedExtent = true;
    bitmap = "tlab/gui/icons/default/radioButton";
    hasBitmapArray = true;
    category = "Tools";
};



new GuiControlProfile( ToolsGuiOverlayProfile ) {
    opaque = true;
    fillcolor = "255 255 255";
    fontColor = "0 0 0";
    fontColorHL = "255 255 255";
    fillColor = "0 0 0 100";
    category = "Tools";
};
singleton GuiControlProfile(ToolsGuiMenuProfile) {
    opaque = true;
    fillcolor = "255 255 255";
    fontColor = "0 0 0";
    fontColorHL = "255 255 255";
    fillColor = "0 0 0 100";
    category = "Tools";
};
singleton GuiControlProfile(ToolsGuiMenuAltProfile) {
    opaque = true;
    fillcolor = "255 255 255";
    fontColor = "0 0 0";
    fontColorHL = "255 255 255";
    fillColor = "0 0 0 100";
    category = "Tools";
};


new GuiControlProfile( ToolsGuiListBoxProfile ) {
    tab = true;
    canKeyFocus = true;
    category = "Tools";
};



new GuiControlProfile( ToolsGuiTextPadProfile ) {
    fontType = ($platform $= "macos") ? "Monaco" : "Lucida Console";
    fontSize = ($platform $= "macos") ? 13 : 12;
    tab = true;
    canKeyFocus = true;

    // Deviate from the Default
    opaque=true;
    fillColor = "255 255 255";
    border = 0;
    category = "Tools";
};

new GuiControlProfile( ToolsGuiFormProfile : ToolsDefaultProfile ) {
    opaque = false;
    border = 5;
    justify = "center";
    profileForChildren = ToolsButtonProfile;
    opaque = false;
    hasBitmapArray = true;
    bitmap = "tlab/gui/icons/default/button";
    category = "Tools";
};

// ----------------------------------------------------------------------------
singleton GuiControlProfile( ToolsGuiBackFillProfile ) {
    opaque = true;
    fillColor = "0 94 94";
    border = "1";
    borderColor = "255 128 128";
    fontType = "Gotham Book";
    fontSize = 12;
    fontColor = "0 0 0";
    fontColorHL = "50 50 50";
    fixedExtent = 1;
    justify = "center";
    category = "Editor";
    fontColors[1] = "50 50 50 255";
    fontColors[9] = "255 0 255 255";
};




