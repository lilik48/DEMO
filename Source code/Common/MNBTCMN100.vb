'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNBTCMN100
'*  機能名称：共通処理
'*  処理　　：共通処理（ＢＬ）
'*  内容　　：共通処理のビジネスロジック
'*  ファイル：MNBTCMN100.vb
'*  備考　　：
'*
'*  Created：2015/06/25 RS. Pham Van Map
'***************************************************************************************
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Security.Cryptography
Imports System.Reflection
Imports Npgsql
Imports System.Runtime.InteropServices


Namespace Common
    Public Class MNBTCMN100

#Region "Const"
        <DllImport("User32.dll")> _
        Public Shared Function RegisterHotKey(ByVal hwnd As IntPtr, _
                        ByVal id As Integer, ByVal fsModifiers As Integer, _
                        ByVal vk As Integer) As Integer
        End Function
        <DllImport("User32.dll")> _
        Public Shared Function UnregisterHotKey(ByVal hwnd As IntPtr, _
                        ByVal id As Integer) As Integer
        End Function

        Public Const KEY_ALT As Integer = &H1
        Public Const _HOTKEY As Integer = &H312

        Public Const CST_IniFile As String = "message.ini"
        Public Const CST_RegexAlphaNumeric As String = "^[0-9a-zA-Z!""#$%&'()*+,-./:;<=>?@\\\^_`{|}~\[\]]+$"
        Public Const CST_RegexLetterAndNumber As String = "^[0-9a-zA-Z]+$"
        Public Const CST_RegexLess2Bit As String = "^.{1,2}$"
        Public Const CST_RegexLess3Bit As String = "^.{1,3}$"
        Public Const CST_RegexLess8Bit As String = "^.{1,8}$"
        Public Const CST_RegexLess80Bit As String = "^.{1,80}$"
        Public Const CST_RegexLess13Bit As String = "^.{1,13}$"
        Public Const CST_RegexLess50Bit As String = "^.{1,50}$"
        Public Const CST_RegexLess30Bit As String = "^.{1,30}$"
        Public Const CST_RegexLess20Bit As String = "^.{1,20}$"
        Public Const CST_RegexLess5Bit As String = "^.{1,5}$"
        Public Const CST_RegexLess10Bit As String = "^.{1,10}$"
        Public Const CST_RegexLess5Word As String = "^\w{1,5}$"
        Public Const CST_RegexLess20Word As String = "^\w{1,20}$"
        Public Const CST_RegexLetter As String = "^[a-zA-Z]+$"
        Public Const CST_RegexSpecial As String = "^[!""#$%&'()*+,-./:;<=>?@\\\^_`{|}~\[\]]+$"
        Public Const CST_RegexNumber As String = "^[0-9]+$"
        Public Const CST_RegexLess3degitNumber As String = "^\d{1,3}$"
        Public Const CST_RegexHourType As String = "^(([0-9]?[0-9])|(9][0-9])):([0-9]?[0-9])?$"
        Public Const CST_RegexHourRight As String = "^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])?$"
        Public Const CST_RegexDate As String = "^\d{4}/\d{2}/\d{2}$"
        Public Const CST_RegexDate2 As String = "^\d{4}/\d{1}/\d{1}$"
        Public Const CST_RegexDate3 As String = "^\d{4}/\d{1}/\d{2}$"
        Public Const CST_RegexDate4 As String = "^\d{4}/\d{2}/\d{1}$"
        Public Const CST_RegexBarcode As String = "^[\w-]{5}-\d{1,}-[\w-]{1,}$" 'Update 12/09/2015 '"^\w{5}-\d{1,}-[\w-]{1,}$" '"^\w{5}-\d{1,}-\w{1,}$"
        Public Const CST_RegexKanji As String = "^[一-龠\s]+$"
        Public Const CST_RegexKanjiLess30Byte As String = "^[一-龠\s]{1,30}$"
        Public Const CST_RegexKataKanaFullsize As String = "^[ァ-ヴー\s]+$" '"^[ァ-ヴー\s]+$"
        Public Const CST_RegexKataKanaFullsizeLess30Byte As String = "^[ァ-ヴー\s]{1,30}$" '"^[ァ-ヴー\s]{1,30}$"
        Public Const CST_RegexHiragana As String = "^[ぁ-ゔ\s]+$"
        Public Const CST_RegexKataKanaHalfsize As String = "^[0-9a-zA-Z!""#$%&'()*+,-./:;<=>?@\\\^_`{|}~\[\]ｧ-ﾝﾞﾟｦ-ﾟ\s]+$" '"^[ｧ-ﾝﾞﾟ\s]+$"
        Public Const CST_RegexKataKanaHalfsizeLess30Byte As String = "^[0-9a-zA-Z!""#$%&'()*+,-./:;<=>?@\\\^_`{|}~\[\]ｧ-ﾝﾞﾟｦ-ﾟ\s]{1,30}$" '"^[ｧ-ﾝﾞﾟ\s]{1,30}$"
        Public Const CST_RegexCompanyBranchNo As String = "^[0-9,]+$"
        Public Const CST_RegexCompanyBranchNoIsNumber As String = "^\d{1,},\d{1,},\d{1,}$"
        Public Const CST_RegexCompanyCd As String = "^[0-9A-Z\-]+$" '"^[0-9A-Z\-.$\/+% ]+$" 'update redmine 34023
        Public Const CST_KeyHash As String = "thongth"
        'Check input date into cell format yy/MM/dd or yyMMdd
        Public Const CST_RegexDateInCell As String = "^\d{2}\/{1}\d{2}\/{1}\d{2}$|^[0-9]{6}$"
        Public Const CST_RegexInputDate As String = "^[\d\/]+$"

        Public Shared plastControl As Control
#End Region

#Region "Input Log"
        Public Shared p_intSeq1st As Long = 0L
        Public Shared p_intSeq2nd As Long = 0L
        Public Shared p_intSeqDetail As Long = 0L

        ''' <summary>
        ''' Input Log to T_SYSTEMLOG
        ''' <author>ThangNB</author>
        ''' <updatedate>2015/06/30</updatedate>
        ''' <content>
        ''' - Add ByRef context
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function InputLogMaster(ByRef inout_context As mynoEntities, ByVal in_strEventType As String,
                                              ByVal in_strAppCd As String, ByVal in_strCompanyCd As String,
                                              ByVal in_intBranchCd As Integer) As Long

            Dim tSystemLog As t_systemlog = New t_systemlog()

            ' Add eventType param to object T_SYSTEMLOG
            If Not in_strEventType Is Nothing AndAlso Not String.IsNullOrEmpty(in_strEventType) Then
                tSystemLog.eventtype = in_strEventType
            Else
                tSystemLog.eventtype = Nothing
            End If

            ' Add appCd param to object T_SYSTEMLOG
            If Not in_strAppCd Is Nothing AndAlso Not String.IsNullOrEmpty(in_strAppCd) Then
                tSystemLog.appcd = in_strAppCd
            Else
                tSystemLog.appcd = Nothing
            End If

            ' Add companyCd param to object T_SYSTEMLOG
            If Not in_strCompanyCd Is Nothing AndAlso Not String.IsNullOrEmpty(in_strCompanyCd) Then
                tSystemLog.companycd = in_strCompanyCd
            Else
                tSystemLog.companycd = Nothing
            End If

            ' Add branchCd param to object T_SYSTEMLOG
            If in_intBranchCd <> Nothing And in_intBranchCd > 0 Then
                tSystemLog.companybranchno = in_intBranchCd
            Else
                tSystemLog.companybranchno = Nothing
            End If

            ' Add time system to object T_SYSTEMLOG
            tSystemLog.adddatetime = GetCurrentTimestamp(inout_context)

            ' Add coder user login to object T_SYSTEMLOG
            tSystemLog.addjusercd = p_strUserCdLogin
            ' Add name pc login to object T_SYSTEMLOG
            tSystemLog.terminalcd = p_strTerminalCdLogin
            Try
                ' Add object to entities
                inout_context.t_systemlog.Add(tSystemLog)

                If inout_context.SaveChanges() Then
                    InputLogMaster = tSystemLog.seq
                    p_intSeq1st = tSystemLog.seq
                End If
                Return InputLogMaster
            Catch ex As Exception
                p_intSeq1st = 0L
                ShowMessageException()
                InputLogMaster = -1
            End Try
        End Function

        ''' <summary>
        ''' Input Log to T_SYSTEMDETAILLOG
        ''' <author>ThangNB</author>
        ''' <updatedate>2015/06/30</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub InputLogDetail(ByRef context As mynoEntities, ByVal seq As Long, ByVal staffCd As String,
                                         ByVal message As String, ByVal data As String)

            Dim tSystemDetailLog As t_systemdetaillog = New t_systemdetaillog()

            ' Add SEQ param to object T_SYSTEMDETAILLOG
            tSystemDetailLog.seq = seq
            'auto increment detail seq
            If p_intSeq1st > 0L AndAlso p_intSeq2nd = p_intSeq1st Then
                p_intSeqDetail = p_intSeqDetail + 1L
            ElseIf p_intSeq2nd <> p_intSeq1st Then
                p_intSeqDetail = 1L
            End If
            tSystemDetailLog.detailseq = p_intSeqDetail
            p_intSeq2nd = seq
            'end auto increment detail seq

            ' Add staffCd param to object T_SYSTEMDETAILLOG
            If Not staffCd Is Nothing AndAlso Not String.IsNullOrEmpty(staffCd) Then
                tSystemDetailLog.staffcd = staffCd
            Else
                tSystemDetailLog.staffcd = Nothing
            End If

            ' Add message param to object T_SYSTEMDETAILLOG
            If Not message Is Nothing AndAlso Not String.IsNullOrEmpty(message) Then
                tSystemDetailLog.message = message
            Else
                tSystemDetailLog.message = Nothing
            End If

            ' Add data param to object T_SYSTEMDETAILLOG
            If Not data Is Nothing AndAlso Not String.IsNullOrEmpty(data) Then
                tSystemDetailLog.data = data
            Else
                tSystemDetailLog.data = Nothing
            End If

            ' Add datetime to object T_SYSTEMDETAILLOG
            tSystemDetailLog.adddatetime = GetCurrentTimestamp(context)

            ' Add coder user login to object T_SYSTEMDETAILLOG
            tSystemDetailLog.addjusercd = p_strUserCdLogin
            ' Add name pc login to object T_SYSTEMDETAILLOG
            tSystemDetailLog.terminalcd = p_strTerminalCdLogin
            Try

                ' Add object to entities
                context.t_systemdetaillog.Add(tSystemDetailLog)
                context.SaveChanges()

            Catch ex As Exception
                ShowMessageException()
            End Try
        End Sub

#End Region

#Region "Message"

        ''' <summary>
        ''' Show message common
        ''' <author>ThangNB</author>
        ''' <updatedate>2015/06/26</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function ShowMessage(in_strMsgKey As String, in_strParamOne As String, in_strParamTwo As String,
                                           in_strParamThree As String) As Integer

            Try
                ShowMessage = 0
                ' Get contents message by key
                Dim value As String = GetMessageFromIni(in_strMsgKey)
                ' Check value after get
                If value <> String.Empty Then

                    If in_strMsgKey.Length > 5 Then
                        value = value.Replace("/n", vbCrLf)
                        Dim subStringType As String = in_strMsgKey.Substring(5, 1)
                        'Set param to message
                        If in_strParamOne <> String.Empty And in_strParamTwo <> String.Empty And in_strParamThree <> String.Empty Then
                            value = value.Replace("{0}", in_strParamOne).Replace("{1}", in_strParamTwo).Replace("{2}", in_strParamThree)
                        ElseIf in_strParamOne <> String.Empty And in_strParamTwo <> String.Empty And in_strParamThree.Equals(String.Empty) _
                            Then
                            value = value.Replace("{0}", in_strParamOne).Replace("{1}", in_strParamTwo)
                        ElseIf _
                            in_strParamOne <> String.Empty And in_strParamTwo.Equals(String.Empty) And
                            in_strParamThree.Equals(String.Empty) Then
                            value = value.Replace("{0}", in_strParamOne)
                        End If
                        Dim questionMark As Boolean = value.Contains("？")

                        'Set icon and button in messagebox
                        If subStringType.Equals("I") AndAlso questionMark Then
                            ShowMessage = MessageBox.Show(value, "", MessageBoxButtons.OKCancel,
                                                          MessageBoxIcon.Question)
                        ElseIf subStringType.Equals("I") Then
                            ShowMessage = MessageBox.Show(value, "Thông tin", MessageBoxButtons.OK,
                                                          MessageBoxIcon.Information)
                        ElseIf subStringType.Equals("E") Then
                            ShowMessage = MessageBox.Show(value, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        ElseIf subStringType.Equals("W") Then
                            ShowMessage = MessageBox.Show(value, "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                        End If
                    End If
                Else
                    Throw New Exception("Không thể tìm " & in_strMsgKey & " trong file messsage.ini")
                End If
                Return ShowMessage
            Catch ex As Exception
                ShowMessageException()
                'Set to Click Cancel
                Return 2
            End Try
        End Function

        ''' <summary>
        ''' Show message common - type: confirm
        ''' <author>MapPV</author>
        ''' <updatedate>2015/06/30</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function ShowMessageConfirm(in_strMsgKey As String, paramOne As String, paramTwo As String,
                                                  paramThree As String, Optional default_Button As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1) As Integer

            Try
                ShowMessageConfirm = 0
                ' Get contents message by key
                Dim value As String = GetMessageFromIni(in_strMsgKey)
                ' Replace /n by New line
                value = value.Replace("/n", vbCrLf)
                ' Check value after get
                If value <> String.Empty Then

                    If in_strMsgKey.Length > 5 Then

                        Dim subStringType As String = in_strMsgKey.Substring(5, 1)
                        'Set param to message
                        If paramOne <> String.Empty And paramTwo <> String.Empty And paramThree <> String.Empty Then
                            value = value.Replace("{0}", paramOne).Replace("{1}", paramTwo).Replace("{2}", paramThree)
                        ElseIf paramOne <> String.Empty And paramTwo <> String.Empty And paramThree.Equals(String.Empty) _
                            Then
                            value = value.Replace("{0}", paramOne).Replace("{1}", paramTwo)
                        ElseIf _
                            paramOne <> String.Empty And paramTwo.Equals(String.Empty) And
                            paramThree.Equals(String.Empty) Then
                            value = value.Replace("{0}", paramOne)
                        End If
                        'Dim questionMark As Boolean = value.Contains("？")

                        'Set icon and button in messagebox
                        If subStringType.Equals("I") Then
                            ShowMessageConfirm = MessageBox.Show(value, "", MessageBoxButtons.OKCancel,
                                                                 MessageBoxIcon.Question, defaultButton:=default_Button)
                        End If
                    End If
                Else
                    Throw New Exception("Cannot find " & in_strMsgKey & " in messsage.ini file")
                End If
                Return ShowMessageConfirm
            Catch ex As Exception
                ShowMessageException()
                Return 2
            End Try
        End Function

        ''' <summary>
        ''' Show message common - type: confirm, focus cancel
        ''' <author>MapPV</author>
        ''' <updatedate>2015/06/30</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function ShowMessageConfirmFocusCancel(in_strMsgKey As String, paramOne As String, paramTwo As String,
                                                  paramThree As String) As Integer

            Try
                ShowMessageConfirmFocusCancel = 0
                ' Get contents message by key
                Dim value As String = GetMessageFromIni(in_strMsgKey)
                ' Replace /n by New line
                value = value.Replace("/n", vbCrLf)
                ' Check value after get
                If value <> String.Empty Then

                    If in_strMsgKey.Length > 5 Then

                        Dim subStringType As String = in_strMsgKey.Substring(5, 1)
                        'Set param to message
                        If paramOne <> String.Empty And paramTwo <> String.Empty And paramThree <> String.Empty Then
                            value = value.Replace("{0}", paramOne).Replace("{1}", paramTwo).Replace("{2}", paramThree)
                        ElseIf paramOne <> String.Empty And paramTwo <> String.Empty And paramThree.Equals(String.Empty) _
                            Then
                            value = value.Replace("{0}", paramOne).Replace("{1}", paramTwo)
                        ElseIf _
                            paramOne <> String.Empty And paramTwo.Equals(String.Empty) And
                            paramThree.Equals(String.Empty) Then
                            value = value.Replace("{0}", paramOne)
                        End If
                        'Dim questionMark As Boolean = value.Contains("？")

                        'Set icon and button in messagebox
                        If subStringType.Equals("I") Then
                            ShowMessageConfirmFocusCancel = MessageBox.Show(value, "", MessageBoxButtons.OKCancel,
                                                                 MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                        End If
                    End If
                Else
                    Throw New Exception("Cannot find " & in_strMsgKey & " in messsage.ini file")
                End If
                Return ShowMessageConfirmFocusCancel
            Catch ex As Exception
                ShowMessageException()
                Return 2
            End Try
        End Function

        ''' <summary>
        ''' Get conten message from ini file
        ''' <author>MapPV</author>
        ''' <updatedate>2015/06/26</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Function GetMessageFromIni(ByVal key As String) As String
            Dim ret As String = ""
            Try
                Dim file As New StreamReader(CST_IniFile, Encoding.GetEncoding("shift_jis"))
                Dim iniLine As String = file.ReadLine()
                While Not iniLine Is Nothing
                    If InStr(iniLine, key, CompareMethod.Text) = 1 Then
                        ret = Trim(iniLine.Substring(InStr(iniLine, "=", CompareMethod.Text)))
                        Exit While
                    End If
                    iniLine = file.ReadLine()
                End While
            Catch ex As Exception
                ShowMessageException()
            End Try
            Return ret
        End Function

        ''' <summary>
        ''' Get message
        ''' <author>AnhND</author>
        ''' <updatedate>2015/07/03</updatedate>
        ''' <content>
        ''' get only string message, don't show dialog _ todo
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function GetMessageContent(msgKey As String, paramOne As String, paramTwo As String,
                                                 paramThree As String) As String

            Try
                ' Get contents message by key
                Dim value As String = GetMessageFromIni(msgKey)
                ' Check value after get
                If value <> String.Empty Then

                    'Set param to message
                    If paramOne <> String.Empty And paramTwo <> String.Empty And paramThree <> String.Empty Then
                        value = value.Replace("{0}", paramOne).Replace("{1}", paramTwo).Replace("{2}", paramThree)
                    ElseIf paramOne <> String.Empty And paramTwo <> String.Empty And paramThree.Equals(String.Empty) _
                        Then
                        value = value.Replace("{0}", paramOne).Replace("{1}", paramTwo)
                    ElseIf _
                        paramOne <> String.Empty And paramTwo.Equals(String.Empty) And paramThree.Equals(String.Empty) _
                        Then
                        value = value.Replace("{0}", paramOne)
                    End If
                End If
                Return value
            Catch ex As Exception
                ShowMessageException()
                Return String.Empty
            End Try
        End Function

        Public Shared Sub ShowMessageException()
            MessageBox.Show("Đã xảy ra lỗi " & Environment.NewLine &
                            "Mã lỗi：" & Err.Number & Environment.NewLine & "Nội dung：" &
                            Err.Description, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Sub

        Public Shared Function ShowMessageExceptionWithReturn() As Integer
            ShowMessageExceptionWithReturn = MessageBox.Show("Đã xảy ra lỗi" & Environment.NewLine &
                            "Mã lỗi：" & Err.Number & Environment.NewLine & "Nội dung：" &
                            Err.Description, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Function
#End Region

#Region "Valid"

        ''' <summary>
        ''' Check input date into cell degit or /
        ''' <author>ThienNQ</author>
        ''' <updatedate>2015/08/12</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidatInputDate(text As String) As Boolean
            If String.IsNullOrEmpty(text) Then Return False
            Dim regexCheck As New Regex(CST_RegexInputDate)
            If Not regexCheck.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check input date into cell format yy/MM/dd or yyMMdd
        ''' <author>ThienNQ</author>
        ''' <updatedate>2015/08/12</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateDateInCell(text As String) As Boolean
            If String.IsNullOrEmpty(text) Then Return False
            Dim regexCheck As New Regex(CST_RegexDateInCell)
            If Not regexCheck.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate Company BranchNo input text is number + "," like "123,123,123" 
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/16</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateCompanyBranchNoIsNumber(text As String) As Boolean
            Dim regexCheck As New Regex(CST_RegexCompanyBranchNoIsNumber)
            If Not regexCheck.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate Company BranchNo input text with number and , 
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/16</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateCompanyBranchNo(text As String) As Boolean
            Dim regexCheck As New Regex(CST_RegexCompanyBranchNo)
            If Not regexCheck.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate Japanese Calendar format
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateJapaneseCalendar(text As String) As Boolean
            Try
                Dim culture As New CultureInfo("ja-JP", True)
                culture.DateTimeFormat.Calendar = New JapaneseCalendar()
                Dim target As String = text
                Dim result As DateTime = DateTime.ParseExact(target, "ggyy年M月d日", culture)
                Return False
            Catch ex As Exception
                Return True
            End Try
        End Function

        ''' <summary>
        ''' convert Japanese Calendar -> world calendar format
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function ConvertJapaneseToWorldCalendar(text As String) As String
            Try
                Dim culture As New CultureInfo("ja-JP", True)
                culture.DateTimeFormat.Calendar = New JapaneseCalendar()
                Dim target As String = text
                Dim result As DateTime = DateTime.ParseExact(target, "ggyy年MM月dd日", culture)
                Return result
            Catch ex As Exception
                Return ""
            End Try
        End Function

        Public Shared Function ConvertJapaneseToEN(text As String) As Nullable(Of DateTime)
            Try
                Dim culture As New CultureInfo("ja-JP", True)
                culture.DateTimeFormat.Calendar = New JapaneseCalendar()
                Dim target As String = text
                Dim result As DateTime = DateTime.ParseExact(target, "ggyy年MM月dd日", culture)
                Return result
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' convert to world Calendar -> japanes calendar format
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function ConvertWorldToJapaneseCalendar(text As Nullable(Of DateTime)) As String
            Try
                If text Is Nothing Then Return ""
                Dim culture As New CultureInfo("ja-JP", True)
                culture.DateTimeFormat.Calendar = New JapaneseCalendar()

                Dim target As New DateTime()
                target = text
                Dim result As String = target.ToString("ggyy年MM月dd日", culture)
                Return result
            Catch ex As Exception
                Return ""
            End Try
        End Function

        ''' <summary>
        ''' Check validate katakana input text (include "space", half size and less 30 byte)
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateOnlyKatakanaHalfsizeLess30byte(text As String) As Boolean
            Dim regexCheck As New Regex(CST_RegexKataKanaHalfsizeLess30Byte)
            If Not regexCheck.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function


        ''' <summary>
        ''' Check validate katakana input text (include "space",fullsize and less 30 byte)
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateOnlyKatakanaFullsizeLess30byte(text As String) As Boolean
            Dim regexCheck As New Regex(CST_RegexKataKanaFullsizeLess30Byte)
            If Not regexCheck.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate katakana input text (include "space" and half size)
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateOnlyKatakanaHalfsize(text As String) As Boolean
            Dim regexCheck As New Regex(CST_RegexKataKanaHalfsize)
            If Not regexCheck.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate hiragana input text (include "space")
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateOnlyHiragana(text As String) As Boolean
            Dim regexCheck As New Regex(CST_RegexHiragana)
            If Not regexCheck.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate katakana input text (include "space" and fullsize)
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateOnlyKatakanaFullsize(text As String) As Boolean
            Dim regexCheck As New Regex(CST_RegexKataKanaFullsize)
            If Not regexCheck.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate kanji Less 30 Byte input text (include "space")
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateOnlyKanjiLess30Byte(text As String) As Boolean
            Dim regexCheck As New Regex(CST_RegexKanjiLess30Byte)
            If Not regexCheck.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate kanji input text (include "space")
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateOnlyKanji(text As String) As Boolean
            Dim regexCheck As New Regex(CST_RegexKanji)
            If Not regexCheck.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check External Character
        ''' <author>MapPV</author>
        ''' <updatedate>2015/06/30</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckExternalCharacter(character As String) As Boolean
            Try
                'Check character null or empty
                If character = Nothing Or character = String.Empty Then
                    CheckExternalCharacter = False
                Else
                    'Declare【ｼﾌﾄJISｺｰﾄﾞ：8540～889E】 
                    Dim fromIntOne As Integer = Convert.ToInt32("8540", 16)
                    Dim toIntOne As Integer = Convert.ToInt32("889E", 16)
                    'Declare【ｼﾌﾄJISｺｰﾄﾞ：EB40～EFFC】
                    Dim fromIntTwo As Integer = Convert.ToInt32("EB40", 16)
                    Dim toIntTwo As Integer = Convert.ToInt32("EFFC", 16)
                    'Declare【ｼﾌﾄJISｺｰﾄﾞ：F040～ FFFF】
                    Dim fromIntThree As Integer = Convert.ToInt32("F040", 16)
                    Dim toIntThree As Integer = Convert.ToInt32("FFFF", 16)
                    'Declare count character special
                    Dim count As Integer = 0
                    Dim rtn As String
                    'Create a character array. 
                    For value As Integer = 0 To character.Length - 1
                        ' Exit condition if the value is three.
                        Dim chars() As Char = character(value).ToString.ToCharArray
                        ' Encode the array of characters.
                        Dim sjisBytes() As Byte = Encoding.GetEncoding("shift_jis").GetBytes(chars)
                        rtn = String.Empty
                        For Each sjisByte In sjisBytes
                            rtn += String.Format("{0:X1}", sjisByte)
                            'Console.Write("{0:X1} ", sjisByte)
                        Next
                        Dim valueInt = Convert.ToInt32(rtn, 16)
                        'Check valid or invalid
                        If valueInt > fromIntOne AndAlso valueInt < toIntOne Then
                            count = count + 1
                        ElseIf valueInt > fromIntTwo AndAlso valueInt < toIntTwo Then
                            count = count + 1
                        ElseIf valueInt > fromIntThree AndAlso valueInt < toIntThree Then
                            count = count + 1
                        End If
                    Next

                    If count > 0 Then
                        'CheckExternalCharacter = True
                        CheckExternalCharacter = False
                    Else
                        CheckExternalCharacter = False
                    End If
                End If

            Catch ex As Exception
                MNBTCMN100.ShowMessageException()
                CheckExternalCharacter = Nothing
            End Try
        End Function

        ''' <summary>
        ''' Check validate Only Letter
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateOnlyLeter(text As String) As Boolean
            Dim strSurname As String = "^[a-zA-Z\s]+$"
            Dim reSurname As New Regex(strSurname)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate Letter or Number
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateLetterOrNumber(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLetterAndNumber)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' check textbox have less than 20 input charater
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidaLess20BitInput(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLess20Bit)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function
        ''' <summary>
        ''' check textbox have less than 30 input charater
        ''' <author>ThangNB</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidaLess30BitInput(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLess30Bit)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function
        ''' <summary>
        ''' check textbox have less than 2 input charater
        ''' <author>ThangNB</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidaLess2BitInput(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLess2Bit)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function
        ''' <summary>
        ''' check textbox have less than 3 input charater
        ''' <author>ThangNB</author>
        ''' <updatedate>2015/07/20</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidaLess3BitInput(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLess3Bit)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function
        ''' <summary>
        ''' check textbox have less than 8 input charater
        ''' <author>ThangNB</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidaLess8BitInput(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLess8Bit)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function
        ''' <summary>
        ''' check textbox have less than 80 input charater
        ''' <author>ThangNB</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidaLess80BitInput(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLess80Bit)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function
        ''' <summary>
        ''' check textbox have less than 13 input charater
        ''' <author>ThangNB</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidaLess13BitInput(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLess13Bit)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function
        ''' <summary>
        ''' check textbox have less than 50 input charater
        ''' <author>ThangNB</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidaLess50BitInput(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLess50Bit)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate with textbox only number
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateOnlyNumber(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexNumber)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate textbox with 3 digit only number
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidate3Digit(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLess3degitNumber)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate with textbox 5 any word or number
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidate5Word(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLess5Word)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate with textbox 5 any word or number
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidaLess5BitInput(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLess5Bit)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate with textbox 10 any word or number
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidaLess10BitInput(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLess10Bit)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate with textbox 20 any word or number
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidate20Word(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexLess20Word)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' ''' <summary>
        ''' Check validate Alphanumeric
        ''' </summary>
        ''' <param name="text">Text input</param>
        ''' <remarks></remarks>
        Public Shared Function CheckValidAlphanumeric(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexAlphaNumeric)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check valid barcode
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/01</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks>
        ''' check validate barcode have type like 'aaa111-123-aaa222'
        ''' todo
        ''' </remarks>
        Public Shared Function CheckValidateBarcode(_Barcode As String) As Boolean
            Dim reDate As New Regex(CST_RegexBarcode)
            If Not reDate.IsMatch(_Barcode) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Check validate with Hours type is "HH:MM"
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateHoursType(_Hours As String) As Integer
            Dim reHoursType As New Regex(CST_RegexHourType)
            Dim reHoursRight As New Regex(CST_RegexHourRight)

            If Not reHoursType.IsMatch(_Hours) Then
                ' not right format HH:MMで
                Return 2
            ElseIf Not reHoursRight.IsMatch(_Hours) Then
                ' Hours that does not exist like 90:61
                Return 1
            Else : Return 0
            End If
        End Function

        ''' <summary>
        ''' check datetime type is "yyyy/MM/dd" and exist
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateDateType(_Date As String) As Integer
            Dim reDate As New Regex(CST_RegexDate)
            If reDate.IsMatch(_Date) Then
                Try
                    Dim checkdate As DateTime = DateTime.ParseExact(_Date, "yyyy/MM/dd", CultureInfo.InvariantCulture)
                    Return 0
                Catch ex As Exception
                    ' Date that does not exist like 2015/02/31
                    Return 1
                End Try
            Else
                ' not right format YYYY/MM/DDで
                Return 2
            End If
        End Function

        ''' <summary>
        ''' check datetime type is "yyyy/MM/dd"; "yyyy/M/d" and exist
        ''' <author>ThangNB</author>
        ''' <updatedate>2015/07/27</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateDateMulType(_Date As String) As Integer
            Dim reDate As New Regex(CST_RegexDate)
            Dim reDate1 As New Regex(CST_RegexDate2)
            Dim reDate2 As New Regex(CST_RegexDate3)
            Dim reDate3 As New Regex(CST_RegexDate4)
            If reDate.IsMatch(_Date) Or reDate1.IsMatch(_Date) Or reDate2.IsMatch(_Date) Or reDate3.IsMatch(_Date) Then
                Try
                    Dim thisDt As DateTime
                    Dim formats() As String = {"yyyy/MM/dd", "yyyy/M/d", "yyyy/M/dd", "yyyy/MM/d"}
                    If DateTime.TryParseExact(_Date, formats, Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, thisDt) Then
                        Return 0
                    Else
                        Return 1
                    End If
                Catch ex As Exception
                    ' Date that does not exist like 2015/02/31
                    Return 1
                End Try
            Else
                ' not right format YYYY/MM/DDで
                Return 2
            End If
        End Function

        ''' <summary>
        ''' Compare start date and end date
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CheckStartDateAndEndDate(_StartDate As String, _EndDate As String) As Boolean
            Try
                Dim startDate As DateTime = CDate(_StartDate)
                Dim endDate As DateTime = CDate(_EndDate)

                If DateTime.Compare(startDate, endDate) <= 0 Then
                    Return False
                Else : Return True
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        ''' <summary>
        ''' Enter the number of digits check
        ''' </summary>
        ''' <author>ThongTH</author>
        ''' <updatedate>2015/07/29</updatedate>
        ''' <param name="text">text input</param>
        ''' <param name="length">max length</param>
        ''' <returns>false: number byte of text input bigger than more max length of text, true:else</returns>
        ''' <remarks></remarks>
        Public Shared Function checkLength(ByVal text As String, ByVal length As Integer) As Boolean
            'The conversion as Shift JIS to byte array
            Dim bytesData = System.Text.Encoding.GetEncoding(932).GetBytes(text)
            'compare bytesData with maxlength
            If bytesData.Length > length Then
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Check directory exists
        '''  <author>ThongTH</author>
        ''' <updatedate>2015/07/29</updatedate>
        ''' </summary>
        ''' <param name="directory">true: exists, false: not exist</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckDirectoryExists(directory As String) As Boolean
            If My.Computer.FileSystem.DirectoryExists(directory) Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Check the soundness of MyNumber
        ''' </summary>
        ''' <param name="in_strMyNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckValidateMyNumber(ByVal in_strMyNumber As String) As Boolean
            CheckValidateMyNumber = False
            If MNBTCMN100.CheckValidateOnlyNumber(in_strMyNumber) Or in_strMyNumber.Length <> 12 Then Exit Function
            Try
                Dim arrStr As Char() = StrReverse(in_strMyNumber).ToCharArray

                Dim sum As Integer = 0
                For i = 0 To arrStr.Length - 1
                    If i >= 1 And i <= 6 Then
                        sum += (i + 1) * CInt(arrStr(i).ToString)
                    ElseIf i >= 7 And i <= arrStr.Count - 1 Then
                        sum += (i - 5) * CInt(arrStr(i).ToString)
                    End If
                Next
                Dim intLeftLogic = arrStr.Count - 1 - (sum Mod (arrStr.Count - 1))
                If intLeftLogic >= 10 Then
                    intLeftLogic = 0
                End If
                If intLeftLogic = CInt(arrStr(0).ToString) Then
                    CheckValidateMyNumber = True
                End If
            Catch ex As Exception
            End Try
        End Function

        'temp
        ''' <summary>
        ''' Check the soundness of MyNumber
        ''' </summary>
        ''' <param name="in_strMyNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GenMyNumber(ByVal in_strMyNumber As String) As String
            GenMyNumber = "-1"
            If MNBTCMN100.CheckValidateOnlyNumber(in_strMyNumber) Or in_strMyNumber.Length <> 12 Then Exit Function
            Try
                Dim arrStr As Char() = StrReverse(in_strMyNumber).ToCharArray

                Dim sum As Integer
                For i = 0 To arrStr.Length - 1
                    If i >= 1 And i <= 6 Then
                        sum += (i + 1) * CInt(arrStr(i).ToString)
                    ElseIf i >= 7 And i <= arrStr.Count - 1 Then
                        sum += (i - 5) * CInt(arrStr(i).ToString)
                    End If
                Next
                Dim intLeftLogic = 11 - (sum Mod 11)
                If sum Mod (arrStr.Count - 1) <= 1 Then
                    intLeftLogic = 0
                    'If (CInt(arrStr(0).ToString) = 0 OrElse CInt(arrStr(0).ToString) = 1) Then
                    '    GenMyNumber = True
                    'End If
                End If
                GenMyNumber = intLeftLogic.ToString
                System.Console.WriteLine(in_strMyNumber.Substring(0, in_strMyNumber.Length - 1) & GenMyNumber)
                'MessageBox.Show(in_strMyNumber & GenMyNumber)
                'GenMyNumber = intLeftLogic.ToString


            Catch ex As Exception
            End Try
        End Function

        ''' <summary>
        ''' Count digit in strings
        ''' <author>ThangNB</author>
        ''' <updatedate>2015/07/13</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CountDigitInString(showCharacter As String) As Integer
            Try
                Dim resultCount As Integer = 0
                For i As Integer = 0 To showCharacter.Length - 1
                    If (Regex.IsMatch(showCharacter(i), CST_RegexNumber)) Then
                        resultCount = resultCount + 1
                    End If
                Next
                Return resultCount
            Catch ex As Exception
                Return -1
            End Try
        End Function

        ''' <summary>
        ''' Compare code
        ''' <author>MapPV</author>
        ''' <updatedate>2015/08/03</updatedate>
        ''' <content>
        ''' If start > end => return false
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CompareCode(ByVal strStart As String, ByVal strEnd As String) As Boolean
            Try
                If Not CheckValidateOnlyNumber(strStart) AndAlso Not CheckValidateOnlyNumber(strEnd) Then
                    If (CDec(strStart) >= CDec(strEnd)) Then
                        Return False
                    End If
                Else
                    If String.Compare(strStart, strEnd, True) > 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End If
                Return True
            Catch ex As Exception
                If String.Compare(strStart, strEnd, True) > 0 Then
                    Return False
                Else
                    Return True
                End If
            End Try

        End Function

        ''' ''' <summary>
        ''' Check validate CompanyCD insert
        ''' </summary>
        ''' <param name="text">Text input</param>
        ''' <remarks></remarks>
        Public Shared Function CheckValidCompanyCdOrStaffCd(text As String) As Boolean
            Dim reSurname As New Regex(CST_RegexCompanyCd)
            If Not reSurname.IsMatch(text) Then
                Return True
            Else : Return False
            End If
        End Function

#End Region

#Region "Encrypt-Decrypt"

        ''' <summary>
        ''' Encrypt Sha256
        ''' <author>MapPV</author>
        ''' <updatedate>2015/06/30</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function EncryptSha256(ByVal rawString As String) As String
            Try
                Dim crypt As SHA256 = New SHA256Managed()
                EncryptSha256 = String.Empty
                Dim crypto As Byte() = crypt.ComputeHash(Encoding.ASCII.GetBytes(rawString), 0,
                                                         Encoding.ASCII.GetByteCount(rawString))
                For Each bit As Byte In crypto
                    EncryptSha256 += bit.ToString("x2")
                Next
            Catch ex As Exception
                MNBTCMN100.ShowMessageException()
                Return ""
            End Try
        End Function

        ''' <summary>
        ''' Encrypt Aes
        ''' <author>MapPV</author>
        ''' <updatedate>2015/06/30</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function EncryptAes(ByVal input As String, ByVal pass As String) As String
            Dim aes As New RijndaelManaged
            Dim hashAes As New MD5CryptoServiceProvider
            Dim encrypted As String
            Try
                Dim hash(31) As Byte
                Dim temp As Byte() = hashAes.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pass))
                Array.Copy(temp, 0, hash, 0, 16)
                Array.Copy(temp, 0, hash, 15, 16)
                aes.Key = hash
                aes.Mode = CipherMode.ECB
                Dim desEncrypter As ICryptoTransform = aes.CreateEncryptor
                Dim buffer As Byte() = ASCIIEncoding.ASCII.GetBytes(input)
                encrypted = Convert.ToBase64String(desEncrypter.TransformFinalBlock(buffer, 0, buffer.Length))
                Return encrypted
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

        ''' <summary>
        ''' Decrypt Aes
        ''' <author>MapPV</author>
        ''' <updatedate>2015/06/30</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function DecryptAes(ByVal input As String, ByVal pass As String) As String

            Dim aes As New RijndaelManaged
            Dim hashAes As New MD5CryptoServiceProvider
            Dim decrypted As String
            Try
                Dim hash(31) As Byte
                Dim temp As Byte() = hashAes.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pass))
                Array.Copy(temp, 0, hash, 0, 16)
                Array.Copy(temp, 0, hash, 15, 16)
                aes.Key = hash
                aes.Mode = CipherMode.ECB
                Dim desDecrypter As ICryptoTransform = aes.CreateDecryptor
                Dim buffer As Byte() = Convert.FromBase64String(input)
                decrypted = ASCIIEncoding.ASCII.GetString(desDecrypter.TransformFinalBlock(buffer, 0, buffer.Length))
                Return decrypted
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

#End Region

#Region "Get-Set common info"

        ''' <summary>
        ''' GetAuthorityByApp
        ''' </summary>
        ''' <param name="appcd">appcd input</param>
        ''' <param name="authorityCd">authorityCd input</param>
        ''' <remarks></remarks>
        Public Shared Function GetAuthorityByApp(ByVal appcd As String, ByVal authorityCd As String) As m_appxauth
            Dim result As m_appxauth
            Dim context As New mynoEntities()
            Try
                result =
                    (From mAppxauth In context.m_appxauth
                        Where mAppxauth.appcd = appcd AndAlso mAppxauth.authoritycd = authorityCd
                        Select mAppxauth).SingleOrDefault()
                Return result
            Catch ex As Exception
                MNBTCMN100.ShowMessageException()
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' get Computer computer
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/01</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function GetCpuId() As String

            Return Environment.MachineName

        End Function

        ''' <summary>
        ''' Get title screen
        ''' <author>MapPV</author>
        ''' <updatedate>2015/07/01</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks>
        ''' todo
        ''' </remarks>
        Public Shared Function SetTitleScreen() As String
            Return "Import dữ liệu (Tên đăng nhập：" + p_strUserCdLogin + ")"
        End Function

        ''' <summary>
        ''' Get LogType for cboLogType in MNUIMTN500
        ''' <author>AnhPD</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function DictionaryLogType() As List(Of String)
            Dim dic As New Dictionary(Of String, String)
            dic.Add("全て", "全て")
            dic.Add("操作ログ", "操作ログ")
            dic.Add("更新ログ", "更新ログ")
            dic.Add("バッチログ", "バッチログ")
            Dim lst As New List(Of String)(dic.Keys)
            Return lst
        End Function

        ''' <summary>
        ''' Get config constant path save file generate xlsx
        ''' </summary>
        ''' <author>ThangNB</author>
        ''' <updatedate>2015/07/10</updatedate>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetConfig(ByVal key As String, ByVal iniFileName As String) As String
            Dim ret As String = ""
            Try
                Dim file As New StreamReader(iniFileName, Encoding.GetEncoding("shift_jis"))
                Dim iniLine As String = file.ReadLine()
                While Not iniLine Is Nothing
                    If InStr(iniLine, key, CompareMethod.Text) = 1 Then
                        ret = Trim(iniLine.Substring(InStr(iniLine, "=", CompareMethod.Text)))
                        Exit While
                    End If
                    iniLine = file.ReadLine()
                End While
                file.Close()
                Return ret
            Catch ex As Exception
                MNBTCMN100.ShowMessageException()
                Return ret
            End Try
        End Function

        ''' <summary>
        ''' get current time in database
        ''' <author>MapPV</author>
        ''' <updatedate>2015/06/26</updatedate>
        ''' <content>
        ''' </content>
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function GetCurrentTimestamp(ByRef context As mynoEntities) As DateTime
            Try
                GetCurrentTimestamp = context.Database.SqlQuery(Of DateTime)("select CURRENT_TIMESTAMP").First()
            Catch ex As Exception
                ShowMessageException()
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Creates a csv data format from object
        ''' </summary>
        ''' <param name="objData">object.</param>
        ''' <returns>string.</returns>
        Public Shared Function CreateCsvDataFromObject(objData As Object) As String
            If objData Is Nothing Then
                Return String.Empty
            End If

            Dim sbCsvData As New StringBuilder()
            Dim t As Type = objData.[GetType]()
            Dim pi As PropertyInfo() = t.GetProperties()

            For index As Integer = 0 To pi.Length - 1
                sbCsvData.Append(pi(index).GetValue(objData, Nothing))

                If index < pi.Length - 1 Then
                    sbCsvData.Append(",")
                End If
            Next

            Return sbCsvData.ToString()
        End Function

        ''' <summary>
        ''' Creates a csv data format from object
        ''' </summary>
        ''' <param name="objData">object.</param>
        ''' <returns>string.</returns>
        Public Shared Function CreateCsvDataImport(objData As Object) As String
            If objData Is Nothing Then
                Return String.Empty
            End If

            Dim sbCsvData As New StringBuilder()
            Dim t As Type = objData.[GetType]()
            Dim pi As PropertyInfo() = t.GetProperties()

            For index As Integer = 2 To pi.Length - 1
                sbCsvData.Append(pi(index).GetValue(objData, Nothing))

                If index < pi.Length - 1 Then
                    sbCsvData.Append(",")
                End If
            Next

            Return sbCsvData.ToString()
        End Function

        ''' <summary>
        ''' get SYS_GUID from DB
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetSysGuid(ByRef contextMyNo As mynoEntities) As String
            Try
                Dim k = contextMyNo.Database.SqlQuery(Of String)("select md5(clock_timestamp()::text||random()::text||nextval('seq_t_privateno'))").ToList()
                GetSysGuid = k(0)
            Catch ex As Exception
                Throw ex
            End Try
        End Function


        ''' <summary>
        ''' Get previous record
        ''' ThongTH
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="list"></param>
        ''' <param name="current"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetPrevious(Of T)(list As IEnumerable(Of T), current As T) As T
            Try
                Return list.TakeWhile(Function(x) Not x.Equals(current)).Last()
            Catch
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' If data is empty then set to NULL
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="current"></param>     
        ''' <remarks></remarks>
        Public Shared Sub SetEmptyToNothing(Of T)(ByRef current As T)
            Try
                Dim fields As FieldInfo() = current.[GetType]().GetFields(BindingFlags.[Public] Or BindingFlags.NonPublic Or BindingFlags.Instance)
                For Each field In fields
                    If String.IsNullOrEmpty(field.GetValue(current)) Then
                        field.SetValue(current, Nothing)
                    End If
                Next
            Catch
                MNBTCMN100.ShowMessageException()
            End Try
        End Sub

        Public Shared Function GetNext(Of T)(list As IEnumerable(Of T), current As T) As T
            Try
                Return list.SkipWhile(Function(x) Not x.Equals(current)).Skip(1).First()
            Catch
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Convert datatime to yyyy/MM/dd
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CnvDateYYYYMMDD(value As String) As String
            Try
                If String.IsNullOrEmpty(value) Then
                    Return ""
                End If
                Return CDate(value).ToString("yyyy/MM/dd")
            Catch ex As Exception
                Return ""
            End Try
        End Function

        ''' <summary>
        ''' Convert datatime to yyyy/MM/dd HH:MM
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CnvDateYYYYMMDDHHMM(value As Nullable(Of DateTime)) As String
            Try
                If value Is Nothing Then
                    Return ""
                End If
                Return String.Format("{0:yyyy/MM/dd HH:mm}", value)
            Catch ex As Exception
                Return ""
            End Try
        End Function

        ''' <summary>
        ''' Convert datatime to yyyy/MM/dd HH:MM:SS
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CnvDateYYYYMMDDHHMMSS(value As Nullable(Of DateTime)) As String
            Try
                If value Is Nothing Then
                    Return ""
                End If
                Return String.Format("{0:yyyy/MM/dd HH:mm:ss}", value)
            Catch ex As Exception
                Return ""
            End Try
        End Function

        ''' <summary>
        ''' Check value for checkbox
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CnvBlnDbNull(value As Nullable(Of Integer)) As Boolean
            Try
                If value Is Nothing Then
                    Return False
                End If
                Return value
            Catch ex As Exception
                Return False
            End Try
        End Function


        ''' <summary>
        ''' Convert to interger
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CnvIntegerDbNull(value As Object) As Nullable(Of Integer)
            Try
                If value Is Nothing Then
                    Return Nothing
                End If
                Return CInt(value)
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Convert value to string
        ''' </summary>
        ''' <param name="value">string type</param>
        ''' <returns>string</returns>
        ''' <remarks></remarks>
        Public Shared Function CnvDbNullToString(value As Object) As String
            If value Is Nothing Then
                Return ""
            Else
                Return value.ToString().Trim()
            End If
        End Function

        ''' <summary>
        ''' Convert datatime to yyyy/MM/dd
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CnvDateYYYYMMDD(value As Nullable(Of Date)) As String
            Try
                If value Is Nothing Then
                    Return ""
                End If
                Return CDate(value).ToString("yyyy/MM/dd")
            Catch ex As Exception
                Return ""
            End Try
        End Function

        ''' <summary>
        ''' Convert From yy/MM/dd to yy年MM月dd日
        ''' </summary>
        ''' <param name="in_strCalendar"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ConvertCalendarJPToWordFromCell(ByVal in_strCalendar As String) As String
            Try
                ConvertCalendarJPToWordFromCell = CDate(in_strCalendar).ToString("yy") + "年" + CDate(in_strCalendar).ToString("MM") + "月" + CDate(in_strCalendar).ToString("dd") + "日"
            Catch ex As Exception
                ConvertCalendarJPToWordFromCell = ""
            End Try
        End Function

        ''' <summary>
        ''' Convert From yy/MM/dd to yy年MM月dd日
        ''' </summary>
        ''' <param name="in_strCalendar"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function FormatCalendarJP(ByVal in_strCalendar As String) As String
            Try
                Dim calendar = in_strCalendar.Split("/")
                If calendar.Length < 3 Then
                    If in_strCalendar.Length = 6 Then
                        FormatCalendarJP = in_strCalendar.Substring(0, 2) + "年" + in_strCalendar.Substring(2, 2) + "月" + in_strCalendar.Substring(4, 2) + "日"
                        Return FormatCalendarJP
                    End If
                End If
                FormatCalendarJP = calendar(0) + "年" + calendar(1) + "月" + calendar(2) + "日"
            Catch ex As Exception
                FormatCalendarJP = ""
            End Try
        End Function

        ''' <summary>
        ''' Set name for export file, part company and branch code
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function NamedPartCompanyExport(ByVal in_strCommanyCd As String, ByVal in_strBranchCd As String) As String
            Dim strCompany As String = String.Empty
            Dim strBranch As String = String.Empty

            If String.IsNullOrEmpty(in_strCommanyCd.Trim) Or in_strCommanyCd.Contains(",") Then
                'Case companycd is empty or input multiple
                strCompany = "_multi"
            Else
                'Case companycd is input one
                strCompany = "_" & in_strCommanyCd
            End If

            If String.IsNullOrEmpty(in_strBranchCd.Trim) Or in_strBranchCd.Contains(",") Or in_strBranchCd.Equals("0") Then
                'case branch is empty or input multiple
                strBranch = "_mlt"
            Else
                'case branch is input one
                Dim pad As Char
                pad = "0"c
                strBranch = "_" & in_strBranchCd.PadLeft(3, pad)
                'strBranch = "_" & in_strBranchCd
            End If

            Return strCompany & strBranch
        End Function
#End Region

#Region "Method"
        ''' <summary>
        ''' Set event Keypress, keydown for combobox
        ''' </summary>
        ''' <param name="cbb">ComboBox</param>
        ''' <remarks></remarks>
        Public Shared Sub CbbInitEvent(ByRef cbb As ComboBox)
            RemoveHandler cbb.KeyPress, AddressOf cbb_KeyPress
            AddHandler cbb.KeyPress, AddressOf cbb_KeyPress

            RemoveHandler cbb.KeyDown, AddressOf cbb_KeyDown
            AddHandler cbb.KeyDown, AddressOf cbb_KeyDown

        End Sub

        Private Shared Sub cbb_KeyPress(sender As Object, e As KeyPressEventArgs)
            e.Handled = True
        End Sub

        Private Shared Sub cbb_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyData = DirectCast(Shortcut.CtrlC, Keys) Then
                Clipboard.SetData(DataFormats.Text, sender.SelectedText)
            ElseIf e.KeyData = Keys.Left OrElse e.KeyData = Keys.Right OrElse e.KeyData = Keys.Up OrElse e.KeyData = Keys.Down Then
            Else
                e.Handled = True
            End If
        End Sub
#End Region
#Region "frmLoading Progress"

        Public Sub setProgressMax(pMax As Integer, Optional pMessage As String = "")

            Debug.Print(pMessage & " = " & Now())

            frmLoading.ToolStripProgressBar1.Maximum = pMax
            frmLoading.ToolStripStatusLabel1.Text = pMessage

        End Sub

        Public Sub setProgress(pValue As Integer, Optional pInterval As Integer = 10)

            If pValue Mod pInterval <> 0 Then Exit Sub

            Dim strMax As String = ""

            If frmLoading.ToolStripProgressBar1.Visible = True Then
                strMax = " / " & frmLoading.ToolStripProgressBar1.Maximum
                frmLoading.ToolStripProgressBar1.Value = pValue
            End If

            Dim strMsg() As String = frmLoading.ToolStripStatusLabel1.Text.ToString.Split(" ")

            If strMsg Is Nothing OrElse strMsg.Count = 0 OrElse String.IsNullOrWhiteSpace(strMsg(0)) Then
                frmLoading.ToolStripStatusLabel1.Text = pValue & strMax
            Else
                frmLoading.ToolStripStatusLabel1.Text = strMsg(0) & " " & pValue & strMax
            End If

        End Sub
#End Region

    End Class
End Namespace