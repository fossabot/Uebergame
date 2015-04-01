//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::createUndo( %this, %class, %desc ) {
    pushInstantGroup();
    %action = new UndoScriptAction() {
        class = %class;
        superClass = BaseLabPhysicAction;
        actionName = %desc;
        editor = LabPhysicPlugin;
        treeview = LabPhysicTree;
        //inspector = LabPhysicInspector;
    };
    popInstantGroup();
    return %action;
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::submitUndo( %this, %action ) {
    %action.addToManager( Editor.getUndoManager() );
}

//=============================================================================================
//    BaseLabPhysicAction.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function BaseLabPhysicAction::redo( %this ) {
}

//---------------------------------------------------------------------------------------------

function BaseLabPhysicAction::undo( %this ) {
}

//=============================================================================================
//    ActionCreatePhysicData.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionCreatePhysicData::redo( %this ) {
    %db = %this.db;

    %db.name = %this.dbName;

    %this.editor.PM.setDirty( %db, %this.fname );
    %this.editor.addExistingItem( %db );
    %this.editor.selectDatablock( %db );
    %this.editor.flagInspectorAsDirty( true );

    UnlistedDatablocks.remove( %id );
}

//---------------------------------------------------------------------------------------------

function ActionCreatePhysicData::undo( %this ) {
    %db = %this.db;

    %itemId = %this.treeview.findItemByName( %db.name );
    if( !%itemId )
        %itemId = %this.treeview.findItemByName( %db.name @ " *" );

    %this.treeview.removeItem( %itemId );
    %this.editor.resetSelectedDatablock();
    %this.editor.PM.removeDirty( %db );

    %this.dbName = %db.name;
    %db.name = "";

    UnlistedDatablocks.add( %this.db );
}

//=============================================================================================
//    ActionDeletePhysicData.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function ActionDeletePhysicData::redo( %this ) {
    %db = %this.db;

    %itemId = %this.treeview.findItemByName( %db.name );
    if( !%itemId )
        %itemId = %this.treeview.findItemByName( %db.name @ " *" );

    // Remove from tree and file.

    %this.treeview.removeItem( %db );
    %this.editor.resetSelectedDatablock();
    if( %db.getFileName() !$= "" )
        %this.editor.PM.removeObjectFromFile( %db );

    // Unassign name.

    %this.dbName = %db.name;
    %db.name = "";

    // Add to unlisted.

    UnlistedDatablocks.add( %db );
}

//---------------------------------------------------------------------------------------------

function ActionDeletePhysicData::undo( %this ) {
    %db = %this.db;

    // Restore name.

    %db.name = %this.dbName;

    // Add to tree and select.

    %this.editor.addExistingItem( %db, true );
    %this.editor.selectDatablock( %db );

    // Mark as dirty.

    %this.editor.PM.setDirty( %db, %this.fname );
    %this.editor.syncDirtyState();

    // Remove from unlisted.

    UnlistedDatablocks.remove( %id );
}
