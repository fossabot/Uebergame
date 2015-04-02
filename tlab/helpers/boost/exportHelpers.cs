//==============================================================================
// Boost! --> Exporting and Configs Helpers
// Copyright NordikLab Studio, 2013
//==============================================================================
//==============================================================================
// Globals exporting system (During development only)
//==============================================================================

//==============================================================================
//Export client globals
function exportClientGlobals() {

	if (!$Cfg::Dev::ExportGlobals) return;
	//Export each data modules
	export("$UI*", "data/globals/client/uiGlobals.cs", False);
	export("$My*", "data/globals/client/myGlobals.cs", False);
	export("$cSlot*", "data/globals/client/cSlot.cs", False);
	export("$cEvent*", "data/globals/client/cEvent.cs", False);
	export("$cGame*", "data/globals/client/cGame.cs", False);
	export("$cStanding*", "data/globals/client/cStanding.cs", False);
	export("$cRace*", "data/globals/client/cRace.cs", False);
	export("$GarageCar*", "data/globals/garage/garageCar.cs", False);
	export("$cAdmin*", "data/globals/client/admin.cs", False);


}
//------------------------------------------------------------------------------
//==============================================================================
function exportServerGlobals() {
	//Export each data modules
	if (!$Cfg::Dev::ExportGlobals) return;
	export("$Slot*", "data/globals/server/Slot.cs", False);
	export("$Event*", "data/globals/server/Event.cs", False);
	export("$Track*", "data/globals/server/Track.cs", False);
	export("$Game*", "data/globals/server/Game.cs", False);
	export("$Standing*", "data/globals/server/Standing.cs", False);
	export("$Server*", "data/globals/server/Server.cs", False);
	export("$Session*", "data/globals/server/Session.cs", False);

}
//------------------------------------------------------------------------------
//==============================================================================
function exportDatabaseGlobals() {
	if (!$Cfg::Dev::ExportGlobals) return;
	//Export each data modules
	export("$DB_*", "data/globals/Database/localDB.cs", False);
}
//------------------------------------------------------------------------------
//==============================================================================
function exportDataGlobals() {
	//Export each data modules
	if (!$Cfg::Dev::ExportGlobals) return;
	export("$Data*", "data/globals/data/allData.cs", False);
	export("$Data_Car*", "data/globals/data/dataCar.cs", False);
	export("$Build*", "data/globals/data/buildInfo.cs", False);
}
//------------------------------------------------------------------------------
//==============================================================================
function exportCarGlobals() {
	if (!$Cfg::Dev::ExportGlobals) return;
	export("$CarStreet*", "data/globals/car/carStreet.cs", False);
	export("$CarBase*", "data/globals/car/carBasic.cs", False);
}
//------------------------------------------------------------------------------
//==============================================================================
function exportEveryGlobals() {
	if (!$Cfg::Dev::ExportGlobals) return;
	export("$*", "data/globals/all/allGlobals.cs", False);
}
//------------------------------------------------------------------------------
//==============================================================================
function exportDevGlobals() {
	if (!$Cfg::Dev::ExportGlobals) return;
	export("$Dev_Bug*", "data/globals/dev/bugs.cs", False);

}
//------------------------------------------------------------------------------
//==============================================================================
function exportTestGlobals() {
	if (!$Cfg::Dev::ExportGlobals) return;
	export("$Test*", "data/globals/test/fullTest.cs", False);

}
//------------------------------------------------------------------------------

//==============================================================================
function exportAllGlobals() {
	if (!$Cfg::Dev::ExportGlobals) return;
	//Export each data modules
	exportClientGlobals();
	exportServerGlobals();
	exportDatabaseGlobals();
	exportDataGlobals();
	exportCarGlobals();
}
//------------------------------------------------------------------------------

//==============================================================================
// Export Objects content to file (During development only)
//==============================================================================
//==============================================================================
//Export specified obj to file with optional name
function exportObj(%obj,%name) {
	%name = %obj.getName();
	if (%name $= "")
		%name = %obj.getName();

	%file =  "data/globals/objects/"@%name@".cs";
	%fileWrite = new FileObject();

	%result = %fileWrite.OpenForWrite(%file);

	%fileWrite.writeObject(%obj);
	%fileWrite.close();
	%fileWrite.delete();
}
//------------------------------------------------------------------------------
//==============================================================================
function exportTextIds() {
	%file =  "data/globals/lang/TextIds.txt";
	%fileWrite = new FileObject();
	%result = %fileWrite.OpenForWrite(%file);

	%recordCount = getRecordCount($TextIds);
	for(%i=0; %i<%recordCount; %i++) {
		%record = getRecord($TextIds,%i);
		%id = getField(%record,0);
		%text = getField(%record,1);

		%fileWrite.writeLine(%id SPC "=" SPC %text);
	}
	%fileWrite.close();
	%fileWrite.delete();
}
//------------------------------------------------------------------------------
