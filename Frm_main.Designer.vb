<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frm_Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_Main))
        Lst_Img = New ListView()
        Ctx_Asset = New ContextMenuStrip(components)
        MenuItem_DisableAsset = New ToolStripMenuItem()
        MenuItem_EnableAsset = New ToolStripMenuItem()
        ToolStripSeparator3 = New ToolStripSeparator()
        MenuItem_OpenLocation = New ToolStripMenuItem()
        ToolStripSeparator2 = New ToolStripSeparator()
        MenuItem_CreateLocalCopy = New ToolStripMenuItem()
        MenuItem_DeleteLocalAsset = New ToolStripMenuItem()
        MenuItem_RenameLocalAsset = New ToolStripMenuItem()
        MenuItem_EditLocalAsset = New ToolStripMenuItem()
        Cmb_Mods = New ComboBox()
        Cmb_AssetType = New ComboBox()
        Cmb_Cat = New ComboBox()
        Btn_DisableSelectedItems = New Button()
        Btn_EnableSelectedItems = New Button()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        ToolTips = New ToolTip(components)
        Mst_Main = New MenuStrip()
        Msm_Main = New ToolStripMenuItem()
        Msm_InstCustomAssets = New ToolStripMenuItem()
        ToolStripSeparator1 = New ToolStripSeparator()
        Msm_Close = New ToolStripMenuItem()
        Msm_Filter = New ToolStripMenuItem()
        Msm_FiltersDisabledOnly = New ToolStripMenuItem()
        Msm_About = New ToolStripMenuItem()
        Txt_InfoBar = New TextBox()
        Lbl_Loading = New Label()
        ProcLoading = New Process()
        Chk_ShowEAI = New CheckBox()
        Ctx_Asset.SuspendLayout()
        Mst_Main.SuspendLayout()
        SuspendLayout()
        ' 
        ' Lst_Img
        ' 
        Lst_Img.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Lst_Img.BackColor = Color.LightGray
        Lst_Img.ContextMenuStrip = Ctx_Asset
        Lst_Img.Location = New Point(12, 62)
        Lst_Img.Name = "Lst_Img"
        Lst_Img.ShowGroups = False
        Lst_Img.Size = New Size(1240, 583)
        Lst_Img.TabIndex = 0
        Lst_Img.TabStop = False
        Lst_Img.UseCompatibleStateImageBehavior = False
        ' 
        ' Ctx_Asset
        ' 
        Ctx_Asset.Items.AddRange(New ToolStripItem() {MenuItem_DisableAsset, MenuItem_EnableAsset, ToolStripSeparator3, MenuItem_OpenLocation, ToolStripSeparator2, MenuItem_CreateLocalCopy, MenuItem_DeleteLocalAsset, MenuItem_RenameLocalAsset, MenuItem_EditLocalAsset})
        Ctx_Asset.Name = "Ctx_Asset"
        Ctx_Asset.Size = New Size(184, 170)
        ' 
        ' MenuItem_DisableAsset
        ' 
        MenuItem_DisableAsset.Name = "MenuItem_DisableAsset"
        MenuItem_DisableAsset.Size = New Size(183, 22)
        MenuItem_DisableAsset.Text = "Disable Asset"
        ' 
        ' MenuItem_EnableAsset
        ' 
        MenuItem_EnableAsset.Name = "MenuItem_EnableAsset"
        MenuItem_EnableAsset.Size = New Size(183, 22)
        MenuItem_EnableAsset.Text = "Enable Asset"
        ' 
        ' ToolStripSeparator3
        ' 
        ToolStripSeparator3.Name = "ToolStripSeparator3"
        ToolStripSeparator3.Size = New Size(180, 6)
        ' 
        ' MenuItem_OpenLocation
        ' 
        MenuItem_OpenLocation.Name = "MenuItem_OpenLocation"
        MenuItem_OpenLocation.Size = New Size(183, 22)
        MenuItem_OpenLocation.Text = "Open in File Explorer"
        ' 
        ' ToolStripSeparator2
        ' 
        ToolStripSeparator2.Name = "ToolStripSeparator2"
        ToolStripSeparator2.Size = New Size(180, 6)
        ' 
        ' MenuItem_CreateLocalCopy
        ' 
        MenuItem_CreateLocalCopy.Name = "MenuItem_CreateLocalCopy"
        MenuItem_CreateLocalCopy.Size = New Size(183, 22)
        MenuItem_CreateLocalCopy.Text = "Create Local Copy"
        ' 
        ' MenuItem_DeleteLocalAsset
        ' 
        MenuItem_DeleteLocalAsset.Name = "MenuItem_DeleteLocalAsset"
        MenuItem_DeleteLocalAsset.Size = New Size(183, 22)
        MenuItem_DeleteLocalAsset.Text = "Delete Local Asset"
        ' 
        ' MenuItem_RenameLocalAsset
        ' 
        MenuItem_RenameLocalAsset.Name = "MenuItem_RenameLocalAsset"
        MenuItem_RenameLocalAsset.Size = New Size(183, 22)
        MenuItem_RenameLocalAsset.Text = "Rename Local Asset"
        ' 
        ' MenuItem_EditLocalAsset
        ' 
        MenuItem_EditLocalAsset.Name = "MenuItem_EditLocalAsset"
        MenuItem_EditLocalAsset.Size = New Size(183, 22)
        MenuItem_EditLocalAsset.Text = "Edit Asset Properties"
        ' 
        ' Cmb_Mods
        ' 
        Cmb_Mods.FormattingEnabled = True
        Cmb_Mods.Location = New Point(221, 32)
        Cmb_Mods.Name = "Cmb_Mods"
        Cmb_Mods.Size = New Size(300, 23)
        Cmb_Mods.TabIndex = 1
        ToolTips.SetToolTip(Cmb_Mods, "List of subscribed mods that contain decals, netlanes or surfaces to be managed.")
        ' 
        ' Cmb_AssetType
        ' 
        Cmb_AssetType.FormattingEnabled = True
        Cmb_AssetType.Location = New Point(618, 32)
        Cmb_AssetType.Name = "Cmb_AssetType"
        Cmb_AssetType.Size = New Size(171, 23)
        Cmb_AssetType.TabIndex = 3
        ToolTips.SetToolTip(Cmb_AssetType, "Type of assets to manage:" & vbCrLf & "- Decals" & vbCrLf & "- Netlanes" & vbCrLf & "- Surfaces" & vbCrLf & vbCrLf & "Only existing type will be shown in the list.")
        ' 
        ' Cmb_Cat
        ' 
        Cmb_Cat.FormattingEnabled = True
        Cmb_Cat.Location = New Point(909, 32)
        Cmb_Cat.Name = "Cmb_Cat"
        Cmb_Cat.Size = New Size(206, 23)
        Cmb_Cat.TabIndex = 4
        ToolTips.SetToolTip(Cmb_Cat, "List of assets categories." & vbCrLf & vbCrLf & "Only available categories will be shown in the list.")
        ' 
        ' Btn_DisableSelectedItems
        ' 
        Btn_DisableSelectedItems.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Btn_DisableSelectedItems.ImageAlign = ContentAlignment.MiddleLeft
        Btn_DisableSelectedItems.Location = New Point(938, 652)
        Btn_DisableSelectedItems.Name = "Btn_DisableSelectedItems"
        Btn_DisableSelectedItems.Size = New Size(154, 25)
        Btn_DisableSelectedItems.TabIndex = 5
        Btn_DisableSelectedItems.Text = "Disable Selected Assets"
        Btn_DisableSelectedItems.TextAlign = ContentAlignment.MiddleRight
        ToolTips.SetToolTip(Btn_DisableSelectedItems, "Disable all selected assets to skip loading in game.")
        Btn_DisableSelectedItems.UseVisualStyleBackColor = True
        ' 
        ' Btn_EnableSelectedItems
        ' 
        Btn_EnableSelectedItems.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Btn_EnableSelectedItems.ImageAlign = ContentAlignment.MiddleLeft
        Btn_EnableSelectedItems.Location = New Point(1098, 652)
        Btn_EnableSelectedItems.Name = "Btn_EnableSelectedItems"
        Btn_EnableSelectedItems.Size = New Size(154, 25)
        Btn_EnableSelectedItems.TabIndex = 6
        Btn_EnableSelectedItems.Text = "Enable Selected Assets"
        Btn_EnableSelectedItems.TextAlign = ContentAlignment.MiddleRight
        ToolTips.SetToolTip(Btn_EnableSelectedItems, "Enable all selected assets to show again in game.")
        Btn_EnableSelectedItems.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(114, 37)
        Label1.Name = "Label1"
        Label1.Size = New Size(101, 15)
        Label1.TabIndex = 7
        Label1.Text = "Subscribed Mods:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(547, 37)
        Label2.Name = "Label2"
        Label2.Size = New Size(65, 15)
        Label2.TabIndex = 8
        Label2.Text = "Asset Type:"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(814, 37)
        Label3.Name = "Label3"
        Label3.Size = New Size(89, 15)
        Label3.TabIndex = 9
        Label3.Text = "Asset Category:"
        ' 
        ' Mst_Main
        ' 
        Mst_Main.Items.AddRange(New ToolStripItem() {Msm_Main, Msm_Filter, Msm_About})
        Mst_Main.Location = New Point(0, 0)
        Mst_Main.Name = "Mst_Main"
        Mst_Main.RenderMode = ToolStripRenderMode.System
        Mst_Main.Size = New Size(1264, 24)
        Mst_Main.TabIndex = 10
        ' 
        ' Msm_Main
        ' 
        Msm_Main.DropDownItems.AddRange(New ToolStripItem() {Msm_InstCustomAssets, ToolStripSeparator1, Msm_Close})
        Msm_Main.Name = "Msm_Main"
        Msm_Main.Size = New Size(46, 20)
        Msm_Main.Text = "Main"
        ' 
        ' Msm_InstCustomAssets
        ' 
        Msm_InstCustomAssets.Name = "Msm_InstCustomAssets"
        Msm_InstCustomAssets.Size = New Size(186, 22)
        Msm_InstCustomAssets.Text = "Install Custom Assets"
        ' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New Size(183, 6)
        ' 
        ' Msm_Close
        ' 
        Msm_Close.Name = "Msm_Close"
        Msm_Close.Size = New Size(186, 22)
        Msm_Close.Text = "Exit"
        ' 
        ' Msm_Filter
        ' 
        Msm_Filter.DropDownItems.AddRange(New ToolStripItem() {Msm_FiltersDisabledOnly})
        Msm_Filter.Name = "Msm_Filter"
        Msm_Filter.Size = New Size(45, 20)
        Msm_Filter.Text = "Filter"
        ' 
        ' Msm_FiltersDisabledOnly
        ' 
        Msm_FiltersDisabledOnly.CheckOnClick = True
        Msm_FiltersDisabledOnly.Name = "Msm_FiltersDisabledOnly"
        Msm_FiltersDisabledOnly.Size = New Size(208, 22)
        Msm_FiltersDisabledOnly.Text = "Show only disabled items"
        ' 
        ' Msm_About
        ' 
        Msm_About.Name = "Msm_About"
        Msm_About.Size = New Size(52, 20)
        Msm_About.Text = "About"
        ' 
        ' Txt_InfoBar
        ' 
        Txt_InfoBar.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Txt_InfoBar.Location = New Point(12, 653)
        Txt_InfoBar.Name = "Txt_InfoBar"
        Txt_InfoBar.ReadOnly = True
        Txt_InfoBar.Size = New Size(920, 23)
        Txt_InfoBar.TabIndex = 11
        ' 
        ' Lbl_Loading
        ' 
        Lbl_Loading.BorderStyle = BorderStyle.FixedSingle
        Lbl_Loading.FlatStyle = FlatStyle.Flat
        Lbl_Loading.Font = New Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Lbl_Loading.Location = New Point(506, 343)
        Lbl_Loading.Name = "Lbl_Loading"
        Lbl_Loading.Size = New Size(200, 44)
        Lbl_Loading.TabIndex = 13
        Lbl_Loading.Text = "LOADING..."
        Lbl_Loading.TextAlign = ContentAlignment.MiddleCenter
        Lbl_Loading.Visible = False
        ' 
        ' ProcLoading
        ' 
        ProcLoading.StartInfo.Domain = ""
        ProcLoading.StartInfo.LoadUserProfile = False
        ProcLoading.StartInfo.Password = Nothing
        ProcLoading.StartInfo.StandardErrorEncoding = Nothing
        ProcLoading.StartInfo.StandardInputEncoding = Nothing
        ProcLoading.StartInfo.StandardOutputEncoding = Nothing
        ProcLoading.StartInfo.UseCredentialsForNetworkingOnly = False
        ProcLoading.StartInfo.UserName = ""
        ProcLoading.SynchronizingObject = Me
        ' 
        ' Chk_ShowEAI
        ' 
        Chk_ShowEAI.AutoSize = True
        Chk_ShowEAI.Location = New Point(15, 36)
        Chk_ShowEAI.Name = "Chk_ShowEAI"
        Chk_ShowEAI.Size = New Size(84, 19)
        Chk_ShowEAI.TabIndex = 14
        Chk_ShowEAI.Text = "EAI Sorting"
        Chk_ShowEAI.UseVisualStyleBackColor = True
        ' 
        ' Frm_Main
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1264, 681)
        Controls.Add(Chk_ShowEAI)
        Controls.Add(Lbl_Loading)
        Controls.Add(Txt_InfoBar)
        Controls.Add(Mst_Main)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Btn_EnableSelectedItems)
        Controls.Add(Btn_DisableSelectedItems)
        Controls.Add(Cmb_Cat)
        Controls.Add(Cmb_AssetType)
        Controls.Add(Cmb_Mods)
        Controls.Add(Lst_Img)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "Frm_Main"
        Text = "[G87] EAI Asset Manager - v1.4.1 r3 Beta"
        Ctx_Asset.ResumeLayout(False)
        Mst_Main.ResumeLayout(False)
        Mst_Main.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Lst_Img As ListView
    Friend WithEvents Cmb_Mods As ComboBox
    Friend WithEvents Cmb_AssetType As ComboBox
    Friend WithEvents Cmb_Cat As ComboBox
    Friend WithEvents Ctx_Asset As ContextMenuStrip
    Friend WithEvents MenuItem_DisableAsset As ToolStripMenuItem
    Friend WithEvents MenuItem_EnableAsset As ToolStripMenuItem
    Friend WithEvents MenuItem_OpenLocation As ToolStripMenuItem
    Friend WithEvents Btn_DisableSelectedItems As Button
    Friend WithEvents Btn_EnableSelectedItems As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents ToolTips As ToolTip
    Friend WithEvents Mst_Main As MenuStrip
    Friend WithEvents Msm_Main As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents Msm_Close As ToolStripMenuItem
    Friend WithEvents Msm_Filter As ToolStripMenuItem
    Friend WithEvents Msm_FiltersDisabledOnly As ToolStripMenuItem
    Friend WithEvents MenuItem_CreateLocalCopy As ToolStripMenuItem
    Friend WithEvents MenuItem_DeleteLocalAsset As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents MenuItem_RenameLocalAsset As ToolStripMenuItem
    Friend WithEvents Txt_InfoBar As TextBox
    Friend WithEvents Msm_About As ToolStripMenuItem
    Friend WithEvents MenuItem_EditLocalAsset As ToolStripMenuItem
    Friend WithEvents Msm_InstCustomAssets As ToolStripMenuItem
    Friend WithEvents Lbl_Loading As Label
    Friend WithEvents ProcLoading As Process
    Friend WithEvents Chk_ShowEAI As CheckBox

End Class
