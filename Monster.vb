Imports System.Drawing
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

End Class
