Public Class ArmorBandedMail
    Inherits ArmorBase
    Public Sub New()
        Me.New("Banded Mail Armor")
    End Sub
    Public Sub New(ArmorName As String)
        MyBase.New()
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Heavy
            .Type = ArmorType.BandedMail
            .Value = 250
            .ArmorBonus = 6
            .MaxDexterityBonus = 1
            .ArcaneSpellFailureChance = 0.35
            .Weight = 35
        End With
    End Sub
End Class
