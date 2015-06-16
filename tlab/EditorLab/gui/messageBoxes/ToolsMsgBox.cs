//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------


// Cleanup Dialog created by 'core'
if( isObject( ToolsMsgPopupDlg ) )
	ToolsMsgPopupDlg.delete();

if( isObject( ToolsMsgBoxYesNoDlg ) )
	ToolsMsgBoxYesNoDlg.delete();

if( isObject( ToolsMsgBoxYesNoCancelDlg ) )
	ToolsMsgBoxYesNoCancelDlg.delete();

if( isObject( ToolsMsgBoxOKCancelDetailsDlg ) )
	ToolsMsgBoxOKCancelDetailsDlg.delete();

if( isObject( ToolsMsgBoxOKCancelDlg ) )
	ToolsMsgBoxOKCancelDlg.delete();

if( isObject( ToolsMsgBoxOKDlg ) )
	ToolsMsgBoxOKDlg.delete();

if( isObject( ToolsDropdownDlg ) )
	ToolsDropdownDlg.delete();


// Load Editor Dialogs
exec("./ToolsMsgBoxOk.gui");
exec("./ToolsMsgBoxYesNo.gui");
exec("./ToolsMsgBoxYesNoCancel.gui");
exec("./ToolsMsgBoxOKCancel.gui");
exec("./ToolsMsgBoxOKCancelDetailsDlg.gui");
exec("./ToolsMsgPopup.gui");
exec("./ToolsDropdownDlg.gui");



// --------------------------------------------------------------------
// Message Sound
// --------------------------------------------------------------------
/*new SFXDescription(ToolsMsgBoxAudioDescription)
{
   volume      = 1.0;
   isLooping   = false;
   is3D        = false;
   channel     = $GuiAudioType;
};

new SFXProfile(ToolsMsgBoxBeep)
{
   filename    = "./ToolsMsgBoxSound";
   description = ToolsMsgBoxAudioDescription;
   preload     = true;
};*/




//---------------------------------------------------------------------------------------------
// messageCallback
// Calls a callback passed to a message box.
//---------------------------------------------------------------------------------------------
function ToolsMsgCallback(%dlg, %callback) {
	Canvas.popDialog(%dlg);
	eval(%callback);
}

//The # in the function passed replaced with the output
//of the preset menu.
function ToolsIOCallback(%dlg, %callback) {
	%id = TIODropdownMenu.getSelected();
	%text = TIODropdownMenu.getTextById(%id);
	%callback = strreplace(%callback, "#", %text);
	eval(%callback);
	Canvas.popDialog(%dlg);
}

//---------------------------------------------------------------------------------------------
// TMBSetText
// Sets the text of a message box and resizes it to accomodate the new string.
//---------------------------------------------------------------------------------------------
function TMBSetText(%text, %frame, %msg) {
	// Get the extent of the text box.
	%ext = %text.getExtent();
	// Set the text in the center of the text box.
	%text.setText("<just:center>" @ %msg);
	// Force the textbox to resize itself vertically.
	%text.forceReflow();
	// Grab the new extent of the text box.
	%newExtent = %text.getExtent();
	// Get the vertical change in extent.
	%deltaY = getWord(%newExtent, 1) - getWord(%ext, 1);
	// Resize the window housing the text box.
	%windowPos = %frame.getPosition();
	%windowExt = %frame.getExtent();
	%frame.resize(getWord(%windowPos, 0), getWord(%windowPos, 1) - (%deltaY / 2),
					  getWord(%windowExt, 0), getWord(%windowExt, 1) + %deltaY);
	%frame.canMove = "0";
	//%frame.canClose = "0";
	%frame.resizeWidth = "0";
	%frame.resizeHeight = "0";
	%frame.canMinimize = "0";
	%frame.canMaximize = "0";
	//sfxPlayOnce( ToolsMsgBoxBeep );
}

//---------------------------------------------------------------------------------------------
// Various message box display functions. Each one takes a window title, a message, and a
// callback for each button.
//---------------------------------------------------------------------------------------------
/*
function LabMsgOK(%title, %message, %callback) {
	TMBOKFrame.text = %title;
	Canvas.pushDialog(ToolsMsgBoxOKDlg);
	TMBSetText(TMBOKText, TMBOKFrame, %message);
	ToolsMsgBoxOKDlg.callback = %callback;
}

function ToolsMsgBoxOKDlg::onSleep( %this ) {
	%this.callback = "";
}
*/
/*
function LabMsgOkCancel(%title, %message, %callback, %cancelCallback) {
	TMBOKCancelFrame.text = %title;
	Canvas.pushDialog(ToolsMsgBoxOKCancelDlg);
	TMBSetText(TMBOKCancelText, TMBOKCancelFrame, %message);
	ToolsMsgBoxOKCancelDlg.callback = %callback;
	ToolsMsgBoxOKCancelDlg.cancelCallback = %cancelCallback;
}

function ToolsMsgBoxOKCancelDlg::onSleep( %this ) {
	%this.callback = "";
}

function ToolsMsgBoxOKCancelDetails(%title, %message, %details, %callback, %cancelCallback) {
	if(%details $= "") {
		TMBOKCancelDetailsButton.setVisible(false);
	}

	TMBOKCancelDetailsScroll.setVisible(false);

	TMBOKCancelDetailsFrame.setText( %title );

	Canvas.pushDialog(ToolsMsgBoxOKCancelDetailsDlg);
	TMBSetText(TMBOKCancelDetailsText, TMBOKCancelDetailsFrame, %message);
	TMBOKCancelDetailsInfoText.setText(%details);

	%textExtent = TMBOKCancelDetailsText.getExtent();
	%textExtentY = getWord(%textExtent, 1);
	%textPos = TMBOKCancelDetailsText.getPosition();
	%textPosY = getWord(%textPos, 1);

	%extentY = %textPosY + %textExtentY + 65;

	TMBOKCancelDetailsInfoText.setExtent(285, 128);

	TMBOKCancelDetailsFrame.setExtent(300, %extentY);

	ToolsMsgBoxOKCancelDetailsDlg.callback = %callback;
	ToolsMsgBoxOKCancelDetailsDlg.cancelCallback = %cancelCallback;

	TMBOKCancelDetailsFrame.defaultExtent = TMBOKCancelDetailsFrame.getExtent();
}

function TMBOKCancelDetailsToggleInfoFrame() {
	if(!TMBOKCancelDetailsScroll.isVisible()) {
		TMBOKCancelDetailsScroll.setVisible(true);
		TMBOKCancelDetailsText.forceReflow();
		%textExtent = TMBOKCancelDetailsText.getExtent();
		%textExtentY = getWord(%textExtent, 1);
		%textPos = TMBOKCancelDetailsText.getPosition();
		%textPosY = getWord(%textPos, 1);

		%verticalStretch = %textExtentY;

		if((%verticalStretch > 260) || (%verticalStretch < 0))
			%verticalStretch = 260;

		%extent = TMBOKCancelDetailsFrame.defaultExtent;
		%height = getWord(%extent, 1);

		%posY = %textPosY + %textExtentY + 10;
		%posX = getWord(TMBOKCancelDetailsScroll.getPosition(), 0);
		TMBOKCancelDetailsScroll.setPosition(%posX, %posY);
		TMBOKCancelDetailsScroll.setExtent(getWord(TMBOKCancelDetailsScroll.getExtent(), 0), %verticalStretch);
		TMBOKCancelDetailsFrame.setExtent(300, %height + %verticalStretch + 10);
	} else {
		%extent = TMBOKCancelDetailsFrame.defaultExtent;
		%width = getWord(%extent, 0);
		%height = getWord(%extent, 1);
		TMBOKCancelDetailsFrame.setExtent(%width, %height);
		TMBOKCancelDetailsScroll.setVisible(false);
	}
}

function ToolsMsgBoxOKCancelDetailsDlg::onSleep( %this ) {
	%this.callback = "";
}

function LabMsgYesNo(%title, %message, %yesCallback, %noCallback) {
	TMBYesNoFrame.text = %title;
	ToolsMsgBoxYesNoDlg.profile = "GuiOverlayProfile";
	Canvas.pushDialog(ToolsMsgBoxYesNoDlg);
	TMBSetText(TMBYesNoText, TMBYesNoFrame, %message);
	ToolsMsgBoxYesNoDlg.yesCallBack = %yesCallback;
	ToolsMsgBoxYesNoDlg.noCallback = %noCallBack;
}

function LabMsgYesNoCancel(%title, %message, %yesCallback, %noCallback, %cancelCallback) {
	TMBYesNoCancelFrame.text = %title;
	ToolsMsgBoxYesNoDlg.profile = "GuiOverlayProfile";
	Canvas.pushDialog(ToolsMsgBoxYesNoCancelDlg);
	TMBSetText(TMBYesNoCancelText, TMBYesNoCancelFrame, %message);
	ToolsMsgBoxYesNoCancelDlg.yesCallBack = %yesCallback;
	ToolsMsgBoxYesNoCancelDlg.noCallback = %noCallBack;
	ToolsMsgBoxYesNoCancelDlg.cancelCallback = %cancelCallback;
}

function ToolsMsgBoxYesNoDlg::onSleep( %this ) {
	%this.yesCallback = "";
	%this.noCallback = "";
}

//---------------------------------------------------------------------------------------------
// ToolsMsgPopup
// Displays a message box with no buttons. Disappears after %delay milliseconds.
//---------------------------------------------------------------------------------------------
function LabMsg(%title, %message, %delay) {
	// Currently two lines max.
	ToolsMsgPopFrame.setText(%title);
	Canvas.pushDialog(ToolsMsgPopupDlg);
	TMBSetText(ToolsMsgPopText, ToolsMsgPopFrame, %message);
	if (%delay !$= "")
		schedule(%delay, 0, CloseToolsMsgPopup);
}

//---------------------------------------------------------------------------------------------
// TIODropdown
// By passing in a simgroup or simset, the user will be able to choose a child of that group
// through a guiPopupMenuCtrl
//---------------------------------------------------------------------------------------------

function TIODropdown(%title, %message, %simgroup, %callback, %cancelCallback) {
	TIODropdownFrame.text = %title;
	Canvas.pushDialog(TIODropdownDlg);
	TMBSetText(TIODropdownText, TIODropdownFrame, %message);

	if(isObject(%simgroup)) {
		for(%i = 0; %i < %simgroup.getCount(); %i++)
			TIODropdownMenu.add(%simgroup.getObject(%i).getName());

	}

	TIODropdownMenu.sort();
	TIODropdownMenu.setFirstSelected(0);

	TIODropdownDlg.callback = %callback;
	TIODropdownDlg.cancelCallback = %cancelCallback;
}

function TIODropdownDlg::onSleep( %this ) {
	%this.callback = "";
	%this.cancelCallback = "";
	TIODropdownMenu.clear();
}

function CloseLabMsg() {
	Canvas.popDialog(ToolsMsgPopupDlg);
}
*/
//---------------------------------------------------------------------------------------------
// "Old" message box function aliases for backwards-compatibility.
//---------------------------------------------------------------------------------------------

function ToolsMsgBoxOKOld( %title, %message, %callback ) {
	LabMsgOK( %title, %message, %callback );
}
function ToolsMsgBoxOKCancelOld( %title, %message, %callback, %cancelCallback ) {
	LabMsgOkCancel( %title, %message, %callback, %cancelCallback );
}
function ToolsMsgBoxYesNoOld( %title, %message, %yesCallback, %noCallback ) {
	LabMsgYesNo( %title, %message, %yesCallback, %noCallback );
}
