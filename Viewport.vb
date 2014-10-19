'
' Methods related to the display of the player and map
Module Viewport
    Public Sub MapScroll()
        If Player1.Location.X >= ViewportOrigin.X + ViewportSize.Width - Viewport.XScrollBufferGet() Then
            Debug.WriteLine("right scroll border hit")

            If ViewportOrigin.X < Map.GetLength(1) - ViewportSize.Width Then
                ' If we are too close to the right edge of the map to scroll fully, then scroll just enough
                Dim NewOriginLeft As Integer = ViewportOrigin.X + (ViewportSize.Width / 2)

                If Map.GetLength(1) - NewOriginLeft < ViewportSize.Width Then
                    OriginXSet(Map.GetLength(1) - (ViewportSize.Width) + 1)
                Else
                    OriginXSet(ViewportOrigin.X + (ViewportSize.Width / 2))
                End If

                Viewport.MapDraw()
            End If

        ElseIf Player1.Location.X <= ViewportOrigin.X + Viewport.XScrollBufferGet() Then
            Debug.WriteLine("left scroll border hit")

            If ViewportOrigin.X > 0 Then
                ' If we are too close to the left edge of the map to scroll fully, then scroll just enough
                Dim NewOriginLeft As Integer = ViewportOrigin.X - (ViewportSize.Width / 2)

                If NewOriginLeft < 0 Then
                    OriginXSet(0)
                Else
                    OriginXSet(ViewportOrigin.X - (ViewportSize.Width / 2))
                End If

                Viewport.MapDraw()
            End If

        ElseIf Player1.Location.Y >= ViewportOrigin.Y + ViewportSize.Height - 1 - Viewport.YScrollBufferGet() Then
            Debug.WriteLine("bottom scroll border hit")

            If ViewportOrigin.Y < Map.GetLength(2) - ViewportSize.Height Then
                ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
                Dim NewOriginTop As Integer = ViewportOrigin.Y + (ViewportSize.Height / 2)
                If Map.GetLength(2) - NewOriginTop < ViewportSize.Height Then
                    OriginYSet(Map.GetLength(2) - ViewportSize.Height + 1)
                Else
                    OriginYSet(ViewportOrigin.Y + (ViewportSize.Height / 2))
                End If

                Viewport.MapDraw()
            End If

        ElseIf Player1.Location.Y <= ViewportOrigin.Y + Viewport.YScrollBufferGet() Then
            Debug.WriteLine("top scroll border hit")

            If ViewportOrigin.Y > 0 Then
                ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
                Dim NewOriginTop As Integer = ViewportOrigin.Y - (ViewportSize.Height / 2)
                If NewOriginTop < 0 Then
                    OriginYSet(0)
                Else
                    OriginYSet(ViewportOrigin.Y - (ViewportSize.Height / 2))
                End If
                Viewport.MapDraw()
            End If
        End If
    End Sub

    Public Sub VisibleCellsProcess()
        ' Light the cells that are visible as a result of the player move
        Dim NewlyVisibleCells As List(Of Point) = VisibleCellsGet(Player1.Location, Player1.VisualRange)
        For Each p As Point In NewlyVisibleCells
            Console.SetCursorPosition(p.X - ViewportOrigin.X, p.Y - ViewportOrigin.Y)
            Map(Player1.MapLevel, p.X, p.Y).IsRevealed = True
            Map(Player1.MapLevel, p.X, p.Y).IsVisible = True
            MapTile.Render(p)
        Next

        ' dim the cells that are no longer visible as a result of the player move
        For Each p As Point In Player1.VisibleCells
            If Not NewlyVisibleCells.Contains(p) Then
                Map(Player1.MapLevel, p.X, p.Y).IsVisible = False
                Console.SetCursorPosition(p.X - ViewportOrigin.X, p.Y - ViewportOrigin.Y)
                MapTile.Render(p)
            End If
        Next

        Player1.VisibleCells = NewlyVisibleCells
    End Sub

    Public Sub StatusUpdate()
        Dim Anchor As Integer = Console.WindowHeight - MESSAGEAREAHEIGHT
        Console.SetCursorPosition(0, Anchor)
        Console.Write("{0}, {1} {2}", Player1.Name, Player1.Alignment, Player1.Class)

        Console.SetCursorPosition(0, Anchor + 1)
        Console.Write(Space(20))
        Console.SetCursorPosition(0, Anchor + 1)
        Console.Write("HP:{0}/{2} AC:{1}", Player1.HitPointsCurrent, Player1.ArmorClass, Player1.HitPointsMax)

        Console.SetCursorPosition(0, Anchor + 2)
        Console.Write("STR:{0} INT:{1} WIS:{2}", Player1.Strength, Player1.Intelligence, Player1.Wisdom)

        Console.SetCursorPosition(0, Anchor + 3)
        Console.Write("DEX:{0} CON:{1} CHA:{2}", Player1.Dexterity, Player1.Constitution, Player1.Charisma)

    End Sub

#Region "Viewport.MessageWrite Methods"
    ' This overload of Viewport.MessageWrite allows us to specify White as the default color without using an optional parameter.
    ' Overloads > optional parameters
    Public Sub MessageWrite(Message As String)
        MessageWrite(Message, ConsoleColor.White)
    End Sub

    Public Sub MessageWrite(NewMessage As String, NewMessageColor As ConsoleColor)
        Messages.Add(New Message(NewMessage, NewMessageColor))
        Dim FirstMessageLine As Integer = IIf(Messages.Count <= MESSAGEAREAHEIGHT, 0, Messages.Count - MESSAGEAREAHEIGHT)
        Dim LastMessageLine As Integer = Messages.Count - 1

        Dim c As ConsoleColor = Console.ForegroundColor
        For y = FirstMessageLine To LastMessageLine
            Console.SetCursorPosition(0, y - FirstMessageLine)
            Console.ForegroundColor = Messages(y).Color
            Console.Write(Messages(y).Text & Space(Console.WindowWidth - Messages(y).Text.Length))
        Next
        Console.ForegroundColor = c
    End Sub

    Public Function XScrollBufferGet() As Integer
        Return ViewportSize.Width / 5
    End Function

    Public Function YScrollBufferGet() As Integer
        Return ViewportSize.Height / 5
    End Function

#End Region

    Public Sub ItemsDraw()
        ' get the list of visible cells once so we don't have to recalculate repeatedly
        Dim VisibleCells As List(Of Point) = VisibleCellsGet(Player1.Location, Player1.VisualRange)

        For Each a As ItemBase In Items
            ' Is the item's location even on the screen?
            If Viewport.ContainsLocation(a.Location) Then

                ' make the artifact invisible, and only make it visible if it's in the list of visible cells
                a.IsVisible = False
                If VisibleCells.Contains(a.Location) Then
                    a.Draw()
                Else
                    MapTile.Render(a.Location)
                End If
            End If
        Next
    End Sub

    Public Sub OriginXSet(x As Integer)
        ViewportOrigin = New Point(x, ViewportOrigin.Y)
    End Sub

    Public Sub OriginYSet(y As Integer)
        ViewportOrigin = New Point(ViewportOrigin.X, y)
    End Sub

    Public Sub Clear()
        For row = MESSAGEAREAHEIGHT + 1 To Console.WindowHeight - STATUSAREAHEIGHT - 2
            Console.SetCursorPosition(0, row)
            Console.Write(Space(Console.WindowWidth - 1))
        Next
    End Sub

    Public Sub CreaturesDraw()
        ' get the list of visible cells once so we don't have to recalculate repeatedly
        Dim VisibleCells As List(Of Point) = VisibleCellsGet(Player1.Location, Player1.VisualRange)

        For Each m As CreatureBase In Creatures
            ' Is the monster's location even on the screen?
            If Viewport.ContainsLocation(m.Location) Then

                ' make the creature invisible, and only make it visible if it's in the list of visible cells
                m.IsVisible = False
                If VisibleCells.Contains(m.Location) Then
                    m.Draw()
                Else
                    MapTile.Render(m.Location)
                End If
            End If
        Next
    End Sub

    Public Sub MapDraw()
        For x As Integer = ViewportOrigin.X To ViewportOrigin.X + ViewportSize.Width - 1
            For y As Integer = ViewportOrigin.Y To ViewportOrigin.Y + ViewportSize.Height - 2
                Console.SetCursorPosition(x - ViewportOrigin.X, y - ViewportOrigin.Y)
                Console.Write(" ")
            Next
        Next

        For x As Integer = ViewportOrigin.X To ViewportOrigin.X + ViewportSize.Width - 1
            For y As Integer = ViewportOrigin.Y To ViewportOrigin.Y + ViewportSize.Height - 2
                If Map(Player1.MapLevel, x, y).IsRevealed Then
                    Console.SetCursorPosition(x - ViewportOrigin.X, y - ViewportOrigin.Y)
                    MapTile.Render(New Point(x, y))
                End If
            Next
        Next
    End Sub

    'Public Sub LocationClear(Location As Point)
    '    Console.SetCursorPosition(Location.X - ViewportOrigin.X, Location.Y - ViewportOrigin.Y)
    '    If Map(0, Location.X - ViewportOrigin.X, Location.Y - ViewportOrigin.Y).IsRevealed Then
    '        MapTile.Render(Location)
    '    Else
    '        Console.Write(" ")
    '    End If
    'End Sub

    Public Sub BorderDraw()
        For x = 0 To ViewportSize.Width - 1
            Console.SetCursorPosition(x, ViewportSize.Height - 1)
            GraphicsCharacterDraw(SINGLEHORIZONTALLINE)
            Console.SetCursorPosition(x, 5)
            GraphicsCharacterDraw(SINGLEHORIZONTALLINE)
        Next

    End Sub

    Public Function ContainsLocation(Location As Point) As Boolean
        Dim ReturnValue As Boolean = False
        If Location.X >= ViewportOrigin.X AndAlso Location.X <= ViewportOrigin.X + ViewportSize.Width Then
            If Location.Y >= ViewportOrigin.Y AndAlso Location.Y <= ViewportOrigin.Y + ViewportSize.Height Then
                ReturnValue = True
            End If
        End If
        Return ReturnValue
    End Function

End Module



