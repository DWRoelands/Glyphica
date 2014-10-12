﻿Public Class ArmorLeather
    Inherits ArmorBase
    Public Sub New()
        Me.New("Leather Armor")
    End Sub
    Public Sub New(ArmorName As String)
        MyBase.New()
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Light
            .Type = ArmorType.Leather
            .Cost = 10
            .ArmorBonus = 2
            .MaxDexterityBonus = 6
            .ArcaneSpellFailureChance = 0.1
            .Weight = 15
        End With
    End Sub
End Class
