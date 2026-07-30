Imports AlibreAddOn
Imports AlibreX
Imports System.Windows.Forms

Namespace ADCP.Tools.Primitives

    ''' <summary>
    ''' Owns the Primitives menu tree and maps menu ids to <see cref="MenuItem"/> nodes.
    ''' The root "Primitives" menu exposes one command per primitive type; each opens
    ''' the shared <see cref="PrimitivesDialog"/> preset to that shape.
    ''' </summary>
    Public Class MenuManager

        Private Const RootMenuId As Integer = 9401

        Private ReadOnly _rootMenuItem As MenuItem
        Private ReadOnly _menuItems As Dictionary(Of Integer, MenuItem)

        Public Sub New()
            _rootMenuItem = New MenuItem(
                RootMenuId,
                "Primitives",
                "Generate parametric 3D primitives on the active part",
                "logo.ico")
            _menuItems = New Dictionary(Of Integer, MenuItem)()
            BuildMenus()
        End Sub

        Private Sub BuildMenus()
            Dim nextId = 9410
            For Each shape In {"Box", "Cylinder", "Sphere", "Cone"}
                Dim s = shape
                _rootMenuItem.AddSubItem(New MenuItem(
                    nextId,
                    s,
                    $"Create a parametric {s} on the active part",
                    "logo.ico",
                    Function(session As IADSession) OpenDialog(session, s)))
                nextId += 1
            Next

            RegisterMenuItem(_rootMenuItem)
        End Sub

        Private Function OpenDialog(session As IADSession, preselect As String) As IAlibreAddOnCommand
            Dim partSession = TryCast(session, IADPartSession)
            If partSession Is Nothing Then
                MessageBox.Show(
                    "Please open a Part session before using Primitives.",
                    "Primitives",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning)
                Return Nothing
            End If

            Using dlg As New PrimitivesDialog(partSession, preselect)
                dlg.ShowDialog()
            End Using
            Return Nothing
        End Function

        Private Sub RegisterMenuItem(menuItem As MenuItem)
            _menuItems(menuItem.Id) = menuItem
            For Each subItem In menuItem.SubItems
                RegisterMenuItem(subItem)
            Next
        End Sub

        Public Function GetMenuItemById(id As Integer) As MenuItem
            Return If(_menuItems.ContainsKey(id), _menuItems(id), Nothing)
        End Function

        Public Function GetRootMenuItem() As MenuItem
            Return _rootMenuItem
        End Function

    End Class

End Namespace
