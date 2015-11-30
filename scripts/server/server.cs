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

// Variables used by server scripts & code.  The ones marked with (c)
// are accessed from code.  Variables preceeded by Pref:: are server
// preferences and stored automatically in the ServerPrefs.cs file
// in between server sessions.
//
//    (c) Server::ServerType              {SinglePlayer, MultiPlayer}
//    (c) Server::GameType                Unique game name
//    (c) Server::Dedicated               Bool
//    ( ) Server::MissionFile             Mission .mis file name
//    (c) Server::MissionName             DisplayName from .mis file
//    (c) Server::MissionType             Not used
//    (c) Server::PlayerCount             Current player count
//    (c) Server::GuidList                Player GUID (record list?)
//    (c) Server::Status                  Current server status
//
//    (c) Pref::Server::Name              Server Name
//    (c) Pref::Server::Password          Password for client connections
//    ( ) Pref::Server::AdminPassword     Password for client admins
//    (c) Pref::Server::Info              Server description
//    (c) Pref::Server::MaxPlayers        Max allowed players
//    (c) Pref::Server::RegionMask        Registers this mask with master server
//    ( ) Pref::Server::BanTime           Duration of a player ban
//    ( ) Pref::Server::KickBanTime       Duration of a player kick & ban
//    ( ) Pref::Server::MaxChatLen        Max chat message len
//    ( ) Pref::Server::FloodProtectionEnabled Bool

//-----------------------------------------------------------------------------
function Torque::initServer(%this)
{
      echo("\n--------- Initializing " @ $appName @ ": Server Scripts ---------");
   //$Server::BotCount = 0;	// PZ
   $Server::GameType = $appName;
   //$Server::TestCheats = 0; // PZ Dev purposes for testing and editing

   // Server::Status is returned in the Game Info Query and represents the
   // current status of the server. This string sould be very short.
   $Server::Status = "Unknown";

   // Turn on testing/debug script functions
   $Server::TestCheats = false;

   // Specify where the mission files are.
   $Server::MissionFileSpec = "levels/*.mis";

   // Base server functionality
   exec("./audio.cs");
   exec("./message.cs");
   exec("./commands.cs");
   exec("./levelInfo.cs");
   exec("./missionLoad.cs");
   exec("./missionDownload.cs");
   exec("./clientConnection.cs");
   //exec("./admin.cs"); // PZ Code
   exec("./kickban.cs");
   //exec("./gametypes/coregame.cs");
   exec("./spawn.cs");
   exec("./camera.cs");
   exec("./centerPrint.cs");
   //exec("./library.cs");
   
   //initServer();
}




//-----------------------------------------------------------------------------

function Torque::initDedicated(%this)
{
   echo("\n--------- Starting Dedicated Server ---------");

   echo("$levelToLoad:" SPC $levelToLoad SPC "$missionTypeArg" SPC $missionTypeArg);

   // Make sure this variable reflects the correct state.
   $Server::Dedicated = true;

   // The server isn't started unless a mission and type has been specified.
   if ( $levelToLoad $= "" || $missionTypeArg $= "" )
   {
      error( "No level or game type specified. Creation of server failed." );
      return false;
   }

   %level = "";
   for( %file = findFirstFile( $Server::MissionFileSpec ); %file !$= ""; %file = findNextFile( $Server::MissionFileSpec ) )
   {
      if ( fileName( %file ) $= $levelToLoad )
      {
         %level = %file;
         break;
      }
   }

   if ( !isFile( %level ) )
      return false;

   tge.createServer( "MultiPlayer", %level, $missionTypeArg );
}

/// Attempt to find an open port to initialize the server with
function portInit(%port)
{
   %failCount = 0;
   while(%failCount < 10 && !setNetPort(%port))
   {
      echo("Port init failed on port " @ %port @ " trying next port.");
      %port++; %failCount++;
   }
}

/// Create a server of the given type, load the given level, and then
/// create a local client connection to the server.
//
/// @return true if successful.
function createAndConnectToLocalServer( %serverType, %level, %missionType )
{
   if( !tge.createServer( %serverType, %level, %missionType ) )
      return false;
   
   %conn = new GameConnection( ServerConnection );
   RootGroup.add( ServerConnection );

   %conn.setConnectArgs( $pref::Player::Name );
   %conn.setJoinPassword( $Client::Password );
   
   %result = %conn.connectLocal();
   if( %result !$= "" )
   {
      %conn.delete();
      tge.destroyServer();
      
      return false;
   }

   return true;
}

/// Create a server with either a "SinglePlayer" or "MultiPlayer" type
/// Specify the level to load on the server
function Torque::createServer(%this, %serverType, %level, %missionType)
{
   // Increase the server session number.  This is used to make sure we're
   // working with the server session we think we are.
   $Server::Session++;
   
   if (%level $= "")
   {
      error("createServer(): level name unspecified");
      return false;
   }
   
   // Make sure our level name is relative so that it can send
   // across the network correctly
   %level = makeRelativePath(%level, getWorkingDirectory());

   tge.destroyServer();

   $missionSequence = 0;
   $Server::PlayerCount = 0;
   $Server::ServerType = %serverType;
   $Server::MissionType = %missionType;
   $Server::LoadFailMsg = "";
   $Physics::isSinglePlayer = true;
   
   // Setup for multi-player, the network must have been
   // initialized before now.
   if (%serverType $= "MultiPlayer")
   {
      $Physics::isSinglePlayer = false;
            
      echo("Starting multiplayer mode");

      // Make sure the network port is set to the correct pref.
      portInit($Pref::Server::Port);
      allowConnections(true);

      if ($pref::Net::DisplayOnMaster !$= "Never" )
         schedule(0,0,startHeartbeat);
   }

   // Create the ServerGroup that will persist for the lifetime of the server.
   $ServerGroup = new SimGroup(ServerGroup);

   // Load up any core datablocks
   exec("art/datablocks/datablockExec.cs");

   // Let the game initialize some things now that the
   // the server has been created
   // Start onServerCreated();
      // Server::GameType is sent to the master server.
   // This variable should uniquely identify your game and/or mod.
   $Server::GameType = $appName;

   // Server::MissionType sent to the master server.  Clients can
   // filter servers based on mission type.
   $Server::MissionType = "Deathmatch";

   // GameStartTime is the sim time the game started. Used to calculated
   // game elapsed time.
   $Game::StartTime = 0;

   // Create the server physics world.
   physicsInitWorld( "server" );

   // Load up any objects or datablocks saved to the editor managed scripts
   %datablockFiles = new ArrayObject();
   %datablockFiles.add( "art/ribbons/ribbonExec.cs" );   
   %datablockFiles.add( "art/particles/managedParticleData.cs" );
   %datablockFiles.add( "art/particles/managedParticleEmitterData.cs" );
   %datablockFiles.add( "art/decals/managedDecalData.cs" );
   %datablockFiles.add( "art/datablocks/managedDatablocks.cs" );
   %datablockFiles.add( "art/forest/managedItemData.cs" );
   %datablockFiles.add( "art/datablocks/datablockExec.cs" );   
   loadDatablockFiles( %datablockFiles, true );

   // Run the other gameplay scripts in this folder
   exec("./scriptExec.cs");

   // Keep track of when the game started
   $Game::StartTime = $Sim::Time;
   // end onservercreated   
   
   // Game files
   exec("./gametypes/CoreGame.cs"); // Parent to all, want this loaded first

   %search = "./gametypes/*Game.cs";
   for(%file = findFirstFile(%search); %file !$= ""; %file = findNextFile(%search))
   {
     %type = fileBase(%file);
     if(%type !$= CoreGame)
        exec("./gametypes/" @ %type @ ".cs");
   }

   // Mission scripting support. Auto-execute files - Temporary until I figure out how to just exe the mapscript in the proper dir itself
   %path = "levels/*.cs";
   for( %file = findFirstFile( %path ); %file !$= ""; %file = findNextFile( %path ) )
   {
       if( fileBase(%file) $= fileBase(%level) )
          exec( %file );
   }

   // Load the level
   %this.loadMission(%level, %missionType, true);
   
   return true;
}

/// Shut down the server
function Torque::destroyServer(%this)
{
   $Server::ServerType = "";
   allowConnections(false);
   stopHeartbeat();
   $missionRunning = false;
   $Game::Running = false;
   
   // End any running levels
   endMission();
      // This function is called as part of a server shutdown.

   physicsDestroyWorld( "server" );

   // Clean up the GameCore package here as it persists over the
   // life of the server.
   if (isPackage(GameCore))
   {
      deactivatePackage(GameCore);
   }

   // Delete all the server objects
   if (isObject(ServerGroup))
      ServerGroup.delete();

   // Delete all the connections:
   while (ClientGroup.getCount())
   {
      %client = ClientGroup.getObject(0);
      %client.delete();
   }

   $Server::GuidList = "";

   // Delete all the data blocks...
   deleteDataBlocks();
   
   // Save any server settings
   echo( "Exporting server prefs..." );
   export("$Pref::Server::*", GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/server.config.cs", False);

   // Increase the server session number.  This is used to make sure we're
   // working with the server session we think we are.
   $Server::Session++;
}

/// Reset the server's default prefs
function resetServerDefaults()
{
   echo( "Resetting server defaults..." );
   
   exec( "./defaults.cs" );
   exec( GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/server.config.cs" );

   allowConnections(true); // ZOD: Open up the server for connections again.  
   
   // Reload the current level
   tge.loadMission( $pref::Server::MissionFile, $pref::Server::MissionType, false );
   $resettingServer = false;
   echo( "Server reset complete." );
}

/// Guid list maintenance functions
function addToServerGuidList( %guid )
{
   %count = getFieldCount( $Server::GuidList );
   for ( %i = 0; %i < %count; %i++ )
   {
      if ( getField( $Server::GuidList, %i ) == %guid )
         return;
   }

   $Server::GuidList = $Server::GuidList $= "" ? %guid : $Server::GuidList TAB %guid;
}

function removeFromServerGuidList( %guid )
{
   %count = getFieldCount( $Server::GuidList );
   for ( %i = 0; %i < %count; %i++ )
   {
      if ( getField( $Server::GuidList, %i ) == %guid )
      {
         $Server::GuidList = removeField( $Server::GuidList, %i );
         return;
      }
   }
}

/// When the server is queried for information, the value of this function is
/// returned as the status field of the query packet.  This information is
/// accessible as the ServerInfo::State variable.
function onServerInfoQuery()
{
   return "Doing Ok";
}

//-----------------------------------------------------------------------------



//-----------------------------------------------------------------------------

function initDedicated()
{
   enableWinConsole(true);
   echo("\n--------- Starting Dedicated Server ---------");

   // Make sure this variable reflects the correct state.
   $Server::Dedicated = true;

   // The server isn't started unless a mission has been specified.
   if ($missionArg !$= "") {
      createServer("MultiPlayer", $missionArg);
   }
   else
      echo("No mission specified (use -mission filename)");
}

