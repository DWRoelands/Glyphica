Imports System.Drawing
Public Class Player

    Public Property Location

    Public Enum PlayerMoveResult
        Blocked
        Move
    End Enum

    Public Sub MoveTo(NewLocation As Point)
        Location = NewLocation
    End Sub

    'Public Function MoveProcess(TargetLocationTile As ViewPort.MapTile) As PlayerMoveResult
    '    Dim ReturnValue As PlayerMoveResult
    '    Select Case TargetLocationTile
    '        Case ViewPort.MapTile.Empty
    '            ReturnValue = PlayerMoveResult.Move
    '        Case ViewPort.MapTile.Solid
    '            ReturnValue = PlayerMoveResult.Blocked
    '        Case Else
    '            ReturnValue = PlayerMoveResult.Move
    '    End Select
    '    Return ReturnValue
    'End Function

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
