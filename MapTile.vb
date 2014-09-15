Imports System.Drawing
Public Class MapTile

    Public Enum TileType
        Empty
        Wall
        StairsDown
        StairsUp
    End Enum

    Public Property Type As TileType
    Public Property Location As Point
    Public ReadOnly Property BlocksVision As Boolean
        Get
            Select Case Type
                Case TileType.Empty
                    Return False
                Case TileType.StairsDown
                    Return False
                Case TileType.StairsUp
                    Return False
                Case TileType.Wall
                    Return True
                Case Else
                    Return False
            End Select
        End Get
    End Property
    Public ReadOnly Property DisplayCharacter As Char
        Get
            Select Case Type
                Case TileType.Empty
                    Return " "c
                Case TileType.Wall
                    Return "#"c
                Case TileType.StairsDown
                    Return ">"c
                Case TileType.StairsUp
                    Return "<"c
                Case Else
                    Return String.Empty
            End Select
        End Get
    End Property

    Public Sub New(NewType As TileType, NewLocation As Point)
        Me.Type = NewType
        Me.Location = NewLocation
    End Sub

End Class
