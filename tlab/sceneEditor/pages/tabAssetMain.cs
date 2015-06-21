//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
// Allow to manage different GUI Styles without conflict
//==============================================================================
$AssetsLab_CategoryList = "Map models" TAB "Trees" TAB "Plants/Flowers" TAB "Grass";
$AssetsLab_PathList = "art/modelPacks/" TAB "art/modelPacks/" TAB "art/shapes/";
$AssetsLab_DisplayList = "List" TAB "Small icons" TAB "Large icons";
//==============================================================================
//SEP_AssetPage.setMeshFolderDepth($ThisControl);
function SceneEditorPlugin::initAssets( %this ) {
	if (!isObject(AssetsLab))
		$AssetsLab = newScriptObject("AssetsLab");

	//Fill the menus
	AssetPathMenu.clear();
	AssetPathMenu.add("All",0);

	for(%i=0; %i<getFieldCount($AssetsLab_PathList); %i++) {
		%item = getField($AssetsLab_PathList,%i);
		AssetPathMenu.add(%item,%i+1);
	}

	//Fill the menus
	AssetCategoryMenu.clear();
	AssetCategoryMenu.add("All",0);

	for(%i=0; %i<getFieldCount($AssetsLab_CategoryList); %i++) {
		%item = getField($AssetsLab_CategoryList,%i);
		AssetCategoryMenu.add(%item,%i+1);
	}

	//Fill the menus
	AssetDisplayMenu.clear();

	for(%i=0; %i<getFieldCount($AssetsLab_DisplayList); %i++) {
		%item = getField($AssetsLab_DisplayList,%i);
		AssetDisplayMenu.add(%item,%i);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
//SEP_AssetPage.setMeshFolderDepth($ThisControl);
function AssetsLab::onMenuSelect( %this,%menu ) {
	%menuType = %menu.internalName;

	switch$(%menuType) {
	case "SearchPath":
		%path = %menu.getText();
		devLog("AssetsLab_Path Selected Text=",%path);
		AssetsLab.navigateAssets(%path);

	case "Category":
		%category = %menu.getText();
		devLog("AssetsLab_Category Selected Text=",%category);

	case "Display":
		%displayMode = %menu.getText();
		devLog("AssetsLab_Display Selected Text=",%displayMode);
	}
}
//------------------------------------------------------------------------------
