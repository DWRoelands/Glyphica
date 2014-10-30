Public Class ArmorBandedMail
    Inherits ArmorBase
    Public Sub New()
        MyBase.New()
        With Me
            .Name = "Banded Mail Name"
            .Tier = ArmorTier.Heavy
            .Type = ArmorType.BandedMail
            .Value = 250
            .ArmorBonus = 6
            .MaximumDeterityBonus = 1
            .ArcaneSpellFailureChance = 0.35
            .Weight = 35
        End With
    End Sub

End Class
