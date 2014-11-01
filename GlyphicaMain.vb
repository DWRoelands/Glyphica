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
        Player1.Attributes.Add(New CreatureAttribute(CreatureAttribute.AttributeId.VisualRange, 8))
        Player1.Attributes.Add(New CreatureAttribute(CreatureAttribute.AttributeId.HitPoints_Base, Dice.RollDice("4d8")))
        Player1.Attributes.Add(New CreatureAttribute(CreatureAttribute.AttributeId.Damage_Base, "1d8"))
        Player1.Attributes.Add(New CreatureAttribute(CreatureAttribute.AttributeId.Initiative, 10))
        Player1.Attributes.Add(New CreatureAttribute(CreatureAttribute.AttributeId.ArmorClass, 5))



        Player1.MapLevel = 0
        Player1.Name = "Duane"




        ViewportSize = New Size(Console.WindowWidth, Console.WindowHeight - STATUSAREAHEIGHT)
        ViewportOrigin = New Point(0, 0)     ' The upper-left coordinate of the rectangular section of the map displayed in the viewport
        Viewport.BorderDraw()

        MapLoad()

        Dim C As New ChestSmall
        C.Location = New Point(20, 13)
        Items.Add(C)

        Player1.Location = New Point(13, 13)

        Viewport.MapScroll()
        Viewport.VisibleCellsProcess()
        Viewport.CreaturesDraw()
        Viewport.ItemsDraw()
        Player1.Draw()

        Trace.Listeners.Add(New GlyphicaTraceListener)

        ' MAIN GAME LOOP
        Dim KeyPress As ConsoleKeyInfo
        Do
            Viewport.StatusUpdate()

            Dim ToLocation As New Point
            KeyPress = Console.ReadKey(True)

            Select Case KeyPress.Key

                Case ConsoleKey.I
                    InventoryManage(Player1)

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

                    If Player1.EquippedWeapon IsNot Nothing AndAlso Player1.EquippedWeapon.Tier = WeaponBase.WeaponType.Ranged Then
                        If Player1.EquippedAmmunition IsNot Nothing AndAlso Player1.EquippedAmmunition.Type = Player1.EquippedWeapon.AmmunitionType Then
                            If KeyPress.Modifiers And ConsoleModifiers.Shift Then
                                ' allow player to choose target
                                Dim Target As CreatureBase = TargetSelect(Player1.Location)
                                If Target IsNot Nothing Then
                                    CombatResolve(Target, CombatType.Ranged)
                                Else
                                    Viewport.CreaturesDraw()
                                    Viewport.ItemsDraw()
                                    Player1.Draw()
                                    Continue Do
                                End If
                            Else
                                ' lowercase "s" automatically target the closest creature
                                Dim ClosestCreature As CreatureBase = Player1.FindClosest
                                If ClosestCreature IsNot Nothing AndAlso ClosestCreature.IsVisible Then
                                    CombatResolve(Player1.FindClosest(), CombatType.Ranged)
                                Else
                                    Viewport.MessageWrite("There is nothing to shoot here.")
                                End If
                            End If

                            Viewport.CreaturesDraw()
                            Viewport.ItemsDraw()
                            Player1.Draw()
                        Else
                            Viewport.MessageWrite("You do not have the right kind of ammunition equipped!")
                        End If
                    Else
                        Viewport.MessageWrite("You do not have a ranged weapon equipped!")
                    End If



                    ' jump to the bottom of the loop as there is no "move" to process
                    Continue Do

            End Select

            Select Case KeyPress.KeyChar
                Case ">"c  '' go down stairs
                    If MapTile.GetTile(Player1.Location).TileType = MapTile.MapTileType.StairsDown Then
                        Player1.MapLevel += 1
                        Viewport.MapDraw()
                        Player1.Location = StairsUpFind()
                    End If

                Case "<"c  '' go up stairs
                    If MapTile.GetTile(Player1.Location).TileType = MapTile.MapTileType.StairsUp Then
                        Player1.MapLevel -= 1
                        Viewport.MapDraw()
                        Player1.Location = StairsDownFind()
                    End If

            End Select

            Player1.PreMoveProcess(ToLocation)
            Viewport.Refresh()
            Player1.PostMoveProcess()

        Loop While KeyPress.Key <> ConsoleKey.X And Player1.AttributeGet(CreatureAttribute.AttributeId.HitPoints) > 0

        Viewport.StatusUpdate()

        If Player1.AttributeGet(CreatureAttribute.AttributeId.HitPoints) <= 0 Then
            Viewport.MessageWrite("You have died.")
            Viewport.MessageWrite("Press any key to exit.")
            WaitForKeypress()
        End If

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
        Viewport.MessageWrite("Target which creature?  (ESC to cancel)")
        For Each m As CreatureBase In VisibleCreatures
            Viewport.MessageWrite(String.Format("{0} - {1}", TargetNumber, m.Name))
            TargetNumber += 1
        Next

        Dim TargetKey As ConsoleKeyInfo
        TargetNumber = 0
        Do
            TargetKey = Console.ReadKey(True)
        Loop Until TargetKey.Key = ConsoleKey.Escape Or Integer.TryParse(TargetKey.KeyChar, TargetNumber)

        ' ESC - user has cancelled
        If TargetKey.Key = ConsoleKey.Escape Then
            Viewport.MessageWrite("Cancelled")
            Return Nothing
        End If

        If TargetNumber > VisibleCreatures.Count Then
            Viewport.MessageWrite("Invalid target")
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
            PlayerInitiative = Dice.RollDice("1d20") + Player1.AttributeGet(CreatureAttribute.AttributeId.Initiative) + 20
            EnemyInitiative = Dice.RollDice("1d20") + Enemy.AttributeGet(CreatureAttribute.AttributeId.Initiative)
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
        Trace.Write(String.Format("Attacker H:{0} AC:{1}", AttackerRoll, Defender.AttributeGet(CreatureAttribute.AttributeId.ArmorClass)))
        If AttackerRoll >= Defender.AttributeGet(CreatureAttribute.AttributeId.ArmorClass) Then
            Dim Damage As Integer = Dice.RollDice(Attacker.DamageDice)
            If Attacker Is Player1 Then
                Viewport.MessageWrite(String.Format("You hit the {0} for {1} damage.", Defender.Name, Damage))
            Else
                Viewport.MessageWrite(String.Format("The {0} hit you for {1} damage.", Attacker.Name, Damage))
            End If
            Defender.HitPointsCurrent -= Damage
        Else
            If Attacker Is Player1 Then
                Viewport.MessageWrite(String.Format("You missed the {0}.", Defender.Name))
            Else
                Viewport.MessageWrite(String.Format("The {0} missed.", Attacker.Name))
            End If
        End If

        ' did the defender die?
        If Defender.HitPointsCurrent <= 0 Then
            If Defender Is Player1 Then
                Exit Sub
            Else
                Viewport.MessageWrite(String.Format("You killed the {0}!", Defender.Name))
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
                    Viewport.MessageWrite(String.Format("You hit the {0} for {1} damage.", Attacker.Name, Damage))
                Else
                    Viewport.MessageWrite(String.Format("The {0} hit you for {1} damage.", Defender.Name, Damage))
                End If
                Attacker.HitPointsCurrent -= Damage
            Else
                If Defender Is Player1 Then
                    Viewport.MessageWrite(String.Format("You missed the {0}.", Attacker.Name))
                Else
                    Viewport.MessageWrite(String.Format("The {0} missed.", Defender.Name))
                End If
            End If

            ' did the attacker die
            If Attacker.HitPointsCurrent <= 0 Then
                If Attacker Is Player1 Then
                    Exit Sub
                Else
                    Viewport.MessageWrite(String.Format("You killed the {0}!", Defender.Name))
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

End Module
