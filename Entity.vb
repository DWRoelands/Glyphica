Imports System.Drawing
Public Class Entity

    Public Enum EntityType
        Player
        Monster
    End Enum

    Public Property [Type] As EntityType
    Public Property MapLevel As Integer
    Public Property Location As Point
End Class
