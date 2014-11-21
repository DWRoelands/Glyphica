Public Interface IItemEffect
    Property ItemId As Guid
    Property EffectId As ItemEffect_Base.ItemEffectId
    Property IsActiveOnEquip As Boolean
    Property IsRemovedOnUnEquip As Boolean
    Property IsActiveOnPickup As Boolean
    Property IsRemovedOnDrop As Boolean

    Sub OnPickup(Item As ItemBase, Creature As CreatureBase)
    Sub OnDrop(Item As ItemBase, Creature As CreatureBase)
    Sub OnEquip(Item As ItemBase, Creature As CreatureBase)
    Sub OnUnEquip(Item As ItemBase, Creature As CreatureBase)
End Interface
