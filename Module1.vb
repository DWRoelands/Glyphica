Module Module1

    Const TESTMAPLOCATION As String = "C:\Users\Duane\Documents\GitHub\Glyphica\Map Files\"
    Dim Player1 As Player
    Dim vp As ViewPort

    Public Sub Main()
        Console.CursorVisible = False
        Console.WindowWidth = 80
        Console.WindowHeight = 40
        Console.SetBufferSize(80, 40)
        Player1 = New Player

        vp = New ViewPort(30, 30)
        vp.MapLoad()
        vp.BorderDraw()
        vp.MapDraw()
        vp.OriginSet(New Coordinate(0, 0))

        Player1.CurrentLocation = New Coordinate(vp.ViewPortWidth / 2, vp.ViewPortHeight / 2)

        Console.SetCursorPosition(Player1.CurrentLocation.Left, Player1.CurrentLocation.Top)
        Console.Write("@")

        Dim KeyPress As ConsoleKeyInfo
        Do
            Dim TargetLocation As New Coordinate
            KeyPress = Console.ReadKey(True)

            Select Case KeyPress.Key
                Case ConsoleKey.DownArrow, ConsoleKey.NumPad2
                    TargetLocation = New Coordinate(Player1.CurrentLocation.Left, Player1.CurrentLocation.Top + 1)

                Case ConsoleKey.UpArrow, ConsoleKey.NumPad8
                    TargetLocation = New Coordinate(Player1.CurrentLocation.Left, Player1.CurrentLocation.Top - 1)

                Case ConsoleKey.LeftArrow, ConsoleKey.NumPad4
                    TargetLocation = New Coordinate(Player1.CurrentLocation.Left - 1, Player1.CurrentLocation.Top)

                Case ConsoleKey.RightArrow, ConsoleKey.NumPad6
                    TargetLocation = New Coordinate(Player1.CurrentLocation.Left + 1, Player1.CurrentLocation.Top)

                Case ConsoleKey.NumPad7
                    TargetLocation = New Coordinate(Player1.CurrentLocation.Left - 1, Player1.CurrentLocation.Top - 1)

                Case ConsoleKey.NumPad9
                    TargetLocation = New Coordinate(Player1.CurrentLocation.Left + 1, Player1.CurrentLocation.Top - 1)

                Case ConsoleKey.NumPad1
                    TargetLocation = New Coordinate(Player1.CurrentLocation.Left - 1, Player1.CurrentLocation.Top + 1)

                Case ConsoleKey.NumPad3
                    TargetLocation = New Coordinate(Player1.CurrentLocation.Left + 1, Player1.CurrentLocation.Top + 1)
            End Select

            Select Case KeyPress.KeyChar
                Case ">"c  '' go down stairs
                    If vp.LocationGet(Player1.CurrentLocation) = ">" Then
                        vp.MapLevel += 1
                        vp.MapDraw()
                        PlayerMove(vp.StairsUpFind())
                    End If

                Case "<"c  '' go up stairs
                    If vp.LocationGet(Player1.CurrentLocation) = "<" Then
                        vp.MapLevel -= 1
                        vp.MapDraw()
                        PlayerMove(vp.StairsDownFind())
                    End If

            End Select

            Select Case Player1.MoveProcess(vp.LocationGet(TargetLocation))
                Case Player.PlayerMoveResult.Move
                    PlayerMove(TargetLocation)
            End Select

        Loop While KeyPress.Key <> ConsoleKey.X

    End Sub

    Public Sub PlayerMove(TargetLocation)
        vp.LocationClear(Player1.CurrentLocation)
        Player1.MoveTo(TargetLocation)
        vp.PlayerDraw(Player1)
    End Sub




End Module
