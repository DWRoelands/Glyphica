Public Class QuarterstaffSmall
    Inherits WeaponBase
    Public Sub New()
        Me.New("Small Quarterstaff")
    End Sub

    Public Sub New(WeaponName As String)
        MyBase.New()
        Me.Name = WeaponName
        Me.Damage = "1d4"
        Me.Value = 0
        Me.CriticalModifier = 2
        Me.Weight = 4
        Me.Tier = WeaponType.MeleeTwoHanded
    End Sub
End Class
