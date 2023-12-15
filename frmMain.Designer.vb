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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.tmrTick = New System.Windows.Forms.Timer(Me.components)
        Me.trayIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.cmsTray = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.QuitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsTray.SuspendLayout()
        Me.SuspendLayout()
        '
        'tmrTick
        '
        Me.tmrTick.Enabled = True
        Me.tmrTick.Interval = 750
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
        Me.cmsTray.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.QuitToolStripMenuItem})
        Me.cmsTray.Name = "cmsTray"
        Me.cmsTray.Size = New System.Drawing.Size(181, 48)
        '
        'QuitToolStripMenuItem
        '
        Me.QuitToolStripMenuItem.Image = Global.Ghost_Clipper.My.Resources.Resources.ghostbig_1_png
        Me.QuitToolStripMenuItem.Name = "QuitToolStripMenuItem"
        Me.QuitToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.QuitToolStripMenuItem.Text = "Quit"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(264, 64)
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
End Class
