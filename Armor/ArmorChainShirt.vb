Public Class ArmorChainShirt
    Inherits Armor
    Public Sub New()
        Me.New("Chain Shirt Armor")
    End Sub
    Public Sub New(ArmorName As String)
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Light
            .Type = ArmorType.ChainShirt
            .Cost = 100
            .ArmorBonus = 4
            .MaxDexterityBonus = 4
            .ArcaneSpellFailureChance = 0.2
            .Weight = 25
        End With
    End Sub
End Class
