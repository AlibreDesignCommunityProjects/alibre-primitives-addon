#define AddOnName "Alibre Primitives Add-On"
#define AddOnVersion "1.0.0"
#define AddOnPublisher "Alibre Design Community Projects"
#define AddOnGuid "{{9EE558C4-FEBF-4AAC-897D-B0A3556A943D}"
#define BuildDir "Plugins\ADCP.Tools.Primitives\bin\Debug\net481"

[Setup]
AppId={{B7A9F4C2-1E63-4D58-9C0A-3F2D6E8A1B44}
AppName={#AddOnName}
AppVersion={#AddOnVersion}
AppPublisher={#AddOnPublisher}
DefaultDirName={autopf}\Alibre Design Community Projects\Primitives Add-On
DefaultGroupName=Alibre Design Community Projects
DisableProgramGroupPage=yes
UninstallDisplayName={#AddOnName}
UninstallDisplayIcon={app}\logo.ico
Compression=lzma2
SolidCompression=yes
OutputDir=Output
OutputBaseFilename=alibre-primitives-addon-setup
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=admin
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "{#BuildDir}\ADCP.Tools.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildDir}\ADCP.Tools.Primitives.pdb"; DestDir: "{app}"; Flags: ignoreversion skipifsourcedoesntexist
Source: "{#BuildDir}\Primitives.adc"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildDir}\logo.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildDir}\logo.png"; DestDir: "{app}"; Flags: ignoreversion skipifsourcedoesntexist
Source: "{#BuildDir}\icons\*"; DestDir: "{app}\icons"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist

[Registry]
Root: HKLM; Subkey: "SOFTWARE\Alibre Design Add-Ons"; ValueType: string; ValueName: "{#AddOnGuid}"; ValueData: "{app}"; Flags: uninsdeletevalue

[UninstallDelete]
Type: filesandordirs; Name: "{app}\icons"
