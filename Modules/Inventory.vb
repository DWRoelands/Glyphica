Module Inventory

    Const ITEMNAMESTART As Integer = 4

    Public Sub InventoryManage()
        ViewportClear()

        Dim ListRange As New Point(0, Console.WindowHeight - STATUSAREAHEIGHT - MESSAGEAREAHEIGHT)

        Dim SortedInventory = From InventoryItem As ItemBase In Player1.Inventory Order By InventoryItem.Name

        Dim ListStart As Integer = 0

        Do
            Dim ScreenRow As Integer = STATUSAREAHEIGHT + 2
            For ItemIndex As Integer = 0 To SortedInventory.Count - 1
                If IsBetween(ItemIndex, ListRange.X, ListRange.Y) Then
                    If SortedInventory(ItemIndex).IsEquipped Then
                        Console.SetCursorPosition(ITEMNAMESTART - 1, ScreenRow)
                        GraphicsCharacterDraw(EQUIPPEDINDICATOR)
                    End If
                    Console.SetCursorPosition(ITEMNAMESTART, ScreenRow)
                    If SortedInventory(ItemIndex).IsSelected Then
                        Console.ForegroundColor = ConsoleColor.Black
                        Console.BackgroundColor = ConsoleColor.White
                    End If
                    Console.Write(SortedInventory(ItemIndex).Name)
                    Console.ForegroundColor = ConsoleColor.White
                    Console.BackgroundColor = ConsoleColor.Black
                    ScreenRow += 1
                End If
            Next

            Select Case Console.ReadKey.Key
                Case ConsoleKey.DownArrow
                    For Each InventoryItem In SortedInventory
                        If InventoryItem.IsSelected Then
                            If Not (InventoryItem Is SortedInventory.Last) Then
                                SortedInventory.
                            End If
                        End If
                    Next



            End Select




        Loop


        Console.ReadLine()

        ViewportClear()

    End Sub


    Private Sub CenterMessage(Message As String)

        Dim y As Integer = Console.WindowHeight / 2
        Dim x As Integer = Console.WindowWidth / 2 - Message.Length / 2
        Console.SetCursorPosition(x, y)
        Console.Write(Message)
        Message = "Press any key"
        y = Console.WindowHeight / 2 + 1
        x = Console.WindowWidth / 2 - Message.Length / 2
        Console.SetCursorPosition(x, y)
        Console.Write(Message)

    End Sub

    'TODO: Move to utility module
    Private Function IsBetween(Value As Integer, RangeStart As Integer, RangeEnd As Integer) As Boolean
        Return (Value >= RangeStart And Value <= RangeEnd)
    End Function

End Module
