//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================



// Find all DTS or COLLADA models. Note: most of this section was shamelessly
// stolen from creater.ed.cs => great work whoever did the original!
function LabModelSelectWindow::navigate( %this, %address ) {
	// Freeze the icon array so it doesn't update until we've added all of the
	// icons
	%this-->shapeLibrary.frozen = true;
	%this-->shapeLibrary.clear();
	LabModelSelectMenu.clear();
	%filePatterns = "*.dts" TAB "*.dae" TAB "*.kmz";
	%fullPath = findFirstFileMultiExpr( %filePatterns );

	while ( %fullPath !$= "" ) {
		// Ignore cached DTS files
		if ( endswith( %fullPath, "cached.dts" ) ) {
			%fullPath = findNextFileMultiExpr( %filePatterns );
			continue;
		}

		// Ignore assets in the tools folder
		%fullPath = makeRelativePath( %fullPath, getMainDotCSDir() );
		%splitPath = strreplace( %fullPath, "/", " " );

		if ( getWord( %splitPath, 0 ) $= "tools" ) {
			%fullPath = findNextFileMultiExpr( %filePatterns );
			continue;
		}

		%dirCount = getWordCount( %splitPath ) - 1;
		%pathFolders = getWords( %splitPath, 0, %dirCount - 1 );
		// Add this file's path ( parent folders ) to the
		// popup menu if it isn't there yet.
		%temp = strreplace( %pathFolders, " ", "/" );
		%r = LabModelSelectMenu.findText( %temp );

		if ( %r == -1 )
			LabModelSelectMenu.add( %temp );

		// Is this file in the current folder?
		if ( stricmp( %pathFolders, %address ) == 0 ) {
			%this.addShapeIcon( %fullPath );
		}
		// Then is this file in a subfolder we need to add
		// a folder icon for?
		else {
			%wordIdx = 0;
			%add = false;

			if ( %address $= "" ) {
				%add = true;
				%wordIdx = 0;
			} else {
				for ( ; %wordIdx < %dirCount; %wordIdx++ ) {
					%temp = getWords( %splitPath, 0, %wordIdx );

					if ( stricmp( %temp, %address ) == 0 ) {
						%add = true;
						%wordIdx++;
						break;
					}
				}
			}

			if ( %add == true ) {
				%folder = getWord( %splitPath, %wordIdx );
				// Add folder icon if not already present
				%ctrl = %this.findIconCtrl( %folder );

				if ( %ctrl == -1 )
					%this.addFolderIcon( %folder );
			}
		}

		%fullPath = findNextFileMultiExpr( %filePatterns );
	}

	%this-->shapeLibrary.sort( "alphaIconCompare" );

	for ( %i = 0; %i < %this-->shapeLibrary.getCount(); %i++ )
		%this-->shapeLibrary.getObject( %i ).autoSize = false;

	%this-->shapeLibrary.frozen = false;
	%this-->shapeLibrary.refresh();
	%this.address = %address;
	LabModelSelectMenu.sort();
	%str = strreplace( %address, " ", "/" );
	%r = LabModelSelectMenu.findText( %str );

	if ( %r != -1 )
		LabModelSelectMenu.setSelected( %r, false );
	else
		LabModelSelectMenu.setText( %str );
}

function LabModelSelectWindow::navigateDown( %this, %folder ) {
	if ( %this.address $= "" )
		%address = %folder;
	else
		%address = %this.address SPC %folder;

	// Because this is called from an IconButton::onClick command
	// we have to wait a tick before actually calling navigate, else
	// we would delete the button out from under itself.
	%this.schedule( 1, "navigate", %address );
}

function LabModelSelectWindow::navigateUp( %this ) {
	%count = getWordCount( %this.address );

	if ( %count == 0 )
		return;

	if ( %count == 1 )
		%address = "";
	else
		%address = getWords( %this.address, 0, %count - 2 );

	%this.navigate( %address );
}

function LabModelSelectWindow::findIconCtrl( %this, %name ) {
	for ( %i = 0; %i < %this-->shapeLibrary.getCount(); %i++ ) {
		%ctrl = %this-->shapeLibrary.getObject( %i );

		if ( %ctrl.text $= %name )
			return %ctrl;
	}

	return -1;
}

function LabModelSelectWindow::createIcon( %this ) {
	%ctrl = new GuiIconButtonCtrl() {
		profile = "ToolsButtonArray";
		iconLocation = "Left";
		textLocation = "Right";
		extent = "348 19";
		textMargin = 8;
		buttonMargin = "2 2";
		autoSize = false;
		sizeIconToButton = true;
		makeIconSquare = true;
		buttonType = "radioButton";
		groupNum = "-1";
	};
	return %ctrl;
}

function LabModelSelectWindow::addFolderIcon( %this, %text ) {
	%ctrl = %this.createIcon();
	%ctrl.altCommand = "LabModelSelectWindow.navigateDown( \"" @ %text @ "\" );";
	%ctrl.iconBitmap = "tlab/gui/icons/default/folder.png";
	%ctrl.text = %text;
	%ctrl.tooltip = %text;
	%ctrl.class = "CreatorFolderIconBtn";
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	%this-->shapeLibrary.addGuiControl( %ctrl );
}

function LabModelSelectWindow::addShapeIcon( %this, %fullPath ) {
	%ctrl = %this.createIcon();
	%ext = fileExt( %fullPath );
	%file = fileBase( %fullPath );
	%fileLong = %file @ %ext;
	%tip = %fileLong NL
			 "Size: " @ fileSize( %fullPath ) / 1000.0 SPC "KB" NL
			 "Date Created: " @ fileCreatedTime( %fullPath ) NL
			 "Last Modified: " @ fileModifiedTime( %fullPath );
	%ctrl.altCommand = "LabModelSelectWindow.onSelect( \"" @ %fullPath @ "\" );";
	%ctrl.iconBitmap = ( ( %ext $= ".dts" ) ? EditorIconRegistry::findIconByClassName( "TSStatic" ) : "tlab/gui/icons/default/iconCollada" );
	%ctrl.text = %file;
	%ctrl.class = "CreatorStaticIconBtn";
	%ctrl.tooltip = %tip;
	%ctrl.buttonType = "radioButton";
	%ctrl.groupNum = "-1";
	// Check if a shape specific icon is available
	%formats = ".png .jpg .dds .bmp .gif .jng .tga";
	%count = getWordCount( %formats );

	for ( %i = 0; %i < %count; %i++ ) {
		%ext = getWord( %formats, %i );

		if ( isFile( %fullPath @ %ext ) ) {
			%ctrl.iconBitmap = %fullPath @ %ext;
			break;
		}
	}

	%this-->shapeLibrary.addGuiControl( %ctrl );
}