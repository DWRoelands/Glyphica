Imports System.Drawing
Public Class Monster
    Inherits Thing

    Public Property HitPoints As Integer
    Public Property DamageDice As String
    Public Property ArmorClass As Integer

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

End Class
