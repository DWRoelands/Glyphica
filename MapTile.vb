Imports System.Drawing
Public Class MapTile

    Public Enum MapTileType
        Empty
        Wall
        StairsDown
        StairsUp
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
                Case Else
                    Return False
            End Select
        End Get
    End Property
    Public ReadOnly Property DisplayCharacter As Char
        Get
            Select Case TileType
                Case MapTileType.Empty
                    Return "."c
                Case MapTileType.Wall
                    Return "#"c
                Case MapTileType.StairsDown
                    Return ">"c
                Case MapTileType.StairsUp
                    Return "<"c
                Case Else
                    Return String.Empty
            End Select
        End Get
    End Property

    Public Sub New(NewType As MapTileType)
        Me.TileType = NewType
        'Me.Location = NewLocation
    End Sub

End Class
