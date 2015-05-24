//==============================================================================
// Lab Editor -> Default Containers Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// GuiWindowCtrl Profiles
//==============================================================================

//==============================================================================
//ToolsWindowProfile Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsWindowProfile : ToolsDefaultProfile ) {
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
    bitmap = "tlab/gui/assets/container_assets/GuiWindowProfile.png";
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
//ToolsWindowProfile Dark center variation
singleton GuiControlProfile( ToolsWindowProfile_Dark : ToolsWindowProfile ) {
	bitmap = "tlab/gui/assets/container_assets/GuiWindowProfile_Dark.png";
   fillColor = "20 25 31 255";
   fontColors[1] = "0 0 0 255";
   fontColorHL = "0 0 0 255";
};
//------------------------------------------------------------------------------

//==============================================================================
// GuiScrollCtrl Profiles
//==============================================================================
//==============================================================================
//ToolsScrollProfile Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsScrollProfile : ToolsDefaultProfile ) {
     opaque = "1";
    fillcolor = "254 254 254 55";
    fontColor = "0 0 0";
    fontColorHL = "150 150 150";
    border = "0";
    bitmap = "tlab/gui/assets/container_assets/GuiScrollProfile.png";
    hasBitmapArray = true;
    category = "Tools";
    fontColors[1] = "150 150 150 255";
    borderThickness = "3";
    bevelColorHL = "57 152 152 255";
    bevelColorLL = "150 196 201 255";  
};
//------------------------------------------------------------------------------
//ToolsScrollProfile Dark Fill 100%
singleton GuiControlProfile( ToolsScrollProfile_Dark : ToolsScrollProfile ) {
	bitmap = "tlab/gui/assets/container_assets/GuiScrollProfile.png";
};
//------------------------------------------------------------------------------
//ToolsScrollProfile Dark Fill 50%
singleton GuiControlProfile( ToolsScrollProfile_Dark_T50 : ToolsScrollProfile ) {
	bitmap = "tlab/gui/assets/container_assets/GuiScrollProfile.png";
};
//------------------------------------------------------------------------------

//==============================================================================
// GuiTabBookCtrl Profiles
//==============================================================================

//==============================================================================
//ToolsTabBookProfile Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsTabBookProfile : ToolsDefaultProfile ) {
    border = "0";
    opaque = false;
    hasBitmapArray = true;
    bitmap = "tlab/gui/assets/container_assets/GuiTabBookProfile";

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
//ToolsTabBookProfile Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsTabBookBasic : ToolsDefaultProfile ) {
    border = "0";
    opaque = false;
    hasBitmapArray = true;
    bitmap = "tlab/gui/assets/container_assets/ToolsTabBookBasic";

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
//ToolsTabBookProfile Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsTabBookColor : ToolsDefaultProfile ) {
    border = "0";
    opaque = false;
    hasBitmapArray = true;
    bitmap = "tlab/gui/assets/container_assets/ToolsTabBookColor";

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
// GuiRolloutCtrl Profiles
//==============================================================================

//==============================================================================
//ToolsRolloutProfile Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsRolloutProfile : ToolsDefaultProfile ) {
    border = 1;
    borderColor = "200 200 200";
    hasBitmapArray = true;
    bitmap = "tlab/gui/assets/container_assets/GuiRolloutProfile.png";
    textoffset = "17 0";
    fontType = "Aileron Bold";
    fontSize = "16";
    fontColors[0] = "253 253 253 255";
    fontColor = "253 253 253 255";
   fontColors[7] = "255 0 255 255";

};
//------------------------------------------------------------------------------
//ToolsRolloutProfile Thin Version
singleton GuiControlProfile( ToolsRolloutProfile_Thin : ToolsRolloutProfile ) {
    bitmap = "tlab/gui/assets/container_assets/GuiRolloutProfile_Thin.png";
    fontSize = "14";
};
//------------------------------------------------------------------------------
//ToolsRolloutProfile Light Version
singleton GuiControlProfile( ToolsRolloutProfile_Light : ToolsRolloutProfile ) {
    border = 1;
    borderColor = "200 200 200";
    hasBitmapArray = true;
    bitmap = "tlab/gui/assets/container_assets/GuiRolloutProfile_Light.png";
    textoffset = "17 0";
    fontType = "Aileron Bold";
    fontSize = "16";
    fontColors[0] = "253 253 253 255";
    fontColor = "253 253 253 255";
   fontColors[7] = "255 0 255 255";

};
//------------------------------------------------------------------------------
//==============================================================================
//ToolsRolloutTitle Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsRolloutTitle : ToolsDefaultProfile ) {
    border = 1;
    borderColor = "200 200 200";
    hasBitmapArray = true;
    bitmap = "tlab/gui/assets/container_assets/GuiRolloutTitle.png";
    textoffset = "17 0";
    fontType = "Aileron Bold";
    fontSize = "16";
    fontColors[0] = "253 253 253 255";
    fontColor = "253 253 253 255";
   fontColors[7] = "255 0 255 255";

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
