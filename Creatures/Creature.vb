Imports System.Math
Public Class Creature
    Inherits Thing

    Public Property HitPoints As Integer
    Public Property DamageDice As String
    Public Property ArmorClass As Integer
    Public Property Initiative As Integer
    Public Property VisualRange As Integer
    Public Property VisibleCells As New List(Of Point)

    Public Property Strength As Integer
    Public Property Dexterity As Integer
    Public Property Constitution As Integer
    Public Property Intelligence As Integer
    Public Property Wisdom As Integer
    Public Property Charisma As Integer





    Private _HitDice As String
    Public Property HitDice As String
        Get
            Return _HitDice
        End Get
        Set(value As String)
            _HitDice = value
            HitPoints = Dice.RollDice(value)
        End Set
    End Property

    Public Enum MonsterType As Integer
        Kobold
    End Enum

    Public Shared Function Find(Location As Point) As Creature
        Dim ReturnValue As Creature = Nothing
        For Each c As Creature In Creatures
            If c.Location.X = Location.X AndAlso c.Location.Y = Location.Y Then
                ReturnValue = c
                Exit For
            End If
        Next
        Return ReturnValue
    End Function

    Public Shared Function FindClosest(Location As Point) As Creature

        Dim ReturnValue As Creature = Nothing
        Dim ClosestDistance As Decimal = 0
        For Each c As Creature In Creatures
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

    Public Shared Sub Kill(DeadCreature As Creature)
        Artifacts.Add(New Corpse(DeadCreature.MapLevel, DeadCreature.Location, DeadCreature.Name))
        Creatures.Remove(DeadCreature)
    End Sub


    ' TODO: Maybe put this into a "Utility" module
    Public Shared Function DistanceGet(Location1 As Point, Location2 As Point) As Decimal
        Return Sqrt((Abs(Location2.X - Location1.X) ^ 2) + (Abs(Location2.Y - Location1.Y) ^ 2))
    End Function



End Class
