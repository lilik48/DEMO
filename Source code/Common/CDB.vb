Imports Npgsql
Imports MyNo.Common

Public Class CDB
    Public p_cn As NpgsqlConnection = New NpgsqlConnection
    Public p_cm As NpgsqlCommand = New NpgsqlCommand
    Public p_tr As NpgsqlTransaction
    Public rs As NpgsqlDataReader

    Private c_connection As String = String.Format("Server=" & p_strHostname & ";Port=" & p_strPortname & ";Username=" & p_strUsername + ";Password=" & p_strPassword + ";Database=" & p_strDatabaseName & ";")

    ''' <summary>
    ''' constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Open connect
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Connect() As Boolean
        Try
            p_cn.ConnectionString = c_connection
            p_cn.Open()
            p_cm.Connection = p_cn
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Disconnect to DB
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Disconnect()
        Try
            p_cm = Nothing
            p_tr = Nothing
            p_cn.Close()
            p_cn = Nothing
        Catch ex As Exception
             MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    ''' <summary>
    ''' Check current connect
    ''' </summary>
    ''' <returns>false: discontected, true: connecting</returns>
    ''' <remarks></remarks>
    Public Function IsConnect() As Boolean
        If p_cm.Connection Is Nothing Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Execute SQL
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExecuteSQL(ByVal sql As String) As Integer
        Dim cnt As Integer = 0
        Try
            p_cm.CommandText = sql
            cnt = p_cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        End Try
        Return cnt
    End Function

    ''' <summary>
    ''' Execute Select
    ''' </summary>
    ''' <param name="cDb"></param>
    ''' <param name="sql"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SelectOpen(ByRef cDb As CDB, ByVal sql As String) As Boolean
        Try
            'ﾚｺｰﾄﾞ取得
            cDb.p_cm.CommandText = sql

            If cDb.IsConnect = False Then
                cDb.Connect()
            End If

            rs = cDb.p_cm.ExecuteReader()
            If rs.HasRows = False Then
                Return False
            End If
            Return True
        Catch ex As Exception
            MNBTCMN100.ShowMessageException()
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Cursor Close
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Close()
        Try            
            rs.Close()
            rs = Nothing
        Catch ex As Exception
             MNBTCMN100.ShowMessageException()
        End Try
    End Sub

    ''' <summary>
    ''' Next record retrieval
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NextRecord() As Boolean
        Try
            If rs.Read() = False Then
                Return False
            End If
            Return True
        Catch ex As Exception
           MNBTCMN100.ShowMessageException()
            Return False
        End Try
    End Function
End Class
