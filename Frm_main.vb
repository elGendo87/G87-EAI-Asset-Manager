Imports System.IO

Public Class Frm_Main

    ' NEW: Declare Txt_InfoBar as Public (if not already done in designer)
    ' This is assumed to be a TextBox control named Txt_InfoBar on your form.
    ' If you added it in the designer, it's likely already declared.
    ' Public WithEvents Txt_InfoBar As TextBox ' Uncomment if you need to declare it here

    Private Sub Frm_Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormFormat()
        DisableCmbandBtns()
        UpdateActionButtonsState() ' Initialize button state on load
        Txt_InfoBar.Text = "" ' Clear info bar on load
    End Sub

    ' Set the form title here as ModScanner.vb no longer handles it
    Public Sub FormFormat()
        ' Me.Text = "[G87] Asset Manager - V1.2.3 Beta" ' Updated to V1.2.3 - REMOVED AGAIN AS PER USER REQUEST
        Me.Width = 1280
        Me.Height = 720
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MaximizeBox = True
        Me.MinimizeBox = True
        Me.MinimumSize = New Size(1280, 720)

        With Cmb_Mods
            .DropDownStyle = ComboBoxStyle.DropDownList
        End With

        With Cmb_AssetType
            .DropDownStyle = ComboBoxStyle.DropDownList
        End With

        With Cmb_Cat
            .DropDownStyle = ComboBoxStyle.DropDownList
        End With

        With Lst_Img
            .View = View.LargeIcon
            .LargeImageList = New ImageList() With {.ImageSize = New Size(128, 128)}
            .CheckBoxes = True
        End With

        ' NEW: Configure Txt_InfoBar
        If Txt_InfoBar IsNot Nothing Then
            Txt_InfoBar.ReadOnly = True ' Make it read-only
            Txt_InfoBar.BackColor = System.Drawing.SystemColors.Control ' Match background
            Txt_InfoBar.BorderStyle = BorderStyle.None ' No border
            Txt_InfoBar.Multiline = False ' Ensure it's a single line
        End If
    End Sub

    Private Sub Msm_Close_Click(sender As Object, e As EventArgs) Handles Msm_Close.Click
        End
    End Sub

    ' NEW: Event handler for Msm_About (About menu item)
    Private Sub Msm_About_Click(sender As Object, e As EventArgs) Handles Msm_About.Click
        MessageBox.Show("This program was created by @elgendo87 with the help of Gemini AI." & Environment.NewLine &
                        "Use it at your own risk.", "About Asset Manager", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Msm_ScanMods_Click(sender As Object, e As EventArgs) Handles Msm_ScanMods.Click
        IsFilterDisabledOnlyActive = False
        Msm_FiltersDisabledOnly.Checked = False

        ModScanner.currentModPath = ""
        ModScanner.currentAssetTypePath = ""
        ModScanner.currentCategoryPath = ""
        ModScanner.currentSelectedAssetTag = ""

        Btn_Scan.PerformClick()
    End Sub

    Private Sub Btn_Scan_Click(sender As Object, e As EventArgs) Handles Btn_Scan.Click
        IsFilterDisabledOnlyActive = False
        Msm_FiltersDisabledOnly.Checked = False

        ModScanner.currentModPath = ""
        ModScanner.currentAssetTypePath = ""
        ModScanner.currentCategoryPath = ""
        ModScanner.currentSelectedAssetTag = ""

        ScanFolders()
        UpdateActionButtonsState() ' Update button state after a full scan
        UpdateInfoBar() ' NEW: Update info bar after a full scan
    End Sub

    Private Sub Cmb_Mods_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_Mods.SelectedIndexChanged
        If Cmb_Mods.SelectedItem IsNot Nothing Then
            Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem
            Dim modRootPath As String = selectedMod.Value.ToString()
            ModScanner.currentModPath = modRootPath

            ' Clear asset type and category path before loading new ones
            ModScanner.currentAssetTypePath = ""
            ModScanner.currentCategoryPath = ""
            ModScanner.currentSelectedAssetTag = "" ' Also clear asset selection

            LoadAssetTypes(modRootPath)
            ModScanner.RestoreSelections() ' Explicitly call RestoreSelections here to ensure cascade
        End If
        UpdateActionButtonsState() ' Update button state after combo box change (which affects ListView)
        UpdateInfoBar() ' NEW: Update info bar when mod selection changes
    End Sub

    Private Sub Cmb_AssetType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_AssetType.SelectedIndexChanged
        If Cmb_AssetType.SelectedItem IsNot Nothing AndAlso Cmb_Mods.SelectedItem IsNot Nothing Then
            Dim selectedAssetType As ComboBoxItem = Cmb_AssetType.SelectedItem
            Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem

            Dim modRootPath As String = selectedMod.Value.ToString()
            Dim assetSubFolder As String = selectedAssetType.Value.ToString()
            Dim fullAssetPath As String = Path.Combine(modRootPath, assetSubFolder)
            ModScanner.currentAssetTypePath = assetSubFolder

            ' Clear category path before loading new ones
            ModScanner.currentCategoryPath = ""
            ModScanner.currentSelectedAssetTag = "" ' Also clear asset selection

            LoadCategories(fullAssetPath)
            ModScanner.RestoreSelections() ' Explicitly call RestoreSelections here to ensure cascade
        End If
        UpdateActionButtonsState() ' Update button state after combo box change (which affects ListView)
        ' UpdateInfoBar() is called by Cmb_Mods_SelectedIndexChanged or after actions, no need here.
    End Sub

    Private Sub Cmb_Cat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_Cat.SelectedIndexChanged
        If Cmb_Cat.SelectedItem IsNot Nothing AndAlso Cmb_AssetType.SelectedItem IsNot Nothing AndAlso Cmb_Mods.SelectedItem IsNot Nothing Then
            Dim selectedCategory As ComboBoxItem = Cmb_Cat.SelectedItem
            Dim selectedAssetType As ComboBoxItem = Cmb_AssetType.SelectedItem
            Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem

            Dim modRootPath As String = selectedMod.Value.ToString()
            Dim assetSubFolder As String = selectedAssetType.Value.ToString()
            Dim categoryFolder As String = selectedCategory.Value.ToString()

            Dim fullCategoryPath As String = Path.Combine(modRootPath, assetSubFolder, categoryFolder)
            ModScanner.currentCategoryPath = categoryFolder

            LoadAssetsIntoListView(fullCategoryPath)
        End If
        UpdateActionButtonsState() ' Update button state after combo box change (which affects ListView)
        ' UpdateInfoBar() is called by Cmb_Mods_SelectedIndexChanged or after actions, no need here.
    End Sub

    Private Sub Ctx_Asset_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Ctx_Asset.Opening
        If Lst_Img.SelectedItems.Count <> 1 Then
            e.Cancel = True
            Return
        End If

        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim currentFolderName As String = Path.GetFileName(selectedItem.Tag.ToString())
        Dim assetFullPath As String = selectedItem.Tag.ToString() ' Get full path for EAI check

        ' Determine if the selected asset is from the Extra Assets Importer folder
        Dim isEAIAsset As Boolean = assetFullPath.StartsWith(CustomFiles.GetEAICustomFolderPath(), StringComparison.OrdinalIgnoreCase)

        If currentFolderName.StartsWith(".") Then
            MenuItem_DisableAsset.Visible = False
            MenuItem_EnableAsset.Visible = True
            MenuItem_CreateLocalCopy.Visible = False ' Disable for disabled assets
            MenuItem_DeleteLocalAsset.Visible = False ' Disable for disabled assets
            MenuItem_RenameLocalAsset.Visible = False ' Disable for disabled assets
            MenuItem_EditLocalAsset.Visible = False ' NEW: Disable for disabled assets
        Else
            MenuItem_DisableAsset.Visible = True
            MenuItem_EnableAsset.Visible = False
            MenuItem_CreateLocalCopy.Visible = True ' Enable for enabled assets
            MenuItem_DeleteLocalAsset.Visible = isEAIAsset ' Enable only for EAI assets
            MenuItem_RenameLocalAsset.Visible = isEAIAsset ' Enable only for EAI assets
            MenuItem_EditLocalAsset.Visible = isEAIAsset ' NEW: Enable only for EAI assets
        End If

        MenuItem_OpenLocation.Visible = True
    End Sub

    ' Event handler for "Create local copy" context menu item
    Private Sub MenuItem_CreateLocalCopy_Click(sender As Object, e As EventArgs) Handles MenuItem_CreateLocalCopy.Click
        If Lst_Img.SelectedItems.Count = 0 Then Return

        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim sourceAssetPath As String = selectedItem.Tag.ToString()

        ' Ensure Asset Type and Category are selected before proceeding
        If Cmb_AssetType.SelectedItem Is Nothing OrElse Cmb_Cat.SelectedItem Is Nothing Then
            MessageBox.Show("Please select an Asset Type and Category first.", "Missing Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim assetType As String = DirectCast(Cmb_AssetType.SelectedItem, ComboBoxItem).Value.ToString()
        Dim categoryName As String = DirectCast(Cmb_Cat.SelectedItem, ComboBoxItem).Value.ToString()

        ' Call the function in the new CustomFiles module
        Dim copySuccessful As Boolean = CustomFiles.CreateLocalCopy(sourceAssetPath, assetType, categoryName)

        If copySuccessful Then
            ModScanner.StoreCurrentSelections()
            ModScanner.ScanFolders()
            UpdateActionButtonsState()
            UpdateInfoBar() ' NEW: Update info bar after copy
        End If
    End Sub

    ' Event handler for "Delete Local Asset" context menu item
    Private Sub MenuItem_DeleteLocalAsset_Click(sender As Object, e As EventArgs) Handles MenuItem_DeleteLocalAsset.Click
        If Lst_Img.SelectedItems.Count = 0 Then Return

        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim assetFullPath As String = selectedItem.Tag.ToString()

        ModScanner.currentSelectedAssetTag = ""

        ' Call the function in the CustomFiles module to handle deletion
        Dim deletionSuccessful As Boolean = CustomFiles.DeleteLocalAsset(assetFullPath)

        If deletionSuccessful Then
            ModScanner.StoreCurrentSelections()
            If IsFilterDisabledOnlyActive Then
                ModScanner.ScanFilteredFolders()
            Else
                ModScanner.ScanFolders()
            End If
            UpdateActionButtonsState()
            UpdateInfoBar() ' NEW: Update info bar after deletion
        End If
    End Sub

    ' Event handler for "Rename Local Asset" context menu item
    Private Sub MenuItem_RenameLocalAsset_Click(sender As Object, e As EventArgs) Handles MenuItem_RenameLocalAsset.Click
        If Lst_Img.SelectedItems.Count = 0 Then Return

        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim assetFullPath As String = selectedItem.Tag.ToString()

        ModScanner.currentSelectedAssetTag = assetFullPath

        ' Call the function in the CustomFiles module to handle renaming
        Dim renameSuccessful As Boolean = CustomFiles.RenameLocalAsset(assetFullPath)

        If renameSuccessful Then
            ModScanner.StoreCurrentSelections()
            If IsFilterDisabledOnlyActive Then
                ModScanner.ScanFilteredFolders()
            Else
                ModScanner.ScanFolders()
            End If
            UpdateActionButtonsState()
            UpdateInfoBar() ' NEW: Update info bar after rename
        End If
    End Sub

    ' NEW: Event handler for "Edit Asset Properties" context menu item
    Private Sub MenuItem_EditLocalAsset_Click(sender As Object, e As EventArgs) Handles MenuItem_EditLocalAsset.Click
        If Lst_Img.SelectedItems.Count = 0 Then Return

        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim assetFullPath As String = selectedItem.Tag.ToString()
        Dim assetName As String = selectedItem.Text ' Name displayed in ListView
        Dim assetIcon As Image = Lst_Img.LargeImageList.Images(selectedItem.ImageKey) ' Get the actual Image object

        ' Ensure Asset Type and Category are selected before proceeding
        If Cmb_AssetType.SelectedItem Is Nothing OrElse Cmb_Cat.SelectedItem Is Nothing Then
            MessageBox.Show("Please select an Asset Type and Category first.", "Missing Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim assetType As String = DirectCast(Cmb_AssetType.SelectedItem, ComboBoxItem).Text ' Use .Text for display
        Dim categoryName As String = DirectCast(Cmb_Cat.SelectedItem, ComboBoxItem).Text ' Use .Text for display

        Using assetEditorForm As New Frm_AssetEditor()
            ' Pass the data to the Frm_AssetEditor via EditorMod
            EditorMod.LoadAssetData(assetEditorForm, assetFullPath, assetName, assetIcon, assetType, categoryName) ' NEW: Call EditorMod
            assetEditorForm.ShowDialog() ' Show the form as a modal dialog
        End Using

        ' After the editor form closes, re-scan to reflect any potential changes (e.g., asset name)
        ModScanner.StoreCurrentSelections()
        If IsFilterDisabledOnlyActive Then
            ModScanner.ScanFilteredFolders()
        Else
            ModScanner.ScanFolders()
        End If
        UpdateActionButtonsState()
        UpdateInfoBar()
    End Sub

    Private Sub MenuItem_DisableAsset_Click(sender As Object, e As EventArgs) Handles MenuItem_DisableAsset.Click
        If Lst_Img.SelectedItems.Count = 0 Then Return

        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim assetFullPath As String = selectedItem.Tag.ToString()
        ModScanner.currentSelectedAssetTag = assetFullPath

        DisableAsset(assetFullPath)
        RecargarVistaActual()
        UpdateActionButtonsState()
        UpdateInfoBar() ' NEW: Update info bar after disabling
    End Sub

    Private Sub MenuItem_EnableAsset_Click(sender As Object, e As EventArgs) Handles MenuItem_EnableAsset.Click
        If Lst_Img.SelectedItems.Count = 0 Then Return

        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim assetFullPath As String = selectedItem.Tag.ToString()
        ModScanner.currentSelectedAssetTag = assetFullPath

        EnableAsset(assetFullPath)
        If IsFilterDisabledOnlyActive Then
            ModScanner.StoreCurrentSelections()
            ScanFilteredFolders()
        Else
            RecargarVistaActual()
        End If
        UpdateActionButtonsState()
        UpdateInfoBar() ' NEW: Update info bar after enabling
    End Sub

    Private Sub MenuItem_OpenLocation_Click(sender As Object, e As EventArgs) Handles MenuItem_OpenLocation.Click
        If Lst_Img.SelectedItems.Count = 1 Then
            Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
            Dim assetFullPath As String = selectedItem.Tag.ToString()

            If System.IO.Directory.Exists(assetFullPath) Then
                Try
                    Process.Start("explorer.exe", assetFullPath)
                Catch ex As Exception
                    MessageBox.Show("Could not open location: " & ex.Message, "Error Opening Folder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                MessageBox.Show("Item location does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub RecargarVistaActual()
        If Cmb_Cat.SelectedItem IsNot Nothing Then
            Dim selectedCategory As ComboBoxItem = Cmb_Cat.SelectedItem
            Dim modRootPath As String = DirectCast(Cmb_Mods.SelectedItem, ComboBoxItem).Value.ToString()
            Dim assetSubFolder As String = DirectCast(Cmb_AssetType.SelectedItem, ComboBoxItem).Value.ToString()
            Dim categoryFolder As String = selectedCategory.Value.ToString()

            Dim fullCategoryPath As String = Path.Combine(modRootPath, assetSubFolder, categoryFolder)

            LoadAssetsIntoListView(fullCategoryPath)
        End If
        UpdateActionButtonsState() ' Update button state after reloading list view
        ' UpdateInfoBar() is called by the specific action handlers, no need here.
    End Sub

    Private Sub Btn_DisableSelectedItems_Click(sender As Object, e As EventArgs) Handles Btn_DisableSelectedItems.Click
        If MessageBox.Show("Are you sure you want to disable the " & Lst_Img.CheckedItems.Count & " selected items?", "Confirm Disable", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim checkedAssetTags As New List(Of String)
            For Each item As ListViewItem In Lst_Img.CheckedItems
                checkedAssetTags.Add(item.Tag.ToString())
            Next

            For Each assetFullPath In checkedAssetTags
                DisableAsset(assetFullPath)
            Next

            If checkedAssetTags.Count > 0 Then
                ModScanner.currentSelectedAssetTag = checkedAssetTags(0)
            Else
                ModScanner.currentSelectedAssetTag = ""
            End If

            RecargarVistaActual()
            UpdateActionButtonsState()
            UpdateInfoBar() ' NEW: Update info bar after batch disable
        End If
    End Sub

    Private Sub Btn_EnableSelectedItems_Click(sender As Object, e As EventArgs) Handles Btn_EnableSelectedItems.Click
        If MessageBox.Show("Are you sure you want to enable the " & Lst_Img.CheckedItems.Count & " selected items?", "Confirm Enable", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim checkedAssetTags As New List(Of String)
            For Each item As ListViewItem In Lst_Img.CheckedItems
                checkedAssetTags.Add(item.Tag.ToString())
            Next

            For Each assetFullPath In checkedAssetTags
                EnableAsset(assetFullPath)
            Next

            If IsFilterDisabledOnlyActive Then
                ModScanner.StoreCurrentSelections()
                ScanFilteredFolders()
            Else
                RecargarVistaActual()
            End If
            UpdateActionButtonsState()
            UpdateInfoBar() ' NEW: Update info bar after batch enable
        End If
    End Sub

    Private Sub Msm_FiltersDisabledOnly_Click(sender As Object, e As EventArgs) Handles Msm_FiltersDisabledOnly.Click
        IsFilterDisabledOnlyActive = Not IsFilterDisabledOnlyActive
        Msm_FiltersDisabledOnly.Checked = IsFilterDisabledOnlyActive

        ModScanner.StoreCurrentSelections()
        If IsFilterDisabledOnlyActive Then
            ModScanner.ScanFilteredFolders()
        Else
            ModScanner.ScanFolders()
        End If
        UpdateActionButtonsState()
        UpdateInfoBar() ' NEW: Update info bar after filter change
    End Sub

    Private Sub Lst_Img_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles Lst_Img.ItemChecked
        UpdateActionButtonsState()
        UpdateInfoBar() ' NEW: Update info bar when item check state changes (affects counts/sizes)
    End Sub

    Private Sub Lst_Img_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Lst_Img.SelectedIndexChanged
        UpdateActionButtonsState()
        ' UpdateInfoBar() is called by Cmb_Mods_SelectedIndexChanged or after actions, no need here.
    End Sub

    Private Sub UpdateActionButtonsState()
        Dim checkedCount As Integer = Lst_Img.CheckedItems.Count
        Dim enabledCheckedCount As Integer = 0
        Dim disabledCheckedCount As Integer = 0

        If checkedCount > 0 Then
            For Each item As ListViewItem In Lst_Img.CheckedItems
                If Path.GetFileName(item.Tag.ToString()).StartsWith(".") Then
                    disabledCheckedCount += 1
                Else
                    enabledCheckedCount += 1
                End If
            Next
        End If

        If checkedCount > 0 Then
            If enabledCheckedCount > 0 AndAlso disabledCheckedCount = 0 Then
                Btn_DisableSelectedItems.Enabled = True
                Btn_EnableSelectedItems.Enabled = False
            ElseIf disabledCheckedCount > 0 AndAlso enabledCheckedCount = 0 Then
                Btn_DisableSelectedItems.Enabled = False
                Btn_EnableSelectedItems.Enabled = True
            Else
                Btn_DisableSelectedItems.Enabled = False
                Btn_EnableSelectedItems.Enabled = False
            End If
        Else
            Btn_DisableSelectedItems.Enabled = False
            Btn_EnableSelectedItems.Enabled = False
        End If
    End Sub

    ' NEW: Subroutine to update the Txt_InfoBar with mod statistics
    Private Sub UpdateInfoBar()
        If Cmb_Mods.SelectedItem IsNot Nothing Then
            Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem
            Dim modRootPath As String = selectedMod.Value.ToString()

            Dim stats As ModScanner.ModStatistics = ModScanner.CalculateModStatistics(modRootPath)

            Txt_InfoBar.Text = "Total Enabled Assets: " & stats.TotalEnabledAssets.ToString() & " (" & stats.TotalEnabledAssetSizeInMb.ToString("F2") & " MB) | " &
                               "Total Disabled Assets: " & stats.TotalDisabledAssets.ToString() & " (" & stats.TotalDisabledAssetSizeInMb.ToString("F2") & " MB)"
        Else
            Txt_InfoBar.Text = "No mod selected."
        End If
    End Sub

End Class
