//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// a simple function for rotations to vectors calculations
function echoTransformVector(%obj) {
	%transform = %obj.getTransform();
	echo("Transform: " @ %transform);

	%pos = getWords(%transform, 0, 2);
	echo("Position: " @ %pos);

	%aa = getWords(%transform, 3, 6);

	echo("Angular Axis: " @ %aa);

	%tmat = VectorOrthoBasis(%aa);
	echo("Transform Matrix: " @ %tmat);

	%rv = getWords(%tmat, 0, 2);
	echo("Right Vector: " @ %rv);

	%fv = getWords(%tmat, 3, 5);
	echo("Forward Vector: " @ %fv);

	%uv = getWords(%tmat, 6, 8);
	echo("Up Vector: " @ %uv);
}
//------------------------------------------------------------------------------

/// Shortcut for typing dbgSetParameters with the default values torsion uses.
function dbgTorsion()
{
   dbgSetParameters( 6060, "password", false );
}

/// Reset the input state to a default of all-keys-up.
/// A helpful remedy for when Torque misses a button up event do to your breakpoints
/// and can't stop shooting / jumping / strafing.
function mvReset()
{
   for ( %i = 0; %i < 6; %i++ )
      setVariable( "mvTriggerCount" @ %i, 0 );
      
   $mvUpAction = 0;
   $mvDownAction = 0;
   $mvLeftAction = 0;
   $mvRightAction = 0;
   
   // There are others.
}


function colorTest() {
	msgAll("\c00\c11\c22\c33\c44\c55\c66\c77\c88\c99");
}