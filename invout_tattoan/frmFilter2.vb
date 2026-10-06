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
Imports libscommon
Imports libscontrol
Imports libscontrol.voucherseachlib


Namespace v20artthd
    Public Class frmFilter2
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
            Me.ds = New DataSet
            Me.dvOrder = New DataView
            Me.InitializeComponent
        End Sub

        Private Sub cboReports_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboReports.SelectedIndexChanged
            If Not Information.IsNothing(DirMain.rpTable2) Then
                Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable2.Rows.Item(Me.cboReports.SelectedIndex), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
            End If
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Me.Close()
        End Sub

        Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
            Me.pnContent.Text = StringType.FromObject(DirMain.oVar.Item("m_process"))
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
            If (((Me.txtKy1.Value < 1) Or (Me.txtKy2.Value > 12)) Or (((Me.txtNam1.Value * 12) + Me.txtKy1.Value) > ((Me.txtNam2.Value * 12) + Me.txtKy2.Value))) Then
                Msg.Alert(StringType.FromObject(DirMain.oLan.Item("409")))
                Me.txtKy1.Focus()
            Else
                DirMain.isContinue = True
                Me.Close()
            End If
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
            Dim control As control
            For Each control In Me.Controls
                If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                    control.Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
                End If
            Next
            Obj.Init(Me)
            Me.Text = DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text
            Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
            Me.CancelButton = Me.cmdCancel
            Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
            Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable2.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
            Me.txtKy1.Value = Date.Now.Month
            Me.txtKy2.Value = Date.Now.Month
            Me.txtNam1.Value = Date.Now.Year
            Me.txtNam2.Value = Date.Now.Year
            Me.txtMa_dvcs.Text = DirMain.fPrint.txtMa_dvcs.Text
            Me.validated_ngay1()
            Me.validated_ngay2()
        End Sub

        <DebuggerStepThrough()> _
Private Sub InitializeComponent()
            Me.cmdOk = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.txtKy2 = New libscontrol.txtNumeric()
            Me.txtNam2 = New libscontrol.txtNumeric()
            Me.txtKy1 = New libscontrol.txtNumeric()
            Me.lblMa_vi_tri = New System.Windows.Forms.Label()
            Me.lblMa_kh = New System.Windows.Forms.Label()
            Me.txtNam1 = New libscontrol.txtNumeric()
            Me.lblMa_dvcs = New System.Windows.Forms.Label()
            Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
            Me.lblTen_dvcs = New System.Windows.Forms.Label()
            Me.lblMau_bc = New System.Windows.Forms.Label()
            Me.cboReports = New System.Windows.Forms.ComboBox()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.txtTitle = New System.Windows.Forms.TextBox()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.lblTu_ngay = New System.Windows.Forms.Label()
            Me.lblDen_ngay = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.Location = New System.Drawing.Point(13, 179)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.Size = New System.Drawing.Size(120, 33)
            Me.cmdOk.TabIndex = 7
            Me.cmdOk.Tag = "L003"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.Location = New System.Drawing.Point(134, 179)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(120, 33)
            Me.cmdCancel.TabIndex = 8
            Me.cmdCancel.Tag = "L004"
            Me.cmdCancel.Text = "Huy"
            '
            'txtKy2
            '
            Me.txtKy2.Format = ""
            Me.txtKy2.Location = New System.Drawing.Point(256, 58)
            Me.txtKy2.MaxLength = 2
            Me.txtKy2.Name = "txtKy2"
            Me.txtKy2.Size = New System.Drawing.Size(48, 26)
            Me.txtKy2.TabIndex = 2
            Me.txtKy2.Tag = "FNNBDF"
            Me.txtKy2.Text = "0"
            Me.txtKy2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtKy2.Value = 0R
            '
            'txtNam2
            '
            Me.txtNam2.Format = ""
            Me.txtNam2.Location = New System.Drawing.Point(307, 58)
            Me.txtNam2.MaxLength = 4
            Me.txtNam2.Name = "txtNam2"
            Me.txtNam2.Size = New System.Drawing.Size(77, 26)
            Me.txtNam2.TabIndex = 3
            Me.txtNam2.Tag = "FNNBDF"
            Me.txtNam2.Text = "0"
            Me.txtNam2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNam2.Value = 0R
            '
            'txtKy1
            '
            Me.txtKy1.Format = ""
            Me.txtKy1.Location = New System.Drawing.Point(256, 23)
            Me.txtKy1.MaxLength = 2
            Me.txtKy1.Name = "txtKy1"
            Me.txtKy1.Size = New System.Drawing.Size(48, 26)
            Me.txtKy1.TabIndex = 0
            Me.txtKy1.Tag = "FNNBDF"
            Me.txtKy1.Text = "1"
            Me.txtKy1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtKy1.Value = 1.0R
            '
            'lblMa_vi_tri
            '
            Me.lblMa_vi_tri.AutoSize = True
            Me.lblMa_vi_tri.Location = New System.Drawing.Point(26, 58)
            Me.lblMa_vi_tri.Name = "lblMa_vi_tri"
            Me.lblMa_vi_tri.Size = New System.Drawing.Size(39, 20)
            Me.lblMa_vi_tri.TabIndex = 58
            Me.lblMa_vi_tri.Tag = "L408"
            Me.lblMa_vi_tri.Text = "Den"
            '
            'lblMa_kh
            '
            Me.lblMa_kh.AutoSize = True
            Me.lblMa_kh.Location = New System.Drawing.Point(26, 23)
            Me.lblMa_kh.Name = "lblMa_kh"
            Me.lblMa_kh.Size = New System.Drawing.Size(81, 20)
            Me.lblMa_kh.TabIndex = 59
            Me.lblMa_kh.Tag = "L407"
            Me.lblMa_kh.Text = "Tu ky/nam"
            '
            'txtNam1
            '
            Me.txtNam1.Format = ""
            Me.txtNam1.Location = New System.Drawing.Point(307, 23)
            Me.txtNam1.MaxLength = 4
            Me.txtNam1.Name = "txtNam1"
            Me.txtNam1.Size = New System.Drawing.Size(77, 26)
            Me.txtNam1.TabIndex = 1
            Me.txtNam1.Tag = "FNNBDF"
            Me.txtNam1.Text = "0"
            Me.txtNam1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNam1.Value = 0R
            '
            'lblMa_dvcs
            '
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New System.Drawing.Point(525, 222)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New System.Drawing.Size(53, 20)
            Me.lblMa_dvcs.TabIndex = 1
            Me.lblMa_dvcs.Tag = "L102"
            Me.lblMa_dvcs.Text = "Don vi"
            Me.lblMa_dvcs.Visible = False
            '
            'txtMa_dvcs
            '
            Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New System.Drawing.Point(602, 210)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.Size = New System.Drawing.Size(160, 26)
            Me.txtMa_dvcs.TabIndex = 4
            Me.txtMa_dvcs.Tag = "FCML"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            Me.txtMa_dvcs.Visible = False
            '
            'lblTen_dvcs
            '
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New System.Drawing.Point(768, 210)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New System.Drawing.Size(72, 20)
            Me.lblTen_dvcs.TabIndex = 7
            Me.lblTen_dvcs.Tag = "L002"
            Me.lblTen_dvcs.Text = "Ten dvcs"
            Me.lblTen_dvcs.Visible = False
            '
            'lblMau_bc
            '
            Me.lblMau_bc.AutoSize = True
            Me.lblMau_bc.Location = New System.Drawing.Point(26, 95)
            Me.lblMau_bc.Name = "lblMau_bc"
            Me.lblMau_bc.Size = New System.Drawing.Size(101, 20)
            Me.lblMau_bc.TabIndex = 2
            Me.lblMau_bc.Tag = "L103"
            Me.lblMau_bc.Text = "Mau bao cao"
            '
            'cboReports
            '
            Me.cboReports.ItemHeight = 20
            Me.cboReports.Location = New System.Drawing.Point(256, 92)
            Me.cboReports.Name = "cboReports"
            Me.cboReports.Size = New System.Drawing.Size(480, 28)
            Me.cboReports.TabIndex = 5
            Me.cboReports.Text = "cboReports"
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New System.Drawing.Point(26, 130)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(61, 20)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Tag = "L104"
            Me.lblTitle.Text = "Tieu de"
            '
            'txtTitle
            '
            Me.txtTitle.Location = New System.Drawing.Point(256, 127)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New System.Drawing.Size(480, 26)
            Me.txtTitle.TabIndex = 6
            Me.txtTitle.Tag = "NB"
            Me.txtTitle.Text = "txtTieu_de"
            '
            'GroupBox1
            '
            Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.GroupBox1.Location = New System.Drawing.Point(13, 0)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(729, 165)
            Me.GroupBox1.TabIndex = 60
            Me.GroupBox1.TabStop = False
            '
            'lblTu_ngay
            '
            Me.lblTu_ngay.AutoSize = True
            Me.lblTu_ngay.Location = New System.Drawing.Point(390, 26)
            Me.lblTu_ngay.Name = "lblTu_ngay"
            Me.lblTu_ngay.Size = New System.Drawing.Size(85, 20)
            Me.lblTu_ngay.TabIndex = 61
            Me.lblTu_ngay.Tag = ""
            Me.lblTu_ngay.Text = "lblTu_ngay"
            '
            'lblDen_ngay
            '
            Me.lblDen_ngay.AutoSize = True
            Me.lblDen_ngay.Location = New System.Drawing.Point(390, 61)
            Me.lblDen_ngay.Name = "lblDen_ngay"
            Me.lblDen_ngay.Size = New System.Drawing.Size(97, 20)
            Me.lblDen_ngay.TabIndex = 62
            Me.lblDen_ngay.Tag = ""
            Me.lblDen_ngay.Text = "lblDen_ngay"
            '
            'frmFilter2
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(8, 19)
            Me.ClientSize = New System.Drawing.Size(755, 255)
            Me.Controls.Add(Me.lblDen_ngay)
            Me.Controls.Add(Me.lblTu_ngay)
            Me.Controls.Add(Me.txtNam1)
            Me.Controls.Add(Me.txtMa_dvcs)
            Me.Controls.Add(Me.txtKy2)
            Me.Controls.Add(Me.lblMau_bc)
            Me.Controls.Add(Me.txtTitle)
            Me.Controls.Add(Me.lblTitle)
            Me.Controls.Add(Me.txtNam2)
            Me.Controls.Add(Me.lblTen_dvcs)
            Me.Controls.Add(Me.txtKy1)
            Me.Controls.Add(Me.lblMa_vi_tri)
            Me.Controls.Add(Me.lblMa_dvcs)
            Me.Controls.Add(Me.lblMa_kh)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.cboReports)
            Me.Controls.Add(Me.GroupBox1)
            Me.Name = "frmFilter2"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmFilter"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private Sub txtKy1_Validated(ByVal sender As Object, ByVal e As EventArgs)
            Me.validated_ngay1()
        End Sub

        Private Sub txtKy2_Validated(ByVal sender As Object, ByVal e As EventArgs)
            Me.validated_ngay2()
        End Sub

        Private Sub txtNam1_Validated(ByVal sender As Object, ByVal e As EventArgs)
            Me.validated_ngay1()
        End Sub

        Private Sub txtNam2_Validated(ByVal sender As Object, ByVal e As EventArgs)
            Me.validated_ngay2()
        End Sub

        Private Sub validated_ngay1()
            If ((Me.txtKy1.Value < 1) Or (Me.txtKy1.Value > 12)) Then
                Me.txtKy1.Focus()
            ElseIf ((Me.txtNam1.Value < 1900) Or (Me.txtNam1.Value > 3000)) Then
                Me.txtNam1.Focus()
            Else
                Me.dDate = startdate.GetStartDateOfYear(DirMain.oDirFormLib.appConn, CInt(Math.Round(Me.txtNam1.Value)))
                Me.lblTu_ngay.Text = StringType.FromDate(Me.dDate.AddMonths(CInt(Math.Round(CDbl((Me.txtKy1.Value - 1))))).Date)
            End If
        End Sub

        Private Sub validated_ngay2()
            If ((Me.txtKy2.Value < 1) Or (Me.txtKy2.Value > 12)) Then
                Me.txtKy2.Focus()
            ElseIf ((Me.txtNam2.Value < 1900) Or (Me.txtNam2.Value > 3000)) Then
                Me.txtNam2.Focus()
            Else
                Me.dDate = startdate.GetStartDateOfYear(DirMain.oDirFormLib.appConn, CInt(Math.Round(Me.txtNam2.Value)))
                Me.lblDen_ngay.Text = StringType.FromDate(Me.dDate.AddMonths(CInt(Math.Round(Me.txtKy2.Value))).AddDays(-1).Date)
            End If
        End Sub


        ' Properties
        Friend WithEvents cboReports As ComboBox
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents GroupBox1 As GroupBox
        Friend WithEvents lblDen_ngay As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMa_kh As Label
        Friend WithEvents lblMa_vi_tri As Label
        Friend WithEvents lblMau_bc As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTitle As Label
        Friend WithEvents lblTu_ngay As Label
        Friend WithEvents txtKy1 As txtNumeric
        Friend WithEvents txtKy2 As txtNumeric
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtNam1 As txtNumeric
        Friend WithEvents txtNam2 As txtNumeric
        Friend WithEvents txtTitle As TextBox


        Private components As IContainer
        Private dDate As DateTime
        Public ds As DataSet
        Private dvOrder As DataView
        Public pnContent As StatusBarPanel
    End Class
End Namespace

