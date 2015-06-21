//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$FEP_CreateBrushGroupAsParentFolder = true;
$FEP_RegenerateExistingData = false;
//==============================================================================
function FEP_Manager::initDataGenerator( %this ) {
	FEP_ForestDataGenerator-->sourceFolder.setText("[Select model folder to generate from]");
	FEP_ForestDataGenerator-->groupName.setText("[Brush group name]");
	FEP_ForestDataGenerator-->prefix.setText("[Data prefix]");
	FEP_ForestDataGenerator-->doBackup.setStateOn("1");
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::dataGenFolder( %this, %path ) {
	%path = makeRelativePath(%path );
	devLog("Path os",%path);
	FEP_ForestDataGenerator-->sourceFolder.setText(%path);
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::generateForestData( %this, %path ) {
	%baseFolder = FEP_ForestDataGenerator-->sourceFolder.getText();
	%name = FEP_ForestDataGenerator-->groupName.getText();
	%prefix = FEP_ForestDataGenerator-->prefix.getText();
	%backup = FEP_ForestDataGenerator-->doBackup.isStateOn();
	devLog("Folder",%baseFolder,"Name",%name,"Prefix",%prefix,"Backup",%backup);
	buildForestDataFromFolder(%baseFolder,%name,%prefix,%backup,$FEP_CreateBrushGroupAsParentFolder);
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::doOpenDialog( %this, %filter, %callback ) {
	%currentFile = "art/shapes/";
	%dlg = new OpenFolderDialog() {
		Title = "Select Export Folder";
		Filters = %filter;
		DefaultFile = %currentFile;
		ChangePath = false;
		MustExist = true;
		MultipleFiles = false;
	};

	if(%dlg.Execute())
		FEP_Manager.dataGenFolder(%dlg.FileName);

	%dlg.delete();
}
//------------------------------------------------------------------------------
