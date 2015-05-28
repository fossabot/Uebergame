//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

function initializeRiverEditor() {
    echo(" % - Initializing River Editor");

    exec( "./riverEditor.cs" );
    exec( "./gui/riverEditorGui.gui" );
    exec( "./gui/RiverEditorTools.gui" );
    exec( "./gui/riverEditorToolbar.gui" );
    exec( "./gui/riverEditorPalette.gui" );
    exec( "tlab/riverEditor/riverEditorGui.cs" );
    exec( "tlab/riverEditor/RiverEditorParams.cs" );
    exec( "tlab/riverEditor/RiverEditorPlugin.cs" );

    // Add ourselves to EditorGui, where all the other tools reside
    Lab.addPluginEditor("RiverEditor",RiverEditorGui);
    Lab.addPluginGui("RiverEditor",RiverEditorTools);
    Lab.addPluginToolbar("RiverEditor",RiverEditorToolbar);
    Lab.addPluginPalette("RiverEditor",   RiverEditorPalette);

    Lab.createPlugin("RiverEditor","River Editor");
    RiverEditorPlugin.editorGui = RiverEditorGui;


    %map = new ActionMap();
    %map.bindCmd( keyboard, "backspace", "RiverEditorGui.deleteNode();", "" );
    %map.bindCmd( keyboard, "1", "RiverEditorGui.prepSelectionMode();", "" );
    %map.bindCmd( keyboard, "2", "EWToolsPaletteArray->RiverEditorMoveMode.performClick();", "" );
    %map.bindCmd( keyboard, "3", "EWToolsPaletteArray->RiverEditorRotateMode.performClick();", "" );
    %map.bindCmd( keyboard, "4", "EWToolsPaletteArray->RiverEditorScaleMode.performClick();", "" );
    %map.bindCmd( keyboard, "5", "EWToolsPaletteArray->RiverEditorAddRiverMode.performClick();", "" );
    %map.bindCmd( keyboard, "=", "EWToolsPaletteArray->RiverEditorInsertPointMode.performClick();", "" );
    %map.bindCmd( keyboard, "numpadadd", "EWToolsPaletteArray->RiverEditorInsertPointMode.performClick();", "" );
    %map.bindCmd( keyboard, "-", "EWToolsPaletteArray->RiverEditorRemovePointMode.performClick();", "" );
    %map.bindCmd( keyboard, "numpadminus", "EWToolsPaletteArray->RiverEditorRemovePointMode.performClick();", "" );
    %map.bindCmd( keyboard, "z", "RiverEditorShowSplineBtn.performClick();", "" );
    %map.bindCmd( keyboard, "x", "RiverEditorWireframeBtn.performClick();", "" );
    %map.bindCmd( keyboard, "v", "RiverEditorShowRoadBtn.performClick();", "" );
    RiverEditorPlugin.map = %map;

    // RiverEditorPlugin.initSettings();
}

function destroyRiverEditor() {
}

