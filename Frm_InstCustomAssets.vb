Imports System.IO
Imports Microsoft.VisualBasic.FileIO
Imports System.Windows.Forms
Imports System.Text ' For StringBuilder, although not used here, good practice to import if needed

Public Class Frm_InstCustomAssets

    ' Private variables to hold the state of the form and installation process
    Private _eaiCustomPath As String
    Private _validAssetPaths As New List(Of String)()
    Private _invalidJsonFolders As New List(Of String)()
    Private _missingPngFolders As New List(Of String)()
    Private _selectedCategoryName As String = ""

    Private Sub Frm_InstCustomAssets_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ResetForm() ' Clear the form

        ' Load Asset Types into the first ComboBox
        Cmb_InstAssetType.Items.AddRange({"Decals", "Surfaces", "NetLanes"})

        ' Disable controls initially
        DisableAllControls()

        ' Get and store the ExtraAssetsImporter path
        _eaiCustomPath = CustomFiles.GetEAICustomFolderPath()

        ' Set the initial directory for the folder browser dialog to the user's downloads folder
        FBD_Assets.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
    End Sub

    ' Subroutine to disable all relevant controls
    Private Sub DisableAllControls()

        Cmb_InstAssetType.Enabled = True
        Cmb_InstAssetCat.Enabled = False
        Btn_FindFiles.Enabled = False
        Txt_FolderPath.Enabled = False
        Lst_Assets.Enabled = False
        Btn_Install.Enabled = False

    End Sub

    ' Subroutine to clear controls and reset state
    Private Sub ResetForm()

        Cmb_InstAssetType.SelectedIndex = -1
        Cmb_InstAssetCat.Items.Clear()

        Lst_Assets.Items.Clear()
        Txt_FolderPath.Text = ""
        _validAssetPaths.Clear()
        _invalidJsonFolders.Clear()
        _missingPngFolders.Clear()
        _selectedCategoryName = ""


        DisableAllControls()

    End Sub

    Private Sub Cmb_InstAssetType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_InstAssetType.SelectedIndexChanged
        ' Reset category combo box and other controls
        Cmb_InstAssetCat.Items.Clear()
        Cmb_InstAssetCat.Enabled = False
        Btn_FindFiles.Enabled = False
        Lst_Assets.Items.Clear()
        Btn_Install.Enabled = False
        Txt_FolderPath.Text = ""

        ' Only proceed if an item is selected
        If Cmb_InstAssetType.SelectedItem IsNot Nothing AndAlso Cmb_InstAssetType.Text <> "" Then
            Dim assetType As String = Cmb_InstAssetType.SelectedItem.ToString()
            Dim categories As String()

            Select Case assetType
                Case "Decals"
                    categories = {"Alphabet", "Beach", "Graffiti", "Ground", "Industry", "Leaf", "Misc", "Numbers", "Parking", "Puddles", "RoadAssets", "RoadMarkings", "Stains", "Trash", "WallDecor"}
                Case "Surfaces"
                    categories = {"Brick", "Concrete", "Grass", "Ground", "Misc", "Pavement", "Rock", "Sand", "Tiles", "Water", "Wood"}
                Case "NetLanes"
                    categories = {"RoadMarking", "Roadway", "Misc"}
                Case Else
                    Return
            End Select

            ' Add categories to the combo box and enable it
            Cmb_InstAssetCat.Items.AddRange(categories)
            Cmb_InstAssetCat.Enabled = True
        End If
    End Sub

    Private Sub Cmb_InstAssetCat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmb_InstAssetCat.SelectedIndexChanged
        ' Reset controls and enable the find files button if a category is selected
        Btn_FindFiles.Enabled = Cmb_InstAssetCat.SelectedItem IsNot Nothing
        Lst_Assets.Items.Clear()
        Btn_Install.Enabled = False
        Txt_FolderPath.Text = ""
        _validAssetPaths.Clear()

    End Sub

    Private Sub Btn_FindFiles_Click(sender As Object, e As EventArgs) Handles Btn_FindFiles.Click
        ' Show the folder browser dialog
        If FBD_Assets.ShowDialog() = DialogResult.OK Then
            Dim pathCopyFrom As String = FBD_Assets.SelectedPath
            Txt_FolderPath.Text = pathCopyFrom
            Txt_FolderPath.ReadOnly = True
            Txt_FolderPath.Enabled = True

            ' Clear previous lists
            _validAssetPaths.Clear()
            _invalidJsonFolders.Clear()
            _missingPngFolders.Clear()

            ' Perform asset verification
            Dim assetType As String = Cmb_InstAssetType.SelectedItem.ToString()
            Dim requiredJsonFile1 As String = ""
            Dim requiredJsonFile2 As String = ""

            Select Case assetType
                Case "Decals"
                    requiredJsonFile1 = "decal.json"
                Case "Surfaces"
                    requiredJsonFile1 = "surface.json"
                Case "NetLanes"
                    requiredJsonFile1 = "netlane.json"
                    requiredJsonFile2 = "decal.json"
            End Select

            ' Iterate through subdirectories to find valid assets
            For Each assetFolder As String In Directory.GetDirectories(pathCopyFrom)
                Dim hasJson1 As Boolean = File.Exists(Path.Combine(assetFolder, requiredJsonFile1))
                Dim hasJson2 As Boolean = (requiredJsonFile2 = "" OrElse File.Exists(Path.Combine(assetFolder, requiredJsonFile2)))

                If hasJson1 AndAlso hasJson2 Then
                    ' Check for the BaseColorMap PNG file
                    Dim hasPng As Boolean = False
                    For Each file As String In Directory.GetFiles(assetFolder)
                        If Path.GetFileName(file).ToLower().Contains("_basecolormap.png") Then
                            hasPng = True
                            Exit For
                        End If
                    Next

                    If hasPng Then
                        _validAssetPaths.Add(assetFolder)
                    Else
                        _missingPngFolders.Add(Path.GetFileName(assetFolder))
                    End If
                Else
                    _invalidJsonFolders.Add(Path.GetFileName(assetFolder))
                End If
            Next

            ' Perform the second verification for the category name
            If _validAssetPaths.Count <> 0 Then
                Dim pathFolderName As String = Path.GetFileName(pathCopyFrom)
                Dim selectedCategoryName As String = Cmb_InstAssetCat.SelectedItem.ToString()
                Dim matchingCategory As String = ""

                For Each item In Cmb_InstAssetCat.Items
                    If pathFolderName.Equals(item.ToString(), StringComparison.InvariantCultureIgnoreCase) Then
                        matchingCategory = item.ToString()
                        Exit For
                    End If
                Next

                If matchingCategory <> "" AndAlso matchingCategory <> selectedCategoryName Then
                    Dim result As DialogResult = MessageBox.Show(
                    $"The folder name '{pathFolderName}' matches the category '{matchingCategory}'. Do you want to use the selected category '{selectedCategoryName}'? " & vbCrLf & vbCrLf &
                    $"If you select 'No', the category will be changed to '{matchingCategory}'.",
                    "Category Mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                    If result = DialogResult.Yes Then
                        _selectedCategoryName = selectedCategoryName
                    Else
                        _selectedCategoryName = matchingCategory
                        ' Update the combo box to reflect the change
                        Cmb_InstAssetCat.SelectedItem = matchingCategory
                    End If
                Else
                    _selectedCategoryName = selectedCategoryName
                End If
            Else
                MessageBox.Show("No valid assets were found in the selected directory.", "No Assets Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Inform the user about folders to be skipped due to missing files
            Dim summaryMessage As String = ""
                If _invalidJsonFolders.Count > 0 Then
                    summaryMessage &= "The following folders will be skipped due to missing required JSON files:" & vbCrLf & String.Join(vbCrLf, _invalidJsonFolders) & vbCrLf
                End If
                If _missingPngFolders.Count > 0 Then
                    summaryMessage &= "The following folders will be skipped due to missing a '_BaseColorMap.png' file:" & vbCrLf & String.Join(vbCrLf, _missingPngFolders) & vbCrLf
                End If

                If Not String.IsNullOrEmpty(summaryMessage) Then
                    MessageBox.Show(summaryMessage, "Asset Verification Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

                ' Populate the list box with valid assets and enable the install button
                If _validAssetPaths.Count > 0 Then
                    For Each path In _validAssetPaths
                        Lst_Assets.Items.Add(System.IO.Path.GetFileName(path))
                    Next
                    Lst_Assets.Enabled = True
                    Btn_Install.Enabled = True
                    Cmb_InstAssetType.Enabled = False
                    Cmb_InstAssetCat.Enabled = False
                    Btn_FindFiles.Enabled = False
                Else
                    MessageBox.Show("No valid assets were found in the selected directory.", "No Assets Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
    End Sub

    Private Sub Btn_Install_Click(sender As Object, e As EventArgs) Handles Btn_Install.Click
        ' Determine destination paths
        Dim assetTypeFolder As String
        Select Case Cmb_InstAssetType.SelectedItem.ToString()
            Case "Decals"
                assetTypeFolder = "CustomDecals"
            Case "Surfaces"
                assetTypeFolder = "CustomSurfaces"
            Case "NetLanes"
                assetTypeFolder = "CustomNetLanes"
            Case Else
                MessageBox.Show("Invalid Asset Type selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
        End Select

        Dim destinationAssetTypePath As String = Path.Combine(_eaiCustomPath, assetTypeFolder)
        Dim destinationCategoryPath As String = Path.Combine(destinationAssetTypePath, _selectedCategoryName)

        ' Verify and create Asset Type folder
        If Not Directory.Exists(destinationAssetTypePath) Then
            Dim result As DialogResult = MessageBox.Show($"The directory '{destinationAssetTypePath}' does not exist. Do you want to create it?", "Create Directory", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                Directory.CreateDirectory(destinationAssetTypePath)
            Else
                MessageBox.Show("Installation cancelled by user.", "Installation Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
        End If

        ' Verify and create Asset Category folder
        If Not Directory.Exists(destinationCategoryPath) Then
            Dim result As DialogResult = MessageBox.Show($"The category directory '{destinationCategoryPath}' does not exist. Do you want to create it?", "Create Directory", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                Directory.CreateDirectory(destinationCategoryPath)
            Else
                MessageBox.Show("Installation cancelled by user.", "Installation Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
        End If

        ' Copy the valid assets
        Dim copiedCount As Integer = 0
        For Each sourcePath As String In _validAssetPaths
            Dim destinationPath As String = Path.Combine(destinationCategoryPath, Path.GetFileName(sourcePath))
            Try
                If Directory.Exists(destinationPath) Then
                    Dim overwriteResult As DialogResult = MessageBox.Show($"The folder '{Path.GetFileName(sourcePath)}' already exists in the destination. Do you want to replace it?", "Overwrite Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If overwriteResult = DialogResult.Yes Then
                        ' Delete existing directory before copying to avoid conflicts
                        Directory.Delete(destinationPath, True)
                        FileSystem.CopyDirectory(sourcePath, destinationPath, True)
                        copiedCount += 1
                    End If
                Else
                    FileSystem.CopyDirectory(sourcePath, destinationPath, True)
                    copiedCount += 1
                End If
            Catch ex As Exception
                MessageBox.Show($"An error occurred while copying '{Path.GetFileName(sourcePath)}': {ex.Message}", "Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Next

        ' Provide final summary
        Dim finalMessage As New System.Text.StringBuilder()
        finalMessage.AppendLine($"{copiedCount} asset folder(s) were successfully installed.")

        If _invalidJsonFolders.Count > 0 OrElse _missingPngFolders.Count > 0 Then
            finalMessage.AppendLine(vbCrLf & "The following folders were skipped during verification:")
            For Each folderName In _invalidJsonFolders
                finalMessage.AppendLine($"- {folderName} (missing required JSON files)")
            Next
            For Each folderName In _missingPngFolders
                finalMessage.AppendLine($"- {folderName} (missing _BaseColorMap.png file)")
            Next
        End If

        MessageBox.Show(finalMessage.ToString(), "Installation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' Reset the form for a new installation
        ResetForm()

    End Sub

    Private Sub Btn_Cancel_Click(sender As Object, e As EventArgs) Handles Btn_Cancel.Click

        Me.Close() 'close the form

    End Sub

    Private Sub Frm_InstCustomAssets_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        ' Reset the form state when closing
        ResetForm()

    End Sub
End Class
