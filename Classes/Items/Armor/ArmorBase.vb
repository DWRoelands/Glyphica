Public Class ArmorBase
    Inherits ItemBase
    Public Enum ArmorTier
        Light
        Medium
        Heavy
    End Enum

    Public Enum ArmorType
        Padded
        Leather
        StuddedLeather
        ChainShirt
        Hide
        ScaleMail
        Chainmail
        Breastplate
        SplintMail
        BandedMail
        HalfPlate
        FullPlate
    End Enum

    Public Property Tier As ArmorTier
    Public Property Type As ArmorType
    Public Property MaxDexterityBonus As Integer
    Public Property ArcaneSpellFailureChance As Decimal
    Public Property ArmorBonus As Integer

    Public Sub New()
        Me.DisplayCharacter = "a"
        Me.IsEquippable = True
        Me.IsPortable = True
    End Sub

    Public Overridable Sub OnEquip_Handler() Handles Me.OnEquip
        With Me.Effects
            .Add(New EffectArmorClass(Me.ArmorBonus))
            .Add(New EffectArcaneSpellFailureChance(Me.ArcaneSpellFailureChance))
            .Add(New EffectMaximumDexterityBonus(Me.MaxDexterityBonus))
        End With
    End Sub

    Public Overridable Sub OnPickup_Handler() Handles Me.OnPickup
        With Me.Effects
            .Clear()
            .Add(New EffectWeight(Me.Weight))
        End With
    End Sub
End Class
