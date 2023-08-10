Imports libscommon
Imports libscontrol
Imports libscontrol.clsvoucher
Imports libscontrol.voucherseachlib
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

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
            Dim args As Object() = New Object() {ObjectType.AddObj("rep_title", RuntimeHelpers.GetObjectValue(Interaction.IIf((ObjectType.ObjTst(RuntimeHelpers.GetObjectValue(Reg.GetRegistryKey("Language")), "V", False) = 0), "", "2")))}
            Me.txtTitle.Text = Strings.Trim(StringType.FromObject(RuntimeHelpers.GetObjectValue(LateBinding.LateGet(DirMain.rpTable.Rows.Item(Me.cboReports.SelectedIndex), Nothing, "Item", args, Nothing, Nothing))))
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        MyBase.Close()
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        If reportformlib.CheckEmptyField(Me, Me.tabReports, DirMain.oVar) Then
            DirMain.strUnit = Strings.Trim(Me.txtMa_dvcs.Text)
            DirMain.nNam = CInt(Math.Round(Me.txtNam.Value))
            Reg.SetRegistryKey("DFYear", Me.txtNam.Value)
            Me.pnContent.Text = StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oVar.Item("m_process")))
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
                DirMain.strGroups = Strings.Trim(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbGroup.SelectedValue)))
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
        reportformlib.AddFreeFields(DirMain.sysConn, page, 7)
        Me.tabReports.TabPages.Remove(page)
        reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
        Dim vslunit As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
        Dim vslItem As New vouchersearchlibobj(Me.TXTMA_VT, Me.lblTen_vt, DirMain.sysConn, DirMain.appConn, "Dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
        Dim vslItemGroup1 As New vouchersearchlibobj(Me.TXTNH_VT, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=1", True, Me.cmdCancel)
        Dim vslItemGroup2 As New vouchersearchlibobj(Me.TXTNH_VT2, Me.lblTen_nh2, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup2", "loai_nh=2", True, Me.cmdCancel)
        Dim vslItemGroup3 As New vouchersearchlibobj(Me.TXTNH_VT3, Me.lblTen_nh3, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup3", "loai_nh=3", True, Me.cmdCancel)
        Dim clGroup1 As New CharLib(Me.txtGroup1, "0,1,2,3")
        Dim clGroup2 As New CharLib(Me.txtGroup2, "0,1,2,3")
        Dim clGroup3 As New CharLib(Me.txtGroup3, "0,1,2,3")
        Me.txtGroup1.Text = StringType.FromInteger(0)
        Me.txtGroup2.Text = StringType.FromInteger(0)
        Me.txtGroup3.Text = StringType.FromInteger(0)
        MyBase.CancelButton = Me.cmdCancel
        Me.pnContent = clsVoucher.AddStb(Me)
        Dim document As New PrintDocument
        Me.pnContent.Text = document.PrinterSettings.PrinterName
        Dim args As Object() = New Object() {ObjectType.AddObj("rep_title", RuntimeHelpers.GetObjectValue(Interaction.IIf((ObjectType.ObjTst(RuntimeHelpers.GetObjectValue(Reg.GetRegistryKey("Language")), "V", False) = 0), "", "2")))}
        Me.txtTitle.Text = Strings.Trim(StringType.FromObject(RuntimeHelpers.GetObjectValue(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", args, Nothing, Nothing))))
        Me.txtNam.Value = Convert.ToInt32(RuntimeHelpers.GetObjectValue(Reg.GetRegistryKey("DFYear")))
        Me.txtTitle.Text = Me.txtTitle.Text.Replace("%nam%", Me.txtNam.Text)
        DirMain.oAdvFilter = New clsAdvFilter(Me, Me.TabAdv, Me.tabReports, DirMain.appConn, DirMain.sysConn, Me.pnContent, Me.cmdCancel)
        DirMain.oAdvFilter.AddAdvSelect(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cAdvtables"))))
        DirMain.oAdvFilter.AddComboboxValue(Me.CbbGroup, DirMain.SysID, "002", (Me.ds), "Group")
        DirMain.oAdvFilter.AddComboboxValue(Me.CbbPrintAmtTotal, DirMain.SysID, "005", (Me.ds), "isTotalPrintQty")
        Dim grdOrder As clsgrid = Me.grdOrder
        DirMain.oAdvFilter.InitGridOrder((grdOrder), DirMain.SysID, "001", (Me.ds), "Order")
        Me.grdOrder = grdOrder
        Me.tabReports.SelectedIndex = 0
        reportformlib.grdOrderDataview = Me.grdOrder.dvGrid
        reportformlib.grdSelectDataview = DirMain.oAdvFilter.GetDataview
        DirMain.oxInv = New xInv(Me.tabReports, Me.pnContent, DirMain.appConn, DirMain.sysConn)
        Me.tabReports.TabPages.Remove(Me.TabAdv)
        Me.tabReports.TabPages.Remove(Me.tbgOptions)
        Me.tabReports.TabPages.Remove(Me.tbgOrder)
    End Sub

    <DebuggerStepThrough>
    Private Sub InitializeComponent()
        Me.txtMa_dvcs = New TextBox
        Me.lblMa_dvcs = New Label
        Me.lblTen_dvcs = New Label
        Me.cmdOk = New Button
        Me.cmdCancel = New Button
        Me.tabReports = New TabControl
        Me.tbgFilter = New TabPage
        Me.txtNam = New txtNumeric
        Me.cboReports = New ComboBox
        Me.lblDateFromTo = New Label
        Me.lblMau_bc = New Label
        Me.lblTitle = New Label
        Me.txtTitle = New TextBox
        Me.tbgOptions = New TabPage
        Me.txtGroup3 = New TextBox
        Me.txtGroup2 = New TextBox
        Me.txtGroup1 = New TextBox
        Me.lblTk = New Label
        Me.CbbPrintAmtTotal = New ComboBox
        Me.CbbGroup = New ComboBox
        Me.Label2 = New Label
        Me.Label6 = New Label
        Me.TabAdv = New TabPage
        Me.tbgOrder = New TabPage
        Me.grdOrder = New clsgrid
        Me.Label5 = New Label
        Me.Label1 = New Label
        Me.lblTen_nh2 = New Label
        Me.lblTen_nh3 = New Label
        Me.lblTen_nh = New Label
        Me.TXTNH_VT2 = New TextBox
        Me.TXTNH_VT3 = New TextBox
        Me.Label3 = New Label
        Me.TXTNH_VT = New TextBox
        Me.Label4 = New Label
        Me.TXTMA_VT = New TextBox
        Me.lblTen_vt = New Label
        Me.tabReports.SuspendLayout()
        Me.tbgFilter.SuspendLayout()
        Me.tbgOptions.SuspendLayout()
        Me.tbgOrder.SuspendLayout()
        Me.grdOrder.BeginInit()
        Me.SuspendLayout()
        Me.txtMa_dvcs.CharacterCasing = CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New Point(160, 83)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.Size = New Size(100, 20)
        Me.txtMa_dvcs.TabIndex = 6
        Me.txtMa_dvcs.Tag = "FCML"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New Point(20, 85)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New Size(38, 13)
        Me.lblMa_dvcs.TabIndex = 1
        Me.lblMa_dvcs.Tag = "L004"
        Me.lblMa_dvcs.Text = "Don vi"
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New Point(264, 85)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New Size(52, 13)
        Me.lblTen_dvcs.TabIndex = 7
        Me.lblTen_dvcs.Tag = "L002"
        Me.lblTen_dvcs.Text = "Ten dvcs"
        Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdOk.Location = New Point(3, 217)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New Size(75, 23)
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Tag = "L001"
        Me.cmdOk.Text = "Nhan"
        Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdCancel.Location = New Point(79, 217)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New Size(75, 23)
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
        Me.tabReports.Size = New Size(609, 209)
        Me.tabReports.TabIndex = 0
        Me.tbgFilter.Controls.Add(Me.TXTNH_VT2)
        Me.tbgFilter.Controls.Add(Me.TXTNH_VT3)
        Me.tbgFilter.Controls.Add(Me.Label3)
        Me.tbgFilter.Controls.Add(Me.TXTNH_VT)
        Me.tbgFilter.Controls.Add(Me.Label4)
        Me.tbgFilter.Controls.Add(Me.TXTMA_VT)
        Me.tbgFilter.Controls.Add(Me.lblTen_vt)
        Me.tbgFilter.Controls.Add(Me.txtNam)
        Me.tbgFilter.Controls.Add(Me.cboReports)
        Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
        Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblMau_bc)
        Me.tbgFilter.Controls.Add(Me.lblTitle)
        Me.tbgFilter.Controls.Add(Me.txtTitle)
        Me.tbgFilter.Location = New Point(4, 22)
        Me.tbgFilter.Name = "tbgFilter"
        Me.tbgFilter.Size = New Size(601, 183)
        Me.tbgFilter.TabIndex = 0
        Me.tbgFilter.Tag = "L100"
        Me.tbgFilter.Text = "Dieu kien loc"
        Me.txtNam.Format = ""
        Me.txtNam.Location = New Point(160, 12)
        Me.txtNam.MaxLength = 4
        Me.txtNam.Name = "txtNam"
        Me.txtNam.Size = New Size(40, 20)
        Me.txtNam.TabIndex = 0
        Me.txtNam.Tag = "FNNBDF"
        Me.txtNam.Text = "0"
        Me.txtNam.TextAlign = HorizontalAlignment.Right
        Me.txtNam.Value = 0
        Me.cboReports.Location = New Point(160, 106)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New Size(300, 21)
        Me.cboReports.TabIndex = 7
        Me.cboReports.Text = "cboReports"
        Me.lblDateFromTo.AutoSize = True
        Me.lblDateFromTo.Location = New Point(20, 15)
        Me.lblDateFromTo.Name = "lblDateFromTo"
        Me.lblDateFromTo.Size = New Size(29, 13)
        Me.lblDateFromTo.TabIndex = 0
        Me.lblDateFromTo.Tag = "L003"
        Me.lblDateFromTo.Text = "Nam"
        Me.lblMau_bc.AutoSize = True
        Me.lblMau_bc.Location = New Point(20, 108)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New Size(70, 13)
        Me.lblMau_bc.TabIndex = 2
        Me.lblMau_bc.Tag = "L006"
        Me.lblMau_bc.Text = "Mau bao cao"
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New Point(20, 132)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New Size(43, 13)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Tag = "L007"
        Me.lblTitle.Text = "Tieu de"
        Me.txtTitle.Location = New Point(160, 130)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New Size(300, 20)
        Me.txtTitle.TabIndex = 8
        Me.txtTitle.Tag = "NB"
        Me.txtTitle.Text = "txtTieu_de"
        Me.tbgOptions.Controls.Add(Me.txtGroup3)
        Me.tbgOptions.Controls.Add(Me.txtGroup2)
        Me.tbgOptions.Controls.Add(Me.txtGroup1)
        Me.tbgOptions.Controls.Add(Me.lblTk)
        Me.tbgOptions.Controls.Add(Me.CbbPrintAmtTotal)
        Me.tbgOptions.Controls.Add(Me.CbbGroup)
        Me.tbgOptions.Controls.Add(Me.Label2)
        Me.tbgOptions.Controls.Add(Me.Label6)
        Me.tbgOptions.Location = New Point(4, 22)
        Me.tbgOptions.Name = "tbgOptions"
        Me.tbgOptions.Size = New Size(601, 183)
        Me.tbgOptions.TabIndex = 2
        Me.tbgOptions.Tag = "L200"
        Me.tbgOptions.Text = "Lua chon"
        Me.txtGroup3.CharacterCasing = CharacterCasing.Upper
        Me.txtGroup3.Location = New Point(214, 37)
        Me.txtGroup3.MaxLength = 1
        Me.txtGroup3.Name = "txtGroup3"
        Me.txtGroup3.Size = New Size(24, 20)
        Me.txtGroup3.TabIndex = 4
        Me.txtGroup3.Tag = "FC"
        Me.txtGroup3.Text = "TXTNO_CO"
        Me.txtGroup2.CharacterCasing = CharacterCasing.Upper
        Me.txtGroup2.Location = New Point(187, 37)
        Me.txtGroup2.MaxLength = 1
        Me.txtGroup2.Name = "txtGroup2"
        Me.txtGroup2.Size = New Size(24, 20)
        Me.txtGroup2.TabIndex = 2
        Me.txtGroup2.Tag = "FC"
        Me.txtGroup2.Text = "TXTNO_CO"
        Me.txtGroup1.CharacterCasing = CharacterCasing.Upper
        Me.txtGroup1.Location = New Point(160, 37)
        Me.txtGroup1.MaxLength = 1
        Me.txtGroup1.Name = "txtGroup1"
        Me.txtGroup1.Size = New Size(24, 20)
        Me.txtGroup1.TabIndex = 1
        Me.txtGroup1.Tag = "FC"
        Me.txtGroup1.Text = "TXTNO_CO"
        Me.lblTk.AutoSize = True
        Me.lblTk.Location = New Point(20, 39)
        Me.lblTk.Name = "lblTk"
        Me.lblTk.Size = New Size(131, 13)
        Me.lblTk.TabIndex = 151
        Me.lblTk.Tag = "LA01"
        Me.lblTk.Text = "Thu tu sap xep theo nhom"
        Me.CbbPrintAmtTotal.DropDownStyle = ComboBoxStyle.DropDownList
        Me.CbbPrintAmtTotal.Location = New Point(160, 85)
        Me.CbbPrintAmtTotal.Name = "CbbPrintAmtTotal"
        Me.CbbPrintAmtTotal.Size = New Size(300, 21)
        Me.CbbPrintAmtTotal.TabIndex = 135
        Me.CbbPrintAmtTotal.Visible = False
        Me.CbbGroup.DropDownStyle = ComboBoxStyle.DropDownList
        Me.CbbGroup.Location = New Point(160, 13)
        Me.CbbGroup.Name = "CbbGroup"
        Me.CbbGroup.Size = New Size(300, 21)
        Me.CbbGroup.TabIndex = 0
        Me.Label2.AutoSize = True
        Me.Label2.Location = New Point(20, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New Size(59, 13)
        Me.Label2.TabIndex = 128
        Me.Label2.Tag = "L201"
        Me.Label2.Text = "Nhom theo"
        Me.Label6.AutoSize = True
        Me.Label6.Location = New Point(8, 87)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New Size(83, 13)
        Me.Label6.TabIndex = 125
        Me.Label6.Tag = "L202"
        Me.Label6.Text = "In tong so luong"
        Me.Label6.Visible = False
        Me.TabAdv.Location = New Point(4, 22)
        Me.TabAdv.Name = "TabAdv"
        Me.TabAdv.Size = New Size(601, 183)
        Me.TabAdv.TabIndex = 1
        Me.TabAdv.Tag = "L400"
        Me.TabAdv.Text = "Advance filter"
        Me.tbgOrder.Controls.Add(Me.grdOrder)
        Me.tbgOrder.Location = New Point(4, 22)
        Me.tbgOrder.Name = "tbgOrder"
        Me.tbgOrder.Size = New Size(601, 183)
        Me.tbgOrder.TabIndex = 3
        Me.tbgOrder.Tag = "L300"
        Me.tbgOrder.Text = "Thu tu sap xep"
        Me.grdOrder.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        Me.grdOrder.CaptionVisible = False
        Me.grdOrder.Cell_EnableRaisingEvents = False
        Me.grdOrder.DataMember = ""
        Me.grdOrder.HeaderForeColor = SystemColors.ControlText
        Me.grdOrder.Location = New Point(0, 0)
        Me.grdOrder.Name = "grdOrder"
        Me.grdOrder.Size = New Size(601, 183)
        Me.grdOrder.TabIndex = 0
        Me.Label5.AutoSize = True
        Me.Label5.Enabled = False
        Me.Label5.Location = New Point(178, 242)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New Size(65, 13)
        Me.Label5.TabIndex = 17
        Me.Label5.Tag = "L012"
        Me.Label5.Text = "Nhom vat tu"
        Me.Label5.Visible = False
        Me.Label1.AutoSize = True
        Me.Label1.Enabled = False
        Me.Label1.Location = New Point(178, 219)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New Size(52, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Tag = "L011"
        Me.Label1.Text = "Ma vat tu"
        Me.Label1.Visible = False
        Me.lblTen_nh2.AutoSize = True
        Me.lblTen_nh2.Location = New Point(304, 240)
        Me.lblTen_nh2.Name = "lblTen_nh2"
        Me.lblTen_nh2.Size = New Size(44, 13)
        Me.lblTen_nh2.TabIndex = 56
        Me.lblTen_nh2.Tag = "L015"
        Me.lblTen_nh2.Text = "Ten_nh"
        Me.lblTen_nh2.Visible = False
        Me.lblTen_nh3.AutoSize = True
        Me.lblTen_nh3.Location = New Point(360, 240)
        Me.lblTen_nh3.Name = "lblTen_nh3"
        Me.lblTen_nh3.Size = New Size(44, 13)
        Me.lblTen_nh3.TabIndex = 57
        Me.lblTen_nh3.Tag = "L015"
        Me.lblTen_nh3.Text = "Ten_nh"
        Me.lblTen_nh3.Visible = False
        Me.lblTen_nh.AutoSize = True
        Me.lblTen_nh.Location = New Point(232, 224)
        Me.lblTen_nh.Name = "lblTen_nh"
        Me.lblTen_nh.Size = New Size(44, 13)
        Me.lblTen_nh.TabIndex = 18
        Me.lblTen_nh.Tag = "L015"
        Me.lblTen_nh.Text = "Ten_nh"
        Me.lblTen_nh.Visible = False
        Me.TXTNH_VT2.CharacterCasing = CharacterCasing.Upper
        Me.TXTNH_VT2.Location = New Point(264, 59)
        Me.TXTNH_VT2.Name = "TXTNH_VT2"
        Me.TXTNH_VT2.Size = New Size(100, 20)
        Me.TXTNH_VT2.TabIndex = 3
        Me.TXTNH_VT2.Tag = "FCML"
        Me.TXTNH_VT2.Text = "TXTNH_VT2"
        Me.TXTNH_VT3.CharacterCasing = CharacterCasing.Upper
        Me.TXTNH_VT3.Location = New Point(368, 59)
        Me.TXTNH_VT3.Name = "TXTNH_VT3"
        Me.TXTNH_VT3.Size = New Size(100, 20)
        Me.TXTNH_VT3.TabIndex = 4
        Me.TXTNH_VT3.Tag = "FCML"
        Me.TXTNH_VT3.Text = "TXTNH_VT3"
        Me.Label3.AutoSize = True
        Me.Label3.Location = New Point(20, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New Size(65, 13)
        Me.Label3.TabIndex = 24
        Me.Label3.Tag = "L012"
        Me.Label3.Text = "Nhom vat tu"
        Me.TXTNH_VT.CharacterCasing = CharacterCasing.Upper
        Me.TXTNH_VT.Location = New Point(160, 59)
        Me.TXTNH_VT.Name = "TXTNH_VT"
        Me.TXTNH_VT.Size = New Size(100, 20)
        Me.TXTNH_VT.TabIndex = 2
        Me.TXTNH_VT.Tag = "FCML"
        Me.TXTNH_VT.Text = "TXTNH_VT"
        Me.Label4.AutoSize = True
        Me.Label4.Location = New Point(20, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New Size(52, 13)
        Me.Label4.TabIndex = 22
        Me.Label4.Tag = "L011"
        Me.Label4.Text = "Ma vat tu"
        Me.TXTMA_VT.CharacterCasing = CharacterCasing.Upper
        Me.TXTMA_VT.Location = New Point(160, 36)
        Me.TXTMA_VT.Name = "TXTMA_VT"
        Me.TXTMA_VT.Size = New Size(100, 20)
        Me.TXTMA_VT.TabIndex = 1
        Me.TXTMA_VT.Tag = "FCML"
        Me.TXTMA_VT.Text = "TXTMA_VT"
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New Point(264, 38)
        Me.lblTen_vt.Name = "lblTen_vt"
        Me.lblTen_vt.Size = New Size(56, 13)
        Me.lblTen_vt.TabIndex = 23
        Me.lblTen_vt.Tag = "L014"
        Me.lblTen_vt.Text = "Ten vat tu"
        Me.AutoScaleBaseSize = New Size(5, 13)
        Me.ClientSize = New Size(608, 270)
        Me.Controls.Add(Me.tabReports)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.lblTen_nh2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblTen_nh3)
        Me.Controls.Add(Me.lblTen_nh)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmFilter"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "frmFilter"
        Me.tabReports.ResumeLayout(False)
        Me.tbgFilter.ResumeLayout(False)
        Me.tbgFilter.PerformLayout()
        Me.tbgOptions.ResumeLayout(False)
        Me.tbgOptions.PerformLayout()
        Me.tbgOrder.ResumeLayout(False)
        Me.grdOrder.EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub






    Private Sub txtGroup1_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup1.Enter
        Me.flag = False
    End Sub

    Private Sub txtGroup1_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup1.Validated
        Me.intGroup1 = CInt(Math.Round(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup1.Text))))
        Me.intGroup2 = CInt(Math.Round(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup2.Text))))
        Me.intGroup3 = CInt(Math.Round(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup3.Text))))
        If (((Me.intGroup2 + Me.intGroup3) > 0) And (((Me.intGroup1 = Me.intGroup2) Or (Me.intGroup1 = Me.intGroup3)) Or (Me.intGroup1 = 0))) Then
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
        Me.intGroup1 = CInt(Math.Round(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup1.Text))))
        Me.intGroup2 = CInt(Math.Round(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup2.Text))))
        Me.intGroup3 = CInt(Math.Round(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup3.Text))))
        If (((Me.intGroup2 + Me.intGroup3) > 0) And (((Me.intGroup2 = Me.intGroup1) Or (Me.intGroup2 = Me.intGroup3)) Or (Me.intGroup2 = 0))) Then
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
        Me.intGroup1 = CInt(Math.Round(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup1.Text))))
        Me.intGroup2 = CInt(Math.Round(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup2.Text))))
        Me.intGroup3 = CInt(Math.Round(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup3.Text))))
        If ((Me.intGroup3 > 0) And ((Me.intGroup3 = Me.intGroup2) Or (Me.intGroup3 = Me.intGroup1))) Then
            DirMain.fPrint.txtGroup3.Focus()
        End If
    End Sub

    Private Sub txtNam_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles txtNam.Leave
        Dim args As Object() = New Object() {ObjectType.AddObj("rep_title", RuntimeHelpers.GetObjectValue(Interaction.IIf((ObjectType.ObjTst(RuntimeHelpers.GetObjectValue(Reg.GetRegistryKey("Language")), "V", False) = 0), "", "2")))}
        Dim str As String = Strings.Trim(StringType.FromObject(RuntimeHelpers.GetObjectValue(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", args, Nothing, Nothing)))).Replace("%nam%", Me.txtNam.Text)
        Me.txtTitle.Text = str
    End Sub


    ' Properties
    Friend WithEvents CbbGroup As ComboBox
    Friend WithEvents CbbPrintAmtTotal As ComboBox
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
    Friend WithEvents lblDateFromTo As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMau_bc As Label
    Friend WithEvents lblTen_dvcs As Label
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
    Friend WithEvents txtGroup1 As TextBox
    Friend WithEvents txtGroup2 As TextBox
    Friend WithEvents txtGroup3 As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents TXTMA_VT As TextBox
    Friend WithEvents txtNam As txtNumeric
    Friend WithEvents TXTNH_VT As TextBox
    Friend WithEvents TXTNH_VT2 As TextBox
    Friend WithEvents TXTNH_VT3 As TextBox
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

