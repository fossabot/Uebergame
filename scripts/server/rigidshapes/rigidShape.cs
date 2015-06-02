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

// Hook into the mission editor.
function RigidShapeData::create(%data)
{
   // The mission editor invokes this method when it wants to create
   // an object of the given datablock type.
   %obj = new RigidShape() {
      dataBlock = %data;
      scale = %data.scale !$= "" ? %data.scale : "1 1 1";
   };
   return %obj;
}

function RigidShapeData::onAdd(%data, %obj)
{
   %obj.setEnergyLevel(%data.MaxEnergy);
   %obj.setRechargeRate(%data.rechargeRate);
   %obj.setRepairRate(0);
}

function RigidShapeData::onNewDataBlock(%data, %obj)
{
   //echo("RigidShapeData::onNewDataBlock(" SPC %data @", "@ %obj SPC ")");
}

function RigidShapeData::damage(%data, %obj, %sourceObject, %position, %amount, %damageType)
{
   if ( %obj.isDestroyed() || %data.isInvincible || !$BaseSacking )
      return;

   //echo("RigidShapeData::damage( "@%data.getName()@", "@%obj@", "@sourceObject@", "@%position@", "@%amount@", "@%damageType@" )");

   if ( isObject( %sourceObject ) )
      %obj.lastDamagedBy = %sourceObject;
   else
      %obj.lastDamagedBy = 0;

   %obj.damageTimeMS = GetSimTime();

   if ( %data.isShielded )
      %amount = %obj.imposeShield(%position, %amount, %damageType);

   // Cap the amount of damage applied if same team
   //if ( !%data.deployedObject )
   //{
   //   if ( isObject( %sourceObject ) )
   //   {
   //      if ( %sourceObject.team == %obj.team )
   //      {
   //         %curDamage = %obj.getDamageLevel();
   //         %availableDamage = %data.disabledLevel - %curDamage - 0.05;
   //         if ( %amount > %availableDamage )
   //            %amount = %availableDamage;
   //      }
   //   }
   //}

   %damageScale = %data.damageScale[%damageType];
   if( %damageScale !$= "" )
      %amount *= %damageScale;

   // apply damage
   if (%amount > 0)
      %obj.applyDamage(%amount);
}

function RigidShapeData::onDamage(%data, %obj)
{
   //echo("RigidShapeData::onDamage( "@%data@", "@%obj@" )");
   // Set damage state based on current damage level
   %damage = %obj.getDamageLevel();
   if ( %damage >= %data.destroyedLevel )
   {
      if ( %obj.getDamageState() !$= "Destroyed" )
      {
         %obj.setDamageState(Destroyed);
         // if object has an explosion damage radius associated with it, apply explosion damage
         if(%data.damageRadius)
            radiusDamage(%obj, 0, %obj.getWorldBoxCenter(), %data.damageRadius, %data.radiusDamage, %data.radiusDamageType, %data.areaImpulse);

         %obj.setDamageLevel(%data.maxDamage);
      }
   }
   else if(%damage >= %data.disabledLevel)
   {
      if ( %obj.getDamageState() !$= "Disabled" )
         %obj.setDamageState(Disabled);
   }
   else
   {
      // Lets add some sound to this, grab the sound profile from the datablock - ZOD
      if ( %data.damageSound !$= "" )
         ServerPlay3D(%data.damageSound, %obj.getTransform());

      if ( %obj.getDamageState() !$= "Enabled" )
         %obj.setDamageState(Enabled);
   }
}

function RigidShapeData::onEnabled(%data, %obj, %state)
{
   //echo("RigidShapeData::onEnabled( "@%data@", "@%obj@", "@%state@" )");

   // Lets add some sound to this, grab the sound profile from the datablock - ZOD
   if ( %data.ambientSound !$= "" )
      ServerPlay3D(%data.ambientSound, %obj.getTransform());
}

function RigidShapeData::onDisabled(%data, %obj, %state)
{
   //echo("RigidShapeData::onDisabled( "@%data@", "@%obj@", "@%state@" )");
}

function RigidShapeData::onDestroyed(%data, %obj, %prevState)
{
   //echo("RigidShapeData::onDestroyed( "@%data@", "@%obj@", "@%prevState@" )");

   // delete object
   if ( !%data.renderWhenDestroyed )
      %obj.schedule(200, "delete");
}

function RigidShapeData::onCollision(%data, %obj, %col, %vec, %speed)
{
   //error("RigidShapeData::onCollision(" SPC %data.getName() @", "@ %obj @", "@ %col.getClassName() @", "@ %vec @", "@ %speed SPC ")");
   //if it is colliding with it's self or the terrain, ignore it
   if ( %obj == %col || %col.getType() & $TypeMasks::TerrainObjectType || %speed < 1 )
      return;

   if ( %col.getType() & $TypeMasks::ShapeBaseObjectType )
   {
      if ( %obj.lastCollider = %col )
      {
         //if we just ran into this one, lets give it half a second before we apply another impulse
         if ( getSimTime() - %obj.lastColTime < 550 )
            return;
      }

      %velocity = %col.getVelocity();
      %normal = vectorDot( %velocity, VectorNormalize( %velocity ) );
      if( %normal < 15 )
         %multi = 8;
      else
         %multi = 8 / %normal;

      %objPos = %obj.posFromTransform();
      %colPos = %col.posFromTransform();
      %vec = VectorNormalize( VectorSub( %objPos, %colPos ) );
      %force = VectorScale( %vec, %col.getDatablock().mass * %multi );
      %obj.applyImpulse( %objPos, %force );

      %obj.lastCollider = %col;
      %obj.lastColTime = getSimTime();
   }
}
