//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function initializeBase() {
	echo(" % - Initializing Base Editor");
	// Load Custom Editors
	loadDirectory( expandFilename( "./canvas" ) );
	loadDirectory( expandFilename( "./menuBar" ) );
	loadDirectory( expandFilename( "./utils" ) );
}

function destroyBase() {
}
