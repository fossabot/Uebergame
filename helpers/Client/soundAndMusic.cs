//==============================================================================
// GameLab -> 
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
///Play a music file or profile, looping or not and in 3D if position specified
///File is relative to art/sound/ folder
function playMusic( %fileOrProfile, %loop,%pos ) {
   if (%pos.x !$="" && %pos.y !$="" && %pos.x !$="")
      %desc = AudioMusicLoop3D;
   else
      %desc = AudioMusicLoop2D;
   
   if (isFile(%fileOrProfile))
      %file = %fileOrProfile;
   else if (isFile("art/sound/"@%fileOrProfile))
      %file = "art/sound/"@%fileOrProfile;

   %currentList = findPlayingSourceType("Music");
   foreach$(%src in %currentList)
      %src.delete();  
  
   
   if (isFile(%file)){
      $GameMusicSource = sfxCreateSource( %desc, %file, %pos.x, %pos.y, %pos.z );
      if (isObject($GameMusicSource))
         $GameMusicSource.play();
   }
   else if (isObject(%fileOrProfile)){
      $GameMusicSource = sfxCreateSource(%fileOrProfile, %pos.x, %pos.y, %pos.z );
   }
   sfxDeleteWhenStopped($GameMusicSource);
	
}
//------------------------------------------------------------------------------
//==============================================================================
///Play a music file or profile, looping or not and in 3D if position specified
///File is relative to art/sound/ folder
function stopMusic(%checkAll  ) {
   $GameMusicSource.delete();
   
   if (!%checkAll) return;
   %currentList = findPlayingSourceType("Music");
   foreach$(%src in %currentList)
      %src.delete(); 	
}
//------------------------------------------------------------------------------
//==============================================================================
///Play a music file or profile, looping or not and in 3D if position specified
///File is relative to art/sound/ folder
function findPlayingSourceType( %channel ) {
   %fullChannel = "AudioChannel"@%channel;
   
   %currentList = sfxDumpSourcesToString(true);
   for(;%i<getFieldCount(%currentList);%i++){
      %field = getField(%currentList,%i);
     
      %field = strreplace(%field,":","");
      %obj = firstWord(trim(%field));
   
      %field = removeWord(%field,0);
      %field = strreplace(%field,",","\t");
	   
	    %group = getField(%field,8);
	    
	     %thisChan = strreplace(%group,"="," ");
	     %thisChan = getWord(trim(%thisChan),1);
	    
	      if (%thisChan $= %fullChannel)
	         %return = strAddWord(%return,%obj);
   }
   return %return;
}
//------------------------------------------------------------------------------

/*
bool 	sfxCreateDevice (string provider, string device, bool useHardware, int maxBuffers)
 	Try to create a new sound device using the given properties. 
SFXSource 	sfxCreateSource (SFXTrack track)
 	Create a new source that plays the given track. 
SFXSource 	sfxCreateSource (SFXTrack track, float x, float y, float z)
 	Create a new source that plays the given track and position its 3D sounds source at the given coordinates (if it is a 3D sound). 
SFXSound 	sfxCreateSource (SFXDescription description, string filename)
 	Create a temporary SFXProfile from the given description and filename and then create and return a new source that plays the profile. 
SFXSound 	sfxCreateSource (SFXDescription description, string filename, float x, float y, float z)
 	Create a temporary SFXProfile from the given description and filename and then create and return a new source that plays the profile. Position the sound source at the given coordinates (if it is a 3D sound). 
void 	sfxDeleteDevice ()
 	Delete the currently active sound device and release all its resources. 
void 	sfxDeleteWhenStopped (SFXSource source)
 	Mark the given source for deletion as soon as it moves into stopped state. 
void 	sfxDumpSources (bool includeGroups=false)
 	Dump information about all current SFXSource instances to the console. 
string 	sfxDumpSourcesToString (bool includeGroups=false)
 	Dump information about all current SFXSource instances to a string. 
string 	sfxGetActiveStates ()
 	Return a newline-separated list of all active states. 
string 	sfxGetAvailableDevices ()
 	Get a list of all available sound devices. 
string 	sfxGetDeviceInfo ()
 	Return information about the currently active sound device. 
SFXDistanceModel 	sfxGetDistanceModel ()
 	Get the falloff curve type currently being applied to 3D sounds. 
float 	sfxGetDopplerFactor ()
 	Get the current global doppler effect setting. 
float 	sfxGetRolloffFactor ()
 	Get the current global scale factor applied to volume attenuation of 3D sounds in the logarithmic model. 
SFXSource 	sfxPlay (SFXSource source)
 	Start playback of the given source. 
void 	sfxPlay (SFXTrack track)
 	Create a new play-once source for the given track and start playback of the source. 
void 	sfxPlay (SFXTrack track, float x, float y, float z)
 	Create a new play-once source for the given track, position its 3D sound at the given coordinates (if the track's description is set up for 3D sound) and start playback of the source. 
SFXSource 	sfxPlayOnce (SFXTrack track)
 	Create a play-once source for the given track. 
SFXSource 	sfxPlayOnce (SFXTrack track, float x, float y, float z, float fadeInTime=-1)
 	Create a play-once source for the given given track and position the source's 3D sound at the given coordinates only if the track's description is set up for 3D sound). 
SFXSource 	sfxPlayOnce (SFXDescription description, string filename)
 	Create a new temporary SFXProfile from the given description and filename, then create a play-once source for it and start playback. 
SFXSource 	sfxPlayOnce (SFXDescription description, string filename, float x, float y, float z, float fadeInTime=-1)
 	Create a new temporary SFXProfile from the given description and filename, then create a play-once source for it and start playback. Position the source's 3D sound at the given coordinates (only if the description is set up for 3D sound). 
void 	sfxSetDistanceModel (SFXDistanceModel model)
 	Set the falloff curve type to use for distance-based volume attenuation of 3D sounds. 
void 	sfxSetDopplerFactor (float value)
 	Set the global doppler effect scale factor. 
void 	sfxSetRolloffFactor (float value)
 	Set the global scale factor to apply to volume attenuation of 3D sounds in the logarithmic model. 
void 	sfxStop (SFXSource source)
 	Stop playback of the given source. 
void 	sfxStopAndDelete (SFXSource source)
 	Stop playback of the given source (if it is not already stopped) and delete the source. 
 	*/