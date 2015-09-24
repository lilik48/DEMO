'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNUIMTN410
'*  機能名称：ユーザマスタメンテナンス登録・照会処理
'*  処理　　：ユーザマスタメンテナンス登録・照会処理（ＢＬ）
'*  内容　　：ユーザマスタメンテナンス登録・照会処理のビジネスロジック
'*  ファイル：MNUIMTN410CTL.vb
'*  備考　　：
'*
'*  Created：2015/06/28 RS. Ngo Duc Anh
Imports System.Data.Entity
Imports System.Text
Imports System.Transactions
Imports MyNo.Common

Public Class MNUIMTN410CTL
    Public contextMyNo As mynoEntities

    
    ''' <summary>
    '''     Get user by usercd
    ''' </summary>
    ''' <param name="in_strUserCd">userCd</param>
    ''' <remarks></remarks>
    Public Function GetUserByUserCd(ByVal in_strUserCd As String) As m_user
        Try
            contextMyNo = New mynoEntities()
            Using transScope As DbContextTransaction = contextMyNo.Database.BeginTransaction(Data.IsolationLevel.Chaos)
                Dim strSqlSelect As String
                Dim query = (From u In contextMyNo.m_user Where u.usercd = in_strUserCd And u.delflg = 0
                        Select u)
                strSqlSelect = query.ToString()
                'replace parameter input log
                If strSqlSelect.IndexOf("@p__linq__0") > 0 Then
                    strSqlSelect = strSqlSelect.Replace("@p__linq__0", "'" & in_strUserCd & "'")
                End If
                Dim intSEQ As Integer = MNBTCMN100.InputLogMaster(contextMyNo,
                                                                  "2", "MNUIMTN410", "", 0)
                MNBTCMN100.InputLogDetail(contextMyNo, intSEQ, "", "アンカー押下 (" + strSqlSelect + ")", "")
                transScope.Commit()
                Return query.SingleOrDefault()
            End Using
        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21: add code
            'MNBTCMN100.ShowMessageException()  ' Update ThongTH 2015/09/21: comment code
            'Return Nothing
        End Try
    End Function

    
    ''' <summary>
    '''     Get random string
    ''' </summary>
    ''' <remarks></remarks>
    Public Function RandomString() As String

        Dim strRandom As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        Dim rd As New Random
        Dim sbReturn As New StringBuilder

        For i As Integer = 1 To 8
            Dim idx As Integer = rd.Next(0, 61)
            sbReturn.Append(strRandom.Substring(idx, 1))
        Next
        Return sbReturn.ToString()
    End Function

    
    ''' <summary>
    '''     CheckInput
    ''' </summary>
    ''' <param name="in_strUserCD">userCD</param>
    ''' <param name="in_strUserName">userName</param>
    ''' <param name="in_strAuthorityCD">authorityCD</param>
    ''' <param name="in_strOneTimePassword">oneTimePassword</param>
    ''' <param name="in_intUserMaintenanceFlag">userMaintenanceFlag</param>
    ''' <remarks></remarks>
    Public Function CheckInput(ByVal in_strUserCD As String, ByVal in_strUserName As String,
                               ByVal in_strAuthorityCD As String, ByVal in_strOneTimePassword As String,
                               ByVal in_intUserMaintenanceFlag As Integer) As Boolean
        Try
            If in_strUserCD = Nothing OrElse in_strUserCD = "" Then
                MNBTCMN100.ShowMessage("MSGVWE00001", "ユーザコード", "", "")
                Return False
            ElseIf MNBTCMN100.CheckExternalCharacter(in_strUserCD) = True Then
                MNBTCMN100.ShowMessage("MSGVWE00003", "ユーザコード", "", "")
                Return False
            ElseIf in_strUserName = Nothing OrElse in_strUserName = "" Then
                MNBTCMN100.ShowMessage("MSGVWE00001", "ユーザ名", "", "")
                Return False
            ElseIf MNBTCMN100.CheckExternalCharacter(in_strUserName) = True Then
                MNBTCMN100.ShowMessage("MSGVWE00003", "ユーザ名", "", "")
                Return False
            ElseIf in_strAuthorityCD = Nothing OrElse in_strAuthorityCD = "" Then
                MNBTCMN100.ShowMessage("MSGVWE00005", "権限", "", "")
                Return False
            ElseIf _
                in_intUserMaintenanceFlag = 0 AndAlso
                (in_strOneTimePassword = Nothing OrElse in_strOneTimePassword = "") Then
                MNBTCMN100.ShowMessage("MSGVWE00006", "", "", "")
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21: add code
            'MNBTCMN100.ShowMessageException()  ' Update ThongTH 2015/09/21: comment code            
            'Return False
        End Try
    End Function

    
    ''' <summary>
    '''     CheckUserExist
    ''' </summary>
    ''' <param name="in_strUserCD">userCD</param>
    ''' <remarks></remarks>
    Public Function CheckUserExist(ByVal in_strUserCD As String) As Boolean
        contextMyNo = New mynoEntities()
        Dim query = (From u In contextMyNo.m_user Where u.usercd = in_strUserCD
                Select u)
        Dim user = query.FirstOrDefault
        If user Is Nothing Then
            Return False
        End If
        Return True
    End Function

    
    ''' <summary>
    '''     InsertNewUser
    ''' </summary>
    ''' <param name="in_strUserCD">userCD</param>
    ''' <param name="in_strName">name</param>
    ''' <param name="in_strOnetimePassword">onetimePassword</param>
    ''' <param name="in_strAuthoritycd">authoritycd</param>
    ''' <remarks></remarks>
    Public Sub InsertNewUser(ByVal in_strUserCD As String, ByVal in_strName As String,
                             ByVal in_strOnetimePassword As String,
                             ByVal in_strAuthoritycd As String)
        Try
            contextMyNo = New mynoEntities()
            Dim user = New m_user
            user.usercd = in_strUserCD
            user.password = Nothing
            user.name = in_strName
            user.onetimepassword = in_strOnetimePassword
            user.passwordflg = 1
            user.terminalcd = p_strTerminalCdLogin
            user.addjusercd = p_strUserCdLogin
            user.authoritycd = in_strAuthoritycd
            user.delflg = 0
            user.adddatetime = MNBTCMN100.GetCurrentTimestamp(contextMyNo).ToString("yyyy/MM/dd HH:mm:ss")

            Using transScope As DbContextTransaction = contextMyNo.Database.BeginTransaction(IsolationLevel.Chaos)
                contextMyNo.m_user.Add(user)
                'get csv data format
                Dim strCsvDataAfter As String = MNBTCMN100.CreateCsvDataFromObject(user)
                'end get csv data format

                'insert log
                Dim intSEQ As Integer = MNBTCMN100.InputLogMaster(contextMyNo,
                                                                  "4", "MNUIMTN410", "", 0)
                MNBTCMN100.InputLogDetail(contextMyNo, intSEQ, "", "新規登録前   (ユーザーコード=" + in_strUserCD + ")", "")
                MNBTCMN100.InputLogDetail(contextMyNo, intSEQ, "", "新規登録後", strCsvDataAfter)
                transScope.Commit()
            End Using
        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21: add code
            'MNBTCMN100.ShowMessageException()  ' Update ThongTH 2015/09/21: comment code            
        End Try
    End Sub

    
    ''' <summary>
    '''     UpdateUser
    ''' </summary>
    ''' <param name="in_strUserCD">userCD</param>
    ''' <param name="in_strName">name</param>
    ''' <param name="in_strOnetimePassword">onetimePassword</param>
    ''' <param name="in_strAuthoritycd">authoritycd</param>
    ''' <remarks></remarks>
    Public Sub UpdateUser(ByVal in_strUserCD As String, ByVal in_strName As String,
                          ByVal in_strOnetimePassword As String,
                          ByVal in_strAuthoritycd As String)
        Try

            contextMyNo = New mynoEntities()
            Using transScope As DbContextTransaction = contextMyNo.Database.BeginTransaction(IsolationLevel.Chaos)
                Dim query = (From u In contextMyNo.m_user Where u.usercd = in_strUserCD And u.delflg = 0
                        Select u)
                Dim user = query.SingleOrDefault()

                'create csv data format
                Dim strCsvDataBeforeUpdate As String = MNBTCMN100.CreateCsvDataFromObject(user)

                user.usercd = in_strUserCD
                user.name = in_strName
                user.onetimepassword = in_strOnetimePassword
                If in_strOnetimePassword <> "" AndAlso in_strOnetimePassword <> Nothing Then
                    user.passwordflg = 1
                    user.password = Nothing
                Else
                    user.passwordflg = 0
                End If
                ' user.passwordflg = 1
                user.terminalcd = p_strTerminalCdLogin
                user.updusercd = p_strUserCdLogin
                user.authoritycd = in_strAuthoritycd
                user.delflg = 0
                user.upddatetime = MNBTCMN100.GetCurrentTimestamp(contextMyNo).ToString("yyyy/MM/dd HH:mm:ss")
                'create csv data format
                Dim strCsvDataAfterUpdate As String = MNBTCMN100.CreateCsvDataFromObject(user)

                'input log
                Dim intSEQ As Integer = MNBTCMN100.InputLogMaster(contextMyNo,
                                                                  "4", "MNUIMTN410", "", 0)
                MNBTCMN100.InputLogDetail(contextMyNo, intSEQ, "", "更新前（" + in_strUserCD + "）", strCsvDataBeforeUpdate)
                MNBTCMN100.InputLogDetail(contextMyNo, intSEQ, "", "更新後（" + in_strUserCD + "）", strCsvDataAfterUpdate)
                contextMyNo.SaveChanges()
                transScope.Commit()
            End Using
        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21: add code
            'MNBTCMN100.ShowMessageException()  ' Update ThongTH 2015/09/21: comment code            
        End Try
    End Sub

    
    ''' <summary>
    '''     DeleteUser
    ''' </summary>
    ''' <param name="in_strUserCD">userCD</param>
    ''' <remarks></remarks>
    Public Sub DeleteUser(ByVal in_strUserCD As String)
        Try

            contextMyNo = New mynoEntities()
            Using transScope As DbContextTransaction = contextMyNo.Database.BeginTransaction(IsolationLevel.Chaos)
                Dim query = (From u In contextMyNo.m_user Where u.usercd = in_strUserCD
                        Select u)
                Dim user = query.SingleOrDefault()
                'create csv data format
                Dim strCsvDataBeforeUpdate As String = MNBTCMN100.CreateCsvDataFromObject(user)

                user.delflg = 1
                user.terminalcd = p_strTerminalCdLogin
                user.updusercd = p_strUserCdLogin
                user.upddatetime = MNBTCMN100.GetCurrentTimestamp(contextMyNo)
                Dim intSEQ As Integer = MNBTCMN100.InputLogMaster(contextMyNo,
                                                                  "4", "MNUIMTN410", "", 0)
                MNBTCMN100.InputLogDetail(contextMyNo, intSEQ, "", "削除前       (ユーザーコード=" + in_strUserCD + "）", strCsvDataBeforeUpdate)
                MNBTCMN100.InputLogDetail(contextMyNo, intSEQ, "", "削除後", "")
                contextMyNo.SaveChanges()
                transScope.Commit()
            End Using
        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21: add code
            'MNBTCMN100.ShowMessageException()  ' Update ThongTH 2015/09/21: comment code            
        End Try
    End Sub
End Class

Public Class Item
    Public Property Value As String
    Public Property Name As String
End Class