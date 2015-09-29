Public Class CustomCombobox
    Inherits ComboBox

    ' Delete ThongTH 20150903: issue 改善 #34062
    'Private Sub CustomCombobox_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
    '    ' open drop down
    '    If e.Alt And e.KeyCode = Keys.Down Then
    '        MyBase.DroppedDown = True
    '    ElseIf e.Alt And e.KeyCode = Keys.Up Then
    '        MyBase.DroppedDown = False
    '    ElseIf e.KeyCode = Keys.Enter Then
    '        MyBase.DroppedDown = False
    '    ElseIf e.KeyCode = Keys.Escape Then
    '        MyBase.DroppedDown = False
    '    End If
    'End Sub
End Class
