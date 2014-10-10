Public Class Kobold
    Inherits Creature
    Public Sub New(_MapLevel As Integer, _Location As Point)
        With Me
            .BaseArmorClass = 15
            .BaseStrength = 9
            .BaseDexterity = 13
            .BaseConstitution = 10
            .BaseIntelligence = 10
            .BaseWisdom = 9
            .BaseCharisma = 8
            .DamageDice = "3d6-1"
            .Description = "a kobold"
            .DisplayCharacter = "k"
            .HitDice = "1d8"
            .Initiative = 1
            .Location = _Location
            .MapLevel = _MapLevel
            .Name = "Kobold"

        End With
    End Sub


End Class
