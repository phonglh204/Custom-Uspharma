<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Example_ViewerData
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.grdMaster = New System.Windows.Forms.DataGridView()
        Me.grdDetail = New System.Windows.Forms.DataGridView()
        CType(Me.grdMaster, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdMaster
        '
        Me.grdMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdMaster.Location = New System.Drawing.Point(13, 27)
        Me.grdMaster.Name = "grdMaster"
        Me.grdMaster.RowHeadersWidth = 51
        Me.grdMaster.RowTemplate.Height = 24
        Me.grdMaster.Size = New System.Drawing.Size(1165, 254)
        Me.grdMaster.TabIndex = 0
        '
        'grdDetail
        '
        Me.grdDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdDetail.Location = New System.Drawing.Point(12, 287)
        Me.grdDetail.Name = "grdDetail"
        Me.grdDetail.RowHeadersWidth = 51
        Me.grdDetail.RowTemplate.Height = 24
        Me.grdDetail.Size = New System.Drawing.Size(1165, 424)
        Me.grdDetail.TabIndex = 1
        '
        'Example_ViewerData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1190, 723)
        Me.Controls.Add(Me.grdDetail)
        Me.Controls.Add(Me.grdMaster)
        Me.Name = "Example_ViewerData"
        Me.Text = "Example_ViewerData"
        CType(Me.grdMaster, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grdMaster As System.Windows.Forms.DataGridView
    Friend WithEvents grdDetail As System.Windows.Forms.DataGridView
End Class
