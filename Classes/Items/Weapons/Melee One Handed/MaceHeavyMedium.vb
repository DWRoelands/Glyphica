Public Class MaceHeavyMedium
    Inherits WeaponBase
    Public Sub New()
        Me.New("Medium Heavy Mace")
    End Sub

    Public Sub New(WeaponName As String)
        MyBase.New()
        Me.Name = WeaponName
        Me.Damage = "1d8"
        Me.Value = 12
        Me.CriticalModifier = 2
        Me.Weight = 8
        Me.Tier = WeaponType.MeleeOneHanded
    End Sub
End Class
