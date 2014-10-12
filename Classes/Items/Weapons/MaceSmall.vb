Public Class MaceSmall
    Inherits WeaponBase
    Public Sub New()
        Me.New("Small Mace")
    End Sub

    Public Sub New(WeaponName As String)
        MyBase.New()
        Me.Damage = "1d4"
        Me.Cost = 5
        Me.CriticalModifier = 2
        Me.Weight = 4
    End Sub
End Class
