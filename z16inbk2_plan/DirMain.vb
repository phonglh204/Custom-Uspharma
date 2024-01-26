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
                DirMain.cForm = CharType.FromString(Strings.Trim(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 1, "#"c)))
                DirMain.appConn = Sys.GetConn
                Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                DirMain.SysID = "z16inbk2_plan"
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
        Dim obj2 As Object = Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301"))), "%d1", StringType.FromDate(DirMain.dFrom), 1, -1, CompareMethod.Binary)), "%d2", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)


        Dim _date As Object = DirMain.oLan.Item("302").ToString.Trim.Replace("%d", DirMain.dTo.Day.ToString.Trim).Replace("%m", DirMain.dTo.Month.ToString.Trim).Replace("%y", DirMain.dTo.Year.ToString.Trim)
        Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
        Dim clsprint As New clsprint(getGrid.GetForm, strFile, Nothing)
        clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
        clsprint.oVar = DirMain.oVar
        clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
        clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
        clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(_date))
        clsprint.oRpt.SetParameterValue("r_ma_vt", Strings.Trim(DirMain.fPrint.txtMa_vt.Text))
        clsprint.oRpt.SetParameterValue("r_ten_vt", Strings.Trim(DirMain.fPrint.lblTen_vt.Text))
        Try
            clsprint.oRpt.SetParameterValue("r_dvt", Strings.Trim(StringType.FromObject(DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Item(2).Item("dvt"))))
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            clsprint.oRpt.SetParameterValue("r_dvt", "")
            ProjectData.ClearProjectError()
        End Try
        Try
            clsprint.oRpt.SetParameterValue("r_tong_nt", RuntimeHelpers.GetObjectValue(DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Item(0).Item("tien_nt")))
        Catch exception5 As Exception
            ProjectData.SetProjectError(exception5)
            Dim exception2 As Exception = exception5
            Try
                clsprint.oRpt.SetParameterValue("r_tong_nt", "0")
            Catch exception6 As Exception
                ProjectData.SetProjectError(exception6)
                Dim exception3 As Exception = exception6
                ProjectData.ClearProjectError()
            End Try
            ProjectData.ClearProjectError()
        End Try
        Try
            clsprint.oRpt.SetParameterValue("h_gia_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("903")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
            clsprint.oRpt.SetParameterValue("h_tien_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("904")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
        Catch exception7 As Exception
            ProjectData.SetProjectError(exception7)
            Dim exception4 As Exception = exception7
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

    Private Sub ReportProc(ByVal nIndex As Integer)
        Select Case nIndex
            Case 0
                If (StringType.StrCmp(StringType.FromChar(DirMain.cForm), "2", False) = 0) Then
                    DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Trim(StringType.FromObject(DirMain.oLan.Item("901")))
                End If
                DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text, "%s", (Strings.Trim(DirMain.fPrint.txtMa_vt.Text) & " - " & Strings.Trim(DirMain.fPrint.lblTen_vt.Text)), 1, -1, CompareMethod.Binary)
                DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Trim(DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text)
                Exit Select
            Case 2
                DirMain.Print(0)
                Exit Select
            Case 3
                DirMain.Print(1)
                Exit Select
        End Select
    End Sub

    Public Sub ShowReport()
        Dim num As Integer
        Dim num2 As Integer
        Try
            ProjectData.ClearProjectError()
            num2 = 1
            'Dim str As String = StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj((StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(("EXEC fs_TransListOfItem" & DirMain.oxInv.xStore), Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vt.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_khon.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vv.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nx.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "ma_gd"), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "loai_ct"), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "ma_ct"), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvFrom.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvTo.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))), ObjectType.AddObj(ObjectType.AddObj(", '", Reg.GetRegistryKey("Language")), "'"))), ObjectType.AddObj(", ", DirMain.oLen.Item("so_ct")))) & ", '" & StringType.FromChar(DirMain.cForm) & "'"), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvtables")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin1")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin2")), ""))))
            Dim str As String = "EXEC sp16_inbk2_plan" & DirMain.oxInv.xStore
            str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_khon.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vv.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nx.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "ma_gd"), "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "loai_ct"), "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridTransCode(DirMain.fPrint.grdTransCode, "ma_ct"), "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvFrom.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvTo.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
            str += ", '" + Reg.GetRegistryKey("Language") + "'"
            str += ", " + DirMain.oLen.Item("so_ct")
            str += ", '" + StringType.FromChar(DirMain.cForm) + "'"
            str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvtables")), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin1")), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.drAdvFilter.Item("cadvjoin2")), "")
            Dim expression As String = StringType.FromObject(Interaction.IIf((StringType.StrCmp(Strings.Trim(DirMain.oAdvFilter.GetAdvSelectKey), "", False) = 0), "1=1", DirMain.oAdvFilter.GetAdvSelectKey))
            str = (str & ",'" & Strings.Replace(expression, "'", "''", 1, -1, CompareMethod.Binary) & "'")
            DirMain.oDirFormLib = New reportformlib("0111111111")
            oDirFormLib.sysConn = DirMain.sysConn
            oDirFormLib.appConn = DirMain.appConn
            oDirFormLib.oLan = DirMain.oLan
            oDirFormLib.oLen = DirMain.oLen
            oDirFormLib.oVar = DirMain.oVar
            oDirFormLib.SysID = DirMain.SysID
            oDirFormLib.cForm = DirMain.SysID
            oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
            oDirFormLib.strAliasReports = "inbk2"
            oDirFormLib.Init()
            oDirFormLib.strSQLRunReports = str
            AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            oDirFormLib.Show()
            RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            DirMain.oDirFormLib = Nothing
        Catch ex As Exception
            Msg.Alert(StringType.FromObject(DirMain.oLan.Item("900")), 2)
        End Try
        If (num <> 0) Then
            ProjectData.ClearProjectError()
        End If
    End Sub


    ' Fields
    Public appConn As SqlConnection
    Public cForm As Char
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
    Public rpTable As DataTable
    Public strAccount As String
    Public strAccountRef As String
    Private strCustID As String
    Private strCustName As String
    Public strUnit As String
    Public sysConn As SqlConnection
    Public SysID As String
End Module

