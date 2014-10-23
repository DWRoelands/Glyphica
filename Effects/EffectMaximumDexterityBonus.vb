Public Class EffectMaximumDexterityBonus
    Inherits EffectBase
    Private MaximumDexterityBonus As Integer
    Public Sub New(Modifier As Integer)
        MaximumDexterityBonus = Modifier
    End Sub
    Public Overrides Sub Process(Creature As CreatureBase)
        Creature.DexterityModifier = Math.Min(Creature.DexterityModifier, MaximumDexterityBonus)
    End Sub
End Class
