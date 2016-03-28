Public Class starmap
    Const numFactions As Integer = 5
    Const numStars As Integer = 5
    Const numPlanetsMin As Integer = 7
    Const numPlanetsMax As Integer = 10
    Friend Shared Function build(ByVal aSeed As Integer) As starmap
        Dim r As New Random(aSeed)

        Dim starmap As New starmap
        With starmap
            .seed = aSeed

            For n = 1 To numFactions
                Dim faction As faction = faction.build(r)
                .factions.Add(faction)
            Next

            For n = 1 To numStars
                Dim star As star = star.build(starmap, numPlanetsMin, numPlanetsMax, r)
                ._stars.Add(star)
            Next

            .factionPairs.Clear()
        End With
        Return starmap
    End Function
    Private factionPairs As New List(Of faction())
    Private Sub buildFactionPairs()
        For n = 0 To factions.Count - 1
            For p = 0 To factions.Count - 1
                If n.Equals(p) = False Then factionPairs.Add({factions(n), factions(p)})
            Next
        Next
    End Sub
    Friend Function getFactionPairRandom(ByRef r As Random) As faction()
        If factionPairs.Count = 0 Then buildFactionPairs()

        Dim roll As Integer = r.Next(factionPairs.Count)
        getFactionPairRandom = factionPairs(roll)
        factionPairs.RemoveAt(roll)
    End Function


    Friend Sub consoleReport(ByVal indent As Integer)
        Console.WriteLine(vbSpace(indent) & "Starmap Seed: " & seed & vbCrLf)

        For Each star In _stars
            star.consoleReport(indent)
            Console.WriteLine()
        Next
    End Sub
    Friend Sub consoleReportFactions(ByVal indent As Integer)
        Console.WriteLine(vbSpace(indent) & "Factions" & vbCrLf)
        For Each faction In factions
            faction.consoleReport(indent)
            Console.WriteLine()
        Next
    End Sub

    Private seed As Integer
    Private _stars As New List(Of star)
    Friend ReadOnly Property stars As List(Of star)
        Get
            Return _stars
        End Get
    End Property
    Friend Function getStar(ByVal starName As String) As star
        For Each star In stars
            If star.name.ToLower = starName.ToLower Then Return star
        Next
        Return Nothing
    End Function
    Friend Function getPlanet(ByVal starName As String, ByVal planetNumber As Integer)
        Dim star As star = getStar(starName)
        If star Is Nothing Then Return Nothing
        Return star.getplanet(planetNumber)
    End Function
    Friend Function getStarRandom() As star
        Return _stars(rng.Next(_stars.Count))
    End Function
    Friend Function getPlanetRandom()
        Dim star As star = getStarRandom()
        Return star.getPlanetRandom
    End Function

    Private factions As New List(Of faction)

    Friend Sub tick()
        For Each star In _stars
            star.tick()
        Next
    End Sub
End Class
