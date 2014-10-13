Public Class ArmorHide
    Inherits ArmorBase
    Public Sub New()
        Me.New("Hide Armor")
    End Sub
    Public Sub New(ArmorName As String)
        MyBase.New()
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Medium
            .Type = ArmorType.Hide
            .Value = 15
            .ArmorBonus = 3
            .MaxDexterityBonus = 4
            .ArcaneSpellFailureChance = 0.2
            .Weight = 25
        End With
    End Sub
End Class
