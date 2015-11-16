// Torque Input Map File
if (isObject(moveMap)) moveMap.delete();
new ActionMap(moveMap);
moveMap.bindCmd(keyboard, "escape", "", "escapeFromGame();");
moveMap.bind(keyboard, "w", moveForward);
moveMap.bind(keyboard, "a", moveleft);
moveMap.bind(keyboard, "s", movebackward);
moveMap.bind(keyboard, "d", moveright);
moveMap.bind(keyboard, "up", moveup);
moveMap.bind(keyboard, "down", movedown);
moveMap.bind(keyboard, "left", turnLeft);
moveMap.bind(keyboard, "right", turnRight);
moveMap.bind(keyboard, "home", panUp);
moveMap.bind(keyboard, "end", panDown);
moveMap.bind(keyboard, "space", jump);
moveMap.bind(keyboard, "c", doCrouch);
moveMap.bind(keyboard, "f", toggleFreeLook);
moveMap.bind(keyboard, "lshift", doSprint);
moveMap.bind(keyboard, "e", mouseJet);
moveMap.bind(keyboard, "q", melee);
moveMap.bind(keyboard, "r", reloadWeapon);
moveMap.bind(keyboard, "+", nextWeapon);
moveMap.bind(keyboard, "minus", cycleLoadoutPrev);
moveMap.bind(keyboard, "1", useFirstWeaponSlot);
moveMap.bind(keyboard, "2", useSecondWeaponSlot);
moveMap.bind(keyboard, "3", useThirdWeaponSlot);
moveMap.bind(keyboard, "4", useFourthWeaponSlot);
moveMap.bind(keyboard, "5", useFifthWeaponSlot);
moveMap.bind(keyboard, "6", useSixthWeaponSlot);
moveMap.bind(keyboard, "7", toggleVehicleHud);
moveMap.bind(keyboard, "8", useEighthWeaponSlot);
moveMap.bind(keyboard, "alt w", throwWeapon);
moveMap.bind(keyboard, "alt a", tossAmmo);
moveMap.bind(keyboard, "b", triggerSpecial);
moveMap.bind(keyboard, "alt b", tossSpecial);
moveMap.bind(keyboard, "g", throwGrenade);
moveMap.bind(keyboard, "alt g", tossGrenade);
moveMap.bind(keyboard, "m", autoMountVehicle);
moveMap.bind(keyboard, "z", toggleZoomFOV);
moveMap.bind(keyboard, "tab", toggleFirstPerson);
moveMap.bind(keyboard, "ctrl w", celebrationWave);
moveMap.bind(keyboard, "ctrl s", celebrationSalute);
moveMap.bind(keyboard, "ctrl k", doSuicide);
moveMap.bind(keyboard, "ctrl f", throwFlag);
moveMap.bind(keyboard, "f1", showPlayerList);
moveMap.bind(keyboard, "f2", toggleScoreScreen);
moveMap.bind(keyboard, "u", toggleMessageHud);
moveMap.bind(keyboard, "y", teamMessageHud);
moveMap.bind(keyboard, "o", fireTeamMessageHud);
moveMap.bind(keyboard, "pageup", pageMessageHudUp);
moveMap.bind(keyboard, "pagedown", pageMessageHudDown);
moveMap.bind(keyboard, "p", resizeMessageHud);
moveMap.bind(keyboard, "v", toggleQuickChatHud);
moveMap.bind(keyboard, "ctrl o", bringUpOptions);
moveMap.bind(keyboard, "insert", voteYes);
moveMap.bind(keyboard, "delete", voteNo);
moveMap.bind(keyboard, "i", toggleArmoryDlg);
moveMap.bind(keyboard, "0", cycleLoadoutNext);
moveMap.bind(keyboard, "f3", toggleMusicPlayer);
moveMap.bind(keyboard, "ctrl h", hideHUDs);
moveMap.bindCmd(keyboard, "x", "commandToServer(\'setWanderPosition\');", "");
moveMap.bind(keyboard, "t", toggleMessageHud);
moveMap.bind(mouse0, "xaxis", S, 0.626086, yaw);
moveMap.bind(mouse0, "yaxis", S, 0.626087, pitch);
moveMap.bind(mouse0, "button0", mouseFire);
moveMap.bind(mouse0, "zaxis", cycleWeaponAxis);
moveMap.bind(mouse0, "button1", toggleIronSights);
moveMap.bind(gamepad0, "thumbrx", D, "-0.23 0.23", gamepadYaw);
moveMap.bind(gamepad0, "thumbry", D, "-0.23 0.23", gamepadPitch);
moveMap.bind(gamepad0, "thumblx", D, "-0.23 0.23", gamePadMoveX);
moveMap.bind(gamepad0, "thumbly", D, "-0.23 0.23", gamePadMoveY);
moveMap.bind(gamepad0, "btn_a", jump);
moveMap.bind(gamepad0, "btn_back", toggleCamera);
moveMap.bindCmd(gamepad0, "lpov", "toggleLightColorViz();", "");
moveMap.bindCmd(gamepad0, "upov", "toggleDepthViz();", "");
moveMap.bindCmd(gamepad0, "dpov", "toggleNormalsViz();", "");
moveMap.bindCmd(gamepad0, "rpov", "toggleLightSpecularViz();", "");
moveMap.bind(gamepad0, "triggerr", gamepadFire);
moveMap.bind(gamepad0, "triggerl", gamepadAltTrigger);
moveMap.bind(gamepad0, "btn_b", toggleZoomFOV);
if (isObject(spectatorMap)) spectatorMap.delete();
new ActionMap(spectatorMap);
spectatorMap.bindCmd(keyboard, "escape", "", "escapeFromGame();");
spectatorMap.bind(keyboard, "up", SD, "0 0", 1, moveup);
spectatorMap.bind(keyboard, "down", SD, "0 0", 1, movedown);
spectatorMap.bind(keyboard, "space", SD, "0 0", 1, jump);
spectatorMap.bind(keyboard, "e", SD, "0 0", 1, mouseJet);
spectatorMap.bind(mouse0, "button0", SD, "0 0", 1, mouseFire);
if (isObject(vehicleMap)) vehicleMap.delete();
new ActionMap(vehicleMap);
vehicleMap.bindCmd(keyboard, "escape", "", "escapeFromGame();");
vehicleMap.bind(keyboard, "w", SD, "0 0", 1, moveForward);
vehicleMap.bind(keyboard, "s", SD, "0 0", 1, movebackward);
vehicleMap.bind(keyboard, "a", SD, "0 0", 1, moveleft);
vehicleMap.bind(keyboard, "d", SD, "0 0", 1, moveright);
vehicleMap.bind(keyboard, "up", SD, "0 0", 1, moveup);
vehicleMap.bind(keyboard, "down", SD, "0 0", 1, movedown);
vehicleMap.bind(keyboard, "e", SD, "0 0", 1, mouseJet);
vehicleMap.bind(keyboard, "space", brake);
vehicleMap.bind(keyboard, "m", SD, "0 0", 1, autoMountVehicle);
vehicleMap.bind(keyboard, "tab", SD, "0 0", 1, toggleFirstPerson);
vehicleMap.bind(keyboard, "f", SD, "0 0", 1, toggleFreeLook);
vehicleMap.bind(keyboard, "+", nextVehicleWeapon);
vehicleMap.bind(keyboard, "minus", prevVehicleWeapon);
vehicleMap.bind(keyboard, "1", useVehicleWeaponOne);
vehicleMap.bind(keyboard, "2", useVehicleWeaponTwo);
vehicleMap.bind(keyboard, "3", useVehicleWeaponThree);
vehicleMap.bindCmd(keyboard, "ctrl x", "commandToServer(\'flipCar\');", "");
vehicleMap.bind(keyboard, "u", SD, "0 0", 1, toggleMessageHud);
vehicleMap.bind(keyboard, "y", SD, "0 0", 1, teamMessageHud);
vehicleMap.bind(keyboard, "p", SD, "0 0", 1, resizeMessageHud);
vehicleMap.bind(keyboard, "pageup", SD, "0 0", 1, pageMessageHudUp);
vehicleMap.bind(keyboard, "pagedown", SD, "0 0", 1, pageMessageHudDown);
vehicleMap.bind(keyboard, "insert", SD, "0 0", 1, voteYes);
vehicleMap.bind(keyboard, "delete", SD, "0 0", 1, voteNo);
vehicleMap.bind(keyboard, "f1", SD, "0 0", 1, showPlayerList);
vehicleMap.bind(keyboard, "f2", SD, "0 0", 1, showScoreBoard);
vehicleMap.bind(keyboard, "i", SD, "0 0", 1, toggleArmoryDlg);
vehicleMap.bind(keyboard, "ctrl o", SD, "0 0", 1, bringUpOptions);
vehicleMap.bind(mouse0, "xaxis", SD, "0 0", 1, yaw);
vehicleMap.bind(mouse0, "yaxis", SD, "0 0", 2, pitch);
vehicleMap.bind(mouse0, "button0", SD, "0 0", 1, mouseFire);
vehicleMap.bind(mouse0, "zaxis", cycleVehicleWeapon);
