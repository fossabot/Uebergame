//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
function ZipObject::addPath( %this, %path, %pathInZip ) {
	%beginPath = expandFilename( %path );
	%path = pathConcat(%path, "*");
	%file = findFirstFile( %path );

	while(%file !$= "") {
		%zipRel = makeRelativePath( %file, %beginPath );
		%finalZip = pathConcat(%pathInZip, %zipRel);
		%this.addFile( %file, %finalZip );
		%file = findNextFile(%path);
	}
}