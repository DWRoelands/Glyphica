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

    Public Sub PostMoveProcess()

        ' is there a container in the same spot as the player?
        Dim Container As ContainerBase = Nothing
        For Each i As ItemBase In Items.Where(Function(x) TypeOf x Is ContainerBase)
            If i.Location = Me.Location Then
                Container = i
            End If
        Next

        If Container IsNot Nothing Then
            If Ask(String.Format("Do you want to search the {0}?", Container.Name)) Then

            End If
        End If




    End Sub

End Class
