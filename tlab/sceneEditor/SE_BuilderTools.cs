//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function SEPLab::getAbsPosition( %this ) {
   devLog("getAbsPos");
  

	%pos = EWorldEditor.getSelectionCentroid();
	SceneEditorTransformTools-->Pos0.setText(getWord(%pos, 0));
	SceneEditorTransformTools-->Pos1.setText(getWord(%pos, 1));
	SceneEditorTransformTools-->Pos2.setText(getWord(%pos, 2));

	// Turn off relative as we're populating absolute values
	%this-->PosRelative.setValue(0);

	// Finally, set the Position check box as active.  The user
	// likely wants this if they're getting the position.
	%this-->DoPosition.setValue(1);

      
}

function SEPLab::setAbsPosition( %this ) {  
	%pos = EWorldEditor.getSelectionCentroid();
	%pos.x = SceneEditorTransformTools-->Pos0.getText(getWord(%pos, 0));
	%pos.y = SceneEditorTransformTools-->Pos1.getText(getWord(%pos, 1));
	%pos.z = SceneEditorTransformTools-->Pos2.getText(getWord(%pos, 2));

	 %this.applyPosition(%pos,false);     
}

function SEPLab::setRelPosition( %this ) {  
	%pos = EWorldEditor.getSelectionCentroid();
	%pos.x = SceneEditorTransformTools-->Pos0.getText(getWord(%pos, 0));
	%pos.y = SceneEditorTransformTools-->Pos1.getText(getWord(%pos, 1));
	%pos.z = SceneEditorTransformTools-->Pos2.getText(getWord(%pos, 2));

	 %this.applyPosition(%pos,true);     
}
function SEPLab::getAbsPosId( %this,%id ) {
   devLog("getAbsPosId",%id);
   %pos = EWorldEditor.getSelectionCentroid();
   %edit = SceneEditorTransformTools.findObjectByInternalName("Pos"@%id,true);
   %edit.setText(getWord(%pos,%id));   
}
function SEPLab::setAbsPosId( %this,%id ) {
   devLog("setAbsPos",%id);
   %pos = EWorldEditor.getSelectionCentroid();
   %edit = SceneEditorTransformTools.findObjectByInternalName("Pos"@%id,true);
   %posId = %edit.getValue();
   
   %newPos = setWord(%pos,%id,%posId);
   %this.applyPosition(%newPos,false);
   
}
function SEPLab::setRelPosId( %this,%id ) {
   devLog("setRelPos",%id);
   %pos = "0 0 0";
   %edit = SceneEditorTransformTools.findObjectByInternalName("Pos"@%id,true);
   %posId = %edit.getValue();
   
   %newPos = setWord(%pos,%id,%posId);
   
   %this.applyPosition(%newPos,true);
}


function SEPLab::applyPosition( %this,%pos,%relative ) {
 
	EWorldEditor.transformSelection(true,%pos,%relative,false,"",false,false,   false,false,false,false);
}

function SEPLab::applyTransformation( %this ) {
   %baseGui = SceneEditorTransformTools;
	%position = %baseGui-->DoPosition.getValue();
	%p = %baseGui-->Pos0.getValue() SPC %baseGui-->Pos1.getValue() SPC %baseGui-->Pos2.getValue();
	%relativePos = %baseGui-->PosRelative.getValue();

	%rotate = %baseGui-->DoRotation.getValue();
	%r = mDegToRad(%baseGui-->Rot0.getValue()) SPC mDegToRad(%baseGui-->Rot1.getValue()) SPC mDegToRad(%baseGui-->Rot2.getValue());
	%relativeRot = %baseGui-->RotRelative.getValue();
	%rotLocal = %baseGui-->RotLocal.getValue();

	// We need to check which Tab page is active
	if( %baseGui-->ScaleTabBook.getSelectedPage() == 0 ) {
		// Scale Page
		%scale = %baseGui-->DoScale.getValue();
		%s = %baseGui-->Scale0.getValue() SPC %baseGui-->Scale1.getValue() SPC %baseGui-->Scale2.getValue();
		%sRelative = %baseGui-->ScaleRelative.getValue();
		%sLocal = %baseGui-->ScaleLocal.getValue();

		%size = false;
	} else {
		// Size Page
		%size = %baseGui-->DoSize.getValue();
		%s = %baseGui-->Size0.getValue() SPC %baseGui-->Size1.getValue() SPC %baseGui-->Size2.getValue();
		%sRelative = %baseGui-->SizeRelative.getValue();
		%sLocal = %baseGui-->SizeLocal.getValue();

		%scale = false;
	}

	EWorldEditor.transformSelection(%position, %p, %relativePos, %rotate, %r, %relativeRot, %rotLocal, %scale ? 1 : (%size ? 2 : 0), %s, %sRelative, %sLocal);
}