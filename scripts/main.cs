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

$WORD::RES_X = 0;
$WORD::RES_Y = 1;
$WORD::FULLSCREEN = 2;
$WORD::BITDEPTH = 3;
$WORD::REFRESH = 4;
$WORD::AA = 5;
//-----------------------------------------------------------------------------
// Package overrides to initialize the mod.
package scripts{
//---------------------------------------------------------------------------------------------
// onStart
// Called when the engine is starting up. Initializes this mod.
//---------------------------------------------------------------------------------------------

function Torque::onStart(%this)
{
   Parent::onStart(%this);

   exec("art/core/gui/profiles.cs");
   exec( "./client/defaults.cs" );
   exec( "./server/defaults.cs" );

   
   $ScriptGroup = new SimGroup(ScriptClassGroup);

   // Build the mission listing for server and client
   echo("----- Adding missions to list -----");
   %this.buildMissionList();
   
   // Initialise stuff.
   exec("scripts/client/core.cs");
   %this.initializeCore();

   echo(" % - Initialized Core");
   
   
   echo("\n--------- Initializing Directory: scripts ---------");

   // Load the scripts that start it all...
   exec("./client/init.cs");
   exec("./server/server.cs");
   
   // Init the physics plugin.
   physicsInit();
      
   // Start up the audio system.
   sfxStartup();

   //exec("scripts/server/coreserver.cs");
   
   exec("./gui/guiTreeViewCtrl.cs");
   exec("./gui/messageBoxes/messageBox.ed.cs");
   
   // Server gets loaded for all sessions, since clients
   // can host in-game servers.
   %this.initServer();

   // Start up in either client, or dedicated server mode
   if ( $isDedicated || $pref::Server::Dedicated )
   {
      tge.initDedicated();
   }
   else
      tge.initClient();
}

function onExit()
{
   sfxShutdown();
   // Ensure that we are disconnected and/or the server is destroyed.
   // This prevents crashes due to the SceneGraph being deleted before
   // the objects it contains.
   if ($Server::Dedicated)
      tge.destroyServer();
   else
      disconnect();
   
   // Destroy the physics plugin.
   physicsDestroy();
      
   echo("Exporting client prefs");
   export("$pref::*", GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/client.config.cs", False);

   echo("Exporting server prefs");
   export("$Pref::Server::*", GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/server.config.cs", False);
   BanList::Export(GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/banlist.cs");
	
	   // Shutdown stuff.
   shutdownCore();
   Parent::onExit();
}


function Torque::displayHelp(%this)
{
   // Let the parent do its stuff.
   Parent::displayHelp();

   error("Core Mod options:\n" @
         "  -fullscreen            Starts game in full screen mode\n" @
         "  -windowed              Starts game in windowed mode\n" @
         "  -autoVideo             Auto detect video, but prefers OpenGL\n" @
         "  -openGL                Force OpenGL acceleration\n" @
         "  -directX               Force DirectX acceleration\n" @
         "  -voodoo2               Force Voodoo2 acceleration\n" @
         "  -prefs <configFile>    Exec the config file\n" @
         "  -dedicated             Start as dedicated server\n"@
         "  -connect <address>     For non-dedicated: Connect to a game at <address>\n" @
         "  -level <filename>      For dedicated: Load the mission\n" @
         "  -type <game_type>      For dedicated: Use the specified gametype\n"
   );
}

function Torque::parseArgs(%this)
{
   // Call the parent
   Parent::parseArgs(%this);

   // Arguments, which override everything else.
   for (%i = 1; %i < $Game::argc ; %i++)
   {
      %arg = $Game::argv[%i];
      %nextArg = $Game::argv[%i+1];
      %hasNextArg = $Game::argc - %i > 1;
   
      switch$ (%arg)
      {
         case "-fullscreen":
            setFullScreen(true);
            $argUsed[%i]++;

         case "-windowed":
            setFullScreen(false);
            $argUsed[%i]++;

         case "-openGL":
            $pref::Video::displayDevice = "OpenGL";
            $argUsed[%i]++;

         case "-directX":
            $pref::Video::displayDevice = "D3D";
            $argUsed[%i]++;

         case "-voodoo2":
            $pref::Video::displayDevice = "Voodoo2";
            $argUsed[%i]++;

         case "-autoVideo":
            $pref::Video::displayDevice = "";
            $argUsed[%i]++;

         case "-prefs":
            $argUsed[%i]++;
            if (%hasNextArg) {
               exec(%nextArg, true, true);
               $argUsed[%i+1]++;
               %i++;
            }
            else
               error("Error: Missing Command Line argument. Usage: -prefs <path/script.cs>");
		 //--------------------
         case "-dedicated":
            $Server::Dedicated = true;
            enableWinConsole(true);
            $argUsed[%i]++;

         //--------------------
         case "-mission":
            $argUsed[%i]++;
            if (%hasNextArg) {
               $missionArg = %nextArg;
               $argUsed[%i+1]++;
               %i++;
            }
            else
               error("Error: Missing Command Line argument. Usage: -mission <filename>");

         //--------------------
         case "-connect":
            $argUsed[%i]++;
            if (%hasNextArg) {
               $JoinGameAddress = %nextArg;
               $argUsed[%i+1]++;
               %i++;
            }
            else
               error("Error: Missing Command Line argument. Usage: -connect <ip_address>");
      }
   }
}


}; // package scripts

// Activate the game package.
activatePackage(scripts);

function LogEcho(%string)
{
   if( $pref::LogEchoEnabled )
   {
/*
      %file = $pref::Server::LogPath @"/"@ "echos.txt";
      %log = new FileObject();
      %log.openForAppend(%file);
      %log.writeLine("\"" @ %string );
      %log.close();
      %log.delete();
*/
      echo(%string);
   }
}

//-----------------------------------------------------------------------------
// Mission List Creation
//-----------------------------------------------------------------------------

// This can be easily overloaded for more types.
function Torque::getMissionTypeDisplayNames(%this)
{
   for ( %type = 0; %type < $HostTypeCount; %type++ )
   {
      if       ( $HostTypeName[%type]  = "SDM" )
         $HostTypeDisplayName[%type]   = "Deathmatch";
      else if  ( $HostTypeName[%type]  = "TDM" )
         $HostTypeDisplayName[%type]   = "Team Deathmatch";
      else if  ( $HostTypeName[%type]  = "SPB" )
         $HostTypeDisplayName[%type]   = "Solo Paintball";
      else if  ( $HostTypeName[%type]  = "TPB" )
         $HostTypeDisplayName[%type]   = "Team Paintball";
      else
         $HostTypeDisplayName[%type] = $HostTypeName[%type];
   }
}

function Torque::buildMissionList(%this)
{
   %search = "levels/*.mis";
   $HostTypeCount = 0;
   $HostMissionCount = 0;

   %i = 0;
   for ( %file = findFirstFile(%search); %file !$= ""; %file = findNextFile(%search) )
   {
      // Skip our new level/mission if we arent choosing a level
      // to launch in the editor.
      %name = fileBase(%file); // get the name

      // Do not add the new mission template into rotation.
      if ( !$startWorldEditor && %name $= NewLevel )
         continue;

      %this.addMissionFile( %file );
   }
}

function Torque::addMissionFile( %this, %file )
{
   
   %idx = $HostMissionCount;
   $HostMissionCount++;
   $HostMissionFile[%idx] = %file;
   $HostMissionName[%idx] = fileBase(%file);

   %LevelInfoObject = %this.getLevelInfo(%file);

   if ( %LevelInfoObject != 0 )
   {
      if( %LevelInfoObject.levelName !$= "" )
         $HostMissionName[%idx] = %LevelInfoObject.levelName;

      if ( %LevelInfoObject.MissionTypes !$= "" )
         %typeList = %LevelInfoObject.MissionTypes;

      for ( %word = 0; ( %misType = getWord( %typeList, %word ) ) !$= ""; %word++ )
      {
         for ( %i = 0; %i < $HostTypeCount; %i++ )
         {
            if ( $HostTypeName[%i] $= %misType )
               break;
         }

         if ( %i == $HostTypeCount )
         {
            $HostTypeCount++;
            $HostTypeName[%i] = %misType;
            $HostMissionCount[%i] = 0;
         }

         // add the mission to the type
         %ct = $HostMissionCount[%i];
         $HostMission[%i, $HostMissionCount[%i]] = %idx;
         $HostMissionCount[%i]++;
      }

      for( %i = 0; %LevelInfoObject.desc[%i] !$= ""; %i++ )
      {
         $HostMissionDesc[$HostMissionFile[%idx], %i] = %LevelInfoObject.desc[%i];
         //echo($HostMissionDesc[$HostMissionFile[%idx], %i]);
      }

      $MissionDisplayName[$HostMissionFile[%idx]] = $HostMissionName[%idx];

      %LevelInfoObject.delete();

      %this.getMissionTypeDisplayNames();

      echo($HostMissionName[%idx] SPC "added to mission list");
   }
}

// Read the level file and find the level object strings, then create the action object via eval so we
// can extract the params from it and load them into global vars
function Torque::getLevelInfo( %this, %missionFile ) 
{
   %file = new FileObject();
   
   %LevelInfoObject = "";
   
   if ( %file.openForRead( %missionFile ) )
   {
      %inInfoBlock = false;

      while ( !%file.isEOF() )
      {
         %line = %file.readLine();
         %line = trim( %line );
         if( %line $= "new ScriptObject(LevelInfo) {" )
            %inInfoBlock = true;
         else if( %line $= "new LevelInfo(theLevelInfo) {" )
            %inInfoBlock = true;
         else if( %inInfoBlock && %line $= "};" ) {
            %inInfoBlock = false;

            %LevelInfoObject = %LevelInfoObject @ %line;
            break;
         }

         if( %inInfoBlock )
            %LevelInfoObject = %LevelInfoObject @ %line @ " ";
      }

      %file.close();
   }
   %file.delete();

   if( %LevelInfoObject !$= "" )
   {
      %LevelInfoObject = "%LevelInfoObject = " @ %LevelInfoObject;
      eval( %LevelInfoObject );

      return %LevelInfoObject;
   }
	
   // Didn't find our LevelInfo
   return 0; 
}

function Torque::getMissionCount(%this, %type)
{
   // Find out how many mission files this gametype has associated with it
   for ( %count = 0; %count < $HostTypeCount; %count++ )
   {
      if ( $HostTypeName[%count] $= %type )
         break;
   }
   return $HostMissionCount[%count];
}
