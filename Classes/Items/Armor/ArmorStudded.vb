Public Class ArmorStudded
    Inherits ArmorBase
    Public Sub New()
        Me.New("Studded Leather Armor")
    End Sub
    Public Sub New(ArmorName As String)
        MyBase.New()
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Light
            .Type = ArmorType.StuddedLeather
            .Value = 25
            .ArmorBonus = 3
            .MaximumDeterityBonus = 5
            .ArcaneSpellFailureChance = 0.15
            .Weight = 20
        End With
    End Sub
End Class
