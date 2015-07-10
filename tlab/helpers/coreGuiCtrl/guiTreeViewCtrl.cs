//==============================================================================
// HelpersLab -> GuiTreeViewCtrl Helpers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Default onDefineIcons functions (Make sure to set valid path to images)
function GuiTreeViewCtrl::onDefineIcons( %this )
{
   %icons = "art/images/treeview/default:" @
            "art/images/treeview/simgroup:" @
            "art/images/treeview/simgroup_closed:" @
            "art/images/treeview/simgroup_selected:" @
            "art/images/treeview/simgroup_selected_closed:" @      
            "art/images/treeview/hidden:" @      
            "art/images/treeview/shll_icon_passworded_hi:" @
            "art/images/treeview/shll_icon_passworded:" @      
            "art/images/treeview/default";
              
   %this.buildIconTable(%icons);   
}
//------------------------------------------------------------------------------
