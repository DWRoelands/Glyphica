Public Class ArmorSplintMail
    Inherits Armor
    Public Sub New()
        Me.New("Splintmail Armor")
    End Sub
    Public Sub New(ArmorName As String)
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Heavy
            .Type = ArmorType.SplintMail
            .Cost = 200
            .ArmorBonus = 6
            .MaxDexterityBonus = 0
            .ArcaneSpellFailureChance = 0.4
            .Weight = 45
        End With
    End Sub
End Class
