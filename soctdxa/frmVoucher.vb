Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol.clsvoucher.clsVoucher
Imports libscontrol
Imports libscommon
Imports libscontrol.voucherseachlib

Public Class frmVoucher
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Activated, New EventHandler(AddressOf Me.frmVoucher_Activated)
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmVoucher_Load)
        Me.arrControlButtons = New Button(13 - 1) {}
        'Me.oTitleButton = New TitleButton(Me)
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
        Dim obj2 As Object = "stt_rec is null or stt_rec = ''"
        Me.grdHeader.ScatterBlank()
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
        Me.RefreshCharge(0)
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
        Me.grdCharge.ReadOnly = False
        Me.oSecurity.SetReadOnly()
        xtabControl.ReadOnlyTabControls(False, Me.tbDetail)
        xtabControl.ScatterMemvarBlankTabControl(Me.tbDetail)
        Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
    End Sub

    'Private Sub AfterUpdateSO(ByVal lcIDNumber As String, ByVal lcAction As String)
    '    Dim tcSQL As String = String.Concat(New String() {"fs_AfterUpdateSO '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
    '    Sql.SQLExecute((modVoucher.appConn), tcSQL)
    'End Sub
    Private Function AfterUpdateSO(ByVal lcIDNumber As String, ByVal lcAction As String) As String
        Dim tcSQL As String = String.Concat(New String() {"EXEC fs_AfterUpdateSO '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
        Return tcSQL
    End Function

    Private Sub AllocateBy(ByVal nAmount As Decimal, ByVal nTQ As Decimal, ByVal cQ As String, ByVal cField As String, ByVal nRound As Integer)
        If (Decimal.Compare(nTQ, Decimal.Zero) <> 0) Then
            Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(i).Item(cQ))) Then
                    Return
                End If
                Dim str As String = cField
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(nAmount, tblDetail.Item(i).Item(cQ)), nTQ), nRound}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    nRound = IntegerType.FromObject(args(1))
                End If
                tblDetail.Item(i).Item(str) = ObjectType.AddObj(tblDetail.Item(i).Item(str), LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                i += 1
            Loop
        End If
    End Sub

    Private Sub AllocateBy(ByVal nAmount As Decimal, ByVal nTQ As Decimal, ByVal cQ As String, ByVal cField As String, ByVal nRound As Integer, ByVal cQty As String)
        If (Decimal.Compare(nTQ, Decimal.Zero) = 0) Then
            Return
        End If
        Dim num5 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim num As Integer = 0
        For num = 0 To num5
            If (Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item(cQ))) Or Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item(cQty)))) Then
                Return
            End If
            Dim str As String = cField
            Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.MulObj(ObjectType.MulObj(nAmount, tblDetail.Item(num).Item("so_luong")), tblDetail.Item(num).Item("he_so")), tblDetail.Item(num).Item(cQ)), nTQ), nRound}
            Dim copyBack As Boolean() = New Boolean() {False, True}
            If copyBack(1) Then
                nRound = IntegerType.FromObject(args(1))
            End If
            tblDetail.Item(num).Item(str) = ObjectType.AddObj(tblDetail.Item(num).Item(str), LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
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
                tblDetail.Item(num).Item("cp_vc_nt") = 0
                tblDetail.Item(num).Item("cp_bh_nt") = 0
                tblDetail.Item(num).Item("cp_khac_nt") = 0
                tblDetail.Item(num).Item("cp_vc") = 0
                tblDetail.Item(num).Item("cp_bh") = 0
                tblDetail.Item(num).Item("cp_khac") = 0
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("so_luong"))) Then
                    tblDetail.Item(num).Item("so_luong") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("he_so"))) Then
                    tblDetail.Item(num).Item("he_so") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("volume"))) Then
                    tblDetail.Item(num).Item("volume") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("weight"))) Then
                    tblDetail.Item(num).Item("weight") = 0
                End If
                num8 = DecimalType.FromObject(ObjectType.AddObj(num8, ObjectType.MulObj(ObjectType.MulObj(tblDetail.Item(num).Item("volume"), tblDetail.Item(num).Item("so_luong")), tblDetail.Item(num).Item("he_so"))))
                zero = DecimalType.FromObject(ObjectType.AddObj(zero, ObjectType.MulObj(ObjectType.MulObj(tblDetail.Item(num).Item("weight"), tblDetail.Item(num).Item("so_luong")), tblDetail.Item(num).Item("he_so"))))
                num += 1
            Loop
            Dim num10 As Integer = (modVoucher.tblCharge.Count - 1)
            num = 0
            Do While (num <= num10)
                If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblCharge.Item(num).Item("ma_cp"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(tblCharge.Item(num).Item("ma_cp"))), "", False) <> 0)) Then
                    Dim str3 As String = ""
                    Dim str4 As String = ""
                    Dim str5 As String = ""
                    Dim str6 As String = ""
                    Dim num5 As Decimal = 0
                    Dim num7 As Decimal = 0
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblCharge.Item(num).Item("tien_cp_nt"))) Then
                        tblCharge.Item(num).Item("tien_cp_nt") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblCharge.Item(num).Item("tien_cp"))) Then
                        tblCharge.Item(num).Item("tien_cp") = 0
                    End If
                    Dim str2 As String = Strings.Trim(StringType.FromObject(tblCharge.Item(num).Item("loai_cp")))
                    Dim str As String = Strings.Trim(StringType.FromObject(tblCharge.Item(num).Item("loai_pb")))
                    Dim nAmount As Decimal = DecimalType.FromObject(tblCharge.Item(num).Item("tien_cp_nt"))
                    Dim num2 As Decimal = DecimalType.FromObject(tblCharge.Item(num).Item("tien_cp"))
                    Dim sLeft As String = str2
                    If (StringType.StrCmp(sLeft, "1", False) = 0) Then
                        str5 = "cp_vc"
                        str3 = "cp_vc_nt"
                    ElseIf (StringType.StrCmp(sLeft, "2", False) = 0) Then
                        str5 = "cp_bh"
                        str3 = "cp_bh_nt"
                    ElseIf (StringType.StrCmp(sLeft, "3", False) = 0) Then
                        str5 = "cp_khac"
                        str3 = "cp_khac_nt"
                    End If
                    Dim str7 As String = str
                    If (StringType.StrCmp(str7, "1", False) = 0) Then
                        str6 = "so_luong"
                        str4 = "so_luong"
                        num7 = New Decimal(Me.txtT_so_luong.Value)
                        num5 = New Decimal(Me.txtT_so_luong.Value)
                        Me.AllocateBy(num2, num7, str6, str5, nRound)
                        Me.AllocateBy(nAmount, num5, str4, str3, num4)
                    ElseIf (StringType.StrCmp(str7, "3", False) = 0) Then
                        str6 = "weight"
                        str4 = "weight"
                        num7 = zero
                        num5 = zero
                        Me.AllocateBy(num2, num7, str6, str5, nRound, "so_luong")
                        Me.AllocateBy(nAmount, num5, str4, str3, num4, "so_luong")
                    ElseIf (StringType.StrCmp(str7, "2", False) = 0) Then
                        str6 = "volume"
                        str4 = "volume"
                        num7 = num8
                        num5 = num8
                        Me.AllocateBy(num2, num7, str6, str5, nRound, "so_luong")
                        Me.AllocateBy(nAmount, num5, str4, str3, num4, "so_luong")
                    ElseIf (StringType.StrCmp(str7, "4", False) = 0) Then
                        str6 = "tien2"
                        str4 = "tien_nt2"
                        num7 = New Decimal(Me.txtT_tien2.Value)
                        num5 = New Decimal(Me.txtT_tien_nt2.Value)
                        Me.AllocateBy(num2, num7, str6, str5, nRound)
                        Me.AllocateBy(nAmount, num5, str4, str3, num4)
                    End If
                End If
                num += 1
            Loop
            Me.AuditCharge()
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
            'Dim view2 As DataRowView = modVoucher.tblCharge.Item(num)
            If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblCharge.Item(num).Item("ma_cp"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(tblCharge.Item(num).Item("ma_cp"))), "", False) <> 0)) Then
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblCharge.Item(num).Item("tien_cp_nt"))) Then
                    tblCharge.Item(num).Item("tien_cp_nt") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblCharge.Item(num).Item("tien_cp"))) Then
                    tblCharge.Item(num).Item("tien_cp") = 0
                End If
                Dim sLeft As String = Strings.Trim(StringType.FromObject(tblCharge.Item(num).Item("loai_cp")))
                If (StringType.StrCmp(sLeft, "1", False) = 0) Then
                    num7 = DecimalType.FromObject(ObjectType.AddObj(num7, tblCharge.Item(num).Item("tien_cp_nt")))
                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, tblCharge.Item(num).Item("tien_cp")))
                ElseIf (StringType.StrCmp(sLeft, "2", False) = 0) Then
                    num3 = DecimalType.FromObject(ObjectType.AddObj(num3, tblCharge.Item(num).Item("tien_cp_nt")))
                    num2 = DecimalType.FromObject(ObjectType.AddObj(num2, tblCharge.Item(num).Item("tien_cp")))
                ElseIf (StringType.StrCmp(sLeft, "3", False) = 0) Then
                    num5 = DecimalType.FromObject(ObjectType.AddObj(num5, tblCharge.Item(num).Item("tien_cp_nt")))
                    num4 = DecimalType.FromObject(ObjectType.AddObj(num4, tblCharge.Item(num).Item("tien_cp")))
                End If
            End If
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
            tblDetail.Item(num).Item("cp_nt") = ObjectType.AddObj(ObjectType.AddObj(tblDetail.Item(num).Item("cp_vc_nt"), tblDetail.Item(num).Item("cp_bh_nt")), tblDetail.Item(num).Item("cp_khac_nt"))
            tblDetail.Item(num).Item("cp") = ObjectType.AddObj(ObjectType.AddObj(tblDetail.Item(num).Item("cp_vc"), tblDetail.Item(num).Item("cp_bh")), tblDetail.Item(num).Item("cp_khac"))
            num += 1
        Loop
        Me.UpdateList()
    End Sub

    'Private Sub BeforUpdateSO(ByVal lcIDNumber As String, ByVal lcAction As String)
    '    Dim tcSQL As String = String.Concat(New String() {"fs_BeforUpdateSO '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
    '    Sql.SQLExecute((modVoucher.appConn), tcSQL)
    'End Sub
    Function BeforUpdateSO(ByVal lcIDNumber As String, ByVal lcAction As String) As String
        Dim tcSQL As String = String.Concat(New String() {"EXEC fs_BeforUpdateSO '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
        Return tcSQL
    End Function

    Public Sub Cancel()
        Dim num2 As Integer
        Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
        If (currentRowIndex >= 0) Then
            Me.grdDetail.Select(currentRowIndex)
        End If
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            Me.RefreshCharge(0)
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
                xtabControl.ScatterTabControl(modVoucher.tblMaster.Item(Me.iOldMasterRow), Me.tbDetail)
                Me.RefreshCharge(1)
            End If
            Me.EDTranType()
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

    Private Function CheckCredit() As Boolean
        Dim str As String = "EXEC fs_CheckCredit "
        If (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) Then
            str = StringType.FromObject(ObjectType.AddObj(str, ObjectType.AddObj(ObjectType.AddObj("'", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'")))
        Else
            str = (str & "'New'")
        End If
        str = StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj((((((str & ", '" & modVoucher.VoucherCode & "'") & ", '" & Strings.Trim(Me.txtLoai_ct.Text) & "'") & ", '" & Strings.Trim(Strings.Left(Me.cboAction.Text, 1)) & "'") & ", '" & Strings.Trim(Me.txtMa_kh.Text) & "'") & ", '" & Strings.Trim(Me.txtMa_tt.Text) & "'"), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Me.txtT_tt.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, ""))))
        Return (ByteType.FromObject(Sql.GetValue(modVoucher.appConn, str)) = 1)
    End Function

    Private Sub chkCk_thue_yn_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCk_thue_yn.CheckedChanged
        Dim num5 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim iRow As Integer = 0
        For iRow = 0 To num5
            Me.RecalcTax(iRow, 2)
        Next
        Me.UpdateList()
    End Sub

    Public Sub Delete()
        If Me.oSecurity.GetStatusDelelete Then
            Dim num As Integer
            Dim tcSQL As String
            Dim str5 As String
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
            If (ObjectType.ObjTst(modVoucher.oVar.Item("m_pack_yn"), 0, False) = 0) Then
                str5 = "ctcp20, ct70, ct90, ct84, ph84"
                tcSQL = ""
            Else
                str5 = (Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & ", ctcp20, ct70, ct90, ct84, ph84")
                tcSQL = GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)
            End If
            Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(str5, ","c))
            num = 1
            Do While (num <= num3)
                Dim cTable As String = Strings.Trim(Fox.GetWordNum(str5, num, ","c))
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
            If (ObjectType.ObjTst(modVoucher.oVar.Item("m_pack_yn"), 0, False) = 0) Then
                tcSQL = ((String.Concat(New String() {tcSQL, ChrW(13), "UPDATE ", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), " SET Status = '*'"}) & ", datetime2 = GETDATE(), user_id2 = " & StringType.FromObject(Reg.GetRegistryKey("CurrUserId"))) & "  WHERE " & cKey)
            End If
            tcSQL = Me.BeforUpdateSO(lcIDNumber, "Del") + Chr(13) + tcSQL
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
                tblDetail.Item(currentRowIndex).Delete()
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
                AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), False)
                tblCharge.Item(currentRowIndex).Delete()
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
            ChangeFormatColumn(Me.colGia_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia")))
            ChangeFormatColumn(Me.colTien_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colCk_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colCTien_cp_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            Me.colTien_nt2.HeaderText = StringType.FromObject(modVoucher.oLan.Item("018"))
            Me.colGia_ban_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("021"))
            Me.colCk_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("028"))
            Me.colThue_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("027"))
            Me.colGia_nt2.HeaderText = StringType.FromObject(modVoucher.oLan.Item("032"))
            Me.colCTien_cp_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("018"))
            Me.txtT_tien_nt2.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_cp_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_ck_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tien_nt2.Value = Me.txtT_tien_nt2.Value
            Me.txtT_thue_nt.Value = Me.txtT_thue_nt.Value
            Me.txtT_tt_nt.Value = Me.txtT_tt_nt.Value
            Me.txtT_ck_nt.Value = Me.txtT_ck_nt.Value
            Me.txtT_cp_nt.Value = Me.txtT_cp_nt.Value
            Try
                Me.colTien2.MappingName = "H1"
                Me.colGia2.MappingName = "H4"
                Me.colCk.MappingName = "H6"
                Me.colThue.MappingName = "H8"
                Me.colGia_ban.MappingName = "H7"
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Try
                Me.colCTien_cp.MappingName = "H5"
            Catch exception3 As Exception
                ProjectData.SetProjectError(exception3)
                Dim exception As Exception = exception3
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien2.Visible = False
            Me.txtT_thue.Visible = False
            Me.txtT_tt.Visible = False
            Me.txtT_ck.Visible = False
            Me.txtT_cp.Visible = False
        Else
            Me.txtTy_gia.Enabled = True
            ChangeFormatColumn(Me.colGia_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
            ChangeFormatColumn(Me.colTien_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colCk_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colCTien_cp_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            Me.colTien_nt2.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("019")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colGia_nt2.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("033")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colGia_ban_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("023")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colCk_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("031")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colThue_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("026")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colCTien_cp_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("019")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.txtT_tien_nt2.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_ck_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_cp_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tien_nt2.Value = Me.txtT_tien_nt2.Value
            Me.txtT_thue_nt.Value = Me.txtT_thue_nt.Value
            Me.txtT_tt_nt.Value = Me.txtT_tt_nt.Value
            Me.txtT_ck_nt.Value = Me.txtT_ck_nt.Value
            Me.txtT_cp_nt.Value = Me.txtT_cp_nt.Value
            Try
                Me.colTien2.MappingName = "tien2"
                Me.colGia2.MappingName = "gia2"
                Me.colThue.MappingName = "thue"
                If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) <> 0) Then
                    Me.colCk.MappingName = "ck"
                    Me.colGia_ban.MappingName = "gia_ban"
                End If
            Catch exception4 As Exception
                ProjectData.SetProjectError(exception4)
                ProjectData.ClearProjectError()
            End Try
            Try
                Me.colCTien_cp.MappingName = "tien_cp"
            Catch exception5 As Exception
                ProjectData.SetProjectError(exception5)
                Dim exception2 As Exception = exception5
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien2.Visible = True
            Me.txtT_thue.Visible = True
            Me.txtT_tt.Visible = True
            Me.txtT_ck.Visible = True
            Me.txtT_cp.Visible = True
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
            Me.txtMa_gd.Focus()
        End If
        Me.EDTBColumns()
        Me.InitFlowHandling(Me.cboAction)
        Me.EDStatus()
        Me.grdCharge.ReadOnly = False
        Me.oSecurity.SetReadOnly()
        If Not Me.oSecurity.GetStatusEdit Then
            Me.cmdSave.Enabled = False
        End If
        xtabControl.ReadOnlyTabControls(False, Me.tbDetail)
        Me.EDTrans()
        Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
    End Sub

    Private Sub EditAllocatedCharge(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Me.frmView = New Form
            Me.grdMV = New gridformtran
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
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), (grdMV), (tbs), (cols), "SOECharge")
            index = 0
            Do
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                cols(index).TextBox.Enabled = ((index >= 2) And (index <= 7))
                index += 1
            Loop While (index < MaxColumns)
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
                    tblDetail.Item(index).Item("cp_nt") = ObjectType.AddObj(ObjectType.AddObj(tblDetail.Item(index).Item("cp_vc_nt"), tblDetail.Item(index).Item("cp_bh_nt")), tblDetail.Item(index).Item("cp_khac_nt"))
                    tblDetail.Item(index).Item("cp") = ObjectType.AddObj(ObjectType.AddObj(tblDetail.Item(index).Item("cp_vc"), tblDetail.Item(index).Item("cp_bh")), tblDetail.Item(index).Item("cp_khac"))
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
        Me.grdCharge.ReadOnly = Not Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            modVoucher.tbcCharge(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            index += 1
        Loop While (index < MaxColumns)
        Try
            Me.colTen_vt.TextBox.Enabled = False
            Me.colSl_xuat.TextBox.Enabled = False
            Me.colSl_hd.TextBox.Enabled = False
            Me.colThue_suat.TextBox.Enabled = False
            Me.colGia_ban_nt.TextBox.Enabled = False
            Me.colGia_ban.TextBox.Enabled = False
            Me.colCTen_cp.TextBox.Enabled = False
            GetColumn(Me.grdDetail, "ton_order").TextBox.Enabled = False
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
    End Sub

    Private Sub EDTBColumns(ByVal lED As Boolean)
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index).TextBox.Enabled = lED
            modVoucher.tbcCharge(index).TextBox.Enabled = lED
            index += 1
        Loop While (index < MaxColumns)
        Try
            Me.colCTen_cp.TextBox.Enabled = False
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Me.EDStatus(lED)
    End Sub

    Private Sub EDTrans()
        Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
        Me.txtNgay_ct3.Enabled = (StringType.StrCmp(Strings.Trim(Me.txtLoai_ct.Text), "2", False) = 0)
        Me.txtNgay_ct0.Enabled = (StringType.StrCmp(Strings.Trim(Me.txtLoai_ct.Text), "3", False) = 0)
        Me.txtSo_ct0.Enabled = (StringType.StrCmp(Strings.Trim(Me.txtLoai_ct.Text), "3", False) = 0)
    End Sub

    Private Sub EDTranType()
        'Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
        'If (StringType.StrCmp(Strings.Trim(Me.txtLoai_ct.Text), "3", False) = 0) Then
        '    Me.colMa_lo.MappingName = "ma_lo"
        '    Me.colMa_vi_tri.MappingName = "ma_vi_tri"
        'Else
        '    Me.colMa_lo.MappingName = "T1"
        '    Me.colMa_vi_tri.MappingName = "T2"
        'End If
    End Sub

    Private Sub EnterObjects(ByVal sender As Object, ByVal e As EventArgs)
        Me.iOldRow = Me.grdDetail.CurrentRowIndex
        Dim objArray3 As Object() = New Object(1 - 1) {}
        Dim o As Object = sender
        Dim args As Object() = New Object(0 - 1) {}
        Dim paramnames As String() = Nothing
        objArray3(0) = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(LateBinding.LateGet(o, Nothing, "Name", args, paramnames, Nothing), Nothing, "Trim", New Object(0 - 1) {}, Nothing, Nothing))
        Dim objArray2 As Object() = objArray3
        Dim copyBack As Boolean() = New Boolean() {True}
        If copyBack(0) Then
            LateBinding.LateSetComplex(LateBinding.LateGet(o, Nothing, "Name", args, paramnames, Nothing), Nothing, "Trim", New Object() {RuntimeHelpers.GetObjectValue(objArray2(0))}, Nothing, True, True)
        End If
        Dim obj2 As Object = LateBinding.LateGet(Nothing, GetType(Strings), "UCase", objArray2, Nothing, copyBack)
        If (ObjectType.ObjTst(obj2, "MA_VT", False) = 0) Then
            Me.sOldStringMa_vt = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        ElseIf (ObjectType.ObjTst(obj2, "MA_KHO", False) = 0) Then
            Me.sOldStringMa_kho = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        ElseIf (ObjectType.ObjTst(obj2, "DVT", False) = 0) Then
            Me.sOldStringDvt = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        ElseIf (ObjectType.ObjTst(obj2, "SO_LUONG", False) = 0) Then
            Me.sOldStringSo_luong = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        Else
            Me.sOldString = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        End If
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
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) = 0) Then
            Me.lblSo_ct.Left = Me.lblSo_hdo.Left
            Me.txtSo_ct.Left = Me.txtSo_hd0.Left
        End If
        oVoucher.Init()
        Dim lib6 As New DirLib(Me.txtMa_gd, Me.lblTen_gd, modVoucher.sysConn, modVoucher.appConn, "dmmagd", "ma_gd", "ten_gd", "VCTransCode", ("ma_ct = '" & modVoucher.VoucherCode & "'"), False, Me.cmdEdit)
        AddHandler Me.txtMa_gd.Validated, New EventHandler(AddressOf Me.txtMa_gd_Valid)
        Dim lib7 As New DirLib(Me.txtMa_dvcs, Me.lblTen_dvcs, modVoucher.sysConn, modVoucher.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", False, Me.cmdEdit)
        Dim lib5 As New DirLib(Me.txtMa_tt, Me.lblTen_tt, modVoucher.sysConn, modVoucher.appConn, "dmtt", "ma_tt", "ten_tt", "Term", "1=1", True, Me.cmdEdit)
        Dim lib3 As New DirLib(Me.txtMa_nvbh, Me.lblTen_nvbh, modVoucher.sysConn, modVoucher.appConn, "dmnvbh", "ma_nvbh", "ten_nvbh", "SaleEmployee", "1=1", True, Me.cmdEdit)
        Dim lib4 As New CharLib(Me.txtStatus, "0, 1")
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
        modVoucher.alCharge = "ctcp20tmp"
        Dim cFile As String = ("Structure\Voucher\" & modVoucher.VoucherCode)
        If Not Sys.XML2DataSet((modVoucher.dsMain), cFile) Then
            Dim tcSQL As String = ("SELECT * FROM " & modVoucher.alMaster)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alMaster, (modVoucher.dsMain))
            tcSQL = ("SELECT * FROM " & modVoucher.alDetail)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alDetail, (modVoucher.dsMain))
            tcSQL = ("SELECT * FROM " & modVoucher.alCharge)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alCharge, (modVoucher.dsMain))
            Sys.DataSet2XML(modVoucher.dsMain, cFile)
        End If
        modVoucher.tblMaster.Table = modVoucher.dsMain.Tables.Item(modVoucher.alMaster)
        modVoucher.tblDetail.Table = modVoucher.dsMain.Tables.Item(modVoucher.alDetail)
        modVoucher.tblCharge.Table = modVoucher.dsMain.Tables.Item(modVoucher.alCharge)
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), grdDetail, (modVoucher.tbsDetail), (modVoucher.tbcDetail), "SODetail")
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
        Me.colGia2 = GetColumn(Me.grdDetail, "gia2")
        Me.colGia_nt2 = GetColumn(Me.grdDetail, "gia_nt2")
        Me.colTien2 = GetColumn(Me.grdDetail, "tien2")
        Me.colTien_nt2 = GetColumn(Me.grdDetail, "tien_nt2")
        Me.colCk = GetColumn(Me.grdDetail, "ck")
        Me.colCk_nt = GetColumn(Me.grdDetail, "ck_nt")
        Me.colTen_vt = GetColumn(Me.grdDetail, "ten_vt")
        Me.colSl_xuat = GetColumn(Me.grdDetail, "sl_xuat")
        Me.colSl_hd = GetColumn(Me.grdDetail, "sl_hd")
        Me.colThue_suat = GetColumn(Me.grdDetail, "thue_suat")
        Me.colGia_ban_nt = GetColumn(Me.grdDetail, "gia_ban_nt")
        Me.colGia_ban = GetColumn(Me.grdDetail, "gia_ban")
        Me.IniTax()
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
        Dim lib2 As New DirLib(Me.txtMa_htvc, Me.lblTen_htvc, modVoucher.sysConn, modVoucher.appConn, "dmhtvc", "ma_htvc", "ten_htvc", "Carry", "1=1", True, Me.cmdEdit)
        Me.oSOAddress = New dirblanklib(Me.txtMa_dc, Me.lblTen_dc, modVoucher.sysConn, modVoucher.appConn, "dmdc2", "ma_dc", "ten_dc", "SOAddress", "1=1", True, Me.cmdEdit)
        Dim monumber As New monumber(GetColumn(Me.grdDetail, "so_lsx"))
        Dim _lib As New DirLib(Me.txtMa_kh, Me.lblTen_kh, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", sKey, False, Me.cmdEdit)
        AddHandler Me.txtMa_kh.Validated, New EventHandler(AddressOf Me.txtMa_kh_valid)
        'Dim clscustomerref As New clscustomerref(modVoucher.appConn, Me.txtMa_kh, New TextBox, modVoucher.VoucherCode, Me.oVoucher)
        AddHandler Me.txtMa_dc.Enter, New EventHandler(AddressOf Me.txtMa_dc_Enter)
        Me.oInvItemDetail = New VoucherLibObj(Me.colMa_vt, "ten_vt", modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        VoucherLibObj.oClassMsg = oVoucher.oClassMsg
        Me.oInvItemDetail.Colkey = True
        VoucherLibObj.dvDetail = modVoucher.tblDetail
        Me.oLocation = New VoucherKeyLibObj(Me.colMa_vi_tri, "ten_vi_tri", modVoucher.sysConn, modVoucher.appConn, "dmvitri", "ma_vi_tri", "ten_vi_tri", "Location", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oLot = New VoucherKeyLibObj(Me.colMa_lo, "ten_lo", modVoucher.sysConn, modVoucher.appConn, "dmlo", "ma_lo", "ten_lo", "Lot", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        AddHandler Me.colMa_vi_tri.TextBox.Move, New EventHandler(AddressOf Me.WhenLocationEnter)
        AddHandler Me.colMa_lo.TextBox.Move, New EventHandler(AddressOf Me.WhenLotEnter)
        AddHandler Me.colMa_vt.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKey)
        AddHandler Me.colMa_vt.TextBox.Validated, New EventHandler(AddressOf Me.WhenItemLeave)
        Try
            oVoucher.AddValidFields(Me.grdDetail, modVoucher.tblDetail, Me.pnContent, Me.cmdEdit)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Me.colTen_vt.TextBox.Enabled = False
        Me.colSl_xuat.TextBox.Enabled = False
        Me.colSl_hd.TextBox.Enabled = False
        Me.colThue_suat.TextBox.Enabled = False
        Me.colGia_ban_nt.TextBox.Enabled = False
        Me.colGia_ban.TextBox.Enabled = False
        oVoucher.HideFields(Me.grdDetail)
        'ChangeFormatColumn(Me.colSo_luong, StringType.FromObject(modVoucher.oVar.Item("m_ip_sl")))
        AddHandler Me.colSo_luong.TextBox.Leave, New EventHandler(AddressOf Me.txtSo_luong_valid)
        AddHandler Me.colGia_nt2.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_nt2_valid)
        AddHandler Me.colGia2.TextBox.Leave, New EventHandler(AddressOf Me.txtGia2_valid)
        AddHandler Me.colTien_nt2.TextBox.Leave, New EventHandler(AddressOf Me.txtTien_nt2_valid)
        AddHandler Me.colTien2.TextBox.Leave, New EventHandler(AddressOf Me.txtTien2_valid)
        AddHandler Me.colCk_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtCk_nt_valid)
        AddHandler Me.colCk.TextBox.Leave, New EventHandler(AddressOf Me.txtCk_valid)
        AddHandler Me.colSo_luong.TextBox.Enter, New EventHandler(AddressOf Me.txtSo_luong_enter)
        AddHandler Me.colGia_nt2.TextBox.Enter, New EventHandler(AddressOf Me.txtGia_nt2_enter)
        AddHandler Me.colGia2.TextBox.Enter, New EventHandler(AddressOf Me.txtGia2_enter)
        AddHandler Me.colTien_nt2.TextBox.Enter, New EventHandler(AddressOf Me.txtTien_nt2_enter)
        AddHandler Me.colTien2.TextBox.Enter, New EventHandler(AddressOf Me.txtTien2_enter)
        AddHandler Me.colCk_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtCk_nt_enter)
        AddHandler Me.colCk.TextBox.Enter, New EventHandler(AddressOf Me.txtCk_enter)
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
        Loop While (index < MaxColumns)
        Dim menu As New ContextMenu
        Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItem), Shortcut.F4)
        Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItem), Shortcut.F8)
        menu.MenuItems.Add(item)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item2)
        Dim menu2 As New ContextMenu
        Dim item3 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("057")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F5)
        Dim item4 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("058")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F6)
        menu2.MenuItems.Add(item3)
        menu2.MenuItems.Add(New MenuItem("-"))
        menu2.MenuItems.Add(item4)
        Me.ContextMenu = menu2
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) = 0) Then
            menu2.MenuItems.Item(1).Visible = False
            item3.Enabled = False
            item3.Visible = False
            item4.Enabled = False
            item4.Visible = False
        End If
        Me.txtKeyPress.Left = (-100 - Me.txtKeyPress.Width)
        Me.grdDetail.ContextMenu = menu
        ScatterMemvarBlank(Me)
        oVoucher.cAction = "Start"
        Me.isActive = False
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), 2, False) = 0) Then
            Dim control8 As Control
            Dim controlArray5 As Control() = New Control() {Me.lblSo_hdo, Me.txtSo_hd0, Me.lblNgay_ct3, Me.txtNgay_ct3, Me.lblTien_ck, Me.txtT_ck, Me.txtT_ck_nt, Me.lblNgay_hd1, Me.txtNgay_hd1, Me.lblNgay_hd2, Me.txtNgay_hd2, Me.lblStatus_hd, Me.txtStatus_hd, Me.lblSo_ct0, Me.txtSo_ct0, Me.lblNgay_ct0, Me.txtNgay_ct0}
            Dim controlArray As Control() = New Control() {Me.lblStatus, Me.cboStatus, Me.lblAction, Me.cboAction}
            Dim controlArray2 As Control() = New Control() {Me.txtNgay_lct, Me.txtTy_gia}
            Dim controlArray3 As Control() = New Control() {Me.lblNgay_lct, Me.lblTy_gia}
            Dim controlArray4 As Control() = New Control() {Me.lblT_cp, Me.txtT_cp, Me.txtT_cp_nt}
            Dim control2 As Control
            For Each control2 In controlArray5
                control2.Visible = False
            Next
            Dim control3 As Control
            For Each control3 In controlArray
                control8 = control3
                control8.Top = (control8.Top + (Me.cboStatus.Height + 1))
            Next
            Dim tbDetail As TabControl = Me.tbDetail
            tbDetail.Top = (tbDetail.Top + (Me.cboStatus.Height + 1))
            tbDetail = Me.tbDetail
            tbDetail.Height = (tbDetail.Height - (Me.cboStatus.Height + 1))
            Dim obj5 As Object = (Me.txtTy_gia.Left - Me.cmdMa_nt.Left)
            Dim obj6 As Object = (Me.lblTy_gia.Left - (Me.txtDien_giai.Left + Me.txtDien_giai.Width))
            Dim control4 As Control
            For Each control4 In controlArray2
                control4.Left = Me.txtSo_hd0.Left
            Next
            Dim control5 As Control
            For Each control5 In controlArray3
                control5.Left = Me.lblSo_hdo.Left
            Next
            Me.cmdMa_nt.Left = IntegerType.FromObject(ObjectType.SubObj(Me.txtTy_gia.Left, obj5))
            Me.txtDien_giai.Width = IntegerType.FromObject(ObjectType.SubObj(ObjectType.SubObj(Me.lblTy_gia.Left, obj6), Me.txtDien_giai.Left))
            Dim control6 As Control
            For Each control6 In controlArray4
                control8 = control6
                control8.Top = (control8.Top - (Me.txtT_cp.Height + 1))
            Next
            Dim obj7 As Object = (Me.lblTen_nvbh.Left - Me.txtMa_nvbh.Left)
            Me.lblMa_nvbh.Left = Me.lblNgay_hd1.Left
            Me.txtMa_nvbh.Left = Me.txtNgay_hd1.Left
            Me.lblTen_nvbh.Left = IntegerType.FromObject(ObjectType.AddObj(Me.txtMa_nvbh.Left, obj7))
            Dim activeControl As Control = Me.ActiveControl
            Me.tbDetail.TabPages.Remove(Me.tbgCust)
            Me.tbDetail.TabPages.Remove(Me.tbgOthers)
            Me.tbDetail.SelectedIndex = 0
            If (Not activeControl Is Nothing) Then
                Me.ActiveControl = activeControl
            End If
        End If
        Me.txtNgay_lct.AddCalenderControl()
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
        aGrid = Nothing
        Me.oSecurity.Init()
        Me.oSecurity.Invisible()
        Me.oSecurity.SetReadOnly()
        Me.grdCharge.ReadOnly = True
        Me.InitCharge()
        Me.InitSOPrice()
        Me.colCTen_cp.TextBox.Enabled = False
        xtabControl.ScatterMemvarBlankTabControl(Me.tbDetail)
        xtabControl.ReadOnlyTabControls(True, Me.tbDetail)
        xtabControl.SendTabKeys(Me.tbDetail)
        xtabControl.SetMaxlength(Me.tbDetail, modVoucher.alMaster, modVoucher.sysConn)
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

    Private Sub grdCharge_CurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdCharge.CurrentCellChanged
        If Not Me.lAllowCurrentCellChanged Then
            Return
        End If
        Dim currentRowIndex As Integer = grdCharge.CurrentRowIndex
        Dim columnNumber As Integer = grdCharge.CurrentCell.ColumnNumber
        If IsDBNull(grdCharge.Item(currentRowIndex, columnNumber)) Then
            Return
        End If
        Dim oValue As String = Strings.Trim(StringType.FromObject(grdCharge.Item(currentRowIndex, columnNumber)))
        Dim sLeft As String = grdCharge.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName.ToUpper.ToString
        Dim oOldObject As Object
        If (StringType.StrCmp(sLeft, "TIEN_CP_NT", False) = 0) Then
            oOldObject = Me.noldCTien_cp_nt
            SetOldValue((oOldObject), oValue)
            Me.noldCTien_cp_nt = DecimalType.FromObject(oOldObject)
        ElseIf (StringType.StrCmp(sLeft, "TIEN_CP", False) = 0) Then
            oOldObject = Me.noldCTien_cp
            SetOldValue((oOldObject), oValue)
            Me.noldCTien_cp = DecimalType.FromObject(oOldObject)
        End If
    End Sub

    Private Sub grdDetail_CurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdDetail.CurrentCellChanged
        On Error Resume Next
        If Not Me.lAllowCurrentCellChanged Then
            Return
        End If
        Dim currentRowIndex As Integer = grdDetail.CurrentRowIndex
        Dim columnNumber As Integer = grdDetail.CurrentCell.ColumnNumber
        If IsDBNull(grdCharge.Item(currentRowIndex, columnNumber)) Then
            Return
        End If
        Dim oValue As String = Strings.Trim(StringType.FromObject(grdDetail.Item(currentRowIndex, columnNumber)))
        Dim sLeft As String = grdDetail.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName.ToUpper.ToString
        Dim cOldItem As Object
        If (StringType.StrCmp(sLeft, "MA_VT", False) = 0) Then
            cOldItem = Me.cOldItem
            SetOldValue((cOldItem), oValue)
            Me.cOldItem = StringType.FromObject(cOldItem)
            cOldItem = Me.sOldStringMa_vt
            SetOldValue((cOldItem), oValue)
            Me.sOldStringMa_vt = StringType.FromObject(cOldItem)
        ElseIf (StringType.StrCmp(sLeft, "MA_KHO", False) = 0) Then
            cOldItem = Me.cOldSite
            SetOldValue((cOldItem), oValue)
            Me.cOldSite = StringType.FromObject(cOldItem)
            cOldItem = Me.sOldStringMa_kho
            SetOldValue((cOldItem), oValue)
            Me.sOldStringMa_kho = StringType.FromObject(cOldItem)
        ElseIf (StringType.StrCmp(sLeft, "DVT", False) = 0) Then
            cOldItem = Me.sOldStringDvt
            SetOldValue((cOldItem), oValue)
            Me.sOldStringDvt = StringType.FromObject(cOldItem)
        ElseIf (StringType.StrCmp(sLeft, "SO_LUONG", False) = 0) Then
            cOldItem = Me.noldSo_luong
            SetOldValue((cOldItem), oValue)
            Me.noldSo_luong = DecimalType.FromObject(cOldItem)
            cOldItem = Me.sOldStringSo_luong
            SetOldValue((cOldItem), oValue)
            Me.sOldStringSo_luong = StringType.FromObject(cOldItem)
        ElseIf (StringType.StrCmp(sLeft, "GIA_NT2", False) = 0) Then
            cOldItem = Me.noldGia_nt2
            SetOldValue((cOldItem), oValue)
            Me.noldGia_nt2 = DecimalType.FromObject(cOldItem)
        ElseIf (StringType.StrCmp(sLeft, "GIA2", False) = 0) Then
            cOldItem = Me.noldGia2
            SetOldValue((cOldItem), oValue)
            Me.noldGia2 = DecimalType.FromObject(cOldItem)
        ElseIf (StringType.StrCmp(sLeft, "TIEN_NT2", False) = 0) Then
            cOldItem = Me.noldTien_nt2
            SetOldValue((cOldItem), oValue)
            Me.noldTien_nt2 = DecimalType.FromObject(cOldItem)
        ElseIf (StringType.StrCmp(sLeft, "TIEN2", False) = 0) Then
            cOldItem = Me.noldTien2
            SetOldValue((cOldItem), oValue)
            Me.noldTien2 = DecimalType.FromObject(cOldItem)
        ElseIf (StringType.StrCmp(sLeft, "CK_NT", False) = 0) Then
            cOldItem = Me.noldCk_nt
            SetOldValue((cOldItem), oValue)
            Me.noldCk_nt = DecimalType.FromObject(cOldItem)
        ElseIf (StringType.StrCmp(sLeft, "CK", False) = 0) Then
            cOldItem = Me.noldCk
            SetOldValue((cOldItem), oValue)
            Me.noldCk = DecimalType.FromObject(cOldItem)
        End If
    End Sub

    Private Sub grdLeave(ByVal sender As Object, ByVal e As EventArgs) Handles grdDetail.Leave
        If VoucherLibObj.isLostFocus Then
            VoucherLibObj.isLostFocus = False
        End If
    End Sub

    Private Sub grdMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(num).Item("stt_rec")), "'")
        modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
        Dim i As Integer
        For i = 0 To tblDetail.Count - 1
            Try
                tblDetail.Item(i).Item("ton_order") = CDbl(Sql.GetValue(appConn, "select ton00 From z16socobalance13 where ma_dvcs='" + tblMaster(num).Item("ma_dvcs") + "' AND ma_vt=" + Sql.ConvertVS2SQLType(tblDetail.Item(i).Item("ma_vt"), "")))
            Catch ex As Exception
            End Try
        Next
    End Sub

    Private Sub grdPCRetrieveMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num).Item("stt_rec")), "'")
        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
    End Sub

    Private Sub grdRetrieveMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num).Item("stt_rec")), "'")
        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
    End Sub

    Private Sub IniTax()
        Me.colMa_thue = GetColumn(Me.grdDetail, "Ma_thue")
        Me.colThue = GetColumn(Me.grdDetail, "Thue")
        Me.colThue_nt = GetColumn(Me.grdDetail, "Thue_nt")
        Me.oTaxCodeDetail = New VoucherLibObj(Me.colMa_thue, "ten_thue", modVoucher.sysConn, modVoucher.appConn, "dmthue", "ma_thue", "ten_thue", "Tax", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        AddHandler Me.colMa_thue.TextBox.Validated, New EventHandler(AddressOf Me.txtMa_thue_valid)
        AddHandler Me.colMa_thue.TextBox.Enter, New EventHandler(AddressOf Me.txtMa_thue_enter)
        AddHandler Me.colThue_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtThue_nt_valid)
        AddHandler Me.colThue.TextBox.Leave, New EventHandler(AddressOf Me.txtThue_valid)
        AddHandler Me.colThue_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtThue_nt_enter)
        AddHandler Me.colThue.TextBox.Enter, New EventHandler(AddressOf Me.txtThue_enter)
        AddHandler Me.colThue_nt.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneTax)
        AddHandler Me.colThue.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneTax)
    End Sub

    Private Sub InitCharge()
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblCharge), (grdCharge), (modVoucher.tbsCharge), (modVoucher.tbcCharge), "SOCharge")
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
        Loop While (index < MaxColumns)
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
    Friend WithEvents txtS1 As TextBox
    Friend WithEvents lblS1 As Label
    Friend WithEvents lblSo_ct As Label
    Friend WithEvents txtFnote1 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtFnote2 As TextBox
    Friend WithEvents Label3 As Label

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
        Me.lblS1 = New System.Windows.Forms.Label()
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
        Me.tbgOthers = New System.Windows.Forms.TabPage()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFnote1 = New System.Windows.Forms.TextBox()
        Me.txtMa_htvc = New System.Windows.Forms.TextBox()
        Me.lblMa_htvc = New System.Windows.Forms.Label()
        Me.lblTen_htvc = New System.Windows.Forms.Label()
        Me.lblDia_chi = New System.Windows.Forms.Label()
        Me.lblTen_dc = New System.Windows.Forms.Label()
        Me.txtMa_dc = New System.Windows.Forms.TextBox()
        Me.lblMa_dc = New System.Windows.Forms.Label()
        Me.tbgCharge = New System.Windows.Forms.TabPage()
        Me.grdCharge = New libscontrol.clsgrid()
        Me.tbgCust = New System.Windows.Forms.TabPage()
        Me.txtFax = New System.Windows.Forms.TextBox()
        Me.lblFax_cc = New System.Windows.Forms.Label()
        Me.txtDien_thoai = New System.Windows.Forms.TextBox()
        Me.lblDt_cc = New System.Windows.Forms.Label()
        Me.txtDia_chi = New System.Windows.Forms.TextBox()
        Me.lblDc_cc = New System.Windows.Forms.Label()
        Me.txtTen_kh0 = New System.Windows.Forms.TextBox()
        Me.lblTen_ncc = New System.Windows.Forms.Label()
        Me.tbgOther = New System.Windows.Forms.TabPage()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtFnote2 = New System.Windows.Forms.TextBox()
        Me.lblNgay_ct0 = New System.Windows.Forms.Label()
        Me.txtNgay_ct0 = New libscontrol.txtDate()
        Me.lblSo_ct0 = New System.Windows.Forms.Label()
        Me.txtSo_ct0 = New System.Windows.Forms.TextBox()
        Me.lblMa_nvbh = New System.Windows.Forms.Label()
        Me.lblTen_nvbh = New System.Windows.Forms.Label()
        Me.txtStatus_hd = New System.Windows.Forms.TextBox()
        Me.lblStatus_hd = New System.Windows.Forms.Label()
        Me.lblNgay_hd2 = New System.Windows.Forms.Label()
        Me.txtNgay_hd2 = New libscontrol.txtDate()
        Me.lblNgay_hd1 = New System.Windows.Forms.Label()
        Me.txtNgay_hd1 = New libscontrol.txtDate()
        Me.txtMa_nvbh = New System.Windows.Forms.TextBox()
        Me.txtT_tien2 = New libscontrol.txtNumeric()
        Me.txtT_ck = New libscontrol.txtNumeric()
        Me.txtT_ck_nt = New libscontrol.txtNumeric()
        Me.txtT_tien_nt2 = New libscontrol.txtNumeric()
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
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.lblTien_ck = New System.Windows.Forms.Label()
        Me.lblMa_tt = New System.Windows.Forms.Label()
        Me.txtMa_tt = New System.Windows.Forms.TextBox()
        Me.lblTen_tt = New System.Windows.Forms.Label()
        Me.lblTen = New System.Windows.Forms.Label()
        Me.txtDien_giai = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblT_cp = New System.Windows.Forms.Label()
        Me.txtT_cp_nt = New libscontrol.txtNumeric()
        Me.txtT_cp = New libscontrol.txtNumeric()
        Me.txtT_so_luong = New libscontrol.txtNumeric()
        Me.txtLoai_ct = New System.Windows.Forms.TextBox()
        Me.txtMa_gd = New System.Windows.Forms.TextBox()
        Me.lblMa_gd = New System.Windows.Forms.Label()
        Me.lblTen_gd = New System.Windows.Forms.Label()
        Me.txtStt_rec_hd0 = New System.Windows.Forms.TextBox()
        Me.lblNgay_ct3 = New System.Windows.Forms.Label()
        Me.lblSo_hdo = New System.Windows.Forms.Label()
        Me.txtNgay_ct3 = New libscontrol.txtDate()
        Me.txtSo_hd0 = New System.Windows.Forms.TextBox()
        Me.lblT_thue = New System.Windows.Forms.Label()
        Me.txtT_thue_nt = New libscontrol.txtNumeric()
        Me.txtT_thue = New libscontrol.txtNumeric()
        Me.lblT_tt = New System.Windows.Forms.Label()
        Me.txtT_tt_nt = New libscontrol.txtNumeric()
        Me.txtT_tt = New libscontrol.txtNumeric()
        Me.chkCk_thue_yn = New System.Windows.Forms.CheckBox()
        Me.txtS1 = New System.Windows.Forms.TextBox()
        Me.lblSo_ct = New System.Windows.Forms.Label()
        Me.tbDetail.SuspendLayout()
        Me.tpgDetail.SuspendLayout()
        CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbgOthers.SuspendLayout()
        Me.tbgCharge.SuspendLayout()
        CType(Me.grdCharge, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbgCust.SuspendLayout()
        Me.tbgOther.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSave.Location = New System.Drawing.Point(2, 474)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(72, 26)
        Me.cmdSave.TabIndex = 27
        Me.cmdSave.Tag = "CB01"
        Me.cmdSave.Text = "Luu"
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'cmdNew
        '
        Me.cmdNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Location = New System.Drawing.Point(74, 474)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.Size = New System.Drawing.Size(72, 26)
        Me.cmdNew.TabIndex = 28
        Me.cmdNew.Tag = "CB02"
        Me.cmdNew.Text = "Moi"
        Me.cmdNew.UseVisualStyleBackColor = False
        '
        'cmdPrint
        '
        Me.cmdPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrint.Location = New System.Drawing.Point(146, 474)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.Size = New System.Drawing.Size(72, 26)
        Me.cmdPrint.TabIndex = 29
        Me.cmdPrint.Tag = "CB03"
        Me.cmdPrint.Text = "In ctu"
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Location = New System.Drawing.Point(218, 474)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(72, 26)
        Me.cmdEdit.TabIndex = 30
        Me.cmdEdit.Tag = "CB04"
        Me.cmdEdit.Text = "Sua"
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Location = New System.Drawing.Point(290, 474)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(72, 26)
        Me.cmdDelete.TabIndex = 31
        Me.cmdDelete.Tag = "CB05"
        Me.cmdDelete.Text = "Xoa"
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdView
        '
        Me.cmdView.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Location = New System.Drawing.Point(362, 474)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.Size = New System.Drawing.Size(72, 26)
        Me.cmdView.TabIndex = 32
        Me.cmdView.Tag = "CB06"
        Me.cmdView.Text = "Xem"
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Location = New System.Drawing.Point(434, 474)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(72, 26)
        Me.cmdSearch.TabIndex = 33
        Me.cmdSearch.Tag = "CB07"
        Me.cmdSearch.Text = "Tim"
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Location = New System.Drawing.Point(506, 474)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(72, 26)
        Me.cmdClose.TabIndex = 34
        Me.cmdClose.Tag = "CB08"
        Me.cmdClose.Text = "Quay ra"
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdOption
        '
        Me.cmdOption.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOption.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOption.Location = New System.Drawing.Point(910, 474)
        Me.cmdOption.Name = "cmdOption"
        Me.cmdOption.Size = New System.Drawing.Size(24, 26)
        Me.cmdOption.TabIndex = 35
        Me.cmdOption.TabStop = False
        Me.cmdOption.Tag = "CB09"
        Me.cmdOption.UseVisualStyleBackColor = False
        '
        'cmdTop
        '
        Me.cmdTop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTop.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTop.Location = New System.Drawing.Point(933, 474)
        Me.cmdTop.Name = "cmdTop"
        Me.cmdTop.Size = New System.Drawing.Size(24, 26)
        Me.cmdTop.TabIndex = 36
        Me.cmdTop.TabStop = False
        Me.cmdTop.Tag = "CB10"
        Me.cmdTop.UseVisualStyleBackColor = False
        '
        'cmdPrev
        '
        Me.cmdPrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdPrev.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrev.Location = New System.Drawing.Point(955, 474)
        Me.cmdPrev.Name = "cmdPrev"
        Me.cmdPrev.Size = New System.Drawing.Size(24, 26)
        Me.cmdPrev.TabIndex = 37
        Me.cmdPrev.TabStop = False
        Me.cmdPrev.Tag = "CB11"
        Me.cmdPrev.UseVisualStyleBackColor = False
        '
        'cmdNext
        '
        Me.cmdNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdNext.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNext.Location = New System.Drawing.Point(978, 474)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.Size = New System.Drawing.Size(24, 26)
        Me.cmdNext.TabIndex = 38
        Me.cmdNext.TabStop = False
        Me.cmdNext.Tag = "CB12"
        Me.cmdNext.UseVisualStyleBackColor = False
        '
        'cmdBottom
        '
        Me.cmdBottom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBottom.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBottom.Location = New System.Drawing.Point(1001, 474)
        Me.cmdBottom.Name = "cmdBottom"
        Me.cmdBottom.Size = New System.Drawing.Size(24, 26)
        Me.cmdBottom.TabIndex = 39
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
        'lblS1
        '
        Me.lblS1.AutoSize = True
        Me.lblS1.Location = New System.Drawing.Point(2, 102)
        Me.lblS1.Name = "lblS1"
        Me.lblS1.Size = New System.Drawing.Size(89, 17)
        Me.lblS1.TabIndex = 16
        Me.lblS1.Tag = "LZ01"
        Me.lblS1.Text = "So don hang"
        '
        'txtSo_ct
        '
        Me.txtSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSo_ct.BackColor = System.Drawing.Color.White
        Me.txtSo_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_ct.Location = New System.Drawing.Point(928, 6)
        Me.txtSo_ct.Name = "txtSo_ct"
        Me.txtSo_ct.Size = New System.Drawing.Size(96, 22)
        Me.txtSo_ct.TabIndex = 6
        Me.txtSo_ct.Tag = "FCNBCF"
        Me.txtSo_ct.Text = "TXTSO_CT"
        Me.txtSo_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNgay_lct
        '
        Me.txtNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNgay_lct.BackColor = System.Drawing.Color.White
        Me.txtNgay_lct.Location = New System.Drawing.Point(928, 30)
        Me.txtNgay_lct.MaxLength = 10
        Me.txtNgay_lct.Name = "txtNgay_lct"
        Me.txtNgay_lct.Size = New System.Drawing.Size(96, 22)
        Me.txtNgay_lct.TabIndex = 7
        Me.txtNgay_lct.Tag = "FDNBDF"
        Me.txtNgay_lct.Text = "  /  /    "
        Me.txtNgay_lct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_lct.Value = New Date(CType(0, Long))
        '
        'txtTy_gia
        '
        Me.txtTy_gia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTy_gia.BackColor = System.Drawing.Color.White
        Me.txtTy_gia.Format = "m_ip_tg"
        Me.txtTy_gia.Location = New System.Drawing.Point(928, 78)
        Me.txtTy_gia.MaxLength = 8
        Me.txtTy_gia.Name = "txtTy_gia"
        Me.txtTy_gia.Size = New System.Drawing.Size(96, 22)
        Me.txtTy_gia.TabIndex = 10
        Me.txtTy_gia.Tag = "FNCF"
        Me.txtTy_gia.Text = "m_ip_tg"
        Me.txtTy_gia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTy_gia.Value = 0R
        '
        'lblNgay_lct
        '
        Me.lblNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNgay_lct.AutoSize = True
        Me.lblNgay_lct.Location = New System.Drawing.Point(827, 32)
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
        Me.lblNgay_ct.Location = New System.Drawing.Point(306, 526)
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
        Me.lblTy_gia.Location = New System.Drawing.Point(827, 82)
        Me.lblTy_gia.Name = "lblTy_gia"
        Me.lblTy_gia.Size = New System.Drawing.Size(47, 17)
        Me.lblTy_gia.TabIndex = 22
        Me.lblTy_gia.Tag = "L012"
        Me.lblTy_gia.Text = "Ty gia"
        '
        'txtNgay_ct
        '
        Me.txtNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNgay_ct.BackColor = System.Drawing.Color.White
        Me.txtNgay_ct.Location = New System.Drawing.Point(421, 524)
        Me.txtNgay_ct.MaxLength = 10
        Me.txtNgay_ct.Name = "txtNgay_ct"
        Me.txtNgay_ct.Size = New System.Drawing.Size(120, 22)
        Me.txtNgay_ct.TabIndex = 10
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
        Me.cmdMa_nt.Location = New System.Drawing.Point(885, 78)
        Me.cmdMa_nt.Name = "cmdMa_nt"
        Me.cmdMa_nt.Size = New System.Drawing.Size(43, 24)
        Me.cmdMa_nt.TabIndex = 9
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
        Me.tbDetail.Controls.Add(Me.tbgOthers)
        Me.tbDetail.Controls.Add(Me.tbgCharge)
        Me.tbDetail.Controls.Add(Me.tbgCust)
        Me.tbDetail.Controls.Add(Me.tbgOther)
        Me.tbDetail.Location = New System.Drawing.Point(2, 166)
        Me.tbDetail.Name = "tbDetail"
        Me.tbDetail.SelectedIndex = 0
        Me.tbDetail.Size = New System.Drawing.Size(1024, 220)
        Me.tbDetail.TabIndex = 14
        Me.tbDetail.Tag = ""
        '
        'tpgDetail
        '
        Me.tpgDetail.BackColor = System.Drawing.SystemColors.Control
        Me.tpgDetail.Controls.Add(Me.grdDetail)
        Me.tpgDetail.Location = New System.Drawing.Point(4, 25)
        Me.tpgDetail.Name = "tpgDetail"
        Me.tpgDetail.Size = New System.Drawing.Size(1016, 191)
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
        Me.grdDetail.Size = New System.Drawing.Size(1018, 188)
        Me.grdDetail.TabIndex = 0
        Me.grdDetail.Tag = "L020CF"
        '
        'tbgOthers
        '
        Me.tbgOthers.Controls.Add(Me.Label2)
        Me.tbgOthers.Controls.Add(Me.txtFnote1)
        Me.tbgOthers.Controls.Add(Me.txtMa_htvc)
        Me.tbgOthers.Controls.Add(Me.lblMa_htvc)
        Me.tbgOthers.Controls.Add(Me.lblTen_htvc)
        Me.tbgOthers.Controls.Add(Me.lblDia_chi)
        Me.tbgOthers.Controls.Add(Me.lblTen_dc)
        Me.tbgOthers.Controls.Add(Me.txtMa_dc)
        Me.tbgOthers.Controls.Add(Me.lblMa_dc)
        Me.tbgOthers.Location = New System.Drawing.Point(4, 25)
        Me.tbgOthers.Name = "tbgOthers"
        Me.tbgOthers.Size = New System.Drawing.Size(1016, 191)
        Me.tbgOthers.TabIndex = 3
        Me.tbgOthers.Tag = "L004"
        Me.tbgOthers.Text = "Thong tin giao hang"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(2, 57)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 17)
        Me.Label2.TabIndex = 118
        Me.Label2.Tag = "LZ02"
        Me.Label2.Text = "Thoi gian"
        '
        'txtFnote1
        '
        Me.txtFnote1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFnote1.BackColor = System.Drawing.Color.White
        Me.txtFnote1.Location = New System.Drawing.Point(106, 54)
        Me.txtFnote1.Name = "txtFnote1"
        Me.txtFnote1.Size = New System.Drawing.Size(894, 22)
        Me.txtFnote1.TabIndex = 117
        Me.txtFnote1.Tag = "FCCF"
        Me.txtFnote1.Text = "txtFnote1"
        '
        'txtMa_htvc
        '
        Me.txtMa_htvc.BackColor = System.Drawing.Color.White
        Me.txtMa_htvc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_htvc.Location = New System.Drawing.Point(106, 30)
        Me.txtMa_htvc.Name = "txtMa_htvc"
        Me.txtMa_htvc.Size = New System.Drawing.Size(96, 22)
        Me.txtMa_htvc.TabIndex = 1
        Me.txtMa_htvc.Tag = "FCCF"
        Me.txtMa_htvc.Text = "TXTMA_HTVC"
        '
        'lblMa_htvc
        '
        Me.lblMa_htvc.AutoSize = True
        Me.lblMa_htvc.Location = New System.Drawing.Point(2, 32)
        Me.lblMa_htvc.Name = "lblMa_htvc"
        Me.lblMa_htvc.Size = New System.Drawing.Size(86, 17)
        Me.lblMa_htvc.TabIndex = 115
        Me.lblMa_htvc.Tag = "L006"
        Me.lblMa_htvc.Text = "Hinh thuc vc"
        '
        'lblTen_htvc
        '
        Me.lblTen_htvc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_htvc.AutoSize = True
        Me.lblTen_htvc.Location = New System.Drawing.Point(210, 32)
        Me.lblTen_htvc.Name = "lblTen_htvc"
        Me.lblTen_htvc.Size = New System.Drawing.Size(172, 17)
        Me.lblTen_htvc.TabIndex = 116
        Me.lblTen_htvc.Tag = "FCRF"
        Me.lblTen_htvc.Text = "Ten hinh thuc van chuyen"
        '
        'lblDia_chi
        '
        Me.lblDia_chi.AutoSize = True
        Me.lblDia_chi.Location = New System.Drawing.Point(210, 8)
        Me.lblDia_chi.Name = "lblDia_chi"
        Me.lblDia_chi.Size = New System.Drawing.Size(51, 17)
        Me.lblDia_chi.TabIndex = 113
        Me.lblDia_chi.Tag = "L015"
        Me.lblDia_chi.Text = "Dia chi"
        '
        'lblTen_dc
        '
        Me.lblTen_dc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_dc.AutoSize = True
        Me.lblTen_dc.Location = New System.Drawing.Point(288, 9)
        Me.lblTen_dc.Name = "lblTen_dc"
        Me.lblTen_dc.Size = New System.Drawing.Size(92, 17)
        Me.lblTen_dc.TabIndex = 114
        Me.lblTen_dc.Tag = "FCRF"
        Me.lblTen_dc.Text = "Ten noi nhan"
        '
        'txtMa_dc
        '
        Me.txtMa_dc.BackColor = System.Drawing.Color.White
        Me.txtMa_dc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dc.Location = New System.Drawing.Point(106, 6)
        Me.txtMa_dc.Name = "txtMa_dc"
        Me.txtMa_dc.Size = New System.Drawing.Size(96, 22)
        Me.txtMa_dc.TabIndex = 0
        Me.txtMa_dc.Tag = "FCCF"
        Me.txtMa_dc.Text = "TXTMA_DC"
        '
        'lblMa_dc
        '
        Me.lblMa_dc.AutoSize = True
        Me.lblMa_dc.Location = New System.Drawing.Point(2, 8)
        Me.lblMa_dc.Name = "lblMa_dc"
        Me.lblMa_dc.Size = New System.Drawing.Size(65, 17)
        Me.lblMa_dc.TabIndex = 112
        Me.lblMa_dc.Tag = "L005"
        Me.lblMa_dc.Text = "Noi nhan"
        '
        'tbgCharge
        '
        Me.tbgCharge.Controls.Add(Me.grdCharge)
        Me.tbgCharge.Location = New System.Drawing.Point(4, 25)
        Me.tbgCharge.Name = "tbgCharge"
        Me.tbgCharge.Size = New System.Drawing.Size(1016, 191)
        Me.tbgCharge.TabIndex = 2
        Me.tbgCharge.Tag = "L034"
        Me.tbgCharge.Text = "Chi phi"
        '
        'grdCharge
        '
        Me.grdCharge.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdCharge.BackgroundColor = System.Drawing.Color.White
        Me.grdCharge.CaptionBackColor = System.Drawing.SystemColors.Control
        Me.grdCharge.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdCharge.CaptionForeColor = System.Drawing.Color.Black
        Me.grdCharge.CaptionText = "Nhap chi phi: F4 - Them dong, F8 - Xoa dong"
        Me.grdCharge.Cell_EnableRaisingEvents = False
        Me.grdCharge.DataMember = ""
        Me.grdCharge.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdCharge.Location = New System.Drawing.Point(-1, -1)
        Me.grdCharge.Name = "grdCharge"
        Me.grdCharge.Size = New System.Drawing.Size(1018, 188)
        Me.grdCharge.TabIndex = 1
        Me.grdCharge.Tag = "L035"
        '
        'tbgCust
        '
        Me.tbgCust.Controls.Add(Me.txtFax)
        Me.tbgCust.Controls.Add(Me.lblFax_cc)
        Me.tbgCust.Controls.Add(Me.txtDien_thoai)
        Me.tbgCust.Controls.Add(Me.lblDt_cc)
        Me.tbgCust.Controls.Add(Me.txtDia_chi)
        Me.tbgCust.Controls.Add(Me.lblDc_cc)
        Me.tbgCust.Controls.Add(Me.txtTen_kh0)
        Me.tbgCust.Controls.Add(Me.lblTen_ncc)
        Me.tbgCust.Location = New System.Drawing.Point(4, 25)
        Me.tbgCust.Name = "tbgCust"
        Me.tbgCust.Size = New System.Drawing.Size(1016, 191)
        Me.tbgCust.TabIndex = 4
        Me.tbgCust.Tag = "L043"
        Me.tbgCust.Text = "Thong tin khach hang"
        '
        'txtFax
        '
        Me.txtFax.BackColor = System.Drawing.Color.White
        Me.txtFax.Enabled = False
        Me.txtFax.Location = New System.Drawing.Point(568, 30)
        Me.txtFax.Name = "txtFax"
        Me.txtFax.Size = New System.Drawing.Size(180, 22)
        Me.txtFax.TabIndex = 3
        Me.txtFax.Tag = "FCCF"
        Me.txtFax.Text = "txtFax"
        '
        'lblFax_cc
        '
        Me.lblFax_cc.AutoSize = True
        Me.lblFax_cc.Location = New System.Drawing.Point(470, 32)
        Me.lblFax_cc.Name = "lblFax_cc"
        Me.lblFax_cc.Size = New System.Drawing.Size(30, 17)
        Me.lblFax_cc.TabIndex = 124
        Me.lblFax_cc.Tag = "L047"
        Me.lblFax_cc.Text = "Fax"
        '
        'txtDien_thoai
        '
        Me.txtDien_thoai.BackColor = System.Drawing.Color.White
        Me.txtDien_thoai.Enabled = False
        Me.txtDien_thoai.Location = New System.Drawing.Point(568, 6)
        Me.txtDien_thoai.Name = "txtDien_thoai"
        Me.txtDien_thoai.Size = New System.Drawing.Size(180, 22)
        Me.txtDien_thoai.TabIndex = 2
        Me.txtDien_thoai.Tag = "FCCF"
        Me.txtDien_thoai.Text = "txtDien_thoai"
        '
        'lblDt_cc
        '
        Me.lblDt_cc.AutoSize = True
        Me.lblDt_cc.Location = New System.Drawing.Point(470, 8)
        Me.lblDt_cc.Name = "lblDt_cc"
        Me.lblDt_cc.Size = New System.Drawing.Size(91, 17)
        Me.lblDt_cc.TabIndex = 123
        Me.lblDt_cc.Tag = "L046"
        Me.lblDt_cc.Text = "So dien thoai"
        '
        'txtDia_chi
        '
        Me.txtDia_chi.BackColor = System.Drawing.Color.White
        Me.txtDia_chi.Enabled = False
        Me.txtDia_chi.Location = New System.Drawing.Point(106, 30)
        Me.txtDia_chi.Name = "txtDia_chi"
        Me.txtDia_chi.Size = New System.Drawing.Size(360, 22)
        Me.txtDia_chi.TabIndex = 1
        Me.txtDia_chi.Tag = "FCCF"
        Me.txtDia_chi.Text = "txtDia_chi"
        '
        'lblDc_cc
        '
        Me.lblDc_cc.AutoSize = True
        Me.lblDc_cc.Location = New System.Drawing.Point(2, 32)
        Me.lblDc_cc.Name = "lblDc_cc"
        Me.lblDc_cc.Size = New System.Drawing.Size(51, 17)
        Me.lblDc_cc.TabIndex = 122
        Me.lblDc_cc.Tag = "L045"
        Me.lblDc_cc.Text = "Dia chi"
        '
        'txtTen_kh0
        '
        Me.txtTen_kh0.BackColor = System.Drawing.Color.White
        Me.txtTen_kh0.Enabled = False
        Me.txtTen_kh0.Location = New System.Drawing.Point(106, 6)
        Me.txtTen_kh0.Name = "txtTen_kh0"
        Me.txtTen_kh0.Size = New System.Drawing.Size(360, 22)
        Me.txtTen_kh0.TabIndex = 0
        Me.txtTen_kh0.Tag = "FCCF"
        Me.txtTen_kh0.Text = "txtTen_kh0"
        '
        'lblTen_ncc
        '
        Me.lblTen_ncc.AutoSize = True
        Me.lblTen_ncc.Location = New System.Drawing.Point(2, 8)
        Me.lblTen_ncc.Name = "lblTen_ncc"
        Me.lblTen_ncc.Size = New System.Drawing.Size(111, 17)
        Me.lblTen_ncc.TabIndex = 121
        Me.lblTen_ncc.Tag = "L044"
        Me.lblTen_ncc.Text = "Ten khach hang"
        '
        'tbgOther
        '
        Me.tbgOther.Controls.Add(Me.Label3)
        Me.tbgOther.Controls.Add(Me.txtFnote2)
        Me.tbgOther.Controls.Add(Me.lblNgay_ct0)
        Me.tbgOther.Controls.Add(Me.txtNgay_ct0)
        Me.tbgOther.Controls.Add(Me.lblSo_ct0)
        Me.tbgOther.Controls.Add(Me.txtSo_ct0)
        Me.tbgOther.Controls.Add(Me.lblMa_nvbh)
        Me.tbgOther.Controls.Add(Me.lblTen_nvbh)
        Me.tbgOther.Controls.Add(Me.txtStatus_hd)
        Me.tbgOther.Controls.Add(Me.lblStatus_hd)
        Me.tbgOther.Controls.Add(Me.lblNgay_hd2)
        Me.tbgOther.Controls.Add(Me.txtNgay_hd2)
        Me.tbgOther.Controls.Add(Me.lblNgay_hd1)
        Me.tbgOther.Controls.Add(Me.txtNgay_hd1)
        Me.tbgOther.Controls.Add(Me.txtMa_nvbh)
        Me.tbgOther.Location = New System.Drawing.Point(4, 25)
        Me.tbgOther.Name = "tbgOther"
        Me.tbgOther.Size = New System.Drawing.Size(1016, 191)
        Me.tbgOther.TabIndex = 5
        Me.tbgOther.Tag = "L048"
        Me.tbgOther.Text = "Thong tin khac"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(2, 76)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(152, 17)
        Me.Label3.TabIndex = 121
        Me.Label3.Tag = "LZ03"
        Me.Label3.Text = "Dieu khoan thanh toan"
        '
        'txtFnote2
        '
        Me.txtFnote2.Location = New System.Drawing.Point(163, 76)
        Me.txtFnote2.Name = "txtFnote2"
        Me.txtFnote2.Size = New System.Drawing.Size(730, 22)
        Me.txtFnote2.TabIndex = 120
        Me.txtFnote2.Tag = "FCCF"
        Me.txtFnote2.Text = "txtFnote2"
        '
        'lblNgay_ct0
        '
        Me.lblNgay_ct0.AutoSize = True
        Me.lblNgay_ct0.Location = New System.Drawing.Point(278, 57)
        Me.lblNgay_ct0.Name = "lblNgay_ct0"
        Me.lblNgay_ct0.Size = New System.Drawing.Size(97, 17)
        Me.lblNgay_ct0.TabIndex = 119
        Me.lblNgay_ct0.Tag = "L007"
        Me.lblNgay_ct0.Text = "Ngay hoa don"
        '
        'txtNgay_ct0
        '
        Me.txtNgay_ct0.BackColor = System.Drawing.Color.White
        Me.txtNgay_ct0.Location = New System.Drawing.Point(374, 54)
        Me.txtNgay_ct0.MaxLength = 10
        Me.txtNgay_ct0.Name = "txtNgay_ct0"
        Me.txtNgay_ct0.Size = New System.Drawing.Size(96, 22)
        Me.txtNgay_ct0.TabIndex = 5
        Me.txtNgay_ct0.Tag = "FDCF"
        Me.txtNgay_ct0.Text = "  /  /    "
        Me.txtNgay_ct0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_ct0.Value = New Date(CType(0, Long))
        '
        'lblSo_ct0
        '
        Me.lblSo_ct0.AutoSize = True
        Me.lblSo_ct0.Location = New System.Drawing.Point(278, 32)
        Me.lblSo_ct0.Name = "lblSo_ct0"
        Me.lblSo_ct0.Size = New System.Drawing.Size(81, 17)
        Me.lblSo_ct0.TabIndex = 117
        Me.lblSo_ct0.Tag = "L053"
        Me.lblSo_ct0.Text = "So hoa don"
        '
        'txtSo_ct0
        '
        Me.txtSo_ct0.BackColor = System.Drawing.Color.White
        Me.txtSo_ct0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_ct0.Location = New System.Drawing.Point(374, 30)
        Me.txtSo_ct0.Name = "txtSo_ct0"
        Me.txtSo_ct0.Size = New System.Drawing.Size(96, 22)
        Me.txtSo_ct0.TabIndex = 4
        Me.txtSo_ct0.Tag = "FCCF"
        Me.txtSo_ct0.Text = "TXTSO_CT0"
        Me.txtSo_ct0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMa_nvbh
        '
        Me.lblMa_nvbh.AutoSize = True
        Me.lblMa_nvbh.Location = New System.Drawing.Point(278, 8)
        Me.lblMa_nvbh.Name = "lblMa_nvbh"
        Me.lblMa_nvbh.Size = New System.Drawing.Size(46, 17)
        Me.lblMa_nvbh.TabIndex = 114
        Me.lblMa_nvbh.Tag = "L052"
        Me.lblMa_nvbh.Text = "Ma nv"
        '
        'lblTen_nvbh
        '
        Me.lblTen_nvbh.AutoSize = True
        Me.lblTen_nvbh.Location = New System.Drawing.Point(480, 8)
        Me.lblTen_nvbh.Name = "lblTen_nvbh"
        Me.lblTen_nvbh.Size = New System.Drawing.Size(99, 17)
        Me.lblTen_nvbh.TabIndex = 115
        Me.lblTen_nvbh.Tag = "FCRF"
        Me.lblTen_nvbh.Text = "Ten nhan vien"
        '
        'txtStatus_hd
        '
        Me.txtStatus_hd.BackColor = System.Drawing.Color.White
        Me.txtStatus_hd.Enabled = False
        Me.txtStatus_hd.Location = New System.Drawing.Point(163, 54)
        Me.txtStatus_hd.Name = "txtStatus_hd"
        Me.txtStatus_hd.Size = New System.Drawing.Size(96, 22)
        Me.txtStatus_hd.TabIndex = 2
        Me.txtStatus_hd.Tag = "FCCF"
        Me.txtStatus_hd.Text = "txtStatus_hd"
        '
        'lblStatus_hd
        '
        Me.lblStatus_hd.AutoSize = True
        Me.lblStatus_hd.Location = New System.Drawing.Point(2, 57)
        Me.lblStatus_hd.Name = "lblStatus_hd"
        Me.lblStatus_hd.Size = New System.Drawing.Size(73, 17)
        Me.lblStatus_hd.TabIndex = 113
        Me.lblStatus_hd.Tag = "L051"
        Me.lblStatus_hd.Text = "Trang thai"
        '
        'lblNgay_hd2
        '
        Me.lblNgay_hd2.AutoSize = True
        Me.lblNgay_hd2.Location = New System.Drawing.Point(2, 32)
        Me.lblNgay_hd2.Name = "lblNgay_hd2"
        Me.lblNgay_hd2.Size = New System.Drawing.Size(94, 17)
        Me.lblNgay_hd2.TabIndex = 112
        Me.lblNgay_hd2.Tag = "L050"
        Me.lblNgay_hd2.Text = "Ngay hieu luc"
        '
        'txtNgay_hd2
        '
        Me.txtNgay_hd2.BackColor = System.Drawing.Color.White
        Me.txtNgay_hd2.Enabled = False
        Me.txtNgay_hd2.Location = New System.Drawing.Point(163, 30)
        Me.txtNgay_hd2.MaxLength = 10
        Me.txtNgay_hd2.Name = "txtNgay_hd2"
        Me.txtNgay_hd2.Size = New System.Drawing.Size(96, 22)
        Me.txtNgay_hd2.TabIndex = 1
        Me.txtNgay_hd2.Tag = "FDCF"
        Me.txtNgay_hd2.Text = "  /  /    "
        Me.txtNgay_hd2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_hd2.Value = New Date(CType(0, Long))
        '
        'lblNgay_hd1
        '
        Me.lblNgay_hd1.AutoSize = True
        Me.lblNgay_hd1.Location = New System.Drawing.Point(2, 8)
        Me.lblNgay_hd1.Name = "lblNgay_hd1"
        Me.lblNgay_hd1.Size = New System.Drawing.Size(84, 17)
        Me.lblNgay_hd1.TabIndex = 111
        Me.lblNgay_hd1.Tag = "L049"
        Me.lblNgay_hd1.Text = "Ngay lap hd"
        '
        'txtNgay_hd1
        '
        Me.txtNgay_hd1.BackColor = System.Drawing.Color.White
        Me.txtNgay_hd1.Enabled = False
        Me.txtNgay_hd1.Location = New System.Drawing.Point(163, 6)
        Me.txtNgay_hd1.MaxLength = 10
        Me.txtNgay_hd1.Name = "txtNgay_hd1"
        Me.txtNgay_hd1.Size = New System.Drawing.Size(96, 22)
        Me.txtNgay_hd1.TabIndex = 0
        Me.txtNgay_hd1.Tag = "FDCF"
        Me.txtNgay_hd1.Text = "  /  /    "
        Me.txtNgay_hd1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_hd1.Value = New Date(CType(0, Long))
        '
        'txtMa_nvbh
        '
        Me.txtMa_nvbh.BackColor = System.Drawing.Color.White
        Me.txtMa_nvbh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_nvbh.Location = New System.Drawing.Point(374, 6)
        Me.txtMa_nvbh.Name = "txtMa_nvbh"
        Me.txtMa_nvbh.Size = New System.Drawing.Size(96, 22)
        Me.txtMa_nvbh.TabIndex = 3
        Me.txtMa_nvbh.Tag = "FCCF"
        Me.txtMa_nvbh.Text = "TXTMA_NVBH"
        '
        'txtT_tien2
        '
        Me.txtT_tien2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien2.BackColor = System.Drawing.Color.White
        Me.txtT_tien2.Enabled = False
        Me.txtT_tien2.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien2.Format = "m_ip_tien"
        Me.txtT_tien2.Location = New System.Drawing.Point(904, 394)
        Me.txtT_tien2.MaxLength = 10
        Me.txtT_tien2.Name = "txtT_tien2"
        Me.txtT_tien2.Size = New System.Drawing.Size(120, 22)
        Me.txtT_tien2.TabIndex = 18
        Me.txtT_tien2.Tag = "FN"
        Me.txtT_tien2.Text = "m_ip_tien"
        Me.txtT_tien2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien2.Value = 0R
        '
        'txtT_ck
        '
        Me.txtT_ck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_ck.BackColor = System.Drawing.Color.White
        Me.txtT_ck.Enabled = False
        Me.txtT_ck.ForeColor = System.Drawing.Color.Black
        Me.txtT_ck.Format = "m_ip_tien"
        Me.txtT_ck.Location = New System.Drawing.Point(227, 418)
        Me.txtT_ck.MaxLength = 10
        Me.txtT_ck.Name = "txtT_ck"
        Me.txtT_ck.Size = New System.Drawing.Size(120, 22)
        Me.txtT_ck.TabIndex = 20
        Me.txtT_ck.Tag = "FN"
        Me.txtT_ck.Text = "m_ip_tien"
        Me.txtT_ck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_ck.Value = 0R
        '
        'txtT_ck_nt
        '
        Me.txtT_ck_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_ck_nt.BackColor = System.Drawing.Color.White
        Me.txtT_ck_nt.Enabled = False
        Me.txtT_ck_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_ck_nt.Format = "m_ip_tien_nt"
        Me.txtT_ck_nt.Location = New System.Drawing.Point(106, 418)
        Me.txtT_ck_nt.MaxLength = 13
        Me.txtT_ck_nt.Name = "txtT_ck_nt"
        Me.txtT_ck_nt.Size = New System.Drawing.Size(120, 22)
        Me.txtT_ck_nt.TabIndex = 19
        Me.txtT_ck_nt.Tag = "FN"
        Me.txtT_ck_nt.Text = "m_ip_tien_nt"
        Me.txtT_ck_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_ck_nt.Value = 0R
        '
        'txtT_tien_nt2
        '
        Me.txtT_tien_nt2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien_nt2.BackColor = System.Drawing.Color.White
        Me.txtT_tien_nt2.Enabled = False
        Me.txtT_tien_nt2.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien_nt2.Format = "m_ip_tien_nt"
        Me.txtT_tien_nt2.Location = New System.Drawing.Point(783, 394)
        Me.txtT_tien_nt2.MaxLength = 13
        Me.txtT_tien_nt2.Name = "txtT_tien_nt2"
        Me.txtT_tien_nt2.Size = New System.Drawing.Size(120, 22)
        Me.txtT_tien_nt2.TabIndex = 17
        Me.txtT_tien_nt2.Tag = "FN"
        Me.txtT_tien_nt2.Text = "m_ip_tien_nt"
        Me.txtT_tien_nt2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien_nt2.Value = 0R
        '
        'txtStatus
        '
        Me.txtStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtStatus.BackColor = System.Drawing.Color.White
        Me.txtStatus.Location = New System.Drawing.Point(10, 504)
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
        Me.lblStatus.Location = New System.Drawing.Point(827, 105)
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
        Me.lblStatusMess.Location = New System.Drawing.Point(58, 506)
        Me.lblStatusMess.Name = "lblStatusMess"
        Me.lblStatusMess.Size = New System.Drawing.Size(253, 17)
        Me.lblStatusMess.TabIndex = 42
        Me.lblStatusMess.Tag = ""
        Me.lblStatusMess.Text = "1 - Ghi vao SC, 0 - Chua ghi vao so cai"
        Me.lblStatusMess.Visible = False
        '
        'txtKeyPress
        '
        Me.txtKeyPress.Location = New System.Drawing.Point(353, 81)
        Me.txtKeyPress.Name = "txtKeyPress"
        Me.txtKeyPress.Size = New System.Drawing.Size(12, 22)
        Me.txtKeyPress.TabIndex = 13
        '
        'cboStatus
        '
        Me.cboStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStatus.BackColor = System.Drawing.Color.White
        Me.cboStatus.Enabled = False
        Me.cboStatus.Location = New System.Drawing.Point(904, 103)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New System.Drawing.Size(120, 24)
        Me.cboStatus.TabIndex = 11
        Me.cboStatus.TabStop = False
        Me.cboStatus.Tag = ""
        Me.cboStatus.Text = "cboStatus"
        '
        'cboAction
        '
        Me.cboAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAction.BackColor = System.Drawing.Color.White
        Me.cboAction.Location = New System.Drawing.Point(904, 128)
        Me.cboAction.Name = "cboAction"
        Me.cboAction.Size = New System.Drawing.Size(120, 24)
        Me.cboAction.TabIndex = 12
        Me.cboAction.TabStop = False
        Me.cboAction.Tag = "CF"
        Me.cboAction.Text = "cboAction"
        '
        'lblAction
        '
        Me.lblAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAction.AutoSize = True
        Me.lblAction.Location = New System.Drawing.Point(827, 130)
        Me.lblAction.Name = "lblAction"
        Me.lblAction.Size = New System.Drawing.Size(39, 17)
        Me.lblAction.TabIndex = 33
        Me.lblAction.Tag = ""
        Me.lblAction.Text = "Xu ly"
        '
        'lblMa_kh
        '
        Me.lblMa_kh.AutoSize = True
        Me.lblMa_kh.Location = New System.Drawing.Point(2, 32)
        Me.lblMa_kh.Name = "lblMa_kh"
        Me.lblMa_kh.Size = New System.Drawing.Size(69, 17)
        Me.lblMa_kh.TabIndex = 34
        Me.lblMa_kh.Tag = "L002"
        Me.lblMa_kh.Text = "Ma khach"
        '
        'txtMa_kh
        '
        Me.txtMa_kh.BackColor = System.Drawing.Color.White
        Me.txtMa_kh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kh.Location = New System.Drawing.Point(106, 30)
        Me.txtMa_kh.Name = "txtMa_kh"
        Me.txtMa_kh.Size = New System.Drawing.Size(96, 22)
        Me.txtMa_kh.TabIndex = 1
        Me.txtMa_kh.Tag = "FCNBCF"
        Me.txtMa_kh.Text = "TXTMA_KH"
        '
        'lblTen_kh
        '
        Me.lblTen_kh.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_kh.Location = New System.Drawing.Point(202, 32)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(615, 18)
        Me.lblTen_kh.TabIndex = 36
        Me.lblTen_kh.Tag = "FCRF"
        Me.lblTen_kh.Text = "Ten Khach"
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(559, 397)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(76, 17)
        Me.lblTotal.TabIndex = 60
        Me.lblTotal.Tag = "L013"
        Me.lblTotal.Text = "Tong cong"
        '
        'lblTien_ck
        '
        Me.lblTien_ck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTien_ck.AutoSize = True
        Me.lblTien_ck.Location = New System.Drawing.Point(2, 421)
        Me.lblTien_ck.Name = "lblTien_ck"
        Me.lblTien_ck.Size = New System.Drawing.Size(75, 17)
        Me.lblTien_ck.TabIndex = 61
        Me.lblTien_ck.Tag = "L014"
        Me.lblTien_ck.Text = "Chiet khau"
        '
        'lblMa_tt
        '
        Me.lblMa_tt.AutoSize = True
        Me.lblMa_tt.Location = New System.Drawing.Point(2, 81)
        Me.lblMa_tt.Name = "lblMa_tt"
        Me.lblMa_tt.Size = New System.Drawing.Size(39, 17)
        Me.lblMa_tt.TabIndex = 65
        Me.lblMa_tt.Tag = "L008"
        Me.lblMa_tt.Text = "Ma tt"
        '
        'txtMa_tt
        '
        Me.txtMa_tt.BackColor = System.Drawing.Color.White
        Me.txtMa_tt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_tt.Location = New System.Drawing.Point(106, 78)
        Me.txtMa_tt.Name = "txtMa_tt"
        Me.txtMa_tt.Size = New System.Drawing.Size(36, 22)
        Me.txtMa_tt.TabIndex = 3
        Me.txtMa_tt.Tag = "FCCF"
        Me.txtMa_tt.Text = "TXTMA_TT"
        '
        'lblTen_tt
        '
        Me.lblTen_tt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_tt.Location = New System.Drawing.Point(149, 81)
        Me.lblTen_tt.Name = "lblTen_tt"
        Me.lblTen_tt.Size = New System.Drawing.Size(668, 17)
        Me.lblTen_tt.TabIndex = 66
        Me.lblTen_tt.Tag = "FCRF"
        Me.lblTen_tt.Text = "Ten thanh toan"
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
        Me.txtDien_giai.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDien_giai.BackColor = System.Drawing.Color.White
        Me.txtDien_giai.Location = New System.Drawing.Point(106, 54)
        Me.txtDien_giai.Name = "txtDien_giai"
        Me.txtDien_giai.Size = New System.Drawing.Size(711, 22)
        Me.txtDien_giai.TabIndex = 2
        Me.txtDien_giai.Tag = "FCCF"
        Me.txtDien_giai.Text = "txtDien_giai"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(2, 57)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 17)
        Me.Label1.TabIndex = 75
        Me.Label1.Tag = "L029"
        Me.Label1.Text = "Dien giai"
        '
        'lblT_cp
        '
        Me.lblT_cp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblT_cp.AutoSize = True
        Me.lblT_cp.Location = New System.Drawing.Point(2, 445)
        Me.lblT_cp.Name = "lblT_cp"
        Me.lblT_cp.Size = New System.Drawing.Size(51, 17)
        Me.lblT_cp.TabIndex = 81
        Me.lblT_cp.Tag = "L030"
        Me.lblT_cp.Text = "Chi phi"
        '
        'txtT_cp_nt
        '
        Me.txtT_cp_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_cp_nt.BackColor = System.Drawing.Color.White
        Me.txtT_cp_nt.Enabled = False
        Me.txtT_cp_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp_nt.Format = "m_ip_tien_nt"
        Me.txtT_cp_nt.Location = New System.Drawing.Point(106, 443)
        Me.txtT_cp_nt.MaxLength = 13
        Me.txtT_cp_nt.Name = "txtT_cp_nt"
        Me.txtT_cp_nt.Size = New System.Drawing.Size(120, 22)
        Me.txtT_cp_nt.TabIndex = 23
        Me.txtT_cp_nt.Tag = "FN"
        Me.txtT_cp_nt.Text = "m_ip_tien_nt"
        Me.txtT_cp_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp_nt.Value = 0R
        '
        'txtT_cp
        '
        Me.txtT_cp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_cp.BackColor = System.Drawing.Color.White
        Me.txtT_cp.Enabled = False
        Me.txtT_cp.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp.Format = "m_ip_tien"
        Me.txtT_cp.Location = New System.Drawing.Point(227, 443)
        Me.txtT_cp.MaxLength = 10
        Me.txtT_cp.Name = "txtT_cp"
        Me.txtT_cp.Size = New System.Drawing.Size(120, 22)
        Me.txtT_cp.TabIndex = 24
        Me.txtT_cp.Tag = "FN"
        Me.txtT_cp.Text = "m_ip_tien"
        Me.txtT_cp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp.Value = 0R
        '
        'txtT_so_luong
        '
        Me.txtT_so_luong.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_so_luong.BackColor = System.Drawing.Color.White
        Me.txtT_so_luong.Enabled = False
        Me.txtT_so_luong.ForeColor = System.Drawing.Color.Black
        Me.txtT_so_luong.Format = "m_ip_sl"
        Me.txtT_so_luong.Location = New System.Drawing.Point(661, 394)
        Me.txtT_so_luong.MaxLength = 8
        Me.txtT_so_luong.Name = "txtT_so_luong"
        Me.txtT_so_luong.Size = New System.Drawing.Size(120, 22)
        Me.txtT_so_luong.TabIndex = 16
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
        Me.lblTen_gd.Location = New System.Drawing.Point(149, 8)
        Me.lblTen_gd.Name = "lblTen_gd"
        Me.lblTen_gd.Size = New System.Drawing.Size(668, 17)
        Me.lblTen_gd.TabIndex = 88
        Me.lblTen_gd.Tag = "FCRF"
        Me.lblTen_gd.Text = "Ten giao dich"
        '
        'txtStt_rec_hd0
        '
        Me.txtStt_rec_hd0.BackColor = System.Drawing.Color.White
        Me.txtStt_rec_hd0.Location = New System.Drawing.Point(384, 526)
        Me.txtStt_rec_hd0.Name = "txtStt_rec_hd0"
        Me.txtStt_rec_hd0.Size = New System.Drawing.Size(120, 22)
        Me.txtStt_rec_hd0.TabIndex = 100
        Me.txtStt_rec_hd0.Tag = "FCCF"
        Me.txtStt_rec_hd0.Text = "TXTSTT_REC_HD0"
        Me.txtStt_rec_hd0.Visible = False
        '
        'lblNgay_ct3
        '
        Me.lblNgay_ct3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNgay_ct3.AutoSize = True
        Me.lblNgay_ct3.Location = New System.Drawing.Point(827, 57)
        Me.lblNgay_ct3.Name = "lblNgay_ct3"
        Me.lblNgay_ct3.Size = New System.Drawing.Size(56, 17)
        Me.lblNgay_ct3.TabIndex = 104
        Me.lblNgay_ct3.Tag = "L042"
        Me.lblNgay_ct3.Text = "Ngay hl"
        '
        'lblSo_hdo
        '
        Me.lblSo_hdo.AutoSize = True
        Me.lblSo_hdo.Location = New System.Drawing.Point(2, 130)
        Me.lblSo_hdo.Name = "lblSo_hdo"
        Me.lblSo_hdo.Size = New System.Drawing.Size(89, 17)
        Me.lblSo_hdo.TabIndex = 103
        Me.lblSo_hdo.Tag = "L041"
        Me.lblSo_hdo.Text = "So hop dong"
        '
        'txtNgay_ct3
        '
        Me.txtNgay_ct3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNgay_ct3.BackColor = System.Drawing.Color.White
        Me.txtNgay_ct3.Location = New System.Drawing.Point(928, 54)
        Me.txtNgay_ct3.MaxLength = 10
        Me.txtNgay_ct3.Name = "txtNgay_ct3"
        Me.txtNgay_ct3.Size = New System.Drawing.Size(96, 22)
        Me.txtNgay_ct3.TabIndex = 8
        Me.txtNgay_ct3.Tag = "FDCF"
        Me.txtNgay_ct3.Text = "  /  /    "
        Me.txtNgay_ct3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_ct3.Value = New Date(CType(0, Long))
        '
        'txtSo_hd0
        '
        Me.txtSo_hd0.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSo_hd0.BackColor = System.Drawing.Color.White
        Me.txtSo_hd0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_hd0.Location = New System.Drawing.Point(106, 128)
        Me.txtSo_hd0.Name = "txtSo_hd0"
        Me.txtSo_hd0.Size = New System.Drawing.Size(711, 22)
        Me.txtSo_hd0.TabIndex = 5
        Me.txtSo_hd0.Tag = "FCCF"
        Me.txtSo_hd0.Text = "TXTSO_HD0"
        '
        'lblT_thue
        '
        Me.lblT_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblT_thue.AutoSize = True
        Me.lblT_thue.Location = New System.Drawing.Point(661, 421)
        Me.lblT_thue.Name = "lblT_thue"
        Me.lblT_thue.Size = New System.Drawing.Size(68, 17)
        Me.lblT_thue.TabIndex = 110
        Me.lblT_thue.Tag = "L055"
        Me.lblT_thue.Text = "Tien thue"
        '
        'txtT_thue_nt
        '
        Me.txtT_thue_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_thue_nt.BackColor = System.Drawing.Color.White
        Me.txtT_thue_nt.Enabled = False
        Me.txtT_thue_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue_nt.Format = "m_ip_tien_nt"
        Me.txtT_thue_nt.Location = New System.Drawing.Point(783, 418)
        Me.txtT_thue_nt.MaxLength = 13
        Me.txtT_thue_nt.Name = "txtT_thue_nt"
        Me.txtT_thue_nt.Size = New System.Drawing.Size(120, 22)
        Me.txtT_thue_nt.TabIndex = 21
        Me.txtT_thue_nt.Tag = "FN"
        Me.txtT_thue_nt.Text = "m_ip_tien_nt"
        Me.txtT_thue_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue_nt.Value = 0R
        '
        'txtT_thue
        '
        Me.txtT_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_thue.BackColor = System.Drawing.Color.White
        Me.txtT_thue.Enabled = False
        Me.txtT_thue.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue.Format = "m_ip_tien"
        Me.txtT_thue.Location = New System.Drawing.Point(904, 418)
        Me.txtT_thue.MaxLength = 10
        Me.txtT_thue.Name = "txtT_thue"
        Me.txtT_thue.Size = New System.Drawing.Size(120, 22)
        Me.txtT_thue.TabIndex = 22
        Me.txtT_thue.Tag = "FN"
        Me.txtT_thue.Text = "m_ip_tien"
        Me.txtT_thue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue.Value = 0R
        '
        'lblT_tt
        '
        Me.lblT_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblT_tt.AutoSize = True
        Me.lblT_tt.Location = New System.Drawing.Point(661, 445)
        Me.lblT_tt.Name = "lblT_tt"
        Me.lblT_tt.Size = New System.Drawing.Size(113, 17)
        Me.lblT_tt.TabIndex = 109
        Me.lblT_tt.Tag = "L056"
        Me.lblT_tt.Text = "Tong thanh toan"
        '
        'txtT_tt_nt
        '
        Me.txtT_tt_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tt_nt.BackColor = System.Drawing.Color.White
        Me.txtT_tt_nt.Enabled = False
        Me.txtT_tt_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tt_nt.Format = "m_ip_tien_nt"
        Me.txtT_tt_nt.Location = New System.Drawing.Point(783, 443)
        Me.txtT_tt_nt.MaxLength = 13
        Me.txtT_tt_nt.Name = "txtT_tt_nt"
        Me.txtT_tt_nt.Size = New System.Drawing.Size(120, 22)
        Me.txtT_tt_nt.TabIndex = 25
        Me.txtT_tt_nt.Tag = "FN"
        Me.txtT_tt_nt.Text = "m_ip_tien_nt"
        Me.txtT_tt_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tt_nt.Value = 0R
        '
        'txtT_tt
        '
        Me.txtT_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tt.BackColor = System.Drawing.Color.White
        Me.txtT_tt.Enabled = False
        Me.txtT_tt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tt.Format = "m_ip_tien"
        Me.txtT_tt.Location = New System.Drawing.Point(904, 443)
        Me.txtT_tt.MaxLength = 10
        Me.txtT_tt.Name = "txtT_tt"
        Me.txtT_tt.Size = New System.Drawing.Size(120, 22)
        Me.txtT_tt.TabIndex = 26
        Me.txtT_tt.Tag = "FN"
        Me.txtT_tt.Text = "m_ip_tien"
        Me.txtT_tt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tt.Value = 0R
        '
        'chkCk_thue_yn
        '
        Me.chkCk_thue_yn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkCk_thue_yn.Location = New System.Drawing.Point(10, 397)
        Me.chkCk_thue_yn.Name = "chkCk_thue_yn"
        Me.chkCk_thue_yn.Size = New System.Drawing.Size(201, 18)
        Me.chkCk_thue_yn.TabIndex = 15
        Me.chkCk_thue_yn.TabStop = False
        Me.chkCk_thue_yn.Tag = "L054FLCF"
        Me.chkCk_thue_yn.Text = "Chiet khau sau thue"
        Me.chkCk_thue_yn.Visible = False
        '
        'txtS1
        '
        Me.txtS1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtS1.BackColor = System.Drawing.Color.White
        Me.txtS1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtS1.Location = New System.Drawing.Point(106, 103)
        Me.txtS1.Name = "txtS1"
        Me.txtS1.Size = New System.Drawing.Size(711, 22)
        Me.txtS1.TabIndex = 4
        Me.txtS1.Tag = "FCCF"
        Me.txtS1.Text = "TXTS1"
        '
        'lblSo_ct
        '
        Me.lblSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSo_ct.AutoSize = True
        Me.lblSo_ct.Location = New System.Drawing.Point(827, 8)
        Me.lblSo_ct.Name = "lblSo_ct"
        Me.lblSo_ct.Size = New System.Drawing.Size(64, 17)
        Me.lblSo_ct.TabIndex = 112
        Me.lblSo_ct.Tag = "L009"
        Me.lblSo_ct.Text = "So phieu"
        '
        'frmVoucher
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(1029, 526)
        Me.Controls.Add(Me.lblSo_ct)
        Me.Controls.Add(Me.txtS1)
        Me.Controls.Add(Me.lblT_thue)
        Me.Controls.Add(Me.txtT_thue_nt)
        Me.Controls.Add(Me.txtT_thue)
        Me.Controls.Add(Me.lblT_tt)
        Me.Controls.Add(Me.txtT_tt_nt)
        Me.Controls.Add(Me.txtT_tt)
        Me.Controls.Add(Me.lblNgay_ct3)
        Me.Controls.Add(Me.txtNgay_ct3)
        Me.Controls.Add(Me.txtMa_gd)
        Me.Controls.Add(Me.lblMa_gd)
        Me.Controls.Add(Me.txtLoai_ct)
        Me.Controls.Add(Me.txtT_so_luong)
        Me.Controls.Add(Me.lblT_cp)
        Me.Controls.Add(Me.txtT_cp_nt)
        Me.Controls.Add(Me.txtT_cp)
        Me.Controls.Add(Me.txtDien_giai)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblTen)
        Me.Controls.Add(Me.lblMa_tt)
        Me.Controls.Add(Me.txtMa_tt)
        Me.Controls.Add(Me.lblTien_ck)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.txtMa_kh)
        Me.Controls.Add(Me.lblMa_kh)
        Me.Controls.Add(Me.lblAction)
        Me.Controls.Add(Me.txtKeyPress)
        Me.Controls.Add(Me.lblStatusMess)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.txtT_tien_nt2)
        Me.Controls.Add(Me.txtT_ck_nt)
        Me.Controls.Add(Me.txtT_ck)
        Me.Controls.Add(Me.txtT_tien2)
        Me.Controls.Add(Me.lblTy_gia)
        Me.Controls.Add(Me.lblNgay_ct)
        Me.Controls.Add(Me.lblNgay_lct)
        Me.Controls.Add(Me.txtTy_gia)
        Me.Controls.Add(Me.lblS1)
        Me.Controls.Add(Me.lblMa_dvcs)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.txtNgay_ct)
        Me.Controls.Add(Me.txtNgay_lct)
        Me.Controls.Add(Me.txtSo_ct)
        Me.Controls.Add(Me.txtMa_dvcs)
        Me.Controls.Add(Me.lblTen_dvcs)
        Me.Controls.Add(Me.txtStt_rec_hd0)
        Me.Controls.Add(Me.txtSo_hd0)
        Me.Controls.Add(Me.lblSo_hdo)
        Me.Controls.Add(Me.chkCk_thue_yn)
        Me.Controls.Add(Me.lblTen_gd)
        Me.Controls.Add(Me.lblTen_tt)
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
        Me.tbgOthers.ResumeLayout(False)
        Me.tbgOthers.PerformLayout()
        Me.tbgCharge.ResumeLayout(False)
        CType(Me.grdCharge, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbgCust.ResumeLayout(False)
        Me.tbgCust.PerformLayout()
        Me.tbgOther.ResumeLayout(False)
        Me.tbgOther.PerformLayout()
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
            str = "EXEC fs_LoadSOTran '" + cLan + "', '" + cIDVoucher + "', '" + Trim(oVoucherRow.Item("m_sl_ct0")) + "', '" + Trim(oVoucherRow.Item("m_phdbf"))
            str += "', '" + Trim(oVoucherRow.Item("m_ctdbf")) + "', '" + VoucherCode + "', -1"
        Else
            str = "EXEC fs_LoadSOTran '" + cLan + "', '" + cIDVoucher + "', '" + Trim(oVoucherRow.Item("m_sl_ct0")) + "', '" + Trim(oVoucherRow.Item("m_phdbf"))
            str += "', '" + Trim(oVoucherRow.Item("m_ctdbf")) + "', '" + VoucherCode + "', " + Trim(Reg.GetRegistryKey("CurrUserID"))
        End If
        str = (str & GetLoadParameters())
        Dim ds As New DataSet
        Sql.SQLDecompressRetrieve((modVoucher.appConn), str, "trantmp", (ds))
        AppendFrom(modVoucher.tblMaster, ds.Tables.Item(0))
        AppendFrom(modVoucher.tblDetail, ds.Tables.Item(1))
        If (modVoucher.tblMaster.Count > 0) Then
            Me.iMasterRow = 0
            Dim _strfilter As String = "stt_rec = '" + tblMaster.Item(Me.iMasterRow).Item("stt_rec") + "'"
            modVoucher.tblDetail.RowFilter = _strfilter
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

    Private Sub InitSOPrice()
        Dim str As String
        Dim num As Integer
        Dim str3 As String = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "sysspmasterinfo", "xread", ("xid = '" & modVoucher.VoucherCode & "'")))
        If (StringType.StrCmp(Strings.Trim(str3), "", False) <> 0) Then
            Dim num5 As Integer = IntegerType.FromObject(Fox.GetWordCount(str3, ","c))
            Dim num4 As Integer
            Dim i As Integer
            Dim str2 As String
            Dim flag As Boolean
            num = 1
            Do While (num <= num5)
                str = Strings.Trim(Fox.GetWordNum(str3, num, ","c))
                num4 = (Me.Controls.Count - 1)
                i = 0
                Do While (i <= num4)
                    str2 = Strings.Trim(Me.Controls.Item(i).Name)
                    flag = False
                    Try
                        Dim obj2 As Object = DirectCast(Me.Controls.Item(i), Label)
                    Catch exception1 As Exception
                        ProjectData.SetProjectError(exception1)
                        Dim exception As Exception = exception1
                        flag = True
                        ProjectData.ClearProjectError()
                    End Try
                    If ((StringType.StrCmp(Strings.Trim(str2), "", False) <> 0) AndAlso ((StringType.StrCmp(Strings.Right(str2, (Strings.Len(str2) - 3)).ToUpper, str.ToUpper, False) = 0) And flag)) Then
                        Dim box As TextBox = DirectCast(Me.Controls.Item(i), TextBox)
                        AddHandler box.Enter, New EventHandler(AddressOf Me.ReadOnlyObjects)
                    End If
                    i += 1
                Loop
                num += 1
            Loop
        End If
        Dim ds As New DataSet
        Dim tcSQL As String = ("SELECT * FROM sysspdetailinfo WHERE xid = '" & modVoucher.VoucherCode & "' ORDER BY xorder")
        Sql.SQLRetrieve((modVoucher.appConn), tcSQL, "sysspdetailinfo", (ds))
        Dim num3 As Integer = (ds.Tables.Item(0).Rows.Count - 1)
        num = 0
        Do While (num <= num3)
            str = Strings.Trim(StringType.FromObject(ds.Tables.Item(0).Rows.Item(num).Item("xvalid")))
            GetColumn(Me.grdDetail, str).TextBox.Name = GetColumn(Me.grdDetail, str).MappingName
            AddHandler GetColumn(Me.grdDetail, str).TextBox.Validated, New EventHandler(AddressOf Me.ValidObjects)
            AddHandler GetColumn(Me.grdDetail, str).TextBox.Enter, New EventHandler(AddressOf Me.EnterObjects)
            num += 1
        Loop
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

    Private Sub NewItem(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If (currentRowIndex < 0) Then
                modVoucher.tblDetail.AddNew()
                Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
            ElseIf ((Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) AndAlso Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt")))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt"))), "", False) <> 0)) Then
                Me.grdDetail.BeforeAddNewItem()
                Me.grdDetail.CurrentCell = New DataGridCell(modVoucher.tblDetail.Count, 0)
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
                Me.grdCharge.BeforeAddNewItem()
                Me.grdCharge.CurrentCell = New DataGridCell(modVoucher.tblCharge.Count, 0)
                Me.grdCharge.AfterAddNewItem()
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
                    oVoucher.ViewDeletedRecord("fs_SearchDeletedSOTran", "SOMaster", "SODetail", "t_tt", "t_tt_nt")
                    Exit Select
            End Select
        End If
    End Sub

    Private Function Post() As String
        Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "groupby", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Dim str3 As String = "EXEC fs_PostSO "
        Return (StringType.FromObject(ObjectType.AddObj(((((((str3 & "'" & modVoucher.VoucherCode & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))) & "'") & ", '" & Strings.Trim(str) & "'"), ObjectType.AddObj(ObjectType.AddObj(", '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))) & ", 1")
    End Function

    Public Sub Print()
        Dim print As New frmPrint
        print.txtTitle.Text = StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct"))), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct2")))))
        print.txtSo_lien.Value = DoubleType.FromObject(modVoucher.oVoucherRow.Item("so_lien"))
        rpTable = clsprint.InitComboReport(modVoucher.sysConn, print.cboReports, "SOTran")
        If Not Information.IsNothing(rpTable) Then
            print.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(rpTable.Rows.Item(print.cboReports.SelectedIndex), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        End If
        Dim result As DialogResult = print.ShowDialog
        If ((result <> DialogResult.Cancel) AndAlso (print.txtSo_lien.Value > 0)) Then
            Dim selectedIndex As Integer = print.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(rpTable.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
            Dim view As New DataView
            Dim ds As New DataSet
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj((("EXEC fs_PrintSOTran '" & modVoucher.cLan) & "', " & "[a.stt_rec = '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'], '"), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf")))), "'"))
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
            clsprint.SetReportVar(modVoucher.sysConn, modVoucher.appConn, "SOTran", modVoucher.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(print.txtTitle.Text))
            Dim str2 As String = Strings.Replace(Strings.Replace(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("401")), "%s1", Me.txtNgay_ct.Value.Day.ToString, 1, -1, CompareMethod.Binary), "%s2", Me.txtNgay_ct.Value.Month.ToString, 1, -1, CompareMethod.Binary), "%s3", Me.txtNgay_ct.Value.Year.ToString, 1, -1, CompareMethod.Binary)
            Dim str4 As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("402")), "%s", Strings.Trim(Me.txtSo_ct.Text), 1, -1, CompareMethod.Binary)
            Dim str As String = Strings.Replace(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("403")), "%s1", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary), "%s", clsprint.Num2Words(New Decimal(Me.txtT_tt_nt.Value)), 1, -1, CompareMethod.Binary)
            clsprint.oRpt.SetParameterValue("s_byword", str)
            clsprint.oRpt.SetParameterValue("t_date", str2)
            clsprint.oRpt.SetParameterValue("t_number", str4)
            clsprint.oRpt.SetParameterValue("nAmount", Me.txtT_tien_nt2.Value)
            clsprint.oRpt.SetParameterValue("f_kh", (Strings.Trim(Me.txtMa_kh.Text) & " - " & Strings.Trim(Me.lblTen_kh.Text)))
            Dim str3 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "dia_chi", ("ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'"))))
            clsprint.oRpt.SetParameterValue("f_dia_chi", str3)
            str3 = Strings.Trim(Me.lblTen_dc.Text)
            clsprint.oRpt.SetParameterValue("f_ten_dc", str3)
            If (result = DialogResult.OK) Then
                clsprint.PrintReport(CInt(Math.Round(print.txtSo_lien.Value)))
                clsprint.oRpt.SetDataSource(view.Table)
            Else
                clsprint.ShowReports()
            End If
            clsprint.oRpt.Close()
            ds = Nothing
            'table = Nothing
            print.Dispose()
        End If
    End Sub

    Private Sub ReadOnlyObjects(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Integer = 0
        Dim num6 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim num As Integer = 0
        For num = 0 To num6
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ma_vt")), "C") Then
                num2 = 1
            End If
        Next
        'If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
        '    LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(num2 > 0)}, Nothing)
        'End If
    End Sub

    Private Sub RecalcTax(ByVal iRow As Integer, ByVal nType As Integer)
        Dim num As Byte
        Dim zero As Decimal
        Dim decimals As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num = decimals
        Else
            num = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(iRow).Item("thue_suat"))) Then
            zero = DecimalType.FromObject(modVoucher.tblDetail.Item(iRow).Item("thue_suat"))
        Else
            zero = Decimal.Zero
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(iRow).Item("Tien_nt2"))) Then
            modVoucher.tblDetail.Item(iRow).Item("Tien_nt2") = 0
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(iRow).Item("Tien2"))) Then
            modVoucher.tblDetail.Item(iRow).Item("Tien2") = 0
        End If
        Dim num4 As Decimal = DecimalType.FromObject(modVoucher.tblDetail.Item(iRow).Item("Tien2"))
        Dim num2 As Decimal = DecimalType.FromObject(modVoucher.tblDetail.Item(iRow).Item("Tien_nt2"))
        If Not Me.chkCk_thue_yn.Checked Then
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(iRow).Item("ck_nt"))) Then
                num2 = DecimalType.FromObject(ObjectType.SubObj(num2, modVoucher.tblDetail.Item(iRow).Item("ck_nt")))
            End If
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(iRow).Item("ck"))) Then
                num4 = DecimalType.FromObject(ObjectType.SubObj(num4, modVoucher.tblDetail.Item(iRow).Item("ck")))
            End If
        End If
        If (nType > 1) Then
            modVoucher.tblDetail.Item(iRow).Item("thue_nt") = RuntimeHelpers.GetObjectValue(Fox.Round(Decimal.Divide(Decimal.Multiply(num2, zero), 100), num))
        End If
        modVoucher.tblDetail.Item(iRow).Item("thue") = RuntimeHelpers.GetObjectValue(Fox.Round(Decimal.Divide(Decimal.Multiply(num4, zero), 100), decimals))
    End Sub

    Public Sub RefrehForm()
        Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
        Me.grdHeader.Scatter()
        ScatterMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'")
        modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
        Me.RefreshCharge(1)
        Me.EDTranType()
        Me.UpdateList()
        Me.vCaptionRefresh()
        xtabControl.ScatterTabControl(modVoucher.tblMaster.Item(Me.iMasterRow), Me.tbDetail)
        Me.cmdNew.Focus()
    End Sub

    Private Sub RefreshCharge(ByVal nType As Byte)
        modVoucher.tblCharge.Table.Clear()
        If (nType <> 0) Then
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(("fs_LoadSOCharge '" & modVoucher.cLan & "', '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
            Sql.SQLRetrieve((modVoucher.appConn), tcSQL, modVoucher.alCharge, (modVoucher.tblCharge.Table.DataSet))
        End If
    End Sub

    Private Sub RestoreCharge()
        Dim cString As String = "cp_vc, cp_vc_nt, cp_bh, cp_bh_nt, cp_khac, cp_khac_nt"
        Dim num4 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim i As Integer = 0
        Dim num3 As Integer
        Dim j As Integer
        Dim str2 As String
        Dim str As String
        Do While (i <= num4)
            num3 = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            j = 1
            Do While (j <= num3)
                str2 = Strings.Trim(Fox.GetWordNum(cString, j, ","c))
                str = (str2 & "2")
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
                Me.RetrieveItemsFromSC()
                Exit Select
            Case 2
                Me.RetrieveItemsFromSQ()
                Exit Select
        End Select
        Me.oInvItemDetail.Cancel = cancel
    End Sub

    Private Sub RetrieveItemsFromSC()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim tcSQL As String = "EXEC fs_SearchSCTran4SO '" + cLan + "', '" + Strings.Trim(Me.txtMa_kh.Text) + "'"
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
                frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("059"))
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
                Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "SCMaster4SO")
                index = 0
                Do
                    If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                        cols(index).NullText = StringType.FromInteger(0)
                    Else
                        cols(index).NullText = ""
                    End If
                    index += 1
                Loop While (index < MaxColumns)
                Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "SCDetail4SO")
                index = 0
                Do
                    If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                        cols(index).NullText = StringType.FromInteger(0)
                    Else
                        cols(index).NullText = ""
                    End If
                    index += 1
                Loop While (index < MaxColumns)
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
                Dim num10 As Integer = (count - 1)
                index = 0
                Do While (index <= num10)
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien2"))) Then
                        zero = DecimalType.FromObject(ObjectType.AddObj(zero, Me.tblRetrieveMaster.Item(index).Item("t_tien2")))
                    End If
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2"))) Then
                        num4 = DecimalType.FromObject(ObjectType.AddObj(num4, Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2")))
                    End If
                    index += 1
                Loop
                expression = Strings.Replace(Strings.Replace(Strings.Replace(expression, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, CompareMethod.Binary), "%n2", Strings.Trim(Strings.Format(num4, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))), 1, -1, CompareMethod.Binary), "%n3", Strings.Trim(Strings.Format(zero, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))), 1, -1, CompareMethod.Binary)
                panel.Text = expression
                AddHandler gridformtran2.CurrentCellChanged, New EventHandler(AddressOf Me.grdPCRetrieveMVCurrentCellChanged)
                gridformtran2.CurrentRowIndex = 0
                Dim rowNumber As Integer = 0
                Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(rowNumber).Item("stt_rec")), "'")
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
                Dim tblRetrieveDetail As DataView = Me.tblRetrieveDetail
                tblRetrieveDetail.RowFilter = (tblRetrieveDetail.RowFilter & " AND sl_dh0 <> 0")
                Dim num9 As Integer = (Me.tblRetrieveDetail.Count - 1)
                index = 0
                Do While (index <= num9)
                    tblRetrieveDetail.Item(index).Item("so_luong") = RuntimeHelpers.GetObjectValue(tblRetrieveDetail.Item(index).Item("sl_dh0"))
                    tblRetrieveDetail.Item(index).Row.AcceptChanges()
                    index += 1
                Loop
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
                Dim num8 As Integer = (tbl.Rows.Count - 1)
                index = 0
                Do While (index <= num8)
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        tbl.Rows.Item(index).Item("stt_rec") = ""
                    Else
                        tbl.Rows.Item(index).Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                    End If
                    tbl.Rows.Item(index).Item("sl_xuat") = 0
                    tbl.Rows.Item(index).Item("sl_giao") = 0
                    tbl.Rows.Item(index).Item("sl_hd") = 0
                    tbl.Rows.Item(index).Item("sl_tl") = 0
                    tbl.Rows.Item(index).Item("sl_tl0") = 0
                    tbl.Rows.Item(index).Item("sl_dh") = 0
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
                        ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_hd")), "C") Then
                            modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                        End If
                        index = (index + -1)
                    Loop
                    Dim num6 As Integer = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))
                    If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                        num6 = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
                    End If
                    Dim num7 As Integer = (modVoucher.tblDetail.Count - 1)
                    index = 0
                    Do While (index <= num7)
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt2"))) Then
                            tblDetail.Item(index).Item("tien_nt2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("so_luong"), tblDetail.Item(index).Item("gia_nt2")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt2"))) Then
                            tblDetail.Item(index).Item("gia2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("gia_nt2"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                        End If
                        Dim args As Object() = New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("so_luong"), tblDetail.Item(index).Item("gia_nt2")), num6}
                        Dim copyBack As Boolean() = New Boolean() {False, True}
                        If copyBack(1) Then
                            num6 = IntegerType.FromObject(args(1))
                        End If
                        tblDetail.Item(index).Item("tien_nt2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                        tblDetail.Item(index).Item("tien2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("tien_nt2"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                        Me.RecalcTax(index, 2)
                        index += 1
                    Loop
                    Try
                        rowNumber = gridformtran2.CurrentCell.RowNumber
                        Dim view As DataRowView = Me.tblRetrieveMaster.Item(rowNumber)
                        Me.txtStt_rec_hd0.Text = StringType.FromObject(view.Item("stt_rec"))
                        Me.txtSo_hd0.Text = StringType.FromObject(view.Item("so_ct"))
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("ngay_ct"))) Then
                            Me.txtNgay_hd1.Value = DateType.FromObject(view.Item("ngay_ct"))
                        Else
                            Me.txtNgay_hd1.Text = StringType.FromObject(Fox.GetEmptyDate)
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("ngay_ct3"))) Then
                            Me.txtNgay_hd2.Value = DateType.FromObject(view.Item("ngay_ct3"))
                        Else
                            Me.txtNgay_hd2.Text = StringType.FromObject(Fox.GetEmptyDate)
                        End If
                        Me.txtStatus_hd.Text = StringType.FromObject(view.Item("status"))
                        view = Nothing
                        If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                            Me.txtMa_kh.Text = StringType.FromObject(Me.tblRetrieveMaster.Item(rowNumber).Item("ma_kh"))
                            Me.txtMa_kh_valid(Me.txtMa_kh, New EventArgs)
                            Me.txtMa_kh.Focus()
                        End If
                    Catch exception3 As Exception
                        ProjectData.SetProjectError(exception3)
                        Dim exception2 As Exception = exception3
                        ProjectData.ClearProjectError()
                    End Try
                    Me.UpdateList()
                End If
                frmAdd.Dispose()
            End If
            ds = Nothing
            Me.tblRetrieveMaster = Nothing
            Me.tblRetrieveDetail = Nothing
        End If
    End Sub

    Private Sub RetrieveItemsFromSQ()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("064")), 2)
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
                    str3 = (str3 & " AND a.ma_kh LIKE '" & Strings.Trim(Me.txtMa_kh.Text) & "%'")
                    Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchSQTran4SO '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph61', 'ct61'"})
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
                        frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("063"))
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
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "SQMaster")
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
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "SQDetail4SO")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index < MaxColumns)
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
                        Dim num10 As Integer = (count - 1)
                        index = 0
                        Do While (index <= num10)
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien2"))) Then
                                zero = DecimalType.FromObject(ObjectType.AddObj(zero, Me.tblRetrieveMaster.Item(index).Item("t_tien2")))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2"))) Then
                                num4 = DecimalType.FromObject(ObjectType.AddObj(num4, Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2")))
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
                        button4.Text = StringType.FromObject(modVoucher.oLan.Item("060"))
                        button4.Width = 100
                        button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Top = button4.Top
                        button2.Left = (button4.Left + 110)
                        button2.Visible = True
                        button2.Text = StringType.FromObject(modVoucher.oLan.Item("061"))
                        button2.Width = 120
                        button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
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
                        Dim num9 As Integer = (Me.tblRetrieveDetail.Count - 1)
                        index = 0
                        Do While (index <= num9)
                            tblRetrieveDetail.Item(index).Item("stt_rec_bg") = RuntimeHelpers.GetObjectValue(tblRetrieveDetail.Item(index).Item("stt_rec"))
                            tblRetrieveDetail.Item(index).Item("stt_rec0bg") = RuntimeHelpers.GetObjectValue(tblRetrieveDetail.Item(index).Item("stt_rec0"))
                            tblRetrieveDetail.Item(index).Item("so_luong") = RuntimeHelpers.GetObjectValue(tblRetrieveDetail.Item(index).Item("sl_dh0"))
                            tblRetrieveDetail.Item(index).Row.AcceptChanges()
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
                        Dim num8 As Integer = (tbl.Rows.Count - 1)
                        index = 0
                        Do While (index <= num8)
                            If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                tbl.Rows.Item(index).Item("stt_rec") = ""
                            Else
                                tbl.Rows.Item(index).Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                            End If
                            tbl.Rows.Item(index).Item("sl_dh") = 0
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
                                ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_bg")), "C") Then
                                    modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                                End If
                                index = (index + -1)
                            Loop
                            Dim num6 As Integer = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))
                            If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                                num6 = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
                            End If
                            Dim num7 As Integer = (modVoucher.tblDetail.Count - 1)
                            index = 0
                            Do While (index <= num7)
                                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt2"))) Then
                                    tblDetail.Item(index).Item("tien_nt2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("so_luong"), tblDetail.Item(index).Item("gia_nt2")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                                End If
                                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt2"))) Then
                                    tblDetail.Item(index).Item("gia2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("gia_nt2"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                                End If
                                Dim args As Object() = New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("so_luong"), tblDetail.Item(index).Item("gia_nt2")), num6}
                                Dim copyBack As Boolean() = New Boolean() {False, True}
                                If copyBack(1) Then
                                    num6 = IntegerType.FromObject(args(1))
                                End If
                                tblDetail.Item(index).Item("tien_nt2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                                tblDetail.Item(index).Item("tien2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("tien_nt2"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                                Me.RecalcTax(index, 2)
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
        If ((StringType.StrCmp(Strings.Trim(Strings.Left(Me.cboAction.Text, 1)), "1", False) = 0) AndAlso Not Me.CheckCredit) Then
            Me.cboAction.SelectedIndex = 2
        End If
        Me.txtStatus.Text = Strings.Trim(StringType.FromObject(Me.tblHandling.Rows.Item(Me.cboAction.SelectedIndex).Item("action_id")))
        Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
        Me.txtNgay_ct.Value = Me.txtNgay_lct.Value
        Try
            Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Try
        Catch exception2 As Exception
            ProjectData.SetProjectError(exception2)
            ProjectData.ClearProjectError()
        End Try
        If Not Me.oSecurity.GetActionRight Then
            oVoucher.isContinue = False
        ElseIf Not Me.grdHeader.CheckEmpty(RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("035"))) Then
            oVoucher.isContinue = False
        Else
            Dim num As Integer
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
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("022")), 2)
                oVoucher.isContinue = False
            Else
                Dim str As String
                Dim num2 As Integer
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
                Dim num12 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num12)
                    Dim num11 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    num2 = 1
                    Do While (num2 <= num11)
                        str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str))) Then
                            modVoucher.tblDetail.Item(num).Item(str) = ""
                        End If
                        num2 += 1
                    Loop
                    num += 1
                Loop
                cString = StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
                Dim num10 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Dim _han_visa As Date
                Do While (num <= num10)
                    If tblDetail.Item(num).Item("ma_vt").ToString.Trim.Substring(0, 1) = "A" Then
                        If IsDBNull(Sql.GetValue(appConn, "dmvt", "ngay_td2", "ma_vt='" + tblDetail.Item(num).Item("ma_vt").ToString.Trim + "'")) Then
                            If Msg.Question("Chưa khai báo hạn visa của hàng:" + tblDetail.Item(num).Item("ma_vt").ToString.Trim + ". Bạn có muốn lưu tiếp không?") = 0 Then
                                oVoucher.isContinue = False
                                Return
                            End If
                        Else
                            _han_visa = CDate(Sql.GetValue(appConn, "dmvt", "ngay_td2", "ma_vt='" + tblDetail.Item(num).Item("ma_vt").ToString.Trim + "'"))
                            If _han_visa < Me.txtNgay_ct.Value Then
                                If Msg.Question("Quá hạn visa của hàng:" + tblDetail.Item(num).Item("ma_vt").ToString.Trim + ". Bạn có muốn lưu tiếp không?") = 0 Then
                                    oVoucher.isContinue = False
                                    Return
                                End If
                            End If
                        End If
                    End If


                    Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    num2 = 1
                    Do While (num2 <= num9)
                        str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str))) Then
                            modVoucher.tblDetail.Item(num).Item(str) = 0
                        End If
                        num2 += 1
                    Loop
                    If (StringType.StrCmp(Strings.Trim(Me.txtLoai_ct.Text), "3", False) = 0) Then
                        modVoucher.tblDetail.Item(num).Item("sl_xuat") = RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("so_luong"))
                        modVoucher.tblDetail.Item(num).Item("sl_giao") = RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("so_luong"))
                    End If
                    num += 1
                Loop
                'If (StringType.StrCmp(Me.txtStatus.Text, "0", False) <> 0) Then
                Dim str3 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldcheck", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
                    If (StringType.StrCmp(Strings.Trim(str3), "", False) <> 0) Then
                        num3 = (modVoucher.tblDetail.Count - 1)
                        Dim sLeft As String = clsfields.CheckEmptyFieldList("stt_rec", str3, modVoucher.tblDetail)
                        Try
                            If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                                Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("044")), "%s", GetColumn(Me.grdDetail, sLeft).HeaderText, 1, -1, CompareMethod.Binary), 2)
                                oVoucher.isContinue = False
                                Return
                            End If
                        Catch exception3 As Exception
                            ProjectData.SetProjectError(exception3)
                            Dim exception As Exception = exception3
                            ProjectData.ClearProjectError()
                        End Try
                    End If
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        Me.cIDNumber = ""
                    Else
                        Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
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
                    If (StringType.StrCmp(Strings.Trim(Me.txtLoai_ct.Text), "3", False) = 0) Then
                        If (StringType.StrCmp(Strings.Trim(Me.txtStatus.Text), "2", False) = 0) Then
                            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("065")), 2)
                            oVoucher.isContinue = False
                            Return
                        End If
                        'If (StringType.StrCmp(Strings.Trim(Me.txtSo_ct0.Text), "", False) = 0) Then
                        '    Me.tbDetail.SelectedIndex = 4
                        '    Me.txtSo_ct0.Focus()
                        '    Msg.Alert(StringType.FromObject(modVoucher.oVar.Item("m_not_blank")), 2)
                        '    oVoucher.isContinue = False
                        '    Return
                        'End If
                        'If (ObjectType.ObjTst(Me.txtNgay_ct0.Text, Fox.GetEmptyDate, False) = 0) Then
                        '    Me.tbDetail.SelectedIndex = 4
                        '    Me.txtNgay_ct0.Focus()
                        '    Msg.Alert(StringType.FromObject(modVoucher.oVar.Item("m_not_blank")), 2)
                        '    oVoucher.isContinue = False
                        '    Return
                        'End If
                    End If
                    num3 = (modVoucher.tblDetail.Count - 1)
                    If (StringType.StrCmp(Strings.Trim(Me.txtLoai_ct.Text), "3", False) = 0) Then
                        num = num3
                        Do While (num >= 0)
                            If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ngay_giao"))) AndAlso (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("ngay_giao"), Me.txtNgay_lct.Value, False) > 0)) Then
                                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("067")), 2)
                                oVoucher.isContinue = False
                                Return
                            End If
                            num = (num + -1)
                        Loop
                    Else
                        num = num3
                        Do While (num >= 0)
                            If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ngay_giao"))) AndAlso (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("ngay_giao"), Me.txtNgay_lct.Value, False) < 0)) Then
                                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("066")), 2)
                                oVoucher.isContinue = False
                                Return
                            End If
                            num = (num + -1)
                        Loop
                    End If
                'End If
                If Not Me.xInventory.isValid Then
                    oVoucher.isContinue = False
                Else
                    Dim strSQL As String = ""
                    Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
                    If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                        auditamount.AuditAmounts(New Decimal(Me.txtT_tien2.Value), "tien2", modVoucher.tblDetail)
                        auditamount.AuditAmounts(New Decimal(Me.txtT_ck.Value), "ck", modVoucher.tblDetail)
                    End If
                    Me.UpdateSO()
                    Me.UpdateList()
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        Me.cIDNumber = oVoucher.GetIdentityNumber
                        modVoucher.tblMaster.AddNew()
                        Me.iMasterRow = (modVoucher.tblMaster.Count - 1)
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec") = Me.cIDNumber
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_ct") = modVoucher.VoucherCode
                    Else
                        Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                        strSQL += ChrW(13) + Me.BeforUpdateSO(Me.cIDNumber, "Edit")
                    End If
                    xtabControl.GatherMemvarTabControl(modVoucher.tblMaster.Item(Me.iMasterRow), Me.tbDetail)
                    DirLib.SetDatetime(modVoucher.appConn, modVoucher.tblMaster.Item(Me.iMasterRow), oVoucher.cAction)
                    Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
                    Me.grdHeader.Gather()
                    clsvoucher.clsVoucher.GatherMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
                    modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct"))), Me.txtSo_ct.MaxLength)
                    modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct0") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct0"))), Me.txtSo_ct0.MaxLength)
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        strSQL += ChrW(13) + GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row)
                    Else
                        Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                        strSQL += ChrW(13) + ((GenSQLUpdate((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row, cKey) & ChrW(13) & GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)) & ChrW(13) & GenSQLDelete("ctcp20", cKey))
                    End If
                    cString = "ma_ct, ngay_ct, so_ct, stt_rec"
                    Dim str5 As String = ("stt_rec = '" & Me.cIDNumber & "' or stt_rec = '' or stt_rec is null")
                    modVoucher.tblDetail.RowFilter = str5
                    num3 = (modVoucher.tblDetail.Count - 1)
                    Dim expression As Integer = 0
                    Dim num8 As Integer = num3
                    Dim num7 As Integer
                    num = 0
                    Do While (num <= num8)
                        If (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("stt_rec"), Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "", RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))), False) = 0) Then
                            num7 = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                            num2 = 1
                            Do While (num2 <= num7)
                                str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                                modVoucher.tblDetail.Item(num).Item(str) = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item(str))
                                num2 += 1
                            Loop
                            expression += 1
                            modVoucher.tblDetail.Item(num).Item("line_nbr") = expression
                            Me.grdDetail.Update()
                            strSQL += ChrW(13) + GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), modVoucher.tblDetail.Item(num).Row)
                        End If
                        num += 1
                    Loop
                    cString = "ma_ct, so_ct, loai_ct, ngay_ct, ngay_lct, stt_rec, ma_dvcs, datetime0, datetime2, user_id0, user_id2, status"
                    expression = 0
                    num3 = (modVoucher.tblCharge.Count - 1)
                    Dim num6 As Integer = num3
                    Dim num5 As Integer
                    num = 0
                    Do While (num <= num6)
                        num5 = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
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
                        strSQL += ChrW(13) + GenSQLInsert((modVoucher.appConn), "ctcp20", modVoucher.tblCharge.Item(num).Row)
                        num += 1
                    Loop
                    oVoucher.IncreaseVoucherNo(Strings.Trim(Me.txtSo_ct.Text))
                    Me.EDTBColumns(False)
                    strSQL += ChrW(13) + Me.Post
                    strSQL += ChrW(13) + Me.grdHeader.SQLUpdateFreeField(modVoucher.appConn, Conversions.ToString(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")))
                    strSQL += ChrW(13) + Me.AfterUpdateSO(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "Save")
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
                    xtabControl.ReadOnlyTabControls(True, Me.tbDetail)
                End If
            End If
        End If
    End Sub

    Private Sub SaveCharge()
        Dim cString As String = "cp_vc, cp_vc_nt, cp_bh, cp_bh_nt, cp_khac, cp_khac_nt"
        Dim num4 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim i As Integer = 0
        Dim num3 As Integer
        Dim j As Integer
        Dim str2 As String
        Dim str As String
        Do While (i <= num4)
            num3 = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            j = 1
            Do While (j <= num3)
                str2 = Strings.Trim(Fox.GetWordNum(cString, j, ","c))
                str = (str2 & "2")
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
        If Not Me.oInvItemDetail.Cancel Then
            Me.iOldRow = Me.grdDetail.CurrentRowIndex
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
            Me.cOldItem = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        End If
    End Sub

    Private Sub SetEmptyColKeyCharge(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRowIndex As Integer = Me.grdCharge.CurrentRowIndex
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("ma_cp"))) Then
        End If
        Me.coldCMa_cp = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub ShowTabDetail()
        Me.tbDetail.SelectedIndex = 0
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

    Private Sub tbDetail_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles tbDetail.Enter
        Me.grdDetail.Focus()
    End Sub

    Private Sub TransTypeLostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtMa_gd.LostFocus
        Me.EDTranType()
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

    Private Sub txtCk_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldCk = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtCk_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldCk_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtCk_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num4 As Decimal = Me.noldCk_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ck_nt") = num
            modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ck") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
            Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtCk_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim noldCk As Decimal = Me.noldCk
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, noldCk) <> 0) Then
            modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ck") = num
            Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 1)
            Me.UpdateList()
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
            modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("tien_cp_nt") = num
            modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("tien_cp") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
        End If
        Me.ShowTotalCharge(2)
    End Sub

    Private Sub txtCTien_cp_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num3 As Decimal = Me.noldCTien_cp
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num3) <> 0) Then
            modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("tien_cp") = num
        End If
        Me.ShowTotalCharge(1)
    End Sub

    Private Sub txtDien_giai_Leave(ByVal sender As Object, ByVal e As EventArgs)
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
            modVoucher.tblDetail.Item(Me.grdMV.CurrentRowIndex).Item(cField) = num
            modVoucher.tblDetail.Item(Me.grdMV.CurrentRowIndex).Item(cRef) = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
        End If
        Me.ShowTotalECharge(cField, True)
    End Sub

    Private Sub txtGia_nt2_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia_nt2 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtGia_nt2_valid(ByVal sender As Object, ByVal e As EventArgs)
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
        Dim num6 As Decimal = Me.noldGia_nt2
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num6) <> 0) Then
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("gia_nt2") = num
                .Item("tien_nt2") = Math.Round(.Item("so_luong") * num, digits)
                .Item("gia2") = Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits)
                .Item("Tien2") = Fox.Round(.Item("tien_nt2") * Me.txtTy_gia.Value, num5)
                If IsDBNull(.Item("ck_nt")) Then
                    .Item("ck_nt") = 0
                End If
                If IsDBNull(.Item("ck")) Then
                    .Item("ck") = 0
                End If
                If IsDBNull(.Item("cp_nt")) Then
                    .Item("cp_nt") = 0
                End If
                If IsDBNull(.Item("cp")) Then
                    .Item("cp") = 0
                End If
                .Item("tt_nt") = .Item("tien_nt2") + .Item("thue_nt") + .Item("cp_nt") - .Item("ck_nt")
                .Item("tt") = Fox.Round(.Item("tt_nt") * Me.txtTy_gia.Value, num5)
                Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtGia2_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia2 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtGia2_valid(ByVal sender As Object, ByVal e As EventArgs)
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
        Dim num6 As Decimal = Me.noldGia2
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num6) <> 0) Then
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("gia2") = num
            Dim args As Object() = New Object() {ObjectType.MulObj(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("so_luong"), num), num5}
            Dim copyBack As Boolean() = New Boolean() {False, True}
            If copyBack(1) Then
                num5 = ByteType.FromObject(args(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("tien2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
            Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 1)
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtKeyPress_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtKeyPress.Enter
        Me.grdDetail.Focus()
        Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
    End Sub

    Private Sub txtMa_dc_Enter(ByVal sender As Object, ByVal e As EventArgs)
        Dim str As String = ("ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'")
        Me.oSOAddress.Key = str
    End Sub

    'Private Sub txtMa_gd_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtMa_gd.Enter
    '    If (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) Then
    '        Me.txtMa_gd.ReadOnly = True
    '    End If
    '    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
    '        Dim flag As Boolean = False
    '        Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
    '        Dim i As Integer = 0
    '        Do While (i <= num2)
    '            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("ma_vt")), "C") Then
    '                flag = True
    '                Exit Do
    '            End If
    '            i += 1
    '        Loop
    '        Me.txtMa_gd.ReadOnly = flag
    '    End If
    'End Sub

    Private Sub txtMa_gd_Valid(ByVal sender As Object, ByVal e As EventArgs)
        If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
            Me.EDTrans()
            If Not Me.txtNgay_ct3.Enabled Then
                Me.txtNgay_ct3.Text = StringType.FromObject(Fox.GetEmptyDate)
            End If
        End If
    End Sub

    Private Sub txtMa_kh_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim cKey As String = ("ma_kh = '" & Me.txtMa_kh.Text & "'")
        If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) And (StringType.StrCmp(Strings.Trim(Me.txtMa_tt.Text), "", False) = 0)) Then
            Me.txtMa_tt.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "ma_tt", cKey)))
        End If
        If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
            Me.txtTen_kh0.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", StringType.FromObject(ObjectType.AddObj("ten_kh", Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), "", "2"))), cKey)))
            Me.txtDia_chi.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "dia_chi", cKey)))
            Me.txtDien_thoai.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "dien_thoai", cKey)))
            Me.txtFax.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "fax", cKey)))
        End If
    End Sub

    Private Sub txtMa_thue_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.coldMa_thue = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub txtMa_thue_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Byte
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num = num2
        Else
            num = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim str As String = Me.coldMa_thue
        Dim str3 As String = StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))
        If (StringType.StrCmp(Strings.Trim(str3), Strings.Trim(str), False) <> 0) Then
            Dim str2 As String
            Dim zero As Decimal
            If (StringType.StrCmp(Strings.Trim(str3), "", False) = 0) Then
                zero = Decimal.Zero
                str2 = ""
            Else
                Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmthue", ("ma_thue = '" & Strings.Trim(str3) & "'")), DataRow)
                zero = DecimalType.FromObject(row.Item("thue_suat"))
                str2 = StringType.FromObject(row.Item("tk_thue_no3"))
                row = Nothing
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue_suat") = zero
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("tk_thue") = str2
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue") = str3
            Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
            Me.colThue_nt.TextBox.Text = Strings.Trim(StringType.FromObject(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue_nt")))
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
        If (Decimal.Compare(num, num4) <> 0) Then
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("gia_nt2"))) Then
                tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("gia_nt2") = 0
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("gia2"))) Then
                tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("gia2") = 0
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("so_luong") = num
            Dim args As Object() = New Object() {ObjectType.MulObj(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("gia_nt2"), num), num2}
            Dim copyBack As Boolean() = New Boolean() {False, True}
            If copyBack(1) Then
                num2 = ByteType.FromObject(args(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("tien_nt2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
            Dim objArray2 As Object() = New Object() {ObjectType.MulObj(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("gia2"), num), num3}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num3 = ByteType.FromObject(objArray2(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("tien2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
            Me.grdDetail.Refresh()
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtThue_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.nOldThue = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtThue_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.nOldThue_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtThue_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num3 As Decimal = Me.nOldThue_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num3) <> 0) Then
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue_nt") = num
            Dim args As Object() = New Object() {ObjectType.MulObj(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("Thue_nt"), Me.txtTy_gia.Value), num2}
            Dim copyBack As Boolean() = New Boolean() {False, True}
            If copyBack(1) Then
                num2 = ByteType.FromObject(args(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtThue_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim nOldThue As Decimal = Me.nOldThue
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, nOldThue) <> 0) Then
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien_nt2_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien_nt2 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTien_nt2_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num4 As Decimal = Me.noldTien_nt2
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("Tien_nt2") = num
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("Tien2") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
            Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien2_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien2 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTien2_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num3 As Decimal = Me.noldTien2
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num3) <> 0) Then
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("Tien2") = num
            Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 1)
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTk_Validated(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private Sub txtTy_gia_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtTy_gia.Enter
        oVoucher.noldFCrate = New Decimal(Me.txtTy_gia.Value)
    End Sub

    Private Sub txtTy_gia_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtTy_gia.Validated
        Me.vFCRate()
    End Sub

    Public Sub UpdateList()
        Dim zero As Decimal = Decimal.Zero
        Dim num10 As Decimal = Decimal.Zero
        Dim num4 As Decimal = Decimal.Zero
        Dim num5 As Decimal = Decimal.Zero
        Dim num2 As Decimal = Decimal.Zero
        Dim num3 As Decimal = Decimal.Zero
        Dim num7 As Decimal = Decimal.Zero
        Dim num8 As Decimal = Decimal.Zero
        Dim num6 As Decimal = Decimal.Zero
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit", "View"}) Then
            Dim num11 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num11)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien2"))) Then
                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblDetail.Item(i).Item("tien2")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien_nt2"))) Then
                    num10 = DecimalType.FromObject(ObjectType.AddObj(num10, modVoucher.tblDetail.Item(i).Item("tien_nt2")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("cp"))) Then
                    num4 = DecimalType.FromObject(ObjectType.AddObj(num4, modVoucher.tblDetail.Item(i).Item("cp")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("cp_nt"))) Then
                    num5 = DecimalType.FromObject(ObjectType.AddObj(num5, modVoucher.tblDetail.Item(i).Item("cp_nt")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("ck"))) Then
                    num2 = DecimalType.FromObject(ObjectType.AddObj(num2, modVoucher.tblDetail.Item(i).Item("ck")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("ck_nt"))) Then
                    num3 = DecimalType.FromObject(ObjectType.AddObj(num3, modVoucher.tblDetail.Item(i).Item("ck_nt")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("thue"))) Then
                    num7 = DecimalType.FromObject(ObjectType.AddObj(num7, modVoucher.tblDetail.Item(i).Item("thue")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("thue_nt"))) Then
                    num8 = DecimalType.FromObject(ObjectType.AddObj(num8, modVoucher.tblDetail.Item(i).Item("thue_nt")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("so_luong"))) Then
                    num6 = DecimalType.FromObject(ObjectType.AddObj(num6, modVoucher.tblDetail.Item(i).Item("so_luong")))
                End If
                i += 1
            Loop
        End If
        Me.txtT_so_luong.Value = Convert.ToDouble(num6)
        Me.txtT_cp.Value = Convert.ToDouble(num4)
        Me.txtT_cp_nt.Value = Convert.ToDouble(num5)
        Me.txtT_ck.Value = Convert.ToDouble(num2)
        Me.txtT_ck_nt.Value = Convert.ToDouble(num3)
        Me.txtT_thue.Value = Convert.ToDouble(num7)
        Me.txtT_thue_nt.Value = Convert.ToDouble(num8)
        Me.txtT_tien2.Value = Convert.ToDouble(zero)
        Me.txtT_tien_nt2.Value = Convert.ToDouble(num10)
        Me.txtT_tt.Value = ((Me.txtT_tien2.Value + Me.txtT_thue.Value) - Me.txtT_ck.Value)
        Me.txtT_tt_nt.Value = ((Me.txtT_tien_nt2.Value + Me.txtT_thue_nt.Value) - Me.txtT_ck_nt.Value)
    End Sub

    Private Sub UpdateSO()
    End Sub

    Private Sub ValidObjects(ByVal sender As Object, ByVal e As EventArgs)
        'If Not ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
        '    Return
        'End If
        'Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
        'If (Me.iOldRow <> currentRowIndex) Then
        '    Return
        'End If
        'Dim ds As New DataSet
        'Dim num4 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        'Dim num3 As Byte
        'If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
        '    num3 = num4
        'Else
        '    num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        'End If
        'Dim str5 As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        'Dim objArray3 As Object() = New Object(1 - 1) {}
        'Dim o As Object = sender
        'Dim args As Object() = New Object(0 - 1) {}
        'Dim paramnames As String() = Nothing
        'objArray3(0) = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(LateBinding.LateGet(o, Nothing, "Name", args, paramnames, Nothing), Nothing, "Trim", New Object(0 - 1) {}, Nothing, Nothing))
        'Dim objArray2 As Object() = objArray3
        'Dim copyBack As Boolean() = New Boolean() {True}
        'If copyBack(0) Then
        '    LateBinding.LateSetComplex(LateBinding.LateGet(o, Nothing, "Name", args, paramnames, Nothing), Nothing, "Trim", New Object() {RuntimeHelpers.GetObjectValue(objArray2(0))}, Nothing, True, True)
        'End If
        'Dim obj2 As Object = LateBinding.LateGet(Nothing, GetType(Strings), "UCase", objArray2, Nothing, copyBack)
        'Dim sOldStringDvt As String
        'If (ObjectType.ObjTst(obj2, "MA_VT", False) = 0) Then
        '    sOldStringDvt = Me.sOldStringMa_vt
        'ElseIf (ObjectType.ObjTst(obj2, "MA_KHO", False) = 0) Then
        '    sOldStringDvt = Me.sOldStringMa_kho
        'ElseIf (ObjectType.ObjTst(obj2, "DVT", False) = 0) Then
        '    sOldStringDvt = Me.sOldStringDvt
        'ElseIf (ObjectType.ObjTst(obj2, "SO_LUONG", False) = 0) Then
        '    sOldStringDvt = Strings.Replace(Me.sOldStringSo_luong, " ", "", 1, -1, CompareMethod.Binary)
        'Else
        '    Return
        'End If

        'If (StringType.StrCmp(Strings.Trim(str5), Strings.Trim(sOldStringDvt), False) = 0) Then
        '    Return
        'End If
        'Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Name", New Object(0 - 1) {}, Nothing, Nothing)))
        'Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "sysspdetailinfo", String.Concat(New String() {"xid = '", modVoucher.VoucherCode, "' AND xvalid = '", str, "'"})), DataRow)
        'Dim str4 As String = StringType.FromObject(row.Item("xfields"))
        'Dim str3 As String = StringType.FromObject(row.Item("xfcfields"))
        'Dim cString As String = StringType.FromObject(row.Item("xreffields"))
        'If (StringType.StrCmp(Strings.Trim(str4), "", False) = 0) Then
        '    Return
        'End If
        'Dim str8 As String = "EXEC fs_GetSOPrice "
        'str8 = (str8 & "'" & Strings.Trim(str) & "'")
        'str8 = (str8 & ", '" & Strings.Trim(modVoucher.VoucherCode) & "'")
        'str8 = StringType.FromObject(ObjectType.AddObj(str8, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, ""))))
        'str8 = (str8 & ", '" & Strings.Trim(Me.txtMa_tt.Text) & "'")
        'str8 = (str8 & ", '" & Strings.Trim(Me.cmdMa_nt.Text) & "'")
        'str8 = (str8 & ", '" & Strings.Trim(Me.txtMa_kh.Text) & "'")
        'Dim view2 As DataRowView = modVoucher.tblDetail.Item(currentRowIndex)
        'If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("ma_vt"))) Then
        '    str8 = (str8 & ", ''")
        'Else
        '    str8 = (str8 & ", '" & Strings.Trim(StringType.FromObject(view2.Item("ma_vt"))) & "'")
        'End If
        'If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("ma_kho"))) Then
        '    str8 = (str8 & ", ''")
        'Else
        '    str8 = (str8 & ", '" & Strings.Trim(StringType.FromObject(view2.Item("ma_kho"))) & "'")
        'End If
        'If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("dvt"))) Then
        '    str8 = (str8 & ", ''")
        'Else
        '    str8 = (str8 & ", N'" & Strings.Trim(StringType.FromObject(view2.Item("dvt"))) & "'")
        'End If
        'If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("so_luong"))) Then
        '    str8 = (str8 & ", 0")
        'Else
        '    str8 = (str8 & ", " & Strings.Trim(StringType.FromObject(view2.Item("so_luong"))))
        'End If
        'If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("he_so"))) Then
        '    str8 = (str8 & ", 1")
        'Else
        '    str8 = (str8 & ", " & Strings.Trim(StringType.FromObject(view2.Item("he_so"))) & "")
        'End If
        'Sql.SQLRetrieve((modVoucher.appConn), str8, "xprice", (ds))
        'If ds.Tables(0).Rows.Count = 0 Then
        '    Return
        'End If
        'Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(str4, ","c))
        'Dim nWordPosition As Integer = 1
        'For nWordPosition = 1 To num9
        '    str = Strings.Trim(Fox.GetWordNum(str4, nWordPosition, ","c))
        '    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(ds.Tables.Item(0).Rows.Item(0).Item(str))) Then
        '        modVoucher.tblDetail.Item(currentRowIndex).Item(str) = RuntimeHelpers.GetObjectValue(ds.Tables.Item(0).Rows.Item(0).Item(str))
        '    End If
        'Next
        'If (StringType.StrCmp(Strings.Trim(str3), "", False) <> 0) Then
        '    Dim num8 As Integer = IntegerType.FromObject(Fox.GetWordCount(str3, ","c))
        '    For nWordPosition = 1 To num8
        '        str = Strings.Trim(Fox.GetWordNum(str3, nWordPosition, ","c))
        '        Dim str2 As String = Strings.Trim(Fox.GetWordNum(cString, nWordPosition, ","c))
        '        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item(str))) Then
        '            tblDetail.Item(currentRowIndex).Item(str2) = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(currentRowIndex).Item(str), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
        '        End If
        '    Next
        'End If
        'If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item("gia_nt2"))) Then
        '    tblDetail.Item(currentRowIndex).Item("gia_nt2") = 0
        'End If
        'If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item("gia2"))) Then
        '    tblDetail.Item(currentRowIndex).Item("gia2") = 0
        'End If
        'If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item("so_luong"))) Then
        '    tblDetail.Item(currentRowIndex).Item("so_luong") = 0
        'End If
        'objArray2 = New Object() {ObjectType.MulObj(tblDetail.Item(currentRowIndex).Item("gia_nt2"), tblDetail.Item(currentRowIndex).Item("so_luong")), num3}
        'copyBack = New Boolean() {False, True}
        'If copyBack(1) Then
        '    num3 = ByteType.FromObject(objArray2(1))
        'End If
        'tblDetail.Item(currentRowIndex).Item("tien_nt2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
        'objArray2 = New Object() {ObjectType.MulObj(tblDetail.Item(currentRowIndex).Item("gia2"), tblDetail.Item(currentRowIndex).Item("so_luong")), num4}
        'copyBack = New Boolean() {False, True}
        'If copyBack(1) Then
        '    num4 = ByteType.FromObject(objArray2(1))
        'End If
        'tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("tien2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
        'Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
        'ds = Nothing
        'Me.UpdateList()
        If Not ((oVoucher.cAction = "New") Or (oVoucher.cAction = "Edit")) Then
            Return
        End If
        Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
        If (Me.iOldRow <> currentRowIndex) Then
            Return
        End If
        Dim ds As New DataSet
        Dim num4 As Byte = oVar.Item("m_round_tien")
        Dim num3 As Byte
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num3 = num4
        Else
            num3 = oVar.Item("m_round_tien_nt")
        End If
        Dim str5 As String = Trim(sender.Text)
        Dim sLeft As String = UCase(sender.Name)
        Dim oldValue As String = ""
        Select Case sLeft
            Case "MA_VT"
                oldValue = Me.sOldStringMa_vt
            Case "MA_KHO"
                oldValue = Me.sOldStringMa_kho
            Case "DVT"
                oldValue = Me.sOldStringDvt
            Case "SO_LUONG"
                oldValue = Replace(Me.sOldStringSo_luong, " ", "")
        End Select
        If (StringType.StrCmp(Strings.Trim(str5), Strings.Trim(oldValue), False) = 0) Then
            Return
        End If
        Dim str As String = Strings.Trim(sender.name)
        Dim row As DataRow = Sql.GetRow(appConn, "sysspdetailinfo", "xid = '" + VoucherCode + "' AND xvalid = '" + str + "'")
        Dim str4 As String = row.Item("xfields")
        Dim str3 As String = row.Item("xfcfields")
        Dim cString As String = row.Item("xreffields")
        If str4.Trim = "" Then
            Return
        End If
        Dim str8 As String = "EXEC fs_GetSOPrice "
        str8 = (str8 & "'" & Strings.Trim(str) & "'")
        str8 = (str8 & ", '" & Strings.Trim(VoucherCode) & "'")
        str8 += ", " + Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")
        str8 = (str8 & ", '" & Strings.Trim(Me.txtMa_tt.Text) & "'")
        str8 = (str8 & ", '" & Strings.Trim(Me.cmdMa_nt.Text) & "'")
        str8 = (str8 & ", '" & Strings.Trim(Me.txtMa_kh.Text) & "'")
        Dim view2 As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("ma_vt"))) Then
            str8 = (str8 & ", ''")
        Else
            str8 = (str8 & ", '" & Strings.Trim(StringType.FromObject(view2.Item("ma_vt"))) & "'")
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("ma_kho"))) Then
            str8 = (str8 & ", ''")
        Else
            str8 = (str8 & ", '" & Strings.Trim(StringType.FromObject(view2.Item("ma_kho"))) & "'")
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("dvt"))) Then
            str8 = (str8 & ", ''")
        Else
            str8 = (str8 & ", N'" & Strings.Trim(StringType.FromObject(view2.Item("dvt"))) & "'")
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("so_luong"))) Then
            str8 = (str8 & ", 0")
        Else
            str8 = (str8 & ", " & Strings.Trim(StringType.FromObject(view2.Item("so_luong"))))
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("he_so"))) Then
            str8 = (str8 & ", 1")
        Else
            str8 = (str8 & ", " & Strings.Trim(StringType.FromObject(view2.Item("he_so"))) & "")
        End If
        view2 = Nothing
        Sql.SQLRetrieve(appConn, str8, "xprice", ds)
        If ds.Tables(0).Rows.Count = 0 Then
            Return
        End If
        Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(str4, ","c))
        Dim nWordPosition As Integer = 1
        For nWordPosition = 1 To num9
            str = Strings.Trim(Fox.GetWordNum(str4, nWordPosition, ","c))
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(ds.Tables.Item(0).Rows.Item(0).Item(str))) Then
                modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item(str) = RuntimeHelpers.GetObjectValue(ds.Tables.Item(0).Rows.Item(0).Item(str))
            End If
        Next
        With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If str3.Trim <> "" Then
                Dim num8 As Integer = IntegerType.FromObject(Fox.GetWordCount(str3, ","c))
                Dim str2 As String
                nWordPosition = 1
                For nWordPosition = 1 To num8
                    str = Strings.Trim(Fox.GetWordNum(str3, nWordPosition, ","c))
                    str2 = Strings.Trim(Fox.GetWordNum(cString, nWordPosition, ","c))
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item(str))) Then
                        .Item(str2) = Fox.Round(.Item(str) * Me.txtTy_gia.Value, CInt(oVar.Item("m_round_gia")))
                    End If
                Next
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia_nt2"))) Then
                .Item("gia_nt2") = 0
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia2"))) Then
                .Item("gia2") = 0
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("so_luong"))) Then
                .Item("so_luong") = 0
            End If
            .Item("tien_nt2") = Fox.Round(.Item("gia_nt2") * .Item("so_luong"), num3)
            .Item("tien2") = Math.Round(.Item("gia2") * .Item("so_luong"), num4)
        End With
        Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
        ds = Nothing
        Me.UpdateList()
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
            Dim num As Integer
            Dim num3 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num3)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("gia_nt2"))) Then
                    tblDetail.Item(num).Item("gia2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("gia_nt2"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("gia_ban_nt"))) Then
                    tblDetail.Item(num).Item("gia_ban") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("gia_ban_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("tien_nt2"))) Then
                    tblDetail.Item(num).Item("tien2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("tien_nt2"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("thue_nt"))) Then
                    tblDetail.Item(num).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("thue_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("ck_nt"))) Then
                    tblDetail.Item(num).Item("ck") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("ck_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("cp_vc_nt"))) Then
                    tblDetail.Item(num).Item("cp_vc") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("cp_vc_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("cp_bh_nt"))) Then
                    tblDetail.Item(num).Item("cp_bh") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("cp_bh_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("cp_khac_nt"))) Then
                    tblDetail.Item(num).Item("cp_khac") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("cp_khac_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("cp_nt"))) Then
                    tblDetail.Item(num).Item("cp") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("cp_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                num += 1
            Loop
            Dim num2 As Integer = (modVoucher.tblCharge.Count - 1)
            num = 0
            Do While (num <= num2)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblCharge.Item(num).Item("tien_cp_nt"))) Then
                    tblCharge.Item(num).Item("tien_cp") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblCharge.Item(num).Item("tien_cp_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                num += 1
            Loop
        End If
        Me.txtT_tien2.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_tien_nt2.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_cp.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_cp_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_ck.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_ck_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_thue.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_thue_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_tt.Value = ((Me.txtT_tien2.Value + Me.txtT_thue.Value) - Me.txtT_ck.Value)
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
        Loop While (index < MaxColumns)
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
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblMaster), gridformtran2, (tbs), (cols), "SOMaster")
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
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), gridformtran, (style), (cols), "SODetail")
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
        Dim collection As New Collection
        Dim collection2 As Collection = collection
        collection2.Add(Me, "Form", Nothing, Nothing)
        collection2.Add(gridformtran2, "grdHeader", Nothing, Nothing)
        collection2.Add(gridformtran, "grdDetail", Nothing, Nothing)
        collection2 = Nothing
        Me.oSecurity.aVGrid = collection
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
        Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        If (StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.coldCMa_cp), False) = 0) Then
            Return
        End If
        If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("ma_cp")), "C") Then
            tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("loai_cp") = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "dmcp", "loai_cp", ("ma_loai = '" & str & "'")))
            tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("loai_pb") = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "dmcp", "loai_pb", ("ma_loai = '" & str & "'")))
        Else
            tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("tien_cp_nt") = 0
            tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("tien_cp") = 0
        End If

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

        If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item("ma_vt")), "C") Then
            Return
        End If
        Dim str2 As String = Strings.Trim(StringType.FromObject(tblDetail.Item(currentRowIndex).Item("ma_vt")))
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmvt", ("ma_vt = '" & str2 & "'")), DataRow)
        tblDetail.Item(currentRowIndex).Item("volume") = RuntimeHelpers.GetObjectValue(row.Item("volume"))
        tblDetail.Item(currentRowIndex).Item("weight") = RuntimeHelpers.GetObjectValue(row.Item("weight"))
        If BooleanType.FromObject(ObjectType.NotObj(row.Item("sua_tk_vt"))) Then
            tblDetail.Item(currentRowIndex).Item("tk_vt") = RuntimeHelpers.GetObjectValue(row.Item("tk_vt"))
        ElseIf clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item("tk_vt")), "C") Then
            tblDetail.Item(currentRowIndex).Item("tk_vt") = RuntimeHelpers.GetObjectValue(row.Item("tk_vt"))
        End If
        tblDetail.Item(currentRowIndex).Item("dvt") = RuntimeHelpers.GetObjectValue(row.Item("dvt"))
        Me.colDvt.TextBox.Text = StringType.FromObject(tblDetail.Item(currentRowIndex).Item("dvt"))
        tblDetail.Item(currentRowIndex).Item("he_so") = 1
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
        Try
            tblDetail.Item(currentRowIndex).Item("ton_order") = CDbl(Sql.GetValue(appConn, "select ton00 From z16socobalance13 where ma_dvcs=" + Sql.ConvertVS2SQLType(Me.txtMa_dvcs.Text, "") + " AND ma_vt=" + Sql.ConvertVS2SQLType(str2, "")))
        Catch ex As Exception
        End Try
        If BooleanType.FromObject(ObjectType.NotObj(row.Item("lo_yn"))) Then
            tblDetail.Item(currentRowIndex).Item("ma_lo") = ""
        ElseIf clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item("ma_lo")), "C") Then
            Dim str3 As String = StringType.FromObject(Sql.GetValue(modVoucher.appConn, ("fs_GetLotNumber '" & Strings.Trim(str2) & "'")))
            tblDetail.Item(currentRowIndex).Item("ma_lo") = str3
        End If
        If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item("ma_kho")), "C") Then
            tblDetail.Item(currentRowIndex).Item("ma_kho") = RuntimeHelpers.GetObjectValue(row.Item("ma_kho"))
            Try
                tblDetail.Item(currentRowIndex).Item("ma_kho") = Sql.GetValue(appConn, "select min(ma_kho) from dmkho where dbo.ff_inlist(ma_kho,'" + RuntimeHelpers.GetObjectValue(row.Item("ma_kho")) + "')=1 and ma_dvcs='" + Me.txtMa_dvcs.Text + "'")
            Catch ex As Exception
            End Try
        End If
        If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item("ma_vi_tri")), "C") Then
            tblDetail.Item(currentRowIndex).Item("ma_vi_tri") = RuntimeHelpers.GetObjectValue(row.Item("ma_vi_tri"))
        End If
        If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item("ma_thue")), "C") Then
            Dim row2 As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmthue", StringType.FromObject(ObjectType.AddObj("ma_thue = ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(row.Item("ma_thue")), "")))), DataRow)
            If Not (row2 Is Nothing) Then
                Me.coldMa_thue = ""
                tblDetail.Item(currentRowIndex).Item("ma_thue") = RuntimeHelpers.GetObjectValue(row2.Item("ma_thue"))
                Me.colMa_thue.TextBox.Text = StringType.FromObject(tblDetail.Item(currentRowIndex).Item("ma_thue"))
                Me.txtMa_thue_valid(Me.colMa_thue.TextBox, New EventArgs)
            End If
        End If
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

    Private Sub WhenNoneTax(ByVal sender As Object, ByVal e As EventArgs)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue"))) Then
            Me.grdDetail.TabProcess()
        Else
            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue"))), "", False) = 0) Then
                Me.grdDetail.TabProcess()
            End If
        End If
    End Sub

    Private Sub WhenSiteEnter(ByVal sender As Object, ByVal e As EventArgs)
        Me.cOldSite = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub WhenSiteLeave(ByVal sender As Object, ByVal e As EventArgs)
        If (Me.grdDetail.CurrentRowIndex >= 0) Then
            Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
            If Not ((StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.cOldSite), False) = 0) And Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ten_kho")), "C")) Then
                If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkho", "dai_ly_yn", ("ma_kho = '" & str & "'"))) Then
                    Dim str3 As String = Strings.Trim(StringType.FromObject(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")))
                    Dim sLeft As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "tk_dl", ("ma_vt = '" & str3 & "'"))))
                    If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                        tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("tk_vt") = sLeft
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub WhenUOMEnter(ByVal sender As Object, ByVal e As EventArgs)
        If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")), "C") Then
            Return
        End If
        If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(StringType.FromObject(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"))) & "'"))) Then
            Dim str As String = ("(ma_vt = '" & Strings.Trim(StringType.FromObject(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"))) & "' OR ma_vt = '*')")
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
        If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")), "C") Then
            Return
        End If
        If Not BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(StringType.FromObject(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"))) & "'"))) Then
            Return
        End If
        Dim cKey As String = "(ma_vt = '" + Trim(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")) + "' OR ma_vt = '*') AND dvt = N'"
        cKey += Trim(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)) + "'"
        Dim num As Decimal = DecimalType.FromObject(Sql.GetValue((modVoucher.appConn), "dmqddvt", "he_so", cKey))
        tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("He_so") = num
    End Sub

    'Private Sub Tinh_ton_dat_hang(IDNumber As String)
    '    Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(num).Item("stt_rec")), "'")
    '    modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
    'End Sub

    ' Properties
    Friend WithEvents cboAction As ComboBox
    Friend WithEvents cboStatus As ComboBox
    Friend WithEvents chkCk_thue_yn As CheckBox
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
    Friend WithEvents Label1 As Label
    Friend WithEvents lblAction As Label
    Friend WithEvents lblDc_cc As Label
    Friend WithEvents lblDia_chi As Label
    Friend WithEvents lblDt_cc As Label
    Friend WithEvents lblFax_cc As Label
    Friend WithEvents lblMa_dc As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMa_gd As Label
    Friend WithEvents lblMa_htvc As Label
    Friend WithEvents lblMa_kh As Label
    Friend WithEvents lblMa_nvbh As Label
    Friend WithEvents lblMa_tt As Label
    Friend WithEvents lblNgay_ct As Label
    Friend WithEvents lblNgay_ct0 As Label
    Friend WithEvents lblNgay_ct3 As Label
    Friend WithEvents lblNgay_hd1 As Label
    Friend WithEvents lblNgay_hd2 As Label
    Friend WithEvents lblNgay_lct As Label
    Friend WithEvents lblSo_ct0 As Label
    Friend WithEvents lblSo_hdo As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblStatus_hd As Label
    Friend WithEvents lblStatusMess As Label
    Friend WithEvents lblT_cp As Label
    Friend WithEvents lblT_thue As Label
    Friend WithEvents lblT_tt As Label
    Friend WithEvents lblTen As Label
    Friend WithEvents lblTen_dc As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_gd As Label
    Friend WithEvents lblTen_htvc As Label
    Friend WithEvents lblTen_kh As Label
    Friend WithEvents lblTen_ncc As Label
    Friend WithEvents lblTen_nvbh As Label
    Friend WithEvents lblTen_tt As Label
    Friend WithEvents lblTien_ck As Label
    Friend WithEvents lblTotal As Label
    Friend WithEvents lblTy_gia As Label
    Friend WithEvents tbDetail As TabControl
    Friend WithEvents tbgCharge As TabPage
    Friend WithEvents tbgCust As TabPage
    Friend WithEvents tbgOther As TabPage
    Friend WithEvents tbgOthers As TabPage
    Friend WithEvents tpgDetail As TabPage
    Friend WithEvents txtDia_chi As TextBox
    Friend WithEvents txtDien_giai As TextBox
    Friend WithEvents txtDien_thoai As TextBox
    Friend WithEvents txtFax As TextBox
    Friend WithEvents txtKeyPress As TextBox
    Friend WithEvents txtLoai_ct As TextBox
    Friend WithEvents txtMa_dc As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_gd As TextBox
    Friend WithEvents txtMa_htvc As TextBox
    Friend WithEvents txtMa_kh As TextBox
    Friend WithEvents txtMa_nvbh As TextBox
    Friend WithEvents txtMa_tt As TextBox
    Friend WithEvents txtNgay_ct As txtDate
    Friend WithEvents txtNgay_ct0 As txtDate
    Friend WithEvents txtNgay_ct3 As txtDate
    Friend WithEvents txtNgay_hd1 As txtDate
    Friend WithEvents txtNgay_hd2 As txtDate
    Friend WithEvents txtNgay_lct As txtDate
    Friend WithEvents txtSo_ct As TextBox
    Friend WithEvents txtSo_ct0 As TextBox
    Friend WithEvents txtSo_hd0 As TextBox
    Friend WithEvents txtStatus As TextBox
    Friend WithEvents txtStatus_hd As TextBox
    Friend WithEvents txtStt_rec_hd0 As TextBox
    Friend WithEvents txtT_ck As txtNumeric
    Friend WithEvents txtT_ck_nt As txtNumeric
    Friend WithEvents txtT_cp As txtNumeric
    Friend WithEvents txtT_cp_nt As txtNumeric
    Friend WithEvents txtT_so_luong As txtNumeric
    Friend WithEvents txtT_thue As txtNumeric
    Friend WithEvents txtT_thue_nt As txtNumeric
    Friend WithEvents txtT_tien_nt2 As txtNumeric
    Friend WithEvents txtT_tien2 As txtNumeric
    Friend WithEvents txtT_tt As txtNumeric
    Friend WithEvents txtT_tt_nt As txtNumeric
    Friend WithEvents txtTen_kh0 As TextBox
    Friend WithEvents txtTy_gia As txtNumeric

    Public arrControlButtons(12) As Button
    Public cIDNumber As String
    Private colCk As DataGridTextBoxColumn
    Private colCk_nt As DataGridTextBoxColumn
    Private colCMa_cp As DataGridTextBoxColumn
    Private colCTen_cp As DataGridTextBoxColumn
    Private colCTien_cp As DataGridTextBoxColumn
    Private colCTien_cp_nt As DataGridTextBoxColumn
    Private coldCMa_cp As String
    Public cOldIDNumber As String
    Private cOldItem As String
    Private coldMa_thue As String
    Private cOldSite As String
    Private colDvt As DataGridTextBoxColumn
    Private colGia_ban As DataGridTextBoxColumn
    Private colGia_ban_nt As DataGridTextBoxColumn
    Private colGia_nt2 As DataGridTextBoxColumn
    Private colGia2 As DataGridTextBoxColumn
    Private colMa_kho As DataGridTextBoxColumn
    Private colMa_lo As DataGridTextBoxColumn
    Private colMa_thue As DataGridTextBoxColumn
    Private colMa_vi_tri As DataGridTextBoxColumn
    Private colMa_vt As DataGridTextBoxColumn
    Private colSl_hd As DataGridTextBoxColumn
    Private colSl_xuat As DataGridTextBoxColumn
    Private colSo_luong As DataGridTextBoxColumn
    Private colTen_vt As DataGridTextBoxColumn
    Private colThue As DataGridTextBoxColumn
    Private colThue_nt As DataGridTextBoxColumn
    Private colThue_suat As DataGridTextBoxColumn
    Private colTien_nt2 As DataGridTextBoxColumn
    Private colTien2 As DataGridTextBoxColumn
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
    Private nColumnControl As Integer
    Private noldCk As Decimal
    Private noldCk_nt As Decimal
    Private noldCTien_cp As Decimal
    Private noldCTien_cp_nt As Decimal
    Private nOldECharge As Decimal
    Private noldGia_nt2 As Decimal
    Private noldGia2 As Decimal
    Private noldSo_luong As Decimal
    Private nOldThue As Decimal
    Private nOldThue_nt As Decimal
    Private noldTien_nt2 As Decimal
    Private noldTien2 As Decimal
    Private oInvItemDetail As VoucherLibObj
    Private oldtblDetail As DataTable
    Private oLocation As VoucherKeyLibObj
    Private oLot As VoucherKeyLibObj
    Private oSecurity As clssecurity
    Private oSite As VoucherKeyLibObj
    Private oSOAddress As dirblanklib
    Private oTaxCodeDetail As VoucherLibObj
    'Private oTitleButton As TitleButton
    Private oUOM As VoucherKeyCheckLibObj
    Public oVoucher As clsvoucher.clsVoucher
    Private pn As StatusBarPanel
    Public pnContent As StatusBarPanel
    Private sOldString As String = ""
    Private sOldStringDvt As String = ""
    Private sOldStringMa_kho As String = ""
    Private sOldStringMa_vt As String = ""
    Private sOldStringSo_luong As String = ""
    Private tblHandling As DataTable
    Private tblRetrieveDetail As DataView
    Private tblRetrieveMaster As DataView
    Private tblStatus As DataTable
    Private xInventory As clsInventory
End Class

