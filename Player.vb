Imports System.Drawing
Public Class Player
    Inherits Monster

    Public Property VisualRange As Integer = 5
    Public Property VisibleCells As New List(Of Point)

    Public Enum PlayerMoveResult As Integer
        Undefined
        Blocked
        Move
        Combat
        Thing
    End Enum

End Class
