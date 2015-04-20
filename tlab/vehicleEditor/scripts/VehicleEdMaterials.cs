//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
// Material Editing
//------------------------------------------------------------------------------

function VehicleEdMaterials::updateMaterialList( %this ) {
    // --- MATERIALS TAB ---
    VehicleEdMaterialList.clear();
    VehicleEdMaterialList.addRow( -2, "Name" TAB "Mapped" );
    VehicleEdMaterialList.setRowActive( -2, false );
    VehicleEdMaterialList.addRow( -1, "<none>" );
    %count = VehicleEditor.shape.getTargetCount();
    for ( %i = 0; %i < %count; %i++ ) {
        %matName = VehicleEditor.shape.getTargetName( %i );
        %mapped = getMaterialMapping( %matName );
        if ( %mapped $= "" )
            VehicleEdMaterialList.addRow( WarningMaterial.getID(), %matName TAB "unmapped" );
        else
            VehicleEdMaterialList.addRow( %mapped.getID(), %matName TAB %mapped );
    }

    VehicleEdMaterials-->materialListHeader.setExtent( getWord( VehicleEdMaterialList.extent, 0 ) SPC "19" );
}

function VehicleEdMaterials::updateSelectedMaterial( %this, %highlight ) {
    // Remove the highlight effect from the old selection
    if ( isObject( %this.selectedMaterial ) ) {
        %this.selectedMaterial.diffuseMap[1] = %this.savedMap;
        %this.selectedMaterial.reload();
    }

    // Apply the highlight effect to the new selected material
    %this.selectedMapTo = getField( VehicleEdMaterialList.getRowText( VehicleEdMaterialList.getSelectedRow() ), 0 );
    %this.selectedMaterial = VehicleEdMaterialList.getSelectedId();
    %this.savedMap = %this.selectedMaterial.diffuseMap[1];
    if ( %highlight && isObject( %this.selectedMaterial ) ) {
        %this.selectedMaterial.diffuseMap[1] = "tlab/VehicleEditor/images/highlight_material";
        %this.selectedMaterial.reload();
    }
}

function VehicleEdMaterials::editSelectedMaterial( %this ) {
    if ( isObject( %this.selectedMaterial ) ) {
        // Remove the highlight effect from the selected material, then switch
        // to the Material Editor
        %this.updateSelectedMaterial( false );

        // Create a temporary TSStatic so the MaterialEditor can query the model's
        // materials.
        pushInstantGroup();
        %this.tempShape = new TSStatic() {
            shapeName = VehicleEditor.shape.baseShape;
            collisionType = "None";
        };
        popInstantGroup();

        MaterialEditorGui.currentMaterial = %this.selectedMaterial;
        MaterialEditorGui.currentObject = $Lab::materialEditorList = %this.tempShape;

        VehicleEdSelectWindow.setVisible( false );
        VehicleEdPropWindow.setVisible( false );

        EditorGui-->MatEdPropertiesWindow.setVisible( true );
        EditorGui-->MatEdPreviewWindow.setVisible( true );

        MatEd_phoBreadcrumb.setVisible( true );
        MatEd_phoBreadcrumb.command = "VehicleEdMaterials.editSelectedMaterialEnd();";

        advancedTextureMapsRollout.Expanded = false;
        materialAnimationPropertiesRollout.Expanded = false;
        materialAdvancedPropertiesRollout.Expanded = false;

        MaterialEditorGui.open();
        MaterialEditorGui.setActiveMaterial( %this.selectedMaterial );

        %id = SubMaterialSelector.findText( %this.selectedMapTo );
        if( %id != -1 )
            SubMaterialSelector.setSelected( %id );
    }
}

function VehicleEdMaterials::editSelectedMaterialEnd( %this, %closeEditor ) {
    MatEd_phoBreadcrumb.setVisible( false );
    MatEd_phoBreadcrumb.command = "";

    MaterialEditorGui.quit();
    EditorGui-->MatEdPropertiesWindow.setVisible( false );
    EditorGui-->MatEdPreviewWindow.setVisible( false );

    // Delete the temporary TSStatic
    %this.tempShape.delete();

    if( !%closeEditor ) {
        VehicleEdSelectWindow.setVisible( true );
        VehicleEdPropWindow.setVisible( true );
    }
}