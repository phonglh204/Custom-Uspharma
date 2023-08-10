Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol

Namespace inlosd0
    Public Class frmDirInfor
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
            AddHandler MyBase.Closed, New EventHandler(AddressOf Me.frmDirInfor_Closed)
            Me.InitializeComponent
        End Sub

        Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
            Me.txtMa_dvcs.Tag = "FC"
            Dim strKeyField As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj((Strings.Trim(StringType.FromInteger(DirMain.nBgYear)) & ", "), Sql.ConvertVS2SQLType(Me.txtMa_kho.Text, "")), ","), Sql.ConvertVS2SQLType(Me.txtMa_vi_tri.Text, "")), ","), Sql.ConvertVS2SQLType(Me.txtMa_vt.Text, "")), ","), Sql.ConvertVS2SQLType(Me.txtMa_lo.Text, "")))
            DirMain.oDirFormLib.SaveFormDir(Me, strKeyField)
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
            Dim sRight As String = Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("DFUnit")))
            If (StringType.StrCmp(DirMain.oDirFormLib.cAction, "New", False) = 0) Then
                Me.txtNam.Text = StringType.FromInteger(DirMain.nBgYear)
                Unit.SetUnit(Me.txtMa_dvcs)
            Else
                Me.txtMa_dvcs.Text = DirMain.oDirFormLib.oDir.ob.dv.Item(DirMain.oDirFormLib.iCurrRow).Item("ma_dvcs").ToString.Trim
                If (ObjectType.ObjTst(Sql.GetValue((DirMain.oDirFormLib.appConn), "dmvt", "gia_ton", ("ma_vt = '" & Strings.Trim(Me.txtMa_vt.Text) & "'")), 3, False) = 0) Then
                    Me.cmdOk.Enabled = False
                End If
                If (StringType.StrCmp(Me.txtMa_dvcs.Text.Trim, sRight, False) <> 0) Then
                    Me.txtMa_kho.Enabled = False
                    Me.txtMa_vi_tri.Enabled = False
                End If
            End If
            Dim obj3 As Object = New DirLib(Me.txtMa_kho, Me.lblTen_kho, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmkho", "ma_kho", "ten_kho", "Store", ("RTRIM(ma_dvcs) = '" & sRight & "'"), False, Me.cmdCancel)
            Dim obj2 As Object = New DirLib(Me.txtMa_vt, Me.lblTen_vt, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "gia_ton <> 3", False, Me.cmdCancel)
            Dim obj4 As Object = New DirLib(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmdvcs", "Ma_dvcs", "Ten_dvcs", "Unit", "1=1", False, Me.cmdCancel)
            AddHandler Me.txtMa_vt.Validated, New EventHandler(AddressOf Me.txtMa_vt_Validated)
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_vt.Text), "", False) <> 0) Then
                Me.lblDvt.Text = StringType.FromObject(Sql.GetValue((DirMain.oDirFormLib.appConn), "dmvt", "dvt", ("ma_vt = '" & Strings.Trim(Me.txtMa_vt.Text) & "'")))
            Else
                Me.lblDvt.Text = ""
            End If
            Me.oLocation = New dirblanklib(Me.txtMa_vi_tri, Me.lblTen_vi_tri, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmvitri", "ma_vi_tri", "ten_vi_tri", "Location", "1=1", False, Me.cmdCancel)
            Me.oLot = New dirblanklib(Me.txtMa_lo, Me.lblTen_lo, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmlo", "ma_lo", "ten_lo", "Lot", "1=1", False, Me.cmdCancel)
            AddHandler Me.txtMa_vi_tri.Enter, New EventHandler(AddressOf Me.txtMa_kho_LostFocus)
            AddHandler Me.txtMa_lo.Enter, New EventHandler(AddressOf Me.txtMa_vt_LostFocus)
        End Sub

        <DebuggerStepThrough()> _
Private Sub InitializeComponent()
            Me.grpInfor = New GroupBox
            Me.txtNam = New TextBox
            Me.Label5 = New Label
            Me.Label1 = New Label
            Me.txtdien_giai = New TextBox
            Me.Label3 = New Label
            Me.Label2 = New Label
            Me.lblTen_vt = New Label
            Me.txtMa_vt = New TextBox
            Me.lblTy_gia = New Label
            Me.lblvat_tu = New Label
            Me.cmdCancel = New Button
            Me.cmdOk = New Button
            Me.txtMa_kho = New TextBox
            Me.lblMa_kho = New Label
            Me.lblTen_kho = New Label
            Me.txtDu_nt00 = New txtNumeric
            Me.txtDu00 = New txtNumeric
            Me.txtTon00 = New txtNumeric
            Me.lblDvt = New Label
            Me.lblMa_vi_tri = New Label
            Me.txtMa_vi_tri = New TextBox
            Me.lblTen_vi_tri = New Label
            Me.lblTen_lo = New Label
            Me.txtMa_lo = New TextBox
            Me.lblMa_lo = New Label
            Me.lblTen_dvcs = New Label
            Me.lblMa_dvcs = New Label
            Me.txtMa_dvcs = New TextBox
            Me.SuspendLayout()
            Me.grpInfor.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.grpInfor.Location = New Point(8, 7)
            Me.grpInfor.Name = "grpInfor"
            Me.grpInfor.Size = New Size(592, 227)
            Me.grpInfor.TabIndex = 7
            Me.grpInfor.TabStop = False
            Me.txtNam.Enabled = False
            Me.txtNam.Location = New Point(240, 307)
            Me.txtNam.Name = "txtNam"
            Me.txtNam.TabIndex = 80
            Me.txtNam.Tag = "FN"
            Me.txtNam.Text = "txtNam"
            Me.txtNam.Visible = False
            Me.Label5.AutoSize = True
            Me.Label5.Location = New Point(408, 312)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New Size(58, 16)
            Me.Label5.TabIndex = 79
            Me.Label5.Tag = "L003"
            Me.Label5.Text = "Don vi tinh"
            Me.Label5.Visible = False
            Me.Label1.AutoSize = True
            Me.Label1.Location = New Point(23, 207)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New Size(48, 16)
            Me.Label1.TabIndex = 78
            Me.Label1.Tag = "L007"
            Me.Label1.Text = "Dien giai"
            Me.txtdien_giai.AutoSize = False
            Me.txtdien_giai.Location = New Point(155, 205)
            Me.txtdien_giai.Name = "txtdien_giai"
            Me.txtdien_giai.Size = New Size(364, 20)
            Me.txtdien_giai.TabIndex = 9
            Me.txtdien_giai.Tag = "FC"
            Me.txtdien_giai.Text = "txtdien_giai"
            Me.Label3.AutoSize = True
            Me.Label3.Location = New Point(23, 184)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New Size(84, 16)
            Me.Label3.TabIndex = 77
            Me.Label3.Tag = "L006"
            Me.Label3.Text = "Du dau ngoai te"
            Me.Label2.AutoSize = True
            Me.Label2.Location = New Point(23, 161)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New Size(41, 16)
            Me.Label2.TabIndex = 76
            Me.Label2.Tag = "L005"
            Me.Label2.Text = "Du dau"
            Me.lblTen_vt.AutoSize = True
            Me.lblTen_vt.Location = New Point(256, 46)
            Me.lblTen_vt.Name = "lblTen_vt"
            Me.lblTen_vt.Size = New Size(54, 16)
            Me.lblTen_vt.TabIndex = 75
            Me.lblTen_vt.Tag = "RF"
            Me.lblTen_vt.Text = "Ten vat tu"
            Me.txtMa_vt.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_vt.Location = New Point(155, 44)
            Me.txtMa_vt.Name = "txtMa_vt"
            Me.txtMa_vt.TabIndex = 1
            Me.txtMa_vt.Tag = "FCNB"
            Me.txtMa_vt.Text = "TXTMA_VT"
            Me.lblTy_gia.AutoSize = True
            Me.lblTy_gia.Location = New Point(23, 138)
            Me.lblTy_gia.Name = "lblTy_gia"
            Me.lblTy_gia.Size = New Size(46, 16)
            Me.lblTy_gia.TabIndex = 70
            Me.lblTy_gia.Tag = "L004"
            Me.lblTy_gia.Text = "Ton dau"
            Me.lblvat_tu.AutoSize = True
            Me.lblvat_tu.Location = New Point(23, 46)
            Me.lblvat_tu.Name = "lblvat_tu"
            Me.lblvat_tu.Size = New Size(34, 16)
            Me.lblvat_tu.TabIndex = 68
            Me.lblvat_tu.Tag = "L002"
            Me.lblvat_tu.Text = "Vat tu"
            Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New Point(84, 243)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 11
            Me.cmdCancel.Tag = "L009"
            Me.cmdCancel.Text = "Huy"
            Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdOk.Location = New Point(8, 243)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 10
            Me.cmdOk.Tag = "L008"
            Me.cmdOk.Text = "Nhan"
            Me.txtMa_kho.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_kho.Location = New Point(155, 67)
            Me.txtMa_kho.Name = "txtMa_kho"
            Me.txtMa_kho.TabIndex = 2
            Me.txtMa_kho.Tag = "FCNB"
            Me.txtMa_kho.Text = "TXTMA_KHO"
            Me.lblMa_kho.AutoSize = True
            Me.lblMa_kho.Location = New Point(23, 69)
            Me.lblMa_kho.Name = "lblMa_kho"
            Me.lblMa_kho.Size = New Size(41, 16)
            Me.lblMa_kho.TabIndex = 73
            Me.lblMa_kho.Tag = "L001"
            Me.lblMa_kho.Text = "Ma kho"
            Me.lblTen_kho.AutoSize = True
            Me.lblTen_kho.Location = New Point(256, 69)
            Me.lblTen_kho.Name = "lblTen_kho"
            Me.lblTen_kho.Size = New Size(45, 16)
            Me.lblTen_kho.TabIndex = 74
            Me.lblTen_kho.Tag = "RF"
            Me.lblTen_kho.Text = "Ten kho"
            Me.txtDu_nt00.Format = "m_ip_tien_nt"
            Me.txtDu_nt00.Location = New Point(155, 182)
            Me.txtDu_nt00.MaxLength = 13
            Me.txtDu_nt00.Name = "txtDu_nt00"
            Me.txtDu_nt00.TabIndex = 8
            Me.txtDu_nt00.Tag = "FN"
            Me.txtDu_nt00.Text = "m_ip_tien_nt"
            Me.txtDu_nt00.TextAlign = HorizontalAlignment.Right
            Me.txtDu_nt00.Value = 0
            Me.txtDu00.Format = "m_ip_tien"
            Me.txtDu00.Location = New Point(155, 159)
            Me.txtDu00.MaxLength = 10
            Me.txtDu00.Name = "txtDu00"
            Me.txtDu00.TabIndex = 6
            Me.txtDu00.Tag = "FN"
            Me.txtDu00.Text = "m_ip_tien"
            Me.txtDu00.TextAlign = HorizontalAlignment.Right
            Me.txtDu00.Value = 0
            Me.txtTon00.Format = "m_ip_sl"
            Me.txtTon00.Location = New Point(155, 136)
            Me.txtTon00.MaxLength = 8
            Me.txtTon00.Name = "txtTon00"
            Me.txtTon00.TabIndex = 5
            Me.txtTon00.Tag = "FN"
            Me.txtTon00.Text = "m_ip_sl"
            Me.txtTon00.TextAlign = HorizontalAlignment.Right
            Me.txtTon00.Value = 0
            Me.lblDvt.AutoSize = True
            Me.lblDvt.Location = New Point(259, 138)
            Me.lblDvt.Name = "lblDvt"
            Me.lblDvt.Size = New Size(58, 16)
            Me.lblDvt.TabIndex = 2
            Me.lblDvt.Tag = "RF"
            Me.lblDvt.Text = "Don vi tinh"
            Me.lblMa_vi_tri.AutoSize = True
            Me.lblMa_vi_tri.Location = New Point(23, 92)
            Me.lblMa_vi_tri.Name = "lblMa_vi_tri"
            Me.lblMa_vi_tri.Size = New Size(44, 16)
            Me.lblMa_vi_tri.TabIndex = 93
            Me.lblMa_vi_tri.Tag = "L011"
            Me.lblMa_vi_tri.Text = "Ma vi tri"
            Me.txtMa_vi_tri.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_vi_tri.Location = New Point(155, 90)
            Me.txtMa_vi_tri.Name = "txtMa_vi_tri"
            Me.txtMa_vi_tri.TabIndex = 3
            Me.txtMa_vi_tri.Tag = "FC"
            Me.txtMa_vi_tri.Text = "TXTMA_VI_TRI"
            Me.lblTen_vi_tri.AutoSize = True
            Me.lblTen_vi_tri.Location = New Point(256, 92)
            Me.lblTen_vi_tri.Name = "lblTen_vi_tri"
            Me.lblTen_vi_tri.Size = New Size(47, 16)
            Me.lblTen_vi_tri.TabIndex = 92
            Me.lblTen_vi_tri.Tag = "RF"
            Me.lblTen_vi_tri.Text = "Ten vi tri"
            Me.lblTen_lo.AutoSize = True
            Me.lblTen_lo.Location = New Point(259, 115)
            Me.lblTen_lo.Name = "lblTen_lo"
            Me.lblTen_lo.Size = New Size(36, 16)
            Me.lblTen_lo.TabIndex = 96
            Me.lblTen_lo.Tag = "RF"
            Me.lblTen_lo.Text = "Ten lo"
            Me.txtMa_lo.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_lo.Location = New Point(155, 113)
            Me.txtMa_lo.Name = "txtMa_lo"
            Me.txtMa_lo.TabIndex = 4
            Me.txtMa_lo.Tag = "FC"
            Me.txtMa_lo.Text = "TXTMA_LO"
            Me.lblMa_lo.AutoSize = True
            Me.lblMa_lo.Location = New Point(23, 115)
            Me.lblMa_lo.Name = "lblMa_lo"
            Me.lblMa_lo.Size = New Size(30, 16)
            Me.lblMa_lo.TabIndex = 95
            Me.lblMa_lo.Tag = "L012"
            Me.lblMa_lo.Text = "So lo"
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New Point(259, 23)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New Size(65, 16)
            Me.lblTen_dvcs.TabIndex = 15
            Me.lblTen_dvcs.Tag = ""
            Me.lblTen_dvcs.Text = "lblTen_dvcs"
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New Point(23, 23)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New Size(36, 16)
            Me.lblMa_dvcs.TabIndex = 14
            Me.lblMa_dvcs.Tag = "L014"
            Me.lblMa_dvcs.Text = "Don vi"
            Me.txtMa_dvcs.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_dvcs.Enabled = False
            Me.txtMa_dvcs.Location = New Point(155, 21)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.TabIndex = 0
            Me.txtMa_dvcs.Tag = ""
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            Me.AutoScaleBaseSize = New Size(5, 13)
            Me.ClientSize = New Size(608, 271)
            Me.Controls.Add(Me.txtMa_dvcs)
            Me.Controls.Add(Me.lblMa_dvcs)
            Me.Controls.Add(Me.lblTen_dvcs)
            Me.Controls.Add(Me.lblTen_lo)
            Me.Controls.Add(Me.txtMa_lo)
            Me.Controls.Add(Me.lblMa_lo)
            Me.Controls.Add(Me.lblMa_vi_tri)
            Me.Controls.Add(Me.txtMa_vi_tri)
            Me.Controls.Add(Me.lblTen_vi_tri)
            Me.Controls.Add(Me.lblDvt)
            Me.Controls.Add(Me.txtDu_nt00)
            Me.Controls.Add(Me.txtDu00)
            Me.Controls.Add(Me.txtTon00)
            Me.Controls.Add(Me.txtNam)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.txtdien_giai)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.lblTen_vt)
            Me.Controls.Add(Me.txtMa_vt)
            Me.Controls.Add(Me.lblTy_gia)
            Me.Controls.Add(Me.lblvat_tu)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.txtMa_kho)
            Me.Controls.Add(Me.lblMa_kho)
            Me.Controls.Add(Me.lblTen_kho)
            Me.Controls.Add(Me.grpInfor)
            Me.Name = "frmDirInfor"
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "frmDirInfor"
            Me.ResumeLayout(False)
        End Sub

        Private Sub lblMa_vi_tri_Click(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Private Sub lblTen_kho_Click(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Private Sub lblTen_vi_tri_Click(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Private Sub txtMa_kho_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
            Dim cKey As String = ("ma_kho = '" & Strings.Trim(Me.txtMa_kho.Text) & "'")
            Me.oLocation.Key = cKey
            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((DirMain.oDirFormLib.appConn), "dmvitri", "ma_vi_tri", cKey))), "", False) <> 0) Then
                Me.oLocation.Blank = False
                Me.txtMa_vi_tri.Tag = "FCNB"
            Else
                Me.txtMa_vi_tri.Text = ""
                Me.lblTen_vi_tri.Text = ""
                Me.oLocation.Blank = False
                Me.txtMa_vi_tri.Tag = "FC"
            End If
        End Sub

        Private Sub txtMa_kho_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Private Sub txtMa_vi_tri_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Private Sub txtMa_vt_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
            Dim cKey As String = ("ma_vt = '" & Strings.Trim(Me.txtMa_vt.Text) & "'")
            Me.oLot.Key = cKey
            If BooleanType.FromObject(Sql.GetValue((DirMain.oDirFormLib.appConn), "dmvt", "lo_yn", cKey)) Then
                Me.oLot.Blank = False
                Me.txtMa_lo.Tag = "FCNB"
            Else
                Me.txtMa_lo.Text = ""
                Me.lblTen_lo.Text = ""
                Me.oLot.Blank = True
                Me.txtMa_lo.Tag = "FC"
            End If
        End Sub

        Private Sub txtMa_vt_Validated(ByVal sender As Object, ByVal e As EventArgs)
            Me.lblDvt.Text = StringType.FromObject(Sql.GetValue((DirMain.oDirFormLib.appConn), "dmvt", "dvt", ("ma_vt = '" & Strings.Trim(Me.txtMa_vt.Text) & "'")))
        End Sub


        ' Properties
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents grpInfor As GroupBox
        Friend WithEvents Label1 As Label
        Friend WithEvents Label2 As Label
        Friend WithEvents Label3 As Label
        Friend WithEvents Label5 As Label
        Friend WithEvents lblDvt As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMa_kho As Label
        Friend WithEvents lblMa_lo As Label
        Friend WithEvents lblMa_vi_tri As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTen_kho As Label
        Friend WithEvents lblTen_lo As Label
        Friend WithEvents lblTen_vi_tri As Label
        Friend WithEvents lblTen_vt As Label
        Friend WithEvents lblTy_gia As Label
        Friend WithEvents lblvat_tu As Label
        Friend WithEvents txtdien_giai As TextBox
        Friend WithEvents txtDu_nt00 As txtNumeric
        Friend WithEvents txtDu00 As txtNumeric
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtMa_kho As TextBox
        Friend WithEvents txtMa_lo As TextBox
        Friend WithEvents txtMa_vi_tri As TextBox
        Friend WithEvents txtMa_vt As TextBox
        Friend WithEvents txtNam As TextBox
        Friend WithEvents txtTon00 As txtNumeric

        Private components As IContainer
        Private oLocation As dirblanklib
        Private oLot As dirblanklib
    End Class
End Namespace

