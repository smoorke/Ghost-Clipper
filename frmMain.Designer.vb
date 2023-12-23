<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.tmrTick = New System.Windows.Forms.Timer(Me.components)
        Me.trayIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.cmsTray = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveLocationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.QuitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmsTray.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripSeparator1
        '
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New System.Drawing.Size(144, 6)
        '
        'tmrTick
        '
        Me.tmrTick.Enabled = True
        Me.tmrTick.Interval = 666
        '
        'trayIcon
        '
        Me.trayIcon.ContextMenuStrip = Me.cmsTray
        Me.trayIcon.Icon = CType(resources.GetObject("trayIcon.Icon"), System.Drawing.Icon)
        Me.trayIcon.Text = "Ghost Clipper"
        Me.trayIcon.Visible = True
        '
        'cmsTray
        '
        Me.cmsTray.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveLocationToolStripMenuItem, ToolStripSeparator1, Me.QuitToolStripMenuItem})
        Me.cmsTray.Name = "cmsTray"
        Me.cmsTray.Size = New System.Drawing.Size(148, 54)
        '
        'SaveLocationToolStripMenuItem
        '
        Me.SaveLocationToolStripMenuItem.Checked = Global.GhostClipper.My.MySettings.Default.SaveLoc
        Me.SaveLocationToolStripMenuItem.CheckOnClick = True
        Me.SaveLocationToolStripMenuItem.Name = "SaveLocationToolStripMenuItem"
        Me.SaveLocationToolStripMenuItem.Size = New System.Drawing.Size(147, 22)
        Me.SaveLocationToolStripMenuItem.Text = "Save Location"
        Me.SaveLocationToolStripMenuItem.ToolTipText = "Save the startup Location of GHOSTBUSTERS" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'QuitToolStripMenuItem
        '
        Me.QuitToolStripMenuItem.Image = Global.GhostClipper.My.Resources.Resources.ghostbig_1_png
        Me.QuitToolStripMenuItem.Name = "QuitToolStripMenuItem"
        Me.QuitToolStripMenuItem.Size = New System.Drawing.Size(147, 22)
        Me.QuitToolStripMenuItem.Text = "Quit"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(281, 142)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.Opacity = 0R
        Me.ShowInTaskbar = False
        Me.Text = "Ghost Clipper"
        Me.cmsTray.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tmrTick As Timer
    Friend WithEvents trayIcon As NotifyIcon
    Friend WithEvents cmsTray As ContextMenuStrip
    Friend WithEvents QuitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveLocationToolStripMenuItem As ToolStripMenuItem
End Class
