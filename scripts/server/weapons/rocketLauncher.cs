//--------------------------------------------------------------------------
// Sounds
//--------------------------------------------------------------------------

datablock SFXProfile(RLFireSound)
{
   filename = "art/sound/turret/wpn_turret_fire";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(RLReloadSound)
{
   filename = "art/sound/weapons/wpn_lurker_reload";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(RLSwitchinSound)
{
   filename = "art/sound/weapons/wpn_lurker_switchin";
   description = AudioClose3D;
   preload = true;
};

//-----------------------------------------------------------------------------
// Splash particles
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Explosion
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Underwater Explosion
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Particle Trails
//-----------------------------------------------------------------------------

// ----------------------------------------------------------------------------
// Muzzle flash
// ----------------------------------------------------------------------------

datablock ParticleData(RocketFireParticle)
{
   textureName = "art/particles/smokeParticle";

   dragCoeffiecient = 0;
   gravityCoefficient = 0;
   inheritedVelFactor = 1;

   lifetimeMS = 300;
   lifetimeVarianceMS = 0;

   spinRandomMin = -135;
   spinRandomMax =  135;

   colors[0] = "0.66 0.75 0.75 0.5";
   colors[1] = "0.1 0.1 0.5 0.8";
   colors[2] = "0.1 0.1 0.5 0.0";

   //colors[0] = "0.8 0.35 0.05 1.0";
   //colors[1] = "0.9 0.3 0.1 0.75";
   //colors[2] = "0.2 0.2 0.2 0.0";

   sizes[0] = 0.2;
   sizes[1] = 0.5;
   sizes[2] = 1;

   times[0] = 0.0;
   times[1] = 0.3;
   times[2] = 1.0;
};

datablock ParticleEmitterData(RocketFireEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;

   ejectionOffset = 0;
   ejectionVelocity = 10;
   velocityVariance = 0;

   thetaMin = 0;
   thetaMax = 0;

   particles = "RocketFireParticle";
};

//-----------------------------------------------------------------------------
// Light
//-----------------------------------------------------------------------------

datablock LightDescription(RocketLauncherLightDesc)
{
   color = "1 0 0 1";
   range = 2.0;
   brightness = 2.0;
   animationType = PulseLightAnim;
   animationPeriod = 0.25;
};

//-----------------------------------------------------------------------------
// Shell ejected during reload.
//-----------------------------------------------------------------------------

datablock DebrisData(RocketShell)
{
   shapeFile = "art/shapes/weapons/shared/RifleShell.DAE";
   scale = "2 2 2";
   lifetime = 6.0;
   minSpinSpeed = 150.0;
   maxSpinSpeed = 300.0;
   elasticity = 0.65;
   friction = 0.05;
   numBounces = 2;
   staticOnMaxBounce = true;
   snapOnMaxBounce = false;
   ignoreWater = true;
   fade = true;
};

//-----------------------------------------------------------------------------
// Projectile Object
//-----------------------------------------------------------------------------

datablock ProjectileData( RocketProjectile )
{
   projectileShapeName = "art/shapes/weapons/shared/rocket.dts";
   //sound               = "";
   directDamage        = 0;
   radiusDamage        = 50;
   damageRadius        = 10;
   areaImpulse         = 1500;
   impactForce         = 5;

   damageType          = $DamageType::RocketLauncher;

   explosion           = RocketLauncherExplosion;
   waterExplosion      = UnderwaterGrenadeExplosion;
   decal               = ScorchRXDecal;

   particleEmitter     = GrenadeTrailEmitter;
   particleWaterEmitter = UWGrenadeTrailEmitter;

   Splash              = RocketSplash;
   muzzleVelocity      = 80;
   velInheritFactor    = 1;

   armingDelay         = 0; // How long it should not detonate on impact
   lifetime            = 8000; // How long the projectile should exist before deleting itself
   fadeDelay           = 4000; // Brief Amount of time, in milliseconds, before the projectile begins to fade out.

   isBallistic         = 0; // Causes the projectile to be affected by gravity "arc".
   bounceElasticity    = 0;
   bounceFriction      = 0;
   gravityMod          = 1;

   lightDesc           = "RocketLauncherLightDesc";
};

//-----------------------------------------------------------------------------
// Ammo Item
//-----------------------------------------------------------------------------
datablock ItemData(RocketLauncherClip : DefaultClip)
{
   shapeFile = "art/shapes/weapons/grenade/grenade.dae";
   pickUpName = 'Rocket clip';
};

datablock ItemData(RocketLauncherAmmo : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/grenade/grenade.dae";
   pickUpName = 'Rockets';
   //clip = RocketLauncherClip;
};

//--------------------------------------------------------------------------
// Weapon Item.  This is the item that exists in the world, i.e. when it's
// been dropped, thrown or is acting as re-spawnable item.  When the weapon
// is mounted onto a shape, the SoldierWeaponImage is used.
//-----------------------------------------------------------------------------
datablock ItemData(RocketLauncher : DefaultWeapon)
{
   shapeFile = "art/shapes/weapons/Lurker/TP_Lurker.DAE";
   pickUpName = 'Rocket Launcher';
   image = RocketLauncherImage;
};

datablock ShapeBaseImageData(RocketLauncherImage)
{
   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   class = "WeaponImage";
   className = "WeaponImage";

   // Basic Item properties
   shapeFile = "art/shapes/weapons/Lurker/TP_Lurker.DAE";
   shapeFileFP = "art/shapes/weapons/Lurker/FP_Lurker.DAE";
   emap = true;
   computeCRC = false;

   imageAnimPrefix = "Rifle";
   imageAnimPrefixFP = "Rifle";

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   firstPerson = true;
   useEyeNode = true;
   animateOnServer = true;
   cloakable = true;

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   correctMuzzleVector = true;
   correctMuzzleVectorTP = true;

   // Projectiles and Ammo.
   item = RocketLauncher;
   ammo = RocketLauncherAmmo;
   //clip = RocketLauncherClip;

   usesEnergy = 0;
   minEnergy = 0;

   projectile = RocketProjectile;
   projectileType = Projectile;
   projectileSpread = 0.02;

   //altProjectile = RocketProjectileAlt;
   //altProjectileSpread = "0.02";

   casing = GrenadeShell;
   shellExitDir        = "1.0 0.3 1.0";
   shellExitOffset     = "0.15 -0.56 -0.1";
   shellExitVariance   = 15.0;
   shellVelocity       = 3.0;

   // Weapon lights up while firing
   lightType = "WeaponFireLight"; // NoLight, ConstantLight, SpotLight, PulsingLight, WeaponFireLight.
   lightColor = "0.82 0.84 0.47 1.0";
   lightDuration = "500"; //< The duration in SimTime of Pulsing or WeaponFire type lights.
   lightRadius = 2.0; // Extent of light.
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
   stateTimeoutValue[1]             = 2.0;
   stateSequence[1]                 = "switch_in";
   stateSound[1]                    = RLSwitchinSound;

   // Ready to fire, just waiting for the trigger
   stateName[2]                     = "Ready";
   stateTransitionGeneric0In[2]     = "SprintEnter";
   stateTransitionOnMotion[2]       = "ReadyMotion";
   stateScaleAnimation[2]           = false;
   stateScaleAnimationFP[2]         = false;
   stateTransitionOnNoAmmo[2]       = "NoAmmo";
   stateTransitionOnTriggerDown[2]  = "Fire";
   stateSequence[2]                 = "idle";
   stateTimeoutValue[2]             = 6.0;

   stateName[3]                     = "ReadyFidget";
   stateTransitionGeneric0In[3]     = "SprintEnter";
   stateTransitionOnNoMotion[3]     = "Ready";
   stateWaitForTimeout[3]           = false;
   stateScaleAnimation[3]           = false;
   stateScaleAnimationFP[3]         = false;
   stateSequenceTransitionIn[3]     = true;
   stateSequenceTransitionOut[3]    = true;
   stateTransitionOnNoAmmo[3]       = "NoAmmo";
   stateTransitionOnTriggerDown[3]  = "Fire";
   stateSequence[3]                 = "idle_fidget1";
   stateTimeoutValue[3]             = 6.0;

   // Ready to fire with player moving
   stateName[4]                     = "ReadyMotion";
   stateTransitionGeneric0In[4]     = "SprintEnter";
   stateTransitionOnNoMotion[4]     = "Ready";
   stateWaitForTimeout[4]           = false;
   stateScaleAnimation[4]           = false;
   stateScaleAnimationFP[4]         = false;
   stateSequenceTransitionIn[4]     = true;
   stateSequenceTransitionOut[4]    = true;
   stateTransitionOnNoAmmo[4]       = "NoAmmo";
   stateTransitionOnTriggerDown[4]  = "Fire";
   stateSequence[4]                 = "Run";

   // Fire the weapon. Calls the fire script which does
   // the actual work.
   stateName[5]                     = "Fire";
   stateTransitionGeneric0In[5]     = "SprintEnter";
   stateTransitionOnTimeout[5]      = "Reload";
   stateTimeoutValue[5]             = 0.4;
   stateWaitForTimeout[5]           = true;
   stateFire[5]                     = true;
   stateRecoil[5]                   = "light_recoil";
   stateAllowImageChange[5]         = false;
   stateSequence[5]                 = "Fire";
   stateScaleAnimation[5]           = true;
   stateSequenceNeverTransition[5]  = true;
   stateSequenceRandomFlash[5]      = true;        // use muzzle flash sequence
   stateScript[5]                   = "onFire";
   //stateEmitter[5]                  = RyderFireSmokeEmitter;
   //stateEmitterTime[5]              = 0.025;
   stateEjectShell[5]               = true;
   stateSound[5]                    = RLFireSound;

   // Put another round in the chamber
   stateName[6]                     = "Reload";
   stateTransitionGeneric0In[6]     = "SprintEnter";
   stateTransitionOnNoAmmo[6]       = "NoAmmo";
   stateTransitionOnTimeout[6]      = "Ready";
   stateWaitForTimeout[6]           = false;
   stateTimeoutValue[6]             = 3.3;
   stateAllowImageChange[6]         = false;
   stateSequence[6]                 = "reload";
   stateSound[6]                    = RLReloadSound;

   // No ammo in the weapon, just idle until something
   // shows up. Play the dry fire sound if the trigger is
   // pulled.
   stateName[7]                     = "NoAmmo";
   stateTransitionGeneric0In[7]     = "SprintEnter";
   stateTransitionOnMotion[7]       = "NoAmmoMotion";
   stateTransitionOnAmmo[7]         = "ReloadClip";
   stateTimeoutValue[7]             = 0.1;   // Slight pause to allow script to run when trigger is still held down from Fire state
   stateScript[7]                   = "onClipEmpty";
   stateTransitionOnTriggerDown[7]  = "DryFire";
   stateSequence[7]                 = "idle";
   stateScaleAnimation[7]           = false;
   stateScaleAnimationFP[7]         = false;
   
   stateName[8]                     = "NoAmmoMotion";
   stateTransitionGeneric0In[8]     = "SprintEnter";
   stateTransitionOnNoMotion[8]     = "NoAmmo";
   stateWaitForTimeout[8]           = false;
   stateScaleAnimation[8]           = false;
   stateScaleAnimationFP[8]         = false;
   stateSequenceTransitionIn[8]     = true;
   stateSequenceTransitionOut[8]    = true;
   stateTransitionOnAmmo[8]         = "ReloadClip";
   stateTransitionOnTriggerDown[8]  = "DryFire";
   stateSequence[8]                 = "Run";

   // No ammo dry fire
   stateName[9]                     = "DryFire";
   stateTransitionGeneric0In[9]     = "SprintEnter";
   stateTransitionOnAmmo[9]         = "ReloadClip";
   stateWaitForTimeout[9]           = "0";
   stateTimeoutValue[9]             = 1.0;
   stateTransitionOnTimeout[9]      = "NoAmmo";
   stateScript[9]                   = "onDryFire";

   // Play the reload clip animation
   stateName[10]                     = "ReloadClip";
   stateTransitionGeneric0In[10]     = "SprintEnter";
   stateTransitionOnTimeout[10]      = "Ready";
   stateWaitForTimeout[10]           = true;
   stateTimeoutValue[10]             = 3.0;
   stateReload[10]                   = true;
   stateSequence[10]                 = "reload";
   stateShapeSequence[10]            = "Reload";
   stateScaleShapeSequence[10]       = true;
   stateSound[10]                    = RLReloadSound;

   // Start Sprinting
   stateName[11]                    = "SprintEnter";
   stateTransitionGeneric0Out[11]   = "SprintExit";
   stateTransitionOnTimeout[11]     = "Sprinting";
   stateWaitForTimeout[11]          = false;
   stateTimeoutValue[11]            = 0.5;
   stateWaitForTimeout[11]          = false;
   stateScaleAnimation[11]          = false;
   stateScaleAnimationFP[11]        = false;
   stateSequenceTransitionIn[11]    = true;
   stateSequenceTransitionOut[11]   = true;
   stateAllowImageChange[11]        = false;
   stateSequence[11]                = "sprint";

   // Sprinting
   stateName[12]                    = "Sprinting";
   stateTransitionGeneric0Out[12]   = "SprintExit";
   stateWaitForTimeout[12]          = false;
   stateScaleAnimation[12]          = false;
   stateScaleAnimationFP[12]        = false;
   stateSequenceTransitionIn[12]    = true;
   stateSequenceTransitionOut[12]   = true;
   stateAllowImageChange[12]        = false;
   stateSequence[12]                = "sprint";
   
   // Stop Sprinting
   stateName[13]                    = "SprintExit";
   stateTransitionGeneric0In[13]    = "SprintEnter";
   stateTransitionOnTimeout[13]     = "Ready";
   stateWaitForTimeout[13]          = false;
   stateTimeoutValue[13]            = 0.5;
   stateSequenceTransitionIn[13]    = true;
   stateSequenceTransitionOut[13]   = true;
   stateAllowImageChange[13]        = false;
   stateSequence[13]                = "sprint";
};

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowWeapon("Soldier");
SmsInv.AddWeapon(RocketLauncher, "Rocket Launcher", 1);

//SmsInv.AllowClip("armor\tSoldier\t3");
//SmsInv.AddClip(RocketLauncherClip, "Rockets", 3);

SmsInv.AllowAmmo("armor\tSoldier\t6");
SmsInv.AddAmmo(RocketLauncherAmmo, 6);
