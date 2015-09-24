Imports MyNo.Common.MNBTCMN100
Imports MyNo.Common

Public Class BaseForm
    Inherits System.Windows.Forms.Form

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
End Class
