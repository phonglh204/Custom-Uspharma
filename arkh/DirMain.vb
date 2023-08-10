Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports libscontrol.dirformlib
Imports libscontrol
Imports Microsoft.VisualBasic
Imports libscommon
Imports System.Windows.Forms
Imports System.Drawing

Module DirMain
    ' Methods
    <STAThread()>
    Public Sub main()
        oDirFormLib = New DirFormLibBrowse
        oDirFormLib.SysID = "Customer"
        oDirFormLib.Init()
        oDirFormLib.oDir.strEnabled = "111111111"
        If (oDirFormLib.sysConn Is Nothing) Then
            ProjectData.EndApp()
        End If
        oDirFormLib.lUniKey = False
        oDirFormLib.oDir.ebutHand = New ToolBarButtonClickEventHandler(AddressOf DirMain.tbrClick)
        oDirFormLib.oDir.emnuHand = New EventHandler(AddressOf DirMain.mnuclick)
        AddHandler DirMain.oDirFormLib.oDir.ob.frmLookup.Load, New EventHandler(AddressOf DirMain.DirLoad)
        oDirFormLib.frmUpdate = New frmDirInfor
        oDirFormLib.Show()
        oDirFormLib.Close()
    End Sub
    Public Sub DirLoad()
        DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(6).Text = Conversions.ToString(DirMain.oDirFormLib.oLan.Item("045"))
        DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(6).Shortcut = Shortcut.None
        DirMain.oDirFormLib.oDir.tbr.Buttons.Item(6).Style = ToolBarButtonStyle.PushButton
        DirMain.oDirFormLib.oDir.tbr.Buttons.Item(6).ToolTipText = Conversions.ToString(DirMain.oDirFormLib.oLan.Item("045"))
        DirMain.oDirFormLib.oDir.tbr.ImageList.Images.Item(6) = Image.FromFile(Conversions.ToString(Operators.AddObject(Reg.GetRegistryKey("ImageDir"), "Calc.png")))
        DirMain.oDirFormLib.oDir.ob.grdLookup.ContextMenu.MenuItems.Item(6).Text = Conversions.ToString(DirMain.oDirFormLib.oLan.Item("045"))
        DirMain.oDirFormLib.oDir.ob.grdLookup.ContextMenu.MenuItems.Item(6).Shortcut = Shortcut.None
    End Sub
    Public Sub mnuclick(ByVal sender As Object, ByVal e As EventArgs)
        Select Case ByteType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing))
            Case 0
                oDirFormLib.dirAddNew()
                Exit Select
            Case 1
                oDirFormLib.dirEdit()
                Exit Select
            Case 2
                DirMain.Delete()
                Exit Select
            Case 4
                DirMain.ChangeCode()
                Exit Select
            Case 6
                EIUpdateCus()
                Exit Select
            Case 8
                oDirFormLib.oDir.ob.frmLookup.Close()
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
                DirMain.Delete()
                Exit Select
            Case 4
                DirMain.ChangeCode()
                Exit Select
            Case 6
                EIUpdateCus()
                Exit Select
            Case 8
                oDirFormLib.oDir.ob.frmLookup.Close()
                Exit Select
        End Select
    End Sub
    Private Sub ChangeCode()
        Dim str As String = Nothing
        If (Not oDirFormLib.oDir.ob.CurDataRow Is Nothing) Then
            str = StringType.FromObject(oDirFormLib.oDir.ob.CurDataRow.Item("ma_kh"))
        End If
        If (IntegerType.FromObject(Sql.GetValue((oDirFormLib.appConn), StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("if exists(select 1 from khhddt where ma_kh = ", Sql.ConvertVS2SQLType(str.Trim, "")), ") select 1 as xvalue else select 0 as xvalue")))) <> 0) Then
            Msg.Alert(Strings.Replace(StringType.FromObject(oDirFormLib.oLan.Item("040")), "%s", Strings.Trim(str), 1, -1, CompareMethod.Binary), 2)
        Else
            oDirFormLib.dirChangeCode()
        End If
    End Sub

    Private Sub Delete()
        Dim str As String = Nothing
        If (Not oDirFormLib.oDir.ob.CurDataRow Is Nothing) Then
            str = StringType.FromObject(oDirFormLib.oDir.ob.CurDataRow.Item("ma_kh"))
        End If
        If (IntegerType.FromObject(Sql.GetValue((oDirFormLib.appConn), StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("if exists(select 1 from khhddt where ma_kh = ", Sql.ConvertVS2SQLType(str.Trim, "")), ") select 1 as xvalue else select 0 as xvalue")))) <> 0) Then
            Msg.Alert(Strings.Replace(StringType.FromObject(oDirFormLib.oLan.Item("043")), "%s", Strings.Trim(str), 1, -1, CompareMethod.Binary), 1)
        Else
            oDirFormLib.dirDelete()
        End If
    End Sub
    Private Sub EIUpdateCus()
        Dim str As String = Nothing, e_mail As String
        If (Not oDirFormLib.oDir.ob.CurDataRow Is Nothing) Then
            str = StringType.FromObject(oDirFormLib.oDir.ob.CurDataRow.Item("ma_kh"))
            e_mail = oDirFormLib.oDir.ob.CurDataRow.Item("e_mail").ToString.Trim
            If e_mail = "" Then
                Msg.Alert("Email is empty!", 2)
                Return
            End If
            Dim Collection As New Collection
            Collection.Add(str, "customerList", Nothing, Nothing)
            Collection.Add(Reg.GetRegistryKey("DFUnit"), "unit", Nothing, Nothing)
            Dim message As connect_vnpt.Message = DirectCast(connect_vnpt.Client.ExcuteCommand(DirMain.oDirFormLib.sysConn, (DirMain.oDirFormLib.appConn), "200", Nothing, Collection), connect_vnpt.Message)
            If (message.Success = "1") Then
                Msg.Alert(Conversions.ToString(DirMain.oDirFormLib.oLan.Item("900")), 2)
            Else
                Msg.Alert(Conversions.ToString(Operators.AddObject(Operators.AddObject(DirMain.oDirFormLib.oLan.Item("901"), ChrW(13) & ChrW(10)), String.Format(message.Message, message.reference))), 2)
            End If
        End If
    End Sub

    ' Fields
    Public oDirFormLib As DirFormLibBrowse
End Module

