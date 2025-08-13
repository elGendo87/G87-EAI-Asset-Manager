Imports System.Diagnostics
Public Class Frm_About
    Private Sub Frm_About_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Text = "About " & Frm_Main.Text
        Me.Icon = Frm_Main.Icon

        Btn_Close.Image = imageDictionary("Close")

    End Sub

    Private Sub Btn_Close_Click(sender As Object, e As EventArgs) Handles Btn_Close.Click
        Me.Close()
    End Sub

    Private Sub Lbl_DiscordLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles Lbl_DiscordLink.LinkClicked
        Dim url As String = "https://discord.gg/fr7EKdY8V6"
        Try
            Dim psi As New ProcessStartInfo With {
            .FileName = url,
            .UseShellExecute = True
        }
            Process.Start(psi)
        Catch ex As Exception
            MessageBox.Show("Failed to open de link =(" & vbCrLf & ex.Message)
        End Try
    End Sub

End Class