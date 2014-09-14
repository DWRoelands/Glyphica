Public Class ViewPort

    Const TESTMAPLOCATION As String = "C:\Users\Duane\Documents\GitHub\Glyphica\Map Files\"

    Public Enum MapTile
        Empty = 0
        Solid = 1
        StairsDown = 2
        StairsUp = 3
    End Enum

    ' level, x, y
    Private Map(,,) As MapTile

#Region "Properties"
    Public Property ViewPortHeight As Integer
    Public Property ViewPortWidth As Integer
    Public Property Origin As New Coordinate  '' this represents the top-left coordinate of the map that is displayed in the viewport
    Public Property MapLevel As Integer = 0
    Public ReadOnly Property MapHeight As Integer
        Get
            Return Map.GetUpperBound(1)
        End Get
    End Property

    Public ReadOnly Property MapWidth As Integer
        Get
            Return Map.GetUpperBound(2)
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
#End Region

    Public Sub MapLoad()

        ' this is temporary code which loads a text file map into the integer-based array which is the live map

        Dim MapLines As New List(Of String)
        Using sr As New System.IO.StreamReader(TESTMAPLOCATION & "testmap3.txt")
            Dim line As String = sr.ReadLine
            Do While line IsNot Nothing
                MapLines.Add(line)
                line = sr.ReadLine
            Loop
        End Using

        ReDim Map(0, MapLines.Count - 1, MapLines(0).Length - 1)

        Dim x As Integer = 0
        For Each MapLine As String In MapLines
            For y As Integer = 0 To MapLine.Length - 1
                Select Case MapLine.Substring(y, 1)
                    Case " "
                        Map(0, x, y) = MapTile.Empty
                    Case "#"
                        Map(0, x, y) = MapTile.Solid
                    Case ">"
                        Map(0, x, y) = MapTile.StairsDown
                    Case "<"
                        Map(0, x, y) = MapTile.StairsUp
                End Select
            Next
            x += 1
        Next
    End Sub

    Public Sub New(Height As Integer, Width As Integer)
        ViewPortHeight = Height
        ViewPortWidth = Width
    End Sub

    Public Sub OriginSet(NewOrigin As Coordinate)
        Origin = NewOrigin
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

        For x As Integer = 0 To ViewPortHeight - 2
            For y As Integer = 0 To ViewPortWidth - 1
                Dim MapChar As String = String.Empty
                Select Case Map(0, Origin.Top + x, Origin.Left + y)
                    Case MapTile.Empty
                        MapChar = " "
                    Case MapTile.Solid
                        MapChar = "#"
                    Case MapTile.StairsDown
                        MapChar = ">"
                    Case MapTile.StairsUp
                        MapChar = "<"
                End Select
                Console.SetCursorPosition(y, x)
                Console.Write(MapChar)
            Next
        Next
    End Sub

    Public Function LocationGet(Location As Coordinate) As MapTile
        Return Map(0, Location.Top, Location.Left)
    End Function

    Public Sub LocationClear(Location As Coordinate)
        Console.SetCursorPosition(Location.Left - Origin.Left, Location.Top - Origin.Top)
        Console.Write(MapCharGet(LocationGet(New Coordinate(Location.Left - Origin.Left, Location.Top - Origin.Top))))
    End Sub

    Public Sub PlayerDraw(Player1 As Player)

        If Player1.CurrentLocation.Left >= Origin.Left + ViewPortWidth - VerticalScrollBorder Then
            Debug.WriteLine("right scroll border hit")

            ' If we are too close to the right edge of the map to scroll fully, then scroll just enough
            Dim NewOriginLeft As Integer = Origin.Left + (ViewPortWidth / 2)

            If MapWidth - NewOriginLeft < ViewPortWidth Then
                Origin.Left = MapWidth - ViewPortWidth + 1
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
        Debug.WriteLine(String.Format("LEFT:{0} TOP:{1}", Player1.CurrentLocation.Left, Player1.CurrentLocation.Top))

    End Sub

    Public Function StairsUpFind() As Coordinate
        Dim ReturnValue As Coordinate = Nothing

        For x As Integer = 0 To Map.GetUpperBound(1)
            For y As Integer = 0 To Map.GetUpperBound(2)
                If Map(0, x, y) = MapTile.StairsUp Then
                    ReturnValue = New Coordinate(y, x)
                End If
            Next
        Next

        Return ReturnValue
    End Function

    Public Function StairsDownFind() As Coordinate
        Dim ReturnValue As Coordinate = Nothing

        For x As Integer = 0 To Map.GetUpperBound(1)
            For y As Integer = 0 To Map.GetUpperBound(2)
                If Map(0, x, y) = MapTile.StairsDown Then
                    ReturnValue = New Coordinate(y, x)
                End If
            Next
        Next

        Return ReturnValue
    End Function

    Private Function MapCharGet(MapValue As MapTile) As String
        Dim ReturnValue As String = String.Empty
        Select Case MapValue
            Case MapTile.Empty
                ReturnValue = " "
            Case MapTile.Solid
                ReturnValue = "#"
            Case MapTile.StairsDown
                ReturnValue = ">"
            Case MapTile.StairsUp
                ReturnValue = "<"
        End Select
        Return ReturnValue
    End Function
End Class




