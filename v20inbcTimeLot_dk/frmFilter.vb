Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon
Imports libscontrol.voucherseachlib

Namespace v20inbcTimeLot
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
                If (StringType.StrCmp(StringType.FromChar(DirMain.cForm), "1", False) = 0) Then
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
                DirMain.dTo = Me.txtDTo.Value
                DirMain.strPKy = Strings.Trim(Me.txtKy.Text)
                DirMain.itg = CInt(Math.Round(Me.txtTime.Value))
                If (StringType.StrCmp(StringType.FromChar(DirMain.cForm), "1", False) = 0) Then
                    Reg.SetRegistryKey("DFDFrom", Me.txtDTo.Value)
                Else
                    Reg.SetRegistryKey("DFDTo", Me.txtDTo.Value)
                End If
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
            Dim page As New TabPage
            Me.tabReports.TabPages.Add(page)
            reportformlib.AddFreeFields(DirMain.sysConn, page, 7)
            Me.tabReports.TabPages.Remove(page)
            reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
            Dim lib1 As New CharLib(Me.txtKy, "1,2")
            Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtMa_kho, Me.lblTen_kho, DirMain.sysConn, DirMain.appConn, "dmkho", "ma_kho", "ten_kho", "Store", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj1 As New vouchersearchlibobj(Me.txtMa_vt, Me.lblTen_vt, DirMain.sysConn, DirMain.appConn, "Dmvt", "ma_vt", "ten_vt", "Item", "lo_yn = 1", True, Me.cmdCancel)
            Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtloai_vt, Me.lblTen_loai, DirMain.sysConn, DirMain.appConn, "Dmloaivt", "ma_loai_vt", "ten_loai_vt", "ItemType", "1=1", True, Me.cmdCancel)
            Me.CancelButton = Me.cmdCancel
            Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
            If (StringType.StrCmp(StringType.FromChar(DirMain.cForm), "1", False) = 0) Then
                DirMain.fPrint.Text = Strings.Trim(StringType.FromObject(DirMain.oLan.Item("901")))
                Me.txtTitle.Text = Strings.Trim(StringType.FromObject(DirMain.oLan.Item("902")))
                Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
            Else
                Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
                Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
            End If
            DirMain.oAdvFilter = New clsAdvFilter(Me, Me.TabAdv, Me.tabReports, DirMain.appConn, DirMain.sysConn, Me.pnContent, Me.cmdCancel)
            DirMain.oAdvFilter.AddAdvSelect(StringType.FromObject(DirMain.ReportRow.Item("cAdvtables")))
            DirMain.oAdvFilter.AddComboboxValue(Me.CbbGroup, DirMain.SysID, "002", (Me.ds), "Group")
            DirMain.oAdvFilter.AddComboboxValue(Me.CbbPrintAmtTotal, DirMain.SysID, "005", (Me.ds), "isTotalPrintQty")
            DirMain.oAdvFilter.InitGridOrder((grdOrder), DirMain.SysID, "001", (Me.ds), "Order")
            Me.tabReports.TabPages.Remove(Me.tbgOptions)
            Me.tabReports.TabPages.Remove(Me.tbgOrder)
            Me.tabReports.SelectedIndex = 0
            reportformlib.grdOrderDataview = Me.grdOrder.dvGrid
            reportformlib.grdSelectDataview = DirMain.oAdvFilter.GetDataview
            DirMain.oxInv = New xInv(Me.tabReports, Me.pnContent, DirMain.appConn, DirMain.sysConn)
            Me.txtTime.MaxLength = 5
            Me.txtKy.MaxLength = 1
            Me.txtKy.Text = StringType.FromInteger(DirMain.iTime)
            Me.txtSo_ky.MaxLength = 5
            Me.txtTime.Value = DirMain.iTime1
            Me.txtSo_ky.Value = DirMain.iTime2
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
            Me.txtSo_ky = New txtNumeric
            Me.Label7 = New Label
            Me.Label4 = New Label
            Me.lblTime = New Label
            Me.txtTime = New txtNumeric
            Me.cboReports = New ComboBox
            Me.Label5 = New Label
            Me.txtloai_vt = New TextBox
            Me.lblTen_loai = New Label
            Me.Label1 = New Label
            Me.txtMa_vt = New TextBox
            Me.lblTen_vt = New Label
            Me.txtDTo = New txtDate
            Me.lblDateFromTo = New Label
            Me.lblMau_bc = New Label
            Me.lblTitle = New Label
            Me.txtTitle = New TextBox
            Me.lblTen_kho = New Label
            Me.txtKy = New TextBox
            Me.txtMa_kho = New TextBox
            Me.Label3 = New Label
            Me.lblMa_kho = New Label
            Me.tbgOrder = New TabPage
            Me.grdOrder = New clsgrid
            Me.tbgOptions = New TabPage
            Me.CbbPrintAmtTotal = New ComboBox
            Me.CbbGroup = New ComboBox
            Me.Label2 = New Label
            Me.Label6 = New Label
            Me.TabAdv = New TabPage
            Me.tabReports.SuspendLayout()
            Me.tbgFilter.SuspendLayout()
            Me.tbgOrder.SuspendLayout()
            Me.grdOrder.BeginInit
            Me.tbgOptions.SuspendLayout()
            Me.SuspendLayout()
            Me.txtMa_dvcs.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New Point(160, 174)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.TabIndex = 7
            Me.txtMa_dvcs.Tag = "FCML"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New Point(20, 176)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New Size(36, 16)
            Me.lblMa_dvcs.TabIndex = 1
            Me.lblMa_dvcs.Tag = "L004"
            Me.lblMa_dvcs.Text = "Don vi"
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New Point(264, 176)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New Size(50, 16)
            Me.lblTen_dvcs.TabIndex = 7
            Me.lblTen_dvcs.Tag = "L002"
            Me.lblTen_dvcs.Text = "Ten dvcs"
            Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdOk.Location = New Point(3, 288)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 1
            Me.cmdOk.Tag = "L001"
            Me.cmdOk.Text = "Nhan"
            Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdCancel.Location = New Point(79, 288)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 2
            Me.cmdCancel.Tag = "L002"
            Me.cmdCancel.Text = "Huy"
            Me.tabReports.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.tabReports.Controls.Add(Me.tbgFilter)
            Me.tabReports.Controls.Add(Me.tbgOrder)
            Me.tabReports.Controls.Add(Me.tbgOptions)
            Me.tabReports.Controls.Add(Me.TabAdv)
            Me.tabReports.Location = New Point(-2, 0)
            Me.tabReports.Name = "tabReports"
            Me.tabReports.SelectedIndex = 0
            Me.tabReports.Size = New Size(609, 280)
            Me.tabReports.TabIndex = 1
            Me.tbgFilter.Controls.Add(Me.txtSo_ky)
            Me.tbgFilter.Controls.Add(Me.Label7)
            Me.tbgFilter.Controls.Add(Me.Label4)
            Me.tbgFilter.Controls.Add(Me.lblTime)
            Me.tbgFilter.Controls.Add(Me.txtTime)
            Me.tbgFilter.Controls.Add(Me.cboReports)
            Me.tbgFilter.Controls.Add(Me.Label5)
            Me.tbgFilter.Controls.Add(Me.txtloai_vt)
            Me.tbgFilter.Controls.Add(Me.lblTen_loai)
            Me.tbgFilter.Controls.Add(Me.Label1)
            Me.tbgFilter.Controls.Add(Me.txtMa_vt)
            Me.tbgFilter.Controls.Add(Me.lblTen_vt)
            Me.tbgFilter.Controls.Add(Me.txtDTo)
            Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
            Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblMau_bc)
            Me.tbgFilter.Controls.Add(Me.lblTitle)
            Me.tbgFilter.Controls.Add(Me.txtTitle)
            Me.tbgFilter.Controls.Add(Me.lblTen_kho)
            Me.tbgFilter.Controls.Add(Me.txtKy)
            Me.tbgFilter.Controls.Add(Me.txtMa_kho)
            Me.tbgFilter.Controls.Add(Me.Label3)
            Me.tbgFilter.Controls.Add(Me.lblMa_kho)
            Me.tbgFilter.Location = New Point(4, 22)
            Me.tbgFilter.Name = "tbgFilter"
            Me.tbgFilter.Size = New Size(601, 254)
            Me.tbgFilter.TabIndex = 0
            Me.tbgFilter.Tag = "L100"
            Me.tbgFilter.Text = "Dieu kien loc"
            Me.txtSo_ky.Format = "m_ip_tien"
            Me.txtSo_ky.Location = New Point(160, 82)
            Me.txtSo_ky.MaxLength = 10
            Me.txtSo_ky.Name = "txtSo_ky"
            Me.txtSo_ky.TabIndex = 3
            Me.txtSo_ky.Tag = "FNNB"
            Me.txtSo_ky.Text = "m_ip_tien"
            Me.txtSo_ky.TextAlign = HorizontalAlignment.Right
            Me.txtSo_ky.Value = 0
            Me.Label7.AutoSize = True
            Me.Label7.Location = New Point(20, 84)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New Size(32, 16)
            Me.Label7.TabIndex = 23
            Me.Label7.Tag = "L025"
            Me.Label7.Text = "So ky"
            Me.Label4.AutoSize = True
            Me.Label4.Location = New Point(192, 38)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New Size(98, 16)
            Me.Label4.TabIndex = 22
            Me.Label4.Tag = "L033"
            Me.Label4.Text = "1 - ngay , 2 - thang"
            Me.lblTime.AutoSize = True
            Me.lblTime.Location = New Point(20, 61)
            Me.lblTime.Name = "lblTime"
            Me.lblTime.Size = New Size(79, 16)
            Me.lblTime.TabIndex = 19
            Me.lblTime.Tag = "L020"
            Me.lblTime.Text = "chu ky tinh tuoi"
            Me.txtTime.Format = "m_ip_tien"
            Me.txtTime.Location = New Point(160, 59)
            Me.txtTime.MaxLength = 10
            Me.txtTime.Name = "txtTime"
            Me.txtTime.TabIndex = 2
            Me.txtTime.Tag = "FNNB"
            Me.txtTime.Text = "m_ip_tien"
            Me.txtTime.TextAlign = HorizontalAlignment.Right
            Me.txtTime.Value = 0
            Me.cboReports.Location = New Point(160, 197)
            Me.cboReports.Name = "cboReports"
            Me.cboReports.Size = New Size(300, 21)
            Me.cboReports.TabIndex = 8
            Me.cboReports.Text = "cboReports"
            Me.Label5.AutoSize = True
            Me.Label5.Location = New Point(20, 153)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New Size(56, 16)
            Me.Label5.TabIndex = 17
            Me.Label5.Tag = "L012"
            Me.Label5.Text = "Loai vat tu"
            Me.txtloai_vt.CharacterCasing = CharacterCasing.Upper
            Me.txtloai_vt.Location = New Point(160, 151)
            Me.txtloai_vt.Name = "txtloai_vt"
            Me.txtloai_vt.TabIndex = 6
            Me.txtloai_vt.Tag = "FCML"
            Me.txtloai_vt.Text = "TXTNH_VT"
            Me.lblTen_loai.AutoSize = True
            Me.lblTen_loai.Location = New Point(264, 153)
            Me.lblTen_loai.Name = "lblTen_loai"
            Me.lblTen_loai.Size = New Size(48, 16)
            Me.lblTen_loai.TabIndex = 18
            Me.lblTen_loai.Tag = "L015"
            Me.lblTen_loai.Text = "Ten_loai"
            Me.Label1.AutoSize = True
            Me.Label1.Location = New Point(20, 130)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New Size(50, 16)
            Me.Label1.TabIndex = 14
            Me.Label1.Tag = "L011"
            Me.Label1.Text = "Ma vat tu"
            Me.txtMa_vt.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_vt.Location = New Point(160, 128)
            Me.txtMa_vt.Name = "txtMa_vt"
            Me.txtMa_vt.TabIndex = 5
            Me.txtMa_vt.Tag = "FCML"
            Me.txtMa_vt.Text = "TXTMA_VT"
            Me.lblTen_vt.AutoSize = True
            Me.lblTen_vt.Location = New Point(264, 130)
            Me.lblTen_vt.Name = "lblTen_vt"
            Me.lblTen_vt.Size = New Size(54, 16)
            Me.lblTen_vt.TabIndex = 15
            Me.lblTen_vt.Tag = "L014"
            Me.lblTen_vt.Text = "Ten vat tu"
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
            Me.lblDateFromTo.Tag = "L003"
            Me.lblDateFromTo.Text = "Den ngay"
            Me.lblMau_bc.AutoSize = True
            Me.lblMau_bc.Location = New Point(20, 199)
            Me.lblMau_bc.Name = "lblMau_bc"
            Me.lblMau_bc.Size = New Size(69, 16)
            Me.lblMau_bc.TabIndex = 2
            Me.lblMau_bc.Tag = "L006"
            Me.lblMau_bc.Text = "Mau bao cao"
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New Point(20, 222)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New Size(42, 16)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Tag = "L007"
            Me.lblTitle.Text = "Tieu de"
            Me.txtTitle.Location = New Point(160, 220)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New Size(300, 20)
            Me.txtTitle.TabIndex = 9
            Me.txtTitle.Tag = "NB"
            Me.txtTitle.Text = "txtTieu_de"
            Me.lblTen_kho.AutoSize = True
            Me.lblTen_kho.Location = New Point(264, 107)
            Me.lblTen_kho.Name = "lblTen_kho"
            Me.lblTen_kho.Size = New Size(45, 16)
            Me.lblTen_kho.TabIndex = 12
            Me.lblTen_kho.Tag = "L002"
            Me.lblTen_kho.Text = "Ten kho"
            Me.txtKy.CharacterCasing = CharacterCasing.Upper
            Me.txtKy.Location = New Point(160, 36)
            Me.txtKy.MaxLength = 1
            Me.txtKy.Name = "txtKy"
            Me.txtKy.Size = New Size(24, 20)
            Me.txtKy.TabIndex = 1
            Me.txtKy.Tag = "FCML"
            Me.txtKy.Text = "1"
            Me.txtMa_kho.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_kho.Location = New Point(160, 105)
            Me.txtMa_kho.Name = "txtMa_kho"
            Me.txtMa_kho.TabIndex = 4
            Me.txtMa_kho.Tag = "FCML"
            Me.txtMa_kho.Text = "TXTMA_KHO"
            Me.Label3.AutoSize = True
            Me.Label3.Location = New Point(20, 38)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New Size(58, 16)
            Me.Label3.TabIndex = 20
            Me.Label3.Tag = "L022"
            Me.Label3.Text = "ky tinh tuoi"
            Me.lblMa_kho.AutoSize = True
            Me.lblMa_kho.Location = New Point(20, 107)
            Me.lblMa_kho.Name = "lblMa_kho"
            Me.lblMa_kho.Size = New Size(41, 16)
            Me.lblMa_kho.TabIndex = 10
            Me.lblMa_kho.Tag = "L005"
            Me.lblMa_kho.Text = "Ma kho"
            Me.tbgOrder.Controls.Add(Me.grdOrder)
            Me.tbgOrder.Location = New Point(4, 22)
            Me.tbgOrder.Name = "tbgOrder"
            Me.tbgOrder.Size = New Size(601, 254)
            Me.tbgOrder.TabIndex = 3
            Me.tbgOrder.Tag = "L300"
            Me.tbgOrder.Text = "Thu tu sap xep"
            Me.grdOrder.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.grdOrder.CaptionVisible = False
            Me.grdOrder.DataMember = ""
            Me.grdOrder.HeaderForeColor = SystemColors.ControlText
            Me.grdOrder.Location = New Point(0, 0)
            Me.grdOrder.Name = "grdOrder"
            Me.grdOrder.Size = New Size(601, 254)
            Me.grdOrder.TabIndex = 0
            Me.tbgOptions.Controls.Add(Me.CbbPrintAmtTotal)
            Me.tbgOptions.Controls.Add(Me.CbbGroup)
            Me.tbgOptions.Controls.Add(Me.Label2)
            Me.tbgOptions.Controls.Add(Me.Label6)
            Me.tbgOptions.Location = New Point(4, 22)
            Me.tbgOptions.Name = "tbgOptions"
            Me.tbgOptions.Size = New Size(601, 254)
            Me.tbgOptions.TabIndex = 2
            Me.tbgOptions.Tag = "L200"
            Me.tbgOptions.Text = "Lua chon"
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
            Me.CbbGroup.TabIndex = 134
            Me.Label2.AutoSize = True
            Me.Label2.Location = New Point(20, 15)
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
            Me.Label6.Visible = False
            Me.TabAdv.Location = New Point(4, 22)
            Me.TabAdv.Name = "TabAdv"
            Me.TabAdv.Size = New Size(601, 254)
            Me.TabAdv.TabIndex = 1
            Me.TabAdv.Tag = "L400"
            Me.TabAdv.Text = "Advance filter"
            Me.AutoScaleBaseSize = New Size(5, 13)
            Me.ClientSize = New Size(608, 341)
            Me.Controls.Add(Me.tabReports)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Name = "frmFilter"
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "frmFilter"
            Me.tabReports.ResumeLayout(False)
            Me.tbgFilter.ResumeLayout(False)
            Me.tbgOrder.ResumeLayout(False)
            Me.grdOrder.EndInit
            Me.tbgOptions.ResumeLayout(False)
            Me.ResumeLayout(False)
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
        Friend WithEvents Label7 As Label
        Friend WithEvents lblDateFromTo As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMa_kho As Label
        Friend WithEvents lblMau_bc As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTen_kho As Label
        Friend WithEvents lblTen_loai As Label
        Friend WithEvents lblTen_vt As Label
        Friend WithEvents lblTime As Label
        Friend WithEvents lblTitle As Label
        Friend WithEvents TabAdv As TabPage
        Friend WithEvents tabReports As TabControl
        Friend WithEvents tbgFilter As TabPage
        Friend WithEvents tbgOptions As TabPage
        Friend WithEvents tbgOrder As TabPage
        Friend WithEvents txtDTo As txtDate
        Friend WithEvents txtKy As TextBox
        Friend WithEvents txtloai_vt As TextBox
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtMa_kho As TextBox
        Friend WithEvents txtMa_vt As TextBox
        Friend WithEvents txtSo_ky As txtNumeric
        Friend WithEvents txtTime As txtNumeric
        Friend WithEvents txtTitle As TextBox

        Private components As IContainer
        Public ds As DataSet
        Private dvOrder As DataView
        Public pnContent As StatusBarPanel
    End Class
End Namespace

