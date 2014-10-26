Public MustInherit Class ItemEffect_Base
    Implements iItemEffect

    Public Enum ItemEffectId
        ArcaneSpellFailureChance
        ArmorBonus
        MaximumDexterityBonus
        Weight
    End Enum

    Public Sub New(Item As ItemBase)
        AddHandler Item.OnEquip, AddressOf OnEquip_Handler
        AddHandler Item.OnUnEquip, AddressOf OnUnEquip_Handler
        AddHandler Item.OnPickup, AddressOf OnPickup_Handler
        AddHandler Item.OnDrop, AddressOf OnDrop_Handler
    End Sub

    Public Overridable Sub OnDrop_Handler(Item As ItemBase, Creature As CreatureBase) Implements iItemEffect.OnDrop_Handler
        If IsRemovedOnDrop Then
            Creature.ItemEffects.Remove(Me)
        End If
    End Sub

    Public Overridable Sub OnEquip_Handler(Item As ItemBase, Creature As CreatureBase) Implements iItemEffect.OnEquip_Handler
        If IsActiveOnEquip Then
            Creature.ItemEffects.Add(Me)
        End If
    End Sub

    Public Overridable Sub OnPickup_Handler(Item As ItemBase, Creature As CreatureBase) Implements iItemEffect.OnPickup_Handler
        If IsActiveOnPickup Then
            Creature.ItemEffects.Add(Me)
        End If
    End Sub

    Public Overridable Sub OnUnEquip_Handler(Item As ItemBase, Creature As CreatureBase) Implements iItemEffect.OnUnEquip_Handler
        If IsRemovedOnUnEquip Then
            Creature.ItemEffects.Remove(Me)
        End If
    End Sub

    Public Property ItemId As Guid Implements iItemEffect.ItemId
    Public Property IsActiveOnEquip As Boolean Implements iItemEffect.IsActiveOnEquip
    Public Property IsActiveOnPickup As Boolean Implements iItemEffect.IsActiveOnPickup
    Public Property IsRemovedOnDrop As Boolean Implements iItemEffect.IsRemovedOnDrop
    Public Property IsRemovedOnUnEquip As Boolean Implements iItemEffect.IsRemovedOnUnEquip

    Public Sub New()
        MyBase.New()
    End Sub

    Public Property EffectId As ItemEffectId Implements iItemEffect.EffectId
End Class
