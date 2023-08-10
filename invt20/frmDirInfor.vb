Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon

Public Class frmDirInfor
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
        AddHandler MyBase.Closed, New EventHandler(AddressOf Me.frmDirInfor_Closed)
        Me.lblInfor = New Label
        Me.InitializeComponent()
    End Sub

    Private Sub chkLo_yn_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.chkTao_lo.Enabled = BooleanType.FromObject(LateBinding.LateGet(sender, Nothing, "Checked", New Object(0 - 1) {}, Nothing, Nothing))
        If Not Me.chkTao_lo.Enabled Then
            Me.chkTao_lo.Checked = False
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Me.isCon = False
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        Me.txtGia_ton.Text = StringType.FromObject(Me.dtCalcType.Rows.Item(Me.cbbGia_ton.SelectedIndex).Item("ma_pp"))
        If (Not Me.chkLo_yn.Checked AndAlso (StringType.StrCmp(StringType.FromObject(Sql.GetValue((oDirFormLib.appConn), ("IF EXISTS (SELECT 1 FROM dmlo WHERE ma_vt='" & Me.txtMa_vt.Text.Trim & "')SELECT 1 AS xValue ELSE SELECT 0 AS xValue"))), "1", False) = 0)) Then
            Msg.Alert(StringType.FromObject(oDirFormLib.oLan.Item("900")), 2)
        End If
        oDirFormLib.SaveFormDir(Me, Me.txtMa_vt.Text)
        Me.isCon = True
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub FillComboBox()
        Dim str As String
        Dim ds As New DataSet
        If (StringType.StrCmp(oDirFormLib.cAction, "New", False) = 0) Then
            str = "SELECT ma_pp, RTRIM(ma_pp) + ' - ' + ten_pp AS xten_pp, RTRIM(ma_pp) + ' - ' + ten_pp2 AS xten_pp2 FROM v20dmpptgtb WHERE status = '1' ORDER BY ma_pp"
        Else
            str = ("SELECT ma_pp, RTRIM(ma_pp) + ' - ' + ten_pp AS xten_pp, RTRIM(ma_pp) + ' - ' + ten_pp2 AS xten_pp2 FROM v20dmpptgtb WHERE status = '1' OR ma_pp = '" & Me.txtGia_ton.Text & "' ORDER BY ma_pp")
        End If
        Sql.SQLRetrieve((oDirFormLib.appConn), str, "dt", (ds))
        Me.dtCalcType = ds.Tables.Item(0)
        Dim num2 As Integer = (Me.dtCalcType.Rows.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num2)
            Me.cbbGia_ton.Items.Add(RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Me.dtCalcType.Rows.Item(i), Nothing, "Item", New Object() {ObjectType.AddObj("xten_pp", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
            If (StringType.StrCmp(Strings.Trim(Me.txtGia_ton.Text), Strings.Trim(StringType.FromObject(Me.dtCalcType.Rows.Item(i).Item("ma_pp"))), False) = 0) Then
                Me.cbbGia_ton.SelectedIndex = i
            End If
            i += 1
        Loop
        If (Me.cbbGia_ton.SelectedIndex < 0) Then
            Me.cbbGia_ton.SelectedIndex = 0
        End If
    End Sub

    Private Sub frmDirInfor_Closed(ByVal sender As Object, ByVal e As EventArgs)
        Dim infor As New frmDirInfor
        oDirFormLib.frmUpdate = infor
        oDirFormLib.oTab = infor.tabInfor
        If Me.isCon Then
            Dim tcSQL As String = String.Concat(New String() {"fs_UpdateItem2UOMConv '", Strings.Trim(Me.txtMa_vt.Text), "', '", Strings.Trim(DirMain.cOldItem), "'"})
            Sql.SQLExecute((oDirFormLib.appConn), tcSQL)
        End If
    End Sub

    Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
        If (StringType.StrCmp(oDirFormLib.cAction, "New", False) = 0) Then
            Me.txtStatus.Text = "1"
            Me.chkVt_ton_kho.Checked = True
            Me.chkKk_yn.Checked = True
            Me.txtKieu_lo.Text = "1"
            Me.txtCach_xuat.Text = "1"
            Me.txtAbc_code.Text = " "
            DirMain.cOldItem = ""
            Me.txtTg_th2.Value = 0
            Me.txtKieu_hd.Text = "0"
            Me.txtHeight0.Value = 100
            If SysID = "Item_Pharmacy" Then
                Me.txtPharmacy_yn.Text = "1"
            Else
                Me.txtPharmacy_yn.Text = "0"
            End If
        Else
            DirMain.cOldItem = Me.txtMa_vt.Text
            Me.txtTg_th2.Value = Me.txtTg_th.Value
        End If
        Me.FillComboBox()

        Dim oPX As New DirLib(Me.TXTS1, Me.lblTen_Px, oDirFormLib.sysConn, oDirFormLib.appConn, "sfdmxuong", "ma_px", "ten_px", "v20sfdmxuong", "1 = 1", True, Me.cmdCancel)
        Dim oThuocTPCN As New CharLib(Me.TXTS2, "0, 1, 2")

        Dim _lib As New CharLib(Me.txtAbc_code, " , A, B, C")
        Dim lib27 As New CharLib(Me.txtGia_ton, "1, 2, 3, 4")
        Dim obj2 As Object = New CharLib(Me.txtStatus, "0, 1")
        Dim lib16 As New CharLib(Me.txtKieu_lo, "1, 2")
        Dim lib11 As New CharLib(Me.txtCach_xuat, "1, 2, 3, 4")
        Dim lib17 As New CharLib(Me.txtKieu_hd, "0, 1, 2, 3")
        Dim lib13 As New DirLib(Me.txtTk_vt, Me.lblTen_tk_vt, oDirFormLib.sysConn, oDirFormLib.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", False, Me.cmdCancel)
        Dim lib3 As New DirLib(Me.txtTk_gv, Me.lblTen_tk_gv, oDirFormLib.sysConn, oDirFormLib.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim lib21 As New DirLib(Me.txtTk_dt, Me.lblTen_tk_dt, oDirFormLib.sysConn, oDirFormLib.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim lib10 As New DirLib(Me.txtTk_dtnb, Me.lblTen_tk_dtnb, oDirFormLib.sysConn, oDirFormLib.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim lib24 As New DirLib(Me.txtTk_tl, Me.lblTen_tk_tl, oDirFormLib.sysConn, oDirFormLib.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim lib2 As New DirLib(Me.txtTk_dl, Me.lblTen_tk_dl, oDirFormLib.sysConn, oDirFormLib.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", True, Me.cmdCancel)
        Dim lib28 As New DirLib(Me.txtTk_spdd, Me.lblTen_tk_spdd, oDirFormLib.sysConn, oDirFormLib.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", True, Me.cmdCancel)
        Dim lib5 As New DirLib(Me.txtTk_cl_vt, Me.lblTen_tk_cl_vt, oDirFormLib.sysConn, oDirFormLib.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", True, Me.cmdCancel)
        Dim lib6 As New DirLib(Me.txtTk_ck, Me.lblTen_tk_ck, oDirFormLib.sysConn, oDirFormLib.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim lib20 As New DirLib(Me.txtTk_cpbh, Me.lblTen_tk_cpbh, oDirFormLib.sysConn, oDirFormLib.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim _text As String = Me.txtDvt.Text
        Me.txtDvt.Text = _text
        Dim lib26 As New DirLib(Me.txtDvt, Me.lblInfor, oDirFormLib.sysConn, oDirFormLib.appConn, "dmdvt", "dvt", "ten_dvt", "UOM", "1=1", False, Me.cmdCancel)
        Dim lib14 As New DirLib(Me.txtLoai_vt, Me.lblTen_loai_vt, oDirFormLib.sysConn, oDirFormLib.appConn, "dmloaivt", "ma_loai_vt", "ten_loai_vt", "ItemType", "1=1", False, Me.cmdCancel)
        Dim lib7 As New DirLib(Me.txtNh_vt1, Me.lblTen_nh_vt1, oDirFormLib.sysConn, oDirFormLib.appConn, "dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh = 1", True, Me.cmdCancel)
        Dim lib8 As New DirLib(Me.txtNh_vt2, Me.lblTen_nh_vt2, oDirFormLib.sysConn, oDirFormLib.appConn, "dmnhvt", "ma_nh", "ten_nh", "ItemGroup2", "loai_nh = 2", True, Me.cmdCancel)
        Dim lib9 As New DirLib(Me.txtNh_vt3, Me.lblTen_nh_vt3, oDirFormLib.sysConn, oDirFormLib.appConn, "dmnhvt", "ma_nh", "ten_nh", "ItemGroup3", "loai_nh = 3", True, Me.cmdCancel)
        Dim lib4 As New DirLib(Me.txtNh_vt9, Me.lblTen_nh_vt9, oDirFormLib.sysConn, oDirFormLib.appConn, "dmnhvt2", "ma_nh", "ten_nh", "ItemPriceClass", "1=1", True, Me.cmdCancel)
        Dim lib18 As New DirLib(Me.txtMa_kh0, Me.lblTen_kh0, oDirFormLib.sysConn, oDirFormLib.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", True, Me.cmdCancel)
        Dim lib22 As New DirLib(Me.txtMa_kh2, Me.lblTen_kh2, oDirFormLib.sysConn, oDirFormLib.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", True, Me.cmdCancel)
        Dim lib15 As New DirLib(Me.txtMa_kh, Me.lblTen_kh, oDirFormLib.sysConn, oDirFormLib.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", True, Me.cmdCancel)
        Dim lib25 As New DirLib(Me.txtMa_thue, Me.lblTen_thue, oDirFormLib.sysConn, oDirFormLib.appConn, "dmthue", "ma_thue", "ten_thue", "Tax", "1=1", True, Me.cmdCancel)
        Dim lib12 As New DirLib(Me.txtMa_thue_nk, Me.lblTen_thue_nk, oDirFormLib.sysConn, oDirFormLib.appConn, "dmthuenk", "ma_thue", "ten_thue", "IMPTax", "1=1", True, Me.cmdCancel)
        'Dim lib23 As New DirLib(Me.txtMa_kho, Me.lblTen_kho, oDirFormLib.sysConn, oDirFormLib.appConn, "dmkho", "ma_kho", "ten_kho", "Site", "1=1", True, Me.cmdCancel)
        Dim getcodeMa_kho As New clsGetcodes.clsGetcodes(txtMa_kho, oDirFormLib.sysConn, oDirFormLib.appConn, "dmkho", "ma_kho", "Site", "Status=1", Me.cmdCancel)
        Dim lib19 As New DirLib(Me.txtMa_lo_trinh, Me.lblTen_lo_trinh, oDirFormLib.sysConn, oDirFormLib.appConn, "phrt", "ma_lo_trinh", "ten_lo_trinh", "v20SFRouting", "1=1", True, Me.cmdCancel)
        Me.oLocation = New dirkeylib(Me.txtMa_vi_tri, Me.lblTen_vi_tri, oDirFormLib.sysConn, oDirFormLib.appConn, "dmvitri", "ma_vi_tri", "ten_vi_tri", "Location", "1=1", True, Me.cmdCancel)
        Me.isCon = False
        Me.txtDvt.Enabled = Me.txtMa_vt.Enabled
        AddHandler Me.chkLo_yn.CheckedChanged, New EventHandler(AddressOf Me.chkLo_yn_CheckedChanged)
        Me.chkLo_yn_CheckedChanged(Me.chkLo_yn, New EventArgs)
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) = 0) Then
            Me.lblNh_vt9.Visible = False
            Me.txtNh_vt9.Visible = False
            Me.lblTen_nh_vt9.Visible = False
            Dim control2 As Control
            For Each control2 In Me.txtNh_vt9.Parent.Controls
                If (((Strings.InStr(control2.Anchor.ToString, "Top", CompareMethod.Binary) > 0) And (Strings.InStr(control2.Anchor.ToString, "Bottom", CompareMethod.Binary) = 0)) AndAlso (control2.Top > Me.txtNh_vt9.Top)) Then
                    Dim control3 As Control = control2
                    control3.Top = (control3.Top - (Me.txtNh_vt9.Height + 3))
                End If
            Next
            Dim activeControl As Control = Me.ActiveControl
            Me.tabInfor.TabPages.Remove(Me.tabPur)
            Me.tabInfor.TabPages.Remove(Me.tabMRP)
            Dim num As Integer = (Me.txtNh_vt9.Height + 3)
            Me.Height = (Me.Height - num)
            Me.Top = (Me.Top + CInt(Math.Round(CDbl((CDbl(num) / 2)))))
            Me.tabInfor.SelectedIndex = 0
            If (Not activeControl Is Nothing) Then
                Me.ActiveControl = activeControl
            ElseIf Me.txtMa_vt.Enabled Then
                Me.ActiveControl = Me.txtMa_vt
            Else
                Me.ActiveControl = Me.txtTen_vt
            End If
        End If
        If (StringType.StrCmp(oDirFormLib.cAction, "Edit", False) = 0) Then
            Me.txtDvt.Text = StringType.FromObject(oDirFormLib.oDir.ob.CurDataRow.Item("dvt"))
        End If
        If ((Not Information.IsNothing(oDirFormLib.oDir.ob.CurDataRow) AndAlso (StringType.StrCmp(oDirFormLib.cAction, "New", False) = 0)) AndAlso (Strings.InStr(StringType.FromObject(Me.txtDvt.Tag), "DF", CompareMethod.Binary) > 0)) Then
            Me.txtDvt.Text = StringType.FromObject(oDirFormLib.oDir.ob.CurDataRow.Item("dvt"))
        End If
        Me.txtDvt.CharacterCasing = CharacterCasing.Normal
        Dim Pharmacy_yn As New CharLib(Me.txtPharmacy_yn, "0, 1")
        If SysID <> "Item" Then
            Me.txtPharmacy_yn.ReadOnly = True
        End If
        If oDirFormLib.cAction = "Edit" Then
            If Not (Sql.GetValue(oDirFormLib.appConn, "PharmacyProduct", "PharmacyCode", "PrdCode=" + Sql.ConvertVS2SQLType(oDirFormLib.oDir.ob.CurDataRow.Item("ma_vt"), "")) Is Nothing) Then
                Me.txtMa_vt.ReadOnly = True
                Me.txtPharmacy_yn.ReadOnly = True
            End If
        End If
    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label15 As Label
    Friend WithEvents TXTS1 As TextBox
    Friend WithEvents lblTen_Px As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents TXTS2 As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents txtXhoatchat As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents txtNong_do_ham_luong As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents txtS4 As txtNumeric
    Friend WithEvents Label20 As Label
    Friend WithEvents txtS5 As txtNumeric
    Friend WithEvents Label21 As Label
    Friend WithEvents txtPharmacy_yn As TextBox
    Friend WithEvents Label22 As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents txtPharmacy_status As TextBox
    Friend WithEvents Label24 As Label
    Friend WithEvents TxtHumidity As txtNumeric

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtMa_vt = New System.Windows.Forms.TextBox()
        Me.txtTen_vt = New System.Windows.Forms.TextBox()
        Me.txtTen_vt2 = New System.Windows.Forms.TextBox()
        Me.lblMa_kho = New System.Windows.Forms.Label()
        Me.lblTen_vt = New System.Windows.Forms.Label()
        Me.lblTen_vt2 = New System.Windows.Forms.Label()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.grpInfor = New System.Windows.Forms.GroupBox()
        Me.lblStatusMess = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.txtStatus = New System.Windows.Forms.TextBox()
        Me.lblMa_vt = New System.Windows.Forms.Label()
        Me.lblDvt = New System.Windows.Forms.Label()
        Me.txtDvt = New System.Windows.Forms.TextBox()
        Me.lblNh_vt1 = New System.Windows.Forms.Label()
        Me.txtNh_vt1 = New System.Windows.Forms.TextBox()
        Me.txtLoai_vt = New System.Windows.Forms.TextBox()
        Me.txtAbc_code = New System.Windows.Forms.TextBox()
        Me.lblLoai_vt = New System.Windows.Forms.Label()
        Me.lblAbc_code = New System.Windows.Forms.Label()
        Me.lblTen_nh_vt1 = New System.Windows.Forms.Label()
        Me.lblTen_loai_vt = New System.Windows.Forms.Label()
        Me.txtGhi_chu = New System.Windows.Forms.TextBox()
        Me.lblGia_ton = New System.Windows.Forms.Label()
        Me.txtGia_ton = New System.Windows.Forms.TextBox()
        Me.lblTk_vt = New System.Windows.Forms.Label()
        Me.txtTk_vt = New System.Windows.Forms.TextBox()
        Me.lblTen_tk_vt = New System.Windows.Forms.Label()
        Me.lblTen_tk_gv = New System.Windows.Forms.Label()
        Me.lblTk_gv = New System.Windows.Forms.Label()
        Me.txtTk_gv = New System.Windows.Forms.TextBox()
        Me.lblTen_tk_dt = New System.Windows.Forms.Label()
        Me.lblTk_dt = New System.Windows.Forms.Label()
        Me.txtTk_dt = New System.Windows.Forms.TextBox()
        Me.lblTen_tk_tl = New System.Windows.Forms.Label()
        Me.lblTk_tl = New System.Windows.Forms.Label()
        Me.txtTk_tl = New System.Windows.Forms.TextBox()
        Me.lblTen_tk_spdd = New System.Windows.Forms.Label()
        Me.lblTk_spdd = New System.Windows.Forms.Label()
        Me.txtTk_spdd = New System.Windows.Forms.TextBox()
        Me.txtSl_min = New libscontrol.txtNumeric()
        Me.lblSl_min = New System.Windows.Forms.Label()
        Me.lblSl_max = New System.Windows.Forms.Label()
        Me.txtSl_max = New libscontrol.txtNumeric()
        Me.tabInfor = New System.Windows.Forms.TabControl()
        Me.tabMain = New System.Windows.Forms.TabPage()
        Me.cbbGia_ton = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtNh_vt3 = New System.Windows.Forms.TextBox()
        Me.lblTen_nh_vt3 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNh_vt2 = New System.Windows.Forms.TextBox()
        Me.lblTen_nh_vt2 = New System.Windows.Forms.Label()
        Me.lblNh_vt9 = New System.Windows.Forms.Label()
        Me.txtNh_vt9 = New System.Windows.Forms.TextBox()
        Me.lblTen_nh_vt9 = New System.Windows.Forms.Label()
        Me.chkNhieu_dvt = New System.Windows.Forms.CheckBox()
        Me.chkKk_yn = New System.Windows.Forms.CheckBox()
        Me.chkLo_yn = New System.Windows.Forms.CheckBox()
        Me.LblMa_thue_nk = New System.Windows.Forms.Label()
        Me.txtMa_thue_nk = New System.Windows.Forms.TextBox()
        Me.lblTen_thue_nk = New System.Windows.Forms.Label()
        Me.lblMa_thue = New System.Windows.Forms.Label()
        Me.txtMa_thue = New System.Windows.Forms.TextBox()
        Me.lblTen_thue = New System.Windows.Forms.Label()
        Me.lblMa_vi_tri = New System.Windows.Forms.Label()
        Me.txtMa_vi_tri = New System.Windows.Forms.TextBox()
        Me.lblTen_vi_tri = New System.Windows.Forms.Label()
        Me.txtMa_kho = New System.Windows.Forms.TextBox()
        Me.chkVt_ton_kho = New System.Windows.Forms.CheckBox()
        Me.tabAccount = New System.Windows.Forms.TabPage()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtTk_cpbh = New System.Windows.Forms.TextBox()
        Me.lblTen_tk_cpbh = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtTk_ck = New System.Windows.Forms.TextBox()
        Me.lblTen_tk_ck = New System.Windows.Forms.Label()
        Me.lblTk_cl_vt = New System.Windows.Forms.Label()
        Me.txtTk_cl_vt = New System.Windows.Forms.TextBox()
        Me.lblTen_tk_cl_vt = New System.Windows.Forms.Label()
        Me.lblTen_tk_dl = New System.Windows.Forms.Label()
        Me.lblTk_dl = New System.Windows.Forms.Label()
        Me.txtTk_dl = New System.Windows.Forms.TextBox()
        Me.lblTk_dtnb = New System.Windows.Forms.Label()
        Me.txtTk_dtnb = New System.Windows.Forms.TextBox()
        Me.lblTen_tk_dtnb = New System.Windows.Forms.Label()
        Me.chkSua_tk_vt = New System.Windows.Forms.CheckBox()
        Me.tabPur = New System.Windows.Forms.TabPage()
        Me.lblTen_kh = New System.Windows.Forms.Label()
        Me.lblMa_kh = New System.Windows.Forms.Label()
        Me.txtMa_kh = New System.Windows.Forms.TextBox()
        Me.lblTen_kh2 = New System.Windows.Forms.Label()
        Me.lblMa_kh2 = New System.Windows.Forms.Label()
        Me.txtMa_kh2 = New System.Windows.Forms.TextBox()
        Me.txtOng_ba = New System.Windows.Forms.TextBox()
        Me.lblOng_ba = New System.Windows.Forms.Label()
        Me.lblTen_kh0 = New System.Windows.Forms.Label()
        Me.lblMa_kh0 = New System.Windows.Forms.Label()
        Me.txtMa_kh0 = New System.Windows.Forms.TextBox()
        Me.lblTg_th = New System.Windows.Forms.Label()
        Me.txtTg_th = New libscontrol.txtNumeric()
        Me.txtDvttg_th = New System.Windows.Forms.TextBox()
        Me.lblSo_lo_chuan = New System.Windows.Forms.Label()
        Me.txtSl_lo_chuan = New libscontrol.txtNumeric()
        Me.txtDvtsl_lo_chuan = New System.Windows.Forms.TextBox()
        Me.tabLot = New System.Windows.Forms.TabPage()
        Me.chkTao_lo = New System.Windows.Forms.CheckBox()
        Me.txtSo_ngay_bh = New libscontrol.txtNumeric()
        Me.lblSo_ngay_bh = New System.Windows.Forms.Label()
        Me.lblCach_xuatMess = New System.Windows.Forms.Label()
        Me.txtCach_xuat = New System.Windows.Forms.TextBox()
        Me.lblCach_xuat = New System.Windows.Forms.Label()
        Me.lblKieu_loMess = New System.Windows.Forms.Label()
        Me.txtKieu_lo = New System.Windows.Forms.TextBox()
        Me.lblKieu_lo = New System.Windows.Forms.Label()
        Me.txtSo_ngay_sp = New libscontrol.txtNumeric()
        Me.lblSo_ngay_sp = New System.Windows.Forms.Label()
        Me.tabMRP = New System.Windows.Forms.TabPage()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtS5 = New libscontrol.txtNumeric()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtS4 = New libscontrol.txtNumeric()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtMa_lo_trinh = New System.Windows.Forms.TextBox()
        Me.lblTen_lo_trinh = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtTg_th2 = New libscontrol.txtNumeric()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtSo_ngay_dh = New libscontrol.txtNumeric()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtVung_hd = New libscontrol.txtNumeric()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtCo_lo = New libscontrol.txtNumeric()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtTon_at = New libscontrol.txtNumeric()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtKieu_hd = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tabStyle = New System.Windows.Forms.TabPage()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.TXTS2 = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TXTS1 = New System.Windows.Forms.TextBox()
        Me.lblTen_Px = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtHumidity = New libscontrol.txtNumeric()
        Me.txtDvtgauge = New System.Windows.Forms.TextBox()
        Me.lblGauge = New System.Windows.Forms.Label()
        Me.txtGauge = New libscontrol.txtNumeric()
        Me.txtDvtdensity = New System.Windows.Forms.TextBox()
        Me.lblDensity = New System.Windows.Forms.Label()
        Me.txtDensity = New libscontrol.txtNumeric()
        Me.txtDvtvolume0 = New System.Windows.Forms.TextBox()
        Me.lblVolume0 = New System.Windows.Forms.Label()
        Me.txtVolume0 = New libscontrol.txtNumeric()
        Me.txtDvtweight0 = New System.Windows.Forms.TextBox()
        Me.lblWeight0 = New System.Windows.Forms.Label()
        Me.txtWeight0 = New libscontrol.txtNumeric()
        Me.txtDvtdiameter = New System.Windows.Forms.TextBox()
        Me.lblDiameter = New System.Windows.Forms.Label()
        Me.txtDiameter = New libscontrol.txtNumeric()
        Me.txtDvtdepth = New System.Windows.Forms.TextBox()
        Me.lblDepth = New System.Windows.Forms.Label()
        Me.txtDepth = New libscontrol.txtNumeric()
        Me.txtDvtwidth0 = New System.Windows.Forms.TextBox()
        Me.lblWidth0 = New System.Windows.Forms.Label()
        Me.txtWidth0 = New libscontrol.txtNumeric()
        Me.txtDvtlength0 = New System.Windows.Forms.TextBox()
        Me.lblLength0 = New System.Windows.Forms.Label()
        Me.txtLength0 = New libscontrol.txtNumeric()
        Me.lblHeight0 = New System.Windows.Forms.Label()
        Me.txtHeight0 = New libscontrol.txtNumeric()
        Me.txtDvtweight2 = New System.Windows.Forms.TextBox()
        Me.lblWeight2 = New System.Windows.Forms.Label()
        Me.txtWeight2 = New libscontrol.txtNumeric()
        Me.txtXstyle = New System.Windows.Forms.TextBox()
        Me.lblXstyle = New System.Windows.Forms.Label()
        Me.txtXsize = New System.Windows.Forms.TextBox()
        Me.lblXsize = New System.Windows.Forms.Label()
        Me.txtXcolor = New System.Windows.Forms.TextBox()
        Me.lblXcolor = New System.Windows.Forms.Label()
        Me.txtPack_size = New System.Windows.Forms.TextBox()
        Me.lblPack_size = New System.Windows.Forms.Label()
        Me.txtDvtpacks = New System.Windows.Forms.TextBox()
        Me.lblPacks = New System.Windows.Forms.Label()
        Me.txtPacks = New libscontrol.txtNumeric()
        Me.txtDvtweight = New System.Windows.Forms.TextBox()
        Me.lblWeight = New System.Windows.Forms.Label()
        Me.txtWeight = New libscontrol.txtNumeric()
        Me.txtDvtvolume = New System.Windows.Forms.TextBox()
        Me.lblVolume = New System.Windows.Forms.Label()
        Me.txtVolume = New libscontrol.txtNumeric()
        Me.txtDvtwidth = New System.Windows.Forms.TextBox()
        Me.lblWidth = New System.Windows.Forms.Label()
        Me.txtWidth = New libscontrol.txtNumeric()
        Me.txtDvtlength = New System.Windows.Forms.TextBox()
        Me.lblLength = New System.Windows.Forms.Label()
        Me.txtLength = New libscontrol.txtNumeric()
        Me.txtDvtheight = New System.Windows.Forms.TextBox()
        Me.txtNuoc_sx = New System.Windows.Forms.TextBox()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.txtMa_vt2 = New System.Windows.Forms.TextBox()
        Me.lblMa_vt2 = New System.Windows.Forms.Label()
        Me.txtHeight = New libscontrol.txtNumeric()
        Me.txtDvtheight0 = New System.Windows.Forms.TextBox()
        Me.grpItem = New System.Windows.Forms.GroupBox()
        Me.lblHeight = New System.Windows.Forms.Label()
        Me.grpFr = New System.Windows.Forms.GroupBox()
        Me.tabOther = New System.Windows.Forms.TabPage()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.txtPharmacy_status = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.txtPharmacy_yn = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.txtNong_do_ham_luong = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtXhoatchat = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblGhi_chu = New System.Windows.Forms.Label()
        Me.tabUD = New System.Windows.Forms.TabPage()
        Me.tabInfor.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabAccount.SuspendLayout()
        Me.tabPur.SuspendLayout()
        Me.tabLot.SuspendLayout()
        Me.tabMRP.SuspendLayout()
        Me.tabStyle.SuspendLayout()
        Me.tabOther.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtMa_vt
        '
        Me.txtMa_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_vt.Location = New System.Drawing.Point(155, 22)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_vt.TabIndex = 0
        Me.txtMa_vt.Tag = "FCNBDF"
        Me.txtMa_vt.Text = "TXTMA_VT"
        '
        'txtTen_vt
        '
        Me.txtTen_vt.Location = New System.Drawing.Point(155, 45)
        Me.txtTen_vt.Name = "txtTen_vt"
        Me.txtTen_vt.Size = New System.Drawing.Size(364, 20)
        Me.txtTen_vt.TabIndex = 1
        Me.txtTen_vt.Tag = "FCNB"
        Me.txtTen_vt.Text = "txtTen_vt"
        '
        'txtTen_vt2
        '
        Me.txtTen_vt2.Location = New System.Drawing.Point(155, 68)
        Me.txtTen_vt2.Name = "txtTen_vt2"
        Me.txtTen_vt2.Size = New System.Drawing.Size(364, 20)
        Me.txtTen_vt2.TabIndex = 2
        Me.txtTen_vt2.Tag = "FC"
        Me.txtTen_vt2.Text = "txtTen_vt2"
        '
        'lblMa_kho
        '
        Me.lblMa_kho.AutoSize = True
        Me.lblMa_kho.Location = New System.Drawing.Point(19, 219)
        Me.lblMa_kho.Name = "lblMa_kho"
        Me.lblMa_kho.Size = New System.Drawing.Size(43, 13)
        Me.lblMa_kho.TabIndex = 6
        Me.lblMa_kho.Tag = "L111"
        Me.lblMa_kho.Text = "Ma kho"
        '
        'lblTen_vt
        '
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New System.Drawing.Point(23, 47)
        Me.lblTen_vt.Name = "lblTen_vt"
        Me.lblTen_vt.Size = New System.Drawing.Size(56, 13)
        Me.lblTen_vt.TabIndex = 7
        Me.lblTen_vt.Tag = "L002"
        Me.lblTen_vt.Text = "Ten vat tu"
        '
        'lblTen_vt2
        '
        Me.lblTen_vt2.AutoSize = True
        Me.lblTen_vt2.Location = New System.Drawing.Point(23, 70)
        Me.lblTen_vt2.Name = "lblTen_vt2"
        Me.lblTen_vt2.Size = New System.Drawing.Size(65, 13)
        Me.lblTen_vt2.TabIndex = 8
        Me.lblTen_vt2.Tag = "L003"
        Me.lblTen_vt2.Text = "Ten vat tu 2"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(8, 615)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 23)
        Me.cmdOk.TabIndex = 4
        Me.cmdOk.Tag = "L004"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(84, 615)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Tag = "L005"
        Me.cmdCancel.Text = "Huy"
        '
        'grpInfor
        '
        Me.grpInfor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpInfor.Location = New System.Drawing.Point(8, 7)
        Me.grpInfor.Name = "grpInfor"
        Me.grpInfor.Size = New System.Drawing.Size(675, 89)
        Me.grpInfor.TabIndex = 14
        Me.grpInfor.TabStop = False
        '
        'lblStatusMess
        '
        Me.lblStatusMess.AutoSize = True
        Me.lblStatusMess.Location = New System.Drawing.Point(184, 357)
        Me.lblStatusMess.Name = "lblStatusMess"
        Me.lblStatusMess.Size = New System.Drawing.Size(128, 13)
        Me.lblStatusMess.TabIndex = 21
        Me.lblStatusMess.Tag = "L118"
        Me.lblStatusMess.Text = "1 - Co su dung, 0 - Khong"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(19, 357)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(55, 13)
        Me.lblStatus.TabIndex = 19
        Me.lblStatus.Tag = "L117"
        Me.lblStatus.Text = "Trang thai"
        '
        'txtStatus
        '
        Me.txtStatus.Location = New System.Drawing.Point(151, 355)
        Me.txtStatus.MaxLength = 1
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(25, 20)
        Me.txtStatus.TabIndex = 17
        Me.txtStatus.TabStop = False
        Me.txtStatus.Tag = "FN"
        Me.txtStatus.Text = "txtStatus"
        Me.txtStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMa_vt
        '
        Me.lblMa_vt.AutoSize = True
        Me.lblMa_vt.Location = New System.Drawing.Point(23, 26)
        Me.lblMa_vt.Name = "lblMa_vt"
        Me.lblMa_vt.Size = New System.Drawing.Size(52, 13)
        Me.lblMa_vt.TabIndex = 21
        Me.lblMa_vt.Tag = "L001"
        Me.lblMa_vt.Text = "Ma vat tu"
        '
        'lblDvt
        '
        Me.lblDvt.AutoSize = True
        Me.lblDvt.Location = New System.Drawing.Point(19, 12)
        Me.lblDvt.Name = "lblDvt"
        Me.lblDvt.Size = New System.Drawing.Size(58, 13)
        Me.lblDvt.TabIndex = 23
        Me.lblDvt.Tag = "L101"
        Me.lblDvt.Text = "Don vi tinh"
        '
        'txtDvt
        '
        Me.txtDvt.Location = New System.Drawing.Point(151, 10)
        Me.txtDvt.Name = "txtDvt"
        Me.txtDvt.Size = New System.Drawing.Size(100, 20)
        Me.txtDvt.TabIndex = 0
        Me.txtDvt.Tag = "FCNBDF"
        Me.txtDvt.Text = "txtDvt"
        '
        'lblNh_vt1
        '
        Me.lblNh_vt1.AutoSize = True
        Me.lblNh_vt1.Location = New System.Drawing.Point(19, 104)
        Me.lblNh_vt1.Name = "lblNh_vt1"
        Me.lblNh_vt1.Size = New System.Drawing.Size(65, 13)
        Me.lblNh_vt1.TabIndex = 30
        Me.lblNh_vt1.Tag = "L109"
        Me.lblNh_vt1.Text = "Nhom vat tu"
        '
        'txtNh_vt1
        '
        Me.txtNh_vt1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt1.Location = New System.Drawing.Point(151, 102)
        Me.txtNh_vt1.Name = "txtNh_vt1"
        Me.txtNh_vt1.Size = New System.Drawing.Size(100, 20)
        Me.txtNh_vt1.TabIndex = 6
        Me.txtNh_vt1.Tag = "FCDF"
        Me.txtNh_vt1.Text = "TXTNH_VT1"
        '
        'txtLoai_vt
        '
        Me.txtLoai_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLoai_vt.Location = New System.Drawing.Point(151, 79)
        Me.txtLoai_vt.Name = "txtLoai_vt"
        Me.txtLoai_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtLoai_vt.TabIndex = 5
        Me.txtLoai_vt.Tag = "FCDFNB"
        Me.txtLoai_vt.Text = "TXTLOAI_VT"
        '
        'txtAbc_code
        '
        Me.txtAbc_code.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAbc_code.Location = New System.Drawing.Point(151, 194)
        Me.txtAbc_code.Name = "txtAbc_code"
        Me.txtAbc_code.Size = New System.Drawing.Size(25, 20)
        Me.txtAbc_code.TabIndex = 10
        Me.txtAbc_code.Tag = "FCDF"
        Me.txtAbc_code.Text = "TXTABC_CODE"
        '
        'lblLoai_vt
        '
        Me.lblLoai_vt.AutoSize = True
        Me.lblLoai_vt.Location = New System.Drawing.Point(19, 81)
        Me.lblLoai_vt.Name = "lblLoai_vt"
        Me.lblLoai_vt.Size = New System.Drawing.Size(57, 13)
        Me.lblLoai_vt.TabIndex = 34
        Me.lblLoai_vt.Tag = "L108"
        Me.lblLoai_vt.Text = "Loai vat tu"
        '
        'lblAbc_code
        '
        Me.lblAbc_code.AutoSize = True
        Me.lblAbc_code.Location = New System.Drawing.Point(19, 196)
        Me.lblAbc_code.Name = "lblAbc_code"
        Me.lblAbc_code.Size = New System.Drawing.Size(28, 13)
        Me.lblAbc_code.TabIndex = 35
        Me.lblAbc_code.Tag = "L110"
        Me.lblAbc_code.Text = "ABC"
        '
        'lblTen_nh_vt1
        '
        Me.lblTen_nh_vt1.AutoSize = True
        Me.lblTen_nh_vt1.Location = New System.Drawing.Point(256, 104)
        Me.lblTen_nh_vt1.Name = "lblTen_nh_vt1"
        Me.lblTen_nh_vt1.Size = New System.Drawing.Size(85, 13)
        Me.lblTen_nh_vt1.TabIndex = 51
        Me.lblTen_nh_vt1.Text = "Ten nhom vat tu"
        '
        'lblTen_loai_vt
        '
        Me.lblTen_loai_vt.AutoSize = True
        Me.lblTen_loai_vt.Location = New System.Drawing.Point(256, 81)
        Me.lblTen_loai_vt.Name = "lblTen_loai_vt"
        Me.lblTen_loai_vt.Size = New System.Drawing.Size(75, 13)
        Me.lblTen_loai_vt.TabIndex = 52
        Me.lblTen_loai_vt.Text = "Ten loai vat tu"
        '
        'txtGhi_chu
        '
        Me.txtGhi_chu.Location = New System.Drawing.Point(72, 10)
        Me.txtGhi_chu.Multiline = True
        Me.txtGhi_chu.Name = "txtGhi_chu"
        Me.txtGhi_chu.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtGhi_chu.Size = New System.Drawing.Size(443, 40)
        Me.txtGhi_chu.TabIndex = 0
        Me.txtGhi_chu.Tag = "FT"
        Me.txtGhi_chu.Text = "txtGhi_chu"
        '
        'lblGia_ton
        '
        Me.lblGia_ton.AutoSize = True
        Me.lblGia_ton.Location = New System.Drawing.Point(19, 58)
        Me.lblGia_ton.Name = "lblGia_ton"
        Me.lblGia_ton.Size = New System.Drawing.Size(108, 13)
        Me.lblGia_ton.TabIndex = 67
        Me.lblGia_ton.Tag = "L106"
        Me.lblGia_ton.Text = "Cach tinh gia ton kho"
        '
        'txtGia_ton
        '
        Me.txtGia_ton.Location = New System.Drawing.Point(368, 55)
        Me.txtGia_ton.MaxLength = 1
        Me.txtGia_ton.Name = "txtGia_ton"
        Me.txtGia_ton.Size = New System.Drawing.Size(25, 20)
        Me.txtGia_ton.TabIndex = 4
        Me.txtGia_ton.Tag = "FNDF"
        Me.txtGia_ton.Text = "txtGia_ton"
        Me.txtGia_ton.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtGia_ton.Visible = False
        '
        'lblTk_vt
        '
        Me.lblTk_vt.AutoSize = True
        Me.lblTk_vt.Location = New System.Drawing.Point(19, 12)
        Me.lblTk_vt.Name = "lblTk_vt"
        Me.lblTk_vt.Size = New System.Drawing.Size(50, 13)
        Me.lblTk_vt.TabIndex = 71
        Me.lblTk_vt.Tag = "L201"
        Me.lblTk_vt.Text = "Tk vat tu"
        '
        'txtTk_vt
        '
        Me.txtTk_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_vt.Location = New System.Drawing.Point(151, 10)
        Me.txtTk_vt.Name = "txtTk_vt"
        Me.txtTk_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_vt.TabIndex = 0
        Me.txtTk_vt.Tag = "FCNBDF"
        Me.txtTk_vt.Text = "TXTTK_VT"
        '
        'lblTen_tk_vt
        '
        Me.lblTen_tk_vt.AutoSize = True
        Me.lblTen_tk_vt.Location = New System.Drawing.Point(256, 12)
        Me.lblTen_tk_vt.Name = "lblTen_tk_vt"
        Me.lblTen_tk_vt.Size = New System.Drawing.Size(68, 13)
        Me.lblTen_tk_vt.TabIndex = 72
        Me.lblTen_tk_vt.Text = "Ten tk vat tu"
        '
        'lblTen_tk_gv
        '
        Me.lblTen_tk_gv.AutoSize = True
        Me.lblTen_tk_gv.Location = New System.Drawing.Point(256, 58)
        Me.lblTen_tk_gv.Name = "lblTen_tk_gv"
        Me.lblTen_tk_gv.Size = New System.Drawing.Size(76, 13)
        Me.lblTen_tk_gv.TabIndex = 75
        Me.lblTen_tk_gv.Text = "Ten tk gia von"
        '
        'lblTk_gv
        '
        Me.lblTk_gv.AutoSize = True
        Me.lblTk_gv.Location = New System.Drawing.Point(19, 58)
        Me.lblTk_gv.Name = "lblTk_gv"
        Me.lblTk_gv.Size = New System.Drawing.Size(58, 13)
        Me.lblTk_gv.TabIndex = 74
        Me.lblTk_gv.Tag = "L203"
        Me.lblTk_gv.Text = "Tk gia von"
        '
        'txtTk_gv
        '
        Me.txtTk_gv.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_gv.Location = New System.Drawing.Point(151, 56)
        Me.txtTk_gv.Name = "txtTk_gv"
        Me.txtTk_gv.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_gv.TabIndex = 2
        Me.txtTk_gv.Tag = "FCDF"
        Me.txtTk_gv.Text = "TXTTK_GV"
        '
        'lblTen_tk_dt
        '
        Me.lblTen_tk_dt.AutoSize = True
        Me.lblTen_tk_dt.Location = New System.Drawing.Point(256, 81)
        Me.lblTen_tk_dt.Name = "lblTen_tk_dt"
        Me.lblTen_tk_dt.Size = New System.Drawing.Size(124, 13)
        Me.lblTen_tk_dt.TabIndex = 81
        Me.lblTen_tk_dt.Text = "Ten tai khoan doanh thu"
        '
        'lblTk_dt
        '
        Me.lblTk_dt.AutoSize = True
        Me.lblTk_dt.Location = New System.Drawing.Point(19, 81)
        Me.lblTk_dt.Name = "lblTk_dt"
        Me.lblTk_dt.Size = New System.Drawing.Size(71, 13)
        Me.lblTk_dt.TabIndex = 80
        Me.lblTk_dt.Tag = "L204"
        Me.lblTk_dt.Text = "Tk doanh thu"
        '
        'txtTk_dt
        '
        Me.txtTk_dt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_dt.Location = New System.Drawing.Point(151, 79)
        Me.txtTk_dt.Name = "txtTk_dt"
        Me.txtTk_dt.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_dt.TabIndex = 3
        Me.txtTk_dt.Tag = "FCDF"
        Me.txtTk_dt.Text = "TXTTK_DT"
        '
        'lblTen_tk_tl
        '
        Me.lblTen_tk_tl.AutoSize = True
        Me.lblTen_tk_tl.Location = New System.Drawing.Point(256, 127)
        Me.lblTen_tk_tl.Name = "lblTen_tk_tl"
        Me.lblTen_tk_tl.Size = New System.Drawing.Size(114, 13)
        Me.lblTen_tk_tl.TabIndex = 84
        Me.lblTen_tk_tl.Text = "Ten tk hang ban tra lai"
        '
        'lblTk_tl
        '
        Me.lblTk_tl.AutoSize = True
        Me.lblTk_tl.Location = New System.Drawing.Point(19, 127)
        Me.lblTk_tl.Name = "lblTk_tl"
        Me.lblTk_tl.Size = New System.Drawing.Size(107, 13)
        Me.lblTk_tl.TabIndex = 83
        Me.lblTk_tl.Tag = "L206"
        Me.lblTk_tl.Text = "Tk hang ban bi tra lai"
        '
        'txtTk_tl
        '
        Me.txtTk_tl.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_tl.Location = New System.Drawing.Point(151, 125)
        Me.txtTk_tl.Name = "txtTk_tl"
        Me.txtTk_tl.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_tl.TabIndex = 5
        Me.txtTk_tl.Tag = "FCDF"
        Me.txtTk_tl.Text = "TXTTK_TL"
        '
        'lblTen_tk_spdd
        '
        Me.lblTen_tk_spdd.AutoSize = True
        Me.lblTen_tk_spdd.Location = New System.Drawing.Point(256, 173)
        Me.lblTen_tk_spdd.Name = "lblTen_tk_spdd"
        Me.lblTen_tk_spdd.Size = New System.Drawing.Size(129, 13)
        Me.lblTen_tk_spdd.TabIndex = 87
        Me.lblTen_tk_spdd.Text = "Ten tk san pham do dang"
        '
        'lblTk_spdd
        '
        Me.lblTk_spdd.AutoSize = True
        Me.lblTk_spdd.Location = New System.Drawing.Point(19, 173)
        Me.lblTk_spdd.Name = "lblTk_spdd"
        Me.lblTk_spdd.Size = New System.Drawing.Size(111, 13)
        Me.lblTk_spdd.TabIndex = 86
        Me.lblTk_spdd.Tag = "L208"
        Me.lblTk_spdd.Text = "Tk san pham do dang"
        '
        'txtTk_spdd
        '
        Me.txtTk_spdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_spdd.Location = New System.Drawing.Point(151, 171)
        Me.txtTk_spdd.Name = "txtTk_spdd"
        Me.txtTk_spdd.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_spdd.TabIndex = 7
        Me.txtTk_spdd.Tag = "FCDF"
        Me.txtTk_spdd.Text = "TXTTK_SPDD"
        '
        'txtSl_min
        '
        Me.txtSl_min.Format = "m_ip_sl"
        Me.txtSl_min.Location = New System.Drawing.Point(151, 309)
        Me.txtSl_min.MaxLength = 8
        Me.txtSl_min.Name = "txtSl_min"
        Me.txtSl_min.Size = New System.Drawing.Size(100, 20)
        Me.txtSl_min.TabIndex = 15
        Me.txtSl_min.Tag = "FN"
        Me.txtSl_min.Text = "m_ip_sl"
        Me.txtSl_min.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSl_min.Value = 0R
        '
        'lblSl_min
        '
        Me.lblSl_min.AutoSize = True
        Me.lblSl_min.Location = New System.Drawing.Point(19, 311)
        Me.lblSl_min.Name = "lblSl_min"
        Me.lblSl_min.Size = New System.Drawing.Size(107, 13)
        Me.lblSl_min.TabIndex = 89
        Me.lblSl_min.Tag = "L115"
        Me.lblSl_min.Text = "So luong ton toi thieu"
        '
        'lblSl_max
        '
        Me.lblSl_max.AutoSize = True
        Me.lblSl_max.Location = New System.Drawing.Point(19, 334)
        Me.lblSl_max.Name = "lblSl_max"
        Me.lblSl_max.Size = New System.Drawing.Size(96, 13)
        Me.lblSl_max.TabIndex = 91
        Me.lblSl_max.Tag = "L116"
        Me.lblSl_max.Text = "So luong ton toi da"
        '
        'txtSl_max
        '
        Me.txtSl_max.Format = "m_ip_sl"
        Me.txtSl_max.Location = New System.Drawing.Point(151, 332)
        Me.txtSl_max.MaxLength = 8
        Me.txtSl_max.Name = "txtSl_max"
        Me.txtSl_max.Size = New System.Drawing.Size(100, 20)
        Me.txtSl_max.TabIndex = 16
        Me.txtSl_max.Tag = "FN"
        Me.txtSl_max.Text = "m_ip_sl"
        Me.txtSl_max.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSl_max.Value = 0R
        '
        'tabInfor
        '
        Me.tabInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabInfor.Controls.Add(Me.tabMain)
        Me.tabInfor.Controls.Add(Me.tabAccount)
        Me.tabInfor.Controls.Add(Me.tabPur)
        Me.tabInfor.Controls.Add(Me.tabLot)
        Me.tabInfor.Controls.Add(Me.tabMRP)
        Me.tabInfor.Controls.Add(Me.tabStyle)
        Me.tabInfor.Controls.Add(Me.tabOther)
        Me.tabInfor.Controls.Add(Me.tabUD)
        Me.tabInfor.Location = New System.Drawing.Point(8, 104)
        Me.tabInfor.Name = "tabInfor"
        Me.tabInfor.SelectedIndex = 0
        Me.tabInfor.Size = New System.Drawing.Size(675, 503)
        Me.tabInfor.TabIndex = 3
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.cbbGia_ton)
        Me.tabMain.Controls.Add(Me.Label5)
        Me.tabMain.Controls.Add(Me.txtNh_vt3)
        Me.tabMain.Controls.Add(Me.lblTen_nh_vt3)
        Me.tabMain.Controls.Add(Me.Label3)
        Me.tabMain.Controls.Add(Me.txtNh_vt2)
        Me.tabMain.Controls.Add(Me.lblTen_nh_vt2)
        Me.tabMain.Controls.Add(Me.lblNh_vt9)
        Me.tabMain.Controls.Add(Me.txtNh_vt9)
        Me.tabMain.Controls.Add(Me.lblTen_nh_vt9)
        Me.tabMain.Controls.Add(Me.chkNhieu_dvt)
        Me.tabMain.Controls.Add(Me.chkKk_yn)
        Me.tabMain.Controls.Add(Me.chkLo_yn)
        Me.tabMain.Controls.Add(Me.LblMa_thue_nk)
        Me.tabMain.Controls.Add(Me.txtMa_thue_nk)
        Me.tabMain.Controls.Add(Me.lblTen_thue_nk)
        Me.tabMain.Controls.Add(Me.lblMa_thue)
        Me.tabMain.Controls.Add(Me.txtMa_thue)
        Me.tabMain.Controls.Add(Me.lblTen_thue)
        Me.tabMain.Controls.Add(Me.lblMa_vi_tri)
        Me.tabMain.Controls.Add(Me.txtMa_vi_tri)
        Me.tabMain.Controls.Add(Me.lblTen_vi_tri)
        Me.tabMain.Controls.Add(Me.txtMa_kho)
        Me.tabMain.Controls.Add(Me.chkVt_ton_kho)
        Me.tabMain.Controls.Add(Me.lblDvt)
        Me.tabMain.Controls.Add(Me.txtDvt)
        Me.tabMain.Controls.Add(Me.lblGia_ton)
        Me.tabMain.Controls.Add(Me.txtGia_ton)
        Me.tabMain.Controls.Add(Me.lblNh_vt1)
        Me.tabMain.Controls.Add(Me.txtNh_vt1)
        Me.tabMain.Controls.Add(Me.lblTen_nh_vt1)
        Me.tabMain.Controls.Add(Me.lblLoai_vt)
        Me.tabMain.Controls.Add(Me.txtLoai_vt)
        Me.tabMain.Controls.Add(Me.lblTen_loai_vt)
        Me.tabMain.Controls.Add(Me.lblAbc_code)
        Me.tabMain.Controls.Add(Me.txtAbc_code)
        Me.tabMain.Controls.Add(Me.lblMa_kho)
        Me.tabMain.Controls.Add(Me.lblSl_min)
        Me.tabMain.Controls.Add(Me.lblSl_max)
        Me.tabMain.Controls.Add(Me.lblStatus)
        Me.tabMain.Controls.Add(Me.txtSl_min)
        Me.tabMain.Controls.Add(Me.txtSl_max)
        Me.tabMain.Controls.Add(Me.txtStatus)
        Me.tabMain.Controls.Add(Me.lblStatusMess)
        Me.tabMain.Location = New System.Drawing.Point(4, 22)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.Size = New System.Drawing.Size(667, 477)
        Me.tabMain.TabIndex = 0
        Me.tabMain.Tag = "L100"
        Me.tabMain.Text = "Thong tin chinh"
        '
        'cbbGia_ton
        '
        Me.cbbGia_ton.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbbGia_ton.Location = New System.Drawing.Point(151, 55)
        Me.cbbGia_ton.Name = "cbbGia_ton"
        Me.cbbGia_ton.Size = New System.Drawing.Size(208, 21)
        Me.cbbGia_ton.TabIndex = 4
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(19, 150)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 13)
        Me.Label5.TabIndex = 99
        Me.Label5.Tag = "LA02"
        Me.Label5.Text = "Nhom vat tu"
        '
        'txtNh_vt3
        '
        Me.txtNh_vt3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt3.Location = New System.Drawing.Point(151, 148)
        Me.txtNh_vt3.Name = "txtNh_vt3"
        Me.txtNh_vt3.Size = New System.Drawing.Size(100, 20)
        Me.txtNh_vt3.TabIndex = 8
        Me.txtNh_vt3.Tag = "FCDF"
        Me.txtNh_vt3.Text = "TXTNH_VT3"
        '
        'lblTen_nh_vt3
        '
        Me.lblTen_nh_vt3.AutoSize = True
        Me.lblTen_nh_vt3.Location = New System.Drawing.Point(256, 150)
        Me.lblTen_nh_vt3.Name = "lblTen_nh_vt3"
        Me.lblTen_nh_vt3.Size = New System.Drawing.Size(85, 13)
        Me.lblTen_nh_vt3.TabIndex = 100
        Me.lblTen_nh_vt3.Text = "Ten nhom vat tu"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(19, 127)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 13)
        Me.Label3.TabIndex = 96
        Me.Label3.Tag = "LA01"
        Me.Label3.Text = "Nhom vat tu"
        '
        'txtNh_vt2
        '
        Me.txtNh_vt2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt2.Location = New System.Drawing.Point(151, 125)
        Me.txtNh_vt2.Name = "txtNh_vt2"
        Me.txtNh_vt2.Size = New System.Drawing.Size(100, 20)
        Me.txtNh_vt2.TabIndex = 7
        Me.txtNh_vt2.Tag = "FCDF"
        Me.txtNh_vt2.Text = "TXTNH_VT2"
        '
        'lblTen_nh_vt2
        '
        Me.lblTen_nh_vt2.AutoSize = True
        Me.lblTen_nh_vt2.Location = New System.Drawing.Point(256, 127)
        Me.lblTen_nh_vt2.Name = "lblTen_nh_vt2"
        Me.lblTen_nh_vt2.Size = New System.Drawing.Size(85, 13)
        Me.lblTen_nh_vt2.TabIndex = 97
        Me.lblTen_nh_vt2.Text = "Ten nhom vat tu"
        '
        'lblNh_vt9
        '
        Me.lblNh_vt9.AutoSize = True
        Me.lblNh_vt9.Location = New System.Drawing.Point(19, 173)
        Me.lblNh_vt9.Name = "lblNh_vt9"
        Me.lblNh_vt9.Size = New System.Drawing.Size(52, 13)
        Me.lblNh_vt9.TabIndex = 93
        Me.lblNh_vt9.Tag = "L119"
        Me.lblNh_vt9.Text = "Nhom gia"
        '
        'txtNh_vt9
        '
        Me.txtNh_vt9.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt9.Location = New System.Drawing.Point(151, 171)
        Me.txtNh_vt9.Name = "txtNh_vt9"
        Me.txtNh_vt9.Size = New System.Drawing.Size(100, 20)
        Me.txtNh_vt9.TabIndex = 9
        Me.txtNh_vt9.Tag = "FCDF"
        Me.txtNh_vt9.Text = "TXTNH_VT9"
        '
        'lblTen_nh_vt9
        '
        Me.lblTen_nh_vt9.AutoSize = True
        Me.lblTen_nh_vt9.Location = New System.Drawing.Point(256, 173)
        Me.lblTen_nh_vt9.Name = "lblTen_nh_vt9"
        Me.lblTen_nh_vt9.Size = New System.Drawing.Size(72, 13)
        Me.lblTen_nh_vt9.TabIndex = 94
        Me.lblTen_nh_vt9.Text = "Ten nhom gia"
        '
        'chkNhieu_dvt
        '
        Me.chkNhieu_dvt.Location = New System.Drawing.Point(256, 12)
        Me.chkNhieu_dvt.Name = "chkNhieu_dvt"
        Me.chkNhieu_dvt.Size = New System.Drawing.Size(147, 16)
        Me.chkNhieu_dvt.TabIndex = 1
        Me.chkNhieu_dvt.Tag = "L102FL"
        Me.chkNhieu_dvt.Text = "Nhieu dvt"
        '
        'chkKk_yn
        '
        Me.chkKk_yn.Location = New System.Drawing.Point(256, 30)
        Me.chkKk_yn.Name = "chkKk_yn"
        Me.chkKk_yn.Size = New System.Drawing.Size(147, 25)
        Me.chkKk_yn.TabIndex = 3
        Me.chkKk_yn.Tag = "L105FL"
        Me.chkKk_yn.Text = "Theo doi kiem ke"
        '
        'chkLo_yn
        '
        Me.chkLo_yn.Location = New System.Drawing.Point(151, 36)
        Me.chkLo_yn.Name = "chkLo_yn"
        Me.chkLo_yn.Size = New System.Drawing.Size(100, 15)
        Me.chkLo_yn.TabIndex = 2
        Me.chkLo_yn.Tag = "L104FL"
        Me.chkLo_yn.Text = "Theo doi lo"
        '
        'LblMa_thue_nk
        '
        Me.LblMa_thue_nk.AutoSize = True
        Me.LblMa_thue_nk.Location = New System.Drawing.Point(19, 288)
        Me.LblMa_thue_nk.Name = "LblMa_thue_nk"
        Me.LblMa_thue_nk.Size = New System.Drawing.Size(61, 13)
        Me.LblMa_thue_nk.TabIndex = 81
        Me.LblMa_thue_nk.Tag = "L114"
        Me.LblMa_thue_nk.Text = "Ma thue nk"
        '
        'txtMa_thue_nk
        '
        Me.txtMa_thue_nk.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_thue_nk.Location = New System.Drawing.Point(151, 286)
        Me.txtMa_thue_nk.Name = "txtMa_thue_nk"
        Me.txtMa_thue_nk.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_thue_nk.TabIndex = 14
        Me.txtMa_thue_nk.Tag = "FCDF"
        Me.txtMa_thue_nk.Text = "TXTMA_THUE_NK"
        '
        'lblTen_thue_nk
        '
        Me.lblTen_thue_nk.AutoSize = True
        Me.lblTen_thue_nk.Location = New System.Drawing.Point(256, 288)
        Me.lblTen_thue_nk.Name = "lblTen_thue_nk"
        Me.lblTen_thue_nk.Size = New System.Drawing.Size(104, 13)
        Me.lblTen_thue_nk.TabIndex = 82
        Me.lblTen_thue_nk.Text = "Ten thue nhap khau"
        '
        'lblMa_thue
        '
        Me.lblMa_thue.AutoSize = True
        Me.lblMa_thue.Location = New System.Drawing.Point(19, 265)
        Me.lblMa_thue.Name = "lblMa_thue"
        Me.lblMa_thue.Size = New System.Drawing.Size(46, 13)
        Me.lblMa_thue.TabIndex = 78
        Me.lblMa_thue.Tag = "L113"
        Me.lblMa_thue.Text = "Ma thue"
        '
        'txtMa_thue
        '
        Me.txtMa_thue.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_thue.Location = New System.Drawing.Point(151, 263)
        Me.txtMa_thue.Name = "txtMa_thue"
        Me.txtMa_thue.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_thue.TabIndex = 13
        Me.txtMa_thue.Tag = "FCDF"
        Me.txtMa_thue.Text = "TXTMA_THUE"
        '
        'lblTen_thue
        '
        Me.lblTen_thue.AutoSize = True
        Me.lblTen_thue.Location = New System.Drawing.Point(256, 265)
        Me.lblTen_thue.Name = "lblTen_thue"
        Me.lblTen_thue.Size = New System.Drawing.Size(50, 13)
        Me.lblTen_thue.TabIndex = 79
        Me.lblTen_thue.Text = "Ten thue"
        '
        'lblMa_vi_tri
        '
        Me.lblMa_vi_tri.AutoSize = True
        Me.lblMa_vi_tri.Location = New System.Drawing.Point(19, 242)
        Me.lblMa_vi_tri.Name = "lblMa_vi_tri"
        Me.lblMa_vi_tri.Size = New System.Drawing.Size(44, 13)
        Me.lblMa_vi_tri.TabIndex = 75
        Me.lblMa_vi_tri.Tag = "L112"
        Me.lblMa_vi_tri.Text = "Ma vi tri"
        '
        'txtMa_vi_tri
        '
        Me.txtMa_vi_tri.BackColor = System.Drawing.Color.White
        Me.txtMa_vi_tri.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_vi_tri.Location = New System.Drawing.Point(151, 240)
        Me.txtMa_vi_tri.Name = "txtMa_vi_tri"
        Me.txtMa_vi_tri.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_vi_tri.TabIndex = 12
        Me.txtMa_vi_tri.Tag = "FC"
        Me.txtMa_vi_tri.Text = "TXTMA_VI_TRI"
        '
        'lblTen_vi_tri
        '
        Me.lblTen_vi_tri.AutoSize = True
        Me.lblTen_vi_tri.Location = New System.Drawing.Point(256, 242)
        Me.lblTen_vi_tri.Name = "lblTen_vi_tri"
        Me.lblTen_vi_tri.Size = New System.Drawing.Size(48, 13)
        Me.lblTen_vi_tri.TabIndex = 76
        Me.lblTen_vi_tri.Text = "Ten vi tri"
        '
        'txtMa_kho
        '
        Me.txtMa_kho.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kho.Location = New System.Drawing.Point(151, 217)
        Me.txtMa_kho.Name = "txtMa_kho"
        Me.txtMa_kho.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_kho.TabIndex = 11
        Me.txtMa_kho.Tag = "FCDF"
        Me.txtMa_kho.Text = "TXTMA_KHO"
        '
        'chkVt_ton_kho
        '
        Me.chkVt_ton_kho.Location = New System.Drawing.Point(19, 36)
        Me.chkVt_ton_kho.Name = "chkVt_ton_kho"
        Me.chkVt_ton_kho.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkVt_ton_kho.Size = New System.Drawing.Size(109, 15)
        Me.chkVt_ton_kho.TabIndex = 1
        Me.chkVt_ton_kho.Tag = "L103FL"
        Me.chkVt_ton_kho.Text = "Theo doi ton kho"
        '
        'tabAccount
        '
        Me.tabAccount.Controls.Add(Me.Label12)
        Me.tabAccount.Controls.Add(Me.txtTk_cpbh)
        Me.tabAccount.Controls.Add(Me.lblTen_tk_cpbh)
        Me.tabAccount.Controls.Add(Me.Label2)
        Me.tabAccount.Controls.Add(Me.txtTk_ck)
        Me.tabAccount.Controls.Add(Me.lblTen_tk_ck)
        Me.tabAccount.Controls.Add(Me.lblTk_cl_vt)
        Me.tabAccount.Controls.Add(Me.txtTk_cl_vt)
        Me.tabAccount.Controls.Add(Me.lblTen_tk_cl_vt)
        Me.tabAccount.Controls.Add(Me.lblTen_tk_dl)
        Me.tabAccount.Controls.Add(Me.lblTk_dl)
        Me.tabAccount.Controls.Add(Me.txtTk_dl)
        Me.tabAccount.Controls.Add(Me.lblTk_dtnb)
        Me.tabAccount.Controls.Add(Me.txtTk_dtnb)
        Me.tabAccount.Controls.Add(Me.lblTen_tk_dtnb)
        Me.tabAccount.Controls.Add(Me.lblTk_vt)
        Me.tabAccount.Controls.Add(Me.txtTk_vt)
        Me.tabAccount.Controls.Add(Me.lblTen_tk_vt)
        Me.tabAccount.Controls.Add(Me.lblTk_gv)
        Me.tabAccount.Controls.Add(Me.txtTk_gv)
        Me.tabAccount.Controls.Add(Me.lblTen_tk_gv)
        Me.tabAccount.Controls.Add(Me.lblTk_dt)
        Me.tabAccount.Controls.Add(Me.txtTk_dt)
        Me.tabAccount.Controls.Add(Me.lblTen_tk_dt)
        Me.tabAccount.Controls.Add(Me.lblTen_tk_tl)
        Me.tabAccount.Controls.Add(Me.lblTk_tl)
        Me.tabAccount.Controls.Add(Me.txtTk_tl)
        Me.tabAccount.Controls.Add(Me.lblTk_spdd)
        Me.tabAccount.Controls.Add(Me.txtTk_spdd)
        Me.tabAccount.Controls.Add(Me.lblTen_tk_spdd)
        Me.tabAccount.Controls.Add(Me.chkSua_tk_vt)
        Me.tabAccount.Location = New System.Drawing.Point(4, 22)
        Me.tabAccount.Name = "tabAccount"
        Me.tabAccount.Size = New System.Drawing.Size(667, 477)
        Me.tabAccount.TabIndex = 1
        Me.tabAccount.Tag = "L200"
        Me.tabAccount.Text = "Tai khoan"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(19, 242)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(102, 13)
        Me.Label12.TabIndex = 118
        Me.Label12.Tag = "L211"
        Me.Label12.Text = "Tk chi phi ban hang"
        '
        'txtTk_cpbh
        '
        Me.txtTk_cpbh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_cpbh.Location = New System.Drawing.Point(151, 240)
        Me.txtTk_cpbh.Name = "txtTk_cpbh"
        Me.txtTk_cpbh.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_cpbh.TabIndex = 10
        Me.txtTk_cpbh.Tag = "FCDF"
        Me.txtTk_cpbh.Text = "TXTTK_CPBH"
        '
        'lblTen_tk_cpbh
        '
        Me.lblTen_tk_cpbh.AutoSize = True
        Me.lblTen_tk_cpbh.Location = New System.Drawing.Point(256, 242)
        Me.lblTen_tk_cpbh.Name = "lblTen_tk_cpbh"
        Me.lblTen_tk_cpbh.Size = New System.Drawing.Size(120, 13)
        Me.lblTen_tk_cpbh.TabIndex = 119
        Me.lblTen_tk_cpbh.Text = "Ten tk chi phi ban hang"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(19, 219)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 13)
        Me.Label2.TabIndex = 112
        Me.Label2.Tag = "L210"
        Me.Label2.Text = "Tk chiet khau"
        '
        'txtTk_ck
        '
        Me.txtTk_ck.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_ck.Location = New System.Drawing.Point(151, 217)
        Me.txtTk_ck.Name = "txtTk_ck"
        Me.txtTk_ck.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_ck.TabIndex = 9
        Me.txtTk_ck.Tag = "FCDF"
        Me.txtTk_ck.Text = "TXTTK_CK"
        '
        'lblTen_tk_ck
        '
        Me.lblTen_tk_ck.AutoSize = True
        Me.lblTen_tk_ck.Location = New System.Drawing.Point(256, 219)
        Me.lblTen_tk_ck.Name = "lblTen_tk_ck"
        Me.lblTen_tk_ck.Size = New System.Drawing.Size(91, 13)
        Me.lblTen_tk_ck.TabIndex = 113
        Me.lblTen_tk_ck.Text = "Ten tk chiet khau"
        '
        'lblTk_cl_vt
        '
        Me.lblTk_cl_vt.AutoSize = True
        Me.lblTk_cl_vt.Location = New System.Drawing.Point(19, 196)
        Me.lblTk_cl_vt.Name = "lblTk_cl_vt"
        Me.lblTk_cl_vt.Size = New System.Drawing.Size(114, 13)
        Me.lblTk_cl_vt.TabIndex = 109
        Me.lblTk_cl_vt.Tag = "L209"
        Me.lblTk_cl_vt.Text = "Tk chenh lech gia von"
        '
        'txtTk_cl_vt
        '
        Me.txtTk_cl_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_cl_vt.Location = New System.Drawing.Point(151, 194)
        Me.txtTk_cl_vt.Name = "txtTk_cl_vt"
        Me.txtTk_cl_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_cl_vt.TabIndex = 8
        Me.txtTk_cl_vt.Tag = "FCDF"
        Me.txtTk_cl_vt.Text = "TXTTK_CL_VT"
        '
        'lblTen_tk_cl_vt
        '
        Me.lblTen_tk_cl_vt.AutoSize = True
        Me.lblTen_tk_cl_vt.Location = New System.Drawing.Point(256, 196)
        Me.lblTen_tk_cl_vt.Name = "lblTen_tk_cl_vt"
        Me.lblTen_tk_cl_vt.Size = New System.Drawing.Size(132, 13)
        Me.lblTen_tk_cl_vt.TabIndex = 110
        Me.lblTen_tk_cl_vt.Text = "Ten tk chenh lech gia von"
        '
        'lblTen_tk_dl
        '
        Me.lblTen_tk_dl.AutoSize = True
        Me.lblTen_tk_dl.Location = New System.Drawing.Point(256, 150)
        Me.lblTen_tk_dl.Name = "lblTen_tk_dl"
        Me.lblTen_tk_dl.Size = New System.Drawing.Size(65, 13)
        Me.lblTen_tk_dl.TabIndex = 107
        Me.lblTen_tk_dl.Text = "Ten tk dai ly"
        '
        'lblTk_dl
        '
        Me.lblTk_dl.AutoSize = True
        Me.lblTk_dl.Location = New System.Drawing.Point(19, 150)
        Me.lblTk_dl.Name = "lblTk_dl"
        Me.lblTk_dl.Size = New System.Drawing.Size(47, 13)
        Me.lblTk_dl.TabIndex = 106
        Me.lblTk_dl.Tag = "L207"
        Me.lblTk_dl.Text = "Tk dai ly"
        '
        'txtTk_dl
        '
        Me.txtTk_dl.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_dl.Location = New System.Drawing.Point(151, 148)
        Me.txtTk_dl.Name = "txtTk_dl"
        Me.txtTk_dl.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_dl.TabIndex = 6
        Me.txtTk_dl.Tag = "FCDF"
        Me.txtTk_dl.Text = "TXTTK_DL"
        '
        'lblTk_dtnb
        '
        Me.lblTk_dtnb.AutoSize = True
        Me.lblTk_dtnb.Location = New System.Drawing.Point(19, 104)
        Me.lblTk_dtnb.Name = "lblTk_dtnb"
        Me.lblTk_dtnb.Size = New System.Drawing.Size(103, 13)
        Me.lblTk_dtnb.TabIndex = 103
        Me.lblTk_dtnb.Tag = "L205"
        Me.lblTk_dtnb.Text = "Tk doanh thu noi bo"
        '
        'txtTk_dtnb
        '
        Me.txtTk_dtnb.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_dtnb.Location = New System.Drawing.Point(151, 102)
        Me.txtTk_dtnb.Name = "txtTk_dtnb"
        Me.txtTk_dtnb.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_dtnb.TabIndex = 4
        Me.txtTk_dtnb.Tag = "FCDF"
        Me.txtTk_dtnb.Text = "TXTTK_DTNB"
        '
        'lblTen_tk_dtnb
        '
        Me.lblTen_tk_dtnb.AutoSize = True
        Me.lblTen_tk_dtnb.Location = New System.Drawing.Point(256, 104)
        Me.lblTen_tk_dtnb.Name = "lblTen_tk_dtnb"
        Me.lblTen_tk_dtnb.Size = New System.Drawing.Size(121, 13)
        Me.lblTen_tk_dtnb.TabIndex = 104
        Me.lblTen_tk_dtnb.Text = "Ten tk doanh thu noi bo"
        '
        'chkSua_tk_vt
        '
        Me.chkSua_tk_vt.Location = New System.Drawing.Point(18, 33)
        Me.chkSua_tk_vt.Name = "chkSua_tk_vt"
        Me.chkSua_tk_vt.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkSua_tk_vt.Size = New System.Drawing.Size(147, 20)
        Me.chkSua_tk_vt.TabIndex = 1
        Me.chkSua_tk_vt.Tag = "L202FL"
        Me.chkSua_tk_vt.Text = "Sua tk vat tu"
        Me.chkSua_tk_vt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tabPur
        '
        Me.tabPur.Controls.Add(Me.lblTen_kh)
        Me.tabPur.Controls.Add(Me.lblMa_kh)
        Me.tabPur.Controls.Add(Me.txtMa_kh)
        Me.tabPur.Controls.Add(Me.lblTen_kh2)
        Me.tabPur.Controls.Add(Me.lblMa_kh2)
        Me.tabPur.Controls.Add(Me.txtMa_kh2)
        Me.tabPur.Controls.Add(Me.txtOng_ba)
        Me.tabPur.Controls.Add(Me.lblOng_ba)
        Me.tabPur.Controls.Add(Me.lblTen_kh0)
        Me.tabPur.Controls.Add(Me.lblMa_kh0)
        Me.tabPur.Controls.Add(Me.txtMa_kh0)
        Me.tabPur.Controls.Add(Me.lblTg_th)
        Me.tabPur.Controls.Add(Me.txtTg_th)
        Me.tabPur.Controls.Add(Me.txtDvttg_th)
        Me.tabPur.Controls.Add(Me.lblSo_lo_chuan)
        Me.tabPur.Controls.Add(Me.txtSl_lo_chuan)
        Me.tabPur.Controls.Add(Me.txtDvtsl_lo_chuan)
        Me.tabPur.Location = New System.Drawing.Point(4, 22)
        Me.tabPur.Name = "tabPur"
        Me.tabPur.Size = New System.Drawing.Size(667, 477)
        Me.tabPur.TabIndex = 3
        Me.tabPur.Tag = "L400"
        Me.tabPur.Text = "Mua hang"
        '
        'lblTen_kh
        '
        Me.lblTen_kh.AutoSize = True
        Me.lblTen_kh.Location = New System.Drawing.Point(256, 81)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(126, 13)
        Me.lblTen_kh.TabIndex = 109
        Me.lblTen_kh.Text = "Ten khach hang lan cuoi"
        '
        'lblMa_kh
        '
        Me.lblMa_kh.AutoSize = True
        Me.lblMa_kh.Location = New System.Drawing.Point(19, 81)
        Me.lblMa_kh.Name = "lblMa_kh"
        Me.lblMa_kh.Size = New System.Drawing.Size(115, 13)
        Me.lblMa_kh.TabIndex = 108
        Me.lblMa_kh.Tag = "L404"
        Me.lblMa_kh.Text = "Nha cung cap lan cuoi"
        '
        'txtMa_kh
        '
        Me.txtMa_kh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kh.Location = New System.Drawing.Point(151, 79)
        Me.txtMa_kh.Name = "txtMa_kh"
        Me.txtMa_kh.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_kh.TabIndex = 3
        Me.txtMa_kh.Tag = "FC"
        Me.txtMa_kh.Text = "TXTMA_KH"
        '
        'lblTen_kh2
        '
        Me.lblTen_kh2.AutoSize = True
        Me.lblTen_kh2.Location = New System.Drawing.Point(256, 58)
        Me.lblTen_kh2.Name = "lblTen_kh2"
        Me.lblTen_kh2.Size = New System.Drawing.Size(107, 13)
        Me.lblTen_kh2.TabIndex = 106
        Me.lblTen_kh2.Text = "Ten khach hang phu"
        '
        'lblMa_kh2
        '
        Me.lblMa_kh2.AutoSize = True
        Me.lblMa_kh2.Location = New System.Drawing.Point(19, 58)
        Me.lblMa_kh2.Name = "lblMa_kh2"
        Me.lblMa_kh2.Size = New System.Drawing.Size(96, 13)
        Me.lblMa_kh2.TabIndex = 105
        Me.lblMa_kh2.Tag = "L403"
        Me.lblMa_kh2.Text = "Nha cung cap phu"
        '
        'txtMa_kh2
        '
        Me.txtMa_kh2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kh2.Location = New System.Drawing.Point(151, 56)
        Me.txtMa_kh2.Name = "txtMa_kh2"
        Me.txtMa_kh2.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_kh2.TabIndex = 2
        Me.txtMa_kh2.Tag = "FC"
        Me.txtMa_kh2.Text = "TXTMA_KH2"
        '
        'txtOng_ba
        '
        Me.txtOng_ba.Location = New System.Drawing.Point(151, 10)
        Me.txtOng_ba.Name = "txtOng_ba"
        Me.txtOng_ba.Size = New System.Drawing.Size(100, 20)
        Me.txtOng_ba.TabIndex = 0
        Me.txtOng_ba.Tag = "FC"
        Me.txtOng_ba.Text = "txtOng_ba"
        '
        'lblOng_ba
        '
        Me.lblOng_ba.AutoSize = True
        Me.lblOng_ba.Location = New System.Drawing.Point(19, 12)
        Me.lblOng_ba.Name = "lblOng_ba"
        Me.lblOng_ba.Size = New System.Drawing.Size(85, 13)
        Me.lblOng_ba.TabIndex = 102
        Me.lblOng_ba.Tag = "L401"
        Me.lblOng_ba.Text = "Nguoi mua hang"
        '
        'lblTen_kh0
        '
        Me.lblTen_kh0.AutoSize = True
        Me.lblTen_kh0.Location = New System.Drawing.Point(256, 35)
        Me.lblTen_kh0.Name = "lblTen_kh0"
        Me.lblTen_kh0.Size = New System.Drawing.Size(115, 13)
        Me.lblTen_kh0.TabIndex = 103
        Me.lblTen_kh0.Text = "Ten khach hang chinh"
        '
        'lblMa_kh0
        '
        Me.lblMa_kh0.AutoSize = True
        Me.lblMa_kh0.Location = New System.Drawing.Point(19, 35)
        Me.lblMa_kh0.Name = "lblMa_kh0"
        Me.lblMa_kh0.Size = New System.Drawing.Size(104, 13)
        Me.lblMa_kh0.TabIndex = 102
        Me.lblMa_kh0.Tag = "L402"
        Me.lblMa_kh0.Text = "Nha cung cap chinh"
        '
        'txtMa_kh0
        '
        Me.txtMa_kh0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kh0.Location = New System.Drawing.Point(151, 33)
        Me.txtMa_kh0.Name = "txtMa_kh0"
        Me.txtMa_kh0.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_kh0.TabIndex = 1
        Me.txtMa_kh0.Tag = "FC"
        Me.txtMa_kh0.Text = "TXTMA_KH0"
        '
        'lblTg_th
        '
        Me.lblTg_th.AutoSize = True
        Me.lblTg_th.Location = New System.Drawing.Point(19, 104)
        Me.lblTg_th.Name = "lblTg_th"
        Me.lblTg_th.Size = New System.Drawing.Size(98, 13)
        Me.lblTg_th.TabIndex = 109
        Me.lblTg_th.Tag = "L602"
        Me.lblTg_th.Text = "Thoi gian thuc hien"
        '
        'txtTg_th
        '
        Me.txtTg_th.Format = "### ### ##0"
        Me.txtTg_th.Location = New System.Drawing.Point(151, 102)
        Me.txtTg_th.MaxLength = 3
        Me.txtTg_th.Name = "txtTg_th"
        Me.txtTg_th.Size = New System.Drawing.Size(67, 20)
        Me.txtTg_th.TabIndex = 4
        Me.txtTg_th.Tag = "FN"
        Me.txtTg_th.Text = "0"
        Me.txtTg_th.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTg_th.Value = 0R
        '
        'txtDvttg_th
        '
        Me.txtDvttg_th.Location = New System.Drawing.Point(221, 102)
        Me.txtDvttg_th.Name = "txtDvttg_th"
        Me.txtDvttg_th.Size = New System.Drawing.Size(30, 20)
        Me.txtDvttg_th.TabIndex = 5
        Me.txtDvttg_th.Tag = "FC"
        Me.txtDvttg_th.Text = "txtDvttg_th"
        '
        'lblSo_lo_chuan
        '
        Me.lblSo_lo_chuan.AutoSize = True
        Me.lblSo_lo_chuan.Location = New System.Drawing.Point(19, 127)
        Me.lblSo_lo_chuan.Name = "lblSo_lo_chuan"
        Me.lblSo_lo_chuan.Size = New System.Drawing.Size(93, 13)
        Me.lblSo_lo_chuan.TabIndex = 112
        Me.lblSo_lo_chuan.Tag = "L603"
        Me.lblSo_lo_chuan.Text = "So luong lo chuan"
        '
        'txtSl_lo_chuan
        '
        Me.txtSl_lo_chuan.Format = "m_ip_sl"
        Me.txtSl_lo_chuan.Location = New System.Drawing.Point(151, 125)
        Me.txtSl_lo_chuan.MaxLength = 8
        Me.txtSl_lo_chuan.Name = "txtSl_lo_chuan"
        Me.txtSl_lo_chuan.Size = New System.Drawing.Size(67, 20)
        Me.txtSl_lo_chuan.TabIndex = 6
        Me.txtSl_lo_chuan.Tag = "FN"
        Me.txtSl_lo_chuan.Text = "m_ip_sl"
        Me.txtSl_lo_chuan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSl_lo_chuan.Value = 0R
        '
        'txtDvtsl_lo_chuan
        '
        Me.txtDvtsl_lo_chuan.Location = New System.Drawing.Point(221, 125)
        Me.txtDvtsl_lo_chuan.Name = "txtDvtsl_lo_chuan"
        Me.txtDvtsl_lo_chuan.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtsl_lo_chuan.TabIndex = 7
        Me.txtDvtsl_lo_chuan.Tag = "FC"
        Me.txtDvtsl_lo_chuan.Text = "txtDvtsl_lo_chuan"
        '
        'tabLot
        '
        Me.tabLot.Controls.Add(Me.chkTao_lo)
        Me.tabLot.Controls.Add(Me.txtSo_ngay_bh)
        Me.tabLot.Controls.Add(Me.lblSo_ngay_bh)
        Me.tabLot.Controls.Add(Me.lblCach_xuatMess)
        Me.tabLot.Controls.Add(Me.txtCach_xuat)
        Me.tabLot.Controls.Add(Me.lblCach_xuat)
        Me.tabLot.Controls.Add(Me.lblKieu_loMess)
        Me.tabLot.Controls.Add(Me.txtKieu_lo)
        Me.tabLot.Controls.Add(Me.lblKieu_lo)
        Me.tabLot.Controls.Add(Me.txtSo_ngay_sp)
        Me.tabLot.Controls.Add(Me.lblSo_ngay_sp)
        Me.tabLot.Location = New System.Drawing.Point(4, 22)
        Me.tabLot.Name = "tabLot"
        Me.tabLot.Size = New System.Drawing.Size(667, 477)
        Me.tabLot.TabIndex = 2
        Me.tabLot.Tag = "L300"
        Me.tabLot.Text = "Lo"
        '
        'chkTao_lo
        '
        Me.chkTao_lo.Location = New System.Drawing.Point(152, 102)
        Me.chkTao_lo.Name = "chkTao_lo"
        Me.chkTao_lo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTao_lo.Size = New System.Drawing.Size(264, 20)
        Me.chkTao_lo.TabIndex = 4
        Me.chkTao_lo.Tag = "L307FL"
        Me.chkTao_lo.Text = "Cho phep tao lo ngay khi nhap"
        '
        'txtSo_ngay_bh
        '
        Me.txtSo_ngay_bh.Format = "##0"
        Me.txtSo_ngay_bh.Location = New System.Drawing.Point(151, 79)
        Me.txtSo_ngay_bh.MaxLength = 4
        Me.txtSo_ngay_bh.Name = "txtSo_ngay_bh"
        Me.txtSo_ngay_bh.Size = New System.Drawing.Size(100, 20)
        Me.txtSo_ngay_bh.TabIndex = 3
        Me.txtSo_ngay_bh.Tag = "FN"
        Me.txtSo_ngay_bh.Text = "0"
        Me.txtSo_ngay_bh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSo_ngay_bh.Value = 0R
        '
        'lblSo_ngay_bh
        '
        Me.lblSo_ngay_bh.AutoSize = True
        Me.lblSo_ngay_bh.Location = New System.Drawing.Point(19, 81)
        Me.lblSo_ngay_bh.Name = "lblSo_ngay_bh"
        Me.lblSo_ngay_bh.Size = New System.Drawing.Size(116, 13)
        Me.lblSo_ngay_bh.TabIndex = 108
        Me.lblSo_ngay_bh.Tag = "L306"
        Me.lblSo_ngay_bh.Text = "TG bao hanh (so ngay)"
        '
        'lblCach_xuatMess
        '
        Me.lblCach_xuatMess.AutoSize = True
        Me.lblCach_xuatMess.Location = New System.Drawing.Point(184, 35)
        Me.lblCach_xuatMess.Name = "lblCach_xuatMess"
        Me.lblCach_xuatMess.Size = New System.Drawing.Size(232, 13)
        Me.lblCach_xuatMess.TabIndex = 106
        Me.lblCach_xuatMess.Tag = "L304"
        Me.lblCach_xuatMess.Text = "1 - Theo HSD, 2 - NTXT, 3 - Lien tuc, 4 - NSXT"
        '
        'txtCach_xuat
        '
        Me.txtCach_xuat.Location = New System.Drawing.Point(151, 33)
        Me.txtCach_xuat.MaxLength = 1
        Me.txtCach_xuat.Name = "txtCach_xuat"
        Me.txtCach_xuat.Size = New System.Drawing.Size(25, 20)
        Me.txtCach_xuat.TabIndex = 1
        Me.txtCach_xuat.Tag = "FN"
        Me.txtCach_xuat.Text = "txtCach_xuat"
        Me.txtCach_xuat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblCach_xuat
        '
        Me.lblCach_xuat.AutoSize = True
        Me.lblCach_xuat.Location = New System.Drawing.Point(19, 35)
        Me.lblCach_xuat.Name = "lblCach_xuat"
        Me.lblCach_xuat.Size = New System.Drawing.Size(55, 13)
        Me.lblCach_xuat.TabIndex = 105
        Me.lblCach_xuat.Tag = "L303"
        Me.lblCach_xuat.Text = "Cach xuat"
        '
        'lblKieu_loMess
        '
        Me.lblKieu_loMess.AutoSize = True
        Me.lblKieu_loMess.Location = New System.Drawing.Point(184, 12)
        Me.lblKieu_loMess.Name = "lblKieu_loMess"
        Me.lblKieu_loMess.Size = New System.Drawing.Size(207, 13)
        Me.lblKieu_loMess.TabIndex = 103
        Me.lblKieu_loMess.Tag = "L302"
        Me.lblKieu_loMess.Text = "1 - Tinh theo ngay nhap, 2 - Ngay su dung"
        '
        'txtKieu_lo
        '
        Me.txtKieu_lo.Location = New System.Drawing.Point(151, 10)
        Me.txtKieu_lo.MaxLength = 1
        Me.txtKieu_lo.Name = "txtKieu_lo"
        Me.txtKieu_lo.Size = New System.Drawing.Size(25, 20)
        Me.txtKieu_lo.TabIndex = 0
        Me.txtKieu_lo.Tag = "FN"
        Me.txtKieu_lo.Text = "txtKieu_lo"
        Me.txtKieu_lo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblKieu_lo
        '
        Me.lblKieu_lo.AutoSize = True
        Me.lblKieu_lo.Location = New System.Drawing.Point(19, 12)
        Me.lblKieu_lo.Name = "lblKieu_lo"
        Me.lblKieu_lo.Size = New System.Drawing.Size(39, 13)
        Me.lblKieu_lo.TabIndex = 102
        Me.lblKieu_lo.Tag = "L301"
        Me.lblKieu_lo.Text = "Kieu lo"
        '
        'txtSo_ngay_sp
        '
        Me.txtSo_ngay_sp.Format = "##0"
        Me.txtSo_ngay_sp.Location = New System.Drawing.Point(151, 56)
        Me.txtSo_ngay_sp.MaxLength = 4
        Me.txtSo_ngay_sp.Name = "txtSo_ngay_sp"
        Me.txtSo_ngay_sp.Size = New System.Drawing.Size(100, 20)
        Me.txtSo_ngay_sp.TabIndex = 2
        Me.txtSo_ngay_sp.Tag = "FN"
        Me.txtSo_ngay_sp.Text = "0"
        Me.txtSo_ngay_sp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSo_ngay_sp.Value = 0R
        '
        'lblSo_ngay_sp
        '
        Me.lblSo_ngay_sp.AutoSize = True
        Me.lblSo_ngay_sp.Location = New System.Drawing.Point(19, 58)
        Me.lblSo_ngay_sp.Name = "lblSo_ngay_sp"
        Me.lblSo_ngay_sp.Size = New System.Drawing.Size(109, 13)
        Me.lblSo_ngay_sp.TabIndex = 102
        Me.lblSo_ngay_sp.Tag = "L305"
        Me.lblSo_ngay_sp.Text = "Vong doi sp (so ngay)"
        '
        'tabMRP
        '
        Me.tabMRP.Controls.Add(Me.Label20)
        Me.tabMRP.Controls.Add(Me.txtS5)
        Me.tabMRP.Controls.Add(Me.Label19)
        Me.tabMRP.Controls.Add(Me.txtS4)
        Me.tabMRP.Controls.Add(Me.Label13)
        Me.tabMRP.Controls.Add(Me.txtMa_lo_trinh)
        Me.tabMRP.Controls.Add(Me.lblTen_lo_trinh)
        Me.tabMRP.Controls.Add(Me.Label11)
        Me.tabMRP.Controls.Add(Me.txtTg_th2)
        Me.tabMRP.Controls.Add(Me.Label10)
        Me.tabMRP.Controls.Add(Me.txtSo_ngay_dh)
        Me.tabMRP.Controls.Add(Me.Label9)
        Me.tabMRP.Controls.Add(Me.txtVung_hd)
        Me.tabMRP.Controls.Add(Me.Label8)
        Me.tabMRP.Controls.Add(Me.txtCo_lo)
        Me.tabMRP.Controls.Add(Me.Label7)
        Me.tabMRP.Controls.Add(Me.txtTon_at)
        Me.tabMRP.Controls.Add(Me.Label4)
        Me.tabMRP.Controls.Add(Me.txtKieu_hd)
        Me.tabMRP.Controls.Add(Me.Label6)
        Me.tabMRP.Location = New System.Drawing.Point(4, 22)
        Me.tabMRP.Name = "tabMRP"
        Me.tabMRP.Size = New System.Drawing.Size(667, 477)
        Me.tabMRP.TabIndex = 7
        Me.tabMRP.Tag = "LA00"
        Me.tabMRP.Text = "Hoach dinh"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(19, 198)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(71, 13)
        Me.Label20.TabIndex = 124
        Me.Label20.Tag = ""
        Me.Label20.Text = "Giá bán buôn"
        '
        'txtS5
        '
        Me.txtS5.Format = "m_ip_gia"
        Me.txtS5.Location = New System.Drawing.Point(151, 197)
        Me.txtS5.MaxLength = 9
        Me.txtS5.Name = "txtS5"
        Me.txtS5.Size = New System.Drawing.Size(100, 20)
        Me.txtS5.TabIndex = 8
        Me.txtS5.Tag = "FN"
        Me.txtS5.Text = "m_ip_gia"
        Me.txtS5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtS5.Value = 0R
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(19, 174)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(61, 13)
        Me.Label19.TabIndex = 122
        Me.Label19.Tag = ""
        Me.Label19.Text = "Giá kê khai"
        '
        'txtS4
        '
        Me.txtS4.Format = "m_ip_gia"
        Me.txtS4.Location = New System.Drawing.Point(151, 172)
        Me.txtS4.MaxLength = 9
        Me.txtS4.Name = "txtS4"
        Me.txtS4.Size = New System.Drawing.Size(100, 20)
        Me.txtS4.TabIndex = 7
        Me.txtS4.Tag = "FN"
        Me.txtS4.Text = "m_ip_gia"
        Me.txtS4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtS4.Value = 0R
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(19, 150)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(56, 13)
        Me.Label13.TabIndex = 119
        Me.Label13.Tag = "LA09"
        Me.Label13.Text = "Ma lo trinh"
        '
        'txtMa_lo_trinh
        '
        Me.txtMa_lo_trinh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_lo_trinh.Location = New System.Drawing.Point(151, 148)
        Me.txtMa_lo_trinh.Name = "txtMa_lo_trinh"
        Me.txtMa_lo_trinh.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_lo_trinh.TabIndex = 6
        Me.txtMa_lo_trinh.Tag = "FCDF"
        Me.txtMa_lo_trinh.Text = "TXTMA_LO_TRINH"
        '
        'lblTen_lo_trinh
        '
        Me.lblTen_lo_trinh.AutoSize = True
        Me.lblTen_lo_trinh.Location = New System.Drawing.Point(254, 150)
        Me.lblTen_lo_trinh.Name = "lblTen_lo_trinh"
        Me.lblTen_lo_trinh.Size = New System.Drawing.Size(60, 13)
        Me.lblTen_lo_trinh.TabIndex = 120
        Me.lblTen_lo_trinh.Text = "Ten lo trinh"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(19, 104)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(98, 13)
        Me.Label11.TabIndex = 117
        Me.Label11.Tag = "L602"
        Me.Label11.Text = "Thoi gian thuc hien"
        '
        'txtTg_th2
        '
        Me.txtTg_th2.Format = "##0"
        Me.txtTg_th2.Location = New System.Drawing.Point(151, 102)
        Me.txtTg_th2.MaxLength = 3
        Me.txtTg_th2.Name = "txtTg_th2"
        Me.txtTg_th2.Size = New System.Drawing.Size(100, 20)
        Me.txtTg_th2.TabIndex = 4
        Me.txtTg_th2.Tag = ""
        Me.txtTg_th2.Text = "0"
        Me.txtTg_th2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTg_th2.Value = 0R
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(19, 81)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(104, 13)
        Me.Label10.TabIndex = 114
        Me.Label10.Tag = "LA07"
        Me.Label10.Text = "So ngay dat lai hang"
        '
        'txtSo_ngay_dh
        '
        Me.txtSo_ngay_dh.Format = "##0"
        Me.txtSo_ngay_dh.Location = New System.Drawing.Point(151, 79)
        Me.txtSo_ngay_dh.MaxLength = 3
        Me.txtSo_ngay_dh.Name = "txtSo_ngay_dh"
        Me.txtSo_ngay_dh.Size = New System.Drawing.Size(100, 20)
        Me.txtSo_ngay_dh.TabIndex = 3
        Me.txtSo_ngay_dh.Tag = "FN"
        Me.txtSo_ngay_dh.Text = "0"
        Me.txtSo_ngay_dh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSo_ngay_dh.Value = 0R
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(19, 127)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(88, 13)
        Me.Label9.TabIndex = 112
        Me.Label9.Tag = "LA08"
        Me.Label9.Text = "Vung hoach dinh"
        '
        'txtVung_hd
        '
        Me.txtVung_hd.Format = "##0"
        Me.txtVung_hd.Location = New System.Drawing.Point(151, 125)
        Me.txtVung_hd.MaxLength = 3
        Me.txtVung_hd.Name = "txtVung_hd"
        Me.txtVung_hd.Size = New System.Drawing.Size(100, 20)
        Me.txtVung_hd.TabIndex = 5
        Me.txtVung_hd.Tag = "FN"
        Me.txtVung_hd.Text = "0"
        Me.txtVung_hd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtVung_hd.Value = 0R
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(19, 58)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(31, 13)
        Me.Label8.TabIndex = 110
        Me.Label8.Tag = "LA06"
        Me.Label8.Text = "Co lo"
        '
        'txtCo_lo
        '
        Me.txtCo_lo.Format = "m_ip_sl"
        Me.txtCo_lo.Location = New System.Drawing.Point(151, 56)
        Me.txtCo_lo.MaxLength = 8
        Me.txtCo_lo.Name = "txtCo_lo"
        Me.txtCo_lo.Size = New System.Drawing.Size(100, 20)
        Me.txtCo_lo.TabIndex = 2
        Me.txtCo_lo.Tag = "FN"
        Me.txtCo_lo.Text = "m_ip_sl"
        Me.txtCo_lo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtCo_lo.Value = 0R
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(19, 35)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(86, 13)
        Me.Label7.TabIndex = 108
        Me.Label7.Tag = "LA05"
        Me.Label7.Text = "Ton kho an toan"
        '
        'txtTon_at
        '
        Me.txtTon_at.Format = "m_ip_sl"
        Me.txtTon_at.Location = New System.Drawing.Point(151, 33)
        Me.txtTon_at.MaxLength = 8
        Me.txtTon_at.Name = "txtTon_at"
        Me.txtTon_at.Size = New System.Drawing.Size(100, 20)
        Me.txtTon_at.TabIndex = 1
        Me.txtTon_at.Tag = "FN"
        Me.txtTon_at.Text = "m_ip_sl"
        Me.txtTon_at.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTon_at.Value = 0R
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(184, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(325, 13)
        Me.Label4.TabIndex = 106
        Me.Label4.Tag = "LA04"
        Me.Label4.Text = "0 - Khong hoach dinh, 1 - Roi rac, 2 - Co lo, 3 - Co dinh ky dat hang"
        '
        'txtKieu_hd
        '
        Me.txtKieu_hd.Location = New System.Drawing.Point(151, 10)
        Me.txtKieu_hd.MaxLength = 1
        Me.txtKieu_hd.Name = "txtKieu_hd"
        Me.txtKieu_hd.Size = New System.Drawing.Size(25, 20)
        Me.txtKieu_hd.TabIndex = 0
        Me.txtKieu_hd.Tag = "FN"
        Me.txtKieu_hd.Text = "txtKieu_hd"
        Me.txtKieu_hd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(19, 12)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 13)
        Me.Label6.TabIndex = 105
        Me.Label6.Tag = "LA03"
        Me.Label6.Text = "Kieu hoach dinh"
        '
        'tabStyle
        '
        Me.tabStyle.Controls.Add(Me.Label16)
        Me.tabStyle.Controls.Add(Me.Label17)
        Me.tabStyle.Controls.Add(Me.TXTS2)
        Me.tabStyle.Controls.Add(Me.Label15)
        Me.tabStyle.Controls.Add(Me.TXTS1)
        Me.tabStyle.Controls.Add(Me.lblTen_Px)
        Me.tabStyle.Controls.Add(Me.Label1)
        Me.tabStyle.Controls.Add(Me.TxtHumidity)
        Me.tabStyle.Controls.Add(Me.txtDvtgauge)
        Me.tabStyle.Controls.Add(Me.lblGauge)
        Me.tabStyle.Controls.Add(Me.txtGauge)
        Me.tabStyle.Controls.Add(Me.txtDvtdensity)
        Me.tabStyle.Controls.Add(Me.lblDensity)
        Me.tabStyle.Controls.Add(Me.txtDensity)
        Me.tabStyle.Controls.Add(Me.txtDvtvolume0)
        Me.tabStyle.Controls.Add(Me.lblVolume0)
        Me.tabStyle.Controls.Add(Me.txtVolume0)
        Me.tabStyle.Controls.Add(Me.txtDvtweight0)
        Me.tabStyle.Controls.Add(Me.lblWeight0)
        Me.tabStyle.Controls.Add(Me.txtWeight0)
        Me.tabStyle.Controls.Add(Me.txtDvtdiameter)
        Me.tabStyle.Controls.Add(Me.lblDiameter)
        Me.tabStyle.Controls.Add(Me.txtDiameter)
        Me.tabStyle.Controls.Add(Me.txtDvtdepth)
        Me.tabStyle.Controls.Add(Me.lblDepth)
        Me.tabStyle.Controls.Add(Me.txtDepth)
        Me.tabStyle.Controls.Add(Me.txtDvtwidth0)
        Me.tabStyle.Controls.Add(Me.lblWidth0)
        Me.tabStyle.Controls.Add(Me.txtWidth0)
        Me.tabStyle.Controls.Add(Me.txtDvtlength0)
        Me.tabStyle.Controls.Add(Me.lblLength0)
        Me.tabStyle.Controls.Add(Me.txtLength0)
        Me.tabStyle.Controls.Add(Me.lblHeight0)
        Me.tabStyle.Controls.Add(Me.txtHeight0)
        Me.tabStyle.Controls.Add(Me.txtDvtweight2)
        Me.tabStyle.Controls.Add(Me.lblWeight2)
        Me.tabStyle.Controls.Add(Me.txtWeight2)
        Me.tabStyle.Controls.Add(Me.txtXstyle)
        Me.tabStyle.Controls.Add(Me.lblXstyle)
        Me.tabStyle.Controls.Add(Me.txtXsize)
        Me.tabStyle.Controls.Add(Me.lblXsize)
        Me.tabStyle.Controls.Add(Me.txtXcolor)
        Me.tabStyle.Controls.Add(Me.lblXcolor)
        Me.tabStyle.Controls.Add(Me.txtPack_size)
        Me.tabStyle.Controls.Add(Me.lblPack_size)
        Me.tabStyle.Controls.Add(Me.txtDvtpacks)
        Me.tabStyle.Controls.Add(Me.lblPacks)
        Me.tabStyle.Controls.Add(Me.txtPacks)
        Me.tabStyle.Controls.Add(Me.txtDvtweight)
        Me.tabStyle.Controls.Add(Me.lblWeight)
        Me.tabStyle.Controls.Add(Me.txtWeight)
        Me.tabStyle.Controls.Add(Me.txtDvtvolume)
        Me.tabStyle.Controls.Add(Me.lblVolume)
        Me.tabStyle.Controls.Add(Me.txtVolume)
        Me.tabStyle.Controls.Add(Me.txtDvtwidth)
        Me.tabStyle.Controls.Add(Me.lblWidth)
        Me.tabStyle.Controls.Add(Me.txtWidth)
        Me.tabStyle.Controls.Add(Me.txtDvtlength)
        Me.tabStyle.Controls.Add(Me.lblLength)
        Me.tabStyle.Controls.Add(Me.txtLength)
        Me.tabStyle.Controls.Add(Me.txtDvtheight)
        Me.tabStyle.Controls.Add(Me.txtNuoc_sx)
        Me.tabStyle.Controls.Add(Me.Label37)
        Me.tabStyle.Controls.Add(Me.txtMa_vt2)
        Me.tabStyle.Controls.Add(Me.lblMa_vt2)
        Me.tabStyle.Controls.Add(Me.txtHeight)
        Me.tabStyle.Controls.Add(Me.txtDvtheight0)
        Me.tabStyle.Controls.Add(Me.grpItem)
        Me.tabStyle.Controls.Add(Me.lblHeight)
        Me.tabStyle.Controls.Add(Me.grpFr)
        Me.tabStyle.Location = New System.Drawing.Point(4, 22)
        Me.tabStyle.Name = "tabStyle"
        Me.tabStyle.Size = New System.Drawing.Size(667, 477)
        Me.tabStyle.TabIndex = 4
        Me.tabStyle.Tag = "L500"
        Me.tabStyle.Text = "Kich co/Mau sac"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(250, 356)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(149, 13)
        Me.Label16.TabIndex = 171
        Me.Label16.Tag = ""
        Me.Label16.Text = "0 - Thuốc, 1 - TPCN, 2 - Khác"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(19, 357)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(78, 13)
        Me.Label17.TabIndex = 170
        Me.Label17.Tag = ""
        Me.Label17.Text = "Thuốc / TPCN"
        '
        'TXTS2
        '
        Me.TXTS2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TXTS2.Location = New System.Drawing.Point(143, 353)
        Me.TXTS2.Name = "TXTS2"
        Me.TXTS2.Size = New System.Drawing.Size(100, 20)
        Me.TXTS2.TabIndex = 41
        Me.TXTS2.Tag = "FCDF"
        Me.TXTS2.Text = "TXTS2"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(19, 331)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(64, 13)
        Me.Label15.TabIndex = 167
        Me.Label15.Tag = ""
        Me.Label15.Text = "Phân xưởng"
        '
        'TXTS1
        '
        Me.TXTS1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TXTS1.Location = New System.Drawing.Point(143, 327)
        Me.TXTS1.Name = "TXTS1"
        Me.TXTS1.Size = New System.Drawing.Size(100, 20)
        Me.TXTS1.TabIndex = 40
        Me.TXTS1.Tag = "FCDF"
        Me.TXTS1.Text = "TXTS1"
        '
        'lblTen_Px
        '
        Me.lblTen_Px.AutoSize = True
        Me.lblTen_Px.Location = New System.Drawing.Point(248, 331)
        Me.lblTen_Px.Name = "lblTen_Px"
        Me.lblTen_Px.Size = New System.Drawing.Size(40, 13)
        Me.lblTen_Px.TabIndex = 168
        Me.lblTen_Px.Text = "Ten px"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(298, 302)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 163
        Me.Label1.Tag = "L525"
        Me.Label1.Text = "Ty le do am"
        '
        'TxtHumidity
        '
        Me.TxtHumidity.Format = "m_ip_sl"
        Me.TxtHumidity.Location = New System.Drawing.Point(386, 300)
        Me.TxtHumidity.MaxLength = 8
        Me.TxtHumidity.Name = "TxtHumidity"
        Me.TxtHumidity.Size = New System.Drawing.Size(67, 20)
        Me.TxtHumidity.TabIndex = 39
        Me.TxtHumidity.Tag = "FN"
        Me.TxtHumidity.Text = "m_ip_sl"
        Me.TxtHumidity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TxtHumidity.Value = 0R
        '
        'txtDvtgauge
        '
        Me.txtDvtgauge.Location = New System.Drawing.Point(221, 56)
        Me.txtDvtgauge.Name = "txtDvtgauge"
        Me.txtDvtgauge.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtgauge.TabIndex = 3
        Me.txtDvtgauge.Tag = "FC"
        Me.txtDvtgauge.Text = "txtDvtgauge"
        '
        'lblGauge
        '
        Me.lblGauge.AutoSize = True
        Me.lblGauge.Location = New System.Drawing.Point(19, 56)
        Me.lblGauge.Name = "lblGauge"
        Me.lblGauge.Size = New System.Drawing.Size(105, 13)
        Me.lblGauge.TabIndex = 161
        Me.lblGauge.Tag = "L503"
        Me.lblGauge.Text = "Tieu chuan danh gia"
        '
        'txtGauge
        '
        Me.txtGauge.Format = "m_ip_sl"
        Me.txtGauge.Location = New System.Drawing.Point(151, 56)
        Me.txtGauge.MaxLength = 8
        Me.txtGauge.Name = "txtGauge"
        Me.txtGauge.Size = New System.Drawing.Size(67, 20)
        Me.txtGauge.TabIndex = 2
        Me.txtGauge.Tag = "FN"
        Me.txtGauge.Text = "m_ip_sl"
        Me.txtGauge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtGauge.Value = 0R
        '
        'txtDvtdensity
        '
        Me.txtDvtdensity.Location = New System.Drawing.Point(456, 278)
        Me.txtDvtdensity.Name = "txtDvtdensity"
        Me.txtDvtdensity.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtdensity.TabIndex = 38
        Me.txtDvtdensity.Tag = "FC"
        Me.txtDvtdensity.Text = "txtDVT"
        '
        'lblDensity
        '
        Me.lblDensity.AutoSize = True
        Me.lblDensity.Location = New System.Drawing.Point(298, 284)
        Me.lblDensity.Name = "lblDensity"
        Me.lblDensity.Size = New System.Drawing.Size(40, 13)
        Me.lblDensity.TabIndex = 158
        Me.lblDensity.Tag = "L524"
        Me.lblDensity.Text = "Mat do"
        '
        'txtDensity
        '
        Me.txtDensity.Format = "m_ip_sl"
        Me.txtDensity.Location = New System.Drawing.Point(386, 278)
        Me.txtDensity.MaxLength = 8
        Me.txtDensity.Name = "txtDensity"
        Me.txtDensity.Size = New System.Drawing.Size(67, 20)
        Me.txtDensity.TabIndex = 37
        Me.txtDensity.Tag = "FN"
        Me.txtDensity.Text = "m_ip_sl"
        Me.txtDensity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDensity.Value = 0R
        '
        'txtDvtvolume0
        '
        Me.txtDvtvolume0.Location = New System.Drawing.Point(456, 232)
        Me.txtDvtvolume0.Name = "txtDvtvolume0"
        Me.txtDvtvolume0.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtvolume0.TabIndex = 34
        Me.txtDvtvolume0.Tag = "FC"
        Me.txtDvtvolume0.Text = "txtDvtvolume0"
        '
        'lblVolume0
        '
        Me.lblVolume0.AutoSize = True
        Me.lblVolume0.Location = New System.Drawing.Point(298, 236)
        Me.lblVolume0.Name = "lblVolume0"
        Me.lblVolume0.Size = New System.Drawing.Size(46, 13)
        Me.lblVolume0.TabIndex = 154
        Me.lblVolume0.Tag = "L522"
        Me.lblVolume0.Text = "The tich"
        '
        'txtVolume0
        '
        Me.txtVolume0.Format = "m_ip_sl"
        Me.txtVolume0.Location = New System.Drawing.Point(386, 232)
        Me.txtVolume0.MaxLength = 8
        Me.txtVolume0.Name = "txtVolume0"
        Me.txtVolume0.Size = New System.Drawing.Size(67, 20)
        Me.txtVolume0.TabIndex = 33
        Me.txtVolume0.Tag = "FN"
        Me.txtVolume0.Text = "m_ip_sl"
        Me.txtVolume0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtVolume0.Value = 0R
        '
        'txtDvtweight0
        '
        Me.txtDvtweight0.Location = New System.Drawing.Point(456, 255)
        Me.txtDvtweight0.Name = "txtDvtweight0"
        Me.txtDvtweight0.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtweight0.TabIndex = 36
        Me.txtDvtweight0.Tag = "FC"
        Me.txtDvtweight0.Text = "txtDVT"
        '
        'lblWeight0
        '
        Me.lblWeight0.AutoSize = True
        Me.lblWeight0.Location = New System.Drawing.Point(298, 260)
        Me.lblWeight0.Name = "lblWeight0"
        Me.lblWeight0.Size = New System.Drawing.Size(57, 13)
        Me.lblWeight0.TabIndex = 151
        Me.lblWeight0.Tag = "L523"
        Me.lblWeight0.Text = "Khoi luong"
        '
        'txtWeight0
        '
        Me.txtWeight0.Format = "m_ip_sl"
        Me.txtWeight0.Location = New System.Drawing.Point(386, 255)
        Me.txtWeight0.MaxLength = 8
        Me.txtWeight0.Name = "txtWeight0"
        Me.txtWeight0.Size = New System.Drawing.Size(67, 20)
        Me.txtWeight0.TabIndex = 35
        Me.txtWeight0.Tag = "FN"
        Me.txtWeight0.Text = "m_ip_sl"
        Me.txtWeight0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtWeight0.Value = 0R
        '
        'txtDvtdiameter
        '
        Me.txtDvtdiameter.Location = New System.Drawing.Point(456, 209)
        Me.txtDvtdiameter.Name = "txtDvtdiameter"
        Me.txtDvtdiameter.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtdiameter.TabIndex = 32
        Me.txtDvtdiameter.Tag = "FC"
        Me.txtDvtdiameter.Text = "txtDvtdiameter"
        '
        'lblDiameter
        '
        Me.lblDiameter.AutoSize = True
        Me.lblDiameter.Location = New System.Drawing.Point(298, 212)
        Me.lblDiameter.Name = "lblDiameter"
        Me.lblDiameter.Size = New System.Drawing.Size(62, 13)
        Me.lblDiameter.TabIndex = 148
        Me.lblDiameter.Tag = "L521"
        Me.lblDiameter.Text = "Duong kinh"
        '
        'txtDiameter
        '
        Me.txtDiameter.Format = "m_ip_sl"
        Me.txtDiameter.Location = New System.Drawing.Point(386, 209)
        Me.txtDiameter.MaxLength = 8
        Me.txtDiameter.Name = "txtDiameter"
        Me.txtDiameter.Size = New System.Drawing.Size(67, 20)
        Me.txtDiameter.TabIndex = 31
        Me.txtDiameter.Tag = "FN"
        Me.txtDiameter.Text = "m_ip_sl"
        Me.txtDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDiameter.Value = 0R
        '
        'txtDvtdepth
        '
        Me.txtDvtdepth.Location = New System.Drawing.Point(456, 186)
        Me.txtDvtdepth.Name = "txtDvtdepth"
        Me.txtDvtdepth.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtdepth.TabIndex = 30
        Me.txtDvtdepth.Tag = "FC"
        Me.txtDvtdepth.Text = "txtDvtdepth"
        '
        'lblDepth
        '
        Me.lblDepth.AutoSize = True
        Me.lblDepth.Location = New System.Drawing.Point(298, 188)
        Me.lblDepth.Name = "lblDepth"
        Me.lblDepth.Size = New System.Drawing.Size(41, 13)
        Me.lblDepth.TabIndex = 145
        Me.lblDepth.Tag = "L520"
        Me.lblDepth.Text = "Do sau"
        '
        'txtDepth
        '
        Me.txtDepth.Format = "m_ip_sl"
        Me.txtDepth.Location = New System.Drawing.Point(386, 186)
        Me.txtDepth.MaxLength = 8
        Me.txtDepth.Name = "txtDepth"
        Me.txtDepth.Size = New System.Drawing.Size(67, 20)
        Me.txtDepth.TabIndex = 29
        Me.txtDepth.Tag = "FN"
        Me.txtDepth.Text = "m_ip_sl"
        Me.txtDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDepth.Value = 0R
        '
        'txtDvtwidth0
        '
        Me.txtDvtwidth0.Location = New System.Drawing.Point(456, 163)
        Me.txtDvtwidth0.Name = "txtDvtwidth0"
        Me.txtDvtwidth0.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtwidth0.TabIndex = 28
        Me.txtDvtwidth0.Tag = "FC"
        Me.txtDvtwidth0.Text = "txtDvtwidth0"
        '
        'lblWidth0
        '
        Me.lblWidth0.AutoSize = True
        Me.lblWidth0.Location = New System.Drawing.Point(298, 164)
        Me.lblWidth0.Name = "lblWidth0"
        Me.lblWidth0.Size = New System.Drawing.Size(58, 13)
        Me.lblWidth0.TabIndex = 142
        Me.lblWidth0.Tag = "L519"
        Me.lblWidth0.Text = "Chieu rong"
        '
        'txtWidth0
        '
        Me.txtWidth0.Format = "m_ip_sl"
        Me.txtWidth0.Location = New System.Drawing.Point(386, 163)
        Me.txtWidth0.MaxLength = 8
        Me.txtWidth0.Name = "txtWidth0"
        Me.txtWidth0.Size = New System.Drawing.Size(67, 20)
        Me.txtWidth0.TabIndex = 27
        Me.txtWidth0.Tag = "FN"
        Me.txtWidth0.Text = "m_ip_sl"
        Me.txtWidth0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtWidth0.Value = 0R
        '
        'txtDvtlength0
        '
        Me.txtDvtlength0.Location = New System.Drawing.Point(456, 139)
        Me.txtDvtlength0.Name = "txtDvtlength0"
        Me.txtDvtlength0.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtlength0.TabIndex = 26
        Me.txtDvtlength0.Tag = "FC"
        Me.txtDvtlength0.Text = "txtDvtlength0"
        '
        'lblLength0
        '
        Me.lblLength0.AutoSize = True
        Me.lblLength0.Location = New System.Drawing.Point(298, 140)
        Me.lblLength0.Name = "lblLength0"
        Me.lblLength0.Size = New System.Drawing.Size(51, 13)
        Me.lblLength0.TabIndex = 139
        Me.lblLength0.Tag = "L518"
        Me.lblLength0.Text = "Chieu dai"
        '
        'txtLength0
        '
        Me.txtLength0.Format = "m_ip_sl"
        Me.txtLength0.Location = New System.Drawing.Point(386, 139)
        Me.txtLength0.MaxLength = 8
        Me.txtLength0.Name = "txtLength0"
        Me.txtLength0.Size = New System.Drawing.Size(67, 20)
        Me.txtLength0.TabIndex = 25
        Me.txtLength0.Tag = "FN"
        Me.txtLength0.Text = "m_ip_sl"
        Me.txtLength0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtLength0.Value = 0R
        '
        'lblHeight0
        '
        Me.lblHeight0.AutoSize = True
        Me.lblHeight0.Location = New System.Drawing.Point(298, 118)
        Me.lblHeight0.Name = "lblHeight0"
        Me.lblHeight0.Size = New System.Drawing.Size(55, 13)
        Me.lblHeight0.TabIndex = 136
        Me.lblHeight0.Tag = "L517"
        Me.lblHeight0.Text = "Chieu cao"
        '
        'txtHeight0
        '
        Me.txtHeight0.Format = "m_ip_sl"
        Me.txtHeight0.Location = New System.Drawing.Point(386, 116)
        Me.txtHeight0.MaxLength = 8
        Me.txtHeight0.Name = "txtHeight0"
        Me.txtHeight0.Size = New System.Drawing.Size(67, 20)
        Me.txtHeight0.TabIndex = 23
        Me.txtHeight0.Tag = "FN"
        Me.txtHeight0.Text = "m_ip_sl"
        Me.txtHeight0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtHeight0.Value = 0R
        '
        'txtDvtweight2
        '
        Me.txtDvtweight2.Location = New System.Drawing.Point(199, 232)
        Me.txtDvtweight2.Name = "txtDvtweight2"
        Me.txtDvtweight2.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtweight2.TabIndex = 19
        Me.txtDvtweight2.Tag = "FC"
        Me.txtDvtweight2.Text = "txtDvtweight2"
        '
        'lblWeight2
        '
        Me.lblWeight2.AutoSize = True
        Me.lblWeight2.Location = New System.Drawing.Point(41, 234)
        Me.lblWeight2.Name = "lblWeight2"
        Me.lblWeight2.Size = New System.Drawing.Size(37, 13)
        Me.lblWeight2.TabIndex = 132
        Me.lblWeight2.Tag = "L513"
        Me.lblWeight2.Text = "Bao bi"
        '
        'txtWeight2
        '
        Me.txtWeight2.Format = "m_ip_sl"
        Me.txtWeight2.Location = New System.Drawing.Point(129, 232)
        Me.txtWeight2.MaxLength = 8
        Me.txtWeight2.Name = "txtWeight2"
        Me.txtWeight2.Size = New System.Drawing.Size(67, 20)
        Me.txtWeight2.TabIndex = 18
        Me.txtWeight2.Tag = "FN"
        Me.txtWeight2.Text = "m_ip_sl"
        Me.txtWeight2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtWeight2.Value = 0R
        '
        'txtXstyle
        '
        Me.txtXstyle.Location = New System.Drawing.Point(404, 56)
        Me.txtXstyle.Name = "txtXstyle"
        Me.txtXstyle.Size = New System.Drawing.Size(100, 20)
        Me.txtXstyle.TabIndex = 6
        Me.txtXstyle.Tag = "FC"
        Me.txtXstyle.Text = "txtXstyle"
        '
        'lblXstyle
        '
        Me.lblXstyle.AutoSize = True
        Me.lblXstyle.Location = New System.Drawing.Point(280, 58)
        Me.lblXstyle.Name = "lblXstyle"
        Me.lblXstyle.Size = New System.Drawing.Size(75, 13)
        Me.lblXstyle.TabIndex = 130
        Me.lblXstyle.Tag = "L506"
        Me.lblXstyle.Text = "Dang bao che"
        '
        'txtXsize
        '
        Me.txtXsize.Location = New System.Drawing.Point(404, 33)
        Me.txtXsize.Name = "txtXsize"
        Me.txtXsize.Size = New System.Drawing.Size(100, 20)
        Me.txtXsize.TabIndex = 5
        Me.txtXsize.Tag = "FC"
        Me.txtXsize.Text = "txtXsize"
        '
        'lblXsize
        '
        Me.lblXsize.AutoSize = True
        Me.lblXsize.Location = New System.Drawing.Point(280, 35)
        Me.lblXsize.Name = "lblXsize"
        Me.lblXsize.Size = New System.Drawing.Size(43, 13)
        Me.lblXsize.TabIndex = 128
        Me.lblXsize.Tag = "L505"
        Me.lblXsize.Text = "Kich co"
        '
        'txtXcolor
        '
        Me.txtXcolor.Location = New System.Drawing.Point(404, 10)
        Me.txtXcolor.Name = "txtXcolor"
        Me.txtXcolor.Size = New System.Drawing.Size(100, 20)
        Me.txtXcolor.TabIndex = 4
        Me.txtXcolor.Tag = "FC"
        Me.txtXcolor.Text = "txtXcolor"
        '
        'lblXcolor
        '
        Me.lblXcolor.AutoSize = True
        Me.lblXcolor.Location = New System.Drawing.Point(280, 12)
        Me.lblXcolor.Name = "lblXcolor"
        Me.lblXcolor.Size = New System.Drawing.Size(48, 13)
        Me.lblXcolor.TabIndex = 126
        Me.lblXcolor.Tag = "L504"
        Me.lblXcolor.Text = "Mau sac"
        '
        'txtPack_size
        '
        Me.txtPack_size.Location = New System.Drawing.Point(129, 278)
        Me.txtPack_size.Name = "txtPack_size"
        Me.txtPack_size.Size = New System.Drawing.Size(100, 20)
        Me.txtPack_size.TabIndex = 22
        Me.txtPack_size.Tag = "FC"
        Me.txtPack_size.Text = "txtPack_size"
        '
        'lblPack_size
        '
        Me.lblPack_size.AutoSize = True
        Me.lblPack_size.Location = New System.Drawing.Point(41, 280)
        Me.lblPack_size.Name = "lblPack_size"
        Me.lblPack_size.Size = New System.Drawing.Size(60, 13)
        Me.lblPack_size.TabIndex = 124
        Me.lblPack_size.Tag = "L515"
        Me.lblPack_size.Text = "Kich co goi"
        '
        'txtDvtpacks
        '
        Me.txtDvtpacks.Location = New System.Drawing.Point(199, 255)
        Me.txtDvtpacks.Name = "txtDvtpacks"
        Me.txtDvtpacks.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtpacks.TabIndex = 21
        Me.txtDvtpacks.Tag = "FC"
        Me.txtDvtpacks.Text = "txtDVT"
        '
        'lblPacks
        '
        Me.lblPacks.AutoSize = True
        Me.lblPacks.Location = New System.Drawing.Point(41, 257)
        Me.lblPacks.Name = "lblPacks"
        Me.lblPacks.Size = New System.Drawing.Size(37, 13)
        Me.lblPacks.TabIndex = 121
        Me.lblPacks.Tag = "L514"
        Me.lblPacks.Text = "So goi"
        '
        'txtPacks
        '
        Me.txtPacks.Format = "m_ip_sl"
        Me.txtPacks.Location = New System.Drawing.Point(129, 255)
        Me.txtPacks.MaxLength = 8
        Me.txtPacks.Name = "txtPacks"
        Me.txtPacks.Size = New System.Drawing.Size(67, 20)
        Me.txtPacks.TabIndex = 20
        Me.txtPacks.Tag = "FN"
        Me.txtPacks.Text = "m_ip_sl"
        Me.txtPacks.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtPacks.Value = 0R
        '
        'txtDvtweight
        '
        Me.txtDvtweight.Location = New System.Drawing.Point(199, 209)
        Me.txtDvtweight.Name = "txtDvtweight"
        Me.txtDvtweight.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtweight.TabIndex = 17
        Me.txtDvtweight.Tag = "FC"
        Me.txtDvtweight.Text = "txtDvtweight"
        '
        'lblWeight
        '
        Me.lblWeight.AutoSize = True
        Me.lblWeight.Location = New System.Drawing.Point(41, 211)
        Me.lblWeight.Name = "lblWeight"
        Me.lblWeight.Size = New System.Drawing.Size(57, 13)
        Me.lblWeight.TabIndex = 118
        Me.lblWeight.Tag = "L512"
        Me.lblWeight.Text = "Khoi luong"
        '
        'txtWeight
        '
        Me.txtWeight.Format = "m_ip_sl"
        Me.txtWeight.Location = New System.Drawing.Point(129, 209)
        Me.txtWeight.MaxLength = 8
        Me.txtWeight.Name = "txtWeight"
        Me.txtWeight.Size = New System.Drawing.Size(67, 20)
        Me.txtWeight.TabIndex = 16
        Me.txtWeight.Tag = "FN"
        Me.txtWeight.Text = "m_ip_sl"
        Me.txtWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtWeight.Value = 0R
        '
        'txtDvtvolume
        '
        Me.txtDvtvolume.Location = New System.Drawing.Point(199, 186)
        Me.txtDvtvolume.Name = "txtDvtvolume"
        Me.txtDvtvolume.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtvolume.TabIndex = 15
        Me.txtDvtvolume.Tag = "FC"
        Me.txtDvtvolume.Text = "txtDvtvolume"
        '
        'lblVolume
        '
        Me.lblVolume.AutoSize = True
        Me.lblVolume.Location = New System.Drawing.Point(41, 188)
        Me.lblVolume.Name = "lblVolume"
        Me.lblVolume.Size = New System.Drawing.Size(46, 13)
        Me.lblVolume.TabIndex = 115
        Me.lblVolume.Tag = "L511"
        Me.lblVolume.Text = "The tich"
        '
        'txtVolume
        '
        Me.txtVolume.Format = "m_ip_sl"
        Me.txtVolume.Location = New System.Drawing.Point(129, 186)
        Me.txtVolume.MaxLength = 8
        Me.txtVolume.Name = "txtVolume"
        Me.txtVolume.Size = New System.Drawing.Size(67, 20)
        Me.txtVolume.TabIndex = 14
        Me.txtVolume.Tag = "FN"
        Me.txtVolume.Text = "m_ip_sl"
        Me.txtVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtVolume.Value = 0R
        '
        'txtDvtwidth
        '
        Me.txtDvtwidth.Location = New System.Drawing.Point(199, 163)
        Me.txtDvtwidth.Name = "txtDvtwidth"
        Me.txtDvtwidth.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtwidth.TabIndex = 13
        Me.txtDvtwidth.Tag = "FC"
        Me.txtDvtwidth.Text = "txtDVT"
        '
        'lblWidth
        '
        Me.lblWidth.AutoSize = True
        Me.lblWidth.Location = New System.Drawing.Point(41, 165)
        Me.lblWidth.Name = "lblWidth"
        Me.lblWidth.Size = New System.Drawing.Size(58, 13)
        Me.lblWidth.TabIndex = 112
        Me.lblWidth.Tag = "L510"
        Me.lblWidth.Text = "Chieu rong"
        '
        'txtWidth
        '
        Me.txtWidth.Format = "m_ip_sl"
        Me.txtWidth.Location = New System.Drawing.Point(129, 163)
        Me.txtWidth.MaxLength = 8
        Me.txtWidth.Name = "txtWidth"
        Me.txtWidth.Size = New System.Drawing.Size(67, 20)
        Me.txtWidth.TabIndex = 12
        Me.txtWidth.Tag = "FN"
        Me.txtWidth.Text = "m_ip_sl"
        Me.txtWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtWidth.Value = 0R
        '
        'txtDvtlength
        '
        Me.txtDvtlength.Location = New System.Drawing.Point(199, 139)
        Me.txtDvtlength.Name = "txtDvtlength"
        Me.txtDvtlength.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtlength.TabIndex = 11
        Me.txtDvtlength.Tag = "FC"
        Me.txtDvtlength.Text = "txtDvtlength"
        '
        'lblLength
        '
        Me.lblLength.AutoSize = True
        Me.lblLength.Location = New System.Drawing.Point(41, 141)
        Me.lblLength.Name = "lblLength"
        Me.lblLength.Size = New System.Drawing.Size(51, 13)
        Me.lblLength.TabIndex = 109
        Me.lblLength.Tag = "L509"
        Me.lblLength.Text = "Chieu dai"
        '
        'txtLength
        '
        Me.txtLength.Format = "m_ip_sl"
        Me.txtLength.Location = New System.Drawing.Point(129, 139)
        Me.txtLength.MaxLength = 8
        Me.txtLength.Name = "txtLength"
        Me.txtLength.Size = New System.Drawing.Size(67, 20)
        Me.txtLength.TabIndex = 10
        Me.txtLength.Tag = "FN"
        Me.txtLength.Text = "m_ip_sl"
        Me.txtLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtLength.Value = 0R
        '
        'txtDvtheight
        '
        Me.txtDvtheight.Location = New System.Drawing.Point(199, 116)
        Me.txtDvtheight.Name = "txtDvtheight"
        Me.txtDvtheight.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtheight.TabIndex = 9
        Me.txtDvtheight.Tag = "FC"
        Me.txtDvtheight.Text = "txtDVT"
        '
        'txtNuoc_sx
        '
        Me.txtNuoc_sx.Location = New System.Drawing.Point(151, 33)
        Me.txtNuoc_sx.Name = "txtNuoc_sx"
        Me.txtNuoc_sx.Size = New System.Drawing.Size(100, 20)
        Me.txtNuoc_sx.TabIndex = 1
        Me.txtNuoc_sx.Tag = "FC"
        Me.txtNuoc_sx.Text = "txtNuoc_sx"
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(19, 35)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(46, 13)
        Me.Label37.TabIndex = 104
        Me.Label37.Tag = "L502"
        Me.Label37.Text = "Nuoc sx"
        '
        'txtMa_vt2
        '
        Me.txtMa_vt2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_vt2.Location = New System.Drawing.Point(151, 10)
        Me.txtMa_vt2.Name = "txtMa_vt2"
        Me.txtMa_vt2.Size = New System.Drawing.Size(66, 20)
        Me.txtMa_vt2.TabIndex = 0
        Me.txtMa_vt2.Tag = "FC"
        Me.txtMa_vt2.Text = "TXTMA_VT2"
        '
        'lblMa_vt2
        '
        Me.lblMa_vt2.AutoSize = True
        Me.lblMa_vt2.Location = New System.Drawing.Point(19, 12)
        Me.lblMa_vt2.Name = "lblMa_vt2"
        Me.lblMa_vt2.Size = New System.Drawing.Size(43, 13)
        Me.lblMa_vt2.TabIndex = 102
        Me.lblMa_vt2.Tag = "L501"
        Me.lblMa_vt2.Text = "Ma phu"
        '
        'txtHeight
        '
        Me.txtHeight.Format = "m_ip_sl"
        Me.txtHeight.Location = New System.Drawing.Point(129, 116)
        Me.txtHeight.MaxLength = 8
        Me.txtHeight.Name = "txtHeight"
        Me.txtHeight.Size = New System.Drawing.Size(67, 20)
        Me.txtHeight.TabIndex = 8
        Me.txtHeight.Tag = "FN"
        Me.txtHeight.Text = "m_ip_sl"
        Me.txtHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtHeight.Value = 0R
        '
        'txtDvtheight0
        '
        Me.txtDvtheight0.Location = New System.Drawing.Point(456, 116)
        Me.txtDvtheight0.Name = "txtDvtheight0"
        Me.txtDvtheight0.Size = New System.Drawing.Size(30, 20)
        Me.txtDvtheight0.TabIndex = 24
        Me.txtDvtheight0.Tag = "FC"
        Me.txtDvtheight0.Text = "txtDvtheight0"
        '
        'grpItem
        '
        Me.grpItem.Location = New System.Drawing.Point(280, 100)
        Me.grpItem.Name = "grpItem"
        Me.grpItem.Size = New System.Drawing.Size(224, 224)
        Me.grpItem.TabIndex = 20
        Me.grpItem.TabStop = False
        Me.grpItem.Tag = "L516"
        Me.grpItem.Text = "Mat hang"
        '
        'lblHeight
        '
        Me.lblHeight.AutoSize = True
        Me.lblHeight.Location = New System.Drawing.Point(41, 118)
        Me.lblHeight.Name = "lblHeight"
        Me.lblHeight.Size = New System.Drawing.Size(55, 13)
        Me.lblHeight.TabIndex = 106
        Me.lblHeight.Tag = "L508"
        Me.lblHeight.Text = "Chieu cao"
        '
        'grpFr
        '
        Me.grpFr.Location = New System.Drawing.Point(23, 100)
        Me.grpFr.Name = "grpFr"
        Me.grpFr.Size = New System.Drawing.Size(232, 208)
        Me.grpFr.TabIndex = 8
        Me.grpFr.TabStop = False
        Me.grpFr.Tag = "L507"
        Me.grpFr.Text = "Van chuyen"
        '
        'tabOther
        '
        Me.tabOther.Controls.Add(Me.Label23)
        Me.tabOther.Controls.Add(Me.txtPharmacy_status)
        Me.tabOther.Controls.Add(Me.Label24)
        Me.tabOther.Controls.Add(Me.Label21)
        Me.tabOther.Controls.Add(Me.txtPharmacy_yn)
        Me.tabOther.Controls.Add(Me.Label22)
        Me.tabOther.Controls.Add(Me.txtNong_do_ham_luong)
        Me.tabOther.Controls.Add(Me.Label18)
        Me.tabOther.Controls.Add(Me.txtXhoatchat)
        Me.tabOther.Controls.Add(Me.Label14)
        Me.tabOther.Controls.Add(Me.lblGhi_chu)
        Me.tabOther.Controls.Add(Me.txtGhi_chu)
        Me.tabOther.Location = New System.Drawing.Point(4, 22)
        Me.tabOther.Name = "tabOther"
        Me.tabOther.Size = New System.Drawing.Size(667, 477)
        Me.tabOther.TabIndex = 5
        Me.tabOther.Tag = "L600"
        Me.tabOther.Text = "Ghi chu"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(11, 361)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(115, 13)
        Me.Label23.TabIndex = 174
        Me.Label23.Tag = "L117"
        Me.Label23.Text = "Đưa lên dược quốc gia"
        '
        'txtPharmacy_status
        '
        Me.txtPharmacy_status.Location = New System.Drawing.Point(142, 360)
        Me.txtPharmacy_status.MaxLength = 1
        Me.txtPharmacy_status.Name = "txtPharmacy_status"
        Me.txtPharmacy_status.Size = New System.Drawing.Size(25, 20)
        Me.txtPharmacy_status.TabIndex = 4
        Me.txtPharmacy_status.TabStop = False
        Me.txtPharmacy_status.Tag = "FN"
        Me.txtPharmacy_status.Text = "txtPharmacy_status"
        Me.txtPharmacy_status.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(176, 361)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(128, 13)
        Me.Label24.TabIndex = 175
        Me.Label24.Tag = "L118"
        Me.Label24.Text = "1 - Co su dung, 0 - Khong"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(11, 338)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(94, 13)
        Me.Label21.TabIndex = 171
        Me.Label21.Tag = ""
        Me.Label21.Text = "Pharmacy Product"
        '
        'txtPharmacy_yn
        '
        Me.txtPharmacy_yn.Location = New System.Drawing.Point(142, 336)
        Me.txtPharmacy_yn.MaxLength = 1
        Me.txtPharmacy_yn.Name = "txtPharmacy_yn"
        Me.txtPharmacy_yn.Size = New System.Drawing.Size(25, 20)
        Me.txtPharmacy_yn.TabIndex = 3
        Me.txtPharmacy_yn.TabStop = False
        Me.txtPharmacy_yn.Tag = "FC"
        Me.txtPharmacy_yn.Text = "txtPharmacy_yn"
        Me.txtPharmacy_yn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(176, 338)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(72, 13)
        Me.Label22.TabIndex = 172
        Me.Label22.Tag = ""
        Me.Label22.Text = "1- Yes, 0 - No"
        '
        'txtNong_do_ham_luong
        '
        Me.txtNong_do_ham_luong.Location = New System.Drawing.Point(72, 196)
        Me.txtNong_do_ham_luong.Multiline = True
        Me.txtNong_do_ham_luong.Name = "txtNong_do_ham_luong"
        Me.txtNong_do_ham_luong.Size = New System.Drawing.Size(444, 138)
        Me.txtNong_do_ham_luong.TabIndex = 2
        Me.txtNong_do_ham_luong.Tag = "FC"
        Me.txtNong_do_ham_luong.Text = "txtNong_do_ham_luong"
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(8, 199)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(62, 49)
        Me.Label18.TabIndex = 169
        Me.Label18.Tag = ""
        Me.Label18.Text = "Nồng độ hàm lượng"
        '
        'txtXhoatchat
        '
        Me.txtXhoatchat.Location = New System.Drawing.Point(72, 53)
        Me.txtXhoatchat.Multiline = True
        Me.txtXhoatchat.Name = "txtXhoatchat"
        Me.txtXhoatchat.Size = New System.Drawing.Size(443, 138)
        Me.txtXhoatchat.TabIndex = 1
        Me.txtXhoatchat.Tag = "FC"
        Me.txtXhoatchat.Text = "txtXhoatchat"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(9, 56)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(54, 13)
        Me.Label14.TabIndex = 167
        Me.Label14.Tag = "L526"
        Me.Label14.Text = "Hoat chat"
        '
        'lblGhi_chu
        '
        Me.lblGhi_chu.AutoSize = True
        Me.lblGhi_chu.Location = New System.Drawing.Point(9, 12)
        Me.lblGhi_chu.Name = "lblGhi_chu"
        Me.lblGhi_chu.Size = New System.Drawing.Size(44, 13)
        Me.lblGhi_chu.TabIndex = 55
        Me.lblGhi_chu.Tag = "L601"
        Me.lblGhi_chu.Text = "Ghi chu"
        '
        'tabUD
        '
        Me.tabUD.Location = New System.Drawing.Point(4, 22)
        Me.tabUD.Name = "tabUD"
        Me.tabUD.Size = New System.Drawing.Size(667, 477)
        Me.tabUD.TabIndex = 6
        Me.tabUD.Tag = "L700"
        Me.tabUD.Text = "Khac"
        '
        'frmDirInfor
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(692, 644)
        Me.Controls.Add(Me.tabInfor)
        Me.Controls.Add(Me.lblMa_vt)
        Me.Controls.Add(Me.lblTen_vt2)
        Me.Controls.Add(Me.lblTen_vt)
        Me.Controls.Add(Me.txtTen_vt2)
        Me.Controls.Add(Me.txtTen_vt)
        Me.Controls.Add(Me.txtMa_vt)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.grpInfor)
        Me.Name = "frmDirInfor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmDirInfor"
        Me.tabInfor.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabMain.PerformLayout()
        Me.tabAccount.ResumeLayout(False)
        Me.tabAccount.PerformLayout()
        Me.tabPur.ResumeLayout(False)
        Me.tabPur.PerformLayout()
        Me.tabLot.ResumeLayout(False)
        Me.tabLot.PerformLayout()
        Me.tabMRP.ResumeLayout(False)
        Me.tabMRP.PerformLayout()
        Me.tabStyle.ResumeLayout(False)
        Me.tabStyle.PerformLayout()
        Me.tabOther.ResumeLayout(False)
        Me.tabOther.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private Sub txtAbc_code_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtAbc_code.KeyDown
        If ((e.KeyData = Keys.Back) Or (e.KeyData = Keys.Delete)) Then
            Me.txtAbc_code.Text = ""
        End If
    End Sub

    Private Sub txtAbc_code_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtAbc_code.KeyPress
        If Fox.InList(Strings.Trim(StringType.FromChar(e.KeyChar)), New Object() {"a", "b", "c"}) Then
            Me.txtAbc_code.Text = StringType.FromChar(Strings.UCase(e.KeyChar))
        End If
    End Sub

    Private Sub txtMa_vi_tri_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMa_vi_tri.Enter
        If (StringType.StrCmp(Me.txtMa_kho.Text, "", False) = 0) Then
            Me.txtMa_vi_tri.Text = ""
            Me.lblTen_vi_tri.Text = ""
            Me.txtMa_vi_tri.ReadOnly = True
        Else
            Me.txtMa_vi_tri.ReadOnly = False
            Me.oLocation.Key = ("ma_kho = '" & Strings.Trim(Me.txtMa_kho.Text) & "'")
        End If
    End Sub

    Private Sub txtTg_th_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtTg_th.Validated
        Me.txtTg_th2.Value = Me.txtTg_th.Value
    End Sub

    Private Sub txtTg_th2_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtTg_th2.Validated
        Me.txtTg_th.Value = Me.txtTg_th2.Value
    End Sub


    ' Properties
    Friend WithEvents cbbGia_ton As ComboBox
    Friend WithEvents chkKk_yn As CheckBox
    Friend WithEvents chkLo_yn As CheckBox
    Friend WithEvents chkNhieu_dvt As CheckBox
    Friend WithEvents chkSua_tk_vt As CheckBox
    Friend WithEvents chkTao_lo As CheckBox
    Friend WithEvents chkVt_ton_kho As CheckBox
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grpFr As GroupBox
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents grpItem As GroupBox
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label37 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents lblAbc_code As Label
    Friend WithEvents lblCach_xuat As Label
    Friend WithEvents lblCach_xuatMess As Label
    Friend WithEvents lblDensity As Label
    Friend WithEvents lblDepth As Label
    Friend WithEvents lblDiameter As Label
    Friend WithEvents lblDvt As Label
    Friend WithEvents lblGauge As Label
    Friend WithEvents lblGhi_chu As Label
    Friend WithEvents lblGia_ton As Label
    Friend WithEvents lblHeight As Label
    Friend WithEvents lblHeight0 As Label
    Friend WithEvents lblKieu_lo As Label
    Friend WithEvents lblKieu_loMess As Label
    Friend WithEvents lblLength As Label
    Friend WithEvents lblLength0 As Label
    Friend WithEvents lblLoai_vt As Label
    Friend WithEvents lblMa_kh As Label
    Friend WithEvents lblMa_kh0 As Label
    Friend WithEvents lblMa_kh2 As Label
    Friend WithEvents lblMa_kho As Label
    Friend WithEvents lblMa_thue As Label
    Friend WithEvents LblMa_thue_nk As Label
    Friend WithEvents lblMa_vi_tri As Label
    Friend WithEvents lblMa_vt As Label
    Friend WithEvents lblMa_vt2 As Label
    Friend WithEvents lblNh_vt1 As Label
    Friend WithEvents lblNh_vt9 As Label
    Friend WithEvents lblOng_ba As Label
    Friend WithEvents lblPack_size As Label
    Friend WithEvents lblPacks As Label
    Friend WithEvents lblSl_max As Label
    Friend WithEvents lblSl_min As Label
    Friend WithEvents lblSo_lo_chuan As Label
    Friend WithEvents lblSo_ngay_bh As Label
    Friend WithEvents lblSo_ngay_sp As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblStatusMess As Label
    Friend WithEvents lblTen_kh As Label
    Friend WithEvents lblTen_kh0 As Label
    Friend WithEvents lblTen_kh2 As Label
    Friend WithEvents lblTen_lo_trinh As Label
    Friend WithEvents lblTen_loai_vt As Label
    Friend WithEvents lblTen_nh_vt1 As Label
    Friend WithEvents lblTen_nh_vt2 As Label
    Friend WithEvents lblTen_nh_vt3 As Label
    Friend WithEvents lblTen_nh_vt9 As Label
    Friend WithEvents lblTen_thue As Label
    Friend WithEvents lblTen_thue_nk As Label
    Friend WithEvents lblTen_tk_ck As Label
    Friend WithEvents lblTen_tk_cl_vt As Label
    Friend WithEvents lblTen_tk_cpbh As Label
    Friend WithEvents lblTen_tk_dl As Label
    Friend WithEvents lblTen_tk_dt As Label
    Friend WithEvents lblTen_tk_dtnb As Label
    Friend WithEvents lblTen_tk_gv As Label
    Friend WithEvents lblTen_tk_spdd As Label
    Friend WithEvents lblTen_tk_tl As Label
    Friend WithEvents lblTen_tk_vt As Label
    Friend WithEvents lblTen_vi_tri As Label
    Friend WithEvents lblTen_vt As Label
    Friend WithEvents lblTen_vt2 As Label
    Friend WithEvents lblTg_th As Label
    Friend WithEvents lblTk_cl_vt As Label
    Friend WithEvents lblTk_dl As Label
    Friend WithEvents lblTk_dt As Label
    Friend WithEvents lblTk_dtnb As Label
    Friend WithEvents lblTk_gv As Label
    Friend WithEvents lblTk_spdd As Label
    Friend WithEvents lblTk_tl As Label
    Friend WithEvents lblTk_vt As Label
    Friend WithEvents lblVolume As Label
    Friend WithEvents lblVolume0 As Label
    Friend WithEvents lblWeight As Label
    Friend WithEvents lblWeight0 As Label
    Friend WithEvents lblWeight2 As Label
    Friend WithEvents lblWidth As Label
    Friend WithEvents lblWidth0 As Label
    Friend WithEvents lblXcolor As Label
    Friend WithEvents lblXsize As Label
    Friend WithEvents lblXstyle As Label
    Friend WithEvents tabAccount As TabPage
    Friend WithEvents tabInfor As TabControl
    Friend WithEvents tabLot As TabPage
    Friend WithEvents tabMain As TabPage
    Friend WithEvents tabMRP As TabPage
    Friend WithEvents tabOther As TabPage
    Friend WithEvents tabPur As TabPage
    Friend WithEvents tabStyle As TabPage
    Friend WithEvents tabUD As TabPage
    Friend WithEvents txtAbc_code As TextBox
    Friend WithEvents txtCach_xuat As TextBox
    Friend WithEvents txtCo_lo As txtNumeric
    Friend WithEvents txtDensity As txtNumeric
    Friend WithEvents txtDepth As txtNumeric
    Friend WithEvents txtDiameter As txtNumeric
    Friend WithEvents txtDvt As TextBox
    Friend WithEvents txtDvtdensity As TextBox
    Friend WithEvents txtDvtdepth As TextBox
    Friend WithEvents txtDvtdiameter As TextBox
    Friend WithEvents txtDvtgauge As TextBox
    Friend WithEvents txtDvtheight As TextBox
    Friend WithEvents txtDvtheight0 As TextBox
    Friend WithEvents txtDvtlength As TextBox
    Friend WithEvents txtDvtlength0 As TextBox
    Friend WithEvents txtDvtpacks As TextBox
    Friend WithEvents txtDvtsl_lo_chuan As TextBox
    Friend WithEvents txtDvttg_th As TextBox
    Friend WithEvents txtDvtvolume As TextBox
    Friend WithEvents txtDvtvolume0 As TextBox
    Friend WithEvents txtDvtweight As TextBox
    Friend WithEvents txtDvtweight0 As TextBox
    Friend WithEvents txtDvtweight2 As TextBox
    Friend WithEvents txtDvtwidth As TextBox
    Friend WithEvents txtDvtwidth0 As TextBox
    Friend WithEvents txtGauge As txtNumeric
    Friend WithEvents txtGhi_chu As TextBox
    Friend WithEvents txtGia_ton As TextBox
    Friend WithEvents txtHeight As txtNumeric
    Friend WithEvents txtHeight0 As txtNumeric
    Friend WithEvents txtKieu_hd As TextBox
    Friend WithEvents txtKieu_lo As TextBox
    Friend WithEvents txtLength As txtNumeric
    Friend WithEvents txtLength0 As txtNumeric
    Friend WithEvents txtLoai_vt As TextBox
    Friend WithEvents txtMa_kh As TextBox
    Friend WithEvents txtMa_kh0 As TextBox
    Friend WithEvents txtMa_kh2 As TextBox
    Friend WithEvents txtMa_kho As TextBox
    Friend WithEvents txtMa_lo_trinh As TextBox
    Friend WithEvents txtMa_thue As TextBox
    Friend WithEvents txtMa_thue_nk As TextBox
    Friend WithEvents txtMa_vi_tri As TextBox
    Friend WithEvents txtMa_vt As TextBox
    Friend WithEvents txtMa_vt2 As TextBox
    Friend WithEvents txtNh_vt1 As TextBox
    Friend WithEvents txtNh_vt2 As TextBox
    Friend WithEvents txtNh_vt3 As TextBox
    Friend WithEvents txtNh_vt9 As TextBox
    Friend WithEvents txtNuoc_sx As TextBox
    Friend WithEvents txtOng_ba As TextBox
    Friend WithEvents txtPack_size As TextBox
    Friend WithEvents txtPacks As txtNumeric
    Friend WithEvents txtSl_lo_chuan As txtNumeric
    Friend WithEvents txtSl_max As txtNumeric
    Friend WithEvents txtSl_min As txtNumeric
    Friend WithEvents txtSo_ngay_bh As txtNumeric
    Friend WithEvents txtSo_ngay_dh As txtNumeric
    Friend WithEvents txtSo_ngay_sp As txtNumeric
    Friend WithEvents txtStatus As TextBox
    Friend WithEvents txtTen_vt As TextBox
    Friend WithEvents txtTen_vt2 As TextBox
    Friend WithEvents txtTg_th As txtNumeric
    Friend WithEvents txtTg_th2 As txtNumeric
    Friend WithEvents txtTk_ck As TextBox
    Friend WithEvents txtTk_cl_vt As TextBox
    Friend WithEvents txtTk_cpbh As TextBox
    Friend WithEvents txtTk_dl As TextBox
    Friend WithEvents txtTk_dt As TextBox
    Friend WithEvents txtTk_dtnb As TextBox
    Friend WithEvents txtTk_gv As TextBox
    Friend WithEvents txtTk_spdd As TextBox
    Friend WithEvents txtTk_tl As TextBox
    Friend WithEvents txtTk_vt As TextBox
    Friend WithEvents txtTon_at As txtNumeric
    Friend WithEvents txtVolume As txtNumeric
    Friend WithEvents txtVolume0 As txtNumeric
    Friend WithEvents txtVung_hd As txtNumeric
    Friend WithEvents txtWeight As txtNumeric
    Friend WithEvents txtWeight0 As txtNumeric
    Friend WithEvents txtWeight2 As txtNumeric
    Friend WithEvents txtWidth As txtNumeric
    Friend WithEvents txtWidth0 As txtNumeric
    Friend WithEvents txtXcolor As TextBox
    Friend WithEvents txtXsize As TextBox
    Friend WithEvents txtXstyle As TextBox
    Private components As IContainer
    Private dtCalcType As DataTable
    Private isCon As Boolean
    Private lblInfor As Label
    Private oLocation As dirkeylib
End Class

