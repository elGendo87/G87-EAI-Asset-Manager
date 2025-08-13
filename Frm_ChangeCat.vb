Imports System.Runtime.InteropServices
Public Class Frm_ChangeCat

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function DestroyIcon(hIcon As IntPtr) As Boolean
    End Function

    ' Select the current asset type by asset tag
    Dim CurrentAssetPath As String = ChangingCatAssetPath
    Dim CurrentAssetType As String
    Dim OriginalCat As String = System.IO.Path.GetDirectoryName(CurrentAssetPath).Split("\").Last() ' will store the original cat in case user cancel

    Private Sub Frm_ChangeCat_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' if empty asset name then return
        If ChangingCatAssetName = "" Then
            Me.Close()
            Return
        End If

        ' Set form title with the asset name
        Me.Text = "Changing '" & ChangingCatAssetName & "' Category"

        'Set Form Icon
        Dim bmp As Bitmap = CType(imageDictionary("Category"), Bitmap)

        ' Crear el handle
        Dim hIcon As IntPtr = bmp.GetHicon()

        ' Clonar el icono para que no dependa del handle
        Using tempIcon As Icon = Icon.FromHandle(hIcon)
            Me.Icon = CType(tempIcon.Clone(), Icon)
        End Using

        ' Liberar el handle
        DestroyIcon(hIcon)

        Btn_Cancel.Image = imageDictionary("Cancel")
        Btn_Change.Image = imageDictionary("QSave")


        ' Determine the asset type based on the path (case-insensitive)
        If CurrentAssetPath.IndexOf("\CustomDecals\", StringComparison.OrdinalIgnoreCase) >= 0 Then
            CurrentAssetType = "Decals"
        ElseIf CurrentAssetPath.IndexOf("\CustomSurfaces\", StringComparison.OrdinalIgnoreCase) >= 0 _
    OrElse CurrentAssetPath.IndexOf("\Surfaces\", StringComparison.OrdinalIgnoreCase) >= 0 Then
            CurrentAssetType = "Surfaces"
        ElseIf CurrentAssetPath.IndexOf("\CustomNetlanes\", StringComparison.OrdinalIgnoreCase) >= 0 Then
            CurrentAssetType = "NetLanes"
        Else
            ' If the asset type is not recognized, close the form
            MessageBox.Show("Asset type not recognized. Please check the asset.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ChangingCatAssetNew = OriginalCat ' restore the original category in case of cancel
            Me.Close()
            Return
        End If

        ' Determine the current asset category based on the asset path knowing that the asset path is in the format "\{AssetType}\{AssetCategory}\{AssetName}"
        Dim CurrentAssetCat As String = System.IO.Path.GetDirectoryName(CurrentAssetPath).Split("\").Last()

        'Dim PathToRemove As String = CurrentAssetCat & "\" & ChangingCatAssetName
        'MsgBox(PathToRemove, MsgBoxStyle.Information, "Path to Remove")

        Txt_CurrentAssetCat.Text = CurrentAssetCat

        ' Populate the list of categories by current asset type
        Dim categories As String()

        Select Case CurrentAssetType
            Case "Decals"
                categories = {"Alphabet", "Beach", "Graffiti", "Ground", "Industry", "Leaf", "Misc", "Numbers", "Parking", "Puddles", "RoadAssets", "RoadMarkings", "Stains", "Trash", "WallDecor"}
            Case "Surfaces"
                categories = {"Brick", "Concrete", "Grass", "Ground", "Misc", "Pavement", "Rock", "Sand", "Tiles", "Water", "Wood"}
            Case "NetLanes"
                categories = {"RoadMarking", "Roadway", "Misc", "Water"}
            Case Else
                Return
        End Select

        ' Add categories to the combo box and enable it
        Cmb_NewAssetCat.Items.AddRange(categories)
        Cmb_NewAssetCat.Enabled = True

        ' Set the current asset category in the combo box if exist (because it may not exist in the list)
        Cmb_NewAssetCat.Text = If(categories.Contains(CurrentAssetCat), CurrentAssetCat, categories(0))

    End Sub

    Private Sub Btn_Cancel_Click(sender As Object, e As EventArgs) Handles Btn_Cancel.Click
        ChangingCatAssetNew = OriginalCat ' restore the original category in case of cancel
        Me.Close()
    End Sub

    Private Sub Btn_Change_Click(sender As Object, e As EventArgs) Handles Btn_Change.Click

        ' if empty new asset category then return
        If Cmb_NewAssetCat.Text = "" Then
            MessageBox.Show("Please select a new asset category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ChangingCatAssetNew = OriginalCat ' restore the original category in case of cancel
            Return
        End If
        ' if the new asset category is the same as the current one then return
        If Cmb_NewAssetCat.Text = Txt_CurrentAssetCat.Text Then
            MessageBox.Show("The new asset category is the same as the current one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ChangingCatAssetNew = OriginalCat ' restore the original category in case of cancel
            Return
        End If

        ' Save the new asset category
        ChangingCatAssetNew = Cmb_NewAssetCat.Text

        ' Change the asset category by renaming the folder
        Dim CurrentAssetPath As String = ChangingCatAssetPath '...\ExtraAssetsImporter\CustomDecals\DirtyRoads\G87 Dirt Road Transition 22
        Dim CurrentAssetCat As String = System.IO.Path.GetDirectoryName(CurrentAssetPath).Split("\").Last() 'DirtyRoads
        'Dim PathToRemove As String = CurrentAssetCat & "\" & ChangingCatAssetName
        Dim BasePath As String = System.IO.Path.GetDirectoryName(CurrentAssetPath).Replace(CurrentAssetCat, "") '...\ExtraAssetsImporter\CustomDecals
        'MsgBox(BasePath, MsgBoxStyle.Information, "Base Path")
        Dim NewCatPath As String = BasePath & Cmb_NewAssetCat.Text '...\ExtraAssetsImporter\CustomDecals\NewCategory
        Dim NewAssetPath As String = BasePath & Cmb_NewAssetCat.Text & "\" & ChangingCatAssetName '...\ExtraAssetsImporter\CustomDecals\NewCategory\G87 Dirt Road Transition 22

        Debug.WriteLine("Current Asset Path: " & CurrentAssetPath)
        Debug.WriteLine("New Asset Path: " & NewAssetPath)

        Try
            ' check if the current asset category folder exists, if not then create it
            If Not System.IO.Directory.Exists(NewCatPath) Then
                'ask user if they want to create it
                Dim result = MessageBox.Show("Oh, looks like one or more folders for the new category do not exist." & vbCrLf & vbCrLf & "Do you want to create them?" & vbCrLf & vbCrLf & "Moving to: " & NewCatPath, "Creating Folders", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                If result = MsgBoxResult.Ok Then
                    'create the folder
                    System.IO.Directory.CreateDirectory(NewCatPath)
                Else
                    ChangingCatAssetNew = OriginalCat ' restore the original category in case of cancel
                    Return
                End If

            End If

            'check if new asset exist in the category folder then abort
            If System.IO.Directory.Exists(NewAssetPath) Then
                MessageBox.Show("The asset already exists in the category. Choose a different category or rename the asset.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ChangingCatAssetNew = OriginalCat ' restore the original category in case of cancel
                Return
            End If

            ' move the asset folder to the new category
            System.IO.Directory.Move(CurrentAssetPath, NewAssetPath)

            MessageBox.Show("Asset category changed successfully." & vbCrLf & vbCrLf & "You will be redirected to the new selected category.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error changing asset category: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ChangingCatAssetNew = OriginalCat ' restore the original category in case of cancel
            Me.Close()
        End Try


    End Sub
End Class