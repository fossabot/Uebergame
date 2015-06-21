//==============================================================================
// TorqueLab -> Road Editor Node Manager
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$Lab_REP_ShowNodePosition = false;
//==============================================================================
// Manage selected Road Nodes Data
//==============================================================================

//==============================================================================
// Get current road nodes informations
function Lab::getDecalRoadNodes( %this,%refresh ) {
	if (!isObject(RoadEditorPlugin.selectedRoad))
		return;

	if (%refresh) {
		//Keep linked nodes info
		%linkedNodes = Lab.linkedRoadNodes;
	}

	RoadEditorGui.clearRoadNodesData();
	%tmpFile = "tlab/roadEditor/tempData/currentRoad.cs";
	%roadObj = RoadEditorPlugin.selectedRoad;
	%roadObj.save(%tmpFile);
	%fileObj = getFileReadObj(%tmpFile);
	%nodeData = newArrayObject("CurrentRoadNodes");
	%node = 0;

	if (!isObject(%fileObj))
		return;

	while( !%fileObj.isEOF() ) {
		%line = %fileObj.readline();
		%trimmedLine = trim(%line);

		if (getWord(%trimmedLine,0) !$= "Node")
			continue;

		%data = strReplaceList(%trimmedLine,"Node = \"" TAB "" NL"\"" TAB "" NL";" TAB "");
		%nodeData.setVal(%node,%data);
		Lab.roadNodeList = strAddWord(Lab.roadNodeList,%node,true);
		%node++;
	}

	closeFileObj(%fileObj);
	// Add ourselves to the Editor Settings window

	if (%refresh) {
		//Check linkedRoadNodes validity
		foreach$(%node in %linkedNodes) {
			if (strFind(%linkedNodes,%node)) {
				//This node exist, add it
				Lab.linkedRoadNodes = strAddWord(Lab.linkedRoadNodes,%node,true);
			}
		}

		devLog("Refresh data with linked node:",	Lab.linkedRoadNodes);
	}

	RoadEditorGui.updateRoadNodesData(%refresh);
	RoadEd_TabPageNode.active = 1;
	Lab.linkedRoadNodes = "";
}
//------------------------------------------------------------------------------
//==============================================================================
// Add the current road nodes data to the stack
function RoadEditorGui::updateRoadNodesData( %this,%refresh ) {
	RoadEditorTools-->roadNodePageTitle.text = "Road nodes("@CurrentRoadNodes.count()@")";
	%stack = RoadEditorTools-->nodeStack;
	%pillSrc = RoadEditorTools-->nodeStackSample;
	hide(%pillSrc);
	%stack.clear();
	%count = CurrentRoadNodes.count();

	for(%i = 0; %i< %count; %i++) {
		%key = CurrentRoadNodes.getKey(%i);
		%value = CurrentRoadNodes.getValue(%i);
		%pos = getWords(%value,0,2);
		%width = getWord(%value,3);
		%linkIsOn = false;

		if (%refresh && ($REP_LinkedAll || strFind(Lab.linkedRoadNodes,%key)))
			%linkIsOn = true;

		info("Array dump: Index:",%key,"Key=",%pos ,"Value=",%width);
		%pill = cloneObject(%pillSrc,"","key",%stack);
		%pill.internalName = %key;
		%pill-->id.text = %key@"-";
		%pill-->PosEdit.setValue(%pos);
		%pill-->WidthEdit.setValue(%width);
		%pill-->WidthSlider.setValue(%width);
		%pill.node = %key;
		%pill-->PosEdit.node = %key;
		%pill-->WidthEdit.node = %key;
		%pill-->WidthSlider.node = %key;
		%pill-->Linked.node = %key;
		%pill-->Linked.setStateOn(%linkIsOn);
		%pill-->togglePos.command= "toggleVisible("@%pill-->PosCtrl@");";
		%pill-->SelectBtn.command = "RoadEditorGui.setSelectedNode("@%key@");";
		%pill-->SelectBtn.text = "#"@%key;

		if ($Lab_REP_ShowNodePosition)
			%pill-->PosCtrl.visible = 1;
		else
			%pill-->PosCtrl.visible = 0;

		Lab.nodeLink[%key] = %pill-->Linked;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Clear the road nodes data information (No road selected)
function RoadEditorGui::clearRoadNodesData( %this ) {
	RoadEditorTools-->roadNodePageTitle.text = "Select a road first";
	RoadEditorTools-->globalLink.setStateOn(false);
	Lab.roadNodeList = "";
	Lab.linkedRoadNodes = "";
	%stack = RoadEditorTools-->nodeStack;
	%stack.clear();
}
//------------------------------------------------------------------------------

//==============================================================================
// Node Data Update
//==============================================================================
//==============================================================================
function REP::setCurrentNode( %this,%nodeId ) {
	//Start witn linked node so we end with selectednode selected
	if ($REP_CustomNodeSelection)
		return;

	%stack = RoadEditorTools-->nodeStack;
	%selPill = %stack.findObjectByInternalName(%nodeId);

	if (isObject(%selPill)) {
		foreach (%pill in %stack)
			%pill-->SelectBtn.active = 1;

		%selPill-->SelectBtn.active = 0;
	}

	%nodeWidth = RoadEditorGui.getNodeWidth(%nodeId);
	%nodePos = RoadEditorGui.getNodePosition(%nodeId);
	REP_GlobalNodeBox-->CurrentWidthEdit.setValue(%nodeWidth);
	REP_GlobalNodeBox-->CurrentWidthEdit.node = %nodeId;
}
//------------------------------------------------------------------------------



//==============================================================================
// Node Manager Functions
//==============================================================================

//==============================================================================
// Link nodes together for simultaneous update
function RoadEditorGui::setNodeLink( %this, %ctrl,%linkAll ) {
	%linked = %ctrl.isStateOn();
	$REP_LinkedAll = false;

	if (%linkAll) {
		if (%linked) {
			$REP_LinkedAll = true;
			Lab.linkedRoadNodes = Lab.roadNodeList;
			devLog("All Node added:Linked nodes=",Lab.linkedRoadNodes);
		} else {
			Lab.linkedRoadNodes = "";
			devLog("All Node removed:",Lab.linkedRoadNodes);
		}

		foreach$(%node in Lab.roadNodeList) {
			%linkCtrl = Lab.nodeLink[%node];
			%linkCtrl.setStateOn(%linked);
		}

		return;
	}

	if (%linked) {
		Lab.linkedRoadNodes = strAddWord(Lab.linkedRoadNodes,%ctrl.node,true);
		devLog("Node added:",%ctrl.node,"Linked nodes=",Lab.linkedRoadNodes);
		return;
	}

	Lab.linkedRoadNodes = strRemoveWord(Lab.linkedRoadNodes,%ctrl.node);
	devLog("Node removed:",%ctrl.node,"Linked nodes=",Lab.linkedRoadNodes);
}
//------------------------------------------------------------------------------

//==============================================================================
// Node Width Adjustement functions
//==============================================================================
//==============================================================================
function REP::setNodeWidth( %this, %node,%width,%skipPillUpdate ) {
	if (%width <= 0) {
		warnLog("Can't set node width smaller or equal to 0! Attempted value=",%width);
		return;
	}

	RoadEditorGui.setSelectedNode(%node);
	RoadEditorGui.setNodeWidth(%width);
	%stack = RoadEditorTools-->nodeStack;
	%nodePill = %stack.findObjectByInternalName(%node);

	if (!isObject(%nodePill) /*|| %skipPillUpdate*/)
		return;

	%nodePill-->WidthEdit.setValue(%width);
	%nodePill-->WidthSlider.setValue(%width);
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::setLinkedGlobalWidth( %this ) {
	%value = REP_GlobalNodeBox-->SetWidthEdit.getText();
	%this.setLinkedNodeWidth(%value,false,true);
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::setLinkedRelativeWidth( %this ) {
	%value = REP_GlobalNodeBox-->RelativeWidthEdit.getText();
	%this.setLinkedNodeWidth(%value,true,true);
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::setLinkedNodeWidth( %this, %value,%isRelative,%updateCurrent ) {
	devLog("RoadEditorGui setNodeDataWidth",%ctrl);
	%curNode = %this.getSelectedNode();

	foreach$(%nodeId in Lab.linkedRoadNodes) {
		if (%nodeId $= %curNode && !%updateCurrent)
			continue;

		%width = %value;

		if (%isRelative) {
			RoadEditorGui.setSelectedNode(%nodeId);
			%width = %this.getNodeWidth(%nodeId) + %value;
		}

		REP.setNodeWidth(%nodeId,%width);
		devLog("set Link NodeData Width Node",%nodeId,"Value",%width);
	}

	if (%this.getSelectedNode() !$= %curNode)
		RoadEditorGui.setSelectedNode(%curNode);

	$REP_CustomNodeSelection = false;
}
//------------------------------------------------------------------------------
//==============================================================================
function RoadEditorGui::setNodeDataWidth( %this, %ctrl,%linkOnly ) {
	devLog("RoadEditorGui setNodeDataWidth",%ctrl);
	$REP_CustomNodeSelection = true;
	//Start witn linked node so we end with selectednode selected
	%width = %ctrl.getValue();
	%curNode = %ctrl.node;

	if (%curNode $= "") {
		warnLog("No node submitted for set Node data width. Using selected node!");
		%curNode = %this.getSelectedNode();

		if (%curNode $= "") {
			warnLog("No node selected. Select a node before changing width.");
			return;
		}
	}

	%this.setLinkedNodeWidth(%width);
	REP.setNodeWidth(%curNode,%width,true);
	$REP_CustomNodeSelection = false;
}
//------------------------------------------------------------------------------


//==============================================================================
// Set Relative Width
function RoadEditorGui::setRelativeWidth( %this, %ctrl ) {
	devLog("RoadEditorGui setRelativeWidth",%ctrl);
	$REP_CustomNodeSelection = true; //Used to tell script the incoming selection are from script
	%width = %ctrl.getValue();
	%selNode = %this.getSelectedNode();
	%this.setLinkedNodeWidth(%width,true);
	%curWidth = %this.getNodeWidth(%selNode) + %width;

	if (%curWidth <= 0) {
		warnLog("Node:",%selNode,"Can't set node width under 0:",%curWidth,"Set node width skipped");
		return;
	} else {
		REP.setNodeWidth(%selNode,%curWidth);
		//%this.setNodeWidth(%curWidth);
	}

	$REP_RelativeWidthAdjust = 0;
	$REP_CustomNodeSelection = false; //Used to tell script the incoming selection are from script
}
//------------------------------------------------------------------------------

//==============================================================================
// Node Position Adjustement functions
//==============================================================================
function RoadEditorGui::setNodeDataPos( %this, %ctrl ) {
	//Start witn linked node so we end with selectednode selected
	foreach$(%nodeId in Lab.linkedRoadNodes) {
		if (%nodeId $= %ctrl.node)
			continue;

		%this.setSelectedNode(%nodeId);
		%this.setNodePosition(%ctrl.getValue());
	}

	%this.setSelectedNode(%ctrl.node);
	%this.setNodePosition(%ctrl.getValue());
	devLog("setNodeDataPos Node",%ctrl.node,"Value",%ctrl.getValue());
}
//------------------------------------------------------------------------------




//==============================================================================
function REPGlobalWidthEdit::onReturn( %this ) {
	devLog("REPGlobalWidthEdit::onReturn",%this);
}
function REPGlobalWidthEdit::onValidate( %this ) {
	devLog("REPGlobalWidthEdit::onValidate",%this);
}
function REPGlobalWidthEdit::onTabComplete( %this,%val ) {
	devLog("REPGlobalWidthEdit::onTabComplete",%this,%val);
}
//------------------------------------------------------------------------------

/*
new DecalRoad(RoadA) {
   Material = "Mat_Farm_DirtPathA";
   textureLength = "5";
   breakAngle = "3";
   renderPriority = "10";
   position = "281.549 534.766 77.0376";
   rotation = "1 0 0 0";
   scale = "1 1 1";
   canSave = "1";
   canSaveDynamicFields = "1";
      byGroup = "0";

   Node = "281.549438 534.765686 77.037598 10.000000";
   Node = "263.842529 479.133606 84.437988 10.000000";
   Node = "245.589066 429.808502 95.093262 10.000000";
   Node = "234.988129 392.732849 82.561523 10.000000";
   Node = "263.659149 312.105927 76.943359 10.000000";
   Node = "243.086487 257.008942 80.910156 10.000000";
   Node = "175.321808 201.690506 95.187500 10.000000";
   Node = "55.601074 270.340820 58.212402 10.000000";
};
*/