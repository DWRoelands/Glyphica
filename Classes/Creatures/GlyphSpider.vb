Public Class GlyphSpider
    Inherits CreatureBase

    Public Sub New()
        Me.New("glyph spider")
    End Sub

    Public Sub New(CreatureName As String)
        Me.Name = CreatureName
        With Me
            .BaseArmorClass = 14
            .BaseStrength = 7
            .BaseDexterity = 17
            .BaseConstitution = 10
            .BaseIntelligence = 0
            .BaseWisdom = 10
            .BaseCharisma = 2
            .DamageDice = "1d4-2"
            .DisplayCharacter = "s"
            .HitDice = "1d8"
            .Initiative = 3
        End With
    End Sub
End Class
