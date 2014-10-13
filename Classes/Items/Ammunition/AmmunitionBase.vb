Public Class AmmunitionBase
    Inherits ItemBase
    Public Enum AmmunitionType
        CrossbowBolt
        Arrow
        SlingBullet
    End Enum

    Public Property Type As AmmunitionType
    Public Property Size As Integer
    Public Property Remaining As Integer

    Public Sub New()
        Me.IsEquippable = True
        Me.IsPortable = True
    End Sub
End Class
