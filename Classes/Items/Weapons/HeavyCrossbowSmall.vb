Public Class HeavyCrossbowSmall
    Inherits WeaponBase
    Public Sub New()
        Me.New("Small Heavy Crossbow")
    End Sub

    Public Sub New(WeaponName As String)
        MyBase.New()
        Me.Name = WeaponName
        Me.Damage = "1d8"
        Me.Value = 50
        Me.CriticalModifier = 2
        Me.Weight = 8
        Me.Tier = WeaponType.Ranged
        Me.AmmunitionType = AmmunitionBase.AmmunitionType.CrossbowBolt
    End Sub
End Class
