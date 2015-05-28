//==============================================================================
// TorqueLab -> Initialize editor plugins
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function GuiControl::fitIntoParents( %this ) {	

   %parent = %this.parentGroup;

	%pos = "0 0";
	%extent = %parent.extent;

	%this.position = %pos;
	%this.extent = %extent;
}
//------------------------------------------------------------------------------
