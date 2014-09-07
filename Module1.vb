Module Module1

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
    Dim map As New List(Of String)
    Dim PlayerPosition As Coordinate

    Sub Main()
        Console.CursorVisible = False
        Console.WindowWidth = 80
        Console.WindowHeight = 40




        Using sr As New System.IO.StreamReader("C:\Users\droelands\Documents\Visual Studio 2010\Projects\Glyphica\Glyphica\Map files\testmap1.txt")
            Dim Line As String = sr.ReadLine
            Do While Line IsNot Nothing
                map.Add(Line)
                Line = sr.ReadLine()
            Loop
        End Using

        PlayerPosition = New Coordinate(3, 3)
        DrawMap()
        DrawViewportBorder(60, 30)


        Dim KeyPress As ConsoleKeyInfo
        Do
            Dim targetposition As New Coordinate
            KeyPress = Console.ReadKey
            Select Case KeyPress.Key
                Case ConsoleKey.DownArrow
                    targetposition = New Coordinate(PlayerPosition.Top + 1, PlayerPosition.Left)

                Case ConsoleKey.UpArrow
                    targetposition = New Coordinate(PlayerPosition.Top - 1, PlayerPosition.Left)

                Case ConsoleKey.LeftArrow
                    targetposition = New Coordinate(PlayerPosition.Top, PlayerPosition.Left - 1)

                Case ConsoleKey.RightArrow
                    targetposition = New Coordinate(PlayerPosition.Top, PlayerPosition.Left + 1)

            End Select

            ProcessPlayerMove(PlayerPosition, targetposition)

            DrawMap()
        Loop While KeyPress.Key <> ConsoleKey.X



    End Sub

    Public Sub ProcessPlayerMove(CurrentPosition, TargetPosition)
        ' What's in the target position?
        Dim TargetContents As String = map(TargetPosition.top).Substring(TargetPosition.left, 1)

        Dim MoveResult As String = String.Empty

        Select Case TargetContents
            Case " "
                MoveResult = "MOVE"

            Case "#"
                MoveResult = "BLOCKED"
        End Select

        Select Case MoveResult
            Case "MOVE"
                PlayerPosition = TargetPosition

            Case "BLOCKED"
                '' nothing happens - you can't move here
        End Select

        If CurrentPosition.top <> TargetPosition.top And CurrentPosition.left <> TargetPosition.left Then
            playerposition = TargetPosition
        End If
    End Sub

    Public Sub DrawMap()
        Console.Clear()
        For Each mr As String In map
            Console.WriteLine(mr)
        Next

        Console.CursorTop = PlayerPosition.Top
        Console.CursorLeft = PlayerPosition.Left
        Console.Write("@")
    End Sub

    Public Sub DrawViewportBorder(Height As Integer, width As Integer)
        For x = 0 To Height - 1
            Console.SetCursorPosition(width, x)
            Console.Write(Chr(219))
        Next

        For y = 0 To width - 1
            Console.SetCursorPosition(y, y)
            Console.Write(Chr(219))
        Next






    End Sub





End Module
