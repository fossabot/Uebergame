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

//-----------------------------------------------------------------------------
// Variables used by client scripts & code.  The ones marked with (c)
// are accessed from code.  Variables preceeded by Pref:: are client
// preferences and stored automatically in the ~/client/prefs.cs file
// in between sessions.
//
//    (c) Client::MissionFile             Mission file name
//    ( ) Client::Password                Password for server join

//    (?) Pref::Player::CurrentFOV
//    (?) Pref::Player::DefaultFov
//    ( ) Pref::Input::KeyboardTurnSpeed

//    (c) pref::Master[n]                 List of master servers
//    (c) pref::Net::RegionMask
//    (c) pref::Client::ServerFavoriteCount
//    (c) pref::Client::ServerFavorite[FavoriteCount]
//    .. Many more prefs... need to finish this off

// Moves, not finished with this either...
//    (c) firstPerson
//    $mv*Action...

//-----------------------------------------------------------------------------
// These are variables used to control the shell scripts and
// can be overriden by mods:
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
function Torque::initClient(%this)
{
   echo("\n--------- Initializing " @ $appName @ ": Client Scripts ---------");

   // Make sure this variable reflects the correct state.
   $Server::Dedicated = false;

   // Game information used to query the master server
   $Client::GameTypeQuery = $appName;
   $Client::MissionTypeQuery = "Any";
   $Client::GameType = ""; // PZ code

   // These should be game specific GuiProfiles.  Custom profiles are saved out
   // from the Gui Editor.  Either of these may override any that already exist.
   exec("~/gui/defaultGameProfiles.cs"); 
   exec("~/gui/customProfiles.cs");   
   // Default player key bindings
   exec("./default.bind.cs");
   
   if (isFile( GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/bindings.config.cs" ) )
      exec( GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/bindings.config.cs" );

   
   // The common module provides basic client functionality
//initBaseClient();
   // Base client functionality
   exec( "./message.cs" );
   exec( "./mission.cs" );
   exec( "./missionDownload.cs");
   exec( "./onmissiondownload.cs" );
   
   exec( "./renderManager.cs" );
   exec( "./lighting.cs" );
	exec( "./actionMap.cs" );
   
 // Client scripts
	exec("./serverConnection.cs");
   //exec("./callbacks.cs"); // TODO BKS PRZ
   // Client scripts
   exec("./playerList.cs");
   exec("./chatHud.cs");
  	
	exec("./client.cs");	
    exec("./messageHud.cs");
	//exec("./game.cs");
   	
	initRenderManager();
    initLightingSystems(); 

   // Use our prefs to configure our Canvas/Window
   configureCanvas();

 
   // Load up the shell GUIs
   //exec("~/gui/StartupGui.gui");// BKS #Missing
   exec("~/gui/mainMenuGui.gui");

   exec("~/gui/exitGameGui.gui");
   exec("~/gui/chooseLevelDlg.gui");
 //  exec("~/gui/serverOptionsDlg.gui");// BKS #Missing
   exec("~/gui/joinServerDlg.gui");
 //  exec("~/gui/ipJoinDlg.gui");// BKS #Missing
   exec("~/gui/optionsDlg.gui");
   exec("~/gui/loadingGui.gui");
   exec("~/gui/remapDlg.gui");
   exec("~/gui/endGameGui.gui");
   exec("~/gui/helpDlg.gui");
   
     // Load up the Game GUIs
   exec("~/gui/playGui.gui");
   exec("~/gui/hudlessGui.gui"); // For screen shots
   exec("~/gui/ChatHud.gui");
   //exec("~/gui/messageHud.gui");// BKS #Missing
   //exec("~/gui/scoreHud.gui");// BKS #Missing
   //exec("~/gui/voteHudDlg.gui");// BKS #Missing
  // exec("~/gui/clusterHud.gui");// BKS #Missing
   //exec("~/gui/quickChatHud.gui");// BKS #Missing
  // exec("~/gui/adminDlg.gui");// BKS #Missing
  // exec("~/gui/armoryHud.gui");// BKS #Missing
   //exec("~/gui/vehicleHud.gui");// BKS #Missing
   //exec("~/gui/bombTimerDlg.gui");// BKS #Missing
  // exec("~/gui/fireTeamHud.gui");// BKS #Missing
  // exec("~/gui/guiMusicPlayer.gui");// BKS #Missing
  // exec("~/gui/RecordingsDlg.gui");// BKS #Missing

   // Gui scripts
   //exec("~/gui/startupGui.cs");
  // exec("~/gui/mainMenuGui.cs");// BKS #Missing
   //exec("~/gui/recordings.cs");// BKS #Missing
   exec("~/gui/chooseLevelDlg.cs");

  // exec("~/gui/serverOptionsDlg.cs");// BKS #Missing
  // exec("~/gui/joinServerDlg.cs");// BKS #Missing
   exec("~/gui/optionsDlg.cs");
   exec("~/gui/loadingGui.cs");
   exec("~/gui/playGui.cs");
  // exec("~/gui/adminDlg.cs");// BKS #Missing
  // exec("~/gui/clusterHud.cs");// BKS #Missing
  // exec("~/gui/quickChatHud.cs");// BKS #Missing
  // exec("~/gui/objectiveHud.cs");// BKS #Missing
   //exec("~/gui/scoreHud.cs");// BKS #Missing
   //exec("~/gui/armoryHud.cs");// BKS #Missing
  // exec("~/gui/vehicleHud.cs");// BKS #Missing
  // exec("~/gui/fireTeamHud.cs");// BKS #Missing
   exec("~/gui/guiMusicPlayer.cs");

   loadMaterials();

   // Really shouldn't be starting the networking unless we are
   // going to connect to a remote server, or host a multi-player
   // game.
   setNetPort(0);

   // Copy saved script prefs into C++ code.
   setDefaultFov( $pref::Player::defaultFov );
   setZoomSpeed( $pref::Player::zoomSpeed );

   if( isScriptFile( expandFilename("./audioData.cs") ) )
      exec( "./audioData.cs" );

   // Start up the main menu... this is separated out into a
   // method for easier mod override.

   if ($startWorldEditor || $startGUIEditor) {
      // Editor GUI's will start up in the primary main.cs once
      // engine is initialized.
      return;
   }

   // Connect to server if requested.
   if ($JoinGameAddress !$= "") {
      // If we are instantly connecting to an address, load the
      // loading GUI then attempt the connect.
      tge.loadLoadingGui();
      connect($JoinGameAddress, "", $Pref::Player::Name);
   }
   else {
      // Otherwise go to the splash screen.
      Canvas.setCursor("DefaultCursor");
      tge.loadMainMenu();
   }   
}


//-----------------------------------------------------------------------------

function Torque::loadMainMenu(%this)
{
   // Startup the client with the Main menu...
   if (isObject( MainMenuGui ))
      Canvas.setContent( MainMenuGui );
   
   Canvas.setCursor("DefaultCursor");

   // first check if we have a level file to load
   if ($levelToLoad !$= "")
   {
      %levelFile = "levels/";
      %ext = getSubStr($levelToLoad, strlen($levelToLoad) - 3, 3);
      if(%ext !$= "mis")
         %levelFile = %levelFile @ $levelToLoad @ ".mis";
      else
         %levelFile = %levelFile @ $levelToLoad;

      // Clear out the $levelToLoad so we don't attempt to load the level again
      // later on.
      $levelToLoad = "";
      
      // let's make sure the file exists
      %file = findFirstFile(%levelFile);

      if(%file !$= "")
         createAndConnectToLocalServer("SinglePlayer", %file, $pref::Server::MissionType);
   }
}

function Torque::loadLoadingGui(%this, %displayText)
{
   Canvas.setContent("LoadingGui");
   LoadingProgress.setValue(1);

   if (%displayText !$= "")
   {
      LoadingProgressTxt.setValue(%displayText);
   }
   else
   {
      LoadingProgressTxt.setValue("WAITING FOR SERVER");
   }

   Canvas.repaint();
}
