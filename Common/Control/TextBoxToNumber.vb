
Public Class TextBoxToNumber
    Inherits BaseTextBox

    Private bkText As String

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)

        If String.IsNullOrWhiteSpace(Text) Then
            Exit Sub
        End If

        If Common.MNBTCMN100.CheckValidateOnlyNumber(Text) Then
            Text = bkText
        End If

    End Sub

    Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs)
        MyBase.OnKeyPress(e)

        '押されたキーが 0～9,バックスペースでない場合は、イベントをキャンセルする
        If e.KeyChar < "0"c OrElse "9"c < e.KeyChar Then
            If e.KeyChar <> vbBack Then
                e.Handled = True
                Exit Sub
            End If
        End If

        bkText = Text

    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()

        Margin = New Padding(3, 5, 3, 5)
        ImeMode = ImeMode.Off

    End Sub

    Protected Overrides Sub OnGotFocus(e As EventArgs)
        MyBase.OnGotFocus(e)

        SelectAll()

    End Sub
End Class
