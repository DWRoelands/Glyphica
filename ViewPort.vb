Public Class ViewPort

    Const TESTMAPLOCATION As String = "C:\Users\droelands\Documents\GitHub\Glyphica\Map Files\"

    Public Property Height As Integer
    Public Property Width As Integer
    Public Property Origin As New Coordinate  '' this represents the top-left coordinate of the map that is displayed in the viewport

    Private Map As List(Of String)

    Public Sub New(ViewPortHeight As Integer, ViewPortWidth As Integer)
        Height = ViewPortHeight
        Width = ViewPortWidth
    End Sub

    Public Sub OriginSet(NewOrigin As Coordinate)
        Origin = NewOrigin
    End Sub

    Public Sub MapLoad()
        Map = New List(Of String)
        Using sr As New System.IO.StreamReader(TESTMAPLOCATION & "testmap1.txt")
            Dim Line As String = sr.ReadLine
            Do While Line IsNot Nothing
                Map.Add(Line)
                Line = sr.ReadLine()
            Loop
        End Using
    End Sub

    Public Sub BorderDraw()
        For x = 0 To Height - 1
            SolidBlockDraw(New Coordinate(Width, x))
        Next

        For y = 0 To Width - 1
            SolidBlockDraw(New Coordinate(y, Height - 1))
        Next

    End Sub

    Private Sub SolidBlockDraw(Position As Coordinate)
        Dim SOLIDBLOCK As Byte = 219
        Dim c As Char = System.Text.Encoding.GetEncoding(437).GetChars(New Byte() {SOLIDBLOCK})(0)
        Console.SetCursorPosition(Position.Left, Position.Top)
        Console.Write(c)
    End Sub

    Public Sub MapDraw()
        For ViewportRow = 0 To Height - 2
            Console.SetCursorPosition(0, ViewportRow)
            Console.Write(Map(ViewportRow).Substring(Origin.Left, Width))
        Next
    End Sub

    Public Function MapTileGet(Location As Coordinate) As String
        Return Map(Location.Top).Substring(Location.Left, 1)
    End Function

    Public Function PlayerMoveProcess(Player1 As Player) As Player.PlayerMoveResult
        Dim ReturnValue As Player.PlayerMoveResult
        Dim TargetContents As String = MapTileGet(Player1.TargetPosition)
        Select Case TargetContents
            Case " "
                ReturnValue = Player.PlayerMoveResult.Move
            Case "#"
                ReturnValue = Player.PlayerMoveResult.Blocked
        End Select
        Return ReturnValue
    End Function
End Class
