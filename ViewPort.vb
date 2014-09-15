Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
Public Class ViewPort

    Const TESTMAPLOCATION As String = "C:\Users\duane\Documents\GitHub\Glyphica\Map Files\"

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

    Public Sub New(Width As Integer, Height As Integer)
        ViewPortSize = New Size(Width, Height)
    End Sub

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
                        Map(0, x, y) = New MapTile(MapTile.MapTileType.Empty, New Point(x, y))
                    Case "#"
                        Map(0, x, y) = New MapTile(MapTile.MapTileType.Wall, New Point(x, y))
                    Case ">"
                        Map(0, x, y) = New MapTile(MapTile.MapTileType.StairsDown, New Point(x, y))
                    Case "<"
                        Map(0, x, y) = New MapTile(MapTile.MapTileType.StairsUp, New Point(x, y))
                End Select
            Next
            y += 1
        Next

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
        Debug.WriteLine("MapDraw()")

        For x As Integer = Origin.X To Origin.X + ViewPortSize.Width - 1
            For y As Integer = Origin.Y To Origin.Y + ViewPortSize.Height - 2
                Console.SetCursorPosition(x - Origin.X, y - Origin.Y)
                Console.Write(" ")
            Next
        Next

        For x As Integer = Origin.X To Origin.X + ViewPortSize.Width - 1
            For y As Integer = Origin.Y To Origin.Y + ViewPortSize.Height - 2
                If Map(0, x, y).IsVisible Then
                    Console.SetCursorPosition(x - Origin.X, y - Origin.Y)
                    Console.Write(Map(0, x, y).DisplayCharacter)
                End If
            Next
        Next
        Debug.WriteLine("7,9:" & Map(0, 7, 9).IsVisible)
    End Sub

    Public Function LocationGet(Location As Point) As MapTile
        Return Map(0, Location.X, Location.Y)
    End Function

    Public Sub LocationClear(Location As Point)
        Console.SetCursorPosition(Location.X - Origin.X, Location.Y - Origin.Y)
        If Map(0, Location.X - Origin.X, Location.Y - Origin.Y).IsVisible Then
            Console.Write(Map(0, Location.X - Origin.X, Location.Y - Origin.Y).DisplayCharacter)
        Else
            Console.Write(" ")
        End If
    End Sub

    Public Sub OriginSetX(x As Integer)
        Origin = New Point(x, Origin.Y)
    End Sub

    Public Sub OriginSetY(y As Integer)
        Origin = New Point(Origin.X, y)
    End Sub


    Public Sub PlayerDraw(Player1 As Player)
        Debug.WriteLine(String.Format("PlayerDraw:{0},{1}", Player1.Location.x, Player1.Location.y))
        If Player1.Location.X >= Origin.X + ViewPortSize.Width - VerticalScrollBorder Then
            Debug.WriteLine("right scroll border hit")

            If Origin.X < Map.GetLength(1) - ViewPortSize.Width Then
                ' If we are too close to the right edge of the map to scroll fully, then scroll just enough
                Dim NewOriginLeft As Integer = Origin.X + (ViewPortSize.Width / 2)

                If MapSize.Width - NewOriginLeft < ViewPortSize.Width Then
                    OriginSetX(MapSize.Width - (ViewPortSize.Width) + 1)
                Else
                    OriginSetX(Origin.X + (ViewPortSize.Width / 2))
                End If

                MapDraw()
            End If

        ElseIf Player1.Location.X <= Origin.X + VerticalScrollBorder Then
            Debug.WriteLine("left scroll border hit")

            If Origin.X > 0 Then
                ' If we are too close to the left edge of the map to scroll fully, then scroll just enough
                Dim NewOriginLeft As Integer = Origin.X - (ViewPortSize.Width / 2)

                If NewOriginLeft < 0 Then
                    OriginSetX(0)
                Else
                    OriginSetX(Origin.X - (ViewPortSize.Width / 2))
                End If

                MapDraw()
            End If

        ElseIf Player1.Location.Y >= Origin.Y + ViewPortSize.Height - 1 - HorizontalScrollBorder Then
            Debug.WriteLine("bottom scroll border hit")

            If Origin.Y < Map.GetLength(2) - ViewPortSize.Height Then
                ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
                Dim NewOriginTop As Integer = Origin.Y + (ViewPortSize.Height / 2)

                If MapSize.Height - NewOriginTop < ViewPortSize.Height Then
                    OriginSetY(MapSize.Height - ViewPortSize.Height + 1)
                Else
                    OriginSetY(Origin.Y + (ViewPortSize.Height / 2))
                End If

                MapDraw()
            End If

        ElseIf Player1.Location.Y <= Origin.Y + HorizontalScrollBorder Then
            Debug.WriteLine("top scroll border hit")

            If Origin.Y > 0 Then
                ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
                Dim NewOriginTop As Integer = Origin.Y - (ViewPortSize.Height / 2)
                If NewOriginTop < 0 Then
                    OriginSetY(0)
                Else
                    OriginSetY(Origin.Y - (ViewPortSize.Height / 2))
                End If
                MapDraw()
            End If

        End If

        For Each p As Point In GetVisibleCells(Player1.Location, 5)
            Console.SetCursorPosition(p.X - Origin.X, p.Y - Origin.Y)
            Console.Write(Map(0, p.X, p.Y).DisplayCharacter)
            Map(0, p.X, p.Y).IsVisible = True
        Next

        Console.SetCursorPosition(Player1.Location.X - Origin.X, Player1.Location.Y - Origin.Y)

        Dim c As ConsoleColor = Console.ForegroundColor
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("@")
        Console.ForegroundColor = c
        Debug.WriteLine("7,9:" & Map(0, 7, 9).IsVisible)
    End Sub

    Public Function StairsUpFind() As Point
        Dim ReturnValue As Point = Nothing

        For x As Integer = 0 To Map.GetUpperBound(1)
            For y As Integer = 0 To Map.GetUpperBound(2)
                If Map(0, x, y).TileType = MapTile.MapTileType.StairsUp Then
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
                If Map(0, x, y).TileType = MapTile.MapTileType.StairsDown Then
                    ReturnValue = New Point(y, x)
                End If
            Next
        Next

        Return ReturnValue
    End Function




    Public Function PlayerMoveAttempt(Player1 As Player, Target As Point) As Player.PlayerMoveResult
        Return Player1.PlayerMoveAttempt(Map(0, Target.X, Target.Y))
    End Function

#Region "FOV Recurse code"

    ' The code in this region was graciously released to the public domain by Andy Stobirski
    ' His original C# class is on GitHub at https://github.com/AndyStobirski/RogueLike/blob/master/FOVRecurse.cs
    ' I converted it to VB.NET for this project.
    ' I would not have been able to do this myself.  Thank you, Andy.

    Private VisibleOctants As New List(Of Integer)() From {1, 2, 3, 4, 5, 6, 7, 8}
    Private VisiblePoints As List(Of Point)

    ' Confirm that a point is within the bounds of the map array
    Private Function Point_Valid(pX As Integer, pY As Integer) As Boolean
        Return pX >= 0 And pX < Map.GetLength(1) - 1 And pY >= 0 And pY < Map.GetLength(2)
    End Function

    Private Function GetVisibleCells(Location As Point, Range As Integer) As List(Of Point)
        VisiblePoints = New List(Of Point)()
        For Each o As Integer In VisibleOctants
            ScanOctant(1, o, 1.0, 0.0, Location, Range)
        Next
        Return VisiblePoints
    End Function

    Protected Sub ScanOctant(pDepth As Integer, pOctant As Integer, pStartSlope As Double, pEndSlope As Double, Location As Point, Range As Integer)

        Dim visrange2 As Integer = Range * Range
        Dim x As Integer = 0
        Dim y As Integer = 0

        Select Case pOctant

            Case 1
                'nnw
                y = Location.Y - pDepth
                If y < 0 Then
                    Return
                End If

                x = Location.X - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If x < 0 Then
                    x = 0
                End If

                While GetSlope(x, y, Location.X, Location.Y, False) >= pEndSlope
                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then
                        If Map(0, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            'current cell blocked
                            If x - 1 >= 0 AndAlso Map(0, x - 1, y).BlocksVision = False Then
                                'prior cell within range AND open...
                                '...incremenet the depth, adjust the endslope and recurse
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, Location.X, Location.Y, False), Location, Range)
                            End If
                        Else

                            If x - 1 >= 0 AndAlso Map(0, x - 1, y).BlocksVision Then
                                'prior cell within range AND open...
                                '..adjust the startslope
                                pStartSlope = GetSlope(x - 0.5, y - 0.5, Location.X, Location.Y, False)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    x += 1
                End While
                x -= 1
                Exit Select

            Case 2
                'nne
                y = Location.Y - pDepth
                If y < 0 Then
                    Return
                End If

                x = Location.X + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If x >= Map.GetLength(1) Then
                    x = Map.GetLength(1) - 1
                End If

                While GetSlope(x, y, Location.X, Location.Y, False) <= pEndSlope
                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then
                        If Map(0, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If x + 1 < Map.GetLength(1) AndAlso Map(0, x + 1, y).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, Location.X, Location.Y, False), Location, Range)
                            End If
                        Else
                            If x + 1 < Map.GetLength(1) AndAlso Map(0, x + 1, y).BlocksVision Then
                                pStartSlope = -GetSlope(x + 0.5, y - 0.5, Location.X, Location.Y, False)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    x -= 1
                End While
                x += 1
                Exit Select

            Case 3

                x = Location.X + pDepth
                If x >= Map.GetLength(1) Then
                    Return
                End If

                y = Location.Y - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If y < 0 Then
                    y = 0
                End If

                While GetSlope(x, y, Location.X, Location.Y, True) <= pEndSlope

                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then

                        If Map(0, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If y - 1 >= 0 AndAlso Map(0, x, y - 1).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, Location.X, Location.Y, True), Location, Range)
                            End If
                        Else
                            If y - 1 >= 0 AndAlso Map(0, x, y - 1).BlocksVision Then
                                pStartSlope = -GetSlope(x + 0.5, y - 0.5, Location.X, Location.Y, True)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    y += 1
                End While
                y -= 1
                Exit Select

            Case 4

                x = Location.X + pDepth
                If x >= Map.GetLength(1) Then
                    Return
                End If

                y = Location.Y + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If y >= Map.GetLength(2) Then
                    y = Map.GetLength(2) - 1
                End If

                While GetSlope(x, y, Location.X, Location.Y, True) >= pEndSlope

                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then

                        If Map(0, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If y + 1 < Map.GetLength(2) AndAlso Map(0, x, y + 1).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, Location.X, Location.Y, True), Location, Range)
                            End If
                        Else
                            If y + 1 < Map.GetLength(2) AndAlso Map(0, x, y + 1).BlocksVision Then
                                pStartSlope = GetSlope(x + 0.5, y + 0.5, Location.X, Location.Y, True)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    y -= 1
                End While
                y += 1
                Exit Select

            Case 5

                y = Location.Y + pDepth
                If y >= Map.GetLength(2) Then
                    Return
                End If

                x = Location.X + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If x >= Map.GetLength(1) Then
                    x = Map.GetLength(1) - 1
                End If

                While GetSlope(x, y, Location.X, Location.Y, False) >= pEndSlope
                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then

                        If Map(0, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If x + 1 < Map.GetLength(1) AndAlso Map(0, x + 1, y).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, Location.X, Location.Y, False), Location, Range)
                            End If
                        Else
                            If x + 1 < Map.GetLength(1) AndAlso Map(0, x + 1, y).BlocksVision Then
                                pStartSlope = GetSlope(x + 0.5, y + 0.5, Location.X, Location.Y, False)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    x -= 1
                End While
                x += 1
                Exit Select

            Case 6

                y = Location.Y + pDepth
                If y >= Map.GetLength(2) Then
                    Return
                End If

                x = Location.X - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If x < 0 Then
                    x = 0
                End If

                While GetSlope(x, y, Location.X, Location.Y, False) <= pEndSlope
                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then

                        If Map(0, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If x - 1 >= 0 AndAlso Map(0, x - 1, y).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, Location.X, Location.Y, False), Location, Range)
                            End If
                        Else
                            If x - 1 >= 0 AndAlso Map(0, x - 1, y).BlocksVision Then
                                pStartSlope = -GetSlope(x - 0.5, y + 0.5, Location.X, Location.Y, False)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    x += 1
                End While
                x -= 1
                Exit Select

            Case 7

                x = Location.X - pDepth
                If x < 0 Then
                    Return
                End If

                y = Location.Y + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If y >= Map.GetLength(2) Then
                    y = Map.GetLength(2) - 1
                End If

                While GetSlope(x, y, Location.X, Location.Y, True) <= pEndSlope

                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then

                        If Map(0, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If y + 1 < Map.GetLength(2) AndAlso Map(0, x, y + 1).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, Location.X, Location.Y, True), Location, Range)
                            End If
                        Else
                            If y + 1 < Map.GetLength(2) AndAlso Map(0, x, y + 1).BlocksVision Then
                                pStartSlope = -GetSlope(x - 0.5, y + 0.5, Location.X, Location.Y, True)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    y -= 1
                End While
                y += 1
                Exit Select

            Case 8
                'wnw
                x = Location.X - pDepth
                If x < 0 Then
                    Return
                End If

                y = Location.Y - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If y < 0 Then
                    y = 0
                End If

                While GetSlope(x, y, Location.X, Location.Y, True) >= pEndSlope

                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then

                        If Map(0, x, y).BlocksVision Then
                            If y - 1 >= 0 AndAlso Map(0, x, y - 1).BlocksVision = False Then
                                VisiblePoints.Add(New Point(x, y))   ' testing
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, Location.X, Location.Y, True), Location, Range)

                            End If
                        Else
                            If y - 1 >= 0 AndAlso Map(0, x, y - 1).BlocksVision Then
                                pStartSlope = GetSlope(x - 0.5, y - 0.5, Location.X, Location.Y, True)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    y += 1
                End While
                y -= 1
                Exit Select
        End Select


        If x < 0 Then
            x = 0
        ElseIf x >= Map.GetLength(1) Then
            x = Map.GetLength(0) - 1
        End If

        If y < 0 Then
            y = 0
        ElseIf y >= Map.GetLength(2) Then
            y = Map.GetLength(2) - 1
        End If

        If pDepth < Range And Map(0, x, y).BlocksVision = False Then
            ScanOctant(pDepth + 1, pOctant, pStartSlope, pEndSlope, Location, Range)
        End If

    End Sub

    Private Function GetSlope(pX1 As Double, pY1 As Double, pX2 As Double, pY2 As Double, pInvert As Boolean) As Double
        If pInvert Then
            Return (pY1 - pY2) / (pX1 - pX2)
        Else
            Return (pX1 - pX2) / (pY1 - pY2)
        End If
    End Function

    Private Function GetVisDistance(pX1 As Integer, pY1 As Integer, pX2 As Integer, pY2 As Integer) As Integer
        Return ((pX1 - pX2) * (pX1 - pX2)) + ((pY1 - pY2) * (pY1 - pY2))
    End Function

#End Region

End Class




