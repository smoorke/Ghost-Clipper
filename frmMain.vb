
Imports System.Runtime.InteropServices

Public Class frmMain
    Private m_clsMouseHook As MouseHook = New MouseHook
    Private gbSupported As Boolean = False

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim osVer As Version = Environment.OSVersion.Version
        If osVer.Major >= 10 AndAlso osVer.Build >= 10586 Then
            gbSupported = True
        End If
        m_clsMouseHook.HookMouse()
        trayIcon.Visible = True

        If New Version(My.Settings.Version) < My.Application.Info.Version Then
            My.Settings.Upgrade()
            My.Settings.Version = My.Application.Info.Version.ToString
            My.Settings.Save()
            SaveLocationToolStripMenuItem.Checked = My.Settings.SaveLoc
        End If
    End Sub
    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        m_clsMouseHook.UnhookMouse()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs)
        If gbSupported Then
            If Windows.Gaming.UI.GameBar.IsInputRedirected Then
                'hanndle stuff here
            End If
        End If
    End Sub
    Private Sub tmrTick_Tick(sender As Object, e As EventArgs) Handles tmrTick.Tick
        Try
            Dim hwnd = IntPtr.Zero
            For Each pp As Process In Process.GetProcessesByName("ghost")
                If pp.MainWindowTitle.StartsWith("GHOSTBUSTERS") Then
                    hwnd = pp.MainWindowHandle
                    Exit For
                End If
            Next

            If gbSupported AndAlso Windows.Gaming.UI.GameBar.IsInputRedirected Then
                hwnd = IntPtr.Zero
            End If

            If hwnd <> IntPtr.Zero Then

                Dim rcW As RECT
                GetWindowRect(hwnd, rcW)
                Dim ptC As New Point(0, 0)
                ClientToScreen(hwnd, ptC)
                If ptC.X <> rcW.left AndAlso ptC.Y <> rcW.top Then 'GB is windowed
                    If My.Settings.SaveLoc Then
                        If rcW.left = 0 AndAlso rcW.top = 0 Then   'GB is at top left
                            SetWindowPos(hwnd, SWP_HWND.TOP, My.Settings.Location.X, My.Settings.Location.Y,
                                    -1, -1, SetWindowPosFlags.IgnoreResize)
                        ElseIf Not MouseButtons.HasFlag(MouseButtons.Left) Then 'User is not dragging window
                            Dim newloc = New Point(rcW.left, rcW.top)
                            If newloc <> My.Settings.Location Then
                                My.Settings.Location = newloc
                                My.Settings.Save()
                            End If
                        End If
                    End If
                End If

                GetClientRect(hwnd, m_clsMouseHook.rcC)
            End If
            m_clsMouseHook.hwnd = hwnd
        Catch ex As Exception
            m_clsMouseHook.hwnd = IntPtr.Zero
        End Try
    End Sub
    Private Sub QuitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QuitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            'cp.Style = cp.Style
            cp.ExStyle = cp.ExStyle Or WindowStylesEx.WS_EX_TOOLWINDOW
            'cp.ClassStyle = cp.ClassStyle
            Return cp
        End Get
    End Property
    Protected Overloads Overrides ReadOnly Property ShowWithoutActivation() As Boolean
        Get
            Return True
        End Get
    End Property

    Private Sub SaveLocationToolStripMenuItem_CheckedChanged(sender As ToolStripMenuItem, e As EventArgs) Handles SaveLocationToolStripMenuItem.CheckedChanged
        My.Settings.SaveLoc = sender.Checked
        My.Settings.Save()
    End Sub
End Class

Module Native
#Region " SetWindowPos "
    Enum SWP_HWND As Integer
        ''' <summary>
        ''' 1 Places the window at the bottom Of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status And Is placed at the bottom Of all other windows.
        ''' </summary>
        BOTTOM = 1
        ''' <summary> 
        ''' -2 Places the window above all non-topmost windows (that Is, behind all topmost windows). This flag has no effect If the window Is already a non-topmost window.
        ''' </summary>
        NOTOPMOST = -2
        ''' <summary>
        ''' 0 Places the window at the top Of the Z order. 
        ''' </summary>
        TOP = 0
        ''' <summary>
        ''' -1 Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated. 
        ''' </summary>
        TOPMOST = -1
    End Enum

    <Flags>
    Public Enum SetWindowPosFlags As UInteger
        ''' <summary>If the calling thread and the thread that owns the window are attached to different input queues,
        ''' the system posts the request to the thread that owns the window. This prevents the calling thread from
        ''' blocking its execution while other threads process the request.</summary>
        ''' <remarks>SWP_ASYNCWINDOWPOS</remarks>
        ASyncWindowPosition = &H4000
        ''' <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
        ''' <remarks>SWP_DEFERERASE</remarks>
        DeferErase = &H2000
        ''' <summary>Draws a frame (defined in the window's class description) around the window.</summary>
        ''' <remarks>SWP_DRAWFRAME</remarks>
        DrawFrame = &H20
        ''' <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to
        ''' the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE
        ''' is sent only when the window's size is being changed.</summary>
        ''' <remarks>SWP_FRAMECHANGED</remarks>
        FrameChanged = &H20
        ''' <summary>Hides the window.</summary>
        ''' <remarks>SWP_HIDEWINDOW</remarks>
        HideWindow = &H80
        ''' <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the
        ''' top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter
        ''' parameter).</summary>
        ''' <remarks>SWP_NOACTIVATE</remarks>
        DoNotActivate = &H10
        ''' <summary>Discards the entire contents of the client area. If this flag is not specified, the valid
        ''' contents of the client area are saved and copied back into the client area after the window is sized or
        ''' repositioned.</summary>
        ''' <remarks>SWP_NOCOPYBITS</remarks>
        DoNotCopyBits = &H100
        ''' <summary>Retains the current position (ignores X and Y parameters).</summary>
        ''' <remarks>SWP_NOMOVE</remarks>
        IgnoreMove = &H2
        ''' <summary>Does not change the owner window's position in the Z order.</summary>
        ''' <remarks>SWP_NOOWNERZORDER</remarks>
        DoNotChangeOwnerZOrder = &H200
        ''' <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to
        ''' the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent
        ''' window uncovered as a result of the window being moved. When this flag is set, the application must
        ''' explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
        ''' <remarks>SWP_NOREDRAW</remarks>
        DoNotRedraw = &H8
        ''' <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
        ''' <remarks>SWP_NOREPOSITION</remarks>
        DoNotReposition = &H200
        ''' <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
        ''' <remarks>SWP_NOSENDCHANGING</remarks>
        DoNotSendChangingEvent = &H400
        ''' <summary>Retains the current size (ignores the cx and cy parameters).</summary>
        ''' <remarks>SWP_NOSIZE</remarks>
        IgnoreResize = &H1
        ''' <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
        ''' <remarks>SWP_NOZORDER</remarks>
        IgnoreZOrder = &H4
        ''' <summary>Displays the window.</summary>
        ''' <remarks>SWP_SHOWWINDOW</remarks>
        ShowWindow = &H40
        ''' <summary>Undocumented</summary>
        ''' <remarks>SWP_NOCLIENTSIZE</remarks>
        NoClientSize = &H800
        ''' <summary>Undocumented</summary>
        ''' <remarks>SWP_NOCLIENTMOVE</remarks>
        NoClientMove = &H1000
        ''' <summary>Undocumented</summary>
        ''' <remarks>SWP_STATECHANGED</remarks>
        StateChanged = &H8000
    End Enum

    <DllImport("user32.dll", SetLastError:=True)>
    Public Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr,
                                 ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer,
                                 ByVal uFlags As SetWindowPosFlags) As Boolean
    End Function
#End Region

    <DllImport("user32.dll")>
    Public Function GetForegroundWindow() As IntPtr : End Function
    <DllImport("user32.dll")>
    Public Function GetWindowRect(ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function GetClientRect(ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean : End Function
    <StructLayout(LayoutKind.Sequential)>
    Public Structure RECT
        Public left, top, right, bottom As Integer
        Public Sub New(left As Integer, top As Integer, right As Integer, bottom As Integer)
            Me.left = left
            Me.top = top
            Me.right = right
            Me.bottom = bottom
        End Sub
    End Structure
    <StructLayout(LayoutKind.Sequential)>
    Public Structure CURSORINFO
        Public cbSize As Int32
        Public flags As Int32
        Public hCursor As IntPtr
        Public ptScreenpos As Point
    End Structure
    <DllImport("user32.dll")>
    Public Function GetCursorInfo(ByRef pci As CURSORINFO) As Boolean : End Function
    <DllImport("user32.dll")>
    Public Function ClientToScreen(ByVal hWnd As IntPtr, ByRef lpPoint As Point) As Boolean : End Function
    '<Flags()>
    'Public Enum WindowStyles As Long
    '    WS_BORDER = &H800000
    '    WS_CAPTION = &HC00000
    '    WS_CHILD = &H40000000
    '    WS_CLIPCHILDREN = &H2000000
    '    WS_CLIPSIBLINGS = &H4000000
    '    WS_DISABLED = &H8000000
    '    WS_DLGFRAME = &H400000
    '    WS_GROUP = &H20000
    '    WS_HSCROLL = &H100000
    '    WS_MAXIMIZE = &H1000000
    '    WS_MAXIMIZEBOX = &H10000
    '    WS_MINIMIZE = &H20000000
    '    WS_MINIMIZEBOX = &H20000
    '    WS_OVERLAPPED = &H0
    '    WS_OVERLAPPEDWINDOW = WS_OVERLAPPED Or WS_CAPTION Or WS_SYSMENU Or WS_SIZEFRAME Or WS_MINIMIZEBOX Or WS_MAXIMIZEBOX
    '    WS_POPUP = &H80000000UI
    '    WS_POPUPWINDOW = WS_POPUP Or WS_BORDER Or WS_SYSMENU
    '    WS_SIZEFRAME = &H40000
    '    WS_SYSMENU = &H80000
    '    WS_TABSTOP = &H10000
    '    WS_VISIBLE = &H10000000
    '    WS_VSCROLL = &H200000
    'End Enum
    <Flags()>
    Public Enum WindowStylesEx As UInteger
        ''' <summary>Specifies a window that accepts drag-drop files.</summary>
        WS_EX_ACCEPTFILES = &H10

        ''' <summary>Forces a top-level window onto the taskbar when the window is visible.</summary>
        WS_EX_APPWINDOW = &H40000

        ''' <summary>Specifies a window that has a border with a sunken edge.</summary>
        WS_EX_CLIENTEDGE = &H200

        ''' <summary>
        ''' Specifies a window that paints all descendants in bottom-to-top painting order using double-buffering.
        ''' This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. This style is not supported in Windows 2000.
        ''' </summary>
        ''' <remarks>
        ''' With WS_EX_COMPOSITED set, all descendants of a window get bottom-to-top painting order using double-buffering.
        ''' Bottom-to-top painting order allows a descendent window to have translucency (alpha) and transparency (color-key) effects,
        ''' but only if the descendent window also has the WS_EX_TRANSPARENT bit set.
        ''' Double-buffering allows the window and its descendents to be painted without flicker.
        ''' </remarks>
        WS_EX_COMPOSITED = &H2000000

        ''' <summary>
        ''' Specifies a window that includes a question mark in the title bar. When the user clicks the question mark,
        ''' the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message.
        ''' The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command.
        ''' The Help application displays a pop-up window that typically contains help for the child window.
        ''' WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
        ''' </summary>
        WS_EX_CONTEXTHELP = &H400

        ''' <summary>
        ''' Specifies a window which contains child windows that should take part in dialog box navigation.
        ''' If this style is specified, the dialog manager recurses into children of this window when performing navigation operations
        ''' such as handling the TAB key, an arrow key, or a keyboard mnemonic.
        ''' </summary>
        WS_EX_CONTROLPARENT = &H10000

        ''' <summary>Specifies a window that has a double border.</summary>
        WS_EX_DLGMODALFRAME = &H1

        ''' <summary>
        ''' Specifies a window that is a layered window.
        ''' This cannot be used for child windows or if the window has a class style of either CS_OWNDC or CS_CLASSDC.
        ''' </summary>
        WS_EX_LAYERED = &H80000

        ''' <summary>
        ''' Specifies a window with the horizontal origin on the right edge. Increasing horizontal values advance to the left.
        ''' The shell language must support reading-order alignment for this to take effect.
        ''' </summary>
        WS_EX_LAYOUTRTL = &H400000

        ''' <summary>Specifies a window that has generic left-aligned properties. This is the default.</summary>
        WS_EX_LEFT = &H0

        ''' <summary>
        ''' Specifies a window with the vertical scroll bar (if present) to the left of the client area.
        ''' The shell language must support reading-order alignment for this to take effect.
        ''' </summary>
        WS_EX_LEFTSCROLLBAR = &H4000

        ''' <summary>
        ''' Specifies a window that displays text using left-to-right reading-order properties. This is the default.
        ''' </summary>
        WS_EX_LTRREADING = &H0

        ''' <summary>
        ''' Specifies a multiple-document interface (MDI) child window.
        ''' </summary>
        WS_EX_MDICHILD = &H40

        ''' <summary>
        ''' Specifies a top-level window created with this style does not become the foreground window when the user clicks it.
        ''' The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
        ''' The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the WS_EX_APPWINDOW style.
        ''' To activate the window, use the SetActiveWindow or SetForegroundWindow function.
        ''' </summary>
        WS_EX_NOACTIVATE = &H8000000

        ''' <summary>
        ''' Specifies a window which does not pass its window layout to its child windows.
        ''' </summary>
        WS_EX_NOINHERITLAYOUT = &H100000

        ''' <summary>
        ''' Specifies that a child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
        ''' </summary>
        WS_EX_NOPARENTNOTIFY = &H4

        ''' <summary>
        ''' The window does not render to a redirection surface.
        ''' This is for windows that do not have visible content or that use mechanisms other than surfaces to provide their visual.
        ''' </summary>
        WS_EX_NOREDIRECTIONBITMAP = &H200000

        ''' <summary>Specifies an overlapped window.</summary>
        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE Or WS_EX_CLIENTEDGE

        ''' <summary>Specifies a palette window, which is a modeless dialog box that presents an array of commands.</summary>
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE Or WS_EX_TOOLWINDOW Or WS_EX_TOPMOST

        ''' <summary>
        ''' Specifies a window that has generic "right-aligned" properties. This depends on the window class.
        ''' The shell language must support reading-order alignment for this to take effect.
        ''' Using the WS_EX_RIGHT style has the same effect as using the SS_RIGHT (static), ES_RIGHT (edit), and BS_RIGHT/BS_RIGHTBUTTON (button) control styles.
        ''' </summary>
        WS_EX_RIGHT = &H1000

        ''' <summary>Specifies a window with the vertical scroll bar (if present) to the right of the client area. This is the default.</summary>
        WS_EX_RIGHTSCROLLBAR = &H0

        ''' <summary>
        ''' Specifies a window that displays text using right-to-left reading-order properties.
        ''' The shell language must support reading-order alignment for this to take effect.
        ''' </summary>
        WS_EX_RTLREADING = &H2000

        ''' <summary>Specifies a window with a three-dimensional border style intended to be used for items that do not accept user input.</summary>
        WS_EX_STATICEDGE = &H20000

        ''' <summary>
        ''' Specifies a window that is intended to be used as a floating toolbar.
        ''' A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font.
        ''' A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB.
        ''' If a tool window has a system menu, its icon is not displayed on the title bar.
        ''' However, you can display the system menu by right-clicking or by typing ALT+SPACE.
        ''' </summary>
        WS_EX_TOOLWINDOW = &H80

        ''' <summary>
        ''' Specifies a window that should be placed above all non-topmost windows and should stay above them, even when the window is deactivated.
        ''' To add or remove this style, use the SetWindowPos function.
        ''' </summary>
        WS_EX_TOPMOST = &H8

        ''' <summary>
        ''' Specifies a window that should not be painted until siblings beneath the window (that were created by the same thread) have been painted.
        ''' The window appears transparent because the bits of underlying sibling windows have already been painted.
        ''' To achieve transparency without these restrictions, use the SetWindowRgn function.
        ''' </summary>
        WS_EX_TRANSPARENT = &H20

        ''' <summary>Specifies a window that has a border with a raised edge.</summary>
        WS_EX_WINDOWEDGE = &H100
    End Enum
End Module


Public Class MouseHook : Implements IDisposable

    Private Shared m_iMouseHandle As Integer = 0

    Private Const HC_ACTION As Integer = 0
    Private Const WH_MOUSE_LL As Integer = 14
    Private Const WM_MOUSEMOVE As Integer = &H200

    Public Delegate Function MouseHookCallBack(
        ByVal nCode As Integer,
        ByVal wParam As IntPtr,
        ByVal lParam As IntPtr) As Integer

    Public Declare Function GetModuleHandle Lib "kernel32.dll" _
    Alias "GetModuleHandleA" (
    ByVal ModuleName As String) As IntPtr

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function SetWindowsHookEx _
          (ByVal idHook As Integer, ByVal HookProc As MouseHookCallBack,
           ByVal hInstance As IntPtr, ByVal wParam As Integer) As Integer
    End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function CallNextHookEx _
          (ByVal idHook As Integer, ByVal nCode As Integer,
           ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    End Function

    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Overloads Shared Function UnhookWindowsHookEx _
              (ByVal idHook As Integer) As Boolean
    End Function

    'We only care about the first member so we marshal directly to a point
    '<StructLayout(LayoutKind.Sequential)>
    'Public Structure LLMouseHookStruct
    '    Public pt As Point
    '    Public mousedata As UInteger
    '    Public flags As UInteger
    '    Public time As UInteger
    '    Public dwExtraInfo As ULong
    'End Structure

    Public hwnd As IntPtr = IntPtr.Zero
    Friend rcC As RECT

    Private Function MouseProc(
        ByVal nCode As Integer,
        ByVal wParam As IntPtr,
        ByVal lParam As IntPtr) As Integer

        If (nCode = HC_ACTION) Then
            Select Case wParam.ToInt32()
                Case WM_MOUSEMOVE

                    If hwnd = IntPtr.Zero Then Exit Select
                    If hwnd <> GetForegroundWindow() Then Exit Select 'GetForegroundWindow() can be IntPtr.Zero when switching active app

                    'Dim uInfo As LLMouseHookStruct = Marshal.PtrToStructure(lParam, GetType(LLMouseHookStruct))
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
                    ElseIf cpos.Y < ptCTL.Y Then 'top border with exception to be able to drag window
                        Dim pci As New CURSORINFO With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(CURSORINFO))}
                        GetCursorInfo(pci)
                        If pci.flags = 0 Then 'Cursor is not visible
                            Cursor.Position = New Point(cpos.X, ptCTL.Y)
                            Return 1
                        End If
                    ElseIf cpos.Y > ptCBR.Y Then 'bottom border
                        Cursor.Position = New Point(cpos.X, ptCBR.Y)
                        Return 1
                    End If

            End Select
        End If

        Return CallNextHookEx(m_iMouseHandle, nCode, wParam, lParam)

    End Function
    Private m_clsMouseHookCallBack As MouseHookCallBack = New MouseHookCallBack(AddressOf MouseProc)
    Private disposedValue As Boolean

    Public Sub HookMouse()
        'm_clsMouseHookCallBack()
        m_iMouseHandle = SetWindowsHookEx(WH_MOUSE_LL,
            m_clsMouseHookCallBack,
            GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0)
        If m_iMouseHandle = 0 Then
            Throw New System.Exception("Mouse hook failed.")
            'Else
            '    GC.KeepAlive(m_clsMouseHookCallBack)
        End If
    End Sub

    Public Sub UnhookMouse()
        If (m_iMouseHandle <> 0) Then UnhookWindowsHookEx(m_iMouseHandle)
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
