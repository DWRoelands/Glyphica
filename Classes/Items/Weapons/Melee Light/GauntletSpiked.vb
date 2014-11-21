Public Class GauntletSpiked
    Inherits WeaponBase
    Public Sub New()
        Me.New("Spiked Gauntlet")
    End Sub

    Public Sub New(WeaponName As String)
        MyBase.New()
        Me.Name = WeaponName
        Me.Damage = "1d4"
        Me.Value = 5
        Me.CriticalModifier = 2
        Me.Weight = 1
        Me.Tier = WeaponType.MeleeLight
    End Sub
End Class
