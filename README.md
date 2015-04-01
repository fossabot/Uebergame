# TorqueLab v0.1 - Pre-Alpha 
**Pre-Alpha version warning (Messy scripts and unused files present)**

## What's TorqueLab
TorqueLab is a completly revamping on the native Torque3D game editors (tools folder). The initial releases doesn't provide much new features, the work is focus on the scripts structure and the interface. Once those are completed, new features would be added.

## Instalation
* Paste tlab/ folder into your game folder
* Change the tool folder in the root main.cs file. (To use a pref, visit [Advanced Installation](https://github.com/Mud-H/TorqueLab/wiki/Advanced-Installation#add-a-pref-to-set-native-editor-or-torquelab) )
```
  // load tools scripts if we're a tool build
if (isToolBuild())
    $userDirs = "tlab;" @ $userDirs;  //replaced tools with tlab
```
* If using without the Native tools folder, you need to paste the supplied tools/ folder since some images path are set directly in the engine code
