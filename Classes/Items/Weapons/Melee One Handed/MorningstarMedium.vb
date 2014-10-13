﻿Public Class MorningstarMedium
    Inherits WeaponBase
    Public Sub New()
        Me.New("Medium Morningstar")
    End Sub

    Public Sub New(WeaponName As String)
        MyBase.New()
        Me.Name = WeaponName
        Me.Damage = "1d8"
        Me.Value = 8
        Me.CriticalModifier = 2
        Me.Weight = 6
        Me.Tier = WeaponType.MeleeOneHanded
    End Sub
End Class
