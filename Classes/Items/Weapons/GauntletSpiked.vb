﻿Public Class GauntletSpiked
    Inherits WeaponBase
    Public Sub New()
        Me.New("Spiked Gauntlet")
    End Sub

    Public Sub New(WeaponName As String)
        MyBase.New()
        Me.Damage = "1d4"
        Me.Cost = 5
        Me.CriticalModifier = 2
        Me.Weight = 1
    End Sub
End Class
