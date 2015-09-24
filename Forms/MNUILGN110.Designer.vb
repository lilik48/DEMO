<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMNUILGN110
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
        Me.btnFinish = New System.Windows.Forms.Button()
        Me.btnChangePassword = New System.Windows.Forms.Button()
        Me.txtPasswordHistory = New System.Windows.Forms.TextBox()
        Me.txtPasswordNew = New System.Windows.Forms.TextBox()
        Me.txtPasswordConfirm = New System.Windows.Forms.TextBox()
        Me.txtUserCD = New System.Windows.Forms.TextBox()
        Me.lblPasswordConfirm = New System.Windows.Forms.Label()
        Me.lblPasswordHistory = New System.Windows.Forms.Label()
        Me.lblPasswordNew = New System.Windows.Forms.Label()
        Me.lblUserCD = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnFinish
        '
        Me.btnFinish.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFinish.Location = New System.Drawing.Point(317, 173)
        Me.btnFinish.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnFinish.Name = "btnFinish"
        Me.btnFinish.Size = New System.Drawing.Size(101, 35)
        Me.btnFinish.TabIndex = 6
        Me.btnFinish.Text = "Đóng"
        Me.btnFinish.UseVisualStyleBackColor = True
        '
        'btnChangePassword
        '
        Me.btnChangePassword.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChangePassword.Location = New System.Drawing.Point(197, 173)
        Me.btnChangePassword.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnChangePassword.Name = "btnChangePassword"
        Me.btnChangePassword.Size = New System.Drawing.Size(101, 35)
        Me.btnChangePassword.TabIndex = 5
        Me.btnChangePassword.Text = "Cập nhật"
        Me.btnChangePassword.UseVisualStyleBackColor = True
        '
        'txtPasswordHistory
        '
        Me.txtPasswordHistory.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.txtPasswordHistory.Location = New System.Drawing.Point(167, 61)
        Me.txtPasswordHistory.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPasswordHistory.MaxLength = 20
        Me.txtPasswordHistory.Name = "txtPasswordHistory"
        Me.txtPasswordHistory.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPasswordHistory.Size = New System.Drawing.Size(250, 23)
        Me.txtPasswordHistory.TabIndex = 2
        '
        'txtPasswordNew
        '
        Me.txtPasswordNew.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.txtPasswordNew.Location = New System.Drawing.Point(167, 96)
        Me.txtPasswordNew.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPasswordNew.MaxLength = 20
        Me.txtPasswordNew.Name = "txtPasswordNew"
        Me.txtPasswordNew.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPasswordNew.Size = New System.Drawing.Size(250, 23)
        Me.txtPasswordNew.TabIndex = 3
        '
        'txtPasswordConfirm
        '
        Me.txtPasswordConfirm.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.txtPasswordConfirm.Location = New System.Drawing.Point(167, 131)
        Me.txtPasswordConfirm.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPasswordConfirm.MaxLength = 20
        Me.txtPasswordConfirm.Name = "txtPasswordConfirm"
        Me.txtPasswordConfirm.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPasswordConfirm.Size = New System.Drawing.Size(250, 23)
        Me.txtPasswordConfirm.TabIndex = 4
        '
        'txtUserCD
        '
        Me.txtUserCD.Enabled = False
        Me.txtUserCD.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.txtUserCD.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtUserCD.Location = New System.Drawing.Point(167, 26)
        Me.txtUserCD.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtUserCD.MaxLength = 20
        Me.txtUserCD.Name = "txtUserCD"
        Me.txtUserCD.Size = New System.Drawing.Size(250, 23)
        Me.txtUserCD.TabIndex = 1
        '
        'lblPasswordConfirm
        '
        Me.lblPasswordConfirm.AutoSize = True
        Me.lblPasswordConfirm.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.lblPasswordConfirm.Location = New System.Drawing.Point(27, 135)
        Me.lblPasswordConfirm.Name = "lblPasswordConfirm"
        Me.lblPasswordConfirm.Size = New System.Drawing.Size(122, 16)
        Me.lblPasswordConfirm.TabIndex = 10
        Me.lblPasswordConfirm.Text = "Gõ lại mật khẩu mới"
        '
        'lblPasswordHistory
        '
        Me.lblPasswordHistory.AutoSize = True
        Me.lblPasswordHistory.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.lblPasswordHistory.Location = New System.Drawing.Point(27, 65)
        Me.lblPasswordHistory.Name = "lblPasswordHistory"
        Me.lblPasswordHistory.Size = New System.Drawing.Size(77, 16)
        Me.lblPasswordHistory.TabIndex = 11
        Me.lblPasswordHistory.Text = "Mật khẩu cũ"
        '
        'lblPasswordNew
        '
        Me.lblPasswordNew.AutoSize = True
        Me.lblPasswordNew.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.lblPasswordNew.Location = New System.Drawing.Point(27, 100)
        Me.lblPasswordNew.Name = "lblPasswordNew"
        Me.lblPasswordNew.Size = New System.Drawing.Size(85, 16)
        Me.lblPasswordNew.TabIndex = 7
        Me.lblPasswordNew.Text = "Mật khẩu mới"
        '
        'lblUserCD
        '
        Me.lblUserCD.AutoSize = True
        Me.lblUserCD.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.lblUserCD.Location = New System.Drawing.Point(27, 30)
        Me.lblUserCD.Name = "lblUserCD"
        Me.lblUserCD.Size = New System.Drawing.Size(94, 16)
        Me.lblUserCD.TabIndex = 8
        Me.lblUserCD.Text = "Tên đăng nhập"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblUserCD)
        Me.GroupBox1.Controls.Add(Me.lblPasswordNew)
        Me.GroupBox1.Controls.Add(Me.btnFinish)
        Me.GroupBox1.Controls.Add(Me.lblPasswordHistory)
        Me.GroupBox1.Controls.Add(Me.btnChangePassword)
        Me.GroupBox1.Controls.Add(Me.lblPasswordConfirm)
        Me.GroupBox1.Controls.Add(Me.txtPasswordHistory)
        Me.GroupBox1.Controls.Add(Me.txtUserCD)
        Me.GroupBox1.Controls.Add(Me.txtPasswordNew)
        Me.GroupBox1.Controls.Add(Me.txtPasswordConfirm)
        Me.GroupBox1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(439, 220)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Thay đổi mật khẩu"
        '
        'frmMNUILGN110
        '
        Me.AcceptButton = Me.btnChangePassword
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(457, 232)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("MS PGothic", 11.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMNUILGN110"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EVN IMPORT"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnFinish As System.Windows.Forms.Button
    Friend WithEvents btnChangePassword As System.Windows.Forms.Button
    Friend WithEvents txtPasswordHistory As System.Windows.Forms.TextBox
    Friend WithEvents txtPasswordNew As System.Windows.Forms.TextBox
    Friend WithEvents txtPasswordConfirm As System.Windows.Forms.TextBox
    Friend WithEvents txtUserCD As System.Windows.Forms.TextBox
    Friend WithEvents lblPasswordConfirm As System.Windows.Forms.Label
    Friend WithEvents lblPasswordHistory As System.Windows.Forms.Label
    Friend WithEvents lblPasswordNew As System.Windows.Forms.Label
    Friend WithEvents lblUserCD As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
End Class
