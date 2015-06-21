//==============================================================================
// Boost! -> GuiControl Functions Helpers
// Copyright NordikLab Studio, 2013
//==============================================================================
//==============================================================================
// Schedule global on-off - Used to limit output of fast logs
//==============================================================================


//==============================================================================
// WORLD / SCREEN POSITIONING HELPERS
//==============================================================================
//==============================================================================
// Get the Screen coords for a 3D position
// fixDuplicatedFieldsInFile(DVD_Datablock.getFilename());
function fixDuplicatedFieldsInFile(%file,%format) {
	 %fileObj = getFileReadObj(%file);
	 
	 if (%format)
	   warnLog("Auto formatting is disabled because it's not stable yet!");
   %format = false;
   
   if (!isObject(%fileObj)) return;
   // Set the line count output to 0
   %i = 0;
   //Go through each line in the file
   while( !%fileObj.isEOF() ) {
      %line = %fileObj.readline();
      %trimLine = trim(%line);
      %skipLine = false;
      //Find a new datablock declaration  
      %firstWord = firstWord(%trimLine);
      if (  %firstWord $= "datablock" || %firstWord $= "new" || %firstWord $= "singleton"){
         //We have a datablock declaration line, get the name
         %fixLine = strreplace(%trimLine,"(","\t");
         %fixLine = strreplace(%fixLine,")","\t");
         %decPart = getField(%fixLine,1);
         %fixDec = strreplace(%decPart,":","\t");
         %dataName = getField(%fixDec,0);
         %parent = getField(%fixDec,1);
         devLog("FixLine:",%fixLine,"DecPart=",%decPart,"FixDec=",%fixDec);
         devLog("Data declaration found with name:",%dataName,"Parent:",%parent);
         %currentData = %dataName;
   }
      else if (strstr(%trimLine,"};") !$="-1"){
         //Current data end
         devLog("End of data:",%currentData);
         %currentData = "";
        
      }
      else if (%currentData !$=""){
         %fixLine = strReplaceList(%trimLine,"{\n\"\n;");
         %fixLine = strreplace(%fixLine,"=","\t");
         %field = trim(getField(%fixLine,0));
         %value = trim(getField(%fixLine,1));
         devLog("Check for field FixLine =",%fixLine,"Field=",%field,"Value=",%value);
         %testValue = "";
         %testValue = %dataField[%currentData,%field];
         if ( %testValue !$= ""){
            devLog("DUPLICATED FIELD:",%field,"CurrentVal=",%testValue,"NewVal=",%value);
            %skipLine = true;
            %removed = strAddWord(%removed,%removedId++@"- "@%field@">>"@%value);
         }
         %dataField[%currentData,%field] = %value;
      }
      if (%format){
         if (%field !$="" && %value !$="")
         %line = "   "@%field@" = \""@%value@"\";";
      }
      if (!%skipLine)
         %output[%i++]= %line;
         
   }
   
   closeFileObj(%fileObj);
   
   %fileObj = getFileWriteObj(%file);
   while(%j++ <= %i){
      devLog("WriteLine:",%output[%j]);
      %fileObj.writeLine(%output[%j]);
   }
   closeFileObj(%fileObj);
   
   devLog("Removed list=",%removed);
   return %removed;
}
//------------------------------------------------------------------------------


