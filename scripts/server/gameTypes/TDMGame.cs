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

// DisplayName = Master Gametype

//--- GAME RULES BEGIN ---
$HostGameRules["TDM", 0] = "Kill the enemy and don't get killed.";
$HostGameRules["TDM", 1] = "Team with the most kills wins!";
//--- GAME RULES END ---

//-----------------------------------------------------------------------------
// <> EXECUTION ORDER <>
//
// CoreGame::activatePackages
// CoreGame::onMissionLoaded
// CoreGame::setupGameParams
// CoreGame::checkMatchStart
// CoreGame::Countdown
// CoreGame::setClientState
// CoreGame::onClientEnterGame
// CoreGame::sendClientTeamList
// CoreGame::spawnPlayer
// CoreGame::pickSpawnPoint
// CoreGame::createPlayer
// CoreGame::loadOut
// CoreGame::startGame
//
// CoreGame::EndCountdown
// CoreGame::onGameDurationEnd
// CoreGame::cycleGame
// CoreGame::endGame
// CoreGame::onCyclePauseEnd
// CoreGame::deactivatePackages
//
//-----------------------------------------------------------------------------

// CoreGame
// ----------------------------------------------------------------------------
// Depends on methods found in CoreGame.cs.  Those added here are specific to
// this game type and/or over-ride the "default" game functionaliy.
//
// The desired Game Type must be added to each mission's LevelInfo object.
//   - gameType = "Deathmatch";
// If this information is missing then the CoreGame will default to Deathmatch.
// ----------------------------------------------------------------------------

function TDMGame::activatePackages(%game)
{
   LogEcho("TDMGame::activatePackages(" SPC %game.class SPC ")");
   if ( isPackage( %game.class ) && %game.class !$= CoreGame )
      activatePackage( %game.class );
}
function TDMGame::deactivatePackages(%game)
{
   LogEcho("TDMGame::deactivatePackages(" SPC %game.class SPC ")");
   if ( isActivePackage( %game.class ) && %game.class !$= CoreGame )
      deactivatePackage( %game.class );
}
function TDMGame::onMissionLoaded(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TDMGame::onMissionLoaded");

   $Server::MissionType = "DeathMatch";
   parent::onMissionLoaded(%game);
}

function TDMGame::initGameVars(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TDMGame::initGameVars");

   //-----------------------------------------------------------------------------
   // What kind of "player" is spawned is either controlled directly by the
   // SpawnSphere or it defaults back to the values set here. This also controls
   // which SimGroups to attempt to select the spawn sphere's from by walking down
   // the list of SpawnGroups till it finds a valid spawn object.
   // These override the values set in scripts/server/spawn.cs
   //-----------------------------------------------------------------------------
   
   // Leave $Game::defaultPlayerClass and $Game::defaultPlayerDataBlock as empty strings ("")
   // to spawn a the $Game::defaultCameraClass as the control object.
   $Game::defaultPlayerClass = "Player";
   $Game::defaultPlayerDataBlock = "TorqueSoldierPlayerData";
   $Game::defaultPlayerSpawnGroups = "PlayerSpawnPoints PlayerDropPoints";

   //-----------------------------------------------------------------------------
   // What kind of "camera" is spawned is either controlled directly by the
   // SpawnSphere or it defaults back to the values set here. This also controls
   // which SimGroups to attempt to select the spawn sphere's from by walking down
   // the list of SpawnGroups till it finds a valid spawn object.
   // These override the values set in scripts/server/spawn.cs
   //-----------------------------------------------------------------------------
   $Game::defaultCameraClass = "Camera";
   $Game::defaultCameraDataBlock = "Observer";
   $Game::defaultCameraSpawnGroups = "CameraSpawnPoints PlayerSpawnPoints PlayerDropPoints";

   // Set the gameplay parameters
   %game.duration = $pref::Game::Duration;
   %game.endgameScore = $pref::Game::EndGameScore;
   %game.endgamePause = $pref::Game::EndGamePause;
   %game.allowCycling = $pref::Game::AllowCycling;   // Is mission cycling allowed?
}

function TDMGame::startGame(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TDMGame::startGame");

   parent::startGame(%game);
}

function TDMGame::endGame(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TDMGame::endGame");

   parent::endGame(%game);
}

function TDMGame::endMission(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TDMGame::endMission");

   parent::endGame(%game);
}

function TDMGame::onGameDurationEnd(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TDMGame::onGameDurationEnd");

   parent::onGameDurationEnd(%game);
}

function TDMGame::onClientEnterGame(%game, %client)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TDMGame::onClientEnterGame");

   parent::onClientEnterGame(%game, %client);
}

function TDMGame::onClientLeaveGame(%game, %client)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TDMGame::onClientLeaveGame");

   parent::onClientLeaveGame(%game, %client);

}
