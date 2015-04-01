//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//exec("scripts/gui/profileStyleSetup.cs");
//==============================================================================

//==============================================================================
// Standard Text Profiles
//==============================================================================
singleton GuiControlProfile(ToolsGuiTextBase : ToolsDefaultProfile) {
    fontColor = "254 254 254 255";
    fontType = "Gotham Bold";
    fillColor = "238 236 240 255";
    bevelColorHL = "Magenta";
    justify = "Left";
    category = "LabText";
    fontColors[0] = "254 254 254 255";
    fontColors[1] = "254 254 254 255";
    fontColors[2] = "254 254 254 255";
    fontColorHL = "254 254 254 255";
    fontColorNA = "254 254 254 255";
    fontSize = "15";
   fontColors[4] = "Magenta";
   fontColorLink = "Magenta";

};
singleton GuiControlProfile(ToolsGuiTextBase_C : ToolsGuiTextBase) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsGuiTextBase_R : ToolsGuiTextBase) {
    locked = true;
    justify = "Right";
};
singleton GuiControlProfile (ToolsGuiTextBase_Auto : ToolsGuiTextBase) {    
    autoSizeWidth = true;
    autoSizeHeight = true;   
};

singleton GuiControlProfile( ToolsGuiTextBase_ML : ToolsGuiTextBase ) {   
    autoSizeWidth = true;
    autoSizeHeight = true;
    border = false;    
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile(ToolsGuiTextBaseSmall : ToolsGuiTextBase) {
    fontSize = "14";
    fontType = "Gotham Book";
};
singleton GuiControlProfile(ToolsGuiTextBaseSmall_C : ToolsGuiTextBaseSmall) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsGuiTextBaseSmall_R : ToolsGuiTextBaseSmall) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile(ToolsGuiTextBaseBig : ToolsGuiTextBase) {
    fontSize = "28";
};
singleton GuiControlProfile(ToolsGuiTextBaseBig_C : ToolsGuiTextBaseBig) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsGuiTextBaseBig_R : ToolsGuiTextBaseBig) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------


//==============================================================================
// Standard Text Header Profiles
//==============================================================================
//==============================================================================
singleton GuiControlProfile(ToolsGuiTextHeaderBase : ToolsDefaultProfile) {
    fontColor = "254 254 254 255";
    fontType = "Gotham Black";
    fillColor = "238 236 240 255";
    bevelColorHL = "Magenta";
    justify = "Left";
    category = "LabText";
    fontColors[0] = "254 254 254 255";
    fontColors[1] = "254 254 254 255";
    fontColors[2] = "254 254 254 255";
    fontColorHL = "254 254 254 255";
    fontColorNA = "254 254 254 255";
    fontSize = "17";

};
singleton GuiControlProfile(ToolsGuiTextHeaderBase_C : ToolsGuiTextHeaderBase) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsGuiTextHeaderBase_R : ToolsGuiTextHeaderBase) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------

//==============================================================================
// Standard Text Profiles
//==============================================================================
singleton GuiControlProfile(ToolsGuiTextBase : ToolsDefaultProfile) {
    fontColor = "254 254 254 255";
    fontType = "Gotham Black";
    fillColor = "238 236 240 255";
    bevelColorHL = "Magenta";
    justify = "Left";
    category = "LabText";
    fontColors[0] = "254 254 254 255";
    fontColors[1] = "254 254 254 255";
    fontColors[2] = "254 254 254 255";
    fontColorHL = "254 254 254 255";
    fontColorNA = "254 254 254 255";
    fontSize = "15";

};
singleton GuiControlProfile(ToolsGuiTextBase_C : ToolsGuiTextBase) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsGuiTextBase_R : ToolsGuiTextBase) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile(ToolsGuiTextBaseSmall : ToolsGuiTextBase) {
    fontSize = "14";
    fontType = "Gotham Book";
};
singleton GuiControlProfile(ToolsGuiTextBaseSmall_C : ToolsGuiTextBaseSmall) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsGuiTextBaseSmall_R : ToolsGuiTextBaseSmall) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile(ToolsGuiTextBaseBig : ToolsGuiTextBase) {
    fontSize = "28";
};
singleton GuiControlProfile(ToolsGuiTextBaseBig_C : ToolsGuiTextBaseBig) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsGuiTextBaseBig_R : ToolsGuiTextBaseBig) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------


//==============================================================================
// Standard Text Header Profiles
//==============================================================================
//==============================================================================
singleton GuiControlProfile(ToolsGuiTextHeaderBase : ToolsDefaultProfile) {
    fontColor = "254 254 254 255";
    fontType = "Gotham Black";
    fillColor = "238 236 240 255";
    bevelColorHL = "Magenta";
    justify = "Left";
    category = "LabText";
    fontColors[0] = "254 254 254 255";
    fontColors[1] = "254 254 254 255";
    fontColors[2] = "254 254 254 255";
    fontColorHL = "254 254 254 255";
    fontColorNA = "254 254 254 255";
    fontSize = "16";

};
singleton GuiControlProfile(ToolsGuiTextHeaderBase_C : ToolsGuiTextHeaderBase) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsGuiTextHeaderBase_R : ToolsGuiTextHeaderBase) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------

//==============================================================================
// Dark text profile for Light background
//==============================================================================
//==============================================================================
singleton GuiControlProfile(ToolsGuiTextAlt : ToolsDefaultProfile) {
    fontColor = "254 224 97 255";
    fontType = "Gotham Black";
    fillColor = "238 236 240 255";
    bevelColorHL = "Magenta";
    justify = "left";
    category = "LabText";
   fontSize = "16";
   fontColors[0] = "254 224 97 255";

};
singleton GuiControlProfile(ToolsGuiTextAlt_C : ToolsGuiTextAlt) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsGuiTextAlt_R : ToolsGuiTextAlt) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile(ToolsGuiTextAltSmall : ToolsGuiTextAlt) {
    fontSize = "18";
};
singleton GuiControlProfile(ToolsGuiTextAltSmall_C : ToolsGuiTextAltSmall) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsGuiTextAltSmall_R : ToolsGuiTextAltSmall) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile(ToolsGuiTextAltBig : ToolsGuiTextAlt) {
    fontSize = "18";
   fontColors[0] = "222 175 51 255";
   fontColor = "222 175 51 255";
};
singleton GuiControlProfile(ToolsGuiTextAltBig_C : ToolsGuiTextAltBig) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsGuiTextAltBig_R : ToolsGuiTextAltBig) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------

//==============================================================================
// Standard Text Header Profiles
//==============================================================================
//==============================================================================
singleton GuiControlProfile(ToolsGuiTextHeaderAlt : ToolsDefaultProfile) {
    fontColor = "0 255 231 208";
    fontType = "Gotham Black";
    fillColor = "238 236 240 255";
    bevelColorHL = "Magenta";
    justify = "Left";
    category = "LabText";
    fontColors[0] = "0 255 231 208";
    fontColors[1] = "254 254 254 255";
    fontColors[2] = "254 254 254 255";
    fontColorHL = "254 254 254 255";
    fontColorNA = "254 254 254 255";
    fontSize = "24";

};
singleton GuiControlProfile(ToolsGuiTextHeaderAlt_C : ToolsGuiTextHeaderAlt) {
    locked = true;
    justify = "Center";
};
singleton GuiControlProfile(ToolsGuiTextHeaderAlt_R : ToolsGuiTextHeaderAlt) {
    locked = true;
    justify = "Right";
};
//------------------------------------------------------------------------------
