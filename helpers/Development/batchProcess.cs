//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
function buildContentUseBatch() {
	//This script will create a batch file used to move only the content used to a folder
	%file =  "data/moveUsedContent.txt";
	%fileWrite = new FileObject();
	%result = %fileWrite.OpenForWrite(%file);


	//Start with the datablocks
	//Sample xcopy art\materials.cs xcopytest\art\
	//Or xcopy "art\materials.cs" "xcopytest3\art\"

	foreach( %obj in DatablockGroup ) {
		%invalid = false;
		if (%obj.textureName !$="" && %exportedFile[%obj.textureName] $= "") {
			%fileSrc = %obj.textureName;
			%fileConv = strreplace(%fileSrc,"/","\\");
			if (isFile(%fileSrc)) {
				%fileWrite.writeLine("xcopy "@%fileConv SPC "content\\"@%fileConv);
			} else if (isFile(%fileSrc@".png")) {
				%fileWrite.writeLine("xcopy "@%fileConv@".png" SPC "content\\"@%fileConv@".png");
			} else if (isFile(%fileSrc@".dds")) {
				%fileWrite.writeLine("xcopy "@%fileConv@".dds" SPC "content\\"@%fileConv@".dds");
			} else {
				%invalid = true;
				%fileWrite.writeLine("REM Problem with file:"@%fileSrc);
			}
			if (!%invalid)
				%exportedFile[%obj.textureName] = "Success";
		}

	}
	%fileWrite.close();
	%fileWrite.delete();
}
//------------------------------------------------------------------------------
