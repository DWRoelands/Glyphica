Module Combat

    Public Enum CombatType
        Melee
        Ranged
        Magic
    End Enum

    Public Sub Resolve(Enemy As CreatureBase, Type As CombatType)
        Select Case Type
            Case CombatType.Melee, CombatType.Ranged
                ResolvePhysical(Enemy, Type)
            Case CombatType.Magic
                ResolveMagical(Enemy)
        End Select

    End Sub

    Private Sub ResolvePhysical(Enemy As CreatureBase, Type As CombatType)

        Dim Defender As CreatureBase = Nothing
        Dim Attacker As CreatureBase = Nothing

        ' roll initiative
        ' Only matters for melee combat
        Dim PlayerInitiative As Integer
        Dim EnemyInitiative As Integer

        If Type = CombatType.Melee Then
            ' do initiative roll
            PlayerInitiative = Dice.RollDice("1d20") + Main.Player1.AttributeGet(CreatureAttribute.AttributeId.Initiative) + 20
            EnemyInitiative = Dice.RollDice("1d20") + Enemy.AttributeGet(CreatureAttribute.AttributeId.Initiative)
            Attacker = IIf(PlayerInitiative > EnemyInitiative, Main.Player1, Enemy)
            Defender = IIf(PlayerInitiative < EnemyInitiative, Main.Player1, Enemy)
        Else
            ' ranged combat - attacker is the "shooter", i.e. whoever initiated combat
            Defender = IIf(Main.Player1 Is Enemy, Main.Player1, Enemy)
            Attacker = IIf(Main.Player1 IsNot Enemy, Main.Player1, Enemy)
        End If

        ' Attacker goes first
        ' TODO: implement "20 aleays hits" and "1 always misses"
        Dim AttackerRoll As Integer = Dice.RollDice("1d20")
        Trace.Write(String.Format("Attacker H:{0} AC:{1}", AttackerRoll, Defender.AttributeGet(CreatureAttribute.AttributeId.ArmorClass)))
        If AttackerRoll >= Defender.AttributeGet(CreatureAttribute.AttributeId.ArmorClass) Then
            Dim Damage As Integer = Dice.RollDice(Attacker.DamageDiceMelee)
            If Attacker Is Main.Player1 Then
                Main.MessageWrite(String.Format("You hit the {0} for {1} damage.", Defender.Name, Damage))
            Else
                Main.MessageWrite(String.Format("The {0} hit you for {1} damage.", Attacker.Name, Damage))
            End If
            Defender.AttributeSet(CreatureAttribute.AttributeId.HitPoints_Current, Defender.AttributeGet(CreatureAttribute.AttributeId.HitPoints_Current) - Damage)
        Else
            If Attacker Is Main.Player1 Then
                Main.MessageWrite(String.Format("You missed the {0}.", Defender.Name))
            Else
                Main.MessageWrite(String.Format("The {0} missed.", Attacker.Name))
            End If
        End If

        ' did the defender die?
        If Defender.AttributeGet(CreatureAttribute.AttributeId.HitPoints_Current) <= 0 Then
            If Defender Is Main.Player1 Then
                Exit Sub
            Else
                Main.MessageWrite(String.Format("You killed the {0}!", Defender.Name))
                CreatureBase.Kill(Defender)
                Exit Sub
            End If
        End If

        ' Defender only gets to participate if this is melee combat
        ' If the defender is still alive, combat continues
        ' TODO: implement "20 aleays hits" and "1 always misses"
        If Type = CombatType.Melee Then
            Dim DefenderRoll As Integer = Dice.RollDice("1d20")
            Trace.Write(String.Format("Defender H:{0} AC:{1}", DefenderRoll, Attacker.AttributeGet(CreatureAttribute.AttributeId.ArmorClass)))
            If DefenderRoll >= Attacker.AttributeGet(CreatureAttribute.AttributeId.ArmorClass) Then
                Dim Damage As Integer = Dice.RollDice(Defender.DamageDiceMelee)
                If Defender Is Main.Player1 Then
                    Main.MessageWrite(String.Format("You hit the {0} for {1} damage.", Attacker.Name, Damage))
                Else
                    Main.MessageWrite(String.Format("The {0} hit you for {1} damage.", Defender.Name, Damage))
                End If
                Attacker.AttributeSet(CreatureAttribute.AttributeId.HitPoints_Current, Attacker.AttributeGet(CreatureAttribute.AttributeId.HitPoints_Current) - Damage)
            Else
                If Defender Is Main.Player1 Then
                    Main.MessageWrite(String.Format("You missed the {0}.", Attacker.Name))
                Else
                    Main.MessageWrite(String.Format("The {0} missed.", Defender.Name))
                End If
            End If

            ' did the attacker die
            If Attacker.AttributeGet(CreatureAttribute.AttributeId.HitPoints_Current) <= 0 Then
                If Attacker Is Main.Player1 Then
                    Exit Sub
                Else
                    Main.MessageWrite(String.Format("You killed the {0}!", Defender.Name))
                    CreatureBase.Kill(Attacker)
                    Exit Sub
                End If
            End If
        End If

    End Sub

    Private Sub ResolveMagical(Enemy As CreatureBase)

    End Sub
End Module
