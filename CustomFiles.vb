Imports System.IO
Imports Microsoft.VisualBasic.FileIO ' For FileSystem.CopyDirectory
Imports Microsoft.VisualBasic.Interaction ' For InputBox

Module CustomFiles

    ' Function to get the EAI custom folder path
    Public Function GetEAICustomFolderPath() As String
        Dim defaultLocalLowPath As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        Dim appDataRoot As String = Path.GetDirectoryName(defaultLocalLowPath) ' This gives C:\Users\Username\AppData
        Return Path.Combine(appDataRoot, "LocalLow\Colossal Order\Cities Skylines II\ModsData\ExtraAssetsImporter")
    End Function

    ' Helper function for asset name validation
    ' Allowed characters: alphanumeric, space, hyphen, underscore
    ' Max length: 128 characters
    Private Function IsValidAssetName(name As String) As Boolean
        If String.IsNullOrEmpty(name) OrElse name.Length > 128 OrElse name.StartsWith(".") Then
            Return False
        End If

        For Each c As Char In name
            If Not (Char.IsLetterOrDigit(c) OrElse c = " "c OrElse c = "-"c OrElse c = "_"c) Then
                Return False
            End If
        Next
        Return True
    End Function

    ' Copies an asset directory to the ExtraAssetsImporter folder
    ' Returns True if copy is successful, False otherwise (including user cancellation)
    Public Function CreateLocalCopy(sourceAssetPath As String, assetType As String, categoryName As String) As Boolean
        Dim destinationBasePath As String = GetEAICustomFolderPath()
        Dim destAssetTypePath As String = Path.Combine(destinationBasePath, assetType)
        Dim destCategoryPath As String = Path.Combine(destAssetTypePath, categoryName)

        ' Create destination directories if they don't exist
        Try
            If Not Directory.Exists(destCategoryPath) Then
                Directory.CreateDirectory(destCategoryPath)
            End If
        Catch ex As Exception
            MessageBox.Show("Error creating destination directory: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

        Dim originalAssetName As String = Path.GetFileName(sourceAssetPath)
        Dim newAssetName As String = originalAssetName
        Dim counter As Integer = 0
        Dim isValidInput As Boolean = False
        Dim userCancelled As Boolean = False

        ' Loop to get a valid and unique name from the user
        Do
            Dim suggestedName As String
            If counter = 0 Then
                suggestedName = originalAssetName
            Else
                ' Suggest "OriginalName Copy" or "OriginalName Copy N"
                suggestedName = originalAssetName & " Copy" & If(counter > 1, " " & counter.ToString(), "")
            End If

            Dim inputResult As String = InputBox(
                "Enter a name for the local copy of '" & originalAssetName & "'.",
                "Create Local Copy",
                suggestedName
            )

            If inputResult = "" Then ' User clicked Cancel or entered empty string
                userCancelled = True
                Exit Do
            End If

            newAssetName = inputResult.Trim()

            ' Validate the new name
            If Not IsValidAssetName(newAssetName) Then
                MessageBox.Show("The name cannot start with a dot ('.'), exceed 128 characters, or contain invalid characters. Only letters, numbers, spaces, hyphens (-), and underscores (_) are allowed.", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ElseIf Directory.Exists(Path.Combine(destCategoryPath, newAssetName)) Then
                MessageBox.Show("A folder with this name already exists in the destination. Please choose a different name.", "Name Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                counter += 1 ' Increment counter for next suggestion
            Else
                isValidInput = True ' Name is valid and unique
            End If
        Loop While Not isValidInput AndAlso Not userCancelled

        If userCancelled Then
            MessageBox.Show("Copy operation cancelled by the user.", "Operation Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        Dim finalDestPath As String = Path.Combine(destCategoryPath, newAssetName)

        ' Perform the directory copy
        Try
            FileSystem.CopyDirectory(sourceAssetPath, finalDestPath, True) ' True to overwrite if destination exists, though we checked.
            MessageBox.Show("Asset '" & originalAssetName & "' copied successfully to '" & finalDestPath & "'.", "Copy Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return True
        Catch ex As Exception
            MessageBox.Show("Error copying asset: " & ex.Message, "Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' Deletes an asset directory from the ExtraAssetsImporter folder
    ' Returns True if deletion is successful, False otherwise
    Public Function DeleteLocalAsset(assetPath As String) As Boolean
        Dim eaiPath As String = GetEAICustomFolderPath()

        ' Ensure the asset is indeed within the EAI folder before attempting deletion
        If Not assetPath.StartsWith(eaiPath, StringComparison.OrdinalIgnoreCase) Then
            MessageBox.Show("This asset cannot be deleted as it is not located in the Extra Assets Importer folder.", "Deletion Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If Not Directory.Exists(assetPath) Then
            MessageBox.Show("The asset folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        Dim assetName As String = Path.GetFileName(assetPath)

        ' Change icon to Warning
        If MessageBox.Show("Are you sure you want to permanently delete the local asset '" & assetName & "'?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                FileSystem.DeleteDirectory(assetPath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException)
                MessageBox.Show("Asset '" & assetName & "' deleted successfully.", "Deletion Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return True
            Catch ex As OperationCanceledException
                MessageBox.Show("Deletion operation cancelled by the user.", "Operation Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            Catch ex As Exception
                MessageBox.Show("Error deleting asset: " & ex.Message, "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        Else
            MessageBox.Show("Deletion operation cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If
    End Function

    ' Renames a local asset directory within the ExtraAssetsImporter folder
    ' Returns True if rename is successful, False otherwise (including user cancellation)
    Public Function RenameLocalAsset(currentAssetPath As String) As Boolean
        Dim eaiPath As String = GetEAICustomFolderPath()

        ' Ensure the asset is indeed within the EAI folder before attempting rename
        If Not currentAssetPath.StartsWith(eaiPath, StringComparison.OrdinalIgnoreCase) Then
            MessageBox.Show("This asset cannot be renamed as it is not located in the Extra Assets Importer folder.", "Rename Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If Not Directory.Exists(currentAssetPath) Then
            MessageBox.Show("The asset folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        Dim originalName As String = Path.GetFileName(currentAssetPath)
        Dim parentPath As String = Path.GetDirectoryName(currentAssetPath)
        Dim newName As String = originalName
        Dim isValidInput As Boolean = False
        Dim userCancelled As Boolean = False

        ' Loop to get a valid and unique name from the user
        Do
            Dim inputResult As String = InputBox(
                "Enter a new name for '" & originalName & "'.",
                "Rename Local Asset",
                newName ' Suggest the current name initially
            )

            If inputResult = "" Then ' User clicked Cancel or entered empty string
                userCancelled = True
                Exit Do
            End If

            newName = inputResult.Trim()

            ' If the new name is the same as the original, no action needed
            If newName.Equals(originalName, StringComparison.OrdinalIgnoreCase) Then
                MessageBox.Show("The new name is the same as the current name. No rename performed.", "No Change", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            ' Validate the new name
            If Not IsValidAssetName(newName) Then
                MessageBox.Show("The name cannot start with a dot ('.'), exceed 128 characters, or contain invalid characters. Only letters, numbers, spaces, hyphens (-), and underscores (_) are allowed.", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ElseIf Directory.Exists(Path.Combine(parentPath, newName)) Then
                MessageBox.Show("A folder with this name already exists in this location. Please choose a different name.", "Name Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                isValidInput = True ' Name is valid and unique
            End If
        Loop While Not isValidInput AndAlso Not userCancelled

        If userCancelled Then
            MessageBox.Show("Rename operation cancelled by the user.", "Operation Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        Dim newFullPath As String = Path.Combine(parentPath, newName)

        ' Perform the directory rename
        Try
            Directory.Move(currentAssetPath, newFullPath)
            MessageBox.Show("Asset '" & originalName & "' successfully renamed to '" & newName & "'.", "Rename Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return True
        Catch ex As Exception
            MessageBox.Show("Error renaming asset: " & ex.Message, "Rename Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

End Module
