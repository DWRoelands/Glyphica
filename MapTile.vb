Imports System.Drawing
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
    'Public Property Location As Point
    Public Property IsVisible As Boolean = False
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
        'Me.Location = NewLocation
    End Sub

End Class
