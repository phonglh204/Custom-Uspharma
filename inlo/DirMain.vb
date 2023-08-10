Imports libscontrol.balanceviewlib

Module DirMain
    Public oDirFormLib As DirFormLibBrowse
    Sub main()
        Control.CheckForIllegalCrossThreadCalls = False
        oDirFormLib = New DirFormLibBrowse()
        With oDirFormLib
            If .sysConn Is Nothing Then
                End
            End If
            .SysID = "Lot"
            .Init()
            .oDir.strEnabled = "111100111"
            .frmUpdate = New frmDirInfor
            .Show()
            .Close()
        End With
    End Sub

End Module
