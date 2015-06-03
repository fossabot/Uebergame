//==============================================================================
// TorqueLab -> Scene Editor Plugin
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
//Initialize the Scene Editor Plugin
function initializeSceneEditor()
{
	echo( " - Initializing Scene Editor" );
	execSceneEd(true);

	Lab.addPluginGui("SceneEditor",SceneEditorTools);
	Lab.addPluginToolbar("SceneEditor",SceneEditorToolbar);
	Lab.addPluginPalette("SceneEditor",SceneEditorPalette);
	Lab.createPlugin("SceneEditor","Scene Editor",true);
		EWorldEditor.dropType = SceneEditorPlugin.getCfg("DropType");
	SceneEditorPlugin.isDefaultPlugin = true;
	SceneEditorPlugin.superClass = "WEditorPlugin";
	$SEPtools = newScriptObject("SEPtools");
}
//------------------------------------------------------------------------------
//==============================================================================
// Load the Scene Editor Plugin scripts, load Guis if %loadgui = true
function execSceneEd(%loadGui)
{
	if (%loadGui)
	{
		exec("tlab/sceneEditor/gui/SceneEditorTools.gui");
		exec("./gui/SceneEditorToolbar.gui" );
		exec("./gui/SceneEditorPalette.gui" );
	}
	
	exec("tlab/sceneEditor/sceneEditor.cs");
	exec("tlab/sceneEditor/SceneEditorPlugin.cs");
	exec("tlab/sceneEditor/SceneEditorParams.cs");
	exec("tlab/sceneEditor/SceneEditorToolbar.cs");
	exec("tlab/sceneEditor/SceneEditorTools.cs");
	execPattern("tlab/sceneEditor/creator/*.cs");
	execPattern("tlab/sceneEditor/scene/*.cs");
	execPattern("tlab/sceneEditor/tools/*.cs");
}
//------------------------------------------------------------------------------