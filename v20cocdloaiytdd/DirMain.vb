Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol.balanceviewlib

Namespace v20cocdloaiytdd
    Module DirMain
        ' Methods
        Private Sub DirLoad(ByVal sender As Object, ByVal e As EventArgs)
            Dim num2 As Byte = CByte((DirMain.oDirFormLib.oDir.ob.frmLookup.Controls.Count - 1))
            Dim i As Byte = 0
            Do While (i <= num2)
                If (Strings.InStr(Strings.LCase(DirMain.oDirFormLib.oDir.ob.frmLookup.Controls.Item(i).GetType.ToString), "refreshbutton", CompareMethod.Binary) > 0) Then
                    DirMain.oDirFormLib.oDir.ob.frmLookup.Controls.Item(i).Visible = False
                    Exit Do
                End If
                i = CByte((i + 1))
            Loop
            Dim tcSQL As String = "EXEC fs20_GetCOWIPFStyleQty "
            tcSQL = ((tcSQL & Strings.Trim(Conversion.Str(DirMain.nPeriod))) & ", " & Strings.Trim(Conversion.Str(DirMain.nYear)))
            DirMain.oDirFormLib.oDir.ob.dv.Table.Clear()
            Sql.SQLRetrieve((DirMain.oDirFormLib.appConn), tcSQL, DirMain.oDirFormLib.cTableDir, (DirMain.oDirFormLib.oDir.ob.ds))
            DirMain.oDirFormLib.oDir.ob.dv.Table = DirMain.oDirFormLib.oDir.ob.ds.Tables.Item(DirMain.oDirFormLib.cTableDir)
            DirMain.oDirFormLib.oDir.ob.frmLookup.Text = Strings.Replace(DirMain.oDirFormLib.oDir.ob.frmLookup.Text, "%s", StringType.FromObject(DirMain.oDirFormLib.oLan.Item(RuntimeHelpers.GetObjectValue(Interaction.IIf((DirMain.nFormStyle = 0), "301", "302")))), 1, -1, CompareMethod.Binary)
        End Sub

        <STAThread()>
        Public Sub Main(ByVal cmdArgs As String())
            DirMain.oDirFormLib = New DirFormLibBrowse
            If (DirMain.oDirFormLib.sysConn Is Nothing) Then
                ProjectData.EndApp()
            End If
            oDirFormLib.SysID = "v20COWIPFStyleQty"
            DirMain.nFormStyle = ByteType.FromString(cmdArgs(0))
            oDirFormLib.Init()
            If (DirMain.nFormStyle = 1) Then
                Dim dates As New frmDates
                dates.Text = StringType.FromObject(oDirFormLib.oLan.Item("100"))
                dates.ShowDialog()
                If Not DirMain.isCon Then
                    dates.Dispose()
                    oDirFormLib.Close()
                    Return
                End If
            Else
                DirMain.nPeriod = 0
                DirMain.nYear = IntegerType.FromObject(Sql.GetValue((oDirFormLib.appConn), "dmstt", "nam_bd", "1=1"))
            End If
            oDirFormLib.oDir.strEnabled = "111100001"
            oDirFormLib.oDir.ebutHand = New ToolBarButtonClickEventHandler(AddressOf DirMain.tbrClick)
            oDirFormLib.oDir.emnuHand = New EventHandler(AddressOf DirMain.mnuclick)
            AddHandler DirMain.oDirFormLib.oDir.ob.frmLookup.Load, New EventHandler(AddressOf DirMain.DirLoad)
            oDirFormLib.frmUpdate = New frmDirInfor
            oDirFormLib.Show()
            oDirFormLib.Close()
        End Sub

        Public Sub mnuclick(ByVal sender As Object, ByVal e As EventArgs)
            Select Case ByteType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing))
                Case 0
                    DirMain.oDirFormLib.dirAddNew()
                    Exit Select
                Case 1
                    DirMain.oDirFormLib.dirEdit()
                    Exit Select
                Case 2
                    DirMain.oDirFormLib.dirDelete()
                    Exit Select
                Case 8
                    DirMain.oDirFormLib.oDir.ob.frmLookup.Close()
                    Exit Select
            End Select
        End Sub

        Public Sub tbrClick(ByVal sender As Object, ByVal e As ToolBarButtonClickEventArgs)
            Dim objArray2 As Object() = New Object(1 - 1) {}
            Dim args As ToolBarButtonClickEventArgs = e
            objArray2(0) = args.Button
            Dim objArray As Object() = objArray2
            Dim copyBack As Boolean() = New Boolean() {True}
            If copyBack(0) Then
                args.Button = DirectCast(objArray(0), ToolBarButton)
            End If
            Select Case ByteType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "Buttons", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "IndexOf", objArray, Nothing, copyBack))
                Case 0
                    DirMain.oDirFormLib.dirAddNew()
                    Exit Select
                Case 1
                    DirMain.oDirFormLib.dirEdit()
                    Exit Select
                Case 2
                    DirMain.oDirFormLib.dirDelete()
                    Exit Select
                Case 8
                    DirMain.oDirFormLib.oDir.ob.frmLookup.Close()
                    Exit Select
            End Select
        End Sub


        ' Fields
        Public isCon As Boolean
        Public nFormStyle As Byte
        Public nPeriod As Integer
        Public nYear As Integer
        Public oDirFormLib As DirFormLibBrowse
    End Module
End Namespace

