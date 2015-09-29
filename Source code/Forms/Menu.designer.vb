<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Menu
    Inherits Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Menu))
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Nhập và lịch sử dữ liệu")
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Danh sách dữ liệu")
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Nhập xuất dữ liệu", New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2})
        Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Danh sách nhân viên")
        Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Quản trị hệ thống", New System.Windows.Forms.TreeNode() {TreeNode4})
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.tvMenu = New System.Windows.Forms.TreeView()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnMessageList = New System.Windows.Forms.Button()
        Me.btnPrint2 = New System.Windows.Forms.Button()
        Me.btnStatus = New System.Windows.Forms.Button()
        Me.btnCloseAll = New System.Windows.Forms.Button()
        Me.btnPrint1 = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.Controls.Add(Me.SplitContainer1)
        Me.Panel2.Location = New System.Drawing.Point(9, 81)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1160, 456)
        Me.Panel2.TabIndex = 13
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BackgroundImage = CType(resources.GetObject("SplitContainer1.BackgroundImage"), System.Drawing.Image)
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.tvMenu)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TabControl1)
        Me.SplitContainer1.Size = New System.Drawing.Size(1160, 456)
        Me.SplitContainer1.SplitterDistance = 154
        Me.SplitContainer1.SplitterWidth = 8
        Me.SplitContainer1.TabIndex = 14
        '
        'tvMenu
        '
        Me.tvMenu.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvMenu.Location = New System.Drawing.Point(0, 0)
        Me.tvMenu.Margin = New System.Windows.Forms.Padding(3, 10, 3, 3)
        Me.tvMenu.Name = "tvMenu"
        TreeNode1.Name = "ucImport"
        TreeNode1.Text = "Nhập và lịch sử dữ liệu"
        TreeNode2.Name = "ucSearchData"
        TreeNode2.Text = "Danh sách dữ liệu"
        TreeNode3.Name = "NodeStep1"
        TreeNode3.Text = "Nhập xuất dữ liệu"
        TreeNode4.Name = "ucNhanVien"
        TreeNode4.Text = "Danh sách nhân viên"
        TreeNode5.Name = "NodeStep8"
        TreeNode5.Text = "Quản trị hệ thống"
        Me.tvMenu.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode3, TreeNode5})
        Me.tvMenu.ShowLines = False
        Me.tvMenu.Size = New System.Drawing.Size(154, 456)
        Me.tvMenu.TabIndex = 5
        '
        'TabControl1
        '
        Me.TabControl1.AllowDrop = True
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(998, 456)
        Me.TabControl1.TabIndex = 6
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnMessageList)
        Me.Panel1.Controls.Add(Me.btnPrint2)
        Me.Panel1.Controls.Add(Me.btnStatus)
        Me.Panel1.Controls.Add(Me.btnCloseAll)
        Me.Panel1.Controls.Add(Me.btnPrint1)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1179, 80)
        Me.Panel1.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 30.75!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(12, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(235, 47)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Application"
        '
        'btnMessageList
        '
        Me.btnMessageList.BackColor = System.Drawing.SystemColors.Control
        Me.btnMessageList.FlatAppearance.BorderSize = 0
        Me.btnMessageList.Location = New System.Drawing.Point(318, 17)
        Me.btnMessageList.Name = "btnMessageList"
        Me.btnMessageList.Size = New System.Drawing.Size(58, 58)
        Me.btnMessageList.TabIndex = 3
        Me.btnMessageList.UseVisualStyleBackColor = True
        Me.btnMessageList.Visible = False
        '
        'btnPrint2
        '
        Me.btnPrint2.BackColor = System.Drawing.SystemColors.Control
        Me.btnPrint2.Enabled = False
        Me.btnPrint2.FlatAppearance.BorderSize = 0
        Me.btnPrint2.Location = New System.Drawing.Point(510, 17)
        Me.btnPrint2.Name = "btnPrint2"
        Me.btnPrint2.Size = New System.Drawing.Size(58, 58)
        Me.btnPrint2.TabIndex = 3
        Me.btnPrint2.UseVisualStyleBackColor = True
        Me.btnPrint2.Visible = False
        '
        'btnStatus
        '
        Me.btnStatus.BackColor = System.Drawing.SystemColors.Control
        Me.btnStatus.FlatAppearance.BorderSize = 0
        Me.btnStatus.Image = CType(resources.GetObject("btnStatus.Image"), System.Drawing.Image)
        Me.btnStatus.Location = New System.Drawing.Point(382, 17)
        Me.btnStatus.Name = "btnStatus"
        Me.btnStatus.Size = New System.Drawing.Size(58, 58)
        Me.btnStatus.TabIndex = 1
        Me.btnStatus.UseVisualStyleBackColor = True
        Me.btnStatus.Visible = False
        '
        'btnCloseAll
        '
        Me.btnCloseAll.BackColor = System.Drawing.SystemColors.Control
        Me.btnCloseAll.FlatAppearance.BorderSize = 0
        Me.btnCloseAll.Image = CType(resources.GetObject("btnCloseAll.Image"), System.Drawing.Image)
        Me.btnCloseAll.Location = New System.Drawing.Point(574, 17)
        Me.btnCloseAll.Name = "btnCloseAll"
        Me.btnCloseAll.Size = New System.Drawing.Size(58, 58)
        Me.btnCloseAll.TabIndex = 4
        Me.btnCloseAll.UseVisualStyleBackColor = True
        Me.btnCloseAll.Visible = False
        '
        'btnPrint1
        '
        Me.btnPrint1.BackColor = System.Drawing.SystemColors.Control
        Me.btnPrint1.Enabled = False
        Me.btnPrint1.FlatAppearance.BorderSize = 0
        Me.btnPrint1.Location = New System.Drawing.Point(446, 17)
        Me.btnPrint1.Name = "btnPrint1"
        Me.btnPrint1.Size = New System.Drawing.Size(58, 58)
        Me.btnPrint1.TabIndex = 2
        Me.btnPrint1.UseVisualStyleBackColor = True
        Me.btnPrint1.Visible = False
        '
        'Menu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1179, 541)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "Menu"
        Me.Text = "Menu"
        Me.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents tvMenu As System.Windows.Forms.TreeView
    Friend WithEvents btnPrint1 As System.Windows.Forms.Button
    Friend WithEvents btnCloseAll As System.Windows.Forms.Button
    Friend WithEvents btnStatus As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents btnPrint2 As System.Windows.Forms.Button
    Friend WithEvents btnMessageList As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
