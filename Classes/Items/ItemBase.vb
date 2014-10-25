Public Class ItemBase
    Inherits Base
    Public IsEquippable As Boolean
    Public IsEquipped As Boolean
    Public IsPortable As Boolean
    Public IsCarried As Boolean
    Public IsSelected As Boolean  ' This is here to make the inventory screen easier to manage
    Public Weight As Integer
    Public Value As Integer
    Public Id As Guid
    Public Effects As New List(Of Effect_Base)

    Public Sub New()
        MyBase.New()

    End Sub

End Class

