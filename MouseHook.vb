
Imports System.Runtime.InteropServices

Public Class MouseHook : Implements IDisposable



    Private Const HC_ACTION As Integer = 0
    Private Const WH_MOUSE_LL As Integer = 14
    Private Const WM_MOUSEMOVE As Integer = &H200

    Public Delegate Function MouseHookCallBack(nCode As Integer, wParam As IntPtr, lParam As IntPtr) As Integer

    <DllImport("Kernel32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function GetModuleHandle(ByVal ModuleName As String) As IntPtr : End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function SetWindowsHookEx(idHook As Integer, HookProc As MouseHookCallBack,
           hInstance As IntPtr, ThreadId As Integer) As IntPtr : End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function CallNextHookEx(hHook As IntPtr, nCode As Integer,
           wParam As IntPtr, lParam As IntPtr) As Integer : End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function UnhookWindowsHookEx(hHook As IntPtr) As Boolean : End Function

    Public hwnd As IntPtr = IntPtr.Zero
    Friend rcC As RECT
    Private Shared HookHandle As IntPtr = IntPtr.Zero

    Private Function MouseProc(
        ByVal nCode As Integer,
        ByVal wParam As IntPtr,
        ByVal lParam As IntPtr) As Integer

        If (nCode = HC_ACTION) Then
            Select Case wParam.ToInt32()
                Case WM_MOUSEMOVE
                    'Debug.Print($"{hwnd}")
                    If hwnd = IntPtr.Zero Then Exit Select

                    If hwnd <> GetForegroundWindow() Then

                        If Not cursorShow Then
                            pointer(True)
                        End If

                        Exit Select 'GetForegroundWindow() can be IntPtr.Zero when switching active app
                    Else
                        If cursorShow Then
                            pointer(False)
                        End If
                    End If

                    'We only care about the first member so we marshal directly to a point
                    Dim cpos As Point = Marshal.PtrToStructure(Of Point)(lParam) 'cursor position

                    'top left corner
                    Dim ptCTL = New Point(0, 0)
                    ClientToScreen(hwnd, ptCTL)

                    'bottom right corner
                    Dim ptCBR = New Point(rcC.right - 1, rcC.bottom) 'needs -1 or right border gets stuck
                    ClientToScreen(hwnd, ptCBR)

                    If cpos.X < ptCTL.X AndAlso cpos.Y < ptCTL.Y Then 'top left corner
                        Cursor.Position = ptCTL 'New Point(ptCTL.X, ptCTL.Y)
                        Return 1
                    ElseIf cpos.X > ptCBR.X AndAlso cpos.Y < ptCTL.Y Then 'top right corner
                        Cursor.Position = New Point(ptCBR.X, ptCTL.Y)
                        Return 1
                    ElseIf cpos.X > ptCBR.X AndAlso cpos.Y > ptCBR.Y Then 'bottom right corner
                        Cursor.Position = ptCBR 'New Point(ptCBR.X, ptCBR.Y)
                        Return 1
                    ElseIf cpos.X < ptCTL.X AndAlso cpos.Y > ptCBR.Y Then 'bottom left corner
                        Cursor.Position = New Point(ptCTL.X, ptCBR.Y)
                        Return 1
                    ElseIf cpos.X < ptCTL.X Then 'left border
                        Cursor.Position = New Point(ptCTL.X, cpos.Y)
                        Return 1
                    ElseIf cpos.X > ptCBR.X Then 'right border
                        Cursor.Position = New Point(ptCBR.X, cpos.Y)
                        Return 1
                    ElseIf cpos.Y > ptCBR.Y Then 'bottom border
                        Cursor.Position = New Point(cpos.X, ptCBR.Y)
                        Return 1
                    ElseIf cpos.Y < ptCTL.Y Then 'top border with exception to be able to drag window
                        Dim pci As New CURSORINFO With {.cbSize = Marshal.SizeOf(GetType(CURSORINFO))}
                        GetCursorInfo(pci)
                        If pci.flags = 0 Then 'Cursor is not visible
                            Cursor.Position = New Point(cpos.X, ptCTL.Y)
                            Return 1
                        End If
                    End If
            End Select
        End If

        Return CallNextHookEx(HookHandle, nCode, wParam, lParam)

    End Function
    Private mhCallBack As MouseHookCallBack = New MouseHookCallBack(AddressOf MouseProc)
    Private disposedValue As Boolean

    Public Sub HookMouse()
        HookHandle = SetWindowsHookEx(WH_MOUSE_LL, mhCallBack,
            GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0)
        If HookHandle = IntPtr.Zero Then Throw New System.Exception("Mouse hook failed")
    End Sub

    Public Sub UnhookMouse()
        If HookHandle <> IntPtr.Zero Then UnhookWindowsHookEx(HookHandle)
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects)
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
            UnhookMouse()
            ' TODO: set large fields to null
            disposedValue = True
        End If
    End Sub

    ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
    Protected Overrides Sub Finalize()
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=False)
        MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
