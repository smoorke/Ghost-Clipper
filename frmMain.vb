Public Class frmMain

    Private gbSupported As Boolean = False
    Private m_clsMouseHook As MouseHook = New MouseHook

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Environment.OSVersion.Version >= New Version("10.0.10586") Then gbSupported = True

        m_clsMouseHook.HookMouse()

        If New Version(My.Settings.Version) < My.Application.Info.Version Then
            My.Settings.Upgrade()
            My.Settings.Version = My.Application.Info.Version.ToString
            My.Settings.Save()
            SaveLocationToolStripMenuItem.Checked = My.Settings.SaveLoc
        End If

        cmsTray.Renderer = New ToolStripProfessionalRenderer(New ThemedColorTable)
        If WinUsingDarkTheme() Then ThemedColorTable.darkMode = True
        cmsTray.ForeColor = If(ThemedColorTable.darkMode, Color.White, Color.Black)

    End Sub
    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        m_clsMouseHook.UnhookMouse()
    End Sub

    Private gbProc As Process
    Private newLatch = False

    Private Sub tmrTick_Tick(sender As Object, e As EventArgs) Handles tmrTick.Tick
        Try 'We try here since Process.HasExited() will except when Process is elevated
            Dim hwnd = IntPtr.Zero
            If gbProc Is Nothing OrElse gbProc.HasExited() Then
                For Each pp As Process In Process.GetProcessesByName("ghost")
                    If pp.MainWindowTitle.StartsWith("GHOSTBUSTERS") Then
                        hwnd = pp.MainWindowHandle
                        gbProc = pp
                        newLatch = True
                        SetWindowLong(Me.Handle, GWL_HWNDPARENT, hwnd) 'this allows showcursor to work
                        Exit For
                    End If
                Next
            Else
                hwnd = gbProc.MainWindowHandle
                newLatch = False
            End If
            If hwnd <> IntPtr.Zero AndAlso (IsIconic(hwnd) OrElse
                (gbSupported AndAlso Windows.Gaming.UI.GameBar.IsInputRedirected)) Then hwnd = IntPtr.Zero
            If hwnd <> IntPtr.Zero Then
                gbProc.Refresh()
                If Not gbProc.MainWindowTitle.StartsWith("GHOSTBUSTERS") Then
                    If Not cursorShow Then pointer(True)
                    hwnd = IntPtr.Zero 'sysmenu is open
                End If
            End If

            If hwnd <> IntPtr.Zero Then

                Dim rcW As RECT
                GetWindowRect(hwnd, rcW)
                Dim ptC As New Point(0, 0)
                ClientToScreen(hwnd, ptC)

                Dim scr = Screen.FromPoint(ptC)
                Dim sb = scr.Bounds

                If ptC.X <> rcW.left AndAlso ptC.Y <> rcW.top Then 'GB is windowed
                    If My.Settings.SaveLoc Then
                        If rcW.left = sb.X AndAlso rcW.top = sb.Y Then 'GB is at top left of screen
                            If sb.Location <> My.Settings.Location Then 'Don't restore when saved location is top left
                                SetWindowPos(hwnd, SWP_HWND.TOP, My.Settings.Location.X, My.Settings.Location.Y,
                                    -1, -1, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate)
                            End If
                        ElseIf Not newLatch Then 'Don't save Location on startup, it can be wrong when GB is on secondary monitors
                            Dim newloc = New Point(rcW.left, rcW.top)
                            If newloc <> My.Settings.Location Then
                                My.Settings.Location = newloc
                                If Not MouseButtons.HasFlag(MouseButtons.Left) Then My.Settings.Save() 'User is not dragging window
                            End If
                        End If
                    Else 'Not My.Settings.SaveLoc
                        If rcW.left = sb.X AndAlso rcW.top = sb.Y Then 'GB is at top left of screen
                            Dim wa = scr.WorkingArea
                            Dim newloc = New Point(Math.Max(sb.X, wa.X), Math.Max(sb.Y, wa.Y)) 'fix taskbar overlap if it is at top or left
                            If newloc <> New Point(rcW.left, rcW.top) Then
                                SetWindowPos(hwnd, SWP_HWND.TOP, newloc.X, newloc.Y,
                                    -1, -1, SetWindowPosFlags.IgnoreResize Or SetWindowPosFlags.DoNotActivate)
                            End If
                        End If
                    End If
                End If

                GetClientRect(hwnd, m_clsMouseHook.rcC)
            End If
            m_clsMouseHook.hwnd = hwnd
        Catch ex As Exception
            gbProc = Nothing
            m_clsMouseHook.hwnd = IntPtr.Zero
        End Try
    End Sub
    Private Sub QuitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QuitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private themechanging As Boolean = False
    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_DWMCOLORIZATIONCOLORCHANGED
                If Not themechanging Then
                    themechanging = True

                    ThemedColorTable.darkMode = WinUsingDarkTheme()
                    cmsTray.ForeColor = If(ThemedColorTable.darkMode, Color.White, Color.Black)

                    Task.Run(Sub()
                                 Threading.Thread.Sleep(7500)
                                 themechanging = False
                             End Sub)
                End If
        End Select
        MyBase.WndProc(m)
    End Sub

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.Style = cp.Style Or WindowStyles.WS_MINIMIZE And Not WindowStyles.WS_VISIBLE
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
