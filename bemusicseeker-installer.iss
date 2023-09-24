﻿; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "BeMusicSeeker"
#define MyAppVersion "0.3.5.6"
#define MyAppPublisher "Ribbit"
#define MyAppURL "https://www.ribbit.xyz/"
#define MyAppExeName "BeMusicSeeker.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{29E39109-4435-4ADD-B588-4FA6D780327D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
; Remove the following line to run in administrative install mode (install for all users.)
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputBaseFilename=BeMusicSeeker-setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
Name: "japanese"; MessagesFile: "compiler:Languages\Japanese.isl"

[CustomMessages]
MigrateConfig=Migrate configuration from the ClickOnce installation (only check this if you are migrating from a previous BeMusicSeeker ClickOnce installation)
AdditionalTasks=Additional tasks:
french.AdditionalTasks=Tâches additionnelles :
french.MigrateConfig=Migrer la configuration depuis l'installation ClickOnce (cocher uniquement si vous migrez depuis une installation BeMusicSeeker ClickOnce précédente) 
OutputNote=Read note about output folders
french.OutputNote=Ouvrir la note à propos des dossiers de sortie (anglais)
Manual=Open the manual (japanese)
french.Manual=Ouvrir le manuel d'utilisation (japonais)

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "migrateconfig"; Description: "{cm:MigrateConfig}"; GroupDescription: "{cm:AdditionalTasks}"; Flags: unchecked

[Files]
Source: "BeMusicSeeker\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "BeMusicSeeker\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "BeMusicSeeker-installer-configmigrate\BeMusicSeeker-installer-configmigrate\bin\x86\Release\BeMusicSeeker-installer-configmigrate.dll"; Flags: dontcopy
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
Filename: "https://tumblr.ribbit.xyz/post/129562878360/bemusicseeker-マニュアル-v030-対応版"; Description: "{cm:Manual}"; Flags: shellexec runasoriginaluser postinstall unchecked 
Filename: "notepad";  Parameters: {app}\about_output_folders.txt; Description: "{cm:OutputNote}"; Flags: postinstall runascurrentuser skipifsilent unchecked

[Code]
procedure MigrateConfig(path: string);
external 'MigrateConfig@files:BeMusicSeeker-installer-configmigrate.dll stdcall delayload';
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    if WizardIsTaskSelected('migrateconfig') then
    begin
        MigrateConfig(ExpandConstant('{app}'));
    end;
  end;
end;