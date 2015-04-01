//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================



//==============================================================================
// Specific Test functions -- Old system which would be likely replaced with new
//==============================================================================

//----------------------------------------------------------------------------
// TestNet -> Test different NetWorking package
function testNet(%mode,%local) {
	if (!%mode)
		return;

	switch$(%mode) {
	case "1":
		$Cfg::Net::PacketRateToClient = "16";
		$Cfg::Net::PacketRateToServer = "32";
		$Cfg::Net::PacketSize = "256";

	case "2":
		$Cfg::Net::PacketRateToClient = "64";
		$Cfg::Net::PacketRateToServer = "64";
		$Cfg::Net::PacketSize = "256";

	case "3":
		$Cfg::Net::PacketRateToClient = "128";
		$Cfg::Net::PacketRateToServer = "128";
		$Cfg::Net::PacketSize = "256";

	case "4":
		$Cfg::Net::PacketRateToClient = "128";
		$Cfg::Net::PacketRateToServer = "128";
		$Cfg::Net::PacketSize = "1024";

	}
	if (!%local) {
		msgAll("Set Net Test Mode" SPC %mode,"testNet",%mode, true);
		foreach(%client in ClientGroup) {
			%client.checkMaxRate();
		}
	}
}

//----------------------------------------------------------------------------
// devNet -> Change single Net setting
function netPacket(%setting,%value,%local) {

	eval("$Cfg::Net::Packet"@%setting@" = %value;");


	if (!%local) {
		msgAll("Set Net Packet-->" SPC %setting SPC "set to-->" SPC %value,"netPacket",%setting,%value, true);
	}
}

//----------------------------------------------------------------------------
// testCar -> Test different Car package
function testCar(%mode,%local) {
	if (!%mode)
		return;
	switch$(%mode) {
	case "1":
		$CarBase_Integration = "16";
		$CarBase_MaxPredictionTicks = "30";
		$CarBase_MaxWarpTicks = "5";
		$CarBase_MoveRetryCount = "3";

	case "2":
		$CarBase_Integration = "16";
		$CarBase_MaxPredictionTicks = "5";
		$CarBase_MaxWarpTicks = "5";
		$CarBase_MoveRetryCount = "3";

	case "3":
		$CarBase_Integration = "16";
		$CarBase_MaxPredictionTicks = "0";
		$CarBase_MaxWarpTicks = "5";
		$CarBase_MoveRetryCount = "3";

	case "4":
		$CarBase_Integration = "16";
		$CarBase_MaxPredictionTicks = "10";
		$CarBase_MaxWarpTicks = "10";
		$CarBase_MoveRetryCount = "3";

	case "5":
		$CarBase_Integration = "16";
		$CarBase_MaxPredictionTicks = "10";
		$CarBase_MaxWarpTicks = "2";
		$CarBase_MoveRetryCount = "3";

	case "6":
		$CarBase_Integration = "16";
		$CarBase_MaxPredictionTicks = "10";
		$CarBase_MaxWarpTicks = "0";
		$CarBase_MoveRetryCount = "3";

	case "7":
		$CarBase_Integration = "16";
		$CarBase_MaxPredictionTicks = "0";
		$CarBase_MaxWarpTicks = "0";
		$CarBase_MoveRetryCount = "0";

	case "8":
		$CarBase_Integration = "16";
		$CarBase_MaxPredictionTicks = "10";
		$CarBase_MaxWarpTicks = "5";
		$CarBase_MoveRetryCount = "0";

	}

	if (!%local)
		msgAll("Set Car Test Mode" SPC %mode,"testCar",%mode, true);

}

//----------------------------------------------------------------------------
// devCar -> Change single Car global setting
function devCar(%setting,%value,%local) {

	$CarBase_[%setting] = %value;


	if (!%local) {
		msgAll("Set devCar" SPC %setting SPC "to" SPC %value,"devCar",%setting,%value, true);
	}
}

//----------------------------------------------------------------------------
// Debug commands
//----------------------------------------------------------------------------

