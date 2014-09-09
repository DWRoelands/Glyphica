Public Class Player
    Public Property CurrentLocation As Coordinate  '' the current position of the player on the current level of the map

    Public Enum PlayerMoveResult
        Blocked
        Move
    End Enum

    Public Sub MoveTo(NewLocation As Coordinate)
        Dim OldLocation As Coordinate = CurrentLocation
        CurrentLocation = NewLocation
    End Sub

    Public Function MoveProcess(TargetLocationSymbol As String) As PlayerMoveResult
        Dim ReturnValue As PlayerMoveResult
        Select Case TargetLocationSymbol
            Case " "
                ReturnValue = PlayerMoveResult.Move
            Case "#"
                ReturnValue = PlayerMoveResult.Blocked
            Case Else
                ReturnValue = PlayerMoveResult.Move
        End Select
        Return ReturnValue
    End Function


End Class
