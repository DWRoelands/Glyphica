Public MustInherit Class Base
    Public Property MapLevel As Integer
    Public Property Location As Point
    Public Property Name As String = String.Empty
    Public Property Description As String = String.Empty
    Public Property DisplayColor As ConsoleColor = ConsoleColor.White
    Public Property DisplayCharacter As String = String.Empty
    Public Property HasBeenSeen As Boolean = False
    Public Property IsVisible As Boolean = False

    Protected Function GraphicsCharacterGet(bvalue As Byte) As Char
        Return System.Text.Encoding.GetEncoding(437).GetChars(New Byte() {bvalue})(0)
    End Function

    Public Overridable Sub Draw()
        Console.ForegroundColor = Me.DisplayColor
        Console.SetCursorPosition(Me.Location.X, Me.Location.Y)
        Console.Write(Me.DisplayCharacter)

        If Not Me.HasBeenSeen Then
            MessageWrite(String.Format("There is {0} here.", Me.Description))
            Me.HasBeenSeen = True
        End If

        Me.IsVisible = True
    End Sub


End Class
