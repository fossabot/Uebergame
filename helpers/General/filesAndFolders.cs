//==============================================================================
// Boost! -> Main functions used on game launch
// Copyright NordikLab Studio, 2013
//==============================================================================


//==============================================================================
// Get the World coords from a screen pos (X Y Depth)
function execPattern(%pattern,%noDso,%preCmd,%postCmd) {
	//Start by loading all bind functions
	for(%file = findFirstFile(%pattern); %file !$= ""; %file = findNextFile(%pattern)) {
		exec( %file );
	}

	%isScript = strstr(%pattern,".cs");

	if (%noDso || %isScript $= "-1" )
		return;

	%pattern = strreplace(%pattern,".cs",".cs.dso");

	for(%file = findFirstFile(%pattern); %file !$= ""; %file = findNextFile(%pattern)) {
		devLog("Should be .dso only",%file);
		exec( %file );
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// FileObject Helpers
//==============================================================================

//==============================================================================
// Get the World coords from a screen pos (X Y Depth)
function writeObjectToFile(%obj,%file,%prepend) {
	%fileWrite = new FileObject();
	%result = %fileWrite.OpenForWrite(%file);

	if (%result && isObject(%obj)) {
		if (%prepend !$="")
			%fileWrite.writeObject(%obj,%prepend);
		else
			%fileWrite.writeObject(%obj);
	}

	%fileWrite.close();
	%fileWrite.delete();
	return %result;
}
//------------------------------------------------------------------------------
//==============================================================================
// Get the World coords from a screen pos (X Y Depth)
function appendObjectToFile(%obj,%file,%prepend) {
	%fileWrite = new FileObject();
	%result = %fileWrite.OpenForAppend(%file);

	if (%result && isObject(%obj)) {
		if (%prepend !$="")
			%fileWrite.writeObject(%obj,%prepend);
		else
			%fileWrite.writeObject(%obj);
	}

	%fileWrite.close();
	%fileWrite.delete();
	return %result;
}
//------------------------------------------------------------------------------
//==============================================================================
function getLoadFilename(%filespec, %callback, %currentFile) {
	%dlg = new OpenFileDialog() {
		Filters = %filespec;
		DefaultFile = %currentFile;
		ChangePath = false;
		MustExist = true;
		MultipleFiles = false;
	};

	if ( filePath( %currentFile ) !$= "" )
		%dlg.DefaultPath = filePath(%currentFile);

	if ( %dlg.Execute() ) {
		eval(%callback @ "(\"" @ %dlg.FileName @ "\");");
		$Tools::FileDialogs::LastFilePath = filePath( %dlg.FileName );
	}

	%dlg.delete();
}
//------------------------------------------------------------------------------
//==============================================================================
function getSaveFilename( %filespec, %callback, %currentFile, %overwrite ) {
	if( %overwrite $= "" )
		%overwrite = true;

	%dlg = new SaveFileDialog() {
		Filters = %filespec;
		DefaultFile = %currentFile;
		ChangePath = false;
		OverwritePrompt = %overwrite;
	};

	if( filePath( %currentFile ) !$= "" )
		%dlg.DefaultPath = filePath( %currentFile );
	else
		%dlg.DefaultPath = getMainDotCSDir();

	if( %dlg.Execute() ) {
		%filename = %dlg.FileName;
		eval( %callback @ "(\"" @ %filename @ "\");" );
	}

	%dlg.delete();
}
//==============================================================================
$TLab_ImageExtensions = "png dds jpg jpeg bmp tga";
function isImageFile(%file) {
	%file = strreplace(%file,"\"","");

	if (isFile(%file))
		return true;

	%extId = 0;
	%extension = getWord($TLab_ImageExtensions,%extId);

	while(%extension !$= "") {
		%testFile = %file @"."@%extension;
		devLog("Testing image file",%testFile,"IsFile",isFile(%testFile));

		if (isFile(%testFile))
			return true;

		%extId++;
		%extension = getWord($TLab_ImageExtensions,%extId);
	}

	return false;
}
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
function getFolderName( %filter, %callback,%startFolder,%title ) {
	if (%title $= "")
		%title = "Select Export Folder";

	%dlg = new OpenFolderDialog() {
		Title = %title;
		Filters = %filter;
		DefaultFile = %startFolder;
		ChangePath = false;
		MustExist = true;
		MultipleFiles = false;
	};

	if(%dlg.Execute()) {
		%relativePath = makeRelativePath(%dlg.FileName);
		%cb = %callback@"(\""@%relativePath@"/\");";
		devLog("Callback is:",%cb,"Relative:",makeRelativePath(%dlg.FileName));
		eval(%cb);
	}

	%dlg.delete();
}

//--------------------------------------------------------------------------
// returns the text in a file with "\n" at the end of each line
//--------------------------------------------------------------------------

function loadFileText( %file) {
	%fo = new FileObject();
	%fo.openForRead(%file);
	%text = "";

	while(!%fo.isEOF()) {
		%text = %text @ %fo.readLine();

		if (!%fo.isEOF()) %text = %text @ "\n";
	}

	%fo.delete();
	return %text;
}

//==============================================================================
// Get the World coords from a screen pos (X Y Depth)
function getFileFolder(%file,%depth) {
	if (%depth $= "")
		%depth = 0;

	%folder = filePath(%file);
	%folder = strReplace(%folder,"/"," ");
	%folderId = getWordCount(%folder) - 1 - %depth;
	%lastFolder = getWord(%folder,%folderId);
	return %lastFolder;
}
//------------------------------------------------------------------------------


//==============================================================================
// Get the World coords from a screen pos (X Y Depth)
function getFullPath(%path,%dir) {
	if (%path $="") {
		error("Invalid path");
		return;
	}

	if (%dir $="") {
		%return = makeFullPath( %path );
	} else
		%return = makeFullPath( %path,%dir );

	return %return;
}
//------------------------------------------------------------------------------


//==============================================================================
// Get the World coords from a screen pos (X Y Depth)
function getMultiExtensionFileList(%path,%extensions) {
	foreach$(%ext in %extensions) {
		%pattern = %path@"/*."@%ext;

		for(%file = findFirstFile(%pattern); %file !$= ""; %file = findNextFile(%pattern)) {
			%relFile = strreplace(%file,%path,"");
			%fileList = strAddRecord(%fileList,%relFile);
		}
	}

	return %fileList;
}
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
// Check if a script file exists, compiled or not.
function isScriptFile(%path) {
	if( isFile(%path @ ".dso") || isFile(%path) )
		return true;

	return false;
}