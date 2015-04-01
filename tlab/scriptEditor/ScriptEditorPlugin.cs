//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function ScriptEditorPlugin::onWorldEditorStartup( %this ) {
    Parent::onWorldEditorStartup( %this );
    // Add ourselves to the Editor Settings window  
}

function ScriptEditorPlugin::onActivated( %this ) {   
    Parent::onActivated(%this);
}

function ScriptEditorPlugin::onDeactivated( %this ) {   
    Parent::onDeactivated(%this);
}


function ScriptEditorPlugin::handleEscape( %this ) {
    Parent::handleEscape(%this);
}

function ScriptEditorPlugin::isDirty( %this ) {
    return ScriptEditorGui.isDirty;
}

function ScriptEditorPlugin::onSaveMission( %this, %missionFile ) {
     Parent::onSaveMission(%this, %missionFile);
}
