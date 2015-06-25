//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Create the Plugin object with initial data
function EditorSaveMission() {
	// just save the mission without renaming it
	if(isFunction("getObjectLimit") && MissionGroup.getFullCount() >= getObjectLimit()) {
		LabMsgOk( "Object Limit Reached", "You have exceeded the object limit of " @ getObjectLimit() @ " for this demo. You can remove objects if you would like to add more." );
		return;
	}

	// first check for dirty and read-only files:
	if((EWorldEditor.isDirty || ETerrainEditor.isMissionDirty) && !isWriteableFileName($Server::MissionFile)) {
		ToolsMsgBox("Error", "Mission file \""@ $Server::MissionFile @ "\" is read-only.  Continue?", "Ok", "Stop");
		return false;
	}

	if(ETerrainEditor.isDirty) {
		// Find all of the terrain files
		initContainerTypeSearch($TypeMasks::TerrainObjectType);

		while ((%terrainObject = containerSearchNext()) != 0) {
			if (!isWriteableFileName(%terrainObject.terrainFile)) {
				if (ToolsMsgBox("Error", "Terrain file \""@ %terrainObject.terrainFile @ "\" is read-only.  Continue?", "Ok", "Stop") == $MROk)
					continue;
				else
					return false;
			}
		}
	}

	// now write the terrain and mission files out:

	if(EWorldEditor.isDirty || ETerrainEditor.isMissionDirty)
		MissionGroup.save($Server::MissionFile);

	if(ETerrainEditor.isDirty) {
		// Find all of the terrain files
		initContainerTypeSearch($TypeMasks::TerrainObjectType);

		while ((%terrainObject = containerSearchNext()) != 0)
			%terrainObject.save(%terrainObject.terrainFile);
	}

	ETerrainPersistMan.saveDirty();

	// Give EditorPlugins a chance to save.
	for ( %i = 0; %i < EditorPluginSet.getCount(); %i++ ) {
		%obj = EditorPluginSet.getObject(%i);

		//if ( %obj.isDirty() )
			%obj.onSaveMission( $Server::MissionFile );
	}

	EditorClearDirty();
	EditorGui.saveAs = false;
	return true;
}

function EditorSaveMissionAs( %missionName ) {
	if(isFunction("getObjectLimit") && MissionGroup.getFullCount() >= getObjectLimit()) {
		LabMsgOk( "Object Limit Reached", "You have exceeded the object limit of " @ getObjectLimit() @ " for this demo. You can remove objects if you would like to add more." );
		return;
	}

	if(!$Pref::disableSaving && !isWebDemo()) {
		// If we didn't get passed a new mission name then
		// prompt the user for one.
		if ( %missionName $= "" ) {
			%dlg = new SaveFileDialog() {
				Filters        = $Pref::WorldEditor::FileSpec;
				DefaultPath    = Lab.levelsDirectory;
				ChangePath     = false;
				OverwritePrompt   = true;
			};
			%ret = %dlg.Execute();

			if(%ret) {
				// Immediately override/set the levelsDirectory
				Lab.levelsDirectory = collapseFilename(filePath( %dlg.FileName ));
				%missionName = %dlg.FileName;
			}

			%dlg.delete();

			if(! %ret)
				return;
		}

		if( fileExt( %missionName ) !$= ".mis" )
			%missionName = %missionName @ ".mis";

		EWorldEditor.isDirty = true;
		%saveMissionFile = $Server::MissionFile;
		$Server::MissionFile = %missionName;
		%copyTerrainsFailed = false;
		// Rename all the terrain files.  Save all previous names so we can
		// reset them if saving fails.
		%newMissionName = fileBase(%missionName);
		%oldMissionName = fileBase(%saveMissionFile);
		initContainerTypeSearch( $TypeMasks::TerrainObjectType );
		%savedTerrNames = new ScriptObject();

		for( %i = 0;; %i ++ ) {
			%terrainObject = containerSearchNext();

			if( !%terrainObject )
				break;

			%savedTerrNames.array[ %i ] = %terrainObject.terrainFile;
			%terrainFilePath = makeRelativePath( filePath( %terrainObject.terrainFile ), getMainDotCsDir() );
			%terrainFileName = fileName( %terrainObject.terrainFile );

			// Workaround to have terrains created in an unsaved "New Level..." mission
			// moved to the correct place.

			if( EditorGui.saveAs && %terrainFilePath $= "tlab/art/terrains" )
				%terrainFilePath = "art/terrains";

			// Try and follow the existing naming convention.
			// If we can't, use systematic terrain file names.
			if( strstr( %terrainFileName, %oldMissionName ) >= 0 )
				%terrainFileName = strreplace( %terrainFileName, %oldMissionName, %newMissionName );
			else
				%terrainFileName = %newMissionName @ "_" @ %i @ ".ter";

			%newTerrainFile = %terrainFilePath @ "/" @ %terrainFileName;

			if (!isWriteableFileName(%newTerrainFile)) {
				if (ToolsMsgBox("Error", "Terrain file \""@ %newTerrainFile @ "\" is read-only.  Continue?", "Ok", "Stop") == $MROk)
					continue;
				else {
					%copyTerrainsFailed = true;
					break;
				}
			}

			if( !%terrainObject.save( %newTerrainFile ) ) {
				error( "Failed to save '" @ %newTerrainFile @ "'" );
				%copyTerrainsFailed = true;
				break;
			}

			%terrainObject.terrainFile = %newTerrainFile;
		}

		ETerrainEditor.isDirty = false;

		// Save the mission.
		if(%copyTerrainsFailed || !EditorSaveMission()) {
			// It failed, so restore the mission and terrain filenames.
			$Server::MissionFile = %saveMissionFile;
			initContainerTypeSearch( $TypeMasks::TerrainObjectType );

			for( %i = 0;; %i ++ ) {
				%terrainObject = containerSearchNext();

				if( !%terrainObject )
					break;

				%terrainObject.terrainFile = %savedTerrNames.array[ %i ];
			}
		}

		%savedTerrNames.delete();
	} else {
		EditorSaveMissionMenuDisableSave();
	}
}

function EditorOpenMission(%filename) {
	if( EditorIsDirty() && !isWebDemo() ) {
		// "EditorSaveBeforeLoad();", "getLoadFilename(\"*.mis\", \"EditorDoLoadMission\");"
		if(ToolsMsgBox("Mission Modified", "Would you like to save changes to the current mission \"" @
							$Server::MissionFile @ "\" before opening a new mission?", SaveDontSave, Question) == $MROk) {
			if(! EditorSaveMission())
				return;
		}
	}

	if(%filename $= "") {
		%dlg = new OpenFileDialog() {
			Filters        = $Pref::WorldEditor::FileSpec;
			DefaultPath    = Lab.levelsDirectory;
			ChangePath     = false;
			MustExist      = true;
		};
		%ret = %dlg.Execute();

		if(%ret) {
			// Immediately override/set the levelsDirectory
			Lab.levelsDirectory = collapseFilename(filePath( %dlg.FileName ));
			%filename = %dlg.FileName;
		}

		%dlg.delete();

		if(! %ret)
			return;
	}

	// close the current editor, it will get cleaned up by MissionCleanup
	if( isObject( "Editor" ) )
		Editor.close( DlgLoadingLevel );

	EditorClearDirty();

	// If we haven't yet connnected, create a server now.
	// Otherwise just load the mission.

	if( !$missionRunning ) {
		activatePackage( "BootEditor" );
		StartLevel( %filename );
	} else {
		Game.loadMission( %filename, true ) ;
		pushInstantGroup();
		// recreate and open the editor
		Editor::create();
		MissionCleanup.add( Editor );
		MissionCleanup.add( Editor.getUndoManager() );
		EditorGui.loadingMission = true;
		Editor.open();
		popInstantGroup();
	}
}
//------------------------------------------------------------------------------
