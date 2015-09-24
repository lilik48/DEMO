Imports System.Data.Entity
Imports System.Transactions
Imports MyNo.Common


Public Class frmMNUILGN100
    Private Sub frmMNUILGN100_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RemoveHandler txtUserCD.TextChanged, AddressOf Me.txtInput_TextChanged
        RemoveHandler txtPassWord.TextChanged, AddressOf Me.txtInput_TextChanged
        AddHandler txtUserCD.TextChanged, AddressOf Me.txtInput_TextChanged
        AddHandler txtPassWord.TextChanged, AddressOf Me.txtInput_TextChanged
    End Sub

    Private Sub btnEnd_Click(sender As Object, e As EventArgs) Handles btnEnd.Click
        Try
            Me.Close()
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub btnChangePass_Click(sender As Object, e As EventArgs) Handles btnChangePass.Click
        frmMNUILGN110.p_intPasswordChangeFlg = 0
        frmMNUILGN110.txtUserCD.Text = String.Empty
        frmMNUILGN110.txtPasswordHistory.Text = String.Empty
        frmMNUILGN110.txtPasswordNew.Text = String.Empty
        frmMNUILGN110.txtPasswordConfirm.Text = String.Empty
        Me.Hide()
        frmMNUILGN110.ShowDialog()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Try
            Dim clsMNUILGN100CTL As New MNUILGN100CTL
            clsMNUILGN100CTL.CheckLogin(txtUserCD.Text, txtPassWord.Text)
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private c_strTextBefore As String = Nothing
    Private c_strTextSelection As Integer = Nothing

    Private Sub txtInput_TextChanged(sender As Object, e As EventArgs)
        If sender.Text.ToString.Equals("") Then
            Return
        End If

        Dim isValid = MNBTCMN100.CheckValidAlphanumeric(sender.Text)
        Try
            If isValid = True Then
                RemoveHandler txtPassWord.TextChanged, AddressOf Me.txtInput_TextChanged
                RemoveHandler txtUserCD.TextChanged, AddressOf Me.txtInput_TextChanged
                sender.Text = c_strTextBefore
                sender.SelectionStart = c_strTextSelection
                AddHandler txtPassWord.TextChanged, AddressOf Me.txtInput_TextChanged
                AddHandler txtUserCD.TextChanged, AddressOf Me.txtInput_TextChanged
            End If
        Catch ex As Exception
            sender.Text = ""
        End Try
    End Sub

    Private Sub txtInput_KeyDown(sender As Object, e As KeyEventArgs) Handles txtUserCD.KeyDown, txtPassWord.KeyDown
        c_strTextBefore = sender.Text
        c_strTextSelection = sender.SelectionStart()
    End Sub
End Class