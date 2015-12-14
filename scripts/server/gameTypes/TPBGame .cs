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

// ----------------------------------------------------------------------------
// Team Painball
// ----------------------------------------------------------------------------
// Depends on methods found in CoreGame.cs.  Those added here are specific to
// this game type and/or over-ride the "default" game functionaliy.
//
// The desired Game Type must be added to each mission's LevelInfo object.
//   - gameType = "Deathmatch";
// If this information is missing then the CoreGame will default to Deathmatch.
// ----------------------------------------------------------------------------

function TPBGame::onMissionLoaded(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TPBGame::onMissionLoaded");

   $Server::MissionType = "Team Paintball";
   parent::onMissionLoaded(%game);
}

function TPBGame::initGameVars(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TPBGame::initGameVars");

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
   $Game::defaultPlayerDataBlock = "PaintballPlayerData";
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

function TPBGame::startGame(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TPBGame::startGame");

   parent::startGame(%game);
}

function TPBGame::endGame(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TPBGame::endGame");

   parent::endGame(%game);
}

function TPBGame::endMission(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TPBGame::endMission");

   parent::endGame(%game);
}

function TPBGame::onGameDurationEnd(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TPBGame::onGameDurationEnd");

   parent::onGameDurationEnd(%game);
}

function TPBGame::onClientEnterGame(%game, %client)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TPBGame::onClientEnterGame");

   parent::onClientEnterGame(%game, %client);
}

function TPBGame::onClientLeaveGame(%game, %client)
{
   echo (%game @"\c4 -> "@ %game.class @" -> TPBGame::onClientLeaveGame");

   parent::onClientLeaveGame(%game, %client);

}
