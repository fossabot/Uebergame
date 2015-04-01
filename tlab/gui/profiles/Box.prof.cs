//==============================================================================
// GameLab -> Interface profiles
// Copyright NordikLab Studio, 2014
//==============================================================================


//==============================================================================
// UIWindow Profile - Default window
singleton GuiControlProfile(ToolsGuiBoxMain : ToolsDefaultProfile)
{
	fontSize = "18";
	textOffset = "8 1";
	bitmap = "tlab/gui/images/container_assets/GuiBoxMain";
   fontType = "Aeleron Bold";
   fillColor = "233 233 233 255";
   border = "-1";
   hasBitmapArray = "1";
};
//------------------------------------------------------------------------------

//==============================================================================
// Box Fill Profiles
//==============================================================================
//==============================================================================
// UIWindow Profile - Default window
singleton GuiControlProfile(ToolsGuiBoxFillBlue : GuiDefaultProfile)
{
	fontSize = "18";
	textOffset = "8 1";
	bitmap = "art/gui/tlab/container_assets/GuiBoxFillBlue";
   fontType = "Aeleron Bold";
   fillColor = "233 233 233 255";
   border = "-1";
   hasBitmapArray = "1";
   fontColors[4] = "Fuchsia";
   fontColorLink = "Fuchsia";
};
//------------------------------------------------------------------------------
//==============================================================================
// UIWindow Profile - Default window
singleton GuiControlProfile(ToolsGuiBoxFillBlue_Header : ToolsGuiBoxFillBlue)
{	
	bitmap =  "art/gui/tlab/container_assets/GuiBoxFillBlueHead";  
};
//------------------------------------------------------------------------------
//==============================================================================
// UIWindow Profile - Default window
singleton GuiControlProfile(ToolsGuiBoxFillDark : GuiDefaultProfile)
{
	fontSize = "18";
	textOffset = "8 1";
	bitmap = "art/gui/tlab/container_assets/GuiBoxFillDark";
   fontType = "Aeleron Bold";
   fillColor = "233 233 233 255";
   border = "-1";
   hasBitmapArray = "1";
   fontColors[4] = "Fuchsia";
   fontColorLink = "Fuchsia";
};
//------------------------------------------------------------------------------
//==============================================================================
// UIWindow Profile - Default window
singleton GuiControlProfile(ToolsGuiBoxFillDark_Header : ToolsGuiBoxFillDark)
{	
	bitmap =  "art/gui/tlab/container_assets/GuiBoxFillDarkHead";  
};
//------------------------------------------------------------------------------
//==============================================================================
// Box Overlay Profiles
//==============================================================================
//==============================================================================
// UIWindow Profile - Default window
singleton GuiControlProfile(ToolsGuiBoxOverlay : ToolsDefaultProfile)
{
	fontSize = "18";
	textOffset = "8 1";
	bitmap = "tlab/gui/images/container_assets/GuiBoxOverlay";
   fontType = "Aeleron Bold";
   fillColor = "233 233 233 255";
   border = "-1";
   hasBitmapArray = "1";
};
//------------------------------------------------------------------------------
//==============================================================================
// UIWindow Profile - Default window
singleton GuiControlProfile(ToolsGuiBoxOverlay_T50 : ToolsGuiBoxOverlay)
{	
	bitmap =  "tlab/gui/images/container_assets/GuiBoxOverlay_T50";  
};
//------------------------------------------------------------------------------
//==============================================================================
// UIWindow Profile - Default window
singleton GuiControlProfile(ToolsGuiBoxOverlay_T75 : ToolsGuiBoxOverlay)
{	
	bitmap = "tlab/gui/images/container_assets/GuiBoxOverlay_T75";  
};
//------------------------------------------------------------------------------

//==============================================================================
// Box Border Profiles
//==============================================================================
//==============================================================================
// UIWindow Profile - Default window
singleton GuiControlProfile(ToolsGuiBoxBorder : ToolsDefaultProfile)
{
	fontSize = "18";
	textOffset = "8 1";
	bitmap = "tlab/gui/images/container_assets/GuiBoxBorder";
   fontType = "Aeleron Bold";
   fillColor = "233 233 233 255";
   border = "-1";
   hasBitmapArray = "1";
};
//------------------------------------------------------------------------------
//==============================================================================
// UIWindow Profile - Default window
singleton GuiControlProfile(ToolsGuiBoxBorder_T50 : ToolsGuiBoxBorder)
{	
	bitmap =  "tlab/gui/images/container_assets/GuiBoxBorder_T50";  
};
//------------------------------------------------------------------------------
//==============================================================================
// UIWindow Profile - Default window
singleton GuiControlProfile(ToolsGuiBoxBorder_T75 : ToolsGuiBoxBorder)
{	
	bitmap =  "tlab/gui/images/container_assets/GuiBoxBorder_T75";  
};
//------------------------------------------------------------------------------

//==============================================================================
// Banner Progress Profiles
//==============================================================================
//==============================================================================
// UIWindow Profile - Default window
singleton GuiControlProfile(ToolsGuiBannerProgress : ToolsDefaultProfile)
{
	fontSize = "18";
	textOffset = "8 1";
	bitmap = "tlab/gui/images/container_assets/GuiBannerProgress";
   fontType = "Aeleron Bold";
   fillColor = "233 233 233 255";
   border = "-1";
   hasBitmapArray = "1";
};
//------------------------------------------------------------------------------