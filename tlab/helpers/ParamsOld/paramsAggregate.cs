//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// AggregateParam
//------------------------------------------------------------------------------
// An Aggregate Control is a plain GuiControl that contains other controls,
// which all share a single job or represent a single value.
//==============================================================================

//==============================================================================
// SetValue-> propagates the value to any control that has an internal name.
function AggregateParam::setValue(%this, %val, %child) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if( %obj == %child )
			continue;



		if(%obj.internalName !$= "")
			%obj.setValueSafe(%val);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// getValue-> uses the value of the first control that has an internal name, if it has not cached a value via .setValue
function AggregateParam::getValue(%this) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if(%obj.internalName !$= "") {
			//error("obj = " @ %obj.getId() @ ", " @ %obj.getName() @ ", " @ %obj.internalName );
			//error(" value = " @ %obj.getValue());
			return %obj.getValue();
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// updateFromChild-> called by child controls to propagate a new value, and to trigger the onAction() callback.
function AggregateParam::updateFromChild(%this, %child) {

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
//------------------------------------------------------------------------------
// onAction-> here only to prevent console spam warnings.
function AggregateParam::onAction(%this) {

}
//------------------------------------------------------------------------------
// callMethod-> call a method on all children that have an internalName and that implement the method.
function AggregateParam::callMethod(%this, %method, %args) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if(%obj.internalName !$= "" && %obj.isMethod(%method))
			eval(%obj @ "." @ %method @ "( " @ %args @ " );");
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// AggregateBox
//------------------------------------------------------------------------------
// An Aggregate Control is a plain GuiControl that contains other controls,
// which all share a single job or represent a single value.
//==============================================================================
function AggregateBox::updateFromChild(%this, %child) {
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

//==============================================================================
// SetValue-> propagates the value to any control that has an internal name.
function AggregateBox::setValue(%this, %val, %child) {
	logd(" AggregateBox::setValue( %this, %val, %child )", %this, %val, %child );
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);

		if( %obj == %child )
			continue;

		if(%obj.internalName !$= "" && %obj.internalName !$= "field")
			%obj.setValueSafe(%val);
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// getValue-> uses the value of the first control that has an internal name, if it has not cached a value via .setValue
function AggregateBox::getValue(%this) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if(%obj.internalName !$= "") {
			//error("obj = " @ %obj.getId() @ ", " @ %obj.getName() @ ", " @ %obj.internalName );
			//error(" value = " @ %obj.getValue());
			return %obj.getValue();
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// updateFromChild-> called by child controls to propagate a new value, and to trigger the onAction() callback.
function AggregateBox::updateFromChild(%this, %child) {
	%val = %child.getValue();
	if (%child.skipPrecision) {
		%this.setValue(%val, %child);
		%this.onAction();
		return;
	}
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
//------------------------------------------------------------------------------
// onAction-> here only to prevent console spam warnings.
function AggregateBox::onAction(%this) {

}
//------------------------------------------------------------------------------
// callMethod-> call a method on all children that have an internalName and that implement the method.
function AggregateBox::callMethod(%this, %method, %args) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if(%obj.internalName !$= "" && %obj.isMethod(%method))
			eval(%obj @ "." @ %method @ "( " @ %args @ " );");
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// AggregateBox
//------------------------------------------------------------------------------
// An Aggregate Control is a plain GuiControl that contains other controls,
// which all share a single job or represent a single value.
//==============================================================================

//==============================================================================
// SetValue-> propagates the value to any control that has an internal name.
function AggregateGlobal::setValue(%this, %val, %child) {

	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);

		if( %obj == %child )
			continue;

		if(%obj.internalName !$= "" && %obj.internalName !$= "field")
			%obj.setValueSafe(%val);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// updateFromChild-> called by child controls to propagate a new value, and to trigger the onAction() callback.
function AggregateGlobal::updateFromChild(%this, %child) {

	eval("%val = "@%child.variable@";");

	%this.setValue(%val, %child);
	%this.onAction();
}
//------------------------------------------------------------------------------
// onAction-> here only to prevent console spam warnings.
function AggregateGlobal::onAction(%this) {

}
//------------------------------------------------------------------------------
// callMethod-> call a method on all children that have an internalName and that implement the method.
function AggregateGlobal::callMethod(%this, %method, %args) {
	for(%i = 0; %i < %this.getCount(); %i++) {
		%obj = %this.getObject(%i);
		if(%obj.internalName !$= "" && %obj.isMethod(%method))
			eval(%obj @ "." @ %method @ "( " @ %args @ " );");
	}
}
//------------------------------------------------------------------------------