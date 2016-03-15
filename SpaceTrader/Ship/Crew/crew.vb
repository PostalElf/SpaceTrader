Public Class crew
    Friend Shared Function build(ByVal race As eRace) As crew
        Dim crew As New crew
        With crew
            ._race = race
            Select Case race
                Case eRace.Human
                    If coinFlip() = True Then .gender = True Else .gender = eGender.Female
                    If .gender = eGender.Male Then .nameFirst = getRandomAndRemove(namesFirstMale, "data/namesFirstMale.txt") Else .nameFirst = getRandomAndRemove(namesFirstFemale, "data/namesFirstFemale.txt")
                    .nameLast = getRandomAndRemove(namesLast, "data/namesLast.txt")

                Case eRace.Uplifted
                    .gender = eGender.Thing
                    .nameFirst = rng.Next(65536).ToString("X")
                    .nameLast = rng.Next(0, 16).ToString("X") & "x"
            End Select
        End With
        Return crew
    End Function
    Private Shared namesFirstMale As New List(Of String)
    Private Shared namesFirstFemale As New List(Of String)
    Private Shared namesLast As New List(Of String)

    Friend crewQuarters As hcCrewQuarters
    Private nameFirst As String
    Private nameLast As String
    Friend ReadOnly Property name As String
        Get
            Select Case race
                Case eRace.Human : Return nameFirst & " " & nameLast
                Case eRace.Uplifted : Return nameLast & nameFirst
                Case Else : Return Nothing
            End Select
        End Get
    End Property
    Private gender As eGender
    Private _race As eRace
    Friend ReadOnly Property race As eRace
        Get
            Return _race
        End Get
    End Property
End Class
