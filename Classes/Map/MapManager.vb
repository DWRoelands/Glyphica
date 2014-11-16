Public Class MapManager
    Private Map As List(Of MapTile)
    Private Dimensions As List(Of Size)

    Public Property CurrentLevel As Integer

    Public Sub New()
        Map = New List(Of MapTile)
        Dimensions = New List(Of Size)
    End Sub

    Public Function DimensionsGet() As Size
        Return Dimensions(Me.CurrentLevel)
    End Function

    Public Function TileGet(MapLevel As Integer, x As Integer, y As Integer) As MapTile
        Return (From m As MapTile In Map Where m.MapLevel = MapLevel And m.Location.X = x And m.Location.Y = y).ToList(0)
    End Function

    Public Sub Load()

        Dim LevelSize As New Size


        ' this is temporary code which loads a text file map into the integer-based array which is the live map
        Dim MapLocation As String
        If System.IO.Directory.Exists(TESTMAPLOCATION1) Then
            MapLocation = TESTMAPLOCATION1
        Else
            MapLocation = TESTMAPLOCATION2
        End If

        Dim MapLines As New List(Of String)
        Dim y As Integer = 0
        Using sr As New System.IO.StreamReader(MapLocation & "testmap3.txt")
            Dim line As String = sr.ReadLine
            Do While line IsNot Nothing
                Dim x As Integer = 0
                For Each s As String In line
                    Dim NewMapTile As New MapTile
                    NewMapTile.Location = New Point(x, y)
                    Select Case s
                        Case " "
                            NewMapTile.TileType = MapTile.MapTileType.Empty
                        Case "#"
                            NewMapTile.TileType = MapTile.MapTileType.Wall
                        Case Else
                            NewMapTile.TileType = MapTile.MapTileType.Empty
                    End Select
                    Map.Add(NewMapTile)
                    LevelSize.Width = x
                    x += 1
                Next
                LevelSize.Height = y
                y += 1
                line = sr.ReadLine
            Loop
        End Using
        Me.Dimensions.Add(LevelSize)
    End Sub

End Class
