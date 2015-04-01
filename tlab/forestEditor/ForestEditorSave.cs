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

    if ( isObject( theForest ) )
        theForest.saveDataFile();

    ForestBrushGroup.save( "art/forest/brushes.cs" );
}

