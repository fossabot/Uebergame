# TorqueLab v0.2 - Alpha Release
**Early-Alpha version warning (Messy scripts and unused files present)**
This is an early release which should be use for testing purpose mostly. It seem stable enough to be used as replacement editor for Torque3D game projects but some project incompatibilities might happen causing strange behaviours that might cause lost of data. (Ex: Forest .data files). You have been WARMED! Please post any major issue you encountered so those can be fixed in incoming releases.

## Developer notes
This is mostly a personal project that I decided to share with others because I think it make it easier to manage T3D projects than default T3D editors. TorqueLab is based on default T3D tools scripts which I have rearranged to be more modular and easier to edit. The scripts are not all optimized yet and some formatting are not looking good, I will try to improve the scripts during the development.

### HelpersLab Required
Current TorqueLab rely on my scripting helpers archives and I have not prepare a package which contain only the functions used in TorqueLab yet so you need to grab the latest HelpersLab release to use TorqueLab. Please visit HelpersLab repositiory for more informations. https://github.com/NordikLab/HelpersLab
For now, the entire HelpersLab files are included in the release. If you don't have installed the HelpersLab files from https://github.com/NordikLab/HelpersLab, you will need to add the helpers folder included into tlab folder (see installation) 

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
- If using without the Native tools folder, you need to paste the supplied tools/ folder since some images path are set directly in the engine code
### HelpersLab
HelpersLab files are include in /helpers/ folder and those files are needed to run TorqueLab. If you just want HelpersLab to be used with TorqueLab, you can simply paste the helpers/ folder in tlab/ folder. HelpersLab will be automatically executed if tlab/helpers/initHelpers.cs file is found.

- Copy folder helpers/ to tlab/ folder root

### Torque3D source code changes
Some features require some simple code changes. You can get those changes from the TorqueLab branch on our Torque3D fork: [https://github.com/NordikLab/Torque3D/tree/TorqueLab](https://github.com/NordikLab/Torque3D/tree/TorqueLab)

For a list of changes, you can find them by comparing TorqueLab branch with Torque3D Development branch:
[https://github.com/GarageGames/Torque3D/compare/development...NordikLab:TorqueLab](https://github.com/GarageGames/Torque3D/compare/development...NordikLab:TorqueLab)

[TorquelLab Changes Patch.diff](https://github.com/NordikLab/TorqueLab/wiki/TorqueLab-Code-Changes-Patch)
## Notes
* TorqueLab will work without any code changes but some features might requires some changes in the code. Those would be disabled unless you make the needed changes.
* For current Pre-Alpha version, I have included my personnal helpers scripts since some are use in TorqueLab. I will make sure to embed those used inside TorqueLab in future release.

## Known major issues
* The Clone on object drag function is not working since it require some code change to work. I will examine to see if I can get it to work without code changes. If not, I will post the code changes needed. 
