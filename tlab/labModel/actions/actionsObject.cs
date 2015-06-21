//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
// Rename object
function LabModel::doRenameObject( %this, %oldName, %newName ) {
	%action = %this.createAction( ActionRenameObject, "Rename object" );
	%action.oldName = %oldName;
	%action.newName = %newName;
	%this.doAction( %action );
}

function ActionRenameObject::doit( %this ) {
	if ( LabModel.shape.renameObject( %this.oldName, %this.newName ) ) {
		LabModelPropWindow.update_onObjectRenamed( %this.oldName, %this.newName );
		return true;
	}

	return false;
}

function ActionRenameObject::undo( %this ) {
	Parent::undo( %this );

	if ( LabModel.shape.renameObject( %this.newName, %this.oldName ) )
		LabModelPropWindow.update_onObjectRenamed( %this.newName, %this.oldName );
}


//------------------------------------------------------------------------------
// Remove mesh
function LabModel::doRemoveMesh( %this, %meshName ) {
	%action = %this.createAction( ActionRemoveMesh, "Remove mesh" );
	%action.meshName = %meshName;
	%this.doAction( %action );
}

function ActionRemoveMesh::doit( %this ) {
	if ( LabModel.shape.removeMesh( %this.meshName ) ) {
		LabModelPropWindow.update_onMeshRemoved( %this.meshName );
		return true;
	}

	return false;
}

function ActionRemoveMesh::undo( %this ) {
	Parent::undo( %this );
}

//------------------------------------------------------------------------------
// Add meshes from file
function LabModel::doAddMeshFromFile( %this, %filename, %size ) {
	%action = %this.createAction( ActionAddMeshFromFile, "Add mesh from file" );
	%action.filename = %filename;
	%action.size = %size;
	%this.doAction( %action );
}

function ActionAddMeshFromFile::doit( %this ) {
	%this.meshList = LabModel.addLODFromFile( LabModel.shape, %this.filename, %this.size, 1 );

	if ( %this.meshList !$= "" ) {
		%count = getFieldCount( %this.meshList );

		for ( %i = 0; %i < %count; %i++ )
			LabModelPropWindow.update_onMeshAdded( getField( %this.meshList, %i ) );

		LabModelMaterials.updateMaterialList();
		return true;
	}

	return false;
}

function ActionAddMeshFromFile::undo( %this ) {
	// Remove all the meshes we added
	%count = getFieldCount( %this.meshList );

	for ( %i = 0; %i < %count; %i ++ ) {
		%name = getField( %this.meshList, %i );
		LabModel.shape.removeMesh( %name );
		LabModelPropWindow.update_onMeshRemoved( %name );
	}

	LabModelMaterials.updateMaterialList();
}



//------------------------------------------------------------------------------
// Update bounds
function LabModel::doSetBounds( %this ) {
	%action = %this.createAction( ActionSetBounds, "Set bounds" );
	%action.oldBounds = LabModel.shape.getBounds();
	%action.newBounds = LabModelPreview.computeShapeBounds();
	%this.doAction( %action );
}

function ActionSetBounds::doit( %this ) {
	return LabModel.shape.setBounds( %this.newBounds );
}

function ActionSetBounds::undo( %this ) {
	Parent::undo( %this );
	LabModel.shape.setBounds( %this.oldBounds );
}
