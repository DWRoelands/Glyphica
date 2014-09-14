Imports System.Drawing
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
    Public Property Origin As New Point  '' this represents the top-left coordinate of the map that is displayed in the viewport
    Public Property MapLevel As Integer = 0
    Public Property MapSize As Size
    Public Property ViewPortSize As Size

    Public ReadOnly Property HorizontalScrollBorder As Integer
        Get
            Return ViewPortSize.Width / 5
        End Get
    End Property

    Public ReadOnly Property VerticalScrollBorder As Integer
        Get
            Return ViewPortSize.Height / 5
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

        ReDim Map(0, MapLines(0).Length - 1, MapLines.Count - 1)
        MapSize = New Size(MapLines(0).Length - 1, MapLines.Count - 1)

        Dim y As Integer = 0
        For Each MapLine As String In MapLines
            For x As Integer = 0 To MapLine.Length - 1
                Select Case MapLine.Substring(x, 1)
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
            y += 1
        Next

    End Sub

    Public Sub New(Width As Integer, Height As Integer)
        ViewPortSize = New Size(Width, Height)
    End Sub

    Public Sub OriginSet(NewOrigin As Point)
        Origin = NewOrigin
    End Sub

    Public Sub BorderDraw()
        For y = 0 To ViewPortSize.Height - 1
            SolidBlockDraw(New Point(ViewPortSize.Width, y))
        Next

        For x = 0 To ViewPortSize.Width - 1
            SolidBlockDraw(New Point(x, ViewPortSize.Height - 1))
        Next

    End Sub

    Private Sub SolidBlockDraw(Position As Point)
        Dim SOLIDBLOCK As Byte = 219
        Dim c As Char = System.Text.Encoding.GetEncoding(437).GetChars(New Byte() {SOLIDBLOCK})(0)
        Console.SetCursorPosition(Position.X, Position.Y)
        Console.Write(c)
    End Sub

    Public Sub MapDraw()
        For x As Integer = 0 To ViewPortSize.Width - 1
            For y As Integer = 0 To ViewPortSize.Height - 2
                Dim MapChar As String = String.Empty
                Select Case Map(0, Origin.X + x, Origin.Y + y)
                    Case MapTile.Empty
                        MapChar = " "
                    Case MapTile.Solid
                        MapChar = "#"
                    Case MapTile.StairsDown
                        MapChar = ">"
                    Case MapTile.StairsUp
                        MapChar = "<"
                End Select
                Console.SetCursorPosition(x, y)
                Console.Write(MapChar)
            Next
        Next
    End Sub

    Public Function LocationGet(Location As Point) As MapTile
        Return Map(0, Location.X, Location.Y)
    End Function

    Public Sub LocationClear(Location As Point)
        Console.SetCursorPosition(Location.X - Origin.X, Location.Y - Origin.Y)
        Console.Write(MapCharGet(LocationGet(New Point(Location.X - Origin.X, Location.Y - Origin.Y))))
    End Sub

    Public Sub OriginSetX(x As Integer)
        Origin = New Point(x, Origin.Y)
    End Sub

    Public Sub OriginSetY(y As Integer)
        Origin = New Point(Origin.X, y)
    End Sub


    Public Sub PlayerDraw(Player1 As Player)

        If Player1.CurrentLocation.X >= Origin.X + ViewPortSize.Width - VerticalScrollBorder Then
            Debug.WriteLine("right scroll border hit")

            ' If we are too close to the right edge of the map to scroll fully, then scroll just enough
            Dim NewOriginLeft As Integer = Origin.X + (ViewPortSize.Width / 2)

            If MapSize.Width - NewOriginLeft < ViewPortSize.Width Then
                OriginSetX(MapSize.Width - (ViewPortSize.Width) + 1)
            Else
                OriginSetX(Origin.X + (ViewPortSize.Width / 2))
            End If

            MapDraw()

        ElseIf Player1.CurrentLocation.X <= Origin.X + VerticalScrollBorder Then
            Debug.WriteLine("left scroll border hit")

            ' If we are too close to the left edge of the map to scroll fully, then scroll just enough
            Dim NewOriginLeft As Integer = Origin.X - (ViewPortSize.Width / 2)

            If NewOriginLeft < 0 Then
                OriginSetX(0)
            Else
                OriginSetX(Origin.X - (ViewPortSize.Width / 2))
            End If

            MapDraw()

        ElseIf Player1.CurrentLocation.Y >= Origin.Y + ViewPortSize.Height - 1 - HorizontalScrollBorder Then
            Debug.WriteLine("bottom scroll border hit")

            ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
            Dim NewOriginTop As Integer = Origin.Y + (ViewPortSize.Height / 2)

            If MapSize.Height - NewOriginTop < ViewPortSize.Height Then
                OriginSetY(MapSize.Height - ViewPortSize.Height + 1)
            Else
                OriginSetY(Origin.Y + (ViewPortSize.Height / 2))
            End If

            MapDraw()

        ElseIf Player1.CurrentLocation.Y <= Origin.Y + HorizontalScrollBorder Then
            Debug.WriteLine("top scroll border hit")

            ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
            Dim NewOriginTop As Integer = Origin.Y - (ViewPortSize.Height / 2)
            If NewOriginTop < 0 Then
                OriginSetY(0)
            Else
                OriginSetY(Origin.Y - (ViewPortSize.Height / 2))
            End If

            MapDraw()
        End If

        Console.SetCursorPosition(Player1.CurrentLocation.X - Origin.X, Player1.CurrentLocation.Y - Origin.Y)
        Console.Write("@")
        Debug.WriteLine(String.Format("LEFT:{0} TOP:{1}", Player1.CurrentLocation.X, Player1.CurrentLocation.Y))

    End Sub

    Public Function StairsUpFind() As Point
        Dim ReturnValue As Point = Nothing

        For x As Integer = 0 To Map.GetUpperBound(1)
            For y As Integer = 0 To Map.GetUpperBound(2)
                If Map(0, x, y) = MapTile.StairsUp Then
                    ReturnValue = New Point(y, x)
                End If
            Next
        Next

        Return ReturnValue
    End Function

    Public Function StairsDownFind() As Point
        Dim ReturnValue As Point = Nothing

        For x As Integer = 0 To Map.GetUpperBound(1)
            For y As Integer = 0 To Map.GetUpperBound(2)
                If Map(0, x, y) = MapTile.StairsDown Then
                    ReturnValue = New Point(y, x)
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




