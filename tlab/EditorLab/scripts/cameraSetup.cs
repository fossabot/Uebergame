//==============================================================================
// Lab Editor -> Manage the Editor Camera
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
/* Con::setIntVariable( "$EditTsCtrl::DisplayTypeTop", DisplayTypeTop);                = 0
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeBottom", DisplayTypeBottom);          = 1
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeFront", DisplayTypeFront);            = 2
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeBack", DisplayTypeBack);              = 3
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeLeft", DisplayTypeLeft);              = 4
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeRight", DisplayTypeRight);            = 5
   Con::setIntVariable( "$EditTsCtrl::DisplayTypePerspective", DisplayTypePerspective);= 6
   Con::setIntVariable( "$EditTsCtrl::DisplayTypeIsometric", DisplayTypeIsometric);    = 7*/
//==============================================================================

//==============================================================================
// Define some global value
//==============================================================================
$LabDefaultCameraView = "Standard Camera";

$LabCameraDisplayName[0] = "Top View";
$LabCameraDisplayName[1] = "Bottom View";
$LabCameraDisplayName[2] = "Front View";
$LabCameraDisplayName[3] = "Back View";
$LabCameraDisplayName[4] = "Left View";
$LabCameraDisplayName[5] = "Right View";
$LabCameraDisplayName[6] = "Standard Camera";
$LabCameraDisplayName[7] = "Isometric View";

$LabCameraDisplayType["Top View"] = $EditTsCtrl::DisplayTypeTop;
$LabCameraDisplayType["Bottom View"] = $EditTsCtrl::DisplayTypeBottom;
$LabCameraDisplayType["Left View"] = $EditTsCtrl::DisplayTypeLeft;
$LabCameraDisplayType["Right View"] = $EditTsCtrl::DisplayTypeRight;
$LabCameraDisplayType["Front View"] = $EditTsCtrl::DisplayTypeFront;
$LabCameraDisplayType["Back View"] = $EditTsCtrl::DisplayTypeBack;
$LabCameraDisplayType["Isometric View"] = $EditTsCtrl::DisplayTypeIsometric;
$LabCameraDisplayType["Standard Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["1st Person Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["3rd Person Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["Orbit Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["Smooth Camera"] = $EditTsCtrl::DisplayTypePerspective;
$LabCameraDisplayType["Smooth Rot Camera"] = $EditTsCtrl::DisplayTypePerspective;

$LabCameraDisplayMode["Top View"] = "Standard";
$LabCameraDisplayMode["Bottom View"] ="Standard";
$LabCameraDisplayMode["Left View"] = "Standard";
$LabCameraDisplayMode["Right View"] = "Standard";
$LabCameraDisplayMode["Front View"] ="Standard";
$LabCameraDisplayMode["Back View"] = "Standard";
$LabCameraDisplayMode["Isometric View"] = "Standard";
$LabCameraDisplayMode["Standard Camera"] = "Standard";
$LabCameraDisplayMode["1st Person Camera"] = "Player";
$LabCameraDisplayMode["3rd Person Camera"] = "PlayerThird";
$LabCameraDisplayMode["Orbit Camera"] = "Orbit";
$LabCameraDisplayMode["Smooth Camera"] = "Newton";
$LabCameraDisplayMode["Smooth Rot Camera"] = "NewtonDamped";


$LabCameraTypesIcon = "tlab/gui/icons/toolbar_assets/ToggleCamera";

//------------------------------------------------------------------------------
function Lab::setInitialCamera(%this)
{  
   %this.gameControlObject = LocalClientConnection.getControlObject();
   if (!isObject(LocalClientConnection.camera)){
      %this.gameCam = LocalClientConnection.getCameraObject();
      LocalClientConnection.camera = spawnObject("Camera", "Observer");
     
   }
    LocalClientConnection.camera.scopeToClient(LocalClientConnection);
   LocalClientConnection.setCameraObject(LocalClientConnection.camera);
      
   Lab.clientWasControlling = LocalClientConnection.getControlObject();
   if (%this.LaunchInFreeview || !isObject(LocalClientConnection.player)){
      
      %pos = LocalClientConnection.player.getPosition();
      %pos.z += 5;    
      %this.setCameraFreeMode(%pos);
      %this.setCameraViewType(%this.cameraDisplayMode);
        
      return;
   }
   if (LocalClientConnection.getControlObject() != LocalClientConnection.player)
      %this.setCameraPlayerMode();
             
  
}
function Lab::setGameCamera(%this)
{  
   if (isObject(%this.gameCam)){
      %this.gameCam.scopeToClient(LocalClientConnection);
      LocalClientConnection.setCameraObject(%this.gameCam);    
   }  
}

function Lab::toggleControlObject(%this)
{  
   if (!isObject(%this.gameControlObject)){
      warnLog("There's no Game control object stored:",%this.gameControlObject);
      return;
   }
   //If Client is controlling game object, set control camera, else do contrary...
   if (%this.gameControlObject == LocalClientConnection.getControlObject())      
      LocalClientConnection.setCOntrolObject(LocalClientConnection.camera);   
   else
      LocalClientConnection.setCOntrolObject(%this.gameControlObject);  
}

//==============================================================================
function MenuCameraStatus::onWake( %this ) {
	%this.add( "Standard Camera" );
	%this.add( "1st Person Camera" );
	%this.add( "3rd Person Camera" );
	%this.add( "Orbit Camera" );
	%this.add( "Top View" );
	%this.add( "Bottom View" );
	%this.add( "Left View" );
	%this.add( "Right View" );
	%this.add( "Front View" );
	%this.add( "Back View" );
	%this.add( "Isometric View" );
	%this.add( "Smooth Camera" );
	%this.add( "Smooth Rot Camera" );
}

function MenuCameraStatus::onSelect( %this, %id, %text ) {
	Lab.setCameraViewMode(%text);
}


function Lab::setCameraPlayerMode(%this)
{   
   if (!isObject( LocalClientConnection.player)){
      warnLog("You don't have a player assigned, set spawnPlayer true to spawn one automatically");
      return;
   }
   
   %player = LocalClientConnection.player;
   %player.setVelocity("0 0 0");
   LocalClientConnection.setControlObject(%player);
   
   %this.syncCameraGui();
}
function Lab::setCameraFreeMode(%this,%pos)
{      
   %camera = LocalClientConnection.camera;
   %camera.setVelocity("0 0 0");  
   if (%pos !$= "")
      %camera.position = %pos;
   LocalClientConnection.setControlObject(%camera);   
   %this.syncCameraGui();
}

function Lab::setCameraViewMode( %this, %mode,%skipType ) {

	if(%mode $= "Top View") {
		%mode = "Standard Camera";
	}

	Lab.SetEditorCameraView($LabCameraDisplayMode[%mode]);
	// commandToServer( 'SetEditorCameraView', $LabCameraDisplayMode[%mode] );
	if (%skipType)
	   return;
	  
	Lab.setCameraViewType( $LabCameraDisplayType[%mode] );
	Lab.currentCameraMode = %mode;

}
function Lab::setCameraViewType( %this, %type ) { 
	%gui = %this.currentEditor.editorGui;
	if( !isObject( %gui ) )
		return;
   if ($LabCameraDisplayType[%type] !$="")
      %type = $LabCameraDisplayType[%type]; 

   Lab.checkMenuItem("viewTypeMenu",0, 7, %type );


	// Store the current camera rotation so we can restore it correctly when
	// switching back to perspective view
	if ( %gui.getDisplayType() == $EditTSCtrl::DisplayTypePerspective )
		%this.lastPerspectiveCamRotation = LocalClientConnection.camera.getRotation();

	%gui.setDisplayType( %type );

	if ( %gui.getDisplayType() == $EditTSCtrl::DisplayTypePerspective )
		LocalClientConnection.camera.setRotation( %this.lastPerspectiveCamRotation );

	%this.cameraDisplayType = %type;
	
	EToolCamViewDlg.updateCurrentView();
	
}


$SkipCameraSync = false;
function Lab::syncCameraGui( %this,%forced ) {
	if( !EditorIsActive())
		return;


	// Sync projection type
	%displayType = Lab.currentEditor.editorGui.getDisplayType();

	if (!%displayType) %displayType = 6;

   Lab.checkMenuItem("viewTypeMenu",0,7,%displayType);
	

	// Set the camera object's mode and rotation so that it moves correctly
	// based on the current editor mode
	
	if( %displayType != $EditTSCtrl::DisplayTypePerspective ) {
		switch( %displayType ) {
		case $EditTSCtrl::DisplayTypeTop:
			%name = "Top View";
			%camRot = "0 0 0";
		case $EditTSCtrl::DisplayTypeBottom:
			%name = "Bottom View";
			%camRot = "3.14159 0 0";
		case $EditTSCtrl::DisplayTypeLeft:
			%name = "Left View";
			%camRot = "-1.571 0 1.571";
		case $EditTSCtrl::DisplayTypeRight:
			%name = "Right View";
			%camRot = "-1.571 0 -1.571";
		case $EditTSCtrl::DisplayTypeFront:
			%name = "Front View";
			%camRot = "-1.571 0 3.14159";
		case $EditTSCtrl::DisplayTypeBack:
			%name = "Back View";
			%camRot = "-1.571 0 0";
		case $EditTSCtrl::DisplayTypeIsometric:
			%name = "Isometric View";
			%camRot = "0 0 0";
		}

		LocalClientConnection.camera.controlMode = "Fly";
		LocalClientConnection.camera.setRotation( %camRot );

		if( $SkipCameraSync )
			return;
		EditorGuiStatusBar.setCamera( %name );

		return;
	}
   %cameraTypesButton = EditorGuiToolbar-->cameraTypes;
   %cameraTypesButton.setBitmap($LabCameraTypesIcon); //Default Toggle Camera Icon
   
	// Sync camera settings.
	%flyModeRadioItem = -1;
	if(LocalClientConnection.getControlObject() != LocalClientConnection.player) {
		%mode = LocalClientConnection.camera.getMode();
		if( $SkipCameraSync )
			return;

		if(%mode $= "Fly" && LocalClientConnection.camera.newtonMode) {
			if(LocalClientConnection.camera.newtonRotation == true) {
				EditorGui-->NewtonianRotationCamera.setStateOn(true);
				//%cameraTypesButton.setBitmap("tlab/gui/icons/default/menubar/smooth-cam-rot");
				%flyModeRadioItem = 4;
				EditorGuiStatusBar.setCamera("Smooth Rot Camera");
			} else {
				EditorGui-->NewtonianCamera.setStateOn(true);
				//%cameraTypesButton.setBitmap("tlab/gui/icons/default/menubar/smooth-cam");
				%flyModeRadioItem = 3;
				EditorGuiStatusBar.setCamera("Smooth Camera");
			}
		} else if(%mode $= "EditOrbit") {
			EditorGui-->OrbitCamera.setStateOn(true);
			//%cameraTypesButton.setBitmap("tlab/gui/icons/default/menubar/orbit-cam");
			%flyModeRadioItem = 1;
			EditorGuiStatusBar.setCamera("Orbit Camera");
		} else { // default camera mode
			//EditorGui-->StandardCamera.setStateOn(true);
			//%cameraTypesButton.setBitmap("tlab/gui/icons/default/toolbar/camera");
			%flyModeRadioItem = 0;
			EditorGuiStatusBar.setCamera("Standard Camera");
		}

		//quick way select menu bar options
		Lab.checkFindItem("Camera",0,1,0);
      Lab.checkMenuItem("EditorFreeCameraTypeOptions",0, 4, %flyModeRadioItem);
      Lab.checkMenuItem("EditorPlayerCameraTypeOptions",0, 4, -1);
		
	} else if (!$isFirstPersonVar) { // if 3rd person

		if( $SkipCameraSync )
			return;
		//EditorGui-->trdPersonCamera.setStateOn(true);
		//%cameraTypesButton.setBitmap("tlab/gui/icons/default/toolbar/3rd-person-camera");
		%flyModeRadioItem = 1;
		//quick way select menu bar options
		Lab.checkFindItem("Camera",0,1,1);
      Lab.checkMenuItem("EditorPlayerCameraTypeOptions",0, 2, %flyModeRadioItem);     
	
		EditorGuiStatusBar.setCamera("3rd Person Camera");
	} else if ($isFirstPersonVar) { // if 1st Person

		if( $SkipCameraSync )
			return;
		EditorGui-->PlayerCamera.setStateOn(true);
		//%cameraTypesButton.setBitmap("tlab/gui/icons/default/toolbar/player");
		%flyModeRadioItem = 0;
		//quick way select menu bar options
		Lab.checkFindItem("Camera",0,1,1);
      Lab.checkMenuItem("EditorPlayerCameraTypeOptions",0, 2, %flyModeRadioItem);   
       Lab.checkMenuItem("EditorFreeCameraTypeOptions",0, 4, -1);       
		
		EditorGuiStatusBar.setCamera("1st Person Camera");
	}
}



function Lab::fitCameraToSelection( %this,%orbit ) {
   if (%orbit){
      Lab.setCameraViewMode("Orbit Camera",false);
   }
   //GuiShapeEdPreview have it's own function
   if (isObject(Lab.fitCameraGui)){
      Lab.fitCameraGui.fitToShape();
      return;
   }
  %radius = EWorldEditor.getSelectionRadius()+1;
   LocalClientConnection.camera.autoFitRadius(%radius);
   LocalClientConnection.setControlObject(LocalClientConnection.camera);
  %this.syncCameraGui();
   
}


