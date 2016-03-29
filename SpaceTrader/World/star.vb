Public Class star
    Friend Shared Function build(ByRef starmap As starmap, ByVal numPlanetsMin As Integer, ByVal numPlanetsMax As Integer, ByRef r As Random) As star
        Dim star As New star
        With star
            ._starmap = starmap
            ._name = getRandomAndRemove(star.starNames, "data/starnames.txt", r)
            .xy = New xy(r.Next(1, maxX), r.Next(1, maxY))

            Dim factionPair As faction() = starmap.getFactionPair(r)
            For n = 1 To r.Next(numPlanetsMin, numPlanetsMax + 1)
                Dim planet As planet = planet.build(star, n, factionPair, r)
                ._planets.Add(planet)
            Next
        End With
        Return star
    End Function
    Private Shared starNames As New List(Of String)
    Private Const maxX As Integer = 100
    Private Const maxY As Integer = 100

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

    Private _name As String
    Friend ReadOnly Property name As String
        Get
            Return _name
        End Get
    End Property
    Private _starmap As starmap
    Friend ReadOnly Property starmap As starmap
        Get
            Return _starmap
        End Get
    End Property
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
