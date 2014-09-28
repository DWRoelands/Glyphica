' The code in this module comes from Jason Dean.
' http://blog.vbmagic.net/2011/04/19/rolling-the-dice/

Module Dice
    Public Function RollDice(value As String) As Integer
        ' setup working variables to make things easier
        Dim tmp As String = ""
        Dim modType As String = ""
        Dim numberOfDice As Integer = 0
        Dim numberOfSidesPerDice As Integer = 0
        Dim modifier As Integer = 0
        ' First we check to see if there is a "d" in the string
        If value.Contains("d") = True Then
            ' There is so first we need to get the value infront of the d
            tmp = Field(value, "d"c, 1).Trim
            ' and convert it to an integer if it's a number, errors are silently
            ' ignored for now. 
            If Integer.TryParse(tmp, numberOfDice) = False Then
                numberOfDice = 0
            End If
            ' Now look at the value after the d
            tmp = Field(value, "d"c, 2).Trim
            ' does it contain a + or a -?
            If tmp.Contains("+") = True Then
                modType = "+"
            End If
            If tmp.Contains("-") = True Then
                modType = "-"
            End If
            ' if does not contain a + or a -, then there is no modifer
            If modType = "" Then
                ' and we take the right side of the d as the number of sides
                ' of the dice
                If Integer.TryParse(tmp, numberOfSidesPerDice) = False Then
                    numberOfSidesPerDice = 0
                End If
            Else
                ' there is a + or a - so we need to extract the number on the left
                ' side of the +/-
                Dim bit As String = Field(tmp, CChar(modType), 1).Trim
                If Integer.TryParse(bit, numberOfSidesPerDice) = False Then
                    numberOfSidesPerDice = 0
                End If
                ' now we take the right side of the +/- and set that to the modifier
                bit = Field(tmp, CChar(modType), 2).Trim
                If Integer.TryParse(bit, modifier) = False Then
                    modifier = 0
                End If
            End If
        Else
            ' Ah so there is no d so we assume it's not a forumlar, just a number
            numberOfDice = 0
            numberOfSidesPerDice = 0
            If Integer.TryParse(value, 0) = True Then
                modifier = 0
            Else
                modifier = 0
            End If
        End If


        ' Now comes time to roll the dice
        Dim lp As Integer
        Dim total As Integer = 0
        ' Set up a random object randomised by the syystem date and time
        Dim objRandom As New System.Random(CType(System.DateTime.Now.Ticks Mod System.Int32.MaxValue, Integer))
        ' loop through the number of dice
        For lp = 1 To numberOfDice
            ' add each roll to the total
            total = total + CInt(objRandom.Next(numberOfSidesPerDice) + 1)
        Next
        ' now modify the total if needed
        If modType = "+" Then
            total += modifier
        ElseIf modType = "-" Then
            total -= modifier
        End If
        ' we have the results of the dice roll
        Return total
    End Function

    ' Using the delimiter to split the string into chunks, return the pos chunk
    ' e.g. Field("1d6+1","d",2) would return "6+1"
    Private Function Field(ByVal sourceString As String, ByVal delimiter As Char, ByVal pos As Integer) As String
        Dim parts() As String = sourceString.Split(delimiter)
        If pos > parts.Length Then
            Return ""
        Else
            Return parts(pos - 1)
        End If
    End Function
End Module
