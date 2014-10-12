Public Class DaggerSmall
    Inherits WeaponBase
    Public Sub New()
        Me.New("Small Dagger")
    End Sub

    Public Sub New(WeaponName As String)
        Me.Damage = "1d3"
        Me.Cost = 2
        Me.CriticalModifier = 2
        Me.Weight = 1
    End Sub
End Class
