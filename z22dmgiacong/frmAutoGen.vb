Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon
Imports libscontrol.voucherseachlib

Namespace z22dmgiacong
    Public Class frmAutoGen
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmnamdn_Load)
            Me.InitializeComponent()
        End Sub

        Private Function CheckFieldEmpty(ByVal frm As Form) As Boolean
            Dim objectValue As Object
            For Each objectValue In frm.Controls
                If ((Not LateBinding.LateGet(objectValue, Nothing, "Tag", New Object(0 - 1) {}, Nothing, Nothing) Is Nothing) AndAlso (LateBinding.LateGet(objectValue, Nothing, "Tag", New Object(0 - 1) {}, Nothing, Nothing).ToString.IndexOf("NB") > -1)) Then
                    Dim sLeft As String = LateBinding.LateGet(objectValue, Nothing, "Tag", New Object(0 - 1) {}, Nothing, Nothing).ToString.Substring(0, 2)
                    If (StringType.StrCmp(sLeft, "FC", False) = 0) Then
                        If (ObjectType.ObjTst(LateBinding.LateGet(objectValue, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing), "", False) = 0) Then
                            Msg.Alert(StringType.FromObject(DirMain.oDirFormLib.oVar.Item("m_not_blank")), 2)
                            LateBinding.LateCall(objectValue, Nothing, "focus", New Object(0 - 1) {}, Nothing, Nothing)
                            Return False
                        End If
                    ElseIf ((StringType.StrCmp(sLeft, "FD", False) = 0) AndAlso (ObjectType.ObjTst(LateBinding.LateGet(objectValue, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing), Fox.GetEmptyDate, False) = 0)) Then
                        Msg.Alert(StringType.FromObject(DirMain.oDirFormLib.oVar.Item("m_not_blank")), 2)
                        LateBinding.LateCall(objectValue, Nothing, "focus", New Object(0 - 1) {}, Nothing, Nothing)
                        Return False
                    End If
                End If
            Next
            Return True
        End Function

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Me.Close()
        End Sub

        Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
            If Me.CheckFieldEmpty(Me) Then
                If (Me.txtcong_suat.Value <= 0) Then
                    Msg.Alert(StringType.FromObject(DirMain.oDirFormLib.oLan.Item("100")), 2)
                    Me.txtcong_suat.Focus()
                Else
                    Dim str As String = "EXEC fs_CRPGetItem "
                    str = StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(str, Sql.ConvertVS2SQLType(Me.txtma_vt.Text, ""))), ObjectType.AddObj(",", Sql.ConvertVS2SQLType(Me.txtNh_vt1.Text, "")))), ObjectType.AddObj(",", Sql.ConvertVS2SQLType(Me.txtNh_vt2.Text, "")))), ObjectType.AddObj(",", Sql.ConvertVS2SQLType(Me.txtNh_vt3.Text, "")))), ObjectType.AddObj(",", Sql.ConvertVS2SQLType(Me.txtngay_hl.Value, "")))), ObjectType.AddObj(",", Sql.ConvertVS2SQLType(Me.txtcong_suat.Value, "")))), ObjectType.AddObj(",", Reg.GetRegistryKey("CurrUserID"))))
                    Dim ds As New DataSet
                    Sql.SQLRetrieve((DirMain.oDirFormLib.appConn), str, "Filter", (ds))
                    DirMain.tblFilter = New DataView(ds.Tables.Item(0))
                    If (DirMain.tblFilter.Count = 0) Then
                        Msg.Alert(StringType.FromObject(DirMain.oDirFormLib.oLan.Item("201")), 2)
                    Else
                        Dim filter As New frmFilter
                        If (filter.ShowDialog = DialogResult.OK) Then
                            Dim num4 As Integer = (DirMain.tblFilter.Count - 1)
                            Dim i As Integer = 0
                            Do While (i <= num4)
                                If (ObjectType.ObjTst(DirMain.tblFilter.Item(i).Item("flag"), True, False) = 0) Then
                                    Dim toRow As DataRow = DirMain.oDirFormLib.oDir.ob.dv.Table.NewRow
                                    Dim num3 As Integer = (DirMain.oDirFormLib.oDir.ob.dv.Table.Columns.Count - 1)
                                    Dim j As Integer = 0
                                    Do While (j <= num3)
                                        Try
                                            toRow.Item(DirMain.oDirFormLib.oDir.ob.dv.Table.Columns.Item(j).ColumnName) = RuntimeHelpers.GetObjectValue(DirMain.tblFilter.Item(i).Item(DirMain.oDirFormLib.oDir.ob.dv.Table.Columns.Item(j).ColumnName))
                                        Catch exception1 As Exception
                                            ProjectData.SetProjectError(exception1)
                                            ProjectData.ClearProjectError()
                                        End Try
                                        j += 1
                                    Loop
                                    Sql.SQLInsert((DirMain.oDirFormLib.appConn), "crdmtgsp", toRow)
                                    DirMain.oDirFormLib.oDir.ob.dv.Table.Rows.Add(toRow)
                                End If
                                i += 1
                            Loop
                            Me.Close()
                        End If
                    End If
                End If
            End If
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub frmnamdn_Load(ByVal sender As Object, ByVal e As EventArgs)
            Obj.Init(Me)
            Dim control As Control
            For Each control In Me.Controls
                If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                    control.Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
                End If
                If (Strings.InStr(StringType.FromObject(control.Tag), "ML", CompareMethod.Binary) > 0) Then
                    Dim obj2 As Object = Strings.Right(control.Name, (control.Name.Length - 3))
                    Dim box As TextBox = DirectCast(control, TextBox)
                    box.MaxLength = IntegerType.FromObject(DirMain.oDirFormLib.oLen.Item(RuntimeHelpers.GetObjectValue(obj2)))
                End If
            Next
            Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtma_vt, Me.lblten_vt, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "loai_vt IN (41, 51)", True, Me.cmdCancel)
            Dim dlnh_vt1 As New DirLib(Me.txtNh_vt1, Me.lblTen_nh_vt1, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh = 1", True, Me.cmdCancel)
            Dim lib2 As New DirLib(Me.txtNh_vt2, Me.lblTen_nh_vt2, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmnhvt", "ma_nh", "ten_nh", "ItemGroup2", "loai_nh = 2", True, Me.cmdCancel)
            Dim lib3 As New DirLib(Me.txtNh_vt3, Me.lblTen_nh_vt3, DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, "dmnhvt", "ma_nh", "ten_nh", "ItemGroup3", "loai_nh = 3", True, Me.cmdCancel)
            Me.Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("200"))
            Me.txtcong_suat.Format = StringType.FromObject(DirMain.oDirFormLib.oVar.Item("m_ip_cs"))
            Me.txtcong_suat.Value = 0
            Me.txtma_vt.Text = ""
            Me.txtNh_vt1.Text = ""
            Me.txtNh_vt2.Text = ""
            Me.txtNh_vt3.Text = ""
        End Sub

        <DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.cmdOk = New Button
            Me.cmdCancel = New Button
            Me.grpInfor = New GroupBox
            Me.GroupBox2 = New GroupBox
            Me.lblma_vt = New Label
            Me.txtma_vt = New TextBox
            Me.lblngay_hl = New Label
            Me.txtngay_hl = New txtDate
            Me.lblcong_suat = New Label
            Me.txtcong_suat = New txtNumeric
            Me.lblten_vt = New Label
            Me.Label5 = New Label
            Me.txtNh_vt3 = New TextBox
            Me.lblTen_nh_vt3 = New Label
            Me.Label3 = New Label
            Me.txtNh_vt2 = New TextBox
            Me.lblTen_nh_vt2 = New Label
            Me.lblNh_vt1 = New Label
            Me.txtNh_vt1 = New TextBox
            Me.lblTen_nh_vt1 = New Label
            Me.SuspendLayout()
            Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdOk.Location = New Point(8, &HC9)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 6
            Me.cmdOk.Tag = "L015"
            Me.cmdOk.Text = "Nhan"
            Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New Point(&H54, &HC9)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 7
            Me.cmdCancel.Tag = "L016"
            Me.cmdCancel.Text = "Huy"
            Me.grpInfor.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Top))
            Me.grpInfor.Location = New Point(8, 5)
            Me.grpInfor.Name = "grpInfor"
            Me.grpInfor.Size = New Size(&H250, &H73)
            Me.grpInfor.TabIndex = &H11
            Me.grpInfor.TabStop = False
            Me.GroupBox2.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.GroupBox2.Location = New Point(8, 120)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New Size(&H250, &H48)
            Me.GroupBox2.TabIndex = &H12
            Me.GroupBox2.TabStop = False
            Me.lblma_vt.AutoSize = True
            Me.lblma_vt.Location = New Point(&H17, &H17)
            Me.lblma_vt.Name = "lblma_vt"
            Me.lblma_vt.Size = New Size(&H49, &H10)
            Me.lblma_vt.TabIndex = &H9B
            Me.lblma_vt.Tag = "L001"
            Me.lblma_vt.Text = "Ma san pham"
            Me.lblma_vt.TextAlign = ContentAlignment.BottomLeft
            Me.txtma_vt.CharacterCasing = CharacterCasing.Upper
            Me.txtma_vt.Location = New Point(&H9B, &H15)
            Me.txtma_vt.Name = "txtma_vt"
            Me.txtma_vt.TabIndex = 0
            Me.txtma_vt.Tag = "FCDF"
            Me.txtma_vt.Text = "TXTMA_VT"
            Me.lblngay_hl.AutoSize = True
            Me.lblngay_hl.Location = New Point(&H17, &H8A)
            Me.lblngay_hl.Name = "lblngay_hl"
            Me.lblngay_hl.Size = New Size(&H3A, &H10)
            Me.lblngay_hl.TabIndex = &H9D
            Me.lblngay_hl.Tag = "L002"
            Me.lblngay_hl.Text = "Hieu luc tu"
            Me.lblngay_hl.TextAlign = ContentAlignment.BottomLeft
            Me.txtngay_hl.Location = New Point(&H9B, &H88)
            Me.txtngay_hl.MaxLength = 10
            Me.txtngay_hl.Name = "txtngay_hl"
            Me.txtngay_hl.TabIndex = 4
            Me.txtngay_hl.Tag = "FDNBDF"
            Me.txtngay_hl.Text = "  /  /    "
            Me.txtngay_hl.TextAlign = HorizontalAlignment.Right
            Me.txtngay_hl.Value = New DateTime(0)
            Me.lblcong_suat.AutoSize = True
            Me.lblcong_suat.Location = New Point(&H17, &HA1)
            Me.lblcong_suat.Name = "lblcong_suat"
            Me.lblcong_suat.Size = New Size(&H40, &H10)
            Me.lblcong_suat.TabIndex = &H9F
            Me.lblcong_suat.Tag = "L004"
            Me.lblcong_suat.Text = "Dinh muc tg"
            Me.lblcong_suat.TextAlign = ContentAlignment.BottomLeft
            Me.txtcong_suat.Format = "m_ip_cs"
            Me.txtcong_suat.Location = New Point(&H9B, &H9F)
            Me.txtcong_suat.MaxLength = 8
            Me.txtcong_suat.Name = "txtcong_suat"
            Me.txtcong_suat.TabIndex = 5
            Me.txtcong_suat.Tag = "FNDF"
            Me.txtcong_suat.Text = "m_ip_cs"
            Me.txtcong_suat.TextAlign = HorizontalAlignment.Right
            Me.txtcong_suat.Value = 0
            Me.lblten_vt.AutoSize = True
            Me.lblten_vt.Location = New Point(&H103, &H17)
            Me.lblten_vt.Name = "lblten_vt"
            Me.lblten_vt.Size = New Size(&H24, &H10)
            Me.lblten_vt.TabIndex = &H99
            Me.lblten_vt.Tag = ""
            Me.lblten_vt.Text = "Ten vt"
            Me.lblten_vt.TextAlign = ContentAlignment.BottomLeft
            Me.Label5.AutoSize = True
            Me.Label5.Location = New Point(&H17, &H5C)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New Size(&H41, &H10)
            Me.Label5.TabIndex = &HA7
            Me.Label5.Tag = "L012"
            Me.Label5.Text = "Nhom vat tu"
            Me.txtNh_vt3.CharacterCasing = CharacterCasing.Upper
            Me.txtNh_vt3.Location = New Point(&H9B, 90)
            Me.txtNh_vt3.Name = "txtNh_vt3"
            Me.txtNh_vt3.TabIndex = 3
            Me.txtNh_vt3.Tag = "FCDF"
            Me.txtNh_vt3.Text = "TXTNH_VT3"
            Me.lblTen_nh_vt3.AutoSize = True
            Me.lblTen_nh_vt3.Location = New Point(&H103, &H5C)
            Me.lblTen_nh_vt3.Name = "lblTen_nh_vt3"
            Me.lblTen_nh_vt3.Size = New Size(&H56, &H10)
            Me.lblTen_nh_vt3.TabIndex = &HA8
            Me.lblTen_nh_vt3.Text = "Ten nhom vat tu"
            Me.Label3.AutoSize = True
            Me.Label3.Location = New Point(&H17, &H45)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New Size(&H41, &H10)
            Me.Label3.TabIndex = &HA5
            Me.Label3.Tag = "L011"
            Me.Label3.Text = "Nhom vat tu"
            Me.txtNh_vt2.CharacterCasing = CharacterCasing.Upper
            Me.txtNh_vt2.Location = New Point(&H9B, &H43)
            Me.txtNh_vt2.Name = "txtNh_vt2"
            Me.txtNh_vt2.TabIndex = 2
            Me.txtNh_vt2.Tag = "FCDF"
            Me.txtNh_vt2.Text = "TXTNH_VT2"
            Me.lblTen_nh_vt2.AutoSize = True
            Me.lblTen_nh_vt2.Location = New Point(&H103, &H45)
            Me.lblTen_nh_vt2.Name = "lblTen_nh_vt2"
            Me.lblTen_nh_vt2.Size = New Size(&H56, &H10)
            Me.lblTen_nh_vt2.TabIndex = &HA6
            Me.lblTen_nh_vt2.Text = "Ten nhom vat tu"
            Me.lblNh_vt1.AutoSize = True
            Me.lblNh_vt1.Location = New Point(&H17, &H2E)
            Me.lblNh_vt1.Name = "lblNh_vt1"
            Me.lblNh_vt1.Size = New Size(&H41, &H10)
            Me.lblNh_vt1.TabIndex = &HA3
            Me.lblNh_vt1.Tag = "L010"
            Me.lblNh_vt1.Text = "Nhom vat tu"
            Me.txtNh_vt1.CharacterCasing = CharacterCasing.Upper
            Me.txtNh_vt1.Location = New Point(&H9B, &H2C)
            Me.txtNh_vt1.Name = "txtNh_vt1"
            Me.txtNh_vt1.TabIndex = 1
            Me.txtNh_vt1.Tag = "FCDF"
            Me.txtNh_vt1.Text = "TXTNH_VT1"
            Me.lblTen_nh_vt1.AutoSize = True
            Me.lblTen_nh_vt1.Location = New Point(&H103, &H2E)
            Me.lblTen_nh_vt1.Name = "lblTen_nh_vt1"
            Me.lblTen_nh_vt1.Size = New Size(&H56, &H10)
            Me.lblTen_nh_vt1.TabIndex = &HA4
            Me.lblTen_nh_vt1.Text = "Ten nhom vat tu"
            Me.AutoScaleBaseSize = New Size(5, 13)
            Me.ClientSize = New Size(&H260, &HE5)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.txtNh_vt3)
            Me.Controls.Add(Me.lblTen_nh_vt3)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.txtNh_vt2)
            Me.Controls.Add(Me.lblTen_nh_vt2)
            Me.Controls.Add(Me.lblNh_vt1)
            Me.Controls.Add(Me.txtNh_vt1)
            Me.Controls.Add(Me.lblTen_nh_vt1)
            Me.Controls.Add(Me.lblma_vt)
            Me.Controls.Add(Me.txtma_vt)
            Me.Controls.Add(Me.lblngay_hl)
            Me.Controls.Add(Me.txtngay_hl)
            Me.Controls.Add(Me.lblcong_suat)
            Me.Controls.Add(Me.txtcong_suat)
            Me.Controls.Add(Me.lblten_vt)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.grpInfor)
            Me.Controls.Add(Me.GroupBox2)
            Me.Name = "frmAutoGen"
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "frmAutoGen"
            Me.ResumeLayout(False)
        End Sub


        ' Properties
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents GroupBox2 As GroupBox
        Friend WithEvents grpInfor As GroupBox
        Friend WithEvents Label3 As Label
        Friend WithEvents Label5 As Label
        Friend WithEvents lblcong_suat As Label
        Friend WithEvents lblma_vt As Label
        Friend WithEvents lblngay_hl As Label
        Friend WithEvents lblNh_vt1 As Label
        Friend WithEvents lblTen_nh_vt1 As Label
        Friend WithEvents lblTen_nh_vt2 As Label
        Friend WithEvents lblTen_nh_vt3 As Label
        Friend WithEvents lblten_vt As Label
        Friend WithEvents txtcong_suat As txtNumeric
        Friend WithEvents txtma_vt As TextBox
        Friend WithEvents txtngay_hl As txtDate
        Friend WithEvents txtNh_vt1 As TextBox
        Friend WithEvents txtNh_vt2 As TextBox
        Friend WithEvents txtNh_vt3 As TextBox

        Private components As IContainer
    End Class
End Namespace

