﻿Imports System.Drawing
Public Class Player
    Public Property CurrentLocation As Point  '' the current position of the player on the current level of the map

    Public Enum PlayerMoveResult
        Blocked
        Move
    End Enum

    Public Sub MoveTo(NewLocation As Point)
        CurrentLocation = NewLocation
    End Sub

    Public Function MoveProcess(TargetLocationTile As ViewPort.MapTile) As PlayerMoveResult
        Dim ReturnValue As PlayerMoveResult
        Select Case TargetLocationTile
            Case ViewPort.MapTile.Empty
                ReturnValue = PlayerMoveResult.Move
            Case ViewPort.MapTile.Solid
                ReturnValue = PlayerMoveResult.Blocked
            Case Else
                ReturnValue = PlayerMoveResult.Move
        End Select
        Return ReturnValue
    End Function


End Class
