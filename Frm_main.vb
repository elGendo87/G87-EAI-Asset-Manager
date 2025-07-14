Imports System.IO

Public Class Frm_Main

    Private Sub Frm_Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormFormat()
        DisableCmbandBtns() ' Disable controls on startup
    End Sub

    Private Sub Msm_Close_Click(sender As Object, e As EventArgs) Handles Msm_Close.Click
        End ' Closes the application
    End Sub

    Private Sub Msm_ScanMods_Click(sender As Object, e As EventArgs) Handles Msm_ScanMods.Click
        ' Reset filter state before performing a full scan
        IsFilterDisabledOnlyActive = False
        Msm_FiltersDisabledOnly.Checked = False

        Btn_Scan.PerformClick() ' Simulate click on scan button for full scan
    End Sub

    Private Sub Btn_Scan_Click(sender As Object, e As EventArgs) Handles Btn_Scan.Click
        ' Reset filter state before a full scan
        IsFilterDisabledOnlyActive = False
        Msm_FiltersDisabledOnly.Checked = False

        ScanFolders() ' Call the function for full folder scan
    End Sub

    Private Sub Cmb_Mods_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_Mods.SelectedIndexChanged
        If Cmb_Mods.SelectedItem IsNot Nothing Then
            Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem
            Dim modRootPath As String = selectedMod.Value.ToString()

            LoadAssetTypes(modRootPath)
        End If
    End Sub

    ' Renamed from Cmb_Assets_SelectedIndexChanged
    Private Sub Cmb_AssetType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_AssetType.SelectedIndexChanged
        If Cmb_AssetType.SelectedItem IsNot Nothing AndAlso Cmb_Mods.SelectedItem IsNot Nothing Then
            Dim selectedAssetType As ComboBoxItem = Cmb_AssetType.SelectedItem ' Renamed
            Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem

            Dim modRootPath As String = selectedMod.Value.ToString()
            Dim assetSubFolder As String = selectedAssetType.Value.ToString()

            Dim fullAssetPath As String = Path.Combine(modRootPath, assetSubFolder)

            LoadCategories(fullAssetPath)
        End If
    End Sub

    Private Sub Cmb_Cat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_Cat.SelectedIndexChanged
        If Cmb_Cat.SelectedItem IsNot Nothing AndAlso Cmb_AssetType.SelectedItem IsNot Nothing AndAlso Cmb_Mods.SelectedItem IsNot Nothing Then ' Renamed Cmb_Assets
            Dim selectedCategory As ComboBoxItem = Cmb_Cat.SelectedItem
            Dim selectedAssetType As ComboBoxItem = Cmb_AssetType.SelectedItem ' Renamed
            Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem

            Dim modRootPath As String = selectedMod.Value.ToString()
            Dim assetSubFolder As String = selectedAssetType.Value.ToString()
            Dim categoryFolder As String = selectedCategory.Value.ToString()

            Dim fullCategoryPath As String = Path.Combine(modRootPath, assetSubFolder, categoryFolder)

            LoadAssetsIntoListView(fullCategoryPath)
        End If
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
        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim assetFullPath As String = selectedItem.Tag.ToString()

        DisableAsset(assetFullPath)
        ' If filter is active, disabled items are already shown. Just refresh current view.
        RecargarVistaActual()
    End Sub

    Private Sub MenuItem_EnableAsset_Click(sender As Object, e As EventArgs) Handles MenuItem_EnableAsset.Click
        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim assetFullPath As String = selectedItem.Tag.ToString()

        EnableAsset(assetFullPath)
        ' If filter is active, enabling an item means it should disappear from the list.
        ' Re-scanning with filter active is needed to update all ComboBoxes.
        If IsFilterDisabledOnlyActive Then
            ScanFilteredFolders() ' Call the specialized filtered scan
        Else
            RecargarVistaActual() ' Only refresh current view if no filter is active
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

    ' Helper function to refresh the current ListView
    Private Sub RecargarVistaActual()
        If Cmb_Cat.SelectedItem IsNot Nothing AndAlso Cmb_AssetType.SelectedItem IsNot Nothing AndAlso Cmb_Mods.SelectedItem IsNot Nothing Then ' Renamed Cmb_Assets
            Dim selectedCategory As ComboBoxItem = Cmb_Cat.SelectedItem
            Dim selectedAssetType As ComboBoxItem = Cmb_AssetType.SelectedItem ' Renamed
            Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem
            Dim modRootPath As String = selectedMod.Value.ToString()
            Dim assetSubFolder As String = selectedAssetType.Value.ToString()
            Dim categoryFolder As String = selectedCategory.Value.ToString()

            Dim fullCategoryPath As String = Path.Combine(modRootPath, assetSubFolder, categoryFolder)

            LoadAssetsIntoListView(fullCategoryPath) ' Reload view with updated names and colors
        End If
    End Sub

    Private Sub Btn_DisableSelectedItems_Click(sender As Object, e As EventArgs) Handles Btn_DisableSelectedItems.Click
        If Lst_Img.CheckedItems.Count = 0 Then
            MessageBox.Show("Please select at least one item to disable.", "Nothing Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If MessageBox.Show("Are you sure you want to disable the " & Lst_Img.CheckedItems.Count & " selected items?", "Confirm Disable", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            For Each item As ListViewItem In Lst_Img.CheckedItems
                Dim assetFullPath As String = item.Tag.ToString()
                DisableAsset(assetFullPath)
            Next

            RecargarVistaActual()
        End If
    End Sub

    Private Sub Btn_EnableSelectedItems_Click(sender As Object, e As EventArgs) Handles Btn_EnableSelectedItems.Click
        If Lst_Img.CheckedItems.Count = 0 Then
            MessageBox.Show("Please select at least one item to enable.", "Nothing Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If MessageBox.Show("Are you sure you want to enable the " & Lst_Img.CheckedItems.Count & " selected items?", "Confirm Enable", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            For Each item As ListViewItem In Lst_Img.CheckedItems
                Dim assetFullPath As String = item.Tag.ToString()
                EnableAsset(assetFullPath)
            Next

            ' If filter is active, enabling an item means it should disappear from the list.
            ' Re-scanning with filter active is needed to update all ComboBoxes.
            If IsFilterDisabledOnlyActive Then
                ScanFilteredFolders() ' Call the specialized filtered scan
            Else
                RecargarVistaActual() ' Only refresh current view if no filter is active
            End If
        End If
    End Sub

    ' Handle for "Show only disabled assets" menu item
    Private Sub Msm_FiltersDisabledOnly_Click(sender As Object, e As EventArgs) Handles Msm_FiltersDisabledOnly.Click
        IsFilterDisabledOnlyActive = Not IsFilterDisabledOnlyActive ' Toggle filter state
        Msm_FiltersDisabledOnly.Checked = IsFilterDisabledOnlyActive ' Update menu checkbox

        If IsFilterDisabledOnlyActive Then
            ScanFilteredFolders() ' Perform a specialized scan for only disabled assets
        Else
            ScanFolders() ' Perform a full scan to show all assets
        End If
    End Sub

End Class