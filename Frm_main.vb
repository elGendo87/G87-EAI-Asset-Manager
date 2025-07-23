Imports System.IO

Public Class Frm_Main

    Private Sub Frm_Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormFormat()
        DisableCmbandBtns()
        UpdateActionButtonsState() ' Initialize button state on load
    End Sub

    Private Sub Msm_Close_Click(sender As Object, e As EventArgs) Handles Msm_Close.Click
        End
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
    End Sub

    Private Sub Cmb_Mods_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_Mods.SelectedIndexChanged
        If Cmb_Mods.SelectedItem IsNot Nothing Then
            Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem
            Dim modRootPath As String = selectedMod.Value.ToString()
            ModScanner.currentModPath = modRootPath

            LoadAssetTypes(modRootPath)
        End If
        UpdateActionButtonsState() ' Update button state after combo box change (which affects ListView)
    End Sub

    Private Sub Cmb_AssetType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_AssetType.SelectedIndexChanged
        If Cmb_AssetType.SelectedItem IsNot Nothing AndAlso Cmb_Mods.SelectedItem IsNot Nothing Then
            Dim selectedAssetType As ComboBoxItem = Cmb_AssetType.SelectedItem
            Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem

            Dim modRootPath As String = selectedMod.Value.ToString()
            Dim assetSubFolder As String = selectedAssetType.Value.ToString()
            Dim fullAssetPath As String = Path.Combine(modRootPath, assetSubFolder)
            ModScanner.currentAssetTypePath = assetSubFolder

            LoadCategories(fullAssetPath)
        End If
        UpdateActionButtonsState() ' Update button state after combo box change (which affects ListView)
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
    End Sub

    Private Sub Ctx_Asset_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Ctx_Asset.Opening
        If Lst_Img.SelectedItems.Count <> 1 Then
            e.Cancel = True
            Return
        End If

        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim currentFolderName As String = Path.GetFileName(selectedItem.Tag.ToString())

        If currentFolderName.StartsWith(".") Then
            MenuItem_DisableAsset.Visible = False
            MenuItem_EnableAsset.Visible = True
        Else
            MenuItem_DisableAsset.Visible = True
            MenuItem_EnableAsset.Visible = False
        End If

        MenuItem_OpenLocation.Visible = True
    End Sub

    Private Sub MenuItem_DisableAsset_Click(sender As Object, e As EventArgs) Handles MenuItem_DisableAsset.Click
        If Lst_Img.SelectedItems.Count = 0 Then Return

        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim assetFullPath As String = selectedItem.Tag.ToString()
        ModScanner.currentSelectedAssetTag = assetFullPath

        DisableAsset(assetFullPath)
        RecargarVistaActual()
        UpdateActionButtonsState() ' Update button state after action
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
        UpdateActionButtonsState() ' Update button state after action
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
            UpdateActionButtonsState() ' Update button state after action
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

            If checkedAssetTags.Count > 0 Then
                ModScanner.currentSelectedAssetTag = checkedAssetTags(0)
            Else
                ModScanner.currentSelectedAssetTag = ""
            End If

            If IsFilterDisabledOnlyActive Then
                ModScanner.StoreCurrentSelections()
                ScanFilteredFolders()
            Else
                RecargarVistaActual()
            End If
            UpdateActionButtonsState() ' Update button state after action
        End If
    End Sub

    Private Sub Msm_FiltersDisabledOnly_Click(sender As Object, e As EventArgs) Handles Msm_FiltersDisabledOnly.Click
        IsFilterDisabledOnlyActive = Not IsFilterDisabledOnlyActive
        Msm_FiltersDisabledOnly.Checked = IsFilterDisabledOnlyActive

        ModScanner.StoreCurrentSelections()
        If IsFilterDisabledOnlyActive Then
            ScanFilteredFolders()
        Else
            ScanFolders()
        End If
        UpdateActionButtonsState() ' Update button state after filter change
    End Sub

    Private Sub Lst_Img_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles Lst_Img.ItemChecked
        UpdateActionButtonsState()
    End Sub

    Private Sub Lst_Img_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Lst_Img.SelectedIndexChanged
        UpdateActionButtonsState()
    End Sub

    ' Updated: Subroutine to update the enabled state of the action buttons
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

        ' If there are checked items:
        If checkedCount > 0 Then
            ' Case 1: Only enabled assets are checked
            If enabledCheckedCount > 0 AndAlso disabledCheckedCount = 0 Then
                Btn_DisableSelectedItems.Enabled = True
                Btn_EnableSelectedItems.Enabled = False
                ' Case 2: Only disabled assets are checked
            ElseIf disabledCheckedCount > 0 AndAlso enabledCheckedCount = 0 Then
                Btn_DisableSelectedItems.Enabled = False
                Btn_EnableSelectedItems.Enabled = True
                ' Case 3: Mix of enabled and disabled assets checked, or no items checked (though covered by the first If)
            Else ' (enabledCheckedCount > 0 AndAlso disabledCheckedCount > 0)
                Btn_DisableSelectedItems.Enabled = False
                Btn_EnableSelectedItems.Enabled = False
            End If
        Else
            ' No items checked, disable both buttons
            Btn_DisableSelectedItems.Enabled = False
            Btn_EnableSelectedItems.Enabled = False
        End If
    End Sub

End Class