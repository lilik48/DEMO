'***************************************************************************************
'*  システム：ＭＮサービスシステム
'*  機能ＩＤ：MNUIKTO100Model
'*  機能名称：個人番号調査キット出力画面
'*  処理　　：シ個人番号調査キット出力画面
'*  内容　　：個人番号調査キット出力画面
'*  ファイル：MNUIKTO100Model.vb
'*  備考　　：
'*
'*  Created：2015/07/16 RS. Pham Duc Anh
'***************************************************************************************

Imports System
Imports System.Collections.Generic
Public Class MNUIKTO100Model
    Public Property companycd As String
    Public Property companybranchno As Integer
    Public Property staffcd As String
    Public Property sendpostno As String
    Public Property sumpostnocount As String
    Public Property printflg As String
    Public Property sumpostno As String
    Public Property printflgGroup As String
    Public Property seq As Integer
    Public Property sendpostsort As Integer
    Public Property familyno As Integer
    Public Property companyname As String
    Public Property sendpostnomark As String
    Public Property fammilyname As String
    Public Property sendaddress1 As String
    Public Property sendaddress2 As String
    Public Property fammilynamekana As String
    Public Property insuredflg As String
    Public Property sex As String
    Public Property birthdate As Date?
    Public Property birthdateOut As String
    Public Property printdate As Date
    Public Property printdateOut As String
    Public Property customerbarcode As String
    Public Property barcode As String
    Public Property contactbelongname1 As String
    Public Property contactbelongname2 As String
    Public Property contactbelongname As String
    Public Property contactaddress1 As String
    Public Property contactaddress2 As String
    Public Property contactaddress As String
    Public Property contacttel As String
    Public Property contactpostno As String
    Public Property address1 As String
    Public Property address2 As String
    Public Property address As String
    Public Property trusteeflg07 As Nullable(Of Integer)
    Public Property sendname As String
End Class

Partial Public Class w_privateno_group
    Public Property seq As Long
    Public Property companycd As String
    Public Property companybranchno As Integer
    Public Property staffcd As String
    Public Property familyno As Integer
    Public Property companyname As String
    Public Property sendpostno As String
    Public Property sendpostnomark As String
    Public Property sendname As String
    Public Property sendaddress1 As String
    Public Property sendaddress2 As String
    Public Property familyname As String
    Public Property familynamekana As String
    Public Property insuredflg As String
    Public Property sex As String
    Public Property birthdate As String
    Public Property printdate As String
    Public Property customerbarcode As String
    Public Property barcode As String
    Public Property contactbelongname As String
    Public Property contactaddress As String
    Public Property contacttel As String
    Public Property address As String
    Public Property sendpostsort As Nullable(Of Integer)
    Public Property sumpostno As String
    Public Property printflg As Nullable(Of Integer)
    Public Property staffcdgroup As String
    Public Property familynamestaff As String

End Class