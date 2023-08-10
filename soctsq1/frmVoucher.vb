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

Namespace soctsq1
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
            Unit.SetUnit(Me.txtMa_dvcs)
            Me.EDFC()
            Me.cOldIDNumber = Me.cIDNumber
            Me.iOldMasterRow = Me.iMasterRow
            Me.RefreshCharge(0)
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
            Me.oSecurity.SetReadOnly()
            xtabControl.ReadOnlyTabControls(False, Me.tbDetail)
            xtabControl.ScatterMemvarBlankTabControl(Me.tbDetail)
            Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
        End Sub

        Private Sub AfterUpdateSQ(ByVal lcIDNumber As String, ByVal lcAction As String)
            Dim tcSQL As String = String.Concat(New String() {"fs_AfterUpdateSQ '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
            Sql.SQLExecute((modVoucher.appConn), tcSQL)
        End Sub

        Private Sub AllocateBy(ByVal nAmount As Decimal, ByVal nTQ As Decimal, ByVal cQ As String, ByVal cField As String, ByVal nRound As Integer)
            If (Decimal.Compare(nTQ, Decimal.Zero) <> 0) Then
                Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    Dim view2 As DataRowView = modVoucher.tblDetail.Item(i)
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item(cQ))) Then
                        Return
                    End If
                    Dim view As DataRowView = view2
                    Dim str As String = cField
                    Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(nAmount, view2.Item(cQ)), nTQ), nRound}
                    Dim copyBack As Boolean() = New Boolean() {False, True}
                    If copyBack(1) Then
                        nRound = IntegerType.FromObject(args(1))
                    End If
                    view.Item(str) = ObjectType.AddObj(view.Item(str), LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                    view2 = Nothing
                    i += 1
                Loop
            End If
        End Sub

        Private Sub AllocateBy(ByVal nAmount As Decimal, ByVal nTQ As Decimal, ByVal cQ As String, ByVal cField As String, ByVal nRound As Integer, ByVal cQty As String)
            On Error Resume Next
            If (Decimal.Compare(nTQ, Decimal.Zero) = 0) Then
                Return
            End If
            Dim num5 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim num As Integer = 0
            For num = 0 To num5
                Dim view2 As DataRowView = modVoucher.tblDetail.Item(num)
                If (Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item(cQ))) Or Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item(cQty)))) Then
                    Return
                End If
                Dim view As DataRowView = view2
                Dim str As String = cField
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.MulObj(ObjectType.MulObj(nAmount, view2.Item("so_luong")), view2.Item("he_so")), view2.Item(cQ)), nTQ), nRound}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    nRound = IntegerType.FromObject(args(1))
                End If
                view.Item(str) = ObjectType.AddObj(view.Item(str), LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                view2 = Nothing
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
                    Dim view2 As DataRowView = modVoucher.tblDetail.Item(num)
                    view2.Item("cp_vc_nt") = 0
                    view2.Item("cp_bh_nt") = 0
                    view2.Item("cp_khac_nt") = 0
                    view2.Item("cp_vc") = 0
                    view2.Item("cp_bh") = 0
                    view2.Item("cp_khac") = 0
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("so_luong"))) Then
                        view2.Item("so_luong") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("he_so"))) Then
                        view2.Item("he_so") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("volume"))) Then
                        view2.Item("volume") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("weight"))) Then
                        view2.Item("weight") = 0
                    End If
                    num8 = DecimalType.FromObject(ObjectType.AddObj(num8, ObjectType.MulObj(ObjectType.MulObj(view2.Item("volume"), view2.Item("so_luong")), view2.Item("he_so"))))
                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, ObjectType.MulObj(ObjectType.MulObj(view2.Item("weight"), view2.Item("so_luong")), view2.Item("he_so"))))
                    view2 = Nothing
                    num += 1
                Loop
                Dim num10 As Integer = (modVoucher.tblCharge.Count - 1)
                num = 0
                Do While (num <= num10)
                    Dim view As DataRowView = modVoucher.tblCharge.Item(num)
                    If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("ma_cp"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(view.Item("ma_cp"))), "", False) <> 0)) Then
                        Dim str3 As String = ""
                        Dim str4 As String = ""
                        Dim str5 As String = ""
                        Dim str6 As String = ""
                        Dim num5 As Decimal = 0
                        Dim num7 As Decimal = 0
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("tien_cp_nt"))) Then
                            view.Item("tien_cp_nt") = 0
                        End If
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("tien_cp"))) Then
                            view.Item("tien_cp") = 0
                        End If
                        Dim str2 As String = Strings.Trim(StringType.FromObject(view.Item("loai_cp")))
                        Dim str As String = Strings.Trim(StringType.FromObject(view.Item("loai_pb")))
                        Dim nAmount As Decimal = DecimalType.FromObject(view.Item("tien_cp_nt"))
                        Dim num2 As Decimal = DecimalType.FromObject(view.Item("tien_cp"))
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
                    view = Nothing
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
                Dim view2 As DataRowView = modVoucher.tblCharge.Item(num)
                If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("ma_cp"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(view2.Item("ma_cp"))), "", False) <> 0)) Then
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("tien_cp_nt"))) Then
                        view2.Item("tien_cp_nt") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("tien_cp"))) Then
                        view2.Item("tien_cp") = 0
                    End If
                    Dim sLeft As String = Strings.Trim(StringType.FromObject(view2.Item("loai_cp")))
                    If (StringType.StrCmp(sLeft, "1", False) = 0) Then
                        num7 = DecimalType.FromObject(ObjectType.AddObj(num7, view2.Item("tien_cp_nt")))
                        zero = DecimalType.FromObject(ObjectType.AddObj(zero, view2.Item("tien_cp")))
                    ElseIf (StringType.StrCmp(sLeft, "2", False) = 0) Then
                        num3 = DecimalType.FromObject(ObjectType.AddObj(num3, view2.Item("tien_cp_nt")))
                        num2 = DecimalType.FromObject(ObjectType.AddObj(num2, view2.Item("tien_cp")))
                    ElseIf (StringType.StrCmp(sLeft, "3", False) = 0) Then
                        num5 = DecimalType.FromObject(ObjectType.AddObj(num5, view2.Item("tien_cp_nt")))
                        num4 = DecimalType.FromObject(ObjectType.AddObj(num4, view2.Item("tien_cp")))
                    End If
                End If
                view2 = Nothing
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
                Dim view As DataRowView = modVoucher.tblDetail.Item(num)
                view.Item("cp_nt") = ObjectType.AddObj(ObjectType.AddObj(view.Item("cp_vc_nt"), view.Item("cp_bh_nt")), view.Item("cp_khac_nt"))
                view.Item("cp") = ObjectType.AddObj(ObjectType.AddObj(view.Item("cp_vc"), view.Item("cp_bh")), view.Item("cp_khac"))
                view = Nothing
                num += 1
            Loop
            Me.UpdateList()
        End Sub

        Private Sub BeforUpdateSQ(ByVal lcIDNumber As String, ByVal lcAction As String)
            Dim tcSQL As String = String.Concat(New String() {"fs_BeforUpdateSQ '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
            Sql.SQLExecute((modVoucher.appConn), tcSQL)
        End Sub

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
            If Me.oSecurity.GetStatusDelelete Then
                Dim num As Integer
                Dim str4 As String
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
                    str5 = "ctcp20"
                    str4 = ""
                Else
                    str5 = (Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & ", ctcp20")
                    str4 = GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)
                End If
                Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(str5, ","c))
                num = 1
                Do While (num <= num3)
                    Dim cTable As String = Strings.Trim(Fox.GetWordNum(str5, num, ","c))
                    str4 = (str4 & ChrW(13) & GenSQLDelete(cTable, cKey))
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
                    str4 = ((String.Concat(New String() {str4, ChrW(13), "UPDATE ", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), " SET Status = '*'"}) & ", datetime2 = GETDATE(), user_id2 = " & StringType.FromObject(Reg.GetRegistryKey("CurrUserId"))) & "  WHERE " & cKey)
                End If
                Me.BeforUpdateSQ(lcIDNumber, "Del")
                Sql.SQLExecute((modVoucher.appConn), str4)
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

        Private Sub DeleteItemCharge(ByVal sender As Object, ByVal e As EventArgs)
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
                Dim currentRowIndex As Integer = Me.grdCharge.CurrentRowIndex
                If ((((currentRowIndex >= 0) And (currentRowIndex < modVoucher.tblCharge.Count)) AndAlso Not Me.grdCharge.EndEdit(Me.grdCharge.TableStyles.Item(0).GridColumnStyles.Item(Me.grdCharge.CurrentCell.ColumnNumber), currentRowIndex, False)) AndAlso (ObjectType.ObjTst(Msg.Question(StringType.FromObject(modVoucher.oVar.Item("m_sure_dele")), 1), 1, False) = 0)) Then
                    Me.grdCharge.Select(currentRowIndex)
                    Dim view As DataRowView = modVoucher.tblCharge.Item(currentRowIndex)
                    AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), False)
                    view.Delete()
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
                ChangeFormatColumn(Me.colCTien_cp_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
                ChangeFormatColumn(Me.colThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
                Me.colTien_nt2.HeaderText = StringType.FromObject(modVoucher.oLan.Item("018"))
                Me.colGia_ban_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("021"))
                Me.colCk_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("028"))
                Me.colGia_nt2.HeaderText = StringType.FromObject(modVoucher.oLan.Item("032"))
                Me.colCTien_cp_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("018"))
                Me.colThue_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("017"))

                Me.txtT_tien_nt2.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
                Me.txtT_cp_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
                Me.txtT_ck_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
                Me.txtT_tien_nt2.Value = Me.txtT_tien_nt2.Value
                Me.txtT_ck_nt.Value = Me.txtT_ck_nt.Value
                Me.txtT_cp_nt.Value = Me.txtT_cp_nt.Value
                Try
                    Me.colTien2.MappingName = "H1"
                    Me.colGia2.MappingName = "H4"
                    Me.colCk.MappingName = "H6"
                    Me.colGia_ban.MappingName = "H7"
                    Me.colThue.MappingName = "H8"
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
                Me.txtT_ck.Visible = False
                Me.txtT_cp.Visible = False
            Else
                Me.txtTy_gia.Enabled = True
                ChangeFormatColumn(Me.colGia_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
                ChangeFormatColumn(Me.colTien_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
                ChangeFormatColumn(Me.colCk_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
                ChangeFormatColumn(Me.colCTien_cp_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
                ChangeFormatColumn(Me.colThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
                Me.colTien_nt2.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("019")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
                Me.colGia_nt2.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("033")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
                Me.colGia_ban_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("023")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
                Me.colCk_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("031")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
                Me.colCTien_cp_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("019")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
                Me.colThue_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("026")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
                Me.txtT_tien_nt2.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
                Me.txtT_ck_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
                Me.txtT_cp_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
                Me.txtT_tien_nt2.Value = Me.txtT_tien_nt2.Value
                Me.txtT_ck_nt.Value = Me.txtT_ck_nt.Value
                Me.txtT_cp_nt.Value = Me.txtT_cp_nt.Value
                Try
                    Me.colTien2.MappingName = "tien2"
                    Me.colGia2.MappingName = "gia2"
                    Me.colCk.MappingName = "ck"
                    Me.colGia_ban.MappingName = "gia_ban"
                    Me.colThue.MappingName = "thue"
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
                Me.txtMa_kh.Focus()
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
            Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
        End Sub

        Private Sub EditAllocatedCharge(ByVal sender As Object, ByVal e As EventArgs)
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
                Me.frmView = New Form
                Me.grdMV = New gridformtran
                Dim tbs As New DataGridTableStyle
                Dim style As New DataGridTableStyle
                Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(modVoucher.MaxColumns) {}
                Dim index As Integer = 0
                Do
                    cols(index) = New DataGridTextBoxColumn
                    If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                        cols(index).NullText = StringType.FromInteger(0)
                    Else
                        cols(index).NullText = ""
                    End If
                    index += 1
                Loop While (index < modVoucher.MaxColumns)
                Dim frmView As Form = Me.frmView
                frmView.Top = 0
                frmView.Left = 0
                frmView.Width = Me.Width
                frmView.Height = Me.Height
                frmView.Text = StringType.FromObject(modVoucher.oLan.Item("203"))
                frmView.StartPosition = FormStartPosition.CenterParent
                Me.pn = AddStb(Me.frmView)
                frmView = Nothing
                Dim grdMV As gridformtran = Me.grdMV
                grdMV.CaptionVisible = False
                grdMV.ReadOnly = False
                grdMV.Top = 0
                grdMV.Left = 0
                grdMV.Height = ((Me.Height - 60) - SystemInformation.CaptionHeight)
                grdMV.Width = Me.Width
                grdMV.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
                grdMV.BackgroundColor = Color.White
                grdMV = Nothing
                Me.frmView.Controls.Add(Me.grdMV)
                Dim grdFill As DataGrid = Me.grdMV
                Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), (grdFill), (tbs), (cols), "SOECharge")
                Me.grdMV = DirectCast(grdFill, gridformtran)
                index = 0
                Do
                    If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                        cols(index).NullText = StringType.FromInteger(0)
                    Else
                        cols(index).NullText = ""
                    End If
                    cols(index).TextBox.Enabled = ((index >= 2) And (index <= 7))
                    index += 1
                Loop While (index < modVoucher.MaxColumns)
                Me.AddEChargeHandler()
                Me.pn.Text = ""
                Obj.Init(Me.frmView)
                Dim button2 As New Button
                Dim button As New Button
                Dim button4 As Button = button2
                button4.Top = ((Me.Height - SystemInformation.CaptionHeight) - &H37)
                button4.Left = 0
                button4.Visible = True
                button4.Text = StringType.FromObject(modVoucher.oLan.Item("038"))
                button4.Width = &H4B
                button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                button4.DialogResult = DialogResult.OK
                button4 = Nothing
                Dim button3 As Button = button
                button3.Top = button2.Top
                button3.Left = ((button2.Left + button2.Width) + 1)
                button3.Visible = True
                button3.Text = StringType.FromObject(modVoucher.oLan.Item("039"))
                button3.Width = button2.Width
                button3.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                button3.Enabled = True
                button3.DialogResult = DialogResult.Cancel
                button3 = Nothing
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
                        Dim view As DataRowView = modVoucher.tblDetail.Item(index)
                        view.Item("cp_nt") = ObjectType.AddObj(ObjectType.AddObj(view.Item("cp_vc_nt"), view.Item("cp_bh_nt")), view.Item("cp_khac_nt"))
                        view.Item("cp") = ObjectType.AddObj(ObjectType.AddObj(view.Item("cp_vc"), view.Item("cp_bh")), view.Item("cp_khac"))
                        view = Nothing
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
            On Error Resume Next
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
            Loop While (index < modVoucher.MaxColumns)
            Try
                Me.colTen_vt.TextBox.Enabled = False
                Me.colThue_suat.TextBox.Enabled = False
                Me.colGia_ban_nt.TextBox.Enabled = False
                Me.colGia_ban.TextBox.Enabled = False
                Me.colCTen_cp.TextBox.Enabled = False
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
            Loop While (index < modVoucher.MaxColumns)
            Try
                Me.colCTen_cp.TextBox.Enabled = False
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Me.EDStatus(lED)
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
            oVoucher.Init
            Me.txtNgay_lct.AddCalenderControl()
            Dim lib5 As New DirLib(Me.txtMa_dvcs, Me.lblTen_dvcs, modVoucher.sysConn, modVoucher.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", False, Me.cmdEdit)
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
            Dim grdDetail As DataGrid = Me.grdDetail
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), (grdDetail), (modVoucher.tbsDetail), (modVoucher.tbcDetail), "SQDetail")
            Me.grdDetail = DirectCast(grdDetail, clsgrid)
            oVoucher.SetMaxlengthItem(Me.grdDetail, modVoucher.alDetail, modVoucher.sysConn)
            Me.grdDetail.dvGrid = modVoucher.tblDetail
            Me.grdDetail.cFieldKey = "ma_vt"
            Me.grdDetail.AllowSorting = False
            Me.grdDetail.TableStyles.Item(0).AllowSorting = False
            Me.colMa_vt = GetColumn(Me.grdDetail, "ma_vt")
            Me.colDvt = GetColumn(Me.grdDetail, "Dvt")
            Me.colMa_kho = GetColumn(Me.grdDetail, "ma_kho")
            Me.colSo_luong = GetColumn(Me.grdDetail, "so_luong")
            Me.colGia2 = GetColumn(Me.grdDetail, "gia2")
            Me.colGia_nt2 = GetColumn(Me.grdDetail, "gia_nt2")
            Me.colTien2 = GetColumn(Me.grdDetail, "tien2")
            Me.colTien_nt2 = GetColumn(Me.grdDetail, "tien_nt2")
            Me.colCk = GetColumn(Me.grdDetail, "ck")
            Me.colCk_nt = GetColumn(Me.grdDetail, "ck_nt")
            Me.colTen_vt = GetColumn(Me.grdDetail, "ten_vt")
            Me.colThue_suat = GetColumn(Me.grdDetail, "thue_suat")
            Me.colGia_ban_nt = GetColumn(Me.grdDetail, "gia_ban_nt")
            Me.colGia_ban = GetColumn(Me.grdDetail, "gia_ban")
            Me.colThue_nt = GetColumn(Me.grdDetail, "thue_nt")
            Me.colThue = GetColumn(Me.grdDetail, "thue")
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
            Dim oCustomer As New DirLib(Me.txtMa_kh, Me.lblTen_kh, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", sKey, False, Me.cmdEdit)
            AddHandler Me.txtMa_kh.Validated, New EventHandler(AddressOf Me.txtMa_kh_valid)
            AddHandler Me.txtMa_dc.Enter, New EventHandler(AddressOf Me.txtMa_dc_Enter)
            Dim clscustomerref As New clscustomerref(modVoucher.appConn, Me.txtMa_kh, Me.txtOng_ba, modVoucher.VoucherCode, Me.oVoucher)
            Me.oInvItemDetail = New VoucherLibObj(Me.colMa_vt, "ten_vt", modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
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
            Me.colTen_vt.TextBox.Enabled = False
            Me.colThue_suat.TextBox.Enabled = False
            Me.colGia_ban_nt.TextBox.Enabled = False
            Me.colGia_ban.TextBox.Enabled = False
            oVoucher.HideFields(Me.grdDetail)
            ChangeFormatColumn(Me.colSo_luong, StringType.FromObject(modVoucher.oVar.Item("m_ip_sl")))
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
            Loop While (index < modVoucher.MaxColumns)
            Dim menu As New ContextMenu
            Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItem), Shortcut.F4)
            Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItem), Shortcut.F8)
            menu.MenuItems.Add(item)
            menu.MenuItems.Add(New MenuItem("-"))
            menu.MenuItems.Add(item2)
            Dim menu2 As New ContextMenu
            Dim item3 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("043")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F5)
            menu2.MenuItems.Add(item3)
            Me.ContextMenu = menu2
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

        Private Sub grdCharge_CurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
            On Error Resume Next
            If Not Me.lAllowCurrentCellChanged Then
                Return
            End If
            Dim grdCharge As clsgrid = Me.grdCharge
            Dim currentRowIndex As Integer = grdCharge.CurrentRowIndex
            Dim columnNumber As Integer = grdCharge.CurrentCell.ColumnNumber
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
            grdCharge = Nothing
        End Sub

        Private Sub grdDetail_CurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
            On Error Resume Next
            If Not Me.lAllowCurrentCellChanged Then
                Return
            End If
            Dim grdDetail As clsgrid = Me.grdDetail
            Dim currentRowIndex As Integer = grdDetail.CurrentRowIndex
            Dim columnNumber As Integer = grdDetail.CurrentCell.ColumnNumber
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
            ElseIf (StringType.StrCmp(sLeft, "THUE_NT", False) = 0) Then
                Me.nOldThue_nt = DecimalType.FromObject(oValue)
            ElseIf (StringType.StrCmp(sLeft, "THUE", False) = 0) Then
                Me.nOldThue = DecimalType.FromObject(oValue)
            End If
            grdDetail = Nothing
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

        Private Sub IniTax()
            Me.colMa_thue = GetColumn(Me.grdDetail, "Ma_thue")
            Me.oTaxCodeDetail = New VoucherLibObj(Me.colMa_thue, "ten_thue", modVoucher.sysConn, modVoucher.appConn, "dmthue", "ma_thue", "ten_thue", "Tax", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
            AddHandler Me.colMa_thue.TextBox.Validated, New EventHandler(AddressOf Me.txtMa_thue_valid)
            AddHandler Me.colMa_thue.TextBox.Enter, New EventHandler(AddressOf Me.txtMa_thue_enter)
            AddHandler Me.colThue_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtThue_nt_valid)
            AddHandler Me.colThue.TextBox.Leave, New EventHandler(AddressOf Me.txtThue_valid)
            'AddHandler Me.colThue_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtThue_nt_enter)
            'AddHandler Me.colThue.TextBox.Enter, New EventHandler(AddressOf Me.txtThue_enter)
            AddHandler Me.colThue_nt.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneTax)
            AddHandler Me.colThue.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneTax)
        End Sub
        'Private Sub txtThue_enter(ByVal sender As Object, ByVal e As EventArgs)
        '    Me.nOldThue = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        'End Sub

        'Private Sub txtThue_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        '    Me.nOldThue_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        'End Sub

        Private Sub txtThue_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
            Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
            Dim num3 As Decimal = Me.nOldThue_nt
            Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
            If (Decimal.Compare(num, num3) <> 0) Then
                modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue_nt") = num
                Dim args As Object() = New Object() {ObjectType.MulObj(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("Thue_nt"), Me.txtTy_gia.Value), num2}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(args(1))
                End If
                modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
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
        Private Sub WhenNoneTax(ByVal sender As Object, ByVal e As EventArgs)
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue"))) Then
                Me.grdDetail.TabProcess()
            Else
                If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue"))), "", False) = 0) Then
                    Me.grdDetail.TabProcess()
                End If
            End If
        End Sub

        Private Sub InitCharge()
            Dim grdCharge As DataGrid = Me.grdCharge
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblCharge), (grdCharge), (modVoucher.tbsCharge), (modVoucher.tbcCharge), "SQCharge")
            Me.grdCharge = DirectCast(grdCharge, clsgrid)
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
            Loop While (index < modVoucher.MaxColumns)
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

        <DebuggerStepThrough>
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
            Me.tbDetail = New TabControl
            Me.tpgDetail = New TabPage
            Me.grdDetail = New clsgrid
            Me.tbgCharge = New TabPage
            Me.grdCharge = New clsgrid
            Me.tbgOthers = New TabPage
            Me.txtMa_htvc = New TextBox
            Me.lblMa_htvc = New Label
            Me.lblTen_htvc = New Label
            Me.lblDia_chi = New Label
            Me.lblTen_dc = New Label
            Me.txtMa_dc = New TextBox
            Me.lblMa_dc = New Label
            Me.txtT_tien2 = New txtNumeric
            Me.txtT_ck = New txtNumeric
            Me.txtT_ck_nt = New txtNumeric
            Me.txtT_tien_nt2 = New txtNumeric
            Me.txtStatus = New TextBox
            Me.lblStatus = New Label
            Me.lblStatusMess = New Label
            Me.txtKeyPress = New TextBox
            Me.cboStatus = New ComboBox
            Me.cboAction = New ComboBox
            Me.lblAction = New Label
            Me.lblMa_kh = New Label
            Me.txtMa_kh = New TextBox
            Me.lblTen_kh = New Label
            Me.lblOng_ba = New Label
            Me.txtOng_ba = New TextBox
            Me.lblTotal = New Label
            Me.lblTien_ck = New Label
            Me.lblMa_tt = New Label
            Me.txtMa_tt = New TextBox
            Me.lblTen_tt = New Label
            Me.lblTen = New Label
            Me.txtNgay_hl = New txtDate
            Me.lblNgay_hl = New Label
            Me.txtDien_giai = New TextBox
            Me.Label1 = New Label
            Me.lvlT_cp = New Label
            Me.txtT_cp_nt = New txtNumeric
            Me.txtT_cp = New txtNumeric
            Me.txtT_so_luong = New txtNumeric
            Me.txtLoai_ct = New TextBox
            Me.tbDetail.SuspendLayout()
            Me.tpgDetail.SuspendLayout()
            Me.grdDetail.BeginInit()
            Me.tbgCharge.SuspendLayout()
            Me.grdCharge.BeginInit()
            Me.tbgOthers.SuspendLayout()
            Me.SuspendLayout()
            Me.cmdSave.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdSave.BackColor = SystemColors.Control
            Me.cmdSave.Location = New Point(2, 428)
            Me.cmdSave.Name = "cmdSave"
            Me.cmdSave.Size = New Size(60, 23)
            Me.cmdSave.TabIndex = 21
            Me.cmdSave.Tag = "CB01"
            Me.cmdSave.Text = "Luu"
            Me.cmdNew.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdNew.BackColor = SystemColors.Control
            Me.cmdNew.Location = New Point(62, 428)
            Me.cmdNew.Name = "cmdNew"
            Me.cmdNew.Size = New Size(60, 23)
            Me.cmdNew.TabIndex = 22
            Me.cmdNew.Tag = "CB02"
            Me.cmdNew.Text = "Moi"
            Me.cmdPrint.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdPrint.BackColor = SystemColors.Control
            Me.cmdPrint.Location = New Point(122, 428)
            Me.cmdPrint.Name = "cmdPrint"
            Me.cmdPrint.Size = New Size(60, 23)
            Me.cmdPrint.TabIndex = 23
            Me.cmdPrint.Tag = "CB03"
            Me.cmdPrint.Text = "In ctu"
            Me.cmdEdit.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdEdit.BackColor = SystemColors.Control
            Me.cmdEdit.Location = New Point(182, 428)
            Me.cmdEdit.Name = "cmdEdit"
            Me.cmdEdit.Size = New Size(60, 23)
            Me.cmdEdit.TabIndex = 24
            Me.cmdEdit.Tag = "CB04"
            Me.cmdEdit.Text = "Sua"
            Me.cmdDelete.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdDelete.BackColor = SystemColors.Control
            Me.cmdDelete.Location = New Point(242, 428)
            Me.cmdDelete.Name = "cmdDelete"
            Me.cmdDelete.Size = New Size(60, 23)
            Me.cmdDelete.TabIndex = 25
            Me.cmdDelete.Tag = "CB05"
            Me.cmdDelete.Text = "Xoa"
            Me.cmdView.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdView.BackColor = SystemColors.Control
            Me.cmdView.Location = New Point(302, 428)
            Me.cmdView.Name = "cmdView"
            Me.cmdView.Size = New Size(60, 23)
            Me.cmdView.TabIndex = 26
            Me.cmdView.Tag = "CB06"
            Me.cmdView.Text = "Xem"
            Me.cmdSearch.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdSearch.BackColor = SystemColors.Control
            Me.cmdSearch.Location = New Point(362, 428)
            Me.cmdSearch.Name = "cmdSearch"
            Me.cmdSearch.Size = New Size(60, 23)
            Me.cmdSearch.TabIndex = 27
            Me.cmdSearch.Tag = "CB07"
            Me.cmdSearch.Text = "Tim"
            Me.cmdClose.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdClose.BackColor = SystemColors.Control
            Me.cmdClose.Location = New Point(422, 428)
            Me.cmdClose.Name = "cmdClose"
            Me.cmdClose.Size = New Size(60, 23)
            Me.cmdClose.TabIndex = 28
            Me.cmdClose.Tag = "CB08"
            Me.cmdClose.Text = "Quay ra"
            Me.cmdOption.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.cmdOption.BackColor = SystemColors.Control
            Me.cmdOption.Location = New Point(543, 428)
            Me.cmdOption.Name = "cmdOption"
            Me.cmdOption.Size = New Size(20, 23)
            Me.cmdOption.TabIndex = 29
            Me.cmdOption.TabStop = False
            Me.cmdOption.Tag = "CB09"
            Me.cmdTop.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.cmdTop.BackColor = SystemColors.Control
            Me.cmdTop.Location = New Point(562, 428)
            Me.cmdTop.Name = "cmdTop"
            Me.cmdTop.Size = New Size(20, 23)
            Me.cmdTop.TabIndex = 30
            Me.cmdTop.TabStop = False
            Me.cmdTop.Tag = "CB10"
            Me.cmdPrev.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.cmdPrev.BackColor = SystemColors.Control
            Me.cmdPrev.Location = New Point(581, 428)
            Me.cmdPrev.Name = "cmdPrev"
            Me.cmdPrev.Size = New Size(20, 23)
            Me.cmdPrev.TabIndex = 31
            Me.cmdPrev.TabStop = False
            Me.cmdPrev.Tag = "CB11"
            Me.cmdNext.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.cmdNext.BackColor = SystemColors.Control
            Me.cmdNext.Location = New Point(600, 428)
            Me.cmdNext.Name = "cmdNext"
            Me.cmdNext.Size = New Size(20, 23)
            Me.cmdNext.TabIndex = 32
            Me.cmdNext.TabStop = False
            Me.cmdNext.Tag = "CB12"
            Me.cmdBottom.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.cmdBottom.BackColor = SystemColors.Control
            Me.cmdBottom.Location = New Point(619, 428)
            Me.cmdBottom.Name = "cmdBottom"
            Me.cmdBottom.Size = New Size(20, 23)
            Me.cmdBottom.TabIndex = 33
            Me.cmdBottom.TabStop = False
            Me.cmdBottom.Tag = "CB13"
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New Point(272, 456)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New Size(46, 16)
            Me.lblMa_dvcs.TabIndex = 13
            Me.lblMa_dvcs.Tag = "L001"
            Me.lblMa_dvcs.Text = "Ma dvcs"
            Me.lblMa_dvcs.Visible = False
            Me.txtMa_dvcs.BackColor = Color.White
            Me.txtMa_dvcs.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New Point(320, 456)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.TabIndex = 0
            Me.txtMa_dvcs.Tag = "FCNBCF"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            Me.txtMa_dvcs.Visible = False
            Me.lblTen_dvcs.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Top))
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New Point(424, 456)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New Size(87, 16)
            Me.lblTen_dvcs.TabIndex = 15
            Me.lblTen_dvcs.Tag = "FCRF"
            Me.lblTen_dvcs.Text = "Ten don vi co so"
            Me.lblTen_dvcs.Visible = False
            Me.lblSo_ct.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            Me.lblSo_ct.AutoSize = True
            Me.lblSo_ct.Location = New Point(438, 7)
            Me.lblSo_ct.Name = "lblSo_ct"
            Me.lblSo_ct.Size = New Size(36, 16)
            Me.lblSo_ct.TabIndex = 16
            Me.lblSo_ct.Tag = "L009"
            Me.lblSo_ct.Text = "So ctu"
            Me.txtSo_ct.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            Me.txtSo_ct.BackColor = Color.White
            Me.txtSo_ct.CharacterCasing = CharacterCasing.Upper
            Me.txtSo_ct.Location = New Point(538, 5)
            Me.txtSo_ct.Name = "txtSo_ct"
            Me.txtSo_ct.TabIndex = 6
            Me.txtSo_ct.Tag = "FCNBCF"
            Me.txtSo_ct.Text = "TXTSO_CT"
            Me.txtSo_ct.TextAlign = HorizontalAlignment.Right
            Me.txtNgay_lct.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            Me.txtNgay_lct.BackColor = Color.White
            Me.txtNgay_lct.Location = New Point(538, 26)
            Me.txtNgay_lct.MaxLength = 10
            Me.txtNgay_lct.Name = "txtNgay_lct"
            Me.txtNgay_lct.TabIndex = 7
            Me.txtNgay_lct.Tag = "FDNBCFDF"
            Me.txtNgay_lct.Text = "  /  /    "
            Me.txtNgay_lct.TextAlign = HorizontalAlignment.Right
            Me.txtNgay_lct.Value = New DateTime(0)
            Me.txtTy_gia.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            Me.txtTy_gia.BackColor = Color.White
            Me.txtTy_gia.Format = "m_ip_tg"
            Me.txtTy_gia.Location = New Point(538, 47)
            Me.txtTy_gia.MaxLength = 8
            Me.txtTy_gia.Name = "txtTy_gia"
            Me.txtTy_gia.TabIndex = 9
            Me.txtTy_gia.Tag = "FNCF"
            Me.txtTy_gia.Text = "m_ip_tg"
            Me.txtTy_gia.TextAlign = HorizontalAlignment.Right
            Me.txtTy_gia.Value = 0
            Me.lblNgay_lct.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            Me.lblNgay_lct.AutoSize = True
            Me.lblNgay_lct.Location = New Point(438, 28)
            Me.lblNgay_lct.Name = "lblNgay_lct"
            Me.lblNgay_lct.Size = New Size(61, 16)
            Me.lblNgay_lct.TabIndex = 20
            Me.lblNgay_lct.Tag = "L010"
            Me.lblNgay_lct.Text = "Ngay lap ct"
            Me.lblNgay_ct.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            Me.lblNgay_ct.AutoSize = True
            Me.lblNgay_ct.Location = New Point(40, 456)
            Me.lblNgay_ct.Name = "lblNgay_ct"
            Me.lblNgay_ct.Size = New Size(83, 16)
            Me.lblNgay_ct.TabIndex = 21
            Me.lblNgay_ct.Tag = "L011"
            Me.lblNgay_ct.Text = "Ngay hach toan"
            Me.lblNgay_ct.Visible = False
            Me.lblTy_gia.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            Me.lblTy_gia.AutoSize = True
            Me.lblTy_gia.Location = New Point(438, 49)
            Me.lblTy_gia.Name = "lblTy_gia"
            Me.lblTy_gia.Size = New Size(35, 16)
            Me.lblTy_gia.TabIndex = 22
            Me.lblTy_gia.Tag = "L012"
            Me.lblTy_gia.Text = "Ty gia"
            Me.txtNgay_ct.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            Me.txtNgay_ct.BackColor = Color.White
            Me.txtNgay_ct.Location = New Point(136, 454)
            Me.txtNgay_ct.MaxLength = 10
            Me.txtNgay_ct.Name = "txtNgay_ct"
            Me.txtNgay_ct.TabIndex = 10
            Me.txtNgay_ct.Tag = "FDNBCFDF"
            Me.txtNgay_ct.Text = "  /  /    "
            Me.txtNgay_ct.TextAlign = HorizontalAlignment.Right
            Me.txtNgay_ct.Value = New DateTime(0)
            Me.txtNgay_ct.Visible = False
            Me.cmdMa_nt.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            Me.cmdMa_nt.BackColor = SystemColors.Control
            Me.cmdMa_nt.Enabled = False
            Me.cmdMa_nt.Location = New Point(498, 47)
            Me.cmdMa_nt.Name = "cmdMa_nt"
            Me.cmdMa_nt.Size = New Size(36, 20)
            Me.cmdMa_nt.TabIndex = 8
            Me.cmdMa_nt.TabStop = False
            Me.cmdMa_nt.Tag = "FCCFCMDDF"
            Me.cmdMa_nt.Text = "VND"
            Me.tbDetail.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.tbDetail.Controls.Add(Me.tpgDetail)
            Me.tbDetail.Controls.Add(Me.tbgCharge)
            Me.tbDetail.Controls.Add(Me.tbgOthers)
            Me.tbDetail.Location = New Point(2, 121)
            Me.tbDetail.Name = "tbDetail"
            Me.tbDetail.SelectedIndex = 0
            Me.tbDetail.Size = New Size(638, 233)
            Me.tbDetail.TabIndex = 13
            Me.tpgDetail.BackColor = SystemColors.Control
            Me.tpgDetail.Controls.Add(Me.grdDetail)
            Me.tpgDetail.Location = New Point(4, 22)
            Me.tpgDetail.Name = "tpgDetail"
            Me.tpgDetail.Size = New Size(630, 207)
            Me.tpgDetail.TabIndex = 0
            Me.tpgDetail.Tag = "L016"
            Me.tpgDetail.Text = "Chung tu"
            Me.grdDetail.Cell_EnableRaisingEvents = False
            Me.grdDetail.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.grdDetail.BackgroundColor = Color.White
            Me.grdDetail.CaptionBackColor = SystemColors.Control
            Me.grdDetail.CaptionFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, 0)
            Me.grdDetail.CaptionForeColor = Color.Black
            Me.grdDetail.CaptionText = "F4 - Them, F8 - Xoa"
            Me.grdDetail.DataMember = ""
            Me.grdDetail.HeaderForeColor = SystemColors.ControlText
            Me.grdDetail.Location = New Point(-1, -1)
            Me.grdDetail.Name = "grdDetail"
            Me.grdDetail.Size = New Size(633, 208)
            Me.grdDetail.TabIndex = 0
            Me.grdDetail.Tag = "L020CF"
            Me.tbgCharge.Controls.Add(Me.grdCharge)
            Me.tbgCharge.Location = New Point(4, 22)
            Me.tbgCharge.Name = "tbgCharge"
            Me.tbgCharge.Size = New Size(630, 207)
            Me.tbgCharge.TabIndex = 2
            Me.tbgCharge.Tag = "L034"
            Me.tbgCharge.Text = "Chi phi"
            Me.grdCharge.Cell_EnableRaisingEvents = False
            Me.grdCharge.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.grdCharge.BackgroundColor = Color.White
            Me.grdCharge.CaptionBackColor = SystemColors.Control
            Me.grdCharge.CaptionFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, 0)
            Me.grdCharge.CaptionForeColor = Color.Black
            Me.grdCharge.CaptionText = "Nhap chi phi: F4 - Them dong, F8 - Xoa dong"
            Me.grdCharge.DataMember = ""
            Me.grdCharge.HeaderForeColor = SystemColors.ControlText
            Me.grdCharge.Location = New Point(-1, -1)
            Me.grdCharge.Name = "grdCharge"
            Me.grdCharge.Size = New Size(633, 208)
            Me.grdCharge.TabIndex = 1
            Me.grdCharge.Tag = "L035"
            Me.tbgOthers.Controls.Add(Me.txtMa_htvc)
            Me.tbgOthers.Controls.Add(Me.lblMa_htvc)
            Me.tbgOthers.Controls.Add(Me.lblTen_htvc)
            Me.tbgOthers.Controls.Add(Me.lblDia_chi)
            Me.tbgOthers.Controls.Add(Me.lblTen_dc)
            Me.tbgOthers.Controls.Add(Me.txtMa_dc)
            Me.tbgOthers.Controls.Add(Me.lblMa_dc)
            Me.tbgOthers.Location = New Point(4, 22)
            Me.tbgOthers.Name = "tbgOthers"
            Me.tbgOthers.Size = New Size(630, 207)
            Me.tbgOthers.TabIndex = 3
            Me.tbgOthers.Tag = "L004"
            Me.tbgOthers.Text = "Thong tin giao hang"
            Me.txtMa_htvc.BackColor = Color.White
            Me.txtMa_htvc.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_htvc.Location = New Point(88, 26)
            Me.txtMa_htvc.Name = "txtMa_htvc"
            Me.txtMa_htvc.Size = New Size(80, 20)
            Me.txtMa_htvc.TabIndex = 111
            Me.txtMa_htvc.Tag = "FCCF"
            Me.txtMa_htvc.Text = "TXTMA_HTVC"
            Me.lblMa_htvc.AutoSize = True
            Me.lblMa_htvc.Location = New Point(2, 28)
            Me.lblMa_htvc.Name = "lblMa_htvc"
            Me.lblMa_htvc.Size = New Size(66, 16)
            Me.lblMa_htvc.TabIndex = 115
            Me.lblMa_htvc.Tag = "L006"
            Me.lblMa_htvc.Text = "Hinh thuc vc"
            Me.lblTen_htvc.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Top))
            Me.lblTen_htvc.AutoSize = True
            Me.lblTen_htvc.Location = New Point(175, 28)
            Me.lblTen_htvc.Name = "lblTen_htvc"
            Me.lblTen_htvc.Size = New Size(133, 16)
            Me.lblTen_htvc.TabIndex = 116
            Me.lblTen_htvc.Tag = "FCRF"
            Me.lblTen_htvc.Text = "Ten hinh thuc van chuyen"
            Me.lblDia_chi.AutoSize = True
            Me.lblDia_chi.Location = New Point(175, 7)
            Me.lblDia_chi.Name = "lblDia_chi"
            Me.lblDia_chi.Size = New Size(39, 16)
            Me.lblDia_chi.TabIndex = 113
            Me.lblDia_chi.Tag = "L015"
            Me.lblDia_chi.Text = "Dia chi"
            Me.lblTen_dc.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Top))
            Me.lblTen_dc.AutoSize = True
            Me.lblTen_dc.Location = New Point(240, 8)
            Me.lblTen_dc.Name = "lblTen_dc"
            Me.lblTen_dc.Size = New Size(70, 16)
            Me.lblTen_dc.TabIndex = 114
            Me.lblTen_dc.Tag = "FCRF"
            Me.lblTen_dc.Text = "Ten noi nhan"
            Me.txtMa_dc.BackColor = Color.White
            Me.txtMa_dc.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_dc.Location = New Point(88, 5)
            Me.txtMa_dc.Name = "txtMa_dc"
            Me.txtMa_dc.Size = New Size(80, 20)
            Me.txtMa_dc.TabIndex = 110
            Me.txtMa_dc.Tag = "FCCF"
            Me.txtMa_dc.Text = "TXTMA_DC"
            Me.lblMa_dc.AutoSize = True
            Me.lblMa_dc.Location = New Point(2, 7)
            Me.lblMa_dc.Name = "lblMa_dc"
            Me.lblMa_dc.Size = New Size(49, 16)
            Me.lblMa_dc.TabIndex = 112
            Me.lblMa_dc.Tag = "L005"
            Me.lblMa_dc.Text = "Noi nhan"
            Me.txtT_tien2.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.txtT_tien2.BackColor = Color.White
            Me.txtT_tien2.Enabled = False
            Me.txtT_tien2.ForeColor = Color.Black
            Me.txtT_tien2.Format = "m_ip_tien"
            Me.txtT_tien2.Location = New Point(538, 359)
            Me.txtT_tien2.MaxLength = 10
            Me.txtT_tien2.Name = "txtT_tien2"
            Me.txtT_tien2.TabIndex = 16
            Me.txtT_tien2.Tag = "FN"
            Me.txtT_tien2.Text = "m_ip_tien"
            Me.txtT_tien2.TextAlign = HorizontalAlignment.Right
            Me.txtT_tien2.Value = 0
            Me.txtT_ck.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.txtT_ck.BackColor = Color.White
            Me.txtT_ck.Enabled = False
            Me.txtT_ck.ForeColor = Color.Black
            Me.txtT_ck.Format = "m_ip_tien"
            Me.txtT_ck.Location = New Point(538, 401)
            Me.txtT_ck.MaxLength = 10
            Me.txtT_ck.Name = "txtT_ck"
            Me.txtT_ck.TabIndex = 20
            Me.txtT_ck.Tag = "FN"
            Me.txtT_ck.Text = "m_ip_tien"
            Me.txtT_ck.TextAlign = HorizontalAlignment.Right
            Me.txtT_ck.Value = 0
            Me.txtT_ck_nt.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.txtT_ck_nt.BackColor = Color.White
            Me.txtT_ck_nt.Enabled = False
            Me.txtT_ck_nt.ForeColor = Color.Black
            Me.txtT_ck_nt.Format = "m_ip_tien_nt"
            Me.txtT_ck_nt.Location = New Point(437, 401)
            Me.txtT_ck_nt.MaxLength = 13
            Me.txtT_ck_nt.Name = "txtT_ck_nt"
            Me.txtT_ck_nt.TabIndex = 19
            Me.txtT_ck_nt.Tag = "FN"
            Me.txtT_ck_nt.Text = "m_ip_tien_nt"
            Me.txtT_ck_nt.TextAlign = HorizontalAlignment.Right
            Me.txtT_ck_nt.Value = 0
            Me.txtT_tien_nt2.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.txtT_tien_nt2.BackColor = Color.White
            Me.txtT_tien_nt2.Enabled = False
            Me.txtT_tien_nt2.ForeColor = Color.Black
            Me.txtT_tien_nt2.Format = "m_ip_tien_nt"
            Me.txtT_tien_nt2.Location = New Point(437, 359)
            Me.txtT_tien_nt2.MaxLength = 13
            Me.txtT_tien_nt2.Name = "txtT_tien_nt2"
            Me.txtT_tien_nt2.TabIndex = 15
            Me.txtT_tien_nt2.Tag = "FN"
            Me.txtT_tien_nt2.Text = "m_ip_tien_nt"
            Me.txtT_tien_nt2.TextAlign = HorizontalAlignment.Right
            Me.txtT_tien_nt2.Value = 0
            Me.txtStatus.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.txtStatus.BackColor = Color.White
            Me.txtStatus.Location = New Point(8, 454)
            Me.txtStatus.MaxLength = 1
            Me.txtStatus.Name = "txtStatus"
            Me.txtStatus.Size = New Size(25, 20)
            Me.txtStatus.TabIndex = 41
            Me.txtStatus.TabStop = False
            Me.txtStatus.Tag = "FCCF"
            Me.txtStatus.Text = "txtStatus"
            Me.txtStatus.TextAlign = HorizontalAlignment.Right
            Me.txtStatus.Visible = False
            Me.lblStatus.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            Me.lblStatus.AutoSize = True
            Me.lblStatus.Location = New Point(438, 70)
            Me.lblStatus.Name = "lblStatus"
            Me.lblStatus.Size = New Size(55, 16)
            Me.lblStatus.TabIndex = 29
            Me.lblStatus.Tag = ""
            Me.lblStatus.Text = "Trang thai"
            Me.lblStatusMess.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.lblStatusMess.AutoSize = True
            Me.lblStatusMess.Location = New Point(48, 456)
            Me.lblStatusMess.Name = "lblStatusMess"
            Me.lblStatusMess.Size = New Size(199, 16)
            Me.lblStatusMess.TabIndex = 42
            Me.lblStatusMess.Tag = ""
            Me.lblStatusMess.Text = "1 - Ghi vao SC, 0 - Chua ghi vao so cai"
            Me.lblStatusMess.Visible = False
            Me.txtKeyPress.AutoSize = False
            Me.txtKeyPress.Location = New Point(408, 96)
            Me.txtKeyPress.Name = "txtKeyPress"
            Me.txtKeyPress.Size = New Size(10, 10)
            Me.txtKeyPress.TabIndex = 12
            Me.txtKeyPress.Text = ""
            Me.cboStatus.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            Me.cboStatus.BackColor = Color.White
            Me.cboStatus.Enabled = False
            Me.cboStatus.Location = New Point(498, 68)
            Me.cboStatus.Name = "cboStatus"
            Me.cboStatus.Size = New Size(140, 21)
            Me.cboStatus.TabIndex = 10
            Me.cboStatus.TabStop = False
            Me.cboStatus.Tag = ""
            Me.cboStatus.Text = "cboStatus"
            Me.cboAction.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            Me.cboAction.BackColor = Color.White
            Me.cboAction.Location = New Point(498, 89)
            Me.cboAction.Name = "cboAction"
            Me.cboAction.Size = New Size(140, 21)
            Me.cboAction.TabIndex = 11
            Me.cboAction.TabStop = False
            Me.cboAction.Tag = "CF"
            Me.cboAction.Text = "cboAction"
            Me.lblAction.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            Me.lblAction.AutoSize = True
            Me.lblAction.Location = New Point(438, 91)
            Me.lblAction.Name = "lblAction"
            Me.lblAction.Size = New Size(29, 16)
            Me.lblAction.TabIndex = 33
            Me.lblAction.Tag = ""
            Me.lblAction.Text = "Xu ly"
            Me.lblMa_kh.AutoSize = True
            Me.lblMa_kh.Location = New Point(2, 7)
            Me.lblMa_kh.Name = "lblMa_kh"
            Me.lblMa_kh.Size = New Size(53, 16)
            Me.lblMa_kh.TabIndex = 34
            Me.lblMa_kh.Tag = "L002"
            Me.lblMa_kh.Text = "Ma khach"
            Me.txtMa_kh.BackColor = Color.White
            Me.txtMa_kh.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_kh.Location = New Point(88, 5)
            Me.txtMa_kh.Name = "txtMa_kh"
            Me.txtMa_kh.TabIndex = 1
            Me.txtMa_kh.Tag = "FCNBCF"
            Me.txtMa_kh.Text = "TXTMA_KH"
            Me.lblTen_kh.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Top))
            Me.lblTen_kh.Location = New Point(192, 7)
            Me.lblTen_kh.Name = "lblTen_kh"
            Me.lblTen_kh.Size = New Size(233, 12)
            Me.lblTen_kh.TabIndex = 36
            Me.lblTen_kh.Tag = "FCRF"
            Me.lblTen_kh.Text = "Ten Khach"
            Me.lblOng_ba.AutoSize = True
            Me.lblOng_ba.Location = New Point(2, 28)
            Me.lblOng_ba.Name = "lblOng_ba"
            Me.lblOng_ba.Size = New Size(59, 16)
            Me.lblOng_ba.TabIndex = 37
            Me.lblOng_ba.Tag = "L003"
            Me.lblOng_ba.Text = "Nguoi mua"
            Me.txtOng_ba.BackColor = Color.White
            Me.txtOng_ba.Location = New Point(88, 26)
            Me.txtOng_ba.Name = "txtOng_ba"
            Me.txtOng_ba.TabIndex = 2
            Me.txtOng_ba.Tag = "FCCF"
            Me.txtOng_ba.Text = "txtOng_ba"
            Me.lblTotal.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.lblTotal.AutoSize = True
            Me.lblTotal.Location = New Point(251, 361)
            Me.lblTotal.Name = "lblTotal"
            Me.lblTotal.Size = New Size(58, 16)
            Me.lblTotal.TabIndex = 60
            Me.lblTotal.Tag = "L013"
            Me.lblTotal.Text = "Tong cong"
            Me.lblTien_ck.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.lblTien_ck.AutoSize = True
            Me.lblTien_ck.Location = New Point(336, 403)
            Me.lblTien_ck.Name = "lblTien_ck"
            Me.lblTien_ck.Size = New Size(58, 16)
            Me.lblTien_ck.TabIndex = 61
            Me.lblTien_ck.Tag = "L014"
            Me.lblTien_ck.Text = "Chiet khau"
            Me.lblMa_tt.AutoSize = True
            Me.lblMa_tt.Location = New Point(2, 91)
            Me.lblMa_tt.Name = "lblMa_tt"
            Me.lblMa_tt.Size = New Size(29, 16)
            Me.lblMa_tt.TabIndex = 65
            Me.lblMa_tt.Tag = "L008"
            Me.lblMa_tt.Text = "Ma tt"
            Me.txtMa_tt.BackColor = Color.White
            Me.txtMa_tt.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_tt.Location = New Point(88, 89)
            Me.txtMa_tt.Name = "txtMa_tt"
            Me.txtMa_tt.Size = New Size(24, 20)
            Me.txtMa_tt.TabIndex = 5
            Me.txtMa_tt.Tag = "FCCF"
            Me.txtMa_tt.Text = "TXTMA_TT"
            Me.lblTen_tt.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Top))
            Me.lblTen_tt.Location = New Point(113, 91)
            Me.lblTen_tt.Name = "lblTen_tt"
            Me.lblTen_tt.Size = New Size(312, 16)
            Me.lblTen_tt.TabIndex = 66
            Me.lblTen_tt.Tag = "FCRF"
            Me.lblTen_tt.Text = "Ten thanh toan"
            Me.lblTen.AutoSize = True
            Me.lblTen.Location = New Point(574, 456)
            Me.lblTen.Name = "lblTen"
            Me.lblTen.Size = New Size(58, 16)
            Me.lblTen.TabIndex = 68
            Me.lblTen.Tag = "RF"
            Me.lblTen.Text = "Ten chung"
            Me.lblTen.Visible = False
            Me.txtNgay_hl.BackColor = Color.White
            Me.txtNgay_hl.Location = New Point(88, 68)
            Me.txtNgay_hl.MaxLength = 10
            Me.txtNgay_hl.Name = "txtNgay_hl"
            Me.txtNgay_hl.TabIndex = 4
            Me.txtNgay_hl.Tag = "FDCFDF"
            Me.txtNgay_hl.Text = "  /  /    "
            Me.txtNgay_hl.TextAlign = HorizontalAlignment.Right
            Me.txtNgay_hl.Value = New DateTime(0)
            Me.lblNgay_hl.AutoSize = True
            Me.lblNgay_hl.Location = New Point(2, 70)
            Me.lblNgay_hl.Name = "lblNgay_hl"
            Me.lblNgay_hl.Size = New Size(73, 16)
            Me.lblNgay_hl.TabIndex = 72
            Me.lblNgay_hl.Tag = "L007"
            Me.lblNgay_hl.Text = "Ngay hieu luc"
            Me.txtDien_giai.BackColor = Color.White
            Me.txtDien_giai.Location = New Point(88, 47)
            Me.txtDien_giai.Name = "txtDien_giai"
            Me.txtDien_giai.Size = New Size(337, 20)
            Me.txtDien_giai.TabIndex = 3
            Me.txtDien_giai.Tag = "FCCF"
            Me.txtDien_giai.Text = "txtDien_giai"
            Me.Label1.AutoSize = True
            Me.Label1.Location = New Point(2, 49)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New Size(48, 16)
            Me.Label1.TabIndex = 75
            Me.Label1.Tag = "L029"
            Me.Label1.Text = "Dien giai"
            Me.lvlT_cp.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.lvlT_cp.AutoSize = True
            Me.lvlT_cp.Location = New Point(336, 382)
            Me.lvlT_cp.Name = "lvlT_cp"
            Me.lvlT_cp.Size = New Size(39, 16)
            Me.lvlT_cp.TabIndex = 81
            Me.lvlT_cp.Tag = "L030"
            Me.lvlT_cp.Text = "Chi phi"
            Me.txtT_cp_nt.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.txtT_cp_nt.BackColor = Color.White
            Me.txtT_cp_nt.Enabled = False
            Me.txtT_cp_nt.ForeColor = Color.Black
            Me.txtT_cp_nt.Format = "m_ip_tien_nt"
            Me.txtT_cp_nt.Location = New Point(437, 380)
            Me.txtT_cp_nt.MaxLength = 13
            Me.txtT_cp_nt.Name = "txtT_cp_nt"
            Me.txtT_cp_nt.TabIndex = 17
            Me.txtT_cp_nt.Tag = "FN"
            Me.txtT_cp_nt.Text = "m_ip_tien_nt"
            Me.txtT_cp_nt.TextAlign = HorizontalAlignment.Right
            Me.txtT_cp_nt.Value = 0
            Me.txtT_cp.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.txtT_cp.BackColor = Color.White
            Me.txtT_cp.Enabled = False
            Me.txtT_cp.ForeColor = Color.Black
            Me.txtT_cp.Format = "m_ip_tien"
            Me.txtT_cp.Location = New Point(538, 380)
            Me.txtT_cp.MaxLength = 10
            Me.txtT_cp.Name = "txtT_cp"
            Me.txtT_cp.TabIndex = 18
            Me.txtT_cp.Tag = "FN"
            Me.txtT_cp.Text = "m_ip_tien"
            Me.txtT_cp.TextAlign = HorizontalAlignment.Right
            Me.txtT_cp.Value = 0
            Me.txtT_so_luong.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
            Me.txtT_so_luong.BackColor = Color.White
            Me.txtT_so_luong.Enabled = False
            Me.txtT_so_luong.ForeColor = Color.Black
            Me.txtT_so_luong.Format = "m_ip_sl"
            Me.txtT_so_luong.Location = New Point(336, 359)
            Me.txtT_so_luong.MaxLength = 8
            Me.txtT_so_luong.Name = "txtT_so_luong"
            Me.txtT_so_luong.TabIndex = 14
            Me.txtT_so_luong.Tag = "FN"
            Me.txtT_so_luong.Text = "m_ip_sl"
            Me.txtT_so_luong.TextAlign = HorizontalAlignment.Right
            Me.txtT_so_luong.Value = 0
            Me.txtLoai_ct.BackColor = Color.White
            Me.txtLoai_ct.CharacterCasing = CharacterCasing.Upper
            Me.txtLoai_ct.Location = New Point(520, 456)
            Me.txtLoai_ct.Name = "txtLoai_ct"
            Me.txtLoai_ct.Size = New Size(30, 20)
            Me.txtLoai_ct.TabIndex = 85
            Me.txtLoai_ct.Tag = "FC"
            Me.txtLoai_ct.Text = "TXTLOAI_CT"
            Me.txtLoai_ct.Visible = False
            Me.AutoScaleBaseSize = New Size(5, 13)
            Me.ClientSize = New Size(642, 473)
            Me.Controls.Add(Me.txtLoai_ct)
            Me.Controls.Add(Me.txtT_so_luong)
            Me.Controls.Add(Me.lvlT_cp)
            Me.Controls.Add(Me.txtT_cp_nt)
            Me.Controls.Add(Me.txtT_cp)
            Me.Controls.Add(Me.txtDien_giai)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.lblNgay_hl)
            Me.Controls.Add(Me.txtNgay_hl)
            Me.Controls.Add(Me.lblTen)
            Me.Controls.Add(Me.lblMa_tt)
            Me.Controls.Add(Me.txtMa_tt)
            Me.Controls.Add(Me.lblTien_ck)
            Me.Controls.Add(Me.lblTotal)
            Me.Controls.Add(Me.txtOng_ba)
            Me.Controls.Add(Me.lblOng_ba)
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
            Me.Controls.Add(Me.lblSo_ct)
            Me.Controls.Add(Me.lblMa_dvcs)
            Me.Controls.Add(Me.txtStatus)
            Me.Controls.Add(Me.txtNgay_ct)
            Me.Controls.Add(Me.txtNgay_lct)
            Me.Controls.Add(Me.txtSo_ct)
            Me.Controls.Add(Me.txtMa_dvcs)
            Me.Controls.Add(Me.lblTen_dvcs)
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
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "frmVoucher"
            Me.tbDetail.ResumeLayout(False)
            Me.tpgDetail.ResumeLayout(False)
            Me.grdDetail.EndInit()
            Me.tbgCharge.ResumeLayout(False)
            Me.grdCharge.EndInit()
            Me.tbgOthers.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub

        Private Sub InitInventory()
            Me.xInventory.ColItem = Me.colMa_vt
            Me.xInventory.ColSite = Me.colMa_kho
            Me.xInventory.ColUOM = Me.colDvt
            Me.xInventory.colQty = Me.colSo_luong
            Me.xInventory.txtUnit = Me.txtMa_dvcs
            Me.xInventory.InvVoucher = Me.oVoucher
            Me.xInventory.oInvItem = Me.oInvItemDetail
            Me.xInventory.oInvSite = Me.oSite
            Me.xInventory.oInvUOM = Me.oUOM
            Me.xInventory.Init()
        End Sub

        Public Sub InitRecords()
            Dim str As String
            If oVoucher.isRead Then
                str = String.Concat(New String() {"EXEC fs_LoadSQTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', -1"})
            Else
                str = String.Concat(New String() {"EXEC fs_LoadSQTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
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

        Private Sub InitSOPrice()
            Dim str As String
            Dim num As Integer
            Dim str3 As String = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "sysspmasterinfo", "xread", ("xid = '" & modVoucher.VoucherCode & "'")))
            If (StringType.StrCmp(Strings.Trim(str3), "", False) <> 0) Then
                Dim num5 As Integer = IntegerType.FromObject(Fox.GetWordCount(str3, ","c))
                num = 1
                Do While (num <= num5)
                    str = Strings.Trim(Fox.GetWordNum(str3, num, ","c))
                    Dim num4 As Integer = (Me.Controls.Count - 1)
                    Dim i As Integer = 0
                    Do While (i <= num4)
                        Dim str2 As String = Strings.Trim(Me.Controls.Item(i).Name)
                        Dim flag As Boolean = False
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
                Dim column As New DataGridTextBoxColumn
                column = GetColumn(Me.grdDetail, str)
                column.TextBox.Name = column.MappingName
                AddHandler column.TextBox.Validated, New EventHandler(AddressOf Me.ValidObjects)
                AddHandler column.TextBox.Enter, New EventHandler(AddressOf Me.EnterObjects)
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

        Private Sub NewItemCharge(ByVal sender As Object, ByVal e As EventArgs)
            If (Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) AndAlso Not Me.grdCharge.ReadOnly) Then
                Dim cell As DataGridCell
                Dim currentRowIndex As Integer = Me.grdCharge.CurrentRowIndex
                If (currentRowIndex < 0) Then
                    modVoucher.tblCharge.AddNew()
                    cell = New DataGridCell(0, 0)
                    Me.grdCharge.CurrentCell = cell
                ElseIf (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblCharge.Item(currentRowIndex).Item("ma_cp"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblCharge.Item(currentRowIndex).Item("ma_cp"))), "", False) <> 0)) Then
                    Dim count As Integer = modVoucher.tblCharge.Count
                    Me.grdCharge.BeforeAddNewItem()
                    cell = New DataGridCell(count, 0)
                    Me.grdCharge.CurrentCell = cell
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
                        oVoucher.ViewDeletedRecord("fs_SearchDeletedSQTran", "SQMaster", "SQDetail", "t_tien2", "t_tien_nt2")
                        Exit Select
                End Select
            End If
        End Sub

        Private Function Post() As String
            Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "groupby", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            Dim str3 As String = "EXEC fs_PostSQ "
            Return (StringType.FromObject(ObjectType.AddObj(((((((str3 & "'" & modVoucher.VoucherCode & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))) & "'") & ", '" & Strings.Trim(str) & "'"), ObjectType.AddObj(ObjectType.AddObj(", '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))) & ", 1")
        End Function

        Public Sub Print()
            Dim print As New frmPrint
            print.txtSo_lien.Value = DoubleType.FromObject(modVoucher.oVoucherRow.Item("so_lien"))
            'print.txtTitle.Text = StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct"))), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct2")))))
            Dim result As DialogResult = print.ShowDialog
            If ((result <> DialogResult.Cancel) AndAlso (print.txtSo_lien.Value > 0)) Then
                Dim selectedIndex As Integer = print.cboReports.SelectedIndex
                Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(print.table.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
                Dim view As New DataView
                Dim ds As New DataSet
                Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj((("EXEC fs_PrintSQTran '" & modVoucher.cLan) & "', " & "[a.stt_rec = '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'], '"), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf")))), "'"))
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
                clsprint.SetReportVar(modVoucher.sysConn, modVoucher.appConn, "SQTran", modVoucher.oOption, clsprint.oRpt)
                clsprint.oRpt.SetParameterValue("Title", Strings.Trim(print.txtTitle.Text))
                Dim str2 As String = Strings.Replace(Strings.Replace(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("401")), "%s1", Me.txtNgay_ct.Value.Day.ToString, 1, -1, CompareMethod.Binary), "%s2", Me.txtNgay_ct.Value.Month.ToString, 1, -1, CompareMethod.Binary), "%s3", Me.txtNgay_ct.Value.Year.ToString, 1, -1, CompareMethod.Binary)
                Dim str4 As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("402")), "%s", Strings.Trim(Me.txtSo_ct.Text), 1, -1, CompareMethod.Binary)
                Dim str As String = Strings.Replace(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("403")), "%s1", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary), "%s", clsprint.Num2Words(New Decimal(Me.txtT_tien_nt2.Value)), 1, -1, CompareMethod.Binary)
                clsprint.oRpt.SetParameterValue("s_byword", str)
                clsprint.oRpt.SetParameterValue("t_date", str2)
                clsprint.oRpt.SetParameterValue("t_number", str4)
                clsprint.oRpt.SetParameterValue("nAmount", Me.txtT_tien_nt2.Value)
                clsprint.oRpt.SetParameterValue("f_ong_ba", Strings.Trim(Me.txtOng_ba.Text))
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
                print.Dispose()
            End If
        End Sub

        Private Sub ReadOnlyObjects(ByVal sender As Object, ByVal e As EventArgs)
            On Error Resume Next
            Dim num2 As Integer = 0
            Dim num6 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim num As Integer = 0
            For num = 0 To num6
                If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ma_vt")), "C") Then
                    num2 = 1
                    Exit For
                End If
            Next
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
                LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(num2 > 0)}, Nothing)
            End If
        End Sub

        Public Sub RefrehForm()
            Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
            Me.grdHeader.Scatter()
            ScatterMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
            Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'")
            modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
            Me.RefreshCharge(1)
            Me.UpdateList()
            Me.vCaptionRefresh()
            xtabControl.ScatterTabControl(modVoucher.tblMaster.Item(Me.iMasterRow), Me.tbDetail)
            Me.cmdNew.Focus()
        End Sub

        Private Sub RefreshCharge(ByVal nType As Byte)
            modVoucher.tblCharge.Table.Clear()

            If (nType <> 0) Then
                Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(("fs_LoadSOCharge '" & modVoucher.cLan & "', '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, modVoucher.alCharge, (modVoucher.tblCharge.Table.DataSet))
            End If
        End Sub

        Private Sub RefreshControlField()
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

        Public Sub Save()
            Me.txtStatus.Text = Strings.Trim(StringType.FromObject(Me.tblHandling.Rows.Item(Me.cboAction.SelectedIndex).Item("action_id")))
            Me.txtNgay_ct.Value = Me.txtNgay_lct.Value
            Try
                Dim cell As New DataGridCell(0, 0)
                Me.grdDetail.CurrentCell = cell
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
                    Do While (num <= num10)
                        Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                        num2 = 1
                        Do While (num2 <= num9)
                            str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str))) Then
                                modVoucher.tblDetail.Item(num).Item(str) = 0
                            End If
                            num2 += 1
                        Loop
                        num += 1
                    Loop
                    If (StringType.StrCmp(Me.txtStatus.Text, "0", False) <> 0) Then
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
                        If (DateTime.Compare(Me.txtNgay_hl.Value, Me.txtNgay_lct.Value) < 0) Then
                            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("041")), 2)
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
                            auditamount.AuditAmounts(New Decimal(Me.txtT_tien2.Value), "tien2", modVoucher.tblDetail)
                            auditamount.AuditAmounts(New Decimal(Me.txtT_ck.Value), "ck", modVoucher.tblDetail)
                        End If
                        Me.UpdateSQ()
                        Me.UpdateList()

                        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                            Me.cIDNumber = oVoucher.GetIdentityNumber
                            modVoucher.tblMaster.AddNew()
                            Me.iMasterRow = (modVoucher.tblMaster.Count - 1)
                            modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec") = Me.cIDNumber
                            modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_ct") = modVoucher.VoucherCode
                        Else
                            Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                            Me.BeforUpdateSQ(Me.cIDNumber, "Edit")
                        End If
                        xtabControl.GatherMemvarTabControl(modVoucher.tblMaster.Item(Me.iMasterRow), Me.tbDetail)
                        DirLib.SetDatetime(modVoucher.appConn, modVoucher.tblMaster.Item(Me.iMasterRow), oVoucher.cAction)
                        Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
                        Me.grdHeader.Gather()
                        GatherMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct"))), Me.txtSo_ct.MaxLength)
                        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                            str6 = GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row)
                        Else
                            Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                            str6 = ((GenSQLUpdate((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row, cKey) & ChrW(13) & GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)) & ChrW(13) & GenSQLDelete("ctcp20", cKey))
                        End If
                        cString = "ma_ct, ngay_ct, so_ct, stt_rec"
                        Dim str5 As String = ("stt_rec = '" & Me.cIDNumber & "' or stt_rec = '' or stt_rec is null")
                        modVoucher.tblDetail.RowFilter = str5
                        num3 = (modVoucher.tblDetail.Count - 1)
                        Dim expression As Integer = 0
                        Dim num8 As Integer = num3
                        num = 0
                        Do While (num <= num8)
                            If (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("stt_rec"), Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "", RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))), False) = 0) Then
                                Dim num7 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                                num2 = 1
                                Do While (num2 <= num7)
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
                            str6 = (str6 & ChrW(13) & GenSQLInsert((modVoucher.appConn), "ctcp20", modVoucher.tblCharge.Item(num).Row))
                            num += 1
                        Loop
                        oVoucher.IncreaseVoucherNo(Strings.Trim(Me.txtSo_ct.Text))
                        Me.EDTBColumns(False)
                        Sql.SQLCompressExecute((modVoucher.appConn), str6)
                        str6 = Me.Post
                        Sql.SQLExecute((modVoucher.appConn), str6)
                        Me.grdHeader.UpdateFreeField(modVoucher.appConn, StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")))
                        Me.AfterUpdateSQ(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "Save")
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
            Dim frm As New frmSearch()
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
                Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                view.Item("ck_nt") = num
                view.Item("ck") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                view = Nothing
                Me.UpdateList()
            End If
        End Sub

        Private Sub txtCk_valid(ByVal sender As Object, ByVal e As EventArgs)
            Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
            Dim noldCk As Decimal = Me.noldCk
            Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
            If (Decimal.Compare(num, noldCk) <> 0) Then
                Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                view.Item("ck") = num
                view = Nothing
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
                Dim view As DataRowView = modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex)
                view.Item("tien_cp_nt") = num
                view.Item("tien_cp") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                view = Nothing
            End If
            Me.ShowTotalCharge(2)
        End Sub

        Private Sub txtCTien_cp_valid(ByVal sender As Object, ByVal e As EventArgs)
            Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
            Dim num3 As Decimal = Me.noldCTien_cp
            Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
            If (Decimal.Compare(num, num3) <> 0) Then
                Dim view As DataRowView = modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex)
                view.Item("tien_cp") = num
                view = Nothing
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
                Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdMV.CurrentRowIndex)
                view.Item(cField) = num
                view = Nothing
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
                Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdMV.CurrentRowIndex)
                view.Item(cField) = num
                view.Item(cRef) = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                view = Nothing
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
                Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                view.Item("gia_nt2") = num
                view.Item("gia2") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                Dim args As Object() = New Object() {ObjectType.MulObj(view.Item("so_luong"), num), num3}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(args(1))
                End If
                view.Item("tien_nt2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.MulObj(view.Item("tien_nt2"), Me.txtTy_gia.Value), num5}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num5 = ByteType.FromObject(objArray2(1))
                End If
                view.Item("Tien2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                view = Nothing
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
                Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                view.Item("gia2") = num
                Dim args As Object() = New Object() {ObjectType.MulObj(view.Item("so_luong"), num), num5}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num5 = ByteType.FromObject(args(1))
                End If
                view.Item("tien2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                view = Nothing
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

        Private Sub txtMa_kh_valid(ByVal sender As Object, ByVal e As EventArgs)
            If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) And (StringType.StrCmp(Strings.Trim(Me.txtMa_tt.Text), "", False) = 0)) Then
                Me.txtMa_tt.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "ma_tt", ("ma_kh = '" & Me.txtMa_kh.Text & "'"))))
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
                Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                view.Item("thue_suat") = zero
                view.Item("tk_thue") = str2
                view.Item("ma_thue") = str3
                Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
                Me.UpdateList()
                Me.colThue_nt.TextBox.Text = Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue_nt")))
                view = Nothing
            End If
        End Sub
        Private Sub RecalcTax(ByVal iRow As Integer, ByVal nType As Integer)
            Dim num As Byte
            Dim decimals As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
            If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
                num = decimals
            Else
                num = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            End If
            Dim m_round_gia_nt, m_round_gia As Byte
            m_round_gia = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
            If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
                m_round_gia_nt = m_round_gia
            Else
                m_round_gia_nt = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
            End If
            With modVoucher.tblDetail.Item(iRow)
                If IsDBNull(.Item("thue_suat")) Then
                    .Item("thue_suat") = 0
                End If
                If IsDBNull(.Item("Tien_nt2")) Then
                    .Item("Tien_nt2") = 0
                End If
                If IsDBNull(.Item("Tien2")) Then
                    .Item("Tien2") = 0
                End If
                .Item("thue_nt") = Math.Round((.Item("tien_nt2") - .Item("ck_nt")) * .Item("thue_suat") / 100, num)
                .Item("thue") = Math.Round(.Item("thue_nt") * Me.txtTy_gia.Value, decimals)
            End With
        End Sub

        Private Sub txtNumber_Enter(ByVal sender As Object, ByVal e As EventArgs)
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
                Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("gia_nt2"))) Then
                    view.Item("gia_nt2") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("gia2"))) Then
                    view.Item("gia2") = 0
                End If
                view.Item("so_luong") = num
                Dim args As Object() = New Object() {ObjectType.MulObj(view.Item("gia_nt2"), num), num2}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(args(1))
                End If
                view.Item("tien_nt2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.MulObj(view.Item("gia2"), num), num3}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(objArray2(1))
                End If
                view.Item("tien2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                view = Nothing
                Me.grdDetail.Refresh()
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
                Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                view.Item("Tien_nt2") = num
                view.Item("Tien2") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                view = Nothing
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
                Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                view.Item("Tien2") = num
                view = Nothing
                Me.UpdateList()
            End If
        End Sub

        Private Sub txtTk_Validated(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Private Sub txtTy_gia_Enter(ByVal sender As Object, ByVal e As EventArgs)
            oVoucher.noldFCrate = New Decimal(Me.txtTy_gia.Value)
        End Sub

        Private Sub txtTy_gia_Validated(ByVal sender As Object, ByVal e As EventArgs)
            Me.vFCRate()
        End Sub

        Public Sub UpdateList()
            Dim zero As Decimal = Decimal.Zero
            Dim num8 As Decimal = Decimal.Zero
            Dim num4 As Decimal = Decimal.Zero
            Dim num5 As Decimal = Decimal.Zero
            Dim num2 As Decimal = Decimal.Zero
            Dim num3 As Decimal = Decimal.Zero
            Dim num6 As Decimal = Decimal.Zero
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit", "View"}) Then
                Dim num9 As Integer = (modVoucher.tblDetail.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num9)
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien2"))) Then
                        zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblDetail.Item(i).Item("tien2")))
                    End If
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien_nt2"))) Then
                        num8 = DecimalType.FromObject(ObjectType.AddObj(num8, modVoucher.tblDetail.Item(i).Item("tien_nt2")))
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
            Me.txtT_tien2.Value = Convert.ToDouble(zero)
            Me.txtT_tien_nt2.Value = Convert.ToDouble(num8)
        End Sub

        Private Sub UpdateSQ()
        End Sub

        Private Sub ValidObjects(ByVal sender As Object, ByVal e As EventArgs)
            On Error Resume Next
            If Not ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
                Return
            End If
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If (Me.iOldRow <> currentRowIndex) Then
                Return
            End If
            Dim ds As New DataSet
            Dim num4 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
            Dim num3 As Byte
            If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
                num3 = num4
            Else
                num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            End If
            Dim str5 As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
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
            Dim sOldStringDvt As String
            If (ObjectType.ObjTst(obj2, "MA_VT", False) = 0) Then
                sOldStringDvt = Me.sOldStringMa_vt
            ElseIf (ObjectType.ObjTst(obj2, "MA_KHO", False) = 0) Then
                sOldStringDvt = Me.sOldStringMa_kho
            ElseIf (ObjectType.ObjTst(obj2, "DVT", False) = 0) Then
                sOldStringDvt = Me.sOldStringDvt
            ElseIf (ObjectType.ObjTst(obj2, "SO_LUONG", False) = 0) Then
                sOldStringDvt = Strings.Replace(Me.sOldStringSo_luong, " ", "", 1, -1, CompareMethod.Binary)
            Else
                sOldStringDvt = Me.sOldStringSo_luong
            End If
            If (StringType.StrCmp(Strings.Trim(str5), Strings.Trim(sOldStringDvt), False) = 0) Then
                Return
            End If
            Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Name", New Object(0 - 1) {}, Nothing, Nothing)))
            Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "sysspdetailinfo", String.Concat(New String() {"xid = '", modVoucher.VoucherCode, "' AND xvalid = '", str, "'"})), DataRow)
            Dim str4 As String = StringType.FromObject(row.Item("xfields"))
            Dim str3 As String = StringType.FromObject(row.Item("xfcfields"))
            Dim cString As String = StringType.FromObject(row.Item("xreffields"))
            If (StringType.StrCmp(Strings.Trim(str4), "", False) = 0) Then
                Return
            End If
            Dim str8 As String = "EXEC fs_GetSOPrice "
            str8 = (str8 & "'" & Strings.Trim(str) & "'")
            str8 = (str8 & ", '" & Strings.Trim(modVoucher.VoucherCode) & "'")
            str8 = StringType.FromObject(ObjectType.AddObj(str8, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, ""))))
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
            Sql.SQLRetrieve((modVoucher.appConn), str8, "xprice", (ds))
            Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(str4, ","c))
            Dim nWordPosition As Integer = 1
            For nWordPosition = 1 To num9
                str = Strings.Trim(Fox.GetWordNum(str4, nWordPosition, ","c))
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(ds.Tables.Item(0).Rows.Item(0).Item(str))) Then
                    modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item(str) = RuntimeHelpers.GetObjectValue(ds.Tables.Item(0).Rows.Item(0).Item(str))
                End If
            Next
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If (StringType.StrCmp(Strings.Trim(str3), "", False) <> 0) Then
                Dim num8 As Integer = IntegerType.FromObject(Fox.GetWordCount(str3, ","c))
                For nWordPosition = 1 To num8
                    str = Strings.Trim(Fox.GetWordNum(str3, nWordPosition, ","c))
                    Dim str2 As String = Strings.Trim(Fox.GetWordNum(cString, nWordPosition, ","c))
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item(str))) Then
                        view.Item(str2) = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(view.Item(str), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                    End If
                Next
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("gia_nt2"))) Then
                view.Item("gia_nt2") = 0
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("gia2"))) Then
                view.Item("gia2") = 0
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("so_luong"))) Then
                view.Item("so_luong") = 0
            End If
            objArray2 = New Object() {ObjectType.MulObj(view.Item("gia_nt2"), view.Item("so_luong")), num3}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num3 = ByteType.FromObject(objArray2(1))
            End If
            view.Item("tien_nt2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Math), "Round", objArray2, Nothing, copyBack))
            objArray2 = New Object() {ObjectType.MulObj(view.Item("gia2"), view.Item("so_luong")), num4}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num4 = ByteType.FromObject(objArray2(1))
            End If
            view.Item("tien2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Math), "Round", objArray2, Nothing, copyBack))
            view = Nothing
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
                Dim tblDetail As DataView = modVoucher.tblDetail
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
                tblDetail = Nothing
                Dim tblCharge As DataView = modVoucher.tblCharge
                Dim num2 As Integer = (modVoucher.tblCharge.Count - 1)
                num = 0
                Do While (num <= num2)
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblCharge.Item(num).Item("tien_cp_nt"))) Then
                        tblCharge.Item(num).Item("tien_cp") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblCharge.Item(num).Item("tien_cp_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                    End If
                    num += 1
                Loop
                tblCharge = Nothing
            End If
            Me.txtT_tien2.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_tien_nt2.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
            Me.txtT_cp.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_cp_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
            Me.txtT_ck.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_ck_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        End Sub

        Public Sub View()
            Dim num3 As Decimal
            Dim frmAdd As New Form
            Dim gridformtran2 As New gridformtran
            Dim gridformtran As New gridformtran
            Dim tbs As New DataGridTableStyle
            Dim style As New DataGridTableStyle
            Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(modVoucher.MaxColumns) {}
            Dim index As Integer = 0
            Do
                cols(index) = New DataGridTextBoxColumn
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index < modVoucher.MaxColumns)
            Dim form2 As Form = frmAdd
            form2.Top = 0
            form2.Left = 0
            form2.Width = Me.Width
            form2.Height = Me.Height
            form2.Text = Me.Text
            form2.StartPosition = FormStartPosition.CenterParent
            Dim panel As StatusBarPanel = AddStb(frmAdd)
            form2 = Nothing
            Dim gridformtran4 As gridformtran = gridformtran2
            gridformtran4.CaptionVisible = False
            gridformtran4.ReadOnly = True
            gridformtran4.Top = 0
            gridformtran4.Left = 0
            gridformtran4.Height = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
            gridformtran4.Width = (Me.Width - 5)
            gridformtran4.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            gridformtran4.BackgroundColor = Color.White
            gridformtran4 = Nothing
            Dim gridformtran3 As gridformtran = gridformtran
            gridformtran3.CaptionVisible = False
            gridformtran3.ReadOnly = True
            gridformtran3.Top = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
            gridformtran3.Left = 0
            gridformtran3.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 30))))
            gridformtran3.Width = (Me.Width - 5)
            gridformtran3.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
            gridformtran3.BackgroundColor = Color.White
            gridformtran3 = Nothing
            Dim button As New Button
            button.Visible = True
            button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
            button.Left = (-100 - button.Width)
            frmAdd.Controls.Add(button)
            frmAdd.CancelButton = button
            frmAdd.Controls.Add(gridformtran2)
            frmAdd.Controls.Add(gridformtran)
            Dim grdFill As DataGrid = gridformtran2
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblMaster), (grdFill), (tbs), (cols), "SQMaster")
            gridformtran2 = DirectCast(grdFill, gridformtran)
            index = 0
            Do
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index < modVoucher.MaxColumns)
            cols(2).Alignment = HorizontalAlignment.Right
            grdFill = gridformtran
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), (grdFill), (style), (cols), "SQDetail")
            gridformtran = DirectCast(grdFill, gridformtran)
            index = 0
            Do
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index < modVoucher.MaxColumns)
            oVoucher.HideFields(gridformtran)
            Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
            Dim count As Integer = modVoucher.tblMaster.Count
            Dim zero As Decimal = Decimal.Zero
            Dim num5 As Integer = (count - 1)
            index = 0
            Do While (index <= num5)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(index).Item("t_tien2"))) Then
                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblMaster.Item(index).Item("t_tien2")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(index).Item("t_tien_nt2"))) Then
                    num3 = DecimalType.FromObject(ObjectType.AddObj(num3, modVoucher.tblMaster.Item(index).Item("t_tien_nt2")))
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
            On Error Resume Next
            Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
            If (StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.coldCMa_cp), False) = 0) Then
                Return
            End If
            Dim view As DataRowView = modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex)
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_cp")), "C") Then
                view.Item("loai_cp") = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "dmcp", "loai_cp", ("ma_loai = '" & str & "'")))
                view.Item("loai_pb") = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "dmcp", "loai_pb", ("ma_loai = '" & str & "'")))
            Else
                view.Item("tien_cp_nt") = 0
                view.Item("tien_cp") = 0
            End If
            view = Nothing
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
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
                Dim str2 As String = Strings.Trim(StringType.FromObject(view.Item("ma_vt")))
                Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmvt", ("ma_vt = '" & str2 & "'")), DataRow)
                view.Item("volume") = RuntimeHelpers.GetObjectValue(row.Item("volume"))
                view.Item("weight") = RuntimeHelpers.GetObjectValue(row.Item("weight"))
                If BooleanType.FromObject(ObjectType.NotObj(row.Item("sua_tk_vt"))) Then
                    view.Item("tk_vt") = RuntimeHelpers.GetObjectValue(row.Item("tk_vt"))
                ElseIf clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("tk_vt")), "C") Then
                    view.Item("tk_vt") = RuntimeHelpers.GetObjectValue(row.Item("tk_vt"))
                End If
                view.Item("dvt") = RuntimeHelpers.GetObjectValue(row.Item("dvt"))
                Me.colDvt.TextBox.Text = StringType.FromObject(view.Item("dvt"))
                view.Item("he_so") = 1
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
                    view.Item("ma_lo") = ""
                End If
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_kho")), "C") Then
                    view.Item("ma_kho") = RuntimeHelpers.GetObjectValue(row.Item("ma_kho"))
                End If
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vi_tri")), "C") Then
                    view.Item("ma_vi_tri") = RuntimeHelpers.GetObjectValue(row.Item("ma_vi_tri"))
                End If
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_thue")), "C") Then
                    Dim row2 As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmthue", StringType.FromObject(ObjectType.AddObj("ma_thue = ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(row.Item("ma_thue")), "")))), DataRow)
                    If Not (row2 Is Nothing) Then
                        Me.coldMa_thue = ""
                        view.Item("ma_thue") = RuntimeHelpers.GetObjectValue(row2.Item("ma_thue"))
                        Me.colMa_thue.TextBox.Text = StringType.FromObject(view.Item("ma_thue"))
                        Me.txtMa_thue_valid(Me.colMa_thue.TextBox, New EventArgs)
                    End If
                End If
            End If
            view.Item("gia_nt2") = Sql.GetValue(oVoucher.appConn, "vdmgia4", "gia_nt2", "ma_vt='" + view.Item("ma_vt") + "' AND ma_kh='" + Me.txtMa_kh.Text + "' AND ma_nt='" + Me.cmdMa_nt.Text + "'")
            view.Item("gia2") = Math.Round(view.Item("gia_nt2") * Me.txtTy_gia.Value, 0)
            view = Nothing
        End Sub

        Private Sub WhenSiteEnter(ByVal sender As Object, ByVal e As EventArgs)
            Me.cOldSite = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        End Sub

        Private Sub WhenSiteLeave(ByVal sender As Object, ByVal e As EventArgs)
            If (Me.grdDetail.CurrentRowIndex >= 0) Then
                Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
                Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not ((StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.cOldSite), False) = 0) And Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ten_kho")), "C")) Then
                    If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkho", "dai_ly_yn", ("ma_kho = '" & str & "'"))) Then
                        Dim str3 As String = Strings.Trim(StringType.FromObject(view.Item("ma_vt")))
                        Dim sLeft As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "tk_dl", ("ma_vt = '" & str3 & "'"))))
                        If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                            view.Item("tk_vt") = sLeft
                        End If
                    End If
                    view = Nothing
                End If
            End If
        End Sub

        Private Sub WhenUOMEnter(ByVal sender As Object, ByVal e As EventArgs)
            On Error Resume Next
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then

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
            End If
            view = Nothing
        End Sub

        Private Sub WhenUOMLeave(ByVal sender As Object, ByVal e As EventArgs)
            On Error Resume Next
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
                If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "'"))) Then
                    Dim cKey As String = String.Concat(New String() {"(ma_vt = '", Strings.Trim(StringType.FromObject(view.Item("ma_vt"))), "' OR ma_vt = '*') AND dvt = N'", Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))), "'"})
                    Dim num As Decimal = DecimalType.FromObject(Sql.GetValue((modVoucher.appConn), "dmqddvt", "he_so", cKey))
                    view.Item("He_so") = num
                End If
            End If
            view = Nothing
        End Sub

        Private Sub RetrieveItems(ByVal sender As Object, ByVal e As EventArgs)
            Select Case IntegerType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing))
                Case 0
                    Me.RetrieveItemsFromSalseDone()
                    Exit Select
            End Select
        End Sub
        Private Sub RetrieveItemsFromSalseDone()
            If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
                If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                    Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("044")), 2)
                Else
                    Dim tcSQL As String = String.Concat(New String() {"EXEC spSearchSVTran4SQ '", modVoucher.cLan, "', '", Strings.Trim(Me.txtMa_kh.Text), "'"})
                    Dim ds As New DataSet
                    Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", ds)
                    If (ds.Tables.Item(0).Rows.Count <= 0) Then
                        Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
                    Else
                        Dim tbl As DataTable
                        tbl = ds.Tables(0)
                        Dim num6 As Integer = (tbl.Rows.Count - 1)
                        Dim Index As Integer = 0
                        Do While (index <= num6)
                            With tbl.Rows.Item(index)
                                If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                    .Item("stt_rec") = ""
                                Else
                                    .Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                                End If
                                tbl.Rows.Item(Index).AcceptChanges()
                            End With
                            index += 1
                        Loop

                        While modVoucher.tblDetail.Count > 0
                            modVoucher.tblDetail.Item(modVoucher.tblDetail.Count - 1).Delete()
                        End While

                        AppendFrom(modVoucher.tblDetail, tbl)
                        Dim count As Integer = modVoucher.tblDetail.Count
                        Index = (count - 1)
                        Do While (index >= 0)
                            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("ma_vt")), "C") Then
                                modVoucher.tblDetail.Item(index).Delete()
                            Else
                                modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                            End If
                            index = (index + -1)
                        Loop
                        Me.UpdateList()
                    End If
                    ds = Nothing
                End If
            End If
        End Sub
        ' Properties
#Region "Declare control and Varriable"
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
        Friend WithEvents grdCharge As clsgrid
        Friend WithEvents grdDetail As clsgrid
        Friend WithEvents Label1 As Label
        Friend WithEvents lblAction As Label
        Friend WithEvents lblDia_chi As Label
        Friend WithEvents lblMa_dc As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMa_htvc As Label
        Friend WithEvents lblMa_kh As Label
        Friend WithEvents lblMa_tt As Label
        Friend WithEvents lblNgay_ct As Label
        Friend WithEvents lblNgay_hl As Label
        Friend WithEvents lblNgay_lct As Label
        Friend WithEvents lblOng_ba As Label
        Friend WithEvents lblSo_ct As Label
        Friend WithEvents lblStatus As Label
        Friend WithEvents lblStatusMess As Label
        Friend WithEvents lblTen As Label
        Friend WithEvents lblTen_dc As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTen_htvc As Label
        Friend WithEvents lblTen_kh As Label
        Friend WithEvents lblTen_tt As Label
        Friend WithEvents lblTien_ck As Label
        Friend WithEvents lblTotal As Label
        Friend WithEvents lblTy_gia As Label
        Friend WithEvents lvlT_cp As Label
        Friend WithEvents tbDetail As TabControl
        Friend WithEvents tbgCharge As TabPage
        Friend WithEvents tbgOthers As TabPage
        Friend WithEvents tpgDetail As TabPage
        Friend WithEvents txtDien_giai As TextBox
        Friend WithEvents txtKeyPress As TextBox
        Friend WithEvents txtLoai_ct As TextBox
        Friend WithEvents txtMa_dc As TextBox
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtMa_htvc As TextBox
        Friend WithEvents txtMa_kh As TextBox
        Friend WithEvents txtMa_tt As TextBox
        Friend WithEvents txtNgay_ct As txtDate
        Friend WithEvents txtNgay_hl As txtDate
        Friend WithEvents txtNgay_lct As txtDate
        Friend WithEvents txtOng_ba As TextBox
        Friend WithEvents txtSo_ct As TextBox
        Friend WithEvents txtStatus As TextBox
        Friend WithEvents txtT_ck As txtNumeric
        Friend WithEvents txtT_ck_nt As txtNumeric
        Friend WithEvents txtT_cp As txtNumeric
        Friend WithEvents txtT_cp_nt As txtNumeric
        Friend WithEvents txtT_so_luong As txtNumeric
        Friend WithEvents txtT_tien_nt2 As txtNumeric
        Friend WithEvents txtT_tien2 As txtNumeric
        Friend WithEvents txtTy_gia As txtNumeric

        Public arrControlButtons As Button()
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
        Private colMa_thue As DataGridTextBoxColumn
        Private colMa_vt As DataGridTextBoxColumn
        Private colSo_luong As DataGridTextBoxColumn
        Private colTen_vt As DataGridTextBoxColumn
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
        Private noldTien_nt2 As Decimal
        Private noldTien2 As Decimal
        Private oInvItemDetail As VoucherLibObj
        Private oldtblDetail As DataTable
        Private oSecurity As clssecurity
        Private oSite As VoucherKeyLibObj
        Private oSOAddress As dirblanklib
        Private oTaxCodeDetail As VoucherLibObj
        'Private oTitleButton As TitleButton
        Private oUOM As VoucherKeyCheckLibObj
        Public oVoucher As clsvoucher.clsVoucher
        Private pn As StatusBarPanel
        Public pnContent As StatusBarPanel
        Private sOldString As String
        Private sOldStringDvt As String
        Private sOldStringMa_kho As String
        Private sOldStringMa_vt As String
        Private sOldStringSo_luong As String
        Private tblHandling As DataTable
        Private tblStatus As DataTable
        Private xInventory As clsInventory
        Private colThue, colThue_nt As DataGridTextBoxColumn
        Private nOldThue As Decimal
        Private nOldThue_nt As Decimal
#End Region
    End Class
End Namespace

