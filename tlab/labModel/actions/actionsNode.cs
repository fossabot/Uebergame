//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//------------------------------------------------------------------------------
// Add node
function LabModel::doAddNode( %this, %nodeName, %parentName, %transform ) {
	%action = %this.createAction( ActionAddNode, "Add node" );
	%action.nodeName = %nodeName;
	%action.parentName = %parentName;
	%action.transform = %transform;
	%this.doAction( %action );
}

function ActionAddNode::doit( %this ) {
	if ( LabModel.shape.addNode( %this.nodeName, %this.parentName, %this.transform ) ) {
		LabModelPropWindow.update_onNodeAdded( %this.nodeName, -1 );
		return true;
	}

	return false;
}

function ActionAddNode::undo( %this ) {
	Parent::undo( %this );

	if ( LabModel.shape.removeNode( %this.nodeName ) )
		LabModelPropWindow.update_onNodeRemoved( %this.nodeName, 1 );
}

//------------------------------------------------------------------------------
// Remove node
function LabModel::doRemoveNode( %this, %nodeName ) {
	%action = %this.createAction( ActionRemoveNode, "Remove node" );
	%action.nodeName =%nodeName;
	%action.nodeChildIndex = LabModelNodeTreeView.getChildIndexByName( %nodeName );
	// Need to delete all child nodes of this node as well, so recursively collect
	// all of the names.
	%action.nameList = %this.getNodeNames( %nodeName, "" );
	%action.nameCount = getFieldCount( %action.nameList );

	for ( %i = 0; %i < %action.nameCount; %i++ )
		%action.names[%i] = getField( %action.nameList, %i );

	%this.doAction( %action );
}

function ActionRemoveNode::doit( %this ) {
	for ( %i = 0; %i < %this.nameCount; %i++ )
		LabModel.shape.removeNode( %this.names[%i] );

	// Update GUI
	LabModelPropWindow.update_onNodeRemoved( %this.nameList, %this.nameCount );
	return true;
}

function ActionRemoveNode::undo( %this ) {
	Parent::undo( %this );
}

//------------------------------------------------------------------------------
// Rename node
function LabModel::doRenameNode( %this, %oldName, %newName ) {
	%action = %this.createAction( ActionRenameNode, "Rename node" );
	%action.oldName = %oldName;
	%action.newName = %newName;
	%this.doAction( %action );
}

function ActionRenameNode::doit( %this ) {
	if ( LabModel.shape.renameNode( %this.oldName, %this.newName ) ) {
		LabModelPropWindow.update_onNodeRenamed( %this.oldName, %this.newName );
		return true;
	}

	return false;
}

function ActionRenameNode::undo( %this ) {
	Parent::undo( %this );

	if ( LabModel.shape.renameNode( %this.newName, %this.oldName ) )
		LabModelPropWindow.update_onNodeRenamed( %this.newName, %this.oldName );
}

//------------------------------------------------------------------------------
// Set node parent
function LabModel::doSetNodeParent( %this, %name, %parent ) {
	if ( %parent $= "<root>" )
		%parent = "";

	%action = %this.createAction( ActionSetNodeParent, "Set parent node" );
	%action.nodeName = %name;
	%action.parentName = %parent;
	%action.oldParentName = LabModel.shape.getNodeParentName( %name );
	%this.doAction( %action );
}

function ActionSetNodeParent::doit( %this ) {
	if ( LabModel.shape.setNodeParent( %this.nodeName, %this.parentName ) ) {
		LabModelPropWindow.update_onNodeParentChanged( %this.nodeName );
		return true;
	}

	return false;
}

function ActionSetNodeParent::undo( %this ) {
	Parent::undo( %this );

	if ( LabModel.shape.setNodeParent( %this.nodeName, %this.oldParentName ) )
		LabModelPropWindow.update_onNodeParentChanged( %this.nodeName );
}

//------------------------------------------------------------------------------
// Edit node transform
function LabModel::doEditNodeTransform( %this, %nodeName, %newTransform, %isWorld, %gizmoID ) {
	// If dragging the 3D gizmo, combine all movement into a single action. Undoing
	// that action will return the node to where it was when the gizmo drag started.
	%last = LabModelUndoManager.getUndoAction( LabModelUndoManager.getUndoCount() - 1 );

	if ( ( %last != -1 ) && ( %last.class $= ActionEditNodeTransform ) &&
			( %last.nodeName $= %nodeName ) && ( %last.gizmoID != -1 ) && ( %last.gizmoID == %gizmoID ) ) {
		// Use the last action to do the edit, and modify it so it only applies
		// the latest transform
		%last.newTransform = %newTransform;
		%last.isWorld = %isWorld;
		%last.doit();
		LabModel.setDirty( true );
	} else {
		%action = %this.createAction( ActionEditNodeTransform, "Edit node transform" );
		%action.nodeName = %nodeName;
		%action.newTransform = %newTransform;
		%action.isWorld = %isWorld;
		%action.gizmoID = %gizmoID;
		%action.oldTransform = %this.shape.getNodeTransform( %nodeName, %isWorld );
		%this.doAction( %action );
	}
}

function ActionEditNodeTransform::doit( %this ) {
	LabModel.shape.setNodeTransform( %this.nodeName, %this.newTransform, %this.isWorld );
	LabModelPropWindow.update_onNodeTransformChanged();
	return true;
}

function ActionEditNodeTransform::undo( %this ) {
	Parent::undo( %this );
	LabModel.shape.setNodeTransform( %this.nodeName, %this.oldTransform, %this.isWorld );
	LabModelPropWindow.update_onNodeTransformChanged();
}

//------------------------------------------------------------------------------
// Edit object node
function LabModel::doSetObjectNode( %this, %objName, %node ) {
	%action = %this.createAction( ActionSetObjectNode, "Set object node" );
	%action.objName = %objName;
	%action.oldNode = %this.shape.getObjectNode( %objName );
	%action.newNode = %node;
	%this.doAction( %action );
}

function ActionSetObjectNode::doit( %this ) {
	if ( LabModel.shape.setObjectNode( %this.objName, %this.newNode ) ) {
		LabModelPropWindow.update_onObjectNodeChanged( %this.objName );
		return true;
	}

	return false;
}

function ActionSetObjectNode::undo( %this ) {
	Parent::undo( %this );

	if ( LabModel.shape.setObjectNode( %this.objName, %this.oldNode ) )
		LabModelPropWindow.update_onObjectNodeChanged( %this.objName );
}