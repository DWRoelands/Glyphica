Public Class MapTile

    Public Enum MapTileType
        Empty
        Wall
        StairsDown
        StairsUp
        Door
        DoorLocked
    End Enum

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

    Public Sub New(NewType As MapTileType)
        Me.TileType = NewType
    End Sub

    Public Shared Function GetTile(Location As Point) As MapTile
        Return Map(Player1.MapLevel, Location.X, Location.Y)
    End Function

    Public Shared Sub Render(Location As Point)
        Dim t As MapTile.MapTileType = Map(Player1.MapLevel, Location.X, Location.Y).TileType
        If Not MapTile.GetTile(Location).IsRevealed Then
            Exit Sub
        End If

        Console.SetCursorPosition(Location.X, Location.Y)

        Select Case t
            Case MapTile.MapTileType.Wall
                If Map(Player1.MapLevel, Location.X, Location.Y).IsVisible Then
                    Console.ForegroundColor = ConsoleColor.White
                Else
                    Console.ForegroundColor = ConsoleColor.DarkGray
                End If

                ' vertical wall
                If Map(Player1.MapLevel, Location.X - 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                    Map(Player1.MapLevel, Location.X + 1, Location.Y).TileType = MapTile.MapTileType.Empty Then
                    GraphicsCharacterDraw(VERTICALWALL)
                End If

                ' horizontal wall
                If Map(Player1.MapLevel, Location.X, Location.Y - 1).TileType = MapTile.MapTileType.Empty And _
                    Map(Player1.MapLevel, Location.X, Location.Y + 1).TileType = MapTile.MapTileType.Empty Then
                    GraphicsCharacterDraw(HORIZONTALWALL)
                End If

                ' upper-left corner
                If Map(Player1.MapLevel, Location.X - 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                    Map(Player1.MapLevel, Location.X, Location.Y - 1).TileType = MapTile.MapTileType.Empty Then
                    GraphicsCharacterDraw(UPPERLEFTCORNER)
                End If

                ' lower-left corner
                If Map(Player1.MapLevel, Location.X - 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                    Map(Player1.MapLevel, Location.X, Location.Y + 1).TileType = MapTile.MapTileType.Empty Then
                    GraphicsCharacterDraw(LOWERLEFTCORNER)
                End If

                ' upper-right corner
                If Map(Player1.MapLevel, Location.X + 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                    Map(Player1.MapLevel, Location.X, Location.Y - 1).TileType = MapTile.MapTileType.Empty Then
                    GraphicsCharacterDraw(UPPERRIGHTCORNER)
                End If

                ' lower-right corner
                If Map(Player1.MapLevel, Location.X + 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                    Map(Player1.MapLevel, Location.X, Location.Y + 1).TileType = MapTile.MapTileType.Empty Then
                    GraphicsCharacterDraw(LOWERRIGHTCORNER)
                End If

            Case MapTile.MapTileType.Door
                Dim c As ConsoleColor = Console.ForegroundColor
                Console.ForegroundColor = ConsoleColor.DarkYellow
                GraphicsCharacterDraw(DOOR)
                Console.ForegroundColor = c

            Case MapTile.MapTileType.Empty
                If Map(Player1.MapLevel, Location.X, Location.Y).IsVisible Then
                    Console.ForegroundColor = ConsoleColor.White
                Else
                    Console.ForegroundColor = ConsoleColor.DarkGray
                End If
                Console.Write(".")

            Case MapTile.MapTileType.StairsDown
                Console.Write(">")

            Case MapTile.MapTileType.StairsUp
                Console.Write("<")

        End Select
    End Sub


End Class
