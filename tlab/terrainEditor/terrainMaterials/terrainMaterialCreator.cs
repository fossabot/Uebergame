//==============================================================================
// TorqueLab -> TerrainMaterial Creator
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function TerrainMaterialDlg::newMat( %this ) {
	if (isObject(%this.activeMat)) {
		LabMsgYesNo("New terrain material","Do you want to clone the current terrain material:" SPC %this.activeMat.internalName SPC " or you prefer to create a blank one?" SPC
						"Cloned material will be stored in same file as clone, new material will be saved in art/terrains/materials.cs." NL "Clone current material?","TerrainMaterialDlg.cloneMat();","TerrainMaterialDlg.createMat();");
	} else
		%this.createMat();
}
function TerrainMaterialDlg::createMat( %this ) {
	show(TMD_NewMaterialContainer);
	// Create a unique material name.
	%matName = getUniqueInternalName( "newMaterial", TerrainMaterialSet, true );
	// Create the new material.
	%newMat = new TerrainMaterial() {
		internalName = %matName;
		parentGroup = TerrainMaterialDlgNewGroup;
	};
	%newMat.setFileName( "art/terrains/materials.cs" );
	// Mark it as dirty and to be saved in the default location.
	ETerrainMaterialPersistMan.setDirty( %newMat, "art/terrains/materials.cs" );
	%this.setFilteredMaterialsSet(true);
	%matLibTree = %this-->matLibTree;
	%matLibTree.buildVisibleTree( true );
	%item = %matLibTree.findItemByObjectId( %newMat );
	%matLibTree.selectItem( %item );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::cloneMat( %this, %internalName,%tags,%isGroundCoverMat ) {	
	%src = %this.activeMat;

	if (!isObject(%src)) {
		warnLog("Invalid TerrainMaterial to clone from:",%src,"Creating a new material instead.");
		%this.newMat();
		return;
	}

	if (%internalName $= "")
		%internalName = %src.internalName;
		
	// Create a unique material name.
	%matName = getUniqueInternalName( %internalName, TerrainMaterialSet, true );
	// Create the new material.
	%newMat = new TerrainMaterial() {
		internalName = %matName;
		parentGroup = TerrainMaterialDlgNewGroup;
	};
	if ($TerrainMatDlg_CreateFromClone){
		%newMat.assignFieldsFrom(%src);
		if (%newMat.isGroundCoverMat !$= "")
			%newMat.isGroundCoverMat = false;
		if (%isGroundCoverMat)	
			%newMat.isGroundCoverMat = true;
	}		

	%newMat.internalName = %matName;
	
	if (%tags !$= "")
		%newMat.customTags = %tags;
	

	%file = %src.getFileName();
	%newMat.setFileName( %file );
	// Mark it as dirty and to be saved in the default location.
	
	ETerrainMaterialPersistMan.setDirty( %newMat );
	ETerrainMaterialPersistMan.saveDirtyObject( %newMat );
	
	FilteredTerrainMaterialsSet.add(%newMat);
	%this.refreshMaterialTree(%newMat);
	%matLibTree = %this-->matLibTree;
	//%matLibTree.buildVisibleTree( true );
	%matLibTree.schedule(200,"selectItem",%newMat.getId());
	%matLibTree.selectItem(%newMat.getId());
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMatNameEdit::onValidate( %this ) {		
	%matName = getUniqueInternalName( %this.getText(), TerrainMaterialSet, true );
	%this.setText(%matName);
	
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::advCloneMat( %this ) {		
	%gui = TerrainMatDlg_Cloning;
	%name = %gui-->cloneInternalName.getText();
	%tags = %gui-->cloneTags.getText();
	%isGroundCoverMat = %gui-->isGroundCoverMat.isStateOn();
	%this.cloneMat(%name,%tags,%isGroundCoverMat);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::setCloningSource( %this,%source ) {
	if (!isObject(%source)){
			hide(TerrainMatDlg_Cloning);
			return;
	}
	%gui = TerrainMatDlg_Cloning;
	show(%gui);	
	%unique = getUniqueInternalName( %source.internalName@"_1", TerrainMaterialSet, true );
	%gui-->cloneInternalName.setText(%unique);
	%gui-->cloneTags.setText(%source.customTags);
	%gui-->isGroundCoverMat.setStateOn(false);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::deleteMat( %this ) {
	if( !isObject( %this.activeMat ) )
		return;

	// Cannot delete this material if it is the only one left on the Terrain
	if ( ( ETerrainEditor.getMaterialCount() == 1 ) &&
			( ETerrainEditor.getMaterialIndex( %this.activeMat.internalName ) != -1 ) ) {
		LabMsgOK( "Error", "Cannot delete this Material, it is the only " @
					 "Material still in use by the active Terrain." );
		return;
	}

	TerrainMaterialSet.remove( %this.activeMat );
	TerrainMaterialDlgDeleteGroup.add( %this.activeMat );
	%matLibTree = %this-->matLibTree;
	%matLibTree.open( TerrainMaterialSet, false );
	%matLibTree.selectItem( 1 );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainMaterialDlg::setCreateMode( %this,%mode ) {
	switch$(%mode){
		case "Clone":
			$TerrainMatDlg_CreateFromClone = true;
		
			TerrainMatDlg_Cloning-->CloneOptions.visible = 1;
		case "Blank":
		$TerrainMatDlg_CreateFromClone = false;
			TerrainMatDlg_Cloning-->CloneOptions.visible = 0;
	}
}
//------------------------------------------------------------------------------
