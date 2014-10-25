Public MustInherit Class Effect_Base
    Public SourceId As Guid
    Public Sub New()
        Me.SourceId = New Guid
    End Sub

    Public MustOverride Function CriteriaCheck(Item As ItemBase, Creature As CreatureBase) As Boolean

End Class
