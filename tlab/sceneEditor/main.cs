//==============================================================================
// TorqueLab -> Scene Editor Plugin
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function initializeSceneEditor()
{
	echo( " - Initializing Scene Editor" );
	execSceneEditor(true);
	Lab.addPluginGui("SceneEditor",SceneEditorTools);
	Lab.addPluginToolbar("SceneEditor",SceneEditorToolbar);
	Lab.addPluginPalette("SceneEditor",SceneEditorPalette);
	Lab.createPlugin("SceneEditor","Scene Editor",true);
	SceneEditorPlugin.isDefaultPlugin = true;
	SceneEditorPlugin.superClass = "WEditorPlugin";
	$SEPtools = newScriptObject("SEPtools");
}
function execSceneEditor(%loadGui)
{
	if (%loadGui)
	{
		exec("tlab/sceneEditor/gui/SceneEditorTools.gui");
		exec("./gui/SceneEditorToolbar.gui" );
		exec("./gui/SceneEditorPalette.gui" );
	}

	exec("tlab/sceneEditor/objectCreator.cs");
	exec("tlab/sceneEditor/sceneEditor.cs");
	exec("tlab/sceneEditor/sceneEditorTree.cs");
	exec("tlab/sceneEditor/sceneGroupsTree.cs");
	exec("tlab/sceneEditor/SceneEditorPlugin.cs");
	exec("tlab/sceneEditor/SceneEditorParams.cs");
	exec("tlab/sceneEditor/SceneEditorToolbar.cs");
	exec("tlab/sceneEditor/SE_BuilderTools.cs");
}
