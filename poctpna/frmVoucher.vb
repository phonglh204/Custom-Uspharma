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
Public Class frmVoucher
    Inherits Form
    Public arrControlButtons(12) As Button
    Public cIDNumber As String
    Private colCMa_cp As DataGridTextBoxColumn
    Private colCTen_cp As DataGridTextBoxColumn
    Private colCTien_cp As DataGridTextBoxColumn
    Private colCTien_cp_nt As DataGridTextBoxColumn
    Private coldCMa_cp As String
    Public cOldIDNumber As String
    Private cOldItem As String
    Private cOldSite As String
    Private coldTk As String
    Private coldVMa_thue As String
    Private colDvt As DataGridTextBoxColumn
    Private coldVTk_thue_no As String
    Private colGia_nt0 As DataGridTextBoxColumn
    Private colGia0 As DataGridTextBoxColumn
    Private colMa_kho As DataGridTextBoxColumn
    Private colMa_lo As DataGridTextBoxColumn
    Private colMa_vi_tri As DataGridTextBoxColumn
    Private colMa_vt As DataGridTextBoxColumn
    Private colPd_line As DataGridTextBoxColumn
    Private colPo_line As DataGridTextBoxColumn
    Private colSo_dh As DataGridTextBoxColumn
    Private colSo_luong As DataGridTextBoxColumn
    Private colSo_pn As DataGridTextBoxColumn
    Private colTen_vt As DataGridTextBoxColumn
    Private colTien_nt0 As DataGridTextBoxColumn
    Private colTien0 As DataGridTextBoxColumn
    Private colTk_vt As DataGridTextBoxColumn
    Private colVDia_chi As DataGridTextBoxColumn
    Private colVMa_kh As DataGridTextBoxColumn
    Private colVMa_kh2 As DataGridTextBoxColumn
    Private colVMa_kho As DataGridTextBoxColumn
    Private colVMa_so_thue As DataGridTextBoxColumn
    Private colVMa_thue As DataGridTextBoxColumn
    Private colVMa_tt As DataGridTextBoxColumn
    Private colVMau_bc As DataGridTextBoxColumn
    Private colVNgay_ct0 As DataGridTextBoxColumn
    Private colVSo_ct0 As DataGridTextBoxColumn
    Private colVSo_seri0 As DataGridTextBoxColumn
    Private colVT_Thue As DataGridTextBoxColumn
    Private colVT_thue_nt As DataGridTextBoxColumn
    Private colVT_Tien As DataGridTextBoxColumn
    Private colVT_tien_nt As DataGridTextBoxColumn
    Private colVTen_kh As DataGridTextBoxColumn
    Private colVTen_vt As DataGridTextBoxColumn
    Private colVThue_suat As DataGridTextBoxColumn
    Private colVTk_thue_no As DataGridTextBoxColumn
    Private components As IContainer
    Private frmView As Form
    Private grdHeader As grdHeader
    Private grdMV As gridformtran
    Public iDetailRow As Integer
    Public iMasterRow As Integer
    Public iOldMasterRow As Integer
    Private iOldRow As Integer
    Private isActive As Boolean
    Private lAllowCurrentCellChanged As Boolean
    Private m_ma_thue_1 As String
    Private nColumnControl As Integer
    Private noldCTien_cp As Decimal
    Private noldCTien_cp_nt As Decimal
    Private nOldECharge As Decimal
    Private noldGia_nt0 As Decimal
    Private noldGia0 As Decimal
    Private noldSo_luong As Decimal
    Private noldTien_nt0 As Decimal
    Private noldTien0 As Decimal
    Private noldVT_Thue As Decimal
    Private noldVT_Thue_nt As Decimal
    Private noldVT_tien As Decimal
    Private noldVT_tien_nt As Decimal
    Private oInvItemDetail As VoucherLibObj
    Private oldtblDetail As DataTable
    Private oLocation As VoucherKeyLibObj
    Private oLot As VoucherKeyLibObj
    Private oSecurity As clssecurity
    Private oSite As VoucherKeyLibObj
    Private oTaxAuthority As VoucherLibObj
    'Private oTitleButton As TitleButton
    Private oUOM As VoucherKeyCheckLibObj
    Private oVCustomerDetail As VoucherLibObj
    Private oVDrTaxAccount As VoucherLibObj
    Public oVoucher As clsvoucher.clsVoucher
    Private oVTaxCodeDetail As VoucherLibObj
    Private pn As StatusBarPanel
    Public pnContent As StatusBarPanel
    Private TaxAuthority_IsFocus As Boolean
    Private tblHandling As DataTable
    Private tblRetrieveDetail As DataView
    Private tblRetrieveMaster As DataView
    Private tblStatus As DataTable
    Private xInventory As clsInventory
    ' Methods
    Public Sub New()
        AddHandler MyBase.Activated, New EventHandler(AddressOf Me.frmVoucher_Activated)
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmVoucher_Load)
        Me.arrControlButtons = New Button(13 - 1) {}
        'Me.oTitleButton = New TitleButton(Me)
        Me.TaxAuthority_IsFocus = True
        Me.m_ma_thue_1 = Nothing
        Me.lAllowCurrentCellChanged = True
        Me.frmView = New Form
        Me.grdMV = New gridformtran
        Me.xInventory = New clsInventory
        Me.InitializeComponent()
    End Sub

    Private Sub AddEChargeHandler()
        Dim column5 As DataGridTextBoxColumn = GetColumn(Me.grdMV, "cp_vc")
        Dim col As DataGridTextBoxColumn = GetColumn(Me.grdMV, "cp_vc_nt")
        Dim column As DataGridTextBoxColumn = GetColumn(Me.grdMV, "cp_bh")
        Dim column2 As DataGridTextBoxColumn = GetColumn(Me.grdMV, "cp_bh_nt")
        Dim column3 As DataGridTextBoxColumn = GetColumn(Me.grdMV, "cp_khac")
        Dim column4 As DataGridTextBoxColumn = GetColumn(Me.grdMV, "cp_khac_nt")
        AddHandler column5.TextBox.Enter, New EventHandler(AddressOf Me.txtECp_vc_enter)
        AddHandler column5.TextBox.Leave, New EventHandler(AddressOf Me.txtECp_vc_valid)
        AddHandler col.TextBox.Enter, New EventHandler(AddressOf Me.txtECp_vc_nt_enter)
        AddHandler col.TextBox.Leave, New EventHandler(AddressOf Me.txtECp_vc_nt_valid)
        AddHandler column.TextBox.Enter, New EventHandler(AddressOf Me.txtECp_bh_enter)
        AddHandler column.TextBox.Leave, New EventHandler(AddressOf Me.txtECp_bh_valid)
        AddHandler column2.TextBox.Enter, New EventHandler(AddressOf Me.txtECp_bh_nt_enter)
        AddHandler column2.TextBox.Leave, New EventHandler(AddressOf Me.txtECp_bh_nt_valid)
        AddHandler column3.TextBox.Enter, New EventHandler(AddressOf Me.txtECp_khac_enter)
        AddHandler column3.TextBox.Leave, New EventHandler(AddressOf Me.txtECp_khac_valid)
        AddHandler column4.TextBox.Enter, New EventHandler(AddressOf Me.txtECp_khac_nt_enter)
        AddHandler column4.TextBox.Leave, New EventHandler(AddressOf Me.txtECp_khac_nt_valid)
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            ChangeFormatColumn(col, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(column2, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(column4, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            col.HeaderText = column5.HeaderText
            column2.HeaderText = column.HeaderText
            column4.HeaderText = column3.HeaderText
            column5.MappingName = "x01"
            column.MappingName = "x02"
            column3.MappingName = "x03"
        End If
    End Sub

    Public Sub AddNew()
        Me.grdHeader.ScatterBlank()
        modVoucher.tblDetail.AddNew()
        modVoucher.tblDetail.RowFilter = "stt_rec is null or stt_rec = ''"
        Me.pnContent.Text = ""
        ScatterMemvarBlankWithDefault(Me)
        Me.chkGia_thue_yn.Checked = True
        If (ObjectType.ObjTst(Me.txtNgay_ct.Text, Fox.GetEmptyDate, False) = 0) Then
            Me.txtNgay_ct.Value = DateAndTime.Now.Date
            Me.txtNgay_lct.Value = Me.txtNgay_ct.Value
        End If
        If (StringType.StrCmp(Strings.Trim(Me.cmdMa_nt.Text), "", False) = 0) Then
            Me.cmdMa_nt.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("ma_nt"))
        End If
        Me.txtTy_gia.Value = DoubleType.FromObject(oVoucher.GetFCRate(Me.cmdMa_nt.Text, Me.txtNgay_ct.Value))
        Me.txtSo_ct.Text = oVoucher.GetVoucherNo
        Me.txtMa_gd.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_ma_gd"))
        Me.txtStatus.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_status"))
        Unit.SetUnit(Me.txtMa_dvcs)
        Me.EDFC()
        Me.cOldIDNumber = Me.cIDNumber
        Me.iOldMasterRow = Me.iMasterRow
        Me.RefreshCharge(0)
        Me.RefreshVAT(0)
        Me.UpdateList()
        Me.ShowTabDetail()
        If Me.txtMa_dvcs.Enabled Then
            Me.txtMa_dvcs.Focus()
        Else
            Me.txtMa_kh.Focus()
        End If
        Me.EDTBColumns()
        Me.InitFlowHandling(Me.cboAction)
        Me.EDStatus()
        Me.grdCharge.ReadOnly = False
        Me.grdOther.ReadOnly = True
        Me.oSecurity.SetReadOnly()
        Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
    End Sub

    Private Sub AfterUpdatePV(ByVal lcIDNumber As String, ByVal lcAction As String)
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) <> 0) Then
            Dim tcSQL As String = String.Concat(New String() {"fs_AfterUpdatePV '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
            Sql.SQLExecute((modVoucher.appConn), tcSQL)
        End If
    End Sub

    Private Sub AllocateBy(ByVal nAmount As Decimal, ByVal nTQ As Decimal, ByVal cQ As String, ByVal cField As String, ByVal nRound As Integer)
        If (nTQ <> 0) Then
            Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                With modVoucher.tblDetail.Item(i)
                    If Information.IsDBNull(.Item(cQ)) Then
                        Return
                    End If
                    .Item(cField) = .Item(cField) + Fox.Round(nAmount * .Item(cQ) / nTQ, nRound)
                End With
                i += 1
            Loop
        End If
    End Sub

    Private Sub AllocateBy(ByVal nAmount As Decimal, ByVal nTQ As Decimal, ByVal cQ As String, ByVal cField As String, ByVal nRound As Integer, ByVal cQty As String)
        On Error Resume Next
        If nTQ = 0 Then
            Return
        End If
        Dim i As Integer
        For i = 0 To tblDetail.Count - 1
            With tblDetail(i)
                If IsDBNull(.Item(cQ)) Or IsDBNull(.Item(cQty)) Then
                    Return
                End If
                .Item(cField) += Fox.Round(nAmount * .Item("so_luong") * .Item("he_so") * .Item(cQ) / nTQ, nRound)
            End With
        Next
    End Sub

    Private Sub AllocateCharge(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim nRound As Integer = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))
            Dim num4 As Integer = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            Dim zero As Decimal = Decimal.Zero
            Dim num8 As Decimal = Decimal.Zero
            Dim num11 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim num As Integer = 0
            Do While (num <= num11)
                With modVoucher.tblDetail.Item(num)
                    .Item("cp_vc_nt") = 0
                    .Item("cp_bh_nt") = 0
                    .Item("cp_khac_nt") = 0
                    .Item("cp_vc") = 0
                    .Item("cp_bh") = 0
                    .Item("cp_khac") = 0
                    If IsDBNull(.Item("so_luong")) Then
                        .Item("so_luong") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("he_so"))) Then
                        .Item("he_so") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("volume"))) Then
                        .Item("volume") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("weight"))) Then
                        .Item("weight") = 0
                    End If
                    num8 = num8 + .Item("volume") * .Item("so_luong") * .Item("he_so")
                    zero = zero + .Item("weight") * .Item("so_luong") * .Item("he_so")
                End With
                num += 1
            Loop
            Dim num10 As Integer = (modVoucher.tblCharge.Count - 1)
            num = 0
            Dim str3 As String = ""
            Dim str4 As String = ""
            Dim str5 As String = ""
            Dim str6 As String = ""
            Dim num5 As Decimal = 0
            Dim num7 As Decimal = 0
            Do While (num <= num10)
                With modVoucher.tblCharge.Item(num)
                    If (Not IsDBNull(.Item("ma_cp")) AndAlso (.Item("ma_cp") <> "")) Then
                        If IsDBNull(.Item("tien_cp_nt")) Then
                            .Item("tien_cp_nt") = 0
                        End If
                        If IsDBNull(.Item("tien_cp")) Then
                            .Item("tien_cp") = 0
                        End If
                        If .Item("loai_cp") = "1" Then
                            str5 = "cp_vc"
                            str3 = "cp_vc_nt"
                        ElseIf .Item("loai_cp") = "2" Then
                            str5 = "cp_bh"
                            str3 = "cp_bh_nt"
                        ElseIf .Item("loai_cp") = "3" Then
                            str5 = "cp_khac"
                            str3 = "cp_khac_nt"
                        End If
                        If .Item("loai_pb") = "1" Then
                            str6 = "so_luong"
                            str4 = "so_luong"
                            num7 = New Decimal(Me.txtT_so_luong.Value)
                            num5 = New Decimal(Me.txtT_so_luong.Value)
                            Me.AllocateBy(.Item("tien_cp"), num7, str6, str5, nRound)
                            Me.AllocateBy(.Item("tien_cp_nt"), num5, str4, str3, num4)
                        ElseIf (.Item("loai_pb") = "3") Then
                            str6 = "weight"
                            str4 = "weight"
                            num7 = zero
                            num5 = zero
                            Me.AllocateBy(.Item("tien_cp"), num7, str6, str5, nRound, "so_luong")
                            Me.AllocateBy(.Item("tien_cp_nt"), num5, str4, str3, num4, "so_luong")
                        ElseIf (.Item("loai_pb") = "2") Then
                            str6 = "volume"
                            str4 = "volume"
                            num7 = num8
                            num5 = num8
                            Me.AllocateBy(.Item("tien_cp"), num7, str6, str5, nRound, "so_luong")
                            Me.AllocateBy(.Item("tien_cp_nt"), num5, str4, str3, num4, "so_luong")
                        ElseIf (.Item("loai_pb") = "4") Then
                            str6 = "tien0"
                            str4 = "tien_nt0"
                            num7 = Me.txtT_tien0.Value + Interaction.IIf(Me.chkGia_thue_yn.Checked, 0, Me.txtT_thue.Value)
                            num5 = Me.txtT_tien_nt0.Value + Interaction.IIf(Me.chkGia_thue_yn.Checked, 0, Me.txtT_thue_nt.Value)
                            Me.AllocateBy(.Item("tien_cp"), num7, str6, str5, nRound)
                            Me.AllocateBy(.Item("tien_cp_nt"), num5, str4, str3, num4)
                        End If
                    End If
                End With
                num += 1
            Loop
            Me.AuditCharge()
        End If
    End Sub

    Private Sub AppendVAT()
        If ((Me.txtT_tien0.Value = 0) And (modVoucher.tblOther.Count < 1)) Then
            Me.grdOther.ReadOnly = True
        Else
            Me.grdOther.ReadOnly = False
        End If
        If ((modVoucher.tblOther.Count < 1) And (Me.txtT_tien0.Value > 0)) Then
            Dim row As DataRow = modVoucher.tblOther.Table.NewRow
            row.Item("mau_bc") = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "vatform", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
            row.Item("so_ct0") = Fox.PadL(Strings.Trim(Me.txtSo_ct0.Text), Me.txtSo_ct0.MaxLength)
            row.Item("so_seri0") = Me.txtSo_seri0.Text
            If (ObjectType.ObjTst(Me.txtNgay_ct0.Text, Fox.GetEmptyDate, False) <> 0) Then
                row.Item("ngay_ct0") = Me.txtNgay_ct0.Value
            End If
            row.Item("t_tien") = Me.txtT_tien0.Value
            row.Item("t_tien_nt") = Me.txtT_tien_nt0.Value
            row.Item("ma_kh") = Me.txtMa_kh.Text
            Dim maxFields As Integer = clsfields.GetMaxFields("tien0", modVoucher.tblDetail)
            row.Item("ten_vt") = RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(maxFields).Item("ten_vt"))
            If (ObjectType.ObjTst(modVoucher.oOption.Item("m_po_tt"), "1", False) = 0) Then
                row.Item("ma_tt") = Me.txtMa_tt.Text
            End If
            modVoucher.tblOther.Table.Rows.Add(row)
            Me.grdOther.Refresh()
            Me.grdOther.CurrentCell = New DataGridCell(0, 0)
        End If
    End Sub
    Private Sub AppendVATDetail()
        If ((Me.txtT_tien0.Value = 0) And (modVoucher.tblOther.Count < 1)) Then
            Me.grdOther.ReadOnly = True
        Else
            Me.grdOther.ReadOnly = False
        End If
        If (Me.txtT_tien0.Value > 0) Then
            Dim i As Integer
            For i = tblOther.Count - 1 To 0 Step -1
                tblOther.Item(i).Delete()
            Next
            For i = 0 To tblDetail.Count - 1
                Dim row As DataRow = modVoucher.tblOther.Table.NewRow
                row.Item("mau_bc") = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "vatform", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
                row.Item("so_ct0") = Fox.PadL(Strings.Trim(Me.txtSo_ct0.Text), Me.txtSo_ct0.MaxLength)
                row.Item("so_seri0") = Me.txtSo_seri0.Text
                If (ObjectType.ObjTst(Me.txtNgay_ct0.Text, Fox.GetEmptyDate, False) <> 0) Then
                    row.Item("ngay_ct0") = Me.txtNgay_ct0.Value
                End If
                row.Item("t_tien") = tblDetail.Item(i).Item("tien")
                row.Item("t_tien_nt") = tblDetail.Item(i).Item("tien_nt")
                row.Item("ma_kh") = Me.txtMa_kh.Text
                row.Item("ten_vt") = RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("ten_vt"))
                If (ObjectType.ObjTst(modVoucher.oOption.Item("m_po_tt"), "1", False) = 0) Then
                    row.Item("ma_tt") = Me.txtMa_tt.Text
                End If
                modVoucher.tblOther.Table.Rows.Add(row)
            Next
            Me.grdOther.Refresh()
            Me.grdOther.CurrentCell = New DataGridCell(i, 0)
        End If
    End Sub

    Private Sub AuditCharge()
        Dim num As Integer
        Dim zero As Decimal = Decimal.Zero
        Dim num2 As Decimal = Decimal.Zero
        Dim num4 As Decimal = Decimal.Zero
        Dim num7 As Decimal = Decimal.Zero
        Dim num3 As Decimal = Decimal.Zero
        Dim num5 As Decimal = Decimal.Zero
        Dim num9 As Integer = (modVoucher.tblCharge.Count - 1)
        num = 0
        Do While (num <= num9)
            With modVoucher.tblCharge.Item(num)
                If (Not IsDBNull(.Item("ma_cp")) AndAlso (.Item("ma_cp") <> "")) Then
                    If IsDBNull(.Item("tien_cp_nt")) Then
                        .Item("tien_cp_nt") = 0
                    End If
                    If IsDBNull(.Item("tien_cp")) Then
                        .Item("tien_cp") = 0
                    End If
                    If (.Item("loai_cp") = "1") Then
                        num7 = num7 + .Item("tien_cp_nt")
                        zero = zero + .Item("tien_cp")
                    ElseIf (.Item("loai_cp") = "2") Then
                        num3 = num3 + .Item("tien_cp_nt")
                        num2 = num2 + .Item("tien_cp")
                    ElseIf (.Item("loai_cp") = "3") Then
                        num5 = num5 + .Item("tien_cp_nt")
                        num4 = num4 + .Item("tien_cp")
                    End If
                End If
            End With
            num += 1
        Loop
        auditamount.AuditAmounts(num7, "cp_vc_nt", modVoucher.tblDetail)
        auditamount.AuditAmounts(num3, "cp_bh_nt", modVoucher.tblDetail)
        auditamount.AuditAmounts(num5, "cp_khac_nt", modVoucher.tblDetail)
        auditamount.AuditAmounts(zero, "cp_vc", modVoucher.tblDetail)
        auditamount.AuditAmounts(num2, "cp_bh", modVoucher.tblDetail)
        auditamount.AuditAmounts(num4, "cp_khac", modVoucher.tblDetail)
        Dim num8 As Integer = (modVoucher.tblDetail.Count - 1)
        num = 0
        Do While (num <= num8)
            With modVoucher.tblDetail.Item(num)
                .Item("cp_nt") = .Item("cp_vc_nt") + .Item("cp_bh_nt") + .Item("cp_khac_nt")
                .Item("cp") = .Item("cp_vc") + .Item("cp_bh") + .Item("cp_khac")
            End With
            num += 1
        Loop
        Me.UpdateList()
    End Sub

    Private Sub BeforUpdatePV(ByVal lcIDNumber As String, ByVal lcAction As String)
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) <> 0) Then
            Dim tcSQL As String = String.Concat(New String() {"fs_BeforUpdatePV '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
            Sql.SQLExecute((modVoucher.appConn), tcSQL)
        End If
    End Sub

    Public Sub Cancel()
        Dim num2 As Integer
        Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
        If (currentRowIndex >= 0) Then
            Me.grdDetail.Select(currentRowIndex)
        End If
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            Me.RefreshCharge(0)
            Me.RefreshVAT(0)
            num2 = (modVoucher.tblDetail.Count - 1)
            currentRowIndex = num2
            Do While (currentRowIndex >= 0)
                If Not IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) Then
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
                Me.RefreshCharge(1)
                Me.RefreshVAT(1)
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

    Private Sub chkGia_thue_yn_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkGia_thue_yn.CheckedChanged
        Me.UpdateList()
    End Sub

    Public Sub Delete()
        If Me.oSecurity.GetStatusDelelete Then
            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "cttt30", "stt_rec", ("LEFT(stt_rec_tt, 10) = '" & Strings.Mid(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), 1, 10) & "'")))), "", False) <> 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oVar.Item("m_inv_not_delete")), 1)
            Else
                Dim str As String
                Dim num As Integer
                Dim str5 As String
                Dim str6 As String
                Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
                Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                Dim str3 As String = ("LEFT(stt_rec, 10) = '" & Strings.Mid(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), 1, 10) & "'")
                Dim lcIDNumber As String = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
                num = num2
                Do While (num >= 0)
                    If Not IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("stt_rec"))) Then
                        If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                            modVoucher.tblDetail.Item(num).Delete()
                        End If
                    Else
                        modVoucher.tblDetail.Item(num).Delete()
                    End If
                    num = (num + -1)
                Loop
                If (ObjectType.ObjTst(modVoucher.oVar.Item("m_pack_yn"), 0, False) = 0) Then
                    str6 = "ct00, ct11, ph11, ct70, ct90, ct74, ph74, ctcp30, ctgt30"
                    str5 = ""
                Else
                    str6 = (Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & ", ct00, ct11, ph11, ct70, ct90, ct74, ph74, ctcp30, ctgt30")
                    str5 = GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)
                End If
                Dim num4 As Integer = IntegerType.FromObject(Fox.GetWordCount(str6, ","c))
                num = 1
                Do While (num <= num4)
                    str = Strings.Trim(Fox.GetWordNum(str6, num, ","c))
                    str5 = (str5 & ChrW(13) & GenSQLDelete(str, cKey))
                    num += 1
                Loop
                str6 = "cttt30"
                Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(str6, ","c))
                num = 1
                Do While (num <= num3)
                    str = Strings.Trim(Fox.GetWordNum(str6, num, ","c))
                    str5 = (str5 & ChrW(13) & GenSQLDelete(str, str3))
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
                    str5 = ((String.Concat(New String() {str5, ChrW(13), "UPDATE ", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), " SET Status = '*'"}) & ", datetime2 = GETDATE(), user_id2 = " & StringType.FromObject(Reg.GetRegistryKey("CurrUserId"))) & "  WHERE " & cKey)
                End If
                Me.BeforUpdatePV(lcIDNumber, "Del")
                Sql.SQLExecute((modVoucher.appConn), str5)
                Me.pnContent.Text = ""
            End If
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

    Private Sub DeleteItemCharge(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim currentRowIndex As Integer = Me.grdCharge.CurrentRowIndex
            If ((((currentRowIndex >= 0) And (currentRowIndex < modVoucher.tblCharge.Count)) AndAlso Not Me.grdCharge.EndEdit(Me.grdCharge.TableStyles.Item(0).GridColumnStyles.Item(Me.grdCharge.CurrentCell.ColumnNumber), currentRowIndex, False)) AndAlso (ObjectType.ObjTst(Msg.Question(StringType.FromObject(modVoucher.oVar.Item("m_sure_dele")), 1), 1, False) = 0)) Then
                Me.grdCharge.Select(currentRowIndex)
                Dim view As DataRowView = modVoucher.tblCharge.Item(currentRowIndex)
                AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), False)
                view.Delete()
                If (modVoucher.tblCharge.Count = 0) Then
                    Me.AllocateCharge(RuntimeHelpers.GetObjectValue(New Object), New EventArgs)
                End If
                AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), True)
            End If
        End If
    End Sub

    Private Sub DeleteItemVAT(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim currentRowIndex As Integer = Me.grdOther.CurrentRowIndex
            If ((((currentRowIndex >= 0) And (currentRowIndex < modVoucher.tblOther.Count)) AndAlso Not Me.grdOther.EndEdit(Me.grdOther.TableStyles.Item(0).GridColumnStyles.Item(Me.grdOther.CurrentCell.ColumnNumber), currentRowIndex, False)) AndAlso (ObjectType.ObjTst(Msg.Question(StringType.FromObject(modVoucher.oVar.Item("m_sure_dele")), 1), 1, False) = 0)) Then
                If (modVoucher.tblOther.Count = 1) Then
                    'Me.grdOther.CurrentCell = 0
                End If
                Me.grdOther.Select(currentRowIndex)
                Dim view As DataRowView = modVoucher.tblOther.Item(currentRowIndex)
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
            ChangeFormatColumn(Me.colGia_nt0, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia")))
            ChangeFormatColumn(Me.colTien_nt0, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colCTien_cp_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colVT_tien_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colVT_thue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            Me.colTien_nt0.HeaderText = StringType.FromObject(modVoucher.oLan.Item("018"))
            Me.colGia_nt0.HeaderText = StringType.FromObject(modVoucher.oLan.Item("032"))
            Me.colCTien_cp_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("018"))
            Me.colVT_tien_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("025"))
            Me.colVT_thue_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("027"))
            Me.txtT_tien_nt0.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_cp_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tien_nt0.Value = Me.txtT_tien_nt0.Value
            Me.txtT_thue_nt.Value = Me.txtT_thue_nt.Value
            Me.txtT_cp_nt.Value = Me.txtT_cp_nt.Value
            Me.txtT_tt_nt.Value = Me.txtT_tt_nt.Value
            Try
                Me.colTien0.MappingName = "H1"
                Me.colGia0.MappingName = "H4"
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Try
                Me.colVT_Tien.MappingName = "H2"
                Me.colVT_Thue.MappingName = "H3"
                Me.colCTien_cp.MappingName = "H5"
            Catch exception3 As Exception
                ProjectData.SetProjectError(exception3)
                Dim exception As Exception = exception3
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien0.Visible = False
            Me.txtT_thue.Visible = False
            Me.txtT_cp.Visible = False
            Me.txtT_tt.Visible = False
        Else
            Me.txtTy_gia.Enabled = True
            ChangeFormatColumn(Me.colGia_nt0, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
            ChangeFormatColumn(Me.colTien_nt0, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colCTien_cp_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colVT_tien_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colVT_thue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            Me.colTien_nt0.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("019")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colGia_nt0.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("033")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colCTien_cp_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("019")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colVT_tien_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("024")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colVT_thue_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("026")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.txtT_tien_nt0.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_cp_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tien_nt0.Value = Me.txtT_tien_nt0.Value
            Me.txtT_thue_nt.Value = Me.txtT_thue_nt.Value
            Me.txtT_cp_nt.Value = Me.txtT_cp_nt.Value
            Me.txtT_tt_nt.Value = Me.txtT_tt_nt.Value
            Try
                Me.colTien0.MappingName = "tien0"
                Me.colGia0.MappingName = "gia0"
            Catch exception4 As Exception
                ProjectData.SetProjectError(exception4)
                ProjectData.ClearProjectError()
            End Try
            Try
                Me.colVT_Tien.MappingName = "t_tien"
                Me.colVT_Thue.MappingName = "t_thue"
                Me.colCTien_cp.MappingName = "tien_cp"
            Catch exception5 As Exception
                ProjectData.SetProjectError(exception5)
                Dim exception2 As Exception = exception5
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien0.Visible = True
            Me.txtT_thue.Visible = True
            Me.txtT_cp.Visible = True
            Me.txtT_tt.Visible = True
        End If
        Me.EDStatus()
        Me.oSecurity.Invisible()
    End Sub

    Public Sub Edit()
        Dim flag As Boolean = (Sql.GetValue(appConn, "cttt30", "stt_rec", "LEFT(stt_rec_tt, 10) = '" & Strings.Mid(tblMaster.Item(Me.iMasterRow).Item("stt_rec"), 1, 10) & "'") <> "")
        Dim _stt_rec As String = Sql.GetValue(appConn, "cttt30", "stt_rec", "stt_rec_tt='" + tblMaster.Item(iMasterRow).Item("stt_rec") + "'")
        flag = (_stt_rec <> "")
        Me.txtMa_kh.ReadOnly = flag
        Me.txtTk.ReadOnly = flag
        Me.txtMa_dvcs.ReadOnly = flag
        Me.txtMa_gd.ReadOnly = flag
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
        Me.InitFlowHandling(Me.cboAction)
        Me.EDStatus()
        'Me.txtMa_kh.ReadOnly = flag
        Me.grdCharge.ReadOnly = False
        Me.oSecurity.SetReadOnly()
        If Not Me.oSecurity.GetStatusEdit Then
            Me.cmdSave.Enabled = False
        ElseIf ((ObjectType.ObjTst(modVoucher.oOption.Item("m_pay_rec_type"), "1", False) = 0) AndAlso flag) Then
            Msg.Alert(StringType.FromObject(modVoucher.oVar.Item("m_inv_not_edit")), 2)
            Me.cmdSave.Enabled = False
        End If
        Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
    End Sub

    Private Sub EditAllocatedCharge(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Me.frmView = New Form
            Me.grdMV = New gridformtran
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
            frmView.Top = 0
            frmView.Left = 0
            frmView.Width = Me.Width
            frmView.Height = Me.Height
            frmView.Text = StringType.FromObject(modVoucher.oLan.Item("203"))
            frmView.StartPosition = FormStartPosition.CenterParent
            Me.pn = AddStb(Me.frmView)
            grdMV.CaptionVisible = False
            grdMV.ReadOnly = False
            grdMV.Top = 0
            grdMV.Left = 0
            grdMV.Height = ((Me.Height - 60) - SystemInformation.CaptionHeight)
            grdMV.Width = (Me.Width - 5)
            grdMV.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            grdMV.BackgroundColor = Color.White
            Me.frmView.Controls.Add(Me.grdMV)
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), (grdMV), (tbs), (cols), "ECharge")
            index = 0
            Do
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                cols(index).TextBox.Enabled = ((index >= 2) And (index <= 7))
                index += 1
            Loop While (index <= &H1D)
            Me.AddEChargeHandler()
            Me.pn.Text = ""
            Obj.Init(Me.frmView)
            Dim button2 As New Button
            Dim button As New Button
            button2.Top = ((Me.Height - SystemInformation.CaptionHeight) - &H37)
            button2.Left = 0
            button2.Visible = True
            button2.Text = StringType.FromObject(modVoucher.oLan.Item("038"))
            button2.Width = &H4B
            button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            button2.DialogResult = DialogResult.OK
            button.Top = button2.Top
            button.Left = ((button2.Left + button2.Width) + 1)
            button.Visible = True
            button.Text = StringType.FromObject(modVoucher.oLan.Item("039"))
            button.Width = button2.Width
            button.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            button.Enabled = True
            button.DialogResult = DialogResult.Cancel
            Me.frmView.Controls.Add(button2)
            Me.frmView.Controls.Add(button)
            Dim allowNew As Boolean = modVoucher.tblDetail.AllowNew
            Dim allowDelete As Boolean = modVoucher.tblDetail.AllowDelete
            modVoucher.tblDetail.AllowDelete = False
            modVoucher.tblDetail.AllowNew = False
            Me.SaveCharge()
            If (Me.frmView.ShowDialog = DialogResult.OK) Then
                Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
                index = 0
                Do While (index <= num2)
                    With modVoucher.tblDetail.Item(index)
                        .Item("cp_nt") = .Item("cp_vc_nt") + .Item("cp_bh_nt") + .Item("cp_khac_nt")
                        .Item("cp") = .Item("cp_vc") + .Item("cp_bh") + .Item("cp_khac")
                    End With
                    index += 1
                Loop
                Me.UpdateList()
                Me.isValidCharge()
            Else
                Me.RestoreCharge()
            End If
            Me.frmView.Dispose()
            modVoucher.tblDetail.AllowNew = allowNew
            modVoucher.tblDetail.AllowDelete = allowDelete
        End If
    End Sub

    Private Sub EDStatus()
        oVoucher.RefreshHandling(Me.cboAction)
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
        Me.grdOther.ReadOnly = Not Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
        Me.grdCharge.ReadOnly = Not Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            modVoucher.tbcCharge(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            modVoucher.tbcOther(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            index += 1
        Loop While (index <= &H1D)
        Try
            Me.colTen_vt.TextBox.Enabled = False
            Me.colSo_pn.TextBox.Enabled = False
            Me.colSo_dh.TextBox.Enabled = False
            Me.colPd_line.TextBox.Enabled = False
            Me.colPo_line.TextBox.Enabled = False
            Me.colCTen_cp.TextBox.Enabled = False
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Try
            Me.colVThue_suat.TextBox.Enabled = False
        Catch exception2 As Exception
            ProjectData.SetProjectError(exception2)
            ProjectData.ClearProjectError()
        End Try
    End Sub

    Private Sub EDTBColumns(ByVal lED As Boolean)
        Me.grdOther.ReadOnly = Not lED
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index).TextBox.Enabled = lED
            modVoucher.tbcCharge(index).TextBox.Enabled = lED
            modVoucher.tbcOther(index).TextBox.Enabled = lED
            index += 1
        Loop While (index <= &H1D)
        Try
            Me.colTen_vt.TextBox.Enabled = False
            Me.colSo_pn.TextBox.Enabled = False
            Me.colSo_dh.TextBox.Enabled = False
            Me.colPd_line.TextBox.Enabled = False
            Me.colPo_line.TextBox.Enabled = False
            Me.colCTen_cp.TextBox.Enabled = False
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Try
            Me.colVThue_suat.TextBox.Enabled = False
        Catch exception2 As Exception
            ProjectData.SetProjectError(exception2)
            ProjectData.ClearProjectError()
        End Try
        Me.EDStatus(lED)
    End Sub

    Private Sub frmRetrieveLoad(ByVal sender As Object, ByVal e As EventArgs)
        LateBinding.LateSet(sender, Nothing, "Text", New Object() {RuntimeHelpers.GetObjectValue(modVoucher.oLan.Item("047"))}, Nothing)
    End Sub

    Private Sub frmVoucher_Activated(ByVal sender As Object, ByVal e As EventArgs)
        If Not Me.isActive Then
            Me.isActive = True
            Me.InitRecords()
        End If
    End Sub

    Private Sub frmVoucher_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim flagArray As Boolean()
        Dim objArray2 As Object()
        'Me.oTitleButton.Code = modVoucher.VoucherCode
        'Me.oTitleButton.Connection = modVoucher.sysConn
        clsdrawlines.Init(Me, Me.tbDetail)
        Me.oVoucher = New clsvoucher.clsVoucher(arrControlButtons, Me, pnContent)
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
        Dim lib6 As New DirLib(Me.txtMa_dvcs, Me.lblTen_dvcs, modVoucher.sysConn, modVoucher.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", False, Me.cmdEdit)
        Dim lib4 As New DirLib(Me.txtMa_tt, Me.lblTen_tt, modVoucher.sysConn, modVoucher.appConn, "dmtt", "ma_tt", "ten_tt", "Term", "1=1", True, Me.cmdEdit)
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
        modVoucher.alOther = "ctgt30tmp"
        modVoucher.alCharge = "ctcp30tmp"
        Dim cFile As String = ("Structure\Voucher\" & modVoucher.VoucherCode)
        If Not Sys.XML2DataSet((modVoucher.dsMain), cFile) Then
            Dim tcSQL As String = ("SELECT * FROM " & modVoucher.alMaster)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alMaster, (modVoucher.dsMain))
            tcSQL = ("SELECT * FROM " & modVoucher.alDetail)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alDetail, (modVoucher.dsMain))
            tcSQL = ("SELECT * FROM " & modVoucher.alOther)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alOther, (modVoucher.dsMain))
            tcSQL = ("SELECT * FROM " & modVoucher.alCharge)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alCharge, (modVoucher.dsMain))
            Sys.DataSet2XML(modVoucher.dsMain, cFile)
        End If
        modVoucher.tblMaster.Table = modVoucher.dsMain.Tables.Item(modVoucher.alMaster)
        modVoucher.tblDetail.Table = modVoucher.dsMain.Tables.Item(modVoucher.alDetail)
        modVoucher.tblOther.Table = modVoucher.dsMain.Tables.Item(modVoucher.alOther)
        modVoucher.tblCharge.Table = modVoucher.dsMain.Tables.Item(modVoucher.alCharge)
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), (grdDetail), (modVoucher.tbsDetail), (modVoucher.tbcDetail), "PVDetail")
        oVoucher.SetMaxlengthItem(Me.grdDetail, modVoucher.alDetail, modVoucher.sysConn)
        Me.grdDetail.dvGrid = modVoucher.tblDetail
        Me.grdDetail.cFieldKey = "ma_vt"
        Me.grdDetail.AllowSorting = False
        Me.grdDetail.TableStyles.Item(0).AllowSorting = False
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblOther), (grdOther), (modVoucher.tbsOther), (modVoucher.tbcOther), "PVVAT")
        oVoucher.SetMaxlengthItem(Me.grdOther, modVoucher.alOther, modVoucher.sysConn)
        Me.grdOther.dvGrid = modVoucher.tblOther
        Me.grdOther.cFieldKey = "mau_bc"
        Me.grdOther.AllowSorting = False
        Me.grdOther.TableStyles.Item(0).AllowSorting = False
        Me.colMa_vt = GetColumn(Me.grdDetail, "ma_vt")
        Me.colDvt = GetColumn(Me.grdDetail, "Dvt")
        Me.colMa_kho = GetColumn(Me.grdDetail, "ma_kho")
        Me.colMa_vi_tri = GetColumn(Me.grdDetail, "ma_vi_tri")
        Me.colMa_lo = GetColumn(Me.grdDetail, "ma_lo")
        Me.colTk_vt = GetColumn(Me.grdDetail, "tk_vt")
        Me.colSo_luong = GetColumn(Me.grdDetail, "so_luong")
        Me.colGia0 = GetColumn(Me.grdDetail, "gia0")
        Me.colGia_nt0 = GetColumn(Me.grdDetail, "gia_nt0")
        Me.colTien0 = GetColumn(Me.grdDetail, "tien0")
        Me.colTien_nt0 = GetColumn(Me.grdDetail, "tien_nt0")
        Me.colTen_vt = GetColumn(Me.grdDetail, "ten_vt")
        Me.colSo_pn = GetColumn(Me.grdDetail, "so_pn")
        Me.colSo_dh = GetColumn(Me.grdDetail, "so_dh")
        Me.colPd_line = GetColumn(Me.grdDetail, "pd_line")
        Me.colPo_line = GetColumn(Me.grdDetail, "po_line")
        Dim sKey As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keyaccount", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Dim str2 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keycust", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Me.colVMau_bc = GetColumn(Me.grdOther, "mau_bc")
        Me.colVSo_ct0 = GetColumn(Me.grdOther, "so_ct0")
        Me.colVSo_seri0 = GetColumn(Me.grdOther, "so_seri0")
        Me.colVNgay_ct0 = GetColumn(Me.grdOther, "ngay_ct0")
        Me.colVMa_kh = GetColumn(Me.grdOther, "ma_kh")
        Me.colVMa_kho = GetColumn(Me.grdOther, "ma_kho")
        Me.colVTen_kh = GetColumn(Me.grdOther, "ten_kh")
        Me.colVDia_chi = GetColumn(Me.grdOther, "dia_chi")
        Me.colVMa_so_thue = GetColumn(Me.grdOther, "ma_so_thue")
        Me.colVTen_vt = GetColumn(Me.grdOther, "ten_vt")
        Me.colVT_tien_nt = GetColumn(Me.grdOther, "t_tien_nt")
        Me.colVT_Tien = GetColumn(Me.grdOther, "t_tien")
        Me.colVMa_thue = GetColumn(Me.grdOther, "ma_thue")
        Me.colVThue_suat = GetColumn(Me.grdOther, "thue_suat")
        Me.colVT_thue_nt = GetColumn(Me.grdOther, "t_thue_nt")
        Me.colVT_Thue = GetColumn(Me.grdOther, "t_thue")
        Me.colVTk_thue_no = GetColumn(Me.grdOther, "tk_thue_no")
        Me.colVMa_kh2 = GetColumn(Me.grdOther, "ma_kh2")
        Me.colVMa_tt = GetColumn(Me.grdOther, "ma_tt")
        Me.oVDrTaxAccount = New VoucherLibObj(Me.colVTk_thue_no, "ten_tk_thue", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", sKey, modVoucher.tblOther, Me.pnContent, False, Me.cmdEdit)
        Me.oVTaxCodeDetail = New VoucherLibObj(Me.colVMa_thue, "ten_thue", modVoucher.sysConn, modVoucher.appConn, "dmthue", "ma_thue", "ten_thue", "Tax", "1=1", modVoucher.tblOther, Me.pnContent, False, Me.cmdEdit)
        Me.oVCustomerDetail = New VoucherLibObj(Me.colVMa_kh, "ten_khtmp", modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", modVoucher.tblOther, Me.pnContent, True, Me.cmdEdit)
        Me.oTaxAuthority = New VoucherLibObj(Me.colVMa_kh2, "ten_kh2tmp", modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", modVoucher.tblOther, Me.pnContent, True, Me.cmdEdit)
        Dim obj3 As Object = New VoucherLibObj(Me.colVMa_tt, "ten_tt", modVoucher.sysConn, modVoucher.appConn, "dmtt", "ma_tt", "ten_tt", "Term", "1=1", modVoucher.tblOther, Me.pnContent, True, Me.cmdEdit)
        If (ObjectType.ObjTst(modVoucher.oOption.Item("m_po_tt"), "0", False) = 0) Then
            Me.colVMa_tt.MappingName = "H_ma_tt"
        End If
        Dim obj4 As Object = New VoucherLibObj(Me.colVMa_kho, "ten_kho", modVoucher.sysConn, modVoucher.appConn, "dmkho", "ma_kho", "ten_kho", "Site", ("ma_dvcs = '" & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("DFUnit"))) & "'"), modVoucher.tblOther, Me.pnContent, True, Me.cmdEdit)
        Me.colVThue_suat.TextBox.Enabled = False
        AddHandler Me.colVT_tien_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtVT_tien_nt_enter)
        AddHandler Me.colVT_Tien.TextBox.Enter, New EventHandler(AddressOf Me.txtVT_tien_enter)
        AddHandler Me.colVT_thue_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtVT_thue_nt_enter)
        AddHandler Me.colVT_Thue.TextBox.Enter, New EventHandler(AddressOf Me.txtVT_thue_enter)
        AddHandler Me.colVMa_thue.TextBox.Enter, New EventHandler(AddressOf Me.txtVMa_thue_enter)
        AddHandler Me.colVTen_kh.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneCustomer)
        AddHandler Me.colVDia_chi.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneCustomer)
        AddHandler Me.colVMa_so_thue.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneCustomer)
        AddHandler Me.colVMa_kh.TextBox.Validated, New EventHandler(AddressOf Me.txtVMa_kh_valid)
        AddHandler Me.colVMa_thue.TextBox.Validated, New EventHandler(AddressOf Me.txtVMa_thue_valid)
        AddHandler Me.colVT_tien_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtVT_tien_nt_valid)
        AddHandler Me.colVT_Tien.TextBox.Leave, New EventHandler(AddressOf Me.txtVT_tien_valid)
        AddHandler Me.colVT_thue_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtVT_thue_nt_valid)
        AddHandler Me.colVT_Thue.TextBox.Leave, New EventHandler(AddressOf Me.txtVT_thue_valid)
        AddHandler Me.colVTk_thue_no.TextBox.Enter, New EventHandler(AddressOf Me.txtVTk_thue_no_Enter)
        AddHandler Me.colVTk_thue_no.TextBox.Validated, New EventHandler(AddressOf Me.txtVTk_thue_no_Validated)
        AddHandler Me.colVMa_kh2.TextBox.Enter, New EventHandler(AddressOf Me.txtVMa_kh2_Enter)
        Dim clsvatform As New clsvatform(Me.colVMau_bc, modVoucher.appConn, modVoucher.sysConn, Me.pnContent, Me.cmdEdit, modVoucher.tblOther)
        Dim clsrightfield As New clsrightfield(Me.colVSo_ct0)
        Dim monumber2 As New monumber(GetColumn(Me.grdOther, "so_lsx"))
        Me.colVSo_seri0.TextBox.CharacterCasing = CharacterCasing.Upper
        Me.oSite = New VoucherKeyLibObj(Me.colMa_kho, "ten_kho", modVoucher.sysConn, modVoucher.appConn, "dmkho", "ma_kho", "ten_kho", "Site", ("ma_dvcs = '" & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("DFUnit"))) & "'"), modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
        Dim obj2 As New VoucherLibObj(Me.colTk_vt, "ten_tk_vt", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", sKey, modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
        Me.oLocation = New VoucherKeyLibObj(Me.colMa_vi_tri, "ten_vi_tri", modVoucher.sysConn, modVoucher.appConn, "dmvitri", "ma_vi_tri", "ten_vi_tri", "Location", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oLot = New VoucherKeyLibObj(Me.colMa_lo, "ten_lo", modVoucher.sysConn, modVoucher.appConn, "dmlo", "ma_lo", "ten_lo", "Lot", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oLot.FreeInput = True
        Me.oUOM = New VoucherKeyCheckLibObj(Me.colDvt, "ten_dvt", modVoucher.sysConn, modVoucher.appConn, "vdmvtqddvt", "dvt", "ten_dvt", "UOMItem", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oUOM.Cancel = True
        Me.colDvt.TextBox.CharacterCasing = CharacterCasing.Normal
        AddHandler Me.colMa_kho.TextBox.Enter, New EventHandler(AddressOf Me.WhenSiteEnter)
        AddHandler Me.colMa_kho.TextBox.Validated, New EventHandler(AddressOf Me.WhenSiteLeave)
        AddHandler Me.colMa_vi_tri.TextBox.Move, New EventHandler(AddressOf Me.WhenLocationEnter)
        AddHandler Me.colMa_lo.TextBox.Move, New EventHandler(AddressOf Me.WhenLotEnter)
        AddHandler Me.colDvt.TextBox.Move, New EventHandler(AddressOf Me.WhenUOMEnter)
        AddHandler Me.colDvt.TextBox.Validated, New EventHandler(AddressOf Me.WhenUOMLeave)
        Dim monumber As New monumber(GetColumn(Me.grdDetail, "so_lsx"))
        Dim lib2 As New DirLib(Me.txtMa_kh, Me.lblTen_kh, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", str2, False, Me.cmdEdit)
        AddHandler Me.txtMa_kh.Validated, New EventHandler(AddressOf Me.txtMa_kh_valid)
        Dim clscustomerref As New clscustomerref(modVoucher.appConn, Me.txtMa_kh, Me.txtOng_ba, modVoucher.VoucherCode, Me.oVoucher)
        Dim lib5 As New DirLib(Me.txtMa_gd, Me.lblTen_gd, modVoucher.sysConn, modVoucher.appConn, "dmmagd", "ma_gd", "ten_gd", "VCTransCode", ("ma_ct = '" & modVoucher.VoucherCode & "'"), False, Me.cmdEdit)
        Dim oTk As New DirLib(Me.txtTk, Me.lblTen_tk, modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", sKey, False, Me.cmdEdit)
        AddHandler Me.txtTk.Validated, New EventHandler(AddressOf Me.txtTk_Validated)
        Me.oInvItemDetail = New VoucherLibObj(Me.colMa_vt, "ten_vt", modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        VoucherLibObj.oClassMsg = oVoucher.oClassMsg
        Me.oInvItemDetail.Colkey = True
        VoucherLibObj.dvDetail = modVoucher.tblDetail
        Me.oInvItemDetail.oTabSelectedWhenCancel = Me.tpgOther
        AddHandler Me.colMa_vt.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKey)
        AddHandler Me.colMa_vt.TextBox.Validated, New EventHandler(AddressOf Me.WhenItemLeave)
        AddHandler Me.colVMau_bc.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKeyVAT)
        AddHandler Me.colVMau_bc.TextBox.Validated, New EventHandler(AddressOf Me.txtVMau_bc_Validated)
        Try
            oVoucher.AddValidFields(Me.grdDetail, modVoucher.tblDetail, Me.pnContent, Me.cmdEdit)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Try
            oVoucher.AddValidFields(Me.grdOther, modVoucher.tblOther, Me.pnContent, Me.cmdEdit)
        Catch exception2 As Exception
            ProjectData.SetProjectError(exception2)
            ProjectData.ClearProjectError()
        End Try
        Me.colTen_vt.TextBox.Enabled = False
        Me.colSo_pn.TextBox.Enabled = False
        Me.colSo_dh.TextBox.Enabled = False
        Me.colPd_line.TextBox.Enabled = False
        Me.colPo_line.TextBox.Enabled = False
        oVoucher.HideFields(Me.grdDetail)
        oVoucher.HideFields(Me.grdOther)
        ChangeFormatColumn(Me.colSo_luong, StringType.FromObject(modVoucher.oVar.Item("m_ip_sl")))
        AddHandler Me.colSo_luong.TextBox.Leave, New EventHandler(AddressOf Me.txtSo_luong_valid)
        AddHandler Me.colGia_nt0.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_nt0_valid)
        AddHandler Me.colGia0.TextBox.Leave, New EventHandler(AddressOf Me.txtGia0_valid)
        AddHandler Me.colTien_nt0.TextBox.Leave, New EventHandler(AddressOf Me.txtTien_nt0_valid)
        AddHandler Me.colTien0.TextBox.Leave, New EventHandler(AddressOf Me.txtTien0_valid)
        AddHandler Me.colSo_luong.TextBox.Enter, New EventHandler(AddressOf Me.txtSo_luong_enter)
        AddHandler Me.colGia_nt0.TextBox.Enter, New EventHandler(AddressOf Me.txtGia_nt0_enter)
        AddHandler Me.colGia0.TextBox.Enter, New EventHandler(AddressOf Me.txtGia0_enter)
        AddHandler Me.colTien_nt0.TextBox.Enter, New EventHandler(AddressOf Me.txtTien_nt0_enter)
        AddHandler Me.colTien0.TextBox.Enter, New EventHandler(AddressOf Me.txtTien0_enter)
        AddHandler Me.colTk_vt.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneInputItemAccount)
        Dim objectValue As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldchar", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim obj7 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim obj6 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fielddate", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim index As Integer = 0
        Do
            Dim args As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj7)}
            flagArray = New Boolean() {True}
            If flagArray(0) Then
                obj7 = RuntimeHelpers.GetObjectValue(args(0))
            End If
            If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", args, Nothing, flagArray)), modVoucher.tbcDetail(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                modVoucher.tbcDetail(index).NullText = "0"
            Else
                objArray2 = New Object() {RuntimeHelpers.GetObjectValue(obj6)}
                flagArray = New Boolean() {True}
                If flagArray(0) Then
                    obj6 = RuntimeHelpers.GetObjectValue(objArray2(0))
                End If
                If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", objArray2, Nothing, flagArray)), modVoucher.tbcDetail(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
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
        Dim strSQL As String = "SELECT dbo.ff_GetSQLFieldsType('ctgt30', 'numeric') + '#' + dbo.ff_GetSQLFieldsType('ctgt30', 'char') + dbo.ff_GetSQLFieldsType('ctgt30', 'nchar') + '#' + dbo.ff_GetSQLFieldsType('ctgt30', 'smalldatetime') AS fields"
        Dim cString As String = StringType.FromObject(Ini.GetIniValue((modVoucher.appConn), strSQL, "InputVAT", "FieldList", "Ini\Value"))
        objectValue = Fox.GetWordNum(cString, 2, "#"c)
        obj7 = Fox.GetWordNum(cString, 1, "#"c)
        obj6 = Fox.GetWordNum(cString, 3, "#"c)
        index = 0
        Do
            objArray2 = New Object() {RuntimeHelpers.GetObjectValue(obj7)}
            flagArray = New Boolean() {True}
            If flagArray(0) Then
                obj7 = RuntimeHelpers.GetObjectValue(objArray2(0))
            End If
            If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", objArray2, Nothing, flagArray)), modVoucher.tbcOther(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                modVoucher.tbcOther(index).NullText = "0"
            Else
                objArray2 = New Object() {RuntimeHelpers.GetObjectValue(obj6)}
                flagArray = New Boolean() {True}
                If flagArray(0) Then
                    obj6 = RuntimeHelpers.GetObjectValue(objArray2(0))
                End If
                If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", objArray2, Nothing, flagArray)), modVoucher.tbcOther(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                    modVoucher.tbcOther(index).NullText = StringType.FromObject(Fox.GetEmptyDate)
                Else
                    modVoucher.tbcOther(index).NullText = ""
                End If
            End If
            If (index <> 0) Then
                AddHandler modVoucher.tbcOther(index).TextBox.Enter, New EventHandler(AddressOf Me.txtE_Enter)
            End If
            index += 1
        Loop While (index <= &H1D)
        Dim menu2 As New ContextMenu
        Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItem), Shortcut.F4)
        Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItem), Shortcut.F8)
        Dim item5 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("207")), New EventHandler(AddressOf Me.LotItem), Shortcut.F9)
        menu2.MenuItems.Add(item)
        menu2.MenuItems.Add(item2)
        menu2.MenuItems.Add(New MenuItem("-"))
        menu2.MenuItems.Add(item5)
        Dim menu As New ContextMenu
        Dim item3 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItemVAT), Shortcut.F4)
        Dim item4 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItemVAT), Shortcut.F8)
        Dim item6 As New MenuItem("Cập nhật mỗi mặt hàng một hóa đơn", New EventHandler(AddressOf Me.AppendVATDetail), Shortcut.F9)
        menu.MenuItems.Add(item3)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item4)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item6)
        Me.InitContextMenu()
        Me.txtKeyPress.Left = (-100 - Me.txtKeyPress.Width)
        Me.grdDetail.ContextMenu = menu2
        Me.grdOther.ContextMenu = menu
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
        aGrid.Add(Me.grdCharge, "grdCharge", Nothing, Nothing)
        aGrid.Add(Me.grdOther, "grdOther", Nothing, Nothing)
        aGrid = Nothing
        Me.oSecurity.Init()
        Me.oSecurity.Invisible()
        Me.oSecurity.SetReadOnly()
        Me.grdCharge.ReadOnly = True
        Me.InitCharge()
        Me.colCTen_cp.TextBox.Enabled = False
        Me.InitInventory()
    End Sub

    Private Function GetIDItem(ByVal tblItem As DataView, ByVal sStart As String) As String
        Dim str2 As String = (sStart & "00")
        Dim num2 As Integer = (tblItem.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num2)
            If (Not IsDBNull(tblItem.Item(i).Item("stt_rec0")) AndAlso (ObjectType.ObjTst(tblItem.Item(i).Item("stt_rec0"), str2, False) > 0)) Then
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

    Private Sub grdCharge_CurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdCharge.CurrentCellChanged
        If Not Me.lAllowCurrentCellChanged Then
            Return
        End If
        Dim currentRowIndex As Integer = grdCharge.CurrentRowIndex
        Dim columnNumber As Integer = grdCharge.CurrentCell.ColumnNumber
        Dim oValue As String = Strings.Trim(StringType.FromObject(grdCharge.Item(currentRowIndex, columnNumber)))
        Dim sLeft As String = grdCharge.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName.ToUpper.ToString
        Dim oOldObject As Object
        If (StringType.StrCmp(sLeft, "TIEN_CP_NT", False) = 0) Then
            oOldObject = Me.noldCTien_cp_nt
            SetOldValue((oOldObject), oValue)
            Me.noldCTien_cp_nt = DecimalType.FromObject(oOldObject)
        End If
        If (StringType.StrCmp(sLeft, "TIEN_CP", False) = 0) Then
            oOldObject = Me.noldCTien_cp
            SetOldValue((oOldObject), oValue)
            Me.noldCTien_cp = DecimalType.FromObject(oOldObject)
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
        Dim cOldSite As Object

        If (StringType.StrCmp(sLeft, "MA_KHO", False) = 0) Then
            cOldSite = Me.cOldSite
            SetOldValue((cOldSite), oValue)
            Me.cOldSite = StringType.FromObject(cOldSite)
            Return
        End If
        If (StringType.StrCmp(sLeft, "SO_LUONG", False) = 0) Then
            cOldSite = Me.noldSo_luong
            SetOldValue((cOldSite), oValue)
            Me.noldSo_luong = DecimalType.FromObject(cOldSite)
            Return
        End If
        If (StringType.StrCmp(sLeft, "GIA_NT0", False) = 0) Then
            cOldSite = Me.noldGia_nt0
            SetOldValue((cOldSite), oValue)
            Me.noldGia_nt0 = DecimalType.FromObject(cOldSite)
            Return
        End If
        If (StringType.StrCmp(sLeft, "GIA0", False) = 0) Then
            cOldSite = Me.noldGia0
            SetOldValue((cOldSite), oValue)
            Me.noldGia0 = DecimalType.FromObject(cOldSite)
            Return
        End If
        If (StringType.StrCmp(sLeft, "TIEN_NT0", False) <> 0) Then
            cOldSite = Me.noldTien_nt0
            SetOldValue((cOldSite), oValue)
            Me.noldTien_nt0 = DecimalType.FromObject(cOldSite)
            Return
        End If
        If (StringType.StrCmp(sLeft, "TIEN0", False) = 0) Then
            cOldSite = Me.noldTien0
            SetOldValue((cOldSite), oValue)
            Me.noldTien0 = DecimalType.FromObject(cOldSite)
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

    Private Sub grdOther_CurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdOther.CurrentCellChanged
        If Not Me.lAllowCurrentCellChanged Then
            Return
        End If
        Dim currentRowIndex As Integer = grdOther.CurrentRowIndex
        Dim columnNumber As Integer = grdOther.CurrentCell.ColumnNumber
        If IsDBNull(grdOther.Item(currentRowIndex, columnNumber)) Then
            Return
        End If
        Dim oValue As String = Strings.Trim(StringType.FromObject(grdOther.Item(currentRowIndex, columnNumber)))
        Dim sLeft As String = grdOther.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName.ToUpper.ToString
        Dim oOldObject As Object
        If (StringType.StrCmp(sLeft, "MA_THUE", False) = 0) Then
            oOldObject = Me.coldVMa_thue
            SetOldValue((oOldObject), oValue)
            Me.coldVMa_thue = StringType.FromObject(oOldObject)
            Return
        End If
        If (StringType.StrCmp(sLeft, "TK_THUE_NO", False) = 0) Then
            oOldObject = Me.coldVTk_thue_no
            SetOldValue((oOldObject), oValue)
            Me.coldVTk_thue_no = StringType.FromObject(oOldObject)
            Return
        End If
        If (StringType.StrCmp(sLeft, "T_THUE_NT", False) = 0) Then
            oOldObject = Me.noldVT_Thue_nt
            SetOldValue((oOldObject), oValue)
            Me.noldVT_Thue_nt = DecimalType.FromObject(oOldObject)
            Return
        End If
        If (StringType.StrCmp(sLeft, "T_THUE", False) = 0) Then
            oOldObject = Me.noldVT_Thue
            SetOldValue((oOldObject), oValue)
            Me.noldVT_Thue = DecimalType.FromObject(oOldObject)
            Return
        End If
        If (StringType.StrCmp(sLeft, "T_TIEN_NT", False) = 0) Then
            oOldObject = Me.noldVT_tien_nt
            SetOldValue((oOldObject), oValue)
            Me.noldVT_tien_nt = DecimalType.FromObject(oOldObject)
            Return
        End If
        If (StringType.StrCmp(sLeft, "T_TIEN", False) = 0) Then
            oOldObject = Me.noldVT_tien
            SetOldValue((oOldObject), oValue)
            Me.noldVT_tien = DecimalType.FromObject(oOldObject)
        End If
    End Sub

    Private Sub grdRetrieveMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num).Item("stt_rec")), "'")
        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
    End Sub

    Private Sub InitCharge()
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblCharge), (grdCharge), (modVoucher.tbsCharge), (modVoucher.tbcCharge), "PVCharge")
        oVoucher.SetMaxlengthItem(Me.grdCharge, modVoucher.alCharge, modVoucher.sysConn)
        Me.grdCharge.dvGrid = modVoucher.tblCharge
        Me.grdCharge.cFieldKey = "ma_cp"
        Me.grdCharge.AllowSorting = False
        Me.grdCharge.TableStyles.Item(0).AllowSorting = False
        Me.colCMa_cp = GetColumn(Me.grdCharge, "ma_cp")
        Me.colCTen_cp = GetColumn(Me.grdCharge, "ten_cp")
        Me.colCTien_cp_nt = GetColumn(Me.grdCharge, "tien_cp_nt")
        Me.colCTien_cp = GetColumn(Me.grdCharge, "tien_cp")
        Dim obj2 As New VoucherLibObj(Me.colCMa_cp, "ten_cp", modVoucher.sysConn, modVoucher.appConn, "dmcp", "ma_loai", "ten_cp", "Charge", ("(ma_ct = '' OR ma_ct = '" & modVoucher.VoucherCode & "')"), modVoucher.tblCharge, Me.pnContent, True, Me.cmdEdit)
        Dim str As String = "tien_cp_nt, tien_cp"
        Dim index As Integer = 0
        Do
            If (Strings.InStr(Strings.LCase(str), modVoucher.tbcCharge(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                modVoucher.tbcCharge(index).NullText = "0"
            Else
                modVoucher.tbcCharge(index).NullText = ""
            End If
            If (index <> 0) Then
                AddHandler modVoucher.tbcCharge(index).TextBox.Enter, New EventHandler(AddressOf Me.txtC_Enter)
            End If
            index += 1
        Loop While (index <= &H1D)
        Dim menu As New ContextMenu
        Dim item4 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("203")), New EventHandler(AddressOf Me.EditAllocatedCharge), Shortcut.F3)
        Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItemCharge), Shortcut.F4)
        Dim item3 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItemCharge), Shortcut.F8)
        Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("204")), New EventHandler(AddressOf Me.AllocateCharge), Shortcut.F9)
        menu.MenuItems.Add(item4)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item)
        menu.MenuItems.Add(item3)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item2)
        Me.grdCharge.ContextMenu = menu
        AddHandler Me.colCTien_cp_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtCTien_cp_nt_enter)
        AddHandler Me.colCTien_cp.TextBox.Enter, New EventHandler(AddressOf Me.txtCTien_cp_enter)
        AddHandler Me.colCTien_cp_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtCTien_cp_nt_valid)
        AddHandler Me.colCTien_cp.TextBox.Leave, New EventHandler(AddressOf Me.txtCTien_cp_valid)
        AddHandler Me.colCMa_cp.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKeyCharge)
        AddHandler Me.colCMa_cp.TextBox.Validated, New EventHandler(AddressOf Me.WhenChargeLeave)
    End Sub

    Private Sub InitContextMenu()
        Dim menu As New ContextMenu
        Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("205")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F5)
        Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("206")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F6)
        Dim item4 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("051")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F7)
        menu.MenuItems.Add(item)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item2)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item4)
        Me.ContextMenu = menu
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) = 0) Then
            menu.MenuItems.Item(1).Visible = False
            item2.Enabled = False
            item2.Visible = False
        End If
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

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.cmdSave = New Button
        Me.cmdNew = New Button
        Me.cmdPrint = New Button
        Me.cmdEdit = New Button
        Me.cmdDelete = New Button
        Me.cmdView = New Button
        Me.cmdSearch = New Button
        Me.cmdClose = New Button
        Me.cmdOption = New Button
        Me.cmdTop = New Button
        Me.cmdPrev = New Button
        Me.cmdNext = New Button
        Me.cmdBottom = New Button
        Me.lblMa_dvcs = New Label
        Me.txtMa_dvcs = New TextBox
        Me.lblTen_dvcs = New Label
        Me.lblSo_ct = New Label
        Me.txtSo_ct = New TextBox
        Me.txtNgay_lct = New txtDate
        Me.txtTy_gia = New txtNumeric
        Me.lblNgay_lct = New Label
        Me.lblNgay_ct = New Label
        Me.lblTy_gia = New Label
        Me.txtNgay_ct = New txtDate
        Me.cmdMa_nt = New Button
        Me.tbDetail = New System.Windows.Forms.TabControl
        Me.tpgDetail = New System.Windows.Forms.TabPage
        Me.grdDetail = New clsgrid
        Me.tbgCharge = New System.Windows.Forms.TabPage
        Me.grdCharge = New clsgrid
        Me.tpgOther = New System.Windows.Forms.TabPage
        Me.grdOther = New clsgrid
        Me.txtT_tien0 = New txtNumeric
        Me.txtT_thue = New txtNumeric
        Me.txtT_thue_nt = New txtNumeric
        Me.txtT_tien_nt0 = New txtNumeric
        Me.txtStatus = New TextBox
        Me.lblStatus = New Label
        Me.lblStatusMess = New Label
        Me.txtKeyPress = New TextBox
        Me.cboStatus = New System.Windows.Forms.ComboBox
        Me.cboAction = New System.Windows.Forms.ComboBox
        Me.lblAction = New Label
        Me.lblMa_kh = New Label
        Me.txtMa_kh = New TextBox
        Me.lblTen_kh = New Label
        Me.lblOng_ba = New Label
        Me.txtOng_ba = New TextBox
        Me.lblTk = New Label
        Me.txtTk = New TextBox
        Me.lblTen_tk = New Label
        Me.txtT_tt_nt = New txtNumeric
        Me.txtT_tt = New txtNumeric
        Me.lblTotal = New Label
        Me.lblTien_thue = New Label
        Me.lblTien_tt = New Label
        Me.lblMa_tt = New Label
        Me.txtMa_tt = New TextBox
        Me.lblTen_tt = New Label
        Me.lblTen = New Label
        Me.txtSo_ct0 = New TextBox
        Me.lblSo_hd = New Label
        Me.txtNgay_ct0 = New txtDate
        Me.lblNgay_hd = New Label
        Me.txtDien_giai = New TextBox
        Me.Label1 = New Label
        Me.txtMa_gd = New TextBox
        Me.lblMa_gd = New Label
        Me.lblTen_gd = New Label
        Me.lvlT_cp = New Label
        Me.txtT_cp_nt = New txtNumeric
        Me.txtT_cp = New txtNumeric
        Me.txtT_so_luong = New txtNumeric
        Me.chkGia_thue_yn = New System.Windows.Forms.CheckBox
        Me.txtLoai_ct = New TextBox
        Me.lblSo_seri = New Label
        Me.txtSo_seri0 = New TextBox
        Me.tbDetail.SuspendLayout()
        Me.tpgDetail.SuspendLayout()
        CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbgCharge.SuspendLayout()
        CType(Me.grdCharge, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpgOther.SuspendLayout()
        CType(Me.grdOther, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSave.Location = New System.Drawing.Point(2, 428)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(60, 23)
        Me.cmdSave.TabIndex = 28
        Me.cmdSave.Tag = "CB01"
        Me.cmdSave.Text = "Luu"
        '
        'cmdNew
        '
        Me.cmdNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Location = New System.Drawing.Point(62, 428)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.Size = New System.Drawing.Size(60, 23)
        Me.cmdNew.TabIndex = 29
        Me.cmdNew.Tag = "CB02"
        Me.cmdNew.Text = "Moi"
        '
        'cmdPrint
        '
        Me.cmdPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrint.Location = New System.Drawing.Point(122, 428)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.Size = New System.Drawing.Size(60, 23)
        Me.cmdPrint.TabIndex = 30
        Me.cmdPrint.Tag = "CB03"
        Me.cmdPrint.Text = "In ctu"
        '
        'cmdEdit
        '
        Me.cmdEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Location = New System.Drawing.Point(182, 428)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(60, 23)
        Me.cmdEdit.TabIndex = 31
        Me.cmdEdit.Tag = "CB04"
        Me.cmdEdit.Text = "Sua"
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Location = New System.Drawing.Point(242, 428)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(60, 23)
        Me.cmdDelete.TabIndex = 32
        Me.cmdDelete.Tag = "CB05"
        Me.cmdDelete.Text = "Xoa"
        '
        'cmdView
        '
        Me.cmdView.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Location = New System.Drawing.Point(302, 428)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.Size = New System.Drawing.Size(60, 23)
        Me.cmdView.TabIndex = 33
        Me.cmdView.Tag = "CB06"
        Me.cmdView.Text = "Xem"
        '
        'cmdSearch
        '
        Me.cmdSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Location = New System.Drawing.Point(362, 428)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(60, 23)
        Me.cmdSearch.TabIndex = 34
        Me.cmdSearch.Tag = "CB07"
        Me.cmdSearch.Text = "Tim"
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Location = New System.Drawing.Point(422, 428)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(60, 23)
        Me.cmdClose.TabIndex = 35
        Me.cmdClose.Tag = "CB08"
        Me.cmdClose.Text = "Quay ra"
        '
        'cmdOption
        '
        Me.cmdOption.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOption.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOption.Location = New System.Drawing.Point(543, 428)
        Me.cmdOption.Name = "cmdOption"
        Me.cmdOption.Size = New System.Drawing.Size(20, 23)
        Me.cmdOption.TabIndex = 36
        Me.cmdOption.TabStop = False
        Me.cmdOption.Tag = "CB09"
        '
        'cmdTop
        '
        Me.cmdTop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTop.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTop.Location = New System.Drawing.Point(562, 428)
        Me.cmdTop.Name = "cmdTop"
        Me.cmdTop.Size = New System.Drawing.Size(20, 23)
        Me.cmdTop.TabIndex = 37
        Me.cmdTop.TabStop = False
        Me.cmdTop.Tag = "CB10"
        '
        'cmdPrev
        '
        Me.cmdPrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdPrev.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrev.Location = New System.Drawing.Point(581, 428)
        Me.cmdPrev.Name = "cmdPrev"
        Me.cmdPrev.Size = New System.Drawing.Size(20, 23)
        Me.cmdPrev.TabIndex = 38
        Me.cmdPrev.TabStop = False
        Me.cmdPrev.Tag = "CB11"
        '
        'cmdNext
        '
        Me.cmdNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdNext.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNext.Location = New System.Drawing.Point(600, 428)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.Size = New System.Drawing.Size(20, 23)
        Me.cmdNext.TabIndex = 39
        Me.cmdNext.TabStop = False
        Me.cmdNext.Tag = "CB12"
        '
        'cmdBottom
        '
        Me.cmdBottom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBottom.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBottom.Location = New System.Drawing.Point(619, 428)
        Me.cmdBottom.Name = "cmdBottom"
        Me.cmdBottom.Size = New System.Drawing.Size(20, 23)
        Me.cmdBottom.TabIndex = 40
        Me.cmdBottom.TabStop = False
        Me.cmdBottom.Tag = "CB13"
        '
        'lblMa_dvcs
        '
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New System.Drawing.Point(272, 456)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New System.Drawing.Size(46, 16)
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
        Me.lblTen_dvcs.Size = New System.Drawing.Size(87, 16)
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
        Me.lblSo_ct.Size = New System.Drawing.Size(36, 16)
        Me.lblSo_ct.TabIndex = 16
        Me.lblSo_ct.Tag = "L009"
        Me.lblSo_ct.Text = "So ctu"
        '
        'txtSo_ct
        '
        Me.txtSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSo_ct.BackColor = System.Drawing.Color.White
        Me.txtSo_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_ct.Location = New System.Drawing.Point(538, 5)
        Me.txtSo_ct.Name = "txtSo_ct"
        Me.txtSo_ct.TabIndex = 8
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
        Me.txtNgay_lct.TabIndex = 9
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
        Me.txtTy_gia.TabIndex = 12
        Me.txtTy_gia.Tag = "FNCF"
        Me.txtTy_gia.Text = "m_ip_tg"
        Me.txtTy_gia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTy_gia.Value = 0
        '
        'lblNgay_lct
        '
        Me.lblNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNgay_lct.AutoSize = True
        Me.lblNgay_lct.Location = New System.Drawing.Point(438, 28)
        Me.lblNgay_lct.Name = "lblNgay_lct"
        Me.lblNgay_lct.Size = New System.Drawing.Size(61, 16)
        Me.lblNgay_lct.TabIndex = 20
        Me.lblNgay_lct.Tag = "L010"
        Me.lblNgay_lct.Text = "Ngay lap ct"
        '
        'lblNgay_ct
        '
        Me.lblNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNgay_ct.AutoSize = True
        Me.lblNgay_ct.Location = New System.Drawing.Point(438, 49)
        Me.lblNgay_ct.Name = "lblNgay_ct"
        Me.lblNgay_ct.Size = New System.Drawing.Size(83, 16)
        Me.lblNgay_ct.TabIndex = 21
        Me.lblNgay_ct.Tag = "L011"
        Me.lblNgay_ct.Text = "Ngay hach toan"
        '
        'lblTy_gia
        '
        Me.lblTy_gia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTy_gia.AutoSize = True
        Me.lblTy_gia.Location = New System.Drawing.Point(438, 70)
        Me.lblTy_gia.Name = "lblTy_gia"
        Me.lblTy_gia.Size = New System.Drawing.Size(35, 16)
        Me.lblTy_gia.TabIndex = 22
        Me.lblTy_gia.Tag = "L012"
        Me.lblTy_gia.Text = "Ty gia"
        '
        'txtNgay_ct
        '
        Me.txtNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNgay_ct.BackColor = System.Drawing.Color.White
        Me.txtNgay_ct.Location = New System.Drawing.Point(538, 47)
        Me.txtNgay_ct.MaxLength = 10
        Me.txtNgay_ct.Name = "txtNgay_ct"
        Me.txtNgay_ct.TabIndex = 10
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
        Me.cmdMa_nt.TabIndex = 11
        Me.cmdMa_nt.TabStop = False
        Me.cmdMa_nt.Tag = "FCCFCMDDF"
        Me.cmdMa_nt.Text = "VND"
        '
        'tbDetail
        '
        Me.tbDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDetail.Controls.Add(Me.tpgDetail)
        Me.tbDetail.Controls.Add(Me.tbgCharge)
        Me.tbDetail.Controls.Add(Me.tpgOther)
        Me.tbDetail.Location = New System.Drawing.Point(2, 160)
        Me.tbDetail.Name = "tbDetail"
        Me.tbDetail.SelectedIndex = 0
        Me.tbDetail.Size = New System.Drawing.Size(638, 168)
        Me.tbDetail.TabIndex = 16
        '
        'tpgDetail
        '
        Me.tpgDetail.BackColor = System.Drawing.SystemColors.Control
        Me.tpgDetail.Controls.Add(Me.grdDetail)
        Me.tpgDetail.Location = New System.Drawing.Point(4, 22)
        Me.tpgDetail.Name = "tpgDetail"
        Me.tpgDetail.Size = New System.Drawing.Size(630, 142)
        Me.tpgDetail.TabIndex = 0
        Me.tpgDetail.Tag = "L016"
        Me.tpgDetail.Text = "Chung tu"
        '
        'grdDetail
        '
        Me.grdDetail.Cell_EnableRaisingEvents = False
        Me.grdDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdDetail.BackgroundColor = System.Drawing.Color.White
        Me.grdDetail.CaptionBackColor = System.Drawing.SystemColors.Control
        Me.grdDetail.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdDetail.CaptionForeColor = System.Drawing.Color.Black
        Me.grdDetail.CaptionText = "F4 - Them, F8 - Xoa, F9 - Cap nhat danh muc lo"
        Me.grdDetail.DataMember = ""
        Me.grdDetail.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdDetail.Location = New System.Drawing.Point(-1, -1)
        Me.grdDetail.Name = "grdDetail"
        Me.grdDetail.Size = New System.Drawing.Size(633, 143)
        Me.grdDetail.TabIndex = 0
        Me.grdDetail.Tag = "L020CF"
        '
        'tbgCharge
        '
        Me.tbgCharge.Controls.Add(Me.grdCharge)
        Me.tbgCharge.Location = New System.Drawing.Point(4, 22)
        Me.tbgCharge.Name = "tbgCharge"
        Me.tbgCharge.Size = New System.Drawing.Size(630, 142)
        Me.tbgCharge.TabIndex = 2
        Me.tbgCharge.Tag = "L034"
        Me.tbgCharge.Text = "Chi phi"
        '
        'grdCharge
        '
        Me.grdCharge.Cell_EnableRaisingEvents = False
        Me.grdCharge.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdCharge.BackgroundColor = System.Drawing.Color.White
        Me.grdCharge.CaptionBackColor = System.Drawing.SystemColors.Control
        Me.grdCharge.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdCharge.CaptionForeColor = System.Drawing.Color.Black
        Me.grdCharge.CaptionText = "Nhap chi phi: F4 - Them dong, F8 - Xoa dong"
        Me.grdCharge.DataMember = ""
        Me.grdCharge.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdCharge.Location = New System.Drawing.Point(-1, -1)
        Me.grdCharge.Name = "grdCharge"
        Me.grdCharge.Size = New System.Drawing.Size(633, 143)
        Me.grdCharge.TabIndex = 1
        Me.grdCharge.Tag = "L035"
        '
        'tpgOther
        '
        Me.tpgOther.Controls.Add(Me.grdOther)
        Me.tpgOther.Location = New System.Drawing.Point(4, 22)
        Me.tpgOther.Name = "tpgOther"
        Me.tpgOther.Size = New System.Drawing.Size(630, 142)
        Me.tpgOther.TabIndex = 1
        Me.tpgOther.Tag = "L017"
        Me.tpgOther.Text = "Thue GTGT dau vao"
        '
        'grdOther
        '
        Me.grdOther.Cell_EnableRaisingEvents = False
        Me.grdOther.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdOther.BackgroundColor = System.Drawing.Color.White
        Me.grdOther.CaptionBackColor = System.Drawing.SystemColors.Control
        Me.grdOther.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdOther.CaptionForeColor = System.Drawing.Color.Black
        Me.grdOther.CaptionText = "Nhap chung tu GTGT: F4 - Them dong, F8 - Xoa dong"
        Me.grdOther.DataMember = ""
        Me.grdOther.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdOther.Location = New System.Drawing.Point(-1, -1)
        Me.grdOther.Name = "grdOther"
        Me.grdOther.Size = New System.Drawing.Size(633, 143)
        Me.grdOther.TabIndex = 0
        Me.grdOther.Tag = "L021"
        '
        'txtT_tien0
        '
        Me.txtT_tien0.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien0.BackColor = System.Drawing.Color.White
        Me.txtT_tien0.Enabled = False
        Me.txtT_tien0.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien0.Format = "m_ip_tien"
        Me.txtT_tien0.Location = New System.Drawing.Point(538, 338)
        Me.txtT_tien0.MaxLength = 10
        Me.txtT_tien0.Name = "txtT_tien0"
        Me.txtT_tien0.TabIndex = 20
        Me.txtT_tien0.Tag = "FN"
        Me.txtT_tien0.Text = "m_ip_tien"
        Me.txtT_tien0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien0.Value = 0
        '
        'txtT_thue
        '
        Me.txtT_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_thue.BackColor = System.Drawing.Color.White
        Me.txtT_thue.Enabled = False
        Me.txtT_thue.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue.Format = "m_ip_tien"
        Me.txtT_thue.Location = New System.Drawing.Point(538, 380)
        Me.txtT_thue.MaxLength = 10
        Me.txtT_thue.Name = "txtT_thue"
        Me.txtT_thue.TabIndex = 22
        Me.txtT_thue.Tag = "FN"
        Me.txtT_thue.Text = "m_ip_tien"
        Me.txtT_thue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue.Value = 0
        '
        'txtT_thue_nt
        '
        Me.txtT_thue_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_thue_nt.BackColor = System.Drawing.Color.White
        Me.txtT_thue_nt.Enabled = False
        Me.txtT_thue_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue_nt.Format = "m_ip_tien_nt"
        Me.txtT_thue_nt.Location = New System.Drawing.Point(437, 380)
        Me.txtT_thue_nt.MaxLength = 13
        Me.txtT_thue_nt.Name = "txtT_thue_nt"
        Me.txtT_thue_nt.TabIndex = 21
        Me.txtT_thue_nt.Tag = "FN"
        Me.txtT_thue_nt.Text = "m_ip_tien_nt"
        Me.txtT_thue_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue_nt.Value = 0
        '
        'txtT_tien_nt0
        '
        Me.txtT_tien_nt0.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien_nt0.BackColor = System.Drawing.Color.White
        Me.txtT_tien_nt0.Enabled = False
        Me.txtT_tien_nt0.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien_nt0.Format = "m_ip_tien_nt"
        Me.txtT_tien_nt0.Location = New System.Drawing.Point(437, 338)
        Me.txtT_tien_nt0.MaxLength = 13
        Me.txtT_tien_nt0.Name = "txtT_tien_nt0"
        Me.txtT_tien_nt0.TabIndex = 19
        Me.txtT_tien_nt0.Tag = "FN"
        Me.txtT_tien_nt0.Text = "m_ip_tien_nt"
        Me.txtT_tien_nt0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien_nt0.Value = 0
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
        Me.lblStatus.Size = New System.Drawing.Size(55, 16)
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
        Me.lblStatusMess.Size = New System.Drawing.Size(199, 16)
        Me.lblStatusMess.TabIndex = 42
        Me.lblStatusMess.Tag = ""
        Me.lblStatusMess.Text = "1 - Ghi vao SC, 0 - Chua ghi vao so cai"
        Me.lblStatusMess.Visible = False
        '
        'txtKeyPress
        '
        Me.txtKeyPress.AutoSize = False
        Me.txtKeyPress.Location = New System.Drawing.Point(408, 152)
        Me.txtKeyPress.Name = "txtKeyPress"
        Me.txtKeyPress.Size = New System.Drawing.Size(10, 10)
        Me.txtKeyPress.TabIndex = 15
        Me.txtKeyPress.Text = ""
        '
        'cboStatus
        '
        Me.cboStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStatus.BackColor = System.Drawing.Color.White
        Me.cboStatus.Enabled = False
        Me.cboStatus.Location = New System.Drawing.Point(498, 89)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New System.Drawing.Size(140, 21)
        Me.cboStatus.TabIndex = 13
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
        Me.cboAction.TabIndex = 14
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
        Me.lblAction.Size = New System.Drawing.Size(29, 16)
        Me.lblAction.TabIndex = 33
        Me.lblAction.Tag = ""
        Me.lblAction.Text = "Xu ly"
        '
        'lblMa_kh
        '
        Me.lblMa_kh.AutoSize = True
        Me.lblMa_kh.Location = New System.Drawing.Point(2, 7)
        Me.lblMa_kh.Name = "lblMa_kh"
        Me.lblMa_kh.Size = New System.Drawing.Size(53, 16)
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
        Me.txtMa_kh.TabIndex = 0
        Me.txtMa_kh.Tag = "FCNBCF"
        Me.txtMa_kh.Text = "TXTMA_KH"
        '
        'lblTen_kh
        '
        Me.lblTen_kh.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_kh.Location = New System.Drawing.Point(192, 7)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(233, 12)
        Me.lblTen_kh.TabIndex = 36
        Me.lblTen_kh.Tag = "FCRF"
        Me.lblTen_kh.Text = "Ten Khach"
        '
        'lblOng_ba
        '
        Me.lblOng_ba.AutoSize = True
        Me.lblOng_ba.Location = New System.Drawing.Point(2, 28)
        Me.lblOng_ba.Name = "lblOng_ba"
        Me.lblOng_ba.Size = New System.Drawing.Size(59, 16)
        Me.lblOng_ba.TabIndex = 37
        Me.lblOng_ba.Tag = "L003"
        Me.lblOng_ba.Text = "Nguoi mua"
        '
        'txtOng_ba
        '
        Me.txtOng_ba.BackColor = System.Drawing.Color.White
        Me.txtOng_ba.Location = New System.Drawing.Point(88, 26)
        Me.txtOng_ba.Name = "txtOng_ba"
        Me.txtOng_ba.TabIndex = 1
        Me.txtOng_ba.Tag = "FCCF"
        Me.txtOng_ba.Text = "txtOng_ba"
        '
        'lblTk
        '
        Me.lblTk.AutoSize = True
        Me.lblTk.Location = New System.Drawing.Point(2, 70)
        Me.lblTk.Name = "lblTk"
        Me.lblTk.Size = New System.Drawing.Size(69, 16)
        Me.lblTk.TabIndex = 39
        Me.lblTk.Tag = "L004"
        Me.lblTk.Text = "Tai khoan co"
        '
        'txtTk
        '
        Me.txtTk.BackColor = System.Drawing.Color.White
        Me.txtTk.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk.Location = New System.Drawing.Point(88, 68)
        Me.txtTk.Name = "txtTk"
        Me.txtTk.TabIndex = 3
        Me.txtTk.Tag = "FCNBCF"
        Me.txtTk.Text = "TXTTK"
        '
        'lblTen_tk
        '
        Me.lblTen_tk.Location = New System.Drawing.Point(192, 70)
        Me.lblTen_tk.Name = "lblTen_tk"
        Me.lblTen_tk.Size = New System.Drawing.Size(233, 16)
        Me.lblTen_tk.TabIndex = 43
        Me.lblTen_tk.Tag = "FCRF"
        Me.lblTen_tk.Text = "Ten tai khoan co"
        '
        'txtT_tt_nt
        '
        Me.txtT_tt_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tt_nt.BackColor = System.Drawing.Color.White
        Me.txtT_tt_nt.Enabled = False
        Me.txtT_tt_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tt_nt.Format = "m_ip_tien_nt"
        Me.txtT_tt_nt.Location = New System.Drawing.Point(437, 401)
        Me.txtT_tt_nt.MaxLength = 13
        Me.txtT_tt_nt.Name = "txtT_tt_nt"
        Me.txtT_tt_nt.TabIndex = 26
        Me.txtT_tt_nt.Tag = "FN"
        Me.txtT_tt_nt.Text = "m_ip_tien_nt"
        Me.txtT_tt_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tt_nt.Value = 0
        '
        'txtT_tt
        '
        Me.txtT_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tt.BackColor = System.Drawing.Color.White
        Me.txtT_tt.Enabled = False
        Me.txtT_tt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tt.Format = "m_ip_tien"
        Me.txtT_tt.Location = New System.Drawing.Point(538, 401)
        Me.txtT_tt.MaxLength = 10
        Me.txtT_tt.Name = "txtT_tt"
        Me.txtT_tt.TabIndex = 27
        Me.txtT_tt.Tag = "FN"
        Me.txtT_tt.Text = "m_ip_tien"
        Me.txtT_tt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tt.Value = 0
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(251, 340)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(58, 16)
        Me.lblTotal.TabIndex = 60
        Me.lblTotal.Tag = "L013"
        Me.lblTotal.Text = "Tong cong"
        '
        'lblTien_thue
        '
        Me.lblTien_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTien_thue.AutoSize = True
        Me.lblTien_thue.Location = New System.Drawing.Point(336, 382)
        Me.lblTien_thue.Name = "lblTien_thue"
        Me.lblTien_thue.Size = New System.Drawing.Size(51, 16)
        Me.lblTien_thue.TabIndex = 61
        Me.lblTien_thue.Tag = "L014"
        Me.lblTien_thue.Text = "Tien thue"
        '
        'lblTien_tt
        '
        Me.lblTien_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTien_tt.AutoSize = True
        Me.lblTien_tt.Location = New System.Drawing.Point(336, 403)
        Me.lblTien_tt.Name = "lblTien_tt"
        Me.lblTien_tt.Size = New System.Drawing.Size(86, 16)
        Me.lblTien_tt.TabIndex = 63
        Me.lblTien_tt.Tag = "L015"
        Me.lblTien_tt.Text = "Tong thanh toan"
        '
        'lblMa_tt
        '
        Me.lblMa_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMa_tt.AutoSize = True
        Me.lblMa_tt.Location = New System.Drawing.Point(2, 403)
        Me.lblMa_tt.Name = "lblMa_tt"
        Me.lblMa_tt.Size = New System.Drawing.Size(29, 16)
        Me.lblMa_tt.TabIndex = 65
        Me.lblMa_tt.Tag = "L008"
        Me.lblMa_tt.Text = "Ma tt"
        '
        'txtMa_tt
        '
        Me.txtMa_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtMa_tt.BackColor = System.Drawing.Color.White
        Me.txtMa_tt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_tt.Location = New System.Drawing.Point(72, 401)
        Me.txtMa_tt.Name = "txtMa_tt"
        Me.txtMa_tt.Size = New System.Drawing.Size(24, 20)
        Me.txtMa_tt.TabIndex = 25
        Me.txtMa_tt.Tag = "FCCF"
        Me.txtMa_tt.Text = "TXTMA_TT"
        '
        'lblTen_tt
        '
        Me.lblTen_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTen_tt.Location = New System.Drawing.Point(104, 403)
        Me.lblTen_tt.Name = "lblTen_tt"
        Me.lblTen_tt.Size = New System.Drawing.Size(194, 16)
        Me.lblTen_tt.TabIndex = 66
        Me.lblTen_tt.Tag = "FCRF"
        Me.lblTen_tt.Text = "Ten thanh toan"
        '
        'lblTen
        '
        Me.lblTen.AutoSize = True
        Me.lblTen.Location = New System.Drawing.Point(574, 456)
        Me.lblTen.Name = "lblTen"
        Me.lblTen.Size = New System.Drawing.Size(58, 16)
        Me.lblTen.TabIndex = 68
        Me.lblTen.Tag = "RF"
        Me.lblTen.Text = "Ten chung"
        Me.lblTen.Visible = False
        '
        'txtSo_ct0
        '
        Me.txtSo_ct0.BackColor = System.Drawing.Color.White
        Me.txtSo_ct0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_ct0.Location = New System.Drawing.Point(88, 110)
        Me.txtSo_ct0.Name = "txtSo_ct0"
        Me.txtSo_ct0.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtSo_ct0.TabIndex = 5
        Me.txtSo_ct0.Tag = "FCCF"
        Me.txtSo_ct0.Text = "TXTSO_CT0"
        '
        'lblSo_hd
        '
        Me.lblSo_hd.AutoSize = True
        Me.lblSo_hd.Location = New System.Drawing.Point(2, 112)
        Me.lblSo_hd.Name = "lblSo_hd"
        Me.lblSo_hd.Size = New System.Drawing.Size(34, 16)
        Me.lblSo_hd.TabIndex = 70
        Me.lblSo_hd.Tag = "L006"
        Me.lblSo_hd.Text = "So hd"
        '
        'txtNgay_ct0
        '
        Me.txtNgay_ct0.BackColor = System.Drawing.Color.White
        Me.txtNgay_ct0.Location = New System.Drawing.Point(88, 131)
        Me.txtNgay_ct0.MaxLength = 10
        Me.txtNgay_ct0.Name = "txtNgay_ct0"
        Me.txtNgay_ct0.TabIndex = 7
        Me.txtNgay_ct0.Tag = "FDCFDF"
        Me.txtNgay_ct0.Text = "  /  /    "
        Me.txtNgay_ct0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_ct0.Value = New Date(CType(0, Long))
        '
        'lblNgay_hd
        '
        Me.lblNgay_hd.AutoSize = True
        Me.lblNgay_hd.Location = New System.Drawing.Point(2, 133)
        Me.lblNgay_hd.Name = "lblNgay_hd"
        Me.lblNgay_hd.Size = New System.Drawing.Size(46, 16)
        Me.lblNgay_hd.TabIndex = 72
        Me.lblNgay_hd.Tag = "L007"
        Me.lblNgay_hd.Text = "Ngay hd"
        '
        'txtDien_giai
        '
        Me.txtDien_giai.BackColor = System.Drawing.Color.White
        Me.txtDien_giai.Location = New System.Drawing.Point(88, 47)
        Me.txtDien_giai.Name = "txtDien_giai"
        Me.txtDien_giai.Size = New System.Drawing.Size(337, 20)
        Me.txtDien_giai.TabIndex = 2
        Me.txtDien_giai.Tag = "FCCF"
        Me.txtDien_giai.Text = "txtDien_giai"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(2, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 16)
        Me.Label1.TabIndex = 75
        Me.Label1.Tag = "L029"
        Me.Label1.Text = "Dien giai"
        '
        'txtMa_gd
        '
        Me.txtMa_gd.BackColor = System.Drawing.Color.White
        Me.txtMa_gd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_gd.Location = New System.Drawing.Point(88, 89)
        Me.txtMa_gd.Name = "txtMa_gd"
        Me.txtMa_gd.Size = New System.Drawing.Size(30, 20)
        Me.txtMa_gd.TabIndex = 4
        Me.txtMa_gd.Tag = "FCNBCF"
        Me.txtMa_gd.Text = "TXTMA_GD"
        '
        'lblMa_gd
        '
        Me.lblMa_gd.AutoSize = True
        Me.lblMa_gd.Location = New System.Drawing.Point(2, 91)
        Me.lblMa_gd.Name = "lblMa_gd"
        Me.lblMa_gd.Size = New System.Drawing.Size(68, 16)
        Me.lblMa_gd.TabIndex = 77
        Me.lblMa_gd.Tag = "L005"
        Me.lblMa_gd.Text = "Ma giao dich"
        '
        'lblTen_gd
        '
        Me.lblTen_gd.Location = New System.Drawing.Point(121, 91)
        Me.lblTen_gd.Name = "lblTen_gd"
        Me.lblTen_gd.Size = New System.Drawing.Size(304, 16)
        Me.lblTen_gd.TabIndex = 78
        Me.lblTen_gd.Tag = "FCRF"
        Me.lblTen_gd.Text = "Ten giao dich"
        '
        'lvlT_cp
        '
        Me.lvlT_cp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvlT_cp.AutoSize = True
        Me.lvlT_cp.Location = New System.Drawing.Point(336, 361)
        Me.lvlT_cp.Name = "lvlT_cp"
        Me.lvlT_cp.Size = New System.Drawing.Size(39, 16)
        Me.lvlT_cp.TabIndex = 81
        Me.lvlT_cp.Tag = "L030"
        Me.lvlT_cp.Text = "Chi phi"
        '
        'txtT_cp_nt
        '
        Me.txtT_cp_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_cp_nt.BackColor = System.Drawing.Color.White
        Me.txtT_cp_nt.Enabled = False
        Me.txtT_cp_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp_nt.Format = "m_ip_tien_nt"
        Me.txtT_cp_nt.Location = New System.Drawing.Point(437, 359)
        Me.txtT_cp_nt.MaxLength = 13
        Me.txtT_cp_nt.Name = "txtT_cp_nt"
        Me.txtT_cp_nt.TabIndex = 23
        Me.txtT_cp_nt.Tag = "FN"
        Me.txtT_cp_nt.Text = "m_ip_tien_nt"
        Me.txtT_cp_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp_nt.Value = 0
        '
        'txtT_cp
        '
        Me.txtT_cp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_cp.BackColor = System.Drawing.Color.White
        Me.txtT_cp.Enabled = False
        Me.txtT_cp.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp.Format = "m_ip_tien"
        Me.txtT_cp.Location = New System.Drawing.Point(538, 359)
        Me.txtT_cp.MaxLength = 10
        Me.txtT_cp.Name = "txtT_cp"
        Me.txtT_cp.TabIndex = 24
        Me.txtT_cp.Tag = "FN"
        Me.txtT_cp.Text = "m_ip_tien"
        Me.txtT_cp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp.Value = 0
        '
        'txtT_so_luong
        '
        Me.txtT_so_luong.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_so_luong.BackColor = System.Drawing.Color.White
        Me.txtT_so_luong.Enabled = False
        Me.txtT_so_luong.ForeColor = System.Drawing.Color.Black
        Me.txtT_so_luong.Format = "m_ip_sl"
        Me.txtT_so_luong.Location = New System.Drawing.Point(336, 338)
        Me.txtT_so_luong.MaxLength = 8
        Me.txtT_so_luong.Name = "txtT_so_luong"
        Me.txtT_so_luong.TabIndex = 18
        Me.txtT_so_luong.Tag = "FN"
        Me.txtT_so_luong.Text = "m_ip_sl"
        Me.txtT_so_luong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_so_luong.Value = 0
        '
        'chkGia_thue_yn
        '
        Me.chkGia_thue_yn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkGia_thue_yn.Location = New System.Drawing.Point(8, 339)
        Me.chkGia_thue_yn.Name = "chkGia_thue_yn"
        Me.chkGia_thue_yn.Size = New System.Drawing.Size(152, 16)
        Me.chkGia_thue_yn.TabIndex = 17
        Me.chkGia_thue_yn.TabStop = False
        Me.chkGia_thue_yn.Tag = "L031FLCF"
        Me.chkGia_thue_yn.Text = "Gia chua thue"
        Me.chkGia_thue_yn.Visible = False
        '
        'txtLoai_ct
        '
        Me.txtLoai_ct.BackColor = System.Drawing.Color.White
        Me.txtLoai_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLoai_ct.Location = New System.Drawing.Point(520, 456)
        Me.txtLoai_ct.Name = "txtLoai_ct"
        Me.txtLoai_ct.Size = New System.Drawing.Size(30, 20)
        Me.txtLoai_ct.TabIndex = 85
        Me.txtLoai_ct.Tag = "FC"
        Me.txtLoai_ct.Text = "TXTLOAI_CT"
        Me.txtLoai_ct.Visible = False
        '
        'lblSo_seri
        '
        Me.lblSo_seri.AutoSize = True
        Me.lblSo_seri.Location = New System.Drawing.Point(239, 112)
        Me.lblSo_seri.Name = "lblSo_seri"
        Me.lblSo_seri.Size = New System.Drawing.Size(39, 16)
        Me.lblSo_seri.TabIndex = 119
        Me.lblSo_seri.Tag = "L049"
        Me.lblSo_seri.Text = "So seri"
        '
        'txtSo_seri0
        '
        Me.txtSo_seri0.BackColor = System.Drawing.Color.White
        Me.txtSo_seri0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_seri0.Location = New System.Drawing.Point(325, 112)
        Me.txtSo_seri0.Name = "txtSo_seri0"
        Me.txtSo_seri0.TabIndex = 6
        Me.txtSo_seri0.Tag = "FCCFDF"
        Me.txtSo_seri0.Text = "TXTSO_SERI0"
        Me.txtSo_seri0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmVoucher
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(642, 473)
        Me.Controls.Add(Me.lblSo_seri)
        Me.Controls.Add(Me.txtSo_seri0)
        Me.Controls.Add(Me.txtLoai_ct)
        Me.Controls.Add(Me.txtT_so_luong)
        Me.Controls.Add(Me.lvlT_cp)
        Me.Controls.Add(Me.txtT_cp_nt)
        Me.Controls.Add(Me.txtT_cp)
        Me.Controls.Add(Me.txtMa_gd)
        Me.Controls.Add(Me.lblMa_gd)
        Me.Controls.Add(Me.txtDien_giai)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblNgay_hd)
        Me.Controls.Add(Me.txtNgay_ct0)
        Me.Controls.Add(Me.txtSo_ct0)
        Me.Controls.Add(Me.lblSo_hd)
        Me.Controls.Add(Me.lblTen)
        Me.Controls.Add(Me.lblMa_tt)
        Me.Controls.Add(Me.txtMa_tt)
        Me.Controls.Add(Me.lblTien_tt)
        Me.Controls.Add(Me.lblTien_thue)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.txtT_tt_nt)
        Me.Controls.Add(Me.txtT_tt)
        Me.Controls.Add(Me.txtTk)
        Me.Controls.Add(Me.lblTk)
        Me.Controls.Add(Me.txtOng_ba)
        Me.Controls.Add(Me.lblOng_ba)
        Me.Controls.Add(Me.txtMa_kh)
        Me.Controls.Add(Me.lblMa_kh)
        Me.Controls.Add(Me.lblAction)
        Me.Controls.Add(Me.txtKeyPress)
        Me.Controls.Add(Me.lblStatusMess)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.txtT_tien_nt0)
        Me.Controls.Add(Me.txtT_thue_nt)
        Me.Controls.Add(Me.txtT_thue)
        Me.Controls.Add(Me.txtT_tien0)
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
        Me.Controls.Add(Me.chkGia_thue_yn)
        Me.Controls.Add(Me.lblTen_gd)
        Me.Controls.Add(Me.lblTen_tt)
        Me.Controls.Add(Me.lblTen_tk)
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
        Me.tbgCharge.ResumeLayout(False)
        CType(Me.grdCharge, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpgOther.ResumeLayout(False)
        CType(Me.grdOther, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

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
            str = String.Concat(New String() {"EXEC fs_LoadPVTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', -1"})
        Else
            str = String.Concat(New String() {"EXEC fs_LoadPVTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
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

    Private Function isValidCharge() As Boolean
        Dim flag As Boolean = True
        Dim num As New Decimal(Me.txtT_cp.Value)
        If (Decimal.Compare(clsfields.GetSumValue("tien_cp", modVoucher.tblCharge), num) <> 0) Then
            flag = False
            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("040")), 2)
        End If
        Return flag
    End Function

    Private Sub LotItem(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Me.xInventory.ShowLotUpdate(True)
        End If
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

    Private Sub NewItemCharge(ByVal sender As Object, ByVal e As EventArgs)
        If (Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) AndAlso Not Me.grdCharge.ReadOnly) Then
            Dim currentRowIndex As Integer = Me.grdCharge.CurrentRowIndex
            If (currentRowIndex < 0) Then
                modVoucher.tblCharge.AddNew()
                Me.grdCharge.CurrentCell = New DataGridCell(0, 0)
            ElseIf (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblCharge.Item(currentRowIndex).Item("ma_cp"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblCharge.Item(currentRowIndex).Item("ma_cp"))), "", False) <> 0)) Then
                Dim count As Integer = modVoucher.tblCharge.Count
                Me.grdCharge.BeforeAddNewItem()
                Me.grdCharge.CurrentCell = New DataGridCell(count, 0)
                Me.grdCharge.AfterAddNewItem()
            End If
        End If
    End Sub

    Private Sub NewItemVAT(ByVal sender As Object, ByVal e As EventArgs)
        If (Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) AndAlso Not Me.grdOther.ReadOnly) Then
            Dim currentRowIndex As Integer = Me.grdOther.CurrentRowIndex
            If (currentRowIndex < 0) Then
                modVoucher.tblOther.AddNew()
                Me.grdOther.CurrentCell = New DataGridCell(0, 0)
            ElseIf (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(currentRowIndex).Item("mau_bc"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblOther.Item(currentRowIndex).Item("mau_bc"))), "", False) <> 0)) Then
                Dim count As Integer = modVoucher.tblOther.Count
                Me.grdOther.BeforeAddNewItem()
                Me.grdOther.CurrentCell = New DataGridCell(count, 0)
                Me.grdOther.AfterAddNewItem()
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
                    oVoucher.ViewDeletedRecord("fs_SearchDeletedPVTran", "PVMaster", "PVDetail", "t_tt", "t_tt_nt")
                    Exit Select
                Case 4
                    Dim strKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                    oVoucher.ViewPostedFile("ct00", strKey, "GL")
                    Exit Select
                Case 5
                    Dim str2 As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                    oVoucher.ViewPostedFile("ctgt30", str2, "InputVAT")
                    Exit Select
                Case 6
                    Dim str3 As String = ("LEFT(stt_rec, 10) = '" & Strings.Mid(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), 1, 10) & "' AND loai_tt = 0")
                    oVoucher.ViewPostedFile("cttt30", str3, "AP0")
                    Exit Select
                Case 7
                    Dim str4 As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                    oVoucher.ViewPostedFile("ct70", str4, "IN")
                    Exit Select
            End Select
        End If
    End Sub

    Private Function Post() As String
        Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "groupby", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Dim str3 As String = "EXEC fs_PostPV "
        Return (StringType.FromObject(ObjectType.AddObj(((((((str3 & "'" & modVoucher.VoucherCode & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))) & "'") & ", '" & Strings.Trim(str) & "'"), ObjectType.AddObj(ObjectType.AddObj(", '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))) & ", 1")
    End Function

    Public Sub Print()
        Dim print As New frmPrint
        print.txtTitle.Text = StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct"))), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct2")))))
        print.txtSo_lien.Value = DoubleType.FromObject(modVoucher.oVoucherRow.Item("so_lien"))
        Dim table As DataTable = clsprint.InitComboReport(modVoucher.sysConn, print.cboReports, "PVTran")
        Dim result As DialogResult = print.ShowDialog
        If ((result <> DialogResult.Cancel) AndAlso (print.txtSo_lien.Value > 0)) Then
            Dim selectedIndex As Integer = print.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
            Dim view As New DataView
            Dim ds As New DataSet
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj((("EXEC fs_PrintPVTran '" & modVoucher.cLan) & "', " & "[a.stt_rec = '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'], '"), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf")))), "'"))
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
            clsprint.SetReportVar(modVoucher.sysConn, modVoucher.appConn, "PVTran", modVoucher.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(print.txtTitle.Text))
            Dim falsePart As DateTime = Me.txtNgay_ct.Value
            Dim str2 As String = Strings.Replace(Strings.Replace(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("401")), "%s1", Me.txtNgay_ct.Value.Day.ToString, 1, -1, CompareMethod.Binary), "%s2", Me.txtNgay_ct.Value.Month.ToString, 1, -1, CompareMethod.Binary), "%s3", falsePart.Year.ToString, 1, -1, CompareMethod.Binary)
            Dim str4 As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("402")), "%s", Strings.Trim(Me.txtSo_ct.Text), 1, -1, CompareMethod.Binary)
            Dim str As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("403")), "%s", clsprint.Num2Words(New Decimal(Me.txtT_tt.Value), StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.oOption.Item("m_use_2fc"), "1", False) = 0), RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "SELECT dbo.ff30_FC1()")), RuntimeHelpers.GetObjectValue(modVoucher.oOption.Item("m_ma_nt0"))))), 1, -1, CompareMethod.Binary)
            clsprint.oRpt.SetParameterValue("s_Byword", str)
            clsprint.oRpt.SetParameterValue("t_date", str2)
            clsprint.oRpt.SetParameterValue("t_number", str4)
            clsprint.oRpt.SetParameterValue("nAmount", Me.txtT_tien0.Value)
            clsprint.oRpt.SetParameterValue("nCharge", Me.txtT_cp.Value)
            clsprint.oRpt.SetParameterValue("nTax", Me.txtT_thue.Value)
            clsprint.oRpt.SetParameterValue("nTotal", Me.txtT_tt.Value)
            clsprint.oRpt.SetParameterValue("f_ong_ba", Strings.Trim(Me.txtOng_ba.Text))
            clsprint.oRpt.SetParameterValue("f_kh", (Strings.Trim(Me.txtMa_kh.Text) & " - " & Strings.Trim(Me.lblTen_kh.Text)))
            Try
                falsePart = New DateTime(&H76C, 1, 1)
                clsprint.oRpt.SetParameterValue("f_ngay_hd", RuntimeHelpers.GetObjectValue(Interaction.IIf(Information.IsDate(Me.txtNgay_ct0.Text), Me.txtNgay_ct0.Value, falsePart)))
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                ProjectData.ClearProjectError()
            End Try
            Try
                falsePart = New DateTime(&H76C, 1, 1)
                clsprint.oRpt.SetParameterValue("1f_ngay_hd", RuntimeHelpers.GetObjectValue(Interaction.IIf(Information.IsDate(Me.txtNgay_ct0.Text), Me.txtNgay_ct0.Value, falsePart)))
            Catch exception3 As Exception
                ProjectData.SetProjectError(exception3)
                Dim exception2 As Exception = exception3
                ProjectData.ClearProjectError()
            End Try
            Dim str3 As String = (Strings.Trim(Me.txtTk.Text) & " - " & Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmtk", StringType.FromObject(ObjectType.AddObj("ten_tk", Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), "", "2"))), ("tk = '" & Strings.Trim(Me.txtTk.Text) & "'")))))
            clsprint.oRpt.SetParameterValue("f_tk", str3)
            str3 = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "dia_chi", ("ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'"))))
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

    Public Sub RefrehForm()
        Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
        Me.grdHeader.Scatter()
        ScatterMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'")
        modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
        Me.RefreshCharge(1)
        Me.RefreshVAT(1)
        Me.UpdateList()
        Me.vCaptionRefresh()
        Me.cmdNew.Focus()
    End Sub

    Private Sub RefreshCharge(ByVal nType As Byte)
        modVoucher.tblCharge.Table.Clear()
        If (nType <> 0) Then
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(("fs_LoadCharge '" & modVoucher.cLan & "', '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
            Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, modVoucher.alCharge, (modVoucher.tblCharge.Table.DataSet))
        End If
    End Sub

    Private Sub RefreshControlField()
    End Sub

    Private Sub RefreshVAT(ByVal nType As Byte)
        modVoucher.tblOther.Table.Clear()
        If (nType <> 0) Then
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(("fs_LoadInputVAT '" & modVoucher.cLan & "', '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
            Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, modVoucher.alOther, (modVoucher.tblOther.Table.DataSet))
        End If
    End Sub

    Private Sub RestoreCharge()
        Dim cString As String = "cp_vc, cp_vc_nt, cp_bh, cp_bh_nt, cp_khac, cp_khac_nt"
        Dim num4 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num4)
            Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            Dim j As Integer = 1
            Do While (j <= num3)
                Dim str2 As String = Strings.Trim(Fox.GetWordNum(cString, j, ","c))
                Dim str As String = (str2 & "2")
                modVoucher.tblDetail.Item(i).Item(str2) = RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item(str))
                j += 1
            Loop
            i += 1
        Loop
    End Sub

    Private Sub RetrieveItems(ByVal sender As Object, ByVal e As EventArgs)
        Dim cancel As Boolean = Me.oInvItemDetail.Cancel
        Me.oInvItemDetail.Cancel = True
        Select Case IntegerType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing))
            Case 0
                Me.RetrieveItemsFromPO()
                Exit Select
            Case 2
                Me.RetrieveItemsFromPD()
                Exit Select
            Case 4
                Me.RetrieveItemsFromIR_NB()
                Exit Select
        End Select
        Me.oInvItemDetail.Cancel = cancel
    End Sub

    Private Sub RetrieveItemsFromPD()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("041")), 2)
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
                    Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchPDTran4PV '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph96', 'ct96'"})
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
                        frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("046"))
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
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "PDMaster")
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
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "PDDetail4PV")
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
                            If Not IsDBNull(Me.tblRetrieveMaster.Item(index).Item("t_tien")) Then
                                zero = zero + Me.tblRetrieveMaster.Item(index).Item("t_tien")
                            End If
                            If Not IsDBNull(Me.tblRetrieveMaster.Item(index).Item("t_tien_nt")) Then
                                num4 = num4 + Me.tblRetrieveMaster.Item(index).Item("t_tien_nt")
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
                        button4.Text = StringType.FromObject(modVoucher.oLan.Item("043"))
                        button4.Width = 100
                        button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Top = button4.Top
                        button2.Left = (button4.Left + 110)
                        button2.Visible = True
                        button2.Text = StringType.FromObject(modVoucher.oLan.Item("044"))
                        button2.Width = 120
                        button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Enabled = True
                        button3.Top = button4.Top
                        button3.Left = (button2.Left + 130)
                        button3.Visible = True
                        button3.Text = StringType.FromObject(modVoucher.oLan.Item("045"))
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
                        Me.tblRetrieveDetail.Sort = "ngay_ct, so_ct, stt_rec, line_nbr"
                        Dim num8 As Integer = (Me.tblRetrieveDetail.Count - 1)
                        index = 0
                        Do While (index <= num8)
                            With Me.tblRetrieveDetail.Item(index)
                                .Item("stt_rec_pn") = RuntimeHelpers.GetObjectValue(.Item("stt_rec"))
                                .Item("stt_rec0pn") = RuntimeHelpers.GetObjectValue(.Item("stt_rec0"))
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
                                If IsDBNull(tblDetail.Item(index).Item("stt_rec")) Then
                                    tblDetail.Item(index).Delete()
                                ElseIf (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) Then
                                    If (tblDetail.Item(index).Item("stt_rec") = "") Then
                                        tblDetail.Item(index).Delete()
                                    End If
                                    If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(tblDetail.Item(index).Item("stt_rec"))), tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                                        tblDetail.Item(index).Delete()
                                    End If
                                ElseIf IsDBNull(tblDetail.Item(index).Item("stt_rec")) Then
                                    tblDetail.Item(index).Delete()
                                ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                    tblDetail.Item(index).Delete()
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
                                    .Item("stt_rec") = tblMaster.Item(Me.iMasterRow).Item("stt_rec")
                                End If
                                tbl.Rows.Item(index).AcceptChanges()
                            End With
                            index += 1
                        Loop
                        AppendFrom(tblDetail, tbl)
                        count = tblDetail.Count
                        If flag Then
                            index = (count - 1)
                            Do While (index >= 0)
                                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("ma_vt")), "C") Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_pn")), "C") Then
                                    modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                                End If
                                index = (index + -1)
                            Loop
                            Dim num6 As Integer = (modVoucher.tblDetail.Count - 1)
                            index = 0
                            Do While (index <= num6)
                                With tblDetail(index)
                                    If Not IsDBNull(.Item("gia_nt0")) Then
                                        .Item("tien_nt0") = Fox.Round(.Item("so_luong") * .Item("gia_nt0"), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt")))
                                    End If
                                    If Not IsDBNull(.Item("tien_nt0")) Then
                                        .Item("tien0") = Fox.Round(.Item("tien_nt0") * Me.txtTy_gia.Value, IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien")))
                                    End If
                                End With
                                index += 1
                            Loop
                            Me.txtSo_ct.Text = tblDetail(0).Item("so_ct")
                            If Me.txtTk.Text = "" Then
                                Me.txtTk.Text = Sql.GetValue(appConn, "select min(tk) from dmtk where tk like '331%' and loai_tk=1")
                            End If
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

    Private Sub RetrieveItemsFromPO()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("041")), 2)
            Else
                Dim _date As New frmDate
                AddHandler _date.Load, New EventHandler(AddressOf Me.frmRetrieveLoad)
                If (_date.ShowDialog = DialogResult.OK) Then
                    Dim str3 As String = " 1 = 1 AND a.ma_ct = 'PO1'"
                    If (ObjectType.ObjTst(_date.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct >= ", Sql.ConvertVS2SQLType(_date.txtNgay_ct.Value, "")), ")")))
                    End If
                    If (ObjectType.ObjTst(Me.txtNgay_lct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct <= ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")), ")")))
                    End If
                    Dim strSQLLong As String = str3
                    str3 = (str3 & " AND a.ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'")
                    Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchPOTran4PV '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph94', 'ct94'"})
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
                        frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("042"))
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
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "POMaster")
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
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "PODetail4PV")
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
                        button4.Text = StringType.FromObject(modVoucher.oLan.Item("043"))
                        button4.Width = 100
                        button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Top = button4.Top
                        button2.Left = (button4.Left + 110)
                        button2.Visible = True
                        button2.Text = StringType.FromObject(modVoucher.oLan.Item("044"))
                        button2.Width = 120
                        button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Enabled = False
                        button3.Top = button4.Top
                        button3.Left = (button2.Left + 130)
                        button3.Visible = True
                        button3.Text = StringType.FromObject(modVoucher.oLan.Item("045"))
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
                        Me.tblRetrieveDetail.Sort = "ngay_ct, so_ct, stt_rec, line_nbr"
                        Dim num8 As Integer = (Me.tblRetrieveDetail.Count - 1)
                        index = 0
                        Do While (index <= num8)
                            With Me.tblRetrieveDetail.Item(index)
                                .Item("stt_rec_dh") = RuntimeHelpers.GetObjectValue(.Item("stt_rec"))
                                .Item("stt_rec0dh") = RuntimeHelpers.GetObjectValue(.Item("stt_rec0"))
                                .Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("so_luong0"))
                                .Row.AcceptChanges()
                            End With
                            index += 1
                        Loop
                        Me.tblRetrieveDetail.RowFilter = "so_luong0 <> 0"
                        Dim flag As Boolean = (Me.tblRetrieveDetail.Count > 0)
                        count = (tblDetail.Count - 1)
                        If ((button3.Checked And flag) And (count >= 0)) Then
                            index = count
                            Do While (index >= 0)
                                If Information.IsDBNull(tblDetail.Item(index).Item("stt_rec")) Then
                                    tblDetail.Item(index).Delete()
                                ElseIf (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) Then
                                    If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                        tblDetail.Item(index).Delete()
                                    End If
                                    If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                                        tblDetail.Item(index).Delete()
                                    End If
                                ElseIf IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec"))) Then
                                    tblDetail.Item(index).Delete()
                                ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                    tblDetail.Item(index).Delete()
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
                                    .Item("stt_rec") = tblMaster.Item(Me.iMasterRow).Item("stt_rec")
                                End If
                                .Item("sl_dh") = 0
                                .AcceptChanges()
                            End With
                            index += 1
                        Loop
                        AppendFrom(modVoucher.tblDetail, tbl)
                        count = tblDetail.Count
                        If flag Then
                            index = (count - 1)
                            Do While (index >= 0)
                                If clsfields.isEmpty(tblDetail.Item(index).Item("ma_vt"), "C") Then
                                    tblDetail.Item(index).Delete()
                                ElseIf Not clsfields.isEmpty(tblDetail.Item(index).Item("stt_rec_dh"), "C") Then
                                    tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                                End If
                                index = (index + -1)
                            Loop
                            Dim num6 As Integer = (modVoucher.tblDetail.Count - 1)
                            index = 0
                            Do While (index <= num6)
                                If Not IsDBNull(tblDetail.Item(index).Item("gia_nt0")) Then
                                    tblDetail.Item(index).Item("tien_nt0") = Fox.Round(tblDetail.Item(index).Item("so_luong") * tblDetail.Item(index).Item("gia_nt0"), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt")))
                                End If
                                If Not IsDBNull(tblDetail.Item(index).Item("tien_nt0")) Then
                                    tblDetail.Item(index).Item("tien0") = Fox.Round(tblDetail.Item(index).Item("tien_nt0") * Me.txtTy_gia.Value, IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien")))
                                End If
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

    Public Sub Save()
        Me.txtStatus.Text = Strings.Trim(StringType.FromObject(Me.tblHandling.Rows.Item(Me.cboAction.SelectedIndex).Item("action_id")))
        Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
        Try
            Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Try
        Catch exception3 As Exception
            ProjectData.SetProjectError(exception3)
            ProjectData.ClearProjectError()
        End Try
        Try
        Catch exception4 As Exception
            ProjectData.SetProjectError(exception4)
            ProjectData.ClearProjectError()
        End Try
        If Not Me.oSecurity.GetActionRight Then
            oVoucher.isContinue = False
        ElseIf Not Me.grdHeader.CheckEmpty(RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("035"))) Then
            oVoucher.isContinue = False
        Else
            Dim num As Integer
            Dim num3 As Integer = 0
            Dim num16 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num16)
                If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ma_vt"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("ma_vt"))), "", False) <> 0)) Then
                    num3 = 1
                    Exit Do
                End If
                num += 1
            Loop
            If (num3 = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("022")), 2)
                oVoucher.isContinue = False
            Else
                Dim str As String
                Dim num2 As Integer
                Dim strFieldList As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "vatfieldcheck", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
                num3 = (modVoucher.tblOther.Count - 1)
                Dim sLeft As String = clsfields.CheckEmptyFieldList("mau_bc", strFieldList, modVoucher.tblOther)
                If (StringType.StrCmp(sLeft, "", False) = 0) Then
                    num = num3
                    Do While (num >= 0)
                        With modVoucher.tblOther.Item(num)
                            If Not IsDBNull(.Item("mau_bc")) Then
                                If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblOther.Item(num).Item("mau_bc"))), "", False) = 0) Then
                                    modVoucher.tblOther.Item(num).Delete()
                                Else
                                    If IsDBNull(.Item("ngay_ct0")) Then
                                        sLeft = "ngay_ct0"
                                        Exit Do
                                    End If
                                    Dim str7 As String = StringType.FromObject(.Item("tk_thue_no"))
                                    If (ObjectType.ObjTst(Sql.GetValue((modVoucher.appConn), "dmtk", "tk_cn", ("tk = '" & str7 & "'")), 1, False) = 0) Then
                                        If IsDBNull(.Item("ma_kh2")) Then
                                            sLeft = "ma_kh2"
                                            Exit Do
                                        End If
                                        If (StringType.StrCmp(Strings.Trim(StringType.FromObject(.Item("ma_kh2"))), "", False) = 0) Then
                                            sLeft = "ma_kh2"
                                            Exit Do
                                        End If
                                    End If
                                    If (StringType.StrCmp(sLeft, "", False) = 0) Then
                                        .Item("tk_du") = Me.txtTk.Text
                                    End If
                                End If
                            Else
                                modVoucher.tblOther.Item(num).Delete()
                            End If
                        End With
                        num = (num + -1)
                    Loop
                End If
                Try
                    If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                        Msg.Alert(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("028")), "%s", GetColumn(Me.grdOther, sLeft).HeaderText, 1, -1, CompareMethod.Binary), 2)
                        oVoucher.isContinue = False
                        Return
                    End If
                Catch exception5 As Exception
                    ProjectData.SetProjectError(exception5)
                    Dim exception As Exception = exception5
                    ProjectData.ClearProjectError()
                End Try
                Dim num15 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num15)
                    Dim replacement As String = Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("ma_vt")))
                    If (clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("so_luong")), "N") AndAlso (ObjectType.ObjTst(Sql.GetValue((modVoucher.appConn), "dmvt", "gia_ton", ("ma_vt = '" & replacement & "'")), 3, False) = 0)) Then
                        oVoucher.isContinue = False
                        Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("043")), "%s", replacement, 1, -1, CompareMethod.Binary), 2)
                        Return
                    End If
                    num += 1
                Loop
                If ((ObjectType.ObjTst(modVoucher.oOption.Item("m_kt_mst"), 0, False) > 0) AndAlso Not clsvatform.TaxIDCheck(modVoucher.tblOther, "ma_so_thue")) Then
                    Dim obj2 As Object = modVoucher.oOption.Item("m_kt_mst")
                    If (ObjectType.ObjTst(obj2, 1, False) = 0) Then
                        Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("048")), 2)
                    ElseIf (ObjectType.ObjTst(obj2, 2, False) = 0) Then
                        Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("048")), 1)
                        oVoucher.isContinue = False
                        Return
                    End If
                End If
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
                num3 = (modVoucher.tblCharge.Count - 1)
                num = num3
                Do While (num >= 0)
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblCharge.Item(num).Item("ma_cp"))) Then
                        If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblCharge.Item(num).Item("ma_cp"))), "", False) = 0) Then
                            modVoucher.tblCharge.Item(num).Delete()
                        End If
                    Else
                        modVoucher.tblCharge.Item(num).Delete()
                    End If
                    num = (num + -1)
                Loop
                Dim cString As String = StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldchar", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
                Dim num14 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num14)
                    Dim num13 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    num2 = 1
                    Do While (num2 <= num13)
                        str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str))) Then
                            modVoucher.tblDetail.Item(num).Item(str) = ""
                        End If
                        num2 += 1
                    Loop
                    num += 1
                Loop
                cString = StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
                Dim num12 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num12)
                    Dim num11 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    num2 = 1
                    Do While (num2 <= num11)
                        str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                        If IsDBNull(tblDetail.Item(num).Item(str)) Then
                            modVoucher.tblDetail.Item(num).Item(str) = 0
                        End If
                        num2 += 1
                    Loop
                    num += 1
                Loop
                If (StringType.StrCmp(Me.txtStatus.Text, "0", False) <> 0) Then
                    strFieldList = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldcheck", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
                    If (StringType.StrCmp(Strings.Trim(strFieldList), "", False) <> 0) Then
                        num3 = (modVoucher.tblDetail.Count - 1)
                        sLeft = clsfields.CheckEmptyFieldList("stt_rec", strFieldList, modVoucher.tblDetail)
                        Try
                            If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                                Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("044")), "%s", GetColumn(Me.grdDetail, sLeft).HeaderText, 1, -1, CompareMethod.Binary), 2)
                                oVoucher.isContinue = False
                                Return
                            End If
                        Catch exception6 As Exception
                            ProjectData.SetProjectError(exception6)
                            Dim exception2 As Exception = exception6
                            ProjectData.ClearProjectError()
                        End Try
                    End If
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        Me.cIDNumber = ""
                    Else
                        Me.cIDNumber = StringType.FromObject(tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                    End If
                    If Not Me.isValidCharge Then
                        oVoucher.isContinue = False
                        Return
                    End If
                    If Not oVoucher.CheckDuplVoucherNumber(Fox.PadL(Strings.Trim(Me.txtSo_ct.Text), Me.txtSo_ct.MaxLength), StringType.FromObject(Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "New", Me.cIDNumber))) Then
                        Me.txtSo_ct.Focus()
                        oVoucher.isContinue = False
                        Return
                    End If
                    If Not CheckDuplInvNumber(modVoucher.appConn, modVoucher.sysConn, "0"c, Me.grdOther, modVoucher.tblOther, Me.cIDNumber) Then
                        oVoucher.isContinue = False
                        Return
                    End If
                End If
                If Not Me.xInventory.isValid Then
                    oVoucher.isContinue = False
                Else
                    Dim str6 As String
                    Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
                    If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                        auditamount.AuditAmounts(New Decimal(Me.txtT_tien0.Value), "tien0", modVoucher.tblDetail)
                        auditamount.AuditAmounts(New Decimal(Me.txtT_thue.Value), "thue", modVoucher.tblDetail)
                    End If
                    auditamount.DistributeAmounts(New Decimal(Me.txtT_thue_nt.Value), "tien_nt0", "thue_nt", modVoucher.tblDetail, ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt")))
                    auditamount.DistributeAmounts(New Decimal(Me.txtT_thue.Value), "tien0", "thue", modVoucher.tblDetail, ByteType.FromObject(modVoucher.oVar.Item("m_round_tien")))
                    auditamount.AuditAmounts(New Decimal(Me.txtT_thue_nt.Value), "thue_nt", modVoucher.tblDetail)
                    auditamount.AuditAmounts(New Decimal(Me.txtT_thue.Value), "thue", modVoucher.tblDetail)
                    Me.UpdatePV()
                    Me.UpdateList()
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        Me.cIDNumber = oVoucher.GetIdentityNumber
                        modVoucher.tblMaster.AddNew()
                        Me.iMasterRow = (modVoucher.tblMaster.Count - 1)
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec") = Me.cIDNumber
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_ct") = modVoucher.VoucherCode
                    Else
                        Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                        Me.BeforUpdatePV(Me.cIDNumber, "Edit")
                    End If
                    DirLib.SetDatetime(modVoucher.appConn, modVoucher.tblMaster.Item(Me.iMasterRow), oVoucher.cAction)
                    Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
                    Me.grdHeader.Gather()
                    GatherMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
                    modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct"))), Me.txtSo_ct.MaxLength)
                    modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct0") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct0"))), Me.txtSo_ct0.MaxLength)
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        str6 = GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row)
                    Else
                        Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                        str6 = (((GenSQLUpdate((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row, cKey) & ChrW(13) & GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)) & ChrW(13) & GenSQLDelete("ctgt30", cKey)) & ChrW(13) & GenSQLDelete("ctcp30", cKey))
                    End If
                    cString = "ma_ct, ngay_ct, so_ct, stt_rec"
                    Dim str5 As String = ("stt_rec = '" & Me.cIDNumber & "' or stt_rec = '' or stt_rec is null")
                    modVoucher.tblDetail.RowFilter = str5
                    num3 = (modVoucher.tblDetail.Count - 1)
                    Dim expression As Integer = 0
                    Dim num10 As Integer = num3
                    num = 0
                    Do While (num <= num10)
                        If (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("stt_rec"), Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "", RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))), False) = 0) Then
                            Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                            num2 = 1
                            Do While (num2 <= num9)
                                str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                                modVoucher.tblDetail.Item(num).Item(str) = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item(str))
                                num2 += 1
                            Loop
                            expression += 1
                            modVoucher.tblDetail.Item(num).Item("line_nbr") = expression
                            Me.grdDetail.Update()
                            str6 = (str6 & ChrW(13) & GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), modVoucher.tblDetail.Item(num).Row))
                        End If
                        num += 1
                    Loop
                    cString = "ma_ct, so_ct, loai_ct, ngay_ct, ngay_lct, stt_rec, ma_dvcs, ma_nt, datetime0, datetime2, user_id0, user_id2, status"
                    expression = 0
                    num3 = (modVoucher.tblOther.Count - 1)
                    Dim num8 As Integer = num3
                    num = 0
                    Do While (num <= num8)
                        Dim num7 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                        num2 = 1
                        Do While (num2 <= num7)
                            str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                            modVoucher.tblOther.Item(num).Item(str) = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item(str))
                            num2 += 1
                        Loop
                        expression += 1
                        modVoucher.tblOther.Item(num).Item("stt_rec0") = Strings.Format(expression, "000")
                        modVoucher.tblOther.Item(num).Item("line_nbr") = expression
                        Me.grdOther.Update()
                        str6 = (str6 & ChrW(13) & GenSQLInsert((modVoucher.appConn), "ctgt30", modVoucher.tblOther.Item(num).Row))
                        num += 1
                    Loop
                    cString = "ma_ct, so_ct, loai_ct, ngay_ct, ngay_lct, stt_rec, ma_dvcs, datetime0, datetime2, user_id0, user_id2, status"
                    expression = 0
                    num3 = (modVoucher.tblCharge.Count - 1)
                    Dim num6 As Integer = num3
                    num = 0
                    Do While (num <= num6)
                        Dim num5 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                        num2 = 1
                        Do While (num2 <= num5)
                            str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                            modVoucher.tblCharge.Item(num).Item(str) = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item(str))
                            num2 += 1
                        Loop
                        expression += 1
                        modVoucher.tblCharge.Item(num).Item("stt_rec0") = Strings.Format(expression, "000")
                        modVoucher.tblCharge.Item(num).Item("line_nbr") = expression
                        Me.grdCharge.Update()
                        str6 = (str6 & ChrW(13) & GenSQLInsert((modVoucher.appConn), "ctcp30", modVoucher.tblCharge.Item(num).Row))
                        num += 1
                    Loop
                    oVoucher.IncreaseVoucherNo(Strings.Trim(Me.txtSo_ct.Text))
                    Me.EDTBColumns(False)
                    Sql.SQLCompressExecute((modVoucher.appConn), str6)
                    str6 = Me.Post
                    Sql.SQLExecute((modVoucher.appConn), str6)
                    Me.grdHeader.UpdateFreeField(modVoucher.appConn, StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")))
                    Me.AfterUpdatePV(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "Save")
                    Me.pnContent.Text = StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.tblMaster.Item(Me.iMasterRow).Item("status"), "2", False) <> 0), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("018")), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("019"))))
                    SaveLocalDataView(modVoucher.tblDetail)
                    oVoucher.RefreshStatus(Me.cboStatus)
                End If
            End If
        End If
    End Sub

    Private Sub SaveCharge()
        Dim cString As String = "cp_vc, cp_vc_nt, cp_bh, cp_bh_nt, cp_khac, cp_khac_nt"
        Dim num4 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num4)
            Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            Dim j As Integer = 1
            Do While (j <= num3)
                Dim str2 As String = Strings.Trim(Fox.GetWordNum(cString, j, ","c))
                Dim str As String = (str2 & "2")
                modVoucher.tblDetail.Item(i).Item(str) = RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item(str2))
                j += 1
            Loop
            i += 1
        Loop
    End Sub

    Public Sub Search()
        Dim _frmSearch As New frmSearch
        _frmSearch.ShowDialog()
    End Sub

    Private Sub SetEmptyColKey(ByVal sender As Object, ByVal e As EventArgs)
        Me.iOldRow = Me.grdDetail.CurrentRowIndex
        Me.cOldItem = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        If Not Me.oInvItemDetail.Cancel Then
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

    Private Sub SetEmptyColKeyCharge(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRowIndex As Integer = Me.grdCharge.CurrentRowIndex
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("ma_cp"))) Then
        End If
        Me.coldCMa_cp = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub SetEmptyColKeyVAT(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRowIndex As Integer = Me.grdOther.CurrentRowIndex
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item("mau_bc"))) Then
            Me.VATCarryOn(modVoucher.tblOther, currentRowIndex)
            Fox.KeyBoard(" ")
        End If
    End Sub

    Private Sub ShowTabDetail()
        Me.tbDetail.SelectedIndex = 0
    End Sub

    Private Sub ShowTotalAmount(ByVal nType As Byte)
        Dim str As String
        Dim sumValue As Decimal = clsfields.GetSumValue(StringType.FromObject(Interaction.IIf((nType = 1), "t_tien", "t_tien_nt")), modVoucher.tblOther)
        If (nType = 1) Then
            str = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(modVoucher.oLan.Item("025"), ": "), Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))))))
        ElseIf (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            str = (Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("024")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary) & ": " & Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))))
        Else
            str = (Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("024")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary) & ": " & Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))))
        End If
        Me.pnContent.Text = str
    End Sub

    Private Sub ShowTotalCharge(ByVal nType As Byte)
        Dim str As String
        Dim sumValue As Decimal = clsfields.GetSumValue(StringType.FromObject(Interaction.IIf((nType = 1), "tien_cp", "tien_cp_nt")), modVoucher.tblCharge)
        If (nType = 1) Then
            str = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(modVoucher.oLan.Item("037"), ": "), Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))))))
        ElseIf (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            str = (Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("036")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary) & ": " & Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))))
        Else
            str = (Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("036")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary) & ": " & Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))))
        End If
        Me.pnContent.Text = str
    End Sub

    Private Sub ShowTotalECharge(ByVal cField As String, ByVal isFC As Boolean)
        Dim str As String
        Dim sumValue As Decimal = clsfields.GetSumValue(cField, modVoucher.tblDetail)
        If isFC Then
            If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
                str = (Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("036")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary) & ": " & Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))))
            Else
                str = (Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("036")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary) & ": " & Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))))
            End If
        Else
            str = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(modVoucher.oLan.Item("037"), ": "), Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))))))
        End If
        Me.pn.Text = str
    End Sub

    Private Sub tbDetail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tbDetail.Click
        If (((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) AndAlso (Me.tbDetail.SelectedIndex = 2)) Then
            Me.AppendVAT()
        End If
    End Sub

    Private Sub tbDetail_Enter(ByVal sender As Object, ByVal e As EventArgs)
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

    Private Sub txtC_Enter(ByVal sender As Object, ByVal e As EventArgs)
        If Not Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
        ElseIf Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("ma_cp"))) Then
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
        Else
            Dim sLeft As String = Strings.Trim(StringType.FromObject(modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("ma_cp")))
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(StringType.StrCmp(sLeft, "", False) = 0)}, Nothing)
        End If
    End Sub

    Private Sub txtCTien_cp_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldCTien_cp = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.ShowTotalCharge(1)
    End Sub

    Private Sub txtCTien_cp_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldCTien_cp_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.ShowTotalCharge(2)
    End Sub

    Private Sub txtCTien_cp_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num4 As Decimal = Me.noldCTien_cp_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            With modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex)
                .Item("tien_cp_nt") = num
                .Item("tien_cp") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
            End With
        End If
        Me.ShowTotalCharge(2)
    End Sub

    Private Sub txtCTien_cp_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num3 As Decimal = Me.noldCTien_cp
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num3) <> 0) Then
            tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("tien_cp") = num
        End If
        Me.ShowTotalCharge(1)
    End Sub

    Private Sub txtDien_giai_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles txtDien_giai.Leave
    End Sub

    Private Sub txtE_Enter(ByVal sender As Object, ByVal e As EventArgs)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item("mau_bc"))) Then
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
        Else
            Dim sLeft As String = Strings.Trim(StringType.FromObject(modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item("mau_bc")))
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(StringType.StrCmp(sLeft, "", False) = 0)}, Nothing)
        End If
    End Sub

    Private Sub txtECharge_enter(ByVal sender As Object, ByVal cField As String, ByVal isFc As Boolean)
        Me.nOldECharge = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.ShowTotalECharge(cField, isFc)
    End Sub

    Private Sub txtECharge_valid(ByVal sender As Object, ByVal cField As String)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim nOldECharge As Decimal = Me.nOldECharge
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, nOldECharge) <> 0) Then
            modVoucher.tblDetail.Item(Me.grdMV.CurrentRowIndex).Item(cField) = num
        End If
        Me.ShowTotalECharge(cField, False)
    End Sub

    Private Sub txtECp_bh_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_enter(RuntimeHelpers.GetObjectValue(sender), "cp_bh", False)
    End Sub

    Private Sub txtECp_bh_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_enter(RuntimeHelpers.GetObjectValue(sender), "cp_bh_nt", True)
    End Sub

    Private Sub txtECp_bh_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtFCECharge_valid(RuntimeHelpers.GetObjectValue(sender), "cp_bh_nt", "cp_bh")
    End Sub

    Private Sub txtECp_bh_valid(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_valid(RuntimeHelpers.GetObjectValue(sender), "cp_bh")
    End Sub

    Private Sub txtECp_khac_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_enter(RuntimeHelpers.GetObjectValue(sender), "cp_khac", False)
    End Sub

    Private Sub txtECp_khac_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_enter(RuntimeHelpers.GetObjectValue(sender), "cp_khac_nt", True)
    End Sub

    Private Sub txtECp_khac_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtFCECharge_valid(RuntimeHelpers.GetObjectValue(sender), "cp_khac_nt", "cp_khac")
    End Sub

    Private Sub txtECp_khac_valid(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_valid(RuntimeHelpers.GetObjectValue(sender), "cp_khac")
    End Sub

    Private Sub txtECp_vc_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_enter(RuntimeHelpers.GetObjectValue(sender), "cp_vc", False)
    End Sub

    Private Sub txtECp_vc_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_enter(RuntimeHelpers.GetObjectValue(sender), "cp_vc_nt", True)
    End Sub

    Private Sub txtECp_vc_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtFCECharge_valid(RuntimeHelpers.GetObjectValue(sender), "cp_vc_nt", "cp_vc")
    End Sub

    Private Sub txtECp_vc_valid(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_valid(RuntimeHelpers.GetObjectValue(sender), "cp_vc")
    End Sub

    Private Sub txtFCECharge_valid(ByVal sender As Object, ByVal cField As String, ByVal cRef As String)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim nOldECharge As Decimal = Me.nOldECharge
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, nOldECharge) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdMV.CurrentRowIndex)
                .Item(cField) = num
                .Item(cRef) = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
            End With
        End If
        Me.ShowTotalECharge(cField, True)
    End Sub

    Private Sub txtGia_nt0_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia_nt0 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtGia_nt0_valid(ByVal sender As Object, ByVal e As EventArgs)
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
        Dim num6 As Decimal = Me.noldGia_nt0
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num6) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("gia_nt0") = num
                .Item("gia0") = Fox.Round(num * Me.txtTy_gia.Value, digits)
                .Item("tien_nt0") = Fox.Round(.Item("so_luong") * num, num3)
                .Item("Tien0") = Fox.Round(.Item("tien_nt0") * Me.txtTy_gia.Value, num5)
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtGia0_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia0 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtGia0_valid(ByVal sender As Object, ByVal e As EventArgs)
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
        Dim num6 As Decimal = Me.noldGia0
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num6) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("gia0") = num
                .Item("tien0") = Fox.Round(.Item("so_luong") * num, num5)
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtKeyPress_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtKeyPress.Enter
        Me.grdDetail.Focus()
        Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
    End Sub

    Private Sub txtMa_kh_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtMa_kh.Enter
        If Not ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
            Return
        End If
        Dim flag As Boolean
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            flag = False
        Else
            flag = (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "cttt30", "stt_rec", StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec_tt = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))))), "", False) <> 0)
        End If
        If flag Then
            Me.txtMa_kh.ReadOnly = True
            Return
        End If
        'Dim i As Integer
        'For i = 1 To modVoucher.tblDetail.Count - 1
        '    If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("ma_vt")), "C") Then
        '        Me.txtMa_kh.ReadOnly = True
        '        Return
        '    End If
        'Next
    End Sub

    Private Sub txtMa_kh_valid(ByVal sender As Object, ByVal e As EventArgs)
        If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) And (StringType.StrCmp(Strings.Trim(Me.txtMa_tt.Text), "", False) = 0)) Then
            Me.txtMa_tt.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "ma_tt", ("ma_kh = '" & Me.txtMa_kh.Text & "'"))))
        End If
    End Sub

    Private Sub txtNumber_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtSo_ct.Enter, txtSo_ct0.Enter
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
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia_nt0"))) Then
                    .Item("gia_nt0") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia0"))) Then
                    .Item("gia0") = 0
                End If
                .Item("so_luong") = num
                .Item("tien_nt0") = Fox.Round(.Item("gia_nt0") * num, num2)
                .Item("tien0") = Fox.Round(.Item("gia0") * num, num3)
            End With
            Me.grdDetail.Refresh()
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien_nt0_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien_nt0 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTien_nt0_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num4 As Decimal = Me.noldTien_nt0
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("Tien_nt0") = num
                .Item("Tien0") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien0_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien0 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTien0_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num3 As Decimal = Me.noldTien0
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num3) <> 0) Then
            modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("Tien0") = num
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTk_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.coldTk = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub txtTk_Validated(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private Sub txtTy_gia_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtTy_gia.EnabledChanged
        oVoucher.noldFCrate = New Decimal(Me.txtTy_gia.Value)
    End Sub

    Private Sub txtTy_gia_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtTy_gia.Validated
        Me.vFCRate()
    End Sub

    Private Sub txtVMa_kh_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim str2 As String = StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))
        If (StringType.StrCmp(Strings.Trim(str2), "", False) <> 0) Then
            Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmkh", ("ma_kh = '" & Strings.Trim(str2) & "'")), DataRow)
            Dim cString As String = "ten_kh, dia_chi, ma_so_thue"
            Dim num2 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            Dim i As Integer = 1
            Do While (i <= num2)
                Dim str As String = Strings.Trim(Fox.GetWordNum(cString, i, ","c))
                If (StringType.StrCmp(Strings.Trim(StringType.FromObject(row.Item(str))), "", False) <> 0) Then
                    modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item(str) = RuntimeHelpers.GetObjectValue(row.Item(str))
                End If
                i += 1
            Loop
        End If
    End Sub

    Private Sub txtVMa_kh2_Enter(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRowIndex As Integer = Me.grdOther.CurrentRowIndex
        Dim eValue As String = ""
        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(currentRowIndex).Item("tk_thue_no"))) Then
            eValue = StringType.FromObject(modVoucher.tblOther.Item(currentRowIndex).Item("tk_thue_no"))
        End If
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmtk", StringType.FromObject(ObjectType.AddObj("tk = ", Sql.ConvertVS2SQLType(eValue, "")))), DataRow)
        If (Not row Is Nothing) Then
            If (ObjectType.ObjTst(row.Item("tk_cn"), 1, False) = 0) Then
                Me.oTaxAuthority.Empty = False
            Else
                Me.oTaxAuthority.Empty = True
                If Not Me.TaxAuthority_IsFocus Then
                    Me.grdDetail.TabProcess()
                End If
                Me.TaxAuthority_IsFocus = True
            End If
        Else
            Me.oTaxAuthority.Empty = True
        End If
    End Sub

    Private Sub txtVMa_thue_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.coldVMa_thue = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub txtVMa_thue_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Byte
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num = num2
        Else
            num = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim str3 As String = Me.coldVMa_thue
        Dim str2 As String = StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))
        If (StringType.StrCmp(Strings.Trim(str2), Strings.Trim(str3), False) <> 0) Then
            Dim str As String
            Dim zero As Decimal
            If (StringType.StrCmp(Strings.Trim(str2), "", False) = 0) Then
                zero = Decimal.Zero
                str = ""
            Else
                Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmthue", ("ma_thue = '" & Strings.Trim(str2) & "'")), DataRow)
                zero = DecimalType.FromObject(row.Item("thue_suat"))
                str = StringType.FromObject(row.Item("tk_thue_no3"))
                row = Nothing
            End If
            With modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex)
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("t_tien_nt"))) Then
                    .Item("t_tien_nt") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("t_tien"))) Then
                    .Item("t_tien") = 0
                End If
                .Item("thue_suat") = zero
                .Item("tk_thue_no") = str
                .Item("ma_thue") = str2
                .Item("t_thue_nt") = Fox.Round(.Item("t_tien_nt") * zero / 100, num)
                .Item("t_thue") = Fox.Round(.Item("t_tien") * zero / 100, num2)
            End With
            Me.Valid_Ma_kh2(str, Me.grdOther.CurrentRowIndex)
            Me.grdOther.Refresh()
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtVMau_bc_Validated(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRowIndex As Integer = Me.grdOther.CurrentRowIndex
        If ((currentRowIndex >= 0) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))), "", False) <> 0)) Then
            If (Me.m_ma_thue_1 Is Nothing) Then
                Me.m_ma_thue_1 = StringType.FromObject(modVoucher.oOption.Item("m_ma_thue_1"))
                If (Sql.GetRow((modVoucher.appConn), "dmthue", ("ma_thue = '" & Me.m_ma_thue_1 & "'")) Is Nothing) Then
                    Me.m_ma_thue_1 = ""
                End If
            End If
            If (StringType.StrCmp(Me.m_ma_thue_1, "", False) <> 0) Then
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(currentRowIndex).Item("ma_thue"))) Then
                    modVoucher.tblOther.Item(currentRowIndex).Item("ma_thue") = Me.m_ma_thue_1
                    Me.coldVMa_thue = ""
                    Me.colVMa_thue.TextBox.Text = Me.m_ma_thue_1
                    Me.txtVMa_thue_valid(Me.colVMa_thue.TextBox, New EventArgs)
                ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblOther.Item(currentRowIndex).Item("ma_thue"))), "", False) = 0) Then
                    modVoucher.tblOther.Item(currentRowIndex).Item("ma_thue") = Me.m_ma_thue_1
                    Me.coldVMa_thue = ""
                    Me.colVMa_thue.TextBox.Text = Me.m_ma_thue_1
                    Me.txtVMa_thue_valid(Me.colVMa_thue.TextBox, New EventArgs)
                End If
            End If
        End If
    End Sub

    Private Sub txtVT_thue_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldVT_Thue = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtVT_thue_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldVT_Thue_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtVT_thue_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num3 As Decimal = Me.noldVT_Thue_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num3) <> 0) Then
            With modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex)
                .Item("t_thue_nt") = num
                .Item("t_thue") = Fox.Round(.Item("t_thue_nt") * Me.txtTy_gia.Value, num2)
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtVT_thue_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Decimal = Me.noldVT_Thue
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num2) <> 0) Then
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtVT_tien_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldVT_tien = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.ShowTotalAmount(1)
    End Sub

    Private Sub txtVT_tien_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldVT_tien_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.ShowTotalAmount(2)
    End Sub

    Private Sub txtVT_tien_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num5 As Decimal = Me.noldVT_tien_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num5) <> 0) Then
            Dim zero As Decimal
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item("thue_suat"))) Then
                zero = DecimalType.FromObject(modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item("thue_suat"))
            Else
                zero = Decimal.Zero
            End If
            With modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex)
                .Item("t_tien_nt") = num
                .Item("t_tien") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                .Item("t_thue_nt") = Fox.Round(.Item("t_tien_nt") * zero / 100, num2)
                .Item("t_thue") = Fox.Round(.Item("t_tien") * zero / 100, digits)
            End With
            Me.UpdateList()
        End If
        Me.ShowTotalAmount(2)
    End Sub

    Private Sub txtVT_tien_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num4 As Decimal = Me.noldVT_tien
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            Dim zero As Decimal
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item("thue_suat"))) Then
                zero = DecimalType.FromObject(modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item("thue_suat"))
            Else
                zero = Decimal.Zero
            End If
            With modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex)
                .Item("t_tien") = num
                .Item("t_thue") = Fox.Round(.Item("t_tien") * zero / 100, num2)
            End With
            Me.UpdateList()
        End If
        Me.ShowTotalAmount(1)
    End Sub

    Private Sub txtVTk_thue_no_Enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.coldVTk_thue_no = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub txtVTk_thue_no_Validated(ByVal sender As Object, ByVal e As EventArgs)
        Dim view As DataRowView = modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex)
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmtk", StringType.FromObject(ObjectType.AddObj("tk = ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(view.Item("tk_thue_no")), "")))), DataRow)
        If (Not row Is Nothing) Then
            Me.TaxAuthority_IsFocus = (ObjectType.ObjTst(row.Item("tk_cn"), 1, False) = 0)
        Else
            Me.TaxAuthority_IsFocus = True
        End If
        view = Nothing
        If (StringType.StrCmp(Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))), Me.coldVTk_thue_no, False) <> 0) Then
            Me.Valid_Ma_kh2(Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))), Me.grdOther.CurrentRowIndex)
        End If
    End Sub

    Public Sub UpdateList()
        Dim zero As Decimal = Decimal.Zero
        Dim num8 As Decimal = Decimal.Zero
        Dim num2 As Decimal = Decimal.Zero
        Dim num3 As Decimal = Decimal.Zero
        Dim num5 As Decimal = Decimal.Zero
        Dim num6 As Decimal = Decimal.Zero
        Dim num4 As Decimal = Decimal.Zero
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit", "View"}) Then
            Dim num As Integer
            Dim num10 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num10)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("tien0"))) Then
                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblDetail.Item(num).Item("tien0")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("tien_nt0"))) Then
                    num8 = DecimalType.FromObject(ObjectType.AddObj(num8, modVoucher.tblDetail.Item(num).Item("tien_nt0")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("cp"))) Then
                    num2 = DecimalType.FromObject(ObjectType.AddObj(num2, modVoucher.tblDetail.Item(num).Item("cp")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("cp_nt"))) Then
                    num3 = DecimalType.FromObject(ObjectType.AddObj(num3, modVoucher.tblDetail.Item(num).Item("cp_nt")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("so_luong"))) Then
                    num4 = DecimalType.FromObject(ObjectType.AddObj(num4, modVoucher.tblDetail.Item(num).Item("so_luong")))
                End If
                num += 1
            Loop
            Dim num9 As Integer = (modVoucher.tblOther.Count - 1)
            num = 0
            Do While (num <= num9)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(num).Item("t_thue"))) Then
                    num5 = DecimalType.FromObject(ObjectType.AddObj(num5, modVoucher.tblOther.Item(num).Item("t_thue")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(num).Item("t_thue_nt"))) Then
                    num6 = DecimalType.FromObject(ObjectType.AddObj(num6, modVoucher.tblOther.Item(num).Item("t_thue_nt")))
                End If
                num += 1
            Loop
        End If
        Me.txtT_so_luong.Value = Convert.ToDouble(num4)
        Me.txtT_cp.Value = Convert.ToDouble(num2)
        Me.txtT_cp_nt.Value = Convert.ToDouble(num3)
        Me.txtT_thue.Value = Convert.ToDouble(num5)
        Me.txtT_thue_nt.Value = Convert.ToDouble(num6)
        If Not Me.chkGia_thue_yn.Checked Then
            zero = Decimal.Subtract(zero, num5)
            num8 = Decimal.Subtract(num8, num6)
        End If
        Me.txtT_tien0.Value = Convert.ToDouble(zero)
        Me.txtT_tien_nt0.Value = Convert.ToDouble(num8)
        Me.txtT_tt.Value = ((Me.txtT_tien0.Value + Me.txtT_thue.Value) + Me.txtT_cp.Value)
        Me.txtT_tt_nt.Value = ((Me.txtT_tien_nt0.Value + Me.txtT_thue_nt.Value) + Me.txtT_cp_nt.Value)
    End Sub

    Private Sub UpdatePV()
        If Not Me.chkGia_thue_yn.Checked Then
            auditamount.DistributeAmounts(New Decimal(Me.txtT_tien_nt0.Value), "tien_nt0", "tien_hang_nt", modVoucher.tblDetail, ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt")))
            auditamount.DistributeAmounts(New Decimal(Me.txtT_tien0.Value), "tien0", "tien_hang", modVoucher.tblDetail, ByteType.FromObject(modVoucher.oVar.Item("m_round_tien")))
            auditamount.AuditAmounts(New Decimal(Me.txtT_tien_nt0.Value), "tien_hang_nt", modVoucher.tblDetail)
            auditamount.AuditAmounts(New Decimal(Me.txtT_tien0.Value), "tien_hang", modVoucher.tblDetail)
        End If
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
        num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
        Dim num4 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num4)
            With modVoucher.tblDetail.Item(i)
                If Me.chkGia_thue_yn.Checked Then
                    .Item("Tien_hang") = RuntimeHelpers.GetObjectValue(.Item("tien0"))
                    .Item("Tien_hang_nt") = RuntimeHelpers.GetObjectValue(.Item("tien_nt0"))
                End If
                .Item("tien") = ObjectType.AddObj(.Item("tien_hang"), .Item("cp"))
                .Item("tien_nt") = ObjectType.AddObj(.Item("tien_hang_nt"), .Item("cp_nt"))
                .Item("tt") = ObjectType.AddObj(.Item("tien"), .Item("thue"))
                .Item("tt_nt") = ObjectType.AddObj(.Item("tien_nt"), .Item("thue_nt"))
                If (ObjectType.ObjTst(.Item("so_luong"), 0, False) = 0) Then
                    .Item("gia") = 0
                    .Item("gia_nt") = 0
                Else
                    .Item("gia_nt") = Fox.Round(.Item("tien_nt") / .Item("so_luong"), num2)
                    .Item("gia") = Fox.Round(.Item("tien") / .Item("so_luong"), num3)
                End If
            End With
            i += 1
        Loop
    End Sub

    Private Sub Valid_Ma_kh2(ByVal acct As String, ByVal index As Integer)
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmtk", StringType.FromObject(ObjectType.AddObj("tk = ", Sql.ConvertVS2SQLType(acct, "")))), DataRow)
        If (Not row Is Nothing) Then
            If (ObjectType.ObjTst(row.Item("tk_cn"), 1, False) <> 0) Then
                modVoucher.tblOther.Item(index).Item("ma_kh2") = ""
                modVoucher.tblOther.Item(index).Item("ten_kh2tmp") = ""
            End If
        Else
            modVoucher.tblOther.Item(index).Item("ma_kh2") = ""
            modVoucher.tblOther.Item(index).Item("ten_kh2tmp") = ""
        End If
    End Sub

    Private Sub VATCarryOn(ByVal tblDetail As DataView, ByVal iRow As Integer)
        Me.pnContent.Text = StringType.FromObject(oVoucher.oClassMsg.Item("034"))
        If Not ((iRow < 1) Or (tblDetail.Count <= 1)) Then
            Dim cString As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "vatcarry", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            If (ObjectType.ObjTst(modVoucher.oOption.Item("m_po_tt"), "1", False) = 0) Then
                tblDetail.Item(iRow).Item("ma_tt") = Me.txtMa_tt.Text
            End If
            Dim num2 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            Dim i As Integer = 1
            Do While (i <= num2)
                Dim str As String = Strings.Trim(Fox.GetWordNum(cString, i, ","c))
                tblDetail.Item(iRow).Item(str) = RuntimeHelpers.GetObjectValue(tblDetail.Item((iRow - 1)).Item(str))
                i += 1
            Loop
        End If
    End Sub

    Public Sub vCaptionRefresh()
        Me.EDFC()
        Dim cAction As String = oVoucher.cAction
        If ((StringType.StrCmp(cAction, "Edit", False) = 0) OrElse (StringType.StrCmp(cAction, "View", False) = 0)) Then
            Me.pnContent.Text = StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.tblMaster.Item(Me.iMasterRow).Item("status"), "2", False) <> 0), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("018")), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("019"))))
        Else
            Me.pnContent.Text = ""
        End If
    End Sub

    Public Sub vFCRate()
        If (Me.txtTy_gia.Value <> Convert.ToDouble(oVoucher.noldFCrate)) Then
            Dim num As Integer
            Dim num4 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num4)
                With tblDetail.Item(num)
                    If Not Information.IsDBNull(.Item("gia_nt0")) Then
                        .Item("gia0") = Fox.Round(.Item("gia_nt0") * Me.txtTy_gia.Value, IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia")))
                    End If
                    If Not Information.IsDBNull(.Item("tien_nt0")) Then
                        .Item("tien0") = Fox.Round(.Item("tien_nt0") * Me.txtTy_gia.Value, IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien")))
                    End If
                    If Not Information.IsDBNull(.Item("cp_vc_nt")) Then
                        .Item("cp_vc") = Fox.Round(.Item("cp_vc_nt") * Me.txtTy_gia.Value, IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien")))
                    End If
                    If Not Information.IsDBNull(.Item("cp_bh_nt")) Then
                        .Item("cp_bh") = Fox.Round(.Item("cp_bh_nt") * Me.txtTy_gia.Value, IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien")))
                    End If
                    If Not Information.IsDBNull(.Item("cp_khac_nt")) Then
                        .Item("cp_khac") = Fox.Round(.Item("cp_khac_nt") * Me.txtTy_gia.Value, IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien")))
                    End If
                    If Not Information.IsDBNull(.Item("cp_nt")) Then
                        .Item("cp") = Fox.Round(.Item("cp_nt") * Me.txtTy_gia.Value, IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien")))
                    End If
                End With
                num += 1
            Loop
            Dim num3 As Integer = (modVoucher.tblCharge.Count - 1)
            num = 0
            Do While (num <= num3)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblCharge.Item(num).Item("tien_cp_nt"))) Then
                    tblCharge.Item(num).Item("tien_cp") = Fox.Round(tblCharge.Item(num).Item("tien_cp_nt") * Me.txtTy_gia.Value, IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien")))
                End If
                num += 1
            Loop
            Dim num2 As Integer = (modVoucher.tblOther.Count - 1)
            num = 0
            Do While (num <= num2)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblOther.Item(num).Item("t_tien_nt"))) Then
                    tblOther.Item(num).Item("t_tien") = Fox.Round(tblOther.Item(num).Item("t_tien_nt") * Me.txtTy_gia.Value, IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblOther.Item(num).Item("t_thue_nt"))) Then
                    tblOther.Item(num).Item("t_thue") = Fox.Round(tblOther.Item(num).Item("t_thue_nt") * Me.txtTy_gia.Value, IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien")))
                End If
                num += 1
            Loop
        End If
        Me.txtT_tien0.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_tien_nt0.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_cp.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_cp_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_tt.Value = ((Me.txtT_tien0.Value + Me.txtT_thue.Value) + Me.txtT_cp.Value)
        Me.txtT_tt_nt.Value = ((Me.txtT_tien_nt0.Value + Me.txtT_thue_nt.Value) + Me.txtT_cp_nt.Value)
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
        Fill2Grid.Fill(sysConn, tblMaster, gridformtran2, tbs, cols, "PVMaster")
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
        Fill2Grid.Fill(sysConn, tblDetail, gridformtran, style, cols, "PVDetail")
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
        Dim zero As Decimal = Decimal.Zero
        Dim num5 As Integer = (count - 1)
        index = 0
        Do While (index <= num5)
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(index).Item("t_tt"))) Then
                zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblMaster.Item(index).Item("t_tt")))
            End If
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(index).Item("t_tt_nt"))) Then
                num3 = DecimalType.FromObject(ObjectType.AddObj(num3, modVoucher.tblMaster.Item(index).Item("t_tt_nt")))
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

    Private Sub WhenChargeLeave(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Dim newValue As String
        newValue = Trim(sender.Text)
        If Trim(newValue) = Trim(coldCMa_cp) Then
            Return
        End If
        With tblCharge(grdCharge.CurrentRowIndex)
            If Not clsfields.isEmpty(.Item("ma_cp"), "C") Then
                .Item("loai_cp") = Sql.GetValue(appConn, "dmcp", "loai_cp", "ma_loai = '" + newValue + "'")
                .Item("loai_pb") = Sql.GetValue(appConn, "dmcp", "loai_pb", "ma_loai = '" + newValue + "'")
            Else
                .Item("tien_cp_nt") = 0
                .Item("tien_cp") = 0
            End If
        End With
    End Sub

    Private Sub WhenItemLeave(ByVal sender As Object, ByVal e As EventArgs)
        If oInvItemDetail.Cancel Then
            Return
        End If
        Dim newValue As String
        newValue = Trim(sender.Text)
        If Trim(newValue) = Trim(cOldItem) Then
            Return
        End If
        With tblDetail(grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(.Item("ma_vt"), "C") Then
                Dim cItem As String, dr As DataRow
                cItem = Trim(.Item("ma_vt"))
                dr = Sql.GetRow(appConn, "dmvt", "ma_vt = '" + cItem + "'")
                .Item("volume") = dr.Item("volume")
                .Item("weight") = dr.Item("weight")
                ' Item Account
                If Not dr.Item("sua_tk_vt") Then
                    .Item("tk_vt") = dr.Item("tk_vt")
                Else
                    If clsfields.isEmpty(.Item("tk_vt"), "C") Then
                        .Item("tk_vt") = dr.Item("tk_vt")
                    End If
                End If
                ' UOM
                .Item("dvt") = dr.Item("dvt")
                colDvt.TextBox.Text = .Item("dvt")
                .Item("he_so") = 1
                If dr.Item("nhieu_dvt") Then
                    oUOM.Empty = False
                    colDvt.ReadOnly = False
                    oUOM.Cancel = False
                    oUOM.Check = True
                Else
                    oUOM.Empty = True
                    colDvt.ReadOnly = True
                    oUOM.Cancel = True
                    oUOM.Check = False
                End If
                ' Lot
                If Not dr.Item("lo_yn") Then
                    .Item("ma_lo") = ""
                End If
                ' Default site
                If clsfields.isEmpty(.Item("ma_kho"), "C") Then
                    Try
                        .Item("ma_kho") = Sql.GetValue(appConn, "select min(ma_kho) from dmkho where dbo.ff_inlist(ma_kho,'" + RuntimeHelpers.GetObjectValue(dr.Item("ma_kho")) + "')=1 and ma_dvcs='" + Me.txtMa_dvcs.Text + "'")
                    Catch ex As Exception
                    End Try
                End If
                ' Default location
                If clsfields.isEmpty(.Item("ma_vi_tri"), "C") Then
                    .Item("ma_vi_tri") = dr.Item("ma_vi_tri")
                End If
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
            Me.oLot.Key = cKey
            Me.oLot.Empty = (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmlo", "ma_lo", cKey))), "", False) = 0)
        End If
        view = Nothing
    End Sub

    Private Sub WhenNoneCustomer(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        If Not IsDBNull(tblOther(grdOther.CurrentRowIndex).Item("ma_kh")) Then
            Dim cCust As String
            cCust = tblOther(grdOther.CurrentRowIndex).Item("ma_kh")
            If Trim(Sql.GetValue(appConn, "dmkh", "ma_so_thue", "ma_kh = '" + cCust + "'")) <> "" Then
                grdDetail.TabProcess()
            End If
        End If
    End Sub

    Private Sub WhenNoneInputItemAccount(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        With tblDetail(grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(.Item("ma_vt"), "C") Then
                Dim cItem As String
                cItem = Trim(.Item("ma_vt"))
                If Not Sql.GetValue(appConn, "dmvt", "sua_tk_vt", "ma_vt = '" + cItem + "'") Then
                    grdDetail.TabProcess()
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
                If Not ((StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.cOldSite), False) = 0) And Not clsfields.isEmpty(.Item("ten_kho"), "C")) Then
                    If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkho", "dai_ly_yn", ("ma_kho = '" & str & "'"))) Then
                        Dim str3 As String = Strings.Trim(StringType.FromObject(.Item("ma_vt")))
                        Dim sLeft As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "tk_dl", ("ma_vt = '" & str3 & "'"))))
                        If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                            .Item("tk_vt") = sLeft
                        End If
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub WhenUOMEnter(ByVal sender As Object, ByVal e As EventArgs)
        Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
            view = Nothing
            Return
        End If
        If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "'"))) Then
            Dim str As String = ("(ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "' OR ma_vt = '*')")
            Me.oUOM.Key = str
            Me.oUOM.Empty = False
            Me.colDvt.ReadOnly = False
            Me.oUOM.Cancel = False
            Me.oUOM.Check = True
            view = Nothing
            Return
        End If
        Me.oUOM.Key = "1=1"
        Me.oUOM.Empty = True
        Me.colDvt.ReadOnly = True
        Me.oUOM.Cancel = True
        Me.oUOM.Check = False
        view = Nothing
    End Sub

    Private Sub WhenUOMLeave(ByVal sender As Object, ByVal e As EventArgs)
        With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If clsfields.isEmpty(.Item("ma_vt"), "C") Then
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

    Private Sub RetrieveItemsFromIR_NB()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim _date As New frmFilterPN
            AddHandler _date.Load, New EventHandler(AddressOf Me.frmRetrieveLoad)
            If (_date.ShowDialog = DialogResult.OK) Then
                Dim str3 As String = "1=1"
                If (ObjectType.ObjTst(_date.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                    str3 += " AND (a.ngay_ct >= " + Sql.ConvertVS2SQLType(_date.txtNgay_ct.Value, "") + ") "
                    str3 += " AND (a.ngay_ct <= " + Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "") + ")"
                End If
                If (_date.txtMa_vt.Text <> "") Then
                    str3 += " AND (a.ma_vt like '" + _date.txtMa_vt.Text.Trim.Replace("'", "''") + "') "
                End If
                If (_date.txtMa_lo.Text <> "") Then
                    str3 += " AND (a.ma_lo like '" + _date.txtMa_lo.Text.Trim.Replace("'", "''") + "') "
                End If
                Dim strSQLLong As String = str3
                Dim tcSQL As String = String.Concat(New String() {"EXEC spSearchIRTran4PV '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph74', 'ct74'"})
                tcSQL += ",'" + Replace(Me.txtMa_kh.Text.Trim, "'", "''") + "'"
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
                    frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("052"))
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
                    Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "IRMaster")
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
                    Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "IRDetail4Receipt")
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
                    Me.tblRetrieveDetail.AllowDelete = False
                    Me.tblRetrieveDetail.AllowNew = False
                    gridformtran.TableStyles.Item(0).GridColumnStyles.Item(0).ReadOnly = False
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
                    button4.Text = StringType.FromObject(modVoucher.oLan.Item("043"))
                    button4.Width = 100
                    button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                    button2.Top = button4.Top
                    button2.Left = (button4.Left + 110)
                    button2.Visible = True
                    button2.Text = StringType.FromObject(modVoucher.oLan.Item("044"))
                    button2.Width = 120
                    button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                    button3.Top = button4.Top
                    button3.Left = (button2.Left + 130)
                    button3.Visible = True
                    button3.Text = StringType.FromObject(modVoucher.oLan.Item("045"))
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
                    Me.tblRetrieveDetail.RowFilter = "Tag=1"
                    Me.tblRetrieveDetail.Sort = "ngay_ct, so_ct, stt_rec, stt_rec0"
                    Dim num7 As Integer = (Me.tblRetrieveDetail.Count - 1)
                    index = 0
                    Do While (index <= num7)
                        With Me.tblRetrieveDetail.Item(index)
                            .Item("stt_rec_pn") = RuntimeHelpers.GetObjectValue(.Item("stt_rec"))
                            .Item("stt_rec0pn") = RuntimeHelpers.GetObjectValue(.Item("stt_rec0"))
                            '.Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("sl_xuat0"))
                            .Row.AcceptChanges()
                        End With
                        index += 1
                    Loop
                    Me.tblRetrieveDetail.RowFilter = "Tag = 1"
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
                            ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_pn")), "C") Then
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

    ' Properties
    Friend WithEvents cboAction As ComboBox
    Friend WithEvents cboStatus As ComboBox
    Friend WithEvents chkGia_thue_yn As CheckBox
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
    Friend WithEvents grdCharge As clsgrid
    Friend WithEvents grdDetail As clsgrid
    Friend WithEvents grdOther As clsgrid
    Friend WithEvents Label1 As Label
    Friend WithEvents lblAction As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMa_gd As Label
    Friend WithEvents lblMa_kh As Label
    Friend WithEvents lblMa_tt As Label
    Friend WithEvents lblNgay_ct As Label
    Friend WithEvents lblNgay_hd As Label
    Friend WithEvents lblNgay_lct As Label
    Friend WithEvents lblOng_ba As Label
    Friend WithEvents lblSo_ct As Label
    Friend WithEvents lblSo_hd As Label
    Friend WithEvents lblSo_seri As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblStatusMess As Label
    Friend WithEvents lblTen As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_gd As Label
    Friend WithEvents lblTen_kh As Label
    Friend WithEvents lblTen_tk As Label
    Friend WithEvents lblTen_tt As Label
    Friend WithEvents lblTien_thue As Label
    Friend WithEvents lblTien_tt As Label
    Friend WithEvents lblTk As Label
    Friend WithEvents lblTotal As Label
    Friend WithEvents lblTy_gia As Label
    Friend WithEvents lvlT_cp As Label
    Friend WithEvents tbDetail As TabControl
    Friend WithEvents tbgCharge As TabPage
    Friend WithEvents tpgDetail As TabPage
    Friend WithEvents tpgOther As TabPage
    Friend WithEvents txtDien_giai As TextBox
    Friend WithEvents txtKeyPress As TextBox
    Friend WithEvents txtLoai_ct As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_gd As TextBox
    Friend WithEvents txtMa_kh As TextBox
    Friend WithEvents txtMa_tt As TextBox
    Friend WithEvents txtNgay_ct As txtDate
    Friend WithEvents txtNgay_ct0 As txtDate
    Friend WithEvents txtNgay_lct As txtDate
    Friend WithEvents txtOng_ba As TextBox
    Friend WithEvents txtSo_ct As TextBox
    Friend WithEvents txtSo_ct0 As TextBox
    Friend WithEvents txtSo_seri0 As TextBox
    Friend WithEvents txtStatus As TextBox
    Friend WithEvents txtT_cp As txtNumeric
    Friend WithEvents txtT_cp_nt As txtNumeric
    Friend WithEvents txtT_so_luong As txtNumeric
    Friend WithEvents txtT_thue As txtNumeric
    Friend WithEvents txtT_thue_nt As txtNumeric
    Friend WithEvents txtT_tien_nt0 As txtNumeric
    Friend WithEvents txtT_tien0 As txtNumeric
    Friend WithEvents txtT_tt As txtNumeric
    Friend WithEvents txtT_tt_nt As txtNumeric
    Friend WithEvents txtTk As TextBox
    Friend WithEvents txtTy_gia As txtNumeric


End Class

