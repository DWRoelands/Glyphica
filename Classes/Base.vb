Public MustInherit Class Base
    Public Property MapLevel As Integer
    Public Property Location As Point
    Public Property Name As String = String.Empty
    Public Property Description As String = String.Empty
    Public Property DisplayColor As ConsoleColor = ConsoleColor.White
    Public Property DisplayCharacter As String = String.Empty
    Public Property HasBeenSeen As Boolean = False
    Public Property IsVisible As Boolean = False
    Public Property Inventory As New List(Of ItemBase)

    Protected Function GraphicsCharacterGet(bvalue As Byte) As Char
        Return System.Text.Encoding.GetEncoding(437).GetChars(New Byte() {bvalue})(0)
    End Function

    Public Overridable Sub Draw()
        Console.ForegroundColor = Me.DisplayColor
        Console.SetCursorPosition(Me.Location.X, Me.Location.Y)
        Console.Write(Me.DisplayCharacter)

        If Not Me.HasBeenSeen Then
            Main.MessageWrite(String.Format("There is {0} {1} here.", IIf("AEIOU".Contains(Me.Name.Substring(1, 1).ToUpper), "an", "a"), Me.Name))
            Me.HasBeenSeen = True
        End If

        Me.IsVisible = True
    End Sub

End Class
