Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol

Namespace z16SocoBalance
    Public Class frmDirInfor
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
            AddHandler MyBase.Closed, New EventHandler(AddressOf Me.frmDirInfor_Closed)
            Me.InitializeComponent()
        End Sub

        Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
            Dim strKeyField As String = Strings.Trim(StringType.FromInteger(DirMain.nBgYear)) & ", " & Sql.ConvertVS2SQLType(Me.txtMa_dvcs.Text, "") & "," & Sql.ConvertVS2SQLType(Me.txtMa_vt.Text, "")
            oDirFormLib.SaveFormDir(Me, strKeyField)
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub frmDirInfor_Closed(ByVal sender As Object, ByVal e As EventArgs)
            oDirFormLib.frmUpdate = New frmDirInfor
        End Sub

        Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
            Unit.SetUnit(oDirFormLib.appConn, Me.txtMa_dvcs)
            If (StringType.StrCmp(oDirFormLib.cAction, "New", False) = 0) Then
                Me.txtNam.Text = StringType.FromInteger(DirMain.nBgYear)
                Unit.SetUnit(Me.txtMa_dvcs)
            End If
            Dim obj4 As Object = New DirLib(Me.txtMa_dvcs, Me.lblTen_dvcs, oDirFormLib.sysConn, oDirFormLib.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", False, Me.cmdCancel)
            Dim oItem As Object = New DirLib(Me.txtMa_vt, Me.lblTen_vt, oDirFormLib.sysConn, oDirFormLib.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "loai_vt = '51' or loai_vt='61' or loai_vt='41'", False, Me.cmdCancel)
        End Sub

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
            Me.lblNgay_ct = New System.Windows.Forms.Label()
            Me.lblTy_gia = New System.Windows.Forms.Label()
            Me.cmdOk = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.grpInfor = New System.Windows.Forms.GroupBox()
            Me.lblMa_nt = New System.Windows.Forms.Label()
            Me.txtTon00 = New libscontrol.txtNumeric()
            Me.lblTen_dvcs = New System.Windows.Forms.Label()
            Me.txtMa_vt = New System.Windows.Forms.TextBox()
            Me.txtDu00 = New libscontrol.txtNumeric()
            Me.txtDu_nt00 = New libscontrol.txtNumeric()
            Me.lblTen_vt = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.txtNam = New System.Windows.Forms.TextBox()
            Me.txtdien_giai = New System.Windows.Forms.TextBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'txtMa_dvcs
            '
            Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New System.Drawing.Point(186, 18)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.Size = New System.Drawing.Size(120, 22)
            Me.txtMa_dvcs.TabIndex = 0
            Me.txtMa_dvcs.Tag = "FCNBDF"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            '
            'lblNgay_ct
            '
            Me.lblNgay_ct.AutoSize = True
            Me.lblNgay_ct.Location = New System.Drawing.Point(29, 50)
            Me.lblNgay_ct.Name = "lblNgay_ct"
            Me.lblNgay_ct.Size = New System.Drawing.Size(45, 17)
            Me.lblNgay_ct.TabIndex = 5
            Me.lblNgay_ct.Tag = "L002"
            Me.lblNgay_ct.Text = "Vat tu"
            '
            'lblTy_gia
            '
            Me.lblTy_gia.AutoSize = True
            Me.lblTy_gia.Location = New System.Drawing.Point(29, 76)
            Me.lblTy_gia.Name = "lblTy_gia"
            Me.lblTy_gia.Size = New System.Drawing.Size(79, 17)
            Me.lblTy_gia.TabIndex = 7
            Me.lblTy_gia.Tag = "L003"
            Me.lblTy_gia.Text = "Ton dau ky"
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.Location = New System.Drawing.Point(10, 220)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.Size = New System.Drawing.Size(90, 26)
            Me.cmdOk.TabIndex = 9
            Me.cmdOk.Tag = "L008"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(101, 220)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(90, 26)
            Me.cmdCancel.TabIndex = 10
            Me.cmdCancel.Tag = "L009"
            Me.cmdCancel.Text = "Huy"
            '
            'grpInfor
            '
            Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grpInfor.Location = New System.Drawing.Point(10, 8)
            Me.grpInfor.Name = "grpInfor"
            Me.grpInfor.Size = New System.Drawing.Size(668, 200)
            Me.grpInfor.TabIndex = 1
            Me.grpInfor.TabStop = False
            '
            'lblMa_nt
            '
            Me.lblMa_nt.AutoSize = True
            Me.lblMa_nt.Location = New System.Drawing.Point(29, 23)
            Me.lblMa_nt.Name = "lblMa_nt"
            Me.lblMa_nt.Size = New System.Drawing.Size(48, 17)
            Me.lblMa_nt.TabIndex = 22
            Me.lblMa_nt.Tag = "L001"
            Me.lblMa_nt.Text = "Don vi"
            '
            'txtTon00
            '
            Me.txtTon00.Format = "m_ip_tien"
            Me.txtTon00.Location = New System.Drawing.Point(186, 72)
            Me.txtTon00.MaxLength = 10
            Me.txtTon00.Name = "txtTon00"
            Me.txtTon00.Size = New System.Drawing.Size(120, 22)
            Me.txtTon00.TabIndex = 4
            Me.txtTon00.Tag = "FN"
            Me.txtTon00.Text = "m_ip_tien"
            Me.txtTon00.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtTon00.Value = 0R
            '
            'lblTen_dvcs
            '
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New System.Drawing.Point(307, 23)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New System.Drawing.Size(70, 17)
            Me.lblTen_dvcs.TabIndex = 25
            Me.lblTen_dvcs.Tag = "RF"
            Me.lblTen_dvcs.Text = "Ten doi vi"
            '
            'txtMa_vt
            '
            Me.txtMa_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_vt.Location = New System.Drawing.Point(186, 45)
            Me.txtMa_vt.Name = "txtMa_vt"
            Me.txtMa_vt.Size = New System.Drawing.Size(120, 22)
            Me.txtMa_vt.TabIndex = 1
            Me.txtMa_vt.Tag = "FCNBDF"
            Me.txtMa_vt.Text = "TXTMA_VT"
            '
            'txtDu00
            '
            Me.txtDu00.Format = "m_ip_tien"
            Me.txtDu00.Location = New System.Drawing.Point(186, 99)
            Me.txtDu00.MaxLength = 10
            Me.txtDu00.Name = "txtDu00"
            Me.txtDu00.Size = New System.Drawing.Size(120, 22)
            Me.txtDu00.TabIndex = 5
            Me.txtDu00.Tag = "FN"
            Me.txtDu00.Text = "m_ip_tien"
            Me.txtDu00.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtDu00.Value = 0R
            '
            'txtDu_nt00
            '
            Me.txtDu_nt00.Format = "m_ip_tien_nt"
            Me.txtDu_nt00.Location = New System.Drawing.Point(186, 127)
            Me.txtDu_nt00.MaxLength = 13
            Me.txtDu_nt00.Name = "txtDu_nt00"
            Me.txtDu_nt00.Size = New System.Drawing.Size(120, 22)
            Me.txtDu_nt00.TabIndex = 6
            Me.txtDu_nt00.Tag = "FN"
            Me.txtDu_nt00.Text = "m_ip_tien_nt"
            Me.txtDu_nt00.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtDu_nt00.Value = 0R
            '
            'lblTen_vt
            '
            Me.lblTen_vt.AutoSize = True
            Me.lblTen_vt.Location = New System.Drawing.Point(307, 50)
            Me.lblTen_vt.Name = "lblTen_vt"
            Me.lblTen_vt.Size = New System.Drawing.Size(48, 17)
            Me.lblTen_vt.TabIndex = 34
            Me.lblTen_vt.Tag = "RF"
            Me.lblTen_vt.Text = "Ten vt"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(29, 104)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(72, 17)
            Me.Label2.TabIndex = 35
            Me.Label2.Tag = "L004"
            Me.Label2.Text = "Du dau ky"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(29, 132)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(127, 17)
            Me.Label3.TabIndex = 36
            Me.Label3.Tag = "L005"
            Me.Label3.Text = "Du dau ky ngoai te"
            '
            'txtNam
            '
            Me.txtNam.Location = New System.Drawing.Point(250, 323)
            Me.txtNam.Name = "txtNam"
            Me.txtNam.Size = New System.Drawing.Size(120, 22)
            Me.txtNam.TabIndex = 41
            Me.txtNam.Tag = "FN"
            Me.txtNam.Text = "txtNam"
            Me.txtNam.Visible = False
            '
            'txtdien_giai
            '
            Me.txtdien_giai.Location = New System.Drawing.Point(186, 153)
            Me.txtdien_giai.Name = "txtdien_giai"
            Me.txtdien_giai.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.txtdien_giai.Size = New System.Drawing.Size(437, 22)
            Me.txtdien_giai.TabIndex = 8
            Me.txtdien_giai.Tag = "FC"
            Me.txtdien_giai.Text = "txtdien_giai"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(29, 158)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(63, 17)
            Me.Label1.TabIndex = 43
            Me.Label1.Tag = "L012"
            Me.Label1.Text = "Dien giai"
            '
            'frmDirInfor
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
            Me.ClientSize = New System.Drawing.Size(688, 252)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.txtdien_giai)
            Me.Controls.Add(Me.txtNam)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.lblTen_vt)
            Me.Controls.Add(Me.txtDu_nt00)
            Me.Controls.Add(Me.txtDu00)
            Me.Controls.Add(Me.txtTon00)
            Me.Controls.Add(Me.lblTy_gia)
            Me.Controls.Add(Me.lblNgay_ct)
            Me.Controls.Add(Me.lblMa_nt)
            Me.Controls.Add(Me.lblTen_dvcs)
            Me.Controls.Add(Me.txtMa_vt)
            Me.Controls.Add(Me.txtMa_dvcs)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.grpInfor)
            Me.Name = "frmDirInfor"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmDirInfor"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        ' Properties
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents grpInfor As GroupBox
        Friend WithEvents Label1 As Label
        Friend WithEvents Label2 As Label
        Friend WithEvents Label3 As Label
        Friend WithEvents lblMa_nt As Label
        Friend WithEvents lblNgay_ct As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTen_vt As Label
        Friend WithEvents lblTy_gia As Label
        Friend WithEvents txtdien_giai As TextBox
        Friend WithEvents txtDu00 As txtNumeric
        Friend WithEvents txtDu_nt00 As txtNumeric
        Friend WithEvents txtTon00 As txtNumeric
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtNam As TextBox
        Friend WithEvents txtMa_vt As TextBox

        Private components As IContainer
    End Class
End Namespace

