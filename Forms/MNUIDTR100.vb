'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNUIDTR100
'*  機能名称：基礎データ取込画面
'*  処理　　：基礎データ取込画面
'*  内容　　：基礎データ取込画面
'*  ファイル：MNUIDTR100.vb
'*  備考　　：
'*
'*  Created：2015/07/01
'***************************************************************************************
Imports MyNo.Common
Imports System.Threading

Public Class frmMNUIDTR100

    'Number record paging
    Private p_intNumItem As Integer
    Private p_MNUIDTR100CTL As New MNUIDTR100CTL(Me)
    Private p_mynoContext As mynoEntities
    Private c_strTextBefore As String = Nothing
    Private c_strTextSelection As Integer = Nothing
    Private oldUrlImport As String = MNBTCMN100.GetConfig("Import_Dir_CSV", "config.ini")
    Private p_intfileType As Integer
    Private p_intimportType As Integer

    Private c_trd As Thread

    ''' <summary>
    ''' SetLockAndUnlockControl
    ''' </summary>
    ''' <param name="in_intStatus">status as Integer.</param>
    ''' <remarks></remarks>
    Private Sub SetLockAndUnlockControl(ByVal in_intStatus As Integer)
        If in_intStatus = 0 Then
            txtCompanyCD.Enabled = False
            txtCompanyBranchCD.Enabled = False
            optFileType0.Enabled = False
            optFileType1.Enabled = False
            optImportType0.Enabled = False
            optImportType1.Enabled = False
            txtFilePath.Enabled = False
            btnFileChoose.Enabled = False
            btnImport.Enabled = False
            btnNext.Enabled = True
            btnPrevious.Enabled = True
            btnClose.Enabled = True

        ElseIf in_intStatus = 1 Then
            txtCompanyCD.Enabled = True
            txtCompanyBranchCD.Enabled = True
            optFileType0.Enabled = True
            optFileType1.Enabled = True
            optImportType0.Enabled = True
            optImportType1.Enabled = True
            txtFilePath.Enabled = True
            btnFileChoose.Enabled = True
            btnImport.Enabled = True
            btnNext.Enabled = True
            btnPrevious.Enabled = True
            btnClose.Enabled = True

        End If
    End Sub
    ''' <summary>
    ''' Load Form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MNUIDTR100_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim authority As m_appxauth
        'Set title for screen
        If p_strUserCdLogin <> Nothing Then
            Me.Text = MNBTCMN100.SetTitleScreen()
        End If
        ' Set data for dgvImportLog Have Paging
        p_intNumItem = MNBTCMN100.GetConfig("MaxLine_Gridview_BasicDataImport", "config.ini")
        lblPage.Text = 0
        dgvImportLog.DataSource = p_MNUIDTR100CTL.GetDataImportLogsPaging(p_intNumItem, CInt(lblPage.Text), True)
        If dgvImportLog.DataSource IsNot Nothing Then
            lblPage.Text = 1
        End If

        'Call common function get authority view and update of this screen
        authority = MNBTCMN100.GetAuthorityByApp("MNUIDTR100", p_strAuthorityCdLogin)
        'Set status item on screen
        If authority.viewauthflg = 1 And authority.updauthflg = 0 Then
            SetLockAndUnlockControl(0)
        Else
            If authority.viewauthflg = 1 And authority.updauthflg = 1 Then
                SetLockAndUnlockControl(1)
            End If
        End If
        dgvImportLog.ClearSelection()
        If p_intNumItem > 17 Then
            dgvImportLog.Columns(1).Width = dgvImportLog.Columns(1).Width - 17
        End If
    End Sub

    ''' <summary>
    ''' Event change page of datagridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub changePage(sender As Object, e As EventArgs) Handles btnPrevious.Click, btnNext.Click
        p_intNumItem = MNBTCMN100.GetConfig("MaxLine_Gridview_BasicDataImport", "config.ini")
        Select Case sender.Tag
            Case 0
                If CInt(lblPage.Text) > 1 Then
                    dgvImportLog.DataSource = p_MNUIDTR100CTL.GetDataImportLogsPaging(p_intNumItem, CInt(lblPage.Text), False)
                    lblPage.Text = CInt(lblPage.Text) - 1
                End If
            Case 1
                If (p_intNumItem > 15 AndAlso dgvImportLog.RowCount = p_intNumItem) Or (p_intNumItem < 15 AndAlso String.IsNullOrEmpty(dgvImportLog.Rows(p_intNumItem - 1).Cells(0).Value) = False) Then
                    Dim obj = p_MNUIDTR100CTL.GetDataImportLogsPaging(p_intNumItem, CInt(lblPage.Text), True)
                    If obj IsNot Nothing And obj.Count > 0 Then
                        dgvImportLog.DataSource = obj
                        lblPage.Text = CInt(lblPage.Text) + 1
                    End If
                End If
        End Select
        dgvImportLog.ClearSelection()
    End Sub

    ''' <summary>
    ''' File Choose Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnFileChoose_Click(sender As Object, e As EventArgs) Handles btnFileChoose.Click
        Dim dialog As New OpenFileDialog()

        dialog.InitialDirectory = oldUrlImport
        dialog.Filter = "csv files (*.csv)|*.csv|CSV files (*CSV)|*.CSV"
        dialog.RestoreDirectory = True

        If dialog.ShowDialog() = DialogResult.OK Then
            txtFilePath.Text = dialog.FileName
            oldUrlImport = dialog.FileName
        End If
    End Sub

    ''' <summary>
    ''' Button Import data click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click

        If optFileType0.Checked Then
            'If radiobutton 個人 checkked
            p_intfileType = 1
        ElseIf optFileType1.Checked Then
            'If radiobutton 家族 checkked
            p_intfileType = 2
        End If
        If optImportType0.Checked Then
            'If radiobutton 全件(削除追加) checkked
            p_intimportType = 1
        ElseIf optImportType1.Checked Then
            'If radiobutton 新規追加のみ checkked
            p_intimportType = 2
        End If
        'Check input data on screen
        If p_MNUIDTR100CTL.CheckInputSingle(txtCompanyCD, txtCompanyBranchCD, txtFilePath) Then
            'Check input Correlation
            If p_MNUIDTR100CTL.CheckInputCorrelation(txtCompanyCD.Text, txtCompanyBranchCD.Text, txtFilePath.Text, p_intfileType) Then
                If MNBTCMN100.ShowMessage("MSGVWI00001", "指定ファイルの取込", "実行", "") = DialogResult.OK Then
                    ''Select OK button
                    'c_trd = New Thread(AddressOf frmLoading_Shown)
                    'c_trd.IsBackground = True
                    'c_trd.Start()
                    'frmLoading.ShowDialog()

                    AddHandler frmLoading.Shown, AddressOf frmLoading_Shown
                    frmLoading.ShowDialog()
                    RemoveHandler frmLoading.Shown, AddressOf frmLoading_Shown
                End If
            End If
            txtFilePath.Text = String.Empty
            txtCompanyCD.Text = String.Empty
            txtCompanyBranchCD.Text = String.Empty
        End If

        GC.Collect()

    End Sub

    Private Sub frmLoading_Shown(sender As Object, e As EventArgs)
        Try
            Application.DoEvents()
            Cursor.Current = Cursors.WaitCursor
            'Read file csv, check error file. If error > 0 insert log to DB and show message Else import data to DB
            Dim errFlg As Integer = 0
            Dim msg As String = "取込"
            If p_MNUIDTR100CTL.ProcessDataBasic(txtCompanyCD.Text, txtCompanyBranchCD.Text, txtFilePath.Text, p_intfileType, p_intimportType, errFlg) Then
                If errFlg = 1 Then
                    msg = "取込（警告有り）"
                End If
                MNBTCMN100.ShowMessage("MSGVWI00003", "基礎データ", msg, "")
            End If
            'Load form again
            MNUIDTR100_Load(sender, e)

            Cursor.Current = Cursors.Default
        Catch ex As Exception
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Cell Click dgvImportLog
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvImportLog_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvImportLog.CellClick
        Dim rowIndex As Integer = e.RowIndex
        Dim index As Integer = e.ColumnIndex
        If rowIndex <> -1 Then
            If index = 3 Then
                If MNBTCMN100.ShowMessage("MSGVWI00001", "インポートファイル", "ダウンロード", "") = DialogResult.OK Then
                    'display loading screen
                    frmLoading.Show()
                    Application.DoEvents()
                    Dim strSeq As Integer
                    'Get seq of row click
                    strSeq = dgvImportLog.Rows(e.RowIndex).Cells(0).Value
                    If p_MNUIDTR100CTL.ExportCsv(strSeq) Then
                        MNBTCMN100.ShowMessage("MSGVWI00003", "基礎データ", "出力", "")
                    End If
                    frmLoading.Close()
                End If
            ElseIf index = 9 Then
                'Get value of row エラーリスト(明細)
                Dim results = dgvImportLog.Rows(e.RowIndex).Cells(9).Value
                If results IsNot Nothing Then
                    If MNBTCMN100.ShowMessage("MSGVWI00001", "エラーリスト", "ダウンロード", "") = DialogResult.OK Then
                        Using mynoEntities As New mynoEntities
                            Dim seq As Integer
                            'Get seq of row click
                            seq = dgvImportLog.Rows(e.RowIndex).Cells(0).Value
                            'display loading screen
                            frmLoading.Show()
                            Application.DoEvents()
                            Dim queryOut As String = String.Empty
                            Dim getListData = p_MNUIDTR100CTL.GetDataForExportXlsx(queryOut, mynoEntities, seq)

                            'Check result data
                            If getListData.Count > 0 And getListData IsNot Nothing Then
                                Dim filePathSave As String = String.Empty
                                Dim result As Boolean = p_MNUIDTR100CTL.ExportXlsxListError(getListData, mynoEntities, queryOut, seq, filePathSave)
                                If result = True Then
                                    frmLoading.Close()
                                    'MNBTCMN100.ShowMessage("MSGVWI00003", "エラーリスト", "出力", "")
                                    If MNBTCMN100.ShowMessageConfirm("MSGVWI00008", "エラーリスト", "", "") = DialogResult.OK Then
                                        Try
                                            Process.Start(filePathSave)
                                        Catch ex As Exception
                                            MNBTCMN100.ShowMessageException()
                                        End Try
                                    End If
                                End If
                            End If
                            frmLoading.Close()
                        End Using
                    End If
                End If
            End If
        End If
    End Sub
    ''' <summary>
    ''' Close Form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub
    ''' <summary>
    ''' frmMNUIDTR100 Paint
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmMNUIDTR100_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        Dim myGraphics As Graphics = Me.CreateGraphics
        Dim myPen As Pen
        myPen = New Pen(Drawing.Color.Black, 1)
        myGraphics.DrawRectangle(myPen, 30, 134, 319, 48)
        myGraphics.DrawRectangle(myPen, 370, 134, 319, 48)
    End Sub

    Private Sub txtInput_TextChanged(sender As Object, e As EventArgs) Handles txtCompanyCD.TextChanged, txtCompanyBranchCD.TextChanged
        If sender.Text.ToString.Equals("") Then
            Return
        End If
        Dim isValid = MNBTCMN100.CheckValidAlphanumeric(sender.Text)
        Try
            If isValid = True Then
                RemoveHandler txtCompanyCD.TextChanged, AddressOf Me.txtInput_TextChanged
                RemoveHandler txtCompanyBranchCD.TextChanged, AddressOf Me.txtInput_TextChanged
                sender.Text = c_strTextBefore
                sender.SelectionStart = c_strTextSelection
                AddHandler txtCompanyCD.TextChanged, AddressOf Me.txtInput_TextChanged
                AddHandler txtCompanyBranchCD.TextChanged, AddressOf Me.txtInput_TextChanged
            End If
        Catch ex As Exception
            sender.Text = ""
        End Try
    End Sub

    Private Sub txtInput_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCompanyCD.KeyDown, txtCompanyBranchCD.KeyDown, txtFilePath.KeyDown
        c_strTextBefore = sender.Text
        c_strTextSelection = sender.SelectionStart()
    End Sub

    Private Sub txtFilePath_TextChanged(sender As Object, e As EventArgs) Handles txtFilePath.TextChanged
        If sender.Text.ToString.Equals("") Then
            Return
        End If
        Dim isValid = MNBTCMN100.checkLength(txtFilePath.Text, 255)
        Try
            If Not isValid Then
                RemoveHandler txtFilePath.TextChanged, AddressOf Me.txtFilePath_TextChanged
                sender.Text = c_strTextBefore
                sender.SelectionStart = c_strTextSelection
                AddHandler txtFilePath.TextChanged, AddressOf Me.txtFilePath_TextChanged
            End If
        Catch ex As Exception
            sender.Text = ""
        End Try
    End Sub
End Class