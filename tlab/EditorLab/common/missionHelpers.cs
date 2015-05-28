//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
/*
 LabMenu.addMenu("MyFirstMenu",0);
   LabMenu.addMenuItem(0,"MyFirstItem",0,"Ctrl numpad0",-1);
   LabMenu.addSubmenuItem("MyFirstMenu","MyFirstItem","MyFirstSubItem",0,"",-1);
   */


//==============================================================================
function Lab::setCurrentViewAsPreview(%this) {
  setGui(HudlessPlayGui);
  %name = expandFileName( filePath(MissionGroup.getFilename())@"/"@fileBase(MissionGroup.getFilename()));	
	takeLevelScreenShot(%name);
	schedule(500,0,"setGui",EditorGui);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setNextScreenShotPreview(%this) {
   $LabNextScreenshotIsPreview = true;
}
//------------------------------------------------------------------------------
