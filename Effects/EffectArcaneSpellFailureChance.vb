Public Class EffectArcaneSpellFailureChance
    Inherits EffectBase
    Private ArcaneSpellFailureChance As Decimal
    Public Sub New(Modifier As Decimal)
        ArcaneSpellFailureChance = Modifier
    End Sub

    Public Overrides Sub Process(Creature As CreatureBase)
        Creature.ArcaneSpellFailureChance = Math.Max(Creature.ArcaneSpellFailureChance, ArcaneSpellFailureChance)
    End Sub
End Class
