Public Class ClubSmall
    Inherits WeaponBase
    Public Sub New()
        Me.New("Small Club")
    End Sub

    Public Sub New(WeaponName As String)
        MyBase.New()
        Me.Name = WeaponName
        Me.Damage = "1d4"
        Me.Value = 0
        Me.CriticalModifier = 2
        Me.Weight = 3
        Me.Tier = WeaponType.MeleeOneHanded
    End Sub
End Class
