Imports System.Drawing
Public Class Creature
    Inherits Entity
    Public Enum CreatureMoveResult
        Move    ''no problem
        Wall    ''wall here - can't move
    End Enum

    Public Property VisualRange As Integer

    Public MustOverride



End Class
