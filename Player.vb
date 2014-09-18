Imports System.Drawing
Public Class Player

    Public Property Location
    Public Property VisualRange As Integer = 5

    Public Enum PlayerMoveResult
        Blocked
        Move
    End Enum

End Class
