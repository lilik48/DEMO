Imports MyNo.Common.MNBTCMN100
Imports MyNo.Common

Public Class BaseForm
    Inherits System.Windows.Forms.Form

    'ｸﾗｽﾒﾝﾊﾞ変数宣言
    Public m_ErrorCd As String   'ｴﾗｰｺｰﾄﾞ
    Public m_Result As Boolean   '戻り値 正常:true  異常:false
    Public m_Param1 As String    'ﾊﾟﾗﾒｰﾀ1(検索画面用)
    Public m_Param2 As String    'ﾊﾟﾗﾒｰﾀ2(検索画面用)
    Public m_Param3 As String  'ﾊﾟﾗﾒｰﾀ3(検索画面用)
    Public m_Param4 As String  'ﾊﾟﾗﾒｰﾀ3(検索画面用)
    Public m_Param5 As String  'ﾊﾟﾗﾒｰﾀ3(検索画面用)
    Public m_LastControl As String '前のｺﾝﾄﾛｰﾙ
    Public m_LastValue As String '前のｺﾝﾄﾛｰﾙの値
    Public m_Step As Integer
    'Public m_Tag As ClassPrint

    Public ProjectNo As String
    Public IraiNo As String
    Public TantouCd As String
    '2008/04/25 追加開始 古川 ESCAPEキーによるクローズ制御


    'Test commit
    'ThongTH Set focus for control textbox
    Public m_ctlFocus As Control
    Private m_CloseByEscapeKey As Boolean

    ''' <summary>
    ''' Ham load form
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        For Each Cntrl As Control In Me.Controls
            If TypeOf Cntrl Is TextBoxBase Then
                Dim txtBox As TextBoxBase = Cntrl
                If Not txtBox.ReadOnly Then
                    AddHandler txtBox.GotFocus, AddressOf textBox_SelectALL
                End If
            End If
        Next

        Icon = frmMNUILGN100.Icon
        ShowIcon = True

    End Sub
    Private Sub textBox_SelectALL(ByVal sender As System.Object, ByVal e As System.EventArgs)

        sender.SelectAll()

    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            GC.Collect()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, _
                                           ByVal keyData As System.Windows.Forms.Keys) _
                                           As Boolean

        If keyData = (Keys.Tab Or Keys.Shift) Then
            If Not TypeOf ActiveControl Is DataGridView Then
                MNBTCMN100.plastControl = ActiveControl
            End If
        ElseIf keyData = Keys.Tab Then
            If Not TypeOf ActiveControl Is DataGridView Then
                MNBTCMN100.plastControl = ActiveControl
            End If
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
    Overridable Sub Disconnection()

    End Sub
End Class
