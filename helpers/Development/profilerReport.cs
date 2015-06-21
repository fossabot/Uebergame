//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Profiler dumping system
//==============================================================================
//==============================================================================
//Record profile while profile bind is pressed
function doProfile(%val) {
	if (%val) {
		// key down -- start profile	
		note("Starting profile session...");
		profilerReset();
		profilerEnable(true);
	} else {
		// key up -- finish off profile
		note("Ending profile session...");
		%file = "data/profiler/quickLog_" @ getSimTime() @ ".txt";
		profilerDumpToFile(%file);
		%file = "data/profiler/"@$Cfg::Game::SessionNumber@"/prof_" @$Cfg::Game::SessionNumber@"_"@ getRealTime() @ ".txt";
		profilerDumpToFile(%file);
		profilerEnable(false);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//Record profile until it's set to off
function toggleProfile(%val) {
	if (%val) {
		if($ProfilerActive) {
			$ProfilerActive = false;
			// key up -- finish off profile
			note("Ending a long profile session...");
			%file = "data/profiler/fullLog_" @ getSimTime() @ ".txt";
			profilerDumpToFile(%file);
			%file = "data/profiler/"@$Cfg::Game::SessionNumber@"/profExtent_" @$Cfg::Game::SessionNumber@"_"@ getRealTime() @ ".txt";
			profilerDumpToFile(%file);
			profilerEnable(false);
		} else {
			$ProfilerActive = true;
			// key down -- start profile
			note("Starting a long profile session...");
			profilerReset();
			profilerEnable(true);
		}
	}
}
//------------------------------------------------------------------------------



//==============================================================================
// DISPLAY DEVELOPMENT INFORMATIONS HELPERS
//------------------------------------------------------------------------------



/// Shortcut for typing dbgSetParameters with the default values torsion uses.
function dbgTorsion() {
	dbgSetParameters( 6060, "password", false );
}

/// Reset the input state to a default of all-keys-up.
/// A helpful remedy for when Torque misses a button up event do to your breakpoints
/// and can't stop shooting / jumping / strafing.
function mvReset() {
	for ( %i = 0; %i < 6; %i++ )
		setVariable( "mvTriggerCount" @ %i, 0 );
	$mvUpAction = 0;
	$mvDownAction = 0;
	$mvLeftAction = 0;
	$mvRightAction = 0;
	// There are others.
}

//------------------------
//Display all value of object in console
function logObj(%obj,%prefix, %dynamicOnly) {
	if (!%dynamicOnly) {
		%count = %obj.getFieldCount();
		for(%i=0; %i<%count; %i++) {
			%field = %obj.getField(%i);
			%value = %obj.getFieldValue(%field);
		}
	}
	%count = %obj.getDynamicFieldCount();
	for(%i=0; %i<%count; %i++) {
		%field = %obj.getDynamicField(%i);
		%value = %obj.getFieldValue(%field);
	}
}
function colorTest() {
	msgAll("\c00\c11\c22\c33\c44\c55\c66\c77\c88\c99");
}
//----------------------------------------------------------------------------
// Debug commands
//----------------------------------------------------------------------------

function setLag( %msDelay, %packetLossPercent ) {
	netSimulateLag( %msDelay, %packetLossPercent );

}
function netSimulateLag( %msDelay, %packetLossPercent ) {
	if ( %packetLossPercent $= "" )
		%packetLossPercent = 0;
	commandToServer( 'NetSimulateLag', %msDelay, %packetLossPercent );
}

function serverCmdNetSimulateLag( %client, %msDelay, %packetLossPercent ) {
	//if ( %client.isAdmin )
	%client.setSimulatedNetParams( %packetLossPercent / 100.0, %msDelay );
}

