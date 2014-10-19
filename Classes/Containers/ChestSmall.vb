Public Class ChestSmall
    Inherits ContainerBase
    Public Sub New()
        Me.New("small chest")
    End Sub

    Public Sub New(ContainerName As String)
        MyBase.New()
        Me.Name = ContainerName
        Me.Value = 2
        Me.Weight = 5
        Me.DisplayCharacter = "c"
    End Sub
End Class