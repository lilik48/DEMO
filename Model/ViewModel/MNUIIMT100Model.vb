Public Class MNUIIMT100Model
    Public companycd As String              ' 企業情報の企業コード
    Public companybranchno As Integer        ' 企業情報の枝番
    Public companyname As String            ' 企業情報の企業名
    Public companybranchname As String      ' 企業情報の枝番名
    Public adddatetime As String            ' Đăng ký日時 ※個人情報の1件目のĐăng ký日時       time đăng kí       *time đăng kí của  record đầu tiên  thông tin cá nhân(trong bảng T_PRIVATE)
    Public sumall As Integer                 ' ■取込件数　※判断後SUM
    Public sumkitoutput As Integer           ' ■調査キット出力件数　※判断後SUM
    Public sumunarrival As Integer           ' ■未着者件数　※判断後SUM
    Public sumregisternormal As Integer      ' ■正常Đăng ký件数　※判断後SUM
    Public sumregisterabnormal As Integer    ' ■異常原票Đăng ký件数　※判断後SUM
    Public sumdifference As Integer          ' ■受取差異件数
    Public sumregister1 As Integer           ' ■入力一回目件数　※判断後SUM
    Public sumregister2 As Integer           ' ■入力二回目件数　※判断後SUM
    Public sumdelete As Integer              ' ■データ削除件数　※判断後SUM
    Public sumdelivery As Integer            ' ■納品件数　※判断後SUM
    Public percentcomplete As String        ' ■進捗率 Tỉ lệ tiến độ

    Public delivschdate As Nullable(Of Date)
    Public delivdate As Nullable(Of Date)
    Public delschdate As Nullable(Of Date)
    Public rate As Double
End Class
