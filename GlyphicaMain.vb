Module GlyphicaMain

    Public Map(,,) As MapTile      ' level, x, y

    Public ViewportSize As Size
    Public ViewportOrigin As Point

    Public Player1 As Player

    ' The monsters and NPCs that inhabit the game
    Public Creatures As New List(Of CreatureBase)

    ' The objects in the dungeon
    Public Items As New List(Of ItemBase)

    ' Message buffer
    Public Messages As New List(Of Message)

    Public Enum CombatType
        Melee
        Ranged
        Magic
    End Enum

    Public Sub Main()

        Console.CursorVisible = False
        Console.WindowWidth = 120
        Console.WindowHeight = 50
        Console.SetBufferSize(120, 50)

        Player1 = New Player
        Player1.MapLevel = 0
        Player1.HitDice = "4d8"
        Player1.BaseArmorClass = 10
        Player1.DamageDice = "1d8"
        Player1.Name = "Duane"
        With Player1
            .BaseStrength = 18
            .BaseIntelligence = 17
            .BaseWisdom = 16
            .BaseDexterity = 15
            .BaseConstitution = 14
            .BaseCharisma = 13
        End With

        For x = 1 To 1
            Dim m As New MaceMedium("Mace #" & x)
            Player1.Inventory.Add(m)
        Next

        Dim ab As New ArmorBreastPlate
        Player1.Inventory.Add(ab)

        Dim ad As New ArmorHalfPlate
        Player1.Inventory.Add(ad)

        Dim ap As New ArmorChainMail
        Player1.Inventory.Add(ap)

        For Each InventoryItem As ItemBase In Player1.Inventory
            If TypeOf (InventoryItem) Is WeaponBase Then
                Debug.Write("w")
            End If
            If TypeOf (InventoryItem) Is ArmorBase Then
                Debug.Write("a")
            End If
        Next


        ViewportSize = New Size(Console.WindowWidth, Console.WindowHeight - STATUSAREAHEIGHT)
        ViewportOrigin = New Point(0, 0)     ' The upper-left coordinate of the rectangular section of the map displayed in the viewport
        ViewportBorderDraw()

        MapLoad()

        Player1.Location = New Point(13, 13)
        Player1.Draw()

        ' testing/debugging
        Creatures.Add(New Kobold(0, New Point(19, 13)))
        Creatures.Add(New GlyphSpider(0, New Point(22, 13)))

        ViewportPlayerMoveProcess()

        Trace.Listeners.Add(New GlyphicaTraceListener)

        ' MAIN GAME LOOP
        Dim KeyPress As ConsoleKeyInfo
        Do
            StatusUpdate()

            Dim ToLocation As New Point
            KeyPress = Console.ReadKey(True)

            Select Case KeyPress.Key

                Case ConsoleKey.I
                    InventoryManage()

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

                Case ConsoleKey.S  ' shoot ranged weapon
                    If KeyPress.Modifiers And ConsoleModifiers.Shift Then
                        ' allow player to choose target
                        Dim Target As CreatureBase = TargetSelect(Player1.Location)
                        If Target IsNot Nothing Then
                            CombatResolve(Target, CombatType.Ranged)
                        Else
                            ViewportCreaturesDraw()
                            ViewportArtifactsDraw()
                            Player1.Draw()
                            Continue Do
                        End If
                    Else
                        ' lowercase "s" automatically target the closest creature
                        Dim ClosestCreature As CreatureBase = Player1.FindClosest
                        If ClosestCreature IsNot Nothing AndAlso ClosestCreature.IsVisible Then
                            CombatResolve(Player1.FindClosest(), CombatType.Ranged)
                        Else
                            MessageWrite("There is nothing to shoot here.")
                        End If
                    End If

                    ViewportCreaturesDraw()
                    ViewportArtifactsDraw()
                    Player1.Draw()

                    ' jump to the bottom of the loop as there is no "move" to process
                    Continue Do

            End Select

            Select Case KeyPress.KeyChar
                Case ">"c  '' go down stairs
                    If MapTile.GetTile(Player1.Location).TileType = MapTile.MapTileType.StairsDown Then
                        Player1.MapLevel += 1
                        ViewportMapDraw()
                        Player1.Location = StairsUpFind()
                    End If

                Case "<"c  '' go up stairs
                    If MapTile.GetTile(Player1.Location).TileType = MapTile.MapTileType.StairsUp Then
                        Player1.MapLevel -= 1
                        ViewportMapDraw()
                        Player1.Location = StairsDownFind()
                    End If

            End Select

            Select Case PlayerMoveAttempt(ToLocation)
                Case Player.PlayerMoveResult.Move
                    Player1.Location = ToLocation
                Case Player.PlayerMoveResult.Blocked
                    Select Case MapTile.GetTile(ToLocation).TileType
                        Case MapTile.MapTileType.Wall
                            MessageWrite("You bump your head.")
                    End Select
                Case Player.PlayerMoveResult.Combat
                    Dim Enemy As CreatureBase = CreatureBase.Find(ToLocation)
                    CombatResolve(Enemy, CombatType.Melee)

            End Select

            ViewportPlayerMoveProcess()
            ViewportCreaturesDraw()
            ViewportArtifactsDraw()
            Player1.Draw()

            ' Is the player in the same square as an artifact?  If so, search the artifact for anything it contains
            For Each a As ItemBase In Items
                If a.Location = Player1.Location Then
                    MessageWrite("searching")
                    Exit For
                End If
            Next

        Loop While KeyPress.Key <> ConsoleKey.X And Player1.HitPointsCurrent > 0

        StatusUpdate()

        If Player1.HitPointsCurrent <= 0 Then
            MessageWrite("You have died.")
            MessageWrite("Press any key to exit.")
            WaitForKeypress()
        End If

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

    Public Function TargetSelect(Location As Point)
        ' give the player a way to select a target creature when more than one creature is visible

        Dim VisibleCreatures As New List(Of CreatureBase)
        For Each m As CreatureBase In Creatures
            If Player1.VisibleCells.Contains(m.Location) Then
                VisibleCreatures.Add(m)
            End If
        Next
        ' If there's only one visible monster, just select that one
        If VisibleCreatures.Count = 1 Then
            Return VisibleCreatures(0)
        End If

        ' label targets
        Dim TargetNumber As Integer = 1
        For Each t As CreatureBase In VisibleCreatures
            Console.SetCursorPosition(t.Location.X - ViewportOrigin.X, t.Location.Y - ViewportOrigin.Y)
            Console.ForegroundColor = ConsoleColor.Yellow
            Console.Write(TargetNumber.ToString.Trim)
            TargetNumber += 1
        Next

        TargetNumber = 1
        MessageWrite("Target which creature?  (ESC to cancel)")
        For Each m As CreatureBase In VisibleCreatures
            MessageWrite(String.Format("{0} - {1}", TargetNumber, m.Name))
            TargetNumber += 1
        Next

        Dim TargetKey As ConsoleKeyInfo
        TargetNumber = 0
        Do
            TargetKey = Console.ReadKey(True)
        Loop Until TargetKey.Key = ConsoleKey.Escape Or Integer.TryParse(TargetKey.KeyChar, TargetNumber)

        ' ESC - user has cancelled
        If TargetKey.Key = ConsoleKey.Escape Then
            MessageWrite("Cancelled")
            Return Nothing
        End If

        If TargetNumber > VisibleCreatures.Count Then
            MessageWrite("Invalid target")
            Return Nothing
        End If

        Return VisibleCreatures(TargetNumber - 1)

    End Function

    Public Sub CombatResolve(Enemy As CreatureBase, Type As CombatType)
        Select Case Type
            Case CombatType.Melee, CombatType.Ranged
                CombatResolvePhysical(Enemy, Type)
            Case CombatType.Magic
                CombatResolveMagical(Enemy)
        End Select

    End Sub

    Public Sub CombatResolvePhysical(Enemy As CreatureBase, Type As CombatType)

        Dim Defender As CreatureBase = Nothing
        Dim Attacker As CreatureBase = Nothing

        ' roll initiative
        ' Only matters for melee combat
        Dim PlayerInitiative As Integer
        Dim EnemyInitiative As Integer

        If Type = CombatType.Melee Then
            ' do initiative roll
            PlayerInitiative = Dice.RollDice("1d20") + Player1.Initiative + 20
            EnemyInitiative = Dice.RollDice("1d20") + Enemy.Initiative
            Attacker = IIf(PlayerInitiative > EnemyInitiative, Player1, Enemy)
            Defender = IIf(PlayerInitiative < EnemyInitiative, Player1, Enemy)
        Else
            ' ranged combat - attacker is the "shooter", i.e. whoever initiated combat
            Defender = IIf(Player1 Is Enemy, Player1, Enemy)
            Attacker = IIf(Player1 IsNot Enemy, Player1, Enemy)
        End If

        ' Attacker goes first
        ' TODO: implement "20 aleays hits" and "1 always misses"
        Dim AttackerRoll As Integer = Dice.RollDice("1d20")
        Trace.Write(String.Format("Attacker H:{0} AC:{1}", AttackerRoll, Defender.ArmorClass))
        If AttackerRoll >= Defender.ArmorClass Then
            Dim Damage As Integer = Dice.RollDice(Attacker.DamageDice)
            If Attacker Is Player1 Then
                MessageWrite(String.Format("You hit the {0} for {1} damage.", Defender.Name, Damage))
            Else
                MessageWrite(String.Format("The {0} hit you for {1} damage.", Attacker.Name, Damage))
            End If
            Defender.HitPointsCurrent -= Damage
        Else
            If Attacker Is Player1 Then
                MessageWrite(String.Format("You missed the {0}.", Defender.Name))
            Else
                MessageWrite(String.Format("The {0} missed.", Attacker.Name))
            End If
        End If

        ' did the defender die?
        If Defender.HitPointsCurrent <= 0 Then
            If Defender Is Player1 Then
                Exit Sub
            Else
                MessageWrite(String.Format("You killed the {0}!", Defender.Name))
                CreatureBase.Kill(Defender)
                Exit Sub
            End If
        End If

        ' Defender only gets to participate if this is melee combat
        ' If the defender is still alive, combat continues
        ' TODO: implement "20 aleays hits" and "1 always misses"
        If Type = CombatType.Melee Then
            Dim DefenderRoll As Integer = Dice.RollDice("1d20")
            Trace.Write(String.Format("Defender H:{0} AC:{1}", DefenderRoll, Attacker.ArmorClass))
            If DefenderRoll >= Attacker.ArmorClass Then
                Dim Damage As Integer = Dice.RollDice(Defender.HitDice)
                If Defender Is Player1 Then
                    MessageWrite(String.Format("You hit the {0} for {1} damage.", Attacker.Name, Damage))
                Else
                    MessageWrite(String.Format("The {0} hit you for {1} damage.", Defender.Name, Damage))
                End If
                Attacker.HitPointsCurrent -= Damage
            Else
                If Defender Is Player1 Then
                    MessageWrite(String.Format("You missed the {0}.", Attacker.Name))
                Else
                    MessageWrite(String.Format("The {0} missed.", Defender.Name))
                End If
            End If

            ' did the attacker die
            If Attacker.HitPointsCurrent <= 0 Then
                If Attacker Is Player1 Then
                    Exit Sub
                Else
                    MessageWrite(String.Format("You killed the {0}!", Defender.Name))
                    CreatureBase.Kill(Attacker)
                    Exit Sub
                End If
            End If
        End If

    End Sub

    Public Sub CombatResolveMagical(Enemy As CreatureBase)

    End Sub

    Public Sub WaitForKeypress()
        Console.ReadKey(True)
    End Sub

    Public Function PlayerMoveAttempt(ToLocation As Point) As Player.PlayerMoveResult
        Dim ReturnValue As Player.PlayerMoveResult = Nothing

        ' First, is there a monster here?  If so, the result is combat
        Dim MonsterFound As Boolean = False
        For Each m As CreatureBase In Creatures
            If m.MapLevel = Player1.MapLevel AndAlso m.Location.X = ToLocation.X AndAlso m.Location.Y = ToLocation.Y Then
                MonsterFound = True
                ReturnValue = Player.PlayerMoveResult.Combat
                Exit For
            End If
        Next

        ' No monster 
        ' Check for collision with map feature
        If Not MonsterFound Then
            Select Case MapTile.GetTile(ToLocation).TileType
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
        If Not MapTile.GetTile(Location).IsRevealed Then
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

    Public Sub ViewportPlayerMoveProcess()
        Debug.WriteLine(String.Format("PlayerDraw:{0},{1}", Player1.Location.X, Player1.Location.Y))
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

    Public Sub ViewportArtifactsDraw()

        ' get the list of visible cells once so we don't have to recalculate repeatedly
        Dim VisibleCells As List(Of Point) = VisibleCellsGet(Player1.Location, Player1.VisualRange)

        For Each a As ItemBase In Items
            ' Is the artifact's location even on the screen?
            If IsLocationInViewport(a.Location) Then

                ' make the artifact invisible, and only make it visible if it's in the list of visible cells
                a.IsVisible = False
                If VisibleCells.Contains(a.Location) Then
                    a.Draw()
                Else
                    MaptileRender(a.Location)
                End If
            End If
        Next

    End Sub

    Public Sub ViewportCreaturesDraw()

        ' get the list of visible cells once so we don't have to recalculate repeatedly
        Dim VisibleCells As List(Of Point) = VisibleCellsGet(Player1.Location, Player1.VisualRange)

        For Each m As CreatureBase In Creatures
            ' Is the monster's location even on the screen?
            If IsLocationInViewport(m.Location) Then

                ' make the creature invisible, and only make it visible if it's in the list of visible cells
                m.IsVisible = False
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

    Public Sub ViewportClear()
        For row = MESSAGEAREAHEIGHT + 1 To Console.WindowHeight - STATUSAREAHEIGHT - 2
            Console.SetCursorPosition(0, row)
            Console.Write(Space(Console.WindowWidth - 1))
        Next
    End Sub

#End Region

End Module
