Module Module1

    Const TESTMAPLOCATION As String = "C:\Users\Duane\Documents\GitHub\Glyphica\Map Files\"

    Private Enum PlayerMoveResult
        Blocked
        Move
    End Enum

    Private Class Coordinate
        Public Property Top As Integer
        Public Property Left As Integer
        Public Sub New(InitialTop As Integer, InitalLeft As Integer)
            Me.Top = InitialTop
            Me.Left = InitalLeft
        End Sub

        Public Sub New()

        End Sub

    End Class

    Private Class Player
        Public Property CurrentPosition As Coordinate
        Public Property TargetPosition As Coordinate



        Public Function MoveAttempt() As PlayerMoveResult

            Dim ReturnValue As PlayerMoveResult

            ' What's in the target position?
            Dim TargetContents As String = map(TargetPosition.top).Substring(TargetPosition.left, 1)

            Select Case TargetContents
                Case " "
                    ReturnValue = PlayerMoveResult.Move

                Case "#"
                    ReturnValue = PlayerMoveResult.Blocked
            End Select

            Return ReturnValue
        End Function
    End Class


    Dim map As New List(Of String)
    Dim Player1 As Player

    Sub Main()
        Console.CursorVisible = False
        Console.WindowWidth = 80
        Console.WindowHeight = 40
        Console.SetBufferSize(80, 40)
        Player1 = New Player
        Player1.CurrentPosition = (New Coordinate(3, 3))


        Using sr As New System.IO.StreamReader(TESTMAPLOCATION & "testmap1.txt")
            Dim Line As String = sr.ReadLine
            Do While Line IsNot Nothing
                map.Add(Line)
                Line = sr.ReadLine()
            Loop
        End Using

        DrawMap()
        ViewportBorderDraw(60, 30)


        Dim KeyPress As ConsoleKeyInfo
        Do
            Dim targetposition As New Coordinate
            KeyPress = Console.ReadKey
            Select Case KeyPress.Key
                Case ConsoleKey.DownArrow
                    Player1.TargetPosition = New Coordinate(Player1.CurrentPosition.Top + 1, Player1.CurrentPosition.Left)

                Case ConsoleKey.UpArrow
                    Player1.TargetPosition = New Coordinate(Player1.CurrentPosition.Top - 1, Player1.CurrentPosition.Left)

                Case ConsoleKey.LeftArrow
                    Player1.TargetPosition = New Coordinate(Player1.CurrentPosition.Top, Player1.CurrentPosition.Left - 1)

                Case ConsoleKey.RightArrow
                    Player1.TargetPosition = New Coordinate(Player1.CurrentPosition.Top, Player1.CurrentPosition.Left + 1)
            End Select

            MovesProcess()

        Loop While KeyPress.Key <> ConsoleKey.X



    End Sub

    Public Sub MovesProcess()
        If Player1.MoveAttempt = PlayerMoveResult.Move Then
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
        For x = 0 To Height - 1
            SolidBlockDraw(New Coordinate(width, x))
        Next

        For y = 0 To width - 1
            SolidBlockDraw(New Coordinate(y, Height - 1))
        Next
    End Sub

    Private Sub SolidBlockDraw(Position As Coordinate)
        Dim SOLIDBLOCK As Byte = 219
        Dim c As Char = System.Text.Encoding.GetEncoding(437).GetChars(New Byte() {SOLIDBLOCK})(0)
        Console.SetCursorPosition(Position.Left, Position.Top)
        Console.Write(c)
    End Sub



End Module
