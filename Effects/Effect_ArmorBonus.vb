Public Class Effect_ArmorBonus
    Inherits ItemEffect_Base
    Implements iAttributeEffect

    Public Property AttributeId As CreatureAttribute.AttributeId Implements iAttributeEffect.AttributeId
    Public Property Modifier As Integer Implements iAttributeEffect.Modifier

    Public Sub New(ArmorBonus)
        AttributeId = CreatureAttribute.AttributeId.ArmorClass
        Modifier = ArmorBonus
        IsActiveOnEquip = True
        IsActiveOnPickup = False
        IsRemovedOnDrop = True
        IsRemovedOnUnEquip = True
    End Sub
End Class
