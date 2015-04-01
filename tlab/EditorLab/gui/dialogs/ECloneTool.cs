//==============================================================================
// Lab Helpers ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Profile FONTS manipulation functions
//==============================================================================

//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneTool::toggleVisibility( %this ) {
	if ( %this.visible  ) {
		%this.setVisible(false);
		//SnapToBar-->snappingSettingsBtn.setStateOn(false);
	} else {
		%this.setVisible(true);
		%this.selectWindow();
		%this.setCollapseGroup(false);
		//SnapToBar-->snappingSettingsBtn.setStateOn(true);
	}

}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneTool::onWake( %this ) {
	%this-->copyCount.text = "1";
	%this-->copyOffsetX.text = "0";
	%this-->copyOffsetY.text = "0";
	%this-->copyOffsetZ.text = "0";
}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneTool::doCopy( %this ) {

	%copyCount = %this-->copyCount.getValue();
	%offsetX = %this-->copyOffsetX.getValue();
	%offsetY = %this-->copyOffsetY.getValue();
	%offsetZ = %this-->copyOffsetZ.getValue();
	%count = EWorldEditor.getSelectionSize();
	if (%count < 1) {
		warnLog("There's no selected objects to copy!");
		return;
	}
	for (%i=1; %i<=%copyCount; %i++) {
		for( %j=0; %j<%count; %j++) {
			%obj = EWorldEditor.getSelectedObject( %j );
			if( !%obj.isMemberOfClass("SceneObject") ) continue;

			%clone = %obj.clone();

			%clone.position.x += %offsetX * %i;
			%clone.position.y += %offsetY * %i;
			%clone.position.z += %offsetZ * %i;
			MissionGroup.add(%clone);
		}

	}

}
//------------------------------------------------------------------------------
//==============================================================================
//FONTS -> Change the font to all profile or only those specified in the list
function ECloneTool::setCurrentOffset( %this,%axis ) {

	%obj = EWorldEditor.getSelectedObject(0);
	%size = %obj.getObjectBox();
	%scale = %obj.getScale();

	%sizex = (getWord(%size, 3) - getWord(%size, 0)) * getWord(%scale, 0);
	%sizey = (getWord(%size, 4) - getWord(%size, 1)) * getWord(%scale, 0);
	%sizez = (getWord(%size, 5) - getWord(%size, 2)) * getWord(%scale, 0);

	%textEdit = ECloneTool.findObjectByInternalName("copyOffset"@%axis,true);
	//if (!isObject(%textEdit)) return;

	%textEdit.setValue(%size[%axis]);

}
//------------------------------------------------------------------------------