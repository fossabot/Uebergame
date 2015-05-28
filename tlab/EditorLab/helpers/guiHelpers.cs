//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


function Lab::setGuisToDefault() {
	setGuiParentLabPhysic(false);
	GuiEditorContentList.init();
}

function Lab::setGuisToEditor() {
	setGuiParentLabPhysic(true);
	GuiEditorContentList.init();
}


function Lab::setGuisToEditor() {
	$pref::GuiEditor::ShowEditorsGui = !$pref::GuiEditor::ShowEditorsGui;
	GuiEditorContentList.init();
}



function Lab::savePluginGui(%this,%plugin) {
	//Assemble the GUI
	%groupCtrl  = %plugin@"_GuiSet";
	foreach(%gui in %groupCtrl) {
		%parent = %gui.defaultParent;
		if (!isObject(%parent)) continue;
		%parent.add(%gui);
	}
	if (!isObject(%parent)) {
		warnLog("Invalid Parent GUI to save:",%parent);
		return;
	}
	%currentFile = %parent.getFileName();
	if( isWriteableFileName( %filename ) ) {
		//
		// Extract any existent TorqueScript before writing out to disk
		//
		%fileObject = new FileObject();
		%fileObject.openForRead( %filename );
		%skipLines = true;
		%beforeObject = true;
		// %var++ does not post-increment %var, in torquescript, it pre-increments it,
		// because ++%var is illegal.
		%lines = -1;
		%beforeLines = -1;
		%skipLines = false;
		while( !%fileObject.isEOF() ) {
			%line = %fileObject.readLine();
			if( %line $= "//--- OBJECT WRITE BEGIN ---" )
				%skipLines = true;
			else if( %line $= "//--- OBJECT WRITE END ---" ) {
				%skipLines = false;
				%beforeObject = false;
			} else if( %skipLines == false ) {
				if(%beforeObject)
					%beforeNewFileLines[ %beforeLines++ ] = %line;
				else
					%newFileLines[ %lines++ ] = %line;
			}
		}
		%fileObject.close();
		%fileObject.delete();

		%fo = new FileObject();
		%fo.openForWrite(%filename);

		// Write out the captured TorqueScript that was before the object before the object
		for( %i = 0; %i <= %beforeLines; %i++)
			%fo.writeLine( %beforeNewFileLines[ %i ] );

		%fo.writeLine("//--- OBJECT WRITE BEGIN ---");
		%fo.writeObject(%parent, "%guiContent = ");
		%fo.writeLine("//--- OBJECT WRITE END ---");

		// Write out captured TorqueScript below Gui object
		for( %i = 0; %i <= %lines; %i++ )
			%fo.writeLine( %newFileLines[ %i ] );

		%fo.close();
		%fo.delete();

		%currentObject.setFileName( makeRelativePath( %filename, getMainDotCsDir() ) );

		GuiEditorStatusBar.print( "Saved file '" @ %parent.getFileName() @ "'" );
	} else
		ToolsMsgBoxOk( "Error writing to file", "There was an error writing to file '" @ %currentFile @ "'. The file may be read-only." );
}
