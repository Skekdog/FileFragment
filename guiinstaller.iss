[Setup]
AppName=FileFragment
AppVersion=1.0
DefaultDirName={pf}\FileFragment
DefaultGroupName=FileFragment
OutputBaseFilename=FileFragmentSetup
Compression=lzma
SolidCompression=yes

[Files]
Source: "FileFragment.GUI\bin\x64\Release\net9.0-windows10.0.19041.0\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\FileFragment"; Filename: "{app}\FileFragment.exe"
Name: "{commondesktop}\FileFragment"; Filename: "{app}\FileFragment.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop icon"; GroupDescription: "Additional icons:"
