<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMNUIMTN400
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
        Me.cboAuthorityCD = New MyNo.CustomCombobox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtUserCD = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnAddNew = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.dgvUser = New MyNo.CustomDataGridView()
        Me.UserCD = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UserName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.AuthorityName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.詳細 = New System.Windows.Forms.DataGridViewLinkColumn()
        Me.AuthorityCD = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        CType(Me.dgvUser, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboAuthorityCD
        '
        Me.cboAuthorityCD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboAuthorityCD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAuthorityCD.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.cboAuthorityCD.FormattingEnabled = True
        Me.cboAuthorityCD.Location = New System.Drawing.Point(129, 80)
        Me.cboAuthorityCD.Name = "cboAuthorityCD"
        Me.cboAuthorityCD.Size = New System.Drawing.Size(300, 24)
        Me.cboAuthorityCD.TabIndex = 3
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.txtName.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtName.Location = New System.Drawing.Point(129, 51)
        Me.txtName.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtName.MaxLength = 3276
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(785, 23)
        Me.txtName.TabIndex = 2
        '
        'txtUserCD
        '
        Me.txtUserCD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUserCD.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.txtUserCD.Location = New System.Drawing.Point(129, 23)
        Me.txtUserCD.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtUserCD.MaxLength = 20
        Me.txtUserCD.Name = "txtUserCD"
        Me.txtUserCD.Size = New System.Drawing.Size(785, 23)
        Me.txtUserCD.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.Label4.Location = New System.Drawing.Point(9, 84)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 16)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Nhóm quyền"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.Label2.Location = New System.Drawing.Point(9, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(98, 16)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Tên người dùng"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(9, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 16)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Tên đăng nhập"
        '
        'btnClear
        '
        Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClear.Location = New System.Drawing.Point(814, 112)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(100, 35)
        Me.btnClear.TabIndex = 5
        Me.btnClear.Text = "Xóa (&X)"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSearch.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(689, 112)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(114, 35)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.Text = "Tìm kiếm (&T)"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnAddNew
        '
        Me.btnAddNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddNew.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddNew.Location = New System.Drawing.Point(9, 460)
        Me.btnAddNew.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnAddNew.Name = "btnAddNew"
        Me.btnAddNew.Size = New System.Drawing.Size(225, 35)
        Me.btnAddNew.TabIndex = 7
        Me.btnAddNew.Text = "Đăng ký người dùng mới(&M)"
        Me.btnAddNew.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(783, 460)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(150, 35)
        Me.btnClose.TabIndex = 8
        Me.btnClose.Text = "Đóng(&D)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'dgvUser
        '
        Me.dgvUser.AllowUserToAddRows = False
        Me.dgvUser.AllowUserToDeleteRows = False
        Me.dgvUser.AllowUserToResizeRows = False
        Me.dgvUser.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 9.75!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvUser.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUser.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.UserCD, Me.UserName, Me.AuthorityName, Me.詳細, Me.AuthorityCD})
        Me.dgvUser.Location = New System.Drawing.Point(9, 166)
        Me.dgvUser.Name = "dgvUser"
        Me.dgvUser.ReadOnly = True
        Me.dgvUser.RowHeadersVisible = False
        Me.dgvUser.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.dgvUser.RowTemplate.Height = 20
        Me.dgvUser.RowTemplate.ReadOnly = True
        Me.dgvUser.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvUser.Size = New System.Drawing.Size(924, 286)
        Me.dgvUser.TabIndex = 6
        '
        'UserCD
        '
        Me.UserCD.DataPropertyName = "UserCD"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.Font = New System.Drawing.Font("MS Mincho", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.UserCD.DefaultCellStyle = DataGridViewCellStyle2
        Me.UserCD.HeaderText = "Tên đăng nhập"
        Me.UserCD.Name = "UserCD"
        Me.UserCD.ReadOnly = True
        Me.UserCD.Width = 150
        '
        'UserName
        '
        Me.UserName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.UserName.DataPropertyName = "UserName"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.Font = New System.Drawing.Font("MS Mincho", 11.0!)
        Me.UserName.DefaultCellStyle = DataGridViewCellStyle3
        Me.UserName.HeaderText = "Tên người dùng"
        Me.UserName.Name = "UserName"
        Me.UserName.ReadOnly = True
        '
        'AuthorityName
        '
        Me.AuthorityName.DataPropertyName = "AuthorityName"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.Font = New System.Drawing.Font("MS Mincho", 11.0!)
        Me.AuthorityName.DefaultCellStyle = DataGridViewCellStyle4
        Me.AuthorityName.HeaderText = "Nhóm quyền"
        Me.AuthorityName.Name = "AuthorityName"
        Me.AuthorityName.ReadOnly = True
        Me.AuthorityName.Width = 200
        '
        '詳細
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.Font = New System.Drawing.Font("MS Mincho", 11.0!)
        Me.詳細.DefaultCellStyle = DataGridViewCellStyle5
        Me.詳細.HeaderText = "Xem chi tiết"
        Me.詳細.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline
        Me.詳細.Name = "詳細"
        Me.詳細.ReadOnly = True
        Me.詳細.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.詳細.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.詳細.Text = "Chi tiết"
        Me.詳細.ToolTipText = "Chi tiết"
        Me.詳細.UseColumnTextForLinkValue = True
        Me.詳細.Width = 90
        '
        'AuthorityCD
        '
        Me.AuthorityCD.HeaderText = "AuthorityCD"
        Me.AuthorityCD.Name = "AuthorityCD"
        Me.AuthorityCD.ReadOnly = True
        Me.AuthorityCD.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.btnClear)
        Me.GroupBox1.Controls.Add(Me.txtUserCD)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Controls.Add(Me.txtName)
        Me.GroupBox1.Controls.Add(Me.cboAuthorityCD)
        Me.GroupBox1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(9, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(924, 156)
        Me.GroupBox1.TabIndex = 16
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Tìm kiếm người dùng"
        '
        'frmMNUIMTN400
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(943, 505)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.dgvUser)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnAddNew)
        Me.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.Name = "frmMNUIMTN400"
        Me.Padding = New System.Windows.Forms.Padding(30, 22, 30, 25)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MNサービス(Tên đăng nhập：op00001)"
        CType(Me.dgvUser, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtUserCD As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnAddNew As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents dgvUser As MyNo.CustomDataGridView
    Friend WithEvents cboAuthorityCD As MyNo.CustomCombobox
    Friend WithEvents UserCD As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UserName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AuthorityName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 詳細 As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents AuthorityCD As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
End Class
