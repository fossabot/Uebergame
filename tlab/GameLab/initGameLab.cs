//==============================================================================
// TorqueLab -> GameLab (In-Game Editor GUI)
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::initGameLab( %this ) {
   exec("tlab/GameLab/gui/ToolsGameLabDlg.gui");
   exec("tlab/GameLab/gui/ToolsGameLabDlg.cs");
   
   exec("tlab/GameLab/scripts/manageGameGuis.cs");
}
//------------------------------------------------------------------------------
