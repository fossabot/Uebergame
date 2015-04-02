//==============================================================================
// GameLab Helpers -> Console Reporting helpers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================



//==============================================================================
function compileAll(%quit) {
    echo(" --- Compiling all files ---");
   compileFiles("*.cs");
   compileFiles("*.gui");
   compileFiles("*.ts");  
   echo(" --- Exiting after compile ---");
   if (%quit)
      quit();
	
}
//------------------------------------------------------------------------------
//==============================================================================
function compileTools(%quit) {
   echo(" --- Compiling tools scritps ---");
   compileFiles("tools/*.cs");
   compileFiles("tools/*.gui");
   compileFiles("tools/*.ts");  
   echo(" --- Exiting after compile ---");
   if (%quit)
      quit();
	
}
//------------------------------------------------------------------------------
//==============================================================================
function compileGame(%quit) {
   echo(" --- Compiling tools scritps ---");
   compileFiles("scripts/server/*.cs");
   compileFiles("scripts/FPS/*.cs");
   compileFiles("scripts/FPS/*.gui");
   compileFiles("scripts/RTS/*.cs");
   compileFiles("scripts/RTS/*.gui");
   compileFiles("scripts/game/*.cs");
   compileFiles("scripts/gui/*.cs");
   compileFiles("scripts/gui/*.gui");
   compileFiles("scripts/client/gfx/*.cs");
     compileFiles("scripts/helpers/game/*.cs");
    compileFiles("scripts/helpers/guiControls/*.cs");
   compileFiles("scripts/helpers/ObjectClass/*.cs");
   compileFiles("scripts/helpers/Params/*.cs");
   compileFiles("tools/*.cs");
   compileFiles("tools/*.gui");
   echo(" --- Exiting after compile ---");
   if (%quit)
      quit();
	
}
//------------------------------------------------------------------------------
//==============================================================================
function compilePat(%patternList) {
   echo(" --- Compiling PatternList ---");
   foreach$ (%pattern in %patternList) {
       echo(" --- Compiling Pattern=> "SPC %pattern SPC" ---");
      compileFiles(%pattern);
   }
 
   echo(" --- Exiting after compile ---");
   if (%quit)
      quit();
	
}
//------------------------------------------------------------------------------
if($compileAll)
{
  
}

if($compileTools)
{
   echo(" --- Compiling tools scritps ---");
   compileFiles("tools/*.cs");
   compileFiles("tools/*.gui");
   compileFiles("tools/*.ts");  
   echo(" --- Exiting after compile ---");
   quit();
}