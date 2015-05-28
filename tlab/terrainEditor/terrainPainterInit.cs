//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
// TerrainPainterPlugin
//------------------------------------------------------------------------------

function TerrainPainterPlugin::onWorldEditorStartup( %this ) {
    Parent::onWorldEditorStartup( %this );



    %map = new ActionMap();
    %map.bindCmd( keyboard, "v", "EWTerrainPainterToolbarBrushType->ellipse.performClick();", "" );// Circle Brush
    %map.bindCmd( keyboard, "b", "EWTerrainPainterToolbarBrushType->box.performClick();", "" );// Box Brush
    %map.bindCmd( keyboard, "=", "TerrainPainterPlugin.keyboardModifyBrushSize(1);", "" );// +1 Brush Size
    %map.bindCmd( keyboard, "+", "TerrainPainterPlugin.keyboardModifyBrushSize(1);", "" );// +1 Brush Size
    %map.bindCmd( keyboard, "-", "TerrainPainterPlugin.keyboardModifyBrushSize(-1);", "" );// -1 Brush Size
    %map.bindCmd( keyboard, "[", "TerrainPainterPlugin.keyboardModifyBrushSize(-5);", "" );// -5 Brush Size
    %map.bindCmd( keyboard, "]", "TerrainPainterPlugin.keyboardModifyBrushSize(5);", "" );// +5 Brush Size
    /*%map.bindCmd( keyboard, "]", "PaintBrushSlopeControl->SlopeMinAngle.text += 5", "" );// +5 SlopeMinAngle
    %map.bindCmd( keyboard, "[", "PaintBrushSlopeControl->SlopeMinAngle.text -= 5", "" );// -5 SlopeMinAngle
    %map.bindCmd( keyboard, "'", "PaintBrushSlopeControl->SlopeMaxAngle.text += 5", "" );// +5 SlopeMaxAngle
    %map.bindCmd( keyboard, ";", "PaintBrushSlopeControl->SlopeMaxAngle.text -= 5", "" );// -5 Softness*/

    for(%i=1; %i<10; %i++) {
        %map.bindCmd( keyboard, %i, "TerrainPainterPlugin.keyboardSetMaterial(" @ (%i-1) @ ");", "" );
    }
    %map.bindCmd( keyboard, 0, "TerrainPainterPlugin.keyboardSetMaterial(10);", "" );

    TerrainPainterPlugin.map = %map;

}

function TerrainPainterPlugin::onActivated( %this ) {
    Parent::onActivated( %this );

   

    EWTerrainPainterToolbarBrushType->ellipse.performClick();// Circle Brush
    %this.map.push();

    EditorGui.bringToFront( ETerrainEditor );
    ETerrainEditor.setVisible( true );
    ETerrainEditor.attachTerrain();
    ETerrainEditor.makeFirstResponder( true );

    EditorGui-->TerrainPainter.setVisible(true);
    EditorGui-->TerrainPainterPreview.setVisible(true);
    EWTerrainPainterToolbar.setVisible(true);
    ETerrainEditor.onBrushChanged();
    EPainter.setup();
    TerrainPainterPlugin.syncBrushInfo();

    EditorGuiStatusBar.setSelection("");
}

function TerrainPainterPlugin::onDeactivated( %this ) {
    Parent::onDeactivated( %this );

 

    %this.map.pop();
    //EditorGui-->TerrainPainter.setVisible(false);
    //EditorGui-->TerrainPainterPreview.setVisible(false);
    //EWTerrainPainterToolbar.setVisible(false);
    //ETerrainEditor.setVisible( false );
}

function TerrainPainterPlugin::syncBrushInfo( %this ) {
    // Update gui brush info
    PaintBrushSizeTextEditContainer-->textEdit.text = getWord(ETerrainEditor.getBrushSize(), 0);
    PaintBrushSlopeControl-->SlopeMinAngle.text = ETerrainEditor.getSlopeLimitMinAngle();
    PaintBrushSlopeControl-->SlopeMaxAngle.text = ETerrainEditor.getSlopeLimitMaxAngle();
    PaintBrushPressureTextEditContainer-->textEdit.text = ETerrainEditor.getBrushPressure()*100;
    %brushType = ETerrainEditor.getBrushType();
    eval( "EWTerrainPainterToolbar-->" @ %brushType @ ".setStateOn(1);" );
}



function TerrainPainterPlugin::keyboardModifyBrushSize( %this, %amt) {
    %val = PaintBrushSizeTextEditContainer-->textEdit.getText();
    %val += %amt;
    PaintBrushSizeTextEditContainer-->textEdit.setValue(%val);
    PaintBrushSizeTextEditContainer-->textEdit.forceValidateText();
    ETerrainEditor.setBrushSize( PaintBrushSizeTextEditContainer-->textEdit.getText() );
}

function TerrainPainterPlugin::keyboardSetMaterial( %this, %mat) {
    %name = "EPainterMaterialButton" @ %mat;
    %ctrl = EPainter.findObjectByInternalName(%name, true);
    if(%ctrl) {
        %ctrl.performClick();
    }
}
