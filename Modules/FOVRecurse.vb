' The code in this module comes from Andy Stobirski
' http://www.evilscience.co.uk/field-of-vision-using-recursive-shadow-casting-c-3-5-implementation/
' https://github.com/AndyStobirski/RogueLike/blob/master/FOVRecurse.cs
'
Module FOVRecurse

    Public VisibleOctants As New List(Of Integer)() From {1, 2, 3, 4, 5, 6, 7, 8}
    Public VisiblePoints As List(Of Point)

    Public Function VisibleCellsGet(Location As Point, Range As Integer) As List(Of Point)
        VisiblePoints = New List(Of Point)()
        For Each o As Integer In VisibleOctants
            ScanOctant(1, o, 1.0, 0.0, Location, Range)
        Next
        Return VisiblePoints
    End Function

    Private Sub ScanOctant(pDepth As Integer, pOctant As Integer, pStartSlope As Double, pEndSlope As Double, Location As Point, Range As Integer)

        Dim CurrentLevelWidth As Integer = Main.Map.DimensionsGet.Width
        Dim CurrentLevelHeight As Integer = Main.Map.DimensionsGet.Height
        Dim visrange2 As Integer = Range * Range
        Dim x As Integer = 0
        Dim y As Integer = 0

        Select Case pOctant

            Case 1
                'nnw
                y = Location.Y - pDepth
                If y < 0 Then
                    Return
                End If

                x = Location.X - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If x < 0 Then
                    x = 0
                End If

                While GetSlope(x, y, Location.X, Location.Y, False) >= pEndSlope
                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then
                        If Main.Map.TileGet(Main.Player1.MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            'current cell blocked
                            If x - 1 >= 0 AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x - 1, y).BlocksVision = False Then
                                'prior cell within range AND open...
                                '...incremenet the depth, adjust the endslope and recurse
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, Location.X, Location.Y, False), Location, Range)
                            End If
                        Else

                            If x - 1 >= 0 AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x - 1, y).BlocksVision Then
                                'prior cell within range AND open...
                                '..adjust the startslope
                                pStartSlope = GetSlope(x - 0.5, y - 0.5, Location.X, Location.Y, False)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    x += 1
                End While
                x -= 1
                Exit Select

            Case 2
                'nne
                y = Location.Y - pDepth
                If y < 0 Then
                    Return
                End If

                x = Location.X + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If x >= CurrentLevelWidth Then
                    x = CurrentLevelWidth - 1
                End If

                While GetSlope(x, y, Location.X, Location.Y, False) <= pEndSlope
                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then
                        If Main.Map.TileGet(Main.Player1.MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If x + 1 < CurrentLevelWidth AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x + 1, y).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, Location.X, Location.Y, False), Location, Range)
                            End If
                        Else
                            If x + 1 < CurrentLevelWidth AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x + 1, y).BlocksVision Then
                                pStartSlope = -GetSlope(x + 0.5, y - 0.5, Location.X, Location.Y, False)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    x -= 1
                End While
                x += 1
                Exit Select

            Case 3

                x = Location.X + pDepth
                If x >= CurrentLevelWidth Then
                    Return
                End If

                y = Location.Y - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If y < 0 Then
                    y = 0
                End If

                While GetSlope(x, y, Location.X, Location.Y, True) <= pEndSlope

                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then

                        If Main.Map.TileGet(Main.Player1.MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If y - 1 >= 0 AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x, y - 1).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, Location.X, Location.Y, True), Location, Range)
                            End If
                        Else
                            If y - 1 >= 0 AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x, y - 1).BlocksVision Then
                                pStartSlope = -GetSlope(x + 0.5, y - 0.5, Location.X, Location.Y, True)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    y += 1
                End While
                y -= 1
                Exit Select

            Case 4

                x = Location.X + pDepth
                If x >= CurrentLevelWidth Then
                    Return
                End If

                y = Location.Y + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If y >= CurrentLevelHeight Then
                    y = CurrentLevelHeight - 1
                End If

                While GetSlope(x, y, Location.X, Location.Y, True) >= pEndSlope

                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then

                        If Main.Map.TileGet(Main.Player1.MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If y + 1 < CurrentLevelHeight AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x, y + 1).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, Location.X, Location.Y, True), Location, Range)
                            End If
                        Else
                            If y + 1 < CurrentLevelHeight AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x, y + 1).BlocksVision Then
                                pStartSlope = GetSlope(x + 0.5, y + 0.5, Location.X, Location.Y, True)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    y -= 1
                End While
                y += 1
                Exit Select

            Case 5

                y = Location.Y + pDepth
                If y >= CurrentLevelHeight Then
                    Return
                End If

                x = Location.X + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If x >= CurrentLevelHeight Then
                    x = CurrentLevelHeight - 1
                End If

                While GetSlope(x, y, Location.X, Location.Y, False) >= pEndSlope
                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then

                        If Main.Map.TileGet(Main.Player1.MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If x + 1 < CurrentLevelWidth AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x + 1, y).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, Location.X, Location.Y, False), Location, Range)
                            End If
                        Else
                            If x + 1 < CurrentLevelWidth AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x + 1, y).BlocksVision Then
                                pStartSlope = GetSlope(x + 0.5, y + 0.5, Location.X, Location.Y, False)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    x -= 1
                End While
                x += 1
                Exit Select

            Case 6

                y = Location.Y + pDepth
                If y >= CurrentLevelHeight Then
                    Return
                End If

                x = Location.X - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If x < 0 Then
                    x = 0
                End If

                While GetSlope(x, y, Location.X, Location.Y, False) <= pEndSlope
                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then

                        If Main.Map.TileGet(Main.Player1.MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If x - 1 >= 0 AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x - 1, y).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, Location.X, Location.Y, False), Location, Range)
                            End If
                        Else
                            If x - 1 >= 0 AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x - 1, y).BlocksVision Then
                                pStartSlope = -GetSlope(x - 0.5, y + 0.5, Location.X, Location.Y, False)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    x += 1
                End While
                x -= 1
                Exit Select

            Case 7

                x = Location.X - pDepth
                If x < 0 Then
                    Return
                End If

                y = Location.Y + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If y >= CurrentLevelHeight Then
                    y = CurrentLevelHeight - 1
                End If

                While GetSlope(x, y, Location.X, Location.Y, True) <= pEndSlope

                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then

                        If Main.Map.TileGet(Main.Player1.MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If y + 1 < CurrentLevelHeight AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x, y + 1).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, Location.X, Location.Y, True), Location, Range)
                            End If
                        Else
                            If y + 1 < CurrentLevelHeight AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x, y + 1).BlocksVision Then
                                pStartSlope = -GetSlope(x - 0.5, y + 0.5, Location.X, Location.Y, True)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    y -= 1
                End While
                y += 1
                Exit Select

            Case 8
                'wnw
                x = Location.X - pDepth
                If x < 0 Then
                    Return
                End If

                y = Location.Y - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)))
                If y < 0 Then
                    y = 0
                End If

                While GetSlope(x, y, Location.X, Location.Y, True) >= pEndSlope

                    If GetVisDistance(x, y, Location.X, Location.Y) <= visrange2 Then

                        If Main.Map.TileGet(Main.Player1.MapLevel, x, y).BlocksVision Then
                            VisiblePoints.Add(New Point(x, y))   ' testing
                            If y - 1 >= 0 AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x, y - 1).BlocksVision = False Then
                                ScanOctant(pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, Location.X, Location.Y, True), Location, Range)
                            End If
                        Else
                            If y - 1 >= 0 AndAlso Main.Map.TileGet(Main.Player1.MapLevel, x, y - 1).BlocksVision Then
                                pStartSlope = GetSlope(x - 0.5, y - 0.5, Location.X, Location.Y, True)
                            End If

                            VisiblePoints.Add(New Point(x, y))
                        End If
                    End If
                    y += 1
                End While
                y -= 1
                Exit Select
        End Select


        If x < 0 Then
            x = 0
        ElseIf x >= CurrentLevelWidth Then
            x = CurrentLevelWidth - 1
        End If

        If y < 0 Then
            y = 0
        ElseIf y >= CurrentLevelHeight Then
            y = CurrentLevelHeight - 1
        End If

        If pDepth < Range And Main.Map.TileGet(Main.Player1.MapLevel, x, y).BlocksVision = False Then
            ScanOctant(pDepth + 1, pOctant, pStartSlope, pEndSlope, Location, Range)
        End If

    End Sub

    Private Function GetSlope(pX1 As Double, pY1 As Double, pX2 As Double, pY2 As Double, pInvert As Boolean) As Double
        If pInvert Then
            Return (pY1 - pY2) / (pX1 - pX2)
        Else
            Return (pX1 - pX2) / (pY1 - pY2)
        End If
    End Function

    Private Function GetVisDistance(pX1 As Integer, pY1 As Integer, pX2 As Integer, pY2 As Integer) As Integer
        Return ((pX1 - pX2) * (pX1 - pX2)) + ((pY1 - pY2) * (pY1 - pY2))
    End Function
End Module
