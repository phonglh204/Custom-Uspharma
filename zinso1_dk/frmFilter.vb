Imports libscommon
Imports libscontrol

Public Class frmFilter
    Inherits System.Windows.Forms.Form
    Public pnContent As StatusBarPanel
    Public ds As New DataSet
    Friend WithEvents Label3 As Label
    Friend WithEvents txtMa_lo As TextBox
    Friend WithEvents lblTen_lo As Label
    Dim dvOrder As New DataView
    Dim oLot As dirkeylib
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents cmdOk As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents tabReports As System.Windows.Forms.TabControl
    Friend WithEvents tbgFilter As System.Windows.Forms.TabPage
    Friend WithEvents lblMau_bc As System.Windows.Forms.Label
    Friend WithEvents cboReports As System.Windows.Forms.ComboBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents lblDateFromTo As System.Windows.Forms.Label
    Friend WithEvents txtDFrom As txtDate
    Friend WithEvents txtDTo As txtDate
    Friend WithEvents lblMa_kho As System.Windows.Forms.Label
    Friend WithEvents txtMa_kho As System.Windows.Forms.TextBox
    Friend WithEvents lblTen_kho As System.Windows.Forms.Label
    Friend WithEvents TabAdv As System.Windows.Forms.TabPage
    Friend WithEvents tbgOrder As System.Windows.Forms.TabPage
    Friend WithEvents grdOrder As clsgrid
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtMa_vt As System.Windows.Forms.TextBox
    Friend WithEvents lblTen_vt As System.Windows.Forms.Label
    Friend WithEvents txtR_title As System.Windows.Forms.TextBox
    Friend WithEvents txtR_title2 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.tabReports = New System.Windows.Forms.TabControl()
        Me.tbgFilter = New System.Windows.Forms.TabPage()
        Me.txtR_title2 = New System.Windows.Forms.TextBox()
        Me.txtR_title = New System.Windows.Forms.TextBox()
        Me.cboReports = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMa_vt = New System.Windows.Forms.TextBox()
        Me.lblTen_vt = New System.Windows.Forms.Label()
        Me.lblMa_kho = New System.Windows.Forms.Label()
        Me.txtMa_kho = New System.Windows.Forms.TextBox()
        Me.lblTen_kho = New System.Windows.Forms.Label()
        Me.txtDTo = New libscontrol.txtDate()
        Me.txtDFrom = New libscontrol.txtDate()
        Me.lblDateFromTo = New System.Windows.Forms.Label()
        Me.lblMau_bc = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.TabAdv = New System.Windows.Forms.TabPage()
        Me.tbgOrder = New System.Windows.Forms.TabPage()
        Me.grdOrder = New libscontrol.clsgrid()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtMa_lo = New System.Windows.Forms.TextBox()
        Me.lblTen_lo = New System.Windows.Forms.Label()
        Me.tabReports.SuspendLayout()
        Me.tbgFilter.SuspendLayout()
        Me.tbgOrder.SuspendLayout()
        CType(Me.grdOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(3, 196)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 23)
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Tag = "L001"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(79, 196)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
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
        Me.tabReports.Location = New System.Drawing.Point(-2, 0)
        Me.tabReports.Name = "tabReports"
        Me.tabReports.SelectedIndex = 0
        Me.tabReports.Size = New System.Drawing.Size(609, 188)
        Me.tabReports.TabIndex = 0
        '
        'tbgFilter
        '
        Me.tbgFilter.Controls.Add(Me.Label3)
        Me.tbgFilter.Controls.Add(Me.txtMa_lo)
        Me.tbgFilter.Controls.Add(Me.lblTen_lo)
        Me.tbgFilter.Controls.Add(Me.txtR_title2)
        Me.tbgFilter.Controls.Add(Me.txtR_title)
        Me.tbgFilter.Controls.Add(Me.cboReports)
        Me.tbgFilter.Controls.Add(Me.Label1)
        Me.tbgFilter.Controls.Add(Me.txtMa_vt)
        Me.tbgFilter.Controls.Add(Me.lblTen_vt)
        Me.tbgFilter.Controls.Add(Me.lblMa_kho)
        Me.tbgFilter.Controls.Add(Me.txtMa_kho)
        Me.tbgFilter.Controls.Add(Me.lblTen_kho)
        Me.tbgFilter.Controls.Add(Me.txtDTo)
        Me.tbgFilter.Controls.Add(Me.txtDFrom)
        Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
        Me.tbgFilter.Controls.Add(Me.lblMau_bc)
        Me.tbgFilter.Controls.Add(Me.lblTitle)
        Me.tbgFilter.Controls.Add(Me.txtTitle)
        Me.tbgFilter.Location = New System.Drawing.Point(4, 22)
        Me.tbgFilter.Name = "tbgFilter"
        Me.tbgFilter.Size = New System.Drawing.Size(601, 162)
        Me.tbgFilter.TabIndex = 0
        Me.tbgFilter.Tag = "L100"
        Me.tbgFilter.Text = "Dieu kien loc"
        '
        'txtR_title2
        '
        Me.txtR_title2.Location = New System.Drawing.Point(496, 56)
        Me.txtR_title2.Name = "txtR_title2"
        Me.txtR_title2.Size = New System.Drawing.Size(100, 20)
        Me.txtR_title2.TabIndex = 17
        Me.txtR_title2.Tag = "FC"
        Me.txtR_title2.Text = "txtR_title2"
        Me.txtR_title2.Visible = False
        '
        'txtR_title
        '
        Me.txtR_title.Location = New System.Drawing.Point(496, 32)
        Me.txtR_title.Name = "txtR_title"
        Me.txtR_title.Size = New System.Drawing.Size(100, 20)
        Me.txtR_title.TabIndex = 16
        Me.txtR_title.Tag = "FC"
        Me.txtR_title.Text = "txtR_title"
        Me.txtR_title.Visible = False
        '
        'cboReports
        '
        Me.cboReports.Location = New System.Drawing.Point(160, 107)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New System.Drawing.Size(300, 21)
        Me.cboReports.TabIndex = 7
        Me.cboReports.Text = "cboReports"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Tag = "L011"
        Me.Label1.Text = "Ma vat tu"
        '
        'txtMa_vt
        '
        Me.txtMa_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_vt.Location = New System.Drawing.Point(160, 59)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_vt.TabIndex = 3
        Me.txtMa_vt.Tag = "FCMLNB"
        Me.txtMa_vt.Text = "TXTMA_VT"
        '
        'lblTen_vt
        '
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New System.Drawing.Point(264, 61)
        Me.lblTen_vt.Name = "lblTen_vt"
        Me.lblTen_vt.Size = New System.Drawing.Size(56, 13)
        Me.lblTen_vt.TabIndex = 15
        Me.lblTen_vt.Tag = ""
        Me.lblTen_vt.Text = "Ten vat tu"
        '
        'lblMa_kho
        '
        Me.lblMa_kho.AutoSize = True
        Me.lblMa_kho.Location = New System.Drawing.Point(20, 38)
        Me.lblMa_kho.Name = "lblMa_kho"
        Me.lblMa_kho.Size = New System.Drawing.Size(43, 13)
        Me.lblMa_kho.TabIndex = 10
        Me.lblMa_kho.Tag = "L005"
        Me.lblMa_kho.Text = "Ma kho"
        '
        'txtMa_kho
        '
        Me.txtMa_kho.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kho.Location = New System.Drawing.Point(160, 36)
        Me.txtMa_kho.Name = "txtMa_kho"
        Me.txtMa_kho.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_kho.TabIndex = 2
        Me.txtMa_kho.Tag = "FCMLNB"
        Me.txtMa_kho.Text = "TXTMA_KHO"
        '
        'lblTen_kho
        '
        Me.lblTen_kho.AutoSize = True
        Me.lblTen_kho.Location = New System.Drawing.Point(264, 38)
        Me.lblTen_kho.Name = "lblTen_kho"
        Me.lblTen_kho.Size = New System.Drawing.Size(47, 13)
        Me.lblTen_kho.TabIndex = 12
        Me.lblTen_kho.Tag = ""
        Me.lblTen_kho.Text = "Ten kho"
        '
        'txtDTo
        '
        Me.txtDTo.Location = New System.Drawing.Point(264, 13)
        Me.txtDTo.MaxLength = 10
        Me.txtDTo.Name = "txtDTo"
        Me.txtDTo.Size = New System.Drawing.Size(100, 20)
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
        Me.txtDFrom.Size = New System.Drawing.Size(100, 20)
        Me.txtDFrom.TabIndex = 0
        Me.txtDFrom.Tag = "NB"
        Me.txtDFrom.Text = "  /  /    "
        Me.txtDFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDFrom.Value = New Date(CType(0, Long))
        '
        'lblDateFromTo
        '
        Me.lblDateFromTo.AutoSize = True
        Me.lblDateFromTo.Location = New System.Drawing.Point(20, 16)
        Me.lblDateFromTo.Name = "lblDateFromTo"
        Me.lblDateFromTo.Size = New System.Drawing.Size(69, 13)
        Me.lblDateFromTo.TabIndex = 0
        Me.lblDateFromTo.Tag = "L003"
        Me.lblDateFromTo.Text = "Tu/den ngay"
        '
        'lblMau_bc
        '
        Me.lblMau_bc.AutoSize = True
        Me.lblMau_bc.Location = New System.Drawing.Point(20, 109)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New System.Drawing.Size(70, 13)
        Me.lblMau_bc.TabIndex = 2
        Me.lblMau_bc.Tag = "L006"
        Me.lblMau_bc.Text = "Mau bao cao"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(20, 133)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(43, 13)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Tag = "L007"
        Me.lblTitle.Text = "Tieu de"
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(160, 131)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(300, 20)
        Me.txtTitle.TabIndex = 8
        Me.txtTitle.Tag = "NB"
        Me.txtTitle.Text = "txtTieu_de"
        '
        'TabAdv
        '
        Me.TabAdv.Location = New System.Drawing.Point(4, 22)
        Me.TabAdv.Name = "TabAdv"
        Me.TabAdv.Size = New System.Drawing.Size(601, 135)
        Me.TabAdv.TabIndex = 1
        Me.TabAdv.Tag = "L400"
        Me.TabAdv.Text = "Advance filter"
        '
        'tbgOrder
        '
        Me.tbgOrder.Controls.Add(Me.grdOrder)
        Me.tbgOrder.Location = New System.Drawing.Point(4, 22)
        Me.tbgOrder.Name = "tbgOrder"
        Me.tbgOrder.Size = New System.Drawing.Size(601, 135)
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
        Me.grdOrder.Size = New System.Drawing.Size(601, 135)
        Me.grdOrder.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(33, 13)
        Me.Label3.TabIndex = 31
        Me.Label3.Tag = "L008"
        Me.Label3.Text = "Ma lo"
        '
        'txtMa_lo
        '
        Me.txtMa_lo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_lo.Location = New System.Drawing.Point(160, 83)
        Me.txtMa_lo.Name = "txtMa_lo"
        Me.txtMa_lo.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_lo.TabIndex = 4
        Me.txtMa_lo.Tag = "FCML"
        Me.txtMa_lo.Text = "TXTMA_LO"
        '
        'lblTen_lo
        '
        Me.lblTen_lo.AutoSize = True
        Me.lblTen_lo.Location = New System.Drawing.Point(264, 87)
        Me.lblTen_lo.Name = "lblTen_lo"
        Me.lblTen_lo.Size = New System.Drawing.Size(37, 13)
        Me.lblTen_lo.TabIndex = 32
        Me.lblTen_lo.Tag = "L002"
        Me.lblTen_lo.Text = "Ten lo"
        '
        'frmFilter
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 249)
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

#End Region

    Private Sub frmDirInfor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim tp As New TabPage
        tabReports.TabPages.Add(tp)
        reportformlib.AddFreeFields(sysConn, tp, 8)
        tabReports.TabPages.Remove(tp)

        txtMa_kho.Text = Reg.GetRegistryKey("DFWh")
        txtMa_vt.Text = Reg.GetRegistryKey("DFItem")
        Dim oSite As New DirLib(txtMa_kho, lblTen_kho, sysConn, appConn, "dmkho", "ma_kho", "ten_kho", "Store", "1=1", False, cmdCancel)
        Dim oItem As New DirLib(txtMa_vt, lblTen_vt, sysConn, appConn, "Dmvt", "ma_vt", "ten_vt", "Item", "1=1", False, cmdCancel)
        oLot = New dirkeylib(txtMa_lo, lblTen_lo, sysConn, appConn, "vdmlo", "ma_lo", "ten_lo", "Lot", "ma_vt=''", True, cmdCancel)
        reportformlib.SetRPFormCaption(Me, tabReports, oLan, oVar, oLen)
        txtMa_kho.Text = Reg.GetRegistryKey("DFWh")
        txtMa_vt.Text = Reg.GetRegistryKey("DFItem")

        Me.CancelButton = cmdCancel
        pnContent = clsvoucher.clsVoucher.AddStb(Me)
        Dim pd As New System.Drawing.Printing.PrintDocument
        pnContent.Text = pd.PrinterSettings.PrinterName
        txtTitle.Text = Trim(rpTable.Rows(0).Item("rep_title" + IIf(Reg.GetRegistryKey("Language") = "V", "", "2")))
        txtDFrom.Value = Reg.GetRegistryKey("DFDFrom")
        txtDTo.Value = Reg.GetRegistryKey("DFDTo")
        oAdvFilter = New clsAdvFilter(Me, TabAdv, tabReports, appConn, sysConn, pnContent, cmdCancel)
        oAdvFilter.AddAdvSelect(ReportRow.Item("cAdvtables"))
        TabAdv.Dispose() 'Trong bao cao nay khong can loc chi tiet
        oAdvFilter.InitGridOrder(grdOrder, SysID, "001", ds, "Order")
        tabReports.SelectedIndex = 0
        oDirFormLib.grdOrderDataview = grdOrder.dvGrid
        oDirFormLib.grdSelectDataview = oAdvFilter.GetDataview
        oxInv = New xInv(tabReports, pnContent, appConn, sysConn)
    End Sub

    Private Sub cmdOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        If Not reportformlib.CheckEmptyField(Me, tabReports, oVar) Then
            Return
        End If
        dFrom = txtDFrom.Value
        dTo = txtDTo.Value
        Reg.SetRegistryKey("DFDFrom", txtDFrom.Value)
        Reg.SetRegistryKey("DFDTo", txtDTo.Value)
        Reg.SetRegistryKey("DFWh", Trim(txtMa_kho.Text))
        Reg.SetRegistryKey("DFItem", Trim(txtMa_vt.Text))
        pnContent.Text = oVar("m_process")
        Dim strOrder As String
        strOrder = ""
        ShowReport()
        Dim pd As New System.Drawing.Printing.PrintDocument
        pnContent.Text = pd.PrinterSettings.PrinterName
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cboReports_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboReports.SelectedIndexChanged
        If Not IsNothing(rpTable) Then
            txtTitle.Text = Trim(rpTable.Rows(cboReports.SelectedIndex).Item("rep_title" + IIf(Reg.GetRegistryKey("Language") = "V", "", "2")))
            txtR_title.Text = Trim(rpTable.Rows(cboReports.SelectedIndex).Item("rep_title"))
            txtR_title2.Text = Trim(rpTable.Rows(cboReports.SelectedIndex).Item("rep_title2"))
        End If
    End Sub
    Private Sub txtMa_lo_Enter(sender As Object, e As EventArgs) Handles txtMa_lo.Enter
        oLot.Key = "ma_vt='" + Me.txtMa_vt.Text + "'"
    End Sub
End Class
