<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_About
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_About))
        Label1 = New Label()
        Lbl_DiscordLink = New LinkLabel()
        Btn_Close = New Button()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(12, 9)
        Label1.Name = "Label1"
        Label1.Size = New Size(582, 231)
        Label1.TabIndex = 0
        Label1.Text = resources.GetString("Label1.Text")
        ' 
        ' Lbl_DiscordLink
        ' 
        Lbl_DiscordLink.AutoSize = True
        Lbl_DiscordLink.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Lbl_DiscordLink.Location = New Point(94, 72)
        Lbl_DiscordLink.Name = "Lbl_DiscordLink"
        Lbl_DiscordLink.Size = New Size(221, 21)
        Lbl_DiscordLink.TabIndex = 1
        Lbl_DiscordLink.TabStop = True
        Lbl_DiscordLink.Text = "https://discord.gg/fr7EKdY8V6"
        ' 
        ' Btn_Close
        ' 
        Btn_Close.ImageAlign = ContentAlignment.MiddleLeft
        Btn_Close.Location = New Point(537, 404)
        Btn_Close.Name = "Btn_Close"
        Btn_Close.Size = New Size(75, 25)
        Btn_Close.TabIndex = 2
        Btn_Close.Text = "Close"
        Btn_Close.TextAlign = ContentAlignment.MiddleRight
        Btn_Close.UseVisualStyleBackColor = True
        ' 
        ' Frm_About
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(624, 441)
        Controls.Add(Btn_Close)
        Controls.Add(Lbl_DiscordLink)
        Controls.Add(Label1)
        MaximizeBox = False
        MaximumSize = New Size(640, 480)
        MinimizeBox = False
        MinimumSize = New Size(640, 480)
        Name = "Frm_About"
        StartPosition = FormStartPosition.CenterScreen
        Text = "About"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Lbl_DiscordLink As LinkLabel
    Friend WithEvents Btn_Close As Button
End Class
