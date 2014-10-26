''' <summary>
''' The cap on dexterity bonueses that this item enforces.  Usually an armor effect.
''' </summary>
''' <remarks></remarks>
Public Class Effect_MaximumDexterityBonus
    Inherits ItemEffect_Base
    Implements iAttributeEffect

    Public Property Modifier As Integer Implements iAttributeEffect.Modifier
    Public Property AttributeId As CreatureAttribute.AttributeId Implements iAttributeEffect.AttributeId

    Public Sub New(MaximumDexterityBonues As Integer)
        AttributeId = CreatureAttribute.AttributeId.DexterityMaximumBonus
        Modifier = MaximumDexterityBonues
        IsActiveOnEquip = True
        IsActiveOnPickup = False
        IsRemovedOnDrop = True
        IsRemovedOnUnEquip = True
    End Sub
End Class
