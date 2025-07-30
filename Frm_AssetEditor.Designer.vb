<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_AssetEditor
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_AssetEditor))
        Pbx_Icon = New PictureBox()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Gbx_DecalLayerMask = New GroupBox()
        Lbl_DlmValue = New Label()
        Label5 = New Label()
        Chk_DlmCreatures = New CheckBox()
        Chk_DlmProps = New CheckBox()
        Chk_DlmVehicles = New CheckBox()
        Chk_DlmBuildings = New CheckBox()
        Chk_DlmRoads = New CheckBox()
        Chk_DlmGround = New CheckBox()
        Txt_AssetName = New TextBox()
        Btn_Cancel = New Button()
        Btn_Modify = New Button()
        Gbx_Advanced = New GroupBox()
        Txt_MetallicOpacity = New TextBox()
        Lbl_MetallicOpacity = New Label()
        Txt_Smoothness = New TextBox()
        Txt_Metallic = New TextBox()
        Lbl_Smoothness = New Label()
        Lbl_Metallic = New Label()
        Gbx_DecalSize = New GroupBox()
        Chk_KeepAspectRatio = New CheckBox()
        Lbl_LockStatus = New Label()
        Txt_Z = New TextBox()
        Txt_Y = New TextBox()
        Txt_X = New TextBox()
        Lbl_Length = New Label()
        Lbl_Thickness = New Label()
        Lbl_Width = New Label()
        Lbl_AssetType = New Label()
        Lbl_AssetTypeName = New Label()
        Lbl_AssetCat = New Label()
        Lbl_AssetCatName = New Label()
        Gbx_SurfacesOnly = New GroupBox()
        Txt_MeshSize = New TextBox()
        Lbl_MeshSizeZ = New Label()
        Chk_CalcUV = New CheckBox()
        Lbl_MeshSizeM = New Label()
        Txt_colossal_EdgeNormal = New TextBox()
        Txt_colossal_UVScale = New TextBox()
        Lbl_EdgeNormal = New Label()
        Txt_Roundness = New TextBox()
        Label4 = New Label()
        Lbl_Roundness = New Label()
        ToolTipAE = New ToolTip(components)
        GroupBox1 = New GroupBox()
        Txt_NormalOpacity = New TextBox()
        Lbl_NormalOpacity = New Label()
        Nud_UiPriority = New NumericUpDown()
        Nud_DrawOrder = New NumericUpDown()
        CType(Pbx_Icon, ComponentModel.ISupportInitialize).BeginInit()
        Gbx_DecalLayerMask.SuspendLayout()
        Gbx_Advanced.SuspendLayout()
        Gbx_DecalSize.SuspendLayout()
        Gbx_SurfacesOnly.SuspendLayout()
        GroupBox1.SuspendLayout()
        CType(Nud_UiPriority, ComponentModel.ISupportInitialize).BeginInit()
        CType(Nud_DrawOrder, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Pbx_Icon
        ' 
        Pbx_Icon.Location = New Point(12, 12)
        Pbx_Icon.MaximumSize = New Size(128, 128)
        Pbx_Icon.MinimumSize = New Size(128, 128)
        Pbx_Icon.Name = "Pbx_Icon"
        Pbx_Icon.Size = New Size(128, 128)
        Pbx_Icon.TabIndex = 0
        Pbx_Icon.TabStop = False
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(154, 12)
        Label1.Name = "Label1"
        Label1.Size = New Size(73, 15)
        Label1.TabIndex = 1
        Label1.Text = "Asset Name:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(154, 41)
        Label2.Name = "Label2"
        Label2.Size = New Size(59, 15)
        Label2.TabIndex = 2
        Label2.Text = "UiPriority:"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(152, 71)
        Label3.Name = "Label3"
        Label3.Size = New Size(70, 15)
        Label3.TabIndex = 3
        Label3.Text = "Draw Order:"
        ' 
        ' Gbx_DecalLayerMask
        ' 
        Gbx_DecalLayerMask.Controls.Add(Lbl_DlmValue)
        Gbx_DecalLayerMask.Controls.Add(Label5)
        Gbx_DecalLayerMask.Controls.Add(Chk_DlmCreatures)
        Gbx_DecalLayerMask.Controls.Add(Chk_DlmProps)
        Gbx_DecalLayerMask.Controls.Add(Chk_DlmVehicles)
        Gbx_DecalLayerMask.Controls.Add(Chk_DlmBuildings)
        Gbx_DecalLayerMask.Controls.Add(Chk_DlmRoads)
        Gbx_DecalLayerMask.Controls.Add(Chk_DlmGround)
        Gbx_DecalLayerMask.Location = New Point(146, 101)
        Gbx_DecalLayerMask.Name = "Gbx_DecalLayerMask"
        Gbx_DecalLayerMask.Size = New Size(187, 161)
        Gbx_DecalLayerMask.TabIndex = 4
        Gbx_DecalLayerMask.TabStop = False
        Gbx_DecalLayerMask.Text = "Decal Layer Mask (?)"
        ToolTipAE.SetToolTip(Gbx_DecalLayerMask, resources.GetString("Gbx_DecalLayerMask.ToolTip"))
        ' 
        ' Lbl_DlmValue
        ' 
        Lbl_DlmValue.BorderStyle = BorderStyle.FixedSingle
        Lbl_DlmValue.Location = New Point(124, 35)
        Lbl_DlmValue.Name = "Lbl_DlmValue"
        Lbl_DlmValue.Size = New Size(38, 23)
        Lbl_DlmValue.TabIndex = 7
        Lbl_DlmValue.Text = "-"
        Lbl_DlmValue.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Font = New Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, CByte(0))
        Label5.Location = New Point(105, 17)
        Label5.Name = "Label5"
        Label5.Size = New Size(78, 15)
        Label5.TabIndex = 6
        Label5.Text = "Current Value"
        ' 
        ' Chk_DlmCreatures
        ' 
        Chk_DlmCreatures.AutoSize = True
        Chk_DlmCreatures.Location = New Point(10, 135)
        Chk_DlmCreatures.Name = "Chk_DlmCreatures"
        Chk_DlmCreatures.Size = New Size(76, 19)
        Chk_DlmCreatures.TabIndex = 5
        Chk_DlmCreatures.Text = "Creatures"
        Chk_DlmCreatures.UseVisualStyleBackColor = True
        ' 
        ' Chk_DlmProps
        ' 
        Chk_DlmProps.AutoSize = True
        Chk_DlmProps.Location = New Point(10, 111)
        Chk_DlmProps.Name = "Chk_DlmProps"
        Chk_DlmProps.Size = New Size(56, 19)
        Chk_DlmProps.TabIndex = 4
        Chk_DlmProps.Text = "Props"
        Chk_DlmProps.UseVisualStyleBackColor = True
        ' 
        ' Chk_DlmVehicles
        ' 
        Chk_DlmVehicles.AutoSize = True
        Chk_DlmVehicles.Location = New Point(10, 87)
        Chk_DlmVehicles.Name = "Chk_DlmVehicles"
        Chk_DlmVehicles.Size = New Size(68, 19)
        Chk_DlmVehicles.TabIndex = 3
        Chk_DlmVehicles.Text = "Vehicles"
        Chk_DlmVehicles.UseVisualStyleBackColor = True
        ' 
        ' Chk_DlmBuildings
        ' 
        Chk_DlmBuildings.AutoSize = True
        Chk_DlmBuildings.Location = New Point(10, 63)
        Chk_DlmBuildings.Name = "Chk_DlmBuildings"
        Chk_DlmBuildings.Size = New Size(75, 19)
        Chk_DlmBuildings.TabIndex = 2
        Chk_DlmBuildings.Text = "Buildings"
        Chk_DlmBuildings.UseVisualStyleBackColor = True
        ' 
        ' Chk_DlmRoads
        ' 
        Chk_DlmRoads.AutoSize = True
        Chk_DlmRoads.Location = New Point(10, 39)
        Chk_DlmRoads.Name = "Chk_DlmRoads"
        Chk_DlmRoads.Size = New Size(58, 19)
        Chk_DlmRoads.TabIndex = 1
        Chk_DlmRoads.Text = "Roads"
        Chk_DlmRoads.UseVisualStyleBackColor = True
        ' 
        ' Chk_DlmGround
        ' 
        Chk_DlmGround.AutoSize = True
        Chk_DlmGround.Location = New Point(10, 15)
        Chk_DlmGround.Name = "Chk_DlmGround"
        Chk_DlmGround.Size = New Size(66, 19)
        Chk_DlmGround.TabIndex = 0
        Chk_DlmGround.Text = "Ground"
        Chk_DlmGround.UseVisualStyleBackColor = True
        ' 
        ' Txt_AssetName
        ' 
        Txt_AssetName.Location = New Point(233, 9)
        Txt_AssetName.MaxLength = 128
        Txt_AssetName.Name = "Txt_AssetName"
        Txt_AssetName.Size = New Size(459, 23)
        Txt_AssetName.TabIndex = 5
        Txt_AssetName.TabStop = False
        ToolTipAE.SetToolTip(Txt_AssetName, "Asset Name:" & vbCrLf & vbCrLf & "This is the name of the asset displayed here and EAI in game menu." & vbCrLf & vbCrLf & "Only letters, numbers, hyphens (-), underscores (_), and spaces are allowed.")
        ' 
        ' Btn_Cancel
        ' 
        Btn_Cancel.Location = New Point(617, 406)
        Btn_Cancel.Name = "Btn_Cancel"
        Btn_Cancel.Size = New Size(75, 23)
        Btn_Cancel.TabIndex = 8
        Btn_Cancel.Text = "Cancel"
        Btn_Cancel.UseVisualStyleBackColor = True
        ' 
        ' Btn_Modify
        ' 
        Btn_Modify.Location = New Point(536, 406)
        Btn_Modify.Name = "Btn_Modify"
        Btn_Modify.Size = New Size(75, 23)
        Btn_Modify.TabIndex = 9
        Btn_Modify.Text = "Save"
        Btn_Modify.UseVisualStyleBackColor = True
        ' 
        ' Gbx_Advanced
        ' 
        Gbx_Advanced.Controls.Add(Txt_MetallicOpacity)
        Gbx_Advanced.Controls.Add(Lbl_MetallicOpacity)
        Gbx_Advanced.Controls.Add(Txt_Smoothness)
        Gbx_Advanced.Controls.Add(Txt_Metallic)
        Gbx_Advanced.Controls.Add(Lbl_Smoothness)
        Gbx_Advanced.Controls.Add(Lbl_Metallic)
        Gbx_Advanced.Location = New Point(146, 265)
        Gbx_Advanced.Name = "Gbx_Advanced"
        Gbx_Advanced.Size = New Size(187, 150)
        Gbx_Advanced.TabIndex = 10
        Gbx_Advanced.TabStop = False
        Gbx_Advanced.Text = "Material Properties (?)"
        ToolTipAE.SetToolTip(Gbx_Advanced, resources.GetString("Gbx_Advanced.ToolTip"))
        ' 
        ' Txt_MetallicOpacity
        ' 
        Txt_MetallicOpacity.Location = New Point(108, 77)
        Txt_MetallicOpacity.MaxLength = 4
        Txt_MetallicOpacity.Name = "Txt_MetallicOpacity"
        Txt_MetallicOpacity.Size = New Size(73, 23)
        Txt_MetallicOpacity.TabIndex = 7
        Txt_MetallicOpacity.TabStop = False
        ' 
        ' Lbl_MetallicOpacity
        ' 
        Lbl_MetallicOpacity.AutoSize = True
        Lbl_MetallicOpacity.Location = New Point(6, 80)
        Lbl_MetallicOpacity.Name = "Lbl_MetallicOpacity"
        Lbl_MetallicOpacity.Size = New Size(96, 15)
        Lbl_MetallicOpacity.TabIndex = 6
        Lbl_MetallicOpacity.Text = "Metallic Opacity:"
        ' 
        ' Txt_Smoothness
        ' 
        Txt_Smoothness.Location = New Point(108, 48)
        Txt_Smoothness.MaxLength = 4
        Txt_Smoothness.Name = "Txt_Smoothness"
        Txt_Smoothness.Size = New Size(73, 23)
        Txt_Smoothness.TabIndex = 5
        Txt_Smoothness.TabStop = False
        ' 
        ' Txt_Metallic
        ' 
        Txt_Metallic.Location = New Point(108, 19)
        Txt_Metallic.MaxLength = 4
        Txt_Metallic.Name = "Txt_Metallic"
        Txt_Metallic.Size = New Size(73, 23)
        Txt_Metallic.TabIndex = 4
        Txt_Metallic.TabStop = False
        ' 
        ' Lbl_Smoothness
        ' 
        Lbl_Smoothness.AutoSize = True
        Lbl_Smoothness.Location = New Point(6, 51)
        Lbl_Smoothness.Name = "Lbl_Smoothness"
        Lbl_Smoothness.Size = New Size(75, 15)
        Lbl_Smoothness.TabIndex = 1
        Lbl_Smoothness.Text = "Smoothness:"
        ' 
        ' Lbl_Metallic
        ' 
        Lbl_Metallic.AutoSize = True
        Lbl_Metallic.Location = New Point(6, 22)
        Lbl_Metallic.Name = "Lbl_Metallic"
        Lbl_Metallic.Size = New Size(52, 15)
        Lbl_Metallic.TabIndex = 0
        Lbl_Metallic.Text = "Metallic:"
        ' 
        ' Gbx_DecalSize
        ' 
        Gbx_DecalSize.Controls.Add(Chk_KeepAspectRatio)
        Gbx_DecalSize.Controls.Add(Lbl_LockStatus)
        Gbx_DecalSize.Controls.Add(Txt_Z)
        Gbx_DecalSize.Controls.Add(Txt_Y)
        Gbx_DecalSize.Controls.Add(Txt_X)
        Gbx_DecalSize.Controls.Add(Lbl_Length)
        Gbx_DecalSize.Controls.Add(Lbl_Thickness)
        Gbx_DecalSize.Controls.Add(Lbl_Width)
        Gbx_DecalSize.Location = New Point(339, 101)
        Gbx_DecalSize.Name = "Gbx_DecalSize"
        Gbx_DecalSize.Size = New Size(165, 161)
        Gbx_DecalSize.TabIndex = 11
        Gbx_DecalSize.TabStop = False
        Gbx_DecalSize.Text = "Decal Size (?)"
        ToolTipAE.SetToolTip(Gbx_DecalSize, resources.GetString("Gbx_DecalSize.ToolTip"))
        ' 
        ' Chk_KeepAspectRatio
        ' 
        Chk_KeepAspectRatio.AutoSize = True
        Chk_KeepAspectRatio.Location = New Point(10, 114)
        Chk_KeepAspectRatio.Name = "Chk_KeepAspectRatio"
        Chk_KeepAspectRatio.Size = New Size(145, 19)
        Chk_KeepAspectRatio.TabIndex = 8
        Chk_KeepAspectRatio.Text = "Lock Aspect Ratio (x;z)"
        ToolTipAE.SetToolTip(Chk_KeepAspectRatio, "This option will lock the aspect ratio to prevent the" & vbCrLf & "decal to be deformed when size is changed." & vbCrLf & vbCrLf & "Do not affect Height (y) value.")
        Chk_KeepAspectRatio.UseVisualStyleBackColor = True
        ' 
        ' Lbl_LockStatus
        ' 
        Lbl_LockStatus.AutoSize = True
        Lbl_LockStatus.BackColor = SystemColors.Control
        Lbl_LockStatus.Location = New Point(134, 14)
        Lbl_LockStatus.Name = "Lbl_LockStatus"
        Lbl_LockStatus.Size = New Size(28, 90)
        Lbl_LockStatus.TabIndex = 7
        Lbl_LockStatus.Text = "—┐" & vbCrLf & "    │" & vbCrLf & "   —" & vbCrLf & "   —" & vbCrLf & "    │" & vbCrLf & "—┘"
        ' 
        ' Txt_Z
        ' 
        Txt_Z.Location = New Point(68, 80)
        Txt_Z.MaxLength = 8
        Txt_Z.Name = "Txt_Z"
        Txt_Z.Size = New Size(60, 23)
        Txt_Z.TabIndex = 5
        Txt_Z.TabStop = False
        ' 
        ' Txt_Y
        ' 
        Txt_Y.Location = New Point(68, 49)
        Txt_Y.MaxLength = 8
        Txt_Y.Name = "Txt_Y"
        Txt_Y.Size = New Size(60, 23)
        Txt_Y.TabIndex = 4
        Txt_Y.TabStop = False
        ToolTipAE.SetToolTip(Txt_Y, "This value represent the ""thickness"" of the decal." & vbCrLf & vbCrLf & "Smaller values will prevent the decal for bleeding" & vbCrLf & "to other objects." & vbCrLf & vbCrLf & "Bigger values will work better over irregular shapes" & vbCrLf & "or terrain.")
        ' 
        ' Txt_X
        ' 
        Txt_X.Location = New Point(68, 18)
        Txt_X.MaxLength = 8
        Txt_X.Name = "Txt_X"
        Txt_X.Size = New Size(60, 23)
        Txt_X.TabIndex = 3
        Txt_X.TabStop = False
        ' 
        ' Lbl_Length
        ' 
        Lbl_Length.AutoSize = True
        Lbl_Length.Location = New Point(6, 83)
        Lbl_Length.Name = "Lbl_Length"
        Lbl_Length.Size = New Size(63, 15)
        Lbl_Length.TabIndex = 2
        Lbl_Length.Text = "Length (z):"
        ' 
        ' Lbl_Thickness
        ' 
        Lbl_Thickness.AutoSize = True
        Lbl_Thickness.Location = New Point(6, 52)
        Lbl_Thickness.Name = "Lbl_Thickness"
        Lbl_Thickness.Size = New Size(63, 15)
        Lbl_Thickness.TabIndex = 1
        Lbl_Thickness.Text = "Height (y):"
        ' 
        ' Lbl_Width
        ' 
        Lbl_Width.AutoSize = True
        Lbl_Width.Location = New Point(6, 21)
        Lbl_Width.Name = "Lbl_Width"
        Lbl_Width.Size = New Size(59, 15)
        Lbl_Width.TabIndex = 0
        Lbl_Width.Text = "Width (x):"
        ' 
        ' Lbl_AssetType
        ' 
        Lbl_AssetType.AutoSize = True
        Lbl_AssetType.Location = New Point(12, 153)
        Lbl_AssetType.Name = "Lbl_AssetType"
        Lbl_AssetType.Size = New Size(65, 15)
        Lbl_AssetType.TabIndex = 12
        Lbl_AssetType.Text = "Asset Type:"
        ' 
        ' Lbl_AssetTypeName
        ' 
        Lbl_AssetTypeName.BorderStyle = BorderStyle.FixedSingle
        Lbl_AssetTypeName.FlatStyle = FlatStyle.Flat
        Lbl_AssetTypeName.Location = New Point(12, 175)
        Lbl_AssetTypeName.Name = "Lbl_AssetTypeName"
        Lbl_AssetTypeName.Size = New Size(125, 23)
        Lbl_AssetTypeName.TabIndex = 13
        Lbl_AssetTypeName.Text = "-"
        Lbl_AssetTypeName.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Lbl_AssetCat
        ' 
        Lbl_AssetCat.AutoSize = True
        Lbl_AssetCat.Location = New Point(12, 210)
        Lbl_AssetCat.Name = "Lbl_AssetCat"
        Lbl_AssetCat.Size = New Size(89, 15)
        Lbl_AssetCat.TabIndex = 14
        Lbl_AssetCat.Text = "Asset Category:"
        ' 
        ' Lbl_AssetCatName
        ' 
        Lbl_AssetCatName.BorderStyle = BorderStyle.FixedSingle
        Lbl_AssetCatName.FlatStyle = FlatStyle.Flat
        Lbl_AssetCatName.Location = New Point(12, 225)
        Lbl_AssetCatName.Name = "Lbl_AssetCatName"
        Lbl_AssetCatName.Size = New Size(125, 23)
        Lbl_AssetCatName.TabIndex = 15
        Lbl_AssetCatName.Text = "-"
        Lbl_AssetCatName.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Gbx_SurfacesOnly
        ' 
        Gbx_SurfacesOnly.Controls.Add(Txt_MeshSize)
        Gbx_SurfacesOnly.Controls.Add(Lbl_MeshSizeZ)
        Gbx_SurfacesOnly.Controls.Add(Chk_CalcUV)
        Gbx_SurfacesOnly.Controls.Add(Lbl_MeshSizeM)
        Gbx_SurfacesOnly.Controls.Add(Txt_colossal_EdgeNormal)
        Gbx_SurfacesOnly.Controls.Add(Txt_colossal_UVScale)
        Gbx_SurfacesOnly.Controls.Add(Lbl_EdgeNormal)
        Gbx_SurfacesOnly.Controls.Add(Txt_Roundness)
        Gbx_SurfacesOnly.Controls.Add(Label4)
        Gbx_SurfacesOnly.Controls.Add(Lbl_Roundness)
        Gbx_SurfacesOnly.Location = New Point(510, 101)
        Gbx_SurfacesOnly.Name = "Gbx_SurfacesOnly"
        Gbx_SurfacesOnly.Size = New Size(182, 161)
        Gbx_SurfacesOnly.TabIndex = 16
        Gbx_SurfacesOnly.TabStop = False
        Gbx_SurfacesOnly.Text = "Surfaces Properties (?)"
        ToolTipAE.SetToolTip(Gbx_SurfacesOnly, resources.GetString("Gbx_SurfacesOnly.ToolTip"))
        ' 
        ' Txt_MeshSize
        ' 
        Txt_MeshSize.BorderStyle = BorderStyle.None
        Txt_MeshSize.Location = New Point(71, 109)
        Txt_MeshSize.MaxLength = 8
        Txt_MeshSize.Name = "Txt_MeshSize"
        Txt_MeshSize.ReadOnly = True
        Txt_MeshSize.Size = New Size(52, 16)
        Txt_MeshSize.TabIndex = 9
        Txt_MeshSize.Text = "1.000000"
        ToolTipAE.SetToolTip(Txt_MeshSize, "Minimun value allowed is 0.5")
        ' 
        ' Lbl_MeshSizeZ
        ' 
        Lbl_MeshSizeZ.AutoSize = True
        Lbl_MeshSizeZ.Location = New Point(120, 109)
        Lbl_MeshSizeZ.Name = "Lbl_MeshSizeZ"
        Lbl_MeshSizeZ.Size = New Size(61, 15)
        Lbl_MeshSizeZ.TabIndex = 8
        Lbl_MeshSizeZ.Text = "x 1.000000"
        ' 
        ' Chk_CalcUV
        ' 
        Chk_CalcUV.AutoSize = True
        Chk_CalcUV.Location = New Point(10, 138)
        Chk_CalcUV.Name = "Chk_CalcUV"
        Chk_CalcUV.Size = New Size(120, 19)
        Chk_CalcUV.TabIndex = 7
        Chk_CalcUV.Text = "Calculate UVScale"
        ToolTipAE.SetToolTip(Chk_CalcUV, "Calculate UV Scale:" & vbCrLf & vbCrLf & "This will help you to calculate the UV Scale from" & vbCrLf & "the side of the mesh. For example a 2x2 square" & vbCrLf & "will have 2 meter side mesh, (1/2)=0.5" & vbCrLf & "The calculated UV Scale will be 0.5")
        Chk_CalcUV.UseVisualStyleBackColor = True
        ' 
        ' Lbl_MeshSizeM
        ' 
        Lbl_MeshSizeM.AutoSize = True
        Lbl_MeshSizeM.Location = New Point(6, 109)
        Lbl_MeshSizeM.Name = "Lbl_MeshSizeM"
        Lbl_MeshSizeM.Size = New Size(57, 15)
        Lbl_MeshSizeM.TabIndex = 6
        Lbl_MeshSizeM.Text = "Area Size:"
        ' 
        ' Txt_colossal_EdgeNormal
        ' 
        Txt_colossal_EdgeNormal.Location = New Point(93, 48)
        Txt_colossal_EdgeNormal.MaxLength = 4
        Txt_colossal_EdgeNormal.Name = "Txt_colossal_EdgeNormal"
        Txt_colossal_EdgeNormal.Size = New Size(72, 23)
        Txt_colossal_EdgeNormal.TabIndex = 5
        Txt_colossal_EdgeNormal.TabStop = False
        ToolTipAE.SetToolTip(Txt_colossal_EdgeNormal, "Allowed values from 0 to 1.")
        ' 
        ' Txt_colossal_UVScale
        ' 
        Txt_colossal_UVScale.Location = New Point(93, 77)
        Txt_colossal_UVScale.MaxLength = 8
        Txt_colossal_UVScale.Name = "Txt_colossal_UVScale"
        Txt_colossal_UVScale.Size = New Size(73, 23)
        Txt_colossal_UVScale.TabIndex = 4
        Txt_colossal_UVScale.TabStop = False
        ToolTipAE.SetToolTip(Txt_colossal_UVScale, "Allowed values from 2 to 0.000001." & vbCrLf & vbCrLf & "Not recomended to use values under 0.01")
        ' 
        ' Lbl_EdgeNormal
        ' 
        Lbl_EdgeNormal.AutoSize = True
        Lbl_EdgeNormal.Location = New Point(6, 51)
        Lbl_EdgeNormal.Name = "Lbl_EdgeNormal"
        Lbl_EdgeNormal.Size = New Size(79, 15)
        Lbl_EdgeNormal.TabIndex = 3
        Lbl_EdgeNormal.Text = "Edge Normal:"
        ' 
        ' Txt_Roundness
        ' 
        Txt_Roundness.Location = New Point(93, 19)
        Txt_Roundness.MaxLength = 4
        Txt_Roundness.Name = "Txt_Roundness"
        Txt_Roundness.Size = New Size(72, 23)
        Txt_Roundness.TabIndex = 2
        Txt_Roundness.TabStop = False
        ToolTipAE.SetToolTip(Txt_Roundness, "Allowed values from 0 to 1.")
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(6, 80)
        Label4.Name = "Label4"
        Label4.Size = New Size(55, 15)
        Label4.TabIndex = 1
        Label4.Text = "UV Scale:"
        ' 
        ' Lbl_Roundness
        ' 
        Lbl_Roundness.AutoSize = True
        Lbl_Roundness.Location = New Point(6, 22)
        Lbl_Roundness.Name = "Lbl_Roundness"
        Lbl_Roundness.Size = New Size(68, 15)
        Lbl_Roundness.TabIndex = 0
        Lbl_Roundness.Text = "Roundness:"
        ' 
        ' ToolTipAE
        ' 
        ToolTipAE.AutoPopDelay = 5000
        ToolTipAE.InitialDelay = 100
        ToolTipAE.ReshowDelay = 100
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(Txt_NormalOpacity)
        GroupBox1.Controls.Add(Lbl_NormalOpacity)
        GroupBox1.Location = New Point(339, 265)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(165, 150)
        GroupBox1.TabIndex = 17
        GroupBox1.TabStop = False
        GroupBox1.Text = "Normal Map Properties (?)"
        ToolTipAE.SetToolTip(GroupBox1, resources.GetString("GroupBox1.ToolTip"))
        ' 
        ' Txt_NormalOpacity
        ' 
        Txt_NormalOpacity.Location = New Point(106, 19)
        Txt_NormalOpacity.MaxLength = 4
        Txt_NormalOpacity.Name = "Txt_NormalOpacity"
        Txt_NormalOpacity.Size = New Size(53, 23)
        Txt_NormalOpacity.TabIndex = 11
        Txt_NormalOpacity.TabStop = False
        ' 
        ' Lbl_NormalOpacity
        ' 
        Lbl_NormalOpacity.AutoSize = True
        Lbl_NormalOpacity.Location = New Point(6, 22)
        Lbl_NormalOpacity.Name = "Lbl_NormalOpacity"
        Lbl_NormalOpacity.Size = New Size(94, 15)
        Lbl_NormalOpacity.TabIndex = 10
        Lbl_NormalOpacity.Text = "Normal Opacity:"
        ' 
        ' Nud_UiPriority
        ' 
        Nud_UiPriority.Location = New Point(233, 38)
        Nud_UiPriority.Maximum = New Decimal(New Integer() {99999999, 0, 0, 0})
        Nud_UiPriority.Name = "Nud_UiPriority"
        Nud_UiPriority.Size = New Size(100, 23)
        Nud_UiPriority.TabIndex = 19
        Nud_UiPriority.TabStop = False
        ToolTipAE.SetToolTip(Nud_UiPriority, resources.GetString("Nud_UiPriority.ToolTip"))
        ' 
        ' Nud_DrawOrder
        ' 
        Nud_DrawOrder.Location = New Point(233, 67)
        Nud_DrawOrder.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Nud_DrawOrder.Minimum = New Decimal(New Integer() {170, 0, 0, Integer.MinValue})
        Nud_DrawOrder.Name = "Nud_DrawOrder"
        Nud_DrawOrder.Size = New Size(100, 23)
        Nud_DrawOrder.TabIndex = 18
        Nud_DrawOrder.TabStop = False
        ' 
        ' Frm_AssetEditor
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(704, 441)
        Controls.Add(Nud_UiPriority)
        Controls.Add(Nud_DrawOrder)
        Controls.Add(GroupBox1)
        Controls.Add(Gbx_SurfacesOnly)
        Controls.Add(Lbl_AssetCatName)
        Controls.Add(Lbl_AssetCat)
        Controls.Add(Lbl_AssetTypeName)
        Controls.Add(Lbl_AssetType)
        Controls.Add(Gbx_DecalSize)
        Controls.Add(Gbx_Advanced)
        Controls.Add(Btn_Modify)
        Controls.Add(Btn_Cancel)
        Controls.Add(Txt_AssetName)
        Controls.Add(Gbx_DecalLayerMask)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Pbx_Icon)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        MaximumSize = New Size(720, 480)
        MinimizeBox = False
        MinimumSize = New Size(720, 480)
        Name = "Frm_AssetEditor"
        ShowInTaskbar = False
        Text = "Custom Asset Editor"
        CType(Pbx_Icon, ComponentModel.ISupportInitialize).EndInit()
        Gbx_DecalLayerMask.ResumeLayout(False)
        Gbx_DecalLayerMask.PerformLayout()
        Gbx_Advanced.ResumeLayout(False)
        Gbx_Advanced.PerformLayout()
        Gbx_DecalSize.ResumeLayout(False)
        Gbx_DecalSize.PerformLayout()
        Gbx_SurfacesOnly.ResumeLayout(False)
        Gbx_SurfacesOnly.PerformLayout()
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        CType(Nud_UiPriority, ComponentModel.ISupportInitialize).EndInit()
        CType(Nud_DrawOrder, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Pbx_Icon As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Gbx_DecalLayerMask As GroupBox
    Friend WithEvents Chk_DlmProps As CheckBox
    Friend WithEvents Chk_DlmVehicles As CheckBox
    Friend WithEvents Chk_DlmBuildings As CheckBox
    Friend WithEvents Chk_DlmRoads As CheckBox
    Friend WithEvents Chk_DlmGround As CheckBox
    Friend WithEvents Txt_AssetName As TextBox
    Friend WithEvents Btn_Cancel As Button
    Friend WithEvents Btn_Modify As Button
    Friend WithEvents Gbx_Advanced As GroupBox
    Friend WithEvents Lbl_Smoothness As Label
    Friend WithEvents Lbl_Metallic As Label
    Friend WithEvents Txt_Smoothness As TextBox
    Friend WithEvents Txt_Metallic As TextBox
    Friend WithEvents Gbx_DecalSize As GroupBox
    Friend WithEvents Txt_Z As TextBox
    Friend WithEvents Txt_Y As TextBox
    Friend WithEvents Txt_X As TextBox
    Friend WithEvents Lbl_Length As Label
    Friend WithEvents Lbl_Thickness As Label
    Friend WithEvents Lbl_Width As Label
    Friend WithEvents Lbl_AssetType As Label
    Friend WithEvents Lbl_AssetTypeName As Label
    Friend WithEvents Lbl_AssetCat As Label
    Friend WithEvents Lbl_AssetCatName As Label
    Friend WithEvents Gbx_SurfacesOnly As GroupBox
    Friend WithEvents Lbl_Roundness As Label
    Friend WithEvents Txt_Roundness As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Lbl_EdgeNormal As Label
    Friend WithEvents Txt_colossal_EdgeNormal As TextBox
    Friend WithEvents Txt_colossal_UVScale As TextBox
    Friend WithEvents Txt_MetallicOpacity As TextBox
    Friend WithEvents Lbl_MetallicOpacity As Label
    Friend WithEvents Chk_DlmCreatures As CheckBox
    Friend WithEvents Lbl_DlmValue As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Lbl_LockStatus As Label
    Friend WithEvents Chk_KeepAspectRatio As CheckBox
    Friend WithEvents ToolTipAE As ToolTip
    Friend WithEvents Chk_CalcUV As CheckBox
    Friend WithEvents Lbl_MeshSizeM As Label
    Friend WithEvents Txt_MeshSize As TextBox
    Friend WithEvents Lbl_MeshSizeZ As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Txt_NormalOpacity As TextBox
    Friend WithEvents Lbl_NormalOpacity As Label
    Friend WithEvents Nud_DrawOrder As NumericUpDown
    Friend WithEvents Nud_UiPriority As NumericUpDown
End Class
