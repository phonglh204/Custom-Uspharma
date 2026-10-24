Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing.Printing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol
Imports libscontrol.voucherseachlib

Namespace v20sobcbhkh
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
                DirMain.dFrom = Me.txtDFrom.Value
                Reg.SetRegistryKey("DFDTo", Me.txtDTo.Value)
                Reg.SetRegistryKey("DFDFrom", Me.txtDFrom.Value)
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
            Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
            Me.CancelButton = Me.cmdCancel
            Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
            Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
            Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
            Me.txtDFrom.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
            DirMain.oAdvFilter = New clsAdvFilter(Me, Me.TabAdv, Me.tabReports, DirMain.appConn, DirMain.sysConn, Me.pnContent, Me.cmdCancel)
            DirMain.oAdvFilter.InitGridOrder(grdOrder, DirMain.SysID, "002", Me.ds, "Order")
            DirMain.oAdvFilter.AddAdvSelect(StringType.FromObject(DirMain.ReportRow.Item("cAdvtables")))
            Sql.SQLRetrieve((DirMain.appConn), "Select rp1_id, rp1_name, rp1_name2 from dmmaubc1 where rp_id = 0 order by rp1_id ", "Customer", (Me.ds))
            Me.dr = Me.ds.Tables.Item("Customer").NewRow
            Me.dr.BeginEdit()
            Me.dr.Item("rp1_id") = 0
            Me.dr.Item("rp1_name") = RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("999"))
            Me.dr.Item("rp1_name2") = RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("999"))
            Me.dr.EndEdit()
            Me.ds.Tables.Item("Customer").Rows.Add(Me.dr)

            Me.tabReports.SelectedIndex = 0
            reportformlib.grdOrderDataview = Me.grdOrder.dvGrid
            reportformlib.grdSelectDataview = DirMain.oAdvFilter.GetDataview
            DirMain.oxInv = New xInv(Me.tabReports, Me.pnContent, DirMain.appConn, DirMain.sysConn)
            Dim oCust As New DirLib(Me.txtMa_kh, Me.lblTen_kh, DirMain.sysConn, DirMain.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", True, Me.cmdCancel)
            Dim dlNh_kh1 As New DirLib(Me.txtNh_kh1, New Label, DirMain.sysConn, DirMain.appConn, "Dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=1", True, Me.cmdCancel)
            Dim dlNh_kh2 As New DirLib(Me.txtNh_kh2, New Label, DirMain.sysConn, DirMain.appConn, "Dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=2", True, Me.cmdCancel)
            Dim dlNh_kh3 As New DirLib(Me.txtNh_kh3, New Label, DirMain.sysConn, DirMain.appConn, "Dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=3", True, Me.cmdCancel)
        End Sub
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents txtInvTo As System.Windows.Forms.TextBox
        Friend WithEvents txtMa_kh As TextBox
        Friend WithEvents lblTen_kh As Label
        Friend WithEvents txtNh_kh3 As TextBox
        Friend WithEvents txtNh_kh2 As TextBox
        Friend WithEvents txtNh_kh1 As TextBox
        Friend WithEvents Label19 As Label
        Friend WithEvents txtInvFrom As System.Windows.Forms.TextBox

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
            Me.lblMa_dvcs = New System.Windows.Forms.Label()
            Me.lblTen_dvcs = New System.Windows.Forms.Label()
            Me.cmdOk = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.tabReports = New System.Windows.Forms.TabControl()
            Me.tbgFilter = New System.Windows.Forms.TabPage()
            Me.txtNh_kh3 = New System.Windows.Forms.TextBox()
            Me.txtNh_kh2 = New System.Windows.Forms.TextBox()
            Me.txtNh_kh1 = New System.Windows.Forms.TextBox()
            Me.Label19 = New System.Windows.Forms.Label()
            Me.txtMa_kh = New System.Windows.Forms.TextBox()
            Me.lblTen_kh = New System.Windows.Forms.Label()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.txtInvTo = New System.Windows.Forms.TextBox()
            Me.txtInvFrom = New System.Windows.Forms.TextBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.txtDTo = New libscontrol.txtDate()
            Me.cboReports = New System.Windows.Forms.ComboBox()
            Me.txtDFrom = New libscontrol.txtDate()
            Me.lblDateFromTo = New System.Windows.Forms.Label()
            Me.lblMau_bc = New System.Windows.Forms.Label()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.txtTitle = New System.Windows.Forms.TextBox()
            Me.TabAdv = New System.Windows.Forms.TabPage()
            Me.tbgOrder = New System.Windows.Forms.TabPage()
            Me.grdOrder = New libscontrol.clsgrid()
            Me.tabReports.SuspendLayout()
            Me.tbgFilter.SuspendLayout()
            Me.tbgOrder.SuspendLayout()
            CType(Me.grdOrder, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'txtMa_dvcs
            '
            Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New System.Drawing.Point(256, 153)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.Size = New System.Drawing.Size(160, 26)
            Me.txtMa_dvcs.TabIndex = 8
            Me.txtMa_dvcs.Tag = "FCML"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            '
            'lblMa_dvcs
            '
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New System.Drawing.Point(32, 156)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New System.Drawing.Size(53, 20)
            Me.lblMa_dvcs.TabIndex = 1
            Me.lblMa_dvcs.Tag = "L103"
            Me.lblMa_dvcs.Text = "Don vi"
            '
            'lblTen_dvcs
            '
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New System.Drawing.Point(423, 156)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New System.Drawing.Size(72, 20)
            Me.lblTen_dvcs.TabIndex = 7
            Me.lblTen_dvcs.Tag = "L002"
            Me.lblTen_dvcs.Text = "Ten dvcs"
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.Location = New System.Drawing.Point(5, 324)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.Size = New System.Drawing.Size(120, 34)
            Me.cmdOk.TabIndex = 0
            Me.cmdOk.Tag = "L001"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.Location = New System.Drawing.Point(127, 324)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(120, 34)
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
            Me.tabReports.Controls.Add(Me.TabAdv)
            Me.tabReports.Controls.Add(Me.tbgOrder)
            Me.tabReports.Location = New System.Drawing.Point(-3, 0)
            Me.tabReports.Name = "tabReports"
            Me.tabReports.SelectedIndex = 0
            Me.tabReports.Size = New System.Drawing.Size(789, 312)
            Me.tabReports.TabIndex = 0
            '
            'tbgFilter
            '
            Me.tbgFilter.Controls.Add(Me.txtNh_kh3)
            Me.tbgFilter.Controls.Add(Me.txtNh_kh2)
            Me.tbgFilter.Controls.Add(Me.txtNh_kh1)
            Me.tbgFilter.Controls.Add(Me.Label19)
            Me.tbgFilter.Controls.Add(Me.txtMa_kh)
            Me.tbgFilter.Controls.Add(Me.lblTen_kh)
            Me.tbgFilter.Controls.Add(Me.Label5)
            Me.tbgFilter.Controls.Add(Me.txtInvTo)
            Me.tbgFilter.Controls.Add(Me.txtInvFrom)
            Me.tbgFilter.Controls.Add(Me.Label1)
            Me.tbgFilter.Controls.Add(Me.txtDTo)
            Me.tbgFilter.Controls.Add(Me.cboReports)
            Me.tbgFilter.Controls.Add(Me.txtDFrom)
            Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
            Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblMau_bc)
            Me.tbgFilter.Controls.Add(Me.lblTitle)
            Me.tbgFilter.Controls.Add(Me.txtTitle)
            Me.tbgFilter.Location = New System.Drawing.Point(4, 29)
            Me.tbgFilter.Name = "tbgFilter"
            Me.tbgFilter.Size = New System.Drawing.Size(781, 279)
            Me.tbgFilter.TabIndex = 0
            Me.tbgFilter.Tag = "L100"
            Me.tbgFilter.Text = "Dieu kien loc"
            '
            'txtNh_kh3
            '
            Me.txtNh_kh3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtNh_kh3.Location = New System.Drawing.Point(589, 120)
            Me.txtNh_kh3.Name = "txtNh_kh3"
            Me.txtNh_kh3.Size = New System.Drawing.Size(160, 26)
            Me.txtNh_kh3.TabIndex = 7
            Me.txtNh_kh3.Tag = "FCML"
            Me.txtNh_kh3.Text = "TXTNH_KH3"
            '
            'txtNh_kh2
            '
            Me.txtNh_kh2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtNh_kh2.Location = New System.Drawing.Point(423, 120)
            Me.txtNh_kh2.Name = "txtNh_kh2"
            Me.txtNh_kh2.Size = New System.Drawing.Size(160, 26)
            Me.txtNh_kh2.TabIndex = 6
            Me.txtNh_kh2.Tag = "FCML"
            Me.txtNh_kh2.Text = "TXTNH_KH2"
            '
            'txtNh_kh1
            '
            Me.txtNh_kh1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtNh_kh1.Location = New System.Drawing.Point(256, 120)
            Me.txtNh_kh1.Name = "txtNh_kh1"
            Me.txtNh_kh1.Size = New System.Drawing.Size(160, 26)
            Me.txtNh_kh1.TabIndex = 5
            Me.txtNh_kh1.Tag = "FCML"
            Me.txtNh_kh1.Text = "TXTNH_KH1"
            '
            'Label19
            '
            Me.Label19.AutoSize = True
            Me.Label19.Location = New System.Drawing.Point(32, 123)
            Me.Label19.Name = "Label19"
            Me.Label19.Size = New System.Drawing.Size(98, 20)
            Me.Label19.TabIndex = 160
            Me.Label19.Tag = ""
            Me.Label19.Text = "Nhóm khách"
            '
            'txtMa_kh
            '
            Me.txtMa_kh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_kh.Location = New System.Drawing.Point(256, 87)
            Me.txtMa_kh.Name = "txtMa_kh"
            Me.txtMa_kh.Size = New System.Drawing.Size(160, 26)
            Me.txtMa_kh.TabIndex = 4
            Me.txtMa_kh.Tag = "FCML"
            Me.txtMa_kh.Text = "TXTMA_KH"
            '
            'lblTen_kh
            '
            Me.lblTen_kh.AutoSize = True
            Me.lblTen_kh.Location = New System.Drawing.Point(423, 91)
            Me.lblTen_kh.Name = "lblTen_kh"
            Me.lblTen_kh.Size = New System.Drawing.Size(77, 20)
            Me.lblTen_kh.TabIndex = 56
            Me.lblTen_kh.Tag = "L002"
            Me.lblTen_kh.Text = "lblTen_kh"
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(32, 56)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(140, 20)
            Me.Label5.TabIndex = 54
            Me.Label5.Tag = ""
            Me.Label5.Text = "Số hóa đơn từ/đến"
            Me.Label5.Visible = False
            '
            'txtInvTo
            '
            Me.txtInvTo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtInvTo.Location = New System.Drawing.Point(423, 53)
            Me.txtInvTo.MaxLength = 12
            Me.txtInvTo.Name = "txtInvTo"
            Me.txtInvTo.Size = New System.Drawing.Size(160, 26)
            Me.txtInvTo.TabIndex = 3
            Me.txtInvTo.Tag = "FCML"
            Me.txtInvTo.Text = "TXTINVTO"
            Me.txtInvTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtInvTo.Visible = False
            '
            'txtInvFrom
            '
            Me.txtInvFrom.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtInvFrom.Location = New System.Drawing.Point(256, 53)
            Me.txtInvFrom.MaxLength = 12
            Me.txtInvFrom.Name = "txtInvFrom"
            Me.txtInvFrom.Size = New System.Drawing.Size(160, 26)
            Me.txtInvFrom.TabIndex = 2
            Me.txtInvFrom.Tag = "FCML"
            Me.txtInvFrom.Text = "TXTINVFROM"
            Me.txtInvFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtInvFrom.Visible = False
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(32, 89)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(94, 20)
            Me.Label1.TabIndex = 51
            Me.Label1.Tag = "L102"
            Me.Label1.Text = "Khach hang"
            '
            'txtDTo
            '
            Me.txtDTo.Location = New System.Drawing.Point(421, 19)
            Me.txtDTo.MaxLength = 10
            Me.txtDTo.Name = "txtDTo"
            Me.txtDTo.Size = New System.Drawing.Size(160, 26)
            Me.txtDTo.TabIndex = 1
            Me.txtDTo.Tag = "NB"
            Me.txtDTo.Text = "  /  /    "
            Me.txtDTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtDTo.Value = New Date(CType(0, Long))
            '
            'cboReports
            '
            Me.cboReports.Location = New System.Drawing.Point(256, 187)
            Me.cboReports.Name = "cboReports"
            Me.cboReports.Size = New System.Drawing.Size(493, 28)
            Me.cboReports.TabIndex = 9
            Me.cboReports.Text = "cboReports"
            '
            'txtDFrom
            '
            Me.txtDFrom.Location = New System.Drawing.Point(256, 19)
            Me.txtDFrom.MaxLength = 10
            Me.txtDFrom.Name = "txtDFrom"
            Me.txtDFrom.Size = New System.Drawing.Size(160, 26)
            Me.txtDFrom.TabIndex = 0
            Me.txtDFrom.Tag = "NB"
            Me.txtDFrom.Text = "  /  /    "
            Me.txtDFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtDFrom.Value = New Date(CType(0, Long))
            '
            'lblDateFromTo
            '
            Me.lblDateFromTo.AutoSize = True
            Me.lblDateFromTo.Location = New System.Drawing.Point(32, 22)
            Me.lblDateFromTo.Name = "lblDateFromTo"
            Me.lblDateFromTo.Size = New System.Drawing.Size(96, 20)
            Me.lblDateFromTo.TabIndex = 0
            Me.lblDateFromTo.Tag = "L101"
            Me.lblDateFromTo.Text = "Tu/den ngay"
            '
            'lblMau_bc
            '
            Me.lblMau_bc.AutoSize = True
            Me.lblMau_bc.Location = New System.Drawing.Point(32, 190)
            Me.lblMau_bc.Name = "lblMau_bc"
            Me.lblMau_bc.Size = New System.Drawing.Size(101, 20)
            Me.lblMau_bc.TabIndex = 2
            Me.lblMau_bc.Tag = "L104"
            Me.lblMau_bc.Text = "Mau bao cao"
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New System.Drawing.Point(32, 225)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(61, 20)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Tag = "L105"
            Me.lblTitle.Text = "Tieu de"
            '
            'txtTitle
            '
            Me.txtTitle.Location = New System.Drawing.Point(256, 222)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New System.Drawing.Size(493, 26)
            Me.txtTitle.TabIndex = 10
            Me.txtTitle.Tag = "NB"
            Me.txtTitle.Text = "txtTieu_de"
            '
            'TabAdv
            '
            Me.TabAdv.Location = New System.Drawing.Point(4, 29)
            Me.TabAdv.Name = "TabAdv"
            Me.TabAdv.Size = New System.Drawing.Size(803, 225)
            Me.TabAdv.TabIndex = 1
            Me.TabAdv.Tag = "L400"
            Me.TabAdv.Text = "Advance filter"
            '
            'tbgOrder
            '
            Me.tbgOrder.Controls.Add(Me.grdOrder)
            Me.tbgOrder.Location = New System.Drawing.Point(4, 29)
            Me.tbgOrder.Name = "tbgOrder"
            Me.tbgOrder.Size = New System.Drawing.Size(803, 225)
            Me.tbgOrder.TabIndex = 3
            Me.tbgOrder.Tag = "L300"
            Me.tbgOrder.Text = "Thu tu sap xep"
            '
            'grdOrder
            '
            Me.grdOrder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grdOrder.CaptionVisible = False
            Me.grdOrder.Cell_EnableRaisingEvents = False
            Me.grdOrder.DataMember = ""
            Me.grdOrder.HeaderForeColor = System.Drawing.SystemColors.ControlText
            Me.grdOrder.Location = New System.Drawing.Point(0, 0)
            Me.grdOrder.Name = "grdOrder"
            Me.grdOrder.Size = New System.Drawing.Size(799, 210)
            Me.grdOrder.TabIndex = 0
            '
            'frmFilter
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(8, 19)
            Me.ClientSize = New System.Drawing.Size(789, 401)
            Me.Controls.Add(Me.tabReports)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Name = "frmFilter"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmFilter"
            Me.tabReports.ResumeLayout(False)
            Me.tbgFilter.ResumeLayout(False)
            Me.tbgFilter.PerformLayout()
            Me.tbgOrder.ResumeLayout(False)
            CType(Me.grdOrder, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents cboReports As ComboBox
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents grdOrder As clsgrid
        Friend WithEvents Label1 As Label
        Friend WithEvents lblDateFromTo As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMau_bc As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTitle As Label
        Friend WithEvents TabAdv As TabPage
        Friend WithEvents tabReports As TabControl
        Friend WithEvents tbgFilter As TabPage
        Friend WithEvents tbgOrder As TabPage
        Friend WithEvents txtDFrom As txtDate
        Friend WithEvents txtDTo As txtDate
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtTitle As TextBox

        Private components As IContainer
        Public dr As DataRow
        Public ds As DataSet
        Private dvOrder As DataView
        Private flag As Boolean
        Private intGroup1 As Integer
        Private intGroup2 As Integer
        Private intGroup3 As Integer
        Public pnContent As StatusBarPanel
    End Class
End Namespace

