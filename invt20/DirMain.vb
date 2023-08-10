Imports System.Windows.Forms
Imports libscontrol.dirformlib
Imports Microsoft.VisualBasic.CompilerServices
Imports libscommon
Imports System
Imports System.Drawing
Imports libscontrol
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Net.Http
Imports ConnectWebAPI_Pharmacy

Module DirMain
    ' Methods
    Public Sub main(ByVal CmdArgs As String())
        oDirFormLib = New DirFormLibBrowse
        If CmdArgs(0).Split("#").Length = 0 Then
            SysID = "Item"
            ItemType = "*"
        Else
            SysID = CmdArgs(0).Split("#")(0)
            ItemType = CmdArgs(0).Split("#")(1)
        End If
        BranchCode = Reg.GetRegistryKey("DFUnit").ToString.Trim
        oDirFormLib.SysID = SysID
        oDirFormLib.Init()
        oDirFormLib.oDir.strEnabled = "111111111"
        If (oDirFormLib.sysConn Is Nothing) Then
            ProjectData.EndApp()
        End If
        Control.CheckForIllegalCrossThreadCalls = False
        oDirFormLib.lUniKey = Not CBool(oDirFormLib.oOptions.Item("m_long_ma"))
        Dim infor As New frmDirInfor
        oDirFormLib.frmUpdate = infor
        oDirFormLib.oTab = infor.tabInfor
        oDirFormLib.oDir.ebutHand = New ToolBarButtonClickEventHandler(AddressOf DirMain.tbrClick)
        oDirFormLib.oDir.emnuHand = New EventHandler(AddressOf DirMain.mnuclick)
        AddHandler DirMain.oDirFormLib.oDir.ob.frmLookup.Load, New EventHandler(AddressOf DirMain.DirLoad)

        oDirFormLib.Show()
        oDirFormLib.Close()
    End Sub
    Private Sub DirLoad(ByVal sender As Object, ByVal e As EventArgs)
        Dim _filter As String = "BranchCode='" + BranchCode + "'"
        If SysID = "Item_Pharmacy" Then
            DirMain.oDirFormLib.oDir.ob.dv.RowFilter += _filter
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(6).Text = "Đồng bộ thuốc lên cục quản lý dược"
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(6).Shortcut = Shortcut.None
            DirMain.oDirFormLib.oDir.tbr.Buttons.Item(6).Style = ToolBarButtonStyle.PushButton
            DirMain.oDirFormLib.oDir.tbr.Buttons.Item(6).ToolTipText = "Đồng bộ thuốc lên cục quản lý dược"
            DirMain.oDirFormLib.oDir.tbr.ImageList.Images.Item(6) = Image.FromFile(Conversions.ToString(Operators.AddObject(Reg.GetRegistryKey("ImageDir"), "Calc.png")))
            DirMain.oDirFormLib.oDir.ob.grdLookup.ContextMenu.MenuItems.Item(6).Text = "Đồng bộ thuốc lên cục quản lý dược"
            DirMain.oDirFormLib.oDir.ob.grdLookup.ContextMenu.MenuItems.Item(6).Shortcut = Shortcut.None
        Else
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(6).Visible = False
            DirMain.oDirFormLib.oDir.tbr.Buttons.Item(6).Visible = False
            DirMain.oDirFormLib.oDir.ob.grdLookup.ContextMenu.MenuItems.Item(6).Visible = False
        End If
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
                If SysID = "Item_Pharmacy" Then
                    If Msg.Question("Bạn có chắc muốn xóa thuốc này trên hệ thống dược quốc gia không") <> MsgBoxResult.Ok Then
                        Return
                    End If
                    DeletePharmacy(oDirFormLib.oDir.ob.CurDataRow.Item("ma_vt").ToString.Trim, BranchCode)
                    If Msg.Question("Bạn có muốn xóa luôn trên hệ thống Libs không") <> MsgBoxResult.Ok Then
                        Return
                    End If
                End If
                DirMain.Delete()
                Exit Select
            Case 4
                oDirFormLib.dirChangeCode()
                Exit Select
            Case 6
                If SysID = "Item_Pharmacy" Then
                    UpdatePharmacy(oDirFormLib.oDir.ob.CurDataRow.Item("ma_vt").ToString.Trim, BranchCode)
                End If
                Exit Select
            Case 8
                oDirFormLib.oDir.ob.frmLookup.Close()
                Exit Select
        End Select
    End Sub
    Private Sub Delete()
        Dim str As String = Nothing
        If (Not oDirFormLib.oDir.ob.CurDataRow Is Nothing) Then
            str = oDirFormLib.oDir.ob.CurDataRow.Item("PharmacyCode").ToString.Trim
        End If
        If str <> "" Then
            Msg.Alert("Thuốc này đã được cấp mã cục quản lý dược, không xóa được", 1)
        Else
            oDirFormLib.dirDelete()
        End If
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
                If SysID = "Item_Pharmacy" Then
                    If Msg.Question("Bạn có chắc muốn xóa thuốc này trên hệ thống dược quốc gia không") <> MsgBoxResult.Ok Then
                        Return
                    End If
                    DeletePharmacy(oDirFormLib.oDir.ob.CurDataRow.Item("ma_vt").ToString.Trim, BranchCode)
                    If Msg.Question("Bạn có muốn xóa luôn trên hệ thống Libs không") <> MsgBoxResult.Ok Then
                        Return
                    End If
                End If
                DirMain.Delete()
                Exit Select
            Case 4
                oDirFormLib.dirChangeCode()
                Exit Select
            Case 6
                If oDirFormLib.oDir.ob.CurDataRow.Item("pharmacy_status").ToString.Trim = "0" Then
                    Msg.Alert("Trạng thái thuốc không được cập nhật lên dược quốc gia")
                    Return
                End If
                UpdatePharmacy(oDirFormLib.oDir.ob.CurDataRow.Item("ma_vt").ToString.Trim, BranchCode)
                Exit Select
            Case 8
                oDirFormLib.oDir.ob.frmLookup.Close()
                Exit Select
        End Select
    End Sub
    Private Sub UpdatePharmacy(ByVal _ma_vt As String, ByVal BranchCode As String)
        Dim dr_connection As DataRow = Sql.GetRow(oDirFormLib.appConn, "pharmacy_apiconnection", "unitcode='" + BranchCode + "'")
        If dr_connection Is Nothing Then
            Msg.Alert(IIf(Reg.GetRegistryKey("").ToString.Trim = "V", "Đơn vị " + BranchCode + " chưa đăng ký tài khoản cục quản lý dược quốc gia", "Unit " + BranchCode + " do not register to National Pharmacy"))
            Return
        End If
        Dim uriBase As String = dr_connection.Item("uriBase").ToString.Trim
        Dim ma_co_so As String = dr_connection.Item("ma_co_so").ToString.Trim
        Dim usr As String = dr_connection.Item("usr").ToString.Trim
        Dim pwd As String = dr_connection.Item("pwd").ToString.Trim
        Dim token As token = CommandExcute.GetToken(uriBase, usr, pwd)
        If token Is Nothing Then
            Return
        End If

        Dim commandtype As String
        Dim _collection As New Collection
        _collection.Add(BranchCode, "unit", Nothing, Nothing)
        _collection.Add(token.token, "token", Nothing, Nothing)
        _collection.Add(uriBase, "uriBase", Nothing, Nothing)
        _collection.Add(_ma_vt, "ma_vt", Nothing, Nothing)
        Dim PharmacyCode As Object
        PharmacyCode = Sql.GetValue(oDirFormLib.appConn, "PharmacyProduct", "PharmacyCode", "BranchCode='" + BranchCode + "' AND PrdCode=" + Sql.ConvertVS2SQLType(_ma_vt, ""))
        If PharmacyCode Is Nothing Then
            PharmacyCode = ""
        End If
        If PharmacyCode = "" Then
            commandtype = "02"
        Else
            commandtype = "03"
        End If

        Dim httprp As HttpResponseMessage
        httprp = CommandExcute.CommandExcute(commandtype, oDirFormLib.appConn, _collection)
        If httprp IsNot Nothing Then
            Dim _ma_thuoc As String = httprp.Content.ReadAsStringAsync().Result.Trim
            If httprp.StatusCode = Net.HttpStatusCode.OK Then
                _ma_thuoc = _ma_thuoc.Substring(1, _ma_thuoc.Length - 2)
                Msg.Alert(httprp.StatusCode.ToString + ": " + _ma_thuoc)
                oDirFormLib.oDir.ob.CurDataRow.Item("PharmacyCode") = _ma_thuoc
                oDirFormLib.oDir.ob.CurDataRow.Item("BranchCode") = BranchCode
                oDirFormLib.oDir.ob.grdLookup.Refresh()
            Else
                Msg.Alert(httprp.StatusCode.ToString + ": " + _ma_thuoc)
            End If
        End If
    End Sub

    Private Sub DeletePharmacy(ByVal _ma_vt As String, ByVal BranchCode As String)
        Dim dr_connection As DataRow = Sql.GetRow(oDirFormLib.appConn, "pharmacy_apiconnection", "unitcode='" + BranchCode + "'")
        If dr_connection Is Nothing Then
            Msg.Alert(IIf(Reg.GetRegistryKey("").ToString.Trim = "V", "Đơn vị " + BranchCode + " chưa đăng ký tài khoản cục quản lý dược quốc gia", "Unit " + BranchCode + " do not register to National Pharmacy"))
            Return
        End If
        Dim uriBase As String = dr_connection.Item("uriBase").ToString.Trim
        Dim ma_co_so As String = dr_connection.Item("ma_co_so").ToString.Trim
        Dim usr As String = dr_connection.Item("usr").ToString.Trim
        Dim pwd As String = dr_connection.Item("pwd").ToString.Trim
        Dim token As token = CommandExcute.GetToken(uriBase, usr, pwd)
        If token Is Nothing Then
            Return
        End If

        Dim commandtype As String = "05"
        Dim _collection As New Collection
        _collection.Add(BranchCode, "unit", Nothing, Nothing)
        _collection.Add(token.token, "token", Nothing, Nothing)
        _collection.Add(uriBase, "uriBase", Nothing, Nothing)
        _collection.Add(_ma_vt, "ma_vt", Nothing, Nothing)
        Dim PharmacyCode As Object
        PharmacyCode = Sql.GetValue(oDirFormLib.appConn, "PharmacyProduct", "PharmacyCode", "BranchCode='" + BranchCode + "' AND PrdCode=" + Sql.ConvertVS2SQLType(_ma_vt, ""))
        If PharmacyCode Is Nothing Then
            Msg.Alert("Thuốc này chưa cấp mã quốc gia")
            Return
        End If
        _collection.Add(PharmacyCode, "ma_thuoc", Nothing, Nothing)

        Dim httprp As HttpResponseMessage
        httprp = CommandExcute.CommandExcute(commandtype, oDirFormLib.appConn, _collection)
        If httprp IsNot Nothing Then
            If httprp.StatusCode = Net.HttpStatusCode.OK Then
                Msg.Alert("Xóa thuốc thành công trên hệ thống dược quốc gia")
                oDirFormLib.oDir.ob.CurDataRow.Item("PharmacyCode") = ""
                oDirFormLib.oDir.ob.grdLookup.Refresh()
            Else
                Msg.Alert(httprp.StatusCode.ToString)
            End If
        End If
    End Sub

    ' Fields
    Public cOldItem As String
    Public oDirFormLib As DirFormLibBrowse
    Public SysID As String
    Public ItemType As String
    Public BranchCode As String
End Module

