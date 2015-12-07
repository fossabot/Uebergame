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

// Global movement speed that affects all cameras.  This should be moved
// into the camera datablock.
$Camera::movementSpeed = 30;

function Observer::onTrigger(%this,%obj,%trigger,%state)
{
   // state = 0 means that a trigger key was released
   if (%state == 0)
      return;

   // Default player triggers: 0=fire 1=altFire 2=jump
   %client = %obj.getControllingClient();
   switch$ (%obj.mode)
   {
      case "Observer":
         // Do something interesting.

      case "Corpse":
         // Viewing dead corpse, so we probably want to respawn.
         %client.spawnPlayer();

         // Set the camera back into observer mode, since in
         // debug mode we like to switch to it.
         %this.setMode(%obj,"Observer");
   }
}

function Observer::setMode(%this,%obj,%mode,%arg1,%arg2,%arg3)
{
   switch$ (%mode)
   {
      case "Observer":
         // Let the player fly around
         %obj.setFlyMode();

      case "Corpse":
         // Lock the camera down in orbit around the corpse,
         // which should be arg1
         %transform = %arg1.getTransform();
         %obj.setOrbitMode(%arg1, %transform, 0.5, 4.5, 4.5);
         %obj.orbitObj = %arg1;

		 //lock respawn, unlock after x seconds
         %obj.canResapwn = false;
         %obj.schedule(1000,"unlockRespawn");
   }
   %obj.mode = %mode;
}

//----------------------------------------------------------------------------
// Camera commands
//----------------------------------------------------------------------------
function serverCmdTogglePathCamera(%client, %val)
{
   if(%val)
   {
      %control = %client.PathCamera;
   }
   else
   {
      %control = %client.camera;
   }
   %client.setControlObject(%control);
   clientCmdSyncEditorGui();
}
function serverCmdToggleCamera(%client)
{
   if (%client.getControlObject() == %client.player)
   {
      %client.camera.setVelocity("0 0 0");
      %control = %client.camera;
   }
   else
   {
      %client.player.setVelocity("0 0 0");
      %control = %client.player;
   }
   %client.setControlObject(%control);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorCameraPlayer(%client)
{
   // Switch to Player Mode
   %client.player.setVelocity("0 0 0");
   %client.setControlObject(%client.player);
   ServerConnection.setFirstPerson(1);
   $isFirstPersonVar = 1;

   clientCmdSyncEditorGui();
}

function serverCmdSetEditorCameraPlayerThird(%client)
{
   // Swith to Player Mode
   %client.player.setVelocity("0 0 0");
   %client.setControlObject(%client.player);
   ServerConnection.setFirstPerson(0);
   $isFirstPersonVar = 0;

   clientCmdSyncEditorGui();
}

function serverCmdDropPlayerAtCamera(%client)
{
   // If the player is mounted to something (like a vehicle) drop that at the
   // camera instead. The player will remain mounted.
   %obj = %client.player.getObjectMount();
   if (!isObject(%obj))
      %obj = %client.player;

   %obj.setTransform(%client.camera.getTransform());
   %obj.setVelocity("0 0 0");

   %client.setControlObject(%client.player);
   clientCmdSyncEditorGui();
}

function serverCmdDropCameraAtPlayer(%client)
{
   %client.camera.setTransform(%client.player.getEyeTransform());
   %client.camera.setVelocity("0 0 0");
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdCycleCameraFlyType(%client)
{
   if(%client.camera.getMode() $= "Fly")
	{
		if(%client.camera.newtonMode == false) // Fly Camera
		{
			// Switch to Newton Fly Mode without rotation damping
			%client.camera.newtonMode = "1";
			%client.camera.newtonRotation = "0";
			%client.camera.setVelocity("0 0 0");
		}
		else if(%client.camera.newtonRotation == false) // Newton Camera without rotation damping
		{
			// Switch to Newton Fly Mode with damped rotation
			%client.camera.newtonMode = "1";
			%client.camera.newtonRotation = "1";
			%client.camera.setAngularVelocity("0 0 0");
		}
		else // Newton Camera with rotation damping
		{
			// Switch to Fly Mode
			%client.camera.newtonMode = "0";
			%client.camera.newtonRotation = "0";
		}
		%client.setControlObject(%client.camera);
		clientCmdSyncEditorGui();
	}
}

function serverCmdSetEditorCameraStandard(%client)
{
   // Switch to Fly Mode
   %client.camera.setFlyMode();
   %client.camera.newtonMode = "0";
   %client.camera.newtonRotation = "0";
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorCameraNewton(%client)
{
   // Switch to Newton Fly Mode without rotation damping
   %client.camera.setFlyMode();
   %client.camera.newtonMode = "1";
   %client.camera.newtonRotation = "0";
   %client.camera.setVelocity("0 0 0");
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorCameraNewtonDamped(%client)
{
   // Switch to Newton Fly Mode with damped rotation
   %client.camera.setFlyMode();
   %client.camera.newtonMode = "1";
   %client.camera.newtonRotation = "1";
   %client.camera.setAngularVelocity("0 0 0");
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorOrbitCamera(%client)
{
   %client.camera.setEditOrbitMode();
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorFlyCamera(%client)
{
   %client.camera.setFlyMode();
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdEditorOrbitCameraSelectChange(%client, %size, %center)
{
   if(%size > 0)
   {
      %client.camera.setValidEditOrbitPoint(true);
      %client.camera.setEditOrbitPoint(%center);
   }
   else
   {
      %client.camera.setValidEditOrbitPoint(false);
   }
}

function serverCmdEditorCameraAutoFit(%client, %radius)
{
   %client.camera.autoFitRadius(%radius);
   %client.setControlObject(%client.camera);
  clientCmdSyncEditorGui();
}


//-----------------------------------------------------------------------------
// Camera methods
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------

function Camera::onAdd(%this,%obj)
{
   // Default start mode
   %this.setMode(%this.mode);
}

function Camera::setMode(%this,%mode,%arg1,%arg2,%arg3)
{
   // Punt this one over to our datablock
   %this.getDatablock().setMode(%this,%mode,%arg1,%arg2,%arg3);
}
