Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon
Imports libscontrol.voucherseachlib

Namespace z21coAutopost622
    Public Class frmFilter
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
            Me.InitializeComponent()
        End Sub

        Private Sub cboReports_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboReports.SelectedIndexChanged
            If Not Information.IsNothing(Me.rpTable) Then
                Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(Me.rpTable.Rows.Item(Me.cboReports.SelectedIndex), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
            End If
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Me.Close()
        End Sub

        Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
            If ((Me.txtMonth.Value < 1) Or (Me.txtMonth.Value > 12)) Then
                Me.txtMonth.Focus()
                Msg.Alert(StringType.FromObject(DirMain.oDirFormLib.oLan.Item("111")), 2)
            Else
                nMonth = CInt(Math.Round(Me.txtMonth.Value))
                nYear = CInt(Math.Round(Me.txtYear.Value))
                DirMain.isContinue = True
                Dim document As New PrintDocument
                Me.pnContent.Text = document.PrinterSettings.PrinterName
                Me.Close()
            End If
            Reg.SetRegistryKey("DFMTo", nMonth)
            Reg.SetRegistryKey("DFMFrom", nMonth)
            Reg.SetRegistryKey("DFYear", nYear)
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
            Dim control As Control
            For Each control In Me.Controls
                If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                    control.Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
                End If
            Next
            Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
            Me.rpTable = clsprint.InitComboReport(DirMain.oDirFormLib.sysConn, Me.cboReports, DirMain.oDirFormLib.SysID)
            Dim sKey As String = ("status = '1' AND ma_dvcs IN (SELECT ma_dvcs FROM dbo.sysunitrights where user_id = " & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID"))) & " AND r_access = 1)")
            Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", sKey, True, Me.cmdCancel)
            Me.txtMa_dvcs.Text = ""
            Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(Me.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))

            Me.txtMonth.Value = Reg.GetRegistryKey("DFMTo")
            Me.txtYear.Value = Reg.GetRegistryKey("DFYear")
            'Me.txtMonth.Value = Date.Now.AddDays(-25)
            Me.Validated_Mess1()
            Obj.Init(Me)
        End Sub
        Friend WithEvents txtMonth As txtNumeric
        Friend WithEvents txtYear As txtNumeric
        Friend WithEvents Label1 As System.Windows.Forms.Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
            Me.lblMa_dvcs = New System.Windows.Forms.Label()
            Me.lblTen_dvcs = New System.Windows.Forms.Label()
            Me.cmdOk = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.lblMau_bc = New System.Windows.Forms.Label()
            Me.cboReports = New System.Windows.Forms.ComboBox()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.txtTitle = New System.Windows.Forms.TextBox()
            Me.grpInfor = New System.Windows.Forms.GroupBox()
            Me.lblTu_kyMess = New System.Windows.Forms.Label()
            Me.txtMonth = New libscontrol.txtNumeric()
            Me.lblTy_ky = New System.Windows.Forms.Label()
            Me.txtYear = New libscontrol.txtNumeric()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'txtMa_dvcs
            '
            Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New System.Drawing.Point(186, 83)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.Size = New System.Drawing.Size(125, 22)
            Me.txtMa_dvcs.TabIndex = 4
            Me.txtMa_dvcs.Tag = "FCMaster#dbo.ff_InUnits(a.ma_dvcs, '%s') = 1 #MLEX"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            '
            'lblMa_dvcs
            '
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New System.Drawing.Point(19, 85)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New System.Drawing.Size(48, 17)
            Me.lblMa_dvcs.TabIndex = 1
            Me.lblMa_dvcs.Tag = "L108"
            Me.lblMa_dvcs.Text = "Don vi"
            '
            'lblTen_dvcs
            '
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New System.Drawing.Point(311, 85)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New System.Drawing.Size(66, 17)
            Me.lblTen_dvcs.TabIndex = 7
            Me.lblTen_dvcs.Tag = ""
            Me.lblTen_dvcs.Text = "Ten dvcs"
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.Location = New System.Drawing.Point(10, 181)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.Size = New System.Drawing.Size(90, 27)
            Me.cmdOk.TabIndex = 7
            Me.cmdOk.Tag = "L004"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(101, 181)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(90, 27)
            Me.cmdCancel.TabIndex = 8
            Me.cmdCancel.Tag = "L005"
            Me.cmdCancel.Text = "Huy"
            '
            'lblMau_bc
            '
            Me.lblMau_bc.AutoSize = True
            Me.lblMau_bc.Location = New System.Drawing.Point(19, 113)
            Me.lblMau_bc.Name = "lblMau_bc"
            Me.lblMau_bc.Size = New System.Drawing.Size(90, 17)
            Me.lblMau_bc.TabIndex = 2
            Me.lblMau_bc.Tag = "L109"
            Me.lblMau_bc.Text = "Mau bao cao"
            '
            'cboReports
            '
            Me.cboReports.Location = New System.Drawing.Point(186, 111)
            Me.cboReports.Name = "cboReports"
            Me.cboReports.Size = New System.Drawing.Size(437, 24)
            Me.cboReports.TabIndex = 5
            Me.cboReports.Text = "cboReports"
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New System.Drawing.Point(19, 141)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(56, 17)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Tag = "L110"
            Me.lblTitle.Text = "Tieu de"
            '
            'txtTitle
            '
            Me.txtTitle.Location = New System.Drawing.Point(186, 138)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New System.Drawing.Size(437, 22)
            Me.txtTitle.TabIndex = 6
            Me.txtTitle.Tag = "NB"
            Me.txtTitle.Text = "txtTieu_de"
            '
            'grpInfor
            '
            Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grpInfor.Location = New System.Drawing.Point(10, 9)
            Me.grpInfor.Name = "grpInfor"
            Me.grpInfor.Size = New System.Drawing.Size(588, 157)
            Me.grpInfor.TabIndex = 0
            Me.grpInfor.TabStop = False
            Me.grpInfor.Tag = ""
            '
            'lblTu_kyMess
            '
            Me.lblTu_kyMess.AutoSize = True
            Me.lblTu_kyMess.Location = New System.Drawing.Point(290, 30)
            Me.lblTu_kyMess.Name = "lblTu_kyMess"
            Me.lblTu_kyMess.Size = New System.Drawing.Size(80, 17)
            Me.lblTu_kyMess.TabIndex = 51
            Me.lblTu_kyMess.Tag = ""
            Me.lblTu_kyMess.Text = "01/01/1990"
            '
            'txtMonth
            '
            Me.txtMonth.Format = "#0"
            Me.txtMonth.Location = New System.Drawing.Point(186, 28)
            Me.txtMonth.MaxLength = 2
            Me.txtMonth.Name = "txtMonth"
            Me.txtMonth.Size = New System.Drawing.Size(36, 22)
            Me.txtMonth.TabIndex = 0
            Me.txtMonth.Tag = "FNNB"
            Me.txtMonth.Text = "0"
            Me.txtMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtMonth.Value = 0R
            '
            'lblTy_ky
            '
            Me.lblTy_ky.AutoSize = True
            Me.lblTy_ky.Location = New System.Drawing.Point(19, 30)
            Me.lblTy_ky.Name = "lblTy_ky"
            Me.lblTy_ky.Size = New System.Drawing.Size(24, 17)
            Me.lblTy_ky.TabIndex = 49
            Me.lblTy_ky.Tag = "L001"
            Me.lblTy_ky.Text = "Ky"
            '
            'txtYear
            '
            Me.txtYear.Format = "###0"
            Me.txtYear.Location = New System.Drawing.Point(186, 55)
            Me.txtYear.MaxLength = 5
            Me.txtYear.Name = "txtYear"
            Me.txtYear.Size = New System.Drawing.Size(58, 22)
            Me.txtYear.TabIndex = 52
            Me.txtYear.Tag = "FNNB"
            Me.txtYear.Text = "0"
            Me.txtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtYear.Value = 0R
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(19, 58)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(37, 17)
            Me.Label1.TabIndex = 53
            Me.Label1.Tag = "L003"
            Me.Label1.Text = "Nam"
            '
            'frmFilter
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
            Me.ClientSize = New System.Drawing.Size(608, 241)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.txtYear)
            Me.Controls.Add(Me.lblTitle)
            Me.Controls.Add(Me.lblTen_dvcs)
            Me.Controls.Add(Me.lblMa_dvcs)
            Me.Controls.Add(Me.txtTitle)
            Me.Controls.Add(Me.txtMa_dvcs)
            Me.Controls.Add(Me.lblMau_bc)
            Me.Controls.Add(Me.lblTu_kyMess)
            Me.Controls.Add(Me.txtMonth)
            Me.Controls.Add(Me.lblTy_ky)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.cboReports)
            Me.Controls.Add(Me.grpInfor)
            Me.Name = "frmFilter"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmFilter"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private Sub txtMonth1_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMonth.Validated
            Me.Validated_Mess1()
        End Sub



        Private Sub txtNam1_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Me.Validated_Mess1()
        End Sub

        Private Sub Validated_Mess1()
            If ((Me.txtMonth.Value < 1) Or (Me.txtMonth.Value > 12)) Then
                Me.txtMonth.Focus()
            ElseIf ((Me.txtYear.Value < 1900) Or (Me.txtYear.Value > 3000)) Then
                Me.txtYear.Focus()
            Else
                Me.dDate = startdate.GetStartDateOfYear(DirMain.oDirFormLib.appConn, CInt(Math.Round(Me.txtYear.Value)))
                Me.lblTu_kyMess.Text = StringType.FromDate(Me.dDate.AddMonths(CInt(Math.Round(CDbl((Me.txtMonth.Value - 1))))).Date)
            End If
        End Sub

        ' Properties
        Friend WithEvents cboReports As ComboBox
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents grpInfor As GroupBox
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMau_bc As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTitle As Label
        Friend WithEvents lblTu_kyMess As Label
        Friend WithEvents lblTy_ky As Label
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtTitle As TextBox


        Private components As IContainer
        Private dDate As DateTime
        Public pnContent As StatusBarPanel
        Public rpTable As DataTable
    End Class
End Namespace

