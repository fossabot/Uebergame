//=============================================================================
// HelperLab -> Generate documentation extracted from files
// Copyright NordikLab Studio, 2013
//------------------------------------------------------------------------------
// The scripts will analyse the files in specified folder and will extracted the 
// informations that fit with the generator parameters
//==============================================================================
%header =   "//==============================================================================" NL
            "//Documentation generate with HelperLab doc generator" NL
            "//------------------------------------------------------------------------------";
$HL_DocGenerator_File_Header = %header;
$HL_DocGenerator_File_Extension = ".doc"; //If set to .cs, it will cause Torsion precompile to fail

$HL_DocGenerator_Prefix_Data = "//!";
$HL_DocGenerator_Prefix_List =  "//!" TAB "//+" TAB "//>";
$HL_DocGenerator_List["//!","Prepend"] = "<h2>";
$HL_DocGenerator_List["//!","Append"] = "</h2>";
$HL_DocGenerator_List["//+","Prepend"] = "<p>";
$HL_DocGenerator_List["//+","Append"] = "</p>";
$HL_DocGenerator_List["//>","Prepend"] = "<li>";
$HL_DocGenerator_List["//>","Append"] = "</li>";

$HL_DocGenerator_ListFirst["//>"] = "<ul>" TAB "Addline";
$HL_DocGenerator_ListLast["//>"] = "</ul>" TAB "Addline";


$HL_DocGenerator_Data_PreContent = "<hr><br>";
$HL_DocGenerator_Data_PostContent = "<br>";
$HL_DocGenerator_Data_RemovePrefix = true;
$HL_DocGenerator_Data_EmptyLineBetween = 1;

$HL_DocGenerator_OutputFolder_Mode = "ReplaceRoot";
$HL_DocGenerator_OutputFolder = "docs";
//==============================================================================
// Schedule global on-off - Used to limit output of fast logs
//==============================================================================

//==============================================================================
// Get the Screen coords for a 3D position
//generatePatternDocs("scripts/helpers/*.cs");
function generatePatternDocs(%filesPattern) {
	if (%filesPattern $= "")
		return;
   %filePathScript = "art/gui/*.prof.cs";
   for(%file = findFirstFile(%filesPattern); %file !$= ""; %file = findNextFile(%filesPattern)) {
      generateFileDocs(%file);
   }
	
}
//------------------------------------------------------------------------------
function genDoc() {
   generateFileDocs("scripts/helpers/GuiHelpers/guiControlClass.cs");
}
//==============================================================================
// Get the Screen coords for a 3D position
function generateFileDocs(%file) {
   %fileObj = getFileReadObj(%file);
   
   if (!isObject(%fileObj)) return;
   // Set the line count output to 0
   %i = 0;
   //Go through each line in the file
   while( !%fileObj.isEOF() ) {
      %line = %fileObj.readline();      
      //Now check if the line fit with one of the generator parameter
      //Trim the line to simply find the start chars
      %trimmedLine = trim(%line);
      %firstChars = getSubStr(%trimmedLine,0,3);
      
      %result = strFind($HL_DocGenerator_Prefix_List TAB $HL_DocGenerator_Prefix_Data,%firstChars);
     
      if (!%result)
         continue;
         
         //Check if it's the first data of this type and check if need to add something
      if (%currentType !$= %firstChars){
         //Check the previous data for append stuff
         %lastData = $HL_DocGenerator_ListLast[%currentType];
         devLog("First Data for type:",%currentType,"Is:",%lastData);
         if (%lastData !$= ""){
            %addStr = getField(%lastData,0);
            %addType = getField(%lastData,1);
              devLog("Last Data add line:",%addStr);
            if (%addStr !$= "" && (%addType $= "AddLine" || %addType $= ""))
               %line[%i++] = %addStr;
         }
         
          if (  %firstChars $= $HL_DocGenerator_Prefix_Data){   
            //This is a new data, make sure to terminate previous data
            if (%dataActive){            
               %line[%i++] = $HL_DocGenerator_Data_PostContent;   
                  for(%j = 1; %j <= $HL_DocGenerator_Data_EmptyLineBetween;%j++)
               %line[%i++] = "";
             }
         
            %dataActive = true;
         
            //We have a title, add the precontent to output lines         
             %preContent = $HL_DocGenerator_Data_PreContent;
            if (%preContent !$= "")
                %line[%i++] = %preContent;  
         
       }      
             
         //Now check the new data for prepend stuff
         %firstData = $HL_DocGenerator_ListFirst[%firstChars];
         devLog("First Data for type:",%firstChars,"Is:",%firstData);
         if (%firstData !$= ""){
            %addStr = getField(%firstData,0);
            %addType = getField(%firstData,1);
            devLog("First Data add line:",%addStr);
            if (%addStr !$= "" && (%addType $= "AddLine" || %addType $= ""))
               %line[%i++] = %addStr;
         }
      }
      %currentType = %firstChars;   
      
         
      
      if ($HL_DocGenerator_Data_RemovePrefix)
         %trimmedLine = strreplace(%trimmedLine,%firstChars,"");
      
    
    
      //Look for prepend string
      %prepend = $HL_DocGenerator_List[%firstChars,"Prepend"];
     %append = $HL_DocGenerator_List[%firstChars,"Append"];
      
      
             
      %line[%i++] = %prepend@%trimmedLine@%append;
      info("Adding line to doc:",%prepend@%trimmedLine@%append);
            
   }

   closeFileObj(%fileObj);
   
   //Now we need to add the output lines to a new file
   
   //For now only the replace root mode is available
   %outputFolder = $HL_DocGenerator_OutputFolder;
   if (%outputFolder $= ""){
      //No output folder found, the file will be output beside analysed file with .doc suffix
      %currentExt = fileExt(%file);
      
      //Prevent overiding the file if extension is the same, it will append .doc to the extension
      if (%currentExt $= $HL_DocGenerator_File_Extension)
         %currentExt = %currentExt@$HL_DocGenerator_File_Extension;
         
      %outputFile = strreplace(%file,%currentExt,".doc");
   }
   else {
      //We have an output folder, we will replace the root of file with it
      %folderFields = strreplace(%file,"/","\t");
      devLog("FolderFields Records = ",getRecordCount(%folderFields));
      devLog("FolderFields Fields = ",getFieldCount(%folderFields));
      %outputFile = setField(%folderFields,0,%outputFolder);
      %outputFile = strreplace(%outputFile,"\t","/");
   }
  
   devLog("Extracting doc from:",%file,"to:",%outputFile);
  
   if (%outputFile $= %file) return;
    %fileObj = getFileWriteObj(%outputFile);
   
   //Add the file header
   %headerLines = getRecordCount($HL_DocGenerator_File_Header);
   for(%j = 0; %j < %headerLines; %j++)
      %fileObj.writeLine(getRecord($HL_DocGenerator_File_Header,%j));
   
   for(%j = 1; %j <= %i; %j++)
      %fileObj.writeLine(%line[%j]);
      
    closeFileObj(%fileObj);    
    
}
//------------------------------------------------------------------------------

