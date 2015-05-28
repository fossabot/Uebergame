//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function DecalEditorPlugin::onWorldEditorStartup( %this ) {
    Parent::onWorldEditorStartup( %this );


    //set initial palette setting
    %this.paletteSelection = "AddDecalMode";
}

function DecalEditorPlugin::onActivated( %this ) {
    EditorGui.bringToFront( DecalEditorGui );  
    DecalEditorGui.makeFirstResponder( true );
    

    %this.map.push();

    //WORKAROUND: due to the gizmo mode being stored on its profile (which may be shared),
    //  we may end up with a mismatch between the editor mode and gizmo mode here.
    //  Reset mode explicitly here to work around this.
    DecalEditorGui.setMode( DecalEditorGui.getMode() );

    // Set the current palette selection
    DecalEditorGui.paletteSync( %this.paletteSelection );

    // Store this on a dynamic field
    // in order to restore whatever setting
    // the user had before.
    %this.prevGizmoAlignment = GlobalGizmoProfile.alignment;

    // The DecalEditor always uses Object alignment.
    GlobalGizmoProfile.alignment = "Object";

    DecalEditorGui.rebuildInstanceTree();

    // These could perhaps be the node details like the shape editor
    //ShapeEdPropWindow.syncNodeDetails(-1);

    Parent::onActivated(%this);
}

function DecalEditorPlugin::onDeactivated( %this ) {

    %this.map.pop();

    // Remember last palette selection
    %this.paletteSelection = DecalEditorGui.getMode();

    // Restore the previous Gizmo
    // alignment settings.
    GlobalGizmoProfile.alignment = %this.prevGizmoAlignment;

    Parent::onDeactivated(%this);
}

function DecalEditorPlugin::isDirty( %this ) {
    %dirty = DecalPMan.hasDirty();

    %dirty |= decalManagerDirty();

    return %dirty;
}

function DecalEditorPlugin::onSaveMission( %this, %file ) {
    DecalPMan.saveDirty();
    decalManagerSave( %file @ ".decals" );
}

function DecalEditorPlugin::onEditMenuSelect( %this, %editMenu ) {
    %hasSelection = false;

    if ( DecalEditorGui.getSelectionCount() > 0 )
        %hasSelection = true;

    %editMenu.enableItem( 3, false ); // Cut
    %editMenu.enableItem( 4, false ); // Copy
    %editMenu.enableItem( 5, false ); // Paste
    %editMenu.enableItem( 6, %hasSelection ); // Delete
    %editMenu.enableItem( 8, false ); // Deselect

    // NOTE: If you want to implement Cut, Copy, Paste, or Deselect
    // for this editor simply enable the menu items when it is appropriate
    // and fill in the method stubs below.
}

function DecalEditorPlugin::handleDelete( %this ) {
    DecalEditorGui.deleteSelectedDecal();
}

function DecalEditorPlugin::handleDeselect( %this ) {
}

function DecalEditorPlugin::handleCut( %this ) {
}

function DecalEditorPlugin::handleCopy( %this ) {
}

function DecalEditorPlugin::handlePaste( %this ) {
}

function DecalEditorPlugin::handleEscape( %this ) {
}
