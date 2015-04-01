//==============================================================================
// Boost! -> GuiControl Functions Helpers
// Copyright NordikLab Studio, 2013
//==============================================================================


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

