Public Class ArmorPadded
    Inherits ArmorBase
    Public Sub New()
        Me.New("Padded Armor")
    End Sub
    Public Sub New(ArmorName As String)
        MyBase.New()
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Light
            .Type = ArmorType.Padded
            .Cost = 5
            .ArmorBonus = 1
            .MaxDexterityBonus = 8
            .ArcaneSpellFailureChance = 0.05
            .Weight = 10
        End With
    End Sub

End Class
