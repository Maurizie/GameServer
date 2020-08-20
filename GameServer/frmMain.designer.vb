<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tsServerStatus = New System.Windows.Forms.StatusStrip()
        Me.lbStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.txtLogs = New System.Windows.Forms.TextBox()
        Me.txtCommand = New System.Windows.Forms.TextBox()
        Me.cmdEnter = New System.Windows.Forms.Button()
        Me.lbPlayers = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdStart = New System.Windows.Forms.Button()
        Me.cmdStop = New System.Windows.Forms.Button()
        Me.tsServerStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'tsServerStatus
        '
        Me.tsServerStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lbStatus})
        Me.tsServerStatus.Location = New System.Drawing.Point(0, 395)
        Me.tsServerStatus.Name = "tsServerStatus"
        Me.tsServerStatus.Padding = New System.Windows.Forms.Padding(1, 0, 10, 0)
        Me.tsServerStatus.Size = New System.Drawing.Size(834, 22)
        Me.tsServerStatus.SizingGrip = False
        Me.tsServerStatus.TabIndex = 1
        Me.tsServerStatus.Text = "ServerStatus"
        '
        'lbStatus
        '
        Me.lbStatus.Name = "lbStatus"
        Me.lbStatus.Size = New System.Drawing.Size(36, 17)
        Me.lbStatus.Text = "None"
        '
        'txtLogs
        '
        Me.txtLogs.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.txtLogs.Location = New System.Drawing.Point(9, 7)
        Me.txtLogs.Multiline = True
        Me.txtLogs.Name = "txtLogs"
        Me.txtLogs.ReadOnly = True
        Me.txtLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLogs.Size = New System.Drawing.Size(600, 351)
        Me.txtLogs.TabIndex = 2
        '
        'txtCommand
        '
        Me.txtCommand.Location = New System.Drawing.Point(10, 368)
        Me.txtCommand.Name = "txtCommand"
        Me.txtCommand.Size = New System.Drawing.Size(512, 20)
        Me.txtCommand.TabIndex = 3
        '
        'cmdEnter
        '
        Me.cmdEnter.Location = New System.Drawing.Point(528, 366)
        Me.cmdEnter.Name = "cmdEnter"
        Me.cmdEnter.Size = New System.Drawing.Size(81, 22)
        Me.cmdEnter.TabIndex = 4
        Me.cmdEnter.Text = "Enter"
        Me.cmdEnter.UseVisualStyleBackColor = True
        '
        'lbPlayers
        '
        Me.lbPlayers.FormattingEnabled = True
        Me.lbPlayers.HorizontalScrollbar = True
        Me.lbPlayers.Items.AddRange(New Object() {""})
        Me.lbPlayers.Location = New System.Drawing.Point(615, 33)
        Me.lbPlayers.Name = "lbPlayers"
        Me.lbPlayers.Size = New System.Drawing.Size(207, 329)
        Me.lbPlayers.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(613, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Players Online:"
        '
        'cmdStart
        '
        Me.cmdStart.Location = New System.Drawing.Point(618, 366)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.Size = New System.Drawing.Size(99, 21)
        Me.cmdStart.TabIndex = 7
        Me.cmdStart.Text = "Start"
        Me.cmdStart.UseVisualStyleBackColor = True
        '
        'cmdStop
        '
        Me.cmdStop.Enabled = False
        Me.cmdStop.Location = New System.Drawing.Point(725, 366)
        Me.cmdStop.Name = "cmdStop"
        Me.cmdStop.Size = New System.Drawing.Size(96, 21)
        Me.cmdStop.TabIndex = 8
        Me.cmdStop.Text = "Stop"
        Me.cmdStop.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(834, 417)
        Me.Controls.Add(Me.cmdStop)
        Me.Controls.Add(Me.cmdStart)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lbPlayers)
        Me.Controls.Add(Me.cmdEnter)
        Me.Controls.Add(Me.txtCommand)
        Me.Controls.Add(Me.txtLogs)
        Me.Controls.Add(Me.tsServerStatus)
        Me.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.Text = "Spasm Server"
        Me.tsServerStatus.ResumeLayout(False)
        Me.tsServerStatus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tsServerStatus As System.Windows.Forms.StatusStrip
    Friend WithEvents lbStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents txtLogs As System.Windows.Forms.TextBox
    Friend WithEvents txtCommand As System.Windows.Forms.TextBox
    Friend WithEvents cmdEnter As System.Windows.Forms.Button
    Friend WithEvents lbPlayers As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdStart As System.Windows.Forms.Button
    Friend WithEvents cmdStop As System.Windows.Forms.Button

End Class
