Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports libscontrol

Public Class frmDateEInv
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
        Me.Close()
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmDate_Load(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Me.Text = StringType.FromObject(modVoucher.oLan.Item("300"))
        Dim control As Control
        For Each control In Me.Controls
            If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                control.Text = StringType.FromObject(modVoucher.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
            End If
        Next
        Obj.Init(Me)
        Dim oVoucherBook As New dirkeylib(Me.txtMa_nk, Me.lblTen_nk, sysConn, appConn, "vdmnkhddt", "code", "name", "EIVoucherBook", "1 = 1", True, Me.cmdCancel)
        oVoucherBook.Key = "1=1"
        Me.txtNoFrom.Value = 2686
        Me.txtNoTo.Value = 2686
        Me.txtMa_nk.Text = "01GTKT0/001 :UP/20E"
    End Sub

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.grpInfor = New System.Windows.Forms.GroupBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtMa_nk = New System.Windows.Forms.TextBox()
        Me.lblTen_nk = New System.Windows.Forms.Label()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.txtNoFrom = New libscontrol.txtNumeric()
        Me.txtNoTo = New libscontrol.txtNumeric()
        Me.SuspendLayout()
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdOk.Location = New System.Drawing.Point(10, 105)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(90, 26)
        Me.cmdOk.TabIndex = 3
        Me.cmdOk.Tag = "L302"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(101, 105)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(90, 26)
        Me.cmdCancel.TabIndex = 4
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
        Me.grpInfor.Size = New System.Drawing.Size(745, 90)
        Me.grpInfor.TabIndex = 17
        Me.grpInfor.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(28, 34)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(70, 17)
        Me.Label15.TabIndex = 18
        Me.Label15.Tag = ""
        Me.Label15.Text = "Ma quyen"
        '
        'txtMa_nk
        '
        Me.txtMa_nk.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_nk.Location = New System.Drawing.Point(201, 30)
        Me.txtMa_nk.Name = "txtMa_nk"
        Me.txtMa_nk.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_nk.TabIndex = 0
        Me.txtMa_nk.Tag = ""
        Me.txtMa_nk.Text = "TXTMA_NK"
        '
        'lblTen_nk
        '
        Me.lblTen_nk.AutoSize = True
        Me.lblTen_nk.Location = New System.Drawing.Point(326, 34)
        Me.lblTen_nk.Name = "lblTen_nk"
        Me.lblTen_nk.Size = New System.Drawing.Size(52, 17)
        Me.lblTen_nk.TabIndex = 19
        Me.lblTen_nk.Tag = ""
        Me.lblTen_nk.Text = "Ten nk"
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(28, 60)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(131, 17)
        Me.lblTotal.TabIndex = 62
        Me.lblTotal.Tag = ""
        Me.lblTotal.Text = "Invoice No From/To"
        '
        'txtNoFrom
        '
        Me.txtNoFrom.Format = "### ### ###"
        Me.txtNoFrom.Location = New System.Drawing.Point(201, 57)
        Me.txtNoFrom.MaxLength = 12
        Me.txtNoFrom.Name = "txtNoFrom"
        Me.txtNoFrom.Size = New System.Drawing.Size(120, 22)
        Me.txtNoFrom.TabIndex = 1
        Me.txtNoFrom.Tag = ""
        Me.txtNoFrom.Text = "  "
        Me.txtNoFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNoFrom.Value = 0R
        '
        'txtNoTo
        '
        Me.txtNoTo.Format = "### ### ###"
        Me.txtNoTo.Location = New System.Drawing.Point(327, 57)
        Me.txtNoTo.MaxLength = 12
        Me.txtNoTo.Name = "txtNoTo"
        Me.txtNoTo.Size = New System.Drawing.Size(120, 22)
        Me.txtNoTo.TabIndex = 2
        Me.txtNoTo.Tag = ""
        Me.txtNoTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNoTo.Value = 0R
        '
        'frmDateEInv
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(765, 137)
        Me.Controls.Add(Me.txtNoTo)
        Me.Controls.Add(Me.txtNoFrom)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.txtMa_nk)
        Me.Controls.Add(Me.lblTen_nk)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.grpInfor)
        Me.Name = "frmDateEInv"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmDate"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub


    ' Properties
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents Label15 As Label
    Friend WithEvents txtMa_nk As TextBox
    Friend WithEvents lblTen_nk As Label
    Friend WithEvents lblTotal As Label
    Friend WithEvents txtNoFrom As txtNumeric
    Friend WithEvents txtNoTo As txtNumeric
    Private components As IContainer

End Class

