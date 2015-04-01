//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//------------------------------------------------------------------------------
// TerrainEditorPlugin
//------------------------------------------------------------------------------

function TerrainEditorPlugin::onWorldEditorStartup( %this ) {
    Parent::onWorldEditorStartup( %this );


    %map = new ActionMap();
    %map.bindCmd( keyboard, "1", "EWToolsPaletteArray->brushAdjustHeight.performClick();", "" );    //Grab Terrain
    %map.bindCmd( keyboard, "2", "EWToolsPaletteArray->raiseHeight.performClick();", "" );     // Raise Height
    %map.bindCmd( keyboard, "3", "EWToolsPaletteArray->lowerHeight.performClick();", "" );     // Lower Height
    %map.bindCmd( keyboard, "4", "EWToolsPaletteArray->smoothHeight.performClick();", "" );    // Average Height
    %map.bindCmd( keyboard, "5", "EWToolsPaletteArray->smoothSlope.performClick();", "" );    // Smooth Slope
    %map.bindCmd( keyboard, "6", "EWToolsPaletteArray->paintNoise.performClick();", "" );      // Noise
    %map.bindCmd( keyboard, "7", "EWToolsPaletteArray->flattenHeight.performClick();", "" );   // Flatten
    %map.bindCmd( keyboard, "8", "EWToolsPaletteArray->setHeight.performClick();", "" );       // Set Height
    %map.bindCmd( keyboard, "9", "EWToolsPaletteArray->setEmpty.performClick();", "" );    // Clear Terrain
    %map.bindCmd( keyboard, "0", "EWToolsPaletteArray->clearEmpty.performClick();", "" );  // Restore Terrain
    %map.bindCmd( keyboard, "v", "EWTerrainEditToolbarBrushType->ellipse.performClick();", "" );// Circle Brush
    %map.bindCmd( keyboard, "b", "EWTerrainEditToolbarBrushType->box.performClick();", "" );// Box Brush
    %map.bindCmd( keyboard, "=", "TerrainEditorPlugin.keyboardModifyBrushSize(1);", "" );// +1 Brush Size
    %map.bindCmd( keyboard, "+", "TerrainEditorPlugin.keyboardModifyBrushSize(1);", "" );// +1 Brush Size
    %map.bindCmd( keyboard, "-", "TerrainEditorPlugin.keyboardModifyBrushSize(-1);", "" );// -1 Brush Size
    %map.bindCmd( keyboard, "[", "TerrainEditorPlugin.keyboardModifyBrushSize(-5);", "" );// -5 Brush Size
    %map.bindCmd( keyboard, "]", "TerrainEditorPlugin.keyboardModifyBrushSize(5);", "" );// +5 Brush Size
    /*%map.bindCmd( keyboard, "]", "TerrainBrushPressureTextEditContainer->textEdit.text += 5", "" );// +5 Pressure
    %map.bindCmd( keyboard, "[", "TerrainBrushPressureTextEditContainer->textEdit.text -= 5", "" );// -5 Pressure
    %map.bindCmd( keyboard, "'", "TerrainBrushSoftnessTextEditContainer->textEdit.text += 5", "" );// +5 Softness
    %map.bindCmd( keyboard, ";", "TerrainBrushSoftnessTextEditContainer->textEdit.text -= 5", "" );// -5 Softness*/

    TerrainEditorPlugin.map = %map;

    Lab.terrainMenu = new PopupMenu() {
        superClass = "MenuBuilder";

        barTitle = "Terrain";

        item[0] = "Smooth Heightmap" TAB "" TAB "ETerrainEditor.onSmoothHeightmap();";
    };
}

function TerrainEditorPlugin::onActivated( %this ) {
    Parent::onActivated( %this );

  

   
    ETerrainEditor.switchAction( %action );
    EWToolsPaletteArray.findObjectByInternalName( %action, true ).setStateOn( true );

    EWTerrainEditToolbarBrushType->ellipse.performClick(); // Circle Brush

    //Lab.menuBar.insert( , Lab.menuBar.dynamicItemInsertPos );
// Add our menu.
    Lab.insertDynamicMenu(Lab.terrainMenu);
    EditorGui.bringToFront( ETerrainEditor );
    ETerrainEditor.setVisible( true );
    ETerrainEditor.attachTerrain();
    ETerrainEditor.makeFirstResponder( true );

    EWTerrainEditToolbar.setVisible( true );
    ETerrainEditor.onBrushChanged();
    ETerrainEditor.setup();
    TerrainEditorPlugin.syncBrushInfo();

    EditorGuiStatusBar.setSelection("");
    %this.map.push();
}

function TerrainEditorPlugin::onDeactivated( %this ) {
    Parent::onDeactivated( %this );

    endToolTime("TerrainEditor");
    

    EWTerrainEditToolbar.setVisible( false );
    ETerrainEditor.setVisible( false );

   Lab.removeDynamicMenu(Lab.terrainMenu);
 

    %this.map.pop();
}

function TerrainEditorPlugin::syncBrushInfo( %this ) {
    // Update gui brush info
    TerrainBrushSizeTextEditContainer-->textEdit.text = getWord(ETerrainEditor.getBrushSize(), 0);
    TerrainBrushPressureTextEditContainer-->textEdit.text = ETerrainEditor.getBrushPressure()*100;
    TerrainBrushSoftnessTextEditContainer-->textEdit.text = ETerrainEditor.getBrushSoftness()*100;
    TerrainSetHeightTextEditContainer-->textEdit.text = ETerrainEditor.setHeightVal;

    %brushType = ETerrainEditor.getBrushType();
    eval( "EWTerrainEditToolbar-->" @ %brushType @ ".setStateOn(1);" );
}

function TerrainEditorPlugin::validateBrushSize( %this ) {
    %minBrushSize = 1;
    %maxBrushSize = getWord(ETerrainEditor.maxBrushSize, 0);

    %val = $ThisControl.getText();
    if(%val < %minBrushSize)
        $ThisControl.setValue(%minBrushSize);
    else if(%val > %maxBrushSize)
        $ThisControl.setValue(%maxBrushSize);
}

function TerrainEditorPlugin::keyboardModifyBrushSize( %this, %amt) {
    %val = TerrainBrushSizeTextEditContainer-->textEdit.getText();
    %val += %amt;
    TerrainBrushSizeTextEditContainer-->textEdit.setValue(%val);
    TerrainBrushSizeTextEditContainer-->textEdit.forceValidateText();
    ETerrainEditor.setBrushSize( TerrainBrushSizeTextEditContainer-->textEdit.getText() );
}

//------------------------------------------------------------------------------
// TerrainTextureEditorTool
//------------------------------------------------------------------------------

function TerrainTextureEditorTool::onActivated( %this ) {
    EditorGui.bringToFront( ETerrainEditor );
    ETerrainEditor.setVisible( true );
    ETerrainEditor.attachTerrain();
    ETerrainEditor.makeFirstResponder( true );

    EditorGui-->TextureEditor.setVisible(true);

    EditorGuiStatusBar.setSelection("");
}

function TerrainTextureEditorTool::onDeactivated( %this ) {
    EditorGui-->TextureEditor.setVisible(false);

    ETerrainEditor.setVisible( false );
}

