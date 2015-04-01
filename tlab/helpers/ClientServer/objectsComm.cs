//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//-----------------------------------------------------------------------------
// Load up our main GUI which lets us see the game.

//============================================================================
//Taking screenshot
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
//----------------------------------------------------------------------------
//Load the helpers on execution
