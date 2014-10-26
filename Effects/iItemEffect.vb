Public Interface iItemEffect
    Property ItemId As Guid
    Property EffectId As ItemEffect_Base.ItemEffectId
    Property IsActiveOnEquip As Boolean
    Property IsRemovedOnUnEquip As Boolean
    Property IsActiveOnPickup As Boolean
    Property IsRemovedOnDrop As Boolean

    Sub OnEquip_Handler(Item As ItemBase, Creature As CreatureBase)
    Sub OnPickup_Handler(Item As ItemBase, Creature As CreatureBase)
    Sub OnUnEquip_Handler(Item As ItemBase, Creature As CreatureBase)
    Sub OnDrop_Handler(Item As ItemBase, Creature As CreatureBase)

End Interface
