Public Class Kobold
    Inherits Creature
    Public Sub New(_MapLevel As Integer, _Location As Point)
        With Me
            .ArmorClass = 15
            .DamageDice = "3d6-1"
            .Description = "a kobold"
            .DisplayCharacter = "k"
            '.DisplayColor = ConsoleColor.Green
            .HitDice = "1d8"
            .Initiative = 1
            .Location = _Location
            .MapLevel = _MapLevel
            .Name = "Kobold"
        End With
    End Sub


End Class
