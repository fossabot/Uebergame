//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
/*
%cfgArray.setVal("FIELD",       "DEFAULT" TAB "TEXT" TAB "TextEdit" TAB "" TAB "");
*/
//==============================================================================

function TerrainEditorPlugin::initDefaultCfg( %this,%cfgArray )
{  
     %gid = 1;
   %cfgArray.group[%gid] = "Action values";  
   %cfgArray.setVal("maxBrushSize",       "40 40" TAB "maxBrushSize" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
   %cfgArray.setVal("brushSize",       "2" TAB "brushSize" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setBrushSize(**);" TAB %gid);
   %cfgArray.setVal("brushType",       "box" TAB "brushType" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setBrushType(**);" TAB %gid);
   %cfgArray.setVal("brushPressure",       "1" TAB "brushPressure" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setBrushPressure(**);" TAB %gid);
   %cfgArray.setVal("brushSoftness",       "1" TAB "brushSoftness" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setBrushSoftness(**);" TAB %gid);  
  
  %gid = 2;
   %cfgArray.group[%gid] = "Action values";  
   %cfgArray.setVal("adjustHeightVal",       "10" TAB "adjustHeightVal" TAB "TextEdit" TAB "" TAB "ETerrainEditor"  TAB %gid);
   %cfgArray.setVal("setHeightVal",       "100" TAB "setHeightVal" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
   %cfgArray.setVal("scaleVal",       "1" TAB "scaleVal" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
   %cfgArray.setVal("smoothFactor",       "0.1" TAB "smoothFactor" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
   %cfgArray.setVal("noiseFactor",       "1.0" TAB "noiseFactor" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
   %cfgArray.setVal("softSelectRadius",       "50" TAB "softSelectRadius" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
   %cfgArray.setVal("softSelectFilter",       "1.000000 0.833333 0.666667 0.500000 0.333333 0.166667 0.000000" TAB "softSelectFilter" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
   %cfgArray.setVal("softSelectDefaultFilter",       "1.000000 0.833333 0.666667 0.500000 0.333333 0.166667 0.000000" TAB "softSelectDefaultFilter" TAB "TextEdit" TAB "" TAB "ETerrainEditor" TAB %gid);
   %cfgArray.setVal("slopeMinAngle",       "0" TAB "slopeMinAngle" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setSlopeLimitMinAngle(**);" TAB %gid);
   %cfgArray.setVal("slopeMaxAngle",       "90" TAB "slopeMaxAngle" TAB "TextEdit" TAB "" TAB "ETerrainEditor.setSlopeLimitMaxAngle(**);" TAB %gid);
    

}

