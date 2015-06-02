//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//----------------------------------------------------------------------------
// Enter Chat Message Hud
//----------------------------------------------------------------------------

function MessageHud::open(%this)
{
   %offset = 6;

   if(%this.isVisible())
      return;

   if(%this.isTeamMsg)
      %text = "TEAM CHAT:";
   else if(%this.isFireTeamMsg)
      %text = "FIRE TEAM:";
   else
      %text = "GLOBAL CHAT:";

   MessageHud_Text.setValue(%text);

   %windowPos = getWord( messageHud_Frame.position, 0 ) SPC ( getWord( outerChatHud.position, 1 ) + getWord( outerChatHud.extent, 1 ) + 5 );
   messageHud_Frame.position = %windowPos;

   Canvas.pushDialog(%this);
   %this.setVisible(true);
   deactivateKeyboard();
   MessageHud_Edit.makeFirstResponder(true);

   if ( isObject( hudMap ) )
   {
      hudMap.pop();
      hudMap.delete();
   }
   new ActionMap( hudMap );
   hudMap.blockBind( moveMap, bringUpOptions );
   hudMap.blockBind( moveMap, toggleTeamChoose );
   hudMap.blockBind( moveMap, showPlayerList );
   hudMap.blockBind( moveMap, showScoreBoard );
   hudMap.bindCmd( keyboard, "escape", "", "MessageHud.close();" );
   hudMap.push();
}

//------------------------------------------------------------------------------

function MessageHud::close(%this)
{
   if(!%this.isVisible())
      return;
      
   Canvas.popDialog(%this);
   %this.setVisible(false);
   if ( $enableDirectInput )
      activateKeyboard();

   MessageHud_Edit.setValue("");
   $PrivMsgTarget = "";

   // Make sure the proper key maps are pushed
   tge.updateKeyMaps();
}

//------------------------------------------------------------------------------

function MessageHud::toggleState(%this)
{
   if(%this.isVisible())
      %this.close();
   else
      %this.open();
}

//------------------------------------------------------------------------------

function MessageHud_Edit::onEscape(%this)
{
   MessageHud.close();
}

//------------------------------------------------------------------------------

function MessageHud_Edit::eval(%this)
{
   %text = trim(%this.getValue());
   if(%text !$= "")
   {
      if($PrivMsgTarget !$= "")
      {
         commandToServer('PrivateMessageSent', $PrivMsgTarget, %text);
         $PrivMsgTarget = "";
         %this.setValue( "" );
      }
      else
      {
         if(MessageHud.isTeamMsg)
            commandToServer('TeamMessageSent', %text);
         else if(MessageHud.isFireTeamMsg)
            commandToServer('FireTeamMessageSent', %text);
         else
            commandToServer('MessageSent', %text);
      }
   }

   MessageHud.close();
}

   
//----------------------------------------------------------------------------
// MessageHud key handlers

function toggleMessageHud(%make)
{
   if(%make)
   {
      MessageHud.isTeamMsg = false;
      MessageHud.isFireTeamMsg = false;
      MessageHud.toggleState();
   }
}

function teamMessageHud(%make)
{
   if(%make)
   {
      MessageHud.isTeamMsg = true;
      MessageHud.isFireTeamMsg = false;
      MessageHud.toggleState();
   }
}

function fireTeamMessageHud(%make)
{
   if(%make)
   {
      MessageHud.isTeamMsg = false;
      MessageHud.isFireTeamMsg = true;
      MessageHud.toggleState();
   }
}
