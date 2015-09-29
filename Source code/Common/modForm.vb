'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNBTCMN100
'*  機能名称：共通処理
'*  処理　　：共通処理（ＢＬ）
'*  内容　　：共通処理のビジネスロジック
'*  ファイル：modForm.vb
'*  備考　　：
'*
'*  Created：2015/06/25 RS. Pham Van Map
'*  Updated：2015/07/20 RS. Pham Van Map    [1№]Encrypt EntityConnection Primary database
'*         ：2015/07/21 RS. Pham Van Map    [2№]Encrypt EntityConnection Private Number database

'***************************************************************************************

Imports System.Data.Entity.Core
Imports System.Configuration
Imports System.Data.Entity.Core.EntityClient
Imports Npgsql
Imports System.Data.SqlClient
Imports MyNo.Common

Module modForm
    Public p_strUserCdLogin As String
    Public p_strAuthorityCdLogin As String
    Public p_strTerminalCdLogin As String
    Public p_strHostname As String
    Public p_strPortname As String
    Public p_strUsername As String
    Public p_strPassword As String
    Public p_strDatabaseName As String
    Public p_strConnectStringPrimary As String
    Public p_strConnectStringPrivateNumber As String

    ''' <summary>
    ''' Get ConnectionString of Primary database
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetConnectionStringPrimaryDB() As String

        Dim entityConn As EntityConnection = New EntityConnection(ConfigurationManager.ConnectionStrings("mynoEntities").ConnectionString)
        Dim connString As EntityConnectionStringBuilder = New EntityClient.EntityConnectionStringBuilder()
        connString.Metadata = "res://*/Model.MyNoModel.csdl|res://*/Model.MyNoModel.ssdl|res://*/Model.MyNoModel.msl"
        connString.Provider = "Npgsql"

        Dim connectionStringBuilder As NpgsqlConnectionStringBuilder = New NpgsqlConnectionStringBuilder(entityConn.StoreConnection.ConnectionString)
        p_strHostname = connectionStringBuilder.Host
        p_strPortname = connectionStringBuilder.Port
        connectionStringBuilder.UserName = connectionStringBuilder.UserName
        Dim strPasswordEncrypt = MNBTCMN100.DecryptAes(System.Text.Encoding.UTF8.GetString(connectionStringBuilder.PasswordAsByteArray), MNBTCMN100.CST_KeyHash)
        connectionStringBuilder.Password = strPasswordEncrypt
        p_strUsername = connectionStringBuilder.UserName
        'p_strPassword = "password@" 'comment
        p_strPassword = strPasswordEncrypt 'uncomment
        p_strDatabaseName = connectionStringBuilder.Database
        connString.ProviderConnectionString = connectionStringBuilder.ConnectionString

        Return [String].Format(connString.ConnectionString)

    End Function
End Module