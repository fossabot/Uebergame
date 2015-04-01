//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Define common sky class globals


$LabPhysicInspectorFields["shape"] = "mass friction staticFriction restitution friction staticFriction";
$LabPhysicInspectorFields["force"] = "linearDamping angularDamping linearSleepThreshold angularSleepThreshold";
$LabPhysicInspectorFields["test"] = "dragForce vertFactor normalForce restorativeForce rollForce pitchForce";
$LabPhysicInspectorFields = $LabPhysicInspectorFields["shape"] SPC $LabPhysicInspectorFields["force"] SPC $LabPhysicInspectorFields["test"];


//==============================================================================
// Build unified sky parameters for FX Manager
function Lab::buildLabPhysicInspectorParams( %this ) {
      return; //Fix that..

    foreach$(%field in  $LabPhysicInspectorFields) {
        $PhysicShapeDataFull[%field] = $PhysicShapeDataBrief[%field] SPC "::" SPC $PhysicShapeDataDoc[%field];
    }

    %params = newScriptObject("paramsLabPhysicInspector");

    %params.baseGuiControl = LabPhysicInspectorWindow;
    %params.aggregateStyle = "Box";
    %params.style = "Default_220";
    %params.mouseAreaClass = "LabPhysicFieldMouse";
    %params.tooltipDelay = "2000";
    %params.common["command"] = "updateParamCtrl($ThisControl,\"Lab.updateLabPhysicInspectorField\",\"cfg\",\"\",\"\");";
    %params.common["AltCommand"] = "updateParamCtrl($ThisControl,\"Lab.updateLabPhysicInspectorField\",\"\",\"\",\"\");";


//-------------------------------------------------
// Group 1 Configuration
    %gid++;
    %pid = 0;
    //"mass friction staticFriction restitution";

    %params.groupData[%gid] = "Forces" TAB "LabPhysicStack" TAB "Rollout";
    %params.groupParam[%gid,%pid++] = "mass"  TAB "" TAB "SliderEdit" TAB "range::0 10;;precision::2;;tooltip::"@$PhysicShapeDataFull["mass"];
    %params.groupParam[%gid,%pid++] = "friction"  TAB "" TAB "SliderEdit" TAB "range::0 2;;precision::3;;tooltip::"@$PhysicShapeDataFull["friction"];
    %params.groupParam[%gid,%pid++] = "staticFriction"  TAB "" TAB "SliderEdit" TAB "range::0 2;;precision::3;;tooltip::"@$PhysicShapeDataFull["staticFriction"];
    %params.groupParam[%gid,%pid++] = "restitution"  TAB "" TAB "SliderEdit" TAB "range::0 2;;precision::3;;tooltip::"@$PhysicShapeDataFull["restitution"];



//-------------------------------------------------
// Group 1 Configuration
    %gid++;
    %pid = 0;
    //"linearDamping angularDamping linearSleepThreshold angularSleepThreshold";
    %params.groupData[%gid] = "Misc" TAB "LabPhysicStack Append" TAB "Rollout";
    %params.groupParam[%gid,%pid++] = "linearDamping"  TAB "" TAB "SliderEdit" TAB "range::0 10;;precision::3;;tooltip::"@$PhysicShapeDataFull["linearDamping"];
    %params.groupParam[%gid,%pid++] = "angularDamping"  TAB "" TAB "SliderEdit" TAB "range::0 10;;precision::3;;tooltip::"@$PhysicShapeDataFull["angularDamping"];
    %params.groupParam[%gid,%pid++] = "linearSleepThreshold"  TAB "" TAB "SliderEdit" TAB "range::0 10;;precision::1;;tooltip::"@$PhysicShapeDataFull["linearSleepThreshold"];
    %params.groupParam[%gid,%pid++] = "angularSleepThreshold"  TAB "" TAB "SliderEdit" TAB "range::0 10;;precision::1;;tooltip::"@$PhysicShapeDataFull["angularSleepThreshold"];
//-------------------------------------------------
// Group 1 Configuration
    %gid++;
    %pid = 0;
    //"dragForce vertFactor normalForce restorativeForce rollForce pitchForce";
    %params.groupData[%gid] = "Misc" TAB "LabPhysicStack Append" TAB "Rollout";
    %params.groupParam[%gid,%pid++] = "dragForce"  TAB "" TAB "SliderEdit" TAB "range::0 100000;;precision::3;;tooltip::"@$PhysicShapeDataFull["linearDamping"];
    %params.groupParam[%gid,%pid++] = "vertFactor"  TAB "" TAB "SliderEdit" TAB "range::0 100000;;precision::3;;tooltip::"@$PhysicShapeDataFull["angularDamping"];
    %params.groupParam[%gid,%pid++] = "normalForce"  TAB "" TAB "SliderEdit" TAB "range::0 100000;;precision::1;;tooltip::"@$PhysicShapeDataFull["linearSleepThreshold"];
    %params.groupParam[%gid,%pid++] = "restorativeForce"  TAB "" TAB "SliderEdit" TAB "range::0 100000;;precision::1;;tooltip::"@$PhysicShapeDataFull["angularSleepThreshold"];
    %params.groupParam[%gid,%pid++] = "rollForce"  TAB "" TAB "SliderEdit" TAB "range::0 100000;;precision::1;;tooltip::"@$PhysicShapeDataFull["linearSleepThreshold"];
    %params.groupParam[%gid,%pid++] = "pitchForce"  TAB "" TAB "SliderEdit" TAB "range::0 100000;;precision::1;;tooltip::"@$PhysicShapeDataFull["angularSleepThreshold"];

%this.prepareLabParams(%params);
   // prepareParamsInterfaceB(%params);
    %this.syncLabPhysicInspectorParams();
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::syncLabPhysicInspectorParams( %this,%apply ) {

    if(!isObject(LabPhysicPlugin)) return;
    %data = LabPhysicPlugin.getSelectedDatablock();
    if (!isObject(%data)) return;

    foreach$(%field in $LabPhysicInspectorFields) {
        %value = %data.getFieldValue(%field);

        %intField = getWord(strreplace(%ctrl.internalName,"_"," "),0);
        %ctrl = LabPhysicStack.findObjectByInternalName(%field,true);
        if (!isObject(%ctrl)) {
            warnLog(%data.getName(),"Trying to sync invalid ctrl for field:",%field);
            continue;
        }



       %ctrl.setTypeValue(%value);
        updateParamFriends(%ctrl);
        if (%apply)
            %this.updateLabPhysicInspectorField(%field,%value);
    }

}
//------------------------------------------------------------------------------

function LabPhysicFieldMouse::onMouseEnter(%this,%modifier,%mousePoint,%mouseClickCount) {
    PhysicShapeFieldInfoControl.setText( %this.infoText );
}
$PhysicShapeDataBrief["mass"] = "Value representing the mass of the shape";
$PhysicShapeDataDoc["mass"] = "A shape's mass influences the magnitude of any force exerted on it. " SPC
                              "For example, a PhysicsShape with a large mass requires a much larger force to move than " SPC
                              "the same shape with a smaller mass.\n" SPC
                              "@note A mass of zero will create a kinematic shape while anything greater will create a dynamic shape.";



$PhysicShapeDataBrief["friction"] = "Coefficient of kinetic %friction to be applied to the shape.";
$PhysicShapeDataDoc["friction"] = "Kinetic %friction reduces the velocity of a moving object while it is in contact with a surface. " SPC
                                  "A higher coefficient will result in a larger velocity reduction. " SPC
                                  "A shape's friction should be lower than it's staticFriction, but larger than 0." SPC
                                  "@note This value is only applied while an object is in motion. For an object starting at rest, see PhysicsShape::staticFriction";


$PhysicShapeDataBrief["staticFriction"] = "Coefficient of static %friction to be applied to the shape";
$PhysicShapeDataDoc["staticFriction"] = "Static %friction determines the force needed to start moving an at-rest object in contact with a surface." SPC
                                        "If the force applied onto shape cannot overcome the force of static %friction, the shape will remain at rest. "SPC
                                        "A larger coefficient will require a larger force to start motion. "SPC
                                        "This value should be larger than zero and the physicsShape's friction."SPC
                                        "@note This value is only applied while an object is at rest. For an object in motion, see PhysicsShape::friction";

$PhysicShapeDataBrief["restitution"] = "Coeffecient of a bounce applied to the shape in response to a collision";
$PhysicShapeDataFieldDoc["restitution"] = "Restitution is a ratio of a shape's velocity before and after a collision. "SPC
        "A value of 0 will zero out a shape's post-collision velocity, making it stop on contact. "SPC
        "Larger values will remove less velocity after a collision, making it \'bounce\' with a greater force. "SPC
        "Normal %restitution values range between 0 and 1.0."SPC
        "@note Values near or equaling 1.0 are likely to cause undesirable results in the physics simulation."SPC
        " Because of this it is reccomended to avoid values close to 1.0";

$PhysicShapeDataBrief["linearDamping"] = "Value that reduces an object's linear velocity over time";
$PhysicShapeDataFieldDoc["linearDamping"] = "Larger values will cause velocity to decay quicker";

$PhysicShapeDataBrief["angularDamping"] = "Value that reduces an object's rotational velocity over time";
$PhysicShapeDataFieldDoc["angularDamping"] = "Larger values will cause velocity to decay quicker";

$PhysicShapeDataBrief["linearSleepThreshold"] = "Minimum linear velocity before the shape can be put to sleep";
$PhysicShapeDataFieldDoc["linearSleepThreshold"] ="This should be a positive value. Shapes put to sleep will not be simulated in order to save system resources."SPC
        "@note The shape must be dynamic.";

$PhysicShapeDataBrief["angularSleepThreshold"] = "Minimum rotational velocity before the shape can be put to sleep";
$PhysicShapeDataFieldDoc["angularSleepThreshold"] = "This should be a positive value. Shapes put to sleep will not be simulated in order to save system resources."SPC
        "@note The shape must be dynamic.";

/*
DefineEngineMethod( PhysicsShape, isDestroyed, bool, (),,
   "@brief Returns if a PhysicsShape has been destroyed or not.\n\n" )
{
   return object->isDestroyed();
}

DefineEngineMethod( PhysicsShape, destroy, void, (),,
   "@brief Disables rendering and physical simulation.\n\n"
   "Calling destroy() will also spawn any explosions, debris, and/or destroyedShape "
   "defined for it, as well as remove it from the scene graph.\n\n"
   "Destroyed objects are only created on the server. Ghosting will later update the client.\n\n"
   "@note This does not actually delete the PhysicsShape." )
{
   object->destroy();
}

DefineEngineMethod( PhysicsShape, restore, void, (),,
   "@brief Restores the shape to its state before being destroyed.\n\n"
   "Re-enables rendering and physical simulation on the object and "
   "adds it to the client's scene graph. "
   "Has no effect if the shape is not destroyed.\n\n")
{
   object->restore();
}

DefineConsoleFunction( physicsPluginPresent, bool, (), , "physicsPluginPresent()"
   "@brief Returns true if a physics plugin exists and is initialized.\n\n"
   "@ingroup Physics" )
{
   return PHYSICSMGR != NULL;
}

DefineConsoleFunction( physicsInit, bool, (const char * library), ("default"), "physicsInit( [string library] )")
{
   return PhysicsPlugin::activate( library );
}

DefineConsoleFunction( physicsDestroy, void, (), , "physicsDestroy()")
{
   if ( PHYSICSMGR )
      PHYSICSMGR->destroyPlugin();
}

DefineConsoleFunction( physicsInitWorld, bool, (const char * worldName), , "physicsInitWorld( String worldName )")
{
    bool res = PHYSICSMGR && PHYSICSMGR->createWorld( String( worldName ) );
   return res;
}

DefineConsoleFunction( physicsDestroyWorld, void, (const char * worldName), , "physicsDestroyWorld( String worldName )")
{
   if ( PHYSICSMGR )
      PHYSICSMGR->destroyWorld( worldName );
}


// Control/query of the stop/started state
// of the currently running simulation.
DefineConsoleFunction( physicsStartSimulation, void, (const char * worldName), , "physicsStartSimulation( String worldName )")
{
   if ( PHYSICSMGR )
      PHYSICSMGR->enableSimulation( String( worldName ), true );
}

DefineConsoleFunction( physicsStopSimulation, void, (const char * worldName), , "physicsStopSimulation( String worldName )")
{
   if ( PHYSICSMGR )
      PHYSICSMGR->enableSimulation( String( worldName ), false );
}

DefineConsoleFunction( physicsSimulationEnabled, bool, (), , "physicsStopSimulation( String worldName )")
{
   return PHYSICSMGR && PHYSICSMGR->isSimulationEnabled();
}

// Used for slowing down time on the
// physics simulation, and for pausing/restarting
// the simulation.
DefineConsoleFunction( physicsSetTimeScale, void, (F32 scale), , "physicsSetTimeScale( F32 scale )")
{
   if ( PHYSICSMGR )
      PHYSICSMGR->setTimeScale( scale );
}

// Get the currently set time scale.
DefineConsoleFunction( physicsGetTimeScale, F32, (), , "physicsGetTimeScale()")
{
   return PHYSICSMGR && PHYSICSMGR->getTimeScale();
}

// Used to send a signal to objects in the
// physics simulation that they should store
// their current state for later restoration,
// such as when the editor is closed.
DefineConsoleFunction( physicsStoreState, void, (), , "physicsStoreState()")
{
   PhysicsPlugin::getPhysicsResetSignal().trigger( PhysicsResetEvent_Store );
}

// Used to send a signal to objects in the
// physics simulation that they should restore
// their saved state, such as when the editor is opened.
DefineConsoleFunction( physicsRestoreState, void, (), , "physicsRestoreState()")
{
   if ( PHYSICSMGR )
      PHYSICSMGR->reset();
}

DefineConsoleFunction( physicsDebugDraw, void, (bool enable), , "physicsDebugDraw( bool enable )")
{
   if ( PHYSICSMGR )
      PHYSICSMGR->enableDebugDraw( enable );
}

*/