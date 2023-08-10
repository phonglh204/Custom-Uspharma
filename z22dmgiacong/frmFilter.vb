Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports libscontrol

Namespace z22dmgiacong
    Public Class frmFilter
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmFilter_Load)
            Me.InitializeComponent()
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Me.Close()
        End Sub

        Private Sub cmdOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub frmFilter_Load(ByVal sender As Object, ByVal e As EventArgs)
            Obj.Init(Me)
            Me.Location = DirMain.oDirFormLib.oDir.ob.frmLookup.Location
            Me.Size = DirMain.oDirFormLib.oDir.ob.frmLookup.Size
            Me.Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("200"))
            Me.cmdOK.Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("094"))
            Me.cmdCancel.Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("095"))
            Dim tbs As New DataGridTableStyle
            Dim num As Integer = 30
            Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn((num + 1) - 1) {}
            Dim num5 As Integer = (num - 1)
            Dim i As Integer = 0
            Do While (i <= num5)
                cols(i) = New DataGridTextBoxColumn
                i += 1
            Loop
            Fill2Grid.Fill(DirMain.oDirFormLib.sysConn, (DirMain.tblFilter), grdFilter, (tbs), (cols), "v20crpdmtgsp")
            Dim num4 As Integer = (num - 1)
            Dim j As Integer = 1
            Do While (j <= num4)
                cols(j).ReadOnly = True
                j += 1
            Loop
            cols(0).ReadOnly = False
            DirMain.tblFilter.AllowNew = False
            DirMain.tblFilter.AllowDelete = False
            tbs.SelectionBackColor = Color.Yellow
            tbs.SelectionForeColor = Color.Black
            AddHandler Me.grdFilter.CurrentCellChanged, New EventHandler(AddressOf DirMain.grd_CurrentCellChanged)
            Me.InitMenu()
        End Sub

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.grdFilter = New clsgrid
            Me.cmdOK = New Button
            Me.cmdCancel = New Button
            Me.grdFilter.BeginInit()
            Me.SuspendLayout()
            Me.grdFilter.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.grdFilter.BackgroundColor = Color.White
            Me.grdFilter.CaptionBackColor = SystemColors.Control
            Me.grdFilter.CaptionFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, 0)
            Me.grdFilter.CaptionForeColor = Color.Black
            Me.grdFilter.CaptionText = "F4 - Them, F8 - Xoa"
            Me.grdFilter.CaptionVisible = False
            Me.grdFilter.DataMember = ""
            Me.grdFilter.HeaderForeColor = SystemColors.ControlText
            Me.grdFilter.Location = New Point(0, 0)
            Me.grdFilter.Name = "grdFilter"
            Me.grdFilter.Size = New Size(&H282, 440)
            Me.grdFilter.TabIndex = 0
            Me.grdFilter.Tag = ""
            Me.cmdOK.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdOK.Location = New Point(2, &H1BD)
            Me.cmdOK.Name = "cmdOK"
            Me.cmdOK.TabIndex = 1
            Me.cmdOK.Tag = "L094"
            Me.cmdOK.Text = "Nhan"
            Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdCancel.Location = New Point(&H4D, &H1BD)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 2
            Me.cmdCancel.Tag = "L095"
            Me.cmdCancel.Text = "Huy"
            Me.AutoScaleBaseSize = New Size(5, 13)
            Me.ClientSize = New Size(&H282, &H1D9)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOK)
            Me.Controls.Add(Me.grdFilter)
            Me.Name = "frmFilter"
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "frmFilter"
            Me.grdFilter.EndInit()
            Me.ResumeLayout(False)
        End Sub




        Private Sub InitMenu()
            Dim menu As New ContextMenu
            Dim item As New MenuItem(StringType.FromObject(DirMain.oDirFormLib.oLan.Item("603")), New EventHandler(AddressOf Me.mnuClick), Shortcut.CtrlA)
            Dim item2 As New MenuItem(StringType.FromObject(DirMain.oDirFormLib.oLan.Item("604")), New EventHandler(AddressOf Me.mnuClick), Shortcut.CtrlU)
            menu.MenuItems.AddRange(New MenuItem() {item, item2})
            Me.grdFilter.ContextMenu = menu
        End Sub

        Public Sub mnuClick(ByVal sender As Object, ByVal e As EventArgs)
            Select Case ByteType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing))
                Case 0
                    Me.SelectAll()
                    Exit Select
                Case 1
                    Me.UnSelectAll()
                    Exit Select
            End Select
        End Sub

        Private Sub SelectAll()
            Dim num2 As Integer = (DirMain.tblFilter.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                DirMain.tblFilter.Item(i).Item("flag") = True
                i += 1
            Loop
        End Sub

        Private Sub UnSelectAll()
            Dim num2 As Integer = (DirMain.tblFilter.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                DirMain.tblFilter.Item(i).Item("flag") = False
                i += 1
            Loop
        End Sub


        ' Properties
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOK As Button
        Friend WithEvents grdFilter As clsgrid

        Private components As IContainer
    End Class
End Namespace

