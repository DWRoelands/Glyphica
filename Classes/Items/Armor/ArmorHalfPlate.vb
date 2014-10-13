Public Class ArmorHalfPlate
    Inherits ArmorBase
    Public Sub New()
        Me.New("Half Plate Armor")
    End Sub
    Public Sub New(ArmorName As String)
        MyBase.New()
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Heavy
            .Type = ArmorType.HalfPlate
            .Value = 600
            .ArmorBonus = 7
            .MaxDexterityBonus = 0
            .ArcaneSpellFailureChance = 0.4
            .Weight = 50
        End With
    End Sub
End Class
