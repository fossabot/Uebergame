//--------------------------------------------------------------------------
// Sounds
//--------------------------------------------------------------------------

datablock SFXProfile(GLFireSound)
{
   filename = "art/sound/weapons/wpn_lurker_grenadelaunch";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(GLReloadSound)
{
   filename = "art/sound/weapons/wpn_lurker_reload";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(GLSwitchinSound)
{
   filename = "art/sound/weapons/wpn_lurker_switchin";
   description = AudioClose3D;
   preload = true;
};

//-----------------------------------------------------------------------------
// Tracer particles
//-----------------------------------------------------------------------------

datablock ParticleData(UWGrenadeTrailParticle)
{
   dragCoefficient      = 0.0;
   gravityCoefficient   = -0.25;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 1500;
   lifetimeVarianceMS   = 600;
   useInvAlpha          = false;

   textureName = "art/particles/bubble.png";
   animTexName[0] = "art/particles/bubble.png";

   spinRandomMin        = -100.0;
   spinRandomMax        =  100.0;

   colors[0]     = "0.7 0.8 1.0 0.4";
   colors[1]     = "0.7 0.8 1.0 0.4";
   colors[2]     = "0.7 0.8 1.0 0.0";

   sizes[0]      = 0.5;
   sizes[1]      = 0.5;
   sizes[2]      = 0.5;

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(UWGrenadeTrailEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;
   ejectionVelocity = 1.0;
   ejectionOffset   = 0.1;
   velocityVariance = 0.5;
   thetaMin         = 0;
   thetaMax         = 80;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   particles = "UWGrenadeTrailParticle";
};

datablock ParticleData(GrenadeTrailParticle)
{
   dragCoeffiecient     = 0;
   gravityCoefficient   = "-0.021978";   // rises slowly
   inheritedVelFactor   = "0.0998043";

   lifetimeMS           = 750;  // lasts 2 second
   lifetimeVarianceMS   = 100;   // ...more or less

   textureName = "art/particles/smokeThick.png";
   animTexName[0] = "art/particles/smokeThick.png";

   useInvAlpha = 0;
   spinRandomMin = -90.0;
   spinRandomMax = 90.0;

   colors[0]     = "0.897638 0.897638 0.897638 1";
   colors[1]     = "0.582677 0.582677 0.582677 1";
   colors[2]     = "0.393701 0.393701 0.393701 0";

   sizes[0]      = "0.25";
   sizes[1]      = "0.5";
   sizes[2]      = "1";

   times[0]      = 0.0;
   times[1]      = "0.498039";
   times[2]      = 1.0;
};

datablock ParticleEmitterData(GrenadeTrailEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 5;

   ejectionVelocity = 1;
   velocityVariance = 0.25;

   thetaMin         = 0.0;
   thetaMax         = 50.0;  
   phiReferenceVel = 90;

   particles = "GrenadeTrailParticle";
   ejectionOffset = "0.1";
   blendStyle = "NORMAL";
   orientParticles = "0";
   softParticles = "1";
};

//-----------------------------------------------------------------------------
// Light
//-----------------------------------------------------------------------------

datablock LightDescription(GrenadeLauncherLightDesc)
{
   color = "0.8 0.8 0.75";
   range = 2.0;
   brightness = 2.0;
   animationType = NullLightAnim;
};

//-----------------------------------------------------------------------------
// Shell ejected during reload.
//-----------------------------------------------------------------------------

datablock DebrisData(GrenadeShell)
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

datablock ProjectileData( GrenadeProjectile )
{
   projectileShapeName = "art/shapes/weapons/shared/rocket.dts";
   //sound               = "";
   directDamage        = 0;
   radiusDamage        = 50;
   damageRadius        = 10;
   areaImpulse         = 1;
   impactForce         = 5;

   damageType          = $DamageType::GrenadeLauncher;
   areaImpulse    = 1500;

   explosion           = GrenadeExplosion;
   waterExplosion      = UnderwaterGrenadeExplosion;
   decal               = ScorchBigDecal;

   particleEmitter     = GrenadeTrailEmitter;
   particleWaterEmitter = UWGrenadeTrailEmitter;

   Splash              = GrenadeSplash;
   muzzleVelocity      = 50;
   velInheritFactor    = 0.5;

   armingDelay         = 1000; // How long it should not detonate on impact
   lifetime            = 8000; // How long the projectile should exist before deleting itself
   fadeDelay           = 4000; // Brief Amount of time, in milliseconds, before the projectile begins to fade out.

   bounceElasticity    = 0.4;
   bounceFriction      = 0.3;
   isBallistic         = 1; // Causes the projectile to be affected by gravity "arc".
   gravityMod          = 1;

   lightDesc           = "GrenadeLauncherLightDesc";
};

//-----------------------------------------------------------------------------
// Ammo Item
//-----------------------------------------------------------------------------
datablock ItemData(GrenadeLauncherClip : DefaultClip)
{
   shapeFile = "art/shapes/weapons/grenade/grenade.dae";
   pickUpName = 'Grenade clip';
};

datablock ItemData(GrenadeLauncherAmmo : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/grenade/grenade.dae";
   pickUpName = 'Grenades';
   //clip = GrenadeLauncherClip;
};

//--------------------------------------------------------------------------
// Weapon Item.  This is the item that exists in the world, i.e. when it's
// been dropped, thrown or is acting as re-spawnable item.  When the weapon
// is mounted onto a shape, the SoldierWeaponImage is used.
//-----------------------------------------------------------------------------
datablock ItemData(GrenadeLauncher : DefaultWeapon)
{
   shapeFile = "art/shapes/weapons/Lurker/TP_Lurker.DAE";
   pickUpName = 'Grenade Launcher';
   image = GrenadeLauncherImage;
};

datablock ShapeBaseImageData(LurkerGrenadeLauncherImage)
{
   // Basic Item properties
   shapeFile = "art/shapes/weapons/Lurker/TP_Lurker.DAE";
   shapeFileFP = "art/shapes/weapons/Lurker/FP_Lurker.DAE";
   emap = true;

   imageAnimPrefix = "Rifle";
   imageAnimPrefixFP = "Rifle";

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   firstPerson = true;
   useEyeNode = true;
   animateOnServer = true;

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   class = "WeaponImage";
   className = "WeaponImage";

   // Projectiles and Ammo.
   item = LurkerGrenadeLauncher;
   ammo = LurkerGrenadeAmmo;

   projectile = GrenadeLauncherProjectile;
   projectileType = Projectile;
   projectileSpread = "0.02";

   // Weapon lights up while firing
   lightType = "WeaponFireLight";
   lightColor = "0.992126 0.968504 0.708661 1";
   lightRadius = "4";
   lightDuration = "100";
   lightBrightness = 2;

   // Shake camera while firing.
   shakeCamera = true;
   camShakeFreq = "2 2 2";
   camShakeAmp = "7 7 7";
   camShakeDuration = "0.5";
   camShakeRadius = "2";

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
   stateSound[1]                    = LurkerSwitchinSound;

   // Ready to fire, just waiting for the trigger
   stateName[2]                     = "Ready";
   stateTransitionGeneric0In[2]     = "SprintEnter";
   stateTransitionOnMotion[2]       = "ReadyMotion";
   stateTransitionOnTimeout[2]      = "ReadyFidget";
   stateTimeoutValue[2]             = 10;
   stateWaitForTimeout[2]           = false;
   stateScaleAnimation[2]           = false;
   stateScaleAnimationFP[2]         = false;
   stateTransitionOnNoAmmo[2]       = "NoAmmo";
   stateTransitionOnTriggerDown[2]  = "Fire";
   stateSequence[2]                 = "idle";

   // Same as Ready state but plays a fidget sequence
   stateName[3]                     = "ReadyFidget";
   stateTransitionGeneric0In[3]     = "SprintEnter";
   stateTransitionOnMotion[3]       = "ReadyMotion";
   stateTransitionOnTimeout[3]      = "Ready";
   stateTimeoutValue[3]             = 6;
   stateWaitForTimeout[3]           = false;
   stateTransitionOnNoAmmo[3]       = "NoAmmo";
   stateTransitionOnTriggerDown[3]  = "Fire";
   stateSequence[3]                 = "idle_fidget1";
   stateSound[3]                    = LurkerIdleSound;

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
   stateSequence[4]                 = "run";

   // Fire the weapon. Calls the fire script which does
   // the actual work.
   stateName[5]                     = "Fire";
   stateTransitionGeneric0In[5]     = "SprintEnter";
   stateTransitionOnTimeout[5]      = "NewRound";
   stateTimeoutValue[5]             = 1.2;
   stateFire[5]                     = true;
   stateRecoil[5]                   = "";
   stateAllowImageChange[5]         = false;
   stateSequence[5]                 = "fire_alt";
   stateScaleAnimation[5]           = true;
   stateSequenceNeverTransition[5]  = true;
   stateSequenceRandomFlash[5]      = true;        // use muzzle flash sequence
   stateScript[5]                   = "onFire";
   stateSound[5]                    = LurkerGrenadeFireSound;
   stateEmitter[5]                  = GunFireSmokeEmitter;
   stateEmitterTime[5]              = 0.025;
   stateEjectShell[5]               = true;

   // Put another round into the chamber
   stateName[6]                     = "NewRound";
   stateTransitionGeneric0In[6]     = "SprintEnter";
   stateTransitionOnNoAmmo[6]       = "NoAmmo";
   stateTransitionOnTimeout[6]      = "Ready";
   stateWaitForTimeout[6]           = "0";
   stateTimeoutValue[6]             = 0.05;
   stateAllowImageChange[6]         = false;

   // No ammo in the weapon, just idle until something
   // shows up. Play the dry fire sound if the trigger is
   // pulled.
   stateName[7]                     = "NoAmmo";
   stateTransitionGeneric0In[7]     = "SprintEnter";
   stateTransitionOnMotion[7]       = "NoAmmoReadyMotion";
   stateTransitionOnAmmo[7]         = "ReloadClip";
   stateTimeoutValue[7]             = 0.1;   // Slight pause to allow script to run when trigger is still held down from Fire state
   stateScript[7]                   = "onClipEmpty";
   stateSequence[7]                 = "idle";
   stateScaleAnimation[7]           = false;
   stateScaleAnimationFP[7]         = false;
   stateTransitionOnTriggerDown[7]  = "DryFire";
   
   stateName[8]                     = "NoAmmoReadyMotion";
   stateTransitionGeneric0In[8]     = "SprintEnter";
   stateTransitionOnNoMotion[8]     = "NoAmmo";
   stateWaitForTimeout[8]           = false;
   stateScaleAnimation[8]           = false;
   stateScaleAnimationFP[8]         = false;
   stateSequenceTransitionIn[8]     = true;
   stateSequenceTransitionOut[8]    = true;
   stateTransitionOnAmmo[8]         = "ReloadClip";
   stateTransitionOnTriggerDown[8]  = "DryFire";
   stateSequence[8]                 = "run";

   // No ammo dry fire
   stateName[9]                     = "DryFire";
   stateTransitionGeneric0In[9]     = "SprintEnter";
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
SmsInv.AddWeapon(GrenadeLauncher, "Grenade Launcher", 1);

//SmsInv.AllowClip("armor\tSoldier\t3");
//SmsInv.AddClip(GrenadeLauncherClip, "Grenade Belt", 3);

SmsInv.AllowAmmo("armor\tSoldier\t6");
SmsInv.AddAmmo(GrenadeLauncherAmmo, 6);
