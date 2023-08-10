Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol
Imports libscontrol.voucherseachlib
Imports libscontrol.clsvoucher.clsVoucher


Namespace coctdm1
    Public Class frmSearch
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmSearch_Load)
            Me.InitializeComponent()
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Me.Close()
        End Sub

        Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
            Dim flag As Boolean
            Dim num5 As Integer = IntegerType.FromObject(modVoucher.oLen.Item("so_ct1"))
            Dim expression As String = ("(a.ma_ct = '" & modVoucher.VoucherCode & "')")
            Dim strSQLLong As String = expression
            If (StringType.StrCmp(Me.txtLoc_nsd.Text, "1", False) = 0) Then
                strSQLLong = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj((strSQLLong & " AND (a.user_id0 = "), Reg.GetRegistryKey("CurrUserID")), ")"))
            End If
            Dim str As String = expression
            Dim num8 As Integer = (Me.Controls.Count - 1)
            Dim num2 As Integer = 0
            Do While (num2 <= num8)
                If ((Strings.InStr(StringType.FromObject(Me.Controls.Item(num2).Tag), "Master", CompareMethod.Binary) > 0) Or (Strings.InStr(StringType.FromObject(Me.Controls.Item(num2).Tag), "Detail", CompareMethod.Binary) > 0)) Then
                    flag = False
                    expression = Fox.GetWordNum(StringType.FromObject(Me.Controls.Item(num2).Tag), 2, "#"c)
                    If Me.Controls.Item(num2).GetType Is GetType(txtNumeric) Then
                        Dim numeric As txtNumeric = DirectCast(Me.Controls.Item(num2), txtNumeric)
                        If (numeric.Value <> 0) Then
                            expression = Strings.Replace(expression, "%n", StringType.FromObject(Sql.ConvertVS2SQLType(numeric.Value, "")), 1, -1, CompareMethod.Binary)
                        Else
                            expression = ""
                        End If
                        flag = True
                    End If
                    If Me.Controls.Item(num2).GetType Is GetType(txtDate) Then
                        Dim _date As txtDate = DirectCast(Me.Controls.Item(num2), txtDate)
                        If (ObjectType.ObjTst(_date.Text, Fox.GetEmptyDate, False) <> 0) Then
                            expression = Strings.Replace(expression, "%d", StringType.FromObject(Sql.ConvertVS2SQLType(_date.Value, "")), 1, -1, CompareMethod.Binary)
                        Else
                            expression = ""
                        End If
                        flag = True
                    End If
                    If Not flag Then
                        Dim box As TextBox = DirectCast(Me.Controls.Item(num2), TextBox)
                        If (StringType.StrCmp(Strings.Trim(box.Text), "", False) <> 0) Then
                            If (Strings.InStr(StringType.FromObject(Me.Controls.Item(num2).Tag), "FC", CompareMethod.Binary) > 0) Then
                                expression = Strings.Replace(expression, "%s", Strings.Trim(Strings.Replace(box.Text, "'", "", 1, -1, CompareMethod.Binary)), 1, -1, CompareMethod.Binary)
                            End If
                            If (Strings.InStr(StringType.FromObject(Me.Controls.Item(num2).Tag), "FN", CompareMethod.Binary) > 0) Then
                                expression = Strings.Replace(expression, "%n", box.Text, 1, -1, CompareMethod.Binary)
                            End If
                        Else
                            expression = ""
                        End If
                    End If
                End If
                If ((Strings.InStr(StringType.FromObject(Me.Controls.Item(num2).Tag), "Master", CompareMethod.Binary) > 0) And (StringType.StrCmp(Strings.Trim(expression), "", False) <> 0)) Then
                    If (Strings.InStr(expression, "dbo.", CompareMethod.Binary) > 0) Then
                        strSQLLong = (strSQLLong & " AND (" & expression & ")")
                    Else
                        strSQLLong = (strSQLLong & " AND (a." & expression & ")")
                    End If
                End If
                If ((Strings.InStr(StringType.FromObject(Me.Controls.Item(num2).Tag), "Detail", CompareMethod.Binary) > 0) And (StringType.StrCmp(Strings.Trim(expression), "", False) <> 0)) Then
                    If (Strings.InStr(expression, "dbo.", CompareMethod.Binary) > 0) Then
                        str = (str & " AND (" & expression & ")")
                    Else
                        str = (str & " AND (a." & expression & ")")
                    End If
                End If
                num2 += 1
            Loop
            Dim num7 As Integer = (Me.tabFilter.TabPages.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num7)
                Dim num6 As Integer = (Me.tabFilter.TabPages.Item(i).Controls.Count - 1)
                num2 = 0
                Do While (num2 <= num6)
                    If ((Strings.InStr(StringType.FromObject(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag), "Master", CompareMethod.Binary) > 0) Or (Strings.InStr(StringType.FromObject(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag), "Detail", CompareMethod.Binary) > 0)) Then
                        flag = False
                        expression = Fox.GetWordNum(StringType.FromObject(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag), 2, "#"c)
                        If Me.tabFilter.TabPages.Item(i).Controls.Item(num2).GetType Is GetType(txtNumeric) Then
                            Dim numeric2 As txtNumeric = DirectCast(Me.tabFilter.TabPages.Item(i).Controls.Item(num2), txtNumeric)
                            If (numeric2.Value <> 0) Then
                                expression = Strings.Replace(expression, "%n", StringType.FromObject(Sql.ConvertVS2SQLType(numeric2.Value, "")), 1, -1, CompareMethod.Binary)
                            Else
                                expression = ""
                            End If
                            flag = True
                        End If
                        If Me.tabFilter.TabPages.Item(i).Controls.Item(num2).GetType Is GetType(txtDate) Then
                            Dim date2 As txtDate = DirectCast(Me.tabFilter.TabPages.Item(i).Controls.Item(num2), txtDate)
                            If (ObjectType.ObjTst(date2.Text, Fox.GetEmptyDate, False) <> 0) Then
                                expression = Strings.Replace(expression, "%d", StringType.FromObject(Sql.ConvertVS2SQLType(date2.Value, "")), 1, -1, CompareMethod.Binary)
                            Else
                                expression = ""
                            End If
                            flag = True
                        End If
                        If Not flag Then
                            Dim box2 As TextBox = DirectCast(Me.tabFilter.TabPages.Item(i).Controls.Item(num2), TextBox)
                            If (StringType.StrCmp(Strings.Trim(box2.Text), "", False) <> 0) Then
                                If (Strings.InStr(StringType.FromObject(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag), "FC", CompareMethod.Binary) > 0) Then
                                    expression = Strings.Replace(expression, "%s", Strings.Trim(Strings.Replace(box2.Text, "'", "", 1, -1, CompareMethod.Binary)), 1, -1, CompareMethod.Binary)
                                End If
                                If (Strings.InStr(StringType.FromObject(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag), "FN", CompareMethod.Binary) > 0) Then
                                    expression = Strings.Replace(expression, "%n", box2.Text, 1, -1, CompareMethod.Binary)
                                End If
                            Else
                                expression = ""
                            End If
                        End If
                    End If
                    If ((Strings.InStr(StringType.FromObject(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag), "Master", CompareMethod.Binary) > 0) And (StringType.StrCmp(Strings.Trim(expression), "", False) <> 0)) Then
                        If (Strings.InStr(expression, "dbo.", CompareMethod.Binary) > 0) Then
                            strSQLLong = (strSQLLong & " AND (" & expression & ")")
                        Else
                            strSQLLong = (strSQLLong & " AND (a." & expression & ")")
                        End If
                    End If
                    If ((Strings.InStr(StringType.FromObject(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag), "Detail", CompareMethod.Binary) > 0) And (StringType.StrCmp(Strings.Trim(expression), "", False) <> 0)) Then
                        If (Strings.InStr(expression, "dbo.", CompareMethod.Binary) > 0) Then
                            str = (str & " AND (" & expression & ")")
                        Else
                            str = (str & " AND (a." & expression & ")")
                        End If
                    End If
                    num2 += 1
                Loop
                i += 1
            Loop
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(String.Concat(New String() {"EXEC fs_SearchBMTran '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(str, 10), ", '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "'"}), ObjectType.AddObj(ObjectType.AddObj(", '", Reg.GetRegistryKey("SysData")), "'")))
            Dim ds As New DataSet
            Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "trantmp", (ds))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                Me.Close()
                modVoucher.frmMain.grdDetail.SuspendLayout()
                If (ObjectType.ObjTst(modVoucher.oOption.Item("m_search_type"), 0, False) = 0) Then
                    Dim num As Integer
                    modVoucher.tblDetail.RowFilter = ""
                    Dim num4 As Integer = (modVoucher.tblDetail.Count - 1)
                    num = num4
                    Do While (num >= 0)
                        modVoucher.tblDetail.Item(num).Delete()
                        num = (num + -1)
                    Loop
                    num4 = (modVoucher.tblMaster.Count - 1)
                    num = num4
                    Do While (num >= 0)
                        modVoucher.tblMaster.Item(num).Delete()
                        num = (num + -1)
                    Loop
                    AppendFrom(modVoucher.tblMaster, ds.Tables.Item(0))
                    AppendFrom(modVoucher.tblDetail, ds.Tables.Item(1))
                Else
                    modVoucher.tblMaster.Table = ds.Tables.Item(0)
                    modVoucher.tblDetail.Table = ds.Tables.Item(1)
                    modVoucher.frmMain.grdDetail.TableStyles.Item(0).MappingName = modVoucher.tblDetail.Table.ToString
                End If
                modVoucher.frmMain.iMasterRow = 0
                Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(modVoucher.frmMain.iMasterRow).Item("stt_rec")), "'")
                modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
                frmMain.oVoucher.cAction = "View"
                modVoucher.frmMain.grdDetail.ResumeLayout()
                If (modVoucher.tblMaster.Count = 1) Then
                    modVoucher.frmMain.RefrehForm()
                Else
                    modVoucher.frmMain.View()
                End If
                frmMain.oVoucher.RefreshButton(frmMain.oVoucher.ctrlButtons, frmMain.oVoucher.cAction)
                If (modVoucher.tblMaster.Count = 1) Then
                    modVoucher.frmMain.cmdEdit.Focus()
                End If
                ds = Nothing
            Else
                Msg.Alert(StringType.FromObject(frmMain.oVoucher.oClassMsg.Item("017")), 2)
                ds = Nothing
            End If
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub frmSearch_Load(ByVal sender As Object, ByVal e As EventArgs)
            vouchersearchlibobj.AddFreeFields(modVoucher.sysConn, Me.tabFilter.TabPages.Item(2), modVoucher.VoucherCode)
            vouchersearchlibobj.AddFreeCode(modVoucher.sysConn, Me.tabFilter.TabPages.Item(1), modVoucher.VoucherCode, modVoucher.sysConn, modVoucher.appConn, Me.cmdCancel)
            frmMain.oVoucher.frmSearch_Load(Me, oLen)
            Dim oUnit As New vouchersearchlibobj(Me.txtMa_bp, Me.lblTen_bp, modVoucher.sysConn, modVoucher.appConn, "vxdmbp", "ma_bp", "ten_bp", "v20CODept", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtMa_sp, Me.lblTen_sp, modVoucher.sysConn, modVoucher.appConn, "vdmsp2", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtMa_vt, Me.lblTen_vt, modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
            FormSetFocus2FirstControl(Me)
        End Sub

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.cmdOk = New Button
            Me.cmdCancel = New Button
            Me.grpMaster = New GroupBox
            Me.txtLoc_nsd = New TextBox
            Me.lblLoc_nsd = New Label
            Me.grdFilterUser = New GroupBox
            Me.grpDetail = New GroupBox
            Me.lblMa_bp = New Label
            Me.txtMa_bp = New TextBox
            Me.lblMa_sp = New Label
            Me.txtMa_sp = New TextBox
            Me.lblTen_sp = New Label
            Me.tabFilter = New TabControl
            Me.tabMain = New TabPage
            Me.txtMa_vt = New TextBox
            Me.lblTen_bp = New Label
            Me.lblTen_vt = New Label
            Me.lblMa_vt = New Label
            Me.tabFilter.SuspendLayout()
            Me.tabMain.SuspendLayout()
            Me.SuspendLayout()
            Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdOk.Location = New Point(0, 183)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 1
            Me.cmdOk.Tag = "L105"
            Me.cmdOk.Text = "Nhan"
            Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New Point(76, 183)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 2
            Me.cmdCancel.Tag = "L106"
            Me.cmdCancel.Text = "Huy"
            Me.grpMaster.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Top))
            Me.grpMaster.Location = New Point(8, 0)
            Me.grpMaster.Name = "grpMaster"
            Me.grpMaster.Size = New Size(586, 56)
            Me.grpMaster.TabIndex = 17
            Me.grpMaster.TabStop = False
            Me.txtLoc_nsd.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.txtLoc_nsd.CharacterCasing = CharacterCasing.Upper
            Me.txtLoc_nsd.Location = New Point(144, 106)
            Me.txtLoc_nsd.MaxLength = 1
            Me.txtLoc_nsd.Name = "txtLoc_nsd"
            Me.txtLoc_nsd.Size = New Size(24, 20)
            Me.txtLoc_nsd.TabIndex = 4
            Me.txtLoc_nsd.TabStop = False
            Me.txtLoc_nsd.Tag = "FC"
            Me.txtLoc_nsd.Text = "TXTLOC_NSD"
            Me.lblLoc_nsd.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.lblLoc_nsd.AutoSize = True
            Me.lblLoc_nsd.Location = New Point(16, 108)
            Me.lblLoc_nsd.Name = "lblLoc_nsd"
            Me.lblLoc_nsd.Size = New Size(101, 16)
            Me.lblLoc_nsd.TabIndex = 3
            Me.lblLoc_nsd.Tag = "L104"
            Me.lblLoc_nsd.Text = "Loc theo NSD (0/1)"
            Me.grdFilterUser.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
            Me.grdFilterUser.Location = New Point(8, 96)
            Me.grdFilterUser.Name = "grdFilterUser"
            Me.grdFilterUser.Size = New Size(586, 38)
            Me.grdFilterUser.TabIndex = 70
            Me.grdFilterUser.TabStop = False
            Me.grpDetail.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.grpDetail.Location = New Point(8, 56)
            Me.grpDetail.Name = "grpDetail"
            Me.grpDetail.Size = New Size(586, 40)
            Me.grpDetail.TabIndex = 69
            Me.grpDetail.TabStop = False
            Me.lblMa_bp.AutoSize = True
            Me.lblMa_bp.Location = New Point(16, 33)
            Me.lblMa_bp.Name = "lblMa_bp"
            Me.lblMa_bp.Size = New Size(64, 16)
            Me.lblMa_bp.TabIndex = 83
            Me.lblMa_bp.Tag = "L102"
            Me.lblMa_bp.Text = "Ma bo phan"
            Me.txtMa_bp.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_bp.Location = New Point(144, 31)
            Me.txtMa_bp.Name = "txtMa_bp"
            Me.txtMa_bp.TabIndex = 1
            Me.txtMa_bp.Tag = "FCMaster#ma_bp like '%s%'#ML"
            Me.txtMa_bp.Text = "TXTMA_BP"
            Me.lblMa_sp.AutoSize = True
            Me.lblMa_sp.Location = New Point(16, 12)
            Me.lblMa_sp.Name = "lblMa_sp"
            Me.lblMa_sp.Size = New Size(73, 16)
            Me.lblMa_sp.TabIndex = 89
            Me.lblMa_sp.Tag = "L101"
            Me.lblMa_sp.Text = "Ma san pham"
            Me.txtMa_sp.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_sp.Location = New Point(144, 10)
            Me.txtMa_sp.Name = "txtMa_sp"
            Me.txtMa_sp.TabIndex = 0
            Me.txtMa_sp.Tag = "FCMaster#ma_sp like '%s%'#ML"
            Me.txtMa_sp.Text = "TXTMA_SP"
            Me.lblTen_sp.AutoSize = True
            Me.lblTen_sp.Location = New Point(245, 12)
            Me.lblTen_sp.Name = "lblTen_sp"
            Me.lblTen_sp.Size = New Size(76, 16)
            Me.lblTen_sp.TabIndex = 96
            Me.lblTen_sp.Tag = ""
            Me.lblTen_sp.Text = "Ten san pham"
            Me.tabFilter.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.tabFilter.Controls.Add(Me.tabMain)
            Me.tabFilter.Location = New Point(0, 8)
            Me.tabFilter.Name = "tabFilter"
            Me.tabFilter.SelectedIndex = 0
            Me.tabFilter.Size = New Size(608, 168)
            Me.tabFilter.TabIndex = 0
            Me.tabMain.Controls.Add(Me.txtMa_vt)
            Me.tabMain.Controls.Add(Me.txtMa_sp)
            Me.tabMain.Controls.Add(Me.lblTen_bp)
            Me.tabMain.Controls.Add(Me.lblTen_vt)
            Me.tabMain.Controls.Add(Me.lblMa_vt)
            Me.tabMain.Controls.Add(Me.lblMa_bp)
            Me.tabMain.Controls.Add(Me.txtMa_bp)
            Me.tabMain.Controls.Add(Me.lblMa_sp)
            Me.tabMain.Controls.Add(Me.lblTen_sp)
            Me.tabMain.Controls.Add(Me.grpDetail)
            Me.tabMain.Controls.Add(Me.lblLoc_nsd)
            Me.tabMain.Controls.Add(Me.txtLoc_nsd)
            Me.tabMain.Controls.Add(Me.grdFilterUser)
            Me.tabMain.Controls.Add(Me.grpMaster)
            Me.tabMain.Location = New Point(4, 22)
            Me.tabMain.Name = "tabMain"
            Me.tabMain.Size = New Size(600, 142)
            Me.tabMain.TabIndex = 0
            Me.tabMain.Text = "Dieu kien loc"
            Me.txtMa_vt.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_vt.Location = New Point(144, 68)
            Me.txtMa_vt.Name = "txtMa_vt"
            Me.txtMa_vt.TabIndex = 2
            Me.txtMa_vt.Tag = "FCDetail#ma_vt like '%s%'#ML"
            Me.txtMa_vt.Text = "TXTMA_VT"
            Me.lblTen_bp.AutoSize = True
            Me.lblTen_bp.Location = New Point(245, 33)
            Me.lblTen_bp.Name = "lblTen_bp"
            Me.lblTen_bp.Size = New Size(68, 16)
            Me.lblTen_bp.TabIndex = 108
            Me.lblTen_bp.Tag = ""
            Me.lblTen_bp.Text = "Ten bo phan"
            Me.lblTen_vt.AutoSize = True
            Me.lblTen_vt.Location = New Point(245, 70)
            Me.lblTen_vt.Name = "lblTen_vt"
            Me.lblTen_vt.Size = New Size(54, 16)
            Me.lblTen_vt.TabIndex = 107
            Me.lblTen_vt.Tag = ""
            Me.lblTen_vt.Text = "Ten vat tu"
            Me.lblMa_vt.AutoSize = True
            Me.lblMa_vt.Location = New Point(16, 70)
            Me.lblMa_vt.Name = "lblMa_vt"
            Me.lblMa_vt.Size = New Size(50, 16)
            Me.lblMa_vt.TabIndex = 106
            Me.lblMa_vt.Tag = "L103"
            Me.lblMa_vt.Text = "Ma vat tu"
            Me.AutoScaleBaseSize = New Size(5, 13)
            Me.ClientSize = New Size(608, 213)
            Me.Controls.Add(Me.tabFilter)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Name = "frmSearch"
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "frmSearch"
            Me.tabFilter.ResumeLayout(False)
            Me.tabMain.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub


        ' Properties
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents grdFilterUser As GroupBox
        Friend WithEvents grpDetail As GroupBox
        Friend WithEvents grpMaster As GroupBox
        Friend WithEvents lblLoc_nsd As Label
        Friend WithEvents lblMa_bp As Label
        Friend WithEvents lblMa_sp As Label
        Friend WithEvents lblMa_vt As Label
        Friend WithEvents lblTen_bp As Label
        Friend WithEvents lblTen_sp As Label
        Friend WithEvents lblTen_vt As Label
        Friend WithEvents tabFilter As TabControl
        Friend WithEvents tabMain As TabPage
        Friend WithEvents txtLoc_nsd As TextBox
        Friend WithEvents txtMa_bp As TextBox
        Friend WithEvents txtMa_sp As TextBox
        Friend WithEvents txtMa_vt As TextBox

        Private components As IContainer
    End Class
End Namespace

