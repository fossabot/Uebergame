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

datablock FlyingVehicleData(Talon : TalonDamageScale)
{
   category = "Vehicles";
   //className = Vehicle;
   shapeFile = "art/shapes/vehicles/drone/sad_drone.dae";
   cloakTexture = "art/textures/particles/cloakTexture.png";
   emap = true;

   numMountPoints = 1;
   mountPose[0] = sitting;
   isProtectedMountPoint[0] = true;

   Explosion = LargeExplosion;
   underwaterExplosion = LargeWaterExplosion;
   Debris = LargeExplosionDebris;
   renderWhenDestroyed = false;
   debrisShapeName = "art/shapes/objects/debris.dts";

   mass = 200;
   drag = 0.15;    //* from 0 - 1.0 was 0.25
   density = 6.0;

   maxEnergy = 280; // Afterburner and any energy weapon pool
   rechargeRate = 0.8;
   maxDamage = 201; // Must be higher then destroyed level
   disabledLevel = 190;
   destroyedLevel = 200;

   repairRate = 0.005;
   inheritEnergyFromMount = false;
   energyPerDamagePoint = 40.0;
   isInvincible = false;
   isShielded = true;

   // Camera
   cameraMaxDist = 12;
   cameraMinDist = 1;
   cameraDefaultFov = 90;
   cameraMinFov = 5;
   cameraMaxFov = 120;
   firstPersonOnly = false;
   useEyePoint = false;
   observeThroughObject = true;
   cameraRoll = true;               // Roll the camera with the vehicle
   cameraLag = 0.9;                 // Velocity lag of camera
   cameraDecay = 0;                 // Decay per sec. rate of velocity lag
   cameraOffset = 2.5;              // Vertical offset from camera mount point
   observeParameters = "1 10 10";

   //hud
   hudImageName = "";
   hudImageNameFriendly = 0; //per each array element
   hudImageNameEnemy = 0;    //per each array element
   hudRenderCenter = false;  //per each array element
   hudRenderModulated = false;//per each array element
   hudRenderAlways = false;  //per each array element
   hudRenderDistance = false;//per each array element
   hudRenderName = false;    //per each array element

   aiAvoidThis = true;
   computeCRC = true;
   hoverHeight = 2;       // Height off the ground at rest
   createHoverHeight = 5; // Height off the ground when created

   // Turbo Jet
   jetForce = 2000;       // Afterburner thrust (this is in addition to normal thrust)
   jetEnergyDrain = 2.8;  // Energy use of the afterburners (low number is less drain...can be fractional)
   minJetEnergy = 28;     // Afterburner can't be used if below this threshhold.
   massCenter = "0 0 0";  // Center of mass for rigid body
   massBox = "0 0 0";     // Size of box used for moment of inertia, if zero it defaults to object bounding box
   bodyRestitution = 0.8; // When you hit the ground, how much you rebound. (between 0 and 1)
   bodyFriction = 0;      // When this gets high it CAN cause probs, doesn't always

   // Maneuvering
   maneuveringForce = 3000;     // Horizontal jets (W,S,D,A key thrust)
   horizontalSurfaceForce = 8;//6  // Horizontal center "wing" (provides "bite" into the wind for climbing/diving and turning)
   verticalSurfaceForce = 4;    // Vertical center "wing" (controls side slip. lower numbers make MORE slide.)
   autoInputDamping = 0.35;//0.55     // Dampen control input so you don't` whack out at very slow speeds
   steeringForce = 500;//300         // Steering jets (force applied when you move the mouse)
   steeringRollForce = 350;//300     // Steering jets (how much you heel over when you turn)
   rollForce = 50;//35              // Auto-roll (self-correction to right you after you roll/invert)
   autoAngularForce = 300;      // Angular stabilizer force (this force levels you out when autostabilizer kicks in)
   rotationalDrag = 10;         // Anguler Drag (dampens the drift after you stop moving the mouse...also tumble drag)
   autoLinearForce = 200;       // Linear stabilzer force (this slows you down when autostabilizer kicks in)
   maxAutoSpeed = 5;//15           // Autostabilizer kicks in when less than this speed. (meters/second)
   vertThrustMultiple = 3.0;

   minRollSpeed = 2000;
   maxSteeringAngle = 2;//0.785; // Max radiens you can rotate the wheel. Smaller number is more maneuverable.

   // Drag will affect max speed thank goodness
   maxDrag = 50; //15
   minDrag = 30; //10

   // Collision
   minImpactSpeed = 10;    // Impacts over this speed invoke the script callback
   softImpactSpeed = 15;
   hardImpactSpeed = 25;
   integration = 3;        // Force integration time: TickSec/Rate  default is 1
   collisionTol = 0.6;     // Collision distance tolerance
   contactTol = 0.4;       // Contact velocity tolerance

   speedDamageScale = 0.06;
   collDamageThresholdVel = 23;
   collDamageMultiplier = 0.02;

   //Particles
   forwardJetEmitter = JetEmitter;
   backwardJetEmitter = JetEmitter;
   downJetEmitter = JetEmitter;
   trailEmitter = ContrailEmitter;
   minTrailSpeed = 15;

   dustEmitter = VehicleDustEmitter;
   triggerDustHeight = 5.0;
   dustHeight = 1.0;

   damageEmitter[0] = LightDamageSmoke;
   damageEmitter[1] = HeavyDamageSmoke;
   damageEmitter[2] = DamageBubbles;
   damageEmitterOffset[0] = "0.0 -3.0 +2.0";
   damageLevelTolerance[0] = 0.3;
   damageLevelTolerance[1] = 0.7;
   numDmgEmitterAreas = 1;

   splashEmitter[0] = VehicleSplashDropletsEmitter;
   splashEmitter[1] = VehicleSplashEmitter;

   splashFreqMod = 300.0;
   splashVelEpsilon = 0.50;
   exitSplashSoundVelocity = 10.0;
   softSplashSoundVelocity = 10.0;
   mediumSplashSoundVelocity = 15.0;
   hardSplashSoundVelocity = 20.0;

   // Audio
   softImpactSound = softImpact;
   hardImpactSound = hardImpact;
   exitingWater = softImpact;
   impactWaterEasy = softImpact;
   impactWaterMedium = softImpact;
   impactWaterHard = hardImpact;
   waterWakeSound = softImpact;
   jetSound = cheetahSqueal;
   engineSound = cheetahEngine;

   // Accessed via script
   nameTag = 'Talon Fighter Bomber';
   checkRadius = 10;
   dismountRadius = 20;
   maxDismountSpeed = 200;
   maxMountSpeed = 5;
   // Radius damage
   canImpulse = true;
   numWeapons = 0;
};

//-----------------------------------------------------------------------------
// FUNCTIONS

function Talon::playerMounted(%data, %obj, %player, %node)
{
   //commandToClient(%player.client, 'setHudMode', 'Pilot', %data.getClassName(), %node);
   Parent::PlayerMounted(%data, %obj, %player, %node);

   // The flyer can get stuck in the terrain so we have to free it before we mount it..
   %obj.applyKick(5, 5, "up");
}
/*
function Talon::onAdd(%this, %obj)
{
   Parent::onAdd( %this, %obj );

   //%obj.mountImage( TalonRocketParam, 0 );
   //%obj.mountImage( TalonRocketImage, 2 );
   //%obj.mountImage( TalonRocketPairImage, 3 );
   %obj.mountImage( TalonBomberImage, 1 );
   %obj.mountImage( XM66Image, 2 );
   %obj.mountImage( XM66PairImage, 3 );

   %obj.selectedWeapon = 1;
   %obj.nextWeaponFire = 2;
   %obj.schedule( 5500, "playThread", 0, "activate" );
}

function Talon::playerDismounted(%data, %obj, %player)
{
   %obj.fireWeapon = false;
   %obj.setImageTrigger(1, false);
   %obj.setImageTrigger(2, false);
   %obj.setImageTrigger(3, false);

   Parent::playerDismounted(%data, %obj, %player);
}

function Talon::onTrigger(%data, %obj, %trigger, %state)
{
   //echo("Talon::onTrigger(" SPC %data.getName() @", "@ %obj.getControllingClient().nameBase @", "@ %trigger @", "@ %state SPC ")");
   if ( %trigger == 0 )
   {
      //echo("Selected Weapon:" SPC %obj.selectedWeapon);
      switch (%state)
      {
         case 0:
            %obj.fireWeapon = false;
            %obj.setImageTrigger(1, false);
            %obj.setImageTrigger(2, false);
            %obj.setImageTrigger(3, false);

         case 1:
            %obj.startTime = getSimTime(); // Grab the trigger event. We use this to switch images
            %obj.fireWeapon = true;
            if ( %obj.selectedWeapon == 1 )
            {
               %obj.setImageTrigger(1, false);
               if ( %obj.nextWeaponFire == 2 )
               {
                  %obj.setImageTrigger(2, true);
                  %obj.setImageTrigger(3, false);
               }
               else
               {
                  %obj.setImageTrigger(2, false);
                  %obj.setImageTrigger(3, true);
               }
            }
            else
            {
               %obj.setImageTrigger(1, true);
               %obj.setImageTrigger(2, false);
               %obj.setImageTrigger(3, false);
            }
      }
   }
}

function Talon::fireNextGun(%this, %obj)
{
   //error("Talon::fireNextGun(" SPC %obj.nextWeaponFire SPC ")");
   //if ( isEventPending( %obj.fireSchedule ) )
   //   cancel( %obj.fireSchedule );

   %obj.setImageTrigger(1, false);
   if ( %obj.fireWeapon )
   {
      %obj.startTime = getSimTime(); // Grab the trigger event. We use this to switch images
      if ( %obj.nextWeaponFire == 2 )
      {
         %obj.setImageTrigger(2, true);
         %obj.setImageTrigger(3, false);
      }
      else
      {
         %obj.setImageTrigger(2, false);
         %obj.setImageTrigger(3, true);
      }
   }
   else
   {
      %obj.setImageTrigger(2, false);
      %obj.setImageTrigger(3, false);
   }
}
*/
//-----------------------------------------------------------------------------
// SMS

//             |Vehicle DB| |Vehicle Name| |Max Allowed|
SmsInv.AddVehicle(Talon, "Talon Fighter", 4);
