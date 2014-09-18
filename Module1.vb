Imports System.Drawing
Module Module1

    Const TESTMAPLOCATION As String = "C:\Users\Duane\Documents\GitHub\Glyphica\Map Files\"
    Const SOLIDBLOCK As Byte = 219

    Dim Map(,,) As MapTile      ' level, x, y
    Dim MapLevel As Integer

    Dim ViewportSize As Size
    Dim ViewportOrigin As Point

    Private VisibleOctants As New List(Of Integer)() From {1, 2, 3, 4, 5, 6, 7, 8}
    Private VisiblePoints As List(Of Point)

    Dim Player1 As Player

    Public Sub Main()
        Console.CursorVisible = False
        Console.WindowWidth = 80
        Console.WindowHeight = 40
        Console.SetBufferSize(80, 40)

        Player1 = New Player

        ViewportSize = New Size(30, 30)
        ViewportOrigin = New Point(0, 0)     ' The upper-left coordinate of the rectangular section of the map displayed in the viewport
        ViewportBorderDraw()

        MapLoad()

        Player1.Location = New Point(13, 13)
        ViewportPlayerDraw()

        Dim KeyPress As ConsoleKeyInfo
        Do
            Dim ToLocation As New Point
            KeyPress = Console.ReadKey(True)

            Select Case KeyPress.Key
                Case ConsoleKey.DownArrow, ConsoleKey.NumPad2
                    ToLocation = New Point(Player1.Location.X, Player1.Location.Y + 1)

                Case ConsoleKey.UpArrow, ConsoleKey.NumPad8
                    ToLocation = New Point(Player1.Location.X, Player1.Location.Y - 1)

                Case ConsoleKey.LeftArrow, ConsoleKey.NumPad4
                    ToLocation = New Point(Player1.Location.X - 1, Player1.Location.Y)

                Case ConsoleKey.RightArrow, ConsoleKey.NumPad6
                    ToLocation = New Point(Player1.Location.X + 1, Player1.Location.Y)

                Case ConsoleKey.NumPad7
                    ToLocation = New Point(Player1.Location.X - 1, Player1.Location.Y - 1)

                Case ConsoleKey.NumPad9
                    ToLocation = New Point(Player1.Location.X + 1, Player1.Location.Y - 1)

                Case ConsoleKey.NumPad1
                    ToLocation = New Point(Player1.Location.X - 1, Player1.Location.Y + 1)

                Case ConsoleKey.NumPad3
                    ToLocation = New Point(Player1.Location.X + 1, Player1.Location.Y + 1)
            End Select

            Select Case KeyPress.KeyChar
                Case ">"c  '' go down stairs
                    If MapTileGet(Player1.Location).TileType = MapTile.MapTileType.StairsDown Then
                        MapLevel += 1
                        ViewportMapDraw()
                        Player1.Location = StairsUpFind()
                    End If

                Case "<"c  '' go up stairs
                    If MapTileGet(Player1.Location).TileType = MapTile.MapTileType.StairsUp Then
                        MapLevel -= 1
                        ViewportMapDraw()
                        Player1.Location = StairsDownFind()
                    End If

            End Select

            Select Case PlayerMoveAttempt(ToLocation)
                Case Player.PlayerMoveResult.Move
                    Player1.Location = ToLocation
                Case Player.PlayerMoveResult.Blocked
                    '' do nothing - couldn't move the player
                    '' later, we'll display some sort of mesage based on what's in the ToLocation
            End Select

            ViewportPlayerDraw()

        Loop While KeyPress.Key <> ConsoleKey.X

    End Sub

    Public Function PlayerMoveAttempt(ToLocation As Point) As Player.PlayerMoveResult
        Dim ReturnValue As Player.PlayerMoveResult
        Select Case MapTileGet(ToLocation).TileType
            Case MapTile.MapTileType.Empty
                ReturnValue = Player.PlayerMoveResult.Move
            Case MapTile.MapTileType.Wall
                ReturnValue = Player.PlayerMoveResult.Blocked
        End Select
        Return ReturnValue
    End Function

    Public Sub ViewportMapDraw()
        Debug.WriteLine("ViewportMapDraw()")

        For x As Integer = ViewportOrigin.X To ViewportOrigin.X + ViewportSize.Width - 1
            For y As Integer = ViewportOrigin.Y To ViewportOrigin.Y + ViewportSize.Height - 2
                Console.SetCursorPosition(x - ViewportOrigin.X, y - ViewportOrigin.Y)
                Console.Write(" ")
            Next
        Next

        For x As Integer = ViewportOrigin.X To ViewportOrigin.X + ViewportSize.Width - 1
            For y As Integer = ViewportOrigin.Y To ViewportOrigin.Y + ViewportSize.Height - 2
                If Map(MapLevel, x, y).IsVisible Then
                    Console.SetCursorPosition(x - ViewportOrigin.X, y - ViewportOrigin.Y)
                    Console.Write(Map(MapLevel, x, y).DisplayCharacter)
                End If
            Next
        Next
    End Sub

    Private Function GetVisibleCells(Location As Point, Range As Integer) As List(Of Point)
        VisiblePoints = New List(Of Point)()
        For Each o As Integer In VisibleOctants
            ScanOctant(1, o, 1.0, 0.0, Location, Range)
        Next
        Return VisiblePoints
    End Function

    Private Sub ScanOctant(pDepth As Integer, pOctant As Integer, pStartSlope As Double, pEndSlope As Double, Location As Point, Range As Integer)

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
                        If Map(MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            'current cell blocked
                            If x - 1 >= 0 AndAlso Map(MapLevel, x - 1, y).BlocksVision = False Then
                                'prior cell within range AND open...
                                '...incremenet the depth, adjust the endslope and recurse
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, Location.X, Location.Y, False), Location, Range)
                            End If
                        Else

                            If x - 1 >= 0 AndAlso Map(MapLevel, x - 1, y).BlocksVision Then
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
                        If Map(MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If x + 1 < Map.GetLength(1) AndAlso Map(MapLevel, x + 1, y).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, Location.X, Location.Y, False), Location, Range)
                            End If
                        Else
                            If x + 1 < Map.GetLength(1) AndAlso Map(MapLevel, x + 1, y).BlocksVision Then
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

                        If Map(MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If y - 1 >= 0 AndAlso Map(MapLevel, x, y - 1).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, Location.X, Location.Y, True), Location, Range)
                            End If
                        Else
                            If y - 1 >= 0 AndAlso Map(MapLevel, x, y - 1).BlocksVision Then
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

                        If Map(MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If y + 1 < Map.GetLength(2) AndAlso Map(MapLevel, x, y + 1).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, Location.X, Location.Y, True), Location, Range)
                            End If
                        Else
                            If y + 1 < Map.GetLength(2) AndAlso Map(MapLevel, x, y + 1).BlocksVision Then
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

                        If Map(MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If x + 1 < Map.GetLength(1) AndAlso Map(MapLevel, x + 1, y).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, Location.X, Location.Y, False), Location, Range)
                            End If
                        Else
                            If x + 1 < Map.GetLength(1) AndAlso Map(MapLevel, x + 1, y).BlocksVision Then
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

                        If Map(MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If x - 1 >= 0 AndAlso Map(MapLevel, x - 1, y).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, Location.X, Location.Y, False), Location, Range)
                            End If
                        Else
                            If x - 1 >= 0 AndAlso Map(MapLevel, x - 1, y).BlocksVision Then
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

                        If Map(MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If y + 1 < Map.GetLength(2) AndAlso Map(MapLevel, x, y + 1).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, Location.X, Location.Y, True), Location, Range)
                            End If
                        Else
                            If y + 1 < Map.GetLength(2) AndAlso Map(MapLevel, x, y + 1).BlocksVision Then
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

                        If Map(MapLevel, x, y).BlocksVision Then
                            If y - 1 >= 0 AndAlso Map(MapLevel, x, y - 1).BlocksVision = False Then
                                VisiblePoints.Add(New Point(x, y))   ' testing
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, Location.X, Location.Y, True), Location, Range)

                            End If
                        Else
                            If y - 1 >= 0 AndAlso Map(MapLevel, x, y - 1).BlocksVision Then
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
            x = Map.GetLength(1) - 1
        End If

        If y < 0 Then
            y = 0
        ElseIf y >= Map.GetLength(2) Then
            y = Map.GetLength(2) - 1
        End If

        If pDepth < Range And Map(MapLevel, x, y).BlocksVision = False Then
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

        ReDim Map(MapLevel, MapLines(0).Length - 1, MapLines.Count - 1)

        Dim y As Integer = 0
        For Each MapLine As String In MapLines
            For x As Integer = 0 To MapLine.Length - 1
                Debug.WriteLine(x & "," & y)
                Select Case MapLine.Substring(x, 1)
                    Case " "
                        Map(MapLevel, x, y) = New MapTile(MapTile.MapTileType.Empty, New Point(x, y))
                    Case "#"
                        Map(MapLevel, x, y) = New MapTile(MapTile.MapTileType.Wall, New Point(x, y))
                    Case ">"
                        Map(MapLevel, x, y) = New MapTile(MapTile.MapTileType.StairsDown, New Point(x, y))
                    Case "<"
                        Map(MapLevel, x, y) = New MapTile(MapTile.MapTileType.StairsUp, New Point(x, y))
                End Select
            Next
            y += 1
        Next

    End Sub


    Public Sub ViewportPlayerDraw()
        Debug.WriteLine(String.Format("PlayerDraw:{0},{1}", Player1.Location.x, Player1.Location.y))
        If Player1.Location.X >= ViewportOrigin.X + ViewportSize.Width - ViewportXScrollBufferGet() Then
            Debug.WriteLine("right scroll border hit")

            If ViewportOrigin.X < Map.GetLength(1) - ViewportSize.Width Then
                ' If we are too close to the right edge of the map to scroll fully, then scroll just enough
                Dim NewOriginLeft As Integer = ViewportOrigin.X + (ViewportSize.Width / 2)

                If Map.GetLength(1) - NewOriginLeft < ViewportSize.Width Then
                    ViewportOriginXSet(Map.GetLength(1) - (ViewportSize.Width) + 1)
                Else
                    ViewportOriginXSet(ViewportOrigin.X + (ViewportSize.Width / 2))
                End If

                ViewportMapDraw()
            End If

        ElseIf Player1.Location.X <= ViewportOrigin.X + ViewportXScrollBufferGet() Then
            Debug.WriteLine("left scroll border hit")

            If ViewportOrigin.X > 0 Then
                ' If we are too close to the left edge of the map to scroll fully, then scroll just enough
                Dim NewOriginLeft As Integer = ViewportOrigin.X - (ViewportSize.Width / 2)

                If NewOriginLeft < 0 Then
                    ViewportOriginXSet(0)
                Else
                    ViewportOriginXSet(ViewportOrigin.X - (ViewportSize.Width / 2))
                End If

                ViewportMapDraw()
            End If

        ElseIf Player1.Location.Y >= ViewportOrigin.Y + ViewportSize.Height - 1 - ViewportYScrollBufferGet() Then
            Debug.WriteLine("bottom scroll border hit")

            If ViewportOrigin.Y < Map.GetLength(2) - ViewportSize.Height Then
                ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
                Dim NewOriginTop As Integer = ViewportOrigin.Y + (ViewportSize.Height / 2)
                If Map.GetLength(2) - NewOriginTop < ViewportSize.Height Then
                    ViewportOriginYSet(Map.GetLength(2) - ViewportSize.Height + 1)
                Else
                    ViewportOriginYSet(ViewportOrigin.Y + (ViewportSize.Height / 2))
                End If

                ViewportMapDraw()
            End If

            ElseIf Player1.Location.Y <= ViewportOrigin.Y + ViewportYScrollBufferGet() Then
                Debug.WriteLine("top scroll border hit")

                If ViewportOrigin.Y > 0 Then
                    ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
                    Dim NewOriginTop As Integer = ViewportOrigin.Y - (ViewportSize.Height / 2)
                    If NewOriginTop < 0 Then
                        ViewportOriginYSet(0)
                    Else
                        ViewportOriginYSet(ViewportOrigin.Y - (ViewportSize.Height / 2))
                    End If
                    ViewportMapDraw()
                End If

            End If

            For Each p As Point In GetVisibleCells(Player1.Location, 5)
                Console.SetCursorPosition(p.X - ViewportOrigin.X, p.Y - ViewportOrigin.Y)
                Console.Write(Map(MapLevel, p.X, p.Y).DisplayCharacter)
                Map(MapLevel, p.X, p.Y).IsVisible = True
            Next

            Console.SetCursorPosition(Player1.Location.X - ViewportOrigin.X, Player1.Location.Y - ViewportOrigin.Y)

            Dim c As ConsoleColor = Console.ForegroundColor
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("@")
            Console.ForegroundColor = c
            Debug.WriteLine("7,9:" & Map(0, 7, 9).IsVisible)
    End Sub

    Public Function ViewportXScrollBufferGet() As Integer
        Return ViewportSize.Width / 5
    End Function

    Public Function ViewportYScrollBufferGet() As Integer
        Return ViewportSize.Height / 5
    End Function

    Public Sub ViewportLocationClear(Location As Point)
        Console.SetCursorPosition(Location.X - ViewportOrigin.X, Location.Y - ViewportOrigin.Y)
        If Map(0, Location.X - ViewportOrigin.X, Location.Y - ViewportOrigin.Y).IsVisible Then
            Console.Write(Map(0, Location.X - ViewportOrigin.X, Location.Y - ViewportOrigin.Y).DisplayCharacter)
        Else
            Console.Write(" ")
        End If
    End Sub

    Public Sub ViewportOriginXSet(x As Integer)
        ViewportOrigin = New Point(x, ViewportOrigin.Y)
    End Sub

    Public Sub ViewportOriginYSet(y As Integer)
        ViewportOrigin = New Point(ViewportOrigin.X, y)
    End Sub

    Public Sub ViewportBorderDraw()
        For y = 0 To ViewportSize.Height - 1
            SolidBlockDraw(New Point(ViewportSize.Width, y))
        Next

        For x = 0 To ViewportSize.Width - 1
            SolidBlockDraw(New Point(x, ViewportSize.Height - 1))
        Next
    End Sub

    Private Sub SolidBlockDraw(Position As Point)
        Dim c As Char = System.Text.Encoding.GetEncoding(437).GetChars(New Byte() {SOLIDBLOCK})(0)
        Console.SetCursorPosition(Position.X, Position.Y)
        Console.Write(c)
    End Sub

    Public Function MapTileGet(Location As Point) As MapTile
        Return Map(MapLevel, Location.X, Location.Y)
    End Function

    Public Function StairsUpFind() As Point
        Dim ReturnValue As Point = Nothing

        For x As Integer = 0 To Map.GetUpperBound(1)
            For y As Integer = 0 To Map.GetUpperBound(2)
                If Map(MapLevel, x, y).TileType = MapTile.MapTileType.StairsUp Then
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
                If Map(MapLevel, x, y).TileType = MapTile.MapTileType.StairsDown Then
                    ReturnValue = New Point(y, x)
                End If
            Next
        Next

        Return ReturnValue
    End Function

    Public Sub PlayerDraw()
        Debug.WriteLine(String.Format("PlayerDraw:{0},{1}", Player1.Location.x, Player1.Location.y))
        If Player1.Location.X >= ViewportOrigin.X + ViewportSize.Width - ViewportXScrollBufferGet() Then
            Debug.WriteLine("right scroll border hit")

            If ViewportOrigin.X < Map.GetLength(1) - ViewportSize.Width Then
                ' If we are too close to the right edge of the map to scroll fully, then scroll just enough
                Dim NewOriginLeft As Integer = ViewportOrigin.X + (ViewportSize.Width / 2)

                If Map.GetLength(1) - NewOriginLeft < ViewportSize.Width Then
                    ViewportOriginXSet(Map.GetLength(1) - (ViewportSize.Width) + 1)
                Else
                    ViewportOriginXSet(ViewportOrigin.X + (ViewportSize.Width / 2))
                End If

                ViewportMapDraw()
            End If

        ElseIf Player1.Location.X <= ViewportOrigin.X + ViewportYScrollBufferGet() Then
            Debug.WriteLine("left scroll border hit")

            If ViewportOrigin.X > 0 Then
                ' If we are too close to the left edge of the map to scroll fully, then scroll just enough
                Dim NewOriginLeft As Integer = ViewportOrigin.X - (ViewportSize.Width / 2)

                If NewOriginLeft < 0 Then
                    ViewportOriginXSet(0)
                Else
                    ViewportOriginXSet(ViewportOrigin.X - (ViewportSize.Width / 2))
                End If

                ViewportMapDraw()
            End If

        ElseIf Player1.Location.Y >= ViewportOrigin.Y + ViewportSize.Height - 1 - ViewportYScrollBufferGet() Then
            Debug.WriteLine("bottom scroll border hit")

            If ViewportOrigin.Y < Map.GetLength(2) - ViewportSize.Height Then
                ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
                Dim NewOriginTop As Integer = ViewportOrigin.Y + (ViewportSize.Height / 2)

                If Map.GetLength(2) - NewOriginTop < ViewportSize.Height Then
                    ViewportOriginYSet(Map.GetLength(2) - ViewportSize.Height + 1)
                Else
                    ViewportOriginYSet(ViewportOrigin.Y + (ViewportSize.Height / 2))
                End If

                ViewportMapDraw()
            End If

        ElseIf Player1.Location.Y <= ViewportOrigin.Y + ViewportXScrollBufferGet() Then
            Debug.WriteLine("top scroll border hit")

            If ViewportOrigin.Y > 0 Then
                ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
                Dim NewOriginTop As Integer = ViewportOrigin.Y - (ViewportSize.Height / 2)
                If NewOriginTop < 0 Then
                    ViewportOriginYSet(0)
                Else
                    ViewportOriginYSet(ViewportOrigin.Y - (ViewportSize.Height / 2))
                End If
                ViewportMapDraw()
            End If

        End If

        For Each p As Point In GetVisibleCells(Player1.Location, 5)
            Console.SetCursorPosition(p.X - ViewportOrigin.X, p.Y - ViewportOrigin.Y)
            Console.Write(Map(0, p.X, p.Y).DisplayCharacter)
            Map(0, p.X, p.Y).IsVisible = True
        Next

        Console.SetCursorPosition(Player1.Location.X - ViewportOrigin.X, Player1.Location.Y - ViewportOrigin.Y)

        Dim c As ConsoleColor = Console.ForegroundColor
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("@")
        Console.ForegroundColor = c
        Debug.WriteLine("7,9:" & Map(0, 7, 9).IsVisible)
    End Sub


End Module
