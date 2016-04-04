Public Class star
    Friend Shared Function build(ByRef starmap As starmap, ByVal numPlanetsMin As Integer, ByVal numPlanetsMax As Integer, ByRef r As Random) As star
        Dim star As New star
        With star
            ._starmap = starmap
            ._name = getRandomAndRemove(star.starNames, "data/starnames.txt", r)
            .xy = buildXy(r)
            .imgString = buildImgString(r)

            Dim factionPair As faction() = starmap.getFactionPair(r)
            For n = 1 To r.Next(numPlanetsMin, numPlanetsMax + 1)
                Dim planet As planet = planet.build(star, n, factionPair, r)
                ._planets.Add(planet)
            Next
        End With
        Return star
    End Function
    Private Shared Function buildXy(ByRef r As Random) As xy
        If xys.Count = 0 Then
            Dim xRange As Integer = (maxX / xySize) - 1
            Dim yRange As Integer = (maxY / xySize) - 1

            For x = 1 To xRange
                For y = 1 To yRange
                    xys.Add(New xy(x * xySize, y * xySize))
                Next
            Next
        End If

        Dim roll As Integer = r.Next(xys.Count - 1)
        buildXy = xys(roll)
        xys.RemoveAt(roll)
    End Function
    Private Shared Function buildImgString(ByRef r As Random) As String
        Select Case r.Next(1, 6)
            Case 1 : Return "Blue"
            Case 2 : Return "BlueWhite"
            Case 3 : Return "OrangeRed"
            Case 4 : Return "Red"
            Case 5 : Return "WhiteYellow"
            Case Else
                MsgBox("Error: buildImgString out of bounds.")
                Return Nothing
        End Select
    End Function
    Private Shared xys As New List(Of xy)
    Private Shared starNames As New List(Of String)
    Friend Const maxX As Integer = 650
    Friend Const maxY As Integer = 450
    Friend Const xySize As Integer = 30

    Public Overrides Function ToString() As String
        Return name
    End Function
    Friend Sub consoleReport(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)

        Console.WriteLine(ind & name & " (" & xy.ToString & ")")
        For Each planet In _planets
            planet.consoleReport(indent + 1)
            Console.WriteLine()
        Next
    End Sub

    Private _starmap As starmap
    Friend ReadOnly Property starmap As starmap
        Get
            Return _starmap
        End Get
    End Property
    Private _name As String
    Friend ReadOnly Property name As String
        Get
            Return _name
        End Get
    End Property
    Friend imgString As String

    Private _planets As New List(Of planet)
    Friend ReadOnly Property planets As List(Of planet)
        Get
            Return _planets
        End Get
    End Property
    Friend Function getPlanet(ByVal planetNumber As Integer) As planet
        If planetNumber <= 0 OrElse planetNumber > planets.Count Then Return Nothing
        Return planets(planetNumber - 1)
    End Function
    Friend Function getPlanetRandom() As planet
        Return _planets(rng.Next(_planets.Count))
    End Function
    Friend ReadOnly Property factions As List(Of faction)
        Get
            Dim total As New List(Of faction)
            For Each planet In planets
                If total.Contains(planet.faction) = False Then total.Add(planet.faction)
            Next
            Return total
        End Get
    End Property

    Friend xy As xy
    Friend Function getDistanceTo(ByRef destination As star) As Integer
        Dim distance As Integer = pythogoras(destination.xy, xy)
        Return distance
    End Function

    Friend Sub tick()
        For Each planet In _planets
            planet.tick()
        Next
    End Sub
End Class
