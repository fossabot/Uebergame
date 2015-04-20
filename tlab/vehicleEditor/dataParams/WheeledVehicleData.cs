//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


function VehicleEditorPlugin::initWheeledVehicleDataParams( %this )
{     
   %arCfg = createParamsArray("labVEParamData","WheeledVehicleData",labVE_WheeledVehicleParams);

   
   %arCfg.group[%gid++] = "General settings";   

   %arCfg.setVal("engineTorque",       "" TAB "engineTorque" TAB "TextEdit" TAB "" TAB "$VehicleEditor::WheeledVehicleData" TAB %gid);
  
   
   %arCfg.group[%gid++] = "Grid settings";
   %arCfg.setVal("maxWheelSpeed",       "1" TAB "maxWheelSpeed" TAB "TextEdit" TAB "" TAB "$VehicleEditor::WheeledVehicleData"  TAB %gid);
 
   buildParamsArray(%arCfg);
   
   $VehicleEditorParams["WheeledVehicleData"] = %arCfg;
   
   %arCfg.postUpdateFunc= "VehicleEditorPlugin.saveChanges(%obj);";  
  
}
