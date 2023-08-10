Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon


Namespace v20cobp
    Public Class frmDirInfor
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Closed, New EventHandler(AddressOf Me.frmDirInfor_Closed)
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
            Me.InitializeComponent()
        End Sub

        Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
            DirMain.oDirFormLib.SaveFormDir(Me, StringType.FromObject(Sql.ConvertVS2SQLType(Me.txtMa_bp.Text, "")))
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
                Me.txtStatus.Text = "1"
                Me.txtCap_bp.Value = 0
            End If
            Dim obj2 As Object = New CharLib(Me.txtStatus, "0, 1")
            Dim vdept As New DirLib(Me.txtMa_bp, Me.lblTen_bp, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmbp", "ma_bp", "ten_bp", "Dept", "1=1", False, Me.cmdCancel)
            Dim lib2 As New DirLib(Me.txtMa_bp0, Me.lblTen_bp0, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmbp", "ma_bp", "ten_bp", "Dept", "1=1", True, Me.cmdCancel)
        End Sub
        Friend WithEvents txtStt As txtNumeric
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents txtS4 As txtNumeric

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.txtMa_bp = New System.Windows.Forms.TextBox
            Me.lblMa_bp = New System.Windows.Forms.Label
            Me.lblTen_bp2 = New System.Windows.Forms.Label
            Me.cmdOk = New System.Windows.Forms.Button
            Me.cmdCancel = New System.Windows.Forms.Button
            Me.grpInfor = New System.Windows.Forms.GroupBox
            Me.lblStatusMess = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.txtStatus = New System.Windows.Forms.TextBox
            Me.txtMa_bp0 = New System.Windows.Forms.TextBox
            Me.lblTen_bp = New System.Windows.Forms.Label
            Me.lblTen_bp0 = New System.Windows.Forms.Label
            Me.chkTruc_tiep = New System.Windows.Forms.CheckBox
            Me.txtCap_bp = New txtNumeric
            Me.txtStt = New txtNumeric
            Me.Label1 = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.txtS4 = New txtNumeric
            Me.SuspendLayout()
            '
            'txtMa_bp
            '
            Me.txtMa_bp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_bp.Location = New System.Drawing.Point(155, 41)
            Me.txtMa_bp.Name = "txtMa_bp"
            Me.txtMa_bp.TabIndex = 1
            Me.txtMa_bp.Tag = "FCNB"
            Me.txtMa_bp.Text = "TXTMA_BP"
            '
            'lblMa_bp
            '
            Me.lblMa_bp.AutoSize = True
            Me.lblMa_bp.Location = New System.Drawing.Point(23, 43)
            Me.lblMa_bp.Name = "lblMa_bp"
            Me.lblMa_bp.Size = New System.Drawing.Size(64, 16)
            Me.lblMa_bp.TabIndex = 5
            Me.lblMa_bp.Tag = "L001"
            Me.lblMa_bp.Text = "Ma bo phan"
            '
            'lblTen_bp2
            '
            Me.lblTen_bp2.AutoSize = True
            Me.lblTen_bp2.Location = New System.Drawing.Point(23, 89)
            Me.lblTen_bp2.Name = "lblTen_bp2"
            Me.lblTen_bp2.Size = New System.Drawing.Size(82, 16)
            Me.lblTen_bp2.TabIndex = 8
            Me.lblTen_bp2.Tag = "L003"
            Me.lblTen_bp2.Text = "Ma bo phan me"
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.Location = New System.Drawing.Point(8, 156)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 5
            Me.cmdOk.Tag = "L005"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(84, 156)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 6
            Me.cmdCancel.Tag = "L006"
            Me.cmdCancel.Text = "Huy"
            '
            'grpInfor
            '
            Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grpInfor.Location = New System.Drawing.Point(8, 4)
            Me.grpInfor.Name = "grpInfor"
            Me.grpInfor.Size = New System.Drawing.Size(592, 140)
            Me.grpInfor.TabIndex = 17
            Me.grpInfor.TabStop = False
            '
            'lblStatusMess
            '
            Me.lblStatusMess.AutoSize = True
            Me.lblStatusMess.Location = New System.Drawing.Point(344, 152)
            Me.lblStatusMess.Name = "lblStatusMess"
            Me.lblStatusMess.Size = New System.Drawing.Size(133, 16)
            Me.lblStatusMess.TabIndex = 20
            Me.lblStatusMess.Tag = "L007"
            Me.lblStatusMess.Text = "1 - Co su dung, 0 - Khong"
            Me.lblStatusMess.Visible = False
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(216, 160)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(55, 16)
            Me.Label2.TabIndex = 19
            Me.Label2.Tag = "L004"
            Me.Label2.Text = "Trang thai"
            Me.Label2.Visible = False
            '
            'txtStatus
            '
            Me.txtStatus.Location = New System.Drawing.Point(304, 152)
            Me.txtStatus.MaxLength = 1
            Me.txtStatus.Name = "txtStatus"
            Me.txtStatus.Size = New System.Drawing.Size(25, 20)
            Me.txtStatus.TabIndex = 3
            Me.txtStatus.TabStop = False
            Me.txtStatus.Tag = "FC"
            Me.txtStatus.Text = "txtStatus"
            Me.txtStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtStatus.Visible = False
            '
            'txtMa_bp0
            '
            Me.txtMa_bp0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_bp0.Location = New System.Drawing.Point(155, 87)
            Me.txtMa_bp0.Name = "txtMa_bp0"
            Me.txtMa_bp0.TabIndex = 3
            Me.txtMa_bp0.Tag = "FC"
            Me.txtMa_bp0.Text = "TXTMA_BP0"
            '
            'lblTen_bp
            '
            Me.lblTen_bp.AutoSize = True
            Me.lblTen_bp.Location = New System.Drawing.Point(259, 43)
            Me.lblTen_bp.Name = "lblTen_bp"
            Me.lblTen_bp.Size = New System.Drawing.Size(68, 16)
            Me.lblTen_bp.TabIndex = 22
            Me.lblTen_bp.Tag = "RF"
            Me.lblTen_bp.Text = "Ten bo phan"
            '
            'lblTen_bp0
            '
            Me.lblTen_bp0.AutoSize = True
            Me.lblTen_bp0.Location = New System.Drawing.Point(259, 89)
            Me.lblTen_bp0.Name = "lblTen_bp0"
            Me.lblTen_bp0.Size = New System.Drawing.Size(74, 16)
            Me.lblTen_bp0.TabIndex = 23
            Me.lblTen_bp0.Tag = "RF"
            Me.lblTen_bp0.Text = "Ten bo phan0"
            '
            'chkTruc_tiep
            '
            Me.chkTruc_tiep.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.chkTruc_tiep.Location = New System.Drawing.Point(23, 66)
            Me.chkTruc_tiep.Name = "chkTruc_tiep"
            Me.chkTruc_tiep.Size = New System.Drawing.Size(145, 16)
            Me.chkTruc_tiep.TabIndex = 2
            Me.chkTruc_tiep.Tag = "L002FLDF"
            Me.chkTruc_tiep.Text = "Truc tiep"
            '
            'txtCap_bp
            '
            Me.txtCap_bp.Format = ""
            Me.txtCap_bp.Location = New System.Drawing.Point(488, 152)
            Me.txtCap_bp.MaxLength = 1
            Me.txtCap_bp.Name = "txtCap_bp"
            Me.txtCap_bp.TabIndex = 24
            Me.txtCap_bp.Tag = "FN"
            Me.txtCap_bp.Text = "0"
            Me.txtCap_bp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtCap_bp.Value = 0
            Me.txtCap_bp.Visible = False
            '
            'txtStt
            '
            Me.txtStt.Format = ""
            Me.txtStt.Location = New System.Drawing.Point(155, 16)
            Me.txtStt.MaxLength = 1
            Me.txtStt.Name = "txtStt"
            Me.txtStt.TabIndex = 0
            Me.txtStt.Tag = "FNNB"
            Me.txtStt.Text = "0"
            Me.txtStt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtStt.Value = 0
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(23, 18)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(92, 16)
            Me.Label1.TabIndex = 26
            Me.Label1.Tag = "L008"
            Me.Label1.Text = "Thu tu cong doan"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(23, 114)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(92, 16)
            Me.Label3.TabIndex = 28
            Me.Label3.Tag = "L009"
            Me.Label3.Text = "Thu tu cong doan"
            '
            'txtS4
            '
            Me.txtS4.Format = "m_ip_tien"
            Me.txtS4.Location = New System.Drawing.Point(155, 112)
            Me.txtS4.MaxLength = 10
            Me.txtS4.Name = "txtS4"
            Me.txtS4.TabIndex = 4
            Me.txtS4.Tag = "FN"
            Me.txtS4.Text = "m_ip_tien"
            Me.txtS4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtS4.Value = 0
            '
            'frmDirInfor
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(608, 185)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.txtS4)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.txtStt)
            Me.Controls.Add(Me.txtCap_bp)
            Me.Controls.Add(Me.chkTruc_tiep)
            Me.Controls.Add(Me.lblTen_bp0)
            Me.Controls.Add(Me.lblTen_bp)
            Me.Controls.Add(Me.txtMa_bp0)
            Me.Controls.Add(Me.lblStatusMess)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.txtStatus)
            Me.Controls.Add(Me.lblTen_bp2)
            Me.Controls.Add(Me.lblMa_bp)
            Me.Controls.Add(Me.txtMa_bp)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.grpInfor)
            Me.Name = "frmDirInfor"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmDirInfor"
            Me.ResumeLayout(False)

        End Sub

        ' Properties
        Friend WithEvents chkTruc_tiep As CheckBox
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents grpInfor As GroupBox
        Friend WithEvents Label2 As Label
        Friend WithEvents lblMa_bp As Label
        Friend WithEvents lblStatusMess As Label
        Friend WithEvents lblTen_bp As Label
        Friend WithEvents lblTen_bp0 As Label
        Friend WithEvents lblTen_bp2 As Label
        Friend WithEvents txtCap_bp As txtNumeric
        Friend WithEvents txtMa_bp As TextBox
        Friend WithEvents txtMa_bp0 As TextBox
        Friend WithEvents txtStatus As TextBox

        Private components As IContainer
    End Class
End Namespace

