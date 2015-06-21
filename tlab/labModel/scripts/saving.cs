//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


function LabModel::isDirty( %this ) {
	return ( isObject( %this.shape ) && LabModelPropWindow-->saveBtn.isActive() );
}

function LabModel::setDirty( %this, %dirty ) {
	if ( %dirty )
		LabModelSelectWindow.text = "Shapes *";
	else
		LabModelSelectWindow.text = "Shapes";

	LabModelPropWindow-->saveBtn.setActive( %dirty );
}

function LabModel::saveChanges( %this ) {
	if ( isObject( LabModel.shape ) ) {
		LabModel.saveConstructor( LabModel.shape );
		LabModel.shape.writeChangeSet();
		LabModel.shape.notifyShapeChanged();      // Force game objects to reload shape
		LabModel.setDirty( false );
	}
}

//------------------------------------------------------------------------------
// Update bounds
function LabModel::doSetBounds( %this ) {
	%action = %this.createAction( ActionSetBounds, "Set bounds" );
	%action.oldBounds = LabModel.shape.getBounds();
	%action.newBounds = LabModelPreview.computeShapeBounds();
	%this.doAction( %action );
}

function ActionSetBounds::doit( %this ) {
	return LabModel.shape.setBounds( %this.newBounds );
}

function ActionSetBounds::undo( %this ) {
	Parent::undo( %this );
	LabModel.shape.setBounds( %this.oldBounds );
}
