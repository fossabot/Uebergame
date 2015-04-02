//==============================================================================
// Lab Helpers ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Profile FONTS manipulation functions
//==============================================================================

//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function changeProfilesFont(%font,%profileList) {

	if (%profileList $= "") {
		changeAllProfilesFont(%font);
	}
	foreach$( %obj in %profileList ) {
		%obj.fontType = %font;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Change the font for all profiles
function changeAllProfilesFont(%font,%use, %category) {
	foreach( %obj in GuiDataGroup ) {
		if( !%obj.isMemberOfClass( "GuiControlProfile" ) )
			continue;

		//Skip if not like specified fontUse
		if (%use !$="" && %obj.fontUse !$= %use)
			continue;
		//Skip if not like specified category
		if (%category !$="" && %obj.category !$= %category)
			continue;

		%obj.fontType = %font;
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function changeProfilesFont(%font,%profileList) {
	foreach( %obj in GuiDataGroup ) {
		if( !%obj.isMemberOfClass( "GuiControlProfile" ) )
			continue;

		%obj.fontType = %font;

	}
}
//------------------------------------------------------------------------------
//==============================================================================
// Profile data manipulations functions
//==============================================================================


//==============================================================================
// Add a specific Justify setting to a GuiControlProfile
function addJustifyChildrenToProfile(%profile,%addLeft,%addCenter,%addRight) {

	if (!isObject(%profile)) {
		warnLog("Invalid profile supplied for addJustify:",%profile);
		return;
	}
	if ( %addLeft) {
		%name = %profile.getName() @"_L";
		%newProfile = new GuiControlProfile(%name);
		%newProfile.assignFieldsFrom(%profile);
		%newProfile.justify = "Left";
	}

	if ( %addCenter) {
		%name = %profile.getName() @"_C";
		%newProfile = new GuiControlProfile(%name);
		%newProfile.assignFieldsFrom(%profile);
		%newProfile.justify = "Center";
	}
	if ( %addRight) {
		%name = %profile.getName() @"_R";
		%newProfile = new GuiControlProfile(%name);
		%newProfile.assignFieldsFrom(%profile);
		%newProfile.justify = "Right";
	}

}
//------------------------------------------------------------------------------



