Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon

Namespace z21coAutopost622
    Public Class frmDates
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmnamdn_Load)
            Me.InitializeComponent()
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            DirMain.isCon = False
            Me.Close()
        End Sub

        Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
            DirMain.isCon = False
            Dim startDateOfYear As DateTime = startdate.GetStartDateOfYear(DirMain.oDirFormLib.appConn, CInt(Math.Round(Me.txtYear.Value)))
            If (Me.txtMonth.Value < 1) Or (Me.txtMonth.Value > 12) Then
                Me.txtMonth.Focus()
                Msg.Alert(StringType.FromObject(DirMain.oDirFormLib.oLan.Item("201")), 2)
            ElseIf (DateTime.Compare(startDateOfYear.AddMonths(CInt(Math.Round(CDbl((Me.txtMonth.Value - 1))))), DirMain.dLocked) <= 0) Then
                Me.txtMonth.Focus()
                Msg.Alert(StringType.FromObject(DirMain.oDirFormLib.oLan.Item("200")), 2)
            Else
                DirMain.nMonth = CInt(Math.Round(Me.txtMonth.Value))
                DirMain.nYear = CInt(Math.Round(Me.txtYear.Value))
                DirMain.isCon = True
                DirMain.dsDvcs = Me.txtDsDvcs.Text
                Me.Close()
            End If
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub frmnamdn_Load(ByVal sender As Object, ByVal e As EventArgs)
            Obj.Init(Me)
            Dim control As Control
            For Each control In Me.Controls
                If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                    control.Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
                End If
                If (Strings.InStr(StringType.FromObject(control.Tag), "ML", CompareMethod.Binary) > 0) Then
                    Dim obj2 As Object = Strings.Right(control.Name, (control.Name.Length - 3))
                    Dim box As TextBox = DirectCast(control, TextBox)
                    box.MaxLength = IntegerType.FromObject(DirMain.oDirFormLib.oLen.Item(RuntimeHelpers.GetObjectValue(obj2)))
                End If
            Next
            Dim sKey As String = ("status = '1' AND ma_dvcs IN (SELECT ma_dvcs FROM dbo.sysunitrights where user_id = " & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID"))) & " AND r_access = 1)")
            Dim getcodes As New clsGetcodes.clsGetcodes(Me.txtDsDvcs, Sys.GetSysConn, Sys.GetConn, "dmdvcs", "ma_dvcs", "Unit", sKey, Me)
            Me.txtMonth.Value = Date.Now.AddDays(-25).Month
            Me.txtYear.Value = Date.Now.AddDays(-25).Year
        End Sub
        Friend WithEvents txtMonth As txtNumeric

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblYear = New System.Windows.Forms.Label()
            Me.cmdOk = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.grpInfor = New System.Windows.Forms.GroupBox()
            Me.lblTy_ky = New System.Windows.Forms.Label()
            Me.txtYear = New libscontrol.txtNumeric()
            Me.txtMonth = New libscontrol.txtNumeric()
            Me.txtDsDvcs = New System.Windows.Forms.TextBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'lblYear
            '
            Me.lblYear.AutoSize = True
            Me.lblYear.Location = New System.Drawing.Point(28, 55)
            Me.lblYear.Name = "lblYear"
            Me.lblYear.Size = New System.Drawing.Size(37, 17)
            Me.lblYear.TabIndex = 5
            Me.lblYear.Tag = "L003"
            Me.lblYear.Text = "Nam"
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.Location = New System.Drawing.Point(10, 135)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.Size = New System.Drawing.Size(90, 27)
            Me.cmdOk.TabIndex = 4
            Me.cmdOk.Tag = "L004"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.Location = New System.Drawing.Point(101, 135)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(90, 27)
            Me.cmdCancel.TabIndex = 5
            Me.cmdCancel.Tag = "L005"
            Me.cmdCancel.Text = "Huy"
            '
            'grpInfor
            '
            Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grpInfor.Location = New System.Drawing.Point(10, 9)
            Me.grpInfor.Name = "grpInfor"
            Me.grpInfor.Size = New System.Drawing.Size(644, 113)
            Me.grpInfor.TabIndex = 17
            Me.grpInfor.TabStop = False
            '
            'lblTy_ky
            '
            Me.lblTy_ky.AutoSize = True
            Me.lblTy_ky.Location = New System.Drawing.Point(28, 30)
            Me.lblTy_ky.Name = "lblTy_ky"
            Me.lblTy_ky.Size = New System.Drawing.Size(49, 17)
            Me.lblTy_ky.TabIndex = 26
            Me.lblTy_ky.Tag = "L001"
            Me.lblTy_ky.Text = "Thang"
            '
            'txtYear
            '
            Me.txtYear.Format = "###0"
            Me.txtYear.Location = New System.Drawing.Point(186, 55)
            Me.txtYear.MaxLength = 5
            Me.txtYear.Name = "txtYear"
            Me.txtYear.Size = New System.Drawing.Size(58, 22)
            Me.txtYear.TabIndex = 2
            Me.txtYear.Tag = "FNNB"
            Me.txtYear.Text = "0"
            Me.txtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtYear.Value = 0R
            '
            'txtMonth
            '
            Me.txtMonth.Format = "#0"
            Me.txtMonth.Location = New System.Drawing.Point(186, 28)
            Me.txtMonth.MaxLength = 3
            Me.txtMonth.Name = "txtMonth"
            Me.txtMonth.Size = New System.Drawing.Size(58, 22)
            Me.txtMonth.TabIndex = 0
            Me.txtMonth.Tag = "FNNB"
            Me.txtMonth.Text = "0"
            Me.txtMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtMonth.Value = 0R
            '
            'txtDsDvcs
            '
            Me.txtDsDvcs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtDsDvcs.Location = New System.Drawing.Point(186, 83)
            Me.txtDsDvcs.Name = "txtDsDvcs"
            Me.txtDsDvcs.Size = New System.Drawing.Size(437, 22)
            Me.txtDsDvcs.TabIndex = 3
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(28, 83)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(109, 17)
            Me.Label1.TabIndex = 27
            Me.Label1.Tag = "L008"
            Me.Label1.Text = "Danh sach dvcs"
            '
            'frmDates
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
            Me.ClientSize = New System.Drawing.Size(664, 170)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.txtYear)
            Me.Controls.Add(Me.txtMonth)
            Me.Controls.Add(Me.lblTy_ky)
            Me.Controls.Add(Me.lblYear)
            Me.Controls.Add(Me.txtDsDvcs)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.grpInfor)
            Me.Name = "frmDates"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmDates"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub


        ' Properties
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents grpInfor As GroupBox
        Friend WithEvents Label1 As Label
        Friend WithEvents lblTy_ky As Label
        Friend WithEvents lblYear As Label
        Friend WithEvents txtDsDvcs As TextBox
        Friend WithEvents txtYear As txtNumeric

        Private components As IContainer
    End Class
End Namespace

