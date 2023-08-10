Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports libscontrol.balanceviewlib
Imports libscontrol
Imports libscommon

Namespace v20cobp
    Module DirMain
        ' Methods
        <STAThread()> _
        Public Sub main()
            oDirFormLib = New DirFormLibBrowse
            'Dim oRpt As New rpDir
            If (oDirFormLib.sysConn Is Nothing) Then
                ProjectData.EndApp()
            End If
            oDirFormLib.SysID = "v20CODept"
            oDirFormLib.Init()
            oDirFormLib.frmUpdate = New frmDirInfor
            'Dim browse As DirFormLibBrowse = DirMain.oDirFormLib
            'reports.SetReportVar(oDirFormLib.sysConn, oDirFormLib.appConn, oDirFormLib.SysID, oDirFormLib.oOptions, oRpt)
            'oDirFormLib.oRpt = oRpt
            oDirFormLib.Show()
            Dim ds As New DataSet
            Sql.SQLRetrieve((oDirFormLib.appConn), "EXEC fs20_CalLevelCODept", "d", (ds))
            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(ds.Tables.Item(0).Rows.Item(0).Item(0))), "", False) <> 0) Then
                Msg.Alert(Strings.Replace(StringType.FromObject(oDirFormLib.oLan.Item("301")), "%s", Strings.Trim(StringType.FromObject(ds.Tables.Item(0).Rows.Item(0).Item(0))), 1, -1, CompareMethod.Binary), 1)
            End If
            oDirFormLib.Close()
            'browse = Nothing
        End Sub


        ' Fields
        Public oDirFormLib As DirFormLibBrowse
    End Module
End Namespace

