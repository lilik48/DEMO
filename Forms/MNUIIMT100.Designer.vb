<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMNUIIMT100
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
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCompanyCD = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.dtgrvCompany = New MyNo.CustomDataGridView()
        Me.btnAddNew = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtCompanyName = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtBranchName = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtBranchNo = New System.Windows.Forms.TextBox()
        Me.btnOutputList = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.dtgrvCompany, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("MS PGothic", 14.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.Location = New System.Drawing.Point(30, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(129, 19)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "企業情報検索"
        '
        'txtCompanyCD
        '
        Me.txtCompanyCD.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCompanyCD.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtCompanyCD.Location = New System.Drawing.Point(117, 57)
        Me.txtCompanyCD.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtCompanyCD.MaxLength = 5
        Me.txtCompanyCD.Name = "txtCompanyCD"
        Me.txtCompanyCD.Size = New System.Drawing.Size(200, 22)
        Me.txtCompanyCD.TabIndex = 1
        Me.txtCompanyCD.Tag = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("MS PGothic", 11.0!)
        Me.Label2.Location = New System.Drawing.Point(30, 96)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 15)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "枝番"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("MS PGothic", 11.0!)
        Me.Label1.Location = New System.Drawing.Point(30, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 15)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "企業コード"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("MS PGothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label5.Location = New System.Drawing.Point(1028, 96)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(182, 15)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "※前方一致、後方一致検索"
        '
        'btnClear
        '
        Me.btnClear.Font = New System.Drawing.Font("MS PGothic", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnClear.Location = New System.Drawing.Point(200, 131)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(150, 35)
        Me.btnClear.TabIndex = 6
        Me.btnClear.Text = "クリア (&D)"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("MS PGothic", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(30, 131)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(150, 35)
        Me.btnSearch.TabIndex = 5
        Me.btnSearch.Text = "検索 (&S)"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'dtgrvCompany
        '
        Me.dtgrvCompany.AllowUserToAddRows = False
        Me.dtgrvCompany.AllowUserToDeleteRows = False
        Me.dtgrvCompany.AllowUserToResizeRows = False
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle7.Font = New System.Drawing.Font("MS PGothic", 11.0!)
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dtgrvCompany.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.dtgrvCompany.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dtgrvCompany.Location = New System.Drawing.Point(30, 186)
        Me.dtgrvCompany.Name = "dtgrvCompany"
        Me.dtgrvCompany.ReadOnly = True
        Me.dtgrvCompany.RowHeadersVisible = False
        DataGridViewCellStyle8.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtgrvCompany.RowsDefaultCellStyle = DataGridViewCellStyle8
        Me.dtgrvCompany.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtgrvCompany.RowTemplate.Height = 20
        Me.dtgrvCompany.RowTemplate.ReadOnly = True
        Me.dtgrvCompany.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dtgrvCompany.Size = New System.Drawing.Size(1290, 465)
        Me.dtgrvCompany.TabIndex = 8
        '
        'btnAddNew
        '
        Me.btnAddNew.Font = New System.Drawing.Font("MS Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAddNew.Location = New System.Drawing.Point(30, 675)
        Me.btnAddNew.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnAddNew.Name = "btnAddNew"
        Me.btnAddNew.Size = New System.Drawing.Size(150, 35)
        Me.btnAddNew.TabIndex = 10
        Me.btnAddNew.Text = "新規追加 (&I)"
        Me.btnAddNew.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("MS Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnClose.Location = New System.Drawing.Point(1170, 675)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(150, 35)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "閉じる (&C)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("MS PGothic", 11.0!)
        Me.Label6.Location = New System.Drawing.Point(343, 61)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(52, 15)
        Me.Label6.TabIndex = 24
        Me.Label6.Text = "企業名"
        '
        'txtCompanyName
        '
        Me.txtCompanyName.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtCompanyName.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtCompanyName.Location = New System.Drawing.Point(408, 57)
        Me.txtCompanyName.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtCompanyName.MaxLength = 80
        Me.txtCompanyName.Name = "txtCompanyName"
        Me.txtCompanyName.Size = New System.Drawing.Size(600, 22)
        Me.txtCompanyName.TabIndex = 2
        Me.txtCompanyName.Tag = "1"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("MS PGothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label7.Location = New System.Drawing.Point(1028, 61)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(182, 15)
        Me.Label7.TabIndex = 26
        Me.Label7.Text = "※前方一致、後方一致検索"
        '
        'txtBranchName
        '
        Me.txtBranchName.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtBranchName.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtBranchName.Location = New System.Drawing.Point(408, 92)
        Me.txtBranchName.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtBranchName.MaxLength = 80
        Me.txtBranchName.Name = "txtBranchName"
        Me.txtBranchName.Size = New System.Drawing.Size(600, 22)
        Me.txtBranchName.TabIndex = 4
        Me.txtBranchName.Tag = "1"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("MS PGothic", 11.0!)
        Me.Label8.Location = New System.Drawing.Point(343, 96)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(52, 15)
        Me.Label8.TabIndex = 29
        Me.Label8.Text = "枝番名"
        '
        'txtBranchNo
        '
        Me.txtBranchNo.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtBranchNo.Location = New System.Drawing.Point(117, 92)
        Me.txtBranchNo.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtBranchNo.MaxLength = 3
        Me.txtBranchNo.Name = "txtBranchNo"
        Me.txtBranchNo.Size = New System.Drawing.Size(200, 22)
        Me.txtBranchNo.TabIndex = 3
        Me.txtBranchNo.Tag = "0"
        Me.txtBranchNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnOutputList
        '
        Me.btnOutputList.Font = New System.Drawing.Font("MS PGothic", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnOutputList.Location = New System.Drawing.Point(1170, 131)
        Me.btnOutputList.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnOutputList.Name = "btnOutputList"
        Me.btnOutputList.Size = New System.Drawing.Size(150, 35)
        Me.btnOutputList.TabIndex = 7
        Me.btnOutputList.Text = "進捗確認リスト出力"
        Me.btnOutputList.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("MS PGothic", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Button1.Location = New System.Drawing.Point(377, 131)
        Me.Button1.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(150, 35)
        Me.Button1.TabIndex = 30
        Me.Button1.Text = "Import data"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'frmMNUIIMT100
        '
        Me.AcceptButton = Me.btnSearch
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1350, 730)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnOutputList)
        Me.Controls.Add(Me.txtBranchName)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtBranchNo)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtCompanyName)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.dtgrvCompany)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnAddNew)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtCompanyCD)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Font = New System.Drawing.Font("MS PGothic", 11.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMNUIIMT100"
        Me.Padding = New System.Windows.Forms.Padding(30, 22, 30, 25)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MNサービス(ユーザID：op00001)"
        CType(Me.dtgrvCompany, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtCompanyCD As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnAddNew As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtCompanyName As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtBranchName As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtBranchNo As System.Windows.Forms.TextBox
    Friend WithEvents btnOutputList As System.Windows.Forms.Button
    Friend WithEvents dtgrvCompany As MyNo.CustomDataGridView
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
