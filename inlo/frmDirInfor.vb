Imports System.Windows.Forms
Imports libscontrol
Imports libscommon

Public Class frmDirInfor
    Inherits Form
    Dim oldMa_lo As String
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents cmdOk As Button
    Friend WithEvents cmdCancel As Button
    Friend WithEvents lblGhi_chu As Label
    Friend WithEvents txtGhi_chu As TextBox
    Friend WithEvents txtStatus As TextBox
    Friend WithEvents lblStatusMess As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents txtMa_vt As TextBox
    Friend WithEvents lblMa_lo As Label
    Friend WithEvents lblTen_lo As Label
    Friend WithEvents lblMa_vt As Label
    Friend WithEvents lblTen_vt As Label
    Friend WithEvents lblTen_lo2 As Label
    Friend WithEvents txtMa_lo As TextBox
    Friend WithEvents txtNgay_nhap As txtDate
    Friend WithEvents lblNgay_nhap As Label
    Friend WithEvents txtNgay_sx As txtDate
    Friend WithEvents lblNgay_sx As Label
    Friend WithEvents txtNgay_hhsd As txtDate
    Friend WithEvents lblNgay_hhsd As Label
    Friend WithEvents txtNgay_hhbh As txtDate
    Friend WithEvents lblNgay_hhbh As Label
    Friend WithEvents lblMa_vt2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents lblTen_ncc As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblTen_nsx As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtTl_hl As txtNumeric
    Friend WithEvents txtTl_da As txtNumeric
    Friend WithEvents Label6 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents txtGc_td1 As TextBox
    Friend WithEvents txtMa_lo0 As TextBox
    Friend WithEvents txtMa_ncc As TextBox
    Friend WithEvents txtMa_nsx As TextBox
    Friend WithEvents txtQuy_cach As TextBox
    Friend WithEvents txtPkn As TextBox
    Friend WithEvents txtHan_dung As TextBox
    Friend WithEvents cboXuat_xu As CusCombobox
    Friend WithEvents txtMa_lo_sx As TextBox
    Friend WithEvents txtPacks As txtNumeric
    Friend WithEvents Label7 As Label
    Friend WithEvents txtNgay_kt As txtDate
    Friend WithEvents Label5 As Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.txtMa_vt = New System.Windows.Forms.TextBox()
        Me.lblMa_lo = New System.Windows.Forms.Label()
        Me.lblTen_lo = New System.Windows.Forms.Label()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.grpInfor = New System.Windows.Forms.GroupBox()
        Me.lblMa_vt = New System.Windows.Forms.Label()
        Me.lblTen_vt = New System.Windows.Forms.Label()
        Me.lblTen_lo2 = New System.Windows.Forms.Label()
        Me.lblGhi_chu = New System.Windows.Forms.Label()
        Me.txtGhi_chu = New System.Windows.Forms.TextBox()
        Me.lblStatusMess = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.txtStatus = New System.Windows.Forms.TextBox()
        Me.txtMa_lo = New System.Windows.Forms.TextBox()
        Me.txtQuy_cach = New System.Windows.Forms.TextBox()
        Me.txtPkn = New System.Windows.Forms.TextBox()
        Me.txtNgay_nhap = New libscontrol.txtDate()
        Me.lblNgay_nhap = New System.Windows.Forms.Label()
        Me.txtNgay_sx = New libscontrol.txtDate()
        Me.lblNgay_sx = New System.Windows.Forms.Label()
        Me.txtNgay_hhsd = New libscontrol.txtDate()
        Me.lblNgay_hhsd = New System.Windows.Forms.Label()
        Me.txtNgay_hhbh = New libscontrol.txtDate()
        Me.lblNgay_hhbh = New System.Windows.Forms.Label()
        Me.txtMa_lo0 = New System.Windows.Forms.TextBox()
        Me.lblMa_vt2 = New System.Windows.Forms.Label()
        Me.txtMa_ncc = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblTen_ncc = New System.Windows.Forms.Label()
        Me.txtMa_nsx = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblTen_nsx = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtTl_hl = New libscontrol.txtNumeric()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtTl_da = New libscontrol.txtNumeric()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtGc_td1 = New System.Windows.Forms.TextBox()
        Me.txtHan_dung = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cboXuat_xu = New libscontrol.CusCombobox()
        Me.txtMa_lo_sx = New System.Windows.Forms.TextBox()
        Me.txtPacks = New libscontrol.txtNumeric()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtNgay_kt = New libscontrol.txtDate()
        Me.SuspendLayout()
        '
        'txtMa_vt
        '
        Me.txtMa_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_vt.Location = New System.Drawing.Point(186, 28)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_vt.TabIndex = 0
        Me.txtMa_vt.Tag = "FCNBDFML"
        Me.txtMa_vt.Text = "TXTMA_VT"
        '
        'lblMa_lo
        '
        Me.lblMa_lo.AutoSize = True
        Me.lblMa_lo.Location = New System.Drawing.Point(28, 59)
        Me.lblMa_lo.Name = "lblMa_lo"
        Me.lblMa_lo.Size = New System.Drawing.Size(40, 17)
        Me.lblMa_lo.TabIndex = 5
        Me.lblMa_lo.Tag = "L002"
        Me.lblMa_lo.Text = "So lo"
        '
        'lblTen_lo
        '
        Me.lblTen_lo.AutoSize = True
        Me.lblTen_lo.Location = New System.Drawing.Point(28, 110)
        Me.lblTen_lo.Name = "lblTen_lo"
        Me.lblTen_lo.Size = New System.Drawing.Size(68, 17)
        Me.lblTen_lo.TabIndex = 7
        Me.lblTen_lo.Tag = "L004"
        Me.lblTen_lo.Text = "Quy cach"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(10, 413)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(90, 26)
        Me.cmdOk.TabIndex = 19
        Me.cmdOk.Tag = "L013"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(101, 413)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(90, 26)
        Me.cmdCancel.TabIndex = 20
        Me.cmdCancel.Tag = "L014"
        Me.cmdCancel.Text = "Huy"
        '
        'grpInfor
        '
        Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpInfor.Location = New System.Drawing.Point(10, 8)
        Me.grpInfor.Name = "grpInfor"
        Me.grpInfor.Size = New System.Drawing.Size(692, 399)
        Me.grpInfor.TabIndex = 17
        Me.grpInfor.TabStop = False
        '
        'lblMa_vt
        '
        Me.lblMa_vt.AutoSize = True
        Me.lblMa_vt.Location = New System.Drawing.Point(28, 32)
        Me.lblMa_vt.Name = "lblMa_vt"
        Me.lblMa_vt.Size = New System.Drawing.Size(66, 17)
        Me.lblMa_vt.TabIndex = 22
        Me.lblMa_vt.Tag = "L001"
        Me.lblMa_vt.Text = "Ma vat tu"
        '
        'lblTen_vt
        '
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New System.Drawing.Point(324, 30)
        Me.lblTen_vt.Name = "lblTen_vt"
        Me.lblTen_vt.Size = New System.Drawing.Size(60, 17)
        Me.lblTen_vt.TabIndex = 25
        Me.lblTen_vt.Tag = "RF"
        Me.lblTen_vt.Text = "Ten kho"
        '
        'lblTen_lo2
        '
        Me.lblTen_lo2.AutoSize = True
        Me.lblTen_lo2.Location = New System.Drawing.Point(28, 136)
        Me.lblTen_lo2.Name = "lblTen_lo2"
        Me.lblTen_lo2.Size = New System.Drawing.Size(127, 17)
        Me.lblTen_lo2.TabIndex = 27
        Me.lblTen_lo2.Tag = "L005"
        Me.lblTen_lo2.Text = "Phieu kiem nghiem"
        '
        'lblGhi_chu
        '
        Me.lblGhi_chu.AutoSize = True
        Me.lblGhi_chu.Location = New System.Drawing.Point(28, 269)
        Me.lblGhi_chu.Name = "lblGhi_chu"
        Me.lblGhi_chu.Size = New System.Drawing.Size(57, 17)
        Me.lblGhi_chu.TabIndex = 35
        Me.lblGhi_chu.Tag = "L010"
        Me.lblGhi_chu.Text = "Ghi chu"
        '
        'txtGhi_chu
        '
        Me.txtGhi_chu.Location = New System.Drawing.Point(186, 267)
        Me.txtGhi_chu.Name = "txtGhi_chu"
        Me.txtGhi_chu.Size = New System.Drawing.Size(502, 22)
        Me.txtGhi_chu.TabIndex = 13
        Me.txtGhi_chu.Tag = "FC"
        Me.txtGhi_chu.Text = "txtGhi_chu"
        '
        'lblStatusMess
        '
        Me.lblStatusMess.AutoSize = True
        Me.lblStatusMess.Location = New System.Drawing.Point(221, 295)
        Me.lblStatusMess.Name = "lblStatusMess"
        Me.lblStatusMess.Size = New System.Drawing.Size(171, 17)
        Me.lblStatusMess.TabIndex = 34
        Me.lblStatusMess.Tag = "L012"
        Me.lblStatusMess.Text = "1 - Co su dung, 0 - Khong"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(28, 295)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(73, 17)
        Me.lblStatus.TabIndex = 33
        Me.lblStatus.Tag = "L011"
        Me.lblStatus.Text = "Trang thai"
        '
        'txtStatus
        '
        Me.txtStatus.Location = New System.Drawing.Point(186, 293)
        Me.txtStatus.MaxLength = 1
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(30, 22)
        Me.txtStatus.TabIndex = 14
        Me.txtStatus.TabStop = False
        Me.txtStatus.Tag = "FN"
        Me.txtStatus.Text = "txtStatus"
        Me.txtStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMa_lo
        '
        Me.txtMa_lo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_lo.Location = New System.Drawing.Point(186, 54)
        Me.txtMa_lo.Name = "txtMa_lo"
        Me.txtMa_lo.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_lo.TabIndex = 1
        Me.txtMa_lo.Tag = "FCNB"
        Me.txtMa_lo.Text = "TXTMA_LO"
        '
        'txtQuy_cach
        '
        Me.txtQuy_cach.Location = New System.Drawing.Point(186, 107)
        Me.txtQuy_cach.Name = "txtQuy_cach"
        Me.txtQuy_cach.Size = New System.Drawing.Size(502, 22)
        Me.txtQuy_cach.TabIndex = 3
        Me.txtQuy_cach.Tag = "FC"
        Me.txtQuy_cach.Text = "txtQuy_cach"
        '
        'txtPkn
        '
        Me.txtPkn.Location = New System.Drawing.Point(186, 134)
        Me.txtPkn.Name = "txtPkn"
        Me.txtPkn.Size = New System.Drawing.Size(502, 22)
        Me.txtPkn.TabIndex = 4
        Me.txtPkn.Tag = "FC"
        Me.txtPkn.Text = "txtPkn"
        '
        'txtNgay_nhap
        '
        Me.txtNgay_nhap.Location = New System.Drawing.Point(186, 81)
        Me.txtNgay_nhap.MaxLength = 10
        Me.txtNgay_nhap.Name = "txtNgay_nhap"
        Me.txtNgay_nhap.Size = New System.Drawing.Size(120, 22)
        Me.txtNgay_nhap.TabIndex = 2
        Me.txtNgay_nhap.Tag = "FD"
        Me.txtNgay_nhap.Text = "01/01/1900"
        Me.txtNgay_nhap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_nhap.Value = New Date(1900, 1, 1, 0, 0, 0, 0)
        '
        'lblNgay_nhap
        '
        Me.lblNgay_nhap.AutoSize = True
        Me.lblNgay_nhap.Location = New System.Drawing.Point(28, 83)
        Me.lblNgay_nhap.Name = "lblNgay_nhap"
        Me.lblNgay_nhap.Size = New System.Drawing.Size(77, 17)
        Me.lblNgay_nhap.TabIndex = 37
        Me.lblNgay_nhap.Tag = "L003"
        Me.lblNgay_nhap.Text = "Ngay nhap"
        '
        'txtNgay_sx
        '
        Me.txtNgay_sx.Location = New System.Drawing.Point(186, 160)
        Me.txtNgay_sx.MaxLength = 10
        Me.txtNgay_sx.Name = "txtNgay_sx"
        Me.txtNgay_sx.Size = New System.Drawing.Size(92, 22)
        Me.txtNgay_sx.TabIndex = 5
        Me.txtNgay_sx.Tag = "FD"
        Me.txtNgay_sx.Text = "01/01/1900"
        Me.txtNgay_sx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_sx.Value = New Date(1900, 1, 1, 0, 0, 0, 0)
        '
        'lblNgay_sx
        '
        Me.lblNgay_sx.AutoSize = True
        Me.lblNgay_sx.Location = New System.Drawing.Point(28, 163)
        Me.lblNgay_sx.Name = "lblNgay_sx"
        Me.lblNgay_sx.Size = New System.Drawing.Size(98, 17)
        Me.lblNgay_sx.TabIndex = 39
        Me.lblNgay_sx.Tag = "L006"
        Me.lblNgay_sx.Text = "Ngay san xuat"
        '
        'txtNgay_hhsd
        '
        Me.txtNgay_hhsd.Location = New System.Drawing.Point(394, 160)
        Me.txtNgay_hhsd.MaxLength = 10
        Me.txtNgay_hhsd.Name = "txtNgay_hhsd"
        Me.txtNgay_hhsd.Size = New System.Drawing.Size(92, 22)
        Me.txtNgay_hhsd.TabIndex = 6
        Me.txtNgay_hhsd.Tag = "FD"
        Me.txtNgay_hhsd.Text = "01/01/1900"
        Me.txtNgay_hhsd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_hhsd.Value = New Date(1900, 1, 1, 0, 0, 0, 0)
        '
        'lblNgay_hhsd
        '
        Me.lblNgay_hhsd.AutoSize = True
        Me.lblNgay_hhsd.Location = New System.Drawing.Point(307, 163)
        Me.lblNgay_hhsd.Name = "lblNgay_hhsd"
        Me.lblNgay_hhsd.Size = New System.Drawing.Size(89, 17)
        Me.lblNgay_hhsd.TabIndex = 41
        Me.lblNgay_hhsd.Tag = "L007"
        Me.lblNgay_hhsd.Text = "Han su dung"
        '
        'txtNgay_hhbh
        '
        Me.txtNgay_hhbh.Location = New System.Drawing.Point(595, 160)
        Me.txtNgay_hhbh.MaxLength = 10
        Me.txtNgay_hhbh.Name = "txtNgay_hhbh"
        Me.txtNgay_hhbh.Size = New System.Drawing.Size(93, 22)
        Me.txtNgay_hhbh.TabIndex = 7
        Me.txtNgay_hhbh.Tag = "FD"
        Me.txtNgay_hhbh.Text = "01/01/1900"
        Me.txtNgay_hhbh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_hhbh.Value = New Date(1900, 1, 1, 0, 0, 0, 0)
        '
        'lblNgay_hhbh
        '
        Me.lblNgay_hhbh.AutoSize = True
        Me.lblNgay_hhbh.Location = New System.Drawing.Point(490, 163)
        Me.lblNgay_hhbh.Name = "lblNgay_hhbh"
        Me.lblNgay_hhbh.Size = New System.Drawing.Size(105, 17)
        Me.lblNgay_hhbh.TabIndex = 43
        Me.lblNgay_hhbh.Tag = "L008"
        Me.lblNgay_hhbh.Text = "Ngay bao hanh"
        '
        'txtMa_lo0
        '
        Me.txtMa_lo0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_lo0.Location = New System.Drawing.Point(186, 237)
        Me.txtMa_lo0.Name = "txtMa_lo0"
        Me.txtMa_lo0.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_lo0.TabIndex = 11
        Me.txtMa_lo0.Tag = "FC"
        Me.txtMa_lo0.Text = "TXTMA_LO0"
        '
        'lblMa_vt2
        '
        Me.lblMa_vt2.AutoSize = True
        Me.lblMa_vt2.Location = New System.Drawing.Point(28, 239)
        Me.lblMa_vt2.Name = "lblMa_vt2"
        Me.lblMa_vt2.Size = New System.Drawing.Size(55, 17)
        Me.lblMa_vt2.TabIndex = 45
        Me.lblMa_vt2.Tag = "L009"
        Me.lblMa_vt2.Text = "Ma phu"
        '
        'txtMa_ncc
        '
        Me.txtMa_ncc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_ncc.Location = New System.Drawing.Point(186, 321)
        Me.txtMa_ncc.Name = "txtMa_ncc"
        Me.txtMa_ncc.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_ncc.TabIndex = 15
        Me.txtMa_ncc.Tag = "FCML"
        Me.txtMa_ncc.Text = "TXTMA_NCC"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 323)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 17)
        Me.Label1.TabIndex = 47
        Me.Label1.Tag = "L015"
        Me.Label1.Text = "Nha cung cap"
        '
        'lblTen_ncc
        '
        Me.lblTen_ncc.AutoSize = True
        Me.lblTen_ncc.Location = New System.Drawing.Point(324, 323)
        Me.lblTen_ncc.Name = "lblTen_ncc"
        Me.lblTen_ncc.Size = New System.Drawing.Size(59, 17)
        Me.lblTen_ncc.TabIndex = 48
        Me.lblTen_ncc.Tag = "RF"
        Me.lblTen_ncc.Text = "Ten ncc"
        '
        'txtMa_nsx
        '
        Me.txtMa_nsx.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_nsx.Location = New System.Drawing.Point(186, 346)
        Me.txtMa_nsx.Name = "txtMa_nsx"
        Me.txtMa_nsx.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_nsx.TabIndex = 16
        Me.txtMa_nsx.Tag = "FCML"
        Me.txtMa_nsx.Text = "TXTMA_NSX"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(28, 348)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(91, 17)
        Me.Label2.TabIndex = 50
        Me.Label2.Tag = "L016"
        Me.Label2.Text = "Nha san xuat"
        '
        'lblTen_nsx
        '
        Me.lblTen_nsx.AutoSize = True
        Me.lblTen_nsx.Location = New System.Drawing.Point(324, 348)
        Me.lblTen_nsx.Name = "lblTen_nsx"
        Me.lblTen_nsx.Size = New System.Drawing.Size(58, 17)
        Me.lblTen_nsx.TabIndex = 51
        Me.lblTen_nsx.Tag = "RF"
        Me.lblTen_nsx.Text = "Ten nsx"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(28, 376)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 17)
        Me.Label4.TabIndex = 53
        Me.Label4.Tag = "L017"
        Me.Label4.Text = "Xuat xu"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(28, 188)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(156, 17)
        Me.Label8.TabIndex = 148
        Me.Label8.Tag = "L018"
        Me.Label8.Text = "Ty le chat luong chot %"
        '
        'txtTl_hl
        '
        Me.txtTl_hl.BackColor = System.Drawing.Color.White
        Me.txtTl_hl.Format = "m_ip_tl"
        Me.txtTl_hl.Location = New System.Drawing.Point(394, 186)
        Me.txtTl_hl.MaxLength = 8
        Me.txtTl_hl.Name = "txtTl_hl"
        Me.txtTl_hl.Size = New System.Drawing.Size(92, 22)
        Me.txtTl_hl.TabIndex = 8
        Me.txtTl_hl.Tag = "FNCF"
        Me.txtTl_hl.Text = "m_ip_tl"
        Me.txtTl_hl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTl_hl.Value = 0R
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(490, 188)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 17)
        Me.Label3.TabIndex = 150
        Me.Label3.Tag = "L023"
        Me.Label3.Text = "Do am"
        '
        'txtTl_da
        '
        Me.txtTl_da.BackColor = System.Drawing.Color.White
        Me.txtTl_da.Format = "m_ip_tl"
        Me.txtTl_da.Location = New System.Drawing.Point(595, 186)
        Me.txtTl_da.MaxLength = 8
        Me.txtTl_da.Name = "txtTl_da"
        Me.txtTl_da.Size = New System.Drawing.Size(93, 22)
        Me.txtTl_da.TabIndex = 9
        Me.txtTl_da.Tag = "FNCF"
        Me.txtTl_da.Text = "m_ip_tl"
        Me.txtTl_da.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTl_da.Value = 0R
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(28, 213)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(89, 17)
        Me.Label6.TabIndex = 153
        Me.Label6.Tag = "L019"
        Me.Label6.Text = "Ghi chu PKH"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(317, 188)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(76, 17)
        Me.Label9.TabIndex = 156
        Me.Label9.Tag = "L022"
        Me.Label9.Text = "Ham luong"
        '
        'txtGc_td1
        '
        Me.txtGc_td1.Location = New System.Drawing.Point(186, 211)
        Me.txtGc_td1.Name = "txtGc_td1"
        Me.txtGc_td1.Size = New System.Drawing.Size(502, 22)
        Me.txtGc_td1.TabIndex = 10
        Me.txtGc_td1.Tag = "FC"
        Me.txtGc_td1.Text = "txtGc_td1"
        '
        'txtHan_dung
        '
        Me.txtHan_dung.Location = New System.Drawing.Point(470, 237)
        Me.txtHan_dung.Name = "txtHan_dung"
        Me.txtHan_dung.Size = New System.Drawing.Size(154, 22)
        Me.txtHan_dung.TabIndex = 12
        Me.txtHan_dung.Tag = "FC"
        Me.txtHan_dung.Text = "TXTHAN_DUNG"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(394, 239)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(70, 17)
        Me.Label5.TabIndex = 158
        Me.Label5.Tag = "L024"
        Me.Label5.Text = "Han dung"
        '
        'cboXuat_xu
        '
        Me.cboXuat_xu.DefaultValue = ""
        Me.cboXuat_xu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboXuat_xu.FieldDisplay = ""
        Me.cboXuat_xu.FieldValue = ""
        Me.cboXuat_xu.IsAutocomplete = False
        Me.cboXuat_xu.IsBlankItem = False
        Me.cboXuat_xu.Location = New System.Drawing.Point(186, 373)
        Me.cboXuat_xu.Name = "cboXuat_xu"
        Me.cboXuat_xu.ReadOnly = False
        Me.cboXuat_xu.Size = New System.Drawing.Size(180, 24)
        Me.cboXuat_xu.TabIndex = 17
        Me.cboXuat_xu.TableName = ""
        Me.cboXuat_xu.Tag = "FC"
        Me.cboXuat_xu.ValDataType = "STRING"
        Me.cboXuat_xu.Value = Nothing
        '
        'txtMa_lo_sx
        '
        Me.txtMa_lo_sx.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_lo_sx.Location = New System.Drawing.Point(296, 418)
        Me.txtMa_lo_sx.Name = "txtMa_lo_sx"
        Me.txtMa_lo_sx.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_lo_sx.TabIndex = 159
        Me.txtMa_lo_sx.Tag = "FC"
        Me.txtMa_lo_sx.Text = "TXTMA_LO_SX"
        Me.txtMa_lo_sx.Visible = False
        '
        'txtPacks
        '
        Me.txtPacks.BackColor = System.Drawing.Color.White
        Me.txtPacks.Format = "m_ip_sl"
        Me.txtPacks.Location = New System.Drawing.Point(493, 421)
        Me.txtPacks.MaxLength = 8
        Me.txtPacks.Name = "txtPacks"
        Me.txtPacks.Size = New System.Drawing.Size(93, 22)
        Me.txtPacks.TabIndex = 160
        Me.txtPacks.Tag = "FN"
        Me.txtPacks.Text = "m_ip_sl"
        Me.txtPacks.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtPacks.Value = 0R
        Me.txtPacks.Visible = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(490, 377)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(82, 17)
        Me.Label7.TabIndex = 162
        Me.Label7.Tag = ""
        Me.Label7.Text = "Ngày Reset"
        '
        'txtNgay_kt
        '
        Me.txtNgay_kt.Location = New System.Drawing.Point(595, 374)
        Me.txtNgay_kt.MaxLength = 10
        Me.txtNgay_kt.Name = "txtNgay_kt"
        Me.txtNgay_kt.Size = New System.Drawing.Size(93, 22)
        Me.txtNgay_kt.TabIndex = 18
        Me.txtNgay_kt.Tag = "FD"
        Me.txtNgay_kt.Text = "01/01/1900"
        Me.txtNgay_kt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_kt.Value = New Date(1900, 1, 1, 0, 0, 0, 0)
        '
        'frmDirInfor
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(712, 445)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtNgay_kt)
        Me.Controls.Add(Me.txtPacks)
        Me.Controls.Add(Me.txtMa_lo_sx)
        Me.Controls.Add(Me.cboXuat_xu)
        Me.Controls.Add(Me.txtHan_dung)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtGc_td1)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtTl_da)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtTl_hl)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtMa_nsx)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblTen_nsx)
        Me.Controls.Add(Me.txtMa_ncc)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblTen_ncc)
        Me.Controls.Add(Me.txtMa_lo0)
        Me.Controls.Add(Me.lblMa_vt2)
        Me.Controls.Add(Me.txtNgay_sx)
        Me.Controls.Add(Me.lblNgay_sx)
        Me.Controls.Add(Me.txtNgay_nhap)
        Me.Controls.Add(Me.lblNgay_nhap)
        Me.Controls.Add(Me.txtPkn)
        Me.Controls.Add(Me.txtQuy_cach)
        Me.Controls.Add(Me.txtMa_lo)
        Me.Controls.Add(Me.lblGhi_chu)
        Me.Controls.Add(Me.txtGhi_chu)
        Me.Controls.Add(Me.lblStatusMess)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.lblTen_lo2)
        Me.Controls.Add(Me.lblTen_lo)
        Me.Controls.Add(Me.lblMa_lo)
        Me.Controls.Add(Me.txtMa_vt)
        Me.Controls.Add(Me.lblMa_vt)
        Me.Controls.Add(Me.lblTen_vt)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.lblNgay_hhbh)
        Me.Controls.Add(Me.txtNgay_hhbh)
        Me.Controls.Add(Me.lblNgay_hhsd)
        Me.Controls.Add(Me.txtNgay_hhsd)
        Me.Controls.Add(Me.grpInfor)
        Me.Name = "frmDirInfor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmDirInfor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub frmDirInfor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = oDirFormLib.oLan("000")
        If oDirFormLib.cAction = "New" Then
            txtStatus.Text = "1"
            oldMa_lo = ""
        End If
        Dim oItem As New DirLib(txtMa_vt, lblTen_vt, oDirFormLib.sysConn, oDirFormLib.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "lo_yn = 1", False, cmdCancel)
        Dim oSupplier As New DirLib(txtMa_ncc, lblTen_ncc, oDirFormLib.sysConn, oDirFormLib.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "cc_yn = 1", True, cmdCancel)
        Dim oManu As New DirLib(txtMa_nsx, lblTen_nsx, oDirFormLib.sysConn, oDirFormLib.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "cc_yn = 1", True, cmdCancel)
        Me.cboXuat_xu.SetItemsF5(oDirFormLib.appConn, "hrlstnational", "1=1", "name", "STRING", "name", True, "name")
        If oDirFormLib.cAction = "Edit" Then
            oldMa_lo = Me.txtMa_lo.Text
            Me.cboXuat_xu.Value = oDirFormLib.oDir.ob.CurDataRow.Item("Xuat_xu")
            'Me.cboXuat_xu.SelectedText = "Anh"
            'Me.cboXuat_xu.SelectedIndex = 3
            Try
                If Me.txtQuy_cach.Text = "" Then
                    Dim dr As DataRow = Sql.GetRow(DirMain.oDirFormLib.appConn, "select * from dbo.ftGetPacksizeItem('" + Me.txtMa_vt.Text.Replace("'", "''") + "')")
                    Me.txtQuy_cach.Text = dr.Item("pack_size")
                    Me.txtPacks.Value = dr.Item("packs")
                End If
                'If CType(findcontrols(DirMain.oDirFormLib.oTab.TabPages.Item(1), "txtsl_td1"), txtNumeric).Value = 0 Then
                '    CType(findcontrols(DirMain.oDirFormLib.oTab.TabPages.Item(1), "txtsl_td1"), txtNumeric).Value = CDec(dr.Item("packs"))
                'End If
            Catch ex As Exception
            End Try
        End If
        AddHandler Me.txtMa_vt.Leave, AddressOf txtMa_vt_Leave
        AddHandler Me.txtNgay_nhap.Leave, AddressOf txtNgay_nhap_Leave
    End Sub

    Private Sub cmdOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        If CInt(Sql.GetValue(oDirFormLib.appConn, "exec spCheckMa_lo '" + Me.txtMa_vt.Text + "', '" + Me.txtMa_lo.Text.Trim + "','" + oDirFormLib.cAction + "','" + oldMa_lo + "'")) = 0 Then
            Msg.Alert("Mã lô đã tồn tại hoặc không hợp lệ")
            Me.txtMa_lo.Focus()
            Me.txtMa_lo.SelectAll()
            Return
        End If
        Me.txtMa_lo_sx.Text = Sql.GetValue(oDirFormLib.appConn, "select dbo.fsDoiMa_lo(" + Sql.ConvertVS2SQLType(Me.txtMa_lo.Text.Trim, "") + ")")
        Me.txtPacks.Value = Sql.GetValue(oDirFormLib.appConn, "select dbo.fsGetquycachthung(" + Sql.ConvertVS2SQLType(Me.txtQuy_cach.Text, "") + ",N'Thùng')")
        DirMain.oDirFormLib.SaveFormDir(Me, Sql.ConvertVS2SQLType(txtMa_vt.Text, "") + ", " + Sql.ConvertVS2SQLType(txtMa_lo.Text, ""))
    End Sub

    Private Sub frmDirInfor_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        oDirFormLib.frmUpdate = New frmDirInfor
    End Sub

    Private Sub txtNgay_nhap_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
        If Trim(txtMa_vt.Text) <> "" And txtNgay_nhap.Text <> Fox.GetEmptyDate() Then
            If Sql.GetValue(oDirFormLib.appConn, "dmvt", "kieu_lo", "ma_vt = '" + Trim(txtMa_vt.Text) + "'") = 1 Then
                If txtNgay_hhsd.Text = Fox.GetEmptyDate() Then
                    Dim iDays As Integer
                    iDays = Sql.GetValue(oDirFormLib.appConn, "dmvt", "so_ngay_sp", "ma_vt = '" + Trim(txtMa_vt.Text) + "'")
                    If iDays <> 0 Then
                        txtNgay_hhsd.Value = txtNgay_nhap.Value.AddDays(iDays)
                    End If
                End If

                If txtNgay_hhbh.Text = Fox.GetEmptyDate() Then
                    Dim iDays As Integer
                    iDays = Sql.GetValue(oDirFormLib.appConn, "dmvt", "so_ngay_bh", "ma_vt = '" + Trim(txtMa_vt.Text) + "'")
                    If iDays <> 0 Then
                        txtNgay_hhbh.Value = txtNgay_nhap.Value.AddDays(iDays)
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub txtMa_vt_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If Me.txtQuy_cach.Text = "" Then
            Dim dr As DataRow = Sql.GetRow(DirMain.oDirFormLib.appConn, "select * from dbo.ftGetPacksizeItem('" + Me.txtMa_vt.Text.Replace("'", "''") + "')")
            Me.txtQuy_cach.Text = dr.Item("pack_size")
            Me.txtPacks.Value = dr.Item("packs")
        End If
    End Sub
    Function findcontrols(ByVal root As Control, ByVal name As String) As Control
        For Each cntrl As Control In root.Controls
            If cntrl.Name.ToLower.Trim = name.ToLower.Trim Then
                Return cntrl
            End If
        Next
        Return Nothing
    End Function
End Class
