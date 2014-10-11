Public Class ArmorBandedMail
    Inherits Armor
    Public Sub New()
        Me.New("Banded Mail Armor")
    End Sub
    Public Sub New(ArmorName As String)
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Heavy
            .Type = ArmorType.BandedMail
            .Cost = 250
            .ArmorBonus = 6
            .MaxDexterityBonus = 1
            .ArcaneSpellFailureChance = 0.35
            .Weight = 35
        End With
    End Sub
End Class
