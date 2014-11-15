Public Class Main

    Public Player1 As Player
    Public vp As Viewport
    Public Creatures As List(Of CreatureBase)
    Public Items As List(Of ItemBase)
    Public Map(,,) As MapTile
    Public Bitmaps As Hashtable

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles Me.Load
        BitmapsLoad()
        MapLoad()
        Initialize()
    End Sub

    Private Sub Initialize()

        'configure the form
        Me.WindowState = FormWindowState.Maximized

        Creatures = New List(Of CreatureBase)
        Items = New List(Of ItemBase)
        Bitmaps = New Hashtable
        Player1 = New Player

        With Player1
            .MapLevel = 0
            .Name = "Duane"
            .Location = New Point(13, 13)
            .Attributes.Add(New CreatureAttribute(CreatureAttribute.AttributeId.VisualRange, 8))
        End With

        vp = New Viewport
        vp.Padding = 20
        vp.Dimensions = New Size((Me.Width - (vp.Padding * 2)) / SPRITESIZE, (Me.Height - Me.rtbMessages.Height - (vp.Padding * 2)) / SPRITESIZE)
        vp.Origin = New Point(0, 0)
        vp.ScrollBuffer = New Point(vp.Dimensions.Width / 5, vp.Dimensions.Height / 5)

        vp.Scroll()
        vp.VisibleCellsProcess()
        'vp.CreaturesDraw()
        'vp.ItemsDraw()
        'Player1.Draw()

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

        ReDim Me.Map(Player1.MapLevel, MapLines(0).Length - 1, MapLines.Count - 1)

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


    Public Function DrawingOriginGet() As Point
        Return New Point(0, 0 + Me.rtbMessages.Height)
    End Function

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim DrawingX As Integer = DrawingOriginGet.X
        Dim DrawingY As Integer = DrawingOriginGet.Y

        For x As Integer = vp.Origin.X To vp.Origin.X + vp.Dimensions.Width - 1
            For y As Integer = vp.Origin.Y To vp.Origin.Y + vp.Dimensions.Height - 2
                If Map(Player1.MapLevel, x, y).IsRevealed Then
                    e.Graphics.DrawImage(CType(Bitmaps(MapTile.BitmapIdGet(x, y)), Bitmap), DrawingX, DrawingY, SPRITESIZE, SPRITESIZE)
                End If
            Next
        Next

        Dim b As Bitmap = CType(Bitmaps(MapTile.BitmapId.WallVertical), Bitmap)
        e.Graphics.DrawImage(b, 200, 200, SPRITESIZE, SPRITESIZE)



    End Sub

    Private Sub BitmapsLoad()

        Bitmaps = New Hashtable

        ' create an empty squaye bitmap
        Using b As New SolidBrush(Color.Black)
            Dim bmp As New Bitmap(16, 16)
            Dim g As Graphics = Graphics.FromImage(bmp)
            g.FillRectangle(b, New Rectangle(0, 0, 16, 16))
            Bitmaps.Add(MapTile.BitmapId.Empty, bmp)
        End Using

        Bitmaps.Add(MapTile.BitmapId.WallUpperLeft, SpriteGet(WALLS, 0, 9))
        Bitmaps.Add(MapTile.BitmapId.WallVertical, SpriteGet(WALLS, 0, 10))
        Bitmaps.Add(MapTile.BitmapId.WallLowerLeft, SpriteGet(WALLS, 0, 11))
        Bitmaps.Add(MapTile.BitmapId.WallHorizontal, SpriteGet(WALLS, 1, 9))
        Bitmaps.Add(MapTile.BitmapId.WallUpperRight, SpriteGet(WALLS, 2, 9))
        Bitmaps.Add(MapTile.BitmapId.WallLowerRight, SpriteGet(WALLS, 2, 11))
        Bitmaps.Add(MapTile.BitmapId.Floor, SpriteGet(WALLS, 3, 3))
        Bitmaps.Add(MapTile.BitmapId.Door, SpriteGet(DOORS, 0, 0))

    End Sub

    Private Function SpriteGet(Filename As String, x As Integer, y As Integer) As Bitmap
        Dim bmp As New Bitmap(16, 16)
        Dim g As Graphics = Graphics.FromImage(bmp)
        g.DrawImage(New Bitmap(Filename), 0, 0, New Rectangle(New Point(x * 16, y * 16), New Size(16, 16)), GraphicsUnit.Pixel)
        Return bmp
    End Function

End Class
