Public Class MapTile

    Public Enum MapTileType
        Empty
        Wall
        StairsDown
        StairsUp
        Door
        DoorLocked
    End Enum

    Public Enum BitmapId
        Empty
        WallUpperLeft
        WallVertical
        WallLowerLeft
        WallHorizontal
        WallUpperRight
        WallLowerRight
        Floor
        Door
        Player
    End Enum

    Public Property MapLevel As Integer
    Public Property Location As Point
    Public Property TileType As MapTileType
    Public Property IsRevealed As Boolean = False
    Public Property IsVisible As Boolean = True
    Public ReadOnly Property BlocksVision As Boolean
        Get
            Select Case TileType
                Case MapTileType.Empty
                    Return False
                Case MapTileType.StairsDown
                    Return False
                Case MapTileType.StairsUp
                    Return False
                Case MapTileType.Wall
                    Return True
                Case MapTileType.Door, MapTileType.DoorLocked
                    Return True
                Case Else
                    Return False
            End Select
        End Get
    End Property

    Public Shared Function BitmapIdGet(x As Integer, y As Integer) As BitmapId
        Dim Location As New Point(x, y)
        If Main.Map.TileGet(Main.Player1.MapLevel, x, y).IsRevealed Then
            Dim t As MapTileType = Main.Map.TileGet(Main.Player1.MapLevel, Location.X, Location.Y).TileType
            Select Case t
                Case MapTileType.Wall

                    ' vertical wall
                    If Main.Map.TileGet(Main.Player1.MapLevel, Location.X - 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                         Main.Map.TileGet(Main.Player1.MapLevel, Location.X + 1, Location.Y).TileType = MapTile.MapTileType.Empty Then
                        Return BitmapId.WallVertical
                    End If

                    ' horizontal wall
                    If Main.Map.TileGet(Main.Player1.MapLevel, Location.X, Location.Y - 1).TileType = MapTile.MapTileType.Empty And _
                         Main.Map.TileGet(Main.Player1.MapLevel, Location.X, Location.Y + 1).TileType = MapTile.MapTileType.Empty Then
                        Return BitmapId.WallHorizontal
                    End If

                    ' upper-left corner
                    If Main.Map.TileGet(Main.Player1.MapLevel, Location.X - 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                         Main.Map.TileGet(Main.Player1.MapLevel, Location.X, Location.Y - 1).TileType = MapTile.MapTileType.Empty Then
                        Return BitmapId.WallUpperLeft
                    End If

                    ' lower-left corner
                    If Main.Map.TileGet(Main.Player1.MapLevel, Location.X - 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                         Main.Map.TileGet(Main.Player1.MapLevel, Location.X, Location.Y + 1).TileType = MapTile.MapTileType.Empty Then
                        Return BitmapId.WallLowerLeft
                    End If

                    ' upper-right corner
                    If Main.Map.TileGet(Main.Player1.MapLevel, Location.X + 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                         Main.Map.TileGet(Main.Player1.MapLevel, Location.X, Location.Y - 1).TileType = MapTile.MapTileType.Empty Then
                        Return BitmapId.WallUpperRight
                    End If

                    ' lower-right corner
                    If Main.Map.TileGet(Main.Player1.MapLevel, Location.X + 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                         Main.Map.TileGet(Main.Player1.MapLevel, Location.X, Location.Y + 1).TileType = MapTile.MapTileType.Empty Then
                        Return BitmapId.WallLowerRight
                    End If

                Case MapTileType.Empty
                    Return BitmapId.Floor

                Case MapTileType.Door
                    Return BitmapId.Door

            End Select
        Else
            Return BitmapId.Empty
        End If

        Return BitmapId.Empty
    End Function

    Public Sub render()

    End Sub

End Class
