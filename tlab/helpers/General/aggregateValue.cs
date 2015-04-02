//==============================================================================
// Boost! -> Main functions used on game launch
// Copyright NordikLab Studio, 2013
//==============================================================================


//------------------------------------------------------------------------------
// An Aggregate Control is a plain GuiControl that contains other controls,
// which all share a single job or represent a single value.
//------------------------------------------------------------------------------

// AggregateControl.setValue( ) propagates the value to any control that has an
// internal name.
function AggregateControl::setValue(%this, %val, %child) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if( %obj == %child )
			continue;
		if(%obj.internalName !$= "")
			%obj.setValueSafe(%val);
	}
}

// AggregateControl.getValue() uses the value of the first control that has an
// internal name, if it has not cached a value via .setValue
function AggregateControl::getValue(%this) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if(%obj.internalName !$= "") {
			//error("obj = " @ %obj.getId() @ ", " @ %obj.getName() @ ", " @ %obj.internalName );
			//error(" value = " @ %obj.getValue());
			return %obj.getValue();
		}
	}
}

// AggregateControl.updateFromChild( ) is called by child controls to propagate
// a new value, and to trigger the onAction() callback.
function AggregateControl::updateFromChild(%this, %child) {
	%val = %child.getValue();
	if(%val == mCeil(%val)) {
		%val = mCeil(%val);
	} else {
		if ( %val <= -100) {
			%val = mCeil(%val);
		} else if ( %val <= -10) {
			%val = mFloatLength(%val, 1);
		} else if ( %val < 0) {
			%val = mFloatLength(%val, 2);
		} else if ( %val >= 1000) {
			%val = mCeil(%val);
		} else if ( %val >= 100) {
			%val = mFloatLength(%val, 1);
		} else if ( %val >= 10) {
			%val = mFloatLength(%val, 2);
		} else if ( %val > 0) {
			%val = mFloatLength(%val, 3);
		}
	}
	%this.setValue(%val, %child);
	%this.onAction();
}

// default onAction stub, here only to prevent console spam warnings.
function AggregateControl::onAction(%this) {
}

// call a method on all children that have an internalName and that implement the method.
function AggregateControl::callMethod(%this, %method, %args) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if(%obj.internalName !$= "" && %obj.isMethod(%method))
			eval(%obj @ "." @ %method @ "( " @ %args @ " );");
	}
}

//==============================================================================
// GuiControlSpecific Aggregate Functions
//==============================================================================

function DropDownTextEditCtrl::updateFromChild( %this, %ctrl ) {
	if( %ctrl.internalName $= "PopUpMenu" ) {
		%this->TextEdit.setText( %ctrl.getText() );
	} else if ( %ctrl.internalName $= "TextEdit" ) {
		%popup = %this->PopupMenu;
		%popup.changeTextById( %popup.getSelected(), %ctrl.getText() );
		%this.onRenameItem();
	}
}

