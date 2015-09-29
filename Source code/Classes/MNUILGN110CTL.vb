'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNUILGN110
'*  機能名称：パスワード変更処理
'*  処理　　：パスワード変更処理（ＢＬ）
'*  内容　　：パスワード変更処理のビジネスロジック
'*  ファイル：MNUILGN110CTL.vb
'*  備考　　：
'*
'*  Created：2015/06/26 RS. Pham Duc Anh
'***************************************************************************************
Imports System.Data.Entity
Imports System.Text.RegularExpressions
Imports MyNo.Common


Public Class MNUILGN110CTL
    ''' <summary>
    '''     Check validate with textbox only letter
    ''' </summary>
    ''' <param name="in_strUserCD">userCD</param>
    ''' <param name="in_strPasswordHistory">passwordHistory</param>
    ''' <param name="in_strPasswordNew">passwordNew</param>
    ''' <param name="is_strPasswordConfirm">passwordConfirm</param>
    ''' <remarks></remarks>
    Public Function CheckInput(ByVal in_strUserCD As String,
         ByVal in_strPasswordHistory As String,
         ByVal in_strPasswordNew As String,
         ByVal is_strPasswordConfirm As String
         ) As Boolean
        Try

            If String.IsNullOrEmpty(in_strUserCD) Then 'Check User code
                MNBTCMN100.ShowMessage("MSGVWE00001", "ユーザコード", "", "")
                Return False
            ElseIf String.IsNullOrEmpty(in_strPasswordHistory) Then 'Check password current
                MNBTCMN100.ShowMessage("MSGVWE00001", "現在パスワード", "", "")
                Return False
            ElseIf String.IsNullOrEmpty(in_strPasswordNew) Then 'Check password new
                MNBTCMN100.ShowMessage("MSGVWE00001", "新パスワード", "", "")
                Return False
            ElseIf in_strPasswordNew.Length < 8 OrElse in_strPasswordNew.Length > 20 Then
                MNBTCMN100.ShowMessage("MSGVWE00008", "新パスワード", "8", "20")
                Return False
            ElseIf CheckValidPasswordCharacter(in_strPasswordNew) = False Then
                MNBTCMN100.ShowMessage("MSGVWE00010", "新パスワード", "", "")
                Return False
            ElseIf String.IsNullOrEmpty(is_strPasswordConfirm) Then
                MNBTCMN100.ShowMessage("MSGVWE00001", "新パスワード確認", "", "")
                'Check password confirm
                Return False
            ElseIf is_strPasswordConfirm.Equals(in_strPasswordNew) = False Then
                MNBTCMN100.ShowMessage("MSGVWE00009", "", "", "")
                Return False
            Else

                Using contextMyNo As New mynoEntities
                    'Check password history
                    Dim clsMNUILGN100CTL As MNUILGN100CTL = New MNUILGN100CTL
                    Dim user As m_user =
                            (From e1 In contextMyNo.m_user Where e1.usercd = in_strUserCD Where e1.delflg = 0 Select e1) _
                            .
                            FirstOrDefault()
                    If user IsNot Nothing Then
                        If user.passwordflg = 1 Then ' Check one time password
                            If Not user.onetimepassword.Equals(in_strPasswordHistory) Then
                                MNBTCMN100.ShowMessage("MSGVWE00011", "", "", "")
                                Return False
                            End If
                        ElseIf user.passwordflg = 0 Then ' Check normal password
                            If Not MNBTCMN100.EncryptSha256(in_strPasswordHistory).Equals(user.password) Then
                                MNBTCMN100.ShowMessage("MSGVWE00011", "", "", "")
                                Return False
                            End If
                        End If
                    Else
                        MNBTCMN100.ShowMessage("MSGVWE00018", "ユーザコード", "", "")
                        Return False
                    End If
                    'End Check password history
                End Using
            End If
            Return True
        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21
            'MNBTCMN100.ShowMessageException() ' Update ThongTH 2015/09/21
            'Return False ' Update ThongTH 2015/09/21
        End Try
    End Function


    ''' <summary>
    '''     Check valid password: input more than two type character
    ''' </summary>
    ''' <remarks></remarks>
    Public Function CheckValidPasswordCharacter(ByVal in_strPasswordNew As String) As Boolean
        Try
            Dim regexLetter As New Regex(MNBTCMN100.CST_RegexLetter)
            Dim regexNumber As New Regex(MNBTCMN100.CST_RegexNumber)
            Dim regexSpecial As New Regex(MNBTCMN100.CST_RegexSpecial)
            Dim regexLetterNumber As New Regex(MNBTCMN100.CST_RegexLetterAndNumber)
            If Not regexLetter.IsMatch(in_strPasswordNew) Then
                If Not regexNumber.IsMatch(in_strPasswordNew) Then
                    If Not regexSpecial.IsMatch(in_strPasswordNew) Then
                        'If Not regexLetterNumber.IsMatch(in_strPasswordNew) Then
                        Return True
                        'End If
                    Else
                        Return False
                        'only special
                    End If
                Else
                    Return False
                    'only number
                End If
            Else
                Return False
                'only letter
            End If

            Return False
        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21
            'MNBTCMN100.ShowMessageException() ' Update ThongTH 2015/09/21
            'Return False ' Update ThongTH 2015/09/21
        End Try
    End Function


    ''' <summary>
    '''     Save Change Password
    ''' </summary>
    ''' <remarks></remarks>
    Public Function SaveChangePassword(ByVal is_strUserCD As String, ByVal in_strPasswordHistory As String,
            ByVal in_strPasswordNew As String) As Boolean
        SaveChangePassword = False
        Try
            Using contextMyNo As New mynoEntities
                Using transScope As DbContextTransaction = contextMyNo.Database.BeginTransaction(IsolationLevel.Chaos)
                    Dim user As m_user =
                            (From e1 In contextMyNo.m_user Where e1.usercd = is_strUserCD Where e1.delflg = 0 Select e1) _
                            .
                            FirstOrDefault()
                    'Encrypt new password
                    in_strPasswordNew = MNBTCMN100.EncryptSha256(in_strPasswordNew)

                    'Being update password
                    user.password = in_strPasswordNew
                    user.onetimepassword = Nothing
                    user.passwordflg = 0
                    user.updusercd = is_strUserCD
                    user.terminalcd = p_strTerminalCdLogin
                    user.upddatetime = MNBTCMN100.GetCurrentTimestamp(contextMyNo)
                    p_strAuthorityCdLogin = user.authoritycd
                    p_strUserCdLogin = user.usercd
                    contextMyNo.SaveChanges()

                    'Input log
                    Dim intSEQ As Integer = MNBTCMN100.InputLogMaster(contextMyNo, "1", "MNUILGN110", "", 0)
                    MNBTCMN100.InputLogDetail(contextMyNo, intSEQ, "", "パスワード変更", "")

                    transScope.Commit()
                    SaveChangePassword = True
                End Using
            End Using
        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21
            'MNBTCMN100.ShowMessageException() ' Update ThongTH 2015/09/21
        End Try
        'Return SaveChangePassword ' Update ThongTH 2015/09/21
    End Function
End Class
