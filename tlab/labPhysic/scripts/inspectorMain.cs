//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//=============================================================================================
//    Events.
//=============================================================================================

//---------------------------------------------------------------------------------------------
function Lab::updateLabPhysicInspectorField( %this, %field, %value ) {


    %data = LabPhysicPlugin.getSelectedDatablock();
    if (!isObject(%data)) return;

    %previousValue = %data.getFieldValue(%field);
    %data.setFieldValue(%field,%value);
    LabPhysicPlugin.flagDatablockAsDirty( %data, true );
    %data.reloadOnLocalClient();
}

// Mouse was moved in this control, causing the callback

function LabPhysicInspector::onFieldSelected( %this, %fieldName, %fieldTypeStr, %fieldDoc ) {
    PhysicShapeFieldInfoControl.setText( "<font:ArialBold:14>" @ %fieldName @ "<font:ArialItalic:14> (" @ %fieldTypeStr @ ") " NL "<font:Arial:14>" @ %fieldDoc );
}

//---------------------------------------------------------------------------------------------


//---------------------------------------------------------------------------------------------

function LabPhysicInspector::onClear( %this ) {
    PhysicShapeFieldInfoControl.setText( "" );
}