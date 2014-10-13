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
End Module
