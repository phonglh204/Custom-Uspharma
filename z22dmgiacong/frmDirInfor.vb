Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon


Namespace z22dmgiacong
    Public Class frmDirInfor
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Closed, New EventHandler(AddressOf Me.frmDirInfor_Closed)
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
            Me.InitializeComponent()
        End Sub

        Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
            DirMain.oDirFormLib.SaveFormDir(Me, StringType.FromObject(Sql.ConvertVS2SQLType(Me.txtngay_hl.Value, "")) + "," + StringType.FromObject(Sql.ConvertVS2SQLType(Me.txtMa_bp.Text, "")))
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub frmDirInfor_Closed(ByVal sender As Object, ByVal e As EventArgs)
            DirMain.oDirFormLib.frmUpdate = New frmDirInfor
        End Sub

        Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
            If (StringType.StrCmp(DirMain.oDirFormLib.cAction, "New", False) = 0) Then
                Me.txtstatus.Text = "1"
                If (DirMain.oDirFormLib.oDir.ob.dv.Count > 0) Then
                    Me.txtngay_hl.Value = DateType.FromObject(DirMain.oDirFormLib.oDir.ob.dv.Item(DirMain.oDirFormLib.oDir.ob.grdLookup.CurrentRowIndex).Item("ngay_hl"))
                End If
            End If
            Me.txtstatus.MaxLength = 1
            Dim obj2 As Object = New CharLib(Me.txtstatus, "0, 1")
            Dim obj3 As Object = New DirLib(Me.txtMa_bp, Me.lblTen_bp, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "vxdmbp", "ma_bp", "ten_bp", "v20CODept", "1=1", False, Me.cmdCancel)
        End Sub
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents txtGia1 As txtNumeric
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents txtGia2 As txtNumeric
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents txtGia4 As txtNumeric
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents txtGia3 As txtNumeric
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents txtGia5 As txtNumeric
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents txtMa_bp As System.Windows.Forms.TextBox
        Friend WithEvents lblMa_bp As System.Windows.Forms.Label
        Friend WithEvents lblTen_bp As System.Windows.Forms.Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.cmdOk = New System.Windows.Forms.Button
            Me.cmdCancel = New System.Windows.Forms.Button
            Me.GroupBox1 = New System.Windows.Forms.GroupBox
            Me.lblten_tt = New System.Windows.Forms.Label
            Me.lblngay_hl = New System.Windows.Forms.Label
            Me.txtngay_hl = New txtDate
            Me.lblstatus = New System.Windows.Forms.Label
            Me.txtstatus = New System.Windows.Forms.TextBox
            Me.txtCs_qd = New txtNumeric
            Me.txthe_so = New txtNumeric
            Me.Label1 = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.txtGia1 = New txtNumeric
            Me.Label3 = New System.Windows.Forms.Label
            Me.txtGia2 = New txtNumeric
            Me.Label4 = New System.Windows.Forms.Label
            Me.txtGia4 = New txtNumeric
            Me.Label5 = New System.Windows.Forms.Label
            Me.txtGia3 = New txtNumeric
            Me.Label6 = New System.Windows.Forms.Label
            Me.txtGia5 = New txtNumeric
            Me.GroupBox2 = New System.Windows.Forms.GroupBox
            Me.txtMa_bp = New System.Windows.Forms.TextBox
            Me.lblMa_bp = New System.Windows.Forms.Label
            Me.lblTen_bp = New System.Windows.Forms.Label
            Me.SuspendLayout()
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.Location = New System.Drawing.Point(8, 301)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 8
            Me.cmdOk.Tag = "L094"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(84, 301)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 9
            Me.cmdCancel.Tag = "L095"
            Me.cmdCancel.Text = "Huy"
            '
            'GroupBox1
            '
            Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.GroupBox1.Location = New System.Drawing.Point(8, 3)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(592, 277)
            Me.GroupBox1.TabIndex = 0
            Me.GroupBox1.TabStop = False
            '
            'lblten_tt
            '
            Me.lblten_tt.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblten_tt.AutoSize = True
            Me.lblten_tt.Location = New System.Drawing.Point(184, 248)
            Me.lblten_tt.Name = "lblten_tt"
            Me.lblten_tt.Size = New System.Drawing.Size(207, 16)
            Me.lblten_tt.TabIndex = 99
            Me.lblten_tt.Tag = "L090"
            Me.lblten_tt.Text = " 1 - Con su dung, 0 - Khong con su dung"
            Me.lblten_tt.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'lblngay_hl
            '
            Me.lblngay_hl.AutoSize = True
            Me.lblngay_hl.Location = New System.Drawing.Point(23, 18)
            Me.lblngay_hl.Name = "lblngay_hl"
            Me.lblngay_hl.Size = New System.Drawing.Size(58, 16)
            Me.lblngay_hl.TabIndex = 2
            Me.lblngay_hl.Tag = "L002"
            Me.lblngay_hl.Text = "Hieu luc tu"
            Me.lblngay_hl.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'txtngay_hl
            '
            Me.txtngay_hl.Location = New System.Drawing.Point(155, 16)
            Me.txtngay_hl.MaxLength = 10
            Me.txtngay_hl.Name = "txtngay_hl"
            Me.txtngay_hl.TabIndex = 0
            Me.txtngay_hl.Tag = "FDNBDF"
            Me.txtngay_hl.Text = "  /  /    "
            Me.txtngay_hl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtngay_hl.Value = New Date(CType(0, Long))
            '
            'lblstatus
            '
            Me.lblstatus.AutoSize = True
            Me.lblstatus.Location = New System.Drawing.Point(23, 248)
            Me.lblstatus.Name = "lblstatus"
            Me.lblstatus.Size = New System.Drawing.Size(55, 16)
            Me.lblstatus.TabIndex = 5
            Me.lblstatus.Tag = "L006"
            Me.lblstatus.Text = "Trang thai"
            Me.lblstatus.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'txtstatus
            '
            Me.txtstatus.Location = New System.Drawing.Point(155, 248)
            Me.txtstatus.Name = "txtstatus"
            Me.txtstatus.Size = New System.Drawing.Size(25, 20)
            Me.txtstatus.TabIndex = 7
            Me.txtstatus.TabStop = False
            Me.txtstatus.Tag = "FC"
            Me.txtstatus.Text = "txtstatus"
            Me.txtstatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'txtCs_qd
            '
            Me.txtCs_qd.Format = "m_ip_cs"
            Me.txtCs_qd.Location = New System.Drawing.Point(304, 296)
            Me.txtCs_qd.MaxLength = 8
            Me.txtCs_qd.Name = "txtCs_qd"
            Me.txtCs_qd.TabIndex = 101
            Me.txtCs_qd.Tag = "FNDF"
            Me.txtCs_qd.Text = "m_ip_cs"
            Me.txtCs_qd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtCs_qd.Value = 0
            Me.txtCs_qd.Visible = False
            '
            'txthe_so
            '
            Me.txthe_so.Format = "m_ip_sl"
            Me.txthe_so.Location = New System.Drawing.Point(440, 296)
            Me.txthe_so.MaxLength = 8
            Me.txthe_so.Name = "txthe_so"
            Me.txthe_so.TabIndex = 102
            Me.txthe_so.Tag = "FNDF"
            Me.txthe_so.Text = "m_ip_sl"
            Me.txthe_so.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txthe_so.Value = 0
            Me.txthe_so.Visible = False
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(48, 96)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(43, 16)
            Me.Label1.TabIndex = 103
            Me.Label1.Tag = "L022"
            Me.Label1.Text = "Don gia"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(32, 122)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(46, 16)
            Me.Label2.TabIndex = 105
            Me.Label2.Tag = "L017"
            Me.Label2.Text = "Pha che"
            Me.Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'txtGia1
            '
            Me.txtGia1.Format = "m_ip_tien"
            Me.txtGia1.Location = New System.Drawing.Point(152, 120)
            Me.txtGia1.MaxLength = 10
            Me.txtGia1.Name = "txtGia1"
            Me.txtGia1.TabIndex = 2
            Me.txtGia1.Tag = "FNDF"
            Me.txtGia1.Text = "m_ip_tien"
            Me.txtGia1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtGia1.Value = 0
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(32, 146)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(110, 16)
            Me.Label3.TabIndex = 107
            Me.Label3.Tag = "L018"
            Me.Label3.Text = "Dap vien/ Dong nang"
            Me.Label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'txtGia2
            '
            Me.txtGia2.Format = "m_ip_tien"
            Me.txtGia2.Location = New System.Drawing.Point(152, 144)
            Me.txtGia2.MaxLength = 10
            Me.txtGia2.Name = "txtGia2"
            Me.txtGia2.TabIndex = 3
            Me.txtGia2.Tag = "FNDF"
            Me.txtGia2.Text = "m_ip_tien"
            Me.txtGia2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtGia2.Value = 0
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(32, 194)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(59, 16)
            Me.Label4.TabIndex = 111
            Me.Label4.Tag = "L020"
            Me.Label4.Text = "Dong goi 1"
            Me.Label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'txtGia4
            '
            Me.txtGia4.Format = "m_ip_tien"
            Me.txtGia4.Location = New System.Drawing.Point(152, 192)
            Me.txtGia4.MaxLength = 10
            Me.txtGia4.Name = "txtGia4"
            Me.txtGia4.TabIndex = 5
            Me.txtGia4.Tag = "FNDF"
            Me.txtGia4.Text = "m_ip_tien"
            Me.txtGia4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtGia4.Value = 0
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(32, 170)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(52, 16)
            Me.Label5.TabIndex = 109
            Me.Label5.Tag = "L019"
            Me.Label5.Text = "Bao phim"
            Me.Label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'txtGia3
            '
            Me.txtGia3.Format = "m_ip_tien"
            Me.txtGia3.Location = New System.Drawing.Point(152, 168)
            Me.txtGia3.MaxLength = 10
            Me.txtGia3.Name = "txtGia3"
            Me.txtGia3.TabIndex = 4
            Me.txtGia3.Tag = "FNDF"
            Me.txtGia3.Text = "m_ip_tien"
            Me.txtGia3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtGia3.Value = 0
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(32, 218)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(59, 16)
            Me.Label6.TabIndex = 113
            Me.Label6.Tag = "L021"
            Me.Label6.Text = "Dong goi 2"
            Me.Label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'txtGia5
            '
            Me.txtGia5.Format = "m_ip_tien"
            Me.txtGia5.Location = New System.Drawing.Point(152, 216)
            Me.txtGia5.MaxLength = 10
            Me.txtGia5.Name = "txtGia5"
            Me.txtGia5.TabIndex = 6
            Me.txtGia5.Tag = "FNDF"
            Me.txtGia5.Text = "m_ip_tien"
            Me.txtGia5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtGia5.Value = 0
            '
            'GroupBox2
            '
            Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.GroupBox2.Location = New System.Drawing.Point(24, 112)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(568, 128)
            Me.GroupBox2.TabIndex = 114
            Me.GroupBox2.TabStop = False
            '
            'txtMa_bp
            '
            Me.txtMa_bp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_bp.Location = New System.Drawing.Point(155, 40)
            Me.txtMa_bp.Name = "txtMa_bp"
            Me.txtMa_bp.TabIndex = 1
            Me.txtMa_bp.Tag = "FCNBDFML"
            Me.txtMa_bp.Text = "TXTMA_VT"
            '
            'lblMa_bp
            '
            Me.lblMa_bp.AutoSize = True
            Me.lblMa_bp.Location = New System.Drawing.Point(23, 42)
            Me.lblMa_bp.Name = "lblMa_bp"
            Me.lblMa_bp.Size = New System.Drawing.Size(36, 16)
            Me.lblMa_bp.TabIndex = 116
            Me.lblMa_bp.Tag = "L001"
            Me.lblMa_bp.Text = "Ma bp"
            '
            'lblTen_bp
            '
            Me.lblTen_bp.AutoSize = True
            Me.lblTen_bp.Location = New System.Drawing.Point(264, 42)
            Me.lblTen_bp.Name = "lblTen_bp"
            Me.lblTen_bp.Size = New System.Drawing.Size(54, 16)
            Me.lblTen_bp.TabIndex = 117
            Me.lblTen_bp.Tag = "RF"
            Me.lblTen_bp.Text = "Ten vat tu"
            '
            'frmDirInfor
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(608, 329)
            Me.Controls.Add(Me.txtMa_bp)
            Me.Controls.Add(Me.lblMa_bp)
            Me.Controls.Add(Me.lblTen_bp)
            Me.Controls.Add(Me.Label6)
            Me.Controls.Add(Me.txtGia5)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.txtGia4)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.txtGia3)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.txtGia2)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.txtGia1)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.txthe_so)
            Me.Controls.Add(Me.txtCs_qd)
            Me.Controls.Add(Me.lblngay_hl)
            Me.Controls.Add(Me.txtngay_hl)
            Me.Controls.Add(Me.lblstatus)
            Me.Controls.Add(Me.txtstatus)
            Me.Controls.Add(Me.lblten_tt)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.GroupBox2)
            Me.Controls.Add(Me.GroupBox1)
            Me.Name = "frmDirInfor"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmDirInfor"
            Me.ResumeLayout(False)

        End Sub



        ' Properties
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents GroupBox1 As GroupBox
        Friend WithEvents lblngay_hl As Label
        Friend WithEvents lblstatus As Label
        Friend WithEvents lblten_tt As Label
        Friend WithEvents txtCs_qd As txtNumeric
        Friend WithEvents txthe_so As txtNumeric
        Friend WithEvents txtngay_hl As txtDate
        Friend WithEvents txtstatus As TextBox


        Private cOldString As String
        Private components As IContainer
        Private oUOM As dirblanklib
    End Class
End Namespace

