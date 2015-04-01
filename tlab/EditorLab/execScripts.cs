//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function Lab::loadEditorProfiles( %this,%loadGui ) {
   if (!%loadGui) return;
//------------------------------------------------------------------------------
//Start by loading the Gui Profiles
exec("tlab/gui/initGuiProfiles.cs");
exec( "tlab/EditorLab/gui/core/cursors.ed.cs" );
exec( "tlab/editorClasses/gui/panels/navPanelProfiles.ed.cs" );

// Make sure we get editor profiles before any GUI's
// BUG: these dialogs are needed earlier in the init sequence, and should be moved to
// common, along with the guiProfiles they depend on.
exec( "tlab/EditorLab/gui/core/guiDialogs.ed.cs" );
}
Lab.loadEditorProfiles($LabExecGui);

//------------------------------------------------------------------------------
//Create Lab Editor Core Objects
exec("tlab/EditorLab/initLabEditor.cs");
exec("tlab/EditorLab/commonSettings.cs");
//------------------------------------------------------------------------------
//Load the Editor Menubar Scripts
exec("tlab/EditorLab/menubar/manageMenu.cs");
exec("tlab/EditorLab/menubar/labstyle/menubarScript.cs");
exec("tlab/EditorLab/menubar/labstyle/buildWorldMenu.cs");
exec("tlab/EditorLab/menubar/native/menus.cs");
exec("tlab/EditorLab/menubar/native/menuHandlers.cs");
exec("tlab/EditorLab/menubar/native/lightingMenu.cs");


//------------------------------------------------------------------------------
//Load the Editor Main Scripts
exec("tlab/EditorLab/scripts/editorCallback.cs");
exec("tlab/EditorLab/scripts/saveEditorGui.cs");




exec("tlab/EditorLab/scripts/cameraSetup.cs");
exec("tlab/EditorLab/scripts/cameraScripts.cs");
exec("tlab/EditorLab/scripts/cameraCommands.cs");
exec("tlab/EditorLab/scripts/editorBinds.cs");
//------------------------------------------------------------------------------
//Load the Editor Main Gui
exec("tlab/EditorLab/helpers/paramsBuilder.cs");
exec("tlab/EditorLab/helpers/paramsFunctions.cs");
exec("tlab/EditorLab/helpers/guiHelpers.cs");
exec("tlab/EditorLab/helpers/objectsGroup.cs");
exec("tlab/EditorLab/helpers/sceneObjects.cs");
exec("tlab/EditorLab/helpers/guiTreeView.cs");
//------------------------------------------------------------------------------
//Load the Editor Main Gui

function execLabFunctions(  ) {
   exec("tlab/EditorLab/functions/objectsGroup.cs");
   exec("tlab/EditorLab/functions/objectsSelect.cs");
   exec("tlab/EditorLab/functions/objectsCopy.cs");
}
execLabFunctions();

//------------------------------------------------------------------------------
//Load the LabGui (Cleaned EditorGui files)
function Lab::loadEditorScripts( %this,%loadGui ) {
   if (%loadGui){
      exec("tlab/EditorLab/editor/gui/EditorGui.gui");
      exec("tlab/EditorLab/gui/cursors.cs");
   }
   exec("tlab/EditorLab/editor/EditorOpen.cs");
   exec("tlab/EditorLab/editor/EditorClose.cs");
   exec("tlab/EditorLab/editor/EditorScript.cs");
   exec("tlab/EditorLab/editor/EWorldEditor.cs");
  exec("tlab/EditorLab/editor/WorldEditor.cs");

   exec("tlab/EditorLab/editor/manageGui.cs");
   
  
   
   exec("tlab/EditorLab/editor/generalFunctions.cs");
}
Lab.loadEditorScripts($LabExecGui);
//------------------------------------------------------------------------------
//Load the Tools scripts (Toolbar and special functions)
function Lab::loadToolbarScripts( %this,%loadGui ) {
  
   exec("tlab/EditorLab/toolbar/toolbarInit.cs");
   exec("tlab/EditorLab/toolbar/toolbarDragDrop.cs");
   exec("tlab/EditorLab/toolbar/toolbarDialogs.cs");
    exec("tlab/EditorLab/toolbar/statusbarSetup.cs");
    exec("tlab/EditorLab/toolbar/palettebarSetup.cs");
}
Lab.loadToolbarScripts($LabExecGui);


//------------------------------------------------------------------------------
//Load the LabGui Ctrl (Cleaned EditorGui files)
function Lab::loadGuis( %this,%loadGui ) {
  if (%loadGui){          
      exec("tlab/EditorLab/gui/CtrlCameraSpeedDropdown.gui");
      exec("tlab/EditorLab/gui/CtrlSnapSizeSlider.gui");
      exec("tlab/EditorLab/gui/messageBoxes/LabMsgBoxesGui.gui");
      exec("tlab/EditorLab/gui/DlgManageSFXParameters.gui" );
      exec("tlab/EditorLab/gui/LabWidgetsGui.gui");
      exec("tlab/EditorLab/gui/DlgAddFMODProject.gui");
      exec("tlab/EditorLab/gui/DlgEditorChooseLevel.gui");      
      exec("tlab/EditorLab/gui/DlgGenericPrompt.gui");
      exec("tlab/EditorLab/gui/DlgObjectBuilder.gui");
      exec("tlab/EditorLab/gui/DlgTimeAdjust.gui");
      exec( "tlab/EditorLab/gui/Settings/LabMissionSettingsDlg.gui" );
   } 
   exec("tlab/EditorLab/gui/messageBoxes/ToolsMsgBox.cs");   
   exec("tlab/EditorLab/gui/messageBoxes/LabMsgBoxesGui.cs");
   exec("tlab/EditorLab/gui/DlgManageSFXParameters.cs" );
   exec("tlab/EditorLab/gui/DlgAddFMODProject.cs");
   exec("tlab/EditorLab/gui/DlgEditorChooseLevel.cs");
   exec( "tlab/EditorLab/gui/Settings/LabMissionSettingsDlg.cs" );
}
Lab.loadGuis($LabExecGui);


//------------------------------------------------------------------------------
//Old Settings Dialog for temporary references
function Lab::loadLabDialogsScripts( %this,%includeGui ) {
	if (%includeGui) {
		exec("tlab/EditorLab/gui/dialogs/ETransformSelection.gui");
		exec("tlab/EditorLab/gui/dialogs/ESnapOptions.gui");
		exec("tlab/EditorLab/gui/dialogs/EVisibilityLayers.gui");
		exec("tlab/EditorLab/gui/dialogs/ECloneTool.gui");
		exec("tlab/EditorLab/gui/dialogs/ECloneDrag.gui");
		exec("tlab/EditorLab/gui/dialogs/ESelectObjects.gui");
		exec("tlab/EditorLab/gui/dialogs/EManageBookmarks.gui");
		exec("tlab/EditorLab/gui/dialogs/ESceneManager.gui");
		exec("tlab/EditorLab/gui/dialogs/EToolDlgGroup.gui");
		exec("tlab/EditorLab/gui/dialogs/EToolCamViewDlg.gui");
		exec("tlab/EditorLab/gui/dialogs/EMissionArea.gui");
		
	}


	exec("tlab/EditorLab/gui/dialogs/ESnapOptions.cs");
	exec("tlab/EditorLab/gui/dialogs/EVisibilityLayers.cs");
	exec("tlab/EditorLab/gui/dialogs/ETransformSelection.cs");
	exec("tlab/EditorLab/gui/dialogs/ECloneTool.cs");
	exec("tlab/EditorLab/gui/dialogs/ECloneDrag.cs");
	exec("tlab/EditorLab/gui/dialogs/ESelectObjects.cs");
	exec("tlab/EditorLab/gui/dialogs/EManageBookmarks.cs");
	exec("tlab/EditorLab/gui/dialogs/ESceneManager.cs");
	exec("tlab/EditorLab/gui/dialogs/EToolDlgGroup.cs");
	exec("tlab/EditorLab/gui/dialogs/EToolCamViewDlg.cs");
	exec("tlab/EditorLab/gui/dialogs/EMissionArea.cs");
}
Lab.loadLabDialogsScripts($LabExecGui);


//------------------------------------------------------------------------------
// Lab Editor Settings scripts
function Lab::loadSettingsScripts( %this,%loadGui ) {
	if (%loadGui)
		exec("tlab/EditorLab/settings/LabSettingsDlg.gui");

	
	exec("tlab/EditorLab/settings/LabSettingsDlg.cs");

	exec("tlab/EditorLab/settings/settingsMain.cs");
	exec("tlab/EditorLab/settings/settingsObject.cs");
	exec("tlab/EditorLab/settings/paramsScript.cs");
	exec("tlab/EditorLab/settings/paramsInit.cs");
	
}
Lab.loadSettingsScripts($LabExecGui);

//------------------------------------------------------------------------------
// Lab Editor common scrits
function Lab::loadCommonScripts( %this ) {
	exec("tlab/EditorLab/common/physicSystem.cs");
	exec("tlab/EditorLab/common/creatorHelpers.cs");
	exec("tlab/EditorLab/common/defineFields.cs");
	exec("tlab/EditorLab/common/areaEditor.cs");
	exec("tlab/EditorLab/common/specialRender.cs");
	exec("tlab/EditorLab/common/undoManager.cs");
}
Lab.loadCommonScripts($LabExecGui);

//------------------------------------------------------------------------------
// Lab Editor Plugins Scripts
function Lab::loadPluginsScripts( %this ) {
	exec("tlab/EditorLab/plugin/setPluginEditorGui.cs");
	exec("tlab/EditorLab/plugin/initPlugin.cs");
	exec("tlab/EditorLab/plugin/managePluginGui.cs");

	exec("tlab/EditorLab/plugin/managePluginBar.cs");

	exec("tlab/EditorLab/plugin/classes/EditorPlugin.cs");
	exec("tlab/EditorLab/plugin/classes/WEditorPlugin.cs");
}
Lab.loadPluginsScripts($LabExecGui);






