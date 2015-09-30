Imports System.Configuration
Imports MyNo.Common

Public Class Menu

    Dim tabPage As TabPage
    Private Const MAXIMUM_SIZE As Integer = 200

#Region "EVENT"
    Private Sub Menu_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Set init for print
        InitPrint()
        'Set with header for tabpage
        TabControl1.ItemSize = New Size(135, 22)
        TabControl1.SizeMode = TabSizeMode.Fixed
        tvMenu.CollapseAll()

        '' Set tooltip for button
        'Dim toolTip As New ToolTip()
        'toolTip.SetToolTip(btnStatus, "ステータス管理")
        'toolTip.SetToolTip(btnPrint1, "帳票１")
        'toolTip.SetToolTip(btnPrint2, "帳票２")
        'toolTip.SetToolTip(btnCloseAll, "閉じる")
        ''toolTip.SetToolTip(btnMessageList, "メッセージ送信")
        'toolTip.SetToolTip(btnMessageList, "メッセージ一覧")


        'Open message list
        'Dim frmMess = New frmMessageList()
        'OpenTagPage(Nothing, frmMess, TabControl1)
        ''Open screen Status manager
        'Dim _tag As New ClassPrint()
        'Dim frmStatus As New frmStatusManagement
        '_tag.m_PrintButton1 = True
        '_tag.m_PrintScreen1 = New frmProjectSonekiPrint()
        'frmStatus.m_Tag = _tag
        'OpenTagPage(Nothing, frmStatus, TabControl1)
        'SetEnablePrintButton()
        'If TabControl1.TabCount > 1 Then
        '    TabControl1.SelectTab(0)
        'End If

        'If BUKA_CD <> TN_BUKA_CD Then
        '    SetMenu() '■	管理部以外のユーザーの場合	
        'End If
    End Sub

    '■	管理部以外のユーザーの場合												
    'Private Sub SetMenu()
    '    Dim _tvMenu As TreeView = New TreeView
    '    CopyTreeNodes(tvMenu, _tvMenu)
    '    For Each parentNode As TreeNode In _tvMenu.Nodes
    '        If parentNode Is Nothing Then Continue For
    '        If parentNode.Name = "NodeStep3" _
    '             Or parentNode.Name = "NodeStep5" _
    '             Or parentNode.Name = "NodeStep6" _
    '             Or parentNode.Name = "NodeStep8" Then
    '            Dim mynode As TreeNode = tvMenu.Nodes(parentNode.Name)
    '            If Not mynode Is Nothing Then
    '                tvMenu.Nodes.Remove(mynode)
    '            End If
    '        End If
    '        For Each childNode As TreeNode In parentNode.Nodes
    '            If childNode Is Nothing Then Continue For
    '            If childNode.Name = UC_JYUTYU _
    '           Or childNode.Name = UC_NYUUSYUKKO _
    '           Or childNode.Name = UC_TYUMONPRINT _
    '           Or childNode.Name = "Node15" _
    '           Or childNode.Name = UC_SEIKYUPRINT Then
    '                Dim mynode As TreeNode = tvMenu.Nodes(parentNode.Name).Nodes(childNode.Name)
    '                If Not mynode Is Nothing Then
    '                    tvMenu.Nodes.Remove(mynode)
    '                End If
    '            End If
    '        Next
    '    Next
    'End Sub

    Public Sub CopyTreeNodes(ByVal treeview1 As TreeView, ByVal treeview2 As TreeView)
        Dim newTn As TreeNode
        For Each tn As TreeNode In treeview1.Nodes
            newTn = New TreeNode(tn.Text)
            newTn.Name = tn.Name
            CopyChildren(newTn, tn)
            treeview2.Nodes.Add(newTn)
        Next
    End Sub
    Public Sub CopyChildren(ByVal parent As TreeNode, ByVal original As TreeNode)
        Dim newTn As TreeNode
        For Each tn As TreeNode In original.Nodes
            newTn = New TreeNode(tn.Text)
            newTn.Name = tn.Name
            parent.Nodes.Add(newTn)
            CopyChildren(newTn, tn)
        Next
    End Sub


    Private Sub tvMenu_NodeMouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvMenu.NodeMouseDoubleClick
        Dim uc As BaseForm
        Dim i As Integer = 1
        Dim maxMenu As Integer = 0
        Dim tag As TabPage = New TabPage(e.Node.Name)
        Dim directoryAccess As String = My.Application.Info.DirectoryPath + "\access\"
        'Dim frmWait As FromWatting = New FromWatting
        'frmWait.Message = getMsg("W088")

        If e.Node IsNot Nothing Then
            Select Case (e.Node.Name)
                'Case UC_MITUMORI1
                '    frmWait.Show()
                '    Application.DoEvents()
                'Case UC_JYUTYU
                '    frmWait.Show()
                '    Application.DoEvents()
                'Case UC_JYUTYU_SALE
                '    frmWait.Show()
                '    Application.DoEvents()
                'Case UC_NOHINSHIJIGAMEN
                '    frmWait.Show()
                '    Application.DoEvents()
            End Select
        End If
        'Get Name in menu treeview
        If e.Node IsNot Nothing Then
            Select Case (e.Node.Name)
                Case "ucNhanVien"
                    uc = New frmMNUIMTN400()
                Case "ucImport"
                    uc = New frmMNUIDTR200()
                    'Case UC_JYUTYU
                    '    uc = New frmJyutyuNew(False)
                    'Case UC_JYUTYU_SALE
                    '    uc = New frmJyutyuNewSale(False)
                    'Case UC_IMPORTED_STATUS
                    '    uc = New frmImportedStatus()
                    'Case UC_NOHINSHIJIGAMEN
                    '    uc = New frmNohinShijiGamen()
                    'Case UC_STATUS
                    '    uc = New frmStatusManagement()
                    'Case UC_NYUUSYUKKO
                    '    uc = New frmNyuusyukko()
                    'Case UC_SEIKYU
                    '    uc = New frmSeikyu()
                    'Case UC_NYUKIN
                    '    uc = New frmNyukin()
                    'Case UC_SIHARAI
                    '    uc = New frmSiharai()
                    'Case UC_SMANAGEMENT
                    '    uc = New frmSManagement()
                    'Case UC_PDSAGYOIRAI
                    '    uc = New frmPdSagyoIrai()
                    'Case UC_GETUJIKOUSIN
                    '    uc = New frmGetujiKousin()
                    '    ' -------------------------- 帳票 ------------------------------------
                    'Case UC_PDSAGYOIRAIPRINT
                    '    uc = New frmPdSagyoIraiPrint()
                    '    uc.ShowDialog()
                    '    Return
                    'Case UC_MITUMORIPRINT
                    '    uc = New frmMitumoriPrint(True, False)
                    '    uc.ShowDialog()
                    '    Return
                    'Case UC_HOUKOKUPRINT
                    '    uc = New frmHoukokuPrint()
                    '    uc.ShowDialog()
                    '    Return
                    'Case UC_TYUMONPRINT
                    '    uc = New frmTyumonPrint()
                    '    uc.ShowDialog()
                    '    Return
                    'Case UC_PROJECTSONEKIPRINT
                    '    uc = New frmProjectSonekiPrint()
                    '    uc.ShowDialog()
                    '    Return
                    'Case UC_SALEREPORT
                    '    uc = New frmSaleReport()
                    '    uc.ShowDialog()
                    '    Return
                    'Case UC_REPORTANALYZE
                    '    uc = New frmAnalyzeReport
                    '    uc.ShowDialog()
                    '    Return
                    'Case UC_SEIKYUPRINT
                    '    If KENGEN_LVL <> 4 Then
                    '        uc = New frmSeikyuKakuninNew()
                    '        uc.ShowDialog()
                    '        Return
                    '    End If
                    'Case UC_SEIKYUITIRANPRINT
                    '    uc = New frmSeikyuItiranPrint()
                    '    uc.ShowDialog()
                    '    Return

                    'Case UC_NOHINSHIJIPRINT
                    '    uc = New frmNohinShijiPrint()
                    '    uc.ShowDialog()
                    '    Return
                    'Case "ucExport"
                    '    uc = New frmMToolExportExcel()

                    'Case UC_MMEISYO
                    '    uc = New frmMMeisyo()
                    'Case UC_MTANTOU
                    '    uc = New frmMTantou()
                    'Case UC_MSYOHIN
                    '    uc = New frmMSyohin()
                    'Case UC_MTOKUISAKI
                    '    uc = New frmMTokuisaki()
                    'Case UC_MSIIRESAKI
                    '    uc = New frmMSiiresaki()
                    'Case UC_MSYSTEM
                    '    uc = New frmMsystem()
                    'Case UC_MFILEMANAGEMENT
                    '    uc = New frmMFileManagement()
                    'Case UC_MIMPORTEXPENDITURE
                    '    uc = New frmMImportExpenditure()
                    'Case UC_MSHOHINBUNRUI
                    '    uc = New frmMShohinBunrui()
                    'Case UC_SYUKKOIRAI
                    '    uc = New frmSyukkoIrai()
                    'Case UC_IMPORTBUSINESS
                    '    OpenFile(directoryAccess + UC_IMPORTBUSINESS + ".accdb")
                    'Case UC_BUYINGANDSELLING
                    '    OpenFile(directoryAccess + UC_BUYINGANDSELLING + ".accdb")
                    'Case UC_TRADINGPROFITANDLOSS
                    '    OpenFile(directoryAccess + UC_TRADINGPROFITANDLOSS + ".accdb")
                    'Case UC_WORKMANAGEMENT
                    '    OpenFile(directoryAccess + UC_WORKMANAGEMENT + ".accdb")
                    'Case UC_TIMEANDATTENDANCEINPUT
                    '    OpenFile(directoryAccess + "出退勤入力 画面入力.accdb")
                    'Case UC_ATTENDANCEMANAGEMENT
                    '    OpenFile(directoryAccess + UC_ATTENDANCEMANAGEMENT + ".accdb")
                    'Case UC_JOBANALYSIS
                    '    OpenFile(directoryAccess + UC_JOBANALYSIS + ".accdb")
                    'Case UC_BUSINESSMANAGEMENT
                    '    OpenFile(directoryAccess + UC_BUSINESSMANAGEMENT + ".accdb")
                    '    'Case UC_WORKMANAGEMENT
                    '    'OpenFile(directoryAccess + UC_WORKMANAGEMENT + ".accdb")
                    'Case UC_BALANCESHEETMANAGEMENT
                    '    OpenFile(directoryAccess + UC_BALANCESHEETMANAGEMENT + ".accdb")
                    'Case UC_MASTERMANAGEMENT
                    '    OpenFile(directoryAccess + UC_MASTERMANAGEMENT + ".accdb")
                    'Case Else
                    '    If e.Node.Level > 0 Then
                    '        MessageBox.Show(MESAGE, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    '    End If
            End Select
            If uc Is Nothing Then Return
            uc.TopLevel = False
            uc.Visible = True
            uc.FormBorderStyle = FormBorderStyle.None
            uc.Dock = DockStyle.None
            tag.Name = e.Node.Name

            ' Check, How tabPage same Name in TabControl
            'For Each tab As TabPage In TabControl1.TabPages
            '    If tab.Name = e.Node.Name Then
            '        i = i + 1
            '    End If
            'Next
            For j As Integer = 0 To TabControl1.TabPages.Count - 1
                If TabControl1.TabPages(j).Name = e.Node.Name Then
                    i = i + 1
                    Dim number As Integer = MNBTCMN100.getNumberMenu(TabControl1.TabPages(j).Text)
                    If number > maxMenu Then
                        maxMenu = number
                    End If
                End If
            Next
            If maxMenu <> 0 Then
                i = maxMenu + 1
            End If
            Dim tabName As String = If(uc.Text.Trim().Length > 8, uc.Text.Substring(0, 8) + "...", uc.Text)
            If i = 1 Then
                tag.Text = tabName
            ElseIf i > 1 Then
                tag.Text = tabName + "(" + i.ToString() + ")"
            End If
            tag.Controls.Add(uc)
            tag.BackColor = Color.White
            TabControl1.TabPages.Add(tag)
            TabControl1.SelectTab(tag)

            'Set defaut focus control
            If uc.m_ctlFocus IsNot Nothing Then
                uc.m_ctlFocus.Focus()
            End If
            Dim control As BaseForm = DirectCast(tag.Controls(0), BaseForm)
            control.Height = Me.Height - 145
            If Me.Width > 1024 Then
                control.Width = Me.Width - SplitContainer1.Panel1.Width - 50
            End If


            'Set enable Print button     
            'uc.m_Tag = e.Node.Tag
            SetEnablePrintButton()

            'THONGTH comment, not use by spec - 20150213
            'Set banner for steps
            'If e.Node.Parent IsNot Nothing Then
            '    Select Case (e.Node.Parent.Name)
            '        Case "NodeStep1"
            '            SetImage(PicBanner, My.Resources.Resources.B1)
            '            uc.m_Step = 1
            '        Case "NodeStep2"
            '            SetImage(PicBanner, My.Resources.Resources.B2)
            '            uc.m_Step = 2
            '        Case "NodeStep3"
            '            SetImage(PicBanner, My.Resources.Resources.B3)
            '            uc.m_Step = 3
            '        Case "NodeStep4"
            '            SetImage(PicBanner, My.Resources.Resources.B4)
            '            uc.m_Step = 4
            '        Case "NodeStep5"
            '            SetImage(PicBanner, My.Resources.Resources.B5)
            '            uc.m_Step = 5
            '        Case Else
            '            uc.m_Step = 0
            '            SetImage(PicBanner, Nothing)
            '    End Select
            'End If
            'frmWait.Close()
        End If
    End Sub

    Private Sub menuItem_Select(ByVal sender As Object, ByVal e As EventArgs)
        If tabPage Is Nothing Then
            Return
        End If
        If (MessageBox.Show("Bạn có chắc chắn muốn đóng màn hình này?", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK) Then

            Dim mnu As MenuItem = DirectCast(sender, MenuItem)
            If mnu.Name = "Close" Then
                Dim control As BaseForm = DirectCast(TabControl1.SelectedTab.Controls(0), BaseForm)
                control.Disconnection()
                TabControl1.TabPages.Remove(tabPage)
            ElseIf mnu.Name = "CloseAll" Then
                For Each tab As TabPage In TabControl1.TabPages
                    If tab IsNot tabPage Then
                        Dim control As BaseForm = DirectCast(tab.Controls(0), BaseForm)
                        control.Disconnection()
                        TabControl1.TabPages.Remove(tab)
                    End If
                Next
            End If
            'THONGTH comment, not use by spec - 20150213
            'If TabControl1.TabPages.Count() > 0 Then
            '    Dim control As frmBaseForm = DirectCast(TabControl1.SelectedTab.Controls(0), frmBaseForm)
            '    Select Case (control.m_Step)
            '        Case 0
            '            SetImage(PicBanner, Nothing)
            '        Case 1
            '            SetImage(PicBanner, My.Resources.Resources.B1)
            '        Case 2
            '            SetImage(PicBanner, My.Resources.Resources.B2)
            '        Case 3
            '            SetImage(PicBanner, My.Resources.Resources.B3)
            '        Case 4
            '            SetImage(PicBanner, My.Resources.Resources.B4)
            '        Case 5
            '            SetImage(PicBanner, My.Resources.Resources.B5)
            '    End Select
            'End If
        End If
    End Sub

    Private Sub SplitContainer1_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        If SplitContainer1.SplitterDistance > MAXIMUM_SIZE Then
            SplitContainer1.SplitterDistance = MAXIMUM_SIZE
        End If
    End Sub

    Private Sub btnCloseAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseAll.Click
        Me.Close()
    End Sub

    Private Sub Menu_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If MsgBox("Bạn có muốn đóng chương trình này không?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            e.Cancel = False
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub btnStatus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStatus.Click
        'Dim _tag As New ClassPrint()
        'Dim frm As New frmStatusManagement
        '_tag.m_PrintButton1 = True
        '_tag.m_PrintScreen1 = New frmProjectSonekiPrint()
        'frm.m_Tag = _tag
        'OpenTagPage(Nothing, frm, TabControl1)
    End Sub

    Private Sub TabControl1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TabControl1.MouseDown
        'store clicked tab
        Dim tc As TabControl = DirectCast(sender, TabControl)
        Dim hover_index As Integer = Me.getHoverTabIndex(tc)
        If hover_index >= 0 Then
            tc.Tag = tc.TabPages(hover_index)
        End If

        Dim i As Integer
        If e.Button = Windows.Forms.MouseButtons.Left Then
            For i = 1 To TabControl1.TabPages.Count
                Dim rPage As Rectangle
                rPage = TabControl1.GetTabRect(i - 1)
                Dim closeButton As New Rectangle(rPage.Left + 5, rPage.Top + 5, 10, 10)
                If (closeButton.Contains(e.Location)) Then
                    If (MessageBox.Show("Bạn có chắc chắn muốn đóng màn hình này?", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK) Then
                        'TabControl1.TabPages.RemoveAt(i - 1)
                        Dim control As BaseForm = DirectCast(TabControl1.SelectedTab.Controls(0), BaseForm)
                        control.Disconnection()
                        TabControl1.TabPages.Remove(TabControl1.SelectedTab)
                        'Set picture banner after close tab
                        'THONGTH comment, not use by spec - 20150213
                        'SetPictureAffterCloseTab()
                        Return
                    End If
                End If
            Next
        End If
        'Create menu context
        For i = 0 To TabControl1.TabCount - 1
            Dim r As Rectangle = TabControl1.GetTabRect(i)
            If r.Contains(e.Location) Then
                tabPage = TabControl1.TabPages(i)
                'tabControl.SelectTab(tabPage)                
                ' set image banner
                'Set picture banner after close tab
                'THONGTH comment, not use by spec - 20150213
                'SetPictureAffterCloseTab()

                If e.Button = Windows.Forms.MouseButtons.Right Then
                    ' Create menu popup                    
                    ' && it is the header that was clicked
                    ' Change slected index, get the page, create contextual menu
                    Dim cm As New ContextMenu()
                    ' Add several items to menu

                    Dim menuItemClose As MenuItem = New MenuItem()
                    menuItemClose.Name = "Close"
                    menuItemClose.Text = "Chỉ đóng tab này"
                    ' add event click for menu
                    AddHandler menuItemClose.Click, AddressOf menuItem_Select
                    cm.MenuItems.Add(menuItemClose)

                    Dim menuItemCloseAll As MenuItem = New MenuItem()
                    menuItemCloseAll.Name = "CloseAll"
                    menuItemCloseAll.Text = "Đóng các tab khác"
                    If TabControl1.TabCount = 1 Then
                        menuItemCloseAll.Enabled = False
                    End If
                    AddHandler menuItemCloseAll.Click, AddressOf menuItem_Select
                    cm.MenuItems.Add(menuItemCloseAll)


                    cm.Show(Me, New Point(e.X + 185, e.Y + 85))
                End If
                Exit For
            End If
        Next
    End Sub

    Private Sub TabControl1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TabControl1.MouseUp
        ' clear stored tab
        Dim tc As TabControl = DirectCast(sender, TabControl)
        tc.Tag = Nothing
    End Sub

    Private Sub TabControl1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TabControl1.MouseMove
        ' mouse button down? tab was clicked?
        Dim tc As TabControl = DirectCast(sender, TabControl)
        If (e.Button <> MouseButtons.Left) OrElse (tc.Tag Is Nothing) Then
            Return
        End If
        Dim clickedTab As TabPage = DirectCast(tc.Tag, TabPage)
        Dim clicked_index As Integer = tc.TabPages.IndexOf(clickedTab)

        ' start drag n drop
        tc.DoDragDrop(clickedTab, DragDropEffects.All)
    End Sub

    Private Sub TabControl1_DragOver(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TabControl1.DragOver
        Dim tc As TabControl = DirectCast(sender, TabControl)

        ' a tab is draged?
        If e.Data.GetData(GetType(TabPage)) Is Nothing Then
            Return
        End If
        Dim dragTab As TabPage = DirectCast(e.Data.GetData(GetType(TabPage)), TabPage)
        Dim dragTab_index As Integer = tc.TabPages.IndexOf(dragTab)

        ' hover over a tab?
        Dim hoverTab_index As Integer = Me.getHoverTabIndex(tc)
        If hoverTab_index < 0 Then
            e.Effect = DragDropEffects.None
            Return
        End If
        Dim hoverTab As TabPage = tc.TabPages(hoverTab_index)
        e.Effect = DragDropEffects.Move

        ' start of drag?
        If dragTab Is hoverTab Then
            Return
        End If

        ' swap dragTab & hoverTab - avoids toggeling
        Dim dragTabRect As Rectangle = tc.GetTabRect(dragTab_index)
        Dim hoverTabRect As Rectangle = tc.GetTabRect(hoverTab_index)

        If dragTabRect.Width < hoverTabRect.Width Then
            Dim tcLocation As Point = tc.PointToScreen(tc.Location)

            If dragTab_index < hoverTab_index Then
                If (e.X - tcLocation.X) > ((hoverTabRect.X + hoverTabRect.Width) - dragTabRect.Width) Then
                    Me.swapTabPages(tc, dragTab, hoverTab)
                End If
            ElseIf dragTab_index > hoverTab_index Then
                If (e.X - tcLocation.X) < (hoverTabRect.X + dragTabRect.Width) Then
                    Me.swapTabPages(tc, dragTab, hoverTab)
                End If
            End If
        Else
            Me.swapTabPages(tc, dragTab, hoverTab)
        End If

        ' select new pos of dragTab
        tc.SelectedIndex = tc.TabPages.IndexOf(dragTab)
    End Sub

    Private Sub TabControl1_DrawItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles TabControl1.DrawItem
        Dim closefont As New Font(e.Font.FontFamily, e.Font.Size, FontStyle.Regular)
        e.Graphics.DrawString("X", closefont, Brushes.Red, e.Bounds.Left + 5, e.Bounds.Top + 5)

        Dim tabctl As TabControl = DirectCast(sender, TabControl)
        Dim g As Graphics = e.Graphics
        Dim font As Font = tabctl.Font
        Dim brush As New SolidBrush(Color.Black)

        If tabctl.SelectedIndex = e.Index Then
            font = New Font(font, FontStyle.Bold)
            brush = New SolidBrush(Color.Red)
        End If
        e.Graphics.DrawString(TabControl1.TabPages(e.Index).Text, font, brush, e.Bounds.Left + 17, e.Bounds.Top + 5)
    End Sub

    Private Sub TabControl1_Selected(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlEventArgs) Handles TabControl1.Selected
        SetEnablePrintButton()
    End Sub

    Private Sub TabControl1_ControlRemoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles TabControl1.ControlRemoved
        If TabControl1.TabPages.Count = 1 Then
            'THONGTH comment, not use by spec - 20150213
            'PicBanner.Image = Nothing
        End If
    End Sub

    Private Sub TabControl1_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SizeChanged
        For Each tab As TabPage In TabControl1.TabPages
            Dim control As BaseForm = DirectCast(tab.Controls(0), BaseForm)
            'If Me.Height > 800 Then
            control.Height = Me.Height - 145
            'Else
            'control.Height = Me.Height
            'End If
            If Me.Width > 1024 Then
                control.Width = Me.Width - SplitContainer1.Panel1.Width - 50
            End If
        Next
    End Sub

    Private Sub Menu_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Application.Exit()
    End Sub

    Private Sub btnPrint1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint1.Click
        'Dim frmPrint1 = DirectCast(btnPrint1.Tag, Form)
        'If TypeOf frmPrint1 Is frmPdSagyoIraiPrint Then
        '    frmPrint1 = New frmPdSagyoIraiPrint()
        '    Dim frmPdSagyoIrai As frmPdSagyoIrai = DirectCast(TabControl1.SelectedTab.Controls(0), frmPdSagyoIrai)
        '    frmPrint1.ProjectNo = frmPdSagyoIrai.txtProjectNo.Text
        '    frmPrint1.IraiNo = frmPdSagyoIrai.txtIraiNo.Text
        'End If
        'If TypeOf frmPrint1 Is frmMitumoriPrint Then
        '    frmPrint1 = New frmMitumoriPrint(False, True)
        '    Dim frmMitumori1 As frmMitumori1 = DirectCast(TabControl1.SelectedTab.Controls(0), frmMitumori1)
        '    frmPrint1.ProjectNo = frmMitumori1.txtNewMitumoriNo.Text
        '    frmPrint1.IraiNo = frmMitumori1.txtNewMitumoriEda.Text
        'End If
        'If TypeOf frmPrint1 Is frmHoukokuPrint Then
        '    Dim frmJyutyu = DirectCast(TabControl1.SelectedTab.Controls(0), Form)
        '    frmPrint1 = New frmHoukokuPrint()
        '    If TypeOf frmJyutyu Is frmJyutyuNewSale Then
        '        Dim JyutyuNewSale As frmJyutyuNewSale = DirectCast(TabControl1.SelectedTab.Controls(0), frmJyutyuNewSale)
        '        frmPrint1.ProjectNo = JyutyuNewSale.txtJyutyuNo.Text
        '        frmPrint1.IraiNo = JyutyuNewSale.txtJyutyuEda.Text
        '        frmPrint1.TantouCd = JyutyuNewSale.txtNewTantouCd.Text
        '    Else
        '        Dim JyutyuNew As frmJyutyuNew = DirectCast(TabControl1.SelectedTab.Controls(0), frmJyutyuNew)
        '        frmPrint1.ProjectNo = JyutyuNew.txtJyutyuNo.Text
        '        frmPrint1.IraiNo = JyutyuNew.txtJyutyuEda.Text
        '        frmPrint1.TantouCd = JyutyuNew.txtNewTantouCd.Text
        '    End If

        'End If
        'If TypeOf frmPrint1 Is frmSeikyuPrint Then
        '    frmPrint1 = New frmSeikyuPrint()
        '    Dim frmSeikyu As frmSeikyu = DirectCast(TabControl1.SelectedTab.Controls(0), frmSeikyu)
        '    frmPrint1.ProjectNo = frmSeikyu.txtSeiriNo.Text
        'End If
        'If TypeOf frmPrint1 Is frmNohinShijiPrint Then
        '    frmPrint1 = New frmNohinShijiPrint()
        '    Dim frmNohinShijiGamen As frmNohinShijiGamen = DirectCast(TabControl1.SelectedTab.Controls(0), frmNohinShijiGamen)
        '    frmPrint1.ProjectNo = frmNohinShijiGamen.txtNohin_No.Text 'QUYNX add 3-11-2014
        '    frmPrint1.IraiNo = frmNohinShijiGamen.txtNohin_No_E.Text  'QUYNX add 3-11-2014
        'End If
        'If TypeOf frmPrint1 Is frmProjectSonekiPrint Then
        '    frmPrint1 = New frmProjectSonekiPrint()
        '    Dim frmStatusManagement As frmStatusManagement = DirectCast(TabControl1.SelectedTab.Controls(0), frmStatusManagement)
        '    If frmStatusManagement.dgProject.RowCount > 0 Then
        '        Dim ProjectNo = frmStatusManagement.dgProject.SelectedCells(1).Value
        '        frmPrint1.ProjectNo = ProjectNo
        '    End If
        'End If
        'If TypeOf frmPrint1 Is frmSeikyuKakuninNew Then
        '    frmPrint1 = New frmSeikyuKakuninNew()
        'End If
        'frmPrint1.ShowDialog()
    End Sub

    Private Sub btnPrint2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint2.Click
        'Dim frmPrint2 = DirectCast(btnPrint2.Tag, Form)
        'If TypeOf frmPrint2 Is frmTyumonPrint Then
        '    frmPrint2 = New frmTyumonPrint
        '    If TabControl1.SelectedTab.Controls(0).Name = "frmJyutyuNewSale" Then
        '        Dim frmJyutyu As frmJyutyuNewSale = DirectCast(TabControl1.SelectedTab.Controls(0), frmJyutyuNewSale)
        '        frmPrint2.ProjectNo = frmJyutyu.txtJyutyuNo.Text
        '        frmPrint2.IraiNo = frmJyutyu.txtJyutyuEda.Text
        '    Else
        '        Dim frmJyutyu As frmJyutyuNew = DirectCast(TabControl1.SelectedTab.Controls(0), frmJyutyuNew)
        '        frmPrint2.ProjectNo = frmJyutyu.txtJyutyuNo.Text
        '        frmPrint2.IraiNo = frmJyutyu.txtJyutyuEda.Text
        '    End If

        'End If
        'If TypeOf frmPrint2 Is frmSeikyuItiranPrint Then
        '    frmPrint2 = New frmSeikyuItiranPrint()
        'End If
        'frmPrint2.ShowDialog()
    End Sub
#End Region
#Region "METHOD"
    Public Sub InitPrint()
        ''Setup PrintButton       
        ''Check all node in treeview
        ''and assign variable each form
        'For Each parentNode As TreeNode In tvMenu.Nodes
        '    For Each childNode As TreeNode In parentNode.Nodes
        '        Dim _tag As New ClassPrint()
        '        Select Case (childNode.Name)
        '            Case UC_MITUMORI1
        '                _tag.m_PrintButton1 = True
        '                _tag.m_PrintScreen1 = New frmMitumoriPrint(True, False)
        '                childNode.Tag = _tag
        '            Case UC_JYUTYU, UC_JYUTYU_SALE
        '                _tag.m_PrintButton1 = True
        '                _tag.m_PrintButton2 = True
        '                _tag.m_PrintScreen1 = New frmHoukokuPrint()
        '                _tag.m_PrintScreen2 = New frmTyumonPrint()
        '                childNode.Tag = _tag
        '            Case UC_SEIKYU
        '                If BUKA_CD = TN_BUKA_CD Then
        '                    _tag.m_PrintButton1 = True
        '                End If
        '                _tag.m_PrintButton2 = True
        '                _tag.m_PrintScreen1 = New frmSeikyuKakuninNew()
        '                _tag.m_PrintScreen2 = New frmSeikyuItiranPrint()
        '                childNode.Tag = _tag
        '            Case UC_PDSAGYOIRAI
        '                _tag.m_PrintButton1 = True
        '                _tag.m_PrintScreen1 = New frmPdSagyoIraiPrint()
        '                childNode.Tag = _tag
        '            Case UC_NOHINSHIJIGAMEN
        '                _tag.m_PrintButton1 = True
        '                _tag.m_PrintScreen1 = New frmNohinShijiPrint()
        '                childNode.Tag = _tag
        '            Case UC_STATUS
        '                _tag.m_PrintButton1 = True
        '                _tag.m_PrintScreen1 = New frmProjectSonekiPrint()
        '                childNode.Tag = _tag
        '        End Select
        '    Next
        'Next
    End Sub

    '=======================================================================
    ' 税率取得
    '=======================================================================
    Public Function getSystemZeiritu() As Boolean
        'Dim db As CDB = New CDB
        'Try
        '    db.Connect()
        '    Dim cSYS As CMSystem = New CMSystem
        '    If cSYS.SelectRecord(db) Then
        '        ZEI_RITU = cSYS.ZEI_RITU
        '        ZEI_RITU1 = cSYS.ZEI_RITU1
        '        ZEI_RITU2 = cSYS.ZEI_RITU2
        '        ZEI_RITU3 = cSYS.ZEI_RITU3
        '        ZEI_YMD1 = cSYS.ZEI_YMD1
        '        ZEI_YMD2 = cSYS.ZEI_YMD2
        '        ZEI_YMD3 = cSYS.ZEI_YMD3
        '    End If
        '    cSYS = Nothing
        'Catch ex As Exception
        '    Throw ex
        'Finally
        '    db.Disconnect()
        '    db = Nothing
        'End Try
    End Function

    '=======================================================================
    ' 権限ﾚﾍﾞﾙ詳細設定
    '=======================================================================
    Public Function getKengen()
        'Dim str() As String '権限ﾚﾍﾞﾙ別使用不可詳細
        'Dim i As Integer

        ' ''iniﾌｧｲﾙより権限を取得
        ''Dim strKengen As String = "KENGEN" & KENGEN_LVL
        ''str = Split(getIniFileString(strKengen), ",")

        ''名称ﾏｽﾀより権限を取得
        'Dim db As CDB = New CDB
        'Try
        '    db.Connect()
        '    Dim strKengen As String = db.getMstName("MMEISYO", "NM2", KBN_KENGEN_LVL, KENGEN_LVL)
        '    str = Split(strKengen, ",")
        'Catch ex As Exception
        '    Throw ex
        'Finally
        '    db.Disconnect()
        '    db = Nothing
        'End Try

        ''ﾃﾞｰﾀがない場合
        'If str(0) = "" Then
        '    For i = 0 To 50
        '        KENGEN_DET(i) = False '権限を初期化
        '    Next
        'Else
        '    For i = 0 To str.Length - 1
        '        KENGEN_DET(i) = IIf(str(i) = "0", False, True)
        '    Next
        'End If

    End Function

    Public Sub OpenFile(ByVal PathFile As String)
        'If IO.File.Exists(PathFile) Then
        '    Process.Start(PathFile)
        'Else
        '    MessageBox.Show(MSG_ERRFile, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'End If
    End Sub

    'THONGTH comment, not use by spec - 20150213
    ' ''' <summary>
    ' ''' Set picture banner after close tab
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Sub SetPictureAffterCloseTab()
    '    If TabControl1.TabPages.Count() > 0 Then
    '        Dim control As frmBaseForm = DirectCast(TabControl1.SelectedTab.Controls(0), frmBaseForm)
    '        Select Case (control.m_Step)
    '            Case 0
    '                SetImage(PicBanner, Nothing)
    '            Case 1
    '                SetImage(PicBanner, My.Resources.Resources.B1)
    '            Case 2
    '                SetImage(PicBanner, My.Resources.Resources.B2)
    '            Case 3
    '                SetImage(PicBanner, My.Resources.Resources.B3)
    '            Case 4
    '                SetImage(PicBanner, My.Resources.Resources.B4)
    '            Case 5
    '                SetImage(PicBanner, My.Resources.Resources.B5)
    '        End Select
    '    End If
    'End Sub

    Private Function getHoverTabIndex(ByVal tc As TabControl) As Integer
        For i As Integer = 0 To tc.TabPages.Count - 1
            If tc.GetTabRect(i).Contains(tc.PointToClient(Cursor.Position)) Then
                Return i
            End If
        Next

        Return -1
    End Function
    Private Sub swapTabPages(ByVal tc As TabControl, ByVal src As TabPage, ByVal dst As TabPage)
        Dim index_src As Integer = tc.TabPages.IndexOf(src)
        Dim index_dst As Integer = tc.TabPages.IndexOf(dst)
        tc.TabPages(index_dst) = src
        tc.TabPages(index_src) = dst
        tc.Refresh()
    End Sub
    ''' <summary>
    ''' Set Print enable after select tab
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetEnablePrintButton()
        'btnPrint1.Enabled = False
        'btnPrint2.Enabled = False
        ''Get form in tabpage
        'If TabControl1.SelectedTab Is Nothing Then Return
        'Dim control As frmBaseForm = DirectCast(TabControl1.SelectedTab.Controls(0), frmBaseForm)
        ''Get variable m_Tag (class print) in control
        'Dim _tag As ClassPrint = control.m_Tag
        'If _tag IsNot Nothing Then
        '    btnPrint1.Enabled = _tag.m_PrintButton1
        '    btnPrint2.Enabled = _tag.m_PrintButton2
        '    btnPrint1.Tag = _tag.m_PrintScreen1
        '    btnPrint2.Tag = _tag.m_PrintScreen2
        'End If
    End Sub
#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim frm = New DemoCbbInSheet()
        'frm.Show()
    End Sub

    Private Sub btnMessageList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMessageList.Click
        'Dim frm = New frmMessageList()
        'OpenTagPage(Nothing, frm, TabControl1)
    End Sub
End Class
