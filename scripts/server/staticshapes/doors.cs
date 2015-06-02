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

datablock SFXProfile(MetalGrillOpenSound)
{
   filename = "art/sound/doors/powered_door_open.ogg";
   description = AudioClose3d;
   preload = false;
};

datablock SFXProfile(MetalGrillCloseSound)
{
   filename = "art/sound/doors/doorSlide.wav";
   description = AudioClose3d;
   preload = false;
};

datablock SFXProfile(MetalGrillDeniedSound)
{
   filename = "art/sound/doors/door_locked.ogg";
   description = AudioClose3d;
   preload = false;
};

datablock StaticShapeData(MetalGrillDoor : StaticShapeDamageScale)
{
   category = "Doors";
   className = Door;

   shapeFile = "art/shapes/doors/metalGrillTrans.dts";
   emap = true;
   dynamicType = $TypeMasks::StaticShapeObjectType;
   aiAvoidThis = false;
   computeCRC = true;

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
   density = 1;

   isShielded = false;
   isInvincible = false;
   maxDamage = 50.1;// Must be higher then destroyed level
   destroyedLevel = 50;
   disabledLevel = 40;

   debrisShapeName =  "art/shapes/objects/debris.dts";
   debris = SmallExplosionDebris;
   renderWhenDestroyed = 1;

   explosion = SmallExplosion;
   damageRadius = 2.0;
   radiusDamage = 5.0;
   damageType = $DamageType::Explosion;
   areaImpulse = 500;

   repairRate = 0.005;

   energyPerDamagePoint = 50;
   maxEnergy = 100;
   inheritEnergyFromMount = false;
   rechargeRate = 0.31;

   // Script access
   nameTag = 'Door';
   openSound = MetalGrillOpenSound;
   closeSound = MetalGrillCloseSound;
   deniedSound = MetalGrillDeniedSound;
   damageSound = "";
   ambientSound = "";
   isTeamDoor = true;
   isDoor = true;

   // Radius damage
   canImpulse = false;
   deployedObject = false;
};

datablock StaticShapeData(RedZoneDoor : MetalGrillDoor)
{
   shapeFile = "art/shapes/doors/door_redZone.dts";
};

datablock StaticShapeData(GrillDoor : MetalGrillDoor)
{
   shapeFile = "art/shapes/doors/metalGrill.dts";
};

datablock StaticShapeData(WoodDoor : MetalGrillDoor)
{
   shapeFile = "art/shapes/doors/woodDoor01.dts";
};

datablock StaticShapeData(SectionDoor : MetalGrillDoor)
{
   shapeFile = "art/shapes/doors/metalDoorSec1.dts";
};

datablock StaticShapeData(Door01 : MetalGrillDoor)
{
   shapeFile = "art/shapes/doors/door1.dts";
};

datablock StaticShapeData(Door02 : MetalGrillDoor)
{
   shapeFile = "art/shapes/doors/door2.dts";
};

datablock StaticShapeData(Door03 : MetalGrillDoor)
{
   shapeFile = "art/shapes/doors/door3.dts";
};

function Door::onAdd(%data, %obj)
{
   Parent::onAdd(%data, %obj);
   %obj.setRepairRate( %data.repairRate );
}

function Door::onGainPowerEnabled(%data, %obj)
{
   Parent::onGainPowerEnabled(%data, %obj);
   if ( %obj.open )
   {
      %obj.setthreaddir(0, "false");
      serverPlay3D( %data.closeSound, %obj.getPosition() );
      %obj.open = false;
   }
}

function Door::onLosePowerDisabled(%data, %obj)
{
   Parent::onLosePowerDisabled(%data, %obj);
   if ( !%obj.open )
   {
      %obj.open = true;
      %obj.setthreaddir(0, "true");
      %obj.playthread(0, "open");
      serverPlay3D( %data.openSound, %obj.getPosition() );
   }
}

function Door::onCollision(%data, %obj, %col)
{
   // Only player objects can open doors, since when does a car have a hand?
   if ( %col.getType() & ( $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType ) )
   {
      // Dead men tell no tales. Must be a door on your team or Switzerland to open.
      if( %col.getState() $= "Dead" || ( %data.isTeamDoor && %col.team != %obj.team && %obj.team != 0 ) )
      {
         if ( isObject( %col.client ) )
            messageClient( %col.client, 'MsgError', '\c2Door is locked.');

         serverPlay3D( %data.deniedSound, %obj.getPosition() );
         return;
      }

      // Can't open an open door fool!
      if ( !%obj.open )
      {
         %obj.open = true;
         %obj.setThreadTimeScale( 0, 0.25 );
         %obj.setthreaddir( 0, "true" );
         %obj.playthread( 0, "open" );
         serverPlay3D( %data.openSound, %obj.getPosition() );
         %data.schedule( 2000, "close", %obj, %col );
      }
   }
}

function Door::close(%data, %obj, %col)
{
   // Object that collided left the server perhaps? Close the door.
   if ( !isObject( %col ) )
   {
      %obj.setThreadTimeScale( 0, 0.25 );
      %obj.setthreaddir(0, "false");
      serverPlay3D( %data.closeSound, %obj.getPosition() );
      %obj.open = false;
   }

   // If collision object is far enough away close the door.
   %dist = VectorDist( %col.posFromTransform(), %obj.getPosition() );
   if ( %dist > 2 )
   {
      %obj.setThreadTimeScale( 0, 0.25 );
      %obj.setthreaddir(0, "false");
      serverPlay3D( %data.closeSound, %obj.getPosition() );
      %obj.open = false;
   }
   else
      //if the collision object is too close, just go ahead and start the timer again
      %data.schedule( 2000, "close", %obj, %col );
}
