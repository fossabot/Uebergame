//==============================================================================
// Boost! -> Helper functions for common settings GUI needs
// Copyright NordikLab Studio, 2013
//==============================================================================
//==============================================================================
/// Delete an object by making sure it exist
function delObj(%obj) {
	if (isObject(%obj))
		%obj.delete();
}
//------------------------------------------------------------------------------


//==============================================================================
// SimObject Show and Hide Helpers
//==============================================================================
//==============================================================================
// Hide an object simply
function hide(%obj) {
	if (!isObject(%obj))
		return false;

	%obj.setVisible(false);
}
//------------------------------------------------------------------------------
//==============================================================================
// Show an object simply
function show(%obj) {
	if (!isObject(%obj))
		return false;
	%obj.setVisible(true);
}
//------------------------------------------------------------------------------
//==============================================================================
// Toggle an object visibility state
function toggleVisible(%obj) {
	if (%obj.visible)
		hide(%obj);
	else
		show(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
// SimObject / SimGroup Helpers
//==============================================================================
/// Clone an object and make source invisible
function cloneObject( %source,%name,%internalName,%parent ) {
   delObj(%name);
	%clone = %source.deepClone();
	//%clone.setVisible(true);
	if (%internalName !$= "")
	   %clone.internalName = %internalName;   
	%clone.setName(%name);
	if (isObject(%parent))
	   %parent.add(%clone);  
   hide(%source);
   show(%clone);
	return %clone;
}

//==============================================================================
// SimObject Function Helpers
//===============================================================================
//==============================================================================
function findIntName(%obj,%intName, %searchChild) {
	if (%searchChild $= "")
		%searchChild = true;
	if (!isObject(%obj)) {
		return;
	}
	%return = %obj.findObjectByInternalName(%intName,%searchChild);
	return %return;

}
//------------------------------------------------------------------------------

//==============================================================================
// Get child of obj by internal Name
function dumpMethods(%object) {
	%array = %object.dumpMethods();
	%array.dumpContent();
}
//------------------------------------------------------------------------------