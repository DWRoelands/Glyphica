Public Class Effect_ArcaneSpellFailureChance
    Inherits ItemEffect_Base
    Implements IAttributeEffect

    Public Property AttributeId As CreatureAttribute.AttributeId Implements IAttributeEffect.AttributeId
    Public Property Modifier As Integer Implements IAttributeEffect.Modifier

    Public Sub New(ArcaneSpellFailureChance)
        AttributeId = CreatureAttribute.AttributeId.ArcaneSpellFailureChance
        Modifier = ArcaneSpellFailureChance
        IsActiveOnEquip = True
        IsActiveOnPickup = False
        IsRemovedOnDrop = True
        IsRemovedOnUnEquip = True
    End Sub
End Class
