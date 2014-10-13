Public Class ArmorFullPlate
    Inherits ArmorBase
    Public Sub New()
        Me.New("Full Plate Armor")
    End Sub
    Public Sub New(ArmorName As String)
        MyBase.New()
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Heavy
            .Type = ArmorType.FullPlate
            .Value = 1500
            .ArmorBonus = 8
            .MaxDexterityBonus = 1
            .ArcaneSpellFailureChance = 0.35
            .Weight = 50
        End With
    End Sub
End Class
