//==============================================================================
// Boost! -> Helper functions for common settings GUI needs
// Copyright NordikLab Studio, 2013
//==============================================================================
//==============================================================================
// Update the GuiControl data field depending of the Class
//==============================================================================
function MenuParams::onSelect(%this,%id,%text){
   %this.text = %text;
}

//==============================================================================
// Params common update functions
//==============================================================================
//==============================================================================
// Generic updateRenderer method
function updateParamCtrl( %ctrl, %updateFunc,%arg1,%arg2,%arg3) {

   

   if (%ctrl.getClassName() $= "GuiColorPickerCtrl"){
 
      %ctrl.updateCommand = %updateFunc@"(%this.internalName,%color,\""@%arg1@"\",\""@%arg2@"\",\""@%arg3@"\");";
      
       %currentColor =   %ctrl.baseColor;
       %callBack = %ctrl@".ColorPicked";
       %updateCallback = %ctrl@".ColorUpdated";       
      
       GetColorF( %currentColor, %callback, %ctrl.getRoot(), %updateCallback, %cancelCallback );
       return;
   }
      
   %field = getWord(strreplace(%ctrl.internalName,"_"," "),0);   
     %value = %ctrl.getTypeValue();  
   

   if (%updateFunc !$= "")
   eval(%updateFunc@"(%field,%value,%arg1,%arg2,%arg3,%ctrl);");   
  

   if (!%ctrl.noFriends)
   updateParamFriends(%ctrl);
   
}
//------------------------------------------------------------------------------
//==============================================================================
// Generic updateRenderer method
function updateParamGlobal( %ctrl, %updateFunc,%arg1,%arg2,%arg3) {

   

   %globalFull = strreplace(%ctrl.internalName,"__","::");   
   
   %global = strreplace(%globalFull,"_"," ");
   %global = getWord(%global,0);   
   
   %value = %ctrl.getTypeValue();
  
  
   eval("$"@%global@ " = "@%value@";");
   
   if (%updateFunc !$= "")
   eval(%updateFunc@"(%global,%value,%arg1,%arg2,%arg3,%ctrl);");   
  
  // if (!%ctrl.noFriends)
   updateParamFriends(%ctrl);
   
}
//------------------------------------------------------------------------------
//==============================================================================
// Generic updateRenderer method
function updateParamFriends( %ctrl) {    
   if (%ctrl.getParent().class $= "AggregateParam" || %ctrl.getParent().class $= "AggregateBox" || %ctrl.getParent().class $= "AggregateGlobal")
      %ctrl.getParent().updateFromChild(%ctrl);
   
}
//------------------------------------------------------------------------------
function syncParamFieldsGlobal( %global,%container,%fields ) {  
    foreach$(%field in %fields){
       eval("%value = $"@%global@%field@";");       
       %ctrl = %container.findObjectByInternalName(%field,true);
       %ctrl.setTypeValue(%value);
       updateParamFriends(%ctrl);
    }   
}

//==============================================================================
// Generic updateRenderer method
function updateParamMenuData( %menu,%menuData,%firstItem) { 
   if (!isObject(%menu)) return;    
   %updType = getWord(%menuData,0);
   %updValue = getWords(%menuData,1); 
   %menu.clear();      
   if (%updType $="group"){
      if(%firstItem !$="")
         %menu.add(%firstItem,0); 
      %menuId = 1;
      foreach(%obj in %updValue){
         %menu.add(%obj.getName(),%menuId);
         %menuId++;
      }
                   
   } 
   else if (%updType $="list"){
      eval("%datalist = $"@%updValue@";");
      if(%firstItem !$="")
         %menu.add(%firstItem,0);            
      %menuId = 1;
      foreach$(%obj in %datalist){
         %menu.add(%obj,%menuId);
         %menuId++;
      }
                   
   }
}
//------------------------------------------------------------------------------

//==============================================================================
// Generic updateRenderer method
function updateParamColorAlpha( %ctrl) {     
   %parent = %ctrl.getParent(); 
   %colorPicker = %parent.findObjectByInternalName(%ctrl.dataType);
   
   %colorPicker.lockedAlpha = %ctrl.lockedAlpha;
   %color = %colorPicker.baseColor;
   %color.a = %ctrl.getValue();
   %colorPicker.colorPicked(%color);
   
}
//------------------------------------------------------------------------------

//==============================================================================
// Empty Editor Gui
function GuiColorPickerCtrl::ColorPicked(%this,%color) {


   %this.baseColor = %color;
   if (%this.alpha !$= "" && !%this.lockedAlpha){
      %this.alphaValue = %color.a;
      %this.baseColor.a = "1";
      %parent = %this.getParent(); 
      %alphaCtrl = %parent.findObjectByInternalName(%this.alpha);
   
      %alphaCtrl.setValue(%this.alphaValue); 
    } 
   else if (%this.lockedAlpha)
       %this.baseColor.a = "1";
   
   if (%this.isIntColor){
      %newColor = ColorFloatToInt(%color);
      %color = %newColor;
   }
      
   eval(%this.updateCommand);
  

}
//------------------------------------------------------------------------------
//==============================================================================
// Empty Editor Gui
function GuiColorPickerCtrl::ColorUpdated(%this,%color) {

    %this.baseColor =  %color;
     if (%this.alpha !$= "" && !%this.lockedAlpha){
      %this.alphaValue = %color.a;
      %this.baseColor.a = "1";
      %parent = %this.getParent(); 
      %alphaCtrl = %parent.findObjectByInternalName(%this.alpha);
   
      %alphaCtrl.setValue(%this.alphaValue);     

    } 
    else if (%this.lockedAlpha)
       %this.baseColor.a = "1";
        
     eval(%this.updateCommand); 
    %this.updateColor();
}
//------------------------------------------------------------------------------
