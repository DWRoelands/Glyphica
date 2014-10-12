Public Class ArmorStudded
    Inherits ArmorBase
    Public Sub New()
        Me.New("Studded Leather Armor")
    End Sub
    Public Sub New(ArmorName As String)
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Light
            .Type = ArmorType.StuddedLeather
            .Cost = 25
            .ArmorBonus = 3
            .MaxDexterityBonus = 5
            .ArcaneSpellFailureChance = 0.15
            .Weight = 20
        End With
    End Sub
End Class
