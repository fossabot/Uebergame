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

// @todo:
//
// - split node transform editboxes into X Y Z and rot X Y Z with spin controls
//   to allow easier manual editing
// - add groundspeed editing ( use same format as node transform editing )
//
// Known bugs/limitations:
//
// - resizing the GuiTextListCtrl should resize the columns as well
// - modifying the from/in/out properties of a sequence will change the sequence
//   order in the shape ( since it results in remove/add sequence commands )
// - deleting a node should not delete its children as well?
//

//------------------------------------------------------------------------------
// Utility Methods
//------------------------------------------------------------------------------

if ( !isObject( VehicleEditor ) ) new ScriptObject( VehicleEditor ) {
    shape = -1;
    deletedCount = 0;
};


// Capitalise the first letter of the input string
function strcapitalise( %str ) {
    %len = strlen( %str );
    return strupr( getSubStr( %str,0,1 ) ) @ getSubStr( %str,1,%len-1 );
}

function VehicleEditor::getObjectShapeFile( %this, %obj ) {
    // Get the path to the shape file used by the given object (not perfect, but
    // works for the vast majority of object types)
    %path = "";
    if ( %obj.isMemberOfClass( "TSStatic" ) )
        %path = %obj.shapeName;
    else if ( %obj.isMemberOfClass( "PhysicsShape" ) )
        %path = %obj.getDataBlock().shapeName;
    else if ( %obj.isMemberOfClass( "GameBase" ) )
        %path = %obj.getDataBlock().shapeFile;

    return %path;
}

// Check if the given name already exists
function VehicleEditor::nameExists( %this, %type, %name ) {
    if ( VehicleEditor.shape == -1 )
        return false;

    if ( %type $= "node" )
        return ( VehicleEditor.shape.getNodeIndex( %name ) >= 0 );
    else if ( %type $= "sequence" )
        return ( VehicleEditor.shape.getSequenceIndex( %name ) >= 0 );
    else if ( %type $= "object" )
        return ( VehicleEditor.shape.getObjectIndex( %name ) >= 0 );
}

// Check if the given 'hint' name exists (spaces could also be underscores)
function VehicleEditor::hintNameExists( %this, %type, %name ) {
    if ( VehicleEditor.nameExists( %type, %name ) )
        return true;

    // If the name contains spaces, try replacing with underscores
    %name = strreplace( %name, " ", "_" );
    if ( VehicleEditor.nameExists( %type, %name ) )
        return true;

    return false;
}

// Generate a unique name from a given base by appending an integer
function VehicleEditor::getUniqueName( %this, %type, %name ) {
    for ( %idx = 1; %idx < 100; %idx++ ) {
        %uniqueName = %name @ %idx;
        if ( !%this.nameExists( %type, %uniqueName ) )
            break;
    }

    return %uniqueName;
}

function VehicleEditor::getProxyName( %this, %seqName ) {
    return "__proxy__" @ %seqName;
}

function VehicleEditor::getUnproxyName( %this, %proxyName ) {
    return strreplace( %proxyName, "__proxy__", "" );
}

function VehicleEditor::getBackupName( %this, %seqName ) {
    return "__backup__" @ %seqName;
}

// Check if this mesh name is a collision hint
function VehicleEditor::isCollisionMesh( %this, %name ) {
    return ( startswith( %name, "ColBox" ) ||
             startswith( %name, "ColSphere" ) ||
             startswith( %name, "ColCapsule" ) ||
             startswith( %name, "ColConvex" ) );
}

//
function VehicleEditor::getSequenceSource( %this, %seqName ) {
    %source = %this.shape.getSequenceSource( %seqName );

    // Use the sequence name as the source for DTS built-in sequences
    %src0 = getField( %source, 0 );
    %src1 = getField( %source, 1 );
    if ( %src0 $= %src1 )
        %source = setField( %source, 1, "" );
    if ( %src0 $= "" )
        %source = setField( %source, 0, %seqName );

    return %source;
}

// Recursively get names for a node and its children
function VehicleEditor::getNodeNames( %this, %nodeName, %names, %exclude ) {
    if ( %nodeName $= %exclude )
        return %names;

    %count = %this.shape.getNodeChildCount( %nodeName );
    for ( %i = 0; %i < %count; %i++ ) {
        %childName = %this.shape.getNodeChildName( %nodeName, %i );
        %names = %this.getNodeNames( %childName, %names, %exclude );
    }

    %names = %names TAB %nodeName;

    return trim( %names );
}

// Get the list of meshes for a particular object
function VehicleEditor::getObjectMeshList( %this, %name ) {
    %list = "";
    %count = %this.shape.getMeshCount( %name );
    for ( %i = 0; %i < %count; %i++ )
        %list = %list TAB %this.shape.getMeshName( %name, %i );
    return trim( %list );
}

// Get the list of meshes for a particular detail level
function VehicleEditor::getDetailMeshList( %this, %detSize ) {
    %list = "";
    %objCount = VehicleEditor.shape.getObjectCount();
    for ( %i = 0; %i < %objCount; %i++ ) {
        %objName = VehicleEditor.shape.getObjectName( %i );
        %meshCount = VehicleEditor.shape.getMeshCount( %objName );
        for ( %j = 0; %j < %meshCount; %j++ ) {
            %size = VehicleEditor.shape.getMeshSize( %objName, %j );
            if ( %size == %detSize )
                %list = %list TAB %this.shape.getMeshName( %objName, %j );
        }
    }
    return trim( %list );
}

function VehicleEditor::isDirty( %this ) {
    return ( isObject( %this.shape ) && VehicleEdPropWindow-->saveBtn.isActive() );
}

function VehicleEditor::setDirty( %this, %dirty ) {
    if ( %dirty )
        VehicleEdSelectWindow.text = "Shapes *";
    else
        VehicleEdSelectWindow.text = "Shapes";

    VehicleEdPropWindow-->saveBtn.setActive( %dirty );
}

function VehicleEditor::saveChanges( %this ) {
    if ( isObject( VehicleEditor.shape ) ) {
        VehicleEditor.saveConstructor( VehicleEditor.shape );
        VehicleEditor.shape.writeChangeSet();
        VehicleEditor.shape.notifyShapeChanged();      // Force game objects to reload shape
        VehicleEditor.setDirty( false );
    }
}

//------------------------------------------------------------------------------
// Shape Selection
//------------------------------------------------------------------------------

function VehicleEditor::findConstructor( %this, %path ) {
    %count = TSShapeConstructorGroup.getCount();
    for ( %i = 0; %i < %count; %i++ ) {
        %obj = TSShapeConstructorGroup.getObject( %i );
        if ( %obj.baseShape $= %path )
            return %obj;
    }
    return -1;
}

function VehicleEditor::createConstructor( %this, %path ) {
    %name = strcapitalise( fileBase( %path ) ) @ strcapitalise( getSubStr( fileExt( %path ), 1, 3 ) );
    %name = strreplace( %name, "-", "_" );
    %name = strreplace( %name, ".", "_" );
    %name = getUniqueName( %name );
    return new TSShapeConstructor( %name ) {
        baseShape = %path;
    };
}

function VehicleEditor::saveConstructor( %this, %constructor ) {
    %savepath = filePath( %constructor.baseShape ) @ "/" @ fileBase( %constructor.baseShape ) @ ".cs";
    new PersistenceManager( VehicleEd_perMan );
    VehicleEd_perMan.setDirty( %constructor, %savepath );
    VehicleEd_perMan.saveDirtyObject( %constructor );
    VehicleEd_perMan.delete();
}

// Handle a selection in the shape selector list
function VehicleEdSelectWindow::onSelect( %this, %path ) {
    // Prompt user to save the old shape if it is dirty
    if ( VehicleEditor.isDirty() ) {
        %cmd = "ColladaImportDlg.showDialog( \"" @ %path @ "\", \"VehicleEditor.selectShape( \\\"" @ %path @ "\\\", ";
        LabMsgYesNoCancel( "Shape Modified", "Would you like to save your changes?", %cmd @ "true );\" );", %cmd @ "false );\" );" );
    } else {
        %cmd = "VehicleEditor.selectShape( \"" @ %path @ "\", false );";
        ColladaImportDlg.showDialog( %path, %cmd );
    }
}

function VehicleEditor::selectShape( %this, %path, %saveOld ) {
    VehicleEdShapeView.setModel( "" );

    if ( %saveOld ) {
        // Save changes to a TSShapeConstructor script
        %this.saveChanges();
    } else if ( VehicleEditor.isDirty() ) {
        // Purge all unsaved changes
        %oldPath = VehicleEditor.shape.baseShape;
        VehicleEditor.shape.delete();
        VehicleEditor.shape = 0;

        reloadResource( %oldPath );   // Force game objects to reload shape
    }

    // Initialise the shape preview window
    if ( !VehicleEdShapeView.setModel( %path ) ) {
        LabMsgOK( "Error", "Failed to load '" @ %path @ "'. Check the console for error messages." );
        return;
    }
    VehicleEdShapeView.fitToShape();

    VehicleEdUndoManager.clearAll();
    VehicleEditor.setDirty( false );

    // Get ( or create ) the TSShapeConstructor object for this shape
    VehicleEditor.shape = VehicleEditor.findConstructor( %path );
    if ( VehicleEditor.shape <= 0 ) {
        VehicleEditor.shape = %this.createConstructor( %path );
        if ( VehicleEditor.shape <= 0 ) {
            error( "VehicleEditor: Error - could not select " @ %path );
            return;
        }
    }

    // Initialise the editor windows
    VehicleEdAdvancedWindow.update_onShapeSelectionChanged();
    VehicleEdMountWindow.update_onShapeSelectionChanged();
    VehicleEdThreadWindow.update_onShapeSelectionChanged();
    VehicleEdColWindow.update_onShapeSelectionChanged();
    VehicleEdPropWindow.update_onShapeSelectionChanged();
    VehicleEdShapeView.refreshShape();

    // Update object type hints
    VehicleEdSelectWindow.updateHints();

    // Update editor status bar
    EditorGuiStatusBar.setSelection( %path );
}


// Find all DTS or COLLADA models. Note: most of this section was shamelessly
// stolen from creater.ed.cs => great work whoever did the original!
function VehicleEdSelectWindow::navigate( %this, %address ) {
    // Freeze the icon array so it doesn't update until we've added all of the
    // icons
    %this-->shapeLibrary.frozen = true;
    %this-->shapeLibrary.clear();
    VehicleEdSelectMenu.clear();

    %filePatterns = "*.dts" TAB "*.dae" TAB "*.kmz";
    %fullPath = findFirstFileMultiExpr( %filePatterns );

    while ( %fullPath !$= "" ) {
        // Ignore cached DTS files
        if ( endswith( %fullPath, "cached.dts" ) ) {
            %fullPath = findNextFileMultiExpr( %filePatterns );
            continue;
        }

        // Ignore assets in the tools folder
        %fullPath = makeRelativePath( %fullPath, getMainDotCSDir() );
        %splitPath = strreplace( %fullPath, "/", " " );
        if ( getWord( %splitPath, 0 ) $= "tools" ) {
            %fullPath = findNextFileMultiExpr( %filePatterns );
            continue;
        }

        %dirCount = getWordCount( %splitPath ) - 1;
        %pathFolders = getWords( %splitPath, 0, %dirCount - 1 );

        // Add this file's path ( parent folders ) to the
        // popup menu if it isn't there yet.
        %temp = strreplace( %pathFolders, " ", "/" );
        %r = VehicleEdSelectMenu.findText( %temp );
        if ( %r == -1 )
            VehicleEdSelectMenu.add( %temp );

        // Is this file in the current folder?
        if ( stricmp( %pathFolders, %address ) == 0 ) {
            %this.addShapeIcon( %fullPath );
        }
        // Then is this file in a subfolder we need to add
        // a folder icon for?
        else {
            %wordIdx = 0;
            %add = false;

            if ( %address $= "" ) {
                %add = true;
                %wordIdx = 0;
            } else {
                for ( ; %wordIdx < %dirCount; %wordIdx++ ) {
                    %temp = getWords( %splitPath, 0, %wordIdx );
                    if ( stricmp( %temp, %address ) == 0 ) {
                        %add = true;
                        %wordIdx++;
                        break;
                    }
                }
            }

            if ( %add == true ) {
                %folder = getWord( %splitPath, %wordIdx );

                // Add folder icon if not already present
                %ctrl = %this.findIconCtrl( %folder );
                if ( %ctrl == -1 )
                    %this.addFolderIcon( %folder );
            }
        }

        %fullPath = findNextFileMultiExpr( %filePatterns );
    }

    %this-->shapeLibrary.sort( "alphaIconCompare" );
    for ( %i = 0; %i < %this-->shapeLibrary.getCount(); %i++ )
        %this-->shapeLibrary.getObject( %i ).autoSize = false;

    %this-->shapeLibrary.frozen = false;
    %this-->shapeLibrary.refresh();
    %this.address = %address;

    VehicleEdSelectMenu.sort();

    %str = strreplace( %address, " ", "/" );
    %r = VehicleEdSelectMenu.findText( %str );
    if ( %r != -1 )
        VehicleEdSelectMenu.setSelected( %r, false );
    else
        VehicleEdSelectMenu.setText( %str );
}

function VehicleEdSelectWindow::navigateDown( %this, %folder ) {
    if ( %this.address $= "" )
        %address = %folder;
    else
        %address = %this.address SPC %folder;

    // Because this is called from an IconButton::onClick command
    // we have to wait a tick before actually calling navigate, else
    // we would delete the button out from under itself.
    %this.schedule( 1, "navigate", %address );
}

function VehicleEdSelectWindow::navigateUp( %this ) {
    %count = getWordCount( %this.address );

    if ( %count == 0 )
        return;

    if ( %count == 1 )
        %address = "";
    else
        %address = getWords( %this.address, 0, %count - 2 );

    %this.navigate( %address );
}

function VehicleEdSelectWindow::findIconCtrl( %this, %name ) {
    for ( %i = 0; %i < %this-->shapeLibrary.getCount(); %i++ ) {
        %ctrl = %this-->shapeLibrary.getObject( %i );
        if ( %ctrl.text $= %name )
            return %ctrl;
    }
    return -1;
}

function VehicleEdSelectWindow::createIcon( %this ) {
    %ctrl = new GuiIconButtonCtrl() {
        profile = "GuiCreatorIconButtonProfile";
        iconLocation = "Left";
        textLocation = "Right";
        extent = "348 19";
        textMargin = 8;
        buttonMargin = "2 2";
        autoSize = false;
        sizeIconToButton = true;
        makeIconSquare = true;
        buttonType = "radioButton";
        groupNum = "-1";
    };

    return %ctrl;
}

function VehicleEdSelectWindow::addFolderIcon( %this, %text ) {
    %ctrl = %this.createIcon();

    %ctrl.altCommand = "VehicleEdSelectWindow.navigateDown( \"" @ %text @ "\" );";
    %ctrl.iconBitmap = "tlab/gui/icons/default/folder.png";
    %ctrl.text = %text;
    %ctrl.tooltip = %text;
    %ctrl.class = "CreatorFolderIconBtn";

    %ctrl.buttonType = "radioButton";
    %ctrl.groupNum = "-1";

    %this-->shapeLibrary.addGuiControl( %ctrl );
}

function VehicleEdSelectWindow::addShapeIcon( %this, %fullPath ) {
    %ctrl = %this.createIcon();

    %ext = fileExt( %fullPath );
    %file = fileBase( %fullPath );
    %fileLong = %file @ %ext;
    %tip = %fileLong NL
           "Size: " @ fileSize( %fullPath ) / 1000.0 SPC "KB" NL
           "Date Created: " @ fileCreatedTime( %fullPath ) NL
           "Last Modified: " @ fileModifiedTime( %fullPath );

    %ctrl.altCommand = "VehicleEdSelectWindow.onSelect( \"" @ %fullPath @ "\" );";
    %ctrl.iconBitmap = ( ( %ext $= ".dts" ) ? EditorIconRegistry::findIconByClassName( "TSStatic" ) : "tlab/gui/icons/default/iconCollada" );
    %ctrl.text = %file;
    %ctrl.class = "CreatorStaticIconBtn";
    %ctrl.tooltip = %tip;

    %ctrl.buttonType = "radioButton";
    %ctrl.groupNum = "-1";

    // Check if a shape specific icon is available
    %formats = ".png .jpg .dds .bmp .gif .jng .tga";
    %count = getWordCount( %formats );
    for ( %i = 0; %i < %count; %i++ ) {
        %ext = getWord( %formats, %i );
        if ( isFile( %fullPath @ %ext ) ) {
            %ctrl.iconBitmap = %fullPath @ %ext;
            break;
        }
    }

    %this-->shapeLibrary.addGuiControl( %ctrl );
}

function VehicleEdSelectMenu::onSelect( %this, %id, %text ) {
    %split = strreplace( %text, "/", " " );
    VehicleEdSelectWindow.navigate( %split );
}

// Update the GUI in response to the shape selection changing
function VehicleEdPropWindow::update_onShapeSelectionChanged( %this ) {
    // --- NODES TAB ---
    VehicleEdNodeTreeView.removeItem( 0 );
    %rootId = VehicleEdNodeTreeView.insertItem( 0, "<root>", 0, "" );
    %count = VehicleEditor.shape.getNodeCount();
    for ( %i = 0; %i < %count; %i++ ) {
        %name = VehicleEditor.shape.getNodeName( %i );
        if ( VehicleEditor.shape.getNodeParentName( %name ) $= "" )
            VehicleEdNodeTreeView.addNodeTree( %name );
    }
    %this.update_onNodeSelectionChanged( -1 );    // no node selected

    // --- SEQUENCES TAB ---
    VehicleEdSequenceList.clear();
    VehicleEdSequenceList.addRow( -1, "Name" TAB "Cyclic" TAB "Blend" TAB "Frames" TAB "Priority" );
    VehicleEdSequenceList.setRowActive( -1, false );
    VehicleEdSequenceList.addRow( 0, "<rootpose>" TAB "" TAB "" TAB "" TAB "" );

    %count = VehicleEditor.shape.getSequenceCount();
    for ( %i = 0; %i < %count; %i++ ) {
        %name = VehicleEditor.shape.getSequenceName( %i );

        // Ignore __backup__ sequences (only used by editor)
        if ( !startswith( %name, "__backup__" ) )
            VehicleEdSequenceList.addItem( %name );
    }
    VehicleEdThreadWindow.onAddThread();        // add thread 0

    // --- DETAILS TAB ---
    // Add detail levels and meshes to tree
    VehicleEdDetailTree.clearSelection();
    VehicleEdDetailTree.removeItem( 0 );
    %root = VehicleEdDetailTree.insertItem( 0, "<root>", "", "" );
    %objCount = VehicleEditor.shape.getObjectCount();
    for ( %i = 0; %i < %objCount; %i++ ) {
        %objName = VehicleEditor.shape.getObjectName( %i );
        %meshCount = VehicleEditor.shape.getMeshCount( %objName );
        for ( %j = 0; %j < %meshCount; %j++ ) {
            %meshName = VehicleEditor.shape.getMeshName( %objName, %j );
            VehicleEdDetailTree.addMeshEntry( %meshName, 1 );
        }
    }

    // Initialise object node list
    VehicleEdDetails-->objectNode.clear();
    VehicleEdDetails-->objectNode.add( "<root>" );
    %nodeCount = VehicleEditor.shape.getNodeCount();
    for ( %i = 0; %i < %nodeCount; %i++ )
        VehicleEdDetails-->objectNode.add( VehicleEditor.shape.getNodeName( %i ) );

    // --- MATERIALS TAB ---
    VehicleEdMaterials.updateMaterialList();
}

//------------------------------------------------------------------------------
// Shape Hints
//------------------------------------------------------------------------------

function VehicleEdHintMenu::onSelect( %this, %id, %text ) {
    VehicleEdSelectWindow.updateHints();
}

function VehicleEdSelectWindow::updateHints( %this ) {
    %objectType = VehicleEdHintMenu.getText();

    VehicleEdSelectWindow-->nodeHints.freeze( true );
    VehicleEdSelectWindow-->sequenceHints.freeze( true );

    // Move all current hint controls to a holder SimGroup
    for ( %i = VehicleEdSelectWindow-->nodeHints.getCount()-1; %i >= 0; %i-- )
        VehicleHintControls.add( VehicleEdSelectWindow-->nodeHints.getObject( %i ) );
    for ( %i = VehicleEdSelectWindow-->sequenceHints.getCount()-1; %i >= 0; %i-- )
        VehicleHintControls.add( VehicleEdSelectWindow-->sequenceHints.getObject( %i ) );

    // Update node and sequence hints, modifying and/or creating gui controls as needed
    for ( %i = 0; %i < VehicleHintGroup.getCount(); %i++ ) {
        %hint = VehicleHintGroup.getObject( %i );
        if ( ( %objectType $= %hint.objectType ) || isMemberOfClass( %objectType, %hint.objectType ) ) {
            for ( %idx = 0; %hint.node[%idx] !$= ""; %idx++ )
                VehicleEdHintMenu.processHint( "node", %hint.node[%idx] );

            for ( %idx = 0; %hint.sequence[%idx] !$= ""; %idx++ )
                VehicleEdHintMenu.processHint( "sequence", %hint.sequence[%idx] );
        }
    }

    VehicleEdSelectWindow-->nodeHints.freeze( false );
    VehicleEdSelectWindow-->nodeHints.updateStack();
    VehicleEdSelectWindow-->sequenceHints.freeze( false );
    VehicleEdSelectWindow-->sequenceHints.updateStack();

}

function VehicleEdHintMenu::processHint( %this, %type, %hint ) {
    %name = getField( %hint, 0 );
    %desc = getField( %hint, 1 );

    // check for arrayed names (ending in 0-N or 1-N)
    %pos = strstr( %name, "0-" );
    if ( %pos == -1 )
        %pos = strstr( %name, "1-" );

    if ( %pos > 0 ) {
        // arrayed name => add controls for each name in the array, but collapse
        // consecutive indices where possible. eg.  if the model only has nodes
        // mount1-3, we should create: mount0 (red), mount1-3 (green), mount4-31 (red)
        %base = getSubStr( %name, 0, %pos );      // array name
        %first = getSubStr( %name, %pos, 1 );     // first index
        %last = getSubStr( %name, %pos+2, 3 );    // last index

        // get the state of the first element
        %arrayStart = %first;
        %prevPresent = VehicleEditor.hintNameExists( %type, %base @ %first );

        for ( %j = %first + 1; %j <= %last; %j++ ) {
            // if the state of this element is different to the previous one, we
            // need to add a hint
            %present = VehicleEditor.hintNameExists( %type, %base @ %j );
            if ( %present != %prevPresent ) {
                VehicleEdSelectWindow.addObjectHint( %type, %base, %desc, %prevPresent, %arrayStart, %j-1 );
                %arrayStart = %j;
                %prevPresent = %present;
            }
        }

        // add hint for the last group
        VehicleEdSelectWindow.addObjectHint( %type, %base, %desc, %prevPresent, %arrayStart, %last );
    } else {
        // non-arrayed name
        %present = VehicleEditor.hintNameExists( %type, %name );
        VehicleEdSelectWindow.addObjectHint( %type, %name, %desc, %present );
    }
}

function VehicleEdSelectWindow::addObjectHint( %this, %type, %name, %desc, %present, %start, %end ) {
    // Get a hint gui control (create one if needed)
    if ( VehicleHintControls.getCount() == 0 ) {
        // Create a new hint gui control
        %ctrl = new GuiIconButtonCtrl() {
            profile = "GuiCreatorIconButtonProfile";
            iconLocation = "Left";
            textLocation = "Right";
            extent = "348 19";
            textMargin = 8;
            buttonMargin = "2 2";
            autoSize = true;
            buttonType = "radioButton";
            groupNum = "-1";
            iconBitmap = "tlab/editorClasses/gui/images/iconCancel";
            text = "hint";
            tooltip = "";
        };

        VehicleHintControls.add( %ctrl );
    }
    %ctrl = VehicleHintControls.getObject( 0 );

    // Initialise the control, then add it to the appropriate list
    %name = %name @ %start;
    if ( %end !$= %start )
        %ctrl.text = %name @ "-" @ %end;
    else
        %ctrl.text = %name;

    %ctrl.tooltip = %desc;
    %ctrl.setBitmap( "tlab/editorClasses/gui/images/" @ ( %present ? "iconAccept" : "iconCancel" ) );
    %ctrl.setStateOn( false );
    %ctrl.resetState();

    switch$ ( %type ) {
    case "node":
        %ctrl.altCommand = %present ? "" : "VehicleEdNodes.onAddNode( \"" @ %name @ "\" );";
        VehicleEdSelectWindow-->nodeHints.addGuiControl( %ctrl );
    case "sequence":
        %ctrl.altCommand = %present ? "" : "VehicleEdSequences.onAddSequence( \"" @ %name @ "\" );";
        VehicleEdSelectWindow-->sequenceHints.addGuiControl( %ctrl );
    }
}

//------------------------------------------------------------------------------

function VehicleEdSeqNodeTabBook::onTabSelected( %this, %name, %index ) {
    %this.activePage = %name;

    switch$ ( %name ) {
    case "Seq":
        VehicleEdPropWindow-->newBtn.ToolTip = "Add new sequence";
        VehicleEdPropWindow-->newBtn.Command = "VehicleEdSequences.onAddSequence();";
        VehicleEdPropWindow-->newBtn.setActive( true );
        VehicleEdPropWindow-->deleteBtn.ToolTip = "Delete selected sequence (cannot be undone)";
        VehicleEdPropWindow-->deleteBtn.Command = "VehicleEdSequences.onDeleteSequence();";
        VehicleEdPropWindow-->deleteBtn.setActive( true );

    case "Node":
        VehicleEdPropWindow-->newBtn.ToolTip = "Add new node";
        VehicleEdPropWindow-->newBtn.Command = "VehicleEdNodes.onAddNode();";
        VehicleEdPropWindow-->newBtn.setActive( true );
        VehicleEdPropWindow-->deleteBtn.ToolTip = "Delete selected node (cannot be undone)";
        VehicleEdPropWindow-->deleteBtn.Command = "VehicleEdNodes.onDeleteNode();";
        VehicleEdPropWindow-->deleteBtn.setActive( true );

    case "Detail":
        VehicleEdPropWindow-->newBtn.ToolTip = "";
        VehicleEdPropWindow-->newBtn.Command = "";
        VehicleEdPropWindow-->newBtn.setActive( false );
        VehicleEdPropWindow-->deleteBtn.ToolTip = "Delete the selected mesh or detail level (cannot be undone)";
        VehicleEdPropWindow-->deleteBtn.Command = "VehicleEdDetails.onDeleteMesh();";
        VehicleEdPropWindow-->deleteBtn.setActive( true );

    case "Mat":
        VehicleEdPropWindow-->newBtn.ToolTip = "";
        VehicleEdPropWindow-->newBtn.Command = "";
        VehicleEdPropWindow-->newBtn.setActive( false );
        VehicleEdPropWindow-->deleteBtn.ToolTip = "";
        VehicleEdPropWindow-->deleteBtn.Command = "";
        VehicleEdPropWindow-->deleteBtn.setActive( false );

        // For some reason, the header is not resized correctly until the Materials tab has been
        // displayed at least once, so resize it here too
        VehicleEdMaterials-->materialListHeader.setExtent( getWord( VehicleEdMaterialList.extent, 0 ) SPC "19" );
    }
}


//------------------------------------------------------------------------------
// Sequence Editing
//------------------------------------------------------------------------------

function VehicleEdPropWindow::onWake( %this ) {
    VehicleEdTriggerList.triggerId = 1;

    VehicleEdTriggerList.addRow( -1, "-1" TAB "Frame" TAB "Trigger" TAB "State" );
    VehicleEdTriggerList.setRowActive( -1, false );
}



//------------------------------------------------------------------------------
// Shape Preview
//------------------------------------------------------------------------------

function VehicleEditorPreview::updatePreviewBackground( %color ) {
    VehicleEditorPreview-->previewBackground.color = %color;
    VehicleEditorToolbar-->previewBackgroundPicker.color = %color;
}

function showVehicleEditorPreview() {
    %visible = VehicleEditorToolbar-->showPreview.getValue();
    VehicleEditorPreview.setVisible( %visible );
}
