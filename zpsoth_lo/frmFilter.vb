Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon
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
            DirMain.dFrom = Me.txtDFrom.Value
            DirMain.dTo = Me.txtDTo.Value
            Reg.SetRegistryKey("DFDFrom", Me.txtDFrom.Value)
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

    Private Sub cmdOk_Validated(ByVal sender As Object, ByVal e As EventArgs)
        Me.flag = True
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
        reportformlib.AddFreeFields(DirMain.sysConn, Me.tabReports.TabPages.Item(3), 15)
        reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
        Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtMa_kh, Me.lblTen_kh, DirMain.sysConn, DirMain.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", DirMain.strKeyCust, True, Me.cmdCancel)
        Dim vouchersearchlibobj6 As New vouchersearchlibobj(Me.txtMa_vt, Me.lblTen_vt, DirMain.sysConn, DirMain.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj15 As New vouchersearchlibobj(Me.txtMa_kho, Me.lblTen_kho, DirMain.sysConn, DirMain.appConn, "dmkho", "ma_kho", "ten_kho", "Store", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtMa_nvbh, Me.lblTen_nvbh, DirMain.sysConn, DirMain.appConn, "dmnvbh", "ma_nvbh", "ten_nvbh", "SaleEmployee", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj7 As New vouchersearchlibobj(Me.txtTk_vt, Me.lblTen_Tk_vt, DirMain.sysConn, DirMain.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj8 As New vouchersearchlibobj(Me.txtTk_vt2, Me.lblTen_Tk_vt2, DirMain.sysConn, DirMain.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj9 As New vouchersearchlibobj(Me.txtNh_vt, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj10 As New vouchersearchlibobj(Me.txtNh_vt2, Me.lblTen_nh2, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup2", "loai_nh=2", True, Me.cmdCancel)
        Dim vouchersearchlibobj11 As New vouchersearchlibobj(Me.txtNh_vt3, Me.lblTen_nh3, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup3", "loai_nh=3", True, Me.cmdCancel)
        Dim vouchersearchlibobj12 As New vouchersearchlibobj(Me.txtLoai_vt, Me.lblTen_loai, DirMain.sysConn, DirMain.appConn, "Dmloaivt", "ma_loai_vt", "ten_loai_vt", "ItemType", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj14 As New vouchersearchlibobj(Me.txtMa_nx, Me.lblTen_nx, DirMain.sysConn, DirMain.appConn, "dmnx", "ma_nx", "ten_nx", "Reason", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj13 As New vouchersearchlibobj(Me.txtMa_vv, Me.lblTen_vv, DirMain.sysConn, DirMain.appConn, "dmvv", "ma_vv", "ten_vv", "Job", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj16 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtMa_td1, Me.lblTen_td1, DirMain.sysConn, DirMain.appConn, "dmtd1", "ma_td", "ten_td", "Free1", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtMa_td2, Me.lblTen_td2, DirMain.sysConn, DirMain.appConn, "dmtd2", "ma_td", "ten_td", "Free2", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtMa_td3, Me.lblTen_td3, DirMain.sysConn, DirMain.appConn, "dmtd3", "ma_td", "ten_td", "Free3", "1=1", True, Me.cmdCancel)
        Dim vGroup_cust1 As New vouchersearchlibobj(Me.txtNh_kh1, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "Dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=1", True, Me.cmdCancel)
        Dim vGroup_cust2 As New vouchersearchlibobj(Me.txtNh_kh2, Me.lblTen_nh2, DirMain.sysConn, DirMain.appConn, "Dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=2", True, Me.cmdCancel)
        Dim vGroup_cust3 As New vouchersearchlibobj(Me.txtNh_kh3, Me.lblTen_nh3, DirMain.sysConn, DirMain.appConn, "Dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=3", True, Me.cmdCancel)
        Dim clgroup1 As New CharLib(Me.txtGroup1, "0,1,2,3")
        Dim lib2 As New CharLib(Me.txtGroup2, "0,1,2,3")
        Dim lib3 As New CharLib(Me.txtGroup3, "0,1,2,3")
        Me.txtGroup1.Text = StringType.FromInteger(0)
        Me.txtGroup2.Text = StringType.FromInteger(0)
        Me.txtGroup3.Text = StringType.FromInteger(0)
        Me.CancelButton = Me.cmdCancel
        Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
        Dim document As New PrintDocument
        Me.pnContent.Text = document.PrinterSettings.PrinterName
        Me.tabReports.TabPages.Remove(Me.tbgFree)
        Me.tabReports.TabPages.Remove(Me.tbgOther)
        Me.tabReports.SelectedIndex = 0
        Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        Me.txtDFrom.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
        Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
        DirMain.oAdvFilter = New clsAdvFilter(Me, Me.tbgAdv, Me.tabReports, DirMain.appConn, DirMain.sysConn, Me.pnContent, Me.cmdCancel)
        DirMain.oAdvFilter.AddAdvSelect(StringType.FromObject(DirMain.drAdvFilter.Item("cadvtables")))
        DirMain.oAdvFilter.InitGridOrder((grdOrder), DirMain.SysID, "001", (Me.ds), "Order")
        DirMain.oAdvFilter.InitGridTransCode(fPrint.grdTransCode, "2", DirMain.cCodeSelected, (Me.ds), "TransCode")
        DirMain.oAdvFilter.AddComboboxValue(Me.CbbGroup, DirMain.SysID, "002", (Me.ds), "Group")
        DirMain.oAdvFilter.AddComboboxValue(Me.CbbTinh_dc, DirMain.SysID, "003", (Me.ds), "Transfer")
        DirMain.oxInv = New xInv(Me.tabReports, Me.pnContent, DirMain.appConn, DirMain.sysConn)
    End Sub
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtNh_kh3 As System.Windows.Forms.TextBox
    Friend WithEvents txtNh_kh2 As System.Windows.Forms.TextBox
    Friend WithEvents txtNh_kh1 As System.Windows.Forms.TextBox

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtMa_dvcs = New System.Windows.Forms.TextBox
        Me.lblMa_dvcs = New System.Windows.Forms.Label
        Me.lblTen_dvcs = New System.Windows.Forms.Label
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tabReports = New System.Windows.Forms.TabControl
        Me.tbgFilter = New System.Windows.Forms.TabPage
        Me.txtNh_vt3 = New System.Windows.Forms.TextBox
        Me.txtNh_vt2 = New System.Windows.Forms.TextBox
        Me.lblTen_nvbh = New System.Windows.Forms.Label
        Me.txtMa_nvbh = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtTk_vt2 = New System.Windows.Forms.TextBox
        Me.lblTen_Tk_vt2 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtNh_vt = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtTk_vt = New System.Windows.Forms.TextBox
        Me.lblTen_Tk_vt = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtLoai_vt = New System.Windows.Forms.TextBox
        Me.lblTen_loai = New System.Windows.Forms.Label
        Me.lblTen_vt = New System.Windows.Forms.Label
        Me.txtMa_vt = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtInvTo = New System.Windows.Forms.TextBox
        Me.txtInvFrom = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblTen_vv = New System.Windows.Forms.Label
        Me.lblTen_nx = New System.Windows.Forms.Label
        Me.lblTen_kho = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtMa_vv = New System.Windows.Forms.TextBox
        Me.txtMa_nx = New System.Windows.Forms.TextBox
        Me.txtMa_kho = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblTen_kh = New System.Windows.Forms.Label
        Me.txtMa_kh = New System.Windows.Forms.TextBox
        Me.lblTk_co = New System.Windows.Forms.Label
        Me.txtDTo = New txtDate
        Me.txtDFrom = New txtDate
        Me.lblDateFromTo = New System.Windows.Forms.Label
        Me.lblMau_bc = New System.Windows.Forms.Label
        Me.cboReports = New System.Windows.Forms.ComboBox
        Me.lblTitle = New System.Windows.Forms.Label
        Me.txtTitle = New System.Windows.Forms.TextBox
        Me.tbgOptions = New System.Windows.Forms.TabPage
        Me.txtGroup3 = New System.Windows.Forms.TextBox
        Me.txtGroup2 = New System.Windows.Forms.TextBox
        Me.txtGroup1 = New System.Windows.Forms.TextBox
        Me.lblTk = New System.Windows.Forms.Label
        Me.CbbGroup = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.CbbTinh_dc = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.tbgAdv = New System.Windows.Forms.TabPage
        Me.tbgOrder = New System.Windows.Forms.TabPage
        Me.grdOrder = New clsgrid
        Me.tbgFree = New System.Windows.Forms.TabPage
        Me.lblMa_td1 = New System.Windows.Forms.Label
        Me.txtMa_td1 = New System.Windows.Forms.TextBox
        Me.txtMa_td2 = New System.Windows.Forms.TextBox
        Me.txtMa_td3 = New System.Windows.Forms.TextBox
        Me.lblTen_td2 = New System.Windows.Forms.Label
        Me.lblTen_td3 = New System.Windows.Forms.Label
        Me.lblMa_td3 = New System.Windows.Forms.Label
        Me.lblMa_td2 = New System.Windows.Forms.Label
        Me.lblTen_td1 = New System.Windows.Forms.Label
        Me.tbgOther = New System.Windows.Forms.TabPage
        Me.tbgTransCode = New System.Windows.Forms.TabPage
        Me.grdTransCode = New clsgrid
        Me.lblTen_nh = New System.Windows.Forms.Label
        Me.lblTen_nh2 = New System.Windows.Forms.Label
        Me.lblTen_nh3 = New System.Windows.Forms.Label
        Me.txtNh_kh3 = New System.Windows.Forms.TextBox
        Me.txtNh_kh2 = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtNh_kh1 = New System.Windows.Forms.TextBox
        Me.tabReports.SuspendLayout()
        Me.tbgFilter.SuspendLayout()
        Me.tbgOptions.SuspendLayout()
        Me.tbgOrder.SuspendLayout()
        CType(Me.grdOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbgFree.SuspendLayout()
        Me.tbgTransCode.SuspendLayout()
        CType(Me.grdTransCode, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtMa_dvcs
        '
        Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New System.Drawing.Point(160, 312)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.TabIndex = 19
        Me.txtMa_dvcs.Tag = "FCML"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        '
        'lblMa_dvcs
        '
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New System.Drawing.Point(20, 314)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New System.Drawing.Size(36, 16)
        Me.lblMa_dvcs.TabIndex = 1
        Me.lblMa_dvcs.Tag = "L102"
        Me.lblMa_dvcs.Text = "Don vi"
        '
        'lblTen_dvcs
        '
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New System.Drawing.Point(264, 314)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New System.Drawing.Size(50, 16)
        Me.lblTen_dvcs.TabIndex = 7
        Me.lblTen_dvcs.Tag = "L002"
        Me.lblTen_dvcs.Text = "Ten dvcs"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(3, 432)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Tag = "L001"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(79, 432)
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
        Me.tabReports.Controls.Add(Me.tbgOptions)
        Me.tabReports.Controls.Add(Me.tbgAdv)
        Me.tabReports.Controls.Add(Me.tbgOrder)
        Me.tabReports.Controls.Add(Me.tbgFree)
        Me.tabReports.Controls.Add(Me.tbgOther)
        Me.tabReports.Controls.Add(Me.tbgTransCode)
        Me.tabReports.Location = New System.Drawing.Point(-2, 0)
        Me.tabReports.Name = "tabReports"
        Me.tabReports.SelectedIndex = 0
        Me.tabReports.Size = New System.Drawing.Size(609, 420)
        Me.tabReports.TabIndex = 0
        Me.tabReports.Tag = "L200"
        '
        'tbgFilter
        '
        Me.tbgFilter.Controls.Add(Me.txtNh_kh3)
        Me.tbgFilter.Controls.Add(Me.txtNh_kh2)
        Me.tbgFilter.Controls.Add(Me.Label12)
        Me.tbgFilter.Controls.Add(Me.txtNh_kh1)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt3)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt2)
        Me.tbgFilter.Controls.Add(Me.lblTen_nvbh)
        Me.tbgFilter.Controls.Add(Me.txtMa_nvbh)
        Me.tbgFilter.Controls.Add(Me.Label15)
        Me.tbgFilter.Controls.Add(Me.Label9)
        Me.tbgFilter.Controls.Add(Me.txtTk_vt2)
        Me.tbgFilter.Controls.Add(Me.lblTen_Tk_vt2)
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
        Me.tbgFilter.Size = New System.Drawing.Size(601, 394)
        Me.tbgFilter.TabIndex = 0
        Me.tbgFilter.Tag = "L100"
        Me.tbgFilter.Text = "Dieu kien loc"
        '
        'txtNh_vt3
        '
        Me.txtNh_vt3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt3.Location = New System.Drawing.Point(368, 243)
        Me.txtNh_vt3.Name = "txtNh_vt3"
        Me.txtNh_vt3.TabIndex = 16
        Me.txtNh_vt3.Tag = "FCML"
        Me.txtNh_vt3.Text = "TXTNH_VT3"
        '
        'txtNh_vt2
        '
        Me.txtNh_vt2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt2.Location = New System.Drawing.Point(264, 243)
        Me.txtNh_vt2.Name = "txtNh_vt2"
        Me.txtNh_vt2.TabIndex = 15
        Me.txtNh_vt2.Tag = "FCML"
        Me.txtNh_vt2.Text = "TXTNH_VT2"
        '
        'lblTen_nvbh
        '
        Me.lblTen_nvbh.AutoSize = True
        Me.lblTen_nvbh.Location = New System.Drawing.Point(264, 153)
        Me.lblTen_nvbh.Name = "lblTen_nvbh"
        Me.lblTen_nvbh.Size = New System.Drawing.Size(126, 16)
        Me.lblTen_nvbh.TabIndex = 52
        Me.lblTen_nvbh.Tag = "RF"
        Me.lblTen_nvbh.Text = "Ten nhan vien ban hang"
        '
        'txtMa_nvbh
        '
        Me.txtMa_nvbh.Location = New System.Drawing.Point(160, 151)
        Me.txtMa_nvbh.Name = "txtMa_nvbh"
        Me.txtMa_nvbh.TabIndex = 10
        Me.txtMa_nvbh.Tag = "FCML"
        Me.txtMa_nvbh.Text = "txtMa_nvbh"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(20, 153)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(105, 16)
        Me.Label15.TabIndex = 51
        Me.Label15.Tag = "L115"
        Me.Label15.Text = "Nhan vien ban hang"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(20, 199)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 16)
        Me.Label9.TabIndex = 48
        Me.Label9.Tag = "L114"
        Me.Label9.Text = "Tk vat tu (HT)"
        '
        'txtTk_vt2
        '
        Me.txtTk_vt2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_vt2.Location = New System.Drawing.Point(160, 197)
        Me.txtTk_vt2.Name = "txtTk_vt2"
        Me.txtTk_vt2.TabIndex = 12
        Me.txtTk_vt2.Tag = "FCML"
        Me.txtTk_vt2.Text = "TXTTK_VT2"
        '
        'lblTen_Tk_vt2
        '
        Me.lblTen_Tk_vt2.AutoSize = True
        Me.lblTen_Tk_vt2.Location = New System.Drawing.Point(264, 199)
        Me.lblTen_Tk_vt2.Name = "lblTen_Tk_vt2"
        Me.lblTen_Tk_vt2.Size = New System.Drawing.Size(57, 16)
        Me.lblTen_Tk_vt2.TabIndex = 49
        Me.lblTen_Tk_vt2.Tag = ""
        Me.lblTen_Tk_vt2.Text = "Tk vat tu 2"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(20, 245)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(65, 16)
        Me.Label7.TabIndex = 45
        Me.Label7.Tag = "L111"
        Me.Label7.Text = "Nhom vat tu"
        '
        'txtNh_vt
        '
        Me.txtNh_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt.Location = New System.Drawing.Point(160, 243)
        Me.txtNh_vt.Name = "txtNh_vt"
        Me.txtNh_vt.TabIndex = 14
        Me.txtNh_vt.Tag = "FCML"
        Me.txtNh_vt.Text = "TXTNH_VT"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(20, 176)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(82, 16)
        Me.Label6.TabIndex = 42
        Me.Label6.Tag = "L109"
        Me.Label6.Text = "Tk vat tu (dmvt)"
        '
        'txtTk_vt
        '
        Me.txtTk_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_vt.Location = New System.Drawing.Point(160, 174)
        Me.txtTk_vt.Name = "txtTk_vt"
        Me.txtTk_vt.TabIndex = 11
        Me.txtTk_vt.Tag = "FCML"
        Me.txtTk_vt.Text = "TXTTK_VT"
        '
        'lblTen_Tk_vt
        '
        Me.lblTen_Tk_vt.AutoSize = True
        Me.lblTen_Tk_vt.Location = New System.Drawing.Point(264, 176)
        Me.lblTen_Tk_vt.Name = "lblTen_Tk_vt"
        Me.lblTen_Tk_vt.Size = New System.Drawing.Size(47, 16)
        Me.lblTen_Tk_vt.TabIndex = 43
        Me.lblTen_Tk_vt.Tag = "L016"
        Me.lblTen_Tk_vt.Text = "Tk vat tu"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(20, 222)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 16)
        Me.Label8.TabIndex = 40
        Me.Label8.Tag = "L110"
        Me.Label8.Text = "Loai vat tu"
        '
        'txtLoai_vt
        '
        Me.txtLoai_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLoai_vt.Location = New System.Drawing.Point(160, 220)
        Me.txtLoai_vt.Name = "txtLoai_vt"
        Me.txtLoai_vt.TabIndex = 13
        Me.txtLoai_vt.Tag = "FCML"
        Me.txtLoai_vt.Text = "TXTLOAI_VT"
        '
        'lblTen_loai
        '
        Me.lblTen_loai.AutoSize = True
        Me.lblTen_loai.Location = New System.Drawing.Point(264, 222)
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
        Me.lblTen_vv.Location = New System.Drawing.Point(264, 291)
        Me.lblTen_vv.Name = "lblTen_vv"
        Me.lblTen_vv.Size = New System.Drawing.Size(62, 16)
        Me.lblTen_vv.TabIndex = 22
        Me.lblTen_vv.Tag = "RF"
        Me.lblTen_vv.Text = "Ten vu viec"
        '
        'lblTen_nx
        '
        Me.lblTen_nx.AutoSize = True
        Me.lblTen_nx.Location = New System.Drawing.Point(264, 268)
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
        Me.Label3.Location = New System.Drawing.Point(20, 291)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 16)
        Me.Label3.TabIndex = 19
        Me.Label3.Tag = "L113"
        Me.Label3.Text = "Vu viec"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 268)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 16)
        Me.Label2.TabIndex = 18
        Me.Label2.Tag = "L112"
        Me.Label2.Text = "Dang nhap xuat"
        '
        'txtMa_vv
        '
        Me.txtMa_vv.Location = New System.Drawing.Point(160, 289)
        Me.txtMa_vv.Name = "txtMa_vv"
        Me.txtMa_vv.TabIndex = 18
        Me.txtMa_vv.Tag = "FCML"
        Me.txtMa_vv.Text = "txtMa_vv"
        '
        'txtMa_nx
        '
        Me.txtMa_nx.Location = New System.Drawing.Point(160, 266)
        Me.txtMa_nx.Name = "txtMa_nx"
        Me.txtMa_nx.TabIndex = 17
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
        Me.lblMau_bc.Location = New System.Drawing.Point(20, 337)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New System.Drawing.Size(69, 16)
        Me.lblMau_bc.TabIndex = 2
        Me.lblMau_bc.Tag = "L103"
        Me.lblMau_bc.Text = "Mau bao cao"
        '
        'cboReports
        '
        Me.cboReports.Location = New System.Drawing.Point(160, 335)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New System.Drawing.Size(300, 21)
        Me.cboReports.TabIndex = 20
        Me.cboReports.Text = "cboReports"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(20, 361)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(42, 16)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Tag = "L104"
        Me.lblTitle.Text = "Tieu de"
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(160, 359)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(300, 20)
        Me.txtTitle.TabIndex = 21
        Me.txtTitle.Tag = "NB"
        Me.txtTitle.Text = "txtTieu_de"
        '
        'tbgOptions
        '
        Me.tbgOptions.Controls.Add(Me.txtGroup3)
        Me.tbgOptions.Controls.Add(Me.txtGroup2)
        Me.tbgOptions.Controls.Add(Me.txtGroup1)
        Me.tbgOptions.Controls.Add(Me.lblTk)
        Me.tbgOptions.Controls.Add(Me.CbbGroup)
        Me.tbgOptions.Controls.Add(Me.Label10)
        Me.tbgOptions.Controls.Add(Me.CbbTinh_dc)
        Me.tbgOptions.Controls.Add(Me.Label11)
        Me.tbgOptions.Location = New System.Drawing.Point(4, 22)
        Me.tbgOptions.Name = "tbgOptions"
        Me.tbgOptions.Size = New System.Drawing.Size(601, 366)
        Me.tbgOptions.TabIndex = 1
        Me.tbgOptions.Tag = "L200"
        Me.tbgOptions.Text = "Lua chon"
        '
        'txtGroup3
        '
        Me.txtGroup3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGroup3.Location = New System.Drawing.Point(214, 61)
        Me.txtGroup3.MaxLength = 1
        Me.txtGroup3.Name = "txtGroup3"
        Me.txtGroup3.Size = New System.Drawing.Size(24, 20)
        Me.txtGroup3.TabIndex = 4
        Me.txtGroup3.Tag = "FC"
        Me.txtGroup3.Text = "TXTNO_CO"
        '
        'txtGroup2
        '
        Me.txtGroup2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGroup2.Location = New System.Drawing.Point(187, 61)
        Me.txtGroup2.MaxLength = 1
        Me.txtGroup2.Name = "txtGroup2"
        Me.txtGroup2.Size = New System.Drawing.Size(24, 20)
        Me.txtGroup2.TabIndex = 3
        Me.txtGroup2.Tag = "FC"
        Me.txtGroup2.Text = "TXTNO_CO"
        '
        'txtGroup1
        '
        Me.txtGroup1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGroup1.Location = New System.Drawing.Point(160, 61)
        Me.txtGroup1.MaxLength = 1
        Me.txtGroup1.Name = "txtGroup1"
        Me.txtGroup1.Size = New System.Drawing.Size(24, 20)
        Me.txtGroup1.TabIndex = 2
        Me.txtGroup1.Tag = "FC"
        Me.txtGroup1.Text = "TXTNO_CO"
        '
        'lblTk
        '
        Me.lblTk.AutoSize = True
        Me.lblTk.Location = New System.Drawing.Point(8, 63)
        Me.lblTk.Name = "lblTk"
        Me.lblTk.Size = New System.Drawing.Size(135, 16)
        Me.lblTk.TabIndex = 143
        Me.lblTk.Tag = "LA01"
        Me.lblTk.Text = "Thu tu sap xep theo nhom"
        '
        'CbbGroup
        '
        Me.CbbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbbGroup.Location = New System.Drawing.Point(160, 13)
        Me.CbbGroup.Name = "CbbGroup"
        Me.CbbGroup.Size = New System.Drawing.Size(300, 21)
        Me.CbbGroup.TabIndex = 0
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(8, 15)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(59, 16)
        Me.Label10.TabIndex = 139
        Me.Label10.Tag = "L203"
        Me.Label10.Text = "Nhom theo"
        '
        'CbbTinh_dc
        '
        Me.CbbTinh_dc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbbTinh_dc.Location = New System.Drawing.Point(160, 37)
        Me.CbbTinh_dc.Name = "CbbTinh_dc"
        Me.CbbTinh_dc.Size = New System.Drawing.Size(300, 21)
        Me.CbbTinh_dc.TabIndex = 1
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(8, 39)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(139, 16)
        Me.Label11.TabIndex = 137
        Me.Label11.Tag = "L204"
        Me.Label11.Text = "Tinh phat sinh dieu chuyen"
        '
        'tbgAdv
        '
        Me.tbgAdv.Location = New System.Drawing.Point(4, 22)
        Me.tbgAdv.Name = "tbgAdv"
        Me.tbgAdv.Size = New System.Drawing.Size(601, 366)
        Me.tbgAdv.TabIndex = 4
        Me.tbgAdv.Tag = "L300"
        Me.tbgAdv.Text = "Loc chi tiet"
        '
        'tbgOrder
        '
        Me.tbgOrder.Controls.Add(Me.grdOrder)
        Me.tbgOrder.Location = New System.Drawing.Point(4, 22)
        Me.tbgOrder.Name = "tbgOrder"
        Me.tbgOrder.Size = New System.Drawing.Size(601, 366)
        Me.tbgOrder.TabIndex = 5
        Me.tbgOrder.Tag = "L400"
        Me.tbgOrder.Text = "Thu tu sap xep"
        '
        'grdOrder
        '
        Me.grdOrder.Cell_EnableRaisingEvents = False
        Me.grdOrder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdOrder.CaptionVisible = False
        Me.grdOrder.DataMember = ""
        Me.grdOrder.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdOrder.Location = New System.Drawing.Point(0, 0)
        Me.grdOrder.Name = "grdOrder"
        Me.grdOrder.Size = New System.Drawing.Size(601, 368)
        Me.grdOrder.TabIndex = 1
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
        Me.tbgFree.Size = New System.Drawing.Size(601, 366)
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
        'tbgOther
        '
        Me.tbgOther.Location = New System.Drawing.Point(4, 22)
        Me.tbgOther.Name = "tbgOther"
        Me.tbgOther.Size = New System.Drawing.Size(601, 366)
        Me.tbgOther.TabIndex = 3
        Me.tbgOther.Tag = "FreeReportOther"
        Me.tbgOther.Text = "Dieu kien khac"
        '
        'tbgTransCode
        '
        Me.tbgTransCode.Controls.Add(Me.grdTransCode)
        Me.tbgTransCode.Location = New System.Drawing.Point(4, 22)
        Me.tbgTransCode.Name = "tbgTransCode"
        Me.tbgTransCode.Size = New System.Drawing.Size(601, 366)
        Me.tbgTransCode.TabIndex = 6
        Me.tbgTransCode.Tag = "L500"
        Me.tbgTransCode.Text = "Ma giao dich"
        '
        'grdTransCode
        '
        Me.grdTransCode.Cell_EnableRaisingEvents = False
        Me.grdTransCode.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdTransCode.CaptionVisible = False
        Me.grdTransCode.DataMember = ""
        Me.grdTransCode.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdTransCode.Location = New System.Drawing.Point(0, -1)
        Me.grdTransCode.Name = "grdTransCode"
        Me.grdTransCode.Size = New System.Drawing.Size(601, 368)
        Me.grdTransCode.TabIndex = 2
        '
        'lblTen_nh
        '
        Me.lblTen_nh.AutoSize = True
        Me.lblTen_nh.Location = New System.Drawing.Point(192, 448)
        Me.lblTen_nh.Name = "lblTen_nh"
        Me.lblTen_nh.Size = New System.Drawing.Size(43, 16)
        Me.lblTen_nh.TabIndex = 46
        Me.lblTen_nh.Tag = "L015"
        Me.lblTen_nh.Text = "Ten_nh"
        Me.lblTen_nh.Visible = False
        '
        'lblTen_nh2
        '
        Me.lblTen_nh2.AutoSize = True
        Me.lblTen_nh2.Location = New System.Drawing.Point(264, 448)
        Me.lblTen_nh2.Name = "lblTen_nh2"
        Me.lblTen_nh2.Size = New System.Drawing.Size(43, 16)
        Me.lblTen_nh2.TabIndex = 47
        Me.lblTen_nh2.Tag = "L015"
        Me.lblTen_nh2.Text = "Ten_nh"
        Me.lblTen_nh2.Visible = False
        '
        'lblTen_nh3
        '
        Me.lblTen_nh3.AutoSize = True
        Me.lblTen_nh3.Location = New System.Drawing.Point(320, 448)
        Me.lblTen_nh3.Name = "lblTen_nh3"
        Me.lblTen_nh3.Size = New System.Drawing.Size(43, 16)
        Me.lblTen_nh3.TabIndex = 48
        Me.lblTen_nh3.Tag = "L015"
        Me.lblTen_nh3.Text = "Ten_nh"
        Me.lblTen_nh3.Visible = False
        '
        'txtNh_kh3
        '
        Me.txtNh_kh3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_kh3.Location = New System.Drawing.Point(368, 82)
        Me.txtNh_kh3.Name = "txtNh_kh3"
        Me.txtNh_kh3.TabIndex = 7
        Me.txtNh_kh3.Tag = "FCML"
        Me.txtNh_kh3.Text = "TXTNH_KH3"
        '
        'txtNh_kh2
        '
        Me.txtNh_kh2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_kh2.Location = New System.Drawing.Point(264, 82)
        Me.txtNh_kh2.Name = "txtNh_kh2"
        Me.txtNh_kh2.TabIndex = 6
        Me.txtNh_kh2.Tag = "FCML"
        Me.txtNh_kh2.Text = "TXTNH_KH2"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(20, 84)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(96, 16)
        Me.Label12.TabIndex = 56
        Me.Label12.Tag = "L116"
        Me.Label12.Text = "Nhom khach hang"
        '
        'txtNh_kh1
        '
        Me.txtNh_kh1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_kh1.Location = New System.Drawing.Point(160, 82)
        Me.txtNh_kh1.Name = "txtNh_kh1"
        Me.txtNh_kh1.TabIndex = 5
        Me.txtNh_kh1.Tag = "FCML"
        Me.txtNh_kh1.Text = "TXTNH_KH1"
        '
        'frmFilter
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 489)
        Me.Controls.Add(Me.lblTen_nh3)
        Me.Controls.Add(Me.lblTen_nh2)
        Me.Controls.Add(Me.tabReports)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.lblTen_nh)
        Me.Name = "frmFilter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmFilter"
        Me.tabReports.ResumeLayout(False)
        Me.tbgFilter.ResumeLayout(False)
        Me.tbgOptions.ResumeLayout(False)
        Me.tbgOrder.ResumeLayout(False)
        CType(Me.grdOrder, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbgFree.ResumeLayout(False)
        Me.tbgTransCode.ResumeLayout(False)
        CType(Me.grdTransCode, System.ComponentModel.ISupportInitialize).EndInit()
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
                DirMain.fPrint.cmdOk.Focus()
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
                DirMain.fPrint.cmdOk.Focus()
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


    ' Properties
    Friend WithEvents CbbGroup As ComboBox
    Friend WithEvents CbbTinh_dc As ComboBox
    Friend WithEvents cboReports As ComboBox
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grdOrder As clsgrid
    Friend WithEvents grdTransCode As clsgrid
    Friend WithEvents Label1 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label15 As Label
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
    Friend WithEvents lblMa_td1 As Label
    Friend WithEvents lblMa_td2 As Label
    Friend WithEvents lblMa_td3 As Label
    Friend WithEvents lblMau_bc As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_kh As Label
    Friend WithEvents lblTen_kho As Label
    Friend WithEvents lblTen_loai As Label
    Friend WithEvents lblTen_nh As Label
    Friend WithEvents lblTen_nh2 As Label
    Friend WithEvents lblTen_nh3 As Label
    Friend WithEvents lblTen_nvbh As Label
    Friend WithEvents lblTen_nx As Label
    Friend WithEvents lblTen_td1 As Label
    Friend WithEvents lblTen_td2 As Label
    Friend WithEvents lblTen_td3 As Label
    Friend WithEvents lblTen_Tk_vt As Label
    Friend WithEvents lblTen_Tk_vt2 As Label
    Friend WithEvents lblTen_vt As Label
    Friend WithEvents lblTen_vv As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents lblTk As Label
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
    Friend WithEvents txtGroup1 As TextBox
    Friend WithEvents txtGroup2 As TextBox
    Friend WithEvents txtGroup3 As TextBox
    Friend WithEvents txtInvFrom As TextBox
    Friend WithEvents txtInvTo As TextBox
    Friend WithEvents txtLoai_vt As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_kh As TextBox
    Friend WithEvents txtMa_kho As TextBox
    Friend WithEvents txtMa_nvbh As TextBox
    Friend WithEvents txtMa_nx As TextBox
    Friend WithEvents txtMa_td1 As TextBox
    Friend WithEvents txtMa_td2 As TextBox
    Friend WithEvents txtMa_td3 As TextBox
    Friend WithEvents txtMa_vt As TextBox
    Friend WithEvents txtMa_vv As TextBox
    Friend WithEvents txtNh_vt As TextBox
    Friend WithEvents txtNh_vt2 As TextBox
    Friend WithEvents txtNh_vt3 As TextBox
    Friend WithEvents txtTitle As TextBox
    Friend WithEvents txtTk_vt As TextBox
    Friend WithEvents txtTk_vt2 As TextBox

    Private components As IContainer
    Public ds As DataSet
    Private dvOrder As DataView
    Private flag As Boolean
    Private intGroup1 As Integer
    Private intGroup2 As Integer
    Private intGroup3 As Integer
    Public pnContent As StatusBarPanel
End Class

