Imports System.Math
Public Class CreatureBase
    Inherits Base

    Public Enum CreatureAlignment
        Lawful
        Neutral
        Chaotic
    End Enum

    Public Enum CreatureClass
        Fighter
        Rogue
        Wizard
    End Enum

#Region "Ability Score Properties"
    Public Property BaseStrength As Integer
    Public Property BaseDexterity As Integer
    Public Property BaseConstitution As Integer
    Public Property BaseIntelligence As Integer
    Public Property BaseWisdom As Integer
    Public Property BaseCharisma As Integer

    Public Property StrengthModifier As Integer
    Public Property DexterityModifier As Integer
    Public Property ConstitutionModifier As Integer
    Public Property IntelligenceModifier As Integer
    Public Property WisdonModifier As Integer
    Public Property CharismaModifier As Integer

    Public ReadOnly Property Strength As Integer
        Get
            Return BaseStrength + StrengthModifier
        End Get
    End Property

    Public ReadOnly Property Intelligence As Integer
        Get
            Return BaseIntelligence + IntelligenceModifier
        End Get
    End Property

    Public ReadOnly Property Wisdom As Integer
        Get
            Return BaseWisdom + WisdonModifier
        End Get
    End Property

    Public ReadOnly Property Dexterity As Integer
        Get
            Return BaseDexterity + DexterityModifier
        End Get
    End Property

    Public ReadOnly Property Constitution As Integer
        Get
            Return BaseConstitution + ConstitutionModifier
        End Get
    End Property

    Public ReadOnly Property Charisma As Integer
        Get
            Return BaseCharisma + CharismaModifier
        End Get
    End Property

#End Region

#Region "Equipment Properties"
    Public Property TotalWeightCarried As Integer
    Public Property EquippedItems As New List(Of EquippableItem)
    Public Property EquippedArmor As Armor

#End Region

    Public Property Alignment As CreatureAlignment
    Public Property [Class] As CreatureClass

    Public Property VisualRange As Integer
    Public Property VisibleCells As New List(Of Point)

    Public Property HitPointsCurrent As Integer
    Public Property HitPointsMax As Integer

    Public Property DamageDice As String

    Public Property Initiative As Integer

    Public Property BaseArmorClass As Integer
    Public Property ArmorClassModifier As Integer
    Public ReadOnly Property ArmorClass As Integer
        Get
            Return BaseArmorClass + ArmorClassModifier
        End Get
    End Property

    Public Property ArcaneSpellFailureChance As Decimal

    Private _HitDice As String
    Public Property HitDice As String
        Get
            Return _HitDice
        End Get
        Set(value As String)
            _HitDice = value
            HitPointsMax = Dice.RollDice(value)
            HitPointsCurrent = HitPointsMax
        End Set
    End Property

    Public Shared Function Find(Location As Point) As CreatureBase
        Dim ReturnValue As CreatureBase = Nothing
        For Each c As CreatureBase In Creatures
            If c.Location.X = Location.X AndAlso c.Location.Y = Location.Y Then
                ReturnValue = c
                Exit For
            End If
        Next
        Return ReturnValue
    End Function

    Public Sub EquipmentEffectsProcess()
        For Each item As EquippableItem In EquippedItems
            item.Process(Me)
        Next
    End Sub

    Public Shared Function FindClosest(Location As Point) As CreatureBase

        Dim ReturnValue As CreatureBase = Nothing
        Dim ClosestDistance As Decimal = 0
        For Each c As CreatureBase In Creatures
            If ReturnValue Is Nothing Then
                ReturnValue = c
            Else
                If c.Location <> Location Then
                    If DistanceGet(Location, c.Location) < DistanceGet(Location, ReturnValue.Location) Then
                        ReturnValue = c
                    End If
                End If
            End If
        Next

        ' If the specified location isn't the player's location, then this method is being called by a monster looking for a target
        If Player1.Location <> Location Then
            If DistanceGet(Location, Player1.Location) < DistanceGet(Location, ReturnValue.Location) Then
                ReturnValue = Player1
            End If
        End If

        Return ReturnValue

    End Function


    Public Shared Sub Kill(DeadCreature As CreatureBase)
        Artifacts.Add(New Corpse(DeadCreature.MapLevel, DeadCreature.Location, DeadCreature.Name))
        Creatures.Remove(DeadCreature)
    End Sub

    ' TODO: Maybe put this into a "Utility" module
    Public Shared Function DistanceGet(Location1 As Point, Location2 As Point) As Decimal
        Return Sqrt((Abs(Location2.X - Location1.X) ^ 2) + (Abs(Location2.Y - Location1.Y) ^ 2))
    End Function



End Class
