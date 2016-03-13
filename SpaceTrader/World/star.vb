Public Class star
    Friend Shared Function build(ByRef starmap As starmap, ByVal numPlanets As Integer, ByRef r As Random) As star
        Dim star As New star
        With star
            ._starmap = starmap
            ._name = getRandomAndRemove(star.starNames, "data/starnames.txt", r)

            numPlanets += rng.Next(4)
            For n = 1 To numPlanets
                Dim planet As planet = planet.build(star, n, r)
                .planets.Add(planet)
            Next
        End With
        Return star
    End Function
    Private Shared starNames As New List(Of String)

    Friend Sub consoleReport(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)

        Console.WriteLine(ind & name)
        For Each planet In planets
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
    Private planets As New List(Of planet)
End Class
