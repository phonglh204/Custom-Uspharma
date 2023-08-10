Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon


Public Class frmDirInfor
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Closed, New EventHandler(AddressOf Me.frmDirInfor_Closed)
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
        Me.InitializeComponent()
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        If (StringType.StrCmp(Strings.Trim(Me.txtNgay_hl.Text), Strings.Trim(StringType.FromObject(Fox.GetEmptyDate)), False) = 0) Then
            Me.txtNgay_hl.Focus()
            Msg.Alert(StringType.FromObject(DirMain.oDirFormLib.oVar.Item("m_not_blank")), 2)
        Else
            DirMain.oDirFormLib.SaveFormDir(Me, StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(Sql.ConvertVS2SQLType(Me.txtMa_vt.Text, ""), ", "), Sql.ConvertVS2SQLType(Me.txtNgay_hl.Value, "")), ", ")))
        End If
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmDirInfor_Closed(ByVal sender As Object, ByVal e As EventArgs)
        DirMain.oDirFormLib.frmUpdate = New frmDirInfor
    End Sub

    Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
        Me.lblTy_gia.Text = Strings.Replace(Me.lblTy_gia.Text, "%s", StringType.FromObject(DirMain.oDirFormLib.oOptions.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary)
        Me.txtNgay_hl.AddCalenderControl()
        Dim obj3 As Object = New DirLib(Me.txtMa_vt, Me.lblTen_vt, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", False, Me.cmdCancel)
        'Dim obj2 As Object = New dirlib.DirLibObj.DirLib(Me.txtMa_nt, Me.lblTen_nt, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmnt", "ma_nt", "ten_nt", "ForeginCurrency", "1=1", False, Me.cmdCancel)
        If (StringType.StrCmp(DirMain.oDirFormLib.cAction, "New", False) = 0) Then
            Me.Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("000"))
        Else
            Me.Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("111"))
        End If
        'Me.txtGia_nt2.Format = StringType.FromObject(DirMain.oDirFormLib.oOptions.Item("m_ip_gia_nt"))
        'Me.txtGia_nt2.Value = Me.txtGia_nt2.Value
    End Sub
    Friend WithEvents txtTime2 As txtNumeric
    Friend WithEvents txtTime1 As txtNumeric
    Friend WithEvents txtSo_luong As txtNumeric
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtNgay_hl As txtDate
    Friend WithEvents txtTime3 As txtNumeric
    Friend WithEvents txtTime4 As txtNumeric
    Friend WithEvents txtTime5 As txtNumeric

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtMa_vt = New System.Windows.Forms.TextBox
        Me.lblNgay_ban = New System.Windows.Forms.Label
        Me.lblTy_gia = New System.Windows.Forms.Label
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.grpInfor = New System.Windows.Forms.GroupBox
        Me.lblMa_vt = New System.Windows.Forms.Label
        Me.txtNgay_hl = New txtDate
        Me.txtTime2 = New txtNumeric
        Me.lblTen_vt = New System.Windows.Forms.Label
        Me.txtTime1 = New txtNumeric
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtSo_luong = New txtNumeric
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtTime3 = New txtNumeric
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtTime4 = New txtNumeric
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtTime5 = New txtNumeric
        Me.Label5 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.SuspendLayout()
        '
        'txtMa_vt
        '
        Me.txtMa_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_vt.Location = New System.Drawing.Point(152, 24)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.TabIndex = 0
        Me.txtMa_vt.Tag = "FCNBDFML"
        Me.txtMa_vt.Text = "TXTMA_VT"
        '
        'lblNgay_ban
        '
        Me.lblNgay_ban.AutoSize = True
        Me.lblNgay_ban.Location = New System.Drawing.Point(24, 74)
        Me.lblNgay_ban.Name = "lblNgay_ban"
        Me.lblNgay_ban.Size = New System.Drawing.Size(73, 16)
        Me.lblNgay_ban.TabIndex = 5
        Me.lblNgay_ban.Tag = "L002"
        Me.lblNgay_ban.Text = "Ngay hieu luc"
        '
        'lblTy_gia
        '
        Me.lblTy_gia.AutoSize = True
        Me.lblTy_gia.Location = New System.Drawing.Point(24, 160)
        Me.lblTy_gia.Name = "lblTy_gia"
        Me.lblTy_gia.Size = New System.Drawing.Size(108, 16)
        Me.lblTy_gia.TabIndex = 7
        Me.lblTy_gia.Tag = "L007"
        Me.lblTy_gia.Text = "Dap vien/ dong nang"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(8, 277)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 8
        Me.cmdOk.Tag = "L004"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(84, 277)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.TabIndex = 9
        Me.cmdCancel.Tag = "L005"
        Me.cmdCancel.Text = "Huy"
        '
        'grpInfor
        '
        Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpInfor.Location = New System.Drawing.Point(8, 7)
        Me.grpInfor.Name = "grpInfor"
        Me.grpInfor.Size = New System.Drawing.Size(592, 265)
        Me.grpInfor.TabIndex = 17
        Me.grpInfor.TabStop = False
        '
        'lblMa_vt
        '
        Me.lblMa_vt.AutoSize = True
        Me.lblMa_vt.Location = New System.Drawing.Point(24, 26)
        Me.lblMa_vt.Name = "lblMa_vt"
        Me.lblMa_vt.Size = New System.Drawing.Size(50, 16)
        Me.lblMa_vt.TabIndex = 22
        Me.lblMa_vt.Tag = "L001"
        Me.lblMa_vt.Text = "Ma vat tu"
        '
        'txtNgay_hl
        '
        Me.txtNgay_hl.Location = New System.Drawing.Point(152, 72)
        Me.txtNgay_hl.MaxLength = 10
        Me.txtNgay_hl.Name = "txtNgay_hl"
        Me.txtNgay_hl.TabIndex = 2
        Me.txtNgay_hl.Tag = "FDNB"
        Me.txtNgay_hl.Text = "01/01/1900"
        Me.txtNgay_hl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_hl.Value = New Date(1900, 1, 1, 0, 0, 0, 0)
        '
        'txtTime2
        '
        Me.txtTime2.Format = "##0.00"
        Me.txtTime2.Location = New System.Drawing.Point(152, 160)
        Me.txtTime2.MaxLength = 7
        Me.txtTime2.Name = "txtTime2"
        Me.txtTime2.TabIndex = 4
        Me.txtTime2.Tag = "FN"
        Me.txtTime2.Text = "0.00"
        Me.txtTime2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTime2.Value = 0
        '
        'lblTen_vt
        '
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New System.Drawing.Point(270, 26)
        Me.lblTen_vt.Name = "lblTen_vt"
        Me.lblTen_vt.Size = New System.Drawing.Size(54, 16)
        Me.lblTen_vt.TabIndex = 25
        Me.lblTen_vt.Tag = "RF"
        Me.lblTen_vt.Text = "Ten vat tu"
        '
        'txtTime1
        '
        Me.txtTime1.Format = "##0.00"
        Me.txtTime1.Location = New System.Drawing.Point(152, 136)
        Me.txtTime1.MaxLength = 7
        Me.txtTime1.Name = "txtTime1"
        Me.txtTime1.TabIndex = 3
        Me.txtTime1.Tag = "FN"
        Me.txtTime1.Text = "0.00"
        Me.txtTime1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTime1.Value = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 136)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 16)
        Me.Label1.TabIndex = 27
        Me.Label1.Tag = "L006"
        Me.Label1.Text = "Pha che"
        '
        'txtSo_luong
        '
        Me.txtSo_luong.Format = "m_ip_sl"
        Me.txtSo_luong.Location = New System.Drawing.Point(152, 48)
        Me.txtSo_luong.MaxLength = 8
        Me.txtSo_luong.Name = "txtSo_luong"
        Me.txtSo_luong.TabIndex = 1
        Me.txtSo_luong.Tag = "FN"
        Me.txtSo_luong.Text = "m_ip_sl"
        Me.txtSo_luong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSo_luong.Value = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(24, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 16)
        Me.Label2.TabIndex = 29
        Me.Label2.Tag = "L003"
        Me.Label2.Text = "Co lo"
        '
        'txtTime3
        '
        Me.txtTime3.Format = "##0.00"
        Me.txtTime3.Location = New System.Drawing.Point(152, 184)
        Me.txtTime3.MaxLength = 7
        Me.txtTime3.Name = "txtTime3"
        Me.txtTime3.TabIndex = 5
        Me.txtTime3.Tag = "FN"
        Me.txtTime3.Text = "0.00"
        Me.txtTime3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTime3.Value = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(24, 184)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 16)
        Me.Label3.TabIndex = 31
        Me.Label3.Tag = "L008"
        Me.Label3.Text = "Bao phin"
        '
        'txtTime4
        '
        Me.txtTime4.Format = "##0.00"
        Me.txtTime4.Location = New System.Drawing.Point(152, 208)
        Me.txtTime4.MaxLength = 7
        Me.txtTime4.Name = "txtTime4"
        Me.txtTime4.TabIndex = 6
        Me.txtTime4.Tag = "FN"
        Me.txtTime4.Text = "0.00"
        Me.txtTime4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTime4.Value = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(24, 208)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 16)
        Me.Label4.TabIndex = 33
        Me.Label4.Tag = "L009"
        Me.Label4.Text = "Dong goi 1"
        '
        'txtTime5
        '
        Me.txtTime5.Format = "##0.00"
        Me.txtTime5.Location = New System.Drawing.Point(152, 232)
        Me.txtTime5.MaxLength = 7
        Me.txtTime5.Name = "txtTime5"
        Me.txtTime5.TabIndex = 7
        Me.txtTime5.Tag = "FN"
        Me.txtTime5.Text = "0.00"
        Me.txtTime5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTime5.Value = 0
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(24, 232)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 16)
        Me.Label5.TabIndex = 35
        Me.Label5.Tag = "L010"
        Me.Label5.Text = "Dong goi 2"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 112)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(576, 152)
        Me.GroupBox1.TabIndex = 36
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Dinh muc gio cong (Tinh theo gio)"
        '
        'frmDirInfor
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 305)
        Me.Controls.Add(Me.txtTime5)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtTime4)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtTime3)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtSo_luong)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtTime1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtTime2)
        Me.Controls.Add(Me.txtNgay_hl)
        Me.Controls.Add(Me.lblTy_gia)
        Me.Controls.Add(Me.lblNgay_ban)
        Me.Controls.Add(Me.txtMa_vt)
        Me.Controls.Add(Me.lblMa_vt)
        Me.Controls.Add(Me.lblTen_vt)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.grpInfor)
        Me.Name = "frmDirInfor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmDirInfor"
        Me.ResumeLayout(False)

    End Sub


    ' Properties
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lblMa_vt As Label
    Friend WithEvents lblNgay_ban As Label
    Friend WithEvents lblTen_vt As Label
    Friend WithEvents lblTy_gia As Label
    Friend WithEvents txtMa_vt As TextBox

    Private components As IContainer
End Class

