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

// Constants for referencing video resolution preferences
$WORD::RES_X = 0;
$WORD::RES_Y = 1;
$WORD::FULLSCREEN = 2;
$WORD::BITDEPTH = 3;
$WORD::REFRESH = 4;
$WORD::AA = 5;

//---------------------------------------------------------------------------------------------
// CorePackage
// Adds functionality for this mod to some standard functions.
//---------------------------------------------------------------------------------------------
package mypackage{
	
//---------------------------------------------------------------------------------------------
// onStart
// Called when the engine is starting up. Initializes this mod.
//---------------------------------------------------------------------------------------------
	function onStart()
	{

	}

//---------------------------------------------------------------------------------------------
// onExit
// Called when the engine is shutting down. Shutdowns this mod.
//---------------------------------------------------------------------------------------------
	function onExit()
	{   

	}

	function loadKeybindings()
	{
	   $keybindCount = 0;
	   // Load up the active projects keybinds.
	   if(isFunction("setupKeybinds"))
		  setupKeybinds();
	}

//---------------------------------------------------------------------------------------------
// displayHelp
// Prints the command line options available for this mod.
//---------------------------------------------------------------------------------------------
	function displayHelp() {

	}

//---------------------------------------------------------------------------------------------
// parseArgs
// Parses the command line arguments and processes those valid for this mod.
//---------------------------------------------------------------------------------------------
	function parseArgs()
	{
	   
	}

};

activatePackage(mypackage);

