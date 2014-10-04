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

    Public Overrides Sub Draw()
        'MyBase.Draw()

        Console.SetCursorPosition(Player1.Location.X - ViewportOrigin.X, Player1.Location.Y - ViewportOrigin.Y)
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("@")
    End Sub

End Class
