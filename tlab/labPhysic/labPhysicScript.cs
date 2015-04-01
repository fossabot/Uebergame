//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function LabPhysicPlugin::createShape(%this) {
    %datablock = %this.getSelectedDatablock();
    if (!isObject(%datablock)) {
        LabMsgOK("No datablock selected","You need to select a datablock before creating a shape!");
        return;
    }
    ColladaImportDlg.showDialog( "scripts/game/physics/cube.dae", "SceneCreatorWindow.createObject( \"PhysicsShapeData::create("@%datablock.getName()@");\" );" );
}

