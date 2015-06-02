//--------------------------------------------------------------------------
// Sounds
//--------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Tracer particles
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Splash particles
//-----------------------------------------------------------------------------

// ----------------------------------------------------------------------------
// Particles
// ----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Explosion
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Light
//-----------------------------------------------------------------------------

datablock LightDescription(GrenadeLightDesc)
{
   color = "0.8 0.8 0";
   range = 2.0;
   brightness = 1;
   animationType = NullLightAnim;
};

//-----------------------------------------------------------------------------
// Projectile Object
//-----------------------------------------------------------------------------

datablock ProjectileData( ThrownGrenade )
{
   projectileShapeName = "art/shapes/weapons/grenade/grenade.dae";
   //sound               = "";
   directDamage        = 0;
   radiusDamage        = 75;
   damageRadius        = 10;
   areaImpulse         = 1;
   impactForce         = 5;

   damageType          = $DamageType::Grenade;
   areaImpulse    = 1500;

   explosion           = GrenadeExplosion;
   waterExplosion      = UnderwaterGrenadeExplosion;
   decal               = ScorchRXDecal;

   //particleEmitter     = "";
   //particleWaterEmitter = "";

   Splash              = GrenadeSplash;
   muzzleVelocity      = 20;
   velInheritFactor    = 0.5;

   armingDelay         = 1500; // How long it should not detonate on impact
   lifetime            = 6000; // How long the projectile should exist before deleting itself
   fadeDelay           = 4000; // Brief Amount of time, in milliseconds, before the projectile begins to fade out.

   bounceElasticity    = 0.6;
   bounceFriction      = 0.3;
   isBallistic         = 1; // Causes the projectile to be affected by gravity "arc".
   gravityMod          = 1;

   lightDesc           = "GrenadeLightDesc";
};

//-----------------------------------------------------------------------------
// Ammo Item
//-----------------------------------------------------------------------------
datablock ItemData(GrenadeMag : DefaultClip)
{
   shapeFile = "art/shapes/weapons/grenade/grenade.dae";
   pickUpName = 'Grenade Magazine';
};

datablock ItemData(GrenadeWeaponAmmo : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/grenade/grenade.dae";
   pickUpName = 'Grenades';
};

//--------------------------------------------------------------------------
// Weapon Item.  This is the item that exists in the world, i.e. when it's
// been dropped, thrown or is acting as re-spawnable item.  When the weapon
// is mounted onto a shape, the SoldierWeaponImage is used.
//-----------------------------------------------------------------------------
datablock ItemData(GrenadeWeapon : DefaultWeapon)
{
   shapeFile = "art/shapes/weapons/grenade/grenade.dae";
   pickUpName = 'Frag Grenade';
   image = GrenadeWeaponImage;
};

datablock ShapeBaseImageData(GrenadeWeaponImage)
{
   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   class = "WeaponImage";
   className = "WeaponImage";

   // Basic Item properties
   shapeFile = "art/shapes/weapons/grenade/grenade.dae";
   shapeFileFP = "art/shapes/weapons/grenade/grenade.dae";
   emap = true;
   computeCRC = false;

   imageAnimPrefix = "ProxMine";
   imageAnimPrefixFP = "ProxMine";

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   firstPerson = true;
   useEyeNode = true;
   animateOnServer = true;
   cloakable = true;

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.
   correctMuzzleVector = true;
   correctMuzzleVectorTP = true;

   // Projectiles and Ammo.
   item = GrenadeWeapon;
   ammo = GrenadeWeaponAmmo;
   //clip = GrenadeMag;

   usesEnergy = 0;
   minEnergy = 0;

   projectile = ThrownGrenade;
   projectileType = Projectile;
   projectileSpread = 0;

   //altProjectile = GrenadeProjectileAlt;
   //altProjectileSpread = "0.02";

   //casing = GrenadeShell;
   //shellExitDir        = "1.0 0.3 1.0";
   //shellExitOffset     = "0.15 -0.56 -0.1";
   //shellExitVariance   = 15.0;
   //shellVelocity       = 3.0;

   // Weapon lights up while firing
   lightType = "NoLight"; // NoLight, ConstantLight, SpotLight, PulsingLight, WeaponFireLight.
   lightColor = "0.82 0.84 0.47 1.0";
   lightDuration = "100"; //< The duration in SimTime of Pulsing or WeaponFire type lights.
   lightRadius = 1.0; // Extent of light.
   lightBrightness = 2; //< Brightness of the light ( if it is WeaponFireLight ).

   // Shake camera while firing.
   shakeCamera = false;
   camShakeFreq = "0 0 0";
   camShakeAmp = "0 0 0";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   useRemainderDT = true;

   // Initial start up state
   stateName[0]                     = "Preactivate";
   stateTransitionOnLoaded[0]       = "Activate";
   stateTransitionOnNoAmmo[0]       = "NoAmmo";

   // Activating the gun.  Called when the weapon is first
   // mounted and there is ammo.
   stateName[1]                     = "Activate";
   stateTransitionGeneric0In[1]     = "SprintEnter";
   stateTransitionOnTimeout[1]      = "Ready";
   stateTimeoutValue[1]             = 0.5;
   stateSequence[1]                 = "switch_in";
   //stateSound[1]                    = GLSwitchinSound;

   // Ready to fire, just waiting for the trigger
   stateName[2]                     = "Ready";
   stateTransitionGeneric0In[2]     = "SprintEnter";
   stateTransitionOnMotion[2]       = "ReadyMotion";
   stateScaleAnimation[2]           = false;
   stateScaleAnimationFP[2]         = false;
   stateTransitionOnNoAmmo[2]       = "NoAmmo";
   stateTransitionOnTriggerDown[2]  = "Charge";
   stateSequence[2]                 = "idle";

   // Ready to fire with player moving
   stateName[3]                     = "ReadyMotion";
   stateTransitionGeneric0In[3]     = "SprintEnter";
   stateTransitionOnNoMotion[3]     = "Ready";
   stateWaitForTimeout[3]           = false;
   stateScaleAnimation[3]           = false;
   stateScaleAnimationFP[3]         = false;
   stateSequenceTransitionIn[3]     = true;
   stateSequenceTransitionOut[3]    = true;
   stateTransitionOnNoAmmo[3]       = "NoAmmo";
   stateTransitionOnTriggerDown[3]  = "Charge";
   stateSequence[3]                 = "run";

   // Fire the weapon. Calls the fire script which does
   // the actual work.
   stateName[4]                     = "Fire";
   stateTransitionGeneric0In[4]     = "SprintEnter";
   stateTransitionOnTimeout[4]      = "WaitForRelease";
   stateTimeoutValue[4]             = 0.4;
   stateWaitForTimeout[4]           = true;
   stateFire[4]                     = true;
   stateRecoil[5]                   = "light_recoil";
   stateAllowImageChange[4]         = false;
   stateSequence[4]                 = "fire";
   stateScaleAnimation[4]           = true;
   stateSequenceNeverTransition[4]  = true;
   stateSequenceRandomFlash[4]      = true;        // use muzzle flash sequence
   stateScript[4]                   = "onFire";
   stateEjectShell[4]               = false;
   //stateSound[4]                    = GLFireSound;

   // Wait for the player to release the trigger
   stateName[5]                     = "WaitForRelease";
   stateTransitionGeneric0In[5]     = "SprintEnter";
   stateTransitionOnTriggerUp[5]    = "Reload";
   stateTimeoutValue[5]             = 0.05;
   stateWaitForTimeout[5]           = true;
   stateAllowImageChange[5]         = false;

   // Put another round in the chamber
   stateName[6]                     = "Reload";
   stateTransitionGeneric0In[6]     = "SprintEnter";
   stateTransitionOnNoAmmo[6]       = "NoAmmo";
   stateTransitionOnTimeout[6]      = "Ready";
   stateWaitForTimeout[6]           = false;
   stateTimeoutValue[6]             = 0.5;
   stateAllowImageChange[6]         = false;

   // No ammo in the weapon, just idle until something
   // shows up. Play the dry fire sound if the trigger is
   // pulled.
   stateName[7]                     = "NoAmmo";
   stateTransitionGeneric0In[7]     = "SprintEnter";
   stateTransitionOnMotion[7]       = "NoAmmoMotion";
   stateTransitionOnAmmo[7]         = "Reload";
   stateTimeoutValue[7]             = 0.1;   // Slight pause to allow script to run when trigger is still held down from Fire state
   stateTransitionOnTriggerDown[7]  = "DryFire";
   stateSequence[7]                 = "idle";
   stateScaleAnimation[7]           = false;
   stateScaleAnimationFP[7]         = false;
   stateScript[6]                   = "switchWeapon";

   stateName[8]                     = "NoAmmoMotion";
   stateTransitionGeneric0In[8]     = "SprintEnter";
   stateTransitionOnNoMotion[8]     = "NoAmmo";
   stateWaitForTimeout[8]           = false;
   stateScaleAnimation[8]           = false;
   stateScaleAnimationFP[8]         = false;
   stateSequenceTransitionIn[8]     = true;
   stateSequenceTransitionOut[8]    = true;
   stateTransitionOnAmmo[8]         = "Reload";
   stateTransitionOnTriggerDown[8]  = "DryFire";
   stateSequence[8]                 = "run";

   // No ammo dry fire
   stateName[9]                     = "DryFire";
   stateTransitionGeneric0In[9]     = "SprintEnter";
   stateTransitionOnAmmo[9]         = "Reload";
   stateWaitForTimeout[9]           = "0";
   stateTimeoutValue[9]             = 1.5;
   stateTransitionOnTimeout[9]      = "NoAmmo";
   stateScript[9]                   = "onDryFire";

   // Start Sprinting
   stateName[10]                    = "SprintEnter";
   stateTransitionGeneric0Out[10]   = "SprintExit";
   stateTransitionOnTimeout[10]     = "Sprinting";
   stateWaitForTimeout[10]          = false;
   stateTimeoutValue[10]            = 0.5;
   stateWaitForTimeout[10]          = false;
   stateScaleAnimation[10]          = false;
   stateScaleAnimationFP[10]        = false;
   stateSequenceTransitionIn[10]    = true;
   stateSequenceTransitionOut[10]   = true;
   stateAllowImageChange[10]        = false;
   stateSequence[10]                = "sprint";

   // Sprinting
   stateName[11]                    = "Sprinting";
   stateTransitionGeneric0Out[11]   = "SprintExit";
   stateWaitForTimeout[11]          = false;
   stateScaleAnimation[11]          = false;
   stateScaleAnimationFP[11]        = false;
   stateSequenceTransitionIn[11]    = true;
   stateSequenceTransitionOut[11]   = true;
   stateAllowImageChange[11]        = false;
   stateSequence[11]                = "sprint";
   
   // Stop Sprinting
   stateName[12]                    = "SprintExit";
   stateTransitionGeneric0In[12]    = "SprintEnter";
   stateTransitionOnTimeout[12]     = "Ready";
   stateWaitForTimeout[12]          = false;
   stateTimeoutValue[12]            = 0.5;
   stateSequenceTransitionIn[12]    = true;
   stateSequenceTransitionOut[12]   = true;
   stateAllowImageChange[12]        = false;
   stateSequence[12]                = "sprint";

   stateName[13]                    = "Charge";
   stateScript[13]                  = "chargeStart";
   stateTransitionOnTriggerUp[13]   = "Fire";
   stateTransitionOnTimeout[13]     = "Fire";
   stateTimeoutValue[13]            = 2;
   stateWaitForTimeout[13]          = false;
};

function GrenadeWeaponImage::onFire(%data, %obj, %slot)
{
   LogEcho("GrenadeWeaponImage::onFire(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %slot SPC ")");

   if ( %data.ammo !$="" && !%obj.client.isAiControlled() ) // Ai has unlimited ammo, cause.. lazy bones
   {
      if ( %obj.getInventory( %data.ammo ) <= 0 )
         return;

      // Decrement inventory ammo. The image's ammo state is update
      // automatically by the ammo inventory hooks.
      %obj.decInventory( %data.ammo, 1 );
   }

   // Work out the force of the throw
   %throwStrength = (getSimTime() - %obj.startTime) / 1000;
   if( %throwStrength < 1 ) // Realy really short hold?
      %throwStrength = 1;

   if( %throwStrength > 2 )
      %throwStrength = 2;

   // Bots just toss them with no charge up so lets change this up a little..
   %client = %obj.client;
   if ( ( isObject( %client ) && %client.isAiControlled() ) || %obj.isBot )
      %throwStrength = 1.25;

   %projVelocity = %throwStrength * 10;

   %data.lightStart = $Sim::Time;

   //if ( %obj.getClassname() $= "Player" || %obj.getClassname() $= "AiPlayer" )
   if ( %obj.isMemberOfClass( "Player" ) )
      %obj.setInvincible( false ); // fire your weapon and your invincibility goes away.

   if( %obj.inStation $= "" && %obj.isCloaked() )
   {
      if( %obj.respawnCloakThread !$= "" )
      {
         cancel(%obj.respawnCloakThread);
         %obj.setCloaked( false );
         %obj.respawnCloakThread = "";
      }
      else
      {
         if( %obj.getEnergyLevel() > 20 )
         {   
            %obj.setCloaked( false );
            %obj.reCloak = %obj.schedule( 1000, "setCloaked", true ); 
         }
      }   
   }

   if (isObject(%obj.lastProjectile) && %obj.deleteLastProjectile)
      %obj.lastProjectile.delete();


   // Add a vertical component to give the object a better arc
   %vec = %obj.getMuzzleVector(%slot);
   %muzzleVector = vectorAdd( %vec, "0 0 0.5" );

   //%muzzleVector = %obj.getMuzzleVector(%slot);

   // Determin initial projectile velocity based on the 
   // gun's muzzle point and the object's current velocity
   %objectVelocity = %obj.getVelocity();
   %muzzleVelocity = VectorAdd( VectorScale( %muzzleVector, %projVelocity ),
                     VectorScale( %objectVelocity, %data.projectile.velInheritFactor ) );

   // Create the projectile object
   %p = new (%data.projectileType)() {
      dataBlock        = %data.projectile;
      initialVelocity  = %muzzleVelocity;
      initialPosition  = %obj.getMuzzlePoint(%slot);
      // This parameter is deleted about 7 ticks into the projectiles flight
      sourceObject     = %obj;
      sourceSlot       = %slot;
      // We use this for the source object when applying damage because it isn't deleted
      origin           = %obj;
      client           = %obj.client;
   };

   %obj.lastProjectile = %p;
   %obj.deleteLastProjectile = %data.deleteLastProjectile;
   if(%obj.client)
      %obj.client.projectile = %p;

   MissionCleanup.add(%p);

   %obj.unmountImage(%slot);
   if ( %obj.inv[%obj.lastWeapon] )
      %obj.use( %obj.lastWeapon );
   else
      %obj.use( %obj.weaponSlot[0] );

   return %p;
}

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowWeapon("Soldier");
SmsInv.AddWeapon(GrenadeWeapon, "Frag Grenade", 1);

//SmsInv.AllowClip("armor\tSoldier\t3");
//SmsInv.AddClip(GrenadeMag, 3);

SmsInv.AllowAmmo("armor\tSoldier\t3");
SmsInv.AddAmmo(GrenadeWeaponAmmo, 1);
