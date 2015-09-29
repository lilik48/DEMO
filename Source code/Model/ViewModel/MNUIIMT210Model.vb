Public Class MNUIIMT210Model
    Public adddatetime As Nullable(Of DateTime)
    Public upddatetime As Nullable(Of DateTime)
    Public upddatetimefull As Nullable(Of DateTime)
    Public barcode As String
    Public companycd As String
    Public companybranchno As Integer
    Public companybranchname As String
    Public companyname As String
    Public staffcd As String
    Public postno As String
    Public stsname As String
    Public tel As String
    Public address1 As String
    Public address2 As String
    Public belongcd1 As String
    Public belongname1 As String
    Public belongname2 As String
    Public familyname As String
    Public familynamekana As String
    Public birthdate As String
    Public sex As String
    Public privateno As String
    Public unofferflg As String
    Public notes As String
    Public abnormalflg As String
    Public abnormalcount As String
    Public delflg As String
    Public privateno1datetime As Nullable(Of DateTime)
    Public privateno2datetime As Nullable(Of DateTime)
    Public privatenocdatetime As Nullable(Of DateTime)    
    Public expdatetime As Nullable(Of DateTime)
    Public deldatetime As Nullable(Of DateTime)
    Public addjusercd As String
    Public updusercd As String
    Public privatenocusercd As String
    Public privateno1usercd As String
    Public privateno2usercd As String
    Public expusercd As String
    Public delusercd As String
    Public familyaddflg As String
    Public fax As String
    Public sendpostno As String
    Public sendprefecturescd As String
    Public sendaddress1 As String
    Public sendaddress2 As String
    Public sendtel As String
    Public sendfax As String
    Public mail As String
    Public vipflg As String
    Public insuredflg As String
    Public sts As String
    Public privatenokey As String
End Class

Public Class MNUIIMT210FamilyResultModel
    Public insuredflg As Integer '家族基礎情報．第３号被保険者フラグが0の時 非表示、1の時 "*"を選択状態にするset trạng thái lựa chọn T_FAMILY.INSUREDFLG = 0=> ko hiển thịT_FAMILY.INSUREDFLG = 1=>  "*"
    Public familyno As Nullable(Of Integer)
    Public familyname As String
    Public familynameFirtName As String  ' 漢字姓 T_FAMILYRESULT.FAMILYNAME cho đến  trước space full size đầu tiên
    Public familynameLastName As String ' 漢字名入力 từ  tiếp của space full size đầu tiên của T_FAMILYRESULT.FAMILYNAME cho đến hết
    Public familynamekana As String
    Public familynamekanaFirtName As String       'カナ姓入力  từ kí tự đầu củaT_FAMILYRESULT.FAMILYNAMEKANA cho đến trước dấu cách halfsize đầu tiền
    Public familynamekanaLastName As String      'カナ名入力 từ space halfsize tiếp theo đầu tiên của T_FAMILYRESULT.FAMILYNAMEKANA cho đến cuối cùng
    Public sex As Nullable(Of Integer)                           '性別入力 set trạng thái lựa chọn T_FAMILYRESULT.SEX=1=>"男"T_FAMILYRESULT.SEX=2=>"女
    ' 生年月日（元号）入力 set kết quả hoán đổi thành lịch nhật của funcition common_lịch tây
    ' 生年月日入力 set kết quả hoán đổi thành lịch nhật của funcition common_lịch tây
    Public birthdate As String
    Public birthdateM As String
    Public birthdateY As String
    Public privateno As String                   ' 個人番号入力  Tiến hành kiểm soát  để hỗ trợ nhập vào từng 4 chữ số một 
    Public unofferflg As Nullable(Of Boolean)
    Public familyaddflg As Nullable(Of Integer)
    Public privatenokey As String
End Class