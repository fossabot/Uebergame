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
// Solo Paintball (Free for All)
// ----------------------------------------------------------------------------
// Depends on methods found in CoreGame.cs.  Those added here are specific to
// this game type and/or over-ride the "default" game functionaliy.
//
// The desired Game Type must be added to each mission's LevelInfo object.
//   - gameType = "Deathmatch";
// If this information is missing then the CoreGame will default to Deathmatch.
// ----------------------------------------------------------------------------

function SPBGame::onMissionLoaded(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> SPBGame::onMissionLoaded");

   $Server::MissionType = "Solo Paintball";
   parent::onMissionLoaded(%game);
}

function SPBGame::initGameVars(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> SPBGame::initGameVars");

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

function SPBGame::loadOut(%game, %player)
{
   echo (%game @"\c4 -> "@ %game.class @" -> SPBGame::loadOut");

   %player.clearWeaponCycle();
   
   %player.setInventory(Ryder, 1);
   %player.setInventory(RyderClip, 2 ); //%player.maxInventory(RyderClip)
   %player.setInventory(RyderAmmo, %player.maxInventory(RyderAmmo));    // Start the gun loaded
   %player.addToWeaponCycle(Ryder);
   
   %player.setInventory(PaintballMarkerBlue, 1);
   %player.setInventory(PaintballMarkerRed, 1);
   %player.setInventory(PaintballMarkerGreen, 1);
   %player.setInventory(PaintballMarkerYellow, 1);
   %player.setInventory(PaintballClip, 10 );
   %player.setInventory(PaintballAmmo, %player.maxInventory(PaintballAmmo));
   %player.addToWeaponCycle(PaintballMarkerBlue);
   %player.addToWeaponCycle(PaintballMarkerRed);
   %player.addToWeaponCycle(PaintballMarkerGreen);
   %player.addToWeaponCycle(PaintballMarkerYellow);
   
   %player.mountImage(Ryder, 0);
}

function SPBGame::startGame(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> SPBGame::startGame");

   parent::startGame(%game);
}

function SPBGame::endGame(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> SPBGame::endGame");

   parent::endGame(%game);
}

function SPBGame::endMission(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> SPBGame::endMission");

   parent::endGame(%game);
}

function SPBGame::onGameDurationEnd(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> SPBGame::onGameDurationEnd");

   parent::onGameDurationEnd(%game);
}

function SPBGame::onClientEnterGame(%game, %client)
{
   echo (%game @"\c4 -> "@ %game.class @" -> SPBGame::onClientEnterGame");

   parent::onClientEnterGame(%game, %client);
}

function SPBGame::onClientLeaveGame(%game, %client)
{
   echo (%game @"\c4 -> "@ %game.class @" -> SPBGame::onClientLeaveGame");

   parent::onClientLeaveGame(%game, %client);

}
