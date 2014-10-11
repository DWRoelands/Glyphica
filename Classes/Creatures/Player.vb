Public Class Player
    Inherits CreatureBase

    Public Enum PlayerMoveResult As Integer
        Undefined
        Blocked
        Move
        Combat
        Thing
    End Enum

    Public Sub New()
        Me.VisualRange = 8
    End Sub

    Public Overrides Sub Draw()
        'MyBase.Draw()

        Console.SetCursorPosition(Player1.Location.X - ViewportOrigin.X, Player1.Location.Y - ViewportOrigin.Y)
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("@")
    End Sub

End Class
