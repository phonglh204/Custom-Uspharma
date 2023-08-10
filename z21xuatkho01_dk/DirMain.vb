Imports System.Data.SqlClient
Imports libscommon
Imports libscontrol

Module DirMain
    Public fPrint As New frmFilter()
    Public oLan As New Collection(), oVar As New Collection(), oLen As New Collection(), oOption As New Collection()
    Public sysConn As SqlConnection, appConn As SqlConnection
    Public SysID As String
    Public rpTable As DataTable
    Public strAccount As String, strAccountRef As String, strUnit As String, dFrom As Date, dTo As Date
    Public oDirFormLib As reportformlib
    Dim oDirFormDetailLib As reportformlib
    Dim strGroupID As String, strDetailName As String, strDetailID As String
    Public oAdvFilter As clsAdvFilter
    Public drAdvFilter As DataRow
    'Public cForm As Char
    Public nx As Char
    Public ReportRow As DataRow
    Public oxInv As xInv

    Sub main(ByVal CmdArgs() As String)
        ' Check Login
        If Not Sys.isLogin() And Reg.GetRegistryKey("Customize") = "0" Then
            Return
        End If
        ' Set Connections
        sysConn = Sys.GetSysConn()
        ' Check rights
        If Reg.GetRegistryKey("Customize") = "0" Then
            If Not Sys.CheckRights(sysConn, "Access") Then
                sysConn.Close()
                sysConn = Nothing
                Return
            End If
        End If
        nx = Trim(Fox.GetWordNum(Trim(CmdArgs(0)), 1, "#"))
        appConn = Sys.GetConn()
        ' Init sysvar
        Sys.InitVar(sysConn, oVar)
        ' Init options
        Sys.InitOptions(appConn, oOption)
        ' Init columns
        Sys.InitColumns(sysConn, oLen)
        SysID = "z21xuatkho01_dk"
        ' Init Message
        Sys.InitMessage(sysConn, oLan, SysID)
        drAdvFilter = Sql.GetRow(sysConn, "reports", "form = '" + SysID + "'")
        ReportRow = Sql.GetRow(sysConn, "reports", "form=" + Sql.ConvertVS2SQLType(SysID, ""))
        PrintReport()
        rpTable = Nothing
    End Sub

    Public Sub PrintReport()
        rpTable = clsprint.InitComboReport(sysConn, fPrint.cboReports, SysID)
        fPrint.ShowDialog()
        fPrint.Dispose()
        sysConn.Close()
        appConn.Close()
    End Sub

    Public Sub ShowReport()
        On Error GoTo SQLError
        Dim strQuery, strTest, subQuery As String
        strQuery = "EXEC spxuatkho01_dk" + oxInv.xStore
        strQuery += Sql.ConvertVS2SQLType(fPrint.CboGroupBy.SelectedValue, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.CboDetailBy.SelectedValue, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtDFrom.Value, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtDTo.Value, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_vt.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_kh.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_kho.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_vv.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_nx.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtNh_vt.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtNh_vt2.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtNh_vt3.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_nh1.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_nh2.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_nh3.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtTk_vt.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtLoai_vt.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(oAdvFilter.GetGridTransCode(fPrint.grdTransCode, "ma_gd"), "")
        strQuery += ", " + Sql.ConvertVS2SQLType(oAdvFilter.GetGridTransCode(fPrint.grdTransCode, "loai_ct"), "")
        strQuery += ", " + Sql.ConvertVS2SQLType(oAdvFilter.GetGridTransCode(fPrint.grdTransCode, "ma_ct"), "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.CbbTinh_dc.SelectedValue, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtInvFrom.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtInvTo.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_dvcs.Text, "")
        strQuery += ", " + oLen("so_ct")
        strQuery += ", '" + nx + "'"
        strQuery += ", " + Sql.ConvertVS2SQLType(oAdvFilter.GetGridOrder(fPrint.grdOrder), "")
        strQuery += ", " + Sql.ConvertVS2SQLType(drAdvFilter("cadvtables"), "")
        strQuery += ", " + Sql.ConvertVS2SQLType(drAdvFilter("cadvjoin1"), "")
        strQuery += ", " + Sql.ConvertVS2SQLType(drAdvFilter("cadvjoin2"), "")
        subQuery = IIf(Trim(oAdvFilter.GetAdvSelectKey()) = "", "1=1", oAdvFilter.GetAdvSelectKey())
        'strTest = strQuery + ",'" + Replace(subQuery + " and 1=0 ", "'", "''") + "'"
        strQuery += ",'" + Replace(subQuery, "'", "''") + "'"
        'Try
        '    Sql.SQLExecute(appConn, strTest)
        'Catch ex As Exception
        '    Msg.Alert(oLan("900"), 2)
        '    Exit Sub
        'End Try
        'Dim ds As New DataSet
        'Sql.SQLRetrieve(appConn, strQuery, "report", ds)
        'ds.WriteXmlSchema("E:\CustomerLocal\Pharma\Program\Rpt\z21xuatkho01.xsd")

        oDirFormLib = New reportformlib("1011111111")
        With oDirFormLib
            .sysConn = sysConn
            .appConn = appConn
            .oLan = oLan
            .oLen = oLen
            .oVar = oVar
            .SysID = SysID
            .cForm = SysID
            .cCode = Trim(rpTable.Rows(fPrint.cboReports.SelectedIndex).Item("rep_id"))
            .strAliasReports = "inth2"
            .Init()
            .strSQLRunReports = strQuery
            AddHandler .ReportProc, AddressOf ReportProc
            .Show()
            RemoveHandler .ReportProc, AddressOf ReportProc
        End With
        'oDirFormLib = Nothing
SQLError:
        If Err.Number <> 0 Then
            Msg.Alert(oLan("900"), 2)
        End If
        oDirFormLib = Nothing
    End Sub

    Private Sub ReportProc(ByVal nIndex As Integer)
        Select Case nIndex
            Case 0
                If nx = "2" Then oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Trim(oLan("901"))
                oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Trim(oDirFormLib.GetClsreports.GetGrid.GetForm.Text)
            Case 1
                If IsNothing(oDirFormLib.GetClsreports.GetGrid.CurDataRow) Then
                    Return
                End If
                With oDirFormLib.GetClsreports.GetGrid.CurDataRow
                    If IsDBNull(.Item("Ma_00")) Then
                        Return
                    End If
                    strDetailID = Trim(.Item("Ma_00"))
                    If Trim(strDetailID) = "" Then
                        Return
                    End If
                    If IsDBNull(.Item("Ten_00")) Then
                        strDetailName = ""
                    Else
                        strDetailName = Trim(IIf(Reg.GetRegistryKey("Language") = "V", .Item("Ten_00"), .Item("Ten_002")))
                    End If
                    strGroupID = .Item("Ma_99")
                End With
                Dim strSQL As String
                strSQL = "fs_ReceiveAmountBy2CriteriaDetail" + oxInv.xStore
                strSQL += Sql.ConvertVS2SQLType(fPrint.CboGroupBy.SelectedValue, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(fPrint.CboDetailBy.SelectedValue, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(strGroupID, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(strDetailID, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(fPrint.txtDFrom.Value, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(fPrint.txtDTo.Value, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_vt.Text, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_kh.Text, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_kho.Text, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_vv.Text, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_nx.Text, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(oAdvFilter.GetGridTransCode(fPrint.grdTransCode, "ma_gd"), "")
                strSQL += ", " + Sql.ConvertVS2SQLType(oAdvFilter.GetGridTransCode(fPrint.grdTransCode, "loai_ct"), "")
                strSQL += ", " + Sql.ConvertVS2SQLType(oAdvFilter.GetGridTransCode(fPrint.grdTransCode, "ma_ct"), "")
                strSQL += ", " + Sql.ConvertVS2SQLType(fPrint.CbbTinh_dc.SelectedValue, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(fPrint.txtInvFrom.Text, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(fPrint.txtInvTo.Text, "")
                strSQL += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_dvcs.Text, "")
                strSQL += ", " + oLen("so_ct")
                strSQL += ", '" + nx + "'"
                strSQL += ", " + Sql.ConvertVS2SQLType(oAdvFilter.GetGridOrder(fPrint.grdOrder), "")
                strSQL += ", " + Sql.ConvertVS2SQLType(drAdvFilter("cadvtables"), "")
                strSQL += ", " + Sql.ConvertVS2SQLType(drAdvFilter("cadvjoin1"), "")
                strSQL += ", " + Sql.ConvertVS2SQLType(drAdvFilter("cadvjoin2"), "")
                strSQL += ",'" + Replace(oAdvFilter.GetAdvSelectKey(), "'", "''") + "'"
                oDirFormDetailLib = New reportformlib("0111110001")
                With oDirFormDetailLib
                    .sysConn = sysConn
                    .appConn = appConn
                    .oLan = oLan
                    .oLen = oLen
                    .oVar = oVar
                    .SysID = SysID
                    .cForm = "RecvAmtBy2CriD"
                    .cCode = Trim(rpTable.Rows(fPrint.cboReports.SelectedIndex).Item("rep_id"))
                    .strAliasReports = "inth3d"
                    .Init()
                    .strSQLRunReports = strSQL
                    RemoveHandler oDirFormLib.ReportProc, AddressOf ReportProc
                    AddHandler .ReportProc, AddressOf ReportDetailProc
                    .Show()
                    RemoveHandler .ReportProc, AddressOf ReportDetailProc
                    AddHandler oDirFormLib.ReportProc, AddressOf ReportProc
                End With
                oDirFormDetailLib = Nothing
            Case 2 ' Print
                Print(0)
            Case 3 ' Preview
                Print(1)
        End Select
    End Sub

    Private Sub ReportDetailProc(ByVal nIndex As Integer)
        Select Case nIndex
            Case 0
                oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Replace(oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text, "%s", Trim(strDetailID) + " - " + strDetailName)
                oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Trim(oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text)
        End Select
    End Sub

    Private Sub Print(ByVal nType As Integer)
        'oDirFormLib.GetClsreports.GetGrid.GetGrid.Select(0)
        Dim iIndex As Integer, strReport As String
        iIndex = fPrint.cboReports.SelectedIndex
        strReport = Reg.GetRegistryKey("ReportDir") + Trim(rpTable.Rows(iIndex).Item("rep_file")) + ".rpt"
        Dim strDate As String
        strDate = oLan("301")
        strDate = Replace(strDate, "%d1", CStr(dFrom))
        strDate = Replace(strDate, "%d2", CStr(dTo))
        With oDirFormLib.GetClsreports.GetGrid
            Dim oReport As New clsprint(.GetForm, strReport, Nothing)
            oReport.oRpt.SetDataSource(.GetDataView.Table)
            oReport.oVar = oVar
            oReport.SetReportVar(sysConn, appConn, SysID, oOption, oReport.oRpt)
            oReport.oRpt.SetParameterValue("Title", Trim(fPrint.txtTitle.Text))
            oReport.oRpt.SetParameterValue("t_date", strDate)
            If Trim(fPrint.txtMa_kho.Text) <> "" Then
                oReport.oRpt.SetParameterValue("r_tat_ca_kho", Trim(fPrint.txtMa_kho.Text) + " - " + Trim(fPrint.lblTen_kho.Text))
            End If
            Try
                oReport.oRpt.SetParameterValue("h_gia_tri_vnd", Replace(oLan("904"), "%s", oOption("m_ma_nt0")))
            Catch ex As Exception
            End Try

            If nType = 0 Then
                oReport.PrintReport(1)
                oReport.oRpt.SetDataSource(.GetDataView.Table)
            Else
                oReport.ShowReports()
            End If
            oReport.oRpt.Close()
        End With
    End Sub
End Module
