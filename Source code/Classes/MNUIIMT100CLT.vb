'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNUIIMT100
'*  機能名称：ユーザマスタ検索処理
'*  処理　　：ユーザマスタ検索処理（ＢＬ）
'*  内容　　：ユーザマスタ検索処理のビジネスロジック
'*  ファイル：MNUIIMT100CLT.vb
'*  備考　　：
'*
'*  Created：2015/07/15 RS. ThienNQ
'*  Updated: 2015/07/31 RS. ThongTH
'***************************************************************************************
Imports System.Data.Entity
Imports System.IO
Imports MyNo.Common
Imports NPOI.XSSF.UserModel

Public Class MNUIIMT100CLT
    Private c_mynoEntities As mynoEntities
    Private c_strPrefixFileName As String
    Private c_frmMNUIIMT100 As Form

    Public Sub New(ByRef in_frmMNUIIMT100 As Form)
        c_frmMNUIIMT100 = in_frmMNUIIMT100
    End Sub

    Public Function ExportData(ByVal in_strCompanyCD As String,
                               ByVal in_strCompanyName As String,
                               ByVal in_strBranchNo As String,
                               ByVal in_strBranchName As String
                               ) As Boolean
        Try
            c_mynoEntities = New mynoEntities()
            ExportData = False
            Using transScope As DbContextTransaction = c_mynoEntities.Database.BeginTransaction(IsolationLevel.Chaos)
                ' Get date time from server
                Dim datTimeNow As DateTime = MNBTCMN100.GetCurrentTimestamp(c_mynoEntities)
                Dim datNow As String = datTimeNow.ToString("yyyyMMddHHmmss")

                '  Set name for export file, part company and branch code
                Dim strPathSave As String = MNBTCMN100.GetConfig("Export_Dir_Xlsx_ProgressConfirmList", "config.ini")
                ' if path file not exit then exit function
                If MNBTCMN100.CheckDirectoryExists(strPathSave) = False Then
                    c_frmMNUIIMT100.BeginInvoke(New Action(Sub()
                                                               frmLoading.Hide()
                                                               c_frmMNUIIMT100.Activate()
                                                               MNBTCMN100.ShowMessage("MSGVWE00018", "進捗確認リストの出力先ディレクトリ", "", "")
                                                           End Sub))                    
                    Exit Function
                End If

                Dim strPartComAndBranch = MNBTCMN100.NamedPartCompanyExport(in_strCompanyCD, in_strBranchNo)
                c_strPrefixFileName = MNBTCMN100.GetConfig("PrefixFileName_Xlsx_ProgressConfirmList", "config.ini")
                Dim filePathSave As String = strPathSave + c_strPrefixFileName + strPartComAndBranch + "_" &
                                             datNow + ".xlsx"

                ' Get data
                Dim strSQL As String = String.Empty
                Dim lstCompany = GetDataCompanyForOutput(in_strCompanyCD, in_strCompanyName, in_strBranchNo,
                                                         in_strBranchName, strSQL)
                ' Cannot get data 
                If lstCompany.Count = 0 Then
                    ' Write log
                    Dim SEQ As Integer = MNBTCMN100.InputLogMaster(c_mynoEntities, "3", "MNUIIMT100", "", 0)
                    MNBTCMN100.InputLogDetail(c_mynoEntities, SEQ, "", "ファイル出力 件数:" & lstCompany.Count.ToString("###,###,###") & " (" & strSQL & ")",
                                              "")
                    transScope.Commit()
                    ' Close loading screen
                    c_frmMNUIIMT100.BeginInvoke(New Action(Sub()
                                                               frmLoading.Hide()
                                                               c_frmMNUIIMT100.Activate()
                                                               MNBTCMN100.ShowMessage("MSGVWE00017", "", "", "")
                                                           End Sub))
                    Exit Function
                End If

                ' out put file excel
                If ExportToExcel(lstCompany, c_mynoEntities, filePathSave) Then
                    ' Write log
                    Dim SEQ As Integer = MNBTCMN100.InputLogMaster(c_mynoEntities, "3", "MNUIIMT100", "", 0)
                    MNBTCMN100.InputLogDetail(c_mynoEntities, SEQ, "", "ファイル出力 件数:" & lstCompany.Count.ToString("###,###,###") & " (" & strSQL & ")",
                                              filePathSave)
                    transScope.Commit()
                    ' Can get data
                    c_frmMNUIIMT100.BeginInvoke(New Action(Sub()
                                                               'MNBTCMN100.ShowMessage("MSGVWI00003", "進捗確認リスト", "出力", "")
                                                               If MNBTCMN100.ShowMessageConfirm("MSGVWI00008", "進捗確認リスト", "", "") = DialogResult.OK Then
                                                                   Try
                                                                       Process.Start(filePathSave)
                                                                   Catch ex As Exception
                                                                       MNBTCMN100.ShowMessageException()
                                                                   End Try
                                                               End If
                                                           End Sub))
                Else
                    ' Write log
                    Dim SEQ As Integer = MNBTCMN100.InputLogMaster(c_mynoEntities, "3", "MNUIIMT100", "", 0)
                    MNBTCMN100.InputLogDetail(c_mynoEntities, SEQ, "", "ファイル出力 件数:" & lstCompany.Count.ToString("###,###,###") & " (" & strSQL & ")",
                                              filePathSave)
                    transScope.Commit()
                End If

            End Using
        Catch ex As Exception
            c_frmMNUIIMT100.BeginInvoke(New Action(Sub()
                                                       frmLoading.Hide()
                                                       c_frmMNUIIMT100.Activate()
                                                       MNBTCMN100.ShowMessageException()
                                                   End Sub))
            ExportData = False
        End Try
    End Function

    Private Function ExportToExcel(in_lstCompany As List(Of MNUIIMT100Model),
                                   ByVal in_context As mynoEntities,
                                   ByVal in_strfilePath As String) As Boolean
        Dim wb2 As XSSFWorkbook = Nothing
        Dim row As Integer = 6

        Try
            Dim strFilePathTemplate As String = ""
            Dim datTimeNow As DateTime = MNBTCMN100.GetCurrentTimestamp(in_context)
            strFilePathTemplate = Directory.GetCurrentDirectory + "\Template\Temp_MNUIIMT100.xlsx"

            'Open book here
            Try
                Using fs As New FileStream(strFilePathTemplate, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                    wb2 = New XSSFWorkbook(fs)
                End Using
            Catch ex As Exception
                c_frmMNUIIMT100.BeginInvoke(New Action(Sub()
                                                           frmLoading.Hide()
                                                           c_frmMNUIIMT100.Activate()
                                                           MNBTCMN100.ShowMessageException()
                                                       End Sub))             
                Return False
            End Try
            'Get sheet by the name
            Dim sht1 As XSSFSheet = wb2.GetSheet(c_strPrefixFileName)
            Dim rowCopy As Integer = 0
            'Copy row temp in excel
            For i As Integer = 0 To in_lstCompany.Count - 2
                rowCopy = 6 + i + 1
                sht1.CopyRow(6, rowCopy)
            Next

            'Set total records to excel
            sht1.GetRow(2).GetCell(15).SetCellValue(in_lstCompany.Count.ToString() + "件")
            ' date time export excel
            sht1.GetRow(3).GetCell(10).SetCellValue(datTimeNow.ToString("yyyy/MM/dd HH:mm"))
            sht1.GetRow(3).GetCell(13).SetCellValue(datTimeNow.ToString("yyyy/MM/dd HH:mm"))
            For Each company In in_lstCompany
                Application.DoEvents()
                sht1.GetRow(row).GetCell(1).SetCellValue(company.companycd)
                sht1.GetRow(row).GetCell(2).SetCellValue(company.companyname)
                sht1.GetRow(row).GetCell(3).SetCellValue(company.companybranchno)
                sht1.GetRow(row).GetCell(4).SetCellValue(company.companybranchname)
                sht1.GetRow(row).GetCell(5).SetCellValue(company.sumall)
                sht1.GetRow(row).GetCell(6).SetCellValue(company.sumkitoutput)
                sht1.GetRow(row).GetCell(7).SetCellValue(
                    CInt(company.sumkitoutput) - (CInt(company.sumregisternormal) + CInt(company.sumregisterabnormal)))
                sht1.GetRow(row).GetCell(8).SetCellValue(company.sumregisternormal)
                sht1.GetRow(row).GetCell(9).SetCellValue(company.sumregisterabnormal)
                sht1.GetRow(row).GetCell(10).SetCellValue(company.sumdifference)
                sht1.GetRow(row).GetCell(11).SetCellValue(company.sumregister1)
                sht1.GetRow(row).GetCell(12).SetCellValue(company.sumregister2)
                sht1.GetRow(row).GetCell(13).SetCellValue(company.sumdelete)
                sht1.GetRow(row).GetCell(14).SetCellValue(company.sumdelivery)

                sht1.GetRow(row).GetCell(15).SetCellValue(
                    If _
                                                             (CInt(company.sumall) - CInt(company.sumdelete) <> 0,
                                                              Math.Round(
                                                                  company.sumregister2 /
                                                                  (company.sumall - company.sumdelete), 2,
                                                                  MidpointRounding.AwayFromZero), 0))
                row = row + 1
            Next

            If rowCopy = 0 Then
                wb2.SetPrintArea(0, 0, 15, 0, 6)
            Else
                wb2.SetPrintArea(0, 0, 15, 0, rowCopy)
            End If

            ' Write data and save as file
            Using fs As New FileStream(in_strfilePath, FileMode.Create)
                wb2.Write(fs)
                fs.Close()
            End Using
            c_frmMNUIIMT100.BeginInvoke(New Action(Sub()
                                                       frmLoading.Hide()
                                                       c_frmMNUIIMT100.Activate()
                                                   End Sub))
            Return True
        Catch ex As Exception
            c_frmMNUIIMT100.BeginInvoke(New Action(Sub()
                                                       frmLoading.Hide()
                                                       c_frmMNUIIMT100.Activate()
                                                       MNBTCMN100.ShowMessageException()
                                                   End Sub))
            Return False
        End Try
    End Function

    Public Function GetDataCompany(ByVal in_strCompanyCD As String,
                                   ByVal in_strCompanyName As String,
                                   ByVal in_strBranchNo As String,
                                   ByVal in_strBranchName As String,
                                   ByRef ou_strSQL As String) As List(Of MNUIIMT100Model)
        c_mynoEntities = New mynoEntities()

        Dim intBranchNo As Integer = 0
        If IsNumeric(in_strBranchNo) Then
            intBranchNo = CInt(in_strBranchNo)
        End If

        Dim lstCompany = (From com In c_mynoEntities.t_company
                          Where com.delflg = 0 _
                      And If(String.IsNullOrEmpty(in_strCompanyCD), True, com.companycd = in_strCompanyCD) _
                      And
                      If _
                          (String.IsNullOrEmpty(in_strCompanyName), True,
                           com.companyname.Contains(in_strCompanyName)) _
                      And If(String.IsNullOrEmpty(in_strBranchNo), True, com.companybranchno = intBranchNo) _
                      And
                      If _
                          (String.IsNullOrEmpty(in_strBranchName), True,
                           com.companybranchname.Contains(in_strBranchName))
                          Order By com.companycd, com.companybranchno
                          Select New MNUIIMT100Model With {
                .companycd = com.companycd,
                .companyname =
                c_mynoEntities.t_company.Where(
                    Function(n) n.companycd = com.companycd And n.companybranchno = com.companybranchno).Max(
                        Function(n) n.companyname),
                .companybranchno = com.companybranchno,
                .companybranchname =
                c_mynoEntities.t_company.Where(
                    Function(n) n.companycd = com.companycd And n.companybranchno = com.companybranchno).Max(
                        Function(n) n.companybranchname),
                .adddatetime =
                c_mynoEntities.t_company.Where(
                    Function(n) n.companycd = com.companycd And n.companybranchno = com.companybranchno).Max(
                        Function(n) n.adddatetime),
                .delivschdate =
                c_mynoEntities.t_company.Where(
                    Function(n) n.companycd = com.companycd And n.companybranchno = com.companybranchno).Max(
                        Function(n) n.delivschdate),
               .delivdate = (From _private In c_mynoEntities.t_private
                             Where _private.delflg = 0 _
                      And _private.companycd = com.companycd _
                      And _private.companybranchno = com.companybranchno
                             Select _private.expdatetime
                ).DefaultIfEmpty(Nothing).Max(),
                .delschdate =
                c_mynoEntities.t_company.Where(
                    Function(n) n.companycd = com.companycd And n.companybranchno = com.companybranchno).Max(
                        Function(n) n.delschdate),
                .sumall = (From _private In c_mynoEntities.t_private
                           Where _private.delflg = 0 _
                      And _private.companycd = com.companycd _
                      And _private.companybranchno = com.companybranchno
                           Select 1
                ).DefaultIfEmpty(0).Sum(),
                .rate = (From _private In c_mynoEntities.t_private
                         Where _private.sts >= "600" _
                      And _private.delflg = 0 _
                      And _private.companycd = com.companycd _
                      And _private.companybranchno = com.companybranchno
                         Select 1
                ).DefaultIfEmpty(0).Sum()})

        ' Get SQL query from linq
        Dim strQuery As String = lstCompany.ToString()

        If strQuery.IndexOf("@p__linq__0") > 0 Then
            For i As Integer = 1 To 6
                strQuery = strQuery.Replace("Project" + i.ToString(), "P" + i.ToString())
            Next
            For i As Integer = 1 To 5
                strQuery = strQuery.Replace("Extent" + i.ToString(), "E" + i.ToString())
            Next

            strQuery = strQuery.Replace("@p__linq__0", "'" & in_strCompanyCD & "'")
            strQuery = strQuery.Replace("@p__linq__1", "'" & in_strCompanyCD & "'")
            strQuery = strQuery.Replace("@p__linq__2", "'" & in_strCompanyName & "'")
            strQuery = strQuery.Replace("@p__linq__3", "'%" & in_strCompanyName & "%'")
            strQuery = strQuery.Replace("@p__linq__4", "'" & in_strBranchNo & "'")
            strQuery = strQuery.Replace("@p__linq__5", intBranchNo)
            strQuery = strQuery.Replace("@p__linq__6", "'" & in_strBranchName & "'")
            strQuery = strQuery.Replace("@p__linq__7", "'%" & in_strBranchName & "%'")
        End If
        Dim message As String = strQuery
        ou_strSQL = message

        Return lstCompany.ToList()
    End Function


    ''' <summary>
    '''     Write log
    ''' </summary>
    ''' <param name="in_strSQL">Query SQL get records</param>
    ''' <remarks></remarks>
    Public Sub Log(in_strSQL As String, in_rowCount As Integer)
        Try
            c_mynoEntities = New mynoEntities()
            Using transScope As DbContextTransaction = c_mynoEntities.Database.BeginTransaction(IsolationLevel.Chaos)
                'setting log system
                Dim intSEQ As Integer = MNBTCMN100.InputLogMaster(c_mynoEntities, "2", "MNUIIMT100", "", 0)
                If intSEQ = -1 Then Exit Sub

                MNBTCMN100.InputLogDetail(c_mynoEntities, intSEQ, "",
                                          "一覧検索 件数:" + in_rowCount.ToString("###,###,###") + " (" + in_strSQL + ")", "")
                transScope.Commit()

            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    ''' <summary>
    '''     Get data for out put company
    ''' </summary>
    ''' <author>ThongTH</author>
    ''' <date>2015/07/30</date>
    ''' <param name="in_strCompanyCD">Company code</param>
    ''' <param name="in_strCompanyName">Company Name</param>
    ''' <param name="in_strBranchNo">Branch No</param>
    ''' <param name="in_strBranchName">Branch Name</param>
    ''' <returns>List data</returns>
    ''' <remarks></remarks>
    Private Function GetDataCompanyForOutput(ByVal in_strCompanyCD As String,
                                             ByVal in_strCompanyName As String,
                                             ByVal in_strBranchNo As String,
                                             ByVal in_strBranchName As String,
                                             ByRef ou_strSQL As String
                                             ) As List(Of MNUIIMT100Model)
        Try
            ' Convert branchno from string to number
            Dim intBranchNo As Nullable(Of Integer)
            If IsNumeric(in_strBranchNo) Then
                intBranchNo = in_strBranchNo
            End If

            ' Get data
            Dim lstCompany = From company In c_mynoEntities.t_company
                    Where company.delflg = 0 _
                          And
                          ((in_strCompanyCD <> String.Empty And company.companycd = in_strCompanyCD) Or
                           String.IsNullOrEmpty(in_strCompanyCD)) _
                          And
                          ((intBranchNo IsNot Nothing And company.companybranchno = intBranchNo) Or
                           intBranchNo Is Nothing) _
                          And
                          (in_strCompanyName <> "" And
                           ((company.companyname.Contains(in_strCompanyName))) Or in_strCompanyName = "") _
                          And
                          (in_strBranchName <> "" And
                           ((company.companybranchname.Contains(in_strBranchName))) Or in_strBranchName = "") _
                    Order By company.companycd, company.companybranchno _
                    Select New MNUIIMT100Model With { _
                    .companycd = company.companycd, _
                    .companybranchno = company.companybranchno, _
                    .companyname = company.companyname, _
                    .companybranchname = company.companybranchname, _
                    .adddatetime = (From _private In c_mynoEntities.t_private
                    Where _private.companycd = company.companycd _
                          And _private.companybranchno = company.companybranchno _
                    Select _private.adddatetime
                    ).Min(), _
                    .sumall = (From _private In c_mynoEntities.t_private _
                    Where _private.sts >= "200" _
                          And _private.companycd = company.companycd _
                          And _private.companybranchno = company.companybranchno _
                    Select 1
                    ).DefaultIfEmpty(0).Sum(), _
                    .sumkitoutput = (From _private In c_mynoEntities.t_private _
                    Where _private.sts >= "300" _
                          And _private.delflg = 0 _
                          And _private.companycd = company.companycd _
                          And _private.companybranchno = company.companybranchno _
                    Select 1
                    ).DefaultIfEmpty(0).Sum(), _
                    .sumregisternormal = (From _private In c_mynoEntities.t_private _
                    Where _private.delflg = 0 _
                          And _private.sts >= "400" _
                          And _private.companycd = company.companycd _
                          And _private.companybranchno = company.companybranchno _
                    Select 1
                    ).DefaultIfEmpty(0).Sum(), _
                    .sumdifference = (From _private In c_mynoEntities.t_private _
                                         Where _private.delflg = 1 _
                                               And _private.companycd = company.companycd _
                                               And _private.companybranchno = company.companybranchno _
                                         Select 1
                                         ).DefaultIfEmpty(0).Sum() + (From _private In c_mynoEntities.t_private _
                                         Where _private.delflg = 0 _
                                               And _private.companycd = company.companycd _
                                               And _private.companybranchno = company.companybranchno _
                                               And
                                               c_mynoEntities.t_familyresult.Any(
                                                   Function(n) n.companycd = _private.companycd _
                                                               And n.companybranchno = _private.companybranchno _
                                                               And n.staffcd = _private.staffcd _
                                                               And n.familyno >= 1 _
                                                               And n.resultflg = 3 _
                                                               And ((n.familyaddflg = 1 And n.unofferflg = 0) _
                                                                    Or (n.familyaddflg = 0 And n.unofferflg = 1)) _
                                                               And n.delflg = 0) _
                                         Select 1
                                         ).DefaultIfEmpty(0).Sum(), _
                    .sumregisterabnormal = (From _private In c_mynoEntities.t_private _
                    Where _private.delflg = 0 _
                          And _private.abnormalflg = 1 _
                          And _private.sts = "300" _
                          And _private.companycd = company.companycd _
                          And _private.companybranchno = company.companybranchno _
                    Select 1
                    ).DefaultIfEmpty(0).Sum(), _
                    .sumregister1 = (From _private In c_mynoEntities.t_private _
                    Where _private.delflg = 0 _
                          And _private.sts >= "500" _
                          And _private.companycd = company.companycd _
                          And _private.companybranchno = company.companybranchno _
                    Select 1
                    ).DefaultIfEmpty(0).Sum(), _
                    .sumregister2 = (From _private In c_mynoEntities.t_private _
                    Where _private.delflg = 0 _
                          And _private.sts >= "600" _
                          And _private.companycd = company.companycd _
                          And _private.companybranchno = company.companybranchno _
                    Select 1
                    ).DefaultIfEmpty(0).Sum(), _
                    .sumdelete = (From _private In c_mynoEntities.t_private _
                    Where _private.delflg = 1 _
                          And _private.companycd = company.companycd _
                          And _private.companybranchno = company.companybranchno _
                    Select 1
                    ).DefaultIfEmpty(0).Sum(), _
                    .sumdelivery = (From _private In c_mynoEntities.t_private _
                    Where _private.delflg = 0 _
                          And _private.sts = "700" _
                          And _private.companycd = company.companycd _
                          And _private.companybranchno = company.companybranchno _
                    Select 1
                    ).DefaultIfEmpty(0).Sum() _
                    }

            ou_strSQL = lstCompany.ToString()

            If ou_strSQL.IndexOf("@p__linq__0") > 0 Then

                For i As Integer = 1 To 24
                    ou_strSQL = ou_strSQL.Replace("Project" + i.ToString(), "P" + i.ToString())
                Next
                For i As Integer = 1 To 13
                    ou_strSQL = ou_strSQL.Replace("Extent" + i.ToString(), "E" + i.ToString())
                Next

                ou_strSQL = ou_strSQL.Replace("@p__linq__9", "'" & in_strCompanyName & "'")
                ou_strSQL = ou_strSQL.Replace("@p__linq__10", "'" & in_strBranchName & "'")
                ou_strSQL = ou_strSQL.Replace("@p__linq__11", "'%" & in_strBranchName & "%'")
                ou_strSQL = ou_strSQL.Replace("@p__linq__12", "'" & in_strBranchName & "'")
                ou_strSQL = ou_strSQL.Replace("@p__linq__0", "'" & in_strCompanyCD & "'")
                ou_strSQL = ou_strSQL.Replace("@p__linq__1", "'" & in_strCompanyCD & "'")
                ou_strSQL = ou_strSQL.Replace("@p__linq__2", "'" & in_strCompanyCD & "'")

                ou_strSQL = ou_strSQL.Replace("@p__linq__3", "'" & in_strCompanyCD & "'")
                ou_strSQL = ou_strSQL.Replace("@p__linq__4", If(intBranchNo Is Nothing, "NULL", intBranchNo))
                ou_strSQL = ou_strSQL.Replace("@p__linq__5", If(intBranchNo Is Nothing, "NULL", intBranchNo))
                ou_strSQL = ou_strSQL.Replace("@p__linq__6", If(intBranchNo Is Nothing, "NULL", intBranchNo))

                ou_strSQL = ou_strSQL.Replace("@p__linq__7", "'" & in_strCompanyName & "'")
                ou_strSQL = ou_strSQL.Replace("@p__linq__8", "'%" & in_strCompanyName & "%'")

            End If

            Return lstCompany.ToList()
        Catch ex As Exception
            Return Nothing
            MNBTCMN100.ShowMessageException()
        End Try
    End Function
End Class
