'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNUIMTN400
'*  機能名称：ユーザマスタ検索処理
'*  処理　　：ユーザマスタ検索処理（ＢＬ）
'*  内容　　：ユーザマスタ検索処理のビジネスロジック
'*  ファイル：MNUIMTN400CTL.vb
'*  備考　　：
'*
'*  Created：2015/06/26 RS. Ngo Duc Anh
'***************************************************************************************
Imports System.Data.Entity
Imports System.Transactions
Imports MyNo.Common

Public Class MNUIMTN400CTL
    Public contextMyNo As mynoEntities

    
    ''' <summary>
    '''     Set data to cmbAuthorityCD
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetListAuthorityCDs() As List(Of Item)
        Try
            contextMyNo = New mynoEntities()
            Dim lstAuthorityCD = (From auth In contextMyNo.m_authority
                    Order By auth.viewpriority Ascending
                    Select New Item With {.Value = auth.authoritycd,
                    .Name = auth.name}).ToList()
            Dim lstReturn As List(Of Item) = New List(Of Item)
            lstReturn.Add(New Item With {.Value = Nothing,
                             .Name = ""})
            lstReturn.AddRange(lstAuthorityCD)
            Return lstReturn
        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21: add code
            'MNBTCMN100.ShowMessageException() 'Update ThongTH 2015/09/21: comment code
            'Return Nothing
        End Try
    End Function

    
    ''' <summary>
    '''     Set data to dgvUser
    ''' </summary>
    ''' <param name="in_strUserCd">userCd</param>
    ''' <param name="in_strUserName">userName</param>
    ''' <param name="in_strAuthorityCd">authorityCd</param>
    ''' <remarks></remarks>
    Public Function GetListUsers(ByVal in_strUserCd As String, ByVal in_strUserName As String,
                                 ByVal in_strAuthorityCd As String) _
        As List(Of DataGridViewItem)
        contextMyNo = New mynoEntities()

        Dim lstUsers As New List(Of DataGridViewItem)
        Dim strSqlSelect As String
        Try
            Using transScope As DbContextTransaction = contextMyNo.Database.BeginTransaction(IsolationLevel.Chaos)
                'no input search conditions
                If (in_strUserCd = "" OrElse in_strUserCd = Nothing) AndAlso
                   (in_strUserName = "" OrElse in_strUserName = Nothing) AndAlso
                   (in_strAuthorityCd = "" OrElse in_strAuthorityCd = Nothing) Then
                    Dim userQuery = (From user In contextMyNo.m_user Join auth In contextMyNo.m_authority
                            On user.authoritycd Equals auth.authoritycd Where user.delflg = 0
                            Order By user.authoritycd, user.usercd Ascending
                            Select New DataGridViewItem With {.UserCD = user.usercd,
                            .UserName = user.name,
                            .AuthorityName = auth.name,
                            .AuthorityCD = auth.authoritycd})
                    lstUsers = userQuery.ToList()
                    strSqlSelect = userQuery.ToString()

                ElseIf _
                    in_strUserCd <> "" AndAlso in_strUserCd <> Nothing AndAlso
                    (in_strUserName = "" OrElse in_strUserName = Nothing) AndAlso
                    (in_strAuthorityCd = "" OrElse in_strAuthorityCd = Nothing) Then _
'input search conditions: only userCd
                    Dim userQuery = (From user In contextMyNo.m_user Join auth In contextMyNo.m_authority
                            On user.authoritycd Equals auth.authoritycd
                            Where user.delflg = 0 And user.usercd = in_strUserCd
                            Order By user.authoritycd, user.usercd Ascending
                            Select New DataGridViewItem With {.UserCD = user.usercd,
                            .UserName = user.name,
                            .AuthorityName = auth.name,
                            .AuthorityCD = auth.authoritycd})
                    lstUsers = userQuery.ToList()
                    strSqlSelect = userQuery.ToString()
                    'replace parameter input log
                    If strSqlSelect.IndexOf("@p__linq__0") > 0 Then
                        strSqlSelect = strSqlSelect.Replace("@p__linq__0", "'" & in_strUserCd & "'")
                    End If

                ElseIf _
                    (in_strUserCd = "" OrElse in_strUserCd = Nothing) AndAlso
                    (in_strUserName <> "" AndAlso in_strUserName <> Nothing) AndAlso
                    (in_strAuthorityCd = "" OrElse in_strAuthorityCd = Nothing) Then _
'input search conditions: only UserName
                    Dim userQuery = (From user In contextMyNo.m_user Join auth In contextMyNo.m_authority
                            On user.authoritycd Equals auth.authoritycd
                            Where _
                            user.delflg = 0 And
                            user.name.Contains(in_strUserName)
                            Order By user.authoritycd, user.usercd Ascending
                            Select New DataGridViewItem With {.UserCD = user.usercd,
                            .UserName = user.name,
                            .AuthorityName = auth.name,
                            .AuthorityCD = auth.authoritycd})
                    lstUsers = userQuery.ToList()
                    strSqlSelect = userQuery.ToString()
                    'replace parameter input log
                    If strSqlSelect.IndexOf("@p__linq__0") > 0 Then
                        strSqlSelect = strSqlSelect.Replace("@p__linq__0", "'%" & in_strUserName & "%'")
                    End If
                ElseIf _
                    (in_strUserCd = "" OrElse in_strUserCd = Nothing) AndAlso
                    (in_strUserName = "" OrElse in_strUserName = Nothing) AndAlso
                    (in_strAuthorityCd <> "" AndAlso in_strAuthorityCd <> Nothing) Then _
'input search conditions: only AuthorityCd
                    Dim userQuery = (From user In contextMyNo.m_user Join auth In contextMyNo.m_authority
                            On user.authoritycd Equals auth.authoritycd
                            Where user.delflg = 0 And user.authoritycd = in_strAuthorityCd
                            Order By user.authoritycd, user.usercd Ascending
                            Select New DataGridViewItem With {.UserCD = user.usercd,
                            .UserName = user.name,
                            .AuthorityName = auth.name,
                            .AuthorityCD = auth.authoritycd})
                    lstUsers = userQuery.ToList()
                    strSqlSelect = userQuery.ToString()
                    'replace parameter input log
                    If strSqlSelect.IndexOf("@p__linq__0") > 0 Then
                        strSqlSelect = strSqlSelect.Replace("@p__linq__0", "'" & in_strAuthorityCd & "%'")
                    End If
                ElseIf _
                    (in_strUserCd <> "" AndAlso in_strUserCd <> Nothing) AndAlso
                    (in_strUserName = "" OrElse in_strUserName = Nothing) AndAlso
                    (in_strAuthorityCd <> "" AndAlso in_strAuthorityCd <> Nothing) Then _
                    'input search conditions: authoritycd & usercd
                    Dim userQuery = (From user In contextMyNo.m_user Join auth In contextMyNo.m_authority
                            On user.authoritycd Equals auth.authoritycd
                            Where _
                            user.delflg = 0 And user.authoritycd = in_strAuthorityCd And user.usercd = in_strUserCd
                            Order By user.authoritycd, user.usercd Ascending
                            Select New DataGridViewItem With {.UserCD = user.usercd,
                            .UserName = user.name,
                            .AuthorityName = auth.name,
                            .AuthorityCD = auth.authoritycd})
                    lstUsers = userQuery.ToList()
                    strSqlSelect = userQuery.ToString()
                    'replace parameter input log
                    If strSqlSelect.IndexOf("@p__linq__0") > 0 Then
                        strSqlSelect = strSqlSelect.Replace("@p__linq__0", "'" & in_strAuthorityCd & "'")
                        strSqlSelect = strSqlSelect.Replace("@p__linq__1", "'" & in_strUserCd & "'")
                    End If
                ElseIf _
                    (in_strUserCd <> "" AndAlso in_strUserCd <> Nothing) AndAlso
                    (in_strUserName <> "" AndAlso in_strUserName <> Nothing) AndAlso
                    (in_strAuthorityCd = "" OrElse in_strAuthorityCd = Nothing) Then _
'input search conditions: usercd, username
                    Dim userQuery = (From user In contextMyNo.m_user Join auth In contextMyNo.m_authority
                            On user.authoritycd Equals auth.authoritycd
                            Where _
                            user.delflg = 0 And
                            user.name.Contains(in_strUserName) And
                            user.usercd = in_strUserCd
                            Order By user.authoritycd, user.usercd Ascending
                            Select New DataGridViewItem With {.UserCD = user.usercd,
                            .UserName = user.name,
                            .AuthorityName = auth.name,
                            .AuthorityCD = auth.authoritycd})
                    lstUsers = userQuery.ToList()
                    strSqlSelect = userQuery.ToString()
                    'replace parameter input log
                    If strSqlSelect.IndexOf("@p__linq__0") > 0 Then
                        strSqlSelect = strSqlSelect.Replace("@p__linq__0", "'%" & in_strUserName & "%'")
                        strSqlSelect = strSqlSelect.Replace("@p__linq__1", "'" & in_strUserCd & "'")
                    End If
                ElseIf _
                    (in_strUserCd = "" OrElse in_strUserCd = Nothing) AndAlso
                    (in_strUserName <> "" AndAlso in_strUserName <> Nothing) AndAlso
                    (in_strAuthorityCd <> "" AndAlso in_strAuthorityCd <> Nothing) Then _
'input search conditions: AuthorityCd, username
                    Dim userQuery = (From user In contextMyNo.m_user Join auth In contextMyNo.m_authority
                            On user.authoritycd Equals auth.authoritycd
                            Where _
                            user.delflg = 0 And
                            user.name.Contains(in_strUserName) And
                            user.authoritycd = in_strAuthorityCd
                            Order By user.authoritycd, user.usercd Ascending
                            Select New DataGridViewItem With {.UserCD = user.usercd,
                            .UserName = user.name,
                            .AuthorityName = auth.name,
                            .AuthorityCD = auth.authoritycd})
                    lstUsers = userQuery.ToList()
                    strSqlSelect = userQuery.ToString()
                    'replace parameter input log
                    If strSqlSelect.IndexOf("@p__linq__0") > 0 Then
                        strSqlSelect = strSqlSelect.Replace("@p__linq__0", "'%" & in_strUserName & "%'")
                        strSqlSelect = strSqlSelect.Replace("@p__linq__1", "'" & in_strAuthorityCd & "'")
                    End If
                Else 'input search conditions: input all
                    Dim userQuery = (From user In contextMyNo.m_user Join auth In contextMyNo.m_authority
                            On user.authoritycd Equals auth.authoritycd
                            Where _
                            user.delflg = 0 And
                            user.name.Contains(in_strUserName) And
                            user.authoritycd = in_strAuthorityCd And
                            user.usercd = in_strUserCd
                            Order By user.authoritycd, user.usercd Ascending
                            Select New DataGridViewItem With {.UserCD = user.usercd,
                            .UserName = user.name,
                            .AuthorityName = auth.name,
                            .AuthorityCD = auth.authoritycd})
                    lstUsers = userQuery.ToList()
                    strSqlSelect = userQuery.ToString()
                    'replace parameter input log
                    If strSqlSelect.IndexOf("@p__linq__0") > 0 Then

                        strSqlSelect = strSqlSelect.Replace("@p__linq__0", "'%" & in_strUserName & "%'")
                        strSqlSelect = strSqlSelect.Replace("@p__linq__1", "'" & in_strAuthorityCd & "'")
                        strSqlSelect = strSqlSelect.Replace("@p__linq__2", "'" & in_strUserCd & "'")
                    End If
                End If

                Dim intSEQ As Integer = MNBTCMN100.InputLogMaster(contextMyNo,
                                                                  "2", "MNUIMTN400", "", 0)
                MNBTCMN100.InputLogDetail(contextMyNo, intSEQ, "",
                                          "一覧検索 件数:" + lstUsers.Count.ToString("###,###,###") + " (" + strSqlSelect + ")", "")
                transScope.Commit()

                'If lstUsers.Count() < 19 Then
                '    For i = 0 To 19 - lstUsers.Count()
                '        lstUsers.Add(New DataGridViewItem)
                '    Next
                'End If
                Return lstUsers
            End Using

        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21: add code
            'MNBTCMN100.ShowMessageException() 'Update ThongTH 2015/09/21: comment code
            'Return Nothing
        End Try
    End Function

    Public Class Item
        Public Property Value As String
        Public Property Name As String
    End Class

    Public Class DataGridViewItem
        Public Property UserCD As String
        Public Property UserName As String
        Public Property AuthorityCD As String
        Public Property AuthorityName As String
    End Class
End Class
