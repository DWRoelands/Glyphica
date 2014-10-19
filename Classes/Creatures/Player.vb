Public Class Player
    Inherits CreatureBase

    Public Enum MoveBeforeResult As Integer
        Undefined
        Blocked
        Move
        Combat
        Thing
    End Enum

    Public Enum MoveAfterResult
        Container
        Trap
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

    Public Sub PreMoveProcess(ToLocation As Point)

        ' First, is there a monster here?
        Dim Enemy As CreatureBase = Nothing
        For Each m As CreatureBase In Creatures
            If m.MapLevel = Player1.MapLevel AndAlso m.Location.X = ToLocation.X AndAlso m.Location.Y = ToLocation.Y Then
                Enemy = CreatureBase.Find(ToLocation)
                Exit For
            End If
        Next

        ' If there's a monster here, perform a round of melee combat and exit the sub
        If Enemy IsNot Nothing Then
            CombatResolve(Enemy, CombatType.Melee)
            Exit Sub
        End If

        ' If there's a wall, humiliate the player
        If MapTile.GetTile(ToLocation).TileType = MapTile.MapTileType.Wall Then
            Viewport.MessageWrite("You bump your head.")
            Exit Sub
        End If

        '' If we reach this point, then there is nothing in the target location that requires special
        '' pre-move handling
        Player1.Location = ToLocation


    End Sub





    ''' <summary>
    ''' Perform all of the checks and processing required before a player moves into a chosen location
    ''' </summary>
    ''' <param name="ToLocation">A System.Drawing.Point represnting the two-dimensional position on the current map level</param>
    ''' <returns>A PlayerMoveResult enum</returns>
    ''' <remarks></remarks>
    Public Function MoveBefore(ToLocation As Point) As MoveBeforeResult
        Dim ReturnValue As Player.MoveBeforeResult = Nothing

        ' First, is there a monster here?  If so, the result is combat
        Dim MonsterFound As Boolean = False
        For Each m As CreatureBase In Creatures
            If m.MapLevel = Player1.MapLevel AndAlso m.Location.X = ToLocation.X AndAlso m.Location.Y = ToLocation.Y Then
                MonsterFound = True
                ReturnValue = Player.MoveBeforeResult.Combat
                Exit For
            End If
        Next

        ' No monster 
        ' Check for collision with map feature
        If Not MonsterFound Then
            Select Case MapTile.GetTile(ToLocation).TileType
                Case MapTile.MapTileType.Empty
                    ReturnValue = Player.MoveBeforeResult.Move
                Case MapTile.MapTileType.Wall
                    ReturnValue = Player.MoveBeforeResult.Blocked
                Case MapTile.MapTileType.Door
                    Map(Player1.MapLevel, ToLocation.X, ToLocation.Y).TileType = MapTile.MapTileType.Empty
                    Debug.WriteLine("The door opens")
                    Return Player.MoveBeforeResult.Move
                Case MapTile.MapTileType.DoorLocked
                    Debug.WriteLine("The door is locked")
                    Return Player.MoveBeforeResult.Blocked
            End Select
        End If

        Return ReturnValue

    End Function

    Public Function MoveAfter() As MoveAfterResult

    End Function

End Class
