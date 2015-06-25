// This package is activated when a MainMenuLevel
// type level is loaded.
package MainMenuLevelGame
{
   function GameConnection::initialControlSet(%this)
   {
      echo ("*** MainMenuLevelGame Initial Control Object");
      // The first control object has been set by the 
      //server and we are now ready to go.
      // first check if the editor is active
      if (!isToolBuild() || !Editor::checkActiveLoadDone())
      {
         if (Canvas.getContent() != MainMenuGui.getId())
         {
            // Display the main menu GUI with this level
            Canvas.setContent(MainMenuGui);
         }
      }
   }
};
function MainMenuLevelGame::onMissionLoaded(%game)
{
   $Server::MissionType = "MainMenuLevel";
   parent::onMissionLoaded(%game);
}
// This method sets up whatever properties are required
// for the level.
function MainMenuLevelGame::initGameVars(%game)
{
   // What kind of "camera" is spawned is either controlled
   // directly by the SpawnSphere or it defaults back to 
   //the values set here. This also controls which 
   //SimGroups to attempt to select the spawn sphere's from 
   //by walking down the list of SpawnGroups till it finds 
   //a valid spawn object. These override the values set in
   // core/scripts/server/spawn.cs
   $Game::defaultCameraClass = "Camera";
   $Game::defaultCameraDataBlock = "Observer";
      $Game::defaultCameraSpawnGroups = 
      "CameraSpawnPoints PlayerSpawnPoints PlayerDropPoints";
   // Set the gameplay parameters
   %game.duration = 0;
   %game.endgameScore = 0;
   %game.endgamePause = 0;
   %game.allowCycling = false;
}
// This method is called when the client connects to
// the level.  In our case, this is the only client that
// will connect with the main menu level, and will be
// in a single player mode (also known as a local client).
function MainMenuLevelGame::onClientEnterGame(%game, %client)
{
   // Spawn a camera for the local client
   %cameraSpawnPoint =
      pickCameraSpawnPoint($Game::DefaultCameraSpawnGroups);
   %client.spawnCamera(%cameraSpawnPoint);
}
// This method is called when our local client is removed
// from the level, either because the main menu GUI is no
// longer the Canvas' content, or the program has been 
//exited.
function MainMenuLevelGame::onClientLeaveGame(%game, %client)
{
   // Cleanup the camera
   if (isObject(%client.camera))
      %client.camera.delete();
}