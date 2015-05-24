//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::setEditor( %this, %newEditor, %dontActivate ) {   
	
	//First make sure the new editor is valid
	if (%newEditor $= ""){
	   %newEditor = Lab.defaultPlugin;
		if( !isObject( %newEditor )) {
			warnLog("Set editor called for invalid editor plugin:",%newEditor);
			return;
		}
	}
	// If we have a special set editor function, run that instead
	if( %newEditor.isMethod( "setEditorFunction" ) ) {
		%pluginActivated= %newEditor.setEditorFunction();
		if (!%pluginActivated){
			warnLog("Plugin failed to activate, keeping the current plugin active");
			return;
		}
	}	
	
	//Check the EditorGui of both, if different hide previous and show new
	//If there's a editor loaded, deactivate it
	if ( isObject( %this.currentEditor ) ) {
		//Cancel if the current is the same as new
		if( %this.currentEditor.getId() == %newEditor.getId() )
			return;

		if( %this.currentEditor.isActivated )
			%this.currentEditor.onDeactivated();

		if( isObject( %this.currentEditor.editorGui ) ) {
			%this.orthoFOV = %this.currentEditor.editorGui.getOrthoFOV();
		}
	}	
	%this.syncEditor( %newEditor );
	%this.currentEditor = %newEditor;
	%this.currentEditor.onActivated();
	//Lab.activatePlugin(%this.currentEditor);  	
}
function Lab::syncEditor( %this,%newEditor ) {   
   if (%newEditor $= "")
	   %newEditor = %this.currentEditor;
	if (!isObject(%newEditor)) {
		warnLog("Trying to sync Toolbar for invalid editor:",%newEditor);
		return;
	}

	%editorName = %newEditor.getName();
   if ($Cfg_UseCoreMenubar){
      // Sync with menu bar
      %menu = Lab.findMenu( "Editors" );
      %count = %menu.getItemCount();
      for ( %i = 0; %i < %count; %i++ ) {
         %pluginObj = getField( %menu.item[%i], 2 );
         if ( %pluginObj $= %newEditor ) {
            %menu.checkRadioItem( 0, %count, %i );
            break;
         }
      }
   }
	%icon = ToolsToolbarArray.findObjectByInternalName(%newEditor.plugin);
	%icon.setStateOn(1); //Grouped radio button => Other will be turned off automatically

	%paletteName = strreplace(%newEditor, "Plugin", "Palette");
	if (%newEditor.customPalette !$= "") {
		%paletteName = %newEditor.customPalette;
	}
	Lab.togglePluginPalette(%paletteName);
	//EWToolsPaletteContainer.togglePalette(%paletteName);
}

