//==============================================================================
// Lab Editor -> Scene Editor Plugin
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
function EScript::saveAndReloadFile( %this ) {   
   %pageId = EScriptBook.getSelectedPage();
   %page = EScriptBook.getObject(%pageId);
   %file = %page.file;
   %textCtrl = %page-->textArea;
   devLog("Want to save file:",%file);
   %this.saveFile(%textCtrl,%file);
   
}
//------------------------------------------------------------------------------

//==============================================================================
function EScript::saveFile( %this,%textCtrl,%file ) {  
   %file = "tlab/scriptEditor/saves/"@%file; 
    // Keep reading until we reach the end of the file
   %text = %textCtrl.getText();
  
   %text = strreplace(%text,"<br>","\n");    
 
  %fileWrite = new FileObject();
   %fileWrite.openForWrite(%file);     
   %fieldCount = getFieldCount(%text); 
   for(;%i<%fieldCount;%i++){
      
      %line = getField(%text,%i);     
      %line = strreplace(%line,"^","   "); 
      %line = stripMLControlChars(%line);    
      %fileWrite.writeLine(%line);
   }
   %fileWrite.close();
   %fileWrite.delete();
}
//------------------------------------------------------------------------------


//==============================================================================
function EScriptTree::onSelectPath( %this,%path ) {  
   devLog("EScriptTree::onSelectPath( %this,%path )", %item,%path);
   %this.openPath(%path);
}
//------------------------------------------------------------------------------
//==============================================================================
function EScriptTree::openPath( %this,%path ) {  
   devLog("EScriptTree::openPath( %this,%path )", %item,%path);
   EScriptPageSample.visible = 0;
   %page = EScriptPageSample.deepClone();
   %page.visible = 1;
   %page.text = filebase(%path);
   EScriptBook.add(%page);
   %page.file = %path;
   
   %textCtrl = %page-->textArea;
   
   %fileRead = new FileObject();
   %fileRead.openForRead(%path);
   // Keep reading until we reach the end of the file
   while( !%fileRead.isEOF() )
   {
      %line = %fileRead.readline();
      %textCtrl.addText(%line @ "<br>");
      echo(%line);
   }
   %fileRead.close();
   %fileRead.delete();
}
//------------------------------------------------------------------------------

//==============================================================================
function EScriptTree::onInspect( %this,%item,%arg ) {  
   devLog("EScriptTree::onInspect( %this,%item,%arg )", %item,%arg);
   
}
//------------------------------------------------------------------------------
//==============================================================================
function EScriptTree::onKeyDown( %this,%item,%arg ) {  
   devLog("EScriptTree::onKeyDown( %this,%item,%arg )", %item,%arg);
   
}
//------------------------------------------------------------------------------
//==============================================================================
function EScriptTree::onRightMouseDown( %this,%item,%mousePos,%obj ) {  
   devLog("EScriptTree::onRightMouseDown( %this,%item,%mousePos,%obj)", %item,%mousePos,%obj);
   
}
//------------------------------------------------------------------------------

//==============================================================================
function EScriptBook::onTabRightClick( %this,%text,%index ) {  
   devLog("EScriptBook::onTabRightClick( %this,%text,%index )",%text,%index );

    %page = EScriptBook.getObject(%index); 
    if (%page.getName() $= "EScriptPageSample")
      return;
    delObj(%page);
   EScriptBook.selectPage(%index-1);
}
//------------------------------------------------------------------------------
