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

datablock StaticShapeData(CaptureSwitch)
{
   catagory = "Objectives";
   shapeFile = "~/data/shapes/objectives/tower/team1/fusionTower.dts";
   emap = true;
   computeCRC = true;

   cameraMaxDist = 10;
   cameraMinDist = 0.2;
   cameraDefaultFov = 90;
   cameraMinFov = 5;
   cameraMaxFov = 120;
   firstPersonOnly = false;
   useEyePoint = false;
   observeThroughObject = false;

   isInvincible = true;
   needsNoPower = true;

   nameTag = 'Switch';
   // Radius damage
   canImpulse = false;
};

function CaptureSwitch::objectiveInit(%data, %obj)
{
   // add this switch to missioncleanup
   %switchSet = nameToID("MissionCleanup/Switches");
   if ( %switchSet <= 0 )
   {
      %switchSet = new SimSet("Switches");
      MissionCleanup.add(%switchSet);
   }
   %switchSet.add(%obj);
}

function CaptureSwitch::onCollision(%data, %obj, %colObj)
{
   if ( isObject( Game ) && %colObj.getType() & ( $TypeMasks::PlayerObjectType ) )
      if ( %colObj.getState() !$= "Dead" )
         Game.playerTouchSwitch(%obj, %colObj);
}
