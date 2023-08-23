Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Windows.Forms
Imports libscontrol

Namespace inctpxa
    Public Class frmChange_Lot
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
            Me.Text = StringType.FromObject(modVoucher.oLan.Item("305"))
            Dim control As Control
            For Each control In Me.Controls
                If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                    control.Text = StringType.FromObject(modVoucher.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
                End If
            Next
            Obj.Init(Me)
        End Sub
        Friend WithEvents Label2 As Label
        Friend WithEvents txtMa_lo As TextBox
        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.cmdOk = New Button
            Me.cmdCancel = New Button
            Me.grpInfor = New System.Windows.Forms.GroupBox
            Me.Label2 = New Label
            Me.txtMa_lo = New TextBox
            Me.SuspendLayout()
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.cmdOk.Location = New System.Drawing.Point(8, 77)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 5
            Me.cmdOk.Tag = "L302"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(84, 77)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 6
            Me.cmdCancel.Tag = "L303"
            Me.cmdCancel.Text = "Huy"
            '
            'grpInfor
            '
            Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grpInfor.Location = New System.Drawing.Point(8, 8)
            Me.grpInfor.Name = "grpInfor"
            Me.grpInfor.Size = New System.Drawing.Size(592, 56)
            Me.grpInfor.TabIndex = 17
            Me.grpInfor.TabStop = False
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(23, 32)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(30, 16)
            Me.Label2.TabIndex = 112
            Me.Label2.Tag = "L034"
            Me.Label2.Text = "So lo"
            '
            'txtMa_lo
            '
            Me.txtMa_lo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_lo.Location = New System.Drawing.Point(155, 32)
            Me.txtMa_lo.Name = "txtMa_lo"
            Me.txtMa_lo.TabIndex = 2
            Me.txtMa_lo.Tag = ""
            Me.txtMa_lo.Text = ""
            '
            'frmChange_Lot
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(608, 105)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.txtMa_lo)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.grpInfor)
            Me.Name = "frmChange_Lot"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmDate"
            Me.ResumeLayout(False)

        End Sub

        ' Properties
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents grpInfor As GroupBox

        Private components As IContainer
    End Class
End Namespace

