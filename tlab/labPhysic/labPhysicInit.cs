//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

// Main code for the Datablock Editor plugin.


$DATABLOCK_EDITOR_DEFAULT_FILENAME = "art/datablocks/managedDatablocks.cs";

//==============================================================================
//    Initialization.
//==============================================================================

//==============================================================================
function LabPhysicPlugin::init( %this ) {
    %this.update();
    if( !LabPhysicTree.getItemCount() )
        %this.populateTrees();
}
//------------------------------------------------------------------------------
//==============================================================================
function LabPhysicPlugin::update( %this ) {
    //LabPhysicInspectorWindow.position = getWord($pref::Video::mode, 0) - 250 SPC getWord(EditorGuiToolbar.extent, 1) + getWord(LabPhysicTreeWindow.extent, 1) - 2;
    //LabPhysicTreeWindow.position = getWord($pref::Video::mode, 0) - 250 SPC getWord(EditorGuiToolbar.extent, 1) - 1;
    //LabPhysicTreeWindow.extent = "250 400";
}
//------------------------------------------------------------------------------

function LabPhysicPlugin::initDefaultSettings( %this ) {
    Lab.addDefaultSetting("SimulationSpeed",2);
    Lab.addDefaultSetting("AddObjectRighClick","1");
}

//==============================================================================
// WorldEditor Open/Close Callbacks
//==============================================================================

//==============================================================================
function LabPhysicPlugin::onWorldEditorStartup( %this ) {


    Parent::onWorldEditorStartup( %this );



}
//------------------------------------------------------------------------------
//==============================================================================
function LabPhysicPlugin::onWorldEditorShutdown( %this ) {

}
//------------------------------------------------------------------------------
//==============================================================================
// Plugins Activated/Desactivated
//==============================================================================
//==============================================================================
function LabPhysicPlugin::onActivated( %this ) {


    show(LabPhysic_GuiGroup);
    EditorGui-->SceneEditorToolbar.setVisible(false);
    EditorGui.bringToFront( LabPhysicPlugin );

    LabPhysicTreeWindow.setVisible( true );
    LabPhysicInspectorWindow.setVisible( true );
    LabPhysicInspectorWindow.makeFirstResponder( true );

    hide(LabForestToolbar);

    %this.map.push();
    //Lab.setEditor( LabPhysicPlugin );
    // Set the status bar here until all tool have been hooked up
    EditorGuiStatusBar.setInfo( "Physic Lab." );

    %numSelected = %this.getNumSelectedDatablocks();
    if( !%numSelected )
        EditorGuiStatusBar.setSelection( "" );
    else
        EditorGuiStatusBar.setSelection( %numSelected @ " datablocks selected" );

    %this.init();
    LabPhysicPlugin.readSettings();

    if( EWorldEditor.getSelectionSize() == 1 )
        %this.onObjectSelected( EWorldEditor.getSelectedObject( 0 ) );

    Parent::onActivated( %this );

    Lab.GetMissionPhysicsShapes();
}
//------------------------------------------------------------------------------
//==============================================================================
function LabPhysicPlugin::onDeactivated( %this ) {
   // LabPhysicPlugin.writeSettings();
    %this.map.pop();
    Parent::onDeactivated(%this);
}
//------------------------------------------------------------------------------

//==============================================================================
// Mission Exit
//==============================================================================

//==============================================================================
function LabPhysicPlugin::onExitMission( %this ) {
    LabPhysicTree.clear();
    // LabPhysicInspector.inspect( "" );
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::openDatablock( %this, %datablock ) {
    Lab.setEditor( %this );
    %this.selectDatablock( %datablock );
    LabPhysicTreeTabBook.selectedPage = 0;
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::setEditorFunction( %this ) {
    return true;
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::onObjectSelected( %this, %object ) {
    // Select datablock of object if this is a GameBase object.

    if( %object.isMemberOfClass( "GameBase" ) )
        %this.selectDatablock( %object.getDatablock() );
    else if( %object.isMemberOfClass( "SFXEmitter" ) && isObject( %object.track ) )
        %this.selectDatablock( %object.track );
    else if( %object.isMemberOfClass( "LightBase" ) && isObject( %object.animationType ) )
        %this.selectDatablock( %object.animationType );
}

//---------------------------------------------------------------------------------------------
//LabPhysicPlugin.populateTrees();
function LabPhysicPlugin::populateTrees(%this) {
    LabPhysicTree.populateTree();

}

//---------------------------------------------------------------------------------------------


//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::isExcludedDatablockType( %this, %className ) {
    switch$( %className ) {
    case "SimDatablock":
        return true;
    case "SFXTrack": // Abstract.
        return true;
    case "SFXFMODEvent": // Internally created.
        return true;
    case "SFXFMODEventGroup": // Internally created.
        return true;
    }
    return false;
}

//=============================================================================================
//    Settings.
//=============================================================================================

//---------------------------------------------------------------------------------------------

//=============================================================================================
//    Persistence.
//=============================================================================================

//---------------------------------------------------------------------------------------------

//- Return true if there is any datablock with unsaved changes.
function LabPhysicPlugin::isDirty( %this ) {
    return %this.PM.hasDirty();
}

//---------------------------------------------------------------------------------------------

//- Return true if any of the currently selected datablocks has unsaved changes.
function LabPhysicPlugin::selectedDatablockIsDirty( %this ) {
    %tree = LabPhysicTree;

    %count = %tree.getSelectedItemsCount();
    %selected = %tree.getSelectedItemList();

    foreach$( %id in %selected ) {
        %db = %tree.getItemValue( %id );
        if( %this.PM.isDirty( %db ) )
            return true;
    }

    return false;
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::syncDirtyState( %this ) {
    %tree = LabPhysicTree;

    %count = %tree.getSelectedItemsCount();
    %selected = %tree.getSelectedItemList();
    %haveDirty = false;

    foreach$( %id in %selected ) {
        %db = %tree.getItemValue( %id );
        if( %this.PM.isDirty( %db ) ) {
            %this.flagDatablockAsDirty( %db, true );
            %haveDirty = true;
        } else
            %this.flagInspectorAsDirty( %db, false );
    }

    %this.flagInspectorAsDirty( %haveDirty );
}

//---------------------------------------------------------------------------------------------

//-
function LabPhysicPlugin::flagInspectorAsDirty( %this, %dirty ) {
    if( %dirty )
        LabPhysicInspectorWindow.text = "Datablock *";
    else
        LabPhysicInspectorWindow.text = "Datablock";
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::flagDatablockAsDirty(%this, %datablock, %dirty ) {
    %tree = LabPhysicTree;

    %id = %tree.findItemByValue( %datablock.getId() );
    if( %id == 0 )
        return;

    // Tag the item caption and sync the persistence manager.

    if( %dirty ) {
        LabPhysicTree.editItem( %id, %datablock.getName() @ " *", %datablock.getId() );
        %this.PM.setDirty( %datablock );
    } else {
        LabPhysicTree.editItem( %id, %datablock.getName(), %datablock.getId() );
        %this.PM.removeDirty( %datablock );
    }

    // Sync the inspector dirty state.

    %this.flagInspectorAsDirty( %this.PM.hasDirty() );
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::showSaveNewFileDialog(%this) {
    %currentFile = %this.getSelectedDatablock().getFilename();
    getSaveFilename( "TorqueScript Files|*.cs|All Files|*.*", %this @ ".saveNewFileFinish", %currentFile, false );
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::saveNewFileFinish( %this, %newFileName ) {
    // Clear the first responder to capture any inspector changes
    %ctrl = canvas.getFirstResponder();
    if( isObject(%ctrl) )
        %ctrl.clearFirstResponder();

    %tree = LabPhysicTree;
    %count = %tree.getSelectedItemsCount();
    %selected = %tree.getSelectedItemList();

    foreach$( %id in %selected ) {
        %db = %tree.getItemValue( %id );
        %db = %this.getSelectedDatablock();

        // Remove from current file.

        %oldFileName = %db.getFileName();
        if( %oldFileName !$= "" )
            %this.PM.removeObjectFromFile( %db, %oldFileName );

        // Save to new file.

        %this.PM.setDirty( %db, %newFileName );
        if( %this.PM.saveDirtyObject( %db ) ) {
            // Clear dirty state.

            %this.flagDatablockAsDirty( %db, false );
        }
    }

    LabPhysicInspectorWindow-->DatablockFile.setText( %newFileName );
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::save( %this ) {
    // Clear the first responder to capture any inspector changes
    %ctrl = canvas.getFirstResponder();
    if( isObject(%ctrl) )
        %ctrl.clearFirstResponder();

    %tree = LabPhysicTree;
    %count = %tree.getSelectedItemsCount();
    %selected = %tree.getSelectedItemList();

    for( %i = 0; %i < %count; %i ++ ) {
        %id = getWord( %selected, %i );
        %db = %tree.getItemValue( %id );

        if( %this.PM.isDirty( %db ) ) {
            %this.PM.saveDirtyObject( %db );
            %this.flagDatablockAsDirty( %db, false );
        }
    }
}

//=============================================================================================
//    Selection.
//=============================================================================================

//---------------------------------------------------------------------------------------------

$LabPhysicInspectFilters[1] = "+Object +Physics";
$LabPhysicInspectFilters[2] = "-Object";
$LabPhysicInspectFilters[3] = "+Physics";
$LabPhysicInspectFilters[4] = "Physics";
$LabPhysicInspectFilters[5] = "NOTHING +Physics";
function LabPhysicPlugin::setFilters( %this,%id ) {
   LabPhysicInspector.groupFilters = $LabPhysicInspectFilters[%id];
}

function LabPhysicPlugin::getNumSelectedDatablocks( %this ) {
    return LabPhysicTree.getSelectedItemsCount();
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::getSelectedDatablock( %this, %index ) {
    %tree = LabPhysicTree;
    if( !%tree.getSelectedItemsCount() )
        return 0;

    if( !%index )
        %id = %tree.getSelectedItem();
    else
        %id = getWord( %tree.getSelectedItemList(), %index );

    return %tree.getItemValue( %id );
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::resetSelectedDatablock( %this ) {
    LabPhysicTree.clearSelection();
    LabPhysicInspector.inspect(0);
    LabPhysicInspectorWindow-->DatablockFile.setText("");

    EditorGuiStatusBar.setSelection( "" );
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::selectDatablockCheck( %this, %datablock ) {
    if( %this.selectedDatablockIsDirty() )
        %this.showSaveDialog( %datablock );
    else
        %this.selectDatablock( %datablock );
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::selectDatablock( %this, %datablock, %add, %dontSyncTree ) {

if( %add )
        LabPhysicInspector.addInspect( %datablock );
    else
        LabPhysicInspector.inspect( %datablock );


    if( !%dontSyncTree ) {
        %id = LabPhysicTree.findItemByValue( %datablock.getId() );

        if( !%add )
            LabPhysicTree.clearSelection();

        LabPhysicTree.selectItem( %id, true );
        LabPhysicTree.scrollVisible( %id );
    }

    %this.syncDirtyState();

    // Update the filename text field.

    %numSelected = %this.getNumSelectedDatablocks();
    %fileNameField = LabPhysicInspectorWindow-->DatablockFile;

    if( %numSelected == 1 ) {
        %fileName = %datablock.getFilename();
        if( %fileName !$= "" )
            %fileNameField.setText( %fileName );
        else
            %fileNameField.setText( $DATABLOCK_EDITOR_DEFAULT_FILENAME );
    } else {
        %fileNameField.setText( "" );
    }

    EditorGuiStatusBar.setSelection( %this.getNumSelectedDatablocks() @ " Datablocks Selected" );

    Lab.syncLabPhysicInspectorParams();
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::unselectDatablock( %this, %datablock, %dontSyncTree ) {
    LabPhysicInspector.removeInspect( %datablock );

    if( !%dontSyncTree ) {
        %id = LabPhysicTree.findItemByValue( %datablock.getId() );
        LabPhysicTree.selectItem( %id, false );
    }

    %this.syncDirtyState();

    // If we have exactly one selected datablock remaining, re-enable
    // the save-as button.

    %numSelected = %this.getNumSelectedDatablocks();
    if( %numSelected == 1 ) {
        LabPhysicInspectorWindow-->saveAsButton.setActive( true );

        %fileNameField = LabPhysicInspectorWindow-->DatablockFile;
        %fileNameField.setText( %this.getSelectedDatablock().getFilename() );
        %fileNameField.setActive( true );
    }

    EditorGuiStatusBar.setSelection( %this.getNumSelectedDatablocks() @ " Datablocks Selected" );
}

//=============================================================================================
//    Creation and Deletion.
//=============================================================================================

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::deleteDatablock( %this ) {
    %tree = LabPhysicTree;

    // If we have more than single datablock selected,
    // turn our undos into a compound undo.

    %numSelected = %tree.getSelectedItemsCount();
    if( %numSelected > 1 )
        Editor.getUndoManager().pushCompound( "Delete Multiple Datablocks" );

    for( %i = 0; %i < %numSelected; %i ++ ) {
        %id = %tree.getSelectedItem( %i );
        %db = %tree.getItemValue( %id );

        %fileName = %db.getFileName();

        // Remove the datablock from the tree.

        LabPhysicTree.removeItem( %id );

        // Create undo.

        %action = %this.createUndo( ActionDeletePhysicData, "Delete Datablock" );
        %action.db = %db;
        %action.dbName = %db.getName();
        %action.fname = %fileName;

        %this.submitUndo( %action );

        // Kill the datablock in the file.

        if( %fileName !$= "" )
            %this.PM.removeObjectFromFile( %db );

        UnlistedDatablocks.add( %db );

        // Show some confirmation.

        if( %numSelected == 1 )
            LabMsgOK( "Datablock Deleted", "The datablock (" @ %db.getName() @ ") has been removed from " @
                           "it's file (" @ %db.getFilename() @ ") and upon restart will cease to exist" );
    }

    // Close compound, if we were deleting multiple datablocks.

    if( %numSelected > 1 )
        Editor.getUndoManager().popCompound();

    // Show confirmation for multiple datablocks.

    if( %numSelected > 1 )
        LabMsgOK( "Datablocks Deleted", "The datablocks have been deleted and upon restart will cease to exist." );

    // Clear selection.

    LabPhysicPlugin.resetSelectedDatablock();
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::createDatablock(%this) {
    %class = "PhysicShapeData";

    // Need to prompt for a name.

    LabPhysicCreatePrompt-->CreateDatablockName.setText("Name");
    LabPhysicCreatePrompt-->CreateDatablockName.selectAllText();

    // Populate the copy source dropdown.

    %list = LabPhysicCreatePrompt-->CopySourceDropdown;
    %list.clear();
    %list.add( "", 0 );

    %set = DataBlockSet;
    %count = %set.getCount();
    for( %i = 0; %i < %count; %i ++ ) {
        %datablock = %set.getObject( %i );
        %datablockClass = %datablock.getClassName();

        if( !isMemberOfClass( %datablockClass, %class ) )
            continue;

        %list.add( %datablock.getName(), %i + 1 );
    }

    // Set up state of client-side checkbox.

    %clientSideCheckBox = LabPhysicCreatePrompt-->ClientSideCheckBox;
    %canBeClientSide = LabPhysicPlugin::canBeClientSideDatablock( %class );
    %clientSideCheckBox.setStateOn( %canBeClientSide );
    %clientSideCheckBox.setActive( %canBeClientSide );

    // Show the dialog.

    canvas.pushDialog( LabPhysicCreatePrompt, 0, true );

}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::createPromptNameCheck(%this) {
    %name = LabPhysicCreatePrompt-->CreateDatablockName.getText();
    if( !Editor::validateObjectName( %name, true ) )
        return;

    // Fetch the copy source and clear the list.

    %copySource = LabPhysicCreatePrompt-->copySourceDropdown.getText();
    LabPhysicCreatePrompt-->copySourceDropdown.clear();

    // Remove the dialog and create the datablock.

    canvas.popDialog( LabPhysicCreatePrompt );
    %this.createDatablockFinish( %name, %copySource );
}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::createDatablockFinish( %this, %name, %copySource ) {
    %class = "PhysicShapeData";
    %action = %this.createUndo( ActionCreatePhysicData, "Create New Datablock" );

    if( LabPhysicCreatePrompt-->ClientSideCheckBox.isStateOn() )
        %dbType = "singleton ";
    else
        %dbType = "datablock ";

    if( %copySource !$= "" )
        %eval = %dbType @ %class @ "(" @ %name @ " : " @ %copySource @ ") { canSaveDynamicFields = \"1\"; };";
    else
        %eval = %dbType @ %class @ "(" @ %name @ ") { canSaveDynamicFields = \"1\"; };";

    %res = eval( %eval );

    %action.db = %name.getId();
    %action.dbName = %name;
    %action.fname = $DATABLOCK_EDITOR_DEFAULT_FILENAME;

    %this.submitUndo( %action );

    %action.redo();

}

//---------------------------------------------------------------------------------------------

function LabPhysicPlugin::canBeClientSideDatablock( %className ) {
    switch$( %className ) {
    case "SFXProfile" or
            "SFXPlayList" or
            "SFXAmbience" or
            "SFXEnvironment" or
            "SFXState" or
            "SFXDescription" or
            "SFXFMODProject":
        return true;

    default:
        return false;
    }
}
