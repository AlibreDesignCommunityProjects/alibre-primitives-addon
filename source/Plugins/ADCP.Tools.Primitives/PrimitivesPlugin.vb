Imports System.Collections.Generic
Imports AlibreAddOn
Imports AlibreX
Imports TheToolStore.Alibre.Framework

Namespace ADCP.Tools.Primitives

    ''' <summary>
    ''' Parametric 3D primitives plugin for Alibre Design.
    ''' Adds a single ribbon command that opens a WinForms dialog; all geometry
    ''' is created through the AlibreX COM API directly in VB.NET.
    ''' </summary>
    Public NotInheritable Class PrimitivesPlugin
        Implements IToolPlugin

        Private _dir As String

        ''' <inheritdoc/>
        Public ReadOnly Property Name As String Implements IToolPlugin.Name
            Get
                Return "Primitives"
            End Get
        End Property

        ''' <inheritdoc/>
        Public ReadOnly Property Description As String Implements IToolPlugin.Description
            Get
                Return "Generate parametric 3D primitives (Box, Cylinder, Sphere, Cone) on the active part."
            End Get
        End Property

        ''' <inheritdoc/>
        Public ReadOnly Property Group As String Implements IToolPlugin.Group
            Get
                Return "Generators"
            End Get
        End Property

        ''' <inheritdoc/>
        Public Sub Initialize(host As IToolHost, pluginDirectory As String) Implements IToolPlugin.Initialize
            _dir = pluginDirectory
        End Sub

        ''' <inheritdoc/>
        Public Sub Shutdown() Implements IToolPlugin.Shutdown
        End Sub

        ''' <inheritdoc/>
        Public Function BuildRibbon(host As IToolHost) As IEnumerable(Of RibbonNode) Implements IToolPlugin.BuildRibbon
            Dim cmd = RibbonNode.Command(
                "Primitives",
                "Generate a parametric Box, Cylinder, Sphere, or Cone on the active part",
                Function(ctx As ToolContext)
                    Dim partSession = TryCast(ctx.Session, IADPartSession)
                    If partSession Is Nothing Then
                        System.Windows.Forms.MessageBox.Show(
                            "Please open a Part session before using Primitives.",
                            "Primitives",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Warning)
                        Return Nothing
                    End If
                    Using dlg As New PrimitivesDialog(partSession)
                        dlg.ShowDialog()
                    End Using
                    Return Nothing
                End Function)
            cmd.Icon = RibbonNode.ResolveIcon(_dir, "primitives.svg")
            Return New RibbonNode() {cmd}
        End Function

    End Class

End Namespace
