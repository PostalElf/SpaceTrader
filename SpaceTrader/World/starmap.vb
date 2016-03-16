Public Class starmap
    Friend Shared Function build(ByVal numStars As Integer, ByVal numPlanets As Integer, ByVal aSeed As Integer) As starmap
        Dim r As New Random(aSeed)
        Dim starmap As New starmap
        With starmap
            .seed = aSeed
            For n = 1 To numStars
                Dim star As star = star.build(starmap, numPlanets, r)
                ._stars.Add(star)
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

    Private seed As Integer
    Private _stars As New List(Of star)
    Friend ReadOnly Property stars As List(Of star)
        Get
            Return _stars
        End Get
    End Property
    Friend Function getStarRandom() As star
        Return _stars(rng.Next(_stars.Count))
    End Function
    Friend Function getPlanetRandom()
        Dim star As star = getStarRandom()
        Return star.getPlanetRandom
    End Function

    Friend Sub tick()
        For Each star In _stars
            star.tick()
        Next
    End Sub
End Class
