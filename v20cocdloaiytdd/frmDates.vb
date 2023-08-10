Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol

Namespace v20cocdloaiytdd
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
            If Not ((Me.txtTu_ky.Value >= 1) And (Me.txtTu_ky.Value < 13)) Then
                Me.txtTu_ky.Focus()
                Msg.Alert(StringType.FromObject(DirMain.oDirFormLib.oLan.Item("201")), 2)
            Else
                DirMain.nPeriod = CInt(Math.Round(Me.txtTu_ky.Value))
                DirMain.nYear = CInt(Math.Round(Me.txtYear.Value))
                DirMain.isCon = True
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
            Me.txtTu_ky.Value = DateAndTime.Now.Month
            Me.txtYear.Value = DateAndTime.Now.Year
        End Sub

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblYear = New Label
            Me.cmdOk = New Button
            Me.cmdCancel = New Button
            Me.grpInfor = New GroupBox
            Me.lblTy_ky = New Label
            Me.txtYear = New txtNumeric
            Me.txtTu_ky = New txtNumeric
            Me.SuspendLayout()
            Me.lblYear.AutoSize = True
            Me.lblYear.Location = New Point(23, 43)
            Me.lblYear.Name = "lblYear"
            Me.lblYear.Size = New Size(28, 16)
            Me.lblYear.TabIndex = 5
            Me.lblYear.Tag = "L102"
            Me.lblYear.Text = "Nam"
            Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdOk.Location = New Point(8, 80)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 2
            Me.cmdOk.Tag = "L103"
            Me.cmdOk.Text = "Nhan"
            Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdCancel.Location = New Point(84, 80)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 3
            Me.cmdCancel.Tag = "L104"
            Me.cmdCancel.Text = "Huy"
            Me.grpInfor.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.grpInfor.Location = New Point(8, 4)
            Me.grpInfor.Name = "grpInfor"
            Me.grpInfor.Size = New Size(592, 68)
            Me.grpInfor.TabIndex = 17
            Me.grpInfor.TabStop = False
            Me.lblTy_ky.AutoSize = True
            Me.lblTy_ky.Location = New Point(23, 20)
            Me.lblTy_ky.Name = "lblTy_ky"
            Me.lblTy_ky.Size = New Size(32, 16)
            Me.lblTy_ky.TabIndex = 26
            Me.lblTy_ky.Tag = "L101"
            Me.lblTy_ky.Text = "Tu ky"
            Me.txtYear.Format = "##0"
            Me.txtYear.Location = New Point(155, 41)
            Me.txtYear.MaxLength = 4
            Me.txtYear.Name = "txtYear"
            Me.txtYear.Size = New Size(30, 20)
            Me.txtYear.TabIndex = 1
            Me.txtYear.Tag = "FNNB"
            Me.txtYear.Text = "0"
            Me.txtYear.TextAlign = HorizontalAlignment.Right
            Me.txtYear.Value = 0
            Me.txtTu_ky.Format = "#0"
            Me.txtTu_ky.Location = New Point(155, 18)
            Me.txtTu_ky.MaxLength = 3
            Me.txtTu_ky.Name = "txtTu_ky"
            Me.txtTu_ky.Size = New Size(30, 20)
            Me.txtTu_ky.TabIndex = 0
            Me.txtTu_ky.Tag = "FNNB"
            Me.txtTu_ky.Text = "0"
            Me.txtTu_ky.TextAlign = HorizontalAlignment.Right
            Me.txtTu_ky.Value = 0
            Me.AutoScaleBaseSize = New Size(5, 13)
            Me.ClientSize = New Size(608, 109)
            Me.Controls.Add(Me.txtYear)
            Me.Controls.Add(Me.txtTu_ky)
            Me.Controls.Add(Me.lblTy_ky)
            Me.Controls.Add(Me.lblYear)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.grpInfor)
            Me.Name = "frmDates"
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "frmDates"
            Me.ResumeLayout(False)
        End Sub


        ' Properties
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents grpInfor As GroupBox
        Friend WithEvents lblTy_ky As Label
        Friend WithEvents lblYear As Label
        Friend WithEvents txtTu_ky As txtNumeric
        Friend WithEvents txtYear As txtNumeric
        Private components As IContainer
    End Class
End Namespace

