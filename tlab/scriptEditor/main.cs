//==============================================================================
// Lab Editor -> Scene Editor Plugin
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function initializeScriptEditor() {
    echo( " - Initializing Scene Editor" );
    
 exec("tlab/scriptEditor/gui/ScriptEditorGui.gui");
 exec("tlab/scriptEditor/gui/ScriptEditorTools.gui");
    exec("tlab/scriptEditor/gui/ScriptEditorToolbar.gui");
    
    exec("tlab/scriptEditor/ScriptEditorGui.cs");
    exec("tlab/scriptEditor/ScriptEditorPlugin.cs");
    exec("tlab/scriptEditor/ScriptEditorTree.cs");
    
    $EScript = newScriptObject("EScript");
    

   Lab.addPluginEditor("ScriptEditor",ScriptEditorGui);
    Lab.addPluginGui("ScriptEditor",ScriptEditorTools); 
    Lab.addPluginToolbar("ScriptEditor",ScriptEditorToolbar);


    Lab.createPlugin("ScriptEditor","Script Editor",true);
    ScriptEditorPlugin.superClass = "EditorPlugin";    
   ScriptEditorPlugin.no3D = true;
}
