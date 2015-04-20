//==============================================================================
// TorqueLab -> GameLab (In-Game Editor GUI)
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::initGameLab( %this ) {
   exec("tlab/GameLab/gui/GameLabDlg.gui");
   exec("tlab/GameLab/gui/GameLabDlg.cs");
   
   exec("tlab/GameLab/scripts/manageGameGuis.cs");
}
//------------------------------------------------------------------------------
