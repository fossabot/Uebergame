//==============================================================================
// GameLab -> Netwroking related helpers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Connect to a server
//==============================================================================
//updateSfxProfileFromPrefix("SFX_RainStorm","description","AudioLoop2d");
//==============================================================================
function updateSfxProfileFromPrefix(%filter,%field,%value) {
	if(!isObject(LabHelpers_PM))
		new PersistenceManager("LabHelpers_PM");

	foreach(%data in DataBlockGroup) {
		if (!%data.isMemberOfClass("SFXProfile"))
			continue;

		if (!strFind(%data.getName(),%filter))
			continue;

		%currentVal = %data.getFieldValue(%field);

		if (%currentVal $= %value) {
			continue;
		}

		eval("%data."@%field@" = " @%value@";");
		LabHelpers_PM.setDirty(%data);
		LabHelpers_PM.saveDirtyObject( %data );
	}
}


function generateSfxProfileFromFolder(%startFolder,%prefix,%description) {
	if (%prefix $= "")
		%prefix = "SFX_";

	if (%description $= "")
		%description = "AudioDefault3D";

	%newSfxList = "";
	%files = getMultiExtensionFileList(%startFolder,"ogg wav");

	for(%i = 0; %i < getRecordCount(%files); %i++) {
		%relFile = getRecord(%files,%i);
		%file = %startFolder@%relFile;
		%filename = fileBase(%file);
		%path =  filePath(%file);
		%folderFields = strReplace(%path,"/","\t");
		%folderCount = getFieldCOunt(%folderFields);
		%lastFolder = getField(%folderFields, %folderCount-1);
		%dataName = %prefix@%lastFolder@"_"@%filename;

		if (!isObject(%dataName)) {
			%eval = "datablock SFXProfile(" @ %dataName @ ") { preload = \"1\"; };";
			eval( %eval );
			%dataName.description = %description;
			%dataName.fileName = %file;
			%newSfxList = strAddWord(%newSfxList, %dataName.getName());
			%sfxFolders = strAddWord(%sfxFolders, %path,true);
			%sfxFolderProfiles[%path] =  strAddWord(%sfxFolderProfiles[%path], %dataName.getName());
		} else {
			warnLog("Can't create SFXProfile for sound:",%filename,"There's already a profile using wanted name:",%dataName);
		}
	}

	foreach$(%path in %sfxFolders) {
		%profiles = %sfxFolderProfiles[%path];
		saveGenerateSFXProfilesToFolder(%path,%profiles);
	}

	return %newSfxList;
}
function saveGenerateSFXProfilesToFolder(%folder,%profiles) {
	%saveTo = %folder@"/sfxProfiles.cs";
	%fileObj = getFileWriteObj(%saveTo,true);

	foreach$(%obj in %profiles) {
		if (isObject(%obj)) {
			%fileObj.writeObject(%obj);
		} else {
			warnLog("Trying to save invalid object:",%obj);
		}
	}

	closeFileObj(%fileObj);
}
