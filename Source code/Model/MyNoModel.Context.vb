﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure

Partial Public Class MyNoEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New(p_strConnectStringPrimary)
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        Throw New UnintentionalCodeFirstException()
    End Sub

    Public Overridable Property m_app() As DbSet(Of m_app)
    Public Overridable Property m_appxauth() As DbSet(Of m_appxauth)
    Public Overridable Property m_authority() As DbSet(Of m_authority)
    Public Overridable Property m_prefectures() As DbSet(Of m_prefectures)
    Public Overridable Property m_user() As DbSet(Of m_user)
    Public Overridable Property t_bienap() As DbSet(Of t_bienap)
    Public Overridable Property t_bienapimport() As DbSet(Of t_bienapimport)
    Public Overridable Property t_bienaplichsu() As DbSet(Of t_bienaplichsu)
    Public Overridable Property t_company() As DbSet(Of t_company)
    Public Overridable Property t_family() As DbSet(Of t_family)
    Public Overridable Property t_familyresult() As DbSet(Of t_familyresult)
    Public Overridable Property t_histcompany() As DbSet(Of t_histcompany)
    Public Overridable Property t_histfamily() As DbSet(Of t_histfamily)
    Public Overridable Property t_histfamilyresult() As DbSet(Of t_histfamilyresult)
    Public Overridable Property t_histprivate() As DbSet(Of t_histprivate)
    Public Overridable Property t_importdetaillog() As DbSet(Of t_importdetaillog)
    Public Overridable Property t_importlog() As DbSet(Of t_importlog)
    Public Overridable Property t_private() As DbSet(Of t_private)
    Public Overridable Property t_privateno() As DbSet(Of t_privateno)
    Public Overridable Property t_systemdetaillog() As DbSet(Of t_systemdetaillog)
    Public Overridable Property t_systemlog() As DbSet(Of t_systemlog)
    Public Overridable Property w_company() As DbSet(Of w_company)
    Public Overridable Property w_family() As DbSet(Of w_family)
    Public Overridable Property w_private() As DbSet(Of w_private)
    Public Overridable Property w_privateno() As DbSet(Of w_privateno)
    Public Overridable Property w_sumpostno() As DbSet(Of w_sumpostno)

End Class
