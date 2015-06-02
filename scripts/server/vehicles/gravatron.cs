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

datablock ParticleData(GravTrailParticle)
{
   textureName          = "art/particles/dustParticle";
   dragCoefficient      = 1.5;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS           = 200;
   lifetimeVarianceMS   = 0;

   colors[0] = "0.9 0.7 0.3 0.6";
   colors[1] = "0.3 0.3 0.5 0";
   colors[2] = "0.46 0.46 0.36 0";

   sizes[0] = 1.2;
   sizes[1] = 2.6;
   sizes[2] = 3;

   times[0] = 0;
   times[1] = 0.5;
   times[2] = 1;
};

datablock ParticleEmitterData(GravTrailEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 10;
   velocityVariance = 0;
   ejectionOffset   = 0;
   thetaMin         = 90;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   useEmitterColors = false;
   useEmitterSizes  = false;
   particles        = "GravTrailParticle\tContrailParticle";
};

datablock ParticleData(GravJetParticle)
{
   dragCoefficient      = 1.5;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS           = 100;
   lifetimeVarianceMS   = 0;
   textureName          = "art/particles/particleTest";

   colors[0] = "0.9 0.7 0.3 0.6";
   colors[1] = "0.3 0.3 0.5 0";

   sizes[0] = 0.5;
   sizes[1] = 1.5;
};

datablock ParticleEmitterData(GravJetEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;
   ejectionVelocity = 20;
   velocityVariance = 1.0;
   ejectionOffset   = 0;
   thetaMin         = 0;
   thetaMax         = 10;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   particles        = "GravJetParticle";
};

datablock HoverVehicleData(Gravatron : GravatronDamageScale)
{
   category = "Vehicles";
   shapeFile = "art/shapes/vehicles/hover/hover.dts";
   cloakTexture = "art/particles/cloakTexture.png";
   emap = true;

   numMountPoints = 1;
   mountPose[0] = root;
   isProtectedMountPoint[0] = false;

   cameraDefaultFov = 90;
   cameraMinFov = 5;
   cameraMaxFov = 120;
   cameraMaxDist = 8;
   cameraMinDist = 1;
   cameraOffset = 0.7;       // Vertical offset from camera mount point
   cameraRoll = false;       // Roll the camera with the vehicle
   cameraLag = 0.5;          // Velocity lag of camera
   cameraDecay = 0.75;       // Decay per sec. rate of velocity lag
   firstPersonOnly = false;
   useEyePoint = false;
   observeThroughObject = false;
   observeParameters = "1 10 10";

   Explosion = LargeExplosion;
   underwaterExplosion = LargeWaterExplosion;
   Debris = LargeExplosionDebris;
   renderWhenDestroyed = false;
   debrisShapeName = "art/shapes/objects/debris.dts";

   maxDamage = 301; // Must be higher then destroyed level
   disabledLevel = 290;
   destroyedLevel = 300;
   repairRate = 0.005;
   isInvincible = false;
   isShielded = true;

   inheritEnergyFromMount = false;
   rechargeRate = 0.7;
   energyPerDamagePoint = 75;
   maxEnergy = 150;
   minJetEnergy = 15;
   jetEnergyDrain = 1.3;

   //steering ala VehicleData
   maxSteeringAngle = 0.785;  // Maximum steering angle, should match animation
   hoverHeight = 1.5;       // Height off the ground at rest
   createHoverHeight = 2; // Height off the ground when created

   mass = 50;
   drag = 0.25;    //* from 0 - 1.0
   density = 0.9;
   //massCenter = "0 0 0"; // Center of mass for rigid body
   //massBox = "0 0 0";    // Size of box used for moment of inertia,
                           // if zero it defaults to object bounding box
   //Physics
   //maxDrag = 500;
   //minDrag = 250;

   bodyFriction = 0.1;     //when this gets high it CAN cause probs, doesn't always
   bodyRestitution = 0.5;  // 0.9
   minImpactSpeed = 29;    // Impacts over this invoke the script callback
   softImpactSpeed = 20;   // Play SoftImpact Sound
   hardImpactSpeed = 28;   // Play HardImpact Sound

   integration = 6;           // Physics integration: TickSec/Rate
   //these tols will often work better when closer to 1 (range 0-1)
   collisionTol = 0.1;        // Collision distance tolerance
   contactTol = 0.5;          // Contact velocity tolerance

   // Impact damage
   speedDamageScale = 0.05;
   collDamageThresholdVel = 23;
   collDamageMultiplier   = 0.03;

   // Hover Class
   dragForce              = 30;     //* Drag force factor that acts opposite to the vehicle velocity.
   vertFactor             = 1.0;    //* Scalar applied to the vertical portion of the velocity drag
   floatingThrustFactor   = 0.5;    //* Scalar applied to the vehicle's thrust force when the vehicle is floating.
   floatingGravMag        = 5;      //* Added to gravity, like a Z Drag. Forces vehicle down
                                    //*
   mainThrustForce        = 800;    //* Force generated by thrusting the vehicle forward.
   reverseThrustForce     = 600;    //* Force generated by thrusting the vehicle backward.
   strafeThrustForce      = 600;    //* Force generated by thrusting the vehicle to one side.
   turboFactor            = 1.5;    //* Scale factor applied to the vehicle's thrust force when jetting.
                                    //*
   brakingForce           = 900;    //* the force applied for brakes
   brakingActivationSpeed = 30;     //* how fast it applies brakes (higher = faster)
                                    //*
   stabLenMin             = 3;      //* hovers basically ride on a spring, which during normal movement keeps the vehicle within
   stabLenMax             = 6;      //* the following values distance from the ground
   stabSpringConstant     = 500;    //* higher values provide for less bobbles from bumps and direction changes
   stabDampingConstant    = 250;    //* higher values quickly smooths out residual up and down motion from landings and bumps
                                    //*
   gyroDrag               = 30;     //* determines turning rate of vehicle lower values provide for quick turn responce, but are less
                                    //* normalized and hard to control. lower numbers also decrease the effect of the direction it is
	                              //* facing upon its actual direction of momentum.
   normalForce            = 500;    //* normalizes the vehicles roll and pitch with respect to the terrain. High numbers provide
                                    //* a stable smooth positioning. Low numbers allow more roll and pitch and increases terrain collisions
   restorativeForce       = 250;    //* how fast the vehicle will be restored to paralell over the terrain; higher is faster
   steeringForce          = 800;   //* the rate of turn for the vehicle, higher is faster
   rollForce              = 250;    //* this makes rolling/banking (turning) more responsive (higher numbers reduces the ability
                                    //* to turn 180 degrees without changing the direction of momentum.
   pitchForce             = 100;    //* higher numbers gives more force and movement for pitching the nose up/down

   //Particles
   forwardJetEmitter = GravJetEmitter;
   dustTrailEmitter = GravTrailEmitter;
   dustTrailOffset = "0 0 0";
   triggerTrailHeight = 2.5;
   dustTrailFreqMod = 15.0;

   dustEmitter = VehicleDustEmitter;
   triggerDustHeight = 4;
   dustHeight = 1.0;

   damageEmitter[0] = LightDamageSmoke;
   damageEmitter[1] = HeavyDamageSmoke;
   damageEmitter[2] = DamageBubbles;
   damageEmitterOffset[0] = "0 0 -1.6";
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
   floatSound = DirtKickup;

   spawnOffset = "0 0 1";

   // Accessed via script
   nameTag = 'Gravatron Chariot';
   canImpulse = true;
   checkRadius = 10; // How large an area to look for things in the way when spawning
   dismountRadius = 20;
   maxDismountSpeed = 200;
   maxMountSpeed = 5;
   numWeapons = 0;
};

//-----------------------------------------------------------------------------
// Functions

function Gravatron::playerMounted(%data, %obj, %player, %node)
{
   Parent::PlayerMounted(%data, %obj, %player, %node);

   // The hover gets stuck in the terrain so we have to free it before we mount it..
   %obj.applyKick(5, 5, "up");
}

function Gravatron::onTrigger(%data, %obj, %trigger, %state)
{
   %player = %obj.getMountNodeObject(0);
   if ( %trigger == 2 )
   {
      if ( %state == 1 )
      {
         %player.getDataBlock().doDismount( %player, 0 );
      }
   }
}

//-----------------------------------------------------------------------------
// SMS

//             |Vehicle DB| |Vehicle Name| |Max Allowed|
SmsInv.AddVehicle(Gravatron, "Gravatron Chariot", 6);
