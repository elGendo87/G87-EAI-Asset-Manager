<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_ChangeCat
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
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_ChangeCat))
        Label1 = New Label()
        Label2 = New Label()
        Txt_CurrentAssetCat = New TextBox()
        Cmb_NewAssetCat = New ComboBox()
        Btn_Change = New Button()
        Btn_Cancel = New Button()
        Tooltip = New ToolTip(components)
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 46)
        Label1.Name = "Label1"
        Label1.Size = New Size(164, 15)
        Label1.TabIndex = 0
        Label1.Text = "Select the new asset category:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(12, 9)
        Label2.Name = "Label2"
        Label2.Size = New Size(128, 15)
        Label2.TabIndex = 1
        Label2.Text = "Current asset category:"
        ' 
        ' Txt_CurrentAssetCat
        ' 
        Txt_CurrentAssetCat.Location = New Point(146, 6)
        Txt_CurrentAssetCat.Name = "Txt_CurrentAssetCat"
        Txt_CurrentAssetCat.ReadOnly = True
        Txt_CurrentAssetCat.Size = New Size(276, 23)
        Txt_CurrentAssetCat.TabIndex = 2
        Txt_CurrentAssetCat.TabStop = False
        Tooltip.SetToolTip(Txt_CurrentAssetCat, "Current asset category.")
        ' 
        ' Cmb_NewAssetCat
        ' 
        Cmb_NewAssetCat.Enabled = False
        Cmb_NewAssetCat.FormattingEnabled = True
        Cmb_NewAssetCat.Location = New Point(182, 43)
        Cmb_NewAssetCat.Name = "Cmb_NewAssetCat"
        Cmb_NewAssetCat.Size = New Size(240, 23)
        Cmb_NewAssetCat.TabIndex = 3
        Tooltip.SetToolTip(Cmb_NewAssetCat, "List of available categories for the asset." & vbCrLf & vbCrLf & "This list is dynamic for each type of asset.")
        ' 
        ' Btn_Change
        ' 
        Btn_Change.ImageAlign = ContentAlignment.MiddleLeft
        Btn_Change.Location = New Point(266, 76)
        Btn_Change.Name = "Btn_Change"
        Btn_Change.Size = New Size(75, 25)
        Btn_Change.TabIndex = 4
        Btn_Change.Text = "Change"
        Btn_Change.TextAlign = ContentAlignment.MiddleRight
        Tooltip.SetToolTip(Btn_Change, resources.GetString("Btn_Change.ToolTip"))
        Btn_Change.UseVisualStyleBackColor = True
        ' 
        ' Btn_Cancel
        ' 
        Btn_Cancel.ImageAlign = ContentAlignment.MiddleLeft
        Btn_Cancel.Location = New Point(347, 76)
        Btn_Cancel.Name = "Btn_Cancel"
        Btn_Cancel.Size = New Size(75, 25)
        Btn_Cancel.TabIndex = 5
        Btn_Cancel.Text = "Cancel"
        Btn_Cancel.TextAlign = ContentAlignment.MiddleRight
        Btn_Cancel.UseVisualStyleBackColor = True
        ' 
        ' Tooltip
        ' 
        Tooltip.AutoPopDelay = 5000
        Tooltip.InitialDelay = 100
        Tooltip.ReshowDelay = 100
        ' 
        ' Frm_ChangeCat
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(434, 111)
        Controls.Add(Btn_Cancel)
        Controls.Add(Btn_Change)
        Controls.Add(Cmb_NewAssetCat)
        Controls.Add(Txt_CurrentAssetCat)
        Controls.Add(Label2)
        Controls.Add(Label1)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        MaximumSize = New Size(450, 150)
        MinimizeBox = False
        MinimumSize = New Size(450, 150)
        Name = "Frm_ChangeCat"
        StartPosition = FormStartPosition.CenterParent
        Text = "Frm_ChangeCat"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Txt_CurrentAssetCat As TextBox
    Friend WithEvents Cmb_NewAssetCat As ComboBox
    Friend WithEvents Btn_Change As Button
    Friend WithEvents Btn_Cancel As Button
    Friend WithEvents Tooltip As ToolTip
End Class
