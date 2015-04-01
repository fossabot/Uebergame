//==============================================================================
// Lab Editor -> Core Profiles
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
    opaque = false;
    fillColor = "242 241 240";
    fillColorHL ="228 228 235";
    fillColorSEL = "98 100 137";
    fillColorNA = "255 255 255 ";

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
    // sounds
    //soundButtonDown = "";
    //soundButtonOver = "";
};


new GuiControlProfile (ToolsGuiSolidDefaultProfile) {
    opaque = true;
    border = true;
    category = "Tools";
};

new GuiControlProfile (ToolsGuiTransparentProfile) {
    opaque = false;
    border = false;
    category = "Tools";
};

new GuiControlProfile( ToolsGuiGroupBorderProfile ) {
    border = false;
    opaque = false;
    hasBitmapArray = true;

    bitmap = "tlab/gui/oldImages/group-border";
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

new GuiControlProfile( ToolsGuiModelessDialogProfile ) {
    modal = false;
    category = "Tools";
};


new GuiControlProfile( ToolsGuiRLProgressBitmapProfile ) {
    border = false;
    hasBitmapArray = true;
    bitmap = "./images/rl-loadingbar";
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
    bitmap = "./images/tab";
    hasBitmapArray = true;
    category = "Tools";
};



new GuiControlProfile( ToolsGuiRadioProfile ) {
    fontSize = 14;
    fillColor = "232 232 232";
    fontColor = "20 20 20";
    fontColorHL = "80 80 80";
    fixedExtent = true;
    bitmap = "./images/radioButton";
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



new GuiControlProfile( ToolsGuiTreeViewProfile ) {
    bitmap = "tlab/gui/images/element_assets/GuiTreeViewBase.png";
    autoSizeHeight = true;
    canKeyFocus = true;
    fillColor = "255 255 255";
    fillColorHL = "228 228 235";
    fillColorSEL = "98 100 137";
    fillColorNA = "255 255 255";
    fontColor = "2 2 2 255";
    fontColorHL = "0 0 0";
    fontColorSEL= "255 255 255";
    fontColorNA = "200 200 200";
    borderColor = "128 000 000";
    borderColorHL = "255 228 235";
    fontSize = 14;
    opaque = false;
    border = false;
    category = "Tools";
    fontColors[2] = "200 200 200 255";
   fontType = "Gotham Book";
   fontColors[0] = "2 2 2 255";
};
new GuiControlProfile( ToolsGuiTreeViewDark ) {
    bitmap = "tlab/gui/images/element_assets/GuiTreeViewBase.png";
    autoSizeHeight = true;
    canKeyFocus = true;
    fillColor = "255 255 255";
    fillColorHL = "228 228 235";
    fillColorSEL = "98 100 137";
    fillColorNA = "255 255 255";
    fontColor = "238 240 211 255";
    fontColorHL = "0 0 0";
    fontColorSEL= "255 255 255";
    fontColorNA = "200 200 200";
    borderColor = "128 000 000";
    borderColorHL = "255 228 235";
    fontSize = "16";
    opaque = false;
    border = false;
    category = "Tools";
    fontColors[2] = "200 200 200 255";
   fontType = "Gotham Book";
   fontColors[0] = "238 240 211 255";
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
    profileForChildren = ToolsGuiButtonProfile;
    opaque = false;
    hasBitmapArray = true;
    bitmap = "./images/button";
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



singleton GuiControlProfile( GuiCreatorIconButtonProfile ) {
    opaque = true;
    fillColor = "225 243 252 255";
    fillColorHL = "225 243 252 0";
    fillColorNA = "225 243 252 0";
    fillColorSEL = "225 243 252 0";

    //tab = true;
    //canKeyFocus = true;

    fontType = "Gotham Book";
    fontSize = "14";

    fontColor = "250 250 247 255";
    fontColorSEL = "43 107 206";
    fontColorHL = "244 244 244";
    fontColorNA = "100 100 100";

    border = 1;
    borderColor   = "153 222 253 255";
    borderColorHL = "156 156 156";
    borderColorNA = "153 222 253 0";

    //bevelColorHL = "255 255 255";
    //bevelColorLL = "0 0 0";
    category = "Editor";
    fontColors[1] = "244 244 244 255";
    fontColors[2] = "100 100 100 255";
    fontColors[3] = "43 107 206 255";
    fontColors[9] = "255 0 255 255";
   fontColors[0] = "250 250 247 255";
};
//------------------------------------------------------------------------------


//==============================================================================
// Used in SourceCode
singleton GuiControlProfile (ToolsGuiMenuBarProfile) {
    opaque = true;
    border = "1";
    category = "Tools";
   fillColor = "44 44 44 255";
   fontType = "Gotham Book";
   fontSize = "17";
   fontColors[0] = "254 254 254 255";
   fontColor = "254 254 254 255";
   justify = "Center";
   textOffset = "10 6";
   fontColors[8] = "255 0 255 255";
};
//------------------------------------------------------------------------------
