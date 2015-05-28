//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function initializeLabPhysic() {
   return;
    echo( " - Initializing Lab Physic Editor" );


    exec("tlab/labPhysic/labPhysicInit.cs");
    exec("tlab/labPhysic/labPhysicUndo.cs");
    exec("tlab/labPhysic/labPhysicScript.cs");

    
    exec("tlab/labPhysic/gui/LabPhysicCreateDlg.gui");
    exec("tlab/labPhysic/gui/LabPhysicTools.gui");
    exec("tlab/labPhysic/gui/LabPhysicPaletteGui.gui");
    exec("tlab/labPhysic/gui/LabPhysicToolbar.gui");

    //exec("tlab/labPhysic/gui/LabPhysicCreatePrompt.gui");
    exec("tlab/labPhysic/guiCallbacks.cs");
    exec("tlab/labPhysic/scripts/datablockTree.cs");
    exec("tlab/labPhysic/scripts/builder.cs");
    exec("tlab/labPhysic/scripts/inspectorMain.cs");
    exec("tlab/labPhysic/scripts/inspectorParams.cs");

    Lab.buildLabPhysicInspectorParams();
    // Add ourselves to EditorGui, where all the other tools reside
    LabPhysicInspectorWindow.setVisible( true );
    LabPhysicTreeWindow.setVisible( true );

    //LabPhysicInspectorWindow.defaultParent = LabPhysicInspectorWindow.getParent();
    //LabPhysicTreeWindow.defaultParent = LabPhysicTreeWindow.getParent();

    //Add the plugin GUI elements
    Lab.addPluginGui("LabPhysic",LabPhysicTools);
    // Lab.addPluginGui("LabPhysic",LabPhysicTreeWindow);
    // Lab.addPluginGui("LabPhysic",LabPhysicInspectorWindow);
    Lab.addPluginDlg("LabPhysic",   LabPhysicCreateDlg);
    Lab.addPluginPalette("LabPhysic",   LabPhysicPalette);
    Lab.addPluginToolbar("LabPhysic",LabPhysicToolbar);

    Lab.createPlugin("LabPhysic","Lab physic editor");

    // create our persistence manager
    LabPhysicPlugin.PM = new PersistenceManager();

    %map = new ActionMap();
    %map.bindCmd( keyboard, "1", "LabPhysicNoneModeBtn.performClick();", "" ); // Select
    %map.bindCmd( keyboard, "2", "LabPhysicMoveModeBtn.performClick();", "" );   // Move
    %map.bindCmd( keyboard, "3", "LabPhysicRotateModeBtn.performClick();", "" ); // Rotate
    %map.bindCmd( keyboard, "4", "LabPhysicScaleModeBtn.performClick();", "" );  // Scale
    %map.bindCmd( keyboard, "backspace", "LabPhysicPlugin.onDeleteKey();", "" );
    %map.bindCmd( keyboard, "delete", "LabPhysicPlugin.onDeleteKey();", "" );
    %map.bindCmd( keyboard, "ctrl 1", "Lab.physicsStartSimulation();", "" );
    %map.bindCmd( keyboard, "ctrl 2", "Lab.physicsStopSimulation();", "" );



    LabPhysicPlugin.map = %map;
  
}

//---------------------------------------------------------------------------------------------
function reloadLabPhysicParams() {
    exec("tlab/labPhysic/paramsInspector.cs");
    Lab.buildLabPhysicInspectorParams();
}

function setGuiParentLabPhysic(%addToEditor) {
    if (%addToEditor) {
        EditorGui.add( LabPhysicInspectorWindow );
        EditorGui.add( LabPhysicTreeWindow );
    } else {
        LabPhysicInspectorWindow.defaultParent.add( LabPhysicInspectorWindow );
        LabPhysicTreeWindow.defaultParent.add( LabPhysicTreeWindow );
    }
}

function initLabPhysicGui() {

    // exec("tlab/labPhysic/gui/LabPhysicToolbar.gui");
}


function addLabPhysic() {
    EditorGui.add( LabPhysicInspectorWindow );
    EditorGui.add( LabPhysicTreeWindow );
    EditorGui.add( LabPhysicToolbar );
}
function removeLabPhysic() {
    EditorGui.remove( LabPhysicInspectorWindow );
    EditorGui.remove( LabPhysicTreeWindow );
    GuiEditorContentList.add(LabPhysicTreeWindow,"LabPhysicTreeWindow");
    EditorGui.remove( LabPhysicToolbar );
}

function destroyLabPhysic() {
}
