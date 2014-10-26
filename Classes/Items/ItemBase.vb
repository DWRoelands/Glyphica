Public Class ItemBase
    Inherits Base


#Region "Basic item properties"
    Public Property ItemId As Guid
    Public Property IsEquippable As Boolean
    Public Property IsEquipped As Boolean
    Public Property IsPortable As Boolean
    Public Property Value As Integer
    Public Property Effects As New List(Of ItemEffect_Base)
    Public Property IsSelected As Boolean
#End Region

#Region "Properties implemented as effects"
    ''' <summary>
    ''' The weight of the item as represented by the weight effect in place on the item.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Weight is implemented as an item effect because it affects player encumbrance</remarks>
    Public Property Weight As Integer
        Get
            Return (From ie As Effect_Weight In Effects.Where(Function(x) TypeOf x Is Effect_Weight)).ToList(0).Modifier

        End Get
        Set(value As Integer)
            For Each ie As ItemEffect_Base In Effects.Where(Function(x) TypeOf x Is Effect_Weight)
                Effects.Remove(ie)
            Next
            Effects.Add(EffectFactory.ItemEffectGet(ItemEffect_Base.ItemEffectId.Weight, value))
        End Set
    End Property
#End Region

#Region "Events"
    Public Event OnEquip(Item As ItemBase, Creature As CreatureBase)
    Public Event OnUnEquip(Item As ItemBase, Creature As CreatureBase)
    Public Event OnPickup(Item As ItemBase, Creature As CreatureBase)
    Public Event OnDrop(Item As ItemBase, Creature As CreatureBase)
#End Region

    Public Sub New()
        MyBase.New()
        Me.ItemId = New Guid
    End Sub

End Class

