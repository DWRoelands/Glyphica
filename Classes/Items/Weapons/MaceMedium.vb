Public Class MaceMedium
    Inherits WeaponBase
    Public Sub New()
        Me.New("Medium Mace")
    End Sub

    Public Sub New(WeaponName As String)
        MyBase.New()
        Me.Name = WeaponName
        Me.Damage = "1d6"
        Me.Value = 5
        Me.CriticalModifier = 2
        Me.Weight = 4
        Me.Tier = WeaponType.MeleeLight
    End Sub
End Class
