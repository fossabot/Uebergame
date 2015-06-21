//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
// Add/edit collision geometry
function LabModel::doEditCollision( %this, %type, %target, %depth, %merge, %concavity,
												%maxVerts, %boxMax, %sphereMax, %capsuleMax ) {
	%colData = LabModelColWindow.lastColSettings;
	%action = %this.createAction( ActionEditCollision, "Edit shape collision" );
	%action.oldType = getField( %colData, 0 );
	%action.oldTarget = getField( %colData, 1 );
	%action.oldDepth = getField( %colData, 2 );
	%action.oldMerge = getField( %colData, 3 );
	%action.oldConcavity = getField( %colData, 4 );
	%action.oldMaxVerts = getField( %colData, 5 );
	%action.oldBoxMax = getField( %colData, 6 );
	%action.oldSphereMax = getField( %colData, 7 );
	%action.oldCapsuleMax = getField( %colData, 8 );
	%action.newType = %type;
	%action.newTarget = %target;
	%action.newDepth = %depth;
	%action.newMerge = %merge;
	%action.newConcavity = %concavity;
	%action.newMaxVerts = %maxVerts;
	%action.newBoxMax = %boxMax;
	%action.newSphereMax = %sphereMax;
	%action.newCapsuleMax = %capsuleMax;
	%this.doAction( %action );
}

function ActionEditCollision::updateCollision( %this, %type, %target, %depth, %merge, %concavity,
		%maxVerts, %boxMax, %sphereMax, %capsuleMax ) {
	%colDetailSize = -1;
	%colNode = "Col" @ %colDetailSize;
	// TreeView items are case sensitive, but TSShape names are not, so fixup case
	// if needed
	%index = LabModel.shape.getNodeIndex( %colNode );

	if ( %index != -1 )
		%colNode = LabModel.shape.getNodeName( %index );

	// First remove the old detail and collision nodes
	%meshList = LabModel.getDetailMeshList( %colDetailSize );
	%meshCount = getFieldCount( %meshList );

	if ( %meshCount > 0 ) {
		LabModel.shape.removeDetailLevel( %colDetailSize );

		for ( %i = 0; %i < %meshCount; %i++ )
			LabModelPropWindow.update_onMeshRemoved( getField( %meshList, %i ) );
	}

	%nodeList = LabModel.getNodeNames( %colNode, "" );
	%nodeCount = getFieldCount( %nodeList );

	if ( %nodeCount > 0 ) {
		for ( %i = 0; %i < %nodeCount; %i++ )
			LabModel.shape.removeNode( getField( %nodeList, %i ) );

		LabModelPropWindow.update_onNodeRemoved( %nodeList, %nodeCount );
	}

	// Add the new node and geometry
	if ( %type $= "" )
		return;

	if ( !LabModel.shape.addCollisionDetail( %colDetailSize, %type, %target,
			%depth, %merge, %concavity, %maxVerts,
			%boxMax, %sphereMax, %capsuleMax ) )
		return false;

	// Update UI
	%meshList = LabModel.getDetailMeshList( %colDetailSize );
	LabModelPropWindow.update_onNodeAdded( %colNode, LabModel.shape.getNodeCount() );    // will also add child nodes
	%count = getFieldCount( %meshList );

	for ( %i = 0; %i < %count; %i++ )
		LabModelPropWindow.update_onMeshAdded( getField( %meshList, %i ) );

	LabModelColWindow.lastColSettings = %type TAB %target TAB %depth TAB %merge TAB
													%concavity TAB %maxVerts TAB %boxMax TAB %sphereMax TAB %capsuleMax;
	LabModelColWindow.update_onCollisionChanged();
	return true;
}

function ActionEditCollision::doit( %this ) {
	LabModelWaitGui.show( "Generating collision geometry..." );
	%success = %this.updateCollision( %this.newType, %this.newTarget, %this.newDepth, %this.newMerge,
												 %this.newConcavity, %this.newMaxVerts, %this.newBoxMax,
												 %this.newSphereMax, %this.newCapsuleMax );
	LabModelWaitGui.hide();
	return %success;
}

function ActionEditCollision::undo( %this ) {
	Parent::undo( %this );
	LabModelWaitGui.show( "Generating collision geometry..." );
	%this.updateCollision( %this.oldType, %this.oldTarget, %this.oldDepth, %this.oldMerge,
								  %this.oldConcavity, %this.oldMaxVerts, %this.oldBoxMax,
								  %this.oldSphereMax, %this.oldCapsuleMax );
	LabModelWaitGui.hide();
}
