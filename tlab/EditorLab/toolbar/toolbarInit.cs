//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


function Lab::loadPluginsToolbar() {
	foreach(%gui in LabToolbarGuiSet) {
		%container = %gui.editorContainer;
		%container.add(%gui);
	}
}

