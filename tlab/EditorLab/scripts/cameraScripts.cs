//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::SyncEditorGui() {
	if (isObject(EditorGui))
		Lab.syncCameraGui();
}

function Lab::CreateCameraViewContextMenu(%this) {
	if( !isObject( LGM.contextMenuField ) )
		Lab.contextMenuCamView = new PopupMenu() {
		superClass = "MenuBuilder";
		isPopup = true;
		item[ 0 ] = "Free view" TAB "" TAB "Lab.setCameraViewMode(\"Standard Camera\");";
		item[ 1 ] = "Orbit view" TAB "" TAB "Lab.setCameraViewMode(\"Orbit Camera\");";
		item[ 2 ] = "1st Person" TAB "" TAB "Lab.setCameraViewMode(\"1st Person Camera\");";
		item[ 3 ] = "3rd Person" TAB "" TAB "Lab.setCameraViewMode(\"3rd Person Camera\");";
		item[ 4 ] = "Smooth move" TAB "" TAB "Lab.setCameraViewMode(\"Smooth Camera\");";
		item[ 5 ] = "Smooth rot." TAB "" TAB "Lab.setCameraViewMode(\"Smooth Rot Camera\");";	
		object = -1;
		profile = "ToolsDropdownProfile";
	};
}
function Lab::showCameraViewContextMenu( %this ) {
	loga("Lab::showCameraViewContextMenu( %this )",%this);

	if( !isObject( Lab.CreateCameraViewContextMenu ))
		Lab.CreateCameraViewContextMenu();

	//Lab.contextMenuField.setItem(0,"-->"@%this.linkedField@"<-- Field Options","");
	//Lab.contextMenuField.setItem(2,"-->"@$LGM_SelectedProfile@"<-- Profile Options","");
	//$LGM_ProfileFieldContext = %this.linkedField;
	Lab.contextMenuCamView.showPopup( Canvas );
}