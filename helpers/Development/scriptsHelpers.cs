//==============================================================================
// GameLab Helpers -> Development scripts helpers
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
// Load a script type file assigned to a scriptGroup :: $ScriptGroup[%type] = "file_path_here";
function loadScript(%type) {
	if (!isFile($ScriptGroup[%type])) {
		warnLog("Invalid file to execute for type:",%type,"File checked:",$ScriptGroup[%type]);
		return;
	}
	exec($ScriptGroup[%type]);
}
//------------------------------------------------------------------------------
