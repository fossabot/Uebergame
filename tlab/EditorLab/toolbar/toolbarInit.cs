//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


function Lab::loadPluginsToolbar() {
	foreach(%gui in LabToolbarGuiSet) {
		%container = %gui.editorContainer;
		%container.add(%gui);
	}
}

function Lab::loadPluginsPalettes() {
	newSimSet("LabPaletteItemSet");
	foreach(%paletteId in LabPaletteGuiSet) {
		%paletteGroup = 0;
		%i = %paletteId.getCount();
		for( ; %i != 0; %i--) {
			%paletteId.getObject(0).defaultParent = %paletteId;
			%paletteId.getObject(0).visible = 0;
			%paletteId.getObject(0).groupNum = %paletteGroup;
			%paletteId.getObject(0).paletteName = %paletteId.getName();
			LabPaletteItemSet.add(%paletteId.getObject(0));
			EWToolsPaletteArray.addGuiControl(%paletteId.getObject(0));

		}
		%paletteGroup++;
	}
}
