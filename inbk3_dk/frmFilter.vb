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

Namespace inbk3
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
                If (StringType.StrCmp(StringType.FromChar(DirMain.cForm), "2", False) = 0) Then
                    Me.txtTitle.Text = Strings.Trim(StringType.FromObject(DirMain.oLan.Item("902")))
                Else
                    Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(Me.cboReports.SelectedIndex), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
                End If
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
                Reg.SetRegistryKey("DFItem", Me.txtMa_vt.Text)
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
            reportformlib.AddFreeFields(DirMain.sysConn, Me.tabReports.TabPages.Item(3), 10)
            Me.txtMa_vt.Text = StringType.FromObject(Reg.GetRegistryKey("DFItem"))
            Dim dMa_vt As New DirLib(Me.txtMa_vt, Me.lblTen_vt, DirMain.sysConn, DirMain.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", False, Me.cmdCancel)
            reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
            Me.txtMa_vt.Text = StringType.FromObject(Reg.GetRegistryKey("DFItem"))
            Dim oKh As New vouchersearchlibobj(Me.txtMa_kh, Me.lblTen_kh, DirMain.sysConn, DirMain.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj7 As New vouchersearchlibobj(Me.txtMa_kho, Me.lblTen_kho, DirMain.sysConn, DirMain.appConn, "dmkho", "ma_kho", "ten_kho", "Store", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj8 As New vouchersearchlibobj(Me.txtMa_khon, Me.lblTen_khon, DirMain.sysConn, DirMain.appConn, "dmkho", "ma_kho", "ten_kho", "Store", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj6 As New vouchersearchlibobj(Me.txtMa_nx, Me.lblTen_nx, DirMain.sysConn, DirMain.appConn, "dmnx", "ma_nx", "ten_nx", "Reason", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtMa_vv, Me.lblTen_vv, DirMain.sysConn, DirMain.appConn, "dmvv", "ma_vv", "ten_vv", "Job", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj9 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtMa_td1, Me.lblTen_td1, DirMain.sysConn, DirMain.appConn, "dmtd1", "ma_td", "ten_td", "Free1", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtMa_td2, Me.lblTen_td2, DirMain.sysConn, DirMain.appConn, "dmtd2", "ma_td", "ten_td", "Free2", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtMa_td3, Me.lblTen_td3, DirMain.sysConn, DirMain.appConn, "dmtd3", "ma_td", "ten_td", "Free3", "1=1", True, Me.cmdCancel)
            Me.CancelButton = Me.cmdCancel
            Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
            Me.tabReports.TabPages.Remove(Me.tbgFree)
            Me.tabReports.TabPages.Remove(Me.tbgOther)
            Me.tabReports.TabPages.Remove(Me.tbgOptions)
            If (StringType.StrCmp(StringType.FromChar(DirMain.cForm), "2", False) = 0) Then
                DirMain.fPrint.Text = Strings.Trim(StringType.FromObject(DirMain.oLan.Item("003")))
                Me.txtTitle.Text = Strings.Trim(StringType.FromObject(DirMain.oLan.Item("902")))
            Else
                Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
            End If
            Me.txtDFrom.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
            Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
            DirMain.oAdvFilter = New clsAdvFilter(Me, Me.tbgAdv, Me.tabReports, DirMain.appConn, DirMain.sysConn, Me.pnContent, Me.cmdCancel)
            DirMain.oAdvFilter.AddAdvSelect(StringType.FromObject(DirMain.drAdvFilter.Item("cadvtables")))
            Dim fPrint As frmFilter = DirMain.fPrint
            Dim grdOrder As clsgrid = fPrint.grdOrder
            DirMain.oAdvFilter.InitGridOrder((grdOrder), DirMain.SysID, "001", (Me.ds), "Order")
            fPrint.grdOrder = grdOrder
            fPrint = DirMain.fPrint
            grdOrder = fPrint.grdTransCode
            DirMain.oAdvFilter.InitGridTransCode((grdOrder), StringType.FromChar(DirMain.cForm), (Me.ds), "TransCode")
            fPrint.grdTransCode = grdOrder
            DirMain.oxInv = New xInv(Me.tabReports, Me.pnContent, DirMain.appConn, DirMain.sysConn, True)
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
            Me.lblTen_vv = New Label
            Me.lblTen_nx = New Label
            Me.Label2 = New Label
            Me.Label6 = New Label
            Me.txtMa_vv = New TextBox
            Me.txtMa_nx = New TextBox
            Me.Label5 = New Label
            Me.txtInvTo = New TextBox
            Me.txtInvFrom = New TextBox
            Me.lblTen_vt = New Label
            Me.txtMa_vt = New TextBox
            Me.Label4 = New Label
            Me.lblTen_khon = New Label
            Me.txtMa_khon = New TextBox
            Me.Label1 = New Label
            Me.lblTen_kh = New Label
            Me.txtMa_kh = New TextBox
            Me.lblTk_co = New Label
            Me.txtDTo = New txtDate
            Me.txtDFrom = New txtDate
            Me.lblDateFromTo = New Label
            Me.lblMau_bc = New Label
            Me.cboReports = New ComboBox
            Me.lblTitle = New Label
            Me.txtTitle = New TextBox
            Me.txtMa_kho = New TextBox
            Me.lblTen_kho = New Label
            Me.Label3 = New Label
            Me.tbgOther = New TabPage
            Me.tbgOptions = New TabPage
            Me.tbgFree = New TabPage
            Me.lblMa_td1 = New Label
            Me.txtMa_td1 = New TextBox
            Me.txtMa_td2 = New TextBox
            Me.txtMa_td3 = New TextBox
            Me.lblTen_td2 = New Label
            Me.lblTen_td3 = New Label
            Me.lblMa_td3 = New Label
            Me.lblMa_td2 = New Label
            Me.lblTen_td1 = New Label
            Me.tbgAdv = New TabPage
            Me.tbgOrder = New TabPage
            Me.grdOrder = New clsgrid
            Me.tbgTransCode = New TabPage
            Me.grdTransCode = New clsgrid
            Me.tabReports.SuspendLayout()
            Me.tbgFilter.SuspendLayout()
            Me.tbgFree.SuspendLayout()
            Me.tbgOrder.SuspendLayout()
            Me.grdOrder.BeginInit()
            Me.tbgTransCode.SuspendLayout()
            Me.grdTransCode.BeginInit()
            Me.SuspendLayout()
            Me.txtMa_dvcs.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New Point(160, 197)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.TabIndex = 16
            Me.txtMa_dvcs.Tag = "FCML"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New Point(20, 199)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New Size(36, 16)
            Me.lblMa_dvcs.TabIndex = 1
            Me.lblMa_dvcs.Tag = "L102"
            Me.lblMa_dvcs.Text = "Don vi"
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New Point(264, 199)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New Size(50, 16)
            Me.lblTen_dvcs.TabIndex = 7
            Me.lblTen_dvcs.Tag = "L002"
            Me.lblTen_dvcs.Text = "Ten dvcs"
            Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdOk.Location = New Point(3, 316)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 1
            Me.cmdOk.Tag = "L001"
            Me.cmdOk.Text = "Nhan"
            Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdCancel.Location = New Point(79, 316)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 2
            Me.cmdCancel.Tag = "L002"
            Me.cmdCancel.Text = "Huy"
            Me.tabReports.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.tabReports.Controls.Add(Me.tbgFilter)
            Me.tabReports.Controls.Add(Me.tbgOther)
            Me.tabReports.Controls.Add(Me.tbgOptions)
            Me.tabReports.Controls.Add(Me.tbgFree)
            Me.tabReports.Controls.Add(Me.tbgAdv)
            Me.tabReports.Controls.Add(Me.tbgOrder)
            Me.tabReports.Controls.Add(Me.tbgTransCode)
            Me.tabReports.Location = New Point(-2, 0)
            Me.tabReports.Name = "tabReports"
            Me.tabReports.SelectedIndex = 0
            Me.tabReports.Size = New Size(609, 304)
            Me.tabReports.TabIndex = 0
            Me.tabReports.Tag = "L200"
            Me.tbgFilter.Controls.Add(Me.lblTen_vv)
            Me.tbgFilter.Controls.Add(Me.lblTen_nx)
            Me.tbgFilter.Controls.Add(Me.Label2)
            Me.tbgFilter.Controls.Add(Me.Label6)
            Me.tbgFilter.Controls.Add(Me.txtMa_vv)
            Me.tbgFilter.Controls.Add(Me.txtMa_nx)
            Me.tbgFilter.Controls.Add(Me.Label5)
            Me.tbgFilter.Controls.Add(Me.txtInvTo)
            Me.tbgFilter.Controls.Add(Me.txtInvFrom)
            Me.tbgFilter.Controls.Add(Me.lblTen_vt)
            Me.tbgFilter.Controls.Add(Me.txtMa_vt)
            Me.tbgFilter.Controls.Add(Me.Label4)
            Me.tbgFilter.Controls.Add(Me.lblTen_khon)
            Me.tbgFilter.Controls.Add(Me.txtMa_khon)
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
            Me.tbgFilter.Controls.Add(Me.txtMa_kho)
            Me.tbgFilter.Controls.Add(Me.lblTen_kho)
            Me.tbgFilter.Controls.Add(Me.Label3)
            Me.tbgFilter.Location = New Point(4, 22)
            Me.tbgFilter.Name = "tbgFilter"
            Me.tbgFilter.Size = New Size(601, 278)
            Me.tbgFilter.TabIndex = 0
            Me.tbgFilter.Tag = "L100"
            Me.tbgFilter.Text = "Dieu kien loc"
            Me.lblTen_vv.AutoSize = True
            Me.lblTen_vv.Location = New Point(264, 176)
            Me.lblTen_vv.Name = "lblTen_vv"
            Me.lblTen_vv.Size = New Size(62, 16)
            Me.lblTen_vv.TabIndex = 49
            Me.lblTen_vv.Tag = "RF"
            Me.lblTen_vv.Text = "Ten vu viec"
            Me.lblTen_nx.AutoSize = True
            Me.lblTen_nx.Location = New Point(264, 153)
            Me.lblTen_nx.Name = "lblTen_nx"
            Me.lblTen_nx.Size = New Size(105, 16)
            Me.lblTen_nx.TabIndex = 48
            Me.lblTen_nx.Tag = "RF"
            Me.lblTen_nx.Text = "Ten dang nhap xuat"
            Me.Label2.AutoSize = True
            Me.Label2.Location = New Point(20, 176)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New Size(41, 16)
            Me.Label2.TabIndex = 47
            Me.Label2.Tag = "L109"
            Me.Label2.Text = "Vu viec"
            Me.Label6.AutoSize = True
            Me.Label6.Location = New Point(20, 153)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New Size(84, 16)
            Me.Label6.TabIndex = 46
            Me.Label6.Tag = "L105"
            Me.Label6.Text = "Dang nhap xuat"
            Me.txtMa_vv.Location = New Point(160, 174)
            Me.txtMa_vv.Name = "txtMa_vv"
            Me.txtMa_vv.TabIndex = 15
            Me.txtMa_vv.Tag = "FCML"
            Me.txtMa_vv.Text = "txtMa_vv"
            Me.txtMa_nx.Location = New Point(160, 151)
            Me.txtMa_nx.Name = "txtMa_nx"
            Me.txtMa_nx.TabIndex = 14
            Me.txtMa_nx.Tag = "FCML"
            Me.txtMa_nx.Text = "txtMa_nx"
            Me.Label5.AutoSize = True
            Me.Label5.Location = New Point(20, 38)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New Size(74, 16)
            Me.Label5.TabIndex = 43
            Me.Label5.Tag = "L111"
            Me.Label5.Text = "Hoa don tu so"
            Me.txtInvTo.CharacterCasing = CharacterCasing.Upper
            Me.txtInvTo.Location = New Point(264, 36)
            Me.txtInvTo.MaxLength = 12
            Me.txtInvTo.Name = "txtInvTo"
            Me.txtInvTo.TabIndex = 3
            Me.txtInvTo.Tag = "FCML"
            Me.txtInvTo.Text = "TXTINVTO"
            Me.txtInvTo.TextAlign = HorizontalAlignment.Right
            Me.txtInvFrom.CharacterCasing = CharacterCasing.Upper
            Me.txtInvFrom.Location = New Point(160, 36)
            Me.txtInvFrom.MaxLength = 12
            Me.txtInvFrom.Name = "txtInvFrom"
            Me.txtInvFrom.TabIndex = 2
            Me.txtInvFrom.Tag = "FCML"
            Me.txtInvFrom.Text = "TXTINVFROM"
            Me.txtInvFrom.TextAlign = HorizontalAlignment.Right
            Me.lblTen_vt.AutoSize = True
            Me.lblTen_vt.Location = New Point(264, 61)
            Me.lblTen_vt.Name = "lblTen_vt"
            Me.lblTen_vt.Size = New Size(54, 16)
            Me.lblTen_vt.TabIndex = 37
            Me.lblTen_vt.Tag = "RF"
            Me.lblTen_vt.Text = "Ten vat tu"
            Me.txtMa_vt.Location = New Point(160, 59)
            Me.txtMa_vt.Name = "txtMa_vt"
            Me.txtMa_vt.TabIndex = 4
            Me.txtMa_vt.Tag = "FCMLNB"
            Me.txtMa_vt.Text = "txtMa_vt"
            Me.Label4.AutoSize = True
            Me.Label4.Location = New Point(20, 61)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New Size(50, 16)
            Me.Label4.TabIndex = 24
            Me.Label4.Tag = "L110"
            Me.Label4.Text = "Ma vat tu"
            Me.lblTen_khon.AutoSize = True
            Me.lblTen_khon.Location = New Point(264, 107)
            Me.lblTen_khon.Name = "lblTen_khon"
            Me.lblTen_khon.Size = New Size(101, 16)
            Me.lblTen_khon.TabIndex = 20
            Me.lblTen_khon.Tag = "RF"
            Me.lblTen_khon.Text = "Ten kho hang nhap"
            Me.txtMa_khon.Location = New Point(160, 105)
            Me.txtMa_khon.Name = "txtMa_khon"
            Me.txtMa_khon.TabIndex = 6
            Me.txtMa_khon.Tag = "FCML"
            Me.txtMa_khon.Text = "txtMa_khon"
            Me.Label1.AutoSize = True
            Me.Label1.Location = New Point(20, 107)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New Size(105, 16)
            Me.Label1.TabIndex = 14
            Me.Label1.Tag = "L108"
            Me.Label1.Text = "Ma kho dieu chuyen"
            Me.lblTen_kh.AutoSize = True
            Me.lblTen_kh.Location = New Point(264, 130)
            Me.lblTen_kh.Name = "lblTen_kh"
            Me.lblTen_kh.Size = New Size(85, 16)
            Me.lblTen_kh.TabIndex = 13
            Me.lblTen_kh.Tag = "RF"
            Me.lblTen_kh.Text = "Ten khach hang"
            Me.txtMa_kh.Location = New Point(160, 128)
            Me.txtMa_kh.Name = "txtMa_kh"
            Me.txtMa_kh.TabIndex = 7
            Me.txtMa_kh.Tag = "FCML"
            Me.txtMa_kh.Text = "txtMa_kh"
            Me.lblTk_co.AutoSize = True
            Me.lblTk_co.Location = New Point(20, 130)
            Me.lblTk_co.Name = "lblTk_co"
            Me.lblTk_co.Size = New Size(65, 16)
            Me.lblTk_co.TabIndex = 11
            Me.lblTk_co.Tag = "L106"
            Me.lblTk_co.Text = "Khach hang"
            Me.txtDTo.Location = New Point(263, 13)
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
            Me.lblDateFromTo.Tag = "L101"
            Me.lblDateFromTo.Text = "Tu/den ngay"
            Me.lblMau_bc.AutoSize = True
            Me.lblMau_bc.Location = New Point(20, 222)
            Me.lblMau_bc.Name = "lblMau_bc"
            Me.lblMau_bc.Size = New Size(69, 16)
            Me.lblMau_bc.TabIndex = 2
            Me.lblMau_bc.Tag = "L103"
            Me.lblMau_bc.Text = "Mau bao cao"
            Me.cboReports.Location = New Point(160, 220)
            Me.cboReports.Name = "cboReports"
            Me.cboReports.Size = New Size(300, 21)
            Me.cboReports.TabIndex = 17
            Me.cboReports.Text = "cboReports"
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New Point(20, 245)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New Size(42, 16)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Tag = "L104"
            Me.lblTitle.Text = "Tieu de"
            Me.txtTitle.Location = New Point(160, 243)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New Size(300, 20)
            Me.txtTitle.TabIndex = 18
            Me.txtTitle.Tag = "NB"
            Me.txtTitle.Text = "txtTieu_de"
            Me.txtMa_kho.Location = New Point(160, 82)
            Me.txtMa_kho.Name = "txtMa_kho"
            Me.txtMa_kho.TabIndex = 5
            Me.txtMa_kho.Tag = "FCML"
            Me.txtMa_kho.Text = "txtMa_kho"
            Me.lblTen_kho.AutoSize = True
            Me.lblTen_kho.Location = New Point(264, 84)
            Me.lblTen_kho.Name = "lblTen_kho"
            Me.lblTen_kho.Size = New Size(98, 16)
            Me.lblTen_kho.TabIndex = 40
            Me.lblTen_kho.Tag = "RF"
            Me.lblTen_kho.Text = "Ten kho hang xuat"
            Me.Label3.AutoSize = True
            Me.Label3.Location = New Point(20, 84)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New Size(41, 16)
            Me.Label3.TabIndex = 39
            Me.Label3.Tag = "L107"
            Me.Label3.Text = "Ma kho"
            Me.tbgOther.Location = New Point(4, 22)
            Me.tbgOther.Name = "tbgOther"
            Me.tbgOther.Size = New Size(601, 278)
            Me.tbgOther.TabIndex = 3
            Me.tbgOther.Tag = "FreeReportOther"
            Me.tbgOther.Text = "Dieu kien khac"
            Me.tbgOptions.Location = New Point(4, 22)
            Me.tbgOptions.Name = "tbgOptions"
            Me.tbgOptions.Size = New Size(601, 278)
            Me.tbgOptions.TabIndex = 1
            Me.tbgOptions.Tag = "L200"
            Me.tbgOptions.Text = "Lua chon"
            Me.tbgFree.Controls.Add(Me.lblMa_td1)
            Me.tbgFree.Controls.Add(Me.txtMa_td1)
            Me.tbgFree.Controls.Add(Me.txtMa_td2)
            Me.tbgFree.Controls.Add(Me.txtMa_td3)
            Me.tbgFree.Controls.Add(Me.lblTen_td2)
            Me.tbgFree.Controls.Add(Me.lblTen_td3)
            Me.tbgFree.Controls.Add(Me.lblMa_td3)
            Me.tbgFree.Controls.Add(Me.lblMa_td2)
            Me.tbgFree.Controls.Add(Me.lblTen_td1)
            Me.tbgFree.Location = New Point(4, 22)
            Me.tbgFree.Name = "tbgFree"
            Me.tbgFree.Size = New Size(601, 278)
            Me.tbgFree.TabIndex = 2
            Me.tbgFree.Tag = "FreeReportCaption"
            Me.tbgFree.Text = "Dieu kien ma tu do"
            Me.lblMa_td1.AutoSize = True
            Me.lblMa_td1.Location = New Point(20, 16)
            Me.lblMa_td1.Name = "lblMa_td1"
            Me.lblMa_td1.Size = New Size(57, 16)
            Me.lblMa_td1.TabIndex = 82
            Me.lblMa_td1.Tag = "FreeCaption1"
            Me.lblMa_td1.Text = "Ma tu do 1"
            Me.txtMa_td1.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_td1.Location = New Point(160, 12)
            Me.txtMa_td1.Name = "txtMa_td1"
            Me.txtMa_td1.TabIndex = 79
            Me.txtMa_td1.Tag = "FCDetail#ma_td1 like '%s%'#ML"
            Me.txtMa_td1.Text = "TXTMA_TD1"
            Me.txtMa_td2.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_td2.Location = New Point(160, 35)
            Me.txtMa_td2.Name = "txtMa_td2"
            Me.txtMa_td2.TabIndex = 80
            Me.txtMa_td2.Tag = "FCDetail#ma_td2 like '%s%'#ML"
            Me.txtMa_td2.Text = "TXTMA_TD2"
            Me.txtMa_td3.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_td3.Location = New Point(160, 58)
            Me.txtMa_td3.Name = "txtMa_td3"
            Me.txtMa_td3.TabIndex = 81
            Me.txtMa_td3.Tag = "FCDetail#ma_td3 like '%s%'#ML"
            Me.txtMa_td3.Text = "TXTMA_TD3"
            Me.lblTen_td2.AutoSize = True
            Me.lblTen_td2.Location = New Point(272, 39)
            Me.lblTen_td2.Name = "lblTen_td2"
            Me.lblTen_td2.Size = New Size(61, 16)
            Me.lblTen_td2.TabIndex = 86
            Me.lblTen_td2.Tag = ""
            Me.lblTen_td2.Text = "Ten tu do 2"
            Me.lblTen_td3.AutoSize = True
            Me.lblTen_td3.Location = New Point(272, 62)
            Me.lblTen_td3.Name = "lblTen_td3"
            Me.lblTen_td3.Size = New Size(61, 16)
            Me.lblTen_td3.TabIndex = 87
            Me.lblTen_td3.Tag = ""
            Me.lblTen_td3.Text = "Ten tu do 3"
            Me.lblMa_td3.AutoSize = True
            Me.lblMa_td3.Location = New Point(20, 62)
            Me.lblMa_td3.Name = "lblMa_td3"
            Me.lblMa_td3.Size = New Size(57, 16)
            Me.lblMa_td3.TabIndex = 84
            Me.lblMa_td3.Tag = "FreeCaption3"
            Me.lblMa_td3.Text = "Ma tu do 3"
            Me.lblMa_td2.AutoSize = True
            Me.lblMa_td2.Location = New Point(20, 39)
            Me.lblMa_td2.Name = "lblMa_td2"
            Me.lblMa_td2.Size = New Size(57, 16)
            Me.lblMa_td2.TabIndex = 83
            Me.lblMa_td2.Tag = "FreeCaption2"
            Me.lblMa_td2.Text = "Ma tu do 2"
            Me.lblTen_td1.AutoSize = True
            Me.lblTen_td1.Location = New Point(272, 16)
            Me.lblTen_td1.Name = "lblTen_td1"
            Me.lblTen_td1.Size = New Size(61, 16)
            Me.lblTen_td1.TabIndex = 85
            Me.lblTen_td1.Tag = ""
            Me.lblTen_td1.Text = "Ten tu do 1"
            Me.tbgAdv.Location = New Point(4, 22)
            Me.tbgAdv.Name = "tbgAdv"
            Me.tbgAdv.Size = New Size(601, 278)
            Me.tbgAdv.TabIndex = 4
            Me.tbgAdv.Tag = "L300"
            Me.tbgAdv.Text = "Loc chi tiet"
            Me.tbgOrder.Controls.Add(Me.grdOrder)
            Me.tbgOrder.Location = New Point(4, 22)
            Me.tbgOrder.Name = "tbgOrder"
            Me.tbgOrder.Size = New Size(601, 278)
            Me.tbgOrder.TabIndex = 5
            Me.tbgOrder.Tag = "L400"
            Me.tbgOrder.Text = "Thu tu sap xep"
            Me.grdOrder.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.grdOrder.CaptionVisible = False
            Me.grdOrder.DataMember = ""
            Me.grdOrder.HeaderForeColor = SystemColors.ControlText
            Me.grdOrder.Location = New Point(0, 0)
            Me.grdOrder.Name = "grdOrder"
            Me.grdOrder.Size = New Size(601, 280)
            Me.grdOrder.TabIndex = 1
            Me.tbgTransCode.Controls.Add(Me.grdTransCode)
            Me.tbgTransCode.Location = New Point(4, 22)
            Me.tbgTransCode.Name = "tbgTransCode"
            Me.tbgTransCode.Size = New Size(601, 278)
            Me.tbgTransCode.TabIndex = 6
            Me.tbgTransCode.Tag = "L500"
            Me.tbgTransCode.Text = "Ma giao dich"
            Me.grdTransCode.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.grdTransCode.CaptionVisible = False
            Me.grdTransCode.DataMember = ""
            Me.grdTransCode.HeaderForeColor = SystemColors.ControlText
            Me.grdTransCode.Location = New Point(0, -1)
            Me.grdTransCode.Name = "grdTransCode"
            Me.grdTransCode.Size = New Size(601, 280)
            Me.grdTransCode.TabIndex = 2
            Me.AutoScaleBaseSize = New Size(5, 13)
            Me.ClientSize = New Size(608, 373)
            Me.Controls.Add(Me.tabReports)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Name = "frmFilter"
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "frmFilter"
            Me.tabReports.ResumeLayout(False)
            Me.tbgFilter.ResumeLayout(False)
            Me.tbgFree.ResumeLayout(False)
            Me.tbgOrder.ResumeLayout(False)
            Me.grdOrder.EndInit()
            Me.tbgTransCode.ResumeLayout(False)
            Me.grdTransCode.EndInit()
            Me.ResumeLayout(False)
        End Sub


        ' Properties
        Friend WithEvents cboReports As ComboBox
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents grdOrder As clsgrid
        Friend WithEvents grdTransCode As clsgrid
        Friend WithEvents Label1 As Label
        Friend WithEvents Label2 As Label
        Friend WithEvents Label3 As Label
        Friend WithEvents Label4 As Label
        Friend WithEvents Label5 As Label
        Friend WithEvents Label6 As Label
        Friend WithEvents lblDateFromTo As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMa_td1 As Label
        Friend WithEvents lblMa_td2 As Label
        Friend WithEvents lblMa_td3 As Label
        Friend WithEvents lblMau_bc As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTen_kh As Label
        Friend WithEvents lblTen_kho As Label
        Friend WithEvents lblTen_khon As Label
        Friend WithEvents lblTen_nx As Label
        Friend WithEvents lblTen_td1 As Label
        Friend WithEvents lblTen_td2 As Label
        Friend WithEvents lblTen_td3 As Label
        Friend WithEvents lblTen_vt As Label
        Friend WithEvents lblTen_vv As Label
        Friend WithEvents lblTitle As Label
        Friend WithEvents lblTk_co As Label
        Friend WithEvents tabReports As TabControl
        Friend WithEvents tbgAdv As TabPage
        Friend WithEvents tbgFilter As TabPage
        Friend WithEvents tbgFree As TabPage
        Friend WithEvents tbgOptions As TabPage
        Friend WithEvents tbgOrder As TabPage
        Friend WithEvents tbgOther As TabPage
        Friend WithEvents tbgTransCode As TabPage
        Friend WithEvents txtDFrom As txtDate
        Friend WithEvents txtDTo As txtDate
        Friend WithEvents txtInvFrom As TextBox
        Friend WithEvents txtInvTo As TextBox
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtMa_kh As TextBox
        Friend WithEvents txtMa_kho As TextBox
        Friend WithEvents txtMa_khon As TextBox
        Friend WithEvents txtMa_nx As TextBox
        Friend WithEvents txtMa_td1 As TextBox
        Friend WithEvents txtMa_td2 As TextBox
        Friend WithEvents txtMa_td3 As TextBox
        Friend WithEvents txtMa_vt As TextBox
        Friend WithEvents txtMa_vv As TextBox
        Friend WithEvents txtTitle As TextBox

        Private components As IContainer
        Public ds As DataSet
        Private dvOrder As DataView
        Private intGroup1 As Integer
        Private intGroup2 As Integer
        Private intGroup3 As Integer
        Public pnContent As StatusBarPanel
    End Class
End Namespace

