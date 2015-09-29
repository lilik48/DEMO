'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNUIMNU100
'*  機能名称：メインメニュー画面
'*  処理　　：メインメニュー画面
'*  内容　　：メインメニュー画面
'*  ファイル：MNUIMNU100.vb
'*  備考　　：
'*
'*  Created：2015/06/26 RS. Pham Duc Anh
'***************************************************************************************
Imports MyNo.Common

Public Class frmMNUIMNU100
    Dim MNUIMNU100CTL As New MNUIMNU100CTL

    Private Sub MNUIMNU100_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Text = MNBTCMN100.SetTitleScreen()

        Try
            ' Get listApp By AuthorityCD
            Dim lstAppCD As List(Of String) = MNUIMNU100CTL.GetListAppByAuthorityCD(p_strAuthorityCdLogin)

            'Setting display or hidden follow App and ViewAuthFlg
            If lstAppCD Is Nothing OrElse lstAppCD.Count = 0 Then
                btnMNUIDTR100.Visible = False
                btnMNUIDTR200.Visible = False
                btnMNUIKTO100.Visible = False
                btnMNUIKTR100.Visible = False
                btnMNUIKTR200.Visible = False
                btnMNUIKTR300.Visible = False
                btnMNUIKTR400.Visible = False
                btnMNUIIMT100.Visible = False
                btnMNUIIMT200.Visible = False
                btnMNUIMTN100.Visible = False
                btnMNUIMTN200.Visible = False
                btnMNUIMTN300.Visible = False
                btnMNUIMTN400.Visible = False
                btnMNUIMTN500.Visible = False
                Return
            End If

            'MNUIDTR100
            If lstAppCD.Contains("MNUIDTR100") Then
                btnMNUIDTR100.Visible = True
            Else
                btnMNUIDTR100.Visible = False
            End If
            'MNUIDTR200
            If lstAppCD.Contains("MNUIDTR200") Then
                btnMNUIDTR200.Visible = True
            Else
                btnMNUIDTR200.Visible = False
            End If
            'MNUIKTO100
            If lstAppCD.Contains("MNUIKTO100") Then
                btnMNUIKTO100.Visible = True
            Else
                btnMNUIKTO100.Visible = False
            End If
            'MNUIKTR100
            If lstAppCD.Contains("MNUIKTR100") Then
                btnMNUIKTR100.Visible = True
            Else
                btnMNUIKTR100.Visible = False
            End If
            'MNUIKTR200
            If lstAppCD.Contains("MNUIKTR200") Then
                btnMNUIKTR200.Visible = True
            Else
                btnMNUIKTR200.Visible = False
            End If
            'MNUIKTR300
            If lstAppCD.Contains("MNUIKTR300") Then
                btnMNUIKTR300.Visible = True
            Else
                btnMNUIKTR300.Visible = False
            End If
            'MNUIKTR400
            If lstAppCD.Contains("MNUIKTR400") Then
                btnMNUIKTR400.Visible = True
            Else
                btnMNUIKTR400.Visible = False
            End If
            'MNUIIMT100
            If lstAppCD.Contains("MNUIIMT100") Then
                btnMNUIIMT100.Visible = True
            Else
                btnMNUIIMT100.Visible = False
            End If
            'MNUIIMT200
            If lstAppCD.Contains("MNUIIMT200") Then
                btnMNUIIMT200.Visible = True
            Else
                btnMNUIIMT200.Visible = False
            End If
            'MNUIMTN100
            If lstAppCD.Contains("MNUIMTN100") Then
                btnMNUIMTN100.Visible = True
            Else
                btnMNUIMTN100.Visible = False
            End If
            'MNUIMTN200
            If lstAppCD.Contains("MNUIMTN200") Then
                btnMNUIMTN200.Visible = True
            Else
                btnMNUIMTN200.Visible = False
            End If
            'MNUIMTN300
            If lstAppCD.Contains("MNUIMTN300") Then
                btnMNUIMTN300.Visible = True
            Else
                btnMNUIMTN300.Visible = False
            End If
            'MNUIMTN400
            If lstAppCD.Contains("MNUIMTN400") Then
                btnMNUIMTN400.Visible = True
            Else
                btnMNUIMTN400.Visible = False
            End If
            'MNUIMTN500
            If lstAppCD.Contains("MNUIMTN500") Then
                btnMNUIMTN500.Visible = True
            Else
                btnMNUIMTN500.Visible = False
            End If
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Me.Close()
    End Sub

    Private Sub frmMNUIMNU100_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Update ThongTH 2015/09/21: add code Try .. Catch
        Try
            If MNBTCMN100.ShowMessageConfirm("MSGVWI00004", "", "", "") = DialogResult.OK Then
                MNUIMNU100CTL.Logout("MNUIMNU100")
            Else
                e.Cancel = True
            End If
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    Private Sub btnMNUIDTR100_Click(sender As Object, e As EventArgs) Handles btnMNUIDTR100.Click
        Dim frmMNUIDTR100 As New frmMNUIDTR100
        frmMNUIDTR100.ShowDialog()
    End Sub

    Private Sub btnMNUIDTR200_Click(sender As Object, e As EventArgs) Handles btnMNUIDTR200.Click
        Dim frmMNUIDTR200 As New frmMNUIDTR200
        frmMNUIDTR200.ShowDialog()
    End Sub

    Private Sub btnMNUIIMT100_Click(sender As Object, e As EventArgs) Handles btnMNUIIMT100.Click
        Dim frmMNUIIMT100 As New frmMNUIIMT100
        frmMNUIIMT100.ShowDialog()
    End Sub

  

    Private Sub btnMNUIMTN400_Click(sender As Object, e As EventArgs) Handles btnMNUIMTN400.Click
        Dim frmMNUIMTN400 As New frmMNUIMTN400
        frmMNUIMTN400.ShowDialog()
    End Sub

   

    Private Sub frmMNUIMNU100_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        Dim myGraphics As Graphics = Me.CreateGraphics
        Dim myPen As Pen
        myPen = New Pen(Drawing.Color.Black, 1)
        myGraphics.DrawRectangle(myPen, 30, 68, 416, 277) '30:61:416:285  ' +0, +7, +0, -8
        myGraphics.DrawRectangle(myPen, 466, 68, 416, 277)
        myGraphics.DrawRectangle(myPen, 902, 68, 416, 277)
        myGraphics.DrawRectangle(myPen, 30, 373, 416, 330)
        myGraphics.DrawRectangle(myPen, 466, 373, 416, 330)
    End Sub

    Private Sub frmMNUIMNU100_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        'Me.TopMost = True
    End Sub

    Private Sub frmMNUIMNU100_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        'Me.TopMost = False
    End Sub
End Class