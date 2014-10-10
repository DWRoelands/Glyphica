Public MustInherit Class EquippableItem
    Public Property Name As String
    Public Property Weight As Integer
    Public Property Cost As Integer

    Public MustOverride Sub Process(Wearer As Creature)

End Class
