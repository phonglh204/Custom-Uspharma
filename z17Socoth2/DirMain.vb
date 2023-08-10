Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports libscommon
Imports libscontrol
Imports libscontrol.reportformlib

Namespace z16Socoth1
    Module DirMain
        ' Methods
        <STAThread()>
        Public Sub main(ByVal CmdArgs As String())
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
                    DirMain.SysID = "z17Socoth2"
                    Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                    DirMain.ReportRow = DirectCast(Sql.GetRow((DirMain.sysConn), "reports", StringType.FromObject(ObjectType.AddObj("form=", Sql.ConvertVS2SQLType(DirMain.SysID, "")))), DataRow)
                    DirMain.PrintReport()
                    DirMain.rpTable = Nothing
                End If
            End If
        End Sub

        Private Sub Print(ByVal nType As Integer)
            Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
            Dim obj2 As Object = Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301"))), "%d1", StringType.FromDate(DirMain.fPrint.txtDFrom.Value), 1, -1, CompareMethod.Binary)
            obj2 = Strings.Replace(StringType.FromObject(obj2), "%d2", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
            Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
            Dim clsprint As New clsprint(getGrid.GetForm, strFile, Nothing)
            clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
            clsprint.oVar = DirMain.oVar
            clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
            clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(obj2))
            Try
                clsprint.oRpt.SetParameterValue("h_tien_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("903")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
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

        Private Sub ReportProc(ByVal nIndex As Integer)
            Select Case nIndex
                Case 1
                    Detail1()
                    Return
                Case 2
                    DirMain.Print(0)
                    Exit Select
                Case 3
                    DirMain.Print(1)
                    Exit Select
            End Select
        End Sub
        Private Sub Detail1()
            If Not Information.IsNothing(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow) Then
                Dim curDataRow As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(curDataRow.Item("Ma_vt"))) Then
                    Return
                End If
                If curDataRow.Item("Ma_vt").ToString.Trim = "" Then
                    Return
                End If
                Dim str As String = "EXEC sp17Socoth2_detail "
                str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
                str += "," + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")
                str += ", " + Sql.ConvertVS2SQLType(curDataRow.Item("Ma_vt").ToString.Trim, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_kh1.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_kh2.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_kh3.Text, "")
                DirMain.oDirFormDetailLib = New reportformlib("0111110001")
                oDirFormDetailLib.sysConn = DirMain.sysConn
                oDirFormDetailLib.appConn = DirMain.appConn
                oDirFormDetailLib.oLan = DirMain.oLan
                oDirFormDetailLib.oLen = DirMain.oLen
                oDirFormDetailLib.oVar = DirMain.oVar
                oDirFormDetailLib.SysID = DirMain.SysID
                oDirFormDetailLib.cForm = "z17Socoth2_detail"
                oDirFormDetailLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
                oDirFormDetailLib.strAliasReports = "z17Socoth2_detail"
                oDirFormDetailLib.Init()
                oDirFormDetailLib.strSQLRunReports = str
                RemoveHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                AddHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                oDirFormDetailLib.Show()
                RemoveHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                AddHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                oDirFormDetailLib = Nothing
            End If
        End Sub
        Private Sub ReportDetailProc(ByVal nIndex As Integer)
            If (nIndex = 0) Then
                DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text, "%s", Strings.Trim(DirMain.strMa_vt), 1, -1, CompareMethod.Binary)
            End If
        End Sub

        Public Sub ShowReport()
            Try
                Dim str As String = "EXEC sp17Socoth2 "
                str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
                str += "," + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vt.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt2.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt3.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.strGroups, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")
                Dim expression As String = StringType.FromObject(Interaction.IIf((StringType.StrCmp(Strings.Trim(DirMain.oAdvFilter.GetAdvSelectKey), "", False) = 0), "1=1", DirMain.oAdvFilter.GetAdvSelectKey))
                str = (str & ",'" & Strings.Replace(expression, "'", "''", 1, -1, CompareMethod.Binary) & "'")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_kh1.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_kh2.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_kh3.Text, "")
                'Dim ds As New DataSet
                'Sql.SQLRetrieve(appConn, str, "report", ds)
                'ds.WriteXmlSchema(Reg.GetRegistryKey("ReportDir") + "z17Socoth2.xsd")
                DirMain.oDirFormLib = New reportformlib("1000011111")
                oDirFormLib.sysConn = DirMain.sysConn
                oDirFormLib.appConn = DirMain.appConn
                oDirFormLib.oLan = DirMain.oLan
                oDirFormLib.oLen = DirMain.oLen
                oDirFormLib.oVar = DirMain.oVar
                oDirFormLib.SysID = DirMain.SysID
                oDirFormLib.cForm = DirMain.SysID
                oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
                oDirFormLib.strAliasReports = "insd2"
                oDirFormLib.Init()
                oDirFormLib.strSQLRunReports = str
                AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                oDirFormLib.Show()
                RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                DirMain.oDirFormLib = Nothing
            Catch ex As Exception
                Msg.Alert(StringType.FromObject(DirMain.oLan.Item("900")), 2)
            End Try
        End Sub


        ' Fields
        Public appConn As SqlConnection
        'Public cForm As Char
        Public dTo As DateTime
        Public fPrint As frmFilter = New frmFilter
        Public oAdvFilter As clsAdvFilter
        Private oDirFormDetail4DetailLib As reportformlib
        Private oDirFormDetailLib As reportformlib
        Public oDirFormLib As reportformlib
        Public oLan As Collection = New Collection
        Public oLen As Collection = New Collection
        Public oOption As Collection = New Collection
        Public oVar As Collection = New Collection
        Public oxInv As xInv
        Public ReportRow As DataRow
        Public rpTable As DataTable
        Public strGroups As String
        Public strMa_vt As String
        Public strUnit As String
        Public sysConn As SqlConnection
        Public SysID As String
    End Module
End Namespace

