//==============================================================================
// Lab Editor -> Default Containers Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// GuiSliderCtrl Profiles
//==============================================================================

//==============================================================================
singleton GuiControlProfile( ToolsGuiSliderProfile ) {
    bitmap = "tlab/gui/images/element_assets/GuiSliderDot.png";
    category = "Tools";
   fontColors[8] = "255 0 255 255";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiSliderBoxProfile ) {
    bitmap = "./images/slider-w-box";
    category = "Tools";
};
//------------------------------------------------------------------------------

//==============================================================================
// GuiPopUpMenuCtrl Profiles
//==============================================================================

//==============================================================================
singleton GuiControlProfile( ToolsGuiPopupMenuItemBorder : ToolsDefaultProfile ) {
    opaque = true;
    border = true;
    fontColor = "0 0 0";
    fontColorHL = "0 0 0";
    fontColorNA = "255 255 255";
    fixedExtent = false;
    justify = "center";
    canKeyFocus = false;
    bitmap = "./images/button";
    category = "Tools";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiPopUpMenuDefault : ToolsDefaultProfile ) {
    opaque = true;
    mouseOverSelected = true;
    textOffset = "3 3";
    border = 0;
    borderThickness = 0;
    fixedExtent = true;
    bitmap = "./images/scrollbar";
    hasBitmapArray = true;
    profileForChildren = ToolsGuiPopupMenuItemBorder;
    fillColor = "242 241 240 ";//"255 255 255";//100
    fillColorHL = "228 228 235 ";//"204 203 202";
    fillColorSEL = "98 100 137 ";//"204 203 202";
    // font color is black
    fontColorHL = "0 0 0 ";//"0 0 0";
    fontColorSEL = "255 255 255";//"0 0 0";
    borderColor = "100 100 100";
    category = "Tools";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiPopUpMenuProfile : ToolsGuiPopUpMenuDefault ) {
    textOffset         = "6 4";
    bitmap             = "tlab/gui/images/element_assets/GuiDropdownMain.png";
    hasBitmapArray     = true;
    border             = "1";
    profileForChildren = ToolsGuiPopUpMenuDefault;
    category = "Tools";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiPopUpMenuTabProfile : ToolsGuiPopUpMenuDefault ) {
    bitmap             = "./images/dropDown-tab";
    textOffset         = "6 4";
    canKeyFocus        = true;
    hasBitmapArray     = true;
    border             = 1;
    profileForChildren = ToolsGuiPopUpMenuDefault;
    category = "Tools";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( ToolsGuiPopUpMenuEditProfile : ToolsGuiPopUpMenuDefault ) {
    textOffset         = "6 4";
    canKeyFocus        = true;
    bitmap             = "./images/dropDown";
    hasBitmapArray     = true;
    border             = 1;
    profileForChildren = ToolsGuiPopUpMenuDefault;
    category = "Tools";
};
//------------------------------------------------------------------------------
