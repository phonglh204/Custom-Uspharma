Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports libscommon
Imports libscontrol
Imports libscontrol.voucherseachlib
Imports libscontrol.reportformlib

Namespace z18thbom
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
                    DirMain.SysID = "z18thbom"
                    Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                    'Dim str As String = (Process.GetCurrentProcess.ProcessName & ".Exe" & " " & Strings.Trim(Interaction.Command))
                    'Dim cKey As String = ("UPPER(Exe) = '" & Strings.UCase(str) & "'")
                    'DirMain.strTitle = StringType.FromObject(Sql.GetValue((DirMain.sysConn), "command", StringType.FromObject(ObjectType.AddObj("bar", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))), cKey))
                    'DirMain.fPrint.Text = DirMain.strTitle
                    DirMain.PrintReport()
                    DirMain.rpTable = Nothing
                    DirMain.dsRT = Nothing
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
                DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text, "%s", (DirMain.strItemID & " - " & DirMain.strItemName), 1, -1, CompareMethod.Binary)
                DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Trim(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text)
            End If
        End Sub

        Private Sub ReportProc(ByVal nIndex As Integer)
            Select Case nIndex
                Case 0
                    'DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.FindForm.Text = DirMain.strTitle
                    Exit Select
                Case 1
                    If Not Information.IsNothing(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow) Then
                        Dim str As String
                        str = "a.ma_sp"
                        Dim curDataRow As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(curDataRow.Item("stt"))) Then
                            Return
                        End If
                        DirMain.strItemID = Strings.Trim(StringType.FromObject(curDataRow.Item("ma_vt")))
                        If (StringType.StrCmp(DirMain.strItemID, "", False) = 0) Then
                            Return
                        End If
                        DirMain.strItemName = StringType.FromObject(Sql.GetValue((DirMain.appConn), "dmvt", StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("language"), "V", False) = 0), "ten_vt", "ten_vt2")), ("RTRIM(ma_vt) = '" & DirMain.strItemID & "'")))
                        Dim str3 As String = ""
                        Dim cString As String = "so_luong, sl_xuat, tien, tien_xuat, tien_nt, tien_nt_x"
                        Dim num2 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                        Dim i As Integer = 1
                        Do While (i <= num2)
                            Dim str4 As String = Strings.Trim(Fox.GetWordNum(cString, i, ","c))
                            str3 = (str3 & Strings.Trim(StringType.FromObject(curDataRow.Item(str4))) & ", ")
                            i += 1
                        Loop
                        Dim strKey As String = DirMain.strKey
                        strKey = (String.Concat(New String() {strKey}) & " AND RTRIM(ma_vt) = '" & Strings.Trim(DirMain.strItemID) & "'")
                        'MsgBox(strKey)
                        'MsgBox(Strings.Trim(StringType.FromObject(curDataRow.Item("ma_sp"))))
                        strKey += " AND RTRIM(ma_sp)='" + Strings.Trim(StringType.FromObject(curDataRow.Item("ma_sp"))) + "'"
                        strKey += " AND LTRIM(RTRIM(so_lsx))='" + Strings.Trim(StringType.FromObject(curDataRow.Item("so_lsx"))) + "'"
                        DirMain.oDirFormDetailLib = New reportformlib("0111110001")
                        oDirFormDetailLib.sysConn = DirMain.sysConn
                        oDirFormDetailLib.appConn = DirMain.appConn
                        oDirFormDetailLib.oLan = DirMain.oLan
                        oDirFormDetailLib.oLen = DirMain.oLen
                        oDirFormDetailLib.oVar = DirMain.oVar
                        oDirFormDetailLib.SysID = DirMain.SysID
                        oDirFormDetailLib.cForm = "Detail"
                        oDirFormDetailLib.cCode = StringType.FromObject(Interaction.IIf((DirMain.fPrint.cboReports.SelectedIndex = 0), "201", "202"))
                        oDirFormDetailLib.strAliasReports = "glth1Detail"
                        oDirFormDetailLib.Init()
                        oDirFormDetailLib.strSQLRunReports = ("fs30_ReportDetailItem " & str3 & vouchersearchlibobj.ConvertLong2ShortStrings(strKey, 10))
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
            Dim str As String = "EXEC spz18thbom "
            str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
            str += "," + Sql.ConvertVS2SQLType(DirMain.fPrint.txtSo_lsx.Text, "")
            str += "," + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vt.Text, "")
            str += "," + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt1.Text, "")
            str += "," + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt2.Text, "")
            str += "," + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt3.Text, "")
            str += "," + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_bp.Text, "")
            str += "," + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_lo_sp.Text, "")
            str += "," + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text.Trim, "")
            DirMain.oDirFormLib = New reportformlib("1011111111")
            oDirFormLib.sysConn = DirMain.sysConn
            oDirFormLib.appConn = DirMain.appConn
            oDirFormLib.oLan = DirMain.oLan
            oDirFormLib.oLen = DirMain.oLen
            oDirFormLib.oVar = DirMain.oVar
            oDirFormLib.SysID = DirMain.SysID
            oDirFormLib.cForm = DirMain.SysID
            oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
            oDirFormLib.strAliasReports = "covvth1"
            oDirFormLib.Init()
            oDirFormLib.strSQLRunReports = str
            AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            oDirFormLib.Show()
            RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            DirMain.oDirFormLib = Nothing
        End Sub


        ' Fields
        Public appConn As SqlConnection
        Public cReasonCode As String
        Public dFrom As DateTime
        Public dsRT As DataSet
        Public dTo As DateTime
        Public fPrint As frmFilter = New frmFilter
        Private oDirFormDetailLib As reportformlib
        Private oDirFormLib As reportformlib
        Public oLan As Collection = New Collection
        Public oLen As Collection = New Collection
        Public oOption As Collection = New Collection
        Public oVar As Collection = New Collection
        Public rpTable As DataTable
        Public strItemID As String
        Public strItemName As String
        Public strKey As String
        Public strTitle As String
        Public sysConn As SqlConnection
        Public SysID As String
    End Module
End Namespace

