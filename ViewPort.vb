Public Class ViewPort
    Public Property Height As Integer
    Public Property Width As Integer

    Public Sub New(ViewPortHeight As Integer, ViewPortWidth As Integer)
        Height = ViewPortHeight
        Width = ViewPortWidth
    End Sub

    Public Sub BorderDraw()
        For x = 0 To Height - 1
            SolidBlockDraw(New Coordinate(Width, x))
        Next

        For y = 0 To Width - 1
            SolidBlockDraw(New Coordinate(y, Height - 1))
        Next

    End Sub

    Private Sub SolidBlockDraw(Position As Coordinate)
        Dim SOLIDBLOCK As Byte = 219
        Dim c As Char = System.Text.Encoding.GetEncoding(437).GetChars(New Byte() {SOLIDBLOCK})(0)
        Console.SetCursorPosition(Position.Left, Position.Top)
        Console.Write(c)
    End Sub

End Class
