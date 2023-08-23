Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Windows.Forms
Imports libscontrol
Imports libscontrol.voucherseachlib

Namespace inctpxa
    Public Class frmDateLSX
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDate_Load)
            Me.InitializeComponent()
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Me.Close()
        End Sub

        Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
            Me.Close()
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub frmDate_Load(ByVal sender As Object, ByVal e As EventArgs)
            On Error Resume Next
            Me.Text = StringType.FromObject(modVoucher.oLan.Item("305"))
            Dim control As Control
            For Each control In Me.Controls
                If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                    control.Text = StringType.FromObject(modVoucher.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
                End If
            Next
            Obj.Init(Me)
            Me.txtNgay_ct.AddCalenderControl()
            Me.txtNgay_ct.Value = DateAndTime.Now.Date
            Dim vouchersearchlibobj6 As New vouchersearchlibobj(Me.txtMa_vt, Me.lblTen_vt, modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
            Dim oFcode1 As New DirLib(Me.txtFcode1, Me.lblTen_fcode1, modVoucher.sysConn, modVoucher.appConn, "z21dmmayphache", "ma", "ten", "z21dmmayphache", "1=1", False, Me.cmdCancel)
            Dim oType As New CharLib(Me.txtType, "*,0,1,2")
            Me.txtType.Text = "*"
            Me.txtFcode1.Text = "2"
        End Sub
        Friend WithEvents lblTen_vt As Label
        Friend WithEvents lblMa_vt As Label
        Friend WithEvents txtMa_vt As TextBox
        Friend WithEvents Label2 As Label
        Friend WithEvents txtMa_lo As TextBox
        Friend WithEvents txtFcode1 As TextBox
        Friend WithEvents Label11 As Label
        Friend WithEvents lblTen_fcode1 As Label
        Friend WithEvents txtType As TextBox
        Friend WithEvents Label1 As Label
        Friend WithEvents Label3 As Label
        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblNgay_ct = New Label
            Me.cmdOk = New Button
            Me.cmdCancel = New Button
            Me.grpInfor = New System.Windows.Forms.GroupBox
            Me.txtNgay_ct = New txtDate
            Me.lblTen_vt = New Label
            Me.lblMa_vt = New Label
            Me.txtMa_vt = New TextBox
            Me.Label2 = New Label
            Me.txtMa_lo = New TextBox
            Me.txtFcode1 = New TextBox
            Me.Label11 = New Label
            Me.lblTen_fcode1 = New Label
            Me.txtType = New TextBox
            Me.Label1 = New Label
            Me.Label3 = New Label
            Me.SuspendLayout()
            '
            'lblNgay_ct
            '
            Me.lblNgay_ct.AutoSize = True
            Me.lblNgay_ct.Location = New System.Drawing.Point(23, 23)
            Me.lblNgay_ct.Name = "lblNgay_ct"
            Me.lblNgay_ct.Size = New System.Drawing.Size(98, 16)
            Me.lblNgay_ct.TabIndex = 7
            Me.lblNgay_ct.Tag = "L301"
            Me.lblNgay_ct.Text = "Ngay chung tu moi"
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.cmdOk.Location = New System.Drawing.Point(8, 149)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 5
            Me.cmdOk.Tag = "L302"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(84, 149)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 6
            Me.cmdCancel.Tag = "L303"
            Me.cmdCancel.Text = "Huy"
            '
            'grpInfor
            '
            Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grpInfor.Location = New System.Drawing.Point(8, 8)
            Me.grpInfor.Name = "grpInfor"
            Me.grpInfor.Size = New System.Drawing.Size(592, 128)
            Me.grpInfor.TabIndex = 17
            Me.grpInfor.TabStop = False
            '
            'txtNgay_ct
            '
            Me.txtNgay_ct.Location = New System.Drawing.Point(155, 21)
            Me.txtNgay_ct.MaxLength = 10
            Me.txtNgay_ct.Name = "txtNgay_ct"
            Me.txtNgay_ct.TabIndex = 0
            Me.txtNgay_ct.Text = "01/01/1900"
            Me.txtNgay_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNgay_ct.Value = New Date(1900, 1, 1, 0, 0, 0, 0)
            '
            'lblTen_vt
            '
            Me.lblTen_vt.AutoSize = True
            Me.lblTen_vt.Location = New System.Drawing.Point(264, 44)
            Me.lblTen_vt.Name = "lblTen_vt"
            Me.lblTen_vt.Size = New System.Drawing.Size(54, 16)
            Me.lblTen_vt.TabIndex = 110
            Me.lblTen_vt.Tag = ""
            Me.lblTen_vt.Text = "Ten vat tu"
            '
            'lblMa_vt
            '
            Me.lblMa_vt.AutoSize = True
            Me.lblMa_vt.Location = New System.Drawing.Point(23, 44)
            Me.lblMa_vt.Name = "lblMa_vt"
            Me.lblMa_vt.Size = New System.Drawing.Size(73, 16)
            Me.lblMa_vt.TabIndex = 109
            Me.lblMa_vt.Tag = "L033"
            Me.lblMa_vt.Text = "Ma san pham"
            '
            'txtMa_vt
            '
            Me.txtMa_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_vt.Location = New System.Drawing.Point(155, 42)
            Me.txtMa_vt.Name = "txtMa_vt"
            Me.txtMa_vt.TabIndex = 1
            Me.txtMa_vt.Tag = "FCNB"
            Me.txtMa_vt.Text = "TXTMA_VT"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(23, 65)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(30, 16)
            Me.Label2.TabIndex = 112
            Me.Label2.Tag = "L034"
            Me.Label2.Text = "So lo"
            '
            'txtMa_lo
            '
            Me.txtMa_lo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_lo.Location = New System.Drawing.Point(155, 63)
            Me.txtMa_lo.Name = "txtMa_lo"
            Me.txtMa_lo.TabIndex = 2
            Me.txtMa_lo.Tag = ""
            Me.txtMa_lo.Text = ""
            '
            'txtFcode1
            '
            Me.txtFcode1.BackColor = System.Drawing.Color.White
            Me.txtFcode1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtFcode1.Location = New System.Drawing.Point(155, 85)
            Me.txtFcode1.Name = "txtFcode1"
            Me.txtFcode1.TabIndex = 3
            Me.txtFcode1.Tag = "FCCF"
            Me.txtFcode1.Text = "TXTFCODE1"
            '
            'Label11
            '
            Me.Label11.AutoSize = True
            Me.Label11.Location = New System.Drawing.Point(23, 88)
            Me.Label11.Name = "Label11"
            Me.Label11.Size = New System.Drawing.Size(69, 16)
            Me.Label11.TabIndex = 155
            Me.Label11.Tag = "LZ05"
            Me.Label11.Text = "May pha che"
            '
            'lblTen_fcode1
            '
            Me.lblTen_fcode1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTen_fcode1.Location = New System.Drawing.Point(264, 88)
            Me.lblTen_fcode1.Name = "lblTen_fcode1"
            Me.lblTen_fcode1.Size = New System.Drawing.Size(233, 15)
            Me.lblTen_fcode1.TabIndex = 156
            Me.lblTen_fcode1.Tag = "FCRF"
            Me.lblTen_fcode1.Text = "Ten bo phan"
            '
            'txtType
            '
            Me.txtType.BackColor = System.Drawing.Color.White
            Me.txtType.Location = New System.Drawing.Point(155, 107)
            Me.txtType.Name = "txtType"
            Me.txtType.Size = New System.Drawing.Size(24, 20)
            Me.txtType.TabIndex = 4
            Me.txtType.Tag = "FCCF"
            Me.txtType.Text = "*"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(23, 109)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(81, 16)
            Me.Label1.TabIndex = 158
            Me.Label1.Tag = "Z06"
            Me.Label1.Text = "Kieu lay du lieu"
            '
            'Label3
            '
            Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.Label3.Location = New System.Drawing.Point(184, 110)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(392, 15)
            Me.Label3.TabIndex = 159
            Me.Label3.Tag = "Z07"
            Me.Label3.Text = "* - Tat ca, 0 - Nguyen vat lieu, 1 - Bao bi dong goi 1, 2- Bao bi dong goi 2"
            '
            'frmDateLSX
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(608, 177)
            Me.Controls.Add(Me.txtType)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.txtFcode1)
            Me.Controls.Add(Me.Label11)
            Me.Controls.Add(Me.lblTen_fcode1)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.txtMa_lo)
            Me.Controls.Add(Me.lblTen_vt)
            Me.Controls.Add(Me.lblMa_vt)
            Me.Controls.Add(Me.txtMa_vt)
            Me.Controls.Add(Me.lblNgay_ct)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.txtNgay_ct)
            Me.Controls.Add(Me.grpInfor)
            Me.Name = "frmDateLSX"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmDate"
            Me.ResumeLayout(False)

        End Sub

        ' Properties
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents grpInfor As GroupBox
        Friend WithEvents lblNgay_ct As Label
        Friend WithEvents txtNgay_ct As txtDate

        Private components As IContainer
    End Class
End Namespace

