''' <summary>
''' Set item for comboboxcell
''' </summary>
''' <remarks></remarks>
Public Class BoxItem

    Sub New()
        ' TODO: Complete member initialization 
    End Sub

    Public Property ID As String
    Public Property Value As String
    Public Property viewpriority As Integer
    Public Sub New(id As String, value As String)
        Me.ID = id
        Me.Value = value
    End Sub
End Class