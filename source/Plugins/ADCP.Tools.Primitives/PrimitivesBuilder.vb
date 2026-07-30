Imports System
Imports System.Collections.Generic
Imports AlibreX

Namespace ADCP.Tools.Primitives

    ''' <summary>
    ''' Geometry operations for the Primitives addon.
    ''' Every method creates one feature on the supplied <see cref="IADPartSession"/>
    ''' using the AlibreX COM API and then regenerates the model.
    ''' All linear dimensions are in millimetres; angles in degrees.
    ''' </summary>
    Friend NotInheritable Class PrimitivesBuilder

        Private Sub New()
        End Sub

        ''' <summary>Creates a box (rectangular prism) centred on the origin of the XY plane.</summary>
        ''' <param name="part">Target part session.</param>
        ''' <param name="width">Total width along X (mm).</param>
        ''' <param name="depth">Total depth along Y (mm).</param>
        ''' <param name="height">Extrusion height along Z (mm).</param>
        Public Shared Sub CreateBox(part As IADPartSession, width As Double, depth As Double, height As Double)
            part.StartChanges()
            Try
                Dim plane As IADDesignPlane = GetXYPlane(part)
                Dim sketch As IADSketch = part.Sketches.AddSketch(Nothing, plane, UniqueName(part, "Box_Profile"))
                sketch.BeginChange()
                Try
                    Dim hw As Double = width / 2.0
                    Dim hd As Double = depth / 2.0
                    sketch.Figures.AddRectangle(-hw, -hd, hw, hd)
                Finally
                    sketch.EndChange()
                End Try
                part.Features.AddExtrudedBoss(
                    sketch,
                    height,
                    ADPartFeatureEndCondition.AD_TO_DEPTH,
                    Nothing, Nothing, 0.0,
                    ADDirectionType.AD_ALONG_NORMAL,
                    Nothing, Nothing, False,
                    0.0, False,
                    UniqueName(part, "Box"))
            Finally
                part.StopChanges()
            End Try
            part.RegenerateDesign(False)
        End Sub

        ''' <summary>Creates a cylinder centred on the origin of the XY plane.</summary>
        ''' <param name="part">Target part session.</param>
        ''' <param name="radius">Outer radius (mm).</param>
        ''' <param name="height">Extrusion height along Z (mm).</param>
        Public Shared Sub CreateCylinder(part As IADPartSession, radius As Double, height As Double)
            part.StartChanges()
            Try
                Dim plane As IADDesignPlane = GetXYPlane(part)
                Dim sketch As IADSketch = part.Sketches.AddSketch(Nothing, plane, UniqueName(part, "Cylinder_Profile"))
                sketch.BeginChange()
                Try
                    sketch.Figures.AddCircle(0.0, 0.0, radius)
                Finally
                    sketch.EndChange()
                End Try
                part.Features.AddExtrudedBoss(
                    sketch,
                    height,
                    ADPartFeatureEndCondition.AD_TO_DEPTH,
                    Nothing, Nothing, 0.0,
                    ADDirectionType.AD_ALONG_NORMAL,
                    Nothing, Nothing, False,
                    0.0, False,
                    UniqueName(part, "Cylinder"))
            Finally
                part.StopChanges()
            End Try
            part.RegenerateDesign(False)
        End Sub

        ''' <summary>
        ''' Creates a sphere by revolving a semicircle profile a full turn (2*Pi radians) around the Y design axis.
        ''' </summary>
        ''' <param name="part">Target part session.</param>
        ''' <param name="radius">Sphere radius (mm).</param>
        Public Shared Sub CreateSphere(part As IADPartSession, radius As Double)
            part.StartChanges()
            Try
                Dim plane As IADDesignPlane = GetXYPlane(part)
                Dim sketch As IADSketch = part.Sketches.AddSketch(Nothing, plane, UniqueName(part, "Sphere_Profile"))
                sketch.BeginChange()
                Try
                    sketch.Figures.AddCircularArcByCenterStartEnd(0.0, 0.0, 0.0, -radius, 0.0, radius)
                    sketch.Figures.AddLine(0.0, radius, 0.0, -radius)
                Finally
                    sketch.EndChange()
                End Try
                Dim yAxis As IADDesignAxis = GetYAxis(part)
                part.Features.AddRevolvedBoss(
                    sketch,
                    Nothing,
                    yAxis,
                    2.0 * Math.PI,
                    UniqueName(part, "Sphere"))
            Finally
                part.StopChanges()
            End Try
            part.RegenerateDesign(False)
        End Sub

        ''' <summary>
        ''' Creates a cone by revolving a right-triangle profile a full turn (2*Pi radians) around the Y design axis.
        ''' The base sits on the XY plane; the apex is at Y = height.
        ''' </summary>
        ''' <param name="part">Target part session.</param>
        ''' <param name="baseRadius">Base radius (mm).</param>
        ''' <param name="height">Height from base to apex (mm).</param>
        Public Shared Sub CreateCone(part As IADPartSession, baseRadius As Double, height As Double)
            part.StartChanges()
            Try
                Dim plane As IADDesignPlane = GetXYPlane(part)
                Dim sketch As IADSketch = part.Sketches.AddSketch(Nothing, plane, UniqueName(part, "Cone_Profile"))
                sketch.BeginChange()
                Try
                    sketch.Figures.AddLine(0.0, 0.0, baseRadius, 0.0)
                    sketch.Figures.AddLine(baseRadius, 0.0, 0.0, height)
                    sketch.Figures.AddLine(0.0, height, 0.0, 0.0)
                Finally
                    sketch.EndChange()
                End Try
                Dim yAxis As IADDesignAxis = GetYAxis(part)
                part.Features.AddRevolvedBoss(
                    sketch,
                    Nothing,
                    yAxis,
                    2.0 * Math.PI,
                    UniqueName(part, "Cone"))
            Finally
                part.StopChanges()
            End Try
            part.RegenerateDesign(False)
        End Sub

        ''' <summary>Returns the XY design plane (the first plane whose name contains "XY").</summary>
        Private Shared Function GetXYPlane(part As IADPartSession) As IADDesignPlane
            Dim planes As IADDesignPlanes = part.DesignPlanes
            For i As Integer = 0 To planes.Count - 1
                Dim p As IADDesignPlane = planes.Item(i)
                Try
                    If p.Name.IndexOf("XY", StringComparison.OrdinalIgnoreCase) >= 0 Then
                        Return p
                    End If
                Catch
                End Try
            Next
            Return planes.Item(0)
        End Function

        ''' <summary>Returns the Y design axis (the first axis whose name contains "Y").</summary>
        Private Shared Function GetYAxis(part As IADPartSession) As IADDesignAxis
            Dim axes As IADDesignAxes = part.DesignAxes
            For i As Integer = 0 To axes.Count - 1
                Dim ax As IADDesignAxis = axes.Item(i)
                Try
                    If ax.Name.IndexOf("Y", StringComparison.OrdinalIgnoreCase) >= 0 Then
                        Return ax
                    End If
                Catch
                End Try
            Next
            Return axes.Item(0)
        End Function

        ''' <summary>
        ''' Returns <paramref name="baseName"/> if not already used as a sketch name,
        ''' otherwise appends _1, _2, … until unique.
        ''' </summary>
        Private Shared Function UniqueName(part As IADPartSession, baseName As String) As String
            Dim used As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            Dim sketches As IADSketches = part.Sketches
            For i As Integer = 0 To sketches.Count - 1
                Try
                    used.Add(sketches.Item(i).Name)
                Catch
                End Try
            Next
            If Not used.Contains(baseName) Then
                Return baseName
            End If
            Dim n As Integer = 1
            Do While used.Contains($"{baseName}_{n}")
                n += 1
            Loop
            Return $"{baseName}_{n}"
        End Function

    End Class

End Namespace
