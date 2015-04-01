//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================



function ForestEditorPlugin::onWorldEditorStartup( %this ) {
    Parent::onWorldEditorStartup( %this );
    new PersistenceManager( ForestDataManager );

    %brushPath = "art/forest/brushes.cs";
    if ( !isFile( %brushPath ) )
        createPath( %brushPath );

    // This creates the ForestBrushGroup, all brushes, and elements.
    exec( %brushpath );

    if ( !isObject( ForestBrushGroup ) ) {
        new SimGroup( ForestBrushGroup );
        %this.showError = true;
    }

    ForestEditBrushTree.open( ForestBrushGroup );

    if ( !isObject( ForestItemDataSet ) )
        new SimSet( ForestItemDataSet );
   
    if ( !isObject( ForestMeshGroup ) )
        new SimGroup( ForestMeshGroup );
 
    ForestEditMeshTree.open( ForestItemDataSet );


    ForestEditTabBook.selectPage(0);
}

function ForestEditorPlugin::onWorldEditorShutdown( %this ) {
    if ( isObject( ForestBrushGroup ) )
        ForestBrushGroup.delete();
    if ( isObject( ForestDataManager ) )
        ForestDataManager.delete();
}

function ForestEditorPlugin::onActivated( %this ) {
    EditorGui.bringToFront( ForestEditorGui );
    ForestEditorGui.setVisible( true );
   
    ForestEditorGui.makeFirstResponder( true );
    //ForestEditToolbar.setVisible( true );

    %this.map.push();
    Parent::onActivated(%this);

    ForestEditBrushTree.open( ForestBrushGroup );
    ForestEditMeshTree.open( ForestItemDataSet );

    // Open the Brush tab.
    ForestEditTabBook.selectPage(0);

    // Sync the pallete button state
    %forestBrushSize = %this.getCfg("BrushSize");
    %this.previousBrushSize = ETerrainEditor.getBrushSize();
    ETerrainEditor.setBrushSize(%forestBrushSize);

    // And toolbar.
    %tool = ForestEditorGui.getActiveTool();
    if ( isObject( %tool ) )
        %tool.onActivated();

    if ( !isObject( %tool ) ) {
        ForestEditorPaintModeBtn.performClick();

        if ( ForestEditBrushTree.getItemCount() > 0 ) {
            ForestEditBrushTree.selectItem( 0, true );
        }
    } else if ( %tool == ForestTools->SelectionTool ) {
        %mode = GlobalGizmoProfile.mode;
        switch$ (%mode) {
        case "None":
            ForestEditorSelectModeBtn.performClick();
        case "Move":
            ForestEditorMoveModeBtn.performClick();
        case "Rotate":
            ForestEditorRotateModeBtn.performClick();
        case "Scale":
            ForestEditorScaleModeBtn.performClick();
        }
    } else if ( %tool == ForestTools->BrushTool ) {
        %mode = ForestTools->BrushTool.mode;
        switch$ (%mode) {
        case "Paint":
            ForestEditorPaintModeBtn.performClick();
        case "Erase":
            ForestEditorEraseModeBtn.performClick();
        case "EraseSelected":
            ForestEditorEraseSelectedModeBtn.performClick();
        }
    }

    if ( %this.showError )
        LabMsgOK( "Error", "Your art/forest folder does not contain a valid brushes.cs. Brushes you create will not be saved!" );
}

function ForestEditorPlugin::onDeactivated( %this ) {
    ForestEditorGui.setVisible( false );  

    ETerrainEditor.setBrushSize( this.previousBrushSize);
    
    %tool = ForestEditorGui.getActiveTool();
    if ( isObject( %tool ) )
        %tool.onDeactivated();

    // Also take this opportunity to save.
    ForestDataManager.saveDirty();

    %this.map.pop();

    Parent::onDeactivated(%this);
}

function ForestEditorPlugin::onEditorSleep( %this ) {
}
