﻿Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Runtime.CompilerServices
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

    Private Sub cmdOk_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Validated
        Me.flag = True
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim page As New TabPage
        Me.tabReports.TabPages.Add(page)
        reportformlib.AddFreeFields(DirMain.sysConn, page, 8)

        Me.tabReports.TabPages.Remove(page)
        reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
        Dim vouchersearchlibobj6 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj7 As New vouchersearchlibobj(Me.txtMa_kho, Me.lblTen_kho, DirMain.sysConn, DirMain.appConn, "dmkho", "ma_kho", "ten_kho", "Store", "1=1", True, Me.cmdCancel)
        Dim oNh_kho1 As New vouchersearchlibobj(Me.txtNh_kho, Me.lblTen_nh_kho, DirMain.sysConn, DirMain.appConn, "Dmnhkho", "ma_nh", "ten_nh", "SiteGroup", "1=1", True, Me.cmdCancel)

        Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtMa_vt, Me.lblTen_vt, DirMain.sysConn, DirMain.appConn, "Dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtNh_vt, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtNh_vt2, Me.lblTen_nh2, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=2", True, Me.cmdCancel)
        Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtNh_vt3, Me.lblTen_nh3, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=3", True, Me.cmdCancel)
        Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtLoai_vt, Me.lblTen_loai, DirMain.sysConn, DirMain.appConn, "Dmloaivt", "ma_loai_vt", "ten_loai_vt", "ItemType", "1=1", True, Me.cmdCancel)
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
        Me.txtDFrom.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
        Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
        DirMain.oAdvFilter = New clsAdvFilter(Me, Me.TabAdv, Me.tabReports, DirMain.appConn, DirMain.sysConn, Me.pnContent, Me.cmdCancel)
        DirMain.oAdvFilter.AddAdvSelect(StringType.FromObject(DirMain.ReportRow.Item("cAdvtables")))
        DirMain.oAdvFilter.AddComboboxValue(Me.CbbGroup, DirMain.SysID, "002", (Me.ds), "Group")
        DirMain.oAdvFilter.AddComboboxValue(Me.CbbTinh_dc, DirMain.SysID, "003", (Me.ds), "Transfer")
        DirMain.oAdvFilter.AddComboboxValue(Me.CbbPrintAmtTotal, DirMain.SysID, "005", (Me.ds), "isTotalPrintQty")
        DirMain.oAdvFilter.AddComboboxValue(Me.cbbQtycol, DirMain.SysID, "006", (Me.ds), "PrintType")
        DirMain.oAdvFilter.InitGridOrder(grdOrder, DirMain.SysID, "001", (Me.ds), "Order")
        Me.tabReports.SelectedIndex = 0
        reportformlib.grdOrderDataview = Me.grdOrder.dvGrid
        reportformlib.grdSelectDataview = DirMain.oAdvFilter.GetDataview
        DirMain.oxInv = New xInv(Me.tabReports, Me.pnContent, DirMain.appConn, DirMain.sysConn)
        Me.txtMa_dvcs.Text = "KHO"
    End Sub
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtNh_kho As System.Windows.Forms.TextBox
    Friend WithEvents lblTen_nh_kho As System.Windows.Forms.Label

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtMa_dvcs = New System.Windows.Forms.TextBox
        Me.lblMa_dvcs = New System.Windows.Forms.Label
        Me.lblTen_dvcs = New System.Windows.Forms.Label
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tabReports = New System.Windows.Forms.TabControl
        Me.tbgFilter = New System.Windows.Forms.TabPage
        Me.lblTen_nh_kho = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtNh_kho = New System.Windows.Forms.TextBox
        Me.txtNh_vt2 = New System.Windows.Forms.TextBox
        Me.txtNh_vt3 = New System.Windows.Forms.TextBox
        Me.cboReports = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtLoai_vt = New System.Windows.Forms.TextBox
        Me.lblTen_loai = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtNh_vt = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtMa_vt = New System.Windows.Forms.TextBox
        Me.lblTen_vt = New System.Windows.Forms.Label
        Me.lblMa_kho = New System.Windows.Forms.Label
        Me.txtMa_kho = New System.Windows.Forms.TextBox
        Me.lblTen_kho = New System.Windows.Forms.Label
        Me.lblDateFromTo = New System.Windows.Forms.Label
        Me.lblMau_bc = New System.Windows.Forms.Label
        Me.lblTitle = New System.Windows.Forms.Label
        Me.txtTitle = New System.Windows.Forms.TextBox
        Me.txtDTo = New txtDate
        Me.txtDFrom = New txtDate
        Me.tbgOptions = New System.Windows.Forms.TabPage
        Me.txtGroup3 = New System.Windows.Forms.TextBox
        Me.txtGroup2 = New System.Windows.Forms.TextBox
        Me.txtGroup1 = New System.Windows.Forms.TextBox
        Me.lblTk = New System.Windows.Forms.Label
        Me.cbbQtycol = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.CbbTinh_dc = New System.Windows.Forms.ComboBox
        Me.CbbPrintAmtTotal = New System.Windows.Forms.ComboBox
        Me.CbbGroup = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.TabAdv = New System.Windows.Forms.TabPage
        Me.tbgOrder = New System.Windows.Forms.TabPage
        Me.lblTen_nh = New System.Windows.Forms.Label
        Me.lblTen_nh2 = New System.Windows.Forms.Label
        Me.lblTen_nh3 = New System.Windows.Forms.Label
        Me.grdOrder = New clsgrid

        Me.tabReports.SuspendLayout()
        Me.tbgFilter.SuspendLayout()
        Me.tbgOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtMa_dvcs
        '
        Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New System.Drawing.Point(160, 150)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.TabIndex = 11
        Me.txtMa_dvcs.Tag = "FCML"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        '
        'lblMa_dvcs
        '
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New System.Drawing.Point(20, 150)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New System.Drawing.Size(36, 16)
        Me.lblMa_dvcs.TabIndex = 1
        Me.lblMa_dvcs.Tag = "L004"
        Me.lblMa_dvcs.Text = "Don vi"
        '
        'lblTen_dvcs
        '
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New System.Drawing.Point(264, 150)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New System.Drawing.Size(50, 16)
        Me.lblTen_dvcs.TabIndex = 7
        Me.lblTen_dvcs.Tag = "L002"
        Me.lblTen_dvcs.Text = "Ten dvcs"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(3, 253)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Tag = "L001"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(79, 253)
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
        Me.tabReports.Controls.Add(Me.TabAdv)
        Me.tabReports.Controls.Add(Me.tbgOrder)
        Me.tabReports.Location = New System.Drawing.Point(-2, 0)
        Me.tabReports.Name = "tabReports"
        Me.tabReports.SelectedIndex = 0
        Me.tabReports.Size = New System.Drawing.Size(609, 245)
        Me.tabReports.TabIndex = 0
        '
        'tbgFilter
        '
        Me.tbgFilter.Controls.Add(Me.lblTen_nh_kho)
        Me.tbgFilter.Controls.Add(Me.Label7)
        Me.tbgFilter.Controls.Add(Me.txtNh_kho)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt2)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt3)
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
        Me.tbgFilter.Controls.Add(Me.txtMa_kho)
        Me.tbgFilter.Controls.Add(Me.lblTen_kho)
        Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
        Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblMau_bc)
        Me.tbgFilter.Controls.Add(Me.lblTitle)
        Me.tbgFilter.Controls.Add(Me.txtTitle)
        Me.tbgFilter.Controls.Add(Me.txtDTo)
        Me.tbgFilter.Controls.Add(Me.txtDFrom)
        Me.tbgFilter.Location = New System.Drawing.Point(4, 22)
        Me.tbgFilter.Name = "tbgFilter"
        Me.tbgFilter.Size = New System.Drawing.Size(601, 219)
        Me.tbgFilter.TabIndex = 0
        Me.tbgFilter.Tag = "L100"
        Me.tbgFilter.Text = "Dieu kien loc"
        '
        'lblTen_nh_kho
        '
        Me.lblTen_nh_kho.AutoSize = True
        Me.lblTen_nh_kho.Location = New System.Drawing.Point(264, 59)
        Me.lblTen_nh_kho.Name = "lblTen_nh_kho"
        Me.lblTen_nh_kho.Size = New System.Drawing.Size(61, 16)
        Me.lblTen_nh_kho.TabIndex = 26
        Me.lblTen_nh_kho.Tag = ""
        Me.lblTen_nh_kho.Text = "Ten nh kho"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(20, 59)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(56, 16)
        Me.Label7.TabIndex = 25
        Me.Label7.Tag = "LZ01"
        Me.Label7.Text = "Nhom kho"
        '
        'txtNh_kho
        '
        Me.txtNh_kho.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_kho.Location = New System.Drawing.Point(160, 57)
        Me.txtNh_kho.Name = "txtNh_kho"
        Me.txtNh_kho.TabIndex = 3
        Me.txtNh_kho.Tag = "FCML"
        Me.txtNh_kho.Text = "TXTNH_KHO"
        '
        'txtNh_vt2
        '
        Me.txtNh_vt2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt2.Location = New System.Drawing.Point(264, 102)
        Me.txtNh_vt2.Name = "txtNh_vt2"
        Me.txtNh_vt2.TabIndex = 8
        Me.txtNh_vt2.Tag = "FCML"
        Me.txtNh_vt2.Text = "TXTNH_VT2"
        '
        'txtNh_vt3
        '
        Me.txtNh_vt3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt3.Location = New System.Drawing.Point(368, 102)
        Me.txtNh_vt3.Name = "txtNh_vt3"
        Me.txtNh_vt3.TabIndex = 9
        Me.txtNh_vt3.Tag = "FCML"
        Me.txtNh_vt3.Text = "TXTNH_VT3"
        '
        'cboReports
        '
        Me.cboReports.Location = New System.Drawing.Point(160, 174)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New System.Drawing.Size(300, 21)
        Me.cboReports.TabIndex = 12
        Me.cboReports.Text = "cboReports"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(20, 126)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 16)
        Me.Label8.TabIndex = 20
        Me.Label8.Tag = "L013"
        Me.Label8.Text = "Loai vat tu"
        '
        'txtLoai_vt
        '
        Me.txtLoai_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLoai_vt.Location = New System.Drawing.Point(160, 126)
        Me.txtLoai_vt.Name = "txtLoai_vt"
        Me.txtLoai_vt.TabIndex = 10
        Me.txtLoai_vt.Tag = "FCML"
        Me.txtLoai_vt.Text = "TXTLOAI_VT"
        '
        'lblTen_loai
        '
        Me.lblTen_loai.AutoSize = True
        Me.lblTen_loai.Location = New System.Drawing.Point(264, 126)
        Me.lblTen_loai.Name = "lblTen_loai"
        Me.lblTen_loai.Size = New System.Drawing.Size(56, 16)
        Me.lblTen_loai.TabIndex = 21
        Me.lblTen_loai.Tag = "L016"
        Me.lblTen_loai.Text = "Loai vat tu"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(20, 102)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 16)
        Me.Label5.TabIndex = 17
        Me.Label5.Tag = "L012"
        Me.Label5.Text = "Nhom vat tu"
        '
        'txtNh_vt
        '
        Me.txtNh_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt.Location = New System.Drawing.Point(160, 102)
        Me.txtNh_vt.Name = "txtNh_vt"
        Me.txtNh_vt.TabIndex = 7
        Me.txtNh_vt.Tag = "FCML"
        Me.txtNh_vt.Text = "TXTNH_VT"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 16)
        Me.Label1.TabIndex = 14
        Me.Label1.Tag = "L011"
        Me.Label1.Text = "Ma vat tu"
        '
        'txtMa_vt
        '
        Me.txtMa_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_vt.Location = New System.Drawing.Point(160, 78)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.TabIndex = 6
        Me.txtMa_vt.Tag = "FCML"
        Me.txtMa_vt.Text = "TXTMA_VT"
        '
        'lblTen_vt
        '
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New System.Drawing.Point(264, 80)
        Me.lblTen_vt.Name = "lblTen_vt"
        Me.lblTen_vt.Size = New System.Drawing.Size(54, 16)
        Me.lblTen_vt.TabIndex = 15
        Me.lblTen_vt.Tag = "L014"
        Me.lblTen_vt.Text = "Ten vat tu"
        '
        'lblMa_kho
        '
        Me.lblMa_kho.AutoSize = True
        Me.lblMa_kho.Location = New System.Drawing.Point(20, 38)
        Me.lblMa_kho.Name = "lblMa_kho"
        Me.lblMa_kho.Size = New System.Drawing.Size(41, 16)
        Me.lblMa_kho.TabIndex = 10
        Me.lblMa_kho.Tag = "L005"
        Me.lblMa_kho.Text = "Ma kho"
        '
        'txtMa_kho
        '
        Me.txtMa_kho.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kho.Location = New System.Drawing.Point(160, 36)
        Me.txtMa_kho.Name = "txtMa_kho"
        Me.txtMa_kho.TabIndex = 2
        Me.txtMa_kho.Tag = "FCML"
        Me.txtMa_kho.Text = "TXTMA_KHO"
        '
        'lblTen_kho
        '
        Me.lblTen_kho.AutoSize = True
        Me.lblTen_kho.Location = New System.Drawing.Point(264, 38)
        Me.lblTen_kho.Name = "lblTen_kho"
        Me.lblTen_kho.Size = New System.Drawing.Size(45, 16)
        Me.lblTen_kho.TabIndex = 12
        Me.lblTen_kho.Tag = "L002"
        Me.lblTen_kho.Text = "Ten kho"
        '
        'lblDateFromTo
        '
        Me.lblDateFromTo.AutoSize = True
        Me.lblDateFromTo.Location = New System.Drawing.Point(20, 16)
        Me.lblDateFromTo.Name = "lblDateFromTo"
        Me.lblDateFromTo.Size = New System.Drawing.Size(67, 16)
        Me.lblDateFromTo.TabIndex = 0
        Me.lblDateFromTo.Tag = "L003"
        Me.lblDateFromTo.Text = "Tu/den ngay"
        '
        'lblMau_bc
        '
        Me.lblMau_bc.AutoSize = True
        Me.lblMau_bc.Location = New System.Drawing.Point(20, 174)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New System.Drawing.Size(69, 16)
        Me.lblMau_bc.TabIndex = 2
        Me.lblMau_bc.Tag = "L006"
        Me.lblMau_bc.Text = "Mau bao cao"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(20, 198)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(42, 16)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Tag = "L007"
        Me.lblTitle.Text = "Tieu de"
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(160, 198)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(300, 20)
        Me.txtTitle.TabIndex = 13
        Me.txtTitle.Tag = "NB"
        Me.txtTitle.Text = "txtTieu_de"
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
        'tbgOptions
        '
        Me.tbgOptions.Controls.Add(Me.txtGroup3)
        Me.tbgOptions.Controls.Add(Me.txtGroup2)
        Me.tbgOptions.Controls.Add(Me.txtGroup1)
        Me.tbgOptions.Controls.Add(Me.lblTk)
        Me.tbgOptions.Controls.Add(Me.cbbQtycol)
        Me.tbgOptions.Controls.Add(Me.Label4)
        Me.tbgOptions.Controls.Add(Me.CbbTinh_dc)
        Me.tbgOptions.Controls.Add(Me.CbbPrintAmtTotal)
        Me.tbgOptions.Controls.Add(Me.CbbGroup)
        Me.tbgOptions.Controls.Add(Me.Label3)
        Me.tbgOptions.Controls.Add(Me.Label2)
        Me.tbgOptions.Controls.Add(Me.Label6)
        Me.tbgOptions.Location = New System.Drawing.Point(4, 22)
        Me.tbgOptions.Name = "tbgOptions"
        Me.tbgOptions.Size = New System.Drawing.Size(601, 219)
        Me.tbgOptions.TabIndex = 2
        Me.tbgOptions.Tag = "L200"
        Me.tbgOptions.Text = "Lua chon"
        '
        'txtGroup3
        '
        Me.txtGroup3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGroup3.Location = New System.Drawing.Point(214, 109)
        Me.txtGroup3.MaxLength = 1
        Me.txtGroup3.Name = "txtGroup3"
        Me.txtGroup3.Size = New System.Drawing.Size(24, 20)
        Me.txtGroup3.TabIndex = 6
        Me.txtGroup3.Tag = "FC"
        Me.txtGroup3.Text = "TXTNO_CO"
        '
        'txtGroup2
        '
        Me.txtGroup2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGroup2.Location = New System.Drawing.Point(187, 109)
        Me.txtGroup2.MaxLength = 1
        Me.txtGroup2.Name = "txtGroup2"
        Me.txtGroup2.Size = New System.Drawing.Size(24, 20)
        Me.txtGroup2.TabIndex = 5
        Me.txtGroup2.Tag = "FC"
        Me.txtGroup2.Text = "TXTNO_CO"
        '
        'txtGroup1
        '
        Me.txtGroup1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGroup1.Location = New System.Drawing.Point(160, 109)
        Me.txtGroup1.MaxLength = 1
        Me.txtGroup1.Name = "txtGroup1"
        Me.txtGroup1.Size = New System.Drawing.Size(24, 20)
        Me.txtGroup1.TabIndex = 4
        Me.txtGroup1.Tag = "FC"
        Me.txtGroup1.Text = "TXTNO_CO"
        '
        'lblTk
        '
        Me.lblTk.AutoSize = True
        Me.lblTk.Location = New System.Drawing.Point(8, 111)
        Me.lblTk.Name = "lblTk"
        Me.lblTk.Size = New System.Drawing.Size(135, 16)
        Me.lblTk.TabIndex = 151
        Me.lblTk.Tag = "LA01"
        Me.lblTk.Text = "Thu tu sap xep theo nhom"
        '
        'cbbQtycol
        '
        Me.cbbQtycol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbbQtycol.Location = New System.Drawing.Point(160, 61)
        Me.cbbQtycol.Name = "cbbQtycol"
        Me.cbbQtycol.Size = New System.Drawing.Size(300, 21)
        Me.cbbQtycol.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 63)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(99, 16)
        Me.Label4.TabIndex = 137
        Me.Label4.Tag = "L204"
        Me.Label4.Text = "In cac vat tu ton kh"
        '
        'CbbTinh_dc
        '
        Me.CbbTinh_dc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbbTinh_dc.Location = New System.Drawing.Point(160, 37)
        Me.CbbTinh_dc.Name = "CbbTinh_dc"
        Me.CbbTinh_dc.Size = New System.Drawing.Size(300, 21)
        Me.CbbTinh_dc.TabIndex = 1
        '
        'CbbPrintAmtTotal
        '
        Me.CbbPrintAmtTotal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbbPrintAmtTotal.Location = New System.Drawing.Point(160, 85)
        Me.CbbPrintAmtTotal.Name = "CbbPrintAmtTotal"
        Me.CbbPrintAmtTotal.Size = New System.Drawing.Size(300, 21)
        Me.CbbPrintAmtTotal.TabIndex = 3
        '
        'CbbGroup
        '
        Me.CbbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbbGroup.Location = New System.Drawing.Point(160, 13)
        Me.CbbGroup.Name = "CbbGroup"
        Me.CbbGroup.Size = New System.Drawing.Size(300, 21)
        Me.CbbGroup.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 39)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(139, 16)
        Me.Label3.TabIndex = 130
        Me.Label3.Tag = "L203"
        Me.Label3.Text = "Tinh phat sinh dieu chuyen"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 16)
        Me.Label2.TabIndex = 128
        Me.Label2.Tag = "L201"
        Me.Label2.Text = "Nhom theo"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 87)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 16)
        Me.Label6.TabIndex = 125
        Me.Label6.Tag = "L202"
        Me.Label6.Text = "In tong so luong"
        '
        'TabAdv
        '
        Me.TabAdv.Location = New System.Drawing.Point(4, 22)
        Me.TabAdv.Name = "TabAdv"
        Me.TabAdv.Size = New System.Drawing.Size(601, 219)
        Me.TabAdv.TabIndex = 1
        Me.TabAdv.Tag = "L400"
        Me.TabAdv.Text = "Advance filter"
        '
        'tbgOrder
        '
        Me.tbgOrder.Controls.Add(Me.grdOrder)
        Me.tbgOrder.Location = New System.Drawing.Point(4, 22)
        Me.tbgOrder.Name = "tbgOrder"
        Me.tbgOrder.Size = New System.Drawing.Size(601, 219)
        Me.tbgOrder.TabIndex = 3
        Me.tbgOrder.Tag = "L300"
        Me.tbgOrder.Text = "Thu tu sap xep"

        Me.grdOrder.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        Me.grdOrder.CaptionVisible = False
        Me.grdOrder.DataMember = ""
        Me.grdOrder.HeaderForeColor = SystemColors.ControlText
        Me.grdOrder.Location = New Point(0, 0)
        Me.grdOrder.Name = "grdOrder"
        Me.grdOrder.Size = New Size(&H259, &HCE)
        Me.grdOrder.TabIndex = 0

        '
        'lblTen_nh
        '
        Me.lblTen_nh.AutoSize = True
        Me.lblTen_nh.Location = New System.Drawing.Point(264, 248)
        Me.lblTen_nh.Name = "lblTen_nh"
        Me.lblTen_nh.Size = New System.Drawing.Size(43, 16)
        Me.lblTen_nh.TabIndex = 18
        Me.lblTen_nh.Tag = "L015"
        Me.lblTen_nh.Text = "Ten_nh"
        Me.lblTen_nh.Visible = False
        '
        'lblTen_nh2
        '
        Me.lblTen_nh2.AutoSize = True
        Me.lblTen_nh2.Location = New System.Drawing.Point(344, 256)
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
        Me.lblTen_nh3.Location = New System.Drawing.Point(400, 256)
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
        Me.ClientSize = New System.Drawing.Size(608, 306)
        Me.Controls.Add(Me.tabReports)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.lblTen_nh2)
        Me.Controls.Add(Me.lblTen_nh3)
        Me.Controls.Add(Me.lblTen_nh)
        Me.Name = "frmFilter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmFilter"
        Me.tabReports.ResumeLayout(False)
        Me.tbgFilter.ResumeLayout(False)
        Me.tbgOptions.ResumeLayout(False)
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
    Friend WithEvents Label8 As Label
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
    Friend WithEvents lblTen_vt As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents lblTk As Label
    Friend WithEvents TabAdv As TabPage
    Friend WithEvents tabReports As TabControl
    Friend WithEvents tbgFilter As TabPage
    Friend WithEvents tbgOptions As TabPage
    Friend WithEvents tbgOrder As TabPage
    Friend WithEvents txtDFrom As txtDate
    Friend WithEvents txtDTo As txtDate
    Friend WithEvents txtGroup1 As TextBox
    Friend WithEvents txtGroup2 As TextBox
    Friend WithEvents txtGroup3 As TextBox
    Friend WithEvents txtLoai_vt As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_kho As TextBox
    Friend WithEvents txtMa_vt As TextBox
    Friend WithEvents txtNh_vt As TextBox
    Friend WithEvents txtNh_vt2 As TextBox
    Friend WithEvents txtNh_vt3 As TextBox
    Friend WithEvents txtTitle As TextBox

    Private components As IContainer
    Public ds As DataSet
    Private dvOrder As DataView
    Private flag As Boolean
    Private intGroup1 As Integer
    Private intGroup2 As Integer
    Private intGroup3 As Integer
    Public pnContent As StatusBarPanel
End Class

