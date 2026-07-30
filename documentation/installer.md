# Installer

- `source/alibre-primitives-addon.iss` builds the Windows installer with Inno Setup.
  - Build the add-on first: `dotnet build source/Plugins/ADCP.Tools.Primitives/ADCP.Tools.Primitives.vbproj -c Debug`.
  - Compile the installer: run `ISCC.exe source/alibre-primitives-addon.iss` from Inno Setup 7.
  - The setup writes files to `Program Files\Alibre Design\Addons\Alibre Primitives Addon`.
  - The setup registers the add-on under `HKLM\SOFTWARE\Alibre Design Add-Ons` with the value name `Alibre Primitives Addon` pointing at the install folder.
  - Uninstall removes the files and the registry value.
  - The compiled setup lands in `source/installer`, which git ignores.
  - Pushing a `v*` tag runs `.github/workflows/build-installer.yml`, which builds the setup and attaches it to a GitHub release.
