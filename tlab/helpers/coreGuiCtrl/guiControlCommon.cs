//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Called each time a GuiControl is loaded
function GuiControl::onAdd	(%this) {
    //Check if want to assign to a guiGroup
    if (%this.guiGroup !$= "" ) 
      addCtrlToGuiGroup(%this);   
}
//------------------------------------------------------------------------------

//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function GuiControl::fitIntoParents( %this,%mode ) {	
	
	
   %parent = %this.parentGroup;
	%pos = "0 0";
	%extent = %parent.extent;
	if (%mode $= "" || %mode $= "width"){
		%this.position.x = 0;
		%this.extent.x = %extent.x;
	}
	if (%mode $= "" || %mode $= "height"){
		%this.position.y = 0;
		%this.extent.y = %extent.y;
	}
}
//------------------------------------------------------------------------------

//==============================================================================
// Clear the editors menu (for dev purpose only as now)
function GuiControl::clearChildResponder( %this ) {		
	%responder = %this.getFirstResponder();	
	if (isObject(%responder))
		%responder.clearFirstResponder();
}
//------------------------------------------------------------------------------


//==============================================================================
/// Set a GuiControl to be aligned with parent top, bottom, left or right
/// %direction : Direction to allign with (top,bottom,left,right)
/// %margin(opt.): Set a margin to be added to direction.
/// Usage: GuiControl.AlignCtrlToParent("top",10); //The Y position will be set to 10
function GuiControl::AlignCtrlToParent(%this,%direction,%margin) {
      if (%margin $= "")
         %margin = 0;
     
      %parent = %this.parentGroup;
     if (!isObject(%parent)){
         info(%this.getName()," have no parent to be forced inside");
       
         return;      
      } 
      %posX = %this.getPosition().x;
       %posY = %this.getPosition().y;
      switch$(%direction){
         case "right": //Set max right of parent        
            %posX = %parent.getExtent().x - %this.getExtent().x - %margin;
         case "left": //Set max left of parent        
            %posX= %margin;
         case "top": //Set max left of parent        
            %posY = %margin;
          case "bottom": //Set max right of parent         
          %posY = %parent.getExtent().y - %this.getExtent().y -%margin;          
          
           
         case "centerX": //Set max right of parent    
            %diff = %parent.getExtent().x - %this.getExtent().x;
            %posX = (%parent.getExtent().x - %this.getExtent().x)/2;   
              
      }   
    %this.setPosition(%posX,%posY);
}
//------------------------------------------------------------------------------


//==============================================================================
/// Get the real position of a control inside the canvas. 
function GuiControl::getRealPosition(%this) {   
   %parent = %this.parentGroup;
   %pos = %this.position;
   while(isObject(%parent)){
     %pos.x += %parent.position.x;
      %pos.y += %parent.position.y;
	   %parent = %parent.parentGroup;
   }
   return %pos;
}
//----------------------------------------------------------------------------


//==============================================================================
/// Force a control extent to be inside the extent of another ctrl. (Generally a parent)
function GuiControl::forceInsideCtrl(%this,%ctrl) {   
   %maxX = %ctrl.extent.x - %this.extent.x;
   %maxY = %ctrl.extent.y - %this.extent.y;
   
   if (%maxX < 0 || %maxY < 0){
      warnLog("The control doesn't fit into the target because it's bigger! Source:",%this.extent,"Target:",%ctrl.extent);
      return;
   }  
   %this.position.x = mClamp(%this.position.x,0,%maxX);
   %this.position.y = mClamp(%this.position.y,0,%maxY);  
}
//----------------------------------------------------------------------------

//==============================================================================
// Set Value Safe - From Torque3D (Not sure if needed anymore)
//==============================================================================

//==============================================================================
/// Safe way to set a value without affecting the commands (From Torque3D default)
function GuiControl::setValueSafe(%this, %val) {
	%cmd = %this.command;
	%alt = %this.altCommand;
	%this.command = "";
	%this.altCommand = "";
	%this.setValue(%val);
	%this.command = %cmd;
	%this.altCommand = %alt;
}
//----------------------------------------------------------------------------
//==============================================================================
function GuiControl::shareValueSafe(%this, %dest) {
	%dest.setValueSafe(%this.getValue());
}
//----------------------------------------------------------------------------
//==============================================================================
function GuiControl::shareValueSafeDelay(%this, %dest, %delayMs) {
	%this.schedule(%delayMs, "shareValueSafe", %dest);
}
//----------------------------------------------------------------------------


