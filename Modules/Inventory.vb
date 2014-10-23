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

    Dim ListRange As Point
    Dim DescriptionWidth As Integer
    Dim Row_FilterNames As Integer
    Dim Row_InventoryNames As Integer
    Dim Row_ListFirstItem As Integer
    Dim Row_ListLastItem As Integer
    Dim Row_ListHeaders As Integer
    Dim Row_Helptext As Integer

    'Dim ActiveInventory As ActiveInventoryType
    Dim ActiveInventory As List(Of ItemBase) = Nothing
    Dim ExternalInventory As List(Of ItemBase) = Nothing
    Dim ActiveFilter As InventoryFilterType
    Dim Mode As InventoryMode

    Dim InventoryFilters() As String = {"All Items", "Armor", "Weapons", "Ammunition"}
    Dim SortedInventory As List(Of ItemBase) = Nothing

    Public Enum InventoryFilterType
        AllItems = 0
        Armor = 1
        Weapons = 2
        Ammunition = 3
    End Enum

    Public Enum InventoryMode
        Player
        Vendor
        Container
    End Enum

    Private Sub FiltersDraw()
        Console.SetCursorPosition(4, Row_FilterNames)
        For x = 0 To InventoryFilters.Length - 1
            If ActiveFilter = x Then
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
        For ScreenRow = Row_ListFirstItem To Row_ListLastItem
            Console.SetCursorPosition(ITEMNAMESTART - 1, ScreenRow)
            Console.Write(Space(DESCSTART - ITEMNAMESTART - 1))
        Next

        For Each i As ItemBase In Player1.Inventory
            i.IsSelected = False
        Next
    End Sub

    Private Sub FilterApply()
        Select Case ActiveFilter
            Case InventoryFilterType.AllItems
                SortedInventory = (From InventoryItem As ItemBase In ActiveInventory Order By InventoryItem.Name).ToList
            Case InventoryFilterType.Armor
                SortedInventory = (From InventoryItem As ItemBase In ActiveInventory Where TypeOf InventoryItem Is ArmorBase Order By InventoryItem.Name).ToList
            Case InventoryFilterType.Weapons
                SortedInventory = (From InventoryItem As ItemBase In ActiveInventory Where TypeOf InventoryItem Is WeaponBase Order By InventoryItem.Name).ToList
            Case InventoryFilterType.Ammunition
                SortedInventory = (From InventoryItem As ItemBase In ActiveInventory Where TypeOf InventoryItem Is AmmunitionBase Order By InventoryItem.Name).ToList
        End Select

        If SortedInventory.Count > 0 And (From x As ItemBase In SortedInventory Where x.IsSelected).Count = 0 Then
            SortedInventory(0).IsSelected = True
        End If
    End Sub

    Private Sub FilterPrevious()
        If ActiveFilter > 0 Then
            ActiveFilter -= 1
        Else
            ActiveFilter = InventoryFilters.Length - 1
        End If
    End Sub

    Private Sub FilterNext()
        If ActiveFilter < InventoryFilters.Length - 1 Then
            ActiveFilter += 1
        Else
            ActiveFilter = 0
        End If
    End Sub

    Private Sub InventoryNamesDraw(Source As Base)
        Console.SetCursorPosition(ITEMNAMESTART, Row_InventoryNames)

        If ActiveInventory Is Player1.Inventory Then
            Console.BackgroundColor = ConsoleColor.White
            Console.ForegroundColor = ConsoleColor.Black
        End If
        Console.Write(Player1.Name)

        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Write(" ")

        If ActiveInventory Is Source.Inventory Then
            Console.BackgroundColor = ConsoleColor.White
            Console.ForegroundColor = ConsoleColor.Black
        End If
        Console.Write(Source.Name)

        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
    End Sub

    Public Sub InventoryManage(Source As Base)

        If TypeOf Source Is Player Then
            Mode = InventoryMode.Player
            ActiveInventory = Player1.Inventory
        ElseIf TypeOf Source Is ContainerBase Then
            Mode = InventoryMode.Container
            ExternalInventory = Source.Inventory
            ActiveInventory = ExternalInventory
        ElseIf TypeOf Source Is VendorBase Then
            Mode = InventoryMode.Vendor
            ExternalInventory = Source.Inventory
            ActiveInventory = ExternalInventory
        Else
            Throw New Exception("Could not determine the inventory mode because of an unrecognized source.")
        End If

        ' set up the variables that control the screen layout
        If Source IsNot Player1 Then
            ' We are managing two inventories (e.g. chest, vendor, etc.)
            Row_InventoryNames = MESSAGEAREAHEIGHT + 1
        Else
            ' We are only managing the p;ayer's inventory
            Row_InventoryNames = MESSAGEAREAHEIGHT - 1
        End If

        ' all other rows are positioned relative to the inventory names row
        Row_FilterNames = Row_InventoryNames + 2
        Row_ListHeaders = Row_FilterNames + 2
        Row_ListFirstItem = Row_ListHeaders + 1
        Row_ListLastItem = Console.WindowHeight - STATUSAREAHEIGHT - 4
        Row_Helptext = Console.WindowHeight - STATUSAREAHEIGHT - 2

        ' Other layout variables
        DescriptionWidth = Console.WindowWidth - DESCSTART - 5
        ListRange = New Point(0, Console.WindowHeight - Row_ListFirstItem)

        ' Initialize
        Viewport.Clear()
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White

        Dim HelpText As String = "Left/Right to change filter, Up/Down to change selection, ESCAPE to exit"
        If Source IsNot Player1 Then
            HelpText = "PAGEUP/PAGEDOWN to change view, " & HelpText
        End If
        RightJustifiedMessage(Row_Helptext, HelpText)

        ColumnHeadersDraw()
        ActiveFilter = InventoryFilterType.AllItems

        ' MAIN INVENTORY LOOP
        Do
            If Source IsNot Player1 Then
                InventoryNamesDraw(Source)
            End If

            FilterApply()
            FiltersDraw()

            Dim ScreenRow As Integer = Row_ListFirstItem
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
                    For DescLine As Integer = Row_ListFirstItem To Console.WindowHeight - STATUSAREAHEIGHT - MESSAGEAREAHEIGHT
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
                        Console.SetCursorPosition(DESCSTART, DescLine + Row_ListFirstItem)
                        Console.Write(DescriptionText(DescLine))
                    Next
                End If
            Next

            Select Case Console.ReadKey(True).Key

                Case ConsoleKey.PageDown, ConsoleKey.PageUp
                    ClearList()
                    ActiveInventory = IIf(ActiveInventory Is Player1.Inventory, Source.Inventory, Player1.Inventory)
                    Continue Do

                Case ConsoleKey.LeftArrow
                    ClearList()
                    FilterPrevious()
                    Continue Do

                Case ConsoleKey.RightArrow
                    ClearList()
                    FilterNext()
                    Continue Do

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
                    Player1.ItemEffectsProcess()
                    Viewport.StatusUpdate()

                Case ConsoleKey.Escape
                    Exit Do
            End Select
        Loop

        Viewport.Clear()

    End Sub

    Private Sub ColumnHeadersDraw()
        Console.SetCursorPosition(ITEMNAMESTART, Row_ListHeaders)
        Console.Write("Name")

        Console.SetCursorPosition(ARMORDAMAGESTART, Row_ListHeaders)
        Console.Write("Armor/Damage")

        Console.SetCursorPosition(WEIGHTSTART, Row_ListHeaders)
        Console.Write("Weight")

        Console.SetCursorPosition(VALUESTART, Row_ListHeaders)
        Console.Write("Value")

        Console.SetCursorPosition(DESCSTART, Row_ListHeaders)
        Console.Write("Description")
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
