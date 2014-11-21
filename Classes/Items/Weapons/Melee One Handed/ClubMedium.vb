Public Class ClubMedium
    Inherits WeaponBase
    Public Sub New()
        Me.New("Medium Club")
    End Sub

    Public Sub New(WeaponName As String)
        MyBase.New()
        Me.Name = WeaponName
        Me.Damage = "1d6"
        Me.Value = 0
        Me.CriticalModifier = 2
        Me.Weight = 3
        Me.Tier = WeaponType.MeleeOneHanded
    End Sub
End Class
