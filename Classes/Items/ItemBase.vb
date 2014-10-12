﻿Public Class ItemBase
    Inherits Base
    Public Property Equippable As Boolean
    Public Property IsEquipped As Boolean
    Public Property IsPortable As Boolean
    Public Property IsSellable As Boolean
    Public Property IsSelected As Boolean  ' This is here to make the inventory screen easier to manage
    Public Property Weight As Integer
    Public Property Cost As Integer

    Public Overridable Sub Process(Wearer As CreatureBase)
        ' This method is used to implement the effects that an item has
        ' on the player who is wearing or carrying it
    End Sub

End Class

