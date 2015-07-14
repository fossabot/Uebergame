//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TPG_AutoStepGeneration = true;

//==============================================================================
function TerrainPaintGeneratorGui::updatePillLayerList(%this,%layer) {
	TPG_PillSample.setVisible(false);
	%pill = %layer.pill;
	%menu = %pill-->menuLayers;
	%menu.clear();
	%mats = ETerrainEditor.getMaterials();
	
	for( %i = 0; %i < getRecordCount( %mats ); %i++ ) {
		%matInternalName = getRecord( %mats, %i );
		%mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );
		$TerrainPainterMatId[%matInternalName] = %i;
		%menu.add(%matInternalName,%i);
	}

	%id = %menu.findText(%layer.matInternalName);

	if (%id $= "-1"){
		%id = 0;
		%menu.setText("**"@%layer.matInternalName@"**");
		return;
	}

	%menu.setSelected(%id);
	
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::addNewLayer(%this) {
	%layer = new ScriptObject();
	%layer.internalName = "Layer_"@%this.lastLayerId++;
	%layer.heightMin = $TPG_MinHeight;
	%layer.heightMax = $TPG_MaxHeight;
	%layer.slopeMin = "0";
	%layer.slopeMax = "90";
	%layer.coverage = $TPG_Coverage;
	%layer.useTerrainHeight = $Lab::TerrainPainter::UseTerrainHeight;
	%matId = 0;
	%this.addLayerPill(%layer,true);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::layerSelected(%this,%menu) {
	%matInternalName = %menu.getText();
	
	%matIndex = %menu.getSelected();
	%mat = TerrainMaterialSet.findObjectByInternalName( %matInternalName );
	%layer = %menu.layerObj;
	%layer.matObject = %mat;
	%layer.matInternalName = %matInternalName;
	%layer.matIndex = %matIndex;
}
//------------------------------------------------------------------------------
//==============================================================================
// Terrain Paint Generator - Terrain Paint Generation Functions
//==============================================================================

//==============================================================================
function TPG_FieldCopyButton::onClick(%this) {
	%layer = %this.layerObj;
	%pill = %layer.pill;
	%wordsData = strreplace(%this.internalName,"_"," ");
	%field = getWord(%wordsData,0);
	%action = getWord(%wordsData,1);
	
	if (%action $= "Copy"){
		TPG.clipBoard = %layer.getFieldValue(%field);
	} else if (%action $= "Paste"){
		if (TPG.clipBoard $= "")
			return;
		%layer.setFieldValue(%field,TPG.clipBoard);
		%ctrl = %pill.findObjectByInternalName(%field,true);
		%ctrl.setText(TPG.clipBoard);
		
	}
	
}
//------------------------------------------------------------------------------
//==============================================================================
function TPG::getCurrentHeight(%this,%id) {
	%avgHeight = ETerrainEditor.lastAverageHeight;
	
	%avgHeight = mFloatLength(%avgHeight,2);
	TPG.clipBoard = %avgHeight; 
	
	if (%id $= "")
		return;
	TPG.height[%id] = %avgHeight;
	$TPG_ValueStore[%id] = %avgHeight;
	eval("TPG_StoredValues-->v"@%id@".setText(\""@%avgHeight@"\");");
}
//------------------------------------------------------------------------------
//==============================================================================
function TPG::setCurrentHeight(%this,%id) {
	
	if (TPG.height[%id] $= "")
		return;
	TPG.clipBoard = TPG.height[%id]; 		
}
//------------------------------------------------------------------------------
//==============================================================================
function TPG::pasteCurrentHeight(%this,%id) {
	
	if (TPG.clipBoard $= "")
		return;
	TPG.height[%id] = TPG.clipBoard; 
	$TPG_ValueStore[%id] = TPG.clipBoard;
	eval("TPG_StoredValues-->v"@%id@".setText(\""@TPG.clipBoard@"\");");
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::addLayerPill(%this,%layer,%update) {
	if (!isObject(%layer)) return;

	%srcPill = TPG_PillSample_v2;
	hide(%srcPill);
	%layer.pill = %srcPill.deepClone();
	%layer.pill.setName("");
	%layer.pill.setVisible(true);
	%layer.pill-->menuLayers.layerObj = %layer;
	%layer.pill-->edit.layerObj = %layer;
	%layer.pill-->delete.layerObj = %layer;
	%layer.pill-->up.pill = %layer.pill;
	%layer.pill-->down.pill = %layer.pill;
	%layer.pill.layerObj = %layer;
	%layer.pill-->heightMin.layerObj = %layer;
	%layer.pill-->heightMax.layerObj = %layer;
	%layer.pill-->slopeMin.layerObj = %layer;
	%layer.pill-->slopeMax.layerObj = %layer;
	%layer.pill-->coverage.layerObj = %layer;
	%layer.pill-->heightMin_Copy.layerObj = %layer;
	%layer.pill-->heightMax_Copy.layerObj = %layer;
	%layer.pill-->slopeMin_Copy.layerObj = %layer;
	%layer.pill-->slopeMax_Copy.layerObj = %layer;
	%layer.pill-->heightMin_Paste.layerObj = %layer;
	%layer.pill-->heightMax_Paste.layerObj = %layer;
	%layer.pill-->slopeMin_Paste.layerObj = %layer;
	%layer.pill-->slopeMax_Paste.layerObj = %layer;
	%layer.pill-->coverageCopy.layerObj = %layer;
	//%layer.pill-->overlayCheck.layerObj = %layer;
	//%layer.pill-->overlayCheck.setStateOn(false);
	//%layer.pill-->overlayCheck.text = "";
	%layer.activeCtrl = %layer.pill-->checkbox;
	%layer.activeCtrl.text = "";
	%layer.activeCtrl.setStateOn(true);
	TPG_StackLayers.add(%layer.pill);
	%this.updatePillLayerList(%layer);
	
	if (%update)
		%this.updateLayerPill(%layer);

}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::updateLayerPill(%this,%layer) {
	if (!isObject(%layer) || !isObject(%layer.pill)) return;
	
	%pill = %layer.pill;
	%menu = %pill-->menuLayers;
	
	
	%matName = %layer.matInternalName;
	if (%layer.matInternalName $= ""){
		warnLog("Invalid layer found:",%layer,"Name set to Unnamed");
		%layer.matInternalName = "Unnamed";
	}
	%id = %menu.findText(%layer.matInternalName);
	if (%id > 0){		
		%menu.setSelected(%id);
	}
	else {
		if (!strFind(%layer.matInternalName,"*"))
			%layer.matInternalName = "**"@%layer.matInternalName@"**";
		//The assigned material is not found anymore, put the name between stars to warn
		%menu.setText(%layer.matInternalName);
	}
	
	
	
	
	%layer.pill.layerObj = %layer;
	%layer.pill-->heightMin.setText(%layer.heightMin);
	%layer.pill-->heightMax.setText( %layer.heightMax);
	%layer.pill-->slopeMin.setText(%layer.slopeMin);
	%layer.pill-->slopeMax.setText(%layer.slopeMax);
	%layer.pill-->coverage.setText(%layer.coverage);
	TPG_LayerGroup.add(%layer);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::cancelLayerChanges(%this) {
	TPG_LayerWindow.setVisible(false);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::moveUp(%this, %buttonCtrl) {
	%pillIndex = TPG_StackLayers.getObjectIndex(%buttonCtrl.pill);

	if (%pillIndex <= 0) return;

	%targetPill = TPG_StackLayers.getObject(%pillIndex - 1);
	TPG_StackLayers.reorderChild(%buttonCtrl.pill, %targetPill);
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainPaintGeneratorGui::moveDown(%this, %buttonCtrl) {
	%pillIndex = TPG_StackLayers.getObjectIndex(%buttonCtrl.pill);

	if (%pillIndex > TPG_StackLayers.getCount() - 1) return;

	%targetPill = TPG_StackLayers.getObject(%pillIndex + 1);
	TPG_StackLayers.reorderChild(%targetPill, %buttonCtrl.pill);
}
//------------------------------------------------------------------------------
//==============================================================================
function TPG_PillEdit::onValidate(%this) {	
	TPG.validateLayerSetting(%this.internalName,%this.layerObj);
}
//------------------------------------------------------------------------------
