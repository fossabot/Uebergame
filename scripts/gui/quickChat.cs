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

// Bind - Team - Menu name - Displayed Text - Wav file - Animation - Play3D Audio
openChatRoot();

openChatGroup("G", "Global");
   addChatItem("1", false, "Hello", "Hello.", 0);
   addChatItem("2", false, "Goodbye", "Goodbye.", 0);
   addChatItem("3", false, "Yes", "Yes.", 0);
   addChatItem("4", false, "No", "No.", 0);
   addChatItem("5", false, "Don\'t know", "I don\'t know.", 0);
   addChatItem("6", false, "Thanks", "Thanks.", 0);
   addChatItem("7", false, "Sorry", "Sorry.", 0);
   addChatItem("8", false, "No problem", "No problem.", 0);
   addChatItem("9", false, "Oops", "Oops!","");
   addChatItem("0", false, "Aww crap", "Aww crap!", 0);
   addChatItem("Q", false, "Doh", "Doh!", 0);
   addChatItem("W", false, "Duh", "Duh!", 0);
   addChatItem("E", false, "Whoohoo", "Woo-hoo!", 0, "dance");
closeChatGroup();

openChatGroup("E", "Team");
   addChatItem("1", true, "Yes", "Yes.", 0);
   addChatItem("2", true, "No", "No.", 0);
   addChatItem("3", true, "Don\'t know", "I don\'t know.", 0);
   addChatItem("4", true, "Thanks", "Thanks.", 0);
   addChatItem("5", true, "Sorry", "I'm sorry.", 0);
   addChatItem("6", true, "No problem", "No problem.", 0);
   addChatItem("7", true, "Ready", "I am ready.", "salute");
   addChatItem("8", true, "All clear", "All clear.", "wave");
   addChatItem("9", true, "Follow me", "Follow me!", 0, "wave");
   addChatItem("0", true, "Acknowledged", "Acknowledged.", 0, "salute");
closeChatGroup();

openChatGroup("T", "Taunts");
   addChatItem("1", false, "You idiot", "You idiot!", 0);
   addChatItem("2", false, "How\'d THAT feel?", "How\'d THAT feel?", 0, "taunt1");
   addChatItem("3", false, "I\'ve had worse...", "I\'ve had worse...", 0, "taunt2");
   addChatItem("4", false, "Dance!", "Dance!", 0, "dance");
   addChatItem("5", false, "Come get some!", "Come get some!", 0, "range");
   addChatItem("6", false, "Missed me!", "Missed me!", 0);
closeChatGroup();

openChatGroup("V", "Offense");
   addChatItem("1", true, "Attack!", "Attack!", 0);
   addChatItem("2", true, "Flank", "Flank the enemy!", 0);
   addChatItem("3", true, "Retreat", "Retreat!", 0);
   addChatItem("4", true, "Regroup", "Regroup.", 0);
   addChatItem("5", true, "Retreat", "Retreat!", 0);
   addChatItem("6", true, "Need offense", "We need more offense!", 0);
   addChatItem("7", true, "Move out", "Move out!", 0);
   addChatItem("8", true, "Cover me", "Cover me.", 0);
closeChatGroup();

openChatGroup("D", "Defense");
   addChatItem("1", true, "Defending", "I\'m on defence.", 0);
   addChatItem("2", true, "Need defense", "We need more defence!", 0);
   addChatItem("3", true, "Incomming", "Incomming!", 0);
   addChatItem("4", true, "Taking fire", "I\'m taking fire!", 0);
   addChatItem("5", true, "All clear", "All clear.", 0);
   addChatItem("6", true, "Incomming N", "INCOMING NORTH!", 0);
   addChatItem("7", true, "Incomming S", "INCOMING SOUTH!", 0);
   addChatItem("8", true, "Incomming E", "INCOMING EAST!", 0);
   addChatItem("9", true, "Incomming W", "INCOMING WEST!", 0);
closeChatGroup();

