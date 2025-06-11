Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon


Public Class frmDirInfor
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
        AddHandler MyBase.Closed, New EventHandler(AddressOf Me.frmDirInfor_Closed)
        Me.lblE_mail = New Label
        Me.lblHome_page = New Label
        Me.InitializeComponent()
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        If (((StringType.StrCmp(Me.txtMa_so_thue.Text.Trim, "", False) <> 0) AndAlso (ObjectType.ObjTst(oDirFormLib.oOptions.Item("m_kt_mst"), 0, False) > 0)) AndAlso Not clsCheck.isValidTaxID(Me.txtMa_so_thue.Text)) Then
            If (ObjectType.ObjTst(oDirFormLib.oOptions.Item("m_kt_mst"), 1, False) <> 0) Then
                Msg.Alert(StringType.FromObject(oDirFormLib.oLan.Item("032")), 1)
                Me.txtMa_so_thue.Focus()
                Return
            End If
            Msg.Alert(StringType.FromObject(oDirFormLib.oLan.Item("032")), 2)
        End If
        If Me.txtMa_so_thue.Text <> "" Then
            Dim s As String
            If Not (Sql.GetValue(oDirFormLib.appConn, "dmkh", "ma_kh", "ma_so_thue='" + Me.txtMa_so_thue.Text.Trim.Replace("'", "''") + "' and ma_kh <>'" + old_code.Trim.Replace("'", "''") + "'") Is Nothing) Then
                s = Convert.ToString(Sql.GetValue(oDirFormLib.appConn, "dmkh", "ma_kh", "ma_so_thue='" + Me.txtMa_so_thue.Text.Trim.Replace("'", "''") + "' and ma_kh <>'" + old_code.Trim.Replace("'", "''") + "'"))
                Msg.Alert(Replace(oDirFormLib.oLan.Item("034"), "%s", s.Trim))
                'Return
            End If
        End If
        If Not ((Me.chkCc_yn.Checked Or Me.chkKh_yn.Checked) Or Me.chkNv_yn.Checked) Then
            Msg.Alert(StringType.FromObject(oDirFormLib.oLan.Item("033")), 1)
            Me.chkKh_yn.Focus()
        Else
            oDirFormLib.SaveFormDir(Me, Me.txtMa_kh.Text)
        End If
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
        If (StringType.StrCmp(oDirFormLib.cAction, "New", False) = 0) Then
            Me.txtStatus.Text = "1"
        End If

        Dim oStatus As Object = New CharLib(Me.txtStatus, "0, 1")
        Dim oCust As New DirLib(Me.txtTk, Me.lblTen_tk, oDirFormLib.sysConn, oDirFormLib.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", True, Me.cmdCancel)
        Dim oNh_kh9 As New DirLib(Me.txtNh_kh9, Me.lblTen_nh_kh9, oDirFormLib.sysConn, oDirFormLib.appConn, "dmnhkh2", "ma_nh", "ten_nh", "CustPriceClass", "1=1", True, Me.cmdCancel)
        Dim oNh_kh1 As New DirLib(Me.txtNh_kh1, Me.lblTen_nh_kh1, oDirFormLib.sysConn, oDirFormLib.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh = 1", True, Me.cmdCancel)
        Dim oNh_kh2 As New DirLib(Me.txtNh_kh2, Me.lblTen_nh_kh2, oDirFormLib.sysConn, oDirFormLib.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh = 2", True, Me.cmdCancel)
        Dim oNh_kh3 As New DirLib(Me.txtNh_kh3, Me.lblTen_nh_kh3, oDirFormLib.sysConn, oDirFormLib.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh = 3", True, Me.cmdCancel)
        Dim oNh_kh4 As New DirLib(Me.txtNh_kh4, Me.lblTen_nh_kh4, oDirFormLib.sysConn, oDirFormLib.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh = 4", True, Me.cmdCancel)
        Dim oStaff As New DirLib(Me.txtMa_nvbh, Me.lblTen_nvbh, oDirFormLib.sysConn, oDirFormLib.appConn, "dmnvbh", "ma_nvbh", "ten_nvbh", "SaleEmployee", "1=1", True, Me.cmdCancel)
        Dim oPayment As New DirLib(Me.txtMa_tt, Me.lbTen_tt, oDirFormLib.sysConn, oDirFormLib.appConn, "dmtt", "ma_tt", "ten_tt", "Term", "1=1", True, Me.cmdCancel)
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) <> 0) Then
            Me.txtNgay_gh.AddCalenderControl()
        End If
        Me.lblT_tien_cn.Text = Strings.Replace(Me.lblT_tien_cn.Text, "%s", StringType.FromObject(oDirFormLib.oOptions.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary)
        Me.txtDoi_tac.MaxLength = IntegerType.FromObject(oDirFormLib.oLen.Item("doi_tac"))
        Me.lblE_mail.AutoSize = True
        Me.lblE_mail.Text = Me.txtE_mail.Text.Trim
        Me.lblE_mail.ForeColor = Color.Green
        Me.lblE_mail.Cursor = Cursors.Hand
        Me.lblE_mail.Font = Me.lblVisible.Font
        Me.lblE_mail.TextAlign = ContentAlignment.MiddleLeft
        Me.txtE_mail.Controls.Add(Me.lblE_mail)
        AddHandler Me.txtE_mail.Enter, New EventHandler(AddressOf Me.txt_Enter)
        AddHandler Me.txtE_mail.Validated, New EventHandler(AddressOf Me.txt_Valid)
        AddHandler Me.txtE_mail.TextChanged, New EventHandler(AddressOf Me.txt_TextChange)
        AddHandler Me.lblE_mail.Click, New EventHandler(AddressOf Me.lblE_mail_Click)
        Me.lblHome_page.AutoSize = True
        Me.lblHome_page.Text = Me.txtHome_page.Text.Trim
        Me.lblHome_page.ForeColor = Color.Blue
        Me.lblHome_page.Cursor = Cursors.Hand
        Me.lblHome_page.Font = Me.lblVisible.Font
        Me.lblHome_page.TextAlign = ContentAlignment.MiddleLeft
        Me.txtHome_page.Controls.Add(Me.lblHome_page)
        AddHandler Me.txtHome_page.Enter, New EventHandler(AddressOf Me.txt_Enter)
        AddHandler Me.txtHome_page.Validated, New EventHandler(AddressOf Me.txt_Valid)
        AddHandler Me.txtHome_page.TextChanged, New EventHandler(AddressOf Me.txt_TextChange)
        AddHandler Me.lblHome_page.Click, New EventHandler(AddressOf Me.lblHome_page_Click)
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) = 0) Then
            Dim control3 As Control
            Me.lblT_tien_cn.Visible = False
            Me.txtT_tien_cn.Visible = False
            Me.lblNgay_gh.Visible = False
            Me.txtNgay_gh.Visible = False
            Dim control As Control
            For Each control In Me.txtT_tien_cn.Parent.Controls
                If (((Strings.InStr(control.Anchor.ToString, "Top", CompareMethod.Binary) > 0) And (Strings.InStr(control.Anchor.ToString, "Bottom", CompareMethod.Binary) = 0)) AndAlso (control.Top > Me.txtT_tien_cn.Top)) Then
                    control3 = control
                    control3.Top = (control3.Top - (Me.txtT_tien_cn.Height + 3))
                End If
            Next
            Me.lblNh_kh9.Visible = False
            Me.txtNh_kh9.Visible = False
            Me.lblTen_nh_kh9.Visible = False
            Dim control2 As Control
            For Each control2 In Me.txtNh_kh9.Parent.Controls
                If (((Strings.InStr(control2.Anchor.ToString, "Top", CompareMethod.Binary) > 0) And (Strings.InStr(control2.Anchor.ToString, "Bottom", CompareMethod.Binary) = 0)) AndAlso (control2.Top > Me.txtNh_kh9.Top)) Then
                    control3 = control2
                    control3.Top = (control3.Top - (Me.txtNh_kh9.Height + 3))
                End If
            Next
            Dim num As Integer = (((Me.txtT_tien_cn.Height + 3) + Me.txtNh_kh9.Height) + 3)
            Me.Height = (Me.Height - num)
            Me.Top = (Me.Top + CInt(Math.Round(CDbl((CDbl(num) / 2)))))
        End If
        Me.cboS1.SetItemsF5(oDirFormLib.appConn, "hrlstpcs", "1=1", "name", "STRING", "name", True, "name")
        Me.cboTinh_thanh.SetItemsF5(oDirFormLib.appConn, "hrlstpcs", "1=1", "name", "STRING", "name", True, "name")
        If (StringType.StrCmp(oDirFormLib.cAction, "Edit", False) = 0) Then
            old_code = Me.txtMa_kh.Text
            Me.cboS1.Value = oDirFormLib.oDir.ob.CurDataRow.Item("s1")
            Me.cboTinh_thanh.Value = oDirFormLib.oDir.ob.CurDataRow.Item("tinh_thanh")
        End If
    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cboS1 As CusCombobox
    Friend WithEvents cboTinh_thanh As CusCombobox
    Friend WithEvents lblTen_nh_kh4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtNh_kh4 As TextBox
    Friend WithEvents chkCa_nhan_yn As CheckBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtNo_id As TextBox
    Friend WithEvents txtOng_ba As System.Windows.Forms.TextBox

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtMa_kh = New System.Windows.Forms.TextBox()
        Me.txtTen_kh = New System.Windows.Forms.TextBox()
        Me.txtTen_kh2 = New System.Windows.Forms.TextBox()
        Me.txtTk = New System.Windows.Forms.TextBox()
        Me.lblTen_kh = New System.Windows.Forms.Label()
        Me.lblTen_kh2 = New System.Windows.Forms.Label()
        Me.lblTk = New System.Windows.Forms.Label()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.lblTen_tk = New System.Windows.Forms.Label()
        Me.grpInfor = New System.Windows.Forms.GroupBox()
        Me.txtDoi_tac = New System.Windows.Forms.TextBox()
        Me.lblMa_so_thue = New System.Windows.Forms.Label()
        Me.lblStatusMess = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.txtStatus = New System.Windows.Forms.TextBox()
        Me.lblMa_kh = New System.Windows.Forms.Label()
        Me.lblDia_chi = New System.Windows.Forms.Label()
        Me.txtDia_chi = New System.Windows.Forms.TextBox()
        Me.lblDoi_tac = New System.Windows.Forms.Label()
        Me.txtMa_so_thue = New System.Windows.Forms.TextBox()
        Me.lblHan_tt = New System.Windows.Forms.Label()
        Me.txtMa_tt = New System.Windows.Forms.TextBox()
        Me.lblNh_kh1 = New System.Windows.Forms.Label()
        Me.txtNh_kh1 = New System.Windows.Forms.TextBox()
        Me.txtNh_kh2 = New System.Windows.Forms.TextBox()
        Me.txtNh_kh3 = New System.Windows.Forms.TextBox()
        Me.lblNh_kh2 = New System.Windows.Forms.Label()
        Me.lblNh_kh3 = New System.Windows.Forms.Label()
        Me.lblDien_thoai = New System.Windows.Forms.Label()
        Me.txtDien_thoai = New System.Windows.Forms.TextBox()
        Me.lblFax = New System.Windows.Forms.Label()
        Me.txtFax = New System.Windows.Forms.TextBox()
        Me.lblEmail = New System.Windows.Forms.Label()
        Me.txtE_mail = New System.Windows.Forms.TextBox()
        Me.lblTk_nh = New System.Windows.Forms.Label()
        Me.txtTk_nh = New System.Windows.Forms.TextBox()
        Me.lblTen_nh = New System.Windows.Forms.Label()
        Me.txtNgan_hang = New System.Windows.Forms.TextBox()
        Me.lblTinh_thanh = New System.Windows.Forms.Label()
        Me.chkKh_yn = New System.Windows.Forms.CheckBox()
        Me.chkCc_yn = New System.Windows.Forms.CheckBox()
        Me.chkNv_yn = New System.Windows.Forms.CheckBox()
        Me.lblTen_nh_kh1 = New System.Windows.Forms.Label()
        Me.lblTen_nh_kh2 = New System.Windows.Forms.Label()
        Me.lblTen_nh_kh3 = New System.Windows.Forms.Label()
        Me.lblGhi_chu = New System.Windows.Forms.Label()
        Me.txtGhi_chu = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMa_nvbh = New System.Windows.Forms.TextBox()
        Me.lblTen_nvbh = New System.Windows.Forms.Label()
        Me.lbTen_tt = New System.Windows.Forms.Label()
        Me.lblTen_nh_kh9 = New System.Windows.Forms.Label()
        Me.lblNh_kh9 = New System.Windows.Forms.Label()
        Me.txtNh_kh9 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtHome_page = New System.Windows.Forms.TextBox()
        Me.lblT_tien_cn = New System.Windows.Forms.Label()
        Me.txtT_tien_cn = New libscontrol.txtNumeric()
        Me.lblNgay_gh = New System.Windows.Forms.Label()
        Me.txtNgay_gh = New libscontrol.txtDate()
        Me.lblVisible = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtOng_ba = New System.Windows.Forms.TextBox()
        Me.cboS1 = New libscontrol.CusCombobox()
        Me.cboTinh_thanh = New libscontrol.CusCombobox()
        Me.lblTen_nh_kh4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtNh_kh4 = New System.Windows.Forms.TextBox()
        Me.chkCa_nhan_yn = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNo_id = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtMa_kh
        '
        Me.txtMa_kh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kh.Location = New System.Drawing.Point(186, 18)
        Me.txtMa_kh.Name = "txtMa_kh"
        Me.txtMa_kh.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_kh.TabIndex = 0
        Me.txtMa_kh.Tag = "FCNBDF"
        Me.txtMa_kh.Text = "TXTMA_KH"
        '
        'txtTen_kh
        '
        Me.txtTen_kh.Location = New System.Drawing.Point(186, 45)
        Me.txtTen_kh.Name = "txtTen_kh"
        Me.txtTen_kh.Size = New System.Drawing.Size(437, 22)
        Me.txtTen_kh.TabIndex = 2
        Me.txtTen_kh.Tag = "FCNB"
        Me.txtTen_kh.Text = "txtTen_kh"
        '
        'txtTen_kh2
        '
        Me.txtTen_kh2.Location = New System.Drawing.Point(186, 72)
        Me.txtTen_kh2.Name = "txtTen_kh2"
        Me.txtTen_kh2.Size = New System.Drawing.Size(437, 22)
        Me.txtTen_kh2.TabIndex = 3
        Me.txtTen_kh2.Tag = "FC"
        Me.txtTen_kh2.Text = "txtTen_kh2"
        '
        'txtTk
        '
        Me.txtTk.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk.Location = New System.Drawing.Point(186, 178)
        Me.txtTk.Name = "txtTk"
        Me.txtTk.Size = New System.Drawing.Size(120, 22)
        Me.txtTk.TabIndex = 9
        Me.txtTk.Tag = "FC"
        Me.txtTk.Text = "TXTTKDF"
        '
        'lblTen_kh
        '
        Me.lblTen_kh.AutoSize = True
        Me.lblTen_kh.Location = New System.Drawing.Point(28, 47)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(75, 17)
        Me.lblTen_kh.TabIndex = 7
        Me.lblTen_kh.Tag = "L002"
        Me.lblTen_kh.Text = "Ten khach"
        '
        'lblTen_kh2
        '
        Me.lblTen_kh2.AutoSize = True
        Me.lblTen_kh2.Location = New System.Drawing.Point(28, 74)
        Me.lblTen_kh2.Name = "lblTen_kh2"
        Me.lblTen_kh2.Size = New System.Drawing.Size(87, 17)
        Me.lblTen_kh2.TabIndex = 8
        Me.lblTen_kh2.Tag = "L003"
        Me.lblTen_kh2.Text = "Ten khach 2"
        '
        'lblTk
        '
        Me.lblTk.AutoSize = True
        Me.lblTk.Location = New System.Drawing.Point(28, 180)
        Me.lblTk.Name = "lblTk"
        Me.lblTk.Size = New System.Drawing.Size(94, 17)
        Me.lblTk.TabIndex = 9
        Me.lblTk.Tag = "L006"
        Me.lblTk.Text = "Tk ngam dinh"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(10, 620)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(90, 26)
        Me.cmdOk.TabIndex = 32
        Me.cmdOk.Tag = "L024"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(101, 620)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(90, 26)
        Me.cmdCancel.TabIndex = 33
        Me.cmdCancel.Tag = "L025"
        Me.cmdCancel.Text = "Huy"
        '
        'lblTen_tk
        '
        Me.lblTen_tk.AutoSize = True
        Me.lblTen_tk.Location = New System.Drawing.Point(336, 629)
        Me.lblTen_tk.Name = "lblTen_tk"
        Me.lblTen_tk.Size = New System.Drawing.Size(66, 17)
        Me.lblTen_tk.TabIndex = 16
        Me.lblTen_tk.Text = "lblTen_tk"
        Me.lblTen_tk.Visible = False
        '
        'grpInfor
        '
        Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpInfor.Location = New System.Drawing.Point(10, 0)
        Me.grpInfor.Name = "grpInfor"
        Me.grpInfor.Size = New System.Drawing.Size(632, 614)
        Me.grpInfor.TabIndex = 17
        Me.grpInfor.TabStop = False
        '
        'txtDoi_tac
        '
        Me.txtDoi_tac.Location = New System.Drawing.Point(186, 125)
        Me.txtDoi_tac.Name = "txtDoi_tac"
        Me.txtDoi_tac.Size = New System.Drawing.Size(120, 22)
        Me.txtDoi_tac.TabIndex = 5
        Me.txtDoi_tac.Tag = "FC"
        Me.txtDoi_tac.Text = "txtDoi_tac"
        '
        'lblMa_so_thue
        '
        Me.lblMa_so_thue.AutoSize = True
        Me.lblMa_so_thue.Location = New System.Drawing.Point(342, 21)
        Me.lblMa_so_thue.Name = "lblMa_so_thue"
        Me.lblMa_so_thue.Size = New System.Drawing.Size(78, 17)
        Me.lblMa_so_thue.TabIndex = 27
        Me.lblMa_so_thue.Tag = "L016"
        Me.lblMa_so_thue.Text = "Ma so thue"
        '
        'lblStatusMess
        '
        Me.lblStatusMess.AutoSize = True
        Me.lblStatusMess.Location = New System.Drawing.Point(221, 577)
        Me.lblStatusMess.Name = "lblStatusMess"
        Me.lblStatusMess.Size = New System.Drawing.Size(171, 17)
        Me.lblStatusMess.TabIndex = 21
        Me.lblStatusMess.Tag = "L023"
        Me.lblStatusMess.Text = "1 - Co su dung, 0 - Khong"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(28, 577)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(73, 17)
        Me.lblStatus.TabIndex = 19
        Me.lblStatus.Tag = "L015"
        Me.lblStatus.Text = "Trang thai"
        '
        'txtStatus
        '
        Me.txtStatus.Location = New System.Drawing.Point(186, 575)
        Me.txtStatus.MaxLength = 1
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(30, 22)
        Me.txtStatus.TabIndex = 31
        Me.txtStatus.TabStop = False
        Me.txtStatus.Tag = "FN"
        Me.txtStatus.Text = "txtStatus"
        Me.txtStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMa_kh
        '
        Me.lblMa_kh.AutoSize = True
        Me.lblMa_kh.Location = New System.Drawing.Point(28, 21)
        Me.lblMa_kh.Name = "lblMa_kh"
        Me.lblMa_kh.Size = New System.Drawing.Size(69, 17)
        Me.lblMa_kh.TabIndex = 21
        Me.lblMa_kh.Tag = "L001"
        Me.lblMa_kh.Text = "Ma khach"
        '
        'lblDia_chi
        '
        Me.lblDia_chi.AutoSize = True
        Me.lblDia_chi.Location = New System.Drawing.Point(28, 100)
        Me.lblDia_chi.Name = "lblDia_chi"
        Me.lblDia_chi.Size = New System.Drawing.Size(51, 17)
        Me.lblDia_chi.TabIndex = 23
        Me.lblDia_chi.Tag = "L004"
        Me.lblDia_chi.Text = "Dia chi"
        '
        'txtDia_chi
        '
        Me.txtDia_chi.Location = New System.Drawing.Point(186, 98)
        Me.txtDia_chi.Name = "txtDia_chi"
        Me.txtDia_chi.Size = New System.Drawing.Size(437, 22)
        Me.txtDia_chi.TabIndex = 4
        Me.txtDia_chi.Tag = "FC"
        Me.txtDia_chi.Text = "txtDia_chi"
        '
        'lblDoi_tac
        '
        Me.lblDoi_tac.AutoSize = True
        Me.lblDoi_tac.Location = New System.Drawing.Point(28, 128)
        Me.lblDoi_tac.Name = "lblDoi_tac"
        Me.lblDoi_tac.Size = New System.Drawing.Size(52, 17)
        Me.lblDoi_tac.TabIndex = 25
        Me.lblDoi_tac.Tag = "L005"
        Me.lblDoi_tac.Text = "Doi tac"
        '
        'txtMa_so_thue
        '
        Me.txtMa_so_thue.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_so_thue.Location = New System.Drawing.Point(503, 18)
        Me.txtMa_so_thue.Name = "txtMa_so_thue"
        Me.txtMa_so_thue.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_so_thue.TabIndex = 1
        Me.txtMa_so_thue.Tag = "FC"
        Me.txtMa_so_thue.Text = "TXTMA_SO_THUE"
        '
        'lblHan_tt
        '
        Me.lblHan_tt.AutoSize = True
        Me.lblHan_tt.Location = New System.Drawing.Point(342, 180)
        Me.lblHan_tt.Name = "lblHan_tt"
        Me.lblHan_tt.Size = New System.Drawing.Size(119, 17)
        Me.lblHan_tt.TabIndex = 28
        Me.lblHan_tt.Tag = "L007"
        Me.lblHan_tt.Text = "Ma TT ngam dinh"
        '
        'txtMa_tt
        '
        Me.txtMa_tt.Location = New System.Drawing.Point(503, 178)
        Me.txtMa_tt.Name = "txtMa_tt"
        Me.txtMa_tt.Size = New System.Drawing.Size(30, 22)
        Me.txtMa_tt.TabIndex = 10
        Me.txtMa_tt.Tag = "FCDF"
        Me.txtMa_tt.Text = "txtMa_tt"
        '
        'lblNh_kh1
        '
        Me.lblNh_kh1.AutoSize = True
        Me.lblNh_kh1.Location = New System.Drawing.Point(28, 286)
        Me.lblNh_kh1.Name = "lblNh_kh1"
        Me.lblNh_kh1.Size = New System.Drawing.Size(57, 17)
        Me.lblNh_kh1.TabIndex = 30
        Me.lblNh_kh1.Tag = "L008"
        Me.lblNh_kh1.Text = "Nhom 1"
        '
        'txtNh_kh1
        '
        Me.txtNh_kh1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_kh1.Location = New System.Drawing.Point(186, 284)
        Me.txtNh_kh1.Name = "txtNh_kh1"
        Me.txtNh_kh1.Size = New System.Drawing.Size(120, 22)
        Me.txtNh_kh1.TabIndex = 18
        Me.txtNh_kh1.Tag = "FCDF"
        Me.txtNh_kh1.Text = "TXTNH_KH1"
        '
        'txtNh_kh2
        '
        Me.txtNh_kh2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_kh2.Location = New System.Drawing.Point(186, 310)
        Me.txtNh_kh2.Name = "txtNh_kh2"
        Me.txtNh_kh2.Size = New System.Drawing.Size(120, 22)
        Me.txtNh_kh2.TabIndex = 19
        Me.txtNh_kh2.Tag = "FCDF"
        Me.txtNh_kh2.Text = "TXTNH_KH2"
        '
        'txtNh_kh3
        '
        Me.txtNh_kh3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_kh3.Location = New System.Drawing.Point(186, 337)
        Me.txtNh_kh3.Name = "txtNh_kh3"
        Me.txtNh_kh3.Size = New System.Drawing.Size(120, 22)
        Me.txtNh_kh3.TabIndex = 20
        Me.txtNh_kh3.Tag = "FCDF"
        Me.txtNh_kh3.Text = "TXTNH_KH3"
        '
        'lblNh_kh2
        '
        Me.lblNh_kh2.AutoSize = True
        Me.lblNh_kh2.Location = New System.Drawing.Point(28, 313)
        Me.lblNh_kh2.Name = "lblNh_kh2"
        Me.lblNh_kh2.Size = New System.Drawing.Size(57, 17)
        Me.lblNh_kh2.TabIndex = 34
        Me.lblNh_kh2.Tag = "L009"
        Me.lblNh_kh2.Text = "Nhom 2"
        '
        'lblNh_kh3
        '
        Me.lblNh_kh3.AutoSize = True
        Me.lblNh_kh3.Location = New System.Drawing.Point(28, 339)
        Me.lblNh_kh3.Name = "lblNh_kh3"
        Me.lblNh_kh3.Size = New System.Drawing.Size(57, 17)
        Me.lblNh_kh3.TabIndex = 35
        Me.lblNh_kh3.Tag = "L010"
        Me.lblNh_kh3.Text = "Nhom 3"
        '
        'lblDien_thoai
        '
        Me.lblDien_thoai.AutoSize = True
        Me.lblDien_thoai.Location = New System.Drawing.Point(28, 395)
        Me.lblDien_thoai.Name = "lblDien_thoai"
        Me.lblDien_thoai.Size = New System.Drawing.Size(72, 17)
        Me.lblDien_thoai.TabIndex = 37
        Me.lblDien_thoai.Tag = "L011"
        Me.lblDien_thoai.Text = "Dien thoai"
        '
        'txtDien_thoai
        '
        Me.txtDien_thoai.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDien_thoai.Location = New System.Drawing.Point(186, 392)
        Me.txtDien_thoai.Name = "txtDien_thoai"
        Me.txtDien_thoai.Size = New System.Drawing.Size(120, 22)
        Me.txtDien_thoai.TabIndex = 22
        Me.txtDien_thoai.Tag = "FC"
        Me.txtDien_thoai.Text = "TXTDIEN_THOAI"
        '
        'lblFax
        '
        Me.lblFax.AutoSize = True
        Me.lblFax.Location = New System.Drawing.Point(342, 395)
        Me.lblFax.Name = "lblFax"
        Me.lblFax.Size = New System.Drawing.Size(30, 17)
        Me.lblFax.TabIndex = 39
        Me.lblFax.Tag = "L020"
        Me.lblFax.Text = "Fax"
        '
        'txtFax
        '
        Me.txtFax.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFax.Location = New System.Drawing.Point(503, 392)
        Me.txtFax.Name = "txtFax"
        Me.txtFax.Size = New System.Drawing.Size(120, 22)
        Me.txtFax.TabIndex = 23
        Me.txtFax.Tag = "FC"
        Me.txtFax.Text = "TXTFAX"
        '
        'lblEmail
        '
        Me.lblEmail.AutoSize = True
        Me.lblEmail.Location = New System.Drawing.Point(28, 421)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(42, 17)
        Me.lblEmail.TabIndex = 41
        Me.lblEmail.Tag = "L012"
        Me.lblEmail.Text = "Email"
        '
        'txtE_mail
        '
        Me.txtE_mail.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtE_mail.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtE_mail.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtE_mail.Location = New System.Drawing.Point(186, 419)
        Me.txtE_mail.Name = "txtE_mail"
        Me.txtE_mail.Size = New System.Drawing.Size(438, 23)
        Me.txtE_mail.TabIndex = 24
        Me.txtE_mail.Tag = "FC"
        Me.txtE_mail.Text = "txtE_mail"
        '
        'lblTk_nh
        '
        Me.lblTk_nh.AutoSize = True
        Me.lblTk_nh.Location = New System.Drawing.Point(28, 474)
        Me.lblTk_nh.Name = "lblTk_nh"
        Me.lblTk_nh.Size = New System.Drawing.Size(96, 17)
        Me.lblTk_nh.TabIndex = 43
        Me.lblTk_nh.Tag = "L021"
        Me.lblTk_nh.Text = "Tk ngan hang"
        '
        'txtTk_nh
        '
        Me.txtTk_nh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_nh.Location = New System.Drawing.Point(186, 472)
        Me.txtTk_nh.Name = "txtTk_nh"
        Me.txtTk_nh.Size = New System.Drawing.Size(120, 22)
        Me.txtTk_nh.TabIndex = 26
        Me.txtTk_nh.Tag = "FC"
        Me.txtTk_nh.Text = "TXTTK_NH"
        '
        'lblTen_nh
        '
        Me.lblTen_nh.AutoSize = True
        Me.lblTen_nh.Location = New System.Drawing.Point(311, 474)
        Me.lblTen_nh.Name = "lblTen_nh"
        Me.lblTen_nh.Size = New System.Drawing.Size(105, 17)
        Me.lblTen_nh.TabIndex = 45
        Me.lblTen_nh.Tag = "L013"
        Me.lblTen_nh.Text = "Ten ngan hang"
        '
        'txtNgan_hang
        '
        Me.txtNgan_hang.Location = New System.Drawing.Point(413, 472)
        Me.txtNgan_hang.Name = "txtNgan_hang"
        Me.txtNgan_hang.Size = New System.Drawing.Size(211, 22)
        Me.txtNgan_hang.TabIndex = 27
        Me.txtNgan_hang.Tag = "FC"
        Me.txtNgan_hang.Text = "txtTen_nh"
        '
        'lblTinh_thanh
        '
        Me.lblTinh_thanh.AutoSize = True
        Me.lblTinh_thanh.Location = New System.Drawing.Point(28, 501)
        Me.lblTinh_thanh.Name = "lblTinh_thanh"
        Me.lblTinh_thanh.Size = New System.Drawing.Size(76, 17)
        Me.lblTinh_thanh.TabIndex = 47
        Me.lblTinh_thanh.Tag = "L022"
        Me.lblTinh_thanh.Text = "Tinh thanh"
        '
        'chkKh_yn
        '
        Me.chkKh_yn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkKh_yn.Location = New System.Drawing.Point(52, 231)
        Me.chkKh_yn.Name = "chkKh_yn"
        Me.chkKh_yn.Size = New System.Drawing.Size(125, 23)
        Me.chkKh_yn.TabIndex = 13
        Me.chkKh_yn.Tag = "L017FLDF"
        Me.chkKh_yn.Text = "Khach hang"
        '
        'chkCc_yn
        '
        Me.chkCc_yn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCc_yn.Location = New System.Drawing.Point(212, 231)
        Me.chkCc_yn.Name = "chkCc_yn"
        Me.chkCc_yn.Size = New System.Drawing.Size(119, 23)
        Me.chkCc_yn.TabIndex = 14
        Me.chkCc_yn.Tag = "L018FLDF"
        Me.chkCc_yn.Text = "Nha cung cap"
        '
        'chkNv_yn
        '
        Me.chkNv_yn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkNv_yn.Location = New System.Drawing.Point(369, 231)
        Me.chkNv_yn.Name = "chkNv_yn"
        Me.chkNv_yn.Size = New System.Drawing.Size(125, 23)
        Me.chkNv_yn.TabIndex = 15
        Me.chkNv_yn.Tag = "L019FLDF"
        Me.chkNv_yn.Text = "Nhan vien"
        '
        'lblTen_nh_kh1
        '
        Me.lblTen_nh_kh1.AutoSize = True
        Me.lblTen_nh_kh1.Location = New System.Drawing.Point(311, 286)
        Me.lblTen_nh_kh1.Name = "lblTen_nh_kh1"
        Me.lblTen_nh_kh1.Size = New System.Drawing.Size(102, 17)
        Me.lblTen_nh_kh1.TabIndex = 18
        Me.lblTen_nh_kh1.Text = "lblTen_nh_kh1"
        '
        'lblTen_nh_kh2
        '
        Me.lblTen_nh_kh2.AutoSize = True
        Me.lblTen_nh_kh2.Location = New System.Drawing.Point(311, 313)
        Me.lblTen_nh_kh2.Name = "lblTen_nh_kh2"
        Me.lblTen_nh_kh2.Size = New System.Drawing.Size(102, 17)
        Me.lblTen_nh_kh2.TabIndex = 20
        Me.lblTen_nh_kh2.Text = "lblTen_nh_kh2"
        '
        'lblTen_nh_kh3
        '
        Me.lblTen_nh_kh3.AutoSize = True
        Me.lblTen_nh_kh3.Location = New System.Drawing.Point(311, 339)
        Me.lblTen_nh_kh3.Name = "lblTen_nh_kh3"
        Me.lblTen_nh_kh3.Size = New System.Drawing.Size(102, 17)
        Me.lblTen_nh_kh3.TabIndex = 53
        Me.lblTen_nh_kh3.Text = "lblTen_nh_kh3"
        '
        'lblGhi_chu
        '
        Me.lblGhi_chu.AutoSize = True
        Me.lblGhi_chu.Location = New System.Drawing.Point(28, 527)
        Me.lblGhi_chu.Name = "lblGhi_chu"
        Me.lblGhi_chu.Size = New System.Drawing.Size(57, 17)
        Me.lblGhi_chu.TabIndex = 55
        Me.lblGhi_chu.Tag = "L014"
        Me.lblGhi_chu.Text = "Ghi chu"
        '
        'txtGhi_chu
        '
        Me.txtGhi_chu.Location = New System.Drawing.Point(186, 525)
        Me.txtGhi_chu.Multiline = True
        Me.txtGhi_chu.Name = "txtGhi_chu"
        Me.txtGhi_chu.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtGhi_chu.Size = New System.Drawing.Size(437, 46)
        Me.txtGhi_chu.TabIndex = 30
        Me.txtGhi_chu.Tag = "FT"
        Me.txtGhi_chu.Text = "txtGhi_chu"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(342, 153)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(92, 17)
        Me.Label1.TabIndex = 57
        Me.Label1.Tag = "L026"
        Me.Label1.Text = "Nhan vien bh"
        '
        'txtMa_nvbh
        '
        Me.txtMa_nvbh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_nvbh.Location = New System.Drawing.Point(503, 151)
        Me.txtMa_nvbh.Name = "txtMa_nvbh"
        Me.txtMa_nvbh.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_nvbh.TabIndex = 8
        Me.txtMa_nvbh.Tag = "FC"
        Me.txtMa_nvbh.Text = "TXTMA_NVBH"
        '
        'lblTen_nvbh
        '
        Me.lblTen_nvbh.AutoSize = True
        Me.lblTen_nvbh.Location = New System.Drawing.Point(518, 629)
        Me.lblTen_nvbh.Name = "lblTen_nvbh"
        Me.lblTen_nvbh.Size = New System.Drawing.Size(86, 17)
        Me.lblTen_nvbh.TabIndex = 58
        Me.lblTen_nvbh.Tag = "L022"
        Me.lblTen_nvbh.Text = "lblTen_nvbh"
        Me.lblTen_nvbh.Visible = False
        '
        'lbTen_tt
        '
        Me.lbTen_tt.AutoSize = True
        Me.lbTen_tt.Location = New System.Drawing.Point(422, 629)
        Me.lbTen_tt.Name = "lbTen_tt"
        Me.lbTen_tt.Size = New System.Drawing.Size(51, 17)
        Me.lbTen_tt.TabIndex = 59
        Me.lbTen_tt.Text = "Label2"
        Me.lbTen_tt.Visible = False
        '
        'lblTen_nh_kh9
        '
        Me.lblTen_nh_kh9.AutoSize = True
        Me.lblTen_nh_kh9.Location = New System.Drawing.Point(311, 260)
        Me.lblTen_nh_kh9.Name = "lblTen_nh_kh9"
        Me.lblTen_nh_kh9.Size = New System.Drawing.Size(102, 17)
        Me.lblTen_nh_kh9.TabIndex = 16
        Me.lblTen_nh_kh9.Text = "lblTen_nh_kh9"
        '
        'lblNh_kh9
        '
        Me.lblNh_kh9.AutoSize = True
        Me.lblNh_kh9.Location = New System.Drawing.Point(28, 260)
        Me.lblNh_kh9.Name = "lblNh_kh9"
        Me.lblNh_kh9.Size = New System.Drawing.Size(68, 17)
        Me.lblNh_kh9.TabIndex = 61
        Me.lblNh_kh9.Tag = "L027"
        Me.lblNh_kh9.Text = "Nhom gia"
        '
        'txtNh_kh9
        '
        Me.txtNh_kh9.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_kh9.Location = New System.Drawing.Point(186, 257)
        Me.txtNh_kh9.Name = "txtNh_kh9"
        Me.txtNh_kh9.Size = New System.Drawing.Size(120, 22)
        Me.txtNh_kh9.TabIndex = 17
        Me.txtNh_kh9.Tag = "FCDF"
        Me.txtNh_kh9.Text = "TXTNH_KH9"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(28, 448)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(81, 17)
        Me.Label4.TabIndex = 64
        Me.Label4.Tag = "L028"
        Me.Label4.Text = "Home page"
        '
        'txtHome_page
        '
        Me.txtHome_page.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHome_page.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHome_page.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHome_page.Location = New System.Drawing.Point(186, 446)
        Me.txtHome_page.Name = "txtHome_page"
        Me.txtHome_page.Size = New System.Drawing.Size(438, 23)
        Me.txtHome_page.TabIndex = 25
        Me.txtHome_page.Tag = "FC"
        Me.txtHome_page.Text = "txtHome_page"
        '
        'lblT_tien_cn
        '
        Me.lblT_tien_cn.AutoSize = True
        Me.lblT_tien_cn.Location = New System.Drawing.Point(28, 207)
        Me.lblT_tien_cn.Name = "lblT_tien_cn"
        Me.lblT_tien_cn.Size = New System.Drawing.Size(108, 17)
        Me.lblT_tien_cn.TabIndex = 66
        Me.lblT_tien_cn.Tag = "L029"
        Me.lblT_tien_cn.Text = "Gioi han tien no"
        '
        'txtT_tien_cn
        '
        Me.txtT_tien_cn.Format = "m_ip_tien"
        Me.txtT_tien_cn.Location = New System.Drawing.Point(186, 204)
        Me.txtT_tien_cn.MaxLength = 10
        Me.txtT_tien_cn.Name = "txtT_tien_cn"
        Me.txtT_tien_cn.Size = New System.Drawing.Size(120, 22)
        Me.txtT_tien_cn.TabIndex = 11
        Me.txtT_tien_cn.Tag = "FN"
        Me.txtT_tien_cn.Text = "m_ip_tien"
        Me.txtT_tien_cn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien_cn.Value = 0R
        '
        'lblNgay_gh
        '
        Me.lblNgay_gh.AutoSize = True
        Me.lblNgay_gh.Location = New System.Drawing.Point(342, 207)
        Me.lblNgay_gh.Name = "lblNgay_gh"
        Me.lblNgay_gh.Size = New System.Drawing.Size(69, 17)
        Me.lblNgay_gh.TabIndex = 68
        Me.lblNgay_gh.Tag = "L031"
        Me.lblNgay_gh.Text = "Den ngay"
        '
        'txtNgay_gh
        '
        Me.txtNgay_gh.Location = New System.Drawing.Point(503, 204)
        Me.txtNgay_gh.MaxLength = 10
        Me.txtNgay_gh.Name = "txtNgay_gh"
        Me.txtNgay_gh.Size = New System.Drawing.Size(120, 22)
        Me.txtNgay_gh.TabIndex = 12
        Me.txtNgay_gh.Tag = "FD"
        Me.txtNgay_gh.Text = "  /  /    "
        Me.txtNgay_gh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_gh.Value = New Date(CType(0, Long))
        '
        'lblVisible
        '
        Me.lblVisible.AutoSize = True
        Me.lblVisible.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVisible.ForeColor = System.Drawing.Color.Green
        Me.lblVisible.Location = New System.Drawing.Point(662, 600)
        Me.lblVisible.Name = "lblVisible"
        Me.lblVisible.Size = New System.Drawing.Size(63, 17)
        Me.lblVisible.TabIndex = 69
        Me.lblVisible.Tag = ""
        Me.lblVisible.Text = "lblVisible"
        Me.lblVisible.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(307, 127)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 17)
        Me.Label2.TabIndex = 71
        Me.Label2.Tag = "LZ01"
        Me.Label2.Text = "Dai dien"
        '
        'txtOng_ba
        '
        Me.txtOng_ba.Location = New System.Drawing.Point(365, 125)
        Me.txtOng_ba.Name = "txtOng_ba"
        Me.txtOng_ba.Size = New System.Drawing.Size(259, 22)
        Me.txtOng_ba.TabIndex = 6
        Me.txtOng_ba.Tag = "FC"
        Me.txtOng_ba.Text = "txtOng_ba"
        '
        'cboS1
        '
        Me.cboS1.DefaultValue = ""
        Me.cboS1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboS1.FieldDisplay = ""
        Me.cboS1.FieldValue = ""
        Me.cboS1.IsAutocomplete = False
        Me.cboS1.IsBlankItem = False
        Me.cboS1.Location = New System.Drawing.Point(413, 499)
        Me.cboS1.Name = "cboS1"
        Me.cboS1.ReadOnly = False
        Me.cboS1.Size = New System.Drawing.Size(211, 24)
        Me.cboS1.TabIndex = 29
        Me.cboS1.TableName = ""
        Me.cboS1.Tag = "FC"
        Me.cboS1.ValDataType = "STRING"
        Me.cboS1.Value = Nothing
        '
        'cboTinh_thanh
        '
        Me.cboTinh_thanh.DefaultValue = ""
        Me.cboTinh_thanh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTinh_thanh.FieldDisplay = ""
        Me.cboTinh_thanh.FieldValue = ""
        Me.cboTinh_thanh.IsAutocomplete = False
        Me.cboTinh_thanh.IsBlankItem = False
        Me.cboTinh_thanh.Location = New System.Drawing.Point(186, 499)
        Me.cboTinh_thanh.Name = "cboTinh_thanh"
        Me.cboTinh_thanh.ReadOnly = False
        Me.cboTinh_thanh.Size = New System.Drawing.Size(119, 24)
        Me.cboTinh_thanh.TabIndex = 28
        Me.cboTinh_thanh.TableName = ""
        Me.cboTinh_thanh.Tag = "FC"
        Me.cboTinh_thanh.ValDataType = "STRING"
        Me.cboTinh_thanh.Value = Nothing
        '
        'lblTen_nh_kh4
        '
        Me.lblTen_nh_kh4.AutoSize = True
        Me.lblTen_nh_kh4.Location = New System.Drawing.Point(312, 366)
        Me.lblTen_nh_kh4.Name = "lblTen_nh_kh4"
        Me.lblTen_nh_kh4.Size = New System.Drawing.Size(102, 17)
        Me.lblTen_nh_kh4.TabIndex = 74
        Me.lblTen_nh_kh4.Text = "lblTen_nh_kh4"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(29, 366)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(57, 17)
        Me.Label5.TabIndex = 73
        Me.Label5.Tag = "LZ02"
        Me.Label5.Text = "Nhom 4"
        '
        'txtNh_kh4
        '
        Me.txtNh_kh4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_kh4.Location = New System.Drawing.Point(187, 364)
        Me.txtNh_kh4.Name = "txtNh_kh4"
        Me.txtNh_kh4.Size = New System.Drawing.Size(120, 22)
        Me.txtNh_kh4.TabIndex = 21
        Me.txtNh_kh4.Tag = "FCDF"
        Me.txtNh_kh4.Text = "TXTNH_KH4"
        '
        'chkCa_nhan_yn
        '
        Me.chkCa_nhan_yn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCa_nhan_yn.Location = New System.Drawing.Point(503, 231)
        Me.chkCa_nhan_yn.Name = "chkCa_nhan_yn"
        Me.chkCa_nhan_yn.Size = New System.Drawing.Size(125, 23)
        Me.chkCa_nhan_yn.TabIndex = 16
        Me.chkCa_nhan_yn.Tag = ""
        Me.chkCa_nhan_yn.Text = "Cá nhân"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(28, 154)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 17)
        Me.Label3.TabIndex = 77
        Me.Label3.Tag = ""
        Me.Label3.Text = "CCCD/No ID"
        '
        'txtNo_id
        '
        Me.txtNo_id.Location = New System.Drawing.Point(186, 151)
        Me.txtNo_id.Name = "txtNo_id"
        Me.txtNo_id.Size = New System.Drawing.Size(120, 22)
        Me.txtNo_id.TabIndex = 7
        Me.txtNo_id.Tag = "FC"
        Me.txtNo_id.Text = "txtNo_id"
        '
        'frmDirInfor
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(652, 652)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtNo_id)
        Me.Controls.Add(Me.chkCa_nhan_yn)
        Me.Controls.Add(Me.txtMa_so_thue)
        Me.Controls.Add(Me.lblMa_so_thue)
        Me.Controls.Add(Me.lblTen_nh_kh4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtNh_kh4)
        Me.Controls.Add(Me.cboTinh_thanh)
        Me.Controls.Add(Me.cboS1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtOng_ba)
        Me.Controls.Add(Me.lblVisible)
        Me.Controls.Add(Me.lblNgay_gh)
        Me.Controls.Add(Me.txtNgay_gh)
        Me.Controls.Add(Me.lblT_tien_cn)
        Me.Controls.Add(Me.txtT_tien_cn)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtHome_page)
        Me.Controls.Add(Me.lblTen_nh_kh9)
        Me.Controls.Add(Me.lblNh_kh9)
        Me.Controls.Add(Me.txtNh_kh9)
        Me.Controls.Add(Me.lblTen_nvbh)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtMa_nvbh)
        Me.Controls.Add(Me.lblGhi_chu)
        Me.Controls.Add(Me.lblTen_nh_kh3)
        Me.Controls.Add(Me.lblTen_nh_kh2)
        Me.Controls.Add(Me.lblTen_nh_kh1)
        Me.Controls.Add(Me.lblTinh_thanh)
        Me.Controls.Add(Me.lblTen_nh)
        Me.Controls.Add(Me.lblTk_nh)
        Me.Controls.Add(Me.lblEmail)
        Me.Controls.Add(Me.lblFax)
        Me.Controls.Add(Me.lblDien_thoai)
        Me.Controls.Add(Me.lblNh_kh3)
        Me.Controls.Add(Me.lblNh_kh2)
        Me.Controls.Add(Me.lblNh_kh1)
        Me.Controls.Add(Me.lblHan_tt)
        Me.Controls.Add(Me.lblDoi_tac)
        Me.Controls.Add(Me.lblDia_chi)
        Me.Controls.Add(Me.lblMa_kh)
        Me.Controls.Add(Me.lblStatusMess)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblTen_tk)
        Me.Controls.Add(Me.lblTk)
        Me.Controls.Add(Me.lblTen_kh2)
        Me.Controls.Add(Me.lblTen_kh)
        Me.Controls.Add(Me.txtGhi_chu)
        Me.Controls.Add(Me.txtNgan_hang)
        Me.Controls.Add(Me.txtTk_nh)
        Me.Controls.Add(Me.txtE_mail)
        Me.Controls.Add(Me.txtFax)
        Me.Controls.Add(Me.txtDien_thoai)
        Me.Controls.Add(Me.txtNh_kh3)
        Me.Controls.Add(Me.txtNh_kh2)
        Me.Controls.Add(Me.txtNh_kh1)
        Me.Controls.Add(Me.txtMa_tt)
        Me.Controls.Add(Me.txtDia_chi)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.txtTk)
        Me.Controls.Add(Me.txtTen_kh2)
        Me.Controls.Add(Me.txtTen_kh)
        Me.Controls.Add(Me.txtMa_kh)
        Me.Controls.Add(Me.lbTen_tt)
        Me.Controls.Add(Me.txtDoi_tac)
        Me.Controls.Add(Me.chkNv_yn)
        Me.Controls.Add(Me.chkCc_yn)
        Me.Controls.Add(Me.chkKh_yn)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.grpInfor)
        Me.Name = "frmDirInfor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmDirInfor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private Sub lblE_mail_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Process.Start(("mailto: " & Me.txtE_mail.Text.Trim))
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            ProjectData.ClearProjectError()
        End Try
    End Sub

    Private Sub lblHome_page_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Process.Start(Me.txtHome_page.Text.Trim)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            ProjectData.ClearProjectError()
        End Try
    End Sub

    Private Sub txt_Enter(ByVal sender As Object, ByVal e As EventArgs)
        LateBinding.LateSetComplex(LateBinding.LateGet(sender, Nothing, "Controls", New Object() {0}, Nothing, Nothing), Nothing, "Visible", New Object() {False}, Nothing, False, True)
    End Sub

    Private Sub txt_TextChange(ByVal sender As Object, ByVal e As EventArgs)
        LateBinding.LateSet(sender, Nothing, "Text", New Object() {Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))}, Nothing)
    End Sub

    Private Sub txt_Valid(ByVal sender As Object, ByVal e As EventArgs)
        LateBinding.LateSetComplex(LateBinding.LateGet(sender, Nothing, "Controls", New Object() {0}, Nothing, Nothing), Nothing, "Visible", New Object() {True}, Nothing, False, True)
    End Sub




    ' Fields
    Friend WithEvents chkCc_yn As CheckBox
    Friend WithEvents chkKh_yn As CheckBox
    Friend WithEvents chkNv_yn As CheckBox
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lblDia_chi As Label
    Friend WithEvents lblDien_thoai As Label
    Friend WithEvents lblDoi_tac As Label
    Friend WithEvents lblEmail As Label
    Friend WithEvents lblFax As Label
    Friend WithEvents lblGhi_chu As Label
    Friend WithEvents lblHan_tt As Label
    Friend WithEvents lblMa_kh As Label
    Friend WithEvents lblMa_so_thue As Label
    Friend WithEvents lblNgay_gh As Label
    Friend WithEvents lblNh_kh1 As Label
    Friend WithEvents lblNh_kh2 As Label
    Friend WithEvents lblNh_kh3 As Label
    Friend WithEvents lblNh_kh9 As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblStatusMess As Label
    Friend WithEvents lblT_tien_cn As Label
    Friend WithEvents lblTen_kh As Label
    Friend WithEvents lblTen_kh2 As Label
    Friend WithEvents lblTen_nh As Label
    Friend WithEvents lblTen_nh_kh1 As Label
    Friend WithEvents lblTen_nh_kh2 As Label
    Friend WithEvents lblTen_nh_kh3 As Label
    Friend WithEvents lblTen_nh_kh9 As Label
    Friend WithEvents lblTen_nvbh As Label
    Friend WithEvents lblTen_tk As Label
    Friend WithEvents lblTinh_thanh As Label
    Friend WithEvents lblTk As Label
    Friend WithEvents lblTk_nh As Label
    Friend WithEvents lblVisible As Label
    Friend WithEvents lbTen_tt As Label
    Friend WithEvents txtDia_chi As TextBox
    Friend WithEvents txtDien_thoai As TextBox
    Friend WithEvents txtDoi_tac As TextBox
    Friend WithEvents txtE_mail As TextBox
    Friend WithEvents txtFax As TextBox
    Friend WithEvents txtGhi_chu As TextBox
    Friend WithEvents txtHome_page As TextBox
    Friend WithEvents txtMa_kh As TextBox
    Friend WithEvents txtMa_nvbh As TextBox
    Friend WithEvents txtMa_so_thue As TextBox
    Friend WithEvents txtMa_tt As TextBox
    Friend WithEvents txtNgan_hang As TextBox
    Friend WithEvents txtNgay_gh As txtDate
    Friend WithEvents txtNh_kh1 As TextBox
    Friend WithEvents txtNh_kh2 As TextBox
    Friend WithEvents txtNh_kh3 As TextBox
    Friend WithEvents txtNh_kh9 As TextBox
    Friend WithEvents txtStatus As TextBox
    Friend WithEvents txtT_tien_cn As txtNumeric
    Friend WithEvents txtTen_kh As TextBox
    Friend WithEvents txtTen_kh2 As TextBox
    Friend WithEvents txtTk As TextBox
    Friend WithEvents txtTk_nh As TextBox

    Private components As IContainer
    Private lblE_mail As Label
    Private lblHome_page As Label
    Dim old_code As String = ""
End Class

