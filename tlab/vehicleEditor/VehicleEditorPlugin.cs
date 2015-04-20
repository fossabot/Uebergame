//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


// Replace the command field in an Editor PopupMenu item (returns the original value)
function VehicleEditorPlugin::replaceMenuCmd(%this, %menuTitle, %id, %newCmd) {
   if (!$Cfg_UseCoreMenubar) return;
    %menu = Lab.findMenu( %menuTitle );
    %cmd = getField( %menu.item[%id], 2 );
    %menu.setItemCommand( %id, %newCmd );

    return %cmd;
}

function VehicleEditorPlugin::onWorldEditorStartup(%this) {
    Parent::onWorldEditorStartup( %this );

    // Add ourselves to the Editor Settings window
   
    VehicleEditorAnimDlg.resize( -1, 526, 593, 53 );

    // Initialise gui
    VehicleEdSeqNodeTabBook.selectPage(0);
    VehicleEdAdvancedWindow-->tabBook.selectPage(0);
    VehicleEdSelectWindow-->tabBook.selectPage(0);
    VehicleEdSelectWindow.navigate("");

    SetToggleButtonValue( VehicleEditorToolbar-->orbitNodeBtn, 0 );
    SetToggleButtonValue( VehicleEditorToolbar-->ghostMode, 0 );

    // Initialise hints menu
    VehicleEdHintMenu.clear();
    %count = VehicleHintGroup.getCount();
    for (%i = 0; %i < %count; %i++) {
        %hint = VehicleHintGroup.getObject(%i);
        VehicleEdHintMenu.add(%hint.objectType, %hint);
    }
}

function VehicleEditorPlugin::open(%this, %filename) {
    if ( !%this.isActivated ) {
        // Activate the Shape Editor
       // Lab.setEditor( %this, true );

        // Get editor settings (note the sun angle is not configured in the settings
        // dialog, so apply the settings here instead of in readSettings)
       VehicleEdShapeView.fitIntoParents();
       VehicleEditorPreview-->previewBackground.fitIntoParents();
       
        $wasInWireFrameMode = $gfx::wireframe;
        VehicleEditorToolbar-->wireframeMode.setStateOn($gfx::wireframe);

        if ( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" )
            VehicleEdNodes-->objectTransform.setStateOn(1);
        else
            VehicleEdNodes-->worldTransform.setStateOn(1);

        // Initialise and show the shape editor
      
       
        EditorGui.bringToFront(VehicleEditorPreview);

        EWToolsPaletteArray->WorldEditorMove.performClick();
        %this.map.push();

        // Switch to the VehicleEditor UndoManager
        %this.oldUndoMgr = Editor.getUndoManager();
        Editor.setUndoManager( VehicleEdUndoManager );

        VehicleEdShapeView.setDisplayType( Lab.cameraDisplayType );
        %this.initStatusBar();

        // Customise menu bar
        %this.oldCamFitCmd = %this.replaceMenuCmd( "Camera", 8, "VehicleEdShapeView.fitToShape();" );
        %this.oldCamFitOrbitCmd = %this.replaceMenuCmd( "Camera", 9, "VehicleEdShapeView.fitToShape();" );

        Parent::onActivated(%this);
    }

    // Select the new shape
    if (isObject(VehicleEditor.shape) && (VehicleEditor.shape.baseShape $= %filename)) {
        // Shape is already selected => re-highlight the selected material if necessary
        VehicleEdMaterials.updateSelectedMaterial(VehicleEdMaterials-->highlightMaterial.getValue());
    } else if (%filename !$= "") {
        VehicleEditor.selectShape(%filename, VehicleEditor.isDirty());

        // 'fitToShape' only works after the GUI has been rendered, so force a repaint first
        Canvas.repaint();
        VehicleEdShapeView.fitToShape();
    }
}

function VehicleEditorPlugin::onActivated(%this) {
    %this.open("");
    %this.getVehicleDatablocks();
   //Assign the Camera fit to the GuiShapeEdPreview 
   Lab.fitCameraGui = VehicleEdShapeView;
    // Try to start with the shape selected in the world editor
    %count = EWorldEditor.getSelectionSize();
    for (%i = 0; %i < %count; %i++) {
        %obj = EWorldEditor.getSelectedObject(%i);
        %shapeFile = VehicleEditor.getObjectShapeFile(%obj);
        if (%shapeFile !$= "") {
            if (!isObject(VehicleEditor.shape) || (VehicleEditor.shape.baseShape !$= %shapeFile)) {
                // Call the 'onSelect' method directly if the object is not in the
                // MissionGroup tree (such as a Player or Projectile object).
                VehicleEdShapeTreeView.clearSelection();
                if (!VehicleEdShapeTreeView.selectItem(%obj))
                    VehicleEdShapeTreeView.onSelect(%obj);

                // 'fitToShape' only works after the GUI has been rendered, so force a repaint first
                Canvas.repaint();
                VehicleEdShapeView.fitToShape();
            }
            break;
        }
    }
}

function VehicleEditorPlugin::initStatusBar(%this) {
    EditorGuiStatusBar.setInfo("Shape editor ( Shift Click ) to speed up camera.");
    EditorGuiStatusBar.setSelection( VehicleEditor.shape.baseShape );
}

function VehicleEditorPlugin::onDeactivated(%this) {
    
    // Notify game objects if shape has been modified
    if ( VehicleEditor.isDirty() )
        VehicleEditor.shape.notifyShapeChanged();

   delObj(VehicleEdObject);
    $gfx::wireframe = $wasInWireFrameMode;

    VehicleEdMaterials.updateSelectedMaterial(false);   
   

    if( EditorGui-->MatEdPropertiesWindow.visible ) {
        VehicleEdMaterials.editSelectedMaterialEnd( true );
    }

    %this.map.pop();

    // Restore the original undo manager
    Editor.setUndoManager( %this.oldUndoMgr );

    // Restore menu bar
    %this.replaceMenuCmd( "Camera", 8, %this.oldCamFitCmd );
    %this.replaceMenuCmd( "Camera", 9, %this.oldCamFitOrbitCmd );

    Parent::onDeactivated(%this);
}

function VehicleEditorPlugin::onExitMission( %this ) {
    // unselect the current shape
    VehicleEdShapeView.setModel( "" );
    if (VehicleEditor.shape != -1)
        delObj(VehicleEditor.shape);
    VehicleEditor.shape = 0;
    VehicleEdUndoManager.clearAll();
    VehicleEditor.setDirty( false );

    VehicleEdSequenceList.clear();
    VehicleEdNodeTreeView.removeItem( 0 );
    VehicleEdPropWindow.update_onNodeSelectionChanged( -1 );
    VehicleEdDetailTree.removeItem( 0 );
    VehicleEdMaterialList.clear();

    VehicleEdMountWindow-->mountList.clear();
    VehicleEdThreadWindow-->seqList.clear();
    VehicleEdThreadList.clear();
}

function VehicleEditorPlugin::openShape( %this, %path, %discardChangesToCurrent ) {
//    Lab.setEditor( VehicleEditorPlugin );

    if( VehicleEditor.isDirty() && !%discardChangesToCurrent ) {
        LabMsgYesNo( "Save Changes?",
                          "Save changes to current shape?",
                          "VehicleEditor.saveChanges(); VehicleEditorPlugin.openShape(\"" @ %path @ "\");",
                          "VehicleEditorPlugin.openShape(\"" @ %path @ "\");" );
        return;
    }

    VehicleEditor.selectShape( %path );
    VehicleEdShapeView.fitToShape();
}
function VehicleEditorPlugin::onPreSave( %this ) {
  VehicleEdShapeView.selectedNode = "-1";
   VehicleEdShapeView.selectedObject = "-1";
   VehicleEdShapeView.selectedObjDetail = "-1";
    VehicleEdShapeView.activeThread = "-1";
   
}

