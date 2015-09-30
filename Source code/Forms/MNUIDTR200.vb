'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNUIDTR200
'*  機能名称：企業情報取込画面
'*  処理　　：企業情報取込画面
'*  内容　　：企業情報取込画面
'*  ファイル：MNUIDTR200.vb
'*  備考　　：
'*
'*  Created：2015/07/20 RS. ThangNB
'***************************************************************************************

Imports MyNo.Common
Imports System.Data.Entity
Imports System.Threading
Imports System.IO
Imports NPOI.XSSF.UserModel

Public Class frmMNUIDTR200
    'Start paging
    Private p_intNumItem As Integer
    Private p_mynoContext As MyNoEntities
    Private p_MNUIDTR200CTL As New MNUIDTR200CTL(Me)
    Private c_strTextBefore As String = Nothing
    Private c_strTextSelection As Integer = Nothing
    Private oldUrlImport As String = MNBTCMN100.GetConfig("Import_Dir_CSV", "config.ini")
    Private strPathFile As String = ""
    Private c_trd As Thread

    ''' <summary>
    ''' frmMNUIDTR200 Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmMNUIDTR200_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            'p_mynoContext = New MyNoEntities()
            ''Get record max set in config.ini
            'p_intNumItem = MNBTCMN100.GetConfig("MaxLine_Gridview_CompanyDataImport", "config.ini")
            ''Set title form
            'If p_strUserCdLogin <> Nothing Then
            '    Me.Text = MNBTCMN100.SetTitleScreen()
            'End If
            ''Set data for dgvImportLog Have Paging
            ''lblPage.Text = 0
            'Dim query As String = String.Empty
            'Dim numRecords As Integer = 0
            'dgvImportLog.DataSource = p_MNUIDTR200CTL.GetDataImportPaging(p_intNumItem, CInt(lblPage.Text), True, p_mynoContext, query, numRecords)
            'If dgvImportLog.DataSource IsNot Nothing Then
            '    'lblPage.Text = 1
            'End If
            'dgvImportLog.ClearSelection()
            'If p_intNumItem > 10 Then
            '    dgvImportLog.Columns(1).Width = dgvImportLog.Columns(1).Width - 10
            'End If
            'Using transScope As DbContextTransaction = p_mynoContext.Database.BeginTransaction(IsolationLevel.Chaos)
            '    'write system log
            '    Dim seq As Integer = MNBTCMN100.InputLogMaster(p_mynoContext, 2, "MNUIDTR200", "", Nothing)
            '    Dim message As String = "一覧検索 件数:" & numRecords.ToString("###,###,###") & " (" & query & ")"
            '    MNBTCMN100.InputLogDetail(p_mynoContext, seq, "", message, "")
            '    transScope.Commit()
            'End Using
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try

    End Sub

    ''' <summary>
    ''' Event change page of datagridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ChangePage(sender As Object, e As EventArgs)
        p_mynoContext = New MyNoEntities()
        Dim query As String = String.Empty
        Dim numRecords As Integer = 0
        Select Case sender.Tag
            Case 0
                'Click button previous
                'If CInt(lblPage.Text) > 1 Then
                '    dgvImportLog.DataSource = p_MNUIDTR200CTL.GetDataImportPaging(p_intNumItem, CInt(lblPage.Text), False, p_mynoContext, query, numRecords)
                '    lblPage.Text = CInt(lblPage.Text) - 1
                'End If
            Case 1
                'Click button next
                If (dgvImportLog.RowCount = p_intNumItem) Then
                    'Dim obj As List(Of DataImportGridItem) = p_MNUIDTR200CTL.GetDataImportPaging(p_intNumItem, CInt(lblPage.Text), True, p_mynoContext, query, numRecords)
                    'If obj IsNot Nothing And obj.Count > 0 Then
                    '    dgvImportLog.DataSource = obj
                    '    'lblPage.Text = CInt(lblPage.Text) + 1
                    'End If
                End If
        End Select
        dgvImportLog.ClearSelection()
    End Sub
    ''' <summary>
    ''' Close Form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    ''' <summary>
    ''' Choose file csv
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnFileChoose_Click(sender As Object, e As EventArgs) Handles btnFileChoose.Click
        'open dialog
        Dim dialog As New OpenFileDialog()
        'choose file
        dialog.InitialDirectory = oldUrlImport
        dialog.Filter = "Excel files (*.xlsx)|*.xlsx|Excel files (*xlsx)|*.xlsx"
        dialog.RestoreDirectory = True
        If dialog.ShowDialog() = DialogResult.OK Then
            txtPathFile.Text = dialog.FileName
            oldUrlImport = dialog.FileName
        End If
    End Sub
    ''' <summary>
    ''' Import data from csv to db
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        p_mynoContext = New MyNoEntities()
        'Check validate path file input
        If p_MNUIDTR200CTL.ValidateInputImport(txtPathFile.Text) Then
            'Show message confirm process
            If MNBTCMN100.ShowMessage("MSGVWI00001", "file này không", "nhập", "") = DialogResult.OK Then
                'Select OK button
                strPathFile = txtPathFile.Text
                c_trd = New Thread(AddressOf frmLoading_Shown)
                c_trd.IsBackground = True
                c_trd.Start()
                frmLoading.ShowDialog()

                'AddHandler frmLoading.Shown, AddressOf frmLoading_Shown
                'frmLoading.ShowDialog()
                'RemoveHandler frmLoading.Shown, AddressOf frmLoading_Shown

            End If
        End If
    End Sub

    Private Function GetValueFromCell(cell As NPOI.SS.UserModel.ICell) As String
        Select Case cell.CellType
            Case NPOI.SS.UserModel.CellType.Numeric
                Return CStr(cell.NumericCellValue)
            Case NPOI.SS.UserModel.CellType.String
                Return cell.StringCellValue
            Case Else
                Return ""
        End Select        
        Return ""
    End Function

    Private Sub frmLoading_Shown()
        'Try

        'Open book here
        Try
            Dim wb2 As XSSFWorkbook = Nothing
            Using fs As New FileStream(strPathFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                wb2 = New XSSFWorkbook(fs)
            End Using

            'Get sheet by the name
            Dim sht1 As XSSFSheet = wb2.GetSheet("Sheet1")
            Dim rowCopy As Integer = 0
            Dim db As New MyNoEntities
            Dim bienap As t_bienap
            Dim bienapimport As t_bienapimport
            Dim TenLo As String = ""
            For row As Integer = 2 To sht1.LastRowNum - 1
                bienap = New t_bienap
                bienapimport = New t_bienapimport
                If GetValueFromCell(sht1.GetRow(row).GetCell(0)).StartsWith("Lộ") Then
                    TenLo = GetValueFromCell(sht1.GetRow(row).GetCell(0))
                Else
                    bienap.somay = GetValueFromCell(sht1.GetRow(row).GetCell(15))
                    'bienap.ngaycapnhat = DateCreate
                    db.t_bienap.Add(bienap)
                End If
            Next
            db.SaveChanges()
        Catch ex As Exception
            Me.BeginInvoke(New Action(Sub()
                                          frmLoading.Hide()
                                          MNBTCMN100.ShowMessageException()
                                      End Sub))

        End Try

        'Application.DoEvents()
        'Cursor.Current = Cursors.WaitCursor
        'Dim checkCountItem As Boolean = True
        ''基礎データの項目数を超える場合
        'Using myReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(txtPathFile.Text)
        '    'Specify that reading from a comma-delimited file'
        '    myReader.TextFieldType = FileIO.FieldType.Delimited
        '    myReader.SetDelimiters(",")
        '    Dim currentRow As String()
        '    If Not myReader.EndOfData Then
        '        currentRow = myReader.ReadFields()
        '        'Check data coloum in file csv
        '        If (currentRow.Count() <> 30) Then
        '            checkCountItem = False
        '            frmLoading.Hide()
        '            Me.Activate()
        '            MNBTCMN100.ShowMessage("MSGVWE00020", "", "", "")
        '        End If
        '    End If
        'End Using
        'If checkCountItem Then
        '    'Read file csv, check error csv, If error perform insert log and show message, else insert data to DB
        '    If p_MNUIDTR200CTL.ProcessImportCompany(txtPathFile.Text) Then
        '        MNBTCMN100.ShowMessage("MSGVWI00003", "企業情報", "取込", "")
        '    End If
        'End If
        ''Loading form again
        ''frmMNUIDTR200_Load(sender, e)
        'txtPathFile.Text = String.Empty
        'Cursor.Current = Cursors.Default
        'Catch ex As Exception
        '    Cursor.Current = Cursors.Default
        '    MNBTCMN100.ShowMessageException()
        'End Try
    End Sub
    ''' <summary>
    ''' Clear condition export
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        'txtCompanyCd.Text = String.Empty
        'txtBranchCd.Text = String.Empty
        'txtBranchStart.Text = String.Empty
        'txtBranchEnd.Text = String.Empty
    End Sub
    ''' <summary>
    ''' Cell click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvImportLog_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvImportLog.CellClick
        Dim rowIndex As Integer = e.RowIndex
        Dim index As Integer = e.ColumnIndex
        p_mynoContext = New MyNoEntities()
        If rowIndex <> -1 Then
            If index = 3 Then
                Dim lnkDowloadCsv As String
                lnkDowloadCsv = dgvImportLog.Rows(e.RowIndex).Cells(3).Value
                If lnkDowloadCsv <> "" And lnkDowloadCsv IsNot Nothing Then
                    'Show message confirm process
                    If MNBTCMN100.ShowMessage("MSGVWI00001", "インポートファイル", "ダウンロード", "") = DialogResult.OK Then
                        frmLoading.Show()
                        Application.DoEvents()
                        Dim intSeq As Integer
                        'Get seq of row click
                        intSeq = dgvImportLog.Rows(e.RowIndex).Cells(0).Value
                        If p_MNUIDTR200CTL.ExportCsvLinkFileName(p_mynoContext, intSeq) Then
                            frmLoading.Close()
                            MNBTCMN100.ShowMessage("MSGVWI00003", "企業情報データ", "出力", "")
                        End If
                        frmLoading.Close()
                    End If
                End If
            ElseIf index = 6 Then
                Dim results = dgvImportLog.Rows(e.RowIndex).Cells(6).Value
                If results IsNot Nothing And results <> "" Then
                    'Show message confirm process
                    If MNBTCMN100.ShowMessage("MSGVWI00001", "エラーリスト", "ダウンロード", "") = DialogResult.OK Then
                        frmLoading.Show()
                        Application.DoEvents()
                        Dim intSeq As Integer
                        'Get seq of row click
                        intSeq = dgvImportLog.Rows(e.RowIndex).Cells(0).Value
                        'Get value of row エラーリスト(明細)
                        Dim query As String = String.Empty
                        Dim listXlsx = p_MNUIDTR200CTL.GetDataExportXlsx(p_mynoContext, intSeq, query)
                        If listXlsx.Count > 0 Then
                            Dim filePathSave As String = String.Empty
                            If p_MNUIDTR200CTL.ExportXlsxListError(listXlsx, p_mynoContext, query, intSeq, filePathSave) Then
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
                    End If
                    frmLoading.Close()
                End If
            End If
        End If
    End Sub
    ''' <summary>
    ''' Export csv
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExport_Click(sender As Object, e As EventArgs)
        'p_mynoContext = New MyNoEntities()
        ''Check validate
        'If p_MNUIDTR200CTL.ValidateExport(txtCompanyCd.Text, txtBranchCd.Text, txtBranchStart.Text, txtBranchEnd.Text) Then
        '    'Show message confirm
        '    If MNBTCMN100.ShowMessage("MSGVWI00001", "企業情報エクスポート", "実行", "") = DialogResult.OK Then
        '        frmLoading.Show()
        '        Application.DoEvents()
        '        If p_MNUIDTR200CTL.ExportCsv(txtCompanyCd.Text, txtBranchCd.Text, txtBranchStart.Text, txtBranchEnd.Text) Then
        '            frmLoading.Close()
        '            MNBTCMN100.ShowMessage("MSGVWI00003", "企業情報", "出力", "")
        '        End If
        '    End If
        'End If
    End Sub

    Private Sub txtInput_TextChanged(sender As Object, e As EventArgs)
        If sender.Text.ToString.Equals("") Then
            Return
        End If
        Dim isValid = MNBTCMN100.CheckValidAlphanumeric(sender.Text)
        Try
            If isValid = True Then
                'RemoveHandler txtCompanyCd.TextChanged, AddressOf Me.txtInput_TextChanged
                'RemoveHandler txtBranchCd.TextChanged, AddressOf Me.txtInput_TextChanged
                'sender.Text = c_strTextBefore
                'sender.SelectionStart = c_strTextSelection
                'AddHandler txtCompanyCd.TextChanged, AddressOf Me.txtInput_TextChanged
                'AddHandler txtBranchCd.TextChanged, AddressOf Me.txtInput_TextChanged
            End If
        Catch ex As Exception
            sender.Text = ""
        End Try
    End Sub

    Private Sub txtInput_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPathFile.KeyDown
        c_strTextBefore = sender.Text
        c_strTextSelection = sender.SelectionStart()
    End Sub

    Private Sub txtFilePath_TextChanged(sender As Object, e As EventArgs) Handles txtPathFile.TextChanged
        If sender.Text.ToString.Equals("") Then
            Return
        End If
        Dim isValid = MNBTCMN100.checkLength(txtPathFile.Text, 255)
        Try
            If Not isValid Then
                RemoveHandler txtPathFile.TextChanged, AddressOf Me.txtFilePath_TextChanged
                sender.Text = c_strTextBefore
                sender.SelectionStart = c_strTextSelection
                AddHandler txtPathFile.TextChanged, AddressOf Me.txtFilePath_TextChanged
            End If
        Catch ex As Exception
            sender.Text = ""
        End Try
    End Sub

    'Private Sub dgvImportLog_Enter(sender As Object, e As EventArgs) Handles dgvImportLog.Enter
    '    If dgvImportLog.RowCount <> 0 Then
    '        dgvImportLog.CurrentCell = dgvImportLog.Rows(0).Cells(3)
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' tabindex in dgv
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub dgvImportLog_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvImportLog.KeyDown
    '    If dgvImportLog.CurrentCell IsNot Nothing AndAlso e.KeyValue = Keys.Tab Then
    '        If dgvImportLog.CurrentCell.ColumnIndex < 3 Then
    '            dgvImportLog.CurrentCell = dgvImportLog.Rows(dgvImportLog.CurrentCell.RowIndex).Cells(2)
    '        ElseIf dgvImportLog.CurrentCell.ColumnIndex < 8 Then
    '            dgvImportLog.CurrentCell = dgvImportLog.Rows(dgvImportLog.CurrentCell.RowIndex).Cells(7)
    '        ElseIf dgvImportLog.CurrentCell.ColumnIndex = 8 Then
    '            dgvImportLog.CurrentCell = Nothing
    '            dgvImportLog.ClearSelection()
    '            btnPrevious.Focus()
    '        End If
    '    End If
    'End Sub
End Class