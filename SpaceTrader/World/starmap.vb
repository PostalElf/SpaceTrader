Public Class starmap
    Const numFactions As Integer = 5
    Const numStars As Integer = 5
    Const numPlanetsMin As Integer = 7
    Const numPlanetsMax As Integer = 10
    Friend Shared Function build(ByVal aSeed As Integer) As starmap

        Dim starmap As New starmap
        With starmap
            .seed = aSeed
            .random = New Random(aSeed)

            For n = 1 To numFactions
                Dim faction As faction = faction.build(.random)
                .factions.Add(faction)
            Next

            For n = 1 To numStars
                Dim star As star = star.build(starmap, numPlanetsMin, numPlanetsMax, .random)
                ._stars.Add(star)
            Next

            For n = .factions.Count - 1 To 0 Step -1
                Dim faction As faction = .factions(n)
                If faction.planets.Count = 0 Then .factions.Remove(faction)
            Next
        End With
        Return starmap
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
    Friend random As Random
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
    Friend Function getFactionPair(ByRef r As Random) As faction()
        Dim total() As faction = {factions(0), factions(1)}
        Dim totalMax() As Integer = {factions(0).planets.Count, factions(1).planets.Count}

        '0 is second lowest, 1 is lowest
        For Each faction In factions
            If faction.planets.Count < totalMax(0) Then
                If faction.planets.Count < totalMax(1) Then
                    'is lowest; move lowest (1) to second lowest (0) and replace (1) with faction
                    total(1) = total(0)
                    totalMax(1) = totalMax(0)
                    total(1) = faction
                    totalMax(1) = faction.planets.Count
                Else
                    'is second lowest; replace second lowest (0) with faction
                    total(0) = faction
                    totalMax(0) = faction.planets.Count
                End If
            End If
        Next

        Return total
    End Function

    Friend Sub tick()
        For Each star In _stars
            star.tick()
        Next
    End Sub
End Class
