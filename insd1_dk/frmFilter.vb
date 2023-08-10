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
            DirMain.dTo = Me.txtDTo.Value
            Reg.SetRegistryKey("DFDTo", Me.txtDTo.Value)
            Reg.SetRegistryKey("DFItem", Strings.Trim(Me.txtMa_vt.Text))
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
        Me.txtMa_vt.Text = StringType.FromObject(Reg.GetRegistryKey("DFItem"))
        Dim oMa_vt As New DirLib(Me.txtMa_vt, Me.lblTen_vt, DirMain.sysConn, DirMain.appConn, "Dmvt", "ma_vt", "ten_vt", "Item", "1=1", False, Me.cmdCancel)
        reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
        Me.txtMa_vt.Text = StringType.FromObject(Reg.GetRegistryKey("DFItem"))
        Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtMa_kho, Me.lblTen_kho, DirMain.sysConn, DirMain.appConn, "dmkho", "ma_kho", "ten_kho", "Store", "1=1", True, Me.cmdCancel)
        Me.CancelButton = Me.cmdCancel
        Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
        Dim document As New PrintDocument
        Me.pnContent.Text = document.PrinterSettings.PrinterName
        Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
        DirMain.oAdvFilter = New clsAdvFilter(Me, Me.TabAdv, Me.tabReports, DirMain.appConn, DirMain.sysConn, Me.pnContent, Me.cmdCancel)
        DirMain.oAdvFilter.AddAdvSelect(StringType.FromObject(DirMain.ReportRow.Item("cAdvtables")))
        Me.TabAdv.Dispose()
        DirMain.oAdvFilter.InitGridOrder((grdOrder), DirMain.SysID, "001", (Me.ds), "Order")
        Me.tabReports.SelectedIndex = 0
        reportformlib.grdOrderDataview = Me.grdOrder.dvGrid
        reportformlib.grdSelectDataview = DirMain.oAdvFilter.GetDataview
        DirMain.oxInv = New xInv(Me.tabReports, Me.pnContent, DirMain.appConn, DirMain.sysConn)
    End Sub

    <DebuggerStepThrough()> _
 Private Sub InitializeComponent()
        Me.cmdOk = New Button
        Me.cmdCancel = New Button
        Me.tabReports = New TabControl
        Me.tbgFilter = New TabPage
        Me.cboReports = New ComboBox
        Me.Label1 = New Label
        Me.txtMa_vt = New TextBox
        Me.lblTen_vt = New Label
        Me.lblMa_kho = New Label
        Me.txtMa_kho = New TextBox
        Me.lblTen_kho = New Label
        Me.txtDTo = New txtDate
        Me.lblDateTo = New Label
        Me.lblMau_bc = New Label
        Me.lblTitle = New Label
        Me.txtTitle = New TextBox
        Me.tbgOrder = New TabPage
        Me.grdOrder = New clsgrid
        Me.TabAdv = New TabPage
        Me.tabReports.SuspendLayout()
        Me.tbgFilter.SuspendLayout()
        Me.tbgOrder.SuspendLayout()
        Me.grdOrder.BeginInit()
        Me.SuspendLayout()
        Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdOk.Location = New Point(3, 168)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Tag = "L001"
        Me.cmdOk.Text = "Nhan"
        Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdCancel.Location = New Point(79, 168)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Tag = "L002"
        Me.cmdCancel.Text = "Huy"
        Me.tabReports.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        Me.tabReports.Controls.Add(Me.tbgFilter)
        Me.tabReports.Controls.Add(Me.TabAdv)
        Me.tabReports.Controls.Add(Me.tbgOrder)
        Me.tabReports.Location = New Point(-2, 0)
        Me.tabReports.Name = "tabReports"
        Me.tabReports.SelectedIndex = 0
        Me.tabReports.Size = New Size(609, 160)
        Me.tabReports.TabIndex = 0
        Me.tbgFilter.Controls.Add(Me.cboReports)
        Me.tbgFilter.Controls.Add(Me.Label1)
        Me.tbgFilter.Controls.Add(Me.txtMa_vt)
        Me.tbgFilter.Controls.Add(Me.lblTen_vt)
        Me.tbgFilter.Controls.Add(Me.lblMa_kho)
        Me.tbgFilter.Controls.Add(Me.txtMa_kho)
        Me.tbgFilter.Controls.Add(Me.lblTen_kho)
        Me.tbgFilter.Controls.Add(Me.txtDTo)
        Me.tbgFilter.Controls.Add(Me.lblDateTo)
        Me.tbgFilter.Controls.Add(Me.lblMau_bc)
        Me.tbgFilter.Controls.Add(Me.lblTitle)
        Me.tbgFilter.Controls.Add(Me.txtTitle)
        Me.tbgFilter.Location = New Point(4, 22)
        Me.tbgFilter.Name = "tbgFilter"
        Me.tbgFilter.Size = New Size(601, 134)
        Me.tbgFilter.TabIndex = 0
        Me.tbgFilter.Tag = "L100"
        Me.tbgFilter.Text = "Dieu kien loc"
        Me.cboReports.Location = New Point(160, 82)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New Size(300, 21)
        Me.cboReports.TabIndex = 3
        Me.cboReports.Text = "cboReports"
        Me.Label1.AutoSize = True
        Me.Label1.Location = New Point(20, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New Size(50, 16)
        Me.Label1.TabIndex = 14
        Me.Label1.Tag = "L011"
        Me.Label1.Text = "Ma vat tu"
        Me.txtMa_vt.CharacterCasing = CharacterCasing.Upper
        Me.txtMa_vt.Location = New Point(160, 36)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.TabIndex = 1
        Me.txtMa_vt.Tag = "FCMLNB"
        Me.txtMa_vt.Text = "TXTMA_VT"
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New Point(264, 38)
        Me.lblTen_vt.Name = "lblTen_vt"
        Me.lblTen_vt.Size = New Size(54, 16)
        Me.lblTen_vt.TabIndex = 15
        Me.lblTen_vt.Tag = ""
        Me.lblTen_vt.Text = "Ten vat tu"
        Me.lblMa_kho.AutoSize = True
        Me.lblMa_kho.Location = New Point(20, 61)
        Me.lblMa_kho.Name = "lblMa_kho"
        Me.lblMa_kho.Size = New Size(41, 16)
        Me.lblMa_kho.TabIndex = 10
        Me.lblMa_kho.Tag = "L005"
        Me.lblMa_kho.Text = "Ma kho"
        Me.txtMa_kho.CharacterCasing = CharacterCasing.Upper
        Me.txtMa_kho.Location = New Point(160, 59)
        Me.txtMa_kho.Name = "txtMa_kho"
        Me.txtMa_kho.TabIndex = 2
        Me.txtMa_kho.Tag = "FCML"
        Me.txtMa_kho.Text = "TXTMA_KHO"
        Me.lblTen_kho.AutoSize = True
        Me.lblTen_kho.Location = New Point(264, 61)
        Me.lblTen_kho.Name = "lblTen_kho"
        Me.lblTen_kho.Size = New Size(45, 16)
        Me.lblTen_kho.TabIndex = 12
        Me.lblTen_kho.Tag = ""
        Me.lblTen_kho.Text = "Ten kho"
        Me.txtDTo.Location = New Point(160, 13)
        Me.txtDTo.MaxLength = 10
        Me.txtDTo.Name = "txtDTo"
        Me.txtDTo.TabIndex = 0
        Me.txtDTo.Tag = "NB"
        Me.txtDTo.Text = "  /  /    "
        Me.txtDTo.TextAlign = HorizontalAlignment.Right
        Me.txtDTo.Value = New DateTime(0)
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Location = New Point(20, 16)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New Size(53, 16)
        Me.lblDateTo.TabIndex = 0
        Me.lblDateTo.Tag = "L003"
        Me.lblDateTo.Text = "Den ngay"
        Me.lblMau_bc.AutoSize = True
        Me.lblMau_bc.Location = New Point(20, 84)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New Size(69, 16)
        Me.lblMau_bc.TabIndex = 2
        Me.lblMau_bc.Tag = "L006"
        Me.lblMau_bc.Text = "Mau bao cao"
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New Point(20, 108)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New Size(42, 16)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Tag = "L007"
        Me.lblTitle.Text = "Tieu de"
        Me.txtTitle.Location = New Point(160, 106)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New Size(300, 20)
        Me.txtTitle.TabIndex = 4
        Me.txtTitle.Tag = "NB"
        Me.txtTitle.Text = "txtTieu_de"
        Me.tbgOrder.Controls.Add(Me.grdOrder)
        Me.tbgOrder.Location = New Point(4, 22)
        Me.tbgOrder.Name = "tbgOrder"
        Me.tbgOrder.Size = New Size(601, 134)
        Me.tbgOrder.TabIndex = 3
        Me.tbgOrder.Tag = "L300"
        Me.tbgOrder.Text = "Thu tu sap xep"
        Me.grdOrder.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        Me.grdOrder.CaptionVisible = False
        Me.grdOrder.DataMember = ""
        Me.grdOrder.HeaderForeColor = SystemColors.ControlText
        Me.grdOrder.Location = New Point(0, 0)
        Me.grdOrder.Name = "grdOrder"
        Me.grdOrder.Size = New Size(601, 134)
        Me.grdOrder.TabIndex = 0
        Me.TabAdv.Location = New Point(4, 22)
        Me.TabAdv.Name = "TabAdv"
        Me.TabAdv.Size = New Size(601, 134)
        Me.TabAdv.TabIndex = 1
        Me.TabAdv.Tag = "L400"
        Me.TabAdv.Text = "Advance filter"
        Me.AutoScaleBaseSize = New Size(5, 13)
        Me.ClientSize = New Size(608, 221)
        Me.Controls.Add(Me.tabReports)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Name = "frmFilter"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "frmFilter"
        Me.tabReports.ResumeLayout(False)
        Me.tbgFilter.ResumeLayout(False)
        Me.tbgOrder.ResumeLayout(False)
        Me.grdOrder.EndInit()
        Me.ResumeLayout(False)
    End Sub

    ' Properties
    Friend WithEvents cboReports As ComboBox
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grdOrder As clsgrid
    Friend WithEvents Label1 As Label
    Friend WithEvents lblDateTo As Label
    Friend WithEvents lblMa_kho As Label
    Friend WithEvents lblMau_bc As Label
    Friend WithEvents lblTen_kho As Label
    Friend WithEvents lblTen_vt As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents TabAdv As TabPage
    Friend WithEvents tabReports As TabControl
    Friend WithEvents tbgFilter As TabPage
    Friend WithEvents tbgOrder As TabPage
    Friend WithEvents txtDTo As txtDate
    Friend WithEvents txtMa_kho As TextBox
    Friend WithEvents txtMa_vt As TextBox
    Friend WithEvents txtTitle As TextBox


    Private components As IContainer
    Public ds As DataSet
    Private dvOrder As DataView
    Public pnContent As StatusBarPanel
End Class

