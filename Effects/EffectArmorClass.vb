Public Class EffectArmorClass
    Inherits EffectBase
    Private ArmorClassBonus As Integer
    Public Sub New(Modifier As Integer)
        ArmorClassBonus = Modifier
    End Sub

    Public Overrides Sub Process(Creature As CreatureBase)
        Creature.ArmorClassModifier += Me.ArmorClassBonus
    End Sub
End Class
