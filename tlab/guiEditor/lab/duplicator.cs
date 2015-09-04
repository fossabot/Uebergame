//==============================================================================
// TorqueLab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Field Duplicator
//==============================================================================
%fs = "extent position command altCommand tooltip internalName horizSizing vertSizing profile";
$GuiEditDuplicator_fields = %fs;

//==============================================================================
//Lab.buildDuplicatorParams();
function Lab::buildDuplicatorParams(%this) {
	%arCfg = createParamsArray("GuiEdit_Duplicator",GuiEdit_DuplicatorStack);
	%arCfg.updateFunc = "Lab.updateGuiEditDuplicatorField";
	//%arCfg.style = "LabCfgB_304";
	%arCfg.useNewSystem = true;

	%arCfg.group[%gid++] = "Select fields to copy";
	foreach$(%field in $GuiEditDuplicator_fields)
		%arCfg.setVal(%field,       "" TAB %field TAB "Checkbox" TAB "variable>>$GuiEditDuplicator_"@%field TAB "$GuiEditDuplicator_" TAB %gid);	
	
	buildParamsArray(%arCfg,true);
}
//------------------------------------------------------------------------------
//==============================================================================
// Called from the duplicator fields params and nothing to do yet
function Lab::updateGuiEditDuplicatorField(%this,%field,%value,%ctrl) {	
	
}
//------------------------------------------------------------------------------
//==============================================================================
//Lab.toggleDuplicator();
function Lab::toggleDuplicator(%this) {
	toggleDlg(GuiEditFieldDuplicator);	
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setDuplicatorSource(%this) {
	$GuiEditDuplicator_Source = GuiEditor.getSelection().getObject(0);
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::copyDuplicatorToSelection(%this) {
	%src = $GuiEditDuplicator_Source;
	
	if (!isObject(%src))
		return;
		
	%selection =  GuiEditor.getSelection();
	foreach(%tgt in %selection) {
	//while(isObject(%selection.getObject(%i))) {
		//%tgt = %selection.getObject(%i);
		foreach$(%field in $GuiEditDuplicator_fields){
			if (!$GuiEditDuplicator_[%field])
				continue;
			
			%srcVal = %src.getFieldValue(%field);			
			%tgt.setFieldValue(%field,%srcVal);
		}		
		%i++;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::copySelectedControlData(%this,%dataType) {
	%selection =  GuiEditor.getSelection();
	%control = %selection.getObject(0);
	$GuiEditor_CopiedData["position"] = %control.position;
	$GuiEditor_CopiedData["extent"] = %control.extent;
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::assignFieldsFromReference(%this) {
	%obj = $GuiEditor_ReferenceControl;

	if (!isObject(%obj)) {
		warnLog("No reference object setted");
		return;
	}

	%selection =  GuiEditor.getSelection();
	%i = 0;

	while(isObject(%selection.getObject(%i))) {
		%control = %selection.getObject(%i);
		%control.assignFieldsFrom(%obj);
		%i++;
	}
}
//------------------------------------------------------------------------------