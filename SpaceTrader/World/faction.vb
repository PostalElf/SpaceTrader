Public Class faction
    Private _name As String
    Friend ReadOnly Property name As String
        Get
            Return _name
        End Get
    End Property

    Private planets As New List(Of planet)
    Friend Sub addPlanet(ByRef planet As planet)
        planet.faction = Me
        planets.Add(planet)
    End Sub
    Friend Sub removePlanet(ByRef planet As planet)
        planet.faction = Nothing
        planets.Remove(planet)
    End Sub

    Private _prosperityBase As Integer
    Friend ReadOnly Property prosperityBase As Integer
        Get
            Return _prosperityBase
        End Get
    End Property
    Private _militaryBase As Integer
    Friend ReadOnly Property militaryBase As Integer
        Get
            Return _militaryBase
        End Get
    End Property
End Class
