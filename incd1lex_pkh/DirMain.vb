Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports libscommon
Imports libscontrol
Imports libscontrol.reportformlib
Imports System.Windows.Forms

Namespace incd1lex
    <StandardModule>
    Friend NotInheritable Class DirMain
        ' Methods
        <STAThread>
        Public Shared Sub main()
            If Not BooleanType.FromObject(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0))) Then
                DirMain.sysConn = Sys.GetSysConn
                If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(DirMain.sysConn, "Access")) Then
                    DirMain.sysConn.Close()
                    DirMain.sysConn = Nothing
                Else
                    Control.CheckForIllegalCrossThreadCalls = False
                    DirMain.appConn = Sys.GetConn
                    Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                    Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                    Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                    DirMain.SysID = "ItemLotStock_pkh"
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
            Dim obj2 As Object = Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301"))), "%d1", StringType.FromDate(DirMain.dFrom), 1, -1, CompareMethod.Binary)), "%d2", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
            Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
            Dim clsprint As New clsprint(getGrid.GetForm, strFile, Nothing)
            clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
            clsprint.oVar = DirMain.oVar
            clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
            clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(obj2))
            clsprint.oRpt.SetParameterValue("r_in_tong_sl", RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbPrintAmtTotal.SelectedValue))
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

        Private Shared Sub ReportDetailProc(ByVal nIndex As Integer)
            If (nIndex = 0) Then
                DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text, "%s", Strings.Trim(DirMain.strMa_lo), 1, -1, CompareMethod.Binary)
            End If
        End Sub

        Private Shared Sub ReportProc(ByVal nIndex As Integer)
            Select Case nIndex
                Case 1
                    If Not Information.IsNothing(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow) Then
                        If (ObjectType.ObjTst(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow.Item("systotal"), 1, False) <> 0) Then
                            Return
                        End If
                        Dim curDataRow As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow
                        If (Information.IsDBNull(RuntimeHelpers.GetObjectValue(curDataRow.Item("Ma_lo"))) Or Information.IsDBNull(RuntimeHelpers.GetObjectValue(curDataRow.Item("Ma")))) Then
                            Return
                        End If
                        DirMain.strMa_lo = Strings.Trim(StringType.FromObject(curDataRow.Item("Ma_lo")))
                        Dim str As String
                        str = " AND " + DirMain.subQuery
                        str += Interaction.IIf((StringType.StrCmp(Strings.Trim(StringType.FromObject(curDataRow.Item("Ma_vt"))), "", False) <> 0), ObjectType.AddObj(" AND a.ma_vt = ", Sql.ConvertVS2SQLType(Strings.Trim(StringType.FromObject(curDataRow.Item("Ma_vt"))), "")), "")
                        str += Interaction.IIf((StringType.StrCmp(Strings.Trim(StringType.FromObject(curDataRow.Item("Ma_lo"))), "", False) <> 0), ObjectType.AddObj(" AND a.ma_lo = ", Sql.ConvertVS2SQLType(Strings.Trim(StringType.FromObject(curDataRow.Item("Ma_lo"))), "")), "")
                        str += IIf(fPrint.txtMa_kho.Text = "", "", " AND a.ma_kho like '" + fPrint.txtMa_kho.Text.Trim.Replace("'", "''") + "%'")
                        Dim str3 As String = ""
                        Dim cString As String = "sl_nhap, tien_nhap, tien_nt_n, sl_xuat, tien_xuat, tien_nt_x"
                        Dim num2 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                        Dim i As Integer = 1
                        Do While (i <= num2)
                            Dim str4 As String = Strings.Trim(Fox.GetWordNum(cString, i, ","c))
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(curDataRow.Item(str4))) Then
                                str3 = (str3 & "0,")
                            Else
                                str3 = (str3 & Strings.Trim(StringType.FromObject(curDataRow.Item(str4))) & ", ")
                            End If
                            i += 1
                        Loop
                        curDataRow = Nothing
                        Dim str2 As String = "'" + Reg.GetRegistryKey("Language") + "'"
                        str2 += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
                        str2 += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")
                        str2 += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")
                        str2 += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
                        str2 += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbTinh_dc.SelectedValue), "")
                        str2 += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_lo.Text, "")
                        str2 += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvtables")), "")
                        str2 += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin1")), "")
                        str2 += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin2")), "")
                        str2 += ",'" + Strings.Replace(StringType.FromObject(ObjectType.AddObj(Interaction.IIf((StringType.StrCmp(Strings.Trim(DirMain.oAdvFilter.GetAdvSelectKey), "", False) = 0), "1=1", DirMain.oAdvFilter.GetAdvSelectKey), str)), "'", "''", 1, -1, CompareMethod.Binary) + "'"
                        DirMain.oDirFormDetailLib = New reportformlib("0111110001")
                        Dim oDirFormDetailLib As reportformlib = DirMain.oDirFormDetailLib
                        oDirFormDetailLib.sysConn = DirMain.sysConn
                        oDirFormDetailLib.appConn = DirMain.appConn
                        oDirFormDetailLib.oLan = DirMain.oLan
                        oDirFormDetailLib.oLen = DirMain.oLen
                        oDirFormDetailLib.oVar = DirMain.oVar
                        oDirFormDetailLib.SysID = DirMain.SysID
                        oDirFormDetailLib.cForm = "StockSummaryDetailEx"
                        oDirFormDetailLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
                        oDirFormDetailLib.strAliasReports = "incd1d"
                        oDirFormDetailLib.Init()
                        oDirFormDetailLib.strSQLRunReports = ("spStockSummaryDetailLEx_pkd" & DirMain.oxInv.xStore & str2)
                        RemoveHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                        AddHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                        oDirFormDetailLib.Show()
                        RemoveHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                        AddHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                        oDirFormDetailLib = Nothing
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

        Public Shared Sub ShowReport()
            DirMain.strQuery = ("EXEC spStockSummaryByItemLot_pkh" & DirMain.oxInv.xStore)
            DirMain.strQuery = StringType.FromObject(ObjectType.AddObj(DirMain.strQuery, Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")))
            DirMain.strQuery = StringType.FromObject(ObjectType.AddObj(DirMain.strQuery, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, ""))))
            DirMain.strQuery = StringType.FromObject(ObjectType.AddObj(DirMain.strQuery, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, ""))))
            DirMain.strQuery = StringType.FromObject(ObjectType.AddObj(DirMain.strQuery, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_lo.Text, ""))))
            DirMain.strQuery = StringType.FromObject(ObjectType.AddObj(DirMain.strQuery, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, ""))))
            DirMain.strQuery = StringType.FromObject(ObjectType.AddObj(DirMain.strQuery, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbTinh_dc.SelectedValue), ""))))
            DirMain.strQuery = StringType.FromObject(ObjectType.AddObj(DirMain.strQuery, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbGroup.SelectedValue), ""))))
            DirMain.strQuery = StringType.FromObject(ObjectType.AddObj(DirMain.strQuery, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), ""))))
            DirMain.strQuery = StringType.FromObject(ObjectType.AddObj(DirMain.strQuery, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.cbbQtycol.SelectedValue), ""))))
            DirMain.strQuery = StringType.FromObject(ObjectType.AddObj(DirMain.strQuery, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvtables")), ""))))
            DirMain.strQuery = StringType.FromObject(ObjectType.AddObj(DirMain.strQuery, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin1")), ""))))
            DirMain.strQuery = StringType.FromObject(ObjectType.AddObj(DirMain.strQuery, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin2")), ""))))
            DirMain.subQuery = ""
            If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtMa_lo.Text), "", False) <> 0) Then
                DirMain.subQuery = (DirMain.subQuery & " AND dmlo.ma_lo LIKE '" & Strings.Trim(DirMain.fPrint.txtMa_lo.Text) & "%'")
            End If
            If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtMa_vt.Text), "", False) <> 0) Then
                DirMain.subQuery = (DirMain.subQuery & " AND dmvt.ma_vt = '" & Strings.Trim(DirMain.fPrint.txtMa_vt.Text).Replace("'", "''") + "'")
            End If
            If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtLoai_vt.Text), "", False) <> 0) Then
                DirMain.subQuery = (DirMain.subQuery & " AND dmvt.loai_vt LIKE '" & Strings.Trim(DirMain.fPrint.txtLoai_vt.Text) & "%'")
            End If
            If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtNh_vt.Text), "", False) <> 0) Then
                DirMain.subQuery = (DirMain.subQuery & " AND dmvt.nh_vt1 LIKE '" & Strings.Trim(DirMain.fPrint.txtNh_vt.Text) & "%'")
            End If
            If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtNh_vt2.Text), "", False) <> 0) Then
                DirMain.subQuery = (DirMain.subQuery & " AND dmvt.nh_vt2 LIKE '" & Strings.Trim(DirMain.fPrint.txtNh_vt2.Text) & "%'")
            End If
            If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtNh_vt3.Text), "", False) <> 0) Then
                DirMain.subQuery = (DirMain.subQuery & " AND dmvt.nh_vt3 LIKE '" & Strings.Trim(DirMain.fPrint.txtNh_vt3.Text) & "%'")
            End If
            DirMain.subQuery = StringType.FromObject(ObjectType.AddObj(Interaction.IIf((StringType.StrCmp(Strings.Trim(DirMain.oAdvFilter.GetAdvSelectKey), "", False) = 0), "1=1", DirMain.oAdvFilter.GetAdvSelectKey), DirMain.subQuery))
            DirMain.strTest = (DirMain.strQuery & ",'" & Strings.Replace((DirMain.subQuery & " and 1=0 "), "'", "''", 1, -1, CompareMethod.Binary) & "'")
            DirMain.strQuery = (DirMain.strQuery & ",'" & Strings.Replace(DirMain.subQuery, "'", "''", 1, -1, CompareMethod.Binary) & "'")
            'Dim ds As New DataSet
            'Sql.SQLRetrieve(appConn, strQuery, "report", ds)
            'ds.WriteXmlSchema("D:\LocalCustomer\Uspharma4.0\Rpt\incd1lex.xsd")
            Try
                Sql.SQLExecute((DirMain.appConn), DirMain.strTest)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Msg.Alert(StringType.FromObject(DirMain.oLan.Item("500")), 2)
                ProjectData.ClearProjectError
                Return
            End Try
            DirMain.oDirFormLib = New reportformlib("1111111111")
            Dim oDirFormLib As reportformlib = DirMain.oDirFormLib
            oDirFormLib.sysConn = DirMain.sysConn
            oDirFormLib.appConn = DirMain.appConn
            oDirFormLib.oLan = DirMain.oLan
            oDirFormLib.oLen = DirMain.oLen
            oDirFormLib.oVar = DirMain.oVar
            oDirFormLib.SysID = DirMain.SysID
            oDirFormLib.cForm = DirMain.SysID
            oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
            oDirFormLib.strAliasReports = "incd1ex"
            oDirFormLib.Init
            oDirFormLib.strSQLRunReports = DirMain.strQuery
            AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            oDirFormLib.Show
            RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            oDirFormLib = Nothing
            DirMain.oDirFormLib = Nothing
        End Sub


        ' Fields
        Public Shared appConn As SqlConnection
        Public Shared dFrom As DateTime
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
        Public Shared strMa_lo As String
        Private Shared strQuery As String
        Private Shared strTest As String
        Public Shared strUnit As String
        Private Shared subQuery As String
        Public Shared sysConn As SqlConnection
        Public Shared SysID As String
    End Class
End Namespace

