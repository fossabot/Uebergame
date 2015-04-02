//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

//---------------------------------------------------------------------------------------------
// Path to the folder that contains the editors we will load.
//---------------------------------------------------------------------------------------------
$Lab::resourcePath = "tlab/";

// These must be loaded first, in this order, before anything else is loaded
$Lab::loadFirst = "editorClasses base sceneEditor";

//---------------------------------------------------------------------------------------------
// Object that holds the simObject id that the materialEditor uses to interpret its material list
//---------------------------------------------------------------------------------------------
$Lab::materialEditorList = "";

$LabIgnoreEnableFolderList = "base debugger editorClasses forestEditor levels resources";

if (!$HelperLabLoaded)
   exec("./helpers/initHelpers.cs"); 
//---------------------------------------------------------------------------------------------
// Tools Package.
//---------------------------------------------------------------------------------------------
function EditorIsActive() {
    return ( isObject(EditorGui) && Canvas.getContent() == EditorGui.getId() );
}

function GuiEditorIsActive() {
    return ( isObject(GuiEditorGui) && Canvas.getContent() == GuiEditorGui.getId() );
}



package Lab {
   
    // Start-up.
    function onStart() {
        Parent::onStart();       
        new Settings(EditorSettings) {
            file = "tlab/settings.xml";
        };
        EditorSettings.read();

         new Settings(LabCfg) {
            file = "tlab/config.xml";
        };
        

        echo( " % - Initializing Tools" );
      
        // Default file path when saving from the editor (such as prefabs)
        if ($Pref::WorldEditor::LastPath $= "") {
            $Pref::WorldEditor::LastPath = getMainDotCsDir();
        }

        $Lab = new ScriptObject(Lab);
        $LabExecGui = true;
        exec("tlab/EditorLab/execScripts.cs");
        // Common GUI stuff.
        Lab.initLabEditor();

        //%toggle = $Scripts::ignoreDSOs;
        //$Scripts::ignoreDSOs = true;

        $ignoredDatablockSet = new SimSet();

        // fill the list of editors
        $editors[count] = getWordCount( $Lab::loadFirst );
        for ( %i = 0; %i < $editors[count]; %i++ ) {
            $editors[%i] = getWord( $Lab::loadFirst, %i );
        }

        %pattern = $Lab::resourcePath @ "/*/main.cs";
        %folder = findFirstFile( %pattern );
        if ( %folder $= "") {
            // if we have absolutely no matches for main.cs, we look for main.cs.dso
            %pattern = $Lab::resourcePath @ "/*/main.cs.dso";
            %folder = findFirstFile( %pattern );
        }
        while ( %folder !$= "" ) {
            if( filePath( %folder ) !$= "tools" ) { // Skip the actual 'tools' folder...we want the children
                %folder = filePath( %folder );
                %editor = fileName( %folder );
                if ( IsDirectory( %folder ) ) {
                    // Yes, this sucks and should be done better
                    if ( strstr( $Lab::loadFirst, %editor ) == -1 ) {
                        $editors[$editors[count]] = %editor;
                        $editors[count]++;
                    }
                }
            }
            %folder = findNextFile( %pattern );
        }

        // initialize every editor

        %count = $editors[count];
        //  exec( "./worldEditor/main.cs" );
        foreach$(%tmpFolder in $LabIgnoreEnableFolderList)
            $ToolFolder[%tmpFolder] = "1";
        for ( %i = 0; %i < %count; %i++ ) {
            eval("%enabledEd = $pref::WorldEditor::"@$editors[%i]@"::Enabled;");
            if (!%enabledEd && !$ToolFolder[%tmpFolder]) {
                continue;
            }
            exec( "./" @ $editors[%i] @ "/main.cs" );

            %initializeFunction = "initialize" @ $editors[%i];
            if( isFunction( %initializeFunction ) )
                call( %initializeFunction );
        }

        // Popuplate the default SimObject icons that
        // are used by the various editors.
        EditorIconRegistry::loadFromPath( "tlab/gui/icons/class_assets/" );

        // Load up the tools resources. All the editors are initialized at this point, so
        // resources can override, redefine, or add functionality.
        Lab::LoadResources( $Lab::resourcePath );

        //$Scripts::ignoreDSOs = %toggle;

        if(isWebDemo()) {
            // if this is the web tool demo lets init some value storage
            //$clicks
        }
        Lab.pluginInitCompleted();
    }

    function startToolTime(%tool) {
        if($toolDataToolCount $= "")
            $toolDataToolCount = 0;

        if($toolDataToolEntry[%tool] !$= "true") {
            $toolDataToolEntry[%tool] = "true";
            $toolDataToolList[$toolDataToolCount] = %tool;
            $toolDataToolCount++;
            $toolDataClickCount[%tool] = 0;
        }

        $toolDataStartTime[%tool] = getSimTime();
        $toolDataClickCount[%tool]++;
    }

    function endToolTime(%tool) {
        %startTime = 0;

        if($toolDataStartTime[%tool] !$= "")
            %startTime = $toolDataStartTime[%tool];

        if($toolDataTotalTime[%tool] $= "")
            $toolDataTotalTime[%tool] = 0;

        $toolDataTotalTime[%tool] += getSimTime() - %startTime;
    }

    function dumpToolData() {
        %count = $toolDataToolCount;
        for(%i=0; %i<%count; %i++) {
            %tool = $toolDataToolList[%i];
            %totalTime = $toolDataTotalTime[%tool];
            if(%totalTime $= "")
                %totalTime = 0;
            %clickCount = $toolDataClickCount[%tool];
            echo("---");
            echo("Tool: " @ %tool);
            echo("Time (seconds): " @ %totalTime / 1000);
            echo("Activated: " @ %clickCount);
            echo("---");
        }
    }

    // Shutdown.
    function onExit() {
        if( LabEditor.isInitialized )
            EditorGui.shutdown();

        // Free all the icon images in the registry.
        EditorIconRegistry::clear();

        // Save any Layouts we might be using
        //GuiFormManager::SaveLayout(LevelBuilder, Default, User);

        %count = $editors[count];
        for (%i = 0; %i < %count; %i++) {
            %destroyFunction = "destroy" @ $editors[%i];
            if( isFunction( %destroyFunction ) )
                call( %destroyFunction );
        }

        // Call Parent.
        Parent::onExit();

        // write out our settings xml file
        //EditorSettings.write();
       

    }
};

function Lab::LoadResources( %path ) {
    %resourcesPath = %path @ "resources/";
    %resourcesList = getDirectoryList( %resourcesPath );

    %wordCount = getFieldCount( %resourcesList );
    for( %i = 0; %i < %wordCount; %i++ ) {
        %resource = GetField( %resourcesList, %i );
        if( isFile( %resourcesPath @ %resource @ "/resourceDatabase.cs") )
            ResourceObject::load( %path, %resource );
    }
}

//-----------------------------------------------------------------------------
// Activate Package.
//-----------------------------------------------------------------------------
activatePackage(Lab);

