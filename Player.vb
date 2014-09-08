Public Class Player
    Public Property CurrentPosition As Coordinate  '' the current position of the player on the current level of the map
    Public Property TargetPosition As Coordinate   '' the position to which the player is trying to move (after a keypress)

    Public Enum PlayerMoveResult
        Blocked
        Move
    End Enum

End Class
