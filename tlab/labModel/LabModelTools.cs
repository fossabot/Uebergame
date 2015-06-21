//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$Cfg_Plugins_LabModel_ShowAdvancedPanel = false;
$Cfg_Plugins_LabModel_ShowAnimationPanel = false;
$Cfg_Plugins_LabModel_UseSimplifiedSystem = true;
//==============================================================================
// SceneEditorTools Frame Set Scripts
//==============================================================================
function LabModelTools::sePanelVisibility( %this ) {
	devLog("LabModelTools::togglePanelVisibility( %this,%checkbox )",%this,%checkbox);
	%panel = %checkbox.panel;
}

function LabModelTools::updatePanels( %this ) {
	devLog("LabModelTools::updatePanels:",%this);

	

	%showAdvanced = $Cfg_Plugins_LabModel_ShowAdvancedPanel;
	%showAnimation = $Cfg_Plugins_LabModel_ShowAnimationPanel;
	devLog("LabModelTools::updatePanels Animation=",%showAnimation,"Advanced=",%showAdvanced);
	%extent = 	LabModelTools.extent.y;
	%mainPanelSize = getWord(LabModelTools.rows,1);
	%rows = "0";	
	%currentIndex = 0;
	%this.showCtrl(LabModelPropWindow,%currentIndex++);
	
	%this.hideCtrl(LabModelAdvancedWindow);
	
}

function LabModelTools::updateSimplifiedPanels( %this ) {
	devLog("LabModelTools::updateSimplifiedPanels:",%this);
	%this.hideCtrl(LabModelAdvancedWindow);
	%this.hideCtrl(LabModelAnimationWindow);
	%this.showCtrl(LabModelPropWindow);
	%this.showCtrl(LabModelSubPanel,1);
	%rows = getWords(LabModelTools.rows,0,1);
	%this.setRows(%rows);
}


//------------------------------------------------------------------------------

function LabModelSeqNodeTabBook::onTabSelected( %this, %name, %index ) {
	%this.activePage = %name;

	switch$ ( %name ) {
	case "Seq":
		LabModelPropWindow-->newBtn.ToolTip = "Add new sequence";
		LabModelPropWindow-->newBtn.Command = "LabModelSequences.onAddSequence();";
		LabModelPropWindow-->newBtn.setActive( true );
		LabModelPropWindow-->deleteBtn.ToolTip = "Delete selected sequence (cannot be undone)";
		LabModelPropWindow-->deleteBtn.Command = "LabModelSequences.onDeleteSequence();";
		LabModelPropWindow-->deleteBtn.setActive( true );

	case "Node":
		LabModelPropWindow-->newBtn.ToolTip = "Add new node";
		LabModelPropWindow-->newBtn.Command = "LabModelNodes.onAddNode();";
		LabModelPropWindow-->newBtn.setActive( true );
		LabModelPropWindow-->deleteBtn.ToolTip = "Delete selected node (cannot be undone)";
		LabModelPropWindow-->deleteBtn.Command = "LabModelNodes.onDeleteNode();";
		LabModelPropWindow-->deleteBtn.setActive( true );

	case "Detail":
		LabModelPropWindow-->newBtn.ToolTip = "";
		LabModelPropWindow-->newBtn.Command = "";
		LabModelPropWindow-->newBtn.setActive( false );
		LabModelPropWindow-->deleteBtn.ToolTip = "Delete the selected mesh or detail level (cannot be undone)";
		LabModelPropWindow-->deleteBtn.Command = "LabModelDetails.onDeleteMesh();";
		LabModelPropWindow-->deleteBtn.setActive( true );

	case "Mat":
		LabModelPropWindow-->newBtn.ToolTip = "";
		LabModelPropWindow-->newBtn.Command = "";
		LabModelPropWindow-->newBtn.setActive( false );
		LabModelPropWindow-->deleteBtn.ToolTip = "";
		LabModelPropWindow-->deleteBtn.Command = "";
		LabModelPropWindow-->deleteBtn.setActive( false );
		// For some reason, the header is not resized correctly until the Materials tab has been
		// displayed at least once, so resize it here too
		//LabModelMaterials-->materialListHeader.setExtent( getWord( LabModelMaterialList.extent, 0 ) SPC "19" );
	}
}

