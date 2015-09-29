Imports MyNo.Common


Public Class frmMNUIMTN400

    Private c_strTextBefore As String = Nothing
    Private c_strTextSelection As Integer = Nothing

    Private Sub frmMNUIMTN400_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim appxauthAuthority As m_appxauth
            If p_strUserCdLogin <> Nothing Then
                Me.Text = MNBTCMN100.SetTitleScreen()
            End If
            'Call common function get authority view and update of this screen
            appxauthAuthority = MNBTCMN100.GetAuthorityByApp("MNUIMTN410", p_strAuthorityCdLogin)
            If appxauthAuthority.viewauthflg = 1 AndAlso appxauthAuthority.updauthflg = 1 Then
                SetLockAndUnlockControl(False)
            Else
                If appxauthAuthority.updauthflg = 0 Then
                    SetLockAndUnlockControl(True)
                End If
            End If
            ' Delete ThongTH 20150903: issue 改善 #34062
            'MNBTCMN100.CbbInitEvent(cboAuthorityCD)

            ' Set data to cboAuthorityCD.ValueMember And cboAuthorityCD.DisplayMember
            Dim clsMNUIMTN400CTL As New MNUIMTN400CTL()
            cboAuthorityCD.DataSource = clsMNUIMTN400CTL.GetListAuthorityCDs()
            cboAuthorityCD.DisplayMember = "Name"
            cboAuthorityCD.ValueMember = "Value"
            cboAuthorityCD.SelectedIndex = -1
            ''set default dgvUser
            'dgvUser.RowCount = 20
            'For i = 0 To dgvUser.RowCount - 1
            '    dgvUser.Rows(i).Cells("詳細") = New DataGridViewTextBoxCell
            'Next
            dgvUser.ClearSelection()
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    ''' <summary>
    ''' SetLockAndUnlockControl
    ''' </summary>
    ''' <param name="isLock">isLock as boolean.</param>
    ''' <remarks></remarks>

    Private Sub SetLockAndUnlockControl(ByVal isLock As Boolean)
        If isLock = False Then
            btnAddNew.Enabled = True
        Else
            btnAddNew.Enabled = False

        End If
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim strUserCD As String
        Dim strUserName As String
        Dim strAuthorityCd As String
        Dim clsMNUIMTN400CTL As New MNUIMTN400CTL()
        Try

            strUserCD = txtUserCD.Text
            strUserName = txtName.Text
            strAuthorityCd = cboAuthorityCD.SelectedValue
            ' Set data to dgvUser
            Dim lstUser = clsMNUIMTN400CTL.GetListUsers(strUserCD, strUserName, strAuthorityCd)
            Dim row As Integer = 0
            dgvUser.Rows.Clear()
            dgvUser.RowCount = lstUser.Count
            For Each Item In lstUser
                dgvUser.Rows(row).Cells("UserCD").Value = Item.UserCD
                dgvUser.Rows(row).Cells("UserName").Value = Item.UserName
                dgvUser.Rows(row).Cells("AuthorityName").Value = Item.AuthorityName
                dgvUser.Rows(row).Cells("AuthorityCD").Value = Item.AuthorityCD
                row = row + 1
                'dgvUser.Rows(row).Cells("AuthorityCD").Value = Item.AuthorityCD
            Next
            'dgvUser.DataSource = clsMNUIMTN400CTL.GetListUsers(strUserCD, strUserName, strAuthorityCd)
            'If dgvUser.ColumnCount = 5 Then
            '    dgvUser.Columns("AUTHORITYCD").Visible = False
            'End If
            If dgvUser.RowCount = 0 Then
                MNBTCMN100.ShowMessage("MSGVWE00017", "", "", "")
            End If

            'If dgvUser.RowCount > 20 Then
            '    dgvUser.Columns("AuthorityName").Width = 300 - 16
            'Else
            '    dgvUser.Columns("AuthorityName").Width = 300
            'End If
            'With dgvUser
            '    For i = 0 To dgvUser.RowCount - 1
            '        If String.IsNullOrEmpty(.Rows(i).Cells(1).Value) Then
            '            .Rows(i).Cells(0) = New DataGridViewTextBoxCell
            '        End If
            '    Next
            'End With
            dgvUser.ClearSelection()
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub dgvUser_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUser.CellContentClick
        If e.RowIndex = -1 Then Exit Sub
        If e.ColumnIndex = 3 Then
            Dim strSelectedUserCd As String
            Try
                strSelectedUserCd = dgvUser.Rows(e.RowIndex).Cells(0).Value
                'MapPV update 2015/08/24 : check delete
                Dim clsMNUILGN100CTL As MNUILGN100CTL = New MNUILGN100CTL
                Dim userCheck = clsMNUILGN100CTL.GetUserMasterByUserCD(strSelectedUserCd)
                If Not userCheck Is Nothing Then
                    Dim frmMNUIMTN410 As New frmMNUIMTN410
                    frmMNUIMTN410.p_strUserCD = strSelectedUserCd
                    frmMNUIMTN410.p_intUserMaintenanceFlag = 1
                    frmMNUIMTN410.ShowDialog()
                    If frmMNUIMTN410.p_boolDelFlg Then
                        btnSearch.PerformClick()
                    End If
                End If

            Catch ex As Exception
                MNBTCMN100.ShowMessageException()
            End Try
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        Try
            txtUserCD.Text = ""
            txtName.Text = ""
            cboAuthorityCD.SelectedIndex = -1
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Try
            Me.Close()
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click
        Try
            Dim frmMNUIMTN410 As New frmMNUIMTN410
            frmMNUIMTN410.p_intUserMaintenanceFlag = 0
            frmMNUIMTN410.ShowDialog()
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
        Dim isValid = MNBTCMN100.checkLength(sender.Text, 80)
        Try
            If Not isValid Then
                RemoveHandler txtName.TextChanged, AddressOf txtName_TextChanged
                sender.Text = c_strTextBefore
                sender.SelectionStart = c_strTextSelection
                AddHandler txtName.TextChanged, AddressOf txtName_TextChanged
            End If
        Catch ex As Exception
            sender.Text = ""
        End Try
    End Sub

    'Private Sub dgvUser_Enter(sender As Object, e As EventArgs) Handles dgvUser.Enter
    '    If dgvUser.RowCount <> 0 Then
    '        dgvUser.CurrentCell = dgvUser.Rows(0).Cells(4)
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' Set tabindex in dgv
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub dgvUser_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvUser.KeyDown
    '    If dgvUser.CurrentCell IsNot Nothing AndAlso e.KeyValue = Keys.Tab Then
    '        If dgvUser.CurrentCell.ColumnIndex = 0 Then
    '            dgvUser.CurrentCell = Nothing
    '            dgvUser.ClearSelection()
    '            btnAddNew.Focus()
    '        Else
    '            dgvUser.CurrentCell = dgvUser.Rows(dgvUser.CurrentCell.RowIndex).Cells(4)
    '        End If
    '    End If
    'End Sub
End Class