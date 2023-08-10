Imports libscommon
Imports libscontrol
Imports libscontrol.reportformlib
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices

Namespace z17taikiem
    <StandardModule>
    Friend NotInheritable Class DirMain
        ' Methods
        <STAThread>
        Public Shared Sub main(ByVal CmdArgs As String())
            If Not BooleanType.FromObject(RuntimeHelpers.GetObjectValue(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(RuntimeHelpers.GetObjectValue(Reg.GetRegistryKey("Customize")), "0", False) = 0)))) Then
                DirMain.sysConn = Sys.GetSysConn
                If ((ObjectType.ObjTst(RuntimeHelpers.GetObjectValue(Reg.GetRegistryKey("Customize")), "0", False) = 0) AndAlso Not Sys.CheckRights(DirMain.sysConn, "Access")) Then
                    DirMain.sysConn.Close()
                    DirMain.sysConn = Nothing
                Else
                    DirMain.cForm = CharType.FromString(Strings.Trim(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 1, "#"c)))
                    DirMain.appConn = Sys.GetConn
                    Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                    Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                    Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                    DirMain.SysID = "z17taikiem"
                    Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                    DirMain.ReportRow = DirectCast(Sql.GetRow((DirMain.sysConn), "reports", StringType.FromObject(RuntimeHelpers.GetObjectValue(ObjectType.AddObj("form=", RuntimeHelpers.GetObjectValue(Sql.ConvertVS2SQLType(DirMain.SysID, "")))))), DataRow)
                    DirMain.PrintReport()
                    DirMain.rpTable = Nothing
                End If
            End If
        End Sub

        Private Shared Sub Print(ByVal nType As Integer)
            Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(RuntimeHelpers.GetObjectValue(ObjectType.AddObj(RuntimeHelpers.GetObjectValue(ObjectType.AddObj(RuntimeHelpers.GetObjectValue(Reg.GetRegistryKey("ReportDir")), Strings.Trim(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))))), ".rpt")))
            Dim obj2 As Object = Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301")))), "%d", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
            Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
            Dim clsprint As New clsprint(getGrid.GetForm, strFile, Nothing)
            clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
            clsprint.oVar = DirMain.oVar
            clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
            clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(RuntimeHelpers.GetObjectValue(obj2)))
            If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtMa_kho.Text), "", False) <> 0) Then
                clsprint.oRpt.SetParameterValue("r_tat_ca_kho", (Strings.Trim(DirMain.fPrint.txtMa_kho.Text) & " - " & Strings.Trim(DirMain.fPrint.lblTen_kho.Text)))
            End If
            Try
                clsprint.oRpt.SetParameterValue("h_tien_vnd", Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("903"))), "%s", StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oOption.Item("m_ma_nt0"))), 1, -1, CompareMethod.Binary))
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim ex As Exception = exception1
                ProjectData.SetProjectError(ex)
                ProjectData.ClearProjectError()
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
                    If (StringType.StrCmp(StringType.FromChar(DirMain.cForm), "1", False) <> 0) Then
                        Exit Select
                    End If
                    DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Trim(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("901"))))
                    Return
                Case 1
                    Exit Select
                Case 2
                    DirMain.Print(0)
                    Return
                Case 3
                    DirMain.Print(1)
                    Exit Select
                Case Else
                    Return
            End Select
        End Sub

        Public Shared Sub ShowReport()
            Try
                Dim str As String = Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject((Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(("EXEC sp17taikiem_dk" & DirMain.oxInv.xStore), Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, ""))), Operators.AddObject(",", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vt.Text, "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt.Text, "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt2.Text, "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt3.Text, "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(DirMain.strGroups, "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))) & ", '" & StringType.FromChar(DirMain.cForm) & "'"), Operators.AddObject(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvtables"))), "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin1"))), "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin2"))), ""))))
                Dim expression As String = StringType.FromObject(RuntimeHelpers.GetObjectValue(Interaction.IIf((StringType.StrCmp(Strings.Trim(DirMain.oAdvFilter.GetAdvSelectKey), "", False) = 0), "1=1", DirMain.oAdvFilter.GetAdvSelectKey)))
                str = (str & ",'" & Strings.Replace(expression, "'", "''", 1, -1, CompareMethod.Binary) & "'")
                DirMain.oDirFormLib = New reportformlib("0011111111")
                DirMain.oDirFormLib.sysConn = DirMain.sysConn
                DirMain.oDirFormLib.appConn = DirMain.appConn
                DirMain.oDirFormLib.oLan = DirMain.oLan
                DirMain.oDirFormLib.oLen = DirMain.oLen
                DirMain.oDirFormLib.oVar = DirMain.oVar
                DirMain.oDirFormLib.SysID = DirMain.SysID
                DirMain.oDirFormLib.cForm = DirMain.SysID
                DirMain.oDirFormLib.cCode = Strings.Trim(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id"))))
                DirMain.oDirFormLib.strAliasReports = "insd2"
                DirMain.oDirFormLib.Init()
                DirMain.oDirFormLib.strSQLRunReports = str
                AddHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                DirMain.oDirFormLib.Show()
                RemoveHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                DirMain.oDirFormLib = Nothing
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Msg.Alert(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("900"))), 2)
                ProjectData.ClearProjectError
            End Try
        End Sub


        ' Fields
        Public Shared appConn As SqlConnection
        Public Shared cForm As Char
        Public Shared dTo As DateTime
        Public Shared fPrint As frmFilter = New frmFilter
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
        Public Shared strGroups As String
        Public Shared strMa_vt As String
        Public Shared strUnit As String
        Public Shared sysConn As SqlConnection
        Public Shared SysID As String
    End Class
End Namespace

