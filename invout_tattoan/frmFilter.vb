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

Namespace v20artthd
    Public Class frmFilter
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
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
                If Me.txtNgay_tt.Value < Me.txtDFrom.Value Or Me.txtNgay_tt.Value < Me.txtDTo.Value Or Me.txtNgay_tt.Value < Me.txtDReport.Value Then
                    Msg.Alert("Ngày tất toán phải sau ngày hóa đơn và ngày thanh toán")
                    Return
                End If
                DirMain.dNgay_tt = Me.txtNgay_tt.Value
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
            reportformlib.AddFreeFields(DirMain.sysConn, Me.tabReports.TabPages.Item(3), 12)
            reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
            Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtTk, Me.lblTen_tk, DirMain.sysConn, DirMain.appConn, "dmtk", "tk", "ten_tk", "Account", "tk_cn = 1", True, Me.cmdCancel)
            Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtMa_kh, Me.lblTen_kh, DirMain.sysConn, DirMain.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", DirMain.strKeyCust, True, Me.cmdCancel)
            Dim vouchersearchlibobj6 As New vouchersearchlibobj(Me.txtMa_nh1, Me.lblTen_nh1, DirMain.sysConn, DirMain.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj7 As New vouchersearchlibobj(Me.txtMa_nh2, Me.lblTen_nh2, DirMain.sysConn, DirMain.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=2", True, Me.cmdCancel)
            Dim vouchersearchlibobj8 As New vouchersearchlibobj(Me.txtMa_nh3, Me.lblTen_nh3, DirMain.sysConn, DirMain.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=3", True, Me.cmdCancel)
            Dim vouchersearchlibobj9 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtMa_td1, Me.lblTen_td1, DirMain.sysConn, DirMain.appConn, "dmtd1", "ma_td", "ten_td", "Free1", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtMa_td2, Me.lblTen_td2, DirMain.sysConn, DirMain.appConn, "dmtd2", "ma_td", "ten_td", "Free2", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtMa_td3, Me.lblTen_td3, DirMain.sysConn, DirMain.appConn, "dmtd3", "ma_td", "ten_td", "Free3", "1=1", True, Me.cmdCancel)
            Dim oType As New CharLib(Me.txtType, "0, 1")
            Dim oType1 As New CharLib(Me.txtTao_bt_yn, "0, 1")
            Me.CancelButton = Me.cmdCancel
            Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
            Me.tabReports.TabPages.Remove(Me.tbgFree)
            Me.tabReports.TabPages.Remove(Me.tbgOther)
            Me.tabReports.TabPages.Remove(Me.tbgOptions)
            Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
            Me.txtDFrom.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
            Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
            Me.txtDReport.Value = DateAndTime.Today
            Me.txtType.Text = "0"
            Me.txtTao_bt_yn.Text = "1"
            Me.txtNgay_tt.Value = DateTime.Now.Date
            Me.txtDReport.Value = Me.txtNgay_tt.Value
        End Sub

        Private Function GetEndDateOfCycle(ByVal Period As Integer, ByVal Year As Integer) As DateTime
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj("SELECT dbo.ff_GetEndDateOfCycle(", Sql.ConvertVS2SQLType(Period, "")), ", "), Sql.ConvertVS2SQLType(Year, "")), ") AS d"))
            Dim ds As New DataSet
            Sql.SQLRetrieve((DirMain.appConn), tcSQL, "xds", (ds))
            Return DateType.FromObject(ds.Tables.Item(0).Rows.Item(0).Item("d"))
        End Function

        <DebuggerStepThrough()> _
Private Sub InitializeComponent()
            Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
            Me.lblMa_dvcs = New System.Windows.Forms.Label()
            Me.lblTen_dvcs = New System.Windows.Forms.Label()
            Me.cmdOk = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.tabReports = New System.Windows.Forms.TabControl()
            Me.tbgFilter = New System.Windows.Forms.TabPage()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.txtTao_bt_yn = New System.Windows.Forms.TextBox()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.txtTTo = New libscontrol.txtNumeric()
            Me.txtTFrom = New libscontrol.txtNumeric()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.lbldFrom = New System.Windows.Forms.Label()
            Me.Label9 = New System.Windows.Forms.Label()
            Me.txtType = New System.Windows.Forms.TextBox()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.txtInvTo = New System.Windows.Forms.TextBox()
            Me.txtInvFrom = New System.Windows.Forms.TextBox()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.txtDReport = New libscontrol.txtDate()
            Me.lblTen_nh3 = New System.Windows.Forms.Label()
            Me.lblTen_nh2 = New System.Windows.Forms.Label()
            Me.lblTen_nh1 = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.txtMa_nh3 = New System.Windows.Forms.TextBox()
            Me.txtMa_nh2 = New System.Windows.Forms.TextBox()
            Me.txtMa_nh1 = New System.Windows.Forms.TextBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.lblTen_kh = New System.Windows.Forms.Label()
            Me.lblTen_tk = New System.Windows.Forms.Label()
            Me.txtMa_kh = New System.Windows.Forms.TextBox()
            Me.txtTk = New System.Windows.Forms.TextBox()
            Me.lblTk_co = New System.Windows.Forms.Label()
            Me.lblTk_no = New System.Windows.Forms.Label()
            Me.txtDTo = New libscontrol.txtDate()
            Me.txtDFrom = New libscontrol.txtDate()
            Me.lblDateFromTo = New System.Windows.Forms.Label()
            Me.lblMau_bc = New System.Windows.Forms.Label()
            Me.cboReports = New System.Windows.Forms.ComboBox()
            Me.tbgFree = New System.Windows.Forms.TabPage()
            Me.lblMa_td1 = New System.Windows.Forms.Label()
            Me.txtMa_td1 = New System.Windows.Forms.TextBox()
            Me.txtMa_td2 = New System.Windows.Forms.TextBox()
            Me.txtMa_td3 = New System.Windows.Forms.TextBox()
            Me.lblTen_td2 = New System.Windows.Forms.Label()
            Me.lblTen_td3 = New System.Windows.Forms.Label()
            Me.lblMa_td3 = New System.Windows.Forms.Label()
            Me.lblMa_td2 = New System.Windows.Forms.Label()
            Me.lblTen_td1 = New System.Windows.Forms.Label()
            Me.tbgOptions = New System.Windows.Forms.TabPage()
            Me.tbgOther = New System.Windows.Forms.TabPage()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.txtTitle = New System.Windows.Forms.TextBox()
            Me.txtNgay_tt = New libscontrol.txtDate()
            Me.tabReports.SuspendLayout()
            Me.tbgFilter.SuspendLayout()
            Me.tbgFree.SuspendLayout()
            Me.SuspendLayout()
            '
            'txtMa_dvcs
            '
            Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New System.Drawing.Point(256, 421)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.Size = New System.Drawing.Size(160, 26)
            Me.txtMa_dvcs.TabIndex = 16
            Me.txtMa_dvcs.Tag = "FCML"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            '
            'lblMa_dvcs
            '
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New System.Drawing.Point(32, 424)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New System.Drawing.Size(53, 20)
            Me.lblMa_dvcs.TabIndex = 1
            Me.lblMa_dvcs.Tag = "L102"
            Me.lblMa_dvcs.Text = "Don vi"
            '
            'lblTen_dvcs
            '
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New System.Drawing.Point(422, 424)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New System.Drawing.Size(72, 20)
            Me.lblTen_dvcs.TabIndex = 7
            Me.lblTen_dvcs.Tag = "L002"
            Me.lblTen_dvcs.Text = "Ten dvcs"
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.Location = New System.Drawing.Point(5, 366)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.Size = New System.Drawing.Size(120, 34)
            Me.cmdOk.TabIndex = 1
            Me.cmdOk.Tag = "L001"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.Location = New System.Drawing.Point(126, 366)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(120, 34)
            Me.cmdCancel.TabIndex = 2
            Me.cmdCancel.Tag = "L002"
            Me.cmdCancel.Text = "Huy"
            '
            'tabReports
            '
            Me.tabReports.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tabReports.Controls.Add(Me.tbgFilter)
            Me.tabReports.Controls.Add(Me.tbgFree)
            Me.tabReports.Controls.Add(Me.tbgOptions)
            Me.tabReports.Controls.Add(Me.tbgOther)
            Me.tabReports.Location = New System.Drawing.Point(-3, 0)
            Me.tabReports.Name = "tabReports"
            Me.tabReports.SelectedIndex = 0
            Me.tabReports.Size = New System.Drawing.Size(609, 348)
            Me.tabReports.TabIndex = 0
            Me.tabReports.Tag = ""
            '
            'tbgFilter
            '
            Me.tbgFilter.Controls.Add(Me.txtNgay_tt)
            Me.tbgFilter.Controls.Add(Me.Label7)
            Me.tbgFilter.Controls.Add(Me.txtTao_bt_yn)
            Me.tbgFilter.Controls.Add(Me.Label10)
            Me.tbgFilter.Controls.Add(Me.txtTTo)
            Me.tbgFilter.Controls.Add(Me.txtTFrom)
            Me.tbgFilter.Controls.Add(Me.Label6)
            Me.tbgFilter.Controls.Add(Me.lbldFrom)
            Me.tbgFilter.Controls.Add(Me.Label9)
            Me.tbgFilter.Controls.Add(Me.txtType)
            Me.tbgFilter.Controls.Add(Me.Label8)
            Me.tbgFilter.Controls.Add(Me.Label5)
            Me.tbgFilter.Controls.Add(Me.txtInvTo)
            Me.tbgFilter.Controls.Add(Me.txtInvFrom)
            Me.tbgFilter.Controls.Add(Me.Label4)
            Me.tbgFilter.Controls.Add(Me.txtDReport)
            Me.tbgFilter.Controls.Add(Me.lblTen_nh3)
            Me.tbgFilter.Controls.Add(Me.lblTen_nh2)
            Me.tbgFilter.Controls.Add(Me.lblTen_nh1)
            Me.tbgFilter.Controls.Add(Me.Label3)
            Me.tbgFilter.Controls.Add(Me.Label2)
            Me.tbgFilter.Controls.Add(Me.txtMa_nh3)
            Me.tbgFilter.Controls.Add(Me.txtMa_nh2)
            Me.tbgFilter.Controls.Add(Me.txtMa_nh1)
            Me.tbgFilter.Controls.Add(Me.Label1)
            Me.tbgFilter.Controls.Add(Me.lblTen_kh)
            Me.tbgFilter.Controls.Add(Me.lblTen_tk)
            Me.tbgFilter.Controls.Add(Me.txtMa_kh)
            Me.tbgFilter.Controls.Add(Me.txtTk)
            Me.tbgFilter.Controls.Add(Me.lblTk_co)
            Me.tbgFilter.Controls.Add(Me.lblTk_no)
            Me.tbgFilter.Controls.Add(Me.txtDTo)
            Me.tbgFilter.Controls.Add(Me.txtDFrom)
            Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
            Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblMau_bc)
            Me.tbgFilter.Controls.Add(Me.cboReports)
            Me.tbgFilter.Location = New System.Drawing.Point(4, 29)
            Me.tbgFilter.Name = "tbgFilter"
            Me.tbgFilter.Size = New System.Drawing.Size(601, 315)
            Me.tbgFilter.TabIndex = 0
            Me.tbgFilter.Tag = "L100"
            Me.tbgFilter.Text = "Dieu kien loc"
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(301, 392)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(181, 20)
            Me.Label7.TabIndex = 37
            Me.Label7.Tag = ""
            Me.Label7.Text = "0 - Không tạo, 1 - Có tạo"
            '
            'txtTao_bt_yn
            '
            Me.txtTao_bt_yn.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtTao_bt_yn.Location = New System.Drawing.Point(256, 389)
            Me.txtTao_bt_yn.MaxLength = 1
            Me.txtTao_bt_yn.Name = "txtTao_bt_yn"
            Me.txtTao_bt_yn.Size = New System.Drawing.Size(38, 26)
            Me.txtTao_bt_yn.TabIndex = 15
            Me.txtTao_bt_yn.Tag = "FC"
            Me.txtTao_bt_yn.Text = "TXTTYPE"
            '
            'Label10
            '
            Me.Label10.AutoSize = True
            Me.Label10.Location = New System.Drawing.Point(32, 392)
            Me.Label10.Name = "Label10"
            Me.Label10.Size = New System.Drawing.Size(142, 20)
            Me.Label10.TabIndex = 36
            Me.Label10.Tag = ""
            Me.Label10.Text = "Tạo bút toán số dư"
            '
            'txtTTo
            '
            Me.txtTTo.Format = "m_ip_tien"
            Me.txtTTo.Location = New System.Drawing.Point(422, 153)
            Me.txtTTo.MaxLength = 10
            Me.txtTTo.Name = "txtTTo"
            Me.txtTTo.Size = New System.Drawing.Size(160, 26)
            Me.txtTTo.TabIndex = 8
            Me.txtTTo.Tag = "FN"
            Me.txtTTo.Text = "m_ip_tien"
            Me.txtTTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtTTo.Value = 0R
            '
            'txtTFrom
            '
            Me.txtTFrom.Format = "m_ip_tien"
            Me.txtTFrom.Location = New System.Drawing.Point(256, 153)
            Me.txtTFrom.MaxLength = 10
            Me.txtTFrom.Name = "txtTFrom"
            Me.txtTFrom.Size = New System.Drawing.Size(160, 26)
            Me.txtTFrom.TabIndex = 7
            Me.txtTFrom.Tag = "FN"
            Me.txtTFrom.Text = "m_ip_tien"
            Me.txtTFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtTFrom.Value = 0R
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(32, 156)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(148, 20)
            Me.Label6.TabIndex = 34
            Me.Label6.Tag = "L115"
            Me.Label6.Text = "Con phai thu tu/den"
            '
            'lbldFrom
            '
            Me.lbldFrom.AutoSize = True
            Me.lbldFrom.Location = New System.Drawing.Point(32, 22)
            Me.lbldFrom.Name = "lbldFrom"
            Me.lbldFrom.Size = New System.Drawing.Size(104, 20)
            Me.lbldFrom.TabIndex = 33
            Me.lbldFrom.Tag = "L114"
            Me.lbldFrom.Text = "Ngay tat toan"
            '
            'Label9
            '
            Me.Label9.AutoSize = True
            Me.Label9.Location = New System.Drawing.Point(301, 358)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(159, 20)
            Me.Label9.TabIndex = 31
            Me.Label9.Tag = "L113"
            Me.Label9.Text = "0 - Khong in, 1 - Co in"
            '
            'txtType
            '
            Me.txtType.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtType.Location = New System.Drawing.Point(256, 355)
            Me.txtType.MaxLength = 1
            Me.txtType.Name = "txtType"
            Me.txtType.Size = New System.Drawing.Size(38, 26)
            Me.txtType.TabIndex = 14
            Me.txtType.Tag = "FC"
            Me.txtType.Text = "TXTTYPE"
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(32, 358)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(161, 20)
            Me.Label8.TabIndex = 29
            Me.Label8.Tag = "L112"
            Me.Label8.Text = "In cac HD da tat toan"
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(32, 123)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(109, 20)
            Me.Label5.TabIndex = 27
            Me.Label5.Tag = "L111"
            Me.Label5.Text = "Hoa don tu so"
            '
            'txtInvTo
            '
            Me.txtInvTo.Location = New System.Drawing.Point(422, 120)
            Me.txtInvTo.Name = "txtInvTo"
            Me.txtInvTo.Size = New System.Drawing.Size(160, 26)
            Me.txtInvTo.TabIndex = 6
            Me.txtInvTo.Tag = "FCML"
            Me.txtInvTo.Text = "txtInvTo"
            '
            'txtInvFrom
            '
            Me.txtInvFrom.Location = New System.Drawing.Point(256, 120)
            Me.txtInvFrom.Name = "txtInvFrom"
            Me.txtInvFrom.Size = New System.Drawing.Size(160, 26)
            Me.txtInvFrom.TabIndex = 5
            Me.txtInvFrom.Tag = "FCML"
            Me.txtInvFrom.Text = "txtInvFrom"
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(32, 89)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(130, 20)
            Me.Label4.TabIndex = 24
            Me.Label4.Tag = "L110"
            Me.Label4.Text = "Duoc tt den ngay"
            '
            'txtDReport
            '
            Me.txtDReport.Enabled = False
            Me.txtDReport.Location = New System.Drawing.Point(256, 86)
            Me.txtDReport.MaxLength = 10
            Me.txtDReport.Name = "txtDReport"
            Me.txtDReport.Size = New System.Drawing.Size(160, 26)
            Me.txtDReport.TabIndex = 4
            Me.txtDReport.Tag = "NB"
            Me.txtDReport.Text = "  /  /    "
            Me.txtDReport.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtDReport.Value = New Date(CType(0, Long))
            '
            'lblTen_nh3
            '
            Me.lblTen_nh3.AutoSize = True
            Me.lblTen_nh3.Location = New System.Drawing.Point(422, 324)
            Me.lblTen_nh3.Name = "lblTen_nh3"
            Me.lblTen_nh3.Size = New System.Drawing.Size(140, 20)
            Me.lblTen_nh3.TabIndex = 22
            Me.lblTen_nh3.Tag = "RF"
            Me.lblTen_nh3.Text = "Ten nhom khach 3"
            '
            'lblTen_nh2
            '
            Me.lblTen_nh2.AutoSize = True
            Me.lblTen_nh2.Location = New System.Drawing.Point(422, 291)
            Me.lblTen_nh2.Name = "lblTen_nh2"
            Me.lblTen_nh2.Size = New System.Drawing.Size(140, 20)
            Me.lblTen_nh2.TabIndex = 21
            Me.lblTen_nh2.Tag = "RF"
            Me.lblTen_nh2.Text = "Ten nhom khach 2"
            '
            'lblTen_nh1
            '
            Me.lblTen_nh1.AutoSize = True
            Me.lblTen_nh1.Location = New System.Drawing.Point(422, 257)
            Me.lblTen_nh1.Name = "lblTen_nh1"
            Me.lblTen_nh1.Size = New System.Drawing.Size(140, 20)
            Me.lblTen_nh1.TabIndex = 20
            Me.lblTen_nh1.Tag = "RF"
            Me.lblTen_nh1.Text = "Ten nhom khach 1"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(32, 324)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(111, 20)
            Me.Label3.TabIndex = 19
            Me.Label3.Tag = "L109"
            Me.Label3.Text = "Nhom khach 3"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(32, 291)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(111, 20)
            Me.Label2.TabIndex = 18
            Me.Label2.Tag = "L108"
            Me.Label2.Text = "Nhom khach 2"
            '
            'txtMa_nh3
            '
            Me.txtMa_nh3.Location = New System.Drawing.Point(256, 322)
            Me.txtMa_nh3.Name = "txtMa_nh3"
            Me.txtMa_nh3.Size = New System.Drawing.Size(160, 26)
            Me.txtMa_nh3.TabIndex = 13
            Me.txtMa_nh3.Tag = "FCML"
            Me.txtMa_nh3.Text = "txtMa_nh3"
            '
            'txtMa_nh2
            '
            Me.txtMa_nh2.Location = New System.Drawing.Point(256, 288)
            Me.txtMa_nh2.Name = "txtMa_nh2"
            Me.txtMa_nh2.Size = New System.Drawing.Size(160, 26)
            Me.txtMa_nh2.TabIndex = 12
            Me.txtMa_nh2.Tag = "FCML"
            Me.txtMa_nh2.Text = "txtMa_nh2"
            '
            'txtMa_nh1
            '
            Me.txtMa_nh1.Location = New System.Drawing.Point(256, 254)
            Me.txtMa_nh1.Name = "txtMa_nh1"
            Me.txtMa_nh1.Size = New System.Drawing.Size(160, 26)
            Me.txtMa_nh1.TabIndex = 11
            Me.txtMa_nh1.Tag = "FCML"
            Me.txtMa_nh1.Text = "txtMa_nh1"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(32, 257)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(111, 20)
            Me.Label1.TabIndex = 14
            Me.Label1.Tag = "L107"
            Me.Label1.Text = "Nhom khach 1"
            '
            'lblTen_kh
            '
            Me.lblTen_kh.AutoSize = True
            Me.lblTen_kh.Location = New System.Drawing.Point(422, 224)
            Me.lblTen_kh.Name = "lblTen_kh"
            Me.lblTen_kh.Size = New System.Drawing.Size(123, 20)
            Me.lblTen_kh.TabIndex = 13
            Me.lblTen_kh.Tag = "RF"
            Me.lblTen_kh.Text = "Ten khach hang"
            '
            'lblTen_tk
            '
            Me.lblTen_tk.AutoSize = True
            Me.lblTen_tk.Location = New System.Drawing.Point(422, 190)
            Me.lblTen_tk.Name = "lblTen_tk"
            Me.lblTen_tk.Size = New System.Drawing.Size(105, 20)
            Me.lblTen_tk.TabIndex = 12
            Me.lblTen_tk.Tag = "RF"
            Me.lblTen_tk.Text = "Ten tai khoan"
            '
            'txtMa_kh
            '
            Me.txtMa_kh.Location = New System.Drawing.Point(256, 221)
            Me.txtMa_kh.Name = "txtMa_kh"
            Me.txtMa_kh.Size = New System.Drawing.Size(160, 26)
            Me.txtMa_kh.TabIndex = 10
            Me.txtMa_kh.Tag = "FCML"
            Me.txtMa_kh.Text = "txtMa_kh"
            '
            'txtTk
            '
            Me.txtTk.Location = New System.Drawing.Point(256, 187)
            Me.txtTk.Name = "txtTk"
            Me.txtTk.Size = New System.Drawing.Size(160, 26)
            Me.txtTk.TabIndex = 9
            Me.txtTk.Tag = "FCML"
            Me.txtTk.Text = "txtTk"
            '
            'lblTk_co
            '
            Me.lblTk_co.AutoSize = True
            Me.lblTk_co.Location = New System.Drawing.Point(32, 224)
            Me.lblTk_co.Name = "lblTk_co"
            Me.lblTk_co.Size = New System.Drawing.Size(94, 20)
            Me.lblTk_co.TabIndex = 11
            Me.lblTk_co.Tag = "L106"
            Me.lblTk_co.Text = "Khach hang"
            '
            'lblTk_no
            '
            Me.lblTk_no.AutoSize = True
            Me.lblTk_no.Location = New System.Drawing.Point(32, 190)
            Me.lblTk_no.Name = "lblTk_no"
            Me.lblTk_no.Size = New System.Drawing.Size(78, 20)
            Me.lblTk_no.TabIndex = 10
            Me.lblTk_no.Tag = "L105"
            Me.lblTk_no.Text = "Tai khoan"
            '
            'txtDTo
            '
            Me.txtDTo.Location = New System.Drawing.Point(422, 53)
            Me.txtDTo.MaxLength = 10
            Me.txtDTo.Name = "txtDTo"
            Me.txtDTo.Size = New System.Drawing.Size(160, 26)
            Me.txtDTo.TabIndex = 3
            Me.txtDTo.Tag = "NB"
            Me.txtDTo.Text = "  /  /    "
            Me.txtDTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtDTo.Value = New Date(CType(0, Long))
            '
            'txtDFrom
            '
            Me.txtDFrom.Location = New System.Drawing.Point(256, 53)
            Me.txtDFrom.MaxLength = 10
            Me.txtDFrom.Name = "txtDFrom"
            Me.txtDFrom.Size = New System.Drawing.Size(160, 26)
            Me.txtDFrom.TabIndex = 2
            Me.txtDFrom.Tag = "NB"
            Me.txtDFrom.Text = "  /  /    "
            Me.txtDFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtDFrom.Value = New Date(CType(0, Long))
            '
            'lblDateFromTo
            '
            Me.lblDateFromTo.AutoSize = True
            Me.lblDateFromTo.Location = New System.Drawing.Point(32, 56)
            Me.lblDateFromTo.Name = "lblDateFromTo"
            Me.lblDateFromTo.Size = New System.Drawing.Size(120, 20)
            Me.lblDateFromTo.TabIndex = 0
            Me.lblDateFromTo.Tag = "L101"
            Me.lblDateFromTo.Text = "HD tu/den ngay"
            '
            'lblMau_bc
            '
            Me.lblMau_bc.AutoSize = True
            Me.lblMau_bc.Location = New System.Drawing.Point(32, 457)
            Me.lblMau_bc.Name = "lblMau_bc"
            Me.lblMau_bc.Size = New System.Drawing.Size(101, 20)
            Me.lblMau_bc.TabIndex = 2
            Me.lblMau_bc.Tag = "L103"
            Me.lblMau_bc.Text = "Mau bao cao"
            '
            'cboReports
            '
            Me.cboReports.Location = New System.Drawing.Point(256, 455)
            Me.cboReports.Name = "cboReports"
            Me.cboReports.Size = New System.Drawing.Size(480, 28)
            Me.cboReports.TabIndex = 17
            Me.cboReports.Text = "cboReports"
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
            Me.tbgFree.Location = New System.Drawing.Point(4, 29)
            Me.tbgFree.Name = "tbgFree"
            Me.tbgFree.Size = New System.Drawing.Size(966, 522)
            Me.tbgFree.TabIndex = 2
            Me.tbgFree.Tag = "FreeReportCaption"
            Me.tbgFree.Text = "Dieu kien ma tu do"
            '
            'lblMa_td1
            '
            Me.lblMa_td1.AutoSize = True
            Me.lblMa_td1.Location = New System.Drawing.Point(32, 23)
            Me.lblMa_td1.Name = "lblMa_td1"
            Me.lblMa_td1.Size = New System.Drawing.Size(84, 20)
            Me.lblMa_td1.TabIndex = 82
            Me.lblMa_td1.Tag = "FreeCaption1"
            Me.lblMa_td1.Text = "Ma tu do 1"
            '
            'txtMa_td1
            '
            Me.txtMa_td1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_td1.Location = New System.Drawing.Point(256, 18)
            Me.txtMa_td1.Name = "txtMa_td1"
            Me.txtMa_td1.Size = New System.Drawing.Size(160, 26)
            Me.txtMa_td1.TabIndex = 79
            Me.txtMa_td1.Tag = "FCDetail#ma_td1 like '%s%'#ML"
            Me.txtMa_td1.Text = "TXTMA_TD1"
            '
            'txtMa_td2
            '
            Me.txtMa_td2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_td2.Location = New System.Drawing.Point(256, 51)
            Me.txtMa_td2.Name = "txtMa_td2"
            Me.txtMa_td2.Size = New System.Drawing.Size(160, 26)
            Me.txtMa_td2.TabIndex = 80
            Me.txtMa_td2.Tag = "FCDetail#ma_td2 like '%s%'#ML"
            Me.txtMa_td2.Text = "TXTMA_TD2"
            '
            'txtMa_td3
            '
            Me.txtMa_td3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_td3.Location = New System.Drawing.Point(256, 85)
            Me.txtMa_td3.Name = "txtMa_td3"
            Me.txtMa_td3.Size = New System.Drawing.Size(160, 26)
            Me.txtMa_td3.TabIndex = 81
            Me.txtMa_td3.Tag = "FCDetail#ma_td3 like '%s%'#ML"
            Me.txtMa_td3.Text = "TXTMA_TD3"
            '
            'lblTen_td2
            '
            Me.lblTen_td2.AutoSize = True
            Me.lblTen_td2.Location = New System.Drawing.Point(435, 57)
            Me.lblTen_td2.Name = "lblTen_td2"
            Me.lblTen_td2.Size = New System.Drawing.Size(89, 20)
            Me.lblTen_td2.TabIndex = 86
            Me.lblTen_td2.Tag = ""
            Me.lblTen_td2.Text = "Ten tu do 2"
            '
            'lblTen_td3
            '
            Me.lblTen_td3.AutoSize = True
            Me.lblTen_td3.Location = New System.Drawing.Point(435, 91)
            Me.lblTen_td3.Name = "lblTen_td3"
            Me.lblTen_td3.Size = New System.Drawing.Size(89, 20)
            Me.lblTen_td3.TabIndex = 87
            Me.lblTen_td3.Tag = ""
            Me.lblTen_td3.Text = "Ten tu do 3"
            '
            'lblMa_td3
            '
            Me.lblMa_td3.AutoSize = True
            Me.lblMa_td3.Location = New System.Drawing.Point(32, 91)
            Me.lblMa_td3.Name = "lblMa_td3"
            Me.lblMa_td3.Size = New System.Drawing.Size(84, 20)
            Me.lblMa_td3.TabIndex = 84
            Me.lblMa_td3.Tag = "FreeCaption3"
            Me.lblMa_td3.Text = "Ma tu do 3"
            '
            'lblMa_td2
            '
            Me.lblMa_td2.AutoSize = True
            Me.lblMa_td2.Location = New System.Drawing.Point(32, 57)
            Me.lblMa_td2.Name = "lblMa_td2"
            Me.lblMa_td2.Size = New System.Drawing.Size(84, 20)
            Me.lblMa_td2.TabIndex = 83
            Me.lblMa_td2.Tag = "FreeCaption2"
            Me.lblMa_td2.Text = "Ma tu do 2"
            '
            'lblTen_td1
            '
            Me.lblTen_td1.AutoSize = True
            Me.lblTen_td1.Location = New System.Drawing.Point(435, 23)
            Me.lblTen_td1.Name = "lblTen_td1"
            Me.lblTen_td1.Size = New System.Drawing.Size(89, 20)
            Me.lblTen_td1.TabIndex = 85
            Me.lblTen_td1.Tag = ""
            Me.lblTen_td1.Text = "Ten tu do 1"
            '
            'tbgOptions
            '
            Me.tbgOptions.Location = New System.Drawing.Point(4, 29)
            Me.tbgOptions.Name = "tbgOptions"
            Me.tbgOptions.Size = New System.Drawing.Size(966, 522)
            Me.tbgOptions.TabIndex = 1
            Me.tbgOptions.Tag = "L200"
            Me.tbgOptions.Text = "Lua chon"
            '
            'tbgOther
            '
            Me.tbgOther.Location = New System.Drawing.Point(4, 29)
            Me.tbgOther.Name = "tbgOther"
            Me.tbgOther.Size = New System.Drawing.Size(966, 522)
            Me.tbgOther.TabIndex = 3
            Me.tbgOther.Tag = "FreeReportOther"
            Me.tbgOther.Text = "Dieu kien khac"
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New System.Drawing.Point(256, 526)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(61, 20)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Tag = "L104"
            Me.lblTitle.Text = "Tieu de"
            Me.lblTitle.Visible = False
            '
            'txtTitle
            '
            Me.txtTitle.Location = New System.Drawing.Point(486, 526)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New System.Drawing.Size(480, 26)
            Me.txtTitle.TabIndex = 4
            Me.txtTitle.Tag = "NB"
            Me.txtTitle.Text = "txtTieu_de"
            Me.txtTitle.Visible = False
            '
            'txtNgay_tt
            '
            Me.txtNgay_tt.Location = New System.Drawing.Point(256, 21)
            Me.txtNgay_tt.MaxLength = 10
            Me.txtNgay_tt.Name = "txtNgay_tt"
            Me.txtNgay_tt.Size = New System.Drawing.Size(160, 26)
            Me.txtNgay_tt.TabIndex = 0
            Me.txtNgay_tt.Tag = "NB"
            Me.txtNgay_tt.Text = "  /  /    "
            Me.txtNgay_tt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNgay_tt.Value = New Date(CType(0, Long))
            '
            'frmFilter
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(8, 19)
            Me.ClientSize = New System.Drawing.Size(608, 449)
            Me.Controls.Add(Me.tabReports)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.lblTitle)
            Me.Controls.Add(Me.txtTitle)
            Me.Name = "frmFilter"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmFilter"
            Me.tabReports.ResumeLayout(False)
            Me.tbgFilter.ResumeLayout(False)
            Me.tbgFilter.PerformLayout()
            Me.tbgFree.ResumeLayout(False)
            Me.tbgFree.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub




        ' Properties
        Friend WithEvents cboReports As ComboBox
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents Label1 As Label
        Friend WithEvents Label2 As Label
        Friend WithEvents Label3 As Label
        Friend WithEvents Label4 As Label
        Friend WithEvents Label5 As Label
        Friend WithEvents Label6 As Label
        Friend WithEvents Label8 As Label
        Friend WithEvents Label9 As Label
        Friend WithEvents lblDateFromTo As Label
        Friend WithEvents lbldFrom As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMa_td1 As Label
        Friend WithEvents lblMa_td2 As Label
        Friend WithEvents lblMa_td3 As Label
        Friend WithEvents lblMau_bc As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTen_kh As Label
        Friend WithEvents lblTen_nh1 As Label
        Friend WithEvents lblTen_nh2 As Label
        Friend WithEvents lblTen_nh3 As Label
        Friend WithEvents lblTen_td1 As Label
        Friend WithEvents lblTen_td2 As Label
        Friend WithEvents lblTen_td3 As Label
        Friend WithEvents lblTen_tk As Label
        Friend WithEvents lblTitle As Label
        Friend WithEvents lblTk_co As Label
        Friend WithEvents lblTk_no As Label
        Friend WithEvents tabReports As TabControl
        Friend WithEvents tbgFilter As TabPage
        Friend WithEvents tbgFree As TabPage
        Friend WithEvents tbgOptions As TabPage
        Friend WithEvents tbgOther As TabPage
        Friend WithEvents txtDFrom As txtDate
        Friend WithEvents txtDReport As txtDate
        Friend WithEvents txtDTo As txtDate
        Friend WithEvents txtInvFrom As TextBox
        Friend WithEvents txtInvTo As TextBox
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtMa_kh As TextBox
        Friend WithEvents txtMa_nh1 As TextBox
        Friend WithEvents txtMa_nh2 As TextBox
        Friend WithEvents txtMa_nh3 As TextBox
        Friend WithEvents txtMa_td1 As TextBox
        Friend WithEvents txtMa_td2 As TextBox
        Friend WithEvents txtMa_td3 As TextBox
        Friend WithEvents txtTFrom As txtNumeric
        Friend WithEvents txtTitle As TextBox
        Friend WithEvents txtTk As TextBox
        Friend WithEvents txtTTo As txtNumeric
        Friend WithEvents txtType As TextBox

        Private components As IContainer
        Private intGroup1 As Integer
        Private intGroup2 As Integer
        Private intGroup3 As Integer
        Friend WithEvents Label7 As Label
        Friend WithEvents txtTao_bt_yn As TextBox
        Friend WithEvents Label10 As Label
        Friend WithEvents txtNgay_tt As txtDate
        Public pnContent As StatusBarPanel
    End Class
End Namespace

