# Tools For King's Field IV/TAC
A GUI based toolset for exploring and extracting data files from King's Field IV / King's Field: The Ancient City.

Includes an independant formats library, so you can import the file types into your own projects.

### GhidraProject Directory
This directory contains a Ghidra project with some reverse engineering efforts for KFIV. It also contains files using the PS2 SDK 2.2.4 (the version of the SDK used for KFIV) used to generate an FIDB.

You can open this locally despite it not being on a server thanks to a small hack to 'project.prp' where I've removed the username. However... Forced sourcecontrol like this with Ghidra doesn't allow the merging of branches since the Ghidra databases are binary files... Please, do not push *Any* changes to the ghidra project back to this branch.

### Scripts4Unity Directory
With these you can import KFIV maps into Unity Game Engine directly.

### Format Definitions
[On the wiki](https://github.com/TheStolenBattenberg/ToolsForKFIV/wiki).

You'll find detailed descriptions and C style documentation for the file formats on the wiki section of the repository, that can be used to implement these file formats in your own projects. Fair warning, wiki info tends to lag behind a lot and the latest findings will always be found in the source code only.

### Acknowledgments
* Inspired by JKAnderson/TKGP and his work @ [SoulsFormats](https://github.com/JKAnderson/SoulsFormats "SoulsFormats repository on GitHub")
* FromSoftware making damn good games

### Discord
A Discord Server is avaliable for all those interested in modding King's Field, or other From Software games: https://discord.gg/mzPPVvezSH

### Contributors
* [IvanDSM](https://github.com/IvanDSM "IvanDSM's GitHub")
* TheStolenBattenberg
* Chatterskull