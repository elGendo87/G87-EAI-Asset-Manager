Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module ModScanner

    Public Structure ModInfo
        Public Property DisplayName As String
        Public Property RootPath As String

    End Structure

    ' Structure to hold mod statistics
    Public Structure ModStatistics

        Public Property ModVersion As String
        Public Property ModAuthor As String
        Public Property ModID As String

        Public Property TotalEnabledAssets As Integer
        Public Property TotalDisabledAssets As Integer
        Public Property TotalEnabledAssetSizeInMb As Double
        Public Property TotalDisabledAssetSizeInMb As Double

    End Structure

    Public IsFilterDisabledOnlyActive As Boolean = False

    Public allModFoldersCache As New List(Of ModInfo)()

    Public currentModPath As String = ""
    Public currentAssetTypePath As String = ""
    Public currentCategoryPath As String = ""
    Public currentSelectedAssetTag As String = ""

    ' NEW: Dictionary to hold images loaded from DataFiles folder
    Public imageDictionary As New Dictionary(Of String, Image)()
    Public Class ComboBoxItem
        Public Property Text As String
        Public Property Value As Object

        Public Overrides Function ToString() As String
            Return Text
        End Function
    End Class
    Public Sub LoadingStart()

        Dim formWidth As Integer = Frm_Main.ClientSize.Width
        Dim formHeight As Integer = Frm_Main.ClientSize.Height

        Dim x As Integer = (formWidth - Frm_Main.Lbl_Loading.Width) \ 2
        Dim y As Integer = (formHeight - Frm_Main.Lbl_Loading.Height) \ 2

        Frm_Main.Lbl_Loading.Location = New Point(x, y)

        'Frm_Main.Lst_Img.Visible = False

        Frm_Main.Lbl_Loading.Visible = True

        Frm_Main.Update() ' Ensure the UI updates immediately

    End Sub

    Public Sub LoadingStop()

        Frm_Main.Lbl_Loading.Visible = False

        'Frm_Main.Lst_Img.Visible = True

        Frm_Main.Update() ' Ensure the UI updates immediately

    End Sub

    Public Sub DisableCmbandBtns()
        With Frm_Main
            .Cmb_Mods.Enabled = False
            .Cmb_AssetType.Enabled = False
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
            .Cmb_AssetType.Enabled = True
            .Cmb_Cat.Enabled = True
            .Lst_Img.Enabled = True
            '.Btn_DisableSelectedItems.Enabled = True
            '.Btn_EnableSelectedItems.Enabled = True
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
        Frm_Main.Cmb_AssetType.Items.Clear()
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

        Dim eaiRootPath As String = CustomFiles.GetEAICustomFolderPath()
        If Directory.Exists(eaiRootPath) Then
            If HasDisabledAssets(eaiRootPath) Then
                filteredMods.Add(New ModInfo With {.DisplayName = "Extra Assets Importer (Local Assets)", .RootPath = eaiRootPath}) ' Updated DisplayName
            End If
        End If

        PopulateModsComboBox(filteredMods)
        EnableCmbandBtns()

        RestoreSelections()
    End Sub

    Private Sub AddEAICustomFolderToScan(ByRef modList As List(Of ModInfo))
        Dim eaiRootPath As String = CustomFiles.GetEAICustomFolderPath()

        If Directory.Exists(eaiRootPath) Then
            modList.Add(New ModInfo With {.DisplayName = "Extra Assets Importer (Local Assets)", .RootPath = eaiRootPath}) ' Updated DisplayName
        End If
    End Sub

    Public Sub PopulateModsComboBox(ByVal modsToLoad As List(Of ModInfo))
        Frm_Main.Cmb_Mods.Items.Clear()
        Frm_Main.Cmb_AssetType.Items.Clear()
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
            ' Handled by RestoreSelections()
        Else
            Frm_Main.Cmb_AssetType.Enabled = False
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
                        If Path.GetFileName(assetFolder).StartsWith("."c) Then
                            Return True
                        End If
                    Next
                Next
            End If
        Next
        Return False
    End Function

    Public Sub LoadAssetTypes(modRootPath As String)
        Frm_Main.Cmb_AssetType.Items.Clear()
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
            Frm_Main.Cmb_AssetType.Items.Add(item)
        Next

        If Frm_Main.Cmb_AssetType.Items.Count > 0 Then
            ' Handled by RestoreSelections()
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
                    If Path.GetFileName(assetFolder).StartsWith("."c) Then
                        Return True
                    End If
                Next
            Next
        Else
            For Each assetFolder In Directory.GetDirectories(pathToSearch)
                If Path.GetFileName(assetFolder).StartsWith("."c) Then
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
            ' Handled by RestoreSelections()
        Else
            Frm_Main.Lst_Img.Enabled = False
        End If
    End Sub

    Public Sub LoadAssetsIntoListView(categoryPath As String)

        LoadingStart()

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
                        If Not IsFilterDisabledOnlyActive OrElse Path.GetFileName(assetFolder).StartsWith("."c) Then
                            Dim imgForImageList As New Bitmap(originalImage)

                            Dim folderNameWithoutDot As String = Path.GetFileName(assetFolder).TrimStart(".")
                            If Not Frm_Main.Lst_Img.LargeImageList.Images.ContainsKey(folderNameWithoutDot) Then
                                Frm_Main.Lst_Img.LargeImageList.Images.Add(folderNameWithoutDot, imgForImageList)

                            End If

                            Dim item As New ListViewItem With {
                                .Text = folderNameWithoutDot,
                                .ImageKey = folderNameWithoutDot,
                                .Tag = assetFolder
                            }

                            If Path.GetFileName(assetFolder).StartsWith("."c) Then
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

        LoadingStop()

    End Sub

    Public Sub DisableAsset(assetFullPath As String)
        If Directory.Exists(assetFullPath) Then
            Dim currentFolderName As String = Path.GetFileName(assetFullPath)
            If Not currentFolderName.StartsWith("."c) Then
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
            If currentFolderName.StartsWith("."c) Then
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

        If Frm_Main.Cmb_AssetType.SelectedItem IsNot Nothing Then
            currentAssetTypePath = DirectCast(Frm_Main.Cmb_AssetType.SelectedItem, ComboBoxItem).Value.ToString()
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
            Dim found As Boolean = False
            For i As Integer = 0 To Frm_Main.Cmb_Mods.Items.Count - 1
                Dim item As ComboBoxItem = DirectCast(Frm_Main.Cmb_Mods.Items(i), ComboBoxItem)
                If item.Value.ToString().Equals(currentModPath, StringComparison.OrdinalIgnoreCase) Then
                    Frm_Main.Cmb_Mods.SelectedIndex = i
                    found = True
                    Exit For
                End If
            Next
            If Not found AndAlso Frm_Main.Cmb_Mods.Items.Count > 0 Then
                Frm_Main.Cmb_Mods.SelectedIndex = 0
            End If
        ElseIf Frm_Main.Cmb_Mods.Items.Count > 0 Then
            Frm_Main.Cmb_Mods.SelectedIndex = 0
        End If

        If Not String.IsNullOrEmpty(currentAssetTypePath) Then
            Dim found As Boolean = False
            For i As Integer = 0 To Frm_Main.Cmb_AssetType.Items.Count - 1
                Dim item As ComboBoxItem = DirectCast(Frm_Main.Cmb_AssetType.Items(i), ComboBoxItem)
                If item.Value.ToString().Equals(currentAssetTypePath, StringComparison.OrdinalIgnoreCase) Then
                    Frm_Main.Cmb_AssetType.SelectedIndex = i
                    found = True
                    Exit For
                End If
            Next
            If Not found AndAlso Frm_Main.Cmb_AssetType.Items.Count > 0 Then
                Frm_Main.Cmb_AssetType.SelectedIndex = 0
            End If
        ElseIf Frm_Main.Cmb_AssetType.Items.Count > 0 Then
            Frm_Main.Cmb_AssetType.SelectedIndex = 0
        End If

        If Not String.IsNullOrEmpty(currentCategoryPath) Then
            Dim found As Boolean = False
            For i As Integer = 0 To Frm_Main.Cmb_Cat.Items.Count - 1
                Dim item As ComboBoxItem = DirectCast(Frm_Main.Cmb_Cat.Items(i), ComboBoxItem)
                If item.Value.ToString().Equals(currentCategoryPath, StringComparison.OrdinalIgnoreCase) Then
                    Frm_Main.Cmb_Cat.SelectedIndex = i
                    found = True
                    Exit For
                End If
            Next
            If Not found AndAlso Frm_Main.Cmb_Cat.Items.Count > 0 Then
                Frm_Main.Cmb_Cat.SelectedIndex = 0
            End If
        ElseIf Frm_Main.Cmb_Cat.Items.Count > 0 Then
            Frm_Main.Cmb_Cat.SelectedIndex = 0
        End If
    End Sub

    ' Function to calculate statistics for a given mod path
    Public Function CalculateModStatistics(modRootPath As String) As ModStatistics
        Dim stats As New ModStatistics()

        ' NEW: Read metadata from the specific mod folder
        Dim metadataFile As String = Path.Combine(modRootPath, ".metadata", "metadata.json")
        If File.Exists(metadataFile) Then
            Try
                Dim jsonContent As String = File.ReadAllText(metadataFile)
                Dim jsonObject As JObject = JObject.Parse(jsonContent)
                ' Use the keys provided: UserModVersion, Author, ID
                stats.modVersion = If(jsonObject.ContainsKey("UserModVersion"), jsonObject.Value(Of String)("UserModVersion"), "N/A")
                stats.ModAuthor = If(jsonObject.ContainsKey("Author"), jsonObject.Value(Of String)("Author"), "N/A")
                stats.ModID = If(jsonObject.ContainsKey("Id"), jsonObject.Value(Of String)("Id"), "N/A")
            Catch ex As Exception
                Console.WriteLine("Error reading metadata for stats in " & modRootPath & ": " & ex.Message)
                stats.modVersion = "Error"
                stats.ModAuthor = "Error"
                stats.ModID = "Error"
            End Try
        Else
            ' Handle cases like the EAI folder which has no metadata file.
            ' These will be empty strings.
            stats.modVersion = ""
            stats.ModAuthor = ""
            stats.ModID = ""
        End If

        ' --- The rest of the function remains the same ---
        stats.TotalEnabledAssets = 0
        stats.TotalDisabledAssets = 0
        stats.TotalEnabledAssetSizeInMb = 0.0
        stats.TotalDisabledAssetSizeInMb = 0.0

        If Not Directory.Exists(modRootPath) Then
            Return stats ' Return stats (with metadata if found) if mod path doesn't exist
        End If

        Dim assetSubFolders As New List(Of String) From {"CustomDecals", "CustomSurfaces", "Surfaces", "CustomNetlanes"}

        For Each subFolderType As String In assetSubFolders
            Dim fullAssetTypePath As String = Path.Combine(modRootPath, subFolderType)
            If Directory.Exists(fullAssetTypePath) Then
                For Each categoryFolder In Directory.GetDirectories(fullAssetTypePath)
                    For Each assetFolder In Directory.GetDirectories(categoryFolder)
                        Dim assetSizeInBytes As Long = GetDirectorySize(assetFolder)
                        Dim assetSizeInMb As Double = Math.Round(assetSizeInBytes / (1024.0 * 1024.0), 2) ' Convert to MB and round to 2 decimal places

                        If Path.GetFileName(assetFolder).StartsWith("."c) Then
                            stats.TotalDisabledAssets += 1
                            stats.TotalDisabledAssetSizeInMb += assetSizeInMb
                        Else
                            stats.TotalEnabledAssets += 1
                            stats.TotalEnabledAssetSizeInMb += assetSizeInMb
                        End If
                    Next
                Next
            End If
        Next

        Return stats
    End Function

    ' Helper function to get the size of a directory recursively in bytes
    Private Function GetDirectorySize(path As String) As Long
        Dim totalSize As Long = 0
        If Not Directory.Exists(path) Then
            Return 0
        End If

        Try
            For Each file As String In Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                Dim fileInfo As New FileInfo(file)
                totalSize += fileInfo.Length
            Next
        Catch ex As Exception
            Console.WriteLine("Error calculating directory size for " & path & ": " & ex.Message)
            ' Return current totalSize even if an error occurs for some files/folders
        End Try
        Return totalSize
    End Function

    ' Load asset types from all mods combined
    Public Sub LoadAssetTypesFromAllMods()
        Frm_Main.Cmb_AssetType.Items.Clear()
        Frm_Main.Cmb_Cat.Items.Clear()
        Frm_Main.Lst_Img.Items.Clear()

        Dim assetTypesToAdd As New HashSet(Of String)()

        ' Iterate through all mods in cache
        For Each modInfo In allModFoldersCache
            Dim modRootPath As String = modInfo.RootPath

            ' Check each asset type
            If Directory.Exists(Path.Combine(modRootPath, "CustomDecals")) Then
                assetTypesToAdd.Add("CustomDecals")
            End If
            If Directory.Exists(Path.Combine(modRootPath, "CustomNetlanes")) Then
                assetTypesToAdd.Add("CustomNetlanes")
            End If

            ' Handle both CustomSurfaces and Surfaces as the same type
            If Directory.Exists(Path.Combine(modRootPath, "CustomSurfaces")) OrElse
           Directory.Exists(Path.Combine(modRootPath, "Surfaces")) Then
                assetTypesToAdd.Add("Surfaces") ' Always use "Surfaces" as the unified type
            End If
        Next

        ' Add found types to ComboBox
        For Each assetType In assetTypesToAdd.OrderBy(Function(x) x)
            Dim displayName As String = If(assetType = "CustomDecals", "Decals",
                                   If(assetType = "CustomNetlanes", "Netlanes", "Surfaces"))
            Frm_Main.Cmb_AssetType.Items.Add(New ComboBoxItem With {.Text = displayName, .Value = assetType})
        Next

        If Frm_Main.Cmb_AssetType.Items.Count > 0 Then
            Frm_Main.Cmb_AssetType.SelectedIndex = 0
        End If
    End Sub

    Public Sub LoadCategoriesFromAllMods(assetType As String)
        Frm_Main.Cmb_Cat.Items.Clear()
        Frm_Main.Lst_Img.Items.Clear()

        Dim categoriesToAdd As New HashSet(Of String)()

        ' Iterate through all mods
        For Each modInfo In allModFoldersCache
            Dim assetTypePath As String = ""

            ' Handle the unified Surfaces type - check both possible folder names
            If assetType = "Surfaces" Then
                Dim customSurfacesPath As String = Path.Combine(modInfo.RootPath, "CustomSurfaces")
                Dim surfacesPath As String = Path.Combine(modInfo.RootPath, "Surfaces")

                If Directory.Exists(customSurfacesPath) Then
                    assetTypePath = customSurfacesPath
                ElseIf Directory.Exists(surfacesPath) Then
                    assetTypePath = surfacesPath
                End If
            Else
                assetTypePath = Path.Combine(modInfo.RootPath, assetType)
            End If

            If Directory.Exists(assetTypePath) Then
                For Each categoryFolder In Directory.GetDirectories(assetTypePath)
                    Dim categoryName As String = Path.GetFileName(categoryFolder)
                    If Not IsFilterDisabledOnlyActive OrElse HasDisabledAssetsInPath(categoryFolder, False) Then
                        categoriesToAdd.Add(categoryName)
                    End If
                Next
            End If
        Next

        ' Add categories to ComboBox
        For Each category In categoriesToAdd.OrderBy(Function(x) x)
            Frm_Main.Cmb_Cat.Items.Add(New ComboBoxItem With {.Text = category, .Value = category})
        Next

        If Frm_Main.Cmb_Cat.Items.Count > 0 Then
            Frm_Main.Cmb_Cat.SelectedIndex = 0
        End If
    End Sub

    Public Sub LoadAssetsFromAllMods(assetType As String, category As String)
        LoadingStart()

        Frm_Main.Lst_Img.Items.Clear()
        Frm_Main.Lst_Img.LargeImageList.Images.Clear()

        ' Iterate through all mods
        For Each modInfo In allModFoldersCache
            Dim categoryPath As String = ""

            ' Handle the unified Surfaces type - check both possible folder names
            If assetType = "Surfaces" Then
                Dim customSurfacesPath As String = Path.Combine(modInfo.RootPath, "CustomSurfaces", category)
                Dim surfacesPath As String = Path.Combine(modInfo.RootPath, "Surfaces", category)

                If Directory.Exists(customSurfacesPath) Then
                    LoadAssetsFromPath(customSurfacesPath, modInfo.DisplayName)
                End If
                If Directory.Exists(surfacesPath) Then
                    LoadAssetsFromPath(surfacesPath, modInfo.DisplayName)
                End If
            Else
                categoryPath = Path.Combine(modInfo.RootPath, assetType, category)
                If Directory.Exists(categoryPath) Then
                    LoadAssetsFromPath(categoryPath, modInfo.DisplayName)
                End If
            End If
        Next

        LoadingStop()
    End Sub

    Private Sub LoadAssetsFromPath(categoryPath As String, modName As String)
        For Each assetFolder In Directory.GetDirectories(categoryPath)
            Dim iconPath As String = Path.Combine(assetFolder, "icon.png")

            If File.Exists(iconPath) Then
                Try
                    Using originalImage As Image = Image.FromFile(iconPath)
                        If Not IsFilterDisabledOnlyActive OrElse Path.GetFileName(assetFolder).StartsWith("."c) Then
                            Dim folderNameWithoutDot As String = Path.GetFileName(assetFolder).TrimStart(".")
                            Dim uniqueKey As String = $"{modName}_{folderNameWithoutDot}"

                            If Not Frm_Main.Lst_Img.LargeImageList.Images.ContainsKey(uniqueKey) Then
                                Dim imgForImageList As New Bitmap(originalImage)
                                Frm_Main.Lst_Img.LargeImageList.Images.Add(uniqueKey, imgForImageList)
                            End If

                            'Dim displayText As String = $"{folderNameWithoutDot} ({modName})"
                            Dim displayText As String = $"{folderNameWithoutDot}"
                            Dim item As New ListViewItem With {
                            .Text = displayText,
                            .ImageKey = uniqueKey,
                            .Tag = assetFolder
                        }

                            If Path.GetFileName(assetFolder).StartsWith("."c) Then
                                item.ForeColor = Color.Red
                            Else
                                item.ForeColor = Color.Green
                            End If

                            Frm_Main.Lst_Img.Items.Add(item)
                        End If
                    End Using
                Catch ex As Exception
                    Console.WriteLine($"Error loading icon for {assetFolder}: {ex.Message}")
                End Try
            End If
        Next
    End Sub

    ' Unified refresh function that handles both normal and EAI modes
    Public Sub RefreshCurrentView()
        If Frm_Main.Chk_ShowEAI.Checked Then
            ' EAI Mode: Refresh assets from all mods
            If Frm_Main.Cmb_AssetType.SelectedItem IsNot Nothing AndAlso Frm_Main.Cmb_Cat.SelectedItem IsNot Nothing Then
                Dim selectedAssetType As ComboBoxItem = Frm_Main.Cmb_AssetType.SelectedItem
                Dim selectedCategory As ComboBoxItem = Frm_Main.Cmb_Cat.SelectedItem
                LoadAssetsFromAllMods(selectedAssetType.Value.ToString(), selectedCategory.Value.ToString())
            End If
        Else
            ' Normal Mode: Use existing refresh logic
            If Frm_Main.Cmb_Cat.SelectedItem IsNot Nothing AndAlso Frm_Main.Cmb_Mods.SelectedItem IsNot Nothing AndAlso Frm_Main.Cmb_AssetType.SelectedItem IsNot Nothing Then
                Dim selectedCategory As ComboBoxItem = Frm_Main.Cmb_Cat.SelectedItem
                Dim modRootPath As String = DirectCast(Frm_Main.Cmb_Mods.SelectedItem, ComboBoxItem).Value.ToString()
                Dim assetSubFolder As String = DirectCast(Frm_Main.Cmb_AssetType.SelectedItem, ComboBoxItem).Value.ToString()
                Dim categoryFolder As String = selectedCategory.Value.ToString()

                Dim fullCategoryPath As String = Path.Combine(modRootPath, assetSubFolder, categoryFolder)
                LoadAssetsIntoListView(fullCategoryPath)
            End If
        End If
    End Sub

    ' Store the currently selected mod before switching to EAI mode
    Private storedModSelection As String = ""

    Public Sub StoreCurrentModSelection()
        If Frm_Main.Cmb_Mods.SelectedItem IsNot Nothing Then
            Dim selectedMod As ComboBoxItem = Frm_Main.Cmb_Mods.SelectedItem
            storedModSelection = selectedMod.Value.ToString()
        Else
            storedModSelection = ""
        End If
    End Sub

    ' Set EAI mode display (clear mods combo and set EAI text)
    Public Sub SetEAIModeDisplay()
        Frm_Main.Cmb_Mods.Items.Clear()
        Frm_Main.Cmb_Mods.Items.Add(New ComboBoxItem With {.Text = "EAI Sorting", .Value = ""})
        Frm_Main.Cmb_Mods.SelectedIndex = 0
        Frm_Main.Cmb_Mods.Enabled = False
    End Sub

    ' Restore normal mode display (repopulate mods and enable combo)
    Public Sub RestoreNormalModeDisplay()
        Frm_Main.Cmb_Mods.Enabled = True
        PopulateModsComboBox(allModFoldersCache) ' Restore original mod list
    End Sub

    ' Restore mod selection and force refresh
    Public Sub RestoreAndRefreshModSelection()
        If Not String.IsNullOrEmpty(storedModSelection) Then
            ' Try to restore the previously selected mod
            Dim found As Boolean = False
            For i As Integer = 0 To Frm_Main.Cmb_Mods.Items.Count - 1
                Dim item As ComboBoxItem = DirectCast(Frm_Main.Cmb_Mods.Items(i), ComboBoxItem)
                If item.Value.ToString().Equals(storedModSelection, StringComparison.OrdinalIgnoreCase) Then
                    Frm_Main.Cmb_Mods.SelectedIndex = i
                    found = True
                    Exit For
                End If
            Next

            ' If stored selection not found, select first item
            If Not found AndAlso Frm_Main.Cmb_Mods.Items.Count > 0 Then
                Frm_Main.Cmb_Mods.SelectedIndex = 0
            End If
        ElseIf Frm_Main.Cmb_Mods.Items.Count > 0 Then
            ' If no stored selection, select first item
            Frm_Main.Cmb_Mods.SelectedIndex = 0
        End If

        ' Clear stored selection
        storedModSelection = ""
    End Sub

    ' Rescan cache only (without UI changes)
    Public Sub RefreshModsCache()
        allModFoldersCache.Clear()
        Dim modsDirectory As String = GetBaseScanDirectory()
        If String.IsNullOrEmpty(modsDirectory) OrElse Not Directory.Exists(modsDirectory) Then
            Return
        End If

        ' Rebuild cache (same logic as in ScanFolders but without UI updates)
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
    End Sub

    'Unified rescan Function (simplified)
    Public Sub RescanAndRefresh()
        If Frm_Main.Chk_ShowEAI.Checked Then
            ' EAI Mode: Refresh cache and current view
            RefreshModsCache()
            RefreshCurrentView()
        Else
            ' Normal Mode: Standard rescan
            StoreCurrentSelections()
            If IsFilterDisabledOnlyActive Then
                ScanFilteredFolders()
            Else
                ScanFolders()
            End If
            RestoreSelections()
        End If
    End Sub

    ' *** LOAD IMAGES FORM DATAFILES FOLDER ***
    Public Sub LoadImagesFromDataFiles()
        Dim dataFilesPath As String = Path.Combine(Application.StartupPath, "DataFiles")

        imageDictionary.Clear() ' Clear any existing images

        If Directory.Exists(dataFilesPath) Then
            Dim pngFiles() As String = Directory.GetFiles(dataFilesPath, "*.png", SearchOption.TopDirectoryOnly)

            If pngFiles.Length > 0 Then
                For Each filePath As String In pngFiles
                    Try
                        ' Get just the file name WITHOUT the extension to use as the dictionary key
                        Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(filePath)

                        ' Load the image and add it to the dictionary
                        ' Using the filename without extension as the key
                        imageDictionary.Add(fileNameWithoutExtension, Image.FromFile(filePath))
                        Console.WriteLine($"Loaded image: {fileNameWithoutExtension}") ' For debugging purposes
                    Catch ex As Exception
                        Console.WriteLine($"Error loading image '{filePath}': {ex.Message}")
                    End Try
                Next
            Else
                Console.WriteLine("No PNG files found in the 'DataFiles' folder.")
            End If
        Else
            MessageBox.Show("The 'DataFiles' folder was not found at: " & dataFilesPath, "Folder Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ' --- Apply default theme icons on load ---
    ' set icon for each button in the form non _inv colors
    Public Sub ApplyLightThemeIcons()
        Try ' Asignar iconos por defecto a los botones

            Frm_Main.MenuItem_DisableAsset.Image = imageDictionary("Disable")
            Frm_Main.MenuItem_EnableAsset.Image = imageDictionary("Enable")
            Frm_Main.MenuItem_CreateLocalCopy.Image = imageDictionary("Copy")
            Frm_Main.MenuItem_DeleteLocalAsset.Image = imageDictionary("Delete")
            Frm_Main.MenuItem_EditLocalAsset.Image = imageDictionary("Edit")
            Frm_Main.MenuItem_OpenLocation.Image = imageDictionary("OpenExplorer")
            Frm_Main.MenuItem_RenameLocalAsset.Image = imageDictionary("Rename")


            Frm_Main.Msm_Close.Image = imageDictionary("Close")
            Frm_Main.Msm_FiltersDisabledOnly.Image = imageDictionary("Filter")

            Frm_Main.Btn_EnableSelectedItems.Image = imageDictionary("Enable")
            Frm_Main.Btn_DisableSelectedItems.Image = imageDictionary("Disable")



        Catch ex As Exception
            MessageBox.Show("Error loading icon files: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
        End Try

    End Sub

End Module
