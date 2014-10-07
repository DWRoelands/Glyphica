﻿Public Class ArmorChainMail
    Inherits Armor
    Public Sub New()
        Me.New("Chain Mail Armor")
    End Sub
    Public Sub New(ArmorName As String)
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Medium
            .Type = ArmorType.Chainmail
            .Cost = 150
            .ArmorBonus = 5
            .MaxDexterityBonus = 2
            .ArcaneSpellFailureChance = 0.3
            .Weight = 40
        End With
    End Sub
End Class
