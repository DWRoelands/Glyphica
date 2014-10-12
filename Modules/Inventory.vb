Module Inventory

    Const FIRSTCOLUMNX As Integer = 2
    Const SECONDCOLUMNX As Integer = 40

    Dim ActiveColumn As InventoryColumn

    Private Enum InventoryColumn
        Equipped
        Carried
    End Enum


    Public Sub InventoryManage()
        ViewportClear()

        Console.SetCursorPosition(FIRSTCOLUMNX, MESSAGEAREAHEIGHT + 1)
        Console.Write("EQUIPPED ITEMS")

        Console.SetCursorPosition(SECONDCOLUMNX, MESSAGEAREAHEIGHT + 1)
        Console.Write("CARRIED ITEMS")

        Dim EquippedItems = (From InventoryItem In Player1.Inventory Where InventoryItem.IsEquipped).Count
        Dim CarriredItems = (From InventoryItem In Player1.Inventory Where Not InventoryItem.IsEquipped).Count
        If EquippedItems > 0 Then
            ActiveColumn = InventoryColumn.Equipped
        ElseIf CarriredItems > 0 Then
            ActiveColumn = InventoryColumn.Carried
        Else
            CenterMessage("You aren't carrying any items.")
            Console.ReadKey(True)
            ViewportClear()
        End If

        EquippedItemsList(0)
        CarriedItemsList(0)




        Console.ReadKey(True)
        ViewportClear()

    End Sub

    Private Sub EquippedItemsClear()
        For y = MESSAGEAREAHEIGHT + 2 To Console.WindowHeight - STATUSAREAHEIGHT - 2
            Console.SetCursorPosition(FIRSTCOLUMNX, y)
            Console.Write(Space(SECONDCOLUMNX - FIRSTCOLUMNX))
        Next
    End Sub

    Private Sub EquippedItemsList(StartAtItem As Integer)
        Dim y As Integer = 0
        For Each InventoryItem As ItemBase In Player1.Inventory.Where(Function(x) x.IsEquipped)
            If y >= StartAtItem And y <= Console.WindowHeight - MESSAGEAREAHEIGHT - STATUSAREAHEIGHT Then
                If y = StartAtItem Then
                    InventoryItem.IsSelected = True
                End If

                Console.SetCursorPosition(FIRSTCOLUMNX, MESSAGEAREAHEIGHT + y + 2)
                If InventoryItem.IsSelected Then
                    Console.BackgroundColor = ConsoleColor.White
                    Console.ForegroundColor = ConsoleColor.Black
                End If
                Console.Write(InventoryItem.Name)
                Console.BackgroundColor = ConsoleColor.Black
                Console.ForegroundColor = ConsoleColor.White
            End If
            y += 1
        Next
    End Sub

    Private Sub CarriedItemsList(StartAtItem As Integer)
        Dim y As Integer = 0
        For Each InventoryItem As ItemBase In Player1.Inventory.Where(Function(x) x.IsEquipped = False)
            If y >= StartAtItem And y <= Console.WindowHeight - MESSAGEAREAHEIGHT - STATUSAREAHEIGHT - 4 Then
                Console.SetCursorPosition(SECONDCOLUMNX, MESSAGEAREAHEIGHT + y + 2)
                Console.Write(InventoryItem.Name)
            End If
            y += 1
        Next
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



End Module
