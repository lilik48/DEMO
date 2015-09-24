Imports System.ComponentModel
Public Class BaseTextBox
    Inherits TextBox

    Protected Overrides Sub OnGotFocus(e As EventArgs)

        SelectAll()

    End Sub

End Class
