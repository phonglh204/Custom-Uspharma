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

Namespace incd1lex
    Public Class frmFilter
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
            Me.ds = New DataSet
            Me.dvOrder = New DataView
            Me.InitializeComponent
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
                DirMain.dFrom = Me.txtDFrom.Value
                DirMain.dTo = Me.txtDTo.Value
                Reg.SetRegistryKey("DFDFrom", Me.txtDFrom.Value)
                Reg.SetRegistryKey("DFDTo", Me.txtDTo.Value)
                Me.pnContent.Text = StringType.FromObject(DirMain.oVar.Item("m_process"))
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
            reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
            Dim oMa_dvcs As New DirLib(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj As New DirLib(Me.txtMa_vt, Me.lblTen_vt, DirMain.sysConn, DirMain.appConn, "Dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtNh_vt, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtNh_vt2, Me.lblTen_nh2, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=2", True, Me.cmdCancel)
            Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtNh_vt3, Me.lblTen_nh3, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=3", True, Me.cmdCancel)
            Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtLoai_vt, Me.lblTen_loai, DirMain.sysConn, DirMain.appConn, "Dmloaivt", "ma_loai_vt", "ten_loai_vt", "ItemType", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj6 As New vouchersearchlibobj(Me.txtMa_lo, Me.lblTen_lo, DirMain.sysConn, DirMain.appConn, "dmlo", "ma_lo", "ten_lo", "Lot", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj7 As New vouchersearchlibobj(Me.txtMa_kho, Me.lblTen_kho, DirMain.sysConn, DirMain.appConn, "dmkho", "ma_kho", "ten_kho", "Site", "1=1", True, Me.cmdCancel)
            Me.CancelButton = Me.cmdCancel
            Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
            Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
            Me.txtDFrom.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
            Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
            DirMain.oAdvFilter = New clsAdvFilter(Me, Me.TabAdv, Me.tabReports, DirMain.appConn, DirMain.sysConn, Me.pnContent, Me.cmdCancel)
            DirMain.oAdvFilter.AddAdvSelect(StringType.FromObject(DirMain.ReportRow.Item("cAdvtables")))
            DirMain.oAdvFilter.AddComboboxValue(Me.CbbGroup, DirMain.SysID, "002", (Me.ds), "Group")
            DirMain.oAdvFilter.AddComboboxValue(Me.CbbTinh_dc, DirMain.SysID, "003", (Me.ds), "Transfer")
            DirMain.oAdvFilter.AddComboboxValue(Me.CbbPrintAmtTotal, DirMain.SysID, "005", (Me.ds), "isTotalPrintQty")
            DirMain.oAdvFilter.AddComboboxValue(Me.cbbQtycol, DirMain.SysID, "006", (Me.ds), "PrintType")
            Dim grdOrder As clsgrid = Me.grdOrder
            DirMain.oAdvFilter.InitGridOrder((grdOrder), DirMain.SysID, "001", (Me.ds), "Order")
            Me.grdOrder = grdOrder
            Me.tabReports.SelectedIndex = 0
            reportformlib.grdOrderDataview = Me.grdOrder.dvGrid
            reportformlib.grdSelectDataview = DirMain.oAdvFilter.GetDataview
            DirMain.oxInv = New xInv(Me.tabReports, Me.pnContent, DirMain.appConn, DirMain.sysConn)
            Me.txtMa_dvcs.Text = "KHO"
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
            Me.lblTen_kho = New Label
            Me.Label7 = New Label
            Me.txtMa_kho = New TextBox
            Me.txtNh_vt3 = New TextBox
            Me.txtNh_vt2 = New TextBox
            Me.cboReports = New ComboBox
            Me.Label8 = New Label
            Me.txtLoai_vt = New TextBox
            Me.lblTen_loai = New Label
            Me.Label5 = New Label
            Me.txtNh_vt = New TextBox
            Me.Label1 = New Label
            Me.txtMa_vt = New TextBox
            Me.lblTen_vt = New Label
            Me.lblMa_kho = New Label
            Me.txtMa_lo = New TextBox
            Me.lblTen_lo = New Label
            Me.txtDTo = New txtDate
            Me.txtDFrom = New txtDate
            Me.lblDateFromTo = New Label
            Me.lblMau_bc = New Label
            Me.lblTitle = New Label
            Me.txtTitle = New TextBox
            Me.tbgOptions = New TabPage
            Me.cbbQtycol = New ComboBox
            Me.Label4 = New Label
            Me.CbbTinh_dc = New ComboBox
            Me.CbbPrintAmtTotal = New ComboBox
            Me.CbbGroup = New ComboBox
            Me.Label3 = New Label
            Me.Label2 = New Label
            Me.Label6 = New Label
            Me.TabAdv = New TabPage
            Me.tbgOrder = New TabPage
            Me.grdOrder = New clsgrid
            Me.lblTen_nh3 = New Label
            Me.lblTen_nh2 = New Label
            Me.lblTen_nh = New Label
            Me.tabReports.SuspendLayout()
            Me.tbgFilter.SuspendLayout()
            Me.tbgOptions.SuspendLayout()
            Me.tbgOrder.SuspendLayout()
            Me.grdOrder.BeginInit()
            Me.SuspendLayout()
            Me.txtMa_dvcs.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New Point(160, 151)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.TabIndex = 9
            Me.txtMa_dvcs.Tag = "FCML"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New Point(20, 153)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New Size(36, 16)
            Me.lblMa_dvcs.TabIndex = 1
            Me.lblMa_dvcs.Tag = "L004"
            Me.lblMa_dvcs.Text = "Don vi"
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New Point(264, 153)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New Size(50, 16)
            Me.lblTen_dvcs.TabIndex = 7
            Me.lblTen_dvcs.Tag = "L002"
            Me.lblTen_dvcs.Text = "Ten dvcs"
            Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdOk.Location = New Point(3, 265)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 0
            Me.cmdOk.Tag = "L001"
            Me.cmdOk.Text = "Nhan"
            Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdCancel.Location = New Point(79, 265)
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
            Me.tabReports.Size = New Size(609, 257)
            Me.tabReports.TabIndex = 0
            Me.tbgFilter.Controls.Add(Me.lblTen_kho)
            Me.tbgFilter.Controls.Add(Me.Label7)
            Me.tbgFilter.Controls.Add(Me.txtMa_kho)
            Me.tbgFilter.Controls.Add(Me.txtNh_vt3)
            Me.tbgFilter.Controls.Add(Me.txtNh_vt2)
            Me.tbgFilter.Controls.Add(Me.cboReports)
            Me.tbgFilter.Controls.Add(Me.Label8)
            Me.tbgFilter.Controls.Add(Me.txtLoai_vt)
            Me.tbgFilter.Controls.Add(Me.lblTen_loai)
            Me.tbgFilter.Controls.Add(Me.Label5)
            Me.tbgFilter.Controls.Add(Me.txtNh_vt)
            Me.tbgFilter.Controls.Add(Me.Label1)
            Me.tbgFilter.Controls.Add(Me.txtMa_vt)
            Me.tbgFilter.Controls.Add(Me.lblTen_vt)
            Me.tbgFilter.Controls.Add(Me.lblMa_kho)
            Me.tbgFilter.Controls.Add(Me.txtMa_lo)
            Me.tbgFilter.Controls.Add(Me.lblTen_lo)
            Me.tbgFilter.Controls.Add(Me.txtDTo)
            Me.tbgFilter.Controls.Add(Me.txtDFrom)
            Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
            Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblMau_bc)
            Me.tbgFilter.Controls.Add(Me.lblTitle)
            Me.tbgFilter.Controls.Add(Me.txtTitle)
            Me.tbgFilter.Location = New Point(4, 22)
            Me.tbgFilter.Name = "tbgFilter"
            Me.tbgFilter.Size = New Size(601, 231)
            Me.tbgFilter.TabIndex = 0
            Me.tbgFilter.Tag = "L100"
            Me.tbgFilter.Text = "Dieu kien loc"
            Me.lblTen_kho.AutoSize = True
            Me.lblTen_kho.Location = New Point(264, 38)
            Me.lblTen_kho.Name = "lblTen_kho"
            Me.lblTen_kho.Size = New Size(32, 16)
            Me.lblTen_kho.TabIndex = 24
            Me.lblTen_kho.Tag = "L005"
            Me.lblTen_kho.Text = "Ma lo"
            Me.Label7.AutoSize = True
            Me.Label7.Location = New Point(20, 38)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New Size(41, 16)
            Me.Label7.TabIndex = 23
            Me.Label7.Tag = "L016"
            Me.Label7.Text = "Ma kho"
            Me.txtMa_kho.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_kho.Location = New Point(160, 36)
            Me.txtMa_kho.Name = "txtMa_kho"
            Me.txtMa_kho.TabIndex = 2
            Me.txtMa_kho.Tag = "FCML"
            Me.txtMa_kho.Text = "TXTMA_KHO"
            Me.txtNh_vt3.CharacterCasing = CharacterCasing.Upper
            Me.txtNh_vt3.Location = New Point(368, 82)
            Me.txtNh_vt3.Name = "txtNh_vt3"
            Me.txtNh_vt3.TabIndex = 6
            Me.txtNh_vt3.Tag = "FCML"
            Me.txtNh_vt3.Text = "TXTNH_VT3"
            Me.txtNh_vt2.CharacterCasing = CharacterCasing.Upper
            Me.txtNh_vt2.Location = New Point(264, 82)
            Me.txtNh_vt2.Name = "txtNh_vt2"
            Me.txtNh_vt2.TabIndex = 5
            Me.txtNh_vt2.Tag = "FCML"
            Me.txtNh_vt2.Text = "TXTNH_VT2"
            Me.cboReports.Location = New Point(160, 174)
            Me.cboReports.Name = "cboReports"
            Me.cboReports.Size = New Size(300, 21)
            Me.cboReports.TabIndex = 10
            Me.cboReports.Text = "cboReports"
            Me.Label8.AutoSize = True
            Me.Label8.Location = New Point(20, 107)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New Size(56, 16)
            Me.Label8.TabIndex = 20
            Me.Label8.Tag = "L013"
            Me.Label8.Text = "Loai vat tu"
            Me.txtLoai_vt.CharacterCasing = CharacterCasing.Upper
            Me.txtLoai_vt.Location = New Point(160, 105)
            Me.txtLoai_vt.Name = "txtLoai_vt"
            Me.txtLoai_vt.TabIndex = 7
            Me.txtLoai_vt.Tag = "FCML"
            Me.txtLoai_vt.Text = "TXTLOAI_VT"
            Me.lblTen_loai.AutoSize = True
            Me.lblTen_loai.Location = New Point(264, 107)
            Me.lblTen_loai.Name = "lblTen_loai"
            Me.lblTen_loai.Size = New Size(56, 16)
            Me.lblTen_loai.TabIndex = 21
            Me.lblTen_loai.Tag = "L016"
            Me.lblTen_loai.Text = "Loai vat tu"
            Me.Label5.AutoSize = True
            Me.Label5.Location = New Point(20, 84)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New Size(65, 16)
            Me.Label5.TabIndex = 17
            Me.Label5.Tag = "L012"
            Me.Label5.Text = "Nhom vat tu"
            Me.txtNh_vt.CharacterCasing = CharacterCasing.Upper
            Me.txtNh_vt.Location = New Point(160, 82)
            Me.txtNh_vt.Name = "txtNh_vt"
            Me.txtNh_vt.TabIndex = 4
            Me.txtNh_vt.Tag = "FCML"
            Me.txtNh_vt.Text = "TXTNH_VT"
            Me.Label1.AutoSize = True
            Me.Label1.Location = New Point(20, 61)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New Size(50, 16)
            Me.Label1.TabIndex = 14
            Me.Label1.Tag = "L011"
            Me.Label1.Text = "Ma vat tu"
            Me.txtMa_vt.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_vt.Location = New Point(160, 59)
            Me.txtMa_vt.Name = "txtMa_vt"
            Me.txtMa_vt.TabIndex = 3
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
            Me.lblMa_kho.Location = New Point(20, 130)
            Me.lblMa_kho.Name = "lblMa_kho"
            Me.lblMa_kho.Size = New Size(32, 16)
            Me.lblMa_kho.TabIndex = 10
            Me.lblMa_kho.Tag = "L005"
            Me.lblMa_kho.Text = "Ma lo"
            Me.txtMa_lo.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_lo.Location = New Point(160, 128)
            Me.txtMa_lo.Name = "txtMa_lo"
            Me.txtMa_lo.TabIndex = 8
            Me.txtMa_lo.Tag = "FCML"
            Me.txtMa_lo.Text = "TXTMA_LO"
            Me.lblTen_lo.AutoSize = True
            Me.lblTen_lo.Location = New Point(264, 130)
            Me.lblTen_lo.Name = "lblTen_lo"
            Me.lblTen_lo.Size = New Size(36, 16)
            Me.lblTen_lo.TabIndex = 12
            Me.lblTen_lo.Tag = "L002"
            Me.lblTen_lo.Text = "Ten lo"
            Me.txtDTo.Location = New Point(264, 13)
            Me.txtDTo.MaxLength = 10
            Me.txtDTo.Name = "txtDTo"
            Me.txtDTo.TabIndex = 1
            Me.txtDTo.Tag = "NB"
            Me.txtDTo.Text = "  /  /    "
            Me.txtDTo.TextAlign = HorizontalAlignment.Right
            Me.txtDTo.Value = New DateTime(0)
            Me.txtDFrom.Location = New Point(160, 13)
            Me.txtDFrom.MaxLength = 10
            Me.txtDFrom.Name = "txtDFrom"
            Me.txtDFrom.TabIndex = 0
            Me.txtDFrom.Tag = "NB"
            Me.txtDFrom.Text = "  /  /    "
            Me.txtDFrom.TextAlign = HorizontalAlignment.Right
            Me.txtDFrom.Value = New DateTime(0)
            Me.lblDateFromTo.AutoSize = True
            Me.lblDateFromTo.Location = New Point(20, 15)
            Me.lblDateFromTo.Name = "lblDateFromTo"
            Me.lblDateFromTo.Size = New Size(67, 16)
            Me.lblDateFromTo.TabIndex = 0
            Me.lblDateFromTo.Tag = "L003"
            Me.lblDateFromTo.Text = "Tu/den ngay"
            Me.lblMau_bc.AutoSize = True
            Me.lblMau_bc.Location = New Point(20, 176)
            Me.lblMau_bc.Name = "lblMau_bc"
            Me.lblMau_bc.Size = New Size(69, 16)
            Me.lblMau_bc.TabIndex = 2
            Me.lblMau_bc.Tag = "L006"
            Me.lblMau_bc.Text = "Mau bao cao"
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New Point(20, 200)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New Size(42, 16)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Tag = "L007"
            Me.lblTitle.Text = "Tieu de"
            Me.txtTitle.Location = New Point(160, 198)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New Size(300, 20)
            Me.txtTitle.TabIndex = 11
            Me.txtTitle.Tag = "NB"
            Me.txtTitle.Text = "txtTieu_de"
            Me.tbgOptions.Controls.Add(Me.cbbQtycol)
            Me.tbgOptions.Controls.Add(Me.Label4)
            Me.tbgOptions.Controls.Add(Me.CbbTinh_dc)
            Me.tbgOptions.Controls.Add(Me.CbbPrintAmtTotal)
            Me.tbgOptions.Controls.Add(Me.CbbGroup)
            Me.tbgOptions.Controls.Add(Me.Label3)
            Me.tbgOptions.Controls.Add(Me.Label2)
            Me.tbgOptions.Controls.Add(Me.Label6)
            Me.tbgOptions.Location = New Point(4, 22)
            Me.tbgOptions.Name = "tbgOptions"
            Me.tbgOptions.Size = New Size(601, 231)
            Me.tbgOptions.TabIndex = 2
            Me.tbgOptions.Tag = "L200"
            Me.tbgOptions.Text = "Lua chon"
            Me.cbbQtycol.DropDownStyle = ComboBoxStyle.DropDownList
            Me.cbbQtycol.Location = New Point(160, 61)
            Me.cbbQtycol.Name = "cbbQtycol"
            Me.cbbQtycol.Size = New Size(300, 21)
            Me.cbbQtycol.TabIndex = 3
            Me.Label4.AutoSize = True
            Me.Label4.Location = New Point(8, 63)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New Size(99, 16)
            Me.Label4.TabIndex = 137
            Me.Label4.Tag = "L204"
            Me.Label4.Text = "In cac vat tu ton kh"
            Me.CbbTinh_dc.DropDownStyle = ComboBoxStyle.DropDownList
            Me.CbbTinh_dc.Location = New Point(160, 37)
            Me.CbbTinh_dc.Name = "CbbTinh_dc"
            Me.CbbTinh_dc.Size = New Size(300, 21)
            Me.CbbTinh_dc.TabIndex = 2
            Me.CbbPrintAmtTotal.DropDownStyle = ComboBoxStyle.DropDownList
            Me.CbbPrintAmtTotal.Location = New Point(160, 85)
            Me.CbbPrintAmtTotal.Name = "CbbPrintAmtTotal"
            Me.CbbPrintAmtTotal.Size = New Size(300, 21)
            Me.CbbPrintAmtTotal.TabIndex = 4
            Me.CbbGroup.DropDownStyle = ComboBoxStyle.DropDownList
            Me.CbbGroup.Location = New Point(160, 13)
            Me.CbbGroup.Name = "CbbGroup"
            Me.CbbGroup.Size = New Size(300, 21)
            Me.CbbGroup.TabIndex = 1
            Me.Label3.AutoSize = True
            Me.Label3.Location = New Point(8, 39)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New Size(139, 16)
            Me.Label3.TabIndex = 130
            Me.Label3.Tag = "L203"
            Me.Label3.Text = "Tinh phat sinh dieu chuyen"
            Me.Label2.AutoSize = True
            Me.Label2.Location = New Point(8, 15)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New Size(59, 16)
            Me.Label2.TabIndex = 128
            Me.Label2.Tag = "L201"
            Me.Label2.Text = "Nhom theo"
            Me.Label6.AutoSize = True
            Me.Label6.Location = New Point(8, 87)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New Size(84, 16)
            Me.Label6.TabIndex = 125
            Me.Label6.Tag = "L202"
            Me.Label6.Text = "In tong so luong"
            Me.TabAdv.Location = New Point(4, 22)
            Me.TabAdv.Name = "TabAdv"
            Me.TabAdv.Size = New Size(601, 231)
            Me.TabAdv.TabIndex = 1
            Me.TabAdv.Tag = "L400"
            Me.TabAdv.Text = "Advance filter"
            Me.tbgOrder.Controls.Add(Me.grdOrder)
            Me.tbgOrder.Location = New Point(4, 22)
            Me.tbgOrder.Name = "tbgOrder"
            Me.tbgOrder.Size = New Size(601, 231)
            Me.tbgOrder.TabIndex = 3
            Me.tbgOrder.Tag = "L300"
            Me.tbgOrder.Text = "Thu tu sap xep"
            Me.grdOrder.Cell_EnableRaisingEvents = False
            Me.grdOrder.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.grdOrder.CaptionVisible = False
            Me.grdOrder.DataMember = ""
            Me.grdOrder.HeaderForeColor = SystemColors.ControlText
            Me.grdOrder.Location = New Point(0, 0)
            Me.grdOrder.Name = "grdOrder"
            Me.grdOrder.Size = New Size(601, 231)
            Me.grdOrder.TabIndex = 0
            Me.lblTen_nh3.AutoSize = True
            Me.lblTen_nh3.Location = New Point(488, 312)
            Me.lblTen_nh3.Name = "lblTen_nh3"
            Me.lblTen_nh3.Size = New Size(43, 16)
            Me.lblTen_nh3.TabIndex = 64
            Me.lblTen_nh3.Tag = "L015"
            Me.lblTen_nh3.Text = "Ten_nh"
            Me.lblTen_nh3.Visible = False
            Me.lblTen_nh2.AutoSize = True
            Me.lblTen_nh2.Location = New Point(440, 312)
            Me.lblTen_nh2.Name = "lblTen_nh2"
            Me.lblTen_nh2.Size = New Size(43, 16)
            Me.lblTen_nh2.TabIndex = 63
            Me.lblTen_nh2.Tag = "L015"
            Me.lblTen_nh2.Text = "Ten_nh"
            Me.lblTen_nh2.Visible = False
            Me.lblTen_nh.AutoSize = True
            Me.lblTen_nh.Location = New Point(544, 312)
            Me.lblTen_nh.Name = "lblTen_nh"
            Me.lblTen_nh.Size = New Size(43, 16)
            Me.lblTen_nh.TabIndex = 62
            Me.lblTen_nh.Tag = "L015"
            Me.lblTen_nh.Text = "Ten_nh"
            Me.lblTen_nh.Visible = False
            Me.AutoScaleBaseSize = New Size(5, 13)
            Me.ClientSize = New Size(608, 318)
            Me.Controls.Add(Me.lblTen_nh3)
            Me.Controls.Add(Me.lblTen_nh2)
            Me.Controls.Add(Me.lblTen_nh)
            Me.Controls.Add(Me.tabReports)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
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


        ' Properties
        Friend WithEvents CbbGroup As ComboBox
        Friend WithEvents CbbPrintAmtTotal As ComboBox
        Friend WithEvents cbbQtycol As ComboBox
        Friend WithEvents CbbTinh_dc As ComboBox
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
        Friend WithEvents lblDateFromTo As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMa_kho As Label
        Friend WithEvents lblMau_bc As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTen_kho As Label
        Friend WithEvents lblTen_lo As Label
        Friend WithEvents lblTen_loai As Label
        Friend WithEvents lblTen_nh As Label
        Friend WithEvents lblTen_nh2 As Label
        Friend WithEvents lblTen_nh3 As Label
        Friend WithEvents lblTen_vt As Label
        Friend WithEvents lblTitle As Label
        Friend WithEvents TabAdv As TabPage
        Friend WithEvents tabReports As TabControl
        Friend WithEvents tbgFilter As TabPage
        Friend WithEvents tbgOptions As TabPage
        Friend WithEvents tbgOrder As TabPage
        Friend WithEvents txtDFrom As txtDate
        Friend WithEvents txtDTo As txtDate
        Friend WithEvents txtLoai_vt As TextBox
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtMa_kho As TextBox
        Friend WithEvents txtMa_lo As TextBox
        Friend WithEvents txtMa_vt As TextBox
        Friend WithEvents txtNh_vt As TextBox
        Friend WithEvents txtNh_vt2 As TextBox
        Friend WithEvents txtNh_vt3 As TextBox
        Friend WithEvents txtTitle As TextBox

        Private components As IContainer
        Public ds As DataSet
        Private dvOrder As DataView
        Public pnContent As StatusBarPanel
    End Class
End Namespace

