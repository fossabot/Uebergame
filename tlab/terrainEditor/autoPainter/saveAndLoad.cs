//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TPG_FilerFilter = "Lab Painter Layers|*.painter.cs";

//==============================================================================
// Set the layer group file folder base (Editor or Level)
function TPG::setLayersFolderBase(%this,%menu) {
	TPG.folderBase = %menu.getText();
}
//------------------------------------------------------------------------------

//==============================================================================
// Save painter layers set
//==============================================================================
//==============================================================================
// Save all the layers to a file
function TPG::saveLayerGroup(%this) {
	if (%this.folderBase $= "TerrainEditor")
		TPG.layerPath = "tlab/terrainEditor/painterLayers/default.painter.cs";
	else if (%this.folderBase $= "CurrentLevel")
		TPG.layerPath = MissionGroup.getFilename();

	getSaveFilename($TPG_FilerFilter, "TPG.saveLayerGroupHandler", TPG.layerPath);
}
//------------------------------------------------------------------------------
//==============================================================================
// Callback for the saved file
function TPG::saveLayerGroupHandler( %this,%filename ) {
	%filename = makeRelativePath( %filename, getMainDotCsDir() );

	if(strStr(%filename, ".") == -1)
		%filename = %filename @ ".painter.cs";

	// Create a file object for writing
	%fileWrite = new FileObject();
	%fileWrite.OpenForWrite(%filename);
	%fileWrite.writeObject(TPG_LayerGroup);
	%fileWrite.close();
	%fileWrite.delete();
	info("Terrain painter layers save to file:",%filename);
}
//------------------------------------------------------------------------------

//==============================================================================
// Load painter layer set
//==============================================================================

//==============================================================================
// Load all the layers saved in a file
function TPG::loadLayerGroup(%this) {
	if (%this.folderBase $= "TerrainEditor")
		TPG.layerPath = "tlab/terrainEditor/painterLayers/default.painter.cs";
	else if (%this.folderBase $= "CurrentLevel")
		TPG.layerPath = MissionGroup.getFilename();

	getLoadFilename($TPG_FilerFilter, "TPG.loadLayerGroupHandler",TPG.layerPath);
}
//------------------------------------------------------------------------------
//==============================================================================
// Callback for file loader
function TPG::loadLayerGroupHandler(%this,%filename) {
	if ( isScriptFile( %filename ) ) {
		TerrainPaintGeneratorGui.deleteLayerGroup();
		TPG_LayerGroup.delete();
		%filename = expandFilename(%filename);
		exec(%filename);
	}

	foreach(%layer in TPG_LayerGroup) {
	
		%layer.pill = "";
		TerrainPaintGeneratorGui.addLayerPill(%layer,true);
	}

	if (!isObject(TPG_LayerGroup))
		new SimGroup( TPG_LayerGroup );
}
//------------------------------------------------------------------------------
