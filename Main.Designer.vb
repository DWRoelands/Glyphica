<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
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
        Me.rtbMessages = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'rtbMessages
        '
        Me.rtbMessages.Dock = System.Windows.Forms.DockStyle.Top
        Me.rtbMessages.Location = New System.Drawing.Point(0, 0)
        Me.rtbMessages.Name = "rtbMessages"
        Me.rtbMessages.Size = New System.Drawing.Size(808, 67)
        Me.rtbMessages.TabIndex = 0
        Me.rtbMessages.Text = ""
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(808, 392)
        Me.Controls.Add(Me.rtbMessages)
        Me.Name = "Main"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents rtbMessages As System.Windows.Forms.RichTextBox

End Class
