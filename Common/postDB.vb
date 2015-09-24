Imports System.Data.Entity
Imports Npgsql

Public Class postDB
    Implements IDisposable

    Private DBCom As NpgsqlConnection
    Private OpenFlg As Boolean

    Public Sub New(conText As mynoEntities)

        DBCom = conText.Database.Connection

        If DBCom.State = 0 Then
            OpenFlg = True
            DBCom.Open()
        End If

    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose

        Try

            If OpenFlg Then
                DBCom.Close()
                DBCom = Nothing
            End If

        Catch ex As Exception

        End Try

    End Sub

    Public Function getSql(strSql As String, Optional offsetFlg As Boolean = False) As Object

        Dim result_ht As Hashtable = Nothing
        Dim result_lst As List(Of Hashtable) = New List(Of Hashtable)

        Try
            Using postCom = New NpgsqlCommand(strSql, DBCom)

                Dim xRs As NpgsqlDataReader = postCom.ExecuteReader

                Do While (xRs.Read)
                    result_ht = New Hashtable

                    For i = 0 To xRs.FieldCount - 1
                        result_ht(xRs.GetName(i)) = If(IsDBNull(xRs(i)), "", xRs(i))
                    Next

                    If offsetFlg Then
                        Exit Do
                    End If

                    result_lst.Add(result_ht)
                Loop

                xRs.Close()
            End Using

        Catch ex As Exception
            Throw ex
        End Try

        If offsetFlg Then
            Return result_ht
        Else
            Return result_lst
        End If

    End Function

End Class
