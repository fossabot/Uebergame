//==============================================================================
// TorqueLab -> Terrain Paint Generator System
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Terrain Paint Generator Globals Define
//------------------------------------------------------------------------------
$TPG_CoverageMax = 80;
$TPG_ValidateFields = "heightMin heightMax slopeMin slopeMax coverage";
//------------------------------------------------------------------------------

//==============================================================================
// Initialize the Terrain Paint Generator
function TPG::init(%this) {
	if ( !isObject( TPG_LayerGroup ) ) {
		new SimGroup( TPG_LayerGroup  );
	}

	TPG_StackLayers.clear();
	TPG_LayerFolderMenu.clear();
	TPG_LayerFolderMenu.add("TerrainEditor",0);
	TPG_LayerFolderMenu.add("CurrentLevel",1);
	TPG_LayerFolderMenu.setSelected(0);
	$TerrainPaintGeneratorGui_Initialized = true;
}
//------------------------------------------------------------------------------
//==============================================================================
// Initialize the Terrain Paint Generator
function TPG::exec(%this) {
	exec("tlab/terrainEditor/gui/TerrainPaintGeneratorGui.cs");
	exec("tlab/terrainEditor/scripts/paintGenerator.cs");
}
//------------------------------------------------------------------------------
//==============================================================================
// Make sure the stack and group have valid object and are synced
function TPG::checkLayersStackGroup(%this) {
	foreach(%pill in TPG_StackLayers) {
		if(isObject(%pill.layerObj))
			%addList = strAddWord(%addList,%pill.layerObj);
		else
			%badPill = strAddWord(%badPill,%pill);
	}

	foreach(%layer in TPG_LayerGroup) {
		%inStack = strFind(%addList,%layer.getId());

		if (!strFind(%addList,%layer.getId()))
			%deleteLayers = strAddWord(%deleteLayers,%layer);
	}

	foreach$(%pill in %badPill)
		%pill.delete();

	foreach$(%layer in %deleteLayers)
		%layer.delete();
}
//------------------------------------------------------------------------------
//==============================================================================
// Terrain Paint Generator - Layer Validation Functions
//==============================================================================
function TPG::reportValidationErrors(%this) {
	if ($TPG_LayerValidationErrors !$="")
		LabMsgOk(%layer.internalName SPC "validation failed",$TPG_LayerValidationErrors);

	$TPG_LayerValidationErrors = "";
}
//==============================================================================
// Validate all setting for a single layer
function TPG::validateLayerSettings(%this,%layer,%doPostValidation) {
	%validated = false;
	%layer.failedSettings = "";
	$TPG_LayerValidationErrors = "";
	%this.validateLayerSetting("heightMin",%layer,true);
	%this.validateLayerSetting("heightMax",%layer,true);
	%this.validateLayerSetting("slopeMin",%layer,true);
	%this.validateLayerSetting("slopeMax",%layer,true);
	%this.validateLayerSetting("coverage",%layer,true);
	%this.reportValidationErrors();

	if (%doPostValidation && %layer.failedSettings $= "")
		%validated = %this.postLayerValidation(%layer);
}
//------------------------------------------------------------------------------
//==============================================================================
// Validate a single setting for a layer
function TPG::validateLayerSetting(%this,%setting,%layer,%fullValidation) {
	if (!isObject(%layer)) {
		%error = "Invalid layer submitted for validation. There's no such object:\c2" SPC %layer;
		warnLog(%error,"Setting:",%setting);
		return;
		$TPG_LayerValidationErrors = strAddRecord($TPG_LayerValidationErrors,%error);
		%failed = true;
	}

	%layer.isValidated = false;
	%layer.setFieldValue(%setting,"");
	eval("%ctrl = %layer.pill-->"@%setting@";");

	if (!isObject(%ctrl)) {
		%error = %layer.internalName SPC "layer doesn't have a valid field for:" SPC %setting @ ". Please delete it and report the issue.";
		$TPG_LayerValidationErrors = strAddRecord($TPG_LayerValidationErrors,%error);
		%failed = true;
	} else {
		%value = %ctrl.getValue();

		if (!strIsNumeric(%value)) {
			%error = %setting SPC " is not a numeric value, please change it before being able to generate the layers";
			$TPG_LayerValidationErrors = strAddRecord($TPG_LayerValidationErrors,%error);
			%failed = true;
		}

		if (%value $= "") {
			%error = %setting SPC " is not a numeric value, please change it before being able to generate the layers";
			$TPG_LayerValidationErrors = strAddRecord($TPG_LayerValidationErrors,%error);
			%failed = true;
		}
	}

	if (%failed) {
		%layer.failedSettings = strAddWord(%layer.failedSettings,%setting);

		if (!%fullValidation)
			%this.reportValidationErrors();

		return;
	}

	switch$(%setting) {
	case "heightMin":
		if ($Lab::TerrainPainter::ValidateHeight) {
			if (%value < $TPG_MinHeight)
				%value = $TPG_MinHeight;
		}

	case "heightMax":
		if ($Lab::TerrainPainter::ValidateHeight) {
			if (%value < $TPG_MaxHeight)
				%value = $TPG_MaxHeight;
		}

	case "slopeMin":
		if (%value < 0)
			%value = "0";

	case "slopeMax":
		if (%value > 90)
			%value = "90";

	case "coverage":
		%value = mClamp(%value,"0","99");
	}

	%ctrl.setValue(%value);
	%layer.setFieldValue(%setting,%value);
}
//------------------------------------------------------------------------------
//==============================================================================
// Post validate the settings of a layer globally
function TPG::postLayerValidation(%this,%layer) {
	foreach$(%field in $TPG_ValidateFields) {
		if (%layer.getFieldValue(%field) $="") {
			%error = %setting SPC "is not validated";
			%errors = strAddRecord(%errors,%error);
		}
	}

	if (%errors !$="") {
		LabMsgOk("Layer post validation failed",%errors);
		return false;
	}

	if (%layer.getFieldValue("heightMin") > %layer.getFieldValue("heightMax")) {
		%layer.setFieldValue("heightMin",%layer.getFieldValue("heightMax"));
		%report = "Height min was higher than Height max. The height minimum is changed to fit the maximum.";
		%reports = strAddRecord(%reports,%report);
	}

	if (%reports !$="") {
		LabMsgOk("Layer post validation succeed with warnings",%reports);
	}

	%layer.fieldsValidated = true;
	%layer.isValidated = true;
	return true;
}
//------------------------------------------------------------------------------
