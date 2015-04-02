//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// RPE_DatablockEditor.buildInterface();
function buildParamDropdown( %pData ) {  
   %pData.pill-->field.text = %pData.Title;
   %pData.pill-->field.internalName = "";

   if(%pData.Variable !$= "")
   eval("%value = "@%pData.Variable@";");

   //Dropdown ctrl updare
   %menu = %pData.pill-->dropdown;
   %menu.command = %pData.Command;
   %menu.altCommand = %pData.AltCommand;
   %menu.internalName = %pData.InternalName;
   %menu.variable = %pData.Variable;
   %menu.canSaveDynamicFields = true;




   //Update dropdown data
				%menu.clear();
				%selectedId = 0;
				%menuData =  %pData.Option[%pData.Setting,"menuData"];
				%defaultData =  %pData.Option[%pData.Setting,"default"];			
				if (%menuData!$="") {
					%updType = getWord(%menuData,0);
					%updValue = getWords(%menuData,1);
					%menu.guiGroup = %updValue;
					if (%updType $="group") {
						%menuId = 1;
						foreach(%obj in %updValue) {
							%menu.add(%obj.getName(),%menuId);
							%menuId++;
						}
					} else if (%updType $="list") {
						eval("%datalist = $"@%updValue@";");
						%menuId = 1;
						foreach$(%obj in %datalist) {

							%menu.add(%obj.getName(),%menuId);
							%menuId++;
						}
					}
					else if (%updType $="strlist") {
						eval("%datalist = $"@%updValue@";");
						%menuId = 0;
						foreach$(%obj in %datalist) {
							%menu.add(%obj,%menuId);							
							if (%obj $= %defaultData)
							   %selectedId = %menuId;
							%menuId++;
						}
					}
				}
		
            %menu.setSelected(%selectedId,false);
}
//------------------------------------------------------------------------------
