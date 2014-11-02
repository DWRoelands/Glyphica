Imports System.Math
Module Utility
    Public Function WordWrap(Message As String, LineLength As Integer) As List(Of String)
        Dim ReturnValue As New List(Of String)
        Dim Words() As String = Message.Split(" ")
        Dim NewLine As New System.Text.StringBuilder
        For Each Word As String In Words
            If NewLine.Length + 1 + Word.Length < LineLength Then
                NewLine.Append(Word & " ")
            Else
                ReturnValue.Add(NewLine.ToString)
                NewLine = New System.Text.StringBuilder
                NewLine.Append(Word)
            End If
        Next
        ReturnValue.Add(NewLine.ToString)
        Return ReturnValue
    End Function

    Public Function IsBetween(Value As Integer, RangeStart As Integer, RangeEnd As Integer) As Boolean
        Return (Value >= RangeStart And Value <= RangeEnd)
    End Function

    Public Sub GraphicsCharacterDraw(Character As Byte)
        Dim c As Char = System.Text.Encoding.GetEncoding(437).GetChars(New Byte() {Character})(0)
        Console.Write(c)
    End Sub

    Public Function DistanceGet(Location1 As Point, Location2 As Point) As Decimal
        Return Sqrt((Abs(Location2.X - Location1.X) ^ 2) + (Abs(Location2.Y - Location1.Y) ^ 2))
    End Function

    Public Function Ask(Question As String) As Boolean
        MessageWrite(Question)
        Dim Response As ConsoleKeyInfo = Nothing
        Do
            Response = Console.ReadKey(True)
        Loop Until Response.Key = ConsoleKey.Y Or Response.Key = ConsoleKey.N

        Console.WriteLine(IIf(Response.Key = ConsoleKey.Y, "Yes", "No"))

        Return Response.Key = ConsoleKey.Y
    End Function

    Public Sub RightJustifiedMessage(ScreenRow As Integer, Message As String)
        Dim x As Integer = Console.WindowWidth - Message.Length
        Console.SetCursorPosition(Console.WindowWidth - Message.Length - 1, ScreenRow)
        Console.Write(Message)
    End Sub

End Module


