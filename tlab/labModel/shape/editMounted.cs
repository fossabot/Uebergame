//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//------------------------------------------------------------------------------
// Mounted Shapes
//------------------------------------------------------------------------------

function LabModelMountWindow::onWake( %this ) {
	%this-->mountType.clear();
	%this-->mountType.add( "Object", 0 );
	%this-->mountType.add( "Image", 1 );
	%this-->mountType.add( "Wheel", 2 );
	%this-->mountType.setSelected( 1, false );
	%this-->mountSeq.clear();
	%this-->mountSeq.add( "<rootpose>", 0 );
	%this-->mountSeq.setSelected( 0, false );
	%this-->mountPlayBtn.setStateOn( false );

	// Only add the Browse entry the first time so we keep any files the user has
	// set up previously
	if ( LabModelMountShapeMenu.size() == 0 ) {
		LabModelMountShapeMenu.add( "Browse...", 0 );
		LabModelMountShapeMenu.setSelected( 0, false );
	}
}

function LabModelMountWindow::isMountableNode( %this, %nodeName ) {
	return ( startswith( %nodeName, "mount" ) || startswith( %nodeName, "hub" ) );
}

function LabModelMountWindow::update_onShapeSelectionChanged( %this ) {
	%this.unmountAll();
	// Initialise the dropdown menus
	%this-->mountNode.clear();
	%this-->mountNode.add( "<origin>" );
	%count = LabModel.shape.getNodeCount();

	for ( %i = 0; %i < %count; %i++ ) {
		%name = LabModel.shape.getNodeName( %i );

		if ( %this.isMountableNode( %name ) )
			%this-->mountNode.add( %name );
	}

	%this-->mountNode.sort();
	%this-->mountNode.setFirstSelected();
	%this-->mountSeq.clear();
	%this-->mountSeq.add( "<rootpose>", 0 );
	%this-->mountSeq.setSelected( 0, false );
}

function LabModelMountWindow::update_onMountSelectionChanged( %this ) {
	%row = %this-->mountList.getSelectedRow();

	if ( %row > 0 ) {
		%text = %this-->mountList.getRowText( %row );
		%shapePath = getField( %text, 0 );
		LabModelMountShapeMenu.setText( %shapePath );
		%this-->mountNode.setText( getField( %text, 2 ) );
		%this-->mountType.setText( getField( %text, 3 ) );
		// Fill in sequence list
		%this-->mountSeq.clear();
		%this-->mountSeq.add( "<rootpose>", 0 );
		%tss = LabModel.findConstructor( %shapePath );

		if ( !isObject( %tss ) )
			%tss = LabModel.createConstructor( %shapePath );

		if ( isObject( %tss ) ) {
			%count = %tss.getSequenceCount();

			for ( %i = 0; %i < %count; %i++ )
				%this-->mountSeq.add( %tss.getSequenceName( %i ) );
		}

		// Select the currently playing sequence
		%slot = %row - 1;
		%seq = LabModelPreview.getMountThreadSequence( %slot );
		%id = %this-->mountSeq.findText( %seq );

		if ( %id == -1 )
			%id = 0;

		%this-->mountSeq.setSelected( %id, false );
		LabModelMountSeqSlider.setValue( LabModelPreview.getMountThreadPos( %slot ) );
		%this-->mountPlayBtn.setStateOn( LabModelPreview.getMountThreadPos( %slot ) != 0 );
	}
}

function LabModelMountWindow::updateSelectedMount( %this ) {
	%row = %this-->mountList.getSelectedRow();

	if ( %row > 0 )
		%this.mountShape( %row-1 );
}

function LabModelMountWindow::setMountThreadSequence( %this ) {
	%row = %this-->mountList.getSelectedRow();

	if ( %row > 0 ) {
		LabModelPreview.setMountThreadSequence( %row-1, %this-->mountSeq.getText() );
		LabModelPreview.setMountThreadDir( %row-1, %this-->mountPlayBtn.getValue() );
	}
}

function LabModelMountSeqSlider::onMouseDragged( %this ) {
	%row = LabModelMountWindow-->mountList.getSelectedRow();

	if ( %row > 0 ) {
		LabModelPreview.setMountThreadPos( %row-1, %this.getValue() );
		// Pause the sequence when the slider is dragged
		LabModelPreview.setMountThreadDir( %row-1, 0 );
		LabModelMountWindow-->mountPlayBtn.setStateOn( false );
	}
}

function LabModelMountWindow::toggleMountThreadPlayback( %this ) {
	%row = %this-->mountList.getSelectedRow();

	if ( %row > 0 )
		LabModelPreview.setMountThreadDir( %row-1, %this-->mountPlayBtn.getValue() );
}

function LabModelMountShapeMenu::onSelect( %this, %id, %text ) {
	if ( %text $= "Browse..." ) {
		// Allow the user to browse for an external model file
		getLoadFilename( "DTS Files|*.dts|COLLADA Files|*.dae|Google Earth Files|*.kmz", %this @ ".onBrowseSelect", %this.lastPath );
	} else {
		// Modify the current mount
		LabModelMountWindow.updateSelectedMount();
	}
}

function LabModelMountShapeMenu::onBrowseSelect( %this, %path ) {
	%path = makeRelativePath( %path, getMainDotCSDir() );
	%this.lastPath = %path;
	%this.setText( %path );

	// Add entry if unique
	if ( %this.findText( %path ) == -1 )
		%this.add( %path );

	LabModelMountWindow.updateSelectedMount();
}

function LabModelMountWindow::mountShape( %this, %slot ) {
	%model = LabModelMountShapeMenu.getText();
	%node = %this-->mountNode.getText();
	%type = %this-->mountType.getText();

	if ( %model $= "Browse..." )
		%model = "core/art/shapes/octahedron.dts";

	if ( LabModelPreview.mountShape( %model, %node, %type, %slot ) ) {
		%rowText = %model TAB fileName( %model ) TAB %node TAB %type;

		if ( %slot == -1 ) {
			%id = %this.mounts++;
			%this-->mountList.addRow( %id, %rowText );
		} else {
			%id = %this-->mountList.getRowId( %slot+1 );
			%this-->mountList.setRowById( %id, %rowText );
		}

		%this-->mountList.setSelectedById( %id );
	} else {
		LabMsgOK( "Error", "Failed to mount \"" @ %model @ "\". Check the console for error messages.", "" );
	}
}

function LabModelMountWindow::unmountShape( %this ) {
	%row = %this-->mountList.getSelectedRow();

	if ( %row > 0 ) {
		LabModelPreview.unmountShape( %row-1 );
		%this-->mountList.removeRow( %row );
		// Select the next row (if any)
		%count = %this-->mountList.rowCount();

		if ( %row >= %count )
			%row = %count-1;

		if ( %row > 0 )
			%this-->mountList.setSelectedRow( %row );
	}
}

function LabModelMountWindow::unmountAll( %this ) {
	LabModelPreview.unmountAll();
	%this-->mountList.clear();
	%this-->mountList.addRow( -1, "FullPath" TAB "Filename" TAB "Node" TAB "Type" );
	%this-->mountList.setRowActive( -1, false );
}