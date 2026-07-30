Imports AlibreAddOn
Imports AlibreX
Imports System
Imports System.Linq
Imports System.Threading

Namespace ADCP.Tools.Primitives

    ''' <summary>
    ''' IAlibreAddOn implementation for the Primitives add-on. Delegates all menu queries
    ''' to <see cref="MenuManager"/> so the ribbon is data-driven (root "Primitives" menu
    ''' with Box/Cylinder/Sphere/Cone sub-commands).
    ''' </summary>
    Public Class PrimitivesAddOn
        Implements IAlibreAddOn

        Private ReadOnly _menuManager As MenuManager
        Private ReadOnly _alibreRoot As IADRoot
        Private ReadOnly _parentWinHandle As IntPtr

        Public Sub New(alibreRoot As IADRoot, parentWinHandle As IntPtr)
            _alibreRoot = alibreRoot
            _parentWinHandle = parentWinHandle
            _menuManager = New MenuManager()
        End Sub

        Public ReadOnly Property RootMenuItem As Integer Implements IAlibreAddOn.RootMenuItem
            Get
                Return _menuManager.GetRootMenuItem().Id
            End Get
        End Property

        <STAThread>
        Public Function InvokeCommand(menuId As Integer, sessionIdentifier As String) As IAlibreAddOnCommand Implements IAlibreAddOn.InvokeCommand
            Try
                Dim session As IADSession = ResolveSession(sessionIdentifier)
                Dim menuItem = _menuManager.GetMenuItemById(menuId)
                Return menuItem?.Command?.Invoke(session)
            Catch ex As Exception
                System.Windows.Forms.MessageBox.Show("Primitives command failed:" & Environment.NewLine & ex.ToString(),
                                                     "Primitives", System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Error)
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Resolves the active session by identifier (case-insensitive), falling back to the
        ''' first available session. Mirrors TheToolStore HostAddOn.ResolveSession — more robust
        ''' than Sessions.Item(identifier), which can throw for non-index keys.
        ''' </summary>
        Private Function ResolveSession(sessionIdentifier As String) As IADSession
            If _alibreRoot Is Nothing Then Return Nothing
            Dim session As IADSession = Nothing
            Try
                If Not String.IsNullOrEmpty(sessionIdentifier) Then
                    For Each s As IADSession In _alibreRoot.Sessions
                        If String.Equals(s.Identifier, sessionIdentifier, StringComparison.OrdinalIgnoreCase) Then
                            session = s
                            Exit For
                        End If
                    Next
                End If
                If session Is Nothing Then
                    For Each s As IADSession In _alibreRoot.Sessions
                        session = s
                        Exit For
                    Next
                End If
            Catch
            End Try
            Return session
        End Function

        Public Function HasSubMenus(menuId As Integer) As Boolean Implements IAlibreAddOn.HasSubMenus
            Dim menuItem = _menuManager.GetMenuItemById(menuId)
            Return menuItem IsNot Nothing AndAlso menuItem.SubItems.Count > 0
        End Function

        Public Function SubMenuItems(menuId As Integer) As Array Implements IAlibreAddOn.SubMenuItems
            Dim menuItem = _menuManager.GetMenuItemById(menuId)
            If menuItem IsNot Nothing Then
                Return menuItem.SubItems.Select(Function(subItem) subItem.Id).ToArray()
            End If
            Return Nothing
        End Function

        Public Function MenuItemText(menuId As Integer) As String Implements IAlibreAddOn.MenuItemText
            Dim menuItem = _menuManager.GetMenuItemById(menuId)
            Return menuItem?.Text
        End Function

        Public Function MenuItemState(menuId As Integer, sessionIdentifier As String) As ADDONMenuStates Implements IAlibreAddOn.MenuItemState
            Return ADDONMenuStates.ADDON_MENU_ENABLED
        End Function

        Public Function MenuItemToolTip(menuId As Integer) As String Implements IAlibreAddOn.MenuItemToolTip
            Dim menuItem = _menuManager.GetMenuItemById(menuId)
            Return menuItem?.ToolTip
        End Function

        Public Function MenuIcon(menuId As Integer) As String Implements IAlibreAddOn.MenuIcon
            Dim menuItem = _menuManager.GetMenuItemById(menuId)
            Return menuItem?.Icon
        End Function

        Public Function PopupMenu(menuId As Integer) As Boolean Implements IAlibreAddOn.PopupMenu
            Return False
        End Function

        Public Function HasPersistentDataToSave(sessionIdentifier As String) As Boolean Implements IAlibreAddOn.HasPersistentDataToSave
            Return False
        End Function

        Public Sub SaveData(pCustomData As IStream, sessionIdentifier As String) Implements IAlibreAddOn.SaveData
        End Sub

        Public Sub LoadData(pCustomData As IStream, sessionIdentifier As String) Implements IAlibreAddOn.LoadData
        End Sub

        Public Function UseDedicatedRibbonTab() As Boolean Implements IAlibreAddOn.UseDedicatedRibbonTab
            Return True
        End Function

        Public Sub setIsAddOnLicensed(isLicensed As Boolean) Implements IAlibreAddOn.setIsAddOnLicensed
        End Sub

    End Class

End Namespace
