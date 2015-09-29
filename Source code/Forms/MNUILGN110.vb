Imports MyNo.Common


Public Class frmMNUILGN110
    Public p_intPasswordChangeFlg As Integer = 0
    Private c_strTextBefore As String = Nothing
    Private c_strTextSelection As Integer = Nothing

    Dim clsMNUILGN110CTL As New MNUILGN110CTL

    Private Sub frmMNUILGN110_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            SetLockAndUnlockControl(p_intPasswordChangeFlg)
            If p_intPasswordChangeFlg = 1 Then
                txtUserCD.Text = p_strUserCdLogin
            End If

            RemoveHandler txtUserCD.TextChanged, AddressOf Me.txtUserCD_TextChanged
            RemoveHandler txtPasswordHistory.TextChanged, AddressOf Me.txtPassword_TextChanged
            RemoveHandler txtPasswordNew.TextChanged, AddressOf Me.txtPassword_TextChanged
            RemoveHandler txtPasswordConfirm.TextChanged, AddressOf Me.txtPassword_TextChanged
            AddHandler txtUserCD.TextChanged, AddressOf Me.txtUserCD_TextChanged
            AddHandler txtPasswordHistory.TextChanged, AddressOf Me.txtPassword_TextChanged
            AddHandler txtPasswordNew.TextChanged, AddressOf Me.txtPassword_TextChanged
            AddHandler txtPasswordConfirm.TextChanged, AddressOf Me.txtPassword_TextChanged
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub btnFinish_Click(sender As Object, e As EventArgs) Handles btnFinish.Click
        Try
            Me.Close()
            frmMNUILGN100.Visible = True
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub btnChangePassword_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        Try
            If _
                clsMNUILGN110CTL.CheckInput(txtUserCD.Text, txtPasswordHistory.Text, txtPasswordNew.Text,
                                         txtPasswordConfirm.Text) = False Then
                Return
            End If

            If MNBTCMN100.ShowMessage("MSGVWI00005", "Mật khẩu", "Thay đổi", "") = DialogResult.OK Then
                'Save password
                If clsMNUILGN110CTL.SaveChangePassword(txtUserCD.Text, txtPasswordHistory.Text, txtPasswordNew.Text) Then
                    Me.Hide()
                    frmMNUIMNU100.ShowDialog()
                End If

            End If
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub SetLockAndUnlockControl(ByVal in_intStatus As Integer)
        If in_intStatus = 0 Then 'Not login
            txtUserCD.Enabled = True
            txtPasswordHistory.Enabled = True
            txtPasswordNew.Enabled = True
            txtPasswordConfirm.Enabled = True
            btnChangePassword.Enabled = True
            btnFinish.Enabled = True
        ElseIf in_intStatus = 1 Then 'Login by one time password
            txtUserCD.Enabled = False
            txtPasswordHistory.Enabled = True
            txtPasswordNew.Enabled = True
            txtPasswordConfirm.Enabled = True
            btnChangePassword.Enabled = True
            btnFinish.Enabled = True
        End If
    End Sub

    Private Sub txtUserCD_TextChanged(sender As Object, e As EventArgs)
        If sender.Text.ToString.Equals("") Then
            Return
        End If
        Dim isValid = MNBTCMN100.CheckValidAlphanumeric(sender.Text)
        Try
            If isValid = True Then
                RemoveHandler txtUserCD.TextChanged, AddressOf Me.txtUserCD_TextChanged
                sender.Text = c_strTextBefore
                sender.SelectionStart = c_strTextSelection
                AddHandler txtUserCD.TextChanged, AddressOf Me.txtUserCD_TextChanged
            End If
        Catch ex As Exception
            sender.Text = ""
        End Try
    End Sub

    Private Sub txtPassword_TextChanged(sender As Object, e As EventArgs)
        If sender.Text.ToString.Equals("") Then
            Return
        End If
        Dim isValid = MNBTCMN100.CheckValidAlphanumeric(sender.Text)
        Try
            If isValid = True Then
                RemoveHandler txtPasswordHistory.TextChanged, AddressOf Me.txtPassword_TextChanged
                RemoveHandler txtPasswordNew.TextChanged, AddressOf Me.txtPassword_TextChanged
                RemoveHandler txtPasswordConfirm.TextChanged, AddressOf Me.txtPassword_TextChanged
                sender.Text = c_strTextBefore
                sender.SelectionStart = c_strTextSelection
                AddHandler txtPasswordHistory.TextChanged, AddressOf Me.txtPassword_TextChanged
                AddHandler txtPasswordNew.TextChanged, AddressOf Me.txtPassword_TextChanged
                AddHandler txtPasswordConfirm.TextChanged, AddressOf Me.txtPassword_TextChanged
            End If
        Catch ex As Exception
            sender.Text = ""
        End Try
    End Sub

    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs) _
        Handles txtUserCD.KeyDown, txtPasswordHistory.KeyDown, txtPasswordNew.KeyDown, txtPasswordConfirm.KeyDown
        c_strTextBefore = sender.Text
        c_strTextSelection = sender.SelectionStart()
    End Sub
End Class