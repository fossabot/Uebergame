//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//------------------------------------------------------------------------------
// Material Editing
//------------------------------------------------------------------------------

function LabModelMaterials::updateMaterialList( %this ) {
	// --- MATERIALS TAB ---
	LabModelMaterialList.clear();
	LabModelMaterialList.addRow( -2, "Name" TAB "Mapped" );
	LabModelMaterialList.setRowActive( -2, false );
	LabModelMaterialList.addRow( -1, "<none>" );
	%count = LabModel.shape.getTargetCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%matName = LabModel.shape.getTargetName( %i );
		%mapped = getMaterialMapping( %matName );

		if ( %mapped $= "" )
			LabModelMaterialList.addRow( WarningMaterial.getID(), %matName TAB "unmapped" );
		else
			LabModelMaterialList.addRow( %mapped.getID(), %matName TAB %mapped );
	}

	// LabModelMaterials-->materialListHeader.setExtent( getWord( LabModelMaterialList.extent, 0 ) SPC "19" );
}

function LabModelMaterials::updateSelectedMaterial( %this, %highlight ) {
	// Remove the highlight effect from the old selection
	if ( isObject( %this.selectedMaterial ) ) {
		%this.selectedMaterial.diffuseMap[1] = %this.savedMap;
		%this.selectedMaterial.reload();
	}

	// Apply the highlight effect to the new selected material
	%this.selectedMapTo = getField( LabModelMaterialList.getRowText( LabModelMaterialList.getSelectedRow() ), 0 );
	%this.selectedMaterial = LabModelMaterialList.getSelectedId();
	%this.savedMap = %this.selectedMaterial.diffuseMap[1];

	if ( %highlight && isObject( %this.selectedMaterial ) ) {
		%this.selectedMaterial.diffuseMap[1] = "tlab/LabModel/images/highlight_material";
		%this.selectedMaterial.reload();
	}
}

function LabModelMaterials::editSelectedMaterial( %this ) {
	if ( isObject( %this.selectedMaterial ) ) {
		// Remove the highlight effect from the selected material, then switch
		// to the Material Editor
		%this.updateSelectedMaterial( false );
		// Create a temporary TSStatic so the MaterialEditor can query the model's
		// materials.
		pushInstantGroup();
		%this.tempShape = new TSStatic() {
			shapeName = LabModel.shape.baseShape;
			collisionType = "None";
		};
		popInstantGroup();
		MaterialEditorGui.currentMaterial = %this.selectedMaterial;
		MaterialEditorGui.currentObject = $Lab::materialEditorList = %this.tempShape;
		MatEd_phoBreadcrumb.setVisible( true );
		MatEd_phoBreadcrumb.command = "LabModelMaterials.editSelectedMaterialEnd();";
		Lab.setEditor(MaterialEditorPlugin);
		MaterialEditorGui.open();
		MaterialEditorGui.setActiveMaterial( %this.selectedMaterial );
		%id = SubMaterialSelector.findText( %this.selectedMapTo );

		if( %id != -1 )
			SubMaterialSelector.setSelected( %id );
	}
}

function LabModelMaterials::editSelectedMaterialEnd( %this, %closeEditor ) {
	MatEd_phoBreadcrumb.setVisible( false );
	MatEd_phoBreadcrumb.command = "";
	MaterialEditorGui.quit();
	EditorGui-->MatEdPropertiesWindow.setVisible( false );
	EditorGui-->MatEdPreviewWindow.setVisible( false );
	// Delete the temporary TSStatic
	%this.tempShape.delete();

	if( !%closeEditor ) {
		LabModelSelectWindow.setVisible( true );
		LabModelPropWindow.setVisible( true );
	}
}
