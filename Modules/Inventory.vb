Module Inventory

    '
    ' The concept for the inventory management page is completely stolen from the Skyrim mod "SkyUI"
    ' I have not yet encountered a better way to manage inventory in an RPG
    '

    Const ITEMNAMESTART As Integer = 4
    Const ARMORDAMAGESTART As Integer = 30
    Const WEIGHTSTART As Integer = 50
    Const VALUESTART As Integer = 65
    Const DESCSTART As Integer = 80

    Dim FilterNameRow As Integer
    Dim ListStartRow As Integer
    Dim ListLastRow As Integer
    Dim ListHeaderStart As Integer
    Dim SelectedFilter As InventoryFilterType

    Dim InventoryFilters() As String = {"All Items", "Armor", "Weapons", "Ammunition"}
    Dim SortedInventory As List(Of ItemBase) = Nothing

    Public Enum InventoryFilterType
        AllItems = 0
        Armor = 1
        Weapons = 2
        Ammunition = 3
    End Enum

    Private Sub FiltersDraw()

        Console.SetCursorPosition(4, FilterNameRow)
        For x = 0 To InventoryFilters.Length - 1
            If SelectedFilter = x Then
                Console.ForegroundColor = ConsoleColor.Black
                Console.BackgroundColor = ConsoleColor.White
            Else
                Console.ForegroundColor = ConsoleColor.White
                Console.BackgroundColor = ConsoleColor.Black
            End If
            Console.Write(InventoryFilters(x))

            Console.ForegroundColor = ConsoleColor.White
            Console.BackgroundColor = ConsoleColor.Black

            Console.Write("  ")
        Next

    End Sub

    Private Sub ClearList()
        For ScreenRow = ListStartRow To ListLastRow
            Console.SetCursorPosition(ITEMNAMESTART - 1, ScreenRow)
            Console.Write(Space(DESCSTART - ITEMNAMESTART - 1))
        Next

        For Each i As ItemBase In Player1.Inventory
            i.IsSelected = False
        Next
    End Sub

    Private Sub FilterApply()
        Select Case SelectedFilter
            Case InventoryFilterType.AllItems
                SortedInventory = (From InventoryItem As ItemBase In Player1.Inventory Order By InventoryItem.Name).ToList
            Case InventoryFilterType.Armor
                SortedInventory = (From InventoryItem As ItemBase In Player1.Inventory Where TypeOf InventoryItem Is ArmorBase Order By InventoryItem.Name).ToList
            Case InventoryFilterType.Weapons
                SortedInventory = (From InventoryItem As ItemBase In Player1.Inventory Where TypeOf InventoryItem Is WeaponBase Order By InventoryItem.Name).ToList
            Case InventoryFilterType.Ammunition
                SortedInventory = (From InventoryItem As ItemBase In Player1.Inventory Where TypeOf InventoryItem Is AmmunitionBase Order By InventoryItem.Name).ToList
        End Select

        If SortedInventory.Count > 0 Then
            SortedInventory(0).IsSelected = True
        End If
    End Sub

    Private Sub FilterPrevious()
        If SelectedFilter > 0 Then
            SelectedFilter -= 1
        Else
            SelectedFilter = InventoryFilters.Length - 1
        End If
    End Sub

    Private Sub FilterNext()
        If SelectedFilter < InventoryFilters.Length - 1 Then
            SelectedFilter += 1
        Else
            SelectedFilter = 0
        End If
    End Sub

    Public Sub InventoryManage()

        Dim DescriptionWidth As Integer = Console.WindowWidth - DESCSTART - 5
        FilterNameRow = MESSAGEAREAHEIGHT + 1
        ListHeaderStart = FilterNameRow + 2
        ListStartRow = ListHeaderStart + 1
        ListLastRow = Console.WindowHeight - STATUSAREAHEIGHT - 2

        ViewportClear()

        FiltersDraw()

        Dim x As Integer = ITEMNAMESTART
        Dim y As Integer = ListHeaderStart
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

        Dim ListRange As New Point(0, Console.WindowHeight - ListStartRow)
        SelectedFilter = InventoryFilterType.AllItems
        FilterApply()

        Do
            FiltersDraw()

            Dim ScreenRow As Integer = ListStartRow
            For ItemIndex As Integer = 0 To SortedInventory.Count - 1
                If IsBetween(ItemIndex, ListRange.X, ListRange.Y) Then

                    Dim InventoryItem As ItemBase = SortedInventory(ItemIndex)

                    ' Draw or clear the "equipped" indicator
                    Console.SetCursorPosition(ITEMNAMESTART - 1, ScreenRow)
                    If InventoryItem.IsEquipped Then
                        GraphicsCharacterDraw(EQUIPPEDINDICATOR)
                    Else
                        Console.Write(" ")
                    End If

                    ' item name
                    Dim NameText As String = String.Empty
                    If TypeOf InventoryItem Is AmmunitionBase Then
                        ' for ammo, include the group size and number remaining in this group
                        NameText = String.Format("{0} ({1}/{2})", InventoryItem.Name, CType(InventoryItem, AmmunitionBase).Remaining, CType(InventoryItem, AmmunitionBase).Size)
                    Else
                        NameText = InventoryItem.Name
                    End If
                    NameText += Space(ARMORDAMAGESTART - ITEMNAMESTART - NameText.Length)

                    ' item armor/damage
                    ' typeof() allows multiple levels of inheritance, so WeaponBase > HeavyCrowwbowMedium > DwarvenCrossbow will still resolve to WeaponBase
                    Dim ArmorDamageText As String = String.Empty
                    If TypeOf (InventoryItem) Is ArmorBase Then
                        ArmorDamageText = CType(InventoryItem, ArmorBase).ArmorBonus
                    ElseIf TypeOf (InventoryItem) Is WeaponBase Then
                        ArmorDamageText = CType(InventoryItem, WeaponBase).Damage
                    End If
                    ArmorDamageText += Space(WEIGHTSTART - ARMORDAMAGESTART - ArmorDamageText.Length)

                    ' item weight
                    Dim WeightText As String = InventoryItem.Weight & Space(VALUESTART - WEIGHTSTART - InventoryItem.Weight.ToString.Length)

                    ' item value
                    Dim ValueText As String = InventoryItem.Value & Space(DESCSTART - 1 - VALUESTART - InventoryItem.Value.ToString.Length)

                    ' Clear the description
                    For DescLine As Integer = ListStartRow To Console.WindowHeight - STATUSAREAHEIGHT - MESSAGEAREAHEIGHT
                        Console.SetCursorPosition(DESCSTART, DescLine)
                        Console.Write(Space(DescriptionWidth))
                    Next

                    If InventoryItem.IsSelected Then
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
                        Console.SetCursorPosition(DESCSTART, DescLine + ListStartRow)
                        Console.Write(DescriptionText(DescLine))
                    Next
                End If
            Next

            Select Case Console.ReadKey(True).Key

                Case ConsoleKey.LeftArrow
                    ClearList()
                    FilterPrevious()
                    FilterApply()

                Case ConsoleKey.RightArrow
                    ClearList()
                    FilterNext()
                    FilterApply()

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
                                    If TypeOf (InventoryItem) Is ArmorBase Then
                                        For Each ArmorItem As ItemBase In SortedInventory.Where(Function(a) TypeOf (a) Is ArmorBase)
                                            ArmorItem.IsEquipped = False
                                        Next
                                    ElseIf TypeOf (InventoryItem) Is WeaponBase Then
                                        For Each WeaponItem As ItemBase In SortedInventory.Where(Function(w) TypeOf (w) Is WeaponBase)
                                            WeaponItem.IsEquipped = False
                                        Next
                                    End If

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

End Module
