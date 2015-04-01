//==============================================================================
// Lab Editor -> Default Containers Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// GuiWindowCtrl Profiles
//==============================================================================
//------------------------------------------------------------------------------

//==============================================================================
singleton GuiControlProfile (ToolsWindowProfile) {
    opaque = false;
    border = "0";
    fillColor = "53 53 53 255";
    fillColorHL = "221 221 221";
    fillColorNA = "200 200 200";
    fontColor = "236 236 236 255";
    fontColorHL = "0 0 0";
    bevelColorHL = "255 255 255";
    bevelColorLL = "0 0 0";
    text = "untitled";
    bitmap = "tlab/gui/images/container_assets/GuiWindowMain.png";
    textOffset = "8 2";
    hasBitmapArray = true;
    justify = "left";
    category = "Tools";
    fontType = "Gotham Bold";
    fontSize = "19";
    fontColors[0] = "236 236 236 255";
    cursorColor = "0 0 0 255";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile (ToolsWindowAltProfile : ToolsWindowProfile) {
    bitmap = "tlab/gui/images/container_assets/GuiWindowMainAlt.png";
};
//------------------------------------------------------------------------------


//==============================================================================
// GuiRolloutCtrl Profiles
//==============================================================================
//==============================================================================
singleton GuiControlProfile( ToolsRolloutProfile : ToolsDefaultProfile ) {
    border = 1;
    borderColor = "200 200 200";
    hasBitmapArray = true;
    bitmap = "tlab/gui/images/container_assets/GuiRolloutMain.png";
    textoffset = "17 0";
    fontType = "Aileron Bold";
    fontSize = "16";
    fontColors[0] = "253 253 253 255";
    fontColor = "253 253 253 255";
   fontColors[7] = "255 0 255 255";

};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsRolloutSubProfile : ToolsDefaultProfile ) {
    border = 1;
    borderColor = "200 200 200";
    hasBitmapArray = true;
    bitmap = "tlab/gui/images/container_assets/GuiRolloutSub.png";
    textoffset = "17 0";
    fontType = "Aileron Bold";
    fontSize = "16";
    fontColors[0] = "253 253 253 255";
    fontColor = "253 253 253 255";
   fontColors[7] = "255 0 255 255";

};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsRolloutTitleProfile : ToolsDefaultProfile ) {
    border = "0";
    borderColor = "200 200 200";
    hasBitmapArray = true;
    bitmap = "tlab/gui/images/container_assets/GuiRolloutTitle";
    textoffset = "17 0";
    fontType = "Aileron Bold";
    fontSize = "16";
    fontColors[0] = "253 253 253 255";
    fontColor = "253 253 253 255";
   fontColors[7] = "255 0 255 255";
   fontColors[5] = "Magenta";
   fontColorLinkHL = "Magenta";

};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsRolloutTitleThinProfile : ToolsRolloutTitleProfile ) {  
    bitmap = "tlab/gui/images/container_assets/GuiRolloutTitleThin";
   border = "0";
   fontType = "Gotham Book";
   fontSize = "14";
   fontColors[5] = "255 0 255 255";
   fontColorLinkHL = "255 0 255 255";
   textOffset = "17 -1";
};
//------------------------------------------------------------------------------

//==============================================================================
// GuiScrollCtrl Profiles
//==============================================================================

//==============================================================================
singleton GuiControlProfile( ToolsScrollProfile ) {
    opaque = "1";
    fillcolor = "254 254 254 55";
    fontColor = "0 0 0";
    fontColorHL = "150 150 150";
    border = "0";
    bitmap = "tlab/gui/images/container_assets/GuiScrollMain.png";
    hasBitmapArray = true;
    category = "Tools";
    fontColors[1] = "150 150 150 255";
    borderThickness = "3";
    bevelColorHL = "57 152 152 255";
    bevelColorLL = "150 196 201 255";  
};
//------------------------------------------------------------------------------
//==============================================================================
// GuiTabBookCtrl Profiles
//==============================================================================
//==============================================================================
singleton GuiControlProfile( ToolsTabBookProfile ) {
    border = "0";
    opaque = false;
    hasBitmapArray = true;
    bitmap = "tlab/gui/images/container_assets/GuiTabBookMain";

    category = "Tools";
   fontType = "Gotham Book";
   fontSize = "15";
   fontColors[0] = "254 254 254 255";
   fontColor = "254 254 254 255";
   textOffset = "8 0";
   justify = "Top";
   fontColors[9] = "255 0 255 255";
   autoSizeWidth = "1";
   autoSizeHeight = "1";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsTabBookAltProfile ) {
    border = false;
    opaque = false;
    hasBitmapArray = true;
     bitmap = "tlab/gui/images/container_assets/GuiTabBookAlt";
    category = "Tools";
   fontType = "Gotham Book";
   fontColors[0] = "236 236 236 255";
   fontColor = "236 236 236 255";
   justify = "Center";
};
//------------------------------------------------------------------------------
//==============================================================================
// GuiTabPageCtrl Profiles
//==============================================================================

//==============================================================================
singleton GuiControlProfile( ToolsTabPageProfile : ToolsDefaultProfile ) {   
    opaque = true;
    fillColor = "29 43 51 255";    
   justify = "Center";
};
//------------------------------------------------------------------------------

//==============================================================================
// GuiFrameSetCtrl Profiles
//==============================================================================
//==============================================================================
singleton GuiControlProfile (ToolsGuiFrameSetProfile) {
    fillcolor = "255 255 255";
    borderColor = "246 245 244";
    border = 1;
    opaque = true;
    border = true;
    category = "Tools";
};
//------------------------------------------------------------------------------

//==============================================================================
// GuiBitmapBorderCtrl Profiles
//==============================================================================
//==============================================================================
singleton GuiControlProfile( ToolsBitmapBorderTabProfile ) {
    border = false;
    opaque = false;
    hasBitmapArray = true;
    bitmap = "tlab/gui/images/container_assets/GuiBitmapBorderTab";

    category = "Tools";
};
//------------------------------------------------------------------------------
