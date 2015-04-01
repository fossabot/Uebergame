//==============================================================================
// Lab Editor -> Default Containers Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// GuiButtonCtrl Profiles
//==============================================================================

//==============================================================================
singleton GuiControlProfile( ToolsGuiButtonProfile ) {
    fontSize = "16";
    fontType = "Gafata Std Bold";
    fontColor = "254 254 254 255";
    justify = "center";
    category = "Tools";
    opaque = "1";
    border = "1";
    fontColors[0] = "254 254 254 255";
    fontColors[2] = "200 200 200 255";
    fontColorNA = "200 200 200 255";
    bitmap = "tlab/gui/images/button_assets/GuiButtonMain.png";
    fixedExtent = "0";
   bevelColorLL = "Magenta";
   textOffset = "0 2";
   autoSizeWidth = "1";
   autoSizeHeight = "1";
};
//------------------------------------------------------------------------------

//==============================================================================
// GuiIconButtonCtrl Profiles
//==============================================================================

//==============================================================================
singleton GuiControlProfile( ToolsGuiIconButtonProfile ) {
    opaque = true;
    border = "-2";
    fontColor = "254 254 254 255";
    fontColorHL = "0 0 0";
    fontColorNA = "200 200 200";
    fixedExtent = 0;
    justify = "center";
    canKeyFocus = false;
    bitmap = "tlab/gui/images/button_assets/GuiButtonMain.png";
    hasBitmapArray = true;
    category = "Tools";
    fontColors[0] = "254 254 254 255";
    fontColors[2] = "200 200 200 255";
    bevelColorHL = "255 0 255 255";
    fontColors[5] = "Magenta";
    fontColors[9] = "255 0 255 255";
    fontColorLinkHL = "Magenta";
   bevelColorLL = "255 0 255 255";
   fontColors[1] = "0 0 0 255";
   fontType = "Gotham Bold";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiIconButtonSmallProfile : ToolsGuiIconButtonProfile ) {
    bitmap = "tlab/gui/images/button_assets/GuiIconButtonMain.png";
    category = "Tools";
   border = "-2";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiIconButtonStackProfile : ToolsGuiIconButtonProfile ) {
    bitmap = "tlab/gui/images/button_assets/GuiIconButtonMain.png";
    category = "Tools";
   border = "-2";
   fontSize = "16";
};
//------------------------------------------------------------------------------
//==============================================================================
// GuiCheckboxCtrl Profiles
//==============================================================================
//==============================================================================
singleton GuiControlProfile( ToolsGuiCheckBoxProfile ) {
    opaque = false;
    fillColor = "232 232 232";
    border = false;
    borderColor = "100 100 100";
    fontSize = "14";
    fontColor = "250 220 171 255";
    fontColorHL = "80 80 80";
    fontColorNA = "200 200 200";
    fixedExtent = 1;
    justify = "left";

    bitmap = "tlab/gui/images/button_assets/GuiCheckboxBase.png";
    hasBitmapArray = true;
    category = "Tools";
    fontType = "Gotham Book";
    fontColors[0] = "250 220 171 255";
    fontColors[1] = "80 80 80 255";
    fontColors[2] = "200 200 200 255";
   fontColors[4] = "Fuchsia";
   fontColorLink = "Fuchsia";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiCheckBoxMain : ToolsGuiCheckBoxProfile ) {

    bitmap = "tlab/gui/images/button_assets/GuiCheckboxMain.png";   
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiCheckBoxAlt : ToolsGuiCheckBoxProfile ) {

    bitmap = "tlab/gui/images/button_assets/GuiCheckboxAlt.png";   
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiCheckBoxTitle ) {
    opaque = false;
    fillColor = "232 232 232";
    border = false;
    borderColor = "100 100 100";
    fontSize = "14";
    fontColor = "78 215 213 255";
    fontColorHL = "80 80 80";
    fontColorNA = "200 200 200";
    fixedExtent = 1;
    justify = "left";

   bitmap = "tlab/gui/images/button_assets/GuiCheckboxMain.png";
    hasBitmapArray = true;
    category = "Tools";
    fontType = "Gotham Book";
    fontColors[0] = "78 215 213 255";
    fontColors[1] = "80 80 80 255";
    fontColors[2] = "200 200 200 255";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiCheckBoxListProfile : ToolsGuiCheckBoxProfile) {
    bitmap = "./images/checkbox-list";
    category = "Tools";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiCheckBoxListFlipedProfile : ToolsGuiCheckBoxProfile) {
    bitmap = "tlab/gui/profiles/images/checkbox-list_fliped";
    category = "Tools";
   fontType = "Gotham Book";
   fontSize = "16";
   fontColors[0] = "215 229 208 255";
   fontColor = "215 229 208 255";
   fontColors[1] = "197 152 141 255";
   fontColors[4] = "255 0 255 255";
   fontColorHL = "197 152 141 255";
   fontColorLink = "255 0 255 255";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiInspectorCheckBoxTitleProfile : ToolsGuiCheckBoxProfile ) {
    fontColor = "100 100 100";
    category = "Tools";
};
//------------------------------------------------------------------------------
//==============================================================================
new GuiControlProfile( ToolsGuiThumbHighlightButtonProfile : ToolsGuiButtonProfile ) {
    bitmap = "./images/thumbHightlightButton";
    category = "Tools";
};
//------------------------------------------------------------------------------
singleton GuiControlProfile(ToolsGuiRadioMain : GuiDefaultProfile)
{
	fillColor = "254 253 253 255";
	fillColorHL = "221 221 221 255";
	fillColorNA = "200 200 200 255";
	fontSize = "20";
	textOffset = "16 10";
	bitmap = "tlab/gui/images/button_assets/GuiRadioMain.png";
	hasBitmapArray = "1";
	fontColors[0] = "250 250 250 255";
	fontColor = "250 250 250 255";
   border = "0";
   fontColors[2] = "Black";
   fontColorNA = "Black";
   justify = "Center";
   fontType = "Gotham Bold";
   category = "Game";
   fontColors[8] = "255 0 255 255";
};