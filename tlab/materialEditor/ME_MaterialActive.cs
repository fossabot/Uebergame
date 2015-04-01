//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function MaterialEditorGui::prepareActiveObject( %this, %override ) {
    %obj = $Lab::materialEditorList;
    if( MaterialEditorGui.currentObject == %obj && !%override)
        return;

    // TSStatics and ShapeBase objects should have getModelFile methods
    if( %obj.isMethod( "getModelFile" ) ) {
        MaterialEditorGui.currentObject = %obj;

        SubMaterialSelector.clear();
        MaterialEditorGui.currentMeshMode = "Model";

        MaterialEditorGui.setMode();

        for(%j = 0; %j < MaterialEditorGui.currentObject.getTargetCount(); %j++) {
            %target = MaterialEditorGui.currentObject.getTargetName(%j);
            %count = SubMaterialSelector.getCount();
            SubMaterialSelector.add(%target);
        }
    } else { // Other classes that support materials if possible
        %canSupportMaterial = false;
        for( %i = 0; %i < %obj.getFieldCount(); %i++ ) {
            %fieldName = %obj.getField(%i);

            if( %obj.getFieldType(%fieldName) !$= "TypeMaterialName" )
                continue;

            if( !%canSupportMaterial ) {
                MaterialEditorGui.currentObject = %obj;
                SubMaterialSelector.clear();
                SubMaterialSelector.add(%fieldName, 0);
            } else {
                %count = SubMaterialSelector.getCount();
                SubMaterialSelector.add(%fieldName, %count);
            }
            %canSupportMaterial = true;
        }

        if( !%canSupportMaterial ) // Non-relevant classes get returned
            return;

        MaterialEditorGui.currentMeshMode = "EditorShape";
        MaterialEditorGui.setMode();
    }

    %id = SubMaterialSelector.findText( MaterialEditorGui.currentMaterial.mapTo );
    if( %id != -1 )
        SubMaterialSelector.setSelected( %id );
    else
        SubMaterialSelector.setSelected(0);
}
//==============================================================================
// SubMaterial(Material Target) -- Supports different ways to grab the
// material from the dropdown list. We're here either because-
// 1. We have switched over from another editor with an object locked in the
//    $Lab::materialEditorList variable
// 2. We have selected an object using the Object Editor via the Material Editor

function SubMaterialSelector::onSelect( %this ) {
    %material = "";

    if( MaterialEditorGui.currentMeshMode $= "Model" )
        %material = getMapEntry( %this.getText() );
    else
        %material = MaterialEditorGui.currentObject.getFieldValue( %this.getText() );

    %origMat = %material;
    if(%material$="")
        %origMat = %material = %this.getText();
    // if there is no material attached to that objects material field or the
    // object does not have a valid method to grab a material
    if( !isObject( %material ) ) {
        // look for a newMaterial name to grab
        // addiitonally, convert "." to "_" in case we have something like: "base.texname" as a material name
        // at the end we will have generated material name: "base_texname_mat"
        %material = getUniqueName( strreplace(%material, ".", "_") @ "_mat" );

        new Material(%material) {
            diffuseMap[0] = %origMat;
            mapTo = %origMat;
            parentGroup = RootGroup;
        };

        eval( "MaterialEditorGui.currentObject." @ strreplace(%this.getText(),".","_") @ " = " @ %material @ ";");

        if( MaterialEditorGui.currentObject.isMethod("postApply") )
            MaterialEditorGui.currentObject.postApply();
    }

    MaterialEditorGui.prepareActiveMaterial( %material.getId() );
}

//==============================================================================
// Helper functions to help load and update the preview and active material

// Finds the selected line in the material list, then makes it active in the editor.
function MaterialEditorGui::prepareActiveMaterial(%this, %material, %override) {
    // If were not valid, grab the first valid material out of the materialSet
    if( !isObject(%material) )
        %material = MaterialSet.getObject(0);

    // Check made in order to avoid loading the same material. Overriding
    // made in special cases
    if(%material $= MaterialEditorGui.lastMaterial && !%override) {
        return;
    } else {
        if(MaterialEditorGui.materialDirty ) {
            MaterialEditorGui.showSaveDialog( %material );
            return;
        }

        MaterialEditorGui.setActiveMaterial(%material);
    }
}

// Updates the preview material to use the same properties as the selected material,
// and makes that material active in the editor.
function MaterialEditorGui::setActiveMaterial( %this, %material ) {
    // Warn if selecting a CustomMaterial (they can't be properly previewed or edited)
    if ( isObject( %material ) && %material.isMemberOfClass( "CustomMaterial" ) ) {
        LabMsgOK( "Warning", "The selected Material (" @ %material.getName() @
                       ") is a CustomMaterial, and cannot be edited using the Material Editor." );
        return;
    }

    MaterialEditorGui.currentMaterial = %material;
    MaterialEditorGui.lastMaterial = %material;

    // we create or recreate a material to hold in a pristine state
    singleton Material(notDirtyMaterial) {
        mapTo = "matEd_previewMat";
        diffuseMap[0] = "tlab/materialEditor/assets/matEd_mappedMat";
    };

    // Converts the texture files into absolute paths.
    MaterialEditorGui.convertTextureFields();

    // If we're allowing for name changes, make sure to save the name seperately
    %this.originalName = MaterialEditorGui.currentMaterial.name;

    // Copy materials over to other references
    MaterialEditorGui.copyMaterials( MaterialEditorGui.currentMaterial, materialEd_previewMaterial );
    MaterialEditorGui.copyMaterials( MaterialEditorGui.currentMaterial, notDirtyMaterial );
    MaterialEditorGui.guiSync( materialEd_previewMaterial );

    // Necessary functionality in order to render correctly
    materialEd_previewMaterial.flush();
    materialEd_previewMaterial.reload();
    MaterialEditorGui.currentMaterial.flush();
    MaterialEditorGui.currentMaterial.reload();

    MaterialEditorGui.setMaterialNotDirty();
}


function MaterialEditorGui::updateActiveMaterial(%this, %propertyField, %value, %isSlider, %onMouseUp) {
    MaterialEditorGui.setMaterialDirty();

    if(%value $= "")
        %value = "\"\"";

    // Here is where we handle undo actions with slider controls. We want to be able to
    // undo every onMouseUp; so we overrite the same undo action when necessary in order
    // to achieve this desired effect.
    %last = Editor.getUndoManager().getUndoAction(Editor.getUndoManager().getUndoCount() - 1);
    if((%last != -1) && (%last.isSlider) && (!%last.onMouseUp)) {
        %last.field = %propertyField;
        %last.isSlider = %isSlider;
        %last.onMouseUp = %onMouseUp;
        %last.newValue = %value;
    } else {
        %action = %this.createUndo(ActionUpdateActiveMaterial, "Update Active Material");
        %action.material = MaterialEditorGui.currentMaterial;
        %action.object = MaterialEditorGui.currentObject;
        %action.field = %propertyField;
        %action.isSlider = %isSlider;
        %action.onMouseUp = %onMouseUp;
        %action.newValue = %value;
        eval( "%action.oldValue = " @ MaterialEditorGui.currentMaterial @ "." @ %propertyField @ ";");
        %action.oldValue = "\"" @ %action.oldValue @ "\"";
        MaterialEditorGui.submitUndo( %action );
    }

    eval("materialEd_previewMaterial." @ %propertyField @ " = " @ %value @ ";");
    materialEd_previewMaterial.flush();
    materialEd_previewMaterial.reload();

    if (MaterialEditorGui.livePreview == true) {
        eval("MaterialEditorGui.currentMaterial." @ %propertyField @ " = " @ %value @ ";");
        MaterialEditorGui.currentMaterial.flush();
        MaterialEditorGui.currentMaterial.reload();
    }
}

function MaterialEditorGui::updateActiveMaterialName(%this, %name) {
    %action = %this.createUndo(ActionUpdateActiveMaterialName, "Update Active Material Name");
    %action.material =  MaterialEditorGui.currentMaterial;
    %action.object = MaterialEditorGui.currentObject;
    %action.oldName = MaterialEditorGui.currentMaterial.getName();
    %action.newName = %name;
    MaterialEditorGui.submitUndo( %action );

    MaterialEditorGui.currentMaterial.setName(%name);

    // Some objects (ConvexShape, DecalRoad etc) reference Materials by name => need
    // to find and update all these references so they don't break when we rename the
    // Material.
    MaterialEditorGui.updateMaterialReferences( MissionGroup, %action.oldName, %action.newName );
}

singleton Material(baseTkMudRipple4_WRWB_mat)
{
   mapTo = "baseTkMudRipple4_WRWB";
   diffuseMap[0] = "art/textures/Tracks/Ripple/MudRipples/MudRippleB_WRWB.png";
};

singleton Material(baseTkTireStackA_Black_mat)
{
   mapTo = "baseTkTireStackA_Black";
   diffuseMap[0] = "art/textures/Tracks/Objects/Tires/TireStackA_Black.png";
};

singleton Material(baseTkConcWallA_StripeA_mat)
{
   mapTo = "baseTkConcWallA_StripeA";
   diffuseMap[0] = "art/textures/Tracks/Border/ConcWallA/ccWallA_1RedStripe.png";
   materialTag0 = "Miscellaneous";
};

singleton Material(baseTkMetalFenceA_mat)
{
   mapTo = "baseTkMetalFenceA";
   diffuseMap[0] = "art/textures/Tracks/Fence/WallFenceA/TrackWire_A.png";
   materialTag0 = "Miscellaneous";
};

singleton Material(baseTkSimpleWallA_mat)
{
   mapTo = "baseTkSimpleWallA";
   diffuseMap[0] = "art/textures/Tracks/Border/SimpleWallA/sWallA_Base.png";
   materialTag0 = "Miscellaneous";
};

singleton Material(baseTkSimpleWallA_AdA_mat)
{
   mapTo = "baseTkSimpleWallA_AdA";
   diffuseMap[0] = "art/textures/Tracks/Border/SimpleWallA/sWallA_AdA.png";
   materialTag0 = "Miscellaneous";
};

singleton Material(baseTkSimpleWallA_AdB_mat)
{
   mapTo = "baseTkSimpleWallA_AdB";
   diffuseMap[0] = "art/textures/Tracks/Border/SimpleWallA/sWallA_AdB.png";
   materialTag0 = "Miscellaneous";
};
