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
    Public Sub main()
        If Not BooleanType.FromObject(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0))) Then
            DirMain.sysConn = Sys.GetSysConn
            If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(DirMain.sysConn, "Access")) Then
                DirMain.sysConn.Close()
                DirMain.sysConn = Nothing
            Else
                DirMain.appConn = Sys.GetConn
                Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                DirMain.SysID = "z15fabckh_year"
                Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                DirMain.drAdvFilter = DirectCast(Sql.GetRow((DirMain.sysConn), "reports", ("form = '" & DirMain.SysID & "'")), DataRow)
                DirMain.PrintReport()
                DirMain.rpTable = Nothing
            End If
        End If
    End Sub

    Private Sub Print(ByVal nType As Integer)
        DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.Select(0)
        Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
        Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
        Dim str As String = Strings.Replace(StringType.FromObject(DirMain.oLan.Item("301")), "%y1", StringType.FromDouble(DirMain.fPrint.txtNam.Value), 1, -1, CompareMethod.Binary)
        Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
        Dim clsprint As New clsprint(getGrid.GetForm, strFile, Nothing)
        clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
        clsprint.oVar = DirMain.oVar
        clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
        clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
        clsprint.oRpt.SetParameterValue("t_date", str)
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
            DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text, "%s", (Strings.Trim(DirMain.strAssetID) & " - " & DirMain.strAssetName), 1, -1, CompareMethod.Binary)
            DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Trim(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text)
        End If
    End Sub

    Private Sub ReportProc(ByVal nIndex As Integer)
        Select Case nIndex
            Case 1
                If Not Information.IsNothing(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow) Then
                    Dim curDataRow As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow
                    If (Information.IsDBNull(RuntimeHelpers.GetObjectValue(curDataRow.Item("so_the_ts"))) Or Information.IsDBNull(RuntimeHelpers.GetObjectValue(curDataRow.Item("stt")))) Then
                        Return
                    End If
                    DirMain.strAssetID = Strings.Trim(StringType.FromObject(curDataRow.Item("so_the_ts")))
                    If (StringType.StrCmp(Strings.Trim(DirMain.strAssetID), "", False) = 0) Then
                        Return
                    End If
                    curDataRow = Nothing
                    DirMain.strAssetName = Strings.Trim(StringType.FromObject(Sql.GetValue((DirMain.appConn), "dmts", StringType.FromObject(ObjectType.AddObj("ten_ts", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))), ("so_the_ts = '" & DirMain.strAssetID & "'"))))
                    Dim str As String = "EXEC fs20_FADepreciationReportDetail "
                    'str = (StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(str, Sql.ConvertVS2SQLType(DirMain.fPrint.txtMFrom.Value, ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNam.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMTo.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNam2.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.strAssetID, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvtables")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin1")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin2")), "")))) & ",'" & Strings.Replace(DirMain.oAdvFilter.GetAdvSelectKey, "'", "''", 1, -1, CompareMethod.Binary) & "'")
                    str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtNam.Value, "")
                    str += ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.strAssetID, ""))
                    str += ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), ""))
                    str += ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvtables")), ""))
                    str += ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin1")), ""))
                    str += ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin2")), ""))
                    str += ",'" & Strings.Replace(DirMain.oAdvFilter.GetAdvSelectKey, "'", "''", 1, -1, CompareMethod.Binary) & "'"
                    DirMain.oDirFormDetailLib = New reportformlib("0011110001")
                    oDirFormDetailLib.sysConn = DirMain.sysConn
                    oDirFormDetailLib.appConn = DirMain.appConn
                    oDirFormDetailLib.oLan = DirMain.oLan
                    oDirFormDetailLib.oLen = DirMain.oLen
                    oDirFormDetailLib.oVar = DirMain.oVar
                    oDirFormDetailLib.SysID = DirMain.SysID
                    oDirFormDetailLib.cForm = "v20FaDepreciationReportD"
                    oDirFormDetailLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
                    oDirFormDetailLib.strAliasReports = "FaBcTs"
                    oDirFormDetailLib.Init()
                    oDirFormDetailLib.strSQLRunReports = str
                    RemoveHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                    AddHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                    oDirFormDetailLib.Show()
                    RemoveHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                    AddHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                    DirMain.oDirFormDetailLib = Nothing
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
        Dim str As String = "EXEC spz15fabckh_year "
        str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtNam.Value, "")
        str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtLoai_ts.Text, "")
        str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_bpsd.Text, "")
        str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_ts1.Text, "")
        str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_ts2.Text, "")
        str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_ts3.Text, "")
        str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
        str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.cbbGroup.SelectedValue), "")
        str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")
        str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvtables")), "")
        str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin1")), "")
        str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin2")), "")
        Dim expression As String = StringType.FromObject(Interaction.IIf((StringType.StrCmp(Strings.Trim(DirMain.oAdvFilter.GetAdvSelectKey), "", False) = 0), "1=1", DirMain.oAdvFilter.GetAdvSelectKey))
        Dim tcSQL As String = (str & ",'" & Strings.Replace((expression & " and 1=0 "), "'", "''", 1, -1, CompareMethod.Binary) & "'")
        str = (str & ",'" & Strings.Replace(expression, "'", "''", 1, -1, CompareMethod.Binary) & "'")
        Try
            Sql.SQLExecute((DirMain.appConn), tcSQL)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            Msg.Alert(StringType.FromObject(DirMain.oLan.Item("900")), 2)
            ProjectData.ClearProjectError()
            Return
        End Try
        DirMain.oDirFormLib = New reportformlib("0011111111")
        oDirFormLib.sysConn = DirMain.sysConn
        oDirFormLib.appConn = DirMain.appConn
        oDirFormLib.oLan = DirMain.oLan
        oDirFormLib.oLen = DirMain.oLen
        oDirFormLib.oVar = DirMain.oVar
        oDirFormLib.SysID = DirMain.SysID
        oDirFormLib.cForm = DirMain.SysID
        oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
        oDirFormLib.strAliasReports = "fatsin"
        oDirFormLib.Init()
        oDirFormLib.strSQLRunReports = str
        AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
        oDirFormLib.Show()
        RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
        DirMain.oDirFormLib = Nothing
    End Sub


    ' Fields
    Public appConn As SqlConnection
    Public drAdvFilter As DataRow
    Public fPrint As frmFilter = New frmFilter
    Public oAdvFilter As clsAdvFilter
    Private oDirFormDetailLib As reportformlib
    Public oDirFormLib As reportformlib
    Public oLan As Collection = New Collection
    Public oLen As Collection = New Collection
    Public oOption As Collection = New Collection
    Public oVar As Collection = New Collection
    Public rpTable As DataTable
    Public strAssetID As String
    Public strAssetName As String
    Public sysConn As SqlConnection
    Public SysID As String
End Module

