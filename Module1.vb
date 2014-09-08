Module Module1

    Const TESTMAPLOCATION As String = "C:\Users\Duane\Documents\GitHub\Glyphica\Map Files\"



    Dim map As New List(Of String)
    Dim Player1 As Player

    Public Sub Main()
        Console.CursorVisible = False
        Console.WindowWidth = 80
        Console.WindowHeight = 40
        Console.SetBufferSize(80, 40)
        Player1 = New Player
        Player1.CurrentPosition = (New Coordinate(3, 3))

        Dim vp As New ViewPort(60, 35)

        ' load the amap
        Using sr As New System.IO.StreamReader(TESTMAPLOCATION & "testmap1.txt")
            Dim Line As String = sr.ReadLine
            Do While Line IsNot Nothing
                map.Add(Line)
                Line = sr.ReadLine()
            Loop
        End Using

        DrawMap()
        vp.BorderDraw()


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

            MovesProcess()

        Loop While KeyPress.Key <> ConsoleKey.X



    End Sub

    Public Sub MovesProcess()
        If Player1.MoveAttempt() = Player.PlayerMoveResult.Move Then
            Player1.CurrentPosition = Player1.TargetPosition
        End If
    End Sub


    '  current "position values" are screen and not absolute map positions.
    '  must fix




    Public Sub DrawMap()
        Console.Clear()
        For Each mr As String In map
            Console.WriteLine(mr)
        Next

        Console.CursorTop = PlayerPosition.Top
        Console.CursorLeft = PlayerPosition.Left
        Console.Write("@")
    End Sub

    Public Sub ViewportBorderDraw(Height As Integer, width As Integer)
    End Sub









End Module
