//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorPlugin::onWorldEditorStartup( %this ) {
    Parent::onWorldEditorStartup( %this );


    // Add ourselves to the Editor Settings window
  
}

function RoadEditorPlugin::onActivated( %this ) {
    %this.readSettings();

	RoadEd_TabBook.selectPage(0);
	RoadEditorGui.clearRoadNodesData();
    EWToolsPaletteArray->RoadEditorAddRoadMode.performClick();
    EditorGui.bringToFront( RoadEditorGui );

    RoadEditorGui.setVisible( true );
    RoadEditorGui.makeFirstResponder( true );
    RoadEditorToolbar.setVisible( true );

    RoadEditorOptionsWindow.setVisible( true );
    RoadEditorTreeWindow.setVisible( true );

    RoadTreeView.open(ServerDecalRoadSet,true);

    %this.map.push();

    // Set the status bar here until all tool have been hooked up
    EditorGuiStatusBar.setInfo("Road editor.");
    EditorGuiStatusBar.setSelection("");

    Parent::onActivated(%this);
}

function RoadEditorPlugin::onDeactivated( %this ) {
    %this.writeSettings();

    RoadEditorGui.setVisible( false );
    RoadEditorToolbar.setVisible( false );
    RoadEditorOptionsWindow.setVisible( false );
    RoadEditorTreeWindow.setVisible( false );
    %this.map.pop();

    Parent::onDeactivated(%this);
}

function RoadEditorPlugin::onEditMenuSelect( %this, %editMenu ) {
    %hasSelection = false;

    if( isObject( RoadEditorGui.road ) )
        %hasSelection = true;

    %editMenu.enableItem( 3, false ); // Cut
    %editMenu.enableItem( 4, false ); // Copy
    %editMenu.enableItem( 5, false ); // Paste
    %editMenu.enableItem( 6, %hasSelection ); // Delete
    %editMenu.enableItem( 8, false ); // Deselect
}

function RoadEditorPlugin::handleDelete( %this ) {
    RoadEditorGui.onDeleteKey();
}

function RoadEditorPlugin::handleEscape( %this ) {
    return RoadEditorGui.onEscapePressed();
}

function RoadEditorPlugin::isDirty( %this ) {
    return RoadEditorGui.isDirty;
}

function RoadEditorPlugin::onSaveMission( %this, %missionFile ) {
    if( RoadEditorGui.isDirty ) {
        MissionGroup.save( %missionFile );
        RoadEditorGui.isDirty = false;
    }
}

function RoadEditorPlugin::setEditorFunction( %this ) {
    %terrainExists = parseMissionGroup( "TerrainBlock" );

    if( %terrainExists == false )
        LabMsgYesNoCancel("No Terrain","Would you like to create a New Terrain?", "Canvas.pushDialog(CreateNewTerrainGui);");

    return %terrainExists;
}
