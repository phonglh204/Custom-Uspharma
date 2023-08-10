Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol.balanceviewlib
Imports libscommon

Namespace z22dmgiacong
    Module DirMain
        ' Methods
        Private Sub DirLoad(ByVal sender As Object, ByVal e As EventArgs)
            oDirFormLib.oDir.tbr.Buttons.Item(4).ToolTipText = StringType.FromObject(oDirFormLib.oLan.Item("200"))
            oDirFormLib.oDir.tbr.Buttons.Item(4).Style = ToolBarButtonStyle.PushButton
            oDirFormLib.oDir.tbr.ImageList.Images.Item(4) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "export.bmp")))
            DirMain.oDirFormLib.oDir.ob.grdLookup.ContextMenu.MenuItems.Item(4).Text = StringType.FromObject(oDirFormLib.oLan.Item("200"))
            DirMain.oDirFormLib.oDir.ob.grdLookup.ContextMenu.MenuItems.Item(4).Shortcut = Shortcut.CtrlF4
            oDirFormLib.oDir.mnFile.MenuItems.Item(4).Text = StringType.FromObject(oDirFormLib.oLan.Item("200"))
            oDirFormLib.oDir.mnFile.MenuItems.Item(4).Shortcut = Shortcut.CtrlF4
        End Sub

        Public Sub grd_CurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim args As Object() = New Object() {DirMain.nOldRow}
            Dim copyBack As Boolean() = New Boolean() {True}
            LateBinding.LateCall(sender, Nothing, "UnSelect", args, Nothing, copyBack)
            If copyBack(0) Then
                DirMain.nOldRow = IntegerType.FromObject(args(0))
            End If
            args = New Object(1 - 1) {}
            Dim o As Object = sender
            args(0) = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(o, Nothing, "CurrentRowIndex", New Object(0 - 1) {}, Nothing, Nothing))
            Dim objArray3 As Object() = args
            copyBack = New Boolean() {True}
            LateBinding.LateCall(sender, Nothing, "Select", objArray3, Nothing, copyBack)
            If copyBack(0) Then
                LateBinding.LateSetComplex(o, Nothing, "CurrentRowIndex", New Object() {RuntimeHelpers.GetObjectValue(objArray3(0))}, Nothing, True, False)
            End If
            DirMain.nOldRow = IntegerType.FromObject(LateBinding.LateGet(sender, Nothing, "CurrentRowIndex", New Object(0 - 1) {}, Nothing, Nothing))
        End Sub

        <STAThread()>
        Public Sub main()
            oDirFormLib = New DirFormLibBrowse
            'Dim oRpt As New rpDir
            If (oDirFormLib.sysConn Is Nothing) Then
                ProjectData.EndApp()
            End If
            oDirFormLib.SysID = DirMain.sysID
            oDirFormLib.Init()
            oDirFormLib.oDir.strEnabled = "111111111"
            AddHandler DirMain.oDirFormLib.oDir.ob.frmLookup.Load, New EventHandler(AddressOf DirMain.DirLoad)
            oDirFormLib.oDir.ebutHand = New ToolBarButtonClickEventHandler(AddressOf DirMain.tbrClick)
            oDirFormLib.oDir.emnuHand = New EventHandler(AddressOf DirMain.mnuClick)
            oDirFormLib.frmUpdate = New frmDirInfor
            'reports.SetReportVar(oDirFormLib.sysConn, oDirFormLib.appConn, oDirFormLib.SysID, oDirFormLib.oOptions, oRpt)
            'oDirFormLib.oRpt = oRpt
            oDirFormLib.Show()
            oDirFormLib.Close()
        End Sub

        Public Sub mnuClick(ByVal sender As Object, ByVal e As EventArgs)
            Select Case ByteType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing))
                Case 0
                    oDirFormLib.dirAddNew()
                    Exit Select
                Case 1
                    oDirFormLib.dirEdit()
                    Exit Select
                Case 2
                    oDirFormLib.dirDelete()
                    Exit Select
                Case 4
                    Dim frm As New frmAutoGen
                    frm.ShowDialog()
                    Exit Select
                Case 6
                    oDirFormLib.dirPrint()
                    Exit Select
                Case 8
                    DirMain.oDirFormLib.oDir.ob.frmLookup.Close()
                    Exit Select
            End Select
        End Sub

        Public Sub tbrClick(ByVal sender As Object, ByVal e As ToolBarButtonClickEventArgs)
            Dim objArray2 As Object() = New Object(1 - 1) {}
            Dim args As ToolBarButtonClickEventArgs = e
            objArray2(0) = args.Button
            Dim objArray As Object() = objArray2
            Dim copyBack As Boolean() = New Boolean() {True}
            If copyBack(0) Then
                args.Button = DirectCast(objArray(0), ToolBarButton)
            End If
            Select Case ByteType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "Buttons", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "IndexOf", objArray, Nothing, copyBack))
                Case 0
                    oDirFormLib.dirAddNew()
                    Exit Select
                Case 1
                    oDirFormLib.dirEdit()
                    Exit Select
                Case 2
                    oDirFormLib.dirDelete()
                    Exit Select
                Case 4
                    Dim frm As New frmAutoGen
                    frm.ShowDialog()
                    Exit Select
                Case 6
                    oDirFormLib.dirPrint()
                    Exit Select
                Case 8
                    DirMain.oDirFormLib.oDir.ob.frmLookup.Close()
                    Exit Select
            End Select
        End Sub


        ' Fields
        Public nOldRow As Integer = 0
        Public oDirFormLib As DirFormLibBrowse
        Public strKeyField As String
        Public sysID As String = "z22dmgiacong"
        Public tblFilter As DataView = New DataView
    End Module
End Namespace

