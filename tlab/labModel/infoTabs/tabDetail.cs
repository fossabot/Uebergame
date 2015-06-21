//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
//LabModelDetails.getShapeInfo();
function LabModelDetails::getShapeInfo( %this ) {
	%shape = LabModel.shape;
	%objName = LabModel.shape.getObjectName( 0 );

	if (!isObject(%shape)) {
		warnLog("No shape selected!");
		return;
	}

	%detailCount = %shape.getDetailLevelCount();

	for(%i=0; %i<%detailCount; %i++) {
		%detName = %shape.getDetailLevelName(%i);
		%detSize = %shape.getDetailLevelSize(%i);
		devLog(%i,"Detail name:",%detName,"Size:",%detSize);
	}

	%meshCount = %shape.getMeshCount(%objName);
	LabModelSelectedShape.meshCount = %meshCount;

	for(%i=0; %i<%meshCount; %i++) {
		%name =   %shape.getMeshName(	%objName,%i);
		%mat = %shape.getMeshMaterial(%name);
		%size = %shape.getMeshSize(%objName,%i);
		%type = %shape.getMeshType(%name);
		LabModelSelectedShape.meshName[%i] = %name;
		LabModelSelectedShape.meshMaterial[%i] = %mat;
		LabModelSelectedShape.meshSize[%i] = %size;
		LabModelSelectedShape.meshType[%i] = %type;
		devLog(%i,"Mesh name:",%name,"Material:",%mat,"Size:",%size,"Type:",%type);
	}
}
//------------------------------------------------------------------------------

//==============================================================================
//LabModelDetails.setSelectedShapeInfo();
function LabModelDetails::setSelectedShapeInfo( %this ) {
	%shape = LabModel.shape;
	%objName = LabModel.shape.getObjectName( 0 );

	if (!isObject(%shape)) {
		warnLog("No shape selected!");
		return;
	}

	%detailMenu = %this-->detailMenu;
	%detailMenu.clear();
	%detailCount = %shape.getDetailLevelCount();

	for(%i=0; %i<%detailCount; %i++) {
		%detName = %shape.getDetailLevelName(%i);
		%detSize = %shape.getDetailLevelSize(%i);
		%detailMenu.add(%detName SPC %detSize,%i);
	}

	%detailMenu.setSelected(0);
}
//------------------------------------------------------------------------------
//==============================================================================
//LabModelDetails.setSelectedShapeInfo();
function LabModelDetails::setSelectedDetailInfo( %this,%detailIndex ) {
	%shape = LabModel.shape;
	%objName = LabModel.shape.getObjectName( 0 );

	if (!isObject(%shape)) {
		warnLog("No shape selected!");
		return;
	}

	%detName = %shape.getDetailLevelName(%detailIndex);
	%detSize = %shape.getDetailLevelSize(%detailIndex);
	%meshName =   %shape.getMeshName(	%objName,%detailIndex);
	%meshMaterial = %shape.getMeshMaterial(%meshName);
	%meshSize = %shape.getMeshSize(%objName,%detailIndex);
	%meshType = %shape.getMeshType(%meshName);
	LabModelSelectedDetail.detailName = %detName;
	LabModelSelectedDetail.detailSize = %detSize;
	LabModelSelectedDetail.meshName = %meshName;
	LabModelSelectedDetail.meshMaterial = %meshMaterial;
	LabModelSelectedDetail.meshSize = %meshSize;
	LabModelSelectedDetail.meshType = %meshType;
}
//------------------------------------------------------------------------------
//==============================================================================
//LabModelDetails.getShapeInfo();
function LabModelDetails::selectDetailMenu( %this ) {
	%detailMenu = %this-->detailMenu;
	%detailIndex = %detailMenu.getSelected();
	LabModelDetails.setSelectedDetailInfo(%detailIndex);
}
//------------------------------------------------------------------------------
