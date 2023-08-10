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

Namespace z18thbom
    Public Class frmFilter
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
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
                DirMain.strKey = ""
                Dim num5 As Integer = IntegerType.FromObject(DirMain.oLen.Item("so_ct"))
                Reg.SetRegistryKey("DFDFrom", Me.txtDFrom.Value)
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
            'reportformlib.AddFreeFields(DirMain.sysConn, Me.tabReports.TabPages.Item(3), 10)
            'reportformlib.AddFreeCode(DirMain.sysConn, Me.tabReports.TabPages.Item(2), DirMain.sysConn, DirMain.appConn, Me.cmdCancel, "ma_vv")
            reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
            Dim vouchersearchlibobj10 As New vouchersearchlibobj(Me.txtMa_vt, Me.lblTen_sp, DirMain.sysConn, DirMain.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "loai_vt='41' or loai_vt='51'", True, Me.cmdCancel)
            Dim onh_vt1 As New vouchersearchlibobj(Me.txtNh_vt1, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=1", True, Me.cmdCancel)
            Dim onh_vt2 As New vouchersearchlibobj(Me.txtNh_vt2, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=2", True, Me.cmdCancel)
            Dim onh_vt3 As New vouchersearchlibobj(Me.txtNh_vt3, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=3", True, Me.cmdCancel)
            Dim oDept As New DirLib(Me.txtMa_bp, Me.lblTen_bp, DirMain.sysConn, DirMain.appConn, "dmbp", "ma_bp", "ten_bp", "SaleDept", "1=1", False, Me.cmdCancel)
            Dim vouchersearchlibobj11 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
            Dim oJob As New DirLib(Me.txtSo_lsx, Me.lblTen_lsx, DirMain.sysConn, DirMain.appConn, "phlsx", "so_lsx", "dien_giai", "MONumber", "1=1", True, Me.cmdCancel)
            Me.txtDFrom.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
            Me.CancelButton = Me.cmdCancel
            Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
            Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        End Sub
        Friend WithEvents txtSo_lsx As System.Windows.Forms.TextBox
        Friend WithEvents txtMa_vt As System.Windows.Forms.TextBox
        Friend WithEvents lblTen_sp As System.Windows.Forms.Label
        Friend WithEvents lblTen_lsx As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents txtNh_vt3 As TextBox
        Friend WithEvents txtNh_vt2 As TextBox
        Friend WithEvents Label2 As Label
        Friend WithEvents txtNh_vt1 As TextBox
        Friend WithEvents lblTen_nh As Label
        Friend WithEvents Label4 As Label
        Friend WithEvents txtMa_bp As TextBox
        Friend WithEvents lblTen_bp As Label
        Friend WithEvents txtDFrom As txtDate
        Friend WithEvents lblDateFromTo As Label
        Friend WithEvents txtMa_lo_sp As System.Windows.Forms.TextBox

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
            Me.lblMa_dvcs = New System.Windows.Forms.Label()
            Me.lblTen_dvcs = New System.Windows.Forms.Label()
            Me.cmdOk = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.tabReports = New System.Windows.Forms.TabControl()
            Me.tbgFilter = New System.Windows.Forms.TabPage()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.txtMa_bp = New System.Windows.Forms.TextBox()
            Me.lblTen_bp = New System.Windows.Forms.Label()
            Me.txtNh_vt3 = New System.Windows.Forms.TextBox()
            Me.txtNh_vt2 = New System.Windows.Forms.TextBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.txtNh_vt1 = New System.Windows.Forms.TextBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.txtMa_lo_sp = New System.Windows.Forms.TextBox()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.txtMa_vt = New System.Windows.Forms.TextBox()
            Me.lblTen_sp = New System.Windows.Forms.Label()
            Me.lblMau_bc = New System.Windows.Forms.Label()
            Me.cboReports = New System.Windows.Forms.ComboBox()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.txtTitle = New System.Windows.Forms.TextBox()
            Me.lblTen_lsx = New System.Windows.Forms.Label()
            Me.lblMa_vv = New System.Windows.Forms.Label()
            Me.txtSo_lsx = New System.Windows.Forms.TextBox()
            Me.lblTen_nh = New System.Windows.Forms.Label()
            Me.txtDFrom = New libscontrol.txtDate()
            Me.lblDateFromTo = New System.Windows.Forms.Label()
            Me.tabReports.SuspendLayout()
            Me.tbgFilter.SuspendLayout()
            Me.SuspendLayout()
            '
            'txtMa_dvcs
            '
            Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New System.Drawing.Point(160, 144)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.Size = New System.Drawing.Size(100, 20)
            Me.txtMa_dvcs.TabIndex = 8
            Me.txtMa_dvcs.Tag = "FCMaster#dbo.ff_InUnits(a.ma_dvcs, '%s') = 1 #MLEX"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            '
            'lblMa_dvcs
            '
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New System.Drawing.Point(16, 146)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New System.Drawing.Size(38, 13)
            Me.lblMa_dvcs.TabIndex = 1
            Me.lblMa_dvcs.Tag = "L108"
            Me.lblMa_dvcs.Text = "Don vi"
            '
            'lblTen_dvcs
            '
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New System.Drawing.Point(264, 146)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New System.Drawing.Size(52, 13)
            Me.lblTen_dvcs.TabIndex = 7
            Me.lblTen_dvcs.Tag = "RF"
            Me.lblTen_dvcs.Text = "Ten dvcs"
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.Location = New System.Drawing.Point(3, 253)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.Size = New System.Drawing.Size(75, 23)
            Me.cmdOk.TabIndex = 1
            Me.cmdOk.Tag = "L001"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.Location = New System.Drawing.Point(79, 253)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
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
            Me.tabReports.Location = New System.Drawing.Point(-2, 0)
            Me.tabReports.Name = "tabReports"
            Me.tabReports.SelectedIndex = 0
            Me.tabReports.Size = New System.Drawing.Size(609, 248)
            Me.tabReports.TabIndex = 0
            Me.tabReports.Tag = ""
            '
            'tbgFilter
            '
            Me.tbgFilter.Controls.Add(Me.txtDFrom)
            Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
            Me.tbgFilter.Controls.Add(Me.Label4)
            Me.tbgFilter.Controls.Add(Me.txtMa_bp)
            Me.tbgFilter.Controls.Add(Me.lblTen_bp)
            Me.tbgFilter.Controls.Add(Me.txtNh_vt3)
            Me.tbgFilter.Controls.Add(Me.txtNh_vt2)
            Me.tbgFilter.Controls.Add(Me.Label2)
            Me.tbgFilter.Controls.Add(Me.txtNh_vt1)
            Me.tbgFilter.Controls.Add(Me.Label1)
            Me.tbgFilter.Controls.Add(Me.txtMa_lo_sp)
            Me.tbgFilter.Controls.Add(Me.Label3)
            Me.tbgFilter.Controls.Add(Me.txtMa_vt)
            Me.tbgFilter.Controls.Add(Me.lblTen_sp)
            Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblMau_bc)
            Me.tbgFilter.Controls.Add(Me.cboReports)
            Me.tbgFilter.Controls.Add(Me.lblTitle)
            Me.tbgFilter.Controls.Add(Me.txtTitle)
            Me.tbgFilter.Controls.Add(Me.lblTen_lsx)
            Me.tbgFilter.Controls.Add(Me.lblMa_vv)
            Me.tbgFilter.Controls.Add(Me.txtSo_lsx)
            Me.tbgFilter.Location = New System.Drawing.Point(4, 22)
            Me.tbgFilter.Name = "tbgFilter"
            Me.tbgFilter.Size = New System.Drawing.Size(601, 222)
            Me.tbgFilter.TabIndex = 0
            Me.tbgFilter.Tag = "L100"
            Me.tbgFilter.Text = "Dieu kien loc"
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(16, 100)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(64, 13)
            Me.Label4.TabIndex = 134
            Me.Label4.Tag = "L104"
            Me.Label4.Text = "Phan xuong"
            '
            'txtMa_bp
            '
            Me.txtMa_bp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_bp.Location = New System.Drawing.Point(160, 98)
            Me.txtMa_bp.Name = "txtMa_bp"
            Me.txtMa_bp.Size = New System.Drawing.Size(100, 20)
            Me.txtMa_bp.TabIndex = 6
            Me.txtMa_bp.Tag = ""
            Me.txtMa_bp.Text = "TXTMA_BP"
            '
            'lblTen_bp
            '
            Me.lblTen_bp.AutoSize = True
            Me.lblTen_bp.Location = New System.Drawing.Point(264, 100)
            Me.lblTen_bp.Name = "lblTen_bp"
            Me.lblTen_bp.Size = New System.Drawing.Size(40, 13)
            Me.lblTen_bp.TabIndex = 133
            Me.lblTen_bp.Tag = ""
            Me.lblTen_bp.Text = "Ten sp"
            '
            'txtNh_vt3
            '
            Me.txtNh_vt3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtNh_vt3.Location = New System.Drawing.Point(366, 75)
            Me.txtNh_vt3.Name = "txtNh_vt3"
            Me.txtNh_vt3.Size = New System.Drawing.Size(100, 20)
            Me.txtNh_vt3.TabIndex = 5
            Me.txtNh_vt3.Tag = ""
            Me.txtNh_vt3.Text = "TXTNH_VT3"
            '
            'txtNh_vt2
            '
            Me.txtNh_vt2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtNh_vt2.Location = New System.Drawing.Point(263, 75)
            Me.txtNh_vt2.Name = "txtNh_vt2"
            Me.txtNh_vt2.Size = New System.Drawing.Size(100, 20)
            Me.txtNh_vt2.TabIndex = 4
            Me.txtNh_vt2.Tag = ""
            Me.txtNh_vt2.Text = "TXTNH_VT2"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(16, 77)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(35, 13)
            Me.Label2.TabIndex = 129
            Me.Label2.Tag = "L103"
            Me.Label2.Text = "Nhom"
            '
            'txtNh_vt1
            '
            Me.txtNh_vt1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtNh_vt1.Location = New System.Drawing.Point(160, 75)
            Me.txtNh_vt1.Name = "txtNh_vt1"
            Me.txtNh_vt1.Size = New System.Drawing.Size(100, 20)
            Me.txtNh_vt1.TabIndex = 3
            Me.txtNh_vt1.Tag = ""
            Me.txtNh_vt1.Text = "TXTNH_VT1"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(16, 123)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(90, 13)
            Me.Label1.TabIndex = 126
            Me.Label1.Tag = "L105"
            Me.Label1.Text = "So lo thanh pham"
            '
            'txtMa_lo_sp
            '
            Me.txtMa_lo_sp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_lo_sp.Location = New System.Drawing.Point(160, 121)
            Me.txtMa_lo_sp.Name = "txtMa_lo_sp"
            Me.txtMa_lo_sp.Size = New System.Drawing.Size(100, 20)
            Me.txtMa_lo_sp.TabIndex = 7
            Me.txtMa_lo_sp.Tag = "FC"
            Me.txtMa_lo_sp.Text = "TXTMA_LO_SP"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(16, 54)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(55, 13)
            Me.Label3.TabIndex = 124
            Me.Label3.Tag = "L102"
            Me.Label3.Text = "San pham"
            '
            'txtMa_vt
            '
            Me.txtMa_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_vt.Location = New System.Drawing.Point(160, 52)
            Me.txtMa_vt.Name = "txtMa_vt"
            Me.txtMa_vt.Size = New System.Drawing.Size(100, 20)
            Me.txtMa_vt.TabIndex = 2
            Me.txtMa_vt.Tag = ""
            Me.txtMa_vt.Text = "TXTMA_VT"
            '
            'lblTen_sp
            '
            Me.lblTen_sp.AutoSize = True
            Me.lblTen_sp.Location = New System.Drawing.Point(264, 54)
            Me.lblTen_sp.Name = "lblTen_sp"
            Me.lblTen_sp.Size = New System.Drawing.Size(40, 13)
            Me.lblTen_sp.TabIndex = 123
            Me.lblTen_sp.Tag = ""
            Me.lblTen_sp.Text = "Ten sp"
            '
            'lblMau_bc
            '
            Me.lblMau_bc.AutoSize = True
            Me.lblMau_bc.Location = New System.Drawing.Point(16, 169)
            Me.lblMau_bc.Name = "lblMau_bc"
            Me.lblMau_bc.Size = New System.Drawing.Size(70, 13)
            Me.lblMau_bc.TabIndex = 2
            Me.lblMau_bc.Tag = "L109"
            Me.lblMau_bc.Text = "Mau bao cao"
            '
            'cboReports
            '
            Me.cboReports.Location = New System.Drawing.Point(160, 167)
            Me.cboReports.Name = "cboReports"
            Me.cboReports.Size = New System.Drawing.Size(300, 21)
            Me.cboReports.TabIndex = 9
            Me.cboReports.Text = "cboReports"
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New System.Drawing.Point(16, 193)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(43, 13)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Tag = "L110"
            Me.lblTitle.Text = "Tieu de"
            '
            'txtTitle
            '
            Me.txtTitle.Location = New System.Drawing.Point(160, 191)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New System.Drawing.Size(300, 20)
            Me.txtTitle.TabIndex = 10
            Me.txtTitle.Tag = "NB"
            Me.txtTitle.Text = "txtTieu_de"
            '
            'lblTen_lsx
            '
            Me.lblTen_lsx.AutoSize = True
            Me.lblTen_lsx.Location = New System.Drawing.Point(264, 31)
            Me.lblTen_lsx.Name = "lblTen_lsx"
            Me.lblTen_lsx.Size = New System.Drawing.Size(41, 13)
            Me.lblTen_lsx.TabIndex = 108
            Me.lblTen_lsx.Tag = ""
            Me.lblTen_lsx.Text = "Ten lsx"
            '
            'lblMa_vv
            '
            Me.lblMa_vv.AutoSize = True
            Me.lblMa_vv.Location = New System.Drawing.Point(16, 31)
            Me.lblMa_vv.Name = "lblMa_vv"
            Me.lblMa_vv.Size = New System.Drawing.Size(74, 13)
            Me.lblMa_vv.TabIndex = 107
            Me.lblMa_vv.Tag = "L101"
            Me.lblMa_vv.Text = "Lenh san xuat"
            '
            'txtSo_lsx
            '
            Me.txtSo_lsx.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtSo_lsx.Location = New System.Drawing.Point(160, 29)
            Me.txtSo_lsx.Name = "txtSo_lsx"
            Me.txtSo_lsx.Size = New System.Drawing.Size(100, 20)
            Me.txtSo_lsx.TabIndex = 1
            Me.txtSo_lsx.Tag = "FC"
            Me.txtSo_lsx.Text = "TXTSO_LSX"
            '
            'lblTen_nh
            '
            Me.lblTen_nh.AutoSize = True
            Me.lblTen_nh.Location = New System.Drawing.Point(317, 285)
            Me.lblTen_nh.Name = "lblTen_nh"
            Me.lblTen_nh.Size = New System.Drawing.Size(41, 13)
            Me.lblTen_nh.TabIndex = 128
            Me.lblTen_nh.Tag = ""
            Me.lblTen_nh.Text = "Ten nh"
            Me.lblTen_nh.Visible = False
            '
            'txtDFrom
            '
            Me.txtDFrom.Location = New System.Drawing.Point(160, 5)
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
            Me.lblDateFromTo.Location = New System.Drawing.Point(16, 7)
            Me.lblDateFromTo.Name = "lblDateFromTo"
            Me.lblDateFromTo.Size = New System.Drawing.Size(46, 13)
            Me.lblDateFromTo.TabIndex = 136
            Me.lblDateFromTo.Tag = "L106"
            Me.lblDateFromTo.Text = "Tu ngay"
            '
            'frmFilter
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(608, 297)
            Me.Controls.Add(Me.tabReports)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.lblTen_nh)
            Me.Controls.Add(Me.cmdOk)
            Me.Name = "frmFilter"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmFilter"
            Me.tabReports.ResumeLayout(False)
            Me.tbgFilter.ResumeLayout(False)
            Me.tbgFilter.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub


        ' Properties
        Friend WithEvents cboReports As ComboBox
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents Label3 As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMa_vv As Label
        Friend WithEvents lblMau_bc As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTitle As Label
        Friend WithEvents tabReports As TabControl
        Friend WithEvents tbgFilter As TabPage
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtTitle As TextBox

        Private components As IContainer
        Public pnContent As StatusBarPanel
    End Class
End Namespace

