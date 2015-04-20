//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
// Detail/Mesh Editing
//------------------------------------------------------------------------------

function VehicleEdDetails::onWake( %this ) {
    // Initialise popup menus
    %this-->bbType.clear();
    %this-->bbType.add( "None", 0 );
    %this-->bbType.add( "Billboard", 1 );
    %this-->bbType.add( "Z Billboard", 2 );

    %this-->addGeomTo.clear();
    %this-->addGeomTo.add( "current detail", 0 );
    %this-->addGeomTo.add( "new detail", 1 );
    %this-->addGeomTo.setSelected( 0, false );

    VehicleEdDetailTree.onDefineIcons();
}

function VehicleEdDetailTree::onDefineIcons(%this) {
    // Set the tree view icon indices and texture paths
    %this._imageNone = 0;
    %this._imageHidden = 1;

    %icons = ":" @                                        // no icon
             "tlab/gui/icons/default/visible_i:";               // hidden

    %this.buildIconTable( %icons );
}

// Return true if the item in the details tree view is a detail level (false if
// a mesh)
function VehicleEdDetailTree::isDetailItem( %this, %id ) {
    return ( %this.getParent( %id ) == 1 );
}

// Get the detail level index from the ID of an item in the details tree view
function VehicleEdDetailTree::getDetailLevelFromItem( %this, %id ) {
    if ( %this.isDetailItem( %id ) )
        %detSize = %this.getItemValue( %id );

    else
        %detSize = %this.getItemValue( %this.getParent( %id ) );
    return VehicleEditor.shape.getDetailLevelIndex( %detSize );
}

function VehicleEdDetailTree::addMeshEntry( %this, %name, %noSync ) {
    // Add new detail level if required
    %size = getTrailingNumber( %name );
    %detailID = %this.findItemByValue( %size );
    if ( %detailID <= 0 ) {
        %dl = VehicleEditor.shape.getDetailLevelIndex( %size );
        %detName = VehicleEditor.shape.getDetailLevelName( %dl );
        %detailID = VehicleEdDetailTree.insertItem( 1, %detName, %size, "" );

        // Sort details by decreasing size
        for ( %sibling = VehicleEdDetailTree.getPrevSibling( %detailID );
                ( %sibling > 0 ) && ( VehicleEdDetailTree.getItemValue( %sibling ) < %size );
                %sibling = VehicleEdDetailTree.getPrevSibling( %detailID ) )
            VehicleEdDetailTree.moveItemUp( %detailID );

        if ( !%noSync )
            VehicleEdDetails.update_onDetailsChanged();
    }
    return %this.insertItem( %detailID, %name, "", "" );
}

function VehicleEdDetailTree::removeMeshEntry( %this, %name, %size ) {
    %size = getTrailingNumber( %name );
    %id = VehicleEdDetailTree.findItemByName( %name );
    if ( VehicleEditor.shape.getDetailLevelIndex( %size ) < 0 ) {
        // Last mesh of a detail level has been removed => remove the detail level
        %this.removeItem( %this.getParent( %id ) );
        VehicleEdDetails.update_onDetailsChanged();
    } else
        %this.removeItem( %id );
}

function VehicleEdAdvancedWindow::update_onShapeSelectionChanged( %this ) {
    VehicleEdShapeView.currentDL = 0;
    VehicleEdShapeView.onDetailChanged();
}

function VehicleEdPropWindow::update_onDetailRenamed( %this, %oldName, %newName ) {
    // --- DETAILS TAB ---
    // Rename detail entry
    %id = VehicleEdDetailTree.findItemByName( %oldName );
    if ( %id > 0 ) {
        %size = VehicleEdDetailTree.getItemValue( %id );
        VehicleEdDetailTree.editItem( %id, %newName, %size );

        // Sync text if item is selected
        if ( VehicleEdDetailTree.isItemSelected( %id ) &&
                ( VehicleEdDetails-->meshName.getText() !$= %newName ) )
            VehicleEdDetails-->meshName.setText( stripTrailingNumber( %newName ) );
    }
}

function VehicleEdPropWindow::update_onDetailSizeChanged( %this, %oldSize, %newSize ) {
    // --- MISC ---
    VehicleEdShapeView.refreshShape();
    %dl = VehicleEditor.shape.getDetailLevelIndex( %newSize );
    if ( VehicleEdAdvancedWindow-->detailSize.getText() $= %oldSize ) {
        VehicleEdShapeView.currentDL = %dl;
        VehicleEdAdvancedWindow-->detailSize.setText( %newSize );
        VehicleEdDetails-->meshSize.setText( %newSize );
    }

    // --- DETAILS TAB ---
    // Update detail entry then resort details by size
    %id = VehicleEdDetailTree.findItemByValue( %oldSize );
    %detName = VehicleEditor.shape.getDetailLevelName( %dl );
    VehicleEdDetailTree.editItem( %id, %detName, %newSize );

    for ( %sibling = VehicleEdDetailTree.getPrevSibling( %id );
            ( %sibling > 0 ) && ( VehicleEdDetailTree.getItemValue( %sibling ) < %newSize );
            %sibling = VehicleEdDetailTree.getPrevSibling( %id ) )
        VehicleEdDetailTree.moveItemUp( %id );
    for ( %sibling = VehicleEdDetailTree.getNextSibling( %id );
            ( %sibling > 0 ) && ( VehicleEdDetailTree.getItemValue( %sibling ) > %newSize );
            %sibling = VehicleEdDetailTree.getNextSibling( %id ) )
        VehicleEdDetailTree.moveItemDown( %id );

    // Update size values for meshes of this detail
    for ( %child = VehicleEdDetailTree.getChild( %id );
            %child > 0;
            %child = VehicleEdDetailTree.getNextSibling( %child ) ) {
        %meshName = stripTrailingNumber( VehicleEdDetailTree.getItemText( %child ) );
        VehicleEdDetailTree.editItem( %child, %meshName SPC %newSize, "" );
    }
}

function VehicleEdDetails::update_onDetailsChanged( %this ) {
    %detailCount = VehicleEditor.shape.getDetailLevelCount();
    VehicleEdAdvancedWindow-->detailSlider.range = "0" SPC ( %detailCount-1 );
    if ( %detailCount >= 2 )
        VehicleEdAdvancedWindow-->detailSlider.ticks = %detailCount - 2;
    else
        VehicleEdAdvancedWindow-->detailSlider.ticks = 0;

    // Initialise imposter settings
    VehicleEdAdvancedWindow-->bbUseImposters.setValue( VehicleEditor.shape.getImposterDetailLevel() != -1 );

    // Update detail parameters
    if ( VehicleEdShapeView.currentDL < %detailCount ) {
        %settings = VehicleEditor.shape.getImposterSettings( VehicleEdShapeView.currentDL );
        %isImposter = getWord( %settings, 0 );

        VehicleEdAdvancedWindow-->imposterInactive.setVisible( !%isImposter );

        VehicleEdAdvancedWindow-->bbEquatorSteps.setText( getField( %settings, 1 ) );
        VehicleEdAdvancedWindow-->bbPolarSteps.setText( getField( %settings, 2 ) );
        VehicleEdAdvancedWindow-->bbDetailLevel.setText( getField( %settings, 3 ) );
        VehicleEdAdvancedWindow-->bbDimension.setText( getField( %settings, 4 ) );
        VehicleEdAdvancedWindow-->bbIncludePoles.setValue( getField( %settings, 5 ) );
        VehicleEdAdvancedWindow-->bbPolarAngle.setText( getField( %settings, 6 ) );
    }
}

function VehicleEdPropWindow::update_onObjectNodeChanged( %this, %objName ) {
    // --- MISC ---
    VehicleEdShapeView.refreshShape();

    // --- DETAILS TAB ---
    // Update the node popup menu if this object is selected
    if ( VehicleEdDetails-->meshName.getText() $= %objName ) {
        %nodeName = VehicleEditor.shape.getObjectNode( %objName );
        if ( %nodeName $= "" )
            %nodeName = "<root>";
        %id = VehicleEdDetails-->objectNode.findText( %nodeName );
        VehicleEdDetails-->objectNode.setSelected( %id, false );
    }
}

function VehicleEdPropWindow::update_onObjectRenamed( %this, %oldName, %newName ) {
    // --- DETAILS TAB ---
    // Rename tree entries for this object
    %count = VehicleEditor.shape.getMeshCount( %newName );
    for ( %i = 0; %i < %count; %i++ ) {
        %size = getTrailingNumber( VehicleEditor.shape.getMeshName( %newName, %i ) );
        %id = VehicleEdDetailTree.findItemByName( %oldName SPC %size );
        if ( %id > 0 ) {
            VehicleEdDetailTree.editItem( %id, %newName SPC %size, "" );

            // Sync text if item is selected
            if ( VehicleEdDetailTree.isItemSelected( %id ) &&
                    ( VehicleEdDetails-->meshName.getText() !$= %newName ) )
                VehicleEdDetails-->meshName.setText( %newName );
        }
    }
}

function VehicleEdPropWindow::update_onMeshAdded( %this, %meshName ) {
    // --- MISC ---
    VehicleEdShapeView.refreshShape();
    VehicleEdShapeView.updateNodeTransforms();

    // --- COLLISION WINDOW ---
    // Add object to target list if it does not already exist
    if ( !VehicleEditor.isCollisionMesh( %meshName ) ) {
        %objName = stripTrailingNumber( %meshName );
        %id = VehicleEdColWindow-->colTarget.findText( %objName );
        if ( %id == -1 )
            VehicleEdColWindow-->colTarget.add( %objName );
    }

    // --- DETAILS TAB ---
    %id = VehicleEdDetailTree.addMeshEntry( %meshName );
    VehicleEdDetailTree.clearSelection();
    VehicleEdDetailTree.selectItem( %id );
}

function VehicleEdPropWindow::update_onMeshSizeChanged( %this, %meshName, %oldSize, %newSize ) {
    // --- MISC ---
    VehicleEdShapeView.refreshShape();

    // --- DETAILS TAB ---
    // Move the mesh to the new location in the tree
    %selected = VehicleEdDetailTree.getSelectedItem();
    %id = VehicleEdDetailTree.findItemByName( %meshName SPC %oldSize );
    VehicleEdDetailTree.removeMeshEntry( %meshName SPC %oldSize );
    %newId = VehicleEdDetailTree.addMeshEntry( %meshName SPC %newSize );

    // Re-select the new entry if it was selected
    if ( %selected == %id ) {
        VehicleEdDetailTree.clearSelection();
        VehicleEdDetailTree.selectItem( %newId );
    }
}

function VehicleEdPropWindow::update_onMeshRemoved( %this, %meshName ) {
    // --- MISC ---
    VehicleEdShapeView.refreshShape();

    // --- COLLISION WINDOW ---
    // Remove object from target list if it no longer exists
    %objName = stripTrailingNumber( %meshName );
    if ( VehicleEditor.shape.getObjectIndex( %objName ) == -1 ) {
        %id = VehicleEdColWindow-->colTarget.findText( %objName );
        if ( %id != -1 )
            VehicleEdColWindow-->colTarget.clearEntry( %id );
    }

    // --- DETAILS TAB ---
    // Determine which item to select next
    %id = VehicleEdDetailTree.findItemByName( %meshName );
    if ( %id > 0 ) {
        %nextId = VehicleEdDetailTree.getPrevSibling( %id );
        if ( %nextId <= 0 ) {
            %nextId = VehicleEdDetailTree.getNextSibling( %id );
            if ( %nextId <= 0 )
                %nextId = 2;
        }

        // Remove the entry from the tree
        %meshSize = getTrailingNumber( %meshName );
        VehicleEdDetailTree.removeMeshEntry( %meshName, %meshSize );

        // Change selection if needed
        if ( VehicleEdDetailTree.getSelectedItem() == -1 )
            VehicleEdDetailTree.selectItem( %nextId );
    }
}

function VehicleEdDetailTree::onSelect( %this, %id ) {
    %name = %this.getItemText( %id );
    %baseName = stripTrailingNumber( %name );
    %size = getTrailingNumber( %name );

    VehicleEdDetails-->meshName.setText( %baseName );
    VehicleEdDetails-->meshSize.setText( %size );

    // Select the appropriate detail level
    %dl = %this.getDetailLevelFromItem( %id );
    VehicleEdShapeView.currentDL = %dl;

    if ( %this.isDetailItem( %id ) ) {
        // Selected a detail => disable mesh controls
        VehicleEdDetails-->editMeshInactive.setVisible( true );
        VehicleEdShapeView.selectedObject = -1;
        VehicleEdShapeView.selectedObjDetail = 0;
    } else {
        // Selected a mesh => sync mesh controls
        VehicleEdDetails-->editMeshInactive.setVisible( false );

        switch$ ( VehicleEditor.shape.getMeshType( %name ) ) {
        case "normal":
            VehicleEdDetails-->bbType.setSelected( 0, false );
        case "billboard":
            VehicleEdDetails-->bbType.setSelected( 1, false );
        case "billboardzaxis":
            VehicleEdDetails-->bbType.setSelected( 2, false );
        }

        %node = VehicleEditor.shape.getObjectNode( %baseName );
        if ( %node $= "" )
            %node = "<root>";
        VehicleEdDetails-->objectNode.setSelected( VehicleEdDetails-->objectNode.findText( %node ), false );
        VehicleEdShapeView.selectedObject = VehicleEditor.shape.getObjectIndex( %baseName );
        VehicleEdShapeView.selectedObjDetail = %dl;
    }
}

function VehicleEdDetailTree::onRightMouseUp( %this, %itemId, %mouse ) {
    // Open context menu if this is a Mesh item
    if ( !%this.isDetailItem( %itemId ) ) {
        if( !isObject( "VehicleEdMeshPopup" ) ) {
            new PopupMenu( VehicleEdMeshPopup ) {
                superClass = "MenuBuilder";
                isPopup = "1";

                item[ 0 ] = "Hidden" TAB "" TAB "VehicleEdDetailTree.onHideMeshItem( %this._objName, !%this._itemHidden );";
                item[ 1 ] = "-";
                item[ 2 ] = "Hide all" TAB "" TAB "VehicleEdDetailTree.onHideMeshItem( \"\", true );";
                item[ 3 ] = "Show all" TAB "" TAB "VehicleEdDetailTree.onHideMeshItem( \"\", false );";
            };
        }

        VehicleEdMeshPopup._objName = stripTrailingNumber( %this.getItemText( %itemId ) );
        VehicleEdMeshPopup._itemHidden = VehicleEdShapeView.getMeshHidden( VehicleEdMeshPopup._objName );

        VehicleEdMeshPopup.checkItem( 0, VehicleEdMeshPopup._itemHidden );
        VehicleEdMeshPopup.showPopup( Canvas );
    }
}

function VehicleEdDetailTree::onHideMeshItem( %this, %objName, %hide ) {
    if ( %hide )
        %imageId = %this._imageHidden;
    else
        %imageId = %this._imageNone;

    if ( %objName $= "" ) {
        // Show/hide all
        VehicleEdShapeView.setAllMeshesHidden( %hide );
        for ( %parent = %this.getChild(%this.getFirstRootItem()); %parent > 0; %parent = %this.getNextSibling(%parent) )
            for ( %child = %this.getChild(%parent); %child > 0; %child = %this.getNextSibling(%child) )
                %this.setItemImages( %child, %imageId, %imageId );
    } else {
        // Show/hide all meshes for this object
        VehicleEdShapeView.setMeshHidden( %objName, %hide );
        %count = VehicleEditor.shape.getMeshCount( %objName );
        for ( %i = 0; %i < %count; %i++ ) {
            %meshName = VehicleEditor.shape.getMeshName( %objName, %i );
            %id = VehicleEdDetailTree.findItemByName( %meshName );
            if ( %id > 0 )
                %this.setItemImages( %id, %imageId, %imageId );
        }
    }
}

function VehicleEdShapeView::onDetailChanged( %this ) {
    // Update slider
    if ( mRound( VehicleEdAdvancedWindow-->detailSlider.getValue() ) != %this.currentDL )
        VehicleEdAdvancedWindow-->detailSlider.setValue( %this.currentDL );
    VehicleEdAdvancedWindow-->detailSize.setText( %this.detailSize );

    VehicleEdDetails.update_onDetailsChanged();

    %id = VehicleEdDetailTree.getSelectedItem();
    if ( ( %id <= 0 ) || ( %this.currentDL != VehicleEdDetailTree.getDetailLevelFromItem( %id ) ) ) {
        %id = VehicleEdDetailTree.findItemByValue( %this.detailSize );
        if ( %id > 0 ) {
            VehicleEdDetailTree.clearSelection();
            VehicleEdDetailTree.selectItem( %id );
        }
    }
}

function VehicleEdAdvancedWindow::onEditDetailSize( %this ) {
    // Change the size of the current detail level
    %oldSize = VehicleEditor.shape.getDetailLevelSize( VehicleEdShapeView.currentDL );
    %detailSize = %this-->detailSize.getText();
    VehicleEditor.doEditDetailSize( %oldSize, %detailSize );
}

function VehicleEdDetails::onEditName( %this ) {
    %newName = %this-->meshName.getText();

    // Check if we are renaming a detail or a mesh
    %id = VehicleEdDetailTree.getSelectedItem();
    %oldName = VehicleEdDetailTree.getItemText( %id );

    if ( VehicleEdDetailTree.isDetailItem( %id ) ) {
        // Rename the selected detail level
        %oldSize = getTrailingNumber( %oldName );
        VehicleEditor.doRenameDetail( %oldName, %newName @ %oldSize );
    } else {
        // Rename the selected mesh
        VehicleEditor.doRenameObject( stripTrailingNumber( %oldName ), %newName );
    }
}

function VehicleEdDetails::onEditSize( %this ) {
    %newSize = %this-->meshSize.getText();

    // Check if we are changing the size for a detail or a mesh
    %id = VehicleEdDetailTree.getSelectedItem();
    if ( VehicleEdDetailTree.isDetailItem( %id ) ) {
        // Change the size of the selected detail level
        %oldSize = VehicleEdDetailTree.getItemValue( %id );
        VehicleEditor.doEditDetailSize( %oldSize, %newSize );
    } else {
        // Change the size of the selected mesh
        %meshName = VehicleEdDetailTree.getItemText( %id );
        VehicleEditor.doEditMeshSize( %meshName, %newSize );
    }
}

function VehicleEdDetails::onEditBBType( %this ) {
    // This command is only valid for meshes (not details)
    %id = VehicleEdDetailTree.getSelectedItem();
    if ( !VehicleEdDetailTree.isDetailItem( %id ) ) {
        %meshName = VehicleEdDetailTree.getItemText( %id );
        %bbType = VehicleEdDetails-->bbType.getText();
        switch$ ( %bbType ) {
        case "None":
            %bbType = "normal";
        case "Billboard":
            %bbType = "billboard";
        case "Z Billboard":
            %bbType = "billboardzaxis";
        }
        VehicleEditor.doEditMeshBillboard( %meshName, %bbType );
    }
}

function VehicleEdDetails::onSetObjectNode( %this ) {
    // This command is only valid for meshes (not details)
    %id = VehicleEdDetailTree.getSelectedItem();
    if ( !VehicleEdDetailTree.isDetailItem( %id ) ) {
        %meshName = VehicleEdDetailTree.getItemText( %id );
        %objName = stripTrailingNumber( %meshName );
        %node = %this-->objectNode.getText();
        if ( %node $= "<root>" )
            %node = "";
        VehicleEditor.doSetObjectNode( %objName, %node );
    }
}

function VehicleEdDetails::onAddMeshFromFile( %this, %path ) {
    if ( %path $= "" ) {
        getLoadFilename( "DTS Files|*.dts|COLLADA Files|*.dae|Google Earth Files|*.kmz", %this @ ".onAddMeshFromFile", %this.lastPath );
        return;
    }

    %path = makeRelativePath( %path, getMainDotCSDir() );
    %this.lastPath = %path;

    // Determine the detail level to use for the new geometry
    if ( %this-->addGeomTo.getText() $= "current detail" ) {
        %size = VehicleEditor.shape.getDetailLevelSize( VehicleEdShapeView.currentDL );
    } else {
        // Check if the file has an LODXXX hint at the end of it
        %base = fileBase( %path );
        %pos = strstr( %base, "_LOD" );
        if ( %pos > 0 )
            %size = getSubStr( %base, %pos + 4, strlen( %base ) ) + 0;
        else
            %size = 2;

        // Make sure size is not in use
        while ( VehicleEditor.shape.getDetailLevelIndex( %size ) != -1 )
            %size++;
    }

    VehicleEditor.doAddMeshFromFile( %path, %size );
}

function VehicleEdDetails::onDeleteMesh( %this ) {
    %id = VehicleEdDetailTree.getSelectedItem();
    if ( VehicleEdDetailTree.isDetailItem( %id ) ) {
        %detSize = VehicleEdDetailTree.getItemValue( %id );
        VehicleEditor.doRemoveShapeData( "Detail", %detSize );
    } else {
        %name = VehicleEdDetailTree.getItemText( %id );
        VehicleEditor.doRemoveShapeData( "Mesh", %name );
    }
}

function VehicleEdDetails::onToggleImposter( %this, %useImposter ) {
    %hasImposterDetail = ( VehicleEditor.shape.getImposterDetailLevel() != -1 );
    if ( %useImposter == %hasImposterDetail )
        return;

    if ( %useImposter ) {
        // Determine an unused detail size
        for ( %detailSize = 0; %detailSize < 50; %detailSize++ ) {
            if ( VehicleEditor.shape.getDetailLevelIndex( %detailSize ) == -1 )
                break;
        }

        // Set some initial values for the imposter
        %bbEquatorSteps = 6;
        %bbPolarSteps = 0;
        %bbDetailLevel = 0;
        %bbDimension = 128;
        %bbIncludePoles = 0;
        %bbPolarAngle = 0;

        // Add a new imposter detail level to the shape
        VehicleEditor.doEditImposter( -1, %detailSize, %bbEquatorSteps, %bbPolarSteps,
                                    %bbDetailLevel, %bbDimension, %bbIncludePoles, %bbPolarAngle );
    } else {
        // Remove the imposter detail level
        VehicleEditor.doRemoveImposter();
    }
}

function VehicleEdDetails::onEditImposter( %this ) {
    // Modify the parameters of the current imposter detail level
    %detailSize = VehicleEditor.shape.getDetailLevelSize( VehicleEdShapeView.currentDL );
    %bbDimension = VehicleEdAdvancedWindow-->bbDimension.getText();
    %bbDetailLevel = VehicleEdAdvancedWindow-->bbDetailLevel.getText();
    %bbEquatorSteps = VehicleEdAdvancedWindow-->bbEquatorSteps.getText();
    %bbIncludePoles = VehicleEdAdvancedWindow-->bbIncludePoles.getValue();
    %bbPolarSteps = VehicleEdAdvancedWindow-->bbPolarSteps.getText();
    %bbPolarAngle = VehicleEdAdvancedWindow-->bbPolarAngle.getText();

    VehicleEditor.doEditImposter( VehicleEdShapeView.currentDL, %detailSize,
                                %bbEquatorSteps, %bbPolarSteps, %bbDetailLevel, %bbDimension,
                                %bbIncludePoles, %bbPolarAngle );
}


function VehicleEditor::autoAddDetails( %this, %dest ) {
    // Sets of LOD files are named like:
    //
    // MyShape_LOD200.dae
    // MyShape_LOD64.dae
    // MyShape_LOD2.dae
    //
    // Determine the base name of the input file (MyShape_LOD in the example above)
    // and use that to find any other shapes in the set.
    %base = fileBase( %dest.baseShape );
    %pos = strstr( %base, "_LOD" );
    if ( %pos < 0 ) {
        echo( "Not an LOD shape file" );
        return;
    }

    %base = getSubStr( %base, 0, %pos + 4 );

    echo( "Base is: " @ %base );

    %filePatterns = filePath( %dest.baseShape ) @ "/" @ %base @ "*" @ fileExt( %dest.baseShape );

    echo( "Pattern is: " @ %filePatterns );

    %fullPath = findFirstFileMultiExpr( %filePatterns );
    while ( %fullPath !$= "" ) {
        %fullPath = makeRelativePath( %fullPath, getMainDotCSDir() );

        if ( %fullPath !$= %dest.baseShape ) {
            echo( "Found LOD shape file: " @ %fullPath );

            // Determine the detail size ( number after the base name ), then add the
            // new mesh
            %size = strreplace( fileBase( %fullPath ), %base, "" );
            VehicleEditor.addLODFromFile( %dest, %fullPath, %size, 0 );
        }

        %fullPath = findNextFileMultiExpr( %filePatterns );
    }

    if ( %this.shape == %dest ) {
        VehicleEdShapeView.refreshShape();
        VehicleEdDetails.update_onDetailsChanged();
    }
}

function VehicleEditor::addLODFromFile( %this, %dest, %filename, %size, %allowUnmatched ) {
    // Get (or create) a TSShapeConstructor object for the source shape. Need to
    // exec the script manually as the resource may not have been loaded yet
    %csPath = filePath( %filename ) @ "/" @ fileBase( %filename ) @ ".cs";
    if ( isFile( %csPath ) )
        exec( %csPath );

    %source = VehicleEditor.findConstructor( %filename );
    if ( %source == -1 )
        %source = VehicleEditor.createConstructor( %filename );
    %source.lodType = "SingleSize";
    %source.singleDetailSize = %size;

    // Create a temporary TSStatic to ensure the resource is loaded
    %temp = new TSStatic() {
        shapeName = %filename;
        collisionType = "None";
    };

    %meshList = "";
    if ( isObject( %temp ) ) {
        // Add a new mesh for each object in the source shape
        %objCount = %source.getObjectCount();
        for ( %i = 0; %i < %objCount; %i++ ) {
            %objName = %source.getObjectName( %i );

            echo( "Checking for object " @ %objName );

            if ( %allowUnmatched || ( %dest.getObjectIndex( %objName ) != -1 ) ) {
                // Add the source object's highest LOD mesh to the destination shape
                echo( "Adding detail size" SPC %size SPC "for object" SPC %objName );
                %srcName = %source.getMeshName( %objName, 0 );
                %destName = %objName SPC %size;
                %dest.addMesh( %destName, %filename, %srcName );
                %meshList = %meshList TAB %destName;
            }
        }

        %temp.delete();
    }

    return trim( %meshList );
}
