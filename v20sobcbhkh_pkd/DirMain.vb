Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports libscommon
Imports libscontrol
Imports libscontrol.reportformlib

Namespace v20sobcbhkh
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
                    DirMain.cLan = CharType.FromObject(Reg.GetRegistryKey("Language"))
                    DirMain.cVoucher = Strings.Trim(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 1, "#"c))
                    DirMain.appConn = Sys.GetConn
                    Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                    Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                    Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                    DirMain.SysID = "v20sobcbhkh"
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
            Dim obj2 As Object = Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301"))), "%dT", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)), "%dF", StringType.FromDate(DirMain.dFrom), 1, -1, CompareMethod.Binary)
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
                clsprint.ShowReports
            End If
            clsprint.oRpt.Close
            getGrid = Nothing
        End Sub

        Public Shared Sub PrintReport()
            DirMain.rpTable = clsprint.InitComboReport(DirMain.sysConn, DirMain.fPrint.cboReports, DirMain.SysID)
            DirMain.fPrint.ShowDialog
            DirMain.fPrint.Dispose
            DirMain.sysConn.Close
            DirMain.appConn.Close
        End Sub

        Private Shared Sub ReportDetailProc(ByVal nIndex As Integer)
            If (nIndex = 0) Then
                DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text, "%s", DirMain.strCustomer, 1, -1, CompareMethod.Binary)
            End If
        End Sub

        Private Shared Sub ReportProc(ByVal nIndex As Integer)
            Select Case nIndex
                Case 1
                    If Information.IsNothing(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow) Then
                        Return
                    End If
                    Dim curDataRow As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow
                    If BooleanType.FromObject(ObjectType.BitAndObj(Information.IsDBNull(RuntimeHelpers.GetObjectValue(curDataRow.Item("Ma_kh"))), (ObjectType.ObjTst(curDataRow.Item("systotal"), 0, False) = 0))) Then
                        Return
                    End If
                    DirMain.strCustomer = Strings.Trim(StringType.FromObject(curDataRow.Item("Ma_kh")))
                    If (StringType.StrCmp(Strings.Trim(DirMain.strCustomer), "", False) = 0) Then
                        Return
                    End If
                    curDataRow = Nothing
                    'Dim str As String = StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(("fs20_SOCustomerReportDetail" & DirMain.oxInv.xStore), Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.strCustomer, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.cVoucher, ""))))
                    Dim str As String = "fs20_SOCustomerReportDetail_pkd" & DirMain.oxInv.xStore
                    str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
                    str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")
                    str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
                    str += ", " + Sql.ConvertVS2SQLType(DirMain.strCustomer, "")
                    str += ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.cVoucher, ""))
                    str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvFrom.Text, "")
                    str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvTo.Text, "")
                    str += ", " + DirMain.oLen.Item("so_ct")
                    DirMain.oDirFormDetailLib = New reportformlib("0111110001")
                    oDirFormDetailLib.sysConn = DirMain.sysConn
                    oDirFormDetailLib.appConn = DirMain.appConn
                    oDirFormDetailLib.oLan = DirMain.oLan
                    oDirFormDetailLib.oLen = DirMain.oLen
                    oDirFormDetailLib.oVar = DirMain.oVar
                    oDirFormDetailLib.SysID = DirMain.SysID
                    oDirFormDetailLib.cForm = "v20sobcbhkhDetail"
                    oDirFormDetailLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
                    oDirFormDetailLib.strAliasReports = "v20sobcbhkh"
                    oDirFormDetailLib.Init
                    oDirFormDetailLib.strSQLRunReports = str
                    RemoveHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                    AddHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                    oDirFormDetailLib.Show
                    RemoveHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                    AddHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                    DirMain.oDirFormDetailLib = Nothing
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
            'Dim str As String = "EXEC fs20_SOCustomerReport" & DirMain.oxInv.xStore
            'str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
            'str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), ObjectType.AddObj(
            '    str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))), ObjectType.AddObj(
            '    str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.cboCustomer.SelectedValue), "")
            'str += ", " + Sql.ConvertVS2SQLType(DirMain.cVoucher, "")
            'str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")))), ObjectType.AddObj(
            '    str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvtables")), "")))), ObjectType.AddObj(
            '    str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin1")), "")))), ObjectType.AddObj(
            '    str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin2")), "")

            Dim str As String = "EXEC fs20_SOCustomerReport_pkd" & DirMain.oxInv.xStore
            str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_kh1.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_kh2.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_kh3.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.cVoucher, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvtables")), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin1")), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin2")), "")
            Dim expression As String = StringType.FromObject(Interaction.IIf((StringType.StrCmp(Strings.Trim(DirMain.oAdvFilter.GetAdvSelectKey), "", False) = 0), "1=1", DirMain.oAdvFilter.GetAdvSelectKey))
            str = (str & ",'" & Strings.Replace(expression, "'", "''", 1, -1, CompareMethod.Binary) & "'")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvFrom.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvTo.Text, "")
            str += ", " + DirMain.oLen.Item("so_ct")
            DirMain.oDirFormLib = New reportformlib("1011111111")
            oDirFormLib.sysConn = DirMain.sysConn
            oDirFormLib.appConn = DirMain.appConn
            oDirFormLib.oLan = DirMain.oLan
            oDirFormLib.oLen = DirMain.oLen
            oDirFormLib.oVar = DirMain.oVar
            oDirFormLib.SysID = DirMain.SysID
            oDirFormLib.cForm = DirMain.SysID
            oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
            oDirFormLib.strAliasReports = "v20sobcbhkh"
            oDirFormLib.Init
            oDirFormLib.strSQLRunReports = str
            AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            oDirFormLib.Show
            RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            DirMain.oDirFormLib = Nothing
        End Sub


        ' Fields
        Public Shared appConn As SqlConnection
        Public Shared cLan As Char
        Public Shared cVoucher As String
        Public Shared dFrom As DateTime
        Public Shared dTo As DateTime
        Public Shared ewdv As DataView = New DataView
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
        Public Shared strCustomer As String
        Public Shared strGroups As String
        Public Shared strUnit As String
        Public Shared sysConn As SqlConnection
        Public Shared SysID As String
    End Class
End Namespace

