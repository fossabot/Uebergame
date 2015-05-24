//==============================================================================
// Lab Editor -> Default ToolBoxes Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// GuiContainer Profiles
//==============================================================================

//==============================================================================
//ToolsBoxNavDark Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsBoxNavDark : ToolsDefaultProfile ) {
     opaque = false;
    border = -2;
    category = "ToolsContainers";
    bitmap = "tlab/gui/assets/container_assets/GuiBoxNavDark";
    fontColors[2] = "0 0 0 255";
    fontColorNA = "0 0 0 255";
};
//------------------------------------------------------------------------------

//==============================================================================
//ToolsBoxNavTitle Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsBoxNavTitle : ToolsDefaultProfile ) {
    opaque = false;
    border = -2;
    category = "ToolsContainers";
    bitmap = "tlab/gui/assets/container_assets/GuiBoxNavTitle.png";
    fontColors[2] = "0 0 0 255";
    fontColorNA = "0 0 0 255";
   fontType = "Gotham Black";
   fontSize = "17";
   fontColors[0] = "222 222 222 255";
   fontColor = "222 222 222 255";
};
//------------------------------------------------------------------------------
