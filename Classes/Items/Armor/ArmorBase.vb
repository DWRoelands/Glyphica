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

    ' Armor that has other effects on the wearer should overide this method and then call MyBase.Process()
    Public Overrides Sub Process(Wearer As CreatureBase)
        If Me.IsEquipped Then
            Wearer.ArmorClassModifier += Me.ArmorBonus
            Wearer.TotalWeightCarried += Me.Weight
            Wearer.ArcaneSpellFailureChance = IIf(Wearer.ArcaneSpellFailureChance < Me.ArcaneSpellFailureChance, Me.ArcaneSpellFailureChance, Wearer.ArcaneSpellFailureChance)
            Wearer.DexterityModifier = IIf(Wearer.DexterityModifier > Me.MaxDexterityBonus, Me.MaxDexterityBonus, Wearer.DexterityModifier)
        End If
    End Sub

    Public Sub New()
        Me.DisplayCharacter = "a"
        Me.IsEquippable = True
        Me.IsPortable = True
    End Sub
End Class
