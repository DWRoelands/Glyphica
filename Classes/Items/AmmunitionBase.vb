Public Class AmmunitionBase
    Inherits ItemBase

    Public Property Type As String = String.Empty
    Public Property Size As Integer
    Public Property Remaining As Integer

    Public Sub New()
        Me.IsEquippable = True
        Me.IsPortable = True
    End Sub
End Class
