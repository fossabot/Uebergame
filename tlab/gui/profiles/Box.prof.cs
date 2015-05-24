//==============================================================================
// Lab Editor -> Default ToolBoxes Profiles
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// GuiContainer Profiles
//==============================================================================

//==============================================================================
//ToolsBoxTitleDark Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsBoxTitleDark : ToolsDefaultProfile ) {
     opaque = false;
    border = -2;
    category = "ToolsContainers";
    bitmap = "tlab/gui/assets/container/GuiBoxTitleDark";
    fontColors[2] = "0 0 0 255";
    fontColorNA = "0 0 0 255";
};
//------------------------------------------------------------------------------

//==============================================================================
//ToolsBoxTitleBar Style
//------------------------------------------------------------------------------
singleton GuiControlProfile( ToolsBoxTitleBar : ToolsDefaultProfile ) {
    opaque = false;
    border = -2;
    category = "ToolsContainers";
    bitmap = "tlab/gui/assets/container/GuiBoxTitleBar.png";
    fontColors[2] = "0 0 0 255";
    fontColorNA = "0 0 0 255";
   fontType = "Gotham Black";
   fontSize = "17";
   fontColors[0] = "222 222 222 255";
   fontColor = "222 222 222 255";
};
//------------------------------------------------------------------------------
