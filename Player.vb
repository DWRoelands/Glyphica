Imports System.Drawing
Public Class Player

    Public Property Location
    Public Property VisualRange As Integer = 5
    Public Property VisibleCells As New List(Of Point)

    Public Enum PlayerMoveResult
        Blocked
        Move
    End Enum

    Public Sub MoveTo(NewLocation As Point)
        Location = NewLocation
    End Sub

    Public Function PlayerMoveAttempt(Target As MapTile) As PlayerMoveResult
        Dim ReturnValue As PlayerMoveResult
        Select Case Target.TileType
            Case MapTile.MapTileType.Empty
                Returnvalue = PlayerMoveResult.Move
            Case MapTile.MapTileType.Wall
                ReturnValue = PlayerMoveResult.Blocked
        End Select
        Return ReturnValue
    End Function



End Class
