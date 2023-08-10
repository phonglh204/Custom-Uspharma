Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon


Namespace coctsx1
    Public Class frmDate
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
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("064")), 2)
                Return
            End If
            Me.Close()
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub frmDate_Load(ByVal sender As Object, ByVal e As EventArgs)
            Me.Text = StringType.FromObject(modVoucher.oLan.Item("300"))
            Dim control As Control
            For Each control In Me.Controls
                If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                    control.Text = StringType.FromObject(modVoucher.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
                End If
            Next
            Obj.Init(Me)
            Me.txtNgay_ct.AddCalenderControl()
            Me.txtNgay_ct.Value = DateAndTime.Now.Date
            Dim sKey As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keycust", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            Dim oCust As New DirLib(Me.txtMa_kh, Me.lblTen_kh, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", sKey, False, Me.cmdCancel)
            Dim oItem As New DirLib(Me.txtMa_vt, Me.lblTen_vt, modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "loai_vt='51'", False, Me.cmdCancel)
            'Me.txtMa_kh.Text = "USPHARMA"
            'Me.lblTen_kh.Text = Sql.GetValue(appConn, "dmkh", "ten_kh", "ma_kh=''")
        End Sub
        Friend WithEvents txtMa_kh As TextBox
        Friend WithEvents Label2 As Label
        Friend WithEvents lblTen_vt As Label
        Friend WithEvents lblMa_vt As Label
        Friend WithEvents txtMa_vt As TextBox
        Friend WithEvents lblTen_kh As Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblNgay_ct = New System.Windows.Forms.Label()
            Me.cmdOk = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.grpInfor = New System.Windows.Forms.GroupBox()
            Me.txtNgay_ct = New libscontrol.txtDate()
            Me.txtMa_kh = New System.Windows.Forms.TextBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.lblTen_kh = New System.Windows.Forms.Label()
            Me.lblTen_vt = New System.Windows.Forms.Label()
            Me.lblMa_vt = New System.Windows.Forms.Label()
            Me.txtMa_vt = New System.Windows.Forms.TextBox()
            Me.SuspendLayout()
            '
            'lblNgay_ct
            '
            Me.lblNgay_ct.AutoSize = True
            Me.lblNgay_ct.Location = New System.Drawing.Point(28, 74)
            Me.lblNgay_ct.Name = "lblNgay_ct"
            Me.lblNgay_ct.Size = New System.Drawing.Size(126, 17)
            Me.lblNgay_ct.TabIndex = 7
            Me.lblNgay_ct.Tag = "L301"
            Me.lblNgay_ct.Text = "Ngay chung tu moi"
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.cmdOk.Location = New System.Drawing.Point(10, 184)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.Size = New System.Drawing.Size(90, 26)
            Me.cmdOk.TabIndex = 2
            Me.cmdOk.Tag = "L302"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(101, 184)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(90, 26)
            Me.cmdCancel.TabIndex = 3
            Me.cmdCancel.Tag = "L303"
            Me.cmdCancel.Text = "Huy"
            '
            'grpInfor
            '
            Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grpInfor.Location = New System.Drawing.Point(10, 9)
            Me.grpInfor.Name = "grpInfor"
            Me.grpInfor.Size = New System.Drawing.Size(665, 169)
            Me.grpInfor.TabIndex = 17
            Me.grpInfor.TabStop = False
            '
            'txtNgay_ct
            '
            Me.txtNgay_ct.Location = New System.Drawing.Point(186, 74)
            Me.txtNgay_ct.MaxLength = 10
            Me.txtNgay_ct.Name = "txtNgay_ct"
            Me.txtNgay_ct.Size = New System.Drawing.Size(120, 22)
            Me.txtNgay_ct.TabIndex = 1
            Me.txtNgay_ct.Text = "01/01/1900"
            Me.txtNgay_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNgay_ct.Value = New Date(1900, 1, 1, 0, 0, 0, 0)
            '
            'txtMa_kh
            '
            Me.txtMa_kh.BackColor = System.Drawing.Color.White
            Me.txtMa_kh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_kh.Location = New System.Drawing.Point(186, 46)
            Me.txtMa_kh.Name = "txtMa_kh"
            Me.txtMa_kh.Size = New System.Drawing.Size(120, 22)
            Me.txtMa_kh.TabIndex = 0
            Me.txtMa_kh.Tag = "FCNBCF"
            Me.txtMa_kh.Text = "TXTMA_KH"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(28, 48)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(84, 17)
            Me.Label2.TabIndex = 146
            Me.Label2.Tag = "L002"
            Me.Label2.Text = "Khach hang"
            '
            'lblTen_kh
            '
            Me.lblTen_kh.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTen_kh.Location = New System.Drawing.Point(317, 50)
            Me.lblTen_kh.Name = "lblTen_kh"
            Me.lblTen_kh.Size = New System.Drawing.Size(234, 17)
            Me.lblTen_kh.TabIndex = 147
            Me.lblTen_kh.Tag = "FCRF"
            Me.lblTen_kh.Text = "Ten khach hang"
            '
            'lblTen_vt
            '
            Me.lblTen_vt.AutoSize = True
            Me.lblTen_vt.Location = New System.Drawing.Point(311, 105)
            Me.lblTen_vt.Name = "lblTen_vt"
            Me.lblTen_vt.Size = New System.Drawing.Size(72, 17)
            Me.lblTen_vt.TabIndex = 150
            Me.lblTen_vt.Tag = ""
            Me.lblTen_vt.Text = "Ten vat tu"
            '
            'lblMa_vt
            '
            Me.lblMa_vt.AutoSize = True
            Me.lblMa_vt.Location = New System.Drawing.Point(28, 105)
            Me.lblMa_vt.Name = "lblMa_vt"
            Me.lblMa_vt.Size = New System.Drawing.Size(66, 17)
            Me.lblMa_vt.TabIndex = 149
            Me.lblMa_vt.Tag = "L125"
            Me.lblMa_vt.Text = "Ma vat tu"
            '
            'txtMa_vt
            '
            Me.txtMa_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_vt.Location = New System.Drawing.Point(186, 102)
            Me.txtMa_vt.Name = "txtMa_vt"
            Me.txtMa_vt.Size = New System.Drawing.Size(120, 22)
            Me.txtMa_vt.TabIndex = 148
            Me.txtMa_vt.Tag = "FCDetail#ma_vt like '%s%'#ML"
            Me.txtMa_vt.Text = "TXTMA_VT"
            '
            'frmDate
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
            Me.ClientSize = New System.Drawing.Size(685, 216)
            Me.Controls.Add(Me.lblTen_vt)
            Me.Controls.Add(Me.lblMa_vt)
            Me.Controls.Add(Me.txtMa_vt)
            Me.Controls.Add(Me.txtMa_kh)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.lblTen_kh)
            Me.Controls.Add(Me.lblNgay_ct)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.txtNgay_ct)
            Me.Controls.Add(Me.grpInfor)
            Me.Name = "frmDate"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmDate"
            Me.ResumeLayout(False)
            Me.PerformLayout()

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

