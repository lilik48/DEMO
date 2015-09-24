'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNUIIMT100
'*  機能名称：Người dùng検索画面
'*  処理　　：Người dùng検索画面
'*  内容　　：Người dùng検索画面
'*  ファイル：MNUIIMT100.vb
'*  備考　　：
'*
'*  Created：2015/07/15 RS. ThienNQ
'***************************************************************************************
Imports MyNo.Common
Imports MyNo.MNUIIMT100CLT
Imports System.Threading

Public Class frmMNUIIMT100
    'Get at file ini
    Private c_clsMT100CLT As MNUIIMT100CLT = New MNUIIMT100CLT(Me)

    Private c_strTextBefore As String = Nothing
    Private c_strTextSelection As Integer = Nothing
    Private c_lstCompany As List(Of MNUIIMT100Model)
    Private c_trd As Thread

    Private Sub frmMNUIIMT100_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim authority As m_appxauth
            'Call common function get authority view and update of this screen
            authority = MNBTCMN100.GetAuthorityByApp("MNUIIMT110", p_strAuthorityCdLogin)
            If authority Is Nothing OrElse authority.updauthflg = 0 Then
                ControlEnable(0)
            Else
                ControlEnable(1)
            End If
            Me.Text = MNBTCMN100.SetTitleScreen()
            InitForm()

        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    ''' <summary>
    ''' Check before search data
    ''' </summary>
    ''' <param name="ctl">Control</param>
    ''' <returns>True, False</returns>
    ''' <remarks></remarks>
    Private Function CheckInput(ByVal ctl As Control) As Boolean
        If ctl.Text.Trim() = "" Then
            Return True
        End If
        If ctl.Name = "txtBranchNo" Then
            ' Not number
            If Not IsNumeric(ctl.Text) Then
                MNBTCMN100.ShowMessage("MSGVWE00015", "枝番", "数値", "")
                'ctl.Focus()
                Return False
            End If
        End If
        Return True
    End Function

    Private Sub SetTitleForm()
        If p_strUserCdLogin <> Nothing Then
            Me.Text = Replace(Me.Text, "op00001", p_strUserCdLogin)
        End If
    End Sub

    ''' <summary>
    ''' Load Form Default
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitForm()

        With dtgrvCompany
            .ColumnCount = 0
            .ColumnCount = 11
            .Columns(0).Width = 100
            .Columns(0).HeaderCell.Value = "企業コード"
            .Columns(1).Width = 230
            .Columns(1).HeaderText = "企業名"
            .Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .Columns(2).HeaderText = "枝番"
            .Columns(3).Width = 220
            .Columns(3).HeaderText = "枝番名"
            .Columns(4).Width = 100
            .Columns(4).HeaderText = "Đăng ký日"
            .Columns(5).Width = 100
            .Columns(5).HeaderText = "納品予定日"
            .Columns(6).Width = 100
            .Columns(6).HeaderText = "納品日"
            .Columns(7).Width = 100
            .Columns(7).HeaderText = "削除予定日" ' Change spec 削除日 --> 削除予定日
            .Columns(8).Width = 80
            .Columns(8).HeaderText = "進捗率"
            .Columns(9).Width = 93
            .Columns(9).HeaderText = "詳細確認"
            .Columns(10).Width = 93
            .Columns(10).HeaderText = "進捗確認"

            Dim i = 0
            For i = 0 To .ColumnCount - 1
                .Columns(i).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns(i).HeaderCell.Style.Font = New Font("MS PGothic", 11, FontStyle.Regular)
                .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Columns(i).SortMode = SortOrder.None
            Next
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(10).AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            .DefaultCellStyle.Font = New Font("MS Mincho", 11)
        End With

    End Sub


    ''' <summary>
    ''' Change state btnAddNew
    ''' </summary>
    ''' <param name="in_intUpdate"></param>
    ''' <remarks></remarks>
    Private Sub ControlEnable(ByVal in_intUpdate As Integer)
        If in_intUpdate = 1 Then
            btnAddNew.Enabled = True
        Else
            btnAddNew.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Close Form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            Me.Close()
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            '①企業コード、枝番、企業名、枝番名が全て未入力	
            '・③の可変抽出条件は無し。
            ' Check brandno is numberic
            If CheckInput(txtBranchNo) = False Then
                Exit Sub
            End If

            '②共通抽出条件　　※抽出条件は全てAndで結合する。	
            '・企業情報の削除フラグ = 0
            '2 điều kiện extract chung   * Kết hợp toàn bộ điều kiện extract bằng And	
            '. flag xóa trong thông tin công ty = 0
            Dim strSQLMessage As String = String.Empty
            c_lstCompany = c_clsMT100CLT.GetDataCompany(txtCompanyCD.Text, txtCompanyName.Text, txtBranchNo.Text, txtBranchName.Text, strSQLMessage)

            LoadDtgrv(c_lstCompany)

            ' Write log
            c_clsMT100CLT.Log(strSQLMessage, c_lstCompany.Count)

        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    ''' <summary>
    ''' Fill data to datagridview
    ''' [Todo Chưa test phần hiển thị màu khi quá hạn]
    ''' </summary>
    ''' <param name="in_lstListCompany"></param>
    ''' <remarks></remarks>
    Private Function LoadDtgrv(ByVal in_lstListCompany As Object) As Boolean
        With dtgrvCompany
            .RowCount = 0

            'not found, display message
            If in_lstListCompany.Count = 0 Then
                MNBTCMN100.ShowMessage("MSGVWE00017", "", "", "") 'todo Không tồn tại message trong ini
                Return False
            End If

            .RowCount = in_lstListCompany.Count()
            Dim mynoEntities = New mynoEntities()
            Dim currentTime As Date = MNBTCMN100.GetCurrentTimestamp(mynoEntities).ToString("yyyy/MM/dd")
            For i = 0 To in_lstListCompany.Count() - 1
                .Rows(i).Cells(0).Value = in_lstListCompany(i).companycd
                .Rows(i).Cells(1).Value = in_lstListCompany(i).companyname
                .Rows(i).Cells(2).Value = in_lstListCompany(i).companybranchno
                .Rows(i).Cells(3).Value = in_lstListCompany(i).companybranchname
                If Not String.IsNullOrEmpty(in_lstListCompany(i).adddatetime) Then
                    .Rows(i).Cells(4).Value = MNBTCMN100.CnvDateYYYYMMDD(in_lstListCompany(i).adddatetime.ToString())
                End If
                
                '納品予定日
                If IsNothing(in_lstListCompany(i).delivschdate) = False Then
                    .Rows(i).Cells(5).Value = MNBTCMN100.CnvDateYYYYMMDD(in_lstListCompany(i).delivschdate.ToString())
                    '納品日に値が設定されていない場合に背景色の設定を行う。
                    If IsNothing(in_lstListCompany(i).delivdate) Then
                        If CDate(in_lstListCompany(i).delivschdate).ToString("yyyy/MM/dd") < currentTime Then
                            .Rows(i).Cells(5).Style.BackColor = Color.Red
                        ElseIf CDate(in_lstListCompany(i).delivschdate) <= currentTime.AddDays(7) Then
                            .Rows(i).Cells(5).Style.BackColor = Color.Yellow
                        End If
                    End If
                End If

                '納品日
                If IsNothing(in_lstListCompany(i).delivdate) Then   '納品日が設定されていない場合、納品予定日がシステム日付を過ぎていいれば背景書を変更
                    If IsNothing(in_lstListCompany(i).delivschdate) = False AndAlso CDate(in_lstListCompany(i).delivschdate).ToString("yyyy/MM/dd") < currentTime Then
                        .Rows(i).Cells(6).Style.BackColor = Color.LightPink
                    End If
                Else
                    '納品予定日よりも納品日が遅い場合、背景色を変更
                    .Rows(i).Cells(6).Value = MNBTCMN100.CnvDateYYYYMMDD(in_lstListCompany(i).delivdate.ToString())
                    'If CDate(in_lstListCompany(i).delivdate).ToString("yyyy/MM/dd") < currentTime Then
                    If CDate(in_lstListCompany(i).delivdate).ToString("yyyy/MM/dd") > CDate(in_lstListCompany(i).delivschdate).ToString("yyyy/MM/dd") Then
                        .Rows(i).Cells(6).Style.BackColor = Color.LightPink
                    End If
                End If

                '削除予定日
                If IsNothing(in_lstListCompany(i).delschdate) = False Then
                    .Rows(i).Cells(7).Value = MNBTCMN100.CnvDateYYYYMMDD(in_lstListCompany(i).delschdate.ToString)
                End If

                '進捗率
                .Rows(i).Cells(8).Value = IIf(in_lstListCompany(i).sumall = 0, 0, (Math.Round(in_lstListCompany(i).rate / in_lstListCompany(i).sumall, 2, MidpointRounding.AwayFromZero)) * 100).ToString() + "%"
                '詳細
                Dim linkCell9 As DataGridViewLinkCell = New DataGridViewLinkCell()
                linkCell9.Value = "詳細"
                .Rows(i).Cells(9) = linkCell9
                '進捗
                Dim linkCell10 As DataGridViewLinkCell = New DataGridViewLinkCell()
                linkCell10.Value = "進捗"
                .Rows(i).Cells(10) = linkCell10
            Next

            If .RowCount > 0 Then
                '.CurrentCell = .Rows(0).Cells(9)
                .ClearSelection()
            End If

            If .RowCount > 22 Then
                .Columns(0).Width = 100 - 16
            Else
                .Columns(0).Width = 100
            End If

            Return True
        End With

    End Function

    ''' <summary>
    ''' Click Cell labelLink
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dtgrvCompany_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgrvCompany.CellContentClick
        'Try           
        '    If e.RowIndex = -1 Or dtgrvCompany.CurrentCell Is Nothing Then Exit Sub
        '    ' click hyperlink 詳細
        '    If e.ColumnIndex = 9 Then
        '        Dim frmMNUIIMT110 As frmMNUIIMT110 = New frmMNUIIMT110
        '        frmMNUIIMT110.p_strCompanyCD = dtgrvCompany(0, e.RowIndex).Value
        '        frmMNUIIMT110.p_intCopanyBranchNo = dtgrvCompany(2, e.RowIndex).Value
        '        frmMNUIIMT110.p_blnFlag = 2
        '        frmMNUIIMT110.p_CompanyList = c_lstCompany
        '        frmMNUIIMT110.ShowDialog()
        '    ElseIf e.ColumnIndex = 10 Then
        '        Dim frmMNUIIMT120 As frmMNUIIMT120 = New frmMNUIIMT120
        '        frmMNUIIMT120.CompanyCode = dtgrvCompany(0, e.RowIndex).Value
        '        frmMNUIIMT120.BranchCode = dtgrvCompany(2, e.RowIndex).Value
        '        frmMNUIIMT120.ShowDialog()
        '    End If
        'Catch ex As Exception
        '    MNBTCMN100.ShowMessageException()
        'End Try
    End Sub

    Private Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click
        ''企業情報Đăng ký・照会画面へ遷移する。
        'Try
        '    Dim frmMNUIIMT110 As frmMNUIIMT110 = New frmMNUIIMT110
        '    'set flag moving is 1
        '    frmMNUIIMT110.p_blnFlag = 1
        '    frmMNUIIMT110.p_CompanyList = c_lstCompany
        '    frmMNUIIMT110.ShowDialog()
        'Catch ex As Exception
        '    MNBTCMN100.ShowMessageException()
        'End Try
    End Sub

    ''' <summary>
    ''' Clear txtCompanyCD, txtCompanyName, txtBranchNo, txtBranchName
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtCompanyCD.Text = Nothing
        txtCompanyName.Text = Nothing
        txtBranchNo.Text = Nothing
        txtBranchName.Text = Nothing
    End Sub

    Private Sub btnOutputList_Click(sender As Object, e As EventArgs) Handles btnOutputList.Click
        Try
            '①企業コード、枝番、企業名、枝番名が全て未入力	
            '・③の可変抽出条件は無し。
            ' Check brandno is numberic
            If CheckInput(txtBranchNo) = False Then
                Exit Sub
            End If

            'display confirm message
            If MNBTCMN100.ShowMessageConfirm("MSGVWI00001", "danh sách", "xuất", "") = DialogResult.OK Then
                ' Clicked button OK
                ' Open loading screen
                c_trd = New Thread(AddressOf frmLoading_Shown)
                c_trd.IsBackground = True
                c_trd.Start()
                frmLoading.ShowDialog()
            End If
        Catch ex As Exception
            frmLoading.Close()
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    ''' <summary>
    ''' Check validate txt
    ''' [Todo Tag = 1 yet set]
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CheckInputText(sender As Object, e As EventArgs) Handles txtBranchNo.TextChanged
        If sender.Text.ToString.Equals("") Then
            Return
        End If
        If Not MNBTCMN100.checkLength(sender.Text, sender.MaxLength) Then
            sender.Text = c_strTextBefore
            Exit Sub
        End If
        Dim isValid As Boolean = False
        Select Case sender.Tag
            Case 0
                isValid = MNBTCMN100.CheckValidAlphanumeric(sender.Text)
            Case 1
                'isValid = MNBTCMN100.CheckValidateOnlyKatakanaHalfsize(sender.Text)
        End Select

        Try
            If isValid = True Then
                RemoveHandler DirectCast(sender, Control).TextChanged, AddressOf Me.CheckInputText
                sender.Text = c_strTextBefore
                sender.SelectionStart = c_strTextSelection
                AddHandler DirectCast(sender, Control).TextChanged, AddressOf Me.CheckInputText
            End If
        Catch ex As Exception
            sender.Text = ""
        End Try
    End Sub

    ''' <summary>
    ''' Get txt.Value before change text
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub GetOldTextOfTextBox(sender As Object, e As EventArgs) Handles txtCompanyCD.KeyDown, txtCompanyName.KeyDown, txtBranchNo.KeyDown, txtBranchName.KeyDown
        c_strTextBefore = sender.Text
        c_strTextSelection = sender.SelectionStart()
    End Sub

    Private Sub txtCompanyCD_TextChanged(sender As Object, e As EventArgs) Handles txtCompanyCD.TextChanged, txtCompanyName.TextChanged, txtBranchName.TextChanged
        If sender.Text.ToString.Equals("") Then
            Return
        End If
        ' Check input length
        If Not MNBTCMN100.checkLength(sender.Text, sender.MaxLength) Then
            sender.Text = c_strTextBefore
            Exit Sub
        End If
        ' Check full number
        If sender.Name = "txtCompanyCD" Or sender.Name = "txtBranchNo" Then

            Dim isValid = MNBTCMN100.CheckValidAlphanumeric(sender.Text)

            Try
                If isValid = True Then
                    RemoveHandler DirectCast(sender, TextBox).TextChanged, AddressOf Me.txtCompanyCD_TextChanged
                    sender.Text = c_strTextBefore
                    sender.SelectionStart = c_strTextSelection
                    AddHandler DirectCast(sender, TextBox).TextChanged, AddressOf Me.txtCompanyCD_TextChanged
                End If
            Catch ex As Exception
                sender.Text = ""
            End Try
        End If
        c_strTextBefore = sender.Text
    End Sub

    Private Sub frmLoading_Shown()
        Try
            Application.DoEvents()
            ' call function export data
            c_clsMT100CLT.ExportData(txtCompanyCD.Text, txtCompanyName.Text, txtBranchNo.Text, txtBranchName.Text)
            Me.BeginInvoke(New Action(Sub() frmLoading.Close()))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            '①企業コード、枝番、企業名、枝番名が全て未入力	
            '・③の可変抽出条件は無し。
            ' Check brandno is numberic
            If CheckInput(txtBranchNo) = False Then
                Exit Sub
            End If

            'display confirm message
            If MNBTCMN100.ShowMessageConfirm("MSGVWI00001", "danh sách", "xuất", "") = DialogResult.OK Then
                ' Clicked button OK
                ' Open loading screen
                c_trd = New Thread(AddressOf frmLoading_IMP)
                c_trd.IsBackground = True
                c_trd.Start()
                frmLoading.ShowDialog()
            End If
        Catch ex As Exception
            frmLoading.Close()
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub frmLoading_IMP()
        
    End Sub

End Class