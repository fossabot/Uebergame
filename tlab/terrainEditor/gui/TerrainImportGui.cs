//==============================================================================
// Lab Editor -> Procedural Terrain Painter GUI script
// Copyright NordikLab Studio, 2014
//==============================================================================

$Lab::TerrainPainter::UseTerrainHeight = "1";
$Lab::TerrainPainter::ValidateHeight = "1";
$TPPHeightMin = -10000;  
$TPPHeightMax = 10000;  
$TPPSlopeMin = 0;  
$TPPSlopeMax = 90;  

//==============================================================================
// Multi material automatic terrain painter
//==============================================================================
//==============================================================================
function TerrainPainterGui::init(%this) 
{ 
   if ( !isObject( TPG_LayerGroup ) ) {
		new SimGroup( TPG_LayerGroup  );	
	} 
} 
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui::onWake( %this )
{  
   if ( %this.namesArray.getClassName !$= "ArrayObject")
      %this.namesArray = "";
   
   if ( %this.channelsArray.getClassName !$= "ArrayObject")
      %this.channelsArray = "";
      
   if ( !isObject( %this.namesArray ) )
      TerrainImportGui.namesArray = new ArrayObject();

   if ( !isObject( %this.channelsArray ) )
      TerrainImportGui.channelsArray = new ArrayObject();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui::import( %this )
{
   // Gather all the import settings.
   
   %heightMapPng = %this-->HeightfieldFilename.getText();
     
   %metersPerPixel = %this-->MetersPerPixel.getText();
   %heightScale = %this-->HeightScale.getText();
   
   %flipYAxis = %this-->FlipYAxis.isStateOn();
   
   // Grab and validate terrain object name.
   
   %terrainName = %this-->TerrainName.getText();
   if( !( isObject( %terrainName ) && %terrainName.isMemberOfClass( "TerrainBlock" ) ) &&
       !Editor::validateObjectName( %terrainName ) )
      return;
   
   %opacityNames = "";
   %materialNames = "";
   
   %opacityList = %this-->OpacityLayerTextList;
   
   for( %i = 0; %i < %opacityList.rowCount(); %i++ )
   {
      %itemText = %opacityList.getRowTextById( %i );
      %opacityName = %this.namesArray.getValue( %i );
     
      %channelInfo = %this.channelsArray.getValue( %i );
      %channel = getWord( %channelInfo, 0 );
      
      %materialName = getField( %itemText, 2 );
      if (%materialName $= "") continue;

      %opacityNames = %opacityNames @ %opacityName TAB %channel @ "\n";
      %materialNames = %materialNames @ %materialName @ "\n";
      
   }

   %updated = nameToID( %terrainName );

   // This will update an existing terrain with the name %terrainName,
   // or create a new one if %terrainName isn't a TerrainBlock
   %obj = TerrainBlock::import(   %terrainName, 
                                  %heightMapPng, 
                                  %metersPerPixel, 
                                  %heightScale, 
                                  %opacityNames, 
                                  %materialNames,
                                  %flipYAxis );
                                  
      %obj.terrainHeight = %heightScale;

   //Canvas.popDialog( %this );

   if ( isObject( %obj ) )
   {
      if( %obj != %updated )
      {
         // created a new TerrainBlock
         // Submit an undo action. 
         MECreateUndoAction::submit(%obj);
      }

      assert( isObject( EWorldEditor ), 
         "ObjectBuilderGui::processNewObject - EWorldEditor is missing!" );

      // Select it in the editor.
      EWorldEditor.clearSelection();
      EWorldEditor.selectObject(%obj);

      // When we drop the selection don't store undo
      // state for it... the creation deals with it.
      EWorldEditor.dropSelection( true );

      ETerrainEditor.isDirty = true;
      EPainter.updateLayers();
   }
   else
   {
      MessageBox( "Import Terrain", 
         "Terrain import failed! Check console for error messages.", 
         "Ok", "Error" );
   }
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui::doOpenDialog( %this, %filter, %callback,%subFolder )
{
   %currentFile = $EventObj.file;
   %dlg = new OpenFileDialog()
   {
      Filters = %filter;
      DefaultFile = %currentFile;
      ChangePath = false;
      MustExist = true;
      MultipleFiles = false;
   };   
   
    %terrainPath = filePath( %currentFile )@"/terrain";
    if (%subFolder !$="")
      %terrainPath = %terrainPath @"/"@%subFolder;
    
   if(filePath( %currentFile ) $= "")
      %dlg.DefaultPath = getMainDotCSDir();   
   else if(%terrainPath !$= "")
      %dlg.DefaultPath = %terrainPath;
   else
      %dlg.DefaultPath = filePath(%currentFile); 
      
   if(%dlg.Execute())
      eval(%callback @ "(\"" @ %dlg.FileName @ "\");");
   
   
   %dlg.delete();
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui_SetHeightfield( %name )
{
   TerrainImportGui-->HeightfieldFilename.setText( %name );
}
//------------------------------------------------------------------------------
//==============================================================================
$TerrainImportGui::HeightFieldFilter = "Heightfield Files (*.png, *.bmp, *.jpg, *.gif)|*.png;*.bmp;*.jpg;*.gif|All Files (*.*)|*.*|";
$TerrainImportGui::OpacityMapFilter = "Opacity Map Files (*.png, *.bmp, *.jpg, *.gif)|*.png;*.bmp;*.jpg;*.gif|All Files (*.*)|*.*|";
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui::browseForHeightfield( %this )
{
   %this.doOpenDialog( $TerrainImportGui::HeightFieldFilter, "TerrainImportGui_SetHeightfield", "heightmap");
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGuiAddOpacityMap( %name,%red,%green,%blue )
{
   // TODO: Need to actually look at
   // the file here and figure
   // out how many channels it has.

   %txt = makeRelativePath( %name, getWorkingDirectory() );   
   
   // Will need to do this stuff
   // once per channel in the file
   // currently it works with just grayscale.   
   %channelsTxt = "R" TAB "G" TAB "B" TAB "A";
   %bitmapInfo = getBitmapinfo( %name );
   
   %channelCount = getWord( %bitmapInfo, 2 );   
   
   %opacityList = TerrainImportGui-->OpacityLayerTextList;
   for ( %i = 0; %i < %channelCount; %i++ )
   {
      TerrainImportGui.namesArray.push_back( %txt, %name );
      TerrainImportGui.channelsArray.push_back( %txt, getWord( %channelsTxt, %i ) TAB %channelCount );

      //TerrainImportGui.namesArray.echo(); 
      %channel = getWord( %channelsTxt, %i );
      %addText = %channel;
      if (%channel $= "R" && %red !$="")
         %addText = %addText TAB %red;
      else if (%channel $= "G" && %green !$="")
         %addText = %addText TAB %green;
      else if (%channel $= "B" && %blue !$="")
         %addText = %addText TAB %blue;
      %count = %opacityList.rowCount();
      %opacityList.addRow( %count, %txt TAB %addText );
   }
   
   //OpacityMapListBox.addItem( %name );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui::browseForOpacityMap( %this )
{
   TerrainImportGui.doOpenDialog( $TerrainImportGui::OpacityMapFilter, "TerrainImportGuiAddOpacityMap","overlays" );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui::removeOpacitymap( %this )
{   
   %opacityList = %this-->OpacityLayerTextList;
   
   //%itemIdx = OpacityMapListBox.getSelectedItem();   
   %itemIdx = %opacityList.getSelectedId();
   if ( %itemIdx < 0 ) // -1 is no item selected
      return;
  
   %this.namesArray.erase( %itemIdx );  
   %this.channelsArray.erase( %itemIdx );
  
   //%this.namesArray.echo();  
  
   %opacityList.removeRowById( %itemIdx );
      
   //OpacityMapListBox.deleteItem( %itemIdx );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui::onOpacityListDblClick( %this )
{
   %opacityList = %this-->OpacityLayerTextList;
   
   //echo( "Double clicked the opacity list control!" );
   %itemIdx = %opacityList.getSelectedId();
   if ( %itemIdx < 0 )
      return;

   %this.activeIdx = %itemIdx;
   
   %rowTxt = %opacityList.getRowTextById( %itemIdx );
   %matTxt = getField( %rowTxt, 2 );
   %matId = getField( %rowTxt, 3 );
         
   TerrainMaterialDlg.showByObjectId( %matId, TerrainImportGui_TerrainMaterialApplyCallback );
}
//------------------------------------------------------------------------------
//==============================================================================
function TerrainImportGui_TerrainMaterialApplyCallback( %mat, %matIndex, %activeIdx )
{           
   // Skip over a bad selection.
   if ( !isObject( %mat ) )   
      return;
                        
   %opacityList = TerrainImportGui-->OpacityLayerTextList;
                           
   %itemIdx = TerrainImportGui.activeIdx;
   if (%activeIdx!$="")
      %itemIdx = %activeIdx;
   
   if ( %itemIdx < 0 || %itemIdx $= "" )
      return;

   %rowTxt = %opacityList.getRowTextById( %itemIdx );
   
   %columntTxtCount = getFieldCount( %rowTxt );
   if ( %columntTxtCount > 2 )
      %rowTxt = getFields( %rowTxt, 0, 1 );

   %opacityList.setRowById( %itemIdx, %rowTxt TAB %mat.internalName TAB %mat );
}
//------------------------------------------------------------------------------
