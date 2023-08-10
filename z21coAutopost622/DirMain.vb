Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol
Imports libscontrol.balanceviewlib
Imports libscommon
Imports libscontrol.reportformlib

Namespace z21coAutopost622
    Module DirMain
        ' Methods
        Private Sub Create()
            Dim tcSQL As String = "EXEC fs22_COExe622 "
            tcSQL += Strings.Trim(Conversion.Str(DirMain.nMonth))
            tcSQL += ", " + Strings.Trim(Conversion.Str(DirMain.nYear))
            tcSQL += ", " + Strings.Trim(Conversion.Str(DirMain.nUserID))
            tcSQL += "," + Sql.ConvertVS2SQLType(DirMain.dsDvcs, "")
            Sql.SQLExecute((DirMain.oDirFormLib.appConn), tcSQL)
            Msg.Alert(StringType.FromObject(DirMain.oDirFormLib.oVar.Item("m_end_proc")), 2)
        End Sub

        Private Sub Delete()
            If (ObjectType.ObjTst(Msg.Question(StringType.FromObject(DirMain.oDirFormLib.oLan.Item("007")), 1), 1, False) = 0) Then
                Dim tcSQL As String = "EXEC fs22_CODel622 "
                tcSQL = (StringType.FromObject(ObjectType.AddObj(((((tcSQL & Strings.Trim(Conversion.Str(DirMain.nMonth))) & ", " & Strings.Trim(Conversion.Str(DirMain.nMonth))) & ", " & Strings.Trim(Conversion.Str(DirMain.nYear))) & ", '" & DirMain.cVC & "'"), ObjectType.AddObj(",", Sql.ConvertVS2SQLType(DirMain.dsDvcs, "")))) & ",'Del'")
                Sql.SQLExecute((DirMain.oDirFormLib.appConn), tcSQL)
                Msg.Alert(StringType.FromObject(DirMain.oDirFormLib.oVar.Item("m_end_proc")), 2)
            End If
        End Sub

        Private Sub DirLoad(ByVal sender As Object, ByVal e As EventArgs)
            'Dim item As MenuItem
            'For Each item In DirMain.oDirFormLib.oDir.mnFile.MenuItems
            '    item.Shortcut = Shortcut.None
            'Next
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(0).Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("101"))
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(1).Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("102"))
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(2).Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("113"))
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(3).Text = "-"
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(4).Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("104"))
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(5).Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("105"))
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(6).Text = "-"
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(7).Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("103"))
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(8).Visible = False
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(0).Shortcut = Shortcut.F4
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(1).Shortcut = Shortcut.F8
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(2).Shortcut = Shortcut.F5
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(4).Shortcut = Shortcut.CtrlP
            Dim button As ToolBarButton
            For Each button In DirMain.oDirFormLib.oDir.tbr.Buttons
                button.Style = ToolBarButtonStyle.PushButton
            Next
            DirMain.oDirFormLib.oDir.tbr.Buttons.Item(0).ToolTipText = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("101"))
            DirMain.oDirFormLib.oDir.tbr.Buttons.Item(1).ToolTipText = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("102"))
            DirMain.oDirFormLib.oDir.tbr.Buttons.Item(2).ToolTipText = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("113"))
            DirMain.oDirFormLib.oDir.tbr.Buttons.Item(3).Style = ToolBarButtonStyle.Separator
            DirMain.oDirFormLib.oDir.tbr.Buttons.Item(4).ToolTipText = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("104"))
            DirMain.oDirFormLib.oDir.tbr.Buttons.Item(5).ToolTipText = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("105"))
            DirMain.oDirFormLib.oDir.tbr.Buttons.Item(6).Style = ToolBarButtonStyle.Separator
            DirMain.oDirFormLib.oDir.tbr.Buttons.Item(7).ToolTipText = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("103"))
            DirMain.oDirFormLib.oDir.tbr.Buttons.Item(8).Visible = False
            DirMain.oDirFormLib.oDir.tbr.ImageList.Images.Item(DirMain.oDirFormLib.oDir.tbr.Buttons.Item(0).ImageIndex) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "\new.bmp")))
            DirMain.oDirFormLib.oDir.tbr.ImageList.Images.Item(DirMain.oDirFormLib.oDir.tbr.Buttons.Item(1).ImageIndex) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "\delete.bmp")))
            DirMain.oDirFormLib.oDir.tbr.ImageList.Images.Item(DirMain.oDirFormLib.oDir.tbr.Buttons.Item(2).ImageIndex) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "\browser.bmp")))
            DirMain.oDirFormLib.oDir.tbr.ImageList.Images.Item(DirMain.oDirFormLib.oDir.tbr.Buttons.Item(4).ImageIndex) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "\print.bmp")))
            DirMain.oDirFormLib.oDir.tbr.ImageList.Images.Item(DirMain.oDirFormLib.oDir.tbr.Buttons.Item(5).ImageIndex) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "\preview.bmp")))
            DirMain.oDirFormLib.oDir.tbr.ImageList.Images.Item(DirMain.oDirFormLib.oDir.tbr.Buttons.Item(7).ImageIndex) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "\close.bmp")))
            DirMain.oDirFormLib.oDir.ob.grdLookup.ContextMenu.MenuItems.Clear()
            Dim item2 As MenuItem
            For Each item2 In DirMain.oDirFormLib.oDir.mnFile.MenuItems
                DirMain.oDirFormLib.oDir.ob.grdLookup.ContextMenu.MenuItems.Add(item2.CloneMenu)
            Next
            Dim num2 As Byte = CByte((DirMain.oDirFormLib.oDir.ob.frmLookup.Controls.Count - 1))
            Dim i As Byte = 0
            Do While (i <= num2)
                If (Strings.InStr(Strings.LCase(DirMain.oDirFormLib.oDir.ob.frmLookup.Controls.Item(i).GetType.ToString), "refreshbutton", CompareMethod.Binary) > 0) Then
                    DirMain.oDirFormLib.oDir.ob.frmLookup.Controls.Item(i).Visible = False
                    Exit Do
                End If
                i = CByte((i + 1))
            Loop
            Dim tcSQL As String = "EXEC fs21_GetPs622 "
            tcSQL += Conversion.Str(DirMain.nMonth).Trim
            tcSQL += "," + Conversion.Str(DirMain.nYear).Trim
            DirMain.oDirFormLib.oDir.ob.dv.Table.Clear()
            Sql.SQLRetrieve((DirMain.oDirFormLib.appConn), tcSQL, DirMain.oDirFormLib.cTableDir, (DirMain.oDirFormLib.oDir.ob.ds))
        End Sub

        <STAThread()>
        Public Sub Main(ByVal CmdArgs As String())
            DirMain.oDirFormLib = New DirFormLibBrowse
            DirMain.isCon = False
            DirMain.nUserID = IntegerType.FromString(Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID"))))
            DirMain.dLocked = DateType.FromObject(Sql.GetValue((oDirFormLib.appConn), "dmstt", "ngay_ks", "1=1"))
            Dim document As New ReportDocument
            If (oDirFormLib.sysConn Is Nothing) Then
                ProjectData.EndApp()
            End If
            oDirFormLib.SysID = "z21coAutopost622"
            oDirFormLib.Init()
            Dim dates As New frmDates
            dates.Text = StringType.FromObject(oDirFormLib.oLan.Item("006"))
            dates.ShowDialog()
            If Not DirMain.isCon Then
                dates.Dispose()
                oDirFormLib.Close()
            Else
                DirMain.cVC = Strings.Trim(CmdArgs(0))
                oDirFormLib.oDir.strEnabled = "111111111"
                oDirFormLib.oDir.ebutHand = New ToolBarButtonClickEventHandler(AddressOf DirMain.tbrClick)
                oDirFormLib.oDir.emnuHand = New EventHandler(AddressOf DirMain.mnuclick)
                AddHandler DirMain.oDirFormLib.oDir.ob.frmLookup.Load, New EventHandler(AddressOf DirMain.DirLoad)
                oDirFormLib.frmUpdate = Nothing
                oDirFormLib.Show()
                oDirFormLib.Close()
            End If
        End Sub

        Public Sub mnuclick(ByVal sender As Object, ByVal e As EventArgs)
            Select Case ByteType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing))
                Case 0
                    DirMain.Create()
                    Exit Select
                Case 1
                    DirMain.Delete()
                    Exit Select
                Case 2
                    DirMain.ViewDetail()
                    Exit Select
                Case 4
                    DirMain.ProcPrint(1)
                    Exit Select
                Case 5
                    DirMain.ProcPrint(0)
                    Exit Select
                Case 7
                    DirMain.oDirFormLib.oDir.ob.frmLookup.Close()
                    Exit Select
            End Select
        End Sub

        Private Sub Print(ByVal nType As Integer)
            Dim selectedIndex As Integer = DirMain.frmPrint.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.frmPrint.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
            Dim ds As New DataSet
            Dim str As String = "EXEC fs20_TransGLReport "
            str = StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(str, Sql.ConvertVS2SQLType("PK5", ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.frmPrint.txtMonth.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.frmPrint.txtYear.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.frmPrint.txtMonth.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.frmPrint.txtYear.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.frmPrint.txtMa_dvcs.Text, ""))))
            Sql.SQLRetrieve((Sys.GetConn), str, "tc", (ds))
            Dim view As New DataView
            Dim num4 As Integer = IntegerType.FromObject(Sql.GetValue((Sys.GetConn), "dmct", "max_row", "ma_ct = 'PK5'"))
            view.Table = ds.Tables.Item(0)
            view.RowFilter = "sysprint = 1"
            Dim count As Integer = view.Count
            view.RowFilter = ""
            Dim num5 As Integer = num4
            Dim i As Integer = count
            Do While (i <= num5)
                view.AddNew.Item("sysprint") = 1
                i += 1
            Loop
            Dim clsprint As New clsprint(DirMain.oDirFormLib.oDir.ob.frmLookup, strFile, Nothing)
            clsprint.oRpt.SetDataSource(view.Table)
            If (view.Table.Rows.Count > 0) Then
                clsprint.dr = view.Table.Rows.Item(0)
            Else
                view.AddNew()
                clsprint.dr = view.Table.Rows.Item(0)
            End If
            clsprint.oVar = DirMain.oDirFormLib.oVar
            clsprint.SetReportVar(DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, DirMain.oDirFormLib.SysID, DirMain.oDirFormLib.oOptions, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.frmPrint.txtTitle.Text))
            clsprint.oRpt.SetParameterValue("fDfrom", (Strings.Trim(DirMain.frmPrint.txtMonth.Text) & "/" & Strings.Trim(DirMain.frmPrint.txtYear.Text)))
            clsprint.oRpt.SetParameterValue("fDTo", (Strings.Trim(DirMain.frmPrint.txtMonth.Text) & "/" & Strings.Trim(DirMain.frmPrint.txtYear.Text)))
            If (nType = 1) Then
                clsprint.PrintReport(1)
                clsprint.oRpt.SetDataSource(view.Table)
            Else
                clsprint.ShowReports()
            End If
            clsprint.oRpt.Close()
        End Sub

        Public Sub ProcPrint(ByVal nType As Integer)
            DirMain.frmPrint = New frmFilter
            DirMain.frmPrint.Text = DirMain.oDirFormLib.oDir.ob.frmLookup.Text
            DirMain.isContinue = False
            DirMain.frmPrint.ShowDialog()
            If DirMain.isContinue Then
                DirMain.Print(nType)
            End If
            DirMain.frmPrint.Dispose()
            DirMain.frmPrint = Nothing
        End Sub

        Private Sub ReportProc(ByVal nIndex As Integer)
            Dim num As Integer = nIndex
        End Sub

        Public Sub tbrClick(ByVal sender As Object, ByVal e As ToolBarButtonClickEventArgs)
            Dim objArray2 As Object() = New Object(1 - 1) {}
            Dim args As ToolBarButtonClickEventArgs = e
            objArray2(0) = args.Button
            Dim objArray As Object() = objArray2
            Dim copyBack As Boolean() = New Boolean() {True}
            If copyBack(0) Then
                args.Button = DirectCast(objArray(0), ToolBarButton)
            End If
            Select Case ByteType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "Buttons", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "IndexOf", objArray, Nothing, copyBack))
                Case 0
                    DirMain.Create()
                    Exit Select
                Case 1
                    DirMain.Delete()
                    Exit Select
                Case 2
                    DirMain.ViewDetail()
                    Exit Select
                Case 4
                    DirMain.ProcPrint(1)
                    Exit Select
                Case 5
                    DirMain.ProcPrint(0)
                    Exit Select
                Case 7
                    DirMain.oDirFormLib.oDir.ob.frmLookup.Close()
                    Exit Select
            End Select
        End Sub

        Private Sub ViewDetail()
            Return
            Dim curRow As Integer = DirMain.oDirFormLib.oDir.ob.CurRow
            If (curRow >= 0) Then
                Dim expression As String = "1=1"
                Dim style As DataGridColumnStyle
                For Each style In DirMain.oDirFormLib.oDir.ob.grdLookup.TableStyles.Item(0).GridColumnStyles
                    If ((Not DirMain.oDirFormLib.oDir.ob.dv.Table.Columns.Item(style.MappingName).DataType Is GetType(Decimal)) AndAlso Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(DirMain.oDirFormLib.oDir.ob.dv.Item(curRow).Item(style.MappingName)))) Then
                        expression = String.Concat(New String() {expression, " AND ", style.MappingName, " = '", Strings.Replace(StringType.FromObject(DirMain.oDirFormLib.oDir.ob.dv.Item(curRow).Item(style.MappingName)), "'", "''", 1, -1, CompareMethod.Binary), "'"})
                    End If
                Next
                Dim str2 As String = "EXEC fs_GetFADeprAlloc "
                str2 = (StringType.FromObject(ObjectType.AddObj(((((str2 & Strings.Trim(Conversion.Str(DirMain.nMonth))) & ", " & Strings.Trim(Conversion.Str(DirMain.nMonth))) & ", " & Strings.Trim(Conversion.Str(DirMain.nYear))) & ", '2', '" & Strings.Replace(expression, "'", "''", 1, -1, CompareMethod.Binary) & "'"), ObjectType.AddObj(",", Sql.ConvertVS2SQLType(DirMain.dsDvcs, "")))) & ",''")
                Dim reportformlib As New reportformlib("0011110001")
                reportformlib.sysConn = DirMain.oDirFormLib.sysConn
                reportformlib.appConn = DirMain.oDirFormLib.appConn
                reportformlib.oLan = DirMain.oDirFormLib.oLan
                reportformlib.oLen = DirMain.oDirFormLib.oLen
                reportformlib.oVar = DirMain.oDirFormLib.oVar
                reportformlib.SysID = "FADeprAllocDetail"
                reportformlib.cForm = "FADeprAllocDetail"
                reportformlib.cCode = "001"
                reportformlib.strAliasReports = "fapbkh"
                reportformlib.Init()
                reportformlib.strSQLRunReports = str2
                AddHandler reportformlib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                reportformlib.Show()
                RemoveHandler reportformlib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                reportformlib = Nothing
            End If
        End Sub


        ' Fields
        Public cVC As String
        Public dLocked As DateTime
        Public dsDvcs As String
        Private frmPrint As frmFilter
        Public isCon As Boolean
        Public isContinue As Boolean
        Public nMonth As Integer
        Public nUserID As Integer
        Public nYear As Integer
        Public oDirFormLib As DirFormLibBrowse
    End Module
End Namespace

