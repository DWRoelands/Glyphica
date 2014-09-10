Public Class ViewPort

    Const TESTMAPLOCATION As String = "C:\Users\Duane\Documents\GitHub\Glyphica\Map Files\"

    Public Property Height As Integer
    Public Property Width As Integer
    Public Property Origin As New Coordinate  '' this represents the top-left coordinate of the map that is displayed in the viewport

    Private _HorizontalScrollBorder
    Public ReadOnly Property HorizontalScrollBorder As Integer
        Get
            Return Width / 5
        End Get
    End Property

    Private _VerticalScrollBorder As Integer
    Public ReadOnly Property VerticalScrollBorder As Integer
        Get
            Return Height / 5
        End Get
    End Property

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
            Console.Write(Map(ViewportRow + Origin.Top).Substring(Origin.Left, Width))
        Next
    End Sub

    Public Function LocationGet(Location As Coordinate) As String
        Return Map(Location.Top).Substring(Location.Left, 1)
    End Function

    Public Sub LocationClear(Location As Coordinate)
        Console.SetCursorPosition(Location.Left - Origin.Left, Location.Top - Origin.Top)
        Console.Write(" ")
    End Sub

    Public Sub PlayerDraw(Player1 As Player)

        If Player1.CurrentLocation.Left >= Origin.Left + Width - VerticalScrollBorder Then
            Debug.WriteLine("right scroll border hit")
            Origin.Left += (Width / 2)
            MapDraw()
        ElseIf Player1.CurrentLocation.Left <= Origin.Left + VerticalScrollBorder Then
            Debug.WriteLine("left scroll border hit")
            Origin.Left -= (Width / 2)
            MapDraw()
        ElseIf Player1.CurrentLocation.Top >= Origin.Top + Height - 1 - HorizontalScrollBorder Then
            Debug.WriteLine("bottom scroll border hit")
            Origin.Top += (Height / 2)
            MapDraw()
        ElseIf Player1.CurrentLocation.Top <= Origin.Top + HorizontalScrollBorder Then
            Debug.WriteLine("top scroll border hit")
            Origin.Top -= (Height / 2)
            MapDraw()
        End If

        Console.SetCursorPosition(Player1.CurrentLocation.Left - Origin.Left, Player1.CurrentLocation.Top - Origin.Top)
        Console.Write("@")

    End Sub

    Public Sub Scroll()

    End Sub


End Class




