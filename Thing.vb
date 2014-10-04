Imports System.Drawing
Public MustInherit Class Thing
    Public Property MapLevel As Integer
    Public Property Location As Point
    Public Property Name As String = String.Empty
    Public Property Description As String = String.Empty
    Public Property DisplayColor As ConsoleColor
    Public Property DisplayCharacter As String = String.Empty
    Public Property Seen As Boolean = False
    Public Property Visible As Boolean = False

    Protected Function GraphicsCharacterGet(bvalue As Byte) As Char
        Return System.Text.Encoding.GetEncoding(437).GetChars(New Byte() {bvalue})(0)
    End Function

    Public Sub Draw()
        Console.ForegroundColor = Me.DisplayColor
        Console.SetCursorPosition(Me.Location.X, Me.Location.Y)
        Console.Write(Me.DisplayCharacter)

        If Not Me.Seen Then
            MessageWrite(String.Format("There is {0} here.", Me.Description))
            Me.Seen = True
        End If

        Me.Visible = True
    End Sub


End Class
