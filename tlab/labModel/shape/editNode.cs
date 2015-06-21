//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//------------------------------------------------------------------------------
// Node Editing
//------------------------------------------------------------------------------

// Update the GUI in response to the node selection changing
function LabModelPropWindow::update_onNodeSelectionChanged( %this, %id ) {
	if ( %id > 0 ) {
		// Enable delete button and edit boxes
		if ( LabModelSeqNodeTabBook.activePage $= "Node" )
			LabModelPropWindow-->deleteBtn.setActive( true );

		LabModelNodes-->nodeName.setActive( true );
		LabModelNodes-->nodePosition.setActive( true );
		LabModelNodes-->nodeRotation.setActive( true );
		// Update the node inspection data
		%name = LabModelNodeTreeView.getItemText( %id );
		LabModelNodes-->nodeName.setText( %name );
		// Node parent list => ancestor and sibling nodes only (can't re-parent to a descendent)
		LabModelNodeParentMenu.clear();
		%parentNames = LabModel.getNodeNames( "", "<root>", %name );
		%count = getWordCount( %parentNames );

		for ( %i = 0; %i < %count; %i++ )
			LabModelNodeParentMenu.add( getWord(%parentNames, %i), %i );

		%pName = LabModel.shape.getNodeParentName( %name );

		if ( %pName $= "" )
			%pName = "<root>";

		LabModelNodeParentMenu.setText( %pName );

		if ( LabModelNodes-->worldTransform.getValue() ) {
			// Global transform
			%txfm = LabModel.shape.getNodeTransform( %name, 1 );
			LabModelNodes-->nodePosition.setText( getWords( %txfm, 0, 2 ) );
			LabModelNodes-->nodeRotation.setText( getWords( %txfm, 3, 6 ) );
		} else {
			// Local transform (relative to parent)
			%txfm = LabModel.shape.getNodeTransform( %name, 0 );
			LabModelNodes-->nodePosition.setText( getWords( %txfm, 0, 2 ) );
			LabModelNodes-->nodeRotation.setText( getWords( %txfm, 3, 6 ) );
		}

		LabModelPreview.selectedNode = LabModel.shape.getNodeIndex( %name );
	} else {
		// Disable delete button and edit boxes
		if ( LabModelSeqNodeTabBook.activePage $= "Node" )
			LabModelPropWindow-->deleteBtn.setActive( false );

		LabModelNodes-->nodeName.setActive( false );
		LabModelNodes-->nodePosition.setActive( false );
		LabModelNodes-->nodeRotation.setActive( false );
		LabModelNodes-->nodeName.setText( "" );
		LabModelNodes-->nodePosition.setText( "" );
		LabModelNodes-->nodeRotation.setText( "" );
		LabModelPreview.selectedNode = -1;
	}
}

// Update the GUI in response to a node being added
function LabModelPropWindow::update_onNodeAdded( %this, %nodeName, %oldTreeIndex ) {
	// --- MISC ---
	LabModelPreview.refreshShape();
	LabModelPreview.updateNodeTransforms();
	LabModelSelectWindow.updateHints();

	// --- MOUNT WINDOW ---
	if ( LabModelMountWindow.isMountableNode( %nodeName ) ) {
		LabModelMountWindow-->mountNode.add( %nodeName );
		LabModelMountWindow-->mountNode.sort();
	}

	// --- NODES TAB ---
	%id = LabModelNodeTreeView.addNodeTree( %nodeName );

	if ( %oldTreeIndex <= 0 ) {
		// This is a new node => make it the current selection
		if ( %id > 0 ) {
			LabModelNodeTreeView.clearSelection();
			LabModelNodeTreeView.selectItem( %id );
		}
	} else {
		// This node has been un-deleted. Inserting a new item puts it at the
		// end of the siblings, but we want to restore the original order as
		// if the item was never deleted, so move it up as required.
		%childIndex = LabModelNodeTreeView.getChildIndexByName( %nodeName );

		while ( %childIndex > %oldTreeIndex ) {
			LabModelNodeTreeView.moveItemUp( %id );
			%childIndex--;
		}
	}

	// --- DETAILS TAB ---
	LabModelDetails-->objectNode.add( %nodeName );
}

// Update the GUI in response to a node(s) being removed
function LabModelPropWindow::update_onNodeRemoved( %this, %nameList, %nameCount ) {
	// --- MISC ---
	LabModelPreview.refreshShape();
	LabModelPreview.updateNodeTransforms();
	LabModelSelectWindow.updateHints();

	// Remove nodes from the mountable list, and any shapes mounted to the node
	for ( %i = 0; %i < %nameCount; %i++ ) {
		%nodeName = getField( %nameList, %i );
		LabModelMountWindow-->mountNode.clearEntry( LabModelMountWindow-->mountNode.findText( %nodeName ) );

		for ( %j = LabModelMountWindow-->mountList.rowCount()-1; %j >= 1; %j-- ) {
			%text = LabModelMountWindow-->mountList.getRowText( %j );

			if ( getField( %text, 1 ) $= %nodeName ) {
				LabModelPreview.unmountShape( %j-1 );
				LabModelMountWindow-->mountList.removeRow( %j );
			}
		}
	}

	// --- NODES TAB ---
	%lastName = getField( %nameList, %nameCount-1 );
	%id = LabModelNodeTreeView.findItemByName( %lastName );   // only need to remove the parent item

	if ( %id > 0 ) {
		LabModelNodeTreeView.removeItem( %id );

		if ( LabModelNodeTreeView.getSelectedItem() <= 0 )
			LabModelPropWindow.update_onNodeSelectionChanged( -1 );
	}

	// --- DETAILS TAB ---
	for ( %i = 0; %i < %nameCount; %i++ ) {
		%nodeName = getField( %nameList, %i );
		LabModelDetails-->objectNode.clearEntry( LabModelDetails-->objectNode.findText( %nodeName ) );
	}
}

// Update the GUI in response to a node being renamed
function LabModelPropWindow::update_onNodeRenamed( %this, %oldName, %newName ) {
	// --- MISC ---
	LabModelSelectWindow.updateHints();
	// --- MOUNT WINDOW ---
	// Update entries for any shapes mounted to this node
	%rowCount = LabModelMountWindow-->mountList.rowCount();

	for ( %i = 1; %i < %rowCount; %i++ ) {
		%text = LabModelMountWindow-->mountList.getRowText( %i );

		if ( getField( %text, 1 ) $= %oldName ) {
			%text = setField( %text, 1, %newName );
			LabModelMountWindow-->mountList.setRowById( LabModelMountWindow-->mountList.getRowId( %i ), %text );
		}
	}

	// Update list of mountable nodes
	LabModelMountWindow-->mountNode.clearEntry( LabModelMountWindow-->mountNode.findText( %oldName ) );

	if ( LabModelMountWindow.isMountableNode( %newName ) ) {
		LabModelMountWindow-->mountNode.add( %newName );
		LabModelMountWindow-->mountNode.sort();
	}

	// --- NODES TAB ---
	%id = LabModelNodeTreeView.findItemByName( %oldName );
	LabModelNodeTreeView.editItem( %id, %newName, 0 );

	if ( LabModelNodeTreeView.getSelectedItem() == %id )
		LabModelNodes-->nodeName.setText( %newName );

	// --- DETAILS TAB ---
	%id = LabModelDetails-->objectNode.findText( %oldName );

	if ( %id != -1 ) {
		LabModelDetails-->objectNode.clearEntry( %id );
		LabModelDetails-->objectNode.add( %newName, %id );
		LabModelDetails-->objectNode.sortID();

		if ( LabModelDetails-->objectNode.getText() $= %oldName )
			LabModelDetails-->objectNode.setText( %newName );
	}
}

// Update the GUI in response to a node's parent being changed
function LabModelPropWindow::update_onNodeParentChanged( %this, %nodeName ) {
	// --- MISC ---
	LabModelPreview.updateNodeTransforms();
	// --- NODES TAB ---
	%isSelected = 0;
	%id = LabModelNodeTreeView.findItemByName( %nodeName );

	if ( %id > 0 ) {
		%isSelected = ( LabModelNodeTreeView.getSelectedItem() == %id );
		LabModelNodeTreeView.removeItem( %id );
	}

	LabModelNodeTreeView.addNodeTree( %nodeName );

	if ( %isSelected )
		LabModelNodeTreeView.selectItem( LabModelNodeTreeView.findItemByName( %nodeName ) );
}

function LabModelPropWindow::update_onNodeTransformChanged( %this, %nodeName ) {
	// Default to the selected node if none is specified
	if ( %nodeName $= "" ) {
		%id = LabModelNodeTreeView.getSelectedItem();

		if ( %id > 0 )
			%nodeName = LabModelNodeTreeView.getItemText( %id );
		else
			return;
	}

	// --- MISC ---
	LabModelPreview.updateNodeTransforms();

	if ( LabModelNodes-->objectTransform.getValue() )
		Lab.setGizmoAlignment("Object");
	else
		Lab.setGizmoAlignment("World");

	// --- NODES TAB ---
	// Update the node transform fields if necessary
	%id = LabModelNodeTreeView.getSelectedItem();

	if ( ( %id > 0 ) && ( LabModelNodeTreeView.getItemText( %id ) $= %nodeName ) ) {
		%isWorld = LabModelNodes-->worldTransform.getValue();
		%transform = LabModel.shape.getNodeTransform( %nodeName, %isWorld );
		LabModelNodes-->nodePosition.setText( getWords( %transform, 0, 2 ) );
		LabModelNodes-->nodeRotation.setText( getWords( %transform, 3, 6 ) );
	}
}

function LabModelNodeTreeView::onClearSelection( %this ) {
	LabModelPropWindow.update_onNodeSelectionChanged( -1 );
}

function LabModelNodeTreeView::onSelect( %this, %id ) {
	// Update the node name and transform controls
	LabModelPropWindow.update_onNodeSelectionChanged( %id );

	// Update orbit position if orbiting the selected node
	if ( LabModelPreview.orbitNode ) {
		%name = %this.getItemText( %id );
		%transform = LabModel.shape.getNodeTransform( %name, 1 );
		LabModelPreview.setOrbitPos( getWords( %transform, 0, 2 ) );
	}
}

function LabModelPreview::onNodeSelected( %this, %index ) {
	LabModelNodeTreeView.clearSelection();

	if ( %index > 0 ) {
		%name = LabModel.shape.getNodeName( %index );
		%id = LabModelNodeTreeView.findItemByName( %name );

		if ( %id > 0 )
			LabModelNodeTreeView.selectItem( %id );
	}
}

function LabModelNodes::onAddNode( %this, %name ) {
	// Add a new node, using the currently selected node as the initial parent
	if ( %name $= "" )
		%name = LabModel.getUniqueName( "node", "myNode" );

	%id = LabModelNodeTreeView.getSelectedItem();

	if ( %id <= 0 )
		%parent = "";
	else
		%parent = LabModelNodeTreeView.getItemText( %id );

	LabModel.doAddNode( %name, %parent, "0 0 0 0 0 1 0" );
}

function LabModelNodes::onDeleteNode( %this ) {
	// Remove the node and all its children from the shape
	%id = LabModelNodeTreeView.getSelectedItem();

	if ( %id > 0 ) {
		%name = LabModelNodeTreeView.getItemText( %id );
		LabModel.doRemoveShapeData( "Node", %name );
	}
}

// Determine the index of a node in the tree relative to its parent
function LabModelNodeTreeView::getChildIndexByName( %this, %name ) {
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
function LabModelNodeTreeView::addNodeTree( %this, %nodeName ) {
	// Abort if already added => something dodgy has happened and we'd end up
	// recursing indefinitely
	if ( %this.findItemByName( %nodeName ) ) {
		error( "Recursion error in LabModelNodeTreeView::addNodeTree" );
		return 0;
	}

	// Find parent and add me to it
	%parentName = LabModel.shape.getNodeParentName( %nodeName );

	if ( %parentName $= "" )
		%parentName = "<root>";

	%parentId = %this.findItemByName( %parentName );
	%id = %this.insertItem( %parentId, %nodeName, 0, "" );
	// Add children
	%count = LabModel.shape.getNodeChildCount( %nodeName );

	for ( %i = 0; %i < %count; %i++ )
		%this.addNodeTree( LabModel.shape.getNodeChildName( %nodeName, %i ) );

	return %id;
}

function LabModelNodes::onEditName( %this ) {
	%id = LabModelNodeTreeView.getSelectedItem();

	if ( %id > 0 ) {
		%oldName = LabModelNodeTreeView.getItemText( %id );
		%newName = %this-->nodeName.getText();

		if ( %newName !$= "" )
			LabModel.doRenameNode( %oldName, %newName );
	}
}

function LabModelNodeParentMenu::onSelect( %this, %id, %text ) {
	%id = LabModelNodeTreeView.getSelectedItem();

	if ( %id > 0 ) {
		%name = LabModelNodeTreeView.getItemText( %id );
		LabModel.doSetNodeParent( %name, %text );
	}
}

function LabModelNodes::onEditTransform( %this ) {
	%id = LabModelNodeTreeView.getSelectedItem();

	if ( %id > 0 ) {
		%name = LabModelNodeTreeView.getItemText( %id );
		// Get the node transform from the gui
		%pos = %this-->nodePosition.getText();
		%rot = %this-->nodeRotation.getText();
		%txfm = %pos SPC %rot;
		%isWorld = LabModelNodes-->worldTransform.getValue();

		// Do a quick sanity check to avoid setting wildly invalid transforms
		for ( %i = 0; %i < 7; %i++ ) {  // "x y z aa.x aa.y aa.z aa.angle"
			if ( getWord( %txfm, %i ) $= "" )
				return;
		}

		LabModel.doEditNodeTransform( %name, %txfm, %isWorld, -1 );
	}
}

function LabModelPreview::onEditNodeTransform( %this, %node, %txfm, %gizmoID ) {
	LabModel.doEditNodeTransform( %node, %txfm, 1, %gizmoID );
}