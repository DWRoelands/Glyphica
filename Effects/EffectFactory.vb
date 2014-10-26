Public Class EffectFactory

    Public Shared Function ItemEffectGet(Id As ItemEffect_Base.ItemEffectId, Modifier As Integer) As ItemEffect_Base
        Select Case Id
            Case ItemEffect_Base.ItemEffectId.ArcaneSpellFailureChance
                Return New Effect_ArcaneSpellFailureChance(Modifier)

            Case ItemEffect_Base.ItemEffectId.ArmorBonus
                Return New Effect_ArmorBonus(Modifier)

            Case ItemEffect_Base.ItemEffectId.MaximumDexterityBonus
                Return New Effect_MaximumDexterityBonus(Modifier)

            Case ItemEffect_Base.ItemEffectId.Weight
                Return New Effect_Weight(Modifier)
        End Select

        Return Nothing
    End Function



End Class
