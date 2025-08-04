Imports System.IO

Public Class Frm_Main
    ' In Frm_Main_Load, add the checkbox event handler
    Private Sub Frm_Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadingStart()

        FormFormat()
        DisableCmbandBtns()
        'UpdateActionButtonsState() ' Initialize button state on load

        LoadImagesFromDataFiles() 'Load images from data files on load
        ApplyLightThemeIcons() ' Apply light theme icons on load

        Txt_InfoBar.Text = "" ' Clear info bar on load

        DoModsScanning() ' Trigger initial scan to populate the UI

        LoadingStop()
    End Sub

    ' Set the form title here as ModScanner.vb no longer handles it
    Public Sub FormFormat()

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
            '.CheckBoxes = True
        End With

        ' Configure Txt_InfoBar
        If Txt_InfoBar IsNot Nothing Then
            Txt_InfoBar.ReadOnly = True ' Make it read-only
            Txt_InfoBar.BackColor = System.Drawing.SystemColors.Control ' Match background
            'Txt_InfoBar.BorderStyle = BorderStyle.None ' No border
            Txt_InfoBar.Multiline = False ' Ensure it's a single line
        End If
    End Sub

    Private Sub Msm_Close_Click(sender As Object, e As EventArgs) Handles Msm_Close.Click
        End
    End Sub

    ' Event handler for Msm_About (About menu item)
    Private Sub Msm_About_Click(sender As Object, e As EventArgs) Handles Msm_About.Click

        MessageBox.Show("This program was created by @elgendo87 with the help of AI (Gemini and Claude)." & Environment.NewLine &
                        "Thanks to @gagaxm for ideas and testing." & Environment.NewLine & Environment.NewLine &
                        "Use it at your own risk.", "About " & Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub Msm_ScanMods_Click(sender As Object, e As EventArgs)

        IsFilterDisabledOnlyActive = False
        Msm_FiltersDisabledOnly.Checked = False

        currentModPath = ""
        currentAssetTypePath = ""
        currentCategoryPath = ""
        currentSelectedAssetTag = ""

        DoModsScanning()

    End Sub

    Private Sub DoModsScanning()

        LoadingStart()

        IsFilterDisabledOnlyActive = False
        Msm_FiltersDisabledOnly.Checked = False

        currentModPath = ""
        currentAssetTypePath = ""
        currentCategoryPath = ""
        currentSelectedAssetTag = ""

        ScanFolders()
        'UpdateActionButtonsState() ' Update button state after a full scan
        UpdateInfoBar() ' Update info bar after a full scan

        LoadingStop()

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
        'UpdateActionButtonsState() ' Update button state after combo box change (which affects ListView)
        UpdateInfoBar() ' Update info bar when mod selection changes


    End Sub

    'modified with Claude
    Private Sub Cmb_AssetType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_AssetType.SelectedIndexChanged
        If Cmb_AssetType.SelectedItem IsNot Nothing Then
            Dim selectedAssetType As ComboBoxItem = Cmb_AssetType.SelectedItem
            Dim assetType As String = selectedAssetType.Value.ToString()

            If Chk_ShowEAI.Checked Then
                ' EAI Mode: Load categories from all mods
                LoadCategoriesFromAllMods(assetType)
            Else
                ' Normal Mode: Existing behavior
                If Cmb_Mods.SelectedItem IsNot Nothing Then
                    Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem
                    Dim modRootPath As String = selectedMod.Value.ToString()
                    Dim fullAssetPath As String = Path.Combine(modRootPath, assetType)
                    ModScanner.currentAssetTypePath = assetType
                    ModScanner.currentCategoryPath = ""
                    ModScanner.currentSelectedAssetTag = ""
                    LoadCategories(fullAssetPath)
                    ModScanner.RestoreSelections()
                End If
            End If
        End If
        'UpdateActionButtonsState()
    End Sub

    'modified with Claude
    Private Sub Cmb_Cat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_Cat.SelectedIndexChanged
        If Cmb_Cat.SelectedItem IsNot Nothing AndAlso Cmb_AssetType.SelectedItem IsNot Nothing Then
            Dim selectedCategory As ComboBoxItem = Cmb_Cat.SelectedItem
            Dim selectedAssetType As ComboBoxItem = Cmb_AssetType.SelectedItem

            If Chk_ShowEAI.Checked Then
                ' EAI Mode: Load assets from all mods
                LoadAssetsFromAllMods(selectedAssetType.Value.ToString(), selectedCategory.Value.ToString())
            Else
                ' Normal Mode: Existing behavior
                If Cmb_Mods.SelectedItem IsNot Nothing Then
                    Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem
                    Dim modRootPath As String = selectedMod.Value.ToString()
                    Dim fullCategoryPath As String = Path.Combine(modRootPath, selectedAssetType.Value.ToString(), selectedCategory.Value.ToString())
                    ModScanner.currentCategoryPath = selectedCategory.Value.ToString()
                    LoadAssetsIntoListView(fullCategoryPath)
                End If
            End If
        End If
        'UpdateActionButtonsState()
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

        If currentFolderName.StartsWith("."c) Then
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
            ModScanner.RescanAndRefresh() ' Use unified rescan function
            'UpdateActionButtonsState()
            UpdateInfoBar()
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
            ModScanner.RescanAndRefresh() ' Use unified rescan function
            'UpdateActionButtonsState()
            UpdateInfoBar()
        End If
        ' If deletion was successful, the asset will be removed from the ListView

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
            ModScanner.RescanAndRefresh() ' Use unified rescan function
            'UpdateActionButtonsState()
            UpdateInfoBar()
        End If

    End Sub

    ' Event handler for "Edit Local Asset" context menu item Claude
    Private Sub MenuItem_EditLocalAsset_Click(sender As Object, e As EventArgs) Handles MenuItem_EditLocalAsset.Click
        If Lst_Img.SelectedItems.Count = 0 Then Return

        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim assetFullPath As String = selectedItem.Tag.ToString()
        Dim assetName As String = selectedItem.Text
        Dim assetIcon As Image = Lst_Img.LargeImageList.Images(selectedItem.ImageKey)

        If Cmb_AssetType.SelectedItem Is Nothing OrElse Cmb_Cat.SelectedItem Is Nothing Then
            MessageBox.Show("Please select an Asset Type and Category first.", "Missing Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim assetType As String = DirectCast(Cmb_AssetType.SelectedItem, ComboBoxItem).Text
        Dim categoryName As String = DirectCast(Cmb_Cat.SelectedItem, ComboBoxItem).Text

        Using assetEditorForm As New Frm_AssetEditor()
            EditorMod.LoadAssetData(assetEditorForm, assetFullPath, assetName, assetIcon, assetType, categoryName)
            assetEditorForm.ShowDialog()
        End Using

        ' After the editor form closes, re-scan to reflect any potential changes
        ModScanner.RescanAndRefresh() ' Use unified rescan function
        'UpdateActionButtonsState()
        UpdateInfoBar()

    End Sub

    Private Sub MenuItem_DisableAsset_Click(sender As Object, e As EventArgs) Handles MenuItem_DisableAsset.Click
        If Lst_Img.SelectedItems.Count = 0 Then Return

        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim assetFullPath As String = selectedItem.Tag.ToString()
        ModScanner.currentSelectedAssetTag = assetFullPath

        DisableAsset(assetFullPath)
        ModScanner.RefreshCurrentView() ' Use unified refresh function
        'UpdateActionButtonsState()
        UpdateInfoBar()
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
            ModScanner.RefreshCurrentView() ' Use unified refresh function
        End If
        'UpdateActionButtonsState()
        UpdateInfoBar()
    End Sub

    ' Also update the batch operations
    Private Sub Btn_DisableSelectedItems_Click(sender As Object, e As EventArgs) Handles Btn_DisableSelectedItems.Click
        If MessageBox.Show("Are you sure you want to disable the " & Lst_Img.SelectedItems.Count & " selected items?", "Confirm Disable", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim selectedAssetTags As New List(Of String)
            For Each item As ListViewItem In Lst_Img.SelectedItems
                selectedAssetTags.Add(item.Tag.ToString())
            Next

            For Each assetFullPath In selectedAssetTags
                DisableAsset(assetFullPath)
            Next

            If selectedAssetTags.Count > 0 Then
                ModScanner.currentSelectedAssetTag = selectedAssetTags(0)
            Else
                ModScanner.currentSelectedAssetTag = ""
            End If

            ModScanner.RefreshCurrentView() ' Use unified refresh function
            'UpdateActionButtonsState()
            UpdateInfoBar()
        End If
    End Sub

    Private Sub Btn_EnableSelectedItems_Click(sender As Object, e As EventArgs) Handles Btn_EnableSelectedItems.Click
        If MessageBox.Show("Are you sure you want to enable the " & Lst_Img.SelectedItems.Count & " selected items?", "Confirm Enable", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim selectedAssetTags As New List(Of String)
            For Each item As ListViewItem In Lst_Img.SelectedItems
                selectedAssetTags.Add(item.Tag.ToString())
            Next

            For Each assetFullPath In selectedAssetTags
                EnableAsset(assetFullPath)
            Next

            If IsFilterDisabledOnlyActive Then
                ModScanner.StoreCurrentSelections()
                ScanFilteredFolders()
            Else
                ModScanner.RefreshCurrentView() ' Use unified refresh function
            End If
            'UpdateActionButtonsState()
            UpdateInfoBar()
        End If
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

    ' Enable/Disable buttons based on selection
    Private Sub Lst_Img_SelectionChanged(sender As Object, e As EventArgs) Handles Lst_Img.SelectedIndexChanged
        ' Counters for enabled and disabled items
        Dim enabledSelectedCount As Integer = 0
        Dim disabledSelectedCount As Integer = 0

        For Each item As ListViewItem In Lst_Img.SelectedItems

            If Path.GetFileName(item.Tag.ToString()).StartsWith("."c) Then
                disabledSelectedCount += 1
            Else
                enabledSelectedCount += 1
            End If

        Next

        If enabledSelectedCount > 0 AndAlso disabledSelectedCount = 0 Then

            Btn_DisableSelectedItems.Enabled = True
            Btn_EnableSelectedItems.Enabled = False

        ElseIf disabledSelectedCount > 0 AndAlso enabledSelectedCount = 0 Then

            Btn_DisableSelectedItems.Enabled = False
            Btn_EnableSelectedItems.Enabled = True

        Else

            Btn_DisableSelectedItems.Enabled = False
            Btn_EnableSelectedItems.Enabled = False

        End If

        ' Logic for Txt_InfoBar based on Chk_ShowEAI
        If Chk_ShowEAI.Checked Then
            Select Case Lst_Img.SelectedItems.Count
                Case 0
                    ' No item selected
                    Txt_InfoBar.Text = "Select an item to show info."
                Case 1
                    ' A single item selected
                    Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
                    Dim imageKey As String = selectedItem.ImageKey

                    ' Extract the mod name from the ImageKey
                    Dim modName As String
                    Dim lastUnderscoreIndex As Integer = imageKey.LastIndexOf("_")
                    If lastUnderscoreIndex >= 0 Then
                        modName = imageKey.Substring(0, lastUnderscoreIndex)
                    Else
                        modName = "Unknown Mod" ' If the format is not as expected
                    End If

                    Txt_InfoBar.Text = $"From the mod: {modName}"
                Case Is > 1
                    ' Multiple items selected
                    If enabledSelectedCount > 0 AndAlso disabledSelectedCount > 0 Then
                        ' Mixed selection
                        Txt_InfoBar.Text = $"Selected items: {Lst_Img.SelectedItems.Count} (Mixed)"
                    Else
                        ' Homogeneous selection
                        Txt_InfoBar.Text = $"Selected items: {Lst_Img.SelectedItems.Count}"
                    End If
            End Select
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
        'UpdateActionButtonsState()
        UpdateInfoBar() ' NEW: Update info bar after filter change
    End Sub

    'Private Sub Lst_Img_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles Lst_Img.ItemChecked
    '    UpdateActionButtonsState()
    '    UpdateInfoBar() ' NEW: Update info bar when item check state changes (affects counts/sizes)
    'End Sub

    Private Sub Lst_Img_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Lst_Img.SelectedIndexChanged
        'UpdateActionButtonsState()
        ' UpdateInfoBar() is called by Cmb_Mods_SelectedIndexChanged or after actions, no need here.
    End Sub

    'Private Sub UpdateActionButtonsState()
    '    Dim checkedCount As Integer = Lst_Img.CheckedItems.Count
    '    Dim enabledCheckedCount As Integer = 0
    '    Dim disabledCheckedCount As Integer = 0

    '    If checkedCount > 0 Then
    '        For Each item As ListViewItem In Lst_Img.CheckedItems
    '            If Path.GetFileName(item.Tag.ToString()).StartsWith("."c) Then
    '                disabledCheckedCount += 1
    '            Else
    '                enabledCheckedCount += 1
    '            End If
    '        Next
    '    End If

    '    If checkedCount > 0 Then
    '        If enabledCheckedCount > 0 AndAlso disabledCheckedCount = 0 Then
    '            Btn_DisableSelectedItems.Enabled = True
    '            Btn_EnableSelectedItems.Enabled = False
    '        ElseIf disabledCheckedCount > 0 AndAlso enabledCheckedCount = 0 Then
    '            Btn_DisableSelectedItems.Enabled = False
    '            Btn_EnableSelectedItems.Enabled = True
    '        Else
    '            Btn_DisableSelectedItems.Enabled = False
    '            Btn_EnableSelectedItems.Enabled = False
    '        End If
    '    Else
    '        Btn_DisableSelectedItems.Enabled = False
    '        Btn_EnableSelectedItems.Enabled = False
    '    End If
    'End Sub

    ' Subroutine to update the Txt_InfoBar with mod statistics
    Private Sub UpdateInfoBar()
        If Cmb_Mods.SelectedItem IsNot Nothing Then
            Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem
            Dim modRootPath As String = selectedMod.Value.ToString()

            Dim stats As ModScanner.ModStatistics = ModScanner.CalculateModStatistics(modRootPath)

            ' Use a StringBuilder for efficient string construction
            Dim infoText As New System.Text.StringBuilder()

            ' NEW: Prepend metadata only if it exists (i.e., not the EAI local assets folder)
            If Not String.IsNullOrEmpty(stats.ModID) Then
                infoText.Append(stats.ModID)
                infoText.Append("  │  ")
                infoText.Append(stats.ModAuthor)
                infoText.Append("  │  v")
                infoText.Append(stats.ModVersion)
                infoText.Append("  │  ")
            End If

            ' Append the existing asset statistics
            infoText.Append("Total Enabled Assets: ")
            infoText.Append(stats.TotalEnabledAssets) ' .ToString()) ' Changed to append directly
            infoText.Append(" (")
            infoText.Append(stats.TotalEnabledAssetSizeInMb.ToString("F2"))
            infoText.Append(" MB)  │  ") ' Using the same separator for consistency
            infoText.Append("Total Disabled Assets: ")
            infoText.Append(stats.TotalDisabledAssets) '.ToString()) ' changed to append directly
            infoText.Append(" (")
            infoText.Append(stats.TotalDisabledAssetSizeInMb.ToString("F2"))
            infoText.Append(" MB)")

            Txt_InfoBar.Text = infoText.ToString()
        Else
            Txt_InfoBar.Text = "No mod selected."
        End If
    End Sub

    Private Sub Msm_InstCustomAssets_Click(sender As Object, e As EventArgs) Handles Msm_InstCustomAssets.Click
        Using installerForm As New Frm_InstCustomAssets()
            installerForm.ShowDialog()
        End Using
    End Sub

    'Show EAI checkbox event handler
    Private Sub Chk_ShowEAI_CheckedChanged(sender As Object, e As EventArgs) Handles Chk_ShowEAI.CheckedChanged
        If Chk_ShowEAI.Checked Then
            ' EAI Mode: Store current mod selection, disable Cmb_Mods and change to EAI display
            StoreCurrentModSelection()
            SetEAIModeDisplay()
            LoadAssetTypesFromAllMods()
        Else
            ' Normal Mode: Restore mods list, enable Cmb_Mods and refresh display
            RestoreNormalModeDisplay()
            RestoreAndRefreshModSelection()
        End If
    End Sub
End Class
