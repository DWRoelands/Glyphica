Imports System.Math
Public Class Monster
    Inherits Thing

    Public Property HitPoints As Integer
    Public Property DamageDice As String
    Public Property ArmorClass As Integer
    Public Property Initiative As Integer

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

    Public Shared Function Find(Location As Point) As Monster
        Dim ReturnValue As Monster = Nothing
        For Each m As Monster In Monsters
            If m.Location.X = Location.X AndAlso m.Location.Y = Location.Y Then
                ReturnValue = m
                Exit For
            End If
        Next
        Return ReturnValue
    End Function

    Public Shared Function FindClosest(Location As Point) As Monster

        Dim ReturnValue As Monster = Nothing
        Dim ClosestDistance As Decimal = 0
        For Each m As Monster In Monsters
            If ReturnValue Is Nothing Then
                ReturnValue = m
            Else
                If m.Location <> Location Then
                    If DistanceGet(Location, m.Location) < DistanceGet(Location, ReturnValue.Location) Then
                        ReturnValue = m
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

    ' TODO: Maybe put this into a "Utility" module
    Public Shared Function DistanceGet(Location1 As Point, Location2 As Point) As Decimal
        Return Sqrt((Abs(Location2.X - Location1.X) ^ 2) + (Abs(Location2.Y - Location1.Y) ^ 2))
    End Function



End Class
