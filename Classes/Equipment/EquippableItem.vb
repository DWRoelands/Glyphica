Public MustInherit Class EquippableItem
    Inherits Base
    Public Property Weight As Integer
    Public Property Cost As Integer

    Public MustOverride Sub Process(Wearer As CreatureBase)

End Class
