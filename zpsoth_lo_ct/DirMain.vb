Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports libscommon
Imports libscontrol
Imports libscontrol.reportformlib

Module DirMain
    ' Methods
    <STAThread()> _
    Public Sub main(ByVal CmdArgs As String())
        If Not BooleanType.FromObject(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0))) Then
            DirMain.sysConn = Sys.GetSysConn
            If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(DirMain.sysConn, "Access")) Then
                DirMain.sysConn.Close()
                DirMain.sysConn = Nothing
            Else
                DirMain.cCodeSelected = Strings.Trim(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 1, "#"c))
                Try
                    DirMain.strKeyCust = Strings.Replace(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 2, "#"c), "%", " ", 1, -1, CompareMethod.Binary)
                Catch exception1 As Exception
                    ProjectData.SetProjectError(exception1)
                    Dim exception As Exception = exception1
                    DirMain.strKeyCust = "1=1"
                    ProjectData.ClearProjectError()
                End Try
                DirMain.appConn = Sys.GetConn
                Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                DirMain.SysID = "zpsoth_lo_ct"
                Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                DirMain.drAdvFilter = DirectCast(Sql.GetRow((DirMain.sysConn), "reports", ("form = '" & DirMain.SysID & "'")), DataRow)
                DirMain.ReportRow = DirectCast(Sql.GetRow((DirMain.sysConn), "reports", StringType.FromObject(ObjectType.AddObj("form=", Sql.ConvertVS2SQLType(DirMain.SysID, "")))), DataRow)
                DirMain.PrintReport()
                DirMain.rpTable = Nothing
            End If
        End If
    End Sub

    Private Sub Print(ByVal nType As Integer)
        DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.Select(0)
        Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
        Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
        Dim obj2 As Object = Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301"))), "%d1", StringType.FromDate(DirMain.dFrom), 1, -1, CompareMethod.Binary)), "%d2", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
        Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
        Dim clsprint As New clsprint(getGrid.GetForm, strFile, Nothing)
        clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
        clsprint.oVar = DirMain.oVar
        clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
        clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
        clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(obj2))
        Try
            clsprint.oRpt.SetParameterValue("h_tien_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("904")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            ProjectData.ClearProjectError()
        End Try
        If (nType = 0) Then
            clsprint.PrintReport(1)
            clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
        Else
            clsprint.ShowReports()
        End If
        clsprint.oRpt.Close()
        getGrid = Nothing
    End Sub

    Public Sub PrintReport()
        DirMain.rpTable = clsprint.InitComboReport(DirMain.sysConn, DirMain.fPrint.cboReports, DirMain.SysID)
        DirMain.fPrint.ShowDialog()
        DirMain.fPrint.Dispose()
        DirMain.sysConn.Close()
        DirMain.appConn.Close()
    End Sub

    Private Sub ReportDetailProc(ByVal nIndex As Integer)
        If (nIndex = 0) Then
            DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(StringType.FromObject(DirMain.oLan.Item("903")), "%s", (Strings.Trim(DirMain.strItem) & " - " & DirMain.strItemName), 1, -1, CompareMethod.Binary)
            DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Trim(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text)
        End If
    End Sub

    Private Sub ReportProc(ByVal nIndex As Integer)
        Select Case nIndex
            Case 0
                DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Trim(DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text)
                Exit Select
            Case 1
                If Not Information.IsNothing(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow) Then
                    Dim curDataRow As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(curDataRow.Item("Ma_vt"))) Then
                        Return
                    End If
                    DirMain.strItem = Strings.Trim(StringType.FromObject(curDataRow.Item("Ma_vt")))
                    If (StringType.StrCmp(Strings.Trim(DirMain.strItem), "", False) = 0) Then
                        Return
                    End If
                    curDataRow = Nothing
                    DirMain.strItemName = Strings.Trim(StringType.FromObject(Sql.GetValue((DirMain.appConn), "dmvt", StringType.FromObject(ObjectType.AddObj("ten_vt", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))), ("ma_vt = '" & DirMain.strItem & "'"))))
                    Dim str As String = (StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj((StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(("fs_SOSalesSummaryDetail" & DirMain.oxInv.xStore), ObjectType.AddObj(ObjectType.AddObj("'", Reg.GetRegistryKey("Language")), "'"))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.strItem, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nvbh.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vv.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nx.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtTk_vt2.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtLoai_vt.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "ma_gd"), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "loai_ct"), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "ma_ct"), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbTinh_dc.SelectedValue), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvFrom.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvTo.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))), ObjectType.AddObj(", ", DirMain.oLen.Item("so_ct")))) & ", '2'"), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvtables")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin1")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin2")), "")))) & ",'" & Strings.Replace(DirMain.oAdvFilter.GetAdvSelectKey, "'", "''", 1, -1, CompareMethod.Binary) & "'")
                    oDirFormDetailLib = New reportformlib("0111110001")
                    oDirFormDetailLib.sysConn = DirMain.sysConn
                    oDirFormDetailLib.appConn = DirMain.appConn
                    oDirFormDetailLib.oLan = DirMain.oLan
                    oDirFormDetailLib.oLen = DirMain.oLen
                    oDirFormDetailLib.oVar = DirMain.oVar
                    oDirFormDetailLib.SysID = DirMain.SysID
                    oDirFormDetailLib.cForm = "SOTransListOfItem"
                    oDirFormDetailLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
                    oDirFormDetailLib.strAliasReports = "inth1d"
                    oDirFormDetailLib.Init()
                    oDirFormDetailLib.strSQLRunReports = str
                    RemoveHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                    AddHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                    oDirFormDetailLib.Show()
                    RemoveHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                    AddHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                    oDirFormDetailLib = Nothing
                    Exit Select
                End If
                Return
            Case 2
                DirMain.Print(0)
                Exit Select
            Case 3
                DirMain.Print(1)
                Exit Select
        End Select
    End Sub

    Public Sub ShowReport()
        Dim num2 As Integer
        Try
            ProjectData.ClearProjectError()
            num2 = 1
            'Dim str As String = StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj((StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(("EXEC fs20_SOSalesSummary" & DirMain.oxInv.xStore), Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vt.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nvbh.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vv.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nx.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt2.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt3.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.strGroups, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtTk_vt.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtTk_vt2.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtLoai_vt.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "ma_gd"), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "loai_ct"), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "ma_ct"), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbTinh_dc.SelectedValue), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvFrom.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvTo.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))), ObjectType.AddObj(", ", DirMain.oLen.Item("so_ct")))) & ", '2'"), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvtables")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin1")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin2")), ""))))
            Dim str As String = "EXEC zp_soth_lo_ct" & DirMain.oxInv.xStore
            str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_kh1.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_kh2.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_kh3.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nvbh.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vv.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nx.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt2.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt3.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.strGroups, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtTk_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtTk_vt2.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtTk_gv.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtLoai_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "ma_gd"), "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "loai_ct"), "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "ma_ct"), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbTinh_dc.SelectedValue), "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvFrom.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvTo.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
            str += ", " + DirMain.oLen.Item("so_ct")
            str += ", '2'"
            str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvtables")), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin1")), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin2")), "")
            Dim expression As String = StringType.FromObject(Interaction.IIf((StringType.StrCmp(Strings.Trim(DirMain.oAdvFilter.GetAdvSelectKey), "", False) = 0), "1=1", DirMain.oAdvFilter.GetAdvSelectKey))
            str = (str & ",'" & Strings.Replace(expression, "'", "''", 1, -1, CompareMethod.Binary) & "'")
            oDirFormLib = New reportformlib("0111111111")
            oDirFormLib.sysConn = DirMain.sysConn
            oDirFormLib.appConn = DirMain.appConn
            oDirFormLib.oLan = DirMain.oLan
            oDirFormLib.oLen = DirMain.oLen
            oDirFormLib.oVar = DirMain.oVar
            oDirFormLib.SysID = DirMain.SysID
            oDirFormLib.cForm = DirMain.SysID
            oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
            oDirFormLib.strAliasReports = "inth1"
            oDirFormLib.Init()
            oDirFormLib.strSQLRunReports = str
            AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            oDirFormLib.Show()
            RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            oDirFormLib = Nothing
        Catch
        Msg.Alert(StringType.FromObject(DirMain.oLan.Item("900")), 2)
        End Try
    End Sub


    ' Fields
    Public appConn As SqlConnection
    Public cCodeSelected As String
    Public dFrom As DateTime
    Public drAdvFilter As DataRow
    Public dTo As DateTime
    Public fPrint As frmFilter = New frmFilter
    Public oAdvFilter As clsAdvFilter
    Private oDirFormDetailLib As reportformlib
    Public oDirFormLib As reportformlib
    Public oLan As Collection = New Collection
    Public oLen As Collection = New Collection
    Public oOption As Collection = New Collection
    Public oVar As Collection = New Collection
    Public oxInv As xInv
    Public ReportRow As DataRow
    Public rpTable As DataTable
    Public strAccount As String
    Public strAccountRef As String
    Private strCustID As String
    Private strCustName As String
    Public strGroups As String
    Public strItem As String
    Public strItemName As String
    Public strKeyCust As String
    Public strUnit As String
    Public sysConn As SqlConnection
    Public SysID As String
End Module

