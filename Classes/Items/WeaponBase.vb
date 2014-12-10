Public Class WeaponBase
    Inherits ItemBase

    Public Property IsRanged As Boolean
    Public Property IsMelee As Boolean
    Public Property Damage As String = String.Empty
    Public Property AmmunitionType As String = String.Empty

    Public Sub New()
        Me.DisplayCharacter = "w"
        Me.IsEquippable = True
        Me.IsPortable = True
    End Sub

End Class
