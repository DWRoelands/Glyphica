Public Class ArmorBreastPlate
    Inherits ArmorBase
    Public Sub New()
        Me.New("Breastplate Armor")
    End Sub
    Public Sub New(ArmorName As String)
        MyBase.New()
        Me.Name = ArmorName
        With Me
            .Tier = ArmorTier.Medium
            .Type = ArmorType.Breastplate
            .Value = 200
            .ArmorBonus = 5
            .MaximumDeterityBonus = 3
            .ArcaneSpellFailureChance = 0.25
            .Weight = 30
            .Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec neque leo, faucibus vitae nibh eu, gravida interdum metus. Vestibulum tortor tellus, viverra vel eros a, pretium aliquet lorem. Quisque venenatis neque vel suscipit hendrerit. Aenean."
        End With
    End Sub
End Class
