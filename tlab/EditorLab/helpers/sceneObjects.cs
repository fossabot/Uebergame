//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
//Editor Initialization callbacks
//==============================================================================
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function Lab::copySelection( %this,%selectSet,%offset, %copies ) {


	%count = %selectSet.getCount();
	if (%count < 1) {
		warnLog("There's no selected objects to copy!");
		return;
	}
	for (%i=1; %i<=%copies; %i++) {
		for( %j=0; %j<%count; %j++) {
			%obj = EWorldEditor.getSelectedObject( %j );
			if( !%obj.isMemberOfClass("SceneObject") ) continue;

			%clone = %obj.clone();

			%clone.position.x += %offset.x * %i;
			%clone.position.y += %offset.y * %i;
			%clone.position.z += %offset.z * %i;
			MissionGroup.add(%clone);
		}

	}

}

