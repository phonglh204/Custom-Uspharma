Imports libscommon
Imports libscontrol
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms

Module DirMain
    ' Methods
    <STAThread>
    Public Sub main(ByVal CmdArgs As String())
        If Not BooleanType.FromObject(RuntimeHelpers.GetObjectValue(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(RuntimeHelpers.GetObjectValue(Reg.GetRegistryKey("Customize")), "0", False) = 0)))) Then
            DirMain.sysConn = Sys.GetSysConn
            If ((ObjectType.ObjTst(RuntimeHelpers.GetObjectValue(Reg.GetRegistryKey("Customize")), "0", False) = 0) AndAlso Not Sys.CheckRights(DirMain.sysConn, "Access")) Then
                DirMain.sysConn.Close()
                DirMain.sysConn = Nothing
            Else
                Control.CheckForIllegalCrossThreadCalls = False
                DirMain.appConn = Sys.GetConn
                Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                DirMain.SysID = "zKeHoachNam03"
                Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                DirMain.ReportRow = DirectCast(Sql.GetRow((DirMain.sysConn), "reports", StringType.FromObject(RuntimeHelpers.GetObjectValue(ObjectType.AddObj("form=", RuntimeHelpers.GetObjectValue(Sql.ConvertVS2SQLType(DirMain.SysID, "")))))), DataRow)
                DirMain.PrintReport()
                DirMain.rpTable = Nothing
            End If
        End If
    End Sub

    Private Sub Print(ByVal nType As Integer)
        Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
        Dim str As String = StringType.FromObject(RuntimeHelpers.GetObjectValue(ObjectType.AddObj(RuntimeHelpers.GetObjectValue(ObjectType.AddObj(RuntimeHelpers.GetObjectValue(Reg.GetRegistryKey("ReportDir")), Strings.Trim(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))))), ".rpt")))
        Dim browse As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
        Dim clsprint As New clsprint(browse.GetForm, str, Nothing)
        clsprint.oRpt.SetDataSource(browse.GetDataView.Table)
        clsprint.oVar = DirMain.oVar
        clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
        clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
        'Try
        '    clsprint.oRpt.SetParameterValue("h_tien_vnd", Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("903"))), "%s", StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oOption.Item("m_ma_nt0"))), 1, -1, CompareMethod.Binary))
        'Catch exception1 As Exception
        '    ProjectData.SetProjectError(exception1)
        '    Dim ex As Exception = exception1
        '    ProjectData.SetProjectError(ex)
        '    ProjectData.ClearProjectError()
        '    ProjectData.ClearProjectError()
        'End Try
        If (nType = 0) Then
            clsprint.PrintReport(1)
            clsprint.oRpt.SetDataSource(DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Table)
        Else
            clsprint.ShowReports()
        End If
        clsprint.oRpt.Close()
        browse = Nothing
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
            Case 0, 1
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

    Public Sub ShowReport()
        Try
            Dim str As String = Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject(Conversions.ToString(Operators.AddObject("EXEC zKeHoachNam03 ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNam.Value, ""))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.TXTMA_VT.Text, "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.TXTNH_VT.Text, "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.TXTNH_VT2.Text, "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.TXTNH_VT3.Text, "")))), Operators.AddObject(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, ""))))
            'Dim ds As New DataSet
            'Sql.SQLRetrieve(appConn, str, "report", ds)
            'ds.WriteXmlSchema("D:\LocalCustomer\Uspharma4.0\Rpt\zKeHoachNam03.xsd")

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
            AddHandler DirMain.oDirFormLib.ReportProc, AddressOf ReportProc
            DirMain.oDirFormLib.Show()
            RemoveHandler DirMain.oDirFormLib.ReportProc, AddressOf ReportProc
            DirMain.oDirFormLib = Nothing
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            Msg.Alert(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("900"))), 2)
            ProjectData.ClearProjectError()
        End Try
    End Sub


    ' Fields
    Public appConn As SqlConnection
    Public fPrint As frmFilter = New frmFilter
    Public nNam As Integer
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

