//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
// Node Editing
//------------------------------------------------------------------------------

// Update the GUI in response to the node selection changing
function VehicleEdPropWindow::update_onNodeSelectionChanged( %this, %id ) {
    if ( %id > 0 ) {
        // Enable delete button and edit boxes
        if ( VehicleEdSeqNodeTabBook.activePage $= "Node" )
            VehicleEdPropWindow-->deleteBtn.setActive( true );
        VehicleEdNodes-->nodeName.setActive( true );
        VehicleEdNodes-->nodePosition.setActive( true );
        VehicleEdNodes-->nodeRotation.setActive( true );

        // Update the node inspection data
        %name = VehicleEdNodeTreeView.getItemText( %id );

        VehicleEdNodes-->nodeName.setText( %name );

        // Node parent list => ancestor and sibling nodes only (can't re-parent to a descendent)
        VehicleEdNodeParentMenu.clear();
        %parentNames = VehicleEditor.getNodeNames( "", "<root>", %name );
        %count = getWordCount( %parentNames );
        for ( %i = 0; %i < %count; %i++ )
            VehicleEdNodeParentMenu.add( getWord(%parentNames, %i), %i );

        %pName = VehicleEditor.shape.getNodeParentName( %name );
        if ( %pName $= "" )
            %pName = "<root>";
        VehicleEdNodeParentMenu.setText( %pName );

        if ( VehicleEdNodes-->worldTransform.getValue() ) {
            // Global transform
            %txfm = VehicleEditor.shape.getNodeTransform( %name, 1 );
            VehicleEdNodes-->nodePosition.setText( getWords( %txfm, 0, 2 ) );
            VehicleEdNodes-->nodeRotation.setText( getWords( %txfm, 3, 6 ) );
        } else {
            // Local transform (relative to parent)
            %txfm = VehicleEditor.shape.getNodeTransform( %name, 0 );
            VehicleEdNodes-->nodePosition.setText( getWords( %txfm, 0, 2 ) );
            VehicleEdNodes-->nodeRotation.setText( getWords( %txfm, 3, 6 ) );
        }

        VehicleEdShapeView.selectedNode = VehicleEditor.shape.getNodeIndex( %name );
    } else {
        // Disable delete button and edit boxes
        if ( VehicleEdSeqNodeTabBook.activePage $= "Node" )
            VehicleEdPropWindow-->deleteBtn.setActive( false );
        VehicleEdNodes-->nodeName.setActive( false );
        VehicleEdNodes-->nodePosition.setActive( false );
        VehicleEdNodes-->nodeRotation.setActive( false );

        VehicleEdNodes-->nodeName.setText( "" );
        VehicleEdNodes-->nodePosition.setText( "" );
        VehicleEdNodes-->nodeRotation.setText( "" );

        VehicleEdShapeView.selectedNode = -1;
    }
}

// Update the GUI in response to a node being added
function VehicleEdPropWindow::update_onNodeAdded( %this, %nodeName, %oldTreeIndex ) {
    // --- MISC ---
    VehicleEdShapeView.refreshShape();
    VehicleEdShapeView.updateNodeTransforms();
    VehicleEdSelectWindow.updateHints();

    // --- MOUNT WINDOW ---
    if ( VehicleEdMountWindow.isMountableNode( %nodeName ) ) {
        VehicleEdMountWindow-->mountNode.add( %nodeName );
        VehicleEdMountWindow-->mountNode.sort();
    }

    // --- NODES TAB ---
    %id = VehicleEdNodeTreeView.addNodeTree( %nodeName );
    if ( %oldTreeIndex <= 0 ) {
        // This is a new node => make it the current selection
        if ( %id > 0 ) {
            VehicleEdNodeTreeView.clearSelection();
            VehicleEdNodeTreeView.selectItem( %id );
        }
    } else {
        // This node has been un-deleted. Inserting a new item puts it at the
        // end of the siblings, but we want to restore the original order as
        // if the item was never deleted, so move it up as required.
        %childIndex = VehicleEdNodeTreeView.getChildIndexByName( %nodeName );
        while ( %childIndex > %oldTreeIndex ) {
            VehicleEdNodeTreeView.moveItemUp( %id );
            %childIndex--;
        }
    }

    // --- DETAILS TAB ---
    VehicleEdDetails-->objectNode.add( %nodeName );
}

// Update the GUI in response to a node(s) being removed
function VehicleEdPropWindow::update_onNodeRemoved( %this, %nameList, %nameCount ) {
    // --- MISC ---
    VehicleEdShapeView.refreshShape();
    VehicleEdShapeView.updateNodeTransforms();
    VehicleEdSelectWindow.updateHints();

    // Remove nodes from the mountable list, and any shapes mounted to the node
    for ( %i = 0; %i < %nameCount; %i++ ) {
        %nodeName = getField( %nameList, %i );
        VehicleEdMountWindow-->mountNode.clearEntry( VehicleEdMountWindow-->mountNode.findText( %nodeName ) );

        for ( %j = VehicleEdMountWindow-->mountList.rowCount()-1; %j >= 1; %j-- ) {
            %text = VehicleEdMountWindow-->mountList.getRowText( %j );
            if ( getField( %text, 1 ) $= %nodeName ) {
                VehicleEdShapeView.unmountShape( %j-1 );
                VehicleEdMountWindow-->mountList.removeRow( %j );
            }
        }
    }

    // --- NODES TAB ---
    %lastName = getField( %nameList, %nameCount-1 );
    %id = VehicleEdNodeTreeView.findItemByName( %lastName );   // only need to remove the parent item
    if ( %id > 0 ) {
        VehicleEdNodeTreeView.removeItem( %id );
        if ( VehicleEdNodeTreeView.getSelectedItem() <= 0 )
            VehicleEdPropWindow.update_onNodeSelectionChanged( -1 );
    }

    // --- DETAILS TAB ---
    for ( %i = 0; %i < %nameCount; %i++ ) {
        %nodeName = getField( %nameList, %i );
        VehicleEdDetails-->objectNode.clearEntry( VehicleEdDetails-->objectNode.findText( %nodeName ) );
    }
}

// Update the GUI in response to a node being renamed
function VehicleEdPropWindow::update_onNodeRenamed( %this, %oldName, %newName ) {
    // --- MISC ---
    VehicleEdSelectWindow.updateHints();

    // --- MOUNT WINDOW ---
    // Update entries for any shapes mounted to this node
    %rowCount = VehicleEdMountWindow-->mountList.rowCount();
    for ( %i = 1; %i < %rowCount; %i++ ) {
        %text = VehicleEdMountWindow-->mountList.getRowText( %i );
        if ( getField( %text, 1 ) $= %oldName ) {
            %text = setField( %text, 1, %newName );
            VehicleEdMountWindow-->mountList.setRowById( VehicleEdMountWindow-->mountList.getRowId( %i ), %text );
        }
    }

    // Update list of mountable nodes
    VehicleEdMountWindow-->mountNode.clearEntry( VehicleEdMountWindow-->mountNode.findText( %oldName ) );
    if ( VehicleEdMountWindow.isMountableNode( %newName ) ) {
        VehicleEdMountWindow-->mountNode.add( %newName );
        VehicleEdMountWindow-->mountNode.sort();
    }

    // --- NODES TAB ---
    %id = VehicleEdNodeTreeView.findItemByName( %oldName );
    VehicleEdNodeTreeView.editItem( %id, %newName, 0 );
    if ( VehicleEdNodeTreeView.getSelectedItem() == %id )
        VehicleEdNodes-->nodeName.setText( %newName );

    // --- DETAILS TAB ---
    %id = VehicleEdDetails-->objectNode.findText( %oldName );
    if ( %id != -1 ) {
        VehicleEdDetails-->objectNode.clearEntry( %id );
        VehicleEdDetails-->objectNode.add( %newName, %id );
        VehicleEdDetails-->objectNode.sortID();
        if ( VehicleEdDetails-->objectNode.getText() $= %oldName )
            VehicleEdDetails-->objectNode.setText( %newName );
    }
}

// Update the GUI in response to a node's parent being changed
function VehicleEdPropWindow::update_onNodeParentChanged( %this, %nodeName ) {
    // --- MISC ---
    VehicleEdShapeView.updateNodeTransforms();

    // --- NODES TAB ---
    %isSelected = 0;
    %id = VehicleEdNodeTreeView.findItemByName( %nodeName );
    if ( %id > 0 ) {
        %isSelected = ( VehicleEdNodeTreeView.getSelectedItem() == %id );
        VehicleEdNodeTreeView.removeItem( %id );
    }
    VehicleEdNodeTreeView.addNodeTree( %nodeName );
    if ( %isSelected )
        VehicleEdNodeTreeView.selectItem( VehicleEdNodeTreeView.findItemByName( %nodeName ) );
}

function VehicleEdPropWindow::update_onNodeTransformChanged( %this, %nodeName ) {
    // Default to the selected node if none is specified
    if ( %nodeName $= "" ) {
        %id = VehicleEdNodeTreeView.getSelectedItem();
        if ( %id > 0 )
            %nodeName = VehicleEdNodeTreeView.getItemText( %id );
        else
            return;
    }

    // --- MISC ---
    VehicleEdShapeView.updateNodeTransforms();
    if ( VehicleEdNodes-->objectTransform.getValue() )
        GlobalGizmoProfile.setFieldValue(alignment, Object);
    else
        GlobalGizmoProfile.setFieldValue(alignment, World);

    // --- NODES TAB ---
    // Update the node transform fields if necessary
    %id = VehicleEdNodeTreeView.getSelectedItem();
    if ( ( %id > 0 ) && ( VehicleEdNodeTreeView.getItemText( %id ) $= %nodeName ) ) {
        %isWorld = VehicleEdNodes-->worldTransform.getValue();
        %transform = VehicleEditor.shape.getNodeTransform( %nodeName, %isWorld );
        VehicleEdNodes-->nodePosition.setText( getWords( %transform, 0, 2 ) );
        VehicleEdNodes-->nodeRotation.setText( getWords( %transform, 3, 6 ) );
    }
}

function VehicleEdNodeTreeView::onClearSelection( %this ) {
    VehicleEdPropWindow.update_onNodeSelectionChanged( -1 );
}

function VehicleEdNodeTreeView::onSelect( %this, %id ) {
    // Update the node name and transform controls
    VehicleEdPropWindow.update_onNodeSelectionChanged( %id );

    // Update orbit position if orbiting the selected node
    if ( VehicleEdShapeView.orbitNode ) {
        %name = %this.getItemText( %id );
        %transform = VehicleEditor.shape.getNodeTransform( %name, 1 );
        VehicleEdShapeView.setOrbitPos( getWords( %transform, 0, 2 ) );
    }
}

function VehicleEdShapeView::onNodeSelected( %this, %index ) {
    VehicleEdNodeTreeView.clearSelection();
    if ( %index > 0 ) {
        %name = VehicleEditor.shape.getNodeName( %index );
        %id = VehicleEdNodeTreeView.findItemByName( %name );
        if ( %id > 0 )
            VehicleEdNodeTreeView.selectItem( %id );
    }
}

function VehicleEdNodes::onAddNode( %this, %name ) {
    // Add a new node, using the currently selected node as the initial parent
    if ( %name $= "" )
        %name = VehicleEditor.getUniqueName( "node", "myNode" );

    %id = VehicleEdNodeTreeView.getSelectedItem();
    if ( %id <= 0 )
        %parent = "";
    else
        %parent = VehicleEdNodeTreeView.getItemText( %id );

    VehicleEditor.doAddNode( %name, %parent, "0 0 0 0 0 1 0" );
}

function VehicleEdNodes::onDeleteNode( %this ) {
    // Remove the node and all its children from the shape
    %id = VehicleEdNodeTreeView.getSelectedItem();
    if ( %id > 0 ) {
        %name = VehicleEdNodeTreeView.getItemText( %id );
        VehicleEditor.doRemoveShapeData( "Node", %name );
    }
}

// Determine the index of a node in the tree relative to its parent
function VehicleEdNodeTreeView::getChildIndexByName( %this, %name ) {
    %id = %this.findItemByName( %name );
    %parentId = %this.getParent( %id );
    %childId = %this.getChild( %parentId );
    if ( %childId <= 0 )
        return 0;   // bad!

    %index = 0;
    while ( %childId != %id ) {
        %childId = %this.getNextSibling( %childId );
        %index++;
    }

    return %index;
}

// Add a node and its children to the node tree view
function VehicleEdNodeTreeView::addNodeTree( %this, %nodeName ) {
    // Abort if already added => something dodgy has happened and we'd end up
    // recursing indefinitely
    if ( %this.findItemByName( %nodeName ) ) {
        error( "Recursion error in VehicleEdNodeTreeView::addNodeTree" );
        return 0;
    }

    // Find parent and add me to it
    %parentName = VehicleEditor.shape.getNodeParentName( %nodeName );
    if ( %parentName $= "" )
        %parentName = "<root>";

    %parentId = %this.findItemByName( %parentName );
    %id = %this.insertItem( %parentId, %nodeName, 0, "" );

    // Add children
    %count = VehicleEditor.shape.getNodeChildCount( %nodeName );
    for ( %i = 0; %i < %count; %i++ )
        %this.addNodeTree( VehicleEditor.shape.getNodeChildName( %nodeName, %i ) );

    return %id;
}

function VehicleEdNodes::onEditName( %this ) {
    %id = VehicleEdNodeTreeView.getSelectedItem();
    if ( %id > 0 ) {
        %oldName = VehicleEdNodeTreeView.getItemText( %id );
        %newName = %this-->nodeName.getText();
        if ( %newName !$= "" )
            VehicleEditor.doRenameNode( %oldName, %newName );
    }
}

function VehicleEdNodeParentMenu::onSelect( %this, %id, %text ) {
    %id = VehicleEdNodeTreeView.getSelectedItem();
    if ( %id > 0 ) {
        %name = VehicleEdNodeTreeView.getItemText( %id );
        VehicleEditor.doSetNodeParent( %name, %text );
    }
}

function VehicleEdNodes::onEditTransform( %this ) {
    %id = VehicleEdNodeTreeView.getSelectedItem();
    if ( %id > 0 ) {
        %name = VehicleEdNodeTreeView.getItemText( %id );

        // Get the node transform from the gui
        %pos = %this-->nodePosition.getText();
        %rot = %this-->nodeRotation.getText();
        %txfm = %pos SPC %rot;
        %isWorld = VehicleEdNodes-->worldTransform.getValue();

        // Do a quick sanity check to avoid setting wildly invalid transforms
        for ( %i = 0; %i < 7; %i++ ) {  // "x y z aa.x aa.y aa.z aa.angle"
            if ( getWord( %txfm, %i ) $= "" )
                return;
        }

        VehicleEditor.doEditNodeTransform( %name, %txfm, %isWorld, -1 );
    }
}

function VehicleEdShapeView::onEditNodeTransform( %this, %node, %txfm, %gizmoID ) {
    VehicleEditor.doEditNodeTransform( %node, %txfm, 1, %gizmoID );
}