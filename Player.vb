Imports System.Drawing
Public Class Player

    Public Property Location
    Public Property VisualRange As Integer = 5

    Public Enum PlayerMoveResult
        Blocked
        Move
    End Enum

    Public Sub MoveTo(NewLocation As Point)
        Location = NewLocation
    End Sub

    Public Function PlayerMoveAttempt(Target As MapTile) As PlayerMoveResult
        Dim ReturnValue As PlayerMoveResult
        Select Case Target.Type
            Case MapTile.TileType.Empty
                Returnvalue = PlayerMoveResult.Move
            Case MapTile.TileType.Wall
                ReturnValue = PlayerMoveResult.Blocked
        End Select
        Return ReturnValue
    End Function



End Class
