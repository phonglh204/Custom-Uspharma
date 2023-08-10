Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports libscontrol.balanceviewlib

Module DirMain
    ' Methods
    Private Sub frmDir_Load(ByVal sender As Object, ByVal e As EventArgs)
        'DirectCast(Browse.grdLookup.TableStyles.Item(0).GridColumnStyles.Item("gia_nt2"), DataGridTextBoxColumn).Format = StringType.FromObject(DirMain.oDirFormLib.oOptions.Item("m_ip_gia_nt"))
    End Sub

    <STAThread()>
    Public Sub main()
        DirMain.oDirFormLib = New DirFormLibBrowse
        'Dim oRpt As New rpDir
        If (DirMain.oDirFormLib.sysConn Is Nothing) Then
            ProjectData.EndApp()
        End If
        DirMain.oDirFormLib.SysID = "z21coctdm2"
        DirMain.oDirFormLib.Init()
        oDirFormLib.oDir.strEnabled = "111100111"
        DirMain.oDirFormLib.frmUpdate = New frmDirInfor
        'reports.SetReportVar(DirMain.oDirFormLib.sysConn, DirMain.oDirFormLib.appConn, DirMain.oDirFormLib.SysID, DirMain.oDirFormLib.oOptions, oRpt)
        'DirMain.oDirFormLib.oRpt = oRpt
        AddHandler DirMain.oDirFormLib.oDir.ob.frmLookup.Load, New EventHandler(AddressOf DirMain.frmDir_Load)
        DirMain.oDirFormLib.Show()
        DirMain.oDirFormLib.Close()
    End Sub


    ' Fields
    Public oDirFormLib As DirFormLibBrowse
End Module

