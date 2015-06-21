//==============================================================================
// Boost! -> GuiControl Functions Helpers
// Copyright NordikLab Studio, 2013
//==============================================================================
$HLab_ForestDataGen_Folder = "art/models/boostEnv/";
//==============================================================================
// Schedule global on-off - Used to limit output of fast logs
//==============================================================================
// *-OFF
//==============================================================================
// Get the Screen coords for a 3D position
function rebuildForestData() {
	if (!isObject(ForestBrushGroup))
		exec("art/forest/brushes.cs");

	ForestBrushGroup.deleteAllObjects();
	buildForestFromFolder("Trees");
	buildForestFromFolder("Bushes");
	buildForestFromFolder("Plants");
	buildForestFromFolder("Flowers");
	buildForestFromFolder("Grass");
	buildForestFromFolder("Rocks");
	buildForestFromFolder("Fields");
}
//------------------------------------------------------------------------------
function buildForestDataFromBaseFolder(%name,%prefix,%backup) {
	%baseFolder = $HLab_ForestDataGen_Folder;
	buildForestDataFromFolder(%baseFolder,%name,%prefix,%backup);
}

//==============================================================================
// Get the Screen coords for a 3D position
function buildForestDataFromFolder(%baseFolder,%name,%prefix,%backup,%createBrushGroup) {
	%fullFolder = %baseFolder@"/";

	if (!isDirectory(%fullFolder )) {
		warnLog("Invalid folder to serach for forest models:",%fullFolder);
		return;
	}

	if (%name $= "")
		%name = getFileFolder(%baseFolder);

	%itemFile = "art/forest/auto/fh_"@%name@"Items.cs";
	%itemWrite = new FileObject();
	%itemWrite.OpenForWrite(%itemFile);
	%rootGroup = new SimGroup() {
		internalName = %name;
	};
	%searchFolder = %fullFolder@"*.dae";

	for(%daeFile = findFirstFile(%searchFolder); %daeFile !$= ""; %daeFile = findNextFile(%searchFolder)) {
		%intName = fileBase(%daeFile);
		//%intName = %prefix@"_"@fileBase(%daeFile);
		%dataName = "FI_"@%intName;

		if (isObject(%dataName))
			continue;

		%itemWrite.writeLine("datablock TSForestItemData("@%dataName@")");
		%itemWrite.writeLine("{");
		%itemWrite.writeLine("internalName = \"item_"@%intName@"\";");
		%itemWrite.writeLine("shapeFile = \""@%daeFile@"\";");
		%itemWrite.writeLine("};");
	}

	%itemWrite.close();
	%itemWrite.delete();
	exec(%itemFile);

	//Now go through each files again to add a brush with latest items
	for(%daeFile = findFirstFile(%searchFolder); %daeFile !$= ""; %daeFile = findNextFile(%searchFolder)) {
		%intName = %prefix@"_"@fileBase(%daeFile);
		%dataName = "FI_"@fileBase(%daeFile);
		%filePath = filePath(%daeFile);
		devLog("Filepath:",%filePath);
		devLog("Replace:",%fullFolder);
		%subFolders = strreplace(%filePath,%fullFolder,"");
		devLog("Subfolders:",%subFolders);
		%subFolders = strreplace(%subFolders,"/"," ");
		devLog("Subfolders Words:",%subFolders);
		%level = 1;
		%parentGroup = %rootGroup;
		%folderCount = getWordCount(%subFolders);
		%folderId = 0;

		foreach$(%subFolder in %subFolders) {
			%folderId++;

			if (%createBrushGroup && %folderId >= %folderCount) {
				if (!isObject(%subGroup[%subFolder])) {
					%subGroup[%subFolder] = new ForestBrush() {
						internalName = %subFolder;
						parentGroup = %parentGroup;
					};
					%parentGroup.add(%subGroup[%subFolder]);
				}
			} else if (!isObject(%subGroup[%subFolder])) {
				%subGroup[%subFolder] = new SimGroup() {
					internalName = %subFolder;
				};
				%parentGroup.add(%subGroup[%subFolder]);
			}

			%parentGroup = %subGroup[%subFolder];
		}

		%newBrush = new ForestBrushElement() {
			internalName = %intName;
			canSave = "1";
			canSaveDynamicFields = "1";
			ForestItemData = %dataName;
			probability = "1";
			rotationRange = "360";
			scaleMin = "1";
			scaleMax = "1";
			scaleExponent = "1";
			sinkMin = "0";
			sinkMax = "0";
			sinkRadius = "1";
			slopeMin = "0";
			slopeMax = "90";
			elevationMin = "-10000";
			elevationMax = "10000";
		};
		%parentGroup.add(%newBrush);
	}

	if (%backup) {
		%brushFile = "art/forest/tmp_"@%name@"Brushes.txt";
		%brushWrite = new FileObject();
		%brushWrite.OpenForWrite(%brushFile);
		%brushWrite.writeObject(%rootGroup);
		%brushWrite.close();
		%brushWrite.delete();
		ForestBrushGroup.save("art/forest/brushes.backup");
	}

	if (!isObject(ForestBrushGroup))
		exec("art/forest/brushes.cs");

	ForestBrushGroup.add(%rootGroup);
	ForestBrushGroup.save( "art/forest/brushes.cs" );
}
//------------------------------------------------------------------------------
//==============================================================================
// Get the Screen coords for a 3D position
function buildForestFromFolder(%baseFolder,%folder,%backup,%baseFolder) {
	%baseFolder = $HLab_ForestDataGen_Folder;
	%fullFolder = %baseFolder;

	if (%folder !$= "")
		%fullFolder = %baseFolder@%folder@"/";

	if (!IsDirectory($HLab_ForestDataGen_Folder@%folder@"/")) {
		warnLog("Invalid directory specified");
		return;
	}

	%itemFile = "art/forest/auto/fh_"@%folder@"Items.cs";
	%itemWrite = new FileObject();
	%itemWrite.OpenForWrite(%itemFile);
	%rootGroup = new SimGroup() {
		internalName = "FH "@%folder;
	};
	%searchFolder = %fullFolder@"*.dae";

	for(%daeFile = findFirstFile(%searchFolder); %daeFile !$= ""; %daeFile = findNextFile(%searchFolder)) {
		%intName = "fh"@%folder@"_"@fileBase(%daeFile);
		%dataName = "FI_"@%intName;
		%itemWrite.writeLine("datablock TSForestItemData("@%dataName@")");
		%itemWrite.writeLine("{");
		%itemWrite.writeLine("internalName = \"item_"@%intName@"\";");
		%itemWrite.writeLine("shapeFile = \""@%daeFile@"\";");
		%itemWrite.writeLine("};");
	}

	%itemWrite.close();
	%itemWrite.delete();
	exec(%itemFile);

	//Now go through each files again to add a brush with latest items
	for(%daeFile = findFirstFile(%searchFolder); %daeFile !$= ""; %daeFile = findNextFile(%searchFolder)) {
		%intName = "fh"@%folder@"_"@fileBase(%daeFile);
		%dataName = "FI_"@%intName;
		%filePath = filePath(%daeFile);
		devLog("Filepath:",%filePath);
		devLog("Replace:",%fullFolder);
		%subFolders = strreplace(%filePath,%fullFolder,"");
		devLog("Subfolders:",%subFolders);
		%subFolders = strreplace(%subFolders,"/"," ");
		devLog("Subfolders Words:",%subFolders);
		%level = 1;
		%parentGroup = %rootGroup;

		foreach$(%subFolder in %subFolders) {
			if (!isObject(%subGroup[%subFolder])) {
				%subGroup[%subFolder] = new SimGroup() {
					internalName = %subFolder;
				};
				%parentGroup.add(%subGroup[%subFolder]);
			}

			%parentGroup = %subGroup[%subFolder];
		}

		%newBrush = new ForestBrushElement() {
			internalName = %intName;
			canSave = "1";
			canSaveDynamicFields = "1";
			ForestItemData = %dataName;
			probability = "1";
			rotationRange = "360";
			scaleMin = "1";
			scaleMax = "1";
			scaleExponent = "1";
			sinkMin = "0";
			sinkMax = "0";
			sinkRadius = "1";
			slopeMin = "0";
			slopeMax = "90";
			elevationMin = "-10000";
			elevationMax = "10000";
		};
		%parentGroup.add(%newBrush);
	}

	if (%backup) {
		%brushFile = "art/forest/tmp_"@%folder@"Brushes.txt";
		%brushWrite = new FileObject();
		%brushWrite.OpenForWrite(%brushFile);
		%brushWrite.writeObject(%rootGroup);
		%brushWrite.close();
		%brushWrite.delete();
		ForestBrushGroup.save("art/forest/brushes.backup");
	}

	if (!isObject(ForestBrushGroup))
		exec("art/forest/brushes.cs");

	ForestBrushGroup.add(%rootGroup);
	ForestBrushGroup.save( "art/forest/brushes.cs" );
}
//------------------------------------------------------------------------------
// *-ON