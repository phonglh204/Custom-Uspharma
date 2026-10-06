Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol
Imports libscontrol.clsreports

Namespace v20artthd
    <StandardModule> _
    Friend NotInheritable Class DirMain
        ' Methods
        Private Shared Sub Delete()
            If Not DirMain.isCheck Then
                Msg.Alert(StringType.FromObject(DirMain.oLan.Item("403")), 2)
            Else
                Dim sLeft As String = ""
                Dim num3 As Integer = (DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Count - 1)
                Dim num As Integer = 0
                Do While (num <= num3)
                    Dim view2 As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Item(num)
                    If BooleanType.FromObject(view2.Item("Tag")) Then
                        Dim row As DataRow = DirectCast(Sql.GetRow((DirMain.appConn), "cttt20", StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj("stt_rec_tt = '", view2.Item("stt_rec")), "' AND ma_ct = '"), DirMain.cVC), "'"))), DataRow)
                        If ((Not row Is Nothing) AndAlso Not clsvoucher.clsVoucher.CheckLockedDate(DateType.FromObject(row.Item("ngay_ct")), DirMain.appConn, DirMain.cVC, False)) Then
                            sLeft = String.Concat(New String() {sLeft, "; ", Strings.Trim(StringType.FromObject(view2.Item("so_ct"))), " - ", Strings.Format(RuntimeHelpers.GetObjectValue(view2.Item("ngay_ct")), "dd/MM/yyyy")})
                        End If
                    End If
                    view2 = Nothing
                    num += 1
                Loop
                If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                    sLeft = Strings.Mid(sLeft, 3)
                    Msg.Alert(StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(DirMain.oVar.Item("m_ks_dn"), "("), sLeft), ")")), 2)
                ElseIf (ObjectType.ObjTst(Msg.Question(StringType.FromObject(DirMain.oLan.Item("405")), 1), 1, False) = 0) Then
                    Dim num2 As Integer = (DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Count - 1)
                    num = 0
                    Do While (num <= num2)
                        Dim view As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Item(num)
                        If BooleanType.FromObject(view.Item("Tag")) Then
                            view.Item("Tag") = False
                            Dim str2 As String = "spInvOut_tattoan_delete "
                            str2 += Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(view.Item("stt_rec")), "")
                            str2 += ", '" + Strings.Trim(DirMain.cVC) + "'"
                            str2 += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(Reg.GetRegistryKey("CurrUserID")), "")
                            Sql.SQLExecute((DirMain.appConn), str2)
                        End If
                        view = Nothing
                        num += 1
                    Loop
                    DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.Refresh
                End If
            End If
        End Sub

        Private Shared Sub grdKeyup(ByVal sender As Object, ByVal e As KeyEventArgs)
            If (e.Control And (e.KeyCode = Keys.A)) Then
                DirMain.SelectRows(True)
            End If
            If (e.Control And (e.KeyCode = Keys.U)) Then
                DirMain.SelectRows(False)
            End If
        End Sub

        Private Shared Function isCheck() As Boolean
            Dim num2 As Integer = (DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                If BooleanType.FromObject(DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Item(i).Item("Tag")) Then
                    Return True
                End If
                i += 1
            Loop
            Return False
        End Function

        <STAThread> _
        Public Shared Sub main(ByVal CmdArgs As String())
            If Not BooleanType.FromObject(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0))) Then
                DirMain.sysConn = Sys.GetSysConn
                If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(DirMain.sysConn, "Access")) Then
                    DirMain.sysConn.Close
                    DirMain.sysConn = Nothing
                Else
                    DirMain.cVC = Strings.Trim(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 1, "#"c))
                    Try 
                        DirMain.strKeyCust = Strings.Replace(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 2, "#"c), "%", " ", 1, -1, CompareMethod.Binary)
                    Catch exception1 As Exception
                        ProjectData.SetProjectError(exception1)
                        Dim exception As Exception = exception1
                        DirMain.strKeyCust = "1=1"
                        ProjectData.ClearProjectError
                    End Try
                    DirMain.appConn = Sys.GetConn
                    Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                    Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                    Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                    DirMain.SysID = "v20ARFullyPaid4Invoice"
                    Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                    DirMain.PrintReport
                    DirMain.rpTable = Nothing
                End If
            End If
        End Sub

        Private Shared Sub mnFileClick(ByVal sender As Object, ByVal e As EventArgs)
            Select Case IntegerType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0  - 1) {}, Nothing, Nothing))
                Case 5
                    DirMain.ReportProc(5)
                    Exit Select
                Case 8
                    DirMain.ReportProc(8)
                    Exit Select
            End Select
        End Sub

        Private Shared Sub Post()
            If Not clsvoucher.clsVoucher.CheckLockedDate(DirMain.fPrint.txtNgay_tt.Value, DirMain.appConn, DirMain.cVC, False) Then
                'If Not clsvoucher.clsVoucher.CheckLockedDate(CDate(Sql.GetValue(appConn, "dbo.ff_GetEndDateOfCycle(" + fPrint.txtKy.Value.ToString + ", " + fPrint.txtNam.Value.ToString + ")")), DirMain.appConn, DirMain.cVC, False) Then
                Msg.Alert(StringType.FromObject(DirMain.oVar.Item("m_ks_dn")), 2)
            ElseIf Not DirMain.isCheck Then
                Msg.Alert(StringType.FromObject(DirMain.oLan.Item("403")), 2)
            ElseIf (ObjectType.ObjTst(Msg.Question(StringType.FromObject(DirMain.oLan.Item("404")), 1), 1, False) = 0) Then
                Dim num2 As Integer = (DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    Dim view As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Item(i)
                    If BooleanType.FromObject(view.Item("Tag")) Then
                        view.Item("Tag") = False
                        Dim str As String = "spInvOut_tattoan_post "
                        str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtNgay_tt.Value, "")
                        str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(view.Item("stt_rec")), "")
                        str += ", '" + Strings.Trim(DirMain.cVC) + "'"
                        str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(view.Item("con_pt")), "")
                        str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(view.Item("con_pt_nt")), "")
                        str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(Reg.GetRegistryKey("CurrUserID")), "")
                        str += "," + Sql.ConvertVS2SQLType(DirMain.fPrint.txtTao_bt_yn.Text, "")
                        Sql.SQLExecute((DirMain.appConn), str)
                    End If
                    view = Nothing
                    i += 1
                Loop
                DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.Refresh()
            End If
        End Sub

        Public Shared Sub PrintReport()
            DirMain.rpTable = clsprint.InitComboReport(DirMain.sysConn, DirMain.fPrint.cboReports, DirMain.SysID)
            DirMain.fPrint.ShowDialog
            DirMain.fPrint.Dispose
            DirMain.sysConn.Close
            DirMain.appConn.Close
        End Sub

        Private Shared Sub ProcReport(ByVal nType As Integer)
            Dim filter As New frmFilter2
            DirMain.rpTable2 = clsprint.InitComboReport(DirMain.sysConn, filter.cboReports, (DirMain.SysID & "P"))
            DirMain.isContinue = False
            filter.ShowDialog
            If DirMain.isContinue Then
                Dim selectedIndex As Integer = filter.cboReports.SelectedIndex
                Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable2.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
                Dim ds As New DataSet
                Dim str As String = "EXEC fs20_TransGLReportDetailByCust "
                str = StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(str, Sql.ConvertVS2SQLType("PT9", ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(filter.txtKy1.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(filter.txtNam1.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(filter.txtKy2.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(filter.txtNam2.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(filter.txtMa_dvcs.Text, ""))))
                Sql.SQLRetrieve((Sys.GetConn), str, "tc", (ds))
                Dim view As New DataView
                Dim num4 As Integer = IntegerType.FromObject(Sql.GetValue((Sys.GetConn), "dmct", "max_row", "ma_ct = 'PT9'"))
                view.Table = ds.Tables.Item(0)
                view.RowFilter = "sysprint = 1"
                Dim count As Integer = view.Count
                view.RowFilter = ""
                Dim num5 As Integer = num4
                Dim i As Integer = count
                Do While (i <= num5)
                    view.AddNew.Item("sysprint") = 1
                    i += 1
                Loop
                Dim clsprint As New clsprint(DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm, strFile, Nothing)
                clsprint.oRpt.SetDataSource(view.Table)
                If (view.Table.Rows.Count > 0) Then
                    clsprint.dr = view.Table.Rows.Item(0)
                Else
                    view.AddNew
                    clsprint.dr = view.Table.Rows.Item(0)
                End If
                clsprint.oVar = DirMain.oDirFormLib.oVar
                clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
                clsprint.oRpt.SetParameterValue("Title", Strings.Trim(filter.txtTitle.Text))
                clsprint.oRpt.SetParameterValue("fDfrom", (Strings.Trim(filter.txtKy1.Text) & "/" & Strings.Trim(filter.txtNam1.Text)))
                clsprint.oRpt.SetParameterValue("fDTo", (Strings.Trim(filter.txtKy2.Text) & "/" & Strings.Trim(filter.txtNam2.Text)))
                If (nType = 1) Then
                    clsprint.PrintReport(1)
                    clsprint.oRpt.SetDataSource(view.Table)
                Else
                    clsprint.ShowReports
                End If
                clsprint.oRpt.Close
                filter.Dispose
                filter = Nothing
            End If
        End Sub

        Private Shared Sub ReportProc(ByVal nIndex As Integer)
            Dim getGrid As ReportBrowse
            Select Case nIndex
                Case 0
                    getGrid = DirMain.oDirFormLib.GetClsreports.GetGrid
                    getGrid.GetGrid.ReadOnly = False
                    getGrid.GetDataView.AllowDelete = False
                    getGrid.GetDataView.AllowNew = False
                    Dim num As Integer = 0
                    Do While (1 <> 0)
                        Try 
                            num += 1
                            getGrid.GetGrid.TableStyles.Item(0).GridColumnStyles.Item(num).ReadOnly = True
                        Catch exception1 As Exception
                            ProjectData.SetProjectError(exception1)
                            Dim exception As Exception = exception1
                            ProjectData.ClearProjectError
                            Exit Do
                        End Try
                    Loop
                    Exit Select
                Case 1, 4, 6, 7
                    Return
                Case 2
                    DirMain.Delete
                    Return
                Case 3
                    DirMain.ProcReport(1)
                    Return
                Case 5
                    DirMain.Post
                    Return
                Case 8
                    DirMain.ProcReport(0)
                    Return
                Case Else
                    Return
            End Select
            AddHandler getGrid.GetGrid.KeyUp, New KeyEventHandler(AddressOf DirMain.grdKeyup)
            getGrid = Nothing
            DirMain.oDirFormLib.GetClsreports.tbr.Buttons.Item(5).ToolTipText = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("401"))
            DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(5).Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("401"))
            DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(5).Shortcut = Shortcut.CtrlF9
            DirMain.oDirFormLib.GetClsreports.tbr.Buttons.Item(5).Style = ToolBarButtonStyle.PushButton
            DirMain.oDirFormLib.GetClsreports.tbr.ImageList.Images.Item(5) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "new.bmp")))
            DirMain.oDirFormLib.GetClsreports.tbr.Buttons.Item(6).ToolTipText = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("402"))
            DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(6).Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("402"))
            DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(6).Shortcut = Shortcut.CtrlF8
            DirMain.oDirFormLib.GetClsreports.tbr.ImageList.Images.Item(6) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "delete.bmp")))
            DirMain.oDirFormLib.GetClsreports.tbr.Buttons.Item(7).ToolTipText = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("415"))
            DirMain.oDirFormLib.GetClsreports.tbr.ImageList.Images.Item(7) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "print.bmp")))
            DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(7).Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("415"))
            DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(7).Shortcut = Shortcut.CtrlP
            DirMain.oDirFormLib.GetClsreports.tbr.Buttons.Item(8).ToolTipText = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("406"))
            DirMain.oDirFormLib.GetClsreports.tbr.ImageList.Images.Item(8) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "preview.bmp")))
            DirMain.oDirFormLib.GetClsreports.tbr.Buttons.Item(8).Style = ToolBarButtonStyle.PushButton
            DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(8).Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("406"))
            DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.ContextMenu.MenuItems.Clear
            Dim num3 As Integer = (DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num3)
                DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.ContextMenu.MenuItems.Add(DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(i).CloneMenu)
                AddHandler DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(i).Click, New EventHandler(AddressOf DirMain.mnFileClick)
                AddHandler DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.ContextMenu.MenuItems.Item(i).Click, New EventHandler(AddressOf DirMain.mnFileClick)
                i += 1
            Loop
        End Sub

        Private Shared Sub SelectRows(ByVal lType As Boolean)
            Dim num2 As Integer = (DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Item(i).Item("Tag") = lType
                i += 1
            Loop
        End Sub

        Public Shared Sub ShowReport()
            Dim str As String = "EXEC spInvOut_tattoan_get "
            str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDReport.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtTk.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nh1.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nh2.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nh3.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvFrom.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvTo.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtTFrom.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtTTo.Value, "")
            str += ", " + DirMain.fPrint.txtType.Text
            str += ", " + DirMain.oLen.Item("so_ct")
            str += ", '" + Strings.Trim(DirMain.cVC) + "'"
            DirMain.oDirFormLib = New reportformlib("0111111111")
            oDirFormLib.sysConn = DirMain.sysConn
            oDirFormLib.appConn = DirMain.appConn
            oDirFormLib.oLan = DirMain.oLan
            oDirFormLib.oLen = DirMain.oLen
            oDirFormLib.oVar = DirMain.oVar
            oDirFormLib.SysID = DirMain.SysID
            oDirFormLib.cForm = DirMain.SysID
            oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
            oDirFormLib.strAliasReports = "arttbk1"
            oDirFormLib.Init
            oDirFormLib.strSQLRunReports = str
            AddHandler oDirFormLib.ReportProc, New reportformlib.ReportProcEventHandler(AddressOf DirMain.ReportProc)
            AddHandler oDirFormLib.GetClsreports.tbr.ButtonClick, New ToolBarButtonClickEventHandler(AddressOf DirMain.tbrClick)
            oDirFormLib.Show
            RemoveHandler oDirFormLib.ReportProc, New reportformlib.ReportProcEventHandler(AddressOf DirMain.ReportProc)
            RemoveHandler oDirFormLib.GetClsreports.tbr.ButtonClick, New ToolBarButtonClickEventHandler(AddressOf DirMain.tbrClick)
            DirMain.oDirFormLib = Nothing
        End Sub

        Private Shared Sub tbrClick(ByVal sender As Object, ByVal e As ToolBarButtonClickEventArgs)
            Dim objArray2 As Object() = New Object(1  - 1) {}
            Dim args As ToolBarButtonClickEventArgs = e
            objArray2(0) = args.Button
            Dim objArray As Object() = objArray2
            Dim copyBack As Boolean() = New Boolean() { True }
            If copyBack(0) Then
                args.Button = DirectCast(objArray(0), ToolBarButton)
            End If
            Select Case IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "Buttons", New Object(0  - 1) {}, Nothing, Nothing), Nothing, "IndexOf", objArray, Nothing, copyBack))
                Case 5
                    DirMain.ReportProc(5)
                    Exit Select
                Case 8
                    DirMain.ReportProc(8)
                    Exit Select
            End Select
        End Sub


        ' Fields
        Public Shared appConn As SqlConnection
        Private Shared cVC As String
        Public Shared dFrom As DateTime
        Public Shared dTo As DateTime
        Public Shared fPrint As frmFilter = New frmFilter
        Public Shared isContinue As Boolean
        Public Shared dNgay_tt As DateTime
        Private Shared oDirFormDetailLib As reportformlib
        Public Shared oDirFormLib As reportformlib
        Public Shared oLan As Collection = New Collection
        Public Shared oLen As Collection = New Collection
        Public Shared oOption As Collection = New Collection
        Public Shared oVar As Collection = New Collection
        Public Shared rpTable As DataTable
        Public Shared rpTable2 As DataTable
        Public Shared strAccount As String
        Public Shared strAccountRef As String
        Private Shared strCustID As String
        Private Shared strCustName As String
        Public Shared strKeyCust As String
        Public Shared strUnit As String
        Public Shared sysConn As SqlConnection
        Public Shared SysID As String
    End Class
End Namespace

