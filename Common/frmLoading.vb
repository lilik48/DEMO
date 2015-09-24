'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：x
'*  機能名称：処理中画面
'*  処理　　：処理中画面
'*  内容　　：処理中画面
'*  ファイル：frmLoading.vb
'*  備考　　：
'*
'*  Created：2015/06/26 RS. Pham Van Map
'***************************************************************************************
Imports MyNo.Common

Public Class frmLoading
    Private ALT_F4 As Boolean = False

    Private Sub frmLoading_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If ALT_F4 Then
            e.Cancel = True
        End If
        ALT_F4 = False
    End Sub
    Private Sub frmLoading_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ALT_F4 = (e.KeyCode.Equals(Keys.F4) AndAlso e.Alt = True)
    End Sub

    Private Sub frmLoading_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ToolStripStatusLabel1.Text = ""
        ToolStripProgressBar1.Visible = False

        Me.Text = MNBTCMN100.SetTitleScreen()

    End Sub

    Private Sub ToolStripStatusLabel1_TextChanged(sender As Object, e As EventArgs) Handles ToolStripStatusLabel1.TextChanged

        If ToolStripProgressBar1.Maximum = 0 Then
            ToolStripProgressBar1.Visible = False
        Else
            ToolStripProgressBar1.Visible = True
        End If

        Application.DoEvents()

    End Sub

End Class