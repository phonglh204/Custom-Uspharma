Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports libscontrol

Public Class frmChangeCellValue
    Inherits Form
    ' Methods
    Private _fieldName As String
    Public Sub New(ByVal fieldName As String)
        _fieldName = fieldName
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDate_Load)
        Me.InitializeComponent()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        Me.Close()
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmDate_Load(ByVal sender As Object, ByVal e As EventArgs)
        Me.Text = "Change quantity or price form" + _fieldName
        'Dim control As Control
        'For Each control In Me.Controls
        '    If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
        '        control.Text = StringType.FromObject(modVoucher.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
        '    End If
        'Next
        Obj.Init(Me)
    End Sub

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.grpInfor = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtLine_nbr = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtMa_vt = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtGia_nt_new = New libscontrol.txtNumeric()
        Me.txtGia_nt = New libscontrol.txtNumeric()
        Me.txtSo_luong_new = New libscontrol.txtNumeric()
        Me.txtSo_luong = New libscontrol.txtNumeric()
        Me.SuspendLayout()
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdOk.Location = New System.Drawing.Point(10, 146)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(90, 27)
        Me.cmdOk.TabIndex = 2
        Me.cmdOk.Tag = ""
        Me.cmdOk.Text = "&Nhận"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(101, 146)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(90, 27)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Tag = ""
        Me.cmdCancel.Text = "&Hủy"
        '
        'grpInfor
        '
        Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpInfor.Location = New System.Drawing.Point(10, 2)
        Me.grpInfor.Name = "grpInfor"
        Me.grpInfor.Size = New System.Drawing.Size(588, 138)
        Me.grpInfor.TabIndex = 17
        Me.grpInfor.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(28, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 17)
        Me.Label2.TabIndex = 21
        Me.Label2.Tag = ""
        Me.Label2.Text = "Quantity"
        '
        'txtLine_nbr
        '
        Me.txtLine_nbr.BackColor = System.Drawing.SystemColors.Window
        Me.txtLine_nbr.Enabled = False
        Me.txtLine_nbr.Location = New System.Drawing.Point(164, 39)
        Me.txtLine_nbr.Name = "txtLine_nbr"
        Me.txtLine_nbr.ReadOnly = True
        Me.txtLine_nbr.Size = New System.Drawing.Size(120, 22)
        Me.txtLine_nbr.TabIndex = 4
        Me.txtLine_nbr.Tag = ""
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(28, 43)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 17)
        Me.Label3.TabIndex = 23
        Me.Label3.Tag = ""
        Me.Label3.Text = "Line number"
        '
        'txtMa_vt
        '
        Me.txtMa_vt.BackColor = System.Drawing.SystemColors.Window
        Me.txtMa_vt.Enabled = False
        Me.txtMa_vt.Location = New System.Drawing.Point(164, 14)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.ReadOnly = True
        Me.txtMa_vt.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_vt.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 17)
        Me.Label1.TabIndex = 19
        Me.Label1.Tag = ""
        Me.Label1.Text = "Item Code"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(28, 94)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 17)
        Me.Label5.TabIndex = 26
        Me.Label5.Tag = ""
        Me.Label5.Text = "Price"
        '
        'txtGia_nt_new
        '
        Me.txtGia_nt_new.BackColor = System.Drawing.Color.White
        Me.txtGia_nt_new.ForeColor = System.Drawing.Color.Black
        Me.txtGia_nt_new.Format = ""
        Me.txtGia_nt_new.Location = New System.Drawing.Point(290, 96)
        Me.txtGia_nt_new.MaxLength = 1
        Me.txtGia_nt_new.Name = "txtGia_nt_new"
        Me.txtGia_nt_new.Size = New System.Drawing.Size(120, 22)
        Me.txtGia_nt_new.TabIndex = 1
        Me.txtGia_nt_new.Tag = ""
        Me.txtGia_nt_new.Text = "0"
        Me.txtGia_nt_new.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtGia_nt_new.Value = 0R
        '
        'txtGia_nt
        '
        Me.txtGia_nt.BackColor = System.Drawing.Color.White
        Me.txtGia_nt.Enabled = False
        Me.txtGia_nt.ForeColor = System.Drawing.Color.Black
        Me.txtGia_nt.Format = ""
        Me.txtGia_nt.Location = New System.Drawing.Point(164, 96)
        Me.txtGia_nt.MaxLength = 1
        Me.txtGia_nt.Name = "txtGia_nt"
        Me.txtGia_nt.Size = New System.Drawing.Size(120, 22)
        Me.txtGia_nt.TabIndex = 30
        Me.txtGia_nt.Tag = ""
        Me.txtGia_nt.Text = "0"
        Me.txtGia_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtGia_nt.Value = 0R
        '
        'txtSo_luong_new
        '
        Me.txtSo_luong_new.BackColor = System.Drawing.Color.White
        Me.txtSo_luong_new.ForeColor = System.Drawing.Color.Black
        Me.txtSo_luong_new.Format = ""
        Me.txtSo_luong_new.Location = New System.Drawing.Point(290, 68)
        Me.txtSo_luong_new.MaxLength = 1
        Me.txtSo_luong_new.Name = "txtSo_luong_new"
        Me.txtSo_luong_new.Size = New System.Drawing.Size(120, 22)
        Me.txtSo_luong_new.TabIndex = 0
        Me.txtSo_luong_new.Tag = ""
        Me.txtSo_luong_new.Text = "0"
        Me.txtSo_luong_new.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSo_luong_new.Value = 0R
        '
        'txtSo_luong
        '
        Me.txtSo_luong.BackColor = System.Drawing.Color.White
        Me.txtSo_luong.Enabled = False
        Me.txtSo_luong.ForeColor = System.Drawing.Color.Black
        Me.txtSo_luong.Format = ""
        Me.txtSo_luong.Location = New System.Drawing.Point(164, 68)
        Me.txtSo_luong.MaxLength = 1
        Me.txtSo_luong.Name = "txtSo_luong"
        Me.txtSo_luong.Size = New System.Drawing.Size(120, 22)
        Me.txtSo_luong.TabIndex = 28
        Me.txtSo_luong.Tag = ""
        Me.txtSo_luong.Text = "0"
        Me.txtSo_luong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSo_luong.Value = 0R
        '
        'frmChangeCellValue
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(608, 178)
        Me.Controls.Add(Me.txtGia_nt_new)
        Me.Controls.Add(Me.txtGia_nt)
        Me.Controls.Add(Me.txtSo_luong_new)
        Me.Controls.Add(Me.txtSo_luong)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtLine_nbr)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtMa_vt)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.grpInfor)
        Me.Name = "frmChangeCellValue"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmDate"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub


    ' Properties
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtLine_nbr As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtMa_vt As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtSo_luong As txtNumeric
    Friend WithEvents txtSo_luong_new As txtNumeric
    Friend WithEvents txtGia_nt As txtNumeric
    Friend WithEvents txtGia_nt_new As txtNumeric
    Private components As IContainer
End Class

