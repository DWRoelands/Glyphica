Imports System.Drawing
Public Class Kobold
    Inherits Monster
    Public Sub New(_MapLevel As Integer, _Location As Point)
        With Me
            .MapLevel = _MapLevel
            .Location = _Location
            .Name = "Kobold"
            .Description = "A short reptilian humanoid"
            .DisplayCharacter = "k"
            .HitDice = "1d8"
            .DamageDice = "1d6-1"
            .DisplayColor = ConsoleColor.Green
            .Initiative = 1
        End With
    End Sub
End Class
