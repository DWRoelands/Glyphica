Public Class ArmorChainMail
    Inherits ArmorBase
    Public Sub New()
        Me.New("Chain Mail Armor")
    End Sub
    Public Sub New(ArmorName As String)
        MyBase.New()
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Medium
            .Type = ArmorType.Chainmail
            .Value = 150
            .ArmorBonus = 5
            .MaximumDeterityBonus = 2
            .ArcaneSpellFailureChance = 0.3
            .Weight = 40
        End With
    End Sub
End Class
