'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNUIDTR100
'*  機能名称：基礎データ取込処理
'*  処理　　：基礎データ取込処理（ＢＬ）
'*  内容　　：基礎データ取込処理のビジネスロジック
'*  ファイル：MNUIDTR100CTL.vb
'*  備考　　：
'*
'*  Created：2015/07/01 RS. ThangNB
'***************************************************************************************
Imports System.Data.Entity
Imports System.IO
Imports System.Runtime.Remoting.Contexts
Imports NPOI.SS.Formula.Functions
Imports MyNo.Common
Imports System.Text
Imports NPOI.XSSF.UserModel

Public Class MNUIDTR100CTL

    Private p_mynoContext As mynoEntities
    Private p_strCompanyName As String
    Private p_strCompanyBranchName As String

    Private c_frmMNUIDTR100 As Form

    Public Sub New(in_frmMNUIDTR100 As Form)
        c_frmMNUIDTR100 = in_frmMNUIDTR100
    End Sub

    ''' <summary>
    ''' Get Data ImportLogs Paging
    ''' </summary>
    ''' <param name="in_intNumItem"></param>
    ''' <param name="in_intNumPage"></param>
    ''' <param name="in_blnFlagNextPage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataImportLogsPaging(ByVal in_intNumItem As Integer, ByVal in_intNumPage As Integer, ByVal in_blnFlagNextPage As Boolean) As List(Of DataImportLogGridItem)
        Try
            Dim intSkip As Integer
            If in_blnFlagNextPage = False Then
                intSkip = in_intNumItem * (in_intNumPage - 1) - in_intNumItem
            Else
                intSkip = in_intNumItem * in_intNumPage
            End If
            p_mynoContext = New mynoEntities()
            'Get data from table t_importlog
            Dim queryList = From impLog In p_mynoContext.t_importlog
                            Order By impLog.seq Descending
                            Where impLog.filetype = 1 OrElse impLog.filetype = 2
                            Select New DataImportLogGridItem With {
                                .seq = impLog.seq,
                                .dataImpDateTime = impLog.impdatetime,
                                .dataImpUserCD = impLog.impusercd,
                                .dataFileName = impLog.filename,
                                .dataCompanyCD = impLog.companycd,
                                .dataCompanyBranchNo = impLog.companybranchno,
                                .importType = If(impLog.imptype = 1, "全件", If(impLog.imptype = 2, "新規", String.Empty)),
                                .fileType = If(impLog.filetype = 1, "個人", If(impLog.filetype = 2, "家族", String.Empty)),
                                .dataResult = If(impLog.errflg = 0, "○", If(impLog.errflg = 1, "○", "☓")),
                                .dataError = If(impLog.errflg = 0, Nothing, If(impLog.errflg = 1, "重複データ", "ダウンロード"))
                                }
            Return queryList.Skip(intSkip).Take(in_intNumItem).ToList()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' <summary>
    ''' Check input data on screen
    ''' </summary>
    ''' <param name="in_strCompanyCd">companyCd as String</param>
    ''' <param name="in_strCompanyBranchNo">companyBranchNo as String</param>
    ''' <param name="in_strFilePath">filePath as String</param>
    ''' <remarks></remarks>
    Public Function CheckInputSingle(ByRef in_strCompanyCd As Control, ByRef in_strCompanyBranchNo As Control, ByRef in_strFilePath As Control) As Boolean
        Try
            If in_strCompanyCd.Text = "" OrElse in_strCompanyCd.Text = Nothing Then
                MNBTCMN100.ShowMessage("MSGVWE00001", "企業コード", "", "")
                Return False
            ElseIf MNBTCMN100.CheckExternalCharacter(in_strCompanyCd.Text) Then
                MNBTCMN100.ShowMessage("MSGVWE00003", "企業コード", "", "")
                Return False
            ElseIf in_strCompanyBranchNo.Text = "" OrElse in_strCompanyBranchNo.Text = Nothing Then
                MNBTCMN100.ShowMessage("MSGVWE00001", "枝番", "", "")
                Return False
            ElseIf MNBTCMN100.CheckValidateOnlyNumber(in_strCompanyBranchNo.Text) Then
                MNBTCMN100.ShowMessage("MSGVWE00015", "枝番", "数値", "")
                Return False
            ElseIf MNBTCMN100.CheckExternalCharacter(in_strCompanyBranchNo.Text) Then
                MNBTCMN100.ShowMessage("MSGVWE00013", "枝番", "", "")
                Return False
            ElseIf in_strFilePath.Text = "" OrElse in_strFilePath.Text = Nothing Then
                MNBTCMN100.ShowMessage("MSGVWE00001", "ファイル参照パス", "", "")
                Return False
            ElseIf IO.File.Exists(in_strFilePath.Text) = False Then
                MNBTCMN100.ShowMessage("MSGVWE00018", "ファイル参照パス", "", "")
                Return False
            ElseIf IO.Path.GetExtension(in_strFilePath.Text) <> ".csv" AndAlso IO.Path.GetExtension(in_strFilePath.Text) <> ".CSV" Then
                MNBTCMN100.ShowMessage("MSGVWE00019", "ファイル参照パス", "", "")
                Return False
            End If
            Return True
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
            Return False
        End Try
    End Function

    ''' <summary>
    ''' CheckInputCorrelation
    ''' </summary>
    ''' <param name="in_strCompanyCd"></param>
    ''' <param name="in_strCompanyBranchNo"></param>
    ''' <param name="in_strFilePath"></param>
    ''' <param name="in_intFileType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckInputCorrelation(ByVal in_strCompanyCd As String, ByVal in_strCompanyBranchNo As String, ByVal in_strFilePath As String, ByVal in_intFileType As Integer) As Boolean
        Try
            p_mynoContext = New mynoEntities()
            'Get data from table t_company
            Dim existQuery = (From tc In p_mynoContext.t_company
                              Where tc.companycd = in_strCompanyCd AndAlso
                              tc.companybranchno = in_strCompanyBranchNo AndAlso
                              tc.delflg = 0
                              Select tc.companybranchname, tc.companyname).ToList()
            'Check exist data
            If existQuery.Count < 1 Then
                MNBTCMN100.ShowMessage("MSGVWE00018", "企業情報", "", "")
                Return False
            Else
                p_strCompanyBranchName = existQuery(0).companybranchname
                p_strCompanyName = existQuery(0).companyname
            End If
            'Get data in table t_private
            Dim statusQuery = (From tp In p_mynoContext.t_private
                          Where tp.companycd = in_strCompanyCd AndAlso
                          tp.companybranchno = in_strCompanyBranchNo AndAlso
                          tp.delflg = 0 AndAlso tp.sts >= 400
                            Select tp).ToList()
            'Check status
            If statusQuery IsNot Nothing AndAlso statusQuery.Count > 0 Then
                MNBTCMN100.ShowMessage("MSGVWE00025", "正常原票登録済み以降", "基礎データの取込", "")
                Return False
            End If
            'Check content csv
            '基礎データの項目数を超える場合
            Using myReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(in_strFilePath)
                'Specify that reading from a comma-delimited file'
                myReader.TextFieldType = FileIO.FieldType.Delimited
                myReader.SetDelimiters(",")
                Dim currentRow As String()
                If Not myReader.EndOfData Then
                    currentRow = myReader.ReadFields()
                    '1:個人 2:家族
                    If (in_intFileType = 1 AndAlso currentRow.Count() <> 24) OrElse
                        (in_intFileType = 2 AndAlso currentRow.Count() <> 8) Then
                        MNBTCMN100.ShowMessage("MSGVWE00020", "", "", "")
                        Return False
                    End If

                End If
            End Using
            Return True
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Process data from csv file and import to DB
    ''' </summary>
    ''' <param name="in_strCompanyCd"></param>
    ''' <param name="in_strBranchCd"></param>
    ''' <param name="in_strPathFile"></param>
    ''' <param name="in_intFileType"></param>
    ''' <param name="in_intImportType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessDataBasic(ByVal in_strCompanyCd As String, ByVal in_strBranchCd As String, ByVal in_strPathFile As String,
                                     ByVal in_intFileType As Integer, ByVal in_intImportType As Integer,
                                    ByRef p_errFlg As Integer) As Boolean
        Try
            Dim result As Boolean = False
            p_mynoContext = New mynoEntities()
            Using transScope As DbContextTransaction = p_mynoContext.Database.BeginTransaction(IsolationLevel.Chaos)
                'Call function setting first log system and insert log import, insert content csv to DB
                Dim systemSeq As Integer
                'Call function setting first log system
                systemSeq = MNBTCMN100.InputLogMaster(p_mynoContext, "4", "MNUIDTR100", in_strCompanyCd, CInt(in_strBranchCd))

                'import data to t_importlog
                ImportLog(p_mynoContext, systemSeq, in_strCompanyCd, in_strBranchCd, in_strPathFile, in_intFileType, in_intImportType)

                'register content file csv to DB
                ImportFromCsv(p_mynoContext, systemSeq, in_strCompanyCd, in_strBranchCd, in_strPathFile, in_intFileType)

                'Select data in table import from DB and check error
                Dim queryListwPrivate As List(Of w_private) = New List(Of w_private)()
                Dim queryListwFamily As List(Of w_family) = New List(Of w_family)()
                Dim errorType As Integer = 0
                Dim resultCheckErr As Boolean = GetDataImportAnndCheckErr(queryListwPrivate, queryListwFamily, systemSeq,
                                                                          in_intFileType, in_intImportType, in_strPathFile,
                                                                          in_strCompanyCd, in_strBranchCd, p_mynoContext, errorType)

                If resultCheckErr = True Then
                    If errorType = 9 Then
                        p_errFlg = 1
                    End If

                    If ProcessDataImport(in_strPathFile, systemSeq, in_strCompanyCd, in_strBranchCd,
                                      in_intFileType, in_intImportType, queryListwPrivate, queryListwFamily, p_mynoContext, p_errFlg) Then
                        result = True
                    Else
                        result = False
                    End If
                End If
                transScope.Commit()
                If errorType = 1 Then
                    MNBTCMN100.ShowMessage("MSGVWE00018", "取込対象データ", "", "")
                ElseIf errorType = 2 Then
                    MNBTCMN100.ShowMessage("MSGVWE00021", "取込対象データ", "", "")
                End If
            End Using
            Return result
        Catch ex As Exception
            'c_frmMNUIDTR100.BeginInvoke(New Action(Sub()
            '                                           frmLoading.Hide()
            '                                           c_frmMNUIDTR100.Activate()
            '                                       End Sub))
            frmLoading.Hide()
            c_frmMNUIDTR100.Activate()
            MNBTCMN100.ShowMessageException()
            Return False
        End Try
    End Function
    ''' <summary>
    ''' ImportLog - 6.1.1.4.b
    ''' </summary>
    ''' <param name="inout_context"></param>
    ''' <param name="in_strCompanyCd"></param>
    ''' <param name="in_strCompanyBranchNo"></param>
    ''' <param name="in_strFilePath"></param>
    ''' <param name="in_intFileType"></param>
    ''' <param name="in_intImportType"></param>
    ''' <remarks></remarks>
    Public Sub ImportLog(ByRef inout_context As mynoEntities, ByVal in_intSystemSeq As Integer, ByVal in_strCompanyCd As String, ByVal in_strCompanyBranchNo As String,
                              ByVal in_strFilePath As String, ByVal in_intFileType As Integer, ByVal in_intImportType As Integer)
        'Get datetime now
        Dim dateNow = MNBTCMN100.GetCurrentTimestamp(inout_context)

        Dim item As New t_importlog()
        item.seq = in_intSystemSeq
        item.companycd = in_strCompanyCd
        item.companyname = p_strCompanyName
        item.companybranchno = CInt(in_strCompanyBranchNo)
        item.companybranchname = p_strCompanyBranchName
        item.impdatetime = dateNow
        item.impusercd = p_strUserCdLogin
        item.adddatetime = dateNow
        item.addjusercd = p_strUserCdLogin
        item.terminalcd = p_strTerminalCdLogin
        item.filename = IO.Path.GetFileName(in_strFilePath)
        'read binary file

        Dim fInfo As New FileInfo(in_strFilePath)
        Dim numBytes As Long = fInfo.Length
        Dim fStream As New FileStream(in_strFilePath, FileMode.Open, FileAccess.Read)
        Dim br As New BinaryReader(fStream)
        Dim data As Byte() = br.ReadBytes(CInt(numBytes))
        br.Close()
        fStream.Close()
        item.csvfile = data
        item.imptype = in_intImportType
        item.filetype = in_intFileType
        inout_context.t_importlog.Add(item)

        inout_context.SaveChanges()
    End Sub
    ''' <summary>
    ''' ImportFromCsv - 6.1.1.5
    ''' </summary>
    ''' <param name="inout_context"></param>
    ''' <param name="in_intImportSeq">seq auto zen in table t_importlog</param>
    ''' <param name="in_strCompanyCd"></param>
    ''' <param name="in_strCompanyBranchNo"></param>
    ''' <param name="in_strFilePath"></param>
    ''' <param name="in_intFileType"></param>
    ''' <remarks></remarks>
    Public Sub ImportFromCsv(ByRef inout_context As mynoEntities, ByVal in_intImportSeq As Integer,
                             ByVal in_strCompanyCd As String, ByVal in_strCompanyBranchNo As String,
                             ByVal in_strFilePath As String, ByVal in_intFileType As Integer)

        Dim common As MNBTCMN100 = New MNBTCMN100

        common.setProgressMax(0, "CSVデータ登録(1/4)")

        'radio「個人」is checked
        If in_intFileType = 1 Then
            Dim fieldData() As String
            Dim countInt As Integer = 1
            Dim strSql As String
            Dim bindData As Hashtable
            Dim bindParams() As Npgsql.NpgsqlParameter

            strSql = "INSERT INTO W_PRIVATE "
            strSql = strSql & " VALUES ("
            strSql = strSql & " CAST(:seq AS BIGINT)"
            strSql = strSql & ",CAST(:line AS BIGINT)"
            strSql = strSql & ",:companycd"
            strSql = strSql & ",:companybranchno"
            strSql = strSql & ",:staffcd"
            strSql = strSql & ",:familyname"
            strSql = strSql & ",:familynamekana"
            strSql = strSql & ",:birthdate"
            strSql = strSql & ",:sex"
            strSql = strSql & ",:postno"
            strSql = strSql & ",:prefecturescd"
            strSql = strSql & ",:address1"
            strSql = strSql & ",:address2"
            strSql = strSql & ",:tel"
            strSql = strSql & ",:fax"
            strSql = strSql & ",:sendpostno"
            strSql = strSql & ",:sendprefecturescd"
            strSql = strSql & ",:sendaddress1"
            strSql = strSql & ",:sendaddress2"
            strSql = strSql & ",:sendtel"
            strSql = strSql & ",:sendfax"
            strSql = strSql & ",:mail"
            strSql = strSql & ",:belongcd1"
            strSql = strSql & ",:belongname1"
            strSql = strSql & ",:belongname2"
            strSql = strSql & ",:vipflg"
            strSql = strSql & ")"

            Dim wholeFile As String
            Dim lineData() As String
            wholeFile = My.Computer.FileSystem.ReadAllText(in_strFilePath, System.Text.Encoding.GetEncoding("Shift_JIS"))
            If wholeFile <> "" Then
                lineData = Split(wholeFile, vbNewLine)
                'Dim item As w_private
                'Dim listItem As List(Of w_private) = New List(Of w_private)()
                For Each lineOfText As String In lineData

                    common.setProgress(countInt)

                    Application.DoEvents()
                    fieldData = lineOfText.Split(",")
                    If fieldData.Count() = 1 And fieldData.GetValue(0) = "" Then
                        Console.Write("New line")
                    Else
                        If fieldData.Count() <= 24 Then

                            ReDim Preserve fieldData(23)
                            bindData = New Hashtable
                            bindData("seq") = in_intImportSeq
                            bindData("line") = countInt
                            If fieldData.GetValue(0) = "" Then
                                bindData("companycd") = Nothing
                            Else
                                bindData("companycd") = fieldData.GetValue(0)
                            End If
                            If fieldData.GetValue(1) = "" Then
                                bindData("companybranchno") = Nothing
                            Else
                                bindData("companybranchno") = fieldData.GetValue(1)
                            End If
                            If fieldData.GetValue(2) = "" Then
                                bindData("staffcd") = Nothing
                            Else
                                bindData("staffcd") = fieldData.GetValue(2)
                            End If
                            If fieldData.GetValue(3) = "" Then
                                bindData("familyname") = Nothing
                            Else
                                bindData("familyname") = fieldData.GetValue(3)
                            End If
                            If fieldData.GetValue(4) = "" Then
                                bindData("familynamekana") = Nothing
                            Else
                                bindData("familynamekana") = fieldData.GetValue(4)
                            End If
                            If fieldData.GetValue(5) = "" Then
                                bindData("birthdate") = Nothing
                            Else
                                bindData("birthdate") = fieldData.GetValue(5)
                            End If
                            If fieldData.GetValue(6) = "" Then
                                bindData("sex") = Nothing
                            Else
                                bindData("sex") = fieldData.GetValue(6)
                            End If
                            If fieldData.GetValue(7) = "" Then
                                bindData("postno") = Nothing
                            Else
                                bindData("postno") = fieldData.GetValue(7)
                            End If
                            If fieldData.GetValue(8) = "" Then
                                bindData("prefecturescd") = Nothing
                            Else
                                bindData("prefecturescd") = fieldData.GetValue(8)
                            End If
                            If fieldData.GetValue(9) = "" Then
                                bindData("address1") = Nothing
                            Else
                                bindData("address1") = fieldData.GetValue(9)
                            End If
                            If fieldData.GetValue(10) = "" Then
                                bindData("address2") = Nothing
                            Else
                                bindData("address2") = fieldData.GetValue(10)
                            End If
                            If fieldData.GetValue(11) = "" Then
                                bindData("tel") = Nothing
                            Else
                                bindData("tel") = fieldData.GetValue(11)
                            End If
                            If fieldData.GetValue(12) = "" Then
                                bindData("fax") = Nothing
                            Else
                                bindData("fax") = fieldData.GetValue(12)
                            End If
                            If fieldData.GetValue(13) = "" Then
                                bindData("sendpostno") = Nothing
                            Else
                                bindData("sendpostno") = fieldData.GetValue(13)
                            End If
                            If fieldData.GetValue(14) = "" Then
                                bindData("sendprefecturescd") = Nothing
                            Else
                                bindData("sendprefecturescd") = fieldData.GetValue(14)
                            End If
                            If fieldData.GetValue(15) = "" Then
                                bindData("sendaddress1") = Nothing
                            Else
                                bindData("sendaddress1") = fieldData.GetValue(15)
                            End If
                            If fieldData.GetValue(16) = "" Then
                                bindData("sendaddress2") = Nothing
                            Else
                                bindData("sendaddress2") = fieldData.GetValue(16)
                            End If
                            If fieldData.GetValue(17) = "" Then
                                bindData("sendtel") = Nothing
                            Else
                                bindData("sendtel") = fieldData.GetValue(17)
                            End If
                            If fieldData.GetValue(18) = "" Then
                                bindData("sendfax") = Nothing
                            Else
                                bindData("sendfax") = fieldData.GetValue(18)
                            End If
                            If fieldData.GetValue(19) = "" Then
                                bindData("mail") = Nothing
                            Else
                                bindData("mail") = fieldData.GetValue(19)
                            End If
                            If fieldData.GetValue(20) = "" Then
                                bindData("belongcd1") = Nothing
                            Else
                                bindData("belongcd1") = fieldData.GetValue(20)
                            End If
                            If fieldData.GetValue(21) = "" Then
                                bindData("belongname1") = Nothing
                            Else
                                bindData("belongname1") = fieldData.GetValue(21)
                            End If
                            If fieldData.GetValue(22) = "" Then
                                bindData("belongname2") = Nothing
                            Else
                                bindData("belongname2") = fieldData.GetValue(22)
                            End If
                            If fieldData.GetValue(23) = "" Then
                                bindData("vipflg") = Nothing
                            Else
                                bindData("vipflg") = fieldData.GetValue(23)
                            End If

                            bindParams = setParameters(bindData)

                            inout_context.Database.ExecuteSqlCommand(strSql, bindParams)
                            countInt += 1

                        End If
                    End If
                Next lineOfText
                'inout_context.w_private.AddRange(listItem)
                common.setProgress(countInt - 1, 1)
            End If
        End If

        'radio「家族」is checked
        If in_intFileType = 2 Then

            Dim strSql As String
            Dim bindData As Hashtable
            Dim bindParams() As Npgsql.NpgsqlParameter

            strSql = "INSERT INTO W_FAMILY "
            strSql = strSql & " VALUES ("
            strSql = strSql & " CAST(:seq AS BIGINT)"
            strSql = strSql & ",CAST(:line AS BIGINT)"
            strSql = strSql & ",:companycd"
            strSql = strSql & ",:companybranchno"
            strSql = strSql & ",:staffcd"
            strSql = strSql & ",:familyname"
            strSql = strSql & ",:familynamekana"
            strSql = strSql & ",:birthdate"
            strSql = strSql & ",:sex"
            strSql = strSql & ",:insuredflg"
            strSql = strSql & ")"

            Using MyReader As New Microsoft.VisualBasic.
                          FileIO.TextFieldParser(in_strFilePath, System.Text.Encoding.GetEncoding("Shift_JIS"))
                MyReader.TextFieldType = FileIO.FieldType.Delimited
                MyReader.SetDelimiters(",")
                Dim fieldData() As String
                Dim count As Integer = 1
                While Not MyReader.EndOfData

                    common.setProgress(count)

                    Application.DoEvents()
                    fieldData = MyReader.ReadFields()
                    If fieldData.Count() = 1 And fieldData.GetValue(0) = "" Then
                        Console.Write("New line")
                    Else
                        If (fieldData.Count() <= 8) Then

                            ReDim Preserve fieldData(8)
                            bindData = New Hashtable
                            bindData("seq") = in_intImportSeq
                            bindData("line") = count
                            If fieldData.GetValue(0) = "" Then
                                bindData("companycd") = Nothing
                            Else
                                bindData("companycd") = fieldData.GetValue(0)
                            End If
                            If fieldData.GetValue(1) = "" Then
                                bindData("companybranchno") = Nothing
                            Else
                                bindData("companybranchno") = fieldData.GetValue(1)
                            End If
                            If fieldData.GetValue(2) = "" Then
                                bindData("staffcd") = Nothing
                            Else
                                bindData("staffcd") = fieldData.GetValue(2)
                            End If
                            If fieldData.GetValue(3) = "" Then
                                bindData("familyname") = Nothing
                            Else
                                bindData("familyname") = fieldData.GetValue(3)
                            End If
                            If fieldData.GetValue(4) = "" Then
                                bindData("familynamekana") = Nothing
                            Else
                                bindData("familynamekana") = fieldData.GetValue(4)
                            End If
                            If fieldData.GetValue(5) = "" Then
                                bindData("birthdate") = Nothing
                            Else
                                bindData("birthdate") = fieldData.GetValue(5)
                            End If
                            If fieldData.GetValue(6) = "" Then
                                bindData("sex") = Nothing
                            Else
                                bindData("sex") = fieldData.GetValue(6)
                            End If
                            If fieldData.GetValue(7) = "" Then
                                bindData("insuredflg") = Nothing
                            Else
                                bindData("insuredflg") = fieldData.GetValue(7)
                            End If

                            bindParams = setParameters(bindData)

                            inout_context.Database.ExecuteSqlCommand(strSql, bindParams)
                            count += 1

                        End If
                    End If
                End While
                MyReader.Close()

                common.setProgress(count - 1, 1)

            End Using
        End If
        inout_context.SaveChanges()
    End Sub
    ''' <summary>
    ''' Get data import And Check Err - 6.1.1.6, 6.1.1.7
    ''' </summary>
    ''' <param name="inout_lstwPrivate"></param>
    ''' <param name="inout_lstwFamily"></param>
    ''' <param name="in_intFileType"></param>
    ''' <param name="in_intImportType"></param>
    ''' <param name="in_strFilePath"></param>
    ''' <param name="in_strCompCode"></param>
    ''' <param name="in_strBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataImportAnndCheckErr(ByRef inout_lstwPrivate As List(Of w_private), ByRef inout_lstwFamily As List(Of w_family),
                                              ByVal in_intSeq As Integer, ByVal in_intFileType As Integer, ByVal in_intImportType As Integer,
                                              ByVal in_strFilePath As String, ByVal in_strCompCode As String, ByVal in_strBranchCode As String, ByRef mynoContext As mynoEntities, ByRef errorType As Integer) As Boolean
        Dim checkReuslt As Boolean = True
        'radio「個人」is checked
        If in_intFileType = 1 Then
            inout_lstwPrivate = (From wp In mynoContext.w_private
                            Where wp.seq = in_intSeq
                            Order By wp.line Ascending
                            Select wp).ToList()
            'radio「家族」is checked
        ElseIf in_intFileType = 2 Then
            inout_lstwFamily = (From wf In mynoContext.w_family
                            Where wf.seq = in_intSeq
                            Order By wf.line Ascending
                            Select wf).ToList()
        End If
        'Case list w_private = 0 Or w_family = 0
        If (in_intFileType = 1 And inout_lstwPrivate.Count = 0) Or (in_intFileType = 2 And inout_lstwFamily.Count = 0) Then
            'insert log detail for table t_systemdetaillog
            MNBTCMN100.InputLogDetail(mynoContext, in_intSeq, "", "取込エラー", in_strFilePath)
            'Get content message by msgkey = MSGVWE00018
            Dim message As String = MNBTCMN100.GetMessageContent("MSGVWE00018", "取込対象データ", "", "")
            'insert data to table t_importdetaillog
            ImportDetailLog(mynoContext, in_intSeq, 1, 0, message, Nothing, Nothing)
            mynoContext.SaveChanges()
            'c_frmMNUIDTR100.BeginInvoke(New Action(Sub()
            '                                           frmLoading.Hide()
            '                                           c_frmMNUIDTR100.Activate()
            '                                       End Sub))
            frmLoading.Hide()
            c_frmMNUIDTR100.Activate()
            'MNBTCMN100.ShowMessage("MSGVWE00018", "取込対象データ", "", "")
            errorType = 1
            checkReuslt = False
        Else

            'Check error for data in file csv
            If in_intFileType = 1 Then
                Dim errResults As Integer = 0
                mynoContext = CheckDataCsvFilePrivateOn(mynoContext, in_intSeq, inout_lstwPrivate, in_strCompCode, in_strBranchCode, in_intImportType, errResults)
                'the csv file in list w_private have error 
                If errResults > 0 Then
                    'insert log to table t_systemdetaillog
                    MNBTCMN100.InputLogDetail(mynoContext, in_intSeq, "", "取込エラー", in_strFilePath)
                    'Update errflg = 2 in table t_importlog
                    Dim updateQuery = (From im In mynoContext.t_importlog
                                       Where im.seq = in_intSeq
                                       Select im).SingleOrDefault()
                    updateQuery.errflg = 2
                    'Delete data in table w_private
                    Dim delPrivate = (From wp In mynoContext.w_private Where wp.seq = in_intSeq Select wp).ToList()
                    mynoContext.w_private.RemoveRange(delPrivate)

                    frmLoading.Hide()
                    c_frmMNUIDTR100.Activate()
                    'MNBTCMN100.ShowMessage("MSGVWE00021", "取込対象データ", "", "")
                    'c_frmMNUIDTR100.BeginInvoke(New Action(Sub()
                    '                                           frmLoading.Hide()
                    '                                           c_frmMNUIDTR100.Activate()
                    '                                       End Sub))
                    errorType = 2
                    checkReuslt = False
                End If
            ElseIf in_intFileType = 2 Then
                Dim errResults As Integer = 0
                Dim wrnResults As Integer = 0
                mynoContext = CheckDataCsvFileFamilyOn(mynoContext, in_intSeq, inout_lstwFamily, in_strCompCode, in_strBranchCode, in_intImportType, errResults, wrnResults)
                'the csv file in list w_family have error 
                If errResults > 0 Then
                    'insert log to table t_systemdetaillog
                    MNBTCMN100.InputLogDetail(mynoContext, in_intSeq, "", "取込エラー", in_strFilePath)
                    'Update errflg = 2 in table t_importlog
                    Dim updateQuery = (From im In mynoContext.t_importlog
                                       Where im.seq = in_intSeq
                                       Select im).SingleOrDefault()
                    updateQuery.errflg = 2
                    'Delete data in table w_family
                    Dim delFamily = (From wf In mynoContext.w_family Where wf.seq = in_intSeq Select wf).ToList()
                    mynoContext.w_family.RemoveRange(delFamily)

                    frmLoading.Hide()
                    c_frmMNUIDTR100.Activate()
                    'MNBTCMN100.ShowMessage("MSGVWE00021", "取込対象データ", "", "")
                    'c_frmMNUIDTR100.BeginInvoke(New Action(Sub()
                    '                                           frmLoading.Hide()
                    '                                           c_frmMNUIDTR100.Activate()
                    '                                       End Sub))
                    errorType = 2
                    checkReuslt = False
                Else
                    If wrnResults > 0 Then
                        errorType = 9
                    End If
                End If
            End If
            'mynoContext.SaveChanges()
        End If
        Return checkReuslt
    End Function
    ''' <summary>
    ''' Check Data Csv File in the case Family On
    ''' </summary>
    ''' <param name="in_context"></param>
    ''' <param name="in_intSeq"></param>
    ''' <param name="in_lstQuerywFamily"></param>
    ''' <param name="in_strCompCode"></param>
    ''' <param name="in_strBranchCode"></param>
    ''' <param name="in_intImportType"></param>
    ''' <param name="inout_intErrCount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckDataCsvFileFamilyOn(ByVal in_context As mynoEntities, ByVal in_intSeq As Integer, ByVal in_lstQuerywFamily As List(Of w_family),
                                              ByVal in_strCompCode As String, ByVal in_strBranchCode As String,
                                              ByVal in_intImportType As Integer, ByRef inout_intErrCount As Integer,
                                              ByRef inout_intWrnCount As Integer) As mynoEntities

        Dim message As String
        Dim wFamily As w_family
        Dim common As MNBTCMN100 = New MNBTCMN100

        If in_lstQuerywFamily.Count > 0 Then
            Dim errComCdFlag As Boolean = False
            Dim errBranchCdFlag As Boolean = False
            Dim errStaffCdFlag As Boolean = False
            Dim errNamekanaFlag As Boolean = False
            Dim errSexFlag As Boolean = False
            Dim errBrithDayFlag As Boolean = False
            'Dim queryWfamily = (From wf In in_context.w_family Select wf Where wf.seq = in_intSeq).ToList()
            'Dim queryTfamily = (From wf In in_context.t_family Select wf Where wf.companycd = in_strCompCode And wf.companybranchno = in_strBranchCode).ToList()
            'Dim queryTprivate = (From tp In in_context.t_private Select tp Where tp.companycd = in_strCompCode And tp.companybranchno = in_strBranchCode).ToList()
            'Dim lstErrorCheckFamily As List(Of LstDetailLog) = New List(Of LstDetailLog)()

            common.setProgressMax(in_lstQuerywFamily.Count, "CSVデータチェック(2/4)")

            For i As Integer = 0 To in_lstQuerywFamily.Count - 1

                common.setProgress(i + 1)

                Application.DoEvents()
                wFamily = in_lstQuerywFamily(i)
                Dim csvData = MNBTCMN100.CreateCsvDataImport(wFamily)
                If wFamily.companycd = "" Or wFamily.companycd Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "企業コード", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, Nothing)
                    errComCdFlag = True
                End If
                If wFamily.companybranchno = "" Or wFamily.companybranchno Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "枝番", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, Nothing)
                    errBranchCdFlag = True
                End If
                If wFamily.staffcd = "" Or wFamily.staffcd Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "社員コード", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, Nothing)
                    errStaffCdFlag = True
                Else
                    'Check alphanumeric wFamily.staffcd
                    If MNBTCMN100.CheckValidCompanyCdOrStaffCd(wFamily.staffcd) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "社員コード", "半角英数", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.staffcd)
                        errStaffCdFlag = True
                    End If
                    If MNBTCMN100.checkLength(wFamily.staffcd, 10) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "社員コード", "10バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.staffcd)
                        errStaffCdFlag = True
                    End If
                End If
                If wFamily.familyname = "" Or wFamily.familyname Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "漢字氏名", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, Nothing)
                Else
                    If MNBTCMN100.CheckExternalCharacter(wFamily.familyname) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00003", "漢字氏名", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.familyname)
                    End If
                    If MNBTCMN100.checkLength(wFamily.familyname, 30) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "漢字氏名", "30バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.familyname)
                    End If
                End If
                If wFamily.familynamekana = "" Or wFamily.familynamekana Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "カナ氏名", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, Nothing)
                    errNamekanaFlag = True
                Else
                    If MNBTCMN100.CheckExternalCharacter(wFamily.familynamekana) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00003", "カナ氏名", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.familynamekana)
                        errNamekanaFlag = True
                    End If
                    'Check kana half size wFamily.familynamekana
                    If MNBTCMN100.CheckValidateOnlyKatakanaHalfsize(wFamily.familynamekana) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "カナ氏名", "半角カナ", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.familynamekana)
                    End If
                    If MNBTCMN100.checkLength(wFamily.familynamekana, 30) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "カナ氏名", "30バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.familynamekana)
                        errNamekanaFlag = True
                    End If
                End If
                If wFamily.birthdate = "" Or wFamily.birthdate Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "生年月日", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, Nothing)
                    errBrithDayFlag = True
                Else
                    If MNBTCMN100.CheckValidateDateMulType(wFamily.birthdate) <> 0 Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00013", "生年月日", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.birthdate)
                        errBrithDayFlag = True
                    End If
                End If
                'If wFamily.sex = "" Or wFamily.sex Is Nothing Then
                '    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "性別", "", "")
                '    inout_intErrCount = inout_intErrCount + 1
                '    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.sex)
                '    errSexFlag = True
                'Else
                If wFamily.sex <> "" And wFamily.sex IsNot Nothing Then
                    If wFamily.sex <> "1" And wFamily.sex <> "2" Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "性別", "1または2", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.sex)
                        errSexFlag = True
                    End If
                End If
                'End If
                If wFamily.insuredflg = "" Or wFamily.insuredflg Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "第３号被保険者フラグ", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, Nothing)
                Else
                    If wFamily.insuredflg <> "0" And wFamily.insuredflg <> "1" Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "第３号被保険者フラグ", "0または1", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.insuredflg)
                    End If
                End If
                If (wFamily.companycd <> "" And wFamily.companycd IsNot Nothing) And (wFamily.companybranchno <> "" And wFamily.companybranchno IsNot Nothing) Then
                    Dim pad As Char
                    pad = "0"c
                    in_strCompCode = in_strCompCode.PadLeft(3, pad)
                    in_strBranchCode = in_strBranchCode.PadLeft(3, pad)
                    If Not wFamily.companycd.PadLeft(3, pad).Equals(in_strCompCode) Or Not wFamily.companybranchno.PadLeft(3, pad).Equals(in_strBranchCode) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00023", "企業コード、枝番", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.companycd & "、" & wFamily.companybranchno)
                    End If
                End If
                'If errComCdFlag = False Or errBranchCdFlag = False Or errStaffCdFlag = False Or errNamekanaFlag = False Or errSexFlag = False Or errBrithDayFlag = False Then

                '    Dim query = (From wf In in_context.w_family
                '                Where wf.seq = in_intSeq And wf.companycd = wFamily.companycd And wf.companybranchno = wFamily.companybranchno And wf.staffcd = wFamily.staffcd And wf.familynamekana = wFamily.familynamekana And wf.sex = wFamily.sex And wf.birthdate = wFamily.birthdate
                '                Select wf).ToList()
                '    If query.Count >= 2 Then
                '        Dim lstLine As StringBuilder = New StringBuilder()
                '        For j As Integer = 0 To query.Count - 1
                '            lstLine.Append(query(j).line)
                '            If j < query.Count - 1 Then
                '                lstLine.Append(",")
                '            End If
                '        Next
                '        Dim param As String = lstLine.ToString() + "行目と家族情報"
                '        message = MNBTCMN100.GetMessageContent("MSGVWE00024", param, "", "")
                '        inout_intErrCount = inout_intErrCount + 1
                '        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.companycd & "、" & wFamily.companybranchno & "、" & wFamily.staffcd & "、" & wFamily.familynamekana & "、" & wFamily.sex & "、" & wFamily.birthdate)
                '    End If
                'End If
                If in_intImportType = 2 Then
                    Dim query = (From wf In in_context.t_family
                                 Where wf.companycd = wFamily.companycd And wf.companybranchno = wFamily.companybranchno And wf.staffcd = wFamily.staffcd And wf.familynamekana = wFamily.familynamekana And wf.delflg = 0 And If(wFamily.sex <> "", wf.sex = wFamily.sex, 1 = 1) And wf.birthdate = wFamily.birthdate
                                 Select wf).ToList()
                    If query.Count >= 1 Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00024", "家族情報", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        inout_intWrnCount += 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.companycd & "、" & wFamily.companybranchno & "、" & wFamily.staffcd & "、" & wFamily.familynamekana & "、" & wFamily.sex & "、" & wFamily.birthdate)
                    End If
                End If
                If wFamily.companybranchno <> "" And wFamily.companybranchno IsNot Nothing Then
                    If MNBTCMN100.CheckValidateOnlyNumber(wFamily.companybranchno) = False Then
                        Dim querySql = (From tp In in_context.t_private
                                        Where tp.companycd = wFamily.companycd And tp.companybranchno = CInt(wFamily.companybranchno) And tp.staffcd = wFamily.staffcd And tp.delflg = 0).ToList()
                        If querySql.Count = 0 Then
                            message = MNBTCMN100.GetMessageContent("MSGVWE00018", "家族情報に紐づく社員", "", "")
                            inout_intErrCount = inout_intErrCount + 1
                            ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wFamily.line, message, csvData, wFamily.companycd & "、" & wFamily.companybranchno & "、" & wFamily.staffcd)
                        End If
                    End If
                End If
            Next

            common.setProgress(in_lstQuerywFamily.Count, 1)

            inout_intErrCount -= inout_intWrnCount

        End If
        Return in_context
    End Function

    'Private Function AddError(ByVal in_intdetailSeq As Integer, ByVal in_intline As Integer, ByVal in_strMessage As String,
    '                          ByVal in_strCsvData As String, ByVal in_strColData As String) As LstDetailLog
    '    Try
    '        Dim logError As LstDetailLog = New LstDetailLog()
    '        logError.tempDetailSeq = in_intdetailSeq
    '        logError.tempLine = in_intline
    '        logError.tempMsg = in_strMessage
    '        logError.tempCsvData = in_strCsvData
    '        logError.tempColData = in_strColData

    '        Return logError
    '    Catch ex As Exception
    '        MNBTCMN100.ShowMessageException()
    '        Return Nothing
    '    End Try
    'End Function

    ''' <summary>
    ''' Check data csv file in the case private on
    ''' </summary>
    ''' <param name="in_context"></param>
    ''' <param name="in_intSeq"></param>
    ''' <param name="in_lstQuerywPrivate"></param>
    ''' <param name="in_strCompCode"></param>
    ''' <param name="in_strBranchCode"></param>
    ''' <param name="in_intImportType"></param>
    ''' <param name="inout_intErrCount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckDataCsvFilePrivateOn(ByVal in_context As mynoEntities, ByVal in_intSeq As Integer, ByVal in_lstQuerywPrivate As List(Of w_private), ByVal in_strCompCode As String,
                                               ByVal in_strBranchCode As String, ByVal in_intImportType As Integer, ByRef inout_intErrCount As Integer) As mynoEntities

        Dim wPrivate As w_private
        Dim message As String

        Dim common As MNBTCMN100 = New MNBTCMN100

        If in_lstQuerywPrivate.Count > 0 Then
            Dim errComCdFlag As Boolean = False
            Dim errbranchFlag As Boolean = False
            Dim errStaffCdFlag As Boolean = False
            Dim lstPefrecCd = (From mp In in_context.m_prefectures Select mp.prefecturescd).ToList()
            Dim stime = DateTime.Now
            Dim lstTprivate = (From p In in_context.t_private Select p Where p.companycd = in_strCompCode And p.companybranchno = in_strBranchCode).ToList()
            Dim etime = DateTime.Now
            Debug.Print("get t_private: " + (stime - etime).ToString())
            Dim stime2 = DateTime.Now
            'Dim lstWprivate = (From wp In in_context.w_private Select wp Where wp.seq = in_intSeq).ToList()
            Dim etime2 = DateTime.Now
            Debug.Print("get w_private: " + (stime - etime).ToString())
            'Dim lstErrorCheck As List(Of LstDetailLog) = New List(Of LstDetailLog)()

            common.setProgressMax(in_lstQuerywPrivate.Count, "CSVデータチェック(2/4)")

            For i As Integer = 0 To in_lstQuerywPrivate.Count - 1

                common.setProgress(i + 1)

                Application.DoEvents()
                wPrivate = in_lstQuerywPrivate(i)
                Dim csvData = MNBTCMN100.CreateCsvDataImport(wPrivate)
                'Check nothing w_private.companycd
                If wPrivate.companycd = "" Or wPrivate.companycd Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "企業コード", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, Nothing)
                    errComCdFlag = True
                End If
                'Check nothing wPrivate.companybranchno
                If wPrivate.companybranchno = "" Or wPrivate.companybranchno Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "枝番", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, Nothing)
                    errbranchFlag = True
                End If
                'Check nothing wPrivate.staffcd
                If wPrivate.staffcd = "" Or wPrivate.staffcd Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "社員コード", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, Nothing)
                    errStaffCdFlag = True
                Else
                    'Check alphanumeric wPrivate.staffcd
                    If MNBTCMN100.CheckValidCompanyCdOrStaffCd(wPrivate.staffcd) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "社員コード", "半角英数", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.staffcd)
                        errStaffCdFlag = True
                    End If
                    If MNBTCMN100.checkLength(wPrivate.staffcd, 10) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "社員コード", "10バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.staffcd)
                        errStaffCdFlag = True
                    End If
                End If
                'Check nothing wPrivate.familyname
                If wPrivate.familyname = "" Or wPrivate.familyname Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "漢字氏名", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, Nothing)
                Else
                    'Check External Character wPrivate.familyname漢字氏名
                    If MNBTCMN100.CheckExternalCharacter(wPrivate.familyname) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00003", "漢字氏名", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.familyname)
                    End If
                    If MNBTCMN100.checkLength(wPrivate.familyname, 30) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "漢字氏名", "30バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.familyname)
                    End If
                End If
                'Check nothing wPrivate.familynamekana
                If wPrivate.familynamekana = "" Or wPrivate.familynamekana Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "カナ氏名", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, Nothing)
                Else
                    'Check External Character wPrivate.familynamekana
                    If MNBTCMN100.CheckExternalCharacter(wPrivate.familynamekana) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00003", "カナ氏名", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.familynamekana)
                    End If
                    'Check kana half size wPrivate.familynamekana
                    If MNBTCMN100.CheckValidateOnlyKatakanaHalfsize(wPrivate.familynamekana) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "カナ氏名", "半角カナ", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.familynamekana)
                    End If
                    If MNBTCMN100.checkLength(wPrivate.familynamekana, 30) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "カナ氏名", "30バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.familynamekana)
                    End If
                End If
                'Check nothing wPrivate.birthdate
                If wPrivate.birthdate = "" Or wPrivate.birthdate Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "生年月日", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, Nothing)
                Else
                    'Check format birthdate
                    If MNBTCMN100.CheckValidateDateMulType(wPrivate.birthdate) <> 0 Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00013", "生年月日", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.birthdate)
                    End If
                End If
                'Check nothing wPrivate.sex
                'If wPrivate.sex = "" Or wPrivate.sex Is Nothing Then
                '    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "性別", "", "")
                '    inout_intErrCount = inout_intErrCount + 1
                '    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.sex)
                'Else
                If wPrivate.sex <> "" And wPrivate.sex IsNot Nothing Then
                    If wPrivate.sex <> "1" And wPrivate.sex <> "2" Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "性別", "1または2", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.sex)
                    End If
                End If
                'End If
                'Check nothing wPrivate.postno
                If wPrivate.postno = "" Or wPrivate.postno Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "郵便番号", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, Nothing)
                Else
                    If MNBTCMN100.checkLength(wPrivate.postno, 8) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "郵便番号", "8バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.postno)
                    End If
                    If MNBTCMN100.CountDigitInString(wPrivate.postno) <> 7 Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "郵便番号", "123-4567または1234567形式", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.postno)
                    End If
                End If
                'Check nothing wPrivate.prefecturescd
                If wPrivate.prefecturescd = "" Or wPrivate.prefecturescd Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "都道府県コード", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, Nothing)
                Else
                    If MNBTCMN100.checkLength(wPrivate.prefecturescd, 2) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "都道府県コード", "2バイト", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.prefecturescd)
                    End If

                    If lstPefrecCd.Contains(wPrivate.prefecturescd) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00018", "都道府県マスタに都道府県コード", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.prefecturescd)
                    End If
                End If
                'Check nothing wPrivate.address1
                If wPrivate.address1 = "" Or wPrivate.address1 Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "住所１", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, Nothing)
                Else
                    'Check External Character wPrivate.address1
                    If MNBTCMN100.CheckExternalCharacter(wPrivate.address1) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00003", "住所１", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.address1)
                    End If
                    If MNBTCMN100.checkLength(wPrivate.address1, 80) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "住所１", "80バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.address1)
                    End If
                End If

                If wPrivate.address2 <> "" And wPrivate.address2 IsNot Nothing Then
                    'Check External Character wPrivate.address2
                    If MNBTCMN100.CheckExternalCharacter(wPrivate.address2) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00003", "住所２", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.address2)
                    End If
                    If MNBTCMN100.checkLength(wPrivate.address2, 80) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "住所２", "80バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.address2)
                    End If
                End If
                'Check nothing wPrivate.sendpostno
                If wPrivate.sendpostno = "" Or wPrivate.sendpostno Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "宛名送付先郵便番号", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, Nothing)
                Else
                    If MNBTCMN100.checkLength(wPrivate.sendpostno, 8) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "宛名送付先郵便番号", "8バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.sendpostno)
                    End If
                    If MNBTCMN100.CountDigitInString(wPrivate.sendpostno) <> 7 Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "宛名送付先郵便番号", "123-4567または1234567形式", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.sendpostno)
                    End If
                End If
                'Check nothing wPrivate.sendprefecturescd
                If wPrivate.sendprefecturescd = "" Or wPrivate.sendprefecturescd Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "宛名送付先都道府県コード", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, Nothing)
                Else
                    If MNBTCMN100.checkLength(wPrivate.sendprefecturescd, 2) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "宛名送付先都道府県コード", "2バイト", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.sendprefecturescd)
                    End If
                    If lstPefrecCd.Contains(wPrivate.sendprefecturescd) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00018", "都道府県マスタに宛名送付先都道府県コード", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.sendprefecturescd)
                    End If
                End If
                'Check nothing wPrivate.sendaddress1
                If wPrivate.sendaddress1 = "" Or wPrivate.sendaddress1 Is Nothing Then
                    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "宛名送付先住所１", "", "")
                    inout_intErrCount = inout_intErrCount + 1
                    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, Nothing)
                Else
                    'Check External Character wPrivate.sendaddress1
                    If MNBTCMN100.CheckExternalCharacter(wPrivate.sendaddress1) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00003", "宛名送付先住所１", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.sendaddress1)
                    End If
                    If MNBTCMN100.checkLength(wPrivate.sendaddress1, 80) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "宛名送付先住所１", "80バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.sendaddress1)
                    End If
                End If
                'Check nothing wPrivate.sendaddress2
                If wPrivate.sendaddress2 <> "" And wPrivate.sendaddress2 IsNot Nothing Then
                    'Check External Character wPrivate.sendaddress2
                    If MNBTCMN100.CheckExternalCharacter(wPrivate.sendaddress2) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00003", "宛名送付先住所２", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, Nothing)
                    End If
                    If MNBTCMN100.checkLength(wPrivate.sendaddress2, 80) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "宛名送付先住所２", "80バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.sendaddress2)
                    End If
                End If
                'Check nothing wPrivate.vipflg
                'If wPrivate.vipflg = "" Or wPrivate.vipflg Is Nothing Then
                '    message = MNBTCMN100.GetMessageContent("MSGVWE00001", "VIPフラグ", "", "")
                '    inout_intErrCount = inout_intErrCount + 1
                '    ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.vipflg)
                'Else
                If wPrivate.vipflg <> "" And wPrivate.vipflg IsNot Nothing Then
                    If wPrivate.vipflg <> "V" And wPrivate.vipflg <> "K" And wPrivate.vipflg <> "A" And wPrivate.vipflg <> " " Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "VIPフラグ", "VまたはKまたはAまたはスペース", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.vipflg)
                    End If
                End If
                'End If
                'Check wPrivate.companycd diff txtCompanycd.Text Or wPrivate.companybranchno diff txtcompanybranchno.Text
                If (wPrivate.companycd <> "" And wPrivate.companycd IsNot Nothing) And (wPrivate.companybranchno <> "" And wPrivate.companybranchno IsNot Nothing) Then
                    Dim pad As Char
                    pad = "0"c
                    in_strCompCode = in_strCompCode.PadLeft(3, pad)
                    in_strBranchCode = in_strBranchCode.PadLeft(3, pad)
                    If Not wPrivate.companycd.PadLeft(3, pad).Equals(in_strCompCode) Or Not wPrivate.companybranchno.PadLeft(3, pad).Equals(in_strBranchCode) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00023", "企業コード、枝番", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.companycd)
                    End If
                End If
                If wPrivate.belongname1 <> "" And wPrivate.belongname1 IsNot Nothing Then
                    'Check External Character wPrivate.belongname1
                    If MNBTCMN100.CheckExternalCharacter(wPrivate.belongname1) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00003", "所属名１", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.belongname1)
                    End If
                    If MNBTCMN100.checkLength(wPrivate.belongname1, 80) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "所属名１", "80バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.belongname1)
                    End If
                End If
                If wPrivate.belongname2 <> "" And wPrivate.belongname2 IsNot Nothing Then
                    'Check External Character wPrivate.belongname2
                    If MNBTCMN100.CheckExternalCharacter(wPrivate.belongname2) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00003", "所属名２", "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.belongname2)
                    End If
                    If MNBTCMN100.checkLength(wPrivate.belongname2, 80) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "所属名２", "80バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.belongname2)
                    End If
                End If
                If wPrivate.tel <> "" And wPrivate.tel IsNot Nothing Then
                    If MNBTCMN100.checkLength(wPrivate.tel, 13) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "電話番号", "13バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.tel)
                    End If
                End If
                If wPrivate.fax <> "" And wPrivate.fax IsNot Nothing Then
                    If MNBTCMN100.checkLength(wPrivate.fax, 13) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "ＦＡＸ", "13バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.fax)
                    End If
                End If
                If wPrivate.sendtel <> "" And wPrivate.sendtel IsNot Nothing Then
                    If MNBTCMN100.checkLength(wPrivate.sendtel, 13) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "宛名送付先電話番号", "13バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.sendtel)
                    End If
                End If
                If wPrivate.sendfax <> "" And wPrivate.sendfax IsNot Nothing Then
                    If MNBTCMN100.checkLength(wPrivate.sendfax, 13) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "宛名送付先ＦＡＸ", "13バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.sendfax)
                    End If
                End If
                If wPrivate.mail <> "" And wPrivate.mail IsNot Nothing Then
                    If MNBTCMN100.checkLength(wPrivate.mail, 50) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "メールアドレス", "50バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.mail)
                    End If
                End If
                If wPrivate.belongcd1 <> "" And wPrivate.belongcd1 IsNot Nothing Then
                    'Check alphanumeric wPrivate.belongcd1
                    If MNBTCMN100.CheckValidAlphanumeric(wPrivate.belongcd1) Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "所属コード", "半角英数", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.belongcd1)
                    End If
                    If MNBTCMN100.checkLength(wPrivate.belongcd1, 20) = False Then
                        message = MNBTCMN100.GetMessageContent("MSGVWE00015", "所属コード", "20バイト以内", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.belongcd1)
                    End If
                End If
                If errComCdFlag = False Or errbranchFlag = False Or errStaffCdFlag = False Then

                    'Dim querySql = "Select LINE  from W_COMPANY"
                    'querySql = querySql & " where companycd = '" & wPrivate.companycd & "'"
                    'querySql = querySql & " And companybranchno = '" & wPrivate.companybranchno & "'"
                    'querySql = querySql & " And staffcd = '" & wPrivate.staffcd & "'"
                    Dim query = (From wp In in_context.w_private
                                 Where wp.seq = in_intSeq And wp.companycd = wPrivate.companycd And wp.companybranchno = wPrivate.companybranchno And wp.staffcd = wPrivate.staffcd
                                 Select New With {.line = wp.line}).ToList()
                    If query.Count >= 2 Then
                        Dim lstLine As StringBuilder = New StringBuilder()
                        For j As Integer = 0 To query.Count - 1
                            lstLine.Append(query(j).line)
                            If j < query.Count - 1 Then
                                lstLine.Append(",")
                            End If
                        Next
                        Dim param As String = lstLine.ToString() & "行目の企業コード、枝番、社員コード"
                        message = MNBTCMN100.GetMessageContent("MSGVWE00024", param, "", "")
                        inout_intErrCount = inout_intErrCount + 1
                        ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, wPrivate.companycd & "、" & wPrivate.companybranchno & "、" & wPrivate.staffcd)
                    End If
                End If
                If in_intImportType = 2 Then
                    If errComCdFlag = False Or errbranchFlag = False Or errStaffCdFlag = False Then
                        Dim query = (From p In lstTprivate
                                 Where p.companycd = wPrivate.companycd And p.companybranchno = CInt(wPrivate.companybranchno) And p.staffcd = wPrivate.staffcd
                                 Select p).ToList()
                        If query.Count > 0 Then
                            Dim param As String = wPrivate.companycd & "、" & wPrivate.companybranchno & "、" & wPrivate.staffcd
                            message = MNBTCMN100.GetMessageContent("MSGVWE00022", param, "", "")
                            inout_intErrCount = inout_intErrCount + 1
                            ImportDetailLog(in_context, in_intSeq, inout_intErrCount, wPrivate.line, message, csvData, param)
                        End If
                    End If
                End If
            Next

            common.setProgress(in_lstQuerywPrivate.Count, 1)

        End If
        Return in_context
    End Function

    ''' <summary>
    ''' Import data to table t_importdetaillog
    ''' </summary>
    ''' <param name="inout_context">context as mynoEntities</param>
    ''' <param name="in_intSeq">SEQ as String</param>
    ''' <param name="in_intLine">line as Integer</param>
    ''' <param name="in_strMessage">message as String</param>
    ''' <param name="in_strRecData">recData as String</param>
    ''' <param name="in_intColData">colData as String</param>
    ''' <remarks></remarks>
    Public Sub ImportDetailLog(ByRef inout_context As mynoEntities, ByVal in_intSeq As Integer, ByVal in_intDetailSeq As Integer,
                               ByVal in_intLine As Integer, ByVal in_strMessage As String,
                               ByVal in_strRecData As String, ByVal in_intColData As String)
        Try
            Dim params() As Npgsql.NpgsqlParameter
            Dim bindParan As Hashtable = New Hashtable
            Dim strSql As String = ""


            strSql = strSql & "insert into t_importdetaillog"
            strSql = strSql & " values("
            strSql = strSql & " cast(:seq as integer)"
            strSql = strSql & ", cast(:detailseq as bigint)"
            strSql = strSql & ", cast(:line as bigint)"
            strSql = strSql & ",2"
            strSql = strSql & ",:message"
            strSql = strSql & ",:recdata"
            strSql = strSql & ",:coldata"
            strSql = strSql & ",current_timestamp"
            strSql = strSql & ",:addjusercd"
            strSql = strSql & ",:terminalcd"
            strSql = strSql & ")"

            bindParan("seq") = in_intSeq
            bindParan("detailseq") = in_intDetailSeq
            If in_intLine = 0 Then
                bindParan("line") = Nothing
            Else
                bindParan("line") = in_intLine
            End If

            bindParan("message") = in_strMessage
            bindParan("recdata") = in_strRecData
            bindParan("coldata") = in_intColData
            bindParan("addjusercd") = p_strTerminalCdLogin
            bindParan("terminalcd") = p_strTerminalCdLogin

            params = setParameters(bindParan)

            inout_context.Database.ExecuteSqlCommand(strSql, params)
        Catch ex As Exception
            'c_frmMNUIDTR100.BeginInvoke(New Action(Sub()
            '                                           frmLoading.Hide()
            '                                           c_frmMNUIDTR100.Activate()
            '                                       End Sub))
            frmLoading.Hide()
            c_frmMNUIDTR100.Activate()
            MNBTCMN100.ShowMessageException()
        End Try

    End Sub

    ' ''' <summary>
    ' ''' Import data to table t_importdetaillog of List Error
    ' ''' </summary>
    ' ''' <param name="inout_context">context as mynoEntities</param>
    ' ''' <param name="in_intSeq">SEQ as String</param>
    ' ''' <param name="in_lstLog">list Error</param>
    ' ''' <remarks></remarks>
    'Public Sub ImportDetailLogList(ByRef inout_context As mynoEntities, ByVal in_intSeq As Integer, ByVal in_lstLog As List(Of LstDetailLog))
    '    Try
    '        Dim item As t_importdetaillog
    '        Dim listItem As List(Of t_importdetaillog) = New List(Of t_importdetaillog)()
    '        Dim datetimeNow As DateTime = MNBTCMN100.GetCurrentTimestamp(inout_context)
    '        For i As Integer = 0 To in_lstLog.Count - 1
    '            item = New t_importdetaillog
    '            item.seq = in_intSeq
    '            item.detailseq = in_lstLog(i).tempDetailSeq
    '            item.line = in_lstLog(i).tempLine
    '            item.errflg = 2
    '            item.message = in_lstLog(i).tempMsg
    '            item.recdata = in_lstLog(i).tempCsvData
    '            item.coldata = in_lstLog(i).tempColData
    '            item.adddatetime = datetimeNow
    '            item.addjusercd = p_strUserCdLogin
    '            item.terminalcd = p_strTerminalCdLogin
    '            listItem.Add(item)
    '        Next
    '        inout_context.t_importdetaillog.AddRange(listItem)
    '        inout_context.SaveChanges()
    '    Catch ex As Exception
    '        frmLoading.Hide()
    '        c_frmMNUIDTR100.Activate()
    '        MNBTCMN100.ShowMessageException()
    '    End Try

    'End Sub
    ''' <summary>
    ''' Import data to DB and finish process
    ''' </summary>
    ''' <param name="in_srtFilePath"></param>
    ''' <param name="in_intSeq"></param>
    ''' <param name="in_strCompanyCd"></param>
    ''' <param name="in_strBranchNoCd"></param>
    ''' <param name="in_intFileType"></param>
    ''' <param name="in_intImportType"></param>
    ''' <param name="in_lstwPrivate"></param>
    ''' <param name="in_lstwFamily"></param>
    ''' <remarks></remarks>
    Public Function ProcessDataImport(ByVal in_srtFilePath As String, ByVal in_intSeq As Integer, ByVal in_strCompanyCd As String,
                                 ByVal in_strBranchNoCd As String, ByVal in_intFileType As Integer,
                                 ByVal in_intImportType As Integer, ByVal in_lstwPrivate As List(Of w_private),
                                 ByVal in_lstwFamily As List(Of w_family), ByRef inout_mynoContext As mynoEntities,
                                ByRef p_errFlg As Integer) As Boolean
        Try
            Dim strSql As String
            Dim common As MNBTCMN100 = New MNBTCMN100

            common.setProgressMax(0, "登録処理(3/4)")

            'If radio 「全件(削除追加)」 checked
            If in_intImportType = 1 Then
                'If radio「個人」checked
                If in_intFileType = 1 Then

                    strSql = "delete from {0}"
                    strSql = strSql & " where companycd = '" & in_strCompanyCd & "'"
                    strSql = strSql & " and companybranchno = " & in_strBranchNoCd

                    inout_mynoContext.Database.ExecuteSqlCommand(String.Format(strSql, "t_private"))
                    inout_mynoContext.Database.ExecuteSqlCommand(String.Format(strSql, "t_family"))
                    inout_mynoContext.Database.ExecuteSqlCommand(String.Format(strSql, "t_familyresult"))

                ElseIf in_intFileType = 2 Then
                    'If radio「家族」checked
                    strSql = "delete from {0}"
                    strSql = strSql & " where companycd = '" & in_strCompanyCd & "'"
                    strSql = strSql & " and companybranchno = '" & in_strBranchNoCd & "'"
                    strSql = strSql & " and familyno != 0"

                    inout_mynoContext.Database.ExecuteSqlCommand(String.Format(strSql, "t_family"))
                    inout_mynoContext.Database.ExecuteSqlCommand(String.Format(strSql, "t_familyresult"))

                End If
                inout_mynoContext.SaveChanges()
            End If
            Dim countImportDb As Integer = 0
            'If radio「個人」checked
            If in_intFileType = 1 Then
                ' Check list private data 
                If in_lstwPrivate.Count > 0 Then
                    Dim current As DateTime = MNBTCMN100.GetCurrentTimestamp(inout_mynoContext).ToString("yyyy/MM/dd HH:mm:ss")
                    Dim countPriImportDb As Integer = in_lstwPrivate.Count

                    Application.DoEvents()
                    Dim StartTime As DateTime = DateTime.Now

                    common.setProgressMax(0, "登録(Private)(3/4)")
                    CreatePrivete(inout_mynoContext, in_intSeq, current)

                    common.setProgressMax(0, "登録(Family)(3/4)")
                    CreateFamily(inout_mynoContext, in_intSeq, current)

                    common.setProgressMax(0, "登録(FamilyResult)(3/4)")
                    CreateFamilyResult(inout_mynoContext, in_intSeq, current)

                    Dim EndTime As DateTime = DateTime.Now
                    Console.WriteLine("total time" + (EndTime - StartTime).ToString())
                    countImportDb = countPriImportDb
                End If

            ElseIf in_intFileType = 2 Then
                'If radio「家族」checked
                ' Check list private data 
                If in_lstwFamily.Count > 0 Then
                    Dim wfmailyOrderBy = (From ord In in_lstwFamily Order By ord.staffcd).ToList()
                    Dim current As DateTime = MNBTCMN100.GetCurrentTimestamp(inout_mynoContext).ToString("yyyy/MM/dd HH:mm:ss")
                    'Count total record in table w_family
                    Dim query = From wf In inout_mynoContext.t_family Select wf
                    Dim countFamImportDb As Integer = in_lstwFamily.Count

                    'For i As Integer = 0 To wfmailyOrderBy.Count - 1
                    Application.DoEvents()

                    common.setProgressMax(0, "登録(Family)(3/4)")
                    CreateForFamilyCheck(inout_mynoContext, in_intSeq, current)

                    common.setProgressMax(0, "登録(FamilyResult)(3/4)")
                    CreateForFamilyResultCheck(inout_mynoContext, in_intSeq, current)

                    countImportDb = countFamImportDb
                End If
            End If
            inout_mynoContext.SaveChanges()

            common.setProgressMax(0, "不要データ削除(4/4)")

            '印刷区分の設定
            UpdateForPrivatePrtFlg(inout_mynoContext, in_intSeq, in_intImportType, in_intFileType, in_strCompanyCd, in_strBranchNoCd)

            Console.WriteLine("in_intSeq" + in_intSeq.ToString())

            'Insert log detail 6.1.1.8.4.a
            MNBTCMN100.InputLogDetail(inout_mynoContext, in_intSeq, "", "ファイルファイル取込件数:" + countImportDb.ToString("###,###,###"), in_srtFilePath)

            '家族情報の重複チェック(ワークテーブル内)
            Dim errFlg As Integer = OverlapFamilyCheck(inout_mynoContext, in_intSeq, in_srtFilePath)

            If p_errFlg = 1 OrElse errFlg = 1 Then
                errFlg = 1
                p_errFlg = 1
            End If

            strSql = "update t_importlog set errflg = " & errFlg & " where seq = " & in_intSeq
            '            strSql = "update t_importlog set errflg = 0 where seq = " & in_intSeq
            inout_mynoContext.Database.ExecuteSqlCommand(strSql)

            strSql = "delete from w_private where seq = " & in_intSeq
            inout_mynoContext.Database.ExecuteSqlCommand(strSql)

            strSql = "delete from w_family where seq = " & in_intSeq
            inout_mynoContext.Database.ExecuteSqlCommand(strSql)

            frmLoading.Hide()
            c_frmMNUIDTR100.Activate()
            Return True
        Catch ex As Exception
            frmLoading.Hide()
            c_frmMNUIDTR100.Activate()
            MNBTCMN100.ShowMessageException()
            Return False
        End Try
    End Function
    '*****************************************************
    '*処理内容： 家族情報が11件以上存在する場合、印刷区分を2に設定する。
    '*
    '*戻り値：無し
    '*****************************************************
    Private Sub UpdateForPrivatePrtFlg(inout_mynoContext As mynoEntities, in_intSeq As Integer,
                                       in_intImportType As Integer, in_intFileType As Integer,
                                       in_strCompanyCd As String, in_strBranchNoCd As String)

        Dim current As DateTime = MNBTCMN100.GetCurrentTimestamp(inout_mynoContext).ToString("yyyy/MM/dd HH:mm:ss")
        Dim strSql As String

        strSql = "UPDATE T_PRIVATE TP SET PRINTFLG = TD.PRTF"
        strSql = strSql & " , UPDDATETIME = '" & current & "'"
        strSql = strSql & " , UPDUSERCD   = '" & p_strUserCdLogin & "'"
        strSql = strSql & "  FROM ("
        strSql = strSql & " SELECT "
        strSql = strSql & "        WT.COMPANYCD "
        strSql = strSql & "       ,WT.COMPANYBRANCHNO "
        strSql = strSql & "       ,WT.STAFFCD "
        strSql = strSql & "       ,CASE "
        strSql = strSql & "          WHEN WT.CNT > 10 THEN 2"
        strSql = strSql & "          ELSE 1"
        strSql = strSql & "      END AS PRTF"
        strSql = strSql & " FROM "
        strSql = strSql & " ("
        strSql = strSql & "     SELECT P.COMPANYCD"
        strSql = strSql & "             ,P.companybranchno"
        strSql = strSql & "             ,P.STAFFCD"
        strSql = strSql & "             ,COUNT(*) AS CNT"
        strSql = strSql & "       FROM T_PRIVATE P"

        If in_intImportType = 2 Then
            If in_intFileType = 1 Then
                strSql = strSql & "            INNER JOIN  ("
                strSql = strSql & "                                SELECT COMPANYCD, COMPANYBRANCHNO, STAFFCD FROM W_PRIVATE"
            Else
                strSql = strSql & "            INNER JOIN  ("
                strSql = strSql & "                                SELECT COMPANYCD, COMPANYBRANCHNO, STAFFCD FROM W_FAMILY"
            End If

            strSql = strSql & "                                  WHERE SEQ = " & in_intSeq
            strSql = strSql & "                                    GROUP BY COMPANYCD"
            strSql = strSql & "                                                   ,companybranchno"
            strSql = strSql & "                                                   ,STAFFCD"
            strSql = strSql & "                                  ) WPP ON"
            strSql = strSql & "                     P.COMPANYCD = WPP.COMPANYCD"
            strSql = strSql & "                 AND P.companybranchno = CAST(WPP.companybranchno AS INTEGER)"
            strSql = strSql & "                 AND P.STAFFCD         = WPP.STAFFCD"
        End If
        strSql = strSql & "            LEFT JOIN T_FAMILY F ON "
        strSql = strSql & "                     P.COMPANYCD       = F.COMPANYCD"
        strSql = strSql & "                 AND P.companybranchno = F.companybranchno"
        strSql = strSql & "                 AND P.STAFFCD         = F.STAFFCD"
        strSql = strSql & "     GROUP BY P.COMPANYCD"
        strSql = strSql & "             ,P.companybranchno"
        strSql = strSql & "             ,P.STAFFCD"
        strSql = strSql & " ) WT"
        strSql = strSql & " WHERE "
        strSql = strSql & "      WT.COMPANYCD       = '" & in_strCompanyCd & "'"
        strSql = strSql & "  AND WT.companybranchno = " & in_strBranchNoCd
        strSql = strSql & " ) TD"
        strSql = strSql & " WHERE "
        strSql = strSql & "         TP.COMPANYCD              = TD.COMPANYCD"
        strSql = strSql & "  AND TP.companybranchno      = TD.companybranchno"
        strSql = strSql & "  AND TP.STAFFCD                   = TD.STAFFCD"
        strSql = strSql & "  AND TP.PRINTFLG                  <> TD.PRTF"

        inout_mynoContext.Database.ExecuteSqlCommand(strSql)

    End Sub
    '*****************************************************
    '*処理内容： 家族情報に重複するデータが存在するかチェックする。
    '*
    '*戻り値：存在する場合=1,存在しない場合=0
    '*****************************************************
    Private Function OverlapFamilyCheck(ByRef inout_context As mynoEntities, ByVal in_intSeq As Integer, in_srtFilePath As String) As Integer

        Dim strSql As String
        Dim lstRs As List(Of Hashtable)

        strSql = " select * "
        strSql = strSql & " from "
        strSql = strSql & " ( "
        strSql = strSql & " select "
        strSql = strSql & "   companycd "
        strSql = strSql & " , companybranchno "
        strSql = strSql & " , staffcd "
        strSql = strSql & " , familynamekana "
        strSql = strSql & " , sex "
        strSql = strSql & " , birthdate "
        strSql = strSql & " , line "
        strSql = strSql & " , count(*) over(partition by companycd, companybranchno, staffcd, familynamekana, sex, birthdate) as cnt "
        strSql = strSql & " from "
        strSql = strSql & "   w_family "
        strSql = strSql & " where "
        strSql = strSql & "  seq = " & in_intSeq
        strSql = strSql & " ) f "
        strSql = strSql & " where "
        strSql = strSql & "  cnt > 1 "
        strSql = strSql & " order by"
        strSql = strSql & "   companycd "
        strSql = strSql & " , companybranchno "
        strSql = strSql & " , staffcd "
        strSql = strSql & " , familynamekana "
        strSql = strSql & " , sex "
        strSql = strSql & " , birthdate "
        strSql = strSql & " , line "

        Using pdb = New postDB(inout_context)
            lstRs = pdb.getSql(strSql)
        End Using

        If lstRs Is Nothing OrElse lstRs.Count = 0 Then Return 0

        Dim Message = MNBTCMN100.GetMessageContent("MSGVWE00024", "家族情報", "", "")
        Dim iCnt As Integer = 0
        For Each hRs As Hashtable In lstRs
            iCnt += 1
            Dim colData As String() = {hRs("companycd"), hRs("companybranchno"), hRs("staffcd"), hRs("familynamekana"), hRs("sex"), hRs("birthdate")}

            ImportDetailLog(inout_context, in_intSeq, iCnt, hRs("line"), Message, in_srtFilePath, String.Join("、", colData))

        Next

        Return 1

    End Function
    ''' <summary>
    ''' Export file Csv
    ''' </summary>
    ''' <param name="in_StrSeq"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExportCsv(ByVal in_StrSeq As Integer) As Boolean
        Try
            ExportCsv = False
            p_mynoContext = New mynoEntities()
            Dim queryData = From im In p_mynoContext.t_importlog
                            Where im.seq = in_StrSeq
                            Select New With {.companyCd = im.companycd, .branch = im.companybranchno, .csvFile = im.csvfile, .filename = im.filename}
            Dim pathSave As String = MNBTCMN100.GetConfig("Export_Dir_Csv_BasicDataiFile", "config.ini")
            If IO.Directory.Exists(pathSave) Then
                Dim query = queryData.SingleOrDefault()
                'Export file
                If query IsNot Nothing Then
                    Dim filepath As String
                    filepath = pathSave + query.filename
                    If query.csvFile IsNot Nothing Then
                        'Delete file csv if exists
                        If IO.File.Exists(filepath) Then
                            File.Delete(filepath)
                        End If
                        Dim csvOut As String = System.Text.Encoding.GetEncoding("shift_jis").GetString(query.csvFile)
                        File.AppendAllText(filepath, csvOut, System.Text.Encoding.GetEncoding("shift_jis"))
                        Using transScope As DbContextTransaction = p_mynoContext.Database.BeginTransaction(IsolationLevel.Chaos)
                            'Call function setting first log system
                            Dim seq As Integer = MNBTCMN100.InputLogMaster(p_mynoContext, "2", "MNUIDTR100", query.companyCd, query.branch)

                            Dim strQuery As String = queryData.ToString()
                            'replace parameter input log
                            If strQuery.IndexOf("@p__linq__0") > 0 Then
                                strQuery = strQuery.Replace("Project1", "P1")
                                strQuery = strQuery.Replace("@p__linq__0", "'" & in_StrSeq.ToString() & "'")
                            End If
                            Dim message As String = "ファイル出力 件数:1 (" & strQuery & ")"
                            MNBTCMN100.InputLogDetail(p_mynoContext, seq, "", message, filepath)
                            transScope.Commit()
                        End Using
                        ExportCsv = True
                    End If
                End If
            Else
                frmLoading.Close()
                MNBTCMN100.ShowMessage("MSGVWE00018", "基礎データ取込ファイルの出力先ディレクトリ", "", "")
                ExportCsv = False
            End If
            frmLoading.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' <summary>
    ''' Get data for export xlsx in table t_importlog and t_importdetaillog
    ''' </summary>
    ''' <param name="out_Query"></param>
    ''' <param name="mynoContext"></param>
    ''' <param name="in_Seq"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataForExportXlsx(ByRef out_Query As String, ByRef mynoContext As mynoEntities, ByVal in_Seq As Integer) As List(Of ItemListImport)
        Try
            Dim queryData = From im In mynoContext.t_importlog Join imd In mynoContext.t_importdetaillog
                            On im.seq Equals imd.seq
                            Where im.seq = in_Seq
                            Order By imd.detailseq Ascending
                            Select New ItemListImport With {
                                .Seq = im.seq,
                                .CompanyCd = im.companycd,
                                .CompanyBranchNo = im.companybranchno,
                                .CompanyName = im.companyname,
                                .CompanyBranchName = im.companyname,
                                .ImpDatetime = im.impdatetime,
                                .Line = imd.line,
                                .Message = imd.message,
                                .RecData = imd.recdata,
                                .ColData = imd.coldata,
                                .DetailSeq = imd.detailseq
                                }
            out_Query = queryData.ToString()
            Return queryData.ToList()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' <summary>
    ''' xlsx export file
    ''' </summary>
    ''' <param name="in_lstGetListError"></param>
    ''' <param name="in_context"></param>
    ''' <param name="in_strQuery"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExportXlsxListError(ByVal in_lstGetListError As List(Of ItemListImport), ByVal in_context As mynoEntities, ByVal in_strQuery As String, ByVal in_intSeq As Integer, ByRef inout_strFilePathSave As String) As Boolean
        Dim wb As XSSFWorkbook = Nothing
        Try
            Using transScope As DbContextTransaction = in_context.Database.BeginTransaction(IsolationLevel.Chaos)
                Dim FilePath As String = Directory.GetCurrentDirectory + "\Template\Temp_MNUIDTR100.xlsx"
                'Open book here
                Using fs As New FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Read)
                    wb = New XSSFWorkbook(fs)
                End Using

                'Get sheet by the name
                Dim sht1 As XSSFSheet = wb.GetSheet("基礎データエラーリスト")
                Dim printRow As Integer = 0
                'Copy row temp in excel
                For i As Integer = 0 To in_lstGetListError.Count - 2
                    Dim rowCopy As Integer
                    rowCopy = 8 + i + 1
                    sht1.CopyRow(8, rowCopy)
                    printRow = rowCopy
                Next

                Dim currentDate As DateTime = MNBTCMN100.GetCurrentTimestamp(in_context)
                'Set data to excel
                Dim itemError As ItemListImport = in_lstGetListError(0)
                'Update data to 件数
                sht1.GetRow(2).GetCell(5).SetCellValue("件数：" & in_lstGetListError.Count.ToString() & "件")
                'Update data to 企業コード
                If itemError.CompanyCd <> Nothing Then
                    sht1.GetRow(3).GetCell(2).SetCellValue(itemError.CompanyCd)
                Else
                    sht1.GetRow(3).GetCell(2).SetCellValue(String.Empty)
                End If
                'Update data to 企業名
                If itemError.CompanyName <> Nothing Then
                    sht1.GetRow(4).GetCell(2).SetCellValue(itemError.CompanyName)
                Else
                    sht1.GetRow(4).GetCell(2).SetCellValue(String.Empty)
                End If
                'Update data to 枝番
                If itemError.CompanyBranchNo <> Nothing Then
                    sht1.GetRow(3).GetCell(4).SetCellValue(itemError.CompanyBranchNo.ToString())
                Else
                    sht1.GetRow(3).GetCell(4).SetCellValue(String.Empty)
                End If
                'Update data to 枝番名
                If itemError.CompanyBranchName <> Nothing Then
                    sht1.GetRow(4).GetCell(4).SetCellValue(itemError.CompanyBranchName)
                Else
                    sht1.GetRow(4).GetCell(4).SetCellValue(String.Empty)
                End If
                'Update data to 基礎データ取込日時
                If itemError.ImpDatetime <> Nothing Then
                    sht1.GetRow(5).GetCell(2).SetCellValue(itemError.ImpDatetime.ToString("yyyy/MM/dd HH:mm"))
                Else
                    sht1.GetRow(5).GetCell(2).SetCellValue(String.Empty)
                End If
                'Update data to 出力日時
                sht1.GetRow(5).GetCell(4).SetCellValue(currentDate.ToString("yyyy/MM/dd HH:mm"))
                'Update Seq
                If itemError.Seq <> Nothing Then
                    sht1.GetRow(3).GetCell(5).SetCellValue("SEQ ：" & itemError.Seq.ToString())
                Else
                    sht1.GetRow(3).GetCell(6).SetCellValue(String.Empty)
                End If
                'Set data to table
                Dim errorItem As ItemListImport
                For i As Integer = 0 To in_lstGetListError.Count - 1
                    errorItem = New ItemListImport
                    errorItem = in_lstGetListError(i)
                    Dim row As Integer = 8 + i
                    sht1.GetRow(row).Height = 270
                    'Update data to 行数
                    If errorItem.Line IsNot Nothing Then
                        sht1.GetRow(row).GetCell(1).SetCellValue(CInt(errorItem.Line).ToString("###,###,###") & "行目")
                    Else
                        sht1.GetRow(row).GetCell(1).SetCellValue(String.Empty)
                    End If
                    'Update data to エラーメッセージ
                    If errorItem.Message <> Nothing Then
                        sht1.GetRow(row).GetCell(2).SetCellValue(errorItem.Message)
                    Else
                        sht1.GetRow(row).GetCell(2).SetCellValue(String.Empty)
                    End If
                    'Update data to エラー項目
                    If errorItem.ColData <> Nothing Then
                        sht1.GetRow(row).GetCell(3).SetCellValue(errorItem.ColData)
                    Else
                        sht1.GetRow(row).GetCell(3).SetCellValue(String.Empty)
                    End If
                    'Update data to データ内容
                    If errorItem.RecData <> Nothing Then
                        sht1.GetRow(row).GetCell(4).SetCellValue(errorItem.RecData)
                    Else
                        sht1.GetRow(row).GetCell(4).SetCellValue(String.Empty)
                    End If
                    sht1.GetRow(row).GetCell(5).SetCellValue(String.Empty)
                    'Update data to 備考
                    'If errorItem.DetailSeq <> Nothing Then
                    '    sht1.GetRow(row).GetCell(5).SetCellValue(errorItem.DetailSeq)
                    'Else
                    '    sht1.GetRow(row).GetCell(5).SetCellValue(String.Empty)
                    'End If
                Next
                If printRow = 0 Then
                    wb.SetPrintArea(0, 0, 5, 0, 8)
                Else
                    wb.SetPrintArea(0, 0, 5, 0, printRow + 1)
                End If
                Dim pathSave As String = MNBTCMN100.GetConfig("Export_Dir_Xlsx_BasicDataImportErrorList", "config.ini")
                If IO.Directory.Exists(pathSave) = False Then
                    frmLoading.Close()
                    ExportXlsxListError = False
                    MNBTCMN100.ShowMessage("MSGVWE00018", "基礎データ取込エラーリストの出力先ディレクトリ", "", "")
                Else
                    'Save file
                    Dim dateNow As String = currentDate.ToString("yyyyMMddHHmmss")
                    Dim branchno As String = itemError.CompanyBranchNo.ToString
                    Dim pad As Char
                    pad = "0"c
                    branchno = branchno.PadLeft(3, pad)
                    'Get name file 
                    Dim nameFile As String = MNBTCMN100.GetConfig("PrefixFileName_Xlsx_BasicDataImportErrorList", "config.ini")
                    Dim str = System.IO.Path.Combine(pathSave, nameFile & "_" & itemError.CompanyCd.ToString & "_" & branchno & "_" & dateNow & "_" & in_intSeq.ToString("0000000000") & ".xlsx")
                    Dim filePathSave As String = pathSave & nameFile & "_" & itemError.CompanyCd.ToString & "_" & branchno & "_" & dateNow & "_" & in_intSeq.ToString("0000000000") & ".xlsx"
                    Using fs As New FileStream(filePathSave, FileMode.Create)
                        wb.Write(fs)
                    End Using
                    'InsertLog
                    Dim seq As Integer = MNBTCMN100.InputLogMaster(in_context, "2", "MNUIDTR100", itemError.CompanyCd, itemError.CompanyBranchNo)
                    'replace parameter input log
                    If in_strQuery.IndexOf("@p__linq__0") > 0 Then
                        in_strQuery = in_strQuery.Replace("Project1", "P1")
                        in_strQuery = in_strQuery.Replace("@p__linq__0", "'" & in_intSeq.ToString() & "'")
                    End If
                    Dim message As String = "ファイル出力 件数:" & in_lstGetListError.Count.ToString("###,###,###") & " (" & in_strQuery & ")"
                    inout_strFilePathSave = filePathSave
                    MNBTCMN100.InputLogDetail(in_context, seq, "", message, filePathSave)
                    transScope.Commit()
                    ExportXlsxListError = True
                End If
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub CreatePrivete(inout_mynoContext As mynoEntities, in_intImportSeq As Integer, current As DateTime)

        'Declare variable for sql
        Dim strSql As String

        strSql = "INSERT INTO T_PRIVATE(companycd, companybranchno, staffcd, sts, postno, "
        strSql = strSql & "             prefecturescd, address1, address2, tel, fax, "
        strSql = strSql & "             sendpostno, sendprefecturescd, sendaddress1, sendaddress2, sendtel, "
        strSql = strSql & "             sendfax, mail, belongcd1, belongname1, belongname2, "
        strSql = strSql & "             notes, vipflg, abnormalflg, abnormalcount, printflg, "
        strSql = strSql & "             sendpostnoflg, privateno1usercd, privateno1datetime, privateno2usercd, privateno2datetime, "
        strSql = strSql & "             privatenocusercd, privatenocdatetime, expusercd, expdatetime, delflg, "
        strSql = strSql & "             deldatetime, delusercd, adddatetime, addjusercd, upddatetime, "
        strSql = strSql & "             updusercd, terminalcd)"
        strSql = strSql & " select"
        strSql = strSql & " COMPANYCD"
        strSql = strSql & ", cast(COMPANYBRANCHNO as integer)"
        strSql = strSql & ", STAFFCD"
        strSql = strSql & ", '200'"
        strSql = strSql & ", POSTNO"
        strSql = strSql & ", PREFECTURESCD"
        strSql = strSql & ", ADDRESS1"
        strSql = strSql & ", ADDRESS2"
        strSql = strSql & ", TEL"
        strSql = strSql & ", FAX"
        strSql = strSql & ", SENDPOSTNO"
        strSql = strSql & ", SENDPREFECTURESCD"
        strSql = strSql & ", SENDADDRESS1"
        strSql = strSql & ", SENDADDRESS2"
        strSql = strSql & ", SENDTEL"
        strSql = strSql & ", SENDFAX"
        strSql = strSql & ", MAIL"
        strSql = strSql & ", BELONGCD1"
        strSql = strSql & ", BELONGNAME1"
        strSql = strSql & ", BELONGNAME2"
        strSql = strSql & ", Null"
        strSql = strSql & ", VIPFLG"
        strSql = strSql & ", 0"
        strSql = strSql & ", 0"
        strSql = strSql & ", 0"
        strSql = strSql & ", CASE when SUBSTRING(sendpostno, LENGTH(sendpostno)-1, LENGTH(sendpostno)) = '00' THEN 1"
        strSql = strSql & "       ELSE 2"
        strSql = strSql & "  END"
        strSql = strSql & ", Null"
        strSql = strSql & ", Null"
        strSql = strSql & ", Null"
        strSql = strSql & ", Null"
        strSql = strSql & ", Null"
        strSql = strSql & ", Null"
        strSql = strSql & ", Null"
        strSql = strSql & ", Null"
        strSql = strSql & ", 0"
        strSql = strSql & ", Null"
        strSql = strSql & ", Null"
        strSql = strSql & ", '" & current & "'"
        strSql = strSql & ", '" & p_strUserCdLogin & "'"
        strSql = strSql & ", Null"
        strSql = strSql & ", Null"
        strSql = strSql & ", '" & p_strTerminalCdLogin & "'"
        strSql = strSql & " FROM"
        strSql = strSql & "      W_PRIVATE wPrivate"
        strSql = strSql & " WHERE "
        strSql = strSql & "      companybranchno IS NOT NULL"
        strSql = strSql & "    and seq = " & in_intImportSeq
        inout_mynoContext.Database.ExecuteSqlCommand(strSql)
    End Sub

    Private Sub CreateFamily(inout_mynoContext As mynoEntities, in_intImportSeq As Integer, current As DateTime)

        Dim strSql As String

        strSql = "insert into t_family (companycd, companybranchno, staffcd, familyno, familyname, "
        strSql = strSql & "             familynamekana, birthdate, sex, insuredflg, delflg, "
        strSql = strSql & "             deldatetime, delusercd, adddatetime, addjusercd, upddatetime, "
        strSql = strSql & "             updusercd, terminalcd )"
        strSql = strSql & " select"
        strSql = strSql & "  COMPANYCD"
        strSql = strSql & ", cast(COMPANYBRANCHNO as integer)"
        strSql = strSql & ", STAFFCD"
        strSql = strSql & ", 0"
        strSql = strSql & ", FAMILYNAME"
        strSql = strSql & ", FAMILYNAMEKANA"
        strSql = strSql & ", to_timestamp(BIRTHDATE,'yyyy/mm/dd')"
        strSql = strSql & ", CASE when SEX != '' THEN cast(SEX as integer) ELSE Null END"
        strSql = strSql & ", 0"
        strSql = strSql & ", 0"
        strSql = strSql & ", Null"
        strSql = strSql & ", Null"
        strSql = strSql & ", '" & current & "'"
        strSql = strSql & ", '" & p_strUserCdLogin & "'"
        strSql = strSql & ", Null"
        strSql = strSql & ", Null"
        strSql = strSql & ", '" & p_strTerminalCdLogin & "'"
        strSql = strSql & " FROM "
        strSql = strSql & "      w_private"
        strSql = strSql & " WHERE "
        strSql = strSql & "      companybranchno IS NOT NULL"
        strSql = strSql & "    and seq = " & in_intImportSeq

        inout_mynoContext.Database.ExecuteSqlCommand(strSql)
    End Sub

    Private Sub CreateFamilyResult(inout_mynoContext As mynoEntities, in_intImportSeq As Integer, current As DateTime)
        Dim strSql As String

        For i As Integer = 1 To 3
            strSql = "insert into t_familyresult(companycd, companybranchno, staffcd, familyno, resultflg, "
            strSql = strSql & "                  familyname, familynamekana, birthdate, sex, privatenokey, "
            strSql = strSql & "                  familyaddflg, unofferflg, insuredflg, delflg, deldatetime, "
            strSql = strSql & "                  delusercd, adddatetime, addjusercd, upddatetime, updusercd, terminalcd)"
            strSql = strSql & " select"
            strSql = strSql & "  COMPANYCD"
            strSql = strSql & ", cast(COMPANYBRANCHNO as integer)"
            strSql = strSql & ", STAFFCD"
            strSql = strSql & ", 0"
            strSql = strSql & ", " & i
            strSql = strSql & ", FAMILYNAME"
            strSql = strSql & ", FAMILYNAMEKANA"
            strSql = strSql & ", to_timestamp(BIRTHDATE,'yyyy/mm/dd')"
            strSql = strSql & ", CASE when SEX != '' THEN cast(SEX as integer) ELSE Null END"
            strSql = strSql & ", Null"
            strSql = strSql & ", 0"
            strSql = strSql & ", 0"
            strSql = strSql & ", 0"
            strSql = strSql & ", 0"
            strSql = strSql & ", Null"
            strSql = strSql & ", Null"
            strSql = strSql & ", '" & current & "'"
            strSql = strSql & ", '" & p_strUserCdLogin & "'"
            strSql = strSql & ", Null"
            strSql = strSql & ", Null"
            strSql = strSql & ", '" & p_strTerminalCdLogin & "'"
            strSql = strSql & " FROM "
            strSql = strSql & "      w_private"
            strSql = strSql & " WHERE "
            strSql = strSql & "      companybranchno IS NOT NULL"
            strSql = strSql & "    and seq = " & in_intImportSeq

            inout_mynoContext.Database.ExecuteSqlCommand(strSql)
        Next
    End Sub

    Private Function setParameters(pData As Hashtable) As Npgsql.NpgsqlParameter()

        Dim Parameters() As Npgsql.NpgsqlParameter
        Dim i As Integer = 0

        ReDim Parameters(pData.Count - 1)
        For Each key As String In pData.Keys
            Parameters(i) = New Npgsql.NpgsqlParameter(key, pData(key))
            i += 1
        Next

        Return Parameters

    End Function

    Private Sub CreateForFamilyCheck(inout_mynoContext As mynoEntities, in_intImportSeq As Integer, current As DateTime)

        Dim strSql As String

        strSql = "insert into t_family (companycd, companybranchno, staffcd, familyno, familyname, "
        strSql = strSql & "             familynamekana, birthdate, sex, insuredflg, delflg, "
        strSql = strSql & "             deldatetime, delusercd, adddatetime, addjusercd, upddatetime, "
        strSql = strSql & "             updusercd, terminalcd )"
        strSql = strSql & " select"
        strSql = strSql & "  COMPANYCD"
        strSql = strSql & ", cast(COMPANYBRANCHNO as integer)"
        strSql = strSql & ", STAFFCD"
        strSql = strSql & ", (select max(familyno) from t_family where companycd = w_family.companycd and companybranchno = cast(w_family.companybranchno As integer) and staffcd = w_family.staffcd) + row_number() OVER (partition by w_family.staffcd order by w_family.line)"
        strSql = strSql & ", FAMILYNAME"
        strSql = strSql & ", FAMILYNAMEKANA"
        strSql = strSql & ", to_timestamp(BIRTHDATE,'yyyy/mm/dd')"
        strSql = strSql & ", CASE when SEX != '' THEN cast(SEX as integer) ELSE Null END"
        strSql = strSql & ", cast(insuredflg as integer)"
        strSql = strSql & ", 0"
        strSql = strSql & ", Null"
        strSql = strSql & ", Null"
        strSql = strSql & ", '" & current & "'"
        strSql = strSql & ", '" & p_strUserCdLogin & "'"
        strSql = strSql & ", Null"
        strSql = strSql & ", Null"
        strSql = strSql & ", '" & p_strTerminalCdLogin & "'"
        strSql = strSql & " FROM "
        strSql = strSql & "      w_family"
        strSql = strSql & " WHERE "
        strSql = strSql & "      companybranchno IS NOT NULL"
        strSql = strSql & "    and seq = " & in_intImportSeq

        inout_mynoContext.Database.ExecuteSqlCommand(strSql)
    End Sub

    Private Sub CreateForFamilyResultCheck(inout_mynoContext As mynoEntities, in_intImportSeq As Integer, current As DateTime)

        Dim strSql As String


        strSql = "insert into t_familyresult(companycd, companybranchno, staffcd, familyno, resultflg, "
        strSql = strSql & "                  familyname, familynamekana, birthdate, sex, privatenokey, "
        strSql = strSql & "                  familyaddflg, unofferflg, insuredflg, delflg, "
        strSql = strSql & "                  delusercd, adddatetime, addjusercd, updusercd, terminalcd)"
        strSql = strSql & " (select"
        strSql = strSql & "  COMPANYCD As companycd"
        strSql = strSql & ", cast(COMPANYBRANCHNO as integer) As companybranchno"
        strSql = strSql & ", STAFFCD As staffcd"
        strSql = strSql & ", (select max(familyno) from t_familyresult where companycd = w_family.companycd and companybranchno = cast(w_family.companybranchno As integer) and staffcd = w_family.staffcd) + row_number() OVER (partition by w_family.staffcd order by w_family.line) As familyno"
        strSql = strSql & ", 1 As resultflg"
        strSql = strSql & ", FAMILYNAME As familyname"
        strSql = strSql & ", FAMILYNAMEKANA As familynamekana"
        strSql = strSql & ", to_timestamp(BIRTHDATE,'yyyy/mm/dd') As birthdate"
        strSql = strSql & ", CASE when SEX != '' THEN cast(SEX as integer) ELSE Null END As sex"
        strSql = strSql & ", Null As privatenokey"
        strSql = strSql & ", 0 As familyaddflg"
        strSql = strSql & ", 0 As unofferflg"
        strSql = strSql & ", 0 As insuredflg"
        strSql = strSql & ", 0 As delflg"
        'strSql = strSql & ", NULL As deldatetime"
        strSql = strSql & ", Null"
        'strSql = strSql & ", '" & current & "' As adddatetime"
        strSql = strSql & " ,CURRENT_TIMESTAMP As sysDate"
        strSql = strSql & ", '" & p_strUserCdLogin & "' As addjusercd"
        'strSql = strSql & ", Null As upddatetime"
        strSql = strSql & ", Null As updusercd"
        strSql = strSql & ", '" & p_strTerminalCdLogin & "' As terminalcd"
        strSql = strSql & " FROM "
        strSql = strSql & "      w_family"
        strSql = strSql & " WHERE "
        strSql = strSql & "      companybranchno IS NOT NULL"
        strSql = strSql & "    and seq = " & in_intImportSeq & ")"

        strSql = strSql & " UNION ALL"

        strSql = strSql & " (select"
        strSql = strSql & "  COMPANYCD As companycd"
        strSql = strSql & ", cast(COMPANYBRANCHNO as integer) As companybranchno"
        strSql = strSql & ", STAFFCD As staffcd"
        strSql = strSql & ", (select max(familyno) from t_familyresult where companycd = w_family.companycd and companybranchno = cast(w_family.companybranchno As integer) and staffcd = w_family.staffcd) + row_number() OVER (partition by w_family.staffcd order by w_family.line) As familyno"
        strSql = strSql & ", 2 As resultflg"
        strSql = strSql & ", FAMILYNAME As familyname"
        strSql = strSql & ", FAMILYNAMEKANA As familynamekana"
        strSql = strSql & ", to_timestamp(BIRTHDATE,'yyyy/mm/dd') As birthdate"
        strSql = strSql & ", CASE when SEX != '' THEN cast(SEX as integer) ELSE Null END As sex"
        strSql = strSql & ", Null As privatenokey"
        strSql = strSql & ", 0 As familyaddflg"
        strSql = strSql & ", 0 As unofferflg"
        strSql = strSql & ", 0 As insuredflg"
        strSql = strSql & ", 0 As delflg"
        'strSql = strSql & ", NULL As deldatetime"
        strSql = strSql & ", Null"
        'strSql = strSql & ", '" & current & "' As adddatetime"
        strSql = strSql & " ,CURRENT_TIMESTAMP As sysDate"
        strSql = strSql & ", '" & p_strUserCdLogin & "' As addjusercd"
        'strSql = strSql & ", Null As upddatetime"
        strSql = strSql & ", Null As updusercd"
        strSql = strSql & ", '" & p_strTerminalCdLogin & "' As terminalcd"
        strSql = strSql & " FROM "
        strSql = strSql & "      w_family"
        strSql = strSql & " WHERE "
        strSql = strSql & "      companybranchno IS NOT NULL"
        strSql = strSql & "    and seq = " & in_intImportSeq & ")"

        strSql = strSql & " UNION ALL"

        strSql = strSql & " (select"
        strSql = strSql & "  COMPANYCD As companycd"
        strSql = strSql & ", cast(COMPANYBRANCHNO as integer) As companybranchno"
        strSql = strSql & ", STAFFCD As staffcd"
        strSql = strSql & ", (select max(familyno) from t_familyresult where companycd = w_family.companycd and companybranchno = cast(w_family.companybranchno As integer) and staffcd = w_family.staffcd) + row_number() OVER (partition by w_family.staffcd order by w_family.line) As familyno"
        strSql = strSql & ", 3 As resultflg"
        strSql = strSql & ", FAMILYNAME As familyname"
        strSql = strSql & ", FAMILYNAMEKANA As familynamekana"
        strSql = strSql & ", to_timestamp(BIRTHDATE,'yyyy/mm/dd') As birthdate"
        strSql = strSql & ", CASE when SEX != '' THEN cast(SEX as integer) ELSE Null END As sex"
        strSql = strSql & ", Null As privatenokey"
        strSql = strSql & ", 0 As familyaddflg"
        strSql = strSql & ", 0 As unofferflg"
        strSql = strSql & ", 0 As insuredflg"
        strSql = strSql & ", 0 As delflg"
        'strSql = strSql & ", NULL As deldatetime"
        strSql = strSql & ", Null "
        'strSql = strSql & ", '" & current & "' As adddatetime"
        strSql = strSql & " ,CURRENT_TIMESTAMP As sysDate"
        strSql = strSql & ", '" & p_strUserCdLogin & "' As addjusercd"
        'strSql = strSql & ", Null As upddatetime"
        strSql = strSql & ", Null As updusercd"
        strSql = strSql & ", '" & p_strTerminalCdLogin & "' As terminalcd"
        strSql = strSql & " FROM "
        strSql = strSql & "      w_family"
        strSql = strSql & " WHERE "
        strSql = strSql & "      companybranchno IS NOT NULL"
        strSql = strSql & "    and seq = " & in_intImportSeq & ")"

        inout_mynoContext.Database.ExecuteSqlCommand(strSql)

    End Sub
End Class

Public Class DataImportLogGridItem
    Public Property seq As String
    Public Property dataImpDateTime As Date?
    Public Property dataImpUserCD As String
    Public Property dataFileName As String
    Public Property dataCompanyCD As String
    Public Property dataCompanyBranchNo As Integer?
    Public Property importType As String
    Public Property fileType As String
    Public Property dataResult As String
    Public Property dataError As String
End Class
Public Class ItemListImport
    Public Property Seq As Integer
    Public Property CompanyCd As String
    Public Property CompanyBranchNo As Integer
    Public Property CompanyName As String
    Public Property CompanyBranchName As String
    Public Property ImpDatetime As DateTime
    Public Property Line As Integer?
    Public Property Message As String
    Public Property RecData As String
    Public Property ColData As String
    Public Property DetailSeq As String
End Class
Public Class FlgPrintModel
    Public Property CompanyCd As String
    Public Property CompanyBranchNo As Integer
    Public Property StaffCd As String
    Public Property PrintFlg As String
End Class
'Public Class LstDetailLog
'    Public Property tempDetailSeq As Integer
'    Public Property tempLine As Integer
'    Public Property tempMsg As String
'    Public Property tempCsvData As String
'    Public Property tempColData As String
'End Class