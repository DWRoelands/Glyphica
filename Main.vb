Public Class Main

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles Me.Load
        Initialize()
    End Sub

    Private Sub Initialize()

        'configure the form
        Me.WindowState = FormWindowState.Maximized

        Creatures = New List(Of CreatureBase)
        Items = New List(Of ItemBase)
        Player1 = New Player

        With Player1
            .MapLevel = 0
            .Name = "Duane"
            .Location = New Point(13, 13)
        End With

        vp = New Viewport
        vp.Padding = 20
        vp.Dimensions = New Size((Me.Width - (vp.Padding * 2)) / SPRITESIZE, (Me.Height - Me.rtbMessages.Height - (vp.Padding * 2)) / SPRITESIZE)
        vp.Origin = New Point(0, 0)
        vp.ScrollBuffer = New Point(vp.Dimensions.Width / 5, vp.Dimensions.Height / 5)

        vp.Scroll()
        vp.VisibleCellsProcess()
        vp.CreaturesDraw()
        vp.ItemsDraw()
        Player1.Draw()

    End Sub

    Private Sub MapLoad()

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

    Public Sub MessageWrite(Message As String)
        MessageWrite(Message, Color.Black)
    End Sub

    Public Sub MessageWrite(Message As String, MessageColor As Color)
        With rtbMessages
            .SelectionColor = MessageColor
            .AppendText(Message)
            .SelectionStart = rtbMessages.TextLength
            .ScrollToCaret()
        End With
    End Sub
End Class
