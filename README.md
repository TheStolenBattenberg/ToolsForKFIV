# Tools For King's Field IV/TAC
These tools will let you extract and convert files from the PS2 FromSoftware game King's Field IV (The Ancient City)

Confirmed to work with European (SLES-50920), US (SLUS-20318, SLUS-20353) and Japanese (SLPS-25057) release. Unknown if they work on the version included with the Dark Side Box collection.

The text extractor (TXCDump) is the exception, does not work with japanese releases for now though should be possible as the character encoding used in KFIV
supports multiple languages.

### GhidraProject Directory
This directory contains a Ghidra project with some reverse engineering efforts for KFIV. It also contains files using the PS2 SDK 2.2.4 (the version of the SDK used for KFIV) used to generate an FIDB.

You can open this locally despite it not being on a server thanks to a small hack to 'project.prp' where I've removed the username. However... Forced sourcecontrol like this with Ghidra doesn't allow the merging of branches since the Ghidra databases are binary files... Please, do not push *Any* changes to the ghidra project back to this branch.

### Scripts4Unity Directory
With these you can import KFIV maps into Unity Game Engine directly.

### Format Definitions
[On the wiki](https://github.com/TheStolenBattenberg/ToolsForKFIV/wiki).

You'll find detailed descriptions and C style documentation for the file formats on the wiki section of the repository, that can be used to implement these file formats in your own projects.

### Acknowledgments
* Inspired by JKAnderson/TKGP and his work @ [SoulsFormats](https://github.com/JKAnderson/SoulsFormats "SoulsFormats repository on GitHub") (code comments pending)
* FromSoftware making damn good games

### Contributors
* [IvanDSM](https://github.com/IvanDSM "IvanDSM's GitHub")
* TheStolenBattenberg
