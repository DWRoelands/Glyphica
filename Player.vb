Imports System.Drawing
Public Class Player

    Public Property Location
    Public Property VisualRange As Integer = 5
    Public Property MapLevel As Integer

    Public Enum PlayerMoveResult As Integer
        Undefined
        Blocked
        Move
        Combat
        Thing
    End Enum

End Class
