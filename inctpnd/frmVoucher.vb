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
Imports libscontrol
Imports libscontrol.clsvoucher.clsVoucher
Imports libscontrol.voucherseachlib

Namespace inctpnd
    Public Class frmVoucher
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Activated, New EventHandler(AddressOf Me.frmVoucher_Activated)
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmVoucher_Load)
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
            Me.txtMa_gd.Text = Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ma_gd")))
            Me.txtStatus.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_status"))
            Unit.SetUnit(Me.txtMa_dvcs)
            Me.EDFC()
            Me.cOldIDNumber = Me.cIDNumber
            Me.iOldMasterRow = Me.iMasterRow
            Me.UpdateList()
            Me.ShowTabDetail()
            If Me.txtMa_dvcs.Enabled Then
                Me.txtMa_dvcs.Focus()
            Else
                Me.txtMa_kh.Focus()
            End If
            Me.EDTBColumns()
            Me.oSecurity.SetReadOnly()
            Me.InitFlowHandling(Me.cboAction)
            Me.EDStatus()
            Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
        End Sub

        Public Sub Cancel()
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
                    Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iOldMasterRow).Row
                    Me.grdHeader.Scatter()
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
            If Not Me.oSecurity.GetStatusDelelete Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("023")), 1)
            Else
                Dim num As Integer = 0
                Dim str3 As String = ""
                Dim str4 As String = ""
                Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
                Dim lcIDNumber As String = modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")
                Dim cKey As String = "stt_rec = '" + lcIDNumber + "'"
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
                str4 = tblStock
                If modVoucher.VoucherCode = "PND" Then
                    str4 += ",ct00"
                End If
                If CInt(oVar.Item("m_pack_yn")) = 1 Then
                    str4 = oVoucherRow.Item("m_phdbf").ToString.Trim + "," + str4
                    str3 = GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)
                End If

                Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(str4, ","c))
                num = 1
                Do While (num <= num3)
                    Dim cTable As String = Strings.Trim(Fox.GetWordNum(str4, num, ","c))
                    str3 = (str3 & ChrW(13) & GenSQLDelete(cTable, cKey))
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
                If (ObjectType.ObjTst(modVoucher.oVar.Item("m_pack_yn"), 0, False) = 0) Then
                    str3 = ((String.Concat(New String() {str3, ChrW(13), "UPDATE ", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), " SET Status = '*'"}) & ", datetime2 = GETDATE(), user_id2 = " & StringType.FromObject(Reg.GetRegistryKey("CurrUserId"))) & "  WHERE " & cKey)
                End If
                Me.BeforUpdateIR(lcIDNumber, "Del")
                Sql.SQLExecute((modVoucher.appConn), str3)
                Me.pnContent.Text = ""
            End If
        End Sub

        Private Sub DeleteItem(ByVal sender As Object, ByVal e As EventArgs)
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
                Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
                If ((((currentRowIndex >= 0) And (currentRowIndex < modVoucher.tblDetail.Count)) AndAlso Not Me.grdDetail.EndEdit(Me.grdDetail.TableStyles.Item(0).GridColumnStyles.Item(Me.grdDetail.CurrentCell.ColumnNumber), currentRowIndex, False)) AndAlso (ObjectType.ObjTst(Msg.Question(StringType.FromObject(modVoucher.oVar.Item("m_sure_dele")), 1), 1, False) = 0)) Then
                    Me.grdDetail.Select(currentRowIndex)
                    Dim view As DataRowView = modVoucher.tblDetail.Item(currentRowIndex)
                    AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), False)
                    view.Delete()
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
                ChangeFormatColumn(Me.colTien_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
                ChangeFormatColumn(Me.colGia_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia")))
                Me.colTien_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("018"))
                Me.colGia_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("024"))
                Me.txtT_tien_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
                Try
                    Me.colTien.MappingName = "H1"
                    Me.colGia.MappingName = "H2"
                Catch exception1 As Exception
                    ProjectData.SetProjectError(exception1)
                    ProjectData.ClearProjectError()
                End Try
                Me.txtT_tien.Visible = False
            Else
                Me.txtTy_gia.Enabled = True
                ChangeFormatColumn(Me.colTien_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
                ChangeFormatColumn(Me.colGia_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
                Me.colTien_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("019")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
                Me.colGia_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("025")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
                Me.txtT_tien_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
                Me.txtT_tien_nt.Value = Me.txtT_tien_nt.Value
                Try
                    Me.colTien.MappingName = "tien"
                    Me.colGia.MappingName = "gia"
                Catch exception2 As Exception
                    ProjectData.SetProjectError(exception2)
                    ProjectData.ClearProjectError()
                End Try
                Me.txtT_tien.Visible = True
            End If
            Me.EDStatus()
            Me.oSecurity.Invisible()
        End Sub

        Public Sub Edit()
            Me.oldtblDetail = Copy2Table(modVoucher.tblDetail)
            Me.iOldMasterRow = Me.iMasterRow
            oVoucher.rOldMaster = modVoucher.tblMaster.Item(Me.iMasterRow)
            Me.ShowTabDetail()
            If Me.txtMa_dvcs.Enabled Then
                Me.txtMa_dvcs.Focus()
            Else
                Me.txtMa_kh.Focus()
            End If
            Me.EDTBColumns()
            Me.oSecurity.SetReadOnly()
            If Not Me.oSecurity.GetStatusEdit Then
                Me.cmdSave.Enabled = False
            End If
            Me.InitFlowHandling(Me.cboAction)
            Me.EDStatus()
            Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
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
            Try
                oVoucher.RefreshHandling(Me.cboAction)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                ProjectData.ClearProjectError()
            End Try
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
            Loop While (index <= MaxColumns - 1)
            Try
                GetColumn(Me.grdDetail, "ten_vt").TextBox.Enabled = False
                'GetColumn(Me.grdDetail, "ma_lo0").TextBox.Enabled = False
                'GetColumn(Me.grdDetail, "ngay_hhsd").TextBox.Enabled = False
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Try
                GetColumn(Me.grdDetail, "so_dh").TextBox.Enabled = False
                GetColumn(Me.grdDetail, "po_line").TextBox.Enabled = False
            Catch ex As Exception
            End Try
        End Sub

        Private Sub EDTBColumns(ByVal lED As Boolean)
            Dim index As Integer = 0
            Do
                modVoucher.tbcDetail(index).TextBox.Enabled = lED
                index += 1
            Loop While (index <= MaxColumns - 1)
            Try
                GetColumn(Me.grdDetail, "ten_vt").TextBox.Enabled = False
                'GetColumn(Me.grdDetail, "ma_lo0").TextBox.Enabled = False
                'GetColumn(Me.grdDetail, "ngay_hhsd").TextBox.Enabled = False
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Try
                GetColumn(Me.grdDetail, "so_dh").TextBox.Enabled = False
                GetColumn(Me.grdDetail, "po_line").TextBox.Enabled = False
            Catch ex As Exception
            End Try
            Me.EDStatus(lED)
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
            Me.txtNgay_ct.AddCalenderControl()
            Me.txtNgay_lct.AddCalenderControl()
            Dim lib4 As New DirLib(Me.txtMa_dvcs, Me.lblTen_dvcs, modVoucher.sysConn, modVoucher.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", False, Me.cmdEdit)
            Dim lib2 As New CharLib(Me.txtStatus, "0, 1")
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
            modVoucher.alOther = "ctgt30tmp"
            Dim cFile As String = ("Structure\Voucher\" & modVoucher.VoucherCode)
            If Not Sys.XML2DataSet((modVoucher.dsMain), cFile) Then
                Dim tcSQL As String = ("SELECT * FROM " & modVoucher.alMaster)
                Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alMaster, (modVoucher.dsMain))
                tcSQL = ("SELECT * FROM " & modVoucher.alDetail)
                Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alDetail, (modVoucher.dsMain))
                tcSQL = ("SELECT * FROM " & modVoucher.alOther)
                Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alOther, (modVoucher.dsMain))
                Sys.DataSet2XML(modVoucher.dsMain, cFile)
            End If
            modVoucher.tblMaster.Table = modVoucher.dsMain.Tables.Item(modVoucher.alMaster)
            modVoucher.tblDetail.Table = modVoucher.dsMain.Tables.Item(modVoucher.alDetail)
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), Me.grdDetail, (modVoucher.tbsDetail), (modVoucher.tbcDetail), "IRDetail")
            oVoucher.SetMaxlengthItem(Me.grdDetail, modVoucher.alDetail, modVoucher.sysConn)
            Me.grdDetail.dvGrid = modVoucher.tblDetail
            Me.grdDetail.cFieldKey = "Ma_vt"
            Me.grdDetail.AllowSorting = False
            Me.grdDetail.TableStyles.Item(0).AllowSorting = False
            modVoucher.tblDetail.Table.Columns.Item("pn_gia_tb").DefaultValue = False
            Me.colMa_vt = GetColumn(Me.grdDetail, "ma_vt")
            Me.colDvt = GetColumn(Me.grdDetail, "Dvt")
            Me.colSo_lsx = GetColumn(Me.grdDetail, "So_lsx")
            Me.colMa_kho = GetColumn(Me.grdDetail, "ma_kho")
            Me.colMa_vi_tri = GetColumn(Me.grdDetail, "ma_vi_tri")
            Me.colMa_lo = GetColumn(Me.grdDetail, "ma_lo")
            Me.colMa_nx = GetColumn(Me.grdDetail, "ma_nx")
            Me.colTk_vt = GetColumn(Me.grdDetail, "tk_vt")
            Me.colTk_du = GetColumn(Me.grdDetail, "tk_du")
            Me.colSo_luong = GetColumn(Me.grdDetail, "so_luong")
            Me.colGia = GetColumn(Me.grdDetail, "gia")
            Me.colGia_nt = GetColumn(Me.grdDetail, "gia_nt")
            Me.colTien = GetColumn(Me.grdDetail, "tien")
            Me.colTien_nt = GetColumn(Me.grdDetail, "tien_nt")
            Me.colMa_nsx = GetColumn(Me.grdDetail, "ma_nsx")
            Dim sKey As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keyaccount", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            Dim str2 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keycust", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            Me.oSite = New VoucherKeyLibObj(Me.colMa_kho, "ten_kho", modVoucher.sysConn, modVoucher.appConn, "dmkho", "ma_kho", "ten_kho", "Site", ("ma_dvcs = '" & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("DFUnit"))) & "'"), modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
            Dim obj4 As New VoucherLibObj(Me.colMa_nx, "ten_nx", modVoucher.sysConn, modVoucher.appConn, "dmnx", "ma_nx", "ten_nx", "Reason", "1=1", modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
            Dim obj2 As New VoucherLibObj(Me.colTk_vt, "ten_tk_vt", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", sKey, modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
            Dim obj3 As New VoucherLibObj(Me.colTk_du, "ten_tk_du", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", sKey, modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
            Me.oLocation = New VoucherKeyLibObj(Me.colMa_vi_tri, "ten_vi_tri", modVoucher.sysConn, modVoucher.appConn, "dmvitri", "ma_vi_tri", "ten_vi_tri", "Location", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            Me.oLot = New VoucherKeyLibObj(Me.colMa_lo, "ten_lo", modVoucher.sysConn, modVoucher.appConn, "dmlo", "ma_lo", "ten_lo", "Lot", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            Me.oUOM = New VoucherKeyCheckLibObj(Me.colDvt, "ten_dvt", modVoucher.sysConn, modVoucher.appConn, "vdmvtqddvt", "dvt", "ten_dvt", "UOMItem", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            Me.oUOM.Cancel = True
            Me.colDvt.TextBox.CharacterCasing = CharacterCasing.Normal
            Me.oMO = New VoucherKeyLibObj(Me.colSo_lsx, "ten_lo", modVoucher.sysConn, modVoucher.appConn, "phlsx", "so_lsx", "dien_giai", "MONumber", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            AddHandler Me.colMa_kho.TextBox.Enter, New EventHandler(AddressOf Me.WhenSiteEnter)
            AddHandler Me.colMa_kho.TextBox.Validated, New EventHandler(AddressOf Me.WhenSiteLeave)
            AddHandler Me.colMa_vi_tri.TextBox.Move, New EventHandler(AddressOf Me.WhenLocationEnter)
            AddHandler Me.colMa_lo.TextBox.Move, New EventHandler(AddressOf Me.WhenLotEnter)
            AddHandler Me.colMa_lo.TextBox.Validated, New EventHandler(AddressOf Me.WhenLotLeave)
            AddHandler Me.colDvt.TextBox.Move, New EventHandler(AddressOf Me.WhenUOMEnter)
            AddHandler Me.colDvt.TextBox.Validated, New EventHandler(AddressOf Me.WhenUOMLeave)
            AddHandler Me.colSo_lsx.TextBox.Enter, New EventHandler(AddressOf Me.WhenMOEnter)
            AddHandler Me.colMa_nx.TextBox.Enter, New EventHandler(AddressOf Me.WhenReasonEnter)
            AddHandler Me.colMa_nx.TextBox.Validated, New EventHandler(AddressOf Me.WhenReasonLeave)
            Dim monumber As New monumber(GetColumn(Me.grdDetail, "so_lsx"))
            Dim oCus As New DirLib(Me.txtMa_kh, Me.lblTen_kh, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", str2, False, Me.cmdEdit)
            Dim clscustomerref As New clscustomerref(modVoucher.appConn, Me.txtMa_kh, Me.txtOng_ba, modVoucher.VoucherCode, Me.oVoucher)
            Dim lib3 As New DirLib(Me.txtMa_gd, Me.lblTen_gd, modVoucher.sysConn, modVoucher.appConn, "dmmagd", "ma_gd", "ten_gd", "VCTransCode", ("ma_ct = '" & modVoucher.VoucherCode & "'"), False, Me.cmdEdit)
            Me.oInvItemDetail = New VoucherLibObj(Me.colMa_vt, "ten_vt", modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)

            Dim oNSX As New VoucherLibObj(Me.colMa_nsx, "ten_nsx", modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)

            VoucherLibObj.oClassMsg = oVoucher.oClassMsg
            Me.oInvItemDetail.Colkey = True
            VoucherLibObj.dvDetail = modVoucher.tblDetail
            AddHandler Me.colMa_vt.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKey)
            AddHandler Me.colMa_vt.TextBox.Validated, New EventHandler(AddressOf Me.WhenItemLeave)
            Try
                oVoucher.AddValidFields(Me.grdDetail, modVoucher.tblDetail, Me.pnContent, Me.cmdEdit)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            GetColumn(Me.grdDetail, "ten_vt").TextBox.Enabled = False
            'GetColumn(Me.grdDetail, "ma_lo0").TextBox.Enabled = False
            'GetColumn(Me.grdDetail, "ngay_hhsd").TextBox.Enabled = False
            oVoucher.HideFields(Me.grdDetail)
            ChangeFormatColumn(Me.colSo_luong, StringType.FromObject(modVoucher.oVar.Item("m_ip_sl")))
            AddHandler Me.colSo_luong.TextBox.Leave, New EventHandler(AddressOf Me.txtSo_luong_valid)
            AddHandler Me.colGia_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_nt_valid)
            AddHandler Me.colGia.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_valid)
            AddHandler Me.colTien_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtTien_nt_valid)
            AddHandler Me.colTien.TextBox.Leave, New EventHandler(AddressOf Me.txtTien_valid)
            AddHandler Me.colSo_luong.TextBox.Enter, New EventHandler(AddressOf Me.txtSo_luong_enter)
            AddHandler Me.colGia_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtGia_nt_enter)
            AddHandler Me.colGia.TextBox.Enter, New EventHandler(AddressOf Me.txtGia_enter)
            AddHandler Me.colTien_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtTien_nt_enter)
            AddHandler Me.colTien.TextBox.Enter, New EventHandler(AddressOf Me.txtTien_enter)
            AddHandler Me.colTk_vt.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneInputItemAccount)
            Dim objectValue As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldchar", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
            Dim obj7 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
            Dim obj6 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fielddate", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
            Dim index As Integer = 0
            Do
                Dim args As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj7)}
                Dim copyBack As Boolean() = New Boolean() {True}
                If copyBack(0) Then
                    obj7 = RuntimeHelpers.GetObjectValue(args(0))
                End If
                If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", args, Nothing, copyBack)), modVoucher.tbcDetail(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                    modVoucher.tbcDetail(index).NullText = "0"
                Else
                    Dim objArray2 As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj6)}
                    copyBack = New Boolean() {True}
                    If copyBack(0) Then
                        obj6 = RuntimeHelpers.GetObjectValue(objArray2(0))
                    End If
                    If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", objArray2, Nothing, copyBack)), modVoucher.tbcDetail(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                        modVoucher.tbcDetail(index).NullText = StringType.FromObject(Fox.GetEmptyDate)
                    Else
                        modVoucher.tbcDetail(index).NullText = ""
                    End If
                End If
                If (index <> 0) Then
                    If Array.Exists({"ma_lo0", "ngay_hhsd", "ma_ncc", "ma_nsx", "xuat_xu"}, Function(element)
                                                                                                Return element.Equals(tbcDetail(index).MappingName.ToLower())
                                                                                            End Function) Then
                        AddHandler modVoucher.tbcDetail(index).TextBox.Enter, New EventHandler(AddressOf Me.txt_Enter_Lo)
                    Else
                        AddHandler modVoucher.tbcDetail(index).TextBox.Enter, New EventHandler(AddressOf Me.txt_Enter)
                    End If
                    'If Fox.InList(modVoucher.tbcDetail(index).MappingName.ToLower, "ma_lo0,ngay_hhsd,ma_ncc,ma_nxs,xuat_xu") = True Then
                    '    AddHandler modVoucher.tbcDetail(index).TextBox.Enter, New EventHandler(AddressOf Me.txt_Enter_Lo)
                    'Else
                    '    AddHandler modVoucher.tbcDetail(index).TextBox.Enter, New EventHandler(AddressOf Me.txt_Enter)
                    'End If
                End If
                index += 1
            Loop While (index <= MaxColumns - 1)
            Dim menu2 As New ContextMenu
            Dim item5 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("035")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F8)
            Dim item6 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("Z08")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F6)
            menu2.MenuItems.Add(item5)
            menu2.MenuItems.Add(New MenuItem("-"))
            menu2.MenuItems.Add(item6)
            Me.ContextMenu = menu2
            Dim menu As New ContextMenu
            Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItem), Shortcut.F4)
            Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItem), Shortcut.F8)
            Dim item4 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("203")), New EventHandler(AddressOf Me.ViewItem), Shortcut.F5)
            Dim item3 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("204")), New EventHandler(AddressOf Me.LotItem), Shortcut.F9)
            menu.MenuItems.Add(item)
            menu.MenuItems.Add(item2)
            menu.MenuItems.Add(New MenuItem("-"))
            menu.MenuItems.Add(item4)
            menu.MenuItems.Add(item3)
            Me.txtKeyPress.Left = (-100 - Me.txtKeyPress.Width)
            Me.grdDetail.ContextMenu = menu
            Me.tpgOther.Visible = False
            Me.tbDetail.TabPages.Remove(Me.tpgOther)
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
            Dim aGrid As Collection = Me.oSecurity.aGrid
            aGrid.Add(Me, "Form", Nothing, Nothing)
            aGrid.Add(Me.grdHeader, "grdHeader", Nothing, Nothing)
            aGrid.Add(Me.grdDetail, "grdDetail", Nothing, Nothing)
            aGrid = Nothing
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
            If Not Me.lAllowCurrentCellChanged Then
                Return
            End If
            Dim currentRowIndex As Integer = grdDetail.CurrentRowIndex
            Dim columnNumber As Integer = grdDetail.CurrentCell.ColumnNumber
            If IsDBNull(grdDetail.Item(currentRowIndex, columnNumber)) Then
                Return
            End If
            Dim oValue As String = Strings.Trim(StringType.FromObject(grdDetail.Item(currentRowIndex, columnNumber)))
            Dim sLeft As String = grdDetail.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName.ToUpper.ToString
            Select Case sLeft
                Case "MA_KHO"
                    Me.cOldSite = StringType.FromObject(oValue)
                Case "MA_LO"
                    'cOldObject = Me.cOldSite
                    'oVoucher.SetOldValue(cOldObject, oValue)
                    Me.cOldLot = StringType.FromObject(oValue)
                Case "MA_NX"
                    Me.cOldResonCode = StringType.FromObject(oValue)
                Case "SO_LUONG"
                    Me.noldSo_luong = DecimalType.FromObject(oValue)
                Case "GIA_NT"
                    Me.noldGia_nt = DecimalType.FromObject(oValue)
                Case "GIA"
                    Me.noldGia = DecimalType.FromObject(cOldSite)
                Case "TIEN_NT"
                    Me.noldTien_nt = DecimalType.FromObject(oValue)
                Case "TIEN"
                    Me.noldTien = DecimalType.FromObject(oValue)
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
            'If VoucherCode = "PNE" Then
            '    Dim i As Integer
            '    For i = 0 To tblDetail.Count - 1
            '        Try
            '            tblDetail.Item(i).Item("ton13") = CDbl(Sql.GetValue(appConn, "select ton13 From cdlo13_dk where ma_kho=" + Sql.ConvertVS2SQLType(tblDetail.Item(i).Item("ma_kho"), "") + " AND ma_lo=" + Sql.ConvertVS2SQLType(tblDetail.Item(i).Item("ma_lo"), "") + " AND ma_vt=" + Sql.ConvertVS2SQLType(tblDetail.Item(i).Item("ma_vt"), "")))
            '        Catch ex As Exception
            '        End Try
            '    Next
            'End If
        End Sub

        Public Function InitFlowHandling(ByVal cboHandling As ComboBox) As DataTable
            Dim ds As New DataSet
            Dim num2 As Integer = 0
            cboHandling.DropDownStyle = ComboBoxStyle.DropDownList
            Dim sLeft As String = StringType.FromObject(Reg.GetRegistryKey("Language"))
            Dim strSQL As String = String.Concat(New String() {"fs_GetFlowHandling '", modVoucher.VoucherCode, "', '", Me.txtStatus.Text, "'"})
            Sys.Ds2XML(modVoucher.appConn, strSQL, "dmxlct", (ds), ("Structure\Voucher\Handle\Flow\" & modVoucher.VoucherCode & "\" & Strings.Trim(Me.txtStatus.Text)))
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
                Dim item As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(table.Rows.Item(i).Item("action_id"), ". "), Strings.Trim(StringType.FromObject(LateBinding.LateGet(table.Rows.Item(i), Nothing, "Item", New Object() {ObjectType.AddObj("action_name", Interaction.IIf((StringType.StrCmp(sLeft, "V", False) = 0), "", "2"))}, Nothing, Nothing)))))
                cboHandling.Items.Add(item)
                i += 1
            Loop
            ds = Nothing
            cboHandling.SelectedIndex = num2
            Return table
        End Function
        Friend WithEvents lblNgay_hd As Label
        Friend WithEvents txtNgay_ct0 As txtDate
        Friend WithEvents txtSo_ct0 As TextBox
        Friend WithEvents lblSo_hd As Label
        Friend WithEvents Label1 As Label
        Friend WithEvents Label2 As Label
        Friend WithEvents txtFdate1 As txtDate
        Friend WithEvents txtFnote1 As TextBox
        Friend WithEvents Label3 As Label
        Friend WithEvents txtFcode1 As TextBox

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
            Me.tpgOther = New System.Windows.Forms.TabPage()
            Me.txtT_tien = New libscontrol.txtNumeric()
            Me.txtT_tien_nt = New libscontrol.txtNumeric()
            Me.txtStatus = New System.Windows.Forms.TextBox()
            Me.lblStatus = New System.Windows.Forms.Label()
            Me.lblStatusMess = New System.Windows.Forms.Label()
            Me.txtKeyPress = New System.Windows.Forms.TextBox()
            Me.cboStatus = New System.Windows.Forms.ComboBox()
            Me.cboAction = New System.Windows.Forms.ComboBox()
            Me.lblAction = New System.Windows.Forms.Label()
            Me.lblMa_kh = New System.Windows.Forms.Label()
            Me.txtMa_kh = New System.Windows.Forms.TextBox()
            Me.lblTen_kh = New System.Windows.Forms.Label()
            Me.lblOng_ba = New System.Windows.Forms.Label()
            Me.txtOng_ba = New System.Windows.Forms.TextBox()
            Me.lblMa_gd = New System.Windows.Forms.Label()
            Me.txtMa_gd = New System.Windows.Forms.TextBox()
            Me.lblTen_gd = New System.Windows.Forms.Label()
            Me.lblTien_hang = New System.Windows.Forms.Label()
            Me.lblTen = New System.Windows.Forms.Label()
            Me.txtDien_giai = New System.Windows.Forms.TextBox()
            Me.lblDien_giai = New System.Windows.Forms.Label()
            Me.txtT_so_luong = New libscontrol.txtNumeric()
            Me.txtLoai_ct = New System.Windows.Forms.TextBox()
            Me.lblNgay_hd = New System.Windows.Forms.Label()
            Me.txtNgay_ct0 = New libscontrol.txtDate()
            Me.txtSo_ct0 = New System.Windows.Forms.TextBox()
            Me.lblSo_hd = New System.Windows.Forms.Label()
            Me.txtFcode1 = New System.Windows.Forms.TextBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.txtFdate1 = New libscontrol.txtDate()
            Me.txtFnote1 = New System.Windows.Forms.TextBox()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.tbDetail.SuspendLayout()
            Me.tpgDetail.SuspendLayout()
            CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'cmdSave
            '
            Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
            Me.cmdSave.Location = New System.Drawing.Point(2, 428)
            Me.cmdSave.Name = "cmdSave"
            Me.cmdSave.Size = New System.Drawing.Size(60, 23)
            Me.cmdSave.TabIndex = 18
            Me.cmdSave.Tag = "CB01"
            Me.cmdSave.Text = "Luu"
            Me.cmdSave.UseVisualStyleBackColor = False
            '
            'cmdNew
            '
            Me.cmdNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
            Me.cmdNew.Location = New System.Drawing.Point(62, 428)
            Me.cmdNew.Name = "cmdNew"
            Me.cmdNew.Size = New System.Drawing.Size(60, 23)
            Me.cmdNew.TabIndex = 19
            Me.cmdNew.Tag = "CB02"
            Me.cmdNew.Text = "Moi"
            Me.cmdNew.UseVisualStyleBackColor = False
            '
            'cmdPrint
            '
            Me.cmdPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
            Me.cmdPrint.Location = New System.Drawing.Point(122, 428)
            Me.cmdPrint.Name = "cmdPrint"
            Me.cmdPrint.Size = New System.Drawing.Size(60, 23)
            Me.cmdPrint.TabIndex = 20
            Me.cmdPrint.Tag = "CB03"
            Me.cmdPrint.Text = "In ctu"
            Me.cmdPrint.UseVisualStyleBackColor = False
            '
            'cmdEdit
            '
            Me.cmdEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
            Me.cmdEdit.Location = New System.Drawing.Point(182, 428)
            Me.cmdEdit.Name = "cmdEdit"
            Me.cmdEdit.Size = New System.Drawing.Size(60, 23)
            Me.cmdEdit.TabIndex = 21
            Me.cmdEdit.Tag = "CB04"
            Me.cmdEdit.Text = "Sua"
            Me.cmdEdit.UseVisualStyleBackColor = False
            '
            'cmdDelete
            '
            Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
            Me.cmdDelete.Location = New System.Drawing.Point(242, 428)
            Me.cmdDelete.Name = "cmdDelete"
            Me.cmdDelete.Size = New System.Drawing.Size(60, 23)
            Me.cmdDelete.TabIndex = 22
            Me.cmdDelete.Tag = "CB05"
            Me.cmdDelete.Text = "Xoa"
            Me.cmdDelete.UseVisualStyleBackColor = False
            '
            'cmdView
            '
            Me.cmdView.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdView.BackColor = System.Drawing.SystemColors.Control
            Me.cmdView.Location = New System.Drawing.Point(302, 428)
            Me.cmdView.Name = "cmdView"
            Me.cmdView.Size = New System.Drawing.Size(60, 23)
            Me.cmdView.TabIndex = 23
            Me.cmdView.Tag = "CB06"
            Me.cmdView.Text = "Xem"
            Me.cmdView.UseVisualStyleBackColor = False
            '
            'cmdSearch
            '
            Me.cmdSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
            Me.cmdSearch.Location = New System.Drawing.Point(362, 428)
            Me.cmdSearch.Name = "cmdSearch"
            Me.cmdSearch.Size = New System.Drawing.Size(60, 23)
            Me.cmdSearch.TabIndex = 24
            Me.cmdSearch.Tag = "CB07"
            Me.cmdSearch.Text = "Tim"
            Me.cmdSearch.UseVisualStyleBackColor = False
            '
            'cmdClose
            '
            Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
            Me.cmdClose.Location = New System.Drawing.Point(422, 428)
            Me.cmdClose.Name = "cmdClose"
            Me.cmdClose.Size = New System.Drawing.Size(60, 23)
            Me.cmdClose.TabIndex = 25
            Me.cmdClose.Tag = "CB08"
            Me.cmdClose.Text = "Quay ra"
            Me.cmdClose.UseVisualStyleBackColor = False
            '
            'cmdOption
            '
            Me.cmdOption.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdOption.BackColor = System.Drawing.SystemColors.Control
            Me.cmdOption.Location = New System.Drawing.Point(543, 428)
            Me.cmdOption.Name = "cmdOption"
            Me.cmdOption.Size = New System.Drawing.Size(20, 23)
            Me.cmdOption.TabIndex = 26
            Me.cmdOption.TabStop = False
            Me.cmdOption.Tag = "CB09"
            Me.cmdOption.UseVisualStyleBackColor = False
            '
            'cmdTop
            '
            Me.cmdTop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdTop.BackColor = System.Drawing.SystemColors.Control
            Me.cmdTop.Location = New System.Drawing.Point(562, 428)
            Me.cmdTop.Name = "cmdTop"
            Me.cmdTop.Size = New System.Drawing.Size(20, 23)
            Me.cmdTop.TabIndex = 27
            Me.cmdTop.TabStop = False
            Me.cmdTop.Tag = "CB10"
            Me.cmdTop.UseVisualStyleBackColor = False
            '
            'cmdPrev
            '
            Me.cmdPrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdPrev.BackColor = System.Drawing.SystemColors.Control
            Me.cmdPrev.Location = New System.Drawing.Point(581, 428)
            Me.cmdPrev.Name = "cmdPrev"
            Me.cmdPrev.Size = New System.Drawing.Size(20, 23)
            Me.cmdPrev.TabIndex = 28
            Me.cmdPrev.TabStop = False
            Me.cmdPrev.Tag = "CB11"
            Me.cmdPrev.UseVisualStyleBackColor = False
            '
            'cmdNext
            '
            Me.cmdNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdNext.BackColor = System.Drawing.SystemColors.Control
            Me.cmdNext.Location = New System.Drawing.Point(600, 428)
            Me.cmdNext.Name = "cmdNext"
            Me.cmdNext.Size = New System.Drawing.Size(20, 23)
            Me.cmdNext.TabIndex = 29
            Me.cmdNext.TabStop = False
            Me.cmdNext.Tag = "CB12"
            Me.cmdNext.UseVisualStyleBackColor = False
            '
            'cmdBottom
            '
            Me.cmdBottom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdBottom.BackColor = System.Drawing.SystemColors.Control
            Me.cmdBottom.Location = New System.Drawing.Point(619, 428)
            Me.cmdBottom.Name = "cmdBottom"
            Me.cmdBottom.Size = New System.Drawing.Size(20, 23)
            Me.cmdBottom.TabIndex = 30
            Me.cmdBottom.TabStop = False
            Me.cmdBottom.Tag = "CB13"
            Me.cmdBottom.UseVisualStyleBackColor = False
            '
            'lblMa_dvcs
            '
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New System.Drawing.Point(272, 456)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New System.Drawing.Size(48, 13)
            Me.lblMa_dvcs.TabIndex = 13
            Me.lblMa_dvcs.Tag = "L001"
            Me.lblMa_dvcs.Text = "Ma dvcs"
            Me.lblMa_dvcs.Visible = False
            '
            'txtMa_dvcs
            '
            Me.txtMa_dvcs.BackColor = System.Drawing.Color.White
            Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New System.Drawing.Point(320, 456)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.Size = New System.Drawing.Size(100, 20)
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
            Me.lblTen_dvcs.Location = New System.Drawing.Point(424, 456)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New System.Drawing.Size(87, 13)
            Me.lblTen_dvcs.TabIndex = 15
            Me.lblTen_dvcs.Tag = "FCRF"
            Me.lblTen_dvcs.Text = "Ten don vi co so"
            Me.lblTen_dvcs.Visible = False
            '
            'lblSo_ct
            '
            Me.lblSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblSo_ct.AutoSize = True
            Me.lblSo_ct.Location = New System.Drawing.Point(438, 7)
            Me.lblSo_ct.Name = "lblSo_ct"
            Me.lblSo_ct.Size = New System.Drawing.Size(35, 13)
            Me.lblSo_ct.TabIndex = 16
            Me.lblSo_ct.Tag = "L006"
            Me.lblSo_ct.Text = "So pn"
            '
            'txtSo_ct
            '
            Me.txtSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtSo_ct.BackColor = System.Drawing.Color.White
            Me.txtSo_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtSo_ct.Location = New System.Drawing.Point(538, 5)
            Me.txtSo_ct.Name = "txtSo_ct"
            Me.txtSo_ct.Size = New System.Drawing.Size(100, 20)
            Me.txtSo_ct.TabIndex = 9
            Me.txtSo_ct.Tag = "FCNBCF"
            Me.txtSo_ct.Text = "TXTSO_CT"
            Me.txtSo_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'txtNgay_lct
            '
            Me.txtNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtNgay_lct.BackColor = System.Drawing.Color.White
            Me.txtNgay_lct.Location = New System.Drawing.Point(538, 26)
            Me.txtNgay_lct.MaxLength = 10
            Me.txtNgay_lct.Name = "txtNgay_lct"
            Me.txtNgay_lct.Size = New System.Drawing.Size(100, 20)
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
            Me.txtTy_gia.Location = New System.Drawing.Point(538, 68)
            Me.txtTy_gia.MaxLength = 8
            Me.txtTy_gia.Name = "txtTy_gia"
            Me.txtTy_gia.Size = New System.Drawing.Size(100, 20)
            Me.txtTy_gia.TabIndex = 13
            Me.txtTy_gia.Tag = "FNCF"
            Me.txtTy_gia.Text = "m_ip_tg"
            Me.txtTy_gia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtTy_gia.Value = 0R
            '
            'lblNgay_lct
            '
            Me.lblNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblNgay_lct.AutoSize = True
            Me.lblNgay_lct.Location = New System.Drawing.Point(438, 28)
            Me.lblNgay_lct.Name = "lblNgay_lct"
            Me.lblNgay_lct.Size = New System.Drawing.Size(64, 13)
            Me.lblNgay_lct.TabIndex = 20
            Me.lblNgay_lct.Tag = "L007"
            Me.lblNgay_lct.Text = "Ngay lap pn"
            '
            'lblNgay_ct
            '
            Me.lblNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblNgay_ct.AutoSize = True
            Me.lblNgay_ct.Location = New System.Drawing.Point(438, 49)
            Me.lblNgay_ct.Name = "lblNgay_ct"
            Me.lblNgay_ct.Size = New System.Drawing.Size(83, 13)
            Me.lblNgay_ct.TabIndex = 21
            Me.lblNgay_ct.Tag = "L008"
            Me.lblNgay_ct.Text = "Ngay hach toan"
            '
            'lblTy_gia
            '
            Me.lblTy_gia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTy_gia.AutoSize = True
            Me.lblTy_gia.Location = New System.Drawing.Point(438, 70)
            Me.lblTy_gia.Name = "lblTy_gia"
            Me.lblTy_gia.Size = New System.Drawing.Size(36, 13)
            Me.lblTy_gia.TabIndex = 22
            Me.lblTy_gia.Tag = "L009"
            Me.lblTy_gia.Text = "Ty gia"
            '
            'txtNgay_ct
            '
            Me.txtNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtNgay_ct.BackColor = System.Drawing.Color.White
            Me.txtNgay_ct.Location = New System.Drawing.Point(538, 47)
            Me.txtNgay_ct.MaxLength = 10
            Me.txtNgay_ct.Name = "txtNgay_ct"
            Me.txtNgay_ct.Size = New System.Drawing.Size(100, 20)
            Me.txtNgay_ct.TabIndex = 11
            Me.txtNgay_ct.Tag = "FDNBCFDF"
            Me.txtNgay_ct.Text = "  /  /    "
            Me.txtNgay_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNgay_ct.Value = New Date(CType(0, Long))
            '
            'cmdMa_nt
            '
            Me.cmdMa_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdMa_nt.BackColor = System.Drawing.SystemColors.Control
            Me.cmdMa_nt.Enabled = False
            Me.cmdMa_nt.Location = New System.Drawing.Point(498, 68)
            Me.cmdMa_nt.Name = "cmdMa_nt"
            Me.cmdMa_nt.Size = New System.Drawing.Size(36, 20)
            Me.cmdMa_nt.TabIndex = 12
            Me.cmdMa_nt.TabStop = False
            Me.cmdMa_nt.Tag = "FCCFCMDDF"
            Me.cmdMa_nt.Text = "VND"
            Me.cmdMa_nt.UseVisualStyleBackColor = False
            '
            'tbDetail
            '
            Me.tbDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tbDetail.Controls.Add(Me.tpgDetail)
            Me.tbDetail.Controls.Add(Me.tpgOther)
            Me.tbDetail.Location = New System.Drawing.Point(2, 137)
            Me.tbDetail.Name = "tbDetail"
            Me.tbDetail.SelectedIndex = 0
            Me.tbDetail.Size = New System.Drawing.Size(638, 255)
            Me.tbDetail.TabIndex = 17
            '
            'tpgDetail
            '
            Me.tpgDetail.BackColor = System.Drawing.SystemColors.Control
            Me.tpgDetail.Controls.Add(Me.grdDetail)
            Me.tpgDetail.Location = New System.Drawing.Point(4, 22)
            Me.tpgDetail.Name = "tpgDetail"
            Me.tpgDetail.Size = New System.Drawing.Size(630, 229)
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
            Me.grdDetail.CaptionText = "F4 - Them, F5 - Xem phieu xuat, F8 - Xoa, F9 - Cap nhat danh muc lo"
            Me.grdDetail.Cell_EnableRaisingEvents = False
            Me.grdDetail.DataMember = ""
            Me.grdDetail.HeaderForeColor = System.Drawing.SystemColors.ControlText
            Me.grdDetail.Location = New System.Drawing.Point(-1, -1)
            Me.grdDetail.Name = "grdDetail"
            Me.grdDetail.Size = New System.Drawing.Size(633, 230)
            Me.grdDetail.TabIndex = 0
            Me.grdDetail.Tag = "L020CF"
            '
            'tpgOther
            '
            Me.tpgOther.Location = New System.Drawing.Point(4, 22)
            Me.tpgOther.Name = "tpgOther"
            Me.tpgOther.Size = New System.Drawing.Size(630, 229)
            Me.tpgOther.TabIndex = 1
            Me.tpgOther.Tag = "L017"
            Me.tpgOther.Text = "Thue GTGT dau vao"
            '
            'txtT_tien
            '
            Me.txtT_tien.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtT_tien.BackColor = System.Drawing.Color.White
            Me.txtT_tien.Enabled = False
            Me.txtT_tien.ForeColor = System.Drawing.Color.Black
            Me.txtT_tien.Format = "m_ip_tien"
            Me.txtT_tien.Location = New System.Drawing.Point(538, 401)
            Me.txtT_tien.MaxLength = 10
            Me.txtT_tien.Name = "txtT_tien"
            Me.txtT_tien.Size = New System.Drawing.Size(100, 20)
            Me.txtT_tien.TabIndex = 16
            Me.txtT_tien.Tag = "FN"
            Me.txtT_tien.Text = "m_ip_tien"
            Me.txtT_tien.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtT_tien.Value = 0R
            '
            'txtT_tien_nt
            '
            Me.txtT_tien_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtT_tien_nt.BackColor = System.Drawing.Color.White
            Me.txtT_tien_nt.Enabled = False
            Me.txtT_tien_nt.ForeColor = System.Drawing.Color.Black
            Me.txtT_tien_nt.Format = "m_ip_tien_nt"
            Me.txtT_tien_nt.Location = New System.Drawing.Point(437, 401)
            Me.txtT_tien_nt.MaxLength = 13
            Me.txtT_tien_nt.Name = "txtT_tien_nt"
            Me.txtT_tien_nt.Size = New System.Drawing.Size(100, 20)
            Me.txtT_tien_nt.TabIndex = 15
            Me.txtT_tien_nt.Tag = "FN"
            Me.txtT_tien_nt.Text = "m_ip_tien_nt"
            Me.txtT_tien_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtT_tien_nt.Value = 0R
            '
            'txtStatus
            '
            Me.txtStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.txtStatus.BackColor = System.Drawing.Color.White
            Me.txtStatus.Location = New System.Drawing.Point(8, 454)
            Me.txtStatus.MaxLength = 1
            Me.txtStatus.Name = "txtStatus"
            Me.txtStatus.Size = New System.Drawing.Size(25, 20)
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
            Me.lblStatus.Location = New System.Drawing.Point(438, 91)
            Me.lblStatus.Name = "lblStatus"
            Me.lblStatus.Size = New System.Drawing.Size(55, 13)
            Me.lblStatus.TabIndex = 29
            Me.lblStatus.Tag = ""
            Me.lblStatus.Text = "Trang thai"
            '
            'lblStatusMess
            '
            Me.lblStatusMess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblStatusMess.AutoSize = True
            Me.lblStatusMess.Location = New System.Drawing.Point(48, 456)
            Me.lblStatusMess.Name = "lblStatusMess"
            Me.lblStatusMess.Size = New System.Drawing.Size(191, 13)
            Me.lblStatusMess.TabIndex = 42
            Me.lblStatusMess.Tag = ""
            Me.lblStatusMess.Text = "1 - Ghi vao SC, 0 - Chua ghi vao so cai"
            Me.lblStatusMess.Visible = False
            '
            'txtKeyPress
            '
            Me.txtKeyPress.Location = New System.Drawing.Point(408, 96)
            Me.txtKeyPress.Name = "txtKeyPress"
            Me.txtKeyPress.Size = New System.Drawing.Size(10, 20)
            Me.txtKeyPress.TabIndex = 16
            '
            'cboStatus
            '
            Me.cboStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboStatus.BackColor = System.Drawing.Color.White
            Me.cboStatus.Enabled = False
            Me.cboStatus.Location = New System.Drawing.Point(498, 89)
            Me.cboStatus.Name = "cboStatus"
            Me.cboStatus.Size = New System.Drawing.Size(140, 21)
            Me.cboStatus.TabIndex = 14
            Me.cboStatus.TabStop = False
            Me.cboStatus.Tag = ""
            Me.cboStatus.Text = "cboStatus"
            '
            'cboAction
            '
            Me.cboAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboAction.BackColor = System.Drawing.Color.White
            Me.cboAction.Location = New System.Drawing.Point(498, 110)
            Me.cboAction.Name = "cboAction"
            Me.cboAction.Size = New System.Drawing.Size(140, 21)
            Me.cboAction.TabIndex = 15
            Me.cboAction.TabStop = False
            Me.cboAction.Tag = "CF"
            Me.cboAction.Text = "cboAction"
            '
            'lblAction
            '
            Me.lblAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblAction.AutoSize = True
            Me.lblAction.Location = New System.Drawing.Point(438, 112)
            Me.lblAction.Name = "lblAction"
            Me.lblAction.Size = New System.Drawing.Size(30, 13)
            Me.lblAction.TabIndex = 33
            Me.lblAction.Tag = ""
            Me.lblAction.Text = "Xu ly"
            '
            'lblMa_kh
            '
            Me.lblMa_kh.AutoSize = True
            Me.lblMa_kh.Location = New System.Drawing.Point(2, 7)
            Me.lblMa_kh.Name = "lblMa_kh"
            Me.lblMa_kh.Size = New System.Drawing.Size(55, 13)
            Me.lblMa_kh.TabIndex = 34
            Me.lblMa_kh.Tag = "L002"
            Me.lblMa_kh.Text = "Ma khach"
            '
            'txtMa_kh
            '
            Me.txtMa_kh.BackColor = System.Drawing.Color.White
            Me.txtMa_kh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_kh.Location = New System.Drawing.Point(88, 5)
            Me.txtMa_kh.Name = "txtMa_kh"
            Me.txtMa_kh.Size = New System.Drawing.Size(100, 20)
            Me.txtMa_kh.TabIndex = 0
            Me.txtMa_kh.Tag = "FCNBCF"
            Me.txtMa_kh.Text = "TXTMA_KH"
            '
            'lblTen_kh
            '
            Me.lblTen_kh.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTen_kh.Location = New System.Drawing.Point(192, 8)
            Me.lblTen_kh.Name = "lblTen_kh"
            Me.lblTen_kh.Size = New System.Drawing.Size(233, 15)
            Me.lblTen_kh.TabIndex = 36
            Me.lblTen_kh.Tag = "FCRF"
            Me.lblTen_kh.Text = "Ten khach"
            '
            'lblOng_ba
            '
            Me.lblOng_ba.AutoSize = True
            Me.lblOng_ba.Location = New System.Drawing.Point(2, 28)
            Me.lblOng_ba.Name = "lblOng_ba"
            Me.lblOng_ba.Size = New System.Drawing.Size(58, 13)
            Me.lblOng_ba.TabIndex = 37
            Me.lblOng_ba.Tag = "L003"
            Me.lblOng_ba.Text = "Nguoi giao"
            '
            'txtOng_ba
            '
            Me.txtOng_ba.BackColor = System.Drawing.Color.White
            Me.txtOng_ba.Location = New System.Drawing.Point(88, 26)
            Me.txtOng_ba.Name = "txtOng_ba"
            Me.txtOng_ba.Size = New System.Drawing.Size(133, 20)
            Me.txtOng_ba.TabIndex = 1
            Me.txtOng_ba.Tag = "FCCF"
            Me.txtOng_ba.Text = "txtOng_ba"
            '
            'lblMa_gd
            '
            Me.lblMa_gd.AutoSize = True
            Me.lblMa_gd.Location = New System.Drawing.Point(2, 70)
            Me.lblMa_gd.Name = "lblMa_gd"
            Me.lblMa_gd.Size = New System.Drawing.Size(68, 13)
            Me.lblMa_gd.TabIndex = 39
            Me.lblMa_gd.Tag = "L005"
            Me.lblMa_gd.Text = "Ma giao dich"
            '
            'txtMa_gd
            '
            Me.txtMa_gd.BackColor = System.Drawing.Color.White
            Me.txtMa_gd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_gd.Location = New System.Drawing.Point(88, 68)
            Me.txtMa_gd.Name = "txtMa_gd"
            Me.txtMa_gd.Size = New System.Drawing.Size(30, 20)
            Me.txtMa_gd.TabIndex = 4
            Me.txtMa_gd.Tag = "FCNBCF"
            Me.txtMa_gd.Text = "TXTMA_GD"
            '
            'lblTen_gd
            '
            Me.lblTen_gd.Location = New System.Drawing.Point(120, 70)
            Me.lblTen_gd.Name = "lblTen_gd"
            Me.lblTen_gd.Size = New System.Drawing.Size(304, 16)
            Me.lblTen_gd.TabIndex = 43
            Me.lblTen_gd.Tag = "FCRF"
            Me.lblTen_gd.Text = "Ten giao dich"
            '
            'lblTien_hang
            '
            Me.lblTien_hang.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTien_hang.AutoSize = True
            Me.lblTien_hang.Location = New System.Drawing.Point(192, 403)
            Me.lblTien_hang.Name = "lblTien_hang"
            Me.lblTien_hang.Size = New System.Drawing.Size(59, 13)
            Me.lblTien_hang.TabIndex = 60
            Me.lblTien_hang.Tag = "L010"
            Me.lblTien_hang.Text = "Tong cong"
            '
            'lblTen
            '
            Me.lblTen.AutoSize = True
            Me.lblTen.Location = New System.Drawing.Point(574, 456)
            Me.lblTen.Name = "lblTen"
            Me.lblTen.Size = New System.Drawing.Size(59, 13)
            Me.lblTen.TabIndex = 68
            Me.lblTen.Tag = "RF"
            Me.lblTen.Text = "Ten chung"
            Me.lblTen.Visible = False
            '
            'txtDien_giai
            '
            Me.txtDien_giai.BackColor = System.Drawing.Color.White
            Me.txtDien_giai.Location = New System.Drawing.Point(88, 47)
            Me.txtDien_giai.Name = "txtDien_giai"
            Me.txtDien_giai.Size = New System.Drawing.Size(337, 20)
            Me.txtDien_giai.TabIndex = 3
            Me.txtDien_giai.Tag = "FCCF"
            Me.txtDien_giai.Text = "txtDien_giai"
            '
            'lblDien_giai
            '
            Me.lblDien_giai.AutoSize = True
            Me.lblDien_giai.Location = New System.Drawing.Point(2, 49)
            Me.lblDien_giai.Name = "lblDien_giai"
            Me.lblDien_giai.Size = New System.Drawing.Size(48, 13)
            Me.lblDien_giai.TabIndex = 75
            Me.lblDien_giai.Tag = "L004"
            Me.lblDien_giai.Text = "Dien giai"
            '
            'txtT_so_luong
            '
            Me.txtT_so_luong.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtT_so_luong.BackColor = System.Drawing.Color.White
            Me.txtT_so_luong.Enabled = False
            Me.txtT_so_luong.ForeColor = System.Drawing.Color.Black
            Me.txtT_so_luong.Format = "m_ip_sl"
            Me.txtT_so_luong.Location = New System.Drawing.Point(336, 401)
            Me.txtT_so_luong.MaxLength = 8
            Me.txtT_so_luong.Name = "txtT_so_luong"
            Me.txtT_so_luong.Size = New System.Drawing.Size(100, 20)
            Me.txtT_so_luong.TabIndex = 14
            Me.txtT_so_luong.Tag = "FN"
            Me.txtT_so_luong.Text = "m_ip_sl"
            Me.txtT_so_luong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtT_so_luong.Value = 0R
            '
            'txtLoai_ct
            '
            Me.txtLoai_ct.BackColor = System.Drawing.Color.White
            Me.txtLoai_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtLoai_ct.Location = New System.Drawing.Point(504, 454)
            Me.txtLoai_ct.Name = "txtLoai_ct"
            Me.txtLoai_ct.Size = New System.Drawing.Size(30, 20)
            Me.txtLoai_ct.TabIndex = 76
            Me.txtLoai_ct.Tag = "FC"
            Me.txtLoai_ct.Text = "TXTLOAI_CT"
            Me.txtLoai_ct.Visible = False
            '
            'lblNgay_hd
            '
            Me.lblNgay_hd.AutoSize = True
            Me.lblNgay_hd.Location = New System.Drawing.Point(232, 91)
            Me.lblNgay_hd.Name = "lblNgay_hd"
            Me.lblNgay_hd.Size = New System.Drawing.Size(65, 13)
            Me.lblNgay_hd.TabIndex = 80
            Me.lblNgay_hd.Tag = "LZ02"
            Me.lblNgay_hd.Text = "Ngay ct goc"
            '
            'txtNgay_ct0
            '
            Me.txtNgay_ct0.BackColor = System.Drawing.Color.White
            Me.txtNgay_ct0.Location = New System.Drawing.Point(320, 89)
            Me.txtNgay_ct0.MaxLength = 10
            Me.txtNgay_ct0.Name = "txtNgay_ct0"
            Me.txtNgay_ct0.Size = New System.Drawing.Size(80, 20)
            Me.txtNgay_ct0.TabIndex = 6
            Me.txtNgay_ct0.Tag = "FDCFDF"
            Me.txtNgay_ct0.Text = "  /  /    "
            Me.txtNgay_ct0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtNgay_ct0.Value = New Date(CType(0, Long))
            '
            'txtSo_ct0
            '
            Me.txtSo_ct0.BackColor = System.Drawing.Color.White
            Me.txtSo_ct0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtSo_ct0.Location = New System.Drawing.Point(112, 89)
            Me.txtSo_ct0.Name = "txtSo_ct0"
            Me.txtSo_ct0.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.txtSo_ct0.Size = New System.Drawing.Size(100, 20)
            Me.txtSo_ct0.TabIndex = 5
            Me.txtSo_ct0.Tag = "FCCF"
            Me.txtSo_ct0.Text = "TXTSO_CT0"
            '
            'lblSo_hd
            '
            Me.lblSo_hd.AutoSize = True
            Me.lblSo_hd.Location = New System.Drawing.Point(2, 91)
            Me.lblSo_hd.Name = "lblSo_hd"
            Me.lblSo_hd.Size = New System.Drawing.Size(53, 13)
            Me.lblSo_hd.TabIndex = 79
            Me.lblSo_hd.Tag = "LZ01"
            Me.lblSo_hd.Text = "So ct goc"
            '
            'txtFcode1
            '
            Me.txtFcode1.BackColor = System.Drawing.Color.White
            Me.txtFcode1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtFcode1.Location = New System.Drawing.Point(112, 110)
            Me.txtFcode1.Name = "txtFcode1"
            Me.txtFcode1.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.txtFcode1.Size = New System.Drawing.Size(100, 20)
            Me.txtFcode1.TabIndex = 7
            Me.txtFcode1.Tag = "FCCF"
            Me.txtFcode1.Text = "TXTFCODE1"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(2, 112)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(93, 13)
            Me.Label1.TabIndex = 82
            Me.Label1.Tag = "LZ03"
            Me.Label1.Text = "Bien ban ban giao"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(232, 112)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(76, 13)
            Me.Label2.TabIndex = 84
            Me.Label2.Tag = "LZ04"
            Me.Label2.Text = "Ngay ban giao"
            '
            'txtFdate1
            '
            Me.txtFdate1.BackColor = System.Drawing.Color.White
            Me.txtFdate1.Location = New System.Drawing.Point(320, 110)
            Me.txtFdate1.MaxLength = 10
            Me.txtFdate1.Name = "txtFdate1"
            Me.txtFdate1.Size = New System.Drawing.Size(80, 20)
            Me.txtFdate1.TabIndex = 8
            Me.txtFdate1.Tag = "FDCFDF"
            Me.txtFdate1.Text = "  /  /    "
            Me.txtFdate1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtFdate1.Value = New Date(CType(0, Long))
            '
            'txtFnote1
            '
            Me.txtFnote1.BackColor = System.Drawing.Color.White
            Me.txtFnote1.Location = New System.Drawing.Point(292, 25)
            Me.txtFnote1.Name = "txtFnote1"
            Me.txtFnote1.Size = New System.Drawing.Size(133, 20)
            Me.txtFnote1.TabIndex = 2
            Me.txtFnote1.Tag = "FCCF"
            Me.txtFnote1.Text = "txtFnote1"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(222, 27)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(62, 13)
            Me.Label3.TabIndex = 86
            Me.Label3.Tag = "LZ05"
            Me.Label3.Text = "Nguoi nhan"
            '
            'frmVoucher
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(642, 473)
            Me.Controls.Add(Me.txtFnote1)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.txtFdate1)
            Me.Controls.Add(Me.txtFcode1)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.lblNgay_hd)
            Me.Controls.Add(Me.txtNgay_ct0)
            Me.Controls.Add(Me.txtSo_ct0)
            Me.Controls.Add(Me.lblSo_hd)
            Me.Controls.Add(Me.txtLoai_ct)
            Me.Controls.Add(Me.txtT_so_luong)
            Me.Controls.Add(Me.txtDien_giai)
            Me.Controls.Add(Me.lblDien_giai)
            Me.Controls.Add(Me.lblTen)
            Me.Controls.Add(Me.lblTien_hang)
            Me.Controls.Add(Me.txtMa_gd)
            Me.Controls.Add(Me.lblMa_gd)
            Me.Controls.Add(Me.txtOng_ba)
            Me.Controls.Add(Me.lblOng_ba)
            Me.Controls.Add(Me.txtMa_kh)
            Me.Controls.Add(Me.lblMa_kh)
            Me.Controls.Add(Me.lblAction)
            Me.Controls.Add(Me.txtKeyPress)
            Me.Controls.Add(Me.lblStatusMess)
            Me.Controls.Add(Me.lblStatus)
            Me.Controls.Add(Me.txtT_tien_nt)
            Me.Controls.Add(Me.txtT_tien)
            Me.Controls.Add(Me.lblTy_gia)
            Me.Controls.Add(Me.lblNgay_ct)
            Me.Controls.Add(Me.lblNgay_lct)
            Me.Controls.Add(Me.txtTy_gia)
            Me.Controls.Add(Me.lblSo_ct)
            Me.Controls.Add(Me.lblMa_dvcs)
            Me.Controls.Add(Me.txtStatus)
            Me.Controls.Add(Me.txtNgay_ct)
            Me.Controls.Add(Me.txtNgay_lct)
            Me.Controls.Add(Me.txtSo_ct)
            Me.Controls.Add(Me.txtMa_dvcs)
            Me.Controls.Add(Me.lblTen_dvcs)
            Me.Controls.Add(Me.lblTen_gd)
            Me.Controls.Add(Me.lblTen_kh)
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
            Me.xInventory.AllowLotUpdate = True
            Me.xInventory.Init()
        End Sub

        Public Sub InitRecords()
            Dim str As String
            If oVoucher.isRead Then
                str = String.Concat(New String() {"EXEC fs_LoadIRTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', -1"})
            Else
                str = String.Concat(New String() {"EXEC fs_LoadIRTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
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

        Private Sub LotItem(ByVal sender As Object, ByVal e As EventArgs)
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
                Me.xInventory.ShowLotUpdate(False)
            End If
        End Sub

        Private Sub NewItem(ByVal sender As Object, ByVal e As EventArgs)
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
                Dim cell As DataGridCell
                Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
                If (currentRowIndex < 0) Then
                    modVoucher.tblDetail.AddNew()
                    cell = New DataGridCell(0, 0)
                    Me.grdDetail.CurrentCell = cell
                ElseIf ((Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) AndAlso Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt")))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt"))), "", False) <> 0)) Then
                    Dim count As Integer = modVoucher.tblDetail.Count
                    Me.grdDetail.BeforeAddNewItem()
                    cell = New DataGridCell(count, 0)
                    Me.grdDetail.CurrentCell = cell
                    Me.grdDetail.AfterAddNewItem()
                End If
            End If
        End Sub

        Private Sub oBrowIssueLookupLoad(ByVal sender As Object, ByVal e As EventArgs)
            Dim r As Integer = 0
            Dim num6 As Integer = (oBrowIssueLookup.dv.Count - 1)
            Dim num As Integer = 0
            For num = 0 To num6
                If BooleanType.FromObject(ObjectType.BitAndObj((ObjectType.ObjTst(oBrowIssueLookup.dv.Item(num).Item("stt_rec"), Me.strInIDNumber, False) = 0), (ObjectType.ObjTst(oBrowIssueLookup.dv.Item(num).Item("stt_rec0"), Me.strInLineIDNumber, False) = 0))) Then
                    r = num
                    Exit For
                End If
            Next
            If (r > 0) Then
                oBrowIssueLookup.grdLookup.CurrentCell = New DataGridCell(r, 0)
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
                        oVoucher.ViewDeletedRecord("fs_SearchDeletedIRTran", "IRMaster", "IRDetail", "t_tien", "t_tien_nt")
                        Exit Select
                    Case 4
                        Dim strKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                        oVoucher.ViewPostedFile("ct00", strKey, "GL")
                        Exit Select
                    Case 5
                        Dim str2 As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                        Dim code As String = "IN"
                        If VoucherCode = "PNK" Then
                            code = "IN_PKH"
                        End If
                        oVoucher.ViewPostedFile(tblStock, str2, code)
                        Exit Select
                End Select
            End If
        End Sub

        Private Function Post() As String
            Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "groupby", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            Dim str3 As String = "EXEC fs_PostIR "
            Return (StringType.FromObject(ObjectType.AddObj(((((((str3 & "'" & modVoucher.VoucherCode & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))) & "'") & ", '" & Strings.Trim(str) & "'"), ObjectType.AddObj(ObjectType.AddObj(", '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))) & ", 1")
        End Function

        Public Sub Print()
            Dim print As New frmPrint
            print.txtTitle.Text = StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct"))), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct2")))))
            print.txtSo_lien.Value = DoubleType.FromObject(modVoucher.oVoucherRow.Item("so_lien"))
            Dim table As DataTable = clsprint.InitComboReport(modVoucher.sysConn, print.cboReports, "IRTran")
            Dim result As DialogResult = print.ShowDialog
            If ((result <> DialogResult.Cancel) AndAlso (print.txtSo_lien.Value > 0)) Then
                Dim selectedIndex As Integer = print.cboReports.SelectedIndex
                Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
                Dim view As New DataView
                Dim ds As New DataSet
                Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj((("EXEC fs_PrintIRTran '" & modVoucher.cLan) & "', " & "[a.stt_rec = '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'], '"), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf")))), "'"))
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
                clsprint.SetReportVar(modVoucher.sysConn, modVoucher.appConn, "IRTran", modVoucher.oOption, clsprint.oRpt)
                clsprint.oRpt.SetParameterValue("Title", Strings.Trim(print.txtTitle.Text))
                Dim str2 As String = Strings.Replace(Strings.Replace(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("401")), "%s1", Me.txtNgay_ct.Value.Day.ToString, 1, -1, CompareMethod.Binary), "%s2", Me.txtNgay_ct.Value.Month.ToString, 1, -1, CompareMethod.Binary), "%s3", Me.txtNgay_ct.Value.Year.ToString, 1, -1, CompareMethod.Binary)
                Dim str4 As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("402")), "%s", Strings.Trim(Me.txtSo_ct.Text), 1, -1, CompareMethod.Binary)
                Dim str As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("403")), "%s", clsprint.Num2Words(New Decimal(Me.txtT_tien.Value), StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.oOption.Item("m_use_2fc"), "1", False) = 0), RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "SELECT dbo.ff30_FC1()")), RuntimeHelpers.GetObjectValue(modVoucher.oOption.Item("m_ma_nt0"))))), 1, -1, CompareMethod.Binary)
                clsprint.oRpt.SetParameterValue("s_byword", str)
                clsprint.oRpt.SetParameterValue("t_date", str2)
                clsprint.oRpt.SetParameterValue("t_number", str4)
                clsprint.oRpt.SetParameterValue("nTotal", Me.txtT_tien.Value)
                clsprint.oRpt.SetParameterValue("f_ong_ba", Strings.Trim(Me.txtOng_ba.Text))
                clsprint.oRpt.SetParameterValue("f_kh", (Strings.Trim(Me.txtMa_kh.Text) & " - " & Strings.Trim(Me.lblTen_kh.Text)))
                Dim str3 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "dia_chi", ("ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'"))))
                clsprint.oRpt.SetParameterValue("f_dia_chi", str3)
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

        Private Function RealValue(ByVal oValue As Object) As String
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(oValue), "C") Then
                Return ""
            End If
            Return Strings.Trim(StringType.FromObject(oValue))
        End Function

        Public Sub RefrehForm()
            Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
            Me.grdHeader.Scatter()
            ScatterMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
            Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'")
            modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
            Me.UpdateList()
            Me.vCaptionRefresh()
            Me.cmdNew.Focus()
        End Sub

        Private Sub RefreshControlField()
        End Sub

        Public Sub Save()
            Me.txtStatus.Text = Strings.Trim(StringType.FromObject(Me.tblHandling.Rows.Item(Me.cboAction.SelectedIndex).Item("action_id")))
            Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
            Try
                Dim cell As New DataGridCell(0, 0)
                Me.grdDetail.CurrentCell = cell
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            If Not Me.oSecurity.GetActionRight Then
                oVoucher.isContinue = False
            ElseIf Not Me.grdHeader.CheckEmpty(RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("035"))) Then
                oVoucher.isContinue = False
            Else
                Dim num As Integer
                Dim num3 As Integer = 0
                Dim num12 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                'Do While (num <= num12)
                '    If Not (Not Information.IsDBNull(tblDetail.Item(num).Item("ma_vt")) AndAlso (tblDetail.Item(num).Item("ma_vt") <> "")) Then
                '        num3 = 1
                '        Exit Do
                '    End If
                '    If Not (Not Information.IsDBNull(tblDetail.Item(num).Item("tk_vt")) AndAlso (tblDetail.Item(num).Item("tk_vt") <> "")) Then
                '        num3 = 1
                '        Exit Do
                '    End If
                '    If Not (Not Information.IsDBNull(tblDetail.Item(num).Item("tk_du")) AndAlso (tblDetail.Item(num).Item("tk_du") <> "")) Then
                '        num3 = 1
                '        Exit Do
                '    End If
                '    If Not (Not Information.IsDBNull(tblDetail.Item(num).Item("ma_sp")) AndAlso (tblDetail.Item(num).Item("tk_du") <> "")) Then
                '        num3 = 1
                '        Exit Do
                '    End If
                '    num += 1
                'Loop
                If (num3 = 1) Then
                    Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("022")), 2)
                    oVoucher.isContinue = False
                Else
                    Dim str As String
                    Dim num2 As Integer
                    Dim num11 As Integer = (modVoucher.tblDetail.Count - 1)
                    num = 0
                    Do While (num <= num11)
                        Dim replacement As String = Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("ma_vt")))
                        If (clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("so_luong")), "N") AndAlso (ObjectType.ObjTst(Sql.GetValue((modVoucher.appConn), "dmvt", "gia_ton", ("ma_vt = '" & replacement & "'")), 3, False) = 0)) Then
                            oVoucher.isContinue = False
                            Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("043")), "%s", replacement, 1, -1, CompareMethod.Binary), 2)
                            Return
                        End If
                        num += 1
                    Loop
                    num3 = (modVoucher.tblDetail.Count - 1)
                    num = num3
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
                        num3 = (modVoucher.tblDetail.Count - 1)
                        Dim sLeft As String = clsfields.CheckEmptyFieldList("stt_rec", strFieldList, modVoucher.tblDetail)
                        Try
                            If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                                Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("044")), "%s", GetColumn(Me.grdDetail, sLeft).HeaderText, 1, -1, CompareMethod.Binary), 2)
                                oVoucher.isContinue = False
                                Return
                            End If
                        Catch exception2 As Exception
                            ProjectData.SetProjectError(exception2)
                            Dim exception As Exception = exception2
                            ProjectData.ClearProjectError()
                        End Try
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
                    Else
                        Dim str4 As String
                        Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
                        If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                            auditamount.AuditAmounts(New Decimal(Me.txtT_tien.Value), "tien", modVoucher.tblDetail)
                        End If
                        Me.UpdateList()
                        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                            Me.cIDNumber = oVoucher.GetIdentityNumber
                            modVoucher.tblMaster.AddNew()
                            Me.iMasterRow = (modVoucher.tblMaster.Count - 1)
                            modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec") = Me.cIDNumber
                            modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_ct") = modVoucher.VoucherCode
                        Else
                            Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                            Me.BeforUpdateIR(Me.cIDNumber, "Edit")
                        End If
                        DirLib.SetDatetime(modVoucher.appConn, modVoucher.tblMaster.Item(Me.iMasterRow), oVoucher.cAction)
                        Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
                        Me.grdHeader.Gather()
                        GatherMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
                        ' Dinh dang so chung tu
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct"))), Me.txtSo_ct.MaxLength)
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct0") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct0"))), Me.txtSo_ct0.MaxLength)
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("fcode1") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("fcode1"))), Me.txtFcode1.MaxLength)
                        ' end
                        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                            str4 = GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row)
                        Else
                            Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                            str4 = ((GenSQLUpdate((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row, cKey) & ChrW(13) & GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)) & ChrW(13) & GenSQLDelete("ctgt30", cKey))
                        End If
                        cString = "ma_ct, ngay_ct, so_ct, stt_rec"
                        Dim str3 As String = ("stt_rec = '" & Me.cIDNumber & "' or stt_rec = '' or stt_rec is null")
                        modVoucher.tblDetail.RowFilter = str3
                        num3 = (modVoucher.tblDetail.Count - 1)
                        Dim num4 As Integer = 0
                        Dim num6 As Integer = num3
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
                                str4 = (str4 & ChrW(13) & GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), modVoucher.tblDetail.Item(num).Row))
                            End If
                            num += 1
                        Loop
                        oVoucher.IncreaseVoucherNo(Strings.Trim(Me.txtSo_ct.Text))
                        Me.EDTBColumns(False)
                        Sql.SQLCompressExecute((modVoucher.appConn), str4)
                        str4 = Me.Post
                        Sql.SQLExecute((modVoucher.appConn), str4)
                        Me.grdHeader.UpdateFreeField(modVoucher.appConn, StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")))
                        Me.pnContent.Text = StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.tblMaster.Item(Me.iMasterRow).Item("status"), "3", False) <> 0), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("018")), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("019"))))
                        SaveLocalDataView(modVoucher.tblDetail)
                        oVoucher.RefreshStatus(Me.cboStatus)
                    End If
                End If
            End If
        End Sub

        Public Sub Search()
            Dim a As New frmSearch
            a.ShowDialog()
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
        End Sub

        Private Sub tbDetail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tbDetail.Click
            If (Me.tbDetail.SelectedIndex = 0) Then
            End If
        End Sub

        Private Sub tbDetail_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles tbDetail.Enter
            Me.grdDetail.Focus()
        End Sub

        Private Sub txt_Enter(ByVal sender As Object, ByVal e As EventArgs)
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"))) Then
                LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
                Return
            End If
            Dim sLeft As String = Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")))
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(StringType.StrCmp(sLeft, "", False) = 0)}, Nothing)
        End Sub
        Private Sub txt_Enter_Lo(ByVal sender As Object, ByVal e As EventArgs)
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"))) Then
                LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
                Return
            End If
            Try
                If Sql.GetValue(appConn, "dmlo", "ma_lo", "ma_vt='" + tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt") + "' and ma_lo='" + tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_lo") + "'").ToString = "" Then
                    LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {False}, Nothing)
                Else
                    LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
                End If
            Catch ex As Exception
                sender.readonly = False
            End Try
        End Sub

        Private Sub txtGia_enter(ByVal sender As Object, ByVal e As EventArgs)
            Me.noldGia = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
            Me.WhenNoneInputPrice(RuntimeHelpers.GetObjectValue(sender), e)
        End Sub

        Private Sub txtGia_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
            Me.noldGia_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
            Me.WhenNoneInputPrice(RuntimeHelpers.GetObjectValue(sender), e)
        End Sub

        Private Sub txtGia_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
            Dim num2 As Byte
            Dim num3 As Byte
            Dim num5 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
            Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
            If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
                num3 = num5
                num2 = digits
            Else
                num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
                num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
            End If
            Dim num6 As Decimal = Me.noldGia_nt
            Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
            If (Decimal.Compare(num, num6) <> 0) Then
                With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                    .Item("gia_nt") = num
                    .Item("gia") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                    Dim args As Object() = New Object() {ObjectType.MulObj(.Item("so_luong"), num), num3}
                    Dim copyBack As Boolean() = New Boolean() {False, True}
                    If copyBack(1) Then
                        num3 = ByteType.FromObject(args(1))
                    End If
                    .Item("tien_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                    Dim objArray2 As Object() = New Object() {ObjectType.MulObj(.Item("tien_nt"), Me.txtTy_gia.Value), num5}
                    copyBack = New Boolean() {False, True}
                    If copyBack(1) Then
                        num5 = ByteType.FromObject(objArray2(1))
                    End If
                    .Item("Tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                End With
                Me.UpdateList()
            End If
        End Sub

        Private Sub txtGia_valid(ByVal sender As Object, ByVal e As EventArgs)
            Dim num2 As Byte
            Dim num3 As Byte
            Dim num5 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
            Dim num4 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
            If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
                num3 = num5
                num2 = num4
            Else
                num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
                num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
            End If
            Dim noldGia As Decimal = Me.noldGia
            Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
            If (Decimal.Compare(num, noldGia) <> 0) Then
                With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                    .Item("gia") = num
                    Dim args As Object() = New Object() {ObjectType.MulObj(.Item("so_luong"), num), num5}
                    Dim copyBack As Boolean() = New Boolean() {False, True}
                    If copyBack(1) Then
                        num5 = ByteType.FromObject(args(1))
                    End If
                    .Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                End With
                Me.UpdateList()
            End If
        End Sub

        Private Sub txtKeyPress_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtKeyPress.Enter
            Me.grdDetail.Focus()
            Dim cell As New DataGridCell(0, 0)
            Me.grdDetail.CurrentCell = cell
        End Sub

        Private Sub txtNumber_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtSo_ct.Enter
            LateBinding.LateSet(sender, Nothing, "Text", New Object() {Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))}, Nothing)
        End Sub

        Private Sub txtSo_luong_enter(ByVal sender As Object, ByVal e As EventArgs)
            Me.noldSo_luong = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        End Sub

        Private Sub txtSo_luong_valid(ByVal sender As Object, ByVal e As EventArgs)
            Dim num2 As Byte
            Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
            If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
                num2 = num3
            Else
                num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            End If
            Dim num4 As Decimal = Me.noldSo_luong
            Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
            If ((Decimal.Compare(num, Decimal.Zero) = 0) AndAlso Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")), "C")) Then
                Dim replacement As String = Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")))
                If (ObjectType.ObjTst(Sql.GetValue((modVoucher.appConn), "dmvt", "gia_ton", ("ma_vt = '" & replacement & "'")), 3, False) = 0) Then
                    Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("043")), "%s", replacement, 1, -1, CompareMethod.Binary), 2)
                End If
            End If
            If (Decimal.Compare(num, num4) <> 0) Then
                With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia_nt"))) Then
                        .Item("gia_nt") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia"))) Then
                        .Item("gia") = 0
                    End If
                    .Item("so_luong") = num
                    Dim args As Object() = New Object() {ObjectType.MulObj(.Item("gia_nt"), num), num2}
                    Dim copyBack As Boolean() = New Boolean() {False, True}
                    If copyBack(1) Then
                        num2 = ByteType.FromObject(args(1))
                    End If
                    .Item("tien_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                    Dim objArray2 As Object() = New Object() {ObjectType.MulObj(.Item("gia"), num), num3}
                    copyBack = New Boolean() {False, True}
                    If copyBack(1) Then
                        num3 = ByteType.FromObject(objArray2(1))
                    End If
                    .Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                End With
                Me.grdDetail.Refresh()
                Me.UpdateList()
            End If
        End Sub

        Private Sub txtTien_enter(ByVal sender As Object, ByVal e As EventArgs)
            Me.noldTien = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
            Me.WhenNoneInputPrice(RuntimeHelpers.GetObjectValue(sender), e)
        End Sub

        Private Sub txtTien_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
            Me.noldTien_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
            Me.WhenNoneInputPrice(RuntimeHelpers.GetObjectValue(sender), e)
        End Sub

        Private Sub txtTien_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
            Dim num2 As Byte
            Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
            If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
                num2 = digits
            Else
                num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            End If
            Dim num4 As Decimal = Me.noldTien_nt
            Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
            If (Decimal.Compare(num, num4) <> 0) Then
                With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                    .Item("Tien_nt") = num
                    .Item("Tien") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                End With
                Me.UpdateList()
            End If
        End Sub

        Private Sub txtTien_valid(ByVal sender As Object, ByVal e As EventArgs)
            Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
            Dim noldTien As Decimal = Me.noldTien
            Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
            If (Decimal.Compare(num, noldTien) <> 0) Then
                With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                    .Item("Tien") = num
                End With
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
            Dim num4 As Decimal = Decimal.Zero
            Dim num2 As Decimal = Decimal.Zero
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit", "View"}) Then
                Dim num5 As Integer = (modVoucher.tblDetail.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num5)
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien"))) Then
                        zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblDetail.Item(i).Item("tien")))
                    End If
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien_nt"))) Then
                        num4 = DecimalType.FromObject(ObjectType.AddObj(num4, modVoucher.tblDetail.Item(i).Item("tien_nt")))
                    End If
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("so_luong"))) Then
                        num2 = DecimalType.FromObject(ObjectType.AddObj(num2, modVoucher.tblDetail.Item(i).Item("so_luong")))
                    End If
                    i += 1
                Loop
            End If
            Me.txtT_tien.Value = Convert.ToDouble(zero)
            Me.txtT_tien_nt.Value = Convert.ToDouble(num4)
            Me.txtT_so_luong.Value = Convert.ToDouble(num2)
        End Sub

        Public Sub vCaptionRefresh()
            Me.EDFC()
            Dim cAction As String = oVoucher.cAction
            If ((StringType.StrCmp(cAction, "Edit", False) = 0) OrElse (StringType.StrCmp(cAction, "View", False) = 0)) Then
                Me.pnContent.Text = StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.tblMaster.Item(Me.iMasterRow).Item("status"), "3", False) <> 0), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("018")), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("019"))))
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
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(i).Item("tien_nt"))) Then
                        tblDetail.Item(i).Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(i).Item("tien_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                    End If
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(i).Item("gia_nt"))) Then
                        tblDetail.Item(i).Item("gia") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(i).Item("gia_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                    End If
                    i += 1
                Loop
                tblDetail = Nothing
            End If
            Me.txtT_tien.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_tien_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        End Sub

        Public Sub View()
            Dim num3 As Decimal
            Dim frmAdd As New Form
            Dim gridformtran2 As New gridformtran
            Dim gridformtran As New gridformtran
            Dim tbs As New DataGridTableStyle
            Dim style As New DataGridTableStyle
            Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns) {}
            Dim index As Integer = 0
            Do
                cols(index) = New DataGridTextBoxColumn
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index <= MaxColumns - 1)
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
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblMaster), gridformtran2, (tbs), (cols), "IRMaster")
            index = 0
            Do
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index <= MaxColumns - 1)
            cols(2).Alignment = HorizontalAlignment.Right
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), gridformtran, (style), (cols), "IRDetail")
            index = 0
            Do
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index <= MaxColumns - 1)
            oVoucher.HideFields(gridformtran)
            Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
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
            expression = Strings.Replace(expression, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, CompareMethod.Binary)
            If Me.oSecurity.isViewTotalField Then
                expression = Strings.Replace(Strings.Replace(expression, "%n2", Strings.Trim(Strings.Format(num3, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))), 1, -1, CompareMethod.Binary), "%n3", Strings.Trim(Strings.Format(zero, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))), 1, -1, CompareMethod.Binary)
            Else
                expression = Strings.Replace(Strings.Replace(expression, "%n2", "X", 1, -1, CompareMethod.Binary), "%n3", "X", 1, -1, CompareMethod.Binary)
            End If
            panel.Text = expression
            AddHandler gridformtran2.CurrentCellChanged, New EventHandler(AddressOf Me.grdMVCurrentCellChanged)
            gridformtran2.CurrentRowIndex = Me.iMasterRow
            Obj.Init(frmAdd)
            Dim collection As New Collection
            collection.Add(Me, "Form", Nothing, Nothing)
            collection.Add(gridformtran2, "grdHeader", Nothing, Nothing)
            collection.Add(gridformtran, "grdDetail", Nothing, Nothing)
            Me.oSecurity.aVGrid = collection
            Me.oSecurity.InnitView()
            Me.oSecurity.InvisibleView()
            frmAdd.ShowDialog()
            frmAdd.Dispose()
            Me.iMasterRow = gridformtran2.CurrentRowIndex
            Me.RefrehForm()
        End Sub

        Private Sub ViewItem(ByVal sender As Object, ByVal e As EventArgs)
            If Not Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
                Return
            End If
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_vt")), "C") Then
                    Return
                End If
                Dim _date As New frmDate
                If (_date.ShowDialog <> DialogResult.OK) Then
                    Return
                End If
                Dim str As String = "fs_InventoryIssueLookup "
                str = StringType.FromObject(ObjectType.AddObj(str, Sql.ConvertVS2SQLType(_date.txtNgay_ct.Value, "")))
                str = StringType.FromObject(ObjectType.AddObj(str, ObjectType.AddObj(",", Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, ""))))
                str = (str & ", '" & Strings.Trim(StringType.FromObject(.Item("ma_vt"))) & "'")
                str = (str & ", '" & Me.RealValue(RuntimeHelpers.GetObjectValue(.Item("ma_kho"))) & "'")
                str = (str & ", '" & Me.RealValue(RuntimeHelpers.GetObjectValue(.Item("ma_vi_tri"))) & "'")
                str = (str & ", '" & Me.RealValue(RuntimeHelpers.GetObjectValue(.Item("ma_lo"))) & "'")
                str = (str & ", '" & modVoucher.cLan & "'")
                Me.strInIDNumber = StringType.FromObject(Interaction.IIf(clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("stt_rec_px")), "C"), "", RuntimeHelpers.GetObjectValue(.Item("stt_rec_px"))))
                Me.strInLineIDNumber = StringType.FromObject(Interaction.IIf(clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("stt_rec0px")), "C"), "", RuntimeHelpers.GetObjectValue(.Item("stt_rec0px"))))
                Me.oBrowIssueLookup = New clsbrowse
                AddHandler oBrowIssueLookup.frmLookup.Load, New EventHandler(AddressOf Me.oBrowIssueLookupLoad)
                oBrowIssueLookup.Lookup(modVoucher.sysConn, modVoucher.appConn, "IssueLookup", str)
                If Information.IsNothing(oBrowIssueLookup.CurDataRow) Then
                    Return
                End If
                .Item("stt_rec_px") = RuntimeHelpers.GetObjectValue(oBrowIssueLookup.CurDataRow.Item("stt_rec"))
                .Item("stt_rec0px") = RuntimeHelpers.GetObjectValue(oBrowIssueLookup.CurDataRow.Item("stt_rec0"))
                Dim num As Integer = IntegerType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "gia_ton", ("ma_vt = '" & Strings.Trim(StringType.FromObject(.Item("ma_vt"))) & "'")))
                If Not (clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("pn_gia_tb")), "L") Or (num = 2)) Then
                    Return
                End If
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("he_so")), "N") Then
                    .Item("he_so") = 1
                End If
                If (ObjectType.ObjTst(modVoucher.oOption.Item("m_use_2fc"), 0, False) = 0) Then
                    .Item("gia") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(oBrowIssueLookup.CurDataRow.Item("gia"), .Item("he_so")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                    If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("so_luong")), "N") Then
                        .Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(.Item("gia"), .Item("so_luong")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                    End If
                    If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
                        .Item("gia_nt") = RuntimeHelpers.GetObjectValue(.Item("gia"))
                        .Item("tien_nt") = RuntimeHelpers.GetObjectValue(.Item("tien"))
                    Else
                        .Item("gia_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(.Item("gia"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))}, Nothing, Nothing))
                        If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("so_luong")), "N") Then
                            .Item("tien_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(.Item("gia_nt"), .Item("so_luong")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                        End If
                    End If
                ElseIf (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("r_ma_nt1"), False) = 0) Then
                    .Item("gia") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Math), "Round", New Object() {ObjectType.MulObj(oBrowIssueLookup.CurDataRow.Item("gia_nt"), .Item("he_so")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                    .Item("gia_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Math), "Round", New Object() {ObjectType.MulObj(oBrowIssueLookup.CurDataRow.Item("gia"), .Item("he_so")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))}, Nothing, Nothing))
                    If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("so_luong")), "N") Then
                        .Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Math), "Round", New Object() {ObjectType.MulObj(.Item("gia"), .Item("so_luong")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                        .Item("tien_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Math), "Round", New Object() {ObjectType.MulObj(.Item("gia_nt"), .Item("so_luong")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                    End If
                Else
                    .Item("gia") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Math), "Round", New Object() {ObjectType.MulObj(oBrowIssueLookup.CurDataRow.Item("gia"), .Item("he_so")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                    .Item("gia_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Math), "Round", New Object() {ObjectType.MulObj(oBrowIssueLookup.CurDataRow.Item("gia_nt"), .Item("he_so")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))}, Nothing, Nothing))
                    If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("so_luong")), "N") Then
                        .Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Math), "Round", New Object() {ObjectType.MulObj(.Item("gia"), .Item("so_luong")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                        .Item("tien_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Math), "Round", New Object() {ObjectType.MulObj(.Item("gia_nt"), .Item("so_luong")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                    End If
                End If
            End With
            Me.UpdateList()
        End Sub

        Public Sub vTextRefresh()
        End Sub

        Private Sub WhenAddNewItem()
            modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("pn_gia_tb") = False
            modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
        End Sub

        Private Sub WhenItemLeave(ByVal sender As Object, ByVal e As EventArgs)
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
                Dim str2 As String = Strings.Trim(StringType.FromObject(.Item("ma_vt")))
                Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmvt", ("ma_vt = '" & str2 & "'")), DataRow)
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_kho")), "C") Then
                    Try
                        .Item("ma_kho") = Sql.GetValue(appConn, "select min(ma_kho) from dmkho where dbo.ff_inlist(ma_kho,'" + RuntimeHelpers.GetObjectValue(row.Item("ma_kho")) + "')=1 and ma_dvcs='" + Me.txtMa_dvcs.Text + "'")
                    Catch ex As Exception
                    End Try
                End If

                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_vi_tri")), "C") Then
                    .Item("ma_vi_tri") = RuntimeHelpers.GetObjectValue(row.Item("ma_vi_tri"))
                End If
                .Item("tk_vt") = RuntimeHelpers.GetObjectValue(row.Item("tk_vt"))
                If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkho", "dai_ly_yn", StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("ma_kho = '", .Item("ma_kho")), "'")))) Then
                    If (ObjectType.ObjTst(row.Item("tk_dl"), "", False) <> 0) Then
                        .Item("tk_vt") = RuntimeHelpers.GetObjectValue(row.Item("tk_dl"))
                    End If
                End If
                .Item("dvt") = RuntimeHelpers.GetObjectValue(row.Item("dvt"))
                Me.colDvt.TextBox.Text = StringType.FromObject(.Item("dvt"))
                .Item("he_so") = 1
                ' Truong hop kho va ke hoach nhap kho
                If Me.txtMa_dvcs.Text <> "CTY" Then
                    If IsDBNull(.Item("tk_vt")) Then
                        .Item("tk_vt") = ""
                    End If
                    If IsDBNull(.Item("ma_nx")) Then
                        .Item("ma_nx") = .Item("tk_vt")
                        .Item("tk_du") = .Item("tk_vt")
                    End If
                    If .Item("ma_nx") = "" Then
                        .Item("ma_nx") = .Item("tk_vt")
                        .Item("tk_du") = .Item("tk_vt")
                    End If
                End If
                'end
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
                End If
                'If VoucherCode = "PNE" Then
                '    Try
                '        .Item("ton13") = CDbl(Sql.GetValue(appConn, "select ton13 From cdlo13_dk where ma_kho=" + Sql.ConvertVS2SQLType(.Item("ma_kho"), "") + " AND ma_lo=" + Sql.ConvertVS2SQLType(.Item("ma_lo"), "") + " AND ma_vt=" + Sql.ConvertVS2SQLType(.Item("ma_vt"), "")))
                '    Catch ex As Exception
                '    End Try
                'End If
                If clsfields.isEmpty(.Item("so_luong"), "C") Then
                    .Item("so_luong") = 1
                End If
                'If row.Item("tk_nhap") = "" Or IsDBNull(row.Item("tk_nhap")) Then
                '    .Item("ma_nx") = .Item("tk_vt")
                '    .Item("tk_du") = .Item("tk_vt")
                'Else
                '    .Item("ma_nx") = row.Item("tk_nhap")
                '    .Item("tk_du") = row.Item("tk_nhap")
                'End If
                If Me.txtMa_gd.Text.Trim = "4" Then
                    .Item("ma_sp") = .Item("ma_vt")
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
            'Me.cOldLot = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
                Dim cKey As String = ("ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "'")
                Me.oLot.Key = cKey
                Me.oLot.Empty = (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmlo", "ma_lo", cKey))), "", False) = 0)
            End If
            view = Nothing
        End Sub

        Private Sub WhenNoneInputItemAccount(ByVal sender As Object, ByVal e As EventArgs)
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
                Dim str As String = Strings.Trim(StringType.FromObject(view.Item("ma_vt")))
                If BooleanType.FromObject(ObjectType.NotObj(Sql.GetValue((modVoucher.appConn), "dmvt", "sua_tk_vt", ("ma_vt = '" & str & "'")))) Then
                    Me.grdDetail.TabProcess()
                End If
            End If
            view = Nothing
        End Sub

        Private Sub WhenNoneInputPrice(ByVal sender As Object, ByVal e As EventArgs)
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("pn_gia_tb"))) Then
                    If BooleanType.FromObject(view.Item("pn_gia_tb")) Then
                        Dim str As String = Strings.Trim(StringType.FromObject(view.Item("ma_vt")))
                        Dim num As Integer = IntegerType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "gia_ton", ("ma_vt = '" & str & "'")))
                        If ((num = 1) Or (num = 4)) Then
                            Me.grdDetail.TabProcess()
                        End If
                    End If
                End If
            End If
        End Sub

        Private Sub WhenReasonEnter(ByVal sender As Object, ByVal e As EventArgs)
            Me.cOldResonCode = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        End Sub

        Private Sub WhenReasonLeave(ByVal sender As Object, ByVal e As EventArgs)
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_nx")), "C") Then
                    Dim str As String = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmnx", "tk", ("ma_nx = '" & Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))) & "'")))
                    If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("tk_du")), "C") Then
                        .Item("tk_du") = str
                    ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))), Me.cOldResonCode, False) <> 0) Then
                        .Item("tk_du") = str
                    End If
                End If
            End With
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
                            Dim sLeft As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "tk_dl", ("ma_vt = '" & str3 & "'"))))
                            If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                                .Item("tk_vt") = sLeft
                            End If
                        End If
                        'If VoucherCode = "PNE" Then
                        '    Try
                        '        .Item("ton13") = CDbl(Sql.GetValue(appConn, "select ton13 From cdlo13_dk where ma_kho=" + Sql.ConvertVS2SQLType(.Item("ma_kho"), "") + " AND ma_lo=" + Sql.ConvertVS2SQLType(.Item("ma_lo"), "") + " AND ma_vt=" + Sql.ConvertVS2SQLType(.Item("ma_vt"), "")))
                        '    Catch ex As Exception
                        '    End Try
                        'End If
                    End If
                End With
            End If
        End Sub
        Private Sub WhenLotLeave(ByVal sender As Object, ByVal e As EventArgs)
            If (Me.grdDetail.CurrentRowIndex >= 0) Then
                Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
                With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                    If (StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.cOldLot), False) <> 0) And Me.txtMa_gd.Text = "4" Then
                        Dim str3 As String = Strings.Trim(StringType.FromObject(.Item("ma_vt")))
                        'Dim sLeft As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "ctsx", "so_ct", ("ma_vt = '" & str3 & "' AND ma_lo='" & str & "'"))))
                        Dim lsx As String = Sql.GetValue(appConn, "phlsx", "so_lsx", "stt_rec in (select b.stt_rec from ctsx a join phsx b on a.stt_rec=b.stt_rec where a.ma_vt='" + str3.Trim + "' AND a.ma_lo='" + str.Trim + "' and b.ngay_ct<=" + Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "") + " and (b.ngay_th2 is null or b.ngay_th2 >=" + Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "") + "))")
                        If Not IsDBNull(lsx) Then
                            If (StringType.StrCmp(lsx, "", False) <> 0) Then
                                .Item("so_lsx") = lsx
                            End If
                        End If
                    End If
                    Try
                        Dim dr As DataRow = Sql.GetRow(appConn, "dmlo", "ma_vt='" + .Item("ma_vt") + "' AND ma_lo='" + .Item("ma_lo") + "'")
                        .Item("ma_lo0") = dr.Item("ma_lo0")
                        .Item("ngay_hhsd") = dr.Item("ngay_hhsd")
                    Catch ex As Exception
                    End Try
                    'If VoucherCode = "PNE" Then
                    '    Try
                    '        .Item("ton13") = CDbl(Sql.GetValue(appConn, "select ton13 From cdlo13_dk where ma_kho=" + Sql.ConvertVS2SQLType(.Item("ma_kho"), "") + " AND ma_lo=" + Sql.ConvertVS2SQLType(.Item("ma_lo"), "") + " AND ma_vt=" + Sql.ConvertVS2SQLType(.Item("ma_vt"), "")))
                    '    Catch ex As Exception
                    '    End Try
                    'End If
                End With
            End If
        End Sub

        Private Sub WhenUOMEnter(ByVal sender As Object, ByVal e As EventArgs)
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
                Return
            End If
            Dim str As String
            If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "'"))) Then
                str = ("(ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "' OR ma_vt = '*')")
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
        End Sub
        Private Sub WhenMOEnter(ByVal sender As Object, ByVal e As EventArgs)
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(view.Item("ma_vt"), "C") Then
                Dim str As String
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("ma_sp"))) Then
                    modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_sp") = ""
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("ma_lo"))) Then
                    modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_lo") = ""
                End If
                If Me.txtMa_gd.Text.Trim = "4" Then
                    str = "stt_rec in (select b.stt_rec from ctsx a join phsx b on a.stt_rec=b.stt_rec where "
                    str += " a.ma_vt='" + Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) + "'"
                    str += " AND a.ma_lo ='" + Strings.Trim(StringType.FromObject(view.Item("ma_lo"))) + "'"
                    str += " AND b.ngay_ct<=" + Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "")
                    str += " AND (b.ngay_th2 is null or b.ngay_th2 >=" + Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "") + "))"
                    Me.oMO.Key = str
                    Me.oMO.Empty = False
                    Me.oMO.Cancel = False
                ElseIf (ObjectType.ObjTst(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_sp"), "", False) = 0) Then
                    str = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj("stt_rec in (select b.stt_rec from ctsx a join phsx b on a.stt_rec=b.stt_rec where b.ngay_ct<=", Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "")), " and (b.ngay_th2 is null or b.ngay_th2 >="), Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "")), "))"))
                    Me.oMO.Key = str
                    Me.oMO.Empty = True
                    Me.oMO.Cancel = False
                Else
                    str = "(stt_rec in (select b.stt_rec from ctsx a join phsx b on a.stt_rec=b.stt_rec where a.ma_vt='" & Strings.Trim(StringType.FromObject(view.Item("ma_sp"))) & "'"
                    str += " and b.ngay_ct<=" + Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "")
                    str += " and (b.ngay_th2 is null or b.ngay_th2 >=" + Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "") + ")))"
                    Me.oMO.Key = str
                    Me.oMO.Empty = False
                    Me.oMO.Cancel = False
                End If
            End If
        End Sub

        Private Sub WhenUOMLeave(ByVal sender As Object, ByVal e As EventArgs)
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
        Private Sub frmRetrieveLoad(ByVal sender As Object, ByVal e As EventArgs)
            LateBinding.LateSet(sender, Nothing, "Text", New Object() {RuntimeHelpers.GetObjectValue(modVoucher.oLan.Item("304"))}, Nothing)
        End Sub
        Private Sub RetrieveItemsFromIR_NB()
            If Not Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) And (Me.txtMa_dvcs.Text = "KHO" And oVoucher.VoucherCode = "PNK") Then
                RetrieveItemsFromIR_NB2PNK()
                Return
            End If
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) And (Me.txtMa_dvcs.Text <> "KHO" Or oVoucher.VoucherCode = "PNE") Then
                Dim _date As New frmFilterPN
                AddHandler _date.Load, New EventHandler(AddressOf Me.frmRetrieveLoad)
                If (_date.ShowDialog = DialogResult.OK) Then
                    Dim str3 As String = "1=1"
                    If (ObjectType.ObjTst(_date.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        str3 += " AND (a.ngay_ct >= " + Sql.ConvertVS2SQLType(_date.txtNgay_ct.Value, "") + ") "
                        str3 += " AND (a.ngay_ct <= " + Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "") + ")"
                    End If
                    If (_date.txtMa_vt.Text <> "") Then
                        str3 += " AND (a.ma_vt like '" + _date.txtMa_vt.Text.Trim.Replace("'", "''") + "%') "
                    End If
                    If (_date.txtMa_lo.Text <> "") Then
                        str3 += " AND (a.ma_lo like '" + _date.txtMa_lo.Text.Trim.Replace("'", "''") + "%') "
                    End If
                    If (_date.txtMa_kho.Text <> "") Then
                        str3 += " AND (a.ma_kho like '" + _date.txtMa_kho.Text.Trim.Replace("'", "''") + "%') "
                    End If
                    Dim strSQLLong As String = str3
                    Dim tcSQL As String = String.Concat(New String() {"EXEC spSearchIRTran4IR '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph74', 'ct74'"})
                    tcSQL += ",'" + Replace(Me.txtMa_kh.Text.Trim, "'", "''") + "'"
                    tcSQL += ",'" + Replace(Me.txtMa_dvcs.Text.Trim, "'", "''") + "'"
                    tcSQL += ",'" + VoucherCode + "'"
                    Dim ds As New DataSet
                    Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
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
                        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns) {}
                        Dim index As Integer = 0
                        Do
                            cols(index) = New DataGridTextBoxColumn
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index < MaxColumns)
                        frmAdd.Top = 0
                        frmAdd.Left = 0
                        frmAdd.Width = Me.Width
                        frmAdd.Height = Me.Height
                        frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("036"))
                        frmAdd.StartPosition = FormStartPosition.CenterParent
                        Dim panel As StatusBarPanel = AddStb(frmAdd)
                        gridformtran2.CaptionVisible = False
                        gridformtran2.ReadOnly = False
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
                        gridformtran.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 60))))
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
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "IRMaster")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index < MaxColumns)
                        cols(2).Alignment = HorizontalAlignment.Right
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "IRDetail")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index < MaxColumns)
                        oVoucher.HideFields(gridformtran)
                        Me.tblRetrieveDetail.AllowDelete = False
                        Me.tblRetrieveDetail.AllowNew = False
                        index = 1
                        Do While (1 <> 0)
                            Try
                                gridformtran2.TableStyles.Item(0).GridColumnStyles.Item(index).ReadOnly = True
                                index += 1
                            Catch exception1 As Exception
                                ProjectData.SetProjectError(exception1)
                                Dim exception As Exception = exception1
                                ProjectData.ClearProjectError()
                                Exit Do
                            End Try
                        Loop
                        Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
                        Dim count As Integer = Me.tblRetrieveMaster.Count
                        expression = Strings.Replace(Strings.Replace(Strings.Replace(expression, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, CompareMethod.Binary), "%n2", "0", 1, -1, CompareMethod.Binary), "%n3", "0", 1, -1, CompareMethod.Binary)
                        panel.Text = expression
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
                        button4.Text = StringType.FromObject(modVoucher.oLan.Item("029"))
                        button4.Width = 100
                        button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Top = button4.Top
                        button2.Left = (button4.Left + 110)
                        button2.Visible = True
                        button2.Text = StringType.FromObject(modVoucher.oLan.Item("030"))
                        button2.Width = 120
                        button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button3.Top = button4.Top
                        button3.Left = (button2.Left + 130)
                        button3.Visible = True
                        button3.Text = StringType.FromObject(modVoucher.oLan.Item("031"))
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
                        Me.tblRetrieveDetail.Sort = "ngay_ct, so_ct, stt_rec, line_nbr"
                        Me.tblRetrieveMaster.RowFilter = "Tag=1"
                        Dim num7 As Integer = (Me.tblRetrieveMaster.Count - 1)
                        index = 0
                        Dim str7 As String = ""
                        Do While (index <= num7)
                            str7 = StringType.FromObject(ObjectType.AddObj(str7, ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(Interaction.IIf((StringType.StrCmp(str7, "", False) = 0), "", " or "), " (stt_rec = '"), Me.tblRetrieveMaster.Item(index).Item("stt_rec")), "')")))
                            index += 1
                        Loop
                        If str7 = "" Then
                            str7 = "1=0"
                        End If
                        Me.tblRetrieveDetail.RowFilter = str7
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
                            With tbl.Rows.Item(index)
                                If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                    .Item("stt_rec") = ""
                                Else
                                    .Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                                End If
                                tbl.Rows.Item(index).AcceptChanges()
                            End With
                            index += 1
                        Loop
                        AppendFrom(modVoucher.tblDetail, tbl)
                        count = modVoucher.tblDetail.Count
                        If flag Then
                            index = (count - 1)
                            Do While (index >= 0)
                                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("ma_vt")), "C") Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                End If
                                index = (index + -1)
                            Loop
                            count = modVoucher.tblDetail.Count - 1
                            For index = 0 To count
                                modVoucher.tblDetail.Item(index).Item("stt_rec0") = Strings.Format(index + 1, "000")
                            Next
                            RetreiveFillHeader(Me, tblRetrieveMaster.Item(0), "ma_kh,ten_kh,so_ct,ngay_ct,ngay_lct,ma_nt,ty_gia,ong_ba,dien_giai,fnode1,so_ct0,ngay_ct0,fcode1,fdate1")
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

        Private Sub RetrieveItemsFromIR_NB2PNK()
            Dim _date As New frmFilterPN
            AddHandler _date.Load, New EventHandler(AddressOf Me.frmRetrieveLoad)
            If (_date.ShowDialog <> DialogResult.OK) Then
                Return
            End If
            Dim str3 As String = "1=1"
            If (ObjectType.ObjTst(_date.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                str3 += " AND (a.ngay_ct >= " + Sql.ConvertVS2SQLType(_date.txtNgay_ct.Value, "") + ") "
                ' str3 += " AND (a.ngay_ct <= " + Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "") + ")"
            End If
            If (_date.txtMa_vt.Text <> "") Then
                str3 += " AND (a.ma_vt like '" + _date.txtMa_vt.Text.Trim.Replace("'", "''") + "%') "
            End If
            If (_date.txtMa_lo.Text <> "") Then
                str3 += " AND (a.ma_lo like '" + _date.txtMa_lo.Text.Trim.Replace("'", "''") + "%') "
            End If
            If (_date.txtMa_kho.Text <> "") Then
                str3 += " AND (a.ma_kho like '" + _date.txtMa_kho.Text.Trim.Replace("'", "''") + "%') "
            End If
            Dim strSQLLong As String = str3
            Dim tcSQL As String = String.Concat(New String() {"EXEC spSearchIRTran4IR '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph74', 'ct74'"})
            tcSQL += ",''"
            tcSQL += ",'KHO'"
            tcSQL += ",'" + VoucherCode + "'"
            Dim ds As New DataSet
            Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
            Me.tblRetrieveMaster = New DataView
            Me.tblRetrieveDetail = New DataView
            If (ds.Tables.Item(0).Rows.Count <= 0) Then
                Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
                Return
            End If
            Me.tblRetrieveMaster.Table = ds.Tables.Item(0)
            Me.tblRetrieveDetail.Table = ds.Tables.Item(1)
            Dim frmAdd As New Form
            Dim gridformtran2 As New gridformtran
            Dim gridformtran As New gridformtran
            Dim tbs As New DataGridTableStyle
            Dim style As New DataGridTableStyle
            Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns) {}
            Dim index As Integer = 0
            Do
                cols(index) = New DataGridTextBoxColumn
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index < MaxColumns)
            frmAdd.Top = 0
            frmAdd.Left = 0
            frmAdd.Width = Me.Width
            frmAdd.Height = Me.Height
            frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("036"))
            frmAdd.StartPosition = FormStartPosition.CenterParent
            Dim panel As StatusBarPanel = AddStb(frmAdd)
            gridformtran2.CaptionVisible = False
            gridformtran2.ReadOnly = False
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
            gridformtran.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 60))))
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
            Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "IRMaster")
            index = 0
            Do
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index < MaxColumns)
            cols(2).Alignment = HorizontalAlignment.Right
            Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "IRDetail")
            index = 0
            Do
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index < MaxColumns)
            oVoucher.HideFields(gridformtran)
            Me.tblRetrieveDetail.AllowDelete = False
            Me.tblRetrieveDetail.AllowNew = False
            index = 1
            Do While (1 <> 0)
                Try
                    gridformtran2.TableStyles.Item(0).GridColumnStyles.Item(index).ReadOnly = True
                    index += 1
                Catch exception1 As Exception
                    ProjectData.SetProjectError(exception1)
                    Dim exception As Exception = exception1
                    ProjectData.ClearProjectError()
                    Exit Do
                End Try
            Loop
            Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
            Dim count As Integer = Me.tblRetrieveMaster.Count
            expression = Strings.Replace(Strings.Replace(Strings.Replace(expression, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, CompareMethod.Binary), "%n2", "0", 1, -1, CompareMethod.Binary), "%n3", "0", 1, -1, CompareMethod.Binary)
            panel.Text = expression
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
            button4.Text = StringType.FromObject(modVoucher.oLan.Item("029"))
            button4.Width = 100
            button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            button2.Top = button4.Top
            button2.Left = (button4.Left + 110)
            button2.Visible = True
            button2.Text = StringType.FromObject(modVoucher.oLan.Item("030"))
            button2.Width = 120
            button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            button3.Top = button4.Top
            button3.Left = (button2.Left + 130)
            button3.Visible = True
            button3.Text = StringType.FromObject(modVoucher.oLan.Item("031"))
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
            Me.tblRetrieveMaster.RowFilter = "Tag=1"
            Dim num7 As Integer = (Me.tblRetrieveMaster.Count - 1)
            index = 0
            Dim str7 As String = "", strSQL As String = ""
            Try
                Do While (index <= num7)
                    strSQL = "EXEC spCreatePNKFromPND '" + Me.tblRetrieveMaster.Item(index).Item("stt_rec") + "'," + Reg.GetRegistryKey("CurrUserID").ToString.Trim
                    Sql.SQLExecute(appConn, strSQL)
                    index += 1
                Loop
                Msg.Alert("Đã lấy " + index.ToString.Trim + " phiếu, bạn vào tìm kiếm để kiểm tra dữ liệu đã lấy về nếu cần")
            Catch ex As Exception
                Msg.Alert("Có lỗi lấy phiếu: " + Me.tblRetrieveMaster.Item(index).Item("so_ct") + "    " + Me.tblRetrieveMaster.Item(index).Item("ngay_ct").ToString() + "(" + Me.tblRetrieveMaster.Item(index).Item("stt_rec") + ")")
            End Try
            Me.tblRetrieveMaster = Nothing
            Me.tblRetrieveDetail = Nothing
        End Sub

        Private Sub RetreiveFillHeader(_form As Form, drv As DataRowView, fields As String)
            Dim control As Control, fieldname As String
            For Each control In _form.Controls
                Try
                    If control.Name.Length > 3 And control.Tag.ToString.Substring(0, 1) = "F" Then
                        fieldname = control.Name.Substring(3).ToLower
                        If Fox.InList(fieldname, fields.ToLower.Split(",")) Then
                            If control.GetType Is GetType(TextBox) Then
                                DirectCast(control, TextBox).Text = drv.Item(fieldname)
                            ElseIf control.GetType Is GetType(txtNumeric) Then
                                DirectCast(control, txtNumeric).Value = drv.Item(fieldname)
                            ElseIf control.GetType Is GetType(txtDate) Then
                                DirectCast(control, txtDate).Value = drv.Item(fieldname)
                            ElseIf control.GetType Is GetType(Label) Then
                                DirectCast(control, Label).Text = drv.Item(fieldname)
                            End If
                        End If
                    End If
                Catch ex As Exception
                End Try
            Next
        End Sub
        Private Sub grdRetrieveMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
            Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num).Item("stt_rec")), "'")
            Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
        End Sub
        Private Sub RetrieveItems(ByVal sender As Object, ByVal e As EventArgs)
            Dim cancel As Boolean = Me.oInvItemDetail.Cancel
            Me.oInvItemDetail.Cancel = True
            Select Case IntegerType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing))
                Case 0
                    Me.RetrieveItemsFromIR_NB()
                    Exit Select
                Case 2
                    'Me.RetrieveItemsFromPO()
                    Me.RetrieveItemsFromPD()
                    Exit Select
            End Select
            Me.oInvItemDetail.Cancel = cancel
        End Sub
        Private Sub RetrieveItemsFromPO()
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
                If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                    Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("Z07")), 2)
                Else
                    Dim _date As New frmDate
                    AddHandler _date.Load, New EventHandler(AddressOf Me.frmRetrieveLoad)
                    If (_date.ShowDialog = DialogResult.OK) Then
                        Dim str3 As String = " 1 = 1"
                        If (ObjectType.ObjTst(_date.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                            str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct >= ", Sql.ConvertVS2SQLType(_date.txtNgay_ct.Value, "")), ")")))
                        End If
                        If (ObjectType.ObjTst(Me.txtNgay_lct.Text, Fox.GetEmptyDate, False) <> 0) Then
                            str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct <= ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")), ")")))
                        End If
                        Dim strSQLLong As String = str3
                        str3 = (str3 & " AND a.ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'")
                        Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchPOTran4PD '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph94', 'ct94'"})
                        tcSQL += ",'" + Replace(Me.txtMa_dvcs.Text.Trim, "'", "''") + "'"
                        Dim ds As New DataSet
                        Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
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
                                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
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
                            frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("030"))
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
                            gridformtran.ReadOnly = False
                            gridformtran.Top = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                            gridformtran.Left = 0
                            gridformtran.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 60))))
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
                            Dim grdFill As DataGrid = gridformtran2
                            Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), grdFill, (tbs), (cols), "POMaster")
                            gridformtran2 = grdFill
                            index = 0
                            Do
                                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                    cols(index).NullText = StringType.FromInteger(0)
                                Else
                                    cols(index).NullText = ""
                                End If
                                index += 1
                            Loop While (index <= &H1D)
                            cols(2).Alignment = HorizontalAlignment.Right
                            grdFill = gridformtran
                            Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), grdFill, (style), (cols), "PODetail4PD")
                            gridformtran = grdFill
                            index = 0
                            Do
                                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
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
                            Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
                            Dim zero As Decimal = Decimal.Zero
                            Dim num4 As Decimal = Decimal.Zero
                            Dim count As Integer = Me.tblRetrieveMaster.Count
                            Dim num9 As Integer = (count - 1)
                            index = 0
                            Do While (index <= num9)
                                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien"))) Then
                                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, Me.tblRetrieveMaster.Item(index).Item("t_tien")))
                                End If
                                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien_nt"))) Then
                                    num4 = DecimalType.FromObject(ObjectType.AddObj(num4, Me.tblRetrieveMaster.Item(index).Item("t_tien_nt")))
                                End If
                                index += 1
                            Loop
                            expression = Strings.Replace(Strings.Replace(Strings.Replace(expression, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, CompareMethod.Binary), "%n2", Strings.Trim(Strings.Format(num4, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))), 1, -1, CompareMethod.Binary), "%n3", Strings.Trim(Strings.Format(zero, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))), 1, -1, CompareMethod.Binary)
                            panel.Text = expression
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
                            button4.Text = StringType.FromObject(modVoucher.oLan.Item("029"))
                            button4.Width = 100
                            button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                            button2.Top = button4.Top
                            button2.Left = (button4.Left + 110)
                            button2.Visible = True
                            button2.Text = StringType.FromObject(modVoucher.oLan.Item("030"))
                            button2.Width = 120
                            button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                            button3.Top = button4.Top
                            button3.Left = (button2.Left + 130)
                            button3.Visible = True
                            button3.Text = StringType.FromObject(modVoucher.oLan.Item("031"))
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
                            Dim num8 As Integer = (Me.tblRetrieveDetail.Count - 1)
                            index = 0
                            Do While (index <= num8)
                                With Me.tblRetrieveDetail.Item(index)
                                    '.Item("stt_rec_pn") = RuntimeHelpers.GetObjectValue(.Item("stt_rec"))
                                    '.Item("stt_rec0pn") = RuntimeHelpers.GetObjectValue(.Item("stt_rec0"))
                                    .Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("so_luong0"))
                                    .Row.AcceptChanges()
                                End With
                                index += 1
                            Loop
                            Me.tblRetrieveDetail.RowFilter = "so_luong0 <> 0"
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
                            Dim num7 As Integer = (tbl.Rows.Count - 1)
                            index = 0
                            Do While (index <= num7)
                                With tbl.Rows.Item(index)
                                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                        .Item("stt_rec") = ""
                                    Else
                                        .Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                                    End If
                                    .Item("sl_hd") = 0
                                    .Item("sl_dh") = 0
                                    tbl.Rows.Item(index).AcceptChanges()
                                End With
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
                                Dim num6 As Integer = (modVoucher.tblDetail.Count - 1)
                                index = 0
                                Do While (index <= num6)
                                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt"))) Then
                                        tblDetail.Item(index).Item("tien_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("so_luong"), tblDetail.Item(index).Item("gia_nt")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                                    End If
                                    'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_suat_nk"))) Then
                                    '    tblDetail.Item(index).Item("nk_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(ObjectType.MulObj(tblDetail.Item(index).Item("tien_nt"), tblDetail.Item(index).Item("thue_suat_nk")), 100), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                                    'End If
                                    'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_suat"))) Then
                                    '    tblDetail.Item(index).Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(tblDetail.Item(index).Item("tien_nt"), tblDetail.Item(index).Item("nk_nt")), tblDetail.Item(index).Item("thue_suat")), 100), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                                    'End If
                                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("tien_nt"))) Then
                                        tblDetail.Item(index).Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("tien_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                                    End If
                                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt"))) Then
                                        tblDetail.Item(index).Item("gia") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("gia_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                                    End If
                                    'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("nk_nt"))) Then
                                    '    tblDetail.Item(index).Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("nk_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                                    'End If
                                    'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_nt"))) Then
                                    '    tblDetail.Item(index).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("thue_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                                    'End If
                                    index += 1
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
            End If
        End Sub
        Private Sub RetrieveItemsFromPD()
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
                Dim _date As New frmDate
                AddHandler _date.Load, New EventHandler(AddressOf Me.frmRetrieveLoad)
                If (_date.ShowDialog = DialogResult.OK) Then
                    Dim str3 As String = " 1 = 1"
                    If (ObjectType.ObjTst(_date.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct >= ", Sql.ConvertVS2SQLType(_date.txtNgay_ct.Value, "")), ")")))
                    End If
                    If (ObjectType.ObjTst(Me.txtNgay_lct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct <= ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")), ")")))
                    End If
                    Dim strSQLLong As String = str3
                    'str3 = (str3 & " AND a.ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'")
                    Dim tcSQL As String = String.Concat(New String() {"EXEC spSearchPD1Tran4PD '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph96', 'ct96'"})
                    tcSQL += ",'" + Replace(Me.txtMa_dvcs.Text.Trim, "'", "''") + "'"
                    Dim ds As New DataSet
                    Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
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
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
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
                        frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("030"))
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
                        gridformtran.ReadOnly = False
                        gridformtran.Top = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                        gridformtran.Left = 0
                        gridformtran.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 60))))
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
                        Dim grdFill As DataGrid = gridformtran2
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), grdFill, (tbs), (cols), "PDMaster")
                        gridformtran2 = grdFill
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H1D)
                        cols(2).Alignment = HorizontalAlignment.Right
                        grdFill = gridformtran
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), grdFill, (style), (cols), "PD1Detail4PD")
                        gridformtran = grdFill
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H1D)
                        Me.tblRetrieveDetail.AllowDelete = False
                        Me.tblRetrieveDetail.AllowNew = False
                        index = 0
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
                        Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
                        Dim zero As Decimal = Decimal.Zero
                        Dim num4 As Decimal = Decimal.Zero
                        Dim count As Integer = Me.tblRetrieveMaster.Count
                        Dim num9 As Integer = (count - 1)
                        index = 0
                        Do While (index <= num9)
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien"))) Then
                                zero = DecimalType.FromObject(ObjectType.AddObj(zero, Me.tblRetrieveMaster.Item(index).Item("t_tien")))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien_nt"))) Then
                                num4 = DecimalType.FromObject(ObjectType.AddObj(num4, Me.tblRetrieveMaster.Item(index).Item("t_tien_nt")))
                            End If
                            index += 1
                        Loop
                        expression = Strings.Replace(Strings.Replace(Strings.Replace(expression, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, CompareMethod.Binary), "%n2", Strings.Trim(Strings.Format(num4, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))), 1, -1, CompareMethod.Binary), "%n3", Strings.Trim(Strings.Format(zero, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))), 1, -1, CompareMethod.Binary)
                        panel.Text = expression
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
                        button4.Text = StringType.FromObject(modVoucher.oLan.Item("029"))
                        button4.Width = 100
                        button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Top = button4.Top
                        button2.Left = (button4.Left + 110)
                        button2.Visible = True
                        button2.Text = StringType.FromObject(modVoucher.oLan.Item("030"))
                        button2.Width = 120
                        button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button3.Top = button4.Top
                        button3.Left = (button2.Left + 130)
                        button3.Visible = True
                        button3.Text = StringType.FromObject(modVoucher.oLan.Item("031"))
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
                        Dim num8 As Integer = (Me.tblRetrieveDetail.Count - 1)
                        index = 0
                        Do While (index <= num8)
                            With Me.tblRetrieveDetail.Item(index)
                                '.Item("stt_rec_pn") = RuntimeHelpers.GetObjectValue(.Item("stt_rec"))
                                '.Item("stt_rec0pn") = RuntimeHelpers.GetObjectValue(.Item("stt_rec0"))
                                '.Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("so_luong0"))
                                .Row.AcceptChanges()
                            End With
                            index += 1
                        Loop
                        Me.tblRetrieveDetail.RowFilter = "tag = 1"
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
                        Dim num7 As Integer = (tbl.Rows.Count - 1)
                        index = 0
                        Do While (index <= num7)
                            With tbl.Rows.Item(index)
                                If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                    .Item("stt_rec") = ""
                                Else
                                    .Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                                End If
                                '.Item("sl_hd") = 0
                                '.Item("sl_dh") = 0
                                tbl.Rows.Item(index).AcceptChanges()
                            End With
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
                            Dim num6 As Integer = (modVoucher.tblDetail.Count - 1)
                            index = 0
                            Do While (index <= num6)
                                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt"))) Then
                                    tblDetail.Item(index).Item("tien_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("so_luong"), tblDetail.Item(index).Item("gia_nt")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                                End If
                                'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_suat_nk"))) Then
                                '    tblDetail.Item(index).Item("nk_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(ObjectType.MulObj(tblDetail.Item(index).Item("tien_nt"), tblDetail.Item(index).Item("thue_suat_nk")), 100), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                                'End If
                                'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_suat"))) Then
                                '    tblDetail.Item(index).Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(tblDetail.Item(index).Item("tien_nt"), tblDetail.Item(index).Item("nk_nt")), tblDetail.Item(index).Item("thue_suat")), 100), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                                'End If
                                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("tien_nt"))) Then
                                    tblDetail.Item(index).Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("tien_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                                End If
                                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt"))) Then
                                    tblDetail.Item(index).Item("gia") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("gia_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                                End If
                                'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("nk_nt"))) Then
                                '    tblDetail.Item(index).Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("nk_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                                'End If
                                'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_nt"))) Then
                                '    tblDetail.Item(index).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("thue_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                                'End If
                                index += 1
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

        Private Sub BeforUpdateIR(ByVal lcIDNumber As String, ByVal lcAction As String)
            Dim tcSQL As String = String.Concat(New String() {"spBeforUpdateIR '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
            Sql.SQLExecute((modVoucher.appConn), tcSQL)
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
        Friend WithEvents lblAction As Label
        Friend WithEvents lblDien_giai As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMa_gd As Label
        Friend WithEvents lblMa_kh As Label
        Friend WithEvents lblNgay_ct As Label
        Friend WithEvents lblNgay_lct As Label
        Friend WithEvents lblOng_ba As Label
        Friend WithEvents lblSo_ct As Label
        Friend WithEvents lblStatus As Label
        Friend WithEvents lblStatusMess As Label
        Friend WithEvents lblTen As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTen_gd As Label
        Friend WithEvents lblTen_kh As Label
        Friend WithEvents lblTien_hang As Label
        Friend WithEvents lblTy_gia As Label
        Friend WithEvents tbDetail As TabControl
        Friend WithEvents tpgDetail As TabPage
        Friend WithEvents tpgOther As TabPage
        Friend WithEvents txtDien_giai As TextBox
        Friend WithEvents txtKeyPress As TextBox
        Friend WithEvents txtLoai_ct As TextBox
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtMa_gd As TextBox
        Friend WithEvents txtMa_kh As TextBox
        Friend WithEvents txtNgay_ct As txtDate
        Friend WithEvents txtNgay_lct As txtDate
        Friend WithEvents txtOng_ba As TextBox
        Friend WithEvents txtSo_ct As TextBox
        Friend WithEvents txtStatus As TextBox
        Friend WithEvents txtT_so_luong As txtNumeric
        Friend WithEvents txtT_tien As txtNumeric
        Friend WithEvents txtT_tien_nt As txtNumeric
        Friend WithEvents txtTy_gia As txtNumeric

        Public arrControlButtons As Button()
        Public cIDNumber As String
        Public cOldIDNumber As String
        Private cOldItem As String
        Private cOldResonCode As String
        Private cOldSite As String
        Private cOldLot As String
        Private colDvt As DataGridTextBoxColumn
        Private colSo_lsx As DataGridTextBoxColumn
        Private colGia As DataGridTextBoxColumn
        Private colGia_nt As DataGridTextBoxColumn
        Private colMa_kho As DataGridTextBoxColumn
        Private colMa_lo As DataGridTextBoxColumn
        Private colMa_nx As DataGridTextBoxColumn
        Private colMa_vi_tri As DataGridTextBoxColumn
        Private colMa_vt As DataGridTextBoxColumn
        Private colSo_luong As DataGridTextBoxColumn
        Private colTien As DataGridTextBoxColumn
        Private colTien_nt As DataGridTextBoxColumn
        Private colTk_du As DataGridTextBoxColumn
        Private colTk_vt, colMa_nsx As DataGridTextBoxColumn
        Private components As IContainer
        Private grdHeader As grdHeader
        Public iDetailRow As Integer
        Public iMasterRow As Integer
        Public iOldMasterRow As Integer
        Private iOldRow As Integer
        Private isActive As Boolean
        Private lAllowCurrentCellChanged As Boolean
        Private nColumnControl As Integer
        Private noldGia As Decimal
        Private noldGia_nt As Decimal
        Private noldSo_luong As Decimal
        Private noldTien As Decimal
        Private noldTien_nt As Decimal
        Private oBrowIssueLookup As clsbrowse
        Private oInvItemDetail As VoucherLibObj
        Private oldtblDetail As DataTable
        Private oLocation As VoucherKeyLibObj
        Private oLot As VoucherKeyLibObj
        Private oSecurity As clssecurity
        Private oSite As VoucherKeyLibObj
        'Private oTitleButton As TitleButton
        Private oUOM As VoucherKeyCheckLibObj
        Private oMO As VoucherKeyLibObj
        Public oVoucher As clsvoucher.clsVoucher
        Public pnContent As StatusBarPanel
        Private strInIDNumber As String
        Private strInLineIDNumber As String
        Private tblHandling As DataTable
        Private tblStatus As DataTable
        Private xInventory As clsInventory
        Private tblRetrieveDetail As DataView
        Private tblRetrieveMaster As DataView
    End Class
End Namespace

