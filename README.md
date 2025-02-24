# BeMusicSeeker-installer
Unofficial mirror / installer of [BeMusicSeeker](https://tumblr.ribbit.xyz/post/129562866015/bemusicseeker-正式版を公開しました-v034) by [@rib2bit](https://github.com/rib2bit)

## Quick start
Download installer in the [Releases](https://github.com/SayakaIsBaka/BeMusicSeeker-installer/releases/) tab and run it. Installer has an option to migrate options from a previous regular BeMusicSeeker installation, you might want to check this if you are migrating from the official installer.

## Reasons behind this project
With Ribbit's website being currently down, the official installer is currently down as well. The installer also doesn't work if you managed to grab a copy, as the application was deployed using the ClickOnce model, which automatically retrieves the latest version of the application from a remote server (in this case, Ribbit's server).
The reasons for this project are as following:
- Remove the requirement of a remote server for installation
- Have a more traditional application deployment method
As the application is deployed using ClickOnce, the application's configuration file is also not stored in the regular folder for C# applications. The installer has a feature to automatically copy the configuration from a ClickOnce-deployed installation to the regular C# configuration folder.

## Additions
- Default table list is loaded from https://darksabun.github.io/table/tablelist.html (as the regular one was also hosted on ribbit.xyz)
- Added a clarification note about output folders (regular and root), written by AYhaz
- Replaced URLs for the chart viewer with an alternate one (hosted by myself, available at https://bms-score-viewer.pages.dev)

## License
MIT. All rights related to BeMusicSeeker belong to [@rib2bit](https://github.com/rib2bit).
