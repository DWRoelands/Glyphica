﻿Public Class CreatureBase
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

    Public Property Attributes As New List(Of CreatureAttribute)
    Public Property ItemEffects As New List(Of ItemEffect_Base)
    Public Property VisibleCells As New List(Of Point)
    Public Property Alignment As CreatureAlignment
    Public Property [Class] As CreatureClass

    Public Sub New()

    End Sub

    Public ReadOnly Property DamageDiceMelee As String
        Get
            If Me.EquippedWeapon IsNot Nothing Then
                Return Me.EquippedWeapon.Damage
            Else
                Return Me.AttributeGet(CreatureAttribute.AttributeId.DamageDiceMelee)
            End If
        End Get
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

    Public Sub Pickup(Item As ItemBase)
        Item.Pickup(Me)
    End Sub

    Public Sub Drop(Item As ItemBase)
        Item.UnEquip(Me)
        Item.Drop(Me)
    End Sub

    Public Sub UnEquip(Item As ItemBase)
        Item.UnEquip(Me)
    End Sub

    Public Sub Equip(Item As ItemBase)

        Select Case Item.GetType.BaseType
            Case GetType(ArmorBase)
                ' only one armor item can be equipped, so we unequip any equipped armor
                For Each ArmorItem As ArmorBase In Me.Inventory.OfType(Of ArmorBase)()
                    Me.UnEquip(ArmorItem)
                Next

            Case GetType(WeaponBase)
                ' only one weapon can be equipped, so we unequip any equipped weapon
                ' TODO: support dual-wielding
                For Each WeaponItem As WeaponBase In Me.Inventory.OfType(Of WeaponBase)()
                    Me.UnEquip(WeaponItem)
                Next

                ' Only one type of amunition can be equipped, so we unequip and equipped ammunition
            Case GetType(AmmunitionBase)
                For Each AmmoItem As AmmunitionBase In Me.Inventory.OfType(Of AmmunitionBase)()
                    Me.UnEquip(AmmoItem)
                Next

            Case Else
                Throw New Exception("Equip() failed because it didn't know how to equip an item: " & Item.Name)
        End Select

        Item.Equip(Me)

    End Sub

    Public Function AttributeGet(Attribute_ID As CreatureAttribute.AttributeId) As Integer
        Dim ReturnValue As Integer

        ' Calculate the base score for the attribute
        ReturnValue = (From a In Attributes Where a.ID = Attribute_ID).Sum(Function(x) x.Score)

        ' Add the effects of any items
        For Each ie As IAttributeEffect In ItemEffects.OfType(Of IAttributeEffect)()
            If ie.AttributeId = Attribute_ID Then
                ReturnValue += ie.Modifier
            End If
        Next

        Return ReturnValue

    End Function

    Public Sub AttributeSet(Attribute_ID As CreatureAttribute.AttributeId, NewScore As Integer)
        For Each a As CreatureAttribute In Attributes
            If a.ID = Attribute_ID Then a.Score = NewScore
            Exit For
        Next
    End Sub

End Class
