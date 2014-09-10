Module Module1

    Const TESTMAPLOCATION As String = "C:\Users\Duane\Documents\GitHub\Glyphica\Map Files\"
    Dim Player1 As Player

    Public Sub Main()
        Console.CursorVisible = False
        Console.WindowWidth = 80
        Console.WindowHeight = 40
        Console.SetBufferSize(80, 40)
        Player1 = New Player
        Player1.CurrentLocation = (New Coordinate(3, 3))

        Dim vp As New ViewPort(30, 30)
        vp.MapLoad()
        vp.BorderDraw()
        vp.MapDraw()
        vp.OriginSet(New Coordinate(0, 0))

        Player1.CurrentLocation = New Coordinate(vp.Width / 2, vp.Height / 2)

        Console.SetCursorPosition(Player1.CurrentLocation.Left, Player1.CurrentLocation.Top)
        Console.Write("@")

        Dim KeyPress As ConsoleKeyInfo
        Do
            Dim TargetLocation As New Coordinate
            KeyPress = Console.ReadKey(True)
            Select Case KeyPress.Key
                Case ConsoleKey.DownArrow
                    TargetLocation = New Coordinate(Player1.CurrentLocation.Left, Player1.CurrentLocation.Top + 1)

                Case ConsoleKey.UpArrow
                    TargetLocation = New Coordinate(Player1.CurrentLocation.Left, Player1.CurrentLocation.Top - 1)

                Case ConsoleKey.LeftArrow
                    TargetLocation = New Coordinate(Player1.CurrentLocation.Left - 1, Player1.CurrentLocation.Top)

                Case ConsoleKey.RightArrow
                    TargetLocation = New Coordinate(Player1.CurrentLocation.Left + 1, Player1.CurrentLocation.Top)
            End Select

            Select Case Player1.MoveProcess(vp.LocationGet(TargetLocation))
                Case Player.PlayerMoveResult.Move
                    vp.LocationClear(Player1.CurrentLocation)
                    Player1.MoveTo(TargetLocation)
                    vp.PlayerDraw(Player1)
            End Select

        Loop While KeyPress.Key <> ConsoleKey.X

    End Sub


End Module
