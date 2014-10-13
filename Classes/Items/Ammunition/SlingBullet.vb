Public Class SlingBullet
    Inherits AmmunitionBase
    Public Sub New()
        Me.New("Sling Bullet")
    End Sub
    Public Sub New(AmmoName As String)
        MyBase.New()
        Me.Name = AmmoName
        With Me
            .Type = AmmunitionType.SlingBullet
            .Value = 1
            .Weight = 1
            .Size = 10
            .Remaining = 10
        End With
    End Sub
End Class
