Public Class Armor
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
    Public Property ArmorBonus As Integer
    Public Property MaxDexterityBonus As Integer
    Public Property ArcaneSpellFailureChance As Decimal
    Public Property Name As String
    Public Property Weight As Integer
    Public Property Cost As Integer
End Class
