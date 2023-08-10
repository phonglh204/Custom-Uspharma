Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol
Imports libscontrol.balanceviewlib

Namespace inlosd0
    <StandardModule> _
    Friend NotInheritable Class DirMain
        ' Methods
        Public Shared Sub Calc()
            Dim columnNumber As Integer = DirMain.oDirFormLib.oDir.ob.grdLookup.CurrentCell.ColumnNumber
            Dim mappingName As String = DirMain.oDirFormLib.oDir.ob.grdLookup.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName
            If (Strings.InStr(DirMain.oDirFormLib.oDir.ob.ds.Tables.Item(0).Columns.Item(mappingName).DataType.ToString.ToLower, "dec", CompareMethod.Binary) > 0) Then
                Dim zero As Decimal = Decimal.Zero
                Dim num4 As Integer = (DirMain.oDirFormLib.oDir.ob.dv.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num4)
                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, DirMain.oDirFormLib.oDir.ob.dv.Item(i).Item(mappingName)))
                    i += 1
                Loop
                Dim column As DataGridTextBoxColumn = DirectCast(DirMain.oDirFormLib.oDir.ob.grdLookup.TableStyles.Item(0).GridColumnStyles.Item(columnNumber), DataGridTextBoxColumn)
                Msg.Alert((column.HeaderText & " = " & Strings.Format(zero, column.Format)), 3)
            End If
        End Sub

        Private Shared Sub DirLoad(ByVal sender As Object, ByVal e As EventArgs)
            DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(4).Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("010"))
            DirMain.oDirFormLib.oDir.tbr.Buttons.Item(4).ToolTipText = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("010"))
            DirMain.oDirFormLib.oDir.tbr.ImageList.Images.Item(4) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("Imagedir"), "sum.bmp")))
            DirMain.oDirFormLib.oDir.ob.grdLookup.ContextMenu.MenuItems.Clear()
            Dim num2 As Integer = (DirMain.oDirFormLib.oDir.mnFile.MenuItems.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                DirMain.oDirFormLib.oDir.ob.grdLookup.ContextMenu.MenuItems.Add(DirMain.oDirFormLib.oDir.mnFile.MenuItems.Item(i).CloneMenu)
                i += 1
            Loop
        End Sub

        <STAThread()>
        Public Shared Sub main(ByVal CmdArgs As String())
            DirMain.oDirFormLib = New DirFormLibBrowse
            If (DirMain.oDirFormLib.sysConn Is Nothing) Then
                ProjectData.EndApp()
            End If
            nBgYear = Fox.GetWordNum(CmdArgs(0), 1, "#"c)
            DirMain.oDirFormLib.SysID = "inbg_kh"
            DirMain.oDirFormLib.Init()
            DirMain.oDirFormLib.oDir.strEnabled = "111111001"
            DirMain.oDirFormLib.oDir.ebutHand = New ToolBarButtonClickEventHandler(AddressOf DirMain.tbrClick)
            DirMain.oDirFormLib.oDir.emnuHand = New EventHandler(AddressOf DirMain.mnuclick)
            AddHandler DirMain.oDirFormLib.oDir.ob.frmLookup.Load, New EventHandler(AddressOf DirMain.DirLoad)
            DirMain.oDirFormLib.frmUpdate = New frmDirInfor
            DirMain.oDirFormLib.Show()
            DirMain.oDirFormLib.Close()
        End Sub

        Public Shared Sub mnuclick(ByVal sender As Object, ByVal e As EventArgs)
            Select Case ByteType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing))
                Case 0
                    DirMain.oDirFormLib.dirAddNew()
                    Exit Select
                Case 1
                    DirMain.oDirFormLib.dirEdit()
                    Exit Select
                Case 2
                    If (Information.IsNothing(DirMain.oDirFormLib.oDir.ob.CurDataRow) OrElse (ObjectType.ObjTst(Sql.GetValue((DirMain.oDirFormLib.appConn), "dmvt", "gia_ton", StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("ma_vt = '", DirMain.oDirFormLib.oDir.ob.CurDataRow.Item("ma_vt")), "'"))), 3, False) <> 0)) Then
                        DirMain.oDirFormLib.dirDelete()
                        Exit Select
                    End If
                    Msg.Alert(StringType.FromObject(DirMain.oDirFormLib.oLan.Item("013")), 2)
                    Return
                Case 4
                    DirMain.Calc()
                    Exit Select
                Case 6
                    DirMain.oDirFormLib.dirPrint()
                    Exit Select
                Case 8
                    DirMain.oDirFormLib.oDir.ob.frmLookup.Close()
                    Exit Select
            End Select
        End Sub

        Public Shared Sub tbrClick(ByVal sender As Object, ByVal e As ToolBarButtonClickEventArgs)
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
                    If (Information.IsNothing(DirMain.oDirFormLib.oDir.ob.CurDataRow) OrElse (ObjectType.ObjTst(Sql.GetValue((DirMain.oDirFormLib.appConn), "dmvt", "gia_ton", StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("ma_vt = '", DirMain.oDirFormLib.oDir.ob.CurDataRow.Item("ma_vt")), "'"))), 3, False) <> 0)) Then
                        DirMain.oDirFormLib.dirDelete()
                        Exit Select
                    End If
                    Msg.Alert(StringType.FromObject(DirMain.oDirFormLib.oLan.Item("013")), 2)
                    Return
                Case 4
                    DirMain.Calc()
                    Exit Select
                Case 6
                    DirMain.oDirFormLib.dirPrint()
                    Exit Select
                Case 8
                    DirMain.oDirFormLib.oDir.ob.frmLookup.Close()
                    Exit Select
            End Select
        End Sub


        ' Fields
        Public Shared nBgYear As Integer
        Public Shared oDirFormLib As DirFormLibBrowse
    End Class
End Namespace

