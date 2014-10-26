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

    Public ID As CreatureAttribute.AttributeId
    Public Value As String
End Class
