Public Class Bolt
    Inherits AmmunitionBase
    Public Sub New()
        Me.New("Crossbow Bolt")
    End Sub
    Public Sub New(AmmoName As String)
        MyBase.New()
        Me.Name = AmmoName
        With Me
            .Type = AmmunitionType.CrossbowBolt
            .Value = 1
            .Weight = 1
            .Size = 10
            .Remaining = 10
        End With
    End Sub
End Class
