Public Class ArmorScaleMail
    Inherits Armor
    Public Sub New()
        Me.New("Scale Mail Armor")
    End Sub
    Public Sub New(ArmorName As String)
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Medium
            .Type = ArmorType.ScaleMail
            .Cost = 50
            .ArmorBonus = 4
            .MaxDexterityBonus = 3
            .ArcaneSpellFailureChance = 0.25
            .Weight = 30
        End With
    End Sub
End Class
