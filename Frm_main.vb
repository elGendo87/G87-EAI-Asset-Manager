Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.FileIO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class Frm_Main

    Public _originalInfoBarText As String = String.Empty ' Variable to store the original text of Txt_InfoBar
    Private _hoveredItem As ListViewItem = Nothing ' Variable to store the currently hovered item in the ListView

    ' In Frm_Main_Load, add the checkbox event handler
    Private Sub Frm_Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Debug.Flush()

        LoadingStart("Frm_Main_Load Started.")

        FormFormat()
        DisableCmbandBtns()
        'UpdateActionButtonsState() ' Initialize button state on load

        LoadImagesFromDataFiles() 'Load images from data files on load
        ApplyLightThemeIcons() ' Apply light theme icons on load

        Txt_InfoBar.Text = "" ' Clear info bar on load

        ' Load the checkbox setting on startup
        ModScanner.LoadSettings()


        ' If the checkbox was not checked, perform the normal scan
        If Not Chk_ShowEAI.Checked Then
            isFirstLoad = True ' Indicate that the UI is still loading
            DoModsScanning()
            isFirstLoad = False ' Indicate that the UI has finished loading

            'Force the first mod to be selected
            'v1.4.6.3
            If Cmb_Mods.SelectedItem IsNot Nothing Then
                Cmb_Mods.SelectedIndex = -1 ' Select the first mod by default
                Cmb_Mods.SelectedIndex = 0 ' Select the first mod by default
            Else
                MessageBox.Show("No mods found. Please ensure you have at least one EAI mod installed before running the program again.", "No Mods Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End
            End If

            'v1.4.6.3
            If Cmb_AssetType.SelectedItem Is Nothing Or Cmb_AssetType.Text = "" Then
                MessageBox.Show("No assets found or folders are empty. Please ensure you have at least one EAI mod or EAI custom asset installed before running the program again.", "No Assets Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                DisableCmbandBtns() ' Disable buttons if no asset types are found
                End
            End If

            'v1.4.6.3
            If Cmb_Cat.SelectedItem Is Nothing Or Cmb_Cat.Text = "" Then
                MessageBox.Show("No assets found or folders are empty. Please ensure you have at least one EAI mod or EAI custom asset installed before running the program again.", "No Assets Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                DisableCmbandBtns() ' Disable buttons if no asset types are found
                End
            End If

        Else
                ' If the checkbox was checked, simulate a change to the activated state,
                ' so the function that activates that checkbox is triggered automatically
                isFirstLoad = True ' Indicate that the UI is still loading
            DoModsScanning()
            isFirstLoad = False ' Indicate that the UI has finished loading

            Dim temp As New EventArgs()
            Chk_ShowEAI_CheckedChanged(sender, temp)
        End If

        LoadingStop("Frm_Main_Load Stopped.")
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

    ' Save settings on form closing
    Private Sub Frm_Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Save the settings on application close
        ModScanner.SaveSettings()
    End Sub

    Private Sub Msm_Close_Click(sender As Object, e As EventArgs) Handles Msm_Close.Click
        End
    End Sub

    ' Event handler for Msm_About (About menu item)
    Private Sub Msm_About_Click(sender As Object, e As EventArgs) Handles Msm_About.Click
        Using aboutForm As New Frm_About()
            aboutForm.ShowDialog()
        End Using
    End Sub

    Private Sub Msm_ScanMods_Click(sender As Object, e As EventArgs)
        ' removed duplicated calls
        DoModsScanning()

    End Sub

    Private Sub DoModsScanning()

        LoadingStart("DoModsScanning started...")

        IsFilterDisabledOnlyActive = False
        Msm_FiltersDisabledOnly.Checked = False

        currentModPath = ""
        currentAssetTypePath = ""
        currentCategoryPath = ""
        'currentSelectedAssetTag = ""


        ScanFolders()

        'UpdateActionButtonsState() ' Update button state after a full scan
        UpdateInfoBar() ' Update info bar after a full scan

        LoadingStop("DoModsScanning Stopped.")

    End Sub

    ' Handle Cmb Mods
    Private Sub Cmb_Mods_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_Mods.SelectedIndexChanged

        If isRestoringSelections Then Return

        If Cmb_Mods.SelectedItem IsNot Nothing Then
            Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem
            Dim modRootPath As String = selectedMod.Value.ToString()
            ModScanner.currentModPath = modRootPath

            ' Clear asset type and category path before loading new ones
            ModScanner.currentAssetTypePath = ""
            ModScanner.currentCategoryPath = ""
            'ModScanner.currentSelectedAssetTag = "" ' Also clear asset selection

            LoadAssetTypes(modRootPath)


            ModScanner.RestoreSelections("Cmb_Mods_SelectedIndexChanged") ' Explicitly call RestoreSelections here to ensure cascade

        End If
        'UpdateActionButtonsState() ' Update button state after combo box change (which affects ListView)
        UpdateInfoBar() ' Update info bar when mod selection changes


    End Sub

    'Handle Cmb Asset Type. modified with Claude
    Private Sub Cmb_AssetType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_AssetType.SelectedIndexChanged

        If isRestoringSelections Then Return


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
                    'ModScanner.currentSelectedAssetTag = ""
                    LoadCategories(fullAssetPath)

                    ModScanner.RestoreSelections("Cmb_AssetType_SelectedIndexChanged")

                End If
            End If
        End If
        'UpdateActionButtonsState()
    End Sub

    ' Handle Cmb Cat. modified with Claude
    Private Sub Cmb_Cat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_Cat.SelectedIndexChanged

        If isRestoringSelections Then Return


        If Cmb_Cat.SelectedItem IsNot Nothing AndAlso Cmb_AssetType.SelectedItem IsNot Nothing Then
            Dim selectedCategory As ComboBoxItem = Cmb_Cat.SelectedItem
            Dim selectedAssetType As ComboBoxItem = Cmb_AssetType.SelectedItem

            If Chk_ShowEAI.Checked Then

                ' EAI Mode: Load assets from all mods
                If Not isFirstLoad Then 'prevent loading in refreshing
                    LoadAssetsFromAllMods(selectedAssetType.Value.ToString(), selectedCategory.Value.ToString())
                End If

            Else
                    ' Normal Mode: Existing behavior
                    If Cmb_Mods.SelectedItem IsNot Nothing Then
                    Dim selectedMod As ComboBoxItem = Cmb_Mods.SelectedItem
                    Dim modRootPath As String = selectedMod.Value.ToString()
                    Dim fullCategoryPath As String = Path.Combine(modRootPath, selectedAssetType.Value.ToString(), selectedCategory.Value.ToString())
                    Debug.WriteLine("Selected AssetType:" & selectedAssetType.Value.ToString()) ' Debugging line
                    ModScanner.currentCategoryPath = selectedCategory.Value.ToString()

                    ' Only trigger loading if not the first load
                    If Not isFirstLoad Then
                        LoadAssetsIntoListView(fullCategoryPath)
                    End If

                End If
            End If
        End If
        'UpdateActionButtonsState()
    End Sub

    '### CONTEXT MENU HANDLER FOR ASSETS ###
    Private Sub Ctx_Asset_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Ctx_Asset.Opening
        ' Hide all menu items and the bulk operations parent item by default
        For Each item As ToolStripItem In Ctx_Asset.Items
            item.Visible = False
        Next
        Ctx_BulkOperations.Visible = False

        ' Check the number of selected items in the ListView
        If Lst_Img.SelectedItems.Count = 1 Then
            ' --- SINGLE SELECTION LOGIC ---
            Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
            Dim assetPath As String = selectedItem.Tag.ToString()
            Dim currentFolderName As String = Path.GetFileName(assetPath)

            ' Check if the asset is from the Extra Assets Importer folder
            Dim isEAIAsset As Boolean = assetPath.StartsWith(CustomFiles.GetEAICustomFolderPath(), StringComparison.OrdinalIgnoreCase)

            If currentFolderName.StartsWith("."c) Then
                ' Options for a disabled asset
                MenuItem_EnableAsset.Visible = True
                MenuItem_AssetProperties.Visible = True
            Else
                ' Options for an enabled asset
                MenuItem_DisableAsset.Visible = True
                MenuItem_CreateLocalCopy.Visible = True
                MenuItem_DeleteLocalAsset.Visible = isEAIAsset
                MenuItem_RenameLocalAsset.Visible = isEAIAsset
                MenuItem_EditLocalAsset.Visible = isEAIAsset
                MenuItem_AssetProperties.Visible = True
                Ctx_ChangeCat.Visible = isEAIAsset
            End If

            Ctx_SepEnDis.Visible = True ' Separator for enable/disable options
            MenuItem_OpenLocation.Visible = True ' Open location option action
            Ctx_SepOpenLocal.Visible = True ' Separator for open location option

        ElseIf Lst_Img.SelectedItems.Count > 1 Then
            ' --- MULTIPLE SELECTION LOGIC ---
            Dim allEnabled As Boolean = True
            Dim allDisabled As Boolean = True
            Dim allInEAI As Boolean = True

            For Each item As ListViewItem In Lst_Img.SelectedItems
                Dim assetPath As String = item.Tag.ToString()
                Dim currentFolderName As String = Path.GetFileName(assetPath)

                If currentFolderName.StartsWith("."c) Then
                    allEnabled = False
                Else
                    allDisabled = False
                End If

                If Not assetPath.Contains("\ExtraAssetsImporter\") Then
                    allInEAI = False
                End If

                ' If we find a mixed state for both conditions, we can exit the loop early
                If (Not allEnabled And Not allDisabled) And (Not allInEAI) Then
                    Exit For
                End If
            Next

            ' If the selection is not mixed (all enabled or all disabled), show the bulk operations menu
            If allEnabled Or allDisabled Then
                Ctx_BulkOperations.Visible = True

                ' Disable all bulk operation children by default
                For Each item As ToolStripItem In Ctx_BulkOperations.DropDownItems
                    item.Visible = False
                Next

                ' Show specific bulk operations based on the selection state
                If allEnabled Then
                    Ctx_Bulk_DisableAssets.Visible = True
                ElseIf allDisabled Then
                    Ctx_Bulk_EnableAssets.Visible = True
                End If

                ' Show these options ONLY if ALL selected assets are from the EAI folder
                If allInEAI Then
                    Ctx_Bulk_SepEnDis.Visible = True ' Separator for bulk enable/disable options
                    Ctx_Bulk_DeleteAssets.Visible = True
                    Ctx_Bulk_SepActions.Visible = True ' Separator for bulk actions
                    Ctx_Bulk_SetDrawOrder.Visible = True
                    Ctx_Bulk_SetDLM.Visible = True
                    Ctx_Bulk_SetUiPriority.Visible = True
                End If
            End If
        Else ' No items selected
            e.Cancel = True ' Cancel the menu display
        End If
    End Sub

    ' --- HANDLERS FOR BULK OPERATIONS ---
    ' Bulk Disable Assets
    Private Sub Ctx_Bulk_DisableAssets_Click(sender As Object, e As EventArgs) Handles Ctx_Bulk_DisableAssets.Click
        ' Call the logic from the existing button
        Btn_DisableSelectedItems_Click(sender, e)
    End Sub
    ' Bulk Enable Assets
    Private Sub Ctx_Bulk_EnableAssets_Click(sender As Object, e As EventArgs) Handles Ctx_Bulk_EnableAssets.Click
        ' Call the logic from the existing button
        Btn_EnableSelectedItems_Click(sender, e)
    End Sub

    ' Bulk Delete Assets
    Private Sub Ctx_Bulk_DeleteAssets_Click(sender As Object, e As EventArgs) Handles Ctx_Bulk_DeleteAssets.Click

        ' A dedicated bulk deletion function is better.
        If MsgBox("Are you sure you want to delete the selected items?" & vbCrLf & vbCrLf & "This action cannot be undone!", MsgBoxStyle.YesNo, "Confirm Deletion") = MsgBoxResult.No Then
            Return
        End If

        For Each item As ListViewItem In Lst_Img.SelectedItems
            Dim assetPath As String = item.Tag.ToString()
            Try
                If Directory.Exists(assetPath) Then
                    FileSystem.DeleteDirectory(assetPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin)
                    ' Remove the item from the list view after deletion
                    Lst_Img.Items.Remove(item)
                End If
            Catch ex As Exception
                MessageBox.Show($"Error deleting item at path: {assetPath}. Details: {ex.Message}", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next

    End Sub

    'Bulk Set Draw Order
    Private Sub Ctx_Bulk_SetDrawOrder_Click(sender As Object, e As EventArgs) Handles Ctx_Bulk_SetDrawOrder.Click
        Dim drawOrderValue As Integer
        Dim inputIsValid As Boolean = False
        Dim inputResult As String

        Do
            inputResult = InputBox("Enter the DrawOrder value (an integer between -170 and 200)." & vbCrLf & vbCrLf & "Empty value will CANCEL the operation.", "Set DrawOrder", "")
            If inputResult = "" Then ' User canceled
                Return
            End If

            If Integer.TryParse(inputResult, drawOrderValue) AndAlso drawOrderValue >= -170 AndAlso drawOrderValue <= 200 Then
                inputIsValid = True
            Else
                MessageBox.Show("Invalid input. Please enter an integer between -170 and 200.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Loop While Not inputIsValid

        ' FIX: Create a robust copy of the selected items to prevent the "Collection was modified" error.
        Dim selectedItems As List(Of ListViewItem) = Lst_Img.SelectedItems.Cast(Of ListViewItem)().ToList()

        For Each item As ListViewItem In selectedItems
            Dim assetPath As String = item.Tag.ToString()
            UpdateJsonFile(assetPath, "_DrawOrder", drawOrderValue)
        Next
    End Sub

    'Bulk Set Decal Layer Mask (DLM)
    Private Sub Ctx_Bulk_SetDLM_Click(sender As Object, e As EventArgs) Handles Ctx_Bulk_SetDLM.Click
        Dim dlmValue As Integer
        Dim inputIsValid As Boolean = False
        Dim inputResult As String
        Dim validDLMValues As New List(Of Integer) From {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63}

        Do
            inputResult = InputBox("Enter the Decal Layer Mask value. To select multiple values you need to sum them:" & vbCrLf & "1 = Ground " & vbCrLf & "2 = Roads" & vbCrLf & "4 = Buildings" & vbCrLf & "8 = Vehicles" & vbCrLf & "16 = Creatures" & vbCrLf & "32 = Props" & vbCrLf & vbCrLf & "Empty value will CANCEL the operation.", "Set Decal Layer Mask", "")
            If inputResult = "" Then ' User canceled
                Return
            End If

            If Integer.TryParse(inputResult, dlmValue) AndAlso validDLMValues.Contains(dlmValue) Then
                Dim selectedCategories As String = GetDLMCategories(dlmValue)
                If MsgBox($"Are these selected categories correct?{Environment.NewLine}{selectedCategories}", MsgBoxStyle.YesNo, "Confirm Categories") = MsgBoxResult.Yes Then
                    inputIsValid = True
                End If
            Else
                MessageBox.Show("Invalid input. Please enter a valid integer from the list provided.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Loop While Not inputIsValid

        ' FIX: Create a robust copy of the selected items to prevent the "Collection was modified" error.
        Dim selectedItems As List(Of ListViewItem) = Lst_Img.SelectedItems.Cast(Of ListViewItem)().ToList()

        For Each item As ListViewItem In selectedItems
            Dim assetPath As String = item.Tag.ToString()
            UpdateJsonFile(assetPath, "colossal_DecalLayerMask", dlmValue)
        Next
    End Sub

    'Bulk Set UiPriority
    Private Sub Ctx_Bulk_SetUiPriority_Click(sender As Object, e As EventArgs) Handles Ctx_Bulk_SetUiPriority.Click
        Dim startingUiPriority As Integer
        Dim inputIsValid As Boolean = False
        Dim inputResult As String

        Do
            inputResult = InputBox("Enter the STARTING UiPriority value." & vbCrLf & vbCrLf & "The following numbers will be calculated automatically in asset order." & vbCrLf & vbCrLf & "Empty = 0", "Set UiPriority", "")
            'If inputResult = "" Then ' User canceled
            '    Return
            'End If

            If Integer.TryParse(inputResult, startingUiPriority) AndAlso startingUiPriority >= 0 AndAlso startingUiPriority <= 99999999 Then
                inputIsValid = True
            Else
                MessageBox.Show("Invalid input. Please enter an integer between 0 and 99999999.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Loop While Not inputIsValid

        ' Create a robust copy of the selected items to prevent the "Collection was modified" error.
        ' Sort the copied list by name
        Dim sortedItems As List(Of ListViewItem) = Lst_Img.SelectedItems.Cast(Of ListViewItem)().OrderBy(Function(item) Path.GetFileName(item.Tag.ToString())).ToList()

        Dim currentUiPriority As Integer = startingUiPriority
        For Each item As ListViewItem In sortedItems
            Dim assetPath As String = item.Tag.ToString()

            UpdateJsonFile(assetPath, "UiPriority", currentUiPriority, "netlane.json") ' Handle netlane.json first
            UpdateJsonFile(assetPath, "UiPriority", currentUiPriority) ' Handle decal.json or surface.json

            currentUiPriority += 1
        Next
    End Sub

    'Bulk update function for JSON files
    Private Sub UpdateJsonFile(assetPath As String, key As String, value As Object, Optional specificFileName As String = "")
        Dim filePath As String
        Dim fileNames As String()

        If Not String.IsNullOrEmpty(specificFileName) Then
            fileNames = New String() {specificFileName}
        Else
            fileNames = New String() {"decal.json", "surface.json"}
        End If

        For Each fileName As String In fileNames
            filePath = Path.Combine(assetPath, fileName)

            ' Special handling for netlane.json creation
            If fileName = "netlane.json" AndAlso Not File.Exists(filePath) Then
                If key = "UiPriority" Then
                    Try
                        ' Use JToken.FromObject to correctly serialize the value
                        Dim jsonObject As JObject = JObject.FromObject(New With {.UiPriority = JToken.FromObject(value)})
                        File.WriteAllText(filePath, jsonObject.ToString(Newtonsoft.Json.Formatting.Indented))
                        Return
                    Catch ex As Exception
                        MessageBox.Show($"Error creating netlane.json for {Path.GetFileName(assetPath)}: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End Try
                End If
            End If

            If File.Exists(filePath) Then
                Try
                    Dim jsonContent As String = File.ReadAllText(filePath)
                    Dim jsonObject As JObject = JObject.Parse(jsonContent)

                    If key = "UiPriority" Then
                        ' Check if UiPriority exists at the root level
                        If jsonObject.ContainsKey(key) Then
                            ' FIX: Use JToken.FromObject to ensure correct type conversion
                            jsonObject(key) = JToken.FromObject(value)
                        Else
                            ' Otherwise, assume "Float" exists and modify/add it there
                            Dim floatObject As JObject = jsonObject("Float")
                            ' FIX: Use JToken.FromObject to ensure correct type conversion
                            floatObject(key) = JToken.FromObject(value)
                        End If
                    Else
                        ' For other keys (_DrawOrder, colossal_DecalLayerMask), assume "Float" exists.
                        Dim floatObject As JObject = jsonObject("Float")

                        ' FIX: Use JToken.FromObject to ensure correct type conversion
                        floatObject(key) = JToken.FromObject(value)
                    End If

                    File.WriteAllText(filePath, jsonObject.ToString(Formatting.Indented))

                Catch ex As Exception
                    MessageBox.Show($"Error updating {fileName} for {Path.GetFileName(assetPath)}: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

                If String.IsNullOrEmpty(specificFileName) Then
                    Exit For
                End If
            End If
        Next
    End Sub

    'DLM calculator
    Private Function GetDLMCategories(dlmValue As Integer) As String
        Dim result As String = ""
        Dim remainingValue As Integer = dlmValue
        Dim categories As New Dictionary(Of Integer, String) From {
            {32, "Props"},
            {16, "Creatures"},
            {8, "Vehicles"},
            {4, "Buildings"},
            {2, "Roads"},
            {1, "Ground"}
        }

        For Each category As KeyValuePair(Of Integer, String) In categories
            If remainingValue >= category.Key Then
                If remainingValue >= category.Key Then
                    result += category.Value + Environment.NewLine
                    remainingValue -= category.Key
                End If
            End If
        Next
        Return result
    End Function

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
        'MsgBox(assetType) 'Debugging line
        Dim categoryName As String = DirectCast(Cmb_Cat.SelectedItem, ComboBoxItem).Value.ToString()

        ' Call the function in the new CustomFiles module
        Dim copySuccessful As Boolean = CustomFiles.CreateLocalCopy(sourceAssetPath, assetType, categoryName)

        If copySuccessful Then

            If Chk_ShowEAI.Checked Then
                ' EAI Mode: Refresh cache and current view
                RefreshModsCache()
                RefreshCurrentView()

            Else

                'Save the EAI mod path as the current mod path
                Dim itemToFind As String = "Extra Assets Importer (Local Assets)"
                Dim foundItem As ComboBoxItem = Nothing

                For Each item As ComboBoxItem In Cmb_Mods.Items
                    If item.Text.ToString().Equals(itemToFind, StringComparison.OrdinalIgnoreCase) Then
                        foundItem = item
                        Exit For
                    End If
                Next

                ' If the item was found, store its value.
                Dim LocalModPath As String = foundItem.Value.ToString()
                Dim LocalAssetTypePath As String = assetType
                Dim LocalCategoryPath As String = categoryName

                If IsFilterDisabledOnlyActive Then
                    ScanFilteredFolders()
                Else
                    ModScanner.currentModPath = ""
                    ModScanner.currentAssetTypePath = ""
                    ModScanner.currentCategoryPath = ""

                    ' Prevent the Cmb_Mods_SelectedIndexChanged event from triggering while we update the combobox
                    'RemoveHandler Cmb_Mods.SelectedIndexChanged, AddressOf Cmb_Mods_SelectedIndexChanged
                    'RemoveHandler Cmb_AssetType.SelectedIndexChanged, AddressOf Cmb_AssetType_SelectedIndexChanged
                    'RemoveHandler Cmb_Cat.SelectedIndexChanged, AddressOf Cmb_Cat_SelectedIndexChanged

                    isFirstLoad = True

                    ScanFolders()

                    Cmb_Mods.SelectedIndex = -1 ' Reset the selection

                    ' Restore the selection to the Extra Assets Importer mod
                    If Not String.IsNullOrEmpty(LocalModPath) Then
                        For i As Integer = 0 To Cmb_Mods.Items.Count - 1
                            Dim item As ComboBoxItem = DirectCast(Cmb_Mods.Items(i), ComboBoxItem)
                            If item.Value.ToString().Equals(LocalModPath, StringComparison.OrdinalIgnoreCase) Then
                                Cmb_Mods.SelectedIndex = i
                                Exit For
                            End If
                        Next
                    End If

                    Cmb_AssetType.SelectedIndex = -1 ' Reset the selection

                    ' Seleccionar el Cmb_AssetType
                    If Not String.IsNullOrEmpty(LocalAssetTypePath) Then
                        For i As Integer = 0 To Cmb_AssetType.Items.Count - 1
                            Dim item As ComboBoxItem = DirectCast(Cmb_AssetType.Items(i), ComboBoxItem)
                            If item.Value.ToString().Equals(LocalAssetTypePath, StringComparison.OrdinalIgnoreCase) Then
                                Cmb_AssetType.SelectedIndex = i
                                Exit For
                            End If
                        Next
                    End If

                    Cmb_Cat.SelectedIndex = -1 ' Reset the selection
                    isFirstLoad = False

                    ' Seleccionar el Cmb_Cat
                    If Not String.IsNullOrEmpty(LocalCategoryPath) Then
                        For i As Integer = 0 To Cmb_Cat.Items.Count - 1
                            Dim item As ComboBoxItem = DirectCast(Cmb_Cat.Items(i), ComboBoxItem)
                            If item.Value.ToString().Equals(LocalCategoryPath, StringComparison.OrdinalIgnoreCase) Then
                                Cmb_Cat.SelectedIndex = i
                                Exit For
                            End If
                        Next
                    End If

                End If

            End If

            'UpdateActionButtonsState

            ''Autoselect the newly created asset
            'If Chk_ShowEAI.Checked = False Then
            '    Dim eaiItem As ComboBoxItem = Cmb_Mods.Items.Cast(Of ComboBoxItem)().FirstOrDefault(Function(item) item.Text = "Extra Assets Importer (Local Assets)")
            '    If eaiItem IsNot Nothing Then
            '        Cmb_Mods.SelectedItem = eaiItem
            '    End If
            '    Cmb_AssetType.SelectedItem = Cmb_AssetType.Items.Cast(Of ComboBoxItem)().FirstOrDefault(Function(item) item.Value.ToString() = assetType)
            '    Cmb_Cat.SelectedItem = Cmb_Cat.Items.Cast(Of ComboBoxItem)().FirstOrDefault(Function(item) item.Value.ToString() = categoryName)
            'End If

            UpdateInfoBar()

        End If

    End Sub

    ' Event handler for "Delete Local Asset" context menu item
    Private Sub MenuItem_DeleteLocalAsset_Click(sender As Object, e As EventArgs) Handles MenuItem_DeleteLocalAsset.Click
        If Lst_Img.SelectedItems.Count = 0 Then Return

        Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
        Dim assetFullPath As String = selectedItem.Tag.ToString()

        ' Call the function in the CustomFiles module to handle deletion
        Dim deletionSuccessful As Boolean = CustomFiles.DeleteLocalAsset(assetFullPath)

        If deletionSuccessful Then
            ' OLD MORE COMPLICATED CODE
            ''ModScanner.RescanAndRefresh() ' Use unified rescan function
            'Dim LocalAssetTypePath As String = DirectCast(Cmb_AssetType.SelectedItem, ComboBoxItem).Value.ToString()
            'Dim LocalCategoryPath As String = DirectCast(Cmb_Cat.SelectedItem, ComboBoxItem).Value.ToString()
            'Dim LocalSelectedModPath As String = DirectCast(Cmb_Mods.SelectedItem, ComboBoxItem).Value.ToString()

            'If Chk_ShowEAI.Checked Then
            '    ' EAI Mode: Refresh cache and current view
            '    RefreshModsCache()
            '    RefreshCurrentView()
            'Else
            '    ' Normal Mode: Standard rescan

            '    ModScanner.currentModPath = ""
            '    ModScanner.currentAssetTypePath = ""
            '    ModScanner.currentCategoryPath = ""

            '    'StoreCurrentSelections()

            '    If IsFilterDisabledOnlyActive Then
            '        ScanFilteredFolders()
            '    Else
            '        isFirstLoad = True

            '        ScanFolders()

            '        Cmb_Mods.SelectedIndex = -1 ' Reset the selection

            '        ' Restore the selection to the Extra Assets Importer mod
            '        If Not String.IsNullOrEmpty(LocalSelectedModPath) Then
            '            For i As Integer = 0 To Cmb_Mods.Items.Count - 1
            '                Dim item As ComboBoxItem = DirectCast(Cmb_Mods.Items(i), ComboBoxItem)
            '                If item.Value.ToString().Equals(LocalSelectedModPath, StringComparison.OrdinalIgnoreCase) Then
            '                    Cmb_Mods.SelectedIndex = i
            '                    Exit For
            '                End If
            '            Next
            '        End If

            '        Cmb_AssetType.SelectedIndex = -1 ' Reset the selection

            '        ' Seleccionar el Cmb_AssetType
            '        If Not String.IsNullOrEmpty(LocalAssetTypePath) Then
            '            For i As Integer = 0 To Cmb_AssetType.Items.Count - 1
            '                Dim item As ComboBoxItem = DirectCast(Cmb_AssetType.Items(i), ComboBoxItem)
            '                If item.Value.ToString().Equals(LocalAssetTypePath, StringComparison.OrdinalIgnoreCase) Then
            '                    Cmb_AssetType.SelectedIndex = i
            '                    Exit For
            '                End If
            '            Next
            '        End If

            '        Cmb_Cat.SelectedIndex = -1 ' Reset the selection
            '        isFirstLoad = False

            '        ' Seleccionar el Cmb_Cat
            '        If Not String.IsNullOrEmpty(LocalCategoryPath) Then
            '            For i As Integer = 0 To Cmb_Cat.Items.Count - 1
            '                Dim item As ComboBoxItem = DirectCast(Cmb_Cat.Items(i), ComboBoxItem)
            '                If item.Value.ToString().Equals(LocalCategoryPath, StringComparison.OrdinalIgnoreCase) Then
            '                    Cmb_Cat.SelectedIndex = i
            '                    Exit For
            '                End If
            '            Next
            '        End If

            '    End If

            ModScanner.RefreshCurrentView() 'will only refresh the current view, not the entire form


            'UpdateActionButtonsState()
            'End If

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
            'ModScanner.RescanAndRefresh() ' Use unified rescan function
            ModScanner.RefreshCurrentView() ' Use unified refresh function
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
        'ModScanner.RescanAndRefresh() ' Use unified rescan function
        ModScanner.RefreshCurrentView() ' Use unified refresh function
        'UpdateActionButtonsState()
        UpdateInfoBar()

    End Sub

    ' Context menu item for changing the category of a local asset
    Private Sub Ctx_ChangeCat_Click(sender As Object, e As EventArgs) Handles Ctx_ChangeCat.Click
        If Lst_Img.SelectedItems.Count = 0 Then Return
        CustomFiles.ChangingCatAssetPath = Lst_Img.SelectedItems(0).Tag.ToString()
        CustomFiles.ChangingCatAssetName = Lst_Img.SelectedItems(0).Text

        ' Trigger a category refresh if a change was made
        Dim CurrentAssetTypeSelected As String = If(Cmb_AssetType.SelectedItem IsNot Nothing, DirectCast(Cmb_AssetType.SelectedItem, ComboBoxItem).Value.ToString(), String.Empty)
        Dim CurrentCatSelected As String = If(Cmb_Cat.SelectedItem IsNot Nothing, DirectCast(Cmb_Cat.SelectedItem, ComboBoxItem).Value.ToString(), String.Empty)
        Dim LocalCategory As String

        Using Frm_ChangeCat As New Frm_ChangeCat()
            Frm_ChangeCat.ShowDialog()
        End Using

        If CurrentCatSelected = ChangingCatAssetNew Then
            ' No change in category, so just return
            Return
        End If
        'MsgBox(CurrentCatSelected)

        ' If the category was changed, refresh the comboboxes and listview
        LocalCategory = CustomFiles.ChangingCatAssetNew

        isFirstLoad = True 'prevent loading Lst_Img in refreshing

        Cmb_AssetType.SelectedIndex = -1 ' Reset the selection

        ' trigger a refresh of the asset types to reflect any new categories
        If Not String.IsNullOrEmpty(CurrentAssetTypeSelected) Then
            For i As Integer = 0 To Cmb_AssetType.Items.Count - 1
                Dim item As ComboBoxItem = DirectCast(Cmb_AssetType.Items(i), ComboBoxItem)
                If item.Value.ToString().Equals(CurrentAssetTypeSelected, StringComparison.OrdinalIgnoreCase) Then
                    Cmb_AssetType.SelectedIndex = i
                    'Debug.WriteLine("Selected Asset Type: " & item.Text)
                    Exit For
                End If
            Next
        End If

        Cmb_Cat.SelectedIndex = -1 ' Reset the selection

        isFirstLoad = False 'allow loading Lst_Img in refreshing

        ' Select the new category in the combobox
        If Not String.IsNullOrEmpty(LocalCategory) Then
            For i As Integer = 0 To Cmb_Cat.Items.Count - 1
                Dim item As ComboBoxItem = DirectCast(Cmb_Cat.Items(i), ComboBoxItem)
                If item.Value.ToString().Equals(LocalCategory, StringComparison.OrdinalIgnoreCase) Then
                    Cmb_Cat.SelectedIndex = i
                    Debug.WriteLine("Selected Category: " & item.Text)
                    Exit For
                End If
            Next
        End If

    End Sub


    ' Event handler for "Disabling Asset" context menu item
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
    'Event handler for "Enable Asset" context menu item
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

    ' Event handler for "Disabling Assets" context menu item bulk operation
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
    ' Event handler for "Enabling Assets" context menu item bulk operation
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
    ' Event handler for "Open Location" context menu item
    Private Sub MenuItem_OpenLocation_Click(sender As Object, e As EventArgs) Handles MenuItem_OpenLocation.Click
        If Lst_Img.SelectedItems.Count = 1 Then
            Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)
            Dim assetFullPath As String = selectedItem.Tag.ToString()

            MsgBox("Opening location for: " & assetFullPath, MsgBoxStyle.Information, "Open Location")

            If System.IO.Directory.Exists(assetFullPath) Then
                Try
                    Process.Start("explorer.exe", """" & assetFullPath & """")
                Catch ex As Exception
                    MessageBox.Show("Could not open location: " & ex.Message, "Error Opening Folder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                MessageBox.Show("Item location does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    ' Enable/Disable buttons based on selection in Lst_Img
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
    ' Event handler for the "Filters Disabled Only" menu item
    Private Sub Msm_FiltersDisabledOnly_Click(sender As Object, e As EventArgs) Handles Msm_FiltersDisabledOnly.Click
        IsFilterDisabledOnlyActive = Not IsFilterDisabledOnlyActive
        Msm_FiltersDisabledOnly.Checked = IsFilterDisabledOnlyActive

        ModScanner.StoreCurrentSelections()
        If IsFilterDisabledOnlyActive Then
            ModScanner.ScanFilteredFolders()
        Else
            ModScanner.ScanFolders()
        End If

        If Chk_ShowEAI.Enabled Then

            Chk_ShowEAI.Checked = False
            Chk_ShowEAI.Enabled = False ' Disable EAI checkbox when filter is active
        Else
            If IsFilterDisabledOnlyActive = False Then

                Chk_ShowEAI.Enabled = True ' Enable EAI checkbox when filter is inactive

            End If
        End If

        'UpdateActionButtonsState()
        UpdateInfoBar() ' Update info bar after filter change

    End Sub

    ' Mouse move event handler for the ListView to show JSON info in the TextBox
    Private Sub Lst_Img_MouseMove(sender As Object, e As MouseEventArgs) Handles Lst_Img.MouseMove
        ' Get the item at the current mouse position
        Dim itemAtMousePosition As ListViewItem = Lst_Img.GetItemAt(e.X, e.Y)

        ' If the mouse is over an item and it's not the same item as before
        If itemAtMousePosition IsNot Nothing AndAlso itemAtMousePosition IsNot _hoveredItem Then
            _hoveredItem = itemAtMousePosition

            ' Save the original text of the TextBox if it has not been saved yet
            If _originalInfoBarText = String.Empty Then
                _originalInfoBarText = Txt_InfoBar.Text
            End If

            Dim assetFolderPath As String = CStr(_hoveredItem.Tag)
            Dim jsonFilePath As String = String.Empty

            ' Check for "decal.json" first
            Dim decalFilePath As String = Path.Combine(assetFolderPath, "decal.json")
            If File.Exists(decalFilePath) Then
                jsonFilePath = decalFilePath
            Else
                ' If not found, check for "surface.json"
                Dim surfaceFilePath As String = Path.Combine(assetFolderPath, "surface.json")
                If File.Exists(surfaceFilePath) Then
                    jsonFilePath = surfaceFilePath
                End If
            End If

            If Not String.IsNullOrEmpty(jsonFilePath) Then
                Try
                    Dim jsonContent As String = File.ReadAllText(jsonFilePath)
                    Dim jObject As JObject = JObject.Parse(jsonContent)

                    ' Initialize variables with default "N/A" value
                    Dim uiPriority As String = "N/A"
                    Dim drawOrder As String = "N/A"
                    Dim decalLayerMask As String = "N/A"

                    ' Check for UiPriority at the top level first
                    Dim topLevelUiPriority As JToken = jObject("UiPriority")
                    If topLevelUiPriority IsNot Nothing Then
                        uiPriority = topLevelUiPriority.ToString()
                    End If

                    ' Get the Float object and check its properties
                    Dim floatObject As JObject = TryCast(jObject("Float"), JObject)
                    If floatObject IsNot Nothing Then
                        ' If UiPriority was not found at the top level, check inside the Float object
                        If uiPriority = "N/A" Then
                            Dim floatUiPriority As JToken = floatObject("UiPriority")
                            If floatUiPriority IsNot Nothing Then
                                uiPriority = floatUiPriority.ToString()
                            End If
                        End If

                        ' Extract DrawOrder and colossal_DecalLayerMask from the Float object
                        drawOrder = If(floatObject("_DrawOrder") IsNot Nothing, floatObject("_DrawOrder").ToString(), "N/A")
                        decalLayerMask = If(floatObject("colossal_DecalLayerMask") IsNot Nothing, floatObject("colossal_DecalLayerMask").ToString(), "N/A")
                    End If

                    ' Format and display the text
                    Dim infoText As String = $"UiPriority:{uiPriority} | Draw Order:{drawOrder} | Decal Layer Mask:{decalLayerMask}"
                    Txt_InfoBar.Text = infoText
                Catch ex As Exception
                    Txt_InfoBar.Text = $"Error reading or parsing JSON!"
                End Try
            Else
                Txt_InfoBar.Text = "Decal or surface file not found!"
            End If
            ' If the mouse is no longer over an item, but was previously
        ElseIf itemAtMousePosition Is Nothing AndAlso _hoveredItem IsNot Nothing Then
            ' Restore the original text
            _hoveredItem = Nothing
            If Not _originalInfoBarText = String.Empty Then
                Txt_InfoBar.Text = _originalInfoBarText
                _originalInfoBarText = String.Empty
            End If
        End If
    End Sub

    ' Subroutine to update the Txt_InfoBar with mod statistics

    ' InfoBar updating subroutine
    Private Sub UpdateInfoBar()

        ' v1.4.6.3
        If Lst_Img.Items.Count = 0 Then
            Txt_InfoBar.Text = "No assets found."
            Return
        End If

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

    'Custom Assets Installer event handler
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

            Msm_FiltersDisabledOnly.Enabled = False ' Disable filter option in EAI mode

        Else
            ' Normal Mode: Restore mods list, enable Cmb_Mods and refresh display
            RestoreNormalModeDisplay()
            RestoreAndRefreshModSelection()
            Msm_FiltersDisabledOnly.Enabled = Enabled ' Disable filter option in EAI mode

        End If
        ' Save the settings every time the state changes
        ModScanner.SaveSettings()
    End Sub

    ' Go to Local Assets button event handler
    Private Sub Btn_GoLocal_Click(sender As Object, e As EventArgs) Handles Btn_GoLocal.Click
        Dim eaiItem As ComboBoxItem = Cmb_Mods.Items.Cast(Of ComboBoxItem)().FirstOrDefault(Function(item) item.Text = "Extra Assets Importer (Local Assets)")
        If eaiItem IsNot Nothing Then
            Cmb_Mods.SelectedItem = eaiItem
        End If
    End Sub

    ' Context menu item for viewing asset properties JSON files
    Private Sub MenuItem_AssetProperties_Click(sender As Object, e As EventArgs) Handles MenuItem_AssetProperties.Click
        ' Check if an item is selected in the ListView
        If Lst_Img.SelectedItems.Count > 0 Then
            ' Get the first selected item
            Dim selectedItem As ListViewItem = Lst_Img.SelectedItems(0)

            ' The full folder path is stored in the Tag property
            Dim assetFolderPath As String = CStr(selectedItem.Tag)

            Dim jsonFilePath As String = String.Empty

            ' First, check for the "decal.json" file
            Dim decalFilePath As String = Path.Combine(assetFolderPath, "decal.json")
            If File.Exists(decalFilePath) Then
                jsonFilePath = decalFilePath
            End If

            ' If "decal.json" was not found, check for "surface.json"
            If String.IsNullOrEmpty(jsonFilePath) Then
                Dim surfaceFilePath As String = Path.Combine(assetFolderPath, "surface.json")
                If File.Exists(surfaceFilePath) Then
                    jsonFilePath = surfaceFilePath
                End If
            End If

            ' If a valid JSON file path was found
            If Not String.IsNullOrEmpty(jsonFilePath) Then
                Try
                    ' Read the entire content of the JSON file into a string
                    Dim jsonContent As String = File.ReadAllText(jsonFilePath)

                    ' Deserialize the JSON string to format it
                    ' This allows the content to be re-formatted with indentation and line breaks
                    Dim jObject As JObject = JObject.Parse(jsonContent)

                    ' Serialize the JObject back to a string with formatting
                    Dim formattedJson As String = jObject.ToString(Formatting.Indented)

                    ' Display the formatted JSON content in a message box
                    CustomMessageBox.ShowCustom(formattedJson, "Item Properties", MessageBoxButtons.OK, MessageBoxIcon.Information,, 720)
                Catch ex As JsonReaderException
                    ' Handle specific JSON parsing errors
                    CustomMessageBox.ShowCustom($"Error reading JSON content: {ex.Message}", "JSON Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Catch ex As Exception
                    ' Handle any other error that may occur
                    CustomMessageBox.ShowCustom($"Error reading the JSON file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                ' If neither file was found, inform the user
                CustomMessageBox.ShowCustom("Neither 'decal.json' nor 'surface.json' was found in the item's folder.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
    End Sub


End Class
