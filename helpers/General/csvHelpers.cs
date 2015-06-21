//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//-----------------------------------------------------------------------------
// Load up our main GUI which lets us see the game.

//============================================================================
//Taking screenshot
function convertCsvToObject(%csvFile,%delimiter,%quote,%targetGroup,%objPrefix,%nameFieldId,%skipExisting) {
	devLog("convertCsvToObject(%csvFile,%delimiter,%quote,%targetGroup,%objPrefix,%nameFieldId,%skipExisting)",%csvFile,%delimiter,%quote,%targetGroup,%objPrefix,%nameFieldId,%skipExisting);
	
		
	%firstLine = true;
	%fileObj = getFileReadObj(%csvFile);
	if (!isObject(%fileObj)) return;
   while( !%fileObj.isEOF() ) {
      %line = %fileObj.readline();   
      
      if (%line $= "")
      	continue;
      %dataIsHeader = false;	
		if (!%headerStored && %firstLine){
			%dataIsHeader = true;
			%firstLine = false;	
			%headerStored = true;		
		}
		else if (!%headerStored){
			warnLog("No header found to set field names! CSV Conversion aborted!");
			break;
		}			
		
		%lineFields = strreplace(%line,%delimiter,"\t");
		%fieldCount = getFieldCount(%lineFields);
		for(%i = 0;%i< %fieldCount;%i++){
			%field = getField(%lineFields,%i);
			
			if (%dataIsHeader){
				%fieldName[%i] = %field;						
				continue;
			}
			
			%fieldValue[%i] = %field;			
			if (%nameFieldId $= %i){
				%objName = %field;				
			}				
		}
		if (%dataIsHeader)
			continue;
			
		%sObj = "";
		//Now create the object
		%intName = validateObjectName(%objPrefix@%objName);
		devLog("Creating Car data with line:",%line,"Name",%intName);
		
		%checkData = %targetGroup.findObjectByInternalName(%intName,true);
		if (isObject(%checkData)){
			if (%skipExisting)
				continue;
				
			warnLog("Overwriting existing car data:",%intName);
			%sObj = %checkData;			
		}		
		else{
			%intName = getUniqueInternalName(%intName,%targetGroup,true);
			%newName = getUniqueName("CarData_");	
			%sObj = newScriptObject(%newName,%targetGroup);
			%sObj.internalName =	%intName ;
		}			
		
		
		for(%i = 0;%i< %fieldCount;%i++){	
			eval("%sObj."@	%fieldName[%i] @" = \""@ %fieldValue[%i] @ "\";");
			//%sObj.setFieldValue(%fieldName[%i],%fieldValue[%i]);					
		}
		
   }	
}
//----------------------------------------------------------------------------
//============================================================================
//Taking screenshot
function convertCsvToObjectHeaders(%csvFile,%delimiter,%quote,%targetGroup,%objPrefix,%nameFieldId,%headerFields,%updateMode) {
	devLog("convertCsvToObject(%csvFile,%delimiter,%quote,%targetGroup,%objPrefix,%nameFieldId,%headerFields,%updateMode)",%csvFile,%delimiter,%quote,%targetGroup,%objPrefix,%nameFieldId,%updateMode);
	if (%headerFields !$= ""){
		%fieldCount = getFieldCount(%headerFields);
		for(%i = 0;%i< %fieldCount;%i++){
			%field = getField(%headerFields,%i);
			%fieldName[%i] = %field;
		}
		%headerStored = true;
	}
		
	%firstLine = true;
	%fileObj = getFileReadObj(%csvFile);
	if (!isObject(%fileObj)) return;
   while( !%fileObj.isEOF() ) {
      %line = %fileObj.readline();   
      
      if (%line $= "")
      	continue;
      %dataIsHeader = false;	
		if (!%headerStored && %firstLine){
			%dataIsHeader = true;
			%firstLine = false;	
			%headerStored = true;		
		}
		else if (!%headerStored){
			warnLog("No header found to set field names! CSV Conversion aborted!");
			break;
		}	
		else{
			%name = getUniqueName(%objPrefix);
			%sObj = newScriptObject(%name,%targetGroup);
		}	
		
		devLog("Reading line:",%line);
		%lineFields = strreplace(%line,%delimiter,"\t");
		%fieldCount = getFieldCount(%lineFields);
		for(%i = 0;%i< %fieldCount;%i++){
			%field = getField(%lineFields,%i);
			
			if (%dataIsHeader){
				%fieldName[%i] = %field;
				continue;
			}
			
			%sObj.setFieldValue(%fieldName[%i],%field);
			if (%nameFieldId $= %i){
				%newName = validateObjectName(%objPrefix@%field);
				%newName = getUniqueName(%newName);
				%sObj.setName(%newName);
			}
				
		}
   }
		
		if (isObject(%targetGroup))
			%targetGroup.outputLog();
			
	
}
//----------------------------------------------------------------------------

//============================================================================
//Taking screenshot

//----------------------------------------------------------------------------
//Load the helpers on execution
