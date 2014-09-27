Imports System.Drawing
Public Class Monster
    Public Enum MonsterType As Integer
        Kobold
    End Enum

    Public Property Name As String
    Public Property Location As Point
    Public Property MapLevel As Integer

    Private _HitDice As String
    Public Property HitDice As String
        Get
            Return _HitDice
        End Get
        Set(value As String)
            HitPoints = Dice.RollDice(value)
        End Set
    End Property

    Public Property HitPoints As Integer
    Public Property DamageDice As String
    Public Property ArmorClass As Integer
    Public Property Description As String
    Public Property DisplayCharacter As String
    Public Property DisplayColor As ConsoleColor
End Class
