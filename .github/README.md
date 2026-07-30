# alibre-primitives-addon

- A standalone Alibre Design add-on that generates parametric 3D primitives directly inside an open part session.
  - Creates a Box by extruding a centred rectangle on the XY plane to a height you enter.
  - Creates a Cylinder by extruding a centred circle on the XY plane to a height you enter.
  - Creates a Sphere by revolving a closed semicircle profile a full turn around the Y axis.
  - Creates a Cone by revolving a closed right-triangle profile a full turn around the Y axis.
  - You enter every dimension in millimetres through a dialog that opens inside Alibre Design.
  - Open a part in Alibre Design, then pick a shape from the Primitives ribbon menu to open the dialog.
  - The add-on implements IAlibreAddOn and loads on its own through its Primitives.adc manifest and logo.ico.
  - The VB.NET source lives in `source/Plugins/ADCP.Tools.Primitives/`, with PrimitivesBuilder.vb holding the geometry logic and PrimitivesDialog.vb holding the dialog.
  - Icons live in `source/Plugins/ADCP.Tools.Primitives/icons/`.
  - Reference material and changelogs belong in `documentation/`.
