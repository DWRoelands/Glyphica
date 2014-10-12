Public Class WeaponBase
    Inherits ItemBase

    Public Enum WeaponType
        MeleeLight
        MeleeOneHanded
        MeleeTwoHanded
        Ranged
    End Enum

    Public Property Damage As String
    Public Property CriticalModifier As Integer
End Class
