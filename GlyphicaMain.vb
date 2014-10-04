Imports System.Drawing
Module GlyphicaMain

    Const TESTMAPLOCATION1 As String = "C:\Users\duane\Documents\GitHub\Glyphica\Map Files\"
    Const TESTMAPLOCATION2 As String = "C:\Users\droelands\Documents\GitHub\Glyphica\Map Files\"

    Const SOLIDBLOCK As Byte = 219
    Const HORIZONTALWALL As Byte = 205
    Const VERTICALWALL As Byte = 186
    Const UPPERLEFTCORNER As Byte = 201
    Const LOWERLEFTCORNER As Byte = 200
    Const UPPERRIGHTCORNER As Byte = 187
    Const LOWERRIGHTCORNER As Byte = 188
    Const SINGLEHORIZONTALLINE As Byte = 196
    Const DOOR As Byte = 176

    Const MESSAGEAREAHEIGHT As Integer = 5
    Const STATUSAREAHEIGHT As Integer = 5


    Public Map(,,) As MapTile      ' level, x, y

    Dim ViewportSize As Size
    Dim ViewportOrigin As Point



    Public Player1 As Player

    Public Monsters As New List(Of Monster)
    Public Artifacts As New List(Of Artifact)
    Public Messages As New List(Of Message)

    Public Sub Main()

        Console.CursorVisible = False
        Console.WindowWidth = 80
        Console.WindowHeight = 40
        Console.SetBufferSize(80, 40)

        Player1 = New Player
        Player1.MapLevel = 0
        Player1.HitDice = "4d8"
        Player1.ArmorClass = 10

        ViewportSize = New Size(Console.WindowWidth, Console.WindowHeight - STATUSAREAHEIGHT)
        ViewportOrigin = New Point(0, 0)     ' The upper-left coordinate of the rectangular section of the map displayed in the viewport
        ViewportBorderDraw()

        MapLoad()

        Player1.Location = New Point(13, 13)

        ' testing/debugging
        Monsters.Add(New Kobold(0, New Point(33, 18)))

        Dim t As New Artifact
        t.Name = "Potion"
        t.Location = New Point(33, 16)
        t.DisplayColor = ConsoleColor.Red
        t.DisplayCharacter = "p"
        'Artifacts.Add(t)

        ViewportPlayerDraw()

        Trace.Listeners.Add(New GlyphicaTraceLilstener)

        ' MAIN GAME LOOP
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

                Case ConsoleKey.L
                    For Each p As Point In Player1.VisibleCells
                        Console.SetCursorPosition(p.X, p.Y)
                        Console.ForegroundColor = ConsoleColor.Yellow
                        Console.Write("x")
                    Next

                Case ConsoleKey.K
                    ViewportMapDraw()
            End Select

            Select Case KeyPress.KeyChar
                Case ">"c  '' go down stairs
                    If MapTileGet(Player1.Location).TileType = MapTile.MapTileType.StairsDown Then
                        Player1.MapLevel += 1
                        ViewportMapDraw()
                        Player1.Location = StairsUpFind()
                    End If

                Case "<"c  '' go up stairs
                    If MapTileGet(Player1.Location).TileType = MapTile.MapTileType.StairsUp Then
                        Player1.MapLevel -= 1
                        ViewportMapDraw()
                        Player1.Location = StairsDownFind()
                    End If

            End Select

            Select Case PlayerMoveAttempt(ToLocation)
                Case Player.PlayerMoveResult.Move
                    Player1.Location = ToLocation
                Case Player.PlayerMoveResult.Blocked
                    Select Case MapTileGet(ToLocation).TileType
                        Case MapTile.MapTileType.Wall
                            MessageWrite("You bump your head.")
                    End Select
                Case Player.PlayerMoveResult.Combat
                    Debug.WriteLine("COMBAT")
                    Dim Enemy As Monster = Monster.Find(ToLocation)
                    CombatResolve(Enemy)
                Case Player.PlayerMoveResult.Thing
                    Debug.WriteLine("THING")
            End Select

            ViewportPlayerDraw()
            ViewportMonstersDraw()
            'ViewportArtifactsDraw()

        Loop While KeyPress.Key <> ConsoleKey.X And Player1.HitPoints > 0

        If Player1.HitPoints <= 0 Then
            MessageWrite("You have died.")
            MessageWrite("Press [ENTER] to exit.")
            Console.ReadLine()

        End If



    End Sub


    Public Sub CombatResolve(Enemy As Monster)

        Debug.WriteLine("Player hp:" & Player1.HitPoints)
        Debug.WriteLine(Enemy.Name & " hp:" & Enemy.HitPoints)

        Dim PlayerInitiative As Integer = Dice.RollDice("1d20") + Player1.Initiative + 20
        Dim EnemyInitiative As Integer = Dice.RollDice("1d20") + Enemy.Initiative
        Trace.Write(String.Format("Initiative P:{0} E:{1}", PlayerInitiative, EnemyInitiative))


        ' Assign attacker and defender based on initiative rolls
        Dim Attacker As Monster = IIf(PlayerInitiative > EnemyInitiative, Player1, Enemy)
        Dim Defender As Monster = IIf(PlayerInitiative < EnemyInitiative, Player1, Enemy)

        ' Attacker goes first
        Dim AttackerRoll As Integer = Dice.RollDice("1d20")
        Trace.Write(String.Format("Attacker H:{0} AC:{1}", AttackerRoll, Defender.ArmorClass))
        If AttackerRoll >= Defender.ArmorClass Then
            Dim Damage As Integer = Dice.RollDice(Attacker.HitDice)
            If Attacker Is Player1 Then
                MessageWrite(String.Format("You hit the {0} for {1} damage!", Defender.Name, Damage))
            Else
                MessageWrite(String.Format("The {0} hit you for {1} damage!", Attacker.Name, Damage))
            End If
            Defender.HitPoints -= Damage
        Else
            If Attacker Is Player1 Then
                MessageWrite("You missed!")
            Else
                MessageWrite(String.Format("The {0} missed!", Attacker.Name))
            End If
        End If

        If Defender.HitPoints <= 0 Then
            If Defender Is Player1 Then
                MessageWrite("You have died.  Press any key...")
                WaitForKeypress()
                End
            Else
                MessageWrite(String.Format("You killed the {0}!", Defender.Name))
                MonsterKill(Defender)
                Exit Sub
            End If
        End If

        ' If the defender is still alive, combat continues

        Dim DefenderRoll As Integer = Dice.RollDice("1d20")
        Trace.Write(String.Format("Defender H:{0} AC:{1}", DefenderRoll, Attacker.ArmorClass))
        If DefenderRoll >= Attacker.ArmorClass Then
            Dim Damage As Integer = Dice.RollDice(Defender.HitDice)
            If Defender Is Player1 Then
                MessageWrite(String.Format("You hit the {0} for {1} damage!", Attacker.Name, Damage))
            Else
                MessageWrite(String.Format("The {0} hit you for {1} damage!", Defender.Name, Damage))
            End If
            Attacker.HitPoints -= Damage
        Else
            If Defender Is Player1 Then
                MessageWrite("You missed!")
            Else
                MessageWrite(String.Format("The {0} missed!", Defender.Name))
            End If
        End If

        If Attacker.HitPoints <= 0 Then
            If Attacker Is Player1 Then
                MessageWrite("You have died.  Press any key...")
                WaitForKeypress()
                End
            Else
                MessageWrite(String.Format("You killed the {0}!", Defender.Name))
                MonsterKill(Attacker)
                Exit Sub
            End If
        End If
    End Sub

    Public Sub WaitForKeypress()
        Console.ReadKey()
    End Sub

    Public Sub MonsterKill(DeadMonster As Monster)
        Artifacts.Add(New Corpse(DeadMonster.MapLevel, DeadMonster.Location, DeadMonster.Name))
        Monsters.Remove(DeadMonster)
    End Sub



    Public Function PlayerMoveAttempt(ToLocation As Point) As Player.PlayerMoveResult
        Dim ReturnValue As Player.PlayerMoveResult = Nothing

        ' First, is there a monster here?  If so, the result is combat
        Dim MonsterFound As Boolean = False
        For Each m As Monster In Monsters
            If m.MapLevel = Player1.MapLevel AndAlso m.Location.X = ToLocation.X AndAlso m.Location.Y = ToLocation.Y Then
                ReturnValue = Player.PlayerMoveResult.Combat
                MonsterFound = True
                Debug.WriteLine("Monster here")
                Exit For
            End If
        Next

        ' No monster.  Is there a thing here?
        Dim ThingFound As Boolean = False
        If Not MonsterFound Then
            For Each t As Artifact In Artifacts
                If t.MapLevel = Player1.MapLevel AndAlso t.Location.X = ToLocation.X AndAlso t.Location.Y = ToLocation.Y Then
                    ReturnValue = Player.PlayerMoveResult.Thing
                    ThingFound = True
                    Debug.WriteLine("Thing here")
                    Exit For
                End If
            Next
        End If

        ' No monster and no things
        ' Check for collision with map feature
        If (Not MonsterFound) And (Not ThingFound) Then
            Select Case MapTileGet(ToLocation).TileType
                Case MapTile.MapTileType.Empty
                    ReturnValue = Player.PlayerMoveResult.Move
                Case MapTile.MapTileType.Wall
                    ReturnValue = Player.PlayerMoveResult.Blocked
                Case MapTile.MapTileType.Door
                    Map(Player1.MapLevel, ToLocation.X, ToLocation.Y).TileType = MapTile.MapTileType.Empty
                    Debug.WriteLine("The door opens")
                    Return Player.PlayerMoveResult.Move
                Case MapTile.MapTileType.DoorLocked
                    Debug.WriteLine("The door is locked")
                    Return Player.PlayerMoveResult.Blocked
            End Select
        End If

        Return ReturnValue
    End Function

    Public Sub MapLoad()

        ' this is temporary code which loads a text file map into the integer-based array which is the live map
        Dim MapLocation As String
        If System.IO.Directory.Exists(TESTMAPLOCATION1) Then
            MapLocation = TESTMAPLOCATION1
        Else
            MapLocation = TESTMAPLOCATION2
        End If


        Dim MapLines As New List(Of String)
        Using sr As New System.IO.StreamReader(MapLocation & "testmap3.txt")
            Dim line As String = sr.ReadLine
            Do While line IsNot Nothing
                MapLines.Add(line)
                line = sr.ReadLine
            Loop
        End Using

        ReDim Map(Player1.MapLevel, MapLines(0).Length - 1, MapLines.Count - 1)

        Dim y As Integer = 0
        For Each MapLine As String In MapLines
            For x As Integer = 0 To MapLine.Length - 1
                'Debug.WriteLine(x & "," & y)
                Select Case MapLine.Substring(x, 1)
                    Case " "
                        Map(Player1.MapLevel, x, y) = New MapTile(MapTile.MapTileType.Empty)
                    Case "#"
                        Map(Player1.MapLevel, x, y) = New MapTile(MapTile.MapTileType.Wall)
                    Case ">"
                        Map(Player1.MapLevel, x, y) = New MapTile(MapTile.MapTileType.StairsDown)
                    Case "<"
                        Map(Player1.MapLevel, x, y) = New MapTile(MapTile.MapTileType.StairsUp)
                    Case "D"
                        Map(Player1.MapLevel, x, y) = New MapTile(MapTile.MapTileType.Door)
                End Select
            Next
            y += 1
        Next

    End Sub

    Private Sub GraphicsCharacterDraw(Character As Byte)
        Dim c As Char = System.Text.Encoding.GetEncoding(437).GetChars(New Byte() {Character})(0)
        Console.Write(c)
    End Sub

    Public Function MapTileGet(Location As Point) As MapTile
        Return Map(Player1.MapLevel, Location.X, Location.Y)
    End Function

    Public Function StairsUpFind() As Point
        Dim ReturnValue As Point = Nothing

        For x As Integer = 0 To Map.GetUpperBound(1)
            For y As Integer = 0 To Map.GetUpperBound(2)
                If Map(Player1.MapLevel, x, y).TileType = MapTile.MapTileType.StairsUp Then
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
                If Map(Player1.MapLevel, x, y).TileType = MapTile.MapTileType.StairsDown Then
                    ReturnValue = New Point(y, x)
                End If
            Next
        Next

        Return ReturnValue
    End Function

    Private Sub MaptileRender(Location As Point)

        Dim t As MapTile.MapTileType = Map(Player1.MapLevel, Location.X, Location.Y).TileType
        If Not MapTileGet(Location).IsRevealed Then
            Exit Sub
        End If

        Console.SetCursorPosition(Location.X, Location.Y)

        Select Case t
            Case MapTile.MapTileType.Wall
                If Map(Player1.MapLevel, Location.X, Location.Y).IsVisible Then
                    Console.ForegroundColor = ConsoleColor.White
                Else
                    Console.ForegroundColor = ConsoleColor.DarkGray
                End If

                ' vertical wall
                If Map(Player1.MapLevel, Location.X - 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                    Map(Player1.MapLevel, Location.X + 1, Location.Y).TileType = MapTile.MapTileType.Empty Then
                    GraphicsCharacterDraw(VERTICALWALL)
                End If

                ' horizontal wall
                If Map(Player1.MapLevel, Location.X, Location.Y - 1).TileType = MapTile.MapTileType.Empty And _
                    Map(Player1.MapLevel, Location.X, Location.Y + 1).TileType = MapTile.MapTileType.Empty Then
                    GraphicsCharacterDraw(HORIZONTALWALL)
                End If

                ' upper-left corner
                If Map(Player1.MapLevel, Location.X - 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                    Map(Player1.MapLevel, Location.X, Location.Y - 1).TileType = MapTile.MapTileType.Empty Then
                    GraphicsCharacterDraw(UPPERLEFTCORNER)
                End If

                ' lower-left corner
                If Map(Player1.MapLevel, Location.X - 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                    Map(Player1.MapLevel, Location.X, Location.Y + 1).TileType = MapTile.MapTileType.Empty Then
                    GraphicsCharacterDraw(LOWERLEFTCORNER)
                End If

                ' upper-right corner
                If Map(Player1.MapLevel, Location.X + 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                    Map(Player1.MapLevel, Location.X, Location.Y - 1).TileType = MapTile.MapTileType.Empty Then
                    GraphicsCharacterDraw(UPPERRIGHTCORNER)
                End If

                ' lower-right corner
                If Map(Player1.MapLevel, Location.X + 1, Location.Y).TileType = MapTile.MapTileType.Empty And _
                    Map(Player1.MapLevel, Location.X, Location.Y + 1).TileType = MapTile.MapTileType.Empty Then
                    GraphicsCharacterDraw(LOWERRIGHTCORNER)
                End If

            Case MapTile.MapTileType.Door
                Dim c As ConsoleColor = Console.ForegroundColor
                Console.ForegroundColor = ConsoleColor.DarkYellow
                GraphicsCharacterDraw(DOOR)
                Console.ForegroundColor = c

            Case MapTile.MapTileType.Empty
                If Map(Player1.MapLevel, Location.X, Location.Y).IsVisible Then
                    Console.ForegroundColor = ConsoleColor.White
                Else
                    Console.ForegroundColor = ConsoleColor.DarkGray
                End If
                Console.Write(".")

            Case MapTile.MapTileType.StairsDown
                Console.Write(">")

            Case MapTile.MapTileType.StairsUp
                Console.Write("<")

        End Select
    End Sub

#Region "MessageWrite Methods"
    ' This overload of MessageWrite allows us to specify White as the default color without using an optional parameter.
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
#End Region

#Region "Viewport Methods"

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

        ' Light the cells that are visible as a result of the player move
        Dim NewlyVisibleCells As List(Of Point) = VisibleCellsGet(Player1.Location, Player1.VisualRange)
        For Each p As Point In NewlyVisibleCells
            Console.SetCursorPosition(p.X - ViewportOrigin.X, p.Y - ViewportOrigin.Y)
            Map(Player1.MapLevel, p.X, p.Y).IsRevealed = True
            Map(Player1.MapLevel, p.X, p.Y).IsVisible = True
            MaptileRender(p)
        Next

        ' dim the cells that are no longer visible as a result of the player move
        For Each p As Point In Player1.VisibleCells
            If Not NewlyVisibleCells.Contains(p) Then
                Map(Player1.MapLevel, p.X, p.Y).IsVisible = False
                Console.SetCursorPosition(p.X - ViewportOrigin.X, p.Y - ViewportOrigin.Y)
                MaptileRender(p)
            End If
        Next

        Player1.VisibleCells = NewlyVisibleCells

        Console.SetCursorPosition(Player1.Location.X - ViewportOrigin.X, Player1.Location.Y - ViewportOrigin.Y)

        Dim c As ConsoleColor = Console.ForegroundColor
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("@")
    End Sub

    Public Function ViewportXScrollBufferGet() As Integer
        Return ViewportSize.Width / 5
    End Function

    Public Function ViewportYScrollBufferGet() As Integer
        Return ViewportSize.Height / 5
    End Function

    Public Sub ViewportLocationClear(Location As Point)
        Console.SetCursorPosition(Location.X - ViewportOrigin.X, Location.Y - ViewportOrigin.Y)
        If Map(0, Location.X - ViewportOrigin.X, Location.Y - ViewportOrigin.Y).IsRevealed Then
            MaptileRender(Location)
            'Console.Write(Map(0, Location.X - ViewportOrigin.X, Location.Y - ViewportOrigin.Y).DisplayCharacter)
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
        For x = 0 To ViewportSize.Width - 1
            Console.SetCursorPosition(x, ViewportSize.Height - 1)
            GraphicsCharacterDraw(SINGLEHORIZONTALLINE)
            Console.SetCursorPosition(x, 5)
            GraphicsCharacterDraw(SINGLEHORIZONTALLINE)
        Next
    End Sub

    'Public Sub ViewportArtifactsDraw()
    '    ' get the list of visible cells once so we don't have to recalculate repeatedly
    '    Dim VisibleCells As List(Of Point) = VisibleCellsGet(Player1.Location, Player1.VisualRange)

    '    For Each t As Artifact In Artifacts
    '        ' Is the artifact's location even on the screen?
    '        If IsLocationInViewport(t.Location) Then
    '            Dim ArtifactIsVisible As Boolean = False
    '            ' is the monster in the list of cells visible to the player?
    '            For Each p As Point In VisibleCells
    '                If p.X = t.Location.X And p.Y = t.Location.Y Then
    '                    ArtifactIsVisible = True
    '                    Exit For
    '                End If
    '            Next

    '            ' If the artifact is visible, draw it, otherwise draw the map location as it should be displayed
    '            Console.SetCursorPosition(t.Location.X - ViewportOrigin.X, t.Location.Y - ViewportOrigin.Y)
    '            If ArtifactIsVisible Then
    '                Dim c As ConsoleColor = Console.ForegroundColor
    '                Console.ForegroundColor = t.DisplayColor
    '                Console.Write(t.DisplayCharacter)
    '                Console.ForegroundColor = c
    '            Else
    '                MaptileRender(t.Location)
    '            End If
    '        End If
    '    Next
    'End Sub

    Public Sub ViewportMonstersDraw()

        ' get the list of visible cells once so we don't have to recalculate repeatedly
        Dim VisibleCells As List(Of Point) = VisibleCellsGet(Player1.Location, Player1.VisualRange)

        For Each m As Monster In Monsters
            ' Is the monster's location even on the screen?
            If IsLocationInViewport(m.Location) Then

                ' make the monster invisible, and only make it visible if it's in the list of visible cells
                m.Visible = False
                If VisibleCells.Contains(m.Location) Then
                    m.Draw()
                Else
                    MaptileRender(m.Location)
                End If
            End If
        Next
    End Sub

    Public Function IsLocationInViewport(Location) As Boolean
        Dim ReturnValue As Boolean = False
        If Location.x >= ViewportOrigin.X AndAlso Location.x <= ViewportOrigin.X + ViewportSize.Width Then
            If Location.y >= ViewportOrigin.Y AndAlso Location.y <= ViewportOrigin.Y + ViewportSize.Height Then
                ReturnValue = True
            End If
        End If
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
                If Map(Player1.MapLevel, x, y).IsRevealed Then
                    Console.SetCursorPosition(x - ViewportOrigin.X, y - ViewportOrigin.Y)
                    MaptileRender(New Point(x, y))
                End If
            Next
        Next
    End Sub

#End Region

End Module
