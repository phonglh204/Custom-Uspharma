Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol
Imports libscontrol.voucherseachlib

Namespace insdkc
    Public Class frmSdkc
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmkcsd_Load)
            AddHandler MyBase.Closed, New EventHandler(AddressOf Me.frmnamdn_Closed)
            Me.oLan = New Collection
            Me.oVar = New Collection
            Me.InitializeComponent()
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Me.Close()
        End Sub

        Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
            Dim num2 As Integer = CInt(Math.Round(Me.txtYearFrom.Value))
            Dim num As Integer = IntegerType.FromObject(Sql.GetValue((Me.appConn), "dmstt", "nam_bd", "1=1"))
            Dim time As DateTime = DateType.FromObject(Sql.GetValue((Me.appConn), "dmdvcs", "ngay_ks", "ma_dvcs='" + Reg.GetRegistryKey("DFUnit").ToString.Trim + "'"))
            If ((num2 < num) Or (num2 <= (time.Year - 1))) Then
                Msg.Alert(StringType.FromObject(Me.oLan.Item("005")), 2)
            Else
                Dim nUserID As Integer = IntegerType.FromString(Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID"))))

                Dim tcSQL As String = ("EXEC spConvertItems2NextYear_pkh " & StringType.FromInteger(num2))
                tcSQL += "," + Sql.ConvertVS2SQLType(Reg.GetRegistryKey("DFUnit").ToString.Trim, "")
                tcSQL += "," + nUserID.ToString.Trim
                Me.pnContent.Text = StringType.FromObject(Me.oVar.Item("m_process"))
                Sql.SQLExecute((Me.appConn), tcSQL)
                Msg.Alert(StringType.FromObject(Me.oVar.Item("m_end_proc")))
                Me.Close()
            End If
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub frmkcsd_Load(ByVal sender As Object, ByVal e As EventArgs)
            If BooleanType.FromObject(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0))) Then
                ProjectData.EndApp()
            End If
            Me.sysConn = Sys.GetSysConn
            If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(Me.sysConn, "Access")) Then
                Me.sysConn.Close()
                Me.sysConn = Nothing
                ProjectData.EndApp()
            End If
            Me.appConn = Sys.GetConn
            Sys.InitMessage(Me.sysConn, Me.oLan, "CopyItemBalances")
            Sys.InitVar(Me.sysConn, Me.oVar)
            Me.Text = StringType.FromObject(Me.oLan.Item("000"))
            Dim control As Control
            For Each control In Me.Controls
                If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                    control.Text = StringType.FromObject(Me.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
                End If
            Next
            Obj.Init(Me)
            Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
            Me.txtYearFrom.MaxLength = 4
            Me.txtYearFrom.Value = DateAndTime.Now.Year
            Me.txtYearTo.Text = StringType.FromDouble((Me.txtYearFrom.Value + 1))
            'Dim sKey As String = ("status = '1' AND ma_dvcs IN (SELECT ma_dvcs FROM dbo.sysunitrights where user_id = " & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID"))) & " AND r_access = 1)")
            'Dim getcodes As New clsGetcodes.clsGetcodes(Me.txtDsdvcs, Sys.GetSysConn, Sys.GetConn, "dmdvcs", "ma_dvcs", "Unit", sKey, Me)
            'Me.txtDsdvcs.Text = ""
        End Sub

        Private Sub frmnamdn_Closed(ByVal sender As Object, ByVal e As EventArgs)
            Me.appConn.Close()
            Me.appConn = Nothing
            Me.sysConn.Close()
            Me.sysConn = Nothing
        End Sub

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblMa_dvcs = New System.Windows.Forms.Label()
            Me.cmdOk = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.grpInfor = New System.Windows.Forms.GroupBox()
            Me.txtYearTo = New System.Windows.Forms.TextBox()
            Me.txtYearFrom = New libscontrol.txtNumeric()
            Me.lblTo = New System.Windows.Forms.Label()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.txtDsdvcs = New System.Windows.Forms.TextBox()
            Me.SuspendLayout()
            '
            'lblMa_dvcs
            '
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New System.Drawing.Point(23, 25)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New System.Drawing.Size(96, 13)
            Me.lblMa_dvcs.TabIndex = 5
            Me.lblMa_dvcs.Tag = "L001"
            Me.lblMa_dvcs.Text = "Ket chuyen tu nam"
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.cmdOk.Location = New System.Drawing.Point(8, 92)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.Size = New System.Drawing.Size(75, 23)
            Me.cmdOk.TabIndex = 2
            Me.cmdOk.Tag = "L003"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(84, 92)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
            Me.cmdCancel.TabIndex = 3
            Me.cmdCancel.Tag = "L004"
            Me.cmdCancel.Text = "Huy"
            '
            'grpInfor
            '
            Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grpInfor.Location = New System.Drawing.Point(8, 8)
            Me.grpInfor.Name = "grpInfor"
            Me.grpInfor.Size = New System.Drawing.Size(592, 78)
            Me.grpInfor.TabIndex = 17
            Me.grpInfor.TabStop = False
            '
            'txtYearTo
            '
            Me.txtYearTo.Enabled = False
            Me.txtYearTo.Location = New System.Drawing.Point(252, 21)
            Me.txtYearTo.Name = "txtYearTo"
            Me.txtYearTo.Size = New System.Drawing.Size(38, 20)
            Me.txtYearTo.TabIndex = 0
            Me.txtYearTo.Text = "txtYearTo"
            '
            'txtYearFrom
            '
            Me.txtYearFrom.Format = ""
            Me.txtYearFrom.Location = New System.Drawing.Point(155, 21)
            Me.txtYearFrom.MaxLength = 1
            Me.txtYearFrom.Name = "txtYearFrom"
            Me.txtYearFrom.Size = New System.Drawing.Size(38, 20)
            Me.txtYearFrom.TabIndex = 18
            Me.txtYearFrom.Text = "0"
            Me.txtYearFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtYearFrom.Value = 0R
            '
            'lblTo
            '
            Me.lblTo.AutoSize = True
            Me.lblTo.Location = New System.Drawing.Point(208, 25)
            Me.lblTo.Name = "lblTo"
            Me.lblTo.Size = New System.Drawing.Size(27, 13)
            Me.lblTo.TabIndex = 19
            Me.lblTo.Tag = "L002"
            Me.lblTo.Text = "Den"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(23, 46)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(38, 13)
            Me.Label1.TabIndex = 20
            Me.Label1.Tag = ""
            Me.Label1.Text = "Don vi"
            Me.Label1.Visible = False
            '
            'txtDsdvcs
            '
            Me.txtDsdvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtDsdvcs.Location = New System.Drawing.Point(155, 44)
            Me.txtDsdvcs.Name = "txtDsdvcs"
            Me.txtDsdvcs.Size = New System.Drawing.Size(439, 20)
            Me.txtDsdvcs.TabIndex = 21
            Me.txtDsdvcs.Tag = "FCMaster#dbo.ff_InUnits(a.ma_dvcs, '%s') = 1 #MLEX"
            Me.txtDsdvcs.Text = "TXTDSDVCS"
            Me.txtDsdvcs.Visible = False
            '
            'frmSdkc
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(608, 149)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.txtDsdvcs)
            Me.Controls.Add(Me.lblTo)
            Me.Controls.Add(Me.txtYearFrom)
            Me.Controls.Add(Me.lblMa_dvcs)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.txtYearTo)
            Me.Controls.Add(Me.grpInfor)
            Me.Name = "frmSdkc"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmSdkc"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        <STAThread()>
        Public Shared Sub Main()
            Application.Run(New frmSdkc)
        End Sub

        Private Sub txtYearFrom_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtYearFrom.Validated
            Me.txtYearTo.Text = StringType.FromDouble((Me.txtYearFrom.Value + 1))
        End Sub


        ' Properties
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents grpInfor As GroupBox
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblTo As Label
        Friend WithEvents txtYearFrom As txtNumeric
        Friend WithEvents txtYearTo As TextBox


        Public appConn As SqlConnection
        Private components As IContainer
        Public oLan As Collection
        Public oVar As Collection
        Public pnContent As StatusBarPanel
        Public sysConn As SqlConnection
        Friend WithEvents Label1 As Label
        Friend WithEvents txtDsdvcs As TextBox
        Public Const SysID As String = "CopyItemBalances"
    End Class
End Namespace

