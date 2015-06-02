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

//--------------------------------------------------------------------------
// Sounds
//--------------------------------------------------------------------------

/*datablock SFXProfile(BulletImpactSound)
{
   filename = "art/sound/explosions/SCARFIRE";
   description = BulletImpactDesc;
   preload = true;
};*/

/*datablock SFXProfile(BulletFlightSound)
{
   filename = "art/sound/explosions/SCARFIRE";
   description = AudioClose3D;
   preload = true;
};*/

datablock LightDescription( BulletProjectileLightDesc )
{
   color  = "0.56 0.36 0.26";
   range = 3.0;
};

//-----------------------------------------------------------------------------
// Tracer particles
//-----------------------------------------------------------------------------

datablock ParticleData(UWBulletTrailParticle)
{
   textureName = "art/particles/bubble";
   animTexName = "art/particles/bubble";

   gravityCoefficient = 0;
   inheritedVelFactor = 0;
   constantAcceleration = 0.0;
   lifetimeMS = 700;
   lifetimeVarianceMS = 150;
   useInvAlpha = true;
   spinRandomMin = -60;
   spinRandomMax = 60;
   spinSpeed = 1;

   colors[0] = "1 1 1 1";
   colors[1] = "1 1 1 0.496063";
   colors[2] = "1 1 1 0";
   colors[3] = "1 1 1 0";

   sizes[0] = "0.0976622";
   sizes[1] = "0.0976622";
   sizes[2] = "0";
   sizes[3] = "0";

   times[0] = 0.0;
   times[1] = "0.247059";
   times[2] = "1";
   times[3] = "1";

   dragCoefficient = "0.2";
};

datablock ParticleEmitterData(UWBulletTrailEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 1;
   velocityVariance = 0;
   thetaMin = 0.0;
   thetaMax = 5.0;
   phiReferenceVel = 90;
   particles = "UWBulletTrailParticle";
   ejectionOffset = "0.1";
   blendStyle = "ADDITIVE";
};

datablock ParticleData(BulletTrailParticle)
{
   textureName = "art/particles/dustParticle";
   animTexName = "art/particles/dustParticle";

   gravityCoefficient = 0;
   inheritedVelFactor = 0;
   constantAcceleration = 0.0;
   lifetimeMS = 700;
   lifetimeVarianceMS = 150;
   useInvAlpha = true;
   spinRandomMin = -60;
   spinRandomMax = 60;
   spinSpeed = 1;

   colors[0] = "1 1 0 1";
   colors[1] = "1 1 0 0.496063";
   colors[2] = "1 1 1 0";
   colors[3] = "1 1 1 0";

   sizes[0] = "0.0976622";
   sizes[1] = "0.0976622";
   sizes[2] = "0";
   sizes[3] = "0";

   times[0] = 0.0;
   times[1] = "0.247059";
   times[2] = "1";
   times[3] = "1";

   dragCoefficient = "0.2";
};

datablock ParticleEmitterData(BulletTrailEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 1;
   velocityVariance = 0;
   thetaMin = 0.0;
   thetaMax = 5.0;
   phiReferenceVel = 90;
   particles = "BulletTrailParticle";
   ejectionOffset = "0.1";
   blendStyle = "ADDITIVE";
};

//-----------------------------------------------------------------------------
// Splash particles
//-----------------------------------------------------------------------------
datablock ParticleData(BulletSplashParticle)
{
   textureName = "art/particles/droplet";
   dragCoefficient = 1;
   gravityCoefficient = 0.0;
   inheritedVelFactor = 0.2;
   constantAcceleration = -1.4;
   lifetimeMS = 300;
   lifetimeVarianceMS = 0;
   colors[0] = "0.7 0.8 1.0 1.0";
   colors[1] = "0.7 0.8 1.0 0.5";
   colors[2] = "0.7 0.8 1.0 0.0";
   sizes[0] = 0.05;
   sizes[1] = 0.2;
   sizes[2] = 0.2;
   times[0] = 0.0;
   times[1] = 0.5;
   times[2] = 1.0;
};

datablock ParticleEmitterData(BulletSplashEmitter)
{
   ejectionPeriodMS = 4;
   periodVarianceMS = 0;
   ejectionVelocity = 3;
   velocityVariance = 1.0;
   ejectionOffset = 0.0;
   thetaMin = 0;
   thetaMax = 50;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvances = false;
   orientParticles = true;
   lifetimeMS = 100;
   particles = "BulletSplashParticle";
};

datablock SplashData(BulletSplash)
{
   emitter[0] = BulletSplashEmitter;
};

//-----------------------------------------------------------------------------
// Explosion
//-----------------------------------------------------------------------------

datablock ParticleData(BulletDust)
{
   textureName          = "art/particles/impact";
   dragCoefficient      = 0;
   gravityCoefficient   = "-0.100122";
   windCoefficient      = 0;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 300;
   spinRandomMin = -180.0;
   spinRandomMax =  180.0;
   useInvAlpha   = true;

   colors[0]     = "0.496063 0.393701 0.299213 0.692913";
   colors[1]     = "0.692913 0.614173 0.535433 0.346457";
   colors[2]     = "0.897638 0.84252 0.795276 0";

   sizes[0]      = 1.0;
   sizes[1]      = 8;
   sizes[2]      = 12;

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
   animTexName = "art/particles/impact";
};

datablock ParticleEmitterData(BulletDustEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 8;
   ejectionVelocity = 3.1;
   velocityVariance = 0.8;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles = "BulletDust";
   blendStyle = "NORMAL";
};

datablock ParticleData(BulletImpactSmokeParticle)
{
   textureName = "art/particles/smoke";
   dragCoefficient = 0.0;
   gravityCoefficient = -0.2;
   inheritedVelFactor = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS = 1000;
   lifetimeVarianceMS = 200;
   useInvAlpha = true;
   spinRandomMin = -90.0;
   spinRandomMax = 90.0;
   colors[0] = "0.7 0.7 0.7 0.0";
   colors[1] = "0.7 0.7 0.7 0.4";
   colors[2] = "0.7 0.7 0.7 0.0";
   sizes[0] = 0.5;
   sizes[1] = 0.5;
   sizes[2] = 1.0;
   times[0] = 0.0;
   times[1] = 0.5;
   times[2] = 1.0;
};

datablock ParticleEmitterData(BulletImpactSmoke)
{
   ejectionPeriodMS = 8;
   periodVarianceMS = 1;
   ejectionVelocity = 1;
   velocityVariance = 0.5;
   ejectionOffset = 0;
   thetaMin = 0;
   thetaMax = 35;
   overrideAdvances = false;
   particles = "BulletImpactSmokeParticle";
   lifetimeMS = 50;
};

datablock ParticleData(BulletSparks)
{
   textureName = "art/particles/spark";
   dragCoefficient = 1;
   gravityCoefficient = 0;
   inheritedVelFactor = 0.2;
   constantAcceleration = 0;
   lifetimeMS = 500;
   lifetimeVarianceMS = 100;
   colors[0] = "0.56 0.36 0.26 1.0";
   colors[1] = "0.56 0.36 0.26 1.0";
   colors[2] = "1.0 0.36 0.26 0.0";
   sizes[0] = 0.6;
   sizes[1] = 0.2;
   sizes[2] = 0.1;
   times[0] = 0;
   times[1] = 0.2;
   times[2] = 1;
};

datablock ParticleEmitterData(BulletSparkEmitter)
{
   ejectionPeriodMS = 4;
   periodVarianceMS = 0;
   ejectionVelocity = 12;
   velocityVariance = 2;
   ejectionOffset = 0;
   thetaMin = 0;
   thetaMax = 50;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvances = false;
   orientParticles = true;
   lifetimeMS = 100;
   particles = "BulletSparks";
};

datablock ExplosionData(BulletExplosion)
{
   soundProfile = BulletImpactSound;
   lifeTimeMS = 65;

   // Volume particles
   particleEmitter = BulletDustEmitter;
   particleDensity = 3;
   particleRadius = 0.25;

   // Point emission
   emitter[0] = BulletImpactSmoke;
   emitter[1] = BulletSparkEmitter;
};

//-----------------------------------------------------------------------------
// Underwater Projectile Explosion
//-----------------------------------------------------------------------------

datablock ParticleData(BulletWaterMistParticle)
{
   textureName          = "art/particles/splatter";
   dragCoefficient      = 0;
   gravityCoefficient   = 0.0;
   windCoefficient      = 0;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 600;
   lifetimeVarianceMS   = 200;
   spinRandomMin        = -180.0;
   spinRandomMax        = 180.0;
   useInvAlpha          = true;

   colors[0] = "1.0 1.0 1.0 0.4";
   colors[1] = "1.0 1.0 1.0 0.2";
   colors[2] = "1.0 1.0 1.0 0.0";

   sizes[0]  = 1.0;
   sizes[1]  = 2.5;
   sizes[2]  = 4.0;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(BulletWaterMistEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 10;
   ejectionVelocity = 2.0;
   velocityVariance = 0.5;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   particles        = "BulletWaterMistParticle";
};

datablock ParticleData(BulletWaterSplashParticle)
{
   textureName        = "art/particles/droplet";
   dragCoefficient    = 0;
   gravityCoefficient = 3.5;
   lifetimeMS         = 650;
   lifetimeVarianceMS = 200;
   spinRandomMin      = -120.0;
   spinRandomMax      =  120.0;
   useInvAlpha        = false;
   
   colors[0] = "1.0 1.0 1.0 0.7";
   colors[1] = "1.0 1.0 1.0 0.35";
   colors[2] = "1.0 1.0 1.0 0.0";

   sizes[0]  = 0.25;
   sizes[1]  = 1.0;
   sizes[2]  = 1.0;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(BulletWaterSplashEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 10.0;
   velocityVariance = 6.0;
   thetaMin         = 0.0;
   thetaMax         = 18.0;
   orientParticles  = false;
   orientOnVelocity = false;
   particles        = "BulletWaterSplashParticle";
};

datablock ParticleData(BulletWaterBubblesParticle)
{
   textureName          = "art/particles/bubble";
   dragCoefficient      = 1;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0;
   lifetimeMS           = 250;
   lifetimeVarianceMS   = 100;

   colors[0] = "0.7 0.8 1.0 1.0";
   colors[1] = "0.7 0.8 1.0 0.5";
   colors[2] = "0.7 0.8 1.0 0.0";

   sizes[0]  = 1.0;
   sizes[1]  = 0.6;
   sizes[2]  = 0.3;

   times[0]  = 0;
   times[1]  = 0.5;
   times[2]  = 1;
};

datablock ParticleEmitterData(BulletWaterBubblesEmitter)
{
   ejectionPeriodMS = 4;
   periodVarianceMS = 0;
   ejectionVelocity = 8;
   velocityVariance = 2;
   ejectionOffset   = 0;
   thetaMin         = 0;
   thetaMax         = 80;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   orientParticles  = true;
   lifetimeMS       = 100;
   particles        = "BulletWaterBubblesParticle";
};

datablock ExplosionData(BulletWaterExplosion)
{
   soundProfile = BulletImpactSound;
   lifeTimeMS = 65;

   particleEmitter = BulletWaterMistEmitter;
   particleDensity = 8;
   particleRadius = 0.8;

   emitter[0] = BulletWaterSplashEmitter;
   emitter[1] = BulletWaterBubblesEmitter;
};

