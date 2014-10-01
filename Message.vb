Public Class Message
    Public Property Text As String
    Public Property Color As ConsoleColor

    Public Sub New(Message As String, MessageColor As ConsoleColor)
        _Text = Message
        _Color = MessageColor
    End Sub
End Class
