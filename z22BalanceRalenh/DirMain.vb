Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports libscontrol.balanceviewlib
Imports libscontrol
Imports libscommon


Namespace z22BalanceRalenh
    Module DirMain
        ' Methods
        Public Sub Calc()
            Dim columnNumber As Integer = oDirFormLib.oDir.ob.grdLookup.CurrentCell.ColumnNumber
            Dim mappingName As String = oDirFormLib.oDir.ob.grdLookup.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName
            If (Strings.InStr(oDirFormLib.oDir.ob.ds.Tables.Item(0).Columns.Item(mappingName).DataType.ToString.ToLower, "dec", CompareMethod.Binary) > 0) Then
                Dim zero As Decimal = Decimal.Zero
                Dim num4 As Integer = (oDirFormLib.oDir.ob.dv.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num4)
                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, oDirFormLib.oDir.ob.dv.Item(i).Item(mappingName)))
                    i += 1
                Loop
                Dim column As DataGridTextBoxColumn = DirectCast(oDirFormLib.oDir.ob.grdLookup.TableStyles.Item(0).GridColumnStyles.Item(columnNumber), DataGridTextBoxColumn)
                Msg.Alert((column.HeaderText & " = " & Strings.Format(zero, column.Format)), 3)
            End If
        End Sub

        Private Sub DirLoad(ByVal sender As Object, ByVal e As EventArgs)
            oDirFormLib.oDir.mnFile.MenuItems.Item(4).Text = StringType.FromObject(oDirFormLib.oLan.Item("010"))
            oDirFormLib.oDir.tbr.Buttons.Item(4).ToolTipText = StringType.FromObject(oDirFormLib.oLan.Item("010"))
            oDirFormLib.oDir.tbr.ImageList.Images.Item(4) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("Imagedir"), "sum.bmp")))
            oDirFormLib.oDir.ob.grdLookup.ContextMenu.MenuItems.Clear()
            Dim num2 As Integer = (oDirFormLib.oDir.mnFile.MenuItems.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                oDirFormLib.oDir.ob.grdLookup.ContextMenu.MenuItems.Add(oDirFormLib.oDir.mnFile.MenuItems.Item(i).CloneMenu)
                i += 1
            Loop
        End Sub

        <STAThread()>
        Public Sub main()
            oDirFormLib = New DirFormLibBrowse
            If (oDirFormLib.sysConn Is Nothing) Then
                ProjectData.EndApp()
            End If
            DirMain.nBgYear = IntegerType.FromObject(Sql.GetValue(oDirFormLib.appConn, "dmstt", "nam_bd", "1=1"))
            oDirFormLib.SysID = "z16SocoBalance"
            oDirFormLib.Init()
            oDirFormLib.oDir.strEnabled = "111111001"
            oDirFormLib.oDir.ebutHand = New ToolBarButtonClickEventHandler(AddressOf DirMain.tbrClick)
            oDirFormLib.oDir.emnuHand = New EventHandler(AddressOf DirMain.mnuclick)
            AddHandler oDirFormLib.oDir.ob.frmLookup.Load, New EventHandler(AddressOf DirMain.DirLoad)
            oDirFormLib.frmUpdate = New frmDirInfor
            oDirFormLib.Show()
            oDirFormLib.Close()
        End Sub

        Public Sub mnuclick(ByVal sender As Object, ByVal e As EventArgs)
            Select Case ByteType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing))
                Case 0
                    oDirFormLib.dirAddNew()
                    Exit Select
                Case 1
                    oDirFormLib.dirEdit()
                    Exit Select
                Case 2
                    oDirFormLib.dirDelete()
                    Exit Select
                Case 4
                    DirMain.Calc()
                    Exit Select
                Case 6
                    oDirFormLib.dirPrint()
                    Exit Select
                Case 8
                    oDirFormLib.oDir.ob.frmLookup.Close()
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
                    oDirFormLib.dirAddNew()
                    Exit Select
                Case 1
                    oDirFormLib.dirEdit()
                    Exit Select
                Case 2
                    oDirFormLib.dirDelete()
                    Exit Select
                Case 4
                    DirMain.Calc()
                    Exit Select
                Case 6
                    oDirFormLib.dirPrint()
                    Exit Select
                Case 8
                    oDirFormLib.oDir.ob.frmLookup.Close()
                    Exit Select
            End Select
        End Sub


        ' Fields
        Public nBgYear As Integer
        Public oDirFormLib As DirFormLibBrowse
    End Module
End Namespace

