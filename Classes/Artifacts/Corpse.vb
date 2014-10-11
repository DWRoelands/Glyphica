Public Class Corpse
    Inherits Artifact
    Public Sub New(_MapLevel As Integer, _Location As Point, MonsterName As String)
        With Me
            .MapLevel = _MapLevel
            .Location = _Location
            .Description = String.Format("the corpse of {0} {1}", If("AEIOU".Contains(MonsterName.Substring(0, 1).ToUpper), "an", "a"), MonsterName)
            .DisplayCharacter = GraphicsCharacterGet(15)
        End With
    End Sub

End Class
