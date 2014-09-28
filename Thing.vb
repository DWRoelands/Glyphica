Imports System.Drawing
Public MustInherit Class Thing
    Public Property MapLevel As Integer
    Public Property Location As Point
    Public Property Name As String = String.Empty
    Public Property Description As String = String.Empty
    Public Property DisplayColor As ConsoleColor
    Public Property DisplayCharacter As String = String.Empty

End Class
