Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports libscommon


Namespace inctpxa
    Module modVoucher
        ' Methods
        <STAThread()> _
        Public Sub main(ByVal CmdArgs As String())
            If BooleanType.FromObject(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0))) Then
                ProjectData.EndApp()
            End If
            modVoucher.sysConn = Sys.GetSysConn
            modVoucher.appConn = Sys.GetConn
            If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(modVoucher.sysConn, "Access")) Then
                modVoucher.sysConn.Close()
                modVoucher.sysConn = Nothing
                ProjectData.EndApp()
            End If
            cIDVoucher = ""
            oVoucherCode = Fox.GetWordNum(CmdArgs(0), 1, "#"c)
            oVoucherRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmct", ("ma_ct = '" & oVoucherCode & "'")), DataRow)
            If (Strings.InStr(CmdArgs(0), "#", CompareMethod.Binary) > 0) Then
                modVoucher.cIDVoucher = Fox.GetWordNum(CmdArgs(0), 2, "#"c)
            End If
            modVoucher.VoucherCode = StringType.FromObject(modVoucher.oVoucherRow.Item("ma_ct"))
            modVoucher.cLan = StringType.FromObject(Reg.GetRegistryKey("Language"))
            If modVoucher.oVoucherRow.Item("s1").ToString.Trim <> "" Then
                modVoucher.SysID = modVoucher.oVoucherRow.Item("s1").ToString.Trim
            End If
            If modVoucher.oVoucherRow.Item("gc_td2").ToString.Trim <> "" Then
                modVoucher.grdMaster_code = modVoucher.oVoucherRow.Item("gc_td2").ToString.Trim
            End If
            If modVoucher.oVoucherRow.Item("gc_td3").ToString.Trim <> "" Then
                modVoucher.grdDetail_code = modVoucher.oVoucherRow.Item("gc_td3").ToString.Trim
            End If


            Sys.InitVar(modVoucher.sysConn, modVoucher.oVar)
            Sys.InitOptions(modVoucher.appConn, modVoucher.oOption)
            Sys.InitMessage(modVoucher.sysConn, modVoucher.oLan, modVoucher.SysID)
            Sys.InitColumns(modVoucher.sysConn, modVoucher.oLen)
            Dim index As Integer = 0
            Do
                modVoucher.tbcDetail(index) = New DataGridTextBoxColumn
                index += 1
            Loop While (index < MaxColumns)
            modVoucher.frmMain = New frmVoucher
            modVoucher.frmMain.ShowDialog()
            modVoucher.sysConn.Close()
            modVoucher.sysConn = Nothing
            modVoucher.appConn.Close()
            modVoucher.appConn = Nothing
        End Sub


        ' Fields
        Public alDetail As String
        Public alMaster As String
        Public alOther As String
        Public appConn As SqlConnection
        Public cIDVoucher As String
        Public cLan As String
        Public dsMain As DataSet = New DataSet
        Public frmMain As frmVoucher
        Public Const MaxColumns As Integer = 40
        Public oLan As Collection = New Collection
        Public oLen As Collection = New Collection
        Public oOption As Collection = New Collection
        Public oVar As Collection = New Collection
        Public oVoucherRow As DataRow
        Public oVoucherCode As String
        Public sysConn As SqlConnection
        Public SysID As String = "ISTran"
        Public tbcDetail As DataGridTextBoxColumn() = New DataGridTextBoxColumn(40) {}
        Public tblDetail As DataView = New DataView
        Public tblMaster As DataView = New DataView
        Public tbsDetail As DataGridTableStyle = New DataGridTableStyle
        Public VoucherCode As String, grdMaster_code As String, grdDetail_code As String
    End Module
End Namespace

