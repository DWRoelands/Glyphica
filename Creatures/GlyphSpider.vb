Public Class GlyphSpider
    Inherits Creature
    Public Sub New(_MapLevel As Integer, _Location As Point)
        With Me
            .Location = _Location
            .MapLevel = _MapLevel
            .Name = "glyph spider"
            .Description = "a glyph spider"
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
