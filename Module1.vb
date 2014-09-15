Imports System.Drawing
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
        vp.OriginSet(New point(0, 0))

        Player1.Location = New Point(vp.ViewPortSize.Width / 2, vp.ViewPortSize.Height / 2)
        vp.PlayerDraw(Player1)

        Dim KeyPress As ConsoleKeyInfo
        Do
            Dim TargetLocation As New Point
            KeyPress = Console.ReadKey(True)

            Select Case KeyPress.Key
                Case ConsoleKey.DownArrow, ConsoleKey.NumPad2
                    TargetLocation = New Point(Player1.Location.X, Player1.Location.Y + 1)

                Case ConsoleKey.UpArrow, ConsoleKey.NumPad8
                    TargetLocation = New Point(Player1.Location.X, Player1.Location.Y - 1)

                Case ConsoleKey.LeftArrow, ConsoleKey.NumPad4
                    TargetLocation = New Point(Player1.Location.X - 1, Player1.Location.Y)

                Case ConsoleKey.RightArrow, ConsoleKey.NumPad6
                    TargetLocation = New Point(Player1.Location.X + 1, Player1.Location.Y)

                Case ConsoleKey.NumPad7
                    TargetLocation = New Point(Player1.Location.X - 1, Player1.Location.Y - 1)

                Case ConsoleKey.NumPad9
                    TargetLocation = New Point(Player1.Location.X + 1, Player1.Location.Y - 1)

                Case ConsoleKey.NumPad1
                    TargetLocation = New Point(Player1.Location.X - 1, Player1.Location.Y + 1)

                Case ConsoleKey.NumPad3
                    TargetLocation = New Point(Player1.Location.X + 1, Player1.Location.Y + 1)
            End Select

            Select Case KeyPress.KeyChar
                Case ">"c  '' go down stairs
                    If vp.LocationGet(Player1.Location).TileType = MapTile.MapTileType.StairsDown Then
                        vp.MapLevel += 1
                        vp.MapDraw()
                        PlayerMove(vp.StairsUpFind())
                    End If

                Case "<"c  '' go up stairs
                    If vp.LocationGet(Player1.Location).TileType = MapTile.MapTileType.StairsUp Then
                        vp.MapLevel -= 1
                        vp.MapDraw()
                        PlayerMove(vp.StairsDownFind())
                    End If

            End Select

            Select Case vp.PlayerMoveAttempt(Player1, TargetLocation)
                Case Player.PlayerMoveResult.Move
                    PlayerMove(TargetLocation)
                Case Player.PlayerMoveResult.Blocked
                    '' do nothing
            End Select

        Loop While KeyPress.Key <> ConsoleKey.X

    End Sub

    Public Sub PlayerMove(TargetLocation)
        vp.LocationClear(Player1.Location)
        Player1.MoveTo(TargetLocation)
        vp.PlayerDraw(Player1)
    End Sub




End Module
