//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function initializeRoadEditor() {
    echo( " - Initializing Road and Path Editor" );

    exec( "./roadEditor.cs" );
    exec( "./gui/roadEditorGui.gui" );
    exec( "./gui/RoadEditorTools.gui" );
    exec( "./gui/roadEditorToolbar.gui");
    exec( "./gui/roadEditorPalette.gui");
    exec( "./roadEditorGui.cs" );
    exec( "./RoadEditorPlugin.cs" );
    exec( "tlab/roadEditor/RoadEditorParams.cs" );
    Lab.addPluginEditor("RoadEditor",RoadEditorGui);
    Lab.addPluginGui("RoadEditor",RoadEditorTools);
    //Lab.addPluginGui("RoadEditor",RoadEditorOptionsWindow);
    //Lab.addPluginGui("RoadEditor",RoadEditorTreeWindow);
    Lab.addPluginToolbar("RoadEditor",RoadEditorToolbar);
    Lab.addPluginPalette("RoadEditor",   RoadEditorPalette);

    Lab.createPlugin("RoadEditor");
    RoadEditorPlugin.editorGui = RoadEditorGui;


    %map = new ActionMap();
    %map.bindCmd( keyboard, "backspace", "RoadEditorGui.onDeleteKey();", "" );
    %map.bindCmd( keyboard, "1", "RoadEditorGui.prepSelectionMode();", "" );
    %map.bindCmd( keyboard, "2", "EWToolsPaletteArray->RoadEditorMoveMode.performClick();", "" );
    %map.bindCmd( keyboard, "4", "EWToolsPaletteArray->RoadEditorScaleMode.performClick();", "" );
    %map.bindCmd( keyboard, "5", "EWToolsPaletteArray->RoadEditorAddRoadMode.performClick();", "" );
    %map.bindCmd( keyboard, "=", "EWToolsPaletteArray->RoadEditorInsertPointMode.performClick();", "" );
    %map.bindCmd( keyboard, "numpadadd", "EWToolsPaletteArray->RoadEditorInsertPointMode.performClick();", "" );
    %map.bindCmd( keyboard, "-", "EWToolsPaletteArray->RoadEditorRemovePointMode.performClick();", "" );
    %map.bindCmd( keyboard, "numpadminus", "EWToolsPaletteArray->RoadEditorRemovePointMode.performClick();", "" );
    %map.bindCmd( keyboard, "z", "RoadEditorShowSplineBtn.performClick();", "" );
    %map.bindCmd( keyboard, "x", "RoadEditorWireframeBtn.performClick();", "" );
    %map.bindCmd( keyboard, "v", "RoadEditorShowRoadBtn.performClick();", "" );
    RoadEditorPlugin.map = %map;

    //RoadEditorPlugin.initSettings();
}

function destroyRoadEditor() {
}
