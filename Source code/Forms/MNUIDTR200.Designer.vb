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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMNUIDTR200))
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblImport = New System.Windows.Forms.Label()
        Me.txtPathFile = New System.Windows.Forms.TextBox()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.btnFileChoose = New System.Windows.Forms.Button()
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
        Me.lblTitle.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.Location = New System.Drawing.Point(19, 8)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(117, 18)
        Me.lblTitle.TabIndex = 2
        Me.lblTitle.Text = "Thông tin nhập"
        '
        'lblImport
        '
        Me.lblImport.AutoSize = True
        Me.lblImport.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.lblImport.Location = New System.Drawing.Point(19, 32)
        Me.lblImport.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblImport.Name = "lblImport"
        Me.lblImport.Size = New System.Drawing.Size(46, 16)
        Me.lblImport.TabIndex = 3
        Me.lblImport.Text = "Import"
        '
        'txtPathFile
        '
        Me.txtPathFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPathFile.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.txtPathFile.Location = New System.Drawing.Point(19, 53)
        Me.txtPathFile.Name = "txtPathFile"
        Me.txtPathFile.Size = New System.Drawing.Size(989, 23)
        Me.txtPathFile.TabIndex = 1
        '
        'btnImport
        '
        Me.btnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImport.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnImport.Location = New System.Drawing.Point(1073, 86)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(100, 35)
        Me.btnImport.TabIndex = 3
        Me.btnImport.Text = "Import"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'btnFileChoose
        '
        Me.btnFileChoose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFileChoose.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFileChoose.Location = New System.Drawing.Point(1073, 52)
        Me.btnFileChoose.Name = "btnFileChoose"
        Me.btnFileChoose.Size = New System.Drawing.Size(100, 25)
        Me.btnFileChoose.TabIndex = 2
        Me.btnFileChoose.Text = "Chọn file"
        Me.btnFileChoose.UseVisualStyleBackColor = True
        '
        'dgvImportLog
        '
        Me.dgvImportLog.AllowUserToAddRows = False
        Me.dgvImportLog.AllowUserToDeleteRows = False
        Me.dgvImportLog.AllowUserToResizeRows = False
        Me.dgvImportLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        Me.dgvImportLog.Location = New System.Drawing.Point(19, 127)
        Me.dgvImportLog.Name = "dgvImportLog"
        Me.dgvImportLog.ReadOnly = True
        Me.dgvImportLog.RowHeadersVisible = False
        Me.dgvImportLog.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.dgvImportLog.RowTemplate.Height = 20
        Me.dgvImportLog.Size = New System.Drawing.Size(1154, 332)
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
        Me.ClientSize = New System.Drawing.Size(1191, 471)
        Me.Controls.Add(Me.dgvImportLog)
        Me.Controls.Add(Me.btnFileChoose)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.txtPathFile)
        Me.Controls.Add(Me.lblImport)
        Me.Controls.Add(Me.lblTitle)
        Me.Font = New System.Drawing.Font("MS PGothic", 11.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
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
    Friend WithEvents dgvImportLog As MyNo.CustomDataGridView
    Friend WithEvents seq As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents impdatetime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents impusercd As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents filename As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents imptype As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents errorFlg As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dowloadFlg As System.Windows.Forms.DataGridViewLinkColumn
End Class
