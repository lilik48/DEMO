<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMNUIDTR100
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
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.企業コード = New System.Windows.Forms.Label()
        Me.lblCompanyBranchCD = New System.Windows.Forms.Label()
        Me.txtCompanyCD = New System.Windows.Forms.TextBox()
        Me.txtCompanyBranchCD = New System.Windows.Forms.TextBox()
        Me.optImportType1 = New System.Windows.Forms.RadioButton()
        Me.optImportType0 = New System.Windows.Forms.RadioButton()
        Me.optFileType1 = New System.Windows.Forms.RadioButton()
        Me.optFileType0 = New System.Windows.Forms.RadioButton()
        Me.txtFilePath = New System.Windows.Forms.TextBox()
        Me.btnFileChoose = New System.Windows.Forms.Button()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.dgvImportLog = New MyNo.CustomDataGridView()
        Me.seq = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dataImpDateTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dataImpUserCD = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dataFileName = New System.Windows.Forms.DataGridViewLinkColumn()
        Me.dataCompanyCD = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dataCompanyBranchNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.importType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fileType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dataResult = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dataError = New System.Windows.Forms.DataGridViewLinkColumn()
        Me.btnPrevious = New System.Windows.Forms.Button()
        Me.lblPage = New System.Windows.Forms.Label()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        CType(Me.dgvImportLog, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.Location = New System.Drawing.Point(30, 22)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(143, 19)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "基礎データ取込"
        '
        '企業コード
        '
        Me.企業コード.AutoSize = True
        Me.企業コード.Location = New System.Drawing.Point(30, 61)
        Me.企業コード.Name = "企業コード"
        Me.企業コード.Size = New System.Drawing.Size(99, 15)
        Me.企業コード.TabIndex = 1
        Me.企業コード.Text = "企業コード(※)"
        '
        'lblCompanyBranchCD
        '
        Me.lblCompanyBranchCD.AutoSize = True
        Me.lblCompanyBranchCD.Location = New System.Drawing.Point(30, 96)
        Me.lblCompanyBranchCD.Name = "lblCompanyBranchCD"
        Me.lblCompanyBranchCD.Size = New System.Drawing.Size(62, 15)
        Me.lblCompanyBranchCD.TabIndex = 2
        Me.lblCompanyBranchCD.Text = "枝番(※)"
        '
        'txtCompanyCD
        '
        Me.txtCompanyCD.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCompanyCD.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!)
        Me.txtCompanyCD.Location = New System.Drawing.Point(150, 57)
        Me.txtCompanyCD.MaxLength = 5
        Me.txtCompanyCD.Name = "txtCompanyCD"
        Me.txtCompanyCD.Size = New System.Drawing.Size(200, 22)
        Me.txtCompanyCD.TabIndex = 1
        '
        'txtCompanyBranchCD
        '
        Me.txtCompanyBranchCD.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!)
        Me.txtCompanyBranchCD.Location = New System.Drawing.Point(150, 92)
        Me.txtCompanyBranchCD.MaxLength = 3
        Me.txtCompanyBranchCD.Name = "txtCompanyBranchCD"
        Me.txtCompanyBranchCD.Size = New System.Drawing.Size(200, 22)
        Me.txtCompanyBranchCD.TabIndex = 2
        Me.txtCompanyBranchCD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'optImportType1
        '
        Me.optImportType1.AutoSize = True
        Me.optImportType1.Location = New System.Drawing.Point(214, 148)
        Me.optImportType1.Name = "optImportType1"
        Me.optImportType1.Size = New System.Drawing.Size(115, 19)
        Me.optImportType1.TabIndex = 4
        Me.optImportType1.TabStop = True
        Me.optImportType1.Text = "新規追加のみ"
        Me.optImportType1.UseVisualStyleBackColor = True
        '
        'optImportType0
        '
        Me.optImportType0.AutoSize = True
        Me.optImportType0.Checked = True
        Me.optImportType0.Location = New System.Drawing.Point(52, 148)
        Me.optImportType0.Name = "optImportType0"
        Me.optImportType0.Size = New System.Drawing.Size(125, 19)
        Me.optImportType0.TabIndex = 3
        Me.optImportType0.TabStop = True
        Me.optImportType0.Text = "全件(削除追加)"
        Me.optImportType0.UseVisualStyleBackColor = True
        '
        'optFileType1
        '
        Me.optFileType1.AutoSize = True
        Me.optFileType1.Location = New System.Drawing.Point(182, 10)
        Me.optFileType1.Name = "optFileType1"
        Me.optFileType1.Size = New System.Drawing.Size(55, 19)
        Me.optFileType1.TabIndex = 6
        Me.optFileType1.TabStop = True
        Me.optFileType1.Text = "家族"
        Me.optFileType1.UseVisualStyleBackColor = True
        '
        'optFileType0
        '
        Me.optFileType0.AutoSize = True
        Me.optFileType0.Checked = True
        Me.optFileType0.Location = New System.Drawing.Point(20, 10)
        Me.optFileType0.Name = "optFileType0"
        Me.optFileType0.Size = New System.Drawing.Size(55, 19)
        Me.optFileType0.TabIndex = 5
        Me.optFileType0.TabStop = True
        Me.optFileType0.Text = "個人"
        Me.optFileType0.UseVisualStyleBackColor = True
        '
        'txtFilePath
        '
        Me.txtFilePath.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!)
        Me.txtFilePath.Location = New System.Drawing.Point(30, 201)
        Me.txtFilePath.Name = "txtFilePath"
        Me.txtFilePath.Size = New System.Drawing.Size(1170, 22)
        Me.txtFilePath.TabIndex = 7
        '
        'btnFileChoose
        '
        Me.btnFileChoose.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnFileChoose.Location = New System.Drawing.Point(1220, 200)
        Me.btnFileChoose.Name = "btnFileChoose"
        Me.btnFileChoose.Size = New System.Drawing.Size(100, 23)
        Me.btnFileChoose.TabIndex = 8
        Me.btnFileChoose.Text = "参照"
        Me.btnFileChoose.UseVisualStyleBackColor = True
        '
        'btnImport
        '
        Me.btnImport.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnImport.Location = New System.Drawing.Point(1220, 243)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(100, 35)
        Me.btnImport.TabIndex = 9
        Me.btnImport.Text = "実行(&R)"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'dgvImportLog
        '
        Me.dgvImportLog.AllowUserToAddRows = False
        Me.dgvImportLog.AllowUserToDeleteRows = False
        Me.dgvImportLog.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.0!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvImportLog.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvImportLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvImportLog.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.seq, Me.dataImpDateTime, Me.dataImpUserCD, Me.dataFileName, Me.dataCompanyCD, Me.dataCompanyBranchNo, Me.importType, Me.fileType, Me.dataResult, Me.dataError})
        Me.dgvImportLog.Location = New System.Drawing.Point(30, 298)
        Me.dgvImportLog.Name = "dgvImportLog"
        Me.dgvImportLog.ReadOnly = True
        Me.dgvImportLog.RowHeadersVisible = False
        Me.dgvImportLog.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.dgvImportLog.RowTemplate.Height = 20
        Me.dgvImportLog.Size = New System.Drawing.Size(1290, 365)
        Me.dgvImportLog.StandardTab = True
        Me.dgvImportLog.TabIndex = 10
        '
        'seq
        '
        Me.seq.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.seq.DataPropertyName = "seq"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!)
        Me.seq.DefaultCellStyle = DataGridViewCellStyle2
        Me.seq.HeaderText = "SEQ"
        Me.seq.Name = "seq"
        Me.seq.ReadOnly = True
        Me.seq.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'dataImpDateTime
        '
        Me.dataImpDateTime.DataPropertyName = "dataImpDateTime"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!)
        DataGridViewCellStyle3.Format = "yyyy/MM/dd HH:mm"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.dataImpDateTime.DefaultCellStyle = DataGridViewCellStyle3
        Me.dataImpDateTime.FillWeight = 94.30569!
        Me.dataImpDateTime.HeaderText = "取込日時"
        Me.dataImpDateTime.Name = "dataImpDateTime"
        Me.dataImpDateTime.ReadOnly = True
        Me.dataImpDateTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.dataImpDateTime.Width = 170
        '
        'dataImpUserCD
        '
        Me.dataImpUserCD.DataPropertyName = "dataImpUserCD"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!)
        Me.dataImpUserCD.DefaultCellStyle = DataGridViewCellStyle4
        Me.dataImpUserCD.FillWeight = 94.30569!
        Me.dataImpUserCD.HeaderText = "取込担当者ID"
        Me.dataImpUserCD.Name = "dataImpUserCD"
        Me.dataImpUserCD.ReadOnly = True
        Me.dataImpUserCD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.dataImpUserCD.Width = 125
        '
        'dataFileName
        '
        Me.dataFileName.DataPropertyName = "dataFileName"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!)
        Me.dataFileName.DefaultCellStyle = DataGridViewCellStyle5
        Me.dataFileName.FillWeight = 94.30569!
        Me.dataFileName.HeaderText = "ファイル"
        Me.dataFileName.Name = "dataFileName"
        Me.dataFileName.ReadOnly = True
        Me.dataFileName.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dataFileName.Width = 339
        '
        'dataCompanyCD
        '
        Me.dataCompanyCD.DataPropertyName = "dataCompanyCD"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!)
        Me.dataCompanyCD.DefaultCellStyle = DataGridViewCellStyle6
        Me.dataCompanyCD.FillWeight = 94.30569!
        Me.dataCompanyCD.HeaderText = "企業コード"
        Me.dataCompanyCD.Name = "dataCompanyCD"
        Me.dataCompanyCD.ReadOnly = True
        Me.dataCompanyCD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.dataCompanyCD.Width = 80
        '
        'dataCompanyBranchNo
        '
        Me.dataCompanyBranchNo.DataPropertyName = "dataCompanyBranchNo"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!)
        Me.dataCompanyBranchNo.DefaultCellStyle = DataGridViewCellStyle7
        Me.dataCompanyBranchNo.FillWeight = 94.30569!
        Me.dataCompanyBranchNo.HeaderText = "枝番"
        Me.dataCompanyBranchNo.Name = "dataCompanyBranchNo"
        Me.dataCompanyBranchNo.ReadOnly = True
        Me.dataCompanyBranchNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.dataCompanyBranchNo.Width = 65
        '
        'importType
        '
        Me.importType.DataPropertyName = "importType"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle8.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!)
        Me.importType.DefaultCellStyle = DataGridViewCellStyle8
        Me.importType.FillWeight = 94.30569!
        Me.importType.HeaderText = "全件/新規"
        Me.importType.Name = "importType"
        Me.importType.ReadOnly = True
        Me.importType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.importType.Width = 105
        '
        'fileType
        '
        Me.fileType.DataPropertyName = "fileType"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle9.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fileType.DefaultCellStyle = DataGridViewCellStyle9
        Me.fileType.HeaderText = "本人/家族"
        Me.fileType.Name = "fileType"
        Me.fileType.ReadOnly = True
        Me.fileType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.fileType.Width = 105
        '
        'dataResult
        '
        Me.dataResult.DataPropertyName = "dataResult"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle10.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!)
        Me.dataResult.DefaultCellStyle = DataGridViewCellStyle10
        Me.dataResult.FillWeight = 139.8601!
        Me.dataResult.HeaderText = "結果"
        Me.dataResult.Name = "dataResult"
        Me.dataResult.ReadOnly = True
        Me.dataResult.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.dataResult.Width = 65
        '
        'dataError
        '
        Me.dataError.DataPropertyName = "dataError"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle11.Font = New System.Drawing.Font("ＭＳ 明朝", 11.0!)
        Me.dataError.DefaultCellStyle = DataGridViewCellStyle11
        Me.dataError.FillWeight = 94.30569!
        Me.dataError.HeaderText = "エラーリスト"
        Me.dataError.Name = "dataError"
        Me.dataError.ReadOnly = True
        Me.dataError.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dataError.Width = 133
        '
        'btnPrevious
        '
        Me.btnPrevious.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrevious.Location = New System.Drawing.Point(30, 669)
        Me.btnPrevious.Name = "btnPrevious"
        Me.btnPrevious.Size = New System.Drawing.Size(25, 23)
        Me.btnPrevious.TabIndex = 11
        Me.btnPrevious.Tag = "0"
        Me.btnPrevious.Text = "&<"
        Me.btnPrevious.UseVisualStyleBackColor = True
        '
        'lblPage
        '
        Me.lblPage.AutoSize = True
        Me.lblPage.Location = New System.Drawing.Point(61, 673)
        Me.lblPage.Name = "lblPage"
        Me.lblPage.Size = New System.Drawing.Size(15, 15)
        Me.lblPage.TabIndex = 12
        Me.lblPage.Text = "1"
        Me.lblPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnNext
        '
        Me.btnNext.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnNext.Location = New System.Drawing.Point(82, 669)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(25, 23)
        Me.btnNext.TabIndex = 13
        Me.btnNext.Tag = "1"
        Me.btnNext.Text = "&>"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnClose.Location = New System.Drawing.Point(1220, 681)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(100, 35)
        Me.btnClose.TabIndex = 14
        Me.btnClose.Text = "閉じる(&C)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.optFileType1)
        Me.Panel1.Controls.Add(Me.optFileType0)
        Me.Panel1.Location = New System.Drawing.Point(373, 138)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(270, 39)
        Me.Panel1.TabIndex = 5
        '
        'frmMNUIDTR100
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1350, 730)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.optImportType0)
        Me.Controls.Add(Me.optImportType1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.lblPage)
        Me.Controls.Add(Me.btnPrevious)
        Me.Controls.Add(Me.dgvImportLog)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.btnFileChoose)
        Me.Controls.Add(Me.txtFilePath)
        Me.Controls.Add(Me.txtCompanyBranchCD)
        Me.Controls.Add(Me.txtCompanyCD)
        Me.Controls.Add(Me.lblCompanyBranchCD)
        Me.Controls.Add(Me.企業コード)
        Me.Controls.Add(Me.lblTitle)
        Me.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMNUIDTR100"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MNサービス(Tên đăng nhập：op00001)"
        CType(Me.dgvImportLog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents 企業コード As System.Windows.Forms.Label
    Friend WithEvents lblCompanyBranchCD As System.Windows.Forms.Label
    Friend WithEvents txtCompanyCD As System.Windows.Forms.TextBox
    Friend WithEvents txtCompanyBranchCD As System.Windows.Forms.TextBox
    Friend WithEvents optImportType0 As System.Windows.Forms.RadioButton
    Friend WithEvents optImportType1 As System.Windows.Forms.RadioButton
    Friend WithEvents optFileType1 As System.Windows.Forms.RadioButton
    Friend WithEvents optFileType0 As System.Windows.Forms.RadioButton
    Friend WithEvents txtFilePath As System.Windows.Forms.TextBox
    Friend WithEvents btnFileChoose As System.Windows.Forms.Button
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents btnPrevious As System.Windows.Forms.Button
    Friend WithEvents lblPage As System.Windows.Forms.Label
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dgvImportLog As MyNo.CustomDataGridView
    Friend WithEvents seq As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dataImpDateTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dataImpUserCD As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dataFileName As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents dataCompanyCD As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dataCompanyBranchNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents importType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents fileType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dataResult As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dataError As System.Windows.Forms.DataGridViewLinkColumn
End Class
