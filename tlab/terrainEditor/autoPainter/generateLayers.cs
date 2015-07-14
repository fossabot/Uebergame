//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$TPG_AutoStepGeneration = true;
//==============================================================================
// Terrain Paint Generator - Terrain Paint Generation Functions
//==============================================================================

//==============================================================================
// Generate all the layers in the layer group
function TPG::generateLayerGroup(%this) {
	//First make sure layers data is right
	TPG.checkLayersStackGroup();

	foreach(%layer in TPG_LayerGroup) {
		%layer.inactive = false;
		%layer.failedSettings = "";
		if ( !%layer.activeCtrl.isStateOn()) {
			%layer.inactive = true;
			devLog("Skipping inactive layer:",%layer.matInternalName);
			continue;
		}
	
		
		%layer.fieldsValidated = false;
		%validated = %this.validateLayerSettings(%layer,true);
		
			if (strFind(%layer.matInternalName,"*"))
		 	%layer.failedSettings = "Invalid material assigned to layer, please select a valid from the menu";	

		if (%layer.failedSettings !$= "") {
			%validationgFailed = true;
			%badLayers = strAddRecord(%badLayers,%layer.internalName SPC "Bad fields:" SPC %layer.failedSettings);
		}
	}

	if (%validationgFailed) {
		if (%badLayers !$="" ) {
			LabMsgOk("Generation aborted!","Some layers have fail the validation. Here's the list:\c2" SPC  %badLayers @ ". Please fix them before attempting generation again.");
		}

		return;
	}

	show(TPG_GenerateProgressWindow);
	TPG_GenerateLayerStack.clear();

	foreach(%layer in TPG_LayerGroup) {
		if (%layer.inactive)
			continue;			
		
		%heightMin = %layer.getFieldValue("heightMin");
		%heightMax = %layer.getFieldValue("heightMax");
		%slopeMin = %layer.getFieldValue("slopeMin");
		%slopeMax = %layer.getFieldValue("slopeMax");
		%coverage = %layer.getFieldValue("coverage");
		
	
		%pill = cloneObject(TPG_GenerateInfoPill_v2,"",%layer.internalName,TPG_GenerateLayerStack);
		%pill.internalName = %layer.internalName;
		%pill-->id.text = "#"@%layerId++ @ "-";
		%pill-->material.text = %layer.matInternalName;
		%pill-->slopeMinMax.setText(%slopeMin SPC "/" SPC %slopeMax);
		%pill-->heightMinMax.text = %heightMin SPC "/" SPC %heightMax;
		%pill-->coverage.text = %coverage;
		%pill-->duration.text = "pending";
	}

	if (%layerId < 1) {
		LabMsgOk("No active layers","There's no active layers to generate terrain materials, operation aborted");
		hide(TPG_GenerateProgressWindow);
		return;
	}
	TPG_GenerateProgressWindow-->cancelButton.visible = 1;
	TPG_GenerateProgressWindow-->reportButton.text = "Start process";
		TPG_GenerateProgressWindow-->reportButton.active = 1;
$TPG_GenerationStatus = "Pending";
TPG_GenerateProgressWindow-->reportText.text = %layerId SPC "layers  are ready to be processed";

	//LabMsgYesNo("Early development feature","The terrain pain generator is still in early development and can cause the engine to freeze. "@
//					"We recommend you to save your work before proceeding with automated painting. Are you sure you want to start the painting process?","TPG.schedule(1000,\"startGeneration\");");
	//%this.schedule(1000,"startGeneration");
}
//------------------------------------------------------------------------------
function TPG_ReportButton::onClick(%this) {
	devLog("OnCLick");
	switch$($TPG_GenerationStatus){
		case "Pending":
			%this.text = "Processing";
			%this.active = false;
			TPG_GenerateProgressWindow-->cancelButton.visible = 0;
			TPG.startGeneration();
		
		case "Completed":
			hide(TPG_GenerateProgressWindow);
	}
}

//==============================================================================
// Start the generation process now that everything is validated
function TPG::startGeneration(%this) {
	if ($TPG_Generating) return;

	TPG.generationStartTime = $Sim::Time;
	$TPG_Generating = true;
	TPG.generatorSteps = "";

	foreach(%layer in TPG_LayerGroup) {
		if (%layer.inactive)
			continue;

		if ($TPG_StepGeneration)
			TPG.generatorSteps = strAddWord(TPG.generatorSteps,%layer.getId());
		else if ($TPG_AutoStepGeneration)
			TPG.generatorSteps = strAddWord(TPG.generatorSteps,%layer.getId());
		else
			%this.generateLayer(%layer);
	}

	$TPG_Generating = false;
	//TPG_GenerateProgressWindow.setVisible(false);

	if ($TPG_StepGeneration)
		%this.doGenerateLayerStep(500);
	else if ($TPG_AutoStepGeneration)
		%this.doGenerateLayerStep(200);
}
//------------------------------------------------------------------------------

function TPG::doGenerateLayerStep(%this,%delay) {
	%layer = getWord(TPG.generatorSteps,0);

	if (%layer $= "")
		return;
		
	%pill = TPG_GenerateLayerStack.findObjectByInternalName(%layer.internalName,true);
	%pill-->duration.text = "Processing";
	TPG_GenerateProgressWindow-->reportText.text ="Processing layer:" SPC %layer.matInternalName @"." SPC getWordCount(TPG.generatorSteps) SPC "left to process.";
	TPG.generatorSteps = removeWord(TPG.generatorSteps,0);
	$TPG_LayerStartTime = $Sim::Time;
	devLog(%layer.matInternalName," Start time:",$TPG_LayerStartTime,"From sim:",$Sim::Time);
	%this.schedule(%delay,"generateLayer",%layer,true);
}
//==============================================================================
// Tell the engine to generate a layer with approved settings
function TPG::generateLayer(%this,%layer,%stepMode) {
	
	ETerrainEditor.paintIndex = %layer.matIndex;
	%heightMin = %layer.getFieldValue("heightMin");
	%heightMax = %layer.getFieldValue("heightMax");
	%slopeMin = %layer.getFieldValue("slopeMin");
	%slopeMax = %layer.getFieldValue("slopeMax");
	%coverage = %layer.getFieldValue("coverage");

	if (!%layer.useTerrainHeight) {
		%heightMin += $Lab_TerrainZ;
		%heightMax += $Lab_TerrainZ;
	}
	
	if (%stepMode)
		info("Step Painting terrain with Mat Index",%layer.matIndex,"Name",%layer.matInternalName,"Height and Slope",%heightMin, %heightMax, %slopeMin, %slopeMax,"Coverage",%coverage);
	else
		info("Painting terrain with Mat Index",%layer.matIndex,"Name",%layer.matInternalName,"Height and Slope",%heightMin, %heightMax, %slopeMin, %slopeMax,"Coverage",%coverage);

	ETerrainEditor.autoMaterialLayer(%heightMin, %heightMax, %slopeMin, %slopeMax,%coverage);
	%this.generateLayerCompleted(%layer,%stepMode);
}
//------------------------------------------------------------------------------

//==============================================================================
// Tell the engine to generate a layer with approved settings
function TPG::generateLayerCompleted(%this,%layer,%stepMode) {
	TPG.generationTotalTime = $Sim::Time - TPG.generationStartTime;
	%layerTime = $Sim::Time - $TPG_LayerStartTime;
	devLog(%layer.matInternalName," Start time:",$TPG_LayerStartTime,"New Sim:",$Sim::Time,"Diff",%layerTime);
	%pill = TPG_GenerateLayerStack.findObjectByInternalName(%layer.internalName,true);
	%pill-->duration.text = mFloatLength(%layerTime,2) SPC "sec";
	if (%stepMode && getWord(TPG.generatorSteps,0) !$= ""){
		
		if ($TPG_AutoStepGeneration)
			TPG.doGenerateLayerStep(200);
		else
			LabMsgYesNo(%layer.matInternalName SPC "step completed","Do you want to proceed with next step:" SPC getWord(TPG.generatorSteps,0).matInternalName SPC "?","TPG.doGenerateLayerStep(500);");
	}
	else{
		$TPG_GenerationStatus = "Completed";
		TPG_GenerateProgressWindow-->reportText.text ="All layers have been processed. It took " @ TPG.generationTotalTime @ "sec to complete";
		TPG_GenerateProgressWindow-->reportButton.text = "Close report";
		TPG_GenerateProgressWindow-->reportButton.active = 1;
		//LabMsgOk("Terrain painting completed","All layers have been successfully painted over the terrain. It took" SPC TPG.generationTotalTime SPC "to complete the process.");
	}
}
//------------------------------------------------------------------------------