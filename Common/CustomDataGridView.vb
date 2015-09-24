Imports MyNo.Common

''' <summary>  
''' DataGridView fill data will be not flicker
''' </summary>  
Public Class CustomDataGridView
    Inherits DataGridView
    Public Sub New()
        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
        UpdateStyles()
    End Sub

    ''' <summary>
    ''' Tab in cells are: link, can input
    ''' </summary>
    ''' <param name="keyData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function ProcessDialogKey(keyData As System.Windows.Forms.Keys) As Boolean
        If keyData = Keys.Tab Then
            If IsNothing(MyBase.CurrentCell) Then
                Return MyBase.ProcessDialogKey(keyData)
            End If
            Dim nextColumn As DataGridViewColumn
            Dim col As Integer = 0
            ' When cell in last column, will be focus to next row
            If MyBase.CurrentCell.RowIndex < MyBase.RowCount Then
                If MyBase.CurrentCell.ColumnIndex = CountColumnVisible() Then
                    If MyBase.CurrentCell.RowIndex < MyBase.RowCount - 1 Then
                        ' set focus to the cell's next row
                        MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex + 1).Cells(0)
                    End If
                End If
            End If

            ' Focus to cell are link or can input (cell is link empty not focus)
            For i = MyBase.CurrentCell.ColumnIndex To CountColumnVisible()
                nextColumn = MyBase.Columns.GetNextColumn(MyBase.Columns(i), DataGridViewElementStates.Visible, DataGridViewElementStates.ReadOnly)
                If Not IsNothing(nextColumn) Then
                    If Not IsNothing(nextColumn) Then
                        If MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index).ReadOnly = False Or _
                            (TypeOf MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index) Is DataGridViewLinkCell AndAlso _
                             Not String.IsNullOrEmpty(MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index).Value)) Then
                            MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index)
                            Return True
                        End If
                    End If
                End If
            Next

            If MyBase.CurrentCell.RowIndex < MyBase.RowCount - 1 Then
                For i = 0 To CountColumnVisible()
                    If MyBase.Rows(MyBase.CurrentCell.RowIndex + 1).Cells(i).ReadOnly = False Or _
                        (TypeOf MyBase.Rows(MyBase.CurrentCell.RowIndex + 1).Cells(i) Is DataGridViewLinkCell AndAlso _
                             Not String.IsNullOrEmpty(MyBase.Rows(MyBase.CurrentCell.RowIndex + 1).Cells(i).Value)) Then
                        MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex + 1).Cells(i)
                        Return True
                    End If
                Next
            End If
            ' Count control in parent
            Dim controls = MyBase.Parent.Controls.Cast(Of Control)().Where(Function(r) r.TabIndex > MyBase.TabIndex And (r.Enabled = True Or r.Visible = False)).OrderBy(Function(r) r.TabIndex).FirstOrDefault()

            If Not controls Is Nothing AndAlso controls.Visible = True Then
                MyBase.ClearSelection()
                controls.Focus()
                Return True
            End If
        End If
        If (keyData = Keys.Tab + Keys.Shift) Then
            'ここに処理を書く
            If IsNothing(MyBase.CurrentCell) Then
                Return MyBase.ProcessDialogKey(keyData)
            End If
            Dim nextColumn As DataGridViewColumn
            Dim col As Integer = 0
            ' is last column
            ' When cell in last column, will be focus to next row
            If MyBase.CurrentCell.RowIndex > 0 Then
                If MyBase.CurrentCell.ColumnIndex = 0 Then
                    If MyBase.CurrentCell.RowIndex > 0 Then
                        ' set focus to the cell's next row
                        MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex - 1).Cells(0)
                    End If
                End If
            End If
            ' if cell is link or can input
            ' Focus to cell are link or can input (cell is link empty not focus)
            For i = MyBase.CurrentCell.ColumnIndex To 0 Step -1
                nextColumn = MyBase.Columns.GetPreviousColumn(MyBase.Columns(i), DataGridViewElementStates.Visible, DataGridViewElementStates.ReadOnly)
                If Not IsNothing(nextColumn) Then
                    If MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index).ReadOnly = False Or _
                          (TypeOf MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index) Is DataGridViewLinkCell AndAlso _
                             Not String.IsNullOrEmpty(MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index).Value)) Then
                        MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index)
                        Return True
                    End If
                End If
            Next
            If MyBase.CurrentCell.RowIndex > 0 Then
                For i = CountColumnVisible() To 0 Step -1
                    If MyBase.Rows(MyBase.CurrentCell.RowIndex - 1).Cells(i).ReadOnly = False Or _
                         (TypeOf MyBase.Rows(MyBase.CurrentCell.RowIndex - 1).Cells(i) Is DataGridViewLinkCell AndAlso _
                             Not String.IsNullOrEmpty(MyBase.Rows(MyBase.CurrentCell.RowIndex - 1).Cells(i).Value)) Then
                        MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex - 1).Cells(i)
                        Return True
                    End If
                Next
            End If
            ' Count control in parent
            Dim controls = MyBase.Parent.Controls.Cast(Of Control)().Where(Function(r) r.TabIndex < MyBase.TabIndex And r.Enabled = True And r.Visible = True).OrderByDescending(Function(r) r.TabIndex)
            For Each ctl In controls
                If Not ctl Is Nothing Then
                    If TypeOf ctl Is Button Or _
                        TypeOf ctl Is TextBox Or _
                        TypeOf ctl Is ComboBox Then
                        MyBase.ClearSelection()
                        ctl.Focus()
                        Return True
                    End If
                End If
            Next
            ' Count control in parent
            Dim controlMax = MyBase.Parent.Controls.Cast(Of Control)().Where(Function(r) r.TabIndex > MyBase.TabIndex And r.Enabled = True And r.Visible = True).OrderByDescending(Function(r) r.TabIndex)
            For Each ctl In controlMax
                If Not ctl Is Nothing Then
                    If TypeOf ctl Is Button Or _
                        TypeOf ctl Is TextBox Or _
                        TypeOf ctl Is ComboBox Then
                        MyBase.ClearSelection()
                        ctl.Focus()
                        Return True
                    End If
                End If
            Next
        End If
        Return MyBase.ProcessDialogKey(keyData)
    End Function

    ''' <summary>
    ''' Tab in cells are: link, can input
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function ProcessDataGridViewKey(e As System.Windows.Forms.KeyEventArgs) As Boolean
        If e.KeyData = Keys.Tab Then
            If IsNothing(MyBase.CurrentCell) Then
                Return MyBase.ProcessDataGridViewKey(e)
            End If
            Dim nextColumn As DataGridViewColumn
            Dim col As Integer = 0
            ' is last column
            ' When cell in last column, will be focus to next row
            If MyBase.CurrentCell.RowIndex < MyBase.RowCount Then
                If MyBase.CurrentCell.ColumnIndex = CountColumnVisible() Then
                    If MyBase.CurrentCell.RowIndex < MyBase.RowCount - 1 Then
                        ' set focus to the cell's next row
                        MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex + 1).Cells(0)
                    End If
                End If
            End If
            ' if cell is link or can input
            ' Focus to cell are link or can input (cell is link empty not focus)
            For i = MyBase.CurrentCell.ColumnIndex To CountColumnVisible()
                nextColumn = MyBase.Columns.GetNextColumn(MyBase.Columns(i), DataGridViewElementStates.Visible, DataGridViewElementStates.ReadOnly)
                If Not IsNothing(nextColumn) Then
                    If MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index).ReadOnly = False Or _
                          (TypeOf MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index) Is DataGridViewLinkCell AndAlso _
                             Not String.IsNullOrEmpty(MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index).Value)) Then
                        MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index)
                        Return True
                    End If
                End If
            Next
            If MyBase.CurrentCell.RowIndex < MyBase.RowCount - 1 Then
                For i = 0 To CountColumnVisible()
                    If MyBase.Rows(MyBase.CurrentCell.RowIndex + 1).Cells(i).ReadOnly = False Or _
                         (TypeOf MyBase.Rows(MyBase.CurrentCell.RowIndex + 1).Cells(i) Is DataGridViewLinkCell AndAlso _
                             Not String.IsNullOrEmpty(MyBase.Rows(MyBase.CurrentCell.RowIndex + 1).Cells(i).Value)) Then
                        MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex + 1).Cells(i)
                        Return True
                    End If
                Next
            End If
            ' Count control in parent
            Dim controls = MyBase.Parent.Controls.Cast(Of Control)().Where(Function(r) r.TabIndex > MyBase.TabIndex And (r.Enabled = True Or r.Visible = False)).OrderBy(Function(r) r.TabIndex).FirstOrDefault()
            If Not controls Is Nothing AndAlso controls.Visible = True Then
                MyBase.ClearSelection()
                controls.Focus()
                Return True
            End If
        End If
        If (e.KeyData = Keys.Tab + Keys.Shift) Then
            'ここに処理を書く
            If IsNothing(MyBase.CurrentCell) Then
                Return MyBase.ProcessDataGridViewKey(e)
            End If
            Dim nextColumn As DataGridViewColumn
            Dim col As Integer = 0
            ' is last column
            ' When cell in last column, will be focus to next row
            If MyBase.CurrentCell.RowIndex > 0 Then
                If MyBase.CurrentCell.ColumnIndex = 0 Then
                    If MyBase.CurrentCell.RowIndex > 0 Then
                        ' set focus to the cell's next row
                        MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex - 1).Cells(0)
                    End If
                End If
            End If
            ' if cell is link or can input
            ' Focus to cell are link or can input (cell is link empty not focus)
            For i = MyBase.CurrentCell.ColumnIndex To 0 Step -1
                nextColumn = MyBase.Columns.GetPreviousColumn(MyBase.Columns(i), DataGridViewElementStates.Visible, DataGridViewElementStates.ReadOnly)
                If Not IsNothing(nextColumn) Then
                    If MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index).ReadOnly = False Or _
                          (TypeOf MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index) Is DataGridViewLinkCell AndAlso _
                             Not String.IsNullOrEmpty(MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index).Value)) Then
                        MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index)
                        Return True
                    End If
                End If
            Next
            If MyBase.CurrentCell.RowIndex > 0 Then
                For i = CountColumnVisible() To 0 Step -1
                    If MyBase.Rows(MyBase.CurrentCell.RowIndex - 1).Cells(i).ReadOnly = False Or _
                         (TypeOf MyBase.Rows(MyBase.CurrentCell.RowIndex - 1).Cells(i) Is DataGridViewLinkCell AndAlso _
                             Not String.IsNullOrEmpty(MyBase.Rows(MyBase.CurrentCell.RowIndex - 1).Cells(i).Value)) Then
                        MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex - 1).Cells(i)
                        Return True
                    End If
                Next
            End If
            ' Count control in parent
            Dim controls = MyBase.Parent.Controls.Cast(Of Control)().Where(Function(r) r.TabIndex < MyBase.TabIndex And r.Enabled = True And r.Visible = True).OrderByDescending(Function(r) r.TabIndex)
            For Each ctl In controls
                If Not ctl Is Nothing Then
                    If TypeOf ctl Is Button Or _
                        TypeOf ctl Is TextBox Or _
                        TypeOf ctl Is ComboBox Then
                        MyBase.ClearSelection()
                        ctl.Focus()
                        Return True
                    End If
                End If
            Next
            ' Count control in parent
            Dim controlMax = MyBase.Parent.Controls.Cast(Of Control)().Where(Function(r) r.TabIndex > MyBase.TabIndex And r.Enabled = True And r.Visible = True).OrderByDescending(Function(r) r.TabIndex)
            For Each ctl In controlMax
                If Not ctl Is Nothing Then
                    If TypeOf ctl Is Button Or _
                        TypeOf ctl Is TextBox Or _
                        TypeOf ctl Is ComboBox Then
                        MyBase.ClearSelection()
                        ctl.Focus()
                        Return True
                    End If
                End If
            Next
        End If
        Return MyBase.ProcessDataGridViewKey(e)
    End Function

    ''' <summary>
    ''' Focus to the first cell is can input or link
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CustomDataGridView_Enter(sender As Object, e As EventArgs) Handles Me.Enter
        ' no row
        If MyBase.RowCount <= 0 Then Return
        If MNBTCMN100.plastControl.TabIndex < MyBase.TabIndex Then
            For i = 0 To CountColumnVisible()
                If (TypeOf MyBase.Rows(0).Cells(i) Is DataGridViewLinkCell AndAlso _
                         Not String.IsNullOrEmpty(MyBase.Rows(0).Cells(i).Value)) Or _
                     MyBase.Rows(0).Cells(i).ReadOnly = False Then
                    MyBase.CurrentCell = MyBase.Rows(0).Cells(i)
                    Exit For
                End If
            Next
        Else
            For i = CountColumnVisible() To 0 Step -1
                If (TypeOf MyBase.Rows(MyBase.RowCount - 1).Cells(i) Is DataGridViewLinkCell AndAlso _
                         Not String.IsNullOrEmpty(MyBase.Rows(MyBase.RowCount - 1).Cells(i).Value)) Or _
                     MyBase.Rows(MyBase.RowCount - 1).Cells(i).ReadOnly = False Then
                    MyBase.CurrentCell = MyBase.Rows(MyBase.RowCount - 1).Cells(i)
                    Exit For
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Get columns visible
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CountColumnVisible() As Integer
        Dim colCount = 0
        For i As Integer = 0 To MyBase.ColumnCount - 1
            If MyBase.Columns(i).Visible Then
                colCount = colCount + 1
            End If
        Next
        Return colCount - 1
    End Function
End Class