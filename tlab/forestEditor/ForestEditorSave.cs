//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function ForestEditorPlugin::isDirty( %this ) {
    %dirty = %this.dirty || ForestEditorGui.isDirty();
    return %dirty;
}

function ForestEditorPlugin::clearDirty( %this ) {
    %this.dirty = false;
}

function ForestEditorPlugin::onSaveMission( %this, %missionFile ) {
    ForestDataManager.saveDirty();
    
    %file = theForest.datafile;
    
    if (!isFile(%file))
    	%file = filePath(theForest.getFilename())@"/data.forest";
    	//%file = strreplace(theForest.getFilename(),".mis",".forest");
    	
    if (isFile(%file))
			theForest.saveDataFile(%file);	

    ForestBrushGroup.save( "art/forest/brushes.cs" );
}

