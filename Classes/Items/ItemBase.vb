Public Class ItemBase
    Inherits Base
    Public Property IsEquippable As Boolean
    Public Property IsEquipped As Boolean
    Public Property IsPortable As Boolean
    Public Property IsSellable As Boolean
    Public Property IsSelected As Boolean  ' This is here to make the inventory screen easier to manage
    Public Property Weight As Integer
    Public Property Value As Integer
    Public Property Effects As List(Of EffectBase)

    Public Overridable Sub Process(Wearer As CreatureBase)
        ' This method is used to implement the effects that an item has
        ' on the player who is wearing or carrying it
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

End Class

