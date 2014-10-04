' This code is a VB conversion of Andy Stobirski's excellent "Simple Roguelike Dungeon Generator"
' Blog post: http://www.evilscience.co.uk/roguelike-dungeon-generator-using-c/
' GitHub: https://github.com/AndyStobirski/RogueLike/blob/master/csMapbuilder
' All copyrights belong to Andy Stobirski
' Re-used with kind permission
'
'=======================================================
' C#-to-VB.NET Conversion Service provided by Telerik (www.telerik.com)
' Conversion powered by NRefactory.
' Twitter: @telerik
' Facebook: facebook.com/telerik
'=======================================================
'
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq

Class MapBuilder
    Public map As Integer(,)

    ''' <summary>
    ''' Built rooms stored here
    ''' </summary>
    Private rctBuiltRooms As List(Of Rectangle)

    ''' <summary>
    ''' Built corridors stored here
    ''' </summary>
    Private lBuilltCorridors As List(Of Point)

    ''' <summary>
    ''' Corridor to be built stored here
    ''' </summary>
    Private lPotentialCorridor As List(Of Point)

    ''' <summary>
    ''' Room to be built stored here
    ''' </summary>
    Private rctCurrentRoom As Rectangle


#Region "builder public properties"

    'room properties
    <Category("Room"), Description("Minimum Size"), DisplayName("Minimum Size")> _
    Public Property Room_Min() As Size
        Get
            Return m_Room_Min
        End Get
        Set(value As Size)
            m_Room_Min = value
        End Set
    End Property
    Private m_Room_Min As Size
    <Category("Room"), Description("Max Size"), DisplayName("Maximum Size")> _
    Public Property Room_Max() As Size
        Get
            Return m_Room_Max
        End Get
        Set(value As Size)
            m_Room_Max = value
        End Set
    End Property
    Private m_Room_Max As Size
    <Category("Room"), Description("Total number"), DisplayName("Rooms to build")> _
    Public Property MaxRooms() As Integer
        Get
            Return m_MaxRooms
        End Get
        Set(value As Integer)
            m_MaxRooms = value
        End Set
    End Property
    Private m_MaxRooms As Integer
    <Category("Room"), Description("Minimum distance between rooms"), DisplayName("Distance from other rooms")> _
    Public Property RoomDistance() As Integer
        Get
            Return m_RoomDistance
        End Get
        Set(value As Integer)
            m_RoomDistance = value
        End Set
    End Property
    Private m_RoomDistance As Integer
    <Category("Room"), Description("Minimum distance of room from existing corridors"), DisplayName("Corridor Distance")> _
    Public Property CorridorDistance() As Integer
        Get
            Return m_CorridorDistance
        End Get
        Set(value As Integer)
            m_CorridorDistance = value
        End Set
    End Property
    Private m_CorridorDistance As Integer

    'corridor properties
    <Category("Corridor"), Description("Minimum corridor length"), DisplayName("Minimum length")> _
    Public Property Corridor_Min() As Integer
        Get
            Return m_Corridor_Min
        End Get
        Set(value As Integer)
            m_Corridor_Min = value
        End Set
    End Property
    Private m_Corridor_Min As Integer
    <Category("Corridor"), Description("Maximum corridor length"), DisplayName("Maximum length")> _
    Public Property Corridor_Max() As Integer
        Get
            Return m_Corridor_Max
        End Get
        Set(value As Integer)
            m_Corridor_Max = value
        End Set
    End Property
    Private m_Corridor_Max As Integer
    <Category("Corridor"), Description("Maximum turns"), DisplayName("Maximum turns")> _
    Public Property Corridor_MaxTurns() As Integer
        Get
            Return m_Corridor_MaxTurns
        End Get
        Set(value As Integer)
            m_Corridor_MaxTurns = value
        End Set
    End Property
    Private m_Corridor_MaxTurns As Integer
    <Category("Corridor"), Description("The distance a corridor has to be away from a closed cell for it to be built"), DisplayName("Corridor Spacing")> _
    Public Property CorridorSpace() As Integer
        Get
            Return m_CorridorSpace
        End Get
        Set(value As Integer)
            m_CorridorSpace = value
        End Set
    End Property
    Private m_CorridorSpace As Integer


    <Category("Probability"), Description("Probability of building a corridor from a room or corridor. Greater than value = room"), DisplayName("Select room")> _
    Public Property BuildProb() As Integer
        Get
            Return m_BuildProb
        End Get
        Set(value As Integer)
            m_BuildProb = value
        End Set
    End Property
    Private m_BuildProb As Integer

    <Category("Map"), DisplayName("Map Size")> _
    Public Property Map_Size() As Size
        Get
            Return m_Map_Size
        End Get
        Set(value As Size)
            m_Map_Size = value
        End Set
    End Property
    Private m_Map_Size As Size
    <Category("Map"), DisplayName("Break Out"), Description("")> _
    Public Property BreakOut() As Integer
        Get
            Return m_BreakOut
        End Get
        Set(value As Integer)
            m_BreakOut = value
        End Set
    End Property
    Private m_BreakOut As Integer



#End Region

    ''' <summary>
    ''' describes the outcome of the corridor building operation
    ''' </summary>
    Private Enum CorridorItemHit

        invalid
        'invalid point generated
        self
        'corridor hit self
        existingcorridor
        'hit a built corridor
        originroom
        'corridor hit origin room 
        existingroom
        'corridor hit existing room
        completed
        'corridor built without problem    
        tooclose
        OK
        'point OK
    End Enum

    'n
    's
    'w
    'e
    Private directions_straight As Point() = New Point() {New Point(0, -1), New Point(0, 1), New Point(1, 0), New Point(-1, 0)}

    Private filledcell As Integer = 1
    Private emptycell As Integer = 0

    Private rnd As New Random()

    Public Sub New(x As Integer, y As Integer)
        Map_Size = New Size(x, y)
        map = New Integer(Map_Size.Width - 1, Map_Size.Height - 1) {}
        Corridor_MaxTurns = 5
        Room_Min = New Size(3, 3)
        Room_Max = New Size(15, 15)
        Corridor_Min = 3
        Corridor_Max = 15
        MaxRooms = 15
        Map_Size = New Size(150, 150)

        RoomDistance = 5
        CorridorDistance = 2
        CorridorSpace = 2

        BuildProb = 50
        BreakOut = 250
    End Sub

    ''' <summary>
    ''' Initialise everything
    ''' </summary>
    Private Sub Clear()
        lPotentialCorridor = New List(Of Point)()
        rctBuiltRooms = New List(Of Rectangle)()
        lBuilltCorridors = New List(Of Point)()

        map = New Integer(Map_Size.Width - 1, Map_Size.Height - 1) {}
        For x As Integer = 0 To Map_Size.Width - 1
            For y As Integer = 0 To Map_Size.Width - 1
                map(x, y) = filledcell
            Next
        Next
    End Sub

#Region "build methods()"

    ''' <summary>
    ''' Randomly choose a room and attempt to build a corridor terminated by
    ''' a room off it, and repeat until MaxRooms has been reached. The map
    ''' is started of by placing a room in approximately the centre of the map
    ''' using the method PlaceStartRoom()
    ''' </summary>
    ''' <returns>Bool indicating if the map was built, i.e. the property BreakOut was not
    ''' exceed</returns>
    Public Function Build_OneStartRoom() As Boolean
        Dim loopctr As Integer = 0

        Dim CorBuildOutcome As CorridorItemHit
        Dim Location As New Point()
        Dim Direction As New Point()

        Clear()

        PlaceStartRoom()

        'attempt to build the required number of rooms
        While rctBuiltRooms.Count() < MaxRooms

            If System.Math.Max(System.Threading.Interlocked.Increment(loopctr), loopctr - 1) > BreakOut Then
                'bail out if this value is exceeded
                Return False
            End If

            If Corridor_GetStart(Location, Direction) Then

                CorBuildOutcome = CorridorMake_Straight(Location, Direction, rnd.[Next](1, Corridor_MaxTurns), If(rnd.[Next](0, 100) > 50, True, False))

                Select Case CorBuildOutcome
                    Case CorridorItemHit.existingroom, CorridorItemHit.existingcorridor, CorridorItemHit.self
                        Corridor_Build()
                        Exit Select

                    Case CorridorItemHit.completed
                        If Room_AttemptBuildOnCorridor(Direction) Then
                            Corridor_Build()
                            Room_Build()
                        End If
                        Exit Select
                End Select
            End If
        End While

        Return True
    End Function

    ''' <summary>
    ''' Randomly choose a room and attempt to build a corridor terminated by
    ''' a room off it, and repeat until MaxRooms has been reached. The map
    ''' is started of by placing two rooms on opposite sides of the map and joins
    ''' them with a long corridor, using the method PlaceStartRooms()
    ''' </summary>
    ''' <returns>Bool indicating if the map was built, i.e. the property BreakOut was not
    ''' exceed</returns>
    Public Function Build_ConnectedStartRooms() As Boolean
        Dim loopctr As Integer = 0

        Dim CorBuildOutcome As CorridorItemHit
        Dim Location As New Point()
        Dim Direction As New Point()

        Clear()

        PlaceStartRooms()

        'attempt to build the required number of rooms
        While rctBuiltRooms.Count() < MaxRooms

            If System.Math.Max(System.Threading.Interlocked.Increment(loopctr), loopctr - 1) > BreakOut Then
                'bail out if this value is exceeded
                Return False
            End If

            If Corridor_GetStart(Location, Direction) Then

                CorBuildOutcome = CorridorMake_Straight(Location, Direction, rnd.[Next](1, Corridor_MaxTurns), If(rnd.[Next](0, 100) > 50, True, False))

                Select Case CorBuildOutcome
                    Case CorridorItemHit.existingroom, CorridorItemHit.existingcorridor, CorridorItemHit.self
                        Corridor_Build()
                        Exit Select

                    Case CorridorItemHit.completed
                        If Room_AttemptBuildOnCorridor(Direction) Then
                            Corridor_Build()
                            Room_Build()
                        End If
                        Exit Select
                End Select
            End If
        End While

        Return True

    End Function

#End Region


#Region "room utilities"

    ''' <summary>
    ''' Place a random sized room in the middle of the map
    ''' </summary>
    Private Sub PlaceStartRoom()
        rctCurrentRoom = New Rectangle() With { _
         .Width = rnd.[Next](Room_Min.Width, Room_Max.Width), _
         .Height = rnd.[Next](Room_Min.Height, Room_Max.Height) _
        }
        rctCurrentRoom.X = Map_Size.Width / 2
        rctCurrentRoom.Y = Map_Size.Height / 2
        Room_Build()
    End Sub


    ''' <summary>
    ''' Place a start room anywhere on the map
    ''' </summary>
    Private Sub PlaceStartRooms()

        Dim startdirection As Point
        Dim connection As Boolean = False
        Dim Location As New Point()
        Dim Direction As New Point()
        Dim CorBuildOutcome As CorridorItemHit

        While Not connection

            Clear()
            startdirection = Direction_Get(New Point())

            'place a room on the top and bottom
            If startdirection.X = 0 Then

                'room at the top of the map
                rctCurrentRoom = New Rectangle() With { _
                 .Width = rnd.[Next](Room_Min.Width, Room_Max.Width), _
                 .Height = rnd.[Next](Room_Min.Height, Room_Max.Height) _
                }
                rctCurrentRoom.X = rnd.[Next](0, Map_Size.Width - rctCurrentRoom.Width)
                rctCurrentRoom.Y = 1
                Room_Build()

                'at the bottom of the map
                rctCurrentRoom = New Rectangle()
                rctCurrentRoom.Width = rnd.[Next](Room_Min.Width, Room_Max.Width)
                rctCurrentRoom.Height = rnd.[Next](Room_Min.Height, Room_Max.Height)
                rctCurrentRoom.X = rnd.[Next](0, Map_Size.Width - rctCurrentRoom.Width)
                rctCurrentRoom.Y = Map_Size.Height - rctCurrentRoom.Height - 1


                Room_Build()
            Else
                'place a room on the east and west side
                'west side of room
                rctCurrentRoom = New Rectangle()
                rctCurrentRoom.Width = rnd.[Next](Room_Min.Width, Room_Max.Width)
                rctCurrentRoom.Height = rnd.[Next](Room_Min.Height, Room_Max.Height)
                rctCurrentRoom.Y = rnd.[Next](0, Map_Size.Height - rctCurrentRoom.Height)
                rctCurrentRoom.X = 1
                Room_Build()

                rctCurrentRoom = New Rectangle()
                rctCurrentRoom.Width = rnd.[Next](Room_Min.Width, Room_Max.Width)
                rctCurrentRoom.Height = rnd.[Next](Room_Min.Height, Room_Max.Height)
                rctCurrentRoom.Y = rnd.[Next](0, Map_Size.Height - rctCurrentRoom.Height)
                rctCurrentRoom.X = Map_Size.Width - rctCurrentRoom.Width - 2

                Room_Build()
            End If



            If Corridor_GetStart(Location, Direction) Then



                CorBuildOutcome = CorridorMake_Straight(Location, Direction, 100, True)

                Select Case CorBuildOutcome
                    Case CorridorItemHit.existingroom
                        Corridor_Build()
                        connection = True
                        Exit Select
                End Select
            End If
        End While
    End Sub

    ''' <summary>
    ''' Make a room off the last point of Corridor, using
    ''' CorridorDirection as an indicator of how to offset the room.
    ''' The potential room is stored in Room.
    ''' </summary>
    Private Function Room_AttemptBuildOnCorridor(pDirection As Point) As Boolean
        rctCurrentRoom = New Rectangle() With { _
         .Width = rnd.[Next](Room_Min.Width, Room_Max.Width), _
         .Height = rnd.[Next](Room_Min.Height, Room_Max.Height) _
        }

        'startbuilding room from this point
        Dim lc As Point = lPotentialCorridor.Last()

        If pDirection.X = 0 Then
            'north/south direction
            rctCurrentRoom.X = rnd.[Next](lc.X - rctCurrentRoom.Width + 1, lc.X)

            If pDirection.Y = 1 Then
                rctCurrentRoom.Y = lc.Y + 1
            Else
                'south
                rctCurrentRoom.Y = lc.Y - rctCurrentRoom.Height - 1
                'north

            End If
        ElseIf pDirection.Y = 0 Then
            'east / west direction
            rctCurrentRoom.Y = rnd.[Next](lc.Y - rctCurrentRoom.Height + 1, lc.Y)

            If pDirection.X = -1 Then
                'west
                rctCurrentRoom.X = lc.X - rctCurrentRoom.Width
            Else
                rctCurrentRoom.X = lc.X + 1
                'east
            End If
        End If

        Return Room_Verify()
    End Function


    ''' <summary>
    ''' Randomly get a point on the edge of a randomly selected room
    ''' </summary>
    ''' <param name="pLocation">Out: Location of point on room edge</param>
    ''' <param name="pDirection">Out: Direction of point</param>
    Private Sub Room_GetEdge(ByRef pLocation As Point, ByRef pDirection As Point)

        rctCurrentRoom = rctBuiltRooms(rnd.[Next](0, rctBuiltRooms.Count()))

        'pick a random point within a room
        'the +1 / -1 on the values are to stop a corner from being chosen
        pLocation = New Point(rnd.[Next](rctCurrentRoom.Left + 1, rctCurrentRoom.Right - 1), rnd.[Next](rctCurrentRoom.Top + 1, rctCurrentRoom.Bottom - 1))


        'get a random direction
        pDirection = directions_straight(rnd.[Next](0, directions_straight.GetLength(0)))

        Do
            'move in that direction
            pLocation.Offset(pDirection)

            If Not Point_Valid(pLocation.X, pLocation.Y) Then
                Return

                'until we meet an empty cell
            End If
        Loop While Point_Get(pLocation.X, pLocation.Y) <> filledcell

    End Sub

#End Region

#Region "corridor utitlies"

    ''' <summary>
    ''' Randomly get a point on an existing corridor
    ''' </summary>
    ''' <param name="pLocation">Out: location of point</param>
    Private Sub Corridor_GetEdge(ByRef pLocation As Point, ByRef pDirection As Point)
        Dim validdirections As New List(Of Point)()

        Do
            'the modifiers below prevent the first of last point being chosen
            pLocation = lBuilltCorridors(rnd.[Next](1, lBuilltCorridors.Count - 1))

            'attempt to locate all the empy map points around the location
            'using the directions to offset the randomly chosen point
            For Each p As Point In directions_straight
                If Point_Valid(pLocation.X + p.X, pLocation.Y + p.Y) Then
                    If Point_Get(pLocation.X + p.X, pLocation.Y + p.Y) = filledcell Then
                        validdirections.Add(p)
                    End If
                End If


            Next
        Loop While validdirections.Count = 0

        pDirection = validdirections(rnd.[Next](0, validdirections.Count))
        pLocation.Offset(pDirection)

    End Sub

    ''' <summary>
    ''' Build the contents of lPotentialCorridor, adding it's points to the builtCorridors
    ''' list then empty
    ''' </summary>
    Private Sub Corridor_Build()
        For Each p As Point In lPotentialCorridor
            Point_Set(p.X, p.Y, emptycell)
            lBuilltCorridors.Add(p)
        Next

        lPotentialCorridor.Clear()
    End Sub

    ''' <summary>
    ''' Get a starting point for a corridor, randomly choosing between a room and a corridor.
    ''' </summary>
    ''' <param name="pLocation">Out: pLocation of point</param>
    ''' <returns>Bool indicating if location found is OK</returns>
    Private Function Corridor_GetStart(ByRef pLocation As Point, ByRef pDirection As Point) As Boolean
        rctCurrentRoom = New Rectangle()
        lPotentialCorridor = New List(Of Point)()

        If lBuilltCorridors.Count > 0 Then
            If rnd.[Next](0, 100) >= BuildProb Then
                Room_GetEdge(pLocation, pDirection)
            Else
                Corridor_GetEdge(pLocation, pDirection)
            End If
        Else
            'no corridors present, so build off a room
            Room_GetEdge(pLocation, pDirection)
        End If

        'finally check the point we've found
        Return Corridor_PointTest(pLocation, pDirection) = CorridorItemHit.OK

    End Function

    ''' <summary>
    ''' Attempt to make a corridor, storing it in the lPotentialCorridor list
    ''' </summary>
    ''' <param name="pStart">Start point of corridor</param>
    ''' <param name="pTurns">Number of turns to make</param>
    Private Function CorridorMake_Straight(ByRef pStart As Point, ByRef pDirection As Point, pTurns As Integer, pPreventBackTracking As Boolean) As CorridorItemHit

        lPotentialCorridor = New List(Of Point)()
        lPotentialCorridor.Add(pStart)

        Dim corridorlength As Integer
        Dim startdirection As New Point(pDirection.X, pDirection.Y)
        Dim outcome As CorridorItemHit

        While pTurns > 0
            pTurns -= 1

            corridorlength = rnd.[Next](Corridor_Min, Corridor_Max)
            'build corridor
            While corridorlength > 0
                corridorlength -= 1

                'make a point and offset it
                pStart.Offset(pDirection)

                outcome = Corridor_PointTest(pStart, pDirection)
                If outcome <> CorridorItemHit.OK Then
                    Return outcome
                Else
                    lPotentialCorridor.Add(pStart)
                End If
            End While

            If pTurns > 1 Then
                If Not pPreventBackTracking Then
                    pDirection = Direction_Get(pDirection)
                Else
                    pDirection = Direction_Get(pDirection, startdirection)
                End If
            End If
        End While

        Return CorridorItemHit.completed
    End Function

    ''' <summary>
    ''' Test the provided point to see if it has empty cells on either side
    ''' of it. This is to stop corridors being built adjacent to a room.
    ''' </summary>
    ''' <param name="pPoint">Point to test</param>
    ''' <param name="pDirection">Direction it is moving in</param>
    ''' <returns></returns>
    Private Function Corridor_PointTest(pPoint As Point, pDirection As Point) As CorridorItemHit


        If Not Point_Valid(pPoint.X, pPoint.Y) Then
            'invalid point hit, exit
            Return CorridorItemHit.invalid
        ElseIf lBuilltCorridors.Contains(pPoint) Then
            'in an existing corridor
            Return CorridorItemHit.existingcorridor
        ElseIf lPotentialCorridor.Contains(pPoint) Then
            'hit self
            Return CorridorItemHit.self
        ElseIf rctCurrentRoom <> Rectangle.Empty AndAlso rctCurrentRoom.Contains(pPoint) Then
            'the corridors origin room has been reached, exit
            Return CorridorItemHit.originroom
        Else
            'is point in a room
            For Each r As Rectangle In rctBuiltRooms
                If r.Contains(pPoint) Then
                    Return CorridorItemHit.existingroom
                End If
            Next
        End If


        'using the property corridor space, check that number of cells on
        'either side of the point are empty
        For Each r As Integer In Enumerable.Range(-CorridorSpace, 2 * CorridorSpace + 1).ToList()
            If pDirection.X = 0 Then
                'north or south
                If Point_Valid(pPoint.X + r, pPoint.Y) Then
                    If Point_Get(pPoint.X + r, pPoint.Y) <> filledcell Then
                        Return CorridorItemHit.tooclose
                    End If
                End If
            ElseIf pDirection.Y = 0 Then
                'east west
                If Point_Valid(pPoint.X, pPoint.Y + r) Then
                    If Point_Get(pPoint.X, pPoint.Y + r) <> filledcell Then
                        Return CorridorItemHit.tooclose
                    End If
                End If

            End If
        Next

        Return CorridorItemHit.OK
    End Function


#End Region

#Region "direction methods"

    ''' <summary>
    ''' Get a random direction, excluding the opposite of the provided direction to
    ''' prevent a corridor going back on it's Build
    ''' </summary>
    ''' <param name="pdir">Current direction</param>
    ''' <returns></returns>
    Private Function Direction_Get(pDir As Point) As Point
        Dim NewDir As Point
        Do
            NewDir = directions_straight(rnd.[Next](0, directions_straight.GetLength(0)))
        Loop While Direction_Reverse(NewDir) = pDir

        Return NewDir
    End Function

    ''' <summary>
    ''' Get a random direction, excluding the provided directions and the opposite of 
    ''' the provided direction to prevent a corridor going back on it's self.
    ''' 
    ''' The parameter pDirExclude is the first direction chosen for a corridor, and
    ''' to prevent it from being used will prevent a corridor from going back on 
    ''' it'self
    ''' </summary>
    ''' <param name="pdir">Current direction</param>
    ''' <param name="pDirExclude">Direction to exclude</param>
    ''' <returns></returns>
    Private Function Direction_Get(pDir As Point, pDirExclude As Point) As Point
        Dim NewDir As Point
        Do
            NewDir = directions_straight(rnd.[Next](0, directions_straight.GetLength(0)))
        Loop While Direction_Reverse(NewDir) = pDir Or Direction_Reverse(NewDir) = pDirExclude


        Return NewDir
    End Function

    Private Function Direction_Reverse(pDir As Point) As Point
        Return New Point(-pDir.X, -pDir.Y)
    End Function

#End Region

#Region "room test"

    ''' <summary>
    ''' Check if rctCurrentRoom can be built
    ''' </summary>
    ''' <returns>Bool indicating success</returns>
    Private Function Room_Verify() As Boolean
        'make it one bigger to ensure that testing gives it a border
        rctCurrentRoom.Inflate(RoomDistance, RoomDistance)

        'check it occupies legal, empty coordinates
        For x As Integer = rctCurrentRoom.Left To rctCurrentRoom.Right
            For y As Integer = rctCurrentRoom.Top To rctCurrentRoom.Bottom
                If Not Point_Valid(x, y) OrElse Point_Get(x, y) <> filledcell Then
                    Return False
                End If
            Next
        Next

        'check it doesn't encroach onto existing rooms
        For Each r As Rectangle In rctBuiltRooms
            If r.IntersectsWith(rctCurrentRoom) Then
                Return False
            End If
        Next

        rctCurrentRoom.Inflate(-RoomDistance, -RoomDistance)

        'check the room is the specified distance away from corridors
        rctCurrentRoom.Inflate(CorridorDistance, CorridorDistance)

        For Each p As Point In lBuilltCorridors
            If rctCurrentRoom.Contains(p) Then
                Return False
            End If
        Next

        rctCurrentRoom.Inflate(-CorridorDistance, -CorridorDistance)

        Return True
    End Function

    ''' <summary>
    ''' Add the global Room to the rooms collection and draw it on the map
    ''' </summary>
    Private Sub Room_Build()
        rctBuiltRooms.Add(rctCurrentRoom)

        For x As Integer = rctCurrentRoom.Left To rctCurrentRoom.Right
            For y As Integer = rctCurrentRoom.Top To rctCurrentRoom.Bottom
                map(x, y) = emptycell
            Next
        Next

    End Sub

#End Region

#Region "Map Utilities"

    ''' <summary>
    ''' Check if the point falls within the map array range
    ''' </summary>
    ''' <param name="x">x to test</param>
    ''' <param name="y">y to test</param>
    ''' <returns>Is point with map array?</returns>
    Private Function Point_Valid(x As Integer, y As Integer) As [Boolean]
        Return x >= 0 And x < map.GetLength(0) And y >= 0 And y < map.GetLength(1)
    End Function

    ''' <summary>
    ''' Set array point to specified value
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <param name="val"></param>
    Private Sub Point_Set(x As Integer, y As Integer, val As Integer)
        map(x, y) = val
    End Sub

    ''' <summary>
    ''' Get the value of the specified point
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <returns></returns>
    Private Function Point_Get(x As Integer, y As Integer) As Integer
        Return map(x, y)
    End Function

#End Region

    Public Delegate Sub moveDelegate()
    Public Event playerMoved As moveDelegate

End Class
