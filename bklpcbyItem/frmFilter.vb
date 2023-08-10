﻿Imports Microsoft.VisualBasic
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
            DirMain.Site = Me.txtMa_kho.Text.Trim
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
        reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
        Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtMa_vt, Me.lblTen_vt, DirMain.sysConn, DirMain.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj13 As New vouchersearchlibobj(Me.txtMa_kho, Me.lblTen_kho, DirMain.sysConn, DirMain.appConn, "dmkho", "ma_kho", "ten_kho", "Store", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj7 As New vouchersearchlibobj(Me.txtNh_vt, New Label, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj8 As New vouchersearchlibobj(Me.txtNh_vt2, New Label, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup2", "loai_nh=2", True, Me.cmdCancel)
        Dim vouchersearchlibobj9 As New vouchersearchlibobj(Me.txtNh_vt3, New Label, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup3", "loai_nh=3", True, Me.cmdCancel)
        Dim vouchersearchlibobj10 As New vouchersearchlibobj(Me.txtLoai_vt, Me.lblTen_loai, DirMain.sysConn, DirMain.appConn, "Dmloaivt", "ma_loai_vt", "ten_loai_vt", "ItemType", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj15 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
        Me.CancelButton = Me.cmdCancel
        Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
        Dim document As New PrintDocument
        Me.pnContent.Text = document.PrinterSettings.PrinterName

        Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        Me.txtDFrom.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
        Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
    End Sub
    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
        Me.lblMa_dvcs = New System.Windows.Forms.Label()
        Me.lblTen_dvcs = New System.Windows.Forms.Label()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.tabReports = New System.Windows.Forms.TabControl()
        Me.tbgFilter = New System.Windows.Forms.TabPage()
        Me.txtNh_vt2 = New System.Windows.Forms.TextBox()
        Me.txtNh_vt3 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtNh_vt = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtLoai_vt = New System.Windows.Forms.TextBox()
        Me.lblTen_loai = New System.Windows.Forms.Label()
        Me.lblTen_vt = New System.Windows.Forms.Label()
        Me.txtMa_vt = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblTen_kho = New System.Windows.Forms.Label()
        Me.txtMa_kho = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDTo = New libscontrol.txtDate()
        Me.txtDFrom = New libscontrol.txtDate()
        Me.lblDateFromTo = New System.Windows.Forms.Label()
        Me.lblMau_bc = New System.Windows.Forms.Label()
        Me.cboReports = New System.Windows.Forms.ComboBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.tabReports.SuspendLayout()
        Me.tbgFilter.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtMa_dvcs
        '
        Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New System.Drawing.Point(160, 127)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_dvcs.TabIndex = 16
        Me.txtMa_dvcs.Tag = "FCML"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        '
        'lblMa_dvcs
        '
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New System.Drawing.Point(20, 129)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New System.Drawing.Size(38, 13)
        Me.lblMa_dvcs.TabIndex = 1
        Me.lblMa_dvcs.Tag = "L102"
        Me.lblMa_dvcs.Text = "Don vi"
        '
        'lblTen_dvcs
        '
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New System.Drawing.Point(264, 129)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New System.Drawing.Size(52, 13)
        Me.lblTen_dvcs.TabIndex = 7
        Me.lblTen_dvcs.Tag = "L002"
        Me.lblTen_dvcs.Text = "Ten dvcs"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(3, 245)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 23)
        Me.cmdOk.TabIndex = 1
        Me.cmdOk.Tag = "L001"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(79, 245)
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
        Me.tabReports.Size = New System.Drawing.Size(609, 233)
        Me.tabReports.TabIndex = 0
        Me.tabReports.Tag = "L200"
        '
        'tbgFilter
        '
        Me.tbgFilter.Controls.Add(Me.txtNh_vt2)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt3)
        Me.tbgFilter.Controls.Add(Me.Label7)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt)
        Me.tbgFilter.Controls.Add(Me.Label8)
        Me.tbgFilter.Controls.Add(Me.txtLoai_vt)
        Me.tbgFilter.Controls.Add(Me.lblTen_loai)
        Me.tbgFilter.Controls.Add(Me.lblTen_vt)
        Me.tbgFilter.Controls.Add(Me.txtMa_vt)
        Me.tbgFilter.Controls.Add(Me.Label4)
        Me.tbgFilter.Controls.Add(Me.lblTen_kho)
        Me.tbgFilter.Controls.Add(Me.txtMa_kho)
        Me.tbgFilter.Controls.Add(Me.Label1)
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
        Me.tbgFilter.Size = New System.Drawing.Size(601, 207)
        Me.tbgFilter.TabIndex = 0
        Me.tbgFilter.Tag = "L100"
        Me.tbgFilter.Text = "Dieu kien loc"
        '
        'txtNh_vt2
        '
        Me.txtNh_vt2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt2.Location = New System.Drawing.Point(263, 104)
        Me.txtNh_vt2.Name = "txtNh_vt2"
        Me.txtNh_vt2.Size = New System.Drawing.Size(100, 20)
        Me.txtNh_vt2.TabIndex = 12
        Me.txtNh_vt2.Tag = "FCML"
        Me.txtNh_vt2.Text = "TXTNH_VT2"
        '
        'txtNh_vt3
        '
        Me.txtNh_vt3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt3.Location = New System.Drawing.Point(366, 104)
        Me.txtNh_vt3.Name = "txtNh_vt3"
        Me.txtNh_vt3.Size = New System.Drawing.Size(100, 20)
        Me.txtNh_vt3.TabIndex = 13
        Me.txtNh_vt3.Tag = "FCML"
        Me.txtNh_vt3.Text = "TXTNH_VT3"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(20, 106)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(65, 13)
        Me.Label7.TabIndex = 45
        Me.Label7.Tag = "L111"
        Me.Label7.Text = "Nhom vat tu"
        '
        'txtNh_vt
        '
        Me.txtNh_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt.Location = New System.Drawing.Point(160, 104)
        Me.txtNh_vt.Name = "txtNh_vt"
        Me.txtNh_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtNh_vt.TabIndex = 11
        Me.txtNh_vt.Tag = "FCML"
        Me.txtNh_vt.Text = "TXTNH_VT"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(20, 83)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(57, 13)
        Me.Label8.TabIndex = 40
        Me.Label8.Tag = "L110"
        Me.Label8.Text = "Loai vat tu"
        '
        'txtLoai_vt
        '
        Me.txtLoai_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLoai_vt.Location = New System.Drawing.Point(160, 81)
        Me.txtLoai_vt.Name = "txtLoai_vt"
        Me.txtLoai_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtLoai_vt.TabIndex = 10
        Me.txtLoai_vt.Tag = "FCML"
        Me.txtLoai_vt.Text = "TXTLOAI_VT"
        '
        'lblTen_loai
        '
        Me.lblTen_loai.AutoSize = True
        Me.lblTen_loai.Location = New System.Drawing.Point(264, 83)
        Me.lblTen_loai.Name = "lblTen_loai"
        Me.lblTen_loai.Size = New System.Drawing.Size(57, 13)
        Me.lblTen_loai.TabIndex = 41
        Me.lblTen_loai.Tag = "L016"
        Me.lblTen_loai.Text = "Loai vat tu"
        '
        'lblTen_vt
        '
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New System.Drawing.Point(264, 37)
        Me.lblTen_vt.Name = "lblTen_vt"
        Me.lblTen_vt.Size = New System.Drawing.Size(56, 13)
        Me.lblTen_vt.TabIndex = 37
        Me.lblTen_vt.Tag = "RF"
        Me.lblTen_vt.Text = "Ten vat tu"
        '
        'txtMa_vt
        '
        Me.txtMa_vt.Location = New System.Drawing.Point(160, 35)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_vt.TabIndex = 5
        Me.txtMa_vt.Tag = "FCML"
        Me.txtMa_vt.Text = "txtMa_vt"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Tag = "L108"
        Me.Label4.Text = "Ma vat tu"
        '
        'lblTen_kho
        '
        Me.lblTen_kho.AutoSize = True
        Me.lblTen_kho.Location = New System.Drawing.Point(264, 60)
        Me.lblTen_kho.Name = "lblTen_kho"
        Me.lblTen_kho.Size = New System.Drawing.Size(74, 13)
        Me.lblTen_kho.TabIndex = 20
        Me.lblTen_kho.Tag = "RF"
        Me.lblTen_kho.Text = "Ten kho hang"
        '
        'txtMa_kho
        '
        Me.txtMa_kho.Location = New System.Drawing.Point(160, 58)
        Me.txtMa_kho.Name = "txtMa_kho"
        Me.txtMa_kho.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_kho.TabIndex = 6
        Me.txtMa_kho.Tag = "FCML"
        Me.txtMa_kho.Text = "txtMa_kho"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 60)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Tag = "L107"
        Me.Label1.Text = "Ma kho"
        '
        'txtDTo
        '
        Me.txtDTo.Location = New System.Drawing.Point(263, 13)
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
        Me.lblDateFromTo.Location = New System.Drawing.Point(20, 15)
        Me.lblDateFromTo.Name = "lblDateFromTo"
        Me.lblDateFromTo.Size = New System.Drawing.Size(69, 13)
        Me.lblDateFromTo.TabIndex = 0
        Me.lblDateFromTo.Tag = "L101"
        Me.lblDateFromTo.Text = "Tu/den ngay"
        '
        'lblMau_bc
        '
        Me.lblMau_bc.AutoSize = True
        Me.lblMau_bc.Location = New System.Drawing.Point(20, 152)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New System.Drawing.Size(70, 13)
        Me.lblMau_bc.TabIndex = 2
        Me.lblMau_bc.Tag = "L103"
        Me.lblMau_bc.Text = "Mau bao cao"
        '
        'cboReports
        '
        Me.cboReports.Location = New System.Drawing.Point(160, 150)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New System.Drawing.Size(306, 21)
        Me.cboReports.TabIndex = 17
        Me.cboReports.Text = "cboReports"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(20, 176)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(43, 13)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Tag = "L104"
        Me.lblTitle.Text = "Tieu de"
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(160, 174)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(306, 20)
        Me.txtTitle.TabIndex = 18
        Me.txtTitle.Tag = "NB"
        Me.txtTitle.Text = "txtTieu_de"
        '
        'frmFilter
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 302)
        Me.Controls.Add(Me.tabReports)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Name = "frmFilter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmFilter"
        Me.tabReports.ResumeLayout(False)
        Me.tbgFilter.ResumeLayout(False)
        Me.tbgFilter.PerformLayout()
        Me.ResumeLayout(False)

    End Sub


    ' Properties
    Friend WithEvents cboReports As ComboBox
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents lblDateFromTo As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMau_bc As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_kho As Label
    Friend WithEvents lblTen_loai As Label
    Friend WithEvents lblTen_vt As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents tabReports As TabControl
    Friend WithEvents tbgFilter As TabPage
    Friend WithEvents txtDFrom As txtDate
    Friend WithEvents txtDTo As txtDate
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
    Private intGroup1 As Integer
    Private intGroup2 As Integer
    Private intGroup3 As Integer
    Public pnContent As StatusBarPanel
End Class

