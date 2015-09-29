'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNUILGN100
'*  機能名称：ログイン処理
'*  処理　　：ログイン処理（ＢＬ）
'*  内容　　：ログイン処理のビジネスロジック
'*  ファイル：MNUILGN100CTL.vb
'*  備考　　：
'*
'*  Created：2015/06/25 RS. Nguyen Xuan Quy
'***************************************************************************************
Imports System.Data.Entity
Imports System.Transactions
Imports MyNo.Common


Public Class MNUILGN100CTL
    ''' <summary>
    '''     Validate data input
    ''' </summary>
    ''' <param name="in_strUserCD">User Code</param>
    ''' <param name="in_strPassword">Password</param>
    ''' <remarks>
    '''     Return: False - not input UserCD or Password.
    '''     True - otherwise
    ''' </remarks>
    Public Function Validate(ByVal in_strUserCD As String, ByVal in_strPassword As String) As Boolean
        Try
            ' check UserCD not input
            If String.IsNullOrEmpty(in_strUserCD) Then
                MNBTCMN100.ShowMessage("MSGVWE00001", "ユーザコード", "", "")
                Return False
            ElseIf MNBTCMN100.CheckExternalCharacter(in_strUserCD) Then
                MNBTCMN100.ShowMessage("MSGVWE00003", "ユーザコード", "", "")
            End If
            ' check Password not input
            If String.IsNullOrEmpty(in_strPassword) Then
                MNBTCMN100.ShowMessage("MSGVWE00001", "パスワード", "", "")
                Return False
            End If
        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21
            'MNBTCMN100.ShowMessageException() ' Update ThongTH 2015/09/21
        End Try
        Return True
    End Function


    ''' <summary>
    '''     Check login
    ''' </summary>
    ''' <param name="in_strUserCD">User Code</param>
    ''' <param name="in_strPassword">Password</param>
    ''' <remarks></remarks>
    Public Sub CheckLogin(ByVal in_strUserCD As String, ByVal in_strPassword As String)
        Try
            Dim isLoginSuccess As Boolean = False
            Dim isPasswordOneTime As Boolean = False
            Using contextMyNo As New mynoEntities
                Using transScope As DbContextTransaction = contextMyNo.Database.BeginTransaction(IsolationLevel.Chaos)
                    'Check Valid parameters
                    If Validate(in_strUserCD, in_strPassword) = False Then
                        Return
                    End If

                    ' Get user info
                    Dim user As m_user = GetUserMasterByUserCD(in_strUserCD)

                    ' Check exist record
                    If IsNothing(user) Then
                        'input log: Login false
                        MNBTCMN100.InputLogDetail(contextMyNo,
                                                  MNBTCMN100.InputLogMaster(contextMyNo, "1", "MNUILGN100", "", 0), "",
                                                  "ログイン失敗", "")
                        MNBTCMN100.ShowMessage("MSGVWE00007", "ユーザコード", "", "")
                    Else
                        If user.passwordflg = 1 Then ' Check first times login
                            If user.onetimepassword.Equals(in_strPassword) Then
                                isLoginSuccess = True
                                isPasswordOneTime = True
                                p_strUserCdLogin = user.usercd
                                p_strAuthorityCdLogin = user.authoritycd
                                'input log: Login success
                                MNBTCMN100.InputLogDetail(contextMyNo,
                                                          MNBTCMN100.InputLogMaster(contextMyNo, "1", "MNUILGN100", "",
                                                                                    0), "", "ログイン成功", "")
                            Else
                                'input log: Login false
                                MNBTCMN100.InputLogDetail(contextMyNo,
                                                          MNBTCMN100.InputLogMaster(contextMyNo, "1", "MNUILGN100", "",
                                                                                    0), "", "ログイン失敗", "")
                                MNBTCMN100.ShowMessage("MSGVWE00007", "パスワード", "", "")
                            End If
                        ElseIf user.passwordflg = 0 Then ' Check normal login
                            If MNBTCMN100.EncryptSha256(in_strPassword).Equals(user.password) Then
                                isLoginSuccess = True
                                p_strUserCdLogin = user.usercd
                                p_strAuthorityCdLogin = user.authoritycd
                                'input log: Login success
                                MNBTCMN100.InputLogDetail(contextMyNo,
                                                          MNBTCMN100.InputLogMaster(contextMyNo, "1", "MNUILGN100", "",
                                                                                    0), "", "ログイン成功", "")
                            Else
                                'input log: Login false
                                MNBTCMN100.InputLogDetail(contextMyNo,
                                                          MNBTCMN100.InputLogMaster(contextMyNo, "1", "MNUILGN100", "",
                                                                                    0), "", "ログイン失敗", "")
                                MNBTCMN100.ShowMessage("MSGVWE00007", "パスワード", "", "")
                            End If
                        End If
                    End If

                    transScope.Commit()
                End Using
            End Using

            If isLoginSuccess AndAlso isPasswordOneTime Then
                Dim frmMNUILGN110 As New frmMNUILGN110
                frmMNUILGN110.p_intPasswordChangeFlg = 1

                frmMNUILGN100.Hide()
                frmMNUILGN110.ShowDialog()
            ElseIf isLoginSuccess AndAlso isPasswordOneTime = False Then
                frmMNUILGN100.Hide()
                Menu.ShowDialog()
            End If

        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21
            'MNBTCMN100.ShowMessageException() ' Update ThongTH 2015/09/21
        End Try
    End Sub


    ''' <summary>
    '''     Get User by UserCD
    ''' </summary>
    ''' <param name="in_strUserCD">User Code</param>
    ''' <remarks></remarks>
    Public Function GetUserMasterByUserCD(ByVal in_strUserCD As String) As m_user
        Try
            Using mynoContext As New mynoEntities
                Dim userQuery = From user As m_user In mynoContext.m_user
                        Where user.usercd = in_strUserCD And user.delflg = 0
                        Select user
                Return userQuery.FirstOrDefault
            End Using
        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21
            'MNBTCMN100.ShowMessageException() ' Update ThongTH 2015/09/21
            Return Nothing
        End Try
    End Function
End Class
