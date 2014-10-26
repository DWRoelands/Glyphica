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

    Public Attributes As New List(Of CreatureAttribute)
    Public ItemEffects As New List(Of ItemEffect_Base)
    Public VisibleCells As New List(Of Point)

    Public Sub New()

    End Sub

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
