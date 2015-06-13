//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

// If we got back no prefs path modification
if( $Gui::fontCacheDirectory $= "")
{
   $Gui::fontCacheDirectory = expandFilename( "art/fonts" );
}

// ----------------------------------------------------------------------------
// GuiDefaultProfile is a special profile that all other profiles inherit
// defaults from. It must exist.
// ----------------------------------------------------------------------------

if( !isObject( GuiDefaultProfile ) )
new GuiControlProfile (GuiDefaultProfile)
{
   tab = false;
   canKeyFocus = false;
   hasBitmapArray = false;
   mouseOverSelected = false;

   // fill color
   opaque = false;
   fillColor = "21 21 21 255";
   fillColorHL ="72 72 72 255";
   fillColorSEL = "72 12 0 255";
   fillColorNA = "18 18 18 255";

   // border color
   border = 0;
   borderColor   = "100 100 100"; 
   borderColorHL = "50 50 50 50";
   borderColorNA = "75 75 75"; 

   // font
   fontType = "Arial";
   fontSize = "14";
   fontCharset = ANSI;

   fontColor = "196 196 196 255";
   fontColorHL = "255 255 255 255";
   fontColorNA = "128 128 128 255";
   fontColorSEL= "196 116 108 255";

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

   // sounds
   //soundButtonDown = "";
   //soundButtonOver = "";
};

if( !isObject( GuiSolidDefaultProfile ) )
new GuiControlProfile (GuiSolidDefaultProfile)
{
   opaque = true;
   border = true;
   category = "Core";
};

if( !isObject( GuiTransparentProfile ) )
new GuiControlProfile (GuiTransparentProfile)
{
   opaque = false;
   border = false;
   category = "Core";
};

if( !isObject( GuiGroupBorderProfile ) )
new GuiControlProfile( GuiGroupBorderProfile )
{
   border = false;
   opaque = false;
   hasBitmapArray = true;
   bitmap = "art/gui/group-border";
   category = "Core";
};

if( !isObject( GuiTabBorderProfile ) )
new GuiControlProfile( GuiTabBorderProfile )
{
   border = false;
   opaque = false;
   hasBitmapArray = true;
   bitmap = "art/gui/tab-border";
   category = "Core";
};

if( !isObject( GuiToolTipProfile ) )
new GuiControlProfile (GuiToolTipProfile)
{
   // fill color
   fillColor = "72 72 72";

   // border color
   borderColor   = "196 196 196 255";

   // font
   fontType = "Arial";
   fontSize = 14;
   fontColor = "255 255 255 255";

   category = "Core";
};

if( !isObject( GuiModelessDialogProfile ) )
new GuiControlProfile( GuiModelessDialogProfile )
{
   modal = false;
   category = "Core";
};

if( !isObject( GuiFrameSetProfile ) )
new GuiControlProfile (GuiFrameSetProfile)
{
   fillcolor = "21 21 21 255";
   borderColor = "128 128 128";
   border = 1;
   opaque = true;
   border = true;
   category = "Core";
};

if( !isObject( GuiWindowProfile ) )
new GuiControlProfile (GuiWindowProfile)
{
   opaque = false;
   border = 2;
   fillColor = "32 32 32 255";
   fillColorHL = "72 72 72 255";
   fillColorNA = "18 18 18 255";
   fontColor = "196 196 196 255";
   fontColorHL = "255 255 255 255";
   bevelColorHL = "255 255 255";
   bevelColorLL = "0 0 0";
   text = "untitled";
   bitmap = "art/gui/window";
   textOffset = "8 4";
   hasBitmapArray = true;
   justify = "left";
   category = "Core";
   fontColors[0] = "196 196 196 255";
   fontColors[1] = "255 255 255 255";
   fontColors[2] = "128 128 128 255";
   fontColorNA = "128 128 128 255";
   fillColorSEL = "72 12 0 255";
   borderThickness = "0";
   fontColors[4] = "160 72 64 255";
   fontColors[5] = "196 116 108 255";
   fontColorLink = "160 72 64 255";
   fontColorLinkHL = "196 116 108 255";
   locked = "1";
};

if( !isObject( GuiInputCtrlProfile ) )
new GuiControlProfile( GuiInputCtrlProfile )
{
   tab = true;
   canKeyFocus = true;
   category = "Core";
};

if( !isObject( GuiTextProfile ) )
new GuiControlProfile (GuiTextProfile)
{
   justify = "left";
   fontColor = "196 196 196 255";
   category = "Core";
};

if( !isObject( GuiTextRightProfile ) )
new GuiControlProfile (GuiTextRightProfile : GuiTextProfile)
{
   justify = "right";
   category = "Core";
};

if( !isObject( GuiAutoSizeTextProfile ) )
new GuiControlProfile (GuiAutoSizeTextProfile)
{
   fontColor = "196 196 196 255";
   autoSizeWidth = true;
   autoSizeHeight = true;   
   category = "Core";
};

if( !isObject( GuiMediumTextProfile ) )
new GuiControlProfile( GuiMediumTextProfile : GuiTextProfile )
{
   fontSize = 24;
   category = "Core";
};

if(!isObject(GuiMediumTextRightProfile))
new GuiControlProfile(GuiMediumTextRightProfile : GuiTextProfile)
{
   fontSize = 24;
   justify = "right";
   category = "Core";
};

if(!isObject(GuiMediumTextCenterProfile))
new GuiControlProfile(GuiMediumTextCenterProfile : GuiTextProfile)
{
   fontSize = 24;
   justify = "center";
   category = "Core";
};

if( !isObject( GuiBigTextProfile ) )
new GuiControlProfile( GuiBigTextProfile : GuiTextProfile )
{
   fontSize = 36;
   category = "Core";
};

if(!isObject(GuiBigTextRightProfile))
new GuiControlProfile(GuiBigTextRightProfile : GuiTextProfile)
{
   fontSize = 36;
   justify = "right";
   category = "Core";
};

if(!isObject(GuiBigTextCenterProfile))
new GuiControlProfile(GuiBigTextCenterProfile : GuiTextProfile)
{
   fontSize = 36;
   justify = "center";
   category = "Core";
};

if( !isObject( GuiMLTextProfile ) )
new GuiControlProfile( GuiMLTextProfile )
{
   fontColorLink = "160 72 64 255";
   fontColorLinkHL = "196 116 108 255";
   autoSizeWidth = true;
   autoSizeHeight = true;  
   border = false;
   category = "Core";
};

if( !isObject( GuiTextArrayProfile ) )
new GuiControlProfile( GuiTextArrayProfile : GuiTextProfile )
{
   fontColor = "196 196 196 255";
   fontColorSEL = "100 100 100 255";
   fontColorHL = "211 211 211 255";
   fontColorNA = "105 105 105 255";
   
   fillColor ="0 0 0";
   fillColorHL = "105 105 105";
   fillColorSEL = "105 105 105";
   border = false;
   category = "Core";
};

if( !isObject( GuiTextEditProfile ) )
new GuiControlProfile( GuiTextEditProfile )
{
   opaque = true;
   bitmap = "art/gui/textEdit";
   hasBitmapArray = true; 
   border = -2; // fix to display textEdit img
   //borderWidth = "1";  // fix to display textEdit img
   //borderColor = "100 100 100";
   fillColor = "21 21 21 255";
   fillColorHL = "72 72 72 255";
   fontColor = "196 196 196 255";
   fontColorHL = "255 255 255";
   fontColorSEL = "72 12 0 255";
   fontColorNA = "128 128 128 255";
   textOffset = "4 2";
   autoSizeWidth = false;
   autoSizeHeight = true;
   justify = "left";
   tab = true;
   canKeyFocus = true;   
   category = "Core";
};

if( !isObject( GuiProgressProfile ) )
new GuiControlProfile( GuiProgressProfile )
{
   opaque = false;
   fillColor = "44 152 162 100";
   border = true;
   borderColor   = "78 88 120";

//> Text parameters
   fontColor = "255 255 255";
   fontType = "Arial Bold";
   fontSize = 20;
   justify = "center";
   //autoSizeWidth = true;
   //autoSizeHeight = true;
//<
   category = "Core";
};

if( !isObject( GuiProgressBitmapProfile ) )
new GuiControlProfile( GuiProgressBitmapProfile )
{
   border = false;
   hasBitmapArray = true;
   bitmap = "art/gui/loadingbar";
   category = "Core";
};

if( !isObject( GuiProgressTextProfile ) )
new GuiControlProfile( GuiProgressTextProfile )
{
   fontSize = "16";
	fontType = "Arial";
   fontColor = "196 196 196 255";
   justify = "center";
   category = "Core";   
};

new GuiControlProfile( GuiButtonProfile )
{
   opaque = true;
   border = true;

   fontColor = "224 224 224 255";
   fontColorHL = "255 255 255 255";
   fontColorNA = "64 64 64 255";
   //fontColorSEL ="0 0 0";
   fixedExtent = false;
   justify = "center";
   canKeyFocus = false;
   bitmap = "art/gui/button";
   hasBitmapArray = false;
   category = "Core";
};

if( !isObject( GuiMenuButtonProfile ) )
new GuiControlProfile( GuiMenuButtonProfile )
{
   opaque = true;
   border = "1";
   fontSize = "24";
   fontType = "Arial Bold";
   fontColor = "93 85 54 255";
   fontColorHL = "230 127 25 255";
   fontColorNA = "64 64 64 255";
   //fontColorSEL ="0 0 0";
   fixedExtent = 0;
   justify = "center";
   canKeyFocus = false;
   hasBitmapArray = false;
   category = "Core";
   fillColor = "20 20 20 255";
   fillColorHL = "20 18 14 255";
   fillColorNA = "20 20 20 255";
   fillColorSEL = "90 90 90 255";
   fontColors[0] = "93 85 54 255";
   fontColors[2] = "64 64 64 255";
   fontColors[1] = "230 127 25 255";
   borderThickness = "3";
   borderColor = "50 50 50 255";
   borderColorHL = "64 54 45 255";
   bevelColorHL = "30 30 30 255";
};

if( !isObject( GuiButtonTabProfile ) )
new GuiControlProfile( GuiButtonTabProfile )
{
   opaque = true;
   border = true;
   fontColor = "224 224 224 255";
   fontColorHL = "255 255 255 255";
   fontColorNA = "64 64 64 255";
   fixedExtent = false;
   justify = "center";
   canKeyFocus = false;
   bitmap = "art/gui/buttontab";
   category = "Core";
};

if( !isObject( GuiCheckBoxProfile ) )
new GuiControlProfile( GuiCheckBoxProfile )
{
   opaque = false;
   fillColor = "116 116 116 255";
   border = false;
   borderColor = "100 100 100";
   fontSize = "16";
   fontColor = "196 196 196 255";
   fontColorHL = "255 255 255 255";
	fontColorNA = "128 128 128 255";
   fixedExtent = 1;
   justify = "left";
   bitmap = "art/gui/checkbox";
   hasBitmapArray = true;
   category = "Core";
};

if( !isObject( GuiScrollProfile ) )
new GuiControlProfile( GuiScrollProfile )
{
   opaque = true;
   fillcolor = "21 21 21 255";
   fontColor = "180 180 180 255";
   fontColorHL = "255 255 255 255";
   //borderColor = GuiDefaultProfile.borderColor;
   border = "1";
   bitmap = "art/gui/scrollBar";
   hasBitmapArray = true;
};

if( !isObject( GuiOverlayProfile ) )
new GuiControlProfile( GuiOverlayProfile )
{
   opaque = true;
   fillcolor = "0 0 0 100";
   fontColor = "0 0 0";
   fontColorHL = "255 255 255";
	fillColor = "0 0 0 100";
   category = "Core";
   fontColors[0] = "0 0 0 255";
};

if( !isObject( GuiSliderProfile ) )
new GuiControlProfile( GuiSliderProfile )
{
   bitmap = "art/gui/slider";
   category = "Core";
};

if( !isObject( GuiSliderBoxProfile ) )
new GuiControlProfile( GuiSliderBoxProfile )
{
   bitmap = "art/gui/slider-w-box";
   category = "Core";
};

// ----------------------------------------------------------------------------
// TODO: Revisit Popupmenu
// ----------------------------------------------------------------------------

if( !isObject( GuiPopupMenuItemBorder ) )
new GuiControlProfile( GuiPopupMenuItemBorder : GuiButtonProfile )
{
   opaque = true;
   border = "1";
   fontColor = "196 196 196 255";
   fontColorHL = "128 128 128 255";
   fontColorNA = "24 24 24 255";
   justify = "center";
   canKeyFocus = false;
   bitmap = "art/gui/button";
   category = "Core";
};

new GuiControlProfile( GuiPopUpMenuDefault )
{
   bitmap = "art/gui/scrollBar";
   category = "Tools";
   mouseOverSelected = "1";
   opaque = "1";
   borderThickness = "0";
   textOffset = "3 3";
   hasBitmapArray = "1";
   profileForChildren = "GuiPopupMenuItemBorder";
   fixedExtent = "1";
   fillColorHL = "72 72 72 255";
   fillColorNA = "18 18 18 255";
   fillColorSEL = "116 116 116 255";
   fontColors[0] = "196 196 196 255";
   fontColors[1] = "255 255 255 255";
   fontColors[2] = "128 128 128 255";
   fontColors[4] = "160 72 64 255";
   fontColors[5] = "196 116 108 255";
   fontColor = "196 196 196 255";
   fontColorHL = "255 255 255 255";
   fontColorNA = "128 128 128 255";
   fontColorLink = "160 72 64 255";
   fontColorLinkHL = "196 116 108 255";
};

if( !isObject( GuiPopUpMenuDefault ) )
new GuiControlProfile( GuiTabBookProfile : GuiDefaultProfile )
{
   opaque = "0";
   mouseOverSelected = "0";
   textOffset = "0 -3";
   border = 0;
   borderThickness = "1";
   fixedExtent = 1;
   bitmap = "art/gui/tab";
   hasBitmapArray = "0";
   profileForChildren = GuiPopupMenuItemBorder;
   fillColor = "21 21 21 255";
   fillColorHL = "100 100 100 255";
   fillColorSEL = "72 12 0 255";
   fontColorHL = "255 255 255 255";
   fontColorSEL = "255 255 255 255";
   borderColor = "100 100 100";
   category = "Core";
   fillColorNA = "150 150 150 255";
   fontColors[0] = "240 240 240 255";
   fontColors[1] = "255 255 255 255";
   fontColors[2] = "128 128 128 255";
   fontColors[3] = "255 255 255 255";
   fontColors[4] = "255 0 255 255";
   fontColors[5] = "255 0 255 255";
   fontColor = "240 240 240 255";
   fontColorNA = "128 128 128 255";
   fontColorLink = "255 0 255 255";
   fontColorLinkHL = "255 0 255 255";
   tab = "1";
   canKeyFocus = "1";
   justify = "Center";
   tabRotation = "Horizontal";
   tabHeight = "24";
   tabPosition = "Top";
   tabWidth = "64";
};

if( !isObject( GuiPopUpMenuProfile ) )
new GuiControlProfile( GuiPopUpMenuProfile : GuiPopUpMenuDefault )
{
   textOffset         = "6 4";
   bitmap             = "art/gui/dropDown";
   hasBitmapArray     = true;
   border             = "0";
   profileForChildren = GuiPopUpMenuDefault;
   category = "Core";
   fontColors[0] = "196 196 196 255";
   fontColors[1] = "255 255 255 255";
   fontColor = "196 196 196 255";
   fontColorHL = "255 255 255 255";
   fontColors[2] = "128 128 128 255";
   fontColorNA = "128 128 128 255";
   fontCharset = "ANSI";
   fontColors[3] = "255 255 255 255";
   fontColorSEL = "255 255 255 255";
   fontColors[4] = "160 72 64 255";
   fontColors[5] = "196 116 108 255";
   fontColorLink = "160 72 64 255";
   fontColorLinkHL = "196 116 108 255";
   fillColor = "21 21 21 255";
   fillColorHL = "72 72 72 255";
   fillColorNA = "18 18 18 255";
   fillColorSEL = "116 116 116 255";
};

if( !isObject( GuiTabBookProfile ) )
new GuiControlProfile( GuiTabBookProfile )
{
   fillColorHL = "100 100 100";
   fillColorNA = "150 150 150";
   fontColor = "30 30 30";
   fontColorHL = "0 0 0";
   fontColorNA = "50 50 50";
   fontType = "Arial";
   fontSize = 14;
   justify = "center";
   bitmap = "art/gui/tab";
   tabWidth = 64;
   tabHeight = 24;
   tabPosition = "Top";
   tabRotation = "Horizontal";
   textOffset = "0 -3";
   tab = true;
   cankeyfocus = true;
   category = "Core";
};

if( !isObject( GuiTabPageProfile ) )
new GuiControlProfile( GuiTabPageProfile : GuiDefaultProfile )
{
   fontType = "Arial";
   fontSize = 10;
   justify = "center";
   bitmap = "art/gui/tab";
   opaque = false;
   fillColor = "21 21 21 255";
   category = "Core";
   fontColors[3] = "255 255 255 255";
   fontColorSEL = "255 255 255 255";
   fontColors[0] = "196 196 196 255";
   fontColor = "196 196 196 255";
};

if( !isObject( GuiConsoleProfile ) )
new GuiControlProfile( GuiConsoleProfile )
{
   fontType = ($platform $= "macos") ? "Monaco" : "Lucida Console";
   fontSize = ($platform $= "macos") ? 13 : 12;
   fontColor = "255 255 255";
   fontColorHL = "0 255 255";
   fontColorNA = "255 0 0";

   fontColors[1] = "127 255 212"; // aquamarine - warn()
   fontColors[2] = "255 128 128"; // pink - error()
   fontColors[3] = "240 230 140"; // khaki
   fontColors[4] = "0 191 255"; // deepskyblue
   fontColors[5] = "255 215 0"; // gold

   fontColors[6] = "100 100 100";
   fontColors[7] = "100 100 0";
   fontColors[8] = "0 0 100";
   fontColors[9] = "0 100 0";
   category = "Core";
};

if( !isObject( GuiConsoleTextProfile ) )
new GuiControlProfile( GuiConsoleTextProfile )
{   
   fontColor = "196 196 196";
   autoSizeWidth = true;
   autoSizeHeight = true;   
   textOffset = "2 2";
   opaque = true;   
   fillColor = "21 21 21 255";
   border = true;
   borderThickness = 1;
   borderColor = "128 128 128";
   category = "Core";
};

$ConsoleDefaultFillColor = "0 0 0 175";

if( !isObject( ConsoleScrollProfile ) )
new GuiControlProfile( ConsoleScrollProfile : GuiScrollProfile )
{
	opaque = true;
	fillColor = $ConsoleDefaultFillColor;
	border = 1;
	//borderThickness = 0;
	borderColor = "0 0 0";
   category = "Core";
};

if( !isObject( ConsoleTextEditProfile ) )
new GuiControlProfile( ConsoleTextEditProfile : GuiTextEditProfile )
{
   fillColor = "242 241 240 255";
   fillColorHL = "255 255 255";   
   category = "Core";
};

if( !isObject( ConsoleErrorTextProfile ) )
new GuiControlProfile( ConsoleErrorTextProfile )
{
   fontColor = "225 225 225";
   fontColorLink = "100 100 100";
   fontColorLinkHL = "255 255 255";
   autoSizeWidth = true;
   autoSizeHeight = true;  
   border = false;
   category = "Core";
};

//-----------------------------------------------------------------------------
// Center and bottom print
//-----------------------------------------------------------------------------

if( !isObject( CenterPrintProfile ) )
new GuiControlProfile ( CenterPrintProfile )
{
   opaque = false;
   fillColor = "128 128 128";
   fontColor = "255 255 128";
   border = true;
   borderWidth = "5";
   borderColor = "66 90 103";
   category = "Core";
};

if( !isObject( CenterPrintTextProfile ) )
new GuiControlProfile ( CenterPrintTextProfile )
{
   opaque = false;
   fontType = "Arial Bold";
   fontSize = 16;
   fontColor = "255 255 255";
   category = "Core";
};

// ----------------------------------------------------------------------------
// Radio button control
// ----------------------------------------------------------------------------

if( !isObject( GuiRadioProfile ) )
new GuiControlProfile( GuiRadioProfile )
{
   // +++ changed Dark UI 1.3
   fontSize = 14;
   fillColor = "32 32 32 255";
   fontColor = "196 196 196 255";
   fontColorHL = "255 255 255 255";
   fixedExtent = 1;
   bitmap = "art/gui/radioButton";
   hasBitmapArray = true;
   category = "Tools";
   modal = "1";
};

if(!isObject(GuiMainMenuBarProfile)) new GuiControlProfile (GuiMainMenuBarProfile)
{
   opaque = true;

   fontType = "Arial Bold";
   fontSize = 14;

// lightgrey: 211 211 211
// gray: 190 190 190
// darkgray: 169 169 169
// dimgray: 105 105 105

   fontColor = "190 190 190";
   fontColorHL = "211 211 211";
   fontColorNA = "169 169 169";

   fillColor = "0 0 0";
   fillColorHL = "105 105 105";

   border = true;
   borderThickness = 4;
   borderColor   = "105 105 105";
   borderColorHL = "169 169 169";
   borderColorNA = "64 64 64";

   textOffset = "6 6";
   fixedExtent = true;
   justify = "center";
   canKeyFocus = false;
   mouseOverSelected = true;
   bitmap = "art/gui/torqueMenu";
   hasBitmapArray = true;
};

//-----------------------------------------------------------------------------
// Black text profiles

if( !isObject( GuiTextProfile ) )
new GuiControlProfile (GuiTextProfile)
{
   justify = "left";
   fontColor = "20 20 20";
   category = "Core";
};

if( !isObject( GuiTextRightProfile ) )
new GuiControlProfile (GuiTextRightProfile : GuiTextProfile)
{
   justify = "right";
   category = "Core";
};

if( !isObject( GuiTextCenterProfile ) )
new GuiControlProfile (GuiTextCenterProfile : GuiTextProfile)
{
   justify = "center";
   category = "Core";
};

if( !isObject( GuiAutoSizeTextProfile ) )
new GuiControlProfile (GuiAutoSizeTextProfile)
{
   fontColor = "0 0 0";
   autoSizeWidth = true;
   autoSizeHeight = true;   
   category = "Core";
};

if( !isObject( GuiMediumTextProfile ) )
new GuiControlProfile( GuiMediumTextProfile : GuiTextProfile )
{
   fontSize = 24;
   category = "Core";
};

if(!isObject(GuiMediumTextRightProfile))
new GuiControlProfile(GuiMediumTextRightProfile : GuiTextProfile)
{
   fontSize = 24;
   justify = "right";
   category = "Core";
};

if(!isObject(GuiMediumTextCenterProfile))
new GuiControlProfile(GuiMediumTextCenterProfile : GuiTextProfile)
{
   fontSize = 24;
   justify = "center";
   category = "Core";
};

if( !isObject( GuiBigTextProfile ) )
new GuiControlProfile( GuiBigTextProfile : GuiTextProfile )
{
   fontSize = 36;
   category = "Core";
};

if(!isObject(GuiBigTextRightProfile))
new GuiControlProfile(GuiBigTextRightProfile : GuiTextProfile)
{
   fontSize = 36;
   justify = "right";
   category = "Core";
};

if(!isObject(GuiBigTextCenterProfile))
new GuiControlProfile(GuiBigTextCenterProfile : GuiTextProfile)
{
   fontSize = 36;
   justify = "center";
   category = "Core";
};

//-----------------------------------------------------------------------------
// White text profiles

if(!isObject(GuiTxtWhtProfile)) new GuiControlProfile(GuiTxtWhtProfile : GuiTextProfile)
{
   fontColor = "255 255 255";
   category = "Text";
};

if(!isObject(GuiBoldTextWhtProfile)) new GuiControlProfile(GuiBoldTextWhtProfile : GuiTextProfile)
{
   fontType = "Arial Bold";
   fontColor = "255 255 255";
   category = "Text";
};

if(!isObject(GuiTxtCenterWhtProfile)) new GuiControlProfile(GuiTxtCenterWhtProfile : GuiTextProfile)
{
   fontColor = "255 255 255";
   justify = "center";
   category = "Text";
};

if(!isObject(GuiTxtRightWhtProfile)) new GuiControlProfile(GuiTxtRightWhtProfile : GuiTextProfile)
{
   fontColor = "255 255 255";
   justify = "right";
   category = "Text";
};

if(!isObject(GuiMedTxtWhtProfile)) new GuiControlProfile(GuiMedTxtWhtProfile : GuiTextProfile)
{
   fontColor = "255 255 255";
   fontSize = 24;
   category = "Text";
};

if(!isObject(GuiMediumTextWhtRightProfile)) new GuiControlProfile(GuiMediumTextWhtRightProfile : GuiTextProfile)
{
   fontColor = "255 255 255";
   fontSize = 24;
   justify = "right";
   category = "Text";
};

if(!isObject(GuiMediumTextWhtCenterProfile)) new GuiControlProfile(GuiMediumTextWhtCenterProfile : GuiTextProfile)
{
   fontColor = "255 255 255";
   fontSize = 24;
   justify = "center";
   category = "Text";
};

if(!isObject(GuiBigTxtWhtProfile)) new GuiControlProfile(GuiBigTxtWhtProfile : GuiTextProfile)
{
   fontColor = "255 255 255";
   fontSize = 36;
   category = "Text";
};

if(!isObject(GuiBigTxtRightWhtProfile)) new GuiControlProfile(GuiBigTxtRightWhtProfile : GuiTextProfile)
{
   fontColor = "255 255 255";
   fontSize = 36;
   justify = "right";
   category = "Text";
};

if(!isObject(GuiBigTxtCenterWhtProfile)) new GuiControlProfile(GuiBigTxtCenterWhtProfile : GuiTextProfile)
{
   fontColor = "255 255 255";
   fontSize = 36;
   justify = "center";
   category = "Text";
};

//-----------------------------------------------------------------------------
// Yellow text profiles

if(!isObject(GuiTxtYelProfile)) new GuiControlProfile(GuiTxtYelProfile : GuiTextProfile)
{
   fontColor = "255 255 128";
   category = "Text";
};

if(!isObject(GuiBoldTextYelProfile)) new GuiControlProfile(GuiBoldTextYelProfile : GuiTextProfile)
{
   fontType = "Arial Bold";
   fontColor = "255 255 128";
   category = "Text";
};

if(!isObject(GuiTxtCenterYelProfile)) new GuiControlProfile(GuiTxtCenterYelProfile : GuiTextProfile)
{
   fontColor = "255 255 128";
   justify = "center";
   category = "Text";
};

if(!isObject(GuiTxtRightYelProfile)) new GuiControlProfile(GuiTxtRightYelProfile : GuiTextProfile)
{
   fontColor = "255 255 128";
   justify = "right";
   category = "Text";
};

if(!isObject(GuiMedTxtYelProfile)) new GuiControlProfile(GuiMedTxtYelProfile : GuiTextProfile)
{
   fontColor = "255 255 128";
   fontSize = 24;
   category = "Text";
};

if(!isObject(GuiMediumTextYelRightProfile)) new GuiControlProfile(GuiMediumTextYelRightProfile : GuiTextProfile)
{
   fontColor = "255 255 128";
   fontSize = 24;
   justify = "right";
   category = "Text";
};

if(!isObject(GuiMediumTextYelCenterProfile)) new GuiControlProfile(GuiMediumTextYelCenterProfile : GuiTextProfile)
{
   fontColor = "255 255 128";
   fontSize = 24;
   justify = "center";
   category = "Text";
};

if(!isObject(GuiBigTxtYelProfile)) new GuiControlProfile(GuiBigYelWhtProfile : GuiTextProfile)
{
   fontColor = "255 255 128";
   fontSize = 36;
   category = "Text";
};

if(!isObject(GuiBigTxtRightYelProfile)) new GuiControlProfile(GuiBigTxtRightYelProfile : GuiTextProfile)
{
   fontColor = "255 255 128";
   fontSize = 36;
   justify = "right";
   category = "Text";
};

if(!isObject(GuiBigTxtCenterYelProfile)) new GuiControlProfile(GuiBigTxtCenterYelProfile : GuiTextProfile)
{
   fontSize = 36;
   fontColor = "255 255 128";
   justify = "center";
   category = "Text";
};

//-----------------------------------------------------------------------------

if( !isObject( GuiHEaderTextProfile ) )
new GuiControlProfile (GuiHeaderTextProfile)
{
   fontType = "Arial";
   fontSize = 36;
   fontColor = "255 255 255";
   justify = "center";
   category = "Text";
};
