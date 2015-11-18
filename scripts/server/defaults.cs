//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

// List of master servers to query, each one is tried in order
// until one responds
$pref::Master[0] = "2:88.198.65.149:28002";
$Pref::Server::RegionMask = 2; // 0 should mean all regions

// Information about the server
$Pref::Server::Name = "Ubergame server";
$Pref::Server::Info = "This is an Ubergame server.";

// The network port is also defined by the client, this value 
// overrides pref::net::port for dedicated servers
$Pref::Server::Port = 28000;

// If the password is set, clients must provide it in order
// to connect to the server
$Pref::Server::Password = "";

// Text to appear on loading screen
// Multiple lines possible just add more varibles
$pref::Server::Message0 = "Server Information";
$pref::Server::Message1 = "Welcome to the Ubergame server!";
$pref::Server::Message2 = "Treat others with respect.";

// The connection error message is transmitted to the client immediatly
// on connection, if any further error occures during the connection
// process, such as network traffic mismatch, or missing files, this error
// message is display. This message should be replaced with information
// usefull to the client, such as the url or ftp address of where the
// latest version of the game can be obtained.
$Pref::Server::ConnectionError =
   "You do not have the correct version of the game or "@
   "the related art needed to play on this server, please contact "@
   "the server operator for more information.";

$pref::Server::Dedicated = 0;

// Password for admin clients
$Pref::Server::AdminPassword = "changeme";
$pref::Server::SuperAdminPassword = "changemetoo";

// Misc server settings.
$pref::Server::BadWordFilter = 1;
$pref::Server::AiCount = 0;
$pref::Server::BaseSacking = 1;
$pref::Server::ConnectLog = 0;
$pref::Server::MissionFile = "levels/TG_DesertRuins/TG_DesertRuins_day.mis";
$pref::Server::ConnLogPath = "logs";
$pref::Server::MissionType = "DM";
$Pref::Server::MaxPlayers = 16;
$Pref::Server::TimeLimit = 30;               // In minutes
$Pref::Server::KickBanTime = 300;            // specified in seconds
$Pref::Server::BanTime = 1800;               // specified in seconds
$Pref::Server::FloodProtectionEnabled = 1;
$Pref::Server::MaxChatLen = 120;
$pref::Server::teamName[0] = "Purgatory";
$pref::Server::teamName[1] = "Green Team";
$pref::Server::teamName[2] = "Red Team";
$pref::Server::warmupTime = 30;
$pref::Server::FriendlyFire = 1;
$pref::Server::TournamentMode = 0;
$pref::Server::DisallowVoteAdmin = 1;
$pref::Server::DisallowVoteMission = 0;
$pref::Server::DisallowVoteSkipMission = 0;
$pref::Server::DisallowVoteFriendlyFire = 1;
$pref::Server::DisallowVoteBaseSacking = 0;
$pref::Server::DisallowVoteServerMode = 0;
$pref::Server::DisallowVoteStartMatch = 0;
$pref::Server::DisallowVoteTimeLimit = 0;
