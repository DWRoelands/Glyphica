''' <summary>
''' Represents the unmodified weight of an item
''' </summary>
''' <remarks></remarks>
Public Class Effect_Weight
    Inherits ItemEffect_Base
    Implements iAttributeEffect

    Public Property AttributeId As CreatureAttribute.AttributeId Implements iAttributeEffect.AttributeId
    Public Property Modifier As Integer Implements iAttributeEffect.Modifier

    Public Sub New(Weight As Integer)
        AttributeId = CreatureAttribute.AttributeId.Weight
        Modifier = Weight
        IsActiveOnEquip = False
        IsActiveOnPickup = True
        IsRemovedOnDrop = True
        IsRemovedOnUnEquip = False
    End Sub
End Class
