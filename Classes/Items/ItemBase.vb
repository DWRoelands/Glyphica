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

    Public Sub New()
        MyBase.New()
    End Sub

    Public Event OnEquip()
    Public Event OnUnEquip()
    Public Event OnPickup()
    Public Event OnDrop()

    Protected Overridable Sub OnUnEquip_Handler() Handles Me.OnUnEquip
        Me.Effects.Clear()
    End Sub

    Protected Overridable Sub OnDrop_Handler() Handles Me.OnDrop
        RaiseEvent OnUnEquip()
    End Sub

End Class

