//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================


//==============================================================================
// Validate a string of specific needs
//==============================================================================
//==============================================================================
// Validate a string used for a filename
function checkFilenameString(%filename) {


	%filename = strreplace(%filename," ","_");
	%filename = stripChars(%filename,"/?<>\\@:*|\"");
	return %filename;
}
//------------------------------------------------------------------------------
//==============================================================================
// Validate a string used for a filename
function addObjToString(%obj,%str) {


	//Check if we already have this obj
	%name = %obj.getName();
	if (%name $="") %name = %obj;

	foreach$(%word in %str) {
		if (%word $= %name) {
			%found = true;
			break;
		}
	}


	if (!%found) {
		%str = trim(%str SPC %name);
	}
	return %str;

}
//------------------------------------------------------------------------------


//==============================================================================
// String Manipulation Helpers
//==============================================================================

//==============================================================================
//
function strAddWord(%str,%word,%unique,%prepend) {
   if (%unique){
      foreach$(%w in %str){
         if (%w $= %word)
            return %str;
      }
   } 
   if (%prepend)
      %newStr = trim(%word SPC %str);	
   else
	   %newStr = trim(%str SPC %word);	
	return %newStr;
}
//------------------------------------------------------------------------------

//==============================================================================
//
function strRemoveWord(%str,%word) {
  %id = 0;
   foreach$(%w in %str){
      if (%w $= %word){
         //Reverse order to remove words from last to not affect index ordering
         %removeIds = strAddWord(%removeIds,%id,true,true);
      }
      %id++;        
   }
   foreach$(%id in %removeIds){
      %preStr = %str;
      %str = removeWord(%str,%id);      
   }
 
	return %str;
}
//------------------------------------------------------------------------------


//==============================================================================
//
function strAddField(%str,%word) {
	%str = %str SPC %word;
	%str = trim(%str);
	return %str;
}
function strAddRecord(%str,%word) {
	%str = %str SPC %word;
	%str = trim(%str);
	return %str;
}


//------------------------------------------------------------------------------
// Convert an HTML string into a game text
function strBeforeChar(%str,%search) {
	%searchPos = strpos(%str,%search);
	%newStr = getSubStr(%str,0,%searchPos);
	return %newStr;
}




//------------------------------------------------------------------------------
// Convert an HTML string into a game text
function convertHtmlToGame(%html) {
	//Replace all </p> with new line
	%newText = strreplace(%html , "</p>" , "\n" );
	%newText = strreplace(%newText , "<p>" , "" );

	return %newText;
}

//------------------------------------------------------------------------------
// Convert the extra fields of K2
function convertExtraFields(%fields) {
	//"[{\"id\":\"1\",\"value\":\"1\"},{\"id\":\"2\",\"value\":\"1\"}]";
	//Replace all </p> with new line
	%newText = strreplace(%fields , "</p>" , "\n" );
	%newText = strreplace(%newText , "<p>" , "" );

	return %newText;
}

//--------------------------------------------------------------------------
// Finds location of %word in %text, starting at %start.  Works just like strPos
//--------------------------------------------------------------------------

function wordPos(%text, %word, %start) {
	if (%start $= "") %start = 0;
	if (strpos(%text, %word, 0) == -1) return -1;
	%count = getWordCount(%text);
	if (%start >= %count) return -1;
	for (%i = %start; %i < %count; %i++) {
		if (getWord( %text, %i) $= %word) return %i;
	}
	return -1;
}

//--------------------------------------------------------------------------
// Finds location of %field in %text, starting at %start.  Works just like strPos
//--------------------------------------------------------------------------

function fieldPos(%text, %field, %start) {
	if (%start $= "") %start = 0;
	if (strpos(%text, %field, 0) == -1) return -1;
	%count = getFieldCount(%text);
	if (%start >= %count) return -1;
	for (%i = %start; %i < %count; %i++) {
		if (getField( %text, %i) $= %field) return %i;
	}
	return -1;
}

//--------------------------------------------------------------------------
// returns the text in a file with "\n" at the end of each line
//--------------------------------------------------------------------------

