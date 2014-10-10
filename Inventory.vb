Module Inventory

    Const FIRSTCOLUMNX As Integer = 2
    Const SECONDCOLUMNX As Integer = 40

    Public Sub InventoryManage()
        ViewportClear()

        Console.SetCursorPosition(FIRSTCOLUMNX, MESSAGEAREAHEIGHT + 1)
        Console.Write("EQUIPPED ITEMS")

        Console.SetCursorPosition(SECONDCOLUMNX, MESSAGEAREAHEIGHT + 1)
        Console.Write("CARRIED ITEMS")

        For x = 0 To Player1.Equipment.Count - 1
            Console.SetCursorPosition(FIRSTCOLUMNX, MESSAGEAREAHEIGHT + 2 + x)
            Console.Write(Player1.Equipment(x).Name)
        Next







    End Sub

    Private Sub EquippedItemsList()

    End Sub





End Module
