//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// TerrainPainterPlugin
//==============================================================================

function EPainter::onActivated( %this ) {
	%this.setDisplayModes();

	// Update the layer listing.
	//%this.updateLayers( %matIndex );
	// Automagically put us into material paint mode.
	ETerrainEditor.currentMode = "paint";
	ETerrainEditor.selectionHidden = true;
	ETerrainEditor.currentAction = paintMaterial;
	ETerrainEditor.currentActionDesc = "Paint material on terrain";
	ETerrainEditor.setAction( ETerrainEditor.currentAction );
	EditorGuiStatusBar.setInfo(ETerrainEditor.currentActionDesc);
	ETerrainEditor.renderVertexSelection = true;
	EPainter-->saveDirtyMaterials.active = 0;
}


//==============================================================================
// Painter Active Layers Functions
//==============================================================================


//==============================================================================
// Update the active material layers list
function EPainter::setPaintLayer( %this, %matIndex) {
	%ctrl = EPainterStack.findObjectByInternalName("Layer_"@%matIndex,true);
	%terrainMat = %ctrl.terrainMat;	
	%pill = EPainterStack.getObject(%matIndex);

	%this.updateSelectedLayerList(%ctrl);
	
	foreach(%ctrl in EPainterStack)
		%ctrl.isActiveCtrl.visible = 0;

	%pill.isActiveCtrl.visible = 1;

	if (!isObject( %terrainMat )) {
		warnLog("Set Paint Layer to invalid material:",%terrainMat);
		return;
	}

	//assert( isObject( %terrainMat ), "TerrainEditor::setPaintMaterial - Got bad material!" );
	ETerrainEditor.paintIndex = %matIndex;
	ETerrainMaterialSelected.selectedMatIndex = %matIndex;
	ETerrainMaterialSelected.selectedMat = %terrainMat;
	ETerrainMaterialSelected.bitmap = %terrainMat.diffuseMap;
	ETerrainMaterialSelected_N.bitmap = %terrainMat.normalMap;
	ETerrainMaterialSelected_M.bitmap = %terrainMat.macroMap;
	ETerrainMaterialSelectedEdit.Visible = isObject(%terrainMat);
	TerrainTextureText.text = %terrainMat.getInternalName();
	ProceduralTerrainPainterDescription.text = "Generate "@ %terrainMat.getInternalName() @" layer";
}

//------------------------------------------------------------------------------


//==============================================================================
// Update the active material layers list
function PainterLayerEdit::onValidate( %this) {
	devLog("PainterLayerEdit onValidate:: FIELD:",%this.internalName,"Mat:",%this.mat);
	%this.mat.setFieldValue(%this.internalName,%this.getText());
	EPainter.setMaterialDirty( %this.mat,%this.nameCtrl );
	
	%this.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function PainterLayerSlider::onMouseDragged( %this) {	
	%field = strreplace(%this.internalName,"Slider","");
	%this.mat.setFieldValue(%field,%this.getValue());
	EPainter.setMaterialDirty( %this.mat,%this.nameCtrl );
	
	%this.updateFriends();
}
//------------------------------------------------------------------------------
//==============================================================================
// Update the active material layers list
function PainterLayerMouse::onMouseDown( %this,%modifier,%pos,%clicks) {
	logd("PainterLayerMouse onMouseDown::",%modifier,%pos,%clicks);

	if (%clicks > 1){
		eval(%this.altCommand);
	}
	else{		
		eval(%this.command);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
function EPainter::showMaterialDeleteDlg( %this, %matInternalName ) {
	LabMsgYesNo( "Confirmation",
					 "Really remove material '" @ %matInternalName @ "' from the terrain?",
					 %this @ ".removeMaterial( " @ %matInternalName @ " );", "" );
}
//------------------------------------------------------------------------------
//==============================================================================
function EPainter::removeMaterial( %this, %matInternalName ) {
	%selIndex = ETerrainEditor.paintIndex - 1;
	// Remove the material from the terrain.
	%index = ETerrainEditor.getMaterialIndex( %matInternalName );

	if( %index != -1 )
		ETerrainEditor.removeMaterial( %index );

	// Update the material list.
	%this.updateLayers( %selIndex );
}
//------------------------------------------------------------------------------
//==============================================================================
// EPainter Set Dirty / Save materials
//==============================================================================
//==============================================================================
function EPainter::setMaterialDirty( %this,%mat,%nameCtrl ) {
	if (isObject(%nameCtrl)) {
		%nameCtrl.setText("*" SPC %mat.internalName);
		$PainterDirtyNames = strAddWord($PainterDirtyNames,%nameCtrl,true);
	}

	// Mark the material as dirty and needing saving.
	%fileName = %mat.getFileName();

	if( %fileName $= "" )
		%fileName = "art/terrains/materials.cs";

	ETerrainMaterialPersistMan.setDirty( %mat, %fileName );
	EPainter-->saveDirtyMaterials.active = 1;
}
//------------------------------------------------------------------------------
//==============================================================================
function EPainter::saveDirtyMaterials( %this ) {
	ETerrainMaterialPersistMan.saveDirty();

	foreach$(%nameCtrl in $PainterDirtyNames) {
		%text= strreplace(%nameCtrl.text,"* ","");
		%nameCtrl.text = %text;
	}

	EPainter-->saveDirtyMaterials.active = 0;
}
//------------------------------------------------------------------------------
//==============================================================================
// LAYER DRAG AND DROP SYSTEM
//==============================================================================
//==============================================================================
// Update the active material layers list
function PainterLayerMouse::onMouseDragged( %this,%modifier,%pos,%clicks) {
	%dragClone = %this.baseCtrl;

	if (isObject(%this.dragClone))
		%dragClone = %this.dragClone;

	%dragClone.layerId = %this.baseCtrl.layerId;

	foreach(%ctrl in EPainterStack)
		%ctrl.dropLayer.visible = 1;

	startDragAndDropCtrl(%dragClone,"TerrainLayer","","EPainter.layerDragFailed();");
}
//------------------------------------------------------------------------------

//==============================================================================
// Update the active material layers list
function LayerDropClass::onControlDropped(%this, %control, %dropPoint) {
	
	%droppedOnLayer = %this.layerID;
	%droppedLayer = %control.dragSourceControl.layerID;
	ETerrainEditor.reorderMaterial(%droppedLayer,%droppedOnLayer);
	EPainter.setPaintLayer(%droppedOnLayer);
	EPainter.updateLayers();

	foreach(%ctrl in EPainterStack)
		%ctrl.dropLayer.visible = 0;
}
//==============================================================================
// Update the active material layers list
function EPainter::layerDragFailed( %this,%modifier,%pos,%clicks) {	
	foreach(%ctrl in EPainterStack)
		%ctrl.dropLayer.visible = 0;
}
//------------------------------------------------------------------------------
