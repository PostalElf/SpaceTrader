Public Class starmap
    Friend Shared Function build(ByVal numStars As Integer, ByVal numPlanets As Integer, ByVal aSeed As Integer) As starmap
        Dim r As New Random(aSeed)
        Dim starmap As New starmap
        With starmap
            .seed = aSeed
            For n = 1 To numStars
                Dim star As star = star.build(starmap, numPlanets, r)
                .stars.Add(star)
            Next
        End With
        Return starmap
    End Function

    Friend Sub consoleReport(ByVal indent As Integer)
        Console.WriteLine(vbSpace(indent) & "Starmap Seed: " & seed & vbCrLf)

        For Each star In stars
            star.consoleReport(indent)
            Console.WriteLine()
        Next
    End Sub

    Private seed As Integer
    Private stars As New List(Of star)
End Class
