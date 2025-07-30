Module EditorMod

    ' Public method to load asset data into the Frm_AssetEditor form
    ' This routine sets the public properties of the Frm_AssetEditor instance.
    Public Sub LoadAssetData(targetForm As Frm_AssetEditor, fullPath As String, name As String, icon As Image, assetType As String, category As String)
        targetForm.AssetFullPath = fullPath
        targetForm.AssetName = name
        targetForm.AssetIcon = icon
        targetForm.AssetType = assetType
        targetForm.AssetCategory = category
    End Sub

End Module
