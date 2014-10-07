﻿Public Class ArmorBreastPlate
    Inherits Armor
    Public Sub New()
        Me.New("Breastplate Armor")
    End Sub
    Public Sub New(ArmorName As String)
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Medium
            .Type = ArmorType.Breastplate
            .Cost = 200
            .ArmorBonus = 5
            .MaxDexterityBonus = 3
            .ArcaneSpellFailureChance = 0.25
            .Weight = 30
        End With
    End Sub
End Class