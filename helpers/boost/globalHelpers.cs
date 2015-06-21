//==============================================================================
// Boost! --> SimObject Helpers
// Copyright NordikLab Studio, 2013
//==============================================================================

//==============================================================================
// String Globals Helpers
//==============================================================================

//==============================================================================
//Export client globals
function storeAllGlobals() {

	if (!$Cfg::Dev::ExportGlobals) return;
	storeClientGlobals();
	storeServerGlobals();

}
//------------------------------------------------------------------------------
//==============================================================================
//Export client globals
function storeClientGlobals() {


	if (!$Cfg::Dev::ExportGlobals) return;
	//Export each data modules
	export("$MySlot*", "data/globalsClient/mySlot.cs", False);
	export("$MyStat*", "data/globalsClient/myStats.cs", False);
	export("$cEventData*", "data/globalsClient/eventData.cs", False);
	export("$cTrackData*", "data/globalsClient/eventData.cs", true);

	export("$Env::*", "data/globalsClient/levelWeather.cs", False);
	export("$PostFX::*", "data/globalsClient/levelWeather.cs", true);

	export("$cSlot*", "data/globalsClient/slotData.cs", False);

	export("$Car*", "data/globalsClient/fullCar.cs", False);
}
//------------------------------------------------------------------------------
//==============================================================================
//Export client globals
function storeServerGlobals() {

	if (!$Cfg::Dev::ExportGlobals) return;
	//Export each data modules
	export("$St*", "data/globalsServer/statistics.cs", False);
	export("$Slot*", "data/globalsServer/slotData.cs", False);


}
//------------------------------------------------------------------------------

