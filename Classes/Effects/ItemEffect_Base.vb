Public MustInherit Class ItemEffect_Base
    Implements IItemEffect

    Public Enum ItemEffectId
        ArcaneSpellFailureChance
        ArmorBonus
        MaximumDexterityBonus
        Weight
    End Enum

    Public Sub New(Item As ItemBase)
    End Sub

    Public Overridable Sub OnDrop(Item As ItemBase, Creature As CreatureBase) Implements IItemEffect.OnDrop
        If IsRemovedOnDrop Then
            Creature.ItemEffects.Remove(Me)
        End If
    End Sub

    Public Overridable Sub OnEquip(Item As ItemBase, Creature As CreatureBase) Implements IItemEffect.OnEquip
        If IsActiveOnEquip Then
            Creature.ItemEffects.Add(Me)
        End If
    End Sub

    Public Overridable Sub OnPickup(Item As ItemBase, Creature As CreatureBase) Implements IItemEffect.OnPickup
        If IsActiveOnPickup Then
            Creature.ItemEffects.Add(Me)
        End If
    End Sub

    Public Overridable Sub OnUnEquip(Item As ItemBase, Creature As CreatureBase) Implements IItemEffect.OnUnEquip
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
