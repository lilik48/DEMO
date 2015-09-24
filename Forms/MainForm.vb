Imports MyNo.Common

Public Class MainForm
    Public Shared Sub Main()
        Try
            Dim bNotRunning As Boolean = True
            Dim processName As String = Process.GetCurrentProcess().ProcessName
            Dim processes As Process() = Process.GetProcessesByName(processName)
            If processes.Length > 1 Then
                bNotRunning = False
            End If

            'Get connect string
            p_strConnectStringPrimary = GetConnectionStringPrimaryDB()
            'MyBase.New(p_strConnectStringPrimary)
            If bNotRunning = True Then
                Using context As New mynoEntities()
                    'Open connection
                    context.m_user.Count()
                End Using
                'Get cpu number
                p_strTerminalCdLogin = Common.MNBTCMN100.GetCpuId()
                System.Windows.Forms.Application.EnableVisualStyles()
                'Console.WriteLine(MNBTCMN100.EncryptAes("password@", MNBTCMN100.CST_KeyHash))
                'Console.WriteLine(MNBTCMN100.EncryptAes("postgres", MNBTCMN100.CST_KeyHash))
                'Console.WriteLine(MNBTCMN100.EncryptAes("root", MNBTCMN100.CST_KeyHash))
                'Console.WriteLine(MNBTCMN100.EncryptAes("Administrator", MNBTCMN100.CST_KeyHash))
                frmMNUILGN100.ShowDialog()

            Else
                'Application already running
                Application.[Exit]()
            End If
        Catch ex As Exception
            'MNBTCMN100.ShowMessageException()
            System.Windows.Forms.Application.EnableVisualStyles()
            frmMNUILGN100.ShowDialog()
        End Try
    End Sub
End Class
