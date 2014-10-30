Public Class CreatureAttribute
    Public Enum AttributeId
        Alignment
        ArcaneSpellFailureChance
        ArmorClass
        CharacterClass
        Charisma
        Constitution
        Damage_Base
        Dexterity
        DexterityMaximumBonus
        Weight
        Weight_Max
        HitDice
        HitPoints
        HitPoints_Base
        Initiative
        Intelligence
        Strength
        VisualRange
        Wisdom
    End Enum

    Public Property ID As CreatureAttribute.AttributeId
    Public Property Value As String

    Public Sub New(Attribute_ID As AttributeId, Attribute_Value As Integer)
        ID = Attribute_ID
        Value = Attribute_Value
    End Sub
End Class
