// Load our special main menu GUI that will include a
// loaded level in the background.
function loadMainMenu()
{
   // We need to track when we're displaying the
   // main menu level
   $UsingMainMenuLevel = true;
      // Startup the client with the Main menu...
   Canvas.setContent( MainMenuGui );
   // Load the main menu level.  We could have this be
   // chosen from a random list of levels if we wanted
   // a different one each time the user returned to
   // the main menu.
   loadLevel("tools/levels/mainMenuLevel.mis");
}
// Load a single player level on the local server.  This
// function differs from the standard one by displaying
// our splash screen rather than the normal loading screen.
function loadLevel( %missionNameOrFile )
{
   // Expand the mission name... this allows you to enter
   // just the name and not the full path and extension.
   %missionFile = expandMissionFileName( %missionNameOrFile );
   if ( %missionFile $= "" )
      return false;
   // Show the splash screen screen immediately.
   Canvas.setContent("MainMenuLevelSplashGui");
   Canvas.repaint();
   // Prepare and launch the server.
   return createAndConnectToLocalServer( "SinglePlayer", %missionFile );
}
// This function is called each time the main menu
// level's progress should be updated.  We're using a
// static splash screen that doesn't require to be
// notified of the loading progress.
function loadLoadingGui(%displayText)
{
   // Do nothing as our splash screen does not
   // display progress.
}
// This function is called any time the user disconnects
// from a level, including our main menu level. The
// only difference between this function and the
// standard one is that we check if we should switch
// to the main menu GUI and level, or if we just came
// from there.
function disconnectedCleanup()
{
   // End mission, if it's running.
      if( $Client::missionRunning )
      clientEndMission();
   // Disable mission lighting if it's going, this is
   // here in case we're disconnected while the mission
   // is loading.
   $lightingMission = false;
   $sceneLighting::terminateLighting = true;
   // Clear misc script stuff
   HudMessageVector.clear();
   //
   LagIcon.setVisible(false);
   PlayerListGui.clear();
   // Clear all print messages
   clientCmdclearBottomPrint();
   clientCmdClearCenterPrint();
   // We can now delete the client physics simulation.
   physicsDestroyWorld( "client" );                 
   if ($UsingMainMenuLevel)
   {
      // We are displaying the main menu level so we
      // don't want to load it again.  The standard
      // level loading code will now start the chosen
      // level once we exit this function.
      $UsingMainMenuLevel = false;
   }
   else
   {
      // We've just come from a standard level, so
      // load the main menu.  We need to do this
      // as a schedule() to allow the previous level
      // to completely clean itself up.
      schedule(0, 0, loadMainMenu);
   }
}