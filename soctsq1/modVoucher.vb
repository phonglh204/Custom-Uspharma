Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports libscommon

Namespace soctsq1
    <StandardModule> _
    Friend NotInheritable Class modVoucher
        ' Methods
        <STAThread> _
        Public Shared Sub main(ByVal CmdArgs As String())
            If BooleanType.FromObject(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0))) Then
                ProjectData.EndApp
            End If
            modVoucher.sysConn = Sys.GetSysConn
            modVoucher.appConn = Sys.GetConn
            If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(modVoucher.sysConn, "Access")) Then
                modVoucher.sysConn.Close
                modVoucher.sysConn = Nothing
                ProjectData.EndApp
            End If
            Control.CheckForIllegalCrossThreadCalls = False
            Sys.InitVar(modVoucher.sysConn, modVoucher.oVar)
            Sys.InitOptions(modVoucher.appConn, modVoucher.oOption)
            Sys.InitMessage(modVoucher.sysConn, modVoucher.oLan, "SQTran")
            Sys.InitColumns(modVoucher.sysConn, modVoucher.oLen)
            modVoucher.cIDVoucher = ""
            modVoucher.oVoucherRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmct", ("ma_ct = '" & Fox.GetWordNum(CmdArgs(0), 1, "#"c) & "'")), DataRow)
            If (Strings.InStr(CmdArgs(0), "#", CompareMethod.Binary) > 0) Then
                modVoucher.cIDVoucher = Fox.GetWordNum(CmdArgs(0), 2, "#"c)
            End If
            modVoucher.VoucherCode = StringType.FromObject(modVoucher.oVoucherRow.Item("ma_ct"))
            modVoucher.cLan = StringType.FromObject(Reg.GetRegistryKey("Language"))
            Dim index As Integer = 0
            Do
                modVoucher.tbcDetail(index) = New DataGridTextBoxColumn
                modVoucher.tbcCharge(index) = New DataGridTextBoxColumn
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
        Public Shared alCharge As String
        Public Shared alDetail As String
        Public Shared alMaster As String
        Public Shared appConn As SqlConnection
        Public Shared cIDVoucher As String
        Public Shared cLan As String
        Public Shared dsMain As DataSet = New DataSet
        Public Shared frmMain As frmVoucher
        Public Const MaxColumns As Integer = 45
        Public Shared oLan As Collection = New Collection
        Public Shared oLen As Collection = New Collection
        Public Shared oOption As Collection = New Collection
        Public Shared oVar As Collection = New Collection
        Public Shared oVoucherRow As DataRow
        Public Shared sysConn As SqlConnection
        Public Const SysID As String = "SQTran"
        Public Shared tbcCharge As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns) {}
        Public Shared tbcDetail As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns) {}
        Public Shared tblCharge As DataView = New DataView
        Public Shared tblDetail As DataView = New DataView
        Public Shared tblMaster As DataView = New DataView
        Public Shared tbsCharge As DataGridTableStyle = New DataGridTableStyle
        Public Shared tbsDetail As DataGridTableStyle = New DataGridTableStyle
        Public Shared VoucherCode As String
    End Class
End Namespace

