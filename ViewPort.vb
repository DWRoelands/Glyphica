Public Class ViewPort

    Const TESTMAPLOCATION As String = "C:\Users\Duane\Documents\GitHub\Glyphica\Map Files\"

    Public Property ViewPortHeight As Integer
    Public Property ViewPortWidth As Integer
    Public Property Origin As New Coordinate  '' this represents the top-left coordinate of the map that is displayed in the viewport

    Public ReadOnly Property MapHeight As Integer
        Get
            Return Map.Count
        End Get
    End Property

    Public ReadOnly Property MapWidth As Integer
        Get
            Return Map(0).Length
        End Get
    End Property

    Private _HorizontalScrollBorder
    Public ReadOnly Property HorizontalScrollBorder As Integer
        Get
            Return ViewPortWidth / 5
        End Get
    End Property

    Private _VerticalScrollBorder As Integer
    Public ReadOnly Property VerticalScrollBorder As Integer
        Get
            Return ViewPortHeight / 5
        End Get
    End Property

    Private Map As List(Of String)

    Public Sub New(Height As Integer, Width As Integer)
        ViewPortHeight = Height
        ViewPortWidth = Width
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
        For x = 0 To ViewPortHeight - 1
            SolidBlockDraw(New Coordinate(ViewPortWidth, x))
        Next

        For y = 0 To ViewPortWidth - 1
            SolidBlockDraw(New Coordinate(y, ViewPortHeight - 1))
        Next

    End Sub

    Private Sub SolidBlockDraw(Position As Coordinate)
        Dim SOLIDBLOCK As Byte = 219
        Dim c As Char = System.Text.Encoding.GetEncoding(437).GetChars(New Byte() {SOLIDBLOCK})(0)
        Console.SetCursorPosition(Position.Left, Position.Top)
        Console.Write(c)
    End Sub

    Public Sub MapDraw()
        For ViewportRow = 0 To ViewPortHeight - 2
            Console.SetCursorPosition(0, ViewportRow)
            Console.Write(Map(ViewportRow + Origin.Top).Substring(Origin.Left, ViewPortWidth))
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

        If Player1.CurrentLocation.Left >= Origin.Left + ViewPortWidth - VerticalScrollBorder Then
            Debug.WriteLine("right scroll border hit")

            ' If we are too close to the right edge of the map to scroll fully, then scroll just enough
            Dim NewOriginLeft As Integer = Origin.Left + (ViewPortWidth / 2)

            If MapWidth - NewOriginLeft < ViewPortWidth Then
                Origin.Left = MapWidth - ViewPortWidth
            Else
                Origin.Left += (ViewPortWidth / 2)
            End If

            MapDraw()

        ElseIf Player1.CurrentLocation.Left <= Origin.Left + VerticalScrollBorder Then
            Debug.WriteLine("left scroll border hit")

            ' If we are too close to the left edge of the map to scroll fully, then scroll just enough
            Dim NewOriginLeft As Integer = Origin.Left - (ViewPortWidth / 2)

            If NewOriginLeft < 0 Then
                Origin.Left = 0
            Else
                Origin.Left -= (ViewPortWidth / 2)
            End If

            MapDraw()

        ElseIf Player1.CurrentLocation.Top >= Origin.Top + ViewPortHeight - 1 - HorizontalScrollBorder Then
            Debug.WriteLine("bottom scroll border hit")

            ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
            Dim NewOriginTop As Integer = Origin.Top + (ViewPortHeight / 2)
            If MapHeight - NewOriginTop < ViewPortHeight Then
                Origin.Top = MapHeight - ViewPortHeight + 1
            Else
                Origin.Top += (ViewPortHeight / 2)
            End If

            MapDraw()

        ElseIf Player1.CurrentLocation.Top <= Origin.Top + HorizontalScrollBorder Then
            Debug.WriteLine("top scroll border hit")

            ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
            Dim NewOriginTop As Integer = Origin.Top - (ViewPortHeight / 2)
            If NewOriginTop < 0 Then
                Origin.Top = 0
            Else
                Origin.Top -= (ViewPortHeight / 2)
            End If

            MapDraw()
        End If

        Console.SetCursorPosition(Player1.CurrentLocation.Left - Origin.Left, Player1.CurrentLocation.Top - Origin.Top)
        Console.Write("@")

    End Sub

    Public Sub Scroll()

    End Sub


End Class




