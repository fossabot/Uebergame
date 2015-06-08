//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::initLabEditor( %this ) {
   
   execPattern("tlab/EditorLab/settings/prefs/*.cs");
	
	
		
	if( !isObject( "Lab_PM" ) )
      new PersistenceManager( Lab_PM ); 	
	new SimGroup(ToolLabGuiGroup);

	$LabPluginGroup = newSimSet("LabPluginGroup");

	newSimSet( ToolGuiSet );
	newSimSet( EditorPluginSet );

	//Create a group to keep track of all objects set
	newSimGroup( LabSceneObjectGroups );

	//Create the ScriptObject for the Plugin
	new ScriptObject( WEditorPlugin ) {
		superClass = "EditorPlugin"; //Default to EditorPlugin class
		editorGui = EWorldEditor; //Default to EWorldEditor
		isHidden = true;
	};

	//Load the scripts packages
	//Lab.loadSettingsScripts(true);
	//Lab.loadPluginsScripts();
	//Lab.loadCommonScripts();
	
  
	//Prepare the Settings
	%this.initEditorGui();
	//%this.initSettings();

	%this.initMenubar();
   
	%this.initParamsSystem();
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::initEditorGui( %this ) {
   
	newScriptObject("LabEditor");
	newSimSet("LabGuiSet");
	newSimSet("LabPluginGuiSet");
	newSimSet("LabActiveFrameSet");

	newSimSet("LabEditorGuiSet");
	newSimSet("LabExtraGuiSet");
	newSimSet("LabToolbarGuiSet");
	newSimSet("LabPaletteGuiSet");
	newSimSet("LabDialogGuiSet");
	newSimSet("LabSettingGuiSet");

	$LabPalletteContainer = EditorGui-->ToolsPaletteContainer;
	$LabPalletteArray = EditorGui-->ToolsPaletteArray;

	$LabPluginsArray = EditorGui-->PluginsArray;

	$LabWorldContainer = EditorGui-->WorldContainer;
	$LabSettingContainer = EditorGui-->SettingContainer;
	$LabToolbarContainer = EditorGui-->ToolbarContainer;
	$LabDialogContainer = EditorGui-->ToolsContainer;
	$LabEditorContainer = EditorGui-->EditorContainer;
	$LabExtraContainer = EditorGui-->ExtraContainer;

	EditorFrameMain.frameMinExtent(1,280,100);

}
//------------------------------------------------------------------------------

//==============================================================================
// All the plugins scripts have been loaded
function Lab::pluginInitCompleted( %this ) {
	//Prepare the Settings	
	%this.initConfigSystem();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::reloadAllSettings( %this ) {
	exec("tlab/EditorLab/commonSettings.cs");
	%this.initCommonParams(); 
	%this.initCommonSettings(); //FIXME Old param system
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::loadEditorData( %this ) {
	exec("tlab/EditorLab/settings/labData.cs");
}
//------------------------------------------------------------------------------


//==============================================================================
// Rexec all EditorLab scripts (Skip .gui files if loadGui false)
function Lab::doExec( %this,%loadGui ) {
   if (%loadGui $="") $LabExecGui = false;
	 exec("tlab/EditorLab/execScripts.cs");
}
//------------------------------------------------------------------------------