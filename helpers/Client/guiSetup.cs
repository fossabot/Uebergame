//==============================================================================
// Helpers Lab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
//! getRealCursorPos() > Get the Cursor position inside the engine window
//+ Canvas.getCursorPos() return the cursor position on the user screen, with this function
//+ you will get the cursor position inside Torque3D window.
//+ Usage: %realPos = getRealCursorPos();
function getRealCursorPos() {
	%cursorPos = Canvas.getCursorPos();
	%windowPos = Canvas.getWindowPosition();
	%realPos = getWords(VectorSub(%cursorPos,%windowPos),0,1);
	
	return %realPos;  
}
//------------------------------------------------------------------------------

//==============================================================================
// Change the font for all profiles
function storeProfilesDefaults(%storeList) {
   
   if (%storeList $= "")
      %storeList = "fontSize";
      
	foreach( %obj in GuiDataGroup ) {
		if( !%obj.isMemberOfClass( "GuiControlProfile" ) )
			continue;
      
      foreach$(%setting in %storeList){
         %value = %obj.getFieldValue(%setting);
         $GuiProfilesDefault[%obj.getName(),%setting] = %value;
         
         devLog(%obj.getName(),"Setting",%setting,"Default is",%value);
      }
		
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Change the font for all profiles
function updateProfilesSetting(%storeList) {
   //$pref::Video::mode = "1280 720 false 32 60 4";
   %screenX = getWord($pref::Video::mode,0);
   %screenY = getWord($pref::Video::mode,1);
   
   
   $GuiScreenRatioX = %screenX / 1024;
   $GuiScreenRatioY = %screenY / 768;   
  devLog("Current X=",%screenX,"Ratio X",$GuiScreenRatioX);
   devLog("Current Y=",%screenY,"Ratio Y",$GuiScreenRatioY);   
	foreach( %obj in GuiDataGroup ) {
		if( !%obj.isMemberOfClass( "GuiControlProfile" ) )
			continue;
      
      %dFontSize = $GuiProfilesDefault[%obj.getName(),"fontSize"];
      %newFontSize = %dFontSize * $GuiScreenRatioX;
      %ceilFontSize = mCeil(%newFontSize);
      devLog(%obj.getName(),"=> DefaultFontSize:",%dFontSize,"RatioSize",%newFontSize,"New Size",%ceilFontSize);
      
		
	}
}
//------------------------------------------------------------------------------