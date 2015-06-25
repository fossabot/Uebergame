// #inventory
// This method will start the drag and drop operation
function DragControl::onMouseDragged( %this )
{
   // Only perform a drag and drop operation if the
   // button has a bitmap defined.
   if (%this.bitmap $= "")
      return;
	  
   // Create the temporary control that forms the payload
   // for the drag operation.
   %payload = new GuiBitmapButtonCtrl();
   
   // Copy the fields from this control into the payload
   %payload.assignFieldsFrom( %this );
   
   // Reset the payload's position
   %payload.position = "0 0";
   
   // Store where this payload cam from.
   %payload.dragSourceControl = %this;
   
   // Calculate the local center of the payload.  We use
   // this to center the control on the cursor.
   %xOffset = getWord( %payload.extent, 0 ) / 2;
   %yOffset = getWord( %payload.extent, 1 ) / 2;
   
   // Calculate the initial position of the
   // GuiDragAndDropControl we are about to create.
   %cursorpos = Canvas.getCursorPos();
   %xPos = getWord( %cursorpos, 0 ) - %xOffset;
   %yPos = getWord( %cursorpos, 1 ) - %yOffset;
   
   // Create the drag control
   %ctrl = new GuiDragAndDropControl()
   {
      profile           = "GuiSolidDefaultProfile";
      position          = %xPos SPC %yPos;
      extent            = %payload.extent;
	  
      // Allow this control to automatically delete
      // itself on mouse up. When the drag is aborted
      // this will also delete the payload.
      deleteOnMouseUp   = true;
	  
      // Use the class field to differentiate between
      // types of drags.
      class             = "GuiDragAndDropControlType_Inventory";
				 
      // Indicate that the payload has not yet been
      // delivered.  This is used to return the payload
      // back to the original owner if it was dropped
      // on an invalid location.
      payloadDelivered  = false;
   };
   
   // Add the payload
   %ctrl.add( %payload );
   
   // Remove the bitmap from this control as it is now
   // within the payload.
   %this.bitmap = "";
   
   // Start the drag by adding the control to the Canvas
   Canvas.getContent().add( %ctrl );
   %ctrl.startDragging( %xOffset, %yOffset);
}

// This method is triggered when the mouse is released
// over the control while in the middle of a drag and
// drop operation.
function DragControl::onControlDropped( %this, %payload, %position )

{
   // Make sure this is an inventory type drop
   if (!%payload.parentGroup.isInNamespaceHierarchy( 
       "GuiDragAndDropControlType_Inventory" ))
   {
         return;
   }
   
   // Check if this control already has a bitmap
   // assigned.  If so then we'll abort the operation.
   if (%this.bitmap !$= "")
   {
      // Send the bitmap back home
      %payload.dragSourceControl.bitmap = %payload.bitmap;
      return;
   }
   
   // Copy the bitmap to us from the payload
   %this.bitmap = %payload.bitmap;
   
   // Indicate that the payload has been delivered
   %payload.payloadDelivered = true;
}

// This method is called when our drag and drop object
// is deleted.
function GuiDragAndDropControlType_Inventory::onRemove(%this)
{
   // Has the payload been delivered?  If not, send it back
   // to the owner.
   if (!%this.payloadDelivered)
   {
      %payload = %this.getObject(0);
      %owner = %payload.dragSourceControl;
      // Give the bitmap back to the original owner
      %owner.bitmap = %payload.bitmap;
   }
}