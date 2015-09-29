<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMNUIDTR200
    Inherits BaseForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblImport = New System.Windows.Forms.Label()
        Me.txtPathFile = New System.Windows.Forms.TextBox()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.btnFileChoose = New System.Windows.Forms.Button()
        Me.lblTitleExport = New System.Windows.Forms.Label()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.lblPage = New System.Windows.Forms.Label()
        Me.btnPrevious = New System.Windows.Forms.Button()
        Me.txtBranchEnd = New System.Windows.Forms.TextBox()
        Me.txtBranchStart = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtBranchCd = New System.Windows.Forms.TextBox()
        Me.txtCompanyCd = New System.Windows.Forms.TextBox()
        Me.lblCompanyBranchCD = New System.Windows.Forms.Label()
        Me.企業コード = New System.Windows.Forms.Label()
        Me.dgvImportLog = New MyNo.CustomDataGridView()
        Me.seq = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.impdatetime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.impusercd = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.filename = New System.Windows.Forms.DataGridViewLinkColumn()
        Me.imptype = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.errorFlg = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dowloadFlg = New System.Windows.Forms.DataGridViewLinkColumn()
        CType(Me.dgvImportLog, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("MS PGothic", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.Location = New System.Drawing.Point(40, 22)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(129, 19)
        Me.lblTitle.TabIndex = 2
        Me.lblTitle.Text = "企業情報取込"
        '
        'lblImport
        '
        Me.lblImport.AutoSize = True
        Me.lblImport.Font = New System.Drawing.Font("MS PGothic", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblImport.Location = New System.Drawing.Point(40, 61)
        Me.lblImport.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblImport.Name = "lblImport"
        Me.lblImport.Size = New System.Drawing.Size(93, 19)
        Me.lblImport.TabIndex = 3
        Me.lblImport.Text = "インポート"
        '
        'txtPathFile
        '
        Me.txtPathFile.Font = New System.Drawing.Font("MS Mincho", 11.0!)
        Me.txtPathFile.Location = New System.Drawing.Point(40, 100)
        Me.txtPathFile.Name = "txtPathFile"
        Me.txtPathFile.Size = New System.Drawing.Size(1096, 22)
        Me.txtPathFile.TabIndex = 1
        '
        'btnImport
        '
        Me.btnImport.Font = New System.Drawing.Font("MS PGothic", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnImport.Location = New System.Drawing.Point(1207, 142)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(100, 35)
        Me.btnImport.TabIndex = 3
        Me.btnImport.Text = "実行(&R)"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'btnFileChoose
        '
        Me.btnFileChoose.Font = New System.Drawing.Font("MS PGothic", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnFileChoose.Location = New System.Drawing.Point(1207, 97)
        Me.btnFileChoose.Name = "btnFileChoose"
        Me.btnFileChoose.Size = New System.Drawing.Size(100, 25)
        Me.btnFileChoose.TabIndex = 2
        Me.btnFileChoose.Text = "参照"
        Me.btnFileChoose.UseVisualStyleBackColor = True
        '
        'lblTitleExport
        '
        Me.lblTitleExport.AutoSize = True
        Me.lblTitleExport.Font = New System.Drawing.Font("MS PGothic", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitleExport.Location = New System.Drawing.Point(40, 487)
        Me.lblTitleExport.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTitleExport.Name = "lblTitleExport"
        Me.lblTitleExport.Size = New System.Drawing.Size(112, 19)
        Me.lblTitleExport.TabIndex = 35
        Me.lblTitleExport.Text = "エクスポート"
        '
        'btnClear
        '
        Me.btnClear.Font = New System.Drawing.Font("MS PGothic", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnClear.Location = New System.Drawing.Point(40, 596)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(100, 35)
        Me.btnClear.TabIndex = 12
        Me.btnClear.Text = "クリア(&D)"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Font = New System.Drawing.Font("MS PGothic", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnExport.Location = New System.Drawing.Point(160, 596)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 35)
        Me.btnExport.TabIndex = 13
        Me.btnExport.Text = "実行(&E)"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("MS PGothic", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnClose.Location = New System.Drawing.Point(1228, 669)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(100, 35)
        Me.btnClose.TabIndex = 14
        Me.btnClose.Text = "閉じる（&C）"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.Font = New System.Drawing.Font("MS PGothic", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnNext.Location = New System.Drawing.Point(93, 437)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(25, 23)
        Me.btnNext.TabIndex = 7
        Me.btnNext.Tag = "1"
        Me.btnNext.Text = "&>"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'lblPage
        '
        Me.lblPage.AutoSize = True
        Me.lblPage.Location = New System.Drawing.Point(72, 441)
        Me.lblPage.Name = "lblPage"
        Me.lblPage.Size = New System.Drawing.Size(15, 15)
        Me.lblPage.TabIndex = 6
        Me.lblPage.Text = "1"
        Me.lblPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnPrevious
        '
        Me.btnPrevious.Font = New System.Drawing.Font("MS PGothic", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrevious.Location = New System.Drawing.Point(40, 437)
        Me.btnPrevious.Name = "btnPrevious"
        Me.btnPrevious.Size = New System.Drawing.Size(25, 23)
        Me.btnPrevious.TabIndex = 5
        Me.btnPrevious.Tag = "0"
        Me.btnPrevious.Text = "&<"
        Me.btnPrevious.UseVisualStyleBackColor = True
        '
        'txtBranchEnd
        '
        Me.txtBranchEnd.Font = New System.Drawing.Font("MS Mincho", 11.0!)
        Me.txtBranchEnd.Location = New System.Drawing.Point(583, 557)
        Me.txtBranchEnd.MaxLength = 3
        Me.txtBranchEnd.Name = "txtBranchEnd"
        Me.txtBranchEnd.Size = New System.Drawing.Size(100, 22)
        Me.txtBranchEnd.TabIndex = 11
        Me.txtBranchEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtBranchStart
        '
        Me.txtBranchStart.Font = New System.Drawing.Font("MS Mincho", 11.0!)
        Me.txtBranchStart.Location = New System.Drawing.Point(431, 557)
        Me.txtBranchStart.MaxLength = 3
        Me.txtBranchStart.Name = "txtBranchStart"
        Me.txtBranchStart.Size = New System.Drawing.Size(100, 22)
        Me.txtBranchStart.TabIndex = 10
        Me.txtBranchStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(407, 561)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(22, 15)
        Me.Label9.TabIndex = 56
        Me.Label9.Text = "自"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(559, 561)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(22, 15)
        Me.Label13.TabIndex = 55
        Me.Label13.Text = "至"
        '
        'txtBranchCd
        '
        Me.txtBranchCd.Font = New System.Drawing.Font("MS Mincho", 11.0!)
        Me.txtBranchCd.Location = New System.Drawing.Point(160, 557)
        Me.txtBranchCd.MaxLength = 100
        Me.txtBranchCd.Name = "txtBranchCd"
        Me.txtBranchCd.Size = New System.Drawing.Size(200, 22)
        Me.txtBranchCd.TabIndex = 9
        Me.txtBranchCd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCompanyCd
        '
        Me.txtCompanyCd.Font = New System.Drawing.Font("MS Mincho", 11.0!)
        Me.txtCompanyCd.Location = New System.Drawing.Point(160, 522)
        Me.txtCompanyCd.MaxLength = 5
        Me.txtCompanyCd.Name = "txtCompanyCd"
        Me.txtCompanyCd.Size = New System.Drawing.Size(200, 22)
        Me.txtCompanyCd.TabIndex = 8
        '
        'lblCompanyBranchCD
        '
        Me.lblCompanyBranchCD.AutoSize = True
        Me.lblCompanyBranchCD.Location = New System.Drawing.Point(40, 561)
        Me.lblCompanyBranchCD.Name = "lblCompanyBranchCD"
        Me.lblCompanyBranchCD.Size = New System.Drawing.Size(37, 15)
        Me.lblCompanyBranchCD.TabIndex = 54
        Me.lblCompanyBranchCD.Text = "枝番"
        '
        '企業コード
        '
        Me.企業コード.AutoSize = True
        Me.企業コード.Location = New System.Drawing.Point(40, 526)
        Me.企業コード.Name = "企業コード"
        Me.企業コード.Size = New System.Drawing.Size(99, 15)
        Me.企業コード.TabIndex = 52
        Me.企業コード.Text = "企業コード(※)"
        '
        'dgvImportLog
        '
        Me.dgvImportLog.AllowUserToAddRows = False
        Me.dgvImportLog.AllowUserToDeleteRows = False
        Me.dgvImportLog.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("MS PGothic", 11.0!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvImportLog.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvImportLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvImportLog.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.seq, Me.impdatetime, Me.impusercd, Me.filename, Me.imptype, Me.errorFlg, Me.dowloadFlg})
        Me.dgvImportLog.Location = New System.Drawing.Point(40, 197)
        Me.dgvImportLog.Name = "dgvImportLog"
        Me.dgvImportLog.ReadOnly = True
        Me.dgvImportLog.RowHeadersVisible = False
        Me.dgvImportLog.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvImportLog.RowTemplate.Height = 20
        Me.dgvImportLog.Size = New System.Drawing.Size(1265, 226)
        Me.dgvImportLog.TabIndex = 4
        '
        'seq
        '
        Me.seq.DataPropertyName = "seq"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.seq.DefaultCellStyle = DataGridViewCellStyle2
        Me.seq.HeaderText = "SEQ"
        Me.seq.Name = "seq"
        Me.seq.ReadOnly = True
        Me.seq.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.seq.Width = 112
        '
        'impdatetime
        '
        Me.impdatetime.DataPropertyName = "impdatetime"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.impdatetime.DefaultCellStyle = DataGridViewCellStyle3
        Me.impdatetime.HeaderText = "取込日時"
        Me.impdatetime.Name = "impdatetime"
        Me.impdatetime.ReadOnly = True
        Me.impdatetime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.impdatetime.Width = 170
        '
        'impusercd
        '
        Me.impusercd.DataPropertyName = "impusercd"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.impusercd.DefaultCellStyle = DataGridViewCellStyle4
        Me.impusercd.HeaderText = "取込担当者コード"
        Me.impusercd.Name = "impusercd"
        Me.impusercd.ReadOnly = True
        Me.impusercd.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.impusercd.Width = 235
        '
        'filename
        '
        Me.filename.DataPropertyName = "filename"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.filename.DefaultCellStyle = DataGridViewCellStyle5
        Me.filename.HeaderText = "ファイル"
        Me.filename.Name = "filename"
        Me.filename.ReadOnly = True
        Me.filename.Width = 455
        '
        'imptype
        '
        Me.imptype.DataPropertyName = "ImportType"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle6.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.imptype.DefaultCellStyle = DataGridViewCellStyle6
        Me.imptype.HeaderText = "区分"
        Me.imptype.Name = "imptype"
        Me.imptype.ReadOnly = True
        Me.imptype.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.imptype.Width = 80
        '
        'errorFlg
        '
        Me.errorFlg.DataPropertyName = "ErrFlg"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle7.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.errorFlg.DefaultCellStyle = DataGridViewCellStyle7
        Me.errorFlg.HeaderText = "結果"
        Me.errorFlg.Name = "errorFlg"
        Me.errorFlg.ReadOnly = True
        Me.errorFlg.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.errorFlg.Width = 60
        '
        'dowloadFlg
        '
        Me.dowloadFlg.DataPropertyName = "DownloadFlg"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle8.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.dowloadFlg.DefaultCellStyle = DataGridViewCellStyle8
        Me.dowloadFlg.HeaderText = "エラーリスト"
        Me.dowloadFlg.Name = "dowloadFlg"
        Me.dowloadFlg.ReadOnly = True
        Me.dowloadFlg.Width = 150
        '
        'frmMNUIDTR200
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1350, 730)
        Me.Controls.Add(Me.txtBranchEnd)
        Me.Controls.Add(Me.txtBranchStart)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.txtBranchCd)
        Me.Controls.Add(Me.txtCompanyCd)
        Me.Controls.Add(Me.lblCompanyBranchCD)
        Me.Controls.Add(Me.企業コード)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.lblPage)
        Me.Controls.Add(Me.btnPrevious)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.lblTitleExport)
        Me.Controls.Add(Me.dgvImportLog)
        Me.Controls.Add(Me.btnFileChoose)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.txtPathFile)
        Me.Controls.Add(Me.lblImport)
        Me.Controls.Add(Me.lblTitle)
        Me.Font = New System.Drawing.Font("MS PGothic", 11.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMNUIDTR200"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MNサービス(Tên đăng nhập：op00001)"
        CType(Me.dgvImportLog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblImport As System.Windows.Forms.Label
    Friend WithEvents txtPathFile As System.Windows.Forms.TextBox
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents btnFileChoose As System.Windows.Forms.Button
    Friend WithEvents lblTitleExport As System.Windows.Forms.Label
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents lblPage As System.Windows.Forms.Label
    Friend WithEvents btnPrevious As System.Windows.Forms.Button
    Friend WithEvents txtBranchEnd As System.Windows.Forms.TextBox
    Friend WithEvents txtBranchStart As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtBranchCd As System.Windows.Forms.TextBox
    Friend WithEvents txtCompanyCd As System.Windows.Forms.TextBox
    Friend WithEvents lblCompanyBranchCD As System.Windows.Forms.Label
    Friend WithEvents 企業コード As System.Windows.Forms.Label
    Friend WithEvents dgvImportLog As MyNo.CustomDataGridView
    Friend WithEvents seq As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents impdatetime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents impusercd As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents filename As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents imptype As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents errorFlg As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dowloadFlg As System.Windows.Forms.DataGridViewLinkColumn
End Class
