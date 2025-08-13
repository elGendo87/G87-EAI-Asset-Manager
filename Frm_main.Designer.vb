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
        MenuItem_EnableAsset = New ToolStripMenuItem()
        MenuItem_DisableAsset = New ToolStripMenuItem()
        Ctx_SepEnDis = New ToolStripSeparator()
        MenuItem_OpenLocation = New ToolStripMenuItem()
        Ctx_SepOpenLocal = New ToolStripSeparator()
        MenuItem_CreateLocalCopy = New ToolStripMenuItem()
        MenuItem_DeleteLocalAsset = New ToolStripMenuItem()
        MenuItem_RenameLocalAsset = New ToolStripMenuItem()
        MenuItem_EditLocalAsset = New ToolStripMenuItem()
        Ctx_ChangeCat = New ToolStripMenuItem()
        MenuItem_AssetProperties = New ToolStripMenuItem()
        Ctx_SepBulk = New ToolStripSeparator()
        Ctx_BulkOperations = New ToolStripMenuItem()
        Ctx_Bulk_EnableAssets = New ToolStripMenuItem()
        Ctx_Bulk_DisableAssets = New ToolStripMenuItem()
        Ctx_Bulk_SepEnDis = New ToolStripSeparator()
        Ctx_Bulk_DeleteAssets = New ToolStripMenuItem()
        Ctx_Bulk_SepActions = New ToolStripSeparator()
        Ctx_Bulk_SetUiPriority = New ToolStripMenuItem()
        Ctx_Bulk_SetDrawOrder = New ToolStripMenuItem()
        Ctx_Bulk_SetDLM = New ToolStripMenuItem()
        Cmb_Mods = New ComboBox()
        Cmb_AssetType = New ComboBox()
        Cmb_Cat = New ComboBox()
        Btn_DisableSelectedItems = New Button()
        Btn_EnableSelectedItems = New Button()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        ToolTips = New ToolTip(components)
        Btn_GoLocal = New Button()
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
        Ctx_Asset.Items.AddRange(New ToolStripItem() {MenuItem_EnableAsset, MenuItem_DisableAsset, Ctx_SepEnDis, MenuItem_OpenLocation, Ctx_SepOpenLocal, MenuItem_CreateLocalCopy, MenuItem_DeleteLocalAsset, MenuItem_RenameLocalAsset, MenuItem_EditLocalAsset, Ctx_ChangeCat, MenuItem_AssetProperties, Ctx_SepBulk, Ctx_BulkOperations})
        Ctx_Asset.Name = "Ctx_Asset"
        Ctx_Asset.Size = New Size(198, 242)
        ' 
        ' MenuItem_EnableAsset
        ' 
        MenuItem_EnableAsset.Name = "MenuItem_EnableAsset"
        MenuItem_EnableAsset.Size = New Size(197, 22)
        MenuItem_EnableAsset.Text = "Enable Asset"
        MenuItem_EnableAsset.ToolTipText = "Will enable a disabled asset to allow it to load in game."
        ' 
        ' MenuItem_DisableAsset
        ' 
        MenuItem_DisableAsset.Name = "MenuItem_DisableAsset"
        MenuItem_DisableAsset.Size = New Size(197, 22)
        MenuItem_DisableAsset.Text = "Disable Asset"
        MenuItem_DisableAsset.ToolTipText = "Will disable the asset, preventing it to load in game."
        ' 
        ' Ctx_SepEnDis
        ' 
        Ctx_SepEnDis.Name = "Ctx_SepEnDis"
        Ctx_SepEnDis.Size = New Size(194, 6)
        ' 
        ' MenuItem_OpenLocation
        ' 
        MenuItem_OpenLocation.Name = "MenuItem_OpenLocation"
        MenuItem_OpenLocation.Size = New Size(197, 22)
        MenuItem_OpenLocation.Text = "Open in File Explorer"
        MenuItem_OpenLocation.ToolTipText = "Will open the folder location of the asset in File Explorer."
        ' 
        ' Ctx_SepOpenLocal
        ' 
        Ctx_SepOpenLocal.Name = "Ctx_SepOpenLocal"
        Ctx_SepOpenLocal.Size = New Size(194, 6)
        ' 
        ' MenuItem_CreateLocalCopy
        ' 
        MenuItem_CreateLocalCopy.Name = "MenuItem_CreateLocalCopy"
        MenuItem_CreateLocalCopy.Size = New Size(197, 22)
        MenuItem_CreateLocalCopy.Text = "Create Local Copy"
        MenuItem_CreateLocalCopy.ToolTipText = "Create a local copy from a mod asset." & vbCrLf & vbCrLf & "Local copies of asset can be edited."
        ' 
        ' MenuItem_DeleteLocalAsset
        ' 
        MenuItem_DeleteLocalAsset.Name = "MenuItem_DeleteLocalAsset"
        MenuItem_DeleteLocalAsset.Size = New Size(197, 22)
        MenuItem_DeleteLocalAsset.Text = "Delete Local Asset"
        MenuItem_DeleteLocalAsset.ToolTipText = "Use to delete a local asset." & vbCrLf & vbCrLf & "Will sent it to the Recycle Bin."
        ' 
        ' MenuItem_RenameLocalAsset
        ' 
        MenuItem_RenameLocalAsset.Name = "MenuItem_RenameLocalAsset"
        MenuItem_RenameLocalAsset.Size = New Size(197, 22)
        MenuItem_RenameLocalAsset.Text = "Rename Local Asset"
        MenuItem_RenameLocalAsset.ToolTipText = "Use to rename local assets."
        ' 
        ' MenuItem_EditLocalAsset
        ' 
        MenuItem_EditLocalAsset.Name = "MenuItem_EditLocalAsset"
        MenuItem_EditLocalAsset.Size = New Size(197, 22)
        MenuItem_EditLocalAsset.Text = "Edit Asset Properties"
        MenuItem_EditLocalAsset.ToolTipText = "Open the Asset Editor."
        ' 
        ' Ctx_ChangeCat
        ' 
        Ctx_ChangeCat.Name = "Ctx_ChangeCat"
        Ctx_ChangeCat.Size = New Size(197, 22)
        Ctx_ChangeCat.Text = "Change Asset Category"
        Ctx_ChangeCat.ToolTipText = "Open the asset category selector."
        ' 
        ' MenuItem_AssetProperties
        ' 
        MenuItem_AssetProperties.Name = "MenuItem_AssetProperties"
        MenuItem_AssetProperties.Size = New Size(197, 22)
        MenuItem_AssetProperties.Text = "View Asset Json"
        MenuItem_AssetProperties.ToolTipText = "Open asset JSON file as plain text to read it's content as is."
        ' 
        ' Ctx_SepBulk
        ' 
        Ctx_SepBulk.Name = "Ctx_SepBulk"
        Ctx_SepBulk.Size = New Size(194, 6)
        ' 
        ' Ctx_BulkOperations
        ' 
        Ctx_BulkOperations.DropDownItems.AddRange(New ToolStripItem() {Ctx_Bulk_EnableAssets, Ctx_Bulk_DisableAssets, Ctx_Bulk_SepEnDis, Ctx_Bulk_DeleteAssets, Ctx_Bulk_SepActions, Ctx_Bulk_SetUiPriority, Ctx_Bulk_SetDrawOrder, Ctx_Bulk_SetDLM})
        Ctx_BulkOperations.Name = "Ctx_BulkOperations"
        Ctx_BulkOperations.Size = New Size(197, 22)
        Ctx_BulkOperations.Text = "Bulk Actions"
        Ctx_BulkOperations.ToolTipText = "This will allow to do bulk actions on all the selected" & vbCrLf & "assets."
        ' 
        ' Ctx_Bulk_EnableAssets
        ' 
        Ctx_Bulk_EnableAssets.Name = "Ctx_Bulk_EnableAssets"
        Ctx_Bulk_EnableAssets.Size = New Size(195, 22)
        Ctx_Bulk_EnableAssets.Text = "Enable Selected Assets"
        Ctx_Bulk_EnableAssets.ToolTipText = "This will enable all the selected assets."
        ' 
        ' Ctx_Bulk_DisableAssets
        ' 
        Ctx_Bulk_DisableAssets.Name = "Ctx_Bulk_DisableAssets"
        Ctx_Bulk_DisableAssets.Size = New Size(195, 22)
        Ctx_Bulk_DisableAssets.Text = "Disable Selected Assets"
        Ctx_Bulk_DisableAssets.ToolTipText = "This will disable all the selected assets."
        ' 
        ' Ctx_Bulk_SepEnDis
        ' 
        Ctx_Bulk_SepEnDis.Name = "Ctx_Bulk_SepEnDis"
        Ctx_Bulk_SepEnDis.Size = New Size(192, 6)
        ' 
        ' Ctx_Bulk_DeleteAssets
        ' 
        Ctx_Bulk_DeleteAssets.Name = "Ctx_Bulk_DeleteAssets"
        Ctx_Bulk_DeleteAssets.Size = New Size(195, 22)
        Ctx_Bulk_DeleteAssets.Text = "Delete Selected Assets"
        Ctx_Bulk_DeleteAssets.ToolTipText = "This will delete all the selected assets." & vbCrLf & vbCrLf & "The folders will be sent to the Recycle Bin."
        ' 
        ' Ctx_Bulk_SepActions
        ' 
        Ctx_Bulk_SepActions.Name = "Ctx_Bulk_SepActions"
        Ctx_Bulk_SepActions.Size = New Size(192, 6)
        ' 
        ' Ctx_Bulk_SetUiPriority
        ' 
        Ctx_Bulk_SetUiPriority.Name = "Ctx_Bulk_SetUiPriority"
        Ctx_Bulk_SetUiPriority.Size = New Size(195, 22)
        Ctx_Bulk_SetUiPriority.Text = "Set UiPriority"
        Ctx_Bulk_SetUiPriority.ToolTipText = "Use this to change the UiPriority of all the" & vbCrLf & "selected assets." & vbCrLf & vbCrLf & "The UiPriority will start by alphabetical order" & vbCrLf & "increasing the starting number by one."
        ' 
        ' Ctx_Bulk_SetDrawOrder
        ' 
        Ctx_Bulk_SetDrawOrder.Name = "Ctx_Bulk_SetDrawOrder"
        Ctx_Bulk_SetDrawOrder.Size = New Size(195, 22)
        Ctx_Bulk_SetDrawOrder.Text = "Set Draw Order"
        Ctx_Bulk_SetDrawOrder.ToolTipText = "Use this to set the same Draw Order to all the" & vbCrLf & "selected assets." & vbCrLf & vbCrLf & "The allowed values go from -170 to 200."
        ' 
        ' Ctx_Bulk_SetDLM
        ' 
        Ctx_Bulk_SetDLM.Name = "Ctx_Bulk_SetDLM"
        Ctx_Bulk_SetDLM.Size = New Size(195, 22)
        Ctx_Bulk_SetDLM.Text = "Set Decal Layer Mask"
        Ctx_Bulk_SetDLM.ToolTipText = "This will set the same Decal Layer Mask to all" & vbCrLf & "the selected assets." & vbCrLf & vbCrLf & "Read the instructions in the input window."
        ' 
        ' Cmb_Mods
        ' 
        Cmb_Mods.FormattingEnabled = True
        Cmb_Mods.Location = New Point(221, 32)
        Cmb_Mods.Name = "Cmb_Mods"
        Cmb_Mods.Size = New Size(336, 23)
        Cmb_Mods.TabIndex = 1
        Cmb_Mods.TabStop = False
        ToolTips.SetToolTip(Cmb_Mods, "List of subscribed mods that contain decals, netlanes or surfaces to be managed.")
        ' 
        ' Cmb_AssetType
        ' 
        Cmb_AssetType.FormattingEnabled = True
        Cmb_AssetType.Location = New Point(870, 32)
        Cmb_AssetType.Name = "Cmb_AssetType"
        Cmb_AssetType.Size = New Size(108, 23)
        Cmb_AssetType.TabIndex = 3
        Cmb_AssetType.TabStop = False
        ToolTips.SetToolTip(Cmb_AssetType, "Type of assets to manage:" & vbCrLf & "- Decals" & vbCrLf & "- Netlanes" & vbCrLf & "- Surfaces" & vbCrLf & vbCrLf & "Only existing type will be shown in the list.")
        ' 
        ' Cmb_Cat
        ' 
        Cmb_Cat.FormattingEnabled = True
        Cmb_Cat.Location = New Point(1098, 32)
        Cmb_Cat.Name = "Cmb_Cat"
        Cmb_Cat.Size = New Size(152, 23)
        Cmb_Cat.TabIndex = 4
        Cmb_Cat.TabStop = False
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
        Label2.Location = New Point(799, 37)
        Label2.Name = "Label2"
        Label2.Size = New Size(65, 15)
        Label2.TabIndex = 8
        Label2.Text = "Asset Type:"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(1003, 37)
        Label3.Name = "Label3"
        Label3.Size = New Size(89, 15)
        Label3.TabIndex = 9
        Label3.Text = "Asset Category:"
        ' 
        ' Btn_GoLocal
        ' 
        Btn_GoLocal.ImageAlign = ContentAlignment.MiddleLeft
        Btn_GoLocal.Location = New Point(563, 31)
        Btn_GoLocal.Name = "Btn_GoLocal"
        Btn_GoLocal.Size = New Size(113, 25)
        Btn_GoLocal.TabIndex = 15
        Btn_GoLocal.Text = "EAI Local Assets"
        Btn_GoLocal.TextAlign = ContentAlignment.MiddleRight
        ToolTips.SetToolTip(Btn_GoLocal, "Select Extra Assets Importer (Local Assets) mod from the listed mods.")
        Btn_GoLocal.UseVisualStyleBackColor = True
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
        Msm_InstCustomAssets.ToolTipText = "This option will open the Custom Assets Installer." & vbCrLf & vbCrLf & "Is still in developement. May have bugs."
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
        Msm_Close.ToolTipText = "Exit the program."
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
        ' Chk_ShowEAI
        ' 
        Chk_ShowEAI.AutoSize = True
        Chk_ShowEAI.Location = New Point(15, 36)
        Chk_ShowEAI.Name = "Chk_ShowEAI"
        Chk_ShowEAI.Size = New Size(80, 19)
        Chk_ShowEAI.TabIndex = 14
        Chk_ShowEAI.Text = "EAI MODE"
        Chk_ShowEAI.UseVisualStyleBackColor = True
        ' 
        ' Frm_Main
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1264, 681)
        Controls.Add(Btn_GoLocal)
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
        Text = "[G87] EAI Asset Manager - v1.4.6.3 Beta"
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
    Friend WithEvents Ctx_SepEnDis As ToolStripSeparator
    Friend WithEvents Ctx_SepOpenLocal As ToolStripSeparator
    Friend WithEvents MenuItem_RenameLocalAsset As ToolStripMenuItem
    Friend WithEvents Txt_InfoBar As TextBox
    Friend WithEvents Msm_About As ToolStripMenuItem
    Friend WithEvents MenuItem_EditLocalAsset As ToolStripMenuItem
    Friend WithEvents Msm_InstCustomAssets As ToolStripMenuItem
    Friend WithEvents Lbl_Loading As Label
    Friend WithEvents Chk_ShowEAI As CheckBox
    Friend WithEvents Btn_GoLocal As Button
    Friend WithEvents MenuItem_AssetProperties As ToolStripMenuItem
    Friend WithEvents Ctx_SepBulk As ToolStripSeparator
    Friend WithEvents Ctx_BulkOperations As ToolStripMenuItem
    Friend WithEvents Ctx_Bulk_DeleteAssets As ToolStripMenuItem
    Friend WithEvents Ctx_Bulk_SetUiPriority As ToolStripMenuItem
    Friend WithEvents Ctx_Bulk_SetDrawOrder As ToolStripMenuItem
    Friend WithEvents Ctx_Bulk_SetDLM As ToolStripMenuItem
    Friend WithEvents Ctx_Bulk_EnableAssets As ToolStripMenuItem
    Friend WithEvents Ctx_Bulk_DisableAssets As ToolStripMenuItem
    Friend WithEvents Ctx_Bulk_SepEnDis As ToolStripSeparator
    Friend WithEvents Ctx_Bulk_SepActions As ToolStripSeparator
    Friend WithEvents Ctx_ChangeCat As ToolStripMenuItem

End Class
