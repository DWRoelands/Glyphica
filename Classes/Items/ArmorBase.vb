Public Class ArmorBase
    Inherits ItemBase

#Region "Enums"
    Public Enum ArmorTier
        Light
        Medium
        Heavy
    End Enum

    Public Enum ArmorType
        Padded
        Leather
        StuddedLeather
        ChainShirt
        Hide
        ScaleMail
        Chainmail
        Breastplate
        SplintMail
        BandedMail
        HalfPlate
        FullPlate
    End Enum
#End Region

#Region "Basic armor properties"
    Public Property Tier As ArmorTier
    Public Property Type As ArmorType
#End Region

#Region "Properties implemented as effects"
    ''' <summary>
    ''' The possibility of spell failure chance as a result of wearing this armor
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Only one effect of this type can be on a piece of armor at a time</remarks>
    Public Property ArcaneSpellFailureChance As Integer
        Get
            Return (From ie As Effect_ArcaneSpellFailureChance In Effects.Where(Function(x) TypeOf x Is Effect_ArcaneSpellFailureChance)).ToList(0).Modifier
        End Get
        Set(value As Integer)
            For Each ie As ItemEffect_Base In Effects.Where(Function(x) TypeOf x Is Effect_ArcaneSpellFailureChance)
                Effects.Remove(ie)
            Next
            Effects.Add(EffectFactory.ItemEffectGet(ItemEffect_Base.ItemEffectId.ArcaneSpellFailureChance, value))
        End Set
    End Property

    ''' <summary>
    ''' The increase to a creature's armor class that results from wearing this armor
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Only one effect of this type can be on a piece of armor at a time</remarks>
    Public Property ArmorBonus As Integer
        Get
            Return (From ie As Effect_ArmorBonus In Effects.Where(Function(x) TypeOf x Is Effect_ArmorBonus)).ToList(0).Modifier
        End Get
        Set(value As Integer)
            For Each ie As ItemEffect_Base In Effects.Where(Function(x) TypeOf x Is Effect_ArmorBonus)
                Effects.Remove(ie)
            Next
            Effects.Add(EffectFactory.ItemEffectGet(ItemEffect_Base.ItemEffectId.ArmorBonus, value))
        End Set
    End Property

    ''' <summary>
    ''' The cap imposed on dexterity bonuses as a result of wearing this armor
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Only one effect of this type can be on a piece of armor at a time</remarks>
    Public Property MaximumDeterityBonus As Integer
        Get
            Return (From ie As Effect_MaximumDexterityBonus In Effects.Where(Function(x) TypeOf x Is Effect_MaximumDexterityBonus)).ToList(0).Modifier
        End Get
        Set(value As Integer)
            For Each ie As ItemEffect_Base In Effects.Where(Function(x) TypeOf x Is Effect_MaximumDexterityBonus)
                Effects.Remove(ie)
            Next
            Effects.Add(EffectFactory.ItemEffectGet(ItemEffect_Base.ItemEffectId.MaximumDexterityBonus, value))
        End Set
    End Property

#End Region

    Public Sub New()
        Me.DisplayCharacter = "a"
        Me.IsEquippable = True
        Me.IsPortable = True
    End Sub

End Class
