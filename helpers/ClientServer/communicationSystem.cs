//==============================================================================
// GameLab Helpers -> Client <-> Server Communication
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Functions that deal with sending information between client and server
//==============================================================================

//==============================================================================
// commandToClient/commandToServer automated system
//==============================================================================
//==============================================================================
// Send Command to all client
function sendCmd( %client, %command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9) {
	cmdClient(%client,%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9);
}
//------------------------------------------------------------------------------
//==============================================================================
// Send Command to all client
function sendCmdLocal(  %command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9) {
	cmdClient(LocalClientConnection,%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9);
}
//------------------------------------------------------------------------------
//==============================================================================
// Send Command to all client
function sendCmdAll( %command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9) {
	foreach(%client in ClientGroup) {
		cmdClient(%client,%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Send Command to all except specified client
function sendCmdAllExcept(%cl, %command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9) {
	foreach(%client in ClientGroup) {
		if(%cl != %client)
			cmdClient(%client,%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Send Command to single client
function cmdClient(%client,%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9) {
	commandToClient( %client,'GameCommand',%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9 );
}
//------------------------------------------------------------------------------

//==============================================================================
// Client-Side Send Game Command to server
function cmdServer(%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9,%a10) {

	commandToServer('GameCommand',%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9,%a10);
}
//------------------------------------------------------------------------------

//==============================================================================
// Client message/command reception
//==============================================================================
//==============================================================================
// Server-Side Game Command received
function serverCmdGameCommand(%client,%command,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10) {

	if (%command!$="") {
		%str = %command@"(%client,%a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9,%a10);";
		//There's a command to execute, prepare it!
		eval(%str);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// GameCommand reception -> Special command to be call on client
function clientCmdGameCommand( %command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9) {
	loge("clientCmdGameCommand(%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9)",%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9);
	//Check if we have a client side command
	if (%command!$="") {
		%str = %command@"(%a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9);";
		//There's a command to execute, prepare it!
		eval(%str);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Server command reception
//==============================================================================

//==============================================================================
//Send Game Message Area to all client
function msgAll(%msg,%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9) {
	foreach(%client in ClientGroup) {
		msgClient(%client,%msg,%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//Send Game Message Area to single client
function msgClient(%client, %msg, %command, %a1, %a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9) {
	commandToClient( %client, 'GameMessage', %msg, %command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9 );
}
//------------------------------------------------------------------------------
//==============================================================================
//Send Game Message Area to all except specified client
function msgAllExcept(%client,  %msg, %command, %a1, %a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9) {
	foreach(%cl in ClientGroup) {
		if(%cl != %client)
			msgClient(%client,%msg,%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//GameMessage reception -> Special message AND/OR command sent by server
function clientCmdGameMessage(%msg, %command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9) {
	loge("clientCmdGameMessage(%msg,%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9)",%msg,%command, %a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9);
	//Only show the message if exist
	if (%msg !$="")
		onGameMessage(%msg);
	//Check if we have a client side command
	if (%command!$="") {
		%str = %command@"(%a1, %a2, %a3, %a4,%a5, %a6,%a7, %a8,%a9);";
		//There's a command to execute, prepare it!
		eval(%str);
	}
}
//------------------------------------------------------------------------------


