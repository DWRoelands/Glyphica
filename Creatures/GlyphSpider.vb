Public Class GlyphSpider
    Inherits Creature
    Public Sub New(_MapLevel As Integer, _Location As Point)
        With Me
            .ArmorClass = 14
            .DamageDice = "1d4+2"
            .Description = "a glyph spider"
            .DisplayCharacter = "s"
            '.DisplayColor = ConsoleColor.Green
            .HitDice = "1d8"
            .Initiative = 3
            .Location = _Location
            .MapLevel = _MapLevel
            .Name = "glyph spider"
        End With
    End Sub
End Class
