Public Class GlyphicaTraceListener
    Inherits TraceListener

    Public Overloads Overrides Sub Write(message As String)
        Dim sw As New BooleanSwitch("GlyphicaTraceSwitch", "Glyphica Tracing Messages")
        If sw.Enabled Then
            Viewport.MessageWrite(message, ConsoleColor.Green)
        End If
    End Sub

    Public Overloads Overrides Sub WriteLine(message As String)

    End Sub
End Class
