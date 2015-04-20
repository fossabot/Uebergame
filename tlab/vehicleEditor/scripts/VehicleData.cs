//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
/*
%cfgArray.setVal("FIELD",       "DEFAULT" TAB "TEXT" TAB "TextEdit" TAB "" TAB "");
*/
//==============================================================================
//VehicleEditorPlugin.getVehicleDatablocks();

function VehicleEditorPlugin::getVehicleDatablocks( %this,%type )
{ 
   doGuiGroupAction("WheeledDataMenu","clear()"); 
    doGuiGroupAction("WheeledTireDataMenu","clear()"); 
     doGuiGroupAction("WheeledSpringDataMenu","clear()"); 
   LabVE_CarDataMenu.clear();
   VehicleEditorDataSet.clear();
   foreach(%data in DatablockSet){
      %class = %data.getClassName();
      %keep = false;
      foreach$(%vClass in $VehicleEditor::Classes){
        
        
         if (%vClass@"Data" $= %class)
            %keep = true;
      }
      if (!%keep) continue;    
         
      if (%selectedId $= "" || $VehicleEditor::CarData $= %data.getName())
         %selectedId = %data.getId();
         
      if (%type !$= ""){
         %checkType = strstr( %class , %type);  
         if (%checkType $="-1")
            continue;
      }
      doGuiGroupAction("WheeledDataMenu","add(\""@%data.getName()@"\",\""@%data.getId()@"\")"); 
      doGuiGroupAction("WheeledTireDataMenu","add(\""@%data.getName()@"\",\""@%data.getId()@"\")"); 
      doGuiGroupAction("WheeledSpringDataMenu","add(\""@%data.getName()@"\",\""@%data.getId()@"\")"); 
      //LabVE_CarDataMenu.add(%data.getName(),%data.getId());
      
      VehicleEditorDataSet.add(%data);
      
   }
   doGuiGroupAction("WheeledDataMenu","setSelected(\""@%selectedId@"\")"); 
  // LabVE_CarDataMenu.setSelected(%selectedId);
   LabVE_CarDataMenu.selectData();
   devLog(VehicleEditorDataSet.getCount(),"vehicles datablocks added");
 //  VehicleEdShapeTreeView.clear();
  // VehicleEdShapeTreeView.open(VehicleEditorDataSet);
  // VehicleEdShapeTreeView.buildVisibleTree(true);
}
//==============================================================================
//VehicleEditorPlugin.getVehicleDatablocks();
function VehicleEditorPlugin::selectDatablock( %this,%datablock )
{ 
   VehicleEdShapeView.renderMounts = true;
  VehicleEdInspector.inspect( %datablock );
  %dataName = %datablock.getClassName();
  %strlen = strlen(%dataName);
  %class = getSubStr(%dataName,0,%strlen-4);

   delObj(%this.shape);  
   delObj(%this.lastShape);   
   %obj = spawnObject(%class, %datablock, "VehicleEdObject", "", "");   
 %this.lastShape = %obj;
  
  %path = VehicleEditor.getObjectShapeFile( %obj );
    if ( %path !$= "" )
        VehicleEdSelectWindow.onSelect( %path );

    // Set the object type (for required nodes and sequences display)
    %objClass = %obj.getClassName();
    %hintId = -1;

    %count = VehicleHintGroup.getCount();
    for ( %i = 0; %i < %count; %i++ ) {
        %hint = VehicleHintGroup.getObject( %i );
        if ( %objClass $= %hint.objectType ) {
            %hintId = %hint;
            break;
        } else if ( isMemberOfClass( %objClass, %hint.objectType ) ) {
            %hintId = %hint;
        }
    }
    VehicleEdHintMenu.setSelected( %hintId );
    
    $VehicleEditor::WheeledVehicleData = %datablock;
    %paramArray = $VehicleEditorParams[%dataName];
    if (isObject(%paramArray))
      syncParamArray(%paramArray);
   else
      devLog("Couldn't find params for datablock class:",%dataName);
      
    %this.mountWheels(%obj);
}

//==============================================================================
//VehicleEditorPlugin.getVehicleDatablocks();
function LabVE_CarDataMenu::selectData( %this ){ 
   loga("LabVE_CarDataMenu::selectData( %this )",%this);
 
   %id = %this.getSelected();
   %name = %this.getText();
   devLog("ID=",%id,"Name",%name);
   
   VehicleEditorPlugin.selectDatablock(%name);
}


function VehicleEditorPlugin::saveChanges( %this,%obj ){  
   devLog(" VehicleEditorPlugin::saveChanges( %this,%obj )",%this,%obj);     
   %obj.reloadOnLocalClient();  
}