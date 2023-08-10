Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon
Imports libscontrol.reportformlib

Namespace v20inbcTimeLot
    <StandardModule>
    Friend NotInheritable Class DirMain
        ' Methods
        Private Shared Sub Fill2GridHorizontalReport()
            Dim str2 As String = ""
            Dim num As Integer
            Dim clspivot As New clspivot
            clspivot.FieldKey = "ky1"
            clspivot.FieldSearch = "xPivot"
            clspivot.Columns = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex), Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "fields", "fields2"))}, Nothing, Nothing)))
            clspivot.ColumnsAlias = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex), Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "fields", "fields2"))}, Nothing, Nothing)))
            clspivot.DataTable = DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Table
            clspivot.Headers = "ten_ky"
            clspivot.Headers2 = "ten_ky"
            clspivot.isShowOrderNo = StringType.FromBoolean(False)
            Dim tbs As New DataGridTableStyle
            DirMain.ewdv.Table = clspivot.GetPivotTable
            Dim cFields As String = StringType.FromObject(DirMain.oLan.Item("920"))
            Dim cHeaders As String = StringType.FromObject(DirMain.oLan.Item("921"))
            Dim str9 As String = StringType.FromObject(DirMain.oLan.Item("977"))
            Dim str7 As String = StringType.FromObject(DirMain.oLan.Item("923"))
            Dim cFieldName As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex), Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "fields", "fields2"))}, Nothing, Nothing)))
            Dim cStringBackward As String = (Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex), Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "headers", "headers2"))}, Nothing, Nothing))) & " ")
            Dim cFieldWidths As String = Strings.RTrim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("widths")))
            Dim cString As String = Strings.RTrim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("formats")))
            Dim num4 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            num = 1
            Do While (num <= num4)
                str2 = StringType.FromObject(ObjectType.AddObj(str2, ObjectType.AddObj(", ", DirMain.oDirFormLib.GetClsreports.oOptions.Item(Strings.Trim(Fox.GetWordNum(cString, num, ","c))))))
                num += 1
            Loop
            str2 = str2.Substring((str2.IndexOf(",") + 1))
            cFields = (cFields & clspivot.GetFieldsNameMix(cFieldName))
            cHeaders = (cHeaders & clspivot.GetHeadersNameMix(IntegerType.FromObject(Fox.GetWordCount(cStringBackward, ","c)), "", cStringBackward))
            str9 = StringType.FromObject(ObjectType.AddObj(str9, clspivot.GetWidthsMix(cFieldWidths)))
            str7 = StringType.FromObject(ObjectType.AddObj(str7, clspivot.GetFormatsMix(str2)))
            Dim obj2 As Object = &HFE
            Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn((IntegerType.FromObject(obj2) + 1) - 1) {}
            Dim num3 As Integer = IntegerType.FromObject(ObjectType.SubObj(obj2, 1))
            num = 0
            Do While (num <= num3)
                cols(num) = New DataGridTextBoxColumn
                num += 1
            Loop
            Fill2Grid.Fill((DirMain.ewdv), (DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid), (tbs), (cols), cFields, cHeaders, str7, str9)
            Dim num2 As Integer = IntegerType.FromObject(ObjectType.SubObj(obj2, 1))
            num = 0
            Do While (num <= num2)
                cols(num).NullText = ""
                num += 1
            Loop
            Dim getGrid As DataGrid = DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid
            getGrid.Height = (getGrid.Height + &H15)
        End Sub

        <STAThread>
        Public Shared Sub main(ByVal CmdArgs As String())
            If Not BooleanType.FromObject(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0))) Then
                DirMain.sysConn = Sys.GetSysConn
                If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(DirMain.sysConn, "Access")) Then
                    DirMain.sysConn.Close()
                    DirMain.sysConn = Nothing
                Else
                    DirMain.cLan = CharType.FromString(Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("Language"))))
                    DirMain.cForm = CharType.FromString(Strings.Trim(Fox.GetWordNum(Strings.Trim("2"), 1, "#"c)))
                    DirMain.iTime = IntegerType.FromString(Strings.Trim(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 1, "#"c)))
                    DirMain.iTime1 = IntegerType.FromString(Strings.Trim(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 2, "#"c)))
                    DirMain.iTime2 = IntegerType.FromString(Strings.Trim(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 3, "#"c)))
                    DirMain.appConn = Sys.GetConn
                    Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                    Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                    Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                    DirMain.SysID = "v20inbcTimeLot"
                    Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                    DirMain.ReportRow = DirectCast(Sql.GetRow((DirMain.sysConn), "reports", StringType.FromObject(ObjectType.AddObj("form=", Sql.ConvertVS2SQLType(DirMain.SysID, "")))), DataRow)
                    DirMain.PrintReport()
                    DirMain.rpTable = Nothing
                End If
            End If
        End Sub

        Private Shared Sub Print(ByVal nType As Integer)
            Dim str2 As String
            Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
            Dim expression As String = StringType.FromObject(DirMain.oLan.Item("301"))
            Dim str As String = StringType.FromObject(DirMain.oLan.Item("955"))
            expression = Strings.Replace(expression, "%d", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
            If (StringType.StrCmp(DirMain.strPKy, "1", False) = 0) Then
                str2 = StringType.FromObject(DirMain.oLan.Item("911"))
            Else
                str2 = StringType.FromObject(DirMain.oLan.Item("922"))
            End If
            str = (Strings.Trim(str) & " " & Strings.Trim(DirMain.fPrint.txtSo_ky.Value.ToString))
            str2 = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(DirMain.oLan.Item("933"), " "), str2))
            Dim str5 As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(DirMain.oLan.Item("944"), " "), DirMain.itg.ToString))
            Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
            Dim clsprint As New clsprint(getGrid.GetForm, strFile, Nothing)
            clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
            clsprint.oVar = DirMain.oVar
            clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
            clsprint.oRpt.SetParameterValue("t_date", expression)
            clsprint.oRpt.SetParameterValue("h_ky", str2)
            clsprint.oRpt.SetParameterValue("h_Time", str5)
            clsprint.oRpt.SetParameterValue("h_so_ky", str)
            If (nType = 0) Then
                clsprint.PrintReport(1)
                clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
            Else
                clsprint.ShowReports()
            End If
            clsprint.oRpt.Close()
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
                    Try
                        DirMain.Fill2GridHorizontalReport()
                    Catch exception1 As Exception
                        ProjectData.SetProjectError(exception1)
                        Dim exception As Exception = exception1
                        ProjectData.ClearProjectError()
                    End Try
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
            Dim str As String = StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj((("EXEC spStockByTimeLot_dk" & DirMain.oxInv.xStore) & " '" & StringType.FromChar(DirMain.cLan) & "'"), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtKy.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtTime.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtSo_ky.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vt.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtloai_vt.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbGroup.SelectedValue), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvtables")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin1")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin2")), ""))))
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
            DirMain.oDirFormLib = New reportformlib("0000011111")
            oDirFormLib.sysConn = DirMain.sysConn
            oDirFormLib.appConn = DirMain.appConn
            oDirFormLib.oLan = DirMain.oLan
            oDirFormLib.oLen = DirMain.oLen
            oDirFormLib.oVar = DirMain.oVar
            oDirFormLib.SysID = DirMain.SysID
            oDirFormLib.cForm = DirMain.SysID
            oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
            oDirFormLib.strAliasReports = "inTimeLot"
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
        Public Shared cLan As Char
        Public Shared dTo As DateTime
        Public Shared ewdv As DataView = New DataView
        Public Shared fPrint As frmFilter = New frmFilter
        Public Shared itg As Integer
        Public Shared iTime As Integer
        Public Shared iTime1 As Integer
        Public Shared iTime2 As Integer
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
        Public Shared strPKy As String
        Public Shared strUnit As String
        Public Shared sysConn As SqlConnection
        Public Shared SysID As String
    End Class
End Namespace

