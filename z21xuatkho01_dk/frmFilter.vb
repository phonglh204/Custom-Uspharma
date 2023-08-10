Imports System.Windows.Forms
Imports System.Data.SqlClient
Imports libscontrol.voucherseachlib
Imports libscontrol.reportformlib
Imports libscontrol
Imports libscommon

Public Class frmFilter
    Inherits System.Windows.Forms.Form
    Public pnContent As StatusBarPanel
    Public ds As New DataSet
    Dim dvOrder As New DataView
    Dim intGroup1 As Integer, intGroup2 As Integer, intGroup3 As Integer
    Dim flag As Boolean = False

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
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents cmdOk As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents tabReports As System.Windows.Forms.TabControl
    Friend WithEvents tbgFilter As System.Windows.Forms.TabPage
    Friend WithEvents tbgOptions As System.Windows.Forms.TabPage
    Friend WithEvents tbgFree As System.Windows.Forms.TabPage
    Friend WithEvents lblMau_bc As Label
    Friend WithEvents cboReports As System.Windows.Forms.ComboBox
    Friend WithEvents lblTitle As Label
    Friend WithEvents txtTitle As TextBox
    Friend WithEvents lblDateFromTo As Label
    Friend WithEvents txtDFrom As txtDate
    Friend WithEvents txtDTo As txtDate
    Friend WithEvents lblMa_td3 As Label
    Friend WithEvents txtMa_td3 As TextBox
    Friend WithEvents lblTen_td2 As Label
    Friend WithEvents lblTen_td3 As Label
    Friend WithEvents lblTen_td1 As Label
    Friend WithEvents lblMa_td1 As Label
    Friend WithEvents lblMa_td2 As Label
    Friend WithEvents txtMa_td2 As TextBox
    Friend WithEvents txtMa_td1 As TextBox
    Friend WithEvents lblTk_co As Label
    Friend WithEvents txtMa_kh As TextBox
    Friend WithEvents lblTen_kh As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtInvTo As TextBox
    Friend WithEvents txtInvFrom As TextBox
    Friend WithEvents txtMa_vt As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtMa_vv As TextBox
    Friend WithEvents txtMa_nx As TextBox
    Friend WithEvents txtMa_kho As TextBox
    Friend WithEvents lblTen_vv As Label
    Friend WithEvents lblTen_nx As Label
    Friend WithEvents lblTen_kho As Label
    Friend WithEvents lblTen_vt As Label
    Friend WithEvents tbgAdv As System.Windows.Forms.TabPage
    Friend WithEvents tbgOrder As System.Windows.Forms.TabPage
    Friend WithEvents grdOrder As clsgrid
    Friend WithEvents tbgTransCode As System.Windows.Forms.TabPage
    Friend WithEvents grdTransCode As clsgrid
    Friend WithEvents tbgOther As System.Windows.Forms.TabPage
    Friend WithEvents Label6 As Label
    Friend WithEvents txtTk_vt As TextBox
    Friend WithEvents lblTen_Tk_vt As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents txtLoai_vt As TextBox
    Friend WithEvents lblTen_loai As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents txtNh_vt As TextBox
    Friend WithEvents lblTen_nh As Label
    Friend WithEvents CbbTinh_dc As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtMa_nh1 As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtMa_nh3 As TextBox
    Friend WithEvents txtMa_nh2 As TextBox
    Friend WithEvents lblTen_nh1 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents CboGroupBy As System.Windows.Forms.ComboBox
    Friend WithEvents CboDetailBy As System.Windows.Forms.ComboBox
    Friend WithEvents txtNh_vt2 As TextBox
    Friend WithEvents lblTen_nh2 As Label
    Friend WithEvents txtNh_vt3 As TextBox
    Friend WithEvents lblTen_nh3 As Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.txtMa_dvcs = New TextBox
        Me.lblMa_dvcs = New Label
        Me.lblTen_dvcs = New Label
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tabReports = New System.Windows.Forms.TabControl
        Me.tbgFilter = New System.Windows.Forms.TabPage
        Me.txtNh_vt2 = New TextBox
        Me.txtNh_vt3 = New TextBox
        Me.txtMa_nh3 = New TextBox
        Me.txtMa_nh2 = New TextBox
        Me.txtMa_nh1 = New TextBox
        Me.Label9 = New Label
        Me.Label7 = New Label
        Me.txtNh_vt = New TextBox
        Me.Label6 = New Label
        Me.txtTk_vt = New TextBox
        Me.lblTen_Tk_vt = New Label
        Me.Label8 = New Label
        Me.txtLoai_vt = New TextBox
        Me.lblTen_loai = New Label
        Me.lblTen_vt = New Label
        Me.txtMa_vt = New TextBox
        Me.Label5 = New Label
        Me.txtInvTo = New TextBox
        Me.txtInvFrom = New TextBox
        Me.Label4 = New Label
        Me.lblTen_vv = New Label
        Me.lblTen_nx = New Label
        Me.lblTen_kho = New Label
        Me.Label3 = New Label
        Me.Label2 = New Label
        Me.txtMa_vv = New TextBox
        Me.txtMa_nx = New TextBox
        Me.txtMa_kho = New TextBox
        Me.Label1 = New Label
        Me.lblTen_kh = New Label
        Me.txtMa_kh = New TextBox
        Me.lblTk_co = New Label
        Me.txtDTo = New txtDate
        Me.txtDFrom = New txtDate
        Me.lblDateFromTo = New Label
        Me.lblMau_bc = New Label
        Me.cboReports = New System.Windows.Forms.ComboBox
        Me.lblTitle = New Label
        Me.txtTitle = New TextBox
        Me.tbgOther = New System.Windows.Forms.TabPage
        Me.tbgOptions = New System.Windows.Forms.TabPage
        Me.CboDetailBy = New System.Windows.Forms.ComboBox
        Me.Label12 = New Label
        Me.CboGroupBy = New System.Windows.Forms.ComboBox
        Me.Label10 = New Label
        Me.CbbTinh_dc = New System.Windows.Forms.ComboBox
        Me.Label11 = New Label
        Me.tbgFree = New System.Windows.Forms.TabPage
        Me.lblMa_td1 = New Label
        Me.txtMa_td1 = New TextBox
        Me.txtMa_td2 = New TextBox
        Me.txtMa_td3 = New TextBox
        Me.lblTen_td2 = New Label
        Me.lblTen_td3 = New Label
        Me.lblMa_td3 = New Label
        Me.lblMa_td2 = New Label
        Me.lblTen_td1 = New Label
        Me.tbgAdv = New System.Windows.Forms.TabPage
        Me.tbgOrder = New System.Windows.Forms.TabPage
        Me.grdOrder = New clsgrid
        Me.tbgTransCode = New System.Windows.Forms.TabPage
        Me.grdTransCode = New clsgrid
        Me.lblTen_nh = New Label
        Me.lblTen_nh1 = New Label
        Me.lblTen_nh2 = New Label
        Me.lblTen_nh3 = New Label
        Me.tabReports.SuspendLayout()
        Me.tbgFilter.SuspendLayout()
        Me.tbgOptions.SuspendLayout()
        Me.tbgFree.SuspendLayout()
        Me.tbgOrder.SuspendLayout()
        CType(Me.grdOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbgTransCode.SuspendLayout()
        CType(Me.grdTransCode, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtMa_dvcs
        '
        Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New System.Drawing.Point(160, 266)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.TabIndex = 17
        Me.txtMa_dvcs.Tag = "FCML"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        '
        'lblMa_dvcs
        '
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New System.Drawing.Point(20, 268)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New System.Drawing.Size(36, 16)
        Me.lblMa_dvcs.TabIndex = 1
        Me.lblMa_dvcs.Tag = "L102"
        Me.lblMa_dvcs.Text = "Don vi"
        '
        'lblTen_dvcs
        '
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New System.Drawing.Point(264, 268)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New System.Drawing.Size(50, 16)
        Me.lblTen_dvcs.TabIndex = 7
        Me.lblTen_dvcs.Tag = "L002"
        Me.lblTen_dvcs.Text = "Ten dvcs"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(3, 380)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Tag = "L001"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(79, 380)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Tag = "L002"
        Me.cmdCancel.Text = "Huy"
        '
        'tabReports
        '
        Me.tabReports.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabReports.Controls.Add(Me.tbgFilter)
        Me.tabReports.Controls.Add(Me.tbgOther)
        Me.tabReports.Controls.Add(Me.tbgOptions)
        Me.tabReports.Controls.Add(Me.tbgFree)
        Me.tabReports.Controls.Add(Me.tbgAdv)
        Me.tabReports.Controls.Add(Me.tbgOrder)
        Me.tabReports.Controls.Add(Me.tbgTransCode)
        Me.tabReports.Location = New System.Drawing.Point(-2, 0)
        Me.tabReports.Name = "tabReports"
        Me.tabReports.SelectedIndex = 0
        Me.tabReports.Size = New System.Drawing.Size(609, 368)
        Me.tabReports.TabIndex = 0
        Me.tabReports.Tag = "L200"
        '
        'tbgFilter
        '
        Me.tbgFilter.Controls.Add(Me.txtNh_vt2)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt3)
        Me.tbgFilter.Controls.Add(Me.txtMa_nh3)
        Me.tbgFilter.Controls.Add(Me.txtMa_nh2)
        Me.tbgFilter.Controls.Add(Me.txtMa_nh1)
        Me.tbgFilter.Controls.Add(Me.Label9)
        Me.tbgFilter.Controls.Add(Me.Label7)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt)
        Me.tbgFilter.Controls.Add(Me.Label6)
        Me.tbgFilter.Controls.Add(Me.txtTk_vt)
        Me.tbgFilter.Controls.Add(Me.lblTen_Tk_vt)
        Me.tbgFilter.Controls.Add(Me.Label8)
        Me.tbgFilter.Controls.Add(Me.txtLoai_vt)
        Me.tbgFilter.Controls.Add(Me.lblTen_loai)
        Me.tbgFilter.Controls.Add(Me.lblTen_vt)
        Me.tbgFilter.Controls.Add(Me.txtMa_vt)
        Me.tbgFilter.Controls.Add(Me.Label5)
        Me.tbgFilter.Controls.Add(Me.txtInvTo)
        Me.tbgFilter.Controls.Add(Me.txtInvFrom)
        Me.tbgFilter.Controls.Add(Me.Label4)
        Me.tbgFilter.Controls.Add(Me.lblTen_vv)
        Me.tbgFilter.Controls.Add(Me.lblTen_nx)
        Me.tbgFilter.Controls.Add(Me.lblTen_kho)
        Me.tbgFilter.Controls.Add(Me.Label3)
        Me.tbgFilter.Controls.Add(Me.Label2)
        Me.tbgFilter.Controls.Add(Me.txtMa_vv)
        Me.tbgFilter.Controls.Add(Me.txtMa_nx)
        Me.tbgFilter.Controls.Add(Me.txtMa_kho)
        Me.tbgFilter.Controls.Add(Me.Label1)
        Me.tbgFilter.Controls.Add(Me.lblTen_kh)
        Me.tbgFilter.Controls.Add(Me.txtMa_kh)
        Me.tbgFilter.Controls.Add(Me.lblTk_co)
        Me.tbgFilter.Controls.Add(Me.txtDTo)
        Me.tbgFilter.Controls.Add(Me.txtDFrom)
        Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
        Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblMau_bc)
        Me.tbgFilter.Controls.Add(Me.cboReports)
        Me.tbgFilter.Controls.Add(Me.lblTitle)
        Me.tbgFilter.Controls.Add(Me.txtTitle)
        Me.tbgFilter.Location = New System.Drawing.Point(4, 22)
        Me.tbgFilter.Name = "tbgFilter"
        Me.tbgFilter.Size = New System.Drawing.Size(601, 342)
        Me.tbgFilter.TabIndex = 0
        Me.tbgFilter.Tag = "L100"
        Me.tbgFilter.Text = "Dieu kien loc"
        '
        'txtNh_vt2
        '
        Me.txtNh_vt2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt2.Location = New System.Drawing.Point(264, 197)
        Me.txtNh_vt2.Name = "txtNh_vt2"
        Me.txtNh_vt2.TabIndex = 13
        Me.txtNh_vt2.Tag = "FCML"
        Me.txtNh_vt2.Text = "TXTNH_VT2"
        '
        'txtNh_vt3
        '
        Me.txtNh_vt3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt3.Location = New System.Drawing.Point(368, 197)
        Me.txtNh_vt3.Name = "txtNh_vt3"
        Me.txtNh_vt3.TabIndex = 14
        Me.txtNh_vt3.Tag = "FCML"
        Me.txtNh_vt3.Text = "TXTNH_VT3"
        '
        'txtMa_nh3
        '
        Me.txtMa_nh3.Location = New System.Drawing.Point(368, 82)
        Me.txtMa_nh3.Name = "txtMa_nh3"
        Me.txtMa_nh3.TabIndex = 7
        Me.txtMa_nh3.Tag = "FCML"
        Me.txtMa_nh3.Text = "txtMa_nh3"
        '
        'txtMa_nh2
        '
        Me.txtMa_nh2.Location = New System.Drawing.Point(264, 82)
        Me.txtMa_nh2.Name = "txtMa_nh2"
        Me.txtMa_nh2.TabIndex = 6
        Me.txtMa_nh2.Tag = "FCML"
        Me.txtMa_nh2.Text = "txtMa_nh2"
        '
        'txtMa_nh1
        '
        Me.txtMa_nh1.Location = New System.Drawing.Point(160, 82)
        Me.txtMa_nh1.Name = "txtMa_nh1"
        Me.txtMa_nh1.TabIndex = 5
        Me.txtMa_nh1.Tag = "FCML"
        Me.txtMa_nh1.Text = "txtMa_nh1"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(20, 84)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(97, 16)
        Me.Label9.TabIndex = 48
        Me.Label9.Tag = "L114"
        Me.Label9.Text = "Nhom khach 1-2-3"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(20, 199)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(65, 16)
        Me.Label7.TabIndex = 45
        Me.Label7.Tag = "L111"
        Me.Label7.Text = "Nhom vat tu"
        '
        'txtNh_vt
        '
        Me.txtNh_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt.Location = New System.Drawing.Point(160, 197)
        Me.txtNh_vt.Name = "txtNh_vt"
        Me.txtNh_vt.TabIndex = 12
        Me.txtNh_vt.Tag = "FCML"
        Me.txtNh_vt.Text = "TXTNH_VT"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(20, 153)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(82, 16)
        Me.Label6.TabIndex = 42
        Me.Label6.Tag = "L109"
        Me.Label6.Text = "Tk vat tu (dmvt)"
        '
        'txtTk_vt
        '
        Me.txtTk_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_vt.Location = New System.Drawing.Point(160, 151)
        Me.txtTk_vt.Name = "txtTk_vt"
        Me.txtTk_vt.TabIndex = 10
        Me.txtTk_vt.Tag = "FCML"
        Me.txtTk_vt.Text = "TXTTK_VT"
        '
        'lblTen_Tk_vt
        '
        Me.lblTen_Tk_vt.AutoSize = True
        Me.lblTen_Tk_vt.Location = New System.Drawing.Point(264, 153)
        Me.lblTen_Tk_vt.Name = "lblTen_Tk_vt"
        Me.lblTen_Tk_vt.Size = New System.Drawing.Size(47, 16)
        Me.lblTen_Tk_vt.TabIndex = 43
        Me.lblTen_Tk_vt.Tag = "L016"
        Me.lblTen_Tk_vt.Text = "Tk vat tu"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(20, 176)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 16)
        Me.Label8.TabIndex = 40
        Me.Label8.Tag = "L110"
        Me.Label8.Text = "Loai vat tu"
        '
        'txtLoai_vt
        '
        Me.txtLoai_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLoai_vt.Location = New System.Drawing.Point(160, 174)
        Me.txtLoai_vt.Name = "txtLoai_vt"
        Me.txtLoai_vt.TabIndex = 11
        Me.txtLoai_vt.Tag = "FCML"
        Me.txtLoai_vt.Text = "TXTLOAI_VT"
        '
        'lblTen_loai
        '
        Me.lblTen_loai.AutoSize = True
        Me.lblTen_loai.Location = New System.Drawing.Point(264, 176)
        Me.lblTen_loai.Name = "lblTen_loai"
        Me.lblTen_loai.Size = New System.Drawing.Size(56, 16)
        Me.lblTen_loai.TabIndex = 41
        Me.lblTen_loai.Tag = "L016"
        Me.lblTen_loai.Text = "Loai vat tu"
        '
        'lblTen_vt
        '
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New System.Drawing.Point(264, 130)
        Me.lblTen_vt.Name = "lblTen_vt"
        Me.lblTen_vt.Size = New System.Drawing.Size(54, 16)
        Me.lblTen_vt.TabIndex = 37
        Me.lblTen_vt.Tag = "RF"
        Me.lblTen_vt.Text = "Ten vat tu"
        '
        'txtMa_vt
        '
        Me.txtMa_vt.Location = New System.Drawing.Point(160, 128)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.TabIndex = 9
        Me.txtMa_vt.Tag = "FCML"
        Me.txtMa_vt.Text = "txtMa_vt"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(20, 38)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(74, 16)
        Me.Label5.TabIndex = 27
        Me.Label5.Tag = "L105"
        Me.Label5.Text = "Hoa don tu so"
        '
        'txtInvTo
        '
        Me.txtInvTo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtInvTo.Location = New System.Drawing.Point(264, 36)
        Me.txtInvTo.MaxLength = 12
        Me.txtInvTo.Name = "txtInvTo"
        Me.txtInvTo.TabIndex = 3
        Me.txtInvTo.Tag = "FCML"
        Me.txtInvTo.Text = "TXTINVTO"
        Me.txtInvTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtInvFrom
        '
        Me.txtInvFrom.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtInvFrom.Location = New System.Drawing.Point(160, 36)
        Me.txtInvFrom.MaxLength = 12
        Me.txtInvFrom.Name = "txtInvFrom"
        Me.txtInvFrom.TabIndex = 2
        Me.txtInvFrom.Tag = "FCML"
        Me.txtInvFrom.Text = "TXTINVFROM"
        Me.txtInvFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 130)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 16)
        Me.Label4.TabIndex = 24
        Me.Label4.Tag = "L108"
        Me.Label4.Text = "Ma vat tu"
        '
        'lblTen_vv
        '
        Me.lblTen_vv.AutoSize = True
        Me.lblTen_vv.Location = New System.Drawing.Point(264, 245)
        Me.lblTen_vv.Name = "lblTen_vv"
        Me.lblTen_vv.Size = New System.Drawing.Size(62, 16)
        Me.lblTen_vv.TabIndex = 22
        Me.lblTen_vv.Tag = "RF"
        Me.lblTen_vv.Text = "Ten vu viec"
        '
        'lblTen_nx
        '
        Me.lblTen_nx.AutoSize = True
        Me.lblTen_nx.Location = New System.Drawing.Point(264, 222)
        Me.lblTen_nx.Name = "lblTen_nx"
        Me.lblTen_nx.Size = New System.Drawing.Size(105, 16)
        Me.lblTen_nx.TabIndex = 21
        Me.lblTen_nx.Tag = "RF"
        Me.lblTen_nx.Text = "Ten dang nhap xuat"
        '
        'lblTen_kho
        '
        Me.lblTen_kho.AutoSize = True
        Me.lblTen_kho.Location = New System.Drawing.Point(264, 107)
        Me.lblTen_kho.Name = "lblTen_kho"
        Me.lblTen_kho.Size = New System.Drawing.Size(73, 16)
        Me.lblTen_kho.TabIndex = 20
        Me.lblTen_kho.Tag = "RF"
        Me.lblTen_kho.Text = "Ten kho hang"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 245)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 16)
        Me.Label3.TabIndex = 19
        Me.Label3.Tag = "L113"
        Me.Label3.Text = "Vu viec"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 222)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 16)
        Me.Label2.TabIndex = 18
        Me.Label2.Tag = "L112"
        Me.Label2.Text = "Dang nhap xuat"
        '
        'txtMa_vv
        '
        Me.txtMa_vv.Location = New System.Drawing.Point(160, 243)
        Me.txtMa_vv.Name = "txtMa_vv"
        Me.txtMa_vv.TabIndex = 16
        Me.txtMa_vv.Tag = "FCML"
        Me.txtMa_vv.Text = "txtMa_vv"
        '
        'txtMa_nx
        '
        Me.txtMa_nx.Location = New System.Drawing.Point(160, 220)
        Me.txtMa_nx.Name = "txtMa_nx"
        Me.txtMa_nx.TabIndex = 15
        Me.txtMa_nx.Tag = "FCML"
        Me.txtMa_nx.Text = "txtMa_nx"
        '
        'txtMa_kho
        '
        Me.txtMa_kho.Location = New System.Drawing.Point(160, 105)
        Me.txtMa_kho.Name = "txtMa_kho"
        Me.txtMa_kho.TabIndex = 8
        Me.txtMa_kho.Tag = "FCML"
        Me.txtMa_kho.Text = "txtMa_kho"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 16)
        Me.Label1.TabIndex = 14
        Me.Label1.Tag = "L107"
        Me.Label1.Text = "Kho hang"
        '
        'lblTen_kh
        '
        Me.lblTen_kh.AutoSize = True
        Me.lblTen_kh.Location = New System.Drawing.Point(264, 61)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(85, 16)
        Me.lblTen_kh.TabIndex = 13
        Me.lblTen_kh.Tag = "RF"
        Me.lblTen_kh.Text = "Ten khach hang"
        '
        'txtMa_kh
        '
        Me.txtMa_kh.Location = New System.Drawing.Point(160, 59)
        Me.txtMa_kh.Name = "txtMa_kh"
        Me.txtMa_kh.TabIndex = 4
        Me.txtMa_kh.Tag = "FCML"
        Me.txtMa_kh.Text = "txtMa_kh"
        '
        'lblTk_co
        '
        Me.lblTk_co.AutoSize = True
        Me.lblTk_co.Location = New System.Drawing.Point(20, 61)
        Me.lblTk_co.Name = "lblTk_co"
        Me.lblTk_co.Size = New System.Drawing.Size(65, 16)
        Me.lblTk_co.TabIndex = 11
        Me.lblTk_co.Tag = "L106"
        Me.lblTk_co.Text = "Khach hang"
        '
        'txtDTo
        '
        Me.txtDTo.Location = New System.Drawing.Point(264, 13)
        Me.txtDTo.MaxLength = 10
        Me.txtDTo.Name = "txtDTo"
        Me.txtDTo.TabIndex = 1
        Me.txtDTo.Tag = "NB"
        Me.txtDTo.Text = "  /  /    "
        Me.txtDTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDTo.Value = New Date(CType(0, Long))
        '
        'txtDFrom
        '
        Me.txtDFrom.Location = New System.Drawing.Point(160, 13)
        Me.txtDFrom.MaxLength = 10
        Me.txtDFrom.Name = "txtDFrom"
        Me.txtDFrom.TabIndex = 0
        Me.txtDFrom.Tag = "NB"
        Me.txtDFrom.Text = "  /  /    "
        Me.txtDFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDFrom.Value = New Date(CType(0, Long))
        '
        'lblDateFromTo
        '
        Me.lblDateFromTo.AutoSize = True
        Me.lblDateFromTo.Location = New System.Drawing.Point(20, 15)
        Me.lblDateFromTo.Name = "lblDateFromTo"
        Me.lblDateFromTo.Size = New System.Drawing.Size(67, 16)
        Me.lblDateFromTo.TabIndex = 0
        Me.lblDateFromTo.Tag = "L101"
        Me.lblDateFromTo.Text = "Tu/den ngay"
        '
        'lblMau_bc
        '
        Me.lblMau_bc.AutoSize = True
        Me.lblMau_bc.Location = New System.Drawing.Point(20, 291)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New System.Drawing.Size(69, 16)
        Me.lblMau_bc.TabIndex = 2
        Me.lblMau_bc.Tag = "L103"
        Me.lblMau_bc.Text = "Mau bao cao"
        '
        'cboReports
        '
        Me.cboReports.Location = New System.Drawing.Point(160, 289)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New System.Drawing.Size(300, 21)
        Me.cboReports.TabIndex = 18
        Me.cboReports.Text = "cboReports"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(20, 315)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(42, 16)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Tag = "L104"
        Me.lblTitle.Text = "Tieu de"
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(160, 313)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(300, 20)
        Me.txtTitle.TabIndex = 19
        Me.txtTitle.Tag = "NB"
        Me.txtTitle.Text = "txtTieu_de"
        '
        'tbgOther
        '
        Me.tbgOther.Location = New System.Drawing.Point(4, 22)
        Me.tbgOther.Name = "tbgOther"
        Me.tbgOther.Size = New System.Drawing.Size(601, 342)
        Me.tbgOther.TabIndex = 3
        Me.tbgOther.Tag = "FreeReportOther"
        Me.tbgOther.Text = "Dieu kien khac"
        '
        'tbgOptions
        '
        Me.tbgOptions.Controls.Add(Me.CboDetailBy)
        Me.tbgOptions.Controls.Add(Me.Label12)
        Me.tbgOptions.Controls.Add(Me.CboGroupBy)
        Me.tbgOptions.Controls.Add(Me.Label10)
        Me.tbgOptions.Controls.Add(Me.CbbTinh_dc)
        Me.tbgOptions.Controls.Add(Me.Label11)
        Me.tbgOptions.Location = New System.Drawing.Point(4, 22)
        Me.tbgOptions.Name = "tbgOptions"
        Me.tbgOptions.Size = New System.Drawing.Size(601, 342)
        Me.tbgOptions.TabIndex = 1
        Me.tbgOptions.Tag = "L200"
        Me.tbgOptions.Text = "Lua chon"
        '
        'CboDetailBy
        '
        Me.CboDetailBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboDetailBy.Location = New System.Drawing.Point(160, 37)
        Me.CboDetailBy.Name = "CboDetailBy"
        Me.CboDetailBy.Size = New System.Drawing.Size(300, 21)
        Me.CboDetailBy.TabIndex = 140
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(8, 39)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(64, 16)
        Me.Label12.TabIndex = 141
        Me.Label12.Tag = "L202"
        Me.Label12.Text = "Chi tiet theo"
        '
        'CboGroupBy
        '
        Me.CboGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboGroupBy.Location = New System.Drawing.Point(160, 13)
        Me.CboGroupBy.Name = "CboGroupBy"
        Me.CboGroupBy.Size = New System.Drawing.Size(300, 21)
        Me.CboGroupBy.TabIndex = 138
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(8, 15)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(59, 16)
        Me.Label10.TabIndex = 139
        Me.Label10.Tag = "L201"
        Me.Label10.Text = "Nhom theo"
        '
        'CbbTinh_dc
        '
        Me.CbbTinh_dc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbbTinh_dc.Location = New System.Drawing.Point(160, 61)
        Me.CbbTinh_dc.Name = "CbbTinh_dc"
        Me.CbbTinh_dc.Size = New System.Drawing.Size(300, 21)
        Me.CbbTinh_dc.TabIndex = 3
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(8, 63)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(139, 16)
        Me.Label11.TabIndex = 137
        Me.Label11.Tag = "L203"
        Me.Label11.Text = "Tinh phat sinh dieu chuyen"
        '
        'tbgFree
        '
        Me.tbgFree.Controls.Add(Me.lblMa_td1)
        Me.tbgFree.Controls.Add(Me.txtMa_td1)
        Me.tbgFree.Controls.Add(Me.txtMa_td2)
        Me.tbgFree.Controls.Add(Me.txtMa_td3)
        Me.tbgFree.Controls.Add(Me.lblTen_td2)
        Me.tbgFree.Controls.Add(Me.lblTen_td3)
        Me.tbgFree.Controls.Add(Me.lblMa_td3)
        Me.tbgFree.Controls.Add(Me.lblMa_td2)
        Me.tbgFree.Controls.Add(Me.lblTen_td1)
        Me.tbgFree.Location = New System.Drawing.Point(4, 22)
        Me.tbgFree.Name = "tbgFree"
        Me.tbgFree.Size = New System.Drawing.Size(601, 342)
        Me.tbgFree.TabIndex = 2
        Me.tbgFree.Tag = "FreeReportCaption"
        Me.tbgFree.Text = "Dieu kien ma tu do"
        '
        'lblMa_td1
        '
        Me.lblMa_td1.AutoSize = True
        Me.lblMa_td1.Location = New System.Drawing.Point(20, 16)
        Me.lblMa_td1.Name = "lblMa_td1"
        Me.lblMa_td1.Size = New System.Drawing.Size(57, 16)
        Me.lblMa_td1.TabIndex = 82
        Me.lblMa_td1.Tag = "FreeCaption1"
        Me.lblMa_td1.Text = "Ma tu do 1"
        '
        'txtMa_td1
        '
        Me.txtMa_td1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_td1.Location = New System.Drawing.Point(160, 12)
        Me.txtMa_td1.Name = "txtMa_td1"
        Me.txtMa_td1.TabIndex = 79
        Me.txtMa_td1.Tag = "FCDetail#ma_td1 like '%s%'#ML"
        Me.txtMa_td1.Text = "TXTMA_TD1"
        '
        'txtMa_td2
        '
        Me.txtMa_td2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_td2.Location = New System.Drawing.Point(160, 35)
        Me.txtMa_td2.Name = "txtMa_td2"
        Me.txtMa_td2.TabIndex = 80
        Me.txtMa_td2.Tag = "FCDetail#ma_td2 like '%s%'#ML"
        Me.txtMa_td2.Text = "TXTMA_TD2"
        '
        'txtMa_td3
        '
        Me.txtMa_td3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_td3.Location = New System.Drawing.Point(160, 58)
        Me.txtMa_td3.Name = "txtMa_td3"
        Me.txtMa_td3.TabIndex = 81
        Me.txtMa_td3.Tag = "FCDetail#ma_td3 like '%s%'#ML"
        Me.txtMa_td3.Text = "TXTMA_TD3"
        '
        'lblTen_td2
        '
        Me.lblTen_td2.AutoSize = True
        Me.lblTen_td2.Location = New System.Drawing.Point(272, 39)
        Me.lblTen_td2.Name = "lblTen_td2"
        Me.lblTen_td2.Size = New System.Drawing.Size(61, 16)
        Me.lblTen_td2.TabIndex = 86
        Me.lblTen_td2.Tag = ""
        Me.lblTen_td2.Text = "Ten tu do 2"
        '
        'lblTen_td3
        '
        Me.lblTen_td3.AutoSize = True
        Me.lblTen_td3.Location = New System.Drawing.Point(272, 62)
        Me.lblTen_td3.Name = "lblTen_td3"
        Me.lblTen_td3.Size = New System.Drawing.Size(61, 16)
        Me.lblTen_td3.TabIndex = 87
        Me.lblTen_td3.Tag = ""
        Me.lblTen_td3.Text = "Ten tu do 3"
        '
        'lblMa_td3
        '
        Me.lblMa_td3.AutoSize = True
        Me.lblMa_td3.Location = New System.Drawing.Point(20, 62)
        Me.lblMa_td3.Name = "lblMa_td3"
        Me.lblMa_td3.Size = New System.Drawing.Size(57, 16)
        Me.lblMa_td3.TabIndex = 84
        Me.lblMa_td3.Tag = "FreeCaption3"
        Me.lblMa_td3.Text = "Ma tu do 3"
        '
        'lblMa_td2
        '
        Me.lblMa_td2.AutoSize = True
        Me.lblMa_td2.Location = New System.Drawing.Point(20, 39)
        Me.lblMa_td2.Name = "lblMa_td2"
        Me.lblMa_td2.Size = New System.Drawing.Size(57, 16)
        Me.lblMa_td2.TabIndex = 83
        Me.lblMa_td2.Tag = "FreeCaption2"
        Me.lblMa_td2.Text = "Ma tu do 2"
        '
        'lblTen_td1
        '
        Me.lblTen_td1.AutoSize = True
        Me.lblTen_td1.Location = New System.Drawing.Point(272, 16)
        Me.lblTen_td1.Name = "lblTen_td1"
        Me.lblTen_td1.Size = New System.Drawing.Size(61, 16)
        Me.lblTen_td1.TabIndex = 85
        Me.lblTen_td1.Tag = ""
        Me.lblTen_td1.Text = "Ten tu do 1"
        '
        'tbgAdv
        '
        Me.tbgAdv.Location = New System.Drawing.Point(4, 22)
        Me.tbgAdv.Name = "tbgAdv"
        Me.tbgAdv.Size = New System.Drawing.Size(601, 342)
        Me.tbgAdv.TabIndex = 4
        Me.tbgAdv.Tag = "L300"
        Me.tbgAdv.Text = "Loc chi tiet"
        '
        'tbgOrder
        '
        Me.tbgOrder.Controls.Add(Me.grdOrder)
        Me.tbgOrder.Location = New System.Drawing.Point(4, 22)
        Me.tbgOrder.Name = "tbgOrder"
        Me.tbgOrder.Size = New System.Drawing.Size(601, 342)
        Me.tbgOrder.TabIndex = 5
        Me.tbgOrder.Tag = "L400"
        Me.tbgOrder.Text = "Thu tu sap xep"
        '
        'grdOrder
        '
        Me.grdOrder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdOrder.CaptionVisible = False
        Me.grdOrder.DataMember = ""
        Me.grdOrder.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdOrder.Location = New System.Drawing.Point(0, 0)
        Me.grdOrder.Name = "grdOrder"
        Me.grdOrder.Size = New System.Drawing.Size(601, 344)
        Me.grdOrder.TabIndex = 1
        '
        'tbgTransCode
        '
        Me.tbgTransCode.Controls.Add(Me.grdTransCode)
        Me.tbgTransCode.Location = New System.Drawing.Point(4, 22)
        Me.tbgTransCode.Name = "tbgTransCode"
        Me.tbgTransCode.Size = New System.Drawing.Size(601, 342)
        Me.tbgTransCode.TabIndex = 6
        Me.tbgTransCode.Tag = "L500"
        Me.tbgTransCode.Text = "Ma giao dich"
        '
        'grdTransCode
        '
        Me.grdTransCode.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdTransCode.CaptionVisible = False
        Me.grdTransCode.DataMember = ""
        Me.grdTransCode.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdTransCode.Location = New System.Drawing.Point(0, -1)
        Me.grdTransCode.Name = "grdTransCode"
        Me.grdTransCode.Size = New System.Drawing.Size(601, 344)
        Me.grdTransCode.TabIndex = 2
        '
        'lblTen_nh
        '
        Me.lblTen_nh.AutoSize = True
        Me.lblTen_nh.Location = New System.Drawing.Point(240, 400)
        Me.lblTen_nh.Name = "lblTen_nh"
        Me.lblTen_nh.Size = New System.Drawing.Size(43, 16)
        Me.lblTen_nh.TabIndex = 46
        Me.lblTen_nh.Tag = "L015"
        Me.lblTen_nh.Text = "Ten_nh"
        Me.lblTen_nh.Visible = False
        '
        'lblTen_nh1
        '
        Me.lblTen_nh1.AutoSize = True
        Me.lblTen_nh1.Location = New System.Drawing.Point(168, 384)
        Me.lblTen_nh1.Name = "lblTen_nh1"
        Me.lblTen_nh1.Size = New System.Drawing.Size(98, 16)
        Me.lblTen_nh1.TabIndex = 21
        Me.lblTen_nh1.Tag = "RF"
        Me.lblTen_nh1.Text = "Ten nhom khach 1"
        Me.lblTen_nh1.Visible = False
        '
        'lblTen_nh2
        '
        Me.lblTen_nh2.AutoSize = True
        Me.lblTen_nh2.Location = New System.Drawing.Point(304, 392)
        Me.lblTen_nh2.Name = "lblTen_nh2"
        Me.lblTen_nh2.Size = New System.Drawing.Size(43, 16)
        Me.lblTen_nh2.TabIndex = 56
        Me.lblTen_nh2.Tag = "L015"
        Me.lblTen_nh2.Text = "Ten_nh"
        Me.lblTen_nh2.Visible = False
        '
        'lblTen_nh3
        '
        Me.lblTen_nh3.AutoSize = True
        Me.lblTen_nh3.Location = New System.Drawing.Point(360, 392)
        Me.lblTen_nh3.Name = "lblTen_nh3"
        Me.lblTen_nh3.Size = New System.Drawing.Size(43, 16)
        Me.lblTen_nh3.TabIndex = 57
        Me.lblTen_nh3.Tag = "L015"
        Me.lblTen_nh3.Text = "Ten_nh"
        Me.lblTen_nh3.Visible = False
        '
        'frmFilter
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 437)
        Me.Controls.Add(Me.lblTen_nh1)
        Me.Controls.Add(Me.tabReports)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.lblTen_nh3)
        Me.Controls.Add(Me.lblTen_nh2)
        Me.Controls.Add(Me.lblTen_nh)
        Me.Name = "frmFilter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmFilter"
        Me.tabReports.ResumeLayout(False)
        Me.tbgFilter.ResumeLayout(False)
        Me.tbgOptions.ResumeLayout(False)
        Me.tbgFree.ResumeLayout(False)
        Me.tbgOrder.ResumeLayout(False)
        CType(Me.grdOrder, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbgTransCode.ResumeLayout(False)
        CType(Me.grdTransCode, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub frmDirInfor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AddFreeFields(sysConn, tabReports.TabPages(3), 14)
        SetRPFormCaption(Me, tabReports, oLan, oVar, oLen)
        Dim oCust As New vouchersearchlibobj(txtMa_kh, lblTen_kh, sysConn, appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", True, cmdCancel)
        Dim oItem As New vouchersearchlibobj(txtMa_vt, lblTen_vt, sysConn, appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, cmdCancel)
        Dim oStore As New vouchersearchlibobj(txtMa_kho, lblTen_kho, sysConn, appConn, "dmkho", "ma_kho", "ten_kho", "Store", "1=1", True, cmdCancel)
        Dim oItemAccount As New vouchersearchlibobj(txtTk_vt, lblTen_Tk_vt, sysConn, appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, cmdCancel)
        Dim oItemGroup As New vouchersearchlibobj(txtNh_vt, lblTen_nh, sysConn, appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=1", True, cmdCancel)
        Dim oItemGroup2 As New vouchersearchlibobj(txtNh_vt2, lblTen_nh2, sysConn, appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup2", "loai_nh=2", True, cmdCancel)
        Dim oItemGroup3 As New vouchersearchlibobj(txtNh_vt3, lblTen_nh3, sysConn, appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup3", "loai_nh=3", True, cmdCancel)
        Dim oItemType As New vouchersearchlibobj(txtLoai_vt, lblTen_loai, sysConn, appConn, "Dmloaivt", "ma_loai_vt", "ten_loai_vt", "ItemType", "1=1", True, cmdCancel)
        Dim oGroup1 As New vouchersearchlibobj(txtMa_nh1, lblTen_nh1, sysConn, appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=1", True, cmdCancel)
        Dim oGroup2 As New vouchersearchlibobj(txtMa_nh2, lblTen_nh1, sysConn, appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=2", True, cmdCancel)
        Dim oGroup3 As New vouchersearchlibobj(txtMa_nh3, lblTen_nh1, sysConn, appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=3", True, cmdCancel)
        Dim oReason As New vouchersearchlibobj(txtMa_nx, lblTen_nx, sysConn, appConn, "dmnx", "ma_nx", "ten_nx", "Reason", "1=1", True, cmdCancel)
        Dim oJob As New vouchersearchlibobj(txtMa_vv, lblTen_vv, sysConn, appConn, "dmvv", "ma_vv", "ten_vv", "Job", "1=1", True, cmdCancel)
        Dim oUnit As New vouchersearchlibobj(txtMa_dvcs, lblTen_dvcs, sysConn, appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, cmdCancel)
        Dim oFree1 As New vouchersearchlibobj(txtMa_td1, lblTen_td1, sysConn, appConn, "dmtd1", "ma_td", "ten_td", "Free1", "1=1", True, cmdCancel)
        Dim oFree2 As New vouchersearchlibobj(txtMa_td2, lblTen_td2, sysConn, appConn, "dmtd2", "ma_td", "ten_td", "Free2", "1=1", True, cmdCancel)
        Dim oFree3 As New vouchersearchlibobj(txtMa_td3, lblTen_td3, sysConn, appConn, "dmtd3", "ma_td", "ten_td", "Free3", "1=1", True, cmdCancel)
        Me.CancelButton = cmdCancel
        pnContent = clsvoucher.clsVoucher.AddStb(Me)
        Dim pd As New System.Drawing.Printing.PrintDocument
        pnContent.Text = pd.PrinterSettings.PrinterName
        tabReports.TabPages.Remove(tbgFree)
        tabReports.TabPages.Remove(tbgOther)
        If nx = "2" Then
            fPrint.Text = Trim(oLan("901"))
            txtTitle.Text = Trim(oLan("902"))
        Else
            txtTitle.Text = Trim(rpTable.Rows(0).Item("rep_title" + IIf(Reg.GetRegistryKey("Language") = "V", "", "2")))
        End If
        txtDFrom.Value = Reg.GetRegistryKey("DFDFrom")
        txtDTo.Value = Reg.GetRegistryKey("DFDTo")
        oAdvFilter = New clsAdvFilter(Me, tbgAdv, tabReports, appConn, sysConn, pnContent, cmdCancel)
        oAdvFilter.AddAdvSelect(drAdvFilter("cadvtables"))
        oAdvFilter.InitGridOrder(fPrint.grdOrder, SysID, "001", ds, "Order")
        oAdvFilter.InitGridTransCode(fPrint.grdTransCode, nx, ds, "TransCode")
        oAdvFilter.AddComboboxValue(CboGroupBy, SysID, "002", ds, "GroupBy")
        oAdvFilter.AddComboboxValue(CboDetailBy, SysID, "003", ds, "DetailBy")
        oAdvFilter.AddComboboxValue(CbbTinh_dc, SysID, "004", ds, "Transfer")
        oxInv = New xInv(tabReports, pnContent, appConn, sysConn)
    End Sub

    Private Sub cmdOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        If Not CheckEmptyField(Me, tabReports, oVar) Then
            Return
        End If
        If fPrint.CboGroupBy.SelectedValue = fPrint.CboDetailBy.SelectedValue Then
            Msg.Alert(oLan("905"), 2)
            Return
        End If
        strUnit = Trim(txtMa_dvcs.Text)
        dFrom = txtDFrom.Value
        dTo = txtDTo.Value
        Reg.SetRegistryKey("DFDFrom", txtDFrom.Value)
        Reg.SetRegistryKey("DFDTo", txtDTo.Value)
        pnContent.Text = oVar("m_process")
        ShowReport()
        Dim pd As New System.Drawing.Printing.PrintDocument
        pnContent.Text = pd.PrinterSettings.PrinterName
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cboReports_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboReports.SelectedIndexChanged
        If Not IsNothing(rpTable) Then
            If nx = "2" Then
                txtTitle.Text = Trim(oLan("902"))
            Else
                txtTitle.Text = Trim(rpTable.Rows(cboReports.SelectedIndex).Item("rep_title" + IIf(Reg.GetRegistryKey("Language") = "V", "", "2")))
            End If
        End If
    End Sub

End Class

