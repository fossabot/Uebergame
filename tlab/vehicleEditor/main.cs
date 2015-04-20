//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
// Shape Editor
//------------------------------------------------------------------------------

function initializeVehicleEditor() {
    echo(" % - Initializing Shape Editor");

    exec("./gui/Profiles.cs");

    exec("tlab/VehicleEditor/gui/VehicleEditorPreview.gui");
    exec("./gui/VehicleEditorAnimDlg.gui");
    exec("./gui/VehicleEditorToolbar.gui");

    exec("./gui/VehicleEditorPalette.gui");
    exec("tlab/VehicleEditor/gui/VehicleEditorTools.gui");

   execVehicleEd();
   Lab.addPluginGui("VehicleEditor",VehicleEditorTools);
    Lab.addPluginEditor("VehicleEditor",VehicleEditorPreview);
    Lab.addPluginEditor("VehicleEditor",VehicleEditorAnimDlg,true);

    Lab.addPluginToolbar("VehicleEditor",VehicleEditorToolbar);
    Lab.addPluginPalette("VehicleEditor",   VehicleEditorPalette);

    Lab.createPlugin("VehicleEditor");
    VehicleEditorPlugin.editorGui = VehicleEdShapeView;

      $VehicleEditorDataSet = newSimSet("VehicleEditorDataSet");
    // Add windows to editor gui


    %map = new ActionMap();
    %map.bindCmd( keyboard, "escape", "ToolsToolbarArray->SceneEditorPalette.performClick();", "" );
    %map.bindCmd( keyboard, "1", "VehicleEditorNoneModeBtn.performClick();", "" );
    %map.bindCmd( keyboard, "2", "VehicleEditorMoveModeBtn.performClick();", "" );
    %map.bindCmd( keyboard, "3", "VehicleEditorRotateModeBtn.performClick();", "" );
    //%map.bindCmd( keyboard, "4", "VehicleEditorScaleModeBtn.performClick();", "" ); // not needed for the shape editor
    %map.bindCmd( keyboard, "n", "VehicleEditorToolbar->showNodes.performClick();", "" );
    %map.bindCmd( keyboard, "t", "VehicleEditorToolbar->ghostMode.performClick();", "" );
    %map.bindCmd( keyboard, "r", "VehicleEditorToolbar->wireframeMode.performClick();", "" );
    %map.bindCmd( keyboard, "f", "VehicleEditorToolbar->fitToShapeBtn.performClick();", "" );
    %map.bindCmd( keyboard, "g", "VehicleEditorToolbar->showGridBtn.performClick();", "" );
    %map.bindCmd( keyboard, "h", "VehicleEdSelectWindow->tabBook.selectPage( 2 );", "" ); // Load help tab
    %map.bindCmd( keyboard, "l", "VehicleEdSelectWindow->tabBook.selectPage( 1 );", "" ); // load Library Tab
    %map.bindCmd( keyboard, "j", "VehicleEdSelectWindow->tabBook.selectPage( 0 );", "" ); // load scene object Tab
    %map.bindCmd( keyboard, "SPACE", "VehicleEditorAnimDlg.togglePause();", "" );
    %map.bindCmd( keyboard, "i", "VehicleEdSequences.onEditSeqInOut(\"in\", VehicleEdSeqSlider.getValue());", "" );
    %map.bindCmd( keyboard, "o", "VehicleEdSequences.onEditSeqInOut(\"out\", VehicleEdSeqSlider.getValue());", "" );
    %map.bindCmd( keyboard, "shift -", "VehicleEdSeqSlider.setValue(VehicleEditorAnimDlg-->seqIn.getText());", "" );
    %map.bindCmd( keyboard, "shift =", "VehicleEdSeqSlider.setValue(VehicleEditorAnimDlg-->seqOut.getText());", "" );
    %map.bindCmd( keyboard, "=", "VehicleEditorAnimDlg-->stepFwdBtn.performClick();", "" );
    %map.bindCmd( keyboard, "-", "VehicleEditorAnimDlg-->stepBkwdBtn.performClick();", "" );

    VehicleEditorPlugin.map = %map;
initVehicleEdParamsData();
    //VehicleEditorPlugin.initSettings();
}
function execVehicleEd() {
    exec("./scripts/VehicleEditor.cs");
    exec("./scripts/VehicleEditorHints.cs");
    exec("./scripts/VehicleEditorActions.cs");
    
    
   exec("tlab/VehicleEditor/scripts/VehicleData.cs");
   exec("tlab/VehicleEditor/scripts/VehicleView.cs");
   exec("tlab/VehicleEditor/scripts/VehicleEdNodes.cs");
   exec("tlab/VehicleEditor/scripts/VehicleEdMaterials.cs");
   exec("tlab/VehicleEditor/scripts/VehicleEdMeshes.cs");
   exec("tlab/VehicleEditor/scripts/VehicleEdCollisions.cs");
   exec("tlab/VehicleEditor/scripts/VehicleEdMounts.cs");   
   
   
    exec("tlab/VehicleEditor/VehicleEditorPlugin.cs");
    exec("tlab/VehicleEditor/VehicleEditorParams.cs");
    
    
   
}

function initVehicleEdParamsData() {
    exec("tlab/VehicleEditor/dataParams/WheeledVehicleData.cs");
    VehicleEditorPlugin.initWheeledVehicleDataParams();
}


   
function destroyVehicleEditor() {
}

function SetToggleButtonValue(%ctrl, %value) {
    if ( %ctrl.getValue() != %value )
        %ctrl.performClick();
}

function VehicleEditorWireframeMode() {
    $gfx::wireframe = !$gfx::wireframe;
    VehicleEditorToolbar-->wireframeMode.setStateOn($gfx::wireframe);
}
