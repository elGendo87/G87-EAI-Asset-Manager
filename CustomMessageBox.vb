Public Class CustomMessageBox
    Inherits Form

    Private txtMessage As TextBox
    Private picIcon As PictureBox
    Private panelButtons As FlowLayoutPanel

    Private Sub New(message As String, title As String, buttons As MessageBoxButtons,
                    icon As MessageBoxIcon, Optional customWidth As Integer = 640,
                    Optional customHeight As Integer = 240)

        ' Set form size based on parameters or defaults
        Me.Text = title
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Size = New Size(customWidth, customHeight)
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowIcon = False
        Me.ShowInTaskbar = False

        ' Icon PictureBox
        picIcon = New PictureBox()
        picIcon.Location = New Point(20, 20)
        SetIcon(icon)
        ' Use native icon size, AutoSize mode
        picIcon.SizeMode = PictureBoxSizeMode.StretchImage
        picIcon.Size = New Size(48, 48)
        Me.Controls.Add(picIcon)

        ' Message TextBox with scroll
        txtMessage = New TextBox()
        txtMessage.Multiline = True
        txtMessage.ReadOnly = True
        txtMessage.ScrollBars = ScrollBars.Vertical
        txtMessage.BorderStyle = BorderStyle.None
        txtMessage.BackColor = Me.BackColor
        txtMessage.Text = message
        ' Position to the right of icon with some margin
        txtMessage.Location = New Point(picIcon.Right + 10, 20)
        ' Width fills the rest of the form minus margins
        txtMessage.Size = New Size(Me.ClientSize.Width - txtMessage.Location.X - 20, Me.ClientSize.Height - 80)
        txtMessage.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txtMessage.TabStop = False ' Prevent focus
        Me.Controls.Add(txtMessage)

        ' Buttons panel
        panelButtons = New FlowLayoutPanel()
        panelButtons.FlowDirection = FlowDirection.RightToLeft
        panelButtons.Location = New Point(0, Me.ClientSize.Height - 35)
        panelButtons.Size = New Size(Me.ClientSize.Width - 10, 40)
        panelButtons.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Me.Controls.Add(panelButtons)

        CreateButtons(buttons)
    End Sub

    Private Sub SetIcon(icon As MessageBoxIcon)
        Select Case icon
            Case MessageBoxIcon.Information
                picIcon.Image = SystemIcons.Information.ToBitmap()
            Case MessageBoxIcon.Warning
                picIcon.Image = SystemIcons.Warning.ToBitmap()
            Case MessageBoxIcon.Error
                picIcon.Image = SystemIcons.Error.ToBitmap()
            Case MessageBoxIcon.Question
                picIcon.Image = SystemIcons.Question.ToBitmap()
            Case MessageBoxIcon.None
                picIcon.Image = imageDictionary("LocalCopy") ' Assuming Frm_Main.Icon is set to a custom icon

            Case Else
                picIcon.Visible = False
        End Select
    End Sub

    Private Sub CreateButtons(buttons As MessageBoxButtons)
        Select Case buttons
            Case MessageBoxButtons.OK
                AddButton("OK", DialogResult.OK)
                Me.AcceptButton = panelButtons.Controls(0)
            Case MessageBoxButtons.OKCancel
                AddButton("Cancel", DialogResult.Cancel)
                AddButton("OK", DialogResult.OK)
                Me.AcceptButton = panelButtons.Controls(1)
                Me.CancelButton = panelButtons.Controls(0)
            Case MessageBoxButtons.YesNo
                AddButton("No", DialogResult.No)
                AddButton("Yes", DialogResult.Yes)
                Me.AcceptButton = panelButtons.Controls(1)
            Case MessageBoxButtons.YesNoCancel
                AddButton("Cancel", DialogResult.Cancel)
                AddButton("No", DialogResult.No)
                AddButton("Yes", DialogResult.Yes)
                Me.AcceptButton = panelButtons.Controls(2)
                Me.CancelButton = panelButtons.Controls(0)
            Case Else
                AddButton("OK", DialogResult.OK)
        End Select
    End Sub

    Private Sub AddButton(text As String, result As DialogResult)
        Dim btn As New Button()
        btn.Text = text
        btn.DialogResult = result
        btn.Width = 80
        btn.Margin = New Padding(10, 5, 0, 5)
        AddHandler btn.Click, Sub()
                                  Me.DialogResult = result
                                  Me.Close()
                              End Sub
        panelButtons.Controls.Add(btn)
    End Sub

    Public Shared Function ShowCustom(message As String,
                                     Optional title As String = "Message",
                                     Optional buttons As MessageBoxButtons = MessageBoxButtons.OK,
                                     Optional icon As MessageBoxIcon = MessageBoxIcon.None,
                                     Optional customWidth As Integer = 640,
                                     Optional customHeight As Integer = 240) As DialogResult

        Using dlg As New CustomMessageBox(message, title, buttons, icon, customWidth, customHeight)
            Return dlg.ShowDialog()
        End Using
    End Function



End Class