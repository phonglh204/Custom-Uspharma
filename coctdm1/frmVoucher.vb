Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports System.Data.SqlClient
Imports libscommon
Imports libscontrol.clsvoucher.clsVoucher
Imports libscontrol

Namespace coctdm1
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
            Me.strOldCode = ""
            Dim obj2 As Object = "stt_rec is null or stt_rec = ''"
            modVoucher.tblDetail.AddNew()
            modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
            Me.pnContent.Text = ""
            ScatterMemvarBlankWithDefault(Me)
            If (StringType.StrCmp(Strings.Trim(Me.cmdMa_nt.Text), "", False) = 0) Then
                Me.cmdMa_nt.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("ma_nt"))
            End If
            Me.txtTy_gia.Value = 1
            Me.txtStatus.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_status"))
            Me.EDFC()
            Me.cOldIDNumber = Me.cIDNumber
            Me.iOldMasterRow = Me.iMasterRow
            Me.UpdateList()
            Me.ShowTabDetail()
            Me.txtMa_sp.Focus()
            Me.EDTBColumns()
            Me.EDStatus()
            xtabControl.ReadOnlyTabControls(False, Me.tbDetail)
            xtabControl.ScatterMemvarBlankTabControl(Me.tbDetail)
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
                    xtabControl.ScatterTabControl(modVoucher.tblMaster.Item(Me.iOldMasterRow), Me.tbDetail)
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
            xtabControl.ReadOnlyTabControls(True, Me.tbDetail)
        End Sub

        Public Sub Delete()
            Dim num As Integer
            Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
            Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
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
            Sql.SQLExecute((modVoucher.appConn), tcSQL)
            Me.pnContent.Text = ""
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

        Private Sub DisabledButtons()
            Me.cmdPrint.Visible = False
            Dim button As New Button
            button.Top = Me.cmdPrint.Top
            button.Left = Me.cmdPrint.Left
            button.Height = Me.cmdPrint.Height
            button.Width = Me.cmdPrint.Width
            button.Anchor = Me.cmdPrint.Anchor
            button.Text = Me.cmdPrint.Text
            button.Visible = True
            button.Enabled = False
            Me.Controls.Add(button)
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Public Sub EDFC()
        End Sub

        Public Sub Edit()
            Me.oldtblDetail = Copy2Table(modVoucher.tblDetail)
            Me.iOldMasterRow = Me.iMasterRow
            oVoucher.rOldMaster = modVoucher.tblMaster.Item(Me.iMasterRow)
            Me.ShowTabDetail()
            Me.txtMa_sp.Focus()
            Me.EDTBColumns()
            Me.EDStatus()
            xtabControl.ReadOnlyTabControls(False, Me.tbDetail)
            Me.strOldCode = Replace(Me.txtMa_sp.Text.Trim + "," + Sql.ConvertVS2SQLType(Me.txtFdate1.Value, "") + Me.txtFcode1.Text.Trim, "'", "")
        End Sub

        Private Sub EDStatus()
            Me.RefreshControlField()
        End Sub

        Private Sub EDStatus(ByVal lED As Boolean)
        End Sub

        Private Sub EDTBColumns()
            Dim index As Integer = 0
            Do
                modVoucher.tbcDetail(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
                index += 1
            Loop While (index <= &H1D)
            Try
                Me.colTen_vt.TextBox.Enabled = False
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
        End Sub

        Private Sub EDTBColumns(ByVal lED As Boolean)
            Dim index As Integer = 0
            Do
                modVoucher.tbcDetail(index).TextBox.Enabled = lED
                index += 1
            Loop While (index <= &H1D)
            Try
                Me.colTen_vt.TextBox.Enabled = False
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
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
            oVoucher.txtVDate = New txtDate
            oVoucher.lblStatus = New Label
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
            If (StringType.StrCmp(modVoucher.cLan, "V", False) = 0) Then
                Me.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("ten_ct"))
            Else
                Me.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("ten_ct2"))
            End If
            Sys.InitMessage(modVoucher.sysConn, oVoucher.oClassMsg, "SysClass")
            Try
                oVoucher.Init()
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Msg.Alert(exception.Message)
                ProjectData.ClearProjectError()
            End Try
            Dim lib2 As New DirLib(Me.txtMa_sp, Me.lblTen_sp, modVoucher.sysConn, modVoucher.appConn, "vdmsp2", "ma_vt", "ten_vt", "Item", "1=1", False, Me.cmdEdit)
            Dim lib3 As New CharLib(Me.txtStatus, "0, 1")
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
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), (grdDetail), (modVoucher.tbsDetail), (modVoucher.tbcDetail), "BMDetail")
            oVoucher.SetMaxlengthItem(Me.grdDetail, modVoucher.alDetail, modVoucher.sysConn)
            modVoucher.tblDetail.Table.Columns.Item("bu_am_yn").DefaultValue = False
            modVoucher.tblDetail.Table.Columns.Item("nvlc").DefaultValue = False
            modVoucher.tblDetail.Table.Columns.Item("dc_tl").DefaultValue = False
            modVoucher.tblDetail.Table.Columns.Item("sd_td_yn").DefaultValue = False
            modVoucher.tblDetail.Table.Columns.Item("ma_tc1").DefaultValue = ""
            Me.grdDetail.dvGrid = modVoucher.tblDetail
            Me.grdDetail.cFieldKey = "Ma_vt"
            Me.grdDetail.AllowSorting = False
            Me.grdDetail.TableStyles.Item(0).AllowSorting = False
            Me.colMa_vt = GetColumn(Me.grdDetail, "ma_vt")
            Me.colDvt = GetColumn(Me.grdDetail, "dvt")
            Me.colTen_vt = GetColumn(Me.grdDetail, "ten_vt")
            Me.colSo_luong = GetColumn(Me.grdDetail, "so_luong")
            Me.colMa_ct = GetColumn(Me.grdDetail, "ma_tc1")
            Me.colS1 = GetColumn(Me.grdDetail, "s1")
            Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keyaccount", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            Dim str2 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keycust", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            Me.oUOM = New VoucherKeyCheckLibObj(Me.colDvt, "ten_dvt", modVoucher.sysConn, modVoucher.appConn, "vdmvtqddvt", "dvt", "ten_dvt", "UOMItem", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            Me.oUOM.Cancel = True
            Me.colDvt.TextBox.CharacterCasing = CharacterCasing.Normal
            AddHandler Me.colDvt.TextBox.Move, New EventHandler(AddressOf Me.WhenUOMEnter)
            AddHandler Me.colDvt.TextBox.Validated, New EventHandler(AddressOf Me.WhenUOMLeave)
            Dim oUnits As New DirLib(Me.txtMa_bp, Me.lblTen_bp, modVoucher.sysConn, modVoucher.appConn, "vxdmbp", "ma_bp", "ten_bp", "v20CODept", "1=1", True, Me.cmdEdit)
            Dim oFcode1 As New DirLib(Me.txtFcode1, Me.lblTen_fcode1, modVoucher.sysConn, modVoucher.appConn, "z21dmmayphache", "ma", "ten", "z21dmmayphache", "1=1", False, Me.cmdEdit)
            Me.oInvItemDetail = New VoucherLibObj(Me.colMa_vt, "ten_vt", modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            Me.oMa_ct = New VoucherLibObj(Me.colMa_ct, "ten_td3", modVoucher.sysConn, modVoucher.appConn, "zpcodmct", "ma_ct", "ten_ct", "zpcodmct", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            VoucherLibObj.oClassMsg = oVoucher.oClassMsg
            Me.oInvItemDetail.Colkey = True
            VoucherLibObj.dvDetail = modVoucher.tblDetail
            Dim oS1 As New VoucherLibObj(Me.colS1, "ten_td1", modVoucher.sysConn, modVoucher.appConn, "z21general_lookup", "code", "name", "z21general_lookup", "code0='dmnvl'", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            AddHandler Me.colMa_vt.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKey)
            AddHandler Me.colMa_vt.TextBox.Validated, New EventHandler(AddressOf Me.WhenItemLeave)
            Try
                oVoucher.AddValidFields(Me.grdDetail, modVoucher.tblDetail, Me.pnContent, Me.cmdEdit)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Me.colTen_vt.TextBox.Enabled = False
            oVoucher.HideFields(Me.grdDetail)
            ChangeFormatColumn(Me.colSo_luong, StringType.FromObject(modVoucher.oVar.Item("m_ip_sl")))
            AddHandler Me.colSo_luong.TextBox.Leave, New EventHandler(AddressOf Me.txtSo_luong_valid)
            Dim objectValue As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldchar", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
            Dim obj4 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
            Dim obj3 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fielddate", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
            Dim index As Integer = 0
            Do
                Dim args As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj4)}
                Dim copyBack As Boolean() = New Boolean() {True}
                If copyBack(0) Then
                    obj4 = RuntimeHelpers.GetObjectValue(args(0))
                End If
                If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", args, Nothing, copyBack)), modVoucher.tbcDetail(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                    modVoucher.tbcDetail(index).NullText = "0"
                Else
                    Dim objArray2 As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj3)}
                    copyBack = New Boolean() {True}
                    If copyBack(0) Then
                        obj3 = RuntimeHelpers.GetObjectValue(objArray2(0))
                    End If
                    If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", objArray2, Nothing, copyBack)), modVoucher.tbcDetail(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
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
            menu.MenuItems.Add(item2)
            Me.txtKeyPress.Left = (-100 - Me.txtKeyPress.Width)
            Me.grdDetail.ContextMenu = menu
            ScatterMemvarBlank(Me)
            oVoucher.cAction = "Start"
            Me.isActive = False
            Me.EDTBColumns()
            Me.txtKl_dv.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtKl_dv.Format))
            Me.UnCheckLockedDate()
            'Me.DisabledButtons()
            xtabControl.ScatterMemvarBlankTabControl(Me.tbDetail)
            xtabControl.ReadOnlyTabControls(True, Me.tbDetail)
            xtabControl.SendTabKeys(Me.tbDetail)
            xtabControl.SetMaxlength(Me.tbDetail, modVoucher.alMaster, modVoucher.sysConn)
            'Me.InitInventory()
            Dim _text As String = Me.txtDvt.Text
            Me.oUOMx = New dirblanklib(Me.txtDvt, New Label, modVoucher.sysConn, modVoucher.appConn, "vdmvtqddvt", "dvt", "ten_dvt", "UOMItem", "1=1", False, Me.cmdEdit)
            AddHandler Me.txtDvt.Enter, New EventHandler(AddressOf Me.txtMa_sp_LostFocus)
            Me.txtDvt.CharacterCasing = CharacterCasing.Normal
            Me.txtDvt.Text = _text
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
            Dim oOldObject As Object
            If sLeft = "SO_LUONG" Then
                oOldObject = Me.noldSo_luong
                SetOldValue((oOldObject), oValue)
                Me.noldSo_luong = DecimalType.FromObject(oOldObject)
            End If
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
        Friend WithEvents lblNgay_lct As Label
        Friend WithEvents txtFdate1 As txtDate
        Friend WithEvents txtSo_luong As txtNumeric
        Friend WithEvents lblSo_luong As Label
        Friend WithEvents Label1 As Label
        Friend WithEvents Label4 As Label
        Friend WithEvents Label8 As Label
        Friend WithEvents txtKl_dv As txtNumeric
        Friend WithEvents Label5 As Label
        Friend WithEvents txtIssued_times As txtNumeric
        Friend WithEvents lblDien_giai As Label
        Friend WithEvents txtPacking_format As TextBox
        Friend WithEvents txtDang_bao_che As TextBox
        Friend WithEvents Label9 As Label
        Friend WithEvents tpgOther As System.Windows.Forms.TabPage
        Friend WithEvents Label10 As Label
        Friend WithEvents txtLy_do As TextBox
        Friend WithEvents lblSo_ct As Label
        Friend WithEvents txtSo_ct As TextBox
        Friend WithEvents txtFcode1 As TextBox
        Friend WithEvents Label11 As Label
        Friend WithEvents lblTen_fcode1 As Label
        Friend WithEvents txtFNote1 As TextBox
        Friend WithEvents txtS4 As txtNumeric

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
            Me.lblDvt = New System.Windows.Forms.Label()
            Me.txtDvt = New System.Windows.Forms.TextBox()
            Me.txtTy_gia = New libscontrol.txtNumeric()
            Me.cmdMa_nt = New System.Windows.Forms.Button()
            Me.tbDetail = New System.Windows.Forms.TabControl()
            Me.tpgDetail = New System.Windows.Forms.TabPage()
            Me.grdDetail = New libscontrol.clsgrid()
            Me.tpgOther = New System.Windows.Forms.TabPage()
            Me.txtLy_do = New System.Windows.Forms.TextBox()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.txtPacking_format = New System.Windows.Forms.TextBox()
            Me.lblDien_giai = New System.Windows.Forms.Label()
            Me.txtDang_bao_che = New System.Windows.Forms.TextBox()
            Me.Label9 = New System.Windows.Forms.Label()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.txtKl_dv = New libscontrol.txtNumeric()
            Me.txtStatus = New System.Windows.Forms.TextBox()
            Me.lblStatusMess = New System.Windows.Forms.Label()
            Me.txtKeyPress = New System.Windows.Forms.TextBox()
            Me.lblMa_bp = New System.Windows.Forms.Label()
            Me.txtMa_bp = New System.Windows.Forms.TextBox()
            Me.lblTen_bp = New System.Windows.Forms.Label()
            Me.lblTien_hang = New System.Windows.Forms.Label()
            Me.txtT_so_luong = New libscontrol.txtNumeric()
            Me.txtMa_sp = New System.Windows.Forms.TextBox()
            Me.lblMa_sp = New System.Windows.Forms.Label()
            Me.lblTen_sp = New System.Windows.Forms.Label()
            Me.txtHe_so = New libscontrol.txtNumeric()
            Me.lblNgay_lct = New System.Windows.Forms.Label()
            Me.txtFdate1 = New libscontrol.txtDate()
            Me.lblSo_luong = New System.Windows.Forms.Label()
            Me.txtSo_luong = New libscontrol.txtNumeric()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.txtIssued_times = New libscontrol.txtNumeric()
            Me.lblSo_ct = New System.Windows.Forms.Label()
            Me.txtSo_ct = New System.Windows.Forms.TextBox()
            Me.txtFcode1 = New System.Windows.Forms.TextBox()
            Me.Label11 = New System.Windows.Forms.Label()
            Me.lblTen_fcode1 = New System.Windows.Forms.Label()
            Me.txtFNote1 = New System.Windows.Forms.TextBox()
            Me.txtS4 = New libscontrol.txtNumeric()
            Me.tbDetail.SuspendLayout()
            Me.tpgDetail.SuspendLayout()
            CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpgOther.SuspendLayout()
            Me.SuspendLayout()
            '
            'cmdSave
            '
            Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
            Me.cmdSave.Location = New System.Drawing.Point(2, 489)
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
            Me.cmdNew.Location = New System.Drawing.Point(74, 489)
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
            Me.cmdPrint.Location = New System.Drawing.Point(146, 489)
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
            Me.cmdEdit.Location = New System.Drawing.Point(218, 489)
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
            Me.cmdDelete.Location = New System.Drawing.Point(290, 489)
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
            Me.cmdView.Location = New System.Drawing.Point(362, 489)
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
            Me.cmdSearch.Location = New System.Drawing.Point(434, 489)
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
            Me.cmdClose.Location = New System.Drawing.Point(506, 489)
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
            Me.cmdOption.Location = New System.Drawing.Point(938, 489)
            Me.cmdOption.Name = "cmdOption"
            Me.cmdOption.Size = New System.Drawing.Size(24, 26)
            Me.cmdOption.TabIndex = 24
            Me.cmdOption.TabStop = False
            Me.cmdOption.Tag = "CB09"
            Me.cmdOption.UseVisualStyleBackColor = False
            Me.cmdOption.Visible = False
            '
            'cmdTop
            '
            Me.cmdTop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdTop.BackColor = System.Drawing.SystemColors.Control
            Me.cmdTop.Location = New System.Drawing.Point(960, 489)
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
            Me.cmdPrev.Location = New System.Drawing.Point(983, 489)
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
            Me.cmdNext.Location = New System.Drawing.Point(1006, 489)
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
            Me.cmdBottom.Location = New System.Drawing.Point(1029, 489)
            Me.cmdBottom.Name = "cmdBottom"
            Me.cmdBottom.Size = New System.Drawing.Size(24, 26)
            Me.cmdBottom.TabIndex = 28
            Me.cmdBottom.TabStop = False
            Me.cmdBottom.Tag = "CB13"
            Me.cmdBottom.UseVisualStyleBackColor = False
            '
            'lblDvt
            '
            Me.lblDvt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblDvt.AutoSize = True
            Me.lblDvt.Location = New System.Drawing.Point(824, 8)
            Me.lblDvt.Name = "lblDvt"
            Me.lblDvt.Size = New System.Drawing.Size(29, 17)
            Me.lblDvt.TabIndex = 16
            Me.lblDvt.Tag = "L003"
            Me.lblDvt.Text = "Dvt"
            '
            'txtDvt
            '
            Me.txtDvt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtDvt.BackColor = System.Drawing.Color.White
            Me.txtDvt.Location = New System.Drawing.Point(920, 6)
            Me.txtDvt.Name = "txtDvt"
            Me.txtDvt.Size = New System.Drawing.Size(120, 22)
            Me.txtDvt.TabIndex = 7
            Me.txtDvt.Tag = "FCNBCF"
            Me.txtDvt.Text = "txtDvt"
            '
            'txtTy_gia
            '
            Me.txtTy_gia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtTy_gia.BackColor = System.Drawing.Color.White
            Me.txtTy_gia.Format = "m_ip_tg"
            Me.txtTy_gia.Location = New System.Drawing.Point(1013, 526)
            Me.txtTy_gia.MaxLength = 8
            Me.txtTy_gia.Name = "txtTy_gia"
            Me.txtTy_gia.Size = New System.Drawing.Size(39, 22)
            Me.txtTy_gia.TabIndex = 9
            Me.txtTy_gia.Tag = "FNCF"
            Me.txtTy_gia.Text = "m_ip_tg"
            Me.txtTy_gia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtTy_gia.Value = 0R
            Me.txtTy_gia.Visible = False
            '
            'cmdMa_nt
            '
            Me.cmdMa_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdMa_nt.BackColor = System.Drawing.SystemColors.Control
            Me.cmdMa_nt.Enabled = False
            Me.cmdMa_nt.Location = New System.Drawing.Point(1008, 526)
            Me.cmdMa_nt.Name = "cmdMa_nt"
            Me.cmdMa_nt.Size = New System.Drawing.Size(44, 23)
            Me.cmdMa_nt.TabIndex = 8
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
            Me.tbDetail.Controls.Add(Me.tpgOther)
            Me.tbDetail.Location = New System.Drawing.Point(2, 166)
            Me.tbDetail.Name = "tbDetail"
            Me.tbDetail.SelectedIndex = 0
            Me.tbDetail.Size = New System.Drawing.Size(1052, 281)
            Me.tbDetail.TabIndex = 11
            '
            'tpgDetail
            '
            Me.tpgDetail.BackColor = System.Drawing.SystemColors.Control
            Me.tpgDetail.Controls.Add(Me.grdDetail)
            Me.tpgDetail.Location = New System.Drawing.Point(4, 25)
            Me.tpgDetail.Name = "tpgDetail"
            Me.tpgDetail.Size = New System.Drawing.Size(1044, 252)
            Me.tpgDetail.TabIndex = 0
            Me.tpgDetail.Tag = "L005"
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
            Me.grdDetail.Size = New System.Drawing.Size(1045, 249)
            Me.grdDetail.TabIndex = 0
            Me.grdDetail.Tag = "L006CF"
            '
            'tpgOther
            '
            Me.tpgOther.Controls.Add(Me.txtLy_do)
            Me.tpgOther.Controls.Add(Me.Label10)
            Me.tpgOther.Controls.Add(Me.txtPacking_format)
            Me.tpgOther.Controls.Add(Me.lblDien_giai)
            Me.tpgOther.Controls.Add(Me.txtDang_bao_che)
            Me.tpgOther.Controls.Add(Me.Label9)
            Me.tpgOther.Controls.Add(Me.Label8)
            Me.tpgOther.Controls.Add(Me.txtKl_dv)
            Me.tpgOther.Location = New System.Drawing.Point(4, 25)
            Me.tpgOther.Name = "tpgOther"
            Me.tpgOther.Size = New System.Drawing.Size(758, 257)
            Me.tpgOther.TabIndex = 1
            Me.tpgOther.Tag = "L022"
            Me.tpgOther.Text = "tpgOther"
            '
            'txtLy_do
            '
            Me.txtLy_do.BackColor = System.Drawing.Color.White
            Me.txtLy_do.Location = New System.Drawing.Point(125, 9)
            Me.txtLy_do.Name = "txtLy_do"
            Me.txtLy_do.Size = New System.Drawing.Size(614, 22)
            Me.txtLy_do.TabIndex = 0
            Me.txtLy_do.Tag = "FCCF"
            Me.txtLy_do.Text = "txtLy_do"
            '
            'Label10
            '
            Me.Label10.AutoSize = True
            Me.Label10.Location = New System.Drawing.Point(19, 12)
            Me.Label10.Name = "Label10"
            Me.Label10.Size = New System.Drawing.Size(97, 17)
            Me.Label10.TabIndex = 154
            Me.Label10.Tag = "L021"
            Me.Label10.Text = "Ly do thay doi"
            '
            'txtPacking_format
            '
            Me.txtPacking_format.BackColor = System.Drawing.Color.White
            Me.txtPacking_format.Location = New System.Drawing.Point(192, 33)
            Me.txtPacking_format.Name = "txtPacking_format"
            Me.txtPacking_format.Size = New System.Drawing.Size(240, 22)
            Me.txtPacking_format.TabIndex = 1
            Me.txtPacking_format.Tag = "FCCF"
            Me.txtPacking_format.Text = "txtPacking_format"
            '
            'lblDien_giai
            '
            Me.lblDien_giai.AutoSize = True
            Me.lblDien_giai.Location = New System.Drawing.Point(19, 36)
            Me.lblDien_giai.Name = "lblDien_giai"
            Me.lblDien_giai.Size = New System.Drawing.Size(68, 17)
            Me.lblDien_giai.TabIndex = 150
            Me.lblDien_giai.Tag = "L019"
            Me.lblDien_giai.Text = "Quy cach"
            '
            'txtDang_bao_che
            '
            Me.txtDang_bao_che.BackColor = System.Drawing.Color.White
            Me.txtDang_bao_che.Location = New System.Drawing.Point(192, 58)
            Me.txtDang_bao_che.Name = "txtDang_bao_che"
            Me.txtDang_bao_che.Size = New System.Drawing.Size(240, 22)
            Me.txtDang_bao_che.TabIndex = 2
            Me.txtDang_bao_che.Tag = "FCCF"
            Me.txtDang_bao_che.Text = "txtDang_bao_che"
            '
            'Label9
            '
            Me.Label9.AutoSize = True
            Me.Label9.Location = New System.Drawing.Point(19, 60)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(97, 17)
            Me.Label9.TabIndex = 152
            Me.Label9.Tag = "L020"
            Me.Label9.Text = "Dang bao che"
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(19, 84)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(179, 17)
            Me.Label8.TabIndex = 146
            Me.Label8.Tag = "L017"
            Me.Label8.Text = "Trong luong vien (MiliGam)"
            '
            'txtKl_dv
            '
            Me.txtKl_dv.BackColor = System.Drawing.Color.White
            Me.txtKl_dv.Format = "m_ip_sl"
            Me.txtKl_dv.Location = New System.Drawing.Point(192, 83)
            Me.txtKl_dv.MaxLength = 8
            Me.txtKl_dv.Name = "txtKl_dv"
            Me.txtKl_dv.Size = New System.Drawing.Size(120, 22)
            Me.txtKl_dv.TabIndex = 3
            Me.txtKl_dv.Tag = "FNCF"
            Me.txtKl_dv.Text = "m_ip_sl"
            Me.txtKl_dv.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtKl_dv.Value = 0R
            '
            'txtStatus
            '
            Me.txtStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.txtStatus.BackColor = System.Drawing.Color.White
            Me.txtStatus.Location = New System.Drawing.Point(10, 519)
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
            'lblStatusMess
            '
            Me.lblStatusMess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblStatusMess.AutoSize = True
            Me.lblStatusMess.Location = New System.Drawing.Point(58, 521)
            Me.lblStatusMess.Name = "lblStatusMess"
            Me.lblStatusMess.Size = New System.Drawing.Size(253, 17)
            Me.lblStatusMess.TabIndex = 42
            Me.lblStatusMess.Tag = ""
            Me.lblStatusMess.Text = "1 - Ghi vao SC, 0 - Chua ghi vao so cai"
            Me.lblStatusMess.Visible = False
            '
            'txtKeyPress
            '
            Me.txtKeyPress.Location = New System.Drawing.Point(518, 55)
            Me.txtKeyPress.Name = "txtKeyPress"
            Me.txtKeyPress.Size = New System.Drawing.Size(12, 12)
            Me.txtKeyPress.TabIndex = 10
            Me.txtKeyPress.Visible = False
            '
            'lblMa_bp
            '
            Me.lblMa_bp.AutoSize = True
            Me.lblMa_bp.Location = New System.Drawing.Point(10, 32)
            Me.lblMa_bp.Name = "lblMa_bp"
            Me.lblMa_bp.Size = New System.Drawing.Size(61, 17)
            Me.lblMa_bp.TabIndex = 34
            Me.lblMa_bp.Tag = "L002"
            Me.lblMa_bp.Text = "Bo phan"
            '
            'txtMa_bp
            '
            Me.txtMa_bp.BackColor = System.Drawing.Color.White
            Me.txtMa_bp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_bp.Location = New System.Drawing.Point(106, 30)
            Me.txtMa_bp.Name = "txtMa_bp"
            Me.txtMa_bp.Size = New System.Drawing.Size(120, 22)
            Me.txtMa_bp.TabIndex = 1
            Me.txtMa_bp.Tag = "FCCF"
            Me.txtMa_bp.Text = "TXTMA_BP"
            '
            'lblTen_bp
            '
            Me.lblTen_bp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTen_bp.Location = New System.Drawing.Point(230, 33)
            Me.lblTen_bp.Name = "lblTen_bp"
            Me.lblTen_bp.Size = New System.Drawing.Size(566, 18)
            Me.lblTen_bp.TabIndex = 36
            Me.lblTen_bp.Tag = "FCRF"
            Me.lblTen_bp.Text = "Ten bo phan"
            '
            'lblTien_hang
            '
            Me.lblTien_hang.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTien_hang.AutoSize = True
            Me.lblTien_hang.Location = New System.Drawing.Point(812, 460)
            Me.lblTien_hang.Name = "lblTien_hang"
            Me.lblTien_hang.Size = New System.Drawing.Size(76, 17)
            Me.lblTien_hang.TabIndex = 60
            Me.lblTien_hang.Tag = "L004"
            Me.lblTien_hang.Text = "Tong cong"
            '
            'txtT_so_luong
            '
            Me.txtT_so_luong.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtT_so_luong.BackColor = System.Drawing.Color.White
            Me.txtT_so_luong.Enabled = False
            Me.txtT_so_luong.ForeColor = System.Drawing.Color.Black
            Me.txtT_so_luong.Format = "m_ip_sl"
            Me.txtT_so_luong.Location = New System.Drawing.Point(932, 458)
            Me.txtT_so_luong.MaxLength = 8
            Me.txtT_so_luong.Name = "txtT_so_luong"
            Me.txtT_so_luong.Size = New System.Drawing.Size(120, 22)
            Me.txtT_so_luong.TabIndex = 12
            Me.txtT_so_luong.Tag = "FN"
            Me.txtT_so_luong.Text = "m_ip_sl"
            Me.txtT_so_luong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtT_so_luong.Value = 0R
            '
            'txtMa_sp
            '
            Me.txtMa_sp.BackColor = System.Drawing.Color.White
            Me.txtMa_sp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_sp.Location = New System.Drawing.Point(106, 6)
            Me.txtMa_sp.Name = "txtMa_sp"
            Me.txtMa_sp.Size = New System.Drawing.Size(120, 22)
            Me.txtMa_sp.TabIndex = 0
            Me.txtMa_sp.Tag = "FCNBCF"
            Me.txtMa_sp.Text = "TXTMA_SP"
            '
            'lblMa_sp
            '
            Me.lblMa_sp.AutoSize = True
            Me.lblMa_sp.Location = New System.Drawing.Point(10, 8)
            Me.lblMa_sp.Name = "lblMa_sp"
            Me.lblMa_sp.Size = New System.Drawing.Size(72, 17)
            Me.lblMa_sp.TabIndex = 81
            Me.lblMa_sp.Tag = "L001"
            Me.lblMa_sp.Text = "San pham"
            '
            'lblTen_sp
            '
            Me.lblTen_sp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTen_sp.Location = New System.Drawing.Point(230, 9)
            Me.lblTen_sp.Name = "lblTen_sp"
            Me.lblTen_sp.Size = New System.Drawing.Size(566, 18)
            Me.lblTen_sp.TabIndex = 82
            Me.lblTen_sp.Tag = "FCRF"
            Me.lblTen_sp.Text = "Ten san pham"
            '
            'txtHe_so
            '
            Me.txtHe_so.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtHe_so.BackColor = System.Drawing.Color.White
            Me.txtHe_so.Format = "m_ip_tg"
            Me.txtHe_so.Location = New System.Drawing.Point(1004, 526)
            Me.txtHe_so.MaxLength = 8
            Me.txtHe_so.Name = "txtHe_so"
            Me.txtHe_so.Size = New System.Drawing.Size(48, 22)
            Me.txtHe_so.TabIndex = 83
            Me.txtHe_so.Tag = "FNCF"
            Me.txtHe_so.Text = "m_ip_tg"
            Me.txtHe_so.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtHe_so.Value = 0R
            Me.txtHe_so.Visible = False
            '
            'lblNgay_lct
            '
            Me.lblNgay_lct.AutoSize = True
            Me.lblNgay_lct.Location = New System.Drawing.Point(10, 58)
            Me.lblNgay_lct.Name = "lblNgay_lct"
            Me.lblNgay_lct.Size = New System.Drawing.Size(94, 17)
            Me.lblNgay_lct.TabIndex = 85
            Me.lblNgay_lct.Tag = "L108"
            Me.lblNgay_lct.Text = "Ngay hieu luc"
            '
            'txtFdate1
            '
            Me.txtFdate1.BackColor = System.Drawing.Color.White
            Me.txtFdate1.Location = New System.Drawing.Point(106, 55)
            Me.txtFdate1.MaxLength = 10
            Me.txtFdate1.Name = "txtFdate1"
            Me.txtFdate1.Size = New System.Drawing.Size(120, 22)
            Me.txtFdate1.TabIndex = 2
            Me.txtFdate1.Tag = "FDNBCFDF"
            Me.txtFdate1.Text = "  /  /    "
            Me.txtFdate1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtFdate1.Value = New Date(CType(0, Long))
            '
            'lblSo_luong
            '
            Me.lblSo_luong.AutoSize = True
            Me.lblSo_luong.Location = New System.Drawing.Point(240, 58)
            Me.lblSo_luong.Name = "lblSo_luong"
            Me.lblSo_luong.Size = New System.Drawing.Size(40, 17)
            Me.lblSo_luong.TabIndex = 129
            Me.lblSo_luong.Tag = "L109"
            Me.lblSo_luong.Text = "Co lo"
            '
            'txtSo_luong
            '
            Me.txtSo_luong.BackColor = System.Drawing.Color.White
            Me.txtSo_luong.Format = "m_ip_sl"
            Me.txtSo_luong.Location = New System.Drawing.Point(288, 55)
            Me.txtSo_luong.MaxLength = 8
            Me.txtSo_luong.Name = "txtSo_luong"
            Me.txtSo_luong.Size = New System.Drawing.Size(120, 22)
            Me.txtSo_luong.TabIndex = 3
            Me.txtSo_luong.Tag = "FNCF"
            Me.txtSo_luong.Text = "m_ip_sl"
            Me.txtSo_luong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtSo_luong.Value = 0R
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(10, 113)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(68, 17)
            Me.Label1.TabIndex = 131
            Me.Label1.Tag = "LZ03"
            Me.Label1.Text = "han dung"
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(240, 113)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(48, 17)
            Me.Label4.TabIndex = 136
            Me.Label4.Tag = "LZ04"
            Me.Label4.Text = "So me"
            '
            'Label5
            '
            Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(824, 55)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(96, 17)
            Me.Label5.TabIndex = 148
            Me.Label5.Tag = "L018"
            Me.Label5.Text = "Lan ban hanh"
            '
            'txtIssued_times
            '
            Me.txtIssued_times.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtIssued_times.BackColor = System.Drawing.Color.White
            Me.txtIssued_times.Format = "m_ip_tien"
            Me.txtIssued_times.Location = New System.Drawing.Point(920, 54)
            Me.txtIssued_times.MaxLength = 1
            Me.txtIssued_times.Name = "txtIssued_times"
            Me.txtIssued_times.Size = New System.Drawing.Size(120, 22)
            Me.txtIssued_times.TabIndex = 9
            Me.txtIssued_times.Tag = "FNCF"
            Me.txtIssued_times.Text = "m_ip_tien"
            Me.txtIssued_times.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtIssued_times.Value = 0R
            '
            'lblSo_ct
            '
            Me.lblSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblSo_ct.AutoSize = True
            Me.lblSo_ct.Location = New System.Drawing.Point(824, 32)
            Me.lblSo_ct.Name = "lblSo_ct"
            Me.lblSo_ct.Size = New System.Drawing.Size(43, 17)
            Me.lblSo_ct.TabIndex = 150
            Me.lblSo_ct.Tag = "L023"
            Me.lblSo_ct.Text = "So px"
            '
            'txtSo_ct
            '
            Me.txtSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtSo_ct.BackColor = System.Drawing.Color.White
            Me.txtSo_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtSo_ct.Location = New System.Drawing.Point(920, 30)
            Me.txtSo_ct.Name = "txtSo_ct"
            Me.txtSo_ct.Size = New System.Drawing.Size(120, 22)
            Me.txtSo_ct.TabIndex = 8
            Me.txtSo_ct.Tag = "FCNBCF"
            Me.txtSo_ct.Text = "TXTSO_CT"
            Me.txtSo_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'txtFcode1
            '
            Me.txtFcode1.BackColor = System.Drawing.Color.White
            Me.txtFcode1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtFcode1.Location = New System.Drawing.Point(106, 83)
            Me.txtFcode1.Name = "txtFcode1"
            Me.txtFcode1.Size = New System.Drawing.Size(120, 22)
            Me.txtFcode1.TabIndex = 4
            Me.txtFcode1.Tag = "FCCF"
            Me.txtFcode1.Text = "TXTFCODE1"
            '
            'Label11
            '
            Me.Label11.AutoSize = True
            Me.Label11.Location = New System.Drawing.Point(10, 85)
            Me.Label11.Name = "Label11"
            Me.Label11.Size = New System.Drawing.Size(89, 17)
            Me.Label11.TabIndex = 152
            Me.Label11.Tag = "LZ02"
            Me.Label11.Text = "May pha che"
            '
            'lblTen_fcode1
            '
            Me.lblTen_fcode1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTen_fcode1.Location = New System.Drawing.Point(230, 87)
            Me.lblTen_fcode1.Name = "lblTen_fcode1"
            Me.lblTen_fcode1.Size = New System.Drawing.Size(566, 17)
            Me.lblTen_fcode1.TabIndex = 153
            Me.lblTen_fcode1.Tag = "FCRF"
            Me.lblTen_fcode1.Text = "Ten bo phan"
            '
            'txtFNote1
            '
            Me.txtFNote1.BackColor = System.Drawing.Color.White
            Me.txtFNote1.Location = New System.Drawing.Point(106, 111)
            Me.txtFNote1.Name = "txtFNote1"
            Me.txtFNote1.Size = New System.Drawing.Size(120, 22)
            Me.txtFNote1.TabIndex = 5
            Me.txtFNote1.Tag = "FCCF"
            Me.txtFNote1.Text = "TXTFNODE1"
            '
            'txtS4
            '
            Me.txtS4.BackColor = System.Drawing.Color.White
            Me.txtS4.Format = "m_ip_sl"
            Me.txtS4.Location = New System.Drawing.Point(288, 111)
            Me.txtS4.MaxLength = 8
            Me.txtS4.Name = "txtS4"
            Me.txtS4.Size = New System.Drawing.Size(120, 22)
            Me.txtS4.TabIndex = 6
            Me.txtS4.Tag = "FNCF"
            Me.txtS4.Text = "m_ip_sl"
            Me.txtS4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtS4.Value = 0R
            '
            'frmVoucher
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.ClientSize = New System.Drawing.Size(1056, 541)
            Me.Controls.Add(Me.txtS4)
            Me.Controls.Add(Me.txtFNote1)
            Me.Controls.Add(Me.txtFcode1)
            Me.Controls.Add(Me.Label11)
            Me.Controls.Add(Me.lblTen_fcode1)
            Me.Controls.Add(Me.lblSo_ct)
            Me.Controls.Add(Me.txtSo_ct)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.txtIssued_times)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.lblSo_luong)
            Me.Controls.Add(Me.txtSo_luong)
            Me.Controls.Add(Me.lblNgay_lct)
            Me.Controls.Add(Me.txtFdate1)
            Me.Controls.Add(Me.txtT_so_luong)
            Me.Controls.Add(Me.txtHe_so)
            Me.Controls.Add(Me.txtMa_sp)
            Me.Controls.Add(Me.lblMa_sp)
            Me.Controls.Add(Me.lblTien_hang)
            Me.Controls.Add(Me.txtMa_bp)
            Me.Controls.Add(Me.lblMa_bp)
            Me.Controls.Add(Me.txtKeyPress)
            Me.Controls.Add(Me.lblStatusMess)
            Me.Controls.Add(Me.txtTy_gia)
            Me.Controls.Add(Me.lblDvt)
            Me.Controls.Add(Me.txtStatus)
            Me.Controls.Add(Me.txtDvt)
            Me.Controls.Add(Me.lblTen_sp)
            Me.Controls.Add(Me.lblTen_bp)
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
            Me.tpgOther.ResumeLayout(False)
            Me.tpgOther.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private Sub InitInventory()
            Me.xInventory.ColItem = Me.colMa_vt
            Me.xInventory.ColSite = Nothing
            Me.xInventory.ColUOM = Me.colDvt
            Me.xInventory.colQty = Me.colSo_luong
            Me.xInventory.txtUnit = Nothing
            Me.xInventory.InvVoucher = Me.oVoucher
            Me.xInventory.oInvItem = Me.oInvItemDetail
            Me.xInventory.oInvUOM = Me.oUOM
            Me.xInventory.Init()
        End Sub

        Public Sub InitRecords()
            Dim str As String
            If oVoucher.isRead Then
                str = String.Concat(New String() {"EXEC fs_LoadBMTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', -1"})
            Else
                str = String.Concat(New String() {"EXEC fs_LoadBMTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
            End If
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
            End If
        End Sub

        Private Function Post() As String
            Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "groupby", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            Dim str3 As String = "EXEC fs_PostBM "
            Return (StringType.FromObject(ObjectType.AddObj(((((((str3 & "'" & modVoucher.VoucherCode & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))) & "'") & ", '" & Strings.Trim(str) & "'"), ObjectType.AddObj(ObjectType.AddObj(", '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))) & ", 1")
        End Function

        Public Sub Print()
            Dim print As New frmPrint
            print.txtTitle.Text = StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct"))), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct2")))))
            print.txtSo_lien.Value = DoubleType.FromObject(modVoucher.oVoucherRow.Item("so_lien"))
            Dim table As DataTable = clsprint.InitComboReport(modVoucher.sysConn, print.cboReports, "BMTran")
            Dim result As DialogResult = print.ShowDialog
            If ((result <> DialogResult.Cancel) AndAlso (print.txtSo_lien.Value > 0)) Then
                Dim selectedIndex As Integer = print.cboReports.SelectedIndex
                Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
                Dim view As New DataView
                Dim ds As New DataSet
                Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj((("EXEC fs_PrintBMTran '" & modVoucher.cLan) & "', " & "[a.stt_rec = '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'], '"), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf")))), "'"))
                Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "cttmp", (ds))
                'ds.WriteXmlSchema("E:\CustomerLocal\Pharma\Program\Rpt\coctdm1.xsd")
                'Return
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
                clsprint.SetReportVar(modVoucher.sysConn, modVoucher.appConn, "BMTran", modVoucher.oOption, clsprint.oRpt)
                clsprint.oRpt.SetParameterValue("Title", Strings.Trim(print.txtTitle.Text))
                'Dim str2 As String = Strings.Replace(Strings.Replace(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("401")), "%s1", Me.txtNgay_ct.Value.Day.ToString, 1, -1, CompareMethod.Binary), "%s2", Me.txtNgay_ct.Value.Month.ToString, 1, -1, CompareMethod.Binary), "%s3", Me.txtNgay_ct.Value.Year.ToString, 1, -1, CompareMethod.Binary)
                'Dim str4 As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("402")), "%s", Strings.Trim(Me.txtSo_ct.Text), 1, -1, CompareMethod.Binary)
                'Dim str As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("403")), "%s", clsprint.Num2Words(New Decimal(Me.txtT_tien.Value), StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.oOption.Item("m_use_2fc"), "1", False) = 0), RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "SELECT dbo.ff30_FC1()")), RuntimeHelpers.GetObjectValue(modVoucher.oOption.Item("m_ma_nt0"))))), 1, -1, CompareMethod.Binary)
                'clsprint.oRpt.SetParameterValue("s_byword", str)
                'clsprint.oRpt.SetParameterValue("t_date", str2)
                'clsprint.oRpt.SetParameterValue("t_number", str4)
                'clsprint.oRpt.SetParameterValue("nTotal", Me.txtT_tien.Value)
                'clsprint.oRpt.SetParameterValue("f_ong_ba", Strings.Trim(Me.txtOng_ba.Text))
                'clsprint.oRpt.SetParameterValue("f_kh", (Strings.Trim(Me.txtMa_kh.Text) & " - " & Strings.Trim(Me.lblTen_kh.Text)))
                'Dim str3 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "dia_chi", ("ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'"))))
                'clsprint.oRpt.SetParameterValue("f_dia_chi", str3)
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
            ScatterMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
            Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'")
            modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
            Me.UpdateList()
            Me.vCaptionRefresh()
            xtabControl.ScatterTabControl(modVoucher.tblMaster.Item(Me.iMasterRow), Me.tbDetail)
            Me.cmdNew.Focus()
        End Sub

        Private Sub RefreshControlField()
        End Sub

        Public Sub Save()
            Dim num As Integer
            Me.txtStatus.Text = "1"
            If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(Me.txtMa_sp.Text) & "'"))) Then
                Dim cKey As String = String.Concat(New String() {"(ma_vt = '", Strings.Trim(Me.txtMa_sp.Text), "' OR ma_vt = '*') AND dvt = N'", Strings.Trim(Me.txtDvt.Text), "'"})
                Dim num5 As Decimal = DecimalType.FromObject(Sql.GetValue((modVoucher.appConn), "dmqddvt", "he_so", cKey))
                Me.txtHe_so.Value = Convert.ToDouble(num5)
            Else
                Me.txtHe_so.Value = 1
            End If
            Try
                Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Dim num3 As Integer = 0
            Dim num13 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num13)
                If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ma_vt"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("ma_vt"))), "", False) <> 0)) Then
                    num3 = 1
                    Exit Do
                End If
                num += 1
            Loop
            If (num3 = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("007")), 2)
                oVoucher.isContinue = False
            Else
                Dim str2 As String
                Dim num2 As Integer
                Dim str5 As String
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
                Dim num12 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num12)
                    Dim num11 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    num2 = 1
                    Do While (num2 <= num11)
                        str2 = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str2))) Then
                            modVoucher.tblDetail.Item(num).Item(str2) = ""
                        End If
                        num2 += 1
                    Loop
                    num += 1
                Loop
                cString = StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
                Dim num10 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num10)
                    Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    num2 = 1
                    Do While (num2 <= num9)
                        str2 = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str2))) Then
                            modVoucher.tblDetail.Item(num).Item(str2) = 0
                        End If
                        num2 += 1
                    Loop
                    num += 1
                Loop
                If (StringType.StrCmp(Me.txtStatus.Text, "0", False) <> 0) Then
                    Dim sLeft As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldcheck", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
                    If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                        num3 = (modVoucher.tblDetail.Count - 1)
                        Dim str7 As String = clsfields.CheckEmptyFieldList("stt_rec", sLeft, modVoucher.tblDetail)
                        Try
                            If (StringType.StrCmp(str7, "", False) <> 0) Then
                                Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("044")), "%s", GetColumn(Me.grdDetail, str7).HeaderText, 1, -1, CompareMethod.Binary), 2)
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
                    If BooleanType.FromObject(ObjectType.NotObj(isValidCodes(modVoucher.appConn, Replace(Me.txtMa_sp.Text.Trim + "," + Sql.ConvertVS2SQLType(Me.txtFdate1.Value, "") + "," + Me.txtFcode1.Text.Trim, "'", ""), Me.strOldCode, "ma_sp,fdate1", "phdm", oVoucher.cAction))) Then
                        Msg.Alert(StringType.FromObject(modVoucher.oVar.Item("m_dup_field")), 2)
                        Me.txtMa_sp.Focus()
                        oVoucher.isContinue = False
                        Return
                    End If
                    Dim num8 As Integer = (modVoucher.tblDetail.Count - 1)
                    num = 0
                    Do While (num <= num8)
                        If ((Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ngay_ct1"))) And Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ngay_ct2")))) AndAlso (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("ngay_ct1"), modVoucher.tblDetail.Item(num).Item("ngay_ct2"), False) > 0)) Then
                            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("009")), 2)
                            oVoucher.isContinue = False
                            Return
                        End If
                        num += 1
                    Loop
                End If
                Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
                Me.UpdateList()
                If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                    Me.cIDNumber = oVoucher.GetIdentityNumber
                    modVoucher.tblMaster.AddNew()
                    Me.iMasterRow = (modVoucher.tblMaster.Count - 1)
                    modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec") = Me.cIDNumber
                    modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_ct") = modVoucher.VoucherCode
                Else
                    Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                End If
                xtabControl.GatherMemvarTabControl(tblMaster.Item(Me.iMasterRow), Me.tbDetail)
                DirLib.SetDatetime(modVoucher.appConn, modVoucher.tblMaster.Item(Me.iMasterRow), oVoucher.cAction)
                GatherMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
                If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                    str5 = GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row)
                Else
                    Dim str10 As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                    str5 = ((GenSQLUpdate((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row, str10) & ChrW(13) & GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), str10)) & ChrW(13) & GenSQLDelete("ctgt30", str10))
                End If
                cString = "ma_ct, stt_rec"
                Dim str4 As String = ("stt_rec = '" & Me.cIDNumber & "' or stt_rec = '' or stt_rec is null")
                modVoucher.tblDetail.RowFilter = str4
                num3 = (modVoucher.tblDetail.Count - 1)
                Dim num4 As Integer = 0
                Dim num7 As Integer = num3
                num = 0
                Do While (num <= num7)
                    If (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("stt_rec"), Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "", RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))), False) = 0) Then
                        Dim num6 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                        num2 = 1
                        Do While (num2 <= num6)
                            str2 = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                            modVoucher.tblDetail.Item(num).Item(str2) = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item(str2))
                            num2 += 1
                        Loop
                        num4 += 1
                        modVoucher.tblDetail.Item(num).Item("line_nbr") = num4
                        Me.grdDetail.Update()
                        str5 = (str5 & ChrW(13) & GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), modVoucher.tblDetail.Item(num).Row))
                    End If
                    num += 1
                Loop
                Me.EDTBColumns(False)
                Sql.SQLCompressExecute((modVoucher.appConn), str5)
                str5 = Me.Post
                Sql.SQLExecute((modVoucher.appConn), str5)
                Me.pnContent.Text = StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.tblMaster.Item(Me.iMasterRow).Item("status"), "3", False) <> 0), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("018")), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("019"))))
                Me.pnContent.Text = ""
                SaveLocalDataView(modVoucher.tblDetail)
                xtabControl.ReadOnlyTabControls(True, Me.tbDetail)
            End If
        End Sub

        Public Sub Search()
            Dim _frmSearch As New frmSearch
            _frmSearch.ShowDialog()
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
            Else
                Dim sLeft As String = Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")))
                LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(StringType.StrCmp(sLeft, "", False) = 0)}, Nothing)
            End If
        End Sub

        Private Sub txtKeyPress_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtKeyPress.Enter
            Me.grdDetail.Focus()
            Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
        End Sub

        Private Sub txtMa_sp_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
            Dim str As String
            Dim str2 As String = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "dvt", ("ma_vt = '" & Strings.Trim(Me.txtMa_sp.Text) & "'")))
            If BooleanType.FromObject(ObjectType.NotObj(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(Me.txtMa_sp.Text) & "'")))) Then
                Me.txtDvt.Text = str2
                Me.txtDvt.ReadOnly = True
                Me.txtDvt.Refresh()
                str = ("ma_vt = '" & Strings.Trim(Me.txtMa_sp.Text) & "'")
            Else
                str = ("ma_vt = '" & Strings.Trim(Me.txtMa_sp.Text) & "' OR ma_vt = '*'")
                If (StringType.StrCmp(Strings.Trim(Me.txtDvt.Text), "", False) = 0) Then
                    Me.txtDvt.Text = str2
                End If
                Me.txtDvt.ReadOnly = False
                Me.txtDvt.Refresh()
            End If
            Me.oUOMx.Key = str
        End Sub

        Private Sub txtNumber_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtDvt.Enter
            LateBinding.LateSet(sender, Nothing, "Text", New Object() {Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))}, Nothing)
        End Sub

        Private Sub txtSo_luong_enter(ByVal sender As Object, ByVal e As EventArgs)
            Me.noldSo_luong = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        End Sub

        Private Sub txtSo_luong_valid(ByVal sender As Object, ByVal e As EventArgs)
            Dim num2 As Decimal = Me.noldSo_luong
            Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
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

        Private Sub UnCheckLockedDate()
            oVoucher.txtVDate.Value = DateType.FromObject(Sql.GetValue((modVoucher.appConn), "dmstt", "ngay_ks", "1=1")).AddDays(1)
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

        Public Sub vCaptionRefresh()
            Me.EDFC()
            Me.pnContent.Text = ""
        End Sub

        Public Sub vFCRate()
        End Sub

        Public Sub View()
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
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblMaster), gridformtran2, (tbs), (cols), "BMMaster")
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
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), gridformtran, (style), (cols), "BMDetail")
            index = 0
            Do
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index <= &H1D)
            oVoucher.HideFields(gridformtran)
            Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
            Dim count As Integer = modVoucher.tblMaster.Count
            expression = Strings.Replace(Strings.Replace(Strings.Replace(expression, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, CompareMethod.Binary), "%n2", "X", 1, -1, CompareMethod.Binary), "%n3", "X", 1, -1, CompareMethod.Binary)
            panel.Text = expression
            AddHandler gridformtran2.CurrentCellChanged, New EventHandler(AddressOf Me.grdMVCurrentCellChanged)
            gridformtran2.CurrentRowIndex = Me.iMasterRow
            Obj.Init(frmAdd)
            Dim collection As New Collection
            collection.Add(Me, "Form", Nothing, Nothing)
            collection.Add(gridformtran2, "grdHeader", Nothing, Nothing)
            collection.Add(gridformtran, "grdDetail", Nothing, Nothing)
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
            End With
        End Sub

        Private Sub WhenUOMEnter(ByVal sender As Object, ByVal e As EventArgs)
            If Me.grdDetail.CurrentRowIndex < 0 Then
                Return
            End If
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")), "C") Then
                Return
            End If
            Dim str As String
            If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(StringType.FromObject(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"))) & "'"))) Then
                str = ("(ma_vt = '" & Strings.Trim(StringType.FromObject(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"))) & "' OR ma_vt = '*')")
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

        Private Sub WhenUOMLeave(ByVal sender As Object, ByVal e As EventArgs)
            If Me.grdDetail.CurrentRowIndex < 0 Then
                Return
            End If
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
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
        Function isValidCodes(ByVal cn As SqlConnection, ByVal cValues As String, ByVal cOldValues As String, ByVal cFields As String, ByVal cTable As String, ByVal cAction As String) As Object
            Dim str2 As String
            Dim array_fields() As String = cFields.Split(","c)
            Dim array_values() As String = cValues.Split(","c)
            Dim array_oldvalues() As String = cOldValues.Split(","c)
            Dim i, n As Integer
            n = array_fields.Length
            str2 = "IF EXISTS(SELECT * FROM " + cTable + " WHERE "
            For i = 0 To n - 1
                str2 += array_fields(i).Trim + " = '" + array_values(i) + "' AND "
            Next
            str2 = Strings.Left(str2, str2.Length - 5)
            If (StringType.StrCmp(cAction, "Edit", False) = 0) Then
                str2 += " AND "
                For i = 0 To n - 1
                    str2 += array_fields(i).Trim + " <> '" + array_oldvalues(i) + "' AND "
                Next
                str2 = Strings.Left(str2, str2.Length - 5)
            End If
            str2 += ")"
            str2 = (str2 & " SELECT 0 AS Value ELSE SELECT 1 AS Value")
            Dim ds As New DataSet
            Sql.SQLRetrieve(cn, str2, "Value", ds)
            Dim flag As Boolean = (ObjectType.ObjTst(ds.Tables.Item("Value").Rows.Item(0).Item("Value"), 1, False) = 0)
            ds = Nothing
            Return flag
        End Function

        ' Properties
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
        Friend WithEvents lblDvt As Label
        Friend WithEvents lblMa_bp As Label
        Friend WithEvents lblMa_sp As Label
        Friend WithEvents lblStatusMess As Label
        Friend WithEvents lblTen_bp As Label
        Friend WithEvents lblTen_sp As Label
        Friend WithEvents lblTien_hang As Label
        Friend WithEvents tbDetail As TabControl
        Friend WithEvents tpgDetail As TabPage
        Friend WithEvents txtDvt As TextBox
        Friend WithEvents txtHe_so As txtNumeric
        Friend WithEvents txtKeyPress As TextBox
        Friend WithEvents txtMa_bp As TextBox
        Friend WithEvents txtMa_sp As TextBox
        Friend WithEvents txtStatus As TextBox
        Friend WithEvents txtT_so_luong As txtNumeric
        Friend WithEvents txtTy_gia As txtNumeric

        Public arrControlButtons As Button()
        Public cIDNumber As String
        Public cOldIDNumber As String
        Private cOldItem As String
        Private colDvt As DataGridTextBoxColumn
        Private colMa_vt As DataGridTextBoxColumn, colMa_ct As DataGridTextBoxColumn
        Private colSo_luong As DataGridTextBoxColumn
        Private colTen_vt As DataGridTextBoxColumn
        Private components As IContainer
        Public iDetailRow As Integer
        Public iMasterRow As Integer
        Public iOldMasterRow As Integer
        Private iOldRow As Integer
        Private isActive As Boolean
        Private lAllowCurrentCellChanged As Boolean
        Private nColumnControl As Integer
        Private noldSo_luong As Decimal
        Private oInvItemDetail As VoucherLibObj, oMa_ct As VoucherLibObj
        Private oldtblDetail As DataTable
        'Private oTitleButton As TitleButton
        Private oUOM As VoucherKeyCheckLibObj
        Private oUOMx As dirblanklib
        Public oVoucher As clsvoucher.clsVoucher
        Public pnContent As StatusBarPanel
        Private strOldCode As String
        Private xInventory As clsInventory
        Private colS1 As DataGridTextBoxColumn
    End Class
End Namespace

