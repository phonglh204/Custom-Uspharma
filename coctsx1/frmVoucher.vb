Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol.clsvoucher.clsVoucher
Imports libscontrol
Imports libscontrol.voucherseachlib

Namespace coctsx1
    Public Class frmVoucher
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmVoucher_Load)
            AddHandler MyBase.Activated, New EventHandler(AddressOf Me.frmVoucher_Activated)
            Me.arrControlButtons = New Button(13 - 1) {}
            'Me.oTitleButton = New TitleButton(Me)
            Me.lAllowCurrentCellChanged = True
            Me.xInventory = New clsInventory
            Me.InitializeComponent()
        End Sub

        Public Sub AddNew()
            Dim obj2 As Object = "stt_rec is null or stt_rec = ''"
            Me.grdHeader.ScatterBlank
            modVoucher.tblDetail.AddNew()
            modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
            Me.pnContent.Text = ""
            ScatterMemvarBlankWithDefault(Me)
            If (ObjectType.ObjTst(Me.txtNgay_ct.Text, Fox.GetEmptyDate, False) = 0) Then
                Me.txtNgay_ct.Value = DateAndTime.Now.Date
                Me.txtNgay_lct.Value = Me.txtNgay_ct.Value
            End If
            If (StringType.StrCmp(Strings.Trim(Me.cmdMa_nt.Text), "", False) = 0) Then
                Me.cmdMa_nt.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("ma_nt"))
            End If
            Me.txtTy_gia.Value = DoubleType.FromObject(oVoucher.GetFCRate(Me.cmdMa_nt.Text, Me.txtNgay_ct.Value))
            Me.txtSo_ct.Text = oVoucher.GetVoucherNo
            Me.txtStatus.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_status"))
            Me.txtMa_gd.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_ma_gd"))
            Unit.SetUnit(Me.txtMa_dvcs)
            Me.EDFC()
            Me.cOldIDNumber = Me.cIDNumber
            Me.iOldMasterRow = Me.iMasterRow
            Me.EDTranType()
            Me.UpdateList()
            Me.ShowTabDetail()
            If Me.txtMa_dvcs.Enabled Then
                Me.txtMa_dvcs.Focus()
            Else
                Me.txtMa_gd.Focus()
            End If
            Me.EDTBColumns()
            Me.InitFlowHandling(Me.cboAction)
            Me.EDStatus()
            Me.oSecurity.SetReadOnly()
            Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
        End Sub

        'Private Sub AfterUpdateMO(ByVal lcIDNumber As String, ByVal lcAction As String)
        '    Dim tcSQL As String = String.Concat(New String() {"fs_AfterUpdateMO '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
        '    Sql.SQLExecute((modVoucher.appConn), tcSQL)
        'End Sub
        Private Function AfterUpdateMO(ByVal lcIDNumber As String, ByVal lcAction As String) As String
            Dim tcSQL As String = String.Concat(New String() {"EXEC fs_AfterUpdateMO '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
            Return tcSQL
        End Function

        'Private Sub BeforUpdateMO(ByVal lcIDNumber As String, ByVal lcAction As String)
        '    Dim tcSQL As String = String.Concat(New String() {"fs_BeforUpdateMO '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
        '    Sql.SQLExecute((modVoucher.appConn), tcSQL)
        'End Sub
        Function BeforUpdateMO(ByVal lcIDNumber As String, ByVal lcAction As String) As String
            Dim tcSQL As String = String.Concat(New String() {"EXEC fs_BeforUpdateMO '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
            Return tcSQL
        End Function

        Public Sub Cancel()
            On Error Resume Next
            Dim num2 As Integer
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If (currentRowIndex >= 0) Then
                Me.grdDetail.Select(currentRowIndex)
            End If
            If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                num2 = (modVoucher.tblDetail.Count - 1)
                currentRowIndex = num2
                Do While (currentRowIndex >= 0)
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) Then
                        If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))), "", False) = 0) Then
                            modVoucher.tblDetail.Item(currentRowIndex).Delete()
                        End If
                    Else
                        modVoucher.tblDetail.Item(currentRowIndex).Delete()
                    End If
                    currentRowIndex = (currentRowIndex + -1)
                Loop
                If (Me.iOldMasterRow = -1) Then
                    ScatterMemvarBlank(Me)
                    Dim obj2 As Object = "stt_rec = ''"
                    modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
                    Me.cmdNew.Focus()
                    oVoucher.cAction = "Start"
                Else
                    ScatterMemvar(modVoucher.tblMaster.Item(Me.iOldMasterRow), Me)
                    Dim obj3 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iOldMasterRow).Item("stt_rec")), "'")
                    modVoucher.tblDetail.RowFilter = StringType.FromObject(obj3)
                    Me.cmdEdit.Focus()
                    oVoucher.cAction = "View"
                End If
            Else
                num2 = (modVoucher.tblDetail.Count - 1)
                currentRowIndex = num2
                Do While (currentRowIndex >= 0)
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) Then
                        If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))), "", False) = 0) Then
                            modVoucher.tblDetail.Item(currentRowIndex).Delete()
                        End If
                        If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                            modVoucher.tblDetail.Item(currentRowIndex).Delete()
                        End If
                    Else
                        modVoucher.tblDetail.Item(currentRowIndex).Delete()
                    End If
                    currentRowIndex = (currentRowIndex + -1)
                Loop
                AppendFrom(modVoucher.tblDetail, Me.oldtblDetail)
                Me.RefrehForm()
                Me.cmdEdit.Focus()
                oVoucher.cAction = "View"
            End If
            Me.UpdateList()
            Me.vCaptionRefresh()
            Me.EDTBColumns()
        End Sub

        Public Sub Delete()
            'Dim row As DataRow
            'row = Sql.GetRow(appConn, "ct00", "so_lsx='" + tblMaster.Item(Me.iMasterRow).Item("so_ct") + "'")
            'If Not row Is Nothing Then
            '    If row("stt_rec") <> "" Then
            '        Dim str As String
            '        str = oLan.Item("019")
            '        str = Replace(str, "%s1", Trim(Sql.GetValue(appConn, "dmct", "ten_ct", "ma_ct='" + Trim(row("ma_ct")) + "'")))
            '        str = Replace(str, "%s2", Trim(row("so_ct")))
            '        Dim d As Date
            '        d = row("ngay_ct")
            '        str = Replace(str, "%s3", d.ToShortDateString)
            '        MsgBox(str)
            '        Return
            '    End If
            'End If
            If Not Me.isDelete Then
                'Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("018")), 2)
                Return
            ElseIf Me.oSecurity.GetStatusDelelete Then
                Dim num As Integer
                Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
                Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                Dim lcIDNumber As String = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
                num = num2
                Do While (num >= 0)
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("stt_rec"))) Then
                        If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                            modVoucher.tblDetail.Item(num).Delete()
                        End If
                    Else
                        modVoucher.tblDetail.Item(num).Delete()
                    End If
                    num = (num + -1)
                Loop
                Dim cString As String = Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf")))
                Dim tcSQL As String = GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)
                Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                num = 1
                Do While (num <= num3)
                    Dim cTable As String = Strings.Trim(Fox.GetWordNum(cString, num, ","c))
                    tcSQL = (tcSQL & ChrW(13) & GenSQLDelete(cTable, cKey))
                    num += 1
                Loop
                modVoucher.tblMaster.Item(Me.iMasterRow).Delete()
                If (Me.iMasterRow > 0) Then
                    Me.iMasterRow -= 1
                ElseIf (modVoucher.tblMaster.Count = 0) Then
                    Me.iMasterRow = -1
                End If
                If (Me.iMasterRow = -1) Then
                    ScatterMemvarBlank(Me)
                    oVoucher.cAction = "Start"
                    Dim obj2 As Object = "stt_rec = ''"
                    modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
                Else
                    oVoucher.cAction = "View"
                    Me.RefrehForm()
                End If
                tcSQL = Me.BeforUpdateMO(lcIDNumber, "Del") + Chr(13) + tcSQL
                Try
                    Sql.SQLExecute((modVoucher.appConn), tcSQL)
                Catch ex As Exception
                    Msg.Alert("Error (Không xóa được phiếu)!" + Chr(13) + ex.ToString())
                End Try
                Me.pnContent.Text = ""
            End If
        End Sub

        Private Sub DeleteItem(ByVal sender As Object, ByVal e As EventArgs)
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
                Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
                If ((((currentRowIndex >= 0) And (currentRowIndex < modVoucher.tblDetail.Count)) AndAlso Not Me.grdDetail.EndEdit(Me.grdDetail.TableStyles.Item(0).GridColumnStyles.Item(Me.grdDetail.CurrentCell.ColumnNumber), currentRowIndex, False)) AndAlso (ObjectType.ObjTst(Msg.Question(StringType.FromObject(modVoucher.oVar.Item("m_sure_dele")), 1), 1, False) = 0)) Then
                    Me.grdDetail.Select(currentRowIndex)
                    AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), False)
                    modVoucher.tblDetail.Item(currentRowIndex).Delete()
                    Me.UpdateList()
                    AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), True)
                End If
            End If
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Public Sub EDFC()
            If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
                Me.txtTy_gia.Enabled = False
            Else
                Me.txtTy_gia.Enabled = True
            End If
            Me.EDStatus()
            Me.oSecurity.Invisible()
        End Sub

        Public Sub Edit()
            Me.oldtblDetail = Copy2Table(modVoucher.tblDetail)
            Me.iOldMasterRow = Me.iMasterRow
            oVoucher.rOldMaster = modVoucher.tblMaster.Item(Me.iMasterRow)
            'Dim row As DataRow
            'row = Sql.GetRow(appConn, "ct00", "so_lsx='" + tblMaster.Item(Me.iMasterRow).Item("so_ct") + "'")
            'Dim _admin As Boolean = Sql.GetValue(oVoucher.sysConn, "userinfo", "admin", "id=" + Reg.GetRegistryKey("CurrUserid"))
            'If Not row Is Nothing Then
            '    If row("stt_rec") <> "" Then
            '        Dim str As String
            '        str = oLan.Item("019")
            '        str = Replace(str, "%s1", Trim(Sql.GetValue(appConn, "dmct", "ten_ct", "ma_ct='" + Trim(row("ma_ct")) + "'")))
            '        str = Replace(str, "%s2", Trim(row("so_ct")))
            '        Dim d As Date
            '        d = row("ngay_ct")
            '        str = Replace(str, "%s3", d.ToShortDateString)
            '        MsgBox(str)
            '        If Not _admin Then
            '            Me.cmdSave.Enabled = False
            '            Return
            '        End If
            '    End If
            'End If
            If Not Me.isEdit Then
                'Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("017")), 2)
                Return
                Me.cmdSave.Enabled = False
            Else
                Me.ShowTabDetail()
                If Me.txtMa_dvcs.Enabled Then
                    Me.txtMa_dvcs.Focus()
                Else
                    Me.txtMa_gd.Focus()
                End If
                Me.EDTBColumns()
                Me.InitFlowHandling(Me.cboAction)
                Me.EDStatus()
                Me.oSecurity.SetReadOnly()
                If Not Me.oSecurity.GetStatusEdit Then
                    Me.cmdSave.Enabled = False
                End If
                Me.EDTrans()
                Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
            End If
        End Sub

        Private Sub EDStatus()
            Try
                oVoucher.RefreshHandling(Me.cboAction)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                ProjectData.ClearProjectError()
            End Try
            If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                Me.cboStatus.SelectedIndex = 0
            Else
                oVoucher.RefreshStatus(Me.cboStatus)
            End If
            Me.RefreshControlField()
            Me.lblAction.Visible = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            Me.cboAction.Visible = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            Me.grdHeader.Edit = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
        End Sub

        Private Sub EDStatus(ByVal lED As Boolean)
            oVoucher.RefreshHandling(Me.cboAction)
            oVoucher.RefreshStatus(Me.cboStatus)
            Me.lblAction.Visible = lED
            Me.cboAction.Visible = lED
            Me.grdHeader.Edit = lED
        End Sub

        Private Sub EDTBColumns()
            Dim index As Integer = 0
            Do
                modVoucher.tbcDetail(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
                index += 1
            Loop While (index <= &H1D)
            Try
                Me.colTen_vt.TextBox.Enabled = False
                ' Me.colSo_dh.TextBox.Enabled = False
                Me.colSo_line.TextBox.Enabled = False
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
        End Sub

        Private Sub EDTBColumns(ByVal lED As Boolean)
            On Error Resume Next
            Dim index As Integer = 0
            For index = 0 To &H1D
                modVoucher.tbcDetail(index).TextBox.Enabled = lED
            Next
            Me.EDStatus(lED)
        End Sub

        Private Sub EDTrans()
            Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
        End Sub

        Private Sub EDTranType()
        End Sub

        Private Sub frmRetrieveLoad(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Private Sub frmVoucher_Activated(ByVal sender As Object, ByVal e As EventArgs)
            If Not Me.isActive Then
                Me.isActive = True
                Me.InitRecords()
            End If
        End Sub

        Private Sub frmVoucher_Load(ByVal sender As Object, ByVal e As EventArgs)
            'Me.oTitleButton.Code = modVoucher.VoucherCode
            'Me.oTitleButton.Connection = modVoucher.sysConn
            clsdrawlines.Init(Me, Me.tbDetail)
            Me.oVoucher = New clsvoucher.clsVoucher(Me.arrControlButtons, Me, Me.pnContent)
            oVoucher.isRead = Sys.CheckRights(modVoucher.sysConn, "Read")
            oVoucher.sysConn = modVoucher.sysConn
            oVoucher.appConn = modVoucher.appConn
            oVoucher.txtVDate = Me.txtNgay_ct
            oVoucher.lblStatus = Me.lblStatus
            oVoucher.lblStatusMess = Me.lblStatusMess
            oVoucher.cmdFC = Me.cmdMa_nt
            oVoucher.txtFCRate = Me.txtTy_gia
            oVoucher.oTab = Me.tbDetail
            oVoucher.oLan = modVoucher.oLan
            oVoucher.oOption = modVoucher.oOption
            oVoucher.oVar = modVoucher.oVar
            oVoucher.oVoucherRow = modVoucher.oVoucherRow
            oVoucher.VoucherCode = modVoucher.VoucherCode
            oVoucher.tblMaster = modVoucher.tblMaster
            oVoucher.tblDetail = modVoucher.tblDetail
            oVoucher.txtStatus = Me.txtStatus
            Me.tblHandling = oVoucher.InitHandling(Me.cboAction)
            Me.tblStatus = oVoucher.InitStatus(Me.cboStatus)
            If (StringType.StrCmp(modVoucher.cLan, "V", False) = 0) Then
                Me.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("ten_ct"))
            Else
                Me.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("ten_ct2"))
            End If
            Sys.InitMessage(modVoucher.sysConn, oVoucher.oClassMsg, "SysClass")
            Me.lblStatus.Text = StringType.FromObject(oVoucher.oClassMsg.Item("011"))
            Me.lblAction.Text = StringType.FromObject(oVoucher.oClassMsg.Item("033"))
            oVoucher.Init()
            Me.txtNgay_lct.AddCalenderControl()
            Dim lib4 As New DirLib(Me.txtMa_gd, Me.lblTen_gd, modVoucher.sysConn, modVoucher.appConn, "dmmagd", "ma_gd", "ten_gd", "VCTransCode", ("ma_ct = '" & modVoucher.VoucherCode & "'"), False, Me.cmdEdit)
            AddHandler Me.txtMa_gd.Validated, New EventHandler(AddressOf Me.txtMa_gd_Valid)
            Dim lib5 As New DirLib(Me.txtMa_dvcs, Me.lblTen_dvcs, modVoucher.sysConn, modVoucher.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", False, Me.cmdEdit)
            Dim lib2 As New DirLib(Me.txtMa_md, Me.lblTen_md, modVoucher.sysConn, modVoucher.appConn, "dmmd", "ma_md", "ten_md", "Priority", ("ma_ct = '" & modVoucher.VoucherCode & "'"), False, Me.cmdEdit)
            Dim lib3 As New CharLib(Me.txtStatus, "0, 1")
            Dim ldate As New clsGLdate(Me.txtNgay_lct, Me.txtNgay_ct)
            Unit.SetUnit(modVoucher.appConn, Me.txtMa_dvcs)
            Me.txtNgay_ct.TabStop = (ObjectType.ObjTst(modVoucher.oVoucherRow.Item("m_ngay_ct"), 1, False) = 0)
            Me.iMasterRow = -1
            Me.iOldMasterRow = -1
            Me.iDetailRow = -1
            Me.cIDNumber = ""
            Me.cOldIDNumber = ""
            Me.nColumnControl = -1
            modVoucher.alMaster = (Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "tmp")
            modVoucher.alDetail = (Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "tmp")
            Dim cFile As String = ("Structure\Voucher\" & modVoucher.VoucherCode)
            If Not Sys.XML2DataSet((modVoucher.dsMain), cFile) Then
                Dim tcSQL As String = ("SELECT * FROM " & modVoucher.alMaster)
                Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alMaster, (modVoucher.dsMain))
                tcSQL = ("SELECT * FROM " & modVoucher.alDetail)
                Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alDetail, (modVoucher.dsMain))
                Sys.DataSet2XML(modVoucher.dsMain, cFile)
            End If
            modVoucher.tblMaster.Table = modVoucher.dsMain.Tables.Item(modVoucher.alMaster)
            modVoucher.tblDetail.Table = modVoucher.dsMain.Tables.Item(modVoucher.alDetail)
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), Me.grdDetail, (modVoucher.tbsDetail), (modVoucher.tbcDetail), "MODetail")
            oVoucher.SetMaxlengthItem(Me.grdDetail, modVoucher.alDetail, modVoucher.sysConn)
            Me.grdDetail.dvGrid = modVoucher.tblDetail
            Me.grdDetail.cFieldKey = "ma_vt"
            Me.grdDetail.AllowSorting = False
            Me.grdDetail.TableStyles.Item(0).AllowSorting = False
            Me.colMa_vt = GetColumn(Me.grdDetail, "ma_vt")
            Me.colDvt = GetColumn(Me.grdDetail, "Dvt")
            Me.colMa_kho = GetColumn(Me.grdDetail, "ma_kho")
            Me.colMa_vi_tri = GetColumn(Me.grdDetail, "ma_vi_tri")
            Me.colMa_lo = GetColumn(Me.grdDetail, "ma_lo")
            Me.colSo_luong = GetColumn(Me.grdDetail, "so_luong")
            Me.colTen_vt = GetColumn(Me.grdDetail, "ten_vt")
            Me.colSo_dh = GetColumn(Me.grdDetail, "so_dh")
            Me.colSo_line = GetColumn(Me.grdDetail, "so_line")
            Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keyaccount", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            Dim sKey As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keycust", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            Me.oSite = New VoucherKeyLibObj(Me.colMa_kho, "ten_kho", modVoucher.sysConn, modVoucher.appConn, "dmkho", "ma_kho", "ten_kho", "Site", ("ma_dvcs = '" & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("DFUnit"))) & "'"), modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
            Me.oUOM = New VoucherKeyCheckLibObj(Me.colDvt, "ten_dvt", modVoucher.sysConn, modVoucher.appConn, "vdmvtqddvt", "dvt", "ten_dvt", "UOMItem", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            Me.oUOM.Cancel = True
            Me.colDvt.TextBox.CharacterCasing = CharacterCasing.Normal
            AddHandler Me.colMa_kho.TextBox.Enter, New EventHandler(AddressOf Me.WhenSiteEnter)
            AddHandler Me.colMa_kho.TextBox.Validated, New EventHandler(AddressOf Me.WhenSiteLeave)
            AddHandler Me.colDvt.TextBox.Move, New EventHandler(AddressOf Me.WhenUOMEnter)
            AddHandler Me.colDvt.TextBox.Validated, New EventHandler(AddressOf Me.WhenUOMLeave)
            Dim monumber As New monumber(GetColumn(Me.grdDetail, "so_lsx"))
            'Dim oCust As New DirLib(Me.txtMa_kh, Me.lblTen_kh, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", sKey, False, Me.cmdEdit)
            Dim oDept As New DirLib(Me.txtMa_bp, Me.lblTen_bp, modVoucher.sysConn, modVoucher.appConn, "dmbp", "ma_bp", "ten_bp", "SaleDept", "1=1", False, Me.cmdEdit)
            Me.oInvItemDetail = New VoucherLibObj(Me.colMa_vt, "ten_vt", modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            VoucherLibObj.oClassMsg = oVoucher.oClassMsg
            Me.oInvItemDetail.Colkey = True
            VoucherLibObj.dvDetail = modVoucher.tblDetail
            Me.oLocation = New VoucherKeyLibObj(Me.colMa_vi_tri, "ten_vi_tri", modVoucher.sysConn, modVoucher.appConn, "dmvitri", "ma_vi_tri", "ten_vi_tri", "Location", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            Me.oLot = New VoucherKeyLibObj(Me.colMa_lo, "ten_lo", modVoucher.sysConn, modVoucher.appConn, "dmlo", "ma_lo", "ten_lo", "Lot", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            Me.oSODetail4MO = New VoucherKeyLibObj(Me.colSo_dh, "ten_lo", modVoucher.sysConn, modVoucher.appConn, "vSODetail4MO", "code", "ten", "SODetail4MO", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            AddHandler Me.colMa_vi_tri.TextBox.Move, New EventHandler(AddressOf Me.WhenLocationEnter)
            AddHandler Me.colMa_lo.TextBox.Move, New EventHandler(AddressOf Me.WhenLotEnter)
            AddHandler Me.colSo_dh.TextBox.Move, New EventHandler(AddressOf Me.WhenSODetail4MOEnter)
            AddHandler Me.colMa_vt.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKey)
            AddHandler Me.colMa_vt.TextBox.Validated, New EventHandler(AddressOf Me.WhenItemLeave)
            Try
                oVoucher.AddValidFields(Me.grdDetail, modVoucher.tblDetail, Me.pnContent, Me.cmdEdit)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Me.colTen_vt.TextBox.Enabled = False
            Me.colSo_dh.TextBox.Enabled = False
            Me.colSo_line.TextBox.Enabled = False
            oVoucher.HideFields(Me.grdDetail)
            ChangeFormatColumn(Me.colSo_luong, StringType.FromObject(modVoucher.oVar.Item("m_ip_sl")))
            AddHandler Me.colSo_luong.TextBox.Leave, New EventHandler(AddressOf Me.txtSo_luong_valid)
            AddHandler Me.colSo_luong.TextBox.Enter, New EventHandler(AddressOf Me.txtSo_luong_enter)
            Dim objectValue As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldchar", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
            Dim obj4 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
            Dim obj3 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fielddate", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
            Dim index As Integer = 0
            Do
                Dim objArray As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj4)}
                Dim flagArray As Boolean() = New Boolean() {True}
                If flagArray(0) Then
                    obj4 = RuntimeHelpers.GetObjectValue(objArray(0))
                End If
                If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", objArray, Nothing, flagArray)), modVoucher.tbcDetail(index).MappingName.ToLower, 0) > 0) Then
                    modVoucher.tbcDetail(index).NullText = "0"
                Else
                    Dim objArray2 As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj3)}
                    flagArray = New Boolean() {True}
                    If flagArray(0) Then
                        obj3 = RuntimeHelpers.GetObjectValue(objArray2(0))
                    End If
                    If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", objArray2, Nothing, flagArray)), modVoucher.tbcDetail(index).MappingName.ToLower, 0) > 0) Then
                        modVoucher.tbcDetail(index).NullText = StringType.FromObject(Fox.GetEmptyDate)
                    Else
                        modVoucher.tbcDetail(index).NullText = ""
                    End If
                End If
                If (index <> 0) Then
                    AddHandler modVoucher.tbcDetail(index).TextBox.Enter, New EventHandler(AddressOf Me.txt_Enter)
                End If
                index += 1
            Loop While (index <= &H1D)
            Dim menu As New ContextMenu
            Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItem), Shortcut.F4)
            Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItem), Shortcut.F8)
            menu.MenuItems.Add(item)
            menu.MenuItems.Add(New MenuItem("-"))
            menu.MenuItems.Add(item2)
            Dim menu2 As New ContextMenu
            Dim item3 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("057")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F5)
            menu2.MenuItems.Add(item3)
            Me.ContextMenu = menu2
            If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) = 0) Then
                item3.Enabled = False
                item3.Visible = False
            End If
            Me.txtKeyPress.Left = (-100 - Me.txtKeyPress.Width)
            Me.grdDetail.ContextMenu = menu
            ScatterMemvarBlank(Me)
            oVoucher.cAction = "Start"
            Me.isActive = False
            Me.grdHeader = New grdHeader(Me.tbDetail, (Me.txtKeyPress.TabIndex - 1), Me, modVoucher.appConn, modVoucher.sysConn, modVoucher.VoucherCode, Me.pnContent, Me.cmdEdit)
            Me.EDTBColumns()
            Me.oSecurity = New clssecurity(modVoucher.VoucherCode, IntegerType.FromObject(Reg.GetRegistryKey("CurrUserid")))
            Me.oSecurity.oVoucher = Me.oVoucher
            Me.oSecurity.cboAction = Me.cboAction
            Me.oSecurity.cboStatus = Me.cboStatus
            Me.oSecurity.cTotalField = "t_tt, t_tt_nt"
            Dim aGrid As New Collection
            aGrid.Add(Me, "Form", Nothing, Nothing)
            aGrid.Add(Me.grdHeader, "grdHeader", Nothing, Nothing)
            aGrid.Add(Me.grdDetail, "grdDetail", Nothing, Nothing)
            Me.oSecurity.aGrid = aGrid
            Me.oSecurity.Init()
            Me.oSecurity.Invisible()
            Me.oSecurity.SetReadOnly()
            Me.InitInventory()
        End Sub

        Private Function GetIDItem(ByVal tblItem As DataView, ByVal sStart As String) As String
            Dim str2 As String = (sStart & "00")
            Dim num2 As Integer = (tblItem.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblItem.Item(i).Item("stt_rec0"))) AndAlso (ObjectType.ObjTst(tblItem.Item(i).Item("stt_rec0"), str2, False) > 0)) Then
                    str2 = StringType.FromObject(tblItem.Item(i).Item("stt_rec0"))
                End If
                i += 1
            Loop
            Return Strings.Format(CInt(Math.Round(CDbl((DoubleType.FromString(str2) + 1)))), "000")
        End Function

        Public Sub GoRecno(ByVal cRecno As Object)
            If (StringType.StrCmp(oVoucher.cAction, "View", False) = 0) Then
                Dim obj2 As Object = cRecno
                If (ObjectType.ObjTst(obj2, "Top", False) = 0) Then
                    If (Me.iMasterRow > 0) Then
                        Me.iMasterRow = 0
                        Me.RefrehForm()
                    End If
                ElseIf (ObjectType.ObjTst(obj2, "Prev", False) = 0) Then
                    If (Me.iMasterRow > 0) Then
                        Me.iMasterRow -= 1
                        Me.RefrehForm()
                    End If
                ElseIf (ObjectType.ObjTst(obj2, "Next", False) = 0) Then
                    If ((Me.iMasterRow < (modVoucher.tblMaster.Count - 1)) And (modVoucher.tblMaster.Count > 0)) Then
                        Me.iMasterRow += 1
                        Me.RefrehForm()
                    End If
                ElseIf ((ObjectType.ObjTst(obj2, "Bottom", False) = 0) AndAlso ((Me.iMasterRow < (modVoucher.tblMaster.Count - 1)) And (modVoucher.tblMaster.Count > 0))) Then
                    Me.iMasterRow = (modVoucher.tblMaster.Count - 1)
                    Me.RefrehForm()
                End If
            End If
        End Sub

        Private Sub grdDetail_CurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdDetail.CurrentCellChanged
            On Error Resume Next
            If Not Me.lAllowCurrentCellChanged Then
                Return
            End If
            Dim currentRowIndex As Integer = grdDetail.CurrentRowIndex
            Dim columnNumber As Integer = grdDetail.CurrentCell.ColumnNumber
            Dim oValue As String = Strings.Trim(StringType.FromObject(grdDetail.Item(currentRowIndex, columnNumber)))
            Dim str2 As String = grdDetail.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName.ToUpper.ToString
            Dim cOldSite As Object
            Select Case str2
                Case "MA_KHO"
                    cOldSite = Me.cOldSite
                    SetOldValue((cOldSite), oValue)
                    Me.cOldSite = StringType.FromObject(cOldSite)
                Case "SO_LUONG"
                    cOldSite = Me.noldSo_luong
                    SetOldValue((cOldSite), oValue)
                    Me.noldSo_luong = DecimalType.FromObject(cOldSite)
            End Select
        End Sub

        Private Sub grdLeave(ByVal sender As Object, ByVal e As EventArgs)
            If VoucherLibObj.isLostFocus Then
                VoucherLibObj.isLostFocus = False
            End If
        End Sub

        Private Sub grdMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
            Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(num).Item("stt_rec")), "'")
            modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
        End Sub

        Private Sub grdRetrieveMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
            Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num).Item("stt_rec")), "'")
            Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
        End Sub

        Public Function InitFlowHandling(ByVal cboHandling As ComboBox) As DataTable
            Dim ds As New DataSet
            Dim num2 As Integer = 0
            cboHandling.DropDownStyle = ComboBoxStyle.DropDownList
            Dim str As String = StringType.FromObject(Reg.GetRegistryKey("Language"))
            Dim tcSQL As String = String.Concat(New String() {"fs_GetFlowHandling '", modVoucher.VoucherCode, "', '", Me.txtStatus.Text, "'"})
            Sql.SQLRetrieve((modVoucher.appConn), tcSQL, "dmxlct", (ds))
            cboHandling.Items.Clear()
            Dim table As DataTable = ds.Tables.Item("dmxlct")
            Me.tblHandling.Clear()
            Me.tblHandling = ds.Tables.Item("dmxlct")
            Dim num3 As Integer = (table.Rows.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num3)
                If (ObjectType.ObjTst(table.Rows.Item(i).Item("status"), Me.txtStatus.Text, False) = 0) Then
                    num2 = i
                End If
                Dim item As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(table.Rows.Item(i).Item("action_id"), ". "), Strings.Trim(StringType.FromObject(LateBinding.LateGet(table.Rows.Item(i), Nothing, "Item", New Object() {ObjectType.AddObj("action_name", Interaction.IIf((StringType.StrCmp(str, "V", False) = 0), "", "2"))}, Nothing, Nothing)))))
                cboHandling.Items.Add(item)
                i += 1
            Loop
            ds = Nothing
            cboHandling.SelectedIndex = num2
            Return table
        End Function
        Friend WithEvents txtMa_bp As TextBox
        Friend WithEvents Label2 As Label
        Friend WithEvents lblTen_bp As Label

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.cmdSave = New System.Windows.Forms.Button()
            Me.cmdNew = New System.Windows.Forms.Button()
            Me.cmdPrint = New System.Windows.Forms.Button()
            Me.cmdEdit = New System.Windows.Forms.Button()
            Me.cmdDelete = New System.Windows.Forms.Button()
            Me.cmdView = New System.Windows.Forms.Button()
            Me.cmdSearch = New System.Windows.Forms.Button()
            Me.cmdClose = New System.Windows.Forms.Button()
            Me.cmdOption = New System.Windows.Forms.Button()
            Me.cmdTop = New System.Windows.Forms.Button()
            Me.cmdPrev = New System.Windows.Forms.Button()
            Me.cmdNext = New System.Windows.Forms.Button()
            Me.cmdBottom = New System.Windows.Forms.Button()
            Me.lblMa_dvcs = New System.Windows.Forms.Label()
            Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
            Me.lblTen_dvcs = New System.Windows.Forms.Label()
            Me.lblSo_ct = New System.Windows.Forms.Label()
            Me.txtSo_ct = New System.Windows.Forms.TextBox()
            Me.txtNgay_lct = New libscontrol.txtDate()
            Me.txtTy_gia = New libscontrol.txtNumeric()
            Me.lblNgay_lct = New System.Windows.Forms.Label()
            Me.lblNgay_ct = New System.Windows.Forms.Label()
            Me.lblTy_gia = New System.Windows.Forms.Label()
            Me.txtNgay_ct = New libscontrol.txtDate()
            Me.cmdMa_nt = New System.Windows.Forms.Button()
            Me.tbDetail = New System.Windows.Forms.TabControl()
            Me.tpgDetail = New System.Windows.Forms.TabPage()
            Me.grdDetail = New libscontrol.clsgrid()
            Me.txtStatus = New System.Windows.Forms.TextBox()
            Me.lblStatus = New System.Windows.Forms.Label()
            Me.lblStatusMess = New System.Windows.Forms.Label()
            Me.txtKeyPress = New System.Windows.Forms.TextBox()
            Me.cboStatus = New System.Windows.Forms.ComboBox()
            Me.cboAction = New System.Windows.Forms.ComboBox()
            Me.lblAction = New System.Windows.Forms.Label()
            Me.lblTotal = New System.Windows.Forms.Label()
            Me.lblTen = New System.Windows.Forms.Label()
            Me.txtDien_giai = New System.Windows.Forms.TextBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.txtT_so_luong = New libscontrol.txtNumeric()
            Me.txtLoai_ct = New System.Windows.Forms.TextBox()
            Me.txtMa_gd = New System.Windows.Forms.TextBox()
            Me.lblMa_gd = New System.Windows.Forms.Label()
            Me.lblTen_gd = New System.Windows.Forms.Label()
            Me.txtMa_md = New System.Windows.Forms.TextBox()
            Me.lblMa_md = New System.Windows.Forms.Label()
            Me.lblTen_md = New System.Windows.Forms.Label()
            Me.lblNgay_kh1 = New System.Windows.Forms.Label()
            Me.txtNgay_kh1 = New libscontrol.txtDate()
            Me.lblNgay_kh2 = New System.Windows.Forms.Label()
            Me.txtNgay_kh2 = New libscontrol.txtDate()
            Me.lblNgay_th1 = New System.Windows.Forms.Label()
            Me.txtNgay_th1 = New libscontrol.txtDate()
            Me.lblNgay_th2 = New System.Windows.Forms.Label()
            Me.txtNgay_th2 = New libscontrol.txtDate()
            Me.txtMa_bp = New System.Windows.Forms.TextBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.lblTen_bp = New System.Windows.Forms.Label()
            Me.tbDetail.SuspendLayout()
            Me.tpgDetail.SuspendLayout()
            CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'cmdSave
            '
            Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
            Me.cmdSave.Location = New System.Drawing.Point(2, 421)
            Me.cmdSave.Name = "cmdSave"
            Me.cmdSave.Size = New System.Drawing.Size(72, 26)
            Me.cmdSave.TabIndex = 16
            Me.cmdSave.Tag = "CB01"
            Me.cmdSave.Text = "Luu"
            Me.cmdSave.UseVisualStyleBackColor = False
            '
            'cmdNew
            '
            Me.cmdNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
            Me.cmdNew.Location = New System.Drawing.Point(74, 421)
            Me.cmdNew.Name = "cmdNew"
            Me.cmdNew.Size = New System.Drawing.Size(72, 26)
            Me.cmdNew.TabIndex = 17
            Me.cmdNew.Tag = "CB02"
            Me.cmdNew.Text = "Moi"
            Me.cmdNew.UseVisualStyleBackColor = False
            '
            'cmdPrint
            '
            Me.cmdPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
            Me.cmdPrint.Location = New System.Drawing.Point(146, 421)
            Me.cmdPrint.Name = "cmdPrint"
            Me.cmdPrint.Size = New System.Drawing.Size(72, 26)
            Me.cmdPrint.TabIndex = 18
            Me.cmdPrint.Tag = "CB03"
            Me.cmdPrint.Text = "In ctu"
            Me.cmdPrint.UseVisualStyleBackColor = False
            '
            'cmdEdit
            '
            Me.cmdEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
            Me.cmdEdit.Location = New System.Drawing.Point(218, 421)
            Me.cmdEdit.Name = "cmdEdit"
            Me.cmdEdit.Size = New System.Drawing.Size(72, 26)
            Me.cmdEdit.TabIndex = 19
            Me.cmdEdit.Tag = "CB04"
            Me.cmdEdit.Text = "Sua"
            Me.cmdEdit.UseVisualStyleBackColor = False
            '
            'cmdDelete
            '
            Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
            Me.cmdDelete.Location = New System.Drawing.Point(290, 421)
            Me.cmdDelete.Name = "cmdDelete"
            Me.cmdDelete.Size = New System.Drawing.Size(72, 26)
            Me.cmdDelete.TabIndex = 20
            Me.cmdDelete.Tag = "CB05"
            Me.cmdDelete.Text = "Xoa"
            Me.cmdDelete.UseVisualStyleBackColor = False
            '
            'cmdView
            '
            Me.cmdView.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdView.BackColor = System.Drawing.SystemColors.Control
            Me.cmdView.Location = New System.Drawing.Point(362, 421)
            Me.cmdView.Name = "cmdView"
            Me.cmdView.Size = New System.Drawing.Size(72, 26)
            Me.cmdView.TabIndex = 21
            Me.cmdView.Tag = "CB06"
            Me.cmdView.Text = "Xem"
            Me.cmdView.UseVisualStyleBackColor = False
            '
            'cmdSearch
            '
            Me.cmdSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
            Me.cmdSearch.Location = New System.Drawing.Point(434, 421)
            Me.cmdSearch.Name = "cmdSearch"
            Me.cmdSearch.Size = New System.Drawing.Size(72, 26)
            Me.cmdSearch.TabIndex = 22
            Me.cmdSearch.Tag = "CB07"
            Me.cmdSearch.Text = "Tim"
            Me.cmdSearch.UseVisualStyleBackColor = False
            '
            'cmdClose
            '
            Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
            Me.cmdClose.Location = New System.Drawing.Point(506, 421)
            Me.cmdClose.Name = "cmdClose"
            Me.cmdClose.Size = New System.Drawing.Size(72, 26)
            Me.cmdClose.TabIndex = 23
            Me.cmdClose.Tag = "CB08"
            Me.cmdClose.Text = "Quay ra"
            Me.cmdClose.UseVisualStyleBackColor = False
            '
            'cmdOption
            '
            Me.cmdOption.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdOption.BackColor = System.Drawing.SystemColors.Control
            Me.cmdOption.Location = New System.Drawing.Point(524, 421)
            Me.cmdOption.Name = "cmdOption"
            Me.cmdOption.Size = New System.Drawing.Size(24, 26)
            Me.cmdOption.TabIndex = 24
            Me.cmdOption.TabStop = False
            Me.cmdOption.Tag = "CB09"
            Me.cmdOption.UseVisualStyleBackColor = False
            '
            'cmdTop
            '
            Me.cmdTop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdTop.BackColor = System.Drawing.SystemColors.Control
            Me.cmdTop.Location = New System.Drawing.Point(546, 421)
            Me.cmdTop.Name = "cmdTop"
            Me.cmdTop.Size = New System.Drawing.Size(24, 26)
            Me.cmdTop.TabIndex = 25
            Me.cmdTop.TabStop = False
            Me.cmdTop.Tag = "CB10"
            Me.cmdTop.UseVisualStyleBackColor = False
            '
            'cmdPrev
            '
            Me.cmdPrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdPrev.BackColor = System.Drawing.SystemColors.Control
            Me.cmdPrev.Location = New System.Drawing.Point(569, 421)
            Me.cmdPrev.Name = "cmdPrev"
            Me.cmdPrev.Size = New System.Drawing.Size(24, 26)
            Me.cmdPrev.TabIndex = 26
            Me.cmdPrev.TabStop = False
            Me.cmdPrev.Tag = "CB11"
            Me.cmdPrev.UseVisualStyleBackColor = False
            '
            'cmdNext
            '
            Me.cmdNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdNext.BackColor = System.Drawing.SystemColors.Control
            Me.cmdNext.Location = New System.Drawing.Point(592, 421)
            Me.cmdNext.Name = "cmdNext"
            Me.cmdNext.Size = New System.Drawing.Size(24, 26)
            Me.cmdNext.TabIndex = 27
            Me.cmdNext.TabStop = False
            Me.cmdNext.Tag = "CB12"
            Me.cmdNext.UseVisualStyleBackColor = False
            '
            'cmdBottom
            '
            Me.cmdBottom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdBottom.BackColor = System.Drawing.SystemColors.Control
            Me.cmdBottom.Location = New System.Drawing.Point(615, 421)
            Me.cmdBottom.Name = "cmdBottom"
            Me.cmdBottom.Size = New System.Drawing.Size(24, 26)
            Me.cmdBottom.TabIndex = 28
            Me.cmdBottom.TabStop = False
            Me.cmdBottom.Tag = "CB13"
            Me.cmdBottom.UseVisualStyleBackColor = False
            '
            'lblMa_dvcs
            '
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New System.Drawing.Point(326, 526)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New System.Drawing.Size(60, 17)
            Me.lblMa_dvcs.TabIndex = 13
            Me.lblMa_dvcs.Tag = "L001"
            Me.lblMa_dvcs.Text = "Ma dvcs"
            Me.lblMa_dvcs.Visible = False
            '
            'txtMa_dvcs
            '
            Me.txtMa_dvcs.BackColor = System.Drawing.Color.White
            Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New System.Drawing.Point(384, 526)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.Size = New System.Drawing.Size(120, 22)
            Me.txtMa_dvcs.TabIndex = 0
            Me.txtMa_dvcs.Tag = "FCNBCF"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            Me.txtMa_dvcs.Visible = False
            '
            'lblTen_dvcs
            '
            Me.lblTen_dvcs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New System.Drawing.Point(509, 526)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New System.Drawing.Size(113, 17)
            Me.lblTen_dvcs.TabIndex = 15
            Me.lblTen_dvcs.Tag = "FCRF"
            Me.lblTen_dvcs.Text = "Ten don vi co so"
            Me.lblTen_dvcs.Visible = False
            '
            'lblSo_ct
            '
            Me.lblSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblSo_ct.AutoSize = True
            Me.lblSo_ct.Location = New System.Drawing.Point(398, 8)
            Me.lblSo_ct.Name = "lblSo_ct"
            Me.lblSo_ct.Size = New System.Drawing.Size(45, 17)
            Me.lblSo_ct.TabIndex = 16
            Me.lblSo_ct.Tag = "L009"
            Me.lblSo_ct.Text = "So lsx"
            '
            'txtSo_ct
            '
            Me.txtSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtSo_ct.BackColor = System.Drawing.Color.White
            Me.txtSo_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtSo_ct.Location = New System.Drawing.Point(518, 6)
            Me.txtSo_ct.Name = "txtSo_ct"
            Me.txtSo_ct.Size = New System.Drawing.Size(120, 22)
            Me.txtSo_ct.TabIndex = 9
            Me.txtSo_ct.Tag = "FCNBCF"
            Me.txtSo_ct.Text = "TXTSO_CT"
            Me.txtSo_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'txtNgay_lct
            '
            Me.txtNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtNgay_lct.BackColor = System.Drawing.Color.White
            Me.txtNgay_lct.Location = New System.Drawing.Point(518, 30)
            Me.txtNgay_lct.MaxLength = 10
            Me.txtNgay_lct.Name = "txtNgay_lct"
            Me.txtNgay_lct.Size = New System.Drawing.Size(120, 22)
            Me.txtNgay_lct.TabIndex = 10
            Me.txtNgay_lct.Tag = "FDNBCFDF"
            Me.txtNgay_lct.Text = "  /  /    "
            Me.txtNgay_lct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNgay_lct.Value = New Date(CType(0, Long))
            '
            'txtTy_gia
            '
            Me.txtTy_gia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtTy_gia.BackColor = System.Drawing.Color.White
            Me.txtTy_gia.Format = "m_ip_tg"
            Me.txtTy_gia.Location = New System.Drawing.Point(256, 524)
            Me.txtTy_gia.MaxLength = 8
            Me.txtTy_gia.Name = "txtTy_gia"
            Me.txtTy_gia.Size = New System.Drawing.Size(120, 22)
            Me.txtTy_gia.TabIndex = 13
            Me.txtTy_gia.Tag = "FNCF"
            Me.txtTy_gia.Text = "m_ip_tg"
            Me.txtTy_gia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtTy_gia.Value = 0R
            Me.txtTy_gia.Visible = False
            '
            'lblNgay_lct
            '
            Me.lblNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblNgay_lct.AutoSize = True
            Me.lblNgay_lct.Location = New System.Drawing.Point(398, 32)
            Me.lblNgay_lct.Name = "lblNgay_lct"
            Me.lblNgay_lct.Size = New System.Drawing.Size(64, 17)
            Me.lblNgay_lct.TabIndex = 20
            Me.lblNgay_lct.Tag = "L010"
            Me.lblNgay_lct.Text = "Ngay lap"
            '
            'lblNgay_ct
            '
            Me.lblNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblNgay_ct.AutoSize = True
            Me.lblNgay_ct.Location = New System.Drawing.Point(-90, 526)
            Me.lblNgay_ct.Name = "lblNgay_ct"
            Me.lblNgay_ct.Size = New System.Drawing.Size(108, 17)
            Me.lblNgay_ct.TabIndex = 21
            Me.lblNgay_ct.Tag = "L011"
            Me.lblNgay_ct.Text = "Ngay hach toan"
            Me.lblNgay_ct.Visible = False
            '
            'lblTy_gia
            '
            Me.lblTy_gia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTy_gia.AutoSize = True
            Me.lblTy_gia.Location = New System.Drawing.Point(-42, 526)
            Me.lblTy_gia.Name = "lblTy_gia"
            Me.lblTy_gia.Size = New System.Drawing.Size(47, 17)
            Me.lblTy_gia.TabIndex = 22
            Me.lblTy_gia.Tag = "L012"
            Me.lblTy_gia.Text = "Ty gia"
            Me.lblTy_gia.Visible = False
            '
            'txtNgay_ct
            '
            Me.txtNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtNgay_ct.BackColor = System.Drawing.Color.White
            Me.txtNgay_ct.Location = New System.Drawing.Point(256, 524)
            Me.txtNgay_ct.MaxLength = 10
            Me.txtNgay_ct.Name = "txtNgay_ct"
            Me.txtNgay_ct.Size = New System.Drawing.Size(120, 22)
            Me.txtNgay_ct.TabIndex = 11
            Me.txtNgay_ct.Tag = "FDNBCFDF"
            Me.txtNgay_ct.Text = "  /  /    "
            Me.txtNgay_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNgay_ct.Value = New Date(CType(0, Long))
            Me.txtNgay_ct.Visible = False
            '
            'cmdMa_nt
            '
            Me.cmdMa_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdMa_nt.BackColor = System.Drawing.SystemColors.Control
            Me.cmdMa_nt.Enabled = False
            Me.cmdMa_nt.Location = New System.Drawing.Point(54, 524)
            Me.cmdMa_nt.Name = "cmdMa_nt"
            Me.cmdMa_nt.Size = New System.Drawing.Size(44, 23)
            Me.cmdMa_nt.TabIndex = 12
            Me.cmdMa_nt.TabStop = False
            Me.cmdMa_nt.Tag = "FCCFCMDDF"
            Me.cmdMa_nt.Text = "VND"
            Me.cmdMa_nt.UseVisualStyleBackColor = False
            Me.cmdMa_nt.Visible = False
            '
            'tbDetail
            '
            Me.tbDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tbDetail.Controls.Add(Me.tpgDetail)
            Me.tbDetail.Location = New System.Drawing.Point(2, 185)
            Me.tbDetail.Name = "tbDetail"
            Me.tbDetail.SelectedIndex = 0
            Me.tbDetail.Size = New System.Drawing.Size(638, 194)
            Me.tbDetail.TabIndex = 14
            '
            'tpgDetail
            '
            Me.tpgDetail.BackColor = System.Drawing.SystemColors.Control
            Me.tpgDetail.Controls.Add(Me.grdDetail)
            Me.tpgDetail.Location = New System.Drawing.Point(4, 25)
            Me.tpgDetail.Name = "tpgDetail"
            Me.tpgDetail.Size = New System.Drawing.Size(630, 165)
            Me.tpgDetail.TabIndex = 0
            Me.tpgDetail.Tag = "L016"
            Me.tpgDetail.Text = "Chung tu"
            '
            'grdDetail
            '
            Me.grdDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grdDetail.BackgroundColor = System.Drawing.Color.White
            Me.grdDetail.CaptionBackColor = System.Drawing.SystemColors.Control
            Me.grdDetail.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grdDetail.CaptionForeColor = System.Drawing.Color.Black
            Me.grdDetail.CaptionText = "F4 - Them, F8 - Xoa"
            Me.grdDetail.Cell_EnableRaisingEvents = False
            Me.grdDetail.DataMember = ""
            Me.grdDetail.HeaderForeColor = System.Drawing.SystemColors.ControlText
            Me.grdDetail.Location = New System.Drawing.Point(-1, -1)
            Me.grdDetail.Name = "grdDetail"
            Me.grdDetail.Size = New System.Drawing.Size(631, 162)
            Me.grdDetail.TabIndex = 0
            Me.grdDetail.Tag = "L020CF"
            '
            'txtStatus
            '
            Me.txtStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.txtStatus.BackColor = System.Drawing.Color.White
            Me.txtStatus.Location = New System.Drawing.Point(10, 451)
            Me.txtStatus.MaxLength = 1
            Me.txtStatus.Name = "txtStatus"
            Me.txtStatus.Size = New System.Drawing.Size(30, 22)
            Me.txtStatus.TabIndex = 41
            Me.txtStatus.TabStop = False
            Me.txtStatus.Tag = "FCCF"
            Me.txtStatus.Text = "txtStatus"
            Me.txtStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtStatus.Visible = False
            '
            'lblStatus
            '
            Me.lblStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblStatus.AutoSize = True
            Me.lblStatus.Location = New System.Drawing.Point(398, 57)
            Me.lblStatus.Name = "lblStatus"
            Me.lblStatus.Size = New System.Drawing.Size(73, 17)
            Me.lblStatus.TabIndex = 29
            Me.lblStatus.Tag = ""
            Me.lblStatus.Text = "Trang thai"
            '
            'lblStatusMess
            '
            Me.lblStatusMess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblStatusMess.AutoSize = True
            Me.lblStatusMess.Location = New System.Drawing.Point(58, 453)
            Me.lblStatusMess.Name = "lblStatusMess"
            Me.lblStatusMess.Size = New System.Drawing.Size(253, 17)
            Me.lblStatusMess.TabIndex = 42
            Me.lblStatusMess.Tag = ""
            Me.lblStatusMess.Text = "1 - Ghi vao SC, 0 - Chua ghi vao so cai"
            Me.lblStatusMess.Visible = False
            '
            'txtKeyPress
            '
            Me.txtKeyPress.Location = New System.Drawing.Point(518, 157)
            Me.txtKeyPress.Name = "txtKeyPress"
            Me.txtKeyPress.Size = New System.Drawing.Size(12, 11)
            Me.txtKeyPress.TabIndex = 13
            '
            'cboStatus
            '
            Me.cboStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboStatus.BackColor = System.Drawing.Color.White
            Me.cboStatus.Enabled = False
            Me.cboStatus.Location = New System.Drawing.Point(470, 54)
            Me.cboStatus.Name = "cboStatus"
            Me.cboStatus.Size = New System.Drawing.Size(168, 24)
            Me.cboStatus.TabIndex = 11
            Me.cboStatus.TabStop = False
            Me.cboStatus.Tag = ""
            Me.cboStatus.Text = "cboStatus"
            '
            'cboAction
            '
            Me.cboAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboAction.BackColor = System.Drawing.Color.White
            Me.cboAction.Location = New System.Drawing.Point(470, 78)
            Me.cboAction.Name = "cboAction"
            Me.cboAction.Size = New System.Drawing.Size(168, 24)
            Me.cboAction.TabIndex = 12
            Me.cboAction.TabStop = False
            Me.cboAction.Tag = "CF"
            Me.cboAction.Text = "cboAction"
            '
            'lblAction
            '
            Me.lblAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblAction.AutoSize = True
            Me.lblAction.Location = New System.Drawing.Point(398, 81)
            Me.lblAction.Name = "lblAction"
            Me.lblAction.Size = New System.Drawing.Size(39, 17)
            Me.lblAction.TabIndex = 9
            Me.lblAction.Tag = ""
            Me.lblAction.Text = "Xu ly"
            '
            'lblTotal
            '
            Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTotal.AutoSize = True
            Me.lblTotal.Location = New System.Drawing.Point(396, 392)
            Me.lblTotal.Name = "lblTotal"
            Me.lblTotal.Size = New System.Drawing.Size(76, 17)
            Me.lblTotal.TabIndex = 60
            Me.lblTotal.Tag = "L013"
            Me.lblTotal.Text = "Tong cong"
            '
            'lblTen
            '
            Me.lblTen.AutoSize = True
            Me.lblTen.Location = New System.Drawing.Point(689, 526)
            Me.lblTen.Name = "lblTen"
            Me.lblTen.Size = New System.Drawing.Size(76, 17)
            Me.lblTen.TabIndex = 68
            Me.lblTen.Tag = "RF"
            Me.lblTen.Text = "Ten chung"
            Me.lblTen.Visible = False
            '
            'txtDien_giai
            '
            Me.txtDien_giai.BackColor = System.Drawing.Color.White
            Me.txtDien_giai.Location = New System.Drawing.Point(106, 151)
            Me.txtDien_giai.Name = "txtDien_giai"
            Me.txtDien_giai.Size = New System.Drawing.Size(404, 22)
            Me.txtDien_giai.TabIndex = 8
            Me.txtDien_giai.Tag = "FCCF"
            Me.txtDien_giai.Text = "txtDien_giai"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(2, 153)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(63, 17)
            Me.Label1.TabIndex = 75
            Me.Label1.Tag = "L029"
            Me.Label1.Text = "Dien giai"
            '
            'txtT_so_luong
            '
            Me.txtT_so_luong.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtT_so_luong.BackColor = System.Drawing.Color.White
            Me.txtT_so_luong.Enabled = False
            Me.txtT_so_luong.ForeColor = System.Drawing.Color.Black
            Me.txtT_so_luong.Format = "m_ip_sl"
            Me.txtT_so_luong.Location = New System.Drawing.Point(518, 390)
            Me.txtT_so_luong.MaxLength = 8
            Me.txtT_so_luong.Name = "txtT_so_luong"
            Me.txtT_so_luong.Size = New System.Drawing.Size(120, 22)
            Me.txtT_so_luong.TabIndex = 15
            Me.txtT_so_luong.Tag = "FN"
            Me.txtT_so_luong.Text = "m_ip_sl"
            Me.txtT_so_luong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtT_so_luong.Value = 0R
            '
            'txtLoai_ct
            '
            Me.txtLoai_ct.BackColor = System.Drawing.Color.White
            Me.txtLoai_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtLoai_ct.Location = New System.Drawing.Point(624, 526)
            Me.txtLoai_ct.Name = "txtLoai_ct"
            Me.txtLoai_ct.Size = New System.Drawing.Size(36, 22)
            Me.txtLoai_ct.TabIndex = 85
            Me.txtLoai_ct.Tag = "FC"
            Me.txtLoai_ct.Text = "TXTLOAI_CT"
            Me.txtLoai_ct.Visible = False
            '
            'txtMa_gd
            '
            Me.txtMa_gd.BackColor = System.Drawing.Color.White
            Me.txtMa_gd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_gd.Location = New System.Drawing.Point(106, 6)
            Me.txtMa_gd.Name = "txtMa_gd"
            Me.txtMa_gd.Size = New System.Drawing.Size(36, 22)
            Me.txtMa_gd.TabIndex = 0
            Me.txtMa_gd.Tag = "FCNBCF"
            Me.txtMa_gd.Text = "TXTMA_GD"
            '
            'lblMa_gd
            '
            Me.lblMa_gd.AutoSize = True
            Me.lblMa_gd.Location = New System.Drawing.Point(2, 8)
            Me.lblMa_gd.Name = "lblMa_gd"
            Me.lblMa_gd.Size = New System.Drawing.Size(88, 17)
            Me.lblMa_gd.TabIndex = 87
            Me.lblMa_gd.Tag = "L003"
            Me.lblMa_gd.Text = "Ma giao dich"
            '
            'lblTen_gd
            '
            Me.lblTen_gd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTen_gd.Location = New System.Drawing.Point(145, 9)
            Me.lblTen_gd.Name = "lblTen_gd"
            Me.lblTen_gd.Size = New System.Drawing.Size(237, 18)
            Me.lblTen_gd.TabIndex = 88
            Me.lblTen_gd.Tag = "FCRF"
            Me.lblTen_gd.Text = "Ten giao dich"
            '
            'txtMa_md
            '
            Me.txtMa_md.BackColor = System.Drawing.Color.White
            Me.txtMa_md.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_md.Location = New System.Drawing.Point(106, 30)
            Me.txtMa_md.Name = "txtMa_md"
            Me.txtMa_md.Size = New System.Drawing.Size(36, 22)
            Me.txtMa_md.TabIndex = 1
            Me.txtMa_md.Tag = "FCNBCF"
            Me.txtMa_md.Text = "TXTMA_MD"
            '
            'lblMa_md
            '
            Me.lblMa_md.AutoSize = True
            Me.lblMa_md.Location = New System.Drawing.Point(2, 32)
            Me.lblMa_md.Name = "lblMa_md"
            Me.lblMa_md.Size = New System.Drawing.Size(54, 17)
            Me.lblMa_md.TabIndex = 132
            Me.lblMa_md.Tag = "L004"
            Me.lblMa_md.Text = "Muc do"
            '
            'lblTen_md
            '
            Me.lblTen_md.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTen_md.Location = New System.Drawing.Point(145, 33)
            Me.lblTen_md.Name = "lblTen_md"
            Me.lblTen_md.Size = New System.Drawing.Size(237, 18)
            Me.lblTen_md.TabIndex = 133
            Me.lblTen_md.Tag = "FCRF"
            Me.lblTen_md.Text = "Ten muc do"
            '
            'lblNgay_kh1
            '
            Me.lblNgay_kh1.AutoSize = True
            Me.lblNgay_kh1.Location = New System.Drawing.Point(2, 105)
            Me.lblNgay_kh1.Name = "lblNgay_kh1"
            Me.lblNgay_kh1.Size = New System.Drawing.Size(84, 17)
            Me.lblNgay_kh1.TabIndex = 135
            Me.lblNgay_kh1.Tag = "L005"
            Me.lblNgay_kh1.Text = "Ke hoach tu"
            '
            'txtNgay_kh1
            '
            Me.txtNgay_kh1.BackColor = System.Drawing.Color.White
            Me.txtNgay_kh1.Location = New System.Drawing.Point(106, 103)
            Me.txtNgay_kh1.MaxLength = 10
            Me.txtNgay_kh1.Name = "txtNgay_kh1"
            Me.txtNgay_kh1.Size = New System.Drawing.Size(120, 22)
            Me.txtNgay_kh1.TabIndex = 4
            Me.txtNgay_kh1.Tag = "FDCF"
            Me.txtNgay_kh1.Text = "  /  /    "
            Me.txtNgay_kh1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNgay_kh1.Value = New Date(CType(0, Long))
            '
            'lblNgay_kh2
            '
            Me.lblNgay_kh2.AutoSize = True
            Me.lblNgay_kh2.Location = New System.Drawing.Point(230, 105)
            Me.lblNgay_kh2.Name = "lblNgay_kh2"
            Me.lblNgay_kh2.Size = New System.Drawing.Size(34, 17)
            Me.lblNgay_kh2.TabIndex = 137
            Me.lblNgay_kh2.Tag = "L006"
            Me.lblNgay_kh2.Text = "Den"
            '
            'txtNgay_kh2
            '
            Me.txtNgay_kh2.BackColor = System.Drawing.Color.White
            Me.txtNgay_kh2.Location = New System.Drawing.Point(390, 103)
            Me.txtNgay_kh2.MaxLength = 10
            Me.txtNgay_kh2.Name = "txtNgay_kh2"
            Me.txtNgay_kh2.Size = New System.Drawing.Size(120, 22)
            Me.txtNgay_kh2.TabIndex = 5
            Me.txtNgay_kh2.Tag = "FDCF"
            Me.txtNgay_kh2.Text = "  /  /    "
            Me.txtNgay_kh2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNgay_kh2.Value = New Date(CType(0, Long))
            '
            'lblNgay_th1
            '
            Me.lblNgay_th1.AutoSize = True
            Me.lblNgay_th1.Location = New System.Drawing.Point(2, 129)
            Me.lblNgay_th1.Name = "lblNgay_th1"
            Me.lblNgay_th1.Size = New System.Drawing.Size(87, 17)
            Me.lblNgay_th1.TabIndex = 139
            Me.lblNgay_th1.Tag = "L007"
            Me.lblNgay_th1.Text = "Thuc hien tu"
            '
            'txtNgay_th1
            '
            Me.txtNgay_th1.BackColor = System.Drawing.Color.White
            Me.txtNgay_th1.Location = New System.Drawing.Point(106, 127)
            Me.txtNgay_th1.MaxLength = 10
            Me.txtNgay_th1.Name = "txtNgay_th1"
            Me.txtNgay_th1.Size = New System.Drawing.Size(120, 22)
            Me.txtNgay_th1.TabIndex = 6
            Me.txtNgay_th1.Tag = "FDCF"
            Me.txtNgay_th1.Text = "  /  /    "
            Me.txtNgay_th1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNgay_th1.Value = New Date(CType(0, Long))
            '
            'lblNgay_th2
            '
            Me.lblNgay_th2.AutoSize = True
            Me.lblNgay_th2.Location = New System.Drawing.Point(230, 129)
            Me.lblNgay_th2.Name = "lblNgay_th2"
            Me.lblNgay_th2.Size = New System.Drawing.Size(34, 17)
            Me.lblNgay_th2.TabIndex = 141
            Me.lblNgay_th2.Tag = "L008"
            Me.lblNgay_th2.Text = "Den"
            '
            'txtNgay_th2
            '
            Me.txtNgay_th2.BackColor = System.Drawing.Color.White
            Me.txtNgay_th2.Location = New System.Drawing.Point(390, 127)
            Me.txtNgay_th2.MaxLength = 10
            Me.txtNgay_th2.Name = "txtNgay_th2"
            Me.txtNgay_th2.Size = New System.Drawing.Size(120, 22)
            Me.txtNgay_th2.TabIndex = 7
            Me.txtNgay_th2.Tag = "FDCF"
            Me.txtNgay_th2.Text = "  /  /    "
            Me.txtNgay_th2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNgay_th2.Value = New Date(CType(0, Long))
            '
            'txtMa_bp
            '
            Me.txtMa_bp.BackColor = System.Drawing.Color.White
            Me.txtMa_bp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_bp.Location = New System.Drawing.Point(106, 78)
            Me.txtMa_bp.Name = "txtMa_bp"
            Me.txtMa_bp.Size = New System.Drawing.Size(120, 22)
            Me.txtMa_bp.TabIndex = 3
            Me.txtMa_bp.Tag = "FCNBCF"
            Me.txtMa_bp.Text = "TXTMA_BP"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(2, 81)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(61, 17)
            Me.Label2.TabIndex = 143
            Me.Label2.Tag = "L021"
            Me.Label2.Text = "Bo phan"
            '
            'lblTen_bp
            '
            Me.lblTen_bp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTen_bp.Location = New System.Drawing.Point(230, 82)
            Me.lblTen_bp.Name = "lblTen_bp"
            Me.lblTen_bp.Size = New System.Drawing.Size(152, 17)
            Me.lblTen_bp.TabIndex = 144
            Me.lblTen_bp.Tag = "FCRF"
            Me.lblTen_bp.Text = "Ten Bo phan"
            '
            'frmVoucher
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
            Me.ClientSize = New System.Drawing.Size(642, 473)
            Me.Controls.Add(Me.txtMa_bp)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.lblNgay_th2)
            Me.Controls.Add(Me.lblNgay_th1)
            Me.Controls.Add(Me.txtNgay_th1)
            Me.Controls.Add(Me.lblNgay_kh2)
            Me.Controls.Add(Me.txtNgay_kh2)
            Me.Controls.Add(Me.lblNgay_kh1)
            Me.Controls.Add(Me.txtNgay_kh1)
            Me.Controls.Add(Me.txtMa_md)
            Me.Controls.Add(Me.lblMa_md)
            Me.Controls.Add(Me.lblMa_dvcs)
            Me.Controls.Add(Me.lblStatusMess)
            Me.Controls.Add(Me.txtMa_gd)
            Me.Controls.Add(Me.lblMa_gd)
            Me.Controls.Add(Me.txtLoai_ct)
            Me.Controls.Add(Me.txtT_so_luong)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.lblTen)
            Me.Controls.Add(Me.lblTotal)
            Me.Controls.Add(Me.lblAction)
            Me.Controls.Add(Me.txtKeyPress)
            Me.Controls.Add(Me.lblStatus)
            Me.Controls.Add(Me.lblNgay_lct)
            Me.Controls.Add(Me.lblSo_ct)
            Me.Controls.Add(Me.txtStatus)
            Me.Controls.Add(Me.txtNgay_lct)
            Me.Controls.Add(Me.txtSo_ct)
            Me.Controls.Add(Me.lblTen_dvcs)
            Me.Controls.Add(Me.txtDien_giai)
            Me.Controls.Add(Me.lblNgay_ct)
            Me.Controls.Add(Me.lblTy_gia)
            Me.Controls.Add(Me.txtNgay_th2)
            Me.Controls.Add(Me.txtMa_dvcs)
            Me.Controls.Add(Me.txtNgay_ct)
            Me.Controls.Add(Me.txtTy_gia)
            Me.Controls.Add(Me.lblTen_bp)
            Me.Controls.Add(Me.lblTen_md)
            Me.Controls.Add(Me.lblTen_gd)
            Me.Controls.Add(Me.cboAction)
            Me.Controls.Add(Me.cboStatus)
            Me.Controls.Add(Me.tbDetail)
            Me.Controls.Add(Me.cmdMa_nt)
            Me.Controls.Add(Me.cmdBottom)
            Me.Controls.Add(Me.cmdNext)
            Me.Controls.Add(Me.cmdPrev)
            Me.Controls.Add(Me.cmdTop)
            Me.Controls.Add(Me.cmdOption)
            Me.Controls.Add(Me.cmdClose)
            Me.Controls.Add(Me.cmdSearch)
            Me.Controls.Add(Me.cmdView)
            Me.Controls.Add(Me.cmdDelete)
            Me.Controls.Add(Me.cmdEdit)
            Me.Controls.Add(Me.cmdPrint)
            Me.Controls.Add(Me.cmdNew)
            Me.Controls.Add(Me.cmdSave)
            Me.Name = "frmVoucher"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmVoucher"
            Me.tbDetail.ResumeLayout(False)
            Me.tpgDetail.ResumeLayout(False)
            CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Private Sub InitInventory()
            Me.xInventory.ColItem = Me.colMa_vt
            Me.xInventory.ColLot = Me.colMa_lo
            Me.xInventory.ColSite = Me.colMa_kho
            Me.xInventory.ColLocation = Me.colMa_vi_tri
            Me.xInventory.ColUOM = Me.colDvt
            Me.xInventory.colQty = Me.colSo_luong
            Me.xInventory.txtUnit = Me.txtMa_dvcs
            Me.xInventory.InvVoucher = Me.oVoucher
            Me.xInventory.oInvItem = Me.oInvItemDetail
            Me.xInventory.oInvSite = Me.oSite
            Me.xInventory.oInvLocation = Me.oLocation
            Me.xInventory.oInvLot = Me.oLot
            Me.xInventory.oInvUOM = Me.oUOM
            Me.xInventory.Init()
        End Sub

        Public Sub InitRecords()
            Dim str As String
            If oVoucher.isRead Then
                str = String.Concat(New String() {"EXEC fs_LoadMOTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', -1"})
            Else
                str = String.Concat(New String() {"EXEC fs_LoadMOTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
            End If
            str = (str & GetLoadParameters())
            Dim ds As New DataSet
            Sql.SQLDecompressRetrieve((modVoucher.appConn), str, "trantmp", (ds))
            AppendFrom(modVoucher.tblMaster, ds.Tables.Item(0))
            AppendFrom(modVoucher.tblDetail, ds.Tables.Item(1))
            If (modVoucher.tblMaster.Count > 0) Then
                Me.iMasterRow = 0
                Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'")
                modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
                oVoucher.cAction = "View"
                If (modVoucher.tblMaster.Count = 1) Then
                    Me.RefrehForm()
                Else
                    Me.View()
                End If
                oVoucher.RefreshButton(oVoucher.ctrlButtons, oVoucher.cAction)
                If (modVoucher.tblMaster.Count = 1) Then
                    Me.cmdEdit.Focus()
                End If
            Else
                Me.cmdNew.Focus()
            End If
            ds = Nothing
        End Sub

        Private Function isEdit() As Boolean
            Return True
            Dim row As DataRow
            row = Sql.GetRow(appConn, "select top 1 * from ct00 where so_lsx='" + tblMaster.Item(Me.iMasterRow).Item("so_ct") + "' order by ngay_ct,so_ct")
            If Not row Is Nothing Then
                If row("stt_rec") <> "" Then
                    Dim str As String
                    str = oLan.Item("019")
                    str = Replace(str, "%s1", Trim(Sql.GetValue(appConn, "dmct", "ten_ct", "ma_ct='" + Trim(row("ma_ct")) + "'")))
                    str = Replace(str, "%s2", Trim(row("so_ct")))
                    Dim d As Date
                    d = row("ngay_ct")
                    str = Replace(str, "%s3", d.ToShortDateString)
                    MsgBox(str)
                    Return False
                End If
            End If
            Return True
        End Function
        Private Function isDelete() As Boolean
            Dim row As DataRow
            row = Sql.GetRow(appConn, "select top 1 * from ct00 where so_lsx='" + tblMaster.Item(Me.iMasterRow).Item("so_ct") + "' order by ngay_ct,so_ct")
            If Not row Is Nothing Then
                If row("stt_rec") <> "" Then
                    Dim str As String
                    str = oLan.Item("019")
                    str = Replace(str, "%s1", Trim(Sql.GetValue(appConn, "dmct", "ten_ct", "ma_ct='" + Trim(row("ma_ct")) + "'")))
                    str = Replace(str, "%s2", Trim(row("so_ct")))
                    Dim d As Date
                    d = row("ngay_ct")
                    str = Replace(str, "%s3", d.ToShortDateString)
                    MsgBox(str)
                    Return False
                End If
            End If
            Return True
        End Function

        Private Sub NewItem(ByVal sender As Object, ByVal e As EventArgs)
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
                Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
                If (currentRowIndex < 0) Then
                    modVoucher.tblDetail.AddNew()
                    Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
                ElseIf ((Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) AndAlso Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt")))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt"))), "", False) <> 0)) Then
                    Dim count As Integer = modVoucher.tblDetail.Count
                    Me.grdDetail.BeforeAddNewItem()
                    Me.grdDetail.CurrentCell = New DataGridCell(count, 0)
                    Me.grdDetail.AfterAddNewItem()
                End If
            End If
        End Sub

        Public Sub Options(ByVal nIndex As Integer)
            If (StringType.StrCmp(oVoucher.cAction, "View", False) = 0) Then
                Select Case nIndex
                    Case 0
                        Dim view As DataRowView = modVoucher.tblMaster.Item(Me.iMasterRow)
                        oVoucher.ShowUserInfor(IntegerType.FromObject(view.Item("user_id0")), IntegerType.FromObject(view.Item("user_id2")), DateType.FromObject(view.Item("datetime0")), DateType.FromObject(view.Item("datetime2")))
                        view = Nothing
                        Exit Select
                    Case 2
                        oVoucher.ViewDeletedRecord("fs_SearchDeletedMOTran", "MOMaster", "MODetail", "t_tien", "t_tien_nt")
                        Exit Select
                End Select
            End If
        End Sub

        Private Function Post() As String
            Dim str2 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "groupby", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            Dim str3 As String = "EXEC fs_PostMO "
            Return (StringType.FromObject(ObjectType.AddObj(((((((str3 & "'" & modVoucher.VoucherCode & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))) & "'") & ", '" & Strings.Trim(str2) & "'"), ObjectType.AddObj(ObjectType.AddObj(", '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))) & ", 1")
        End Function

        Public Sub Print()
            Dim print As New frmPrint
            print.txtTitle.Text = StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct"))), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct2")))))
            print.txtSo_lien.Value = DoubleType.FromObject(modVoucher.oVoucherRow.Item("so_lien"))
            Dim table As DataTable = clsprint.InitComboReport(modVoucher.sysConn, print.cboReports, "MOTran")
            Dim result As DialogResult = print.ShowDialog
            If ((result <> DialogResult.Cancel) AndAlso (print.txtSo_lien.Value > 0)) Then
                Dim selectedIndex As Integer = print.cboReports.SelectedIndex
                Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
                Dim view As New DataView
                Dim ds As New DataSet
                Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj((("EXEC fs_PrintMOTran '" & modVoucher.cLan) & "', " & "[a.stt_rec = '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'], '"), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf")))), "'"))
                tcSQL += ",'" + Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id"))) + "'"
                Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "cttmp", (ds))
                Dim num4 As Integer = IntegerType.FromObject(modVoucher.oVoucherRow.Item("max_row"))
                view.Table = ds.Tables.Item("cttmp")
                Dim num6 As Integer = num4
                Dim i As Integer = view.Count
                Do While (i <= num6)
                    view.AddNew()
                    i += 1
                Loop
                Dim clsprint As New clsprint(Me, strFile, Nothing)
                clsprint.oRpt.SetDataSource(view.Table)
                clsprint.oVar = modVoucher.oVar
                clsprint.dr = modVoucher.tblMaster.Item(Me.iMasterRow).Row
                clsprint.SetReportVar(modVoucher.sysConn, modVoucher.appConn, "MOTran", modVoucher.oOption, clsprint.oRpt)
                clsprint.oRpt.SetParameterValue("Title", Strings.Trim(print.txtTitle.Text))
                Dim str As String = Strings.Replace(Strings.Replace(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("401")), "%s1", Me.txtNgay_ct.Value.Day.ToString, 1, -1, 0), "%s2", Me.txtNgay_ct.Value.Month.ToString, 1, -1, 0), "%s3", Me.txtNgay_ct.Value.Year.ToString, 1, -1, 0)
                Dim str3 As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("402")), "%s", Strings.Trim(Me.txtSo_ct.Text), 1, -1, 0)
                clsprint.oRpt.SetParameterValue("t_date", str)
                clsprint.oRpt.SetParameterValue("t_number", str3)
                clsprint.oRpt.SetParameterValue("f_bp", (Strings.Trim(Me.txtMa_bp.Text) & " - " & Strings.Trim(Me.lblTen_bp.Text)))
                Dim str2 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmbp", "dia_chi", ("ma_bp = '" & Strings.Trim(Me.txtMa_bp.Text) & "'"))))
                clsprint.oRpt.SetParameterValue("f_dia_chi", str2)
                str2 = Strings.Trim(Me.txtDien_giai.Text)
                clsprint.oRpt.SetParameterValue("f_dien_giai", str2)
                If (result = DialogResult.OK) Then
                    clsprint.PrintReport(CInt(Math.Round(print.txtSo_lien.Value)))
                    clsprint.oRpt.SetDataSource(view.Table)
                Else
                    clsprint.ShowReports()
                End If
                clsprint.oRpt.Close()
                ds = Nothing
                table = Nothing
                print.Dispose()
            End If
        End Sub

        Public Sub RefrehForm()
            Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
            Me.grdHeader.Scatter()
            ScatterMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
            Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'")
            modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
            Me.EDTranType()
            Me.UpdateList()
            Me.vCaptionRefresh()
            Me.cmdNew.Focus()
        End Sub

        Private Sub RefreshControlField()
        End Sub

        Private Sub RetrieveItems(ByVal sender As Object, ByVal e As EventArgs)
            Dim cancel As Boolean = Me.oInvItemDetail.Cancel
            Me.oInvItemDetail.Cancel = True
            If (IntegerType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing)) = 0) Then
                Me.RetrieveItemsFromSO()
            End If
            Me.oInvItemDetail.Cancel = cancel
        End Sub

        Private Sub RetrieveItemsFromSO()
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
                Dim _date As New frmDate
                AddHandler _date.Load, New EventHandler(AddressOf Me.frmRetrieveLoad)
                If (_date.ShowDialog = DialogResult.OK) Then
                    Dim strSQLLong As String = "1=1"
                    If (ObjectType.ObjTst(_date.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        strSQLLong = StringType.FromObject(ObjectType.AddObj(strSQLLong, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct >= ", Sql.ConvertVS2SQLType(_date.txtNgay_ct.Value, "")), ")")))
                    End If
                    If (ObjectType.ObjTst(Me.txtNgay_lct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        strSQLLong = StringType.FromObject(ObjectType.AddObj(strSQLLong, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct <= ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")), ")")))
                    End If
                    Dim str As String = strSQLLong
                    strSQLLong = (strSQLLong & " AND a.ma_kh LIKE '" & Strings.Trim(_date.txtMa_kh.Text) & "%'")
                    Dim tcSQL As String
                    'tcSQL = String.Concat(New String() {"EXEC fs_SearchSOTran4MO '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(str, 10), ", 'ph64', 'ct64'"})
                    tcSQL = "EXEC spSearchSOTran4MO '" + modVoucher.cLan + "'"
                    If (ObjectType.ObjTst(Me.txtNgay_lct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        tcSQL += "," + Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")
                    Else
                        tcSQL += ",null"
                    End If
                    If (ObjectType.ObjTst(_date.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        tcSQL += "," + Sql.ConvertVS2SQLType(_date.txtNgay_ct.Value, "")
                    Else
                        tcSQL += ",null"
                    End If
                    tcSQL += "," + Sql.ConvertVS2SQLType(_date.txtMa_kh.Text.Trim, "")
                    tcSQL += "," + Sql.ConvertVS2SQLType(_date.txtMa_vt.Text.Trim, "")

                    Dim ds As New DataSet
                    Sql.SQLRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
                    Me.tblRetrieveMaster = New DataView
                    Me.tblRetrieveDetail = New DataView
                    If (ds.Tables.Item(0).Rows.Count <= 0) Then
                        Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
                    Else
                        Me.tblRetrieveMaster.Table = ds.Tables.Item(0)
                        Me.tblRetrieveDetail.Table = ds.Tables.Item(1)
                        Dim frmAdd As New Form
                        Dim gridformtran2 As New gridformtran
                        Dim gridformtran As New gridformtran
                        Dim tbs As New DataGridTableStyle
                        Dim style As New DataGridTableStyle
                        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H1F - 1) {}
                        Dim index As Integer = 0
                        Do
                            cols(index) = New DataGridTextBoxColumn
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H1D)
                        frmAdd.Top = 0
                        frmAdd.Left = 0
                        frmAdd.Width = Me.Width
                        frmAdd.Height = Me.Height
                        frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("059"))
                        frmAdd.StartPosition = FormStartPosition.CenterParent
                        Dim panel As StatusBarPanel = AddStb(frmAdd)
                        gridformtran2.CaptionVisible = False
                        gridformtran2.ReadOnly = True
                        gridformtran2.Top = 0
                        gridformtran2.Left = 0
                        gridformtran2.Height = CInt(Math.Round(CDbl(((CDbl(Me.Height) / 2) - 10))))
                        gridformtran2.Width = (Me.Width - 5)
                        gridformtran2.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
                        gridformtran2.BackgroundColor = Color.White
                        gridformtran.CaptionVisible = False
                        gridformtran.ReadOnly = False
                        gridformtran.Top = CInt(Math.Round(CDbl((CDbl((Me.Height - 20)) / 2))))
                        gridformtran.Left = 0
                        gridformtran.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - 20)) / 2) - 60))))
                        gridformtran.Width = (Me.Width - 5)
                        gridformtran.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
                        gridformtran.BackgroundColor = Color.White
                        Dim button As New Button
                        button.Visible = True
                        button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
                        button.Left = (-100 - button.Width)
                        frmAdd.Controls.Add(button)
                        frmAdd.CancelButton = button
                        frmAdd.Controls.Add(gridformtran2)
                        frmAdd.Controls.Add(gridformtran)
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "SOMaster")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H1D)
                        cols(2).Alignment = HorizontalAlignment.Right
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "SODetail4MO")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H1D)
                        Me.tblRetrieveDetail.AllowDelete = False
                        Me.tblRetrieveDetail.AllowNew = False
                        gridformtran.TableStyles.Item(0).GridColumnStyles.Item(0).ReadOnly = True
                        gridformtran.TableStyles.Item(0).GridColumnStyles.Item(1).ReadOnly = True
                        gridformtran.TableStyles.Item(0).GridColumnStyles.Item(2).ReadOnly = True
                        index = 3
                        Do While (1 <> 0)
                            Try
                                index += 1
                                gridformtran.TableStyles.Item(0).GridColumnStyles.Item(index).ReadOnly = True
                            Catch exception1 As Exception
                                ProjectData.SetProjectError(exception1)
                                Dim exception As Exception = exception1
                                ProjectData.ClearProjectError()
                                Exit Do
                            End Try
                        Loop
                        Dim str5 As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
                        Dim zero As Decimal = Decimal.Zero
                        Dim num4 As Decimal = Decimal.Zero
                        Dim count As Integer = Me.tblRetrieveMaster.Count
                        Dim num8 As Integer = (count - 1)
                        index = 0
                        Do While (index <= num8)
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien2"))) Then
                                zero = DecimalType.FromObject(ObjectType.AddObj(zero, Me.tblRetrieveMaster.Item(index).Item("t_tien2")))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2"))) Then
                                num4 = DecimalType.FromObject(ObjectType.AddObj(num4, Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2")))
                            End If
                            index += 1
                        Loop
                        str5 = Strings.Replace(Strings.Replace(Strings.Replace(str5, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, 0), "%n2", Strings.Trim(Strings.Format(num4, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))), 1, -1, 0), "%n3", Strings.Trim(Strings.Format(zero, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))), 1, -1, 0)
                        panel.Text = str5
                        AddHandler gridformtran2.CurrentCellChanged, New EventHandler(AddressOf Me.grdRetrieveMVCurrentCellChanged)
                        gridformtran2.CurrentRowIndex = 0
                        Dim num2 As Integer = 0
                        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num2).Item("stt_rec")), "'")
                        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
                        Obj.Init(frmAdd)
                        Dim button4 As New RadioButton
                        Dim button2 As New RadioButton
                        Dim button3 As New RadioButton
                        button4.Top = CInt(Math.Round(CDbl((((CDbl((Me.Height - 20)) / 2) + gridformtran.Height) + 5))))
                        button4.Left = 0
                        button4.Visible = True
                        button4.Checked = True
                        button4.Text = StringType.FromObject(modVoucher.oLan.Item("060"))
                        button4.Width = 100
                        button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Top = button4.Top
                        button2.Left = (button4.Left + 110)
                        button2.Visible = True
                        button2.Text = StringType.FromObject(modVoucher.oLan.Item("061"))
                        button2.Width = 120
                        button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Enabled = False
                        button3.Top = button4.Top
                        button3.Left = (button2.Left + 130)
                        button3.Visible = True
                        button3.Text = StringType.FromObject(modVoucher.oLan.Item("062"))
                        button3.Width = 200
                        button3.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        frmAdd.Controls.Add(button4)
                        frmAdd.Controls.Add(button2)
                        frmAdd.Controls.Add(button3)
                        frmAdd.ShowDialog()
                        If button4.Checked Then
                            ds = Nothing
                            Me.tblRetrieveMaster = Nothing
                            Me.tblRetrieveDetail = Nothing
                            Return
                        End If
                        Me.tblRetrieveDetail.RowFilter = ""
                        Me.tblRetrieveDetail.Sort = "ngay_ct, so_ct, stt_rec, stt_rec0"
                        Dim num7 As Integer = (Me.tblRetrieveDetail.Count - 1)
                        index = 0
                        Do While (index <= num7)
                            Me.tblRetrieveDetail.Item(index).Item("so_luong") = RuntimeHelpers.GetObjectValue(Me.tblRetrieveDetail.Item(index).Item("sl_dh0"))
                            Me.tblRetrieveDetail.Item(index).Row.AcceptChanges()
                            index += 1
                        Loop
                        Me.tblRetrieveDetail.RowFilter = "sl_dh0 <> 0"
                        Dim flag As Boolean = (Me.tblRetrieveDetail.Count > 0)
                        count = (modVoucher.tblDetail.Count - 1)
                        If ((button3.Checked And flag) And (count >= 0)) Then
                            index = count
                            Do While (index >= 0)
                                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec"))) Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                ElseIf (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) Then
                                    If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                        modVoucher.tblDetail.Item(index).Delete()
                                    End If
                                    If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                                        modVoucher.tblDetail.Item(index).Delete()
                                    End If
                                ElseIf Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec"))) Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                End If
                                index = (index + -1)
                            Loop
                        End If
                        Dim tbl As New DataTable
                        tbl = Copy2Table(Me.tblRetrieveDetail)
                        Dim num6 As Integer = (tbl.Rows.Count - 1)
                        index = 0
                        Do While (index <= num6)
                            If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                tbl.Rows.Item(index).Item("stt_rec") = ""
                            Else
                                tbl.Rows.Item(index).Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                            End If
                            tbl.Rows.Item(index).AcceptChanges()
                            index += 1
                        Loop
                        AppendFrom(modVoucher.tblDetail, tbl)
                        count = modVoucher.tblDetail.Count
                        If flag Then
                            index = (count - 1)
                            Do While (index >= 0)
                                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("ma_vt")), "C") Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_dh")), "C") Then
                                    modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                                End If
                                index = (index + -1)
                            Loop
                            Me.UpdateList()
                        End If
                        frmAdd.Dispose()
                    End If
                    ds = Nothing
                    Me.tblRetrieveMaster = Nothing
                    Me.tblRetrieveDetail = Nothing
                End If
            End If
        End Sub

        Public Sub Save()
            Try
                Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            If Not Me.oSecurity.GetActionRight Then
                oVoucher.isContinue = False
                Return
            End If
            If Not Me.grdHeader.CheckEmpty(RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("035"))) Then
                oVoucher.isContinue = False
                Return
            End If
            Dim str3 As String = StringType.FromObject(modVoucher.oLan.Item("065"))
            If (((StringType.StrCmp(Me.txtNgay_kh1.Text.Trim, "/  /", False) <> 0) And (StringType.StrCmp(Me.txtNgay_kh2.Text.Trim, "/  /", False) <> 0)) And (DateTime.Compare(Me.txtNgay_kh1.Value, Me.txtNgay_kh2.Value) > 0)) Then
                Msg.Alert(Strings.Replace(Strings.Replace(str3, "%s1", Me.lblNgay_kh1.Text.Trim, 1, -1, 0), "%s2", Me.lblNgay_kh2.Text.Trim, 1, -1, 0), 2)
                Me.txtNgay_kh1.Focus()
                oVoucher.isContinue = False
                Return
            End If
            If BooleanType.FromObject(ObjectType.BitAndObj(ObjectType.BitAndObj((ObjectType.ObjTst(Me.txtNgay_th1.Text, Fox.GetEmptyDate, False) <> 0), (ObjectType.ObjTst(Me.txtNgay_th2.Text, Fox.GetEmptyDate, False) <> 0)), (DateTime.Compare(Me.txtNgay_th1.Value, Me.txtNgay_th2.Value) > 0))) Then
                Msg.Alert(Strings.Replace(Strings.Replace(str3, "%s1", Me.lblNgay_th1.Text.Trim, 1, -1, 0), "%s2", Me.lblNgay_th2.Text.Trim, 1, -1, 0), 2)
                Me.txtNgay_th1.Focus()
                oVoucher.isContinue = False
                Return
            End If
            Dim num As Integer = 0, i As Integer = 0
            num = (modVoucher.tblDetail.Count - 1)
            Do While (num >= 0)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ma_vt"))) Then
                    If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("ma_vt"))), "", False) = 0) Then
                        modVoucher.tblDetail.Item(num).Delete()
                    End If
                Else
                    modVoucher.tblDetail.Item(num).Delete()
                End If
                num = (num + -1)
            Loop
            If (modVoucher.tblDetail.Count = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("022")), 2)
                oVoucher.isContinue = False
                Return
            End If
            num = (modVoucher.tblDetail.Count - 1)
            i = 0
            Dim s As String
            Do While (i <= num)
                If tblDetail.Item(i).Item("so_luong") = 0 Then
                    Msg.Alert("Chưa nhập số lượng sản xuất", 2)
                    oVoucher.isContinue = False
                    Return
                End If
                s = "select a.ma_lo from ctsx a join phsx b on a.stt_rec=b.stt_rec where "
                s += "a.ma_vt='" + tblDetail.Item(i).Item("ma_vt").ToString.Trim + "'"
                s += " and a.ma_lo='" + tblDetail.Item(i).Item("ma_lo").ToString.Trim + "'"
                s += " and b.status <>'*'"
                If oVoucher.cAction = "Edit" Then
                    s += " and b.stt_rec <>'" + tblDetail.Item(i).Item("stt_rec") + "'"
                End If
                If Not Sql.GetValue(appConn, s) Is Nothing Then
                    Msg.Alert("Lô này đã tạo lệnh rồi", 2)
                    oVoucher.isContinue = False
                    Return
                End If
                i += 1
            Loop

            Me.txtStatus.Text = Strings.Trim(StringType.FromObject(Me.tblHandling.Rows.Item(Me.cboAction.SelectedIndex).Item("action_id")))
            Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
            Me.txtNgay_ct.Value = Me.txtNgay_lct.Value
            Dim str As String
            Dim num2 As Integer

            Dim cString As String = StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldchar", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
            Dim num10 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num10)
                Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                num2 = 1
                Do While (num2 <= num9)
                    str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str))) Then
                        modVoucher.tblDetail.Item(num).Item(str) = ""
                    End If
                    num2 += 1
                Loop
                num += 1
            Loop
            cString = StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
            Dim num8 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num8)
                Dim num7 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                num2 = 1
                Do While (num2 <= num7)
                    str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str))) Then
                        modVoucher.tblDetail.Item(num).Item(str) = 0
                    End If
                    num2 += 1
                Loop
                num += 1
            Loop
            If (StringType.StrCmp(Me.txtStatus.Text, "0", False) <> 0) Then
                Dim strFieldList As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldcheck", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
                If (StringType.StrCmp(Strings.Trim(strFieldList), "", False) <> 0) Then
                    num = (modVoucher.tblDetail.Count - 1)
                    Dim cMap As String = clsfields.CheckEmptyFieldList("stt_rec", strFieldList, modVoucher.tblDetail)
                    Try
                        If (StringType.StrCmp(cMap, "", False) <> 0) Then
                            Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("044")), "%s", GetColumn(Me.grdDetail, cMap).HeaderText, 1, -1, 0), 2)
                            oVoucher.isContinue = False
                            Return
                        End If
                    Catch exception2 As Exception
                        ProjectData.SetProjectError(exception2)
                        Dim exception As Exception = exception2
                        ProjectData.ClearProjectError()
                    End Try
                End If
                If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                    Me.cIDNumber = ""
                Else
                    Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                End If
                If Not oVoucher.CheckDuplVoucherNumber(Fox.PadL(Strings.Trim(Me.txtSo_ct.Text), Me.txtSo_ct.MaxLength), StringType.FromObject(Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "New", Me.cIDNumber))) Then
                    Me.txtSo_ct.Focus()
                    oVoucher.isContinue = False
                    Return
                End If
            End If
            If Not Me.xInventory.isValid Then
                oVoucher.isContinue = False
                Return
            End If
            Dim strSQL As String = ""
            Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
            Me.UpdateMO()
            Me.UpdateList()
            If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                Me.cIDNumber = oVoucher.GetIdentityNumber
                modVoucher.tblMaster.AddNew()
                Me.iMasterRow = (modVoucher.tblMaster.Count - 1)
                modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec") = Me.cIDNumber
                modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_ct") = modVoucher.VoucherCode
            Else
                Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                strSQL += Chr(13) + Me.BeforUpdateMO(Me.cIDNumber, "Edit")
            End If
            DirLib.SetDatetime(modVoucher.appConn, modVoucher.tblMaster.Item(Me.iMasterRow), oVoucher.cAction)
            Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
            Me.grdHeader.Gather()
            GatherMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
            modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct"))), Me.txtSo_ct.MaxLength)
            If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                strSQL += Chr(13) + GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row)
            Else
                Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                strSQL += Chr(13) + GenSQLUpdate((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row, cKey) & ChrW(13) & GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)
            End If
            cString = "ma_ct, ngay_ct, so_ct, stt_rec,ma_bp"
            Dim str6 As String = ("stt_rec = '" & Me.cIDNumber & "' or stt_rec = '' or stt_rec is null")
            modVoucher.tblDetail.RowFilter = str6
            num = (modVoucher.tblDetail.Count - 1)
            Dim num4 As Integer = 0
            Dim num6 As Integer = num
            num = 0
            Do While (num <= num6)
                If (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("stt_rec"), Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "", RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))), False) = 0) Then
                    Dim num5 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    num2 = 1
                    Do While (num2 <= num5)
                        str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                        modVoucher.tblDetail.Item(num).Item(str) = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item(str))
                        num2 += 1
                    Loop
                    num4 += 1
                    modVoucher.tblDetail.Item(num).Item("line_nbr") = num4
                    Me.grdDetail.Update()
                    strSQL += Chr(13) + GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), modVoucher.tblDetail.Item(num).Row)
                End If
                num += 1
            Loop
            oVoucher.IncreaseVoucherNo(Strings.Trim(Me.txtSo_ct.Text))
            Me.EDTBColumns(False)
            strSQL += Chr(13) + Me.Post
            strSQL += Chr(13) + Me.grdHeader.SQLUpdateFreeField(modVoucher.appConn, Conversions.ToString(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")))
            strSQL += Chr(13) + Me.AfterUpdateMO(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "Save")
            Try
                Sql.SQLCompressExecute((modVoucher.appConn), strSQL)
            Catch ex As Exception
                Msg.Alert("Error (Không lưu được phiếu)!" + Chr(13) + ex.ToString())
                oVoucher.isContinue = False
                Return
            End Try
            Me.pnContent.Text = ""
            SaveLocalDataView(modVoucher.tblDetail)
            oVoucher.RefreshStatus(Me.cboStatus)
        End Sub

        Public Sub Search()
            Dim frm As New frmSearch
            frm.ShowDialog()
        End Sub

        Private Sub SetEmptyColKey(ByVal sender As Object, ByVal e As EventArgs)
            If Not Me.oInvItemDetail.Cancel Then
                Me.iOldRow = Me.grdDetail.CurrentRowIndex
                Me.cOldItem = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
                Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
                If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) And Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec")))) Then
                    modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("stt_rec") = ""
                    Me.WhenAddNewItem()
                    oVoucher.CarryOn(modVoucher.tblDetail, currentRowIndex)
                End If
                If ((StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) And Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec")))) Then
                    modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                    Me.WhenAddNewItem()
                    oVoucher.CarryOn(modVoucher.tblDetail, currentRowIndex)
                End If
            End If
        End Sub

        Private Sub ShowTabDetail()
            Me.tbDetail.SelectedIndex = 0
        End Sub

        Private Sub tbDetail_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles tbDetail.Enter
            Me.grdDetail.Focus()
        End Sub

        Private Sub TransTypeLostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtMa_gd.Leave
            Me.EDTranType()
        End Sub

        Private Sub txt_Enter(ByVal sender As Object, ByVal e As EventArgs)
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"))) Then
                LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
            Else
                Dim str As String = Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")))
                LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(StringType.StrCmp(str, "", False) = 0)}, Nothing)
            End If
        End Sub

        Private Sub txtDien_giai_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles txtDien_giai.Leave
        End Sub

        Private Sub txtKeyPress_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtKeyPress.Enter
            Me.grdDetail.Focus()
            Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
        End Sub

        Private Sub txtMa_gd_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtMa_gd.Enter
        End Sub

        Private Sub txtMa_gd_Valid(ByVal sender As Object, ByVal e As EventArgs)
            If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
                Me.EDTrans()
            End If
        End Sub



        Private Sub txtNumber_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtSo_ct.Enter
            LateBinding.LateSet(sender, Nothing, "Text", New Object() {Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))}, Nothing)
        End Sub

        Private Sub txtSo_luong_enter(ByVal sender As Object, ByVal e As EventArgs)
            Me.noldSo_luong = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, 0)))
        End Sub

        Private Sub txtSo_luong_valid(ByVal sender As Object, ByVal e As EventArgs)
            Dim num2 As Decimal = Me.noldSo_luong
            Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, 0)))
            If (Decimal.Compare(num, num2) <> 0) Then
                modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("so_luong") = num
                Me.grdDetail.Refresh()
                Me.UpdateList()
            End If
        End Sub

        Private Sub txtTy_gia_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtTy_gia.Enter
            oVoucher.noldFCrate = New Decimal(Me.txtTy_gia.Value)
        End Sub

        Private Sub txtTy_gia_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtTy_gia.Validated
            Me.vFCRate()
        End Sub

        Public Sub UpdateList()
            Dim zero As Decimal = Decimal.Zero
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit", "View"}) Then
                Dim num3 As Integer = (modVoucher.tblDetail.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num3)
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("so_luong"))) Then
                        zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblDetail.Item(i).Item("so_luong")))
                    End If
                    i += 1
                Loop
            End If
            Me.txtT_so_luong.Value = Convert.ToDouble(zero)
        End Sub

        Private Sub UpdateMO()
        End Sub

        Public Sub vCaptionRefresh()
            Me.EDFC()
            Dim cAction As String = oVoucher.cAction
            If ((StringType.StrCmp(cAction, "Edit", False) = 0) OrElse (StringType.StrCmp(cAction, "View", False) = 0)) Then
                Me.pnContent.Text = ""
            Else
                Me.pnContent.Text = ""
            End If
        End Sub

        Public Sub vFCRate()
            If (Me.txtTy_gia.Value <> Convert.ToDouble(oVoucher.noldFCrate)) Then
                Dim tblDetail As DataView = modVoucher.tblDetail
                Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    i += 1
                Loop
                tblDetail = Nothing
            End If
        End Sub

        Public Sub View()
            Dim num3 As Decimal
            Dim frmAdd As New Form
            Dim gridformtran2 As New gridformtran
            Dim gridformtran As New gridformtran
            Dim tbs As New DataGridTableStyle
            Dim style As New DataGridTableStyle
            Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H1F - 1) {}
            Dim index As Integer = 0
            Do
                cols(index) = New DataGridTextBoxColumn
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index <= &H1D)
            frmAdd.Top = 0
            frmAdd.Left = 0
            frmAdd.Width = Me.Width
            frmAdd.Height = Me.Height
            frmAdd.Text = Me.Text
            frmAdd.StartPosition = FormStartPosition.CenterParent
            Dim panel As StatusBarPanel = AddStb(frmAdd)
            gridformtran2.CaptionVisible = False
            gridformtran2.ReadOnly = True
            gridformtran2.Top = 0
            gridformtran2.Left = 0
            gridformtran2.Height = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
            gridformtran2.Width = (Me.Width - 5)
            gridformtran2.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            gridformtran2.BackgroundColor = Color.White
            gridformtran.CaptionVisible = False
            gridformtran.ReadOnly = True
            gridformtran.Top = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
            gridformtran.Left = 0
            gridformtran.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 30))))
            gridformtran.Width = (Me.Width - 5)
            gridformtran.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
            gridformtran.BackgroundColor = Color.White
            Dim button As New Button
            button.Visible = True
            button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
            button.Left = (-100 - button.Width)
            frmAdd.Controls.Add(button)
            frmAdd.CancelButton = button
            frmAdd.Controls.Add(gridformtran2)
            frmAdd.Controls.Add(gridformtran)
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblMaster), gridformtran2, (tbs), (cols), "MOMaster")
            index = 0
            Do
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index <= &H1D)
            cols(2).Alignment = HorizontalAlignment.Right
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), gridformtran, (style), (cols), "MODetail")
            index = 0
            Do
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index <= &H1D)
            oVoucher.HideFields(gridformtran)
            Dim str As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
            Dim count As Integer = modVoucher.tblMaster.Count
            Dim zero As Decimal = Decimal.Zero
            Dim num5 As Integer = (count - 1)
            index = 0
            Do While (index <= num5)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(index).Item("t_tien"))) Then
                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblMaster.Item(index).Item("t_tien")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(index).Item("t_tien_nt"))) Then
                    num3 = DecimalType.FromObject(ObjectType.AddObj(num3, modVoucher.tblMaster.Item(index).Item("t_tien_nt")))
                End If
                index += 1
            Loop
            str = Strings.Replace(str, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, 0)
            If (0 <> 0) Then
                str = Strings.Replace(Strings.Replace(str, "%n2", Strings.Trim(Strings.Format(num3, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))), 1, -1, 0), "%n3", Strings.Trim(Strings.Format(zero, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))), 1, -1, 0)
            Else
                str = Strings.Replace(Strings.Replace(str, "%n2", "X", 1, -1, 0), "%n3", "X", 1, -1, 0)
            End If
            panel.Text = str
            AddHandler gridformtran2.CurrentCellChanged, New EventHandler(AddressOf Me.grdMVCurrentCellChanged)
            gridformtran2.CurrentRowIndex = Me.iMasterRow
            Obj.Init(frmAdd)
            Dim collection2 As New Collection
            collection2.Add(Me, "Form", Nothing, Nothing)
            collection2.Add(gridformtran2, "grdHeader", Nothing, Nothing)
            collection2.Add(gridformtran, "grdDetail", Nothing, Nothing)
            Me.oSecurity.aVGrid = collection2
            Me.oSecurity.InnitView()
            Me.oSecurity.InvisibleView()
            frmAdd.ShowDialog()
            frmAdd.Dispose()
            Me.iMasterRow = gridformtran2.CurrentRowIndex
            Me.RefrehForm()
        End Sub

        Public Sub vTextRefresh()
        End Sub

        Private Sub WhenAddNewItem()
            modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
        End Sub

        Private Sub WhenItemLeave(ByVal sender As Object, ByVal e As EventArgs)
            On Error Resume Next
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If (Me.iOldRow <> currentRowIndex) Then
                Return
            End If
            If Me.oInvItemDetail.Cancel Then
                Return
            End If
            Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
            If (StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.cOldItem), False) = 0) Then
                Return
            End If
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_vt")), "C") Then
                    Return
                End If
                Dim str3 As String = Strings.Trim(StringType.FromObject(.Item("ma_vt")))
                Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmvt", ("ma_vt = '" & str3 & "'")), DataRow)
                .Item("volume") = RuntimeHelpers.GetObjectValue(row.Item("volume"))
                .Item("weight") = RuntimeHelpers.GetObjectValue(row.Item("weight"))
                If BooleanType.FromObject(ObjectType.NotObj(row.Item("sua_tk_vt"))) Then
                    .Item("tk_vt") = RuntimeHelpers.GetObjectValue(row.Item("tk_vt"))
                Else
                    If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("tk_vt")), "C") Then
                        .Item("tk_vt") = RuntimeHelpers.GetObjectValue(row.Item("tk_vt"))
                    End If
                End If
                Dim cString As String = "tk_gv, tk_dt, tk_ck"
                Dim num6 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                Dim nWordPosition As Integer = 1
                For nWordPosition = 1 To num6
                    Dim str2 As String = Strings.Trim(Fox.GetWordNum(cString, nWordPosition, ","c))
                    If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item(str2)), "C") Then
                        .Item(str2) = RuntimeHelpers.GetObjectValue(row.Item(str2))
                    Else
                        If (ObjectType.ObjTst(Sql.GetValue((modVoucher.appConn), "dmtk", "loai_tk", ("tk = '" & Strings.Trim(StringType.FromObject(row.Item(str2))) & "'")), 1, False) = 0) Then
                            .Item(str2) = RuntimeHelpers.GetObjectValue(row.Item(str2))
                        End If
                    End If
                Next
                .Item("dvt") = RuntimeHelpers.GetObjectValue(row.Item("dvt"))
                Me.colDvt.TextBox.Text = StringType.FromObject(.Item("dvt"))
                .Item("he_so") = 1
                If BooleanType.FromObject(row.Item("nhieu_dvt")) Then
                    Me.oUOM.Empty = False
                    Me.colDvt.ReadOnly = False
                    Me.oUOM.Cancel = False
                    Me.oUOM.Check = True
                Else
                    Me.oUOM.Empty = True
                    Me.colDvt.ReadOnly = True
                    Me.oUOM.Cancel = True
                    Me.oUOM.Check = False
                End If
                If BooleanType.FromObject(ObjectType.NotObj(row.Item("lo_yn"))) Then
                    .Item("ma_lo") = ""
                Else
                    If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_lo")), "C") Then
                        Dim str5 As String = StringType.FromObject(Sql.GetValue(modVoucher.appConn, ("spGetLotNumber4MO '" & Strings.Trim(str3) & "'")))
                        .Item("ma_lo") = str5
                    End If
                End If
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_kho")), "C") Then
                    .Item("ma_kho") = Sql.GetValue(appConn, "select min(ma_kho) from dmkho where dbo.ff_inlist(ma_kho,'" + RuntimeHelpers.GetObjectValue(row.Item("ma_kho")) + "')=1 and ma_dvcs='" + Me.txtMa_dvcs.Text + "'")
                End If
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_vi_tri")), "C") Then
                    .Item("ma_vi_tri") = RuntimeHelpers.GetObjectValue(row.Item("ma_vi_tri"))
                End If
            End With
        End Sub

        Private Sub WhenLocationEnter(ByVal sender As Object, ByVal e As EventArgs)
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_kho")), "C") Then
                Dim cKey As String = ("ma_kho = '" & Strings.Trim(StringType.FromObject(view.Item("ma_kho"))) & "'")
                Me.oLocation.Key = cKey
                Me.oLocation.Empty = (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvitri", "ma_vi_tri", cKey))), "", False) = 0)
            End If
            view = Nothing
        End Sub

        Private Sub WhenLotEnter(ByVal sender As Object, ByVal e As EventArgs)
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
                Dim cKey As String = ("ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "'")
                cKey += " AND ma_lo not in (select distinct a.ma_lo from ctsx a join phsx b on a.stt_rec=b.stt_rec where a.ma_vt='" + Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) + "' and b.status<>'*' and b.stt_rec<>'" + view.Item("stt_rec").ToString.Trim + "')"
                Me.oLot.Key = cKey
                Me.oLot.Empty = (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmlo", "ma_lo", cKey))), "", False) = 0)
            End If
            view = Nothing
        End Sub
        Private Sub WhenSODetail4MOEnter(ByVal sender As Object, ByVal e As EventArgs)
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
                Dim cKey As String = ("ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "'")
                Me.oSODetail4MO.Key = cKey
                Me.oSODetail4MO.Empty = (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "vSODetail4MO", "code", cKey))), "", False) = 0)
            End If
            view = Nothing
        End Sub


        Private Sub WhenSiteEnter(ByVal sender As Object, ByVal e As EventArgs)
            Me.cOldSite = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        End Sub

        Private Sub WhenSiteLeave(ByVal sender As Object, ByVal e As EventArgs)
            If (Me.grdDetail.CurrentRowIndex >= 0) Then
                Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
                With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                    If Not ((StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.cOldSite), False) = 0) And Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ten_kho")), "C")) Then
                        If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkho", "dai_ly_yn", ("ma_kho = '" & str & "'"))) Then
                            Dim str3 As String = Strings.Trim(StringType.FromObject(.Item("ma_vt")))
                            Dim str2 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "tk_dl", ("ma_vt = '" & str3 & "'"))))
                            If (StringType.StrCmp(str2, "", False) <> 0) Then
                                .Item("tk_vt") = str2
                            End If
                        End If
                    End If
                End With
            End If
        End Sub

        Private Sub WhenUOMEnter(ByVal sender As Object, ByVal e As EventArgs)
            On Error Resume Next
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
                Return
            End If
            If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "'"))) Then
                Dim str As String = ("(ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "' OR ma_vt = '*')")
                Me.oUOM.Key = str
                Me.oUOM.Empty = False
                Me.colDvt.ReadOnly = False
                Me.oUOM.Cancel = False
                Me.oUOM.Check = True
            Else
                Me.oUOM.Key = "1=1"
                Me.oUOM.Empty = True
                Me.colDvt.ReadOnly = True
                Me.oUOM.Cancel = True
                Me.oUOM.Check = False
            End If
            view = Nothing
        End Sub

        Private Sub WhenUOMLeave(ByVal sender As Object, ByVal e As EventArgs)
            On Error Resume Next
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_vt")), "C") Then
                    Return
                End If
                If Not BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(StringType.FromObject(.Item("ma_vt"))) & "'"))) Then
                    Return
                End If
                Dim cKey As String = String.Concat(New String() {"(ma_vt = '", Strings.Trim(StringType.FromObject(.Item("ma_vt"))), "' OR ma_vt = '*') AND dvt = N'", Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))), "'"})
                Dim num As Decimal = DecimalType.FromObject(Sql.GetValue((modVoucher.appConn), "dmqddvt", "he_so", cKey))
                .Item("He_so") = num
            End With
        End Sub


        ' Properties
        Friend WithEvents cboAction As ComboBox
        Friend WithEvents cboStatus As ComboBox
        Friend WithEvents cmdBottom As Button
        Friend WithEvents cmdClose As Button
        Friend WithEvents cmdDelete As Button
        Friend WithEvents cmdEdit As Button
        Friend WithEvents cmdMa_nt As Button
        Friend WithEvents cmdNew As Button
        Friend WithEvents cmdNext As Button
        Friend WithEvents cmdOption As Button
        Friend WithEvents cmdPrev As Button
        Friend WithEvents cmdPrint As Button
        Friend WithEvents cmdSave As Button
        Friend WithEvents cmdSearch As Button
        Friend WithEvents cmdTop As Button
        Friend WithEvents cmdView As Button
        Friend WithEvents grdDetail As clsgrid
        Friend WithEvents Label1 As Label
        Friend WithEvents lblAction As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMa_gd As Label
        Friend WithEvents lblMa_md As Label
        Friend WithEvents lblNgay_ct As Label
        Friend WithEvents lblNgay_kh1 As Label
        Friend WithEvents lblNgay_kh2 As Label
        Friend WithEvents lblNgay_lct As Label
        Friend WithEvents lblNgay_th1 As Label
        Friend WithEvents lblNgay_th2 As Label
        Friend WithEvents lblSo_ct As Label
        Friend WithEvents lblStatus As Label
        Friend WithEvents lblStatusMess As Label
        Friend WithEvents lblTen As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTen_gd As Label
        Friend WithEvents lblTen_md As Label
        Friend WithEvents lblTotal As Label
        Friend WithEvents lblTy_gia As Label
        Friend WithEvents tbDetail As TabControl
        Friend WithEvents tpgDetail As TabPage
        Friend WithEvents txtDien_giai As TextBox
        Friend WithEvents txtKeyPress As TextBox
        Friend WithEvents txtLoai_ct As TextBox
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtMa_gd As TextBox
        Friend WithEvents txtMa_md As TextBox
        Friend WithEvents txtNgay_ct As txtDate
        Friend WithEvents txtNgay_kh1 As txtDate
        Friend WithEvents txtNgay_kh2 As txtDate
        Friend WithEvents txtNgay_lct As txtDate
        Friend WithEvents txtNgay_th1 As txtDate
        Friend WithEvents txtNgay_th2 As txtDate
        Friend WithEvents txtSo_ct As TextBox
        Friend WithEvents txtStatus As TextBox
        Friend WithEvents txtT_so_luong As txtNumeric
        Friend WithEvents txtTy_gia As txtNumeric

        Public arrControlButtons As Button()
        Public cIDNumber As String
        Public cOldIDNumber As String
        Private cOldItem As String
        Private cOldSite As String
        Private colDvt As DataGridTextBoxColumn
        Private colMa_kho As DataGridTextBoxColumn
        Private colMa_lo As DataGridTextBoxColumn
        Private colMa_vi_tri As DataGridTextBoxColumn
        Private colMa_vt As DataGridTextBoxColumn
        Private colSo_dh As DataGridTextBoxColumn
        Private colSo_line As DataGridTextBoxColumn
        Private colSo_luong As DataGridTextBoxColumn
        Private colTen_vt As DataGridTextBoxColumn
        Private components As IContainer
        Private grdHeader As grdHeader
        Public iDetailRow As Integer
        Public iMasterRow As Integer
        Public iOldMasterRow As Integer
        Private iOldRow As Integer
        Private isActive As Boolean
        Private lAllowCurrentCellChanged As Boolean
        Private nColumnControl As Integer
        Private noldSo_luong As Decimal
        Private oInvItemDetail As VoucherLibObj
        Private oldtblDetail As DataTable
        Private oLocation As VoucherKeyLibObj
        Private oLot, oSODetail4MO As VoucherKeyLibObj
        Private oSecurity As clssecurity
        Private oSite As VoucherKeyLibObj
        'Private oTitleButton As TitleButton
        Private oUOM As VoucherKeyCheckLibObj
        Public oVoucher As clsvoucher.clsVoucher
        Public pnContent As StatusBarPanel
        Private tblHandling As DataTable
        Private tblRetrieveDetail As DataView
        Private tblRetrieveMaster As DataView
        Private tblStatus As DataTable
        Private xInventory As clsInventory
    End Class
End Namespace

