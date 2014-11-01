Public MustInherit Class ItemEffect_Base
    Implements IItemEffect

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

    Public Overridable Sub OnDrop_Handler(Item As ItemBase, Creature As CreatureBase) Implements IItemEffect.OnDrop_Handler
        If IsRemovedOnDrop Then
            Creature.ItemEffects.Remove(Me)
        End If
    End Sub

    Public Overridable Sub OnEquip_Handler(Item As ItemBase, Creature As CreatureBase) Implements IItemEffect.OnEquip_Handler
        If IsActiveOnEquip Then
            Creature.ItemEffects.Add(Me)
        End If
    End Sub

    Public Overridable Sub OnPickup_Handler(Item As ItemBase, Creature As CreatureBase) Implements IItemEffect.OnPickup_Handler
        If IsActiveOnPickup Then
            Creature.ItemEffects.Add(Me)
        End If
    End Sub

    Public Overridable Sub OnUnEquip_Handler(Item As ItemBase, Creature As CreatureBase) Implements IItemEffect.OnUnEquip_Handler
        If IsRemovedOnUnEquip Then
            Creature.ItemEffects.Remove(Me)
        End If
    End Sub

    Public Property ItemId As Guid Implements IItemEffect.ItemId
    Public Property IsActiveOnEquip As Boolean Implements IItemEffect.IsActiveOnEquip
    Public Property IsActiveOnPickup As Boolean Implements IItemEffect.IsActiveOnPickup
    Public Property IsRemovedOnDrop As Boolean Implements IItemEffect.IsRemovedOnDrop
    Public Property IsRemovedOnUnEquip As Boolean Implements IItemEffect.IsRemovedOnUnEquip

    Public Sub New()
        MyBase.New()
    End Sub

    Public Property EffectId As ItemEffectId Implements IItemEffect.EffectId
End Class
