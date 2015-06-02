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

datablock StaticShapeData(BoxA : StaticShapeDamageScale)
{
   category = "Crates";

   shapeFile = "art/shapes/storage/crates/generic/generic_crate_01_a.dae";
   emap = true;
   dynamicType = $TypeMasks::StaticShapeObjectType;
   computeCRC = true;
   scale = "10 10 10";

   cameraMaxDist = 2.20449;
   cameraMinDist = 0.2;
   cameraDefaultFov = 90;
   cameraMinFov = 5;
   cameraMaxFov = 120;
   firstPersonOnly = false;
   useEyePoint = false;
   observeThroughObject = false;

   mass = 10;
   drag = 1;
   density = 20;

   isShielded = false;
   repairRate = 0;
   energyPerDamagePoint = 75;
   maxEnergy = 50;
   rechargeRate = 0;
   inheritEnergyFromMount = false;

   isInvincible = false;
   maxDamage = 15.1; // Must be higher then destroyed level
   destroyedLevel = 15.0;
   disabledLevel = 14.0;

   debrisShapeName =  "art/shapes/objects/debris.dts";
   debris = SmallExplosionDebris;
   renderWhenDestroyed = true;

   explosion = SmallExplosion;
   damageRadius = 2.0;
   radiusDamage = 5.0;
   damageType = $DamageType::Explosion;
   areaImpulse = 500;

   nameTag = 'Crate';
   damageSound = "";
   ambientSound = "";

   // Radius damage
   canImpulse = false;
   deployedObject = false;
};

datablock StaticShapeData(BoxB : BoxA)
{
   shapeFile = "art/shapes/storage/crates/generic/generic_crate_01_b.dae";
};
