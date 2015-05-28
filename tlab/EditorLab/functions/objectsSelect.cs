//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
//Editor Initialization callbacks
//==============================================================================

function Lab::onSelectObject( %this,%obj ) {
	if ($Cfg_3DView_SelectByGroup && !%obj.byGroup && isObject( %obj.partOfSet) && !$LabSingleSelection) {
		Lab.selectObjectGroup(%obj.partOfSet);		
		//This will be recall from script group call (byGroup true)
		return false;
	}	
	
	$LabSingleSelection = false;
	return true;
}


function Lab::onUnSelectObject( %this,%obj ) {
	
	return true;
}
