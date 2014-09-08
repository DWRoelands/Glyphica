Public Class Player
    Public Property CurrentPosition As Coordinate  '' the current position of the player on the current level of the map
    Public Property TargetPosition As Coordinate   '' the position to which the player is trying to move (after a keypress)

    Public Enum PlayerMoveResult
        Blocked
        Move
    End Enum

    Public Function MoveAttempt() As PlayerMoveResult

        Dim ReturnValue As PlayerMoveResult

        ' What's in the target position?
        Dim TargetContents As String = map(TargetPosition.top).Substring(TargetPosition.left, 1)

        Select Case TargetContents
            Case " "
                ReturnValue = PlayerMoveResult.Move

            Case "#"
                ReturnValue = PlayerMoveResult.Blocked
        End Select

        Return ReturnValue
    End Function

End Class
