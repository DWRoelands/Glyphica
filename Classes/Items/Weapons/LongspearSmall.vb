Public Class LongspearSmall
    Inherits WeaponBase
    Public Sub New()
        Me.New("Small Longspear")
    End Sub

    Public Sub New(WeaponName As String)
        MyBase.New()
        Me.Name = WeaponName
        Me.Damage = "1d6"
        Me.Value = 5
        Me.CriticalModifier = 3
        Me.Weight = 9
        Me.Tier = WeaponType.MeleeTwoHanded
    End Sub
End Class
