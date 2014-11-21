Public Class Main

    Public Player1 As Player
    Public vp As Viewport
    Public Creatures As List(Of CreatureBase)
    Public Items As List(Of ItemBase)
    Public Bitmaps As Hashtable
    Public Map As MapManager

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles Me.Load
        BitmapsLoad()
        Initialize()
    End Sub

    Private Sub Initialize()

        'configure the form
        Me.WindowState = FormWindowState.Maximized

        Map = New MapManager
        Map.Load()
        Creatures = New List(Of CreatureBase)
        Items = New List(Of ItemBase)
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
        Player1.Draw()

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

        ' Draw visible tiles
        Dim DrawingX As Integer = DrawingOriginGet.X
        For x As Integer = vp.Origin.X To vp.Origin.X + vp.Dimensions.Width - 1
            Dim DrawingY As Integer = DrawingOriginGet.Y
            For y As Integer = vp.Origin.Y To vp.Origin.Y + vp.Dimensions.Height - 2
                If Me.Map.TileGet(Player1.MapLevel, x, y).IsRevealed Then
                    Debug.WriteLine(MapTile.BitmapIdGet(x, y) & ", " & DrawingX & ", " & DrawingY)
                    e.Graphics.DrawImage(CType(Bitmaps(MapTile.BitmapIdGet(x, y)), Bitmap), DrawingX, DrawingY, SPRITESIZE, SPRITESIZE)
                Else
                    e.Graphics.DrawImage(CType(Bitmaps(MapTile.BitmapId.Empty), Bitmap), DrawingX, DrawingY, SPRITESIZE, SPRITESIZE)
                End If
                DrawingY += SPRITESIZE - 1
            Next
            DrawingX += SPRITESIZE - 1
        Next

        'draw the player
        Dim PlayerX = (Me.Player1.Location.X - Me.vp.Origin.X) * (SPRITESIZE - 1)
        Dim PlayerY = (Me.Player1.Location.Y - Me.vp.Origin.Y) * (SPRITESIZE - 1)
        e.Graphics.DrawImage(CType(Bitmaps(MapTile.BitmapId.Player), Bitmap), PlayerX, PlayerY, SPRITESIZE, SPRITESIZE)






        'Console.SetCursorPosition(Main.Player1.Location.X - Main.vp.Origin.X, Main.Player1.Location.Y - Main.vp.Origin.Y)



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

        Bitmaps.Add(MapTile.BitmapId.WallUpperLeft, SpriteGet(My.Resources.Wall, 0, 12))
        Bitmaps.Add(MapTile.BitmapId.WallVertical, SpriteGet(My.Resources.Wall, 0, 13))
        Bitmaps.Add(MapTile.BitmapId.WallLowerLeft, SpriteGet(My.Resources.Wall, 0, 14))
        Bitmaps.Add(MapTile.BitmapId.WallHorizontal, SpriteGet(My.Resources.Wall, 1, 12))
        Bitmaps.Add(MapTile.BitmapId.WallUpperRight, SpriteGet(My.Resources.Wall, 2, 9))
        Bitmaps.Add(MapTile.BitmapId.WallLowerRight, SpriteGet(My.Resources.Wall, 2, 11))
        Bitmaps.Add(MapTile.BitmapId.Floor, SpriteGet(My.Resources.Wall, 3, 12))
        Bitmaps.Add(MapTile.BitmapId.Door, SpriteGet(My.Resources.Door0, 0, 0))
        Bitmaps.Add(MapTile.BitmapId.Player, SpriteGet(My.Resources.Player0, 1, 1))

    End Sub

    Private Function SpriteGet(Source As Bitmap, x As Integer, y As Integer) As Bitmap
        Dim bmp As New Bitmap(16, 16)
        Dim g As Graphics = Graphics.FromImage(bmp)
        g.DrawImage(Source, 0, 0, New Rectangle(New Point(x * 16, y * 16), New Size(16, 16)), GraphicsUnit.Pixel)
        Return bmp
    End Function

End Class
