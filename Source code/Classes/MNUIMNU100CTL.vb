'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNUIMNU100
'*  機能名称：メインメニュー処理
'*  処理　　：メインメニュー処理（ＢＬ）
'*  内容　　：メインメニュー処理のビジネスロジック
'*  ファイル：MNUIMNU100CTL.vb
'*  備考　　：
'*
'*  Created：2015/06/26 RS. Pham Duc Anh
'***************************************************************************************
Imports System.Data.Entity
Imports System.Transactions
Imports MyNo.Common

Public Class MNUIMNU100CTL
    Public mynocontext As mynoEntities

    Public Function GetListAppByAuthorityCD(ByVal authorityCD As String) As List(Of String)
        Try
            mynocontext = New mynoEntities()
            GetListAppByAuthorityCD = (From m In mynocontext.m_appxauth
                Where m.authoritycd = authorityCD AndAlso m.viewauthflg = 1 Select m.appcd).ToList()

        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21: add code
            ' Update ThongTH 2015/09/21: comment code >>>
            '   MessageBox.Show("システムエラーが発生しました。" & Environment.NewLine _
            '                & "エラーコード：" & Err.Number & Environment.NewLine _
            '                & "エラー内容：" & Err.Description, "エラー",
            '                MessageBoxButtons.OK, MessageBoxIcon.Error)
            '   Return Nothing
            ' <<<<
        End Try
    End Function

    Public Sub Logout(ByVal appCD As String)
        Try
            Using mynocontext As New mynoEntities
                Using transScope As DbContextTransaction = mynocontext.Database.BeginTransaction(IsolationLevel.Chaos)
                    'Input log
                    Dim SEQ As Integer = MNBTCMN100.InputLogMaster(mynocontext, "1", appCD, "", 0)
                    MNBTCMN100.InputLogDetail(mynocontext, SEQ, "", "ログアウト", "")
                    transScope.Commit()
                End Using
            End Using
            p_strUserCdLogin = String.Empty
            p_strAuthorityCdLogin = String.Empty
            frmMNUILGN100.txtPassWord.Text = String.Empty
            frmMNUILGN100.txtUserCD.Text = String.Empty
            frmMNUILGN100.Visible = True
        Catch ex As Exception
            Throw ex ' Update ThongTH 2015/09/21: add code
            'MNBTCMN100.ShowMessageException() 'Update ThongTH 2015/09/21: comment code
        End Try
    End Sub
End Class
