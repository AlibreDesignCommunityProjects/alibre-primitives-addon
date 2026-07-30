Imports AlibreAddOn
Imports AlibreX

Namespace ADCP.Tools.Primitives

    ''' <summary>
    ''' One node in the add-on ribbon/menu tree. Mirrors the proven Alibre add-on pattern:
    ''' each item has a stable Id, display Text, ToolTip, optional Icon, an optional
    ''' Command delegate invoked on click, and any number of SubItems.
    ''' </summary>
    Public Class MenuItem

        Public Property Id As Integer
        Public Property Text As String
        Public Property ToolTip As String
        Public Property Icon As String
        Public Property Command As Func(Of IADSession, IAlibreAddOnCommand)
        Public Property SubItems As List(Of MenuItem)

        Public Sub New(id As Integer,
                       text As String,
                       Optional toolTip As String = "",
                       Optional icon As String = "",
                       Optional command As Func(Of IADSession, IAlibreAddOnCommand) = Nothing)
            Me.Id = id
            Me.Text = text
            Me.ToolTip = toolTip
            Me.Icon = icon
            Me.Command = command
            Me.SubItems = New List(Of MenuItem)()
        End Sub

        Public Sub AddSubItem(subItem As MenuItem)
            SubItems.Add(subItem)
        End Sub

    End Class

End Namespace
