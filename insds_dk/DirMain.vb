Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports libscommon
Imports libscontrol
Imports libscontrol.reportformlib

Namespace insds
    <StandardModule>
    Friend NotInheritable Class DirMain
        ' Methods
        <STAThread>
        Public Shared Sub main(ByVal CmdArgs As String())
            If Not BooleanType.FromObject(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0))) Then
                DirMain.sysConn = Sys.GetSysConn
                If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(DirMain.sysConn, "Access")) Then
                    DirMain.sysConn.Close()
                    DirMain.sysConn = Nothing
                Else
                    DirMain.cForm = CharType.FromString(Strings.Trim(Fox.GetWordNum(Strings.Trim("2"), 1, "#"c)))
                    DirMain.iNgay = IntegerType.FromString(Strings.Trim(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 1, "#"c)))
                    DirMain.appConn = Sys.GetConn
                    Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                    Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                    Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                    DirMain.SysID = "StockBalanceS"
                    Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                    DirMain.ReportRow = DirectCast(Sql.GetRow((DirMain.sysConn), "reports", StringType.FromObject(ObjectType.AddObj("form=", Sql.ConvertVS2SQLType(DirMain.SysID, "")))), DataRow)
                    DirMain.PrintReport()
                    DirMain.rpTable = Nothing
                End If
            End If
        End Sub

        Private Shared Sub Print(ByVal nType As Integer)
            Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
            Dim objectValue As Object = Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301"))), "%d", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
            Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
            Dim clsprint As New clsprint(getGrid.GetForm, strFile, Nothing)
            clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
            clsprint.oVar = DirMain.oVar
            clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
            Dim args As Object() = New Object() {RuntimeHelpers.GetObjectValue(objectValue)}
            Dim copyBack As Boolean() = New Boolean() {True}
            If copyBack(0) Then
                objectValue = RuntimeHelpers.GetObjectValue(args(0))
            End If
            clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Strings), "UCase", args, Nothing, copyBack)))
            If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtMa_kho.Text), "", False) <> 0) Then
                clsprint.oRpt.SetParameterValue("r_tat_ca_kho", (Strings.Trim(DirMain.fPrint.txtMa_kho.Text) & " - " & Strings.Trim(DirMain.fPrint.lblTen_kho.Text)))
            End If
            clsprint.oRpt.SetParameterValue("h_ngay_lc", Strings.Replace(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "UCase", New Object() {RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("904"))}, Nothing, Nothing)), "%D", DirMain.fPrint.txtso_ngay.Text, 1, -1, CompareMethod.Binary))
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
                clsprint.ShowReports
            End If
            clsprint.oRpt.Close
            getGrid = Nothing
        End Sub

        Public Shared Sub PrintReport()
            DirMain.rpTable = clsprint.InitComboReport(DirMain.sysConn, DirMain.fPrint.cboReports, DirMain.SysID)
            DirMain.fPrint.ShowDialog()
            DirMain.fPrint.Dispose()
            DirMain.sysConn.Close()
            DirMain.appConn.Close()
        End Sub

        Private Shared Sub ReportProc(ByVal nIndex As Integer)
            Select Case nIndex
                Case 0
                    If (StringType.StrCmp(StringType.FromChar(DirMain.cForm), "1", False) = 0) Then
                        DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Trim(StringType.FromObject(DirMain.oLan.Item("901")))
                    End If
                    Exit Select
                Case 2
                    DirMain.Print(0)
                    Exit Select
                Case 3
                    DirMain.Print(1)
                    Exit Select
            End Select
        End Sub

        Public Shared Sub ShowReport()
            Dim str As String = StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj((StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(("EXEC spStockBalanceSlow2Rotate_dk" & DirMain.oxInv.xStore), Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtso_ngay.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vt.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt2.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt3.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbGroup.SelectedValue), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))) & ", '" & StringType.FromChar(DirMain.cForm) & "'"), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvtables")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin1")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin2")), ""))))
            Dim expression As String = StringType.FromObject(Interaction.IIf((StringType.StrCmp(Strings.Trim(DirMain.oAdvFilter.GetAdvSelectKey), "", False) = 0), "1=1", DirMain.oAdvFilter.GetAdvSelectKey))
            Dim tcSQL As String = (str & ",'" & Strings.Replace((expression & " and 1=0 "), "'", "''", 1, -1, CompareMethod.Binary) & "'")
            str = (str & ",'" & Strings.Replace(expression, "'", "''", 1, -1, CompareMethod.Binary) & "'")
            Try
                Sql.SQLExecute((DirMain.appConn), tcSQL)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Msg.Alert(StringType.FromObject(DirMain.oLan.Item("900")), 2)
                ProjectData.ClearProjectError
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
            oDirFormLib.strAliasReports = "insdxxx"
            oDirFormLib.Init
            oDirFormLib.strSQLRunReports = str
            AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            oDirFormLib.Show
            RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            DirMain.oDirFormLib = Nothing
        End Sub


        ' Fields
        Public Shared appConn As SqlConnection
        Public Shared cForm As Char
        Public Shared dTo As DateTime
        Public Shared fPrint As frmFilter = New frmFilter
        Public Shared iNgay As Integer
        Public Shared oAdvFilter As clsAdvFilter
        Private Shared oDirFormDetail4DetailLib As reportformlib
        Private Shared oDirFormDetailLib As reportformlib
        Public Shared oDirFormLib As reportformlib
        Public Shared oLan As Collection = New Collection
        Public Shared oLen As Collection = New Collection
        Public Shared oOption As Collection = New Collection
        Public Shared oVar As Collection = New Collection
        Public Shared oxInv As xInv
        Public Shared ReportRow As DataRow
        Public Shared rpTable As DataTable
        Public Shared strMa_vt As String
        Public Shared strUnit As String
        Public Shared sysConn As SqlConnection
        Public Shared SysID As String
    End Class
End Namespace

