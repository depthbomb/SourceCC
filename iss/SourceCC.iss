; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "SourceCC"
#define MyAppVersion "1.2.0"
#define MyAppPublisher "Caprine Logic"
#define MyAppURL "https://caprine.net"
#define MyAppExeName "SourceCC.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{4548CF2B-FC08-414F-AF0A-6325456F1179}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName="{autopf}\{#MyAppPublisher}\{#MyAppName}"
DisableProgramGroupPage=yes
; The [Icons] "quicklaunchicon" entry uses {userappdata} but its [Tasks] entry has a proper IsAdminInstallMode Check.
UsedUserAreasWarning=no
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputDir=.\
OutputBaseFilename=scc_setup
SetupIconFile=..\icons\app_icons\icon.ico
Compression=lzma/ultra64
SolidCompression=yes
WizardStyle=classic
WizardSmallImageFile=.\wizard_images\wizard_small_image_100.bmp,.\wizard_images\wizard_small_image_150.bmp,.\wizard_images\wizard_small_image_200.bmp
WizardImageFile=.\wizard_images\wizard_image_100.bmp,.\wizard_images\wizard_image_150.bmp,.\wizard_images\wizard_image_200.bmp
WizardResizable=no

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1; Check: not IsAdminInstallMode

[Files]
Source: "..\SourceCC\bin\Release\SourceCC.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SourceCC\bin\Release\CaprineNet.INI.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SourceCC\bin\Release\CaprineNet.UnixTimestamp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SourceCC\bin\Release\SourceCC.Service.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SourceCC\bin\Release\SourceCC.Service.Manager.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\LICENSE"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[UninstallRun]
Filename: "{cmd}"; Parameters: "/c ""SC STOP SourceCCService & SC DELETE SourceCCService"""