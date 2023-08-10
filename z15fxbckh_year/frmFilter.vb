Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Windows.Forms
Imports libscontrol
Imports libscontrol.voucherseachlib
Imports libscommon
Imports System.Drawing.Printing

Public Class frmFilter
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
        Me.ds = New DataSet
        Me.dvOrder = New DataView
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
        Dim oControl As New TabPage
        Me.tabReports.TabPages.Add(oControl)
        reportformlib.AddFreeFields(DirMain.sysConn, oControl, 9)
        Me.tabReports.TabPages.Remove(oControl)
        reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
        Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtLoai_ts, Me.lblTen_loai_ts, DirMain.sysConn, DirMain.appConn, "dmplcc", "ma_loai", "ten_loai", "FXType", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj6 As New vouchersearchlibobj(Me.txtMa_bpsd, Me.lblTen_bpsd, DirMain.sysConn, DirMain.appConn, "dmbpcc", "ma_bp", "ten_bp", "FXDept", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtNh_ts1, Me.lblTen_nh_ts1, DirMain.sysConn, DirMain.appConn, "dmnhcc", "ma_nh", "ten_nh", "FXGroup", "loai_nh=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtNh_ts2, Me.lblTen_nh_ts2, DirMain.sysConn, DirMain.appConn, "dmnhcc", "ma_nh", "ten_nh", "FXGroup", "loai_nh=2", True, Me.cmdCancel)
        Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtNh_ts3, Me.lblTen_nh_ts3, DirMain.sysConn, DirMain.appConn, "dmnhcc", "ma_nh", "ten_nh", "FXGroup", "loai_nh=3", True, Me.cmdCancel)
        Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
        Me.CancelButton = Me.cmdCancel
        Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
        Dim document As New PrintDocument
        Me.pnContent.Text = document.PrinterSettings.PrinterName
        Me.tabReports.SelectedIndex = 0
        Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        Me.txtNam.Value = DateAndTime.Now.Year
        DirMain.oAdvFilter = New clsAdvFilter(Me, Me.tbgAdv, Me.tabReports, DirMain.appConn, DirMain.sysConn, Me.pnContent, Me.cmdCancel)
        DirMain.oAdvFilter.AddAdvSelect(StringType.FromObject(DirMain.drAdvFilter.Item("cadvtables")))
        DirMain.oAdvFilter.InitGridOrder((grdOrder), DirMain.SysID, "001", (Me.ds), "Order")
        DirMain.oAdvFilter.AddComboboxValue(Me.cbbGroup, DirMain.SysID, "002", (Me.ds), "Group")
    End Sub

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtMa_dvcs = New System.Windows.Forms.TextBox
        Me.lblMa_dvcs = New System.Windows.Forms.Label
        Me.lblTen_dvcs = New System.Windows.Forms.Label
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tabReports = New System.Windows.Forms.TabControl
        Me.tbgFilter = New System.Windows.Forms.TabPage
        Me.lblMa_kh = New System.Windows.Forms.Label
        Me.txtNam = New txtNumeric
        Me.txtNh_ts3 = New System.Windows.Forms.TextBox
        Me.lblTen_nh_ts3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblTen_bpsd = New System.Windows.Forms.Label
        Me.txtMa_bpsd = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.lblTen_loai_ts = New System.Windows.Forms.Label
        Me.txtLoai_ts = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblTen_nh_ts1 = New System.Windows.Forms.Label
        Me.txtNh_ts1 = New System.Windows.Forms.TextBox
        Me.lblTk_co = New System.Windows.Forms.Label
        Me.lblMau_bc = New System.Windows.Forms.Label
        Me.cboReports = New System.Windows.Forms.ComboBox
        Me.lblTitle = New System.Windows.Forms.Label
        Me.txtTitle = New System.Windows.Forms.TextBox
        Me.txtNh_ts2 = New System.Windows.Forms.TextBox
        Me.lblTen_nh_ts2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.tbgOptions = New System.Windows.Forms.TabPage
        Me.cbbGroup = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.tbgAdv = New System.Windows.Forms.TabPage
        Me.tbgOrder = New System.Windows.Forms.TabPage
        Me.grdOrder = New clsgrid
        Me.tabReports.SuspendLayout()
        Me.tbgFilter.SuspendLayout()
        Me.tbgOptions.SuspendLayout()
        Me.tbgOrder.SuspendLayout()
        CType(Me.grdOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtMa_dvcs
        '
        Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New System.Drawing.Point(160, 145)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.TabIndex = 9
        Me.txtMa_dvcs.Tag = "FCML"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        '
        'lblMa_dvcs
        '
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New System.Drawing.Point(20, 147)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New System.Drawing.Size(36, 16)
        Me.lblMa_dvcs.TabIndex = 1
        Me.lblMa_dvcs.Tag = "L101"
        Me.lblMa_dvcs.Text = "Don vi"
        '
        'lblTen_dvcs
        '
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New System.Drawing.Point(264, 147)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New System.Drawing.Size(50, 16)
        Me.lblTen_dvcs.TabIndex = 7
        Me.lblTen_dvcs.Tag = "L002"
        Me.lblTen_dvcs.Text = "Ten dvcs"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(3, 292)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Tag = "L001"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(79, 292)
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
        Me.tabReports.Location = New System.Drawing.Point(-2, 0)
        Me.tabReports.Name = "tabReports"
        Me.tabReports.SelectedIndex = 0
        Me.tabReports.Size = New System.Drawing.Size(609, 280)
        Me.tabReports.TabIndex = 0
        Me.tabReports.Tag = "L200"
        '
        'tbgFilter
        '
        Me.tbgFilter.Controls.Add(Me.lblMa_kh)
        Me.tbgFilter.Controls.Add(Me.txtNam)
        Me.tbgFilter.Controls.Add(Me.txtNh_ts3)
        Me.tbgFilter.Controls.Add(Me.lblTen_nh_ts3)
        Me.tbgFilter.Controls.Add(Me.Label2)
        Me.tbgFilter.Controls.Add(Me.lblTen_bpsd)
        Me.tbgFilter.Controls.Add(Me.txtMa_bpsd)
        Me.tbgFilter.Controls.Add(Me.Label9)
        Me.tbgFilter.Controls.Add(Me.lblTen_loai_ts)
        Me.tbgFilter.Controls.Add(Me.txtLoai_ts)
        Me.tbgFilter.Controls.Add(Me.Label4)
        Me.tbgFilter.Controls.Add(Me.lblTen_nh_ts1)
        Me.tbgFilter.Controls.Add(Me.txtNh_ts1)
        Me.tbgFilter.Controls.Add(Me.lblTk_co)
        Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblMau_bc)
        Me.tbgFilter.Controls.Add(Me.cboReports)
        Me.tbgFilter.Controls.Add(Me.lblTitle)
        Me.tbgFilter.Controls.Add(Me.txtTitle)
        Me.tbgFilter.Controls.Add(Me.txtNh_ts2)
        Me.tbgFilter.Controls.Add(Me.lblTen_nh_ts2)
        Me.tbgFilter.Controls.Add(Me.Label3)
        Me.tbgFilter.Location = New System.Drawing.Point(4, 22)
        Me.tbgFilter.Name = "tbgFilter"
        Me.tbgFilter.Size = New System.Drawing.Size(601, 254)
        Me.tbgFilter.TabIndex = 0
        Me.tbgFilter.Tag = "L100"
        Me.tbgFilter.Text = "Dieu kien loc"
        '
        'lblMa_kh
        '
        Me.lblMa_kh.AutoSize = True
        Me.lblMa_kh.Location = New System.Drawing.Point(20, 15)
        Me.lblMa_kh.Name = "lblMa_kh"
        Me.lblMa_kh.Size = New System.Drawing.Size(28, 16)
        Me.lblMa_kh.TabIndex = 59
        Me.lblMa_kh.Tag = "L104"
        Me.lblMa_kh.Text = "Nam"
        '
        'txtNam
        '
        Me.txtNam.Format = ""
        Me.txtNam.Location = New System.Drawing.Point(160, 13)
        Me.txtNam.MaxLength = 4
        Me.txtNam.Name = "txtNam"
        Me.txtNam.Size = New System.Drawing.Size(40, 20)
        Me.txtNam.TabIndex = 1
        Me.txtNam.Tag = "FNNBDF"
        Me.txtNam.Text = "0"
        Me.txtNam.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNam.Value = 0
        '
        'txtNh_ts3
        '
        Me.txtNh_ts3.Location = New System.Drawing.Point(160, 123)
        Me.txtNh_ts3.Name = "txtNh_ts3"
        Me.txtNh_ts3.TabIndex = 8
        Me.txtNh_ts3.Tag = "FCML"
        Me.txtNh_ts3.Text = "txtNh_ts3"
        '
        'lblTen_nh_ts3
        '
        Me.lblTen_nh_ts3.AutoSize = True
        Me.lblTen_nh_ts3.Location = New System.Drawing.Point(264, 125)
        Me.lblTen_nh_ts3.Name = "lblTen_nh_ts3"
        Me.lblTen_nh_ts3.Size = New System.Drawing.Size(101, 16)
        Me.lblTen_nh_ts3.TabIndex = 55
        Me.lblTen_nh_ts3.Tag = "RF"
        Me.lblTen_nh_ts3.Text = "Ten nhom tai san 3"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 125)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 16)
        Me.Label2.TabIndex = 54
        Me.Label2.Tag = "L109"
        Me.Label2.Text = "Nhom tai san 3"
        '
        'lblTen_bpsd
        '
        Me.lblTen_bpsd.AutoSize = True
        Me.lblTen_bpsd.Location = New System.Drawing.Point(264, 59)
        Me.lblTen_bpsd.Name = "lblTen_bpsd"
        Me.lblTen_bpsd.Size = New System.Drawing.Size(111, 16)
        Me.lblTen_bpsd.TabIndex = 52
        Me.lblTen_bpsd.Tag = "RF"
        Me.lblTen_bpsd.Text = "Ten bo phan su dung"
        '
        'txtMa_bpsd
        '
        Me.txtMa_bpsd.Location = New System.Drawing.Point(160, 57)
        Me.txtMa_bpsd.Name = "txtMa_bpsd"
        Me.txtMa_bpsd.TabIndex = 5
        Me.txtMa_bpsd.Tag = "FCML"
        Me.txtMa_bpsd.Text = "txtMa_bpsd"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(20, 59)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(89, 16)
        Me.Label9.TabIndex = 51
        Me.Label9.Tag = "L106"
        Me.Label9.Text = "Bo phan su dung"
        '
        'lblTen_loai_ts
        '
        Me.lblTen_loai_ts.AutoSize = True
        Me.lblTen_loai_ts.Location = New System.Drawing.Point(264, 37)
        Me.lblTen_loai_ts.Name = "lblTen_loai_ts"
        Me.lblTen_loai_ts.Size = New System.Drawing.Size(81, 16)
        Me.lblTen_loai_ts.TabIndex = 37
        Me.lblTen_loai_ts.Tag = "RF"
        Me.lblTen_loai_ts.Text = "Ten loai tai san"
        '
        'txtLoai_ts
        '
        Me.txtLoai_ts.Location = New System.Drawing.Point(160, 35)
        Me.txtLoai_ts.Name = "txtLoai_ts"
        Me.txtLoai_ts.TabIndex = 4
        Me.txtLoai_ts.Tag = "FCML"
        Me.txtLoai_ts.Text = "txtLoai_ts"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 16)
        Me.Label4.TabIndex = 24
        Me.Label4.Tag = "L105"
        Me.Label4.Text = "Loai tai san"
        '
        'lblTen_nh_ts1
        '
        Me.lblTen_nh_ts1.AutoSize = True
        Me.lblTen_nh_ts1.Location = New System.Drawing.Point(264, 81)
        Me.lblTen_nh_ts1.Name = "lblTen_nh_ts1"
        Me.lblTen_nh_ts1.Size = New System.Drawing.Size(101, 16)
        Me.lblTen_nh_ts1.TabIndex = 13
        Me.lblTen_nh_ts1.Tag = "RF"
        Me.lblTen_nh_ts1.Text = "Ten nhom tai san 1"
        '
        'txtNh_ts1
        '
        Me.txtNh_ts1.Location = New System.Drawing.Point(160, 79)
        Me.txtNh_ts1.Name = "txtNh_ts1"
        Me.txtNh_ts1.TabIndex = 6
        Me.txtNh_ts1.Tag = "FCML"
        Me.txtNh_ts1.Text = "txtNh_ts1"
        '
        'lblTk_co
        '
        Me.lblTk_co.AutoSize = True
        Me.lblTk_co.Location = New System.Drawing.Point(20, 81)
        Me.lblTk_co.Name = "lblTk_co"
        Me.lblTk_co.Size = New System.Drawing.Size(80, 16)
        Me.lblTk_co.TabIndex = 11
        Me.lblTk_co.Tag = "L107"
        Me.lblTk_co.Text = "Nhom tai san 1"
        '
        'lblMau_bc
        '
        Me.lblMau_bc.AutoSize = True
        Me.lblMau_bc.Location = New System.Drawing.Point(20, 169)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New System.Drawing.Size(69, 16)
        Me.lblMau_bc.TabIndex = 2
        Me.lblMau_bc.Tag = "L102"
        Me.lblMau_bc.Text = "Mau bao cao"
        '
        'cboReports
        '
        Me.cboReports.Location = New System.Drawing.Point(160, 167)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New System.Drawing.Size(300, 21)
        Me.cboReports.TabIndex = 10
        Me.cboReports.Text = "cboReports"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(20, 192)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(42, 16)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Tag = "L103"
        Me.lblTitle.Text = "Tieu de"
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(160, 190)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(300, 20)
        Me.txtTitle.TabIndex = 11
        Me.txtTitle.Tag = "NB"
        Me.txtTitle.Text = "txtTieu_de"
        '
        'txtNh_ts2
        '
        Me.txtNh_ts2.Location = New System.Drawing.Point(160, 101)
        Me.txtNh_ts2.Name = "txtNh_ts2"
        Me.txtNh_ts2.TabIndex = 7
        Me.txtNh_ts2.Tag = "FCML"
        Me.txtNh_ts2.Text = "txtNh_ts2"
        '
        'lblTen_nh_ts2
        '
        Me.lblTen_nh_ts2.AutoSize = True
        Me.lblTen_nh_ts2.Location = New System.Drawing.Point(264, 103)
        Me.lblTen_nh_ts2.Name = "lblTen_nh_ts2"
        Me.lblTen_nh_ts2.Size = New System.Drawing.Size(101, 16)
        Me.lblTen_nh_ts2.TabIndex = 40
        Me.lblTen_nh_ts2.Tag = "RF"
        Me.lblTen_nh_ts2.Text = "Ten nhom tai san 2"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 103)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 16)
        Me.Label3.TabIndex = 39
        Me.Label3.Tag = "L108"
        Me.Label3.Text = "Nhom tai san 2"
        '
        'tbgOptions
        '
        Me.tbgOptions.Controls.Add(Me.cbbGroup)
        Me.tbgOptions.Controls.Add(Me.Label5)
        Me.tbgOptions.Location = New System.Drawing.Point(4, 22)
        Me.tbgOptions.Name = "tbgOptions"
        Me.tbgOptions.Size = New System.Drawing.Size(601, 254)
        Me.tbgOptions.TabIndex = 1
        Me.tbgOptions.Tag = "L200"
        Me.tbgOptions.Text = "Lua chon"
        '
        'cbbGroup
        '
        Me.cbbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbbGroup.Location = New System.Drawing.Point(160, 8)
        Me.cbbGroup.Name = "cbbGroup"
        Me.cbbGroup.Size = New System.Drawing.Size(300, 21)
        Me.cbbGroup.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(20, 10)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 16)
        Me.Label5.TabIndex = 0
        Me.Label5.Tag = "L201"
        Me.Label5.Text = "Nhom theo"
        '
        'tbgAdv
        '
        Me.tbgAdv.Location = New System.Drawing.Point(4, 22)
        Me.tbgAdv.Name = "tbgAdv"
        Me.tbgAdv.Size = New System.Drawing.Size(601, 254)
        Me.tbgAdv.TabIndex = 4
        Me.tbgAdv.Tag = "L300"
        Me.tbgAdv.Text = "Loc chi tiet"
        '
        'tbgOrder
        '
        Me.tbgOrder.Controls.Add(Me.grdOrder)
        Me.tbgOrder.Location = New System.Drawing.Point(4, 22)
        Me.tbgOrder.Name = "tbgOrder"
        Me.tbgOrder.Size = New System.Drawing.Size(601, 254)
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
        Me.grdOrder.Size = New System.Drawing.Size(601, 256)
        Me.grdOrder.TabIndex = 1
        '
        'frmFilter
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 349)
        Me.Controls.Add(Me.tabReports)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Name = "frmFilter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmFilter"
        Me.tabReports.ResumeLayout(False)
        Me.tbgFilter.ResumeLayout(False)
        Me.tbgOptions.ResumeLayout(False)
        Me.tbgOrder.ResumeLayout(False)
        CType(Me.grdOrder, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Private Sub txtKy_Validated(ByVal sender As Object, ByVal e As EventArgs)
        If BooleanType.FromObject(ObjectType.BitOrObj((ObjectType.ObjTst(1, LateBinding.LateGet(sender, Nothing, "Value", New Object(0 - 1) {}, Nothing, Nothing), False) > 0), (ObjectType.ObjTst(LateBinding.LateGet(sender, Nothing, "Value", New Object(0 - 1) {}, Nothing, Nothing), 12, False) > 0))) Then
            LateBinding.LateCall(sender, Nothing, "Focus", New Object(0 - 1) {}, Nothing, Nothing)
        End If
    End Sub


    ' Properties
    Friend WithEvents cbbGroup As ComboBox
    Friend WithEvents cboReports As ComboBox
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grdOrder As clsgrid
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMa_kh As Label
    Friend WithEvents lblMau_bc As Label
    Friend WithEvents lblTen_bpsd As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_loai_ts As Label
    Friend WithEvents lblTen_nh_ts1 As Label
    Friend WithEvents lblTen_nh_ts2 As Label
    Friend WithEvents lblTen_nh_ts3 As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents lblTk_co As Label
    Friend WithEvents tabReports As TabControl
    Friend WithEvents tbgAdv As TabPage
    Friend WithEvents tbgFilter As TabPage
    Friend WithEvents tbgOptions As TabPage
    Friend WithEvents tbgOrder As TabPage
    Friend WithEvents txtLoai_ts As TextBox
    Friend WithEvents txtMa_bpsd As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtNam As txtNumeric
    Friend WithEvents txtNh_ts1 As TextBox
    Friend WithEvents txtNh_ts2 As TextBox
    Friend WithEvents txtNh_ts3 As TextBox
    Friend WithEvents txtTitle As TextBox

    Private components As IContainer
    Public ds As DataSet
    Private dvOrder As DataView
    Public pnContent As StatusBarPanel
End Class

