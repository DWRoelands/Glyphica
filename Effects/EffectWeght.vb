Public Class EffectWeight
    Inherits EffectBase
    Private Weight As Integer
    Public Sub New(Modifier As Integer)
        Me.Weight = Modifier
    End Sub
    Public Overrides Sub Process(Creature As CreatureBase)
        Creature.TotalWeightCarried += Weight
    End Sub
End Class
