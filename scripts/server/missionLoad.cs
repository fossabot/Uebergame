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
// Server mission loading
//-----------------------------------------------------------------------------

// On every mission load except the first, there is a pause after
// the initial mission info is downloaded to the client.
$MissionLoadPause = 5000;

//-----------------------------------------------------------------------------

function loadMission( %missionFile, %missionType, %isFirstMission ) 
{
   // Do not allow clients to connect during loading process
   //allowConnections(false);

   // cleanup
   if(!%isFirstMission)
      endMission();
   echo("<>>>> LOADING MISSION: " @ %missionFile @ " <<<<>");
   echo("<>>>> Stage 1 load <<<<>");

   $LoadingMission = true;
   // Reset all of these
   if (isFunction("clearCenterPrintAll"))
      clearCenterPrintAll();
   if (isFunction("clearBottomPrintAll"))
      clearBottomPrintAll();

   // increment the mission sequence (used for ghost sequencing)
   $missionSequence++;
   $missionRunning = false;
   // Setup some vars needed for server filter
   $Server::MissionType = %missionType;
   $Server::MissionName = fileBase(%missionFile);
   $Server::MissionFile = %missionFile;
   $Server::LoadFailMsg = "";

   // Create a list of inventory items banned by the game type.
   // SmsInv.CreateInvBanCount(); // BKS #TODO
   // Extract mission info from the mission file,
   // including the display name and stuff to send
   // to the client.
   buildLoadInfo(%missionFile, %missionType);

   // Download mission info to the clients
   %count = ClientGroup.getCount();
   for( %cl = 0; %cl < %count; %cl++ ) {
      %client = ClientGroup.getObject( %cl );
      if (!%client.isAIControlled())
         sendLoadInfoToClient(%client);
   }

// Mission scripting support, activate mission package
   // the level designer may have included
   if ( isPackage( $Server::MissionName ) )
   {
      if(!isActivePackage($Server::MissionName))
         activatePackage($Server::MissionName);

      eval($Server::MissionName @ "::preLoad(%first);");
   }
   // Now that we've sent the LevelInfo to the clients
   // clear it so that it won't conflict with the actual
   // LevelInfo loaded in the level
   clearLoadInfo();

   // if this isn't the first mission, allow some time for the server
   // to transmit information to the clients:
   if ( %isFirstMission || !$pref::Server::Multiplayer )
      loadMissionStage2(%missionFile, %missionType, %isFirstMission); //loadMissionStage2();
   else
      schedule( $MissionLoadPause, "loadMissionStage2", %missionFile, %missionType, %isFirstMission );	//schedule( $MissionLoadPause, ServerGroup, loadMissionStage2 );
}

//-----------------------------------------------------------------------------

function loadMissionStage2(%missionFile, %missionType, %isFirstMission) 
{
   echo("<>>>> Stage 2 load <<<<>");

   // Create the mission group off the ServerGroup
   $instantGroup = ServerGroup;

	if ( %missionType $= "" )
   {
      new ScriptObject(Game) {
         class = CoreGame;
      };
   }
   else
   {
      new ScriptObject(Game) {
         class = %missionType @ "Game";
         superClass = CoreGame;
      };
   }
   
   if ( isPackage( %missionType @ "Game" ) )
      Game.activatePackages();
   // Make sure the mission exists
   if ( !isFile( %missionFile ) )
   {
      $Server::LoadFailMsg = "Could not find mission \"" @ %missionFile @ "\"";
      error($Server::LoadFailMsg);

      // Inform clients that are already connected
      for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
         messageClient(ClientGroup.getObject(%clientIndex), 'MsgLoadFailed', $Server::LoadFailMsg);

      Game.schedule(3000, "cycleMissions");
      return( false );
   }

      // Calculate the mission CRC.  The CRC is used by the clients
      // to caching mission lighting.
   $missionCRC = getFileCRC( %missionFile );

   // Mission cleanup group.  This is where run time components will reside.  The MissionCleanup
   // group will be added to the ServerGroup.
   new SimGroup(MissionCleanup);

      // Exec the mission.  The MissionGroup (loaded components) is added to the ServerGroup
   exec(%missionFile);

      if( !isObject(MissionGroup) )
      {
      $Server::LoadFailMsg = "No 'MissionGroup' found in mission \"" @ %missionFile @ "\".";

      error($Server::LoadFailMsg);
      // Inform clients that are already connected
      for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
         messageClient(ClientGroup.getObject(%clientIndex), 'MsgLoadFailed', $Server::LoadFailMsg);    
      Game.schedule(3000, "cycleMissions");
      return( false );
   }

   // Set mission name.
   
   if( isObject( theLevelInfo ) )
      $Server::MissionName = theLevelInfo.levelName;




   // Make the MissionCleanup group the place where all new objects will automatically be added.
   $instantGroup = MissionCleanup;
   
   // Construct MOD paths
   pathOnMissionLoadDone();

   // Mission loading done...
   echo("*** Mission loaded");
   
   // Start all the clients in the mission
   $missionRunning = true;
   for( %clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++ )
   {
      ClientGroup.getObject(%clientIndex).loadMission();
   }

   // Go ahead and launch the game
   onMissionLoaded();

   // Mission loading done...
   echo("<>>>> Mission loaded <<<<>");
   $missionRunning = true;

   // Mission scripting support, activate mission package
   // the level designer has included
   if ( isActivePackage( $Server::MissionName ) )
      eval($Server::MissionName @ "::mapLoaded();");

}


//-----------------------------------------------------------------------------

function endMission()
{
   if (!isObject( MissionGroup ))
      return;

   echo("<>>>> ENDING MISSION <<<<>");
   
   // Mission scripting support, kill the mission package
   // the level designer has included
   if ( isActivePackage( $Server::MissionName ) )
   {
      eval($Server::MissionName @ "::DeactivateMap();");
      deactivatePackage($Server::MissionName);
   }
   
   onMissionEnded();

   // Inform the clients
   for( %clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++ ) {
      // clear ghosts and paths from all clients
      %cl = ClientGroup.getObject( %clientIndex );
      %cl.endMission();
      %cl.resetGhosting();
      %cl.clearPaths();
   }
   
   // Delete everything
   MissionGroup.delete();
   MissionCleanup.delete();
   Game.deactivatePackages();
   Game.delete();
 
   $ServerGroup.delete();
   $ServerGroup = new SimGroup(ServerGroup);
   clearServerPaths();
}


//-----------------------------------------------------------------------------

function resetMission()
{
   echo("*** MISSION RESET");

   // Remove any temporary mission objects
   MissionCleanup.delete();
   $instantGroup = ServerGroup;
   new SimGroup( MissionCleanup );
   $instantGroup = MissionCleanup;

   clearServerPaths();
   //
   onMissionReset();
}

//-----------------------------------------------------------------------------
// 	BKS #Projectz 
//	added various gametype functions, havent trace which are used vs unused as of yet
//	The number of used functions will undoubtedly increase if/when other project z 
// 	features are added from the gametypes folders/files

function SimGroup::setupGameType(%this, %type)
{
   for (%i = 0; %i < %this.getCount(); %i++)
      %this.getObject(%i).setupGameType(%type);
}

function SimObject::setupGameType(%this, %type)
{
   // Thou shalt not spam
}

function ShapeBase::setupGameType(%this, %type)
{
   //error("ShapeBase::setupGameType(" SPC %this.getClassName() SPC %type SPC ")");
   if(%this.gameTypesList $= "")
      return;

   for(%i = 0; (%allow = getWord(%this.gameTypesList, %i)) !$= ""; %i++)
      if(%allow $= %type)
         return;

   %this.setHidden(true);
}

function TSStatic::setupGameType(%this, %type)
{
   if(%this.gameTypesList $= "")
      return;

   for(%i = 0; (%allow = getWord(%this.gameTypesList, %i)) !$= ""; %i++)
      if(%allow $= %type)
         return;

   %this.schedule(0, "delete");
}

function Trigger::setupGameType(%this, %type)
{
   if(%this.gameTypesList $= "")
      return;

   for(%i = 0; (%allow = getWord(%this.gameTypesList, %i)) !$= ""; %i++)
      if(%allow $= %type)
         return;

   %this.schedule(0, "delete");
}

function ParticleEmitterNode::setupGameType(%this, %type)
{
   if(%this.gameTypesList $= "")
      return;

   for(%i = 0; (%allow = getWord(%this.gameTypesList, %i)) !$= ""; %i++)
      if(%allow $= %type)
         return;

   %this.schedule(0, "delete");
}

function SimGroup::initializeObjective(%this)
{
   for (%i = 0; %i < %this.getCount(); %i++)
      %this.getObject(%i).initializeObjective();   
}

function GameBase::initializeObjective(%this)
{
   if( isObject( %this ) )
      %this.getDataBlock().initializeObjective(%this);
}

function SimGroup::activatePhysicalZones(%this)
{
   for (%i = 0; %i < %this.getCount(); %i++)
   {
      %obj = %this.getObject(%i);
      if ( %obj.getClassName() $= SimGroup )
         %obj.activatePhysicalZones();
      else
      {
         if ( %obj.getClassName() $= PhysicalZone )
            %obj.activate();
      }
   }
}

function SimGroup::deactivatePhysicalZones(%this)
{
   for (%i = 0; %i < %this.getCount(); %i++)
   {
      %obj = %this.getObject(%i);
      if ( %obj.getClassName() $= SimGroup )
         %obj.activatePhysicalZones();
      else
      {
         if ( %obj.getClassName() $= PhysicalZone )
            %obj.deactivate();
      }
   }
}
