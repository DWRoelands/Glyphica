Module Module1

    Const TESTMAPLOCATION As String = "C:\Users\Duane\Documents\GitHub\Glyphica\Map Files\"
    Dim Player1 As Player

    Public Sub Main()
        Console.CursorVisible = False
        Console.WindowWidth = 80
        Console.WindowHeight = 40
        Console.SetBufferSize(80, 40)
        Player1 = New Player
        Player1.CurrentPosition = (New Coordinate(3, 3))

        Dim vp As New ViewPort(35, 60)
        vp.MapLoad()
        vp.BorderDraw()
        vp.MapDraw()
        vp.OriginSet(New Coordinate(0, 0))

        Dim KeyPress As ConsoleKeyInfo
        Do
            Dim targetposition As New Coordinate
            KeyPress = Console.ReadKey
            Select Case KeyPress.Key
                Case ConsoleKey.DownArrow
                    Player1.TargetPosition = New Coordinate(Player1.CurrentPosition.Left, Player1.CurrentPosition.Top + 1)

                Case ConsoleKey.UpArrow
                    Player1.TargetPosition = New Coordinate(Player1.CurrentPosition.Left, Player1.CurrentPosition.Top - 1)

                Case ConsoleKey.LeftArrow
                    Player1.TargetPosition = New Coordinate(Player1.CurrentPosition.Left - 1, Player1.CurrentPosition.Top)

                Case ConsoleKey.RightArrow
                    Player1.TargetPosition = New Coordinate(Player1.CurrentPosition.Left + 1, Player1.CurrentPosition.Top)
            End Select

            If vp.PlayerMoveProcess(Player1) = Player.PlayerMoveResult.Move Then
                Player1.CurrentPosition = Player1.TargetPosition
            End If

        Loop While KeyPress.Key <> ConsoleKey.X

    End Sub



End Module
