Module Inventory

    Const FIRSTCOLUMNX As Integer = 2
    Const SECONDCOLUMNX As Integer = 40

    Public Sub InventoryManage()
        ViewportClear()

        Console.SetCursorPosition(FIRSTCOLUMNX, MESSAGEAREAHEIGHT + 1)
        Console.Write("EQUIPPED ITEMS")

        Console.SetCursorPosition(SECONDCOLUMNX, MESSAGEAREAHEIGHT + 1)
        Console.Write("CARRIED ITEMS")

        EquippedItemsList()
    End Sub

    Private Sub EquippedItemsClear()
        For y = MESSAGEAREAHEIGHT + 2 To Console.WindowHeight - STATUSAREAHEIGHT - 2
            Console.SetCursorPosition(FIRSTCOLUMNX, y)
            Console.Write(Space(SECONDCOLUMNX - FIRSTCOLUMNX))
        Next
    End Sub

    Private Sub EquippedItemsList()
        For x = 0 To Player1.Inventory.Count - 1
            Console.SetCursorPosition(FIRSTCOLUMNX, MESSAGEAREAHEIGHT + 2 + x)
            Console.Write(Player1.Inventory(x).Name)
        Next
    End Sub





End Module
