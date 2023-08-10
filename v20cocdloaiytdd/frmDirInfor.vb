Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol

Namespace v20cocdloaiytdd
    Public Class frmDirInfor
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Closed, New EventHandler(AddressOf Me.frmDirInfor_Closed)
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
            Me.lblRef = New Label
            Me.InitializeComponent()
        End Sub

        Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
            Me.txtSo_lsx.Text = Fox.PadL(Strings.Trim(Me.txtSo_lsx.Text), Me.txtSo_lsx.MaxLength)
            Dim strKeyField As String = (StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(Sql.ConvertVS2SQLType(Me.txtNam.Value, "")), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Me.txtKy.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Me.txtLoai_yt.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Me.txtMa_sp.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Me.txtMa_bp.Text, "")))) & ", '" & Strings.Replace(Me.txtSo_lsx.Text, "'", "''", 1, -1, CompareMethod.Binary) & "'")
            DirMain.oDirFormLib.SaveFormDir(Me, strKeyField)
            Dim sqlstr As String
            sqlstr = "EXEC fs21_UpdateEndMO " + Sql.ConvertVS2SQLType(txtSo_lsx.Text.Trim, "")
            sqlstr += "," + nPeriod.ToString.Trim + "," + nYear.ToString.Trim
            sqlstr += "," + txtSl_dd_ck0.Value.ToString.Trim
            Sql.SQLExecute(oDirFormLib.appConn, sqlstr)
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
                Me.txtKy.Value = DirMain.nPeriod
                Me.txtNam.Value = DirMain.nYear
            End If
            Dim lib2 As New DirLib(Me.txtLoai_yt, Me.lblTen_loai_yt, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "xdmloaiyt", "ma_loai", "ten_loai", "v20COFactorStyle", "1=1", False, Me.cmdCancel)
            Dim lib1 As New DirLib(Me.txtMa_sp, Me.lblTen_vt, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "vdmsp", "ma_vt", "ten_vt", "Product", "1=1", False, Me.cmdCancel)
            If (ObjectType.ObjTst(DirMain.oDirFormLib.oOptions.Item("x_dt_bp"), 1, False) = 0) Then
                Me.txtMa_bp.Tag = "FCNB"
                Dim lib3 As New DirLib(Me.txtMa_bp, Me.lblTen_bp, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "vxdmbp", "ma_bp", "ten_bp", "v20CODept", "1=1", False, Me.cmdCancel)
            Else
                Me.txtMa_bp.Text = ""
                Me.lblTen_bp.Text = ""
                Me.txtMa_bp.Enabled = False
            End If
            If (ObjectType.ObjTst(DirMain.oDirFormLib.oOptions.Item("x_dt_lsx"), 1, False) = 0) Then
                Me.txtSo_lsx.Tag = "FCNB"
                Dim lib4 As New DirLib(Me.txtSo_lsx, New Label, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmlsx", "so_lsx", "dien_giai", "MONumberx", "1=1", False, Me.cmdCancel)
            Else
                Me.txtSo_lsx.Text = ""
                Me.txtSo_lsx.Enabled = False
            End If
            Me.Init()
        End Sub

        Private Sub Init()
            ' AddHandler Me.txtSl_dd_ck0.Leave, New EventHandler(AddressOf Me.txtValid)
            'AddHandler Me.txtTl_ht.Leave, New EventHandler(AddressOf Me.txtValid)
            'AddHandler Me.txtSl_dd_ck0.Enter, New EventHandler(AddressOf Me.txtEnter)
            'AddHandler Me.txtTl_ht.Enter, New EventHandler(AddressOf Me.txtEnter)
        End Sub
        Sub rate_cal()
            Dim a, b, c, d, e, tu, mau, kq As Double
            a = txtTl_ht1.Value
            b = txtTl_ht2.Value
            c = txtTl_ht3.Value
            d = txtTl_ht4.Value
            e = txtTl_ht5.Value
            tu = a / 100 + a * b / (100 * 100) + a * b * c / (100 * 100 * 100) + a * b * c * d / (100 * 100 * 100 * 100) + (a / 100) * (b / 100) * (c / 100) * (d / 100) * (e / 100)
            mau = 1 + a / 100 + a * b / (100 * 100) + a * b * c / (100 * 100 * 100) + (a / 100) * (b / 100) * (c / 100) * (d / 100)
            If mau = 0 Then
                Me.txtTl_ht.Value = 0
                Me.txtSl_dd_ck.Value = 0
            Else
                Me.txtTl_ht.Value = Math.Round(tu * 100 / mau, 2)
                Me.txtSl_dd_ck.Value = Math.Round(Me.txtSl_dd_ck0.Value * Math.Round(tu * 100 / mau, 2) / 100, 0)
            End If
        End Sub

        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents txtTl_ht1 As txtNumeric
        Friend WithEvents txtTl_ht2 As txtNumeric
        Friend WithEvents txtTl_ht3 As txtNumeric
        Friend WithEvents txtTl_ht4 As txtNumeric
        Friend WithEvents txtTl_ht5 As txtNumeric
        Friend WithEvents txtTl_ht As txtNumeric

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.grpInfor = New System.Windows.Forms.GroupBox
            Me.lblSl_dd = New System.Windows.Forms.Label
            Me.cmdCancel = New System.Windows.Forms.Button
            Me.cmdOk = New System.Windows.Forms.Button
            Me.txtSl_dd_ck0 = New txtNumeric
            Me.txtNam = New txtNumeric
            Me.txtKy = New txtNumeric
            Me.lblMa_bp = New System.Windows.Forms.Label
            Me.txtMa_bp = New System.Windows.Forms.TextBox
            Me.lblTen_bp = New System.Windows.Forms.Label
            Me.txtSl_dd_ck = New txtNumeric
            Me.lblSl_qd = New System.Windows.Forms.Label
            Me.lblTl_ht = New System.Windows.Forms.Label
            Me.lblSo_lsx = New System.Windows.Forms.Label
            Me.txtTl_ht1 = New txtNumeric
            Me.lblMa_sp = New System.Windows.Forms.Label
            Me.txtMa_sp = New System.Windows.Forms.TextBox
            Me.lblTen_vt = New System.Windows.Forms.Label
            Me.txtSo_lsx = New System.Windows.Forms.TextBox
            Me.lblPer = New System.Windows.Forms.Label
            Me.txtLoai_yt = New System.Windows.Forms.TextBox
            Me.lblTen_loai_yt = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.txtTl_ht2 = New txtNumeric
            Me.Label4 = New System.Windows.Forms.Label
            Me.txtTl_ht3 = New txtNumeric
            Me.Label5 = New System.Windows.Forms.Label
            Me.Label6 = New System.Windows.Forms.Label
            Me.txtTl_ht4 = New txtNumeric
            Me.Label7 = New System.Windows.Forms.Label
            Me.Label8 = New System.Windows.Forms.Label
            Me.txtTl_ht5 = New txtNumeric
            Me.Label9 = New System.Windows.Forms.Label
            Me.Label10 = New System.Windows.Forms.Label
            Me.txtTl_ht = New txtNumeric
            Me.SuspendLayout()
            '
            'grpInfor
            '
            Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grpInfor.Location = New System.Drawing.Point(8, 4)
            Me.grpInfor.Name = "grpInfor"
            Me.grpInfor.Size = New System.Drawing.Size(640, 276)
            Me.grpInfor.TabIndex = 17
            Me.grpInfor.TabStop = False
            '
            'lblSl_dd
            '
            Me.lblSl_dd.AutoSize = True
            Me.lblSl_dd.Location = New System.Drawing.Point(23, 108)
            Me.lblSl_dd.Name = "lblSl_dd"
            Me.lblSl_dd.Size = New System.Drawing.Size(64, 16)
            Me.lblSl_dd.TabIndex = 77
            Me.lblSl_dd.Tag = "L005"
            Me.lblSl_dd.Text = "So luong dd"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(84, 300)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 12
            Me.cmdCancel.Tag = "L011"
            Me.cmdCancel.Text = "Huy"
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.Location = New System.Drawing.Point(8, 300)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 11
            Me.cmdOk.Tag = "L010"
            Me.cmdOk.Text = "Nhan"
            '
            'txtSl_dd_ck0
            '
            Me.txtSl_dd_ck0.BackColor = System.Drawing.Color.White
            Me.txtSl_dd_ck0.Format = "m_ip_sl"
            Me.txtSl_dd_ck0.Location = New System.Drawing.Point(155, 106)
            Me.txtSl_dd_ck0.MaxLength = 8
            Me.txtSl_dd_ck0.Name = "txtSl_dd_ck0"
            Me.txtSl_dd_ck0.TabIndex = 4
            Me.txtSl_dd_ck0.Tag = "FN"
            Me.txtSl_dd_ck0.Text = "m_ip_sl"
            Me.txtSl_dd_ck0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtSl_dd_ck0.Value = 0
            '
            'txtNam
            '
            Me.txtNam.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtNam.BackColor = System.Drawing.Color.White
            Me.txtNam.Enabled = False
            Me.txtNam.Format = "##0"
            Me.txtNam.Location = New System.Drawing.Point(456, 300)
            Me.txtNam.MaxLength = 4
            Me.txtNam.Name = "txtNam"
            Me.txtNam.Size = New System.Drawing.Size(30, 20)
            Me.txtNam.TabIndex = 2
            Me.txtNam.Tag = "FNNB"
            Me.txtNam.Text = "0"
            Me.txtNam.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNam.Value = 0
            Me.txtNam.Visible = False
            '
            'txtKy
            '
            Me.txtKy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtKy.BackColor = System.Drawing.Color.White
            Me.txtKy.Enabled = False
            Me.txtKy.Format = "#0"
            Me.txtKy.Location = New System.Drawing.Point(416, 300)
            Me.txtKy.MaxLength = 3
            Me.txtKy.Name = "txtKy"
            Me.txtKy.Size = New System.Drawing.Size(30, 20)
            Me.txtKy.TabIndex = 1
            Me.txtKy.Tag = "FNNB"
            Me.txtKy.Text = "0"
            Me.txtKy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtKy.Value = 0
            Me.txtKy.Visible = False
            '
            'lblMa_bp
            '
            Me.lblMa_bp.AutoSize = True
            Me.lblMa_bp.Location = New System.Drawing.Point(23, 64)
            Me.lblMa_bp.Name = "lblMa_bp"
            Me.lblMa_bp.Size = New System.Drawing.Size(46, 16)
            Me.lblMa_bp.TabIndex = 97
            Me.lblMa_bp.Tag = "L003"
            Me.lblMa_bp.Text = "Bo phan"
            '
            'txtMa_bp
            '
            Me.txtMa_bp.BackColor = System.Drawing.Color.White
            Me.txtMa_bp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_bp.Location = New System.Drawing.Point(155, 62)
            Me.txtMa_bp.Name = "txtMa_bp"
            Me.txtMa_bp.TabIndex = 2
            Me.txtMa_bp.Tag = "FC"
            Me.txtMa_bp.Text = "TXTMA_BP"
            '
            'lblTen_bp
            '
            Me.lblTen_bp.AutoSize = True
            Me.lblTen_bp.Location = New System.Drawing.Point(256, 64)
            Me.lblTen_bp.Name = "lblTen_bp"
            Me.lblTen_bp.Size = New System.Drawing.Size(39, 16)
            Me.lblTen_bp.TabIndex = 96
            Me.lblTen_bp.Text = "Ten bp"
            '
            'txtSl_dd_ck
            '
            Me.txtSl_dd_ck.BackColor = System.Drawing.Color.White
            Me.txtSl_dd_ck.Format = "m_ip_sl"
            Me.txtSl_dd_ck.Location = New System.Drawing.Point(155, 248)
            Me.txtSl_dd_ck.MaxLength = 8
            Me.txtSl_dd_ck.Name = "txtSl_dd_ck"
            Me.txtSl_dd_ck.TabIndex = 10
            Me.txtSl_dd_ck.Tag = "FN"
            Me.txtSl_dd_ck.Text = "m_ip_sl"
            Me.txtSl_dd_ck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtSl_dd_ck.Value = 0
            '
            'lblSl_qd
            '
            Me.lblSl_qd.AutoSize = True
            Me.lblSl_qd.Location = New System.Drawing.Point(23, 250)
            Me.lblSl_qd.Name = "lblSl_qd"
            Me.lblSl_qd.Size = New System.Drawing.Size(54, 16)
            Me.lblSl_qd.TabIndex = 104
            Me.lblSl_qd.Tag = "L007"
            Me.lblSl_qd.Text = "Sl quy doi"
            '
            'lblTl_ht
            '
            Me.lblTl_ht.AutoSize = True
            Me.lblTl_ht.Location = New System.Drawing.Point(23, 130)
            Me.lblTl_ht.Name = "lblTl_ht"
            Me.lblTl_ht.Size = New System.Drawing.Size(131, 16)
            Me.lblTl_ht.TabIndex = 108
            Me.lblTl_ht.Tag = "L014"
            Me.lblTl_ht.Text = "Ty le hoan thanh pha che"
            '
            'lblSo_lsx
            '
            Me.lblSo_lsx.AutoSize = True
            Me.lblSo_lsx.Location = New System.Drawing.Point(23, 86)
            Me.lblSo_lsx.Name = "lblSo_lsx"
            Me.lblSo_lsx.Size = New System.Drawing.Size(35, 16)
            Me.lblSo_lsx.TabIndex = 112
            Me.lblSo_lsx.Tag = "L004"
            Me.lblSo_lsx.Text = "So lsx"
            '
            'txtTl_ht1
            '
            Me.txtTl_ht1.BackColor = System.Drawing.Color.White
            Me.txtTl_ht1.Format = "#0.00"
            Me.txtTl_ht1.Location = New System.Drawing.Point(216, 128)
            Me.txtTl_ht1.MaxLength = 6
            Me.txtTl_ht1.Name = "txtTl_ht1"
            Me.txtTl_ht1.Size = New System.Drawing.Size(50, 20)
            Me.txtTl_ht1.TabIndex = 5
            Me.txtTl_ht1.Tag = "FN"
            Me.txtTl_ht1.Text = "0.00"
            Me.txtTl_ht1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtTl_ht1.Value = 0
            '
            'lblMa_sp
            '
            Me.lblMa_sp.AutoSize = True
            Me.lblMa_sp.Location = New System.Drawing.Point(23, 20)
            Me.lblMa_sp.Name = "lblMa_sp"
            Me.lblMa_sp.Size = New System.Drawing.Size(59, 16)
            Me.lblMa_sp.TabIndex = 113
            Me.lblMa_sp.Tag = "L001"
            Me.lblMa_sp.Text = "Loai yeu to"
            '
            'txtMa_sp
            '
            Me.txtMa_sp.BackColor = System.Drawing.Color.White
            Me.txtMa_sp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_sp.Location = New System.Drawing.Point(155, 40)
            Me.txtMa_sp.Name = "txtMa_sp"
            Me.txtMa_sp.TabIndex = 1
            Me.txtMa_sp.Tag = "FCNB"
            Me.txtMa_sp.Text = "TXTMA_SP"
            '
            'lblTen_vt
            '
            Me.lblTen_vt.AutoSize = True
            Me.lblTen_vt.Location = New System.Drawing.Point(256, 42)
            Me.lblTen_vt.Name = "lblTen_vt"
            Me.lblTen_vt.Size = New System.Drawing.Size(76, 16)
            Me.lblTen_vt.TabIndex = 115
            Me.lblTen_vt.Tag = "RF"
            Me.lblTen_vt.Text = "Ten san pham"
            '
            'txtSo_lsx
            '
            Me.txtSo_lsx.BackColor = System.Drawing.Color.White
            Me.txtSo_lsx.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtSo_lsx.Location = New System.Drawing.Point(155, 84)
            Me.txtSo_lsx.Name = "txtSo_lsx"
            Me.txtSo_lsx.TabIndex = 3
            Me.txtSo_lsx.Tag = "FC"
            Me.txtSo_lsx.Text = "TXTSO_LSX"
            Me.txtSo_lsx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'lblPer
            '
            Me.lblPer.AutoSize = True
            Me.lblPer.Location = New System.Drawing.Point(288, 130)
            Me.lblPer.Name = "lblPer"
            Me.lblPer.Size = New System.Drawing.Size(14, 16)
            Me.lblPer.TabIndex = 118
            Me.lblPer.Tag = ""
            Me.lblPer.Text = "%"
            '
            'txtLoai_yt
            '
            Me.txtLoai_yt.BackColor = System.Drawing.Color.White
            Me.txtLoai_yt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtLoai_yt.Location = New System.Drawing.Point(155, 18)
            Me.txtLoai_yt.Name = "txtLoai_yt"
            Me.txtLoai_yt.TabIndex = 0
            Me.txtLoai_yt.Tag = "FCNB"
            Me.txtLoai_yt.Text = "TXTLOAI_YT"
            '
            'lblTen_loai_yt
            '
            Me.lblTen_loai_yt.AutoSize = True
            Me.lblTen_loai_yt.Location = New System.Drawing.Point(256, 20)
            Me.lblTen_loai_yt.Name = "lblTen_loai_yt"
            Me.lblTen_loai_yt.Size = New System.Drawing.Size(78, 16)
            Me.lblTen_loai_yt.TabIndex = 120
            Me.lblTen_loai_yt.Tag = "RF"
            Me.lblTen_loai_yt.Text = "Ten loai yeu to"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(23, 42)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(73, 16)
            Me.Label2.TabIndex = 121
            Me.Label2.Tag = "L002"
            Me.Label2.Text = "Ma san pham"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(288, 154)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(14, 16)
            Me.Label3.TabIndex = 127
            Me.Label3.Tag = ""
            Me.Label3.Text = "%"
            '
            'txtTl_ht2
            '
            Me.txtTl_ht2.BackColor = System.Drawing.Color.White
            Me.txtTl_ht2.Format = "#0.00"
            Me.txtTl_ht2.Location = New System.Drawing.Point(224, 152)
            Me.txtTl_ht2.MaxLength = 6
            Me.txtTl_ht2.Name = "txtTl_ht2"
            Me.txtTl_ht2.Size = New System.Drawing.Size(50, 20)
            Me.txtTl_ht2.TabIndex = 6
            Me.txtTl_ht2.Tag = "FN"
            Me.txtTl_ht2.Text = "0.00"
            Me.txtTl_ht2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtTl_ht2.Value = 0
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(23, 154)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(191, 16)
            Me.Label4.TabIndex = 126
            Me.Label4.Tag = "L015"
            Me.Label4.Text = "Ty le hoan thanh dap vien/dong nang"
            '
            'txtTl_ht3
            '
            Me.txtTl_ht3.BackColor = System.Drawing.Color.White
            Me.txtTl_ht3.Format = "#0.00"
            Me.txtTl_ht3.Location = New System.Drawing.Point(224, 176)
            Me.txtTl_ht3.MaxLength = 6
            Me.txtTl_ht3.Name = "txtTl_ht3"
            Me.txtTl_ht3.Size = New System.Drawing.Size(50, 20)
            Me.txtTl_ht3.TabIndex = 7
            Me.txtTl_ht3.Tag = "FN"
            Me.txtTl_ht3.Text = "0.00"
            Me.txtTl_ht3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtTl_ht3.Value = 0
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(288, 178)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(14, 16)
            Me.Label5.TabIndex = 130
            Me.Label5.Tag = ""
            Me.Label5.Text = "%"
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(23, 178)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(138, 16)
            Me.Label6.TabIndex = 129
            Me.Label6.Tag = "L016"
            Me.Label6.Text = "Ty le hoan thanh bao phim"
            '
            'txtTl_ht4
            '
            Me.txtTl_ht4.BackColor = System.Drawing.Color.White
            Me.txtTl_ht4.Format = "#0.00"
            Me.txtTl_ht4.Location = New System.Drawing.Point(224, 200)
            Me.txtTl_ht4.MaxLength = 6
            Me.txtTl_ht4.Name = "txtTl_ht4"
            Me.txtTl_ht4.Size = New System.Drawing.Size(50, 20)
            Me.txtTl_ht4.TabIndex = 8
            Me.txtTl_ht4.Tag = "FN"
            Me.txtTl_ht4.Text = "0.00"
            Me.txtTl_ht4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtTl_ht4.Value = 0
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(288, 202)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(14, 16)
            Me.Label7.TabIndex = 133
            Me.Label7.Tag = ""
            Me.Label7.Text = "%"
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(23, 202)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(144, 16)
            Me.Label8.TabIndex = 132
            Me.Label8.Tag = "L017"
            Me.Label8.Text = "Ty le hoan thanh dong goi 1"
            '
            'txtTl_ht5
            '
            Me.txtTl_ht5.BackColor = System.Drawing.Color.White
            Me.txtTl_ht5.Format = "#0.00"
            Me.txtTl_ht5.Location = New System.Drawing.Point(224, 224)
            Me.txtTl_ht5.MaxLength = 6
            Me.txtTl_ht5.Name = "txtTl_ht5"
            Me.txtTl_ht5.Size = New System.Drawing.Size(50, 20)
            Me.txtTl_ht5.TabIndex = 9
            Me.txtTl_ht5.Tag = "FN"
            Me.txtTl_ht5.Text = "0.00"
            Me.txtTl_ht5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtTl_ht5.Value = 0
            '
            'Label9
            '
            Me.Label9.AutoSize = True
            Me.Label9.Location = New System.Drawing.Point(288, 226)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(14, 16)
            Me.Label9.TabIndex = 136
            Me.Label9.Tag = ""
            Me.Label9.Text = "%"
            '
            'Label10
            '
            Me.Label10.AutoSize = True
            Me.Label10.Location = New System.Drawing.Point(23, 226)
            Me.Label10.Name = "Label10"
            Me.Label10.Size = New System.Drawing.Size(141, 16)
            Me.Label10.TabIndex = 135
            Me.Label10.Tag = "L018"
            Me.Label10.Text = "Ty le hoan thanh dong goi2"
            '
            'txtTl_ht
            '
            Me.txtTl_ht.BackColor = System.Drawing.Color.White
            Me.txtTl_ht.Format = "#0.00"
            Me.txtTl_ht.Location = New System.Drawing.Point(312, 296)
            Me.txtTl_ht.MaxLength = 6
            Me.txtTl_ht.Name = "txtTl_ht"
            Me.txtTl_ht.Size = New System.Drawing.Size(50, 20)
            Me.txtTl_ht.TabIndex = 137
            Me.txtTl_ht.Tag = "FN"
            Me.txtTl_ht.Text = "0.00"
            Me.txtTl_ht.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtTl_ht.Value = 0
            Me.txtTl_ht.Visible = False
            '
            'frmDirInfor
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(656, 329)
            Me.Controls.Add(Me.txtTl_ht)
            Me.Controls.Add(Me.txtTl_ht5)
            Me.Controls.Add(Me.Label9)
            Me.Controls.Add(Me.Label10)
            Me.Controls.Add(Me.txtTl_ht4)
            Me.Controls.Add(Me.Label7)
            Me.Controls.Add(Me.Label8)
            Me.Controls.Add(Me.txtTl_ht3)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.Label6)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.txtLoai_yt)
            Me.Controls.Add(Me.lblTen_loai_yt)
            Me.Controls.Add(Me.lblPer)
            Me.Controls.Add(Me.txtSo_lsx)
            Me.Controls.Add(Me.txtMa_sp)
            Me.Controls.Add(Me.lblTen_vt)
            Me.Controls.Add(Me.lblMa_sp)
            Me.Controls.Add(Me.txtTl_ht1)
            Me.Controls.Add(Me.lblSo_lsx)
            Me.Controls.Add(Me.lblTl_ht)
            Me.Controls.Add(Me.txtSl_dd_ck)
            Me.Controls.Add(Me.lblSl_qd)
            Me.Controls.Add(Me.lblMa_bp)
            Me.Controls.Add(Me.txtMa_bp)
            Me.Controls.Add(Me.lblTen_bp)
            Me.Controls.Add(Me.txtSl_dd_ck0)
            Me.Controls.Add(Me.txtNam)
            Me.Controls.Add(Me.txtKy)
            Me.Controls.Add(Me.lblSl_dd)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.txtTl_ht2)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.grpInfor)
            Me.Name = "frmDirInfor"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmDirInfor"
            Me.ResumeLayout(False)

        End Sub

        Private Sub txtEnter(ByVal sender As Object, ByVal e As EventArgs)
            Me.noldValue = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        End Sub



        ' Properties
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents grpInfor As GroupBox
        Friend WithEvents Label2 As Label
        Friend WithEvents lblMa_bp As Label
        Friend WithEvents lblMa_sp As Label
        Friend WithEvents lblPer As Label
        Friend WithEvents lblSl_dd As Label
        Friend WithEvents lblSl_qd As Label
        Friend WithEvents lblSo_lsx As Label
        Friend WithEvents lblTen_bp As Label
        Friend WithEvents lblTen_loai_yt As Label
        Friend WithEvents lblTen_vt As Label
        Friend WithEvents lblTl_ht As Label
        Friend WithEvents txtKy As txtNumeric
        Friend WithEvents txtLoai_yt As TextBox
        Friend WithEvents txtMa_bp As TextBox
        Friend WithEvents txtMa_sp As TextBox
        Friend WithEvents txtNam As txtNumeric
        Friend WithEvents txtSl_dd_ck As txtNumeric
        Friend WithEvents txtSl_dd_ck0 As txtNumeric
        Friend WithEvents txtSo_lsx As TextBox

        Private components As IContainer
        Private lblRef As Label
        Private noldValue As Decimal


        Private Sub txtTl_ht1_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTl_ht1.Validated
            rate_cal()
        End Sub
        Private Sub txtTl_ht2_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTl_ht2.Validated
            rate_cal()
        End Sub
        Private Sub txtTl_ht3_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTl_ht3.Validated
            rate_cal()
        End Sub
        Private Sub txtTl_ht4_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTl_ht4.Validated
            rate_cal()
        End Sub
        Private Sub txtTl_ht5_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTl_ht5.Validated
            rate_cal()
        End Sub
    End Class
End Namespace

