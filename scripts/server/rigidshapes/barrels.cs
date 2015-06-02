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

datablock RigidShapeData(Barrel01 : StaticShapeDamageScale)
{	
   category = "Barrels";
   className = "Barrel";
   shapeFile = "art/shapes/storage/barrels/wood/barrel_wood_01_a.dae";
   cloakTexture = "art/particles/cloakTexture.png";
   computeCRC = true;
   emap = true;

   // Rigid Body
   mass = 100;
   massCenter = "0 0 0";    // Center of mass for rigid body
   massBox = "0 0 0";         // Size of box used for moment of inertia,
                              // if zero it defaults to object bounding box
   drag = 0.2;                // Drag coefficient
   maxDrag = 0.5;
   minDrag = 0.05;

   density = 20;
   bodyFriction = 0.5;
   bodyRestitution = 0.1;
   minImpactSpeed = 5;        // Impacts over this invoke the script callback
   softImpactSpeed = 5;       // Play SoftImpact Sound
   hardImpactSpeed = 15;      // Play HardImpact Sound

   integration = 4;           // Physics integration: TickSec/Rate
   collisionTol = 0.1;        // Collision distance tolerance
   contactTol = 0.1;          // Contact velocity tolerance
   
   minRollSpeed = 10;

   dragForce = 0.05;
   vertFactor = 0.05;

   normalForce = 0.05;
   restorativeForce = 0.05;
   rollForce = 0.05;
   pitchForce = 0.05;

   isShielded = false;
   repairRate = 0;
   maxEnergy = 10;
   rechargeRate = 0;
   energyPerDamagePoint = 75;
   inheritEnergyFromMount = false;

   isInvincible = false;
   maxDamage = 20.1; // Must be higher then destroyed level
   destroyedLevel = 20.0;
   disabledLevel = 19;

   debrisShapeName =  "art/shapes/objects/debris.dts";
   debris = SmallExplosionDebris;
   renderWhenDestroyed = false;

   explosion = SmallExplosion;
   damageRadius = 5.0;
   radiusDamage = 5;
   damageType = $DamageType::Explosion;
   areaImpulse = 800;

   dustEmitter = "LiftoffDustEmitter"; // scripts/server/player.cs
   triggerDustHeight = 1;
   dustHeight = 5;

   splashEmitter = "PlayerSplashEmitter"; // scripts/server/player.cs
   splashFreqMod = "300";
   splashVelEpsilon = "0.60";
   exitSplashSoundVelocity = "";
   softSplashSoundVelocity = "2";
   mediumSplashSoundVelocity = "5";
   hardSplashSoundVelocity = "8";

   //dustTrailEmitter = "";
   //dustTrailOffset = "";
   //triggerTrailHeight = "";
   //dustTrailFreqMod = "";

   softImpactSound = "LightImpactSoftSound";
   hardImpactSound = "LightImpactHardSound";
   exitingWater = "PlayerExitingWaterSound";
   impactWaterEasy = "PlayerImpactWaterEasySound";
   impactWaterMedium = "PlayerImpactWaterMediumSound";
   impactWaterHard = "PlayerImpactWaterHardSound";
   waterWakeSound = "LightFootStepWadingSound";

   // Radius damage
   canImpulse = true;
};

datablock RigidShapeData(Barrel02 : Barrel01)
{
   shapeFile = "art/shapes/storage/barrels/wood/barrel_wood_01_b.dae";
};

function Barrel::onDestroyed(%data, %obj, %prevState)
{
   if ( getRandom() < 0.5 )
      %obj.tossAmmoCrate();
   else
      %obj.tossPatch();

   Parent::onDestroyed(%data, %obj, %prevState);
}

