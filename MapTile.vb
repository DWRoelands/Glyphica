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

End Class
