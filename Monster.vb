Imports System.Drawing
Public Class Monster
    Public Enum MonsterType As Integer
        Kobold
    End Enum

    Public Property Name As String
    Public Property MapLevel As Integer
    Public Property Location As Point
    Public Property HitDice As String
    Public Property HitPoints As Integer
    Public Property DamageDice As String
    Public Property ArmorClass As Integer
    Public Property Description As String
    Public Property DisplayCharacter As String
    Public Property DisplayColor As ConsoleColor
End Class
