Imports System.Drawing
Public Class Kobold
    Inherits Monster
    Public Sub New(_MapLevel As Integer, _Location As Point)
        With Me
            .ArmorClass = 15
            .DamageDice = "1d6-1"
            .Description = "a kobold"
            .DisplayCharacter = "k"
            .DisplayColor = ConsoleColor.Green
            .HitDice = "1d8"
            .Initiative = 1
            .Location = _Location
            .MapLevel = _MapLevel
            .Name = "Kobold"
        End With
    End Sub


End Class
