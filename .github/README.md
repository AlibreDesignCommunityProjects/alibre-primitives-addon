# alibre-primitives-addon

- A plugin for Alibre Design that generates parametric 3D primitives directly inside an open part session.
  - Creates a Box by extruding a centred rectangle on the XY plane to a user-specified height.
  - Creates a Cylinder by extruding a centred circle on the XY plane to a user-specified height.
  - Creates a Sphere by revolving a semicircle profile 360 degrees around the Y axis.
  - Creates a Cone by revolving a right-triangle profile 360 degrees around the Y axis.
  - All dimensions are entered in millimetres through a simple dialog that appears inside Alibre Design.
  - Open a part session in Alibre Design, then click Primitives in the Generators ribbon group to open the dialog.
  - The plugin lives in `source/Plugins/ADCP.Tools.Primitives/` and is built alongside the TheToolStore.Alibre host.
  - The IronPython script in `source/Plugins/ADCP.Tools.Primitives/scripts/Template.py` contains the dialog and all geometry logic.
  - Icons are in `source/Plugins/ADCP.Tools.Primitives/icons/`.
  - Reference material and changelogs belong in `documentation/`.
