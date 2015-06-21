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

if ( !isObject( LabModel ) ) new ScriptObject( LabModel ) {
	shape = -1;
	deletedCount = 0;
};


// Capitalise the first letter of the input string
function strcapitalise( %str ) {
	%len = strlen( %str );
	return strupr( getSubStr( %str,0,1 ) ) @ getSubStr( %str,1,%len-1 );
}

function LabModel::getObjectShapeFile( %this, %obj ) {
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
function LabModel::nameExists( %this, %type, %name ) {
	if ( LabModel.shape == -1 )
		return false;

	if ( %type $= "node" )
		return ( LabModel.shape.getNodeIndex( %name ) >= 0 );
	else if ( %type $= "sequence" )
		return ( LabModel.shape.getSequenceIndex( %name ) >= 0 );
	else if ( %type $= "object" )
		return ( LabModel.shape.getObjectIndex( %name ) >= 0 );
}

// Check if the given 'hint' name exists (spaces could also be underscores)
function LabModel::hintNameExists( %this, %type, %name ) {
	if ( LabModel.nameExists( %type, %name ) )
		return true;

	// If the name contains spaces, try replacing with underscores
	%name = strreplace( %name, " ", "_" );

	if ( LabModel.nameExists( %type, %name ) )
		return true;

	return false;
}

// Generate a unique name from a given base by appending an integer
function LabModel::getUniqueName( %this, %type, %name ) {
	for ( %idx = 1; %idx < 100; %idx++ ) {
		%uniqueName = %name @ %idx;

		if ( !%this.nameExists( %type, %uniqueName ) )
			break;
	}

	return %uniqueName;
}

function LabModel::getProxyName( %this, %seqName ) {
	return "__proxy__" @ %seqName;
}

function LabModel::getUnproxyName( %this, %proxyName ) {
	return strreplace( %proxyName, "__proxy__", "" );
}

function LabModel::getBackupName( %this, %seqName ) {
	return "__backup__" @ %seqName;
}

// Check if this mesh name is a collision hint
function LabModel::isCollisionMesh( %this, %name ) {
	return ( startswith( %name, "ColBox" ) ||
				startswith( %name, "ColSphere" ) ||
				startswith( %name, "ColCapsule" ) ||
				startswith( %name, "ColConvex" ) );
}

//
function LabModel::getSequenceSource( %this, %seqName ) {
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
function LabModel::getNodeNames( %this, %nodeName, %names, %exclude ) {
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
function LabModel::getObjectMeshList( %this, %name ) {
	%list = "";
	%count = %this.shape.getMeshCount( %name );

	for ( %i = 0; %i < %count; %i++ )
		%list = %list TAB %this.shape.getMeshName( %name, %i );

	return trim( %list );
}

// Get the list of meshes for a particular detail level
function LabModel::getDetailMeshList( %this, %detSize ) {
	%list = "";
	%objCount = LabModel.shape.getObjectCount();

	for ( %i = 0; %i < %objCount; %i++ ) {
		%objName = LabModel.shape.getObjectName( %i );
		%meshCount = LabModel.shape.getMeshCount( %objName );

		for ( %j = 0; %j < %meshCount; %j++ ) {
			%size = LabModel.shape.getMeshSize( %objName, %j );

			if ( %size == %detSize )
				%list = %list TAB %this.shape.getMeshName( %objName, %j );
		}
	}

	return trim( %list );
}


