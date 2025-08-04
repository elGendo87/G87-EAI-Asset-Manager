<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_InstCustomAssets
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
        Btn_FindFiles = New Button()
        Cmb_InstAssetType = New ComboBox()
        Cmb_InstAssetCat = New ComboBox()
        Btn_Install = New Button()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Txt_FolderPath = New TextBox()
        Lst_Assets = New ListBox()
        FBD_Assets = New FolderBrowserDialog()
        Btn_Cancel = New Button()
        SuspendLayout()
        ' 
        ' Btn_FindFiles
        ' 
        Btn_FindFiles.Location = New Point(544, 88)
        Btn_FindFiles.Name = "Btn_FindFiles"
        Btn_FindFiles.Size = New Size(75, 23)
        Btn_FindFiles.TabIndex = 0
        Btn_FindFiles.Text = "Search..."
        Btn_FindFiles.UseVisualStyleBackColor = True
        ' 
        ' Cmb_InstAssetType
        ' 
        Cmb_InstAssetType.FormattingEnabled = True
        Cmb_InstAssetType.Location = New Point(147, 26)
        Cmb_InstAssetType.Name = "Cmb_InstAssetType"
        Cmb_InstAssetType.Size = New Size(195, 23)
        Cmb_InstAssetType.TabIndex = 1
        ' 
        ' Cmb_InstAssetCat
        ' 
        Cmb_InstAssetCat.FormattingEnabled = True
        Cmb_InstAssetCat.Location = New Point(171, 55)
        Cmb_InstAssetCat.Name = "Cmb_InstAssetCat"
        Cmb_InstAssetCat.Size = New Size(171, 23)
        Cmb_InstAssetCat.TabIndex = 2
        ' 
        ' Btn_Install
        ' 
        Btn_Install.Location = New Point(463, 406)
        Btn_Install.Name = "Btn_Install"
        Btn_Install.Size = New Size(75, 23)
        Btn_Install.TabIndex = 3
        Btn_Install.Text = "Install"
        Btn_Install.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(9, 30)
        Label1.Name = "Label1"
        Label1.Size = New Size(132, 15)
        Label1.TabIndex = 4
        Label1.Text = "1.- Select an Asset Type:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(9, 60)
        Label2.Name = "Label2"
        Label2.Size = New Size(156, 15)
        Label2.TabIndex = 5
        Label2.Text = "2.- Select an Asset Category:"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(9, 91)
        Label3.Name = "Label3"
        Label3.Size = New Size(125, 15)
        Label3.TabIndex = 6
        Label3.Text = "3.- Select Path to Files:"
        ' 
        ' Txt_FolderPath
        ' 
        Txt_FolderPath.Location = New Point(133, 88)
        Txt_FolderPath.Name = "Txt_FolderPath"
        Txt_FolderPath.ReadOnly = True
        Txt_FolderPath.Size = New Size(405, 23)
        Txt_FolderPath.TabIndex = 7
        ' 
        ' Lst_Assets
        ' 
        Lst_Assets.FormattingEnabled = True
        Lst_Assets.ItemHeight = 15
        Lst_Assets.Location = New Point(9, 117)
        Lst_Assets.Name = "Lst_Assets"
        Lst_Assets.Size = New Size(610, 274)
        Lst_Assets.TabIndex = 8
        ' 
        ' Btn_Cancel
        ' 
        Btn_Cancel.Location = New Point(544, 406)
        Btn_Cancel.Name = "Btn_Cancel"
        Btn_Cancel.Size = New Size(75, 23)
        Btn_Cancel.TabIndex = 9
        Btn_Cancel.Text = "Cancel"
        Btn_Cancel.UseVisualStyleBackColor = True
        ' 
        ' Frm_InstCustomAssets
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = Btn_Cancel
        ClientSize = New Size(624, 441)
        Controls.Add(Btn_Cancel)
        Controls.Add(Lst_Assets)
        Controls.Add(Txt_FolderPath)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Btn_Install)
        Controls.Add(Cmb_InstAssetCat)
        Controls.Add(Cmb_InstAssetType)
        Controls.Add(Btn_FindFiles)
        KeyPreview = True
        MaximizeBox = False
        MaximumSize = New Size(640, 480)
        MinimizeBox = False
        MinimumSize = New Size(640, 480)
        Name = "Frm_InstCustomAssets"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterScreen
        Text = "Custom Assets Installer"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Btn_FindFiles As Button
    Friend WithEvents Cmb_InstAssetType As ComboBox
    Friend WithEvents Cmb_InstAssetCat As ComboBox
    Friend WithEvents Btn_Install As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Txt_FolderPath As TextBox
    Friend WithEvents Lst_Assets As ListBox
    Friend WithEvents FBD_Assets As FolderBrowserDialog
    Friend WithEvents Btn_Cancel As Button
End Class
