Imports System.Drawing
Public Class potion
    Inherits Artifact
    Public Sub New(_MapLevel As Integer, _Location As Point)
        With Me
            .Description = "a potion"
            .DisplayCharacter = "p"
            '.DisplayColor = ConsoleColor.Blue
            .Location = _Location
            .MapLevel = _MapLevel
            .Name = "potion"
        End With
    End Sub
End Class
