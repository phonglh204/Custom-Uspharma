Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol
Imports libscontrol.voucherseachlib

Public Class frmFilter
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
        Me.ds = New DataSet
        Me.dvOrder = New DataView
        Me.flag = False
        Me.InitializeComponent()
    End Sub

    Private Sub cboReports_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboReports.SelectedIndexChanged
        If Not Information.IsNothing(DirMain.rpTable) Then
            Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(Me.cboReports.SelectedIndex), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        If reportformlib.CheckEmptyField(Me, Me.tabReports, DirMain.oVar) Then
            DirMain.strUnit = Strings.Trim(Me.txtMa_dvcs.Text)
            DirMain.dTo = Me.txtDTo.Value
            Reg.SetRegistryKey("DFDTo", Me.txtDTo.Value)
            Me.pnContent.Text = StringType.FromObject(DirMain.oVar.Item("m_process"))
            DirMain.strGroups = ""
            If (StringType.StrCmp(DirMain.fPrint.txtGroup1.Text, "0", False) <> 0) Then
                DirMain.strGroups = (DirMain.strGroups & ",nh_vt" & Strings.Trim(DirMain.fPrint.txtGroup1.Text))
            End If
            If (StringType.StrCmp(DirMain.fPrint.txtGroup2.Text, "0", False) <> 0) Then
                DirMain.strGroups = (DirMain.strGroups & ",nh_vt" & Strings.Trim(DirMain.fPrint.txtGroup2.Text))
            End If
            If (StringType.StrCmp(DirMain.fPrint.txtGroup3.Text, "0", False) <> 0) Then
                DirMain.strGroups = (DirMain.strGroups & ",nh_vt" & Strings.Trim(DirMain.fPrint.txtGroup3.Text))
            End If
            DirMain.strGroups = Strings.Mid(DirMain.strGroups, 2)
            If (StringType.StrCmp(Strings.Trim(DirMain.strGroups), "", False) = 0) Then
                DirMain.strGroups = Strings.Trim(StringType.FromObject(DirMain.fPrint.CbbGroup.SelectedValue))
            End If
            DirMain.ShowReport()
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
        End If
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Dim page As New TabPage
        Me.tabReports.TabPages.Add(page)
        reportformlib.AddFreeFields(DirMain.sysConn, page, 7)
        Me.tabReports.TabPages.Remove(page)
        reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
        Dim vouchersearchlibobj7 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj8 As New vouchersearchlibobj(Me.txtMa_kho, Me.lblTen_kho, DirMain.sysConn, DirMain.appConn, "dmkho", "ma_kho", "ten_kho", "Store", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtMa_vt, Me.lblTen_vt, DirMain.sysConn, DirMain.appConn, "Dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtTk_vt, Me.lblTen_Tk_vt, DirMain.sysConn, DirMain.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtNh_vt, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtNh_vt2, Me.lblTen_nh2, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup2", "loai_nh=2", True, Me.cmdCancel)
        Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtNh_vt3, Me.lblTen_nh3, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup3", "loai_nh=3", True, Me.cmdCancel)
        Dim vouchersearchlibobj6 As New vouchersearchlibobj(Me.txtLoai_vt, Me.lblTen_loai, DirMain.sysConn, DirMain.appConn, "Dmloaivt", "ma_loai_vt", "ten_loai_vt", "ItemType", "1=1", True, Me.cmdCancel)
        Dim obj3 As Object = New CharLib(Me.txtKho_gui_ban, "1, 2")
        Dim obj2 As Object = New CharLib(Me.txtLoai_bc, "1, 2")
        Dim oGroup1 As New CharLib(Me.txtGroup1, "0,1,2,3")
        Dim lib2 As New CharLib(Me.txtGroup2, "0,1,2,3")
        Dim lib3 As New CharLib(Me.txtGroup3, "0,1,2,3")
        Me.txtGroup1.Text = StringType.FromInteger(0)
        Me.txtGroup2.Text = StringType.FromInteger(0)
        Me.txtGroup3.Text = StringType.FromInteger(0)
        Me.CancelButton = Me.cmdCancel
        Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
        Dim document As New PrintDocument
        Me.pnContent.Text = document.PrinterSettings.PrinterName
        Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
        Me.txtKho_gui_ban.Text = "1"
        Me.txtLoai_bc.Text = "1"
        DirMain.oAdvFilter = New clsAdvFilter(Me, Me.TabAdv, Me.tabReports, DirMain.appConn, DirMain.sysConn, Me.pnContent, Me.cmdCancel)
        DirMain.oAdvFilter.AddAdvSelect(StringType.FromObject(DirMain.ReportRow.Item("cAdvtables")))
        DirMain.oAdvFilter.AddComboboxValue(Me.CbbGroup, DirMain.SysID, "002", (Me.ds), "Group")
        DirMain.oAdvFilter.InitGridOrder((grdOrder), DirMain.SysID, "001", (Me.ds), "Order")
        Me.tabReports.SelectedIndex = 0
        reportformlib.grdOrderDataview = Me.grdOrder.dvGrid
        reportformlib.grdSelectDataview = DirMain.oAdvFilter.GetDataview
        DirMain.oxInv = New xInv(Me.tabReports, Me.pnContent, DirMain.appConn, DirMain.sysConn)
        Me.txtMa_kho.Text = "KHO.NL"
        Me.txtMa_dvcs.Text = "KHO"
        Me.txtMa_dvcs.ReadOnly = True
    End Sub

    <DebuggerStepThrough()> _
Private Sub InitializeComponent()
        Me.txtMa_dvcs = New TextBox
        Me.lblMa_dvcs = New Label
        Me.lblTen_dvcs = New Label
        Me.cmdOk = New Button
        Me.cmdCancel = New Button
        Me.tabReports = New TabControl
        Me.tbgFilter = New TabPage
        Me.txtNh_vt3 = New TextBox
        Me.txtNh_vt2 = New TextBox
        Me.Label3 = New Label
        Me.txtTk_vt = New TextBox
        Me.lblTen_Tk_vt = New Label
        Me.Label8 = New Label
        Me.txtLoai_vt = New TextBox
        Me.lblTen_loai = New Label
        Me.cboReports = New ComboBox
        Me.Label5 = New Label
        Me.txtNh_vt = New TextBox
        Me.Label1 = New Label
        Me.txtMa_vt = New TextBox
        Me.lblTen_vt = New Label
        Me.lblMa_kho = New Label
        Me.txtMa_kho = New TextBox
        Me.lblTen_kho = New Label
        Me.txtDTo = New txtDate
        Me.lblDateFromTo = New Label
        Me.lblMau_bc = New Label
        Me.lblTitle = New Label
        Me.txtTitle = New TextBox
        Me.tbgOptions = New TabPage
        Me.txtGroup3 = New TextBox
        Me.txtGroup2 = New TextBox
        Me.txtGroup1 = New TextBox
        Me.lblTk = New Label
        Me.Label4 = New Label
        Me.Label6 = New Label
        Me.Label7 = New Label
        Me.Label9 = New Label
        Me.txtLoai_bc = New TextBox
        Me.txtKho_gui_ban = New TextBox
        Me.CbbGroup = New ComboBox
        Me.Label2 = New Label
        Me.TabAdv = New TabPage
        Me.tbgOrder = New TabPage
        Me.grdOrder = New clsgrid
        Me.lblTen_nh = New Label
        Me.lblTen_nh2 = New Label
        Me.lblTen_nh3 = New Label
        Me.tabReports.SuspendLayout()
        Me.tbgFilter.SuspendLayout()
        Me.tbgOptions.SuspendLayout()
        Me.tbgOrder.SuspendLayout()
        Me.grdOrder.BeginInit()
        Me.SuspendLayout()
        Me.txtMa_dvcs.CharacterCasing = CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New Point(160, 151)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.TabIndex = 8
        Me.txtMa_dvcs.Tag = "FCML"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New Point(20, 153)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New Size(36, 16)
        Me.lblMa_dvcs.TabIndex = 1
        Me.lblMa_dvcs.Tag = "L103"
        Me.lblMa_dvcs.Text = "Don vi"
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New Point(264, 153)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New Size(50, 16)
        Me.lblTen_dvcs.TabIndex = 7
        Me.lblTen_dvcs.Tag = "L002"
        Me.lblTen_dvcs.Text = "Ten dvcs"
        Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdOk.Location = New Point(3, 264)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Tag = "L001"
        Me.cmdOk.Text = "Nhan"
        Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdCancel.Location = New Point(79, 264)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Tag = "L002"
        Me.cmdCancel.Text = "Huy"
        Me.tabReports.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        Me.tabReports.Controls.Add(Me.tbgFilter)
        Me.tabReports.Controls.Add(Me.tbgOptions)
        Me.tabReports.Controls.Add(Me.TabAdv)
        Me.tabReports.Controls.Add(Me.tbgOrder)
        Me.tabReports.Location = New Point(-2, 0)
        Me.tabReports.Name = "tabReports"
        Me.tabReports.SelectedIndex = 0
        Me.tabReports.Size = New Size(609, 256)
        Me.tabReports.TabIndex = 0
        Me.tbgFilter.Controls.Add(Me.txtNh_vt3)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt2)
        Me.tbgFilter.Controls.Add(Me.Label3)
        Me.tbgFilter.Controls.Add(Me.txtTk_vt)
        Me.tbgFilter.Controls.Add(Me.lblTen_Tk_vt)
        Me.tbgFilter.Controls.Add(Me.Label8)
        Me.tbgFilter.Controls.Add(Me.txtLoai_vt)
        Me.tbgFilter.Controls.Add(Me.lblTen_loai)
        Me.tbgFilter.Controls.Add(Me.cboReports)
        Me.tbgFilter.Controls.Add(Me.Label5)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt)
        Me.tbgFilter.Controls.Add(Me.Label1)
        Me.tbgFilter.Controls.Add(Me.txtMa_vt)
        Me.tbgFilter.Controls.Add(Me.lblTen_vt)
        Me.tbgFilter.Controls.Add(Me.lblMa_kho)
        Me.tbgFilter.Controls.Add(Me.txtMa_kho)
        Me.tbgFilter.Controls.Add(Me.lblTen_kho)
        Me.tbgFilter.Controls.Add(Me.txtDTo)
        Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
        Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblMau_bc)
        Me.tbgFilter.Controls.Add(Me.lblTitle)
        Me.tbgFilter.Controls.Add(Me.txtTitle)
        Me.tbgFilter.Location = New Point(4, 22)
        Me.tbgFilter.Name = "tbgFilter"
        Me.tbgFilter.Size = New Size(601, 230)
        Me.tbgFilter.TabIndex = 0
        Me.tbgFilter.Tag = "L100"
        Me.tbgFilter.Text = "Dieu kien loc"
        Me.txtNh_vt3.CharacterCasing = CharacterCasing.Upper
        Me.txtNh_vt3.Location = New Point(368, 128)
        Me.txtNh_vt3.Name = "txtNh_vt3"
        Me.txtNh_vt3.TabIndex = 7
        Me.txtNh_vt3.Tag = "FCML"
        Me.txtNh_vt3.Text = "TXTNH_VT3"
        Me.txtNh_vt2.CharacterCasing = CharacterCasing.Upper
        Me.txtNh_vt2.Location = New Point(264, 128)
        Me.txtNh_vt2.Name = "txtNh_vt2"
        Me.txtNh_vt2.TabIndex = 6
        Me.txtNh_vt2.Tag = "FCML"
        Me.txtNh_vt2.Text = "TXTNH_VT2"
        Me.Label3.AutoSize = True
        Me.Label3.Location = New Point(20, 84)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New Size(47, 16)
        Me.Label3.TabIndex = 48
        Me.Label3.Tag = "L107"
        Me.Label3.Text = "Tk vat tu"
        Me.txtTk_vt.CharacterCasing = CharacterCasing.Upper
        Me.txtTk_vt.Location = New Point(160, 82)
        Me.txtTk_vt.Name = "txtTk_vt"
        Me.txtTk_vt.TabIndex = 3
        Me.txtTk_vt.Tag = "FCML"
        Me.txtTk_vt.Text = "TXTTK_VT"
        Me.lblTen_Tk_vt.AutoSize = True
        Me.lblTen_Tk_vt.Location = New Point(264, 84)
        Me.lblTen_Tk_vt.Name = "lblTen_Tk_vt"
        Me.lblTen_Tk_vt.Size = New Size(47, 16)
        Me.lblTen_Tk_vt.TabIndex = 49
        Me.lblTen_Tk_vt.Tag = "L016"
        Me.lblTen_Tk_vt.Text = "Tk vat tu"
        Me.Label8.AutoSize = True
        Me.Label8.Location = New Point(20, 107)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New Size(56, 16)
        Me.Label8.TabIndex = 46
        Me.Label8.Tag = "L108"
        Me.Label8.Text = "Loai vat tu"
        Me.txtLoai_vt.CharacterCasing = CharacterCasing.Upper
        Me.txtLoai_vt.Location = New Point(160, 105)
        Me.txtLoai_vt.Name = "txtLoai_vt"
        Me.txtLoai_vt.TabIndex = 4
        Me.txtLoai_vt.Tag = "FCML"
        Me.txtLoai_vt.Text = "TXTLOAI_VT"
        Me.lblTen_loai.AutoSize = True
        Me.lblTen_loai.Location = New Point(264, 107)
        Me.lblTen_loai.Name = "lblTen_loai"
        Me.lblTen_loai.Size = New Size(56, 16)
        Me.lblTen_loai.TabIndex = 47
        Me.lblTen_loai.Tag = "L016"
        Me.lblTen_loai.Text = "Loai vat tu"
        Me.cboReports.Location = New Point(160, 174)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New Size(300, 21)
        Me.cboReports.TabIndex = 9
        Me.cboReports.Text = "cboReports"
        Me.Label5.AutoSize = True
        Me.Label5.Location = New Point(20, 130)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New Size(65, 16)
        Me.Label5.TabIndex = 17
        Me.Label5.Tag = "L109"
        Me.Label5.Text = "Nhom vat tu"
        Me.txtNh_vt.CharacterCasing = CharacterCasing.Upper
        Me.txtNh_vt.Location = New Point(160, 128)
        Me.txtNh_vt.Name = "txtNh_vt"
        Me.txtNh_vt.TabIndex = 5
        Me.txtNh_vt.Tag = "FCML"
        Me.txtNh_vt.Text = "TXTNH_VT"
        Me.Label1.AutoSize = True
        Me.Label1.Location = New Point(20, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New Size(50, 16)
        Me.Label1.TabIndex = 14
        Me.Label1.Tag = "L106"
        Me.Label1.Text = "Ma vat tu"
        Me.txtMa_vt.CharacterCasing = CharacterCasing.Upper
        Me.txtMa_vt.Location = New Point(160, 59)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.TabIndex = 2
        Me.txtMa_vt.Tag = "FCML"
        Me.txtMa_vt.Text = "TXTMA_VT"
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New Point(264, 61)
        Me.lblTen_vt.Name = "lblTen_vt"
        Me.lblTen_vt.Size = New Size(54, 16)
        Me.lblTen_vt.TabIndex = 15
        Me.lblTen_vt.Tag = "L014"
        Me.lblTen_vt.Text = "Ten vat tu"
        Me.lblMa_kho.AutoSize = True
        Me.lblMa_kho.Location = New Point(20, 38)
        Me.lblMa_kho.Name = "lblMa_kho"
        Me.lblMa_kho.Size = New Size(41, 16)
        Me.lblMa_kho.TabIndex = 10
        Me.lblMa_kho.Tag = "L105"
        Me.lblMa_kho.Text = "Ma kho"
        Me.txtMa_kho.CharacterCasing = CharacterCasing.Upper
        Me.txtMa_kho.Location = New Point(160, 36)
        Me.txtMa_kho.Name = "txtMa_kho"
        Me.txtMa_kho.TabIndex = 1
        Me.txtMa_kho.Tag = "FCML"
        Me.txtMa_kho.Text = "TXTMA_KHO"
        Me.lblTen_kho.AutoSize = True
        Me.lblTen_kho.Location = New Point(264, 38)
        Me.lblTen_kho.Name = "lblTen_kho"
        Me.lblTen_kho.Size = New Size(45, 16)
        Me.lblTen_kho.TabIndex = 12
        Me.lblTen_kho.Tag = "L002"
        Me.lblTen_kho.Text = "Ten kho"
        Me.txtDTo.Location = New Point(160, 13)
        Me.txtDTo.MaxLength = 10
        Me.txtDTo.Name = "txtDTo"
        Me.txtDTo.TabIndex = 0
        Me.txtDTo.Tag = "NB"
        Me.txtDTo.Text = "  /  /    "
        Me.txtDTo.TextAlign = HorizontalAlignment.Right
        Me.txtDTo.Value = New DateTime(0)
        Me.lblDateFromTo.AutoSize = True
        Me.lblDateFromTo.Location = New Point(20, 15)
        Me.lblDateFromTo.Name = "lblDateFromTo"
        Me.lblDateFromTo.Size = New Size(53, 16)
        Me.lblDateFromTo.TabIndex = 0
        Me.lblDateFromTo.Tag = "L101"
        Me.lblDateFromTo.Text = "Den ngay"
        Me.lblMau_bc.AutoSize = True
        Me.lblMau_bc.Location = New Point(20, 176)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New Size(69, 16)
        Me.lblMau_bc.TabIndex = 2
        Me.lblMau_bc.Tag = "L102"
        Me.lblMau_bc.Text = "Mau bao cao"
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New Point(20, 200)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New Size(42, 16)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Tag = "L104"
        Me.lblTitle.Text = "Tieu de"
        Me.txtTitle.Location = New Point(160, 198)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New Size(300, 20)
        Me.txtTitle.TabIndex = 10
        Me.txtTitle.Tag = "NB"
        Me.txtTitle.Text = "txtTieu_de"
        Me.tbgOptions.Controls.Add(Me.txtGroup3)
        Me.tbgOptions.Controls.Add(Me.txtGroup2)
        Me.tbgOptions.Controls.Add(Me.txtGroup1)
        Me.tbgOptions.Controls.Add(Me.lblTk)
        Me.tbgOptions.Controls.Add(Me.Label4)
        Me.tbgOptions.Controls.Add(Me.Label6)
        Me.tbgOptions.Controls.Add(Me.Label7)
        Me.tbgOptions.Controls.Add(Me.Label9)
        Me.tbgOptions.Controls.Add(Me.txtLoai_bc)
        Me.tbgOptions.Controls.Add(Me.txtKho_gui_ban)
        Me.tbgOptions.Controls.Add(Me.CbbGroup)
        Me.tbgOptions.Controls.Add(Me.Label2)
        Me.tbgOptions.Location = New Point(4, 22)
        Me.tbgOptions.Name = "tbgOptions"
        Me.tbgOptions.Size = New Size(601, 230)
        Me.tbgOptions.TabIndex = 2
        Me.tbgOptions.Tag = "L200"
        Me.tbgOptions.Text = "Lua chon"
        Me.txtGroup3.CharacterCasing = CharacterCasing.Upper
        Me.txtGroup3.Enabled = False
        Me.txtGroup3.Location = New Point(344, 200)
        Me.txtGroup3.MaxLength = 1
        Me.txtGroup3.Name = "txtGroup3"
        Me.txtGroup3.Size = New Size(24, 20)
        Me.txtGroup3.TabIndex = 3
        Me.txtGroup3.Tag = "FC"
        Me.txtGroup3.Text = "TXTNO_CO"
        Me.txtGroup3.Visible = False
        Me.txtGroup2.CharacterCasing = CharacterCasing.Upper
        Me.txtGroup2.Enabled = False
        Me.txtGroup2.Location = New Point(320, 200)
        Me.txtGroup2.MaxLength = 1
        Me.txtGroup2.Name = "txtGroup2"
        Me.txtGroup2.Size = New Size(24, 20)
        Me.txtGroup2.TabIndex = 2
        Me.txtGroup2.Tag = "FC"
        Me.txtGroup2.Text = "TXTNO_CO"
        Me.txtGroup2.Visible = False
        Me.txtGroup1.CharacterCasing = CharacterCasing.Upper
        Me.txtGroup1.Enabled = False
        Me.txtGroup1.Location = New Point(288, 200)
        Me.txtGroup1.MaxLength = 1
        Me.txtGroup1.Name = "txtGroup1"
        Me.txtGroup1.Size = New Size(24, 20)
        Me.txtGroup1.TabIndex = 1
        Me.txtGroup1.Tag = "FC"
        Me.txtGroup1.Text = "TXTNO_CO"
        Me.txtGroup1.Visible = False
        Me.lblTk.AutoSize = True
        Me.lblTk.Location = New Point(152, 200)
        Me.lblTk.Name = "lblTk"
        Me.lblTk.Size = New Size(135, 16)
        Me.lblTk.TabIndex = 151
        Me.lblTk.Tag = "LA01"
        Me.lblTk.Text = "Thu tu sap xep theo nhom"
        Me.lblTk.Visible = False
        Me.Label4.AutoSize = True
        Me.Label4.Location = New Point(20, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New Size(69, 16)
        Me.Label4.TabIndex = 139
        Me.Label4.Tag = "L204"
        Me.Label4.Text = "Loai bao cao"
        Me.Label6.AutoSize = True
        Me.Label6.Location = New Point(188, 38)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New Size(226, 16)
        Me.Label6.TabIndex = 140
        Me.Label6.Tag = "L205"
        Me.Label6.Text = "1-Bao cao hang doc, 2-Bao cao hang ngang"
        Me.Label7.AutoSize = True
        Me.Label7.Location = New Point(20, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New Size(93, 16)
        Me.Label7.TabIndex = 137
        Me.Label7.Tag = "L202"
        Me.Label7.Text = "Kho hang gui ban"
        Me.Label9.AutoSize = True
        Me.Label9.Location = New Point(188, 15)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New Size(312, 16)
        Me.Label9.TabIndex = 138
        Me.Label9.Tag = "L203"
        Me.Label9.Text = "1-Chi tiet theo kho hang gui ban, 2-Gop cac kho hang gui ban"
        Me.txtLoai_bc.Location = New Point(160, 36)
        Me.txtLoai_bc.MaxLength = 1
        Me.txtLoai_bc.Name = "txtLoai_bc"
        Me.txtLoai_bc.Size = New Size(24, 20)
        Me.txtLoai_bc.TabIndex = 6
        Me.txtLoai_bc.Text = "txtLoai_bc"
        Me.txtLoai_bc.TextAlign = HorizontalAlignment.Right
        Me.txtKho_gui_ban.Location = New Point(160, 13)
        Me.txtKho_gui_ban.MaxLength = 1
        Me.txtKho_gui_ban.Name = "txtKho_gui_ban"
        Me.txtKho_gui_ban.Size = New Size(24, 20)
        Me.txtKho_gui_ban.TabIndex = 4
        Me.txtKho_gui_ban.Text = "txtKho_gui_ban"
        Me.txtKho_gui_ban.TextAlign = HorizontalAlignment.Right
        Me.CbbGroup.DropDownStyle = ComboBoxStyle.DropDownList
        Me.CbbGroup.Enabled = False
        Me.CbbGroup.Location = New Point(288, 176)
        Me.CbbGroup.Name = "CbbGroup"
        Me.CbbGroup.Size = New Size(300, 21)
        Me.CbbGroup.TabIndex = 0
        Me.CbbGroup.Visible = False
        Me.Label2.AutoSize = True
        Me.Label2.Location = New Point(152, 176)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New Size(59, 16)
        Me.Label2.TabIndex = 128
        Me.Label2.Tag = "L201"
        Me.Label2.Text = "Nhom theo"
        Me.Label2.Visible = False
        Me.TabAdv.Location = New Point(4, 22)
        Me.TabAdv.Name = "TabAdv"
        Me.TabAdv.Size = New Size(601, 230)
        Me.TabAdv.TabIndex = 1
        Me.TabAdv.Tag = "L400"
        Me.TabAdv.Text = "Advance filter"
        Me.tbgOrder.Controls.Add(Me.grdOrder)
        Me.tbgOrder.Location = New Point(4, 22)
        Me.tbgOrder.Name = "tbgOrder"
        Me.tbgOrder.Size = New Size(601, 230)
        Me.tbgOrder.TabIndex = 3
        Me.tbgOrder.Tag = "L300"
        Me.tbgOrder.Text = "Thu tu sap xep"
        Me.grdOrder.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        Me.grdOrder.CaptionVisible = False
        Me.grdOrder.DataMember = ""
        Me.grdOrder.HeaderForeColor = SystemColors.ControlText
        Me.grdOrder.Location = New Point(0, 0)
        Me.grdOrder.Name = "grdOrder"
        Me.grdOrder.Size = New Size(601, 230)
        Me.grdOrder.TabIndex = 0
        Me.lblTen_nh.AutoSize = True
        Me.lblTen_nh.Location = New Point(224, 264)
        Me.lblTen_nh.Name = "lblTen_nh"
        Me.lblTen_nh.Size = New Size(43, 16)
        Me.lblTen_nh.TabIndex = 18
        Me.lblTen_nh.Tag = "L015"
        Me.lblTen_nh.Text = "Ten_nh"
        Me.lblTen_nh.Visible = False
        Me.lblTen_nh2.AutoSize = True
        Me.lblTen_nh2.Location = New Point(240, 272)
        Me.lblTen_nh2.Name = "lblTen_nh2"
        Me.lblTen_nh2.Size = New Size(43, 16)
        Me.lblTen_nh2.TabIndex = 56
        Me.lblTen_nh2.Tag = "L015"
        Me.lblTen_nh2.Text = "Ten_nh"
        Me.lblTen_nh2.Visible = False
        Me.lblTen_nh3.AutoSize = True
        Me.lblTen_nh3.Location = New Point(272, 280)
        Me.lblTen_nh3.Name = "lblTen_nh3"
        Me.lblTen_nh3.Size = New Size(43, 16)
        Me.lblTen_nh3.TabIndex = 57
        Me.lblTen_nh3.Tag = "L015"
        Me.lblTen_nh3.Text = "Ten_nh"
        Me.lblTen_nh3.Visible = False
        Me.AutoScaleBaseSize = New Size(5, 13)
        Me.ClientSize = New Size(608, 317)
        Me.Controls.Add(Me.tabReports)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.lblTen_nh3)
        Me.Controls.Add(Me.lblTen_nh2)
        Me.Controls.Add(Me.lblTen_nh)
        Me.Name = "frmFilter"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "frmFilter"
        Me.tabReports.ResumeLayout(False)
        Me.tbgFilter.ResumeLayout(False)
        Me.tbgOptions.ResumeLayout(False)
        Me.tbgOrder.ResumeLayout(False)
        Me.grdOrder.EndInit()
        Me.ResumeLayout(False)
    End Sub

    Private Sub txtGroup1_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup1.Enter
        Me.flag = False
    End Sub

    Private Sub txtGroup1_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup1.Validated
        Me.intGroup1 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup1.Text)))
        Me.intGroup2 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup2.Text)))
        Me.intGroup3 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup3.Text)))
        If (((Me.intGroup2 + Me.intGroup3) <> 0) And (((Me.intGroup1 = Me.intGroup2) Or (Me.intGroup1 = Me.intGroup3)) Or (Me.intGroup1 = 0))) Then
            DirMain.fPrint.txtGroup1.Focus()
        End If
        If (Me.intGroup1 = 0) Then
            DirMain.fPrint.CbbGroup.Enabled = True
        Else
            DirMain.fPrint.CbbGroup.Enabled = False
        End If
    End Sub

    Private Sub txtGroup2_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup2.Enter
        If (StringType.StrCmp(DirMain.fPrint.txtGroup1.Text, "0", False) = 0) Then
            If Me.flag Then
                DirMain.fPrint.txtGroup1.Focus()
            Else
                DirMain.fPrint.txtKho_gui_ban.Focus()
            End If
        End If
        If (StringType.StrCmp(DirMain.fPrint.txtGroup1.Text, "0", False) <> 0) Then
            Me.flag = False
        End If
    End Sub

    Private Sub txtGroup2_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup2.Validated
        Me.intGroup1 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup1.Text)))
        Me.intGroup2 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup2.Text)))
        Me.intGroup3 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup3.Text)))
        If (((Me.intGroup2 + Me.intGroup3) <> 0) And (((Me.intGroup2 = Me.intGroup1) Or (Me.intGroup2 = Me.intGroup3)) Or (Me.intGroup2 = 0))) Then
            DirMain.fPrint.txtGroup2.Focus()
        End If
    End Sub

    Private Sub txtGroup3_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup3.Enter
        If (StringType.StrCmp(DirMain.fPrint.txtGroup2.Text, "0", False) = 0) Then
            If Me.flag Then
                DirMain.fPrint.txtGroup2.Focus()
            Else
                DirMain.fPrint.txtKho_gui_ban.Focus()
            End If
        End If
        If (StringType.StrCmp(DirMain.fPrint.txtGroup1.Text, "0", False) <> 0) Then
            Me.flag = False
        End If
    End Sub

    Private Sub txtGroup3_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup3.Validated
        Me.intGroup1 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup1.Text)))
        Me.intGroup2 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup2.Text)))
        Me.intGroup3 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup3.Text)))
        If ((Me.intGroup3 <> 0) And ((Me.intGroup3 = Me.intGroup2) Or (Me.intGroup3 = Me.intGroup1))) Then
            DirMain.fPrint.txtGroup3.Focus()
        End If
    End Sub

    Private Sub txtKho_gui_ban_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtKho_gui_ban.Validated
        Me.flag = True
    End Sub


    ' Properties
    Friend WithEvents CbbGroup As ComboBox
    Friend WithEvents cboReports As ComboBox
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grdOrder As clsgrid
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents lblDateFromTo As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMa_kho As Label
    Friend WithEvents lblMau_bc As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_kho As Label
    Friend WithEvents lblTen_loai As Label
    Friend WithEvents lblTen_nh As Label
    Friend WithEvents lblTen_nh2 As Label
    Friend WithEvents lblTen_nh3 As Label
    Friend WithEvents lblTen_Tk_vt As Label
    Friend WithEvents lblTen_vt As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents lblTk As Label
    Friend WithEvents TabAdv As TabPage
    Friend WithEvents tabReports As TabControl
    Friend WithEvents tbgFilter As TabPage
    Friend WithEvents tbgOptions As TabPage
    Friend WithEvents tbgOrder As TabPage
    Friend WithEvents txtDTo As txtDate
    Friend WithEvents txtGroup1 As TextBox
    Friend WithEvents txtGroup2 As TextBox
    Friend WithEvents txtGroup3 As TextBox
    Friend WithEvents txtKho_gui_ban As TextBox
    Friend WithEvents txtLoai_bc As TextBox
    Friend WithEvents txtLoai_vt As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_kho As TextBox
    Friend WithEvents txtMa_vt As TextBox
    Friend WithEvents txtNh_vt As TextBox
    Friend WithEvents txtNh_vt2 As TextBox
    Friend WithEvents txtNh_vt3 As TextBox
    Friend WithEvents txtTitle As TextBox
    Friend WithEvents txtTk_vt As TextBox

    Private components As IContainer
    Public ds As DataSet
    Private dvOrder As DataView
    Private flag As Boolean
    Private intGroup1 As Integer
    Private intGroup2 As Integer
    Private intGroup3 As Integer
    Public pnContent As StatusBarPanel
End Class

