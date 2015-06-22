//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SceneEditorDialogs::onActivated( %this ) {	
	if (SceneEditorDialogs.selectedPage $= "")
		SceneEditorDialogs.selectedPage = "0";
	SEP_AmbientBook.selectPage(SceneEditorDialogs.selectedPage);
	SEP_ScatterSkyBook.selectPage(0);
	SEP_PrecipitationBook.selectPage(0);
	SEP_AmbientManager.initBasicCloudsData();
	SEP_ScatterSkyManager.initData();
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::initDialog( %this ) {
	logd("SEP_AmbientManager::initDialog(%this)");

	if (!isObject(SEP_AmbientManager_PM))
		new PersistenceManager(SEP_AmbientManager_PM);

	SEP_ScatterSkyManager.buildParams();
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::onActivated( %this ) {
	logd("SEP_AmbientManager::onActivated(%this)");

}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::onWake( %this ) {
	SEP_PrecipitationManager.initData();
	SEP_PrecipitationBook.selectPage(0);
	
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientManager::onSleep( %this ) {
	logd("SEP_AmbientManager::onSleep( %this )");
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_AmbientBook::onTabSelected( %this,%text,%index ) {
	logd("SEP_AmbientManager::onTabSelected( %this,%text,%index )");
	
	SceneEditorDialogs.selectedPage = %index;
}
//------------------------------------------------------------------------------
