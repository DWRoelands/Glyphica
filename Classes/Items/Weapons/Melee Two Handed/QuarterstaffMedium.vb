Public Class QuarterstaffMedium
    Inherits WeaponBase
    Public Sub New()
        Me.New("Medium Quarterstaff")
    End Sub

    Public Sub New(WeaponName As String)
        MyBase.New()
        Me.Name = WeaponName
        Me.Damage = "1d6"
        Me.Value = 0
        Me.CriticalModifier = 2
        Me.Weight = 4
        Me.Tier = WeaponType.MeleeTwoHanded
    End Sub
End Class
