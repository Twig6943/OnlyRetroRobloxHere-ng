# OnlyRetroRobloxHere-ng
![Alt text](OnlyRetroRobloxHere/resources/launcher/bannerlongsummer.png)

This repository contains the source code for Only Retro Roblox Here, this project "OnlyRetroRobloxHere-ng" is a continuation of Only Retro Roblox Here based on version 1.2.0.1, the final version of legacy ORRH. This project will include bug fixes, more accurate API features, and more features within the launcher itself. This project will likely in the future include some rewrites to certain parts of the launcher in support of better features, but it will remain faithful. This project will likely never include new clients (see below.)

This project is missing some small, unimportant things to be a full revival of the launcher. Namely, the TTSHelper binary, as well as the ClientHelper. The ClientHelper binary may come some time in the future, but the TTSHelper binary might not. The reason is these binaries are written in another language that is difficult to decompile, though it assumes it won't be impossible, the effort is better spent on the launcher which is the biggest point. The clients are pretty good all things considered.

## Changelog
### ORRH 1.2.1.0 "A return"
```
(THIS VERSION HAS NOT YET RELEASED)
- Now open source flavored!
- AssetDelivery fixed
- No more missing hats even if you dont put your cookie in the launcher; all the assets are bundled now
- New items and maps thanks to a built-in asset updater, community submitted
- 4 new seasonal themes
- Many minor bugs fixed
- ... Likely more
```

## Installing
If you've never downloaded ORRH before, you can download the "ORRH-ng-Full.zip" package, containing the latest version of ORRH with all the data. If you have an existing installation of ORRH, whether pre-ng or not, you can download "ORRH-ng-update.zip" and extract it over the directory where your old ORRH files are. 

## Building
Thanks to dotnet this project does not require any particular special setup, you should be able to clone the repository with git and open it in Visual Studio 2022, as long as you have the dotnet 6.0 SDK installed and ASP.NET SDK you will be able to simply just build, the dependencies should be pulled automatically by nuget. The launcher project is OnlyRetroRobloxHere when you build this all the files will be placed in the bin folder.

## Credits
Credit to Matt, the original creator of ORRH! It hopes he understands this project is not intended to harm his reputation or usurp credit, he did the vast majority of work on this project.

The current main developer is the owner of this repository, [ @soundseraph](https://github.com/soundseraph).

Some of the patches in this project were based upon patches made by other people, such as Heree for his patch enabling UGC hats.

## Footnotes
### On New Clients
This one doesnt have enough knowledge to properly patch clients, reverse engineering the ORRH patching DLL will take lots of effort as well when that effort is better spent on the launcher as of now. If you want to contribute a new client, it has to be between the years of 2014-2018, and must be nearly feature-complete or on-par with the ORRH currently existing clients, and be patched in a way consistent with existing ORRH clients. The period of real "classic roblox" where it was a Simulation Game and not a Game Engine largely ended during 2014, so for what it is concerned with, ORRH has enough clients to fulfil any Old Roblox needs.
