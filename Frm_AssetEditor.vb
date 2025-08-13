Imports System.ComponentModel ' Required for BindingFlags
Imports System.Drawing ' For Color
Imports System.Globalization ' For CultureInfo.InvariantCulture
Imports System.IO
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class Frm_AssetEditor

    ' Public properties to hold the data passed from Frm_Main (via EditorMod)
    Public Property AssetFullPath As String
    Public Property AssetName As String
    Public Property AssetIcon As Image
    Public Property AssetType As String
    Public Property AssetCategory As String

    ' Variable to store the original JSON content of the asset
    Private _assetJsonContent As JObject
    Private _netlaneJsonContent As JObject ' For netlane.json if applicable

    ' Lbl_DlmValue is a control on the form and does not need to be declared here.

    ' Constant for the minimum allowed value (0.000001) for internal calculations
    ' NOTE: If the user enters 0 in Txt_X, Txt_Y, Txt_Z, the value will be corrected to 1.0 for the UI.
    Private Const MIN_DECIMAL_VALUE_FOR_CALCULATION As Double = 0.000001

    'Keep Aspect Ratio related variables
    Private KeepRatio As Boolean = False
    Private SavedRatio As Double = 1
    Private isUpdating As Boolean = False

    'Added for decal scale calculation v1.4.2
    Private baseX As Double = 1
    Private ignoreDecalEvent As Boolean = False

    Private Sub Frm_AssetEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set the form title
        Me.Text = "Editing: " & AssetName

        ' Populate basic controls with loaded data
        If AssetIcon IsNot Nothing Then
            Pbx_Icon.Image = AssetIcon
        End If

        Txt_AssetName.Text = AssetName
        Lbl_AssetTypeName.Text = AssetType
        Lbl_AssetCatName.Text = AssetCategory

        ' Set button icons
        Btn_Modify.Image = imageDictionary("QSave")
        Btn_Cancel.Image = imageDictionary("Cancel")

        ' Load and display JSON data
        LoadAssetJsonData()

        ' Disable Btn_Modify by default
        Btn_Modify.Enabled = False

        'new txt decal scale v1.4.2
        Txt_DecalScale.ShortcutsEnabled = False
        Txt_DecalScale.Text = "100"
        Txt_DecalScale.Enabled = False
        Lbl_DecalScale.Enabled = False
        Lbl_Percent.Enabled = False

        ' Attach TextChanged handlers to reset text color to black and enable Btn_Modify
        ' Txt_AssetName now has its own specific handler below
        'AddHandler Txt_UiPriority.TextChanged, AddressOf TextBox_TextChanged_And_EnableModify // Changed to Nud_UiPriority
        AddHandler Txt_Metallic.TextChanged, AddressOf TextBox_TextChanged_And_EnableModify
        AddHandler Txt_Smoothness.TextChanged, AddressOf TextBox_TextChanged_And_EnableModify
        AddHandler Txt_MetallicOpacity.TextChanged, AddressOf TextBox_TextChanged_And_EnableModify
        AddHandler Txt_NormalOpacity.TextChanged, AddressOf TextBox_TextChanged_And_EnableModify
        'AddHandler Txt_DrawOrder.TextChanged, AddressOf TextBox_TextChanged_And_EnableModify // Changed to Nud_DrawOrder
        AddHandler Txt_Roundness.TextChanged, AddressOf TextBox_TextChanged_And_EnableModify
        AddHandler Txt_colossal_UVScale.TextChanged, AddressOf TextBox_TextChanged_And_EnableModify
        AddHandler Txt_colossal_EdgeNormal.TextChanged, AddressOf TextBox_TextChanged_And_EnableModify
        AddHandler Txt_X.TextChanged, AddressOf TextBox_TextChanged_And_EnableModify
        AddHandler Txt_Y.TextChanged, AddressOf TextBox_TextChanged_And_EnableModify
        AddHandler Txt_Z.TextChanged, AddressOf TextBox_TextChanged_And_EnableModify

        AddHandler Nud_UiPriority.TextChanged, AddressOf NumericUpDown_TextChanged_And_EnableModify
        AddHandler Nud_DrawOrder.TextChanged, AddressOf NumericUpDown_TextChanged_And_EnableModify


        ' Attach KeyPress handlers for input validation
        AddHandler Txt_AssetName.KeyPress, AddressOf HandleAssetNameKeyPress
        'AddHandler Txt_UiPriority.KeyPress, AddressOf HandleIntegerKeyPress // Changed to Nud_UiPriority
        'AddHandler Txt_DrawOrder.KeyPress, AddressOf HandleSignedIntegerKeyPress // Changed to Nud_DrawOrder
        AddHandler Txt_Metallic.KeyPress, AddressOf HandleDecimalKeyPress
        AddHandler Txt_Smoothness.KeyPress, AddressOf HandleDecimalKeyPress
        AddHandler Txt_MetallicOpacity.KeyPress, AddressOf HandleDecimalKeyPress
        AddHandler Txt_NormalOpacity.KeyPress, AddressOf HandleDecimalKeyPress
        AddHandler Txt_Roundness.KeyPress, AddressOf HandleDecimalKeyPress
        AddHandler Txt_colossal_UVScale.KeyPress, AddressOf HandleDecimalKeyPress
        AddHandler Txt_colossal_EdgeNormal.KeyPress, AddressOf HandleDecimalKeyPress
        AddHandler Txt_X.KeyPress, AddressOf HandleDecimalKeyPress
        AddHandler Txt_Y.KeyPress, AddressOf HandleDecimalKeyPress
        AddHandler Txt_Z.KeyPress, AddressOf HandleDecimalKeyPress
        AddHandler Txt_MeshSize.KeyPress, AddressOf HandleDecimalKeyPress
        AddHandler Txt_DecalScale.KeyPress, AddressOf HandleDecimalKeyPress 'new in v1.4.2

        ' Attach KeyPress handler for Nud_DrawOrder
        AddHandler Nud_DrawOrder.KeyPress, AddressOf HandleNUDSignedIntegerKeyPress ' Allows hyphen for negatives
        AddHandler Nud_UiPriority.KeyPress, AddressOf HandleIntegerKeyPress ' Allows only positive integers

        ' Attach LostFocus handlers for range and zero value validation
        AddHandler Txt_Metallic.LostFocus, AddressOf HandleDecimalRangeLostFocus
        AddHandler Txt_Smoothness.LostFocus, AddressOf HandleDecimalRangeLostFocus
        AddHandler Txt_MetallicOpacity.LostFocus, AddressOf HandleDecimalRangeLostFocus
        AddHandler Txt_NormalOpacity.LostFocus, AddressOf HandleDecimalRangeLostFocus
        AddHandler Txt_Roundness.LostFocus, AddressOf HandleDecimalRangeLostFocus
        AddHandler Txt_colossal_EdgeNormal.LostFocus, AddressOf HandleDecimalRangeLostFocus

        AddHandler Txt_X.LostFocus, AddressOf HandleDimensionValueLostFocus
        AddHandler Txt_Y.LostFocus, AddressOf HandleDimensionValueLostFocus
        AddHandler Txt_Z.LostFocus, AddressOf HandleDimensionValueLostFocus


        ' Attach LostFocus handlers for UiPriority and DrawOrder
        'AddHandler Txt_UiPriority.LostFocus, AddressOf HandleUiPriorityLostFocus // Changed to nud_UiPriority
        'AddHandler Txt_DrawOrder.LostFocus, AddressOf HandleDrawOrderLostFocus // Changed to nud_DrawOrder
        AddHandler Nud_DrawOrder.LostFocus, AddressOf HandleNUDDrawOrderLostFocus
        AddHandler Nud_UiPriority.LostFocus, AddressOf HandleNUDUiPriorityLostFocus

        'Handle to prevent pasting in NumericUpDown controls
        AddHandler Nud_DrawOrder.KeyDown, AddressOf BlockCtrlV
        AddHandler Nud_UiPriority.KeyDown, AddressOf BlockCtrlV

        ' Handle right-click context menu for textboxes and disable shortcuts
        Dim txtboxes As TextBox() = {Txt_AssetName, Txt_Metallic, Txt_Smoothness, Txt_MetallicOpacity,
                                    Txt_NormalOpacity, Txt_Roundness, Txt_colossal_UVScale, Txt_colossal_EdgeNormal, Txt_X, Txt_Y, Txt_Z,
                                    Txt_MeshSize, Txt_DecalScale}

        For Each txtbox In txtboxes
            AddHandler txtbox.MouseDown, AddressOf HandleRightClick ' Blocks right-click context menu
            txtbox.ShortcutsEnabled = False ' Blocks Ctrl+V, Ctrl+C, etc.
        Next

        ' Disable GroupBoxes that are not needed (but keep them visible)
        Gbx_SurfacesOnly.Enabled = (AssetType = "Surfaces")
        Gbx_DecalSize.Enabled = (AssetType = "Decals" OrElse AssetType = "Netlanes")

        ' Ensure at least one DecalLayerMask checkbox is selected on load
        ' This logic is now handled in SetDecalLayerMaskCheckboxes
        ' If Gbx_DecalLayerMask.Visible AndAlso CountCheckedDecalLayerMaskCheckboxes() = 0 Then
        '     Chk_DlmGround.Checked = True ' Activate the first one by default if none are active
        '     Gbx_DecalLayerMask.ForeColor = Color.Red ' Indicate it was a default/adjusted value
        ' End If

        'Lock status label initialization
        Lbl_LockStatus.Text = "—┐" & vbCrLf &
                              "   —" & vbCrLf &
                              "   —" & vbCrLf &
                              "   —" & vbCrLf &
                              "   —" & vbCrLf &
                              "—┘"

    End Sub

    ' Subroutine to enable the Modify button
    Private Sub EnableModifyButton()
        Btn_Modify.Enabled = True
    End Sub

    'Prevent pasting in NumericUpDown controls
    Private Sub BlockCtrlV(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True
        End If
    End Sub
    ' Specific handler for Txt_AssetName to enable modify button
    Private Sub Txt_AssetName_TextChanged(sender As Object, e As EventArgs) Handles Txt_AssetName.TextChanged
        DirectCast(sender, TextBox).ForeColor = Color.Black
        EnableModifyButton()
    End Sub

    ' Generic handler to reset text color to black and enable Modify button
    Private Sub TextBox_TextChanged_And_EnableModify(sender As Object, e As EventArgs)
        DirectCast(sender, TextBox).ForeColor = Color.Black
        EnableModifyButton()
    End Sub

    ' Generic handler to reset text color to black and enable Modify button from meric up-down controls also prevent UiPriority to exceed 8 digits
    Private Sub NumericUpDown_TextChanged_And_EnableModify(sender As Object, e As EventArgs)
        DirectCast(sender, NumericUpDown).ForeColor = Color.Black

        ' Prevent adding digits if the text reach 8 digits in the Nud_UiPriority control
        If DirectCast(sender, NumericUpDown).Name = "Nud_UiPriority" Then
            Dim innerTextBox As TextBox = TryCast(Nud_UiPriority.Controls.OfType(Of Control)().FirstOrDefault(Function(c) TypeOf c Is TextBox), TextBox)

            If innerTextBox IsNot Nothing Then
                AddHandler innerTextBox.TextChanged, Sub(tbSender, tbArgs)
                                                         Dim tb As TextBox = CType(tbSender, TextBox)

                                                         ' Run logic ONLY if more than 8 characters
                                                         If tb.Text.Length > 8 Then
                                                             ' Keep only the digits
                                                             Dim digitsOnly As String = New String(tb.Text.Where(AddressOf Char.IsDigit).ToArray())

                                                             ' Trim to first 8 digits
                                                             If digitsOnly.Length > 8 Then
                                                                 digitsOnly = digitsOnly.Substring(0, 8)
                                                             End If

                                                             tb.Text = digitsOnly
                                                             tb.SelectionStart = tb.Text.Length ' Keep cursor at the end
                                                         End If
                                                     End Sub
            End If
        End If

        EnableModifyButton()
    End Sub


    ' Generic handler to reset DecalLayerMask GroupBox color and enable Modify button
    Private Sub DecalLayerMaskCheckbox_CheckedChanged_And_EnableModify(sender As Object, e As EventArgs) Handles Chk_DlmGround.CheckedChanged, Chk_DlmRoads.CheckedChanged, Chk_DlmBuildings.CheckedChanged, Chk_DlmVehicles.CheckedChanged, Chk_DlmProps.CheckedChanged, Chk_DlmCreatures.CheckedChanged
        Gbx_DecalLayerMask.ForeColor = SystemColors.ControlText ' Reset to system default text color
        EnableModifyButton()

        Dim currentCheckbox As CheckBox = DirectCast(sender, CheckBox)

        ' If the user is trying to uncheck it
        If Not currentCheckbox.Checked Then
            ' Temporarily remove the handler to prevent re-triggering during programmatic change
            RemoveHandler currentCheckbox.CheckedChanged, AddressOf DecalLayerMaskCheckbox_CheckedChanged_And_EnableModify

            ' Calculate the potential new Decal Layer Mask value if this checkbox remains unchecked
            Dim potentialDlmValue As Integer = 0
            If Chk_DlmGround.Checked Then potentialDlmValue += 1
            If Chk_DlmRoads.Checked Then potentialDlmValue += 2
            If Chk_DlmBuildings.Checked Then potentialDlmValue += 4
            If Chk_DlmVehicles.Checked Then potentialDlmValue += 8
            If Chk_DlmCreatures.Checked Then potentialDlmValue += 16
            If Chk_DlmProps.Checked Then potentialDlmValue += 32

            ' If the potential value is 0, it means this was the last checked checkbox. Revert the change.
            If potentialDlmValue = 0 Then
                currentCheckbox.Checked = True ' Revert unchecking
                MessageBox.Show("At least one Decal Layer Mask must be selected.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            ' Re-add the handler
            AddHandler currentCheckbox.CheckedChanged, AddressOf DecalLayerMaskCheckbox_CheckedChanged_And_EnableModify
        End If

        UpdateDlmValueLabel() ' Update the label after any checkbox change
    End Sub

    ' Subroutine to calculate and update the Lbl_DlmValue text
    Private Sub UpdateDlmValueLabel()
        Dim currentDecalLayerMaskValue As Integer = 0
        If Chk_DlmGround.Checked Then currentDecalLayerMaskValue += 1
        If Chk_DlmRoads.Checked Then currentDecalLayerMaskValue += 2
        If Chk_DlmBuildings.Checked Then currentDecalLayerMaskValue += 4
        If Chk_DlmVehicles.Checked Then currentDecalLayerMaskValue += 8
        If Chk_DlmCreatures.Checked Then currentDecalLayerMaskValue += 16 ' Add Creatures value
        If Chk_DlmProps.Checked Then currentDecalLayerMaskValue += 32

        If Lbl_DlmValue IsNot Nothing Then ' Ensure the label exists before updating
            Lbl_DlmValue.Text = currentDecalLayerMaskValue.ToString()
        End If
    End Sub

    ' MouseDown handler for right click context menu
    Private Sub HandleRightClick(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Right Then
            Return
        End If
    End Sub

    ' KeyPress handler for Txt_AssetName (letters, numbers, -, _, space)
    Private Sub HandleAssetNameKeyPress(sender As Object, e As KeyPressEventArgs)
        ' Allow letters, digits, hyphen, underscore, space, and Backspace key
        If Not (Char.IsLetterOrDigit(e.KeyChar) OrElse
                e.KeyChar = "-"c OrElse
                e.KeyChar = "_"c OrElse
                e.KeyChar = " "c OrElse
                Char.IsControl(e.KeyChar)) Then
            e.Handled = True ' Ignore the character
        End If
    End Sub

    ' KeyPress handler for integer-only fields (0-9)
    Private Sub HandleIntegerKeyPress(sender As Object, e As KeyPressEventArgs)
        ' Allow digits and Backspace key
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Ignore the character
        End If
    End Sub

    ' KeyPress handler for signed integer fields (0-9, -)
    Private Sub HandleSignedIntegerKeyPress(sender As Object, e As KeyPressEventArgs)
        Dim textBox As TextBox = DirectCast(sender, TextBox)

        ' Allow digits and Backspace key
        If Char.IsDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) Then
            e.Handled = False
            Return
        End If

        ' Allow only one hyphen (-) and only at the beginning of the text or if the selection is at the beginning
        If e.KeyChar = "-"c Then
            If textBox.Text.Contains("-"c) OrElse textBox.SelectionStart <> 0 Then
                e.Handled = True ' Ignore if there's already a hyphen or it's not at the beginning
            Else
                e.Handled = False
                Return
            End If
        End If

        e.Handled = True ' Ignore any other character
    End Sub

    ' KeyPress handler for signed integer fields (0-9, -) in NumericUpDown controls
    Private Sub HandleNUDSignedIntegerKeyPress(sender As Object, e As KeyPressEventArgs)
        Dim nudControl As NumericUpDown = DirectCast(sender, NumericUpDown)

        ' Added to find the internal TextBox control of the NumericUpDown
        Dim internalTextBox As TextBox = TryCast(nudControl.Controls.OfType(Of Control)().FirstOrDefault(Function(c) TypeOf c Is TextBox), TextBox)

        If internalTextBox Is Nothing Then Return ' If we can't find the internal TextBox, exit the handler
        Dim cursorPos As Integer = internalTextBox.SelectionStart ' Get the current cursor position in the internal TextBox

        ' Allow digits and Backspace key
        If Char.IsDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) Then
            e.Handled = False
            Return
        End If

        ' Allow only one hyphen (-) and only at the beginning of the text or if the selection is at the beginning
        If e.KeyChar = "-"c Then
            If nudControl.Text.Contains("-"c) OrElse internalTextBox.SelectionStart <> 0 Then
                e.Handled = True ' Ignore if there's already a hyphen or it's not at the beginning
            Else
                e.Handled = False
                Return
            End If
        End If

        e.Handled = True ' Ignore any other character

    End Sub

    ' KeyPress handler for decimal fields (0-9, ., ,)
    Private Sub HandleDecimalKeyPress(sender As Object, e As KeyPressEventArgs)
        Dim textBox As TextBox = DirectCast(sender, TextBox)
        ' Define the desired decimal separator (always the dot)
        Dim desiredDecimalSeparator As Char = "."c ' Use the dot character directly

        ' Allow digits and backspace key
        If Char.IsDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) Then
            e.Handled = False
            Return
        End If

        ' Handle comma key to convert it to a dot
        If e.KeyChar = ","c Then ' If the pressed key is a comma
            ' If the textbox already contains a dot, ignore the comma
            If textBox.Text.Contains(desiredDecimalSeparator) Then
                e.Handled = True ' Ignore if there's already a dot
            Else
                ' If there is no dot, allow the comma (which will be inserted as a dot)
                e.KeyChar = desiredDecimalSeparator ' Change comma to dot
                e.Handled = False
                Return
            End If
        End If

        ' Handle dot key directly (if it's the desired separator)
        If e.KeyChar = desiredDecimalSeparator Then
            If textBox.Text.Contains(desiredDecimalSeparator) Then
                e.Handled = True ' Ignore if there's already a dot
            Else
                e.Handled = False
                Return
            End If
        End If

        e.Handled = True ' Ignore any other character
    End Sub

    ' LostFocus handler for Txt_Metallic, Txt_Smoothness, Txt_MetallicOpacity, Txt_NormalOpacity, Txt_Roundness, Txt_colossal_EdgeNormal
    Private Sub HandleDecimalRangeLostFocus(sender As Object, e As EventArgs) Handles Txt_Metallic.LostFocus, Txt_Smoothness.LostFocus, Txt_MetallicOpacity.LostFocus, Txt_Roundness.LostFocus, Txt_colossal_EdgeNormal.LostFocus
        Dim textBox = DirectCast(sender, TextBox)
        Dim currentValue = 0.0
        Dim needsCorrection = False
        Dim correctionMessage = ""
        Dim correctedValue = ""

        Select Case textBox.Name

            Case Txt_Metallic.Name, Txt_Smoothness.Name, Txt_MetallicOpacity.Name, Txt_NormalOpacity.Name

                ' Try to parse the current value using CultureInfo.InvariantCulture
                If Double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, currentValue) Then
                    If currentValue > 1.0 Then
                        needsCorrection = True
                        correctedValue = "1.0"
                        correctionMessage = "The value for " & textBox.Name & " cannot be greater than 1.0. It has been set to 1.0."
                    End If
                Else ' If parsing fails (e.g., empty or invalid text)
                    needsCorrection = True
                    correctedValue = "1.0"
                    correctionMessage = "Invalid or empty value for " & textBox.Name & ". It has been set to 1.0."
                End If

                If needsCorrection = True Then
                    textBox.Text = correctedValue
                    MessageBox.Show(correctionMessage, "Value Corrected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

            Case Txt_Roundness.Name

                'Special case for Txt_Roundness
                If Double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, currentValue) Then
                    If textBox.Name = Txt_Roundness.Name Then
                        If currentValue > 1.0 Then
                            needsCorrection = True
                            correctedValue = "1.0"
                            correctionMessage = "The value for " & textBox.Name & " cannot be greater than 1.0. It has been set to 1.0."
                        End If
                    Else ' If parsing fails (e.g., empty or invalid text)
                        needsCorrection = True
                        correctedValue = "0.5"
                        correctionMessage = "Invalid or empty value for " & textBox.Name & ". It has been set to 0.5."

                    End If

                    If needsCorrection = True Then
                        textBox.Text = correctedValue
                        MessageBox.Show(correctionMessage, "Value Corrected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If

                End If

            Case Txt_colossal_EdgeNormal.Name
                'Special case for Txt_colossal_EdgeNormal
                If Double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, currentValue) Then
                    If textBox.Name = Txt_colossal_EdgeNormal.Name Then
                        If currentValue > 1.0 Then
                            needsCorrection = True
                            correctedValue = "1.0"
                            correctionMessage = "The value for " & textBox.Name & " cannot be greater than 1.0. It has been set to 1.0."
                        End If
                    Else ' If parsing fails (e.g., empty or invalid text)
                        needsCorrection = True
                        correctedValue = "0.5"
                        correctionMessage = "Invalid or empty value for " & textBox.Name & ". It has been set to 0.5."
                    End If

                    If needsCorrection = True Then
                        textBox.Text = correctedValue
                        MessageBox.Show(correctionMessage, "Value Corrected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If

                End If
        End Select

        'If needsCorrection = "Generic" Or needsCorrection = "Roundness" Or needsCorrection = "EdgeNormal" Then
        '    textBox.Text = correctedValue
        '    MessageBox.Show(correctionMessage, "Value Corrected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        'End If

        EnableModifyButton() ' Always enable the modify button after user interaction

    End Sub

    ' LostFocus handler for Txt_X, Txt_Y, Txt_Z (dimension values)
    Private Sub HandleDimensionValueLostFocus(sender As Object, e As EventArgs) Handles Txt_X.LostFocus, Txt_Y.LostFocus, Txt_Z.LostFocus
        Dim textBox As TextBox = DirectCast(sender, TextBox)
        Dim currentValue As Double = 0.0
        Dim needsCorrection As Boolean = False
        Dim correctionMessage As String = ""

        ' Try to parse the current value using CultureInfo.InvariantCulture
        If Double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, currentValue) Then
            If currentValue <= 0.0 Then
                needsCorrection = True
                correctionMessage = "The value cannot be zero or negative. It has been set to 1.0."
            End If
        Else ' If parsing fails (e.g., empty or invalid text)
            needsCorrection = True
            correctionMessage = "Invalid or empty value. It has been set to 1.0."
        End If

        If needsCorrection Then
            textBox.Text = "1.0" ' Correct to 1.0 as string
            MessageBox.Show(correctionMessage, "Value Corrected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        EnableModifyButton() ' Always enable the modify button after user interaction
    End Sub

    ' LostFocus handler for Txt_collosal_UVScale
    Private Sub HandleUVScaleValueLostFocus(sender As Object, e As EventArgs) Handles Txt_colossal_UVScale.LostFocus
        Dim textBox As TextBox = DirectCast(sender, TextBox)
        Dim currentValue As Double = 0.0
        Dim needsCorrection As Boolean = False
        Dim correctionMessage As String = ""

        ' Try to parse the current value using CultureInfo.InvariantCulture
        If Double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, currentValue) Then
            If currentValue <= 0.0 Or currentValue > 2 Then
                needsCorrection = True
                correctionMessage = "UV Scale cannot be zero or bigger than 2. It has been set to 0.2."
            End If
        Else ' If parsing fails (e.g., empty or invalid text)
            needsCorrection = True
            correctionMessage = "UV Scale is invalid or empty value. It has been set to 0.2."
        End If

        If needsCorrection Then
            textBox.Text = "0.2" ' Correct to 1.0 as string
            MessageBox.Show(correctionMessage, "Value Corrected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        EnableModifyButton() ' Always enable the modify button after user interaction
    End Sub

    ' LostFocus handler for Txt_MeshSize
    Private Sub HandleMeshSizeValueLostFocus(sender As Object, e As EventArgs) Handles Txt_MeshSize.LostFocus
        Dim textBox As TextBox = DirectCast(sender, TextBox)
        Dim currentValue As Double = 1.0
        Dim needsCorrection As Boolean = False
        Dim correctionMessage As String = ""

        ' Try to parse the current value using CultureInfo.InvariantCulture
        If Double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, currentValue) Then
            If currentValue < 0.5 Then
                needsCorrection = True
                correctionMessage = "Mesh Size cannot be smaller than 0.5. It has been set to 1.0."
            End If
        Else ' If parsing fails (e.g., empty or invalid text)
            needsCorrection = True
            correctionMessage = "Mesh Size is invalid or empty value. It has been set to 1.0."
        End If

        If needsCorrection Then
            textBox.Text = "1.0" ' Correct to 1.0 as string
            MessageBox.Show(correctionMessage, "Value Corrected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        EnableModifyButton() ' Always enable the modify button after user interaction
    End Sub

    ' LostFocus handler for Nud_UiPriority
    Private Sub HandleNUDUiPriorityLostFocus(sender As Object, e As EventArgs) Handles Nud_UiPriority.LostFocus
        Dim nudControl As NumericUpDown = DirectCast(sender, NumericUpDown)
        Dim currentValue As Integer = 0
        Dim needsCorrection As Boolean = False
        Dim correctionMessage As String = ""
        Dim correctedValue As Integer = 0 ' EAI default value v1.4.3 r2

        If Integer.TryParse(nudControl.Text, currentValue) Then
            If currentValue < 0 OrElse currentValue > 99999999 Then
                needsCorrection = True
                correctionMessage = "UiPriority value must be between 0 and 99999999. It has been set to " & correctedValue.ToString() & "."
            End If
        Else ' If parsing fails (e.g., empty or invalid text)
            needsCorrection = True
            correctionMessage = "UiPriority is invalid or empty. It has been set to " & correctedValue.ToString() & "."
        End If

        If needsCorrection Then
            nudControl.Text = correctedValue.ToString()
            MessageBox.Show(correctionMessage, "Value Corrected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        EnableModifyButton() ' Always enable the modify button after user interaction
    End Sub

    ' LostFocus handler for Nud_DrawOrder
    Private Sub HandleNUDDrawOrderLostFocus(sender As Object, e As EventArgs) Handles Nud_DrawOrder.LostFocus
        Dim nudControl As NumericUpDown = DirectCast(sender, NumericUpDown)
        Dim currentValue As Integer = 0
        Dim needsCorrection As Boolean = False
        Dim correctionMessage As String = ""
        Dim correctedValue As Integer = 0 ' Will be set to -170 or 200

        If Integer.TryParse(nudControl.Text, currentValue) Then
            If currentValue < -170 Then
                needsCorrection = True
                correctedValue = -170
                correctionMessage = "Draw Order cannot be less than -170. It has been set to -170."
            ElseIf currentValue > 200 Then
                needsCorrection = True
                correctedValue = 200
                correctionMessage = "Draw Order cannot be greater than 200. It has been set to 200."
            End If
        Else ' If parsing fails (e.g., empty or invalid text)
            needsCorrection = True

            correctedValue = 0
            correctionMessage = "Draw Order is invalid or empty. It has been set to " & correctedValue.ToString() & "."

        End If

        If needsCorrection Then
            nudControl.Text = correctedValue.ToString()
            MessageBox.Show(correctionMessage, "Value Corrected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        EnableModifyButton() ' Always enable the modify button after user interaction
    End Sub

    ' Function to rename the asset folder and update AssetFullPath
    Private Function RenameAssetFolder() As Boolean
        Dim currentFullPath As String = Me.AssetFullPath
        Dim currentDirectory As String = Path.GetDirectoryName(currentFullPath) ' Get the parent directory
        Dim currentFolderName As String = Path.GetFileName(currentFullPath) ' Get the current asset folder name

        Dim newAssetName As String = Txt_AssetName.Text.Trim() ' Get new name from textbox and trim leading/trailing spaces

        ' --- Validation ---
        ' 1. Check if the new name is empty or just whitespace
        If String.IsNullOrWhiteSpace(newAssetName) Then
            MessageBox.Show("Asset name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        ' 2. Check for double spaces
        If newAssetName.Contains("  ") Then
            MessageBox.Show("Asset name cannot contain double spaces.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        ' 3. Check for invalid characters (only letters, numbers, -, _, space allowed)
        For Each c As Char In newAssetName
            If Not (Char.IsLetterOrDigit(c) OrElse c = "-"c OrElse c = "_"c OrElse c = " "c) Then
                MessageBox.Show("Asset name contains invalid characters. Only letters, numbers, hyphens (-), underscores (_), and spaces are allowed.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Next

        ' If the new name is the same as the old name, no renaming is needed.
        If String.Equals(newAssetName, currentFolderName, StringComparison.OrdinalIgnoreCase) Then
            Return True ' No change needed, consider it successful
        End If

        Dim newFullPath As String = Path.Combine(currentDirectory, newAssetName)

        ' Check if the new folder already exists
        If Directory.Exists(newFullPath) Then
            MessageBox.Show("Asset with the name '" & newAssetName & "' already exists. Please choose a different name.", "Rename Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        Try
            ' Rename the directory
            Directory.Move(currentFullPath, newFullPath)
            Me.AssetFullPath = newFullPath ' Update the AssetFullPath property
            Me.AssetName = newAssetName ' Update AssetName property to reflect the new folder name
            Me.Text = "Editing: " & Me.AssetName ' Update the form title
            Return True
        Catch ex As Exception
            MessageBox.Show("Error renaming asset: " & ex.Message, "Rename Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' Btn_Modify_Click event handler to save changes to the JSON file
    Private Sub Btn_Modify_Click(sender As Object, e As EventArgs) Handles Btn_Modify.Click

        ' --- 1. Pre-validation of all fields ---
        Dim parsedUiPriority As Integer

        If Not Integer.TryParse(Nud_UiPriority.Text, parsedUiPriority) OrElse parsedUiPriority < 0 OrElse parsedUiPriority > 99999999 Then
            MessageBox.Show("Invalid value for UiPriority. It must be a value between 0 and 99999999.", "uiPriority Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim parsedMetallic As Double
        If Not Double.TryParse(Txt_Metallic.Text, NumberStyles.Any, CultureInfo.InvariantCulture, parsedMetallic) OrElse parsedMetallic < 0.0 OrElse parsedMetallic > 1.0 Then
            MessageBox.Show("Invalid value for _Metallic. It must be a value between 0.0 and 1.0.", "Metallic Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim parsedSmoothness As Double
        If Not Double.TryParse(Txt_Smoothness.Text, NumberStyles.Any, CultureInfo.InvariantCulture, parsedSmoothness) OrElse parsedSmoothness < 0.0 OrElse parsedSmoothness > 1.0 Then
            MessageBox.Show("Invalid value for _Smoothness. It must be a value between 0.0 and 1.0.", "Smoothness Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim parsedMetallicOpacity As Double
        If Not Double.TryParse(Txt_MetallicOpacity.Text, NumberStyles.Any, CultureInfo.InvariantCulture, parsedMetallicOpacity) OrElse parsedMetallicOpacity < 0.0 OrElse parsedMetallicOpacity > 1.0 Then
            MessageBox.Show("Invalid value for _MetallicOpacity. It must be a value between 0.0 and 1.0.", "Metallic Opacity Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim parsedNormalOpacity As Double
        If Not Double.TryParse(Txt_NormalOpacity.Text, NumberStyles.Any, CultureInfo.InvariantCulture, parsedNormalOpacity) OrElse parsedNormalOpacity < 0.0 OrElse parsedNormalOpacity > 1.0 Then
            MessageBox.Show("Invalid value for _NormalOpacity. It must be a value between 0.0 and 1.0.", "Normal Opacity Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim parsedDrawOrder As Integer 'modified to NumericUpDown control

        If Not Integer.TryParse(Nud_DrawOrder.Text, parsedDrawOrder) OrElse parsedDrawOrder < -170 OrElse parsedDrawOrder > 200 Then
            MessageBox.Show("Invalid value for _DrawOrder. It must be a value between -170 and 200.", "Draw Order Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim parsedRoundness As Double = 0.5 ' Default for surfaces // EAI default value v1.4.3 r2
        Dim parsedUVScale As Double = 0.2 ' Default for surfaces // EAI default value v1.4.3 r2
        Dim parsedEdgeNormal As Double = 0.5 ' Default for surfaces // EAI default value v1.4.3 r2

        If AssetType = "Surfaces" Then
            If Not Double.TryParse(Txt_Roundness.Text, NumberStyles.Any, CultureInfo.InvariantCulture, parsedRoundness) OrElse parsedRoundness < 0.0 OrElse parsedRoundness > 1.0 Then
                MessageBox.Show("Invalid value for m_Roundness. It must be a value between 0.0 and 1.0.", "Roundness Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            If Not Double.TryParse(Txt_colossal_UVScale.Text, NumberStyles.Any, CultureInfo.InvariantCulture, parsedUVScale) OrElse parsedUVScale < MIN_DECIMAL_VALUE_FOR_CALCULATION OrElse parsedUVScale > 10.0 Then
                MessageBox.Show("Invalid value for colossal_UVScale. It must be a value between " & MIN_DECIMAL_VALUE_FOR_CALCULATION.ToString(CultureInfo.InvariantCulture) & " and 10.0.", "UV Scale Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            If Not Double.TryParse(Txt_colossal_EdgeNormal.Text, NumberStyles.Any, CultureInfo.InvariantCulture, parsedEdgeNormal) OrElse parsedEdgeNormal < 0.0 OrElse parsedEdgeNormal > 1.0 Then
                MessageBox.Show("Invalid value for colossal_EdgeNormal. It must be a value between 0.0 and 1.0.", "Edge Normal Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        End If

        Dim parsedX As Double = 1.0 ' Default for non-decals/netlanes // EAI default value v1.4.3 r2
        Dim parsedY As Double = 1.0 ' Default for non-decals/netlanes // EAI default value v1.4.3 r2
        Dim parsedZ As Double = 1.0 ' Default for non-decals/netlanes // EAI default value v1.4.3 r2

        If AssetType = "Decals" OrElse AssetType = "Netlanes" Then
            If Not Double.TryParse(Txt_X.Text, NumberStyles.Any, CultureInfo.InvariantCulture, parsedX) OrElse parsedX = 0.0 Then
                MessageBox.Show("Invalid value for X. It cannot be zero.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            If Not Double.TryParse(Txt_Y.Text, NumberStyles.Any, CultureInfo.InvariantCulture, parsedY) OrElse parsedY = 0.0 Then
                MessageBox.Show("Invalid value for Y. It cannot be zero.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            If Not Double.TryParse(Txt_Z.Text, NumberStyles.Any, CultureInfo.InvariantCulture, parsedZ) OrElse parsedZ = 0.0 Then
                MessageBox.Show("Invalid value for Z. It cannot be zero.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        End If

        ' 2. Prepare JSON file paths ---
        ' Ensure the asset folder is renamed before saving
        If Not RenameAssetFolder() Then
            Return ' If renaming fails, stop the saving process
        End If

        Dim jsonFileName As String = ""
        Dim netlaneJsonPath As String = ""
        Dim mainJsonPath As String = ""

        Select Case AssetType
            Case "Decals"
                jsonFileName = "decal.json"
            Case "Surfaces"
                jsonFileName = "surface.json"
            Case "Netlanes"
                jsonFileName = "decal.json"
                netlaneJsonPath = Path.Combine(AssetFullPath, "netlane.json")
            Case Else
                MessageBox.Show("Unsupported asset type for saving: " & AssetType, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
        End Select

        ' Start of JSON saving routine
        mainJsonPath = Path.Combine(AssetFullPath, jsonFileName)

        ' --- Load the original JSON if not already done ---
        If _assetJsonContent Is Nothing Then
            _assetJsonContent = New JObject()
        End If

        Try
            ' --- REBUILD THE JObject TO ENSURE CORRECT PROPERTY ORDER ---
            Dim newAssetJsonContent As New JObject()

            ' 1. Add UiPriority (always first at the root)
            newAssetJsonContent.Add("UiPriority", parsedUiPriority)

            ' 2. Add m_Roundness if Surfaces (second at the root)
            If AssetType = "Surfaces" Then
                newAssetJsonContent.Add("m_Roundness", parsedRoundness)
            End If

            ' 3. Get the existing "Float" and "Vector" objects from the original content
            Dim originalFloatObject As JObject = Nothing
            If _assetJsonContent.ContainsKey("Float") AndAlso _assetJsonContent("Float").Type = JTokenType.Object Then
                originalFloatObject = DirectCast(_assetJsonContent("Float"), JObject).DeepClone()
            Else
                originalFloatObject = New JObject()
            End If

            Dim originalVectorObject As JObject = Nothing
            If _assetJsonContent.ContainsKey("Vector") AndAlso _assetJsonContent("Vector").Type = JTokenType.Object Then
                originalVectorObject = DirectCast(_assetJsonContent("Vector"), JObject).DeepClone()
            Else
                originalVectorObject = New JObject()
            End If

            ' 4. Update the values within the "Float" object
            originalFloatObject("colossal_DecalLayerMask") = Integer.Parse(Lbl_DlmValue.Text)
            originalFloatObject("_Metallic") = parsedMetallic
            originalFloatObject("_Smoothness") = parsedSmoothness
            originalFloatObject("_MetallicOpacity") = parsedMetallicOpacity
            originalFloatObject("_NormalOpacity") = parsedNormalOpacity
            originalFloatObject("_DrawOrder") = parsedDrawOrder

            ' Update properties specific to "Surfaces"
            If AssetType = "Surfaces" Then
                originalFloatObject("colossal_UVScale") = parsedUVScale
                originalFloatObject("colossal_EdgeNormal") = parsedEdgeNormal
            End If

            ' 5. Remove any root-level properties from the float object that might be there
            ' This ensures no duplicates, as UiPriority/m_Roundness are now at the root.
            originalFloatObject.Remove("UiPriority")
            originalFloatObject.Remove("m_Roundness")

            ' 6. Update properties within the "Vector" object for Decals/Netlanes
            If AssetType = "Decals" OrElse AssetType = "Netlanes" Then
                Dim meshSizeObject As JObject = Nothing
                If originalVectorObject.ContainsKey("colossal_MeshSize") AndAlso originalVectorObject("colossal_MeshSize").Type = JTokenType.Object Then
                    meshSizeObject = DirectCast(originalVectorObject("colossal_MeshSize"), JObject)
                Else
                    meshSizeObject = New JObject()
                    originalVectorObject("colossal_MeshSize") = meshSizeObject
                End If

                meshSizeObject("x") = parsedX
                meshSizeObject("y") = parsedY
                meshSizeObject("z") = parsedZ
                If meshSizeObject("w") Is Nothing Then
                    meshSizeObject("w") = 0
                End If
            End If

            ' 7. Add the updated Float and Vector objects to the new JObject
            newAssetJsonContent.Add("Float", originalFloatObject)
            newAssetJsonContent.Add("Vector", originalVectorObject)

            ' 8. Copy the existing prefabIdentifierInfos array from the original JSON
            If _assetJsonContent.ContainsKey("prefabIdentifierInfos") AndAlso _assetJsonContent("prefabIdentifierInfos").Type = JTokenType.Array Then
                Dim originalPrefabInfoArray As JArray = DirectCast(_assetJsonContent("prefabIdentifierInfos"), JArray).DeepClone()
                newAssetJsonContent.Add("prefabIdentifierInfos", originalPrefabInfoArray)
            End If

            ' 9. Copy any other root-level properties from the original JSON
            '    that are not explicitly handled. This preserves all other data.
            For Each prop As JProperty In _assetJsonContent.Properties()
                If Not (prop.Name.Equals("UiPriority", StringComparison.OrdinalIgnoreCase) OrElse
                    prop.Name.Equals("m_Roundness", StringComparison.OrdinalIgnoreCase) OrElse
                    prop.Name.Equals("Float", StringComparison.OrdinalIgnoreCase) OrElse
                    prop.Name.Equals("Vector", StringComparison.OrdinalIgnoreCase) OrElse
                    prop.Name.Equals("prefabIdentifierInfos", StringComparison.OrdinalIgnoreCase)) Then
                    newAssetJsonContent.Add(prop.Name, prop.Value.DeepClone())
                End If
            Next

            _assetJsonContent = newAssetJsonContent ' Replace the old JObject with the newly constructed one.

            ' --- 10. Save the main JSON file ---
            File.WriteAllText(mainJsonPath, _assetJsonContent.ToString(Formatting.Indented))

            ' --- 11. Handle netlane.json for Netlanes asset type (no changes) ---
            If AssetType = "Netlanes" Then
                If _netlaneJsonContent Is Nothing Then
                    _netlaneJsonContent = New JObject()
                End If
                _netlaneJsonContent("UiPriority") = parsedUiPriority
                File.WriteAllText(netlaneJsonPath, _netlaneJsonContent.ToString(Formatting.Indented))
            End If

            MessageBox.Show(Txt_AssetName.Text & " saved successfully!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Btn_Modify.Enabled = False
            Me.Close() ' Close the form after saving

        Catch ex As Exception
            MessageBox.Show("Error saving" & Txt_AssetName.Text & " : " & ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadAssetJsonData()
        Dim jsonFileName As String = ""
        Dim netlaneJsonPath As String = ""
        Dim mainJsonPath As String = ""

        ' Determine the main JSON file name based on asset type
        Select Case AssetType
            Case "Decals"
                jsonFileName = "decal.json"
            Case "Surfaces"
                jsonFileName = "surface.json"
            Case "Netlanes"
                jsonFileName = "decal.json" ' Netlanes also use decal.json
                netlaneJsonPath = Path.Combine(AssetFullPath, "netlane.json") ' Path for netlane.json
            Case Else
                MessageBox.Show("Unsupported asset type for editing: " & AssetType & vbCrLf & vbCrLf & "Operation will be terminated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close() ' Close the form if JSON parsing fails
                Exit Sub
        End Select

        mainJsonPath = Path.Combine(AssetFullPath, jsonFileName)

        ' FIXING JSON v.4.0.0
        If File.Exists(mainJsonPath) Then
            Dim jsonText As String = ""
            Try
                jsonText = File.ReadAllText(mainJsonPath)
                _assetJsonContent = JObject.Parse(jsonText)

            Catch ex As Exception
                MessageBox.Show("A syntax error was detected in the JSON file. Attempting an advanced repair...", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)

                ' Call the new repair function.
                Dim repairedJsonText As String = RepairJsonWithFullTextAnalysis(jsonText)

                Try
                    ' Attempt to parse the pre-repaired JSON.
                    _assetJsonContent = JObject.Parse(repairedJsonText)
                    MessageBox.Show("The JSON file has been successfully repaired and loaded.", "JSON Repaired", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex2 As Exception
                    MessageBox.Show("The current JSON file is corrupt and was impossible to repair." & vbCrLf & vbCrLf & "The operation will be terminated.", "Corrupt JSON", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.Close()
                    Exit Sub
                End Try
            End Try

        Else
            MessageBox.Show(jsonFileName & " was not found for this asset." & vbCrLf & vbCrLf & "The operation will be terminated.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
            Exit Sub
        End If
        ' END FIXING JSON v.4.0.0

        ' Load netlane.json if asset type is Netlane
        If AssetType = "Netlanes" Then
            If File.Exists(netlaneJsonPath) Then
                Try
                    Dim netlaneJsonText As String = File.ReadAllText(netlaneJsonPath)
                    _netlaneJsonContent = JObject.Parse(netlaneJsonText)
                Catch ex As Exception
                    MessageBox.Show("Error reading or parsing netlane.json: " & ex.Message, "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    _netlaneJsonContent = New JObject() ' Initialize as empty object
                End Try
            Else
                MessageBox.Show("netlane.json not found for this Netlane asset. It will be created with default values upon saving.", "netlane.json Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                _netlaneJsonContent = New JObject() ' Initialize as empty object so it's created on save
            End If
        End If

        ' Load netlane.json if asset type is Netlane
        If AssetType = "Netlanes" Then
            If File.Exists(netlaneJsonPath) Then
                Try
                    Dim netlaneJsonText As String = File.ReadAllText(netlaneJsonPath)
                    _netlaneJsonContent = JObject.Parse(netlaneJsonText)
                Catch ex As Exception
                    MessageBox.Show("Error reading or parsing netlane.json: " & ex.Message, "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    _netlaneJsonContent = New JObject() ' Initialize as empty object
                End Try
            Else
                MessageBox.Show("netlane.json not found for this Netlane asset. It will be created with default values upon saving.", "netlane.json Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                _netlaneJsonContent = New JObject() ' Initialize as empty object so it's created on save
            End If
        End If

        ' Load netlane.json if asset type is Netlane
        If AssetType = "Netlanes" Then
            If File.Exists(netlaneJsonPath) Then
                Try
                    Dim netlaneJsonText As String = File.ReadAllText(netlaneJsonPath)
                    _netlaneJsonContent = JObject.Parse(netlaneJsonText)
                Catch ex As Exception
                    MessageBox.Show("Error reading or parsing netlane.json: " & ex.Message, "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    _netlaneJsonContent = New JObject() ' Initialize as empty object
                End Try
            Else
                MessageBox.Show("netlane.json not found for this Netlane asset. It will be created with default values upon saving.", "netlane.json Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                _netlaneJsonContent = New JObject() ' Initialize as empty object so it's created on save
            End If
        End If

        ' === Load values common to all asset types ===
        ' UiPriority (Try Root, then Float if not found)
        Dim uiPriorityFound As Boolean = False
        Dim uiPriorityValue As Integer = 0 'EAI default value v1.4.3

        If _assetJsonContent.ContainsKey("UiPriority") Then
            Dim token As JToken = _assetJsonContent("UiPriority")
            Dim tempInt As Integer? = token.Value(Of Integer?)
            If tempInt.HasValue Then
                uiPriorityValue = tempInt.Value
                uiPriorityFound = True
            End If
        End If

        Dim floatObject As JObject = Nothing
        If _assetJsonContent.ContainsKey("Float") AndAlso _assetJsonContent("Float").Type = JTokenType.Object Then
            floatObject = DirectCast(_assetJsonContent("Float"), JObject)
        End If

        If Not uiPriorityFound AndAlso floatObject IsNot Nothing AndAlso floatObject.ContainsKey("UiPriority") Then
            Dim token As JToken = floatObject("UiPriority")
            Dim tempInt As Integer? = token.Value(Of Integer?)
            If tempInt.HasValue Then
                uiPriorityValue = tempInt.Value
                uiPriorityFound = True
            End If
        End If

        Nud_UiPriority.Text = uiPriorityValue.ToString()
        If Not uiPriorityFound Then
            Nud_UiPriority.ForeColor = Color.Red
        Else
            Nud_UiPriority.ForeColor = Color.Black
        End If


        ' _Metallic (Nested under "Float")
        If floatObject IsNot Nothing Then
            LoadDoubleValue(Txt_Metallic, floatObject, "_Metallic", 1.0) 'EAI default value v1.4.3
        Else
            Txt_Metallic.Text = 1.0.ToString(CultureInfo.InvariantCulture)
            Txt_Metallic.ForeColor = Color.Red
        End If

        ' _Smoothness (Nested under "Float")
        If floatObject IsNot Nothing Then
            LoadDoubleValue(Txt_Smoothness, floatObject, "_Smoothness", 1.0) 'EAI default value v1.4.3
        Else
            Txt_Smoothness.Text = 1.0.ToString(CultureInfo.InvariantCulture)
            Txt_Smoothness.ForeColor = Color.Red
        End If

        ' _MetallicOpacity (Nested under "Float")
        If floatObject IsNot Nothing Then
            LoadDoubleValue(Txt_MetallicOpacity, floatObject, "_MetallicOpacity", 1.0) 'EAI default value v1.4.3
        Else
            Txt_MetallicOpacity.Text = 1.0.ToString(CultureInfo.InvariantCulture)
            Txt_MetallicOpacity.ForeColor = Color.Red
        End If

        ' _NormalOpacity (Nested under "Float")
        If floatObject IsNot Nothing Then
            LoadDoubleValue(Txt_NormalOpacity, floatObject, "_NormalOpacity", 1.0) 'EAI default value v1.4.3
        Else
            Txt_NormalOpacity.Text = 1.0.ToString(CultureInfo.InvariantCulture)
            Txt_NormalOpacity.ForeColor = Color.Red
        End If

        ' _DrawOrder (Nested under "Float")
        Dim defaultDrawOrder As Integer = 0 'EAI default value v1.4.3

        If floatObject IsNot Nothing Then
            'LoadIntegerValue(Txt_DrawOrder, floatObject, "_DrawOrder", defaultDrawOrder)
            LoadIntegerValueUpDown(Nud_DrawOrder, floatObject, "_DrawOrder", defaultDrawOrder)
        Else
            'Txt_DrawOrder.Text = defaultDrawOrder.ToString()
            'Txt_DrawOrder.ForeColor = Color.Red

            Nud_DrawOrder.Text = defaultDrawOrder.ToString()
            Nud_DrawOrder.ForeColor = Color.Red
        End If

        ' colossal_DecalLayerMask (Nested under "Float") - Moved outside the Select Case for common usage
        Dim decalLayerMaskValue As Integer = 1 'EAI default value v1.4.3
        Dim decalLayerMaskFound As Boolean = False

        If floatObject IsNot Nothing Then
            If floatObject.ContainsKey("colossal_DecalLayerMask") Then
                Dim token As JToken = floatObject("colossal_DecalLayerMask")
                Dim tempInt As Integer? = token.Value(Of Integer?)
                If tempInt.HasValue Then
                    decalLayerMaskValue = tempInt.Value
                    decalLayerMaskFound = True
                End If
            End If
        End If
        SetDecalLayerMaskCheckboxes(decalLayerMaskValue, decalLayerMaskFound)

        ' === Load specific values based on asset type ===
        Select Case AssetType
            Case "Surfaces"
                ' m_Roundness (Try Root, then Float if not found)
                Dim mRoundnessFound As Boolean = False
                Dim mRoundnessValue As Double = 0.5 'EAI default value v1.4.3

                If _assetJsonContent.ContainsKey("m_Roundness") Then
                    Dim token As JToken = _assetJsonContent("m_Roundness")
                    Dim tempDouble As Double? = token.Value(Of Double?)
                    If tempDouble.HasValue Then
                        mRoundnessValue = tempDouble.Value
                        mRoundnessFound = True
                    End If
                End If

                If Not mRoundnessFound AndAlso floatObject IsNot Nothing AndAlso floatObject.ContainsKey("m_Roundness") Then
                    Dim token As JToken = floatObject("m_Roundness")
                    Dim tempDouble As Double? = token.Value(Of Double?)
                    If tempDouble.HasValue Then
                        mRoundnessValue = tempDouble.Value
                        mRoundnessFound = True
                    End If
                End If

                Txt_Roundness.Text = mRoundnessValue.ToString(CultureInfo.InvariantCulture)
                If Not mRoundnessFound Then
                    Txt_Roundness.ForeColor = Color.Red
                Else
                    Txt_Roundness.ForeColor = Color.Black
                End If

                ' colossal_UVScale (Nested under "Float")
                If floatObject IsNot Nothing Then
                    LoadDoubleValue(Txt_colossal_UVScale, floatObject, "colossal_UVScale", 0.2) 'EAI default value v1.4.3
                Else
                    Txt_colossal_UVScale.Text = 0.2.ToString(CultureInfo.InvariantCulture)
                    Txt_colossal_UVScale.ForeColor = Color.Red
                End If
                ' colossal_EdgeNormal (Nested under "Float")
                If floatObject IsNot Nothing Then
                    LoadDoubleValue(Txt_colossal_EdgeNormal, floatObject, "colossal_EdgeNormal", 0.5) 'EAI default value v1.4.3
                Else
                    Txt_colossal_EdgeNormal.Text = 0.5.ToString(CultureInfo.InvariantCulture)
                    Txt_colossal_EdgeNormal.ForeColor = Color.Red
                End If

            Case "Decals", "Netlanes"
                ' x, y, z (Nested under "Vector" -> "colossal_MeshSize")
                ' Zero value check removed as per user's request to restore later
                LoadNestedDoubleValue(Txt_X, _assetJsonContent, "Vector", "colossal_MeshSize", "x", 1.0) 'EAI default value v1.4.3
                LoadNestedDoubleValue(Txt_Y, _assetJsonContent, "Vector", "colossal_MeshSize", "y", 1.0) 'EAI default value v1.4.3
                LoadNestedDoubleValue(Txt_Z, _assetJsonContent, "Vector", "colossal_MeshSize", "z", 1.0) 'EAI default value v1.4.3

                ' For Netlanes, UiPriority also in netlane.json
                If AssetType = "Netlanes" Then
                    ' Ensure _netlaneJsonContent is not Nothing if it was created empty
                    If _netlaneJsonContent Is Nothing Then _netlaneJsonContent = New JObject()
                    LoadIntegerValueUpDown(Nud_UiPriority, _netlaneJsonContent, "UiPriority", 0) ' Load from netlane.json EAI default value v1.4.3
                End If
        End Select
    End Sub

    ' New function for a smarter JSON repair. GEMINI
    Private Function RepairJsonWithFullTextAnalysis(ByVal json As String) As String
        ' Step 1: Basic cleaning of comments and extra spaces.
        Dim cleanedJson As String = System.Text.RegularExpressions.Regex.Replace(json, "//.*", "")
        cleanedJson = System.Text.RegularExpressions.Regex.Replace(cleanedJson, "/\*.*?\*/", "", System.Text.RegularExpressions.RegexOptions.Singleline)

        ' Step 2: Replace commas with dots in numeric values.
        ' This corrects the "0,0" error.
        cleanedJson = System.Text.RegularExpressions.Regex.Replace(cleanedJson, """(\w+)"":\s*(\d+),(\d+)", """$1"": $2.$3")

        ' Step 3: Split the text into lines for a detailed inspection.
        Dim lines As String() = cleanedJson.Split(New String() {vbCrLf, vbLf}, StringSplitOptions.None)
        Dim repairedLines As New List(Of String)()

        ' Step 4: Iterate through the lines to correct commas.
        ' We maintain a simple state to know if we are inside a container.
        Dim previousLineEndsWithValue As Boolean = False

        For i As Integer = 0 To lines.Length - 1
            Dim currentLine As String = lines(i).Trim()

            If String.IsNullOrWhiteSpace(currentLine) Then
                Continue For ' Skip empty lines.
            End If

            ' Check if the previous line needs a comma.
            If previousLineEndsWithValue Then
                ' The previous line ended with a value or a closed container,
                ' and this line is not a closing brace, so it needs a comma.
                Dim lastIndex As Integer = repairedLines.Count - 1
                If lastIndex >= 0 Then
                    ' Verify that the current line is not a closing brace,
                    ' nor a closing bracket.
                    If Not currentLine.StartsWith("}") AndAlso Not currentLine.StartsWith("]") Then
                        ' Ensure that the previous line doesn't already end with a comma.
                        If Not repairedLines(lastIndex).EndsWith(",") Then
                            repairedLines(lastIndex) &= ","
                        End If
                    End If
                End If
            End If

            repairedLines.Add(currentLine)

            ' Reset the flag for the next iteration.
            previousLineEndsWithValue = False

            ' Check if the current line ends with a value, a closed object, or a closed array.
            ' If so, the next line might need a comma.
            If currentLine.EndsWith("""") OrElse
           currentLine.EndsWith("}") OrElse
           currentLine.EndsWith("]") OrElse
           System.Text.RegularExpressions.Regex.IsMatch(currentLine, "\d+$") OrElse
           currentLine.EndsWith("true") OrElse currentLine.EndsWith("false") OrElse currentLine.EndsWith("null") Then
                previousLineEndsWithValue = True
            End If

            ' Exception: if the current line is an opening brace, there is no comma.
            If currentLine.EndsWith("{") OrElse currentLine.EndsWith("[") Then
                previousLineEndsWithValue = False
            End If

        Next

        ' Step 5: Join the corrected lines into a single string.
        Return String.Join(Environment.NewLine, repairedLines)
    End Function

    ' Helper to load Double values from a JObject to a TextBox
    Private Sub LoadDoubleValue(targetTextBox As TextBox, jsonObject As JObject, key As String, defaultValue As Double)
        Dim valueFound As Boolean = False
        Dim parsedValue As Double = defaultValue

        If jsonObject.ContainsKey(key) Then
            Dim token As JToken = jsonObject(key)
            Dim tempDouble As Double? = token.Value(Of Double?) ' Use Value(Of T?) for more robust reading
            If tempDouble.HasValue Then
                parsedValue = tempDouble.Value
                valueFound = True
            End If
        End If

        If valueFound Then
            targetTextBox.Text = parsedValue.ToString(CultureInfo.InvariantCulture) ' Use InvariantCulture for display
            targetTextBox.ForeColor = Color.Black
        Else
            targetTextBox.Text = defaultValue.ToString(CultureInfo.InvariantCulture) ' Use InvariantCulture for display
            targetTextBox.ForeColor = Color.Red
        End If
    End Sub

    ' Helper to load Integer values from a JObject to a TextBox
    Private Sub LoadIntegerValue(targetTextBox As TextBox, jsonObject As JObject, key As String, defaultValue As Integer)
        Dim valueFound As Boolean = False
        Dim parsedValue As Integer = defaultValue

        If jsonObject.ContainsKey(key) Then
            Dim token As JToken = jsonObject(key)
            Dim tempInt As Integer? = token.Value(Of Integer?) ' Use Value(Of T?) for more robust reading
            If tempInt.HasValue Then
                parsedValue = tempInt.Value
                valueFound = True
            End If
        End If

        If valueFound Then
            targetTextBox.Text = parsedValue.ToString()
            targetTextBox.ForeColor = Color.Black
        Else
            targetTextBox.Text = defaultValue.ToString()
            targetTextBox.ForeColor = Color.Red
        End If
    End Sub

    ' Helper to load Integer values from a JObject to a NumericUpDown
    Private Sub LoadIntegerValueUpDown(targetNud As NumericUpDown, jsonObject As JObject, key As String, defaultValue As Integer)
        Dim valueFound As Boolean = False
        Dim parsedValue As Integer = defaultValue

        If jsonObject.ContainsKey(key) Then
            Dim token As JToken = jsonObject(key)
            Dim tempInt As Integer? = token.Value(Of Integer?) ' Use Value(Of T?) for more robust reading
            If tempInt.HasValue Then
                parsedValue = tempInt.Value
                valueFound = True
            End If
        End If

        If valueFound Then
            targetNud.Text = parsedValue.ToString()
            targetNud.ForeColor = Color.Black
        Else
            targetNud.Text = defaultValue.ToString()
            targetNud.ForeColor = Color.Red
        End If
    End Sub

    ' Helper to load nested Double values (e.g., "x" inside "vector" inside "colossal_MeshSize")
    Private Sub LoadNestedDoubleValue(targetTextBox As TextBox, jsonObject As JObject, parentKey As String, nestedKey1 As String, nestedKey2 As String, defaultValue As Double)
        Dim valueFound As Boolean = False
        Dim parsedValue As Double = defaultValue

        If jsonObject.ContainsKey(parentKey) Then
            If jsonObject(parentKey).Type = JTokenType.Object Then
                Dim parentObject As JObject = DirectCast(jsonObject(parentKey), JObject)
                If parentObject.ContainsKey(nestedKey1) Then
                    If parentObject(nestedKey1).Type = JTokenType.Object Then
                        Dim nestedObject1 As JObject = DirectCast(parentObject(nestedKey1), JObject)
                        If nestedObject1.ContainsKey(nestedKey2) Then
                            Dim token As JToken = nestedObject1(nestedKey2)
                            Dim tempDouble As Double? = token.Value(Of Double?) ' Use Value(Of T?) for more robust reading
                            If tempDouble.HasValue Then
                                parsedValue = tempDouble.Value
                                valueFound = True
                            End If
                        End If
                    End If
                End If
            End If
        End If

        If valueFound Then
            ' Zero value check removed as per user's request to restore later
            targetTextBox.Text = parsedValue.ToString(CultureInfo.InvariantCulture) ' Use InvariantCulture for display
            targetTextBox.ForeColor = Color.Black
        Else
            targetTextBox.Text = defaultValue.ToString(CultureInfo.InvariantCulture) ' Use InvariantCulture for display
            targetTextBox.ForeColor = Color.Red
        End If
    End Sub

    ' Helper to load nested Integer values (e.g., "x" inside "vector" inside "colossal_MeshSize")
    Private Sub LoadNestedIntegerValue(targetTextBox As TextBox, jsonObject As JObject, parentKey As String, nestedKey1 As String, nestedKey2 As String, defaultValue As Integer)
        Dim valueFound As Boolean = False
        Dim parsedValue As Integer = defaultValue

        If jsonObject.ContainsKey(parentKey) Then
            If jsonObject(parentKey).Type = JTokenType.Object Then
                Dim parentObject As JObject = DirectCast(jsonObject(parentKey), JObject)
                If parentObject.ContainsKey(nestedKey1) Then
                    If parentObject(nestedKey1).Type = JTokenType.Object Then
                        Dim nestedObject1 As JObject = DirectCast(parentObject(nestedKey1), JObject)
                        If nestedObject1.ContainsKey(nestedKey2) Then
                            Dim token As JToken = nestedObject1(nestedKey2)
                            Dim tempInt As Integer? = token.Value(Of Integer?) ' Use Value(Of T?) for more robust reading
                            If tempInt.HasValue Then
                                parsedValue = tempInt.Value
                                valueFound = True
                            End If
                        End If
                    End If
                End If
            End If
        End If

        If valueFound Then
            targetTextBox.Text = parsedValue.ToString()
            targetTextBox.ForeColor = Color.Black
        Else
            targetTextBox.Text = defaultValue.ToString()
            targetTextBox.ForeColor = Color.Red
        End If
    End Sub

    ' Subroutine to set the state of DecalLayerMask CheckBoxes
    Private Sub SetDecalLayerMaskCheckboxes(value As Integer, wasFoundInJson As Boolean)
        ' Reset all checkboxes first
        Chk_DlmGround.Checked = False
        Chk_DlmRoads.Checked = False
        Chk_DlmBuildings.Checked = False
        Chk_DlmVehicles.Checked = False
        Chk_DlmProps.Checked = False
        Chk_DlmCreatures.Checked = False ' Initialize Chk_DlmCreatures

        Dim remainingValue As Integer = value

        ' Process from highest to lowest value
        If remainingValue >= 32 Then
            Chk_DlmProps.Checked = True
            remainingValue -= 32
        End If
        If remainingValue >= 16 Then ' Value 16 (for Creatures)
            Chk_DlmCreatures.Checked = True ' Set Chk_DlmCreatures
            remainingValue -= 16
        End If
        If remainingValue >= 8 Then
            Chk_DlmVehicles.Checked = True
            remainingValue -= 8
        End If
        If remainingValue >= 4 Then
            Chk_DlmBuildings.Checked = True
            remainingValue -= 4
        End If
        If remainingValue >= 2 Then
            Chk_DlmRoads.Checked = True
            remainingValue -= 2
        End If
        If remainingValue >= 1 Then
            Chk_DlmGround.Checked = True
            remainingValue -= 1
        End If

        ' Set the GroupBox color if the value was default
        If Not wasFoundInJson Then
            Gbx_DecalLayerMask.ForeColor = Color.Red
        Else
            Gbx_DecalLayerMask.ForeColor = SystemColors.ControlText ' System default text color
        End If

        UpdateDlmValueLabel() ' Update the label after setting checkboxes from JSON
    End Sub

    ' Cancel Button Click event handler
    Private Sub Btn_Cancel_Click(sender As Object, e As EventArgs) Handles Btn_Cancel.Click
        Me.Close() ' Close the form without saving changes
    End Sub

    'Decal Size Aspect Ratio calculator and controls
    Private Sub Chk_KeepAspectRatio_CheckedChanged(sender As Object, e As EventArgs) Handles Chk_KeepAspectRatio.CheckedChanged
        If Chk_KeepAspectRatio.Checked Then
            'Enable Aspect Ratio Lock and calculate the saved ratio
            If IsNumeric(Txt_X.Text) AndAlso IsNumeric(Txt_Z.Text) Then
                Dim x As Double = CDbl(Txt_X.Text)
                Dim z As Double = CDbl(Txt_Z.Text)
                If z <> 0 AndAlso x <> 0 Then
                    SavedRatio = x / z
                    KeepRatio = True

                    'Prevent user from changing Y value
                    Txt_Y.Enabled = False

                Else
                    MessageBox.Show("One of the values is zero.", "Decal Size Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Chk_KeepAspectRatio.Checked = False
                End If
            Else
                MessageBox.Show("One or both values are not valid numbers.", "Decal Size Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Chk_KeepAspectRatio.Checked = False
            End If


            Lbl_LockStatus.Text = "—┐" & vbCrLf &
                              "    │" & vbCrLf &
                              "    │" & vbCrLf &
                              "    │" & vbCrLf &
                              "    │" & vbCrLf &
                              "—┘"

        Else
            ' Disable Aspect Ratio Lock
            KeepRatio = False

            Txt_Y.Enabled = True

            Lbl_LockStatus.Text = "—┐" & vbCrLf &
                              "   —" & vbCrLf &
                              "   —" & vbCrLf &
                              "   —" & vbCrLf &
                              "   —" & vbCrLf &
                              "—┘"
        End If

        'added for decal scale calculation v1.4.2
        If Chk_KeepAspectRatio.Checked Then
            Txt_DecalScale.Enabled = True
            Lbl_DecalScale.Enabled = True
            Lbl_Percent.Enabled = True
            If Double.TryParse(Txt_X.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, baseX) Then
                Txt_DecalScale.Text = "100"
            End If
        Else
            Txt_DecalScale.Enabled = False
            Txt_DecalScale.Text = ""
            Lbl_DecalScale.Enabled = False
            Lbl_Percent.Enabled = False
        End If

    End Sub
    Private Sub txt_x_TextChanged(sender As Object, e As EventArgs) Handles Txt_X.TextChanged
        If Not KeepRatio OrElse isUpdating Then Exit Sub

        Dim localInput As String = Txt_X.Text

        If Double.TryParse(Txt_X.Text, NumberStyles.Any, CultureInfo.InvariantCulture, Nothing) Then
            isUpdating = True

            Dim x As Double = Convert.ToDouble(localInput, CultureInfo.InvariantCulture)
            Dim z As Double = x / SavedRatio

            Txt_Z.Text = z.ToString("0.######", CultureInfo.InvariantCulture)

            isUpdating = False

            'added decal scale calculation v1.4.2
            If KeepRatio AndAlso baseX <> 0 Then
                Dim porcentaje As Double = (x / baseX) * 100.0
                ignoreDecalEvent = True
                Txt_DecalScale.Text = porcentaje.ToString("0.##", CultureInfo.InvariantCulture)
                ignoreDecalEvent = False
            End If
        Else
            Return
        End If
    End Sub

    Private Sub txt_z_TextChanged(sender As Object, e As EventArgs) Handles Txt_Z.TextChanged
        If Not KeepRatio OrElse isUpdating Then Exit Sub

        Dim localInput As String = Txt_Z.Text

        If Double.TryParse(Txt_Z.Text, NumberStyles.Any, CultureInfo.InvariantCulture, Nothing) Then
            isUpdating = True

            Dim z As Double = Convert.ToDouble(localInput, CultureInfo.InvariantCulture)
            Dim x As Double = z * SavedRatio

            Txt_X.Text = x.ToString("0.######", CultureInfo.InvariantCulture)

            isUpdating = False

            'added for decal scale calculation v1.4.2
            If KeepRatio AndAlso baseX <> 0 Then
                Dim porcentaje As Double = (x / baseX) * 100.0
                ignoreDecalEvent = True
                Txt_DecalScale.Text = porcentaje.ToString("0.##", CultureInfo.InvariantCulture)
                ignoreDecalEvent = False
            End If
        Else
            Return
        End If
    End Sub

    ' Surfaces colossal_UVScale calculator and controls
    Private Sub Chk_CalcUV_CheckedChanged(sender As Object, e As EventArgs) Handles Chk_CalcUV.CheckedChanged
        If Chk_CalcUV.Checked Then
            ' Modo cálculo por tamaño de malla
            Txt_colossal_UVScale.ReadOnly = True
            Txt_MeshSize.ReadOnly = False
            Txt_MeshSize.Enabled = True

            ' Detener la actualización de Lbl_MeshSizeZ por UVscale
            Lbl_MeshSizeZ.Text = "x " & Txt_MeshSize.Text

        Else
            ' Modo manual de escala UV
            Txt_colossal_UVScale.ReadOnly = False
            Txt_MeshSize.ReadOnly = True
            'Txt_MeshSize.Enabled = False
            Txt_MeshSize.Text = ""

            ' Forzar que se calcule nuevamente desde UVscale
            Call Txt_colossal_UVScale_TextChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub Txt_colossal_UVScale_TextChanged(sender As Object, e As EventArgs) Handles Txt_colossal_UVScale.TextChanged
        If Chk_CalcUV.Checked Then Exit Sub ' No hacer nada si está en modo automático

        Dim inputSize As String = Txt_colossal_UVScale.Text.Replace(",", ".")

        If Double.TryParse(inputSize, NumberStyles.Any, CultureInfo.InvariantCulture, Nothing) Then
            Dim uvScale As Double = Convert.ToDouble(inputSize, CultureInfo.InvariantCulture)

            If uvScale <> 0 Then
                Dim result As Double = 1 / uvScale
                Txt_MeshSize.Text = result.ToString("0.######", CultureInfo.InvariantCulture)
                Lbl_MeshSizeZ.Text = "x " & result.ToString("0.######", CultureInfo.InvariantCulture)
            Else
                Txt_MeshSize.Text = "-"
                Lbl_MeshSizeZ.Text = "x —"
            End If
        Else
            Txt_MeshSize.Text = "-"
            Lbl_MeshSizeZ.Text = "x —"
        End If
    End Sub

    Private Sub Txt_MeshSize_TextChanged(sender As Object, e As EventArgs) Handles Txt_MeshSize.TextChanged
        If Not Chk_CalcUV.Checked Then Exit Sub

        Dim inputMeshSize As String = Txt_MeshSize.Text.Replace(",", ".")

        If Double.TryParse(inputMeshSize, NumberStyles.Any, CultureInfo.InvariantCulture, Nothing) Then
            Dim meshSize As Double = Convert.ToDouble(inputMeshSize, CultureInfo.InvariantCulture)

            If meshSize <> 0 Then
                Dim uvScale As Double = 1 / meshSize

                Txt_colossal_UVScale.Text = uvScale.ToString("0.######", CultureInfo.InvariantCulture)
                Lbl_MeshSizeZ.Text = "x " & meshSize.ToString("0.######", CultureInfo.InvariantCulture)
            Else
                Txt_colossal_UVScale.Text = "-"
                Lbl_MeshSizeZ.Text = "x —"
            End If
        Else
            Txt_colossal_UVScale.Text = "-"
            Lbl_MeshSizeZ.Text = "x —"
        End If
    End Sub

    ' Decal Scale calculator and controls
    Private Sub Txt_DecalScale_TextChanged(sender As Object, e As EventArgs) Handles Txt_DecalScale.TextChanged
        If Not Chk_KeepAspectRatio.Checked OrElse isUpdating OrElse ignoreDecalEvent Then Exit Sub

        Dim inputScale As String = Txt_DecalScale.Text.Replace(",", ".")

        If inputScale.Length > 8 Then
            inputScale = inputScale.Substring(0, 8)
            Txt_DecalScale.Text = inputScale
            Txt_DecalScale.SelectionStart = Txt_DecalScale.Text.Length
        End If

        If Double.TryParse(inputScale, NumberStyles.Any, CultureInfo.InvariantCulture, Nothing) Then
            Dim ScalePercent As Double = Convert.ToDouble(inputScale, CultureInfo.InvariantCulture)

            If ScalePercent > 0 Then
                Dim newX As Double = baseX * (ScalePercent / 100.0)
                Dim newZ As Double = newX / SavedRatio

                If newX > 0 AndAlso newZ > 0 Then
                    isUpdating = True
                    Txt_X.Text = newX.ToString("0.##", CultureInfo.InvariantCulture)
                    Txt_Z.Text = newZ.ToString("0.##", CultureInfo.InvariantCulture)
                    isUpdating = False
                End If
            End If
        End If
    End Sub
End Class
