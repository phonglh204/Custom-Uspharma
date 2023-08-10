Imports System.Data.SqlClient
Imports libscommon
Imports libscontrol

Module DirMain
    Public fPrint As New frmFilter
    Public oLan As New Collection, oVar As New Collection, oLen As New Collection, oOption As New Collection
    Public sysConn As SqlConnection, appConn As SqlConnection
    Public SysID As String
    Public rpTable As DataTable
    Public strUnit As String, dFrom As Date, dTo As Date
    Public oDirFormLib As reportformlib
    Public ReportRow As DataRow
    Public oAdvFilter As clsAdvFilter
    Dim nTon_dau As Decimal, nDu_dau As Decimal, nDu_dau_nt As Decimal
    Dim nTon_cuoi As Decimal, nDu_cuoi As Decimal, nDu_cuoi_nt As Decimal
    Public oxInv As xInv

    Sub main()
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
        Control.CheckForIllegalCrossThreadCalls = False
        appConn = Sys.GetConn()
        ' Init sysvar
        Sys.InitVar(sysConn, oVar)
        ' Init options
        Sys.InitOptions(appConn, oOption)
        ' Init columns
        Sys.InitColumns(sysConn, oLen)
        SysID = "z17inso1_dk"
        ' Init Message
        Sys.InitMessage(sysConn, oLan, SysID)
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
        Dim strQuery As String
        strQuery = "EXEC sp17inso1_dk" + oxInv.xStore
        strQuery += Reg.GetRegistryKey("Language")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtDFrom.Value, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtDTo.Value, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_kho.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_vt.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(oAdvFilter.GetGridOrder(fPrint.grdOrder), "")
        strQuery += ", " + Sql.ConvertVS2SQLType(ReportRow("cadvtables"), "")
        strQuery += ", " + Sql.ConvertVS2SQLType(ReportRow("cadvjoin1"), "")
        strQuery += ", " + Sql.ConvertVS2SQLType(ReportRow("cadvjoin2"), "")
        strQuery += ",''"
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_kh.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_nsx.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtXuat_xu.Text, "")
        strQuery += ", " + Sql.ConvertVS2SQLType(fPrint.txtMa_lo.Text, "")

        'Dim ds As New DataSet
        'Sql.SQLRetrieve(appConn, strQuery, "report", ds)
        'ds.WriteXmlSchema("D:\LocalCustomer\Uspharma4.0\Rpt\z17inso1.xsd")

        oDirFormLib = New reportformlib("0111111111")
        With oDirFormLib
            .sysConn = sysConn
            .appConn = appConn
            .oLan = oLan
            .oLen = oLen
            .oVar = oVar
            .SysID = SysID
            .cForm = SysID
            .cCode = Trim(rpTable.Rows(fPrint.cboReports.SelectedIndex).Item("rep_id"))
            .strAliasReports = "inso1"
            .Init()
            .strSQLRunReports = strQuery
            AddHandler .ReportProc, AddressOf ReportProc
            .Show()
            RemoveHandler .ReportProc, AddressOf ReportProc
        End With
        oDirFormLib = Nothing
    End Sub

    Private Sub ReportProc(ByVal nIndex As Integer)
        Select Case nIndex
            Case 0 ' Detail
                DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text, "%s1", (Strings.Trim(DirMain.fPrint.txtMa_kho.Text) & " - " & Strings.Trim(DirMain.fPrint.lblTen_kho.Text)), 1, -1, CompareMethod.Binary)
                DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text, "%s2", (Strings.Trim(DirMain.fPrint.txtMa_vt.Text) & " - " & Strings.Trim(DirMain.fPrint.lblTen_vt.Text)), 1, -1, CompareMethod.Binary)
                DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Trim(DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text)
                Exit Select
            Case 2 ' Print
                Print(0)
            Case 3 ' Preview
                Print(1)
        End Select
    End Sub

    Private Sub Print(ByVal nType As Integer)
        Dim cOldSort As String
        With oDirFormLib.GetClsreports.GetGrid
            cOldSort = .GetDataView.Sort
            .GetDataView.Sort = ""
            nTon_dau = .GetDataView(0).Item("sl_nhap")
            'nDu_dau = .GetDataView(0).Item("Tien_nhap")
            'nDu_dau_nt = .GetDataView(0).Item("Tien_nt_n")
            'nTon_cuoi = .GetDataView(3).Item("sl_nhap")
            'nDu_cuoi = .GetDataView(3).Item("Tien_nhap")
            'nDu_cuoi_nt = .GetDataView(3).Item("Tien_nt_n")
            .GetDataView.Sort = cOldSort
        End With

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
            oReport.dr = clsprint.GetdtRowFromCtrl(fPrint.tbgFilter)
            oReport.oVar = oVar
            oReport.SetReportVar(sysConn, appConn, SysID, oOption, oReport.oRpt)
            oReport.oRpt.SetParameterValue("Title", Trim(fPrint.txtTitle.Text))
            oReport.oRpt.SetParameterValue("t_date", strDate)
            oReport.oRpt.SetParameterValue("Ton_dau", nTon_dau)
            oReport.oRpt.SetParameterValue("Ton_cuoi", nTon_cuoi)
            Try
                oReport.oRpt.SetParameterValue("Du_dau", nDu_dau)
                oReport.oRpt.SetParameterValue("Du_cuoi", nDu_cuoi)
            Catch ex As Exception
            End Try
            Try
                oReport.oRpt.SetParameterValue("Du_dau_nt", nDu_dau_nt)
                oReport.oRpt.SetParameterValue("Du_cuoi_nt", nDu_cuoi_nt)
            Catch ex As Exception
            End Try
            oReport.oRpt.SetParameterValue("Ten_kho", Trim(fPrint.txtMa_kho.Text) + " - " + Trim(fPrint.lblTen_kho.Text))
            oReport.oRpt.SetParameterValue("Ten_vt", Trim(fPrint.lblTen_vt.Text))
            Try
                oReport.oRpt.SetParameterValue("dvt", Trim(Sql.GetValue(appConn, "dmvt", "dvt", "ma_vt = '" + fPrint.txtMa_vt.Text + "'")))
            Catch ex As Exception
            End Try
            Try
                oReport.oRpt.SetParameterValue("ma_vt", Trim(fPrint.txtMa_vt.Text))
            Catch ex As Exception
            End Try
            Try
                oReport.oRpt.SetParameterValue("pack_size", Trim(Sql.GetValue(appConn, "dmvt", "pack_size", "ma_vt = '" + fPrint.txtMa_vt.Text + "'")))
            Catch ex As Exception
            End Try

            Try
                oReport.oRpt.SetParameterValue("f_tk_vt", Trim(Sql.GetValue(appConn, "dmvt", "tk_vt", "ma_vt = '" + fPrint.txtMa_vt.Text + "'")))
            Catch ex As Exception
            End Try
            Try
                oReport.oRpt.SetParameterValue("h_gia_tri_vnd", Replace(oLan("901"), "%s", oOption("m_ma_nt0")))
                oReport.oRpt.SetParameterValue("h_gia_vnd", Replace(oLan("902"), "%s", oOption("m_ma_nt0")))
            Catch ex As Exception
            End Try
            If fPrint.txtMa_lo.Text = "" Then
                Try
                    oReport.oRpt.SetParameterValue("ten_kh", Trim(fPrint.lblTen_kh.Text))
                Catch ex As Exception
                End Try
                Try
                    oReport.oRpt.SetParameterValue("ten_nsx", Trim(fPrint.lblTen_nsx.Text))
                Catch ex As Exception
                End Try
                Try
                    oReport.oRpt.SetParameterValue("xuat_xu", Trim(fPrint.txtXuat_xu.Text))
                Catch ex As Exception
                End Try
            Else
                Dim dr As DataRow = Sql.GetRow(appConn, "vdmlo", "ma_vt='" + fPrint.txtMa_vt.Text.Trim + "' and ma_lo='" + fPrint.txtMa_lo.Text.Trim + "'")
                Try
                    oReport.oRpt.SetParameterValue("ten_kh", dr.Item("ten_ncc"))
                Catch ex As Exception
                End Try
                Try
                    oReport.oRpt.SetParameterValue("ten_nsx", dr.Item("ten_nsx"))
                Catch ex As Exception
                End Try
                Try
                    oReport.oRpt.SetParameterValue("xuat_xu", dr.Item("xuat_xu"))
                Catch ex As Exception
                End Try
            End If
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
