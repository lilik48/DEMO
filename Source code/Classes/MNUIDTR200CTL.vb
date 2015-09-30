'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNUIDTR200
'*  機能名称：企業情報取込処理
'*  処理　　：企業情報取込処理（ＢＬ）
'*  内容　　：企業情報取込処理のビジネスロジック
'*  ファイル：MNUIDTR200CTL.vb
'*  備考　　：
'*
'*  Created：2015/07/20 RS. ThangNB
'***************************************************************************************
Imports MyNo.Common
Imports System.Data.Entity
Imports System.IO
Imports NPOI.XSSF.UserModel
Imports System.Text

Public Class MNUIDTR200CTL
    Private p_mynoContext As mynoEntities
    Public Shared p_intSeq1st As Integer = 0
    Public Shared p_intSeq2nd As Integer = 0
    Public Shared p_intSeqDetail As Long = 0L
    Private c_frmMNUIDTR200 As Form

    Public Sub New(ByRef in_frmMNUIDTR200 As Form)
        c_frmMNUIDTR200 = in_frmMNUIDTR200
    End Sub
    ''' <summary>
    ''' Load form 
    ''' </summary>
    ''' <param name="in_intNumItem">record display in one page</param>
    ''' <param name="in_intNumPage">page number</param>
    ''' <param name="in_blnFlagNextPage">flag previous or next</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataImportPaging(ByVal in_intNumItem As Integer, ByVal in_intNumPage As Integer, ByVal in_blnFlagNextPage As Boolean, ByRef inout_mynoContext As mynoEntities, ByRef out_Query As String, ByRef out_CountRecord As Integer) As List(Of DataImportGridItem)
        Try
            Dim intSkip As Integer
            If in_blnFlagNextPage = False Then
                intSkip = in_intNumItem * (in_intNumPage - 1) - in_intNumItem
            Else
                intSkip = in_intNumItem * in_intNumPage
            End If
            'Get data in table t_importlog
            Dim query = From im In inout_mynoContext.t_importlog
                                Where im.filetype = 3
                                 Order By im.seq Descending
                                 Select New DataImportGridItem With {
                                .Seq = im.seq,
                                .Impdatetime = im.impdatetime,
                                .Impusercd = im.impusercd,
                                .Filename = im.filename,
                                .ImportType = If(im.imptype = 1, "全件", If(im.imptype = 2, "新規", String.Empty)),
                                .ErrFlg = If(im.errflg = 0, "○", "×"),
                                .DownloadFlg = If(im.errflg = 0, Nothing, "ダウンロード")
                         }
            out_Query = query.ToString()
            If out_Query.IndexOf("Project1") > 0 Then
                out_Query = out_Query.Replace("Project1", "P1")
            End If
            out_CountRecord = query.Count()
            Return query.Skip(intSkip).Take(in_intNumItem).ToList()
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Check validate input file path for import data
    ''' </summary>
    ''' <param name="in_strUrlPath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateInputImport(ByVal in_strUrlPath As String) As Boolean
        Try
            If in_strUrlPath = "" Or in_strUrlPath Is Nothing Then
                MNBTCMN100.ShowMessage("MSGVWE00001", "Đường dẫn tham chiếu tập tin", "", "")
                Return False
            ElseIf IO.File.Exists(in_strUrlPath) = False Then
                MNBTCMN100.ShowMessage("MSGVWE00018", "Đường dẫn tham chiếu tập tin", "", "")
                Return False
            ElseIf IO.Path.GetExtension(in_strUrlPath) <> ".xlsx" AndAlso IO.Path.GetExtension(in_strUrlPath) <> ".xlsx" Then
                MNBTCMN100.ShowMessage("MSGVWE00019", "", "", "")
                Return False
            End If
            Return True
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
            Return False
        End Try
    End Function
    ''' <summary>
    ''' import company data from csv file
    ''' </summary>
    ''' <param name="in_strPathFile"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessImportCompany(ByVal in_strPathFile As String) As Boolean
        Try
            Dim results As Boolean = False
            p_mynoContext = New mynoEntities()
            Using transScope As DbContextTransaction = p_mynoContext.Database.BeginTransaction(IsolationLevel.Chaos)
                'insert to t_systemlog and gen SEQ
                Dim seq As Integer = MNBTCMN100.InputLogMaster(p_mynoContext, 4, "MNUIDTR200", "", Nothing)
                'import data to t_importlog
                ImportLog(p_mynoContext, seq, in_strPathFile)

                'register content file csv to DB
                ImportFromCsv(p_mynoContext, seq, in_strPathFile)
                'Check error csv, If error perform insert log and show message, else insert data to DB
                If ProcessCsv(seq, in_strPathFile, p_mynoContext) Then
                    results = True
                End If
                transScope.Commit()
            End Using
            Return results
        Catch ex As Exception
            frmLoading.Close()
            MNBTCMN100.ShowMessageExceptionWithReturn()
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Insert data form table t_importlog
    ''' </summary>
    ''' <param name="p_mynoContext"></param>
    ''' <param name="in_intSeq"></param>
    ''' <param name="in_strFilePath"></param>
    ''' <remarks></remarks>
    Public Sub ImportLog(p_mynoContext As mynoEntities, in_intSeq As Integer, in_strFilePath As String)

        'Get date time
        Dim currentDate = MNBTCMN100.GetCurrentTimestamp(p_mynoContext)
        'Add data to t_importlog
        Dim item As New t_importlog()
        item.seq = in_intSeq
        item.companycd = Nothing
        item.companyname = Nothing
        item.companybranchno = Nothing
        item.companybranchname = Nothing
        item.impdatetime = currentDate.ToString("yyyy/MM/dd HH:mm:ss")
        item.impusercd = p_strUserCdLogin

        'start csvfile read binary file
        Dim fInfo As New FileInfo(in_strFilePath)
        Dim numBytes As Long = fInfo.Length
        Dim fStream As New FileStream(in_strFilePath, FileMode.Open, FileAccess.Read)
        Dim br As New BinaryReader(fStream)
        Dim data As Byte() = br.ReadBytes(CInt(numBytes))
        br.Close()
        fStream.Close()
        item.csvfile = data
        'end csvfile read binary file

        item.filename = IO.Path.GetFileName(in_strFilePath)
        item.imptype = 1
        item.filetype = 3
        item.errflg = Nothing
        item.adddatetime = currentDate.ToString("yyyy/MM/dd HH:mm:ss")
        item.addjusercd = p_strUserCdLogin
        item.terminalcd = p_strTerminalCdLogin

        p_mynoContext.t_importlog.Add(item)
        p_mynoContext.SaveChanges()
        p_intSeq1st = in_intSeq

    End Sub
    ''' <summary>
    ''' Import data from csv to db table w_company
    ''' </summary>
    ''' <param name="p_mynoContext"></param>
    ''' <param name="in_intSeq"></param>
    ''' <param name="in_strFilePath"></param>
    ''' <remarks></remarks>
    Public Sub ImportFromCsv(p_mynoContext As mynoEntities, in_intSeq As Integer, in_strFilePath As String)
        Dim wCompany As New w_company()
        Using myReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(in_strFilePath, System.Text.Encoding.GetEncoding("Shift_JIS"))
            myReader.TextFieldType = FileIO.FieldType.Delimited
            myReader.SetDelimiters(",")
            Dim currentRow As String()
            Dim count As Integer = 1
            While Not myReader.EndOfData
                Application.DoEvents()

                currentRow = myReader.ReadFields()
                If currentRow.Count() = 1 And currentRow.GetValue(0) = "" Then
                    Console.Write("New line")
                Else
                    If (currentRow.Count() <= 30) Then
                        Dim item As New w_company
                        item.seq = in_intSeq
                        item.line = count
                        If currentRow.Count() > 0 Then
                            item.companycd = currentRow.GetValue(0).ToString()
                        End If
                        If currentRow.Count() > 1 Then
                            item.companybranchno = currentRow.GetValue(1).ToString()
                        End If
                        If currentRow.Count() > 2 Then
                            item.companyname = currentRow.GetValue(2).ToString()
                        End If
                        If currentRow.Count() > 3 Then
                            item.companybranchname = currentRow.GetValue(3).ToString()
                        End If
                        If currentRow.Count() > 4 Then
                            item.contactfamilyname = currentRow.GetValue(4).ToString()
                        End If
                        If currentRow.Count() > 5 Then
                            item.contactfamilynamekana = currentRow.GetValue(5).ToString()
                        End If
                        If currentRow.Count() > 6 Then
                            item.contactpostno = currentRow.GetValue(6).ToString()
                        End If
                        If currentRow.Count() > 7 Then
                            item.contactprefecturescd = currentRow.GetValue(7).ToString()
                        End If
                        If currentRow.Count() > 8 Then
                            item.contactaddress1 = currentRow.GetValue(8).ToString()
                        End If
                        If currentRow.Count() > 9 Then
                            item.contactaddress2 = currentRow.GetValue(9).ToString()
                        End If
                        If currentRow.Count() > 10 Then
                            item.contacttel = currentRow.GetValue(10).ToString()
                        End If
                        If currentRow.Count() > 11 Then
                            item.contactfax = currentRow.GetValue(11).ToString()
                        End If
                        If currentRow.Count() > 12 Then
                            item.contactmail = currentRow.GetValue(12).ToString()
                        End If
                        If currentRow.Count() > 13 Then
                            item.contactbelongname1 = currentRow.GetValue(13).ToString()
                        End If
                        If currentRow.Count() > 14 Then
                            item.contactbelongname2 = currentRow.GetValue(14).ToString()
                        End If
                        If currentRow.Count() > 15 Then
                            item.contactnotes = currentRow.GetValue(15).ToString()
                        End If
                        If currentRow.Count() > 16 Then
                            item.trusteeflg01 = currentRow.GetValue(16).ToString()
                        End If
                        If currentRow.Count() > 17 Then
                            item.trusteeflg02 = currentRow.GetValue(17).ToString()
                        End If
                        If currentRow.Count() > 18 Then
                            item.trusteeflg03 = currentRow.GetValue(18).ToString()
                        End If
                        If currentRow.Count() > 19 Then
                            item.trusteeflg04 = currentRow.GetValue(19).ToString()
                        End If
                        If currentRow.Count() > 20 Then
                            item.trusteeflg05 = currentRow.GetValue(20).ToString()
                        End If
                        If currentRow.Count() > 21 Then
                            item.trusteeflg06 = currentRow.GetValue(21).ToString()
                        End If
                        If currentRow.Count() > 22 Then
                            item.trusteeflg07 = currentRow.GetValue(22).ToString()
                        End If
                        If currentRow.Count() > 23 Then
                            item.trusteeflg08 = currentRow.GetValue(23).ToString()
                        End If
                        If currentRow.Count() > 24 Then
                            item.trusteeflg09 = currentRow.GetValue(24).ToString()
                        End If
                        If currentRow.Count() > 25 Then
                            item.trusteeflg10 = currentRow.GetValue(25).ToString()
                        End If
                        If currentRow.Count() > 26 Then
                            item.trusteeflg11 = currentRow.GetValue(26).ToString()
                        End If
                        If currentRow.Count() > 27 Then
                            item.trusteeflg12 = currentRow.GetValue(27).ToString()
                        End If
                        If currentRow.Count() > 28 Then
                            item.delivschdate = currentRow.GetValue(28).ToString()
                        End If
                        If currentRow.Count() > 29 Then
                            item.delschdate = currentRow.GetValue(29).ToString()
                        End If
                        p_mynoContext.w_company.Add(item)
                        count += 1
                    End If
                End If
            End While
            p_mynoContext.SaveChanges()
        End Using
    End Sub
    ''' <summary>
    ''' Process data in file csv
    ''' </summary>
    ''' <param name="in_intSeq"></param>
    ''' <param name="in_strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessCsv(ByVal in_intSeq As Integer, ByVal in_strFilePath As String, ByRef inout_mynoContext As mynoEntities) As Boolean

        Dim checkReuslt As Boolean = True
        'Get data in table w_company 
        Dim queryListwCompany As List(Of w_company) = New List(Of w_company)()
        queryListwCompany = (From com In inout_mynoContext.w_company
                            Where com.seq = in_intSeq
                            Order By com.line Ascending
                            Select com).ToList()
        'If data in w_company < 1
        If queryListwCompany.Count < 1 Then
            'insert log detail for table T_SYSTEMDETAILLOG
            MNBTCMN100.InputLogDetail(inout_mynoContext, in_intSeq, "", "取込エラー", in_strFilePath)
            'Get content message by msgkey = MSGVWE00018
            Dim message As String = MNBTCMN100.GetMessageContent("MSGVWE00018", "取込対象データ", "", "")
            'insert data to table t_importdetaillog
            ImportDetailLog(inout_mynoContext, in_intSeq, Nothing, message, Nothing, Nothing)

            frmLoading.Hide()
            c_frmMNUIDTR200.Activate()
            MNBTCMN100.ShowMessage("MSGVWE00018", "取込対象データ", "", "")
            checkReuslt = False
        Else
            Dim resultsCheckError As Integer = CheckErrorCsv(inout_mynoContext, in_intSeq, queryListwCompany)
            'Case data in file csv have error
            If resultsCheckError > 0 Then
                'Update error flag
                Dim updateErrorFlg = (From im In inout_mynoContext.t_importlog
                                     Where im.seq = in_intSeq).SingleOrDefault()
                updateErrorFlg.errflg = 2
                'Delete data in table w_company
                Dim deleteData = (From c In inout_mynoContext.w_company
                                  Where c.seq = in_intSeq).ToList()
                inout_mynoContext.w_company.RemoveRange(deleteData)

                'Input system detail log
                MNBTCMN100.InputLogDetail(inout_mynoContext, in_intSeq, "", "ファイル取込エラー", in_strFilePath)
                'Close Loading and display message
                'c_frmMNUIDTR200.BeginInvoke(New Action(Sub()
                '                                           frmLoading.Hide()
                '                                           c_frmMNUIDTR200.Activate()
                '                                           MNBTCMN100.ShowMessage("MSGVWE00021", "取込対象データ", "", "")
                '                                       End Sub))
                frmLoading.Hide()
                c_frmMNUIDTR200.Activate()
                MNBTCMN100.ShowMessage("MSGVWE00021", "取込対象データ", "", "")
                checkReuslt = False
            Else
                Dim selectTCom = (From t In inout_mynoContext.t_company).ToList()
                'Delete data in table t_company
                Dim deleteCom As List(Of t_company) = New List(Of t_company)()
                For i As Integer = 0 To queryListwCompany.Count - 1
                    Application.DoEvents()
                    Dim wCompany As w_company = queryListwCompany(i)
                    Dim deleteComSingle = (From t In selectTCom
                                    Where t.companycd = wCompany.companycd And t.companybranchno = wCompany.companybranchno).SingleOrDefault()
                    If deleteComSingle IsNot Nothing Then
                        deleteCom.Add(deleteComSingle)
                    End If
                Next
                If deleteCom IsNot Nothing And deleteCom.Count > 0 Then
                    inout_mynoContext.t_company.RemoveRange(deleteCom)
                End If
                'Next
                'Insert data to table t_company
                Dim numInsert As Integer = 0
                InsertTComapy(inout_mynoContext, queryListwCompany, numInsert)
                'Input system detail log
                Dim message As String = "ファイル取込件数:" & numInsert.ToString("###,###,###")
                MNBTCMN100.InputLogDetail(inout_mynoContext, in_intSeq, "", message, in_strFilePath)
                'Update error flag
                Dim updateErrorFlg = (From im In inout_mynoContext.t_importlog
                                     Where im.seq = in_intSeq).SingleOrDefault()
                updateErrorFlg.errflg = 0
                'Delete data in table w_company
                Dim selectWCom = (From t In inout_mynoContext.w_company
                                  Where t.seq = in_intSeq).ToList()
                inout_mynoContext.w_company.RemoveRange(selectWCom)
                'c_frmMNUIDTR200.BeginInvoke(New Action(Sub()
                '                                           frmLoading.Hide()
                '                                           c_frmMNUIDTR200.Activate()
                '                                       End Sub))
                frmLoading.Hide()
                c_frmMNUIDTR200.Activate()
            End If
            inout_mynoContext.SaveChanges()
        End If
        Return checkReuslt
    End Function
    ''' <summary>
    ''' Add data to table t_company
    ''' </summary>
    ''' <param name="inout_context"></param>
    ''' <param name="lstwCompany"></param>
    ''' <remarks></remarks>
    Public Sub InsertTComapy(ByRef inout_context As mynoEntities, ByVal lstwCompany As List(Of w_company), ByRef out_intNumInset As Integer)

        Dim tCompany As t_company
        Dim wCompany As w_company
        Dim lsttCompany As List(Of t_company) = New List(Of t_company)()
        Dim currentDate = MNBTCMN100.GetCurrentTimestamp(inout_context)
        For i As Integer = 0 To lstwCompany.Count - 1
            Application.DoEvents()
            wCompany = lstwCompany(i)
            tCompany = New t_company
            tCompany.companycd = wCompany.companycd
            tCompany.companybranchno = wCompany.companybranchno
            tCompany.companyname = wCompany.companyname
            tCompany.companybranchname = wCompany.companybranchname
            tCompany.contactfamilyname = wCompany.contactfamilyname
            tCompany.contactfamilynamekana = wCompany.contactfamilynamekana
            tCompany.contactpostno = wCompany.contactpostno
            tCompany.contactprefecturescd = wCompany.contactprefecturescd
            tCompany.contactaddress1 = wCompany.contactaddress1
            If wCompany.contactaddress2 = "" Or wCompany.contactaddress2 Is Nothing Then
                tCompany.contactaddress2 = Nothing
            Else
                tCompany.contactaddress2 = wCompany.contactaddress2
            End If
            tCompany.contacttel = wCompany.contacttel
            If wCompany.contactfax = "" Or wCompany.contactfax Is Nothing Then
                tCompany.contactfax = Nothing
            Else
                tCompany.contactfax = wCompany.contactfax
            End If
            If wCompany.contactmail = "" Or wCompany.contactmail Is Nothing Then
                tCompany.contactmail = Nothing
            Else
                tCompany.contactmail = wCompany.contactmail
            End If
            If wCompany.contactbelongname1 = "" Or wCompany.contactbelongname1 Is Nothing Then
                tCompany.contactbelongname1 = Nothing
            Else
                tCompany.contactbelongname1 = wCompany.contactbelongname1
            End If
            If wCompany.contactbelongname2 = "" Or wCompany.contactbelongname2 Is Nothing Then
                tCompany.contactbelongname2 = Nothing
            Else
                tCompany.contactbelongname2 = wCompany.contactbelongname2
            End If
            If wCompany.contactnotes = "" Or wCompany.contactnotes Is Nothing Then
                tCompany.contactnotes = Nothing
            Else
                tCompany.contactnotes = wCompany.contactnotes
            End If
            tCompany.trusteeflg01 = wCompany.trusteeflg01
            tCompany.trusteeflg02 = wCompany.trusteeflg02
            tCompany.trusteeflg03 = wCompany.trusteeflg03
            tCompany.trusteeflg04 = wCompany.trusteeflg04
            tCompany.trusteeflg05 = wCompany.trusteeflg05
            tCompany.trusteeflg06 = wCompany.trusteeflg06
            tCompany.trusteeflg07 = wCompany.trusteeflg07
            tCompany.trusteeflg08 = wCompany.trusteeflg08
            tCompany.trusteeflg09 = wCompany.trusteeflg09
            tCompany.trusteeflg10 = wCompany.trusteeflg10
            tCompany.trusteeflg11 = wCompany.trusteeflg11
            tCompany.trusteeflg12 = wCompany.trusteeflg12
            tCompany.trusteeflg13 = "0"
            tCompany.trusteeflg14 = "0"
            tCompany.trusteeflg15 = "0"
            tCompany.trusteeflg16 = "0"
            tCompany.trusteeflg17 = "0"
            tCompany.trusteeflg18 = "0"
            tCompany.trusteeflg19 = "0"
            tCompany.trusteeflg20 = "0"
            tCompany.delivschdate = wCompany.delivschdate
            tCompany.delivdate = Nothing
            tCompany.delschdate = wCompany.delschdate
            tCompany.deldate = Nothing
            tCompany.delflg = 0
            tCompany.deldatetime = Nothing
            tCompany.delusercd = Nothing
            tCompany.adddatetime = currentDate
            tCompany.addjusercd = p_strUserCdLogin
            tCompany.upddatetime = Nothing
            tCompany.updusercd = Nothing
            tCompany.terminalcd = p_strTerminalCdLogin
            lsttCompany.Add(tCompany)
            'inout_context.t_company.Add(tCompany)
            out_intNumInset = out_intNumInset + 1
        Next
        inout_context.t_company.AddRange(lsttCompany)
        inout_context.SaveChanges()

    End Sub
    ''' <summary>
    ''' Check error from file csv
    ''' </summary>
    ''' <param name="inout_context"></param>
    ''' <param name="in_intSeq"></param>
    ''' <param name="lstwCompany"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckErrorCsv(ByRef inout_context As mynoEntities, ByVal in_intSeq As Integer, ByVal lstwCompany As List(Of w_company)) As Integer

        Dim countError As Integer = 0
        'Check exist in table m_prefectures
        Dim queryprefecturescd = (From p In inout_context.m_prefectures Select p).ToList()
        For i As Integer = 0 To lstwCompany.Count - 1
            Application.DoEvents()

            Dim wCompany As w_company = lstwCompany(i)
            Dim csvData = MNBTCMN100.CreateCsvDataImport(wCompany)
            Dim message As String

            'start Change spec 03/09/2015
            'Check exits in table t_private
            If wCompany.companycd <> "" And wCompany.companycd IsNot Nothing And wCompany.companybranchno <> "" And wCompany.companybranchno IsNot Nothing And MNBTCMN100.CheckValidateOnlyNumber(wCompany.companybranchno) = False Then
                'Get exist in table t_private
                Dim checkExist = (From lstPri In inout_context.t_private
                                 Where lstPri.companycd = wCompany.companycd And lstPri.companybranchno = wCompany.companybranchno).ToList()
                'If exist in table t_private show message
                If checkExist.Count > 0 Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00029", "", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.companycd)
                    countError = countError + 1
                    Continue For
                End If
            End If
            'end Change spec 03/09/2015

            'Check nothing companycd
            If wCompany.companycd = "" Or wCompany.companycd Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "企業コード", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                ''Check AlphaNumeric
                If MNBTCMN100.CheckValidCompanyCdOrStaffCd(wCompany.companycd) Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "企業コード", "半角英数", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.companycd)
                    countError = countError + 1
                End If
                'The conversion as Shift JIS to byte array
                Dim bytesData = System.Text.Encoding.GetEncoding(932).GetBytes(wCompany.companycd)
                'Check more than byte
                If bytesData.Length <> 5 Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "企業コード", "5バイト", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.companycd)
                    countError = countError + 1
                End If
            End If
            'Check nothing companybranchno
            If wCompany.companybranchno = "" Or wCompany.companybranchno Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "枝番", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check numberic companybranchno
                If MNBTCMN100.CheckValidateOnlyNumber(wCompany.companybranchno) Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "枝番", "数値", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.companybranchno)
                    countError = countError + 1
                Else
                    'Check companybranchno < 0
                    If Convert.ToInt32(wCompany.companybranchno) < 0 Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "枝番", "正の値", "")
                        ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.companybranchno)
                        countError = countError + 1
                    End If
                    'Check more than byte
                    If MNBTCMN100.checkLength(wCompany.companybranchno, 3) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "枝番", "3バイト以内", "")
                        ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.companybranchno)
                        countError = countError + 1
                    End If
                End If
            End If
            'Check nothing companyname
            If wCompany.companyname = "" Or wCompany.companyname Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "企業名", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check External character companyname
                If MNBTCMN100.CheckExternalCharacter(wCompany.companyname) Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00003", "企業名", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.companyname)
                    countError = countError + 1
                End If
                'Check more than byte
                If MNBTCMN100.checkLength(wCompany.companyname, 80) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "企業名", "80バイト以内", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.companyname)
                    countError = countError + 1
                End If
            End If
            'Check nothing companybranchname
            If wCompany.companybranchname = "" Or wCompany.companybranchname Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "枝番名", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check External character companybranchname
                If MNBTCMN100.CheckExternalCharacter(wCompany.companybranchname) Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00003", "枝番名", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.companybranchname)
                    countError = countError + 1
                End If
                'Check more than byte
                If MNBTCMN100.checkLength(wCompany.companybranchname, 80) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "枝番名", "80バイト以内", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.companybranchname)
                    countError = countError + 1
                End If
            End If
            'Check nothing contactfamilyname
            If wCompany.contactfamilyname = "" Or wCompany.contactfamilyname Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "窓口漢字氏名", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check External character contactfamilyname
                If MNBTCMN100.CheckExternalCharacter(wCompany.contactfamilyname) Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00003", "窓口漢字氏名", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactfamilyname)
                    countError = countError + 1
                End If
                'Check more than byte
                If MNBTCMN100.checkLength(wCompany.contactfamilyname, 80) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口漢字氏名", "80バイト以内", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactfamilyname)
                    countError = countError + 1
                End If
            End If
            'Check nothing contactfamilynamekana
            If wCompany.contactfamilynamekana = "" Or wCompany.contactfamilynamekana Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "窓口カナ氏名", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check External character contactfamilynamekana
                If MNBTCMN100.CheckExternalCharacter(wCompany.contactfamilynamekana) Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00003", "窓口カナ氏名", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactfamilynamekana)
                    countError = countError + 1
                End If
                'Check kana half size wFamily.familynamekana
                If MNBTCMN100.CheckValidateOnlyKatakanaHalfsize(wCompany.contactfamilynamekana) Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口カナ氏名", "半角カナ", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactfamilynamekana)
                    countError = countError + 1
                End If
                'Check more than byte
                If MNBTCMN100.checkLength(wCompany.contactfamilynamekana, 80) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口カナ氏名", "80バイト以内", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactfamilynamekana)
                    countError = countError + 1
                End If
            End If
            'Check nothing contactpostno
            If wCompany.contactpostno = "" Or wCompany.contactpostno Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "窓口郵便番号", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check more than byte
                If MNBTCMN100.checkLength(wCompany.contactpostno, 8) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口郵便番号", "8バイト以内", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactpostno)
                    countError = countError + 1
                End If
                'Check count numberic
                If MNBTCMN100.CountDigitInString(wCompany.contactpostno) <> 7 Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口郵便番号", "123-4567または1234567形式", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactpostno)
                    countError = countError + 1
                End If
            End If
            'Check nothing contactprefecturescd
            If wCompany.contactprefecturescd = "" Or wCompany.contactprefecturescd Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "窓口都道府県コード", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check more than byte
                If MNBTCMN100.checkLength(wCompany.contactprefecturescd, 2) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口都道府県コード", "2バイト", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactprefecturescd)
                    countError = countError + 1
                End If
                'Check exist in table m_prefectures
                Dim query = (From p In queryprefecturescd
                            Where p.prefecturescd = wCompany.contactprefecturescd
                            Select p).ToList()
                If query.Count < 1 Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00018", "都道府県マスタに窓口都道府県コード", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactprefecturescd)
                    countError = countError + 1
                End If
            End If
            'Check nothing contactaddress1
            If wCompany.contactaddress1 = "" Or wCompany.contactaddress1 Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "窓口住所１", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check External character contactaddress1
                If MNBTCMN100.CheckExternalCharacter(wCompany.contactaddress1) Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00003", "窓口住所１", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactaddress1)
                    countError = countError + 1
                End If
                'Check more than byte
                If MNBTCMN100.checkLength(wCompany.contactaddress1, 80) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口住所１", "80バイト以内", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactaddress1)
                    countError = countError + 1
                End If
            End If
            If wCompany.contactaddress2 <> "" Then
                'Check External character contactaddress2
                If MNBTCMN100.CheckExternalCharacter(wCompany.contactaddress2) Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00003", "窓口住所２", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactaddress2)
                    countError = countError + 1
                End If
                'Check more than byte
                If MNBTCMN100.checkLength(wCompany.contactaddress2, 80) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口住所２", "80バイト以内", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactaddress2)
                    countError = countError + 1
                End If
            End If
            'Check nothing contacttel
            If wCompany.contacttel = "" Or wCompany.contacttel Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "窓口電話番号", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check more than byte
                If MNBTCMN100.checkLength(wCompany.contacttel, 13) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口電話番号", "13バイト以内", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contacttel)
                    countError = countError + 1
                End If
            End If
            'Check nothing trusteeflg01
            If wCompany.trusteeflg01 = "" Or wCompany.trusteeflg01 Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "記入票作成フラグ", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check the fixed value 0,1
                If wCompany.trusteeflg01 <> "0" And wCompany.trusteeflg01 <> "1" Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "記入票作成フラグ", "0または1", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg01)
                    countError = countError + 1
                End If
            End If
            'Check nothing trusteeflg02
            If wCompany.trusteeflg02 = "" Or wCompany.trusteeflg02 Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "封入封緘フラグ", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check the fixed value 0,1
                If wCompany.trusteeflg02 <> "0" And wCompany.trusteeflg02 <> "1" Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "封入封緘フラグ", "0または1", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg02)
                    countError = countError + 1
                End If
            End If
            'Check nothing trusteeflg03
            If wCompany.trusteeflg03 = "" Or wCompany.trusteeflg03 Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "個人情報登録フラグ", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check the fixed value 0,1
                If wCompany.trusteeflg03 <> "0" And wCompany.trusteeflg03 <> "1" Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "個人情報登録フラグ", "0または1", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg03)
                    countError = countError + 1
                End If
            End If
            'Check nothing trusteeflg04
            If wCompany.trusteeflg04 = "" Or wCompany.trusteeflg04 Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "データ納品フラグ", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check the fixed value 0,1
                If wCompany.trusteeflg04 <> "0" And wCompany.trusteeflg04 <> "1" Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "データ納品フラグ", "0または1", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg04)
                    countError = countError + 1
                End If
            End If
            'Check nothing trusteeflg05
            If wCompany.trusteeflg05 = "" Or wCompany.trusteeflg05 Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "個人宅発送フラグ", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check the fixed value 0,1
                If wCompany.trusteeflg05 <> "0" And wCompany.trusteeflg05 <> "1" Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "個人宅発送フラグ", "0または1", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg05)
                    countError = countError + 1
                End If
            End If
            'Check nothing trusteeflg06
            If wCompany.trusteeflg06 = "" Or wCompany.trusteeflg06 Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "事業所単位発送フラグ", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check the fixed value 0,1
                If wCompany.trusteeflg06 <> "0" And wCompany.trusteeflg06 <> "1" Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "事業所単位発送フラグ", "0または1", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg06)
                    countError = countError + 1
                End If
            End If
            'Check nothing trusteeflg07
            If wCompany.trusteeflg07 = "" Or wCompany.trusteeflg07 Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "個人回収フラグ", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check the fixed value 0,1
                If wCompany.trusteeflg07 <> "0" And wCompany.trusteeflg07 <> "1" Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "個人回収フラグ", "0または1", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg07)
                    countError = countError + 1
                End If
            End If
            'Check nothing trusteeflg08
            If wCompany.trusteeflg08 = "" Or wCompany.trusteeflg08 Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "未着督促フラグ", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check the fixed value 0,1
                If wCompany.trusteeflg08 <> "0" And wCompany.trusteeflg08 <> "1" Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "未着督促フラグ", "0または1", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg08)
                    countError = countError + 1
                End If
            End If
            'Check nothing trusteeflg09
            If wCompany.trusteeflg09 = "" Or wCompany.trusteeflg09 Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "不備確認(一括)フラグ", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check the fixed value 0,1
                If wCompany.trusteeflg09 <> "0" And wCompany.trusteeflg09 <> "1" Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "不備確認(一括)フラグ", "0または1", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg09)
                    countError = countError + 1
                End If
            End If
            'Check nothing trusteeflg10
            If wCompany.trusteeflg10 = "" Or wCompany.trusteeflg10 Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "不備確認(個人)フラグ", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check the fixed value 0,1
                If wCompany.trusteeflg10 <> "0" And wCompany.trusteeflg10 <> "1" Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "不備確認(個人)フラグ", "0または1", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg10)
                    countError = countError + 1
                End If
            End If
            'Check nothing trusteeflg11
            If wCompany.trusteeflg11 = "" Or wCompany.trusteeflg11 Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "追加封入フラグ", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check the fixed value 0,1
                If wCompany.trusteeflg11 <> "0" And wCompany.trusteeflg11 <> "1" Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "追加封入フラグ", "0または1", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg11)
                    countError = countError + 1
                End If
            End If
            'Check nothing trusteeflg12
            If wCompany.trusteeflg12 = "" Or wCompany.trusteeflg12 Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "データスキャンフラグ", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
            Else
                'Check the fixed value 0,1
                If wCompany.trusteeflg12 <> "0" And wCompany.trusteeflg12 <> "1" Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "データスキャンフラグ", "0または1", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg12)
                    countError = countError + 1
                End If
            End If
            Dim checkdelivschdate As Boolean = False
            Dim checkdelschdate As Boolean = False
            'Check nothing delivschdate
            If wCompany.delivschdate = "" Or wCompany.delivschdate Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "納品予定日", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
                checkdelivschdate = True
            Else
                'Check Date delivschdate
                If MNBTCMN100.CheckValidateDateMulType(wCompany.delivschdate) <> 0 Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00013", "納品予定日", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.delivschdate)
                    countError = countError + 1
                    checkdelivschdate = True
                End If
            End If
            'Check nothing delschdate
            If wCompany.delschdate = "" Or wCompany.delschdate Is Nothing Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00001", "削除予定日", "", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, Nothing)
                countError = countError + 1
                checkdelschdate = True
            Else
                'Check Date delschdate
                If MNBTCMN100.CheckValidateDateMulType(wCompany.delschdate) <> 0 Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00013", "削除予定日", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.delschdate)
                    countError = countError + 1
                    checkdelschdate = True
                End If
            End If
            'Check 削除予定日＜納品予定日, insert error 
            If checkdelschdate = False And checkdelivschdate = False Then
                If MNBTCMN100.CheckStartDateAndEndDate(wCompany.delivschdate, wCompany.delschdate) Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00016", "納品予定日", "削除予定日", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.delschdate)
                    countError = countError + 1
                End If
            End If

            If wCompany.contactfax <> "" And wCompany.contactfax IsNot Nothing Then
                'Check more than byte
                If MNBTCMN100.checkLength(wCompany.contactfax, 13) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口ＦＡＸ", "13バイト以内", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactfax)
                    countError = countError + 1
                End If
            End If
            If wCompany.contactmail <> "" And wCompany.contactmail IsNot Nothing Then
                'Check more than byte
                If MNBTCMN100.checkLength(wCompany.contactmail, 50) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口メールアドレス", "50バイト以内", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactmail)
                    countError = countError + 1
                End If
            End If
            If wCompany.contactbelongname1 <> "" And wCompany.contactbelongname1 IsNot Nothing Then
                'Check more than byte
                If MNBTCMN100.checkLength(wCompany.contactbelongname1, 80) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口所属名１", "80バイト以内", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactbelongname1)
                    countError = countError + 1
                End If
                'Check External character contactaddress2
                If MNBTCMN100.CheckExternalCharacter(wCompany.contactbelongname1) Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00003", "窓口所属名１", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactbelongname1)
                    countError = countError + 1
                End If
            End If
            If wCompany.contactbelongname2 <> "" And wCompany.contactbelongname2 IsNot Nothing Then
                'Check more than byte
                If MNBTCMN100.checkLength(wCompany.contactbelongname2, 80) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口所属名2", "80バイト以内", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactbelongname2)
                    countError = countError + 1
                End If
                'Check External character contactaddress2
                If MNBTCMN100.CheckExternalCharacter(wCompany.contactbelongname2) Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00003", "窓口所属名2", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactbelongname2)
                    countError = countError + 1
                End If
            End If

            'Check count records in table w_company
            If wCompany.companycd <> "" And wCompany.companybranchno <> "" Then
                Dim query = (From c In inout_context.w_company
                            Where c.seq = in_intSeq And c.companycd = wCompany.companycd And c.companybranchno = wCompany.companybranchno
                            Select c).ToList()
                If query.Count >= 2 Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00024", "対象の企業情報", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.companycd & " , " & wCompany.companybranchno)
                    countError = countError + 1
                End If
            End If
            'Check Error if both of 1
            If wCompany.trusteeflg09 = "1" And wCompany.trusteeflg10 = "1" Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00015", "不備確認(一括)フラグと不備確認(個人)フラグ", "一つのみ", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg09 & " , " & wCompany.trusteeflg10)
                countError = countError + 1
            End If
            'Check Error if both of 0
            If wCompany.trusteeflg09 = "0" And wCompany.trusteeflg10 = "0" Then
                message = MNBTCMN100.GetMessageContent("MSGVWE00015", "不備確認(一括)フラグと不備確認(個人)フラグ", "一つのみ", "")
                ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.trusteeflg09 & " , " & wCompany.trusteeflg10)
                countError = countError + 1
            End If

            'Check length contactnotes
            If wCompany.contactnotes <> "" And wCompany.contactnotes IsNot Nothing Then
                'Check External character contactaddress2
                If MNBTCMN100.CheckExternalCharacter(wCompany.contactnotes) Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00003", "窓口メモ", "", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactnotes)
                    countError = countError + 1
                End If
                If MNBTCMN100.checkLength(wCompany.contactnotes, 255) = False Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00015", "窓口メモ", "255バイト以内", "")
                    ImportDetailLog(inout_context, in_intSeq, wCompany.line, message, csvData, wCompany.contactnotes)
                    countError = countError + 1
                End If
            End If
        Next

        Return countError
    End Function

    ''' <summary>
    ''' Insert data to table t_importdetaillog
    ''' </summary>
    ''' <param name="inout_context"></param>
    ''' <param name="in_intSeq"></param>
    ''' <param name="in_intLine"></param>
    ''' <param name="in_strMessage"></param>
    ''' <param name="in_strRecData"></param>
    ''' <param name="in_intColData"></param>
    ''' <remarks></remarks>
    Public Sub ImportDetailLog(ByRef inout_context As mynoEntities, ByVal in_intSeq As Integer,
                               ByVal in_intLine As Integer, ByVal in_strMessage As String,
                               ByVal in_strRecData As String, ByVal in_intColData As String)
        Try
            Dim currentDate = MNBTCMN100.GetCurrentTimestamp(inout_context)
            Dim item As New t_importdetaillog
            item.seq = in_intSeq
            If p_intSeq1st > 0 And p_intSeq1st = p_intSeq2nd Then
                p_intSeqDetail = p_intSeqDetail + 1
            ElseIf p_intSeq1st <> p_intSeq2nd Then
                p_intSeqDetail = 1
            End If
            item.detailseq = p_intSeqDetail
            item.line = in_intLine
            item.errflg = 2
            item.message = in_strMessage
            item.recdata = in_strRecData
            item.coldata = in_intColData
            item.adddatetime = currentDate.ToString("yyyy/MM/dd HH:mm:ss")
            item.addjusercd = p_strUserCdLogin
            item.terminalcd = p_strTerminalCdLogin
            inout_context.t_importdetaillog.Add(item)
            inout_context.SaveChanges()
            p_intSeq2nd = in_intSeq
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub
    ''' <summary>
    ''' Export file csv by Link fileName
    ''' </summary>
    ''' <param name="inout_mynoContext"></param>
    ''' <param name="in_intSeq"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExportCsvLinkFileName(ByRef inout_mynoContext As mynoEntities, ByVal in_intSeq As Integer) As Boolean
        Try
            ExportCsvLinkFileName = False
            Using transScope As DbContextTransaction = inout_mynoContext.Database.BeginTransaction(IsolationLevel.Chaos)
                Dim queryData = From t In inout_mynoContext.t_importlog
                            Where t.seq = in_intSeq
                            Select t
                Dim pathSave As String = MNBTCMN100.GetConfig("Export_Dir_Csv_CompanyDataFile", "config.ini")
                'Check Link file path exists
                If IO.Directory.Exists(pathSave) Then
                    Dim query = queryData.SingleOrDefault()
                    'Export file csv
                    If query IsNot Nothing Then
                        Dim filepath As String
                        filepath = pathSave + query.filename
                        If query.csvfile IsNot Nothing Then
                            'Delete file csv if exists
                            If IO.File.Exists(filepath) Then
                                File.Delete(filepath)
                            End If
                            Dim csvOut As String = System.Text.Encoding.GetEncoding("shift_jis").GetString(query.csvfile)
                            File.AppendAllText(filepath, csvOut, System.Text.Encoding.GetEncoding("shift_jis"))

                            'Call function setting first log system
                            Dim seq As Integer = MNBTCMN100.InputLogMaster(inout_mynoContext, "2", "MNUIDTR200", "", Nothing)
                            'replace parameter input log
                            Dim sqlStrQuery As String = queryData.ToString()
                            If sqlStrQuery.IndexOf("@p__linq__0") > 0 Then
                                sqlStrQuery = sqlStrQuery.Replace("Project1", "P1")
                                sqlStrQuery = sqlStrQuery.Replace("@p__linq__0", "'" & in_intSeq.ToString() & "'")
                            End If
                            'system detail log
                            Dim message As String = "ファイル出力 件数:1" & " (" & sqlStrQuery & ")"
                            MNBTCMN100.InputLogDetail(inout_mynoContext, seq, "", message, filepath)
                            transScope.Commit()

                            Return True
                        End If
                    End If
                Else
                    frmLoading.Close()
                    MNBTCMN100.ShowMessage("MSGVWE00018", "企業情報取込ファイルの出力先ディレクトリ", "", "")
                    Return False
                End If
            End Using
        Catch ex As Exception
            frmLoading.Close()
            MNBTCMN100.ShowMessageException()
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Get Data For Export Xlsx
    ''' </summary>
    ''' <param name="inout_mynoContext"></param>
    ''' <param name="in_intSeq"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataExportXlsx(ByRef inout_mynoContext As mynoEntities, ByVal in_intSeq As Integer, ByRef out_strQuery As String) As List(Of DataImportXlsx)
        Try
            Dim queryXlsx = (From il In inout_mynoContext.t_importlog
                            Join dl In inout_mynoContext.t_importdetaillog On il.seq Equals dl.seq
                            Where il.seq = in_intSeq And dl.errflg = 2
                            Order By dl.detailseq, dl.line Ascending
                            Select New DataImportXlsx With {
                                    .Seq = il.seq,
                                    .CompanyCd = il.companycd,
                                    .CompanyBranchCd = il.companybranchno,
                                    .Impdatetime = il.impdatetime,
                                    .Line = dl.line,
                                    .Message = dl.message,
                                    .Recdata = dl.recdata,
                                    .Coldata = dl.coldata
                                })
            'replace parameter input log
            Dim sqlStrQuery As String = queryXlsx.ToString()
            If sqlStrQuery.IndexOf("@p__linq__0") > 0 Then
                sqlStrQuery = sqlStrQuery.Replace("Project1", "P1")
                sqlStrQuery = sqlStrQuery.Replace("@p__linq__0", "'" & in_intSeq.ToString() & "'")
            End If
            out_strQuery = sqlStrQuery
            Return queryXlsx.ToList()
        Catch ex As Exception
            frmLoading.Close()
            MNBTCMN100.ShowMessageException()
            Return Nothing
        End Try
    End Function

    Public Function ExportXlsxListError(ByVal in_lstListError As List(Of DataImportXlsx), ByVal in_context As mynoEntities, ByVal in_strQuery As String, ByVal in_intSeq As Integer, ByRef inout_strFilePathSave As String) As Boolean
        Dim wb As XSSFWorkbook = Nothing
        Try
            Using transScope As DbContextTransaction = in_context.Database.BeginTransaction(IsolationLevel.Chaos)
                Dim FilePath As String = Directory.GetCurrentDirectory + "\Template\Temp_MNUIDTR200.xlsx"
                'Open book here
                Using fs As New FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Read)
                    wb = New XSSFWorkbook(fs)
                End Using

                'Get sheet by the name
                Dim sht1 As XSSFSheet = wb.GetSheet("企業情報取込エラーリスト")
                Dim printRow As Integer = 0
                'Copy row temp in excel
                For i As Integer = 0 To in_lstListError.Count - 2
                    Dim rowCopy As Integer
                    rowCopy = 7 + i + 1
                    sht1.CopyRow(7, rowCopy)
                    printRow = rowCopy
                Next

                Dim currentDate As DateTime = MNBTCMN100.GetCurrentTimestamp(in_context)
                'Set data to excel
                Dim companyCd As String = in_lstListError(0).CompanyCd
                Dim branchCd As String = in_lstListError(0).CompanyBranchCd.ToString()
                'Update data 件数
                sht1.GetRow(2).GetCell(4).SetCellValue("件数：" & in_lstListError.Count.ToString & "件")
                'Update data 基礎データ取込日時
                If in_lstListError(0).Impdatetime IsNot Nothing Then
                    sht1.GetRow(3).GetCell(2).SetCellValue(CDate(in_lstListError(0).Impdatetime).ToString("yyyy/MM/dd HH:mm"))
                Else
                    sht1.GetRow(3).GetCell(2).SetCellValue(String.Empty)
                End If
                'Update data 出力日時
                sht1.GetRow(3).GetCell(4).SetCellValue(currentDate.ToString("yyyy/MM/dd HH:mm"))
                'Update data SEQ
                If in_lstListError(0).Seq <> Nothing Then
                    sht1.GetRow(4).GetCell(4).SetCellValue(in_lstListError(0).Seq)
                Else
                    sht1.GetRow(4).GetCell(4).SetCellValue(String.Empty)
                End If
                'Set data to table
                Dim errorItem As DataImportXlsx
                For i As Integer = 0 To in_lstListError.Count - 1
                    errorItem = in_lstListError(i)
                    Dim row As Integer = 7 + i
                    sht1.GetRow(row).Height = 270
                    'Update data 行数
                    If errorItem.Line IsNot Nothing Then
                        sht1.GetRow(row).GetCell(1).SetCellValue(CLng(errorItem.Line).ToString("###,###,###") & "行目")
                    Else
                        sht1.GetRow(row).GetCell(1).SetCellValue(String.Empty)
                    End If
                    'Update data エラーメッセージ
                    If errorItem.Message <> Nothing Then
                        sht1.GetRow(row).GetCell(2).SetCellValue(errorItem.Message)
                    Else
                        sht1.GetRow(row).GetCell(2).SetCellValue(String.Empty)
                    End If
                    'Update data エラー項目
                    If errorItem.Coldata <> Nothing Then
                        sht1.GetRow(row).GetCell(3).SetCellValue(errorItem.Coldata)
                    Else
                        sht1.GetRow(row).GetCell(3).SetCellValue(String.Empty)
                    End If
                    'Update data データ内容
                    If errorItem.Recdata <> Nothing Then
                        sht1.GetRow(row).GetCell(4).SetCellValue(errorItem.Recdata)
                    Else
                        sht1.GetRow(row).GetCell(4).SetCellValue(String.Empty)
                    End If
                Next
                If printRow = 0 Then
                    wb.SetPrintArea(0, 0, 4, 0, 8)
                Else
                    wb.SetPrintArea(0, 0, 4, 0, printRow)
                End If

                Dim pathSave As String = MNBTCMN100.GetConfig("Export_Dir_Xlsx_CompanyDataImportErrorList", "config.ini")
                If IO.Directory.Exists(pathSave) Then
                    Dim fileName As String = MNBTCMN100.GetConfig("PrefixFileName_Xlsx_CompanyDataImportErrorList", "config.ini")
                    If IO.Directory.Exists(pathSave) = False Then
                        IO.Directory.CreateDirectory(pathSave)
                    End If
                    'Save file
                    Dim dateNow As String = currentDate.ToString("yyyyMMddHHmmss")
                    Dim filePathSave As String = pathSave & fileName & "_" & dateNow & "_" & in_intSeq.ToString("0000000000") & ".xlsx"
                    Using fs As New FileStream(filePathSave, FileMode.Create)
                        wb.Write(fs)
                    End Using
                    'InsertLog
                    Dim seq As Integer = MNBTCMN100.InputLogMaster(in_context, "2", "MNUIDTR200", "", Nothing)
                    Dim message As String = "ファイル出力 件数:" & in_lstListError.Count.ToString("###,###,###") & " (" & in_strQuery & ")"
                    inout_strFilePathSave = filePathSave
                    MNBTCMN100.InputLogDetail(in_context, seq, "", message, filePathSave)
                Else
                    frmLoading.Close()
                    MNBTCMN100.ShowMessage("MSGVWE00018", "企業情報取込エラーリストの出力先ディレクトリ", "", "")
                    Return False
                End If
                transScope.Commit()
                Return True
            End Using
        Catch ex As Exception
            frmLoading.Close()
            MNBTCMN100.ShowMessageException()
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Validate export csv by company
    ''' </summary>
    ''' <param name="companyCd"></param>
    ''' <param name="branchCd"></param>
    ''' <param name="branchCdStart"></param>
    ''' <param name="branchCdEnd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateExport(ByVal companyCd As String, ByVal branchCd As String, ByVal branchCdStart As String, ByVal branchCdEnd As String) As Boolean
        Try
            'Check companycd is blank
            If companyCd = "" Or companyCd Is Nothing Then
                MNBTCMN100.ShowMessage("MSGVWE00001", "企業コード", "", "")
                Return False
            End If
            'Check companycd is blank
            If branchCd <> "" And branchCd IsNot Nothing Then
                'Check Number branch
                If MNBTCMN100.CheckValidateCompanyBranchNo(branchCd) Then
                    MNBTCMN100.ShowMessage("MSGVWE00015", "枝番", "数値", "")
                    Return False
                End If
            End If
            'Check Number branch start
            If branchCdStart <> "" And MNBTCMN100.CheckValidateOnlyNumber(branchCdStart) Then
                MNBTCMN100.ShowMessage("MSGVWE00015", "枝番　自", "数値", "")
                Return False
            End If
            'Check Number branch End
            If branchCdEnd <> "" And MNBTCMN100.CheckValidateOnlyNumber(branchCdEnd) Then
                MNBTCMN100.ShowMessage("MSGVWE00015", "枝番　至", "数値", "")
                Return False
            End If
            'Check branch End < branch start
            If branchCdStart <> "" And branchCdEnd <> "" Then
                If MNBTCMN100.CheckValidateOnlyNumber(branchCdStart) = False And MNBTCMN100.CheckValidateOnlyNumber(branchCdEnd) = False Then
                    If CInt(branchCdStart) > CInt(branchCdEnd) Then
                        MNBTCMN100.ShowMessage("MSGVWE00021", "枝番　自～至", "", "")
                        Return False
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            frmLoading.Close()
            MNBTCMN100.ShowMessageException()
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Export csv company
    ''' </summary>
    ''' <param name="companyCd"></param>
    ''' <param name="branchCd"></param>
    ''' <param name="branchCdStart"></param>
    ''' <param name="branchCdEnd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExportCsv(ByVal companyCd As String, ByVal branchCd As String, ByVal branchCdStart As String, ByVal branchCdEnd As String) As Boolean
        p_mynoContext = New mynoEntities()
        Try

            Dim lstBranchNo As List(Of Nullable(Of Integer)) = New List(Of Nullable(Of Integer))
            For Each item In branchCd.Split(",")
                If Not String.IsNullOrEmpty(item) Then
                    lstBranchNo.Add(item)
                End If
            Next

            Dim selQuery As List(Of t_company) = New List(Of t_company)()
            Dim strQuery As String = String.Empty
            If branchCdStart = "" And branchCdEnd = "" Then
                If branchCd <> "" Then
                    Dim selData = From c In p_mynoContext.t_company
                              Where c.companycd = companyCd And lstBranchNo.Contains(c.companybranchno)
                              Select c
                    strQuery = selData.ToString()
                    selQuery = selData.ToList()
                Else
                    Dim selData = From c In p_mynoContext.t_company
                              Where c.companycd = companyCd
                              Select c
                    strQuery = selData.ToString()
                    selQuery = selData.ToList()
                End If
            Else
                If branchCd <> "" And branchCdStart <> "" And branchCdEnd <> "" Then
                    Dim selData = From c In p_mynoContext.t_company
                               Where c.companycd = companyCd And (lstBranchNo.Contains(c.companybranchno) Or (c.companybranchno >= CInt(branchCdStart) And c.companybranchno <= CInt(branchCdEnd)))
                               Select c
                    strQuery = selData.ToString()
                    selQuery = selData.ToList()
                ElseIf branchCd <> "" And branchCdStart <> "" And branchCdEnd = "" Then
                    Dim selData = From c In p_mynoContext.t_company
                            Where c.companycd = companyCd And (lstBranchNo.Contains(c.companybranchno) Or (c.companybranchno >= CInt(branchCdStart)))
                            Select c
                    strQuery = selData.ToString()
                    selQuery = selData.ToList()
                ElseIf branchCd <> "" And branchCdStart = "" And branchCdEnd <> "" Then
                    Dim selData = From c In p_mynoContext.t_company
                            Where c.companycd = companyCd And (lstBranchNo.Contains(c.companybranchno) Or (c.companybranchno <= CInt(branchCdEnd)))
                            Select c
                    strQuery = selData.ToString()
                    selQuery = selData.ToList()
                ElseIf branchCd = "" And branchCdStart <> "" And branchCdEnd <> "" Then
                    Dim selData = From c In p_mynoContext.t_company
                            Where c.companycd = companyCd And (c.companybranchno >= CInt(branchCdStart) And c.companybranchno <= CInt(branchCdEnd))
                            Select c
                    strQuery = selData.ToString()
                    selQuery = selData.ToList()
                ElseIf branchCd = "" And branchCdStart <> "" And branchCdEnd = "" Then
                    Dim selData = From c In p_mynoContext.t_company
                            Where c.companycd = companyCd And (c.companybranchno >= CInt(branchCdStart))
                            Select c
                    strQuery = selData.ToString()
                    selQuery = selData.ToList()
                ElseIf branchCd = "" And branchCdStart = "" And branchCdEnd <> "" Then
                    Dim selData = From c In p_mynoContext.t_company
                            Where c.companycd = companyCd And (c.companybranchno <= CInt(branchCdEnd))
                            Select c
                    strQuery = selData.ToString()
                    selQuery = selData.ToList()
                End If
            End If

            If selQuery.Count = 0 Then
                frmLoading.Close()
                MNBTCMN100.ShowMessage("MSGVWE00017", "", "", "")
                Return False
            Else
                Dim pathSave As String = MNBTCMN100.GetConfig("Export_Dir_Csv_CompanyDataExport", "config.ini")
                'Check Link file path exists
                If IO.Directory.Exists(pathSave) Then
                    Dim companyExport As String = selQuery(0).companycd
                    Dim branchExport As String = String.Empty
                    If selQuery.Count > 1 Then
                        branchExport = "mlt"
                    Else
                        branchExport = selQuery(0).companybranchno.ToString()
                        Dim pad As Char
                        pad = "0"c
                        branchExport = branchExport.PadLeft(3, pad)
                    End If
                    Dim contents As StringBuilder = New StringBuilder()
                    For i As Integer = 0 To selQuery.Count - 1
                        'Add companycd to StringBuilder
                        If selQuery(i).companycd <> Nothing Then
                            contents.Append(selQuery(i).companycd)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add companybranchno to StringBuilder
                        If selQuery(i).companybranchno > -1 Then
                            'Dim str As String = selQuery(i).companybranchno.ToString()
                            'Dim pad As Char
                            'pad = "0"c
                            'str = str.PadLeft(3, pad)
                            contents.Append(selQuery(i).companybranchno.ToString())
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add companyname to StringBuilder
                        If selQuery(i).companyname <> Nothing Then
                            contents.Append(selQuery(i).companyname)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add companybranchname to StringBuilder
                        If selQuery(i).companybranchname <> Nothing Then
                            contents.Append(selQuery(i).companybranchname)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add companybranchname to StringBuilder
                        If selQuery(i).contactfamilyname <> Nothing Then
                            contents.Append(selQuery(i).contactfamilyname)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add contactfamilynamekana to StringBuilder
                        If selQuery(i).contactfamilynamekana <> Nothing Then
                            contents.Append(selQuery(i).contactfamilynamekana)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add contactpostno to StringBuilder
                        If selQuery(i).contactpostno <> Nothing Then
                            contents.Append(selQuery(i).contactpostno)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add contactprefecturescd to StringBuilder
                        If selQuery(i).contactprefecturescd <> Nothing Then
                            contents.Append(selQuery(i).contactprefecturescd)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add contactaddress1 to StringBuilder
                        If selQuery(i).contactaddress1 <> Nothing Then
                            contents.Append(selQuery(i).contactaddress1)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add contactaddress2 to StringBuilder
                        If selQuery(i).contactaddress2 <> Nothing Then
                            contents.Append(selQuery(i).contactaddress2)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add contacttel to StringBuilder
                        If selQuery(i).contacttel <> Nothing Then
                            contents.Append(selQuery(i).contacttel)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add contactfax to StringBuilder
                        If selQuery(i).contactfax <> Nothing Then
                            contents.Append(selQuery(i).contactfax)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add contactmail to StringBuilder
                        If selQuery(i).contactmail <> Nothing Then
                            contents.Append(selQuery(i).contactmail)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add contactbelongname1 to StringBuilder
                        If selQuery(i).contactbelongname1 <> Nothing Then
                            contents.Append(selQuery(i).contactbelongname1)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add contactbelongname2 to StringBuilder
                        If selQuery(i).contactbelongname2 <> Nothing Then
                            contents.Append(selQuery(i).contactbelongname2)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add contactnotes to StringBuilder
                        If selQuery(i).contactnotes <> Nothing Then
                            contents.Append(selQuery(i).contactnotes)
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add trusteeflg01 to StringBuilder
                        If selQuery(i).trusteeflg01 IsNot Nothing Then
                            contents.Append(CInt(selQuery(i).trusteeflg01).ToString())
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add trusteeflg02 to StringBuilder
                        If selQuery(i).trusteeflg02 IsNot Nothing Then
                            contents.Append(CInt(selQuery(i).trusteeflg02).ToString())
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add trusteeflg03 to StringBuilder
                        If selQuery(i).trusteeflg03 IsNot Nothing Then
                            contents.Append(CInt(selQuery(i).trusteeflg03).ToString())
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add trusteeflg04 to StringBuilder
                        If selQuery(i).trusteeflg04 IsNot Nothing Then
                            contents.Append(CInt(selQuery(i).trusteeflg04).ToString())
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add trusteeflg05 to StringBuilder
                        If selQuery(i).trusteeflg05 IsNot Nothing Then
                            contents.Append(CInt(selQuery(i).trusteeflg05).ToString())
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add trusteeflg06 to StringBuilder
                        If selQuery(i).trusteeflg06 IsNot Nothing Then
                            contents.Append(CInt(selQuery(i).trusteeflg06).ToString())
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add trusteeflg07 to StringBuilder
                        If selQuery(i).trusteeflg07 IsNot Nothing Then
                            contents.Append(CInt(selQuery(i).trusteeflg07).ToString())
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add trusteeflg08 to StringBuilder
                        If selQuery(i).trusteeflg08 IsNot Nothing Then
                            contents.Append(CInt(selQuery(i).trusteeflg08).ToString())
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add trusteeflg09 to StringBuilder
                        If selQuery(i).trusteeflg09 IsNot Nothing Then
                            contents.Append(CInt(selQuery(i).trusteeflg09).ToString())
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add trusteeflg10 to StringBuilder
                        If selQuery(i).trusteeflg10 IsNot Nothing Then
                            contents.Append(CInt(selQuery(i).trusteeflg10).ToString())
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add trusteeflg11 to StringBuilder
                        If selQuery(i).trusteeflg11 IsNot Nothing Then
                            contents.Append(CInt(selQuery(i).trusteeflg11).ToString())
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add trusteeflg12 to StringBuilder
                        If selQuery(i).trusteeflg12 IsNot Nothing Then
                            contents.Append(CInt(selQuery(i).trusteeflg12).ToString())
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add delivschdate to StringBuilder
                        If selQuery(i).delivschdate IsNot Nothing Then
                            contents.Append(String.Format("{0:yyyy/MM/dd}", selQuery(i).delivschdate))
                        Else
                            contents.Append("")
                        End If
                        contents.Append(",")
                        'Add delschdate to StringBuilder
                        If selQuery(i).delschdate IsNot Nothing Then
                            contents.Append(String.Format("{0:yyyy/MM/dd}", selQuery(i).delschdate))
                        Else
                            contents.Append("")
                        End If
                        'Add new line
                        If i + 1 < selQuery.Count Then
                            contents.Append(Environment.NewLine)
                        End If
                    Next
                    'Get file name csv in config.ini
                    Dim prefixFileName As String = MNBTCMN100.GetConfig("PrefixFileName_Csv_CompanyDataExport", "config.ini")
                    Dim dateNow As String = MNBTCMN100.GetCurrentTimestamp(p_mynoContext).ToString("yyyyMMddHHmmss")
                    Dim filepath As String
                    filepath = pathSave & prefixFileName & "_" & companyExport & "_" & branchExport & "_" & dateNow & ".csv"
                    If IO.File.Exists(filepath) Then
                        File.Delete(filepath)
                    End If
                    'Push data to file csv
                    File.AppendAllText(filepath, contents.ToString(), System.Text.Encoding.GetEncoding("shift_jis"))
                    'Call function setting first log system
                    Using transScope As DbContextTransaction = p_mynoContext.Database.BeginTransaction(IsolationLevel.Chaos)
                        Dim seq As Integer = MNBTCMN100.InputLogMaster(p_mynoContext, "2", "MNUIDTR200", "", Nothing)
                        'system detail log
                        Dim message As String = "ファイル出力 件数:" & selQuery.Count.ToString("###,###,###") & " (" & strQuery & ")"
                        MNBTCMN100.InputLogDetail(p_mynoContext, seq, "", message, filepath)
                        transScope.Commit()
                    End Using
                    Return True
                Else
                    frmLoading.Close()
                    MNBTCMN100.ShowMessage("MSGVWE00018", "企業情報エクスポートの出力先ディレクトリ", "", "")
                    Return False
                End If
            End If

        Catch ex As Exception
            frmLoading.Close()
            MNBTCMN100.ShowMessageException()
            Return False
        End Try
    End Function
End Class
Public Class DataImportGridItem
    Public Property Seq As String
    Public Property Impdatetime As Date?
    Public Property Impusercd As String
    Public Property Filename As String
    Public Property ImportType As String
    Public Property ErrFlg As String
    Public Property DownloadFlg As String
End Class
Public Class DataImportXlsx
    Public Property Seq As String
    Public Property CompanyCd As String
    Public Property CompanyBranchCd As Integer?
    Public Property Impdatetime As Date?
    Public Property Line As Long?
    Public Property Message As String
    Public Property Recdata As String
    Public Property Coldata As String
End Class
