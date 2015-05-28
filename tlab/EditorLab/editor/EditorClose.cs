//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Handle the escape bind
function EditorGui::handleEscape( %this ) {
	%result = false;
	//Check if the current Plugin accept the request
	if ( isObject( Lab.currentEditor ) )
		%result = Lab.currentEditor.handleEscape();

	if ( !%result ) {
		Editor.close($HudCtrl);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//Close editor call
function Editor::close(%this, %gui) {

	%this.editorDisabled();
	Canvas.setContent(%gui);
	if(isObject(HudChatEdit))
		HudChatEdit.close();
	
	//Restore the Client COntrolling Object	
   if (isObject( Lab.clientWasControlling))
      LocalClientConnection.setControlObject( Lab.clientWasControlling );
	
}
//------------------------------------------------------------------------------
//==============================================================================
// EditorGui onSleep -> When the EditorGui is hidded
function EditorGui::onSleep( %this ) {
	// Deactivate the current editor plugin.
	if (!$pref::Misc::AlwaysNotifyFileChange)
		stopFileChangeNotifications();
		
	if( Lab.currentEditor.isActivated )
		Lab.currentEditor.onDeactivated();
	

	Lab.saveAllPluginData();
	LabCfg.write();
	
	// Remove the editor's ActionMaps.
	EditorMap.pop();
	MoveMap.pop();

	// Notify the editor plugins that the editor will be closing.
	foreach( %plugin in EditorPluginSet )
		%plugin.onEditorSleep();

	if(isObject($Server::CurrentScene))
		$Server::CurrentScene.open();
	
	//Set the game camera (Will load the same camera object as before entering editor)
   Lab.setGameCamera();
}
//------------------------------------------------------------------------------
//==============================================================================
// Called before onSleep when the canvas content is changed
function EditorGui::onUnsetContent(%this, %newContent) {
	Lab.detachMenus();
}
//------------------------------------------------------------------------------
//==============================================================================
// Shutdown the EditorGui-> Called from the onExit function
function EditorGui::shutdown( %this ) {
	// Store settings.
	LabCfg.write();

	// Deactivate current editor.
	Lab.setEditor( "" );

	// Call the shutdown callback on the editor plugins.
	foreach( %plugin in EditorPluginSet )
		%plugin.onWorldEditorShutdown();
}
//------------------------------------------------------------------------------

//==============================================================================
// Called when a mission is ended-> used to cleanup Plugins objects
function EditorMissionCleanup::onRemove( %this ) {
 
	Lab.levelName = "";
	foreach( %plugin in EditorPluginSet )
		%plugin.onExitMission();
}
//------------------------------------------------------------------------------

