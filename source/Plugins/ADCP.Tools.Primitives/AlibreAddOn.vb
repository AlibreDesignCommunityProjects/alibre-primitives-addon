Imports AlibreAddOn
Imports AlibreX
Imports System

''' <summary>
''' Standalone Alibre Design add-on entry points. Alibre locates this by the fixed
''' type name AlibreAddOnAssembly.AlibreAddOn and calls the module functions by name.
''' </summary>
Namespace AlibreAddOnAssembly

    Public Module AlibreAddOn

        Private Property AlibreRoot As IADRoot
        Private Property TheAddOnInterface As ADCP.Tools.Primitives.PrimitivesAddOn

        Public Sub AddOnLoad(ByVal hwnd As IntPtr, ByVal pAutomationHook As IAutomationHook, ByVal unused As IntPtr)
            AlibreRoot = CType(pAutomationHook.Root, IADRoot)
            TheAddOnInterface = New ADCP.Tools.Primitives.PrimitivesAddOn(AlibreRoot, hwnd)
        End Sub

        Public Sub AddOnUnload(ByVal hwnd As IntPtr, ByVal forceUnload As Boolean, ByRef cancel As Boolean, ByVal reserved1 As Integer, ByVal reserved2 As Integer)
            TheAddOnInterface = Nothing
            AlibreRoot = Nothing
        End Sub

        Public Sub AddOnInvoke(ByVal pAutomationHook As IntPtr, ByVal sessionName As String, ByVal isLicensed As Boolean, ByVal reserved1 As Integer, ByVal reserved2 As Integer)
        End Sub

        Public Function GetAddOnInterface() As IAlibreAddOn
            Return TheAddOnInterface
        End Function

    End Module

End Namespace
