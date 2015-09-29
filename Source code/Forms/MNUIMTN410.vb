Imports MyNo.Common

Public Class frmMNUIMTN410
    Public p_strUserCD As String
    Public p_intUserMaintenanceFlag As Integer
    Public p_boolDelFlg As Boolean
    'Private dtLastTimeUpdate As Date?
    Private clsMNUIMTN410CTL As New MNUIMTN410CTL()
    Private clsMNUIMTN400CTL As New MNUIMTN400CTL()
    Private userCurrent As m_user

    Private c_strTextBefore As String = Nothing
    Private c_strTextSelection As Integer = Nothing

    Private Sub frmMNUIMTN410_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.Text = MNBTCMN100.SetTitleScreen()
            Dim appxauthAuthority As m_appxauth = MNBTCMN100.GetAuthorityByApp("MNUIMTN410", p_strAuthorityCdLogin)
            Dim intStatusLock As Integer
            If appxauthAuthority.viewauthflg = 1 AndAlso appxauthAuthority.updauthflg = 0 Then
                intStatusLock = 1
            ElseIf _
                appxauthAuthority.viewauthflg = 1 AndAlso appxauthAuthority.updauthflg = 1 AndAlso
                p_intUserMaintenanceFlag = 0 Then
                intStatusLock = 2
            ElseIf _
                appxauthAuthority.viewauthflg = 1 AndAlso appxauthAuthority.updauthflg = 1 AndAlso
                p_intUserMaintenanceFlag = 1 Then
                intStatusLock = 3
            End If
            SetLockAndUnlockControl(intStatusLock)
            cboAuthorityCD.DataSource = clsMNUIMTN400CTL.GetListAuthorityCDs()
            cboAuthorityCD.DisplayMember = "Name"
            cboAuthorityCD.ValueMember = "Value"
            cboAuthorityCD.SelectedIndex = -1
            ' Incase edit user
            If p_strUserCD <> Nothing AndAlso p_intUserMaintenanceFlag = 1 Then
                userCurrent = clsMNUIMTN410CTL.GetUserByUserCd(p_strUserCD)
                If userCurrent IsNot Nothing Then
                    txtUserCD.Text = p_strUserCD
                    txtName.Text = userCurrent.name
                    cboAuthorityCD.SelectedValue = userCurrent.authoritycd
                    txtOneTimePassword.Text = userCurrent.onetimepassword
                End If
            Else
                txtOneTimePassword.Text = clsMNUIMTN410CTL.RandomString()
            End If
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub


    ''' <summary>
    '''     SetLockAndUnlockControl
    ''' </summary>
    ''' <param name="in_intStatusLock">isLock as Integer.</param>
    ''' <remarks></remarks>
    Private Sub SetLockAndUnlockControl(ByVal in_intStatusLock As Integer)
        If in_intStatusLock = 1 Then

            txtUserCD.Enabled = False
            txtName.Enabled = False
            cboAuthorityCD.Enabled = False
            btnGeneratePassword.Enabled = False
            btnRegister.Enabled = False
            btnDelete.Enabled = False
            btnClose.Enabled = True
        ElseIf in_intStatusLock = 2 Then
            txtUserCD.Enabled = True
            txtName.Enabled = True
            cboAuthorityCD.Enabled = True
            btnGeneratePassword.Enabled = True
            btnRegister.Enabled = True
            btnDelete.Enabled = False
            btnClose.Enabled = True
        ElseIf in_intStatusLock = 3 Then
            txtUserCD.Enabled = False
            txtName.Enabled = True
            cboAuthorityCD.Enabled = True
            btnGeneratePassword.Enabled = True
            btnRegister.Enabled = True
            btnDelete.Enabled = True
            btnClose.Enabled = True

        End If
    End Sub


    Private Sub btnGeneratePassword_Click(sender As Object, e As EventArgs) Handles btnGeneratePassword.Click
        Try
            txtOneTimePassword.Text = clsMNUIMTN410CTL.RandomString()
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        Try
            Dim strUserCd As String = txtUserCD.Text
            Dim strName As String = txtName.Text
            Dim strAuthority As String = cboAuthorityCD.SelectedValue
            Dim strOnetimePassword As String = txtOneTimePassword.Text
            If _
                clsMNUIMTN410CTL.CheckInput(strUserCd, strName, strAuthority, strOnetimePassword,
                                            p_intUserMaintenanceFlag) Then
                If p_intUserMaintenanceFlag = 0 Then
                    If clsMNUIMTN410CTL.CheckUserExist(strUserCd) Then
                        MNBTCMN100.ShowMessage("MSGVWE00002", "Tên đăng nhập", "", "")
                        Return
                    End If
                    If MNBTCMN100.ShowMessage("MSGVWI00001", "Người dùng", "Đăng ký", "") = DialogResult.OK Then
                        clsMNUIMTN410CTL.InsertNewUser(strUserCd, strName, strOnetimePassword, strAuthority)
                        If MNBTCMN100.ShowMessage("MSGVWI00003", "Người dùng", "Đăng ký", "") = DialogResult.OK Then
                            Me.Hide()
                        End If
                    End If

                Else
                    If MNBTCMN100.ShowMessage("MSGVWI00001", "Người dùng", "Đăng ký", "") = DialogResult.OK Then
                        clsMNUIMTN410CTL.UpdateUser(strUserCd, strName, strOnetimePassword, strAuthority)
                        If MNBTCMN100.ShowMessage("MSGVWI00003", "Người dùng", "Đăng ký", "") = DialogResult.OK Then
                            Me.Hide()
                        End If
                    End If

                End If

            End If


        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            Me.Hide()            
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If MNBTCMN100.ShowMessage("MSGVWI00001", "tài khoản", "xóa", "") = DialogResult.OK Then
                If p_intUserMaintenanceFlag = 1 Then
                    clsMNUIMTN410CTL.DeleteUser(p_strUserCD)
                End If
                If MNBTCMN100.ShowMessage("MSGVWI00003", "tài khoản", "xóa", "") = DialogResult.OK Then
                    p_boolDelFlg = True
                    Me.Hide()
                End If
            End If

        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub txtUserCD_TextChanged(sender As Object, e As EventArgs) Handles txtUserCD.TextChanged
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

    Private Sub txtInput_KeyDown(sender As Object, e As KeyEventArgs) Handles txtUserCD.KeyDown, txtName.KeyDown
        c_strTextBefore = sender.Text
        c_strTextSelection = sender.SelectionStart()
    End Sub

    Private Sub txtName_TextChanged(sender As Object, e As EventArgs) Handles txtName.TextChanged
        If sender.Text.ToString.Equals("") Then
            Return
        End If
        Dim isValid = MNBTCMN100.checkLength(sender.Text, sender.MaxLength)
        If isValid = False Then
            sender.Text = c_strTextBefore
            Exit Sub
        End If
        Try
            If isValid = False Then
                RemoveHandler txtName.TextChanged, AddressOf Me.txtName_TextChanged
                sender.Text = c_strTextBefore
                sender.SelectionStart = c_strTextSelection
                AddHandler txtName.TextChanged, AddressOf Me.txtName_TextChanged
            End If
            c_strTextBefore = sender.Text
        Catch ex As Exception
            sender.Text = ""
        End Try
    End Sub
End Class