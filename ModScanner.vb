Imports System.IO
Imports Newtonsoft.Json

Module ModScanner

    Public Structure ModInfo
        Public Property DisplayName As String
        Public Property RootPath As String
    End Structure

    Public IsFilterDisabledOnlyActive As Boolean = False

    Public allModFoldersCache As New List(Of ModInfo)() ' Declarado como Public

    Public currentModPath As String = "" ' Declarado como Public
    Public currentAssetTypePath As String = "" ' Declarado como Public
    Public currentCategoryPath As String = "" ' Declarado como Public
    Public currentSelectedAssetTag As String = "" ' Declarado como Public

    Public Sub FormFormat()
        Frm_Main.Text = "[G87] Asset Manager - V1.2.2 Beta"
        Frm_Main.Width = 1280
        Frm_Main.Height = 720
        Frm_Main.StartPosition = FormStartPosition.CenterScreen
        Frm_Main.MaximizeBox = True
        Frm_Main.MinimizeBox = True
        Frm_Main.MinimumSize = New Size(1280, 720)

        With Frm_Main.Cmb_Mods
            .DropDownStyle = ComboBoxStyle.DropDownList
        End With

        With Frm_Main.Cmb_AssetType ' Corregido: Cmb_AssetType
            .DropDownStyle = ComboBoxStyle.DropDownList
        End With

        With Frm_Main.Cmb_Cat
            .DropDownStyle = ComboBoxStyle.DropDownList
        End With

        With Frm_Main.Lst_Img
            .View = View.LargeIcon
            .LargeImageList = New ImageList() With {.ImageSize = New Size(128, 128)}
            .CheckBoxes = True
        End With
    End Sub

    Public Sub DisableCmbandBtns()
        With Frm_Main
            .Cmb_Mods.Enabled = False
            .Cmb_AssetType.Enabled = False ' Corregido: Cmb_AssetType
            .Cmb_Cat.Enabled = False
            .Lst_Img.Enabled = False
            .Btn_DisableSelectedItems.Enabled = False
            .Btn_EnableSelectedItems.Enabled = False
            .Msm_Filter.Enabled = False
        End With
    End Sub

    Public Sub EnableCmbandBtns()
        With Frm_Main
            .Cmb_Mods.Enabled = True
            .Cmb_AssetType.Enabled = True ' Corregido: Cmb_AssetType
            .Cmb_Cat.Enabled = True
            .Lst_Img.Enabled = True
            .Btn_DisableSelectedItems.Enabled = True
            .Btn_EnableSelectedItems.Enabled = True
            .Msm_Filter.Enabled = True
        End With
    End Sub

    Private Function GetBaseScanDirectory() As String
        Dim defaultLocalLowPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        Dim appDataRoot As String = Path.GetDirectoryName(defaultLocalLowPath)
        Dim expectedModsPath As String = Path.Combine(appDataRoot, "LocalLow\Colossal Order\Cities Skylines II\.cache\Mods\mods_subscribed")

        If Directory.Exists(expectedModsPath) Then
            Return expectedModsPath
        Else
            Dim result As DialogResult = MessageBox.Show(
                "The 'mods_subscribed' folder was not found at the default location:" & Environment.NewLine &
                expectedModsPath & Environment.NewLine & Environment.NewLine &
                "Do you want to browse for it manually?",
                "Mods Folder Not Found",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )

            If result = DialogResult.Yes Then
                Using folderBrowserDialog As New FolderBrowserDialog()
                    folderBrowserDialog.Description = "Please select the 'mods_subscribed' folder for Cities Skylines II."
                    folderBrowserDialog.ShowNewFolderButton = False

                    If Directory.Exists(Path.GetDirectoryName(expectedModsPath)) Then
                        folderBrowserDialog.SelectedPath = Path.GetDirectoryName(expectedModsPath)
                    ElseIf Directory.Exists(Path.Combine(appDataRoot, "LocalLow\Colossal Order\Cities Skylines II")) Then
                        folderBrowserDialog.SelectedPath = Path.Combine(appDataRoot, "LocalLow\Colossal Order\Cities Skylines II")
                    ElseIf Directory.Exists(appDataRoot) Then
                        folderBrowserDialog.SelectedPath = appDataRoot
                    End If

                    If folderBrowserDialog.ShowDialog() = DialogResult.OK Then
                        If Path.GetFileName(folderBrowserDialog.SelectedPath).Equals("mods_subscribed", StringComparison.OrdinalIgnoreCase) Then
                            Return folderBrowserDialog.SelectedPath
                        Else
                            MessageBox.Show(
                                "The selected folder is not 'mods_subscribed'. Please select the correct folder.",
                                "Incorrect Folder",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            )
                            Return Nothing
                        End If
                    Else
                        MessageBox.Show("No folder was selected. Scan operation cancelled.", "Operation Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return Nothing
                    End If
                End Using
            Else
                MessageBox.Show("Scan operation cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            End If
        End If
    End Function

    Public Sub ScanFolders()
        DisableCmbandBtns()
        allModFoldersCache.Clear()

        Dim modsDirectory As String = GetBaseScanDirectory()
        If String.IsNullOrEmpty(modsDirectory) OrElse Not Directory.Exists(modsDirectory) Then
            MessageBox.Show("Could not determine a valid path for 'mods_subscribed'.", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DisableCmbandBtns()
            Return
        End If

        For Each modFolder In Directory.GetDirectories(modsDirectory)
            Dim hasCustomFolder As Boolean = False
            Dim metadataPath As String = Path.Combine(modFolder, ".metadata")
            Dim metadataFile As String = Path.Combine(metadataPath, "metadata.json")

            If Directory.Exists(Path.Combine(modFolder, "CustomDecals")) Then hasCustomFolder = True
            If Directory.Exists(Path.Combine(modFolder, "CustomSurfaces")) Then hasCustomFolder = True
            If Directory.Exists(Path.Combine(modFolder, "Surfaces")) Then hasCustomFolder = True
            If Directory.Exists(Path.Combine(modFolder, "CustomNetlanes")) Then hasCustomFolder = True

            If hasCustomFolder AndAlso File.Exists(metadataFile) Then
                Try
                    Dim jsonContent As String = File.ReadAllText(metadataFile)
                    Dim jsonObject As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(jsonContent)

                    If jsonObject.ContainsKey("DisplayName") Then
                        Dim displayName As String = jsonObject("DisplayName").ToString()
                        allModFoldersCache.Add(New ModInfo With {.DisplayName = displayName, .RootPath = modFolder})
                    End If
                Catch ex As Exception
                    Console.WriteLine("Error reading metadata.json in " & modFolder & ": " & ex.Message)
                End Try
            End If
        Next

        AddEAICustomFolderToScan(allModFoldersCache)

        PopulateModsComboBox(allModFoldersCache)
        EnableCmbandBtns()

        RestoreSelections()
    End Sub

    Public Sub ScanFilteredFolders()
        DisableCmbandBtns()

        Dim modsDirectory As String = GetBaseScanDirectory()
        If String.IsNullOrEmpty(modsDirectory) OrElse Not Directory.Exists(modsDirectory) Then
            MessageBox.Show("Could not determine a valid path for 'mods_subscribed' for filtering.", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DisableCmbandBtns()
            Return
        End If

        Frm_Main.Cmb_Mods.Items.Clear()
        Frm_Main.Cmb_AssetType.Items.Clear() ' Corregido: Cmb_AssetType
        Frm_Main.Cmb_Cat.Items.Clear()
        Frm_Main.Lst_Img.Items.Clear()

        Dim filteredMods As New List(Of ModInfo)

        For Each modFolder In Directory.GetDirectories(modsDirectory)
            Dim metadataPath As String = Path.Combine(modFolder, ".metadata")
            Dim metadataFile As String = Path.Combine(metadataPath, "metadata.json")

            If File.Exists(metadataFile) AndAlso HasDisabledAssets(modFolder) Then
                Try
                    Dim jsonContent As String = File.ReadAllText(metadataFile)
                    Dim jsonObject As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(jsonContent)

                    If jsonObject.ContainsKey("DisplayName") Then
                        Dim displayName As String = jsonObject("DisplayName").ToString()
                        filteredMods.Add(New ModInfo With {.DisplayName = displayName, .RootPath = modFolder})
                    End If
                Catch ex As Exception
                    Console.WriteLine("Error reading metadata.json in " & modFolder & ": " & ex.Message)
                End Try
            End If
        Next

        Dim eaiRootPath As String = GetEAICustomFolderPath()
        If Directory.Exists(eaiRootPath) Then
            If HasDisabledAssets(eaiRootPath) Then
                filteredMods.Add(New ModInfo With {.DisplayName = "Extra Assets Importer", .RootPath = eaiRootPath})
            End If
        End If

        PopulateModsComboBox(filteredMods)
        EnableCmbandBtns()

        RestoreSelections()
    End Sub

    Private Function GetEAICustomFolderPath() As String
        Dim defaultLocalLowPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        Dim appDataRoot As String = Path.GetDirectoryName(defaultLocalLowPath)
        Return Path.Combine(appDataRoot, "LocalLow\Colossal Order\Cities Skylines II\ModsData\ExtraAssetsImporter")
    End Function

    Private Sub AddEAICustomFolderToScan(ByRef modList As List(Of ModInfo))
        Dim eaiRootPath As String = GetEAICustomFolderPath()

        If Directory.Exists(eaiRootPath) Then
            modList.Add(New ModInfo With {.DisplayName = "Extra Assets Importer", .RootPath = eaiRootPath})
        End If
    End Sub

    Public Sub PopulateModsComboBox(ByVal modsToLoad As List(Of ModInfo))
        Frm_Main.Cmb_Mods.Items.Clear()
        Frm_Main.Cmb_AssetType.Items.Clear() ' Corregido: Cmb_AssetType
        Frm_Main.Cmb_Cat.Items.Clear()
        Frm_Main.Lst_Img.Items.Clear()

        Dim sortedMods = modsToLoad.OrderBy(Function(mods) mods.DisplayName).ToList()

        For Each Mods In sortedMods
            Dim listItem As New ComboBoxItem With {
                .Text = Mods.DisplayName,
                .Value = Mods.RootPath
            }
            Frm_Main.Cmb_Mods.Items.Add(listItem)
        Next

        If Frm_Main.Cmb_Mods.Items.Count > 0 Then
            ' Frm_Main.Cmb_Mods.SelectedIndex = 0
        Else
            Frm_Main.Cmb_AssetType.Enabled = False ' Corregido: Cmb_AssetType
            Frm_Main.Cmb_Cat.Enabled = False
            Frm_Main.Lst_Img.Enabled = False
        End If
    End Sub

    Private Function HasDisabledAssets(modRootPath As String) As Boolean
        Dim assetSubFolders As New List(Of String) From {"CustomDecals", "CustomSurfaces", "Surfaces", "CustomNetlanes"}

        For Each subFolderType As String In assetSubFolders
            Dim fullAssetTypePath As String = Path.Combine(modRootPath, subFolderType)
            If Directory.Exists(fullAssetTypePath) Then
                For Each categoryFolder In Directory.GetDirectories(fullAssetTypePath)
                    For Each assetFolder In Directory.GetDirectories(categoryFolder)
                        If Path.GetFileName(assetFolder).StartsWith(".") Then
                            Return True
                        End If
                    Next
                Next
            End If
        Next
        Return False
    End Function

    Public Sub LoadAssetTypes(modRootPath As String)
        Frm_Main.Cmb_AssetType.Items.Clear() ' Corregido: Cmb_AssetType
        Frm_Main.Cmb_Cat.Items.Clear()
        Frm_Main.Lst_Img.Items.Clear()

        If Not Directory.Exists(modRootPath) Then Return

        Dim assetTypesToAdd As New List(Of ComboBoxItem)

        Dim decalsPath As String = Path.Combine(modRootPath, "CustomDecals")
        If Directory.Exists(decalsPath) Then
            If Not IsFilterDisabledOnlyActive OrElse HasDisabledAssetsInPath(decalsPath, True) Then
                assetTypesToAdd.Add(New ComboBoxItem With {.Text = "Decals", .Value = "CustomDecals"})
            End If
        End If

        Dim netlanesPath As String = Path.Combine(modRootPath, "CustomNetlanes")
        If Directory.Exists(netlanesPath) Then
            If Not IsFilterDisabledOnlyActive OrElse HasDisabledAssetsInPath(netlanesPath, True) Then
                assetTypesToAdd.Add(New ComboBoxItem With {.Text = "Netlanes", .Value = "CustomNetlanes"})
            End If
        End If

        Dim surfacesPath As String = Path.Combine(modRootPath, "CustomSurfaces")
        If Directory.Exists(surfacesPath) Then
            If Not IsFilterDisabledOnlyActive OrElse HasDisabledAssetsInPath(surfacesPath, True) Then
                assetTypesToAdd.Add(New ComboBoxItem With {.Text = "Surfaces", .Value = "CustomSurfaces"})
            End If
        Else
            surfacesPath = Path.Combine(modRootPath, "Surfaces")
            If Directory.Exists(surfacesPath) Then
                If Not IsFilterDisabledOnlyActive OrElse HasDisabledAssetsInPath(surfacesPath, True) Then
                    assetTypesToAdd.Add(New ComboBoxItem With {.Text = "Surfaces", .Value = "Surfaces"})
                End If
            End If
        End If

        For Each item In assetTypesToAdd.OrderBy(Function(x) x.Text)
            Frm_Main.Cmb_AssetType.Items.Add(item) ' Corregido: Cmb_AssetType
        Next

        If Frm_Main.Cmb_AssetType.Items.Count > 0 Then ' Corregido: Cmb_AssetType
            ' Frm_Main.Cmb_AssetType.SelectedIndex = 0 
        Else
            Frm_Main.Cmb_Cat.Enabled = False
            Frm_Main.Lst_Img.Enabled = False
        End If
    End Sub

    Private Function HasDisabledAssetsInPath(pathToSearch As String, Optional isAssetTypePath As Boolean = False) As Boolean
        If Not Directory.Exists(pathToSearch) Then Return False

        If isAssetTypePath Then
            For Each categoryFolder In Directory.GetDirectories(pathToSearch)
                For Each assetFolder In Directory.GetDirectories(categoryFolder)
                    If Path.GetFileName(assetFolder).StartsWith(".") Then
                        Return True
                    End If
                Next
            Next
        Else
            For Each assetFolder In Directory.GetDirectories(pathToSearch)
                If Path.GetFileName(assetFolder).StartsWith(".") Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Public Sub LoadCategories(assetTypePath As String)
        Frm_Main.Cmb_Cat.Items.Clear()
        Frm_Main.Lst_Img.Items.Clear()

        If Not Directory.Exists(assetTypePath) Then
            Console.WriteLine("Error: Asset type directory does not exist: " & assetTypePath)
            Return
        End If

        Dim categoriesToAdd As New List(Of ComboBoxItem)

        For Each categoryFolder In Directory.GetDirectories(assetTypePath)
            Dim categoryName As String = Path.GetFileName(categoryFolder)
            If Not IsFilterDisabledOnlyActive OrElse HasDisabledAssetsInPath(categoryFolder, False) Then
                categoriesToAdd.Add(New ComboBoxItem With {.Text = categoryName, .Value = categoryName})
            End If
        Next

        For Each item In categoriesToAdd.OrderBy(Function(x) x.Text)
            Frm_Main.Cmb_Cat.Items.Add(item)
        Next

        If Frm_Main.Cmb_Cat.Items.Count > 0 Then
            ' Frm_Main.Cmb_Cat.SelectedIndex = 0
        Else
            Frm_Main.Lst_Img.Enabled = False
        End If
    End Sub

    Public Sub LoadAssetsIntoListView(categoryPath As String)
        Frm_Main.Lst_Img.Items.Clear()
        Frm_Main.Lst_Img.LargeImageList.Images.Clear()

        If Not Directory.Exists(categoryPath) Then
            Console.WriteLine("Error: Category directory does not exist: " & categoryPath)
            Return
        End If

        For Each assetFolder In Directory.GetDirectories(categoryPath)
            Dim iconPath As String = Path.Combine(assetFolder, "icon.png")

            If File.Exists(iconPath) Then
                Try
                    Using originalImage As Image = Image.FromFile(iconPath)
                        If Not IsFilterDisabledOnlyActive OrElse Path.GetFileName(assetFolder).StartsWith(".") Then
                            Dim imgForImageList As New Bitmap(originalImage)

                            Dim folderNameWithoutDot As String = Path.GetFileName(assetFolder).TrimStart(".")
                            If Not Frm_Main.Lst_Img.LargeImageList.Images.ContainsKey(folderNameWithoutDot) Then
                                Frm_Main.Lst_Img.LargeImageList.Images.Add(folderNameWithoutDot, imgForImageList)
                            End If

                            Dim item As New ListViewItem()
                            item.Text = folderNameWithoutDot
                            item.ImageKey = folderNameWithoutDot
                            item.Tag = assetFolder

                            If Path.GetFileName(assetFolder).StartsWith(".") Then
                                item.ForeColor = Color.Red
                            Else
                                item.ForeColor = Color.Green
                            End If

                            Frm_Main.Lst_Img.Items.Add(item)
                        End If
                    End Using
                Catch ex As Exception
                    Console.WriteLine("Error loading icon.png or adding to ListView for " & assetFolder & ": " & ex.Message)
                End Try
            Else
                Console.WriteLine("icon.png not found in " & assetFolder & ". Skipping this folder.")
            End If
        Next

        If Not String.IsNullOrEmpty(currentSelectedAssetTag) Then
            For Each item As ListViewItem In Frm_Main.Lst_Img.Items
                If item.Tag.ToString().Equals(currentSelectedAssetTag, StringComparison.OrdinalIgnoreCase) Then
                    item.Selected = True
                    item.EnsureVisible()
                    Exit For
                End If
            Next
            currentSelectedAssetTag = ""
        End If
    End Sub

    Public Sub DisableAsset(assetFullPath As String)
        If Directory.Exists(assetFullPath) Then
            Dim currentFolderName As String = Path.GetFileName(assetFullPath)
            If Not currentFolderName.StartsWith(".") Then
                Dim parentPath As String = Path.GetDirectoryName(assetFullPath)
                Dim newFullPath As String = Path.Combine(parentPath, "." & currentFolderName)
                Try
                    Directory.Move(assetFullPath, newFullPath)
                Catch ex As Exception
                    MessageBox.Show("Error disabling asset: " & ex.Message & Environment.NewLine & "Ensure the game and other applications that might access this folder are closed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        Else
            MessageBox.Show("Asset folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Public Sub EnableAsset(assetFullPath As String)
        If Directory.Exists(assetFullPath) Then
            Dim currentFolderName As String = Path.GetFileName(assetFullPath)
            If currentFolderName.StartsWith(".") Then
                Dim parentPath As String = Path.GetDirectoryName(assetFullPath)
                Dim newFolderName As String = currentFolderName.TrimStart(".")
                Dim newFullPath As String = Path.Combine(parentPath, newFolderName)
                Try
                    Directory.Move(assetFullPath, newFullPath)
                Catch ex As Exception
                    MessageBox.Show("Error enabling asset: " & ex.Message & Environment.NewLine + "Ensure the game and other applications that might access this folder are closed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        Else
            MessageBox.Show("Asset folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Public Sub StoreCurrentSelections()
        If Frm_Main.Cmb_Mods.SelectedItem IsNot Nothing Then
            currentModPath = DirectCast(Frm_Main.Cmb_Mods.SelectedItem, ComboBoxItem).Value.ToString()
        Else
            currentModPath = ""
        End If

        If Frm_Main.Cmb_AssetType.SelectedItem IsNot Nothing Then ' Corregido: Cmb_AssetType
            currentAssetTypePath = DirectCast(Frm_Main.Cmb_AssetType.SelectedItem, ComboBoxItem).Value.ToString() ' Corregido: Cmb_AssetType
        Else
            currentAssetTypePath = ""
        End If

        If Frm_Main.Cmb_Cat.SelectedItem IsNot Nothing Then
            currentCategoryPath = DirectCast(Frm_Main.Cmb_Cat.SelectedItem, ComboBoxItem).Value.ToString()
        Else
            currentCategoryPath = ""
        End If

        If Frm_Main.Lst_Img.SelectedItems.Count > 0 Then
            currentSelectedAssetTag = Frm_Main.Lst_Img.SelectedItems(0).Tag.ToString()
        Else
            currentSelectedAssetTag = ""
        End If
    End Sub

    Public Sub RestoreSelections()
        If Not String.IsNullOrEmpty(currentModPath) Then
            For i As Integer = 0 To Frm_Main.Cmb_Mods.Items.Count - 1
                Dim item As ComboBoxItem = DirectCast(Frm_Main.Cmb_Mods.Items(i), ComboBoxItem)
                If item.Value.ToString().Equals(currentModPath, StringComparison.OrdinalIgnoreCase) Then
                    Frm_Main.Cmb_Mods.SelectedIndex = i
                    Exit For
                End If
            Next
        ElseIf Frm_Main.Cmb_Mods.Items.Count > 0 Then
            Frm_Main.Cmb_Mods.SelectedIndex = 0
        End If

        If Not String.IsNullOrEmpty(currentAssetTypePath) Then
            For i As Integer = 0 To Frm_Main.Cmb_AssetType.Items.Count - 1 ' Corregido: Cmb_AssetType
                Dim item As ComboBoxItem = DirectCast(Frm_Main.Cmb_AssetType.Items(i), ComboBoxItem) ' Corregido: Cmb_AssetType
                If item.Value.ToString().Equals(currentAssetTypePath, StringComparison.OrdinalIgnoreCase) Then
                    Frm_Main.Cmb_AssetType.SelectedIndex = i ' Corregido: Cmb_AssetType
                    Exit For
                End If
            Next
        ElseIf Frm_Main.Cmb_AssetType.Items.Count > 0 Then ' Corregido: Cmb_AssetType
            Frm_Main.Cmb_AssetType.SelectedIndex = 0 ' Corregido: Cmb_AssetType
        End If

        If Not String.IsNullOrEmpty(currentCategoryPath) Then
            For i As Integer = 0 To Frm_Main.Cmb_Cat.Items.Count - 1
                Dim item As ComboBoxItem = DirectCast(Frm_Main.Cmb_Cat.Items(i), ComboBoxItem)
                If item.Value.ToString().Equals(currentCategoryPath, StringComparison.OrdinalIgnoreCase) Then
                    Frm_Main.Cmb_Cat.SelectedIndex = i
                    Exit For
                End If
            Next
        ElseIf Frm_Main.Cmb_Cat.Items.Count > 0 Then
            Frm_Main.Cmb_Cat.SelectedIndex = 0
        End If
    End Sub

    Public Class ComboBoxItem
        Public Property Text As String
        Public Property Value As Object

        Public Overrides Function ToString() As String
            Return Text
        End Function
    End Class

End Module