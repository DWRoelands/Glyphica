Module Inventory

    Const ITEMNAMESTART As Integer = 4
    Const ARMORDAMAGESTART As Integer = 30
    Const WEIGHTSTART As Integer = 50
    Const VALUESTART As Integer = 65
    Const DESCSTART As Integer = 80

    Public Sub InventoryManage()

        Dim DescriptionWidth As Integer = Console.WindowWidth - DESCSTART - 5

        ViewportClear()

        Dim x As Integer = ITEMNAMESTART
        Dim y As Integer = MESSAGEAREAHEIGHT + 1
        Console.SetCursorPosition(x, y)
        Console.Write("Name")

        x = ARMORDAMAGESTART
        Console.SetCursorPosition(x, y)
        Console.Write("Armor/Damage")

        x = WEIGHTSTART
        Console.SetCursorPosition(x, y)
        Console.Write("Weight")

        x = VALUESTART
        Console.SetCursorPosition(x, y)
        Console.Write("Value")

        x = DESCSTART
        Console.SetCursorPosition(x, y)
        Console.Write("Description")


        x = Console.WindowWidth - 20
        y = Console.WindowHeight - STATUSAREAHEIGHT - 2
        Console.SetCursorPosition(x, y)
        Console.Write("Press ESCAPE to exit")

        Dim ListRange As New Point(0, Console.WindowHeight - STATUSAREAHEIGHT - MESSAGEAREAHEIGHT - 4)

        Dim SortedInventory As List(Of ItemBase) = (From InventoryItem As ItemBase In Player1.Inventory Order By InventoryItem.Name).ToList
        If SortedInventory.Count > 0 Then
            SortedInventory(0).IsSelected = True
        End If

        Do
            Dim ScreenRow As Integer = MESSAGEAREAHEIGHT + 2
            For ItemIndex As Integer = 0 To SortedInventory.Count - 1
                If IsBetween(ItemIndex, ListRange.X, ListRange.Y) Then

                    ' Draw or clear the "equipped" indicator
                    Console.SetCursorPosition(ITEMNAMESTART - 1, ScreenRow)
                    If SortedInventory(ItemIndex).IsEquipped Then
                        GraphicsCharacterDraw(EQUIPPEDINDICATOR)
                    Else
                        Console.Write(" ")
                    End If

                    ' item name
                    Dim NameText As String = SortedInventory(ItemIndex).Name & Space(ARMORDAMAGESTART - ITEMNAMESTART - SortedInventory(ItemIndex).Name.Length)

                    ' item armor/damage
                    Dim ArmorDamageText As String = String.Empty
                    Select Case SortedInventory(ItemIndex).GetType.BaseType
                        Case GetType(ArmorBase)
                            ArmorDamageText = CType(SortedInventory(ItemIndex), ArmorBase).ArmorBonus
                        Case GetType(WeaponBase)
                            ArmorDamageText = CType(SortedInventory(ItemIndex), WeaponBase).Damage
                    End Select
                    ArmorDamageText += Space(WEIGHTSTART - ARMORDAMAGESTART - ArmorDamageText.Length)

                    ' item weight
                    Dim WeightText As String = SortedInventory(ItemIndex).Weight & Space(VALUESTART - WEIGHTSTART - SortedInventory(ItemIndex).Weight.ToString.Length)

                    ' item value
                    Dim ValueText As String = SortedInventory(ItemIndex).Value & Space(DESCSTART - 1 - VALUESTART - SortedInventory(ItemIndex).Value.ToString.Length)

                    ' Clear the description
                    For DescLine As Integer = MESSAGEAREAHEIGHT + 2 To Console.WindowHeight - STATUSAREAHEIGHT - MESSAGEAREAHEIGHT
                        Console.SetCursorPosition(DESCSTART, DescLine)
                        Console.Write(Space(DescriptionWidth))
                    Next

                    If SortedInventory(ItemIndex).IsSelected Then
                        Console.ForegroundColor = ConsoleColor.Black
                        Console.BackgroundColor = ConsoleColor.White
                    End If

                    Console.SetCursorPosition(ITEMNAMESTART, ScreenRow)
                    Console.Write(NameText)
                    Console.Write(ArmorDamageText)
                    Console.Write(WeightText)
                    Console.Write(ValueText)

                    Console.ForegroundColor = ConsoleColor.White
                    Console.BackgroundColor = ConsoleColor.Black

                    ScreenRow += 1
                End If
            Next

            ' display the description of the selected item
            For Each InventoryItem In SortedInventory
                If InventoryItem.IsSelected Then
                    Dim DescriptionText As List(Of String) = Utility.WordWrap(InventoryItem.Description, DescriptionWidth)
                    For DescLine As Integer = 0 To DescriptionText.Count - 1
                        Console.SetCursorPosition(DESCSTART, DescLine + MESSAGEAREAHEIGHT + 2)
                        Console.Write(DescriptionText(DescLine))
                    Next
                End If
            Next

            Select Case Console.ReadKey(True).Key

                Case ConsoleKey.DownArrow

                    For Each InventoryItem In SortedInventory
                        If InventoryItem.IsSelected And SortedInventory.IndexOf(InventoryItem) < SortedInventory.Count - 1 Then
                            InventoryItem.IsSelected = False
                            SortedInventory(SortedInventory.IndexOf(InventoryItem) + 1).IsSelected = True

                            ' scroll the list down if necessary
                            If SortedInventory.IndexOf(InventoryItem) = ListRange.Y And ListRange.Y < SortedInventory.Count - 1 Then
                                ListRange.X += 1
                                ListRange.Y += 1
                            End If
                            Continue Do
                        End If
                    Next

                Case ConsoleKey.UpArrow
                    For Each InventoryItem In SortedInventory
                        If InventoryItem.IsSelected And SortedInventory.IndexOf(InventoryItem) > 0 Then
                            InventoryItem.IsSelected = False
                            SortedInventory(SortedInventory.IndexOf(InventoryItem) - 1).IsSelected = True

                            'scroll the list up if necessary
                            If SortedInventory.IndexOf(InventoryItem) = ListRange.X And ListRange.X > 0 Then
                                ListRange.X -= 1
                                ListRange.Y -= 1
                            End If
                            Continue Do
                        End If
                    Next

                Case ConsoleKey.Enter
                    For Each InventoryItem In SortedInventory
                        If InventoryItem.IsSelected Then
                            If InventoryItem.IsEquipped Then
                                InventoryItem.IsEquipped = False
                                'Continue Do
                            Else
                                If InventoryItem.IsEquippable Then
                                    ' certain item types only allow you to equip one of that type
                                    ' here, we unequip any other item of that type before equipping the selected item
                                    Select Case InventoryItem.GetType.BaseType
                                        Case GetType(ArmorBase)
                                            For Each ArmorItem As ItemBase In SortedInventory.Where(Function(a) a.GetType.BaseType = GetType(ArmorBase))
                                                ArmorItem.IsEquipped = False
                                            Next
                                        Case GetType(WeaponBase)
                                            For Each WeaponItem As ItemBase In SortedInventory.Where(Function(w) w.GetType.BaseType = GetType(WeaponBase))
                                                WeaponItem.IsEquipped = False
                                            Next
                                    End Select
                                    InventoryItem.IsEquipped = True
                                End If
                            End If
                        End If
                    Next
                    CreatureBase.ItemEffectsProcess(Player1)
                    StatusUpdate()


                Case ConsoleKey.Escape
                    Exit Do
            End Select
        Loop

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
