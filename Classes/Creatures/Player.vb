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
    End Sub

    Public Overrides Sub Draw()
        'MyBase.Draw()

        'Console.SetCursorPosition(Main.Player1.Location.X - Main.vp.Origin.X, Main.Player1.Location.Y - Main.vp.Origin.Y)
        'Console.ForegroundColor = ConsoleColor.White
        'Console.Write("@")
    End Sub

    Public Sub MoveProcess(ToLocation As Point)

        ' PRE-MOVE PROCESSING
        ' First, is there a monster in the target location?
        Dim Enemy As CreatureBase = Nothing
        For Each m As CreatureBase In Main.Creatures
            If m.MapLevel = Main.Player1.MapLevel AndAlso m.Location.X = ToLocation.X AndAlso m.Location.Y = ToLocation.Y Then
                Enemy = CreatureBase.Find(ToLocation)
                Exit For
            End If
        Next

        ' If there's a monster in the target location, perform a round of melee combat and exit the sub
        If Enemy IsNot Nothing Then
            Combat.Resolve(Enemy, CombatType.Melee)
            Main.Refresh()
            Exit Sub
        End If

        ' If there's a wall, humiliate the player
        If Main.Map.TileGet(Me.MapLevel, ToLocation.X, ToLocation.Y).TileType = MapTile.MapTileType.Wall Then
            Main.MessageWrite("You bump your head.")
            Exit Sub
        End If

        ' END PRE-MOVE PROCESSING

        '' If we reach this point, then there is nothing in the target location that requires special
        '' pre-move handling
        Main.Player1.Location = ToLocation
        Main.Refresh()

        ' POST-MOVE PROCESSING
        ' is there a container in the same spot as the player?
        Dim Container As ContainerBase = Nothing
        For Each i As ItemBase In Main.Items.Where(Function(x) TypeOf x Is ContainerBase)
            If i.Location = Me.Location Then
                Container = i
            End If
        Next

        If Container IsNot Nothing Then
            'If Ask(String.Format("Do you want to search the {0}?", Container.Name)) Then
            '    RefreshViewport = True
            '    InventoryManage(Container)
            '    If RefreshViewport Then
            '        Main.vp.Refresh()
            '        Exit Sub
            '    End If
            'End If
        End If

        ' END POST-MOVE PROCESSING

    End Sub



    Public Sub PreMoveProcess(ToLocation As Point)

        ' First, is there a monster here?
        Dim Enemy As CreatureBase = Nothing
        For Each m As CreatureBase In Main.Creatures
            If m.MapLevel = Main.Player1.MapLevel AndAlso m.Location.X = ToLocation.X AndAlso m.Location.Y = ToLocation.Y Then
                Enemy = CreatureBase.Find(ToLocation)
                Exit For
            End If
        Next

        ' If there's a monster here, perform a round of melee combat and exit the sub
        If Enemy IsNot Nothing Then
            Combat.Resolve(Enemy, CombatType.Melee)
            Exit Sub
        End If

        ' If there's a wall, humiliate the player
        If Main.Map.TileGet(Me.MapLevel, ToLocation.X, ToLocation.Y).TileType = MapTile.MapTileType.Wall Then
            Main.MessageWrite("You bump your head.")
            Exit Sub
        End If

        '' If we reach this point, then there is nothing in the target location that requires special
        '' pre-move handling
        Main.Player1.Location = ToLocation


    End Sub

    Public Sub PostMoveProcess()

        Dim RefreshViewport As Boolean = False

        ' is there a container in the same spot as the player?
        Dim Container As ContainerBase = Nothing
        For Each i As ItemBase In Main.Items.Where(Function(x) TypeOf x Is ContainerBase)
            If i.Location = Me.Location Then
                Container = i
            End If
        Next

        If Container IsNot Nothing Then
            If Ask(String.Format("Do you want to search the {0}?", Container.Name)) Then
                RefreshViewport = True
                InventoryManage(Container)
                If RefreshViewport Then
                    Main.vp.Refresh()
                    Exit Sub
                End If
            End If
        End If

    End Sub

End Class
