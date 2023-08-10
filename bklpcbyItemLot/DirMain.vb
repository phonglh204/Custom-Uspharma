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
    <STAThread>
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
                DirMain.SysID = "bklpcbyItemLot"
                Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                DirMain.PrintReport()
                DirMain.rpTable = Nothing
            End If
        End If
    End Sub

    Private Sub Print(ByVal nType As Integer)
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
            clsprint.oRpt.SetParameterValue("h_gia_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("903")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
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
        DirMain.fPrint.ShowDialog
        DirMain.fPrint.Dispose
        DirMain.sysConn.Close()
        DirMain.appConn.Close()
    End Sub

    Private Sub ReportProc(ByVal nIndex As Integer)
        Select Case nIndex
            Case 1
                If Not Information.IsNothing(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow) Then
                    Dim curDataRow As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(curDataRow.Item("ma_vt"))) Or Information.IsDBNull(RuntimeHelpers.GetObjectValue(curDataRow.Item("ma_lo"))) Then
                        Return
                    End If
                    DirMain.Item = Strings.Trim(StringType.FromObject(curDataRow.Item("ma_vt")))
                    DirMain.Lot = Strings.Trim(StringType.FromObject(curDataRow.Item("ma_lo")))
                    If (StringType.StrCmp(Strings.Trim(DirMain.Item), "", False) = 0) Then
                        Return
                    End If
                    Dim str2 As String = ""
                    Dim cString As String = "so_luong"
                    Dim num2 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    Dim i As Integer = 1
                    Do While (i <= num2)
                        Dim str3 As String = Strings.Trim(Fox.GetWordNum(cString, i, ","c))
                        str2 = (str2 & Strings.Trim(StringType.FromObject(curDataRow.Item(str3))) & ", ")
                        i += 1
                    Loop
                    Dim strSQLLong As String = "spBkLPC "
                    strSQLLong += Sql.ConvertVS2SQLType(DirMain.dFrom, "")
                    strSQLLong += "," + Sql.ConvertVS2SQLType(DirMain.dTo, "")
                    strSQLLong += ",'" + DirMain.Item.Replace("'", "''") + "'"
                    strSQLLong += ",'" + DirMain.Lot.Replace("'", "''") + "'"
                    strSQLLong += ",'" + DirMain.Site.Replace("'", "''") + "'"
                    strSQLLong += ",'" + DirMain.strUnit.Trim + "'"
                    DirMain.oDirFormDetailLib = New reportformlib("0111110001")
                    oDirFormDetailLib.sysConn = DirMain.sysConn
                    oDirFormDetailLib.appConn = DirMain.appConn
                    oDirFormDetailLib.oLan = DirMain.oLan
                    oDirFormDetailLib.oLen = DirMain.oLen
                    oDirFormDetailLib.oVar = DirMain.oVar
                    oDirFormDetailLib.SysID = "bklpc"
                    oDirFormDetailLib.cForm = "bklpc"
                    oDirFormDetailLib.cCode = StringType.FromObject(Interaction.IIf((DirMain.fPrint.cboReports.SelectedIndex = 0), "001", "002"))
                    oDirFormDetailLib.strAliasReports = "glcd1d"
                    oDirFormDetailLib.Init()
                    oDirFormDetailLib.strSQLRunReports = strSQLLong
                    oDirFormDetailLib.Show()
                    DirMain.oDirFormDetailLib = Nothing
                    curDataRow = Nothing
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
        Try
            Dim str As String = "EXEC spBkLpxByItemLot "
            str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtLoai_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt2.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt3.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
            DirMain.oDirFormLib = New reportformlib("1011111111")
            oDirFormLib.sysConn = DirMain.sysConn
            oDirFormLib.appConn = DirMain.appConn
            oDirFormLib.oLan = DirMain.oLan
            oDirFormLib.oLen = DirMain.oLen
            oDirFormLib.oVar = DirMain.oVar
            oDirFormLib.SysID = DirMain.SysID
            oDirFormLib.cForm = DirMain.SysID
            oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
            oDirFormLib.strAliasReports = "inbk4"
            oDirFormLib.Init()
            oDirFormLib.strSQLRunReports = str
            AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            oDirFormLib.Show()
            RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
        Catch
            Msg.Alert(StringType.FromObject(DirMain.oLan.Item("900")), 2)
        End Try
        DirMain.oDirFormLib = Nothing
    End Sub


    ' Fields
    Public appConn As SqlConnection
    Public dTo, dFrom As DateTime
    Public fPrint As frmFilter = New frmFilter
    Private oDirFormDetailLib As reportformlib
    Public oDirFormLib As reportformlib
    Public oLan As Collection = New Collection
    Public oLen As Collection = New Collection
    Public oOption As Collection = New Collection
    Public oVar As Collection = New Collection
    Public rpTable As DataTable
    Public strUnit As String
    Public sysConn As SqlConnection
    Public SysID As String, Item As String, Lot As String, Site As String
End Module

