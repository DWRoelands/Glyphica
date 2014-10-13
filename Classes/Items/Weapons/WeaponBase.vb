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
    Public Property Tier As WeaponType

    Public Sub New()
        Me.DisplayCharacter = "w"
        Me.IsEquippable = True
    End Sub

End Class
