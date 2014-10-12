Public Class MaceMedium
    Inherits WeaponBase
    Public Sub New()
        Me.New("Medium Mace")
    End Sub

    Public Sub New(WeaponName As String)
        MyBase.New()
        Me.Damage = "1d6"
        Me.Cost = 5
        Me.CriticalModifier = 2
        Me.Weight = 4
    End Sub
End Class
