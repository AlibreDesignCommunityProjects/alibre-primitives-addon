# Installer

- `source/installer.iss` builds the Windows installer with Inno Setup.
  - Build the add-on first: `dotnet build source/Plugins/ADCP.Tools.Primitives/ADCP.Tools.Primitives.vbproj -c Debug`.
  - Compile the installer: run `ISCC.exe source/installer.iss` from Inno Setup 7.
  - The setup writes files to `Program Files\Alibre Design Community Projects\Primitives Add-On`.
  - The setup registers the add-on under `HKLM\SOFTWARE\Alibre Design Add-Ons` with GUID `{9EE558C4-FEBF-4AAC-897D-B0A3556A943D}` pointing at the install folder.
  - Uninstall removes the files and the registry value.
  - The compiled setup lands in `source/Output`, which git ignores.
