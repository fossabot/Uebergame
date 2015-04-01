//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// ->Added Gui style support
// -->Delete Profiles when reloaded
// -->Image array store in Global
//==============================================================================


singleton GuiControlProfile (ToolsGuiTextProfile) {
    justify = "left";
    fontColor = "238 238 238 255";
    category = "Tools";
    fontType = "Gotham Book";
    fontSize = "16";
    fontColors[0] = "238 238 238 255";
    cursorColor = "0 0 0 255";
    fontColors[5] = "Fuchsia";
    fontColorLinkHL = "Fuchsia";
};




singleton GuiControlProfile( ToolsGuiTextListProfile : ToolsGuiTextProfile ) {
    tab = true;
    canKeyFocus = true;
    category = "Tools";
};
//==============================================================================
// GuiTextEditCtrl Profiles
//==============================================================================
//==============================================================================
singleton GuiControlProfile( ToolsGuiTextEditProfile ) {
    opaque = true;
    bitmap = "tlab/gui/images/element_assets/GuiTextEditBase.png";

    hasBitmapArray = true;
    border = -2; // fix to display textEdit img
    //borderWidth = "1";  // fix to display textEdit img
    //borderColor = "100 100 100";
    fillColor = "242 241 240 0";
    fillColorHL = "255 255 255";
    fontColor = "0 0 0";
    fontColorHL = "64 50 0 255";
    fontColorSEL = "98 100 137";
    fontColorNA = "6 19 76 255";
    textOffset = "4 2";
    autoSizeWidth = false;
    autoSizeHeight = true;
    justify = "left";
    tab = true;
    canKeyFocus = true;
    category = "Tools";
    fontType = "Gotham Book";
    fontSize = "15";
    fontColors[1] = "64 50 0 255";
    fontColors[2] = "6 19 76 255";
    fontColors[3] = "98 100 137 255";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiTextEditDark ) {
    opaque = true;
    bitmap = "tlab/gui/images/element_assets/GuiTextEditDark.png";

    hasBitmapArray = true;
    border = -2; // fix to display textEdit img
    //borderWidth = "1";  // fix to display textEdit img
    //borderColor = "100 100 100";
    fillColor = "242 241 240 0";
    fillColorHL = "255 255 255";
    fontColor = "194 254 254 255";
    fontColorHL = "255 255 255";
    fontColorSEL = "98 100 137";
    fontColorNA = "200 200 200";
    textOffset = "4 2";
    autoSizeWidth = false;
    autoSizeHeight = true;
    justify = "left";
    tab = true;
    canKeyFocus = true;
    category = "Tools";
    fontType = "Gotham Book";
    fontSize = "15";
    fontColors[1] = "255 255 255 255";
    fontColors[2] = "200 200 200 255";
    fontColors[3] = "98 100 137 255";
   bevelColorHL = "255 0 255 255";
   fontColors[0] = "194 254 254 255";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiNumericTextEditProfile : ToolsGuiTextEditProfile ) {
    numbersOnly = true;
    category = "Tools";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiNumericDropSliderTextProfile : ToolsGuiTextEditProfile ) {
    bitmap = "./images/textEditSliderBox";
    category = "Tools";
};
//------------------------------------------------------------------------------
