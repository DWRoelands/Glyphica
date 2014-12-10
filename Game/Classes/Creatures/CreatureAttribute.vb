Public Class CreatureAttribute
    Public Enum AttributeId
        ArcaneSpellFailureChance
        ArmorClass
        Charisma
        Constitution
        DamageDiceMelee
        Dexterity
        DexterityMaximumBonus
        Weight
        Weight_Max
        HitDice
        HitPoints_Current
        HitPoints_Base
        Initiative
        Intelligence
        Strength
        VisualRange
        Wisdom
    End Enum

    Public Property ID As CreatureAttribute.AttributeId

    Private _Score As Integer
    Public Property Score As Integer
        Get
            If Dice = String.Empty Then
                Return _Score
            Else
                Return RollDice(Dice)
            End If
        End Get
        Set(value As Integer)
            If Dice = String.Empty Then
                _Score = value
            Else
                Throw (New Exception("Can not assign a value to an attribute with a dice formula."))
            End If
        End Set
    End Property
    Public Property Dice As String = String.Empty

    Public Sub New(Attribute_ID As AttributeId, Dice_Formula As String)
        ID = Attribute_ID
        Dice = Dice_Formula
    End Sub

    Public Sub New(Attribute_ID As AttributeId, Attribute_Value As Integer)
        ID = Attribute_ID
        Score = Attribute_Value
    End Sub

    Public Shared Function ScoreGet(Attribute_ID As AttributeId, Creature As CreatureBase) As Integer
        Dim ReturnValue As Integer
        For Each a As CreatureAttribute In Creature.Attributes.Where(Function(x) x.ID = Attribute_ID)
            ReturnValue = a.Score
            Exit For
        Next
        Return ReturnValue
    End Function



End Class
