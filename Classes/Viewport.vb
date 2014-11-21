Public Class Viewport
    Public Padding As Integer
    Public Dimensions As Size
    Public Origin As Point
    Public ScrollBuffer As Point

    Public Sub Refresh()
        Me.Scroll()
        Me.VisibleCellsProcess()
        Me.CreaturesDraw()
        Me.ItemsDraw()
        Player1.Draw()
    End Sub

    Public Sub Clear()
        For row = MESSAGEAREAHEIGHT + 1 To Console.WindowHeight - STATUSAREAHEIGHT - 2
            Console.SetCursorPosition(0, row)
            Console.Write(Space(Console.WindowWidth - 1))
        Next
    End Sub

    Public Sub Scroll()

        If Player1.Location.X >= Me.Origin.X + Me.Dimensions.Width - Me.ScrollBuffer.X Then
            Debug.WriteLine("right scroll border hit")

            If Me.Origin.X < Map.GetLength(1) - Me.Dimensions.Width Then
                ' If we are too close to the right edge of the map to scroll fully, then scroll just enough
                Dim NewOriginLeft As Integer = Me.Origin.X + (Me.Dimensions.Width / 2)

                If Map.GetLength(1) - NewOriginLeft < Me.Dimensions.Width Then
                    Me.Origin.X = (Map.GetLength(1) - (Me.Dimensions.Width) + 1)
                Else
                    Me.Origin.X = (Me.Origin.X + (Me.Dimensions.Width / 2))
                End If

                Me.MapDraw()
            End If

        ElseIf Player1.Location.X <= Me.Origin.X + Me.ScrollBuffer.X Then
            Debug.WriteLine("left scroll border hit")

            If Me.Origin.X > 0 Then
                ' If we are too close to the left edge of the map to scroll fully, then scroll just enough
                Dim NewOriginLeft As Integer = Me.Origin.X - (Me.Dimensions.Width / 2)

                If NewOriginLeft < 0 Then
                    Me.Origin.X = (0)
                Else
                    Me.Origin.X = (Me.Origin.X - (Me.Dimensions.Width / 2))
                End If

                Me.MapDraw()
            End If

        ElseIf Player1.Location.Y >= Me.Origin.Y + Me.Dimensions.Height - 1 - Me.ScrollBuffer.Y Then
            Debug.WriteLine("bottom scroll border hit")

            If Me.Origin.Y < Map.GetLength(2) - Me.Dimensions.Height Then
                ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
                Dim NewOriginTop As Integer = Me.Origin.Y + (Me.Dimensions.Height / 2)
                If Map.GetLength(2) - NewOriginTop < Me.Dimensions.Height Then
                    Me.Origin.Y = (Map.GetLength(2) - Me.Dimensions.Height + 1)
                Else
                    Me.Origin.Y = (Me.Origin.Y + (Me.Dimensions.Height / 2))
                End If

                Me.MapDraw()
            End If

        ElseIf Player1.Location.Y <= Me.Origin.Y + Me.ScrollBuffer.Y Then
            Debug.WriteLine("top scroll border hit")

            If Me.Origin.Y > 0 Then
                ' If we are too close to the bottom edge of the map to scroll fully, then scroll just enough
                Dim NewOriginTop As Integer = Me.Origin.Y - (Me.Dimensions.Height / 2)
                If NewOriginTop < 0 Then
                    Me.Origin.Y = (0)
                Else
                    Me.Origin.Y = (Me.Origin.Y - (Me.Dimensions.Height / 2))
                End If
                Me.MapDraw()
            End If
        End If



    End Sub

    Public Sub VisibleCellsProcess()
        ' Light the cells that are visible as a result of the player move
        Dim NewlyVisibleCells As List(Of Point) = VisibleCellsGet(Player1.Location, Player1.AttributeGet(CreatureAttribute.AttributeId.VisualRange))
        For Each p As Point In NewlyVisibleCells
            Console.SetCursorPosition(p.X - Me.Origin.X, p.Y - Me.Origin.Y)
            Map(Player1.MapLevel, p.X, p.Y).IsRevealed = True
            Map(Player1.MapLevel, p.X, p.Y).IsVisible = True
            MapTile.Render(p)
        Next

        ' dim the cells that are no longer visible as a result of the player move
        For Each p As Point In Player1.VisibleCells
            If Not NewlyVisibleCells.Contains(p) Then
                Map(Player1.MapLevel, p.X, p.Y).IsVisible = False
                Console.SetCursorPosition(p.X - Me.Origin.X, p.Y - Me.Origin.Y)
                MapTile.Render(p)
            End If
        Next

        Player1.VisibleCells = NewlyVisibleCells
    End Sub

    Public Sub CreaturesDraw()
        ' get the list of visible cells once so we don't have to recalculate repeatedly
        Dim VisibleCells As List(Of Point) = VisibleCellsGet(Player1.Location, Player1.AttributeGet(CreatureAttribute.AttributeId.VisualRange))

        For Each m As CreatureBase In Creatures
            ' Is the monster's location even on the screen?
            If Me.ContainsLocation(m.Location) Then

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

    Public Sub ItemsDraw()
        ' get the list of visible cells once so we don't have to recalculate repeatedly
        Dim VisibleCells As List(Of Point) = VisibleCellsGet(Player1.Location, Player1.AttributeGet(CreatureAttribute.AttributeId.VisualRange))

        For Each a As ItemBase In Items
            ' Is the item's location even on the screen?
            If ContainsLocation(a.Location) Then

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

    Public Function ContainsLocation(Location As Point) As Boolean
        Dim ReturnValue As Boolean = False
        If Location.X >= Me.Origin.X AndAlso Location.X <= Me.Origin.X + Me.Dimensions.Width Then
            If Location.Y >= Me.Origin.Y AndAlso Location.Y <= Me.Origin.Y + Me.Dimensions.Height Then
                ReturnValue = True
            End If
        End If
        Return ReturnValue
    End Function

    Public Sub MapDraw()
        For x As Integer = Me.Origin.X To Me.Origin.X + Me.Dimensions.Width - 1
            For y As Integer = Me.Origin.Y To Me.Origin.Y + Me.Dimensions.Height - 2
                Console.SetCursorPosition(x - Me.Origin.X, y - Me.Origin.Y)
                Console.Write(" ")
            Next
        Next

        For x As Integer = Me.Origin.X To Me.Origin.X + Me.Dimensions.Width - 1
            For y As Integer = Me.Origin.Y To Me.Origin.Y + Me.Dimensions.Height - 2
                If Map(Player1.MapLevel, x, y).IsRevealed Then
                    Console.SetCursorPosition(x - Me.Origin.X, y - Me.Origin.Y)
                    MapTile.Render(New Point(x, y))
                End If
            Next
        Next
    End Sub

    Public Sub StatusUpdate()
        Dim Anchor As Integer = Console.WindowHeight - MESSAGEAREAHEIGHT
        Console.SetCursorPosition(0, Anchor)
        Console.Write("{0}, {1} {2}  ", Player1.Name, Player1.Alignment, Player1.Class)

        Console.SetCursorPosition(0, Anchor + 1)
        Console.Write(Space(20))
        Console.SetCursorPosition(0, Anchor + 1)
        Console.Write("HP:{0}/{2} AC:{1}  ", Player1.AttributeGet(CreatureAttribute.AttributeId.HitPoints_Current), Player1.AttributeGet(CreatureAttribute.AttributeId.ArmorClass), Player1.AttributeGet(CreatureAttribute.AttributeId.HitPoints_Base))

        Console.SetCursorPosition(0, Anchor + 2)
        Console.Write("STR:{0} INT:{1} WIS:{2}  ", Player1.AttributeGet(CreatureAttribute.AttributeId.Strength), Player1.AttributeGet(CreatureAttribute.AttributeId.Intelligence), Player1.AttributeGet(CreatureAttribute.AttributeId.Wisdom))

        Console.SetCursorPosition(0, Anchor + 3)
        Console.Write("DEX:{0} CON:{1} CHA:{2}  ", Player1.AttributeGet(CreatureAttribute.AttributeId.Dexterity), Player1.AttributeGet(CreatureAttribute.AttributeId.Constitution), Player1.AttributeGet(CreatureAttribute.AttributeId.Charisma))

        Console.SetCursorPosition(0, Anchor + 4)
        Console.Write("Weight: {0} ", Player1.AttributeGet(CreatureAttribute.AttributeId.Weight))

    End Sub

End Class
