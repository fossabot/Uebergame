//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//---------------------------------------------------------------------------------------------
//LabPhysicPlugin.populateTrees();
function LabPhysicTree::populateTree(%this) {
    // Populate datablock tree.

    if( LapPhysicPlugin.excludeClientOnlyDatablocks )
        %set = DataBlockGroup;
    else
        %set = DataBlockSet;

    %this.clear();

    foreach( %datablock in %set ) {
        %unlistedFound = false;
        %id = %datablock.getId();

        //Only interested in PhysicsShapeData
        if(%datablock.getClassName() !$="PhysicsShapeData") continue;

        foreach( %obj in UnlistedDatablocks )
            if( %obj.getId() == %id ) {
                %unlistedFound = true;
                break;
            }

        if( %unlistedFound )
            continue;

        %this.addExistingItem( %datablock, true );

    }

    %count = %this.getItemCount();
    if (%count < 1 ||%count $="") return;

    %this.sort( 0, true, false, false );

    // Populate datablock type tree.

    %classList = enumerateConsoleClasses( "SimDatablock" );

}

//---------------------------------------------------------------------------------------------

function LabPhysicTree::addExistingItem( %this, %datablock, %dontSort ) {


    // Look up class at root level.  Create if needed.

    %class = %datablock.getClassName();
    %parentID = %this.findItemByName( %class );
    if( %parentID == 0 )
        %parentID = %this.insertItem( 0, %class );

    // If the datablock is already there, don't
    // do anything.

    if( %this.findItemByValue( %datablock.getId() ) )
        return;

    // It doesn't exist so add it.

    %name = %datablock.getName();
    if( LabPhysicPlugin.PM.isDirty( %datablock ) )
        %name = %name @ " *";

    %id = %this.insertItem( %parentID, %name, %datablock.getId() );
    if( !%dontSort )
        %this.sort( %parentID, false, false, false );

    return %id;
}



function LabPhysicTree::onDeleteSelection( %this ) {
    %this.undoDeleteList = "";
}

//---------------------------------------------------------------------------------------------

function LabPhysicTree::onDeleteObject( %this, %object ) {
    // Append it to our list.
    %this.undoDeleteList = %this.undoDeleteList TAB %object;

    // We're gonna delete this ourselves in the
    // completion callback.
    return true;
}

//---------------------------------------------------------------------------------------------

function LabPhysicTree::onObjectDeleteCompleted( %this ) {
    //MEDeleteUndoAction::submit( %this.undoDeleteList );

    // Let the world editor know to
    // clear its selection.
    //EWorldEditor.clearSelection();
    //EWorldEditor.isDirty = true;
}

//---------------------------------------------------------------------------------------------

function LabPhysicTree::onClearSelected(%this) {
    //LabPhysicInspector.inspect( 0 );
}

//---------------------------------------------------------------------------------------------

function LabPhysicTree::onAddSelection( %this, %id ) {
    %obj = %this.getItemValue( %id );

    if( !isObject( %obj ) )
        %this.selectItem( %id, false );
    else
        LabPhysicPlugin.selectDatablock( %obj, true, true );
}

//---------------------------------------------------------------------------------------------

function LabPhysicTree::onRemoveSelection( %this, %id ) {
    %obj = %this.getItemValue( %id );
    if( isObject( %obj ) )
        LabPhysicPlugin.unselectDatablock( %obj, true );
}

//---------------------------------------------------------------------------------------------

function LabPhysicTree::onMouseUp(%this,%itemId,%clicks) {

    if (%clicks > 1) {
        LabPhysicPlugin.createShape();
    }
}
function LabPhysicTree::onRightMouseUp( %this, %id, %mousePos ) {
    %datablock = %this.getItemValue( %id );
    if( !isObject( %datablock ) )
        return;

    if( !isObject( LabPhysicTreePopup ) )
        new PopupMenu( LabPhysicTreePopup ) {
        superClass = "MenuBuilder";
        isPopup = true;

        item[ 0 ] = "Delete" TAB "" TAB "LabPhysicPlugin.selectDatablock( %this.datablockObject ); LabPhysicPlugin.deleteDatablock( %this.datablockObject );";
        item[ 1 ] = "Jump to Definition in Torsion" TAB "" TAB "EditorOpenDeclarationInTorsion( %this.datablockObject );";

        datablockObject = "";
    };

    LabPhysicTreePopup.datablockObject = %datablock;
    LabPhysicTreePopup.showPopup( Canvas );
}

//---------------------------------------------------------------------------------------------

function LabPhysicTreeTabBook::onTabSelected(%this, %text, %id) {
    switch(%id) {
    case 0:
        LabPhysicTreeWindow-->DeleteSelection.visible = true;
        LabPhysicTreeWindow-->CreateSelection.visible = false;

    case 1:
        LabPhysicTreeWindow-->DeleteSelection.visible = false;
        LabPhysicTreeWindow-->CreateSelection.visible = true;
    }
}
