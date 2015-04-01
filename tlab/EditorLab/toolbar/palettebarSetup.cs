//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Toggle the palette bar tools to activate those used by plugin
function Lab::togglePluginPalette(%this, %paletteName) {
	// since the palette window ctrl auto adjusts to child ctrls being visible,
	// loop through the array and pick out the children that belong to a certain tool
	// and label them visible or not visible

	show(EWToolsPaletteContainer);
	for( %i = 0; %i < EWToolsPaletteArray.getCount(); %i++ )
		EWToolsPaletteArray.getObject(%i).visible = 0;

	%windowMultiplier = 0;
	%paletteNameWordCount = getWordCount( %paletteName );
	for(%pallateNum = 0; %pallateNum < %paletteNameWordCount; %pallateNum++) {
		%currentPalette = getWord(%paletteName, %pallateNum);
		for( %i = 0; %i < EWToolsPaletteArray.getCount(); %i++ ) {
			if( EWToolsPaletteArray.getObject(%i).paletteName $= %currentPalette) {
				EWToolsPaletteArray.getObject(%i).visible = 1;
				%windowMultiplier++;
			}
		}
	}

	// auto adjust the palette window extent according to how many
	// children controls we found; if none found, the palette window becomes invisible
	if( %windowMultiplier == 0 || %paletteName $= "")
		EWToolsPaletteContainer.visible = 0;
	else {
		EWToolsPaletteContainer.visible = 1;
		EWToolsPaletteContainer.extent = getWord(EWToolsPaletteContainer.extent, 0) SPC (4 + 26 * %windowMultiplier);
	}
}
//------------------------------------------------------------------------------

