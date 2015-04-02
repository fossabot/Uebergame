//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

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
//----------------------------------------------------------------------------
/// A helper function which will return the ghosted client object
/// from a server object when connected to a local server.
function serverToClientObject( %serverObject ) {
	assert( isObject( LocalClientConnection ), "serverToClientObject() - No local client connection found!" );
	assert( isObject( ServerConnection ), "serverToClientObject() - No server connection found!" );
	%ghostId = LocalClientConnection.getGhostId( %serverObject );
	if ( %ghostId == -1 )
		return 0;
	return ServerConnection.resolveGhostID( %ghostId );
}
