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
    Public Property WisdomModifier As Integer
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
            Return BaseWisdom + WisdomModifier
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
    Public Property Inventory As New List(Of ItemBase)


#End Region

    Public Property Alignment As CreatureAlignment
    Public Property [Class] As CreatureClass

    Public Property VisualRange As Integer
    Public Property VisibleCells As New List(Of Point)

    Public Property HitPointsCurrent As Integer
    Public Property HitPointsMax As Integer

    Public Property DamageDice As String
    Public Property DamageModifier As Integer

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

    Public ReadOnly Property EquippedAmmunition As AmmunitionBase
        Get
            Dim ReturnValue As AmmunitionBase = Nothing
            For Each AmmoBunch As AmmunitionBase In Me.Inventory.Where(Function(w) TypeOf w Is AmmunitionBase)
                If AmmoBunch.IsEquipped Then
                    ReturnValue = AmmoBunch
                    Exit For
                End If
            Next
            Return ReturnValue
        End Get
    End Property

    Public ReadOnly Property EquippedWeapon As WeaponBase
        Get
            Dim ReturnValue As WeaponBase = Nothing
            For Each Weapon As WeaponBase In Me.Inventory.Where(Function(w) TypeOf w Is WeaponBase)
                If Weapon.IsEquipped Then
                    ReturnValue = Weapon
                    Exit For
                End If
            Next
            Return ReturnValue
        End Get
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

    Public Shared Sub ItemEffectsProcess(Creature As CreatureBase)

        ' reset all modifiers to 0
        With Creature
            .ArmorClassModifier = 0
            .DamageModifier = 0
            .StrengthModifier = 0
            .IntelligenceModifier = 0
            .WisdomModifier = 0
            .DexterityModifier = 0
            .ConstitutionModifier = 0
            .CharismaModifier = 0
        End With

        For Each item As ItemBase In Creature.Inventory
            item.Process(Creature)
        Next
    End Sub

    Public Function FindClosest() As CreatureBase

        Dim ReturnValue As CreatureBase = Nothing
        Dim ClosestDistance As Decimal = 0
        For Each c As CreatureBase In Creatures
            If ReturnValue Is Nothing Then
                ReturnValue = c
            Else
                If c.Location <> Me.Location Then
                    If DistanceGet(Me.Location, c.Location) < DistanceGet(Me.Location, ReturnValue.Location) Then
                        ReturnValue = c
                    End If
                End If
            End If
        Next

        ' If the specified location isn't the player's location, then this method is being called by a monster looking for a target
        If Player1.Location <> Me.Location Then
            If DistanceGet(Me.Location, Player1.Location) < DistanceGet(Me.Location, ReturnValue.Location) Then
                ReturnValue = Player1
            End If
        End If

        Return ReturnValue

    End Function

    Public Shared Sub Kill(DeadCreature As CreatureBase)
        Items.Add(New Corpse(DeadCreature.MapLevel, DeadCreature.Location, DeadCreature.Name))
        Creatures.Remove(DeadCreature)
    End Sub

    Public Sub Equip(Item As ItemBase)

        Select Case Item.GetType.BaseType
            Case GetType(ArmorBase)
                ' only one armor item can be equipped, so we unequip any equipped armor
                For Each ArmorItem As ArmorBase In Me.Inventory.OfType(Of ArmorBase)()
                    ArmorItem.IsEquipped = False
                Next
                Item.IsEquipped = True
                Exit Sub

            Case GetType(WeaponBase)
                ' only one weapon can be equipped, so we unequip any equipped weapon
                ' TODO: support dual-wielding
                For Each WeaponItem As WeaponBase In Me.Inventory.OfType(Of WeaponBase)()
                    WeaponItem.IsEquipped = False
                Next
                Item.IsEquipped = True
                Exit Sub

                ' Only one type of amunition can be equipped, so we unequip and equipped ammunition
            Case GetType(AmmunitionBase)
                For Each AmmoItem As AmmunitionBase In Me.Inventory.OfType(Of AmmunitionBase)()
                    AmmoItem.IsEquipped = False
                Next
                Item.IsEquipped = True
                Exit Sub
        End Select

        ' if we reach this point in the method, we have attempted to equip an item for which
        ' there is no "equipping" code.
        Throw New Exception("Equip() failed because it didn't know how to equip an item: " & Item.Name)

    End Sub






End Class
